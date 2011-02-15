// --------------------------------------------------------------------------------------------
// <copyright file="ExportYouVersionTest.cs" from='2011' to='2011' company='SIL International'>
//      Copyright © 2011, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Test methods of ExportYouVersion
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using SIL.PublishingSolution;
using System.Xml;
using SIL.Tool;
using NUnit.Framework;

#endregion Using

namespace Test.YouVersion
{
    
    
    /// <summary>
    ///This is a test class for ExportYouVersionTest and is intended
    ///to contain all ExportYouVersionTest Unit Tests
    ///</summary>
    [TestFixture]
    [Ignore]
    public class ExportYouVersionTest : ExportYouVersion
    {
        /// <summary>
        ///A test for ZipHtml
        ///</summary>
        [Test]
        public void ZipHtmlTest()
        {
            string htmlFolder = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = ZipHtml(htmlFolder);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for WriteChapter
        ///</summary>
        [Test]

        public void WriteChapterTest()
        {
            XmlNode chapterFile = null; // TODO: Initialize to an appropriate value
            string processFolder = string.Empty; // TODO: Initialize to an appropriate value
            string bookCode = string.Empty; // TODO: Initialize to an appropriate value
            string curChapter = string.Empty; // TODO: Initialize to an appropriate value
            WriteChapter(chapterFile, processFolder, bookCode, curChapter);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SplitByChapters
        ///</summary>
        [Test]
        public void SplitByChaptersTest()
        {
            IPublicationInformation projInfo = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = SplitByChapters(projInfo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SetupOutputFolder
        ///</summary>
        [Test]
        public void SetupOutputFolderTest()
        {
            string folder = string.Empty; // TODO: Initialize to an appropriate value
            SetupOutputFolder(folder);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for NestBookData
        ///</summary>
        [Test]
        public void NestBookDataTest()
        {
            IPublicationInformation projInfo = null; // TODO: Initialize to an appropriate value
            NestBookData(projInfo);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Launch
        ///</summary>
        [Test]
        public void LaunchTest()
        {
            ExportYouVersion target = new ExportYouVersion(); // TODO: Initialize to an appropriate value
            string exportType = string.Empty; // TODO: Initialize to an appropriate value
            PublicationInformation publicationInformation = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Launch(exportType, publicationInformation);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Handle
        ///</summary>
        [Test]
        public void HandleTest()
        {
            ExportYouVersion target = new ExportYouVersion(); // TODO: Initialize to an appropriate value
            string inputDataType = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Handle(inputDataType);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetPathFromXhtml
        ///</summary>
        [Test]
        public void GetPathFromXhtmlTest()
        {
            string name = string.Empty; // TODO: Initialize to an appropriate value
            string path = string.Empty; // TODO: Initialize to an appropriate value
            XmlNodeList expected = null; // TODO: Initialize to an appropriate value
            XmlNodeList actual;
            actual = GetPathFromXhtml(name, path);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetXmlDocument
        ///</summary>
        [Test]
        public void GetXmlDocumentTest()
        {
            string name = string.Empty; // TODO: Initialize to an appropriate value
            XmlDocument expected = null; // TODO: Initialize to an appropriate value
            XmlDocument actual;
            actual = GetXmlDocument(name);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ConvertToHtml
        ///</summary>
        [Test]
        public void ConvertToHtmlTest()
        {
            string processFolder = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = ConvertToHtml(processFolder);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CleanUp
        ///</summary>
        [Test]
        public void CleanUpTest()
        {
            string processFolder = string.Empty; // TODO: Initialize to an appropriate value
            string htmlFolder = string.Empty; // TODO: Initialize to an appropriate value
            string zippedResult = string.Empty; // TODO: Initialize to an appropriate value
            CleanUp(processFolder, htmlFolder, zippedResult);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for BookDataNested
        ///</summary>
        [Test]
        public void BookDataNestedTest()
        {
            IPublicationInformation projInfo = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = BookDataNested(projInfo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for AddBookLevelParagraph
        ///</summary>
        [Test]
        public void AddBookLevelParagraphTest()
        {
            XmlDocument chapterFile = null; // TODO: Initialize to an appropriate value
            string classValue = string.Empty; // TODO: Initialize to an appropriate value
            string content = string.Empty; // TODO: Initialize to an appropriate value
            XmlNode chapterFileBody = null; // TODO: Initialize to an appropriate value
            AddBookLevelParagraph(chapterFile, classValue, content, chapterFileBody);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetXmlNamespaceManager
        ///</summary>
        [Test]
        public void GetXmlNamespaceManagerTest()
        {
            XmlDocument xmlDocument = null; // TODO: Initialize to an appropriate value
            XmlNamespaceManager expected = null; // TODO: Initialize to an appropriate value
            XmlNamespaceManager actual;
            actual = GetXmlNamespaceManager(xmlDocument);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
