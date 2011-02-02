// --------------------------------------------------------------------------------------------
// <copyright file="XhtmlExportTest.cs" from='2009' to='2009' company='SIL International'>
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

#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Resources;
using Microsoft.Win32;
using System.IO;
using NUnit.Framework;
using SIL.Tool;

#endregion Using

namespace Test.XhtmlExport
{
    /// <summary>
    /// Test methods of FlexDePlugin
    /// </summary>
    [TestFixture]
    [Category("BatchTest")]
    //[assembly: RegistryPermissionAttribute(SecurityAction.RequestMinimum, ViewAndModify = "HKEY_LOCAL_MACHINE")]
    public class XhtmlExportTest
    {
        #region Setup
        /// <summary>holds path to script file</summary>
        private static string _scriptPath = string.Empty;
        /// <summary>holds full name of the script engine</summary>
        private static string _autoIt = string.Empty;

        private static TestFiles _tf;

        /// <summary>
        /// setup Input, Expected, and Output paths relative to location of program
        /// </summary>
        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.Testing = true;
            RegistryKey myKey = Registry.LocalMachine.OpenSubKey("Software\\Classes\\AutoItScript\\Shell\\Run\\Command", false);
            if (myKey != null)
            {
                _autoIt = myKey.GetValue("", "").ToString();
                if (_autoIt.Length > 5)
                    _autoIt = _autoIt.Substring(0, _autoIt.Length - 5);     // Remove "%1" at end
            }
            _scriptPath = PathPart.Bin(Environment.CurrentDirectory, "/XhtmlExport");
            _tf = new TestFiles("XhtmlExport");
        }
        #endregion Setup

        #region Internal
        /// <summary>
        /// Launch the AutoIt subprocess to export the Xhtml from FieldWorks
        /// </summary>
        private static void FieldWorksXhtmlExport(string app, string proj, string backup, string message)
        {
            FieldWorksXhtmlExport(app, proj, backup, message, null);
        }

        /// <summary>
        /// Launch the AutoIt subprocess to export the Xhtml from FieldWorks
        /// </summary>
        private static void FieldWorksXhtmlExport(string app, string proj, string backup, string message, string incOpt)
        {
            RegistryAccess.GetStringRegistryValue("", "");
            var p1 = new Process();
            p1.StartInfo.UseShellExecute = false;
            p1.StartInfo.EnvironmentVariables.Add("proj", proj);
            p1.StartInfo.EnvironmentVariables.Add("Backup", backup);
            p1.StartInfo.EnvironmentVariables.Add("InputPath", _tf.Input(null));
            p1.StartInfo.EnvironmentVariables.Add("OutputPath", _tf.Output(null));
            p1.StartInfo.Arguments = Common.PathCombine(_scriptPath, app + "XhtmlExport.aut");
            p1.StartInfo.WorkingDirectory = _scriptPath;
            p1.StartInfo.FileName = _autoIt;
            p1.Start();
            if (p1.Id <= 0)
                throw new MissingSatelliteAssemblyException(proj);
            p1.WaitForExit();
            var xhtmlName = proj + ".xhtml";
            var xhtmlExpect = _tf.Expected(xhtmlName);
            var xhtmlOutput = _tf.Output(xhtmlName);
            var ns = new Dictionary<string, string> {{"x", "http://www.w3.org/1999/xhtml"}};
            XmlAssert.Ignore(xhtmlOutput,"/x:html/x:head/x:meta[@name='description']/@content", ns);
            XmlAssert.AreEqual(xhtmlExpect, xhtmlOutput, message + ": " + xhtmlName);
            var cssName = proj + ".css";
            var cssExpect = _tf.Expected(cssName);
            var cssOutput = _tf.Output(cssName);
            TextFileAssert.AreEqualEx(cssExpect, cssOutput, new ArrayList{1}, message + ": " + cssName);
        }
        #endregion Internal

        #region YCE-Test
        /// <summary>
        /// Export Yce-Test Flex data from Fieldworks, compare results to previous exports
        /// </summary>
        [Test]
        [Ignore]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void YceTestExportTest()
        {
            FieldWorksXhtmlExport("Flex", "YCE-Test", "YCE-Test 2010-11-04 1357.fwbackup", "YCE-Test Export changed");
        }
        #endregion YCE-Test

        #region Buang-Test
        /// <summary>
        /// Export Buang-Test Flex data from Fieldworks, compare results to previous exports
        /// </summary>
        [Test]
        [Ignore]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void BuangTestExportTest()
        {
            FieldWorksXhtmlExport("Flex", "Buang-Test", "Buang-Test 2010-11-04 0844.fwbackup", "Export changed", "cl");
        }
        #endregion Buang-Test

        #region Nkonya
        /// <summary>
        /// Export Nkonya TE data from Fieldworks, compare results to previous exports
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void NkonyaExportTest()
        {
            FieldWorksXhtmlExport("Te", "Nkonya", "Nkonya 2010-11-04 1419.fwbackup", "Export changed");
        }
        #endregion Nkonya

        #region Gondwana
        /// <summary>
        /// Export Gondwana Flex data from Fieldworks, compare results to previous exports
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void GondwanaExportTest()
        {
            FieldWorksXhtmlExport("Flex", "Gondwana Test Data", "Gondwana Test Data 2010-11-03 1627.fwbackup", "Export changed");
        }
        #endregion Nkonya
    }
}