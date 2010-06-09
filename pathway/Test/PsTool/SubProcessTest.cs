// --------------------------------------------------------------------------------------------
// <copyright file="SubProcessTest.cs" from='2009' to='2009' company='SIL International'>
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
using SIL.Tool;
using NUnit.Framework;

namespace Test.PsTool
{
    /// <summary>
    ///This is a test class for SubProcessTest and is intended
    ///to contain all SubProcessTest Unit Tests
    ///</summary>
    [TestFixture]
    public class SubProcessTest
    {
        /// <summary>
        ///A test for Run
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RunEmptyTest1()
        {
            string instPath = string.Empty;
            string name = string.Empty;
            bool wait = false;
            SubProcess.Run(instPath, name, wait);
        }

        /// <summary>
        ///A test for Run
        ///</summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RunEmptyTest()
        {
            string instPath = string.Empty;
            string name = string.Empty;
            SubProcess.Run(instPath, name);
        }

        /// <summary>
        ///A test for ExistsOnPath
        ///</summary>
        [Test]
        public void NotepadExistsOnPathTest()
        {
            string name = "Notepad.exe";
            bool expected = true;
            bool actual;
            actual = SubProcess.ExistsOnPath(name);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ExistsOnPath
        ///</summary>
        [Test]
        public void ExistsOnPathTest()
        {
            string name = "candle.exe";
            bool expected = true;
            bool actual;
            actual = SubProcess.ExistsOnPath(name);
            Assert.AreEqual(expected, actual);
        }
    }
}