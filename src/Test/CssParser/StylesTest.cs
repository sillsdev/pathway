// --------------------------------------------------------------------------------------------
// <copyright file="GramTest.cs" from='2009' to='2014' company='SIL International'>
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
// Css Parset Test file
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.CssParserTest
{
    /// <summary>
    /// Test grammar with suite of input to exercise all common valid css constructions
    /// </summary>
    [TestFixture]
    [Category("BatchTest")]
    public class StylesTest
    {
        #region Setup
        /// <summary>
        /// Path to input files
        /// </summary>
        string _inpPath;
        #endregion Setup

        #region Internal
        /// <summary>
        /// Compute input, output and expected paths for use by all tests.
        /// </summary>
        [TestFixtureSetUp]
        protected void SetUp()
        {
            string currentFolder = PathPart.Bin(Environment.CurrentDirectory, "/CssParser/TestFiles");
            _inpPath = Common.PathCombine(currentFolder, "../../../../DistFiles/Styles");
        }
        #endregion Setup

        #region Internal
        /// <summary>
        /// Do a single test.
        /// </summary>
        /// <param name="testName">Test name is also the folder containing the test.</param>
        /// <param name="msg">Message to display if failure occurs.</param>
        protected int OneTest(string styleSheet)
        {
            if (FileData.Get(styleSheet).Trim() == "")  // Skip empty files
                return 0;

            var ctp = new CssTreeParser();
            ctp.Parse(styleSheet);

            if (ctp.Errors.Count != 0)
            {
                Debug.Print(string.Format("{0} fails with {1} errors", Path.GetFileName(styleSheet), ctp.Errors.Count));
                return 1;
            }
            return 0;
        }
        #endregion Internal

        #region DictionaryStylesTest
        /// <summary>
        /// Test stylesheets released for dictionary styles
        /// </summary>
        [Test]
        public void DictionaryStylesTest()
        {
            var searchPath = Common.PathCombine(_inpPath, "Dictionary");
            var errorCount = 0;
            foreach (var stylesheet in Directory.GetFiles(searchPath, "*.css"))
            {
                errorCount += OneTest(stylesheet);
            }
            Assert.AreEqual(0, errorCount, string.Format("{0} stylesheets contain errors", errorCount));
        }
        #endregion ScriptureStylesTest

        #region ScriptureStylesTest
        /// <summary>
        /// Test stylesheets released for Scripture styles
        /// </summary>
        [Test]
        public void ScriptureStylesTest()
        {
            var searchPath = Common.PathCombine(_inpPath, "Scripture");
            var errorCount = 0;
            foreach (var stylesheet in Directory.GetFiles(searchPath, "*.css"))
            {
                errorCount += OneTest(stylesheet);
            }
            Assert.AreEqual(0, errorCount, string.Format("{0} stylesheets contain errors", errorCount));
        }
        #endregion ScriptureStylesTest
    }
}