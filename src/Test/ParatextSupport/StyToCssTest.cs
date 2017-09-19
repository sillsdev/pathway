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
using System.IO;
using System.Threading;
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
			Common.CallerSetting?.Dispose();
			DataCreator.Creator = DataCreator.CreatorProgram.Unknown;
			Common.CallerSetting = new CallerSetting {SettingsFullPath = FileInput("testDb.ssf")};
			if (!Directory.Exists(_outputPath))
			{
				Directory.CreateDirectory(_outputPath);
				while (!Directory.Exists(_outputPath))
					Thread.Sleep(1000);
			}
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			Common.CallerSetting.Dispose();
			Common.CallerSetting = null;
		}
	#endregion setup

        [Test]
        public void WriteLanguageFontDirectionTest()
        {
            const string TestName = "WriteLanguageFontDirectionTest";
			var cssFile = TestName + ".css";
			DataCreator.Creator = DataCreator.CreatorProgram.Paratext7;
			using (Common.CallerSetting = new CallerSetting {SettingsFullPath = FileInput(TestName + ".ssf")})
	        {
				TextWriter sw = new StreamWriter(FileOutput(cssFile));
				WriteLanguageFontDirection(sw);
				sw.Close();
			}

			string expectedFile = FileExpected(cssFile);
	        string outputFile = FileOutput(cssFile);

			TextFileAssert.AreEqual(expectedFile, outputFile, FileData.Get(FileOutput(cssFile)));
        }


        [Test]
		//[Category("SkipOnTeamCity")]
        public void StytoCSSnkoNTProjectCSSTest()
        {
            const string TestName = "nkoNT";
            var cssFile = TestName + ".css";
            string cssFileOutput = FileOutput(cssFile);

            StyToCss styToCssObj = new StyToCss();
			styToCssObj.ConvertStyToCss("nkoNT", cssFileOutput);

			string expectedFile = FileExpected(cssFile);
			string outputFile = FileOutput(cssFile);

			TextFileAssert.AreEqual(expectedFile, outputFile, FileData.Get(FileOutput(cssFile)));
        }

        [Test]
        public void StytoCSSTest()
        {
            const string TestName = "default";
            var cssFile = TestName + ".css";
            string cssFileOutput = FileOutput(cssFile);
            StyToCss styToCssObj = new StyToCss();
            styToCssObj.ConvertStyToCss(cssFileOutput);

			string expectedFile = FileExpected(cssFile);
			string outputFile = FileOutput(cssFile);

			TextFileAssert.AreEqual(expectedFile, outputFile, FileData.Get(FileOutput(cssFile)));
        }

		[Test]
		public void IntroductionStytoCSSTest()
		{
			const string TestName = "Introduction";
			var cssFile = TestName + ".css";
			string cssFileOutput = FileOutput(cssFile);
			StyToCss styToCssObj = new StyToCss();
			styToCssObj.StyFullPath = FileInput(TestName + ".sty");
			styToCssObj.ConvertStyToCss(cssFileOutput);

			string expectedFile = FileExpected(cssFile);
			string outputFile = FileOutput(cssFile);

			TextFileAssert.AreEqual(expectedFile, outputFile, FileData.Get(FileOutput(cssFile)));
		}

		[Test]
		//[Category("SkipOnTeamCity")]
		public void StytoCSSPnCSSTest()
		{
			const string TestName = "aai";
			var cssFile = TestName + ".css";
			string cssFileOutput = FileOutput(cssFile);

			StyToCss styToCssObj = new StyToCss();
			styToCssObj.ConvertStyToCss("aai", cssFileOutput);

			string expectedFile = FileExpected(cssFile);
			string outputFile = FileOutput(cssFile);

			TextFileAssert.AreEqual(expectedFile, outputFile, FileData.Get(FileOutput(cssFile)));
		}

		[Test]
		//[Category("SkipOnTeamCity")]
		public void MergeMainandCustomCssStylesTest()
		{
			const string TestName = "uitrans";
			var cssFile = TestName + ".css";
			string cssFileOutput = FileOutput(cssFile);

			StyToCss styToCssObj = new StyToCss();
			styToCssObj.ConvertStyToCss("uitrans", cssFileOutput);

			string expectedFile = FileExpected(cssFile);
			string outputFile = FileOutput(cssFile);

			TextFileAssert.AreEqual(expectedFile, outputFile, FileData.Get(FileOutput(cssFile)));
		}

		[Test]
		//[Category("SkipOnTeamCity")]
		public void uisTrans_Marker_rq_StytoCSSTest()
		{
			const string TestName = "uisTrans";
			var cssFile = TestName + ".css";
			string cssFileOutput = FileOutput(cssFile);

			StyToCss styToCssObj = new StyToCss();
			styToCssObj.ConvertStyToCss("uisTrans", cssFileOutput);

			string expectedFile = FileExpected(cssFile);
			string outputFile = FileOutput(cssFile);

			TextFileAssert.AreEqual(expectedFile, outputFile, FileData.Get(FileOutput(cssFile)));
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
