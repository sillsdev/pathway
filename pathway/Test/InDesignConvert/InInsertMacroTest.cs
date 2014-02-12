// --------------------------------------------------------------------------------------------
// <copyright file="InInsertMacroTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
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
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.InDesignConvert
{
    [TestFixture]
    public class InInsertMacroTest
    {
        #region Private Variables
        private string _methodName;
        private string _testFolderPath;
        private InInsertMacro _InsertMacro;
        private CssTree _cssTree;
        private Dictionary<string, Dictionary<string, string>> _cssProperty;
        private Dictionary<string, Dictionary<string, string>> _cssClass = new Dictionary<string, Dictionary<string, string>>();
        #endregion

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _InsertMacro = new InInsertMacro();
            _cssTree = new CssTree();
            _testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/InDesignConvert/TestFiles");
        }
        #endregion Setup

        [Test]
        public void ColumnRule()
        {
            _methodName = "ColumnRule";
            string inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/ColumnRule.css");
            _cssClass = _cssTree.CreateCssProperty(inputCSS, true);
            _InsertMacro._cssClass = _cssClass;
            string output = _InsertMacro.GetColumnRule();
            const string expected = "var columnRule=new Array(\"letData 1 Solid #aa0000\");";
            Assert.AreEqual(output, expected, _methodName + " failed");
        }

        [Test]
        public void BorderRule1()
        {
            _methodName = "BorderRule with border input";
            string inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/BorderRule.css");
            _cssClass = _cssTree.CreateCssProperty(inputCSS, true);
            _InsertMacro._cssClass = _cssClass;
            string output = _InsertMacro.GetBorderRule();
            const string expected = "var borderRule=new Array(\"letter\", \"0 solid #00ff00\", \"13 solid #00ff00\", \"0 solid #00ff00\", \"13 solid #00ff00\", \"sample\", \".5 solid #000000\", \"none\", \"none\", \"none\", \"headword\", \".5 solid #000000\", \".5 solid #000000\", \".5 solid #000000\", \".5 solid #000000\", \"entry\", \".5 solid #aa0000\", \".8 solid #aa0000\", \".6 solid #aa0000\", \".7 solid #aa0000\", \"sense\", \".5 solid #000000\", \".5 solid #000000\", \".5 solid #000000\", \".5 solid #000000\");";
            Assert.AreEqual(output, expected, _methodName + " failed");
        }

        [Test]
        public void BorderRule2()
        {
            _methodName = "BorderRule without border input";
            string inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Color1.css");
            _cssClass = _cssTree.CreateCssProperty(inputCSS, true);
            _InsertMacro._cssClass = _cssClass;
            string output = _InsertMacro.GetBorderRule();
            const string expected = "var borderRule=new Array(\"\");";
            Assert.AreEqual(output, expected, _methodName + " failed");
        }

        [Test]
        public void CropMark1()
        {
            _methodName = "Crop Marks";
            string inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/CropMarks.css");
            _cssClass = _cssTree.CreateCssProperty(inputCSS, true);
            _InsertMacro._cssClass = _cssClass;
            string output = _InsertMacro.GetCropMarks();
            const string expected = "var cropMarks = true;";
            Assert.AreEqual(output, expected, _methodName + " failed");
        }

        [Test]
        public void Margin_LetHead()
        {
            _methodName = "Margin_LetHead";
            string inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/margin_letdata.css");
            _cssClass = _cssTree.CreateCssProperty(inputCSS, true);
            _InsertMacro._cssClass = _cssClass;
            string output = _InsertMacro.GetMarginValue();
            const string expected = "var margin=new Array(\"letHead\", \"1.5\", \"4.5\", \"1.666667\", \"4.5\");";
            Assert.AreEqual(output, expected, _methodName + " failed");
        }
    }
}
