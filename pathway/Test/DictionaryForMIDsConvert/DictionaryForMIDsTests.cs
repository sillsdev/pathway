// --------------------------------------------------------------------------------------------
// <copyright file="DictionaryForMIDsTest.cs" from='2013' to='2013' company='SIL International'>
//      Copyright © 2013, SIL International. All Rights Reserved.   
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

using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using NUnit.Framework;
using System;
using SIL.PublishingSolution;
using SIL.Tool;
using NMock2;

namespace Test.DictionaryForMIDsConvert
{
    ///<summary>
    ///This is a test class for CheckCV_XHTMLTest
    ///</summary>
    [TestFixture]
    public class DictionaryForMIDsTest : ExportDictionaryForMIDs
    {
        private readonly Mockery mocks = new Mockery();
        #region Setup

        private TestFiles _testFiles;

        [TestFixtureSetUp]
        public void Setup()
        {
            _testFiles = new TestFiles("DictionaryForMIDsConvert");
        }
        #endregion Setup

        [Test]
        public void ExportTypeTest()
        {
            Assert.AreEqual("DictionaryForMIDs", ExportType);
        }

        [Test]
        public void HandleTest()
        {
            Assert.IsTrue(Handle("dictionary"));
            Assert.IsFalse(Handle("scripture"));
        }

        [Test]
        public void ExportTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            Assert.IsFalse(Export(projInfo));
        }

        [Test]
        public void AddHeadwordTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("sena3-imba.xhtml");
            var input = new DictionaryForMIDsInput(projInfo);
            var sense = input.SelectNodes("//*[@class = 'entry']//*[@id]")[0];
            var rec = new DictionaryForMIDsRec();
            rec.AddHeadword(sense);
            Assert.AreEqual("imba  ", rec.Rec);
        }

        [Test]
        public void AddHeadwordWithPicturePresentTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("hornbill.xhtml");
            var input = new DictionaryForMIDsInput(projInfo);
            var sense = input.SelectNodes("//*[@class = 'entry']//*[@id]")[0];
            var rec = new DictionaryForMIDsRec();
            rec.AddHeadword(sense);
            Assert.AreEqual("daġöl  ", rec.Rec);
        }

        [Test]
        public void AddBeforeSenseTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("hornbill.xhtml");
            var input = new DictionaryForMIDsInput(projInfo);
            var sense = input.SelectNodes("//*[@class = 'entry']//*[@id]")[0];
            var rec = new DictionaryForMIDsRec();
            rec.AddBeforeSense(sense);
            Assert.AreEqual(@"{{\[ⁿda.ˈᵑɢɔl̪\] ", rec.Rec);
        }

        [Test]
        public void AddAfterSenseTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("hornbill.xhtml");
            var input = new DictionaryForMIDsInput(projInfo);
            var sense = input.SelectNodes("//*[@class = 'entry']//*[@id]")[0];
            var rec = new DictionaryForMIDsRec();
            rec.AddAfterSense(sense);
            Assert.AreEqual(" }}", rec.Rec);
        }

        [Test]
        public void AddReversalTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("sena3-imba.xhtml");
            var input = new DictionaryForMIDsInput(projInfo);
            var sense = input.SelectNodes("//*[@class = 'entry']/xhtml:div")[0];
            var rec = new DictionaryForMIDsRec();
            rec.AddReversal(sense);
            Assert.AreEqual("\tcantar", rec.Rec);
        }

        [Test]
        public void VernacularIsoTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("sena3-imba.xhtml");
            var input = new DictionaryForMIDsInput(projInfo);
            var result = input.VernacularIso();
            Assert.AreEqual("seh", result);
        }

        [Test]
        public void VernacularNameTest()
        {
            const string sKey = @"Software\SIL\Pathway";
            const string keyName = "WritingSystemStore";
            var lgFullPath = _testFiles.Output("seh.ldml");
            var lgDirectory = Path.GetDirectoryName(lgFullPath);
            Debug.Assert(!string.IsNullOrEmpty(lgDirectory));
            RegistryKey oKey = Registry.CurrentUser.OpenSubKey(sKey, true);
            RegistryKey myKey = oKey ?? Registry.CurrentUser.CreateSubKey(sKey);
            Debug.Assert(myKey != null);
            var oVal = (oKey == null) ? null : oKey.GetValue(keyName, null);
            myKey.SetValue(keyName, lgDirectory);
            var wr = new StreamWriter(lgFullPath);
            wr.Write(@"<?xml version=""1.0"" encoding=""utf-8""?>
                <ldml><special xmlns:palaso=""urn://palaso.org/ldmlExtensions/v1"">
                        <palaso:languageName value=""Sena"" />
                </special></ldml>");
            wr.Close();
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("sena3-imba.xhtml");
            var input = new DictionaryForMIDsInput(projInfo);
            var result = input.VernacularName();
            if (oKey == null)
            {
                Registry.CurrentUser.DeleteSubKey(sKey);
            }
            else if (oVal == null)
            {
                oKey.DeleteValue(keyName);
            }
            else
            {
                oKey.SetValue(keyName, oVal);
            }
            Assert.AreEqual("Sena", result);
        }

        [Test]
        public void AnalysisIsoTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("sena3-imba.xhtml");
            var input = new DictionaryForMIDsInput(projInfo);
            var result = input.AnalysisIso();
            Assert.AreEqual("pt", result);
        }

        [Test]
        public void AnalysisNameTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("sena3-imba.xhtml");
            var input = new DictionaryForMIDsInput(projInfo);
            var result = input.AnalysisName();
            Assert.AreEqual("Portuguese", result);
        }

        [Test]
        public void CreateDictionaryForMIDsTest()
        {
            var outDir = _testFiles.Output("CreateDictionaryForMIDs");
            if (Directory.Exists(outDir))
            {
                Directory.Delete(outDir, true);
            }
            Directory.CreateDirectory(outDir);

            const string main = "main.txt";
            const string props = "DictionaryForMIDs.properties";
            File.Copy(_testFiles.Input(main), Path.Combine(outDir, main));
            File.Copy(_testFiles.Input(props), Path.Combine(outDir, props));

            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = Path.Combine(outDir, "main.xhtml");
            CreateDictionaryForMIDs(projInfo);
            Assert.True(Directory.Exists(Path.Combine(outDir, "DfM_lojen_SIL")));
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