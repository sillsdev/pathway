// --------------------------------------------------------------------------------------------
// <copyright file="PathwayBTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Erik Brommers, reworked from XhtmlExportTest.cs by Greg Trihus</author>
// Last reviewed:
//
// <remarks>
// PathwayB.exe command line tests
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Text;
using System.IO;
using NUnit.Framework;
using SIL.Tool;

namespace Test
{
    public class PathwayBTest
    {
        #region Setup
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;

        public enum InputFormat
        {
            XHTML,
            USFM,
            USX
        }

        /// <summary>
        /// setup Input, Expected, and Output paths relative to location of program
        /// </summary>
        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../../DistFiles");
            Common.SupportFolder = "";
            Common.ProgBase = Common.ProgInstall;
            Common.Testing = true;
            var testPath = PathPart.Bin(Environment.CurrentDirectory, "/PathwayB/TestFiles");
            _inputPath = Common.PathCombine(testPath, "Input");
            _outputPath = Common.PathCombine(testPath, "Output");
            _expectedPath = Common.PathCombine(testPath, "Expected");
            Directory.CreateDirectory(_outputPath);
        }
        #endregion Setup

        /// <summary>
        /// Runs PathwayB on the data and applies the back end
        /// </summary>
        private string RunPathwayB(InputFormat inFormat, string files, string project, string layout, string inputType, string backend, string message)
        {
            var arg = new StringBuilder();
            if (project.Length < 1 || layout.Length < 1 || inputType.Length < 1 || backend.Length < 1)
            {
                // missing some args -- call usage on PathwayB
                arg.Append("-h ");
            }
            switch (inFormat)
            {
                case InputFormat.XHTML:
                    // "-d .../TestFiles/Output"
                    arg.Append("-d \"");
                    arg.Append(_outputPath);
                    // "-if xhtml"
                    arg.Append("\" -if xhtml");
                    // "-c .../TestFiles/Output/<layout>.css"
                    arg.Append(" -c \"");
                    arg.Append(_outputPath);
                    arg.Append(Path.DirectorySeparatorChar);
                    arg.Append(layout);
                    arg.Append(".css\"");
                    arg.Append(" ");
                    break;
                case InputFormat.USFM:
                    // "-d .../TestFiles/Output"
                    arg.Append("-d \"");
                    arg.Append(_outputPath);
                    // "-if usfm"
                    arg.Append("\" -if usfm");
                    arg.Append(" ");
                    break;
                case InputFormat.USX:
                    // "-u .../TestFiles/Output"
                    arg.Append("-d \"");
                    arg.Append(_outputPath);
                    // "-if usx"
                    arg.Append("\" -if usx");
                    arg.Append(" ");
                    break;
                default:
                    break;
            }
            arg.Append("-f ");
            arg.Append(files);
            arg.Append(" -t \"");
            arg.Append(backend);
            arg.Append("\" -i \"");
            arg.Append(inputType);
            arg.Append("\" -n \"");
            arg.Append(project);
            arg.Append("\"");
            Debug.WriteLine("Calling PathwayB.exe " + arg.ToString());
            var sb = new StringBuilder();
            string errorMessage = "", outputMessage = "";
			string pathwayDirectory = Common.AssemblyPath;
			string pathwayExportDir = Path.Combine(pathwayDirectory, "Export");

			if (!Directory.Exists(pathwayExportDir))
			{
				pathwayExportDir = Path.GetDirectoryName(Common.AssemblyPath);
				pathwayExportDir = Path.Combine(pathwayExportDir, "Export");
			}

            const int timeout = 60;
            try
            {
                var proc = new Process
                {
                    StartInfo =
                    {
						FileName = Common.PathCombine(pathwayExportDir, "PathwayB.exe"),
                        Arguments = arg.ToString(),
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false
                    }
                };

                proc.Start();

                proc.WaitForExit
                    (
                        timeout * 100 * 60
                    );

                errorMessage = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                outputMessage = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
            }
            catch (Exception e)
            {
                sb.AppendLine("Exception encountered in RunPathwayB:");
                sb.AppendLine();
                sb.AppendLine(e.Message);
                if (e.InnerException != null)
                {
                    sb.AppendLine("Inner Exception:");
                    sb.AppendLine(e.InnerException.ToString());
                }
                errorMessage = sb.ToString();
            }
            if (errorMessage.Length > 0) Debug.WriteLine(errorMessage);
            if (outputMessage.Length > 0) Debug.WriteLine(outputMessage);
            // check the results
            switch (backend)
            {
                case "E-Book (Epub2 and Epub3)":
                    epubCheck(layout, message);
                    break;
                case "OpenOffice/LibreOffice":
                    OdtCheck(project, message);
                    break;
                case "InDesign":
                    IdmlCheck(project, message);
                    break;
                default:
                    break;
            }
            if (errorMessage.Length > 0) return errorMessage;
            if (outputMessage.Length > 0) return outputMessage;
            return null;
        }

        /// <summary>
        /// Copy an entire directory.
        /// Code from http://msdn.microsoft.com/en-us/library/bb762914.aspx.
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        /// <param name="copySubDirs"></param>
        private static void DirectoryCopy(
                string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (destDirName.Contains(".svn"))
            {
                return;
            }
            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Common.PathCombine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Common.PathCombine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        /// <summary>
        /// Verifies that the epub we created validates
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="message"></param>
        private void epubCheck(string layout, string message)
        {
            Test.epubConvert.ExportepubTest.IsValid(Common.PathCombine(_outputPath, layout + ".epub"), message);
        }

        /// <summary>
        /// Verifies that the .idml file we created matches our expected version
        /// </summary>
        /// <param name="project"></param>
        /// <param name="message"></param>
        private void IdmlCheck(string project, string message)
        {
            var expectedPath = Common.PathCombine(_expectedPath, project);
            IdmlTest.AreEqual(Common.PathCombine(expectedPath, project + ".idml"), Common.PathCombine(_outputPath, project + ".idml"), message);
        }

        /// <summary>
        /// Verifies that the .odt we created matches our expected version
        /// </summary>
        /// <param name="project"></param>
        /// <param name="message"></param>
        private void OdtCheck(string project, string message)
        {
            var expectedPath = Common.PathCombine(_expectedPath, project);
            OdtTest.AreEqual(Common.PathCombine(expectedPath, project + ".odt"), Common.PathCombine(_outputPath, project + ".odt"), message);
        }

        /// <summary>
        /// Runs PathwayB.exe with the -h parameter, to get the usage info back
        /// </summary>
        [Test]
        [Category("LongTest")]
        //[Category("SkipOnTeamCity")]
        public void UsageTest()
        {
            // Because we haven't supplied the parameters, we should get the usage string back from PathwayB
            var result = RunPathwayB(InputFormat.XHTML, "", "", "", "", "", "Usage test");
            Assert.IsTrue(result.Contains("Usage"), result);
        }

        [Test]
        [Category("LongTest")]
        //[Category("SkipOnTeamCity")]
        public void XhtmlDictionaryMainAndRevTest()
        {
            // clean out old files
            foreach (var file in Directory.GetFiles(_outputPath))
            {
                File.Delete(file);
            }
            var settingsFolder = Common.PathCombine(Common.GetAllUserPath(), "Pathway");
            if (Directory.Exists(settingsFolder))
            {
                Directory.Delete(settingsFolder,true);
            }
            // make a copy of the xhtml and CSS files

            _outputPath = Common.PathCombine(_outputPath, "DictionaryTest");
            Directory.CreateDirectory(_outputPath);

            File.Copy(Common.PathCombine(Common.PathCombine(_inputPath, "Sena 3-01"), "main.xhtml"), Common.PathCombine(_outputPath, "main.xhtml"), true);
            File.Copy(Common.PathCombine(Common.PathCombine(_inputPath, "Sena 3-01"), "FlexRev.xhtml"), Common.PathCombine(_outputPath, "FlexRev.xhtml"), true);
            File.Copy(Common.PathCombine(Common.PathCombine(_inputPath, "Sena 3-01"), "main.css"), Common.PathCombine(_outputPath, "main.css"), true);
            // run the test

            RunPathwayB(InputFormat.XHTML, "\"main.xhtml\", \"FlexRev.xhtml\"", "Sena 3-01", "main", "Dictionary", "E-Book (Epub2 and Epub3)", "MainAndRevTest");
        }

        /// <summary>
        /// EDB 11/1/2011 -
        /// Note: The validation on this test is currently failing due to a bug in TE (FWR 2550 - mismatched cases in hyperlink IDs). There is
        /// a workaround to this for normal TE output (see GetXsltFile() in Exportepub.cs), but it relies on the calling app and whether the export
        /// is running in "testing" mode (i.e., headless output). I haven't found a way to distinguish between scripture from TE and Paratext, so
        /// I've let the logic in GetXsltFile() work properly for Paratext output -- USFM from the command line should be our primary use case.
        /// Export from TE itself is not affected by this code.
        /// </summary>
        [Test]
        [Category("ShortTest")]
        //[Category("SkipOnTeamCity")]
        public void XhtmlScriptureTest()
        {
            // clean out old files
            foreach (var file in Directory.GetFiles(_outputPath))
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            // make a copy of the xhtml and CSS files
            File.Copy(Common.PathCombine(Common.PathCombine(_inputPath, "Sena 3-01"), "Sena 3-01.xhtml"), Common.PathCombine(_outputPath, "Scripture Draft.xhtml"), true);
            File.Copy(Common.PathCombine(Common.PathCombine(_inputPath, "Sena 3-01"), "Sena 3-01.css"), Common.PathCombine(_outputPath, "Scripture Draft.css"), true);
            // run the test
          // TODO  RunPathwayB(InputFormat.XHTML, "\"Scripture Draft.xhtml\"", "Sena 3-01", "Scripture Draft", "Scripture", "E-Book (Epub2 and Epub3)", "xhtmlTest");

        }

        /// <summary>
        /// Tests USFM conversion from the command line. Currently the PathwayB module uses reflection to call into ParatextShared.dll. For this
        /// to pass, you need to have ParatextShared.dll and NetLoc.dll in your Pathway installation directory.
        /// </summary>
        [Test]
        [Category("LongTest")]
        //[Category("SkipOnTeamCity")]
        public void UsfmTest()
        {
            // clean out old files
            foreach (var file in Directory.GetFiles(_outputPath))
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            if (Directory.Exists(Common.PathCombine(_outputPath, "gather")))
            {
                // delete the gather subdirectory files as well
                foreach (var file in Directory.GetFiles(Common.PathCombine(_outputPath, "gather")))
                {
                    File.Delete(file);
                }
            }
            // Copy the files
            var projPath = Common.PathCombine(_inputPath, "KFY");
            //if (Directory.Exists(Common.PathCombine(projPath, "gather")))
            DirectoryCopy(projPath, _outputPath, true);
            // Copy Settings
            var targetSettings = Common.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pathway");
            if (Directory.Exists(targetSettings))
            {
                Directory.Delete(targetSettings, true);
            }
            DirectoryCopy(Common.PathCombine(_inputPath, "Pathway"), targetSettings, true);
            // run the test
            RunPathwayB(InputFormat.USFM, "*", "KFY", "KFY", "Scripture", "xhtml", "usfmTest");
        }

		/// <summary>
        /// Tests USFM conversion from the command line. Currently the PathwayB module uses reflection to call into ParatextShared.dll. For this
        /// to pass, you need to have ParatextShared.dll and NetLoc.dll in your Pathway installation directory.
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void ScriptureEpubExportTest()
        {
            // clean out old files
            foreach (var file in Directory.GetFiles(_outputPath))
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            if (Directory.Exists(Common.PathCombine(_outputPath, "gather")))
            {
                // delete the gather subdirectory files as well
                foreach (var file in Directory.GetFiles(Common.PathCombine(_outputPath, "gather")))
                {
                    File.Delete(file);
                }
            }
            // Copy the files
			var projPath = Common.PathCombine(_inputPath, "aaiNT");
            //if (Directory.Exists(Common.PathCombine(projPath, "gather")))
            DirectoryCopy(projPath, _outputPath, true);
            // Copy Settings
            var targetSettings = Common.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pathway");
            if (Directory.Exists(targetSettings))
            {
                Directory.Delete(targetSettings, true);
            }
            DirectoryCopy(Common.PathCombine(_inputPath, "Pathway"), targetSettings, true);
            // run the test
			RunPathwayB(InputFormat.USFM, "*", "aaiNT", "aaiNT", "Scripture", "E-Book (Epub2 and Epub3)", "usfmTest");
        }

        [Test]
        [Category("ShortTest")]
        //[Category("SkipOnTeamCity")]
        public void UsxTest()
        {
            // clean out old files
            foreach (var file in Directory.GetFiles(_outputPath))
            {
                File.Delete(file);
            }
            if (Directory.Exists(Common.PathCombine(_outputPath, "gather")))
            {
                // delete the gather subdirectory files as well
                foreach (var file in Directory.GetFiles(Common.PathCombine(_outputPath, "gather")))
                {
                    File.Delete(file);
                }
            }
            // TODO: implement test
        }

		/// <summary>
		/// Tests USFM conversion from the command line. Currently the PathwayB module uses reflection to call into ParatextShared.dll. For this
		/// to pass, you need to have ParatextShared.dll and NetLoc.dll in your Pathway installation directory.
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void KFYScriptureEpubExportTest()
		{
			// clean out old files
			foreach (var file in Directory.GetFiles(_outputPath))
			{
				if (File.Exists(file))
					File.Delete(file);
			}
			if (Directory.Exists(Common.PathCombine(_outputPath, "gather")))
			{
				// delete the gather subdirectory files as well
				foreach (var file in Directory.GetFiles(Common.PathCombine(_outputPath, "gather")))
				{
					File.Delete(file);
				}
			}
			// Copy the files
			var projPath = Common.PathCombine(_inputPath, "KFY");
			//if (Directory.Exists(Common.PathCombine(projPath, "gather")))
			DirectoryCopy(projPath, _outputPath, true);
			// Copy Settings
			var targetSettings = Common.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pathway");
			if (Directory.Exists(targetSettings))
			{
				Directory.Delete(targetSettings, true);
			}
			DirectoryCopy(Common.PathCombine(_inputPath, "Pathway"), targetSettings, true);
			// run the test
			RunPathwayB(InputFormat.USFM, "*", "KFY", "KFY", "Scripture", "E-Book (Epub2 and Epub3)", "PathwayBExport");
		}
    }
}
