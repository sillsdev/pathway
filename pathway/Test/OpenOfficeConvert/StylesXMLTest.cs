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
        #endregion Private Variables

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _stylesXML = new StylesXML();
            _errorFile = Common.PathCombine(Path.GetTempPath(), "temp.odt");
            Common.Testing = true;
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/OpenOfficeConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "expected");
            Common.SupportFolder = "";
            Common.ProgInstall = Common.DirectoryPathReplace(Environment.CurrentDirectory + "/../../../PsSupport");
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
            _stylesXML.CreateStyles(input, output, _errorFile, true);

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

        #region Public Functions
        /// <summary>
        /// TD86 .xitem[lang='en'] syntax in Styles.xml
        /// </summary>
        [Test]
        public void LanguageTest()
        {
            FileTest("LanguageTest");
        }

        [Test]
        public void LanguageTestNegative()
        {
            _outputName = "StylesNeg.xml";
            FileTest("LanguageTest");
        }

        ///<summary>
        ///TD96 text-indent syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Ignore]
        public void TextIndentTest()
        {
            _errorMessage = "text-indent syntax failed in Styles.xml";
            FileTest("TextIndentTest");
        }

        ///<summary>
        ///TD100 text-transform syntax in Styles.xml
        /// 
        /// </summary>      
        [Test]
        public void TextTransformTest()
        {
            string input = FileInput("TD100.css");
            string output = FileOutput("stylesTD100.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);
            string expected = FileExpected("stylesTD100.xml");
            XmlAssert.AreEqual(expected, output, "TextTransform syntax failed in Styles.xml");
        }
        /// <summary>
        /// TD-78a  Open Office color: #123abc;
        /// </summary>
        [Test]
        public void ColorTestPositive()
        {
            _errorMessage = "TD-78,Open Office color: #123abc. Syntax failed in Styles.xml";
            FileTest("ColorTestPositive");
        }
        /// <summary>
        /// TD-78a  Open Office color: #123abc;
        /// </summary>
        [Ignore]
        public void ColorTestNegative()
        {
            const string file = "ColorTestPositive";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "StylesNeg.xml");
            FileAssert.AreNotEqual(expected, output, "TD-78,Open Office color: #123abc. Syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD54 font-Weigth: 400 syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontWeightTestA()
        {
            const string file = "TextFontWeightTestA";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Font-Weight-Test-A syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD55 font-Weigth: 700 syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontWeightTestB()
        {
            const string file = "TextFontWeightTestB";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Font-Weight-Test-B syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD37 font-Weigtht: bold syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontWeightTestC()
        {
            const string file = "TextFontWeightTestC";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Font-Weight-Test-C syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD50 font-Weigtht: normal syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontWeightTestD()
        {
            const string file = "TextFontWeightTestD";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Font-Weight-Test-D syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD53 font-Weigtht: inherit; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontWeightTestE()
        {
            const string file = "TextFontWeightTestE";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);
            
            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Font-Weight-Test-E syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD42 font-style: normal; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontStyleTestA()
        {
            const string file = "TextFontStyleTestA";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);
            
            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Font-Style-A syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD43 font-style: inherit; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontStyleTestB()
        {
            const string file = "TextFontStyleTestB";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);
            
            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Font-Style-B syntax failed in Styles.xml");
        }


        ///<summary>
        ///TD49 font-family: in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void FontFamily()
        {
            const string file = "FontFamily";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);
            
            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Font-Family syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD66 text-align: right; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextAlignTestA()
        {
            const string file = "TextAlignTestA";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);
            
            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Text-Align-A syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD67 text-align: justify; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextAlignTestB()
        {
            const string file = "TextAlignTestB";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);
            
            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Text-Align-B syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD60 font-size: 3cm; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontSizeTestA()
        {
            const string file = "TextFontSizeTestA";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);
            
            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Font-Size-A syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD58 font-size: 1.5em; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontSizeTestB()
        {
            const string file = "TextFontSizeTestB";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);
            
            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Font-Size-B syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD57 font-size: xx-small; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontSizeTestC()
        {
            const string file = "TextFontSizeTestC";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);
            
            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Font-Size-C syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD61 font-size: inherit; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontSizeTestD()
        {
            const string file = "TextFontSizeTestD";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Font-Size-D syntax failed in Styles.xml");
        }


        ///<summary>
        ///TD70 padding-bottom: 9pt; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void PaddingBottomTest()
        {
            const string file = "PaddingBottomTest";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "padding-bottom syntax failed in styles.xml");
        }

        ///<summary>
        ///TD71 padding-top: 1in; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void PaddingTopTest()
        {
            const string file = "PaddingTopTest";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "padding-top syntax failed in styles.xml");
        }

        ///<summary>
        ///TD81 page-break-before: always syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void PageBreakBeforeTest()
        {
            const string file = "PageBreakBeforeTest";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output , "Page Break Before syntax failed in styles.xml");
        }

        ///<summary>
        ///TD63 font-variant: normal; syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void TextFontVariantTestA()
        {
            const string file = "TextFontVariantTestA";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "Styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "Styles.xml");
            XmlAssert.AreEqual(expected, output, "Font-Variant-A syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD51 font-weight: bolder;  in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void FontWeightBolder()
        {
            const string file = "FontWeightBolder";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "FontWeightBolder syntax failed in styles.xml");
        }
        ///<summary>
        ///TD-64 font: 80% san-serif; in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void Font()
        {
            const string file = "Font";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "Font syntax failed in styles.xml");
        }

        
        ///<summary>
        ///TD-94 Open Office content: counter(page) in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void ContentCounter()
        {
            const string file = "ContentCounter";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        }

        ///<summary>
        ///TD-161 Vertical-align  - subscript and superscript
        /// <summary>
        /// </summary>      
        [Test]
        public void VerticalAlignTest()
        {
            const string file = "VerticalAlignTest";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output,"VerticalAlignTest  failed in styles.xml");
        }

        ///<summary>
        ///TD-160 & TD-85 (content: "\2666 ";)
        /// <summary>
        /// </summary>      
        [Test]
        public void UnicodeTest()
        {
            const string file = "UnicodeTest";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output,_errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output,"UnicodeTest failed in styles.xml");
        }

        ///<summary>
        ///TD-244 (Update CSSParser to handle revised grammar)
        /// <summary>
        /// </summary>      
        //[Test]
        public void OxesCSSTest()
        {
            const string file = "Oxes";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "OxesCSSTest failed in styles.xml");
        }

        ///<summary>
        ///TD-270 (Direction:ltr)
        /// <summary>
        /// </summary>      
        [Test]
        public void DirectionTest()
        {
            const string file = "Direction";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "DirectionTest failed in styles.xml");
        }

        ///<summary>
        ///TD-305 (widows) and TD-306(orphans)
        /// <summary>
        /// </summary>      
        [Test]
        public void WidowsandOrphans()
        {
            const string file = "WidowsandOrphans";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "WidowsandOrphans failed in styles.xml");
        }

        ///<summary>
        ///TD344 page-break-after: always syntax in Styles.xml
        /// <summary>
        /// </summary>      
        [Test]
        public void PageBreakAfterTest()
        {
            const string file = "PageBreakAfterTest";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "Page Break After syntax failed in styles.xml");
        }

        ///<summary>
        ///TD307 Open Office: border-style, border-color, border-width
        /// <summary>
        /// </summary>      
        [Test]
        public void BorderTest()
        {
            const string file = "Border";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "border-style, border-color and border-width syntax failed in styles.xml");
        }

        ///<summary>
        ///TD350 position:relative;
        /// <summary>
        /// </summary>      
        [Test]
        public void PositionTest()
        {
            const string file = "Position";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "Position syntax failed in styles.xml");
        }

        ///<summary>
        ///TD-407 Unit Conversions in Map Property;
        /// <summary>
        /// </summary>      
        [Test]
        public void UnitConversionTest()
        {
            const string file = "UnitConversion";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        }

        ///<summary>
        ///TD-425 Impliment Start and Last References in same page
        /// <summary>
        /// </summary>      
        [Test]
        public void SinglePageRefTest()
        {
            const string file = "SinglePageRef";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "Single Page References failed in styles.xml");
        }

        ///<summary>
        ///TD-428 Impliment Start and Last References in Mirror page
        /// <summary>
        /// </summary>      
        [Test]
        public void MirroredPageRefTest()
        {
            const string file = "MirroredPageRef";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "Mirrored Page References syntax failed in styles.xml");
        }

        ///<summary>
        ///TD-245 Handle hyphenation related keywords
        /// <summary>
        /// </summary>      
        [Test]
        public void HyphenationKeywordsTest()
        {
            const string file = "HyphenationKeywords";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, "Hyphenation Keywords syntax failed in styles.xml");
        }

        ///<summary>
        ///TD-432 Charis SIL Font
        /// <summary>
        /// </summary>      
        [Test]
        public void CharisSILFont()
        {
            const string file = "CharisSILFont";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        }

        ///<summary>
        ///TD-46  - Open Office font-family: Gentium
        /// <summary>
        /// </summary>      
        [Test]
        public void GentiumFont()
        {
            const string file = "GentiumFont";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            var xd = new XmlDocument();
            xd.Load(output);
            XmlNodeList nl =  xd.GetElementsByTagName("office:styles");
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
        ///TD-59 (Open Office font-size: larger;) -  Doulos SIL Font
        /// <summary>
        /// </summary>      
        [Test]
        public void DoulosSILFont()
        {
            const string file = "DoulosSILFont";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        }

        ///<summary>
        ///TD-461 (Open Office fixed-line-height: 14pt;)
        ///</summary>      
        [Test]
        public void FixedLineHeightTest()
        {
            const string file = "fixed-line-height";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);
            var result = new XmlDocument {XmlResolver = null};
            result.Load(output);
            var root = result.DocumentElement;
            Assert.IsNotNull(root, file + " no xml document");
            var nsManager = new XmlNamespaceManager(result.NameTable);
            foreach (XmlAttribute attribute in root.Attributes)
            {
                var namePart = attribute.Name.Split(':');
                if (namePart[0] == "xmlns")
                    nsManager.AddNamespace(namePart[1], attribute.Value);
            }
            //var size = result.SelectSingleNode("/office:document-styles/office:styles/style:style[@style:name=\"entry_section\"]/style:paragraph-properties/@fo:line-height", nsManager);
            var styles = result.SelectNodes("/office:document-styles/office:styles/style:style", nsManager);
            Assert.IsNotNull(styles, file + " no styles");
            XmlElement style = null;
            foreach (XmlNode myStyle in styles)
            {
                if (myStyle.NodeType != XmlNodeType.Element) continue;
                style = (XmlElement) myStyle;
                var name = style.Attributes.GetNamedItem("style:name").Value;
                if (name == "entry")
                    break;
            }
            Assert.IsNotNull(style, file + " no entry style");
            var pp = (XmlElement)style.SelectSingleNode("style:paragraph-properties",nsManager);
            Assert.IsNotNull(pp, file + " no paragraph properties");
            var size = pp.GetAttribute("fo:line-height");
            Assert.IsNotNull(size, file + " no line height");
            Assert.AreEqual("14pt", size, file + " syntax failed in styles.xml");
        }

        ///<summary>
        ///TD-663 Space between header and text  
        /// <summary>
        /// </summary>      
        [Test]
        public void HeaderSpace()
        {
            const string file = "HeaderSpace";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            _stylesXML.CreateStyles(input, output, _errorFile, true);

            string expected = FileExpected(file + "styles.xml");
            XmlAssert.AreEqual(expected, output, file + " syntax failed in styles.xml");
        }
        #endregion Public Functions
    }
}