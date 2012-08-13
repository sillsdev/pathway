// --------------------------------------------------------------------------------------------
// <copyright file="GoBibleTest.cs" from='2009' to='2009' company='SIL International'>
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
// GoBible Test Support
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.GoBibleConvert
{
    [TestFixture]
    public class UsxToSFMTest
    {

        #region Private Variables
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;
        UsxToSFM _usxToSfm = new UsxToSFM();
        #endregion

        #region SetUp
        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.Testing = true;

            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/GoBibleConvert/TestFiles");
            _inputPath = Common.PathCombine(testPath, "input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "expected");
        }
        #endregion

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void Book()
        {
            const string file = "book";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void chapter()
        {
            const string file = "chapter";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }
        
        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void figure()
        {
            const string file = "figure";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }
        
        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void note()
        {
            const string file = "note";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void note_figure()
        {
            const string file = "note_figure";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void para()
        {
            const string file = "para";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void para_char()
        {
            const string file = "para_char";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void para_empty()
        {
            const string file = "para_empty";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///Compare files
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void verse()
        {
            const string file = "verse";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

        ///<summary>
        ///FULL file compare
        /// </summary>      
        [Test]
        [Category("SkipOnTeamCity")]
        public void rut()
        {
            const string file = "rut";

            string input = Common.PathCombine(_inputPath, file + ".usx");
            string output = Common.PathCombine(_outputPath, file + ".sfm");
            string expected = Common.PathCombine(_expectedPath, file + ".sfm");

            _usxToSfm.ConvertUsxToSFM(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

    }
}
