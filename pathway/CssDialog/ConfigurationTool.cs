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
        public string InputType=string.Empty ;
        public string MediaType = string.Empty;
        public string Style = string.Empty;
        #endregion

        #region Constructor
        public ConfigurationTool()
        {
            Trace.WriteLineIf(_traceOn.Level == TraceLevel.Verbose, "ConfigurationTool Constructor");
            InitializeComponent();
            _CToolBL.inputTypeBL = InputType;
            _CToolBL.MediaTypeEXE = MediaType;
            _CToolBL.StyleEXE = Style.Replace('&', ' ');
            _CToolBL.SetClassReference(this);
            _CToolBL.CreateToolTip();
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
                _CToolBL.inputTypeBL = Param.Value["InputType"];
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

        public TextBox TxtCopyright
        {
            get { return txtCopyright; }
        }

        public TextBox TxtInformation
        {
            get { return txtInformation; }
        }

        public Label Label3
        {
            get { return label3; }
        }

        public Label Label6
        {
            get { return label6; }
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

        public Label Label9
        {
            get { return label9; }
        }

        public ToolTip ToolTip1
        {
            get { return toolTip1; }
        }

        public TextBox TxtDescription
        {
            get { return txtDescription; }
        }

        public Label LblDescription
        {
            get { return lblDescription; }
        }

        public Label LblPublisher
        {
            get { return lblPublisher; }
        }

        public TextBox TxtSource
        {
            get { return txtSource; }
        }

        public Label LblSource
        {
            get { return lblSource; }
        }

        public TextBox TxtPublisher
        {
            get { return txtPublisher; }
        }

        public TextBox TxtCoverage
        {
            get { return txtCoverage; }
        }

        public Label LblCoverage
        {
            get { return lblCoverage; }
        }

        public TextBox TxtRelation
        {
            get { return txtRelation; }
        }

        public Label LblRelation
        {
            get { return lblRelation; }
        }

        public TextBox TxtFormat
        {
            get { return txtFormat; }
        }

        public Label LblFormat
        {
            get { return lblFormat; }
        }

        public TextBox TxtRights
        {
            get { return txtRights; }
        }

        public Label LblRights
        {
            get { return lblRights; }
        }

        public ComboBox DdlEmbedFonts
        {
            get { return ddlEmbedFonts; }
        }

        public Label LblEmbedFonts
        {
            get { return lblEmbedFonts; }
        }

        public PictureBox PicPreview
        {
            get { return picPreview; }
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
            _CToolBL.chkAvailable_ValidatedBL(sender);
        }

        private void txtComment_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtComment_ValidatedBL(sender);
        }

        private void chkAvailable_CheckedChanged(object sender, EventArgs e)
        {
            _CToolBL.chkAvailable_CheckedChangedBL();
        }

        private void tsUndo_Click(object sender, EventArgs e)
        {
            _CToolBL.tsUndo_ClickBL(sender, e);
        }

        private void tsRedo_Click(object sender, EventArgs e)
        {
            _CToolBL.tsRedo_ClickBL(sender, e);
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
            _CToolBL.stylesGrid_RowEnterBL(e);
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
            //_CToolBL.MediaType = "web";
            //_CToolBL.SideBar();
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
            EditCSS(sender,e);
        }

        private void tsPreview_Click(object sender, EventArgs e)
        {
            _CToolBL.tsPreview_ClickBL();
        }

        private void txtInformation_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtInformation_ValidatedBL();
            EditMobileCSS(sender, e);
        }

        private void txtCopyright_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtCopyright_ValidatedBL();
            EditMobileCSS(sender, e);
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
            _CToolBL.SetModifyMode(false);
            _CToolBL.ShowOthersSummaryBL();
        }

        private void txtName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _CToolBL.txtName_ValidatingBL(sender);
        }

        private void toolStripMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _CToolBL.txtName_ValidatingBL(sender);
        }

        private void txtPageGutterWidth_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }

        private void txtPageInside_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }

        private void txtPageOutside_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }

        private void txtPageTop_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }

        private void txtPageBottom_KeyUp(object sender, KeyEventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }

        private void ddlFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlFiles_SelectedIndexChangedBL(sender, e);
            EditMobileCSS(sender, e);
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

        private void ddlEmbedFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CToolBL.ddlEmbedFonts_SelectedIndexChangedBL(sender, e);
            EditOthersCSS(sender, e);
        }

        private void txtDescription_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtDescription_ValidatedBL();
            EditOthersCSS(sender, e);
        }
        private void txtDescription_KeyUp(object sender, EventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }
        private void txtPublisher_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtPublisher_ValidatedBL();
            EditOthersCSS(sender, e);
        }
        private void txtPublisher_KeyUp(object sender, EventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }
        private void txtSource_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtSource_ValidatedBL();
            EditOthersCSS(sender, e);
        }
        private void txtSource_KeyUp(object sender, EventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }
        private void txtFormat_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtFormat_ValidatedBL();
            EditOthersCSS(sender, e);
        }
        private void txtFormat_KeyUp(object sender, EventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }
        private void txtRelation_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtRelation_ValidatedBL();
            EditOthersCSS(sender, e);
        }
        private void txtRelation_KeyUp(object sender, EventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }
        private void txtCoverage_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtCoverage_ValidatedBL();
            EditOthersCSS(sender, e);
        }
        private void txtCoverage_KeyUp(object sender, EventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }
        private void txtRights_Validated(object sender, EventArgs e)
        {
            _CToolBL.txtRights_ValidatedBL();
            EditOthersCSS(sender, e);
        }
        private void txtRights_KeyUp(object sender, EventArgs e)
        {
            _CToolBL.SetModifyMode(true);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            _CToolBL.ShowPreview(1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _CToolBL.ShowPreview(2);
        }
     }
}
