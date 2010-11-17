// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2009, SIL International. All Rights Reserved.
// <copyright file="PrintVia.cs" from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class PrintVia : Form, IExportContents, IScriptureContents
    {
        #region PrintVia Constructors

        private static string _helpTopic = string.Empty;
        private string _installedPath = Common.ProgInstall;
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
            if (!Common.isRightFieldworksVersion())
            {
                MessageBox.Show("Please download and install a Pathway version compatible with your software", "Incompatible Pathway Version", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
            }

            if (this.Text != "Set Defaults")
            {
                Param.LoadSettings();
                ValidateXMLVersion(Param.SettingPath);
            }
            Param.SetValue(Param.InputType, InputType);
            Param.LoadSettings();
            ScriptureAdjust();
            LoadBackEnds();
            LoadLayouts();
            LoadExtraProcessing();
            LoadProperty();
            txtSaveInFolder.Text = Common.GetSaveInFolder(Param.DefaultValue[Param.PublicationLocation], DatabaseName, cmbSelectLayout.Text);
            var iType = true;
            Common.PathwayHelpSetup(iType, Common.FromRegistry("Help"));
            Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
            Common.HelpProv.SetHelpKeyword(this, _helpTopic);
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
            ////string xPathLayouts = "//styles/" + _media + "/style";
            //string xPathLayouts = "//styles/*/style[@approvedBy='GPS' or @shown='Yes']";
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
            cmbSelectLayout.SelectedIndex = cmbSelectLayout.FindStringExact(cmbSelectLayout.Text);
            if (cmbSelectLayout.SelectedIndex == -1)
            {
                cmbSelectLayout.SelectedIndex = 0;
                cmbSelectLayout.Text = cmbSelectLayout.Items[0].ToString();
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
                DialogResult dialogResult = MessageBox.Show("Please Install the Plugin Backends", "Pathway", MessageBoxButtons.AbortRetryIgnore,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                //var msg = new[] { "Please Install the Plugin Backends" };
                //LocDB.Message("defErrMsg", "Please Install the Plugin Backends", msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
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
                Directory.Delete(txtSaveInFolder.Text);
                BtnOk.Enabled = false; 
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a folder for which you have creation permission", "Pathway", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.Yes;
            SaveProperty(this);
            if (Text.Contains("Default"))
                SaveDefaultProperty(this);
            _publicationName = Path.GetFileName(txtSaveInFolder.Text);
            txtSaveInFolder.Text = Path.GetDirectoryName(txtSaveInFolder.Text);
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
                Param.SetValue(Param.PublicationLocation, dlg.SelectedPath);
                txtSaveInFolder.Text = dlg.SelectedPath;
            }
        }

        private void cmbSelectLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            _publicationName = cmbSelectLayout.Text;
            txtSaveInFolder.Text = Common.GetSaveInFolder(Param.DefaultValue[Param.PublicationLocation], DatabaseName, cmbSelectLayout.Text);
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
            //Media = Param.DefaultValue[Param.Media];
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
            BtnOk.Enabled = chkConfigDictionary.Checked || chkRevIndexes.Checked || chkGramSketch.Checked;
        }
        #endregion SetOkStatus

        private void BtnHelp_Click(object sender, EventArgs e)
        {
            var iType = true;
            //iType = InputType.ToLower() != "scripture";
            Common.PathwayHelpSetup(iType, Common.FromRegistry("Help"));
            Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
            Common.HelpProv.SetHelpKeyword(this, _helpTopic);
            SendKeys.Send("{F1}");
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
                string ProgFilesPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                string ConfigToolPath = Common.PathCombine(ProgFilesPath, @"SIL\Pathway7\ConfigurationTool.exe");
                if (!File.Exists(ConfigToolPath))
                    ConfigToolPath = Common.PathCombine(ProgFilesPath, @"SIL\Pathway\ConfigurationTool.exe");
                if (File.Exists(ConfigToolPath))
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
            BtnOk.Enabled = true; 
        }

        private string FindMedia()
        {
            string backend = cmbPrintVia.Text.ToLower();
            string media;

            if (backend == "gobible")
            {
                media = "mobile";
            }
            else if (backend == "e-book (.epub)")
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
            if(cmbPrintVia.Text.ToLower().IndexOf("openoffice") >= 0 )
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
            tt_PrintVia.SetToolTip(chkAvoidOdtCrash, " Large files may crash on exit after saving. \n Eliminating the style sheet solved this problem but doesn't allow user to change the styles. \n So initially leave this box unchecked, but if Open Office is crashing, \n you can probably avoid crashing by checking this box.");
        }

        private void chkSilMember_CheckedChanged(object sender, EventArgs e)
        {
            chkPolicy.Enabled = chkSilMember.Checked;
            if(!chkSilMember.Checked)
                chkPolicy.Checked = chkSilMember.Checked;
        }

        private void chkPolicy_CheckedChanged(object sender, EventArgs e)
        {
            btnPolicy.Enabled = chkPolicy.Checked;
        }

        private void btnPolicy_Click(object sender, EventArgs e)
        {

            string helpPath = Common.PathCombine(_installedPath, "help");
            string fileName = "Intellectual_Property.docx";
            string helpFile = Common.PathCombine(helpPath, fileName);

            try
            {
                Process.Start(helpFile);
            }
            catch
            {
            }
        }
    }
}
