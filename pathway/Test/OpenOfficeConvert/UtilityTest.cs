using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;
using ArrayList=System.Collections.ArrayList;

namespace Test.OpenOfficeConvert
{
    /// <summary>
    ///This is a test class for UtilityTest and is intended
    ///to contain all UtilityTest Unit Tests
    ///</summary>
    [TestFixture]
    [Category("BatchTest")]
    public class UtilityTest
    {

        #region Private Variables
        Utility _utilityXML;
        private string _methodName;
        private static string _inputPath;
        private static string _outputPath;
        private static string _expectedPath;
        #endregion Private Variables

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            _utilityXML = new Utility();
            _inputPath = PathPart.Bin(Environment.CurrentDirectory,"/OpenOfficeConvert/TestFiles/input");
            _outputPath = PathPart.Bin(Environment.CurrentDirectory, "/OpenOfficeConvert/TestFiles/output");
            _expectedPath = PathPart.Bin(Environment.CurrentDirectory, "/OpenOfficeConvert/TestFiles/expected");
            Common.SupportFolder = "";
            Common.ProgInstall = Common.DirectoryPathReplace(Environment.CurrentDirectory + "/../../../PsSupport");
        }
        #endregion Setup


        /// <summary>
        ///A test for ParentValue
        ///</summary>
        [Test]
        public void ParentValueTest1()
        {
            _methodName = "ParentValueTest1";
            const string expected = "Test";
            _utilityXML.ParentValue = expected;
            string actual = _utilityXML.ParentValue;
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for ParentName
        ///</summary>
        [Test]
        public void ParentNameTest1()
        {
            _methodName = "ParentNameTest1";
            const string expected = "Test";
            _utilityXML.ParentName = expected;
            string actual = _utilityXML.ParentName;
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for MissingLang
        ///</summary>
        [Test]
        public void MissingLangTest1()
        {
            _methodName = "MissingLangTest1";
            const bool expected = true;
            _utilityXML.MissingLang = expected;
            bool actual = _utilityXML.MissingLang;
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for MakeClassName
        ///</summary>
        [Test]
        public void MakeClassNameTest1()
        {
            _methodName = "MissingLangTest1";
            const string expected = "Test";
            _utilityXML.MakeClassName = expected;
            string actual = _utilityXML.MakeClassName;
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for ChildName
        ///</summary>
        [Test]
        public void ChildNameTest()
        {
            _methodName = "MissingLangTest1";
            const string expected = "Test";
            _utilityXML.ChildName = expected;
            string actual = _utilityXML.ChildName;
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for SetColumnGap
        ///</summary>
        [Test]
        public void SetColumnGapTest1()
        {
            _methodName = "SetColumnGapTest1";
            string contentFilePath = _inputPath + "SetColumnGapTest1.xhtml";
            var columnInput = new Dictionary<string, string>();
            var columnGapEm = new Dictionary<string, Dictionary<string, string>>();
            columnInput.Add("pageWidth", "8.5");
            columnInput.Add("columnCount", "2");
            columnInput.Add("columnGap", "18pt");
            columnGapEm.Add("Sect_letdata", columnInput);
            var doc = new XmlDocument();
            doc.Load(Common.PathCombine(_expectedPath, "SetColumnGapTest1.xml"));
            if (doc.DocumentElement != null)
            {
                string expNode = doc.DocumentElement.InnerXml;
                Dictionary<string, XmlNode> actual = _utilityXML.SetColumnGap(contentFilePath, columnGapEm);
                Assert.AreEqual(expNode, actual["Sect_letdata"].InnerXml, _methodName + " test failed");
            }
        }

        /// <summary>
        ///A test for IsStyleExist
        ///</summary>
        [Test]
        public void StyleExistTest()
        {
            _methodName = "SetColumnGapTest1";
            string styleFilePath = GetFileNameWithExpectedPath("StyleExistTest.xml");
            const string styleName = "letData";
            bool actual = _utilityXML.IsStyleExist(styleFilePath, styleName);
            Assert.IsTrue(actual, _methodName + " test failed");
        }



        /// <summary>
        ///A test for GetNewChildName
        ///</summary>
        [Test]
        public void GetNewChildNameTest()
        {
            _methodName = "GetNewChildNameTest";
            string styleFilePath = GetFileNameWithExpectedPath("StyleExistTest.xml");
            var styleStack = new Stack();
            styleStack.Push("a_.en_b");
            const string child = "letdata_.";
            const string expected = "letdata_.";
            string actual = _utilityXML.GetNewChildName(styleFilePath, styleStack, child, false);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for GetFontweight
        ///</summary>
        [Test]
        public void GetFontweightTest()
        {
            _methodName = "GetFontweightTest";
            string styleFilePath = GetFileNameWithInputPath("GetFontweightTest.xml"); 
            var styleStack = new Stack();
            styleStack.Push("a");
            var childAttribute = new StyleAttribute {StringValue = "lighter"};
            const string expected = "400";
            string actual = _utilityXML.GetFontweight(styleFilePath, styleStack, childAttribute);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for GetFontsizeInsideSpan
        ///</summary>
        [Test]
        public void GetFontsizeInsideSpanTest()
        {
            _methodName = "GetFontsizeInsideSpanTest";
            string styleFilePath = GetFileNameWithInputPath("GetFontweightTest.xml");
            const float parentFontsize = 2.0F;
            var childAttribute = new StyleAttribute {ClassName = "b", NumericValue = 12};
            const string expected = "24pt";
            string actual = _utilityXML.GetFontsizeInsideSpan(styleFilePath, parentFontsize, childAttribute);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for GetFontsize
        ///</summary>
        [Test]
        public void GetFontsizeTest()
        {
            _methodName = "GetFontsizeTest";
            string styleFilePath = GetFileNameWithInputPath("GetFontweightTest.xml");
            var styleStack = new Stack();
            styleStack.Push("a");
            var childAttribute = new StyleAttribute {Name = "fo:font-size", Unit = "%", NumericValue = 4};
            const string expected = "0.48pt";
            string actual = _utilityXML.GetFontsize(styleFilePath, styleStack, childAttribute);
            Assert.AreEqual(expected, actual, _methodName + " test failed");
        }

        /// <summary>
        ///A test for CreateStyleWithNewValue
        ///</summary>
        [Test]
        public void CreateStyleWithNewValueTest()
        {
            _methodName = "CreateStyleWithNewValueTest";
            string outpath = GetFileNameWithOutputPath("CreateStyleWithNewValueTest.xml");
            string expected = GetFileNameWithExpectedPath("CreateStyleWithNewValueTest.xml");
            string styleFilePath = GetFileNameWithInputPath("CreateStyleWithNewValueTest.xml");
            CopyToOutput(styleFilePath,outpath);
            const string className = "a";
            const string makeClassName = "a";
            IDictionary<string, string> makeAttribute = new Dictionary<string, string>();
            makeAttribute["fo:font-size"] = "12pt";
            const string parentName = "c";
            const string familyType = "paragraph";
            var backgroundClassName = new ArrayList {"abc"};
            _utilityXML.CreateStyleWithNewValue(outpath, className, makeClassName, makeAttribute, parentName, familyType, backgroundClassName);
            XmlAssert.AreEqual(expected, outpath, _methodName + " test failed");
        }

        /// <summary>
        ///A test for CreateStyleHyphenate
        ///</summary>
        [Test]
        public void CreateStyleHyphenateTest()
        {
            _methodName = "CreateStyleHyphenateTest";
            string styleFilePath = GetFileNameWithInputPath("hyphenTest.xml");
            string outpath = GetFileNameWithOutputPath("CreateStyleHyphenateTest.xml");
            string expected = GetFileNameWithExpectedPath("CreateStyleHyphenateTest.xml");
            CopyToOutput(styleFilePath, outpath);
            const string className = "orphans";
            _utilityXML.CreateStyleHyphenate(outpath, className);
            XmlAssert.AreEqual(expected, outpath, _methodName + " test failed");
        }

        /// <summary>
        ///A test for CreateStyle
        ///</summary>
        [Test]
        public void CreateStyleTest()
        {
            _methodName = "CreateStyleTest";
            string outpath = GetFileNameWithOutputPath("CreateStyleTest.xml");
            string expected = GetFileNameWithExpectedPath("CreateStyleTest.xml");
            string styleFilePath = GetFileNameWithInputPath("GetFontweightTest.xml");
            CopyToOutput(styleFilePath, outpath);
            const string className = "a";
            const string makeClassName = "b_a";
            IDictionary<string, string> makeAttribute = new Dictionary<string, string>();
            makeAttribute["fo:font-size"] = "12pt";
            const string parentName = "c";
            const string familyType = "paragraph";
            var backgroundClassName = new ArrayList {"abc"};
            _utilityXML.CreateStyle(outpath, className, makeClassName, parentName, familyType, backgroundClassName, false);
            XmlAssert.AreEqual(expected, outpath, _methodName + " test failed");

        }

        /// <summary>
        ///A test for CreateMasterContents
        ///</summary>
        [Test]
        public void CreateMasterContentsTest()
        {
            _methodName = "CreateMasterContentsTest";

            string styleFilePath = GetFileNameWithInputPath("Master.xml");
            string outpath = GetFileNameWithOutputPath("Master.xml");
            string expected = GetFileNameWithExpectedPath("Master.xml");
            CopyToOutput(styleFilePath, outpath);
            var odtFiles = new ArrayList {"reversal", "grammer"};
            _utilityXML.CreateMasterContents(outpath, odtFiles);
            XmlAssert.AreEqual(expected, outpath, _methodName + " test failed");

        }

        /// <summary>
        ///A test for CreateGraphicsStyle
        ///</summary>
        [Test]
        public void CreateGraphicsStyleTest()
        {
            _methodName = "CreateGraphicsStyleTest";

            string outpath = GetFileNameWithOutputPath("CreateGraphicsStyleTest.xml");
            string expected = GetFileNameWithExpectedPath("CreateGraphicsStyleTest.xml");
            string styleFilePath = GetFileNameWithInputPath("Graphics.xml");
            CopyToOutput(styleFilePath, outpath);
            const string makeClassName = "b_a";
            const string parentName = "c";

            const string position = "right";
            const string side = "left";
            _utilityXML.CreateGraphicsStyle(outpath, makeClassName, parentName, position, side);
            XmlAssert.AreEqual(expected, outpath, _methodName + " test failed");

        }

        /// <summary>
        ///A test for CreateDropCapStyle
        ///</summary>
        [Test]
        public void CreateDropCapStyleTest()
        {
            _methodName = "CreateDropCapStyleTest";
            string outpath = GetFileNameWithOutputPath("CreateDropCapStyleTest.xml");
            string expected = GetFileNameWithExpectedPath("CreateDropCapStyleTest.xml");
            string styleFilePath = GetFileNameWithInputPath("DropCapsTest.xml");
            CopyToOutput(styleFilePath, outpath);
            const string className = "b";
            const string makeClassName = "abc";
            const string parentName = "c";
            const int noOfChar = 4;
            _utilityXML.CreateDropCapStyle(outpath, className, makeClassName, parentName, noOfChar);
            XmlAssert.AreEqual(expected, outpath, _methodName + " test failed");

        }

        private static string GetFileNameWithInputPath(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }
        private static string GetFileNameWithOutputPath(string fileName)
        {
            return Common.PathCombine(_outputPath, fileName);
        }
        private static string GetFileNameWithExpectedPath(string fileName)
        {
            return Common.PathCombine(_expectedPath, fileName);
        }
        private static void CopyToOutput(string input, string output)
        {
            if (File.Exists(input))
                File.Copy(input, output, true);
        }

    }
}
