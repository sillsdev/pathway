// --------------------------------------------------------------------------------------------
// <copyright file="InStylesTest.cs" from='2009' to='2014' company='SIL International'>
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
// Test Cases for InDesign StylesTest
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
    public class InStylesTest : ValidateXMLFile
    {
        #region Private Variables
        private string _input;
        private string _output;
        private Dictionary<string, string> _expected = new Dictionary<string, string>();
        private string _className = "a";
        private string _testFolderPath = string.Empty;
        private InStyles _stylesXML;
        private InStory _storyXML;
        private string _outputPath;
        private string _outputStyles;
        private Dictionary<string, Dictionary<string, string>> _idAllClass = new Dictionary<string, Dictionary<string, string>>();
        PublicationInformation projInfo = new PublicationInformation();
        #endregion

        #region Public Variables
        private Dictionary<string, Dictionary<string, string>> _cssProperty;
        private CssTree _cssTree;
        #endregion

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _stylesXML = new InStyles();
            _storyXML = new InStory();
            _testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/InDesignConvert/TestFiles");
            ClassProperty = _expected;  //Note: All Reference address initialized here
            _output = Common.PathCombine(_testFolderPath, "output");
            _outputPath = Common.PathCombine(_testFolderPath, "output");
            _outputStyles = Common.PathCombine(_outputPath, "Resources");
            projInfo.TempOutputFolder = _outputPath;
            _cssProperty = new Dictionary<string, Dictionary<string, string>>();
            _cssTree = new CssTree();
        }
        #endregion Setup

        #region Public Functions

        private void NodeTest(bool checkAttribute)
        {
            _cssProperty = _cssTree.CreateCssProperty(_input, true);
            _stylesXML.CreateIDStyles(_output, _cssProperty);
            FileNameWithPath = Common.PathCombine(_output, "Styles.xml");
            if (checkAttribute)
            {
                Assert.IsTrue(ValidateNodeAttribute(), _input + " test Failed");
            }
            else
            {
                Assert.IsTrue(ValidateNodeValue(), _input + " test Failed");
            }
        }

        #region FontSize
        [Test]
        public void FontSize1()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontSize1.css");
            _expected.Add("PointSize", "24");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }


        [Test]
        public void FontSize2()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontSize2.css");
            _expected.Add("PointSize", "56.69291");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        [Test]
        public void FontSize3()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontSize3.css");
            _expected.Add("PointSize", "6.6");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        #endregion

        #region FontFamily
        [Test]
        public void FontFamily1()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontFamily1.css");
            _expected.Add("AppliedFont", "Times New Roman");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]/Properties/AppliedFont";
            NodeTest(false);
        }

        [Test]
        public void FontFamily2()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontFamily2.css");
            _expected.Add("AppliedFont", "Gentium");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]/Properties/AppliedFont";
            NodeTest(false);
        }

	//This test is environment dependent and fails if different fonts are installed.
   //     [Test]
   //     [Category("ShortTest")]
   //     [Category("SkipOnTeamCity")]
   //     public void FontFamily3()
   //     {
   //         _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontFamily3.css");
			//if(Common.UsingMonoVM)
			//	_expected.Add("AppliedFont", "sans-serif");
			//else
			//	_expected.Add("AppliedFont", "Arial Unicode MS");

   //         XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]/Properties/AppliedFont";
   //         NodeTest(false);
   //     }

        #endregion FontFamily

        #region FontWeight
        [Test]
        public void FontWeight1()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontWeight1.css");
            _expected.Add("FontStyle", "Bold");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        [Test]
        public void FontWeight2()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontWeight2.css");
            _expected.Add("FontStyle", "Italic");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        [Test]
        public void FontWeight3()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontWeight3.css");
            _expected.Add("FontStyle", "Bold Italic");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        [Test]
        public void FontWeight4()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontWeight4.css");
            _expected.Add("FontStyle", "Regular");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        [Test]
        public void FontWeight5()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontWeight5.css");
            _expected.Add("FontStyle", "Regular");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        [Test]
        public void FontWeight6()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontWeight6.css");
            _expected.Add("FontStyle", "Regular");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        [Test]
        public void FontWeight7()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontWeight7.css");
            _expected.Add("FontStyle", "Bold");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        [Test]
        public void FontWeight8()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontWeight8.css");
            _expected.Add("FontStyle", "Regular");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        [Test]
        public void FontWeight9()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontWeight9.css");
            _expected.Add("FontStyle", "Bold");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }
        #endregion

        #region TextAlign
        [Test]
        public void TextAlign1()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/TextAlign1.css");
            _expected.Add("Justification", "CenterAlign");
            _className = "a";
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        [Test]
        public void TextAlign2()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/TextAlign2.css");
            _expected.Add("Justification", "LeftJustified");
            _className = "a";
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        [Test]
        public void TextAlign3()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/TextAlign3.css");
            _expected.Add("Justification", "RightAlign");
            _className = "a";
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }
        #endregion

        #region FontVariant
        [Test]
        public void FontVariant1()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontVariant1.css");
            _expected.Add("Capitalization", "SmallCaps");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        [Test]
        public void FontVariant2()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/FontVariant2.css");
            _expected.Add("Capitalization", "Normal");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        #endregion

        #region MarginLeft
        [Test]
        public void MarginLeft1()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/MarginLeft1.css");
            _expected.Add("LeftIndent", "-36");
            _className = "a";
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }

        [Test]
        public void MarginLeft2()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Margin.css");
            _expected.Add("LeftIndent", "30");
            _className = "a";
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }
        #endregion

        #region MarginRight
        [Test]
        public void MarginRight()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Margin.css");
            _expected.Add("RightIndent", "40");
            _className = "a";
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }
        #endregion

        #region MarginTop
        [Test]
        public void MarginTop()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Margin.css");
            _expected.Add("SpaceBefore", "50");
            _className = "a";
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }
        #endregion

        #region MarginBottom
        [Test]
        public void MarginBottom()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Margin.css");
            _expected.Add("SpaceAfter", "60");
            _className = "a";
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }
        #endregion

        #region Padding


        [Test]
        public void Padding1()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Padding1.css");
            _cssProperty = _cssTree.CreateCssProperty(_input, true);
            _stylesXML.CreateIDStyles(_output, _cssProperty);
            FileNameWithPath = Common.PathCombine(_output, "Styles.xml");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            _expected.Add("SpaceBefore", "40");
            Assert.IsTrue(ValidateNodeAttribute(), " failed for Padding_Top");

            _expected.Add("RightIndent", "60");
            Assert.IsTrue(ValidateNodeAttribute(), " failed for Padding_Right");

            _expected.Add("SpaceAfter", "36");
            Assert.IsTrue(ValidateNodeAttribute(), " failed for Padding_Bottom");

            _expected.Add("LeftIndent", "50");
            Assert.IsTrue(ValidateNodeAttribute(), " failed for Padding_Left");
        }

        [Test]
        public void Padding2()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Padding2.css");
            _cssProperty = _cssTree.CreateCssProperty(_input, true);
            _stylesXML.CreateIDStyles(_output, _cssProperty);
            FileNameWithPath = Common.PathCombine(_output, "Styles.xml");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            _expected.Add("SpaceBefore", "11");
            Assert.IsTrue(ValidateNodeAttribute(), " failed for Padding_Top");

            _expected.Add("RightIndent", "0");
            Assert.IsTrue(ValidateNodeAttribute(), " failed for Padding_Right");

            _expected.Add("SpaceAfter", "13");
            Assert.IsTrue(ValidateNodeAttribute(), " failed for Padding_Bottom");

            _expected.Add("LeftIndent", "14");
            Assert.IsTrue(ValidateNodeAttribute(), " failed for Padding_Left");
        }
        #endregion

        #region TextIndent
        [Test]
        public void TextIndent()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/TextIndent1.css");
            _expected.Add("FirstLineIndent", "-36");
            _className = "a";
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }
        #endregion

        #region TextDecoration
        [Test]
        public void TextDecoration()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/TextDecoration1.css");
            _expected.Add("Underline", "true");
            _className = "a";
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }
        #endregion

        #region Color
        [Test]
        public void Color()
        {
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Color1.css");
            _expected.Add("FillColor", "Color/#ff0000");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            NodeTest(true);
        }
        #endregion

        #region Tagged Text
        [Test]
        public void TaggedText1()
        {
            string className = "div.header";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/TaggedText.css");
            _expected.Add("PointSize", "24");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + className + "\"]";
            NodeTest(true);
        }
        [Test]
        public void TaggedText2()
        {
            string className = "div";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/TaggedText.css");
            _expected.Add("FillColor", "Color/#ff0000");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + className + "\"]";
            NodeTest(true);
        }
        [Test]
        public void TaggedText3()
        {
            string className = "span.header";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/TaggedText.css");
            _expected.Add("PointSize", "18");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + className + "\"]";
            NodeTest(true);
        }
        [Test]
        public void TaggedText4()
        {
            string className = "span";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/TaggedText.css");
            _expected.Add("FillColor", "Color/#0000ff");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + className + "\"]";
            NodeTest(true);
        }
        #endregion

        #region VerticalAlign
        [Test]
        public void VerticalAlign1()
        {
            const string classname = "baseline";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/VerticalAlign.css");
            _expected.Add("Position", "Normal");
            XPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[3][@Name = \"" + classname + "\"]";
            NodeTest(true);
        }
        [Test]
        public void VerticalAlign2()
        {
            const string classname = "sub";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/VerticalAlign.css");
            _expected.Add("Position", "Subscript");
            XPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[4][@Name = \"" + classname + "\"]";
            NodeTest(true);
        }
        [Test]
        public void VerticalAlign3()
        {
            const string classname = "super";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/VerticalAlign.css");
            _expected.Add("Position", "Superscript");
            XPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[5][@Name = \"" + classname + "\"]";
            NodeTest(true);
        }
        [Test]
        public void VerticalAlign4()
        {
            const string classname = "top";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/VerticalAlign.css");
            _expected.Add("BaselineShift", "50%");
            XPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[7][@Name = \"" + classname + "\"]";
            NodeTest(true);
        }
        [Test]
        public void VerticalAlign5()
        {
            const string classname = "bottom";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/VerticalAlign.css");
            _expected.Add("BaselineShift", "-50%");
            XPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[8][@Name = \"" + classname + "\"]";
            NodeTest(true);
        }
        [Test]
        public void VerticalAlign6()
        {
            const string classname = "percent";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/VerticalAlign.css");
            _expected.Add("BaselineShift", "75%");
            XPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[9][@Name = \"" + classname + "\"]";
            NodeTest(true);
        }
        [Test]
        public void VerticalAlign7()
        {
            const string classname = "percent1";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/VerticalAlign.css");
            _expected.Add("BaselineShift", "25%");
            XPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[10][@Name = \"" + classname + "\"]";
            NodeTest(true);
        }
        [Test]
        public void VerticalAlign8()
        {
            const string classname = "point";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/VerticalAlign.css");
            _expected.Add("BaselineShift", "7");
            XPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[11][@Name = \"" + classname + "\"]";
            NodeTest(true);
        }
        #endregion

        #region BackgroundColor

        [Test]
        public void BackgroundColor1()
        {
            const string classname = "main";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/BackgroundColor.css");
            _expected.Add("StrokeWeight", "1");
            _expected.Add("StrokeColor", "Color/#ff0000");
            _expected.Add("EndJoin", "BevelEndJoin");
            XPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[4][@Name = \"" + classname + "\"]";
            NodeTest(true);
        }
        [Test]
        public void BackgroundColor2()
        {
            const string classname = "letter.-current";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/BackgroundColor.css");
            _expected.Add("StrokeWeight", "1");
            _expected.Add("StrokeColor", "Color/#aaff00");
            _expected.Add("EndJoin", "BevelEndJoin");
            XPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[3][@Name = \"" + classname + "\"]";
            NodeTest(true);
        }
        #endregion

        #region Hyphenation
        [Test]
        public void Hyphenation1()
        {
            const string classname = "withhyphen";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Hyphenation.css");
            _expected.Add("Hyphenation", "true");
            _expected.Add("HyphenateBeforeLast", "2");
            _expected.Add("HyphenateAfterFirst", "3");
            _expected.Add("HyphenateLadderLimit", "1");
            XPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[7][@Name = \"" + classname + "\"]";
            NodeTest(true);
        }

        [Test]
        public void Hyphenation2()
        {
            const string classname = "withouthyphen";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/Hyphenation.css");
            _expected.Add("Hyphenation", "false");
            _expected.Add("HyphenateBeforeLast", "2");
            _expected.Add("HyphenateAfterFirst", "3");
            _expected.Add("HyphenateLadderLimit", "1");
            XPath = "//RootParagraphStyleGroup[1]/ParagraphStyle[8][@Name = \"" + classname + "\"]";
            NodeTest(true);
        }
        #endregion

        #region Position
        [Test]
        public void Position()
        {
            _className = "positionLeft";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/position.css");
            _cssProperty = _cssTree.CreateCssProperty(_input, true);
            _stylesXML.CreateIDStyles(_output, _cssProperty);
            FileNameWithPath = Common.PathCombine(_output, "Styles.xml");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";

            _expected.Add("LeftIndent", "30");
            Assert.IsTrue(ValidateNodeAttribute(), " failed for postionLeft");

            _className = "positionRight";
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";
            _expected.Add("RightIndent", "50");
            Assert.IsTrue(ValidateNodeAttribute(), " failed for postionRight");

        }
        #endregion

        #region IncreaseFontSizeForSuper
        [Test]
        public void IncreaseFontSizeForSuper()
        {
            _className = "VerseNumber";
            _input = Common.DirectoryPathReplace(_testFolderPath + "/input/IncreaseFontSizeForSuper.css");
            _cssProperty = _cssTree.CreateCssProperty(_input, true);
            _stylesXML.CreateIDStyles(_output, _cssProperty);
            FileNameWithPath = Common.PathCombine(_output, "Styles.xml");
            XPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + _className + "\"]";

            _expected.Add("PointSize", "100%");
            Assert.IsTrue(ValidateNodeAttribute(), " failed for IncreaseFontSizeForSuper");
        }
        #endregion

        [Test]
        // TD-1973 and TD-1974
        public void RemoveRelativeInFootnote()
        {
            string _inputXHTML = Common.DirectoryPathReplace(_testFolderPath + "/input/RemoveRelativeInFootnote.xhtml");
            string _inputCSS = Common.DirectoryPathReplace(_testFolderPath + "/input/RemoveRelativeInFootnote.css");
            _cssProperty = _cssTree.CreateCssProperty(_inputCSS, true);
            _idAllClass = _stylesXML.CreateIDStyles(_outputStyles, _cssProperty);
            projInfo.DefaultXhtmlFileWithPath = _inputXHTML;
            _storyXML.CreateStory(projInfo, _idAllClass, _cssTree.SpecificityClass, _cssTree.CssClassOrder);
            FileNameWithPath = Common.PathCombine(_output, "Styles.xml");

            string classname = "NoteGeneralParagraph..footnote-call";
            XPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"" + classname + "\"]";
            _expected.Add("BaselineShift", "50%");
            Assert.IsFalse(ValidateNodeAttribute(), " failed for Footnote - BaselineShift");

            classname = "NoteGeneralParagraph..footnote-marker";
            XPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"" + classname + "\"]";
            _expected.Add("Leading", "100%");
            Assert.IsFalse(ValidateNodeAttribute(), " failed for Footnote - Leading");
        }


        #endregion
    }
}
