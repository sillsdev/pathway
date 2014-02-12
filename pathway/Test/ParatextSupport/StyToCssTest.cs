// --------------------------------------------------------------------------------------------
// <copyright file="StyToCssTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using NMock2;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.ParatextSupport
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Test functions of StyToCSS Convert
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    [TestFixture]
    public class StyToCssTest: StyToCSS
    {
        #region setup
        private static Mockery mocks = new Mockery();
        private static string _inputPath;
        private static string _outputPath;
        private static string _expectedPath;

        [TestFixtureSetUp]
        public void Setup()
        {
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../PsSupport");
            Common.SupportFolder = "";
            Common.ProgBase = Common.ProgInstall;
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/ParatextSupport/TestFiles");
            _inputPath = Common.PathCombine(testPath, "Input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "Expected");
            if (Directory.Exists(_outputPath))
                Directory.Delete(_outputPath, true);
            Directory.CreateDirectory(_outputPath);
        }
        #endregion setup

        [Test]
        public void WriteLanguageFontDirectionTest()
        {
            const string TestName = "WriteLanguageFontDirectionTest";
            var cssFile = TestName + ".css";
            TextWriter sw = new StreamWriter(FileOutput(cssFile));
            WriterSettingsFile = FileInput(TestName + ".ssf");
            Common.TextDirectionLanguageFile = FileInput("Dhivehi.lds");
            WriteLanguageFontDirection(sw);
            sw.Close();
            TextFileAssert.AreEqual(FileExpected(cssFile), FileOutput(cssFile), FileData.Get(FileOutput(cssFile)));
        }

        #region Private Functions
        private static string FileProg(string fileName)
        {
            return Common.PathCombine(Common.GetPSApplicationPath(), fileName);
        }

        private static string FileInput(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }

        private static string FileOutput(string fileName)
        {
            return Common.PathCombine(_outputPath, fileName);
        }

        private static string FileExpected(string fileName)
        {
            return Common.PathCombine(_expectedPath, fileName);
        }

        /// <summary>
        /// Create a simple PublicationInformation instance
        /// </summary>
        private static PublicationInformation GetProjInfo(string XhtmlName, string BlankName)
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
