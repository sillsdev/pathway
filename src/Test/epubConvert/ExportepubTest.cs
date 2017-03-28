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
			_expectedPath = Common.PathCombine(testPath, Common.UsingMonoVM ? "ExpectedLinux" : "Expected");
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
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse};
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
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse };
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
			ExtractzipFilesBasedOnOS(result, FileOutput("main"));
			var resultDoc = Common.DeclareXMLDocument(false);
			resultDoc.Load(FileOutput(Common.DirectoryPathReplace("main/OEBPS/PartFile00001_.xhtml")));
			var nsmgr = new XmlNamespaceManager(resultDoc.NameTable);
			nsmgr.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			//1.19
			string xPath = "//xhtml:ul[@class='footnotes']/xhtml:li[@id='FN_Footnote-LUK-6']";
			XmlNode node = resultDoc.SelectSingleNode(xPath, nsmgr);
			if (!Common.UsingMonoVM)
			{
				Assert.AreEqual("[b]Bahasa Yunani bilang “Badiri di Allah pung muka”. Ini bisa pung arti ‘Karja par Tuhan’. Mar bisa pung arti lai ‘Badiri di Allah pung muka’. Malekat yang badiri di Allah pung muka pung kuasa labe dari malekat laeng. Jadi, Gabriel bukang malekat biasa.", node.InnerText.Trim());
			}
			//1.27
			xPath = "//xhtml:ul[@class='footnotes']/xhtml:li[@id='FN_Footnote-LUK-7']";
			node = resultDoc.SelectSingleNode(xPath, nsmgr);
			Assert.AreEqual("1.27 1.27 Mat. 1:18", node.InnerText.Trim());
			//1.32-33
			xPath = "//xhtml:ul[@class='footnotes']/xhtml:li[@id='FN_Footnote-LUK-9']";
			node = resultDoc.SelectSingleNode(xPath, nsmgr);
			Assert.AreEqual("1.32-33 1.32-33 2Sam. 7:12, 13, 16; Yes. 9:6", node.InnerText.Trim());
			//2.41
			xPath = "//xhtml:ul[@class='footnotes']/xhtml:li[@id='FN_Footnote-LUK-22']";
			node = resultDoc.SelectSingleNode(xPath, nsmgr);
			Assert.AreEqual("[c]2.41 Hari basar Paska Yahudi tu, orang Yahudi inga waktu dong pung tete nene moyang kaluar dari negara Mesir. Dolo dong jadi orang suru-suru di tampa tu, mar Allah kasi kaluar dong la bawa dong ka tana yang Antua su janji par dong.", node.InnerText.Trim());
		}

		[Test]
		public void ReplaceEmptyHrefandXmlLangtoLangChangeTest()
		{
			CleanOutputDirectory();
			const string folderName = "RemoveEmptyHrefTest";
			FolderTree.Copy(FileInput(folderName), FileOutput(folderName));
			ReplaceEmptyHrefandXmlLangtoLang(FileOutput(folderName));
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
			string file = Common.PathCombine(Common.GetPSApplicationPath(), fileName);
			if (!File.Exists(file))
			{
				file = Common.PathCombine(Path.GetDirectoryName(Common.AssemblyPath), fileName);
			}
			return file;
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
			ExtractzipFilesBasedOnOS(result, FileOutput("EpubIndentFileComparison"));
			string expPath = Common.UsingMonoVM ? "ExpectedLinux" : "Expected";
			result = result.Replace("Output", expPath);
			ExtractzipFilesBasedOnOS(result, FileOutput("EpubIndentFileComparisonExpect"));
			FileCompare("EpubIndentFileComparison/OEBPS/PartFile00001_.xhtml", "EpubIndentFileComparisonExpect/OEBPS/PartFile00001_.xhtml");

		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void FootNoteMarker_RQ_Test()
		{
			const string FolderName = "FootnoteMarker_RQ";

			// clean out old files
			foreach (var file in Directory.GetFiles(_outputPath))
			{
				if (File.Exists(file))
					File.Delete(file);
			}
			FolderTree.Copy(FileInput(FolderName), FileOutput(FolderName));
			PublicationInformation projInfo = GetProjInfo(FileOutput(FolderName), true);
			projInfo.ProjectName = "RQMarkerTest";
			projInfo.ProjectInputType = "Dictionary";
			projInfo.IsLexiconSectionExist = true;
			//File.Copy(FileProg(@"Styles\Dictionary\epub.css"), FileOutput("epub.css"));
			var target = new Exportepub();
			var actual = target.Export(projInfo);
			Assert.IsTrue(actual);
			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			ExtractzipFilesBasedOnOS(result, FileOutput(FolderName));

			string outputFileName = FileOutput(FolderName);
			outputFileName = Path.Combine(outputFileName, "OEBPS");


			XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
			XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse };
			var filePath = Common.PathCombine(outputFileName, "PartFile00001_.xhtml");
			XmlReader xmlReader = XmlReader.Create(FileOutput(filePath), xmlReaderSettings);
			xmlDocument.Load(xmlReader);
			xmlReader.Close();
			XmlNodeList nodes = xmlDocument.SelectNodes(".//xhtml:div", namespaceManager);
			Assert.AreEqual(3, nodes.Count, "Should be 3 divs");

			Assert.AreEqual(nodes[0].InnerText, "E e", "Not matched text content");
			Assert.AreEqual(nodes[1].InnerText, "Entry fr. var. of VariantTest [Tam] Sum1 Green ", "Not matched text content");
			Assert.AreEqual(nodes[2].InnerText, "Entry fr. var. of VariantTest [Tam] Sum1 Green ", "Not matched text content");

			xmlDocument = Common.DeclareXMLDocument(true);
			namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse };
			filePath = Common.PathCombine(outputFileName, "RevIndex00001_.xhtml");
			XmlReader xmlReader2 = XmlReader.Create(FileOutput(filePath), xmlReaderSettings);
			xmlDocument.Load(xmlReader2);
			xmlReader.Close();
			nodes = xmlDocument.SelectNodes(".//xhtml:div", namespaceManager);
			Assert.AreEqual(3, nodes.Count, "Should be 3 divs");

			Assert.AreEqual(nodes[0].InnerText, "T t", "Not matched text content");
			Assert.AreEqual(nodes[1].InnerText, "Tested adj Entry (fr. var. of VariantTest [TamTamil] Sum1 Green) ", "Not matched text content");
			Assert.AreEqual(nodes[2].InnerText, "Tested adj Entry (fr. var. of VariantTest [TamTamil] Sum1 Green) ", "Not matched text content");

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
			ExtractzipFilesBasedOnOS(result, FileOutput("InsertBeforeAfterComparison"));
			string expPath = Common.UsingMonoVM ? "ExpectedLinux" : "Expected";
			result = result.Replace("Output", expPath);
			ExtractzipFilesBasedOnOS(result, FileOutput("InsertBeforeAfterComparisonExpect"));
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
			ExtractzipFilesBasedOnOS(result, FileOutput("InsertBeforeAfterFW83Comparison"));
			string expPath = Common.UsingMonoVM ? "ExpectedLinux" : "Expected";
			result = result.Replace("Output", expPath);
			ExtractzipFilesBasedOnOS(result, FileOutput("InsertBeforeAfterFW83Expect"));
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
			ExtractzipFilesBasedOnOS(result, FileOutput("entryguidcomparison"));
			string expPath = Common.UsingMonoVM ? "ExpectedLinux" : "Expected";
			result = result.Replace("Output", expPath);
			ExtractzipFilesBasedOnOS(result, FileOutput("entryguidexpect"));
			FileCompare("entryguidcomparison/OEBPS/PartFile00001_.xhtml", "entryguidexpect/OEBPS/PartFile00001_.xhtml");

		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void ReferenceFontsTest()
		{
			// clean out old files
			foreach (var file in Directory.GetFiles(_outputPath))
			{
				if (File.Exists(file))
					File.Delete(file);
			}
			Common.Testing = true;
			const string XhtmlName = "ReferenceFonts.xhtml";
			const string CssName = "ReferenceFonts.css";
			PublicationInformation projInfo = GetProjInfo(XhtmlName, CssName);
			projInfo.IsReversalExist = false;
			projInfo.ProjectName = "CSS Font Test";
			projInfo.ProjectInputType = "Dictionary";
			projInfo.IsLexiconSectionExist = true;
			File.Copy(FileInput(CssName), FileOutput(CssName), true);
			var parent = new Exportepub();
			parent.EmbedFonts = true;
			var target = new EpubFont(parent);
			var langArray = target.InitializeLangArray(projInfo);
			target.BuildFontsList();
			if (target.EmbedAllFonts(langArray, FileOutput("")))
			{
				target.ReferenceFonts(FileOutput(CssName), projInfo);
				TextFileAssert.AreEqualEx(FileExpected(CssName), FileOutput(CssName), new ArrayList { 4, 8, 12});
			}
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
			ExtractzipFilesBasedOnOS(inputCaseFileName, FileOutput("epub3Testcase"));
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
			ExtractzipFilesBasedOnOS(result, FileOutput("FrenchHorse"));
			result = Common.PathCombine(_expectedPath, "FrenchHorse.epub");
			ExtractzipFilesBasedOnOS(result, FileOutput("FrenchHorseExpected"));

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
			var tdoc = new XmlDocument();
			var reader = XmlReader.Create(tocOutputNCXFile, new XmlReaderSettings { XmlResolver = null });
			tdoc.Load(reader);
			reader.Close();
			var srcNodes = tdoc.SelectNodes(@"//*[starts-with(@src,'Part')]/@src");
			Assert.AreEqual(21, srcNodes.Count);
			Assert.AreEqual("PartFile00001_.xhtml#gca26b453-4696-4aa1-9e28-1f5121e9b066", srcNodes[2].InnerText);
			var textNodes = tdoc.SelectNodes("//*[starts-with(@src,'Part')]/preceding-sibling::*[1]/*");
			Assert.AreEqual("waain ", textNodes[2].InnerText);
			tdoc.DocumentElement.RemoveAll();
		}

		/// <summary>
		/// Test for Creating the toc.ncx File for EPub Output - Level 1
		/// TOC Level - 1. Letter Only
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void EPubTOCCreationTest_Level1_FW83()
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
			string inputDataFolder = Common.PathCombine(_inputPath, "EPubTOCCreationTest_Level1_FW83");
			string outputDataFolder = Common.PathCombine(_outputPath, "EPubTOCCreationTest_Level1_FW83");
			string expectedDataFolder = Common.PathCombine(_expectedPath, "EPubTOCCreationTest_Level1_FW83");
			string inputDirectory = Path.Combine(inputDataFolder, "OEBPS");
			string outputDirectory = Path.Combine(outputDataFolder, "OEBPS");
			string expectedDirectory = Path.Combine(expectedDataFolder, "OEBPS");
			Common.CopyFolderandSubFolder(inputDirectory, outputDirectory, true);
			EpubToc target = new EpubToc(projInfo.ProjectInputType, "1 - Letter Only");
			var bookId = new Guid("1CA29FE3-C044-405F-82AA-F6F577999125");
			target.CreateNcx(projInfo, outputDirectory, bookId);
			string tocOutputNCXFile = Path.Combine(outputDirectory, "toc.ncx");
			string tocExpectedNCXFile = Path.Combine(expectedDirectory, "toc.ncx");

			XmlAssert.AreEqual(tocOutputNCXFile, tocExpectedNCXFile, "NCX Files not Matching");
		}

		/// <summary>
		/// Test for Creating the toc.ncx File for EPub Output - Level 2
		/// TOC Level - 2. Letter and Entry
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void EPubTOCCreationTest_Level2_FW83()
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
			string inputDataFolder = Common.PathCombine(_inputPath, "EPubTOCCreationTest_Level2_FW83");
			string outputDataFolder = Common.PathCombine(_outputPath, "EPubTOCCreationTest_Level2_FW83");
			string expectedDataFolder = Common.PathCombine(_expectedPath, "EPubTOCCreationTest_Level2_FW83");
			string inputDirectory = Path.Combine(inputDataFolder, "OEBPS");
			string outputDirectory = Path.Combine(outputDataFolder, "OEBPS");
			string expectedDirectory = Path.Combine(expectedDataFolder, "OEBPS");
			Common.CopyFolderandSubFolder(inputDirectory, outputDirectory, true);
			EpubToc target = new EpubToc(projInfo.ProjectInputType, "2 - Letter and Entry");
			var bookId = new Guid("A9168BD8-E76E-43F3-97FC-4688CBFA82DA");
			target.CreateNcx(projInfo, outputDirectory, bookId);
			string tocOutputNCXFile = Path.Combine(outputDirectory, "toc.ncx");
			string tocExpectedNCXFile = Path.Combine(expectedDirectory, "toc.ncx");

			XmlAssert.AreEqual(tocOutputNCXFile, tocExpectedNCXFile, "NCX Files not Matching");
		}

		/// <summary>
		/// Test for Creating the toc.ncx File for EPub Output - Level 3
		/// TOC Level - 3. Letter, Entry and Sense
		/// </summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void EPubTOCCreationTest_Level3_FW83()
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
			string inputDataFolder = Common.PathCombine(_inputPath, "EPubTOCCreationTest_Level3_FW83");
			string outputDataFolder = Common.PathCombine(_outputPath, "EPubTOCCreationTest_Level3_FW83");
			string expectedDataFolder = Common.PathCombine(_expectedPath, "EPubTOCCreationTest_Level3_FW83");
			string inputDirectory = Path.Combine(inputDataFolder, "OEBPS");
			string outputDirectory = Path.Combine(outputDataFolder, "OEBPS");
			string expectedDirectory = Path.Combine(expectedDataFolder, "OEBPS");
			Common.CopyFolderandSubFolder(inputDirectory, outputDirectory, true);
			EpubToc target = new EpubToc(projInfo.ProjectInputType, "3 - Letter, Entry and Sense");
			var bookId = new Guid("78239FF9-73E9-4B6A-8CDF-2074BFFFAF4E");
			target.CreateNcx(projInfo, outputDirectory, bookId);
			string tocOutputNCXFile = Path.Combine(outputDirectory, "toc.ncx");
			string tocExpectedNCXFile = Path.Combine(expectedDirectory, "toc.ncx");

			XmlAssert.AreEqual(tocOutputNCXFile, tocExpectedNCXFile, "NCX Files not Matching");
		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void CreateNcxReversalTest()
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
			projInfo.IsReversalExist = true;
			projInfo.ProjectName = "Dictionary Test";
			projInfo.ProjectInputType = "Dictionary";
			projInfo.IsLexiconSectionExist = true;
			CleanOutputDirectory();
			string inputDataFolder = Common.PathCombine(_inputPath, "CreateNCXTestcaseWithReversal");
			string outputDataFolder = Common.PathCombine(_outputPath, "CreateNCXTestcaseWithReversal");
			Common.CopyFolderandSubFolder(inputDataFolder, outputDataFolder, true);
			string outputDirectory = Path.Combine(outputDataFolder, "OEBPS");
			EpubToc target = new EpubToc(projInfo.ProjectInputType, "2 - Letter and Entry");
			var bookId = new Guid("5C3DF448-BADC-4F83-AF5C-B880027FF079");
			target.CreateNcx(projInfo, outputDirectory, bookId);
			string tocOutputNCXFile = Path.Combine(outputDirectory, "toc.ncx");
			var tdoc = new XmlDocument();
			var reader = XmlReader.Create(tocOutputNCXFile, new XmlReaderSettings {XmlResolver = null});
			tdoc.Load(reader);
			reader.Close();
			var revSrcNodes = tdoc.SelectNodes(@"//*[starts-with(@src,'Rev')]/@src");
			Assert.AreEqual(12, revSrcNodes.Count);
			Assert.AreEqual("RevIndex00001_.xhtml#g1396f97c-3ab2-44c5-971d-a568218c7b13", revSrcNodes[2].InnerText);
			var revTextNodes = tdoc.SelectNodes("//*[starts-with(@src,'Rev')]/preceding-sibling::*[1]/*");
			Assert.AreEqual("\x905\x915\x94d\x937\x924\x92c.\xa0", revTextNodes[2].InnerText);
			tdoc.DocumentElement.RemoveAll();
		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void ScriptureGlossaryExportTest()
		{
			CleanOutputDirectory();
			string inputDataFolder = Common.PathCombine(_inputPath, "GlossaryTestcase");
			string outputDataFolder = Common.PathCombine(_outputPath, "GlossaryTestCase");
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
			ExtractzipFilesBasedOnOS(result, FileOutput("GlossaryTestCase"));
			result = Common.PathCombine(_expectedPath, "GlossaryTestCaseExpected.epub");
			ExtractzipFilesBasedOnOS(result, FileOutput("GlossaryTestCaseExpected"));

			string expectedFilesPath = FileOutput("GlossaryTestCaseExpected");
			expectedFilesPath = Common.PathCombine(expectedFilesPath, "OEBPS");
			string[] filesList = Directory.GetFiles(expectedFilesPath);
			foreach (var fileName in filesList)
			{
				var info = new FileInfo(fileName);
				if (info.Extension == ".xhtml" && info.Name.ToLower().Contains("part"))
				{
					FileCompare("GlossaryTestCase/OEBPS/" + info.Name, "GlossaryTestCaseExpected/OEBPS/" + info.Name);
				}
			}
		}

		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void aaiScriptureProjectExportTest()
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
			ExtractzipFilesBasedOnOS(result, FileOutput("aaiScriptureExportTest"));
			result = Common.PathCombine(_expectedPath, "ebookExpected.epub");
			ExtractzipFilesBasedOnOS(result, FileOutput("ebookExpected"));

			string expectedFilesPath = FileOutput("ebookExpected");
			expectedFilesPath = Common.PathCombine(expectedFilesPath, "OEBPS");
			string[] filesList = Directory.GetFiles(expectedFilesPath);
			foreach (var fileName in filesList)
			{
				var info = new FileInfo(fileName);
				if (info.Extension == ".xhtml" && info.Name.ToLower().Contains("part"))
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
			string pathwaySettingFolder = Common.PathCombine(pwf, "Pathway");
			Common.CopyFolderandSubFolder(pathwaySettingFolder, pathwaySettingFolder + "test", true);

			ExtractzipFilesBasedOnOS(_tf.Input("Pathway.zip"), pwf);

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
			ExtractzipFilesBasedOnOS(result, FileOutput("NkontTestCase"));
			result = Common.PathCombine(_expectedPath, "NkontTestCaseExpected.epub");
			ExtractzipFilesBasedOnOS(result, FileOutput("NkontTestCaseExpected"));

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

			if (Directory.Exists(pathwaySettingFolder))
			{
				Common.CopyFolderandSubFolder(pathwaySettingFolder, pathwaySettingFolder + "test", true);
			}

			ExtractzipFilesBasedOnOS(_tf.Input("Pathway.zip"), pwf);

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
			projInfo.IsLexiconSectionExist = true;
			Common.Testing = true;
			var target = new Exportepub();
			var actual = target.Export(projInfo);

			Common.CopyFolderandSubFolder(pathwaySettingFolder + "test", pathwaySettingFolder, true);


			Assert.IsTrue(actual);


			var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
			ExtractzipFilesBasedOnOS(result, FileOutput("main"));
			result = Common.PathCombine(_expectedPath, "mainExpected.epub");
			ExtractzipFilesBasedOnOS(result, FileOutput("mainExpected"));

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

		/// <summary>
		/// Create a simple PublicationInformation instance
		/// </summary>
		private PublicationInformation GetProjInfo(string exportDirecotry, bool reversalExist)
		{
			PublicationInformation projInfo = new PublicationInformation();

			projInfo.DefaultXhtmlFileWithPath = Path.Combine(exportDirecotry, "main.xhtml");
			projInfo.DefaultCssFileWithPath = Path.Combine(exportDirecotry, "main.css");
			projInfo.DictionaryPath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
			projInfo.IsOpenOutput = false;
			projInfo.IsReversalExist = reversalExist;
			if (reversalExist)
			{
				projInfo.DefaultRevCssFileWithPath = Path.Combine(exportDirecotry, "FlexRev.css");
			}

			return projInfo;
		}

		private void ExtractzipFilesBasedOnOS(string result, string outputFilePath)
		{
			if (Common.UsingMonoVM)
			{
				if (outputFilePath == null) return;
				var outputFileNamePathWithoutExtension = Path.Combine(Path.GetDirectoryName(outputFilePath),
						Path.GetFileNameWithoutExtension(outputFilePath));
				if (Path.GetExtension(outputFilePath) != "zip")
				{
					File.Copy(result, outputFileNamePathWithoutExtension + ".zip");
				}
				ZipUtil.UnZipFiles(outputFileNamePathWithoutExtension + ".zip", outputFileNamePathWithoutExtension, "", true);
			}
			else
			{
				var zf = new FastZip();
				zf.ExtractZip(result, outputFilePath, ".*");
			}
		}
		#endregion PrivateFunctions
	}
}
