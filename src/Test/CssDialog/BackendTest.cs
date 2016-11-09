// --------------------------------------------------------------------------------------------
// <copyright file="BackendTest.cs" from='2009' to='2014' company='SIL International'>
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

using System.Collections;
using NUnit.Framework;
using SIL.Tool;

namespace Test.CssDialog
{
    public class BackendTest
    {
        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.Testing = true;
			Common.ProgInstall = Common.AssemblyPath;
            Backend.Load(Common.ProgInstall);
        }
        #endregion Setup

        #region TearDown
        [TestFixtureTearDown]
        protected void TearDown()
        {
            Backend.Load(string.Empty);
            Common.ProgInstall = string.Empty;
            Common.SupportFolder = string.Empty;
        }
        #endregion TearDown

        [Test]
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void GetExportTypeTest()
        {
            ArrayList exportType = Backend.GetExportType("Dictionary");
            ArrayList check = new ArrayList(exportType.Count);
            foreach (string item in exportType)
            {
                Assert.False(check.Contains(item));
                check.Add(item);
            }
        }
    }
}