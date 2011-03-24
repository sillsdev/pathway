using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;
using Test;

namespace Test.Xetex
{
    [TestFixture]
    public class XetexTest
    {
        #region Private Variables
        private string _inputCSS;
        private string _inputXHTML;
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;
        private string _outputStory;
        private string _outputStyles;
        private Dictionary<string, string> _expected = new Dictionary<string, string>();
        private string _className = "a";
        private string _testFolderPath = string.Empty;
        Dictionary<string, Dictionary<string, string>> _idAllClass = new Dictionary<string, Dictionary<string, string>>();
        private InStyles _stylesXML;
        private InStory _storyXML;
        private readonly ArrayList headwordStyles = new ArrayList();

        private Dictionary<string, Dictionary<string, string>> _cssProperty;
        private CssTree _cssTree;
        private PublicationInformation _projInfo;
        private Dictionary<string, List<string>> _classInlineStyle;

        #endregion
        #region Setup
        [TestFixtureSetUp]
        protected void SetUpAll()
        {
            Common.Testing = true;
            _stylesXML = new InStyles();
            _storyXML = new InStory();
            _projInfo = new PublicationInformation();
            _classInlineStyle = new Dictionary<string, List<string>>();
            _testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/Xetex/TestFiles");
            _inputPath = Common.PathCombine(_testFolderPath, "input");
            _outputPath = Common.PathCombine(_testFolderPath, "output");
            _expectedPath = Common.PathCombine(_testFolderPath, "expected");
            _cssProperty = new Dictionary<string, Dictionary<string, string>>();
            Common.SupportFolder = "";
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, "/../PsSupport");
            Common.CopyOfficeFolder(_expectedPath, _outputPath);
        }

        [SetUp]
        protected void SetupEach()
        {
            _cssTree = new CssTree();
        }
        #endregion Setup

        #region Public Functions

        #region Nunits

        /// <summary>
        /// Multi Parent Test - .subsenses > .sense > .xsensenumber { font-size:10pt;}
        /// Parent comes as multiple times
        /// </summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void TextAlignTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "TextAlign";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void TextIndentTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "TextIndent";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void TextColorTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "Color";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void FontStyleItalicTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "FontStyleItalic";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void FontVariantSmallCapTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "FontVariantSmallCap";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void FontSizePointTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "FontSizePoint";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void FontWeightBoldTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "FontWeightBold";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void TextAlignCenterTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "TextAlignCenter";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void MarginTopTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "MarginTop";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void DisplayBlockTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "DisplayBlock";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void TextAlignRightTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "TextAlignRight";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void PseudoAfterTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "PseudoAfter";
            ExportProcess(file);
            FileCompare(file);
        }

        //[Test]
        //[Category("SkipOnTeamCity")]
        //public void TextDecorationTest()
        //{
        //    _projInfo.ProjectInputType = "Dictionary";
        //    const string file = "TextDecoration";
        //    ExportProcess(file);
        //    FileCompare(file);
        //}

        #region NestedDiv
        [Test]
        public void NestedDiv1()
        {
            const string file = "NestedDivCase1";
            _projInfo.ProjectInputType = "Dictionary";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        public void NestedDiv2()
        {
            const string file = "NestedDivCase2";
            _projInfo.ProjectInputType = "Dictionary";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        public void NestedDiv3()
        {
            const string file = "NestedDivCase3";
            _projInfo.ProjectInputType = "Dictionary";
            ExportProcess(file);
            FileCompare(file);
        }


        [Test]
        public void NestedDiv4()
        {
            const string file = "NestedDivCase4";
            _projInfo.ProjectInputType = "Dictionary";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        public void NestedDiv5()
        {
            const string file = "NestedDivCase5";
            _projInfo.ProjectInputType = "Dictionary";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        public void NestedDiv6()
        {
            const string file = "NestedDivCase6";
            _projInfo.ProjectInputType = "Dictionary";
            ExportProcess(file);
            FileCompare(file);
        }


        [Test]
        public void NestedSpan1()
        {
            const string file = "NestedSpan1";
            _projInfo.ProjectInputType = "Dictionary";
            ExportProcess(file);
            FileCompare(file);
        }
        #endregion

        [Test]
        public void EMTest1()
        {
            const string file = "EMTest1";
            _projInfo.ProjectInputType = "Dictionary";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        public void Font1()
        {
            const string file = "Font1";
            _projInfo.ProjectInputType = "Dictionary";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void ImageBaseTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "ImageBase";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void TextIndentPcTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "TextIndentPC";
            ExportProcess(file);
            FileCompare(file);
        }

       [Test]
        [Category("SkipOnTeamCity")]
        public void MarginRightTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "MarginRight";
            ExportProcess(file);
            FileCompare(file);
        }

 		[Test]
        public void DisplayNone()
        {
            const string file = "DiplayNone";
            _projInfo.ProjectInputType = "Dictionary";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void TextAlignJustifyTest()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "TextAlignJustify";
            ExportProcess(file);
            FileCompare(file);
        }


        [Test]
        [Category("SkipOnTeamCity")]
        public void Counter1Test()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "Counter1";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void Counter2Test()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "Counter2";
            ExportProcess(file);
            FileCompare(file);
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void Counter3Test()
        {
            _projInfo.ProjectInputType = "Dictionary";
            const string file = "Counter3";
            ExportProcess(file);
            FileCompare(file);
        }






        #endregion



        #region Private Functions

        private void FileCompare(string file)
        {
            string texOutput = FileOutput(file + ".tex");
            string texExpected = FileExpected(file + ".tex");
            FileAssert.AreEqual(texOutput, texExpected, file + " in tex ");
        }

        private void ExportProcess(string file)
        {
            string input = FileInput(file + ".xhtml");
            _projInfo.DefaultXhtmlFileWithPath = input;

            input = FileInput(file + ".css");
            _projInfo.DefaultCssFileWithPath = input;

            _projInfo.TempOutputFolder = _outputPath;
            _projInfo.OutputExtension = ".tex";

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssClass = cssTree.CreateCssProperty(input, true);

            string xetexFullFile = Path.Combine(_outputPath, file + ".tex");
            StreamWriter xetexFile = new StreamWriter(xetexFullFile);

            XeTexStyles styles = new XeTexStyles();
            _classInlineStyle = styles.CreateXeTexStyles(_outputPath,xetexFile, cssClass);

            XeTexContent content = new XeTexContent();
            Dictionary<string, Dictionary<string, string>> newProperty = content.CreateContent(_outputPath, cssClass, xetexFile, _projInfo.DefaultXhtmlFileWithPath,
                                  _classInlineStyle, cssTree.SpecificityClass, cssTree.CssClassOrder);

            CloseFile(xetexFile);

            ModifyXeTexStyles modifyXeTexStyles = new ModifyXeTexStyles();
            modifyXeTexStyles.ModifyStylesXML(_projInfo.ProjectPath, xetexFile, newProperty, cssClass, xetexFullFile);

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
        private void CloseFile(StreamWriter xetexFile)
        {
            xetexFile.WriteLine();
            xetexFile.WriteLine(@"\bye");
            xetexFile.Flush();
            xetexFile.Close();
        }

        #endregion PrivateFunctions

        #endregion
    }
}
