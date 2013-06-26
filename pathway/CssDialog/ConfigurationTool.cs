// --------------------------------------------------------------------------------------------
// <copyright file="ConfiguraionTool.cs" from='2010' to='2011' company='SIL International'>
//      Copyright © 2010, SIL International. All Rights Reserved.   
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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class ConfigurationTool : Form
    {
        #region Private Variables
        public ConfigurationToolBL _CToolBL = new ConfigurationToolBL();
        public string _previousTxtName;
        private string _lastSelectedLayout = string.Empty;
        private TraceSwitch _traceOn = new TraceSwitch("General", "Trace level for application");
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
            InitializeComponent();

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
                    Directory.Delete(allUsersPath, true);
                }
            }
        }

        public void LoadSettings()
        {
            Trace.WriteLineIf(_CToolBL._traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: LoadSettings");
            //Note: Configuration tool can run by two ways
            //Note: 1 -  Standalone Application
            //Note: 2 -  The ConfigurationTool.EXE is called by PrintVia dialog from FLEX/TE/etc.,);
            if (_CToolBL.inputTypeBL.Length == 0)
            {
                //It will call when the Configtool from Application
                RemoveSettingsFile();
                if (!_fromNunit)
                    ValidateXMLVersion(Param.SettingPath);
                Param.LoadSettings(); // Load StyleSetting.xml
                _CToolBL.inputTypeBL = _CToolBL.LoadInputType();
            }
            else
            {
                //It will call when the Configtool from exe(FLEX/TE/etc.,)
                Param.LoadSettings(); // Load StyleSetting.xml
                Param.SetLoadType = _CToolBL.inputTypeBL;
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
                    _lastSelectedLayout = Param.Value["LayoutSelected"];
                    this.Close();
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

        //**EDB MOVE THIS**
        //public CheckBox ChkColophon
        //{
        //    get { return chkColophon; }
        //}

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
        //private System.Windows.Forms.TableLayoutPanel TLPanelOuter;
        //private System.Windows.Forms.TableLayoutPanel TLPanel1;
        //private System.Windows.Forms.TableLayoutPanel TLPanel2;
        //private System.Windows.Forms.TableLayoutPanel TLPanel3;
        //private System.Windows.Forms.Panel panel1;
        //private System.Windows.Forms.Panel panel2;
        //private System.Windows.Forms.Panel panel3; 
        #endregion

        #region Event Method
        private void ConfigurationTool_Load(object sender, EventArgs e)
        {
            _CToolBL = new ConfigurationToolBL();
            _CToolBL.inputTypeBL = InputType;
            _CToolBL.MediaTypeEXE = MediaType;
            _CToolBL.StyleEXE = Style.Replace('&', ' '); //
            _CToolBL.SetClassReference(this);
            _CToolBL.CreateToolTip();
            _CToolBL.ConfigurationTool_LoadBL();
        }

        public void tsDelete_Click(object sender, EventArgs e)
        {
            _CToolBL.tsDelete_ClickBL();
        }

        private void tsSend_Click(object sender, EventArgs e)
        {
            _CToolBL.tsSend_ClickBL();
        }

        private void tsNew_Click(object sender, EventArgs e)
        {
            _CToolBL.tsNew_ClickBL();
        }

        private void tsDefault_Click(object sender, EventArgs e)
        {
            _CToolBL.tsDefault_ClickBL();
        }

        private void txtPageGutterWidth_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtPageGutterWidth_ValidatedBL(sender, e);
        }

        private void txtPageInside_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtPageInside_ValidatedBL(sender, e);
        }

        private void txtPageOutside_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtPageOutside_ValidatedBL(sender, e);
        }

        private void txtPageTop_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtPageTop_ValidatedBL(sender, e);
        }

        private void txtPageBottom_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtPageBottom_ValidatedBL(sender, e);
        }

        private void ConfigurationTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            _CToolBL.ConfigurationTool_FormClosingBL();
            Style = _CToolBL.StyleEXE.ToString();
        }

        private void btnDictionary_Click(object sender, EventArgs e)
        {
            _CToolBL.btnDictionary_ClickBL();
        }

        private void btnScripture_Click(object sender, EventArgs e)
        {
            _CToolBL.btnScripture_ClickBL();
        }

        private void txtName_Validated(object sender, EventArgs e)
        {
            // _CToolBL.txtName_ValidatedBL(sender);
        }

        private void txtDesc_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtDesc_ValidatedBL(sender);
        }

        private void chkAvailable_Validated(object sender, EventArgs e)
        {
            //_CToolBL.chkAvailable_ValidatedBL(sender);
        }

        private void txtComment_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtComment_ValidatedBL(sender);
        }

        private void chkAvailable_CheckedChanged(object sender, EventArgs e)
        {
            _CToolBL.chkAvailable_CheckedChangedBL(sender);
        }

        private void tsUndo_Click(object sender, EventArgs e)
        {
            //_CToolBL.tsUndo_ClickBL(sender, e);
        }

        private void tsRedo_Click(object sender, EventArgs e)
        {
            //_CToolBL.tsRedo_ClickBL(sender, e);
        }

        private void txtApproved_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtApproved_ValidatedBL(sender);
        }

        private void ConfigurationTool_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.ConfigurationTool_KeyUpBL(sender, e);
        }

        private void stylesGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.Hand;

            //stylesGrid.Enabled = false;
            _CToolBL.stylesGrid_RowEnterBL(e);
            //stylesGrid.Enabled = true;
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            _previousTxtName = txtName.Text;
            SetGotFocusValue(sender, e);
        }

        private void btnPaper_Click(object sender, EventArgs e)
        {
            _CToolBL.MediaType = "paper";
            _CToolBL.SideBar();
        }

        private void btnMobile_Click(object sender, EventArgs e)
        {
            _CToolBL.MediaType = "mobile";
            _CToolBL.SideBar();
        }

        private void btnWeb_Click(object sender, EventArgs e)
        {
            _CToolBL.MediaType = "web";
            _CToolBL.SideBar();
        }

        private void btnOthers_Click(object sender, EventArgs e)
        {
            _CToolBL.MediaType = "others";
            _CToolBL.SideBar();
        }

        private void stylesGrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            _CToolBL.stylesGrid_ColumnWidthChangedBL(e);
        }

        private void txtDesc_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.txtDesc_KeyUpBL();
        }

        private void txtComment_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.txtComment_KeyUpBL();
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.txtName_KeyUpBL();
        }

        private void ddlPageColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlPageColumn_SelectedIndexChangedBL(sender, e);
            EditCSS(sender, e);
        }

        private void tsPreview_Click(object sender, EventArgs e)
        {
            _CToolBL.tsPreview_ClickBL();
        }

        private void ddlRedLetter_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlRedLetter_SelectedIndexChangedBL(sender, e);
            EditMobileCSS(sender, e);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            _CToolBL.btnBrowse_ClickBL();
            EditMobileCSS(sender, e);
        }

        private void ddlFileProduceDict_Validated(object sender, EventArgs e)
        {
            _CToolBL.ddlFileProduceDict_ValidatedBL(sender);
            EditMobileCSS(sender, e);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.tabControl1_SelectedIndexChangedBL();
        }

        private void tsSaveAs_Click(object sender, EventArgs e)
        {
            _CToolBL.tsSaveAs_ClickBL();
        }

        //public void Set(object sender, EventArgs e)
        //{
        //    _CToolBL.SetBL();
        //}

        public void SetGotFocusValue(object sender, EventArgs e)
        {
            _CToolBL.SetGotFocusValueBL(sender);
        }
        #endregion

        private void EditCSS(object sender, EventArgs e)
        {
            _CToolBL.SetModifyMode(false);
            _CToolBL.ShowCssSummary();
        }

        private void EditMobileCSS(object sender, EventArgs e)
        {
            _CToolBL.SetModifyMode(false);
            _CToolBL.ShowMobileSummaryBL();
        }

        private void EditOthersCSS(object sender, EventArgs e)
        {
            //_CToolBL.SetModifyMode(false);
            _CToolBL.ShowOthersSummaryBL();
        }

        private void txtName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _CToolBL.txtName_ValidatingBL(sender);
        }

        private void toolStripMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _CToolBL.PreviousStyleName = txtName.Text;
            _CToolBL.txtName_ValidatingBL(sender);
        }

        private void txtPageGutterWidth_KeyUp(object sender, KeyEventArgs e)
        {
            //if (!RangeCheck(txtPageGutterWidth.Text))
            //{
            //    txtPageGutterWidth.Text = "";
            //}
            _CToolBL.SetModifyMode(true);
        }

        private void txtPageInside_KeyUp(object sender, KeyEventArgs e)
        {
            //if (!RangeCheck(txtPageInside.Text))
            //{
            //    txtPageInside.Text = "";
            //}
            _CToolBL.SetModifyMode(true);
        }

        private bool RangeCheck(string txtValue)
        {
            bool isValid = true;
            try
            {
                if (txtValue.Trim().Length == 0 || txtValue.Trim() == "-" ||
                    txtValue.Trim() == "+")
                    return isValid;

                string numValue = txtValue.Replace("pt", "");
                numValue = numValue.Replace("p", "");
                float value = float.Parse(numValue);
                if (value < -200 || value > 200)
                {
                    isValid = false;
                }
            }
            catch (Exception)
            {
                isValid = false;
            }
            return isValid;

        }

        private void txtPageOutside_KeyUp(object sender, KeyEventArgs e)
        {
            //if (!RangeCheck(txtPageOutside.Text))
            //{
            //    txtPageOutside.Text = "";
            //}
            _CToolBL.SetModifyMode(true);
        }

        private void txtPageTop_KeyUp(object sender, KeyEventArgs e)
        {
            //if (!RangeCheck(txtPageTop.Text))
            //{
            //    txtPageTop.Text = "";
            //}
            _CToolBL.SetModifyMode(true);
        }

        private void txtPageBottom_KeyUp(object sender, KeyEventArgs e)
        {
            //if (!RangeCheck(txtPageBottom.Text))
            //{
            //    txtPageBottom.Text = "";
            //}
            _CToolBL.SetModifyMode(true);
        }

        private void ddlFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlFiles_SelectedIndexChangedBL(sender, e);
            EditMobileCSS(sender, e);
        }

        private void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlLanguage_SelectedIndexChangedBL(sender, e);
            EditMobileCSS(sender, e);
        }

        //**EDB MOVE THIS**
        //private void chkColophon_CheckedChanged(object sender, EventArgs e)
        //{
        //    _CToolBL.chkColophon_CheckedChangedBL(sender, e);
        //    EditOthersCSS(sender, e);
        //}

        private void chkIncludeFontVariants_CheckedChanged(object sender, EventArgs e)
        {
            _CToolBL.chkIncludeFontVariants_CheckedChangedBL(sender, e);
            EditOthersCSS(sender, e);
        }

        private void chkEmbedFonts_CheckedChanged(object sender, EventArgs e)
        {
            _CToolBL.chkEmbedFonts_CheckedChangedBL(sender, e);
            EditOthersCSS(sender, e);
        }

        private void txtBaseFontSize_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtBaseFontSize_ValidatedBL(sender);
            EditOthersCSS(sender, e);
        }

        private void txtDefaultLineHeight_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtDefaultLineHeight_ValidatedBL(sender);
            EditOthersCSS(sender, e);
        }

        private void ddlChapterNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlChapterNumbers_SelectedIndexChangedBL(sender, e);
            EditOthersCSS(sender, e);
        }

        private void ddlReferences_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlReferences_SelectedIndexChangedBL(sender, e);
            EditOthersCSS(sender, e);
        }

        private void ddlDefaultAlignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlDefaultAlignment_SelectedIndexChangedBL(sender, e);
            EditOthersCSS(sender, e);
        }

        private void ddlDefaultFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlDefaultFont_SelectedIndexChangedBL(sender, e);
            EditOthersCSS(sender, e);
        }

        private void ddlMissingFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlMissingFont_SelectedIndexChangedBL(sender, e);
            EditOthersCSS(sender, e);
        }

        private void ddlNonSILFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlNonSILFont_SelectedIndexChangedBL(sender, e);
            EditOthersCSS(sender, e);
        }

        private void txtInformation_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }

        private void txtCopyright_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }

        private void btnBrowse_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }

        private void txtMaxImageWidth_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtMaxImageWidth_ValidatedBL(sender);
            EditOthersCSS(sender, e);
        }

        //**EDB MOVE THIS**
        //private void txtDescription_Validated(object sender, EventArgs e)
        //{
        //    _CToolBL.txtDescription_ValidatedBL(sender);
        //    EditOthersCSS(sender, e);
        //}
        //private void txtPublisher_Validated(object sender, EventArgs e)
        //{
        //    _CToolBL.txtPublisher_ValidatedBL(sender);
        //    EditOthersCSS(sender, e);
        //}
        //private void txtSource_Validated(object sender, EventArgs e)
        //{
        //    _CToolBL.txtSource_ValidatedBL(sender);
        //    EditOthersCSS(sender, e);
        //}
        //private void txtFormat_Validated(object sender, EventArgs e)
        //{
        //    _CToolBL.txtFormat_ValidatedBL(sender);
        //    EditOthersCSS(sender, e);
        //}
        //private void txtRelation_Validated(object sender, EventArgs e)
        //{
        //    _CToolBL.txtRelation_ValidatedBL(sender);
        //    EditOthersCSS(sender, e);
        //}
        //private void txtCoverage_Validated(object sender, EventArgs e)
        //{
        //    _CToolBL.txtCoverage_ValidatedBL(sender);
        //    EditOthersCSS(sender, e);
        //}
        //private void txtRights_Validated(object sender, EventArgs e)
        //{
        //    _CToolBL.txtRights_ValidatedBL(sender);
        //    EditOthersCSS(sender, e);
        //}
        //private void txtBookTitle_Validated(object sender, EventArgs e)
        //{
        //    _CToolBL.txtBookTitle_ValidatedBL(sender);
        //    EditOthersCSS(sender, e);
        //}
        //private void txtCreator_Validated(object sender, EventArgs e)
        //{
        //    _CToolBL.txtCreator_ValidatedBL(sender);
        //    EditOthersCSS(sender, e);
        //}

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            _CToolBL.ShowPreview(1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _CToolBL.ShowPreview(2);
        }

        private void chkAvailable_CheckStateChanged(object sender, EventArgs e)
        {
            _CToolBL.chkAvailable_ValidatedBL(sender);
        }

        private void ddlRunningHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            EditCSS(sender, e);
            string pageType = ddlRunningHead.SelectedItem.ToString();
            _CToolBL.DdlRunningHeadSelectedIndexChangedBl(pageType);
        }

        //**EDB MOVE THIS**
        //private void btnCoverImage_Click(object sender, EventArgs e)
        //{
        //    _CToolBL.btnCoverImage_ClickBL();
        //    EditOthersCSS(sender, e);
        //}

        private void ddlTocLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlTocLevel_SelectedIndexChangedBL(sender, e);
            EditOthersCSS(sender, e);
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _CToolBL.HelpButton_Clicked();
        }

        private void studentManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _CToolBL.StudentManual();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _CToolBL.AboutDialog();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            _CToolBL.HelpButton_Clicked();
        }

        private void chkFixedLineHeight_CheckStateChanged(object sender, EventArgs e)
        {
            EditCSS(sender, e);
            _CToolBL.chkFixedLineHeight_CheckedChangedBL();
        }

        private void chkIncludeCusFnCaller_CheckStateChanged(object sender, EventArgs e)
        {
            EditCSS(sender, e);
            _CToolBL.chkIncludeCusFnCaller_CheckedChangedBL(sender, e);
        }

        private void txtFtpFileLocation_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtFtpFileLocation_ValidatedBL(sender, e);
        }

        private void txtWebUrl_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtWebUrl_ValidatedBL(sender, e);
        }

        private void chkIncludeImage_CheckedChanged(object sender, EventArgs e)
        {
            EditCSS(sender, e);
            _CToolBL.chkIncludeImage_CheckedChangedBL(sender, e);
        }

        private void lnkLblUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = Common.GetDefaultBrowserPath();
                p.StartInfo.Arguments = lnkLblUrl.Text;
                p.Start();
            }
            catch (Exception exc1)
            {
                // System.ComponentModel.Win32Exception is a known exception that occurs when Firefox is default browser.  
                // It actually opens the browser but STILL throws this exception so we can just ignore it.  If not this exception,
                // then attempt to open the URL in IE instead.
                if (exc1.GetType().ToString() != "System.ComponentModel.Win32Exception")
                {
                    // sometimes throws exception so we have to just ignore
                    // this is a common .NET bug that no one online really has a great reason for so now we just need to try to open
                    // the URL using IE if we can.
                    try
                    {
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("IExplore.exe", lnkLblUrl.Text);
                        System.Diagnostics.Process.Start(startInfo);
                        startInfo = null;
                    }
                    catch (Exception exc2)
                    {
                        // still nothing we can do so just show the error to the user here.
                    }
                }
            }
        }

        private void ddlPageNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlPageNumber_SelectedIndexChange();
        }

        private void ConfigurationTool_KeyDown(object sender, KeyEventArgs e)
        {
            if (_CToolBL.IsUnixOs)
            {
                if (e.KeyCode == Keys.ShiftKey)
                {
                    //bool noKey = (int)ModifierKeys == 0;
                    //bool controlKey = (ModifierKeys & Keys.Control) == Keys.Control;
                    bool shiftKey = (ModifierKeys & Keys.Shift) == Keys.Shift;

                    string allUsersPath = Common.GetAllUserPath();
                    if (ModifierKeys == Keys.Shift && shiftKey)
                    {
                        if (Directory.Exists(allUsersPath))
                        {
                            Directory.Delete(allUsersPath, true);
                        }
                    }
                }
            }
        }

        private void txtFnCallerSymbol_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.txtFnCallerSymbol_KeyUpBL();
            _CToolBL.SetModifyMode(true);
        }

        private void chkTurnOffFirstVerse_CheckStateChanged(object sender, EventArgs e)
        {
            EditCSS(sender, e);
            _CToolBL.chkTurnOffFirstVerse_CheckStateChangedBL(sender, e);
        }

        private void txtXrefCusSymbol_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.TxtXRefCusSymbol_KeyUpBL();
            _CToolBL.SetModifyMode(true);
        }

        private void chkXrefCusSymbol_CheckStateChanged(object sender, EventArgs e)
        {
            EditCSS(sender, e);
            _CToolBL.chkXrefCusSymbol_CheckStateChangedBL(sender, e);
        }

        private void chkPageBreaks_CheckedChanged(object sender, EventArgs e)
        {
            EditCSS(sender, e);
            _CToolBL.chkPageBreaks_CheckedChangedBL(sender, e);
        }
    }
}
