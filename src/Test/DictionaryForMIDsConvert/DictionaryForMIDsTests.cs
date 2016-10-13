// --------------------------------------------------------------------------------------------
// <copyright file="DictionaryForMIDsTest.cs" from='2013' to='2014' company='SIL International'>
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
// Test methods of DictionaryForMIDsConvert
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Diagnostics;
using System.IO;
using System.Collections;
using Microsoft.Win32;
using NUnit.Framework;
using System;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.DictionaryForMIDsConvert
{
    ///<summary>
    ///This is a test class for CheckCV_XHTMLTest
    ///</summary>
    [TestFixture]
    public class DictionaryForMIDsTest : ExportDictionaryForMIDs
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
        public void ExportNullTest()
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
            Assert.AreEqual("dagol  ", rec.Rec);
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
            Assert.AreEqual(@"{{\[sample \] ", rec.Rec);
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
            rec.AddReversal(sense, "definition");
            Assert.AreEqual("\tcantar", rec.Rec);
        }

        [Test]
        public void VernacularIsoTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("sena3-imba.xhtml");
            projInfo.IsLexiconSectionExist = true;
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
            projInfo.IsLexiconSectionExist = true;
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
            projInfo.IsLexiconSectionExist = true;
            var input = new DictionaryForMIDsInput(projInfo);
            var result = input.AnalysisIso();
            Assert.AreEqual("pt", result);
        }

        [Test]
        public void AnalysisNameTest()
        {
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Input("sena3-imba.xhtml");
            projInfo.IsLexiconSectionExist = true;
            var input = new DictionaryForMIDsInput(projInfo);
            var result = input.AnalysisName();
            Assert.AreEqual("Portuguese", result);
        }

        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void CreateDictionaryForMIDsTest()
        {
            Common.Testing = true;
            Common.ProgInstall = Environment.CurrentDirectory;
            var outDir = _testFiles.Output("CreateDictionaryForMIDs");
            if (Directory.Exists(outDir))
            {
                Directory.Delete(outDir, true);
            }
            Directory.CreateDirectory(outDir);

            const string main = "main.txt";
            const string props = "DictionaryForMIDs.properties";
            File.Copy(_testFiles.Input(main), Common.PathCombine(outDir, main));
            File.Copy(_testFiles.Input(props), Common.PathCombine(outDir, props));

            PublicationInformation projInfo = new PublicationInformation();
            projInfo.IsLexiconSectionExist = true;
            projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outDir, "main.xhtml");
            var curTesting = Common.Testing;
            Common.Testing = false;
	        _isUnixOS = Common.UsingMonoVM;
            CreateDictionaryForMIDs(projInfo);
            Assert.True(Directory.Exists(Common.PathCombine(outDir, "DfM_lojen_SIL")));
            Common.Testing = curTesting;
        }

        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void CleanUpTest()
        {
            Common.Testing = true;
            const string createFolder = "CreateDictionaryForMIDs";
            var outDir = _testFiles.Output(createFolder);
            if (Directory.Exists(outDir))
            {
                Directory.Delete(outDir, true);
            }
            var inDir = _testFiles.Input(createFolder);
            FolderTree.Copy(inDir, outDir);
            CleanUp(Common.PathCombine(outDir, "main.xhtml"));
            Assert.True(Directory.Exists(Common.PathCombine(outDir, "DfM_lojen_SIL")));
            Assert.False(Directory.Exists(Common.PathCombine(outDir, "dictionary")));
            Assert.False(Directory.Exists(Common.PathCombine(outDir, "Empty_Jar-Jad")));
            Assert.True(File.Exists(Common.PathCombine(outDir, "Convert.log")));
            Assert.True(File.Exists(Common.PathCombine(outDir, "DfM copyright notice.txt")));
            Assert.True(File.Exists(Common.PathCombine(outDir, "DictionaryForMIDs.properties")));
            Assert.True(File.Exists(Common.PathCombine(outDir, "main.txt")));
            Assert.False(File.Exists(Common.PathCombine(outDir, "DfM-Creator.jar")));
            Assert.False(File.Exists(Common.PathCombine(outDir, "go.bat")));
        }

        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void LaunchTest()
        {
            Common.Testing = true;
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Copy("sena3-imba.xhtml");
            projInfo.DefaultCssFileWithPath = _testFiles.Copy("sena3-imba.css");
            projInfo.IsLexiconSectionExist = true;
            Launch("dictionary", projInfo);
            Assert.True(File.Exists(_testFiles.Output("DfM copyright notice.txt")));
            TextFileAssert.AreEqual(_testFiles.Expected("main.txt"), _testFiles.Output("main.txt"), "main.txt");
            TextFileAssert.AreEqualEx(_testFiles.Expected("DictionaryForMIDs.properties"), _testFiles.Output("DictionaryForMIDs.properties"), new ArrayList{ 1 }, "DictionaryForMIDs.properties");
        }

        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void MoveJarFileTest()
        {
            Common.Testing = true;
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Copy("sena3-imba.xhtml");
            FolderTree.Copy(_testFiles.Input("DfM_enseh_SIL"), _testFiles.Output("DfM_enseh_SIL"));
            MoveJarFile(projInfo);
            Assert.True(File.Exists(_testFiles.Output("DfM_enseh_SIL.jar")));
        }

        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void CreateSubmissionTest()
        {
            Common.Testing = true;
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = _testFiles.Copy("sena3-imba.xhtml");
            FolderTree.Copy(_testFiles.Input("DfM_enseh_SIL"), _testFiles.Output("DfM_enseh_SIL"));
            CreateSubmission(projInfo);
            var dirInfo = new DirectoryInfo(_testFiles.Output(""));
            Assert.AreEqual(1, dirInfo.GetFiles("DictionaryForMIDs_*_enseh_SIL.zip").Length);
        }
    }
}