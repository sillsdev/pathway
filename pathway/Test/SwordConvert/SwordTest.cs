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
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.SwordConvert
{
    [TestFixture]
    public class SwordTest
    {

        #region Private Variables
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;
        #endregion

        #region SetUp
        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.Testing = true;

            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/SwordConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "expected");
        }
        #endregion

        /////<summary>
        /////Compare files
        ///// </summary>      
        //[Test]
        //[Category("SkipOnTeamCity")]
        //public void BM2BookTest()
        //{
        //    const string file = "BM2";

        //    string input = Common.PathCombine(_inputPath, file + ".usx");
        //    string output = Common.PathCombine(_outputPath, file + ".xml");
        //    string expected = Common.PathCombine(_expectedPath, file + ".xml");

        //    var inputTmpDir = Common.PathCombine(Path.GetTempPath(), Path.GetRandomFileName());
        //    Directory.CreateDirectory(inputTmpDir);
        //    inputTmpDir = Common.PathCombine(inputTmpDir, Path.GetFileName(input));
        //    File.Copy(input, inputTmpDir, true);

        //    ExportSword swordObj = new ExportSword();
        //    PublicationInformation projInfo = new PublicationInformation();
        //    projInfo.ProjectPath = Path.GetDirectoryName(inputTmpDir);
        //    projInfo.DefaultXhtmlFileWithPath = inputTmpDir;
        //    projInfo.ProjectFileWithPath = projInfo.ProjectPath;
        //    swordObj.Export(projInfo);
        //    FileAssert.AreEqual(expected, output, file + " test fails");
        //}

        /////<summary>
        /////Compare files
        ///// </summary>      
        //[Test]
        //[Category("SkipOnTeamCity")]
        //public void ACCNTBookTest()
        //{
        //    const string file = "ACCNT";

        //    string input = Common.PathCombine(_inputPath, file + ".usx");
        //    string output = Common.PathCombine(_outputPath, file + ".xml");
        //    string expected = Common.PathCombine(_expectedPath, file + ".xml");

        //    var inputTmpDir = Common.PathCombine(Path.GetTempPath(), Path.GetRandomFileName());
        //    Directory.CreateDirectory(inputTmpDir);
        //    inputTmpDir = Common.PathCombine(inputTmpDir, Path.GetFileName(input));
        //    File.Copy(input, inputTmpDir, true);

        //    ExportSword swordObj = new ExportSword();
        //    PublicationInformation projInfo = new PublicationInformation();
        //    projInfo.ProjectPath = Path.GetDirectoryName(inputTmpDir);
        //    projInfo.DefaultXhtmlFileWithPath = inputTmpDir;
        //    projInfo.ProjectFileWithPath = projInfo.ProjectPath;
        //    swordObj.Export(projInfo);
        //    FileAssert.AreEqual(expected, output, file + " test fails");
        //}

        /////<summary>
        /////Compare files
        ///// </summary>      
        //[Test]
        //[Category("SkipOnTeamCity")]
        //public void MatBookTest()
        //{
        //    const string file = "MAT";

        //    string input = Common.PathCombine(_inputPath, file + ".usx");
        //    string output = Common.PathCombine(_outputPath, file + ".xml");
        //    string expected = Common.PathCombine(_expectedPath, file + ".xml");

        //    var inputTmpDir = Common.PathCombine(Path.GetTempPath(), Path.GetRandomFileName());
        //    Directory.CreateDirectory(inputTmpDir);
        //    inputTmpDir = Common.PathCombine(inputTmpDir, Path.GetFileName(input));
        //    File.Copy(input, inputTmpDir, true);

        //    ExportSword swordObj = new ExportSword();
        //    PublicationInformation projInfo = new PublicationInformation();
        //    projInfo.ProjectPath = Path.GetDirectoryName(inputTmpDir);
        //    projInfo.DefaultXhtmlFileWithPath = inputTmpDir;
        //    projInfo.ProjectFileWithPath = projInfo.ProjectPath;
        //    swordObj.Export(projInfo);
        //    FileAssert.AreEqual(expected, output, file + " test fails");
        //}

        ///<summary>
        ///Compare files
        /// </summary>   
        [Ignore]      
        [Test]
        [Category("SkipOnTeamCity")]
        public void RutBookTest()
        {
            const string file = "rut";
            
            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".xml");
            string expected = Common.PathCombine(_expectedPath, file + ".xml");

            var inputTmpDir = Common.PathCombine(Path.GetTempPath(), Path.GetRandomFileName().Replace(".", "_"));
            Directory.CreateDirectory(inputTmpDir);
            string inputTmpDirFileName = string.Empty;
            inputTmpDirFileName = Common.PathCombine(inputTmpDir, "usx");
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
        [Ignore]    
        [Test]
        [Category("SkipOnTeamCity")]
        public void JN2BookTest()
        {
            const string file = "2JN";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".xml");
            string expected = Common.PathCombine(_expectedPath, file + ".xml");

            var inputTmpDir = Common.PathCombine(Path.GetTempPath(), Path.GetRandomFileName().Replace(".", "_"));
            Directory.CreateDirectory(inputTmpDir);
            string inputTmpDirFileName = string.Empty;
            inputTmpDirFileName = Common.PathCombine(inputTmpDir, "usx");
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
