// --------------------------------------------------------------------------------------------
// <copyright file="InPreferencesTest.cs" from='2009' to='2014' company='SIL International'>
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
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.InDesignConvert
{
    [TestFixture]
    public class InPreferencesTest : InPreferences
    {
        #region Private Variables
        private string _inputCSSWithFileName;
        private string _outputPath;
        private string _outputResourcePath;
        private string _testFolderPath = string.Empty;
        private string _fileNameWithPath;
        private string _xPath;
        private string _currentDir;
        private string _path;
        private CssTree _cssTree;
        private InStyles _stylesXML;
        private Dictionary<string, Dictionary<string, string>> _cssProperty;
        private Dictionary<string, Dictionary<string, string>> _idAllClass = new Dictionary<string, Dictionary<string, string>>();
        #endregion

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _cssTree = new CssTree();
            _stylesXML = new InStyles();
            _cssProperty = new Dictionary<string, Dictionary<string, string>>();
            _testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/InDesignConvert/TestFiles");
            _outputPath = Common.PathCombine(_testFolderPath, "output");
            _outputResourcePath = Common.PathCombine(_outputPath, "Resources");
        }
        #endregion Setup

        #region Test
        [Test]
        public void PreferenceDocumentTagTest()
        {
            const string _methodName = "PreferenceDocumentTagTest";
            _inputCSSWithFileName = Common.DirectoryPathReplace(_testFolderPath + "/input/Page1.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSSWithFileName, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputResourcePath, _cssProperty);
            CreateIDPreferences(_outputResourcePath, _idAllClass);
            _fileNameWithPath = Common.PathCombine(_outputResourcePath, "Preferences.xml");
            _xPath = "//DocumentPreference";

            XmlNode node = Common.GetXmlNodeInDesignNamespace(_fileNameWithPath, _xPath);
            XmlAttributeCollection attrb = node.Attributes;
            string result = attrb["PageHeight"].Value;
            Assert.AreEqual(result, "792", _methodName + " failed");

            _xPath = "//DocumentPreference[@PageWidth = \"612\"]";
            node = Common.GetXmlNodeInDesignNamespace(_fileNameWithPath, _xPath);
            attrb = node.Attributes;
            result = attrb["PageWidth"].Value;
            Assert.AreEqual(result, "612", _methodName + " failed");
        }
        #endregion
    }
}
