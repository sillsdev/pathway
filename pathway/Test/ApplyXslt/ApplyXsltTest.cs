// --------------------------------------------------------------------------------------------
// <copyright file="ApplyXslt.cs" from='2011' to='2011' company='SIL International'>
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
// tool to apply xslt in custom process
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Resources;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using System.IO;
using NUnit.Framework;
using SIL.Tool;

#endregion Using

namespace Test.ApplyXslt
{
    [TestFixture]
    public class ApplyXsltTest
    {
        #region Setup
        private static TestFiles _tf;

        /// <summary>
        /// setup Input, Expected, and Output paths relative to location of program
        /// </summary>
        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.Testing = true;
            _tf = new TestFiles("ApplyXslt");
        }
        #endregion Setup

        [Test]
        public void HorseTrackTest()
        {
            var xsltFullName = GetFileNameWithSupportPath("RemoveEmptyDiv.xsl");
            const string defaultExtension = "-1.xhtml";
            Common.XsltProcess(_tf.Copy("in.xhtml"), xsltFullName, defaultExtension);
        }

        #region private Methods
        private static string GetSupportPath()
        {
            return PathPart.Bin(Environment.CurrentDirectory, "/../PsSupport");
        }
        private string GetFileNameWithSupportPath(string name)
        {
            return Common.PathCombine(GetSupportPath(), name);
        }
        #endregion
    }
}
