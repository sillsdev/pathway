// --------------------------------------------------------------------------------------------
// <copyright file="UsxToSFMTest.cs" from='2009' to='2014' company='SIL International'>
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
// GoBible Test Support
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.GoBibleConvert
{
    [TestFixture]
    [Category("ShortTest")]
    public class ExportGoBibleTest : ExportThroughPathway
    {

        #region Private Variables
        private string _inputPath;
        private string _outputPath;
        private PublicationInformation _projInfo;
        #endregion

        #region SetUp
        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../../DistFiles");
            Common.SupportFolder = "";
            Common.ProgBase = Common.ProgInstall;
            Common.Testing = true;
            _projInfo = new PublicationInformation();
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/GoBibleConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "Input");
            _outputPath = Common.PathCombine(testPath, "output");

            string pathwayDirectory = PathwayPath.GetPathwayDir();
            string styleSettingFile = Common.PathCombine(pathwayDirectory, "StyleSettings.xml");
            Common.Testing = true;
            ValidateXMLVersion(styleSettingFile);
            InputType = "Scripture";
            Common.ProgInstall = pathwayDirectory;
            Param.LoadSettings();
            Param.SetValue(Param.InputType, InputType);
            Param.LoadSettings();
        }
        #endregion

        private void ValidateXMLVersion(string filePath)
        {
            var versionControl = new SettingsVersionControl();
            var validator = new SettingsValidator();
            if (File.Exists(filePath))
            {
                versionControl.UpdateSettingsFile(filePath);
                bool isValid = validator.ValidateSettingsFile(filePath, true);
                if (!isValid)
                {
                    this.Close();
                }
            }
        }

        /// <summary>
        ///A test for BuildApplication
        ///</summary>
        [Ignore]
        [Category("SkipOnTeamCity")]
        public void BuildApplicationTest()
        {
            ExportGoBible target = new ExportGoBible();
            string goBibleCreatorPath = _inputPath; 
            target.BuildApplication(goBibleCreatorPath);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }


        /// <summary>
        ///A test for CreateCollectionsTextFile
        ///</summary>
        [Ignore]
        [Category("SkipOnTeamCity")]
        public void CreateCollectionsTextFileTest()
        {
            ExportGoBible target = new ExportGoBible();
            string xx = Common.GetApplicationPath();
            string exportGoBiblePath = xx; 
            target.CreateCollectionsTextFile(exportGoBiblePath,string.Empty);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CreateRamp
        ///</summary>
        [Ignore]
        [Category("SkipOnTeamCity")]
        public void CreateRampTest()
        {
            ExportGoBible target = new ExportGoBible(); // TODO: Initialize to an appropriate value
            PublicationInformation projInfo = null; // TODO: Initialize to an appropriate value
            target.CreateRamp(projInfo);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Export
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void ExportTest()
        {
            string inputSourceDirectory = FileInput("ExportGoBible");
            string outputDirectory = FileOutput("ExportGoBible");
            if(Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            FolderTree.Copy(inputSourceDirectory, outputDirectory);
            Param.LoadSettings();
            _projInfo.ProjectPath = outputDirectory;
            _projInfo.ProjectInputType = "Scripture";
            _projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDirectory, "Go_Bible.xhtml");
            _projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDirectory, "Go_Bible.css");
            
            var target = new ExportGoBible();
            const bool expectedResult = true;
            bool actual = target.Export(_projInfo);
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        ///A test for GetInfo
        ///</summary>
        [Ignore]
        [Category("SkipOnTeamCity")]
        public void GetInfoTest()
        {
            ExportGoBible target = new ExportGoBible(); // TODO: Initialize to an appropriate value
            string metadataValue = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetInfo(metadataValue);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetProjectName
        ///</summary>
        [Ignore]
        [Category("SkipOnTeamCity")]
        public void GetProjectNameTest()
        {
            ExportGoBible target = new ExportGoBible(); // TODO: Initialize to an appropriate value
            IPublicationInformation projInfo = null; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetProjectName(projInfo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GoBibleCreatorTempDirectory
        ///</summary>
        [Ignore]
        [Category("SkipOnTeamCity")]
        public void GoBibleCreatorTempDirectoryTest()
        {
            ExportGoBible target = new ExportGoBible(); // TODO: Initialize to an appropriate value
            string goBibleFullPath = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GoBibleCreatorTempDirectory(goBibleFullPath);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Handle
        ///</summary>
        [Ignore]
        [Category("SkipOnTeamCity")]
        public void HandleTest()
        {
            ExportGoBible target = new ExportGoBible(); // TODO: Initialize to an appropriate value
            string inputDataType = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Handle(inputDataType);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }



        private string FileInput(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }

        private string FileOutput(string fileName)
        {
            return Common.PathCombine(_outputPath, fileName);
        }
    }
}
