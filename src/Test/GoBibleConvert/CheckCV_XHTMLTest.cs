// --------------------------------------------------------------------------------------------
// <copyright file="CheckCV_XHTMLTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
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

using System.Xml;
using NUnit.Framework;
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

        /// <summary>
        ///A test for DuplicateBooks 
        ///</summary>
        [Test]
        [Category("ShortTest")]
        //[Category("SkipOnTeamCity")]
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
        [Category("ShortTest")]
        //[Category("SkipOnTeamCity")]
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
        [Category("ShortTest")]
        //[Category("SkipOnTeamCity")]
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
    }
}