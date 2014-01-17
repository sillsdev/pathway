using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.InDesignConvert
{
    [TestFixture]
    public class InMasterSpreadTest : ValidateXMLFile 
    {
        #region Private Variables
        private string _xPath;
        private string _inputCSS1;
        private string _inputCSS2;
        private string _inputCSS3;
        private string _outputPath;
        private string _outputSpread;
        private string _outputStyle;
        private string _outputStory;
        private string _methodName;
        private string _fileNameWithPath;
        private string _testFolderPath = string.Empty;
        private CssTree _cssTree;
        private InStyles _stylesXML;
        private ArrayList _listofMasterPages;
        private readonly ArrayList headwordStyles = new ArrayList();
        private XmlNodeList nodesList;
        private InMasterSpread _masterSpreadXML;
        private Dictionary<string, Dictionary<string, string>> _idAllClass;
        private Dictionary<string, Dictionary<string, string>> _cssProperty;
        private readonly Dictionary<string, string> _expected = new Dictionary<string, string>();
        #endregion

        #region Public Variables
        public XPathNodeIterator NodeIter;
        #endregion

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/InDesignConvert/TestFiles");
            _inputCSS1 = Common.PathCombine(_testFolderPath, "input/MasterSpread.css");
            _stylesXML = new InStyles();
            _masterSpreadXML = new InMasterSpread();
            _idAllClass = new Dictionary<string, Dictionary<string, string>>();
            _testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/InDesignConvert/TestFiles");
            ClassProperty = _expected;
            _outputPath = Common.PathCombine(_testFolderPath, "output");
            _outputSpread = Common.PathCombine(_outputPath, "MasterSpreads");
            _outputStyle = Common.PathCombine(_outputPath, "Resources");
            _outputStory = Common.PathCombine(_outputPath, "Stories");
            _cssProperty = new Dictionary<string, Dictionary<string, string>>();
            _cssTree = new CssTree();

            _listofMasterPages = new ArrayList
                                     {
                                         "MasterSpread_First.xml",
                                         "MasterSpread_All.xml",
                                         "MasterSpread_Left.xml",
                                         "MasterSpread_Right.xml"
                                     };
        }
        #endregion Setup

        #region Test
        [Test]
        public void MasterPageCount()
        {
            ClearFiles();
            _methodName = "MasterPageCount";
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS1, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyle, _cssProperty);

            _masterSpreadXML.CreateIDMasterSpread(_outputSpread, _idAllClass, headwordStyles);
            for (int i = 0; i < _listofMasterPages.Count - 1; i++)
            {
                string fileWithPath = Common.PathCombine(_outputSpread, _listofMasterPages[i].ToString());
                bool result = File.Exists(fileWithPath);
                Assert.IsTrue(result, fileWithPath + " is missing");
            }
        }

        [Test]
        public void MasterPathPointTypeTest1()
        {
            ClearFiles();
            _methodName = "MasterPathPointTypeTest1";
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS1, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyle, _cssProperty);
            _masterSpreadXML.CreateIDMasterSpread(_outputSpread, _idAllClass, headwordStyles);
            _xPath = "//TextFrame[@Self=\"topleft\"]/Properties/PathGeometry/GeometryPathType/PathPointArray/PathPointType";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "MasterSpread_All.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            Assert.IsTrue(nodesList.Count == 4, _methodName + " failed");
        }

        [Test]
        public void MasterPathPointTypeTest2()
        {
            ClearFiles();
            _methodName = "MasterPathPointTypeTest2";
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS1, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyle, _cssProperty);
            _masterSpreadXML.CreateIDMasterSpread(_outputSpread, _idAllClass, headwordStyles);
            _xPath = "//TextFrame[@Self=\"topleft\"]/Properties/PathGeometry/GeometryPathType/PathPointArray/PathPointType";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "MasterSpread_All.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            XmlNode node = nodesList[0];
            XmlAttributeCollection attrb = node.Attributes;

            string result = attrb["Anchor"].Value;
            // "-198 -324"
            Assert.AreEqual(result, "-192 -330", _methodName + " failed for Anchor");

            result = attrb["LeftDirection"].Value;
            Assert.AreEqual(result, "-192 -330", _methodName + " failed for LeftDirection");

            result = attrb["RightDirection"].Value;
            Assert.AreEqual(result, "-192 -330", _methodName + " failed for RightDirection");
        }

        [Test]
        public void CounterPageTest1()
        {
            ClearFiles();
            _methodName = "CounterPageTest1";
            _inputCSS2 = Common.PathCombine(_testFolderPath, "input/CounterPage1.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS2, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyle, _cssProperty);
            _masterSpreadXML.CreateIDMasterSpread(_outputSpread, _idAllClass, headwordStyles);
            _xPath = "//TextFrame[@Self=\"topright\"]";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "MasterSpread_Left.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            XmlNode node = nodesList[0];
            XmlAttributeCollection attrb = node.Attributes;

            string result = attrb["ParentStory"].Value;
            Assert.AreEqual(result, "utrl", _methodName + " failed for Anchor");

            _xPath = "//TextFrame[@Self=\"topleft\"]";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "MasterSpread_Right.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            attrb = node.Attributes;

            result = attrb["ParentStory"].Value;
            Assert.AreEqual(result, "utlr", _methodName + " failed for Anchor");
        }

        [Test]
        public void CounterPageTest2()
        {
            ClearFiles();
            _methodName = "CounterPageTest2";
            _inputCSS2 = Common.PathCombine(_testFolderPath, "input/CounterPage2.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS2, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyle, _cssProperty);
            _masterSpreadXML.CreateIDMasterSpread(_outputSpread, _idAllClass, headwordStyles);
            _xPath = "//TextFrame[@Self=\"topleft\"]";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "MasterSpread_All.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            XmlNode node = nodesList[0];
            XmlAttributeCollection attrb = node.Attributes;

            string result = attrb["ParentStory"].Value;
            Assert.AreEqual(result, "utla", _methodName + " failed for Anchor");

            _xPath = "//TextFrame[@Self=\"topright\"]";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "MasterSpread_First.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            attrb = node.Attributes;

            result = attrb["ParentStory"].Value;
            Assert.AreEqual(result, "utrf", _methodName + " failed for Anchor");
        }

        [Test]
        public void MasterHeaderFooterTest1()
        {
            ClearFiles();
            _methodName = "MasterHeaderFooterTest1";
            _inputCSS3 = Common.PathCombine(_testFolderPath, "input/Page3.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS3, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyle, _cssProperty);
            _masterSpreadXML.CreateIDMasterSpread(_outputSpread, _idAllClass, headwordStyles);
            _xPath = "//TextFrame[@Self=\"topleft\"]";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "MasterSpread_Left.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            XmlNode node = nodesList[0];
            XmlAttributeCollection attrb = node.Attributes;

            string result = attrb["ParentStory"].Value;
            Assert.AreEqual(result, "utll", _methodName + " failed for ParentStory");

            _xPath = "//TextFrame[@Self=\"bottomcenter\"]";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            attrb = node.Attributes;

            result = attrb["ParentStory"].Value;
            Assert.AreEqual(result, "ubcl", _methodName + " failed for ParentStory");

            _xPath = "//TextFrame[@Self=\"topright\"]";
            _fileNameWithPath = Common.PathCombine(_outputSpread, "MasterSpread_Right.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            attrb = node.Attributes;

            result = attrb["ParentStory"].Value;
            Assert.AreEqual(result, "utrr", _methodName + " failed for ParentStory");

            _xPath = "//TextFrame[@Self=\"bottomcenter\"]";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            attrb = node.Attributes;

            result = attrb["ParentStory"].Value;
            Assert.AreEqual(result, "ubcr", _methodName + " failed for ParentStory");
        }

        [Test]
        public void MasterHeaderFooterTest2()
        {
            ClearFiles();
            _methodName = "MasterHeaderFooterTest1";
            _inputCSS3 = Common.PathCombine(_testFolderPath, "input/Page4.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS3, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyle, _cssProperty);
            _masterSpreadXML.CreateIDMasterSpread(_outputSpread, _idAllClass, headwordStyles);
            _fileNameWithPath = Common.PathCombine(_outputSpread, "MasterSpread_All.xml");
            
            _xPath = "//TextFrame[@Self=\"topleft\"]";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            XmlNode node = nodesList[0];
            XmlAttributeCollection attrb = node.Attributes;

            string result = attrb["ParentStory"].Value;
            Assert.AreEqual(result, "utla", _methodName + " failed for ParentStory");

            _xPath = "//TextFrame[@Self=\"topcenter\"]";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            attrb = node.Attributes;

            result = attrb["ParentStory"].Value;
            Assert.AreEqual(result, "utca", _methodName + " failed for ParentStory");

            _xPath = "//TextFrame[@Self=\"topright\"]";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            attrb = node.Attributes;

            result = attrb["ParentStory"].Value;
            Assert.AreEqual(result, "utra", _methodName + " failed for ParentStory");
        }

        [Test]
        public void AllPageFormat1()
        {
            ClearFiles();
            _methodName = "AllPageFormat1";
            _inputCSS3 = Common.PathCombine(_testFolderPath, "input/allpage1.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS3, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyle, _cssProperty);
            _masterSpreadXML.CreateIDMasterSpread(_outputSpread, _idAllClass, headwordStyles);

            _xPath = "//CharacterStyleRange";
            _fileNameWithPath = Common.PathCombine(_outputStory, "Story_ubca.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            Assert.AreEqual(1, nodesList.Count, _methodName + " test Failed");

            _fileNameWithPath = Common.PathCombine(_outputStory, "Story_utca.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            Assert.AreEqual(5, nodesList.Count, _methodName + " test Failed");
        }

        [Test]
        public void AllPageFormat2()
        {
            ClearFiles();
            _methodName = "AllPageFormat2";
            _inputCSS3 = Common.PathCombine(_testFolderPath, "input/allpage2.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS3, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyle, _cssProperty);
            _masterSpreadXML.CreateIDMasterSpread(_outputSpread, _idAllClass, headwordStyles);


            _xPath = "//CharacterStyleRange";
            _fileNameWithPath = Common.PathCombine(_outputStory, "Story_ubca.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            Assert.AreEqual(1, nodesList.Count, _methodName + " test Failed");

            _fileNameWithPath = Common.PathCombine(_outputStory, "Story_utca.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            Assert.AreEqual(9, nodesList.Count, _methodName + " test Failed");
        }

        [Test]
        public void MirrorPageFormat1()
        {
            ClearFiles();
            _methodName = "MirrorPageFormat1";
            _inputCSS3 = Common.PathCombine(_testFolderPath, "input/mirrorpage1.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS3, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyle, _cssProperty);
            _masterSpreadXML.CreateIDMasterSpread(_outputSpread, _idAllClass, headwordStyles);


            _xPath = "//CharacterStyleRange";
            _fileNameWithPath = Common.PathCombine(_outputStory, "Story_utll.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            Assert.AreEqual(3, nodesList.Count, _methodName + " test Failed");

            _fileNameWithPath = Common.PathCombine(_outputStory, "Story_utrr.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            Assert.AreEqual(3, nodesList.Count, _methodName + " test Failed");

            _fileNameWithPath = Common.PathCombine(_outputStory, "Story_ubcl.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            Assert.AreEqual(1, nodesList.Count, _methodName + " test Failed");
        }

        [Test]
        public void MirrorPageFormat2()
        {
            ClearFiles();
            _methodName = "MirrorPageFormat2";
            _inputCSS3 = Common.PathCombine(_testFolderPath, "input/mirrorpage2.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS3, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyle, _cssProperty);
            _masterSpreadXML.CreateIDMasterSpread(_outputSpread, _idAllClass, headwordStyles);


            _xPath = "//CharacterStyleRange";
            _fileNameWithPath = Common.PathCombine(_outputStory, "Story_utll.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            Assert.AreEqual(5, nodesList.Count, _methodName + " test Failed");

            _fileNameWithPath = Common.PathCombine(_outputStory, "Story_utrr.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            Assert.AreEqual(5, nodesList.Count, _methodName + " test Failed");

            _fileNameWithPath = Common.PathCombine(_outputStory, "Story_utcr.xml");
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            Assert.AreEqual(1, nodesList.Count, _methodName + " test Failed");
        }
        [Test]
        public void MasterPageReferences()
        {
            ClearFiles();
            _methodName = "MasterPageReferences";
            _inputCSS3 = Common.PathCombine(_testFolderPath, "input/MasterPageReference.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS3, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyle, _cssProperty);
            _masterSpreadXML.CreateIDMasterSpread(_outputSpread, _idAllClass, headwordStyles);

            /***********
            //Left Page
            ************/
            _fileNameWithPath = Common.PathCombine(_outputSpread, "MasterSpread_Left.xml");

            _xPath = "//TextFrame[@Self=\"MainFrame\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            XmlNode node = nodesList[0];
            string expected = "<PathPointType Anchor=\"-98.07875 -183.1181\" LeftDirection=\"-98.07875 -183.1181\" RightDirection=\"-98.07875 -183.1181\" /><PathPointType Anchor=\"-98.07875 219.1181\" LeftDirection=\"-98.07875 219.1181\" RightDirection=\"-98.07875 219.1181\" /><PathPointType Anchor=\"98.07875 219.1181\" LeftDirection=\"98.07875 219.1181\" RightDirection=\"98.07875 219.1181\" /><PathPointType Anchor=\"98.07875 -183.1181\" LeftDirection=\"98.07875 -183.1181\" RightDirection=\"98.07875 -183.1181\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");

            _xPath = "//TextFrame[@Self=\"topleft\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            expected = "<PathPointType Anchor=\"-98.07875 -219.1181\" LeftDirection=\"-98.07875 -219.1181\" RightDirection=\"-98.07875 -219.1181\" /><PathPointType Anchor=\"-98.07875 -239.1181\" LeftDirection=\"-98.07875 -239.1181\" RightDirection=\"-98.07875 -239.1181\" /><PathPointType Anchor=\"-22 -239.1181\" LeftDirection=\"-22 -239.1181\" RightDirection=\"-22 -239.1181\" /><PathPointType Anchor=\"-22 -219.1181\" LeftDirection=\"-22 -219.1181\" RightDirection=\"-22 -219.1181\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");

            //topcenter
            _xPath = "//TextFrame[@Self=\"topcenter\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            expected = "<PathPointType Anchor=\"-22 -219.1181\" LeftDirection=\"-22 -219.1181\" RightDirection=\"-22 -219.1181\" /><PathPointType Anchor=\"-22 -239.1181\" LeftDirection=\"-22 -239.1181\" RightDirection=\"-22 -239.1181\" /><PathPointType Anchor=\"40 -239.1181\" LeftDirection=\"40 -239.1181\" RightDirection=\"40 -239.1181\" /><PathPointType Anchor=\"40 -219.1181\" LeftDirection=\"40 -219.1181\" RightDirection=\"40 -219.1181\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");

            //topright
            _xPath = "//TextFrame[@Self=\"topright\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            expected = "<PathPointType Anchor=\"40 -219.1181\" LeftDirection=\"40 -219.1181\" RightDirection=\"40 -219.1181\" /><PathPointType Anchor=\"40 -239.1181\" LeftDirection=\"40 -239.1181\" RightDirection=\"40 -239.1181\" /><PathPointType Anchor=\"116.0788 -239.1181\" LeftDirection=\"116.0788 -239.1181\" RightDirection=\"116.0788 -239.1181\" /><PathPointType Anchor=\"116.0788 -219.1181\" LeftDirection=\"116.0788 -219.1181\" RightDirection=\"116.0788 -219.1181\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");

            //bottomleft
            _xPath = "//TextFrame[@Self=\"bottomleft\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            expected = "<PathPointType Anchor=\"-98.07875 237.1181\" LeftDirection=\"-98.07875 237.1181\" RightDirection=\"-98.07875 237.1181\" /><PathPointType Anchor=\"-98.07875 257.1181\" LeftDirection=\"-98.07875 257.1181\" RightDirection=\"-98.07875 257.1181\" /><PathPointType Anchor=\"-22 257.1181\" LeftDirection=\"-22 257.1181\" RightDirection=\"-22 257.1181\" /><PathPointType Anchor=\"-22 237.1181\" LeftDirection=\"-22 237.1181\" RightDirection=\"-22 237.1181\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");

            //bottomcenter
            _xPath = "//TextFrame[@Self=\"bottomcenter\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            expected = "<PathPointType Anchor=\"40 237.1181\" LeftDirection=\"40 237.1181\" RightDirection=\"40 237.1181\" /><PathPointType Anchor=\"40 257.1181\" LeftDirection=\"40 257.1181\" RightDirection=\"40 257.1181\" /><PathPointType Anchor=\"-22 257.1181\" LeftDirection=\"-22 257.1181\" RightDirection=\"-22 257.1181\" /><PathPointType Anchor=\"-22 237.1181\" LeftDirection=\"-22 237.1181\" RightDirection=\"-22 237.1181\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");

            //bottomright
            _xPath = "//TextFrame[@Self=\"bottomright\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            expected = "<PathPointType Anchor=\"40 237.1181\" LeftDirection=\"40 237.1181\" RightDirection=\"40 237.1181\" /><PathPointType Anchor=\"40 257.1181\" LeftDirection=\"40 257.1181\" RightDirection=\"40 257.1181\" /><PathPointType Anchor=\"116.0788 257.1181\" LeftDirection=\"116.0788 257.1181\" RightDirection=\"116.0788 257.1181\" /><PathPointType Anchor=\"116.0788 237.1181\" LeftDirection=\"116.0788 237.1181\" RightDirection=\"116.0788 237.1181\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");

            /***********
            //First Page
            ************/
            _fileNameWithPath = Common.PathCombine(_outputSpread, "MasterSpread_First.xml");

            _xPath = "//TextFrame[@Self=\"MainFrame\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            expected = "<PathPointType Anchor=\"-69.7323 -69.7323\" LeftDirection=\"-69.7323 -69.7323\" RightDirection=\"-69.7323 -69.7323\" /><PathPointType Anchor=\"-69.7323 105.7323\" LeftDirection=\"-69.7323 105.7323\" RightDirection=\"-69.7323 105.7323\" /><PathPointType Anchor=\"105.7323 105.7323\" LeftDirection=\"105.7323 105.7323\" RightDirection=\"105.7323 105.7323\" /><PathPointType Anchor=\"105.7323 -69.7323\" LeftDirection=\"105.7323 -69.7323\" RightDirection=\"105.7323 -69.7323\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");

            _xPath = "//TextFrame[@Self=\"topleft\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            expected = "<PathPointType Anchor=\"-69.7323 -105.7323\" LeftDirection=\"-69.7323 -105.7323\" RightDirection=\"-69.7323 -105.7323\" /><PathPointType Anchor=\"-69.7323 -125.7323\" LeftDirection=\"-69.7323 -125.7323\" RightDirection=\"-69.7323 -125.7323\" /><PathPointType Anchor=\"-22 -125.7323\" LeftDirection=\"-22 -125.7323\" RightDirection=\"-22 -125.7323\" /><PathPointType Anchor=\"-22 -105.7323\" LeftDirection=\"-22 -105.7323\" RightDirection=\"-22 -105.7323\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");

            //topcenter
            _xPath = "//TextFrame[@Self=\"topcenter\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            expected = "<PathPointType Anchor=\"-22 -105.7323\" LeftDirection=\"-22 -105.7323\" RightDirection=\"-22 -105.7323\" /><PathPointType Anchor=\"-22 -125.7323\" LeftDirection=\"-22 -125.7323\" RightDirection=\"-22 -125.7323\" /><PathPointType Anchor=\"40 -125.7323\" LeftDirection=\"40 -125.7323\" RightDirection=\"40 -125.7323\" /><PathPointType Anchor=\"40 -105.7323\" LeftDirection=\"40 -105.7323\" RightDirection=\"40 -105.7323\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");

            //topright
            _xPath = "//TextFrame[@Self=\"topright\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            expected = "<PathPointType Anchor=\"40 -105.7323\" LeftDirection=\"40 -105.7323\" RightDirection=\"40 -105.7323\" /><PathPointType Anchor=\"40 -125.7323\" LeftDirection=\"40 -125.7323\" RightDirection=\"40 -125.7323\" /><PathPointType Anchor=\"87.7323 -125.7323\" LeftDirection=\"87.7323 -125.7323\" RightDirection=\"87.7323 -125.7323\" /><PathPointType Anchor=\"87.7323 -105.7323\" LeftDirection=\"87.7323 -105.7323\" RightDirection=\"87.7323 -105.7323\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");

            //bottomleft
            _xPath = "//TextFrame[@Self=\"bottomleft\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            expected = "<PathPointType Anchor=\"-69.7323 123.7323\" LeftDirection=\"-69.7323 123.7323\" RightDirection=\"-69.7323 123.7323\" /><PathPointType Anchor=\"-69.7323 143.7323\" LeftDirection=\"-69.7323 143.7323\" RightDirection=\"-69.7323 143.7323\" /><PathPointType Anchor=\"-22 143.7323\" LeftDirection=\"-22 143.7323\" RightDirection=\"-22 143.7323\" /><PathPointType Anchor=\"-22 123.7323\" LeftDirection=\"-22 123.7323\" RightDirection=\"-22 123.7323\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");

            //bottomcenter
            _xPath = "//TextFrame[@Self=\"bottomcenter\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            expected = "<PathPointType Anchor=\"40 123.7323\" LeftDirection=\"40 123.7323\" RightDirection=\"40 123.7323\" /><PathPointType Anchor=\"40 143.7323\" LeftDirection=\"40 143.7323\" RightDirection=\"40 143.7323\" /><PathPointType Anchor=\"-22 143.7323\" LeftDirection=\"-22 143.7323\" RightDirection=\"-22 143.7323\" /><PathPointType Anchor=\"-22 123.7323\" LeftDirection=\"-22 123.7323\" RightDirection=\"-22 123.7323\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");

            //bottomright
            _xPath = "//TextFrame[@Self=\"bottomright\"]//PathPointArray";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, _xPath);
            node = nodesList[0];
            expected = "<PathPointType Anchor=\"40 123.7323\" LeftDirection=\"40 123.7323\" RightDirection=\"40 123.7323\" /><PathPointType Anchor=\"40 143.7323\" LeftDirection=\"40 143.7323\" RightDirection=\"40 143.7323\" /><PathPointType Anchor=\"87.7323 143.7323\" LeftDirection=\"87.7323 143.7323\" RightDirection=\"87.7323 143.7323\" /><PathPointType Anchor=\"87.7323 123.7323\" LeftDirection=\"87.7323 123.7323\" RightDirection=\"87.7323 123.7323\" />";
            Assert.AreEqual(expected, node.InnerXml, _methodName + " failed ");





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
