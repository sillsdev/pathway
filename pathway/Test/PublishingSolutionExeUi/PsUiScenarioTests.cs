// --------------------------------------------------------------------------------------------
// <copyright file="PsUiScenarioTests.cs" from='2009' to='2009' company='SIL International'>
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
// For DE scripts.
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using GuiTestDriver;
using NUnit.Framework;
using SIL.Tool;

namespace Test.PublishingSolutionExeUi
{
    /// <summary>
    /// For DE scripts.
    /// </summary>
    [TestFixture]
    [Category("UserInterface")]
    [Ignore]
    public class PsUiScenarioTests
    {
        #region Setup
        /// <summary>
        /// test root
        /// </summary>
        RunTest _runTest;

        /// <summary>
        /// Base path so user can install these files anywhere on their hard drive
        /// </summary>
        public string TestSourcePath
        {
            get
            {
                return Common.DirectoryPathReplace(Environment.CurrentDirectory + "/../../PublishingSolutionExeUi/");
            }
        }

        /// <summary>
        /// setup test root
        /// </summary>
        [TestFixtureSetUp]
        protected void SetUp()
        {
            var format = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><include><var id=\"testPath\" set=\"{0}TestFiles\"/></include>";
            var sw = new StreamWriter(Common.DirectoryPathReplace(TestSourcePath + "Scripts/DE/testPath.xml"));
            sw.Write(string.Format(format, TestSourcePath));
            sw.Close();
            var outInfo = new DirectoryInfo(Common.DirectoryPathReplace(TestSourcePath + "TestFiles/Output"));
            var fileList = outInfo.GetDirectories();
            foreach (var folderName in fileList)
            {
                if (folderName.Name.Substring(0, 1) != ".")
                    Directory.Delete(folderName.FullName, true);
            }
            _runTest = new RunTest("DE");
        }

        private void RunTestFromFileWithProject(string test)
        {
            FolderTree.Copy(Common.DirectoryPathReplace(TestSourcePath + "TestFiles/Input/OpenDic"), Common.DirectoryPathReplace(TestSourcePath + "TestFiles/Output/OpenDic"));
            FolderTree.Copy(Common.DirectoryPathReplace(TestSourcePath + "TestFiles/Input/OpenScr"), Common.DirectoryPathReplace(TestSourcePath + "TestFiles/Output/OpenScr"));
            _runTest.fromFile(test);
        }
        #endregion Setup

        [Test]
        public void SetUiTestsToRunTests() {}

#if UiTests
        [Test]
		public void NewProject() { _runTest.fromFile("NewPublication.xml"); }

		[Test]
		public void OpenProject() { RunTestFromFileWithProject("OpenProject.xml"); }

        [Test]
        public void PdfExportTest()
        {
            Common.PublishingSolutionsEnvironmentReset();
            FolderTree.Copy(TestSourcePath + @"TestFiles/Input/PdfTest", TestSourcePath + @"TestFiles/Output/PdfTest");
            _runTest.fromFile("PdfExportTest.xml");
            var foundProc = false;
            do
            {
                var procs = Process.GetProcesses();
                foreach (var proc in procs)
                    if (proc.MainWindowTitle == "B-1pe.pdf - Adobe Reader")
                    {
                        proc.Kill();
                        foundProc = true;
                        break;
                    }
            } while (!foundProc);
            Assert.AreEqual(true, File.Exists(TestSourcePath + @"TestFiles/Output/PdfTest/B-1pe.pdf"), "Pdf Export failed");
        }
#endif
    }
}