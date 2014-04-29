// --------------------------------------------------------------------------------------------
// <copyright file="InDesignMapTest.cs" from='2009' to='2014' company='SIL International'>
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
using System.Xml.XPath;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.InDesignConvert
{
    [TestFixture]
    public class InDesignMapTest : ValidateXMLFile 
    {
        #region Private Variables
        private string _inputCSS;
        private string _outputPath;
        private string _outputStyles;
        private string _outputMasterSpreads;
        private string _testFolderPath = string.Empty;
        private CssTree _cssTree;
        private InStyles _stylesXML;
        private InDesignMap _designmapXML;
        private ArrayList _expectedList;
        private readonly ArrayList headwordStyles = new ArrayList();
        private Dictionary<string, Dictionary<string, string>> _idAllClass;
        private Dictionary<string, Dictionary<string, string>> _cssProperty;
        private readonly Dictionary<string, string> _expected = new Dictionary<string, string>();
        #endregion

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _stylesXML = new InStyles();
            _expectedList = new ArrayList();
            _designmapXML = new InDesignMap();
            _idAllClass = new Dictionary<string, Dictionary<string, string>>();
            _testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/InDesignConvert/TestFiles");
            ClassProperty = _expected;
            _outputPath = Common.PathCombine(_testFolderPath, "output");
            _outputStyles = Common.PathCombine(_outputPath, "Resources");
            _outputMasterSpreads = Common.PathCombine(_outputPath, "MasterSpreads");
            _cssProperty = new Dictionary<string, Dictionary<string, string>>();
            _cssTree = new CssTree();
        }
        #endregion Setup

        #region Test
        [Test]
        public void SpreadSourceTest()
        {
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Page1.css");
            XPath = "//idPkg:Spread[@src = \"Spreads/Spread_1.xml\"]";
            bool result = DesignMapNodeTest();
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }

        [Test]
        public void SpreadSourceCountTest()
        {
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Page1.css");
            XPath = "//idPkg:Spread";
            int result = DesignMapNodeCountTest();
            Assert.IsTrue(result == 3, _inputCSS + " test Failed");
        }

        [Test]
        public void StorySourceCountTest1()
        {
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Page3.css");
            XPath = "//idPkg:Story";
            int result = NodeCount();
            Assert.IsTrue(result == 12, _inputCSS + " test Failed");
        }

        [Test]
        public void StoryFileCompareTest2()
        {
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Page3.css");
            XPath = "//idPkg:Story";
            _expectedList.Sort();
            _expectedList.Add("Stories/Story_ubcf.xml");
            _expectedList.Add("Stories/Story_utll.xml");
            _expectedList.Add("Stories/Story_ubcl.xml");
            _expectedList.Add("Stories/Story_utrr.xml");
            _expectedList.Add("Stories/Story_ubcr.xml");
            _expectedList.Add("Stories/Story_1.xml");
            _expectedList.Add("Stories/Story_2.xml");
            _expectedList.Add("Stories/Story_3.xml");
            _expectedList.Add("Stories/Story_4.xml");
            bool result = NodeListCompare();
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }

        [Test]
        public void StorySourceCountTest3()
        {
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Page4.css");
            XPath = "//idPkg:Story";
            int result = NodeCount();
            Assert.IsTrue(result == 12, _inputCSS + " test Failed");
        }

        [Test]
        public void StoryFileCompareTest4()
        {
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Page4.css");
            XPath = "//idPkg:Story";
            _expectedList.Sort();
            _expectedList.Add("Stories/Story_utla.xml");
            _expectedList.Add("Stories/Story_utca.xml");
            _expectedList.Add("Stories/Story_utra.xml");
            _expectedList.Add("Stories/Story_utll.xml");
            _expectedList.Add("Stories/Story_utcl.xml");
            _expectedList.Add("Stories/Story_utrl.xml");
            _expectedList.Add("Stories/Story_utlr.xml");
            _expectedList.Add("Stories/Story_utcr.xml");
            _expectedList.Add("Stories/Story_utrr.xml");
            _expectedList.Add("Stories/Story_1.xml");
            _expectedList.Add("Stories/Story_2.xml");
            _expectedList.Add("Stories/Story_3.xml");
            _expectedList.Add("Stories/Story_4.xml");
            bool result = NodeListCompare();
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }
        #endregion

        #region private Methods

        private bool NodeListCompare()
        {
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            var inMasterSpread = new InMasterSpread();
            var masterPageNames = inMasterSpread.CreateIDMasterSpread(_outputMasterSpreads, _idAllClass, headwordStyles);
            ArrayList test = new ArrayList();
            _designmapXML.CreateIDDesignMap(_outputPath, 4, masterPageNames, test, new ArrayList(), string.Empty);
            FileNameWithPath = Common.PathCombine(_outputPath, "designmap.xml");
            var result1 = Common.GetXmlNodeListInDesignNamespace(FileNameWithPath, XPath);
            foreach (XmlNode fileName in result1)
            {
                if(_expectedList.Contains(fileName))
                {
                    return false;
                }
            }
            return true;
        }

        private int NodeCount()
        {
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            var inMasterSpread = new InMasterSpread();
            var masterPageNames = inMasterSpread.CreateIDMasterSpread(_outputMasterSpreads, _idAllClass, headwordStyles);
            ArrayList test = new ArrayList();
            _designmapXML.CreateIDDesignMap(_outputPath, 4, masterPageNames, test, new ArrayList(), string.Empty);
            FileNameWithPath = Common.PathCombine(_outputPath, "designmap.xml");
            XmlNodeList result = Common.GetXmlNodeListInDesignNamespace(FileNameWithPath, XPath);
            return result.Count;
        }

        private bool DesignMapNodeTest()
        {
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            ArrayList test = new ArrayList();
            ArrayList test1 = new ArrayList();
            _designmapXML.CreateIDDesignMap(_outputPath, 2, test, test1, new ArrayList(),string.Empty);
            FileNameWithPath = Common.PathCombine(_outputPath, "designmap.xml");
            bool result = IsNodeExists();
            return result;
        }

        private int DesignMapNodeCountTest()
        {
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            ArrayList test = new ArrayList();
            ArrayList test1 = new ArrayList();
            _designmapXML.CreateIDDesignMap(_outputPath, 2, test, test1, new ArrayList(), string.Empty);
            FileNameWithPath = Common.PathCombine(_outputPath, "designmap.xml");
            XmlNodeList result = Common.GetXmlNodeListInDesignNamespace(FileNameWithPath, XPath);
            return result.Count;
        }
        #endregion



    }
}
