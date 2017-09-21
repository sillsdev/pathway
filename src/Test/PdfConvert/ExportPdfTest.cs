// --------------------------------------------------------------------------------------------
// <copyright file="ExportPdfTest.cs" from='2009' to='2014' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using SIL.PublishingSolution;
using NUnit.Framework;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using SIL.Tool;

namespace Test.PdfConvert
{
    /// <summary>
    ///This is a test class for MergeCssTest and is intended
    ///to contain all MergeCssTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ExportPdfTest
    {
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;
        private string _testFolderPath = string.Empty;
        private PublicationInformation _projInfo;


        #region Setup
        [TestFixtureSetUp]
        protected void SetUpAll()
        {
            Common.Testing = true;
            _projInfo = new PublicationInformation();
            _testFolderPath = PathPart.Bin(Environment.CurrentDirectory, "/PdfConvert/TestFiles");
            _inputPath = Common.PathCombine(_testFolderPath, "input");
            _outputPath = Common.PathCombine(_testFolderPath, "output");
            _expectedPath = Common.PathCombine(_testFolderPath, "Expected");
            const bool recursive = true;
            if (Directory.Exists(_outputPath))
                Directory.Delete(_outputPath, recursive);
            Directory.CreateDirectory(_outputPath);
            _projInfo.ProjectPath = _testFolderPath;
            Common.SupportFolder = "";
        }

        #endregion

        #region Public Functions

        #region Nunits

        [Test]
        [Category("ShortTest")]
        public void ScriptureReplaceBookNametoBookCodeTest()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "ScripturePreview";
            ExportProcess(file);

            string outputXhtmlFile = Common.PathCombine(_outputPath, file + ".xhtml");
            File.Copy(_projInfo.DefaultXhtmlFileWithPath, outputXhtmlFile, true);

            ExportPdf pdfObj = new ExportPdf();
            pdfObj.ReplaceBookNametoBookCode(outputXhtmlFile);

            FileCompare(file, ".xhtml");
        }

        /// <summary>
        ///A test for Export
        ///</summary>
        [Test]
        [Category("LongTest")]
        public void ExportDictionaryTest()
        {
            string inputSourceDirectory = FileInput("");
            string outputDirectory = FileOutput("");
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            FolderTree.Copy(inputSourceDirectory, outputDirectory);
			Param.SetLoadType = "Dictionary";
            Param.LoadSettings();
            _projInfo.ProjectPath = outputDirectory;
            _projInfo.ProjectInputType = "Dictionary";
            _projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDirectory, "ExportDictionarymain.xhtml");
            _projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDirectory, "ExportDictionarymain.css");

            var target = new ExportPdf();
            const bool expectedResult = true;
            bool actual = target.Export(_projInfo);
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        ///A test for Dictionary Guideword
        ///</summary>
        [Test]
        [Category("LongTest")]
        public void ExportDictionaryGuidewordTest()
        {
            string inputSourceDirectory = FileInput("");
            string outputDirectory = FileOutput("");
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            FolderTree.Copy(inputSourceDirectory, outputDirectory);
			Param.SetLoadType = "Dictionary";
			Param.LoadSettings();
            _projInfo.ProjectPath = outputDirectory;
            _projInfo.ProjectInputType = "Dictionary";
            _projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDirectory, "ExportDictionaryGuideword.xhtml");
            _projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDirectory, "ExportDictionaryGuideword.css");

            var target = new ExportPdf();
            const bool expectedResult = true;
            bool actual = target.Export(_projInfo);
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        ///A test for Export
        ///</summary>
        [Test]
        [Category("LongTest")]
        public void ExportScriptureTest()
        {
            string inputSourceDirectory = FileInput("");
            string outputDirectory = FileOutput("");
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
			FolderTree.Copy(inputSourceDirectory, outputDirectory);
			Param.SetLoadType = "Scripture";
			Param.LoadSettings();
            _projInfo.ProjectPath = outputDirectory;
            _projInfo.ProjectInputType = "Scripture";
            _projInfo.DefaultXhtmlFileWithPath = Common.PathCombine(outputDirectory, "ScriptureDraft.xhtml");
            _projInfo.DefaultCssFileWithPath = Common.PathCombine(outputDirectory, "ScriptureDraft.css");

            var target = new ExportPdf();
            const bool expectedResult = true;
            bool actual = target.Export(_projInfo);
            Assert.AreEqual(expectedResult, actual);
        }


        [Test]
        [Category("ShortTest")]
        public void CSSStyleForHeaderShowInPrincePdfTest()
        {
            _projInfo.ProjectInputType = "Scripture";
            const string file = "CSSStyleForHeaderShowInPrincePdf";
            ExportProcess(file);

            string outputCSSFile = Common.PathCombine(_outputPath, file + ".css");
            File.Copy(_projInfo.DefaultCssFileWithPath, outputCSSFile, true);

            PreExportProcess pdfObj = new PreExportProcess();
            pdfObj.InsertPropertyInCSS(outputCSSFile);

            FileCompare(file, ".css");
        }
        #endregion

        #region Private Functions

        private void FileCompare(string fileName, string fileExtension)
        {
            string output = FileOutput(fileName + fileExtension);
            string expected = FileExpected(fileName + fileExtension);
            TextFileAssert.AreEqual(output, expected, fileName + " in " + fileExtension);
        }

        private void ExportProcess(string file)
        {
            string input = FileInput(file + ".xhtml");
            _projInfo.DefaultXhtmlFileWithPath = input;
            input = FileInput(file + ".css");
            _projInfo.DefaultCssFileWithPath = input;
            _projInfo.TempOutputFolder = _outputPath;
            _projInfo.OutputExtension = ".xhtml";
        }

        private string FileInput(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }

        private string FileOutput(string fileName)
        {
            return Common.PathCombine(_outputPath, fileName);
        }

        private string FileExpected(string fileName)
        {
            return Common.PathCombine(_expectedPath, fileName);
        }

        #endregion

        #endregion
    }
}
