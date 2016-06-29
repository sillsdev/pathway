// --------------------------------------------------------------------------------------------
// <copyright file="GramTest.cs" from='2009' to='2014' company='SIL International'>
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
// Css Parset Test file
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.CssParserTest
{
    /// <summary>
    /// Test grammar with suite of input to exercise all common valid css constructions
    /// </summary>
    [TestFixture]
    [Category("BatchTest")]
    public class GramTest
    {
        #region Setup
        /// <summary>
        /// Path to input files
        /// </summary>
        string _inpPath;

        /// <summary>
        /// Path to expected results
        /// </summary>
        string _expPath;

        /// <summary>
        /// Path to store actual results
        /// </summary>
        string _outPath;

        /// <summary>
        /// Compute input, output and expected paths for use by all tests.
        /// </summary>
        [TestFixtureSetUp]
        protected void SetUp()
        {
            string currentFolder = PathPart.Bin(Environment.CurrentDirectory, "/CssParser/TestFiles");
            _inpPath = Common.PathCombine(currentFolder, "gramInput");
            _expPath = Common.PathCombine(currentFolder, "gramExpect");
            _outPath = Common.PathCombine(currentFolder, "gramOutput");
        }
        #endregion Setup

        #region Internal
        /// <summary>
        /// Do a single test.
        /// </summary>
        /// <param name="testName">Test name is also the folder containing the test.</param>
        /// <param name="msg">Message to display if failure occurs.</param>
        protected void OneTest(string testName, string msg)
        {
            // Create input file name for test
            var inpFileName = Common.PathCombine(_inpPath, testName + ".css");

            var ctp = new CssTreeParser();
            ctp.Parse(inpFileName);
            var strResult = ctp.StringTree();

            // Output result to disk
            var outFileName = Common.PathCombine(_outPath, testName + ".txt");
            var sw = new StreamWriter(outFileName);
            sw.Write(strResult.Replace(@"\", "/"));
            sw.Close();
            var expFileName = Common.PathCombine(_expPath, testName + ".txt");
            TextFileAssert.AreEqual(expFileName, outFileName, msg);

            if (ctp.Errors.Count != 0)
            {
                var strError = ctp.ErrorText();
                var outErrorName = Common.PathCombine(_outPath, testName + "Err.txt");
                var swe = new StreamWriter(outErrorName);
                var strGenericError = Regex.Replace(strError, @"[\\/a-zA-Z0-9\:]+src", "src");
                swe.Write(strGenericError.Replace(@"\", "/"));
                swe.Close();
                var expErrorName = Common.PathCombine(_expPath, testName + "Err.txt");
                var msgErr = msg + " in Error text";
                TextFileAssert.AreEqual(expErrorName, outErrorName, msgErr);
            }
            else
            {
                Assert.AreEqual(File.Exists(testName + "Err.txt"), false, msg + " error not generated");
            }
        }
        #endregion Internal

        #region T1
        /// <summary>
        /// Test comments at top
        /// </summary>
        [Test]
        public void CommentAtTopT1()
        {
            OneTest("T1", "Comments at top of file");
        }
        #endregion T1

        #region T2
        /// <summary>
        /// xml in comment
        /// </summary>
        [Test]
        public void XmlCommentT2()
        {
            OneTest("T2", "xml in comment");
        }
        #endregion T2

        #region T3
        /// <summary>
        /// Greter than selector
        /// </summary>
        [Test]
        public void GreaterThanSelectorT3()
        {
            OneTest("T3", "> in selector failed");
        }
        #endregion T3

        #region T4
        /// <summary>
        /// Plus selector
        /// </summary>
        [Test]
        public void PlusSelectorT4()
        {
            OneTest("T4", "+ in selector failed");
        }
        #endregion T4

        #region T5
        /// <summary>
        /// bracket selector
        /// </summary>
        [Test]
        public void BracketSelectorT5()
        {
            OneTest("T5", "[] in selector field");
        }
        #endregion T5

        #region T6
        /// <summary>
        /// Value in field
        /// </summary>
        [Test]
        public void ValueInFieldT6()
        {
            OneTest("T6", "[class=X] to detect a value in a field");
        }
        #endregion T6

        #region T6b
        /// <summary>
        /// Quoted value in field
        /// </summary>
        [Test]
        public void QuotedValueInFieldT6b()
        {
            OneTest("T6b", "[class='X'] to detect a value in a field");
        }
        #endregion T6b

        #region T7
        /// <summary>
        /// Contains selector
        /// </summary>
        [Test]
        public void ContainsSelectorT7()
        {
            OneTest("T7", "[class~='X'] detects x as an element of a space separated list");
        }
        #endregion T7

        #region T8
        /// <summary>
        /// Begins selector
        /// </summary>
        [Test]
        public void BeginsSelectorT8()
        {
            OneTest("T8", "[lang|=en] lang begins with en");
        }
        #endregion T8

        #region T8b
        /// <summary>
        /// Quoted begins selector
        /// </summary>
        [Test]
        public void QuotedBeginsSelectorT8b()
        {
            OneTest("T8b", "[lang|='en'] lang begins with en");
        }
        #endregion T8b

        #region T9
        /// <summary>
        /// double colon
        /// </summary>
        [Test]
        public void DoubleColonT9()
        {
            OneTest("T9", "class::before allows punctuation before class content");
        }
        #endregion T9

        #region T9B
        /// <summary>
        /// Colon selector
        /// </summary>
        [Test]
        public void ColonSelectorT9B()
        {
            OneTest("T9B", "class:before allows inserting content before class content");
        }
        #endregion T9B

        #region T10
        /// <summary>
        /// Colon after selector
        /// </summary>
        [Test]
        public void ColonAfterSelectorT10()
        {
            OneTest("T10", "class:after allows punctuation after class content");
        }
        #endregion T10

        #region T11
        /// <summary>
        /// Multiple class selector
        /// </summary>
        [Test]
        public void MultiClassSelectorT11()
        {
            OneTest("T11", ".class1 + .class2:after test + and :after together");
        }
        #endregion T11

        #region T12
        /// <summary>
        /// Pseudo Attributed Class selector
        /// </summary>
        [Test]
        public void PseudoAttribClassSelT12()
        {
            OneTest("T12", ".class1[class~=myVal]::after combines [] ~= and ::");
        }
        #endregion T12

        #region T12b
        /// <summary>
        /// Bracket, pseudo, not selector
        /// </summary>
        [Test]
        public void BracketPseudoNotSelT12b()
        {
            OneTest("T12b", ".class1[class~=myValue]::after combines [] ~='x' and ::");
        }
        #endregion T12b

        #region T13
        /// <summary>
        /// page selector
        /// </summary>
        [Test]
        public void PageSelT13()
        {
            OneTest("T13", "@page");
        }
        #endregion T13

        #region T14
        /// <summary>
        /// Left-Right page selector
        /// </summary>
        [Test]
        public void LeftRightPageT14()
        {
            OneTest("T14", "@page:left and @page:right");
        }
        #endregion T14

        #region T15
        /// <summary>
        /// named page
        /// </summary>
        [Test]
        public void NamedPageSelT15()
        {
            OneTest("T15", "@page and an arbitrary name");
        }
        #endregion T15

        #region T16
        /// <summary>
        /// Page header selector
        /// </summary>
        [Test]
        public void PageHeaderSelT16()
        {
            OneTest("T16", "@page {@top-left {} @top-center {} @top-right {} }");
        }
        #endregion T16

        #region T18
        /// <summary>
        /// language selector
        /// </summary>
        [Test]
        public void LanguageSelT18()
        {
            OneTest("T18", ":lang(en) to select element with text of language 'en'");
        }
        #endregion T18

        #region T19
        /// <summary>
        /// follow selector
        /// </summary>
        [Test]
        public void FollowSelT19()
        {
            OneTest("T19", ".class1 .class2 matches two class names with no punctuation between");
        }
        #endregion T19

        #region T21
        /// <summary>
        /// And selector
        /// </summary>
        [Test]
        public void AndSelT21()
        {
            OneTest("T21", ".letter, .headword match class names separated by commas");
        }
        #endregion T21

        #region T21B
        /// <summary>
        /// class tag selector
        /// </summary>
        [Test]
        public void ClassTagSelT21B()
        {
            OneTest("T21B", "span.headword match a tag name with a class name");
        }
        #endregion T21B

        #region T22
        /// <summary>
        /// IntraList selector
        /// </summary>
        [Test]
        public void IntraListSelT22()
        {
            OneTest("T22", ".class + .class[lang=en_US]::before used to insert , between like members of a list");
        }
        #endregion T22

        #region T22b
        /// <summary>
        /// InterLanguage selector
        /// </summary>
        [Test]
        public void InterLangSelT22b()
        {
            OneTest("T22b", ".class[lang='en'] + .class[lang='tpi']::before insert / between text in different languages");
        }
        #endregion T22b

        #region T23
        /// <summary>
        /// Print Media selector
        /// </summary>
        [Test]
        public void PrintMediaSelT23()
        {
            OneTest("T23", "@media print");
        }
        #endregion T23

        #region T23B
        /// <summary>
        /// All media selector
        /// </summary>
        [Test]
        public void AllMediaSelT23B()
        {
            OneTest("T23B", "@media all");
        }
        #endregion T23B

        #region T24
        /// <summary>
        /// Import declaration
        /// </summary>
        [Test]
        public void ImportDeclT24()
        {
            OneTest("T24", "@import 'branch.css';");
        }
        #endregion T24

        #region T24B
        /// <summary>
        /// url include declaration
        /// </summary>
        [Test]
        public void UrlImportDeclT24B()
        {
            OneTest("T24B", "@include 'http://www.sil.org/Africa/Cameroon/branch.css';");
        }
        #endregion T24B

        #region T25
        /// <summary>
        /// margin property with one or more parameters
        /// </summary>
        [Test]
        public void MarginPropT25()
        {
            OneTest("T25", "margin property followed by from 1 to 4 space separated arguments");
        }
        #endregion T25

        #region T26
        /// <summary>
        /// multiple arguments including %
        /// </summary>
        [Test]
        public void MultipleIncPercentExprT26()
        {
            OneTest("T26", "from 1 to for arguments, % arguments, string arguments");
        }
        #endregion T26

        #region T27
        /// <summary>
        /// border content with string set
        /// </summary>
        [Test]
        public void BorderContentStringSetT27()
        {
            OneTest("T27", "border and content argument with string-set function");
        }
        #endregion T27

        #region T28
        /// <summary>
        /// Display property
        /// </summary>
        [Test]
        public void DisplayPropT28()
        {
            OneTest("T28", "display property with acceptable arguments");
        }
        #endregion T28

        #region T28b
        /// <summary>
        /// Inherit Property
        /// </summary>
        [Test]
        public void InheritPropT28b()
        {
            OneTest("T28b", "properties with inherit, as well as units like pc and px");
        }
        #endregion T28b

        #region T29
        /// <summary>
        /// Double Quote declaration
        /// </summary>
        [Test]
        public void DoubleQuoteDeclT29()
        {
            OneTest("T29", ".xitem [lang=\"en\"] + .xitem [lang=\"tpi\"] > .xlanguagetag");
        }
        #endregion T29

        #region T30
        /// <summary>
        /// Bad CSS files
        /// </summary>
        [Test]
        public void FailingCssT30()
        {
            OneTest("T30", "CSS parses with errors");
        }
        #endregion T30

        #region T30b
        /// <summary>
        /// CSS throws error
        /// </summary>
        [Test]
        //[ExpectedException("Antlr.Runtime.Tree.RewriteEarlyExitException")]
        public void CssNoErrorT30b()
        {
            OneTest("T30b", "CSS no error");
        }
        #endregion T30b

        #region T31
        /// <summary>
        /// quoted double quote
        /// </summary>
        [Test]
        public void CssQuotedDoubleQuoteT31()
        {
            OneTest("T31", "CSS Quoted Double Quote");
        }
        #endregion T31

        #region T31b
        /// <summary>
        /// CSS quoted single quote
        /// </summary>
        [Test]
        public void CssQuotedSingleQuoteT31b()
        {
            OneTest("T31b", "CSS quoted single quote");
        }
        #endregion T31b

        #region T32
        /// <summary>
        /// CSS pseudo only selector
        /// </summary>
        [Test]
        public void CssPseudoOnlyT32()
        {
            OneTest("T32", "CSS pseudo only");
        }
        #endregion T32

        #region T33
        /// <summary>
        /// CSS em as tag and unit
        /// </summary>
        [Test]
        public void CssEmAsTagAndUnitT33()
        {
            OneTest("T33", "CSS em as tag and unit");
        }
        #endregion T33

        #region T34
        /// <summary>
        /// Test selector same as unit
        /// </summary>
        [Test]
        public void SelectorIsUnitT34()
        {
            OneTest("T34", "Selector Is Unit");
        }
        #endregion T34

        #region T35
        /// <summary>
        /// Test :not() pseudo selector with embedded pseudo
        /// </summary>
        [Test]
        public void NotFuncWithEmbeddedPseudoT35()
        {
            OneTest("T35", ":not() pseudo selector with embedded pseudo");
        }
        #endregion T35
    }
}