// --------------------------------------------------------------------------------------------
// <copyright file="ConfigurationToolBLTest.cs" from='2009' to='2014' company='SIL International'>
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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.UIConfigurationToolBLTest
{
    [TestFixture]
    public class ConfigurationToolBLTest : ConfigurationToolBL
    {
        private new ConfigurationTool cTool;
        private ConfigurationToolBL cToolBL;
        /// <summary>holds path to input folder for all tests</summary>
        private static string _inputBasePath = string.Empty;
        /// <summary>holds path to output folder for all tests</summary>
        private static string _outputBasePath = string.Empty;
        private string _supportSource = string.Empty;
        private static string _pathwayPath = string.Empty;
        private ArrayList stylename;

        #region SetUp Method
        [TestFixtureSetUp]
        protected void Initialize()
        {
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/ConfigurationTool/TestFiles");
            _inputBasePath = Common.PathCombine(testPath, "input");
            _outputBasePath = Common.PathCombine(testPath, "Output");

            if (Directory.Exists(_outputBasePath))
            {
                DirectoryInfo di = new DirectoryInfo(_outputBasePath);
                Common.CleanDirectory(di);
            }
            Directory.CreateDirectory(_outputBasePath);
            _supportSource = PathPart.Bin(Environment.CurrentDirectory, "/../../DistFiles");

            string folderName = "Graphic";
            CopyFolderSupportToIO(folderName);

            folderName = "Icons";
            CopyFolderSupportToIO(folderName);
        }

        protected void SetUp()
        {
            cTool = new ConfigurationTool();
			Common.Testing = true;
            cToolBL = new ConfigurationToolBL();

            //_pathwayPath = Common.PathCombine(Common.GetAllUserAppPath(), "SIL/Pathway");
            _pathwayPath = Common.GetAllUserPath();

            stylename = new ArrayList
                            {
                                "OneColumn",
                                "TwoColumn",
                                "LikeBuangPNG",
                                "FieldWorksStyles",
                                "FieldWorksArabicBased",
                                "Draft"
                            };

            string folderName = "Styles";
            CopyFolderSupportToIO(folderName);
        }

        private void CopyFile()
        {
            string fileName = "StyleSettings.xml";
            CopyFilesSupportToPathway(fileName, "");

            fileName = "DictionaryStyleSettings.xml";
            CopyFilesSupportToPathway(fileName, "Dictionary");

            fileName = "ScriptureStyleSettings.xml";
            CopyFilesSupportToPathway(fileName, "Scripture");
        }

        private void CopyFilesSupportToPathway(string fileName, string type)
        {
            const string schemaFile = "StyleSettings.xsd";
            string fromFileName = Common.PathCombine(_supportSource, fileName);
            File.Copy(fromFileName, Common.PathCombine(_outputBasePath, fileName), true);
            string partialPath = Common.PathCombine(_pathwayPath, type);
            string toFileName = Common.PathCombine(partialPath, fileName);
            if (Directory.Exists(partialPath))
            {
                DirectoryInfo di = new DirectoryInfo(partialPath);
                Common.CleanDirectory(di);
            }
            Directory.CreateDirectory(partialPath);
            File.Copy(fromFileName, toFileName, true);

            fromFileName = Common.PathCombine(_supportSource, schemaFile);
            File.Copy(fromFileName, Common.PathCombine(_outputBasePath, schemaFile), true);
            toFileName = Common.PathCombine(partialPath, schemaFile);
            File.Copy(fromFileName, toFileName, true);
        }

        private void CopyFolderSupportToIO(string fileName)
        {
            string fromFileName = Common.PathCombine(_supportSource, fileName);
            string toFileName = Common.PathCombine(_inputBasePath, fileName);
            FolderTree.Copy(fromFileName, toFileName);

            toFileName = Common.PathCombine(_outputBasePath, fileName);
            FolderTree.Copy(fromFileName, toFileName);
        }

        private new static void LoadParam()
        {
            // Verifying the input setting file and css file - in Input Folder
            const string settingFile = "DictionaryStyleSettings.xml";
            string sFileName = Common.PathCombine(_outputBasePath, settingFile);
            Common.ProgBase = _outputBasePath;
            Param.LoadValues(sFileName);
            Param.SetLoadType = "Dictionary";
            Param.Value["OutputPath"] = _outputBasePath;
            Param.Value["UserSheetPath"] = _outputBasePath;
        }
        #endregion


        [Test]
        [Category("LongTest")]
        [Category("SkipOnTC")]
        public void NewWithDefaultTest()
        {
            SetUp();
            CopyFile();
            LoadParam();
            cTool._cToolBL = new ConfigurationToolBL();
            cTool._cToolBL.inputTypeBL = "Dictionary";
            cTool._cToolBL.MediaTypeEXE = "paper";
            cTool._cToolBL.StyleEXE = "OneColumn"; //
            cTool._cToolBL.SetClassReference(cTool);
            cTool._cToolBL.CreateToolTip();
            cTool._cToolBL.ConfigurationTool_LoadBL();
            cTool._cToolBL.tsNew_ClickBL();
            cTool.TabControl1.SelectedIndex = 1;
            cTool._cToolBL.tabControl1_SelectedIndexChangedBL();
            int SelectedRowIndex = cTool.StylesGrid.RowCount - 1;
            string actualStyleName = cTool.StylesGrid[0, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("CustomSheet-1", actualStyleName, "GridRowValueTest Test Failes");

			//We skipped because the dropdown value not loading in the Linux Testcases.
			if (Common.UsingMonoVM)
				return;

            string actual = cTool.StylesGrid[1, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("5.25x8.25in - 1 Col - Left aligned - Charis 11 on 13", actual.Trim(), "Grid description Test Failes");
            actual = cTool.StylesGrid[4, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("Yes", actual, "Grid available Test Failes");
            actual = cTool.StylesGrid[2, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("", actual, "Grid comment Test Failes");
            actual = cTool.TxtApproved.Text;
            Assert.AreEqual("", actual, "Grid approvedby Test Failes");
            actual = cTool.DdlPagePageSize.Text;
            Assert.AreEqual("5.25in x 8.25in", actual, "Grid page size Test Failes");
            actual = cTool.TxtPageInside.Text;
            Assert.AreEqual("36pt", actual, "Grid page inside Test Failes");
            actual = cTool.TxtPageOutside.Text;
            Assert.AreEqual("36pt", actual, "Grid page outside Test Failes");
            actual = cTool.TxtPageTop.Text;
            Assert.AreEqual("36pt", actual, "Grid page top Test Failes");
            actual = cTool.TxtPageBottom.Text;
            Assert.AreEqual("36pt", actual, "Grid page bottom Test Failes");
            actual = cTool.DdlPageColumn.Text;
            Assert.AreEqual("1", actual, "Grid page column Test Failes");
            actual = cTool.TxtPageGutterWidth.Text;
            Assert.AreEqual("18pt", actual, "Grid page gutter width Test Failes");
            actual = cTool.DdlJustified.Text;
            Assert.AreEqual("No", actual, "Grid justify Test Failes");
            actual = cTool.DdlVerticalJustify.Text;
            Assert.AreEqual("Top", actual, "Grid vertical justify Test Failes");
            actual = cTool.DdlPicture.Text;
            Assert.AreEqual("Yes", actual, "Grid picture Test Failes");
            actual = cTool.DdlLeading.Text;
            Assert.AreEqual("13", actual, "Grid leading Test Failes");
            actual = cTool.DdlRunningHead.Text;
            Assert.AreEqual("Mirrored", actual, "Grid mirrored Test Failes");
            actual = cTool.DdlRules.Text;
            Assert.AreEqual("Yes", actual, "Grid rules Test Failes");
            actual = cTool.DdlFontSize.Text;
			Assert.AreEqual("No Change", actual, "Grid font size Test Failes");
            actual = cTool.DdlFileProduceDict.Text;
            Assert.AreEqual("One", actual, "Grid file produce Test Failes");
            actual = cTool.DdlSense.Text;
            Assert.AreEqual("No change", actual, "Grid sense Test Failes");

            cTool.Close();
        }

        [Test]
        [Category("LongTest")]
        [Category("SkipOnTC")]
        public void SaveAsWithDefaultTest()
        {
            SetUp();
            CopyFile();
            LoadParam();
            cTool._cToolBL = new ConfigurationToolBL();
            cTool._cToolBL.inputTypeBL = "Dictionary";
            cTool._cToolBL.MediaTypeEXE = "paper";
            cTool._cToolBL.StyleEXE = "OneColumn"; //
            cTool._cToolBL.SetClassReference(cTool);
            cTool._cToolBL.CreateToolTip();
            cTool._cToolBL.ConfigurationTool_LoadBL();
            cTool._cToolBL.tsSaveAs_ClickBL();
            cTool.TabControl1.SelectedIndex = 1;
            cTool._cToolBL.tabControl1_SelectedIndexChangedBL();
            int SelectedRowIndex = cTool.StylesGrid.RowCount - 1;
            string actualStyleName = cTool.StylesGrid[0, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("Copy of OneColumn", actualStyleName, "GridRowValueTest Test Failes");
            string actual = cTool.StylesGrid[1, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("Based on GPS stylesheet OneColumn", actual.Trim(), "GridRowValueTest Test Failes");
            actual = cTool.StylesGrid[4, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("Yes", actual, "GridRowValueTest Test Failes");
            actual = cTool.StylesGrid[2, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtApproved.Text;
            Assert.AreEqual("", actual, "GridRowValueTest Test Failes");

			//We skipped because the dropdown value not loading in the Linux Testcases.
			if (Common.UsingMonoVM)
				return;

            actual = cTool.DdlPagePageSize.Text;
            Assert.AreEqual("5.25in x 8.25in", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageInside.Text;
            Assert.AreEqual("36pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageOutside.Text;
            Assert.AreEqual("36pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageTop.Text;
            Assert.AreEqual("36pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageBottom.Text;
            Assert.AreEqual("36pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlPageColumn.Text;
            Assert.AreEqual("1", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageGutterWidth.Text;
            Assert.AreEqual("18pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlJustified.Text;
            Assert.AreEqual("No", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlVerticalJustify.Text;
            Assert.AreEqual("Top", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlPicture.Text;
            Assert.AreEqual("Yes", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlLeading.Text;
            Assert.AreEqual("13", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlRunningHead.Text;
            Assert.AreEqual("Mirrored", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlRules.Text;
            Assert.AreEqual("Yes", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlFontSize.Text;
			Assert.AreEqual("No Change", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlFileProduceDict.Text;
            Assert.AreEqual("One", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlSense.Text;
            Assert.AreEqual("No change", actual, "GridRowValueTest Test Failes");
            cTool.Close();
        }

        [Test]
        [Category("LongTest")]
        public void DeleteWithDefaultTest()
        {
            SetUp();
            CopyFile();
            LoadParam();
            cTool._cToolBL = new ConfigurationToolBL();
            cTool._cToolBL.inputTypeBL = "Dictionary";
            cTool._cToolBL.MediaTypeEXE = "paper";
            cTool._cToolBL.StyleEXE = "Draft"; //
            cTool._cToolBL.SetClassReference(cTool);
            cTool._cToolBL.CreateToolTip();
            cTool._cToolBL.ConfigurationTool_LoadBL();
            cTool._cToolBL.tsNew_ClickBL();
            int afterNew = cTool.StylesGrid.RowCount;
            Assert.Greater(afterNew, 1, "New Count Test Fails");
            cTool._cToolBL.tsDelete_ClickBL();
            int afterDelete = cTool.StylesGrid.RowCount;
            Assert.AreEqual(afterNew - 1, afterDelete, "New Count Test Fails");
            cTool.Close();
        }

        [Test]
        [Category("ShortTest")]
        public void SaveInputTypeTest()
        {
            SetUp();
            CopyFile();
            Param.Value["OutputPath"] = _outputBasePath;
            Common.SaveInputType("Scripture");
            const string expected = "Scripture";
            var xdoc = new XmlDocument();
            xdoc.Load(Common.PathCombine(Common.GetAllUserPath(), "StyleSettings.xml"));
            XmlNode node = xdoc.SelectSingleNode("//stylePick/settings/property[@name='InputType']");
            if (node != null)
                if (node.Attributes != null)
                    Assert.IsTrue(node.Attributes["value"].Value == expected, "SaveInputType Test failed");
        }

        [Test]
        public void CheckStylesString()
        {
            string returnValue = string.Empty;
            cToolBL = new ConfigurationToolBL();
            returnValue = cToolBL.GenerateStylesString();
            Assert.IsTrue(returnValue == "Styles", "Values not equal");

        }

        [Test]
        public void GetMailBodyTest()
        {
            IsUnixOs = Common.IsUnixOS();
            string returnValue = GetMailBody("Dictionary", "Extract Zip contents to an appropriate folder.%0D%0A%0D%0A");
            if (IsUnixOs)
            {
                Assert.IsTrue(returnValue.Contains("Ubuntu"), "missing Ubuntu");
            }
            else
            {
                Assert.IsTrue(returnValue.Contains("7 and 8"), "Missing 8");
            }
        }

		[Test]
	    public void GetPageNumberListWhenEveryPage()
		{
			Common.Testing = true;
			cTool = new ConfigurationTool();
			cToolBL = new ConfigurationToolBL(cTool);
			Param.LoadSettings();
			var rHItem = new SIL.PublishingSolution.ComboBoxItem("Mirrored", "Mirrored");
			cTool.DdlRunningHead.Items.Add(rHItem);
			cTool.DdlRunningHead.SelectedItem = cTool.DdlRunningHead.Items[0];
			ReloadPageNumberLocList("Every Page", cTool);
			ComboBox result = cToolBL.cTool.DdlPageNumber;
			var expected = new ComboBox();
			var item = new SIL.PublishingSolution.ComboBoxItem("Top Center", "Top Center");
			expected.Items.Add(item);
			item = new SIL.PublishingSolution.ComboBoxItem("Bottom Center", "Bottom Center");
			expected.Items.Add(item);
			for (int i = 0; i < result.Items.Count; i++)
			{
				Assert.AreEqual(expected.Items[i].ToString(), result.Items[i].ToString(), String.Format("PageNumber[{0}] is loaded wrongly for EveryPage", expected.Items[i]));
			}
			Common.Testing = false;
		}

		[Test]
		public void GetPageNumberListWhenMirrored()
		{
			Common.Testing = true;
			cTool = new ConfigurationTool();
			cToolBL = new ConfigurationToolBL(cTool);
			Param.LoadSettings();
			var rHItem = new SIL.PublishingSolution.ComboBoxItem("Mirrored", "Mirrored");
			cTool.DdlRunningHead.Items.Add(rHItem);
			cTool.DdlRunningHead.SelectedItem = cTool.DdlRunningHead.Items[0];
			ReloadPageNumberLocList("Mirrored", cTool);
			ComboBox result = cToolBL.cTool.DdlPageNumber;
			var expected = new ComboBox();
			var item = new SIL.PublishingSolution.ComboBoxItem("Top Inside Margin", "Top Inside Margin");
			expected.Items.Add(item);
			item = new SIL.PublishingSolution.ComboBoxItem("Top Center", "Top Center");
			expected.Items.Add(item);
			item = new SIL.PublishingSolution.ComboBoxItem("Bottom Inside Margin", "Bottom Inside Margin");
			expected.Items.Add(item);
			item = new SIL.PublishingSolution.ComboBoxItem("Bottom Outside Margin", "Bottom Outside Margin");
			expected.Items.Add(item);
			item = new SIL.PublishingSolution.ComboBoxItem("Bottom Center", "Bottom Center");
			expected.Items.Add(item);
			for (int i = 0; i < result.Items.Count; i++)
			{
				Assert.AreEqual(expected.Items[i].ToString(), result.Items[i].ToString(), String.Format("PageNumber[{0}] is loaded wrongly for Mirrored", expected.Items[i]));
			}
			Common.Testing = false;
		}

		[Test]
		public void GetPageNumberListWhenNone()
		{
			Common.Testing = true;
			cTool = new ConfigurationTool();
			cToolBL = new ConfigurationToolBL(cTool);
			Param.LoadSettings();
			var rHItem = new SIL.PublishingSolution.ComboBoxItem("None", "None");
			cTool.DdlRunningHead.Items.Add(rHItem);
			cTool.DdlRunningHead.SelectedItem = cTool.DdlRunningHead.Items[0];
			ReloadPageNumberLocList("None", cTool);
			ComboBox result = cToolBL.cTool.DdlPageNumber;
			var expected = new ComboBox();
			var item = new SIL.PublishingSolution.ComboBoxItem("None", "None");
			expected.Items.Add(item);
			for (int i = 0; i < result.Items.Count; i++)
			{
				Assert.AreEqual(expected.Items[i].ToString(), result.Items[i].ToString(), String.Format("PageNumber[{0}] is loaded wrongly for None", expected.Items[i]));
			}
			Common.Testing = false;
		}

	    private void GridRowCount_Load()
        {
            const int expectedRowCount = 11;
            int rowCount = cTool.StylesGrid.Rows.Count;
            Assert.IsTrue(rowCount == expectedRowCount, "GridRowCount Test failed");
        }

        private void GridRowValue_Load()
        {
            const int ColumnName = 0;
            var outputStyleList = new ArrayList();
            for (int i = 0; i < cTool.StylesGrid.Rows.Count; i++)
            {
                outputStyleList.Add(cTool.StylesGrid[ColumnName, i].Value.ToString());
            }
            Assert.AreEqual(stylename, outputStyleList, "GridRowValueTest Test Failes");
        }

        private void FormButtonEnable_Load()
        {
            int s = cTool.StylesGrid.Rows.Count;
            InfoTabEnable();
            DisplayTabEnable();
            NewButtonEnable();
            SaveAsButtonEnable();
            DeleteButtonEnable(false);
            PreviewButtonEnable();
            DefaultButtonEnable();
            SendButtonEnable();
            ResetButtonEnable();
            PaperButtonEnable();
            MobileButtonEnable();
            WebButtonEnable();
            OthersButtonEnable();
            DictionaryButtonColor();
            ScriptureButtonColor();
        }

        private void ScriptureButtonColor()
        {
            //To check the button Scripture
            Color btnScriptureColor = cTool.BtnScripture.BackColor;
            Assert.IsTrue(SystemColors.Control == btnScriptureColor, "Scripture backColor Test failed");
        }

        private void DictionaryButtonColor()
        {
            //To check the button dictionary
            Color btnDictionaryColor = cTool.BtnDictionary.BackColor;
            Assert.IsTrue(SystemColors.InactiveBorder == btnDictionaryColor, "dictionary backColor Test failed");
        }

        private void OthersButtonEnable()
        {
            //To check the Others button disable property
            bool buttonOthers = cTool.BtnOthers.Enabled;
            Assert.IsTrue(buttonOthers, "Others button enable Test failed");
        }

        private void WebButtonEnable()
        {
            //To check the Web button disable property
            bool buttonWeb = cTool.BtnWeb.Enabled;
            Assert.IsFalse(buttonWeb, "Web button enable Test failed");
        }

        private void MobileButtonEnable()
        {
            //To check the Mobile button disable property
            bool buttonMobile = cTool.BtnMobile.Enabled;
            Assert.IsFalse(buttonMobile, "Mobile button enable Test failed");
        }

        private void PaperButtonEnable()
        {
            //To check the Paper button enable property
            bool buttonPaper = cTool.BtnPaper.Enabled;
            Assert.IsTrue(buttonPaper, "Paper button enable Test failed");
        }

        private void SendButtonEnable()
        {
            //To check the Send button button enable property
            bool buttonSend = cTool.TsSend.Enabled;
            Assert.IsTrue(buttonSend, "Send button enable Test failed");
        }

        private void DefaultButtonEnable()
        {
            //To check the Default button enable property
            bool buttonDefault = cTool.TsDefault.Enabled;
            Assert.IsTrue(buttonDefault, "Default button enable Test failed");
        }

        private void PreviewButtonEnable()
        {
            //To check the Preview button enable property
            bool buttonPreview = cTool.TsPreview.Enabled;
            Assert.IsTrue(buttonPreview, "Preview button enable Test failed");
        }

        private void DeleteButtonEnable(bool status)
        {
            //To check the Delete button button enable property
            bool buttonSend = cTool.TsDelete.Enabled;
            Assert.AreEqual(status, buttonSend, "Delete button enable Test failed");
        }

        private void SaveAsButtonEnable()
        {
            //To check the saveas button enable property
            bool buttonSaveAs = cTool.TsSaveAs.Enabled;
            Assert.IsTrue(buttonSaveAs, "SaveAs button enable Test failed");
        }

        private void NewButtonEnable()
        {
            //To check the new button enable property
            bool buttonNew = cTool.TsNew.Enabled;
            Assert.IsTrue(buttonNew, "New button enable Test failed");
        }

        private void ResetButtonEnable()
        {
            //To check the Preview button enable property
            bool buttonPreview = cTool.TsReset.Enabled;
            Assert.IsTrue(buttonPreview, "Reset button enable Test failed");
        }

        private void DisplayTabEnable()
        {
            //To check the Display Tab sense visiblity
            bool tabDisplayEnable = cTool.TabDisplay.Enabled;
            Assert.IsFalse(tabDisplayEnable, "Tab Display enable  Test failed");
        }

        private void InfoTabEnable()
        {
            //To check the info Tab enable property
            bool tabInfoEnable = cTool.TabInfo.Enabled;
            Assert.IsFalse(tabInfoEnable, "Tab Info enable Test failed");
        }

    }
}
