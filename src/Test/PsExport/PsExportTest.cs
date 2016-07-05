// --------------------------------------------------------------------------------------------
// <copyright file="PsExportTest.cs" from='2009' to='2014' company='SIL International'>
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
// Test methods of FlexDePlugin
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

#endregion Using

namespace Test.PsExport
{
    /// <summary>
    /// Test methods of FlexDePlugin
    /// </summary>
    [TestFixture]
    [Category("BatchTest")]
    public class PsExportTest : SIL.PublishingSolution.PsExport
    {
        #region Setup
        /// <summary>holds path to input folder for all tests</summary>
        private static string _inputBasePath = string.Empty;
        /// <summary>holds path to expected results folder for all tests</summary>
        private static string _expectBasePath = string.Empty;
        /// <summary>holds path to output folder for all tests</summary>
        private static string _outputBasePath = string.Empty;

        /// <summary>
        /// setup Input, Expected, and Output paths relative to location of program
        /// </summary>
        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.Testing = true;
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/PsExport/TestFiles");
            _inputBasePath = Common.PathCombine(testPath, "Input");
            _expectBasePath = Common.PathCombine(testPath, "Expected");
            _outputBasePath = Common.PathCombine(testPath, "Output");
            Common.DeleteDirectory(_outputBasePath);
            Directory.CreateDirectory(_outputBasePath);
            // Set application base for test
            //DoBatch("ConfigurationTool", "postBuild.bat", "Debug");
            //Common.ProgInstall = Environment.CurrentDirectory.Replace("Test", "ConfigurationTool");
            Common.ProgInstall = Environment.CurrentDirectory;
            //FolderTree.Copy(Common.PathCombine(testPath, "../../../../DistFiles/OfficeFiles"),Common.PathCombine(Common.ProgInstall,"OfficeFiles"));
            Backend.Load(Common.ProgInstall);
        }

        /// <summary>
        /// pretend we don't know the type of input after each test
        /// </summary>
        [TearDown]
        protected void TearDown()
        {
            Param.Value[Param.InputType] = "";
        }
        #endregion Setup

        #region Internal
        #region TestPath
        /// <summary>holds path to input folder for all tests</summary>
        private static string _inputTestPath = string.Empty;
        /// <summary>holds path to expected results folder for all tests</summary>
        private static string _expectTestPath = string.Empty;
        /// <summary>holds path to output folder for all tests</summary>
        private static string _outputTestPath = string.Empty;

        private static void TestPathSetup(string testName)
        {
            _inputTestPath = Common.PathCombine(_inputBasePath, testName);
            _expectTestPath = Common.PathCombine(_expectBasePath, testName);
            _outputTestPath = Common.PathCombine(_outputBasePath, testName);
        }

        private static string FileInput(string fileName)
        {
            return Common.PathCombine(_inputTestPath, fileName);
        }

        private static string FileOutput(string fileName)
        {
            return Common.PathCombine(_outputTestPath, fileName);
        }

        private static string FileExpect(string fileName)
        {
            return Common.PathCombine(_expectTestPath, fileName);
        }

        private static string FileCopy(string filename)
        {
            var outFullName = Common.PathCombine(_outputBasePath, filename);
            var inFullName = Common.PathCombine(_inputBasePath, filename);
            const bool overwrite = true;
            File.Copy(inFullName, outFullName, overwrite);
            return outFullName;
        }

        private static string FileTestCopy(string filename)
        {
            var outFullName = Common.PathCombine(_outputTestPath, filename);
            var inFullName = Common.PathCombine(_inputTestPath, filename);
            const bool overwrite = true;
            File.Copy(inFullName, outFullName, overwrite);
            return outFullName;
        }

        private static string TestDataSetup(string test, string data)
        {
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, @"/../../DistFiles");
            Common.SupportFolder = "";
            Common.ProgBase = Common.ProgInstall;
            TestPathSetup(test);
            if (Directory.Exists(_outputTestPath))
                Directory.Delete(_outputTestPath, true);
            Directory.CreateDirectory(_outputTestPath);
            var infile = FileTestCopy(data);
            return infile;
        }
        #endregion TestPath

        #region AcquireUserSettings Common
        /// <summary>
        /// Tests AcquireUserSettings
        /// </summary>
        /// <param name="testName">Test name (and fold name of test).</param>
        /// <param name="mainName">input xhtml name in folder</param>
        /// <param name="cssName">css name in folder</param>
        /// <param name="msg">message identifying test if mismatch (failure).</param>
        protected void AcquireUserSettingsTest(string testName, string mainName, string cssName, string msg)
        {
            CommonOutputSetup(testName);
            Param.SetValue(Param.InputType, "Dictionary");
            Param.LoadSettings();

            File.Copy(FileInput(mainName), FileOutput(mainName));
            JobCopy(cssName);

            var tpe = new SIL.PublishingSolution.PsExport() { DataType = "Scripture" };
            var mainFullName = FileOutput(mainName);
            string job = tpe.GetFluffedCssFullName(mainFullName, _outputTestPath, FileOutput(cssName));
            TextFileAssert.AreEqual(FileExpect(Path.GetFileName(job)), job, msg);
        }

        #endregion AcquireUserSettings Common

        #region SeExport Common
        /// <summary>
        /// Test DeExport function.
        /// </summary>
        /// <param name="testName">test name (also folder name of test)</param>
        /// <param name="mainXhtml">input xhtml name in folder</param>
        /// <param name="jobFileName">job file in folder</param>
        /// <param name="target">desired destination</param>
        /// <param name="msg">message to identify test if error occurs</param>
        protected void SeExportTest(string testName, string mainXhtml, string jobFileName, string target, string msg)
        {
            CommonOutputSetup(testName);
            File.Copy(FileInput(mainXhtml), FileOutput(mainXhtml), true);
            string cssPath = Path.GetFileNameWithoutExtension(mainXhtml);
            File.Copy(FileInput(cssPath) + ".css", FileOutput(cssPath) +".css", true);
            JobCopy(jobFileName);
            FolderTree.Copy(FileInput("Pictures"), FileOutput("Pictures"));

            var tpe = new SIL.PublishingSolution.PsExport { DataType = "Scripture", Destination = target };
            tpe.SeExport(mainXhtml, jobFileName, _outputTestPath);
            switch (target)
            {
                case "OpenOffice/LibreOffice":
                    OdtTest.AreEqual(_expectTestPath, _outputTestPath, msg);
                    break;
                case "Pdf (using Prince)":
                    var outName = Path.GetFileNameWithoutExtension(mainXhtml) + ".pdf";
                    Assert.True(File.Exists(FileOutput(outName)), msg);
                    //FileAssert.AreEqual(FileExpect(outName), FileOutput(outName), msg);
                    break;
                default:
                    Assert.Fail(msg + " unkown destination");
                    break;
            }
        }
        #endregion SeExport Common

        #region PsExport Common

        /// <summary>
        /// Test PsExport function.
        /// </summary>
        /// <param name="testName">test name (also folder name of test)</param>
        /// <param name="mainXhtml">input xhtml name in folder</param>
        /// <param name="dataType"></param>
        /// <param name="target">desired destination</param>
        /// <param name="tests">array of tests to apply to result</param>
        /// <param name="msg">message to identify test if error occurs</param>
        protected void ExportTest(string testName, string mainXhtml, string dataType, string target, string msg = null, ArrayList tests = null)
        {
            CommonOutputSetup(testName);
            CopyExistingFile(mainXhtml);
            var cssName = Path.GetFileNameWithoutExtension(mainXhtml) + ".css";
            CopyExistingFile(cssName);
            if (Directory.Exists(FileInput("Pictures")))
                FolderTree.Copy(FileInput("Pictures"), FileOutput("Pictures"));
            foreach (string fullPath in Directory.GetFiles(_inputTestPath, "*.jpg"))
            {
                var fileName = Path.GetFileName(fullPath);
                File.Copy(fullPath, Common.PathCombine(_outputTestPath, fileName), true);
            }
            CopyExistingFile("FlexRev.xhtml");
            CopyExistingFile("FlexRev.css");

            var tpe = new SIL.PublishingSolution.PsExport { Destination = target, DataType = dataType};
            if (testName.ToLower() == "t5" || testName.ToLower() == "t8")
            {
                tpe._fromNUnit = true;
            }
            tpe.Export(FileOutput(mainXhtml));
            switch (target)
            {
                case "OpenOffice":
                    if (tests != null)
                        OdtTest.DoTests(_outputTestPath, tests);
                    else
                        OdtTest.AreEqual(_expectTestPath, _outputTestPath, msg);
                    break;
                case "Pdf":
                    var outName = Path.GetFileNameWithoutExtension(mainXhtml) + ".pdf";
                    Assert.True(File.Exists(FileOutput(outName)), msg);
                    //FileAssert.AreEqual(FileExpect(outName), FileOutput(outName), msg);
                    break;
                default:
                    Assert.Fail(msg + " unkown destination");
                    break;
            }
        }
        #endregion PsExport Common

        #region Internal private methods
        /// <summary>
        /// erase previous output, load localization files
        /// </summary>
        private static void CommonOutputSetup(string testName)
        {
            Common.PublishingSolutionsEnvironmentReset();
            TestPathSetup(testName);
            var settingsFolder = Common.PathCombine(_inputTestPath, "Pathway");
            if (Directory.Exists(settingsFolder))
            {
                FolderTree.Copy(settingsFolder, Common.GetAllUserPath());
            }

            var di = new DirectoryInfo(_outputTestPath);
            //if (di.Exists)
            //    di.Delete(true);
            Common.DeleteDirectory(_outputTestPath);
            di.Create();

            Common.SupportFolder = "";
			Common.ProgBase = Common.GetPSApplicationPath();
            Param.LoadSettings();
        }
        
        /// <summary>
        /// Copies a file if it exists from the input test path to the output
        /// </summary>
        /// <param name="fileName">file to be copied if it exists</param>
        private static void CopyExistingFile(string fileName)
        {
            if (File.Exists(FileInput(fileName)))
                File.Copy(FileInput(fileName), FileOutput(fileName), true);
        }

        /// <summary>
        /// Copy all referenced css files in input folder
        /// </summary>
        /// <param name="jobFileName">Cascading style sheet file</param>
        private static void JobCopy(string jobFileName)
        {
            string jobFullName = FileInput(jobFileName);
            File.Copy(jobFullName, FileOutput(jobFileName), true);
            var sr = new StreamReader(jobFullName);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                if (line.Length == 0 || line.Substring(0, 1) == "/")
                    continue;
                Match m = Regex.Match(line, "@import \"(.*)\";", RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    JobCopy(m.Groups[1].Value);
                    continue;
                }
                break;
            }
            sr.Close();
            return;
        }
        #endregion Internal private methods
        #endregion Internal

        #region T1
        /// <summary>
        /// Simple test where no changes are made to the settings.
        /// </summary>
        [Test]
        public void AcquireUserSettingsT1()
        {
            AcquireUserSettingsTest("T1", "1pe.xhtml", "Layout_02.css", "T1: Style sheet default preparation");
            //default action is to set style sheet based on last task selected. Layout_02.css is not used.
        }
        #endregion T1

        #region T2
        /// <summary>
        /// Test ODT export
        /// </summary>
        [Test]
        [Ignore]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void SeExportT2()
        {
            SeExportTest("T2", "1pe.xhtml", "Layout_02.css", "OpenOffice/LibreOffice",  "T2: ODT Export Test");
        }
        #endregion T2

        #region T3
        /// <summary>
        /// Test PDF export
        /// </summary>
        [Test]
        [Ignore]
        public void SeExportT3()
        {
            SeExportTest("T3", "1pe.xhtml", "Layout_02.css", "Pdf (using Prince)", "T3: PDF Export Test");
        }
        #endregion T3

        #region T4
        /// <summary>
        /// Test TE Export test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void AcceptT4NkonyaLO()
        {
            var tests = new ArrayList
            {
                new ODet(ODet.Def, "1st master", "mat21-23.odt", ODet.Content, "//style:style[1]/@style:master-page-name", "masterPage"),
                new ODet(ODet.Def, "page layout", "mat21-23.odt", ODet.Styles, "//style:master-page[@style:name='{masterPage}']/@style:page-layout-name", "pageLayout"),
                new ODet(ODet.Chk, "page height", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:page-height", "22.9cm"),
                new ODet(ODet.Chk, "page width", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:page-width", "16.2cm"),
                new ODet(ODet.Chk, "page top margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-top", "1.15cm"),
                new ODet(ODet.Chk, "page left margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-left", "1.5cm"),
                new ODet(ODet.Chk, "page right margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-right", "1.5cm"),
                new ODet(ODet.Chk, "page bottom margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-bottom", "1.15cm"),
                new ODet(ODet.Chk, "title section", "mat21-23.odt", ODet.Content, "//office:body/office:text/*[4]/@text:name", "Sect_scrBook"),
                new ODet(ODet.Chk, "book title", "mat21-23.odt", ODet.Content, "(//text:span[substring-before(@text:style-name, '_') = 'scrBookName'])[1]", "Mateo"),
                new ODet(ODet.Chk, "book code", "mat21-23.odt", ODet.Content, "//text:span[substring-before(@text:style-name, '_') = 'scrBookCode']", "MAT"),
                new ODet(ODet.Chk, "main title", "mat21-23.odt", ODet.Content, "//text:p[substring-before(@text:style-name, '_') = 'TitleMain']/text:span", "Mateo"),
                new ODet(ODet.Chk, "main title center", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:text-align", "center"),
                //new ODet(ODet.Chk, "main title keep with next", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:keep-with-next", "always"),
                new ODet(ODet.Chk, "main title top pad", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:padding-top", "36pt"),
                new ODet(ODet.Chk, "main title bottom pad", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:padding-bottom", "12pt"),
                new ODet(ODet.Chk, "main title left margin", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:margin-left", "0pt"),
                new ODet(ODet.Chk, "main title indent", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:text-indent", "0pt"),
                new ODet(ODet.Chk, "main title orphans", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:orphans", "5"),
                new ODet(ODet.Chk, "main title font weight", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "main title complex font weight", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "main title style", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:font-style", "normal"),
                new ODet(ODet.Chk, "main title font size", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:font-size", "22pt"),
                new ODet(ODet.Chk, "main title complex font size", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@style:font-size-complex", "22pt"),
                new ODet(ODet.Chk, "2nd secondary title", "mat21-23.odt", ODet.Content, "//text:p[substring-before(@text:style-name, '_') = 'TitleSecondary'][2]", "MyTest"),
                new ODet(ODet.Chk, "secondary title center", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@fo:text-align", "center"),
                new ODet(ODet.Chk, "secondary title bottom pad", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@fo:padding-bottom", "2pt"),
                new ODet(ODet.Chk, "secondary title display", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@text:display", "block"),
                new ODet(ODet.Chk, "secondary title style", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@fo:font-style", "italic"),
                new ODet(ODet.Chk, "secondary title font size", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@fo:font-size", "16pt"),
                new ODet(ODet.Chk, "secondary title complex font size", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@style:font-size-complex", "16pt"),
                new ODet(ODet.Chk, "position graphics from top", "mat21-23.odt", ODet.Styles, "//style:style[@style:name='Graphics3']//@style:vertical-pos", "from-top"),
                new ODet(ODet.Chk, "embedded picture", "mat21-23.odt", ODet.Content, "//draw:frame[@draw:style-name='gr3']//@xlink:href", "Pictures/2.jpg"),
                new ODet(ODet.Chk, "Title language", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'span_.nko_TitleMain_')]//@fo:language", "zxx"),
                new ODet(ODet.Chk, "Title language", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'span_.nko_Paragraph_scrSection_')]//@fo:language", "zxx"),
                new ODet(ODet.Chk, "Glossary entry (TD-3665)", "mat21-23.odt", ODet.Content, "//*[starts-with(@text:style-name, 'Line1_')]", "5\u00A0This is a sample text,"),
                new ODet(ODet.Chk, "Punctuation after Glossary entry (TD-3719)", "mat21-23.odt", ODet.Content, "//text:p[26]", " 24 This is sample, text to get the correct content.  25 If you can see the text test, Coding is correct"),
                new ODet(ODet.Chk, "Space after verse", "mat21-23.odt", ODet.Content, "//*[starts-with(@text:style-name, 'Verse')]", "1\u20112\u00A0"),
                
            };

            ExportTest("T4", "mat21-23.xhtml", "Scripture", "OpenOffice", "", tests);
        }
        #endregion T4

        #region T5
        /// <summary>
        /// Test Flex Export test
        /// </summary>
        /// <remarks>For language, see: http://www.w3schools.com/xslfo/prop_language.asp
        /// and http://www.ietf.org/rfc/rfc3066.txt
        /// For country see: http://www.iso.org/iso/iso-3166-1_decoding_table.html
        /// </remarks>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void AcceptT5BuangALO()
        {
            var tests = new ArrayList
            {
                new ODet(ODet.Def, "1st master", ODet.Main, ODet.Content, "//style:style[1]/@style:master-page-name", "masterPage"),
                new ODet(ODet.Def, "page layout", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{masterPage}']/@style:page-layout-name", "pageLayout"),
                new ODet(ODet.Chk, "page top margin", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-top", "2cm"),
                new ODet(ODet.Chk, "page left margin", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-left", "2cm"),
                new ODet(ODet.Chk, "page right margin", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-right", "2cm"),
                new ODet(ODet.Chk, "page bottom margin", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-bottom", "2cm"),
                //First page now has header new ODet(ODet.Chk, "1st Page empty header", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:header-style", ""),
                //First page now has footer new ODet(ODet.Chk, "1st Page empty footer", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:footer-style", ""),
                new ODet(ODet.Def, "left master", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{masterPage}']/@style:next-style-name", "leftMaster"),
                //new ODet(ODet.Chk, "left header variable", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{leftMaster}']//text:variable-get/@text:name", "Left_Guideword_L"),
                //new ODet(ODet.Def, "left header style", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{leftMaster}']//text:span/@text:style-name", "headerTextStyle"),
                //new ODet(ODet.Chk, "left header font", ODet.Main, ODet.Styles, "//style:style[@style:name='{headerTextStyle}']//fo:font-family", "Charis SIL"),
                //new ODet(ODet.Chk, "left header weight", ODet.Main, ODet.Styles, "//style:style[@style:name='{headerTextStyle}']//fo:font-weight", "700"),
                //new ODet(ODet.Chk, "left header size", ODet.Main, ODet.Styles, "//style:style[@style:name='{headerTextStyle}']//fo:font-size", "12pt"),
                new ODet(ODet.Def, "right master", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{leftMaster}']/@style:next-style-name", "rightMaster"),
                //new ODet(ODet.Chk, "right footer variable", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{rightMaster}']//style:footer//draw:frame//text:variable-get/@text:name", "Right_Guideword_R"),
                new ODet(ODet.Chk, "single column letter header", ODet.Main, ODet.Content, "//style:style[@style:name='Sect_letHead']//@fo:column-count", "1"),
                new ODet(ODet.Chk, "double column data", ODet.Main, ODet.Content, "//style:style[@style:name='Sect_letData']//@fo:column-count", "2"),
                new ODet(ODet.Chk, "letter header center", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:text-align", "center"),
                //new ODet(ODet.Chk, "letter header keep with next", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:keep-with-next", "always"),
                new ODet(ODet.Chk, "letter header top margin", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:margin-top", "18pt"),
                new ODet(ODet.Chk, "letter header bottom margin", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:margin-bottom", "18pt"),
                new ODet(ODet.Chk, "letter header font", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:font-family", "Times New Roman"),
                new ODet(ODet.Chk, "letter header complex font", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@style:font-name-complex", "Times New Roman"),
                new ODet(ODet.Chk, "letter header font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "letter header complex font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "letter header font size", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:font-size", "18pt"),
                new ODet(ODet.Chk, "letter header complex font size", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@style:font-size-complex", "18pt"),
                //new ODet(ODet.Chk, "entry background", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:background-color", "transparent"),
                //new ODet(ODet.Chk, "entry alignment", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:text-align", "left"),
                new ODet(ODet.Chk, "entry left margin", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:margin-left", "12pt"),
                new ODet(ODet.Chk, "entry indent", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:text-indent", "-12pt"),
                new ODet(ODet.Chk, "headword font", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@fo:font-family", "Times New Roman"),
                new ODet(ODet.Chk, "headword complex font", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@style:font-name-complex", "Times New Roman"),
                new ODet(ODet.Chk, "headword font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "headword complex font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "headword font style", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@fo:font-style", "normal"),
                new ODet(ODet.Chk, "headword font size", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@fo:font-size", "10pt"),
                new ODet(ODet.Chk, "headword complex font size", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@style:font-size-complex", "10pt"),
                //new ODet(ODet.Chk, "headword language", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@fo:language", "bzh"),
                //new ODet(ODet.Chk, "headword country", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@fo:country", "PG"),
                new ODet(ODet.Chk, "headword left variable", ODet.Main, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[2]//@text:name", "Left_Guideword_L"),
                new ODet(ODet.Chk, "headword right variable", ODet.Main, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[4]//@text:name", "Right_Guideword_R"),
                new ODet(ODet.Chk, "headword left variable value", ODet.Main, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[2]//@office:string-value", "anon"),
                new ODet(ODet.Chk, "headword right variable value", ODet.Main, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[4]//@office:string-value", "anon"),
                new ODet(ODet.Chk, "pronunciation", ODet.Main, ODet.Content, "//text:span[@text:style-name='pronunciation_pronunciations_entry_letData_dicBody']", "a."+ Common.ConvertUnicodeToString("\\02c8") + "non"),
                new ODet(ODet.Chk, "pronunciation font", ODet.Main, ODet.Styles, "//style:style[@style:name='pronunciation_pronunciations_entry_letData_dicBody']//@fo:font-family", "Times New Roman"),
                new ODet(ODet.Chk, "pronunciation complex font", ODet.Main, ODet.Styles, "//style:style[@style:name='pronunciation_pronunciations_entry_letData_dicBody']//@style:font-name-complex", "Times New Roman"),
                new ODet(ODet.Chk, "pronunciation font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='pronunciation_pronunciations_entry_letData_dicBody']//@fo:font-weight", "400"),
                new ODet(ODet.Chk, "pronunciation complex font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='pronunciation_pronunciations_entry_letData_dicBody']//@style:font-weight-complex", "400"),
                new ODet(ODet.Chk, "pronunciation font style", ODet.Main, ODet.Styles, "//style:style[@style:name='pronunciation_pronunciations_entry_letData_dicBody']//@fo:font-style", "normal"),
                new ODet(ODet.Chk, "pronunciation font size", ODet.Main, ODet.Styles, "//style:style[@style:name='pronunciation_pronunciations_entry_letData_dicBody']//@fo:font-size", "10pt"),
                new ODet(ODet.Chk, "pronunciation complex font size", ODet.Main, ODet.Styles, "//style:style[@style:name='pronunciation_pronunciations_entry_letData_dicBody']//@style:font-size-complex", "10pt"),
                new ODet(ODet.Chk, "pronunciation language", ODet.Main, ODet.Styles, "//style:style[@style:name='pronunciation_pronunciations_entry_letData_dicBody']//@fo:language", "zxx"),
                new ODet(ODet.Chk, "pronunciation country", ODet.Main, ODet.Styles, "//style:style[@style:name='pronunciation_pronunciations_entry_letData_dicBody']//@fo:country", "none"),
                new ODet(ODet.Chk, "part of speech", ODet.Main, ODet.Content, "//text:span[@text:style-name='partofspeech_.en_grammaticalinfo_sense_senses_entry_letData_dicBody']", "noun(inal)"),
                new ODet(ODet.Chk, "part of speech parent style", ODet.Main, ODet.Styles, "//style:style[@style:name='partofspeech_.en_grammaticalinfo_sense_senses_entry_letData_dicBody']//@style:parent-style-name", "grammaticalinfo_sense_senses_entry_letData_dicBody"),
                new ODet(ODet.Chk, "part of speech font", ODet.Main, ODet.Styles, "//style:style[@style:name='grammaticalinfo_sense_senses_entry_letData_dicBody']//@fo:font-family", "Times New Roman"),
                new ODet(ODet.Chk, "part of speech complex font", ODet.Main, ODet.Styles, "//style:style[@style:name='grammaticalinfo_sense_senses_entry_letData_dicBody']//@style:font-name-complex", "Times New Roman"),
                new ODet(ODet.Chk, "part of speech font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='grammaticalinfo_sense_senses_entry_letData_dicBody']//@fo:font-weight", "400"),
                new ODet(ODet.Chk, "part of speech complex font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='grammaticalinfo_sense_senses_entry_letData_dicBody']//@style:font-weight-complex", "400"),
                new ODet(ODet.Chk, "part of speech font style", ODet.Main, ODet.Styles, "//style:style[@style:name='grammaticalinfo_sense_senses_entry_letData_dicBody']//@fo:font-style", "italic"),
                new ODet(ODet.Chk, "part of speech font size", ODet.Main, ODet.Styles, "//style:style[@style:name='grammaticalinfo_sense_senses_entry_letData_dicBody']//@fo:font-size", "10pt"),
                new ODet(ODet.Chk, "part of speech complex font size", ODet.Main, ODet.Styles, "//style:style[@style:name='grammaticalinfo_sense_senses_entry_letData_dicBody']//@style:font-size-complex", "10pt"),
                new ODet(ODet.Chk, "part of speech language", ODet.Main, ODet.Styles, "//style:style[@style:name='partofspeech_.en_grammaticalinfo_sense_senses_entry_letData_dicBody']//@fo:language", "en"),
                new ODet(ODet.Chk, "part of speech country", ODet.Main, ODet.Styles, "//style:style[@style:name='partofspeech_.en_grammaticalinfo_sense_senses_entry_letData_dicBody']//@fo:country", "US"),
                new ODet(ODet.Chk, "sense number", ODet.Main, ODet.Content, "//text:span[@text:style-name='xsensenumber_sense_senses_entry_letData_dicBody']", "1) "),
                new ODet(ODet.Chk, "sense number font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='xsensenumber_sense_senses_entry_letData_dicBody']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "sense number complex font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='xsensenumber_sense_senses_entry_letData_dicBody']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "sense number parent style", ODet.Main, ODet.Styles, "//style:style[@style:name='xsensenumber_sense_senses_entry_letData_dicBody']//@style:parent-style-name", "sense_senses_entry_letData_dicBody"),
                new ODet(ODet.Chk, "sense number parent of parent style", ODet.Main, ODet.Styles, "//style:style[@style:name='sense_senses_entry_letData_dicBody']//@style:parent-style-name", "senses_entry_letData_dicBody"),
                new ODet(ODet.Chk, "sense number parent of parent of parent style", ODet.Main, ODet.Styles, "//style:style[@style:name='senses_entry_letData_dicBody']//@style:parent-style-name", "entry_letData_dicBody"),
                //new ODet(ODet.Chk, "sense number language", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:language", "bzh-fonipa"), -- sense numbers don't have a language
                //new ODet(ODet.Chk, "sense number country", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:country", "PG"),
                new ODet(ODet.Chk, "definition", ODet.Main, ODet.Content, "//text:span[@text:style-name='xitem_.en_definition_.en_sense_senses_entry_letData_dicBody']", "fruit, seed"),
                new ODet(ODet.Chk, "definition language", ODet.Main, ODet.Styles, "//style:style[@style:name='xitem_.en_definition_.en_sense_senses_entry_letData_dicBody']//@fo:language", "en"),
                new ODet(ODet.Chk, "definition country", ODet.Main, ODet.Styles, "//style:style[@style:name='xitem_.en_definition_.en_sense_senses_entry_letData_dicBody']//@fo:country", "US"),
                new ODet(ODet.Chk, "definition parent style", ODet.Main, ODet.Styles, "//style:style[@style:name='xitem_.en_definition_.en_sense_senses_entry_letData_dicBody']//@style:parent-style-name", "definition_.en_sense_senses_entry_letData_dicBody"),
                new ODet(ODet.Chk, "definition parent of parent style", ODet.Main, ODet.Styles, "//style:style[@style:name='definition_.en_sense_senses_entry_letData_dicBody']//@style:parent-style-name", "sense_senses_entry_letData_dicBody"),
                new ODet(ODet.Chk, "example font", ODet.Main, ODet.Styles, "//style:style[@style:name='example_xitem_examples_sense_senses_entry_letData_dicBody']//@fo:font-family", "Times New Roman"),
                new ODet(ODet.Chk, "example complex font", ODet.Main, ODet.Styles, "//style:style[@style:name='example_xitem_examples_sense_senses_entry_letData_dicBody']//@style:font-name-complex", "Times New Roman"),
                new ODet(ODet.Chk, "example font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='example_xitem_examples_sense_senses_entry_letData_dicBody']//@fo:font-weight", "400"),
                new ODet(ODet.Chk, "example complex font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='example_xitem_examples_sense_senses_entry_letData_dicBody']//@style:font-weight-complex", "400"),
                new ODet(ODet.Chk, "example font style", ODet.Main, ODet.Styles, "//style:style[@style:name='example_xitem_examples_sense_senses_entry_letData_dicBody']//@fo:font-style", "italic"),
                new ODet(ODet.Chk, "example font size", ODet.Main, ODet.Styles, "//style:style[@style:name='example_xitem_examples_sense_senses_entry_letData_dicBody']//@fo:font-size", "10pt"),
                new ODet(ODet.Chk, "example complex font size", ODet.Main, ODet.Styles, "//style:style[@style:name='example_xitem_examples_sense_senses_entry_letData_dicBody']//@style:font-size-complex", "10pt"),
                //new ODet(ODet.Chk, "example language", ODet.Main, ODet.Styles, "//style:style[@style:name='example_xitem_examples_sense_senses_entry_letData_dicBody']//@fo:language", "bzh"),
                //new ODet(ODet.Chk, "example country", ODet.Main, ODet.Styles, "//style:style[@style:name='example_xitem_examples_sense_senses_entry_letData_dicBody']//@fo:country", "PG"),
                new ODet(ODet.Chk, "translation", ODet.Main, ODet.Content, "//text:span[@text:style-name='translation_.en_translations_xitem_examples_sense_senses_entry_letData_dicBody']", "There is fruit on the corn so let's eat it"),
                new ODet(ODet.Chk, "translation language", ODet.Main, ODet.Styles, "//style:style[@style:name='translation_.en_translations_xitem_examples_sense_senses_entry_letData_dicBody']//@fo:language", "en"),
                new ODet(ODet.Chk, "translation country", ODet.Main, ODet.Styles, "//style:style[@style:name='translation_.en_translations_xitem_examples_sense_senses_entry_letData_dicBody']//@fo:country", "US"),
                new ODet(ODet.Chk, "translation parent style", ODet.Main, ODet.Styles, "//style:style[@style:name='translation_.en_translations_xitem_examples_sense_senses_entry_letData_dicBody']//@style:parent-style-name", "translations_xitem_examples_sense_senses_entry_letData_dicBody"),
                new ODet(ODet.Chk, "translation parent of parent style", ODet.Main, ODet.Styles, "//style:style[@style:name='translations_xitem_examples_sense_senses_entry_letData_dicBody']//@style:parent-style-name", "xitem_examples_sense_senses_entry_letData_dicBody"),
                new ODet(ODet.Chk, "translation parent of parent of parent style", ODet.Main, ODet.Styles, "//style:style[@style:name='xitem_examples_sense_senses_entry_letData_dicBody']//@style:parent-style-name", "examples_sense_senses_entry_letData_dicBody"),
                new ODet(ODet.Chk, "translation parent of parent of parent of parent style", ODet.Main, ODet.Styles, "//style:style[@style:name='examples_sense_senses_entry_letData_dicBody']//@style:parent-style-name", "sense_senses_entry_letData_dicBody"),
                //new ODet(ODet.Chk, "", ODet.Main, ODet.Styles, "", ""),
            };
            ExportTest("T5", "main.xhtml", "Dictionary", "OpenOffice", "", tests);
        }
        #endregion T5

        #region T6
        /// <summary>
        /// Test Flex Export test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void MainAndRevT6()
        {
            var tests = new ArrayList
            {
                new ODet(ODet.Def, "master main", ODet.Mast, ODet.Content, "//office:text/*[3]/text:section-source/@xlink:href", "../main.odt"),
                new ODet(ODet.Def, "master flexrev", ODet.Mast, ODet.Content, "//office:text/*[4]/text:section-source/@xlink:href", "../FlexRev.odt"),
                new ODet(ODet.Def, "1st master", ODet.Main, ODet.Content, "//style:style[1]/@style:master-page-name", "masterPage"),
                new ODet(ODet.Def, "page layout", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{masterPage}']/@style:page-layout-name", "pageLayout"),
                new ODet(ODet.Chk, "page top margin", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-top", "2cm"),
                new ODet(ODet.Chk, "page left margin", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-left", "2cm"),
                new ODet(ODet.Chk, "page right margin", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-right", "2cm"),
                new ODet(ODet.Chk, "page bottom margin", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-bottom", "2cm"),
                // new ODet(ODet.Chk, "1st Page empty header", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:header-style", ""),
                // new ODet(ODet.Chk, "1st Page empty footer", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:footer-style", ""),
                new ODet(ODet.Def, "left master", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{masterPage}']/@style:next-style-name", "leftMaster"),
               // new ODet(ODet.Chk, "left header variable", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{leftMaster}']//text:variable-get/@text:name", "Left_Guideword_L"),
                //new ODet(ODet.Def, "left header style", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{leftMaster}']//text:span/@text:style-name", "headerTextStyle"),
                new ODet(ODet.Def, "right master", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{leftMaster}']/@style:next-style-name", "rightMaster"),
                //new ODet(ODet.Chk, "right footer variable", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{rightMaster}']//style:footer//draw:frame//text:variable-get/@text:name", "Right_Guideword_R"),
                new ODet(ODet.Chk, "single column letter header", ODet.Main, ODet.Content, "//style:style[@style:name='Sect_letHead']//@fo:column-count", "1"),
                new ODet(ODet.Chk, "double column data", ODet.Main, ODet.Content, "//style:style[@style:name='Sect_letData']//@fo:column-count", "2"),
                new ODet(ODet.Chk, "letter header center", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:text-align", "center"),
                new ODet(ODet.Chk, "letter header top margin", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:margin-top", "18pt"),
                new ODet(ODet.Chk, "letter header bottom margin", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:margin-bottom", "18pt"),
                new ODet(ODet.Chk, "letter header font", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:font-family", "Charis SIL"),
                new ODet(ODet.Chk, "letter header complex font", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@style:font-name-complex", "Charis SIL"),
                new ODet(ODet.Chk, "letter header font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "letter header complex font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "letter header font size", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:font-size", "18pt"),
                new ODet(ODet.Chk, "letter header complex font size", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@style:font-size-complex", "18pt"),
                //new ODet(ODet.Chk, "entry background", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:background-color", "transparent"),
                //new ODet(ODet.Chk, "entry alignment", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:text-align", "left"),
                new ODet(ODet.Chk, "entry left margin", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:margin-left", "12pt"),
                new ODet(ODet.Chk, "entry indent", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:text-indent", "-12pt"),
                new ODet(ODet.Chk, "headword font", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@fo:font-family", "Charis SIL"),
                new ODet(ODet.Chk, "headword complex font", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@style:font-name-complex", "Charis SIL"),
                new ODet(ODet.Chk, "headword font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "headword complex font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@style:font-weight-complex", "700"),
                //new ODet(ODet.Chk, "headword font style", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@fo:font-style", "normal"),
                new ODet(ODet.Chk, "headword font size", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@fo:font-size", "10pt"),
                new ODet(ODet.Chk, "headword complex font size", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@style:font-size-complex", "10pt"),
                new ODet(ODet.Chk, "headword left variable", ODet.Main, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[2]//@text:name", "Left_Guideword_L"),
                new ODet(ODet.Chk, "headword right variable", ODet.Main, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[4]//@text:name", "Right_Guideword_R"),
                new ODet(ODet.Chk, "headword left variable value", ODet.Main, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[2]//@office:string-value", "-d"),
                new ODet(ODet.Chk, "headword right variable value", ODet.Main, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[4]//@office:string-value", "-d"),
                //new ODet(ODet.Chk, "pronunciation", ODet.Main, ODet.Content, "//text:span[@text:style-name='span_.bzh-fonipa_pronunciation_pronunciations_entry_letData_dicBody']", Common.ConvertUnicodeToString("\\207f") + "d"),
                //new ODet(ODet.Chk, "pronunciation font", ODet.Main, ODet.Styles, "//style:style[@style:name='pronunciation_pronunciations_entry_letData_dicBody']//@fo:font-family", "Doulos SIL"),
                //new ODet(ODet.Chk, "pronunciation complex font", ODet.Main, ODet.Styles, "//style:style[@style:name='pronunciation_pronunciations_entry_letData_dicBody']//@style:font-name-complex", "Doulos SIL"),
                //new ODet(ODet.Chk, "pronunciation complex font size", ODet.Main, ODet.Styles, "//style:style[@style:name='pronunciation_pronunciations_entry_letData_dicBody']//@style:font-size-complex", "10pt"),
                new ODet(ODet.Chk, "part of speech", ODet.Main, ODet.Content, "//text:span[@text:style-name='span_.en_partofspeech_.en_grammaticalinfo_sense_senses_entry_letData_dicBody']", "N(inal)"),
                new ODet(ODet.Chk, "part of speech language", ODet.Main, ODet.Styles, "//style:style[@style:name='span_.en_partofspeech_.en_grammaticalinfo_sense_senses_entry_letData_dicBody']//@fo:language", "en"),
                new ODet(ODet.Chk, "part of speech country", ODet.Main, ODet.Styles, "//style:style[@style:name='span_.en_partofspeech_.en_grammaticalinfo_sense_senses_entry_letData_dicBody']//@fo:country", "US"),
                new ODet(ODet.Chk, "sense number", ODet.Main, ODet.Content, "//text:span[@text:style-name='xsensenumber_sense_senses_entry_letData_dicBody']", "1"),
                new ODet(ODet.Chk, "definition", ODet.Main, ODet.Content, "//text:span[@text:style-name='span_.en_xitem_.en_definition_.en_sense_senses_entry_letData_dicBody']", "Sunday morning church service."),
                new ODet(ODet.Chk, "definition language", ODet.Main, ODet.Styles, "//style:style[@style:name='span_.en_xitem_.en_definition_.en_sense_senses_entry_letData_dicBody']//@fo:language", "en"),
                new ODet(ODet.Chk, "definition country", ODet.Main, ODet.Styles, "//style:style[@style:name='span_.en_xitem_.en_definition_.en_sense_senses_entry_letData_dicBody']//@fo:country", "US"),
                new ODet(ODet.Chk, "example font", ODet.Main, ODet.Styles, "//style:style[@style:name='example_xitem_examples_sense_senses_entry_letData_dicBody']//@fo:font-family", "Charis SIL"),
                new ODet(ODet.Chk, "translation", ODet.Main, ODet.Content, "//text:span[@text:style-name='span_.en_translation_.en_translations_examples_sense_senses_entry_letData_dicBody']", "They went to the service in the church."),
                new ODet(ODet.Chk, "translation language", ODet.Main, ODet.Styles, "//style:style[@style:name='span_.en_translation_.en_translations_examples_sense_senses_entry_letData_dicBody']//@fo:language", "en"),
                new ODet(ODet.Chk, "translation country", ODet.Main, ODet.Styles, "//style:style[@style:name='span_.en_translation_.en_translations_examples_sense_senses_entry_letData_dicBody']//@fo:country", "US"),
                new ODet(ODet.Chk, "reversalform font", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@fo:font-family", "Times New Roman"),
                new ODet(ODet.Chk, "reversalform complex font", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@style:font-name-complex", "Times New Roman"),
                new ODet(ODet.Chk, "reversalform font weight", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "reversalform complex font weight", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "reversalform font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@fo:font-size", "10pt"),
                new ODet(ODet.Chk, "reversalform complex font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@style:font-size-complex", "10pt"),
                new ODet(ODet.Chk, "reversalform language", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@fo:language", "en"),
                new ODet(ODet.Chk, "reversalform country", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@fo:country", "US"),
                new ODet(ODet.Chk, "reversalform left variable", ODet.Rev, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[2]//@text:name", "Left_Guideword_L"),
                new ODet(ODet.Chk, "reversalform right variable", ODet.Rev, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[4]//@text:name", "Right_Guideword_R"),
                new ODet(ODet.Chk, "reversalform left variable value", ODet.Rev, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[3]//@office:string-value", "aha!"),
                new ODet(ODet.Chk, "reversalform right variable value", ODet.Rev, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[5]//@office:string-value", "aha!"),
                new ODet(ODet.Chk, "headref", ODet.Rev, ODet.Content, "//text:span[@text:style-name='revsensenumber_headref_senses_entry_letData_dicBody']", "ee"),
                new ODet(ODet.Chk, "headref font weight", ODet.Rev, ODet.Styles, "//style:style[@style:name='headref_senses_entry_letData_dicBody']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "headref complex font weight", ODet.Rev, ODet.Styles, "//style:style[@style:name='headref_senses_entry_letData_dicBody']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "headref font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='headref_senses_entry_letData_dicBody']//@fo:font-size", "10pt"),
                new ODet(ODet.Chk, "headref complex font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='headref_senses_entry_letData_dicBody']//@style:font-size-complex", "10pt"),
                new ODet(ODet.Chk, "pronunciation", ODet.Rev, ODet.Content, "//text:span[@text:style-name='span_.bzh-fonipa_pronunciationform_pronunciation_pronunciations_senses_entry_letData_dicBody']", "E" + Common.ConvertUnicodeToString("\\02d0")),
                new ODet(ODet.Chk, "pronunciation language", ODet.Rev, ODet.Styles, "//style:style[@style:name='span_.bzh-fonipa_pronunciationform_pronunciation_pronunciations_senses_entry_letData_dicBody']//@fo:language", "zxx"),
                new ODet(ODet.Chk, "pronunciation country", ODet.Rev, ODet.Styles, "//style:style[@style:name='span_.bzh-fonipa_pronunciationform_pronunciation_pronunciations_senses_entry_letData_dicBody']//@fo:country", "none"),
                new ODet(ODet.Chk, "part of speech", ODet.Rev, ODet.Content, "//text:span[@text:style-name='span_.en_partofspeech_.en_grammaticalinfo_senses_entry_letData_dicBody']", "Interj"),
                new ODet(ODet.Chk, "example font", ODet.Rev, ODet.Styles, "//style:style[@style:name='example_examples_senses_entry_letData_dicBody']//@fo:font-family", "Charis SIL"),
                new ODet(ODet.Chk, "example complex font", ODet.Rev, ODet.Styles, "//style:style[@style:name='example_examples_senses_entry_letData_dicBody']//@style:font-name-complex", "Charis SIL"),
                new ODet(ODet.Chk, "example font style", ODet.Rev, ODet.Styles, "//style:style[@style:name='example_examples_senses_entry_letData_dicBody']//@fo:font-style", "italic"),
                new ODet(ODet.Chk, "example font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='example_examples_senses_entry_letData_dicBody']//@fo:font-size", "10pt"),
                new ODet(ODet.Chk, "example complex font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='example_examples_senses_entry_letData_dicBody']//@style:font-size-complex", "10pt"),
                new ODet(ODet.Chk, "translation", ODet.Rev, ODet.Content, "//text:span[@text:style-name='span_.en_translation_.en_translations_examples_senses_entry_letData_dicBody']", "Look! He's come up over there."),
                new ODet(ODet.Chk, "translation language", ODet.Rev, ODet.Styles, "//style:style[@style:name='span_.en_translation_.en_translations_examples_senses_entry_letData_dicBody']//@fo:language", "en"),
                new ODet(ODet.Chk, "translation country", ODet.Rev, ODet.Styles, "//style:style[@style:name='span_.en_translation_.en_translations_examples_senses_entry_letData_dicBody']//@fo:country", "US"),
                //new ODet(ODet.Chk, "", ODet.Main, ODet.Styles, "", ""),
            };
            ExportTest("T6", "main.xhtml", "Dictionary", "OpenOffice", "", tests);
        }
        #endregion T6

        #region T7
        /// <summary>
        /// Test Flex Export test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void RevT7()
        {
            var tests = new ArrayList
            {
                new ODet(ODet.Def, "1st master", ODet.Rev, ODet.Content, "//style:style[1]/@style:master-page-name", "masterPage"),
                new ODet(ODet.Def, "page layout", ODet.Rev, ODet.Styles, "//style:master-page[@style:name='{masterPage}']/@style:page-layout-name", "pageLayout"),
                new ODet(ODet.Chk, "page top margin", ODet.Rev, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-top", "2cm"),
                new ODet(ODet.Chk, "page left margin", ODet.Rev, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-left", "2cm"),
                new ODet(ODet.Chk, "page right margin", ODet.Rev, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-right", "2cm"),
                new ODet(ODet.Chk, "page bottom margin", ODet.Rev, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-bottom", "2cm"),
                //new ODet(ODet.Chk, "1st Page empty header", ODet.Rev, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:header-style", ""),
                //new ODet(ODet.Chk, "1st Page empty footer", ODet.Rev, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:footer-style", ""),
                new ODet(ODet.Def, "left master", ODet.Rev, ODet.Styles, "//style:master-page[@style:name='{masterPage}']/@style:next-style-name", "leftMaster"),
                //new ODet(ODet.Chk, "left header variable", ODet.Rev, ODet.Styles, "//style:master-page[@style:name='{leftMaster}']//text:variable-get/@text:name", "Left_Guideword_L"),
                //new ODet(ODet.Def, "left header style", ODet.Rev, ODet.Styles, "//style:master-page[@style:name='{leftMaster}']//text:span/@text:style-name", "headerTextStyle"),
                new ODet(ODet.Def, "right master", ODet.Rev, ODet.Styles, "//style:master-page[@style:name='{leftMaster}']/@style:next-style-name", "rightMaster"),
                //new ODet(ODet.Chk, "right footer variable", ODet.Rev, ODet.Styles, "//style:master-page[@style:name='{rightMaster}']//style:footer//draw:frame//text:variable-get/@text:name", "Right_Guideword_R"),
                new ODet(ODet.Chk, "single column letter header", ODet.Rev, ODet.Content, "//style:style[@style:name='Sect_letHead']//@fo:column-count", "1"),
                new ODet(ODet.Chk, "double column data", ODet.Rev, ODet.Content, "//style:style[@style:name='Sect_letData']//@fo:column-count", "2"),
                new ODet(ODet.Chk, "letter header center", ODet.Rev, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:text-align", "center"),
                //new ODet(ODet.Chk, "letter header keep with next", ODet.Rev, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:keep-with-next", "always"),
                new ODet(ODet.Chk, "letter header top margin", ODet.Rev, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:margin-top", "18pt"),
                new ODet(ODet.Chk, "letter header bottom margin", ODet.Rev, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:margin-bottom", "18pt"),
                new ODet(ODet.Chk, "letter header font", ODet.Rev, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:font-family", "Times New Roman"),
                new ODet(ODet.Chk, "letter header complex font", ODet.Rev, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@style:font-name-complex", "Times New Roman"),
                new ODet(ODet.Chk, "letter header font weight", ODet.Rev, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "letter header complex font weight", ODet.Rev, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "letter header font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:font-size", "18pt"),
                new ODet(ODet.Chk, "letter header complex font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@style:font-size-complex", "18pt"),
                //new ODet(ODet.Chk, "entry background", ODet.Rev, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:background-color", "transparent"),
                //new ODet(ODet.Chk, "entry alignment", ODet.Rev, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:text-align", "left"),
                new ODet(ODet.Chk, "entry left margin", ODet.Rev, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:margin-left", "12pt"),
                new ODet(ODet.Chk, "entry indent", ODet.Rev, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:text-indent", "-12pt"),
                new ODet(ODet.Chk, "reversalform font", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@fo:font-family", "Times New Roman"),
                new ODet(ODet.Chk, "reversalform complex font", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@style:font-name-complex", "Times New Roman"),
                new ODet(ODet.Chk, "reversalform font weight", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "reversalform complex font weight", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "reversalform font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@fo:font-size", "10pt"),
                new ODet(ODet.Chk, "reversalform complex font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@style:font-size-complex", "10pt"),
                new ODet(ODet.Chk, "reversalform language", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@fo:language", "en"),
                new ODet(ODet.Chk, "reversalform country", ODet.Rev, ODet.Styles, "//style:style[@style:name='reversalform_entry_letData_dicBody']//@fo:country", "US"),
                new ODet(ODet.Chk, "reversalform left variable", ODet.Rev, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[2]//@text:name", "Left_Guideword_L"),
                new ODet(ODet.Chk, "reversalform right variable", ODet.Rev, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[4]//@text:name", "Right_Guideword_R"),
                new ODet(ODet.Chk, "reversalform left variable value", ODet.Rev, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[3]//@office:string-value", "aha!"),
                new ODet(ODet.Chk, "reversalform right variable value", ODet.Rev, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[5]//@office:string-value", "aha!"),
                //new ODet(ODet.Chk, "headref font", ODet.Rev, ODet.Styles, "//style:style[@style:name='headref_senses_entry_letData_dicBody']//@fo:font-family", "Charis SIL"),
                //new ODet(ODet.Chk, "headref complex font", ODet.Rev, ODet.Styles, "//style:style[@style:name='headref_senses_entry_letData_dicBody']//@style:font-name-complex", "Charis SIL"),
                new ODet(ODet.Chk, "headref font weight", ODet.Rev, ODet.Styles, "//style:style[@style:name='headref_senses_entry_letData_dicBody']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "headref complex font weight", ODet.Rev, ODet.Styles, "//style:style[@style:name='headref_senses_entry_letData_dicBody']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "headref font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='headref_senses_entry_letData_dicBody']//@fo:font-size", "10pt"),
                new ODet(ODet.Chk, "headref complex font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='headref_senses_entry_letData_dicBody']//@style:font-size-complex", "10pt"),
                //new ODet(ODet.Chk, "headref language", ODet.Rev, ODet.Styles, "//style:style[@style:name='headref_senses_entry_letData_dicBody']//@fo:language", "bzh"),
                //new ODet(ODet.Chk, "headref country", ODet.Rev, ODet.Styles, "//style:style[@style:name='headref_senses_entry_letData_dicBody']//@fo:country", "PG"),
                //new ODet(ODet.Chk, "pronunciation", ODet.Rev, ODet.Content, "//text:span[@text:style-name='span_.bzh-fonipa_pronunciationform_pronunciation_pronunciations_senses_entry_letData_dicBody']", "E" + Common.ConvertUnicodeToString("\\02d0")),
                //new ODet(ODet.Chk, "pronunciation font", ODet.Rev, ODet.Styles, "//style:style[@style:name='span_.bzh-fonipa_pronunciationform_pronunciation_pronunciations_senses_entry_letData_dicBody']//@fo:font-family", "Doulos SIL"),
                //new ODet(ODet.Chk, "pronunciation complex font", ODet.Rev, ODet.Styles, "//style:style[@style:name='span_.bzh-fonipa_pronunciationform_pronunciation_pronunciations_senses_entry_letData_dicBody']//@style:font-name-complex", "Doulos SIL"),
                new ODet(ODet.Chk, "pronunciation language", ODet.Rev, ODet.Styles, "//style:style[@style:name='span_.bzh-fonipa_pronunciationform_pronunciation_pronunciations_senses_entry_letData_dicBody']//@fo:language", "zxx"),
                new ODet(ODet.Chk, "pronunciation country", ODet.Rev, ODet.Styles, "//style:style[@style:name='span_.bzh-fonipa_pronunciationform_pronunciation_pronunciations_senses_entry_letData_dicBody']//@fo:country", "none"),
                new ODet(ODet.Chk, "part of speech", ODet.Rev, ODet.Content, "//text:span[@text:style-name='span_.en_span_.en_grammaticalinfo_senses_entry_letData_dicBody']", "Interj"),
                new ODet(ODet.Chk, "part of speech parent style", ODet.Rev, ODet.Styles, "//style:style[@style:name='span_.en_span_.en_grammaticalinfo_senses_entry_letData_dicBody']//@style:parent-style-name", "span_.en_grammaticalinfo_senses_entry_letData_dicBody"),
                new ODet(ODet.Chk, "example font", ODet.Rev, ODet.Styles, "//style:style[@style:name='example_examples_senses_entry_letData_dicBody']//@fo:font-family", "Charis SIL"),
                new ODet(ODet.Chk, "example complex font", ODet.Rev, ODet.Styles, "//style:style[@style:name='example_examples_senses_entry_letData_dicBody']//@style:font-name-complex", "Charis SIL"),
                new ODet(ODet.Chk, "example font style", ODet.Rev, ODet.Styles, "//style:style[@style:name='example_examples_senses_entry_letData_dicBody']//@fo:font-style", "italic"),
                new ODet(ODet.Chk, "example font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='example_examples_senses_entry_letData_dicBody']//@fo:font-size", "10pt"),
                new ODet(ODet.Chk, "example complex font size", ODet.Rev, ODet.Styles, "//style:style[@style:name='example_examples_senses_entry_letData_dicBody']//@style:font-size-complex", "10pt"),
                //new ODet(ODet.Chk, "example language", ODet.Rev, ODet.Styles, "//style:style[@style:name='example_examples_senses_entry_letData_dicBody']//@fo:language", "bzh"),
                //new ODet(ODet.Chk, "example country", ODet.Rev, ODet.Styles, "//style:style[@style:name='example_examples_senses_entry_letData_dicBody']//@fo:country", "PG"),
                new ODet(ODet.Chk, "translation", ODet.Rev, ODet.Content, "//text:span[@text:style-name='span_.en_span_.en_translations_examples_senses_entry_letData_dicBody']", "Look! He's come up over there."),
                //Charis SIL AmArea if installed new ODet(ODet.Chk, "translation font", ODet.Rev, ODet.Styles, "//style:style[@style:name='span_.en_span_.en_translations_examples_senses_entry_letData_dicBody']//@fo:font-family", "Times New Roman"),
                //Charis SIL AmArea if installed new ODet(ODet.Chk, "translation complex font", ODet.Rev, ODet.Styles, "//style:style[@style:name='span_.en_span_.en_translations_examples_senses_entry_letData_dicBody']//@style:font-name-complex", "Times New Roman"),
                new ODet(ODet.Chk, "translation language", ODet.Rev, ODet.Styles, "//style:style[@style:name='span_.en_span_.en_translations_examples_senses_entry_letData_dicBody']//@fo:language", "en"),
                new ODet(ODet.Chk, "translation country", ODet.Rev, ODet.Styles, "//style:style[@style:name='span_.en_span_.en_translations_examples_senses_entry_letData_dicBody']//@fo:country", "US"),
                //new ODet(ODet.Chk, "", ODet.Rev, ODet.Styles, "", ""),
            };

            ExportTest("T7", "FlexRev.xhtml", "Dictionary", "OpenOffice", "", tests);
        }
        #endregion T7

        /// <summary>
        /// Test Flex Export test - Page A5 Test
        /// </summary>
        [Test]
        [Ignore]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void PsExportT8()
        {
            ExportTest("T8", "main.xhtml", "Dictionary", "OpenOffice", "T8: Flex ODT Export Test");
        }

        #region TD3661LineSpace24
        /// <summary>
        /// Test TE Export test
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void TD3661LineSpace24()
        {
            var tests = new ArrayList
            {
                new ODet(ODet.Chk, "Line1 line-height", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'Line1_')]//@fo:line-height", "24pt"),
                new ODet(ODet.Chk, "Line2 line-height", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'Line2_')]//@fo:line-height", "13pt"),
                new ODet(ODet.Chk, "Paragraph line-height", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'Paragraph_')]//@fo:line-height", "24pt"),
                new ODet(ODet.Chk, "ParagraphContinuation line-height", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'ParagraphContinuation_')]//@fo:line-height", "24pt"),
                //new ODet(ODet.Chk, "ParallelPassageReference line-height", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'ParallelPassageReference_')]//@fo:line-height", "24pt"),
                new ODet(ODet.Chk, "ChapterNumber line-height", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'ChapterNumber')]//@fo:line-height", "24pt"),
                new ODet(ODet.Chk, "ChapterNumber2 line-height", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'ChapterNumber2')]//@fo:line-height", "24pt"),
                new ODet(ODet.Chk, "SectionHead line-height", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'SectionHead_')]//@fo:line-height", "24pt"),
                //new ODet(ODet.Chk, "Title line-height", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'TitleMain_')]//@fo:line-height", "24pt"),
                //new ODet(ODet.Chk, "TitleSecondary line-height", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'TitleSecondary_')]//@fo:line-height", "24pt"),
            };

            ExportTest("TD3661", "mat21-23.xhtml", "Scripture", "OpenOffice", "", tests);
        }
        #endregion TD3661LineSpace24

        #region TD3661vLineSpace24
        /// <summary>
        /// Test TE Export test
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void TD3661vLineSpace24()
        {
            var tests = new ArrayList
            {
                new ODet(ODet.Chk, "Line1 line-height-at-least", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'Line1_')]//@style:line-height-at-least", "24pt"),
                new ODet(ODet.Chk, "Line2 line-height-at-least", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'Line2_')]//@style:line-height-at-least", "13pt"),
                new ODet(ODet.Chk, "Paragraph line-height-at-least", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'Paragraph_')]//@style:line-height-at-least", "24pt"),
                new ODet(ODet.Chk, "ParagraphContinuation line-height-at-least", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'ParagraphContinuation_')]//@style:line-height-at-least", "24pt"),
                new ODet(ODet.Chk, "ParallelPassageReference line-height-at-least", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'ParallelPassageReference_')]//@style:line-height-at-least", "24pt"),
                new ODet(ODet.Chk, "ChapterNumber line-height-at-least", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'ChapterNumber')]//@style:line-height-at-least", "24pt"),
                new ODet(ODet.Chk, "ChapterNumber2 line-height-at-least", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'ChapterNumber2')]//@style:line-height-at-least", "24pt"),
                new ODet(ODet.Chk, "SectionHead line-height-at-least", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'SectionHead_')]//@style:line-height-at-least", "24pt"),
                new ODet(ODet.Chk, "Title line-height-at-least", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'TitleMain_')]//@style:line-height-at-least", "24pt"),
                //new ODet(ODet.Chk, "TitleSecondary line-height-at-least", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'TitleSecondary_')]//@style:line-height-at-least", "24pt"),
            };

            ExportTest("TD3661v", "mat21-23.xhtml", "Scripture", "OpenOffice", "", tests);
        }
        #endregion TD3661vLineSpace24

        #region T11
        /// <summary>
        /// Test TE Export test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void T11()
        {
            var tests = new ArrayList
            {
                new ODet(ODet.Chk, "Line1 writing-mode", "D33.odt", ODet.Styles, "//style:style[starts-with(@style:name,'Line1_')]//@style:writing-mode", "rl-tb"),
                new ODet(ODet.Chk, "Line2 writing-mode", "D33.odt", ODet.Styles, "//style:style[starts-with(@style:name,'Line2_')]//@style:writing-mode", "rl-tb"),
                new ODet(ODet.Chk, "Paragraph writing-mode", "D33.odt", ODet.Styles, "//style:style[starts-with(@style:name,'Paragraph_')]//@style:writing-mode", "rl-tb"),
                new ODet(ODet.Chk, "Paragraph align", "D33.odt", ODet.Styles, "//style:style[starts-with(@style:name,'Paragraph_')]//@fo:text-align", "end"),
                new ODet(ODet.Chk, "ChapterNumber writing-mode", "D33.odt", ODet.Styles, "//style:style[starts-with(@style:name,'ChapterNumber')]//@style:writing-mode", "rl-tb"),
                new ODet(ODet.Chk, "SectionHead writing-mode", "D33.odt", ODet.Styles, "//style:style[starts-with(@style:name,'SectionHead_')]//@style:writing-mode", "rl-tb"),
                new ODet(ODet.Chk, "Title writing-mode", "D33.odt", ODet.Styles, "//style:style[starts-with(@style:name,'TitleMain_')]//@style:writing-mode", "rl-tb"),
                new ODet(ODet.Chk, "Title align", "D33.odt", ODet.Styles, "//style:style[starts-with(@style:name,'TitleMain_')]//@fo:text-align", "center"),
                new ODet(ODet.Chk, "main title font size", "D33.odt", ODet.Styles, "//style:style[starts-with(@style:name,'TitleMain_')]//@fo:font-size", "22pt"),
                new ODet(ODet.Chk, "Paragraph font size", "D33.odt", ODet.Styles, "//style:style[starts-with(@style:name,'Paragraph_scrSection')]//@fo:font-size", "11pt"),
            };

            ExportTest("T11", "D33.xhtml", "Scripture", "OpenOffice", "", tests);
        }
        #endregion T11

        #region T12
        /// <summary>
        /// Test TE Export test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void T12()
        {
            var tests = new ArrayList
            {
                new ODet(ODet.Def, "1st master", "mat21-23.odt", ODet.Content, "//style:style[1]/@style:master-page-name", "masterPage"),
                new ODet(ODet.Def, "page layout", "mat21-23.odt", ODet.Styles, "//style:master-page[@style:name='{masterPage}']/@style:page-layout-name", "pageLayout"),
                new ODet(ODet.Chk, "page height", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:page-height", "792pt"),
                new ODet(ODet.Chk, "page width", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:page-width", "612pt"),
                new ODet(ODet.Chk, "page top margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-top", "60pt"),
                new ODet(ODet.Chk, "page left margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-left", "72pt"),
                new ODet(ODet.Chk, "page right margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-right", "50pt"),
                new ODet(ODet.Chk, "page bottom margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-bottom", "80pt"),
            };

            ExportTest("T12", "mat21-23.xhtml", "Scripture", "OpenOffice", "", tests);
        }
        #endregion T12

        #region T13
        /// <summary>
        /// Test Scriputre 1 column Export test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void T13()
        {
            var tests = new ArrayList
            {
                new ODet(ODet.Chk, "single column letter header", "mat21-23.odt", ODet.Content, "//style:style[@style:name='Sect_scrBook']//@fo:column-count", "1"),
                new ODet(ODet.Chk, "double column data", "mat21-23.odt", ODet.Content, "//style:style[@style:name='Sect_columns']//@fo:column-count", "1"),
            };

            ExportTest("T13", "mat21-23.xhtml", "Scripture", "OpenOffice", "", tests);
        }
        #endregion T13

        [Test]
        public void AddHomographAndSenseNumClassNamesTest()
        {
            const string testData = "FlexRev.xhtml";
            var flexRevFullName = FileCopy(testData);
            Common.StreamReplaceInFile(flexRevFullName, "class=\"headword\"", "class=\"headref\"");
            AddHomographAndSenseNumClassNames.Execute(flexRevFullName, flexRevFullName);
            var actual = Common.DeclareXMLDocument(false);
            actual.Load(flexRevFullName);
            var nodes = actual.SelectNodes("//*[@class='revhomographnumber']");
            Assert.AreEqual(8, nodes.Count);
        }

        #region T14
        /// <summary>
        /// Test TE Export test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void T14FieldWorksA4()
        {
            var tests = new ArrayList
            {
                new ODet(ODet.Def, "1st master", "mat21-23.odt", ODet.Content, "//style:style[1]/@style:master-page-name", "masterPage"),
                new ODet(ODet.Def, "page layout", "mat21-23.odt", ODet.Styles, "//style:master-page[@style:name='{masterPage}']/@style:page-layout-name", "pageLayout"),
                new ODet(ODet.Chk, "page height TD-3690", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:page-height", "842pt"),
                new ODet(ODet.Chk, "page width TD-3690", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:page-width", "595pt"),
                new ODet(ODet.Chk, "page top margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-top", "43pt"),
                new ODet(ODet.Chk, "page left margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-left", "52pt"),
                new ODet(ODet.Chk, "page right margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-right", "52pt"),
                new ODet(ODet.Chk, "page bottom margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-bottom", "43pt"),
                new ODet(ODet.Chk, "title section", "mat21-23.odt", ODet.Content, "//text:section[1]/@text:name", "Sect_scrBook"),
                new ODet(ODet.Chk, "book title", "mat21-23.odt", ODet.Content, "(//text:span[substring-before(@text:style-name, '_') = 'scrBookName'])[1]", "Mateo"),
                new ODet(ODet.Chk, "book code", "mat21-23.odt", ODet.Content, "//text:span[substring-before(@text:style-name, '_') = 'scrBookCode']", "MAT"),
                new ODet(ODet.Chk, "main title", "mat21-23.odt", ODet.Content, "//text:p[substring-before(@text:style-name, '_') = 'TitleMain']/text:span", "Mateo"),
                new ODet(ODet.Chk, "main title center", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:text-align", "center"),
                //new ODet(ODet.Chk, "main title keep with next", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:keep-with-next", "always"),
                new ODet(ODet.Chk, "main title top pad", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:padding-top", "36pt"),
                new ODet(ODet.Chk, "main title bottom pad", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:padding-bottom", "12pt"),
                new ODet(ODet.Chk, "main title left margin", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:margin-left", "0pt"),
                new ODet(ODet.Chk, "main title indent", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:text-indent", "0pt"),
                new ODet(ODet.Chk, "main title orphans", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:orphans", "5"),
                new ODet(ODet.Chk, "main title font weight", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "main title complex font weight", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "main title style", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:font-style", "normal"),
                new ODet(ODet.Chk, "main title font size", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:font-size", "22pt"),
                new ODet(ODet.Chk, "main title complex font size", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@style:font-size-complex", "22pt"),
                new ODet(ODet.Chk, "2nd secondary title", "mat21-23.odt", ODet.Content, "//text:p[substring-before(@text:style-name, '_') = 'TitleSecondary'][2]", "MyTest"),
                new ODet(ODet.Chk, "secondary title center", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@fo:text-align", "center"),
                new ODet(ODet.Chk, "secondary title bottom pad", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@fo:padding-bottom", "2pt"),
                new ODet(ODet.Chk, "secondary title display", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@text:display", "block"),
                new ODet(ODet.Chk, "secondary title style", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@fo:font-style", "italic"),
                new ODet(ODet.Chk, "secondary title font size", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@fo:font-size", "16pt"),
                new ODet(ODet.Chk, "secondary title complex font size", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@style:font-size-complex", "16pt"),
                new ODet(ODet.Chk, "position graphics from top", "mat21-23.odt", ODet.Styles, "//style:style[@style:name='Graphics2']//@style:vertical-pos", "from-top"),
                new ODet(ODet.Chk, "embedded picture", "mat21-23.odt", ODet.Content, "//draw:frame[@draw:name='Graphics2']//@xlink:href", "Pictures/1.jpg"),
                new ODet(ODet.Chk, "Title language", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'span_.nko_TitleMain_')]//@fo:language", "zxx"),
                new ODet(ODet.Chk, "Title language", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'span_.nko_Paragraph_scrSection_')]//@fo:language", "zxx"),
                
            };

            ExportTest("T14", "mat21-23.xhtml", "Scripture", "OpenOffice", "", tests);
        }
        #endregion T14

        #region T15
        /// <summary>
        /// Test Dictionary Rights page
        /// </summary>
        /// <remarks>For language, see: http://www.w3schools.com/xslfo/prop_language.asp
        /// and http://www.ietf.org/rfc/rfc3066.txt
        /// For country see: http://www.iso.org/iso/iso-3166-1_decoding_table.html
        /// </remarks>
        [Test]
		[Ignore]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void T15DictionaryRights()
        {
            var tests = new ArrayList
            {
                new ODet(ODet.Def, "1st master", ODet.Main, ODet.Content, "//style:style[1]/@style:master-page-name", "masterPage"),
                new ODet(ODet.Def, "page layout", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{masterPage}']/@style:page-layout-name", "pageLayout"),
                new ODet(ODet.Chk, "page top margin", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-top", "2cm"),
                new ODet(ODet.Chk, "page left margin", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-left", "2cm"),
                new ODet(ODet.Chk, "page right margin", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-right", "2cm"),
                new ODet(ODet.Chk, "page bottom margin", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-bottom", "2cm"),
                //see T5 above new ODet(ODet.Chk, "1st Page empty header", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:header-style", ""),
                //see above new ODet(ODet.Chk, "1st Page empty footer", ODet.Main, ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:footer-style", ""),
                new ODet(ODet.Def, "left master", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{masterPage}']/@style:next-style-name", "leftMaster"),
                //new ODet(ODet.Chk, "left header variable", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{leftMaster}']//text:variable-get/@text:name", "Left_Guideword_L"),
                //new ODet(ODet.Def, "left header style", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{leftMaster}']//text:span/@text:style-name", "headerTextStyle"),
                new ODet(ODet.Def, "right master", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{leftMaster}']/@style:next-style-name", "rightMaster"),
                //new ODet(ODet.Chk, "right footer variable", ODet.Main, ODet.Styles, "//style:master-page[@style:name='{rightMaster}']//style:footer//draw:frame//text:variable-get/@text:name", "Right_Guideword_R"),
                new ODet(ODet.Chk, "single column letter header", ODet.Main, ODet.Content, "//style:style[@style:name='Sect_letHead']//@fo:column-count", "1"),
                new ODet(ODet.Chk, "double column data", ODet.Main, ODet.Content, "//style:style[@style:name='Sect_letData']//@fo:column-count", "2"),
                new ODet(ODet.Chk, "letter header center", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:text-align", "center"),
                new ODet(ODet.Chk, "letter header top margin", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:margin-top", "18pt"),
                new ODet(ODet.Chk, "letter header bottom margin", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:margin-bottom", "18pt"),
                new ODet(ODet.Chk, "letter header font", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:font-family", "Doulos SIL"),
                new ODet(ODet.Chk, "letter header complex font", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@style:font-name-complex", "Doulos SIL"),
                new ODet(ODet.Chk, "letter header font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "letter header complex font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "letter header font size", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@fo:font-size", "18pt"),
                new ODet(ODet.Chk, "letter header complex font size", ODet.Main, ODet.Styles, "//style:style[@style:name='letter_letHead_dicBody']//@style:font-size-complex", "18pt"),
                //new ODet(ODet.Chk, "entry background", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:background-color", "transparent"),
                //new ODet(ODet.Chk, "entry alignment", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:text-align", "left"),
                new ODet(ODet.Chk, "entry left margin", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:margin-left", "12pt"),
                new ODet(ODet.Chk, "entry indent", ODet.Main, ODet.Styles, "//style:style[@style:name='entry_letData_dicBody']//@fo:text-indent", "-12pt"),
                new ODet(ODet.Chk, "headword font", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@fo:font-family", "Doulos SIL"),
                new ODet(ODet.Chk, "headword complex font", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@style:font-name-complex", "Doulos SIL"),
                new ODet(ODet.Chk, "headword font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "headword complex font weight", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "headword font size", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@fo:font-size", "10pt"),
                new ODet(ODet.Chk, "headword complex font size", ODet.Main, ODet.Styles, "//style:style[@style:name='headword_entry_letData_dicBody']//@style:font-size-complex", "10pt"),
                new ODet(ODet.Chk, "headword left variable", ODet.Main, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[2]//@text:name", "Left_Guideword_L"),
                new ODet(ODet.Chk, "headword right variable", ODet.Main, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[4]//@text:name", "Right_Guideword_R"),
                new ODet(ODet.Chk, "headword left variable value", ODet.Main, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[2]//@office:string-value", "congane"),
                new ODet(ODet.Chk, "headword right variable value", ODet.Main, ODet.Content, "//text:p[@text:style-name='entry_letData_dicBody']/text:span[4]//@office:string-value", "congane"),
                new ODet(ODet.Chk, "part of speech", ODet.Main, ODet.Content, "//*[starts-with(@text:style-name, 'span_.es_span_.es_grammaticalinfo')]", "sus"),
                new ODet(ODet.Chk, "part of speech font", ODet.Main, ODet.Styles, "//*[starts-with(@style:name, 'span_.es_span_.es_grammaticalinfo')]//@fo:font-family", "Times New Roman"),
                new ODet(ODet.Chk, "part of speech complex font", ODet.Main, ODet.Styles, "//*[starts-with(@style:name, 'span_.es_span_.es_grammaticalinfo')]//@style:font-name-complex", "Times New Roman"),
                new ODet(ODet.Chk, "part of speech font style", ODet.Main, ODet.Styles, "//*[starts-with(@style:name,'grammaticalinfo')]//@fo:font-style", "italic"),
                new ODet(ODet.Chk, "part of speech font size", ODet.Main, ODet.Styles, "//*[starts-with(@style:name,'grammaticalinfo')]//@fo:font-size", "10pt"),
                new ODet(ODet.Chk, "part of speech complex font size", ODet.Main, ODet.Styles, "//*[starts-with(@style:name,'grammaticalinfo')]//@style:font-size-complex", "10pt"),
                new ODet(ODet.Chk, "part of speech language", ODet.Main, ODet.Styles, "//*[starts-with(@style:name, 'span_.es_span_.es_grammaticalinfo')]//@fo:language", "es"),
                new ODet(ODet.Chk, "part of speech country", ODet.Main, ODet.Styles, "//*[starts-with(@style:name, 'span_.es_span_.es_grammaticalinfo')]//@fo:country", "ES"),
                new ODet(ODet.Chk, "definition", ODet.Main, ODet.Content, "//*[starts-with(@text:style-name, 'span_.es_span_.es_span_.es_sense')]", "zarigueya"),
                new ODet(ODet.Chk, "definition language", ODet.Main, ODet.Styles, "//*[starts-with(@style:name, 'span_.es_span_.es_span_.es_sense')]//@fo:language", "es"),
                new ODet(ODet.Chk, "definition country", ODet.Main, ODet.Styles, "//*[starts-with(@style:name, 'span_.es_span_.es_span_.es_sense')]//@fo:country", "ES"),
                new ODet(ODet.Chk, "rights page header", ODet.Main, ODet.Content, "//text:span[starts-with(@text:style-name, 'LHeading')]", "ABOUT THIS DOCUMENT"),
                new ODet(ODet.Chk, "rights url", ODet.Main, ODet.Content, "//text:span[starts-with(@text:style-name, 'span_LText')][2]", "http://www.ethnologue.com/language/auc"),
                new ODet(ODet.Chk, "rights copyright", ODet.Main, ODet.Content, "(//text:span[starts-with(@text:style-name, 'LText')])[2]", "\u00a9 2016 John Doe\u00ae."),
                new ODet(ODet.Chk, "rights copyright", ODet.Main, ODet.Content, "//*[starts-with(@text:style-name, 'div_FrontMatter')]", "This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 Unported License."),
                new ODet(ODet.Chk, "rights copyright", ODet.Main, ODet.Content, "(//*[starts-with(@text:style-name, 'div_FrontMatter')])[2]", "To view a copy of this license, visit "),
                new ODet(ODet.Chk, "rights copyright", ODet.Main, ODet.Content, "(//*[starts-with(@text:style-name, 'div_FrontMatter')])[3]", "http://creativecommons.org/licenses/by-nc-sa/4.0/"),
            };
            ExportTest("T15", "main.xhtml", "Dictionary", "OpenOffice", "", tests);
        }
        #endregion T15

        #region T16
        /// <summary>
        /// Test Paratext with A5 page size, 36pt column gap, page numbers on outside margins
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void T16A5ScriptureWideGap()
        {
            var tests = new ArrayList
            {
                new ODet(ODet.Def, "1st master", "mat21-23.odt", ODet.Content, "//style:style[1]/@style:master-page-name", "masterPage"),
                new ODet(ODet.Def, "page layout", "mat21-23.odt", ODet.Styles, "//style:master-page[@style:name='{masterPage}']/@style:page-layout-name", "pageLayout"),
                new ODet(ODet.Chk, "page height", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:page-height", "595pt"),
                new ODet(ODet.Chk, "page width", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:page-width", "420pt"),
                new ODet(ODet.Chk, "page top margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-top", "33pt"),
                new ODet(ODet.Chk, "page left margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-left", "43pt"),
                new ODet(ODet.Chk, "page right margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-right", "43pt"),
                new ODet(ODet.Chk, "page bottom margin", "mat21-23.odt", ODet.Styles, "//style:page-layout[@style:name='{pageLayout}']/style:page-layout-properties/@fo:margin-bottom", "33pt"),
                new ODet(ODet.Chk, "title section", "mat21-23.odt", ODet.Content, "//text:section[1]/@text:name", "Sect_scrBook"),
                new ODet(ODet.Chk, "book title", "mat21-23.odt", ODet.Content, "(//text:span[substring-before(@text:style-name, '_') = 'scrBookName'])[1]", "Mateo"),
                new ODet(ODet.Chk, "book code", "mat21-23.odt", ODet.Content, "//text:span[substring-before(@text:style-name, '_') = 'scrBookCode']", "MAT"),
                new ODet(ODet.Chk, "main title", "mat21-23.odt", ODet.Content, "//text:p[substring-before(@text:style-name, '_') = 'TitleMain']/text:span", "Mateo"),
                new ODet(ODet.Chk, "main title center", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:text-align", "center"),
                new ODet(ODet.Chk, "main title top pad", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:padding-top", "36pt"),
                new ODet(ODet.Chk, "main title bottom pad", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:padding-bottom", "12pt"),
                new ODet(ODet.Chk, "main title left margin", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:margin-left", "0pt"),
                new ODet(ODet.Chk, "main title indent", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:text-indent", "0pt"),
                new ODet(ODet.Chk, "main title orphans", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:orphans", "5"),
                new ODet(ODet.Chk, "main title font weight", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:font-weight", "700"),
                new ODet(ODet.Chk, "main title complex font weight", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@style:font-weight-complex", "700"),
                new ODet(ODet.Chk, "main title style", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:font-style", "normal"),
                new ODet(ODet.Chk, "main title font size", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@fo:font-size", "22pt"),
                new ODet(ODet.Chk, "main title complex font size", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleMain']//@style:font-size-complex", "22pt"),
                new ODet(ODet.Chk, "secondary title center", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@fo:text-align", "center"),
                new ODet(ODet.Chk, "secondary title bottom pad", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@fo:padding-bottom", "2pt"),
                new ODet(ODet.Chk, "secondary title display", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@text:display", "block"),
                new ODet(ODet.Chk, "secondary title style", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@fo:font-style", "italic"),
                new ODet(ODet.Chk, "secondary title font size", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@fo:font-size", "16pt"),
                new ODet(ODet.Chk, "secondary title complex font size", "mat21-23.odt", ODet.Styles, "//*[substring-before(@style:name, '_') = 'TitleSecondary']//@style:font-size-complex", "16pt"),
                new ODet(ODet.Chk, "position graphics from top", "mat21-23.odt", ODet.Styles, "//style:style[@style:name='Graphics2']//@style:vertical-pos", "from-top"),
                new ODet(ODet.Chk, "embedded picture", "mat21-23.odt", ODet.Content, "//draw:frame[@draw:style-name='gr2']//@xlink:href", "Pictures/1.jpg"),
                new ODet(ODet.Chk, "Title language", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'span_.nko_TitleMain_')]//@fo:language", "zxx"),
                new ODet(ODet.Chk, "Title language", "mat21-23.odt", ODet.Styles, "//style:style[starts-with(@style:name,'span_.nko_Paragraph_scrSection_')]//@fo:language", "zxx"),
                new ODet(ODet.Chk, "column count", "mat21-23.odt", ODet.Content, "//*[@style:name='Sect_columns']//@fo:column-count", "2"),
                new ODet(ODet.Chk, "1st column end gap TD-3708", "mat21-23.odt", ODet.Content, "//*[@style:name='Sect_columns']//*[@fo:column-count]/*[1]//@fo:end-indent", "18pt"),
                new ODet(ODet.Chk, "2nd column start gap TD-3708", "mat21-23.odt", ODet.Content, "//*[@style:name='Sect_columns']//*[@fo:column-count]/*[2]//@fo:start-indent", "18pt"),
                new ODet(ODet.Chk, "left header page num", "mat21-23.odt", ODet.Styles, "//*[@style:display-name='Left Page']//text:p/*[1]/*[1]/@text:select-page", "current"),
                new ODet(ODet.Chk, "left header guide word", "mat21-23.odt", ODet.Styles, "//*[@style:display-name='Left Page']//@text:name", "Left_Guideword_L"),
                new ODet(ODet.Chk, "header center tab", "mat21-23.odt", ODet.Styles, "//*[@style:name='Header']//*[@style:type='center']/@style:position", "167pt"),
                new ODet(ODet.Chk, "right header page num", "mat21-23.odt", ODet.Styles, "//*[@style:display-name='Right Page']//style:header//*[3]/*[1]/@text:select-page", "current"),
                new ODet(ODet.Chk, "header right tab", "mat21-23.odt", ODet.Styles, "//*[@style:name='Header']//*[@style:type='right']/@style:position", "334pt"),
                new ODet(ODet.Chk, "right header frame", "mat21-23.odt", ODet.Styles, "//*[@style:display-name='Right Page']//style:footer//@svg:y", "32pt"),
                new ODet(ODet.Chk, "right header guide word", "mat21-23.odt", ODet.Styles, "//*[@style:display-name='Right Page']//@text:name", "Right_Guideword_R"),
                
            };

            ExportTest("T16", "mat21-23.xhtml", "Scripture", "OpenOffice", "", tests);
        }
        #endregion T16

        #region T17
        /// <summary>
        /// Test TE Export test
        /// </summary>
        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void T17NoSpaceAfterVerse()
        {
            var tests = new ArrayList
            {
               new ODet(ODet.Chk, "Glossary entry (TD-3665)", "mat21-23.odt", ODet.Content, "//*[starts-with(@text:style-name, 'Line1_')]", "5\uFEFF" +Common.ConvertUnicodeToString("\\201c") + "This is a sample text,"),
                new ODet(ODet.Chk, "Space after verse", "mat21-23.odt", ODet.Content, "//*[starts-with(@text:style-name, 'Verse')]", "1‑2﻿"),
                
            };

            ExportTest("T17", "mat21-23.xhtml", "Scripture", "OpenOffice", "", tests);
        }
        #endregion T17

        [Test]
        public void XsltPreProcess0Test()
        {
            DataType = "Dictionary";
            var infile = TestDataSetup("Pre0", "Predict.xhtml");
            Param.Value["Preprocessing"] = string.Empty;
            UserOptionSelectionBasedXsltPreProcess(infile);
            var files = Directory.GetFiles(_outputTestPath, "*.*");
            Assert.AreEqual(1, files.Length);
        }

        [Test]
        public void XsltPreProcess1Test()
        {
            DataType = "Dictionary";
            const string  data = "Predict.xhtml";
            var infile = TestDataSetup("Pre1", data);
            Param.Value["Preprocessing"] = "Filter Empty Entries";
			UserOptionSelectionBasedXsltPreProcess(infile);
            var files = Directory.GetFiles(_outputTestPath, "*.*");
            Assert.AreEqual(2, files.Length);
            XmlAssert.AreEqual(Common.PathCombine(_expectTestPath, data), infile, "Empty Entries Preprocess produced different results");
        }

        [Test]
        public void XsltPreProcess2Test()
        {
            DataType = "Dictionary";
            const string data = "Predict.xhtml";
            var infile = TestDataSetup("Pre2", data);
            Param.Value["Preprocessing"] = "Filter Empty Entries,Fix Duplicate ids";
			UserOptionSelectionBasedXsltPreProcess(infile);
            var files = Directory.GetFiles(_outputTestPath, "*.*");
            Assert.AreEqual(2, files.Length);
            XmlAssert.AreEqual(Common.PathCombine(_expectTestPath, data), infile, "Preprocess produced different results");
        }

		[Test]
		public void XsltPreProcessLetterHeaderLanguageTest()
		{
			DataType = "Dictionary";
			const string data = "main.xhtml";
			var infile = TestDataSetup("Pre3", data);
			Param.Value["Preprocessing"] = "Letter Header Language";
			UserOptionSelectionBasedXsltPreProcess(infile);
			var files = Directory.GetFiles(_outputTestPath, "*.*");
			Assert.AreEqual(2, files.Length);
			XmlAssert.AreEqual(Common.PathCombine(_expectTestPath, data), infile, "main Preprocess produced different results");
		}

		[Test]
		public void XsltPreProcessReversalLetterHeaderLanguageTest()
		{
			DataType = "Dictionary";
			const string data = "FlexRev.xhtml";
			var infile = TestDataSetup("Pre3", data);
			Param.Value["Preprocessing"] = "Letter Header Language";
			UserOptionSelectionBasedXsltPreProcess(infile);
			var files = Directory.GetFiles(_outputTestPath, "*.*");
			Assert.AreEqual(2, files.Length);
			XmlAssert.AreEqual(Common.PathCombine(_expectTestPath, data), infile, "FlexRev Preprocess produced different results");
		}
		
		[Test]
		public void XsltPreProcessAddColumnFW83MainTest()
		{
			DataType = "Dictionary";
			const string data = "A-One-w.xhtml";
			var infile = TestDataSetup("Pre4", data);
			PermanentXsltPreprocess(infile);
			var files = Directory.GetFiles(_outputTestPath, "*.*");
			Assert.AreEqual(2, files.Length);
			XmlAssert.AreEqual(Common.PathCombine(_expectTestPath, data), infile, "A-One-w Preprocess produced different results");
		}

		[Test]
		public void XsltPreProcessAddColumnFW83ReversalTest()
		{
			DataType = "Dictionary";
			const string data = "Reversal.xhtml";
			var infile = TestDataSetup("Pre4", data);
			PermanentXsltPreprocess(infile);
			var files = Directory.GetFiles(_outputTestPath, "*.*");
			Assert.AreEqual(2, files.Length);
			XmlAssert.AreEqual(Common.PathCombine(_expectTestPath, data), infile, "FW83v Reversal Preprocess produced different results");
		}

    }
}