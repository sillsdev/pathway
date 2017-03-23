// --------------------------------------------------------------------------------------------
// <copyright file="PrintVia.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed:
//
// <remarks>
// Dialog to enter the settings for printing Dictionaries from Flex (or orthers)
// </remarks>
// --------------------------------------------------------------------------------------------


using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using L10NSharp;
using SilTools;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class PrintVia : Form, IExportContents, IScriptureContents
    {
        #region PrintVia Constructors

        private static string _helpTopic = string.Empty;
        public bool _fromPlugIn;
        public PrintVia()
        {
            InitializeComponent();
            _helpTopic = "User_Interface/Dialog_boxes/Print_via.htm";
            _fromPlugIn = true;
        }
        public PrintVia(string mode)
        {
            InitializeComponent();
            Text = mode;
            _helpTopic = "User_Interface/Dialog_boxes/Set_Defaults_dialog_box.htm";
            _fromPlugIn = false;
        }
        #endregion PrintVia Constructors

        #region Properties

        public string DatabaseName { set; get; }

        private static string _media = "paper";
        public string Media
        {
            get
            {
                return _media;
            }
            set
            {
                _media = value;
            }
        }

        public bool ExportMain
        {
            set
            {
                chkConfigDictionary.Checked = value;
            }
            get
            {
                return chkConfigDictionary.Checked;
            }
        }

        public bool ExportReversal
        {
            set
            {
                chkRevIndexes.Checked = value;
            }
            get
            {
                return chkRevIndexes.Checked;
            }
        }

        public bool ExportGrammar
        {
            set
            {
                chkGramSketch.Checked = value;
            }
            get
            {
                return chkGramSketch.Checked;
            }
        }

        public bool ReversalExists
        {
            set
            {
                chkRevIndexes.Enabled = chkRevIndexes.Checked = value;
            }
        }

        public bool GrammarExists
        {
            set
            {
                chkGramSketch.Enabled = chkGramSketch.Checked = value;
            }
        }

        public bool ExistingDirectoryInput
        {
            get
            {
                return false;
            }
        }

        public bool ExistingPublication
        {
            get
            {
                return false;
            }
        }

        public string OutputLocationPath
        {
            get
            {
                return txtSaveInFolder.Text;
            }
        }

        public string ExistingDirectoryLocationPath
        {
            get
            {
                return string.Empty;
            }
        }

        public string ExistingLocationPath
        {
            get
            {
                return string.Empty;
            }
        }

        public string DictionaryName
        {
            set
            {
                cmbSelectLayout.Text = value;
                _publicationName = value;
            }
            get
            {
                return _publicationName;
            }
        }

        public string PublicationName
        {
            set
            {
                cmbSelectLayout.Text = value;
                _publicationName = value;
            }
            get
            {
                return _publicationName;
            }
        }

        private static string _publicationName;
        private string _newSaveInFolderPath = string.Empty;

        public string BackEnd
        {
            set
            {
                cmbPrintVia.Text = value;
            }
            get
            {
                return cmbPrintVia.Text;
            }
        }

        public bool ExtraProcessing
        {
            set
            {
                chkExtraProcessing.Checked = value;
            }
            get
            {
                return chkExtraProcessing.Checked;
            }
        }

        public string InputType { set; get; }
        #endregion Properties

        private void PrintVia_Load(object sender, EventArgs e)
        {
			string formText = LocalizationManager.GetString("ExportThroughPathway.Form.Text", "Set Defaults", "");
			string strDefault = formText.ToString();
            if (!Common.isRightFieldworksVersion())
            {
                var text = LocalizationManager.GetString("PrintVia.PrintViaLoad.Message", "Please download and install a Pathway version compatible with your software", "");
				string caption = LocalizationManager.GetString("PrintVia.PrintViaLoad.Caption", "Incompatible Pathway Version", "");
				Utils.MsgBox(text, caption, MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
            }

            if (this.Text != strDefault)
            {
                Param.LoadSettings();
                ValidateXMLVersion(Param.SettingPath);
            }
            Param.SetValue(Param.InputType, InputType);
            Param.LoadSettings();
            ScriptureAdjust();
            LoadBackEnds();
            LoadLayouts();
            SetOkStatus();
            LoadExtraProcessing();
            LoadProperty();
            txtSaveInFolder.Text = Common.GetSaveInFolder(Param.DefaultValue[Param.PublicationLocation], DatabaseName, cmbSelectLayout.Text);
            ShowHelp.ShowHelpTopic(this, _helpTopic, Common.IsUnixOS(), false);
            Common.databaseName = DatabaseName;
        }

        private void LoadExtraProcessing()
        {
            string message = "Insert first and last word in header";
            if (Param.Value["InputType"] == Common.ProjectType.Scripture.ToString())
            {
                message = "Insert first and last book, chapter and / or verse in header";
            }
            chkExtraProcessing.Text = message;
        }

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

        #region ScriptureAdust
        private static int dictionaryContentTop;
        private static int nextControlTop;
        private static int delta;
        private void ScriptureAdjust()
        {
            if (string.IsNullOrEmpty(InputType) || InputType != "Scripture") return;
            dictionaryContentTop = chkConfigDictionary.Top;
            nextControlTop = BtnBrwsLayout.Top;
            delta = nextControlTop - dictionaryContentTop;
            foreach (Control control in Controls)
                ControlAdjust(control);
            foreach (Control control in groupBox1.Controls)
                ControlAdjust(control);
            groupBox1.Height -= delta;
            Height -= delta;
        }

        private static void ControlAdjust(Control control)
        {
            if (control.Top >= dictionaryContentTop && control.Top < nextControlTop)
            {
                control.Visible = false;
                control.Enabled = false;
            }
            else if (control.Top >= nextControlTop)
            {
                control.Top -= delta;
            }
        }
        #endregion ScriptureAdjust

        #region LoadLayouts
        private void LoadLayouts()
        {
            cmbSelectLayout.Items.Clear();

            ApprovedByValidation();

            ShownValidation();

            if (cmbSelectLayout.Items.Count > 0)
            {
                cmbSelectLayout.SelectedIndex = cmbSelectLayout.FindStringExact(cmbSelectLayout.Text);
                if (cmbSelectLayout.SelectedIndex == -1)
                {
                    cmbSelectLayout.SelectedIndex = 0;
                    cmbSelectLayout.Text = cmbSelectLayout.Items[0].ToString();
                }
            }
        }

        private void ShownValidation()
        {
            string xPathLayouts = "//styles/" + _media + "/style[@approvedBy='GPS' or @shown='Yes']";
            XmlNodeList stylenames = Param.GetItems(xPathLayouts);
            foreach (XmlNode stylename in stylenames)
            {
                var shown = true;
                foreach (XmlAttribute attr in stylename.Attributes)
                {
                    if (attr.Name == "shown")
                    {
                        shown = attr.Value == "Yes";
                        break;
                    }
                }
                if (shown)
                    cmbSelectLayout.Items.Add(stylename.Attributes["name"].Value);
            }
        }

        private void ApprovedByValidation()
        {
            bool isUpdated = false;
            bool isApprovedByExists = false;
            string xPathLayouts = "//styles/" + _media + "/style";
            XmlNodeList stylenames = Param.GetItems(xPathLayouts);
            foreach (XmlNode stylename in stylenames)
            {
                foreach (XmlAttribute attr in stylename.Attributes)
                {
                    if (attr.Name == "approvedBy")
                    {
                        isApprovedByExists = true;
                        if (attr.Value.Trim() == "")
                        {
                            attr.Value = "GPS";
                            isUpdated = true;
                        }
                    }
                }
                if (!isApprovedByExists)
                {
                    Param.AddAttrValue(stylename, "approvedBy", "GPS");
                    isUpdated = true;
                    isApprovedByExists = true;
                }
            }

            if (isUpdated)
            {
                Param.Write();
            }
        }

        #endregion LoadLayouts

        #region LoadBackEnds
        private void LoadBackEnds()
        {
            string BackendsPath = Common.ProgInstall;
            Backend.Load(BackendsPath);
            ArrayList exportType = Backend.GetExportType(InputType);
            if (exportType.Count > 0)
            {
                foreach (string item in exportType)
                {
                    cmbPrintVia.Items.Add(item);
                }
                cmbPrintVia.Text = cmbPrintVia.Items[0].ToString();
            }
            else
            {
                var message = LocalizationManager.GetString("PrintVia.LoadBackEnds.Message", "Please Install the Plugin Backends", "");
				string caption = LocalizationManager.GetString("PrintVia.LoadBackEnds.projectname", "Pathway", "");
				DialogResult dialogResult = Utils.MsgBox(message, caption, MessageBoxButtons.AbortRetryIgnore,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                if (dialogResult == DialogResult.Ignore)
                    return;
                if (dialogResult == DialogResult.Abort)
                    Close();
                else
                    LoadBackEnds();
            }
            cmbPrintVia.SelectedIndex = cmbPrintVia.FindStringExact(cmbPrintVia.Text);
        }
        #endregion LoadBackEnds

        #region Events
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(txtSaveInFolder.Text);
                if (Directory.Exists(txtSaveInFolder.Text))
                {
                    DirectoryInfo di = new DirectoryInfo(txtSaveInFolder.Text);
                    Common.CleanDirectory(di);
                }
                BtnOk.Enabled = false;
            }
            catch (Exception)
            {
                var text = LocalizationManager.GetString("PrintVia.OkButtonClick.Message", "Please select a folder for which you have creation permission", "");
				string caption = LocalizationManager.GetString("PrintVia.OkButtonClick.projectname", "Pathway", "");
				Utils.MsgBox(text, caption, MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.Yes;
            SaveProperty(this);
            if (Text.Contains("Default"))
                SaveDefaultProperty(this);
            _publicationName = Path.GetFileName(txtSaveInFolder.Text);
            txtSaveInFolder.Text = Path.GetDirectoryName(txtSaveInFolder.Text);
            Common.TimeStarted = DateTime.Now;
            Close();
        }

        private void btnBrwsSaveInFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dlg.SelectedPath = Common.PathCombine(documentsPath, Common.SaveInFolderBase);
            DirectoryInfo directoryInfo = new DirectoryInfo(dlg.SelectedPath);
            if (!directoryInfo.Exists)
                directoryInfo.Create();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string folderName = cmbSelectLayout.Text + "_" + DateTime.Now.ToString("yyyy-MM-dd_hhmmss");
                _newSaveInFolderPath = Common.PathCombine(dlg.SelectedPath, folderName);
                Param.SetValue(Param.PublicationLocation, _newSaveInFolderPath);
                txtSaveInFolder.Text = _newSaveInFolderPath;
            }
        }

        private void cmbSelectLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            _publicationName = cmbSelectLayout.Text;
            txtSaveInFolder.Text = Common.GetSaveInFolder(Param.DefaultValue[Param.PublicationLocation], DatabaseName, cmbSelectLayout.Text);
            if (_newSaveInFolderPath.Length > 0)
            {
                txtSaveInFolder.Text = Path.GetDirectoryName(_newSaveInFolderPath) + cmbSelectLayout.Text + "_" + DateTime.Now.ToString("yyyy-MM-dd_hhmmss");
            }

        }

        private void chkConfigDictionary_CheckedChanged(object sender, EventArgs e)
        {
            SetOkStatus();
        }

        private void chkRevIndexes_CheckedChanged(object sender, EventArgs e)
        {
            SetOkStatus();
        }

        private void chkGramSketch_CheckedChanged(object sender, EventArgs e)
        {
            SetOkStatus();
        }
        #endregion Events

        #region LoadProperty & SaveProperty
        private void LoadProperty()
        {
            BackEnd = Param.DefaultValue[Param.PrintVia];
            if (string.IsNullOrEmpty(InputType) || InputType == "Dictionary")
            {
                ExportMain = Param.DefaultValue[Param.ConfigureDictionary] == "True";
                if (chkRevIndexes.Enabled)
                    ExportReversal = Param.DefaultValue[Param.ReversalIndex] == "True";
                if (chkGramSketch.Enabled)
                    ExportGrammar = Param.DefaultValue[Param.GrammarSketch] == "True";
            }
            DictionaryName = Param.DefaultValue[Param.LayoutSelected];
            ExtraProcessing = Param.DefaultValue[Param.ExtraProcessing] == "True";
        }

        private static void SaveProperty(PrintVia dlg)
        {
            Param.SetValue(Param.PrintVia, dlg.BackEnd);
            if (string.IsNullOrEmpty(dlg.InputType) || dlg.InputType == "Dictionary")
            {
                Param.SetValue(Param.ConfigureDictionary, dlg.ExportMain.ToString());
                Param.SetValue(Param.ReversalIndex, dlg.ExportReversal.ToString());
                Param.SetValue(Param.GrammarSketch, dlg.ExportGrammar.ToString());
            }
            Param.SetValue(Param.LayoutSelected, dlg.DictionaryName);
            Param.SetValue(Param.ExtraProcessing, dlg.ExtraProcessing.ToString());
            Param.SetValue(Param.Media, _media);
            Param.SetValue(Param.PublicationLocation, dlg.OutputLocationPath);
            Param.Write();
        }

        private static void SaveDefaultProperty(PrintVia dlg)
        {
            Param.SetDefaultValue(Param.PrintVia, dlg.BackEnd);
            if (string.IsNullOrEmpty(dlg.InputType) || dlg.InputType == "Dictionary")
            {
                Param.SetDefaultValue(Param.ConfigureDictionary, dlg.ExportMain.ToString());
                Param.SetDefaultValue(Param.ReversalIndex, dlg.ExportReversal.ToString());
                Param.SetDefaultValue(Param.GrammarSketch, dlg.ExportGrammar.ToString());
            }
            Param.SetDefaultValue(Param.LayoutSelected, dlg.DictionaryName);
            Param.SetDefaultValue(Param.ExtraProcessing, dlg.ExtraProcessing.ToString());
            Param.SetDefaultValue(Param.Media, _media);
            if (Common.CustomSaveInFolder(dlg.OutputLocationPath))
                Param.SetValue(Param.PublicationLocation, dlg.OutputLocationPath);
            Param.Write();
        }
        #endregion LoadProperty & SaveProperty

        #region SetOkStatus
        private void SetOkStatus()
        {
            if (Param.Value.Count == 0)
                return;
            BtnOk.Enabled = (Param.Value["InputType"] == Common.ProjectType.Scripture.ToString());
            BtnOk.Enabled = BtnOk.Enabled || chkConfigDictionary.Checked || chkRevIndexes.Checked || chkGramSketch.Checked;
            BtnOk.Enabled = BtnOk.Enabled && chkPolicy.Checked;
        }
        #endregion SetOkStatus

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            ShowHelp.ShowHelpTopicKeyPress(this, _helpTopic, Common.IsUnixOS());
        }

        private void BtnBrwsLayout_Click(object sender, EventArgs e)
        {
            string settingPath = Path.GetDirectoryName(Param.SettingPath);
            string inputPath = Common.PathCombine(settingPath, "Styles");
            inputPath = Common.PathCombine(inputPath, Param.Value["InputType"]);
            string stylenamePath = Common.PathCombine(inputPath, "Preview");

            PreviewPrintVia previewPrintVia = new PreviewPrintVia(_media, stylenamePath, EnableEdit());
            previewPrintVia.SelectedStyle = cmbSelectLayout.Text;
            previewPrintVia.InputType = InputType;
            previewPrintVia.ShowDialog();
            LoadLayouts();
            if (previewPrintVia.DialogResult == DialogResult.OK)
            {
                cmbSelectLayout.Text = previewPrintVia.SelectedStyle;
            }
        }

        private bool EnableEdit()
        {
            if (_fromPlugIn)
            {
                // Take into account custom directory installs - just look for the ConfigurationTool.exe
                // in the same directory as this .dll.
                string fullPathwayPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(PrintVia)).CodeBase);
                // if the path returned starts with file://, trim that part out
                string pathwayPath = (fullPathwayPath.StartsWith("file"))
                                         ? fullPathwayPath.Substring(6)
                                         : fullPathwayPath;
                if (File.Exists(Common.PathCombine(pathwayPath, "ConfigurationTool.exe")))
                {
                    return true;
                }
            }
            return false;
        }

        private void cmbPrintVia_SelectedIndexChanged(object sender, EventArgs e)
        {
            _media = FindMedia();

            LoadLayouts();

            ShowAvoidOdtCrash();
            SetOkStatus();
        }

        private string FindMedia()
        {
            string backend = cmbPrintVia.Text.ToLower();
            string media;

            if (backend == "go bible")
            {
                media = "mobile";
            }
            else if (backend == "e-book (.epub)" || backend == "browser (html5)")
            {
                media = "others";
            }
            else
            {
                media = "paper";
            }
            return media;
        }

        private void ShowAvoidOdtCrash()
        {
            if (cmbPrintVia.Text.ToLower().IndexOf("libreoffice") >= 0)
            {
                chkAvoidOdtCrash.Visible = true;
            }
            else
            {
                chkAvoidOdtCrash.Visible = false;
            }
        }

        private void ChkAvoidOdtCrashMouseHover(object sender, EventArgs e)
        {
            tt_PrintVia.SetToolTip(chkAvoidOdtCrash, "Large files (500 or more pages) can cause Open Office to crash after a Save operation.\nIf you experience crashes, select this option to eliminate the stylesheet and apply the style\nattributes as direct formatting. This makes subsequent formatting changes inconvenient.");
        }

        private void chkPolicy_CheckedChanged(object sender, EventArgs e)
        {
            SetOkStatus();
        }

        private void hlPropertyInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _helpTopic = "Concepts/Intellectual_Property.htm";
            BtnHelp_Click(sender, e);
        }
    }
}
