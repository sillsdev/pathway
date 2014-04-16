// --------------------------------------------------------------------------------------------
// <copyright file="PreExportProcessTest.cs" from='2009' to='2014' company='SIL International'>
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
// Test methods of FlexDePlugin
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Xml;
using NUnit.Framework;
using SIL.Tool;

namespace Test.PsTool
{
    [TestFixture]
    public class PreExportProcessTest
    {
        PreExportProcess preExportProcess;
        public string _node;

        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.SupportFolder = "";
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../PsSupport");
            Common.Testing = true;
        }

        /// <summary>
        ///A test for ImagePreprocess
        ///</summary>
        [Test]
        public void ImagePreprocessTest()
        {
            string filename = "ImagePreProcess.xhtml";
            string input = GetFileNameWithPath(filename);
            PublicationInformation projInfo = new PublicationInformation();
            
            string expected = GetFileNameWithExpectedPath(filename);
            string output = GetFileNameWithOutputPath(filename);
            CopyToOutput(input, output);
            projInfo.DefaultXhtmlFileWithPath = output;
            projInfo.ProjectInputType = "Scripture";
            preExportProcess = new PreExportProcess(projInfo);
            output = preExportProcess.ImagePreprocess(false);
            XmlAssert.AreEqual(expected, output, "");

        }

        /// <summary>
        ///A test for GoBibleRearrangeVerseNumbers
        ///</summary>
        [Test]
        public void GoBibleRearrangeVerseNumbersTest()
        {
            string filename = "GoBibleRearrangeVerseNumbers.xhtml";
            string input = GetFileNameWithPath(filename);
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = input;
            preExportProcess = new PreExportProcess(projInfo);
            string expected = GetFileNameWithExpectedPath(filename);
            string output = preExportProcess.GoBibleRearrangeVerseNumbers(projInfo.DefaultXhtmlFileWithPath);
            XmlAssert.AreEqual(expected, output, "");
        }

        /// <summary>
        ///A test for GoBibleRearrangeVerseAlphaNumericTest
        ///</summary>
        [Test]
        public void GoBibleRearrangeVerseAlphaNumericTest()
        {
            string filename = "GoBibleRearrangeVerseAlphaNumeric.xhtml";
            string input = GetFileNameWithPath(filename);
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = input;
            preExportProcess = new PreExportProcess(projInfo);
            string expected = GetFileNameWithExpectedPath(filename);
            string output = preExportProcess.GoBibleRearrangeVerseNumbers(projInfo.DefaultXhtmlFileWithPath);
            XmlAssert.AreEqual(expected, output, "");
        }


        /// <summary>
        ///A test for XelatexXhtmlFileDivLetterAddingLangAttributeTest
        ///</summary>
        [Test]
        public void XelatexXhtmlFileDivLetterAddingLangAttributeTest()
        {
            string filename = "XelatexXhtmlFileDivLetterAddingLangAttribute.xhtml";
            string input = GetFileNameWithPath(filename);
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = input;
            preExportProcess = new PreExportProcess(projInfo);
            string expected = GetFileNameWithExpectedPath(filename);
            string outputFile = GetFileNameWithOutputPath(filename);
            File.Copy(projInfo.DefaultXhtmlFileWithPath, outputFile, true);
            string output = preExportProcess.SetLangforLetter(outputFile);
            XmlAssert.AreEqual(expected, output, "");
        }


        /// <summary>
        ///A test for ParagraphVerserSetUp
        ///</summary>
        [Test]
        public void ParagraphVerserSetUpTest()
        {
            XmlDocument xmldoc = Common.DeclareXMLDocument(false);
            string fileName = "ParaChapBorder.xhtml";
            string input = GetFileNameWithPath(fileName);
            string output = GetFileNameWithOutputPath(fileName);
            CopyToOutput(input, output);
            string expected = GetFileNameWithExpectedPath(fileName);
            xmldoc.Load(output);
            preExportProcess = new PreExportProcess();
            preExportProcess.ParagraphVerserSetUp(xmldoc);
            xmldoc.Save(output);
            TextFileAssert.AreEqual(expected, output);
        }

        /// <summary>
        /// Test removal of @top-left, @top-center and @top-right from @page
        /// </summary>
        [Test]
        public void RemoveHeaderStylesTest()
        {
            const string fileName = "RemoveHeaderStyle.css";
            var input = GetFileNameWithPath(fileName);
            var output = GetFileNameWithOutputPath(fileName);
            CopyToOutput(input, output);
            preExportProcess = new PreExportProcess();
            preExportProcess.RemoveDeclaration(output, "@top-");
            var sr = new StreamReader(output);
            var data = sr.ReadToEnd();
            sr.Close();
            var index = data.IndexOf("@top-", 0, StringComparison.CurrentCultureIgnoreCase);
            Assert.AreEqual(-1, index);
        }
        #region private Methods
        private static string GetPath(string place, string filename)
        {
            return Common.PathCombine(GetTestPath(), Common.PathCombine(place, filename));
        }
        private static string GetTestPath()
        {
            return PathPart.Bin(Environment.CurrentDirectory, "/PsTool/TestFiles/");
        }
        private static string GetFileNameWithPath(string fileName)
        {
            return Common.DirectoryPathReplace(GetPath("InputFiles", fileName));
        }
        private static string GetFileNameWithOutputPath(string fileName)
        {
            return Common.DirectoryPathReplace(GetPath("Output", fileName));
        }
        private static string GetFileNameWithExpectedPath(string fileName)
        {
            return Common.DirectoryPathReplace(GetPath("Expected", fileName));
        }
        private static void CopyToOutput(string input, string output)
        {
            if (File.Exists(input))
                File.Copy(input, output, true);
        }
        #endregion
    }
}
