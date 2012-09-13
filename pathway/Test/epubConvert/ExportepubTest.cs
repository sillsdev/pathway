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
using System.Xml;
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
    public class ExportepubTest: Exportepub
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
            projInfo.ProjectInputType = "Dictionary";
            projInfo.DefaultRevCssFileWithPath = Path.Combine(_inputPath, "FlexRev.css");
            projInfo.ProjectName = "EBook (epub)_" + DateTime.Now.Date.ToShortDateString() + "_" +
                                   DateTime.Now.Date.ToShortTimeString();
            var target = new Exportepub();
            var actual = target.Export(projInfo);
            Assert.IsTrue(actual);
            var result = projInfo.DefaultXhtmlFileWithPath.Replace(".xhtml", ".epub");
            var zf = new FastZip();
            zf.ExtractZip(result, FileOutput("main"),".*");
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
        public void AExportScripturePassTest()
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
            _inputType = "scripture";
            InsertChapterLinkBelowBookName(FileOutput(FolderName));
            XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false };
            var filePath = Path.Combine(FolderName, "PartFile00001_.xhtml");
            XmlReader xmlReader = XmlReader.Create(FileOutput(filePath), xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            XmlNodeList nodes = xmlDocument.SelectNodes(".//xhtml:a", namespaceManager);
            Assert.AreEqual(3, nodes.Count, "Should be 3 chapter links for Titus");
            var next = nodes[nodes.Count - 1].NextSibling.NextSibling;
            Assert.AreEqual("Section_Head", next.Attributes.GetNamedItem("class").InnerText, "Section head should follow chapter links");
        }

        [Test]
        public void ChapterLinkForSingleChapterTest()
        {
            const string FolderName = "ChapterLinkTest";
            FolderTree.Copy(FileInput(FolderName), FileOutput(FolderName));
            _inputType = "scripture";
            InsertChapterLinkBelowBookName(FileOutput(FolderName));
            XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            XmlReaderSettings xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false };
            var filePath = Path.Combine(FolderName, "PartFile00002_.xhtml");
            XmlReader xmlReader = XmlReader.Create(FileOutput(filePath), xmlReaderSettings);
            xmlDocument.Load(xmlReader);
            xmlReader.Close();
            XmlNodeList nodes = xmlDocument.SelectNodes(".//xhtml:a", namespaceManager);
            Assert.AreEqual(0, nodes.Count, "Should be 0 chapter links for Philemon");
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
