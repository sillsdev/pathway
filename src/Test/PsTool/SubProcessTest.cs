// --------------------------------------------------------------------------------------------
// <copyright file="SubProcessTest.cs" from='2009' to='2014' company='SIL International'>
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
// Test methods of FlexDePlugin
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
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
        ///A test for Run when not testing
        ///</summary>
        [Test]
        public void RunEmptyTest()
        {
            Common.Testing = false;
            string instPath = string.Empty;
            string name = string.Empty;
            Assert.Throws(typeof (InvalidOperationException), delegate
                {
                    SubProcess.Run(instPath, name);
                });
        }

        /// <summary>
        /// A test for Run when Testing
        /// </summary>
        [Test]
        public void RunEmptyTest1()
        {
            Common.Testing = true;
            string instPath = string.Empty;
            string name = string.Empty;
            SubProcess.Run(instPath, name);
            Assert.AreEqual(0, SubProcess.ExitCode);
        }

		//Environment dependent test
		///// <summary>
		/////A test for Run
		/////</summary>
		//[Test]
		//[Category("ShortTest")]
		//[Category("SkipOnTeamCity")]
		//public void RunTest()
		//{
		//    Common.Testing = false; // Testing as true will prevent Java from being called.
		//    string instPath = Path.GetTempPath();
		//    string name = "java";
		//    string arg = "-version";
		//    bool wait = true;
		//    const string EchoLog = "JavaVersion.log";
		//    SubProcess.Location = string.Empty;
		//    var javaFullName = SubProcess.JavaFullName(name);
		//    SubProcess.RedirectOutput = EchoLog;
		//    SubProcess.Run(instPath, javaFullName, arg, wait);
		//    string logFullName = Common.PathCombine(instPath, EchoLog);
		//    Assert.IsTrue(File.Exists(logFullName));
		//    File.Delete(logFullName);
		//}

		/// <summary>
		///A test for ExistsOnPath
		///</summary>
		[Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void ExistsOnPathTest()
        {
            string name;
            // pick something that is likely to exist on the path of each OS
            if (Common.UsingMonoVM)
            {
                name = "java";
            }
            else
            {
                name = "Notepad.exe";
            }
            bool expected = true;
            bool actual;
            actual = SubProcess.ExistsOnPath(name);
            Assert.AreEqual(expected, actual);
        }
    }
}