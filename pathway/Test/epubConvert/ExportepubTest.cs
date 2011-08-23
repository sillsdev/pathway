// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: ExportepubTest.cs
// Responsibility: Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.epubConvert
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Test functions of epub Convert
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    [TestFixture]
    public class ExportepubTest
    {
        #region setup
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;

        [TestFixtureSetUp]
        public void Setup()
        {
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../PsSupport");
            Common.SupportFolder = "";
            Common.ProgBase = Common.ProgInstall;
            Common.Testing = true;
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/epubConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "Input");
            _outputPath = Common.PathCombine(testPath, "Output");
            _expectedPath = Common.PathCombine(testPath, "Expected");
//            if (Directory.Exists(_outputPath))
//                Directory.Delete(_outputPath, true);
            Directory.CreateDirectory(_outputPath);
        }
        #endregion setup

        [Test]
        public void ExportTypeTest()
        {
            var target = new Exportepub();
            var actual = target.ExportType;
            Assert.AreEqual("E-Book (.epub)", actual);
        }

        [Test]
        public void HandleDictionaryTest()
        {
            var target = new Exportepub();
            var actual = target.Handle("Dictionary");
            Assert.IsTrue(actual);
        }

        [Test]
        public void HandleScriptureTest()
        {
            var target = new Exportepub();
            var actual = target.Handle("Scripture");
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for Export
        ///</summary>
        [Test]
        public void ExportNullTest()
        {
            var target = new Exportepub();
            PublicationInformation projInfo = null;
            var actual = target.Export(projInfo);
            Assert.IsFalse(actual);
        }

        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void ExportDictionaryPassTest()
        {
            const string XhtmlName = "main.xhtml";
            const string CssName = "main.css";
            PublicationInformation projInfo = GetProjInfo(XhtmlName, CssName);
            File.Copy(FileInput("FlexRev.xhtml"), FileOutput("FlexRev.xhtml"), true);
            File.Copy(FileInput("FlexRev.css"), FileOutput("FlexRev.css"), true);
            projInfo.IsReversalExist = true;
            projInfo.IsLexiconSectionExist = true;
            projInfo.DefaultRevCssFileWithPath = Path.Combine(_inputPath, "FlexRev.css");
            projInfo.ProjectName = "EBook (epub)_" + DateTime.Now.Date.ToShortDateString() + "_" +
                                   DateTime.Now.Date.ToShortTimeString();
            var target = new Exportepub();
            var actual = target.Export(projInfo);
            Assert.IsTrue(actual);
        }

        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void ExportScripturePassTest()
        {
            const string XhtmlName = "Scripture Draft.xhtml";
            const string CssName = "Scripture Draft.css";
            PublicationInformation projInfo = GetProjInfo(XhtmlName, CssName);
            projInfo.IsReversalExist = false;
            projInfo.ProjectName = "Scripture Draft";
            var target = new Exportepub();
            var actual = target.Export(projInfo);
            Assert.IsTrue(actual);
        }


        #region Private Functions
        private string FileProg(string fileName)
        {
            return Common.PathCombine(Common.GetPSApplicationPath(), fileName);
        }

        private string FileInput(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }

        private string FileOutput(string fileName)
        {
            return Common.PathCombine(_outputPath, fileName);
        }

        private string FileExpected(string fileName)
        {
            return Common.PathCombine(_expectedPath, fileName);
        }

        /// <summary>
        /// Create a simple PublicationInformation instance
        /// </summary>
        private PublicationInformation GetProjInfo(string XhtmlName, string BlankName)
        {
            PublicationInformation projInfo = new PublicationInformation();
            File.Copy(FileInput(XhtmlName), FileOutput(XhtmlName), true);
            File.Copy(FileInput(BlankName), FileOutput(BlankName), true);
            projInfo.DefaultXhtmlFileWithPath = FileOutput(XhtmlName);
            projInfo.DefaultCssFileWithPath = FileOutput(BlankName);
            projInfo.IsOpenOutput = false;
            return projInfo;
        }
        #endregion PrivateFunctions
    }
}
