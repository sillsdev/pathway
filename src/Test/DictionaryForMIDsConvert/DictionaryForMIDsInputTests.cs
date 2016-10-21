// --------------------------------------------------------------------------------------------
// <copyright file="DictionaryForMIDsInputTest.cs" from='2016' to='2016' company='SIL International'>
//      Copyright ( c ) 2016, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Test methods of DictionaryForMIDsConvert
// </remarks>
// --------------------------------------------------------------------------------------------

using NUnit.Framework;
using System;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.DictionaryForMIDsConvert
{
    ///<summary>
    ///This is a test class for testing DictionaryForMidsInput methods
    ///</summary>
    [TestFixture]
    public class DictionaryForMIDsInputTest : ExportDictionaryForMIDs
    {
        #region Setup

        private TestFiles _testFiles;

        [TestFixtureSetUp]
        public void Setup()
        {
            _testFiles = new TestFiles("DictionaryForMIDsConvert");
            Common.ProgInstall = Environment.CurrentDirectory;
        }
        #endregion Setup

        /// <summary>
        /// Test passing null to constructor
        /// </summary>
        [Test]
        [ExpectedException("System.NullReferenceException")]
        public void DictionaryForMIDsInputNullTest()
        {
			new DictionaryForMIDsInput(null);
        }

        /// <summary>
        /// Test FieldWorks 8.3 detect
        /// </summary>
        [Test]
        public void Fw83Test()
        {
            var projInfo = new PublicationInformation{ DefaultXhtmlFileWithPath = _testFiles.Input("w1.xhtml") }; 
            var result = new DictionaryForMIDsInput(projInfo);
            Assert.IsTrue(result.Fw83());
        }

        /// <summary>
        /// Test before FieldWorks 8.3
        /// </summary>
        [Test]
        public void Fw82Test()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("wasp.xhtml") };
            var result = new DictionaryForMIDsInput(projInfo);
            Assert.IsFalse(result.Fw83());
        }

        /// <summary>
        /// Test SelectNodes
        /// </summary>
        [Test]
        public void SelectNodesTest()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("wasp.xhtml") };
            var result = new DictionaryForMIDsInput(projInfo);
            var nodes = result.SelectNodes("//*[@class='sense']");
            Assert.IsNotNull(nodes);
            Assert.AreEqual(5,nodes.Count);
        }

        /// <summary>
        /// Test VernacularIso
        /// </summary>
        [Test]
        public void VernacularIsoTest()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("wasp.xhtml"), IsLexiconSectionExist = true};
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("bzh", input.VernacularIso());
        }

        /// <summary>
        /// Test VernacularIso for reversal
        /// </summary>
        [Test]
        public void VernacularIsoOfRevTest()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("FlexRev.xhtml"), IsLexiconSectionExist = false };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("en", input.VernacularIso());
        }

        /// <summary>
        /// Test VerancularIso for Fw 8.3
        /// </summary>
        [Test]
        public void VernacularIsoFw83Test()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("w1.xhtml"), IsLexiconSectionExist = true };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("bzh", input.VernacularIso());
        }

        /// <summary>
        /// Test VernacularISO for Fw 8.3 reversal
        /// </summary>
        [Test]
        public void VernacularIsoOfFw83RevTest()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("FlexRev83.xhtml"), IsLexiconSectionExist = false };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("en", input.VernacularIso());
        }

        /// <summary>
        /// Test VernacularName
        /// </summary>
        [Test]
        public void VernacularNameTest()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("wasp.xhtml"), IsLexiconSectionExist = true };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("Buang, Mapos", input.VernacularName());
        }

        /// <summary>
        /// Test VernacularName for reversal
        /// </summary>
        [Test]
        public void VernacularNameOfRevTest()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("FlexRev.xhtml"), IsLexiconSectionExist = false };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("English", input.VernacularName());
        }

        /// <summary>
        /// Test VerancularIso for Fw 8.3
        /// </summary>
        [Test]
        public void VernacularNameFw83Test()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("w1.xhtml"), IsLexiconSectionExist = true };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("Buang, Mapos", input.VernacularName());
        }

        /// <summary>
        /// Test VernacularName for Fw 8.3 reversal
        /// </summary>
        [Test]
        public void VernacularNameOfFw83RevTest()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("FlexRev83.xhtml"), IsLexiconSectionExist = false };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("English", input.VernacularName());
        }

        /// <summary>
        /// Test AnalysisIso
        /// </summary>
        [Test]
        public void AnalysisIsoTest()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("wasp.xhtml"), IsLexiconSectionExist = true };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("en", input.AnalysisIso());
        }

        /// <summary>
        /// Test AnalysisIso for reversal
        /// </summary>
        [Test]
        public void AnalysisIsoOfRevTest()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("FlexRev.xhtml"), IsLexiconSectionExist = false };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("seh", input.AnalysisIso());
        }

        /// <summary>
        /// Test VerancularIso for Fw 8.3
        /// </summary>
        [Test]
        public void AnalysisIsoFw83Test()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("w1.xhtml"), IsLexiconSectionExist = true };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("en", input.AnalysisIso());
        }

        /// <summary>
        /// Test AnalysisISO for Fw 8.3 reversal
        /// </summary>
        [Test]
        public void AnalysisIsoOfFw83RevTest()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("FlexRev83.xhtml"), IsLexiconSectionExist = false };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("bzh", input.AnalysisIso());
        }

        /// <summary>
        /// Test AnalysisName
        /// </summary>
        [Test]
        public void AnalysisNameTest()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("wasp.xhtml"), IsLexiconSectionExist = true };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("English", input.AnalysisName());
        }

        /// <summary>
        /// Test AnalysisName for reversal
        /// </summary>
        [Test]
        public void AnalysisNameOfRevTest()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("FlexRev.xhtml"), IsLexiconSectionExist = false };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("Sena", input.AnalysisName());
        }

        /// <summary>
        /// Test VerancularIso for Fw 8.3
        /// </summary>
        [Test]
        public void AnalysisNameFw83Test()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("w1.xhtml"), IsLexiconSectionExist = true };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("English", input.AnalysisName());
        }

        /// <summary>
        /// Test AnalysisName for Fw 8.3 reversal
        /// </summary>
        [Test]
        public void AnalysisNameOfFw83RevTest()
        {
            var projInfo = new PublicationInformation { DefaultXhtmlFileWithPath = _testFiles.Input("FlexRev83.xhtml"), IsLexiconSectionExist = false };
            var input = new DictionaryForMIDsInput(projInfo);
            Assert.AreEqual("Buang, Mapos", input.AnalysisName());
        }

    }
}