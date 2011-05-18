using System;
using System.Collections;
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
        private static string _publicationName;
        private string _newSaveInFolderPath = string.Empty;
        private static string _helpTopic = string.Empty;
        public bool _fromPlugIn;
        private static string _media = "paper";

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
            get { return txtBookTitle.Text.Trim(); }
            set { txtBookTitle.Text = value; }
        }
        /// <summary>
        /// dc:description element
        /// </summary>
        public string Description // dc:description
        {
            get { return txtDescription.Text.Trim(); }
            set { txtDescription.Text = value; }
        }
        /// <summary>
        /// dc:creator element
        /// </summary>
        public string Creator // dc:creator
        {
            get { return txtCreator.Text.Trim(); }
            set { txtCreator.Text = value; }
        }
        /// <summary>
        /// dc:publisher element
        /// </summary>
        public string Publisher // dc:publisher
        {
            get { return txtPublisher.Text.Trim(); }
            set { txtPublisher.Text = value; }
        }
        /// <summary>
        /// dc:rights element
        /// </summary>
        public string CopyrightHolder // dc:rights
        {
            get { return txtRights.Text.Trim(); }
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
        public string OutputFolder { 
            get { return txtSaveInFolder.Text; }
            set { txtSaveInFolder.Text = value; }
        }
        #endregion Properties

        private void ExportThroughPathway_Load(object sender, EventArgs e)
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
            try
            {
                Organization = Param.Value["Organization"];

            }
            catch (Exception)
            {
                var dlg = new SelectOrganizationDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Organization = dlg.Organization;
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
            Common.databaseName = DatabaseName;
        }

        /// <summary>
        /// Expands or contracts the dialog, based on the current value of the IsExpanded property.
        /// </summary>
        private void ResizeDialog()
        {
            // Resize the dialog
            Height = (IsExpanded) ? 480 : 225;

            // Show / hide the tab control
            tabControl1.Visible = IsExpanded;

            // Move the bottom UI elements where they need to go
            chkIP.Top = tabControl1.Top + ((IsExpanded) ? (tabControl1.Height + 10) : 0);
            lnkIP.Top = chkIP.Top + chkIP.Height + 3;
            btnOK.Top = lnkIP.Top + lnkIP.Height + 15;
            btnCancel.Top = btnOK.Top;
            btnHelp.Top = btnOK.Top;

            // Set the text on the More / Less Options button
            btnMoreLessOptions.Text = (IsExpanded) ? "Less  ▲" : "More  ▼";
        }

        /// <summary>
        /// Show or hide UI elements on the page depending on the output type, format and style
        /// </summary>
        private void EnableUIElements()
        {
            // Front Matter tab
            chkCoverImage.Enabled = (ddlLayout.Text.Contains("epub"));
            chkCoverImageTitle.Enabled = (chkCoverImage.Enabled && chkCoverImage.Checked);
            btnCoverImage.Enabled = chkCoverImageTitle.Enabled;
            imgCoverImage.Enabled = chkCoverImageTitle.Enabled;

            rdoCustomCopyright.Enabled = chkColophon.Checked;
            rdoStandardCopyright.Enabled = chkColophon.Checked;
            ddlCopyrightStatement.Enabled = (chkColophon.Checked) ? rdoStandardCopyright.Checked : false;
            txtColophonFile.Enabled = (chkColophon.Checked) ? rdoCustomCopyright.Checked : false;
            btnBrowseColophon.Enabled = (chkColophon.Checked) ? rdoCustomCopyright.Checked : false;

            // Processing Options tab
            chkRunningHeader.Enabled = (FindMedia() == "paper");
            chkOOReduceStyleNames.Enabled = (ddlLayout.Text.Contains("OpenOffice"));
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
        private void LoadAvailFormats()
        {
            string BackendsPath = Common.ProgInstall;
            Backend.Load(BackendsPath);
            ArrayList exportType = Backend.GetExportType(InputType);
            if (exportType.Count > 0)
            {
                foreach (string item in exportType)
                {
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
                btnOK.Enabled = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a folder for which you have creation permission", "Pathway", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            // attempt to save the properties - if it doesn't work, leave the dialog open
            if (SaveProperty(this))
            {
                DialogResult = DialogResult.Yes;
                if (Text.Contains("Default"))
                    SaveDefaultProperty(this);
                _publicationName = Path.GetFileName(OutputFolder);
                OutputFolder = Path.GetDirectoryName(OutputFolder);
                Common.TimeStarted = DateTime.Now;
                Close();
            }
        }

        #endregion Events

        #region LoadProperty & SaveProperty
        private void LoadProperty()
        {
            Format = Param.DefaultValue[Param.PrintVia];

            // publication info tab
            Title = Param.GetMetadataValue(Param.Title, Organization);
            Description = Param.GetMetadataValue(Param.Description, Organization);
            Creator = Param.GetMetadataValue(Param.Creator, Organization);
            Publisher = Param.GetMetadataValue(Param.Publisher, Organization);
            CopyrightHolder = Param.GetMetadataValue(Param.CopyrightHolder, Organization);
            Type = Param.GetMetadataValue(Param.Type, Organization);
            Source = Param.GetMetadataValue(Param.Source, Organization);
            Format = Param.GetMetadataValue(Param.Format, Organization);
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
            TitlePage = (Param.GetMetadataValue(Param.TitlePage, Organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.TitlePage, Organization));
            CoverPageTitle = (Param.GetMetadataValue(Param.CoverPageTitle, Organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CoverPageTitle, Organization));
            CopyrightPage = (Param.GetMetadataValue(Param.CopyrightPage, Organization) == null) ? false : Boolean.Parse(Param.GetMetadataValue(Param.CopyrightPage, Organization));
            CopyrightPagePath = Param.GetMetadataValue(Param.CopyrightPageFilename, Organization);
            // license agreement
            XmlNode node;
            // try to get the key
            node = Param.GetItem("//features/feature[@name='Lic_" + Organization + "']");
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
        }

        /// <summary>
        /// Save the properties from the Export Through Pathway dialog to the appropriate StyleSettings xml file
        /// </summary>
        /// <param name="dlg"></param>
        private static bool SaveProperty(ExportThroughPathway dlg)
        {
            bool success = true;
            Param.SetValue(Param.PrintVia, dlg.Format);
            // Publication Information tab
            Param.UpdateMetadataValue(Param.Title, dlg.Title);
            Param.UpdateMetadataValue(Param.Description, dlg.Description);
            Param.UpdateMetadataValue(Param.Creator, dlg.Creator);
            Param.UpdateMetadataValue(Param.Publisher, dlg.Publisher);
            Param.UpdateMetadataValue(Param.CopyrightHolder, dlg.CopyrightHolder);
            // also persist the other DC elements
            Param.UpdateMetadataValue(Param.Type, dlg.Type);
            Param.UpdateMetadataValue(Param.Source, dlg.Source);
            Param.UpdateMetadataValue(Param.Format, dlg.Format);
            Param.UpdateMetadataValue(Param.Contributor, dlg.Contributor);
            Param.UpdateMetadataValue(Param.Relation, dlg.Relation);
            Param.UpdateMetadataValue(Param.Coverage, dlg.Coverage);
            Param.UpdateMetadataValue(Param.Subject, dlg.Subject);
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
            Param.UpdateMetadataValue(Param.CopyrightPage, dlg.CopyrightPage.ToString());
            Param.UpdateMetadataValue(Param.CopyrightPageFilename, dlg.CopyrightPagePath);

            // Processing Options tab
            if (string.IsNullOrEmpty(dlg.InputType) || dlg.InputType == "Dictionary")
            {
                success = (dlg.ExportReversal || dlg.ExportMain || dlg.ExportGrammar);
                if (!success)
                {
                    MessageBox.Show("Please select at least one item to include in the export.");
                    dlg.IsExpanded = true;
                    dlg.ResizeDialog();
                    dlg.tabControl1.SelectedTab = dlg.tabPage3;
                    dlg.chkConfiguredDictionary.Focus();
                }
                Param.SetValue(Param.ConfigureDictionary, dlg.ExportMain.ToString());
                Param.SetValue(Param.ReversalIndex, dlg.ExportReversal.ToString());
                Param.SetValue(Param.GrammarSketch, dlg.ExportGrammar.ToString());
            }
            Param.SetValue(Param.LayoutSelected, dlg.DictionaryName);
            Param.SetValue(Param.ExtraProcessing, dlg.RunningHeader.ToString());
            Param.SetValue(Param.Media, _media);
            Param.SetValue(Param.PublicationLocation, dlg.OutputFolder);
            Param.Write();
            return success;
        }

        private static void SaveDefaultProperty(ExportThroughPathway dlg)
        {
            Param.SetDefaultValue(Param.PrintVia, dlg.Format);
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
            Param.SetDefaultValue(Param.LayoutSelected, dlg.DictionaryName);
            Param.SetDefaultValue(Param.ExtraProcessing, dlg.RunningHeader.ToString());
            Param.SetDefaultValue(Param.Media, _media);
            if (Common.CustomSaveInFolder(dlg.OutputFolder))
                Param.SetValue(Param.PublicationLocation, dlg.OutputFolder);
            Param.Write();
        }
        #endregion LoadProperty & SaveProperty

        #region SetOkStatus
        private void SetOkStatus()
        {
            if (Param.Value.Count == 0)
                return;
            // TODO: any other items to prevent the OK button from being enabled
            btnOK.Enabled = chkIP.Checked;
        }
        #endregion SetOkStatus

        private void btnHelp_Click(object sender, EventArgs e)
        {
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
            previewPrintVia.ShowDialog();
            LoadAvailStylesheets();
            if (previewPrintVia.DialogResult == DialogResult.OK)
            {
                ddlStyle.Text = previewPrintVia.SelectedStyle;
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
                        MessageBox.Show("The selected image is too large. Please select an image with less that 1000 pixels in height and length.",
                            this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    string userPath = (Param.Value["UserSheetPath"]);
                    string imgFileName = Path.GetFileName(filename);
                    string toPath = Path.Combine(userPath, imgFileName);
                    File.Copy(filename, toPath, true);
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
            _helpTopic = "Tasks/Basic_Tasks/Choosing_a_rights_statement.htm";
            btnHelp_Click(sender, e);
        }

        private void ddlCopyrightStatement_SelectedIndexChanged(object sender, EventArgs e)
        {
            // find the copyright file
            XmlNode node;
            // try to get the key
            node = Param.GetItem("//features/feature[@name='Lic_" + Organization + "']");
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
