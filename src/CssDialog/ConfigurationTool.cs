// --------------------------------------------------------------------------------------------
// <copyright file="ConfiguraionTool.cs" from='2010' to='2014' company='SIL International'>
//      Copyright (C) 2010, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// Last reviewed: 
// 
// <remarks>
// Creates the Configuration Tool for Dictionary and Scripture 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using DesktopAnalytics;
using L10NSharp;
using Palaso.UI.WindowsForms;
using SIL.PublishingSolution.Properties;
using SIL.Tool;
using Palaso.Reporting;
using System.Collections.Generic;

namespace SIL.PublishingSolution
{
	public partial class ConfigurationTool : Form
	{
		#region Private Variables

		public ConfigurationToolBL _cToolBL = new ConfigurationToolBL();
		public string _previousTxtName;
		private string _lastSelectedLayout = string.Empty;
		private TraceSwitch _traceOn = new TraceSwitch("General", "Trace level for application");
		private static List<Exception> _pendingExceptionsToReportToAnalytics = new List<Exception>();

		#endregion

		#region Public Variable

		public bool _fromNunit = false;
		public string InputType = string.Empty;
		public string MediaType = string.Empty;
		public string Style = string.Empty;

		#endregion

		#region Constructor

		public ConfigurationTool()
		{
			Trace.WriteLineIf(_traceOn.Level == TraceLevel.Verbose, "ConfigurationTool Constructor");
			Common.InitializeOtherProjects();
			Common.SetupLocalization();
			InitializeComponent();
			if (Common.IsUnixOS())
			{
				ddlPagePageSize.Width = 200;
				ddlPageColumn.Width = 200;
				ddlJustified.Width = 200;
				ddlVerticalJustify.Width = 200;
				ddlPicture.Width = 200;
				ddlLeading.Width = 200;
				ddlRunningHead.Width = 200;
				//ddlReferenceFormat.Width = 200;
				//ddlPageNumber.Width = 200;
				//ddlRules.Width = 200;
			}
		}

		public static void ShowL10NsharpDlg()
		{
			Common.L10NMngr.ShowLocalizationDialogBox(false);
		}

		#endregion

		#region Method

		protected static void RemoveSettingsFile()
		{
			string allUsersPath = Common.GetAllUserPath();
			if (ModifierKeys == Keys.Shift)
			{
				if (Directory.Exists(allUsersPath))
				{
					DirectoryInfo di = new DirectoryInfo(allUsersPath);
					Common.CleanDirectory(di);
				}
			}
		}

		public void LoadSettings()
		{
			Trace.WriteLineIf(_cToolBL._traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: LoadSettings");
			//Note: Configuration tool can run by two ways
			//Note: 1 -  Standalone Application
			//Note: 2 -  The ConfigurationTool.EXE is called by PrintVia dialog from FLEX/TE/etc.,);
			string entryAssemblyName = string.Empty;
			if (!(String.IsNullOrEmpty(Assembly.GetEntryAssembly().FullName)))
			{
				entryAssemblyName = Assembly.GetEntryAssembly().FullName;
			}
			else
			{
				entryAssemblyName = "configurationtool";
			}
			if (entryAssemblyName.Trim().ToLower().Contains("configurationtool"))
			{
				//It will call when the Configtool from Application
				RemoveSettingsFile();
				if (!_fromNunit)
					ValidateXMLVersion(Param.SettingPath);
				Param.LoadSettings(); // Load StyleSetting.xml
				_cToolBL.inputTypeBL = _cToolBL.LoadInputType();
			}
			else
			{
				//It will call when the Configtool from exe(FLEX/TE/etc.,)
				Param.LoadSettings(); // Load StyleSetting.xml
				Param.SetLoadType = _cToolBL.inputTypeBL;
				tsDefault.Enabled = false;
			}
		}

		protected void ValidateXMLVersion(string filePath)
		{
			var versionControl = new SettingsVersionControl();
			var Validator = new SettingsValidator();
			if (File.Exists(filePath))
			{
				versionControl.UpdateSettingsFile(filePath);
				bool isValid = Validator.ValidateSettingsFile(filePath, false);
				if (!isValid)
				{
					if (Param.Value != null && Param.Value.Count > 0)
					{
						_lastSelectedLayout = Param.Value["LayoutSelected"];
						this.Close();
					}
				}
			}
		}

		#endregion

		#region Property

		public DataGridView StylesGrid
		{
			get { return stylesGrid; }
		}

		public ToolStrip ToolStripMain
		{
			get { return toolStripMain; }
		}

		public ToolStripButton TsNew
		{
			get { return tsNew; }
		}

		public ToolStripButton TsDelete
		{
			get { return tsDelete; }
		}

		public ToolStripButton TsUndo
		{
			get { return tsUndo; }
		}

		public ToolStripButton TsRedo
		{
			get { return tsRedo; }
		}

		public ToolStripButton TsPreview
		{
			get { return tsPreview; }
		}

		public ToolStripButton TsDefault
		{
			get { return tsDefault; }
		}

		public ToolStripButton TsSend
		{
			get { return tsSend; }
		}

		public ToolStripButton TsReset
		{
			get { return tsReset; }
		}

		public ToolStripSplitButton ToolStripHelpButton
		{
			get { return toolStripHelpButton; }
		}

		public TableLayoutPanel TableLayoutPanel2
		{
			get { return tableLayoutPanel2; }
		}

		public TabControl TabControl1
		{
			get { return tabControl1; }
		}

		public TabPage TabInfo
		{
			get { return tabInfo; }
		}

		public Button BtnApproved
		{
			get { return btnApproved; }
		}

		public Label LblComment
		{
			get { return lblComment; }
		}

		public CheckBox ChkAvailable
		{
			get { return chkAvailable; }
		}

		public CheckBox ChkPageBreaks
		{
			get { return chkPageBreaks; }
		}


		public TextBox TxtComment
		{
			get { return txtComment; }
		}

		public Label LblAvailable
		{
			get { return lblAvailable; }
		}

		public TextBox TxtDesc
		{
			get { return txtDesc; }
		}

		public Label LblDesc
		{
			get { return lblDesc; }
		}

		public TextBox TxtName
		{
			get { return txtName; }
		}

		public Label LblName
		{
			get { return lblName; }
		}

		public TabPage TabDisplay
		{
			get { return tabDisplay; }
		}

		public TextBox TxtApproved
		{
			get { return txtApproved; }
		}

		public Label LblApproved
		{
			get { return lblApproved; }
		}

		public Label Label2
		{
			get { return label2; }
		}

		public Label LblInfoCaption
		{
			get { return lblInfoCaption; }
		}

		public TextBox TxtCss
		{
			get { return txtCss; }
		}

		public ComboBox DdlPagePageSize
		{
			get { return ddlPagePageSize; }
		}

		public Label LblPagePageSize
		{
			get { return lblPagePageSize; }
		}

		public ComboBox DdlPageColumn
		{
			get { return ddlPageColumn; }
		}

		public ComboBox DdlPageNumber
		{
			get { return ddlPageNumber; }
		}

		public Label LblPageColumn
		{
			get { return lblPageColumn; }
		}

		public Label LblPageNumber
		{
			get { return lblPageNumber; }
		}

		public ToolStripButton TsSaveAs
		{
			get { return tsSaveAs; }
		}

		public ComboBox DdlJustified
		{
			get { return ddlJustified; }
		}

		public Label LblJustified
		{
			get { return lblJustified; }
		}

		public TextBox TxtPageOutside
		{
			get { return txtPageOutside; }
		}

		public TextBox TxtPageInside
		{
			get { return txtPageInside; }
		}

		public TextBox TxtPageTop
		{
			get { return txtPageTop; }
		}

		public TextBox TxtPageBottom
		{
			get { return txtPageBottom; }
		}

		public Label LblPageInside
		{
			get { return lblPageInside; }
		}

		public Label LblPageOutside
		{
			get { return lblPageOutside; }
		}

		public Label LblPageBottom
		{
			get { return lblPageBottom; }
		}

		public Label LblPageTop
		{
			get { return lblPageTop; }
		}

		public Label Label5
		{
			get { return label5; }
		}

		public TextBox TxtPageGutterWidth
		{
			get { return txtPageGutterWidth; }
		}

		//TD-3607
		public TextBox TxtGuidewordLength
		{
			get { return txtGuidewordLength; }
		}

		public Panel PnlGuidewordLength
		{
			get { return pnlGuidewordLength; }
		}

		public Label LblGuidewordLength
		{
			get { return lblGuidewordLength; }
		}

		public Label LblPageGutter
		{
			get { return lblPageGutter; }
		}

		public Button BtnScripture
		{
			get { return btnScripture; }
		}

		public Button BtnDictionary
		{
			get { return btnDictionary; }
		}

		public Label LblType
		{
			get { return lblType; }
		}

		public Button BtnWeb
		{
			get { return btnWeb; }
		}

		public Button BtnMobile
		{
			get { return btnMobile; }
		}

		public Button BtnPaper
		{
			get { return btnPaper; }
		}

		public Button BtnPrevious
		{
			get { return btnPrevious; }
		}

		public Button BtnNext
		{
			get { return btnNext; }
		}

		public ComboBox DdlVerticalJustify
		{
			get { return ddlVerticalJustify; }
		}

		public Label LblVerticalJustify
		{
			get { return lblVerticalJustify; }
		}

		public ComboBox DdlSense
		{
			get { return ddlSense; }
		}

		public Label LblSenseLayout
		{
			get { return lblSenseLayout; }
		}

		public ComboBox DdlPicture
		{
			get { return ddlPicture; }
		}

		public Label LblLineSpace
		{
			get { return lblLineSpace; }
		}

		public Label LblRunningHeader
		{
			get { return lblRunningHeader; }
		}

		public ComboBox DdlLeading
		{
			get { return ddlLeading; }
		}

		public ComboBox DdlRunningHead
		{
			get { return ddlRunningHead; }
		}

		public Label LblRules
		{
			get { return lblRules; }
		}

		public ComboBox DdlRules
		{
			get { return ddlRules; }
		}

		public Label LblFont
		{
			get { return lblFont; }
		}

		public ComboBox DdlFontSize
		{
			get { return ddlFontSize; }
		}

		public Label Label4
		{
			get { return label4; }
		}

		public Button BtnOthers
		{
			get { return btnOthers; }
		}

		public TabPage TabMobile
		{
			get { return tabMobile; }
		}

		public TabPage TabOthers
		{
			get { return tabOthers; }
		}

		public Button BtnBrowse
		{
			get { return btnBrowse; }
		}

		public Label Label1
		{
			get { return label1; }
		}

		public ComboBox DdlRedLetter
		{
			get { return ddlRedLetter; }
		}

		public Label Label7
		{
			get { return label7; }
		}

		public ComboBox DdlFiles
		{
			get { return ddlFiles; }
		}

		public ComboBox DdlLanguage
		{
			get { return ddlLanguage; }
		}

		public ComboBox DdlTocLevel
		{
			get { return ddlTocLevel; }
		}

		public Label Label8
		{
			get { return label8; }
		}

		public PictureBox MobileIcon
		{
			get { return mobileIcon; }
		}

		public ComboBox DdlFileProduceDict
		{
			get { return ddlFileProduceDict; }
		}

		public Label LblFileProduceDict
		{
			get { return lblFileProduceDict; }
		}

		public ToolTip ToolTip1
		{
			get { return toolTip1; }
		}

		public TextBox TxtBaseFontSize
		{
			get { return txtBaseFontSize; }
		}

		public TextBox TxtDefaultLineHeight
		{
			get { return txtDefaultLineHeight; }
		}

		public ComboBox DdlDefaultAlignment
		{
			get { return ddlDefaultAlignment; }
		}

		public Label LblChapterNumbers
		{
			get { return lblChapterNumbers; }
		}

		public ComboBox DdlChapterNumbers
		{
			get { return ddlChapterNumbers; }
		}

		public Label LblReferences
		{
			get { return lblReferences; }
		}

		public ComboBox DdlReferences
		{
			get { return ddlReferences; }
		}

		public Label LblEpubFontsSection
		{
			get { return lblEpubFontsSection; }
		}

		public PictureBox PicFonts
		{
			get { return picFonts; }
		}

		public Label LblDefaultFont
		{
			get { return lblDefaultFont; }
		}

		public ComboBox DdlDefaultFont
		{
			get { return ddlDefaultFont; }
		}

		public Label LblMissingFont
		{
			get { return lblMissingFont; }
		}

		public ComboBox DdlMissingFont
		{
			get { return ddlMissingFont; }
		}

		public Label LblNonSILFont
		{
			get { return lblNonSILFont; }
		}

		public ComboBox DdlNonSILFont
		{
			get { return ddlNonSILFont; }
		}

		public TextBox TxtMaxImageWidth
		{
			get { return txtMaxImageWidth; }
		}

		public Label LblMaxImageWidth
		{
			get { return lblMaxImageWidth; }
		}

		public Label LblPx
		{
			get { return lblPx; }
		}

		public CheckBox ChkEmbedFonts
		{
			get { return chkEmbedFonts; }
		}

		public CheckBox ChkIncludeFontVariants
		{
			get { return chkIncludeFontVariants; }
		}

		public PictureBox PicPreview
		{
			get { return picPreview; }
		}

		public TextBox TxtFtpAddress
		{
			get { return txtFtpFileLocation; }
		}

		public TextBox TxtFtpUsername
		{
			get { return txtFtpUsername; }
		}

		public TextBox TxtFtpPassword
		{
			get { return txtFtpPassword; }
		}

		public TextBox TxtSqlServerName
		{
			get { return txtSqlServerName; }
		}

		public TextBox TxtSqlDBName
		{
			get { return txtSqlDBName; }
		}

		public TextBox TxtSqlUsername
		{
			get { return txtSqlUsername; }
		}

		public TextBox TxtSqlPassword
		{
			get { return txtSqlPassword; }
		}

		public TextBox TxtWebUrl
		{
			get { return txtWebUrl; }
		}

		public TextBox TxtWebAdminUsrNme
		{
			get { return txtWebAdminUsrNme; }
		}

		public TextBox TxtWebAdminPwd
		{
			get { return txtWebAdminPwd; }
		}

		public TextBox TxtWebAdminSiteNme
		{
			get { return txtWebAdminSiteNme; }
		}

		public TextBox TxtWebEmailId
		{
			get { return txtWebEmailID; }
		}

		public TextBox TxtWebFtpFldrNme
		{
			get { return txtWebFtpFldrNme; }
		}

		public CheckBox ChkFixedLineHeight
		{
			get { return chkFixedLineHeight; }
		}

		public CheckBox ChkIncludeImage
		{
			get { return chkIncludeImage; }
		}

		public ComboBox DdlReferenceFormat
		{
			get { return ddlReferenceFormat; }
		}

		public Label LblReferenceFormat
		{
			get { return lblReferenceFormat; }
		}

		public Panel PnlOtherFormat
		{
			get { return pnlOtherFormat; }
		}

		public Panel PnlReferenceFormat
		{
			get { return pnlReferenceFormat; }
		}

		public CheckBox ChkIncludeCusFnCaller
		{
			get { return chkIncludeCusFnCaller; }
		}

		public TextBox TxtFnCallerSymbol
		{
			get { return txtFnCallerSymbol; }
		}

		public CheckBox ChkXrefCusSymbol
		{
			get { return chkXrefCusSymbol; }
		}

		public TextBox TxtXrefCusSymbol
		{
			get { return txtXrefCusSymbol; }
		}

		public CheckBox ChkTurnOffFirstVerse
		{
			get { return chkTurnOffFirstVerse; }
		}

		public CheckBox ChkHideSpaceVerseNo
		{
			get { return chkHideSpaceVerseNo; }
		}

		public CheckBox ChkSplitFileByLetter
		{
			get { return chkSplitFileByLetter; }
		}

		public CheckBox ChkDisableWO
		{
			get { return chkDisableWO; }
		}

        public ComboBox DdlHeaderFontSize
        {
            get { return ddlHeaderFontSize; }
        }

        public CheckBox ChkCenterTitleHeader
        {
            get { return chkCenterTitleHeader; }
        }

		#endregion

		#region Event Method

		private void ConfigurationTool_Load(object sender, EventArgs e)
		{
			SetUpErrorHandling();
            Param.LoadUiLanguageFontInfo();
            UpdateFontOnL10NSharp(LocalizationManager.UILanguageId);

			_cToolBL = new ConfigurationToolBL();
			_cToolBL.inputTypeBL = InputType;
			_cToolBL.MediaTypeEXE = MediaType;
			_cToolBL.StyleEXE = Style.Replace('&', ' '); //
			_cToolBL.SetClassReference(this);
			_cToolBL.CreateToolTip();
			_cToolBL.ConfigurationTool_LoadBL();
		}

		public void tsDelete_Click(object sender, EventArgs e)
		{
			_cToolBL.tsDelete_ClickBL();
		}

		private void tsSend_Click(object sender, EventArgs e)
		{
			_cToolBL.tsSend_ClickBL();
		}

		private void tsNew_Click(object sender, EventArgs e)
		{
			_cToolBL.tsNew_ClickBL();
		}

		private void tsDefault_Click(object sender, EventArgs e)
		{
			_cToolBL.tsDefault_ClickBL();
		}

		private void txtPageGutterWidth_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtPageGutterWidth_ValidatedBL(sender, e);
		}

		private void txtPageInside_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtPageInside_ValidatedBL(sender, e);
		}

		private void txtPageOutside_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtPageOutside_ValidatedBL(sender, e);
		}

		private void txtPageTop_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtPageTop_ValidatedBL(sender, e);
		}

		private void txtPageBottom_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtPageBottom_ValidatedBL(sender, e);
		}

		private void ConfigurationTool_FormClosing(object sender, FormClosingEventArgs e)
		{
			_cToolBL.ConfigurationTool_FormClosingBL();
			Style = _cToolBL.StyleEXE.ToString();
			Settings.Default.Save();
			Common.SaveLocalizationSettings(Settings.Default.UserInterfaceLanguage, null, null);
		}
		private void btnDictionary_Click(object sender, EventArgs e)
		{
			_cToolBL.btnDictionary_ClickBL();
		}

		private void btnScripture_Click(object sender, EventArgs e)
		{
			_cToolBL.btnScripture_ClickBL();
		}

		private void chkAvailable_CheckedChanged(object sender, EventArgs e)
		{
			_cToolBL.chkAvailable_CheckedChangedBL(sender);
		}

		private void txtApproved_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtApproved_ValidatedBL(sender);
		}

		private void ConfigurationTool_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.ConfigurationTool_KeyUpBL(sender, e);
		}

		private void stylesGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			Cursor.Current = Cursors.Hand;
			_cToolBL.stylesGrid_RowEnterBL(e);
		}

		private void txtName_Enter(object sender, EventArgs e)
		{
			_previousTxtName = txtName.Text;
			SetGotFocusValue(sender, e);
		}

		private void btnPaper_Click(object sender, EventArgs e)
		{
			_cToolBL.MediaTypeEXE = "paper";
			_cToolBL.MediaType = "paper";
			_cToolBL.SideBar();
		}

		private void btnMobile_Click(object sender, EventArgs e)
		{
			_cToolBL.MediaTypeEXE = "mobile";
			_cToolBL.MediaType = "mobile";
			_cToolBL.SideBar();
		}

		private void btnWeb_Click(object sender, EventArgs e)
		{
			_cToolBL.MediaTypeEXE = "web";
			_cToolBL.MediaType = "web";
			_cToolBL.SideBar();
		}

		private void btnOthers_Click(object sender, EventArgs e)
		{
			_cToolBL.MediaTypeEXE = "others";
			_cToolBL.MediaType = "others";
			_cToolBL.SideBar();
		}

		private void stylesGrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			_cToolBL.stylesGrid_ColumnWidthChangedBL(e);
		}

		private void txtDesc_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.txtDesc_KeyUpBL();
			_cToolBL.txtDesc_ValidatedBL(sender, txtDesc.Modified);
		}

		private void txtComment_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.txtComment_KeyUpBL();
			_cToolBL.txtComment_ValidatedBL(sender, txtComment.Modified);
		}

		private void txtName_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.txtName_KeyUpBL();
		}

		private void ddlPageColumn_SelectedIndexChanged(object sender, EventArgs e)
		{
			_cToolBL.ddlPageColumn_SelectedIndexChangedBL(sender, e);
			EditCSS(sender, e);
		}

		private void tsPreview_Click(object sender, EventArgs e)
		{
			_cToolBL.tsPreview_ClickBL();
		}

		private void ddlRedLetter_SelectedIndexChanged(object sender, EventArgs e)
		{
			_cToolBL.ddlRedLetter_SelectedIndexChangedBL(sender, e);
			EditMobileCSS(sender, e);
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			_cToolBL.btnBrowse_ClickBL();
			EditMobileCSS(sender, e);
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			_cToolBL.tabControl1_SelectedIndexChangedBL();
		}

		private void tsSaveAs_Click(object sender, EventArgs e)
		{
			_cToolBL.tsSaveAs_ClickBL();
		}

		public void SetGotFocusValue(object sender, EventArgs e)
		{
			_cToolBL.SetGotFocusValueBL(sender);
		}
		#endregion

        #region UI Language Fonts

	    /// <summary>
        /// To set the font-name and font-size to the controls in the form.
        /// </summary>
        /// <param name="langId"></param>
        public void UpdateFontOnL10NSharp(string langId)
        {
            string fontName = "Microsoft Sans Serif";
            string fontSize = "8";
            
            Param.GetFontValues(langId, ref fontName, ref fontSize);

	        //For all labels and textboxes
            List<Control> allControls = GetAllControls(this);
            allControls.ForEach(k => k.Font = new Font(fontName, float.Parse(fontSize)));
            //For ToolStripMenu Items
            for (int i = 0; i < this.toolStripMain.Items.Count; i++)
            {
                toolStripMain.Items[i].Font = new Font(fontName,  float.Parse(fontSize), FontStyle.Bold);
            }
            //For TabControl
            this.tabControl1.Font = new Font(fontName,  float.Parse(fontSize), FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
        }

        /// <summary>
        /// Get the list of controls in the form
        /// </summary>
        /// <param name="container"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<Control> GetAllControls(Control container, List<Control> list)
        {
            var ignoreControls = new List<string>();
            ignoreControls = IgnoreControlList();
            foreach (Control c in container.Controls)
            {
                if (c.Controls.Count > 0)
                    list = GetAllControls(c, list);
                else
                {
                    if (!ignoreControls.Contains(c.Name))
                    {
                        list.Add(c);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// List og Ignored controls which not under UILanguage changes
        /// </summary>
        /// <returns>List of ignored items</returns>
        private List<string> IgnoreControlList()
        {
            var ignoreControls = new List<string>();
            ignoreControls.Add("lblType");
            ignoreControls.Add("lblInfoCaption");
            ignoreControls.Add("txtCss");
            ignoreControls.Add("txtName");
            ignoreControls.Add("txtDesc");
            ignoreControls.Add("txtComment");

            return ignoreControls;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        private List<Control> GetAllControls(Control container)
        {
            return GetAllControls(container, new List<Control>());
        }

        #endregion

		private void EditCSS(object sender, EventArgs e)
		{
			_cToolBL.SetModifyMode(false);
			_cToolBL.ShowCssSummary();
		}

		private void EditMobileCSS(object sender, EventArgs e)
		{
			_cToolBL.SetModifyMode(false);
			_cToolBL.ShowMobileSummaryBL();
		}

		private void EditOthersCSS(object sender, EventArgs e)
		{
			_cToolBL.ShowOthersSummaryBL();
		}

		private void txtName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_cToolBL.txtName_ValidatingBL(sender);
		}

		private void toolStripMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			//_CToolBL.PreviousStyleName = txtName.Text;
			//_CToolBL.txtName_ValidatingBL(sender);
		}

		private void txtPageGutterWidth_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.SetModifyMode(true);
		}

		private void txtPageInside_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.SetModifyMode(true);
		}

		private void txtPageOutside_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.SetModifyMode(true);
		}

		private void txtPageTop_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.SetModifyMode(true);
		}

		private void txtPageBottom_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.SetModifyMode(true);
		}

		private void ddlFiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			_cToolBL.ddlFiles_SelectedIndexChangedBL(sender, e);
			EditMobileCSS(sender, e);
		}

		private void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
		{
			_cToolBL.ddlLanguage_SelectedIndexChangedBL(sender, e);
			EditMobileCSS(sender, e);
		}

		private void chkIncludeFontVariants_CheckedChanged(object sender, EventArgs e)
		{
			_cToolBL.chkIncludeFontVariants_CheckedChangedBL(sender, e);
			EditOthersCSS(sender, e);
		}

		private void chkEmbedFonts_CheckedChanged(object sender, EventArgs e)
		{
			_cToolBL.chkEmbedFonts_CheckedChangedBL(sender, e);
			EditOthersCSS(sender, e);
		}

		private void txtBaseFontSize_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtBaseFontSize_ValidatedBL(sender);
			EditOthersCSS(sender, e);
		}

		private void txtDefaultLineHeight_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtDefaultLineHeight_ValidatedBL(sender);
			EditOthersCSS(sender, e);
		}

		private void ddlChapterNumbers_SelectedIndexChanged(object sender, EventArgs e)
		{
			_cToolBL.ddlChapterNumbers_SelectedIndexChangedBL(sender, e);
			EditOthersCSS(sender, e);
		}

		private void ddlReferences_SelectedIndexChanged(object sender, EventArgs e)
		{
			_cToolBL.ddlReferences_SelectedIndexChangedBL(sender, e);
			EditOthersCSS(sender, e);
		}

		private void ddlDefaultAlignment_SelectedIndexChanged(object sender, EventArgs e)
		{
			_cToolBL.ddlDefaultAlignment_SelectedIndexChangedBL(sender, e);
			EditOthersCSS(sender, e);
		}

		private void ddlDefaultFont_SelectedIndexChanged(object sender, EventArgs e)
		{
			_cToolBL.ddlDefaultFont_SelectedIndexChangedBL(sender, e);
			EditOthersCSS(sender, e);
		}

		private void ddlMissingFont_SelectedIndexChanged(object sender, EventArgs e)
		{
			_cToolBL.ddlMissingFont_SelectedIndexChangedBL(sender, e);
			EditOthersCSS(sender, e);
		}

		private void ddlNonSILFont_SelectedIndexChanged(object sender, EventArgs e)
		{
			_cToolBL.ddlNonSILFont_SelectedIndexChangedBL(sender, e);
			EditOthersCSS(sender, e);
		}

		private void txtMaxImageWidth_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtMaxImageWidth_ValidatedBL(sender);
			EditOthersCSS(sender, e);
		}

		private void btnPrevious_Click(object sender, EventArgs e)
		{
			_cToolBL.ShowPreview(1);
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			_cToolBL.ShowPreview(2);
		}

		private void chkAvailable_CheckStateChanged(object sender, EventArgs e)
		{
			_cToolBL.chkAvailable_ValidatedBL(sender);
		}

		private void ddlRunningHead_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!Common.Testing)
			{
				string pageType = ((ComboBoxItem) ddlRunningHead.SelectedItem).Value;
				_cToolBL.DdlRunningHeadSelectedIndexChangedBl(pageType);
				EditCSS(sender, e);
			}
		}

		private void ddlTocLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			_cToolBL.ddlTocLevel_SelectedIndexChangedBL(sender, e);
			EditOthersCSS(sender, e);
		}

		private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_cToolBL.HelpButton_Clicked(this);
		}

		private void studentManualToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_cToolBL.StudentManual();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_cToolBL.AboutDialog();
		}

		private void toolStripHelpButton_ButtonClick(object sender, EventArgs e)
		{
			_cToolBL.HelpButton_Clicked(this);
		}

		private void chkFixedLineHeight_CheckStateChanged(object sender, EventArgs e)
		{
			EditCSS(sender, e);
			_cToolBL.chkFixedLineHeight_CheckedChangedBL();
		}

		private void chkIncludeCusFnCaller_CheckStateChanged(object sender, EventArgs e)
		{
			EditCSS(sender, e);
			_cToolBL.chkIncludeCusFnCaller_CheckedChangedBL(sender, e);
		}

		private void txtFtpFileLocation_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtFtpFileLocation_ValidatedBL(sender, e);
		}

		private void txtWebUrl_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtWebUrl_ValidatedBL(sender, e);
		}

		private void chkIncludeImage_CheckedChanged(object sender, EventArgs e)
		{
			EditCSS(sender, e);
			_cToolBL.chkIncludeImage_CheckedChangedBL(sender, e);
		}

		private void lnkLblUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			using (Process.Start("http://pathway.sil.org/"))
			{
			}
		}

		private void txtFnCallerSymbol_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.txtFnCallerSymbol_KeyUpBL();
			_cToolBL.SetModifyMode(true);
		}

		private void chkTurnOffFirstVerse_CheckStateChanged(object sender, EventArgs e)
		{
			EditCSS(sender, e);
			_cToolBL.chkTurnOffFirstVerse_CheckStateChangedBL(sender, e);
		}

		private void txtXrefCusSymbol_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.TxtXRefCusSymbol_KeyUpBL();
			_cToolBL.SetModifyMode(true);
		}

		private void chkXrefCusSymbol_CheckStateChanged(object sender, EventArgs e)
		{
			EditCSS(sender, e);
			_cToolBL.chkXrefCusSymbol_CheckStateChangedBL(sender, e);
		}

		private void chkPageBreaks_CheckedChanged(object sender, EventArgs e)
		{
			EditCSS(sender, e);
			_cToolBL.chkPageBreaks_CheckedChangedBL(sender, e);
		}

		private void chkHideSpaceVerseNo_CheckStateChanged(object sender, EventArgs e)
		{
			EditCSS(sender, e);
			_cToolBL.chkHideSpaceVerseNo_CheckStateChangedBL(sender, e);
		}

		private void stylesGrid_SelectionChanged(object sender, EventArgs e)
		{
			_cToolBL.stylesGrid_SelectionChanged(sender, e);
		}

		private void chkSplitFileByLetter_CheckStateChanged(object sender, EventArgs e)
		{
			EditCSS(sender, e);
			_cToolBL.chkSplitFileByLetter_CheckStateChangedBL(sender, e);
		}

		private void chkDisableWO_CheckStateChanged(object sender, EventArgs e)
		{
			EditCSS(sender, e);
			_cToolBL.chkDisableWO_CheckStateChangedBL(sender, e);
		}

		private void tsReset_Click(object sender, EventArgs e)
		{
			_cToolBL.tsReset_ClickBL();
		}

		private void moreHelpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (Process.Start("http://pathway.sil.org/demo/accessing-online-help-and-student-guide/"))
			{
			}
		}

		private void txtSqlUsername_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtSqlUsername_ValidatedBL(sender, e);
		}

		private void txtSqlPassword_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtSqlPassword_ValidatedBL(sender, e);
		}

		private void txtWebAdminUsrNme_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtWebAdminUsrNme_ValidatedBL(sender, e);
		}

		private void txtWebAdminPwd_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtWebAdminPwd_ValidatedBL(sender, e);
		}

		private void txtFtpUsername_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtFtpUsername_ValidatedBL(sender, e);
		}

		private void txtFtpPassword_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtFtpPassword_ValidatedBL(sender, e);
		}

		private void txtWebEmailID_Validated(object sender, EventArgs e)
		{
			_cToolBL.txtWebEmailID_ValidatedBL(sender, e);
		}

		#region Localization
		public static string GetUserConfigFilePath()
		{
			try
			{
				return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
			}
			catch (System.Configuration.ConfigurationErrorsException e)
			{
				_pendingExceptionsToReportToAnalytics.Add(e);
				File.Delete(e.Filename);
				return e.Filename;
			}
		}

		/// <summary>
		/// The email address people should write to with problems (or new localizations?) for HearThis.
		/// </summary>
		public static string IssuesEmailAddress
		{
			get { return "pathway@sil.org"; }
		}

		/// ------------------------------------------------------------------------------------
		private static void SetUpErrorHandling()
		{
			if (ErrorReport.EmailAddress == null)
			{
				ExceptionHandler.Init();
				ErrorReport.EmailAddress = IssuesEmailAddress;
				ErrorReport.AddStandardProperties();
				ExceptionHandler.AddDelegate(ReportError);
			}
		}

		private static void ReportError(object sender, CancelExceptionHandlingEventArgs e)
		{
			Analytics.ReportException(e.Exception);
		}
		#endregion

        private void userInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var uiLanguage = new UILanguageDialog();
            uiLanguage.ShowDialog();
            UpdateFontOnL10NSharp(LocalizationManager.UILanguageId);
            _cToolBL.ConfigurationTool_LoadBL();
        }

        private void chkCenterTitleHeader_CheckStateChanged(object sender, EventArgs e)
        {
            EditCSS(sender, e);
            _cToolBL.chkCenterTitleHeader_CheckStateChangedBL();
        }

		private void txtBaseFontSize_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.SetModifyMode(true);
			_cToolBL.txtBaseFontSize_ValidatedBL(sender);
		}

		private void txtDefaultLineHeight_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.SetModifyMode(true);
			_cToolBL.txtDefaultLineHeight_ValidatedBL(sender);
		}

		private void txtMaxImageWidth_KeyUp(object sender, KeyEventArgs e)
		{
			_cToolBL.SetModifyMode(true);
			_cToolBL.txtMaxImageWidth_ValidatedBL(sender);
		}

	}
}
