// --------------------------------------------------------------------------------------------
// <copyright file="StylesXMLTest.cs" from='2009' to='2014' company='SIL International'>
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
// Test Cases for Stylesxml
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections.Generic;
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
    public class LOStylesXMLTest
    {
        #region Private Variables
        //LOStyles _stylesXML;
        private string _inputPath;
        private string _outputPath;
        private ValidateXMLFile _validate;
        private bool returnValue;
        private PublicationInformation projInfo = new PublicationInformation();
        #endregion Private Variables

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            //_stylesXML = new LOStyles();
            Common.Testing = true;
            returnValue = false;
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/OpenOfficeConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "input");
            _outputPath = Common.PathCombine(testPath, "output");
            Common.PathCombine(testPath, "expected");
            Common.SupportFolder = "";
            Common.ProgInstall = Common.DirectoryPathReplace(Environment.CurrentDirectory + "/../../../../DistFiles");
            projInfo.ProjectInputType = "Dictionary";

	        Common.UseAfterBeforeProcess = true;
        }
        #endregion Setup

        #region Private Functions
        private string FileInput(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }

        private string FileOutput(string fileName)
        {
            return Common.PathCombine(_outputPath, fileName);
        }
        #endregion Private Functions

        //#region File Comparision
        ///<summary>
        ///TD100 text-transform syntax in Styles.xml
        /// 
        /// </summary>      
        [Test]
        public void TextTransformTest_Node()
        {
            const string file = "TD100";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

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

	    [Test]
	    [Category("SkipOnTeamCity")]
	    public void ApplySpanFontStyleTest()
	    {
			const string file = "ApplySpanFontStyleTest";
			string input = FileInput(file + ".css");
			string output = FileOutput(file + "styles.xml");
			GetCssClass(input, output);

			string xpath = "//style:style[@style:name='span_.bzh']";
			_validate = new ValidateXMLFile(output);
			_validate.ClassName = string.Empty;
			_validate.ClassProperty.Add("fo:font-family", "Charis SIL");

			returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
			Assert.IsTrue(returnValue);
	    }

        ///<summary>
        ///TD55 font-Weigth: 700 syntax in Styles.xml

        /// </summary>      
        [Test]
        public void TextFontWeightTestA_Node()
        {
            const string file = "TextFontWeightTestA";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-weight", "400");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "Font-Weight-Test-A syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD55 font-Weigth: 700 syntax in Styles.xml
        /// </summary>      
        [Test]
        public void TextFontWeightTestB_Node()
        {
            const string file = "TextFontWeightTestB";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-weight", "700");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "Font-Weight-Test-B syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD37 font-Weigtht: bold syntax in Styles.xml

        /// </summary>      
        [Test]
        public void TextFontWeightTestC_Node()
        {
            const string file = "TextFontWeightTestC";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-weight", "700");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "Font-Weight-Test-C syntax failed in Styles.xml");
        }
        ///<summary>
        ///TD50 font-Weigtht: normal syntax in Styles.xml
        /// </summary>      
        [Test]
        public void TextFontWeightTestD_Node()
        {
            const string file = "TextFontWeightTestD";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-weight", "700");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "Font-Weight-Test-D syntax failed in Styles.xml");
        }

        ///<summary>
        ///TD53 font-Weigtht: inherit; syntax in Styles.xml

        /// </summary>      
        [Test]
        public void TextFontWeightTestE_Node()
        {
            const string file = "TextFontWeightTestE";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

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
        ///TD-662 (Rudimentary table formatting)
        /// </summary>      
        [Test]
        public void TableProperty()
        {
            const string file = "TableProperty";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "thstyle";
            _validate.ClassProperty.Add("fo:color", "#0000ff");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue, "Table property failed in Styles.xml");
        }

        ///<summary>
        ///TD42 font-style: normal; syntax in Styles.xml

        /// </summary>      
        [Test]
        public void TextFontStyleTestA_Node()
        {
            const string file = "TextFontStyleTestA";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-style", "normal");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD43 font-style: inherit; syntax in Styles.xml
        /// </summary>      
        [Test]
        public void TextFontStyleTestB_Node()
        {
            const string file = "TextFontStyleTestB";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

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

        /// </summary>      
        [Test]
        [Ignore("India team has to figure out why this fails")]
        public void FontFamily_Node()
        {
            const string file = "FontFamily";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

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
            Dictionary<string, Dictionary<string, string>> cssClass = GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:text-align", "right");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

        }

        private Dictionary<string, Dictionary<string, string>> GetCssClass(string input, string output)
        {
            projInfo.DefaultCssFileWithPath = input;
            projInfo.TempOutputFolder = _outputPath;

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);

            Dictionary<string, Dictionary<string, string>> idAllClass = new Dictionary<string, Dictionary<string, string>>();
            LOStyles ooStyles = new LOStyles();

            idAllClass = ooStyles.CreateStyles(projInfo, cssClass, output);
            return cssClass;
        }


        [Test]
        public void TextAlignTestB_Node()
        {
            const string file = "TextAlignTestB";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:text-align", "justify");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

        }

        ///<summary>
        ///TD60 font-size: 3cm; syntax in Styles.xml
        /// </summary>      
        [Test]
        public void TextFontSizeTestA_Node()
        {

            const string file = "TextFontSizeTestA";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-size", "144pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "144pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "letter1";
            _validate.ClassProperty.Add("fo:font-size", "141.7323pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "141.7323pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

        }

        ///<summary>
        ///TD58 font-size: 1.5em; syntax in Styles.xml

        /// </summary>      
        [Test]
        public void TextFontSizeTestB_Node()
        {
            const string file = "TextFontSizeTestB";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "letter";
            _validate.ClassProperty.Add("fo:font-size", "12pt");
            _validate.ClassProperty.Add("fo:font-size-complex", "12pt");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "letter1";
            _validate.ClassProperty.Add("fo:font-size", "150%");
            _validate.ClassProperty.Add("fo:font-size-complex", "150%");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "letter2";
            _validate.ClassProperty.Add("fo:font-size", "150%");
            _validate.ClassProperty.Add("fo:font-size-complex", "150%");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD57 font-size: xx-small; syntax in Styles.xml

        /// </summary>      
        [Test]
        public void TextFontSizeTestC_Node()
        {
            const string file = "TextFontSizeTestC";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

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
        /// </summary>      
        [Test]
        public void TextFontSizeTestD_Node()
        {
            const string file = "TextFontSizeTestD";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

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
        /*

                ///<summary>
                ///TD70 padding-bottom: 9pt; syntax in Styles.xml
                /// </summary>     
                ///
                [Ignore]
                [Test]
                public void PaddingBottomTest_Node()
                {
                    const string file = "PaddingBottomTest";

                    string input = FileInput(file + ".css");
                    string output = FileOutput(file + "styles.xml");
                    _stylesXML.CreateStyles(input, output, _errorFile, true);

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
        */
        ///<summary>
        ///TD81 page-break-before: always syntax in Styles.xml

        /// </summary>      
        [Test]
        public void PageBreakBeforeTest_Node()
        {
            const string file = "PageBreakBeforeTest";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "entry";
            _validate.ClassProperty.Add("fo:break-before", "page");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD63 font-variant: normal; syntax in Styles.xml

        /// </summary>      
        [Test]
        public void TextFontVariantTestA_Node()
        {
            const string file = "TextFontVariantTestA";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

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

        /// </summary>      
        [Test]
        public void FontWeightBolder_Node()
        {
            const string file = "FontWeightBolder";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "a";
            _validate.ClassProperty.Add("fo:font-weight", "700");
            _validate.ClassProperty.Add("fo:font-weight-complex", "700");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "b";
            _validate.ClassProperty.Add("fo:font-weight", "400");
            _validate.ClassProperty.Add("fo:font-weight-complex", "400");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "c";
            _validate.ClassProperty.Add("fo:font-weight", "700");
            _validate.ClassProperty.Add("fo:font-weight-complex", "700");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD-64 font: 80% san-serif; in Styles.xml

        /// </summary>      
        [Test]
        [Ignore("India team has to figure out why this fails")]
        public void Font_Node()
        {
            const string file = "Font";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

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
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "xhomographnumber";
            _validate.ClassProperty.Add("style:text-position", "sub 55%");
            _validate.ClassProperty.Add("fo:font-size", "100%");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate.ClassName = "xhomographnumbersuper";
            _validate.ClassProperty.Add("style:text-position", "super 55%");
            _validate.ClassProperty.Add("fo:font-size", "100%");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

		[Test]
		public void TextDecorationLineThroughTest()
		{
			const string file = "TextDecorationLineThroughTest";

			string input = FileInput(file + ".css");
			string output = FileOutput(file + "styles.xml");
			GetCssClass(input, output);

			_validate = new ValidateXMLFile(output);
			_validate.ClassName = "span_.en_definitionorgloss.-senses.-entry";
			_validate.ClassProperty.Add("style:text-line-through-style", "solid");
			_validate.ClassProperty.Add("style:text-line-through-type", "single");

			returnValue = _validate.ValidateNodeAttributesNS(false);
			Assert.IsTrue(returnValue);
		}


        [Test]
        public void Padding_Node()
        {
            const string file = "Padding";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "entry";
            _validate.ClassProperty.Add("fo:border-left", "0.5pt solid #ffffff");
            _validate.ClassProperty.Add("fo:border-right", "0.5pt solid #ffffff");
            _validate.ClassProperty.Add("fo:border-top", "0.5pt solid #ffffff");
            _validate.ClassProperty.Add("fo:border-bottom", "0.5pt solid #ffffff");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD-270 (Direction:ltr)

        /// </summary>      
        [Test]
        public void DirectionTest_Node()
        {
            const string file = "Direction";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

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

        /// </summary>      
        [Test]
        public void WidowsandOrphans_Node()
        {
            const string file = "WidowsandOrphans";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "orphans";
            _validate.ClassProperty.Add("fo:orphans", "2");
            _validate.ClassProperty.Add("fo:widows", "2");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);
        }


        ///<summary>
        ///TD344 page-break-after: always syntax in Styles.xml

        /// </summary>      
        [Test]
        public void PageBreakAfterTest_Node()
        {
            const string file = "PageBreakAfterTest";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "entry";
            _validate.ClassProperty.Add("fo:break-after", "page");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD307 Open Office: border-style, border-color, border-width

        /// </summary>      
        [Test]
        public void BorderTest_Node()
        {

            const string file = "Border";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "border";
            _validate.ClassProperty.Add("fo:border-left", "solid 3pt #ff0000");
            _validate.ClassProperty.Add("fo:border-right", "solid 3pt #ff0000");
            _validate.ClassProperty.Add("fo:border-top", "solid 0pt #ff0000");
            _validate.ClassProperty.Add("fo:border-bottom", "solid 0pt #ff0000");

            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);
        }


        ///<summary>
        ///TD350 position:relative;

        /// </summary>      
        [Test]
        public void PositionTest_Node()
        {
            const string file = "Position";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

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

        /// </summary>      
        [Test]
        public void UnitConversionTest_Node()
        {
            const string file = "UnitConversion";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

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

        ///<summary>
        ///TD-425 Impliment Start and Last References in same page

        /// </summary>
        [Ignore]      
        [Test]
        public void SinglePageRefTest_Node()
        {

            const string file = "SinglePageRef";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            // Note - single node test
            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "PageHeaderFooter12";
            _validate.ClassProperty.Add("fo:font-weight", "700");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "PageHeaderFooter14";
            _validate.ClassProperty.Add("fo:font-weight", "700");

            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);

            //style:master-page style:name="First_20_Page"
            //Third Node
            //XPath = "//style:style[@style:name='" + ClassName + "']";
            //string xpath = "//style:master-page[style:name=\"First_20_Page\"]";
            string xpath = "//style:master-page[@style:name='Left_20_Page']";
            _validate.ClassName = string.Empty;
            string inner =
                //"<text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">" +
                "<style:header xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\">" +
                "<text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">" +
                "<text:span text:style-name=\"PageHeaderFooter12\">" +
                "<text:chapter text:display=\"name\" text:outline-level=\"9\" />" +
                "</text:span>" +
                "<text:tab />" +
                "<text:span text:style-name=\"PageHeaderFooter13\">" +
                "<text:page-number text:select-page=\"current\">4</text:page-number>" +
                "</text:span>" +
                "<text:tab />" +
                "<text:span text:style-name=\"PageHeaderFooter14\">" +
                "<text:chapter text:display=\"name\" text:outline-level=\"10\" />" +
                "</text:span>" +
                "</text:p>" +
                "</style:header>";

            returnValue = _validate.ValidateNodeInnerXml(xpath, inner);
            Assert.IsTrue(returnValue);

            xpath = "//style:master-page[@style:name='Right_20_Page']";
            _validate.ClassName = string.Empty;
            inner =
                //"<text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">" +
                "<style:header xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\">" +
                "<text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">" +
                "<text:span text:style-name=\"PageHeaderFooter18\">" +
                "<text:chapter text:display=\"name\" text:outline-level=\"9\" />" +
                "</text:span>" +
                "<text:tab />" +
                "<text:span text:style-name=\"PageHeaderFooter19\">" +
                "<text:page-number text:select-page=\"current\">4</text:page-number>" +
                "</text:span>" +
                "<text:tab />" +
                "<text:span text:style-name=\"PageHeaderFooter20\">" +
                "<text:chapter text:display=\"name\" text:outline-level=\"10\" />" +
                "</text:span>" +
                "</text:p>" +
                "</style:header>";

            returnValue = _validate.ValidateNodeInnerXml(xpath, inner);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD-428 Impliment Start and Last References in Mirror page

        /// </summary>      
        [Test]
        [Ignore]
        public void MirroredPageRefTest_Node()
        {
            const string file = "MirroredPageRef";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            //First Node
            string xpath = "//style:page-layout[@style:name='";
            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "pm1";
            _validate.ClassProperty.Add("style:page-usage", "mirrored");

            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            xpath = "samenode";
            _validate.ClassProperty.Add("fo:page-width", "14.8cm");
            _validate.ClassProperty.Add("fo:page-height", "21cm");
            _validate.ClassProperty.Add("style:num-format", "1");
            _validate.ClassProperty.Add("style:print-orientation", "portrait");
            _validate.ClassProperty.Add("style:writing-mode", "lr-tb");
            _validate.ClassProperty.Add("style:footnote-max-height", "0in");

            _validate.ClassProperty.Add("fo:margin-top", "1.15cm");
            _validate.ClassProperty.Add("fo:margin-right", "1.5cm");
            _validate.ClassProperty.Add("fo:margin-bottom", "1.5cm");
            _validate.ClassProperty.Add("fo:margin-left", "1.5cm");

            returnValue = _validate.ValidateNodeAttributesNS(1, xpath); // style:page-layout-properties
            Assert.IsTrue(returnValue);

            //Second Node
            xpath = "//style:page-layout[@style:name='";
            _validate.ClassName = "pm2";
            _validate.ClassProperty.Add("style:page-usage", "mirrored");

            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);

            xpath = "samenode";
            _validate.ClassProperty.Add("fo:page-width", "14.8cm");
            _validate.ClassProperty.Add("fo:page-height", "21cm");
            _validate.ClassProperty.Add("style:num-format", "1");
            _validate.ClassProperty.Add("style:print-orientation", "portrait");
            _validate.ClassProperty.Add("style:writing-mode", "lr-tb");
            _validate.ClassProperty.Add("style:footnote-max-height", "0in");

            _validate.ClassProperty.Add("fo:margin-top", "1.15cm");
            _validate.ClassProperty.Add("fo:margin-right", "1.5cm");
            _validate.ClassProperty.Add("fo:margin-bottom", "1.5cm");
            _validate.ClassProperty.Add("fo:margin-left", "1.5cm");

            returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
            Assert.IsTrue(returnValue);

            //Third Node
            xpath = "//style:header-left";
            _validate.ClassName = string.Empty;
            string inner = "<text:p text:style-name=\"Header\" xmlns:text=\"urn:oasis:names:tc:opendocument:xmlns:text:1.0\">" +
            "<text:span text:style-name=\"AllHeaderPageLeft\">" +
            "<text:chapter text:display=\"name\" text:outline-level=\"9\" />" +
            "</text:span>" +
            "<text:tab />" +
            "<text:span text:style-name=\"AllHeaderPageNumber\">" +
            "<text:page-number text:select-page=\"current\">4</text:page-number>" +
            "</text:span>" +
            "<text:tab />" +
            "<text:span text:style-name=\"AllHeaderPageRight\" />" +
            "</text:p>";
            returnValue = _validate.ValidateNodeInnerXml(xpath, inner);
            Assert.IsTrue(returnValue);

            //Fourth Node
            xpath = "//style:master-page[@style:name='";
            _validate.ClassName = "First_20_Page";

            _validate.ClassProperty.Add("style:display-name", "First Page");
            _validate.ClassProperty.Add("style:page-layout-name", "pm2");
            _validate.ClassProperty.Add("style:next-style-name", "Standard");

            returnValue = _validate.ValidateNodeAttributesNS(0, xpath);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD-245 Handle hyphenation related keywords

        /// </summary>      
        [Test]
        public void HyphenationKeywordsTest_Node()
        {

            const string file = "HyphenationKeywords";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "div.scriptureText";
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

        /// 
        /// </summary>      
        //Note : This is already in Node check
        [Test]
        public void GentiumFont()
        {
            const string file = "GentiumFont";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

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

        /// </summary>      
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void CharisSILFont_Node()
        {
            const string file = "CharisSILFont";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "CharisSIL";
            _validate.ClassProperty.Add("fo:font-family", "Charis SIL");
            _validate.ClassProperty.Add("fo:font-name-complex", "Charis SIL");
            returnValue = _validate.ValidateNodeAttributesNS(false);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD-59 (Open Office font-size: larger;) -  Doulos SIL Font

        /// </summary>      
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void DoulosSILFont_Node()
        {
            const string file = "DoulosSILFont";

            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

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
            GetCssClass(input, output);

            _validate = new ValidateXMLFile(output);
            _validate.ClassName = "entry";
            _validate.ClassProperty.Add("fo:line-height", "14pt");
            returnValue = _validate.ValidateNodeAttributesNS(true);
            Assert.IsTrue(returnValue);

        }


        ///<summary>
        ///TD-663 Space between header and text  

        /// </summary> 
        [Ignore]     
        [Test]
        public void HeaderSpace_Node()
        {
            const string file = "HeaderSpace";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            //First Node
            string xpath = "//style:page-layout[@style:name='pm3']";
            _validate = new ValidateXMLFile(output);
            _validate.ClassName = string.Empty;
            _validate.ClassProperty.Add("fo:padding-top=","72pt");
            //string content = "<style:page-layout-properties fo:page-width=\"8.5in\" fo:page-height=\"11in\" style:num-format=\"1\" style:print-orientation=\"portrait\" fo:margin-top=\"0.7874in\" fo:margin-right=\"0.7874in\" fo:margin-bottom=\"0.7874in\" fo:margin-left=\"0.7874in\" style:writing-mode=\"lr-tb\" style:footnote-max-height=\"0in\" fo:padding-top=\"72pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:background-image /></style:page-layout-properties><style:header-style xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header-footer-properties fo:margin-bottom=\"106.3464pt\" fo:min-height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header-footer-properties fo:margin-bottom=\"106.3464pt\" fo:min-height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style>";

            returnValue = _validate.ValidateNodeAttributesNS(1,xpath);
            Assert.IsTrue(returnValue);

            //second Node
            xpath = "//style:page-layout[@style:name='pm4']";
            _validate = new ValidateXMLFile(output);
            _validate.ClassName = string.Empty;
            _validate.ClassProperty.Add("fo:padding-top=", "72pt");
            //string content = "<style:page-layout-properties fo:page-width=\"8.5in\" fo:page-height=\"11in\" style:num-format=\"1\" style:print-orientation=\"portrait\" fo:margin-top=\"0.7874in\" fo:margin-right=\"0.7874in\" fo:margin-bottom=\"0.7874in\" fo:margin-left=\"0.7874in\" style:writing-mode=\"lr-tb\" style:footnote-max-height=\"0in\" fo:padding-top=\"72pt\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:background-image /></style:page-layout-properties><style:header-style xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header-footer-properties fo:margin-bottom=\"106.3464pt\" fo:min-height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:header-style><style:footer-style xmlns:style=\"urn:oasis:names:tc:opendocument:xmlns:style:1.0\"><style:header-footer-properties fo:margin-bottom=\"106.3464pt\" fo:min-height=\"14.21pt\" fo:margin-left=\"0pt\" fo:margin-right=\"0pt\" style:dynamic-spacing=\"false\" xmlns:fo=\"urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0\" /></style:footer-style>";

            returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
            Assert.IsTrue(returnValue);
        }

        ///<summary>
        ///TD-3807 Page ends with Parallel reference

        /// Add Keep with next paragraph property for parallel reference class
        /// </summary> 
        [Test]
        public void ParallerReferenceKeepWith_Node()
        {
            const string file = "ParallerReferenceKeepWith";
            string input = FileInput(file + ".css");
            string output = FileOutput(file + "styles.xml");
            GetCssClass(input, output);

            string xpath = "//style:style[@style:name='ParallelPassageReference']";
            _validate = new ValidateXMLFile(output);
            _validate.ClassName = string.Empty;
            _validate.ClassProperty.Add("fo:keep-with-next", "always");

            returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
            Assert.IsTrue(returnValue);
        }

		///<summary>
		/// When SubEntry have same/less than Entry's margin-left, calculate part add entry value to SubEntry to maintain indent.
		/// </summary>
		[Test]
		public void EntrySubEntrySameMarginLeft_Node()
		{
			const string file = "EntrySubEntryMarginLeft";
			string input = FileInput(file + ".css");
			string output = FileOutput(file + "styles.xml");
			GetCssClass(input, output);

			string xpath = "//style:style[@style:name='subentry']";
			_validate = new ValidateXMLFile(output);
			_validate.ClassName = string.Empty;
			_validate.ClassProperty.Add("fo:margin-left", "12pt");

			returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
			Assert.IsTrue(returnValue);
		}

		/// <summary>
		/// When header choice is "Same as body", it get the entry's font-size
		/// </summary>
		[Test]
	    public void DictionaryHeaderSameAsBody()
	    {
			const string file = "DictionaryHeaderSameAsBody";
			string input = FileInput(file + ".css");
			string output = FileOutput(file + "styles.xml");
			GetCssClass(input, output);

			string xpath = "//style:style[@style:name='Header']/style:text-properties";
			_validate = new ValidateXMLFile(output);
			_validate.ClassName = string.Empty;
			_validate.ClassProperty.Add("fo:font-size", "10pt");

			returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
			Assert.IsTrue(returnValue);
	    }

		/// <summary>
		///  When header choice is "Same as body", it get the Paragraph's font-size
		/// </summary>
		[Test]
		public void ScriptureHeaderSameAsBody()
		{
			const string file = "ScriptureHeaderSameAsBody";
			string input = FileInput(file + ".css");
			string output = FileOutput(file + "styles.xml");
			projInfo.ProjectInputType = "Scripture";
			GetCssClass(input, output);

			string xpath = "//style:style[@style:name='Header']/style:text-properties";
			_validate = new ValidateXMLFile(output);
			_validate.ClassName = string.Empty;
			_validate.ClassProperty.Add("fo:font-size", "18pt");

			returnValue = _validate.ValidateNodeAttributesNS(1, xpath);
			Assert.IsTrue(returnValue);

		}
    }
}