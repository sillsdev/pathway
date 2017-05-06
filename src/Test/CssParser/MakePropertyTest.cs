// --------------------------------------------------------------------------------------------
// <copyright file="MakePropertyTest.cs" from='2009' to='2014' company='SIL International'>
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
// MakePropertyTest Test
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

#endregion Using

namespace Test.CssParserTest
{
    [TestFixture]
    public class MakePropertyTest
    {
        #region Private Variables
        MakeProperty _makeProperty;
        StyleAttribute _input;
        private Dictionary<string, string> _expected;
        private Dictionary<string, string> _output;
        private string[] _position;
        #endregion Private Variables

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _input = new StyleAttribute();
            _makeProperty = new MakeProperty();
            _expected = new Dictionary<string, string>();
            _position = new[] { "top", "right", "bottom", "left" };
        }
        #endregion Setup

        #region Public Functions

        #region Margin 
        [Test]
        public void Margin1()
        {
            _input.Name = "margin";
            _input.StringValue = "1,pt";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("margin-top", "1");
            _expected.Add("margin-right", "1");
            _expected.Add("margin-bottom", "1");
            _expected.Add("margin-left", "1");
            Assert.IsTrue(CompareDictionary(), "margin 4 parameters test Failed");
        }

        [Test]
        public void Margin2()
        {
            _input.Name = "margin";
            _input.StringValue = "1,pt,2,pt";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("margin-top", "1");
            _expected.Add("margin-right", "2");
            _expected.Add("margin-bottom", "1");
            _expected.Add("margin-left", "2");
            Assert.IsTrue(CompareDictionary(), "margin 4 parameters test Failed");
        }

        [Test]
        public void Margin3()
        {
            _input.Name = "margin";
            _input.StringValue = "1,pt,2,pt,3,pt";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("margin-top", "1");
            _expected.Add("margin-right", "2");
            _expected.Add("margin-bottom", "3");
            _expected.Add("margin-left", "2");
            Assert.IsTrue(CompareDictionary(), "margin 4 parameters test Failed");
        }

        [Test]
        public void Margin4()
        {
            _input.Name = "margin";
            _input.StringValue = "1,pt,2,pt,3,pt,4,pt";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("margin-top", "1");
            _expected.Add("margin-right", "2");
            _expected.Add("margin-bottom", "3");
            _expected.Add("margin-left", "4");
            Assert.IsTrue(CompareDictionary(), "margin 4 parameters test Failed");
        }

        [Test]
        public void Margin5()
        {
            _input.Name = "margin";
            _input.StringValue = "1,m";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("margin-top", "1");
            _expected.Add("margin-right", "1");
            _expected.Add("margin-bottom", "1");
            _expected.Add("margin-left", "1");
            string error = _input.Name + ":" + _input.StringValue;
            Assert.IsTrue(CompareDictionary(), error);

            _input.StringValue = "1";
            error = _input.Name + ":" + _input.StringValue;
            Assert.IsTrue(CompareDictionary(), error);

            _input.StringValue = "cm";
            error = _input.Name + ":" + _input.StringValue;
            Assert.IsTrue(CompareDictionary(), error);

            _input.StringValue = "";
            error = _input.Name + ":" + _input.StringValue;
            Assert.IsTrue(CompareDictionary(), error);
        }
        #endregion Margin

        [Test]
        public void FontSize1()
        {
            _input.Name = "font-size";
            _input.StringValue = "2,in";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-size", "144");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void FontSize2()
        {
            _input.Name = "font-size";
            _input.StringValue = "50,%";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-size","50%");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void FontSize3()
        {
            _input.Name = "font-size";
            _input.StringValue = "0.5,em";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-size", "50%");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }
        [Test]
        public void FontSize4()
        {
            _input.Name = "font-size";
            _input.StringValue = "10,abc";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-size", "");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }
        [Test]
        public void FontSize5()
        {
            _input.Name = "font-size";
            _input.StringValue = "10";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-size", "10");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }


        [Test]
        public void FontSize6()
        {
            _input.Name = "font-size";
            _input.StringValue = "xx-small";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-size", "6.6");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void FontSize7()
        {
            _input.Name = "font-size";
            _input.StringValue = "x-small";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-size", "7.5");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void FontSize8()
        {
            _input.Name = "font-size";
            _input.StringValue = "small";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-size", "10");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void FontSize9()
        {
            _input.Name = "font-size";
            _input.StringValue = "medium";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-size", "12");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void FontSize10()
        {
            _input.Name = "font-size";
            _input.StringValue = "large";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-size", "14");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void FontSize11()
        {
            _input.Name = "font-size";
            _input.StringValue = "x-large";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-size", "18");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void FontSize12()
        {
            _input.Name = "font-size";
            _input.StringValue = "xx-large";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-size", "24");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        #region Border
        /// <summary>
        ///A test for Border
        ///</summary>
        [Test]
        public void Border1()
        {
            _input.Name = "border";
            _input.StringValue = ".8,pt,solid,red";
            _output = _makeProperty.CreateProperty(_input);
            BorderExpectedValues(); 
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void Border2()
        {
            _input.Name = "border";
            _input.StringValue = "solid,.8,pt,red";
            _output = _makeProperty.CreateProperty(_input);
            BorderExpectedValues(); 
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void Border3()
        {
            _input.Name = "border";
            _input.StringValue = "solid,red,.8,pt";
            _output = _makeProperty.CreateProperty(_input);
            BorderExpectedValues(); 
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void Border4()
        {
            _input.Name = "border";
            _input.StringValue = "red,solid,.8,pt";
            _output = _makeProperty.CreateProperty(_input);
            BorderExpectedValues(); 
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void BorderTop()
        {
            _input.Name = "border-top";
            _input.StringValue = "red,solid,.8,pt";
            _output = _makeProperty.CreateProperty(_input);
            _position = new[] {"top"};
            BorderExpectedValues();
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void BorderBottom()
        {
            _input.Name = "border-bottom";
            _input.StringValue = "red,solid,.8,pt";
            _output = _makeProperty.CreateProperty(_input);
            _position = new[] { "bottom" };
            BorderExpectedValues(); 
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void BorderRight()
        {
            _input.Name = "border-right";
            _input.StringValue = "red,solid,.8,pt";
            _output = _makeProperty.CreateProperty(_input);
            _position = new[] { "right" };
            BorderExpectedValues();
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void BorderLeft()
        {
            _input.Name = "border-left";
            _input.StringValue = "red,solid,.8,pt";
            _output = _makeProperty.CreateProperty(_input);
            _position = new[] { "left" };
            BorderExpectedValues(); 
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void BorderInch2Pt()
        {
            _input.Name = "border-left";
            _input.StringValue = "#f00,solid,.8,in";
            _output = _makeProperty.CreateProperty(_input);
            _position = new[] { "left" };
            _expected.Clear();
            foreach (string pos in _position)
            {
                _expected.Add("border-" + pos + "-width", "57.6"); // inch to pt
                _expected.Add("border-" + pos + "-style", "solid");
                _expected.Add("border-" + pos + "-color", "#ff0000"); // #f00 to #ff0000
            }
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void BorderWithOutColor()
        {
            _input.Name = "border-left";
            _input.StringValue = "solid,.8,in";
            _output = _makeProperty.CreateProperty(_input);
            _position = new[] { "left" };
            _expected.Clear();
            foreach (string pos in _position)
            {
                _expected.Add("border-" + pos + "-width", "57.6"); // inch to pt
                _expected.Add("border-" + pos + "-style", "solid");
                _expected.Add("border-" + pos + "-color", "#000000"); //No color - black is expected
            }
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void BorderWithoutColorAndWidth()
        {
            _input.Name = "border-left";
            _input.StringValue = "solid";
            _output = _makeProperty.CreateProperty(_input);
            _position = new[] { "left" };
            _expected.Clear();
            foreach (string pos in _position)
            {
                _expected.Add("border-" + pos + "-width", ".5"); 
                _expected.Add("border-" + pos + "-style", "solid");
                _expected.Add("border-" + pos + "-color", "#000000"); 
            }
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void BorderWithoutStyle()
        {
            _input.Name = "border-left";
            _input.StringValue = "#f00,.8,in";
            _output = _makeProperty.CreateProperty(_input);
            _position = new[] { "left" };
            _expected.Clear();
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        #endregion Border Test

        #region Size
        /// <summary>
        ///A test for Size
        ///</summary>
        [Test]

        public void Size()
        {
            _input.Name = "size";
            _input.StringValue = "8.5,in,11,in";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("page-width", "612");
            _expected.Add("page-height", "792");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }
        #endregion Size


        /// <summary>
        ///A test for SimpleProperty
        ///</summary>
        [Test]
        public void SimpleProperty()
        {
            _input.Name = "text-align";
            _input.StringValue = "right";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("text-align", "right");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        /// <summary>
        ///A test for SimpleProperty
        ///</summary>
        [Test]
        public void SimplePropertyWithUnit()
        {
            _input.Name = "font-size";
            _input.StringValue = "12,in";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-size", "864");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        /// <summary>
        ///A test for GetRgbConcat
        ///</summary>
        [Test]

        public void GetRgbConcat()
        {
            _input.Name = "color";
            _input.StringValue = "rgb,(,255,0,0,)";
            string output = _makeProperty.ColorRGB(_input.StringValue);
            const string expected = "#FF0000";
            Assert.AreEqual(expected,output, _input.Name + " : " + _input.StringValue + " test Failed");
        }

        #region FontFamily
        [Test]
        public void FontFamily1()
        {
            _input.Name = "font-family";
            _input.StringValue = "\"Times New Roman\",serif";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-family", "Times New Roman");
            Assert.IsTrue(CompareDictionary(), CompareMessage());
        }

        [Test]
        public void FontFamily2()
        {
            _input.Name = "font-family";
            _input.StringValue = "sans-serif";
            _makeProperty.PsSupportPath = PathPart.Bin(Environment.CurrentDirectory, "/../../DistFiles");
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-family", "Verdana");
            Assert.IsTrue(CompareDictionary(), CompareMessage());
        }

        [Test]
        public void FontFamily3()
        {
            _input.Name = "font-family";
            _input.StringValue = "Georgia, \"Times New Roman\",serif";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-family", "Georgia");
            Assert.IsTrue(CompareDictionary(), CompareMessage());
        }

        [Test]
        public void FontFamily4()
        {
            _input.Name = "font-family";
            _input.StringValue = "dummyfont, Georgia,\"Times New Roman\",serif";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-family", "Georgia");
            Assert.IsTrue(CompareDictionary(), CompareMessage());
        }

        [Test]
        public void FontFamily5()
        {
            _input.Name = "font-family";
            _input.StringValue = "dummyfont";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-family", "dummyfont");
            Assert.IsTrue(CompareDictionary(), CompareMessage());
        }
        [Test]
        public void FontFamily6()
        {
            _input.Name = "font-family";
            _input.StringValue = "dummyfont, dummyfamily";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-family", "dummyfamily");
            Assert.IsTrue(CompareDictionary(), CompareMessage());
        }
        [Test]
        public void FontFamily7()
        {
            _input.Name = "font-family";
            _input.StringValue = "Arial, sans-serif";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-family", "Arial");
            Assert.IsTrue(CompareDictionary(), CompareMessage());
        }

        [Test]
        public void FontFamily8()
        {
            _input.Name = "font-family";
            _input.StringValue = "inherit";
            _output = _makeProperty.CreateProperty(_input);
            Assert.IsTrue(_output.Count == 0, _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void FontFamily9()
        {
            _input.Name = "font-family";
            _input.StringValue = "dummyfont, fantasy";
            _makeProperty.PsSupportPath = PathPart.Bin(Environment.CurrentDirectory, "/../../DistFiles");
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-family", "Modern");
            Assert.IsTrue(CompareDictionary(), CompareMessage());
        }

	    [Test]
		[Category("SkipOnTC")]
	    public void FontFamily10()
	    {
			_input.Name = "font-family";
			_input.StringValue = "Charis SIL";
			_output = _makeProperty.CreateProperty(_input);
			_expected.Clear();
			_expected.Add("font-family", "Charis SIL");
			Assert.IsTrue(CompareDictionary(), CompareMessage());

	    }
        #endregion FontFamily



        /// <summary>
        ///A test for Font
        ///</summary>
        /// <example> font: italic small-caps bold 24pt/100% Palatino, serif </example>  
        [Test]
        public void Font()
        {
            _input.Name = "font";
            _input.StringValue = "italic,small-caps,bold,24,pt,100,%,Times New Roman,serif";
            _makeProperty.ClearProperty();
            _makeProperty.Font(_input);
            _expected.Clear();
            _expected.Add("font-size", "24");
            _expected.Add("font-family", "Times New Roman");
            _expected.Add("font-style", "italic");
            _expected.Add("font-variant", "small-caps");
            _expected.Add("font-weight", "bold");
            _expected.Add("line-height", "100%");
            _output = _makeProperty.GetProperty;
            Assert.IsTrue(CompareDictionary(), CompareMessage());
        }

        /// <summary>
        ///A test for DeleteSeperator
        ///</summary>
        [Test]

        public void DeleteSeperator()
        {
            _input.Name = "DeleteSeperator";
            _input.StringValue = "12,in";
            string output = _makeProperty.DeleteSeperator(_input.StringValue);
            const string expected = "12in";
            Assert.AreEqual(expected,output , _input.Name + " : " + _input.StringValue + " test Failed");
        }

        /// <summary>
        ///A test for GetMarginValue
        ///</summary>
        [Test]

        public void Convert2pt()
        {
            _input.Name = "GetMarginValue";
            _input.StringValue = "12,in";
            const string expected = "864";
            ArrayList output = _makeProperty.GetPropertyValue(_input);
            string outputString = output[0].ToString();
            Assert.AreEqual(expected, outputString, _input.Name + " : " + _input.StringValue + " test Failed");
        }

        /// <summary>
        ///A test for ColorRGB
        ///</summary>
        /// <example>rgb(125,255,255)</example>
        [Test]

        public void ColorRGBTest()
        {
            _input.Name = "color RGB";
            _input.StringValue = "rgb,(,125,255,255,)";
            const string expected = "#7DFFFF"; 
            string actual = _makeProperty.ColorRGB(_input.StringValue);
            Assert.AreEqual(expected, actual, _input.Name + " : " + _input.StringValue + " test Failed");
        }


        /// <summary>
        ///A test for ColorCMYK
        ///</summary>
        /// <example>cmyk,(,0.5,0.1,0.0,0.2,)</example>
        [Test]

        public void ColorCMYKTest()
        {
            _input.Name = "color CMYK";
            _input.StringValue = "cmyk,(,0.5,0.1,0.0,0.2,)";
            const string expected = "rgb,(,102,184,204,)";
            string actual = _makeProperty.ColorCMYK(_input.StringValue);
            Assert.AreEqual(expected, actual, _input.Name + " : " + _input.StringValue + " test Failed");
        }

        /// <summary>
        ///A test for Color2Hash
        ///</summary>
        [Test]

        public void ColorHash()
        {
            _input.Name = "color #";
            _input.StringValue = "#f0f";
            const string expected = "#ff00ff";
            string actual = _makeProperty.ColorHash(_input.StringValue);
            Assert.AreEqual(expected, actual, _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void FontWeight1()
        {
            _input.Name = "font-weight";
            _input.StringValue = "400";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-weight", "normal");
            Assert.IsTrue(CompareDictionary(), _input.Name + " : " + _input.StringValue + " test Failed");
        }

        [Test]
        public void FontWeight2()
        {
            _input.Name = "font-weight";
            _input.StringValue = "700";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-weight", "bold");
            Assert.IsTrue(CompareDictionary(), CompareMessage());
        }

        [Test]
        public void FontWeight3()
        {
            _input.Name = "font-weight";
            _input.StringValue = "lighter";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-weight", "normal");
            Assert.IsTrue(CompareDictionary(), CompareMessage());
        }

        [Test]
        public void FontWeight4()
        {
            _input.Name = "font-weight";
            _input.StringValue = "bolder";
            _output = _makeProperty.CreateProperty(_input);
            _expected.Clear();
            _expected.Add("font-weight", "bold");
            Assert.IsTrue(CompareDictionary(), CompareMessage());
        }

		/// <summary>
		///A test for Text Decoration Property
		///</summary>
		[Test]
		public void TextDecorationPropertyTest()
		{
			_input.Name = "text-decoration";
			_input.StringValue = "line-through";
			_output = _makeProperty.CreateProperty(_input);
			_expected.Clear();
			_expected.Add("text-decoration", "line-through");
			Assert.IsTrue(CompareDictionary(), CompareMessage());
		}

        #endregion Public Functions

        

        #region private Functions
        private void BorderExpectedValues()
        {
            _expected.Clear();
            foreach (string pos in _position)
            {
                _expected.Add("border-" + pos + "-width", ".8");
                _expected.Add("border-" + pos + "-style", "solid");
                _expected.Add("border-" + pos + "-color", "#ff0000");
            }
        }

        private string _compareExpected;
        private string _compareActual;
        /// <summary>
        /// Compare each value for keys in the dictionary
        /// </summary>
        /// <returns></returns>
        private bool CompareDictionary()
        {
            bool compare = true;
            foreach (KeyValuePair<string, string> dicData in _expected)
            {
                if (dicData.Value != _output[dicData.Key])
                {
                    _compareExpected = dicData.Value;
                    _compareActual = _output[dicData.Key];
                    compare = false;
                    break;
                }
            }
            return compare;
        }

        /// <summary>
        /// Return the results of failed compare operations as a string.
        /// </summary>
        /// <returns>Formatted result message</returns>
        private string CompareMessage()
        {
            return _input.Name + " : expected '" + _compareExpected + "' but was actually '" + _compareActual +
                   "'. Test Failed.";
        }

        #endregion private Functions


    }
}