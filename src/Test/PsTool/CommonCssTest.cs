// --------------------------------------------------------------------------------------------
// <copyright file="CommonCssTest.cs" from='2009' to='2014' company='SIL International'>
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

using System.Collections;
using System.IO;
using NUnit.Framework;
using SIL.Tool;

namespace Test.PsTool
{

    public partial class CommonTest
    {
        /// <summary>
        ///A test for MakeSingleCSS
        ///</summary>
        [Test]
        public void MakeSingleCSSTest1()
        {
            string fileName = "MergedCss.css";
            string input = GetFileNameWithPath("Merge_KeyFile.css");
            string output = GetFileNameWithOutputPath(fileName);
            string expected = GetFileNameWithExpectedPath(fileName);
            fileName = Common.MakeSingleCSS(input, fileName);
            if (File.Exists(fileName))
            {
                File.Copy(fileName, output, true);
            }

            TextFileAssert.AreEqual(expected, output);
            Common.Testing = true;
        }

        /// <summary>
        ///A test for MakeSingleCSS
        ///</summary>
        [Test]
        public void MakeSingleCSSTest2()
        {
            string fileName = "MergedLayout_02.css";
            string input = GetFileNameWithPath("Layout_02.css");
            string output = GetFileNameWithOutputPath(fileName);
            string expected = GetFileNameWithExpectedPath(fileName);
            fileName = Common.MakeSingleCSS(input, fileName);
            if (File.Exists(fileName))
            {
                File.Copy(fileName, output, true);
            }
            TextFileAssert.AreEqual(expected, output);
        }

        /// <summary>
        ///A test for MakeSingleCSS
        ///</summary>

        [Test]
        public void MakeSingleCSSTest3()
        {
            string fileName = "MergedLayout_03.css";
            string input = GetFileNameWithPath("NoFile.css");
			string output = Common.MakeSingleCSS(input, fileName);
			string expected = GetFileNameWithExpectedPath(fileName);
			TextFileAssert.AreEqual(expected, output);
        }

        /// <summary>
        ///A test for MakeSingleCSS
        ///</summary>
        [Test]
        public void MakeSingleCSSTest4()
        {
            string input = GetFileNameWithPath("Layout_02.css");
            string expected = "tempcssfile.css";

            string returnValue = Common.MakeSingleCSS(input, "tempcssfile.css");
            returnValue = Path.GetFileName(returnValue);
			Assert.IsTrue(returnValue.Contains(expected), "MakeSingleCssTest4 Failed");
        }
       
        /// <summary>
        ///A test for GetCSSFileNames
        ///</summary>
        [Test]
        public void GetCSSFileNamesTest1()
        {
            string input = GetFileNameWithPath("Layout_02.css");
            ArrayList expected = new ArrayList();
            string inputPath = Path.GetDirectoryName(input);
            expected.Add(Common.PathCombine(inputPath, "Dictionary.css"));
            expected.Add(Common.PathCombine(inputPath, "Columns_1.css"));
            expected.Add(input);
            string baseCSSName = input;
            ArrayList actual = Common.GetCSSFileNames(input, baseCSSName);
            actual.Add(baseCSSName);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetCSSFileNames
        ///</summary>
        [Test]
        public void GetCSSFileNamesTest2()
        {
            string input = GetFileNameWithPath("FileNotExist.css"); // Empty String also the same
            ArrayList expected = new ArrayList();
            string baseCSSName = input;
            ArrayList actual = Common.GetCSSFileNames(input, baseCSSName);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for SetDefaultCSS
        ///</summary>
        [Test]
        [Category("ShortTest")]
        //[Category("SkipOnTeamCity")]
        public void SetDefaultCSSTest()
        {
            string fileName = "DefaultCss.xhtml";

            string input = GetFileNameWithPath(fileName); // Empty String also the same
            string defaultCSS = "DefaultCss.css";
            string output = GetFileNameWithOutputPath(fileName);
            string expected = GetFileNameWithExpectedPath(fileName);
            CopyToOutput(input, output);
            Common.SetDefaultCSS(output, defaultCSS);
            XmlAssert.AreEqual(expected, output, "Copied files are not equal");
        }

        /// <summary>
        ///A test for GetProjectType
        ///</summary>
        [Test]
        public void GetProjectTypeTest1()
        {
            string fileName = "ProjectTypeS.css";
            string xhtmlPath = SetupXhtml(fileName);
            string expected = "scripture";

            string actual = Common.GetProjectType(xhtmlPath);
            actual = actual.ToLower();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetProjectType
        ///</summary>
        [Test]
        public void GetProjectTypeTest2()
        {
            string fileName = "ProjectTypeD.css";
            string sourceFile = SetupXhtml(fileName);
            string expected = "dictionary";
            string actual = Common.GetProjectType(sourceFile);
            Assert.AreEqual(expected, actual.ToLower());
        }

        /// <summary>
        ///A test for GetProjectType
        ///</summary>
        [Test]
        public void GetProjectTypeTest3()
        {
            string fileName = "FilNotExist.css";
            string sourceFile = SetupXhtml(fileName);
            string expected = "dictionary";
            string actual = Common.GetProjectType(sourceFile);
            Assert.AreEqual(expected, actual.ToLower());
        }
        /// <summary>
        ///A test for GetProjectType
        ///</summary>
        [Test]
        public void GetProjectTypeTestNullEmpty()
        {
            string fileName = "";
            string expected = "dictionary";
            string sourceFile = SetupXhtml(fileName);
            string actual = Common.GetProjectType(sourceFile);
            Assert.AreEqual(expected, actual.ToLower());

            //fileName = null;
            //sourceFile = GetFileNameWithPath(fileName);
            //actual = Common.GetProjectType(sourceFile);
            //Assert.AreEqual(expected, actual.ToLower());

        }

        /// <summary>
        /// Creates a project based on the linked to the css
        /// </summary>
        /// <param name="cssFileName">name to insert in template</param>
        /// <returns>path to xhtml name</returns>
        private static string SetupXhtml(string cssFileName)
        {
            string xhtmlPath = GetFileNameWithPath("xhtmlMain.xhtml");
            string templatePath = GetFileNameWithPath("xhtmlTemplate.xhtml");
            var stream = new StreamWriter(xhtmlPath);
            stream.Write(string.Format(FileData.Get(templatePath), cssFileName));
            stream.Close();
            return xhtmlPath;
        }

        /// <summary>
        ///A test for GetLinkedCSS
        ///</summary>
        [Test]
        public void GetLinkedCSSTest1()
        {
            string xhtmlFile = GetFileNameWithPath("Preview.xhtml");
            string expected = "preview.css";
            string actual = Common.GetLinkedCSS(xhtmlFile);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetLinkedCSS
        ///</summary>
        [Test]
        public void GetLinkedCSSTest2()
        {
            string xhtmlFile = GetFileNameWithPath("FileNotExist");
            string expected = "";
            string actual = Common.GetLinkedCSS(xhtmlFile);
            Assert.AreEqual(expected, actual);
        }
    }
}
