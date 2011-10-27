// --------------------------------------------------------------------------------------------
// <copyright file="PathwayBTest.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
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
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../PsSupport");
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
        private string RunPathwayB(InputFormat inFormat, string project, string layout, string inputType, string backend)
        {
            return RunPathwayB(inFormat, project, layout, inputType, backend, "");
        }

        /// <summary>
        /// Runs PathwayB on the data and applies the back end
        /// </summary>
        private string RunPathwayB(InputFormat inFormat, string project, string layout, string inputType, string backend, string message)
        {
            const bool overwrite = true;
            var arg = new StringBuilder();
            var inPath = Path.Combine(_inputPath, project);
            if (project.Length < 1 || layout.Length < 1 || inputType.Length < 1 || backend.Length < 1)
            {
                // missing some args -- call usage on PathwayB
                arg.Append("-h ");
            }
            switch (inFormat)
            {
                case InputFormat.XHTML:
                    // "-x .../TestFiles/Output/<layout>.xhtml"
                    arg.Append("-x \"");
                    arg.Append(_outputPath);
                    arg.Append(Path.DirectorySeparatorChar);
                    arg.Append(layout);
                    arg.Append(".xhtml\"");
                    // "-c .../TestFiles/Output/<layout>.css"
                    arg.Append(" -c \"");
                    arg.Append(_outputPath);
                    arg.Append(Path.DirectorySeparatorChar);
                    arg.Append(layout);
                    arg.Append(".css\"");
                    // make a copy of the xhtml and CSS files
                    if (File.Exists(Path.Combine(inPath, project + ".xhtml")))
                    {
                        File.Copy(Path.Combine(inPath, project + ".xhtml"), Path.Combine(_outputPath, layout + ".xhtml"),
                                  overwrite);
                    }
                    if (File.Exists(Path.Combine(inPath, project + ".css")))
                    {
                        File.Copy(Path.Combine(inPath, project + ".css"), Path.Combine(_outputPath, layout + ".css"),
                                  overwrite);
                    }
                    arg.Append(" ");
                    break;
                case InputFormat.USFM:
                    // "-d .../TestFiles/Output/<project>"
                    arg.Append("-d \"");
                    arg.Append(_outputPath);
                    // "-m *"
                    arg.Append("\" -m *"); // take all the files in the project directory
                    File.Copy(Path.Combine(inPath, "*.*"), Path.Combine(_outputPath, "*.*"), overwrite);
                    arg.Append(" ");
                    break;
                case InputFormat.USX:
                    // "-u .../TestFiles/Output/<project>"
                    arg.Append("-u \"");
                    arg.Append(_outputPath);
                    // "-m *"
                    arg.Append("\" -m *"); // take all the files in the project directory
                    File.Copy(Path.Combine(inPath, "*.*"), Path.Combine(_outputPath, "*.*"), overwrite);
                    arg.Append(" ");
                    break;
                default:
                    break;
            }
            arg.Append("-t \"");
            arg.Append(backend);
            arg.Append("\" -i \"");
            arg.Append(inputType);
            arg.Append("\" -n \"");
            arg.Append(project);
            arg.Append("\"");
            Debug.WriteLine("Calling PathwayB.exe " + arg.ToString());
            var sb = new StringBuilder();
            string errorMessage = "", outputMessage = "";
            const int timeout = 60;
            try
            {
                var proc = new Process
                {
                    StartInfo =
                    {
                        FileName = Common.PathCombine(PathwayPath.GetPathwayDir(), "PathwayB.exe"),
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
            // check the results
            switch (backend)
            {
                case "E-Book (.epub)":
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
        /// Verifies that the epub we created validates
        /// </summary>
        /// <param name="layout"></param>
        /// <param name="message"></param>
        private void epubCheck(string layout, string message)
        {
            Test.epubConvert.ExportepubTest.IsValid(Path.Combine(_outputPath, layout + ".epub"), message);
        }

        /// <summary>
        /// Verifies that the .idml file we created matches our expected version
        /// </summary>
        /// <param name="project"></param>
        /// <param name="message"></param>
        private void IdmlCheck(string project, string message)
        {
            var expectedPath = Path.Combine(_expectedPath, project);
            IdmlTest.AreEqual(Path.Combine(expectedPath, project + ".idml"), Path.Combine(_outputPath, project + ".idml"), message);
        }

        /// <summary>
        /// Verifies that the .odt we created matches our expected version
        /// </summary>
        /// <param name="project"></param>
        /// <param name="message"></param>
        private void OdtCheck(string project, string message)
        {
            var expectedPath = Path.Combine(_expectedPath, project);
            OdtTest.AreEqual(Path.Combine(expectedPath, project + ".odt"), Path.Combine(_outputPath, project + ".odt"), message);
        }

        /// <summary>
        /// Runs PathwayB.exe with the -h parameter, to get the usage info back
        /// </summary>
        [Test]
        public void UsageTest()
        {
            // Because we haven't supplied the parameters, we should get the usage string back from PathwayB
            var result = RunPathwayB(InputFormat.XHTML, "", "", "", "", "Usage test");
            Assert.IsTrue(result.Contains("Usage"), result);
        }

        [Test]
        public void xhtmlTest()
        {
            RunPathwayB(InputFormat.XHTML, "Sena 3-01", "Scripture Draft", "Scripture", "E-Book (.epub)");
        }

    }
}
