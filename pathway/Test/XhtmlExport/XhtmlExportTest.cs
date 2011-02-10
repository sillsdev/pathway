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
            var p1 = new Process();
            p1.StartInfo.UseShellExecute = false;
            p1.StartInfo.EnvironmentVariables.Add("proj", proj);
            p1.StartInfo.EnvironmentVariables.Add("Backup", backup);
            p1.StartInfo.EnvironmentVariables.Add("InputPath", _tf.Input(null));
            p1.StartInfo.EnvironmentVariables.Add("OutputPath", _tf.Output(null));
            p1.StartInfo.EnvironmentVariables.Add("IncOpt", incOpt);
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

        /// <summary>
        /// Runs Pathway on the data and applies the back end
        /// </summary>
        private static void PathawyB(string project, string inputType, string backend)
        {
            PathawyB(project, inputType, backend, "");
        }

        /// <summary>
        /// Runs Pathway on the data and applies the back end
        /// </summary>
        private static void PathawyB(string project, string inputType, string backend, string message)
        {
            const bool overwrite = true;
            var xhtmlName = project + ".xhtml";
            var xhtmlInput = _tf.Expected(xhtmlName);
            var xhtmlOutput = _tf.SubOutput(project, xhtmlName);
            File.Copy(xhtmlInput, xhtmlOutput, overwrite);
            var cssName = project + ".css";
            var cssInput = _tf.Expected(cssName);
            var cssOutput = _tf.SubOutput(project, cssName);
            File.Copy(cssInput, cssOutput, overwrite);
            var p1 = new Process();
            p1.StartInfo.UseShellExecute = false;
            string arg = string.Format("-x \"{0}\" ", xhtmlOutput);
            arg += string.Format("-c \"{0}\" ", cssOutput);
            arg += string.Format("-t \"{0}\" ", backend);
            arg += string.Format("-i {0} ", inputType);
            arg += string.Format("-n \"{0}\" ", project);
            p1.StartInfo.Arguments = arg;
            p1.StartInfo.WorkingDirectory = _tf.Output(null);
            p1.StartInfo.FileName = Common.PathCombine(PathwayPath.GetPathwayDir(), "PathwayB.exe");
            p1.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            p1.Start();
            if (p1.Id <= 0)
                throw new MissingSatelliteAssemblyException(project);
            p1.WaitForExit();
            if (backend == "OpenOffice")
                OdtCheck(project, message);
            else if (backend == "InDesign")
                IdmlCheck(project, message);
        }

        private static void IdmlCheck(string project, string message)
        {
            var idmlExpect = _tf.SubExpected(project, project + ".idml");
            var idmlOutput = _tf.SubOutput(project, project + ".idml");
            IdmlTest.AreEqual(idmlExpect, idmlOutput, message);
        }

        private static void OdtCheck(string project, string message)
        {
            var odtExpectDir = _tf.Expected(project);
            var odtOutputDir = _tf.Output(project);
            OdtTest.AreEqual(odtExpectDir, odtOutputDir, message);
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

        #region Nkonya Sample
        /// <summary>
        /// Export Nkonya Sample TE data from Fieldworks, compare results to previous exports
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void NkonyaSampleExportTest()
        {
            FieldWorksXhtmlExport("Te", "Nkonya Sample", "Nkonya Sample 2011-02-10 1347.fwbackup", "Export changed");
        }
        #endregion Nkonya Sample

        #region Gondwana Sample
        /// <summary>
        /// Export Gondwana Sample Flex data from Fieldworks, compare results to previous exports
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void GondwanaSampleExportTest()
        {
            FieldWorksXhtmlExport("Flex", "Gondwana Sample", "Gondwana Sample 2011-02-09 0709.fwbackup", "Export changed");
        }
        #endregion Gondwana Sample

        #region Gondwana Sample Open Office
        /// <summary>
        /// Gondwana Sample Open Office Back End Test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void GondwanaSampleOpenOfficeTest()
        {
            PathawyB("Gondwana Sample", "Dictionary", "OpenOffice");
        }
        #endregion Gondwana Sample Open Office

        #region Nkonya Sample Open Office
        /// <summary>
        /// Nkonya Sample Open Office Back End Test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void NkonyaSampleOpenOfficeTest()
        {
            PathawyB("Nkonya Sample", "Scripture", "OpenOffice");
        }
        #endregion Nkonya Sample Open Office

        #region Gondwana Sample InDesign
        /// <summary>
        /// Gondwana Sample InDesign Back End Test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void GondwanaSampleInDesignTest()
        {
            PathawyB("Gondwana Sample", "Dictionary", "InDesign");
        }
        #endregion Gondwana Sample InDesign

        #region Nkonya Sample InDesign
        /// <summary>
        /// Nkonya Sample InDesign Back End Test
        /// </summary>
        [Test]
        [Category("LongTest")]
        [Category("SkipOnTeamCity")]
        public void NkonyaSampleInDesignTest()
        {
            PathawyB("Nkonya Sample", "Scripture", "InDesign");
        }
        #endregion Nkonya Sample InDesign
    }
}