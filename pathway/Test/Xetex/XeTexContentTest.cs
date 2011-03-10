using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;
using Test;

namespace Test.Xetex
{
    [TestFixture]
    public class XetexTest
    {
        #region Private Variables
        private string _inputCSS;
        private string _inputXHTML;
        private string _outputPath;
        private string _expectedPath;
        private string _outputStory;
        private string _outputStyles;
        private Dictionary<string, string> _expected = new Dictionary<string, string>();
        private string _className = "a";
        private string _testFolderPath = string.Empty;
        Dictionary<string, Dictionary<string, string>> _idAllClass = new Dictionary<string, Dictionary<string, string>>();
        private InStyles _stylesXML;
        private InStory _storyXML;
        private readonly ArrayList headwordStyles = new ArrayList();
        #endregion

        #region Public Variables
        public XPathNodeIterator NodeIter;
        private Dictionary<string, Dictionary<string, string>> _cssProperty;
        private CssTree _cssTree;
        #endregion

        #region Setup
        [TestFixtureSetUp]
        protected void SetUpAll()
        {
            _stylesXML = new InStyles();
            _storyXML = new InStory();
            _testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/InDesignConvert/TestFiles");
            //ClassProperty = _expected;  //Note: All Reference address initialized here
            _outputPath = Common.PathCombine(_testFolderPath, "output");
            _expectedPath = Common.PathCombine(_testFolderPath, "expected");
            _expectedPath = Common.PathCombine(_expectedPath, "BuangExpect");
            _outputStyles = Common.PathCombine(_outputPath, "Resources");
            _outputStory = Common.PathCombine(_outputPath, "Stories");
            _cssProperty = new Dictionary<string, Dictionary<string, string>>();
            Common.SupportFolder = "";
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, "/../PsSupport");
            Common.CopyOfficeFolder(_expectedPath, _outputPath);
        }

        [SetUp]
        protected void SetupEach()
        {
            _cssTree = new CssTree();
        }
        #endregion Setup

        #region Public Functions

        /// <summary>
        /// Multi Parent Test - .subsenses > .sense > .xsensenumber { font-size:10pt;}
        /// Parent comes as multiple times
        /// </summary>
        [Test]
        public void T1Test()
        {

            //_projInfo.ProjectInputType = "Dictionary";
            //const string file = "BuangExport";
            //DateTime startTime = DateTime.Now;

            //string styleOutput = GetStyleOutput(file);

            //_totalTime = DateTime.Now - startTime;

            //string styleExpected = Common.PathCombine(_expectedPath, "BuangExportstyles.xml");
            //string contentExpected = Common.PathCombine(_expectedPath, "BuangExportcontent.xml");
            //XmlAssert.AreEqual(styleExpected, styleOutput, file + " in styles.xml");
            //XmlAssert.AreEqual(contentExpected, _projInfo.TempOutputFolder, file + " in content.xml");
        }

        private void ExportProcess(string file)
        {
            //OOContent contentXML = new OOContent();
            //OOStyles stylesXML = new OOStyles();
            //string fileOutput = _index > 0 ? file + _index + ".css" : file + ".css";

            ////string input = FileInput(file + ".css");
            //string input = FileInput(fileOutput);

            //_projInfo.DefaultCssFileWithPath = input;
            //_projInfo.TempOutputFolder = _outputPath;

            //Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            //CssTree cssTree = new CssTree();
            //cssClass = cssTree.CreateCssProperty(input, true);

            ////StyleXML
            //string styleOutput = FileOutput(file + _styleFile);
            //Dictionary<string, Dictionary<string, string>> idAllClass = stylesXML.CreateStyles(_projInfo, cssClass, styleOutput);

            //// ContentXML
            //_projInfo.DefaultXhtmlFileWithPath = FileInput(file + ".xhtml");
            //_projInfo.TempOutputFolder = FileOutput(file);
            //contentXML.CreateStory(_projInfo, idAllClass, cssTree.SpecificityClass, cssTree.CssClassOrder);
            //_projInfo.TempOutputFolder = _projInfo.TempOutputFolder + _contentFile;
            //return styleOutput;
        }

        #endregion
    }
}
