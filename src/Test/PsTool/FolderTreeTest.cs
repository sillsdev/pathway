// --------------------------------------------------------------------------------------------
// <copyright file="FolderTreeTest.cs" from='2009' to='2014' company='SIL International'>
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
    public class FolderTreeTest
    {
        #region Private Variables
	    private TestFiles _tf;
        #endregion

        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
			_tf = new TestFiles("PsTool", inputFolderName: "InputFiles");
            Common.Testing = true;
        }
        #endregion Setup

        /// <summary>
        ///A test for Get
        ///</summary>
        [Test]
        public void CopyTest()
        {
            string source = _tf.Input("CopyFolder");
            string destination = _tf.Output("DestinationFolder");

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

		//Environment dependent test
		///// <summary>
		/////A test for Get
		/////</summary>
		//[Test]
		//public void LongShortDirectoryNameTest()
		//{
		//	if (Common.UsingMonoVM)
		//		return;

		//	string directory = "C:\\Program Files (x86)\\Common Files";
		//	string shortdirectory = ManageDirectory.ShortFileName(directory);
		//	StringAssert.Contains(shortdirectory, "C:\\PROGRA~2\\COMMON~1", "Directory Not matched");

		//	string longdirectory = ManageDirectory.LongFileName(shortdirectory);
		//	StringAssert.Contains(longdirectory, "C:\\Program Files (x86)\\Common Files", "Directory Not matched");
		//}
    }
}

