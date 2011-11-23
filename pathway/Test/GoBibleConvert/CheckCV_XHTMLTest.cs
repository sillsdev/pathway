// --------------------------------------------------------------------------------------------
// <copyright file="CheckCV_XHTMLTest.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Larry Waswick</author>
// <email>larry_waswick@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Test methods of FlexDePlugin
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Xml;
using NUnit.Framework;
using System;
using SIL.PublishingSolution;
using SIL.Tool;
using NMock2;

namespace Test.GoBibleConvert
{
    // ideas borrowed from PsTool CommonTest.cs and CommonXmlTest.cs
    ///<summary>
    ///This is a test class for CheckCV_XHTMLTest
    ///</summary>
    [TestFixture]
    public class CheckCV_XHTMLTest : ExportGoBible
    {
        private readonly Mockery mocks = new Mockery();
        #region Setup

        private TestFiles _testFiles;

        [TestFixtureSetUp]
        public void Setup()
        {
            _testFiles = new TestFiles("GoBibleConvert");
        }
        #endregion Setup

        ///<summary>
        ///A test for XhtmlCheck
        ///</summary>
        [Test]
        [Ignore("Hangs Team City")]
        public void XhtmlCheck()
        {
            const string fileName = "1pe.xhtml";
            // Get "1pe.xhtml" from the InputFiles folder.
            string inputFileName = _testFiles.Input(fileName);
            Debug.Assert(File.Exists(inputFileName));
            // The original XSLT file is in the folder "C:\SIL\btai\PublishingSolution\PublishingSolutionExe\bin\Debug".
            // A copy of it has been put into the InputFiles folder.
            // Common.XsltProcress creates an output file in the same folder as the input file.
            // Therefore, since we want 1pe_cv.xhtml to be created in the output folder,
            // 1pe.xhtml must be moved to the output folder, also.
            string outputFileName = _testFiles.Output(fileName);
            File.Copy(inputFileName, outputFileName, true);
            Debug.Assert(File.Exists(outputFileName));
            const string xsltName = "TE_XHTML-to-Phone_XHTML.xslt";
            string xsltFullName = GetFileNameWithSupportPath(xsltName);
            Debug.Assert(File.Exists(xsltFullName));

            // Transform the original XHTML file.
            string extension = "_cv.xhtml";
            string actualXhtmlOutput = Common.XsltProcess(outputFileName, xsltFullName, extension);
            Debug.Assert(File.Exists(actualXhtmlOutput));

            // Compare the newly transformed file with "1pe_cv.xhtml" from the Expected folder.
            string expectedXhtmlFile = _testFiles.Expected("1pe_cv.xhtml");
            XmlAssert.AreEqual(expectedXhtmlFile, actualXhtmlOutput, "1pe_cv.xhtml was not transformed correctly");
        }

        [Test]
        [Ignore("Hangs Team City")]
        public void RestructureTest()
        {
            const string fileName = "1pe.xhtml";
            const string restructuredFileName = "1pe_cv.xhtml";
            string inputFullName = _testFiles.Input(fileName);
            Debug.Assert(File.Exists(inputFullName));
            string outputFullName = _testFiles.Output(fileName);
            File.Copy(inputFullName, outputFullName, true);
            Debug.Assert(File.Exists(outputFullName));
            const string cssName = "1pe.css";
            string cssFullName = _testFiles.Input(cssName);
            Debug.Assert(File.Exists(cssFullName));
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = outputFullName;
            projInfo.DefaultCssFileWithPath = cssFullName;
            IInProcess inProcess = mocks.NewMock<IInProcess>();
            Expect.Once.On(inProcess).Method("AddToMaximum").With(5);
            Expect.Once.On(inProcess).Method("Bar").WithNoArguments();
            Restructure(projInfo, inProcess);
            Assert.AreEqual(Path.GetDirectoryName(outputFullName), processFolder);
            string expectedRestructuredFullName = _testFiles.Expected(restructuredFileName);
            Assert.AreEqual(Path.GetFileName(expectedRestructuredFullName), Path.GetFileName(restructuredFullName));
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CreateCollectionTest()
        {
            const string fileName = "1pe_cv.xhtml";
            string inputFullName = _testFiles.Input(fileName);
            restructuredFullName = _testFiles.Output(fileName);
            File.Copy(inputFullName, restructuredFullName, true);
            processFolder = _testFiles.Output("");
			Common.ProgBase = GetSupportPath();
            Param.LoadSettings();
            Param.SetValue(Param.InputType, "Scripture");
            Param.LoadSettings();
            // setup - ensure that there is a current organization in the StyleSettings xml
            XmlNode node = Param.GetItem("//stylePick/settings/property[@name='Organization']");
            if (node == null)
            {
                // node doesn't exist yet - create it now
                XmlNode baseNode = Param.GetItem("//stylePick/settings");
                var childNode = Param.xmlMap.CreateNode(XmlNodeType.Element, "property", "");
                Param.AddAttrValue(childNode, "name", "Organization");
                Param.AddAttrValue(childNode, "value", "SIL International");
                baseNode.AppendChild(childNode);
                Param.Write();
            }
            const string layout = "Go Bible";
            Param.UpdateMobileAtrrib("FileProduced", "OneperBook", layout);
            Param.UpdateMobileAtrrib("RedLetter", "Yes", layout);
            Param.UpdateMetadataValue(Param.Description, "Sena 3");
            Param.UpdateMetadataValue(Param.CopyrightHolder, "© 2010 SIL");
            Param.UpdateMobileAtrrib("Icon", @"C:\ProgramData\SIL\Pathway\Scripture\Icon.png", layout);
            Param.SetValue(Param.LayoutSelected, layout);
            Param.Write();
            const string origFileName = "1pe.xhtml";
            IPublicationInformation projInfo = mocks.NewMock<IPublicationInformation>();
            Expect.Exactly(2).On(projInfo).GetProperty("DefaultXhtmlFileWithPath").Will(Return.Value(_testFiles.Input(origFileName)));
            collectionName = GetCollectionName(projInfo);
            CreateCollection();
            const string collections = "Collections.txt";
            string actualFullName = _testFiles.Output(collections);
            string exepectedFullName = _testFiles.Expected(collections);
            TextFileAssert.AreEqualEx(exepectedFullName, actualFullName, new ArrayList {1});
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void BuildApplicationTest()
        {
            const string fileName = "1pe_cv.xhtml";
            const string collections = "Collections.txt";
            const string jarFile = "1Pita.jar";
            string inputFullName = _testFiles.Input(fileName);
            restructuredFullName = _testFiles.Output(fileName);
            File.Copy(inputFullName, restructuredFullName, true);
            string collectionInputFullName = _testFiles.Input(collections);
            collectionFullName = _testFiles.Output(collections);
            File.Copy(collectionInputFullName, collectionFullName, true);
            processFolder = _testFiles.Output("");
			Common.ProgBase = GetSupportPath();
            BuildApplication();
            string actualFullName = _testFiles.Output(jarFile);
            string exepectedFullName = _testFiles.Expected(jarFile);
			Assert.IsTrue(File.Exists(actualFullName));
			// TODO: for mono, FastZip is not creating subdirectories correctly when unpacking
			// (see http://community.sharpdevelop.net/forums/p/9187/25577.aspx)
			if (!Common.UsingMonoVM)
			{
            	GoBibleTest.AreEqual(exepectedFullName, actualFullName);
			}
        }

        [Test]
        [Category("SkipOnTeamCity")]
        public void BuildApplication2Test()
        {
            const string fileName = "1pe_cv.xhtml";
            const string collections = "Collections.txt";
            const string jarFile = "1Pita.jar";
            string inputFullName = _testFiles.Input(fileName);
            string folderWithSpace = _testFiles.Output("with space");
            if (Directory.Exists(folderWithSpace))
                Directory.Delete(folderWithSpace, true);
            Directory.CreateDirectory(folderWithSpace);
            restructuredFullName = Path.Combine(folderWithSpace, fileName);
            File.Copy(inputFullName, restructuredFullName, true);
            string collectionInputFullName = _testFiles.Input(collections);
            collectionFullName = Path.Combine(folderWithSpace, collections);
            File.Copy(collectionInputFullName, collectionFullName, true);
            processFolder = _testFiles.Output("");
			Common.ProgBase = GetSupportPath();
            BuildApplication();
            string actualFullName = Path.Combine(folderWithSpace, jarFile);
            string exepectedFullName = _testFiles.Expected(jarFile);
			Assert.IsTrue(File.Exists(actualFullName));
			// TODO: for mono, FastZip is not creating subdirectories correctly when unpacking
			// (see http://community.sharpdevelop.net/forums/p/9187/25577.aspx)
			if (!Common.UsingMonoVM)
			{
            	GoBibleTest.AreEqual(exepectedFullName, actualFullName);
			}
        }

        [Test]
        public void ChaptersTest()
        {
            const string fileName = "1pe.xhtml";
            string inputFullName = _testFiles.Input(fileName);
            int actual = Chapters(inputFullName);
            const int expected = 5;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DuplicateBooks 
        ///</summary>
        [Test]
        public void IsDuplicateBooksTest()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml("<books><book title='Korin'/><book title='Korin'/></books>");
            XmlNodeList books = xmlDocument.SelectNodes("//book/@title");
            bool result = IsDuplicateBooks(books);
            Assert.IsTrue(result);
        }

        /// <summary>
        ///A test for DuplicateBooks is false
        ///</summary>
        [Test]
        public void NotIsDuplicateBooksTest()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml("<books><book title='1 Korin'/><book title='2 Korin'/></books>");
            XmlNodeList books = xmlDocument.SelectNodes("//book/@title");
            bool result = IsDuplicateBooks(books);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Test if project name can be estracted from PublicationInformation
        /// </summary>
        [Test]
        public void GetProjectNameTest()
        {
            const string fileName = "1pe.xhtml";
            string inputFullName = _testFiles.Input(fileName);
            IPublicationInformation projInfo = mocks.NewMock<IPublicationInformation>();
            Expect.Exactly(1).On(projInfo).GetProperty("DefaultXhtmlFileWithPath").Will(Return.Value(inputFullName));
            var result = GetProjectName(projInfo);
            Assert.AreEqual("TestFiles", result);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Test if project name can be estracted from PublicationInformation
        /// </summary>
        [Test]
        public void GetBookCode1Test()
        {
            const string fileName = "1pe.xhtml";
            string inputFullName = _testFiles.Input(fileName);
            IPublicationInformation projInfo = mocks.NewMock<IPublicationInformation>();
            Expect.Exactly(1).On(projInfo).GetProperty("DefaultXhtmlFileWithPath").Will(Return.Value(inputFullName));
            var result = GetBookCode(projInfo);
            Assert.AreEqual("1Pe", result);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Test if project name can be estracted from PublicationInformation
        /// </summary>
        [Test]
        public void GetCollectionNameTest()
        {
            const string origFileName = "1pe.xhtml";
            IPublicationInformation projInfo = mocks.NewMock<IPublicationInformation>();
            Expect.Exactly(2).On(projInfo).GetProperty("DefaultXhtmlFileWithPath").Will(Return.Value(_testFiles.Input(origFileName)));
            collectionName = GetCollectionName(projInfo);
            Assert.AreEqual("TestFiles_1Pe", collectionName);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Test if project name can be estracted from PublicationInformation
        /// </summary>
        [Test]
        public void GetCollectionName2Test()
        {
            const string origFileName = "luke.xhtml";
            IPublicationInformation projInfo = mocks.NewMock<IPublicationInformation>();
            Expect.Exactly(3).On(projInfo).GetProperty("DefaultXhtmlFileWithPath").Will(Return.Value(_testFiles.Input(origFileName)));
            collectionName = GetCollectionName(projInfo);
            Assert.AreEqual("TestFiles_Matthew", collectionName);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Test when book data is not nested under book tag.
        /// </summary>
        [Test]
        public void BookDataNestedFalseTest()
        {
            const string origFileName = "luke.xhtml";
            IPublicationInformation projInfo = mocks.NewMock<IPublicationInformation>();
            Expect.Once.On(projInfo).GetProperty("DefaultXhtmlFileWithPath").Will(Return.Value(_testFiles.Input(origFileName)));
            var result = BookDataNested(projInfo);
            Assert.IsFalse(result);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Test when book data is nested under book tag.
        /// </summary>
        [Test]
        public void BookDataNestedTrueTest()
        {
            const string origFileName = "1pe.xhtml";
            IPublicationInformation projInfo = mocks.NewMock<IPublicationInformation>();
            Expect.Once.On(projInfo).GetProperty("DefaultXhtmlFileWithPath").Will(Return.Value(_testFiles.Input(origFileName)));
            var result = BookDataNested(projInfo);
            Assert.IsTrue(result);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Nest book data under div with scrBook class.
        /// </summary>
        [Test]
        public void NestBookDataTest()
        {
            const string origFileName = "luke.xhtml";
            string workingCopy = _testFiles.Output(origFileName);
            File.Copy(_testFiles.Input(origFileName),workingCopy, true);
            IPublicationInformation projInfo = mocks.NewMock<IPublicationInformation>();
            Expect.Exactly(3).On(projInfo).GetProperty("DefaultXhtmlFileWithPath").Will(Return.Value(workingCopy));
            NestBookData(projInfo);
            var result = BookDataNested(projInfo);
            Assert.IsTrue(result);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        #region private Methods
        private static string GetSupportPath()
        {
            return PathPart.Bin(Environment.CurrentDirectory, "/../PsSupport");
        }
        private string GetFileNameWithSupportPath(string name)
        {
            return Common.PathCombine(GetSupportPath(), name);
        }

        #endregion
    }
}