// --------------------------------------------------------------------------------------------
// <copyright file="EmbeddedFontTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Erik Brommers</author>
// <email>erik_brommers@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Test methods of EmbeddedFont class
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using epubConvert;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;
using epubValidator;

namespace Test.epubConvert
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Test functions of epub Convert
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class ExportepubTest : Exportepub
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
		public void ExportTypeTest()
		{
			var target = new Exportepub();
			var actual = target.ExportType;
			Assert.AreEqual("E-Book (Epub2 and Epub3)", actual);
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
			// clean out old files
			foreach (var file in Directory.GetFiles(_outputPath))
			{
				if (File.Exists(file))
					File.Delete(file);
			}
			string appDataDir = Common.GetAllUserPath();
			if (Directory.Exists(appDataDir))
			{
				Directory.Delete(appDataDir, true);
			}


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
			projInfo.ProjectInputType = "Scripture";
			var target = new Exportepub();
			var actual = target.Export(projInfo);
			Assert.IsTrue(actual);
		}

		[Test]
		public void ChapterLinkAfterTitleTest()
		{
			const string FolderName = "ChapterLinkTest";
			FolderTree.Copy(FileInput(FolderName), FileOutput(FolderName));
			InputType = "scripture";
			InsertChapterLinkBelowBookName(FileOutput(FolderName));
			XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
			XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false };
			var filePath = Common.PathCombine(FolderName, "PartFile00001_.xhtml");
			XmlReader xmlReader = XmlReader.Create(FileOutput(filePath), xmlReaderSettings);
			xmlDocument.Load(xmlReader);
			xmlReader.Close();
			XmlNodeList nodes = xmlDocument.SelectNodes(".//xhtml:a", namespaceManager);
			Assert.AreEqual(3, nodes.Count, "Should be 3 chapter links for Titus");
			var next = nodes[nodes.Count - 1].NextSibling.NextSibling;
			Assert.AreEqual("Section_Head", next.Attributes.GetNamedItem("class").InnerText, "Section head should follow chapter links");
		}

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
		[Category("ShortTest")]
		[Category("SkipOnTeamCity")]
		public void InsertReferenceLinkInTocFileTest()
		{
			// clean output directory
			CleanOutputDirectory();
			const string FolderName = "ReferenceLink";

			if (!Directory.Exists(FileOutput(FolderName)))
				Directory.CreateDirectory(FileOutput(FolderName));

			FolderTree.Copy(FileInput(FolderName), FileOutput(FolderName));
			FolderTree.Copy(FileExpected(FolderName + "Expected"), FileOutput(FolderName + "Expected"));

			InsertReferenceLinkInTocFile(FileOutput(FolderName));
			FileCompare(FolderName + "/File3TOC00000_.xhtml", FolderName + "Expected" + "/File3TOC00000_.xhtml");
		}


		[Test]
		public void ChapterLinkForSingleChapterTest()
		{
			const string FolderName = "ChapterLinkTest";
			FolderTree.Copy(FileInput(FolderName), FileOutput(FolderName));
			InputType = "scripture";
			InsertChapterLinkBelowBookName(FileOutput(FolderName));
			XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
			XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false };
			var filePath = Common.PathCombine(FolderName, "PartFile00002_.xhtml");
			XmlReader xmlReader = XmlReader.Create(FileOutput(filePath), xmlReaderSettings);
			xmlDocument.Load(xmlReader);
			xmlReader.Close();
			XmlNodeList nodes = xmlDocument.SelectNodes(".//xhtml:a", namespaceManager);
			Assert.AreEqual(0, nodes.Count, "Should be 0 chapter links for Philemon");
		}

		[Test]
		public void SplitPageSectionPageBreakFalseTest()
		{
			const string FolderName = "SplitPageSectionTest";

			string[] outputFiles;

			if (!Directory.Exists(FileOutput(FolderName)))
				Directory.CreateDirectory(FileOutput(FolderName));

			outputFiles = Directory.GetFiles(FileOutput(FolderName), "PartFile*.xhtml");

			foreach (var file in outputFiles)
			{
				if (File.Exists(file))
					File.Delete(file);
			}

			const string TestFolderName = "SplitPageSectionTestOutput";
			FolderTree.Copy(FileInput(FolderName), FileOutput(TestFolderName));


			InputType = "dictionary";
			List<string> htmlFiles = new List<string>();
			string[] files = Directory.GetFiles(FileOutput(TestFolderName), "PartFile*.xhtml");
			foreach (var htmlFile in files)
			{
				htmlFiles.Add(htmlFile);
			}

			PageBreak = false;
			SplitPageSections(htmlFiles, FileOutput(FolderName), "");
			outputFiles = Directory.GetFiles(FileOutput(FolderName), "PartFile*.xhtml");

			if (Directory.Exists(TestFolderName))
				Directory.Delete(TestFolderName);

			Assert.AreEqual(5, outputFiles.Length, "Should be 5 PartFiles but its " + outputFiles.Length.ToString());
		}

		[Test]
		public void SplitPageSectionPageBreakTrueTest()
		{
			const string FolderName = "SplitPageSectionTest";

			string[] outputFiles;

			if (!Directory.Exists(FileOutput(FolderName)))
				Directory.CreateDirectory(FileOutput(FolderName));

			outputFiles = Directory.GetFiles(FileOutput(FolderName), "PartFile*.xhtml");

			foreach (var file in outputFiles)
			{
				if (File.Exists(file))
					File.Delete(file);
			}

			const string TestFolderName = "SplitPageSectionTestOutput";
			FolderTree.Copy(FileInput(FolderName), FileOutput(TestFolderName));


			InputType = "dictionary";
			List<string> htmlFiles = new List<string>();
			string[] files = Directory.GetFiles(FileOutput(TestFolderName), "PartFile*.xhtml");
			foreach (var htmlFile in files)
			{
				htmlFiles.Add(htmlFile);
			}

			PageBreak = true;
			SplitPageSections(htmlFiles, FileOutput(FolderName), "");
			outputFiles = Directory.GetFiles(FileOutput(FolderName), "PartFile*.xhtml");

			if (Directory.Exists(TestFolderName))
				Directory.Delete(TestFolderName);

			Assert.AreEqual(3, outputFiles.Length, "Should be 3 PartFiles but its " + outputFiles.Length.ToString());
		}



		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void FootnoteVerseNumberTest()
		{
			const string XhtmlName = "FootnoteVerseNumber1.xhtml";
			const string CssName = "FootnoteVerseNumber1.css";
			PublicationInformation projInfo = GetProjInfo(XhtmlName, CssName);
			projInfo.IsReversalExist = false;
			projInfo.ProjectName = "Scripture Draft";
			projInfo.ProjectInputType = "Scripture";
			var target = new Exportepub();
			var actual = target.Export(projInfo);
			Assert.IsTrue(actual);
			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			var zf = new FastZip();
			zf.ExtractZip(result, FileOutput("main"), ".*");
			var resultDoc = Common.DeclareXMLDocument(false);
			resultDoc.Load(FileOutput(Common.DirectoryPathReplace("main/OEBPS/PartFile00001_.xhtml")));
			var nsmgr = new XmlNamespaceManager(resultDoc.NameTable);
			nsmgr.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			//1.19
			string xPath = "//xhtml:ul[@class='footnotes']/xhtml:li[@id='FN_Footnote-LUK-6']";
			XmlNode node = resultDoc.SelectSingleNode(xPath, nsmgr);
			Assert.AreEqual(node.InnerText.Trim(), "[b] Bahasa Yunani bilang “Badiri di Allah pung muka”. Ini bisa pung arti ‘Karja par Tuhan’. Mar bisa pung arti lai ‘Badiri di Allah pung muka’. Malekat yang badiri di Allah pung muka pung kuasa labe dari malekat laeng. Jadi, Gabriel bukang malekat biasa.");
			//1.27
			xPath = "//xhtml:ul[@class='footnotes']/xhtml:li[@id='FN_Footnote-LUK-7']";
			node = resultDoc.SelectSingleNode(xPath, nsmgr);
			Assert.AreEqual(node.InnerText.Trim(), "1.27  Mat. 1:18");
			//1.32-33
			xPath = "//xhtml:ul[@class='footnotes']/xhtml:li[@id='FN_Footnote-LUK-9']";
			node = resultDoc.SelectSingleNode(xPath, nsmgr);
			Assert.AreEqual(node.InnerText.Trim(), "1.32-33  2Sam. 7:12, 13, 16; Yes. 9:6");
			//2.41
			xPath = "//xhtml:ul[@class='footnotes']/xhtml:li[@id='FN_Footnote-LUK-22']";
			node = resultDoc.SelectSingleNode(xPath, nsmgr);
			Assert.AreEqual(node.InnerText.Trim(), "[c] 2.41 Hari basar Paska Yahudi tu, orang Yahudi inga waktu dong pung tete nene moyang kaluar dari negara Mesir. Dolo dong jadi orang suru-suru di tampa tu, mar Allah kasi kaluar dong la bawa dong ka tana yang Antua su janji par dong.");
		}

		[Test]
		public void RemoveEmptyHrefTest()
		{
			CleanOutputDirectory();
			const string folderName = "RemoveEmptyHrefTest";
			FolderTree.Copy(FileInput(folderName), FileOutput(folderName));
			ReplaceEmptyHref(FileOutput(folderName));
			string expectedFilesPath = FileExpected(folderName.Replace("Test", "Expected"));
			FileCompare(FileOutput(folderName) + "/PartFile00001_01.xhtml", expectedFilesPath + "/PartFile00001_01.xhtml");
			FileCompare(FileOutput(folderName) + "/PartFile00001_03.xhtml", expectedFilesPath + "/PartFile00001_03.xhtml");
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

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void EpubIndentFileComparisonTest()
		{
			// clean out old files
			CleanOutputDirectory();
			if (!Directory.Exists(FileOutput("ExportDictionary")))
				Directory.CreateDirectory(FileOutput("ExportDictionary"));

			const string XhtmlName = "EpubIndentFileComparison.xhtml";
			const string CssName = "EpubIndentFileComparison.css";
			PublicationInformation projInfo = GetProjInfo(XhtmlName, CssName);
			projInfo.IsReversalExist = false;
			projInfo.ProjectName = "Scripture Draft";
			projInfo.ProjectInputType = "Scripture";
			File.Copy(FileProg(@"Styles\Scripture\epub.css"), FileOutput("epub.css"));
			var target = new Exportepub();
			var actual = target.Export(projInfo);
			Assert.IsTrue(actual);
			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			var zf = new FastZip();
			zf.ExtractZip(result, FileOutput("EpubIndentFileComparison"), ".*");
			var zfExpected = new FastZip();
			result = result.Replace("Output", "Expected");
			zfExpected.ExtractZip(result, FileOutput("EpubIndentFileComparisonExpect"), ".*");
			FileCompare("EpubIndentFileComparison/OEBPS/PartFile00001_01.xhtml", "EpubIndentFileComparisonExpect/OEBPS/PartFile00001_01.xhtml");

		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void ExportDictionaryInsertBeforeAfterTest()
		{
			// clean out old files
			foreach (var file in Directory.GetFiles(_outputPath))
			{
				if (File.Exists(file))
					File.Delete(file);
			}

			const string XhtmlName = "InsertBeforeAfter.xhtml";
			const string CssName = "InsertBeforeAfter.css";
			PublicationInformation projInfo = GetProjInfo(XhtmlName, CssName);
			projInfo.IsReversalExist = false;
			projInfo.ProjectName = "Dictionary Test";
			projInfo.ProjectInputType = "Dictionary";
			projInfo.IsLexiconSectionExist = true;
			File.Copy(FileProg(@"Styles\Dictionary\epub.css"), FileOutput("epub.css"));
			var target = new Exportepub();
			var actual = target.Export(projInfo);
			Assert.IsTrue(actual);
			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			var zf = new FastZip();
			zf.ExtractZip(result, FileOutput("InsertBeforeAfterComparison"), ".*");
			var zfExpected = new FastZip();
			result = result.Replace("Output", "Expected");
			zfExpected.ExtractZip(result, FileOutput("InsertBeforeAfterComparisonExpect"), ".*");
			FileCompare("InsertBeforeAfterComparison/OEBPS/PartFile00001_.xhtml", "InsertBeforeAfterComparisonExpect/OEBPS/PartFile00001_.xhtml");

		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void ExportDictionaryInsertBeforeAfterFW83Test()
		{
			// clean out old files
			foreach (var file in Directory.GetFiles(_outputPath))
			{
				if (File.Exists(file))
					File.Delete(file);
			}

			const string XhtmlName = "InsertBeforeAfterFW83.xhtml";
			const string CssName = "InsertBeforeAfterFW83.css";
			PublicationInformation projInfo = GetProjInfo(XhtmlName, CssName);
			projInfo.IsReversalExist = false;
			projInfo.ProjectName = "Dictionary Test";
			projInfo.ProjectInputType = "Dictionary";
			projInfo.IsLexiconSectionExist = true;
			File.Copy(FileProg(@"Styles\Dictionary\epub.css"), FileOutput("epub.css"));
			var target = new Exportepub();
			var actual = target.Export(projInfo);
			Assert.IsTrue(actual);
			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			var zf = new FastZip();
			zf.ExtractZip(result, FileOutput("InsertBeforeAfterFW83Comparison"), ".*");
			var zfExpected = new FastZip();
			result = result.Replace("Output", "Expected");
			zfExpected.ExtractZip(result, FileOutput("InsertBeforeAfterFW83Expect"), ".*");
			FileCompare("InsertBeforeAfterFW83Comparison/OEBPS/PartFile00001_.xhtml", "InsertBeforeAfterFW83Expect/OEBPS/PartFile00001_.xhtml");

		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void ExportDictionaryRemoveEntryGUIDFW83Test()
		{
			// clean out old files
			foreach (var file in Directory.GetFiles(_outputPath))
			{
				if (File.Exists(file))
					File.Delete(file);
			}

			const string XhtmlName = "entryguid.xhtml";
			const string CssName = "entryguid.css";
			PublicationInformation projInfo = GetProjInfo(XhtmlName, CssName);
			projInfo.IsReversalExist = false;
			projInfo.ProjectName = "Dictionary Test";
			projInfo.ProjectInputType = "Dictionary";
			projInfo.IsLexiconSectionExist = true;
			File.Copy(FileProg(@"Styles\Dictionary\epub.css"), FileOutput("epub.css"));
			var target = new Exportepub();
			var actual = target.Export(projInfo);
			Assert.IsTrue(actual);
			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			var zf = new FastZip();
			zf.ExtractZip(result, FileOutput("entryguidcomparison"), ".*");
			var zfExpected = new FastZip();
			result = result.Replace("Output", "Expected");
			zfExpected.ExtractZip(result, FileOutput("entryguidexpect"), ".*");
			FileCompare("entryguidcomparison/OEBPS/PartFile00001_.xhtml", "entryguidexpect/OEBPS/PartFile00001_.xhtml");

		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void Epub3ExportTest()
		{
			CleanOutputDirectory();
			string inputDataFolder = Common.PathCombine(_inputPath, "epub3Testcase");
			string outputDataFolder = Common.PathCombine(_outputPath, "epub3Testcase");
			Common.CopyFolderandSubFolder(inputDataFolder, outputDataFolder, true);
			string inputCaseFileName = Common.PathCombine(outputDataFolder, "epub3Testcase.zip");
			var zfExtract = new FastZip();
			zfExtract.ExtractZip(inputCaseFileName, FileOutput("epub3Testcase"), ".*");

			const string xhtmlName = "FrenchHorse.xhtml";
			const string cssName = "FrenchHorse.css";
			var projInfo = new PublicationInformation();
			projInfo.ProjectInputType = "Dictionary";
			projInfo.ProjectPath = outputDataFolder;
			projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDataFolder, xhtmlName);
			projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDataFolder, cssName);
			projInfo.ProjectName = "FrenchHorse";
			Common.Testing = true;
			var exportEpub3 = new Epub3Transformation(this, null) { Epub3Directory = outputDataFolder };
			exportEpub3.Export(projInfo);
			Compress(outputDataFolder, Common.PathCombine(outputDataFolder, "FrenchHorse"));
			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			var zf = new FastZip();
			zf.ExtractZip(result, FileOutput("FrenchHorse"), ".*");
			var zfExpected = new FastZip();
			result = Common.PathCombine(_expectedPath, "FrenchHorse.epub");
			zfExpected.ExtractZip(result, FileOutput("FrenchHorseExpected"), ".*");

			string expectedFilesPath = FileOutput("FrenchHorseExpected");
			expectedFilesPath = Common.PathCombine(expectedFilesPath, "OEBPS");
			string[] filesList = Directory.GetFiles(expectedFilesPath);
			foreach (var fileName in filesList)
			{
				var info = new FileInfo(fileName);
				if (info.Extension == ".html")
				{
					FileCompare("FrenchHorse/OEBPS/" + info.Name, "FrenchHorseExpected/OEBPS/" + info.Name);
				}
			}
		}

		/// <summary>
		/// Test for Creating the toc.ncx File for EPub Output
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void CreateNcxTest()
		{
			// clean out old files
			foreach (var file in Directory.GetFiles(_outputPath))
			{
				if (File.Exists(file))
					File.Delete(file);
			}

			const string XhtmlName = "main.xhtml";
			const string CssName = "main.css";
			PublicationInformation projInfo = GetProjInfo(XhtmlName, CssName);
			projInfo.IsReversalExist = false;
			projInfo.ProjectName = "Dictionary Test";
			projInfo.ProjectInputType = "Dictionary";
			projInfo.IsLexiconSectionExist = true;
			CleanOutputDirectory();
			string inputDataFolder = Common.PathCombine(_inputPath, "CreateNCXTestcase");
			string outputDataFolder = Common.PathCombine(_outputPath, "CreateNCXTestcase");
			Common.CopyFolderandSubFolder(inputDataFolder, outputDataFolder, true);
			string outputDirectory = Path.Combine(outputDataFolder, "OEBPS");
			EpubToc target = new EpubToc(projInfo.ProjectInputType, "2 - Letter and Entry");
			var bookId = new Guid("5C3DF448-BADC-4F83-AF5C-B880027FF079");
			target.CreateNcx(projInfo, outputDirectory, bookId);
			string tocOutputNCXFile = Path.Combine(outputDirectory, "toc.ncx");
			string tocExpectedNCXFile = Path.Combine(_expectedPath, "toc.ncx");

			FileCompare(tocOutputNCXFile, tocExpectedNCXFile);
		}


		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void ScriptureGlossaryExportTest()
		{
			CleanOutputDirectory();
			string inputDataFolder = Common.PathCombine(_inputPath, "GlossaryTestcase");
			string outputDataFolder = Common.PathCombine(_outputPath, "GlossaryTestcase");
			Common.CopyFolderandSubFolder(inputDataFolder, outputDataFolder, true);

			const string xhtmlName = "GlossaryTestCase.xhtml";
			const string cssName = "GlossaryTestCase.css";
			var projInfo = new PublicationInformation();
			projInfo.ProjectInputType = "Scripture";
			projInfo.ProjectPath = outputDataFolder;
			projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDataFolder, xhtmlName);
			projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDataFolder, cssName);
			projInfo.ProjectName = "GlossaryTestCase";
			Common.Testing = true;
			var target = new Exportepub();
			var actual = target.Export(projInfo);
			Assert.IsTrue(actual);


			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			var zf = new FastZip();
			zf.ExtractZip(result, FileOutput("GlossaryTestCase"), ".*");
			var zfExpected = new FastZip();
			result = Common.PathCombine(_expectedPath, "GlossaryTestCaseExpected.epub");
			zfExpected.ExtractZip(result, FileOutput("GlossaryTestCaseExpected"), ".*");

			string expectedFilesPath = FileOutput("GlossaryTestCaseExpected");
			expectedFilesPath = Common.PathCombine(expectedFilesPath, "OEBPS");
			string[] filesList = Directory.GetFiles(expectedFilesPath);
			foreach (var fileName in filesList)
			{
				var info = new FileInfo(fileName);
				if (info.Extension == ".xhtml")
				{
					FileCompare("GlossaryTestCase/OEBPS/" + info.Name, "GlossaryTestCaseExpected/OEBPS/" + info.Name);
				}
			}
		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void ScriptureaaiProjectExportTest()
		{
			CleanOutputDirectory();
			string inputDataFolder = Common.PathCombine(_inputPath, "aaiScriptureExportTest");
			string outputDataFolder = Common.PathCombine(_outputPath, "aaiScriptureExportTest");
			Common.CopyFolderandSubFolder(inputDataFolder, outputDataFolder, true);

			const string xhtmlName = "ebook.xhtml";
			const string cssName = "ebook.css";
			var projInfo = new PublicationInformation();
			projInfo.ProjectInputType = "Scripture";
			projInfo.ProjectPath = outputDataFolder;
			projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDataFolder, xhtmlName);
			projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDataFolder, cssName);
			projInfo.ProjectName = "aaiScriptureExportTest";
			Common.Testing = true;
			var target = new Exportepub();
			var actual = target.Export(projInfo);
			Assert.IsTrue(actual);


			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			var zf = new FastZip();
			zf.ExtractZip(result, FileOutput("aaiScriptureExportTest"), ".*");
			var zfExpected = new FastZip();
			result = Common.PathCombine(_expectedPath, "ebookExpected.epub");
			zfExpected.ExtractZip(result, FileOutput("ebookExpected"), ".*");

			string expectedFilesPath = FileOutput("ebookExpected");
			expectedFilesPath = Common.PathCombine(expectedFilesPath, "OEBPS");
			string[] filesList = Directory.GetFiles(expectedFilesPath);
			foreach (var fileName in filesList)
			{
				var info = new FileInfo(fileName);
				if (info.Extension == ".xhtml")
				{
					FileCompare("aaiScriptureExportTest/OEBPS/" + info.Name, "ebookExpected/OEBPS/" + info.Name);
				}
			}
		}


		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void XPathwayScriptureSettingTest()
		{
			_tf = new TestFiles("epubConvert");
			var pwf = Common.PathCombine(Common.GetAllUserAppPath(), "SIL");
			var zfile = new FastZip();
			string pathwaySettingFolder = Common.PathCombine(pwf, "Pathway");
			Common.CopyFolderandSubFolder(pathwaySettingFolder, pathwaySettingFolder + "test", true);

			zfile.ExtractZip(_tf.Input("Pathway.zip"), pwf, ".*");

			LoadParamValue("Scripture");

			CleanOutputDirectory();
			string inputDataFolder = Common.PathCombine(_inputPath, "ScriptureSettingTest");
			string outputDataFolder = Common.PathCombine(_outputPath, "ScriptureSettingTest");
			Common.CopyFolderandSubFolder(inputDataFolder, outputDataFolder, true);

			const string xhtmlName = "NkontTestCase.xhtml";
			const string cssName = "NkontTestCase.css";
			var projInfo = new PublicationInformation();
			projInfo.ProjectInputType = "Scripture";
			projInfo.ProjectPath = outputDataFolder;
			projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDataFolder, xhtmlName);
			projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDataFolder, cssName);
			projInfo.ProjectName = "NkontTestCase";
			Common.Testing = true;
			var target = new Exportepub();
			var actual = target.Export(projInfo);

			Common.CopyFolderandSubFolder(pathwaySettingFolder + "test", pathwaySettingFolder, true);


			Assert.IsTrue(actual);


			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			var zf = new FastZip();
			zf.ExtractZip(result, FileOutput("NkontTestCase"), ".*");
			var zfExpected = new FastZip();
			result = Common.PathCombine(_expectedPath, "NkontTestCaseExpected.epub");
			zfExpected.ExtractZip(result, FileOutput("NkontTestCaseExpected"), ".*");

			string expectedFilesPath = FileOutput("NkontTestCaseExpected");
			expectedFilesPath = Common.PathCombine(expectedFilesPath, "OEBPS");
			string[] filesList = Directory.GetFiles(expectedFilesPath);
			foreach (var fileName in filesList)
			{
				var info = new FileInfo(fileName);
				if (info.Extension == ".xhtml" && !info.Name.Contains("File2Cpy"))
				{
					FileCompare("NkontTestCase/OEBPS/" + info.Name, "NkontTestCaseExpected/OEBPS/" + info.Name);
				}
			}

		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void XPathwayDictionarySettingTest()
		{
			_tf = new TestFiles("epubConvert");
			var pwf = Common.PathCombine(Common.GetAllUserAppPath(), "SIL");
			string pathwaySettingFolder = Common.PathCombine(pwf, "Pathway");
			Common.CopyFolderandSubFolder(pathwaySettingFolder, pathwaySettingFolder + "test", true);

			var zfile = new FastZip();
			zfile.ExtractZip(_tf.Input("Pathway.zip"), pwf, ".*");

			LoadParamValue("Dictionary");

			CleanOutputDirectory();
			string inputDataFolder = Common.PathCombine(_inputPath, "DictionarySettingTest");
			string outputDataFolder = Common.PathCombine(_outputPath, "DictionarySettingTest");
			Common.CopyFolderandSubFolder(inputDataFolder, outputDataFolder, true);

			const string xhtmlName = "main.xhtml";
			const string cssName = "main.css";
			var projInfo = new PublicationInformation();
			projInfo.ProjectInputType = "Dictionary";
			projInfo.ProjectPath = outputDataFolder;
			projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDataFolder, xhtmlName);
			projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDataFolder, cssName);
			projInfo.ProjectName = "ABSDictionaryTestCase";
			Common.Testing = true;
			var target = new Exportepub();
			var actual = target.Export(projInfo);

			Common.CopyFolderandSubFolder(pathwaySettingFolder + "test", pathwaySettingFolder, true);


			Assert.IsTrue(actual);


			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			var zf = new FastZip();
			zf.ExtractZip(result, FileOutput("main"), ".*");
			var zfExpected = new FastZip();
			result = Common.PathCombine(_expectedPath, "mainExpected.epub");
			zfExpected.ExtractZip(result, FileOutput("mainExpected"), ".*");

			string expectedFilesPath = FileOutput("mainExpected");
			expectedFilesPath = Common.PathCombine(expectedFilesPath, "OEBPS");
			string[] filesList = Directory.GetFiles(expectedFilesPath);
			foreach (var fileName in filesList)
			{
				var info = new FileInfo(fileName);
				if (info.Extension == ".xhtml" && !info.Name.Contains("File2Cpy"))
				{
					FileCompare("main/OEBPS/" + info.Name, "mainExpected/OEBPS/" + info.Name);
				}
			}
		}

		private void LoadParamValue(string inputType)
		{
			// Verifying the input setting file and css file - in Input Folder
			string settingFile = inputType + "StyleSettings.xml";
			string sFileName = Common.PathCombine(_inputPath, settingFile);
			// Common.ProgBase = _outputPath;
			Param.LoadValues(sFileName);
			Param.SetLoadType = inputType;
			//Param.Value["OutputPath"] = _outputPath;
			//Param.Value["UserSheetPath"] = _outputPath;
		}

		private void CleanOutputDirectory()
		{
			Common.DeleteDirectory(_outputPath);
		}

		private void FileCompare(string file1, string file2)
		{
			string xhtmlOutput = FileOutput(file1);
			string xhtmlExpected = FileOutput(file2);
            XmlAssert.Ignore(xhtmlOutput, "//*[@name='epub-creator']/@content", null);
			XmlAssert.AreEqual(xhtmlOutput, xhtmlExpected, file1 + " in epub ");
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
