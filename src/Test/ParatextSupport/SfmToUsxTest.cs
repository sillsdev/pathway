// --------------------------------------------------------------------------------------------
// <copyright file="SfmToUsxTest.cs" from='2009' to='2014' company='SIL International'>
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
// Conversion Process
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.ParatextSupport
{
    [TestFixture]
    [Category("ShortTest")]
    public class SfmToUsxTest
    {
        #region Private Variables
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;
        #endregion

        #region SetUp
        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.Testing = true;

            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/ParatextSupport/TestFiles");
            _inputPath = Common.PathCombine(testPath, "Input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "Expected");
            if (!Directory.Exists(_outputPath))
            {
                Directory.CreateDirectory(_outputPath);
                while (!Directory.Exists(_outputPath))
                    Thread.Sleep(1000);
            }
        }
        #endregion

        ///<summary>
        ///Compare files
        /// </summary>
        [Test]
        [Category("SkipOnTC")]
        public void Book()
        {
            SfmToUsx _sfmToUsx = new SfmToUsx();
            const string file = "BookOfMAT";

            string input = Common.PathCombine(_inputPath, file + ".sfm");
            string output = Common.PathCombine(_outputPath, file + ".usx");
            string expected = Common.PathCombine(_expectedPath, file + ".usx");

            _sfmToUsx.ConvertSFMtoUsx(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

		///<summary>
		///Compare files
		/// </summary>
		[Test]
		[Category("SkipOnTC")]
		public void GENKFYBook()
		{
			SfmToUsx _sfmToUsx = new SfmToUsx();
			const string file = "GENKFY";

			string input = Common.PathCombine(_inputPath, file + ".sfm");
			string output = Common.PathCombine(_outputPath, file + ".usx");
			string expected = Common.PathCombine(_expectedPath, file + ".usx");

			_sfmToUsx.ConvertSFMtoUsx(input, output);

			FileAssert.AreEqual(expected, output, file + " test fails");
		}

        ///<summary>
        ///Compare files
        /// </summary>
        [Test]
        [Category("SkipOnTC")]
        public void NKOu3Book()
        {
            SfmToUsx _sfmToUsx = new SfmToUsx();
            const string file = "MATNKOu3";

            string input = Common.PathCombine(_inputPath, file + ".sfm");
            string output = Common.PathCombine(_outputPath, file + ".usx");
            string expected = Common.PathCombine(_expectedPath, file + ".usx");

            _sfmToUsx.ConvertSFMtoUsx(input, output);

            FileAssert.AreEqual(expected, output, file + " test fails");
        }

    }
}
