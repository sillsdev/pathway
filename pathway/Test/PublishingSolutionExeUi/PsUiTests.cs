// --------------------------------------------------------------------------------------------
// <copyright file="PsUiTests.cs" from='2009' to='2009' company='SIL International'>
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
    public class PsUiTests
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
                return Common.PathCombine(Environment.CurrentDirectory, @"..\..\PublishingSolutionExeUi\");
            }
        }

        /// <summary>
        /// setup test root
        /// </summary>
        [TestFixtureSetUp]
        protected void SetUp()
        {
            var format = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><include><var id=\"testPath\" set=\"{0}TestFiles\"/></include>";
            var sw = new StreamWriter(TestSourcePath + @"Scripts/DE/testPath.xml");
            sw.Write(string.Format(format, TestSourcePath));
            sw.Close();
            var outInfo = new DirectoryInfo(TestSourcePath + @"TestFiles/Output");
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
            FolderTree.Copy(TestSourcePath + @"TestFiles/Input/OpenDic", TestSourcePath + @"TestFiles/Output/OpenDic");
            FolderTree.Copy(TestSourcePath + @"TestFiles/Input/OpenScr", TestSourcePath + @"TestFiles/Output/OpenScr");
            _runTest.fromFile(test);
        }
        #endregion Setup

        [Test]
        public void SetUiTestsToRunTests() {}

#if UiTests
        [Test]
        public void MenuFileNew() { _runTest.fromFile("MenuFileNew.xml"); }

        [Test]
        public void MenuFileOpen() { _runTest.fromFile("MenuFileOpen.xml"); }

        [Test]
        public void MenuFileExport() { RunTestFromFileWithProject("MenuFileExport.xml"); }

        [Test]
        public void MenuFileClose() { RunTestFromFileWithProject("MenuFileClose.xml"); }

        [Test]
        public void MenuFileExit() { _runTest.fromFile("MenuFileExit.xml"); }

        [Test]
        public void MenuEditCss() { RunTestFromFileWithProject("MenuEditCss.xml"); }

        [Test]
        public void MenuEditSave() { RunTestFromFileWithProject("MenuEditSave.xml"); }

        [Test]
        public void MenuFormatTaskPick() { RunTestFromFileWithProject("MenuFormatTaskPick.xml"); }

        [Test]
        public void MenuFormatTemplate() { RunTestFromFileWithProject("MenuFormatTemplate.xml"); }

        [Test]
        public void MenuFormatPreview() { RunTestFromFileWithProject("MenuFormatPreview.xml"); }

        [Test]
        public void MenuToolsLocTool() { _runTest.fromFile("MenuToolsLocTool.xml"); }

        [Test]
        public void MenuToolsLocSetup() { _runTest.fromFile("MenuToolsLocSetup.xml"); }

        [Test]
        public void MenuToolsBackup() { RunTestFromFileWithProject("MenuToolsBackup.xml"); }

        [Test]
        public void MenuHelpContents() { _runTest.fromFile("MenuHelpContents.xml"); }

        [Test]
        public void MenuHelpLicense() { _runTest.fromFile("MenuHelpLicense.xml"); }

        [Test]
        public void MenuHelpRelease() { _runTest.fromFile("MenuHelpRelease.xml"); }

        [Test]
        public void MenuHelpReadme() { _runTest.fromFile("MenuHelpReadme.xml"); }

        [Test]
        public void MenuHelpFlex() { _runTest.fromFile("MenuHelpFlex.xml"); }

        [Test]
        public void MenuHelpFeedback() { _runTest.fromFile("MenuHelpFeedback.xml"); }

        [Test]
        public void MenuHelpLargeFiles() { _runTest.fromFile("MenuHelpLargeFiles.xml"); }

        [Test]
        public void MenuHelpAbout() { _runTest.fromFile("MenuHelpAbout.xml"); }
#endif
    }
}