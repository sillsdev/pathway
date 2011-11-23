// --------------------------------------------------------------------------------------------
// <copyright file="CommonXmlTest.cs" from='2009' to='2009' company='SIL International'>
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
// Test methods of FlexDePlugin
// </remarks>
// --------------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.PsTool
{

    public partial class CommonTest
    {
        /// <summary>
        ///A test for GetMetaValue
        ///</summary>
        [Test]
        public void GetMetaValueTest()
        {
            string fileName = GetFileNameWithPath("Test.xhtml");
            string _expected = @"C:\FieldWorks";
            string actual = Common.GetMetaValue(fileName);
            Assert.AreEqual(_expected, actual);
        }

        /// <summary>
        ///A test for ReturnImageSource
        ///</summary>
        [Test]
        public void ReturnImageSourceTest()
        {
            LoadInputDocument("PictureWidth.xhtml");
            var expected = new[] { "Pictures/Home.jpg", "Pictures/Garden.jpg" };
            string stringContent = actualDocument.InnerXml;
            string[] actual = Common.ReturnImageSource(stringContent);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SetProgressBarValue
        ///</summary>
        [Test]
        public void SetProgressBarValue1()
        {
            ProgressBar pb = new ProgressBar();
            string sourcefile = GetFileNameWithPath("ProgressBar.xhtml");
            Common.SetProgressBarValue(pb, sourcefile);
            int expected = 21;
            Assert.AreEqual(expected, pb.Maximum);
        }

        /// <summary>
        ///A test for SetProgressBarValue
        ///</summary>
        [Test]
        public void SetProgressBarValue2()
        {
            ProgressBar pb = new ProgressBar();
            string sourcefile = GetFileNameWithPath("NotExist.xhtml");
            Common.SetProgressBarValue(pb, sourcefile);
            int expected = 0;
            Assert.AreEqual(expected, pb.Maximum);
        }


        /// <summary>
        ///A test for GetMetaDataTest
        ///</summary>
        [Test]
        public void GetMetaDataTest()
        {
            Dictionary<string, string> _metaDataDic = new Dictionary<string, string>();
            string projectInputType = string.Empty;
            projectInputType = "Dictionary";
            string metaDataFull = GetFileNameWithPath("MetaData.xml");
            _metaDataDic = Common.GetMetaData(projectInputType, metaDataFull);

            List<string> metaDataList = new List<string>();
            metaDataList.Add("Title");
            metaDataList.Add("Creator");
            metaDataList.Add("Publisher");
            metaDataList.Add("Description");
            metaDataList.Add("Copyright Holder");
            metaDataList.Add("Subject");

            Dictionary<string, string> metaDataDicExpected = new Dictionary<string, string>();
            metaDataDicExpected["Title"] = "sams";
            metaDataDicExpected["Creator"] = "sams creator";
            metaDataDicExpected["Publisher"] = "sams publisher";
            metaDataDicExpected["Description"] = "sams book";
            metaDataDicExpected["Copyright Holder"] = "sams international © 2011. All Rights Reserved.";
            metaDataDicExpected["Subject"] = "Foreign Literatures and Linguistics; Language Documentation; Dictionary; Reference";

            Assert.AreEqual(_metaDataDic,metaDataDicExpected);
        }

        /// <summary>
        ///A test for GetXmlNodeList
        ///</summary>
        [Test]
        public void GetXmlNodeListTest()
        {
            string xmlFileNameWithPath = GetFileNameWithPath("GenericFont.xml");
            string genericFamily = "cursive";
            string xPath = "//font-preference/generic-family [@name = \"" + genericFamily + "\"]";
            ArrayList actual = Common.GetXmlNodeList(xmlFileNameWithPath, xPath);
            ArrayList expected = new ArrayList();
            expected.Add("Comic Sans MS");
            expected.Add("Lydian Cursive ");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetXmlNode
        ///</summary>
        [Test]
        public void GetXmlNodeTest()
        {
            XmlNode expected =
                LoadXmlDocument(
                    "<generic-family name=\"cursive\"><font>Comic Sans MS</font><font>Lydian Cursive </font></generic-family>",
                    false);

            string xmlFileNameWithPath = GetFileNameWithPath("GenericFont.xml");
            string genericFamily = "cursive";
            string xPath = "//font-preference/generic-family [@name = \"" + genericFamily + "\"]";
            XmlNode actual = Common.GetXmlNode(xmlFileNameWithPath, xPath);
            Assert.AreEqual(expected.OuterXml, actual.OuterXml);
        }

        /// <summary>
        ///A test for XsltProcess
        ///</summary>
        [Test]
        public void XsltProcessTest1()
        {
            string inputFile = GetFileNameWithPath("FlexRev.xml");
            string xsltFile = GetFileNameWithPath("FlexRev.xsl");
            string ext = ".xhtml";
            string expected = GetFileNameWithExpectedPath("FlexRev.xhtml");
            string actual = Common.XsltProcess(inputFile, xsltFile, ext);
            XmlAssert.AreEqual(expected, actual, "FlexRev.xhtml different");
        }

        /// <summary>
        ///A test for XsltProcess
        ///</summary>
        [Test]
        public void XsltProcessTest2()
        {
            string inputFile = GetFileNameWithPath("FlexRev.xml");
            string xsltFile = GetFileNameWithPath("FlexRev.xsl");
            string ext = "";  // No Extenstion
            string expected = GetFileNameWithExpectedPath("FlexRev.xhtml");
            string actual = Common.XsltProcess(inputFile, xsltFile, ext);
            XmlAssert.AreEqual(expected, actual, "FlexRev produced is different");
        }
        /// <summary>
        ///A test for XsltProcess
        ///</summary>
        [Test]
        public void XsltProcessTest3()
        {
            string inputFile = GetFileNameWithPath("");
            string xsltFile = GetFileNameWithPath("");
            string ext = ".xhtml";
            string expected = "";
            string actual = Common.XsltProcess(inputFile, xsltFile, ext);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for XsltProcess
        ///</summary>
        [Test]
        public void XsltProcessTest4()
        {
            string inputFile = GetFileNameWithPath("FlexRev.xml");
            string xsltFile = GetFileNameWithPath("");
            string ext = ".xhtml";
            string expected = "FlexRev.xml";
            string actual = Common.XsltProcess(inputFile, xsltFile, ext);
            Assert.AreEqual(expected, Path.GetFileName(actual));
        }

        /// <summary>
        ///A test for Xslt2Process, test Scritpure params with installed xsl for processed file
        ///</summary>
        [Test]
        public void XsltProcessTest5()
        {
            const string inputName = "GPS12L10.xhtml";
            const string xsltName = "pxhtml2xpw-scr.xsl";
            string inputFile = GetFileNameWithOutputPath(inputName);
            string xsltFile = GetFileNameWithOutputPath(xsltName);
            File.Copy(GetFileNameWithPath(inputName), inputFile, true);
            File.Copy(GetFileNameWithProgPath(xsltName), xsltFile, true);
            string ext = ".txt";
            string expected = "GPS12L10.txt";
            var myParams = new Dictionary<string, string>();
            myParams["ver"] = "bgt";
            string actual = Common.XsltProcess(inputFile, xsltFile, ext, myParams);
            Assert.AreEqual(expected, Path.GetFileName(actual));
            TextFileAssert.AreEqual(GetFileNameWithExpectedPath(expected), actual);
        }

        /// <summary>
        ///A test for Xslt2Process, test x: namespace for xhtml & dictionary params
        ///</summary>
        [Test]
        public void XsltProcessTest6()
        {
            const string inputName = "main.xhtml";
            const string xsltName = "xhtml2xpw-dic.xsl";
            string inputFile = GetFileNameWithOutputPath(inputName);
            string xsltFile = GetFileNameWithOutputPath(xsltName);
            File.Copy(GetFileNameWithPath(inputName), inputFile, true);
            File.Copy(GetFileNameWithPath(xsltName), xsltFile, true);
            string ext = ".txt";
            string expected = "main.txt";
            var myParams = new Dictionary<string, string>();
            myParams["ver"] = "bzh";
            myParams["l1"] = "en";
            myParams["l2"] = "tpi";
            string actual = Common.XsltProcess(inputFile, xsltFile, ext, myParams);
            Assert.AreEqual(expected, Path.GetFileName(actual));
            TextFileAssert.AreEqual(GetFileNameWithExpectedPath(expected), actual);
        }

        ///// <summary>
        /////A test for Xslt2Process
        /////</summary>
        //[Test]
        //public void Xslt2ProcessTest1()
        //{
        //    string inputFile = GetFileNameWithPath("FlexRev.xml");
        //    string xsltFile = GetFileNameWithPath("FlexRev.xsl");
        //    string ext = ".xhtml";
        //    string expected = GetFileNameWithExpectedPath("FlexRev.xhtml");
        //    string actual = Common.Xslt2Process(inputFile, xsltFile, ext);
        //    XmlAssert.AreEqual(expected, actual, "FlexRev.xhtml different");
        //}

        ///// <summary>
        /////A test for Xslt2Process
        /////</summary>
        //[Test]
        //public void Xslt2ProcessTest2()
        //{
        //    string inputFile = GetFileNameWithPath("FlexRev.xml");
        //    string xsltFile = GetFileNameWithPath("FlexRev.xsl");
        //    string ext = "";  // No Extenstion
        //    string expected = GetFileNameWithExpectedPath("FlexRev.xhtml");
        //    string actual = Common.Xslt2Process(inputFile, xsltFile, ext);
        //    XmlAssert.AreEqual(expected, actual, "FlexRev produced is different");
        //}
        ///// <summary>
        /////A test for Xslt2Process
        /////</summary>
        //[Test]
        //public void Xslt2ProcessTest3()
        //{
        //    string inputFile = GetFileNameWithPath("");
        //    string xsltFile = GetFileNameWithPath("");
        //    string ext = ".xhtml";
        //    string expected = "";
        //    string actual = Common.Xslt2Process(inputFile, xsltFile, ext);
        //    Assert.AreEqual(expected, actual);
        //}

        ///// <summary>
        /////A test for Xslt2Process
        /////</summary>
        //[Test]
        //public void Xslt2ProcessTest4()
        //{
        //    string inputFile = GetFileNameWithPath("FlexRev.xml");
        //    string xsltFile = GetFileNameWithPath("");
        //    string ext = ".xhtml";
        //    string expected = "FlexRev.xml";
        //    string actual = Common.Xslt2Process(inputFile, xsltFile, ext);
        //    Assert.AreEqual(expected, Path.GetFileName(actual));
        //}

        ///// <summary>
        /////A test for Xslt2Process
        /////</summary>
        //[Test]
        //public void Xslt2ProcessTest5()
        //{
        //    const string inputName = "GPS12L10.xhtml";
        //    const string xsltName = "xhtml2xpw-scripture1.xsl";
        //    string inputFile = GetFileNameWithOutputPath(inputName);
        //    string xsltFile = GetFileNameWithOutputPath(xsltName);
        //    File.Copy(GetFileNameWithPath(inputName), inputFile, true);
        //    File.Copy(GetFileNameWithPath(xsltName), xsltFile, true);
        //    string ext = ".txt";
        //    string expected = "GPS12L10.txt";
        //    string actual = Common.Xslt2Process(inputFile, xsltFile, ext);
        //    Assert.AreEqual(expected, Path.GetFileName(actual));
        //    TextFileAssert.AreEqual(GetFileNameWithExpectedPath(expected), actual);
        //}

        /// <summary>
        ///A test for CreatePreviewFile
        ///</summary>
        [Test]
        public void CreatePreviewFileTest()
        {
            Param.LoadSettings();
            Param.Value[Param.InputType] = "Scripture";
            Param.LoadSettings();
            string xhtmlFile = GetFileNameWithPath("Preview.xhtml");
            string cssFile = GetFileNameWithPath("Preview.css");
            string outputFileName = "Preview";
            string expected = GetFileNameWithExpectedPath("Preview.html");
            string actual = Preview.CreatePreviewFile(xhtmlFile, cssFile, outputFileName, true);
            TextFileAssert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetLanguageName
        ///</summary>
        [Test]
        public void GetLanguageNameTest()
        {
            string languageCode = "TA";
            string expected = "Tamil";
            string actual = Common.GetLanguageName(languageCode);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for GetLanguageName
        ///</summary>
        [Test]
        public void GetLanguageNameNullEmpty()
        {
            string expected = "";

            string languageCode = null;
            string actual = Common.GetLanguageName(languageCode);
            Assert.AreEqual(expected, actual);

            languageCode = "";
            actual = Common.GetLanguageName(languageCode);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetCountryCode
        ///</summary>
        [Test]
        public void GetCountryCodeTest()
        {
            string language;
            string languageExpected = "en";
            string country;
            string countryExpected = "GB";
            string langCountry = "en-GB";

            Dictionary<string, ArrayList> spellCheck = new Dictionary<string, ArrayList>();
            ArrayList arrayList = new ArrayList();
            arrayList.Add("GB");
            arrayList.Add("US");
            arrayList.Add("ZA");
            spellCheck["en"] = arrayList;

            Common.GetCountryCode(out language, out country, langCountry, spellCheck);
            Assert.AreEqual(languageExpected, language);
            Assert.AreEqual(countryExpected, country);
        }
        /// <summary>
        ///A test for GetPictureFromPath
        ///</summary>
        [Test]
        public void GetPictureFromPathTest()
        {
            string src = "Pictures/Ax1.jpg";
            string metaName = string.Empty;
            string sourcePicturePath = Common.PathCombine(GetTestPath(), "InputFiles");
            string expected = Common.PathCombine(sourcePicturePath, src);
            string actual = Common.GetPictureFromPath(src, metaName, sourcePicturePath);
            Assert.AreEqual(expected, actual);

        }

    }
}
