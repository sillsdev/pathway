// --------------------------------------------------------------------------------------------
#region // Copyright Â© 2009, SIL International. All Rights Reserved.
// <copyright file="PdfTest.cs" from='2009' to='2009' company='SIL International'>
//		Copyright Â© 2009, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using SIL.PublishingSolution;
using NUnit.Framework;

namespace Test.CssDialog
{
    /// <summary>
    ///This is a test class for PdfTest and is intended
    ///to contain all PdfTest Unit Tests
    ///</summary>
    [TestFixture]
    public class PdfTest
    {
        /// <summary>
        ///A test for Xhtml
        ///</summary>
        [Test]
        public void XhtmlTest()
        {
            Pdf target = new Pdf(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Xhtml = expected;
            actual = target.Xhtml;
            Assert.AreEqual(expected, actual);
            //TODO: Verify the correctness of this test method.
        }

        /// <summary>
        ///A test for Css
        ///</summary>
        [Test]
        public void CssTest()
        {
            Pdf target = new Pdf(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Css = expected;
            actual = target.Css;
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [Test]
        public void CreateTest()
        {
            Pdf target = new Pdf(); // TODO: Initialize to an appropriate value
            string outName = string.Empty; // TODO: Initialize to an appropriate value
            CommonTestMethod.DisableDebugAsserts();
            try
            {
                target.Create(outName);
                Assert.Fail("Create accepted null arguments for xhtml or css properties");
            }
            catch (Exception e)
            {
                //var expected = new ArgumentNullException();
                var expected = new AssertionException("Assertion encountered");
                Assert.AreEqual(e.GetType(), expected.GetType());
            }
        }

        /// <summary>
        ///A test for Pdf Constructor
        ///</summary>
        [Test]
        public void PdfConstructorTest1()
        {
            Pdf target = new Pdf();
            // TODO: Implement code to verify target
        }

        /// <summary>
        ///A test for Pdf Constructor
        ///</summary>
        [Test]
        public void PdfConstructorTest()
        {
            string xhtmlFile = string.Empty; // TODO: Initialize to an appropriate value
            string cssFile = string.Empty; // TODO: Initialize to an appropriate value
            Pdf target = new Pdf(xhtmlFile, cssFile);
            // TODO: Implement code to verify target
        }
    }
}