// --------------------------------------------------------------------------------------------
// <copyright file="InStoryTest.cs" from='2009' to='2014' company='SIL International'>
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
    public class InStoryTest : ValidateXMLFile
    {
        #region Private Variables
        private string _inputCSS;
        private string _inputXHTML;
        private string _outputPath;
        private string _expectedPath;
        private string _outputStory;
        private string _outputStyles;
        private Dictionary<string, string> _expected = new Dictionary<string, string>();
        private string _testFolderPath = string.Empty;
        Dictionary<string, Dictionary<string, string>> _idAllClass = new Dictionary<string, Dictionary<string, string>>();
        private InStyles _stylesXML;
        private InStory _storyXML;
        private readonly ArrayList headwordStyles = new ArrayList();
        PublicationInformation projInfo =  new PublicationInformation();
        #endregion

        #region Public Variables
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
            ClassProperty = _expected;  //Note: All Reference address initialized here
            _outputPath = Common.PathCombine(_testFolderPath, "output");
            _expectedPath = Common.PathCombine(_testFolderPath, "Expected");
            _expectedPath = Common.PathCombine(_expectedPath, "BuangExpect");
            _outputStyles = Common.PathCombine(_outputPath, "Resources");
            _outputStory = Common.PathCombine(_outputPath, "Stories");
            projInfo.TempOutputFolder = _outputPath;
            _cssProperty = new Dictionary<string, Dictionary<string, string>>();
            Common.SupportFolder = "";
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, "/../../DistFiles");
            Common.CopyFolderandSubFolder(_expectedPath, _outputPath, true);
			Common.UseAfterBeforeProcess = true;
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
        public void MultiParent()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/MultiParent.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/MultiParent.xhtml");
            projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
            ExportProcess();

            _expected.Clear();
			string styleName = "xsensenumber_1";
            _expected.Add(styleName, "x red ");
            XPath = "//ParagraphStyleRange/CharacterStyleRange[2][@AppliedCharacterStyle = \"CharacterStyle/" +
                    styleName + "\"]";
            bool result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
			styleName = "xsensenumber_2";
            _expected.Add(styleName, "2.1) ");
            XPath = "//ParagraphStyleRange[2]/CharacterStyleRange[8][@AppliedCharacterStyle = \"CharacterStyle/" +
                    styleName + "\"]";
            result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, styleName + " test Failed");
        }
        [Test]
        public void Counter1()
        {
	        Common.UseAfterBeforeProcess = true;
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Counter1.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/Counter.xhtml");
            ExportProcess();
			string classname = "firstoftype sense_2";
            XPath = "//ParagraphStyleRange/CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + classname + "\"]";
            string content = "1.2";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, classname + " test Failed");
			classname = "lastoftype sense_2";
            XPath = "//ParagraphStyleRange/CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + classname + "\"][2]";

            content = "2.4";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, classname + " test Failed");
        }

        [Test]
        public void Counter2()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Counter2.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/Counter.xhtml");
            ExportProcess();
			string classname = "firstoftype sense_2";
            XPath = "//ParagraphStyleRange/CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + classname + "\"]";
            string content = "1.0.0";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, classname + " test Failed");

			classname = "firstoftype sense_2";
            XPath = "//ParagraphStyleRange/CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + classname + "\"][2]";

            content = "2.0.4";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, classname + " test Failed");
        }
        [Test]
        public void Counter3()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Counter3.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/Counter.xhtml");
            ExportProcess();
			string classname = "firstoftype sense_2";
            XPath = "//ParagraphStyleRange/CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + classname + "\"]";
            string content = "1.1";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, classname + " test Failed");

			classname = "lastoftype sense_2";
            XPath = "//ParagraphStyleRange/CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + classname + "\"]";

            content = "1.2";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, classname + " test Failed");
        }

        [Test]
        public void FontFamily4()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/FontFamily4.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/FontFamily4.xhtml");
            ExportProcess();

            _expected.Add("PointSize", "32");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"a_1\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }

        #region NestedDiv
        [Test]
        public void NestedDiv1()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedDivCase1.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedDivCase1.xhtml");
            ExportProcess();

            string styleName = "t1_1";
            XPath = "//ParagraphStyleRange[1][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            string content = "T1 class ";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t2_1";
            XPath = "//ParagraphStyleRange[2][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T2 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t3_1";
            XPath = "//ParagraphStyleRange[3][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T3 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t2_1";
            XPath = "//ParagraphStyleRange[4][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T2 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t1_1";
            XPath = "//ParagraphStyleRange[5][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T1 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        [Test]
        public void NestedDiv2()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedDivCase2.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedDivCase2.xhtml");
            ExportProcess();

            string styleName = "t1_1";
            XPath = "//ParagraphStyleRange[1][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            string content = "T1 class ";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t2_1";
            XPath = "//ParagraphStyleRange[2][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T2 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t3_1";
            XPath = "//ParagraphStyleRange[3][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T3 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t2_1";
            XPath = "//ParagraphStyleRange[4][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T2 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t4_1";
            XPath = "//ParagraphStyleRange[5][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T4 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t1_1";
            XPath = "//ParagraphStyleRange[6][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T1 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

        }

        [Test]
        public void NestedDiv3()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedDivCase3.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedDivCase3.xhtml");
            ExportProcess();

            string styleName = "t1_1";
            XPath = "//ParagraphStyleRange[1][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            string content = "T1 class ";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t3_1";
            XPath = "//ParagraphStyleRange[2][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T3 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t4_1";
            XPath = "//ParagraphStyleRange[3][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T4 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t1_1";
            XPath = "//ParagraphStyleRange[4][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T1 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

        }


        [Test]
        public void NestedDiv4()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedDivCase4.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedDivCase4.xhtml");
            ExportProcess();

            string styleName = "t1_1";
            XPath = "//ParagraphStyleRange[1][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            string content = "T1 class ";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t2_1";
            XPath = "//ParagraphStyleRange[2][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T2 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t3_1";
            XPath = "//ParagraphStyleRange[3][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T3 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t4_1";
            XPath = "//ParagraphStyleRange[4][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T4 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t2_1";
            XPath = "//ParagraphStyleRange[5][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T2 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t4_1";
            XPath = "//ParagraphStyleRange[6][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T4 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t4_1";
            XPath = "//ParagraphStyleRange[7][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T4 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "t1_1";
            XPath = "//ParagraphStyleRange[8][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "T1 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

        }

        [Test]
        public void NestedDiv5()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedDiv5.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedDiv.xhtml");
            ExportProcess();

            _expected.Add("PointSize", "15");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"t3_1\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }

        [Test]
        public void NestedDiv6()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedDiv6.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedDiv.xhtml");
            ExportProcess();

            _expected.Add("PointSize", "15");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"t3_1\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }


        [Test]
        public void NestedSpan1()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedSpan1.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/NestedSpan1.xhtml");
            ExportProcess();

            _expected.Add("PointSize", "8");
            XPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"t3_1\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }


        [Test]
        public void Font1()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Font1.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/Font1.xhtml");
            ExportProcess();

            _expected.Add("PointSize", "8");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"t2_1\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");

            _expected.Clear();
            _expected.Add("AppliedFont", "Verdana");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + "t2_1" + "\"]/Properties/AppliedFont";
            result = ValidateNodeValue();
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }

        [Test]
        public void FontParent()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/FontParent.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/FontParent.xhtml");
            ExportProcess();

            _expected.Add("AppliedFont", "Arial Black");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + "a_1" + "\"]/Properties/AppliedFont";
            bool result = ValidateNodeValue();
            Assert.IsTrue(result, _inputCSS + " test Failed");

            _expected.Clear();
            _expected.Add("AppliedFont", "Arial Black");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + "a_1" + "\"]/Properties/AppliedFont";
            result = ValidateNodeValue();
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }

        #endregion

        [Test]
        public void ColumnGap3()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/ColumnGap3.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/ColumnGap3.xhtml");
            ExportProcess();
            _expected.Add("TextColumnGutter", "18");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"t1_1\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }

        [Test]
        public void Smaller()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/smaller.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/largersmaller.xhtml");
            ExportProcess();

            _expected.Add("PointSize", "13");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"t2_1\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }

        [Test]
        public void Larger()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/larger.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/largersmaller.xhtml");
            ExportProcess();
            _expected.Add("PointSize", "22");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"t2_1\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }

        [Test]
        public void DisplayBlock()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/DisplayBlock.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/DisplayBlock.xhtml");
            ExportProcess();

            string expected = Common.DirectoryPathReplace(_testFolderPath + "/Expected/Resources/DisplayBlock.xml");
            string output = Common.DirectoryPathReplace(_testFolderPath + "/output/Resources/Styles.xml");
            XmlAssert.AreEqual(expected, output, "DisplayBlock syntax failed in Styles.xml");

            expected = Common.DirectoryPathReplace(_testFolderPath + "/Expected/Stories/DisplayBlock.xml");
            output = Common.DirectoryPathReplace(_testFolderPath + "/output/Stories/Story_1.xml");
            XmlAssert.AreEqual(expected, output, "DisplayBlock syntax failed in Stories.xml");

        }

        [Test]
        public void PseudoBefore()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/PseudoBefore.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/PseudoBefore.xhtml");
            ExportProcess();
            string expected = Common.DirectoryPathReplace(_testFolderPath + "/Expected/Stories/PseudoBefore.xml");
            string output = _testFolderPath + "/output/Stories/Story_1.xml";
            TextFileAssert.AreEqual(expected, output, "PseudoBefore syntax failed in Stories.xml");
        }

        [Test]
        public void AttributeTest1()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Attribute1.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/Attribute1.xhtml");
            ExportProcess();

            _expected.Add("FillColor", "Color/#008000");
            XPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"main_2\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");

            //1
            _expected.Clear();
            _expected.Add("FillColor", "Color/#ffa500");
            XPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"main_3\"]";
            result = ValidateNodeAttribute();
            Assert.IsTrue(result, _inputCSS + " 1test Failed");
            //2
            _expected.Clear();
            _expected.Add("FillColor", "Color/#00ffff");
            XPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"main_4\"]";
            result = ValidateNodeAttribute();
            Assert.IsTrue(result, _inputCSS + " 2test Failed");
            //3
            _expected.Clear();
            _expected.Add("FillColor", "Color/#ffff00");
            XPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"main_5\"]";
            result = ValidateNodeAttribute();
            Assert.IsTrue(result, _inputCSS + " 3test Failed");

            //4
            _expected.Clear();
            _expected.Add("FillColor", "Color/#a52a2a");
            XPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"main_6\"]";
            result = ValidateNodeAttribute();
            Assert.IsTrue(result, _inputCSS + " 4test Failed");
            //5
            _expected.Clear();
            _expected.Add("FillColor", "Color/#0000ff");
            XPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"main_7\"]";
            result = ValidateNodeAttribute();
            Assert.IsTrue(result, _inputCSS + " 5test Failed");
            //6
            _expected.Clear();
            _expected.Add("FillColor", "Color/#ff0000");
            XPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"main_8\"]";
            result = ValidateNodeAttribute();
            Assert.IsTrue(result, _inputCSS + " 6test Failed");
            //7
            _expected.Clear();
            _expected.Add("FillColor", "Color/#ee82ee");
            XPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"main_9\"]";
            result = ValidateNodeAttribute();
            Assert.IsTrue(result, _inputCSS + " 7test Failed");

        }
        [Test]
        public void AttributeTest2()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Attribute2.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/Attribute2.xhtml");
            ExportProcess();

            _expected.Add("FillColor", "Color/#0000ff");
            XPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"main_2\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");

            //1
            _expected.Clear();
            _expected.Add("FillColor", "Color/#ff0000");
            XPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"main_3\"]";
            result = ValidateNodeAttribute();
            Assert.IsTrue(result, _inputCSS + " 1test Failed");
        }

        [Test]
        public void PseudoAfter()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/PseudoAfter.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/PseudoAfter.xhtml");
            ExportProcess();

            _expected.Clear();
			string styleName = "letHead_2";
            XPath = "//CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]//Content";
            string content = "###";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
			styleName = "letHead_3";
            XPath = "//CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]//Content";
            content = "***";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        //TD-1969
        [Test]
        public void PrinceTextReplace()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/PrinceTextReplace.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/PrinceTextReplace.xhtml");
            ExportProcess();

            _expected.Clear();
            string styleName = "Paragraph_1";
            XPath = "//CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]//Content";
            string content = "You can (( wrap text (around any object) in InDesign )). ";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        //TD-1970
        [Test]
        public void FootnoteSpanContent()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/FootnoteSpanContent.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/FootnoteSpanContent.xhtml");
            ExportProcess();

            _expected.Clear();
			string styleName = "NoteTargetReference_.zxx_1";
            XPath = "//CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]//Content";
            string content = "21:1 ";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
			styleName = "AlternateReading_.zxx_1";
            XPath = "//CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]//Content";
            content = "runaround text ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
			styleName = "NoteGeneralParagraph_1";
            XPath = "//CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]//Content";
            content = "Keep in mind that text wrap options apply to the object being wrapped.";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        [Test]
        public void Parent1()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Parent1.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/Parent1.xhtml");
            ExportProcess();

            _expected.Clear();
            string styleName = "main_2";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[1][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            bool result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "main_3";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[2][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");


            _expected.Clear();
            styleName = "main_4";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[3][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "main_3";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[6][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "main_3";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[7][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "main_3";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[8][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");
        }


        [Test]
        public void Precede1()
        {
            _cssProperty.Clear();
            _idAllClass.Clear();
            ClassProperty.Clear();

            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Precede1.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/Precede1.xhtml");
            ExportProcess();

            _expected.Clear();
            string styleName = "main_2";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[1][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            bool result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "main_3";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[2][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");


            _expected.Clear();
            styleName = "main_4";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[3][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "main_3";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[6][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "main_3";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[7][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "main_3";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[8][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");

        }

        [Test]
        public void PseudoContains()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/PseudoContains.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/PseudoContains.xhtml");
            ExportProcess();

            _expected.Clear();
            string styleName = "main_3";
            _expected.Add("AppliedCharacterStyle", "CharacterStyle/" + styleName);
            XPath = "//CharacterStyleRange[1][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            bool result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "main_3";
            _expected.Add("AppliedCharacterStyle", "CharacterStyle/" + styleName);
            XPath = "//CharacterStyleRange[1][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        [Test]
        public void MultiClass()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/multiClass.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/multiClass.xhtml");
            ExportProcess();

            _expected.Clear();
			string styleName = "a b c_1";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[1][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            bool result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "a_1";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[2][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "b_1";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[3][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
			styleName = "a c_1";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[6][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        [Test]
        public void Ancestor()
        {

            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Ancestor.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/Ancestor.xhtml");
            ExportProcess();

            _expected.Clear();
			string styleName = "firstoftype letter_1";
            _expected.Add(styleName, "a");
            XPath = "//ParagraphStyleRange[2]/CharacterStyleRange[1][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            bool result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "letter_1";
            _expected.Add(styleName, "b");
            XPath = "//ParagraphStyleRange[2]/CharacterStyleRange[2][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "letter_1";
            _expected.Add(styleName, "d");
            XPath = "//ParagraphStyleRange[2]/CharacterStyleRange[5][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
			styleName = "current letter_1";
            _expected.Add(styleName, "w");
            XPath = "//ParagraphStyleRange[2]/CharacterStyleRange[24][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, styleName + " test Failed");


        }
        /// <summary>
        /// Tag Test - Ex: span{font-size:8pt;}, span[lang='en']{font-size:18pt;}
        /// </summary>
        [Test]
        public void Tag()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Tag.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/Tag.xhtml");
            ExportProcess();

            _expected.Clear();
            string styleName = "main_2";
            _expected.Add(styleName, "span Tag - Red");
            XPath = "//ParagraphStyleRange/CharacterStyleRange[1][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            bool result = StoryXmlNodeTestForTag(false);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "main_3";
            _expected.Add(styleName, " span Header with lang - Orange");
            XPath = "//ParagraphStyleRange/CharacterStyleRange[2][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            result = StoryXmlNodeTestForTag(false);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
			styleName = "header_1";
            _expected.Add(styleName, " span Header - Blue");
            XPath = "//ParagraphStyleRange/CharacterStyleRange[3][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            result = StoryXmlNodeTestForTag(false);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
			styleName = "header_.en_1";
            _expected.Add(styleName, " span Header with lang - Green");
            XPath = "//ParagraphStyleRange/CharacterStyleRange[4][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            result = StoryXmlNodeTestForTag(false);
            Assert.IsTrue(result, styleName + " test Failed");

        }

        /// <summary>
        /// Tag Test - Ex: span{font-size:8pt;}, span[lang='en']{font-size:18pt;}
        /// </summary>
        [Test]
        public void SpacePreserve()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/SpacePreserve.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/SpacePreserve.xhtml");
            ExportProcess();

            _expected.Clear();
			string styleName = "headword";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange/CharacterStyleRange[2][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]//Content";
			string content = "-End";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        /// <summary>
        /// Tag Test - Ex: .Focused {text-decoration:underline;} span[lang='mcb'] {font-weight:bold;}
        /// </summary>
        [Test]
        public void TagTest1()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/TagTest1.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/TagTest1.xhtml");
            ExportProcess();

            _expected.Clear();
            string styleName = "letData_2";
            _expected.Add(styleName, "chomi");
            XPath = "//ParagraphStyleRange/CharacterStyleRange[1][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            bool result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
			styleName = "Focused_.mcb_1";
            _expected.Add(styleName, "vako");
            XPath = "//ParagraphStyleRange/CharacterStyleRange[2][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "letData_2";
            _expected.Add(styleName, "tagantsi");
            XPath = "//ParagraphStyleRange/CharacterStyleRange[3][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
			styleName = "EmphasizedText_1";
            _expected.Add(styleName, "chuparse la mano");
            XPath = "//ParagraphStyleRange/CharacterStyleRange[4][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "letData_3";
            _expected.Add(styleName, ").");
            XPath = "//ParagraphStyleRange/CharacterStyleRange[5][@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]";
            result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, styleName + " test Failed");

        }
        [Test]
        public void DiplayNone()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/DiplayNone.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/DiplayNone.xhtml");
            ExportProcess();

            _expected.Clear();
            string styleName = "a_1";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[1][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            string content = "class a";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            _expected.Clear();
            styleName = "b_1";
            _expected.Add("AppliedParagraphStyle", "ParagraphStyle/" + styleName);
            XPath = "//ParagraphStyleRange[2][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "class b";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        // verify <p> tag above and below space
        [Test]
        public void Para()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/para.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/para.xhtml");
            ExportProcess();

            string styleName = "a_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            string content = "para text para text para text para text para text para para text";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "b_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "B para text ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "p_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "para text ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        #region List

        // verify <p> tag above and below space
        [Test]
        public void List1()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/List1.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/List1.xhtml");
            ExportProcess();

            string styleName = "ol_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            string content = "oooooooo ";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "olFirst.li_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "one1 ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "ol4Next.li_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "two2 ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "ulFirst.li_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "one3 ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "ul4Next.li_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "two 4";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        [Test]
        public void List2()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/List2.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/List2.xhtml");
            ExportProcess();

            string styleName = "section_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            string content = "section ";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "olFirst.li_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "one1 ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "ol4Next.li_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "two2 ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "ulFirst.li_2";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "one1 ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "ul4Next.li_2";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "two2 ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        [Test]
        public void List3()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/List3.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/List3.xhtml");
            ExportProcess();

            string styleName = "olFirst.li_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            string content = "one1 ";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        [Test]
        public void List4()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/List4.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/List4.xhtml");
            ExportProcess();

            string styleName = "olFirst.li_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            string content = "one1 ";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "ol4Next.li_1";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "two2 ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "olFirst.li_2";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "one1 ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "ol4Next.li_2";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "two2 ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");
        }
        #endregion List

        [Test]
        public void MultiLangHeader1()
        {
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/MultiLangHeader1.xhtml");
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/MultiLangHeader1.css");

            PublicationInformation projInfo = new PublicationInformation();

            projInfo.ProjectPath = Path.GetDirectoryName(_inputXHTML);
            projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
            projInfo.DefaultCssFileWithPath = _inputCSS;

            PreExportProcess preProcessor = new PreExportProcess(projInfo);
            preProcessor.GetTempFolderPath();
            preProcessor.ImagePreprocess(false);
            preProcessor.ReplaceInvalidTagtoSpan("_AllComplexFormEntryBackRefs|LexEntryRef_PrimaryLexemes", "span");
            preProcessor.InsertHiddenChapterNumber();
            preProcessor.InsertHiddenVerseNumber();
            preProcessor.GetDefinitionLanguage();

            projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;
            projInfo.DefaultCssFileWithPath = preProcessor.ProcessedCss;

            Dictionary<string, Dictionary<string, string>> cssClass =
                new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
            preProcessor.InsertEmptyXHomographNumber(cssClass);

            Dictionary<string, Dictionary<string, string>> idAllClass =
                new Dictionary<string, Dictionary<string, string>>();
            InStyles inStyles = new InStyles();
            projInfo.TempOutputFolder = _outputPath;
            idAllClass = inStyles.CreateIDStyles(Common.PathCombine(_outputPath, "Resources"), cssClass);

            InGraphic inGraphic = new InGraphic();
            inGraphic.CreateIDGraphic(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), cssClass,
                                      cssTree.cssBorderColor);

            InStory inStory = new InStory();
            Dictionary<string, ArrayList> StyleName =
                inStory.CreateStory(projInfo, idAllClass, cssTree.SpecificityClass, cssTree.CssClassOrder);

            InMasterSpread inMasterSpread = new InMasterSpread();
            ArrayList masterPageNames =
                inMasterSpread.CreateIDMasterSpread(Common.PathCombine(projInfo.TempOutputFolder, "MasterSpreads"),
                                                    idAllClass, StyleName["TextVariables"]);

            InSpread inSpread = new InSpread();
            inSpread.CreateIDSpread(Common.PathCombine(projInfo.TempOutputFolder, "Spreads"), idAllClass,
                                    StyleName["ColumnClass"]);

            InDesignMap inDesignMap = new InDesignMap();
            inDesignMap.CreateIDDesignMap(projInfo.TempOutputFolder, StyleName["ColumnClass"].Count, masterPageNames,
                                          StyleName["TextVariables"], StyleName["CrossRef"], string.Empty);

            InPreferences inPreferences = new InPreferences();
            inPreferences.CreateIDPreferences(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), idAllClass);

            string classname = "dTextVariablenFirst1";
            string output = Common.PathCombine(projInfo.TempOutputFolder, "designmap.xml");
            string xPath = "//TextVariable[@Self = \"" + classname + "\"]";
            XmlNode node = Common.GetXmlNodeInDesignNamespace(output, xPath);
            string result = node.InnerXml;

            string expected = "<MatchCharacterStylePreference TextBefore=\"\" TextAfter=\"\" AppliedCharacterStyle=\"CharacterStyle/headword_.ii\" SearchStrategy=\"FirstOnPage\" ChangeCase=\"None\" DeleteEndPunctuation=\"false\" />";

            Assert.AreEqual(expected, result, "MultiLangHeader1 test failed");

            classname = "dTextVariablenFirst3";
            output = Common.PathCombine(projInfo.TempOutputFolder, "designmap.xml");
            xPath = "//TextVariable[@Self = \"" + classname + "\"]";
            node = Common.GetXmlNodeInDesignNamespace(output, xPath);
            result = node.InnerXml;
	        expected = "<MatchCharacterStylePreference TextBefore=\"\" TextAfter=\"\" AppliedCharacterStyle=\"CharacterStyle/headwordL3_.ii-fonipa\" SearchStrategy=\"FirstOnPage\" ChangeCase=\"None\" DeleteEndPunctuation=\"false\" />";
            Assert.AreEqual(expected, result, "MultiLangHeader1 test failed");

            classname = "dTextVariablenLast1";
            output = Common.PathCombine(projInfo.TempOutputFolder, "designmap.xml");
            xPath = "//TextVariable[@Self = \"" + classname + "\"]";
            node = Common.GetXmlNodeInDesignNamespace(output, xPath);
            result = node.InnerXml;
            expected = "<MatchCharacterStylePreference TextBefore=\"\" TextAfter=\"\" AppliedCharacterStyle=\"CharacterStyle/headword_.ii\" SearchStrategy=\"LastOnPage\" ChangeCase=\"None\" DeleteEndPunctuation=\"false\" />";
            Assert.AreEqual(expected, result, "MultiLangHeader1 test failed");

			classname = "dTextVariablenLast2";
            output = Common.PathCombine(projInfo.TempOutputFolder, "designmap.xml");
            xPath = "//TextVariable[@Self = \"" + classname + "\"]";
            node = Common.GetXmlNodeInDesignNamespace(output, xPath);
            result = node.InnerXml;
            expected = "<MatchCharacterStylePreference TextBefore=\"\" TextAfter=\"\" AppliedCharacterStyle=\"CharacterStyle/headwordminor_.ii\" SearchStrategy=\"LastOnPage\" ChangeCase=\"None\" DeleteEndPunctuation=\"false\" />";
            Assert.AreEqual(expected, result, "MultiLangHeader1 test failed");

			classname = "dTextVariablenLast3";
            output = Common.PathCombine(projInfo.TempOutputFolder, "designmap.xml");
            xPath = "//TextVariable[@Self = \"" + classname + "\"]";
            node = Common.GetXmlNodeInDesignNamespace(output, xPath);
            result = node.InnerXml;
            expected = "<MatchCharacterStylePreference TextBefore=\"\" TextAfter=\"\" AppliedCharacterStyle=\"CharacterStyle/headwordL3_.ii-fonipa\" SearchStrategy=\"LastOnPage\" ChangeCase=\"None\" DeleteEndPunctuation=\"false\" />";
            Assert.AreEqual(expected, result, "MultiLangHeader1 test failed");
        }

        [Test]
        public void TextTransform()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/TextTranform.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/TextTranform.xhtml");
            ExportProcess();

            string styleName = "uppercase_1";
            XPath = "//CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]//Content";
            string content = "SAMPLE TEXT WITH UPPERCASE";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + "test Failed");

            styleName = "lowercase_1";
            XPath = "//CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]//Content";
            content = "sample text with lowercase";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            styleName = "Title_1";
            XPath = "//CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/" + styleName + "\"]//Content";
            content = "Sample Text With Capitalize";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        [Test]
        public void Float()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/PictureWidth.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/PictureWidth.xhtml");
            ExportProcess();

            string styleName = "pictureCaption";
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]/Rectangle/AnchoredObjectSetting";

            string _fileNameWithPath = Common.PathCombine(_outputStory, "Story_1.xml");
            XmlNodeList nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, XPath);
            XmlNode node = nodesList[0];
            if (node == null)
                Assert.IsTrue(false);
            XmlAttributeCollection attrb = node.Attributes;

            string result = attrb["AnchorPoint"].Value;
            Assert.AreEqual(result, "TopRightAnchor", "Float Property failed");

            result = attrb["HorizontalAlignment"].Value;
            Assert.AreEqual(result, "RightAlign", "Float Property failed");
        }

        [Test]
        public void WidthAuto()
        {
            XmlNodeList nodesList;
            XmlNode node;
            XmlAttributeCollection attrb;
            string _fileNameWithPath = Common.PathCombine(_outputStory, "Story_2.xml");
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/WidthAuto.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/WidthAuto.xhtml");
            ExportProcess();

            //Case 1:
            XPath = "//Rectangle";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, XPath);
            node = nodesList[0];
            attrb = node.Attributes;
            string result = attrb["ItemTransform"].Value;
            Assert.AreEqual(result, "1 0 0 1 18 -18", "Case1 ItemTransform Property failed");

            XmlNode childNode = node.SelectSingleNode("//Image");
            attrb = childNode.Attributes;

            result = attrb["ActualPpi"].Value;
            Assert.AreEqual(result, "36 36", "Case1 ActualPpi Property failed");

            result = attrb["EffectivePpi"].Value;
            Assert.AreEqual(result, "72 72", "Case1 EffectivePpi Property failed");

            result = attrb["ItemTransform"].Value;
            Assert.AreEqual(result, "0.5 0 0 0.5 -18 -18", "Case1 ItemTransform Property failed");
            childNode.RemoveAll();

            //Case 2:
            node = nodesList[1];
            attrb = node.Attributes;
            result = attrb["ItemTransform"].Value;
            Assert.AreEqual(result, "1 0 0 1 18 -18", "Case2 ItemTransform Property failed");

            childNode = node.SelectSingleNode("Image");
            attrb = childNode.Attributes;

            result = attrb["ActualPpi"].Value;
            Assert.AreEqual(result, "36 36", "Case2 ActualPpi Property failed");

            result = attrb["EffectivePpi"].Value;
            Assert.AreEqual(result, "72 72", "Case2 EffectivePpi Property failed");

            result = attrb["ItemTransform"].Value;
            Assert.AreEqual(result, "0.5 0 0 0.5 -18 -18", "Case2 ItemTransform Property failed");
            childNode.RemoveAll();

            //Case 3:
            node = nodesList[2];
            attrb = node.Attributes;
            result = attrb["ItemTransform"].Value;
            Assert.AreEqual(result, "1 0 0 1 18 -18", "Case3 ItemTransform Property failed");

            childNode = node.SelectSingleNode("Image");
            attrb = childNode.Attributes;

            result = attrb["ActualPpi"].Value;
            Assert.AreEqual(result, "36 36", "Case3 ActualPpi Property failed");

            result = attrb["EffectivePpi"].Value;
            Assert.AreEqual(result, "72 72", "Case3 EffectivePpi Property failed");

            result = attrb["ItemTransform"].Value;
            Assert.AreEqual(result, "0.5 0 0 0.5 -18 -18", "Case3 ItemTransform Property failed");
            childNode.RemoveAll();
        }

        [Test]
        public void ImageSourceAttrib()
        {
            //img[src='Thomsons-gazelle1.jpg']
            XmlNodeList nodesList;
            XmlNode node;
            XmlAttributeCollection attrb;
            string _fileNameWithPath = Common.PathCombine(_outputStory, "Story_1.xml");
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/ImageSourceAttrib.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/ImageSourceAttrib.xhtml");
            ExportProcess();

            //Case 1:
            XPath = "//Rectangle";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, XPath);
            node = nodesList[0];
            attrb = node.Attributes;
            string result = attrb["ItemTransform"].Value;
            Assert.AreEqual(result, "1 0 0 1 18 -18", "Case1 ItemTransform Property failed");

            XmlNode childNode = node.SelectSingleNode("//Image");
            attrb = childNode.Attributes;

            result = attrb["ActualPpi"].Value;
            Assert.AreEqual(result, "36 36", "Case1 ActualPpi Property failed");

            result = attrb["EffectivePpi"].Value;
            Assert.AreEqual(result, "72 72", "Case1 EffectivePpi Property failed");

            result = attrb["ItemTransform"].Value;
            Assert.AreEqual(result, "0.5 0 0 0.5 -18 -18", "Case1 ItemTransform Property failed");
            childNode.RemoveAll();

            //Case 2:
            node = nodesList[1];
            attrb = node.Attributes;
            result = attrb["ItemTransform"].Value;
            Assert.AreEqual(result, "1 0 0 1 18 -18", "Case2 ItemTransform Property failed");

            childNode = node.SelectSingleNode("Image");
            attrb = childNode.Attributes;

            result = attrb["ActualPpi"].Value;
            Assert.AreEqual(result, "36 36", "Case2 ActualPpi Property failed");

            result = attrb["EffectivePpi"].Value;
            Assert.AreEqual(result, "72 72", "Case2 EffectivePpi Property failed");

            result = attrb["ItemTransform"].Value;
			Assert.AreEqual(result, "0.5 0 0 0.5 -18 -18", "Case2 ItemTransform Property failed");

        }

        [Test]
        public void ImageCaption()
        {
            XmlNodeList nodesList;
            XmlNode node;
            string content;
            string _fileNameWithPath = Common.PathCombine(_outputStory, "Story_2.xml");
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/ImageCaption.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/ImageCaption.xhtml");
            ExportProcess();

            //Case 1:
            XPath = "//ParagraphStyleRange/Rectangle/TextFrame/ParagraphStyleRange/CharacterStyleRange/Content";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, XPath);
            node = nodesList[0];
            content = node.InnerText;
            string expected = "yesupatham pattabiram chennai tamilnadu india";
            StringAssert.IsMatch(expected,content,"ImageCaption Test is failed");
        }

        [Test]
        public void ImageNoCaption()
        {
            XmlNodeList nodesList;
            XmlNode node;
            string content;
            string _fileNameWithPath = Common.PathCombine(_outputStory, "Story_2.xml");
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/ImageNoCaption.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/ImageNoCaption.xhtml");
            ExportProcess();

            //Case 1:
            XPath = "//Rectangle/TextFrame";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, XPath);
            node = nodesList[0];
            content = node.InnerText;
            string expected = string.Empty; // No Caption
            StringAssert.IsMatch(expected, content, "ImageNoCaption Test is failed");

            //Case 2:
            XPath = "//Rectangle";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, XPath);
            node = nodesList[0];
            XmlAttributeCollection attrb = node.Attributes;
            string result = attrb["ItemTransform"].Value;
            Assert.AreEqual(result, "1 0 0 1 18 -18", "Case2 ItemTransform Property failed");

            XmlNode childNode = node.SelectSingleNode("//Image");
            attrb = childNode.Attributes;

            result = attrb["ActualPpi"].Value;
            Assert.AreEqual(result, "36 36", "Case1 ActualPpi Property failed");

            result = attrb["EffectivePpi"].Value;
            Assert.AreEqual(result, "72 72", "Case1 EffectivePpi Property failed");

            result = attrb["ItemTransform"].Value;
            Assert.AreEqual(result, "0.5 0 0 0.5 -18 -18", "Case1 ItemTransform Property failed");
            childNode.RemoveAll();
        }

        [Test]
        public void FloatPrinceColumnTop()
        {
            XmlNodeList nodesList;
            XmlNode node;
            XmlAttributeCollection attrb;
            string _fileNameWithPath = Common.PathCombine(_outputStory, "Story_2.xml");
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/FloatColumnTop.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/FloatColumnTop.xhtml");
            ExportProcess();

            XPath = "//AnchoredObjectSetting";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, XPath);
            node = nodesList[0];
            attrb = node.Attributes;
            string result = attrb["AnchorPoint"].Value;
            Assert.AreEqual(result, "TopCenterAnchor", "Case1 ItemTransform Property failed");

            result = attrb["HorizontalAlignment"].Value;
            Assert.AreEqual(result, "CenterAlign", "Case1 ItemTransform Property failed");

            result = attrb["HorizontalReferencePoint"].Value;
            Assert.AreEqual(result, "ColumnEdge", "Case1 ItemTransform Property failed");

            result = attrb["VerticalAlignment"].Value;
            Assert.AreEqual(result, "CenterAlign", "Case1 ItemTransform Property failed");

            result = attrb["VerticalReferencePoint"].Value;
            Assert.AreEqual(result, "LineBaseline", "Case1 ItemTransform Property failed");
        }

        [Test]
        public void FloatPsColumnBottom()
        {
            XmlNodeList nodesList;
            XmlNode node;
            XmlAttributeCollection attrb;
            string _fileNameWithPath = Common.PathCombine(_outputStory, "Story_2.xml");
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/FloatPsColumnBottom.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/FloatPsColumnBottom.xhtml");
            ExportProcess();

            XPath = "//AnchoredObjectSetting";
            nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, XPath);
            node = nodesList[0];
            attrb = node.Attributes;
            string result = attrb["AnchorPoint"].Value;
            Assert.AreEqual(result, "TopCenterAnchor", "Case1 ItemTransform Property failed");

            result = attrb["HorizontalAlignment"].Value;
            Assert.AreEqual(result, "CenterAlign", "Case1 ItemTransform Property failed");

            result = attrb["HorizontalReferencePoint"].Value;
            Assert.AreEqual(result, "ColumnEdge", "Case1 ItemTransform Property failed");

            result = attrb["VerticalAlignment"].Value;
            Assert.AreEqual(result, "CenterAlign", "Case1 ItemTransform Property failed");

            result = attrb["VerticalReferencePoint"].Value;
            Assert.AreEqual(result, "LineBaseline", "Case1 ItemTransform Property failed");
        }
        [Test]
        public void DropCaps()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/DropCaps.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/DropCaps.xhtml");
            ExportProcess();

            _expected.Add("DropCapCharacters", "1");
            _expected.Add("DropCapLines", "3");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"ChapterNumber1\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }

        [Test]
        // TD-1967 - TD-1968 - DropCaps with No of Digits Test
        public void DropCaps_Digits()
        {
            _storyXML = new InStory();
            string fileName = "dropcaps_digits";
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/" + fileName + ".css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/" + fileName + ".xhtml");
            ExportProcess();

            _expected.Add("DropCapCharacters", "1");
            _expected.Add("DropCapLines", "2");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"ChapterNumber1\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");

            _expected.Add("DropCapCharacters", "2");
            _expected.Add("DropCapLines", "2");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"ChapterNumber2\"]";
            result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");

            _expected.Add("DropCapCharacters", "3");
            _expected.Add("DropCapLines", "2");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"ChapterNumber3\"]";
            result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");

        }
        [Test]
        public void FootNote()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Footnote.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/Footnote.xhtml");
            ExportProcess();

            _expected.Add("Position", "Superscript");
            XPath = "//ParagraphStyleRange[32]/CharacterStyleRange";
            bool result = StoryXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");

            _expected.Add("Content", " 11-1 = ");
            XPath = "//ParagraphStyleRange/CharacterStyleRange/Footnote/ParagraphStyleRange/CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/$ID/[No character style]\"]";
            result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, _inputCSS + " test Failed");

            _expected.Add("Content", "Israel:");
            XPath = "//ParagraphStyleRange/CharacterStyleRange/Footnote/ParagraphStyleRange/CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/Emphasis_1\"]";
            result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, _inputCSS + " test Failed");

            _expected.Add("Content", " Reghia kori Diksonari.");
			XPath = "//ParagraphStyleRange/CharacterStyleRange/Footnote/ParagraphStyleRange/CharacterStyleRange[@AppliedCharacterStyle = \"CharacterStyle/NoteGeneralParagraph_1\"]";
            result = StoryXmlNodeTest(false);
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }

        /// <summary>
        /// h1 to h6
        /// </summary>
        [Test]
        public void TagTest()
        {
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/tagTest.xhtml");

            _storyXML = new InStory();
            _inputCSS= Common.DirectoryPathReplace(_testFolderPath + "/input/tag_Case1.css");
            ExportProcess();
			string styleName = "h1_.en_1";
            XPath = "//ParagraphStyleRange[2][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            string content = "h1 lang";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/tag_Case2.css");
            ExportProcess();
            styleName = "h1_1";
            XPath = "//ParagraphStyleRange[1][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "h1 only ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/tag_Case3.css");
            ExportProcess();
            styleName = "h1_1";
            XPath = "//ParagraphStyleRange[1][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "h1 only ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");

            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/tag_Case4.css");
            ExportProcess();
			styleName = "h1_.en_1";
            XPath = "//ParagraphStyleRange[2][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "h1 lang";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");


            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/tag_Case5.css");
            ExportProcess();
            styleName = "a_1";
            XPath = "//ParagraphStyleRange[3][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = " h1 class ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");


            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/tag_Case6.css");
            ExportProcess();
			styleName = "a_.en_1";
            XPath = "//ParagraphStyleRange[4][@AppliedParagraphStyle = \"ParagraphStyle/" + styleName + "\"]//Content";
            content = "h1 class lang ";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, styleName + " test Failed");
        }

        [Test]
        public void WordLetterSpacing()
        {
            string classname = "entry1_1";

            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/WordLetterSpacing.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/WordLetterSpacing.xhtml");
            ExportProcess();

            _expected.Add("MinimumLetterSpacing", "0");
            _expected.Add("DesiredLetterSpacing", "495");
            _expected.Add("MaximumLetterSpacing", "495");

            _expected.Add("MinimumWordSpacing", "0");
            _expected.Add("DesiredWordSpacing", "495");
            _expected.Add("MaximumWordSpacing", "495");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + classname + "\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");

            classname = "entry2_1";
            _expected.Clear();
            _expected.Add("MinimumLetterSpacing", "0");
            _expected.Add("DesiredLetterSpacing", "240");
            _expected.Add("MaximumLetterSpacing", "240");

            _expected.Add("MinimumWordSpacing", "0");
            _expected.Add("DesiredWordSpacing", "240");
            _expected.Add("MaximumWordSpacing", "240");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + classname + "\"]";
            result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }

        [Test]
        public void LanguageTest()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/Language.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/Language.xhtml");
            ExportProcess();
			string classname = "partofspeech_.en_1";
            _expected.Clear();
            _expected.Add("AppliedLanguage", "$ID/English: USA");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + classname + "\"]";
            bool result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");

			classname = "partofPOR_1";
            _expected.Clear();
            _expected.Add("AppliedLanguage", "$ID/Portuguese");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + classname + "\"]";
            result = StyleXmlNodeTest(true);
            Assert.IsTrue(result, _inputCSS + " test Failed");
        }

        [Test]
        public void CrossRef()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/CrossRef.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/CrossRef.xhtml");
            _cssProperty.Clear();
            _idAllClass.Clear();
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
            var StyleName = _storyXML.CreateStory(projInfo, _idAllClass, _cssTree.SpecificityClass, _cssTree.CssClassOrder);

            string classname = "Hyperlink nema1";
            XPath = "//HyperlinkTextSource[@Name = \"" + classname + "\"]";
            string content = "nema1";
            bool result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, classname + " test Failed");

            classname = "Hyperlink nema2";
            XPath = "//HyperlinkTextSource[@Name = \"" + classname + "\"]";
            content = "nema2";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, classname + " test Failed");

            classname = "Hyperlink nema3";
            XPath = "//HyperlinkTextSource[@Name = \"" + classname + "\"]";
            content = "nema3";
            result = ValidateNodeContent(_outputStory, content);
            Assert.IsTrue(result, classname + " test Failed");

            classname = "Name_namenema1";
            XPath = "//HyperlinkTextDestination[@Name = \"" + classname + "\"]";
            content = "nema1";
            result = ValidateNextNodeContent(_outputStory, content);
            Assert.IsTrue(result, classname + " test Failed");

            classname = "Name_namenema2";
            XPath = "//HyperlinkTextDestination[@Name = \"" + classname + "\"]";
            content = "nema2";
            result = ValidateNextNodeContent(_outputStory, content);
            Assert.IsTrue(result, classname + " test Failed");

            classname = "Name_namenema3";
            XPath = "//HyperlinkTextDestination[@Name = \"" + classname + "\"]";
            content = "nema3";
            result = ValidateNextNodeContent(_outputStory, content);
            Assert.IsTrue(result, classname + " test Failed");

            ArrayList test = new ArrayList();
            InDesignMap _designmapXML = new InDesignMap();
            _designmapXML.CreateIDDesignMap(_outputPath, 4, new ArrayList(), test, StyleName["CrossRef"], string.Empty);
            classname = "Name_namenema3";

            FileNameWithPath = Common.PathCombine(_outputPath,"designmap.xml");
            foreach (string crossref in StyleName["CrossRef"])
            {
                XPath = "//Hyperlink[@Name = \"" + crossref + "\"]";
                result = IsNodeExists();
                Assert.IsTrue(result, classname + " test Failed");
            }
        }

        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void TokPisin()
        {
            //Scripture
            string fileName = "TokPisin";
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/" + fileName + ".xhtml");
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/" + fileName + ".css");
            FileComparisionTest("TokPisinExpect", "1");
        }

        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void TeTest()
        {
            //Scripture
            string fileName = "TeTest";
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/" + fileName + ".xhtml");
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/" + fileName + ".css");
            FileComparisionTest("TeTestExpect", "1");
        }

        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void Bughotugospels()
        {
            //Scripture
            string fileName = "Bughotu-gospels";
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/" + fileName + ".xhtml");
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/" + fileName + ".css");
            FileComparisionTest("Bughotu-gospelsExpect", "2");
        }

        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void B1pe()
        {
            //Scripture
            string fileName = "B1pe";
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/" + fileName + ".xhtml");
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/" + fileName + ".css");
            FileComparisionTest("B1peExpect", "2");
        }

        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void Kabwa()
        {
            //Scripture
            string fileName = "Kabwa";
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/" + fileName + ".xhtml");
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/" + fileName + ".css");
            FileComparisionTest("KabwaExpect", "2");
        }

        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void BuangExportDictionary()
        {
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/BuangExport.xhtml");
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/BuangExport.css");
            FileComparisionTest("BuangExpect", "2");
        }

        public void FileComparisionTest(string fileName, string fileNo)
        {
            PublicationInformation projInfo = new PublicationInformation();

            projInfo.ProjectPath = Path.GetDirectoryName(_inputXHTML);
            projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
            projInfo.DefaultCssFileWithPath = _inputCSS;
            projInfo.ProjectInputType = "Scripture";
            PreExportProcess preProcessor = new PreExportProcess(projInfo);
            preProcessor.GetTempFolderPath();
            preProcessor.ImagePreprocess(false);
            preProcessor.InsertHiddenChapterNumber();
            preProcessor.InsertHiddenVerseNumber();
            projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;
            projInfo.DefaultCssFileWithPath = preProcessor.ProcessedCss;

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
            preProcessor.InsertEmptyXHomographNumber(cssClass);

            Dictionary<string, Dictionary<string, string>> idAllClass = new Dictionary<string, Dictionary<string, string>>();
            InStyles inStyles = new InStyles();
            projInfo.TempOutputFolder = _outputPath;
            idAllClass = inStyles.CreateIDStyles(Common.PathCombine(_outputPath, "Resources"), cssClass);

            InGraphic inGraphic = new InGraphic();
            inGraphic.CreateIDGraphic(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), cssClass, cssTree.cssBorderColor);

            InStory inStory = new InStory();
            Dictionary<string, ArrayList> StyleName = inStory.CreateStory(projInfo, idAllClass, cssTree.SpecificityClass, cssTree.CssClassOrder);

            InMasterSpread inMasterSpread = new InMasterSpread();
            ArrayList masterPageNames = inMasterSpread.CreateIDMasterSpread(Common.PathCombine(projInfo.TempOutputFolder, "MasterSpreads"), idAllClass, headwordStyles);

            InSpread inSpread = new InSpread();
            inSpread.CreateIDSpread(Common.PathCombine(projInfo.TempOutputFolder, "Spreads"), idAllClass, StyleName["ColumnClass"]);

            InDesignMap inDesignMap = new InDesignMap();
            inDesignMap.CreateIDDesignMap(projInfo.TempOutputFolder, StyleName["ColumnClass"].Count, masterPageNames, StyleName["TextVariables"], StyleName["CrossRef"], string.Empty);

            InPreferences inPreferences = new InPreferences();
            inPreferences.CreateIDPreferences(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), idAllClass);

            // Compare files
            string expectedFolder = Common.PathCombine(_testFolderPath, "Expected\\" + fileName);

            string output = Common.PathCombine(projInfo.TempOutputFolder, "Stories\\Story_" + fileNo + ".xml");
            string expected = Common.PathCombine(expectedFolder, "Stories\\Story_" + fileNo + ".xml");
            XmlAssert.AreEqual(output, expected, " Story_"+ fileNo + ".xml is not matching");

            output = Common.PathCombine(projInfo.TempOutputFolder, "Resources\\Styles.xml");
            expected = Common.PathCombine(expectedFolder, "Resources\\Styles.xml");
            XmlAssert.AreEqual(output, expected, " styles.xml is not matching");
            output = Common.PathCombine(projInfo.TempOutputFolder, "Resources\\Graphic.xml");
            expected = Common.PathCombine(expectedFolder, "Resources\\Graphic.xml");
            XmlAssert.AreEqual(output, expected, " Graphic.xml is not matching");

            output = Common.PathCombine(projInfo.TempOutputFolder, "Resources\\Preferences.xml");
            expected = Common.PathCombine(expectedFolder, "Resources\\Preferences.xml");
            XmlAssert.AreEqual(output, expected, " Preferences.xml is not matching");

            output = Common.PathCombine(projInfo.TempOutputFolder, "Spreads\\Spread_1.xml");
            expected = Common.PathCombine(expectedFolder, "Spreads\\Spread_1.xml");
            XmlAssert.AreEqual(output, expected, " Spread_1.xml is not matching");

            output = Common.PathCombine(projInfo.TempOutputFolder, "Spreads\\Spread_2.xml");
            expected = Common.PathCombine(expectedFolder, "Spreads\\Spread_2.xml");
            XmlAssert.AreEqual(output, expected, " Spread_2.xml is not matching");

            output = Common.PathCombine(projInfo.TempOutputFolder, "Spreads\\Spread_3.xml");
            expected = Common.PathCombine(expectedFolder, "Spreads\\Spread_3.xml");
            XmlAssert.AreEqual(output, expected, " Spread_3.xml is not matching");

            output = Common.PathCombine(projInfo.TempOutputFolder, "MasterSpreads\\MasterSpread_All.xml");
            expected = Common.PathCombine(expectedFolder, "MasterSpreads\\MasterSpread_All.xml");
            XmlAssert.AreEqual(output, expected, " MasterSpread_All.xml is not matching");

            output = Common.PathCombine(projInfo.TempOutputFolder, "MasterSpreads\\MasterSpread_First.xml");
            expected = Common.PathCombine(expectedFolder, "MasterSpreads\\MasterSpread_First.xml");
            XmlAssert.AreEqual(output, expected, " MasterSpread_First.xml is not matching");
        }

        [Test]
        public void MultiLangHeader2()
        {
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/MultiLangHeader2.xhtml");
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/MultiLangHeader2.css");

            PublicationInformation projInfo = new PublicationInformation();

            projInfo.ProjectPath = Path.GetDirectoryName(_inputXHTML);
            projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
            projInfo.DefaultCssFileWithPath = _inputCSS;

            PreExportProcess preProcessor = new PreExportProcess(projInfo);
            preProcessor.GetTempFolderPath();
            preProcessor.ImagePreprocess(false);
            preProcessor.ReplaceInvalidTagtoSpan("_AllComplexFormEntryBackRefs|LexEntryRef_PrimaryLexemes", "span");
            preProcessor.InsertHiddenChapterNumber();
            preProcessor.InsertHiddenVerseNumber();
            preProcessor.GetDefinitionLanguage();

            projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;
            projInfo.DefaultCssFileWithPath = preProcessor.ProcessedCss;

            Dictionary<string, Dictionary<string, string>> cssClass =
                new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
            preProcessor.InsertEmptyXHomographNumber(cssClass);

            Dictionary<string, Dictionary<string, string>> idAllClass =
                new Dictionary<string, Dictionary<string, string>>();
            InStyles inStyles = new InStyles();
            projInfo.TempOutputFolder = _outputPath;
            idAllClass = inStyles.CreateIDStyles(Common.PathCombine(_outputPath, "Resources"), cssClass);

            InGraphic inGraphic = new InGraphic();
            inGraphic.CreateIDGraphic(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), cssClass,
                                      cssTree.cssBorderColor);

            InStory inStory = new InStory();
            Dictionary<string, ArrayList> StyleName =
                inStory.CreateStory(projInfo, idAllClass, cssTree.SpecificityClass, cssTree.CssClassOrder);

            InMasterSpread inMasterSpread = new InMasterSpread();
            ArrayList masterPageNames =
                inMasterSpread.CreateIDMasterSpread(Common.PathCombine(projInfo.TempOutputFolder, "MasterSpreads"),
                                                    idAllClass, StyleName["TextVariables"]);

            InSpread inSpread = new InSpread();
            inSpread.CreateIDSpread(Common.PathCombine(projInfo.TempOutputFolder, "Spreads"), idAllClass,
                                    StyleName["ColumnClass"]);

            InDesignMap inDesignMap = new InDesignMap();
            inDesignMap.CreateIDDesignMap(projInfo.TempOutputFolder, StyleName["ColumnClass"].Count, masterPageNames,
                                          StyleName["TextVariables"], StyleName["CrossRef"], string.Empty);

            InPreferences inPreferences = new InPreferences();
            inPreferences.CreateIDPreferences(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), idAllClass);

            string classname = "dTextVariablenFirst1";
            string output = Common.PathCombine(projInfo.TempOutputFolder, "designmap.xml");
            string xPath = "//TextVariable[@Self = \"" + classname + "\"]";
            XmlNode node = Common.GetXmlNodeInDesignNamespace(output, xPath);
            string result = node.InnerXml;
			string expected = "<MatchCharacterStylePreference TextBefore=\"\" TextAfter=\"\" AppliedCharacterStyle=\"CharacterStyle/headword_.seh\" SearchStrategy=\"FirstOnPage\" ChangeCase=\"None\" DeleteEndPunctuation=\"false\" />";
            Assert.AreEqual(expected, result, "MultiLangHeader1 test failed");

            classname = "dTextVariablenLast1";
            output = Common.PathCombine(projInfo.TempOutputFolder, "designmap.xml");
            xPath = "//TextVariable[@Self = \"" + classname + "\"]";
            node = Common.GetXmlNodeInDesignNamespace(output, xPath);
            result = node.InnerXml;
			expected = "<MatchCharacterStylePreference TextBefore=\"\" TextAfter=\"\" AppliedCharacterStyle=\"CharacterStyle/headword_.seh\" SearchStrategy=\"LastOnPage\" ChangeCase=\"None\" DeleteEndPunctuation=\"false\" />";
            Assert.AreEqual(expected, result, "MultiLangHeader1 test failed");
        }

        [Test]
        public void MultiLangHeader3()
        {
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/MultiLangHeader3.xhtml");
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/MultiLangHeader3.css");

            PublicationInformation projInfo = new PublicationInformation();

            projInfo.ProjectPath = Path.GetDirectoryName(_inputXHTML);
            projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
            projInfo.DefaultCssFileWithPath = _inputCSS;

            PreExportProcess preProcessor = new PreExportProcess(projInfo);
            preProcessor.GetTempFolderPath();
            preProcessor.ImagePreprocess(false);
            preProcessor.ReplaceInvalidTagtoSpan("_AllComplexFormEntryBackRefs|LexEntryRef_PrimaryLexemes", "span");
            preProcessor.InsertHiddenChapterNumber();
            preProcessor.InsertHiddenVerseNumber();
            preProcessor.GetDefinitionLanguage();

            projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;
            projInfo.DefaultCssFileWithPath = preProcessor.ProcessedCss;

            Dictionary<string, Dictionary<string, string>> cssClass =
                new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
            preProcessor.InsertEmptyXHomographNumber(cssClass);

            Dictionary<string, Dictionary<string, string>> idAllClass =
                new Dictionary<string, Dictionary<string, string>>();
            InStyles inStyles = new InStyles();
            projInfo.TempOutputFolder = _outputPath;
            idAllClass = inStyles.CreateIDStyles(Common.PathCombine(_outputPath, "Resources"), cssClass);

            InGraphic inGraphic = new InGraphic();
            inGraphic.CreateIDGraphic(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), cssClass,
                                      cssTree.cssBorderColor);

            InStory inStory = new InStory();
            Dictionary<string, ArrayList> StyleName =
                inStory.CreateStory(projInfo, idAllClass, cssTree.SpecificityClass, cssTree.CssClassOrder);

            InMasterSpread inMasterSpread = new InMasterSpread();
            ArrayList masterPageNames =
                inMasterSpread.CreateIDMasterSpread(Common.PathCombine(projInfo.TempOutputFolder, "MasterSpreads"),
                                                    idAllClass, StyleName["TextVariables"]);

            InSpread inSpread = new InSpread();
            inSpread.CreateIDSpread(Common.PathCombine(projInfo.TempOutputFolder, "Spreads"), idAllClass,
                                    StyleName["ColumnClass"]);

            InDesignMap inDesignMap = new InDesignMap();
            inDesignMap.CreateIDDesignMap(projInfo.TempOutputFolder, StyleName["ColumnClass"].Count, masterPageNames,
                                          StyleName["TextVariables"], StyleName["CrossRef"], string.Empty);

            InPreferences inPreferences = new InPreferences();
            inPreferences.CreateIDPreferences(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), idAllClass);

            string classname = "dTextVariablenFirst1";
            string output = Common.PathCombine(projInfo.TempOutputFolder, "designmap.xml");
            string xPath = "//TextVariable[@Self = \"" + classname + "\"]";
            XmlNode node = Common.GetXmlNodeInDesignNamespace(output, xPath);
            string result = node.InnerXml;
			string expected = "<MatchCharacterStylePreference TextBefore=\"\" TextAfter=\"\" AppliedCharacterStyle=\"CharacterStyle/headword_.bzh\" SearchStrategy=\"FirstOnPage\" ChangeCase=\"None\" DeleteEndPunctuation=\"false\" />";
            Assert.AreEqual(expected, result, "MultiLangHeader1 test failed");

            classname = "dTextVariablenLast1";
            output = Common.PathCombine(projInfo.TempOutputFolder, "designmap.xml");
            xPath = "//TextVariable[@Self = \"" + classname + "\"]";
            node = Common.GetXmlNodeInDesignNamespace(output, xPath);
            result = node.InnerXml;
			expected = "<MatchCharacterStylePreference TextBefore=\"\" TextAfter=\"\" AppliedCharacterStyle=\"CharacterStyle/headword_.bzh\" SearchStrategy=\"LastOnPage\" ChangeCase=\"None\" DeleteEndPunctuation=\"false\" />";
            Assert.AreEqual(expected, result, "MultiLangHeader1 test failed");
        }

        [Test]
        public void EmptyTextNode()
        {
            const string classname = "EmptyTextNode";
            _storyXML = new InStory();
            _cssProperty.Clear();
            _idAllClass.Clear();
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/EmptyTextNode.xhtml");
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/EmptyTextNode.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
            _storyXML.CreateStory(projInfo, _idAllClass, _cssTree.SpecificityClass, _cssTree.CssClassOrder);
            XPath = "//ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/letter_1\"]/CharacterStyleRange";
            string _fileNameWithPath = Common.PathCombine(_outputStory, "Story_4.xml");
            XmlNodeList nodesList = Common.GetXmlNodeListInDesignNamespace(_fileNameWithPath, XPath);
            XmlNode node = nodesList[0];
            XmlAttributeCollection attrb = node.Attributes;

            string result = attrb["AppliedCharacterStyle"].Value;
            Assert.AreEqual(result, "CharacterStyle/$ID/[No character style]", classname + " test Failed");
        }

        [Test]
        // compare output with firefox
        public void TaggedText()
        {
            _storyXML = new InStory();
            _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/TaggedText.css");
            _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/TaggedText.xhtml");
            ExportProcess();

			XPath = "//Story/ParagraphStyleRange[@AppliedParagraphStyle = \"ParagraphStyle/div_1\"]";

            _cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            _storyXML.CreateStory(projInfo, _idAllClass, _cssTree.SpecificityClass, _cssTree.CssClassOrder);

            XmlNodeList nodesList = Common.GetXmlNodeListInDesignNamespace(Common.PathCombine(_outputStory, "Story_2.xml"), XPath);
            XmlNode node = nodesList[0];
            Assert.IsTrue(node != null, _inputCSS + " test Failed");
        }



        #region private Methods
        private bool StyleXmlNodeTest(bool checkAttribute)
        {
            //ExportProcess();

            FileNameWithPath = Common.PathCombine(_outputStyles, "Styles.xml");
            bool result;
            if (checkAttribute)
            {
                result = ValidateNodeAttribute();
            }
            else
            {
                result = ValidateNodeValue();
            }
            return result;
        }

        private bool StoryXmlNodeTest(bool checkAttribute)
        {
            //ExportProcess();

            FileNameWithPath = Common.PathCombine(_outputStory, "Story_1.xml");
            bool result;
            if (checkAttribute)
            {
                result = ValidateNodeAttribute();
            }
            else
            {
                result = ValidateNodeValue();
            }
            return result;
        }

        private bool StoryXmlNodeTestForTag(bool checkAttribute)
        {
            //ExportProcess();

            FileNameWithPath = Common.PathCombine(_outputStory, "Story_2.xml");
            bool result;
            if (checkAttribute)
            {
                result = ValidateNodeAttribute();
            }
            else
            {
                result = ValidateNodeValue();
            }
            return result;
        }

        private void ExportProcess()
        {
            _cssProperty.Clear();
            _idAllClass.Clear();
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
            _storyXML.CreateStory(projInfo, _idAllClass, _cssTree.SpecificityClass, _cssTree.CssClassOrder);
        }

        #endregion

        #endregion Setup
    }
}
