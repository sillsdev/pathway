// --------------------------------------------------------------------------------------------
// <copyright file="ContentXMLTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed:
//
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using NUnit.Framework;
using System.Windows.Forms;
using SIL.PublishingSolution;
using SIL.Tool;

#endregion Using

namespace Test.OpenOfficeConvert
{
    [TestFixture]
    public class LOTest : ExportThroughPathway
    {
        #region Private Variables
        private string _inputPath;
        private string _outputPath;
	    private string _testFolderPath = string.Empty;
        private PublicationInformation _projInfo;
        #endregion

        #region Setup
        [TestFixtureSetUp]
        protected void SetUpAll()
        {
            Common.Testing = true;
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../../DistFiles");
            Common.SupportFolder = "";
            Common.ProgBase = Common.ProgInstall;

            _projInfo = new PublicationInformation();
            _testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/OpenOfficeConvert/TestFiles");
            _inputPath = Common.PathCombine(_testFolderPath, "input");
            _outputPath = Common.PathCombine(_testFolderPath, "output");

            if (Directory.Exists(_outputPath))
                Directory.Delete(_outputPath, true);
            Directory.CreateDirectory(_outputPath);
            _projInfo.ProjectPath = _testFolderPath;
            _projInfo.OutputExtension = "odt";
			string pathwayDirectory = Path.GetDirectoryName(Common.AssemblyPath);
            string styleSettingFile = Common.PathCombine(pathwayDirectory, "StyleSettings.xml");

            ValidateXMLVersion(styleSettingFile);
            Common.ProgInstall = pathwayDirectory;
            Param.LoadSettings();
            Param.SetValue(Param.InputType, "Scripture");
            Param.LoadSettings();
        }

        #endregion Setup

        #region Public Functions

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

        #region Nunits

        /// <summary>
        ///A test for Export Xelatex For Scripture
        ///</summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void ScriptureExportTest()
        {
            string inputSourceDirectory = FileInput("ExportLO");
            string outputDirectory = FileOutput("ExportLO");
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            FolderTree.Copy(inputSourceDirectory, outputDirectory);
	        Param.SetLoadType = "Scripture";
            Param.LoadSettings();
            _projInfo.ProjectPath = outputDirectory;
            _projInfo.ProjectInputType = "Scripture";
            _projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDirectory, "ScriptureInput.xhtml");
            _projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDirectory, "ScriptureInput.css");
            _projInfo.OutputExtension = "odt";
            EnableConfigurationSettings(outputDirectory);

            var target = new ExportLibreOffice();
            const bool expectedResult = true;
            bool actual = target.Export(_projInfo);
            Assert.AreEqual(expectedResult, actual);
        }

        private void EnableConfigurationSettings(string outputDirectory)
        {
            Param.SetValue(Param.PrintVia, "OpenOffice/LibreOffice");
            Param.SetValue(Param.LayoutSelected, "A4 Cols ApplicationStyles");
            // Publication Information tab
            Param.UpdateTitleMetadataValue(Param.Title, "LibreOfficeExport", false);
            Param.UpdateMetadataValue(Param.Description, "LibreOfficeDescription");
            Param.UpdateMetadataValue(Param.Creator, "LibreOfficeCreator");
            Param.UpdateMetadataValue(Param.Publisher, "LibreOfficePublisher");
            Param.UpdateMetadataValue(Param.CopyrightHolder, "SIL International");
            // also persist the other DC elements
            Param.UpdateMetadataValue(Param.Subject, "Bible");
            Param.UpdateMetadataValue(Param.Date, DateTime.Today.ToString("yyyy-MM-dd"));
            Param.UpdateMetadataValue(Param.CoverPage, "True");

			string pathwayDirectory = Path.GetDirectoryName(Common.AssemblyPath);
            string coverImageFilePath = Common.PathCombine(pathwayDirectory, "Graphic");
            coverImageFilePath = Common.PathCombine(coverImageFilePath, "cover.png");
            Param.UpdateMetadataValue(Param.CoverPageFilename, coverImageFilePath);

            Param.UpdateMetadataValue(Param.CoverPageTitle, "True");
            Param.UpdateMetadataValue(Param.TitlePage, "True");
            Param.UpdateMetadataValue(Param.CopyrightPage, "True");

            string copyrightsFilePath = Common.PathCombine(pathwayDirectory, "Copyrights");
            copyrightsFilePath = Common.PathCombine(copyrightsFilePath, "SIL_CC-by-nc-sa.xhtml");
            Param.UpdateMetadataValue(Param.CopyrightPageFilename, copyrightsFilePath);

            Param.UpdateMetadataValue(Param.TableOfContents, "True");
            Param.SetValue(Param.PublicationLocation, outputDirectory);
            Param.Write();

        }

        /// <summary>
        ///A test for Export Xelatex For Scripture
        ///</summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void DictionaryExportTest()
        {
            string inputSourceDirectory = FileInput("ExportLO");
            string outputDirectory = FileOutput("ExportLO");
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            FolderTree.Copy(inputSourceDirectory, outputDirectory);
            Param.LoadSettings();
            _projInfo.ProjectPath = outputDirectory;
            _projInfo.ProjectInputType = "Dictionary";
            _projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDirectory, "Dictionarymain.xhtml");
            _projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDirectory, "Dictionarymain.css");
            _projInfo.OutputExtension = "odt";

            var target = new ExportLibreOffice();
            const bool expectedResult = true;
            bool actual = target.Export(_projInfo);
            Assert.AreEqual(expectedResult, actual);
        }

		/// <summary>
		///Paragraph Test
		///</summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void ParagraphTest()
		{
			string inputSourceDirectory = FileInput("ParagraphTest");
			string outputDirectory = FileOutput("ParagraphTest");
			if (Directory.Exists(outputDirectory))
			{
				Directory.Delete(outputDirectory, true);
			}
			FolderTree.Copy(inputSourceDirectory, outputDirectory);
			Param.LoadSettings();
			_projInfo.ProjectPath = outputDirectory;
			_projInfo.ProjectInputType = "Dictionary";
			_projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDirectory, "main.xhtml");
			_projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDirectory, "main.css");
			_projInfo.OutputExtension = "odt";

			var target = new ExportLibreOffice();
			const bool expectedResult = true;
			bool actual = target.Export(_projInfo);
			Assert.AreEqual(expectedResult, actual);
		}

		/// <summary>
		///Paragraph Test
		///</summary>
		[Test]
		[Category("LongTest")]
		[Category("SkipOnTeamCity")]
		public void UnderlineColorTest()
		{
			string inputSourceDirectory = FileInput("UnderlineColorTest");
			string outputDirectory = FileOutput("UnderlineColorTest");
			if (Directory.Exists(outputDirectory))
			{
				Directory.Delete(outputDirectory, true);
			}
			FolderTree.Copy(inputSourceDirectory, outputDirectory);
			Param.LoadSettings();
			_projInfo.ProjectPath = outputDirectory;
			_projInfo.ProjectInputType = "Dictionary";
			_projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDirectory, "main.xhtml");
			_projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDirectory, "main.css");
			_projInfo.OutputExtension = "odt";

			var target = new ExportLibreOffice();
			const bool expectedResult = true;
			bool actual = target.Export(_projInfo);
			Assert.AreEqual(expectedResult, actual);
		}

        private string FileInput(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }

        private string FileOutput(string fileName)
        {
            return Common.PathCombine(_outputPath, fileName);
        }

        #endregion PrivateFunctions

        #endregion
    }
}
