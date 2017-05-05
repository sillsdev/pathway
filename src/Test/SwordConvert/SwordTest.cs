// --------------------------------------------------------------------------------------------
// <copyright file="SwordTest.cs" from='2009' to='2014' company='SIL International'>
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
// Sword Test Support
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.SwordConvert
{
    [TestFixture]
	[Category("SkipOnTeamCity")]
    public class SwordTest : ExportThroughPathway
    {

        #region Private Variables
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;
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
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/SwordConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "Input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "Expected");

			string pathwayDirectory = Common.AssemblyPath;
            string styleSettingFile = Common.PathCombine(pathwayDirectory, "StyleSettings.xml");

			if (!File.Exists(styleSettingFile))
			{
				styleSettingFile = Path.GetDirectoryName(Common.AssemblyPath);
				styleSettingFile = Common.PathCombine(styleSettingFile, "StyleSettings.xml");
			}

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
                }
            }
        }

        private string FileInput(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }

        private string FileOutput(string fileName)
        {
            return Common.PathCombine(_outputPath, fileName);
        }

        /// <summary>
        ///A test for Export
        ///</summary>
        [Test]
        [Category("SkipOnTeamCity")]
        public void ExportSwordTest()
        {
            string inputSourceDirectory = FileInput("SwordExportTest");
            string outputDirectory = FileOutput("SwordExportTest");
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            FolderTree.Copy(inputSourceDirectory, outputDirectory);
            Param.LoadSettings();
            _projInfo.ProjectPath = outputDirectory;
            _projInfo.ProjectInputType = "Scripture";
            _projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDirectory, "A4_Cols_ApplicationStyles.xhtml");
            _projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDirectory, "A4_Cols_ApplicationStyles.css");

            var target = new ExportSword();
            const bool expectedResult = true;
            bool actual = target.Export(_projInfo);
            Assert.AreEqual(expectedResult, actual);
        }

        ///<summary>
        ///Compare files
        /// </summary>
        [Test]
        [Category("SkipOnTC")]
        public void RutBookTest()
        {
            const string file = "rut";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".xml");
            string expected = Common.PathCombine(_expectedPath, file + ".xml");

            var inputTmpDir = Common.PathCombine(Path.GetTempPath(), Path.GetRandomFileName().Replace(".", "_"));
            Directory.CreateDirectory(inputTmpDir);
            string inputTmpDirFileName = string.Empty;
            inputTmpDirFileName = Common.PathCombine(inputTmpDir, "USX");
            Directory.CreateDirectory(inputTmpDirFileName);
            inputTmpDirFileName = Common.PathCombine(inputTmpDirFileName, Path.GetFileName(input));
            File.Copy(input, inputTmpDirFileName, true);

            ExportSword swordObj = new ExportSword();
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.ProjectPath = inputTmpDir;
            projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(inputTmpDir, "Test.xhtml");
            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            swordObj.OpenOutputDirectory = false;
            swordObj.Export(projInfo);

            string osisOutputFile = Common.PathCombine(inputTmpDir, "OSIS");
            var swordTmpDir = Common.PathCombine(Path.GetTempPath(), "Sword");
            osisOutputFile = Common.PathCombine(osisOutputFile, Path.GetFileName(output));
            if (File.Exists(osisOutputFile))
            {
                File.Copy(osisOutputFile, output, true);
            }

            if (Directory.Exists(swordTmpDir))
            {
                swordObj.CopySwordCreatorFolderToTemp(swordTmpDir, Common.PathCombine(_outputPath, file), null);
            }
            string swordOutputPath = Common.PathCombine(_outputPath, file);
            Common.CleanupExportFolder(Common.PathCombine(swordOutputPath, file + ".xml"), ".exe,.dll", string.Empty, string.Empty);

            if (Directory.Exists(inputTmpDir))
            {
                DirectoryInfo di = new DirectoryInfo(inputTmpDir);
                Common.CleanDirectory(di);
            }

            if (Directory.Exists(swordTmpDir))
            {
                DirectoryInfo di = new DirectoryInfo(swordTmpDir);
                Common.CleanDirectory(di);
            }

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>
        [Test]
        [Category("SkipOnTC")]
        public void JN2BookTest()
        {
            const string file = "2JN";

			string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".xml");
            string expected = Common.PathCombine(_expectedPath, file + ".xml");

            var inputTmpDir = Common.PathCombine(Path.GetTempPath(), Path.GetRandomFileName().Replace(".", "_"));
            Directory.CreateDirectory(inputTmpDir);
            string inputTmpDirFileName = string.Empty;
			inputTmpDirFileName = Common.PathCombine(inputTmpDir, "USX");
            Directory.CreateDirectory(inputTmpDirFileName);
            inputTmpDirFileName = Common.PathCombine(inputTmpDirFileName, Path.GetFileName(input));
            File.Copy(input, inputTmpDirFileName, true);

            ExportSword swordObj = new ExportSword();
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.ProjectPath = inputTmpDir;
            projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(inputTmpDir, "Test.xhtml");
            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            swordObj.OpenOutputDirectory = false;
            swordObj.Export(projInfo);

            string osisOutputFile = Common.PathCombine(inputTmpDir, "OSIS");
            var swordTmpDir = Common.PathCombine(Path.GetTempPath(), "Sword");
            osisOutputFile = Common.PathCombine(osisOutputFile, Path.GetFileName(output));
            if (File.Exists(osisOutputFile))
            {
                File.Copy(osisOutputFile, output, true);
            }

            if (Directory.Exists(swordTmpDir))
            {
                swordObj.CopySwordCreatorFolderToTemp(swordTmpDir, Common.PathCombine(_outputPath, file), null);
            }
            string swordOutputPath = Common.PathCombine(_outputPath, file);
            Common.CleanupExportFolder(Common.PathCombine(swordOutputPath, file + ".xml"), ".exe,.dll", string.Empty, string.Empty);

            if (Directory.Exists(inputTmpDir))
            {
                DirectoryInfo di = new DirectoryInfo(inputTmpDir);
                Common.CleanDirectory(di);
            }

            if (Directory.Exists(swordTmpDir))
            {
                DirectoryInfo di = new DirectoryInfo(swordTmpDir);
                Common.CleanDirectory(di);
            }

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>
        [Test]
        [Category("SkipOnTC")]
        public void EzraBookTest()
        {
            const string file = "EZR";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".xml");
            string expected = Common.PathCombine(_expectedPath, file + ".xml");

            var inputTmpDir = Common.PathCombine(Path.GetTempPath(), Path.GetRandomFileName().Replace(".", "_"));
            Directory.CreateDirectory(inputTmpDir);
            string inputTmpDirFileName = string.Empty;
			inputTmpDirFileName = Common.PathCombine(inputTmpDir, "USX");
            Directory.CreateDirectory(inputTmpDirFileName);
            inputTmpDirFileName = Common.PathCombine(inputTmpDirFileName, Path.GetFileName(input));
            File.Copy(input, inputTmpDirFileName, true);

            ExportSword swordObj = new ExportSword();
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.ProjectPath = inputTmpDir;
            projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(inputTmpDir, "Test.xhtml");
            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            swordObj.OpenOutputDirectory = false;
            swordObj.Export(projInfo);

            string osisOutputFile = Common.PathCombine(inputTmpDir, "OSIS");
            var swordTmpDir = Common.PathCombine(Path.GetTempPath(), "Sword");
            osisOutputFile = Common.PathCombine(osisOutputFile, Path.GetFileName(output));
            if (File.Exists(osisOutputFile))
            {
                File.Copy(osisOutputFile, output, true);
            }

            if (Directory.Exists(swordTmpDir))
            {
                swordObj.CopySwordCreatorFolderToTemp(swordTmpDir, Common.PathCombine(_outputPath, file), null);
            }
            string swordOutputPath = Common.PathCombine(_outputPath, file);
            Common.CleanupExportFolder(Common.PathCombine(swordOutputPath, file + ".xml"), ".exe,.dll", string.Empty, string.Empty);

            if (Directory.Exists(inputTmpDir))
            {
                DirectoryInfo di = new DirectoryInfo(inputTmpDir);
                Common.CleanDirectory(di);
            }

            if (Directory.Exists(swordTmpDir))
            {
                DirectoryInfo di = new DirectoryInfo(swordTmpDir);
                Common.CleanDirectory(di);
            }

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>
        [Test]
        [Category("SkipOnTC")]
        public void JudgesBookTest()
        {
            const string file = "JDG";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".xml");
            string expected = Common.PathCombine(_expectedPath, file + ".xml");

            var inputTmpDir = Common.PathCombine(Path.GetTempPath(), Path.GetRandomFileName().Replace(".", "_"));
            Directory.CreateDirectory(inputTmpDir);
            string inputTmpDirFileName = string.Empty;
			inputTmpDirFileName = Common.PathCombine(inputTmpDir, "USX");
            Directory.CreateDirectory(inputTmpDirFileName);
            inputTmpDirFileName = Common.PathCombine(inputTmpDirFileName, Path.GetFileName(input));
            File.Copy(input, inputTmpDirFileName, true);

            ExportSword swordObj = new ExportSword();
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.ProjectPath = inputTmpDir;
            projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(inputTmpDir, "Test.xhtml");
            projInfo.ProjectFileWithPath = projInfo.ProjectPath;
            swordObj.OpenOutputDirectory = false;
            swordObj.Export(projInfo);

            string osisOutputFile = Common.PathCombine(inputTmpDir, "OSIS");
            var swordTmpDir = Common.PathCombine(Path.GetTempPath(), "Sword");
            osisOutputFile = Common.PathCombine(osisOutputFile, Path.GetFileName(output));
            if (File.Exists(osisOutputFile))
            {
                File.Copy(osisOutputFile, output, true);
            }

            if (Directory.Exists(swordTmpDir))
            {
                swordObj.CopySwordCreatorFolderToTemp(swordTmpDir, Common.PathCombine(_outputPath, file), null);
            }
            string swordOutputPath = Common.PathCombine(_outputPath, file);
            Common.CleanupExportFolder(Common.PathCombine(swordOutputPath, file + ".xml"), ".exe,.dll", string.Empty, string.Empty);

            if (Directory.Exists(inputTmpDir))
            {
                DirectoryInfo di = new DirectoryInfo(inputTmpDir);
                Common.CleanDirectory(di);
            }

            if (Directory.Exists(swordTmpDir))
            {
                DirectoryInfo di = new DirectoryInfo(swordTmpDir);
                Common.CleanDirectory(di);
            }

            FileAssert.AreEqual(expected, output, file + " test fails");
        }


    }
}
