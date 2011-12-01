// --------------------------------------------------------------------------------------------
// <copyright file="TePsExportTest.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Test methods of FlexDePlugin
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

#endregion Using

namespace Test.PsExport
{
    /// <summary>
    /// Test methods of FlexDePlugin
    /// </summary>
    [TestFixture]
    [Category("BatchTest")]
    public class PsExportTest : SIL.PublishingSolution.PsExport
    {
        #region Setup
        /// <summary>holds path to input folder for all tests</summary>
        private static string _inputBasePath = string.Empty;
        /// <summary>holds path to expected results folder for all tests</summary>
        private static string _expectBasePath = string.Empty;
        /// <summary>holds path to output folder for all tests</summary>
        private static string _outputBasePath = string.Empty;

        /// <summary>
        /// setup Input, Expected, and Output paths relative to location of program
        /// </summary>
        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.Testing = true;
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/PsExport/TestFiles");
            _inputBasePath = Common.PathCombine(testPath, "Input");
            _expectBasePath = Common.PathCombine(testPath, "Expect");
            _outputBasePath = Common.PathCombine(testPath, "Output");
            Common.DeleteDirectory(_outputBasePath);
            Directory.CreateDirectory(_outputBasePath);
            // Set application base for test
            DoBatch("ConfigurationTool", "postBuild.bat", "Debug");
            Common.ProgInstall = Environment.CurrentDirectory.Replace("Test", "ConfigurationTool");
            FolderTree.Copy(Common.PathCombine(testPath, "../../../PsSupport/OfficeFiles"),Path.Combine(Common.ProgInstall,"OfficeFiles"));
            Backend.Load(Common.ProgInstall);
        }

        private static string _BasePath = string.Empty;
        private static string _ConfigPath = string.Empty;

        public static void DoBatch(string project, string process, string config)
        {
            SetBaseAndConfig();
            var folder = _BasePath + project + _ConfigPath;
            var processPath = Common.PathCombine(_BasePath + project, process);
            //MessageBox.Show(folder);
            SubProcess.Run(folder, processPath, config, true);
        }

        private static void SetBaseAndConfig()
        {
            if (_BasePath != string.Empty) return;
            var m = Regex.Match(Environment.CurrentDirectory, "Test");
            Debug.Assert(m.Success);
            _BasePath = Environment.CurrentDirectory.Substring(0, m.Index);
            _ConfigPath = Environment.CurrentDirectory.Substring(m.Index + m.Length);
        }

        /// <summary>
        /// pretend we don't know the type of input after each test
        /// </summary>
        [TearDown]
        protected void TearDown()
        {
            Param.Value[Param.InputType] = "";
        }
        #endregion Setup

        #region Internal
        #region TestPath
        /// <summary>holds path to input folder for all tests</summary>
        private static string _inputTestPath = string.Empty;
        /// <summary>holds path to expected results folder for all tests</summary>
        private static string _expectTestPath = string.Empty;
        /// <summary>holds path to output folder for all tests</summary>
        private static string _outputTestPath = string.Empty;

        private static void TestPathSetup(string testName)
        {
            _inputTestPath = Common.PathCombine(_inputBasePath, testName);
            _expectTestPath = Common.PathCombine(_expectBasePath, testName);
            _outputTestPath = Common.PathCombine(_outputBasePath, testName);
        }

        private static string FileInput(string fileName)
        {
            return Common.PathCombine(_inputTestPath, fileName);
        }

        private static string FileOutput(string fileName)
        {
            return Common.PathCombine(_outputTestPath, fileName);
        }

        private static string FileExpect(string fileName)
        {
            return Common.PathCombine(_expectTestPath, fileName);
        }
        #endregion TestPath

        #region AcquireUserSettings Common
        /// <summary>
        /// Tests AcquireUserSettings
        /// </summary>
        /// <param name="testName">Test name (and fold name of test).</param>
        /// <param name="mainName">input xhtml name in folder</param>
        /// <param name="cssName">css name in folder</param>
        /// <param name="msg">message identifying test if mismatch (failure).</param>
        protected void AcquireUserSettingsTest(string testName, string mainName, string cssName, string msg)
        {
            CommonOutputSetup(testName);
            Param.SetValue(Param.InputType, "Dictionary");
            Param.LoadSettings();

            File.Copy(FileInput(mainName), FileOutput(mainName));
            JobCopy(cssName);

            var tpe = new SIL.PublishingSolution.PsExport() { DataType = "Scripture" };
            var mainFullName = FileOutput(mainName);
            string job = tpe.GetFluffedCssFullName(mainFullName, _outputTestPath, FileOutput(cssName));
            TextFileAssert.AreEqual(FileExpect(Path.GetFileName(job)), job, msg);
        }

        #endregion AcquireUserSettings Common

        #region SeExport Common
        /// <summary>
        /// Test DeExport function.
        /// </summary>
        /// <param name="testName">test name (also folder name of test)</param>
        /// <param name="mainXhtml">input xhtml name in folder</param>
        /// <param name="jobFileName">job file in folder</param>
        /// <param name="target">desired destination</param>
        /// <param name="msg">message to identify test if error occurs</param>
        protected void SeExportTest(string testName, string mainXhtml, string jobFileName, string target, string msg)
        {
            CommonOutputSetup(testName);
            File.Copy(FileInput(mainXhtml), FileOutput(mainXhtml), true);
            string cssPath = Path.GetFileNameWithoutExtension(mainXhtml);
            File.Copy(FileInput(cssPath) + ".css", FileOutput(cssPath) +".css", true);
            JobCopy(jobFileName);
            FolderTree.Copy(FileInput("Pictures"), FileOutput("Pictures"));

            var tpe = new SIL.PublishingSolution.PsExport { DataType = "Scripture", Destination = target };
            tpe.SeExport(mainXhtml, jobFileName, _outputTestPath);
            switch (target)
            {
                case "OpenOffice/LibreOffice":
                    OdtTest.AreEqual(_expectTestPath, _outputTestPath, msg);
                    break;
                case "Pdf (using Prince)":
                    var outName = Path.GetFileNameWithoutExtension(mainXhtml) + ".pdf";
                    Assert.True(File.Exists(FileOutput(outName)), msg);
                    //FileAssert.AreEqual(FileExpect(outName), FileOutput(outName), msg);
                    break;
                default:
                    Assert.Fail(msg + " unkown destination");
                    break;
            }
        }
        #endregion SeExport Common

        #region PsExport Common
        /// <summary>
        /// Test PsExport function.
        /// </summary>
        /// <param name="testName">test name (also folder name of test)</param>
        /// <param name="mainXhtml">input xhtml name in folder</param>
        /// <param name="target">desired destination</param>
        /// <param name="msg">message to identify test if error occurs</param>
        protected void ExportTest(string testName, string mainXhtml, string dataType, string target, string msg)
        {
            CommonOutputSetup(testName);
            CopyExistingFile(mainXhtml);
            var cssName = Path.GetFileNameWithoutExtension(mainXhtml) + ".css";
            CopyExistingFile(cssName);
            if (Directory.Exists(FileInput("Pictures")))
                FolderTree.Copy(FileInput("Pictures"), FileOutput("Pictures"));
            CopyExistingFile("FlexRev.xhtml");
            CopyExistingFile("FlexRev.css");

            var tpe = new SIL.PublishingSolution.PsExport { Destination = target, DataType = dataType};
            if (testName.ToLower() == "t5" || testName.ToLower() == "t8")
            {
                tpe._fromNUnit = true;
            }
            tpe.Export(FileOutput(mainXhtml));
            switch (target)
            {
                case "OpenOffice":
                    OdtTest.AreEqual(_expectTestPath, _outputTestPath, msg);
                    break;
                case "Pdf":
                    var outName = Path.GetFileNameWithoutExtension(mainXhtml) + ".pdf";
                    Assert.True(File.Exists(FileOutput(outName)), msg);
                    //FileAssert.AreEqual(FileExpect(outName), FileOutput(outName), msg);
                    break;
                default:
                    Assert.Fail(msg + " unkown destination");
                    break;
            }
        }
        #endregion PsExport Common

        #region Internal private methods
        /// <summary>
        /// erase previous output, load localization files
        /// </summary>
        private static void CommonOutputSetup(string testName)
        {
            Common.PublishingSolutionsEnvironmentReset();
            TestPathSetup(testName);

            var di = new DirectoryInfo(_outputTestPath);
            //if (di.Exists)
            //    di.Delete(true);
            Common.DeleteDirectory(_outputTestPath);
            di.Create();

            Common.SupportFolder = "";
			Common.ProgBase = Common.GetPSApplicationPath();
            Param.LoadSettings();
        }
        
        /// <summary>
        /// Copies a file if it exists from the input test path to the output
        /// </summary>
        /// <param name="fileName">file to be copied if it exists</param>
        private static void CopyExistingFile(string fileName)
        {
            if (File.Exists(FileInput(fileName)))
                File.Copy(FileInput(fileName), FileOutput(fileName), true);
        }

        /// <summary>
        /// Copy all referenced css files in input folder
        /// </summary>
        /// <param name="jobFileName">Cascading style sheet file</param>
        private static void JobCopy(string jobFileName)
        {
            string jobFullName = FileInput(jobFileName);
            File.Copy(jobFullName, FileOutput(jobFileName), true);
            var sr = new StreamReader(jobFullName);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line.Length == 0 || line.Substring(0, 1) == "/")
                    continue;
                Match m = Regex.Match(line, "@import \"(.*)\";", RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    JobCopy(m.Groups[1].Value);
                    continue;
                }
                break;
            }
            sr.Close();
            return;
        }
        #endregion Internal private methods
        #endregion Internal

        #region T1
        /// <summary>
        /// Simple test where no changes are made to the settings.
        /// </summary>
        [Test]
        public void AcquireUserSettingsT1()
        {
            AcquireUserSettingsTest("T1", "1pe.xhtml", "Layout_02.css", "T1: Style sheet default preparation");
            //default action is to set style sheet based on last task selected. Layout_02.css is not used.
        }
        #endregion T1

        #region T2
        /// <summary>
        /// Test ODT export
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void SeExportT2()
        {
            SeExportTest("T2", "1pe.xhtml", "Layout_02.css", "OpenOffice/LibreOffice",  "T2: ODT Export Test");
        }
        #endregion T2

        #region T3
        /// <summary>
        /// Test PDF export
        /// </summary>
        [Test]
        [Ignore]
        public void SeExportT3()
        {
            SeExportTest("T3", "1pe.xhtml", "Layout_02.css", "Pdf (using Prince)", "T3: PDF Export Test");
        }
        #endregion T3

        #region T4
        /// <summary>
        /// Test TE Export test
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void PsExportT4()
        {
            ExportTest("T4", "1pe.xhtml", "Scripture", "OpenOffice", "T4: TE ODT Export Test");
        }
        #endregion T4

        #region T5
        /// <summary>
        /// Test Flex Export test
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void PsExportT5()
        {

            ExportTest("T5", "main.xhtml", "Dictionary", "OpenOffice", "T5: Flex ODT Export Test");
        }
        #endregion T5


        /// <summary>
        /// Test Flex Export test
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void PsExportT6()
        {
            ExportTest("T6", "main.xhtml", "Dictionary", "OpenOffice", "T6: Flex ODT Export Test");
        }

        /// <summary>
        /// Test Flex Export test
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void PsExportT7()
        {
            ExportTest("T7", "FlexRev.xhtml", "Dictionary", "OpenOffice", "T7: Flex ODT Export Test");
        }

        /// <summary>
        /// Test Flex Export test - Page A5 Test
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void PsExportT8()
        {
            ExportTest("T8", "main.xhtml", "Dictionary", "OpenOffice", "T8: Flex ODT Export Test");
        }

        #region T9
        /// <summary>
        /// Test Flex Export test
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void PsExportT9()
        {
            //OnePerBook.xhtml
            ExportTest("T9", "main.xhtml", "Scripture", "OpenOffice", "T9: Flex ODT Export Test");
        }
        #endregion T9

        #region T10
        /// <summary>
        /// Test Flex Export test
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void PsExportT10()
        {
            //OnePerLetter.xhtml
            ExportTest("T10", "main.xhtml", "Dictionary", "OpenOffice", "T10: Flex ODT Export Test");
        }
        #endregion T9


        #region T6
        #if OutOfDateTests
        [Test]
        public void AcquireUserSettingsT6()
        {
            AcquireUserSettingsTest("T6", "main-util1.css", "T6: Url blank", true);
        }
        #endif
        #endregion T6

        #region T11
        /// <summary>
        /// Test ODT export
        /// </summary>
        [Test]
        [Ignore]
        public void PsExportT11()
        {
            ExportTest("T11", "main.xhtml", "Dictionary", "OpenOffice/LibreOffice", "T11: Flex ODT Export Test");
            //DeExportTest("T11", "main.xhtml", "A4Setting.css", "LibreOffice", "T11: ODT Export Test");
        }
        #endregion T11
    }
}