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
    public class FileDataTest
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
        public void GetTest1()
        {
            string fileName = "";
            string fileData = FileData.Get(Common.PathCombine(sourceFolder,fileName));
            Assert.IsEmpty(fileData, "GetTest1 Failed");
        }

        [Test]
        public void GetTest2()
        {
            string expected = "@import \"Dictionary.css\";\r\n" +
                              "@import \"FileNotExists.css\";\r\n" +
                              "@import \"Columns_1.css\";\r\n\r\n\r\n";
                
            string fileName = "Layout_02.css";
            string output = FileData.Get(Common.PathCombine(sourceFolder, fileName));
            Assert.AreEqual(expected,output, "GetTest2 Failed");
        }
    }
}

