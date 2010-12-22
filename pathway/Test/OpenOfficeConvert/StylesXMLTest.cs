// --------------------------------------------------------------------------------------------
// <copyright file="StylesXMLTest.cs" from='2009' to='2009' company='SIL International'>
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
// Test Cases for Stylesxml
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Xml;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

#endregion Using

namespace Test.OpenOfficeConvert
{
    [TestFixture]
    [Category("BatchTest")]
    public class StylesXMLTest
    {
        #region Private Variables
        StylesXML _stylesXML;
        string _errorFile;
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;
        private ValidateXMLFile _validate;
        private bool returnValue;
        #endregion Private Variables

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _stylesXML = new StylesXML();
            _errorFile = Common.PathCombine(Path.GetTempPath(), "temp.odt");
            Common.Testing = true;
            returnValue = false;
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/OpenOfficeConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "expected");
            Common.SupportFolder = "";
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, "/../PsSupport");
        }
        #endregion Setup

        #region Private Functions
        private string _outputName = "styles.xml";
        private string _expectedName = "styles.xml";
        private string _errorMessage = " syntax failed in Styles.xml";

        private void FileTest(string file)
        {
            string input = FileInput(file + ".css");
            string output = FileOutput(file + _outputName);
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            string expected = FileExpected(file + _expectedName);
            XmlAssert.AreEqual(expected, output, file + _errorMessage);
            // Reset defaults
            _outputName = "styles.xml";
            _expectedName = "styles.xml";
            _errorMessage = " syntax failed in Styles.xml";
        }

        private string FileInput(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }

        private string FileOutput(string fileName)
        {
            return Common.PathCombine(_outputPath, fileName);
        }

        private string FileExpected(string fileName)
        {
            return Common.PathCombine(_expectedPath, fileName);
        }
        #endregion Private Functions

        #region File Comparision
        ///<summary>
        ///TD-244 (Update CSSParser to handle revised grammar)
        /// <summary>
        /// </summary>      
        [Test]
        public void OxesCSSTest()
        {
            const string file = "Oxes";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "OxesCSSTest failed in styles.xml");
        }
        ///// <summary>
        ///// TD86 .xitem[lang='en'] syntax in Styles.xml
        ///// </summary>
        //[Test]
        //public void LanguageTest()
        //{
        //    FileTest("LanguageTest");
        //}

        //[Test]
        //public void LanguageTestNegative()
        //{
        //    _outputName = "StylesNeg.xml";
        //    FileTest("LanguageTest");
        //}

        //Note why this is in Ignore state
        /////<summary>
        /////TD96 text-indent syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Ignore]
        //public void TextIndentTest()
        //{
        //    _errorMessage = "text-indent syntax failed in Styles.xml";
        //    FileTest("TextIndentTest");
        //}

        /////<summary>
        /////TD100 text-transform syntax in Styles.xml
        ///// 
        ///// </summary>      
        //[Test]
        //public void TextTransformTest()
        //{
        //    string input = FileInput("TD100.css");
        //    string output = FileOutput("stylesTD100.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);
        //    string expected = FileExpected("stylesTD100.xml");
        //    XmlAssert.AreEqual(expected, output, "TextTransform syntax failed in Styles.xml");
        //}

        ///// <summary>
        ///// TD-78a  Open Office color: #123abc;
        ///// </summary>
        //[Test]
        //public void ColorTestPositive()
        //{
        //    _errorMessage = "TD-78,Open Office color: #123abc. Syntax failed in Styles.xml";
        //    FileTest("ColorTestPositive");
        //}
        ///// <summary>
        ///// TD-78a  Open Office color: #123abc;
        ///// </summary>
        //[Ignore]
        //public void ColorTestNegative()
        //{
        //    const string file = "ColorTestPositive";
        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "StylesNeg.xml");
        //    TextFileAssert.AreNotEqual(expected, output, "TD-78,Open Office color: #123abc. Syntax failed in Styles.xml");
        //}

        /////<summary>
        /////TD54 font-Weigth: 400 syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextFontWeightTestA()
        //{
        //    const string file = "TextFontWeightTestA";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font-Weight-Test-A syntax failed in Styles.xml");
        //}

        /////<summary>
        /////TD55 font-Weigth: 700 syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextFontWeightTestB()
        //{
        //    const string file = "TextFontWeightTestB";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font-Weight-Test-B syntax failed in Styles.xml");
        //}

        /////<summary>
        /////TD37 font-Weigtht: bold syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextFontWeightTestC()
        //{
        //    const string file = "TextFontWeightTestC";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font-Weight-Test-C syntax failed in Styles.xml");
        //}


        /////<summary>
        /////TD50 font-Weigtht: normal syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextFontWeightTestD()
        //{
        //    const string file = "TextFontWeightTestD";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font-Weight-Test-D syntax failed in Styles.xml");
        //}

        /////<summary>
        /////TD53 font-Weigtht: inherit; syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextFontWeightTestE()
        //{
        //    const string file = "TextFontWeightTestE";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font-Weight-Test-E syntax failed in Styles.xml");
        //}

        /////<summary>
        /////TD42 font-style: normal; syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextFontStyleTestA()
        //{
        //    const string file = "TextFontStyleTestA";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font-Style-A syntax failed in Styles.xml");
        //}

        /////<summary>
        /////TD43 font-style: inherit; syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextFontStyleTestB()
        //{
        //    const string file = "TextFontStyleTestB";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font-Style-B syntax failed in Styles.xml");
        //}


        /////<summary>
        /////TD49 font-family: in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void FontFamily()
        //{
        //    const string file = "FontFamily";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font-Family syntax failed in Styles.xml");
        //}

        /////<summary>
        /////TD66 text-align: right; syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextAlignTestA()
        //{
        //    const string file = "TextAlignTestA";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Text-Align-A syntax failed in Styles.xml");
        //}


        /////<summary>
        /////TD67 text-align: justify; syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextAlignTestB()
        //{
        //    const string file = "TextAlignTestB";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Text-Align-B syntax failed in Styles.xml");
        //}


        /////<summary>
        /////TD60 font-size: 3cm; syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextFontSizeTestA()
        //{
        //    const string file = "TextFontSizeTestA";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font-Size-A syntax failed in Styles.xml");
        //}


        /////<summary>
        /////TD58 font-size: 1.5em; syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextFontSizeTestB()
        //{
        //    const string file = "TextFontSizeTestB";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font-Size-B syntax failed in Styles.xml");
        //}


        /////<summary>
        /////TD57 font-size: xx-small; syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextFontSizeTestC()
        //{
        //    const string file = "TextFontSizeTestC";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font-Size-C syntax failed in Styles.xml");
        //}


        /////<summary>
        /////TD61 font-size: inherit; syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextFontSizeTestD()
        //{
        //    const string file = "TextFontSizeTestD";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font-Size-D syntax failed in Styles.xml");
        //}
        /////<summary>
        /////TD70 padding-bottom: 9pt; syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void PaddingBottomTest()
        //{
        //    const string file = "PaddingBottomTest";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, "padding-bottom syntax failed in styles.xml");
        //}

        /////<summary>
        /////TD71 padding-top: 1in; syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void PaddingTopTest()
        //{
        //    const string file = "PaddingTopTest";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, "padding-top syntax failed in styles.xml");
        //}

        /////<summary>
        /////TD81 page-break-before: always syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void PageBreakBeforeTest()
        //{
        //    const string file = "PageBreakBeforeTest";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output , "Page Break Before syntax failed in styles.xml");
        //}


        /////<summary>
        /////TD63 font-variant: normal; syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void TextFontVariantTestA()
        //{
        //    const string file = "TextFontVariantTestA";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "Styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "Styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font-Variant-A syntax failed in Styles.xml");
        //}

        /////<summary>
        /////TD51 font-weight: bolder;  in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void FontWeightBolder()
        //{
        //    const string file = "FontWeightBolder";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, "FontWeightBolder syntax failed in styles.xml");
        //}


        /////<summary>
        /////TD-64 font: 80% san-serif; in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void Font()
        //{
        //    const string file = "Font";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Font syntax failed in styles.xml");
        //}

        /////<summary>
        /////TD-94 Open Office content: counter(page) in Styles.xml
        ///// <summary>
        ///// </summary>      
        ////[Test]
        //[Ignore]
        /////Note - We have check the Counter in content.xml
        //public void ContentCounter()
        //{
        //    const string file = "ContentCounter";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        //}

        /////<summary>
        /////TD-161 Vertical-align  - subscript and superscript
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void VerticalAlignTest()
        //{
        //    const string file = "VerticalAlignTest";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output,"VerticalAlignTest  failed in styles.xml");
        //}


        /////<summary>
        /////TD-160 & TD-85 (content: "\2666 ";)
        ///// <summary>
        ///// </summary>      
        ////[Test]
        ////[Ignore]
        ////Note - No use in styles.xml - we have to check it in content.xml
        //public void UnicodeTest()
        //{
        //    const string file = "UnicodeTest";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output,_errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output,"UnicodeTest failed in styles.xml");
        //}

        //Note - this is done based on file comparision - expected style is not exist
        /////<summary>
        /////TD-244 (Update CSSParser to handle revised grammar)
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void OxesCSSTest()
        //{
        //    const string file = "Oxes";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, "OxesCSSTest failed in styles.xml");
        //}

        /////<summary>
        /////TD-270 (Direction:ltr)
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void DirectionTest()
        //{
        //    const string file = "Direction";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, "DirectionTest failed in styles.xml");
        //}

        /////<summary>
        /////TD-305 (widows) and TD-306(orphans)
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void WidowsandOrphans()
        //{
        //    const string file = "WidowsandOrphans";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, "WidowsandOrphans failed in styles.xml");
        //}


        /////<summary>
        /////TD344 page-break-after: always syntax in Styles.xml
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void PageBreakAfterTest()
        //{
        //    const string file = "PageBreakAfterTest";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Page Break After syntax failed in styles.xml");
        //}
        /////<summary>
        /////TD307 Open Office: border-style, border-color, border-width
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void BorderTest()
        //{
        //    const string file = "Border";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, "border-style, border-color and border-width syntax failed in styles.xml");
        //}

        /////<summary>
        /////TD350 position:relative;
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void PositionTest()
        //{
        //    const string file = "Position";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Position syntax failed in styles.xml");
        //}
        /////<summary>
        /////TD-407 Unit Conversions in Map Property;
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void UnitConversionTest()
        //{
        //    const string file = "UnitConversion";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        //}
        /////<summary>
        /////TD-425 Impliment Start and Last References in same page
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void SinglePageRefTest()
        //{
        //    const string file = "SinglePageRef";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Single Page References failed in styles.xml");
        //}
        /////<summary>
        /////TD-428 Impliment Start and Last References in Mirror page
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void MirroredPageRefTest()
        //{
        //    const string file = "MirroredPageRef";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Mirrored Page References syntax failed in styles.xml");
        //}


        /////<summary>
        /////TD-245 Handle hyphenation related keywords
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void HyphenationKeywordsTest()
        //{
        //    const string file = "HyphenationKeywords";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, "Hyphenation Keywords syntax failed in styles.xml");
        //}


        /////<summary>
        /////TD-432 Charis SIL Font
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void CharisSILFont()
        //{
        //    const string file = "CharisSILFont";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        //}

        /////<summary>
        /////TD-46  - Open Office font-family: Gentium
        ///// <summary>
        ///// 
        ///// </summary>      
        ////Note : This is already in Node check
        //[Test]
        //public void GentiumFont()
        //{
        //    const string file = "GentiumFont";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    var xd = new XmlDocument();
        //    xd.Load(output);
        //    XmlNodeList nl = xd.GetElementsByTagName("office:styles");
        //    foreach (XmlNode n in nl[0])
        //    {
        //        if (n.Attributes.GetNamedItem("style:name").Value == "Gentium")
        //        {
        //            string resultFont = n.FirstChild.Attributes.GetNamedItem("fo:font-family").Value;
        //            var fonts = new InstalledFontCollection();
        //            foreach (FontFamily family in fonts.Families)
        //            {
        //                if (family.Name == "Gentium")
        //                {
        //                    Assert.AreEqual(resultFont, "Gentium", "Installed Gentium not recognized");
        //                    return;
        //                }
        //            }
        //            Assert.AreEqual(resultFont, "Times New Roman", "Times New Roman not a recognized substitute for Gentium");
        //            return;
        //        }
        //    }
        //    Assert.Fail("Gentium style missing");
        //    //string expected = FileExpected(file + "styles.xml");
        //    //XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        //}

        /////<summary>
        /////TD-59 (Open Office font-size: larger;) -  Doulos SIL Font
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void DoulosSILFont()
        //{
        //    const string file = "DoulosSILFont";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        //}

        /////<summary>
        /////TD-461 (Open Office fixed-line-height: 14pt;)
        /////</summary>      
        //[Test]
        //public void FixedLineHeightTest()
        //{
        //    const string file = "fixed-line-height";
        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);
        //    var result = new XmlDocument {XmlResolver = null};
        //    result.Load(output);
        //    var root = result.DocumentElement;
        //    Assert.IsNotNull(root, file + " no xml document");
        //    var nsManager = new XmlNamespaceManager(result.NameTable);
        //    foreach (XmlAttribute attribute in root.Attributes)
        //    {
        //        var namePart = attribute.Name.Split(':');
        //        if (namePart[0] == "xmlns")
        //            nsManager.AddNamespace(namePart[1], attribute.Value);
        //    }
        //    //var size = result.SelectSingleNode("/office:document-styles/office:styles/style:style[@style:name=\"entry_section\"]/style:paragraph-properties/@fo:line-height", nsManager);
        //    var styles = result.SelectNodes("/office:document-styles/office:styles/style:style", nsManager);
        //    Assert.IsNotNull(styles, file + " no styles");
        //    XmlElement style = null;
        //    foreach (XmlNode myStyle in styles)
        //    {
        //        if (myStyle.NodeType != XmlNodeType.Element) continue;
        //        style = (XmlElement) myStyle;
        //        var name = style.Attributes.GetNamedItem("style:name").Value;
        //        if (name == "entry")
        //            break;
        //    }
        //    Assert.IsNotNull(style, file + " no entry style");
        //    var pp = (XmlElement)style.SelectSingleNode("style:paragraph-properties",nsManager);
        //    Assert.IsNotNull(pp, file + " no paragraph properties");
        //    var size = pp.GetAttribute("fo:line-height");
        //    Assert.IsNotNull(size, file + " no line height");
        //    Assert.AreEqual("14pt", size, file + " syntax failed in styles.xml");
        //}
        /////<summary>
        /////TD-663 Space between header and text  
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void HeaderSpace()
        //{
        //    const string file = "HeaderSpace";
        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        //}

        /////<summary>
        /////TD-70 Open Office padding-bottom: 2pt;
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void PaddingAll()
        //{
        //    const string file = "Padding_All";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    string expected = FileExpected(file + "styles.xml");
        //    XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        //}

        /////<summary>
        /////TD-70 Open Office padding-bottom: 2pt;
        ///// <summary>
        ///// </summary>      
        //[Test]
        //[Ignore]
        //public void PaddingAll_Node()
        //{
        //    //TODO - FOR NODE TEST
        //    //const string file = "Padding_All";

        //    //string input = FileInput(file + ".css");
        //    //string output = FileOutput(file + "styles.xml");
        //    //_stylesXML.CreateStyles(input, output, _errorFile, true);

        //    //string expected = FileExpected(file + "styles.xml");
        //    //XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        //}
        #endregion

        #region NODE Comparision

        /// <summary>
        /// TD86 .xitem[lang='en'] syntax in Styles.xml
        /// </summary>
        [Test]
        public void LanguageTest_Node()
        {
            const string file = "LanguageTest";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "xitem_.en";
            _validate.ClassProperty.Add("fo:font-size", "50%");
            _validate.ClassProperty.Add("fo:font-size-complex", "50%");


            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "LanguageTest syntax failed in Styles.xml");

            _validate.ClassName = "xitem_.pt";
            _validate.ClassProperty.Add("fo:font-size", "30pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "30pt");


            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "LanguageTest syntax failed in Styles.xml");
        }


        ///<summary>
        ///TD100 text-transform syntax in Styles.xml
        /// 
        /// </summary>      
        [Test]
        public void TextTransformTest_Node()
        {
            string input = FileInput("TD100.css");
            string output = FileOutput("stylesTD100.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "uppercase";
            _validate.ClassProperty.Add("fo:text-transform", "uppercase");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "TextTransform syntax failed in Styles.xml");

            _validate.ClassName = "lowercase";
            _validate.ClassProperty.Add("fo:text-transform", "lowercase");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "TextTransform syntax failed in Styles.xml");

            _validate.ClassName = "Title";
            _validate.ClassProperty.Add("fo:text-transform", "capitalize");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "TextTransform syntax failed in Styles.xml");

        }
        ///<summary>
        ///TD54 font-Weigth: 400 syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontWeightTestA_Node()
        {
            const string file = "TextFontWeightTestA";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-weight", "400");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "Font-Weight-Test-A syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD55 font-Weigth: 700 syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontWeightTestB_Node()
        {
            const string file = "TextFontWeightTestB";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-weight", "700");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "Font-Weight-Test-B syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD37 font-Weigtht: bold syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontWeightTestC_Node()
        {
            const string file = "TextFontWeightTestC";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-weight", "700");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "Font-Weight-Test-C syntax failed in Styles.xml");
        }
        ///<summary>
        ///TD50 font-Weigtht: normal syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontWeightTestD_Node()
        {
            const string file = "TextFontWeightTestD";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-weight", "700");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "Font-Weight-Test-D syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD53 font-Weigtht: inherit; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontWeightTestE_Node()
        {
            const string file = "TextFontWeightTestE";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-weight", "700");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "Font-Weight-Test-E syntax failed in Styles.xml");

            _validate.ClassName = "letter2";
            _validate.ClassProperty.Add("fo:font-weight", "700");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "Font-Weight-Test-E syntax failed in Styles.xml");

        }

        ///<summary>
        ///TD42 font-style: normal; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontStyleTestA_Node()
        {
            const string file = "TextFontStyleTestA";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-style", "normal");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD43 font-style: inherit; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontStyleTestB_Node()
        {
            const string file = "TextFontStyleTestB";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-style", "italic");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            //Note: letter1 should not exist, to write it

            _validate.ClassName = "letter2";
            _validate.ClassProperty.Add("fo:font-style", "italic");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD49 font-family: in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void FontFamily_Node()
        {
            const string file = "FontFamily";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter1";
            _validate.ClassProperty.Add("fo:font-family", "Times New Roman");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "letter2";
            _validate.ClassProperty.Add("fo:font-family", "Verdana");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);


            _validate.ClassName = "letter3"; // Inherit
            _validate.ClassProperty.Add("fo:font-family", "Tahoma");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "letter4";
            _validate.ClassProperty.Add("fo:font-family", "Tahoma");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "letter5";
            _validate.ClassProperty.Add("fo:font-family", "dummyfont");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            // letter6 no style

            _validate.ClassName = "letter7";
            _validate.ClassProperty.Add("fo:font-family", "dummyfamily");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "letter8";
            _validate.ClassProperty.Add("fo:font-family", "Times New Roman");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);


        }

        [Test]
        public void TextAlignTestA_Node()
        {
            const string file = "TextAlignTestA";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:text-align", "right");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

        }


        [Test]
        public void TextAlignTestB_Node()
        {
            const string file = "TextAlignTestB";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:text-align", "justify");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

        }

        ///<summary>
        ///TD60 font-size: 3cm; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontSizeTestA_Node()
        {
            const string file = "TextFontSizeTestA";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-size", "2in");
            _validate.ClassProperty.Add("fo:font-size-complex", "2in");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "letter1";
            _validate.ClassProperty.Add("fo:font-size", "5cm");
            _validate.ClassProperty.Add("fo:font-size-complex", "5cm");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

        }

        ///<summary>
        ///TD58 font-size: 1.5em; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontSizeTestB_Node()
        {
            const string file = "TextFontSizeTestB";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-size", "12pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "12pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "letter1";
            _validate.ClassProperty.Add("fo:font-size", "1.5em");
            _validate.ClassProperty.Add("fo:font-size-complex", "1.5em");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "letter2";
            _validate.ClassProperty.Add("fo:font-size", "1.5em");
            _validate.ClassProperty.Add("fo:font-size-complex", "1.5em");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD57 font-size: xx-small; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontSizeTestC_Node()
        {
            const string file = "TextFontSizeTestC";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-size", "22pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "22pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "letter1";
            _validate.ClassProperty.Add("fo:font-size", "18pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "18pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "letter2";
            _validate.ClassProperty.Add("fo:font-size", "6.6pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "6.6pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }
        ///<summary>
        ///TD61 font-size: inherit; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontSizeTestD_Node()
        {
            const string file = "TextFontSizeTestD";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-size", "22pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "22pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            //Note: letter1 font-size should not exist, to write it
            //_validate.ClassName = "letter1";
            //_validate.ClassProperty.Add("fo:font-size", "18pt");

            //returnValue = _validate.ValidateNodeAttributesNS(false);
            //Assert.IsTrue(returnValue);

            _validate.ClassName = "letter2";
            _validate.ClassProperty.Add("fo:font-size", "6.6pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "6.6pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }


        ///<summary>
        ///TD70 padding-bottom: 9pt; syntax in Styles.xml
        /// <summary>
        /// </summary>     
        ///
        [Ignore]
        [Test]
        public void PaddingBottomTest_Node()
        {
            const string file = "PaddingBottomTest";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-size", "22pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            //Note: letter1 font-size should not exist, to write it
            //_validate.ClassName = "letter1";
            //_validate.ClassProperty.Add("fo:font-size", "18pt");

            //returnValue = _validate.ValidateNodeAttributesNS(false);
            //Assert.IsTrue(returnValue);

            _validate.ClassName = "letter2";
            _validate.ClassProperty.Add("fo:font-size", "6.6pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD81 page-break-before: always syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void PageBreakBeforeTest_Node()
        {
            const string file = "PageBreakBeforeTest";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "entry";
            _validate.ClassProperty.Add("fo:break-before", "page");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD63 font-variant: normal; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontVariantTestA_Node()
        {
            const string file = "TextFontVariantTestA";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-variant", "normal");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "letter1";
            _validate.ClassProperty.Add("fo:font-variant", "small-caps");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD51 font-weight: bolder;  in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void FontWeightBolder_Node()
        {
            const string file = "FontWeightBolder";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "a";
            _validate.ClassProperty.Add("fo:font-weight", "700");
            _validate.ClassProperty.Add("fo:font-weight-complex", "700");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "b";
            _validate.ClassProperty.Add("fo:font-weight", "lighter");
            _validate.ClassProperty.Add("fo:font-weight-complex", "lighter");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "c";
            _validate.ClassProperty.Add("fo:font-weight", "bolder");
            _validate.ClassProperty.Add("fo:font-weight-complex", "bolder");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD-64 font: 80% san-serif; in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void Font_Node()
        {
            const string file = "Font";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "a";
            _validate.ClassProperty.Add("fo:font-weight", "700");
            _validate.ClassProperty.Add("fo:font-weight-complex", "700");
            _validate.ClassProperty.Add("fo:font-size", "24pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "24pt");
            _validate.ClassProperty.Add("fo:font-family", "Times New Roman");

            _validate.ClassProperty.Add("fo:font-name-complex", "Times New Roman");
            _validate.ClassProperty.Add("fo:font-style", "italic");
            _validate.ClassProperty.Add("fo:font-variant", "small-caps");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        [Test]
        public void VerticalAlignTest_Node()
        {
            const string file = "VerticalAlignTest";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "xhomographnumber";
            _validate.ClassProperty.Add("style:text-position", "sub");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "xhomographnumbersuper";
            _validate.ClassProperty.Add("style:text-position", "super");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD-270 (Direction:ltr)
        /// <summary>
        /// </summary>      
        [Test]
        public void DirectionTest_Node()
        {
            const string file = "Direction";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("style:writing-mode", "lr-tb");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "entryFirst";
            _validate.ClassProperty.Add("style:writing-mode", "lr-tb");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "entrylast";
            _validate.ClassProperty.Add("style:writing-mode", "rl-tb");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);
        }


        ///<summary>
        ///TD-305 (widows) and TD-306(orphans)
        /// <summary>
        /// </summary>      
        [Test]
        public void WidowsandOrphans_Node()
        {
            const string file = "WidowsandOrphans";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "orphans";
            _validate.ClassProperty.Add("fo:orphans", "2");
            _validate.ClassProperty.Add("fo:widows", "2");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);
        }


        ///<summary>
        ///TD344 page-break-after: always syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void PageBreakAfterTest_Node()
        {
            const string file = "PageBreakAfterTest";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "entry";
            _validate.ClassProperty.Add("fo:break-after", "page");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD307 Open Office: border-style, border-color, border-width
        /// <summary>
        /// </summary>      
        [Test]
        public void BorderTest_Node()
        {

            const string file = "Border";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "border";
            _validate.ClassProperty.Add("fo:border-left", "solid 3pt #ff0000");
            _validate.ClassProperty.Add("fo:border-right", "solid 3pt #ff0000");
            _validate.ClassProperty.Add("fo:border-top", "solid 0 #ff0000");
            _validate.ClassProperty.Add("fo:border-bottom", "solid 0 #ff0000");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);
        }


        ///<summary>
        ///TD350 position:relative;
        /// <summary>
        /// </summary>      
        [Test]
        public void PositionTest_Node()
        {
            const string file = "Position";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "positionLeft";
            _validate.ClassProperty.Add("fo:margin-left", "50pt");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "positionRight";
            _validate.ClassProperty.Add("fo:margin-left", "-20pt");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

        }

        ///<summary>
        ///TD-407 Unit Conversions in Map Property;
        /// <summary>
        /// </summary>      
        [Test]
        public void UnitConversionTest_Node()
        {
            const string file = "UnitConversion";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "unit";
            _validate.ClassProperty.Add("fo:margin-left", "9pt");
            _validate.ClassProperty.Add("fo:margin-right", "9pt");
            _validate.ClassProperty.Add("fo:margin-top", "9pt");
            _validate.ClassProperty.Add("fo:margin-bottom", "9pt");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("fo:font-size", "24pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "24pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

        }

        /////<summary>
        /////TD-425 Impliment Start and Last References in same page
        ///// <summary>
        ///// </summary>      
        //[Test]

        //public void SinglePageRefTest_Node()
        //{

        //    const string file = "SinglePageRef";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    // Note - single node test
        //    _validate = new ValidateXMLFile(output);
        //    _validate.ClassName = "AllHeaderPageLeft";
        //    _validate.ClassProperty.Add("fo:font-weight", "700");

        //    returnValue = _validate.ValidateNodeAttributesNS(false);
        //    Assert.IsTrue(returnValue);

        //    _validate = new ValidateXMLFile(output);
        //    _validate.ClassName = "AllHeaderPageRight";
        //    _validate.ClassProperty.Add("fo:font-weight", "700");

        //    returnValue = _validate.ValidateNodeAttributesNS(false);
        //    Assert.IsTrue(returnValue);

        //    //style:master-page style:name="First_20_Page"
        //    //Third Node
        //    //XPath = "//style:style[@style:name='" + ClassName + "']";
        //    //string xpath = "//style:master-page[style:name=\"First_20_Page\"]";
        //    string xpath = "//style:master-page[@style:name='XHTML']";
        //    _validate.ClassName = string.Empty;
        //    string inner =
        //        //"<text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">" +
        //        "<style:header xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\">" +
        //        "<text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">" +
        //        "<text:span text:style-name=\"AllHeaderPageLeft\">" +
        //        "<text:chapter text:display=\"name\" text:outline-level=\"9\" />" +
        //        "</text:span>" +
        //        "<text:tab />" +
        //        "<text:span text:style-name=\"AllHeaderPageNumber\">" +
        //        "<text:page-number text:select-page=\"current\">4</text:page-number>" +
        //        "</text:span>" +
        //        "<text:tab />" +
        //        "<text:span text:style-name=\"AllHeaderPageRight\">" +
        //        "<text:chapter text:display=\"name\" text:outline-level=\"10\" />" +
        //        "</text:span>" +
        //        "</text:p>" +
        //        "</style:header>";

        //    returnValue = _validate.ValidateNodeInnerXml(xpath, inner);
        //    Assert.IsTrue(returnValue);

        //}

        /////<summary>
        /////TD-428 Impliment Start and Last References in Mirror page
        ///// <summary>
        ///// </summary>      
        //[Test]
        //public void MirroredPageRefTest_Node()
        //{
        //    const string file = "MirroredPageRef";

        //    string input = FileInput(file + ".css");
        //    string output = FileOutput(file + "styles.xml");
        //    _stylesXML.CreateStyles(input, output, _errorFile, true);

        //    //First Node
        //    string xpath = "//style:page-layout[@style:name='";
        //    _validate = new ValidateXMLFile(output);
        //    _validate.ClassName = "pm1";
        //    _validate.ClassProperty.Add("style:page-usage", "mirrored");

        //    returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
        //    Assert.IsTrue(returnValue);

        //    xpath = "samenode";
        //    _validate.ClassProperty.Add("fo:page-width", "14.8cm");
        //    _validate.ClassProperty.Add("fo:page-height", "21cm");
        //    _validate.ClassProperty.Add("style:num-format", "1");
        //    _validate.ClassProperty.Add("style:print-orientation", "portrait");
        //    _validate.ClassProperty.Add("style:writing-mode", "lr-tb");
        //    _validate.ClassProperty.Add("style:footnote-max-height", "0in");

        //    _validate.ClassProperty.Add("fo:margin-top", "1.15cm");
        //    _validate.ClassProperty.Add("fo:margin-right", "1.5cm");
        //    _validate.ClassProperty.Add("fo:margin-bottom", "1.5cm");
        //    _validate.ClassProperty.Add("fo:margin-left", "1.5cm");

        //    returnValue = _validate.ValidateNodeAttributesNS(1, xpath); // style:page-layout-properties
        //    Assert.IsTrue(returnValue);

        //    //Second Node
        //    xpath = "//style:page-layout[@style:name='";
        //    _validate.ClassName = "pm2";
        //    _validate.ClassProperty.Add("style:page-usage", "mirrored");

        //    returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
        //    Assert.IsTrue(returnValue);

        //    xpath = "samenode";
        //    _validate.ClassProperty.Add("fo:page-width", "14.8cm");
        //    _validate.ClassProperty.Add("fo:page-height", "21cm");
        //    _validate.ClassProperty.Add("style:num-format", "1");
        //    _validate.ClassProperty.Add("style:print-orientation", "portrait");
        //    _validate.ClassProperty.Add("style:writing-mode", "lr-tb");
        //    _validate.ClassProperty.Add("style:footnote-max-height", "0in");

        //    _validate.ClassProperty.Add("fo:margin-top", "1.15cm");
        //    _validate.ClassProperty.Add("fo:margin-right", "1.5cm");
        //    _validate.ClassProperty.Add("fo:margin-bottom", "1.5cm");
        //    _validate.ClassProperty.Add("fo:margin-left", "1.5cm");

        //    returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
        //    Assert.IsTrue(returnValue);

        //    //Third Node
        //    xpath = "//style:header-left";
        //    _validate.ClassName = string.Empty;
        //    string inner = "<text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">" +
        //    "<text:span text:style-name=\"AllHeaderPageLeft\">" +
        //    "<text:chapter text:display=\"name\" text:outline-level=\"9\" />" +
        //    "</text:span>" +
        //    "<text:tab />" +
        //    "<text:span text:style-name=\"AllHeaderPageNumber\">" +
        //    "<text:page-number text:select-page=\"current\">4</text:page-number>" +
        //    "</text:span>" +
        //    "<text:tab />" +
        //    "<text:span text:style-name=\"AllHeaderPageRight\" />" +
        //    "</text:p>";
        //    returnValue = _validate.ValidateNodeInnerXml(xpath, inner);
        //    Assert.IsTrue(returnValue);

        //    //Fourth Node
        //    xpath = "//style:master-page[@style:name='";
        //    _validate.ClassName = "First_20_Page";

        //    _validate.ClassProperty.Add("style:display-name", "First Page");
        //    _validate.ClassProperty.Add("style:page-layout-name", "pm2");
        //    _validate.ClassProperty.Add("style:next-style-name", "Standard");

        //    returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
        //    Assert.IsTrue(returnValue);
        //}

        ///<summary>
        ///TD-245 Handle hyphenation related keywords
        /// <summary>
        /// </summary>      
        [Test]
        public void HyphenationKeywordsTest_Node()
        {

            const string file = "HyphenationKeywords";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "scriptureText.div";
            _validate.ClassProperty.Add("fo:hyphenation-ladder-count", "1");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

            _validate.ClassProperty.Add("fo:hyphenate", "true");
            _validate.ClassProperty.Add("fo:hyphenation-push-char-count", "2");
            _validate.ClassProperty.Add("fo:hyphenation-remain-char-count", "3");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD-46  - Open Office font-family: Gentium
        /// <summary>
        /// 
        /// </summary>      
        //Note : This is already in Node check
        [Test]
        public void GentiumFont()
        {
            const string file = "GentiumFont";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            var xd = new XmlDocument();
            xd.Load(output);
            XmlNodeList nl = xd.GetElementsByTagName("office:styles");
            foreach (XmlNode n in nl[0])
            {
                if (n.Attributes.GetNamedItem("style:name").Value == "Gentium")
                {
                    string resultFont = n.FirstChild.Attributes.GetNamedItem("fo:font-family").Value;
                    var fonts = new InstalledFontCollection();
                    foreach (FontFamily family in fonts.Families)
                    {
                        if (family.Name == "Gentium")
                        {
                            Assert.AreEqual(resultFont, "Gentium", "Installed Gentium not recognized");
                            return;
                        }
                    }
                    Assert.AreEqual(resultFont, "Times New Roman", "Times New Roman not a recognized substitute for Gentium");
                    return;
                }
            }
            Assert.Fail("Gentium style missing");
            //string expected = FileExpected(file + "styles.xml");
            //XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        }

        ///<summary>
        ///TD-432 Charis SIL Font
        /// <summary>
        /// </summary>      
        [Test]
        public void CharisSILFont_Node()
        {
            const string file = "CharisSILFont";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "CharisSIL";
            _validate.ClassProperty.Add("fo:font-family", "Charis SIL");
            _validate.ClassProperty.Add("fo:font-name-complex", "Charis SIL");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD-59 (Open Office font-size: larger;) -  Doulos SIL Font
        /// <summary>
        /// </summary>      
        [Test]
        public void DoulosSILFont_Node()
        {
            const string file = "DoulosSILFont";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "DoulosSIL";
            _validate.ClassProperty.Add("fo:font-family", "Doulos SIL");
            _validate.ClassProperty.Add("fo:font-name-complex", "Doulos SIL");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }


        ///<summary>
        ///TD-461 (Open Office fixed-line-height: 14pt;)
        ///</summary>      
        [Test]
        public void FixedLineHeightTest_Node()
        {
            const string file = "fixed-line-height";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "entry";
            _validate.ClassProperty.Add("fo:line-height", "14pt");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

        }


        ///<summary>
        ///TD-663 Space between header and text  
        /// <summary>
        /// </summary>      
        [Test]
        public void HeaderSpace_Node()
        {
            const string file = "HeaderSpace";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true, true);

            //First Node
            string xpath = "//style:page-layout[@style:name='pm2']";
            _validate = new ValidateXMLFile(output);
            _validate.ClassName = string.Empty;
            string inner = "<style:page-layout-properties fo:page-width=\"8.5in\" fo:page-height=\"11in\" style:num-format=\"1\" style:print-orientation=\"portrait\" fo:margin-top=\"0.7874in\" fo:margin-right=\"0.7874in\" fo:margin-bottom=\"0.7874in\" fo:margin-left=\"0.7874in\" style:writing-mode=\"lr-tb\" style:footnote-max-height=\"0in\" fo:padding-top=\"72pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:background-image /></style:page-layout-properties><style:header-style xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header-footer-properties fo:margin-bottom=\"106.3464pt\" fo:min-height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header-footer-properties fo:margin-bottom=\"106.3464pt\" fo:min-height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style>";

            returnValue = _validate.ValidateNodeInnerXml(xpath, inner);
            Assert.IsTrue(returnValue);

            //second Node
            xpath = "//style:page-layout[@style:name='pm3']";
            _validate = new ValidateXMLFile(output);
            _validate.ClassName = string.Empty;
            inner = "<style:page-layout-properties fo:page-width=\"8.5in\" fo:page-height=\"11in\" style:num-format=\"1\" style:print-orientation=\"portrait\" fo:margin-top=\"0.7874in\" fo:margin-right=\"0.7874in\" fo:margin-bottom=\"0.7874in\" fo:margin-left=\"0.7874in\" style:writing-mode=\"lr-tb\" style:footnote-max-height=\"0in\" fo:padding-top=\"72pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:background-image /><style:footnote-sep style:width=\"0.0071in\" style:distance-before-sep=\"0.0398in\" style:distance-after-sep=\"0.0398in\" style:adjustment=\"left\" style:rel-width=\"25%\" style:color=\"#000000\" /></style:page-layout-properties><style:header-style xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header-footer-properties fo:margin-bottom=\"106.3464pt\" fo:min-height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header-footer-properties fo:margin-bottom=\"106.3464pt\" fo:min-height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style>";

            returnValue = _validate.ValidateNodeInnerXml(xpath, inner);
            Assert.IsTrue(returnValue);
        }

        #endregion
    }
}