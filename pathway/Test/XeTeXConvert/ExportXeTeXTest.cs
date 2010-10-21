// --------------------------------------------------------------------------------------------
// <copyright file="ExportXeTeXTest.cs" from='2010' to='2010' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Test methods of ExportXeTeX
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections;
using System.IO;
using NMock2;
using NUnit.Framework;
using SIL.Tool;
using System.Collections.Generic;
using SIL.PublishingSolution;
#endregion Using

namespace Test.XeTeXConvert
{
    /// <summary>
    ///This is a test class for ExportXeTeXTest and is intended
    ///to contain all ExportXeTeXTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ExportXeTeXTest : ExportXeTeX
    {
        #region Private Variables
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;
        private readonly Mockery mocks = new Mockery();
        #endregion Private Variables

        #region Setup
        [TestFixtureSetUp]
        public void Setup()
        {
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../PsSupport");
            Common.SupportFolder = "";
			Common.ProgBase = Common.ProgInstall;
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/XeTeXConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "expected");
            if (Directory.Exists(_outputPath))
                Directory.Delete(_outputPath, true);
            Directory.CreateDirectory(_outputPath);
        }
        #endregion Setup

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
            return projInfo;
        }

        #endregion PrivateFunctions

        /// <summary>
        ///A test for ExportType
        ///</summary>
        [Test]
        public void ExportTypeTest()
        {
            ExportXeTeX target = new ExportXeTeX();
            string actual;
            actual = target.ExportType.Substring(0,5);
            Assert.AreEqual("XeTeX", actual);
        }

        /// <summary>
        ///A test for SetupStartScript
        ///</summary>
        [Test]
        public void SetupStartScriptTest()
        {
            const string ScriptTemplate = "startXPWtool-tpl.bat";
            string supportTemplate = Common.PathCombine("xetexPathway", ScriptTemplate);
            File.Copy(FileProg(supportTemplate), FileOutput(ScriptTemplate), true);
            string deToolFolder = _outputPath;
            SetupStartScript(deToolFolder);
            const string ActualScript = "startXPWtool.bat";
            TextFileAssert.AreEqualEx(FileExpected(ActualScript), FileOutput(ActualScript), new ArrayList {5, 6});
        }

        /// <summary>
        ///A test for XPWtool
        ///</summary>
        [Test]
        public void XPWtoolTest()
        {
            const string toolName = "XPWtool.bat";
            const string toolCommand = "XPWtoolc.bat";
            TextReader textReader = new StreamReader(FileProg(Common.PathCombine("xetexPathway", toolName)));
            string data = textReader.ReadToEnd();
            textReader.Close();
            Assert.GreaterOrEqual(data.IndexOf(toolCommand), 0);
        }

        /// <summary>
        /// check for dictionary settings
        /// </summary>
        [Test]
        public void DictionaryPropertyTest()
        {
            const string cssName = "DictionaryTemp1.css";
            string cssFullPath = FileInput(cssName);
            CssTree cssTree = new CssTree();
            Dictionary<string, Dictionary<string, string>> cssProperty = cssTree.CreateCssProperty(cssFullPath, true);
            Assert.IsTrue(cssProperty.ContainsKey("definition"));
            Assert.IsTrue(cssProperty.ContainsKey("partofspeech"));
            Assert.IsTrue(cssProperty.ContainsKey("headword"));
            Assert.IsTrue(cssProperty.ContainsKey("pronunciation"));
            Assert.IsTrue(cssProperty.ContainsKey("@page"));
            Assert.IsTrue(cssProperty.ContainsKey("letData"));
            Assert.IsTrue(cssProperty.ContainsKey("entry"));
            Assert.IsTrue(cssProperty["definition"].ContainsKey("font-family"));
            Assert.IsTrue(cssProperty["partofspeech"].ContainsKey("font-family"));
            Assert.IsTrue(cssProperty["headword"].ContainsKey("font-family"));
            Assert.IsTrue(cssProperty["pronunciation"].ContainsKey("font-family"));
            Assert.IsTrue(cssProperty["@page"].ContainsKey("page-width"));
            Assert.IsTrue(cssProperty["@page"].ContainsKey("page-height"));
            Assert.IsTrue(cssProperty["letData"].ContainsKey("column-count"));
            Assert.IsTrue(cssProperty["entry"].ContainsKey("text-align"));
            Assert.IsTrue(cssProperty["letData"].ContainsKey("column-rule-width"));
        }

        /// <summary>
        /// check for scripture settings
        /// </summary>
        [Test]
        public void ScripturePropertyTest()
        {
            const string cssName = "ScriptureTemp1.css";
            string cssFullPath = FileInput(cssName);
            CssTree cssTree = new CssTree();
            Dictionary<string, Dictionary<string, string>> cssProperty = cssTree.CreateCssProperty(cssFullPath, true);
            Assert.IsTrue(cssProperty.ContainsKey("Paragraph"));
            Assert.IsTrue(cssProperty.ContainsKey("Emphasis"));
            Assert.IsTrue(cssProperty.ContainsKey("SectionHead"));
            Assert.IsTrue(cssProperty.ContainsKey("@page"));
            Assert.IsTrue(cssProperty.ContainsKey("columns"));
            Assert.IsTrue(cssProperty.ContainsKey("scrSection"));
            Assert.IsTrue(cssProperty["Paragraph"].ContainsKey("font-family"));
            Assert.IsTrue(cssProperty["Emphasis"].ContainsKey("font-family"));
            Assert.IsTrue(cssProperty["SectionHead"].ContainsKey("font-family"));
            Assert.IsTrue(cssProperty["@page"].ContainsKey("page-width"));
            Assert.IsTrue(cssProperty["@page"].ContainsKey("page-height"));
            Assert.IsTrue(cssProperty["columns"].ContainsKey("column-count"));
            //Assert.IsTrue(cssProperty["columns"].ContainsKey("column-rule-width"));
            Assert.IsTrue(cssProperty["scrSection"].ContainsKey("text-align"));
        }

        /// <summary>
        ///A test for SetupSettings
        ///</summary>
        [Test]
        public void SetupSettingsTest()
        {
            IPreExportProcess data = mocks.NewMock<IPreExportProcess>();
            Expect.Once.On(data).
                GetProperty("ProcessedCss").
                Will(Return.Value(""));
            const string SettingTemplate = "XPWsettings-tpl.txt";
            string supportData = Common.PathCombine("xetexPathway", "data");
            string supportSettings = Common.PathCombine(supportData, SettingTemplate);
            File.Copy(FileProg(supportSettings), FileOutput(SettingTemplate), true);
            string dataPath = _outputPath;
            _inputType = "dictionary";
            _postscriptLanguage = new PostscriptLanguage();
            SetupSettings(data, dataPath);
            const string ActualSettings = "XPWsettings.txt";
            const string ExpectedSettings = "XPWsettings-let.txt";
            File.Copy(FileOutput(ActualSettings), FileOutput(ExpectedSettings));
            TextFileAssert.AreEqual(FileExpected(ExpectedSettings), FileOutput(ActualSettings));
        }

        /// <summary>
        /// Test that the settings referred to in SetupSettings exist in css
        /// </summary>
        [Test]
        public void SetupSettingsPropertyTest()
        {
            const string cssName = "DictionaryTemp1.css";
            string cssFullPath = FileInput(cssName);
            IPreExportProcess preExportProcess = mocks.NewMock<IPreExportProcess>();
            Expect.Once.On(preExportProcess).
                GetProperty("ProcessedCss").
                Will(Return.Value(cssFullPath));
            const string SettingTemplate = "XPWsettings-tpl.txt";
            string supportData = Common.PathCombine("xetexPathway", "data");
            string supportSettings = Common.PathCombine(supportData, SettingTemplate);
            File.Copy(FileProg(supportSettings), FileOutput(SettingTemplate), true);
            string dataPath = _outputPath;
            _inputType = "dictionary";
            _postscriptLanguage = new PostscriptLanguage(); 
            SetupSettings(preExportProcess, dataPath);
            const string ActualSettings = "XPWsettings.txt";
            const string ExpectedSettings = "XPWsettings-gps1.txt";
            File.Copy(FileOutput(ActualSettings), FileOutput(ExpectedSettings));
            TextFileAssert.AreEqual(FileExpected(ExpectedSettings), FileOutput(ActualSettings));
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        ///A test for PreProcess
        ///</summary>
        [Test]
        public void PreProcessTest()
        {
            const string XhtmlName = "minimal.xhtml";
            const string BlankName = "blank.css";
            PublicationInformation projInfo = GetProjInfo(XhtmlName, BlankName);
            IPreExportProcess actual = PreProcess(projInfo);
            XmlAssert.AreEqual(FileExpected(XhtmlName), actual.ProcessedXhtml, "minimal.xhtml not equal");
            TextFileAssert.AreEqual(FileExpected(BlankName), actual.ProcessedCss);
        }

        /// <summary>
        ///A test for LoadLanguage
        ///</summary>
        [Test]
        public void LoadLanguageTest()
        {
            string processedXhtml = FileInput("main-proc.xhtml");
            Dictionary<string, string> xsltMap = new Dictionary<string, string>();
            _inputType = "dictionary";
            var selector = "xitem_.tpi";
            var postscriptName = "TimesNewRomanPSMT";
            var isGraphite = false;
            _postscriptLanguage.SetClass2Postscript(selector, postscriptName);
            _postscriptLanguage.AddPostscriptName(postscriptName, isGraphite);
            LoadLanguage(processedXhtml, xsltMap);
            Assert.AreEqual(8, xsltMap.Keys.Count);
            Assert.Contains("ver", xsltMap.Keys);
            Assert.Contains("l1", xsltMap.Keys);
            Assert.Contains("l2", xsltMap.Keys);
            Assert.Contains("l3", xsltMap.Keys);
            Assert.Contains("verFont", xsltMap.Keys);
            Assert.Contains("l1Font", xsltMap.Keys);
            Assert.Contains("l2Font", xsltMap.Keys);
            Assert.Contains("l3Font", xsltMap.Keys);
            Assert.AreEqual("bzh", xsltMap["ver"]);
            Assert.AreEqual("en", xsltMap["l1"]);
            Assert.AreEqual("bzh-fonipa", xsltMap["l2"]);
            Assert.AreEqual("tpi", xsltMap["l3"]);
            Assert.AreEqual(@"\fonta ", xsltMap["verFont"]);
            Assert.AreEqual(@"\fonta ", xsltMap["l1Font"]);
            Assert.AreEqual(@"\fonta ", xsltMap["l2Font"]);
            Assert.AreEqual(@"\fontb ", xsltMap["l3Font"]);
        }

        /// <summary>
        ///A test for LoadLanguage
        ///</summary>
        [Test]
        public void LoadLanguageTest2()
        {
            string processedXhtml = FileInput("scriptureSample.xhtml");
            Dictionary<string, string> xsltMap = new Dictionary<string, string>();
            _inputType = "scripture";
            var selector = "xitem_.en";
            var postscriptName = "TimesNewRomanPSMT";
            var isGraphite = false;
            _postscriptLanguage.SetClass2Postscript(selector, postscriptName);
            _postscriptLanguage.AddPostscriptName(postscriptName, isGraphite);
            LoadLanguage(processedXhtml, xsltMap);
            Assert.AreEqual(4, xsltMap.Keys.Count);
            Assert.Contains("ver", xsltMap.Keys);
            Assert.Contains("l1", xsltMap.Keys);
            Assert.Contains("verFont", xsltMap.Keys);
            Assert.Contains("l1Font", xsltMap.Keys);
            Assert.AreEqual("bzh", xsltMap["ver"]);
            Assert.AreEqual("en", xsltMap["l1"]);
            Assert.AreEqual(@"\fonta ", xsltMap["verFont"]);
            Assert.AreEqual(@"\fontb ", xsltMap["l1Font"]);
        }

        /// <summary>
        ///A test for LoadLanguage with lots of fonts
        ///</summary>
        [Test]
        public void LoadLanguageTest3()
        {
            _postscriptLanguage = new PostscriptLanguage(); 
            string processedXhtml = FileInput("YCE-proc.xhtml");
            Dictionary<string, string> xsltMap = new Dictionary<string, string>();
            _inputType = "dictionary";
            const string selector = "headword";
            const string postscriptName = "YiPlusPhonetics";
            const bool isGraphite = false;
            PostscriptFontSetup(selector, postscriptName, isGraphite);
            PostscriptFontSetup("xitem_.ii-x-PIN", "TimesNewRomanPSMT", false);
            PostscriptFontSetup("xitem_.ii-fonipa", "DoulosSIL", true);
            PostscriptFontSetup("xitem_.zh-CN", "SimSun-18030", true);
            PostscriptFontSetup("xitem_.zh-CN-x-PIN", "AndikaDesRevA", true);
            LoadLanguage(processedXhtml, xsltMap);
            Assert.AreEqual(12, xsltMap.Keys.Count);
            Dictionary<string, string> results = new Dictionary<string, string>();
            results["ver"] = "ii";
            results["l1"] = "en";
            results["l2"] = "ii-x-PIN";
            results["l3"] = "ii-fonipa";
            results["l4"] = "zh-CN";
            results["l5"] = "zh-CN-x-PIN";
            results["verFont"] = @"\fontb ";
            results["l1Font"] = @"\fonta ";
            results["l2Font"] = @"\fontc ";
            results["l3Font"] = @"\fontd ";
            results["l4Font"] = @"\fonte ";
            results["l5Font"] = @"\fonta ";
            foreach (string s in results.Keys)
            {
                Assert.Contains(s, xsltMap.Keys);
                Assert.AreEqual(results[s], xsltMap[s]);
            }
}

        private void PostscriptFontSetup(string selector, string postscriptName, bool isGraphite)
        {
            _postscriptLanguage.SetClass2Postscript(selector, postscriptName);
            _postscriptLanguage.AddPostscriptName(postscriptName, isGraphite);
        }

        /// <summary>
        ///A test for Null Launch
        ///</summary>
        [Test]
        public void LaunchNullTest()
        {
            ExportXeTeX target = new ExportXeTeX();
            string exportType = string.Empty;
            PublicationInformation publicationInformation = null;
            bool expected = false;
            bool actual;
            actual = target.Launch(exportType, publicationInformation);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Launch
        ///</summary>
        [Test]
        public void LaunchTest()
        {
            Common.Testing = true;
            ExportXeTeX target = new ExportXeTeX();
            const string XhtmlName = "main-proc.xhtml";
            const string BlankName = "blank.css";
            PublicationInformation publicationInformation = GetProjInfo(XhtmlName, BlankName);
            publicationInformation.ProjectInputType = "Dictionary";
            string exportType = "Dictionary";
            bool expected = true;
            bool actual = target.Launch(exportType, publicationInformation);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Handle Dictionary
        ///</summary>
        [Test]
        public void HandleDictionaryTest()
        {
            ExportXeTeX target = new ExportXeTeX();
            string inputDataType = "Dictionary";
            bool expected = true;
            bool actual;
            actual = target.Handle(inputDataType);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Handle Scripture
        ///</summary>
        [Test]
        public void HandleScriptureTest()
        {
            ExportXeTeX target = new ExportXeTeX();
            string inputDataType = "Scripture";
            bool expected = true;
            bool actual;
            actual = target.Handle(inputDataType);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetXeTeXToolFolder
        ///</summary>
        [Test]
        public void GetTempCopyTest()
        {
            string name = "Loc";
            string expected = Common.PathCombine(Path.GetTempPath(), name);
            string actual = GetTempCopy(name);
            Assert.AreEqual(expected, actual);
            Directory.Delete(expected, true);
        }

        /// <summary>
        ///A test for Export
        ///</summary>
        [Test]
        public void ExportNullTest()
        {
            ExportXeTeX target = new ExportXeTeX();
            PublicationInformation projInfo = null;
            bool expected = false;
            bool actual;
            actual = target.Export(projInfo);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CreateTexInput
        ///</summary>
        [Test]
        public void CreateTexInputTest()
        {
            const string XhtmlName = "main-proc.xhtml";
            const string TexName = "main-proc.txt";
            string processedXhtml = FileOutput(XhtmlName);
            File.Copy(FileInput(XhtmlName), processedXhtml, true);
            string fileName = Path.GetFileNameWithoutExtension(XhtmlName);
            string dataPath = _outputPath;
            _inputType = "dictionary";
            var selector = "xitem_.tpi";
            var postscriptName = "TimesNewRomanPSMT";
            var isGraphite = false;
            _postscriptLanguage.SetClass2Postscript(selector, postscriptName);
            _postscriptLanguage.AddPostscriptName(postscriptName, isGraphite);
            CreateTexInput(processedXhtml, fileName, dataPath);
            TextFileAssert.AreEqual(FileExpected(TexName), FileOutput(TexName));
        }

        /// <summary>
        ///A test for AddImages
        ///</summary>
        [Test]
        public void AddImagesTest()
        {
            string processFolder = _inputPath; 
            string dataPath = _outputPath;
            AddImages(processFolder, dataPath);
            Assert.AreEqual(true, File.Exists(FileOutput(Common.PathCombine("image", "2.jpg"))));
            Assert.AreEqual(true, File.Exists(FileOutput(Common.PathCombine("image", "3.jpg"))));
        }
    }
}