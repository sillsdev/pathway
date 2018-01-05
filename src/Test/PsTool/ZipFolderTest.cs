// --------------------------------------------------------------------------------------------
// <copyright file="ZipFolderTest.cs" from='2009' to='2014' company='SIL International'>
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

using System;
using System.IO;
using NUnit.Framework;
using SIL.Tool;

namespace Test.PsTool
{
    [TestFixture]
    public class ZipFolderTest
    {
        #region Private Variables
        private string sourceFolder;
        private string TargetFolderWithFileName;
        #endregion

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            sourceFolder = PathPart.Bin(Environment.CurrentDirectory, "/PsTool/TestFiles/InputFiles");
            TargetFolderWithFileName = PathPart.Bin(Environment.CurrentDirectory, "/PsTool/TestFiles/output");
            Common.CleanDirectory(new DirectoryInfo(TargetFolderWithFileName));
            if (!Directory.Exists(TargetFolderWithFileName)) Directory.CreateDirectory(TargetFolderWithFileName);
            Common.Testing = true;
        }
        #endregion Setup


        /// <summary>
        ///A test for Verify whether file has created
        ///</summary>
        [Test]
        public void CreateZipTest1()
        {
            const string methodName = "CreateZipTest1";
            var zipObj = new ZipFolder();
            const string outputFileNoPath = "Input.zip";
            const int errCount = 0;
            TargetFolderWithFileName = Common.PathCombine(TargetFolderWithFileName, outputFileNoPath);
            zipObj.CreateZip(sourceFolder, TargetFolderWithFileName, errCount);
            bool result = File.Exists(TargetFolderWithFileName);
            Assert.IsTrue(result, methodName + " failed");
        }

        /// <summary>
        ///A test for verify when source not found
        ///</summary>
        [Test]
        public void CreateZipTest2()
        {
            const string methodName = "CreateZipTest2";
            var zipObj = new ZipFolder();
            const string outputFileNoPath = "Input.zip";
            const int errCount = 0;
            TargetFolderWithFileName = Common.PathCombine(TargetFolderWithFileName, outputFileNoPath);
            DeleteFile();
            zipObj.CreateZip("", TargetFolderWithFileName, errCount);
            bool result = File.Exists(TargetFolderWithFileName);
            Assert.IsFalse(result, methodName + " failed");
        }

        /// <summary>
        ///A test for verify when source not found
        ///</summary>
        [Test]
        public void CreateZipTest3()
        {
            const string methodName = "CreateZipTest3";
            var zipObj = new ZipFolder();
            const string outputFileNoPath = "Input.zip";
            const int errCount = 0;
            TargetFolderWithFileName = Common.PathCombine(TargetFolderWithFileName, outputFileNoPath);
            DeleteFile();
            zipObj.CreateZip(sourceFolder, "", errCount);
            bool result = File.Exists(TargetFolderWithFileName);
            Assert.IsFalse(result, methodName + " failed");
        }

        private void DeleteFile()
        {
            if (File.Exists(TargetFolderWithFileName))
                File.Delete(TargetFolderWithFileName);
        }
    }
}
