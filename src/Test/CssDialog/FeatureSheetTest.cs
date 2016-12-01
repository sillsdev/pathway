// --------------------------------------------------------------------------------------------
// <copyright file="FeatureSheetTest.cs" from='2009' to='2014' company='SIL International'>
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
using SIL.PublishingSolution;
using NUnit.Framework;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Test.CssDialog
{
    /// <summary>
    ///This is a test class for FeatureSheetTest and is intended
    ///to contain all FeatureSheetTest Unit Tests
    ///</summary>
    [TestFixture]
    public class FeatureSheetTest
    {
        /// <summary>
        ///A test for Sheet
        ///</summary>
        [Test]
        public void SheetTest()
        {
            FeatureSheet target = new FeatureSheet(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
	        target.Sheet = expected;
            var actual = target.Sheet;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Features
        ///</summary>
        [Test]
        public void FeaturesTest()
        {
            FeatureSheet target = new FeatureSheet(); // TODO: Initialize to an appropriate value
            List<string> expected = null; // TODO: Initialize to an appropriate value
	        target.Features = expected;
            var actual = target.Features;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Write
        ///</summary>
        [Test]
        public void WriteTest()
        {
            FeatureSheet target = new FeatureSheet(); // TODO: Initialize to an appropriate value
            CommonTestMethod.DisableDebugAsserts();
            try
            {
                target.Write();
                Assert.Fail("Write succeeded with no sheet name!");
            }
            catch (Exception e)
            {
                ArgumentNullException expectedException = new ArgumentNullException();
                Assert.AreEqual(expectedException.GetType(), e.GetType());
            }
            finally
            {
                CommonTestMethod.EnableDebugAsserts();
            }
        }

        /// <summary>
        ///A test for SaveFeatures
        ///</summary>
        [Test]
        public void SaveFeaturesTest()
        {
            FeatureSheet target = new FeatureSheet(); // TODO: Initialize to an appropriate value
            TreeView tv = null; // TODO: Initialize to an appropriate value
            // TODO: SaveFeatures depends on settings in Param class being loaded!
            CommonTestMethod.DisableDebugAsserts();
            try
            {
                target.SaveFeatures(tv);
                Assert.Fail("SaveFeatures returned when Param not loaded!");
            }
            catch (Exception e)
            {
                KeyNotFoundException expectedException = new KeyNotFoundException();
                Assert.AreEqual(expectedException.GetType(), e.GetType());
            }
            finally
            {
                CommonTestMethod.EnableDebugAsserts();
            }
        }

        /// <summary>
        ///A test for ReadToEnd
        ///</summary>
        [Test]
        public void ReadToEndTest()
        {
            FeatureSheet target = new FeatureSheet(); // TODO: Initialize to an appropriate value
            // TODO: Sheet parameter must be set
            CommonTestMethod.DisableDebugAsserts();
            try
            {
                target.ReadToEnd();
                Assert.Fail("ReadToEnd returned when Sheet not set!");
            }
            catch (Exception e)
            {
                NullReferenceException expectedException = new NullReferenceException();
                Assert.AreEqual(expectedException.GetType(), e.GetType());
            }
            finally
            {
                CommonTestMethod.EnableDebugAsserts();
            }
        }
	}
}