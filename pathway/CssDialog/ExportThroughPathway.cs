// --------------------------------------------------------------------------------------
// <copyright file="ExportThroughPathway.cs" from='2009' to='2011' company='SIL International'>
//      Copyright © 2009, 2011 SIL International. All Rights Reserved.
//
//      Distributable under the terms specified in the LICENSING.txt file.
// </copyright>
// <author>Erik Brommers</author>
// <email>erik_brommers@sil.org</email>
// Last reviewed:
// 
// <remarks>
// Export Through Pathway dialog (replacement for the PrintVia dialog).
// This dialog is designed to be a simple-to-complex dialog for exporting to the
// various Pathway output formats and stylesheets.
//
// When the user displays this dialog, it checks the <>StyleSettings.xml file
// for the user's Organization. If not found (which will be the case for first launch
// after installation and when the user holds the shift key down to clean out the
// settings), the dialog will display the SelectOrganizationDialog to select one. 
// Each organization can have its own default values for publisher, copyright, etc.;
// specifying an organization allows an organization to pre-fill some of the dialog
// fields for the user. These fields can be overridden by the user.
//
// A fuller description of this dialog can be found in the JIRA issue attachments
// for TD-2344: https://www.jira.insitehome.org/secure/attachment/27139/PubInfo_Proposal_v5.docx
// </remarks>
// --------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class ExportThroughPathway : Form, IExportContents, IScriptureContents
    {
        #region DdlLayout
        protected ComboBox DdlLayout
        {
            get { return ddlLayout; }
        }
        #endregion DdlLayouts
        private static string _publicationName;
        private string _newSaveInFolderPath = string.Empty;
        private static string _helpTopic = string.Empty;
        public bool _fromPlugIn;
        private static string _media = "paper";
        private SettingsHelper _settingsHelper;
        public List<string> XsltFile = new List<string>();

        public ExportThroughPathway()
        {
            InitializeComponent();
            _helpTopic = "User_Interface/Dialog_boxes/Export_Through_Pathway_dialog_box.htm";
            _fromPlugIn = true;
        }

        public ExportThroughPathway(string mode)
        {
            InitializeComponent();
            Text = mode;
            _helpTopic = "User_Interface/Dialog_boxes/Set_Defaults_dialog_box.htm";
            _fromPlugIn = false;
        }

        #region Properties

        public string Organization { get; set; }
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
        public bool IsExpanded { get; set; }

        // main area
        public string Format
        {
            set { ddlLayout.Text = value; }
            get { return ddlLayout.Text; }

        }
        public string Style
        {
            get { return ddlStyle.Text; }
            set { ddlStyle.Text = value; }
        }
        public string InputType { set; get; }

        #region InterfaceMethods
        public string ExistingDirectoryLocationPath
        {
            get
            {
                return string.Empty;
            }
        }

        public string DictionaryName { set; get; }
        public string DatabaseName { set; get; }
        string IScriptureContents.OutputLocationPath
        {
            get { return OutputFolder; }
        }
        public bool ExistingPublication
        {
            get
            {
                return false;
            }
        }
        public string ExistingLocationPath
        {
            get
            {
                return string.Empty;
            }
        }
        public string PublicationName
        {
            set
            {
                ddlStyle.Text = value;
                _publicationName = value;
            }
            get
            {
                return _publicationName;
            }
        }
        public bool ExportReversal
        {
            get { return chkReversalIndexes.Checked; }
            set { chkReversalIndexes.Checked = value; }
        }
        public bool ExportGrammar
        {
            get { return chkGrammarSketch.Checked; }
            set { chkGrammarSketch.Checked = value; }
        }

        public bool ReversalExists
        {
            set { chkReversalIndexes.Checked = value; }
        }

        public bool GrammarExists
        {
            set { chkGrammarSketch.Checked = value; }
        }

        string IExportContents.OutputLocationPath
        {
            get { return OutputFolder; }
        }

        public bool ExistingDirectoryInput
        {
            // EDB 5/10/2011 - code "cockroach" from PrintVia.cs - this doesn't appear to do anything.
            get { return false; }
        }

        public bool ExportMain
        {
            get { return chkConfiguredDictionary.Checked; }
            set { chkConfiguredDictionary.Checked = value; }
        }
        #endregion InterfaceMethods

        // Publication Info (dublin core metadata) tab
        /// <summary>
        /// dc:title element
        /// </summary>
        public string Title // dc:title
        {
            get { return CleanText(txtBookTitle.Text); }
            set { txtBookTitle.Text = value; }
        }
        /// <summary>
        /// dc:description element
        /// </summary>
        public string Description // dc:description
        {
            get { return CleanText(txtDescription.Text); }
            set { txtDescription.Text = value; }
        }
        /// <summary>
        /// dc:creator element
        /// </summary>
        public string Creator // dc:creator
        {
            get { return CleanText(txtCreator.Text); }
            set { txtCreator.Text = value; }
        }
        /// <summary>
        /// dc:publisher element
        /// </summary>
        public string Publisher // dc:publisher
        {
            get { return CleanText(txtPublisher.Text); }
            set { txtPublisher.Text = value; }
        }
        /// <summary>
        /// dc:rights element
        /// </summary>
        public string CopyrightHolder // dc:rights
        {
            get { return CleanText(txtRights.Text); }
            set { txtRights.Text = value; }
        }
        // DC properties that are not in the UI
        /// <summary>
        /// dc:type element. Not in the UI.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// dc:source element. Not in the UI.
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// dc:format element. Not in the UI.
        /// </summary>
        public string DocFormat { get; set; } // really "Format" / dc:format
        /// <summary>
        /// dc:contributor element. Not in the UI.
        /// </summary>
        public string Contributor { get; set; }
        /// <summary>
        /// dc:relation element. Not in the UI.
        /// </summary>
        public string Relation { get; set; }
        /// <summary>
        /// dc:coverage element. Not in the UI.
        /// </summary>
        public string Coverage { get; set; }
        /// <summary>
        /// dc:subject element. Not in the UI.
        /// </summary>
        public string Subject { get; set; } // probably just copy over the default value for this
        /// <summary>
        /// dc:date element. Not in the UI.
        /// </summary>
        public string Date { get; set; } // date modified?
        /// <summary>
        /// dc:language element. Not in the UI.
        /// </summary>
        public string Language { get; set; } // main language for document
        /// <summary>
        /// dc:identifier element. Not in the UI (uniquely identifies the document).
        /// </summary>
        public string Identifier { get; set; } // unique ID

        // Front Matter tab
        public bool CoverPage
        {
            get { return chkCoverImage.Checked; }
            set { chkCoverImage.Checked = value; }
        }
        public string CoverPageImagePath { get; set; }
        public bool CoverPageTitle
        {
            get { return chkCoverImageTitle.Checked; }
            set { chkCoverImageTitle.Checked = value; }
        }
        public bool TitlePage
        {
            get { return chkTitlePage.Checked; }
            set { chkTitlePage.Checked = value; }
        }
        public bool CopyrightPage
        {
            get { return chkColophon.Checked; }
            set { chkColophon.Checked = value; }
        }
        public bool TableOfContents
        {
            get { return chkTOC.Checked; }
            set { chkTOC.Checked = value; }
        }
        /// <summary>
        /// Full path and filename of the Copyright page (xhtml) file.
        /// </summary>
        public string CopyrightPagePath
        {
            get { return txtColophonFile.Text.Trim(); }
            set { txtColophonFile.Text = value; }
        }

        // Processing Options tab
        public bool RunningHeader
        {
            get { return chkRunningHeader.Checked; }
            set { chkRunningHeader.Checked = value; }
        }
        public bool ReduceNumberOfStyles
        {
            get { return chkOOReduceStyleNames.Checked; }
            set { chkOOReduceStyleNames.Checked = value; }
        }
        public string OutputFolder
        {
            get { return txtSaveInFolder.Text; }
            set { txtSaveInFolder.Text = value; }
        }
        #endregion Properties

        private void ExportThroughPathway_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Common.isRightFieldworksVersion())
                {
                    MessageBox.Show("Please download and install a Pathway version compatible with your software", "Incompatible Pathway Version", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    DialogResult = DialogResult.Cancel;
                    Close();
                }

                PopulateFilesFromPreprocessingFolder();

                // Load the .ssf or .ldml as appropriate
                _settingsHelper = new SettingsHelper(DatabaseName);
                _settingsHelper.LoadValues();

                if (this.Text != "Set Defaults")
                {
                    // not setting defaults, just opening the dialog:
                    // load the settings file and migrate it if necessary
                    Param.LoadSettings();
                    Param.SetValue(Param.InputType, InputType);
                    Param.LoadSettings();
                    ValidateXMLVersion(Param.SettingPath);
                }
                else
                {
                    // setting defaults: just load the settings file (don't migrate)
                    Param.SetValue(Param.InputType, InputType);
                    Param.LoadSettings();
                    // add the input type (to give a little more information to the user)
                    Text += " - " + InputType;
                }

                // get the current organization
                Organization = Param.GetOrganization();

                if (Organization == "")
                {
                    // no organization set yet -- display the Select Organization dialog
                    var dlg = new SelectOrganizationDialog(InputType);
                    if (Text.Contains("Set Defaults"))
                    {
                        // if we're setting defaults, provide a clue as to what they're setting the defaults for
                        dlg.Text += " - " + InputType;
                    }
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        Organization = dlg.Organization;
                        PopulateFromSettings();
                    }
                    else
                    {
                        // User pressed cancel - exit out of the export process altogether
                        DialogResult = DialogResult.Cancel;
                        Close();
                    }
                }
                LoadAvailFormats();
                LoadAvailStylesheets();
                IsExpanded = false;
                ResizeDialog();
                SetOkStatus();
                LoadProperty();
                EnableUIElements();
                Common.PathwayHelpSetup();
                Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
                Common.HelpProv.SetHelpKeyword(this, _helpTopic);
                if (AppDomain.CurrentDomain.FriendlyName.ToLower().IndexOf("configurationtool") == -1)
                {
                    Common.databaseName = DatabaseName;
                }
            }
            catch { }
        }


        private void PopulateFilesFromPreprocessingFolder()
        {
            string xsltFullName = Common.PathCombine(Common.GetApplicationPath(), "Preprocessing\\");// + xsltFile[0]
            xsltFullName = Common.PathCombine(xsltFullName, InputType + "\\"); //TD-2871 - separate dictionary and Scripture xslt

            if (Directory.Exists(xsltFullName))
            {
                string[] filePaths = Directory.GetFiles(xsltFullName, "*.xsl");

                // In case the xsl file name change and updated in the psexport.cs file XsltPreProcess

                foreach (var filePath in filePaths)
                {
                    chkLbPreprocess.Items.Add(Path.GetFileNameWithoutExtension(filePath), false);
                }
            }
        }

        //<property name="ReversalIndexes" value="False" />
        //<property name="RemoveEmptyDiv" value="false" />


        /// <summary>
        /// Attempts to pull in values from the Settings helper (either from the .ldml or
        /// .ssf file) that should override the StyleSettings.xml defaults. There are only
        /// a couple values that fall in this category.
        /// </summary>
        private void PopulateFromSettings()
        {
            string valueFromSettings;
            // title / full name
            if (_settingsHelper.Value.TryGetValue("FullName", out valueFromSettings))
            {
                Title = valueFromSettings;
            }
            // copyright holder
            if (_settingsHelper.Value.TryGetValue("Copyright", out valueFromSettings))
            {
                CopyrightHolder = valueFromSettings;
            }
        }

        /// <summary>
        /// Expands or contracts the dialog, based on the current value of the IsExpanded property.
        /// </summary>
        private void ResizeDialog()
        {
            // Show / hide the tab control
            tabControl1.Visible = IsExpanded;

            // Move the bottom UI elements where they need to go
            chkIP.Top = tabControl1.Top + ((IsExpanded) ? (tabControl1.Height + 10) : 0);
            lnkIP.Top = chkIP.Top + chkIP.Height + 3;
            btnOK.Top = lnkIP.Top + lnkIP.Height + 15;
            btnCancel.Top = btnOK.Top;
            btnHelp.Top = btnOK.Top;

            // Resize the dialog
            Height = SystemInformation.CaptionHeight + btnOK.Bottom + 15;//(IsExpanded) ? 587 : 265;

            // Set the text and image on the More / Less Options button
            btnMoreLessOptions.Text = (IsExpanded) ? "Less" : "More";
            btnMoreLessOptions.Image = (IsExpanded) ? Properties.Resources.go_up : Properties.Resources.go_down;
        }

        /// <summary>
        /// Show or hide UI elements on the page depending on the output type, format and style
        /// </summary>
        private void EnableUIElements()
        {
            // Front Matter tab
            // edb - temporary / remove as exports are implemented
            if (!ddlLayout.Text.Contains("epub") && !ddlLayout.Text.Contains("OpenOffice/LibreOffice") && !ddlLayout.Text.Contains("InDesign") && !ddlLayout.Text.Contains("XeLaTex"))
            {
                tabPage2.Enabled = false;
                chkCoverImage.Enabled = false;
                chkCoverImageTitle.Enabled = false;
                btnCoverImage.Enabled = false;
                imgCoverImage.Enabled = false;
                chkColophon.Enabled = false;
                rdoCustomCopyright.Enabled = false;
                rdoStandardCopyright.Enabled = false;
                txtColophonFile.Enabled = false;
                btnBrowseColophon.Enabled = false;
                chkTitlePage.Enabled = false;
                ddlCopyrightStatement.Enabled = false;
                lnkChooseCopyright.Enabled = false;
            }
            else
            {
                tabPage2.Enabled = true;
                chkTitlePage.Enabled = true;
                chkCoverImage.Enabled = (ddlLayout.Text.Contains("epub") || ddlLayout.Text.Contains("OpenOffice/LibreOffice") || ddlLayout.Text.Contains("InDesign") || ddlLayout.Text.Contains("XeLaTex"));
                chkCoverImageTitle.Enabled = (chkCoverImage.Enabled && chkCoverImage.Checked);
                btnCoverImage.Enabled = chkCoverImageTitle.Enabled;
                imgCoverImage.Enabled = chkCoverImageTitle.Enabled;

                chkColophon.Enabled = (!ddlLayout.Text.Contains("Go Bible"));
                rdoCustomCopyright.Enabled = (chkColophon.Checked && chkColophon.Enabled);
                rdoStandardCopyright.Enabled = (chkColophon.Checked && chkColophon.Enabled);
                ddlCopyrightStatement.Enabled = (chkColophon.Checked && chkColophon.Enabled) ? rdoStandardCopyright.Checked : false;
                txtColophonFile.Enabled = (chkColophon.Checked && chkColophon.Enabled) ? rdoCustomCopyright.Checked : false;
                btnBrowseColophon.Enabled = (chkColophon.Checked && chkColophon.Enabled) ? rdoCustomCopyright.Checked : false;
                lnkChooseCopyright.Enabled = true;
            }

            //if (ddlLayout.Text.ToLower() == "xelatex")
            //{
            //    tabPage2.Enabled = true;
            //    chkCoverImage.Enabled = true;
            //    chkCoverImageTitle.Enabled = true;
            //    btnCoverImage.Enabled = true;
            //    imgCoverImage.Enabled = true;
            //    chkColophon.Enabled = true;
            //    rdoCustomCopyright.Enabled = true;
            //    rdoStandardCopyright.Enabled = true;
            //    txtColophonFile.Enabled = true;
            //    btnBrowseColophon.Enabled = true;
            //    chkTitlePage.Enabled = true;
            //    ddlCopyrightStatement.Enabled = true;
            //    lnkChooseCopyright.Enabled = true;
            //}



            // Processing Options tab
            chkRunningHeader.Enabled = (FindMedia() == "paper");
            chkOOReduceStyleNames.Enabled = (ddlLayout.Text.Contains("LibreOffice"));
            if (InputType != "Dictionary")
            {
                grpInclude.Visible = false;
                chkConfiguredDictionary.Visible = false;
                chkReversalIndexes.Visible = false;
                chkGrammarSketch.Visible = false; // currently this is false anyways (it's not implemented)
            }

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

        #region LoadAvailStylesheets
        private void LoadAvailStylesheets()
        {
            ddlStyle.Items.Clear();

            ApprovedByValidation();

            ShownValidation();

            if (ddlStyle.Items.Count > 0)
            {
                ddlStyle.SelectedIndex = ddlStyle.FindStringExact(ddlStyle.Text);
                if (ddlStyle.SelectedIndex == -1)
                {
                    ddlStyle.SelectedIndex = 0;
                    ddlStyle.Text = ddlStyle.Items[0].ToString();
                }
            }
            // if there is only 1 stylesheet, disable the dropdown
            ddlStyle.Enabled = ddlStyle.Items.Count != 1;
        }

        private void ShownValidation()
        {
            //string xPathLayouts = "//styles/" + _media + "/style[@approvedBy='GPS' or @shown='Yes']";
            string xPathLayouts = "//styles/" + _media + "/style[@shown='Yes']";
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
                    ddlStyle.Items.Add(stylename.Attributes["name"].Value);
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
        #endregion LoadAvailStylesheets

        #region LoadAvailFormats
        protected void LoadAvailFormats()
        {
            string BackendsPath = Common.ProgInstall;
            Backend.Load(BackendsPath);
            ArrayList exportType = Backend.GetExportType(InputType);
            exportType.Sort();
            if (exportType.Count > 0)
            {
                foreach (string item in exportType)
                {
                    if (item.Trim().ToLower() == "webonary" && !IsWebDataFilled())
                    {
                        continue;
                    }
                    ddlLayout.Items.Add(item);
                }
                ddlLayout.Text = ddlLayout.Items[0].ToString();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Pathway was unable to find any export formats. Please reinstall Pathway to correct this error.", "Pathway", MessageBoxButtons.AbortRetryIgnore,
                                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                //var msg = new[] { "Please Install the Plugin Backends" };
                //LocDB.Message("defErrMsg", "Please Install the Plugin Backends", msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                if (dialogResult == DialogResult.Ignore)
                    return;
                if (dialogResult == DialogResult.Abort)
                    Close();
                else
                    LoadAvailFormats();
            }
            ddlLayout.SelectedIndex = ddlLayout.FindStringExact(ddlLayout.Text);
        }
        #endregion LoadAvailFormats

        private bool IsWebDataFilled()
        {
            Param.LoadSettings();
            XmlNodeList baseNode1 = Param.GetItems("//styles/" + "web" + "/style[@name='" + "OneWeb" + "']/styleProperty");
            
            foreach (XmlNode VARIABLE in baseNode1)
            {
                string attribName = VARIABLE.Attributes["name"].Value.ToLower();
                string attribValue = VARIABLE.Attributes["value"].Value;
                switch (attribName)
                {
                    case "ftpaddress":
                    case "ftpuserid":
                    case "dbservername":
                    case "dbname":
                    case "dbuserid":
                    case "weburl":
                    case "webadminusrnme":
                    case "webadminpwd":
                    case "webadminsitenme":
                    case "webemailid":
                    case "webftpfldrnme":
                        if (attribValue.Trim().Length == 0)
                        {
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        #region Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(OutputFolder);
                Directory.Delete(OutputFolder);
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a folder for which you have creation permission", "Pathway", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            
            ProcessSendingHelpImprove();

            if (!File.Exists(CoverPageImagePath))
            {
                CoverPageImagePath = Common.PathCombine(Common.GetApplicationPath(), "Graphic\\cover.png");
            }

            //if (Format != "YouVersion")
            //{
            // attempt to save the properties - if it doesn't work, leave the dialog open
            if (SaveProperty(this))
            {
                DialogResult = DialogResult.Yes;
                if (Text.Contains("Default"))
                    SaveDefaultProperty(this);
                _publicationName = Path.GetFileName(OutputFolder);
                OutputFolder = Path.GetDirectoryName(OutputFolder);
                DictionaryName = _publicationName;
                Common.TimeStarted = DateTime.Now;
                _settingsHelper.ClearValues();
            }

            Param.LoadSettings();
            if (Param.Value.ContainsKey(Param.Preprocessing))
            {
                var preprocessing = string.Empty;
                foreach (string chkBoxName in chkLbPreprocess.CheckedItems)
                {
                    if (preprocessing.Length > 0)
                        preprocessing += ",";
                    preprocessing += chkBoxName;
                }
                Param.SetValue(Param.Preprocessing, preprocessing);
            }
            Param.Write();
            //}
            this.Close();
        }

        private static void ProcessSendingHelpImprove()
        {
            string getApplicationPath = Common.GetApplicationPath();
            string helpImproveCommand = Path.Combine(getApplicationPath, "HelpImprove.exe");
            string registryPath = "Software\\SIL\\Pathway";

            if (Common.GetOsName().ToLower() == "windows7")
                registryPath = "Software\\Wow6432Node\\SIL\\Pathway";

            if (File.Exists(helpImproveCommand))
                Common.RunCommand(helpImproveCommand, string.Format("{0} {1} {2}", "204.93.172.30", registryPath, "HelpImprove"), 0);
        }

        #endregion Events

        #region LoadProperty & SaveProperty
        private void LoadProperty()
        {
            // format (destination) and style
            Format = Param.DefaultValue[Param.PrintVia];
            Style = Param.DefaultValue[Param.LayoutSelected];
            // publication info tab
            Param.DatabaseName = DatabaseName;
            if (Title.Trim().Length < 1)
            {
                Title = Param.GetMetadataValue(Param.Title, Organization);
            }
            Description = Param.GetMetadataValue(Param.Description, Organization);
            Creator = Param.GetMetadataValue(Param.Creator, Organization);
            Publisher = Param.GetMetadataValue(Param.Publisher, Organization);
            if (CopyrightHolder.Trim().Length < 1)
            {
                CopyrightHolder = Param.GetMetadataValue(Param.CopyrightHolder, Organization);
            }
            Type = Param.GetMetadataValue(Param.Type, Organization);
            Source = Param.GetMetadataValue(Param.Source, Organization);
            DocFormat = Param.GetMetadataValue(Param.Format, Organization);
            Contributor = Param.GetMetadataValue(Param.Contributor, Organization);
            Relation = Param.GetMetadataValue(Param.Relation, Organization);
            Coverage = Param.GetMetadataValue(Param.Coverage, Organization);
            Subject = Param.GetMetadataValue(Param.Subject, Organization);
            Date = Param.GetMetadataValue(Param.Date, Organization);
            Language = Param.GetMetadataValue(Param.Language, Organization);
            Identifier = Param.GetMetadataValue(Param.Identifier, Organization);

            // front matter tab
            CoverPage = (Param.GetMetadataValue(Param.CoverPage, Organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CoverPage, Organization));
            CoverPageImagePath = Param.GetMetadataValue(Param.CoverPageFilename, Organization);
            if (!string.IsNullOrEmpty(CoverPageImagePath) && CoverPageImagePath.Trim().Length > 0)
            {
                try
                {
                    Image iconImage = Image.FromFile(CoverPageImagePath);
                    imgCoverImage.Image = iconImage;
                }
                catch
                {
                    imgCoverImage.Image = imgCoverImage.InitialImage;
                }
            }
            TitlePage = (Param.GetMetadataValue(Param.TitlePage, Organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.TitlePage, Organization));
            CoverPageTitle = (Param.GetMetadataValue(Param.CoverPageTitle, Organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CoverPageTitle, Organization));
            CopyrightPage = (Param.GetMetadataValue(Param.CopyrightPage, Organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CopyrightPage, Organization));
            CopyrightPagePath = Param.GetMetadataValue(Param.CopyrightPageFilename, Organization);
            TableOfContents = (Param.GetMetadataValue(Param.TableOfContents, Organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.TableOfContents, Organization));
            // license agreement
            XmlNode node;
            // try to get the key
            node = Param.GetItem("//features/feature[@name='Lic_" + XmlConvert.EncodeName(Organization) + "']");
            if (node == null)
            {
                // key not found - fall back on the generic licenses
                node = Param.GetItem("//features/feature[@name='Lic_Other']");
            }
            if (node != null)
            {
                int index;
                foreach (XmlNode subnode in node.ChildNodes)
                {
                    if (subnode.Attributes != null)
                    {
                        var value = subnode.Attributes["name"].Value;
                        if (!ddlCopyrightStatement.Items.Contains(value))
                        {
                            // not currently in the list - add it now
                            index = ddlCopyrightStatement.Items.Add(value);
                            if (Path.GetFileName(CopyrightPagePath) == subnode.Attributes["file"].Value)
                            {
                                // this is the selected copyright statement - 
                                // select it in the dropdown and check the standard radio button
                                ddlCopyrightStatement.SelectedIndex = index;
                                rdoStandardCopyright.Checked = true;
                            }
                        }
                    }
                }
            }
            rdoCustomCopyright.Checked = !(rdoStandardCopyright.Checked);

            // Processing Options tab)
            if (string.IsNullOrEmpty(InputType) || InputType == "Dictionary")
            {
                ExportMain = Param.DefaultValue[Param.ConfigureDictionary] == "True";
                if (chkReversalIndexes.Enabled)
                    ExportReversal = Param.DefaultValue[Param.ReversalIndex] == "True";
                if (chkGrammarSketch.Enabled)
                    ExportGrammar = Param.DefaultValue[Param.GrammarSketch] == "True";
            }
            DictionaryName = Param.DefaultValue[Param.LayoutSelected];
            RunningHeader = Param.DefaultValue[Param.ExtraProcessing] == "True";
            Media = Param.DefaultValue[Param.Media];

            //Fillup XSLT Processing Checkboxes
            var preprocess = Param.Value.ContainsKey(Param.Preprocessing)? Param.Value[Param.Preprocessing]: string.Empty;
            for (int i = 0; i < chkLbPreprocess.Items.Count; i++)
            {
                chkLbPreprocess.SetItemCheckState(i,preprocess.Contains(chkLbPreprocess.Items[i].ToString())? CheckState.Checked : CheckState.Unchecked);
            }
        }

        /// <summary>
        /// Save the properties from the Export Through Pathway dialog to the appropriate StyleSettings xml file
        /// </summary>
        /// <param name="dlg"></param>
        private static bool SaveProperty(ExportThroughPathway dlg)
        {
            bool success = true;
            Param.SetValue(Param.PrintVia, dlg.Format);
            Param.SetValue(Param.LayoutSelected, dlg.Style);
            // Publication Information tab
            Param.UpdateMetadataValue(Param.Title, dlg.Title);
            Param.UpdateMetadataValue(Param.Description, dlg.Description);
            Param.UpdateMetadataValue(Param.Creator, dlg.Creator);
            Param.UpdateMetadataValue(Param.Publisher, dlg.Publisher);
            Param.UpdateMetadataValue(Param.CopyrightHolder, dlg.CopyrightHolder);
            // also persist the other DC elements
            Param.UpdateMetadataValue(Param.Type, dlg.Type);
            Param.UpdateMetadataValue(Param.Source, dlg.Source);
            Param.UpdateMetadataValue(Param.Format, GetFormatMimeType(dlg.Format));
            Param.UpdateMetadataValue(Param.Contributor, dlg.Contributor);
            Param.UpdateMetadataValue(Param.Relation, dlg.Relation);
            Param.UpdateMetadataValue(Param.Coverage, dlg.Coverage);
            Param.UpdateMetadataValue(Param.Subject, dlg.Subject);
            dlg.Date = DateTime.Today.ToString("yyyy-MM-dd"); // Date gets today's date
            Param.UpdateMetadataValue(Param.Date, dlg.Date);
            Param.UpdateMetadataValue(Param.Language, dlg.Language);
            Param.UpdateMetadataValue(Param.Identifier, dlg.Identifier);

            // Front Matter tab
            if (dlg.chkCoverImage.Enabled == false)
            {
                // user can't create a cover image - make sure this isn't checked
                dlg.CoverPage = false;
            }
            Param.UpdateMetadataValue(Param.CoverPage, dlg.CoverPage.ToString());
            Param.UpdateMetadataValue(Param.CoverPageFilename, dlg.CoverPageImagePath);
            Param.UpdateMetadataValue(Param.CoverPageTitle, dlg.CoverPageTitle.ToString());
            Param.UpdateMetadataValue(Param.TitlePage, dlg.TitlePage.ToString());
            if ((dlg.CoverPageTitle || dlg.TitlePage) && (dlg.Title.Trim().Length < 1))
            {
                // User wants a title page or a cover page with a title, but they haven't told us the title.
                // Make them enter one now.
                MessageBox.Show("Please enter a title for this publication.", dlg.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                dlg.IsExpanded = true;
                dlg.ResizeDialog();
                dlg.tabControl1.SelectedTab = dlg.tabPage1;
                dlg.txtBookTitle.Focus();
                return false;
            }
            if (dlg.chkColophon.Enabled == false)
            {
                // user can't create a copyright page - make sure this isn't checked
                dlg.CopyrightPage = false;
            }
            Param.UpdateMetadataValue(Param.CopyrightPage, dlg.CopyrightPage.ToString());
            Param.UpdateMetadataValue(Param.CopyrightPageFilename, dlg.CopyrightPagePath);
            Param.UpdateMetadataValue(Param.TableOfContents, dlg.TableOfContents.ToString());

            // Processing Options tab
            if (string.IsNullOrEmpty(dlg.InputType) || dlg.InputType == "Dictionary")
            {
                success = (dlg.ExportReversal || dlg.ExportMain || dlg.ExportGrammar);
                if (!success)
                {
                    // Dictionary with nothing to export. Make them export something.
                    MessageBox.Show("Please select at least one item to include in the export.", dlg.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dlg.IsExpanded = true;
                    dlg.ResizeDialog();
                    dlg.tabControl1.SelectedTab = dlg.tabPage3;
                    dlg.chkConfiguredDictionary.Focus();
                }
                Param.SetValue(Param.ConfigureDictionary, dlg.ExportMain.ToString());
                Param.SetValue(Param.ReversalIndex, dlg.ExportReversal.ToString());
                Param.SetValue(Param.GrammarSketch, dlg.ExportGrammar.ToString());
            }

            Param.SetValue(Param.ExtraProcessing, dlg.RunningHeader.ToString());
            Param.SetValue(Param.Media, _media);
            Param.SetValue(Param.PublicationLocation, dlg.OutputFolder);
            Param.Write();





            return success;
        }

        /// <summary>
        /// Preprocess the xhtml file to replace image names, and link to the merged css file.
        /// </summary>
        /// <param name="projInfo">information about input data</param>
        /// <returns>path name to processed xthml file</returns>
        private string GetProcessedXhtml(PublicationInformation projInfo)
        {



            var result = projInfo.DefaultXhtmlFileWithPath;

            result = Common.PathCombine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath),
                                              ".xhtml");//collectionName + ".xhtml"
            File.Copy(projInfo.DefaultXhtmlFileWithPath, result, true);

            return result;
        }

        private static void SaveDefaultProperty(ExportThroughPathway dlg)
        {
            Param.SetDefaultValue(Param.PrintVia, dlg.Format);
            Param.SetDefaultValue(Param.LayoutSelected, dlg.Style);
            // TODO: reimplement the persistence here
            // Publication Information tab
            // Front Matter tab

            // Processing Options tab
            if (string.IsNullOrEmpty(dlg.InputType) || dlg.InputType == "Dictionary")
            {
                Param.SetDefaultValue(Param.ConfigureDictionary, dlg.ExportMain.ToString());
                Param.SetDefaultValue(Param.ReversalIndex, dlg.ExportReversal.ToString());
                Param.SetDefaultValue(Param.GrammarSketch, dlg.ExportGrammar.ToString());
            }
            Param.SetDefaultValue(Param.ExtraProcessing, dlg.RunningHeader.ToString());
            Param.SetDefaultValue(Param.Media, _media);
            if (Common.CustomSaveInFolder(dlg.OutputFolder))
                Param.SetValue(Param.PublicationLocation, dlg.OutputFolder);
            Param.Write();
        }
        #endregion LoadProperty & SaveProperty

        /// <summary>
        /// Returns the IANA media type used in the dc:format element for the given export type.
        /// If you create a new export type, look up the format's mime type and add it to the case statement below.
        /// <see cref="http://www.iana.org/assignments/media-types/index.html"/>
        /// </summary>
        /// <param name="exportType">Export type string to look up</param>
        /// <returns>corresponding IANA mime type, to be used in the dc:format element field.</returns>
        public static string GetFormatMimeType(string exportType)
        {
            var value = string.Empty;
            switch (exportType)
            {
                case "E-Book (.epub)":
                    value = "application/epub+zip";
                    break;
                case "Go Bible":
                    // generic archive
                    value = "application/x-java-archive";
                    break;
                case "InDesign":
                    value = "application/x-indesign";
                    break;
                case "Logos Alpha":
                    // generic archive
                    value = "application/zip";
                    break;
                case "OpenOffice/LibreOffice":
                    value = "application/vnd.oasis.opendocument.text";
                    break;
                case "Pdf (using Prince)":
                    value = "application/pdf";
                    break;
                case "Webonary":
                    // not sure about this -- using html
                    value = "text/html";
                    break;
                case "XeLaTex":
                case "XeTex":
                case "XeTeX Alpha":
                    value = "application/x-latex";
                    break;
                case "YouVersion":
                    break;
            }
            return value;
        }

        #region SetOkStatus
        private void SetOkStatus()
        {
            if (Param.Value.Count == 0)
                return;
            // TODO: any other items to prevent the OK button from being enabled
            btnOK.Enabled = chkIP.Checked;
        }
        #endregion SetOkStatus

        private string CleanText(string text)
        {
            string newText = text.Replace("<", "");
            newText = newText.Replace(">", "");
            return newText.Trim();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                _helpTopic = (Text.Contains("Set Defaults"))
                                 ? "User_Interface/Dialog_boxes/Set_Defaults_dialog_box.htm"
                                 : "User_Interface/Dialog_boxes/Export_Through_Pathway_dialog_box.htm";
            }
            Common.PathwayHelpSetup();
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
            previewPrintVia.SelectedStyle = ddlStyle.Text;
            previewPrintVia.InputType = InputType;
            string selectedStyle = ddlStyle.Text;
            previewPrintVia.ShowDialog();
            LoadAvailStylesheets();
            if (previewPrintVia.DialogResult == DialogResult.OK)
            {
                ddlStyle.Text = previewPrintVia.SelectedStyle;
            }
            else
            {
                ddlStyle.Text = selectedStyle;
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
                if (File.Exists(Path.Combine(pathwayPath, "ConfigurationTool.exe")))
                {
                    return true;
                }
            }
            return false;
        }

        private void ddlLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            _media = FindMedia();
            LoadAvailStylesheets();
            EnableUIElements();
            SetOkStatus();
        }



        private string FindMedia()
        {
            string backend = ddlLayout.Text.ToLower();
            string media;

            if (backend == "go bible")
            {
                media = "mobile";
            }
            else if (backend == "e-book (.epub)")
            {
                media = "others";
            }
            else if (backend == "webonary")
            {
                media = "web";
            }
            else
            {
                media = "paper";
            }
            return media;
        }

        private void btnMoreLessOptions_Click(object sender, EventArgs e)
        {
            // expand / collapse the dialog (Toggle)
            IsExpanded = !IsExpanded;
            ResizeDialog();
        }

        private void ddlStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            _publicationName = ddlStyle.Text;
            txtSaveInFolder.Text = Common.GetSaveInFolder(Param.DefaultValue[Param.PublicationLocation], DatabaseName, ddlStyle.Text);
            if (_newSaveInFolderPath.Length > 0)
            {
                txtSaveInFolder.Text = Path.GetDirectoryName(_newSaveInFolderPath) + ddlStyle.Text + "_" + DateTime.Now.ToString("yyyy-MM-dd_hhmm");
            }
        }

        private void chkCoverImage_CheckedChanged(object sender, EventArgs e)
        {
            // enable / disable Cover Image UI
            chkCoverImageTitle.Enabled = chkCoverImage.Checked;
            imgCoverImage.Enabled = chkCoverImage.Checked;
            btnCoverImage.Enabled = chkCoverImage.Checked;
        }

        private void chkColophon_CheckedChanged(object sender, EventArgs e)
        {
            // enable / disable Colophon UI
            //            lblColophonFile.Enabled = chkColophon.Checked;
            rdoStandardCopyright.Enabled = chkColophon.Checked;
            rdoCustomCopyright.Enabled = chkColophon.Checked;
            if (txtColophonFile.Text.Trim().Length < 1 && ddlCopyrightStatement.Items.Count > 0)
            {
                // no custom colophon file set (most likely this is an uninitialized install) -
                // select the first standard rights agreement in the list and the standard rights
                // statement radio button
                rdoStandardCopyright.Checked = true;
                ddlCopyrightStatement.SelectedIndex = 0;
            }
            ddlCopyrightStatement.Enabled = (chkColophon.Checked) ? rdoStandardCopyright.Checked : false;
            txtColophonFile.Enabled = (chkColophon.Checked) ? rdoCustomCopyright.Checked : false;
            btnBrowseColophon.Enabled = (chkColophon.Checked) ? rdoCustomCopyright.Checked : false;
        }

        private void chkConfiguredDictionary_CheckedChanged(object sender, EventArgs e)
        {
            SetOkStatus();
        }

        private void chkReversalIndexes_CheckedChanged(object sender, EventArgs e)
        {
            SetOkStatus();
        }

        private void chkGrammarSketch_CheckedChanged(object sender, EventArgs e)
        {
            SetOkStatus();
        }

        private void chkIP_CheckedChanged(object sender, EventArgs e)
        {
            SetOkStatus();
        }

        private void lnkIP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _helpTopic = "Concepts/Intellectual_Property.htm";
            btnHelp_Click(sender, e);
        }

        private void btnCoverImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files (*.png, *.jpg)|*.png;*.jpg";
            openFile.ShowDialog();

            string filename = openFile.FileName;
            if (filename != "")
            {
                try
                {
                    Image iconImage = Image.FromFile(filename);
                    double height = iconImage.Height;
                    double width = iconImage.Width;
                    if (height > 1000 || width > 1000)
                    {
                        MessageBox.Show("The selected image is too large. Please select an image that is smaller than 1000 x 1000 pixels.",
                            this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    CoverPageImagePath = filename;
                    //string userPath = (Param.Value["UserSheetPath"]);
                    //string imgFileName = Path.GetFileName(filename);
                    //string toPath = Path.Combine(userPath, imgFileName);
                    //File.Copy(filename, toPath, true);
                    imgCoverImage.Image = iconImage;
                }
                catch { }
            }

        }

        private void btnBrowseSaveInFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dlg.SelectedPath = Common.PathCombine(documentsPath, Common.SaveInFolderBase);
            DirectoryInfo directoryInfo = new DirectoryInfo(dlg.SelectedPath);
            if (!directoryInfo.Exists)
                directoryInfo.Create();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string folderName = ddlStyle.Text + "_" + DateTime.Now.ToString("yyyy-MM-dd_hhmm");
                _newSaveInFolderPath = Common.PathCombine(dlg.SelectedPath, folderName);
                Param.SetValue(Param.PublicationLocation, _newSaveInFolderPath);
                txtSaveInFolder.Text = _newSaveInFolderPath;
            }
        }

        private void btnBrowseColophon_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "XHTML Files (*.xhtml)|*.xhtml";
            openFile.ShowDialog();

            string filename = openFile.FileName;
            if (filename != "")
            {
                txtColophonFile.Text = filename;
            }
        }

        private void rdoStandardCopyright_CheckedChanged(object sender, EventArgs e)
        {
            // enable / disable Colophon UI
            ddlCopyrightStatement.Enabled = rdoStandardCopyright.Checked;
            txtColophonFile.Enabled = rdoCustomCopyright.Checked;
            btnBrowseColophon.Enabled = rdoCustomCopyright.Checked;
        }

        private void rdoCustomCopyright_CheckedChanged(object sender, EventArgs e)
        {
            // enable / disable Colophon UI
            ddlCopyrightStatement.Enabled = rdoStandardCopyright.Checked;
            txtColophonFile.Enabled = rdoCustomCopyright.Checked;
            btnBrowseColophon.Enabled = rdoCustomCopyright.Checked;
        }

        private void lnkChooseCopyright_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // TODO: replace with correct help topic
            _helpTopic = "Tasks/Basic_Tasks/Choosing_a_rights_statement_overview.htm";
            btnHelp_Click(sender, e);
        }

        private void ddlCopyrightStatement_SelectedIndexChanged(object sender, EventArgs e)
        {
            // find the copyright file
            XmlNode node;
            // try to get the key
            node = Param.GetItem("//features/feature[@name='Lic_" + XmlConvert.EncodeName(Organization) + "']");
            // key not found - fall back on the generic licenses
            if (node == null)
            {
                node = Param.GetItem("//features/feature[@name='Lic_Other']");
            }
            if (node != null)
            {
                foreach (XmlNode subnode in node.ChildNodes)
                {
                    if (subnode.Attributes != null)
                    {
                        var value = subnode.Attributes["name"].Value;
                        if (ddlCopyrightStatement.SelectedItem.Equals(value))
                        {
                            // this is our item - set the CopyrightFilename
                            var copyrightDir = Path.Combine(Common.GetPSApplicationPath(), "Copyrights");
                            CopyrightPagePath = Path.Combine(copyrightDir, subnode.Attributes["file"].Value);
                        }
                    }
                }
            }
        }



    }
}
