// --------------------------------------------------------------------------------------------
// <copyright file="EmbeddedFontTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Erik Brommers</author>
// <email>erik_brommers@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Test methods of EmbeddedFont class
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;
using System.Xml.Xsl;

namespace Test.epubConvert
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Test functions of xhtml to html and other transformation types
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    [TestFixture]
    public class EpubXsltTransformationTest
    {
        #region setup
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;
        private bool _isUnix;

        [TestFixtureSetUp]
        public void Setup()
        {
            _isUnix = Common.IsUnixOS();
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../../DistFiles");
            Common.SupportFolder = "";
            Common.ProgBase = Common.ProgInstall;
            Common.Testing = true;
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/epubConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "Input");
            _outputPath = Common.PathCombine(testPath, "Output");
            _expectedPath = Common.PathCombine(testPath, "Expected");
            Directory.CreateDirectory(_outputPath);
        }
        #endregion setup
        
        [Test]
        public void ExportTypeXhtmltoHtmlTest()
        {
            const string file = "XhtmltoHtmlTransformation";
            File.Copy(FileInput(file + ".xhtml"), FileOutput(file + ".html"), true);
            XslCompiledTransform xhmltohtml5Space = Epub3Transformation.Loadxhmltohtml5Xslt("Dictionary");
            Common.ApplyXslt(FileOutput(file + ".html"), xhmltohtml5Space);
            FileCompare(file);
        }

        [Test]
        public void ExportTypeEpub3TocTest()
        {
            const string file = "Epub3TOCTransformation";
            File.Copy(FileInput(file + ".ncx"), FileOutput(file + ".ncx"), true);
            XslCompiledTransform epub3TocSpace = Epub3Transformation.LoadEpub3Toc();
            Common.ApplyXslt(FileOutput(file + ".ncx"), epub3TocSpace);
            NcxFileCompare(file);
        }

        [Test]
        public void ExportTypeEpub3CoverPageTest()
        {
            const string file = "Epub3CoverPageTransformation";
            File.Copy(FileInput(file + ".html"), FileOutput(file + ".html"), true);
            XslCompiledTransform epub3CoverPageSpace = Epub3Transformation.LoadEpub3CoverPage();
            Common.ApplyXslt(FileOutput(file + ".html"), epub3CoverPageSpace);
            FileCompare(file);
        }

        private void FileCompare(string file)
        {
            string htmlOutput = FileOutput(file + ".html");
            string htmlExpected = FileExpected(file + ".html");
            TextFileAssert.AreEqual(htmlOutput, htmlExpected, file + " in xhtml to html ");
        }

        private void NcxFileCompare(string file)
        {
            string htmlOutput = FileOutput(file + ".ncx");
            string htmlExpected = _isUnix ? FileExpected(file + "_linux.ncx") : FileExpected(file + ".ncx");
            TextFileAssert.AreEqual(htmlOutput, htmlExpected, file + " in ncx to toc ncx ");
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
    }
}
