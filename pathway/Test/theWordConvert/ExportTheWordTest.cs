// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2013, SIL International. All Rights Reserved.
// <copyright from='2013' to='2013' company='SIL International'>
//		Copyright (c) 2013, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: TheWordConvertTest.cs
// Responsibility: Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.TheWordConvertTest
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Test functions of Wordpress Convert
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    [TestFixture]
    public class ExportTheWordTest: ExportTheWord
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
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/theWordConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "Input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "Expected");
            if (Directory.Exists(_outputPath))
                Directory.Delete(_outputPath, true);
            Directory.CreateDirectory(_outputPath);
        }
        #endregion setup

        [Test]
        public void ExportTypeTest()
        {
            var target = new ExportTheWord();
            var actual = target.ExportType;
            Assert.AreEqual("theWord", actual);
        }

        [Test]
        public void HandleScriptureTest()
        {
            var target = new ExportTheWord();
            var actual = target.Handle("Scripture");
            Assert.IsTrue(actual);
        }

        [Test]
        public void HandleDictionaryTest()
        {
            var target = new ExportTheWord();
            var actual = target.Handle("Dictionary");
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for Export
        ///</summary>
        [Test]
        public void ExportNullTest()
        {
            var target = new ExportTheWord();
            PublicationInformation projInfo = null;
            var actual = target.Export(projInfo);
            Assert.IsFalse(actual);
        }

        [Test]
        public void LoadXsltTest()
        {
            LoadXslt();
            Assert.AreEqual("System.Text.UTF8Encoding", TheWord.OutputSettings.Encoding.ToString());
        }

        [Test]
        public void CollectTestamentBooksTest()
        {
            var curDir = Environment.CurrentDirectory;
            string vrsPath = PathPart.Bin(Environment.CurrentDirectory, "/../theWordConvert");
            Environment.CurrentDirectory = vrsPath;
            var otBooks = new List<string> ();
            var ntBooks = new List<string> ();
            CollectTestamentBooks(otBooks, ntBooks);
            Environment.CurrentDirectory = curDir;
            Assert.AreEqual(39, otBooks.Count);
            Assert.AreEqual(27, ntBooks.Count);
            Assert.AreEqual("GEN", otBooks[0]);
            Assert.AreEqual("MAL", otBooks[38]);
            Assert.AreEqual("MAT", ntBooks[0]);
            Assert.AreEqual("REV", ntBooks[26]);

        }

        [Test]
        public void LoadXsltParametersTest()
        {
            ParatextData = @"C:\";
            Ssf = FileInput("MP1.ssf");
            var actual = LoadXsltParameters();
            Assert.AreEqual(":", actual.GetParam("refPunc", ""));
            Assert.AreEqual(@"file:///C:\MP1\BookNames.xml", actual.GetParam("bookNames", ""));
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
