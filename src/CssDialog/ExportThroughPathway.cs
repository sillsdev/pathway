// --------------------------------------------------------------------------------------
// <copyright file="ExportThroughPathway.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, 2011 SIL International. All Rights Reserved.
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
// for TD-2344: https://jira.sil.org/secure/attachment/27139/PubInfo_Proposal_v5.docx
// </remarks>
// --------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using L10NSharp;
using Palaso.UI.WindowsForms.WritingSystems;
using Palaso.WritingSystems;
using SilTools;
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
        public static bool isFromConfigurationTool = false;
        private bool _isUnixOS = false;
        private string _sDateTime = string.Empty;
        public ExportThroughPathway()
        {
			Common.SetupLocalization();
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
        //public bool RunningHeader
        //{
        //    get { return chkRunningHeader.Checked; }
        //    set { chkRunningHeader.Checked = value; }
        //}
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
	            AssignFontandSizeAllTextbox();
                AssignFolderDateTime();
                if (!Common.isRightFieldworksVersion())
                {
                    var message = LocalizationManager.GetString("ExportThroughPathway.ExportThroughPathwayLoad.Message", "Please download and install a Pathway version compatible with your software", "");
					string caption = LocalizationManager.GetString("ExportThroughPathway.ExportThroughPathwayLoad.ProjectName", "Incompatible Pathway Version", "");
					Utils.MsgBox(message, caption, MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
                _isUnixOS = Common.UnixVersionCheck();
                PopulateFilesFromPreprocessingFolder();

                // Load the .ssf or .ldml as appropriate
                _settingsHelper = new SettingsHelper(DatabaseName);
                _settingsHelper.LoadValues();

                LoadDefaultSettings();

                // get the current organization
                Organization = Param.GetOrganization();

	            if (Organization == "")
	            {
		            // no organization set yet -- display the Select Organization dialog
		            var dlg = new SelectOrganizationDialog(InputType);
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
	            else
	            {
					if (DatabaseName == "{Project_Name}")
					{
						this.Text += " - " + InputType;
					}
	            }
                LoadAvailFormats();
                LoadAvailStylesheets();
                IsExpanded = false;
                ResizeDialog();
                SetOkStatus();
                chkHyphen.Enabled = false;
                chkHyphen.Checked = false;
                clbHyphenlang.Items.Clear();
                //Loads Hyphenation related settings
		        LoadHyphenationSettings();
	            LoadProperty();
                EnableUIElements();

                ShowHelp.ShowHelpTopic(this, _helpTopic, _isUnixOS, false);
                if (AppDomain.CurrentDomain.FriendlyName.ToLower().IndexOf("configurationtool") == -1)
                {
                    Common.databaseName = DatabaseName;
                }
            }
            catch { }
        }

        private void LoadDefaultSettings()
        {
            // not setting defaults, just opening the dialog:
            // load the settings file and migrate it if necessary
            Param.LoadSettings();
            Param.SetValue(Param.InputType, InputType);
            Param.LoadSettings();
            isFromConfigurationTool = true;
        }

        private void LoadHyphenationSettings()
        {
            var ssf = _settingsHelper.GetSettingsFilename(_settingsHelper.Database);
            if (ssf != null && ssf.Trim().Length > 0)
            {
	            if (AppDomain.CurrentDomain.FriendlyName.ToLower() == "paratext.exe")
	            {
		            var paratextpath = Path.Combine(Path.GetDirectoryName(ssf), _settingsHelper.Database);
		            Param.DatabaseName = _settingsHelper.Database;
		            Param.IsHyphen = false;
		            Param.HyphenLang = Common.ParaTextDcLanguage(_settingsHelper.Database, true);
		            foreach (string filename in Directory.GetFiles(paratextpath, "*.txt"))
		            {
			            if (filename.Contains("hyphen"))
			            {
				            Param.IsHyphen = true;
				            Param.HyphenFilepath = filename;
				            Param.HyphenationSelectedLanguagelist.AddRange(Param.HyphenLang.Split(','));
			            }
		            }
	            }
	            if (AppDomain.CurrentDomain.FriendlyName.ToLower().Contains("fieldworks"))
	            {
		            var xdoc = new XmlDocument();
		            try
		            {
						IDictionary<string,string> value=new Dictionary<string, string>();
						var settingsFile = ssf;
			            xdoc.Load(settingsFile);
			            var curVernWssnode = xdoc.SelectSingleNode("//CurVernWss/Uni");
			            if (curVernWssnode != null)
			            {
				            foreach (var lang in curVernWssnode.InnerText.Split(' '))
				            {
								var langname = GetLanguageValues(lang);
					            if (!string.IsNullOrEmpty(lang) && (langname != null) && !value.ContainsKey(lang))
						            value.Add(lang, langname.Replace(',',' '));
				            }
			            }
			            var curAnalysisWssnode = xdoc.SelectSingleNode("//CurAnalysisWss/Uni");
			            if (curAnalysisWssnode != null)
			            {
							foreach (var lang in curAnalysisWssnode.InnerText.Split(' '))
				            {
								var langname = GetLanguageValues(lang);
								if (!string.IsNullOrEmpty(lang) && (langname != null) && !value.ContainsKey(lang))
									value.Add(lang, langname.Replace(',', ' '));
				            }
			            }
						Param.HyphenLang = string.Join(",", value.Select(x => x.Key + ":" + x.Value).ToArray());
		            }
		            catch (Exception)
		            {
		            }
	            }

            }
			Common.EnableHyphenation();
            CreatingHyphenationSettings.ReadHyphenationSettings(_settingsHelper.Database, InputType);
            foreach (string lang in Param.HyphenLang.Split(','))
            {
                    clbHyphenlang.Items.Add(lang, (Param.HyphenationSelectedLanguagelist.Contains(lang) ? true : false));
            }
        }

	    public static string GetLanguageValues(string lang)
	    {
		    if (lang.Contains("-"))
		    {
			    lang = lang.Substring(0, lang.IndexOf("-", System.StringComparison.Ordinal));
		    }
		    var allLangs = from ci in CultureInfo.GetCultures(CultureTypes.NeutralCultures)
			    where ci.TwoLetterISOLanguageName != "iv"
			    orderby ci.DisplayName
			    select ci;
		    var langname = lang.Length == 2
			    ? allLangs.FirstOrDefault(s => s.TwoLetterISOLanguageName == lang)
			    : allLangs.FirstOrDefault(s => s.ThreeLetterISOLanguageName == lang);
		    return langname != null ? langname.EnglishName : Common.GetPalasoLanguageName(lang);
	    }

	    private void AssignFolderDateTime()
        {
            _sDateTime = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
        }


        private void PopulateFilesFromPreprocessingFolder()
        {
			var lstxsltFiles = Common.PreProcessingXsltFilesList();
            var settingsFolder = Path.Combine(Common.GetAllUserPath(), InputType);
            if (Directory.Exists(settingsFolder))
            {
                foreach (string filePath in Directory.GetFiles(settingsFolder, "*.xsl"))
                {
                    var processName = Path.GetFileNameWithoutExtension(filePath);
					if (!lstxsltFiles.Contains(processName))
	                {
		                Debug.Assert(processName != null);
		                chkLbPreprocess.Items.Add(processName, false);
	                }
                }
            }

            string xsltFullName = Common.PathCombine(Common.GetApplicationPath(), "Preprocessing\\");// + xsltFile[0]
            xsltFullName = Common.PathCombine(xsltFullName, InputType + "\\"); //TD-2871 - separate dictionary and Scripture xslt
            if (Directory.Exists(xsltFullName))
            {
                string[] filePaths = Directory.GetFiles(xsltFullName, "*.xsl");
                // In case the xsl file name change and updated in the psexport.cs file XsltPreProcess
                foreach (var filePath in filePaths)
                {
                    var processName = Path.GetFileNameWithoutExtension(filePath);
                    Debug.Assert(processName != null);
					if (!lstxsltFiles.Contains(processName))
					{
						if (!chkLbPreprocess.Items.Contains(processName))
						{
							chkLbPreprocess.Items.Add(processName, false);
						}
					}
                }
            }
        }

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
                //CopyrightHolder = valueFromSettings;
                CopyrightHolder = Param.GetMetadataValue(Param.CopyrightHolder, Organization);
                CopyrightHolder = Common.UpdateCopyrightYear(CopyrightHolder);
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
            btnMoreLessOptions.Text = (IsExpanded) ? LocalizationManager.GetString("ExportThroughPathway.btnMoreLessOptions.Less", "Less", "") : LocalizationManager.GetString("ExportThroughPathway.btnMoreLessOptions.More", "More", "");
            btnMoreLessOptions.Image = (IsExpanded) ? Properties.Resources.go_up : Properties.Resources.go_down;
        }

        /// <summary>
        /// Show or hide UI elements on the page depending on the output type, format and style
        /// </summary>
        private void EnableUIElements()
        {
            // Front Matter tab
            // edb - temporary / remove as exports are implemented
            if (!ddlLayout.Text.Contains("Epub") && !ddlLayout.Text.Contains("OpenOffice/LibreOffice") && !ddlLayout.Text.Contains("InDesign") && !ddlLayout.Text.Contains("XeLaTex"))
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
                chkCoverImage.Enabled = (ddlLayout.Text.Contains("Epub") || ddlLayout.Text.Contains("OpenOffice/LibreOffice") || ddlLayout.Text.Contains("InDesign") || ddlLayout.Text.Contains("XeLaTex"));
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

                chkTOC.Enabled = true;
                if (ddlLayout.Text.Contains("Epub"))
                {
                    chkTOC.Checked = true;
                    chkTOC.Enabled = false;
                }	            
            }

            // Processing Options tab
            chkOOReduceStyleNames.Enabled = (ddlLayout.Text.Contains("LibreOffice"));
            if (InputType != "Dictionary")
            {
                grpInclude.Visible = false;
                chkConfiguredDictionary.Visible = false;
                chkReversalIndexes.Visible = false;
                chkGrammarSketch.Visible = false; // currently this is false anyways (it's not implemented)
            }
			if (ddlLayout.Text.Contains("LibreOffice"))
			{
				chkHyphen.Enabled = Param.IsHyphen;
				chkHyphen.Checked = Param.HyphenEnable;
				for (int i = 0; i < clbHyphenlang.Items.Count; i++)
					clbHyphenlang.SetItemCheckState(i, ((Param.HyphenEnable && Param.HyphenationSelectedLanguagelist.Contains(clbHyphenlang.GetItemText(clbHyphenlang.Items[i]))) ? CheckState.Checked : CheckState.Unchecked));
				clbHyphenlang.Enabled = false;
			}
			else
			{
				chkHyphen.Enabled = false;
				clbHyphenlang.Enabled = false;
				chkHyphen.Checked = false;
				while (clbHyphenlang.CheckedIndices.Count > 0)
					clbHyphenlang.SetItemChecked(clbHyphenlang.CheckedIndices[0], false);
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
            if (ddlLayout.Text.Contains("theWord"))
            {
                ddlStyle.Enabled = false;
                BtnBrwsLayout.Enabled = false;
            }
            else
            {
                BtnBrwsLayout.Enabled = true;
            }
        }

        private void ShownValidation()
        {
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
            OperatingSystem OS = Environment.OSVersion;
            string BackendsPath = Common.ProgInstall;
            Backend.Load(BackendsPath);
			Console.WriteLine( @"InputType from : {0}", InputType);
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
                    if (item.ToLower() == "indesign" && OS.VersionString.ToLower().IndexOf("windows") < 0)
                    {
                        continue;
                    }
                    ddlLayout.Items.Add(item);
                }
                ddlLayout.Text = ddlLayout.Items[0].ToString();
            }
            else
            {
                DialogResult dialogResult;
                if (!Common.Testing)
                {
                    var message = LocalizationManager.GetString("ExportThroughPathway.LoadAvailFormat.Message", "Pathway was unable to find any export formats. Please reinstall Pathway to correct this error.", "");
					string caption = LocalizationManager.GetString("ExportThroughPathway.MessageBoxCaption.ProjectName", "Pathway", "");
                    dialogResult = Utils.MsgBox(message, caption, MessageBoxButtons.AbortRetryIgnore,
                                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    dialogResult = DialogResult.Abort;
                }
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
                    case "comment":
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
                if (!Common.IsUnixOS())
                {
                    int outputFolderLength = OutputFolder.Length;
                    if (OutputFolder.Substring(outputFolderLength - 2, 2) != "\\")
                        OutputFolder = OutputFolder + "\\";
                }

                if (!Directory.Exists(OutputFolder))
                    Directory.CreateDirectory(OutputFolder);
            }
            catch (Exception)
            {
                var message = LocalizationManager.GetString("ExportThroughPathway.OkButtonClick.Message", "Please select a folder for which you have creation permission", "");
				string caption = LocalizationManager.GetString("ExportThroughPathway.MessageBoxCaption.projectname", "Pathway", "");
				Utils.MsgBox(message, caption, MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            ProcessSendingHelpImprove();

            HyphenationSettings();
            if (!File.Exists(CoverPageImagePath))
            {
                CoverPageImagePath = Common.PathCombine(Common.GetApplicationPath(), "Graphic\\cover.png");
            }

            // attempt to save the properties - if it doesn't work, leave the dialog open
            if (!SaveProperty(this))
            {
                return;
            }

            DialogResult = DialogResult.Yes;
            if (Text.Contains("Default"))
                SaveDefaultProperty(this);

            if (!Common.IsUnixOS())
            {
                OutputFolder = Path.GetDirectoryName(OutputFolder);
            }

            DictionaryName = OutputFolder;
            Common.TimeStarted = DateTime.Now;
            _settingsHelper.ClearValues();

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
            this.Close();
        }

        private void HyphenationSettings()
        {
            Param.HyphenEnable = chkHyphen.Checked;
            var hyphenlang = string.Empty;
            foreach (string chkBoxName in clbHyphenlang.CheckedItems)
            {
                if (hyphenlang.Length > 0)
                    hyphenlang += ",";
                hyphenlang += chkBoxName;
            }
            Param.HyphenLang = hyphenlang;
            CreatingHyphenationSettings.CreateHyphenationFile(InputType);
        }

        private static void ProcessSendingHelpImprove()
        {
            string getApplicationPath = Common.GetApplicationPath();
            string helpImproveCommand = Common.PathCombine(getApplicationPath, "HelpImprove.exe");
            const string registryPath = "Software\\SIL\\Pathway";

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
            Title = Param.GetTitleMetadataValue(Param.Title, Organization, isFromConfigurationTool);
            Description = Param.GetMetadataValue(Param.Description, Organization);
            Creator = Param.GetMetadataValue(Param.Creator, Organization);
            Publisher = Param.GetMetadataValue(Param.Publisher, Organization);
            if (CopyrightHolder.Trim().Length < 1)
            {
                CopyrightHolder = Param.GetMetadataValue(Param.CopyrightHolder, Organization);
                CopyrightHolder = Common.UpdateCopyrightYear(CopyrightHolder);
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
                // ReSharper disable RedundantAssignment
                string copyrightFileName = string.Empty;
                // ReSharper restore RedundantAssignment
                if (_isUnixOS)
                {
                    copyrightFileName = Common.LeftRemove(CopyrightPagePath, "\\");
                    copyrightFileName = Path.GetFileName(copyrightFileName);
                }
                else
                {
                    copyrightFileName = Path.GetFileName(CopyrightPagePath);
                }
                foreach (XmlNode subnode in node.ChildNodes)
                {
                    if (subnode.Attributes != null)
                    {
                        var value = subnode.Attributes["name"].Value;
                        if (!ddlCopyrightStatement.Items.Contains(value))
                        {
                            // not currently in the list - add it now
                            int index = ddlCopyrightStatement.Items.Add(value);
                            if (copyrightFileName == subnode.Attributes["file"].Value)
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
                if (chkConfiguredDictionary.Enabled)
                    ExportMain = Param.Value[Param.ConfigureDictionary] == "True";
                if (chkReversalIndexes.Enabled)
                    ExportReversal = Param.Value[Param.ReversalIndex] == "True";
                if (chkGrammarSketch.Enabled)
                    ExportGrammar = Param.Value[Param.GrammarSketch] == "True";
            }
            DictionaryName = Param.DefaultValue[Param.LayoutSelected];
            Media = Param.DefaultValue[Param.Media];

            //Fillup XSLT Processing Checkboxes
            var preprocess = Param.Value.ContainsKey(Param.Preprocessing) ? Param.Value[Param.Preprocessing] : string.Empty;
            for (int i = 0; i < chkLbPreprocess.Items.Count; i++)
            {
                chkLbPreprocess.SetItemCheckState(i, preprocess.Contains(chkLbPreprocess.Items[i].ToString()) ? CheckState.Checked : CheckState.Unchecked);
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
            Param.UpdateTitleMetadataValue(Param.Title, dlg.Title, isFromConfigurationTool);
            Param.UpdateMetadataValue(Param.Description, dlg.Description);
            Param.UpdateMetadataValue(Param.Creator, dlg.Creator);
            Param.UpdateMetadataValue(Param.Publisher, dlg.Publisher);
            Param.UpdateMetadataValue(Param.CopyrightHolder, Common.UpdateCopyrightYear(dlg.CopyrightHolder));
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

            Param.UpdateMetadataValue(Param.CoverPage, dlg.CoverPage.ToString());
            Param.UpdateMetadataValue(Param.CoverPageFilename, dlg.CoverPageImagePath);
            Param.UpdateMetadataValue(Param.CoverPageTitle, dlg.CoverPageTitle.ToString());
            Param.UpdateMetadataValue(Param.TitlePage, dlg.TitlePage.ToString());
            if ((dlg.CoverPageTitle || dlg.TitlePage) && (dlg.Title.Trim().Length < 1))
            {
                // User wants a title page or a cover page with a title, but they haven't told us the title.
                // Make them enter one now.
                var message = LocalizationManager.GetString("ExportThroughPathway.SaveProperty.Message1", "Please enter a title for this publication.", "");
				Utils.MsgBox(message, dlg.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                dlg.IsExpanded = true;
                dlg.ResizeDialog();
                dlg.tabControl1.SelectedTab = dlg.tabPage1;
                dlg.txtBookTitle.Focus();
                return false;
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
                    var message = LocalizationManager.GetString("ExportThroughPathway.SaveProperty.Message2", "Please select at least one item to include in the export.", "");
					Utils.MsgBox(message, dlg.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dlg.IsExpanded = true;
                    dlg.ResizeDialog();
                    dlg.tabControl1.SelectedTab = dlg.tabPage3;
                    dlg.chkConfiguredDictionary.Focus();
                }
                Param.SetValue(Param.ConfigureDictionary, dlg.ExportMain.ToString());
                Param.SetValue(Param.ReversalIndex, dlg.ExportReversal.ToString());
                Param.SetValue(Param.GrammarSketch, dlg.ExportGrammar.ToString());
            }

            Param.SetValue(Param.Media, _media);
            Param.SetValue(Param.PublicationLocation, dlg.OutputFolder);
            Param.Write();
            return success;
        }

        private static void SaveDefaultProperty(ExportThroughPathway dlg)
        {
            Param.SetDefaultValue(Param.PrintVia, dlg.Format);
            Param.SetDefaultValue(Param.LayoutSelected, dlg.Style);
            // Processing Options tab
            if (string.IsNullOrEmpty(dlg.InputType) || dlg.InputType == "Dictionary")
            {
                Param.SetDefaultValue(Param.ConfigureDictionary, dlg.ExportMain.ToString());
                Param.SetDefaultValue(Param.ReversalIndex, dlg.ExportReversal.ToString());
                Param.SetDefaultValue(Param.GrammarSketch, dlg.ExportGrammar.ToString());
            }
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
                case "E-Book (Epub2 and Epub3)":
                    value = "application/epub+zip";
                    break;
                case "Go Bible":
                    // generic archive
                    value = "application/x-java-archive";
                    break;
                case "InDesign":
                    value = "application/x-indesign";
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
            SetTabbedHelpTopic();
            ShowHelp.ShowHelpTopicKeyPress(this, _helpTopic, Common.IsUnixOS());
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

            LoadDefaultSettings();
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

                if (Common.UnixVersionCheck())
                {
                    return true;
                }

                if (File.Exists(Common.PathCombine(pathwayPath, "ConfigurationTool.exe")))
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

            if (backend == "go bible" || backend == "dictionaryformids")
            {
                media = "mobile";
            }
            else if (backend == "e-book (epub2 and epub3)")//e-book (.epub)
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
            SetTabbedHelpTopic();
        }

        private void ddlStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            _publicationName = ddlStyle.Text.Replace(" ", "_");
            txtSaveInFolder.Text = Common.GetSaveInFolder(Param.DefaultValue[Param.PublicationLocation], DatabaseName, ddlStyle.Text.Replace(" ", "_"));
            if (_newSaveInFolderPath.Length > 0)
            {
                txtSaveInFolder.Text = Path.GetDirectoryName(_newSaveInFolderPath) + ddlStyle.Text.Replace(" ", "_") + "_" + _sDateTime;
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
            ShowHelp.ShowHelpTopicKeyPress(this, _helpTopic, Common.IsUnixOS());
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
                        var message = LocalizationManager.GetString("ExportThroughPathway.CoverImageClick.Message",
                            "The selected image is too large. Please select an image that is smaller than 1000 x 1000 pixels.", "");
						Utils.MsgBox(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    CoverPageImagePath = filename;
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

                string folderName = ddlStyle.Text + "_" + _sDateTime;
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
            rdoCustomCopyright.Checked = !(rdoStandardCopyright.Checked);
        }

        private void rdoCustomCopyright_CheckedChanged(object sender, EventArgs e)
        {
            // enable / disable Colophon UI
            ddlCopyrightStatement.Enabled = rdoStandardCopyright.Checked;
            txtColophonFile.Enabled = rdoCustomCopyright.Checked;
            btnBrowseColophon.Enabled = rdoCustomCopyright.Checked;
            txtColophonFile.Text = GetCopyRightFileName();
            rdoStandardCopyright.Checked = !(rdoCustomCopyright.Checked);
        }

        /// <summary>
        /// Get the Custom copyright file when exist in the location 
        /// </summary>
        /// <returns></returns>
        private string GetCopyRightFileName()
        {
            string copyrightFileName = Param.GetMetadataValue(Param.CopyrightPageFilename, Organization);
            if (!File.Exists(copyrightFileName))
            {
                var copyrightDir = Common.PathCombine(Common.GetPSApplicationPath(), "Copyrights");
                copyrightFileName = Common.PathCombine(copyrightDir, "SIL_Custom_Template.xhtml");
            }
            return copyrightFileName;
        }

        private void lnkChooseCopyright_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowHelp.ShowHelpTopicKeyPress(this, "Tasks/Basic_Tasks/Choosing_a_rights_statement_overview.htm", Common.IsUnixOS());
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
                            var copyrightDir = Common.PathCombine(Common.GetPSApplicationPath(), "Copyrights");
                            CopyrightPagePath = Common.PathCombine(copyrightDir, subnode.Attributes["file"].Value);
                        }
                    }
                }
            }
        }

        private void SetTabbedHelpTopic()
        {
            if (!IsExpanded)
            {
				_helpTopic = (DatabaseName == "{Project_Name}")
                                 ? "User_Interface/Dialog_boxes/Set_Defaults_dialog_box.htm"
                                 : "User_Interface/Dialog_boxes/Export_Through_Pathway_dialog_box.htm";
            }
            else
            {
                switch (tabControl1.SelectedIndex)
                {
                    case 0:
                        _helpTopic = "User_Interface/Dialog_boxes/Publication_Info_tab.htm";
                        break;
                    case 1:
                        _helpTopic = "User_Interface/Dialog_boxes/Front_Matter_tab.htm";
                        break;
                    case 2:
                        _helpTopic = "User_Interface/Dialog_boxes/Processing_Options_tab.htm";
                        break;
					case 3:
						_helpTopic = "User_Interface/Dialog_boxes/Hyphenation_tab.htm";
						break;
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTabbedHelpTopic();
			ShowHelp.ShowHelpTopic(this, _helpTopic, _isUnixOS, false);
        }

        private void btnHelpShow_Click(object sender, EventArgs e)
        {
            CallHelp();
        }

        private void CallHelp()
        {
            ShowHelp.ShowHelpTopicKeyPress(this, @"Concepts\Destination.htm", _isUnixOS);
        }

        private void ExportThroughPathway_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                {
                    ShowHelp.ShowHelpTopicKeyPress(this, _helpTopic, _isUnixOS);
                }
            }
            catch { }
        }

        private void chkHyphen_CheckedChanged(object sender, EventArgs e)
        {
            bool hyphencheck = chkHyphen.Checked;
            for (int i = 0; i < clbHyphenlang.Items.Count; i++)
				clbHyphenlang.SetItemCheckState(i, ((hyphencheck && Param.HyphenationSelectedLanguagelist.Contains(clbHyphenlang.GetItemText(clbHyphenlang.Items[i]))) ? CheckState.Checked : CheckState.Unchecked));
            clbHyphenlang.Enabled = false;
        }

		private void btnHelpHyphenation_Click(object sender, EventArgs e)
		{
			ShowHelp.ShowHelpTopicKeyPress(this, @"User_Interface/Dialog_boxes/Hyphenation_tab.htm", _isUnixOS);
		}

		private List<Control> GetAllControls(Control container, List<Control> list)
		{
			foreach (Control c in container.Controls)
			{
				if (c is TextBox) list.Add(c);
				if (c.Controls.Count > 0)
					list = GetAllControls(c, list);
			}

			return list;
		}
		private List<Control> GetAllControls(Control container)
		{
			return GetAllControls(container, new List<Control>());
		}

	    private void AssignFontandSizeAllTextbox()
	    {
			Param.LoadUiLanguageFontInfo();
		    string languageCode = Common.GetLocalizationSettings();
		    string fontName = string.Empty;
			var fontSize = 8f;

			Dictionary<string, string> fontInfo = Param.UiLanguageFontSettings[languageCode];
			foreach (KeyValuePair<string, string> fontValue in fontInfo)
			{
				fontName = fontValue.Key;
				fontSize = Convert.ToInt32(fontValue.Value);
			}

			List<Control> allTextboxes = GetAllControls(this);

		    
		    foreach (var textBoxCtrl in allTextboxes)
		    {
				Font txtBoxFont = new Font(fontName, fontSize);
			    textBoxCtrl.Font = txtBoxFont;
		    }
	    }
    }
}
