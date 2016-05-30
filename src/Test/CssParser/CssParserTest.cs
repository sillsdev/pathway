// --------------------------------------------------------------------------------------------
// <copyright file="CssParserTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Tests for Css Parser 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using NUnit.Framework;
using System.Windows.Forms;
using System.IO;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.CssParserTest
{
    [TestFixture]
    [Category("BatchTest")]
    public class CssParserTest
    {
        #region Private Variables

        private string _methodName;
        private string testPath;
        private string inputCSSPath;
        CssParser target = new CssParser();
        TreeNode OutputNode = new TreeNode();

        readonly TreeNode resultNode = new TreeNode();
        readonly TreeNode expectedNode = new TreeNode();
        readonly TreeNode resultSCNode = new TreeNode();     // Single Class
        readonly TreeNode expectedSCNode = new TreeNode();
        readonly TreeNode resultDCNode = new TreeNode();     //Double Class
        readonly TreeNode expectedDCNode = new TreeNode();
        readonly TreeNode resultTLNode = new TreeNode();     //Two Level
        readonly TreeNode expectedTLNode = new TreeNode();
        readonly TreeNode EmptyNode = new TreeNode();        //Empty Class
        readonly TreeNode resultPseudoNode = new TreeNode(); // Pseudo CLass
        readonly TreeNode expectedPseudoNode = new TreeNode();
        readonly TreeNode resultPDNode = new TreeNode();     // Parent Dual Class
        readonly TreeNode expectedPDNode = new TreeNode();
        readonly TreeNode resultIPCPNode = new TreeNode();   // Inhertit Parent Class Property
        readonly TreeNode expectedIPCPNode = new TreeNode();
        readonly TreeNode resultRUSNode = new TreeNode();   // To remove "_" in classnames
        readonly TreeNode expectedRUSNode = new TreeNode();
        readonly TreeNode resultMLSNode = new TreeNode();   // MultiLineSyntax Property
        readonly TreeNode expectedMLSNode = new TreeNode();
        readonly TreeNode resultOSNode = new TreeNode();   // OxesCase Property
        readonly TreeNode expectedOSNode = new TreeNode();


        #endregion

        #region Setup
        [TestFixtureSetUp]

        protected void SetUp()
        {
            testPath =  PathPart.Bin(Environment.CurrentDirectory, "/CssParser/TestFiles/cssInput");
        }
        #endregion

        #region Result

        public void ResultOneLevelTree()
        {
            var clsCSS = new CssParser();
            var filePath = Common.DirectoryPathReplace(testPath + "/sample1.css");
            var Node = clsCSS.BuildTree(filePath);
            resultNode.Nodes.Add((TreeNode)Node.Clone());
        }

        public void ResultTwoLevelTree()
        {
            var clsCSS = new CssParser();
            var filePath = Common.DirectoryPathReplace(testPath + "/twolevelsample1.css");
            var Node = clsCSS.BuildTree(filePath);
            resultTLNode.Nodes.Add((TreeNode)Node.Clone());
        }

        public void ResultSingleClassCSS()
        {
            var clsCSS = new CssParser();
            var filePath = Common.DirectoryPathReplace(testPath + "/SingleClassFile.css");
            var Node = clsCSS.BuildTree(filePath);
            resultSCNode.Nodes.Add((TreeNode)Node.Clone());
        }

        public void ResultDoubleClassCSS()
        {
            var clsCSS = new CssParser();
            var filePath = Common.DirectoryPathReplace(testPath + "/DoubleClassFile.css");
            var Node = clsCSS.BuildTree(filePath);
            resultDCNode.Nodes.Add((TreeNode)Node.Clone());
        }

        public void ResultPseudoLevelTree()
        {
            var clsCSS = new CssParser();
            var filePath = Common.DirectoryPathReplace(testPath + "/PseudoClassFile.css");
            var Node = clsCSS.BuildTree(filePath);
            resultPseudoNode.Nodes.Add((TreeNode)Node.Clone());
        }

        public void ResultParentDualClassTree()
        {
            var clsCSS = new CssParser();
            var filePath = Common.DirectoryPathReplace(testPath + "/ParentDualClassFile.css");
            var Node = clsCSS.BuildTree(filePath);
            resultPDNode.Nodes.Add((TreeNode)Node.Clone());
        }

        public void ResultInhertitParentClassPropertyTree()
        {
            var clsCSS = new CssParser();
            var filePath = Common.DirectoryPathReplace(testPath + "/InhertitParentClassPropertyFile.css");
            var Node = clsCSS.BuildTree(filePath);
            resultIPCPNode.Nodes.Add((TreeNode)Node.Clone());
        }

        public void ResultRemoveUnderscoreTree()
        {
            var clsCSS = new CssParser();
            var filePath = Common.DirectoryPathReplace(testPath + "/Underscore.css");
            var Node = clsCSS.BuildTree(filePath);
            resultRUSNode.Nodes.Add((TreeNode)Node.Clone());
        }

        public void ResultMultiLineSyntaxTree()
        {
            var clsCSS = new CssParser();
            var filePath = Common.DirectoryPathReplace(testPath + "/MultiLineSyntax.css");
            var Node = clsCSS.BuildTree(filePath);
            resultMLSNode.Nodes.Add((TreeNode)Node.Clone());
        }


        public void ResultOxesPropertyTree()
        {
            var clsCSS = new CssParser();
            var filePath = Common.DirectoryPathReplace(testPath + "/OxesCase.css");
            var Node = clsCSS.BuildTree(filePath);
            resultOSNode.Nodes.Add((TreeNode)Node.Clone());
        }
        #endregion

        #region Expected
        public void ExpectedSingleClassCSS()
        {
            expectedSCNode.Nodes.Clear();
            var SCtree = new TreeNode();
            SCtree.Text = "ROOT";
            SCtree.Nodes.Add("RULE");
            SCtree.Nodes[0].Nodes.Add("CLASS");
            SCtree.Nodes[0].Nodes[0].Nodes.Add("letter");
            SCtree.Nodes[0].Nodes.Add("PROPERTY");
            SCtree.Nodes[0].Nodes[1].Nodes.Add("text-align");
            SCtree.Nodes[0].Nodes[1].Nodes.Add("center");
            SCtree.Nodes[0].Nodes.Add("PROPERTY");
            SCtree.Nodes[0].Nodes[2].Nodes.Add("font-weight");
            SCtree.Nodes[0].Nodes[2].Nodes.Add("bold");
            SCtree.Nodes[0].Nodes.Add("PROPERTY");
            SCtree.Nodes[0].Nodes[3].Nodes.Add("font-size");
            SCtree.Nodes[0].Nodes[3].Nodes.Add("24");
            SCtree.Nodes[0].Nodes[3].Nodes.Add("pt");
            SCtree.Nodes[0].Nodes.Add("PROPERTY");
            SCtree.Nodes[0].Nodes[4].Nodes.Add("font-family");
            SCtree.Nodes[0].Nodes[4].Nodes.Add("\"" + "Times New Roman" + "\"");
            SCtree.Nodes[0].Nodes[4].Nodes.Add("serif");
            expectedSCNode.Nodes.Add((TreeNode)SCtree.Clone());
        }

        public void ExpectedDoubleClassCSS()
        {
            expectedDCNode.Nodes.Clear();
            var DCtree = new TreeNode();
            DCtree.Text = "ROOT";
            DCtree.Nodes.Add("RULE");
            DCtree.Nodes[0].Nodes.Add("CLASS");
            DCtree.Nodes[0].Nodes[0].Nodes.Add("entry");
            DCtree.Nodes[0].Nodes.Add("PROPERTY");
            DCtree.Nodes[0].Nodes[1].Nodes.Add("font-family");
            DCtree.Nodes[0].Nodes[1].Nodes.Add("Gentium");
            DCtree.Nodes[0].Nodes.Add("PROPERTY");
            DCtree.Nodes[0].Nodes[2].Nodes.Add("font-size");
            DCtree.Nodes[0].Nodes[2].Nodes.Add("13");
            DCtree.Nodes[0].Nodes[2].Nodes.Add("pt");
            DCtree.Nodes[0].Nodes.Add("PROPERTY");
            DCtree.Nodes[0].Nodes[3].Nodes.Add("font-weight");
            DCtree.Nodes[0].Nodes[3].Nodes.Add("normal");
            DCtree.Nodes[0].Nodes.Add("PROPERTY");
            DCtree.Nodes[0].Nodes[4].Nodes.Add("font-style");
            DCtree.Nodes[0].Nodes[4].Nodes.Add("normal");
            DCtree.Nodes[0].Nodes.Add("PROPERTY");
            DCtree.Nodes[0].Nodes[5].Nodes.Add("text-align");
            DCtree.Nodes[0].Nodes[5].Nodes.Add("justify");
            DCtree.Nodes[0].Nodes.Add("PROPERTY");
            DCtree.Nodes[0].Nodes[6].Nodes.Add("text-decoration");
            DCtree.Nodes[0].Nodes[6].Nodes.Add("none");
            DCtree.Nodes[0].Nodes.Add("PROPERTY");
            DCtree.Nodes[0].Nodes[7].Nodes.Add("text-indent");
            DCtree.Nodes[0].Nodes[7].Nodes.Add("-36");
            DCtree.Nodes[0].Nodes[7].Nodes.Add("pt");
            DCtree.Nodes.Add("RULE");
            DCtree.Nodes[1].Nodes.Add("CLASS");
            DCtree.Nodes[1].Nodes[0].Nodes.Add("letter");
            DCtree.Nodes[1].Nodes.Add("PROPERTY");
            DCtree.Nodes[1].Nodes[1].Nodes.Add("text-align");
            DCtree.Nodes[1].Nodes[1].Nodes.Add("center");
            DCtree.Nodes[1].Nodes.Add("PROPERTY");
            DCtree.Nodes[1].Nodes[2].Nodes.Add("font-weight");
            DCtree.Nodes[1].Nodes[2].Nodes.Add("bold");
            DCtree.Nodes[1].Nodes.Add("PROPERTY");
            DCtree.Nodes[1].Nodes[3].Nodes.Add("font-size");
            DCtree.Nodes[1].Nodes[3].Nodes.Add("24");
            DCtree.Nodes[1].Nodes[3].Nodes.Add("pt");
            DCtree.Nodes[1].Nodes.Add("PROPERTY");
            DCtree.Nodes[1].Nodes[4].Nodes.Add("font-family");
            DCtree.Nodes[1].Nodes[4].Nodes.Add("\"" + "Times New Roman" + "\"");
            DCtree.Nodes[1].Nodes[4].Nodes.Add("serif");
            expectedDCNode.Nodes.Add((TreeNode)DCtree.Clone());
        }

        public void ExpectedOneLevelTree()
        {
            expectedNode.Nodes.Clear();
            var tree = new TreeNode();
            tree.Text = "ROOT";
            tree.Nodes.Add("RULE");
            tree.Nodes[0].Nodes.Add("CLASS");
            tree.Nodes[0].Nodes[0].Nodes.Add("Base");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[1].Nodes.Add("border");
            tree.Nodes[0].Nodes[1].Nodes.Add("2");
            tree.Nodes[0].Nodes[1].Nodes.Add("px");
            tree.Nodes[0].Nodes[1].Nodes.Add("solid");
            tree.Nodes[0].Nodes[1].Nodes.Add("#CC0000");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[2].Nodes.Add("margin");
            tree.Nodes[0].Nodes[2].Nodes.Add("2");
            tree.Nodes[0].Nodes[2].Nodes.Add("px");
            tree.Nodes.Add("RULE");
            tree.Nodes[1].Nodes.Add("CLASS");
            tree.Nodes[1].Nodes[0].Nodes.Add("letter");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[1].Nodes.Add("background-color");
            tree.Nodes[1].Nodes[1].Nodes.Add("#DBDBDB");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[2].Nodes.Add("color");
            tree.Nodes[1].Nodes[2].Nodes.Add("rgb");
            tree.Nodes[1].Nodes[2].Nodes.Add("(");
            tree.Nodes[1].Nodes[2].Nodes.Add("255");
            tree.Nodes[1].Nodes[2].Nodes.Add("0");
            tree.Nodes[1].Nodes[2].Nodes.Add("0");
            tree.Nodes[1].Nodes[2].Nodes.Add(")");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[3].Nodes.Add("margin");
            tree.Nodes[1].Nodes[3].Nodes.Add("2");
            tree.Nodes[1].Nodes[3].Nodes.Add("px");
            tree.Nodes.Add("RULE");
            tree.Nodes[2].Nodes.Add("CLASS");
            tree.Nodes[2].Nodes[0].Nodes.Add("align");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[1].Nodes.Add("border");
            tree.Nodes[2].Nodes[1].Nodes.Add("2");
            tree.Nodes[2].Nodes[1].Nodes.Add("px");
            tree.Nodes[2].Nodes[1].Nodes.Add("solid");
            tree.Nodes[2].Nodes[1].Nodes.Add("#CC0000");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[2].Nodes.Add("margin");
            tree.Nodes[2].Nodes[2].Nodes.Add("2");
            tree.Nodes[2].Nodes[2].Nodes.Add("px");
            expectedNode.Nodes.Add((TreeNode)tree.Clone());
        }

        public void ExpectedTwoLevelTree()
        {
            expectedTLNode.Nodes.Clear();
            var tree = new TreeNode();
            tree.Text = "ROOT";
            tree.Nodes.Add("RULE");
            tree.Nodes[0].Nodes.Add("CLASS");
            tree.Nodes[0].Nodes[0].Nodes.Add("entry");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[1].Nodes.Add("font-family");
            tree.Nodes[0].Nodes[1].Nodes.Add("\"" + "<default serif>" + "\"");
            tree.Nodes[0].Nodes[1].Nodes.Add("serif");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[2].Nodes.Add("background-color");
            tree.Nodes[0].Nodes[2].Nodes.Add("rgb");
            tree.Nodes[0].Nodes[2].Nodes.Add("(");
            tree.Nodes[0].Nodes[2].Nodes.Add("255");
            tree.Nodes[0].Nodes[2].Nodes.Add("255");
            tree.Nodes[0].Nodes[2].Nodes.Add("255");
            tree.Nodes[0].Nodes[2].Nodes.Add(")");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[3].Nodes.Add("font-weight");
            tree.Nodes[0].Nodes[3].Nodes.Add("normal");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[4].Nodes.Add("font-style");
            tree.Nodes[0].Nodes[4].Nodes.Add("normal");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[5].Nodes.Add("color");
            tree.Nodes[0].Nodes[5].Nodes.Add("rgb");
            tree.Nodes[0].Nodes[5].Nodes.Add("(");
            tree.Nodes[0].Nodes[5].Nodes.Add("0");
            tree.Nodes[0].Nodes[5].Nodes.Add("0");
            tree.Nodes[0].Nodes[5].Nodes.Add("0");
            tree.Nodes[0].Nodes[5].Nodes.Add(")");
            tree.Nodes.Add("RULE");
            tree.Nodes[1].Nodes.Add("CLASS");
            tree.Nodes[1].Nodes[0].Nodes.Add("letdata");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[1].Nodes.Add("column-fill");
            tree.Nodes[1].Nodes[1].Nodes.Add("auto");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[2].Nodes.Add("column-rule");
            tree.Nodes[1].Nodes[2].Nodes.Add("solid");
            tree.Nodes[1].Nodes[2].Nodes.Add("1");
            tree.Nodes[1].Nodes[2].Nodes.Add("pt");
            tree.Nodes[1].Nodes[2].Nodes.Add("#aa0000");
            tree.Nodes.Add("RULE");
            tree.Nodes[2].Nodes.Add("CLASS");
            tree.Nodes[2].Nodes[0].Nodes.Add("grammaticalinfo");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[1].Nodes.Add("font-family");
            tree.Nodes[2].Nodes[1].Nodes.Add("serif");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[2].Nodes.Add("font-size");
            tree.Nodes[2].Nodes[2].Nodes.Add("0.8");
            tree.Nodes[2].Nodes[2].Nodes.Add("em");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[3].Nodes.Add("font-weight");
            tree.Nodes[2].Nodes[3].Nodes.Add("700");
            tree.Nodes.Add("RULE");
            tree.Nodes[3].Nodes.Add("CLASS");
            tree.Nodes[3].Nodes[0].Nodes.Add("locator");
            tree.Nodes[3].Nodes.Add("PROPERTY");
            tree.Nodes[3].Nodes[1].Nodes.Add("display");
            tree.Nodes[3].Nodes[1].Nodes.Add("block");
            tree.Nodes[3].Nodes.Add("PROPERTY");
            tree.Nodes[3].Nodes[2].Nodes.Add("text-align");
            tree.Nodes[3].Nodes[2].Nodes.Add("center");
            tree.Nodes[3].Nodes.Add("PROPERTY");
            tree.Nodes[3].Nodes[3].Nodes.Add("flow");
            tree.Nodes[3].Nodes[3].Nodes.Add("static");
            tree.Nodes[3].Nodes[3].Nodes.Add("(");
            tree.Nodes[3].Nodes[3].Nodes.Add("locator");
            tree.Nodes[3].Nodes[3].Nodes.Add(")");
            tree.Nodes.Add("RULE");
            tree.Nodes[4].Nodes.Add("CLASS");
            tree.Nodes[4].Nodes[0].Nodes.Add("letter");
            tree.Nodes[4].Nodes.Add("PROPERTY");
            tree.Nodes[4].Nodes[1].Nodes.Add("direction");
            tree.Nodes[4].Nodes[1].Nodes.Add("ltr");
            tree.Nodes[4].Nodes.Add("PROPERTY");
            tree.Nodes[4].Nodes[2].Nodes.Add("display");
            tree.Nodes[4].Nodes[2].Nodes.Add("inline");
            tree.Nodes[4].Nodes.Add("PROPERTY");
            tree.Nodes[4].Nodes[3].Nodes.Add("font-weight");
            tree.Nodes[4].Nodes[3].Nodes.Add("normal");
            tree.Nodes[4].Nodes.Add("PROPERTY");
            tree.Nodes[4].Nodes[4].Nodes.Add("font-size");
            tree.Nodes[4].Nodes[4].Nodes.Add("30");
            tree.Nodes[4].Nodes[4].Nodes.Add("pt");
            tree.Nodes[4].Nodes.Add("PROPERTY");
            tree.Nodes[4].Nodes[5].Nodes.Add("font-family");
            tree.Nodes[4].Nodes[5].Nodes.Add("\"" + "Times New Roman" + "\"");
            tree.Nodes[4].Nodes[5].Nodes.Add("serif");
            tree.Nodes.Add("RULE");
            tree.Nodes[5].Nodes.Add("CLASS");
            tree.Nodes[5].Nodes[0].Nodes.Add("slotname");
            tree.Nodes[5].Nodes.Add("PROPERTY");
            tree.Nodes[5].Nodes[1].Nodes.Add("font-size");
            tree.Nodes[5].Nodes[1].Nodes.Add("90");
            tree.Nodes[5].Nodes[1].Nodes.Add("%");
            tree.Nodes[5].Nodes.Add("PROPERTY");
            tree.Nodes[5].Nodes[2].Nodes.Add("font-family");
            tree.Nodes[5].Nodes[2].Nodes.Add("sans-serif");
            tree.Nodes[5].Nodes.Add("PROPERTY");
            tree.Nodes[5].Nodes[3].Nodes.Add("letter-spacing");
            tree.Nodes[5].Nodes[3].Nodes.Add("0.05");
            tree.Nodes[5].Nodes[3].Nodes.Add("em");
            tree.Nodes.Add("RULE");
            tree.Nodes[6].Nodes.Add("CLASS");
            tree.Nodes[6].Nodes[0].Nodes.Add("headword");
            tree.Nodes[6].Nodes.Add("PROPERTY");
            tree.Nodes[6].Nodes[1].Nodes.Add("string-set");
            tree.Nodes[6].Nodes[1].Nodes.Add("guideword");
            tree.Nodes[6].Nodes[1].Nodes.Add("content");
            tree.Nodes[6].Nodes[1].Nodes.Add("(");
            tree.Nodes[6].Nodes[1].Nodes.Add(")");
            tree.Nodes.Add("RULE");
            tree.Nodes[7].Nodes.Add("CLASS");
            tree.Nodes[7].Nodes[0].Nodes.Add("lethead");
            tree.Nodes[7].Nodes.Add("PROPERTY");
            tree.Nodes[7].Nodes[1].Nodes.Add("display");
            tree.Nodes[7].Nodes[1].Nodes.Add("block");
            tree.Nodes[7].Nodes.Add("PROPERTY");
            tree.Nodes[7].Nodes[2].Nodes.Add("column-count");
            tree.Nodes[7].Nodes[2].Nodes.Add("1");
            tree.Nodes[7].Nodes.Add("PROPERTY");
            tree.Nodes[7].Nodes[3].Nodes.Add("margin-right");
            tree.Nodes[7].Nodes[3].Nodes.Add(".75");
            tree.Nodes[7].Nodes[3].Nodes.Add("in");
            tree.Nodes[7].Nodes.Add("PROPERTY");
            tree.Nodes[7].Nodes[4].Nodes.Add("padding-bottom");
            tree.Nodes[7].Nodes[4].Nodes.Add("9");
            tree.Nodes[7].Nodes[4].Nodes.Add("pt");
            expectedTLNode.Nodes.Add((TreeNode)tree.Clone());
        }

        public void ExpectedPseudoLevelTree()
        {
            expectedPseudoNode.Nodes.Clear();
            var tree = new TreeNode();
            tree.Text = "ROOT";
            tree.Nodes.Add("RULE");
            tree.Nodes[0].Nodes.Add("CLASS");
            tree.Nodes[0].Nodes[0].Nodes.Add("slots");
            tree.Nodes[0].Nodes.Add("PSEUDO");
            tree.Nodes[0].Nodes[1].Nodes.Add("before");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[2].Nodes.Add("content");
            tree.Nodes[0].Nodes[2].Nodes.Add(": ");
            tree.Nodes.Add("RULE");
            tree.Nodes[1].Nodes.Add("CLASS");
            tree.Nodes[1].Nodes[0].Nodes.Add("slots");
            tree.Nodes[1].Nodes.Add("PARENTOF");
            tree.Nodes[1].Nodes.Add("CLASS");
            tree.Nodes[1].Nodes[2].Nodes.Add("xitem");
            tree.Nodes[1].Nodes.Add("PRECEDES");
            tree.Nodes[1].Nodes.Add("CLASS");
            tree.Nodes[1].Nodes[4].Nodes.Add("xitem");
            tree.Nodes[1].Nodes.Add("PSEUDO");
            tree.Nodes[1].Nodes[5].Nodes.Add("before");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[6].Nodes.Add("content");
            tree.Nodes[1].Nodes[6].Nodes.Add(", ");
            tree.Nodes.Add("RULE");
            tree.Nodes[2].Nodes.Add("CLASS");
            tree.Nodes[2].Nodes[0].Nodes.Add("xitem");
            tree.Nodes[2].Nodes[0].Nodes.Add("ATTRIB");
            tree.Nodes[2].Nodes[0].Nodes[1].Nodes.Add("lang");
            tree.Nodes[2].Nodes[0].Nodes[1].Nodes.Add("ATTRIBEQUAL");
            tree.Nodes[2].Nodes[0].Nodes[1].Nodes.Add(("'" + "en" + "'"));
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[1].Nodes.Add("font-size");
            tree.Nodes[2].Nodes[1].Nodes.Add("10");
            tree.Nodes[2].Nodes[1].Nodes.Add("pt");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[2].Nodes.Add("text-decoration");
            tree.Nodes[2].Nodes[2].Nodes.Add("none");
            expectedPseudoNode.Nodes.Add((TreeNode)tree.Clone());
        }

        public void ExpectedParentDualClassTree()
        {
            expectedPDNode.Nodes.Clear();
            var tree = new TreeNode();
            tree.Text = "ROOT";
            tree.Nodes.Add("RULE");
            tree.Nodes[0].Nodes.Add("CLASS");
            tree.Nodes[0].Nodes[0].Nodes.Add("locator");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[1].Nodes.Add("text-align");
            tree.Nodes[0].Nodes[1].Nodes.Add("center");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[2].Nodes.Add("font-size");
            tree.Nodes[0].Nodes[2].Nodes.Add("10");
            tree.Nodes[0].Nodes[2].Nodes.Add("pt");
            tree.Nodes.Add("RULE");
            tree.Nodes[1].Nodes.Add("CLASS");
            tree.Nodes[1].Nodes[0].Nodes.Add("locator");
            tree.Nodes[1].Nodes.Add("CLASS");
            tree.Nodes[1].Nodes[1].Nodes.Add("letter");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[2].Nodes.Add("font-weight");
            tree.Nodes[1].Nodes[2].Nodes.Add("bold");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[3].Nodes.Add("font-size");
            tree.Nodes[1].Nodes[3].Nodes.Add("20");
            tree.Nodes[1].Nodes[3].Nodes.Add("pt");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[4].Nodes.Add("color");
            tree.Nodes[1].Nodes[4].Nodes.Add("BLUE");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[5].Nodes.Add("text-align");
            tree.Nodes[1].Nodes[5].Nodes.Add("center");
            tree.Nodes.Add("RULE");
            tree.Nodes[2].Nodes.Add("CLASS");
            tree.Nodes[2].Nodes[0].Nodes.Add("subentry");
            tree.Nodes[2].Nodes.Add("CLASS");
            tree.Nodes[2].Nodes[1].Nodes.Add("letter");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[2].Nodes.Add("font-weight");
            tree.Nodes[2].Nodes[2].Nodes.Add("bold");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[3].Nodes.Add("font-size");
            tree.Nodes[2].Nodes[3].Nodes.Add("20");
            tree.Nodes[2].Nodes[3].Nodes.Add("pt");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[4].Nodes.Add("color");
            tree.Nodes[2].Nodes[4].Nodes.Add("GREEN");
            expectedPDNode.Nodes.Add((TreeNode)tree.Clone());
        }

        public void ExpectedInhertitParentClassPropertyTree()
        {
            expectedIPCPNode.Nodes.Clear();
            var tree = new TreeNode();
            tree.Text = "ROOT";
            tree.Nodes.Add("RULE");
            tree.Nodes[0].Nodes.Add("CLASS");
            tree.Nodes[0].Nodes[0].Nodes.Add("letter");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[1].Nodes.Add("text-align");
            tree.Nodes[0].Nodes[1].Nodes.Add("center");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[2].Nodes.Add("font-style");
            tree.Nodes[0].Nodes[2].Nodes.Add("italic");
            tree.Nodes.Add("RULE");
            tree.Nodes[1].Nodes.Add("CLASS");
            tree.Nodes[1].Nodes[0].Nodes.Add("letter");
            tree.Nodes[1].Nodes[0].Nodes.Add("ATTRIB");
            tree.Nodes[1].Nodes[0].Nodes[1].Nodes.Add("class");
            tree.Nodes[1].Nodes[0].Nodes[1].Nodes.Add("HASVALUE");
            tree.Nodes[1].Nodes[0].Nodes[1].Nodes.Add(("'" + "current" + "'"));
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[1].Nodes.Add("text-align");
            tree.Nodes[1].Nodes[1].Nodes.Add("left");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[2].Nodes.Add("color");
            tree.Nodes[1].Nodes[2].Nodes.Add("blue");
            tree.Nodes.Add("RULE");
            tree.Nodes[2].Nodes.Add("CLASS");
            tree.Nodes[2].Nodes[0].Nodes.Add("locator");
            tree.Nodes[2].Nodes.Add("CLASS");
            tree.Nodes[2].Nodes[1].Nodes.Add("letter");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[2].Nodes.Add("background-color");
            tree.Nodes[2].Nodes[2].Nodes.Add("#00ff00");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[3].Nodes.Add("color");
            tree.Nodes[2].Nodes[3].Nodes.Add("red");
            expectedIPCPNode.Nodes.Add((TreeNode)tree.Clone());
        }

        public void ExpectedRemoveUnderscoreTree()
        {
            expectedRUSNode.Nodes.Clear();
            var tree = new TreeNode();
            tree.Text = "ROOT";
            tree.Nodes.Add("RULE");
            tree.Nodes[0].Nodes.Add("CLASS");
            tree.Nodes[0].Nodes[0].Nodes.Add("articleL1");
            tree.Nodes[0].Nodes.Add("PARENTOF");
            tree.Nodes[0].Nodes.Add("CLASS");
            tree.Nodes[0].Nodes[2].Nodes.Add("pronunciationL2");
            tree.Nodes[0].Nodes.Add("PRECEDES");
            tree.Nodes[0].Nodes.Add("CLASS");
            tree.Nodes[0].Nodes[4].Nodes.Add("pronunciationL3");
            tree.Nodes[0].Nodes.Add("PSEUDO");
            tree.Nodes[0].Nodes[5].Nodes.Add("before");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[6].Nodes.Add("content");
            tree.Nodes[0].Nodes[6].Nodes.Add("1 ");
            tree.Nodes.Add("RULE");
            tree.Nodes[1].Nodes.Add("CLASS");
            tree.Nodes[1].Nodes[0].Nodes.Add("entryL2");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[1].Nodes.Add("color");
            tree.Nodes[1].Nodes[1].Nodes.Add("red");
            expectedRUSNode.Nodes.Add((TreeNode)tree.Clone());
        }

        public void ExpectedMultiLineSyntaxTree()
        {
            expectedMLSNode.Nodes.Clear();
            var tree = new TreeNode();
            tree.Text = "ROOT";
            tree.Nodes.Add("RULE");
            tree.Nodes[0].Nodes.Add("CLASS");
            tree.Nodes[0].Nodes[0].Nodes.Add("letter");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[1].Nodes.Add("text-align");
            tree.Nodes[0].Nodes[1].Nodes.Add("center");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[2].Nodes.Add("width");
            tree.Nodes[0].Nodes[2].Nodes.Add("100");
            tree.Nodes[0].Nodes[2].Nodes.Add("%");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[3].Nodes.Add("margin-top");
            tree.Nodes[0].Nodes[3].Nodes.Add("18");
            tree.Nodes[0].Nodes[3].Nodes.Add("pt");
            tree.Nodes.Add("RULE");
            tree.Nodes[1].Nodes.Add("CLASS");
            tree.Nodes[1].Nodes[0].Nodes.Add("etymologygloss");
            tree.Nodes[1].Nodes.Add("PSEUDO");
            tree.Nodes[1].Nodes[1].Nodes.Add("after");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[2].Nodes.Add("display");
            tree.Nodes[1].Nodes[2].Nodes.Add("inline");
            tree.Nodes.Add("RULE");
            tree.Nodes[2].Nodes.Add("CLASS");
            tree.Nodes[2].Nodes[0].Nodes.Add("definitionL2");
            tree.Nodes[2].Nodes.Add("PSEUDO");
            tree.Nodes[2].Nodes[1].Nodes.Add("after");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[2].Nodes.Add("display");
            tree.Nodes[2].Nodes[2].Nodes.Add("inline");
            tree.Nodes.Add("RULE");
            tree.Nodes[3].Nodes.Add("CLASS");
            tree.Nodes[3].Nodes[0].Nodes.Add("xitem");
            tree.Nodes[3].Nodes.Add("PARENTOF");
            tree.Nodes[3].Nodes.Add("CLASS");
            tree.Nodes[3].Nodes[2].Nodes.Add("sense");
            tree.Nodes[3].Nodes.Add("PRECEDES");
            tree.Nodes[3].Nodes.Add("CLASS");
            tree.Nodes[3].Nodes[4].Nodes.Add("sense");
            tree.Nodes[3].Nodes.Add("PSEUDO");
            tree.Nodes[3].Nodes[5].Nodes.Add("before");
            tree.Nodes[3].Nodes.Add("PROPERTY");
            tree.Nodes[3].Nodes[6].Nodes.Add("display");
            tree.Nodes[3].Nodes[6].Nodes.Add("inline");
            tree.Nodes.Add("RULE");
            tree.Nodes[4].Nodes.Add("TAG");
            tree.Nodes[4].Nodes[0].Nodes.Add("oxes");
            tree.Nodes[4].Nodes.Add("PROPERTY");
            tree.Nodes[4].Nodes[1].Nodes.Add("display");
            tree.Nodes[4].Nodes[1].Nodes.Add("block");
            tree.Nodes.Add("RULE");
            tree.Nodes[5].Nodes.Add("TAG");
            tree.Nodes[5].Nodes[0].Nodes.Add("oxesText");
            tree.Nodes[5].Nodes.Add("PROPERTY");
            tree.Nodes[5].Nodes[1].Nodes.Add("display");
            tree.Nodes[5].Nodes[1].Nodes.Add("block");
            tree.Nodes.Add("RULE");
            tree.Nodes[6].Nodes.Add("TAG");
            tree.Nodes[6].Nodes[0].Nodes.Add("chapter");
            tree.Nodes[6].Nodes.Add("PROPERTY");
            tree.Nodes[6].Nodes[1].Nodes.Add("display");
            tree.Nodes[6].Nodes[1].Nodes.Add("block");
            tree.Nodes.Add("RULE");
            tree.Nodes[7].Nodes.Add("TAG");
            tree.Nodes[7].Nodes[0].Nodes.Add("section");
            tree.Nodes[7].Nodes.Add("PROPERTY");
            tree.Nodes[7].Nodes[1].Nodes.Add("display");
            tree.Nodes[7].Nodes[1].Nodes.Add("block");
            tree.Nodes.Add("RULE");
            tree.Nodes[8].Nodes.Add("TAG");
            tree.Nodes[8].Nodes[0].Nodes.Add("title");
            tree.Nodes[8].Nodes.Add("PROPERTY");
            tree.Nodes[8].Nodes[1].Nodes.Add("display");
            tree.Nodes[8].Nodes[1].Nodes.Add("block");
            expectedMLSNode.Nodes.Add((TreeNode)tree.Clone());
        }

        public void ExpectedOxesPropertyTree()
        {
            expectedOSNode.Nodes.Clear();
            var tree = new TreeNode();
            tree.Text = "ROOT";
            tree.Nodes.Add("RULE");
            tree.Nodes[0].Nodes.Add("TAG");
            tree.Nodes[0].Nodes[0].Nodes.Add("h1");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[1].Nodes.Add("line-height");
            tree.Nodes[0].Nodes[1].Nodes.Add("13");
            tree.Nodes[0].Nodes[1].Nodes.Add("pt");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[2].Nodes.Add("font-weight");
            tree.Nodes[0].Nodes[2].Nodes.Add("bold");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[3].Nodes.Add("padding");
            tree.Nodes[0].Nodes[3].Nodes.Add("0");
            tree.Nodes[0].Nodes.Add("PROPERTY");
            tree.Nodes[0].Nodes[4].Nodes.Add("text-align");
            tree.Nodes[0].Nodes[4].Nodes.Add("left");
            tree.Nodes.Add("RULE");
            tree.Nodes[1].Nodes.Add("TAG");
            tree.Nodes[1].Nodes[0].Nodes.Add("h2");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[1].Nodes.Add("line-height");
            tree.Nodes[1].Nodes[1].Nodes.Add("13");
            tree.Nodes[1].Nodes[1].Nodes.Add("pt");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[2].Nodes.Add("font-weight");
            tree.Nodes[1].Nodes[2].Nodes.Add("bold");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[3].Nodes.Add("padding");
            tree.Nodes[1].Nodes[3].Nodes.Add("0");
            tree.Nodes[1].Nodes.Add("PROPERTY");
            tree.Nodes[1].Nodes[4].Nodes.Add("text-align");
            tree.Nodes[1].Nodes[4].Nodes.Add("left");
            tree.Nodes.Add("RULE");
            tree.Nodes[2].Nodes.Add("TAG");
            tree.Nodes[2].Nodes[0].Nodes.Add("h3");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[1].Nodes.Add("line-height");
            tree.Nodes[2].Nodes[1].Nodes.Add("13");
            tree.Nodes[2].Nodes[1].Nodes.Add("pt");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[2].Nodes.Add("font-weight");
            tree.Nodes[2].Nodes[2].Nodes.Add("bold");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[3].Nodes.Add("padding");
            tree.Nodes[2].Nodes[3].Nodes.Add("0");
            tree.Nodes[2].Nodes.Add("PROPERTY");
            tree.Nodes[2].Nodes[4].Nodes.Add("text-align");
            tree.Nodes[2].Nodes[4].Nodes.Add("left");
            tree.Nodes.Add("RULE");
            tree.Nodes[3].Nodes.Add("TAG");
            tree.Nodes[3].Nodes[0].Nodes.Add("h4");
            tree.Nodes[3].Nodes.Add("PROPERTY");
            tree.Nodes[3].Nodes[1].Nodes.Add("line-height");
            tree.Nodes[3].Nodes[1].Nodes.Add("13");
            tree.Nodes[3].Nodes[1].Nodes.Add("pt");
            tree.Nodes[3].Nodes.Add("PROPERTY");
            tree.Nodes[3].Nodes[2].Nodes.Add("font-weight");
            tree.Nodes[3].Nodes[2].Nodes.Add("bold");
            tree.Nodes[3].Nodes.Add("PROPERTY");
            tree.Nodes[3].Nodes[3].Nodes.Add("padding");
            tree.Nodes[3].Nodes[3].Nodes.Add("0");
            tree.Nodes[3].Nodes.Add("PROPERTY");
            tree.Nodes[3].Nodes[4].Nodes.Add("text-align");
            tree.Nodes[3].Nodes[4].Nodes.Add("left");
            tree.Nodes.Add("RULE");
            tree.Nodes[4].Nodes.Add("TAG");
            tree.Nodes[4].Nodes[0].Nodes.Add("h1");
            tree.Nodes[4].Nodes.Add("CLASS");
            tree.Nodes[4].Nodes[1].Nodes.Add("cover");
            tree.Nodes[4].Nodes.Add("PROPERTY");
            tree.Nodes[4].Nodes[2].Nodes.Add("page");
            tree.Nodes[4].Nodes[2].Nodes.Add("cover_page");
            tree.Nodes[4].Nodes.Add("PROPERTY");
            tree.Nodes[4].Nodes[3].Nodes.Add("line-height");
            tree.Nodes[4].Nodes[3].Nodes.Add("1");
            tree.Nodes[4].Nodes[3].Nodes.Add("em");
            tree.Nodes[4].Nodes.Add("PROPERTY");
            tree.Nodes[4].Nodes[4].Nodes.Add("text-align");
            tree.Nodes[4].Nodes[4].Nodes.Add("center");
            tree.Nodes.Add("RULE");
            tree.Nodes[5].Nodes.Add("TAG");
            tree.Nodes[5].Nodes[0].Nodes.Add("h3");
            tree.Nodes[5].Nodes.Add("CLASS");
            tree.Nodes[5].Nodes[1].Nodes.Add("section");
            tree.Nodes[5].Nodes.Add("PSEUDO");
            tree.Nodes[5].Nodes[2].Nodes.Add("contains");
            tree.Nodes[5].Nodes[2].Nodes.Add("(");
            tree.Nodes[5].Nodes[2].Nodes.Add("'" + "Lamatua Yesus peka,roti" + "'");
            tree.Nodes[5].Nodes[2].Nodes.Add(")");
            tree.Nodes[5].Nodes.Add("PROPERTY");
            tree.Nodes[5].Nodes[3].Nodes.Add("font-size");
            tree.Nodes[5].Nodes[3].Nodes.Add("9");
            tree.Nodes[5].Nodes[3].Nodes.Add("pt");
            tree.Nodes[5].Nodes.Add("PROPERTY");
            tree.Nodes[5].Nodes[4].Nodes.Add("string-set");
            tree.Nodes[5].Nodes[4].Nodes.Add("h3");
            tree.Nodes.Add("RULE");
            tree.Nodes[6].Nodes.Add("ANY");
            tree.Nodes[6].Nodes[0].Nodes.Add("ATTRIB");
            tree.Nodes[6].Nodes[0].Nodes[0].Nodes.Add("size");
            tree.Nodes[6].Nodes[0].Nodes[0].Nodes.Add("ATTRIBEQUAL");
            tree.Nodes[6].Nodes[0].Nodes[0].Nodes.Add("'" + "4lines" + "'");
            tree.Nodes[6].Nodes.Add("PROPERTY");
            tree.Nodes[6].Nodes[1].Nodes.Add("height");
            tree.Nodes[6].Nodes[1].Nodes.Add("52");
            tree.Nodes[6].Nodes[1].Nodes.Add("pt");
            expectedOSNode.Nodes.Add((TreeNode)tree.Clone());
        }
        #endregion

        #region Tests

        /// <summary>
        ///A test for UnicodeConversion
        ///</summary>
        [Test]
        public void UnicodeConversionTest1()
        {
            _methodName = "UnicodeConversionTest1";
            const string parameter = "\\25ba";
            string expected = Common.ConvertUnicodeToString(parameter);
            //const string expected = Common.ConvertUnicodeToString("\\25ba");
            string actual = Common.UnicodeConversion(parameter);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for UnicodeConversion
        ///</summary>
        [Test]
        public void UnicodeConversionTest2()
        {
            _methodName = "UnicodeConversionTest2";
            const string parameter = "\\00A0";
            string actual = Common.UnicodeConversion(parameter);
            Assert.IsTrue(actual.Length == 1, _methodName + " test failed");
        }

        /// <summary>
        ///A test for UnicodeConversion
        ///</summary>
        [Test]
        public void UnicodeConversionTest3()
        {
            _methodName = "UnicodeConversionTest3";
            const string parameter = ", ";
            const string expected = ", ";
            string actual = Common.UnicodeConversion(parameter);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for UnicodeConversion
        ///</summary>
        [Test]
        public void UnicodeConversionTest4()
        {
            _methodName = "UnicodeConversionTest4";
            const string parameter = "\\U8658";
            string actual = Common.UnicodeConversion(parameter);
            const string expected = "U8658";
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for UnicodeConversion
        ///</summary>
        [Test]
        public void UnicodeConversionTest5()
        {
            _methodName = "UnicodeConversionTest5";
            string parameter = string.Empty;
            string actual = Common.UnicodeConversion(parameter);
            Assert.IsTrue(actual.Length == 0, _methodName + " test failed.");
        }

        /// <summary>
        ///A test for GetErrorReport -  Testing with Error File
        ///</summary>
        [Test]
        public void GetErrorReportTest1()
        {
            _methodName = "GetErrorReportTest1";
            target.ErrorList.Clear();
            inputCSSPath = Common.PathCombine(testPath ,"GetErrorReportTest1.css");
            target.GetErrorReport(inputCSSPath);
            int ErrCount = target.ErrorList.Count;
            Assert.IsTrue(ErrCount > 0, _methodName + "test failed.");
        }

        /// <summary>
        ///A test for GetErrorReport - Testing with Valid File
        ///</summary>
        [Test]
        public void GetErrorReportTest2()
        {
            _methodName = "GetErrorReportTest2";
            target.ErrorList.Clear();
            inputCSSPath = Common.PathCombine(testPath , "GetErrorReportTest2.css");
            target.GetErrorReport(inputCSSPath);
            int ErrCount = target.ErrorList.Count;
            Assert.IsTrue(ErrCount == 0, _methodName + "test failed.");
        }

        /// <summary>
        ///A test for GetErrorReport - Testing with File which is not exist
        ///</summary>
        [Test]
        public void GetErrorReportTest3()
        {
            _methodName = "GetErrorReportTest3";
            target.ErrorList.Clear();
            inputCSSPath = Common.PathCombine(testPath, "InValidFilePath.css");
            target.GetErrorReport(inputCSSPath);
            int ErrCount = target.ErrorList.Count;
            Assert.IsTrue(ErrCount == 0, _methodName + "test failed.");
        }

        [Test]
        public void BuildTreeTest1()
        {
            _methodName = "BuildTreeTest1 - File Not Exist ";
            inputCSSPath = Common.PathCombine(testPath, "InValidFilePath.css");
            TreeNode actual = target.BuildTree(inputCSSPath);
            Assert.IsTrue(actual.Nodes.Count == 0, _methodName + "test failed.");
        }

        [Test]
        public void BuildTreeTest2()
        {
            _methodName = "BuildTreeTest2 - EmptyCSSValidate ";
            RemoveCSS();
            inputCSSPath = Common.PathCombine(testPath ,"emptyCSS.css");
            OutputNode.Nodes.Add((TreeNode)target.BuildTree(inputCSSPath).Clone());
            Assert.IsTrue(OutputNode.Nodes.Count == 1, _methodName + " test failed");
        }

        [Test]
        public void BuildTreeTest3()
        {
            _methodName = "BuildTreeTest3 - EmptyCSSValidate1 ";
            RemoveCSS();
            var clsCSS = new CssParser();
			var filePath = Common.PathCombine(testPath, "emptyCSS.css");
            var Node = clsCSS.BuildTree(filePath);
            EmptyNode.Nodes.Add((TreeNode)Node.Clone());
            var cnt = EmptyNode.Nodes[0].Nodes.Count;
            Assert.AreEqual(0, cnt, _methodName + " test failed");
        }

        [Test]
        public void BuildTreeTest4()
        {
            _methodName = "BuildTreeTest4 - OneLevelTreeValidate ";
            RemoveCSS();
            ResultOneLevelTree();
            ExpectedOneLevelTree();
            bool valid = CompareRecursiveTree(expectedNode, resultNode);
            Assert.AreEqual(true, valid, _methodName + " test failed");
        }

        [Test]
        public void BuildTreeTest5()
        {
            _methodName = "BuildTreeTest5 - SingleClassCSSValidate ";
            RemoveCSS();
            ResultSingleClassCSS();
            ExpectedSingleClassCSS();
            bool valid = CompareRecursiveTree(expectedSCNode, resultSCNode);
            Assert.AreEqual(true, valid, _methodName + " test failed");
        }

        [Test]
        public void BuildTreeTest6()
        {
            _methodName = "BuildTreeTest6 - DoubleClassCSSValidate ";
            RemoveCSS();
            ResultDoubleClassCSS();
            ExpectedDoubleClassCSS();
            bool valid = CompareRecursiveTree(expectedDCNode, resultDCNode);
            Assert.AreEqual(true, valid, _methodName + " test failed");
        }

        [Test]
        public void BuildTreeTest7()
        {
            _methodName = "BuildTreeTest7 - TwoLevelTreeValidate ";
            RemoveCSS();
            ResultTwoLevelTree();
            ExpectedTwoLevelTree();
            bool valid = CompareRecursiveTree(expectedTLNode, resultTLNode);
            Assert.AreEqual(true, valid, _methodName + " test failed");
        }

        [Test]
        public void BuildTreeTest8()
        {
            _methodName = "BuildTreeTest8 - PseudoTreeValidate ";
            RemoveCSS();
            ResultPseudoLevelTree();
            ExpectedPseudoLevelTree();
            bool valid = CompareRecursiveTree(expectedPseudoNode, resultPseudoNode);
            Assert.AreEqual(true, valid, _methodName + " test failed");
        }

        [Test]
        public void BuildTreeTest9()
        {
            _methodName = "BuildTreeTest9 - ParentDualClassTreeValidate ";
            RemoveCSS();
            ResultParentDualClassTree();
            ExpectedParentDualClassTree();
            bool valid = CompareRecursiveTree(expectedPDNode, resultPDNode);
            Assert.AreEqual(true, valid, _methodName + " test failed");
        }

        //TD-90 Open Office .locator .letter
        [Test]
        public void BuildTreeTest10()
        {
            _methodName = "BuildTreeTest10 - InheritParentClassPropertyTestA ";
            RemoveCSS();
            ResultInhertitParentClassPropertyTree();
            ExpectedInhertitParentClassPropertyTree();
            bool valid = CompareRecursiveTree(expectedIPCPNode, resultIPCPNode);
            Assert.AreEqual(true, valid, _methodName + " test failed");
        }

        [Test]
        // To remove "_" from Classnames ( letter_L2 == letterL2)
        public void BuildTreeTest11()
        {
            _methodName = "BuildTreeTest11 - RemoveUnderscore ";
            RemoveCSS();
            ResultRemoveUnderscoreTree();
            ExpectedRemoveUnderscoreTree();
            bool valid = CompareRecursiveTree(expectedRUSNode, resultRUSNode);
            Assert.AreEqual(true, valid, _methodName + " test failed");
        }

        [Test]
        // To split MultiLine syntax into Individual (x:before,y:before,z:before{content:"Test"})
        public void BuildTreeTest12()
        {
            _methodName = "BuildTreeTest12 - MultiLineSyntax ";
            RemoveCSS();
            ResultMultiLineSyntaxTree();
            ExpectedMultiLineSyntaxTree();
            bool valid = CompareRecursiveTree(expectedMLSNode, resultMLSNode);
            Assert.AreEqual(true, valid, _methodName + " test failed");
        }

        [Test]
        // Handled new case in Oxes_test.CSS for TD-244
        public void BuildTreeTest13()
        {
            _methodName = "BuildTreeTest13 - OxesProperty ";
            RemoveCSS();
            ResultOxesPropertyTree();
            ExpectedOxesPropertyTree();
            bool valid = CompareRecursiveTree(expectedOSNode, resultOSNode);
            Assert.AreEqual(true, valid, _methodName + " test failed");
        }
        #endregion

        #region Private Funtions
        /// <summary>
        /// To compare the two treenodes 
        /// </summary>
        /// <param name="treeNodeOne">first treenode</param>
        /// <param name="treeNodeTwo">second treenode</param>
        /// <returns></returns>
        private bool CompareRecursiveTree(TreeNode treeNodeOne, TreeNode treeNodeTwo)
        {
            if (treeNodeOne == null || treeNodeTwo == null)
            {
                return treeNodeOne == treeNodeTwo;
            }
            if ((treeNodeOne.Text.ToLower() != treeNodeTwo.Text.ToLower()) || (treeNodeOne.Nodes.Count != treeNodeTwo.Nodes.Count))
            {
                return false;
            }
            for (int i = 0; i < treeNodeOne.Nodes.Count; i++)
            {
                if (!CompareRecursiveTree(treeNodeOne.Nodes[i], treeNodeTwo.Nodes[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public void RemoveCSS()
        {
            var filePath = testPath + "/MergedCSS.css";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        #endregion
    }
}