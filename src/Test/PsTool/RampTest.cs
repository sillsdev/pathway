// --------------------------------------------------------------------------------------------
// <copyright file="RampTest.cs" from='2009' to='2014' company='SIL International'>
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
// Test Ramp files
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SIL.Tool;

namespace Test.PsTool
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Test functions of Ramp package preparation
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    [TestFixture]
    public class RampTest: Ramp
    {
        #region setup
        private static string _inputPath;
        private static string _outputPath;

        [TestFixtureSetUp]
        public void Setup()
        {
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../../DistFiles");
            Common.SupportFolder = "";
            Common.ProgBase = Common.ProgInstall;
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/PsTool/TestFiles");
            _inputPath = Common.PathCombine(testPath, "InputFiles");
            _outputPath = Common.PathCombine(testPath, "output");
            Common.PathCombine(testPath, "Expected");
            if (Directory.Exists(_outputPath))
                Directory.Delete(_outputPath, true);
            Directory.CreateDirectory(_outputPath);
        }

        [SetUp]
        public void TestSetup()
        {
            _fileList.Clear();
            _isoLanguageCode.Clear();
            _isoLanguageCodeandName.Clear();
            _isoLanguageScriptandName.Clear();
            LanguageIso.Clear();
            RampDescriptionHas = null;
            RampDescription = null;
            //var settingsFolder = Common.PathCombine(Common.GetAllUserPath(), "Pathway");
            var settingsFolder = Common.GetAllUserPath();
            if (Directory.Exists(settingsFolder))
            {
                Directory.Delete(settingsFolder, true);
            }
        }
        #endregion setup

        [Test]
        public void SetRampDataModeTest()
        {
            _folderPath = FileInput(Common.PathCombine("rampInput", "Gondwana Sample.xhtml"));
            _projInputType = "Dictionary";
            SetRampData();
            Assert.AreEqual("Text,Graphic", TypeMode);
        }

        [Test]
        public void SetRampDataAudienceTest()
        {
            _folderPath = FileInput(Common.PathCombine("rampInput", "Gondwana Sample.xhtml"));
            _projInputType = "Dictionary";
            SetRampData();
            Assert.AreEqual("wider_audience", BroadType);
        }

        [Test]
        public void AddSubjLanguageTest()
        {
            SubjectLanguage = new List<string>();
            _folderPath = FileInput(Common.PathCombine("rampInput", "Gondwana Sample.xhtml"));
            _projInputType = "Dictionary";
            LoadLanguagefromXML();
            AddSubjLanguage(Common.GetLanguageCode(_folderPath, _projInputType, true));
            Assert.True(SubjectLanguage.Contains("ggo:Gondi, Southern"), "missing Gondwana");
        }

        [Test]
        public void SetRampDataSubjectLanguage2Test()
        {
            _folderPath = FileInput(Common.PathCombine("rampInput2", "sena3.xhtml"));
            _projInputType = "Dictionary";
            LoadLanguagefromXML();
            SetRampData();
            Assert.True(SubjectLanguage.Contains("seh:Sena"), "missing Sena");
        }

        [Test]
        public void AddLanguagIsoTest()
        {
            _folderPath = FileInput(Common.PathCombine("rampInput2", "sena3.xhtml"));
            _projInputType = "Dictionary";
            LoadLanguagefromXML();
            AddLanguageIso(Common.GetLanguageCodeList(_folderPath, _projInputType));
            Assert.AreEqual(3, LanguageIso.Count);
            Assert.True(LanguageIso.Contains("seh:Sena"), "missing Sena");
            Assert.True(LanguageIso.Contains("eng:English"), "missing English");
            Assert.True(LanguageIso.Contains("por:Portuguese"), "missing Portuguese");
        }

        [Test]
        public void AddLanguageScriptTest()
        {
            _folderPath = FileInput(Common.PathCombine("rampInput", "Gondwana Sample.xhtml"));
            _projInputType = "Dictionary";
            LoadLanguagefromXML();
            AddLanguageScript(Common.GetLanguageScriptList(_folderPath, _projInputType));
            Assert.True(LanguageScript.Contains("Telu:Telugu"), "missing Telugu");
        }

        [Test]
        [Category("ShortTest")]
        public void AddContributorTest()
        {
            const string TestFolder = "rampInput";
            _folderPath = FileInput(Common.PathCombine(TestFolder, "Gondwana Sample.xhtml"));
	        _projInputType = Param.SetLoadType = "Dictionary";
			Param.Value[Param.InputType] = "Dictionary";

            SettingsInput(TestFolder);
            SetRampData();
            Assert.True(Contributor.Contains("GOD,compiler"), "should be GOD!");
        }

        [Test]
        [Category("ShortTest")]
        public void RampFormatExtentImagesTest()
        {
            const string TestFolder = "rampInput";
            _folderPath = FileInput(Common.PathCombine(TestFolder, "Gondwana Sample.xhtml"));
            _projInputType = "Dictionary";
            SettingsInput(TestFolder);
            SetRampData();
            Assert.True(FormatExtentImages.Contains("2"), "should be 2!");
        }

        [Test]
        [Category("ShortTest")]
        public void RampRelRequiresTest()
        {
            RelRequires = new List<string>();
            const string TestFolder = "rampInput";
            _folderPath = FileInput(Common.PathCombine(TestFolder, "Gondwana Sample.xhtml"));
            _projInputType = "Dictionary";
            SettingsInput(TestFolder);
            SetRampData();
            Assert.True(RelRequires.Count == 4, "should be 4 fonts!");
        }

        [Test]
        [Category("ShortTest")]
        public void RampRightsTest()
        {
            const string TestFolder = "rampInput";
            _folderPath = FileInput(Common.PathCombine(TestFolder, "Gondwana Sample.xhtml"));
            _projInputType = "Dictionary";
			Param.SetLoadType = "Dictionary";
            SettingsInput(TestFolder);
            SetRampData();
			Assert.True(Rights == "© "+ DateTime.Now.Date.Year + " SIL International®. ");
        }

        [Test]
        [Category("ShortTest")]
        public void RampCreatedOnTest()
        {
            const string TestFolder = "rampInput";
            _folderPath = FileInput(Common.PathCombine(TestFolder, "Gondwana Sample.xhtml"));
            _projInputType = "Dictionary";
            SettingsInput(TestFolder);
            SetRampData();
            Assert.True(CreatedOn.Substring(0,20) == DateTime.Now.ToString("r").Substring(0,20), "Created on is incorrect!");
        }

        [Test]
        [Category("ShortTest")]
        public void RampModifiedDateTest()
        {
            const string TestFolder = "rampInput";
            _folderPath = FileInput(Common.PathCombine(TestFolder, "Gondwana Sample.xhtml"));
            _projInputType = "Dictionary";
            SettingsInput(TestFolder);
            SetRampData();
            Assert.True(ModifiedDate == DateTime.Now.ToString("yyyy-MM-dd"), "Modified on is incorrect!");
        }

        [Test]
        [Category("ShortTest")]
        public void RampDescriptionTest()
        {
            const string TestFolder = "rampInput";
            _folderPath = FileInput(Common.PathCombine(TestFolder, "Gondwana Sample.xhtml"));
            _projInputType = "Dictionary";
			Param.SetLoadType = "Dictionary";
            SettingsInput(TestFolder);
            SetRampData();
            Assert.AreEqual("Just for testing", RampDescription);
            Assert.AreEqual("Y", RampDescriptionHas);
        }

        [Test]
        public void RampDescriptionEmptyTest()
        {
            const string TestFolder = "rampInput2";
            _folderPath = FileInput(Common.PathCombine(TestFolder, "sena3.xhtml"));
            Param.Value[Param.InputType] = "Dictionary";
            SettingsInput(TestFolder);
            SetRampData();
            Assert.AreEqual(null, RampDescription);
            Assert.AreEqual(null, RampDescriptionHas);
        }

        #region Private Functions
        private static void SettingsInput(string TestFolder)
        {
            var settingsFolder = FileInput(Common.PathCombine(TestFolder, "Pathway"));
            if (Directory.Exists(settingsFolder))
            {
                FolderTree.Copy(settingsFolder, Common.GetAllUserPath());
            }
        }

        private static string FileInput(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }
        #endregion PrivateFunctions
    }
}
