using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SIL.PublishingSolution;

namespace Test.InDesignConvert
{
    [TestFixture]
    public class InMapPropertyTest
    {
        #region Private Variables

        private string _methodName;
        InMapProperty _makeAttribute;
        private Dictionary<string, string> _input;
        private Dictionary<string, string> _output;
        private Dictionary<string, string> _expected;

        #endregion Private Variables

        #region Setup
        [TestFixtureSetUp]
        protected void SetUpAll()
        {
            _input = new Dictionary<string, string>();
            _expected = new Dictionary<string, string>();
            _output = new Dictionary<string, string>();
        }

        [SetUp]
        protected void SetupEach()
        {
            // _makeAttribute has a side effect in _IsKeepLineWrittern 
            _makeAttribute = new InMapProperty();
        }
        #endregion Setup

        #region Public Functions

        #region FontSize
        [Test]
        public void FontSize1()
        {
            _input.Add("font-size", "24");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("PointSize", "24");
            Assert.IsTrue(CompareDictionary(), "FontSize1 test Failed");
        }

        [Test]
        public void FontSize2()
        {
            _input.Clear();
            _input.Add("font-size", "");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 0, "Empty FontSize test Failed");
        }

        [Test]
        public void FontSize3()
        {
            _input.Clear();
            _input.Add("font-size", "-24");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 0, "Negative FontSize test Failed");
        }

        [Test]
        public void FontSize4()
        {
            _input.Clear();
            _input.Add("font-size", "ng");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 0, "Invalid input test Failed");
        }

        [Test]
        public void FontSize5()
        {
            _input.Clear();
            _input.Add("font-size", "inherit");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 0, "Invalid input test Failed");
        }

        
        #endregion
        
        #region ColumnCount

        /// <summary>
        ///A test for Column Count 2
        ///</summary>
        [Test]
        public void ColumnCount1()
        {
            _methodName = "ColumnCount1";
            _input.Clear();
            _input.Add("column-count", "2");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("TextColumnCount", "2");
            Assert.IsTrue(CompareDictionary(), _methodName + " test Failed");
        }

        /// <summary>
        ///A test for Column Count ""
        ///</summary>
        [Test]
        public void ColumnCount2()
        {
            _methodName = "ColumnCount2";
            _input.Clear();
            _input.Add("column-count", "");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("TextColumnCount", "1");
            Assert.IsTrue(CompareDictionary(), _methodName + " test Failed");
        }

        /// <summary>
        ///A test for Column Count "sd"
        ///</summary>
        [Test]
        public void ColumnCount3()
        {
            _methodName = "ColumnCount3";
            _input.Clear();
            _input.Add("column-count", "sd");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("TextColumnCount", "1");
            Assert.IsTrue(CompareDictionary(), _methodName + " test Failed");
        }

        /// <summary>
        ///A test for Column Count "-2"
        ///</summary>
        [Test]
        public void ColumnCount4()
        {
            _methodName = "ColumnCount4";
            _input.Clear();
            _input.Add("column-count", "-2");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("TextColumnCount", "1");
            Assert.IsTrue(CompareDictionary(), _methodName + " test Failed");
        }


        #endregion

        #region ColumnGap

        /// <summary>
        ///A test for Column Gap 15
        ///</summary>
        [Test]
        public void ColumnGap1()
        {
            _methodName = "ColumnGap1";
            _input.Clear();
            _input.Add("column-gap", "15");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("TextColumnGutter", "15");
            Assert.IsTrue(CompareDictionary(), _methodName + " test Failed");
        }

        /// <summary>
        ///A test for Column Gap ""
        ///</summary>
        [Test]
        public void ColumnGap2()
        {
            _methodName = "ColumnGap2";
            _input.Clear();
            _input.Add("column-gap", "");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("TextColumnGutter", "12");
            Assert.IsTrue(CompareDictionary(), _methodName + " test Failed");
        }

        /// <summary>
        ///A test for Column Gap "sd"
        ///</summary>
        [Test]
        public void ColumnGap3()
        {
            _methodName = "ColumnGap3";
            _input.Clear();
            _input.Add("column-gap", "sd");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("TextColumnGutter", "12");
            Assert.IsTrue(CompareDictionary(), _methodName + " test Failed");
        }

        /// <summary>
        ///A test for Column Gap "-2"
        ///</summary>
        [Test]
        public void ColumnGap4()
        {
            _methodName = "ColumnGap4";
            _input.Clear();
            _input.Add("column-gap", "-2");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("TextColumnGutter", "12");
            Assert.IsTrue(CompareDictionary(), _methodName + " test Failed");
        }

        #endregion

        #region FontWeight
        [Test]
        public void FontWeight1()
        {
            _input.Clear();
            _input.Add("font-weight", "normal");
            _input.Add("font-style", "normal");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("FontStyle", "Regular");
            Assert.IsTrue(CompareDictionary(), "Normal test Failed");
        }

        [Test]
        public void FontWeight2()
        {
            _input.Clear();
            _input.Add("font-weight", "bold");
            _input.Add("font-style", "normal");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("FontStyle", "Bold");
            Assert.IsTrue(CompareDictionary(), "Bold test Failed");
        }

        [Test]
        public void FontWeight3()
        {
            _input.Clear();
            _output = new Dictionary<string, string>();
            _input.Add("font-weight", "normal");
            _input.Add("font-style", "italic");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("FontStyle", "Italic");
            Assert.IsTrue(CompareDictionary(), "Italic test Failed");
        }

        [Test]
        public void FontWeight4()
        {
            _input.Clear();
            _output = new Dictionary<string, string>();
            _input.Add("font-weight", "bold");
            _input.Add("font-style", "italic");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("FontStyle", "Bold Italic");
            Assert.IsTrue(CompareDictionary(), "Bold Italic test Failed");
        }

        [Test]
        public void FontWeight5()
        {
            _input.Clear();
            _output = new Dictionary<string, string>();
            _input.Add("font-weight", "bold");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("FontStyle", "Bold");
            Assert.IsTrue(CompareDictionary(), "Bold alone test Failed");
        }

        [Test]
        public void FontWeight6()
        {
            _input.Clear();
            _output = new Dictionary<string, string>();
            _input.Add("font-style", "italic");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("FontStyle", "Italic");
            Assert.IsTrue(CompareDictionary(), "Italic alone test Failed");
        }

        [Test]
        public void FontWeight7()
        {
            _input.Clear();
            _output = new Dictionary<string, string>();
            _input.Add("font-weight", "normal");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("FontStyle", "Regular");
            Assert.IsTrue(CompareDictionary(), "Normal FontWeight alone test Failed");
        }

        [Test]
        public void FontWeight8()
        {
            _input.Clear();
            _output = new Dictionary<string, string>();
            _input.Add("font-weight", "inherit");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 0, "Inherit FontWeight alone test Failed");
        }

        [Test]
        public void FontWeight9()
        {
            _input.Clear();
            _output = new Dictionary<string, string>();
            _input.Add("font-style", "normal");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("FontStyle", "Regular");
            Assert.IsTrue(CompareDictionary(), "Normal FontStyle alone test Failed");
        }
       
        #endregion

        #region TextAlign
        [Test]
        public void TextAlign1()
        {
            _input.Clear();
            _input.Add("text-align", "center");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Justification", "CenterAlign");
            Assert.IsTrue(CompareDictionary(), "Center Align test Failed");
        }

        [Test]
        public void TextAlign2()
        {
            _input.Clear();
            _input.Add("text-align", "left");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Justification", "LeftAlign");
            Assert.IsTrue(CompareDictionary(), "Left Align test Failed");
        }

        [Test]
        public void TextAlign3()
        {
            _input.Clear();
            _input.Add("text-align", "right");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Justification", "RightAlign");
            Assert.IsTrue(CompareDictionary(), "Right Align test Failed");
        }

        [Test]
        public void TextAlign4()
        {
            _input.Clear();
            _input.Add("text-align", "justify");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Justification", "FullyJustified");
            Assert.IsTrue(CompareDictionary(), "Justify Align test Failed");
        }

        [Test]
        public void TextAlign5()
        {
            _input.Clear();
            _input.Add("text-align", "");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 0, "Empty Align test Failed");
        }

        [Test]
        public void TextAlign6()
        {
            _input.Clear();
            _input.Add("text-align", "inherit");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 0, "Inherit Align test Failed");
        }
        #endregion

        #region TextDecoration
        [Test]
        public void TextDecoration1()
        {
            _input.Clear();
            _input.Add("text-decoration", "none");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Underline", "false");
            Assert.IsTrue(CompareDictionary(), "text-decoration(none) test Failed");
        }

        [Test]
        public void TextDecoration2()
        {
            _input.Clear();
            _input.Add("text-decoration", "underline");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Underline", "true");
            Assert.IsTrue(CompareDictionary(), "text-decoration(underline) test Failed");
        }

        [Test]
        public void TextDecoration3()
        {
            _input.Clear();
            _input.Add("text-decoration", "inherit");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 0, "text-decoration(inherit) test Failed");
        } 
        #endregion

        #region FontVariant
        [Test]
        public void FontVariant1()
        {
            _input.Clear();
            _input.Add("font-variant", "normal");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Capitalization", "Normal");
            Assert.IsTrue(CompareDictionary(), "FontVariant(normal) test Failed");
        }

        [Test]
        public void FontVariant2()
        {
            _input.Clear();
            _input.Add("font-variant", "small-caps");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Capitalization", "SmallCaps");
            Assert.IsTrue(CompareDictionary(), "FontVariant(SmallCaps) test Failed");
        }

        [Test]
        public void FontVariant3()
        {
            _input.Clear();
            _input.Add("font-variant", "inherit");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 0, "FontVariant(inherit) test Failed");
        } 
        #endregion

        #region MarginLeft
        [Test]
        public void MarginLeft1()
        {
            _input.Clear();
            _input.Add("margin-left", "36");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Margin-Left", "36");
            Assert.IsTrue(CompareDictionary(), "MarginLeft1 test Failed");
        }

        [Test]
        public void MarginLeft2()
        {
            _input.Clear();
            _input.Add("margin-left", "");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 0, "MarginLeft2 test Failed");
        } 
        #endregion

        #region MarginRight
        [Test]
        public void MarginRight1()
        {
            _input.Clear();
            _input.Add("margin-right", "36");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Margin-Right", "36");
            Assert.IsTrue(CompareDictionary(), "MarginRight1 test Failed");
        }

        [Test]
        public void MarginRight2()
        {
            _input.Clear();
            _input.Add("margin-right", "");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 0, "MarginRight2 test Failed");
        }
        #endregion

        #region MarginTop
        [Test]
        public void MarginTop1()
        {
            _input.Clear();
            _input.Add("margin-top", "36");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Margin-Top", "36");
            Assert.IsTrue(CompareDictionary(), "MarginTop1 test Failed");
        }

        [Test]
        public void MarginTop2()
        {
            _input.Clear();
            _input.Add("margin-top", "");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 0, "MarginTop2 test Failed");
        }
        #endregion

        #region MarginBottom
        [Test]
        public void MarginBottom1()
        {
            _input.Clear();
            _input.Add("margin-bottom", "36");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Margin-Bottom", "36");
            Assert.IsTrue(CompareDictionary(), "MarginTop1 test Failed");
        }

        [Test]
        public void MarginBottom2()
        {
            _input.Clear();
            _input.Add("margin-bottom", "");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 0, "MarginTop2 test Failed");
        }
        #endregion

        #region Padding
        [Test]
        public void PaddingTop()
        {
            _input.Clear();
            _input.Add("padding-top", "36");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("SpaceBefore", "36");
            Assert.IsTrue(CompareDictionary(), "PaddingTop test Failed");
        }

        [Test]
        public void PaddingBottom()
        {
            _input.Clear();
            _input.Add("padding-bottom", "36");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("SpaceAfter", "36");
            Assert.IsTrue(CompareDictionary(), "PaddingBottom test Failed");
        }

        [Test]
        public void PaddingLeft()
        {
            _input.Clear();
            _input.Add("padding-left", "36");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("LeftIndent", "36");
            Assert.IsTrue(CompareDictionary(), "PaddingLeft test Failed");
        }

        [Test]
        public void PaddingRight()
        {
            _input.Clear();
            _input.Add("padding-right", "60");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("RightIndent", "60");
            Assert.IsTrue(CompareDictionary(), "PaddingRight test Failed");
        }
        #endregion

        #region Visibility

        [Test]
        public void Visibility1()
        {
            _input.Clear();
            _input.Add("visibility", "visible");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 1, "Visibility(visible) test Failed");
        }

        [Test]
        public void Visibility2()
        {
            _input.Clear();
            _input.Add("visibility", "hidden");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("visibility", "hidden");
            Assert.IsTrue(CompareDictionary(), "Visibility(hidden) test Failed");
        }

        [Test]
        public void Visibility3()
        {
            _input.Clear();
            _input.Add("visibility", "visible");
            _input.Add("color", "#ff0000");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("FillColor", "Color/#ff0000");
            Assert.IsTrue(CompareDictionary(), "Visibility(visible and color) test Failed");
        }

        [Test]
        public void Visibility4()
        {
            _input.Clear();
            _input.Add("visibility", "inherit");
            _output = _makeAttribute.IDProperty(_input);
            Assert.IsTrue(_output.Count == 1, "Visibility(inherit) test Failed");
        } 


        #endregion

        #region TextIndent
        [Test]
        public void TextIndent1()
        {
            _input.Clear();
            _input.Add("text-indent", "-36");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("FirstLineIndent", "-36");
            Assert.IsTrue(CompareDictionary(), "TextIndent test Failed");
        } 
        #endregion

        #region TextTranform
        [Test]
        public void TextTransform()
        {
            _input.Clear();
            _input.Add("text-transform", "uppercase");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("TextTransform", "uppercase");
            Assert.IsTrue(CompareDictionary(), "TextTransform test Failed");
        } 
        #endregion 

        #region VerticalAlign
        [Test]
        public void VerticalAlign1()
        {
            _input.Clear();
            _input.Add("vertical-align", "baseline");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Position", "Normal");
            Assert.IsTrue(CompareDictionary(), "VerticalAlign1(Baseline) test Failed");
        }
        [Test]
        public void VerticalAlign2()
        {
            _input.Clear();
            _input.Add("vertical-align", "sub");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Position", "Subscript");
            Assert.IsTrue(CompareDictionary(), "VerticalAlign2(Sub) test Failed");
        }
        [Test]
        public void VerticalAlign3()
        {
            _input.Clear();
            _input.Add("vertical-align", "super");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Position", "Superscript");
            Assert.IsTrue(CompareDictionary(), "VerticalAlign3(Super) test Failed");
        }
        [Test]
        public void VerticalAlign4()
        {
            _input.Clear();
            _input.Add("vertical-align", "12pt");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("BaselineShift", "12");
            Assert.IsTrue(CompareDictionary(), "VerticalAlign4(Point value) test Failed");
        }
        [Test]
        public void VerticalAlign5()
        {
            _input.Clear();
            _input.Add("vertical-align", "top");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("BaselineShift", "50%");
            Assert.IsTrue(CompareDictionary(), "VerticalAlign5(top) test Failed");
        }
        [Test]
        public void VerticalAlign6()
        {
            _input.Clear();
            _input.Add("vertical-align", "bottom");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("BaselineShift", "-50%");
            Assert.IsTrue(CompareDictionary(), "VerticalAlign6(bottom) test Failed");
        }
        [Test]
        public void VerticalAlign7()
        {
            _input.Clear();
            _input.Add("vertical-align", "middle");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("BaselineShift", "0%");
            Assert.IsTrue(CompareDictionary(), "VerticalAlign7(middle) test Failed");
        }
        [Test]
        public void VerticalAlign8()
        {
            _input.Clear();
            _input.Add("vertical-align", "50%");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("BaselineShift", "50%");
            Assert.IsTrue(CompareDictionary(), "VerticalAlign8(Percent value) test Failed");
        }
        #endregion 

        #region Hyphenation
        [Test]
        public void Hyphenation1()
        {
            _input.Clear();
            _input.Add("hyphens", "auto");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Hyphenation", "true");
            Assert.IsTrue(CompareDictionary(), "Hyphenation1(hyphens:auto) test Failed");
        }
        [Test]
        public void Hyphenation2()
        {
            _input.Clear();
            _input.Add("hyphenate-before", "2");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("HyphenateBeforeLast", "2");
            Assert.IsTrue(CompareDictionary(), "Hyphenation2(hyphenate-before) test Failed");
        }
        [Test]
        public void Hyphenation3()
        {
            _input.Clear();
            _input.Add("hyphenate-after", "2");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("HyphenateAfterFirst", "2");
            Assert.IsTrue(CompareDictionary(), "Hyphenation3(hyphenate-after) test Failed");
        }
        [Test]
        public void Hyphenation4()
        {
            _input.Clear();
            _input.Add("hyphenate-lines", "2");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("HyphenateLadderLimit", "2");
            Assert.IsTrue(CompareDictionary(), "Hyphenation4(hyphenate-lines) test Failed");
        }
        [Test]
        public void Hyphenation5()
        {
            _input.Clear();
            _input.Add("hyphens", "none");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Hyphenation", "false");
            Assert.IsTrue(CompareDictionary(), "Hyphenation5(hyphens:none) test Failed");
        }

        [Test]
        public void Widow()
        {
            _input.Clear();
            _input.Add("widows", "4");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("KeepLastLines", "4");
            _expected.Add("KeepLinesTogether", "true");
            Assert.IsTrue(CompareDictionary(), "Widow test Failed");
        }

        [Test]
        public void Orphan()
        {
            _input.Clear();
            _input.Add("orphans", "3");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("KeepFirstLines", "3");
            //_expected.Add("KeepLinesTogether", "true");
            Assert.IsTrue(CompareDictionary(), "Orphan test Failed");
        }
        #endregion 

        #region LineHeight
        [Test]
        public void LineHeight1()
        {
            _input.Clear();
            _input.Add("line-height", "normal");
            _output = _makeAttribute.IDProperty(_input);
            _expected.Clear();
            _expected.Add("Leading", "Auto");
            Assert.IsTrue(CompareDictionary(), "LineHeight1(Normal) test Failed");
        }
        #endregion 
 
        #endregion Public Functions

        #region Private Functions
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CompareDictionary()
        {
            bool compare = true;
            foreach (KeyValuePair<string, string> dicData in _expected)
            {
                if (dicData.Value != _output[dicData.Key])
                {
                    compare = false;
                    break;
                }
            }
            return compare;
        }
        #endregion Private Functions
    }
}
