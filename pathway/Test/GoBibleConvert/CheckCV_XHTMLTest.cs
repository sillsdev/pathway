// --------------------------------------------------------------------------------------------
// <copyright file="CheckCV_XHTMLTest.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Larry Waswick</author>
// <email>larry_waswick@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Test methods of FlexDePlugin
// </remarks>
// --------------------------------------------------------------------------------------------
using System.IO;
using System.Windows.Forms;
using NUnit.Framework;
using System;
using SIL.PublishingSolution;
using SIL.Tool;
using NMock2;

namespace Test.GoBibleConvert
{
    // ideas borrowed from PsTool CommonTest.cs and CommonXmlTest.cs
    ///<summary>
    ///This is a test class for CheckCV_XHTMLTest
    ///</summary>
    [TestFixture]
    public class CheckCV_XHTMLTest : ExportGoBible
    {
        private readonly Mockery mocks = new Mockery();

        ///<summary>
        ///A test for XhtmlCheck
        ///</summary>
        [Test]
        public void XhtmlCheck()
        {
            const string fileName = "1pe.xhtml";
            // Get "1pe.xhtml" from the InputFiles folder.
            string inputFileName = GetFileNameWithInputPath(fileName);
            // The original XSLT file is in the folder "C:\SIL\btai\PublishingSolution\PublishingSolutionExe\bin\Debug".
            // A copy of it has been put into the InputFiles folder.
            // Common.XsltProcress creates an output file in the same folder as the input file.
            // Therefore, since we want 1pe_cv.xhtml to be created in the output folder,
            // 1pe.xhtml must be moved to the output folder, also.
            string outputFileName = GetFileNameWithOutputPath(fileName);
            File.Copy(inputFileName, outputFileName, true);
            const string xsltName = "TE_XHTML-to-Phone_XHTML.xslt";
            string xsltFullName = GetFileNameWithSupportPath(xsltName);

            // Transform the original XHTML file.
            string extension = "_cv.xhtml";
            string actualXhtmlOutput = Common.XsltProcess(outputFileName, xsltFullName, extension);

            // Compare the newly transformed file with "1pe_cv.xhtml" from the Expected folder.
            string expectedXhtmlFile = GetFileNameWithExpectedPath("1pe_cv.xhtml");
            XmlAssert.AreEqual(expectedXhtmlFile, actualXhtmlOutput, "1pe_cv.xhtml was not transformed correctly");
        }

        [Test]
        [Ignore("Hangs Team City")]
        public void RestructureTest()
        {
            const string fileName = "1pe.xhtml";
            const string restructuredFileName = "1pe_cv.xhtml";
            string inputFullName = GetFileNameWithInputPath(fileName);
            string outputFullName = GetFileNameWithOutputPath(fileName);
            File.Copy(inputFullName, outputFullName, true);
            const string cssName = "1pe.css";
            string cssFullName = GetFileNameWithInputPath(cssName);
            PublicationInformation projInfo = new PublicationInformation();
            projInfo.DefaultXhtmlFileWithPath = outputFullName;
            projInfo.DefaultCssFileWithPath = cssFullName;
            IInProcess inProcess = mocks.NewMock<IInProcess>();
            Expect.Once.On(inProcess).Method("AddToMaximum").With(5);
            Expect.Once.On(inProcess).Method("Bar").WithNoArguments();
            Restructure(projInfo, inProcess);
            Assert.AreEqual(Path.GetDirectoryName(outputFullName), processFolder);
            string expectedRestructuredFullName = GetFileNameWithOutputPath(restructuredFileName);
            Assert.AreEqual(expectedRestructuredFullName, restructuredFullName);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CreateCollectionTest()
        {
            const string fileName = "1pe_cv.xhtml";
            const string collections = "Collections.txt";
            string inputFullName = GetFileNameWithInputPath(fileName);
            restructuredFullName = GetFileNameWithOutputPath(fileName);
            File.Copy(inputFullName, restructuredFullName, true);
            processFolder = GetOutputPath();
			Common.ProgBase = GetSupportPath();
            Param.LoadSettings();
            Param.SetValue(Param.InputType, "Scripture");
            Param.LoadSettings();
            Param.WriteMobileAttrib("FileProduced", "OneperBook");
            Param.WriteMobileAttrib("RedLetter", "Yes");
            Param.WriteMobileAttrib("Information", "Sena 3");
            Param.WriteMobileAttrib("Copyright", "© 2010 SIL");
            Param.WriteMobileAttrib("Icon", @"C:\ProgramData\SIL\Pathway\Scripture\Icon.png");
            CreateCollection();
            string actualFullName = GetFileNameWithOutputPath(collections);
            string exepectedFullName = GetFileNameWithExpectedPath(collections);
            TextFileAssert.AreEqual(exepectedFullName, actualFullName);
        }

        [Test]
        public void BuildApplicationTest()
        {
            const string fileName = "1pe_cv.xhtml";
            const string collections = "Collections.txt";
            const string jarFile = "1Pita.jar";
            string inputFullName = GetFileNameWithInputPath(fileName);
            restructuredFullName = GetFileNameWithOutputPath(fileName);
            File.Copy(inputFullName, restructuredFullName, true);
            string collectionInputFullName = GetFileNameWithInputPath(collections);
            collectionFullName = GetFileNameWithOutputPath(collections);
            File.Copy(collectionInputFullName, collectionFullName, true);
            processFolder = GetOutputPath();
			Common.ProgBase = GetSupportPath();
            BuildApplication();
            string actualFullName = GetFileNameWithOutputPath(jarFile);
            string exepectedFullName = GetFileNameWithExpectedPath(jarFile);
            GoBibleTest.AreEqual(exepectedFullName, actualFullName);
        }

        [Test]
        public void BuildApplication2Test()
        {
            const string fileName = "1pe_cv.xhtml";
            const string collections = "Collections.txt";
            const string jarFile = "1Pita.jar";
            string inputFullName = GetFileNameWithInputPath(fileName);
            string folderWithSpace = GetFileNameWithOutputPath("with space");
            if (Directory.Exists(folderWithSpace))
                Directory.Delete(folderWithSpace, true);
            Directory.CreateDirectory(folderWithSpace);
            restructuredFullName = Path.Combine(folderWithSpace, fileName);
            File.Copy(inputFullName, restructuredFullName, true);
            string collectionInputFullName = GetFileNameWithInputPath(collections);
            collectionFullName = Path.Combine(folderWithSpace, collections);
            File.Copy(collectionInputFullName, collectionFullName, true);
            processFolder = GetOutputPath();
			Common.ProgBase = GetSupportPath();
            BuildApplication();
            string actualFullName = Path.Combine(folderWithSpace, jarFile);
            string exepectedFullName = GetFileNameWithExpectedPath(jarFile);
            GoBibleTest.AreEqual(exepectedFullName, actualFullName);
        }

        [Test]
        public void ChaptersTest()
        {
            const string fileName = "1pe.xhtml";
            string inputFullName = GetFileNameWithInputPath(fileName);
            int actual = Chapters(inputFullName);
            const int expected = 5;
            Assert.AreEqual(expected, actual);
        }

        #region private Methods
        private static string GetPath(string place, string filename)
        {
            return Common.PathCombine(GetTestPath(), Common.PathCombine(place, filename));
        }
        private static string GetOutputPath()
        {
            return Common.PathCombine(GetTestPath(), "Output");
        }
        private static string GetTestPath()
        {
            return PathPart.Bin(Environment.CurrentDirectory, "/GoBibleConvert/TestFiles/");
        }
        private static string GetFileNameWithExpectedPath(string fileName)
        {
            return Common.DirectoryPathReplace(GetPath("Expected", fileName));
        }
        private static string GetFileNameWithInputPath(string fileName)
        {
            return Common.DirectoryPathReplace(GetPath("InputFiles", fileName));
        }
        private static string GetFileNameWithOutputPath(string fileName)
        {
            return Common.DirectoryPathReplace(GetPath("Output", fileName));
        }
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