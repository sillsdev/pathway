// --------------------------------------------------------------------------------------------
// <copyright file="InStylesTest.cs" from='2009' to='2009' company='SIL International'>
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
// Test Cases for InDesign StylesTest
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
    public class InGraphicTest : ValidateXMLFile 
    {
        #region Private Variables
        private string _input;
        private string _output;
        private Dictionary<string, string> _expected = new Dictionary<string, string>();
        private string _testFolderPath = string.Empty;
        private InGraphic _graphicXML;
        private ArrayList _borderColor;
        #endregion

        #region Public Variables
        public XPathNodeIterator NodeIter;
        private Dictionary<string, Dictionary<string, string>> _cssProperty;
        private CSSTree _cssTree;
        #endregion

        #region Setup

        [TestFixtureSetUp]
        protected void SetUp()
        {
            _graphicXML = new InGraphic();
            _testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/InDesignConvert/TestFiles");
            ClassProperty = _expected;  //Note: All Reference address initialized here
            _output = Common.PathCombine(_testFolderPath, "Output");

            _cssProperty = new Dictionary<string, Dictionary<string, string>>();
            _cssTree = new CSSTree();
        }
        #endregion Setup

        #region Public Functions

        private void NodeTest(bool checkAttribute)
        {
            _cssProperty = _cssTree.CreateCssProperty(_input, true);
            _graphicXML.CreateIDGraphic(_output, _cssProperty, _cssTree.cssBorderColor);
            FileNameWithPath = Common.PathCombine(_output, "Graphic.xml");
            if (checkAttribute)
            {
                Assert.IsTrue(ValidateNodeAttribute(), _input + " test Failed");
            }
            else
            {
                Assert.IsTrue(ValidateNodeValue(), _input + " test Failed");
            }
        }

        #region Color
        [Test]
        public void Color1()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Color1.css"); // red
            _expected.Add("Space", "RGB");
            _expected.Add("ColorValue", "255 0 0");
            const string searchClass = "Color/#ff0000";
            XPath = "//Color[@Self = \"" + searchClass + "\"]";
            NodeTest(true);
        }

        [Test]
        public void Color2()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Color2.css");  // #123abc
            _expected.Add("Space", "RGB");
            _expected.Add("ColorValue", "18 58 188");
            const string searchClass = "Color/#123abc";
            XPath = "//Color[@Self = \"" + searchClass + "\"]";
            NodeTest(true);
        }

        [Test]
        public void Color3()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Color3.css"); // rgb(0,0,0)
            _expected.Add("Space", "RGB");
            _expected.Add("ColorValue", "0 0 0");
            const string searchClass = "Color/#000000";
            XPath = "//Color[@Self = \"" + searchClass + "\"]";
            NodeTest(true);
        }

        [Test]
        public void Color4()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Color4.css"); // rgb(0,0,0)
            _expected.Add("Space", "RGB");
            _expected.Add("ColorValue", "255 0 0");
            const string searchClass = "Color/#ff0000";
            XPath = "//Color[@Self = \"" + searchClass + "\"]";
            NodeTest(true);
        }

        [Test]
        public void Color5()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Color4.css"); // rgb(0,0,0)
            _expected.Add("Space", "RGB");
            _expected.Add("ColorValue", "0 255 0");
            const string searchClass = "Color/#00ff00";
            XPath = "//Color[@Self = \"" + searchClass + "\"]";
            NodeTest(true);
        }

        [Test]
        public void Color6()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Color4.css"); // rgb(0,0,0)
            _expected.Add("Space", "RGB");
            _expected.Add("ColorValue", "170 0 0");
            const string searchClass = "Color/#aa0000";
            XPath = "//Color[@Self = \"" + searchClass + "\"]";
            NodeTest(true);
        }

        [Test]
        public void Color7()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Color4.css"); // rgb(0,0,0)
            _expected.Add("Space", "RGB");
            _expected.Add("ColorValue", "0 0 0");
            const string searchClass = "Color/#000000";
            XPath = "//Color[@Self = \"" + searchClass + "\"]";
            NodeTest(true);
        } 
        #endregion

        #endregion

    }
}
