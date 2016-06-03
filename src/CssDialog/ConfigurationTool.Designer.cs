using System.Windows.Forms;

namespace SIL.PublishingSolution
{
    partial class ConfigurationTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationTool));
			this.tsNew = new System.Windows.Forms.ToolStripButton();
			this.stylesGrid = new System.Windows.Forms.DataGridView();
			this.toolStripMain = new System.Windows.Forms.ToolStrip();
			this.tsSaveAs = new System.Windows.Forms.ToolStripButton();
			this.tsDelete = new System.Windows.Forms.ToolStripButton();
			this.tsUndo = new System.Windows.Forms.ToolStripButton();
			this.tsRedo = new System.Windows.Forms.ToolStripButton();
			this.tsPreview = new System.Windows.Forms.ToolStripButton();
			this.tsDefault = new System.Windows.Forms.ToolStripButton();
			this.tsReset = new System.Windows.Forms.ToolStripButton();
			this.tsSend = new System.Windows.Forms.ToolStripButton();
			this.toolStripHelpButton = new System.Windows.Forms.ToolStripSplitButton();
			this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.studentManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.moreHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.userInterfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabInfo = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblName = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblDesc = new System.Windows.Forms.Label();
			this.txtApproved = new System.Windows.Forms.TextBox();
			this.txtDesc = new System.Windows.Forms.TextBox();
			this.lblApproved = new System.Windows.Forms.Label();
			this.lblAvailable = new System.Windows.Forms.Label();
			this.chkAvailable = new System.Windows.Forms.CheckBox();
			this.txtComment = new System.Windows.Forms.TextBox();
			this.lblComment = new System.Windows.Forms.Label();
			this.lnkLblUrl = new System.Windows.Forms.LinkLabel();
			this.lblProjectUrl = new System.Windows.Forms.Label();
			this.btnApproved = new System.Windows.Forms.Button();
			this.tabDisplay = new System.Windows.Forms.TabPage();
			this.tblPnlDisplay = new System.Windows.Forms.TableLayoutPanel();
			this.lblPagePageSize = new System.Windows.Forms.Label();
			this.pnlOtherFormat = new System.Windows.Forms.Panel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblPageNumber = new System.Windows.Forms.Label();
			this.chkSplitFileByLetter = new System.Windows.Forms.CheckBox();
			this.ddlPageNumber = new System.Windows.Forms.ComboBox();
			this.ddlSense = new System.Windows.Forms.ComboBox();
			this.ddlFileProduceDict = new System.Windows.Forms.ComboBox();
			this.lblSenseLayout = new System.Windows.Forms.Label();
			this.lblRules = new System.Windows.Forms.Label();
			this.lblFileProduceDict = new System.Windows.Forms.Label();
			this.ddlRules = new System.Windows.Forms.ComboBox();
			this.lblFont = new System.Windows.Forms.Label();
			this.ddlFontSize = new System.Windows.Forms.ComboBox();
			this.chkCenterTitleHeader = new System.Windows.Forms.CheckBox();
			this.ddlHeaderFontSize = new System.Windows.Forms.ComboBox();
			this.lblHeaderFontSize = new System.Windows.Forms.Label();
			this.pnlGuidewordLength = new System.Windows.Forms.Panel();
			this.txtGuidewordLength = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.pnlReferenceFormat = new System.Windows.Forms.Panel();
			this.tblPnlRefFormat = new System.Windows.Forms.TableLayoutPanel();
			this.chkHideSpaceVerseNo = new System.Windows.Forms.CheckBox();
			this.txtFnCallerSymbol = new System.Windows.Forms.TextBox();
			this.chkTurnOffFirstVerse = new System.Windows.Forms.CheckBox();
			this.txtXrefCusSymbol = new System.Windows.Forms.TextBox();
			this.chkIncludeCusFnCaller = new System.Windows.Forms.CheckBox();
			this.chkXrefCusSymbol = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.ddlReferenceFormat = new System.Windows.Forms.ComboBox();
			this.lblReferenceFormat = new System.Windows.Forms.Label();
			this.chkDisableWO = new System.Windows.Forms.CheckBox();
			this.ddlPagePageSize = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.ddlRunningHead = new System.Windows.Forms.ComboBox();
			this.lblRunningHeader = new System.Windows.Forms.Label();
			this.chkFixedLineHeight = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblPageInside = new System.Windows.Forms.Label();
			this.txtPageInside = new System.Windows.Forms.TextBox();
			this.lblPageOutside = new System.Windows.Forms.Label();
			this.txtPageOutside = new System.Windows.Forms.TextBox();
			this.lblPageTop = new System.Windows.Forms.Label();
			this.txtPageTop = new System.Windows.Forms.TextBox();
			this.lblPageBottom = new System.Windows.Forms.Label();
			this.txtPageBottom = new System.Windows.Forms.TextBox();
			this.lblLineSpace = new System.Windows.Forms.Label();
			this.ddlLeading = new System.Windows.Forms.ComboBox();
			this.ddlPicture = new System.Windows.Forms.ComboBox();
			this.lblPageColumn = new System.Windows.Forms.Label();
			this.ddlPageColumn = new System.Windows.Forms.ComboBox();
			this.ddlVerticalJustify = new System.Windows.Forms.ComboBox();
			this.lblPageGutter = new System.Windows.Forms.Label();
			this.lblVerticalJustify = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtPageGutterWidth = new System.Windows.Forms.TextBox();
			this.lblJustified = new System.Windows.Forms.Label();
			this.ddlJustified = new System.Windows.Forms.ComboBox();
			this.tabMobile = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.mobileIcon = new System.Windows.Forms.PictureBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.ddlLanguage = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.ddlFiles = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.ddlRedLetter = new System.Windows.Forms.ComboBox();
			this.lblMobileOptionsSection = new System.Windows.Forms.Label();
			this.lblGoBibleDescription = new System.Windows.Forms.Label();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.tabOthers = new System.Windows.Forms.TabPage();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblMaxImageWidth = new System.Windows.Forms.Label();
			this.txtMaxImageWidth = new System.Windows.Forms.TextBox();
			this.lblPx = new System.Windows.Forms.Label();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblDefaultAlignment = new System.Windows.Forms.Label();
			this.ddlDefaultAlignment = new System.Windows.Forms.ComboBox();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.lblBaseFontSize = new System.Windows.Forms.Label();
			this.txtBaseFontSize = new System.Windows.Forms.TextBox();			
			this.lblPt = new System.Windows.Forms.Label();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.lblMissingFont = new System.Windows.Forms.Label();
			this.ddlMissingFont = new System.Windows.Forms.ComboBox();
			this.lblNonSILFont = new System.Windows.Forms.Label();
			this.ddlNonSILFont = new System.Windows.Forms.ComboBox();
			this.lblDefaultFont = new System.Windows.Forms.Label();
			this.ddlDefaultFont = new System.Windows.Forms.ComboBox();
			this.chkPageBreaks = new System.Windows.Forms.CheckBox();
			this.chkIncludeImage = new System.Windows.Forms.CheckBox();
			this.ddlReferences = new System.Windows.Forms.ComboBox();
			this.lblReferences = new System.Windows.Forms.Label();
			this.picFonts = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.lblEpubFontsSection = new System.Windows.Forms.Label();
			this.lblEputLayoutSection = new System.Windows.Forms.Label();
			this.chkIncludeFontVariants = new System.Windows.Forms.CheckBox();
			this.lblEpubDescription = new System.Windows.Forms.Label();
			this.chkEmbedFonts = new System.Windows.Forms.CheckBox();
			this.ddlTocLevel = new System.Windows.Forms.ComboBox();
			this.lblTocLevel = new System.Windows.Forms.Label();
			this.lblLineSpacing = new System.Windows.Forms.Label();
			this.ddlChapterNumbers = new System.Windows.Forms.ComboBox();
			this.txtDefaultLineHeight = new System.Windows.Forms.TextBox();
			this.lblChapterNumbers = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tabWeb = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.label17 = new System.Windows.Forms.Label();
			this.txtWebFtpFldrNme = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.txtWebUrl = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.txtWebEmailID = new System.Windows.Forms.TextBox();
			this.txtWebAdminUsrNme = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.txtWebAdminSiteNme = new System.Windows.Forms.TextBox();
			this.txtWebAdminPwd = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.lblTargetFileLocation = new System.Windows.Forms.Label();
			this.txtFtpPassword = new System.Windows.Forms.TextBox();
			this.txtFtpFileLocation = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtFtpUsername = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.label13 = new System.Windows.Forms.Label();
			this.txtSqlPassword = new System.Windows.Forms.TextBox();
			this.txtSqlServerName = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.txtSqlUsername = new System.Windows.Forms.TextBox();
			this.txtSqlDBName = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.tabDict4Mids = new System.Windows.Forms.TabPage();
			this.tabPreview = new System.Windows.Forms.TabPage();
			this.btnPrevious = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.picPreview = new System.Windows.Forms.PictureBox();
			this.tabPicture = new System.Windows.Forms.TabPage();
			this.ChkDontPicture = new System.Windows.Forms.CheckBox();
			this.LblPicPosition = new System.Windows.Forms.Label();
			this.DdlPicPosition = new System.Windows.Forms.ComboBox();
			this.GrpPicture = new System.Windows.Forms.GroupBox();
			this.Lblcm = new System.Windows.Forms.Label();
			this.RadSingleColumn = new System.Windows.Forms.RadioButton();
			this.RadEntirePage = new System.Windows.Forms.RadioButton();
			this.RadWidthIf = new System.Windows.Forms.RadioButton();
			this.RadWidthAll = new System.Windows.Forms.RadioButton();
			this.LblPictureWidth = new System.Windows.Forms.Label();
			this.SpinPicWidth = new System.Windows.Forms.NumericUpDown();
			this.lblGuidewordLength = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblInfoCaption = new System.Windows.Forms.Label();
			this.txtCss = new System.Windows.Forms.TextBox();
			this.TLPanelOuter = new System.Windows.Forms.TableLayoutPanel();
			this.TLPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnScripture = new System.Windows.Forms.Button();
			this.btnDictionary = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnOthers = new System.Windows.Forms.Button();
			this.btnWeb = new System.Windows.Forms.Button();
			this.btnMobile = new System.Windows.Forms.Button();
			this.btnPaper = new System.Windows.Forms.Button();
			this.TLPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.TLPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.lblType = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.l10NSharpExtender1 = new L10NSharp.UI.L10NSharpExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.stylesGrid)).BeginInit();
			this.toolStripMain.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabInfo.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tabDisplay.SuspendLayout();
			this.tblPnlDisplay.SuspendLayout();
			this.pnlOtherFormat.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.pnlGuidewordLength.SuspendLayout();
			this.pnlReferenceFormat.SuspendLayout();
			this.tblPnlRefFormat.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.tabMobile.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mobileIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			this.tabOthers.SuspendLayout();
			this.flowLayoutPanel5.SuspendLayout();
			this.flowLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picFonts)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.tabWeb.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.tabPreview.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
			this.tabPicture.SuspendLayout();
			this.GrpPicture.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SpinPicWidth)).BeginInit();
			this.TLPanelOuter.SuspendLayout();
			this.TLPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.TLPanel2.SuspendLayout();
			this.TLPanel3.SuspendLayout();
			this.panel3.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).BeginInit();
			this.SuspendLayout();
			// 
			// tsNew
			// 
			this.tsNew.AccessibleName = "tsNew";
			this.tsNew.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.tsNew.Image = ((System.Drawing.Image)(resources.GetObject("tsNew.Image")));
			this.tsNew.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.tsNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tsNew, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tsNew, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tsNew, "ConfigurationTool.tsNew");
			this.tsNew.Name = "tsNew";
			this.tsNew.Size = new System.Drawing.Size(36, 49);
			this.tsNew.Text = "&New";
			this.tsNew.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.tsNew.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
			this.tsNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsNew.ToolTipText = "Add a new stylesheet (Alt+N) ";
			this.tsNew.Click += new System.EventHandler(this.tsNew_Click);
			// 
			// stylesGrid
			// 
			this.stylesGrid.AllowUserToAddRows = false;
			this.stylesGrid.AllowUserToDeleteRows = false;
			this.stylesGrid.AllowUserToResizeRows = false;
			this.stylesGrid.BackgroundColor = System.Drawing.SystemColors.Control;
			this.stylesGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.stylesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.stylesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stylesGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.stylesGrid, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.stylesGrid, null);
			this.l10NSharpExtender1.SetLocalizingId(this.stylesGrid, "ConfigurationTool.stylesGrid");
			this.stylesGrid.Location = new System.Drawing.Point(3, 0);
			this.stylesGrid.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.stylesGrid.MultiSelect = false;
			this.stylesGrid.Name = "stylesGrid";
			this.stylesGrid.ReadOnly = true;
			this.stylesGrid.RowHeadersVisible = false;
			this.stylesGrid.RowTemplate.Height = 24;
			this.stylesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.stylesGrid.Size = new System.Drawing.Size(415, 731);
			this.stylesGrid.TabIndex = 0;
			this.stylesGrid.TabStop = false;
			this.stylesGrid.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.stylesGrid_ColumnWidthChanged);
			this.stylesGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.stylesGrid_RowEnter);
			this.stylesGrid.SelectionChanged += new System.EventHandler(this.stylesGrid_SelectionChanged);
			// 
			// toolStripMain
			// 
			this.toolStripMain.AccessibleName = "toolStripMain";
			this.toolStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsNew,
            this.tsSaveAs,
            this.tsDelete,
            this.tsUndo,
            this.tsRedo,
            this.tsPreview,
            this.tsDefault,
            this.tsReset,
            this.tsSend,
            this.toolStripHelpButton});
			this.l10NSharpExtender1.SetLocalizableToolTip(this.toolStripMain, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.toolStripMain, null);
			this.l10NSharpExtender1.SetLocalizingId(this.toolStripMain, "ConfigurationTool.toolStripMain");
			this.toolStripMain.Location = new System.Drawing.Point(0, 0);
			this.toolStripMain.Name = "toolStripMain";
			this.toolStripMain.Size = new System.Drawing.Size(876, 52);
			this.toolStripMain.TabIndex = 0;
			this.toolStripMain.Text = "New";
			this.toolStripMain.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripMain_ItemClicked);
			// 
			// tsSaveAs
			// 
			this.tsSaveAs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.tsSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("tsSaveAs.Image")));
			this.tsSaveAs.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tsSaveAs, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tsSaveAs, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tsSaveAs, "ConfigurationTool.tsSaveAs");
			this.tsSaveAs.Name = "tsSaveAs";
			this.tsSaveAs.Size = new System.Drawing.Size(56, 49);
			this.tsSaveAs.Text = "Save &As";
			this.tsSaveAs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsSaveAs.ToolTipText = "Copy the selected stylesheet into a new stylesheet (Alt+A)";
			this.tsSaveAs.Click += new System.EventHandler(this.tsSaveAs_Click);
			// 
			// tsDelete
			// 
			this.tsDelete.AccessibleName = "tsClose";
			this.tsDelete.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.tsDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsDelete.Image")));
			this.tsDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tsDelete, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tsDelete, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tsDelete, "ConfigurationTool.tsDelete");
			this.tsDelete.Name = "tsDelete";
			this.tsDelete.Size = new System.Drawing.Size(48, 49);
			this.tsDelete.Text = "De&lete";
			this.tsDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsDelete.ToolTipText = "Delete the selected stylesheet (Alt+L)";
			this.tsDelete.Click += new System.EventHandler(this.tsDelete_Click);
			// 
			// tsUndo
			// 
			this.tsUndo.AccessibleName = "tsPreview";
			this.tsUndo.Enabled = false;
			this.tsUndo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.tsUndo.Image = ((System.Drawing.Image)(resources.GetObject("tsUndo.Image")));
			this.tsUndo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tsUndo, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tsUndo, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tsUndo, "ConfigurationTool.tsUndo");
			this.tsUndo.Name = "tsUndo";
			this.tsUndo.Size = new System.Drawing.Size(43, 49);
			this.tsUndo.Text = " &Undo";
			this.tsUndo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsUndo.ToolTipText = "Undo the last change (Alt+U)";
			this.tsUndo.Visible = false;
			// 
			// tsRedo
			// 
			this.tsRedo.AccessibleName = "tsPreview";
			this.tsRedo.Enabled = false;
			this.tsRedo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.tsRedo.Image = ((System.Drawing.Image)(resources.GetObject("tsRedo.Image")));
			this.tsRedo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tsRedo, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tsRedo, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tsRedo, "ConfigurationTool.tsRedo");
			this.tsRedo.Name = "tsRedo";
			this.tsRedo.Size = new System.Drawing.Size(43, 49);
			this.tsRedo.Text = " Redo";
			this.tsRedo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsRedo.ToolTipText = "Redo the last change";
			this.tsRedo.Visible = false;
			// 
			// tsPreview
			// 
			this.tsPreview.AccessibleName = "tsEdit";
			this.tsPreview.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.tsPreview.Image = ((System.Drawing.Image)(resources.GetObject("tsPreview.Image")));
			this.tsPreview.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tsPreview, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tsPreview, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tsPreview, "ConfigurationTool.tsPreview");
			this.tsPreview.Name = "tsPreview";
			this.tsPreview.Size = new System.Drawing.Size(56, 49);
			this.tsPreview.Text = "Pre&view";
			this.tsPreview.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsPreview.ToolTipText = "Preview the layout produced by the selected stylesheet (Alt+V)";
			this.tsPreview.Click += new System.EventHandler(this.tsPreview_Click);
			// 
			// tsDefault
			// 
			this.tsDefault.AccessibleName = "tsHelp";
			this.tsDefault.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.tsDefault.Image = ((System.Drawing.Image)(resources.GetObject("tsDefault.Image")));
			this.tsDefault.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsDefault.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tsDefault, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tsDefault, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tsDefault, "ConfigurationTool.tsDefault");
			this.tsDefault.Name = "tsDefault";
			this.tsDefault.Size = new System.Drawing.Size(58, 49);
			this.tsDefault.Text = "De&faults";
			this.tsDefault.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsDefault.ToolTipText = "Select the Default Settings for the Export Through Pathway dialog (Alt+F)";
			this.tsDefault.Click += new System.EventHandler(this.tsDefault_Click);
			// 
			// tsReset
			// 
			this.tsReset.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tsReset.Image = ((System.Drawing.Image)(resources.GetObject("tsReset.Image")));
			this.tsReset.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsReset.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tsReset, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tsReset, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tsReset, "ConfigurationTool.tsReset");
			this.tsReset.Name = "tsReset";
			this.tsReset.Size = new System.Drawing.Size(44, 49);
			this.tsReset.Text = "&Reset";
			this.tsReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsReset.ToolTipText = "Clear the current settings file (Alt+R)";
			this.tsReset.Click += new System.EventHandler(this.tsReset_Click);
			// 
			// tsSend
			// 
			this.tsSend.AccessibleName = "tsExport";
			this.tsSend.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.tsSend.Image = ((System.Drawing.Image)(resources.GetObject("tsSend.Image")));
			this.tsSend.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsSend.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tsSend, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tsSend, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tsSend, "ConfigurationTool.tsSend");
			this.tsSend.Name = "tsSend";
			this.tsSend.Size = new System.Drawing.Size(39, 49);
			this.tsSend.Text = "S&end";
			this.tsSend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsSend.ToolTipText = "Send the stylesheets and settings to someone else (Alt+E)";
			this.tsSend.Click += new System.EventHandler(this.tsSend_Click);
			// 
			// toolStripHelpButton
			// 
			this.toolStripHelpButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.studentManualToolStripMenuItem,
            this.moreHelpToolStripMenuItem,
            this.userInterfaceToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.toolStripHelpButton.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.toolStripHelpButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripHelpButton.Image")));
			this.toolStripHelpButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStripHelpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.toolStripHelpButton, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.toolStripHelpButton, null);
			this.l10NSharpExtender1.SetLocalizingId(this.toolStripHelpButton, "ConfigurationTool.toolStripHelpButton");
			this.toolStripHelpButton.Name = "toolStripHelpButton";
			this.toolStripHelpButton.Size = new System.Drawing.Size(48, 49);
			this.toolStripHelpButton.Text = "&Help";
			this.toolStripHelpButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.toolStripHelpButton.ToolTipText = "Help (Alt+H)";
			this.toolStripHelpButton.ButtonClick += new System.EventHandler(this.toolStripHelpButton_ButtonClick);
			// 
			// contentsToolStripMenuItem
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.contentsToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.contentsToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizingId(this.contentsToolStripMenuItem, "ConfigurationTool.contentsToolStripMenuItem");
			this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
			this.contentsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.contentsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.contentsToolStripMenuItem.Text = "Help";
			this.contentsToolStripMenuItem.Click += new System.EventHandler(this.contentsToolStripMenuItem_Click);
			// 
			// studentManualToolStripMenuItem
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.studentManualToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.studentManualToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizingId(this.studentManualToolStripMenuItem, "ConfigurationTool.studentManualToolStripMenuItem");
			this.studentManualToolStripMenuItem.Name = "studentManualToolStripMenuItem";
			this.studentManualToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.studentManualToolStripMenuItem.Text = "Student &Manual";
			this.studentManualToolStripMenuItem.Click += new System.EventHandler(this.studentManualToolStripMenuItem_Click);
			// 
			// moreHelpToolStripMenuItem
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.moreHelpToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.moreHelpToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizingId(this.moreHelpToolStripMenuItem, "ConfigurationTool.moreHelpToolStripMenuItem");
			this.moreHelpToolStripMenuItem.Name = "moreHelpToolStripMenuItem";
			this.moreHelpToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.moreHelpToolStripMenuItem.Text = "More &Help";
			this.moreHelpToolStripMenuItem.Click += new System.EventHandler(this.moreHelpToolStripMenuItem_Click);
			// 
			// userInterfaceToolStripMenuItem
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.userInterfaceToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.userInterfaceToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizingId(this.userInterfaceToolStripMenuItem, "ConfigurationTool.userInterfaceToolStripMenuItem");
			this.userInterfaceToolStripMenuItem.Name = "userInterfaceToolStripMenuItem";
			this.userInterfaceToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.userInterfaceToolStripMenuItem.Text = "User Interface";
			this.userInterfaceToolStripMenuItem.Click += new System.EventHandler(this.userInterfaceToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.aboutToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.aboutToolStripMenuItem, null);
			this.l10NSharpExtender1.SetLocalizingId(this.aboutToolStripMenuItem, "ConfigurationTool.aboutToolStripMenuItem");
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.aboutToolStripMenuItem.Text = "&About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabInfo);
			this.tabControl1.Controls.Add(this.tabDisplay);
			this.tabControl1.Controls.Add(this.tabMobile);
			this.tabControl1.Controls.Add(this.tabOthers);
			this.tabControl1.Controls.Add(this.tabWeb);
			this.tabControl1.Controls.Add(this.tabDict4Mids);
			this.tabControl1.Controls.Add(this.tabPreview);
			this.tabControl1.Controls.Add(this.tabPicture);
			this.tabControl1.HotTrack = true;
			this.tabControl1.Location = new System.Drawing.Point(0, 3);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(322, 753);
			this.tabControl1.TabIndex = 0;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabInfo
			// 
			this.tabInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tabInfo.Controls.Add(this.tableLayoutPanel1);
			this.tabInfo.Controls.Add(this.lnkLblUrl);
			this.tabInfo.Controls.Add(this.lblProjectUrl);
			this.tabInfo.Controls.Add(this.btnApproved);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tabInfo, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tabInfo, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tabInfo, "ConfigurationTool.tabInfo");
			this.tabInfo.Location = new System.Drawing.Point(4, 22);
			this.tabInfo.Name = "tabInfo";
			this.tabInfo.Padding = new System.Windows.Forms.Padding(3);
			this.tabInfo.Size = new System.Drawing.Size(314, 727);
			this.tabInfo.TabIndex = 0;
			this.tabInfo.Text = "Info";
			this.tabInfo.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.88525F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.11475F));
			this.tableLayoutPanel1.Controls.Add(this.lblName, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.txtName, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblDesc, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.txtApproved, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.txtDesc, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblApproved, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.lblAvailable, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.chkAvailable, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtComment, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblComment, 0, 3);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(296, 265);
			this.tableLayoutPanel1.TabIndex = 14;
			// 
			// lblName
			// 
			this.lblName.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblName.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblName, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblName, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblName, "ConfigurationTool.lblName");
			this.lblName.Location = new System.Drawing.Point(42, 9);
			this.lblName.Name = "lblName";
			this.lblName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblName.Size = new System.Drawing.Size(35, 13);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name";
			// 
			// txtName
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtName, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtName, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtName, "ConfigurationTool.txtName");
			this.txtName.Location = new System.Drawing.Point(83, 6);
			this.txtName.MaxLength = 50;
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(207, 20);
			this.txtName.TabIndex = 1;
			this.txtName.Enter += new System.EventHandler(this.txtName_Enter);
			this.txtName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyUp);
			this.txtName.Validating += new System.ComponentModel.CancelEventHandler(this.txtName_Validating);
			// 
			// lblDesc
			// 
			this.lblDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblDesc.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblDesc, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblDesc, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblDesc, "ConfigurationTool.lblDesc");
			this.lblDesc.Location = new System.Drawing.Point(17, 29);
			this.lblDesc.Name = "lblDesc";
			this.lblDesc.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblDesc.Size = new System.Drawing.Size(60, 13);
			this.lblDesc.TabIndex = 2;
			this.lblDesc.Text = "Description";
			// 
			// txtApproved
			// 
			this.txtApproved.Enabled = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtApproved, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtApproved, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtApproved, "ConfigurationTool.txtApproved");
			this.txtApproved.Location = new System.Drawing.Point(83, 214);
			this.txtApproved.MaxLength = 10;
			this.txtApproved.Name = "txtApproved";
			this.txtApproved.Size = new System.Drawing.Size(207, 20);
			this.txtApproved.TabIndex = 5;
			this.txtApproved.Enter += new System.EventHandler(this.SetGotFocusValue);
			this.txtApproved.Validated += new System.EventHandler(this.txtApproved_Validated);
			// 
			// txtDesc
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtDesc, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtDesc, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtDesc, "ConfigurationTool.txtDesc");
			this.txtDesc.Location = new System.Drawing.Point(83, 32);
			this.txtDesc.MaxLength = 250;
			this.txtDesc.Multiline = true;
			this.txtDesc.Name = "txtDesc";
			this.txtDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDesc.Size = new System.Drawing.Size(207, 75);
			this.txtDesc.TabIndex = 2;
			this.txtDesc.Enter += new System.EventHandler(this.SetGotFocusValue);
			this.txtDesc.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDesc_KeyUp);
			// 
			// lblApproved
			// 
			this.lblApproved.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblApproved.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblApproved, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblApproved, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblApproved, "ConfigurationTool.lblApproved");
			this.lblApproved.Location = new System.Drawing.Point(9, 211);
			this.lblApproved.Name = "lblApproved";
			this.lblApproved.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblApproved.Size = new System.Drawing.Size(68, 26);
			this.lblApproved.TabIndex = 11;
			this.lblApproved.Text = "Approved By\r\n\r\n";
			// 
			// lblAvailable
			// 
			this.lblAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblAvailable.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblAvailable, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblAvailable, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblAvailable, "ConfigurationTool.lblAvailable");
			this.lblAvailable.Location = new System.Drawing.Point(27, 110);
			this.lblAvailable.Name = "lblAvailable";
			this.lblAvailable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblAvailable.Size = new System.Drawing.Size(50, 13);
			this.lblAvailable.TabIndex = 4;
			this.lblAvailable.Text = "Available";
			// 
			// chkAvailable
			// 
			this.chkAvailable.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkAvailable, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkAvailable, null);
			this.l10NSharpExtender1.SetLocalizingId(this.chkAvailable, "ConfigurationTool.chkAvailable");
			this.chkAvailable.Location = new System.Drawing.Point(83, 113);
			this.chkAvailable.Name = "chkAvailable";
			this.chkAvailable.Size = new System.Drawing.Size(15, 14);
			this.chkAvailable.TabIndex = 3;
			this.chkAvailable.UseVisualStyleBackColor = true;
			this.chkAvailable.CheckedChanged += new System.EventHandler(this.chkAvailable_CheckedChanged);
			this.chkAvailable.CheckStateChanged += new System.EventHandler(this.chkAvailable_CheckStateChanged);
			this.chkAvailable.Enter += new System.EventHandler(this.SetGotFocusValue);
			// 
			// txtComment
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtComment, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtComment, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtComment, "ConfigurationTool.txtComment");
			this.txtComment.Location = new System.Drawing.Point(83, 133);
			this.txtComment.MaxLength = 250;
			this.txtComment.Multiline = true;
			this.txtComment.Name = "txtComment";
			this.txtComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtComment.Size = new System.Drawing.Size(207, 75);
			this.txtComment.TabIndex = 4;
			this.txtComment.Enter += new System.EventHandler(this.SetGotFocusValue);
			this.txtComment.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtComment_KeyUp);
			// 
			// lblComment
			// 
			this.lblComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblComment.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblComment, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblComment, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblComment, "ConfigurationTool.lblComment");
			this.lblComment.Location = new System.Drawing.Point(26, 130);
			this.lblComment.Name = "lblComment";
			this.lblComment.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.lblComment.Size = new System.Drawing.Size(51, 13);
			this.lblComment.TabIndex = 9;
			this.lblComment.Text = "Comment";
			// 
			// lnkLblUrl
			// 
			this.lnkLblUrl.AutoSize = true;
			this.lnkLblUrl.LinkArea = new System.Windows.Forms.LinkArea(0, 22);
			this.lnkLblUrl.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lnkLblUrl, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lnkLblUrl, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lnkLblUrl, "ConfigurationTool.ConfigurationTool.lnkLblUrl");
			this.lnkLblUrl.Location = new System.Drawing.Point(182, 320);
			this.lnkLblUrl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lnkLblUrl.Name = "lnkLblUrl";
			this.lnkLblUrl.Size = new System.Drawing.Size(108, 13);
			this.lnkLblUrl.TabIndex = 13;
			this.lnkLblUrl.TabStop = true;
			this.lnkLblUrl.Text = "http://pathway.sil.org";
			this.lnkLblUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblUrl_LinkClicked);
			// 
			// lblProjectUrl
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblProjectUrl, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblProjectUrl, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblProjectUrl, "ConfigurationTool.lblProjectUrl");
			this.lblProjectUrl.Location = new System.Drawing.Point(6, 294);
			this.lblProjectUrl.Name = "lblProjectUrl";
			this.lblProjectUrl.Size = new System.Drawing.Size(276, 26);
			this.lblProjectUrl.TabIndex = 12;
			this.lblProjectUrl.Text = "Other project related information is available at:";
			// 
			// btnApproved
			// 
			this.btnApproved.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnApproved, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnApproved, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnApproved, "ConfigurationTool.ConfigurationTool.btnApproved");
			this.btnApproved.Location = new System.Drawing.Point(232, 345);
			this.btnApproved.Margin = new System.Windows.Forms.Padding(2);
			this.btnApproved.Name = "btnApproved";
			this.btnApproved.Size = new System.Drawing.Size(43, 25);
			this.btnApproved.TabIndex = 6;
			this.btnApproved.Text = "...";
			this.btnApproved.UseVisualStyleBackColor = true;
			this.btnApproved.Visible = false;
			// 
			// tabDisplay
			// 
			this.tabDisplay.AutoScroll = true;
			this.tabDisplay.Controls.Add(this.tblPnlDisplay);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tabDisplay, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tabDisplay, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tabDisplay, "ConfigurationTool.tabDisplay");
			this.tabDisplay.Location = new System.Drawing.Point(4, 22);
			this.tabDisplay.Name = "tabDisplay";
			this.tabDisplay.Padding = new System.Windows.Forms.Padding(3);
			this.tabDisplay.Size = new System.Drawing.Size(314, 727);
			this.tabDisplay.TabIndex = 1;
			this.tabDisplay.Text = "Properties";
			this.tabDisplay.UseVisualStyleBackColor = true;
			// 
			// tblPnlDisplay
			// 
			this.tblPnlDisplay.ColumnCount = 2;
			this.tblPnlDisplay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.3871F));
			this.tblPnlDisplay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.6129F));
			this.tblPnlDisplay.Controls.Add(this.lblPagePageSize, 0, 0);
			this.tblPnlDisplay.Controls.Add(this.pnlOtherFormat, 0, 13);
			this.tblPnlDisplay.Controls.Add(this.pnlGuidewordLength, 0, 11);
			this.tblPnlDisplay.Controls.Add(this.pnlReferenceFormat, 0, 12);
			this.tblPnlDisplay.Controls.Add(this.chkDisableWO, 1, 6);
			this.tblPnlDisplay.Controls.Add(this.ddlPagePageSize, 1, 0);
			this.tblPnlDisplay.Controls.Add(this.label5, 0, 1);
			this.tblPnlDisplay.Controls.Add(this.ddlRunningHead, 1, 10);
			this.tblPnlDisplay.Controls.Add(this.lblRunningHeader, 0, 10);
			this.tblPnlDisplay.Controls.Add(this.chkFixedLineHeight, 1, 9);
			this.tblPnlDisplay.Controls.Add(this.flowLayoutPanel1, 1, 1);
			this.tblPnlDisplay.Controls.Add(this.lblLineSpace, 0, 8);
			this.tblPnlDisplay.Controls.Add(this.ddlLeading, 1, 8);
			this.tblPnlDisplay.Controls.Add(this.ddlPicture, 1, 7);
			this.tblPnlDisplay.Controls.Add(this.lblPageColumn, 0, 2);
			this.tblPnlDisplay.Controls.Add(this.ddlPageColumn, 1, 2);
			this.tblPnlDisplay.Controls.Add(this.ddlVerticalJustify, 1, 5);
			this.tblPnlDisplay.Controls.Add(this.lblPageGutter, 0, 3);
			this.tblPnlDisplay.Controls.Add(this.lblVerticalJustify, 0, 5);
			this.tblPnlDisplay.Controls.Add(this.label4, 0, 7);
			this.tblPnlDisplay.Controls.Add(this.txtPageGutterWidth, 1, 3);
			this.tblPnlDisplay.Controls.Add(this.lblJustified, 0, 4);
			this.tblPnlDisplay.Controls.Add(this.ddlJustified, 1, 4);
			this.tblPnlDisplay.Location = new System.Drawing.Point(3, 1);
			this.tblPnlDisplay.Name = "tblPnlDisplay";
			this.tblPnlDisplay.RowCount = 14;
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlDisplay.Size = new System.Drawing.Size(301, 716);
			this.tblPnlDisplay.TabIndex = 108;
			// 
			// lblPagePageSize
			// 
			this.lblPagePageSize.AccessibleName = "lblPagePageSize";
			this.lblPagePageSize.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblPagePageSize.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblPagePageSize, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblPagePageSize, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblPagePageSize, "ConfigurationTool.lblPagePageSize");
			this.lblPagePageSize.Location = new System.Drawing.Point(27, 7);
			this.lblPagePageSize.Name = "lblPagePageSize";
			this.lblPagePageSize.Size = new System.Drawing.Size(55, 13);
			this.lblPagePageSize.TabIndex = 33;
			this.lblPagePageSize.Text = "Page Size";
			this.lblPagePageSize.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// pnlOtherFormat
			// 
			this.pnlOtherFormat.AutoSize = true;
			this.tblPnlDisplay.SetColumnSpan(this.pnlOtherFormat, 2);
			this.pnlOtherFormat.Controls.Add(this.tableLayoutPanel2);
			this.pnlOtherFormat.Location = new System.Drawing.Point(3, 496);
			this.pnlOtherFormat.Name = "pnlOtherFormat";
			this.pnlOtherFormat.Size = new System.Drawing.Size(281, 217);
			this.pnlOtherFormat.TabIndex = 103;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.lblPageNumber, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.chkSplitFileByLetter, 1, 7);
			this.tableLayoutPanel2.Controls.Add(this.ddlPageNumber, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.ddlSense, 1, 6);
			this.tableLayoutPanel2.Controls.Add(this.ddlFileProduceDict, 1, 5);
			this.tableLayoutPanel2.Controls.Add(this.lblSenseLayout, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.lblRules, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.lblFileProduceDict, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.ddlRules, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.lblFont, 0, 4);
			this.tableLayoutPanel2.Controls.Add(this.ddlFontSize, 1, 4);
			this.tableLayoutPanel2.Controls.Add(this.chkCenterTitleHeader, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.ddlHeaderFontSize, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.lblHeaderFontSize, 0, 1);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 8;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(278, 214);
			this.tableLayoutPanel2.TabIndex = 110;
			// 
			// lblPageNumber
			// 
			this.lblPageNumber.AccessibleName = "lblPageNumber";
			this.lblPageNumber.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblPageNumber.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblPageNumber, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblPageNumber, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblPageNumber, "ConfigurationTool.lblPageNumber");
			this.lblPageNumber.Location = new System.Drawing.Point(28, 50);
			this.lblPageNumber.Name = "lblPageNumber";
			this.lblPageNumber.Size = new System.Drawing.Size(49, 26);
			this.lblPageNumber.TabIndex = 108;
			this.lblPageNumber.Text = "Page Numbers";
			this.lblPageNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// chkSplitFileByLetter
			// 
			this.chkSplitFileByLetter.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkSplitFileByLetter, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkSplitFileByLetter, null);
			this.l10NSharpExtender1.SetLocalizingId(this.chkSplitFileByLetter, "ConfigurationTool.chkSplitFileByLetter");
			this.chkSplitFileByLetter.Location = new System.Drawing.Point(83, 188);
			this.chkSplitFileByLetter.Name = "chkSplitFileByLetter";
			this.chkSplitFileByLetter.Size = new System.Drawing.Size(109, 17);
			this.chkSplitFileByLetter.TabIndex = 109;
			this.chkSplitFileByLetter.Text = "Split File by Letter";
			this.chkSplitFileByLetter.UseVisualStyleBackColor = true;
			this.chkSplitFileByLetter.CheckStateChanged += new System.EventHandler(this.chkSplitFileByLetter_CheckStateChanged);
			// 
			// ddlPageNumber
			// 
			this.ddlPageNumber.AccessibleName = "ddlPageNumber";
			this.ddlPageNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlPageNumber.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlPageNumber, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlPageNumber, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlPageNumber, "ConfigurationTool.ddlPageNumber");
			this.ddlPageNumber.Location = new System.Drawing.Point(83, 53);
			this.ddlPageNumber.Name = "ddlPageNumber";
			this.ddlPageNumber.Size = new System.Drawing.Size(176, 21);
			this.ddlPageNumber.TabIndex = 99;
			// 
			// ddlSense
			// 
			this.ddlSense.AccessibleName = "ddlPageColumn";
			this.ddlSense.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlSense.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlSense, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlSense, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlSense, "ConfigurationTool.ddlSense");
			this.ddlSense.Location = new System.Drawing.Point(83, 161);
			this.ddlSense.Name = "ddlSense";
			this.ddlSense.Size = new System.Drawing.Size(176, 21);
			this.ddlSense.TabIndex = 103;
			// 
			// ddlFileProduceDict
			// 
			this.ddlFileProduceDict.AccessibleName = "";
			this.ddlFileProduceDict.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlFileProduceDict.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlFileProduceDict, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlFileProduceDict, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlFileProduceDict, "ConfigurationTool.ddlFileProduceDict");
			this.ddlFileProduceDict.Location = new System.Drawing.Point(83, 134);
			this.ddlFileProduceDict.Name = "ddlFileProduceDict";
			this.ddlFileProduceDict.Size = new System.Drawing.Size(176, 21);
			this.ddlFileProduceDict.TabIndex = 102;
			// 
			// lblSenseLayout
			// 
			this.lblSenseLayout.AccessibleName = "lblPageColumn";
			this.lblSenseLayout.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblSenseLayout.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblSenseLayout, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblSenseLayout, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblSenseLayout, "ConfigurationTool.lblSenseLayout");
			this.lblSenseLayout.Location = new System.Drawing.Point(5, 165);
			this.lblSenseLayout.Name = "lblSenseLayout";
			this.lblSenseLayout.Size = new System.Drawing.Size(72, 13);
			this.lblSenseLayout.TabIndex = 106;
			this.lblSenseLayout.Text = "Sense Layout";
			this.lblSenseLayout.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblRules
			// 
			this.lblRules.AccessibleName = "label17";
			this.lblRules.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblRules.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblRules, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblRules, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblRules, "ConfigurationTool.lblRules");
			this.lblRules.Location = new System.Drawing.Point(9, 84);
			this.lblRules.Name = "lblRules";
			this.lblRules.Size = new System.Drawing.Size(68, 13);
			this.lblRules.TabIndex = 104;
			this.lblRules.Text = "Divider Lines";
			this.lblRules.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblFileProduceDict
			// 
			this.lblFileProduceDict.AccessibleName = "lblFileProduceDict";
			this.lblFileProduceDict.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblFileProduceDict.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblFileProduceDict, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblFileProduceDict, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblFileProduceDict, "ConfigurationTool.lblFileProduceDict");
			this.lblFileProduceDict.Location = new System.Drawing.Point(24, 131);
			this.lblFileProduceDict.Name = "lblFileProduceDict";
			this.lblFileProduceDict.Size = new System.Drawing.Size(53, 26);
			this.lblFileProduceDict.TabIndex = 107;
			this.lblFileProduceDict.Text = "Files Produced";
			this.lblFileProduceDict.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ddlRules
			// 
			this.ddlRules.AccessibleName = "ddlPageColumn";
			this.ddlRules.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlRules.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlRules, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlRules, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlRules, "ConfigurationTool.ddlRules");
			this.ddlRules.Location = new System.Drawing.Point(83, 80);
			this.ddlRules.Name = "ddlRules";
			this.ddlRules.Size = new System.Drawing.Size(176, 21);
			this.ddlRules.TabIndex = 100;
			// 
			// lblFont
			// 
			this.lblFont.AccessibleName = "label17";
			this.lblFont.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblFont.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblFont, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblFont, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblFont, "ConfigurationTool.lblFont");
			this.lblFont.Location = new System.Drawing.Point(22, 104);
			this.lblFont.Name = "lblFont";
			this.lblFont.Size = new System.Drawing.Size(55, 26);
			this.lblFont.TabIndex = 105;
			this.lblFont.Text = "Base Font Size";
			this.lblFont.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// ddlFontSize
			// 
			this.ddlFontSize.AccessibleName = "ddlPageColumn";
			this.ddlFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlFontSize.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlFontSize, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlFontSize, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlFontSize, "ConfigurationTool.ddlFontSize");
			this.ddlFontSize.Location = new System.Drawing.Point(83, 107);
			this.ddlFontSize.Name = "ddlFontSize";
			this.ddlFontSize.Size = new System.Drawing.Size(176, 21);
			this.ddlFontSize.TabIndex = 101;
			// 
			// chkCenterTitleHeader
			// 
			this.chkCenterTitleHeader.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkCenterTitleHeader, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkCenterTitleHeader, null);
			this.l10NSharpExtender1.SetLocalizingId(this.chkCenterTitleHeader, "ConfigurationTool.checkBox1");
			this.chkCenterTitleHeader.Location = new System.Drawing.Point(83, 3);
			this.chkCenterTitleHeader.Name = "chkCenterTitleHeader";
			this.chkCenterTitleHeader.Size = new System.Drawing.Size(129, 17);
			this.chkCenterTitleHeader.TabIndex = 110;
			this.chkCenterTitleHeader.Text = "Center Book Title in Header";
			this.toolTip1.SetToolTip(this.chkCenterTitleHeader, "Include and center the Book Title in the Header");
			this.chkCenterTitleHeader.UseVisualStyleBackColor = true;
			this.chkCenterTitleHeader.CheckStateChanged += new System.EventHandler(this.chkCenterTitleHeader_CheckStateChanged);
			// 
			// ddlHeaderFontSize
			// 
			this.ddlHeaderFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlHeaderFontSize.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlHeaderFontSize, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlHeaderFontSize, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlHeaderFontSize, "ConfigurationTool.comboBox1");
			this.ddlHeaderFontSize.Location = new System.Drawing.Point(83, 26);
			this.ddlHeaderFontSize.Name = "ddlHeaderFontSize";
			this.ddlHeaderFontSize.Size = new System.Drawing.Size(121, 21);
			this.ddlHeaderFontSize.TabIndex = 111;
			// 
			// lblHeaderFontSize
			// 
			this.lblHeaderFontSize.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblHeaderFontSize, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblHeaderFontSize, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblHeaderFontSize, "ConfigurationTool.label21");
			this.lblHeaderFontSize.Location = new System.Drawing.Point(3, 23);
			this.lblHeaderFontSize.Name = "lblHeaderFontSize";
			this.lblHeaderFontSize.Size = new System.Drawing.Size(66, 26);
			this.lblHeaderFontSize.TabIndex = 112;
			this.lblHeaderFontSize.Text = "Header Font Size";
			this.lblHeaderFontSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// pnlGuidewordLength
			// 
			this.tblPnlDisplay.SetColumnSpan(this.pnlGuidewordLength, 2);
			this.pnlGuidewordLength.Controls.Add(this.txtGuidewordLength);
			this.pnlGuidewordLength.Controls.Add(this.label9);
			this.pnlGuidewordLength.Location = new System.Drawing.Point(3, 327);
			this.pnlGuidewordLength.Name = "pnlGuidewordLength";
			this.pnlGuidewordLength.Size = new System.Drawing.Size(281, 24);
			this.pnlGuidewordLength.TabIndex = 106;
			// 
			// txtGuidewordLength
			// 
			this.txtGuidewordLength.AccessibleName = "txtPageGutterWidth";
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtGuidewordLength, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtGuidewordLength, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtGuidewordLength, "ConfigurationTool.txtGuidewordLength");
			this.txtGuidewordLength.Location = new System.Drawing.Point(107, 1);
			this.txtGuidewordLength.MaxLength = 2;
			this.txtGuidewordLength.Name = "txtGuidewordLength";
			this.txtGuidewordLength.Size = new System.Drawing.Size(44, 20);
			this.txtGuidewordLength.TabIndex = 106;
			this.txtGuidewordLength.Tag = "Gutter Width";
			// 
			// label9
			// 
			this.label9.AccessibleName = "lblGuidewordLength";
			this.label9.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label9, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label9, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label9, "ConfigurationTool.label9");
			this.label9.Location = new System.Drawing.Point(3, 4);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(94, 13);
			this.label9.TabIndex = 107;
			this.label9.Text = "Guideword Length";
			this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// pnlReferenceFormat
			// 
			this.pnlReferenceFormat.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tblPnlDisplay.SetColumnSpan(this.pnlReferenceFormat, 2);
			this.pnlReferenceFormat.Controls.Add(this.tblPnlRefFormat);
			this.pnlReferenceFormat.Controls.Add(this.flowLayoutPanel6);
			this.pnlReferenceFormat.Controls.Add(this.ddlReferenceFormat);
			this.pnlReferenceFormat.Controls.Add(this.lblReferenceFormat);
			this.pnlReferenceFormat.Location = new System.Drawing.Point(3, 357);
			this.pnlReferenceFormat.Name = "pnlReferenceFormat";
			this.pnlReferenceFormat.Size = new System.Drawing.Size(281, 133);
			this.pnlReferenceFormat.TabIndex = 102;
			// 
			// tblPnlRefFormat
			// 
			this.tblPnlRefFormat.ColumnCount = 2;
			this.tblPnlRefFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.56934F));
			this.tblPnlRefFormat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.43066F));
			this.tblPnlRefFormat.Controls.Add(this.chkHideSpaceVerseNo, 0, 3);
			this.tblPnlRefFormat.Controls.Add(this.txtFnCallerSymbol, 1, 1);
			this.tblPnlRefFormat.Controls.Add(this.chkTurnOffFirstVerse, 0, 2);
			this.tblPnlRefFormat.Controls.Add(this.txtXrefCusSymbol, 1, 0);
			this.tblPnlRefFormat.Controls.Add(this.chkIncludeCusFnCaller, 0, 1);
			this.tblPnlRefFormat.Controls.Add(this.chkXrefCusSymbol, 0, 0);
			this.tblPnlRefFormat.Location = new System.Drawing.Point(81, 32);
			this.tblPnlRefFormat.Name = "tblPnlRefFormat";
			this.tblPnlRefFormat.RowCount = 4;
			this.tblPnlRefFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlRefFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlRefFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlRefFormat.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblPnlRefFormat.Size = new System.Drawing.Size(274, 98);
			this.tblPnlRefFormat.TabIndex = 116;
			// 
			// chkHideSpaceVerseNo
			// 
			this.tblPnlRefFormat.SetColumnSpan(this.chkHideSpaceVerseNo, 2);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkHideSpaceVerseNo, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkHideSpaceVerseNo, null);
			this.l10NSharpExtender1.SetLocalizingId(this.chkHideSpaceVerseNo, "ConfigurationTool.chkHideSpaceVerseNo");
			this.chkHideSpaceVerseNo.Location = new System.Drawing.Point(3, 78);
			this.chkHideSpaceVerseNo.Name = "chkHideSpaceVerseNo";
			this.chkHideSpaceVerseNo.Size = new System.Drawing.Size(197, 17);
			this.chkHideSpaceVerseNo.TabIndex = 111;
			this.chkHideSpaceVerseNo.Text = "Remove Space After Verse No.";
			this.chkHideSpaceVerseNo.UseVisualStyleBackColor = true;
			this.chkHideSpaceVerseNo.CheckStateChanged += new System.EventHandler(this.chkHideSpaceVerseNo_CheckStateChanged);
			// 
			// txtFnCallerSymbol
			// 
			this.txtFnCallerSymbol.AccessibleName = "txtPageGutterWidth";
			this.txtFnCallerSymbol.Enabled = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtFnCallerSymbol, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtFnCallerSymbol, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtFnCallerSymbol, "ConfigurationTool.txtFnCallerSymbol");
			this.txtFnCallerSymbol.Location = new System.Drawing.Point(157, 29);
			this.txtFnCallerSymbol.MaxLength = 1;
			this.txtFnCallerSymbol.Name = "txtFnCallerSymbol";
			this.txtFnCallerSymbol.Size = new System.Drawing.Size(41, 20);
			this.txtFnCallerSymbol.TabIndex = 106;
			this.txtFnCallerSymbol.Tag = "Gutter Width";
			this.txtFnCallerSymbol.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtFnCallerSymbol_KeyUp);
			// 
			// chkTurnOffFirstVerse
			// 
			this.chkTurnOffFirstVerse.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkTurnOffFirstVerse, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkTurnOffFirstVerse, null);
			this.l10NSharpExtender1.SetLocalizingId(this.chkTurnOffFirstVerse, "ConfigurationTool.chkTurnOffFirstVerse");
			this.chkTurnOffFirstVerse.Location = new System.Drawing.Point(3, 55);
			this.chkTurnOffFirstVerse.Name = "chkTurnOffFirstVerse";
			this.chkTurnOffFirstVerse.Size = new System.Drawing.Size(107, 17);
			this.chkTurnOffFirstVerse.TabIndex = 107;
			this.chkTurnOffFirstVerse.Text = "Hide Verse No. 1";
			this.chkTurnOffFirstVerse.UseVisualStyleBackColor = true;
			this.chkTurnOffFirstVerse.CheckStateChanged += new System.EventHandler(this.chkTurnOffFirstVerse_CheckStateChanged);
			// 
			// txtXrefCusSymbol
			// 
			this.txtXrefCusSymbol.Enabled = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtXrefCusSymbol, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtXrefCusSymbol, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtXrefCusSymbol, "ConfigurationTool.txtXrefCusSymbol");
			this.txtXrefCusSymbol.Location = new System.Drawing.Point(157, 3);
			this.txtXrefCusSymbol.Name = "txtXrefCusSymbol";
			this.txtXrefCusSymbol.Size = new System.Drawing.Size(41, 20);
			this.txtXrefCusSymbol.TabIndex = 110;
			this.txtXrefCusSymbol.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtXrefCusSymbol_KeyUp);
			// 
			// chkIncludeCusFnCaller
			// 
			this.chkIncludeCusFnCaller.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkIncludeCusFnCaller, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkIncludeCusFnCaller, null);
			this.l10NSharpExtender1.SetLocalizingId(this.chkIncludeCusFnCaller, "ConfigurationTool.chkIncludeCusFnCaller");
			this.chkIncludeCusFnCaller.Location = new System.Drawing.Point(3, 29);
			this.chkIncludeCusFnCaller.Name = "chkIncludeCusFnCaller";
			this.chkIncludeCusFnCaller.Size = new System.Drawing.Size(135, 17);
			this.chkIncludeCusFnCaller.TabIndex = 105;
			this.chkIncludeCusFnCaller.Text = "Include Footnote Caller";
			this.chkIncludeCusFnCaller.UseVisualStyleBackColor = true;
			this.chkIncludeCusFnCaller.CheckStateChanged += new System.EventHandler(this.chkIncludeCusFnCaller_CheckStateChanged);
			// 
			// chkXrefCusSymbol
			// 
			this.chkXrefCusSymbol.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkXrefCusSymbol, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkXrefCusSymbol, null);
			this.l10NSharpExtender1.SetLocalizingId(this.chkXrefCusSymbol, "ConfigurationTool.chkXrefCusSymbol");
			this.chkXrefCusSymbol.Location = new System.Drawing.Point(3, 3);
			this.chkXrefCusSymbol.Name = "chkXrefCusSymbol";
			this.chkXrefCusSymbol.Size = new System.Drawing.Size(117, 17);
			this.chkXrefCusSymbol.TabIndex = 109;
			this.chkXrefCusSymbol.Text = "Include XRef Caller";
			this.chkXrefCusSymbol.UseVisualStyleBackColor = true;
			this.chkXrefCusSymbol.CheckStateChanged += new System.EventHandler(this.chkXrefCusSymbol_CheckStateChanged);
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.AutoSize = true;
			this.flowLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel6.Location = new System.Drawing.Point(84, 176);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(0, 0);
			this.flowLayoutPanel6.TabIndex = 112;
			// 
			// ddlReferenceFormat
			// 
			this.ddlReferenceFormat.AccessibleName = "ddlReferenceFormat";
			this.ddlReferenceFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlReferenceFormat.DropDownWidth = 177;
			this.ddlReferenceFormat.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlReferenceFormat, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlReferenceFormat, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlReferenceFormat, "ConfigurationTool.ddlReferenceFormat");
			this.ddlReferenceFormat.Location = new System.Drawing.Point(81, 5);
			this.ddlReferenceFormat.Name = "ddlReferenceFormat";
			this.ddlReferenceFormat.Size = new System.Drawing.Size(193, 21);
			this.ddlReferenceFormat.TabIndex = 99;
			// 
			// lblReferenceFormat
			// 
			this.lblReferenceFormat.AccessibleName = "lblReferenceFormat";
			this.lblReferenceFormat.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblReferenceFormat, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblReferenceFormat, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblReferenceFormat, "ConfigurationTool.lblReferenceFormat");
			this.lblReferenceFormat.Location = new System.Drawing.Point(1, 1);
			this.lblReferenceFormat.Name = "lblReferenceFormat";
			this.lblReferenceFormat.Size = new System.Drawing.Size(74, 30);
			this.lblReferenceFormat.TabIndex = 100;
			this.lblReferenceFormat.Text = "Reference Format";
			this.lblReferenceFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chkDisableWO
			// 
			this.chkDisableWO.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkDisableWO, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkDisableWO, null);
			this.l10NSharpExtender1.SetLocalizingId(this.chkDisableWO, "ConfigurationTool.chkDisableWO");
			this.chkDisableWO.Location = new System.Drawing.Point(88, 200);
			this.chkDisableWO.Name = "chkDisableWO";
			this.chkDisableWO.Size = new System.Drawing.Size(139, 17);
			this.chkDisableWO.TabIndex = 107;
			this.chkDisableWO.Text = "Disable widow && orphan";
			this.chkDisableWO.UseVisualStyleBackColor = true;
			this.chkDisableWO.CheckStateChanged += new System.EventHandler(this.chkDisableWO_CheckStateChanged);
			// 
			// ddlPagePageSize
			// 
			this.ddlPagePageSize.AccessibleName = "ddlPagePageSize";
			this.ddlPagePageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlPagePageSize.DropDownWidth = 177;
			this.ddlPagePageSize.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlPagePageSize, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlPagePageSize, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlPagePageSize, "ConfigurationTool.ddlPagePageSize");
			this.ddlPagePageSize.Location = new System.Drawing.Point(88, 3);
			this.ddlPagePageSize.Name = "ddlPagePageSize";
			this.ddlPagePageSize.Size = new System.Drawing.Size(195, 21);
			this.ddlPagePageSize.TabIndex = 0;
			this.ddlPagePageSize.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
			this.ddlPagePageSize.Enter += new System.EventHandler(this.SetGotFocusValue);
			// 
			// label5
			// 
			this.label5.AccessibleName = "lblPagePageSize";
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label5, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label5, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label5, "ConfigurationTool.label5");
			this.label5.Location = new System.Drawing.Point(15, 27);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(67, 13);
			this.label5.TabIndex = 78;
			this.label5.Text = "Page Margin";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// ddlRunningHead
			// 
			this.ddlRunningHead.AccessibleName = "ddlPageColumn";
			this.ddlRunningHead.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlRunningHead.DropDownWidth = 177;
			this.ddlRunningHead.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlRunningHead, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlRunningHead, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlRunningHead, "ConfigurationTool.ddlRunningHead");
			this.ddlRunningHead.Location = new System.Drawing.Point(88, 300);
			this.ddlRunningHead.Name = "ddlRunningHead";
			this.ddlRunningHead.Size = new System.Drawing.Size(195, 21);
			this.ddlRunningHead.TabIndex = 12;
			this.ddlRunningHead.SelectedIndexChanged += new System.EventHandler(this.ddlRunningHead_SelectedIndexChanged);
			// 
			// lblRunningHeader
			// 
			this.lblRunningHeader.AccessibleName = "label17";
			this.lblRunningHeader.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblRunningHeader.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblRunningHeader, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblRunningHeader, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblRunningHeader, "ConfigurationTool.lblRunningHeader");
			this.lblRunningHeader.Location = new System.Drawing.Point(35, 297);
			this.lblRunningHeader.Name = "lblRunningHeader";
			this.lblRunningHeader.Size = new System.Drawing.Size(47, 26);
			this.lblRunningHeader.TabIndex = 87;
			this.lblRunningHeader.Text = "Running Header";
			this.lblRunningHeader.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// chkFixedLineHeight
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkFixedLineHeight, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkFixedLineHeight, null);
			this.l10NSharpExtender1.SetLocalizingId(this.chkFixedLineHeight, "ConfigurationTool.chkFixedLineHeight");
			this.chkFixedLineHeight.Location = new System.Drawing.Point(88, 277);
			this.chkFixedLineHeight.Name = "chkFixedLineHeight";
			this.chkFixedLineHeight.Size = new System.Drawing.Size(130, 17);
			this.chkFixedLineHeight.TabIndex = 11;
			this.chkFixedLineHeight.Text = "Fixed Line Height   ";
			this.chkFixedLineHeight.UseVisualStyleBackColor = true;
			this.chkFixedLineHeight.CheckStateChanged += new System.EventHandler(this.chkFixedLineHeight_CheckStateChanged);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.lblPageInside);
			this.flowLayoutPanel1.Controls.Add(this.txtPageInside);
			this.flowLayoutPanel1.Controls.Add(this.lblPageOutside);
			this.flowLayoutPanel1.Controls.Add(this.txtPageOutside);
			this.flowLayoutPanel1.Controls.Add(this.lblPageTop);
			this.flowLayoutPanel1.Controls.Add(this.txtPageTop);
			this.flowLayoutPanel1.Controls.Add(this.lblPageBottom);
			this.flowLayoutPanel1.Controls.Add(this.txtPageBottom);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(88, 30);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 57);
			this.flowLayoutPanel1.TabIndex = 79;
			// 
			// lblPageInside
			// 
			this.lblPageInside.AccessibleName = "lblPageInside";
			this.lblPageInside.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblPageInside, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblPageInside, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblPageInside, "ConfigurationTool.lblPageInside");
			this.lblPageInside.Location = new System.Drawing.Point(3, 4);
			this.lblPageInside.Name = "lblPageInside";
			this.lblPageInside.Size = new System.Drawing.Size(50, 18);
			this.lblPageInside.TabIndex = 77;
			this.lblPageInside.Text = "Inside";
			this.lblPageInside.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtPageInside
			// 
			this.txtPageInside.AccessibleName = "txtPageInside";
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtPageInside, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtPageInside, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtPageInside, "ConfigurationTool.txtPageInside");
			this.txtPageInside.Location = new System.Drawing.Point(59, 3);
			this.txtPageInside.MaxLength = 6;
			this.txtPageInside.Name = "txtPageInside";
			this.txtPageInside.Size = new System.Drawing.Size(33, 20);
			this.txtPageInside.TabIndex = 1;
			this.txtPageInside.Tag = "Inside";
			this.txtPageInside.Enter += new System.EventHandler(this.SetGotFocusValue);
			this.txtPageInside.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPageInside_KeyUp);
			this.txtPageInside.Validated += new System.EventHandler(this.txtPageInside_Validated);
			// 
			// lblPageOutside
			// 
			this.lblPageOutside.AccessibleName = "lblPageOutside";
			this.lblPageOutside.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblPageOutside, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblPageOutside, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblPageOutside, "ConfigurationTool.lblPageOutside");
			this.lblPageOutside.Location = new System.Drawing.Point(98, 4);
			this.lblPageOutside.Name = "lblPageOutside";
			this.lblPageOutside.Size = new System.Drawing.Size(57, 18);
			this.lblPageOutside.TabIndex = 75;
			this.lblPageOutside.Text = "Outside";
			this.lblPageOutside.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtPageOutside
			// 
			this.txtPageOutside.AccessibleName = "txtPageOutside";
			this.txtPageOutside.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtPageOutside, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtPageOutside, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtPageOutside, "ConfigurationTool.txtPageOutside");
			this.txtPageOutside.Location = new System.Drawing.Point(161, 3);
			this.txtPageOutside.MaxLength = 6;
			this.txtPageOutside.Name = "txtPageOutside";
			this.txtPageOutside.Size = new System.Drawing.Size(33, 20);
			this.txtPageOutside.TabIndex = 2;
			this.txtPageOutside.Tag = "Outside";
			this.txtPageOutside.Enter += new System.EventHandler(this.SetGotFocusValue);
			this.txtPageOutside.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPageOutside_KeyUp);
			this.txtPageOutside.Validated += new System.EventHandler(this.txtPageOutside_Validated);
			// 
			// lblPageTop
			// 
			this.lblPageTop.AccessibleName = "lblPageTop";
			this.lblPageTop.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblPageTop, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblPageTop, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblPageTop, "ConfigurationTool.lblPageTop");
			this.lblPageTop.Location = new System.Drawing.Point(3, 32);
			this.lblPageTop.Name = "lblPageTop";
			this.lblPageTop.Size = new System.Drawing.Size(50, 13);
			this.lblPageTop.TabIndex = 71;
			this.lblPageTop.Text = "Top";
			this.lblPageTop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtPageTop
			// 
			this.txtPageTop.AccessibleName = "txtPageTop";
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtPageTop, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtPageTop, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtPageTop, "ConfigurationTool.txtPageTop");
			this.txtPageTop.Location = new System.Drawing.Point(59, 29);
			this.txtPageTop.MaxLength = 6;
			this.txtPageTop.Name = "txtPageTop";
			this.txtPageTop.Size = new System.Drawing.Size(33, 20);
			this.txtPageTop.TabIndex = 3;
			this.txtPageTop.Tag = "Top";
			this.txtPageTop.Enter += new System.EventHandler(this.SetGotFocusValue);
			this.txtPageTop.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPageTop_KeyUp);
			this.txtPageTop.Validated += new System.EventHandler(this.txtPageTop_Validated);
			// 
			// lblPageBottom
			// 
			this.lblPageBottom.AccessibleName = "lblPageBottom";
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblPageBottom, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblPageBottom, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblPageBottom, "ConfigurationTool.lblPageBottom");
			this.lblPageBottom.Location = new System.Drawing.Point(98, 26);
			this.lblPageBottom.Name = "lblPageBottom";
			this.lblPageBottom.Size = new System.Drawing.Size(57, 18);
			this.lblPageBottom.TabIndex = 73;
			this.lblPageBottom.Text = "Bottom";
			this.lblPageBottom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtPageBottom
			// 
			this.txtPageBottom.AccessibleName = "txtPageBottom";
			this.txtPageBottom.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtPageBottom, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtPageBottom, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtPageBottom, "ConfigurationTool.txtPageBottom");
			this.txtPageBottom.Location = new System.Drawing.Point(161, 29);
			this.txtPageBottom.MaxLength = 6;
			this.txtPageBottom.Name = "txtPageBottom";
			this.txtPageBottom.Size = new System.Drawing.Size(33, 20);
			this.txtPageBottom.TabIndex = 4;
			this.txtPageBottom.Tag = "Bottom";
			this.txtPageBottom.Enter += new System.EventHandler(this.SetGotFocusValue);
			this.txtPageBottom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPageBottom_KeyUp);
			this.txtPageBottom.Validated += new System.EventHandler(this.txtPageBottom_Validated);
			// 
			// lblLineSpace
			// 
			this.lblLineSpace.AccessibleName = "label26";
			this.lblLineSpace.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblLineSpace.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblLineSpace, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblLineSpace, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblLineSpace, "ConfigurationTool.lblLineSpace");
			this.lblLineSpace.Location = new System.Drawing.Point(10, 254);
			this.lblLineSpace.Name = "lblLineSpace";
			this.lblLineSpace.Size = new System.Drawing.Size(72, 13);
			this.lblLineSpace.TabIndex = 86;
			this.lblLineSpace.Text = "Line Spacing ";
			this.lblLineSpace.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// ddlLeading
			// 
			this.ddlLeading.AccessibleName = "ddlPageColumn";
			this.ddlLeading.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlLeading.DropDownWidth = 177;
			this.ddlLeading.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlLeading, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlLeading, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlLeading, "ConfigurationTool.ddlLeading");
			this.ddlLeading.Location = new System.Drawing.Point(88, 250);
			this.ddlLeading.Name = "ddlLeading";
			this.ddlLeading.Size = new System.Drawing.Size(195, 21);
			this.ddlLeading.TabIndex = 10;
			this.ddlLeading.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
			// 
			// ddlPicture
			// 
			this.ddlPicture.AccessibleName = "ddlPageColumn";
			this.ddlPicture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlPicture.DropDownWidth = 177;
			this.ddlPicture.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlPicture, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlPicture, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlPicture, "ConfigurationTool.ddlPicture");
			this.ddlPicture.Location = new System.Drawing.Point(88, 223);
			this.ddlPicture.Name = "ddlPicture";
			this.ddlPicture.Size = new System.Drawing.Size(195, 21);
			this.ddlPicture.TabIndex = 9;
			this.ddlPicture.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
			// 
			// lblPageColumn
			// 
			this.lblPageColumn.AccessibleName = "lblPageColumn";
			this.lblPageColumn.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblPageColumn.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblPageColumn, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblPageColumn, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblPageColumn, "ConfigurationTool.lblPageColumn");
			this.lblPageColumn.Location = new System.Drawing.Point(35, 97);
			this.lblPageColumn.Name = "lblPageColumn";
			this.lblPageColumn.Size = new System.Drawing.Size(47, 13);
			this.lblPageColumn.TabIndex = 42;
			this.lblPageColumn.Text = "Columns";
			this.lblPageColumn.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// ddlPageColumn
			// 
			this.ddlPageColumn.AccessibleName = "ddlPageColumn";
			this.ddlPageColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlPageColumn.DropDownWidth = 177;
			this.ddlPageColumn.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlPageColumn, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlPageColumn, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlPageColumn, "ConfigurationTool.ddlPageColumn");
			this.ddlPageColumn.Location = new System.Drawing.Point(88, 93);
			this.ddlPageColumn.Name = "ddlPageColumn";
			this.ddlPageColumn.Size = new System.Drawing.Size(195, 21);
			this.ddlPageColumn.TabIndex = 5;
			this.ddlPageColumn.SelectedIndexChanged += new System.EventHandler(this.ddlPageColumn_SelectedIndexChanged);
			this.ddlPageColumn.Enter += new System.EventHandler(this.SetGotFocusValue);
			// 
			// ddlVerticalJustify
			// 
			this.ddlVerticalJustify.AccessibleName = "ddlPageColumn";
			this.ddlVerticalJustify.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlVerticalJustify.DropDownWidth = 177;
			this.ddlVerticalJustify.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlVerticalJustify, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlVerticalJustify, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlVerticalJustify, "ConfigurationTool.ddlVerticalJustify");
			this.ddlVerticalJustify.Location = new System.Drawing.Point(88, 173);
			this.ddlVerticalJustify.Name = "ddlVerticalJustify";
			this.ddlVerticalJustify.Size = new System.Drawing.Size(195, 21);
			this.ddlVerticalJustify.TabIndex = 8;
			this.ddlVerticalJustify.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
			// 
			// lblPageGutter
			// 
			this.lblPageGutter.AccessibleName = "lblPageGutter";
			this.lblPageGutter.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblPageGutter.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblPageGutter, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblPageGutter, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblPageGutter, "ConfigurationTool.lblPageGutter");
			this.lblPageGutter.Location = new System.Drawing.Point(17, 123);
			this.lblPageGutter.Name = "lblPageGutter";
			this.lblPageGutter.Size = new System.Drawing.Size(65, 13);
			this.lblPageGutter.TabIndex = 79;
			this.lblPageGutter.Text = "Column Gap";
			this.lblPageGutter.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblVerticalJustify
			// 
			this.lblVerticalJustify.AccessibleName = "lblPageColumn";
			this.lblVerticalJustify.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblVerticalJustify.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblVerticalJustify, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblVerticalJustify, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblVerticalJustify, "ConfigurationTool.lblVerticalJustify");
			this.lblVerticalJustify.Location = new System.Drawing.Point(8, 177);
			this.lblVerticalJustify.Name = "lblVerticalJustify";
			this.lblVerticalJustify.Size = new System.Drawing.Size(74, 13);
			this.lblVerticalJustify.TabIndex = 94;
			this.lblVerticalJustify.Text = "Vertical Justify";
			this.lblVerticalJustify.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.AccessibleName = "lblPageColumn";
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label4.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label4, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label4, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label4, "ConfigurationTool.label4");
			this.label4.Location = new System.Drawing.Point(37, 227);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(45, 13);
			this.label4.TabIndex = 90;
			this.label4.Text = "Pictures";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// txtPageGutterWidth
			// 
			this.txtPageGutterWidth.AccessibleName = "txtPageGutterWidth";
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtPageGutterWidth, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtPageGutterWidth, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtPageGutterWidth, "ConfigurationTool.txtPageGutterWidth");
			this.txtPageGutterWidth.Location = new System.Drawing.Point(88, 120);
			this.txtPageGutterWidth.MaxLength = 6;
			this.txtPageGutterWidth.Name = "txtPageGutterWidth";
			this.txtPageGutterWidth.Size = new System.Drawing.Size(44, 20);
			this.txtPageGutterWidth.TabIndex = 6;
			this.txtPageGutterWidth.Tag = "Gutter Width";
			this.txtPageGutterWidth.Enter += new System.EventHandler(this.SetGotFocusValue);
			this.txtPageGutterWidth.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPageGutterWidth_KeyUp);
			this.txtPageGutterWidth.Validated += new System.EventHandler(this.txtPageGutterWidth_Validated);
			// 
			// lblJustified
			// 
			this.lblJustified.AccessibleName = "lblPageColumn";
			this.lblJustified.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.lblJustified.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblJustified, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblJustified, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblJustified, "ConfigurationTool.lblJustified");
			this.lblJustified.Location = new System.Drawing.Point(37, 150);
			this.lblJustified.Name = "lblJustified";
			this.lblJustified.Size = new System.Drawing.Size(45, 13);
			this.lblJustified.TabIndex = 64;
			this.lblJustified.Text = "Justified";
			this.lblJustified.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// ddlJustified
			// 
			this.ddlJustified.AccessibleName = "ddlPageColumn";
			this.ddlJustified.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlJustified.DropDownWidth = 177;
			this.ddlJustified.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlJustified, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlJustified, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlJustified, "ConfigurationTool.ddlJustified");
			this.ddlJustified.Location = new System.Drawing.Point(88, 146);
			this.ddlJustified.Name = "ddlJustified";
			this.ddlJustified.Size = new System.Drawing.Size(195, 21);
			this.ddlJustified.TabIndex = 7;
			this.ddlJustified.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
			this.ddlJustified.Enter += new System.EventHandler(this.SetGotFocusValue);
			// 
			// tabMobile
			// 
			this.tabMobile.AutoScroll = true;
			this.tabMobile.Controls.Add(this.tableLayoutPanel3);
			this.tabMobile.Controls.Add(this.lblMobileOptionsSection);
			this.tabMobile.Controls.Add(this.lblGoBibleDescription);
			this.tabMobile.Controls.Add(this.pictureBox4);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tabMobile, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tabMobile, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tabMobile, "ConfigurationTool.tabMobile");
			this.tabMobile.Location = new System.Drawing.Point(4, 22);
			this.tabMobile.Name = "tabMobile";
			this.tabMobile.Padding = new System.Windows.Forms.Padding(3);
			this.tabMobile.Size = new System.Drawing.Size(314, 727);
			this.tabMobile.TabIndex = 2;
			this.tabMobile.Text = "Properties";
			this.tabMobile.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.12621F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.87379F));
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.ddlLanguage, 1, 3);
			this.tableLayoutPanel3.Controls.Add(this.label8, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.label20, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.ddlFiles, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.label7, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.ddlRedLetter, 1, 1);
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 78);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 4;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(309, 122);
			this.tableLayoutPanel3.TabIndex = 71;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel3.SetColumnSpan(this.flowLayoutPanel2, 2);
			this.flowLayoutPanel2.Controls.Add(this.label1);
			this.flowLayoutPanel2.Controls.Add(this.mobileIcon);
			this.flowLayoutPanel2.Controls.Add(this.btnBrowse);
			this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 57);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(183, 29);
			this.flowLayoutPanel2.TabIndex = 72;
			// 
			// label1
			// 
			this.label1.AccessibleName = "lblPageColumn";
			this.label1.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label1, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label1, "ConfigurationTool.label1");
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(77, 13);
			this.label1.TabIndex = 9;
			this.label1.Text = "Icon for Phone";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// mobileIcon
			// 
			this.mobileIcon.Image = ((System.Drawing.Image)(resources.GetObject("mobileIcon.Image")));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.mobileIcon, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.mobileIcon, null);
			this.l10NSharpExtender1.SetLocalizingId(this.mobileIcon, "ConfigurationTool.mobileIcon");
			this.mobileIcon.Location = new System.Drawing.Point(86, 3);
			this.mobileIcon.Name = "mobileIcon";
			this.mobileIcon.Size = new System.Drawing.Size(20, 20);
			this.mobileIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.mobileIcon.TabIndex = 65;
			this.mobileIcon.TabStop = false;
			// 
			// btnBrowse
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnBrowse, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnBrowse, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnBrowse, "ConfigurationTool.btnBrowse");
			this.btnBrowse.Location = new System.Drawing.Point(112, 3);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(68, 23);
			this.btnBrowse.TabIndex = 10;
			this.btnBrowse.Text = "Browse...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// ddlLanguage
			// 
			this.ddlLanguage.AccessibleName = "";
			this.ddlLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlLanguage.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlLanguage, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlLanguage, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlLanguage, "ConfigurationTool.ddlLanguage");
			this.ddlLanguage.Location = new System.Drawing.Point(92, 92);
			this.ddlLanguage.Name = "ddlLanguage";
			this.ddlLanguage.Size = new System.Drawing.Size(214, 21);
			this.ddlLanguage.TabIndex = 70;
			this.ddlLanguage.SelectedIndexChanged += new System.EventHandler(this.ddlLanguage_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.AccessibleName = "lblPageColumn";
			this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label8, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label8, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label8, "ConfigurationTool.label8");
			this.label8.Location = new System.Drawing.Point(3, 7);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(83, 13);
			this.label8.TabIndex = 1;
			this.label8.Text = "Files Produced";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label20
			// 
			this.label20.AccessibleName = "lblLanguage";
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label20, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label20, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label20, "ConfigurationTool.label20");
			this.label20.Location = new System.Drawing.Point(3, 89);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(83, 15);
			this.label20.TabIndex = 69;
			this.label20.Text = "Language";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ddlFiles
			// 
			this.ddlFiles.AccessibleName = "";
			this.ddlFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlFiles.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlFiles, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlFiles, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlFiles, "ConfigurationTool.ddlFiles");
			this.ddlFiles.Location = new System.Drawing.Point(92, 3);
			this.ddlFiles.Name = "ddlFiles";
			this.ddlFiles.Size = new System.Drawing.Size(214, 21);
			this.ddlFiles.TabIndex = 2;
			this.ddlFiles.SelectedIndexChanged += new System.EventHandler(this.ddlFiles_SelectedIndexChanged);
			// 
			// label7
			// 
			this.label7.AccessibleName = "lblPageColumn";
			this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label7, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label7, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label7, "ConfigurationTool.label7");
			this.label7.Location = new System.Drawing.Point(3, 34);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(83, 13);
			this.label7.TabIndex = 3;
			this.label7.Text = "Red Letter";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ddlRedLetter
			// 
			this.ddlRedLetter.AccessibleName = "";
			this.ddlRedLetter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlRedLetter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlRedLetter.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlRedLetter, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlRedLetter, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlRedLetter, "ConfigurationTool.ddlRedLetter");
			this.ddlRedLetter.Location = new System.Drawing.Point(92, 30);
			this.ddlRedLetter.Name = "ddlRedLetter";
			this.ddlRedLetter.Size = new System.Drawing.Size(214, 21);
			this.ddlRedLetter.TabIndex = 4;
			this.ddlRedLetter.SelectedIndexChanged += new System.EventHandler(this.ddlRedLetter_SelectedIndexChanged);
			// 
			// lblMobileOptionsSection
			// 
			this.lblMobileOptionsSection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblMobileOptionsSection.BackColor = System.Drawing.SystemColors.InactiveBorder;
			this.lblMobileOptionsSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblMobileOptionsSection, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblMobileOptionsSection, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblMobileOptionsSection, "ConfigurationTool.lblMobileOptionsSection");
			this.lblMobileOptionsSection.Location = new System.Drawing.Point(7, 52);
			this.lblMobileOptionsSection.Name = "lblMobileOptionsSection";
			this.lblMobileOptionsSection.Size = new System.Drawing.Size(350, 23);
			this.lblMobileOptionsSection.TabIndex = 68;
			this.lblMobileOptionsSection.Text = "Options";
			this.lblMobileOptionsSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblGoBibleDescription
			// 
			this.lblGoBibleDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblGoBibleDescription, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblGoBibleDescription, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblGoBibleDescription, "ConfigurationTool.lblGoBibleDescription");
			this.lblGoBibleDescription.Location = new System.Drawing.Point(48, 16);
			this.lblGoBibleDescription.Name = "lblGoBibleDescription";
			this.lblGoBibleDescription.Size = new System.Drawing.Size(305, 26);
			this.lblGoBibleDescription.TabIndex = 66;
			this.lblGoBibleDescription.Text = "Change the settings for mobile content.";
			// 
			// pictureBox4
			// 
			this.pictureBox4.BackgroundImage = global::SIL.PublishingSolution.Properties.Resources.cell;
			this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.pictureBox4, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.pictureBox4, null);
			this.l10NSharpExtender1.SetLocalizingId(this.pictureBox4, "ConfigurationTool.pictureBox4");
			this.pictureBox4.Location = new System.Drawing.Point(7, 7);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(32, 35);
			this.pictureBox4.TabIndex = 67;
			this.pictureBox4.TabStop = false;
			// 
			// tabOthers
			// 
			this.tabOthers.AutoScroll = true;
			this.tabOthers.Controls.Add(this.flowLayoutPanel5);
			this.tabOthers.Controls.Add(this.flowLayoutPanel4);
			this.tabOthers.Controls.Add(this.flowLayoutPanel3);
			this.tabOthers.Controls.Add(this.tableLayoutPanel7);
			this.tabOthers.Controls.Add(this.chkPageBreaks);
			this.tabOthers.Controls.Add(this.chkIncludeImage);
			this.tabOthers.Controls.Add(this.ddlReferences);
			this.tabOthers.Controls.Add(this.lblReferences);
			this.tabOthers.Controls.Add(this.picFonts);
			this.tabOthers.Controls.Add(this.pictureBox2);
			this.tabOthers.Controls.Add(this.lblEpubFontsSection);
			this.tabOthers.Controls.Add(this.lblEputLayoutSection);
			this.tabOthers.Controls.Add(this.chkIncludeFontVariants);
			this.tabOthers.Controls.Add(this.lblEpubDescription);
			this.tabOthers.Controls.Add(this.chkEmbedFonts);
			this.tabOthers.Controls.Add(this.ddlTocLevel);
			this.tabOthers.Controls.Add(this.lblTocLevel);
			this.tabOthers.Controls.Add(this.lblLineSpacing);
			this.tabOthers.Controls.Add(this.ddlChapterNumbers);
			this.tabOthers.Controls.Add(this.txtDefaultLineHeight);
			this.tabOthers.Controls.Add(this.lblChapterNumbers);
			this.tabOthers.Controls.Add(this.pictureBox1);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tabOthers, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tabOthers, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tabOthers, "ConfigurationTool.tabOthers");
			this.tabOthers.Location = new System.Drawing.Point(4, 22);
			this.tabOthers.Name = "tabOthers";
			this.tabOthers.Size = new System.Drawing.Size(314, 727);
			this.tabOthers.TabIndex = 3;
			this.tabOthers.Text = "Properties";
			this.tabOthers.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel5
			// 
			this.flowLayoutPanel5.Controls.Add(this.lblMaxImageWidth);
			this.flowLayoutPanel5.Controls.Add(this.txtMaxImageWidth);
			this.flowLayoutPanel5.Controls.Add(this.lblPx);
			this.flowLayoutPanel5.Location = new System.Drawing.Point(44, 176);
			this.flowLayoutPanel5.Name = "flowLayoutPanel5";
			this.flowLayoutPanel5.Size = new System.Drawing.Size(262, 27);
			this.flowLayoutPanel5.TabIndex = 44;
			// 
			// lblMaxImageWidth
			// 
			this.lblMaxImageWidth.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblMaxImageWidth.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblMaxImageWidth, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblMaxImageWidth, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblMaxImageWidth, "ConfigurationTool.lblMaxImageWidth");
			this.lblMaxImageWidth.Location = new System.Drawing.Point(3, 6);
			this.lblMaxImageWidth.Name = "lblMaxImageWidth";
			this.lblMaxImageWidth.Size = new System.Drawing.Size(117, 13);
			this.lblMaxImageWidth.TabIndex = 7;
			this.lblMaxImageWidth.Text = "Maximum Image Width:";
			this.lblMaxImageWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtMaxImageWidth
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtMaxImageWidth, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtMaxImageWidth, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtMaxImageWidth, "ConfigurationTool.txtMaxImageWidth");
			this.txtMaxImageWidth.Location = new System.Drawing.Point(126, 3);
			this.txtMaxImageWidth.MaxLength = 4;
			this.txtMaxImageWidth.Name = "txtMaxImageWidth";
			this.txtMaxImageWidth.Size = new System.Drawing.Size(45, 20);
			this.txtMaxImageWidth.TabIndex = 8;
			this.txtMaxImageWidth.Enter += new System.EventHandler(this.SetGotFocusValue);
			this.txtMaxImageWidth.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMaxImageWidth_KeyUp);
			this.txtMaxImageWidth.Validated += new System.EventHandler(this.txtMaxImageWidth_Validated);
			// 
			// lblPx
			// 
			this.lblPx.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPx.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblPx, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblPx, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblPx, "ConfigurationTool.lblPx");
			this.lblPx.Location = new System.Drawing.Point(177, 6);
			this.lblPx.Name = "lblPx";
			this.lblPx.Size = new System.Drawing.Size(18, 13);
			this.lblPx.TabIndex = 35;
			this.lblPx.Text = "px";
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.Controls.Add(this.lblDefaultAlignment);
			this.flowLayoutPanel4.Controls.Add(this.ddlDefaultAlignment);
			this.flowLayoutPanel4.Location = new System.Drawing.Point(1, 130);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(305, 24);
			this.flowLayoutPanel4.TabIndex = 43;
			// 
			// lblDefaultAlignment
			// 
			this.lblDefaultAlignment.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblDefaultAlignment.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblDefaultAlignment, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblDefaultAlignment, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblDefaultAlignment, "ConfigurationTool.lblDefaultAlignment");
			this.lblDefaultAlignment.Location = new System.Drawing.Point(3, 7);
			this.lblDefaultAlignment.Name = "lblDefaultAlignment";
			this.lblDefaultAlignment.Size = new System.Drawing.Size(80, 13);
			this.lblDefaultAlignment.TabIndex = 5;
			this.lblDefaultAlignment.Text = "Text Alignment:";
			// 
			// ddlDefaultAlignment
			// 
			this.ddlDefaultAlignment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlDefaultAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlDefaultAlignment.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlDefaultAlignment, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlDefaultAlignment, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlDefaultAlignment, "ConfigurationTool.ddlDefaultAlignment");
			this.ddlDefaultAlignment.Location = new System.Drawing.Point(89, 3);
			this.ddlDefaultAlignment.Name = "ddlDefaultAlignment";
			this.ddlDefaultAlignment.Size = new System.Drawing.Size(103, 21);
			this.ddlDefaultAlignment.TabIndex = 6;
			this.ddlDefaultAlignment.SelectedIndexChanged += new System.EventHandler(this.ddlDefaultAlignment_SelectedIndexChanged);
			this.ddlDefaultAlignment.Enter += new System.EventHandler(this.SetGotFocusValue);
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.Controls.Add(this.lblBaseFontSize);
			this.flowLayoutPanel3.Controls.Add(this.txtBaseFontSize);			
			this.flowLayoutPanel3.Controls.Add(this.lblPt);
			this.flowLayoutPanel3.Location = new System.Drawing.Point(44, 73);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(258, 28);
			this.flowLayoutPanel3.TabIndex = 42;
			// 
			// lblBaseFontSize
			// 
			this.lblBaseFontSize.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblBaseFontSize.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblBaseFontSize, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblBaseFontSize, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblBaseFontSize, "ConfigurationTool.lblBaseFontSize");
			this.lblBaseFontSize.Location = new System.Drawing.Point(3, 6);
			this.lblBaseFontSize.Name = "lblBaseFontSize";
			this.lblBaseFontSize.Size = new System.Drawing.Size(54, 13);
			this.lblBaseFontSize.TabIndex = 1;
			this.lblBaseFontSize.Text = "Font Size:";
			this.lblBaseFontSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtBaseFontSize
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtBaseFontSize, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtBaseFontSize, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtBaseFontSize, "ConfigurationTool.txtBaseFontSize");
			this.txtBaseFontSize.Location = new System.Drawing.Point(63, 3);
			this.txtBaseFontSize.MaxLength = 2;
			this.txtBaseFontSize.Name = "txtBaseFontSize";
			this.txtBaseFontSize.Size = new System.Drawing.Size(45, 20);
			this.txtBaseFontSize.TabIndex = 2;
			this.txtBaseFontSize.Enter += new System.EventHandler(this.SetGotFocusValue);
			this.txtBaseFontSize.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBaseFontSize_KeyUp);
			this.txtBaseFontSize.Validated += new System.EventHandler(this.txtBaseFontSize_Validated);			
			// 
			// lblPt
			// 
			this.lblPt.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblPt.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblPt, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblPt, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblPt, "ConfigurationTool.lblPt");
			this.lblPt.Location = new System.Drawing.Point(133, 6);
			this.lblPt.Name = "lblPt";
			this.lblPt.Size = new System.Drawing.Size(16, 13);
			this.lblPt.TabIndex = 9;
			this.lblPt.Text = "pt";
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.tableLayoutPanel7.ColumnCount = 2;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.97297F));
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.02703F));
			this.tableLayoutPanel7.Controls.Add(this.lblMissingFont, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this.ddlMissingFont, 1, 0);
			this.tableLayoutPanel7.Controls.Add(this.lblNonSILFont, 0, 1);
			this.tableLayoutPanel7.Controls.Add(this.ddlNonSILFont, 1, 1);
			this.tableLayoutPanel7.Controls.Add(this.lblDefaultFont, 0, 2);
			this.tableLayoutPanel7.Controls.Add(this.ddlDefaultFont, 1, 2);
			this.tableLayoutPanel7.Location = new System.Drawing.Point(6, 503);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 3;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.Size = new System.Drawing.Size(296, 82);
			this.tableLayoutPanel7.TabIndex = 41;
			// 
			// lblMissingFont
			// 
			this.lblMissingFont.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblMissingFont.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblMissingFont, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblMissingFont, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblMissingFont, "ConfigurationTool.lblMissingFont");
			this.lblMissingFont.Location = new System.Drawing.Point(3, 7);
			this.lblMissingFont.Name = "lblMissingFont";
			this.lblMissingFont.Size = new System.Drawing.Size(84, 13);
			this.lblMissingFont.TabIndex = 17;
			this.lblMissingFont.Text = "If font is missing:";
			// 
			// ddlMissingFont
			// 
			this.ddlMissingFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlMissingFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlMissingFont.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlMissingFont, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlMissingFont, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlMissingFont, "ConfigurationTool.ddlMissingFont");
			this.ddlMissingFont.Location = new System.Drawing.Point(144, 3);
			this.ddlMissingFont.Name = "ddlMissingFont";
			this.ddlMissingFont.Size = new System.Drawing.Size(149, 21);
			this.ddlMissingFont.TabIndex = 18;
			this.ddlMissingFont.SelectedIndexChanged += new System.EventHandler(this.ddlMissingFont_SelectedIndexChanged);
			// 
			// lblNonSILFont
			// 
			this.lblNonSILFont.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblNonSILFont.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblNonSILFont, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblNonSILFont, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblNonSILFont, "ConfigurationTool.lblNonSILFont");
			this.lblNonSILFont.Location = new System.Drawing.Point(3, 34);
			this.lblNonSILFont.Name = "lblNonSILFont";
			this.lblNonSILFont.Size = new System.Drawing.Size(62, 13);
			this.lblNonSILFont.TabIndex = 19;
			this.lblNonSILFont.Text = "If restricted:";
			// 
			// ddlNonSILFont
			// 
			this.ddlNonSILFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlNonSILFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlNonSILFont.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlNonSILFont, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlNonSILFont, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlNonSILFont, "ConfigurationTool.ddlNonSILFont");
			this.ddlNonSILFont.Location = new System.Drawing.Point(144, 30);
			this.ddlNonSILFont.Name = "ddlNonSILFont";
			this.ddlNonSILFont.Size = new System.Drawing.Size(149, 21);
			this.ddlNonSILFont.TabIndex = 20;
			this.ddlNonSILFont.SelectedIndexChanged += new System.EventHandler(this.ddlNonSILFont_SelectedIndexChanged);
			// 
			// lblDefaultFont
			// 
			this.lblDefaultFont.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.lblDefaultFont.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblDefaultFont, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblDefaultFont, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblDefaultFont, "ConfigurationTool.lblDefaultFont");
			this.lblDefaultFont.Location = new System.Drawing.Point(3, 61);
			this.lblDefaultFont.Name = "lblDefaultFont";
			this.lblDefaultFont.Size = new System.Drawing.Size(74, 13);
			this.lblDefaultFont.TabIndex = 21;
			this.lblDefaultFont.Text = "Fallback Font:";
			// 
			// ddlDefaultFont
			// 
			this.ddlDefaultFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlDefaultFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlDefaultFont.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlDefaultFont, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlDefaultFont, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlDefaultFont, "ConfigurationTool.ddlDefaultFont");
			this.ddlDefaultFont.Location = new System.Drawing.Point(144, 57);
			this.ddlDefaultFont.Name = "ddlDefaultFont";
			this.ddlDefaultFont.Size = new System.Drawing.Size(149, 21);
			this.ddlDefaultFont.TabIndex = 22;
			this.ddlDefaultFont.SelectedIndexChanged += new System.EventHandler(this.ddlDefaultFont_SelectedIndexChanged);
			// 
			// chkPageBreaks
			// 
			this.chkPageBreaks.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkPageBreaks, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkPageBreaks, null);
			this.l10NSharpExtender1.SetLocalizingId(this.chkPageBreaks, "ConfigurationTool.chkPageBreaks");
			this.chkPageBreaks.Location = new System.Drawing.Point(5, 238);
			this.chkPageBreaks.Name = "chkPageBreaks";
			this.chkPageBreaks.Size = new System.Drawing.Size(163, 17);
			this.chkPageBreaks.TabIndex = 40;
			this.chkPageBreaks.Text = "No page breaks within letters";
			this.chkPageBreaks.UseVisualStyleBackColor = true;
			this.chkPageBreaks.Visible = false;
			this.chkPageBreaks.CheckedChanged += new System.EventHandler(this.chkPageBreaks_CheckedChanged);
			// 
			// chkIncludeImage
			// 
			this.chkIncludeImage.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkIncludeImage, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkIncludeImage, null);
			this.l10NSharpExtender1.SetLocalizingId(this.chkIncludeImage, "ConfigurationTool.chkIncludeImage");
			this.chkIncludeImage.Location = new System.Drawing.Point(5, 159);
			this.chkIncludeImage.Name = "chkIncludeImage";
			this.chkIncludeImage.Size = new System.Drawing.Size(98, 17);
			this.chkIncludeImage.TabIndex = 39;
			this.chkIncludeImage.Text = "Include Images";
			this.chkIncludeImage.UseVisualStyleBackColor = true;
			this.chkIncludeImage.CheckedChanged += new System.EventHandler(this.chkIncludeImage_CheckedChanged);
			// 
			// ddlReferences
			// 
			this.ddlReferences.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlReferences.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlReferences.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlReferences, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlReferences, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlReferences, "ConfigurationTool.ddlReferences");
			this.ddlReferences.Location = new System.Drawing.Point(147, 264);
			this.ddlReferences.Name = "ddlReferences";
			this.ddlReferences.Size = new System.Drawing.Size(164, 21);
			this.ddlReferences.TabIndex = 14;
			this.ddlReferences.SelectedIndexChanged += new System.EventHandler(this.ddlReferences_SelectedIndexChanged);
			this.ddlReferences.Enter += new System.EventHandler(this.SetGotFocusValue);
			// 
			// lblReferences
			// 
			this.lblReferences.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblReferences, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblReferences, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblReferences, "ConfigurationTool.lblReferences");
			this.lblReferences.Location = new System.Drawing.Point(5, 267);
			this.lblReferences.Name = "lblReferences";
			this.lblReferences.Size = new System.Drawing.Size(87, 13);
			this.lblReferences.TabIndex = 13;
			this.lblReferences.Text = "Add References:";
			// 
			// picFonts
			// 
			this.picFonts.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picFonts.BackgroundImage")));
			this.picFonts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.picFonts, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.picFonts, null);
			this.l10NSharpExtender1.SetLocalizingId(this.picFonts, "ConfigurationTool.picFonts");
			this.picFonts.Location = new System.Drawing.Point(7, 318);
			this.picFonts.Name = "picFonts";
			this.picFonts.Size = new System.Drawing.Size(32, 29);
			this.picFonts.TabIndex = 38;
			this.picFonts.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.BackgroundImage = global::SIL.PublishingSolution.Properties.Resources.DocumentTools;
			this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.pictureBox2, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.pictureBox2, null);
			this.l10NSharpExtender1.SetLocalizingId(this.pictureBox2, "ConfigurationTool.pictureBox2");
			this.pictureBox2.Location = new System.Drawing.Point(7, 78);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(32, 36);
			this.pictureBox2.TabIndex = 37;
			this.pictureBox2.TabStop = false;
			// 
			// lblEpubFontsSection
			// 
			this.lblEpubFontsSection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblEpubFontsSection.BackColor = System.Drawing.SystemColors.InactiveBorder;
			this.lblEpubFontsSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblEpubFontsSection, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblEpubFontsSection, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblEpubFontsSection, "ConfigurationTool.lblEpubFontsSection");
			this.lblEpubFontsSection.Location = new System.Drawing.Point(7, 292);
			this.lblEpubFontsSection.Name = "lblEpubFontsSection";
			this.lblEpubFontsSection.Size = new System.Drawing.Size(304, 23);
			this.lblEpubFontsSection.TabIndex = 36;
			this.lblEpubFontsSection.Text = "Embedded Fonts";
			this.lblEpubFontsSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblEputLayoutSection
			// 
			this.lblEputLayoutSection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblEputLayoutSection.BackColor = System.Drawing.SystemColors.InactiveBorder;
			this.lblEputLayoutSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblEputLayoutSection, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblEputLayoutSection, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblEputLayoutSection, "ConfigurationTool.lblEputLayoutSection");
			this.lblEputLayoutSection.Location = new System.Drawing.Point(7, 52);
			this.lblEputLayoutSection.Name = "lblEputLayoutSection";
			this.lblEputLayoutSection.Size = new System.Drawing.Size(304, 23);
			this.lblEputLayoutSection.TabIndex = 33;
			this.lblEputLayoutSection.Text = "Layout";
			this.lblEputLayoutSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkIncludeFontVariants
			// 
			this.chkIncludeFontVariants.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkIncludeFontVariants, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkIncludeFontVariants, null);
			this.l10NSharpExtender1.SetLocalizingId(this.chkIncludeFontVariants, "ConfigurationTool.chkIncludeFontVariants");
			this.chkIncludeFontVariants.Location = new System.Drawing.Point(51, 341);
			this.chkIncludeFontVariants.Name = "chkIncludeFontVariants";
			this.chkIncludeFontVariants.Size = new System.Drawing.Size(239, 17);
			this.chkIncludeFontVariants.TabIndex = 16;
			this.chkIncludeFontVariants.Text = "Also Embed Font Variants";
			this.chkIncludeFontVariants.UseVisualStyleBackColor = true;
			this.chkIncludeFontVariants.CheckedChanged += new System.EventHandler(this.chkIncludeFontVariants_CheckedChanged);
			// 
			// lblEpubDescription
			// 
			this.lblEpubDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblEpubDescription, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblEpubDescription, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblEpubDescription, "ConfigurationTool.lblEpubDescription");
			this.lblEpubDescription.Location = new System.Drawing.Point(48, 10);
			this.lblEpubDescription.Name = "lblEpubDescription";
			this.lblEpubDescription.Size = new System.Drawing.Size(256, 38);
			this.lblEpubDescription.TabIndex = 31;
			this.lblEpubDescription.Text = "Change the settings for e-book content.";
			// 
			// chkEmbedFonts
			// 
			this.chkEmbedFonts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkEmbedFonts, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkEmbedFonts, null);
			this.l10NSharpExtender1.SetLocalizingId(this.chkEmbedFonts, "ConfigurationTool.chkEmbedFonts");
			this.chkEmbedFonts.Location = new System.Drawing.Point(51, 318);
			this.chkEmbedFonts.Name = "chkEmbedFonts";
			this.chkEmbedFonts.Size = new System.Drawing.Size(244, 17);
			this.chkEmbedFonts.TabIndex = 15;
			this.chkEmbedFonts.Text = "Embed Fonts in Document";
			this.chkEmbedFonts.UseVisualStyleBackColor = true;
			this.chkEmbedFonts.CheckedChanged += new System.EventHandler(this.chkEmbedFonts_CheckedChanged);
			// 
			// ddlTocLevel
			// 
			this.ddlTocLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlTocLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlTocLevel.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlTocLevel, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlTocLevel, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlTocLevel, "ConfigurationTool.ddlTocLevel");
			this.ddlTocLevel.Location = new System.Drawing.Point(101, 209);
			this.ddlTocLevel.Name = "ddlTocLevel";
			this.ddlTocLevel.Size = new System.Drawing.Size(210, 21);
			this.ddlTocLevel.TabIndex = 10;
			this.ddlTocLevel.SelectedIndexChanged += new System.EventHandler(this.ddlTocLevel_SelectedIndexChanged);
			this.ddlTocLevel.Enter += new System.EventHandler(this.SetGotFocusValue);
			// 
			// lblTocLevel
			// 
			this.lblTocLevel.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblTocLevel, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblTocLevel, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblTocLevel, "ConfigurationTool.lblTocLevel");
			this.lblTocLevel.Location = new System.Drawing.Point(5, 213);
			this.lblTocLevel.Name = "lblTocLevel";
			this.lblTocLevel.Size = new System.Drawing.Size(61, 13);
			this.lblTocLevel.TabIndex = 9;
			this.lblTocLevel.Text = "TOC Level:";
			// 
			// lblLineSpacing
			// 
			this.lblLineSpacing.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblLineSpacing, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblLineSpacing, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblLineSpacing, "ConfigurationTool.lblLineSpacing");
			this.lblLineSpacing.Location = new System.Drawing.Point(48, 107);
			this.lblLineSpacing.Name = "lblLineSpacing";
			this.lblLineSpacing.Size = new System.Drawing.Size(64, 13);
			this.lblLineSpacing.TabIndex = 3;
			this.lblLineSpacing.Text = "Line Height:";
			this.lblLineSpacing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ddlChapterNumbers
			// 
			this.ddlChapterNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlChapterNumbers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlChapterNumbers.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlChapterNumbers, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlChapterNumbers, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlChapterNumbers, "ConfigurationTool.ddlChapterNumbers");
			this.ddlChapterNumbers.Location = new System.Drawing.Point(147, 237);
			this.ddlChapterNumbers.Name = "ddlChapterNumbers";
			this.ddlChapterNumbers.Size = new System.Drawing.Size(164, 21);
			this.ddlChapterNumbers.TabIndex = 12;
			this.ddlChapterNumbers.SelectedIndexChanged += new System.EventHandler(this.ddlChapterNumbers_SelectedIndexChanged);
			this.ddlChapterNumbers.Enter += new System.EventHandler(this.SetGotFocusValue);
			// 
			// txtDefaultLineHeight
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtDefaultLineHeight, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtDefaultLineHeight, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtDefaultLineHeight, "ConfigurationTool.txtDefaultLineHeight");
			this.txtDefaultLineHeight.Location = new System.Drawing.Point(135, 104);
			this.txtDefaultLineHeight.MaxLength = 3;
			this.txtDefaultLineHeight.Name = "txtDefaultLineHeight";
			this.txtDefaultLineHeight.Size = new System.Drawing.Size(45, 20);
			this.txtDefaultLineHeight.TabIndex = 4;
			this.txtDefaultLineHeight.Enter += new System.EventHandler(this.SetGotFocusValue);
			this.txtDefaultLineHeight.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDefaultLineHeight_KeyUp);
			this.txtDefaultLineHeight.Validated += new System.EventHandler(this.txtDefaultLineHeight_Validated);
			// 
			// lblChapterNumbers
			// 
			this.lblChapterNumbers.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblChapterNumbers, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblChapterNumbers, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblChapterNumbers, "ConfigurationTool.lblChapterNumbers");
			this.lblChapterNumbers.Location = new System.Drawing.Point(5, 240);
			this.lblChapterNumbers.Name = "lblChapterNumbers";
			this.lblChapterNumbers.Size = new System.Drawing.Size(92, 13);
			this.lblChapterNumbers.TabIndex = 11;
			this.lblChapterNumbers.Text = "Chapter Numbers:";
			this.lblChapterNumbers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackgroundImage = global::SIL.PublishingSolution.Properties.Resources.epub_logo_color;
			this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.pictureBox1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.pictureBox1, null);
			this.l10NSharpExtender1.SetLocalizingId(this.pictureBox1, "ConfigurationTool.pictureBox1");
			this.pictureBox1.Location = new System.Drawing.Point(7, 7);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 39);
			this.pictureBox1.TabIndex = 32;
			this.pictureBox1.TabStop = false;
			// 
			// tabWeb
			// 
			this.tabWeb.AutoScroll = true;
			this.tabWeb.Controls.Add(this.groupBox3);
			this.tabWeb.Controls.Add(this.groupBox2);
			this.tabWeb.Controls.Add(this.groupBox1);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tabWeb, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tabWeb, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tabWeb, "ConfigurationTool.tabWeb");
			this.tabWeb.Location = new System.Drawing.Point(4, 22);
			this.tabWeb.Name = "tabWeb";
			this.tabWeb.Size = new System.Drawing.Size(314, 727);
			this.tabWeb.TabIndex = 6;
			this.tabWeb.Text = "Properties";
			this.tabWeb.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.AutoSize = true;
			this.groupBox3.Controls.Add(this.tableLayoutPanel6);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.groupBox3, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.groupBox3, null);
			this.l10NSharpExtender1.SetLocalizingId(this.groupBox3, "ConfigurationTool.groupBox3");
			this.groupBox3.Location = new System.Drawing.Point(7, 308);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(286, 217);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Website Details";
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.ColumnCount = 2;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.45196F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.54804F));
			this.tableLayoutPanel6.Controls.Add(this.label17, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.txtWebFtpFldrNme, 1, 6);
			this.tableLayoutPanel6.Controls.Add(this.label19, 0, 6);
			this.tableLayoutPanel6.Controls.Add(this.txtWebUrl, 0, 1);
			this.tableLayoutPanel6.Controls.Add(this.label16, 0, 2);
			this.tableLayoutPanel6.Controls.Add(this.txtWebEmailID, 1, 5);
			this.tableLayoutPanel6.Controls.Add(this.txtWebAdminUsrNme, 1, 2);
			this.tableLayoutPanel6.Controls.Add(this.label18, 0, 5);
			this.tableLayoutPanel6.Controls.Add(this.label15, 0, 3);
			this.tableLayoutPanel6.Controls.Add(this.txtWebAdminSiteNme, 1, 4);
			this.tableLayoutPanel6.Controls.Add(this.txtWebAdminPwd, 1, 3);
			this.tableLayoutPanel6.Controls.Add(this.label14, 0, 4);
			this.tableLayoutPanel6.Location = new System.Drawing.Point(2, 23);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 7;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.Size = new System.Drawing.Size(278, 175);
			this.tableLayoutPanel6.TabIndex = 12;
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.tableLayoutPanel6.SetColumnSpan(this.label17, 2);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label17, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label17, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label17, "ConfigurationTool.label17");
			this.label17.Location = new System.Drawing.Point(3, 0);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(60, 13);
			this.label17.TabIndex = 0;
			this.label17.Text = "Website url";
			// 
			// txtWebFtpFldrNme
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtWebFtpFldrNme, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtWebFtpFldrNme, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtWebFtpFldrNme, "ConfigurationTool.txtWebFtpFldrNme");
			this.txtWebFtpFldrNme.Location = new System.Drawing.Point(95, 146);
			this.txtWebFtpFldrNme.MaxLength = 50;
			this.txtWebFtpFldrNme.Name = "txtWebFtpFldrNme";
			this.txtWebFtpFldrNme.Size = new System.Drawing.Size(174, 20);
			this.txtWebFtpFldrNme.TabIndex = 11;
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label19, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label19, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label19, "ConfigurationTool.label19");
			this.label19.Location = new System.Drawing.Point(3, 143);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(85, 13);
			this.label19.TabIndex = 10;
			this.label19.Text = "Ftp Folder Name";
			// 
			// txtWebUrl
			// 
			this.tableLayoutPanel6.SetColumnSpan(this.txtWebUrl, 2);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtWebUrl, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtWebUrl, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtWebUrl, "ConfigurationTool.txtWebUrl");
			this.txtWebUrl.Location = new System.Drawing.Point(3, 16);
			this.txtWebUrl.MaxLength = 100;
			this.txtWebUrl.Name = "txtWebUrl";
			this.txtWebUrl.Size = new System.Drawing.Size(268, 20);
			this.txtWebUrl.TabIndex = 1;
			this.txtWebUrl.Validated += new System.EventHandler(this.txtWebUrl_Validated);
			// 
			// label16
			// 
			this.label16.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label16.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label16, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label16, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label16, "ConfigurationTool.label16");
			this.label16.Location = new System.Drawing.Point(3, 39);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(55, 26);
			this.label16.TabIndex = 2;
			this.label16.Text = "Admin Username";
			// 
			// txtWebEmailID
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtWebEmailID, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtWebEmailID, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtWebEmailID, "ConfigurationTool.txtWebEmailID");
			this.txtWebEmailID.Location = new System.Drawing.Point(95, 120);
			this.txtWebEmailID.MaxLength = 50;
			this.txtWebEmailID.Name = "txtWebEmailID";
			this.txtWebEmailID.Size = new System.Drawing.Size(174, 20);
			this.txtWebEmailID.TabIndex = 9;
			this.txtWebEmailID.Validated += new System.EventHandler(this.txtWebEmailID_Validated);
			// 
			// txtWebAdminUsrNme
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtWebAdminUsrNme, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtWebAdminUsrNme, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtWebAdminUsrNme, "ConfigurationTool.txtWebAdminUsrNme");
			this.txtWebAdminUsrNme.Location = new System.Drawing.Point(95, 42);
			this.txtWebAdminUsrNme.MaxLength = 25;
			this.txtWebAdminUsrNme.Name = "txtWebAdminUsrNme";
			this.txtWebAdminUsrNme.Size = new System.Drawing.Size(174, 20);
			this.txtWebAdminUsrNme.TabIndex = 3;
			this.txtWebAdminUsrNme.Validated += new System.EventHandler(this.txtWebAdminUsrNme_Validated);
			// 
			// label18
			// 
			this.label18.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label18.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label18, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label18, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label18, "ConfigurationTool.label18");
			this.label18.Location = new System.Drawing.Point(3, 123);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(46, 13);
			this.label18.TabIndex = 8;
			this.label18.Text = "Email ID";
			// 
			// label15
			// 
			this.label15.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label15.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label15, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label15, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label15, "ConfigurationTool.label15");
			this.label15.Location = new System.Drawing.Point(3, 71);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(85, 13);
			this.label15.TabIndex = 4;
			this.label15.Text = "Admin Password";
			// 
			// txtWebAdminSiteNme
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtWebAdminSiteNme, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtWebAdminSiteNme, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtWebAdminSiteNme, "ConfigurationTool.txtWebAdminSiteNme");
			this.txtWebAdminSiteNme.Location = new System.Drawing.Point(95, 94);
			this.txtWebAdminSiteNme.MaxLength = 50;
			this.txtWebAdminSiteNme.Name = "txtWebAdminSiteNme";
			this.txtWebAdminSiteNme.Size = new System.Drawing.Size(174, 20);
			this.txtWebAdminSiteNme.TabIndex = 7;
			// 
			// txtWebAdminPwd
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtWebAdminPwd, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtWebAdminPwd, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtWebAdminPwd, "ConfigurationTool.txtWebAdminPwd");
			this.txtWebAdminPwd.Location = new System.Drawing.Point(95, 68);
			this.txtWebAdminPwd.MaxLength = 25;
			this.txtWebAdminPwd.Name = "txtWebAdminPwd";
			this.txtWebAdminPwd.PasswordChar = '*';
			this.txtWebAdminPwd.Size = new System.Drawing.Size(174, 20);
			this.txtWebAdminPwd.TabIndex = 5;
			this.txtWebAdminPwd.Validated += new System.EventHandler(this.txtWebAdminPwd_Validated);
			// 
			// label14
			// 
			this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label14.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label14, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label14, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label14, "ConfigurationTool.label14");
			this.label14.Location = new System.Drawing.Point(3, 97);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(48, 13);
			this.label14.TabIndex = 6;
			this.label14.Text = "Site Title";
			// 
			// groupBox2
			// 
			this.groupBox2.AutoSize = true;
			this.groupBox2.Controls.Add(this.tableLayoutPanel4);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.groupBox2, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.groupBox2, null);
			this.l10NSharpExtender1.SetLocalizingId(this.groupBox2, "ConfigurationTool.groupBox2");
			this.groupBox2.Location = new System.Drawing.Point(7, 4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(294, 136);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "FTP Details";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
			this.tableLayoutPanel4.Controls.Add(this.lblTargetFileLocation, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.txtFtpPassword, 1, 3);
			this.tableLayoutPanel4.Controls.Add(this.txtFtpFileLocation, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.label3, 0, 3);
			this.tableLayoutPanel4.Controls.Add(this.label6, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.txtFtpUsername, 1, 2);
			this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 22);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 4;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.Size = new System.Drawing.Size(288, 91);
			this.tableLayoutPanel4.TabIndex = 6;
			// 
			// lblTargetFileLocation
			// 
			this.lblTargetFileLocation.AutoSize = true;
			this.tableLayoutPanel4.SetColumnSpan(this.lblTargetFileLocation, 2);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblTargetFileLocation, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblTargetFileLocation, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblTargetFileLocation, "ConfigurationTool.lblTargetFileLocation");
			this.lblTargetFileLocation.Location = new System.Drawing.Point(3, 0);
			this.lblTargetFileLocation.Name = "lblTargetFileLocation";
			this.lblTargetFileLocation.Size = new System.Drawing.Size(193, 13);
			this.lblTargetFileLocation.TabIndex = 0;
			this.lblTargetFileLocation.Text = "FTP Address (ex: ftp://ip address/path)";
			// 
			// txtFtpPassword
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtFtpPassword, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtFtpPassword, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtFtpPassword, "ConfigurationTool.txtFtpPassword");
			this.txtFtpPassword.Location = new System.Drawing.Point(99, 68);
			this.txtFtpPassword.MaxLength = 50;
			this.txtFtpPassword.Name = "txtFtpPassword";
			this.txtFtpPassword.PasswordChar = '*';
			this.txtFtpPassword.Size = new System.Drawing.Size(176, 20);
			this.txtFtpPassword.TabIndex = 5;
			this.txtFtpPassword.Validated += new System.EventHandler(this.txtFtpPassword_Validated);
			// 
			// txtFtpFileLocation
			// 
			this.tableLayoutPanel4.SetColumnSpan(this.txtFtpFileLocation, 2);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtFtpFileLocation, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtFtpFileLocation, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtFtpFileLocation, "ConfigurationTool.txtFtpFileLocation");
			this.txtFtpFileLocation.Location = new System.Drawing.Point(3, 16);
			this.txtFtpFileLocation.MaxLength = 500;
			this.txtFtpFileLocation.Name = "txtFtpFileLocation";
			this.txtFtpFileLocation.Size = new System.Drawing.Size(270, 20);
			this.txtFtpFileLocation.TabIndex = 1;
			this.txtFtpFileLocation.Validated += new System.EventHandler(this.txtFtpFileLocation_Validated);
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label3, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label3, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label3, "ConfigurationTool.label3");
			this.label3.Location = new System.Drawing.Point(3, 71);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(76, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "FTP Password";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label6.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label6, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label6, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label6, "ConfigurationTool.label6");
			this.label6.Location = new System.Drawing.Point(3, 45);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(78, 13);
			this.label6.TabIndex = 2;
			this.label6.Text = "FTP Username";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtFtpUsername
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtFtpUsername, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtFtpUsername, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtFtpUsername, "ConfigurationTool.txtFtpUsername");
			this.txtFtpUsername.Location = new System.Drawing.Point(99, 42);
			this.txtFtpUsername.MaxLength = 1000;
			this.txtFtpUsername.Name = "txtFtpUsername";
			this.txtFtpUsername.Size = new System.Drawing.Size(177, 20);
			this.txtFtpUsername.TabIndex = 3;
			this.txtFtpUsername.Validated += new System.EventHandler(this.txtFtpUsername_Validated);
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.Controls.Add(this.tableLayoutPanel5);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.groupBox1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.groupBox1, null);
			this.l10NSharpExtender1.SetLocalizingId(this.groupBox1, "ConfigurationTool.groupBox1");
			this.groupBox1.Location = new System.Drawing.Point(7, 143);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(286, 163);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "MySql Database Details";
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel5.ColumnCount = 2;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.375F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.625F));
			this.tableLayoutPanel5.Controls.Add(this.label13, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.txtSqlPassword, 1, 4);
			this.tableLayoutPanel5.Controls.Add(this.txtSqlServerName, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.label10, 0, 4);
			this.tableLayoutPanel5.Controls.Add(this.label12, 0, 2);
			this.tableLayoutPanel5.Controls.Add(this.txtSqlUsername, 1, 3);
			this.tableLayoutPanel5.Controls.Add(this.txtSqlDBName, 1, 2);
			this.tableLayoutPanel5.Controls.Add(this.label11, 0, 3);
			this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 25);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 5;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.Size = new System.Drawing.Size(280, 117);
			this.tableLayoutPanel5.TabIndex = 8;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.tableLayoutPanel5.SetColumnSpan(this.label13, 2);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label13, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label13, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label13, "ConfigurationTool.label13");
			this.label13.Location = new System.Drawing.Point(3, 0);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(161, 13);
			this.label13.TabIndex = 0;
			this.label13.Text = "MySql Server name / IP Address";
			// 
			// txtSqlPassword
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtSqlPassword, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtSqlPassword, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtSqlPassword, "ConfigurationTool.txtSqlPassword");
			this.txtSqlPassword.Location = new System.Drawing.Point(99, 94);
			this.txtSqlPassword.MaxLength = 50;
			this.txtSqlPassword.Name = "txtSqlPassword";
			this.txtSqlPassword.PasswordChar = '*';
			this.txtSqlPassword.Size = new System.Drawing.Size(174, 20);
			this.txtSqlPassword.TabIndex = 7;
			this.txtSqlPassword.Validated += new System.EventHandler(this.txtSqlPassword_Validated);
			// 
			// txtSqlServerName
			// 
			this.tableLayoutPanel5.SetColumnSpan(this.txtSqlServerName, 2);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtSqlServerName, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtSqlServerName, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtSqlServerName, "ConfigurationTool.txtSqlServerName");
			this.txtSqlServerName.Location = new System.Drawing.Point(3, 16);
			this.txtSqlServerName.MaxLength = 100;
			this.txtSqlServerName.Name = "txtSqlServerName";
			this.txtSqlServerName.Size = new System.Drawing.Size(268, 20);
			this.txtSqlServerName.TabIndex = 1;
			// 
			// label10
			// 
			this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label10.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label10, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label10, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label10, "ConfigurationTool.label10");
			this.label10.Location = new System.Drawing.Point(3, 97);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(53, 13);
			this.label10.TabIndex = 6;
			this.label10.Text = "Password";
			// 
			// label12
			// 
			this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label12.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label12, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label12, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label12, "ConfigurationTool.label12");
			this.label12.Location = new System.Drawing.Point(3, 45);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(84, 13);
			this.label12.TabIndex = 2;
			this.label12.Text = "Database Name";
			// 
			// txtSqlUsername
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtSqlUsername, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtSqlUsername, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtSqlUsername, "ConfigurationTool.txtSqlUsername");
			this.txtSqlUsername.Location = new System.Drawing.Point(99, 68);
			this.txtSqlUsername.MaxLength = 25;
			this.txtSqlUsername.Name = "txtSqlUsername";
			this.txtSqlUsername.Size = new System.Drawing.Size(174, 20);
			this.txtSqlUsername.TabIndex = 5;
			this.txtSqlUsername.Validated += new System.EventHandler(this.txtSqlUsername_Validated);
			// 
			// txtSqlDBName
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtSqlDBName, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtSqlDBName, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtSqlDBName, "ConfigurationTool.txtSqlDBName");
			this.txtSqlDBName.Location = new System.Drawing.Point(99, 42);
			this.txtSqlDBName.MaxLength = 25;
			this.txtSqlDBName.Name = "txtSqlDBName";
			this.txtSqlDBName.Size = new System.Drawing.Size(174, 20);
			this.txtSqlDBName.TabIndex = 3;
			// 
			// label11
			// 
			this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label11.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label11, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label11, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label11, "ConfigurationTool.label11");
			this.label11.Location = new System.Drawing.Point(3, 71);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(60, 13);
			this.label11.TabIndex = 4;
			this.label11.Text = "User Name";
			// 
			// tabDict4Mids
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tabDict4Mids, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tabDict4Mids, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tabDict4Mids, "ConfigurationTool.tabDict4Mids");
			this.tabDict4Mids.Location = new System.Drawing.Point(4, 22);
			this.tabDict4Mids.Name = "tabDict4Mids";
			this.tabDict4Mids.Size = new System.Drawing.Size(314, 727);
			this.tabDict4Mids.TabIndex = 7;
			this.tabDict4Mids.Text = "Properties";
			this.tabDict4Mids.UseVisualStyleBackColor = true;
			// 
			// tabPreview
			// 
			this.tabPreview.AutoScroll = true;
			this.tabPreview.Controls.Add(this.btnPrevious);
			this.tabPreview.Controls.Add(this.btnNext);
			this.tabPreview.Controls.Add(this.picPreview);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tabPreview, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tabPreview, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tabPreview, "ConfigurationTool.tabPreview");
			this.tabPreview.Location = new System.Drawing.Point(4, 22);
			this.tabPreview.Name = "tabPreview";
			this.tabPreview.Size = new System.Drawing.Size(314, 727);
			this.tabPreview.TabIndex = 4;
			this.tabPreview.Text = "Preview";
			this.tabPreview.UseVisualStyleBackColor = true;
			// 
			// btnPrevious
			// 
			this.btnPrevious.Enabled = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnPrevious, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnPrevious, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnPrevious, "ConfigurationTool.ConfigurationTool.btnPrevious");
			this.btnPrevious.Location = new System.Drawing.Point(158, 0);
			this.btnPrevious.Margin = new System.Windows.Forms.Padding(2);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(25, 23);
			this.btnPrevious.TabIndex = 12;
			this.btnPrevious.Text = "<";
			this.btnPrevious.UseVisualStyleBackColor = true;
			this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
			// 
			// btnNext
			// 
			this.btnNext.Enabled = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnNext, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnNext, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnNext, "ConfigurationTool.ConfigurationTool.btnNext");
			this.btnNext.Location = new System.Drawing.Point(184, 0);
			this.btnNext.Margin = new System.Windows.Forms.Padding(2);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(25, 23);
			this.btnNext.TabIndex = 11;
			this.btnNext.Text = ">";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// picPreview
			// 
			this.picPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.picPreview, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.picPreview, null);
			this.l10NSharpExtender1.SetLocalizingId(this.picPreview, "ConfigurationTool.picPreview");
			this.picPreview.Location = new System.Drawing.Point(3, 24);
			this.picPreview.Name = "picPreview";
			this.picPreview.Size = new System.Drawing.Size(266, 352);
			this.picPreview.TabIndex = 0;
			this.picPreview.TabStop = false;
			// 
			// tabPicture
			// 
			this.tabPicture.AutoScroll = true;
			this.tabPicture.Controls.Add(this.ChkDontPicture);
			this.tabPicture.Controls.Add(this.LblPicPosition);
			this.tabPicture.Controls.Add(this.DdlPicPosition);
			this.tabPicture.Controls.Add(this.GrpPicture);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tabPicture, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tabPicture, null);
			this.l10NSharpExtender1.SetLocalizingId(this.tabPicture, "ConfigurationTool.tabPicture");
			this.tabPicture.Location = new System.Drawing.Point(4, 22);
			this.tabPicture.Name = "tabPicture";
			this.tabPicture.Size = new System.Drawing.Size(314, 727);
			this.tabPicture.TabIndex = 5;
			this.tabPicture.Text = "Pictures";
			this.tabPicture.UseVisualStyleBackColor = true;
			// 
			// ChkDontPicture
			// 
			this.ChkDontPicture.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ChkDontPicture, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ChkDontPicture, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ChkDontPicture, "ConfigurationTool.ChkDontPicture");
			this.ChkDontPicture.Location = new System.Drawing.Point(16, 13);
			this.ChkDontPicture.Name = "ChkDontPicture";
			this.ChkDontPicture.Size = new System.Drawing.Size(135, 17);
			this.ChkDontPicture.TabIndex = 5;
			this.ChkDontPicture.Text = "Do not include pictures";
			this.ChkDontPicture.UseVisualStyleBackColor = true;
			// 
			// LblPicPosition
			// 
			this.LblPicPosition.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.LblPicPosition, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.LblPicPosition, null);
			this.l10NSharpExtender1.SetLocalizingId(this.LblPicPosition, "ConfigurationTool.LblPicPosition");
			this.LblPicPosition.Location = new System.Drawing.Point(23, 202);
			this.LblPicPosition.Name = "LblPicPosition";
			this.LblPicPosition.Size = new System.Drawing.Size(80, 13);
			this.LblPicPosition.TabIndex = 4;
			this.LblPicPosition.Text = "Picture Position";
			// 
			// DdlPicPosition
			// 
			this.DdlPicPosition.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.DdlPicPosition, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.DdlPicPosition, null);
			this.l10NSharpExtender1.SetLocalizingId(this.DdlPicPosition, "ConfigurationTool.DdlPicPosition");
			this.DdlPicPosition.Location = new System.Drawing.Point(109, 199);
			this.DdlPicPosition.Name = "DdlPicPosition";
			this.DdlPicPosition.Size = new System.Drawing.Size(121, 21);
			this.DdlPicPosition.TabIndex = 1;
			// 
			// GrpPicture
			// 
			this.GrpPicture.Controls.Add(this.Lblcm);
			this.GrpPicture.Controls.Add(this.RadSingleColumn);
			this.GrpPicture.Controls.Add(this.RadEntirePage);
			this.GrpPicture.Controls.Add(this.RadWidthIf);
			this.GrpPicture.Controls.Add(this.RadWidthAll);
			this.GrpPicture.Controls.Add(this.LblPictureWidth);
			this.GrpPicture.Controls.Add(this.SpinPicWidth);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.GrpPicture, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.GrpPicture, null);
			this.l10NSharpExtender1.SetLocalizingId(this.GrpPicture, "ConfigurationTool.GrpPicture");
			this.GrpPicture.Location = new System.Drawing.Point(3, 35);
			this.GrpPicture.Name = "GrpPicture";
			this.GrpPicture.Size = new System.Drawing.Size(259, 154);
			this.GrpPicture.TabIndex = 0;
			this.GrpPicture.TabStop = false;
			// 
			// Lblcm
			// 
			this.Lblcm.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.Lblcm, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.Lblcm, null);
			this.l10NSharpExtender1.SetLocalizingId(this.Lblcm, "ConfigurationTool.Lblcm");
			this.Lblcm.Location = new System.Drawing.Point(182, 29);
			this.Lblcm.Name = "Lblcm";
			this.Lblcm.Size = new System.Drawing.Size(21, 13);
			this.Lblcm.TabIndex = 6;
			this.Lblcm.Text = "cm";
			// 
			// RadSingleColumn
			// 
			this.RadSingleColumn.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.RadSingleColumn, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.RadSingleColumn, null);
			this.l10NSharpExtender1.SetLocalizingId(this.RadSingleColumn, "ConfigurationTool.RadSingleColumn");
			this.RadSingleColumn.Location = new System.Drawing.Point(52, 98);
			this.RadSingleColumn.Name = "RadSingleColumn";
			this.RadSingleColumn.Size = new System.Drawing.Size(92, 17);
			this.RadSingleColumn.TabIndex = 5;
			this.RadSingleColumn.TabStop = true;
			this.RadSingleColumn.Text = "Single Column";
			this.RadSingleColumn.UseVisualStyleBackColor = true;
			// 
			// RadEntirePage
			// 
			this.RadEntirePage.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.RadEntirePage, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.RadEntirePage, null);
			this.l10NSharpExtender1.SetLocalizingId(this.RadEntirePage, "ConfigurationTool.RadEntirePage");
			this.RadEntirePage.Location = new System.Drawing.Point(52, 118);
			this.RadEntirePage.Name = "RadEntirePage";
			this.RadEntirePage.Size = new System.Drawing.Size(80, 17);
			this.RadEntirePage.TabIndex = 4;
			this.RadEntirePage.TabStop = true;
			this.RadEntirePage.Text = "Entire Page";
			this.RadEntirePage.UseVisualStyleBackColor = true;
			// 
			// RadWidthIf
			// 
			this.RadWidthIf.AutoSize = true;
			this.RadWidthIf.Checked = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.RadWidthIf, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.RadWidthIf, null);
			this.l10NSharpExtender1.SetLocalizingId(this.RadWidthIf, "ConfigurationTool.RadWidthIf");
			this.RadWidthIf.Location = new System.Drawing.Point(52, 53);
			this.RadWidthIf.Name = "RadWidthIf";
			this.RadWidthIf.Size = new System.Drawing.Size(199, 17);
			this.RadWidthIf.TabIndex = 3;
			this.RadWidthIf.TabStop = true;
			this.RadWidthIf.Text = "Use this width if a width not specified";
			this.RadWidthIf.UseVisualStyleBackColor = true;
			// 
			// RadWidthAll
			// 
			this.RadWidthAll.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.RadWidthAll, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.RadWidthAll, null);
			this.l10NSharpExtender1.SetLocalizingId(this.RadWidthAll, "ConfigurationTool.RadWidthAll");
			this.RadWidthAll.Location = new System.Drawing.Point(52, 76);
			this.RadWidthAll.Name = "RadWidthAll";
			this.RadWidthAll.Size = new System.Drawing.Size(159, 17);
			this.RadWidthAll.TabIndex = 2;
			this.RadWidthAll.TabStop = true;
			this.RadWidthAll.Text = "Use this width for all pictures";
			this.RadWidthAll.UseVisualStyleBackColor = true;
			// 
			// LblPictureWidth
			// 
			this.LblPictureWidth.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.LblPictureWidth, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.LblPictureWidth, null);
			this.l10NSharpExtender1.SetLocalizingId(this.LblPictureWidth, "ConfigurationTool.LblPictureWidth");
			this.LblPictureWidth.Location = new System.Drawing.Point(6, 29);
			this.LblPictureWidth.Name = "LblPictureWidth";
			this.LblPictureWidth.Size = new System.Drawing.Size(71, 13);
			this.LblPictureWidth.TabIndex = 1;
			this.LblPictureWidth.Text = "Picture Width";
			// 
			// SpinPicWidth
			// 
			this.SpinPicWidth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.l10NSharpExtender1.SetLocalizableToolTip(this.SpinPicWidth, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.SpinPicWidth, null);
			this.l10NSharpExtender1.SetLocalizingId(this.SpinPicWidth, "ConfigurationTool.ConfigurationTool.SpinPicWidth");
			this.SpinPicWidth.Location = new System.Drawing.Point(59, 22);
			this.SpinPicWidth.Margin = new System.Windows.Forms.Padding(2);
			this.SpinPicWidth.Name = "SpinPicWidth";
			this.SpinPicWidth.Size = new System.Drawing.Size(101, 20);
			this.SpinPicWidth.TabIndex = 0;
			this.SpinPicWidth.Value = new decimal(new int[] {
            400,
            0,
            0,
            131072});
			// 
			// lblGuidewordLength
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblGuidewordLength, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblGuidewordLength, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblGuidewordLength, "lblGuidewordLength");
			this.lblGuidewordLength.Location = new System.Drawing.Point(0, 0);
			this.lblGuidewordLength.Name = "lblGuidewordLength";
			this.lblGuidewordLength.Size = new System.Drawing.Size(100, 23);
			this.lblGuidewordLength.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.RoyalBlue;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label2, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label2, null);
			this.l10NSharpExtender1.SetLocalizingId(this.label2, "ConfigurationTool.label2");
			this.label2.Location = new System.Drawing.Point(94, 1);
			this.label2.Margin = new System.Windows.Forms.Padding(0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(424, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Stylesheets";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblInfoCaption
			// 
			this.lblInfoCaption.BackColor = System.Drawing.Color.RoyalBlue;
			this.lblInfoCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblInfoCaption.ForeColor = System.Drawing.Color.White;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblInfoCaption, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblInfoCaption, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblInfoCaption, "ConfigurationTool.lblInfoCaption");
			this.lblInfoCaption.Location = new System.Drawing.Point(519, 1);
			this.lblInfoCaption.Margin = new System.Windows.Forms.Padding(0);
			this.lblInfoCaption.Name = "lblInfoCaption";
			this.lblInfoCaption.Size = new System.Drawing.Size(341, 23);
			this.lblInfoCaption.TabIndex = 2;
			this.lblInfoCaption.Text = "Css";
			this.lblInfoCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtCss
			// 
			this.txtCss.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtCss, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtCss, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtCss, "ConfigurationTool.txtCss");
			this.txtCss.Location = new System.Drawing.Point(3, 3);
			this.txtCss.Multiline = true;
			this.txtCss.Name = "txtCss";
			this.txtCss.ReadOnly = true;
			this.txtCss.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtCss.Size = new System.Drawing.Size(314, 35);
			this.txtCss.TabIndex = 0;
			// 
			// TLPanelOuter
			// 
			this.TLPanelOuter.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.TLPanelOuter.ColumnCount = 3;
			this.TLPanelOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
			this.TLPanelOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLPanelOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 356F));
			this.TLPanelOuter.Controls.Add(this.label2, 1, 0);
			this.TLPanelOuter.Controls.Add(this.TLPanel1, 0, 1);
			this.TLPanelOuter.Controls.Add(this.lblInfoCaption, 2, 0);
			this.TLPanelOuter.Controls.Add(this.TLPanel2, 1, 1);
			this.TLPanelOuter.Controls.Add(this.TLPanel3, 2, 1);
			this.TLPanelOuter.Controls.Add(this.lblType, 0, 0);
			this.TLPanelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLPanelOuter.Location = new System.Drawing.Point(0, 52);
			this.TLPanelOuter.Margin = new System.Windows.Forms.Padding(1);
			this.TLPanelOuter.MinimumSize = new System.Drawing.Size(876, 600);
			this.TLPanelOuter.Name = "TLPanelOuter";
			this.TLPanelOuter.RowCount = 2;
			this.TLPanelOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
			this.TLPanelOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLPanelOuter.Size = new System.Drawing.Size(876, 763);
			this.TLPanelOuter.TabIndex = 19;
			// 
			// TLPanel1
			// 
			this.TLPanel1.ColumnCount = 1;
			this.TLPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLPanel1.Controls.Add(this.panel1, 0, 1);
			this.TLPanel1.Controls.Add(this.panel2, 0, 0);
			this.TLPanel1.Location = new System.Drawing.Point(1, 25);
			this.TLPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.TLPanel1.Name = "TLPanel1";
			this.TLPanel1.RowCount = 2;
			this.TLPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
			this.TLPanel1.Size = new System.Drawing.Size(91, 344);
			this.TLPanel1.TabIndex = 3;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnScripture);
			this.panel1.Controls.Add(this.btnDictionary);
			this.panel1.Location = new System.Drawing.Point(3, 291);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(85, 48);
			this.panel1.TabIndex = 0;
			// 
			// btnScripture
			// 
			this.btnScripture.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnScripture.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
			this.btnScripture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnScripture, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnScripture, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnScripture, "ConfigurationTool.btnScripture");
			this.btnScripture.Location = new System.Drawing.Point(0, 23);
			this.btnScripture.Name = "btnScripture";
			this.btnScripture.Size = new System.Drawing.Size(85, 23);
			this.btnScripture.TabIndex = 1;
			this.btnScripture.Text = "&Scripture";
			this.btnScripture.UseVisualStyleBackColor = true;
			this.btnScripture.Click += new System.EventHandler(this.btnScripture_Click);
			// 
			// btnDictionary
			// 
			this.btnDictionary.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnDictionary.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
			this.btnDictionary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnDictionary, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnDictionary, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnDictionary, "ConfigurationTool.btnDictionary");
			this.btnDictionary.Location = new System.Drawing.Point(0, 0);
			this.btnDictionary.Name = "btnDictionary";
			this.btnDictionary.Size = new System.Drawing.Size(85, 23);
			this.btnDictionary.TabIndex = 0;
			this.btnDictionary.Text = "&Dictionary";
			this.btnDictionary.UseVisualStyleBackColor = true;
			this.btnDictionary.Click += new System.EventHandler(this.btnDictionary_Click);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnOthers);
			this.panel2.Controls.Add(this.btnWeb);
			this.panel2.Controls.Add(this.btnMobile);
			this.panel2.Controls.Add(this.btnPaper);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(85, 282);
			this.panel2.TabIndex = 20;
			// 
			// btnOthers
			// 
			this.btnOthers.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnOthers.Enabled = false;
			this.btnOthers.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
			this.btnOthers.FlatAppearance.BorderSize = 0;
			this.btnOthers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnOthers.Image = ((System.Drawing.Image)(resources.GetObject("btnOthers.Image")));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnOthers, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnOthers, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnOthers, "ConfigurationTool.btnOthers");
			this.btnOthers.Location = new System.Drawing.Point(0, 210);
			this.btnOthers.Name = "btnOthers";
			this.btnOthers.Size = new System.Drawing.Size(85, 70);
			this.btnOthers.TabIndex = 3;
			this.btnOthers.Text = "&Others";
			this.btnOthers.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnOthers.UseVisualStyleBackColor = true;
			this.btnOthers.Click += new System.EventHandler(this.btnOthers_Click);
			// 
			// btnWeb
			// 
			this.btnWeb.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnWeb.Enabled = false;
			this.btnWeb.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
			this.btnWeb.FlatAppearance.BorderSize = 0;
			this.btnWeb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnWeb.Image = ((System.Drawing.Image)(resources.GetObject("btnWeb.Image")));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnWeb, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnWeb, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnWeb, "ConfigurationTool.btnWeb");
			this.btnWeb.Location = new System.Drawing.Point(0, 140);
			this.btnWeb.Name = "btnWeb";
			this.btnWeb.Size = new System.Drawing.Size(85, 70);
			this.btnWeb.TabIndex = 2;
			this.btnWeb.Text = "&Web";
			this.btnWeb.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnWeb.UseVisualStyleBackColor = true;
			this.btnWeb.Click += new System.EventHandler(this.btnWeb_Click);
			// 
			// btnMobile
			// 
			this.btnMobile.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnMobile.Enabled = false;
			this.btnMobile.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
			this.btnMobile.FlatAppearance.BorderSize = 0;
			this.btnMobile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnMobile.Image = ((System.Drawing.Image)(resources.GetObject("btnMobile.Image")));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnMobile, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnMobile, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnMobile, "ConfigurationTool.btnMobile");
			this.btnMobile.Location = new System.Drawing.Point(0, 70);
			this.btnMobile.Name = "btnMobile";
			this.btnMobile.Size = new System.Drawing.Size(85, 70);
			this.btnMobile.TabIndex = 1;
			this.btnMobile.Text = "&Mobile";
			this.btnMobile.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnMobile.UseVisualStyleBackColor = true;
			this.btnMobile.Click += new System.EventHandler(this.btnMobile_Click);
			// 
			// btnPaper
			// 
			this.btnPaper.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnPaper.Enabled = false;
			this.btnPaper.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
			this.btnPaper.FlatAppearance.BorderSize = 0;
			this.btnPaper.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPaper.Image = ((System.Drawing.Image)(resources.GetObject("btnPaper.Image")));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnPaper, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnPaper, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnPaper, "ConfigurationTool.btnPaper");
			this.btnPaper.Location = new System.Drawing.Point(0, 0);
			this.btnPaper.Name = "btnPaper";
			this.btnPaper.Size = new System.Drawing.Size(85, 70);
			this.btnPaper.TabIndex = 0;
			this.btnPaper.Text = "&Paper";
			this.btnPaper.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnPaper.UseVisualStyleBackColor = true;
			this.btnPaper.Click += new System.EventHandler(this.btnPaper_Click);
			// 
			// TLPanel2
			// 
			this.TLPanel2.BackColor = System.Drawing.SystemColors.Control;
			this.TLPanel2.ColumnCount = 1;
			this.TLPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLPanel2.Controls.Add(this.stylesGrid, 0, 0);
			this.TLPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TLPanel2.Location = new System.Drawing.Point(97, 28);
			this.TLPanel2.Name = "TLPanel2";
			this.TLPanel2.RowCount = 1;
			this.TLPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TLPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 525F));
			this.TLPanel2.Size = new System.Drawing.Size(418, 731);
			this.TLPanel2.TabIndex = 4;
			// 
			// TLPanel3
			// 
			this.TLPanel3.ColumnCount = 1;
			this.TLPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 331F));
			this.TLPanel3.Controls.Add(this.panel3, 0, 1);
			this.TLPanel3.Controls.Add(this.txtCss, 0, 0);
			this.TLPanel3.Location = new System.Drawing.Point(522, 28);
			this.TLPanel3.MaximumSize = new System.Drawing.Size(331, 900);
			this.TLPanel3.MinimumSize = new System.Drawing.Size(331, 0);
			this.TLPanel3.Name = "TLPanel3";
			this.TLPanel3.RowCount = 2;
			this.TLPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 41F));
			this.TLPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 175F));
			this.TLPanel3.Size = new System.Drawing.Size(331, 731);
			this.TLPanel3.TabIndex = 5;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.tabControl1);
			this.panel3.Location = new System.Drawing.Point(3, 44);
			this.panel3.MaximumSize = new System.Drawing.Size(331, 800);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(325, 684);
			this.panel3.TabIndex = 20;
			// 
			// lblType
			// 
			this.lblType.BackColor = System.Drawing.Color.RoyalBlue;
			this.lblType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblType.ForeColor = System.Drawing.Color.White;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblType, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblType, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblType, "ConfigurationTool.lblType");
			this.lblType.Location = new System.Drawing.Point(1, 1);
			this.lblType.Margin = new System.Windows.Forms.Padding(0);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(91, 23);
			this.lblType.TabIndex = 0;
			this.lblType.Text = "Dictionary";
			this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 793);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(876, 22);
			this.statusStrip1.TabIndex = 20;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.toolStripStatusLabel1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.toolStripStatusLabel1, null);
			this.l10NSharpExtender1.SetLocalizingId(this.toolStripStatusLabel1, "ConfigurationTool.toolStripStatusLabel1");
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(201, 17);
			this.toolStripStatusLabel1.Text = "Changes will be automatically saved.";
			// 
			// l10NSharpExtender1
			// 
			this.l10NSharpExtender1.LocalizationManagerId = "Pathway";
			this.l10NSharpExtender1.PrefixForNewItems = "ConfigurationTool";
			// 
			// ConfigurationTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(876, 815);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.TLPanelOuter);
			this.Controls.Add(this.toolStripMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this, null);
			this.l10NSharpExtender1.SetLocalizationComment(this, null);
			this.l10NSharpExtender1.SetLocalizingId(this, "ConfigurationTool.WindowTitle");
			this.Name = "ConfigurationTool";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Pathway Configuration Tool";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigurationTool_FormClosing);
			this.Load += new System.EventHandler(this.ConfigurationTool_Load);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ConfigurationTool_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.stylesGrid)).EndInit();
			this.toolStripMain.ResumeLayout(false);
			this.toolStripMain.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabInfo.ResumeLayout(false);
			this.tabInfo.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tabDisplay.ResumeLayout(false);
			this.tblPnlDisplay.ResumeLayout(false);
			this.tblPnlDisplay.PerformLayout();
			this.pnlOtherFormat.ResumeLayout(false);
			this.pnlOtherFormat.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.pnlGuidewordLength.ResumeLayout(false);
			this.pnlGuidewordLength.PerformLayout();
			this.pnlReferenceFormat.ResumeLayout(false);
			this.pnlReferenceFormat.PerformLayout();
			this.tblPnlRefFormat.ResumeLayout(false);
			this.tblPnlRefFormat.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.tabMobile.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.mobileIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			this.tabOthers.ResumeLayout(false);
			this.tabOthers.PerformLayout();
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picFonts)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.tabWeb.ResumeLayout(false);
			this.tabWeb.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.tabPreview.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
			this.tabPicture.ResumeLayout(false);
			this.tabPicture.PerformLayout();
			this.GrpPicture.ResumeLayout(false);
			this.GrpPicture.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.SpinPicWidth)).EndInit();
			this.TLPanelOuter.ResumeLayout(false);
			this.TLPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.TLPanel2.ResumeLayout(false);
			this.TLPanel3.ResumeLayout(false);
			this.TLPanel3.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView stylesGrid;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton tsNew;
        private System.Windows.Forms.ToolStripButton tsDelete;
        private System.Windows.Forms.ToolStripButton tsUndo;
        private System.Windows.Forms.ToolStripButton tsRedo;
        private System.Windows.Forms.ToolStripButton tsPreview;
        private System.Windows.Forms.ToolStripButton tsDefault;
        private System.Windows.Forms.ToolStripButton tsSend;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInfo;
        private System.Windows.Forms.Button btnApproved;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.CheckBox chkAvailable;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Label lblAvailable;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TabPage tabDisplay;
        private System.Windows.Forms.TextBox txtApproved;
        private System.Windows.Forms.Label lblApproved;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblInfoCaption;
        private System.Windows.Forms.TextBox txtCss;
        private System.Windows.Forms.ComboBox ddlPagePageSize;
        private System.Windows.Forms.Label lblPagePageSize;
        private System.Windows.Forms.ComboBox ddlPageColumn;
        private System.Windows.Forms.Label lblPageColumn;
        private System.Windows.Forms.ToolStripButton tsSaveAs;
        private System.Windows.Forms.ComboBox ddlJustified;
        private System.Windows.Forms.Label lblJustified;
        private System.Windows.Forms.TextBox txtPageOutside;
        private System.Windows.Forms.TextBox txtPageInside;
        private System.Windows.Forms.TextBox txtPageTop;
        private System.Windows.Forms.TextBox txtPageBottom;
        private System.Windows.Forms.Label lblPageInside;
        private System.Windows.Forms.Label lblPageOutside;
        private System.Windows.Forms.Label lblPageBottom;
        private System.Windows.Forms.Label lblPageTop;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPageGutterWidth;
        private System.Windows.Forms.Label lblPageGutter;
        private System.Windows.Forms.TableLayoutPanel TLPanelOuter;
        private System.Windows.Forms.TableLayoutPanel TLPanel1;
        private System.Windows.Forms.TableLayoutPanel TLPanel2;
        private System.Windows.Forms.TableLayoutPanel TLPanel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnScripture;
        private System.Windows.Forms.Button btnDictionary;
        protected internal System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnWeb;
        private System.Windows.Forms.Button btnMobile;
        private System.Windows.Forms.Button btnPaper;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox ddlVerticalJustify;
        private System.Windows.Forms.Label lblVerticalJustify;
        private System.Windows.Forms.ComboBox ddlPicture;
        private System.Windows.Forms.Label lblLineSpace;
        private System.Windows.Forms.Label lblRunningHeader;
        private System.Windows.Forms.ComboBox ddlLeading;
        private System.Windows.Forms.ComboBox ddlRunningHead;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOthers;
        private System.Windows.Forms.TabPage tabMobile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddlRedLetter;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ddlFiles;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox mobileIcon;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage tabOthers;
        private System.Windows.Forms.TabPage tabPreview;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.TabPage tabPicture;
        private System.Windows.Forms.GroupBox GrpPicture;
        private System.Windows.Forms.CheckBox ChkDontPicture;
        private System.Windows.Forms.Label LblPicPosition;
        private System.Windows.Forms.ComboBox DdlPicPosition;
        private System.Windows.Forms.Label Lblcm;
        private System.Windows.Forms.RadioButton RadSingleColumn;
        private System.Windows.Forms.RadioButton RadEntirePage;
        private System.Windows.Forms.RadioButton RadWidthIf;
        private System.Windows.Forms.RadioButton RadWidthAll;
        private System.Windows.Forms.Label LblPictureWidth;
        private System.Windows.Forms.NumericUpDown SpinPicWidth;
        private System.Windows.Forms.Label lblLineSpacing;
        private System.Windows.Forms.TextBox txtBaseFontSize;
        private System.Windows.Forms.Label lblBaseFontSize;
        private System.Windows.Forms.TextBox txtDefaultLineHeight;
        private System.Windows.Forms.ComboBox ddlChapterNumbers;
        private System.Windows.Forms.Label lblChapterNumbers;
        private System.Windows.Forms.Label lblPt;
        private System.Windows.Forms.ComboBox ddlTocLevel;
        private System.Windows.Forms.Label lblTocLevel;
        private System.Windows.Forms.Label lblPx;
        private System.Windows.Forms.TextBox txtMaxImageWidth;
        private System.Windows.Forms.Label lblMaxImageWidth;
        private System.Windows.Forms.CheckBox chkIncludeFontVariants;
        private System.Windows.Forms.CheckBox chkEmbedFonts;
        private System.Windows.Forms.ComboBox ddlNonSILFont;
        private System.Windows.Forms.ComboBox ddlMissingFont;
        private System.Windows.Forms.Label lblNonSILFont;
        private System.Windows.Forms.Label lblMissingFont;
        private System.Windows.Forms.ComboBox ddlDefaultFont;
        private System.Windows.Forms.Label lblDefaultFont;
        private System.Windows.Forms.ComboBox ddlDefaultAlignment;
        private System.Windows.Forms.Label lblDefaultAlignment;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblEpubDescription;
        private System.Windows.Forms.Label lblEputLayoutSection;
        private System.Windows.Forms.Label lblEpubFontsSection;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox picFonts;
        private System.Windows.Forms.Label lblMobileOptionsSection;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label lblGoBibleDescription;
        private System.Windows.Forms.ToolStripSplitButton toolStripHelpButton;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ComboBox ddlReferences;
        private System.Windows.Forms.Label lblReferences;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtFtpPassword;
        private System.Windows.Forms.Label lblTargetFileLocation;
        private System.Windows.Forms.TextBox txtFtpFileLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFtpUsername;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSqlPassword;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSqlUsername;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtSqlDBName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtSqlServerName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabPage tabWeb;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtWebAdminSiteNme;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtWebAdminPwd;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtWebAdminUsrNme;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtWebUrl;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtWebEmailID;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtWebFtpFldrNme;
        private Label label19;
        private CheckBox chkFixedLineHeight;
        private ComboBox ddlLanguage;
        private Label label20;
        private CheckBox chkIncludeImage;
        private LinkLabel lnkLblUrl;
        private Label lblProjectUrl;
        private ToolStripMenuItem studentManualToolStripMenuItem;
        private Label lblReferenceFormat;
        private ComboBox ddlReferenceFormat;
        private Panel pnlOtherFormat;
        private Label lblPageNumber;
        private ComboBox ddlPageNumber;
        private ComboBox ddlFileProduceDict;
        private Label lblFileProduceDict;
        private ComboBox ddlSense;
        private Label lblSenseLayout;
        private Label lblRules;
        private ComboBox ddlRules;
        private Label lblFont;
        private ComboBox ddlFontSize;
        private Panel pnlReferenceFormat;
        private TextBox txtFnCallerSymbol;
        private CheckBox chkIncludeCusFnCaller;
        private CheckBox chkTurnOffFirstVerse;
        private TextBox txtXrefCusSymbol;
        private CheckBox chkXrefCusSymbol;
        private TabPage tabDict4Mids;
        private CheckBox chkPageBreaks;
        private CheckBox chkHideSpaceVerseNo;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Label lblGuidewordLength;
        private Panel pnlGuidewordLength;
        private TextBox txtGuidewordLength;
        private Label label9;
        private CheckBox chkSplitFileByLetter;
        private CheckBox chkDisableWO;
        private ToolStripButton tsReset;
        private ToolStripMenuItem moreHelpToolStripMenuItem;
        private L10NSharp.UI.L10NSharpExtender l10NSharpExtender1;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tblPnlDisplay;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel6;
        private TableLayoutPanel tblPnlRefFormat;
		private TableLayoutPanel tableLayoutPanel2;
		private TableLayoutPanel tableLayoutPanel3;
		private FlowLayoutPanel flowLayoutPanel2;
		private TableLayoutPanel tableLayoutPanel4;
		private TableLayoutPanel tableLayoutPanel5;
		private TableLayoutPanel tableLayoutPanel6;
		private TableLayoutPanel tableLayoutPanel7;
		private FlowLayoutPanel flowLayoutPanel3;
		private FlowLayoutPanel flowLayoutPanel5;
		private FlowLayoutPanel flowLayoutPanel4;
        private ToolStripMenuItem userInterfaceToolStripMenuItem;
        private CheckBox chkCenterTitleHeader;
        private ComboBox ddlHeaderFontSize;
        private Label lblHeaderFontSize;
    }
}