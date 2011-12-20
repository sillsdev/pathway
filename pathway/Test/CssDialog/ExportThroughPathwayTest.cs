// --------------------------------------------------------------------------------------------
#region // Copyright © 2011, SIL International. All Rights Reserved.
// <copyright file="ExportThroughPathwayTest.cs" from='2011' to='2011' company='SIL International'>
//		Copyright © 2011, SIL International. All Rights Reserved.   
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
using System.Collections;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.CssDialog
{
    public class ExportThroughPathwayTest : ExportThroughPathway
    {
        #region Setup
        [TestFixtureSetUp]
        protected void SetUp()
        {
            Common.Testing = true;
            InputType = "Dictionary";
            Common.ProgInstall = PathwayPath.GetPathwayDir();
            Param.LoadSettings();
            Param.SetValue(Param.InputType, InputType);
            Param.LoadSettings();
        }
        #endregion Setup

        #region TearDown
        [TestFixtureTearDown]
        protected void TearDown()
        {
            Backend.Load(string.Empty);
            Param.UnLoadValues();
            Common.ProgInstall = string.Empty;
            Common.SupportFolder = string.Empty;
            Param.SetLoadType = string.Empty;
        }
        #endregion TearDown

        [Test]
        [Category("SkipOnTeamCity")]
        public void LoadAvailFormatsTest()
        {
            LoadAvailFormats();
            string lastItem = string.Empty;
            foreach (string item in DdlLayout.Items)
            {
                Assert.Greater(string.Compare(item, lastItem), 0, string.Format("This item was {0} and last item was {1}. They should be ascending.", item, lastItem));
                lastItem = item;
            }
        }
    }
}