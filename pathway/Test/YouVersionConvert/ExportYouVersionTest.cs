// --------------------------------------------------------------------------------------------
// <copyright file="ExportYouVersionTest.cs" from='2011' to='2011' company='SIL International'>
//      Copyright © 2011, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Test methods of ExportYouVersion
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System.IO;
using SIL.PublishingSolution;
using System.Xml;
using SIL.Tool;
using NUnit.Framework;
#endregion Using

namespace Test.YouVersionConvert
{
    /// <summary>
    ///This is a test class for ExportYouVersionTest and is intended
    ///to contain all ExportYouVersionTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ExportYouVersionTest : ExportYouVersion
    {
        #region Setup

        private TestFiles _testFiles;

        [TestFixtureSetUp]
        public void Setup()
        {
            _testFiles = new TestFiles("YouVersionConvert");
        }
        #endregion Setup

        /// <summary>
        ///A test for ZipHtml
        ///</summary>
        [Test]
        public void ZipHtmlTest()
        {
            const string testName = "ZipHtmlTest";
            LoadParam(testName);
            string htmlFolder = _testFiles.Output("html");
            FolderTree.Copy(_testFiles.Input("html"), htmlFolder);
            string expected = _testFiles.Output(Path.Combine(testName, "html.zip"));
            string actual = ZipIt(htmlFolder);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for WriteChapter
        ///</summary>
        [Test]
        public void WriteChapterTest()
        {
            var chapterFile = new XmlDocument { XmlResolver = null };
            chapterFile.LoadXml(XhtmlTemplate);
            string processFolder = _testFiles.Output(null);
            string bookCode = "MAT";
            string curChapter = "1";
            WsCode = "nko";
            WriteChapter(chapterFile, processFolder, bookCode, curChapter);
            const string fileName = "MAT.1.xhtml";
            FileAssert.AreEqual(_testFiles.Expected(fileName), _testFiles.Output(fileName));
        }

        [Test]
        public void OneChapterPerSectionTest()
        {
            const string fileName = "nko-1Ti.xhtml";
            string inputFullName = _testFiles.Output(fileName);
            const bool overwrite = true;
            File.Copy(_testFiles.Input(fileName), inputFullName, overwrite);
            string expected = _testFiles.Expected(fileName.Replace(".xhtml", "_ospc.xhtml"));
            string actual = OneChapterPerSection(inputFullName);
            FileAssert.AreEqual(expected,actual);
        }

        /// <summary>
        ///A test for SplitByChapters
        ///</summary>
        [Test]
        public void SplitByChaptersTest()
        {
            const string fileName = "nko-1Ti_ospc.xhtml";
            string input = _testFiles.Output(fileName);
            const bool overwrite = true;
            File.Copy(_testFiles.Input(fileName), input, overwrite);
            string expected = _testFiles.Output("xhtml");
            string actual = SplitByChapters(input);
            Assert.AreEqual(expected, actual);
            DirectoryInfo directoryInfo = new DirectoryInfo(actual);
            Assert.AreEqual(6, directoryInfo.GetFiles().Length);
            const string chapter6 = "1Tim.6.xhtml";
            FileAssert.AreEqual(_testFiles.Expected(chapter6), Path.Combine(actual,chapter6));
        }

        /// <summary>
        ///A test for SplitByChapters
        ///</summary>
        [Test]
        public void SplitByChaptersAchi2CoTest()
        {
            const string fileName = "achiDraft_ospc.xhtml";
            string input = _testFiles.Output(fileName);
            const bool overwrite = true;
            File.Copy(_testFiles.Input(fileName), input, overwrite);
            string expected = _testFiles.Output("xhtml");
            string actual = SplitByChapters(input);
            Assert.AreEqual(expected, actual);
            DirectoryInfo directoryInfo = new DirectoryInfo(actual);
            Assert.AreEqual(13, directoryInfo.GetFiles().Length);
            const string chapter7 = "2Cor.7.xhtml";
            FileAssert.AreEqual(_testFiles.Expected(chapter7), Path.Combine(actual, chapter7));
        }

        /// <summary>
        ///A test for SetupOutputFolder
        ///</summary>
        [Test]
        public void SetupOutputFolderTest()
        {
            const string testFolder = "SetupOutputFolder";
            string folder = _testFiles.Output(testFolder);
            SetupOutputFolder(folder);
            Assert.IsTrue(Directory.Exists(folder));
            DirectoryInfo directoryInfo = new DirectoryInfo(folder);
            Assert.AreEqual(0, directoryInfo.GetFiles().Length);
        }

        /// <summary>
        ///A test for NestBookData
        ///</summary>
        [Test]
        public void NestBookDataTest()
        {
            const string fileName = "nko-1Ti-flat.xhtml";
            string inputFullName = _testFiles.Output(fileName);
            const bool overwrite = true;
            File.Copy(_testFiles.Input(fileName), inputFullName, overwrite);
            NestBookData(inputFullName);
            string expected = _testFiles.Expected(fileName);
            FileAssert.AreEqual(expected, inputFullName);
        }

        /// <summary>
        ///A test for Handle
        ///</summary>
        [Test]
        public void HandleTest()
        {
            bool actual = Handle("Dictionary");
            const bool expectedDictionary = false;
            Assert.AreEqual(expectedDictionary, actual);
            actual = Handle("Scripture");
            const bool expectedScripture = true;
            Assert.AreEqual(expectedScripture, actual);
        }

        /// <summary>
        ///A test for GetPathFromXhtml
        ///</summary>
        [Test]
        public void GetPathFromXhtmlTest()
        {
            const string fileName = "nko-1Ti.xhtml";
            string name = _testFiles.Input(fileName);
            const string path = "//xhtml:span[@class='Chapter_Number']";
            XmlNodeList actual = GetPathFromXhtml(name, path);
            Assert.AreEqual(6, actual.Count);
        }

        /// <summary>
        ///A test for GetXmlDocument
        ///</summary>
        [Test]
        public void GetXmlDocumentTest()
        {
            const string fileName = "nko-1Ti.xhtml";
            string name = _testFiles.Input(fileName);
            XmlDocument actual = GetXmlDocument(name);
            Assert.AreEqual(396, actual.SelectNodes("//descendant::*").Count);
        }

        /// <summary>
        ///A test for Convert to Html
        ///</summary>
        [Test]
        public void ConvertHtmlTest()
        {
            string processFolder = _testFiles.Output("xhtml");
            FolderTree.Copy(_testFiles.Input("xhtml"), processFolder);
            string outFolder = _testFiles.Output("zipHtml");
            SetupOutputFolder(outFolder);
            const string xslt = "TE_XHTML-to-YouVersion_OneChapter_HTML.xslt";
            const string type = "Html";
            const string ext = ".html";
            Convert(processFolder, outFolder, xslt, type, ext);
            string actualOutFolder = Path.Combine(outFolder, type);
            DirectoryInfo directoryInfo = new DirectoryInfo(actualOutFolder);
            Assert.AreEqual(6, directoryInfo.GetFiles().Length);
            const string chapter6 = "nko.1TI.6.html";
            FileAssert.AreEqual(_testFiles.Expected(chapter6), Path.Combine(actualOutFolder,chapter6));
        }

        /// <summary>
        ///A test for Convert to Sql
        ///</summary>
        [Test]
        public void ConvertSqlTest()
        {
            string processFolder = _testFiles.Output("xhtml");
            FolderTree.Copy(_testFiles.Input("xhtml"), processFolder);
            string outFolder = _testFiles.Output("zipSql");
            SetupOutputFolder(outFolder);
            const string xslt = "TE_XHTML-to-YouVersion_OneChapter_SQL.xslt";
            const string type = "Sql";
            const string ext = ".sql";
            Convert(processFolder, outFolder, xslt, type, ext);
            string actualOutFolder = Path.Combine(outFolder, type);
            DirectoryInfo directoryInfo = new DirectoryInfo(actualOutFolder);
            Assert.AreEqual(6, directoryInfo.GetFiles().Length);
            const string chapter6 = "nko.1TI.6.sql";
            FileAssert.AreEqual(_testFiles.Expected(chapter6), Path.Combine(actualOutFolder, chapter6));
        }

        /// <summary>
        ///A test for CleanUp
        ///</summary>
        [Test]
        public void CleanUpTest()
        {
            string processFolder = _testFiles.Output("xhtml");
            string htmlFolder = _testFiles.Output("Html");
            string zippedResult = _testFiles.Output(Path.Combine("ZipHtmlTest", "html.zip"));
            CleanUp(processFolder, htmlFolder, zippedResult);
            Assert.IsFalse(Directory.Exists(processFolder));
            Assert.IsFalse(Directory.Exists(htmlFolder));
        }

        /// <summary>
        ///A test for BookDataNested
        ///</summary>
        [Test]
        public void BookDataNestedTest()
        {
            string inputFullName = _testFiles.Input("nko-1Ti.xhtml");
            bool actual = BookDataNested(inputFullName);
            Assert.IsTrue(actual);
            inputFullName = _testFiles.Input("nko-1Ti-flat.xhtml");
            actual = BookDataNested(inputFullName);
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for AddBookLevelParagraph
        ///</summary>
        [Test]
        public void AddBookLevelParagraphTest()
        {
            XmlDocument chapterFile = new XmlDocument {XmlResolver = null};
            chapterFile.LoadXml(XhtmlTemplate);
            var chapterNs = GetXmlNamespaceManager(chapterFile);
            const string classValue = "scrBookCode";
            const string content = "1TI";
            XmlNode chapterFileBody = chapterFile.SelectSingleNode("//xhtml:body", chapterNs);
            AddBookLevelParagraph(chapterFile, classValue, content, chapterFileBody);
            const string fileName = "AddP.xhtml";
            string fullName = _testFiles.Output(fileName);
            var xmlWriterSettings = new XmlWriterSettings { Indent = true };
            XmlWriter xmlWriter = XmlWriter.Create(fullName, xmlWriterSettings);
            chapterFile.WriteTo(xmlWriter);
            xmlWriter.Close();
            FileAssert.AreEqual(_testFiles.Expected(fullName), fullName);
        }

        private void LoadParam(string outputName)
        {
            const bool overwrite = true;
            const string schemaFile = "StyleSettings.xsd";
            File.Copy(_testFiles.Input(schemaFile), _testFiles.Output(schemaFile), overwrite);
            const string settingFile = "ScriptureStyleSettings.xml";
            string sFileName = _testFiles.Output(settingFile);
            File.Copy(_testFiles.Input(settingFile), sFileName, overwrite);
            Common.ProgBase = _testFiles.Output(null);
            Param.LoadValues(sFileName);
            Param.SetLoadType = "Scripture";
            Param.Value[Param.PublicationLocation] = Common.PathCombine(Common.ProgBase, outputName);
            Param.Value[Param.OutputPath] = Common.ProgBase;
            Param.Value[Param.UserSheetPath] = Common.ProgBase;
        }
    }
}
