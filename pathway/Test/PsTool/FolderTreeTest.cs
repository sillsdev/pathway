// --------------------------------------------------------------------------------------------
// <copyright file="FileDataTest.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
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
    public class FolderTreeTest
    {
        #region Private Variables
        private string sourceFolder;
        #endregion

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            sourceFolder = PathPart.Bin(Environment.CurrentDirectory, "/PsTool/TestFiles/InputFiles");
            Common.Testing = true;
        }
        #endregion Setup

        /// <summary>
        ///A test for Get
        ///</summary>
        [Test]
        public void CopyTest()
        {
            string source = Common.PathCombine(sourceFolder, "CopyFolder");
            string destination = Common.PathCombine(sourceFolder, "DestinationFolder");
            
            if(Directory.Exists(destination))
                Directory.Delete(destination,true);

            FolderTree.Copy(source,destination);
            Assert.IsTrue(Directory.Exists(destination));

            //string[] files = Directory.GetFiles(destination);
            string path1 = Common.PathCombine(Common.PathCombine(destination, "Folder1"),"New1.txt");
            Assert.IsTrue(File.Exists(path1));

            path1 = Common.PathCombine(Common.PathCombine(destination, "Folder2"), "New1.txt");
            Assert.IsTrue(File.Exists(path1));

            path1 = Common.PathCombine(Common.PathCombine(destination, "Folder2"), "New2.txt");
            Assert.IsTrue(File.Exists(path1));

        }

    }
}

