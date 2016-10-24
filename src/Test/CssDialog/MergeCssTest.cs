// --------------------------------------------------------------------------------------------
// <copyright file="MergeCssTest.cs" from='2009' to='2014' company='SIL International'>
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
using NUnit.Framework;
using System.IO;
using SIL.Tool;

namespace Test.CssDialog
{
    
    
    /// <summary>
    ///This is a test class for MergeCssTest and is intended
    ///to contain all MergeCssTest Unit Tests
    ///</summary>
    [TestFixture]
    public class MergeCssTest
    {
        /// <summary>holds path to input folder for all tests</summary>
        string _inputBasePath = string.Empty;
        /// <summary>holds path to expect folder for all tests</summary>
        string _expectBasePath = string.Empty;

        [TestFixtureSetUp]
        protected void SetUp()
        {
            CommonTestMethod.DisableDebugAsserts();
            string currentFolder = PathPart.Bin(Environment.CurrentDirectory, "/CssDialog/TestFiles");
            _inputBasePath = Common.PathCombine(currentFolder, "Input");
            _expectBasePath = Common.PathCombine(currentFolder, "Expected");
        }

        [TestFixtureTearDown]
        protected void TearDown()
        {
            CommonTestMethod.EnableDebugAsserts();
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for OutputLocation
        ///</summary>
        [Test]
        public void OutputLocationTest()
        {
            MergeCss target = new MergeCss(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.OutputLocation = expected;
            actual = target.OutputLocation;
            Assert.AreEqual(expected, actual, "OutputLocationTest failed");
        }

        /// <summary>
        ///A test for Make function
        ///</summary>
        [Test]
        public void MakeTest1()
        {
            MergeCss target = new MergeCss(); // TODO: Initialize to an appropriate value
            string css = Common.PathCombine(_inputBasePath, "MergeFile4.css"); // TODO: Initialize to an appropriate value
            string actual = target.Make(css, "Temp1.css");
            string expected = Common.PathCombine(_expectBasePath, "MergeFile.css"); // TODO: Initialize to an appropriate value
            TextFileAssert.AreEqual(expected, actual, "Make Funtion test failed");
        }

        /// <summary>
        ///A test for Make function when one of the imported file not exist
        ///</summary>
        [Test]
        public void MakeTest2()
        {
            MergeCss target = new MergeCss(); // TODO: Initialize to an appropriate value
            string css = Common.PathCombine(_inputBasePath, "MergeFile5.css"); // TODO: Initialize to an appropriate value
            string actual = target.Make(css, "Temp1.css");
            string expected = Common.PathCombine(_expectBasePath, "MergeMissingFile.css"); // TODO: Initialize to an appropriate value
            TextFileAssert.AreEqual(expected, actual, "Make Funtion missing file test failed");
        }

        /// <summary>
        ///A test for Make function with non-existant preprocessing folder
        ///</summary>
        [Test]
        public void MakeTest3()
        {
            MergeCss target = new MergeCss { OutputLocation = "Preprocess" };
            var workDir = Common.PathCombine(Path.GetTempPath(), "Preprocess");
            if (Directory.Exists(workDir))
                Directory.Delete(workDir,true);
            string css = Common.PathCombine(_inputBasePath, "MergeFile4.css"); // TODO: Initialize to an appropriate value
            string actual = target.Make(css, "Temp1.css");
            string expected = Common.PathCombine(_expectBasePath, "MergeFile.css"); // TODO: Initialize to an appropriate value
            TextFileAssert.AreEqual(expected, actual, "Make Funtion test failed");
        }

        /// <summary>
        ///A test for Make function with non-existant preprocessing folder
        ///</summary>
        [Test]
        public void MakeTest4()
        {
            MergeCss target = new MergeCss { OutputLocation = "Preprocess" };
            var workDir = Common.PathCombine(Path.GetTempPath(), "Preprocess");
            if (Directory.Exists(workDir))
                Directory.Delete(workDir, true);
            string css = Common.PathCombine(_inputBasePath, "MergeFile7.css"); // TODO: Initialize to an appropriate value
            string actual = target.Make(css, "Temp1.css");
            string expected = Common.PathCombine(_expectBasePath, "MergeBottomImportFile.css"); // TODO: Initialize to an appropriate value
            TextFileAssert.AreEqual(expected, actual, "Make Funtion test failed");
        }
    }
}
