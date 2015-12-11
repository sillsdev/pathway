// --------------------------------------------------------------------------------------------
// <copyright file="PartialBookTest.cs" from='2014' to='2014' company='SIL International'>
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
// Test code to add chapters and verses to partial books
// </remarks>
// --------------------------------------------------------------------------------------------
using System.IO;
using NUnit.Framework;
using SIL.PublishingSolution;

namespace Test.GoBibleConvert
{
    [TestFixture]
    public class PartialBookTest : ExportGoBible
    {
        #region Setup

        private TestFiles _testFiles;

        [TestFixtureSetUp]
        public void Setup()
        {
            _testFiles = new TestFiles("GoBibleConvert");
        }
        #endregion Setup
        [Test]
        [Category("ShortTest")]
        public void AddChaptersTest()
        {
            const string fileName = "DEU.SFM";
            string inputFullName = _testFiles.Input(fileName);
            string outputFullName = _testFiles.SubOutput("SFM", fileName);
            File.Copy(inputFullName, outputFullName);
            PartialBooks.AddChapters(_testFiles.Output("SFM"));
            TextFileAssert.AreEqual(_testFiles.Expected(fileName), outputFullName);
        }
    }
}
