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
// File: WordPressConvertTest.cs
// Responsibility: Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.WordPressConvert
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Test functions of Wordpress Convert
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    [TestFixture]
    public class ExportWordPressTest: ExportGoBible
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
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/WordPressConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "expected");
            if (Directory.Exists(_outputPath))
                Directory.Delete(_outputPath, true);
            Directory.CreateDirectory(_outputPath);
        }
        #endregion setup

        [Test]
        public void ExportTypeTest()
        {
            var target = new ExportWordPress();
            var actual = target.ExportType;
            Assert.AreEqual("WordPress Alpha", actual);
        }

        [Test]
        public void HandleDictionaryTest()
        {
            var target = new ExportWordPress();
            var actual = target.Handle("Dictionary");
            Assert.IsTrue(actual);
        }

        [Test]
        public void HandleScriptureTest()
        {
            var target = new ExportWordPress();
            var actual = target.Handle("Scripture");
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for Export
        ///</summary>
        [Test]
        public void ExportNullTest()
        {
            var target = new ExportWordPress();
            PublicationInformation projInfo = null;
            var actual = target.Export(projInfo);
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for Export
        ///</summary>
        [Test]
        public void ExportPassTest()
        {
            const string XhtmlName = "main.xhtml";
            const string CssName = "main.css";
            PublicationInformation projInfo = GetProjInfo(XhtmlName, CssName);
            var target = new ExportWordPress();
            target.skipForNUnitTest = true;
            var actual = target.Export(projInfo);
            Assert.IsTrue(actual);
            const string dataSql = "data.sql";
            Assert.AreEqual("", IsEqualAllButTime(FileExpected(dataSql), FileOutput(dataSql)));
        }

        private static string IsEqualAllButTime(string fileExpectedName, string fileOutputName)
        {
            const string pat = @"\d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d";
            try
            {
                var fileExpected = new StreamReader(fileExpectedName);
                var fileOutput = new StreamReader(fileOutputName);
                while (!fileExpected.EndOfStream)
                {
                    var expectedLine = Regex.Replace(fileExpected.ReadLine(), pat, "");
                    var outputLine = Regex.Replace(fileOutput.ReadLine(), pat, "");
                    if (expectedLine != outputLine)
                        return outputLine;
                }
                if (!fileOutput.EndOfStream)
                    return "Expected stream ended before output stream";
                return "";
            }
            catch (Exception e)
            {
                return e.Message;
            }
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
