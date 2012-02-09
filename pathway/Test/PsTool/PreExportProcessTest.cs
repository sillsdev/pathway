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
            projInfo.DefaultXhtmlFileWithPath = input;
            preExportProcess = new PreExportProcess(projInfo);
            string expected = GetFileNameWithExpectedPath(filename);
            string output = preExportProcess.ImagePreprocess();
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
        ///A test for ParagraphVerserSetUp
        ///</summary>
        [Test]
        public void ParagraphVerserSetUpTest()
        {
            XmlDocument xmldoc = new XmlDocument { XmlResolver = null };
            string fileName = "ParaChapBorder.xhtml";
            string input = GetFileNameWithPath(fileName);
            string output = GetFileNameWithOutputPath(fileName);
            CopyToOutput(input, output);
            string expected = GetFileNameWithExpectedPath(fileName);
            xmldoc.Load(output);
            preExportProcess.ParagraphVerserSetUp(xmldoc);
            xmldoc.Save(output);
            TextFileAssert.AreEqual(expected, output);
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
