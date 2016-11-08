// --------------------------------------------------------------------------------------------
// <copyright file="StyToCssTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
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
    public class StyToCssTest : StyToCss
    {
        #region setup
        private static string _inputPath;
        private static string _outputPath;
        private static string _expectedPath;

        [TestFixtureSetUp]
        public void Setup()
        {
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../../DistFiles");
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


        [Test]
		[Category("SkipOnTeamCity")]
        public void StytoCSSnkoNTProjectCSSTest()
        {
            const string TestName = "nkoNT";
            var cssFile = TestName + ".css";
            string cssFileOutput = FileOutput(cssFile);
            string ssfFileInputPath = FileInput(TestName);
            ssfFileInputPath = Common.PathCombine(ssfFileInputPath, "gather");
            ssfFileInputPath = Common.PathCombine(ssfFileInputPath, TestName + ".ssf");

            StyToCss styToCssObj = new StyToCss();
            styToCssObj.ConvertStyToCss("nkoNT", cssFileOutput, ssfFileInputPath);

            TextFileAssert.AreEqual(FileExpected(cssFile), FileOutput(cssFile), FileData.Get(FileOutput(cssFile)));
        }

        [Test]
        public void StytoCSSTest()
        {
            const string TestName = "default";
            var cssFile = TestName + ".css";
            string cssFileOutput = FileOutput(cssFile);
            StyToCss styToCssObj = new StyToCss();
            styToCssObj.ConvertStyToCss(cssFileOutput);

            TextFileAssert.AreEqual(FileExpected(cssFile), FileOutput(cssFile), FileData.Get(FileOutput(cssFile)));
        }

		[Test]
		[Category("SkipOnTeamCity")]
		public void StytoCSSPnCSSTest()
		{
			const string TestName = "aai";
			var cssFile = TestName + ".css";
			string cssFileOutput = FileOutput(cssFile);
			string ssfFileInputPath = FileInput(TestName);
			ssfFileInputPath = Common.PathCombine(ssfFileInputPath, "gather");
			ssfFileInputPath = Common.PathCombine(ssfFileInputPath, TestName + ".ssf");

			StyToCss styToCssObj = new StyToCss();
			styToCssObj.ConvertStyToCss("aai", cssFileOutput, ssfFileInputPath);

			TextFileAssert.AreEqual(FileExpected(cssFile), FileOutput(cssFile), FileData.Get(FileOutput(cssFile)));
		}

        #region Private Functions
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
        #endregion PrivateFunctions
    }
}
