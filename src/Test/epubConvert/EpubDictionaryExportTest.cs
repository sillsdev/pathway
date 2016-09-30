using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
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
	public class EpubDictionaryExportTest
	{
		#region setup
		private string _inputPath;
		private string _outputPath;
		private string _expectedPath;
		private static TestFiles _tf;

		[TestFixtureSetUp]
		public void Setup()
		{
			Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../../DistFiles");
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
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void ExportDictionaryCssFileComparisonTest()
		{
			// clean out old files
			CleanOutputDirectory();
			if (!Directory.Exists(FileOutput("ExportDictionary")))
				Directory.CreateDirectory(FileOutput("ExportDictionary"));

			const string XhtmlName = "main.xhtml";
			const string CssName = "main.css";
			PublicationInformation projInfo = GetProjInfo(XhtmlName, CssName);
			File.Copy(FileInput("FlexRev.xhtml"), FileOutput("FlexRev.xhtml"), true);
			File.Copy(FileInput("FlexRev.css"), FileOutput("FlexRev.css"), true);
			File.Copy(FileProg(@"Styles\Dictionary\epub.css"), FileOutput("epub.css"), true);
			projInfo.IsReversalExist = true;
			projInfo.IsLexiconSectionExist = true;
			projInfo.ProjectInputType = "Dictionary";
			projInfo.DefaultRevCssFileWithPath = Common.PathCombine(_inputPath, "FlexRev.css");
			string expCssLine = "@import \"" + Path.GetFileName(projInfo.DefaultRevCssFileWithPath) + "\";";
			Common.FileInsertText(FileOutput("epub.css"), expCssLine);
			Param.LoadSettings();
			projInfo.ProjectName = "EBook (epub)_" + DateTime.Now.Date.ToShortDateString() + "_" +
								   DateTime.Now.Date.ToShortTimeString();
			var target = new Exportepub();
			var actual = target.Export(projInfo);
			Assert.IsTrue(actual);
			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			var zf = new FastZip();
			zf.ExtractZip(result, FileOutput("main"), ".*");

			File.Copy(FileExpected("ExportDictionaryCSSFileComparisonExpected.epub"), FileOutput("ExportDictionaryCSSFileComparisonExpected.epub"), true);
			result = FileOutput("ExportDictionaryCSSFileComparisonExpected.epub");
			zf = new FastZip();
			zf.ExtractZip(result, FileOutput("ExportDictionaryCSSFileComparisonExpected"), ".*");

			TextFileAssert.CheckLineAreEqualEx(FileOutput("main/OEBPS/book.css"), FileOutput("ExportDictionaryCSSFileComparisonExpected/OEBPS/book.css"), new ArrayList { 93, 110, 112, 643, 652, 965 });

		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void ExportDictionaryPassTest()
		{
			// clean out old files
			CleanOutputDirectory();
			Directory.CreateDirectory(_outputPath);
			const string XhtmlName = "main.xhtml";
			const string CssName = "main.css";
			PublicationInformation projInfo = GetProjInfo(XhtmlName, CssName);
			File.Copy(FileInput("FlexRev.xhtml"), FileOutput("FlexRev.xhtml"), true);
			File.Copy(FileInput("FlexRev.css"), FileOutput("FlexRev.css"), true);
			File.Copy(FileProg(@"Styles\Dictionary\book.css"), FileOutput("book.css"), true);
			projInfo.IsReversalExist = true;
			projInfo.IsLexiconSectionExist = true;
			projInfo.ProjectInputType = "Dictionary";
			projInfo.DefaultRevCssFileWithPath = Common.PathCombine(_inputPath, "FlexRev.css");
			projInfo.ProjectName = "EBook (epub)_" + DateTime.Now.Date.ToShortDateString() + "_" +
								   DateTime.Now.Date.ToShortTimeString();
			var target = new Exportepub();
			var actual = target.Export(projInfo);
			Assert.IsTrue(actual);
			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			var zf = new FastZip();
			zf.ExtractZip(result, FileOutput("main"), ".*");
			var resultDoc = Common.DeclareXMLDocument(false);
			resultDoc.Load(FileOutput(Common.DirectoryPathReplace("main/OEBPS/PartFile00001_.xhtml")));
			var nsmgr = new XmlNamespaceManager(resultDoc.NameTable);
			nsmgr.AddNamespace("x", "http://www.w3.org/1999/xhtml");
			var node = resultDoc.SelectSingleNode("//x:span[@class='translation_L2']/x:span[2]/x:span", nsmgr);
			Assert.AreEqual(node.InnerText.Trim(), "child of Fatima"); // Fail if content is gone (TD-2814)
		}

		
		public static void IsValid(string filename, string msg)
		{
			Assert.IsTrue(File.Exists(filename), string.Format("{0}: {1} does not exist", msg, filename));
			// Running the unit test - just run the validator and return the result
			var validationResults = epubValidator.Program.ValidateFile(filename);
			Assert.IsTrue(validationResults.Contains("No errors or warnings detected"), string.Format("{0}: Validation Errors: {1}", msg, validationResults));
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
		
		private void CleanOutputDirectory()
		{
			Common.DeleteDirectory(_outputPath);
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
			projInfo.DictionaryPath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
			projInfo.IsOpenOutput = false;
			return projInfo;
		}
		#endregion PrivateFunctions
	}
}
