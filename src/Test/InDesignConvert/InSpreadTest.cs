// --------------------------------------------------------------------------------------------
// <copyright file="InSpreadTest.cs" from='2009' to='2014' company='SIL International'>
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.InDesignConvert
{
    [TestFixture]
    public class InSpreadTest
    {
        #region Private Variables
        private string _xPath;
        private string _inputCSS1;
        private string _inputCSS2;
        private string _inputXHTML;
        private string _inputCSS;
        private string _outputPath;
        private string _methodName;
        private string _outputStyles;
        private string _outputStory;
        private string _outputSpread;
        private string _fileNameWithPath;
        private string _testFolderPath;
        private InStyles _stylesXML;
        private InStory _storyXML;
        private InSpread _spreadXML;
        private CssTree _cssTree;
        private XmlNodeList nodesList;
        private readonly ArrayList _singlePages = new ArrayList();
        private readonly ArrayList _facingPages = new ArrayList();
        private Dictionary<string, Dictionary<string, string>> _idAllClass = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, Dictionary<string, string>> _cssProperty;
        private ArrayList _columnClass = new ArrayList();
        PublicationInformation projInfo = new PublicationInformation();
        #endregion

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _stylesXML = new InStyles();
            _storyXML = new InStory();
            _testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/InDesignConvert/TestFiles");
            _outputPath = Common.PathCombine(_testFolderPath, "output");
            _outputStyles = Common.PathCombine(_outputPath, "Resources");
            _outputStory = Common.PathCombine(_outputPath, "Stories");
            _outputSpread = Common.PathCombine(_outputPath, "Spreads");
            projInfo.TempOutputFolder = _outputPath;
            _cssProperty = new Dictionary<string, Dictionary<string, string>>();
            _cssTree = new CssTree();

            _inputCSS1 = Common.DirectoryPathReplace(_testFolderPath + "/input/Page1.css");
            _inputCSS2 = Common.DirectoryPathReplace(_testFolderPath + "/input/Page2.css");

            _facingPages.Add("Spread_1.xml");
            _facingPages.Add("Spread_2.xml");
            _facingPages.Add("Spread_3.xml");

            _singlePages.AddRange(_facingPages);
            _singlePages.Add("Spread_4.xml");
            _singlePages.Add("Spread_5.xml");

            _columnClass.Add("t1");
            _columnClass.Add("t2");
            _columnClass.Add("t3");
        }
        #endregion Setup

        #region Test
        [Test]
        public void SpreadListTest1()
        {
            ClearFiles();
            _spreadXML = new InSpread();
            _methodName = "SpreadListTest1";
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS1, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            _spreadXML.CreateIDSpread(_outputSpread, _idAllClass, _columnClass);
            for (int i = 0; i < _facingPages.Count; i++)
            {
                string fileWithPath = Common.PathCombine(_outputSpread, _facingPages[i].ToString());
                bool result = File.Exists(fileWithPath);
                Assert.IsTrue(result, fileWithPath + " is missing");
            }
        }

        [Test]
        public void SpreadListTest2()
        {
            ClearFiles();
            _spreadXML = new InSpread();
            _methodName = "SpreadListTest2";
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS1, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            _spreadXML.CreateIDSpread(_outputSpread, _idAllClass, _columnClass);
            for (int i = 0; i < _facingPages.Count; i++)
            {
                string fileWithPath = Common.PathCombine(_outputSpread, _facingPages[i].ToString());
                bool result = File.Exists(fileWithPath);
                Assert.IsTrue(result, fileWithPath + " is missing");
            }
        }

        [Test]
        public void SpreadNameTest()
        {
            ClearFiles();
            _spreadXML = new InSpread();
            _methodName = "SpreadNameTest";
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS1, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            _spreadXML.CreateIDSpread(_outputSpread, _idAllClass, _columnClass);
            _xPath = "//Spread";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "Spread_1.xml");
            XmlNode node = Common.GetXmlNodeInDesignNamespace(_fileNameWithPath, _xPath);
            XmlAttributeCollection attrb = node.Attributes;
            string result = attrb["Self"].Value;
            Assert.AreEqual(result, "Spread_1", _methodName + " failed");
        }

        [Test]
        public void PageNameTest()
        {
            ClearFiles();
            _spreadXML = new InSpread();
            _methodName = "PageNameTest";
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS1, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            _spreadXML.CreateIDSpread(_outputSpread, _idAllClass, _columnClass);
            _xPath = "//Page";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "Spread_1.xml");
            XmlNode node = Common.GetXmlNodeInDesignNamespace(_fileNameWithPath, _xPath);
            XmlAttributeCollection attrb = node.Attributes;
            string result = attrb["Self"].Value;
            Assert.AreEqual(result, "page1", _methodName + " failed");
        }

        [Test]
        public void TextFrameNameTest()
        {
            ClearFiles();
            _spreadXML = new InSpread();
            _methodName = "TextFrameNameTest";
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS1, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            _spreadXML.CreateIDSpread(_outputSpread, _idAllClass, _columnClass);
            _xPath = "//TextFrame";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "Spread_1.xml");
            XmlNode node = Common.GetXmlNodeInDesignNamespace(_fileNameWithPath, _xPath);
            XmlAttributeCollection attrb = node.Attributes;
            string result = attrb["Self"].Value;
            Assert.AreEqual(result, "TF1", _methodName + " failed");
        }

        [Test]
        [Ignore]
        public void MarginPreferenceTest()
        {
            ClearFiles();
            _spreadXML = new InSpread();
            _methodName = "MarginPreferenceTest";
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS2, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            _spreadXML.CreateIDSpread(_outputSpread, _idAllClass, _columnClass);
            _xPath = "//MarginPreference";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "Spread_2.xml");
            XmlNode node = Common.GetXmlNodeInDesignNamespace(_fileNameWithPath, _xPath);
            XmlAttributeCollection attrb = node.Attributes;
            string result = attrb["Top"].Value;
            Assert.AreEqual(result, "71", _methodName + " failed for Margin_Top");

            result = attrb["Right"].Value;
            Assert.AreEqual(result, "72", _methodName + " failed for Margin_Top");

            result = attrb["Bottom"].Value;
            Assert.AreEqual(result, "108", _methodName + " failed for Margin_Top");

            result = attrb["Left"].Value;
            Assert.AreEqual(result, "144", _methodName + " failed for Margin_Top");
        }

        [Test]
        public void PageCountTest()
        {
            ClearFiles();
            _spreadXML = new InSpread();
            _methodName = "PageCountTest";
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS2, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            _spreadXML.CreateIDSpread(_outputSpread, _idAllClass, _columnClass);
            _xPath = "//Page";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "Spread_2.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            Assert.IsTrue(nodesList.Count == 2, _methodName + " failed");
        }

        [Test]
        public void PathPointTypeTest1()
        {
            ClearFiles();
            _spreadXML = new InSpread();
            _methodName = "PathPointTypeTest1";
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS2, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            _spreadXML.CreateIDSpread(_outputSpread, _idAllClass, _columnClass);
            _xPath = "//TextFrame[@Self=\"TF2\"]/Properties/PathGeometry/GeometryPathType/PathPointArray/PathPointType";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "Spread_1.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            Assert.IsTrue(nodesList.Count == 4, _methodName + " failed");
        }

        [Test]
        [Ignore]
        public void PathPointTypeTest2()
        {
            ClearFiles();
            _spreadXML = new InSpread();
            _methodName = "PathPointTypeTest2";
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS2, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            _spreadXML.CreateIDSpread(_outputSpread, _idAllClass, _columnClass);
            _xPath = "//TextFrame[@Self=\"TF2\"]/Properties/PathGeometry/GeometryPathType/PathPointArray/PathPointType";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "Spread_1.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            XmlNode node = nodesList[0];
            XmlAttributeCollection attrb = node.Attributes;

            string result = attrb["Anchor"].Value;
            // "-198 -324"
            Assert.AreEqual(result, "-270 -360", _methodName + " failed for Anchor");

            result = attrb["LeftDirection"].Value;
            Assert.AreEqual(result, "-270 -360", _methodName + " failed for LeftDirection");

            result = attrb["RightDirection"].Value;
            Assert.AreEqual(result, "-270 -360", _methodName + " failed for RightDirection");
        }

        [Test]
        public void ColumnCount()
        {
            ClearFiles();
            _spreadXML = new InSpread();
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/ColumnGap1.xhtml");
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/ColumnGap1.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
	        projInfo.ProjectInputType = "Dictionary";
            _storyXML.CreateStory(projInfo, _idAllClass, _cssTree.SpecificityClass, _cssTree.CssClassOrder);
            _spreadXML.CreateIDSpread(_outputSpread, _idAllClass, _columnClass);
            _xPath = "//TextFrame[@Self=\"TF2\"]/TextFramePreference";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "Spread_1.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            XmlNode node = nodesList[0];
            XmlAttributeCollection attrb = node.Attributes;

            string result = attrb["TextColumnCount"].Value;
            Assert.AreEqual(result, "2", _methodName);
        }

        [Test]
        public void ColumnGap()
        {
            ClearFiles();
            _spreadXML = new InSpread();
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/ColumnGap1.xhtml");
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/ColumnGap1.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
            _storyXML.CreateStory(projInfo, _idAllClass, _cssTree.SpecificityClass, _cssTree.CssClassOrder);
            _spreadXML.CreateIDSpread(_outputSpread, _idAllClass, _columnClass);
            _xPath = "//TextFrame[@Self=\"TF2\"]/TextFramePreference";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "Spread_1.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            XmlNode node = nodesList[0];
            XmlAttributeCollection attrb = node.Attributes;

            string result = attrb["TextColumnGutter"].Value;
            Assert.AreEqual(result, "50", _methodName);
        }

        [Test]
        public void ColumnGapEm()
        {
            ClearFiles();
            _spreadXML = new InSpread();
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/ColumnGap2.xhtml");
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/ColumnGap2.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
            Dictionary<string, ArrayList> stylename = _storyXML.CreateStory(projInfo, _idAllClass, _cssTree.SpecificityClass, _cssTree.CssClassOrder);
            _spreadXML.CreateIDSpread(_outputSpread, _idAllClass, stylename["ColumnClass"]);
            _xPath = "//TextFrame[@Self=\"TF2\"]/TextFramePreference";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "Spread_1.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            XmlNode node = nodesList[0];
            XmlAttributeCollection attrb = node.Attributes;

            string result = attrb["TextColumnGutter"].Value;
            Assert.AreEqual(result, "15", _methodName);
        }
        #endregion

        #region Private Functions
        private void ClearFiles()
        {
            string[] files = Directory.GetFiles(_outputSpread);
            foreach (string file in files)
                File.Delete(file);
        }
        #endregion
    }
}
