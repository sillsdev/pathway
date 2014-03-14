// --------------------------------------------------------------------------------------------
// <copyright file="ContentsTest.cs" from='2009' to='2014' company='SIL International'>
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

using NUnit.Framework;
using SIL.PublishingSolution;

namespace FlexDePluginTests
{
    /// <summary>
    ///This is a test class for ContentsTest and is intended
    ///to contain all ContentsTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ContentsTest : Contents1
    {
        /// <summary>
        ///A test for ContentsReversal
        ///</summary>
        [Test]
        public void ContentsReversalTest()
        {
            Contents1 target = new Contents1(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.ExportReversal = expected;
            actual = target.ExportReversal;
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ContentsMain
        ///</summary>
        [Test]
        public void ContentsMainTest()
        {
            Contents1 target = new Contents1(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.ExportMain = expected;
            actual = target.ExportMain;
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for OutputLocationPath
        ///</summary>
        [Test]
        public void OutputLocationPathTest()
        {
            Contents1 target = new Contents1(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.OutputLocationPath;
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ExistingDirectoryLocationPath
        ///</summary>
        [Test]
        public void ExistingDirectoryLocationPathTest()
        {
            Contents1 target = new Contents1(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ExistingDirectoryLocationPath;
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ExistingDirectoryInput
        ///</summary>
        [Test]
        public void ExistingDirectoryInputTest()
        {
            Contents1 target = new Contents1(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ExistingDirectoryInput;
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DictionaryName
        ///</summary>
        [Test]
        public void DictionaryNameTest()
        {
            Contents1 target = new Contents1(); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.DictionaryName;
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for validateInput
        ///</summary>
        [Test]
        // [DeploymentItem("FlexDePlugin.dll")]
        public void validateInputTest()
        {
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = validateInput();
            Assert.AreEqual(expected, actual);
            // TODO: Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ValidateDirectoryLocation
        ///</summary>
        [Test]
        // [DeploymentItem("FlexDePlugin.dll")]
        public void ValidateDirectoryLocationTest()
        {
            ValidateDirectoryLocation();
            // TODO: A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Contents Constructor
        ///</summary>
        [Test]
        public void ContentsConstructorTest()
        {
            Contents1 target = new Contents1();
            // TODO: TODO: Implement code to verify target");
        }
    }
}
