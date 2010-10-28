using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.Tool;

namespace Test.UIConfigurationToolBLTest
{
    [TestFixture]
    public class ConfigurationToolBLTest
    {
        private ConfigurationTool cTool;
        /// <summary>holds path to input folder for all tests</summary>
        private static string _inputBasePath = string.Empty;
        /// <summary>holds path to expected results folder for all tests</summary>
        private static string _expectBasePath = string.Empty;
        /// <summary>holds path to output folder for all tests</summary>
        private static string _outputBasePath = string.Empty;
        private string _supportSource = string.Empty;
        private static string _pathwayPath = string.Empty;
        private ArrayList stylename;

        #region SetUp Method
        [TestFixtureSetUp]
        protected void SetUp()
        {
            cTool = new ConfigurationTool();
            cTool._fromNunit = true;
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/ConfigurationTool/TestFiles");
            _inputBasePath = Common.PathCombine(testPath, "Input");
            _expectBasePath = Common.PathCombine(testPath, "Expected");
            _outputBasePath = Common.PathCombine(testPath, "Output");

            _pathwayPath = Common.PathCombine(Common.GetAllUserAppPath(), "SIL/Pathway");

            _supportSource = Common.DirectoryPathReplace(testPath + "/../../../PsSupport");



            stylename = new ArrayList
                            {
                                "OneColumn",
                                "TwoColumn",
                                "LikeBuangPNG",
                                "FieldWorksStyles",
                                "FieldWorksArabicBased",
                                "Draft"
                            };

            string folderName = "styles";
            CopyFolderSupportToIO(folderName);

            folderName = "Graphic";
            CopyFolderSupportToIO(folderName);

            folderName = "Icons";
            CopyFolderSupportToIO(folderName);
        }

        private void CopyFile()
        {
            string fileName = "DictionaryStyleSettings.xml";
            CopyFilesSupportToPathway(fileName, "Dictionary");

            fileName = "ScriptureStyleSettings.xml";
            CopyFilesSupportToPathway(fileName, "Scripture");

            fileName = "StyleSettings.xml";
            CopyFilesSupportToPathway(fileName, "");
        }

        private void CopyFilesSupportToPathway(string fileName, string type)
        {
            const string schemaFile = "StyleSettings.xsd";
            string fromFileName = Common.PathCombine(_supportSource, fileName);
            string partialPath = Common.PathCombine(_pathwayPath, type);
            string toFileName = Common.PathCombine(partialPath, fileName);
            if (!Directory.Exists(partialPath))
            {
                Directory.CreateDirectory(partialPath);
            }
            File.Copy(fromFileName, toFileName, true);

            fromFileName = Common.PathCombine(_supportSource, schemaFile);
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

        private static void LoadParam()
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

        //[Test]
        public void LoadTest()
        {
            CopyFile();
            LoadParam();
            cTool._CToolBL.ConfigurationTool_LoadBL();
            //cTool._CToolBL.ConfigurationTool_FormClosingBL();
            GridRowCount_Load();
            GridRowValue_Load();
            FormButtonEnable_Load();
        }

        [Test]
        public void NewWithDefaultTest()
        {
            CopyFile();
            LoadParam();
            cTool._CToolBL.ConfigurationTool_LoadBL();
            cTool._CToolBL.tsNew_ClickBL();
            cTool.TabControl1.SelectedIndex = 1;
            cTool._CToolBL.tabControl1_SelectedIndexChangedBL();
            int SelectedRowIndex = cTool.StylesGrid.RowCount - 1;
            string actualStyleName = cTool.StylesGrid[0, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("CustomSheet-1", actualStyleName, "GridRowValueTest Test Failes");
            string actual = cTool.StylesGrid[1, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("C5 ruled double column with Flex formatting", actual.Trim(), "GridRowValueTest Test Failes");
            actual = cTool.StylesGrid[4, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("Yes", actual, "GridRowValueTest Test Failes");
            actual = cTool.StylesGrid[2, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtApproved.Text;
            Assert.AreEqual("", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlPagePageSize.Text;
            Assert.AreEqual("C5", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageInside.Text;
            Assert.AreEqual("57pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageOutside.Text;
            Assert.AreEqual("57pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageTop.Text;
            Assert.AreEqual("57pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageBottom.Text;
            Assert.AreEqual("57pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlPageColumn.Text;
            Assert.AreEqual("2", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageGutterWidth.Text;
            Assert.AreEqual("12pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlJustified.Text;
            Assert.AreEqual("No", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlVerticalJustify.Text;
            Assert.AreEqual("Top", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlPicture.Text;
            Assert.AreEqual("Yes", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlLeading.Text;
            Assert.AreEqual("No Change", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlRunningHead.Text;
            Assert.AreEqual("Mirrored", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlRules.Text;
            Assert.AreEqual("Yes", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlFontSize.Text;
            Assert.AreEqual("No Change", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlFileProduceDict.Text;
            Assert.AreEqual("One", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlSense.Text;
            Assert.AreEqual("No Change", actual, "GridRowValueTest Test Failes");
        }

        [Test]
        public void SaveAsWithDefaultTest()
        {
            CopyFile();
            LoadParam();
            cTool._CToolBL.ConfigurationTool_LoadBL();
            cTool._CToolBL.tsSaveAs_ClickBL();
            cTool.TabControl1.SelectedIndex = 1;
            cTool._CToolBL.tabControl1_SelectedIndexChangedBL();
            int SelectedRowIndex = cTool.StylesGrid.RowCount - 1;
            string actualStyleName = cTool.StylesGrid[0, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("Copy of FieldWorksStyles", actualStyleName, "GridRowValueTest Test Failes");
            string actual = cTool.StylesGrid[1, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("Based on GPS stylesheet FieldWorksStyles", actual.Trim(), "GridRowValueTest Test Failes");
            actual = cTool.StylesGrid[4, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("Yes", actual, "GridRowValueTest Test Failes");
            actual = cTool.StylesGrid[2, SelectedRowIndex].Value.ToString();
            Assert.AreEqual("", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtApproved.Text;
            Assert.AreEqual("", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlPagePageSize.Text;
            Assert.AreEqual("C5", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageInside.Text;
            Assert.AreEqual("57pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageOutside.Text;
            Assert.AreEqual("57pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageTop.Text;
            Assert.AreEqual("57pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageBottom.Text;
            Assert.AreEqual("57pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlPageColumn.Text;
            Assert.AreEqual("2", actual, "GridRowValueTest Test Failes");
            actual = cTool.TxtPageGutterWidth.Text;
            Assert.AreEqual("12pt", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlJustified.Text;
            Assert.AreEqual("No", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlVerticalJustify.Text;
            Assert.AreEqual("Top", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlPicture.Text;
            Assert.AreEqual("Yes", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlLeading.Text;
            Assert.AreEqual("No Change", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlRunningHead.Text;
            Assert.AreEqual("Mirrored", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlRules.Text;
            Assert.AreEqual("Yes", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlFontSize.Text;
            Assert.AreEqual("No Change", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlFileProduceDict.Text;
            Assert.AreEqual("One", actual, "GridRowValueTest Test Failes");
            actual = cTool.DdlSense.Text;
            Assert.AreEqual("No Change", actual, "GridRowValueTest Test Failes");
        }

        [Test]
        public void DeleteWithDefaultTest()
        {
            CopyFile();
            LoadParam();
            cTool._CToolBL.ConfigurationTool_LoadBL();
            cTool._CToolBL.tsNew_ClickBL();
            int afterNew = cTool.StylesGrid.RowCount;
            Assert.AreEqual(7, afterNew, "New Count Test Failes");
            cTool._CToolBL.tsDelete_ClickBL();
            int afterDelete = cTool.StylesGrid.RowCount;
            Assert.AreEqual(6, afterDelete, "New Count Test Failes");
            //cTool._CToolBL.ConfigurationTool_LoadBL();
        }


        private void AssignNewTest()
        {
            cTool.TxtName.Text = "NewStyle";
            cTool.TxtDesc.Text = "NewDescription";
            cTool._CToolBL.txtDesc_KeyUpBL();
            cTool.ChkAvailable.Checked = true;
            cTool._CToolBL.chkAvailable_CheckedChangedBL();
            cTool.TxtComment.Text = "NewComment";
            cTool._CToolBL.txtComment_KeyUpBL();
            cTool.TxtApproved.Text = "Tester";
            cTool._CToolBL.txtApproved_ValidatedBL(cTool.TxtApproved);
            cTool.DdlPagePageSize.Text = "A4";
            cTool.TxtPageInside.Text = "18pt";
            cTool.TxtPageOutside.Text = "18pt";
            cTool.TxtPageTop.Text = "18pt";
            cTool.TxtPageBottom.Text = "18pt";
            cTool.DdlPageColumn.Text = "2";
            cTool.TxtPageGutterWidth.Text = "6pt";
            cTool.DdlJustified.Text = "Yes";
            cTool.DdlVerticalJustify.Text = "Center";
            cTool.DdlPicture.Text = "Yes";
            cTool.DdlLeading.Text = "12";
            cTool.DdlRunningHead.Text = "Every Page";
            cTool.DdlRules.Text = "Yes";
            cTool.DdlFontSize.Text = "11";
            cTool.DdlFileProduceDict.Text = "One";
            cTool.DdlSense.Text = "Bullet";
            //Mobile Properties
            //cTool.DdlFiles.Text = "";
            //cTool.DdlRedLetter.Text = "";
            //cTool.TxtInformation.Text = "";
            //cTool.TxtCopyright.Text = "";
        }

        private void GetStyleName(string expStylename)
        {
            string expectedStyleName = expStylename;
            int SelectedRowIndex = cTool.StylesGrid.RowCount - 1;
            string actualStyleName = cTool.StylesGrid[0, SelectedRowIndex].Value.ToString();
            Assert.IsTrue(expectedStyleName == actualStyleName, "GetStyleName Test failed");
        }

        private void GridRowCount_Load()
        {
            const int expectedRowCount = 6;
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
            Assert.IsFalse(buttonOthers, "Others button enable Test failed");
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
