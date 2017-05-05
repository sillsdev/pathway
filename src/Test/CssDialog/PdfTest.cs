// --------------------------------------------------------------------------------------------
// <copyright file="PdfTest.cs" from='2009' to='2014' company='SIL International'>
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
using System.IO;
using SIL.PublishingSolution;
using NUnit.Framework;
using SIL.Tool;

namespace Test.CssDialog
{
    /// <summary>
    ///This is a test class for PdfTest and is intended
    ///to contain all PdfTest Unit Tests
    ///</summary>
    [TestFixture]
    public class PdfTest
    {
        #region setup
        private TestFiles _tf;

        [TestFixtureSetUp]
        public void SetUp()
        {
            Common.Testing = true;
            _tf = new TestFiles("CssDialog");
        }
        #endregion setup

        /// <summary>
        ///A test for Xhtml
        ///</summary>
        [Test]
        public void XhtmlTest()
        {
            Pdf target = new Pdf(_tf.Copy("T1.xhtml"), _tf.Copy("T1.css"));
            string expected = _tf.Output("T1.xhtml");
            string actual = target.Xhtml;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Css
        ///</summary>
        [Test]
        public void CssTest()
        {
            Pdf target = new Pdf(_tf.Copy("T1.xhtml"), _tf.Copy("T1.css"));
            string expected = _tf.Output("T1.css");
            string actual = target.Css;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [Test]
		[Category("SkipOnTC")]
        public void CreateTest()
        {
            try
            {
                Pdf target = new Pdf(_tf.Copy("T1.xhtml"), _tf.Copy("T1.css"));
                string outName = _tf.Output("T1.pdf");
                File.Delete(_tf.Output("T1.pdf"));
                target.Create(outName);
                bool fileOutputCreated = false;
                bool fileExpectedCreated = false;
                if(File.Exists(_tf.Output("T1.pdf")))
                {
                    fileOutputCreated = true;
                }
                if(File.Exists(_tf.Expected("T1.pdf")))
                {
                    fileExpectedCreated = true;
                }
                Assert.IsTrue(fileOutputCreated);
                Assert.IsTrue(fileExpectedCreated);
            }
            catch (Pdf.MISSINGPRINCE) // If Prince not installed, ignore test
            {
            }
        }

        /// <summary>
        ///A test for Pdf Constructor
        ///</summary>
        [Test]
        public void PdfConstructorTest1()
        {
            Pdf target = new Pdf();
            Assert.AreEqual(typeof(Pdf), target.GetType());
        }

        /// <summary>
        ///A test for Pdf Constructor
        ///</summary>
        [Test]
        public void PdfConstructorTest()
        {
            string xhtmlFile = string.Empty;
            string cssFile = string.Empty;
            Pdf target = new Pdf(xhtmlFile, cssFile);
            Assert.AreEqual(typeof(Pdf), target.GetType());
        }
    }
}