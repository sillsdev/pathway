// --------------------------------------------------------------------------------------------
// <copyright file="ExportThroughPathwayTest.cs" from='2009' to='2014' company='SIL International'>
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

using System.IO;
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
			string pathwayDirectory = Common.AssemblyPath;
            string styleSettingFile = Common.PathCombine(pathwayDirectory, "StyleSettings.xml");

			if (!File.Exists(styleSettingFile))
			{
				styleSettingFile = Path.GetDirectoryName(Common.AssemblyPath);
				styleSettingFile = Common.PathCombine(styleSettingFile, "StyleSettings.xml");
			}

            Common.Testing = true;
            ValidateXMLVersion(styleSettingFile);
            InputType = "Dictionary";
            Common.ProgInstall = pathwayDirectory;
            Param.LoadSettings();
            Param.SetValue(Param.InputType, InputType);
            Param.LoadSettings();
        }
        #endregion Setup

        #region ValidateXMLVersion
        private void ValidateXMLVersion(string filePath)
        {
            var versionControl = new SettingsVersionControl();
            var Validator = new SettingsValidator();
            if (File.Exists(filePath))
            {
                versionControl.UpdateSettingsFile(filePath);
                bool isValid = Validator.ValidateSettingsFile(filePath, true);
                if (!isValid)
                {
                    this.Close();
                }
            }
        }
        #endregion

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
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void LoadAvailFormatsTest()
        {
            LoadAvailFormats();
            var backends = Backend.GetExportType("Scripture");
            Assert.Less(0, backends.Count);
            string lastItem = string.Empty;
            foreach (string item in DdlLayout.Items)
            {
                Assert.Greater(string.Compare(item, lastItem), 0, string.Format("This item was {0} and last item was {1}. They should be ascending.", item, lastItem));
                lastItem = item;
            }
        }

		/// <summary>
		///A test for InvalidCharsTest
		///</summary>
		[Test]
		public void InvalidCharsTest()
		{
			OutputFolder = @"C:\Turkish Stuff-Texts-07\Dictionary";
			Assert.AreEqual(@"C:\Turkish_Stuff-Texts-07\Dictionary", ReplaceInvalidChars(OutputFolder), "InvalidCharsTest failed");
		}
    }
}