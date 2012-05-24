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
            this.stylesGrid = new System.Windows.Forms.DataGridView();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tsNew = new System.Windows.Forms.ToolStripButton();
            this.tsSaveAs = new System.Windows.Forms.ToolStripButton();
            this.tsDelete = new System.Windows.Forms.ToolStripButton();
            this.tsUndo = new System.Windows.Forms.ToolStripButton();
            this.tsRedo = new System.Windows.Forms.ToolStripButton();
            this.tsPreview = new System.Windows.Forms.ToolStripButton();
            this.tsDefault = new System.Windows.Forms.ToolStripButton();
            this.tsSend = new System.Windows.Forms.ToolStripButton();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabInfo = new System.Windows.Forms.TabPage();
            this.txtApproved = new System.Windows.Forms.TextBox();
            this.lblApproved = new System.Windows.Forms.Label();
            this.btnApproved = new System.Windows.Forms.Button();
            this.lblComment = new System.Windows.Forms.Label();
            this.chkAvailable = new System.Windows.Forms.CheckBox();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.lblDesc = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tabDisplay = new System.Windows.Forms.TabPage();
            this.LblPageNumber = new System.Windows.Forms.Label();
            this.ddlPageNumber = new System.Windows.Forms.ComboBox();
            this.ddlFileProduceDict = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ddlVerticalJustify = new System.Windows.Forms.ComboBox();
            this.lblVerticalJustify = new System.Windows.Forms.Label();
            this.ddlSense = new System.Windows.Forms.ComboBox();
            this.lblSenseLayout = new System.Windows.Forms.Label();
            this.ddlPicture = new System.Windows.Forms.ComboBox();
            this.lblLineSpace = new System.Windows.Forms.Label();
            this.lblRunningHeader = new System.Windows.Forms.Label();
            this.ddlLeading = new System.Windows.Forms.ComboBox();
            this.ddlRunningHead = new System.Windows.Forms.ComboBox();
            this.lblRules = new System.Windows.Forms.Label();
            this.ddlRules = new System.Windows.Forms.ComboBox();
            this.lblFont = new System.Windows.Forms.Label();
            this.ddlFontSize = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPageGutterWidth = new System.Windows.Forms.TextBox();
            this.lblPageGutter = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPageOutside = new System.Windows.Forms.TextBox();
            this.txtPageInside = new System.Windows.Forms.TextBox();
            this.txtPageTop = new System.Windows.Forms.TextBox();
            this.txtPageBottom = new System.Windows.Forms.TextBox();
            this.lblPageInside = new System.Windows.Forms.Label();
            this.lblPageOutside = new System.Windows.Forms.Label();
            this.lblPageBottom = new System.Windows.Forms.Label();
            this.lblPageTop = new System.Windows.Forms.Label();
            this.ddlJustified = new System.Windows.Forms.ComboBox();
            this.lblJustified = new System.Windows.Forms.Label();
            this.ddlPageColumn = new System.Windows.Forms.ComboBox();
            this.lblPageColumn = new System.Windows.Forms.Label();
            this.ddlPagePageSize = new System.Windows.Forms.ComboBox();
            this.lblPagePageSize = new System.Windows.Forms.Label();
            this.tabMobile = new System.Windows.Forms.TabPage();
            this.lblMobileOptionsSection = new System.Windows.Forms.Label();
            this.lblGoBibleDescription = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ddlRedLetter = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ddlFiles = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.mobileIcon = new System.Windows.Forms.PictureBox();
            this.tabOthers = new System.Windows.Forms.TabPage();
            this.ddlReferences = new System.Windows.Forms.ComboBox();
            this.lblReferences = new System.Windows.Forms.Label();
            this.picFonts = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.ddlNonSILFont = new System.Windows.Forms.ComboBox();
            this.lblEpubFontsSection = new System.Windows.Forms.Label();
            this.ddlMissingFont = new System.Windows.Forms.ComboBox();
            this.ddlDefaultAlignment = new System.Windows.Forms.ComboBox();
            this.lblNonSILFont = new System.Windows.Forms.Label();
            this.lblEputLayoutSection = new System.Windows.Forms.Label();
            this.lblMissingFont = new System.Windows.Forms.Label();
            this.lblDefaultAlignment = new System.Windows.Forms.Label();
            this.ddlDefaultFont = new System.Windows.Forms.ComboBox();
            this.lblDefaultFont = new System.Windows.Forms.Label();
            this.lblPx = new System.Windows.Forms.Label();
            this.chkIncludeFontVariants = new System.Windows.Forms.CheckBox();
            this.lblEpubDescription = new System.Windows.Forms.Label();
            this.chkEmbedFonts = new System.Windows.Forms.CheckBox();
            this.txtMaxImageWidth = new System.Windows.Forms.TextBox();
            this.ddlTocLevel = new System.Windows.Forms.ComboBox();
            this.lblTocLevel = new System.Windows.Forms.Label();
            this.lblBaseFontSize = new System.Windows.Forms.Label();
            this.lblMaxImageWidth = new System.Windows.Forms.Label();
            this.txtBaseFontSize = new System.Windows.Forms.TextBox();
            this.lblPt = new System.Windows.Forms.Label();
            this.lblLineSpacing = new System.Windows.Forms.Label();
            this.ddlChapterNumbers = new System.Windows.Forms.ComboBox();
            this.txtDefaultLineHeight = new System.Windows.Forms.TextBox();
            this.lblChapterNumbers = new System.Windows.Forms.Label();
            this.lblPct = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabWeb = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtWebFtpFldrNme = new System.Windows.Forms.TextBox();
            this.txtWebEmailID = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtWebAdminSiteNme = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtWebAdminPwd = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtWebAdminUsrNme = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtWebUrl = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtFtpPassword = new System.Windows.Forms.TextBox();
            this.lblTargetFileLocation = new System.Windows.Forms.Label();
            this.txtFtpFileLocation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFtpUsername = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSqlPassword = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSqlUsername = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSqlDBName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtSqlServerName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.stylesGrid)).BeginInit();
            this.toolStripMain.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabInfo.SuspendLayout();
            this.tabDisplay.SuspendLayout();
            this.tabMobile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mobileIcon)).BeginInit();
            this.tabOthers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFonts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabWeb.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.SuspendLayout();
            // 
            // stylesGrid
            // 
            this.stylesGrid.AllowUserToAddRows = false;
            this.stylesGrid.AllowUserToDeleteRows = false;
            this.stylesGrid.AllowUserToResizeRows = false;
            this.stylesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.stylesGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.stylesGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.stylesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.stylesGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.stylesGrid.Location = new System.Drawing.Point(4, 0);
            this.stylesGrid.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.stylesGrid.MultiSelect = false;
            this.stylesGrid.Name = "stylesGrid";
            this.stylesGrid.ReadOnly = true;
            this.stylesGrid.RowHeadersVisible = false;
            this.stylesGrid.RowTemplate.Height = 24;
            this.stylesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.stylesGrid.Size = new System.Drawing.Size(720, 613);
            this.stylesGrid.TabIndex = 0;
            this.stylesGrid.TabStop = false;
            this.stylesGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.stylesGrid_RowEnter);
            this.stylesGrid.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.stylesGrid_ColumnWidthChanged);
            this.stylesGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.stylesGrid_RowEnter);
            // 
            // toolStripMain
            // 
            this.toolStripMain.AccessibleName = "toolStripMain";
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsNew,
            this.tsSaveAs,
            this.tsDelete,
            this.tsUndo,
            this.tsRedo,
            this.tsPreview,
            this.tsDefault,
            this.tsSend,
            this.toolStripSplitButton1});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1272, 56);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "New";
            this.toolStripMain.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripMain_ItemClicked);
            // 
            // tsNew
            // 
            this.tsNew.AccessibleName = "tsNew";
            this.tsNew.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.tsNew.Image = ((System.Drawing.Image)(resources.GetObject("tsNew.Image")));
            this.tsNew.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.tsNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsNew.Name = "tsNew";
            this.tsNew.Size = new System.Drawing.Size(43, 53);
            this.tsNew.Text = "&New";
            this.tsNew.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.tsNew.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.tsNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsNew.ToolTipText = "Add a brand new stylesheet (Alt+N) ";
            this.tsNew.Click += new System.EventHandler(this.tsNew_Click);
            // 
            // tsSaveAs
            // 
            this.tsSaveAs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.tsSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("tsSaveAs.Image")));
            this.tsSaveAs.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSaveAs.Name = "tsSaveAs";
            this.tsSaveAs.Size = new System.Drawing.Size(66, 53);
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
            this.tsDelete.Name = "tsDelete";
            this.tsDelete.Size = new System.Drawing.Size(55, 53);
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
            this.tsUndo.Name = "tsUndo";
            this.tsUndo.Size = new System.Drawing.Size(53, 53);
            this.tsUndo.Text = " &Undo";
            this.tsUndo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsUndo.ToolTipText = "Undo the last change (Alt+U)";
            this.tsUndo.Click += new System.EventHandler(this.tsUndo_Click);
            // 
            // tsRedo
            // 
            this.tsRedo.AccessibleName = "tsPreview";
            this.tsRedo.Enabled = false;
            this.tsRedo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.tsRedo.Image = ((System.Drawing.Image)(resources.GetObject("tsRedo.Image")));
            this.tsRedo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsRedo.Name = "tsRedo";
            this.tsRedo.Size = new System.Drawing.Size(52, 53);
            this.tsRedo.Text = " &Redo";
            this.tsRedo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsRedo.ToolTipText = "Redo the last change (Alt+R)";
            this.tsRedo.Click += new System.EventHandler(this.tsRedo_Click);
            // 
            // tsPreview
            // 
            this.tsPreview.AccessibleName = "tsEdit";
            this.tsPreview.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.tsPreview.Image = ((System.Drawing.Image)(resources.GetObject("tsPreview.Image")));
            this.tsPreview.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsPreview.Name = "tsPreview";
            this.tsPreview.Size = new System.Drawing.Size(66, 53);
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
            this.tsDefault.Name = "tsDefault";
            this.tsDefault.Size = new System.Drawing.Size(68, 53);
            this.tsDefault.Text = "De&faults";
            this.tsDefault.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsDefault.ToolTipText = "Select the Default Settings for the Print Via dialog (Alt+F)";
            this.tsDefault.Click += new System.EventHandler(this.tsDefault_Click);
            // 
            // tsSend
            // 
            this.tsSend.AccessibleName = "tsExport";
            this.tsSend.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.tsSend.Image = ((System.Drawing.Image)(resources.GetObject("tsSend.Image")));
            this.tsSend.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsSend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSend.Name = "tsSend";
            this.tsSend.Size = new System.Drawing.Size(47, 53);
            this.tsSend.Text = "S&end";
            this.tsSend.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsSend.ToolTipText = "Send the stylesheets and settings to someone else (Alt+E)";
            this.tsSend.Click += new System.EventHandler(this.tsSend_Click);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.toolStripSplitButton1.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(55, 53);
            this.toolStripSplitButton1.Text = "&Help";
            this.toolStripSplitButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.contentsToolStripMenuItem.Text = "Help";
            this.contentsToolStripMenuItem.Click += new System.EventHandler(this.contentsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabInfo);
            this.tabControl1.Controls.Add(this.tabDisplay);
            this.tabControl1.Controls.Add(this.tabMobile);
            this.tabControl1.Controls.Add(this.tabOthers);
            this.tabControl1.Controls.Add(this.tabWeb);
            this.tabControl1.Controls.Add(this.tabPreview);
            this.tabControl1.Controls.Add(this.tabPicture);
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(389, 625);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabInfo
            // 
            this.tabInfo.AutoScroll = true;
            this.tabInfo.Controls.Add(this.txtApproved);
            this.tabInfo.Controls.Add(this.lblApproved);
            this.tabInfo.Controls.Add(this.btnApproved);
            this.tabInfo.Controls.Add(this.lblComment);
            this.tabInfo.Controls.Add(this.chkAvailable);
            this.tabInfo.Controls.Add(this.txtComment);
            this.tabInfo.Controls.Add(this.lblAvailable);
            this.tabInfo.Controls.Add(this.txtDesc);
            this.tabInfo.Controls.Add(this.lblDesc);
            this.tabInfo.Controls.Add(this.txtName);
            this.tabInfo.Controls.Add(this.lblName);
            this.tabInfo.Location = new System.Drawing.Point(4, 25);
            this.tabInfo.Margin = new System.Windows.Forms.Padding(4);
            this.tabInfo.Name = "tabInfo";
            this.tabInfo.Padding = new System.Windows.Forms.Padding(4);
            this.tabInfo.Size = new System.Drawing.Size(381, 596);
            this.tabInfo.TabIndex = 0;
            this.tabInfo.Text = "Info";
            this.tabInfo.UseVisualStyleBackColor = true;
            // 
            // txtApproved
            // 
            this.txtApproved.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApproved.Enabled = false;
            this.txtApproved.Location = new System.Drawing.Point(96, 272);
            this.txtApproved.Margin = new System.Windows.Forms.Padding(4);
            this.txtApproved.MaxLength = 10;
            this.txtApproved.Name = "txtApproved";
            this.txtApproved.Size = new System.Drawing.Size(252, 22);
            this.txtApproved.TabIndex = 5;
            this.txtApproved.Enter += new System.EventHandler(this.SetGotFocusValue);
            this.txtApproved.Validated += new System.EventHandler(this.txtApproved_Validated);
            // 
            // lblApproved
            // 
            this.lblApproved.AutoSize = true;
            this.lblApproved.Location = new System.Drawing.Point(3, 277);
            this.lblApproved.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblApproved.Name = "lblApproved";
            this.lblApproved.Size = new System.Drawing.Size(89, 17);
            this.lblApproved.TabIndex = 11;
            this.lblApproved.Text = "Approved By";
            // 
            // btnApproved
            // 
            this.btnApproved.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApproved.Location = new System.Drawing.Point(317, 308);
            this.btnApproved.Margin = new System.Windows.Forms.Padding(4);
            this.btnApproved.Name = "btnApproved";
            this.btnApproved.Size = new System.Drawing.Size(32, 25);
            this.btnApproved.TabIndex = 6;
            this.btnApproved.Text = "...";
            this.btnApproved.UseVisualStyleBackColor = true;
            this.btnApproved.Visible = false;
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(25, 177);
            this.lblComment.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(67, 17);
            this.lblComment.TabIndex = 9;
            this.lblComment.Text = "Comment";
            // 
            // chkAvailable
            // 
            this.chkAvailable.AutoSize = true;
            this.chkAvailable.Location = new System.Drawing.Point(96, 153);
            this.chkAvailable.Margin = new System.Windows.Forms.Padding(4);
            this.chkAvailable.Name = "chkAvailable";
            this.chkAvailable.Size = new System.Drawing.Size(18, 17);
            this.chkAvailable.TabIndex = 3;
            this.chkAvailable.UseVisualStyleBackColor = true;
            this.chkAvailable.CheckedChanged += new System.EventHandler(this.chkAvailable_CheckedChanged);
            this.chkAvailable.CheckStateChanged += new System.EventHandler(this.chkAvailable_CheckStateChanged);
            this.chkAvailable.Enter += new System.EventHandler(this.SetGotFocusValue);
            this.chkAvailable.Validated += new System.EventHandler(this.chkAvailable_Validated);
            // 
            // txtComment
            // 
            this.txtComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComment.Location = new System.Drawing.Point(96, 174);
            this.txtComment.Margin = new System.Windows.Forms.Padding(4);
            this.txtComment.MaxLength = 250;
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtComment.Size = new System.Drawing.Size(252, 91);
            this.txtComment.TabIndex = 4;
            this.txtComment.Enter += new System.EventHandler(this.SetGotFocusValue);
            this.txtComment.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtComment_KeyUp);
            this.txtComment.Validated += new System.EventHandler(this.txtComment_Validated);
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoSize = true;
            this.lblAvailable.Location = new System.Drawing.Point(27, 153);
            this.lblAvailable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(65, 17);
            this.lblAvailable.TabIndex = 4;
            this.lblAvailable.Text = "Available";
            // 
            // txtDesc
            // 
            this.txtDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDesc.Location = new System.Drawing.Point(96, 54);
            this.txtDesc.Margin = new System.Windows.Forms.Padding(4);
            this.txtDesc.MaxLength = 250;
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDesc.Size = new System.Drawing.Size(252, 91);
            this.txtDesc.TabIndex = 2;
            this.txtDesc.Enter += new System.EventHandler(this.SetGotFocusValue);
            this.txtDesc.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDesc_KeyUp);
            this.txtDesc.Validated += new System.EventHandler(this.txtDesc_Validated);
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Location = new System.Drawing.Point(13, 58);
            this.lblDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(79, 17);
            this.lblDesc.TabIndex = 2;
            this.lblDesc.Text = "Description";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(96, 26);
            this.txtName.Margin = new System.Windows.Forms.Padding(4);
            this.txtName.MaxLength = 50;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(252, 22);
            this.txtName.TabIndex = 1;
            this.txtName.Enter += new System.EventHandler(this.txtName_Enter);
            this.txtName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyUp);
            this.txtName.Validating += new System.ComponentModel.CancelEventHandler(this.txtName_Validating);
            this.txtName.Validated += new System.EventHandler(this.txtName_Validated);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(47, 30);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(45, 17);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // tabDisplay
            // 
            this.tabDisplay.AutoScroll = true;
            this.tabDisplay.Controls.Add(this.LblPageNumber);
            this.tabDisplay.Controls.Add(this.ddlPageNumber);
            this.tabDisplay.Controls.Add(this.ddlFileProduceDict);
            this.tabDisplay.Controls.Add(this.label9);
            this.tabDisplay.Controls.Add(this.ddlVerticalJustify);
            this.tabDisplay.Controls.Add(this.lblVerticalJustify);
            this.tabDisplay.Controls.Add(this.ddlSense);
            this.tabDisplay.Controls.Add(this.lblSenseLayout);
            this.tabDisplay.Controls.Add(this.ddlPicture);
            this.tabDisplay.Controls.Add(this.lblLineSpace);
            this.tabDisplay.Controls.Add(this.lblRunningHeader);
            this.tabDisplay.Controls.Add(this.ddlLeading);
            this.tabDisplay.Controls.Add(this.ddlRunningHead);
            this.tabDisplay.Controls.Add(this.lblRules);
            this.tabDisplay.Controls.Add(this.ddlRules);
            this.tabDisplay.Controls.Add(this.lblFont);
            this.tabDisplay.Controls.Add(this.ddlFontSize);
            this.tabDisplay.Controls.Add(this.label4);
            this.tabDisplay.Controls.Add(this.txtPageGutterWidth);
            this.tabDisplay.Controls.Add(this.lblPageGutter);
            this.tabDisplay.Controls.Add(this.label5);
            this.tabDisplay.Controls.Add(this.txtPageOutside);
            this.tabDisplay.Controls.Add(this.txtPageInside);
            this.tabDisplay.Controls.Add(this.txtPageTop);
            this.tabDisplay.Controls.Add(this.txtPageBottom);
            this.tabDisplay.Controls.Add(this.lblPageInside);
            this.tabDisplay.Controls.Add(this.lblPageOutside);
            this.tabDisplay.Controls.Add(this.lblPageBottom);
            this.tabDisplay.Controls.Add(this.lblPageTop);
            this.tabDisplay.Controls.Add(this.ddlJustified);
            this.tabDisplay.Controls.Add(this.lblJustified);
            this.tabDisplay.Controls.Add(this.ddlPageColumn);
            this.tabDisplay.Controls.Add(this.lblPageColumn);
            this.tabDisplay.Controls.Add(this.ddlPagePageSize);
            this.tabDisplay.Controls.Add(this.lblPagePageSize);
            this.tabDisplay.Location = new System.Drawing.Point(4, 25);
            this.tabDisplay.Margin = new System.Windows.Forms.Padding(4);
            this.tabDisplay.Name = "tabDisplay";
            this.tabDisplay.Padding = new System.Windows.Forms.Padding(4);
            this.tabDisplay.Size = new System.Drawing.Size(381, 596);
            this.tabDisplay.TabIndex = 1;
            this.tabDisplay.Text = "Properties";
            this.tabDisplay.UseVisualStyleBackColor = true;
            // 
            // LblPageNumber
            // 
            this.LblPageNumber.AccessibleName = "LblPageNumber";
            this.LblPageNumber.Location = new System.Drawing.Point(0, 322);
            this.LblPageNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblPageNumber.Name = "LblPageNumber";
            this.LblPageNumber.Size = new System.Drawing.Size(116, 16);
            this.LblPageNumber.TabIndex = 98;
            this.LblPageNumber.Text = "Page Numbers";
            this.LblPageNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlPageNumber
            // 
            this.ddlPageNumber.AccessibleName = "ddlPageNumber";
            this.ddlPageNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlPageNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPageNumber.FormattingEnabled = true;
            this.ddlPageNumber.Location = new System.Drawing.Point(124, 319);
            this.ddlPageNumber.Margin = new System.Windows.Forms.Padding(4);
            this.ddlPageNumber.Name = "ddlPageNumber";
            this.ddlPageNumber.Size = new System.Drawing.Size(200, 24);
            this.ddlPageNumber.TabIndex = 97;
            this.ddlPageNumber.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
            // 
            // ddlFileProduceDict
            // 
            this.ddlFileProduceDict.AccessibleName = "";
            this.ddlFileProduceDict.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlFileProduceDict.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFileProduceDict.FormattingEnabled = true;
            this.ddlFileProduceDict.Location = new System.Drawing.Point(124, 406);
            this.ddlFileProduceDict.Margin = new System.Windows.Forms.Padding(4);
            this.ddlFileProduceDict.Name = "ddlFileProduceDict";
            this.ddlFileProduceDict.Size = new System.Drawing.Size(200, 24);
            this.ddlFileProduceDict.TabIndex = 15;
            this.ddlFileProduceDict.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
            this.ddlFileProduceDict.Validated += new System.EventHandler(this.ddlFileProduceDict_Validated);
            // 
            // label9
            // 
            this.label9.AccessibleName = "lblPageColumn";
            this.label9.Location = new System.Drawing.Point(0, 411);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(116, 16);
            this.label9.TabIndex = 96;
            this.label9.Text = "Files Produced";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlVerticalJustify
            // 
            this.ddlVerticalJustify.AccessibleName = "ddlPageColumn";
            this.ddlVerticalJustify.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlVerticalJustify.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlVerticalJustify.FormattingEnabled = true;
            this.ddlVerticalJustify.Location = new System.Drawing.Point(124, 201);
            this.ddlVerticalJustify.Margin = new System.Windows.Forms.Padding(4);
            this.ddlVerticalJustify.Name = "ddlVerticalJustify";
            this.ddlVerticalJustify.Size = new System.Drawing.Size(200, 24);
            this.ddlVerticalJustify.TabIndex = 9;
            this.ddlVerticalJustify.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
            // 
            // lblVerticalJustify
            // 
            this.lblVerticalJustify.AccessibleName = "lblPageColumn";
            this.lblVerticalJustify.Location = new System.Drawing.Point(0, 204);
            this.lblVerticalJustify.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVerticalJustify.Name = "lblVerticalJustify";
            this.lblVerticalJustify.Size = new System.Drawing.Size(116, 16);
            this.lblVerticalJustify.TabIndex = 94;
            this.lblVerticalJustify.Text = "Vertical Justify";
            this.lblVerticalJustify.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlSense
            // 
            this.ddlSense.AccessibleName = "ddlPageColumn";
            this.ddlSense.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlSense.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSense.FormattingEnabled = true;
            this.ddlSense.Location = new System.Drawing.Point(124, 436);
            this.ddlSense.Margin = new System.Windows.Forms.Padding(4);
            this.ddlSense.Name = "ddlSense";
            this.ddlSense.Size = new System.Drawing.Size(200, 24);
            this.ddlSense.TabIndex = 16;
            this.ddlSense.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
            // 
            // lblSenseLayout
            // 
            this.lblSenseLayout.AccessibleName = "lblPageColumn";
            this.lblSenseLayout.Location = new System.Drawing.Point(0, 441);
            this.lblSenseLayout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSenseLayout.Name = "lblSenseLayout";
            this.lblSenseLayout.Size = new System.Drawing.Size(116, 16);
            this.lblSenseLayout.TabIndex = 92;
            this.lblSenseLayout.Text = "Sense Layout";
            this.lblSenseLayout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlPicture
            // 
            this.ddlPicture.AccessibleName = "ddlPageColumn";
            this.ddlPicture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlPicture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPicture.FormattingEnabled = true;
            this.ddlPicture.Location = new System.Drawing.Point(124, 230);
            this.ddlPicture.Margin = new System.Windows.Forms.Padding(4);
            this.ddlPicture.Name = "ddlPicture";
            this.ddlPicture.Size = new System.Drawing.Size(200, 24);
            this.ddlPicture.TabIndex = 10;
            this.ddlPicture.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
            // 
            // lblLineSpace
            // 
            this.lblLineSpace.AccessibleName = "label26";
            this.lblLineSpace.Location = new System.Drawing.Point(0, 263);
            this.lblLineSpace.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLineSpace.Name = "lblLineSpace";
            this.lblLineSpace.Size = new System.Drawing.Size(116, 16);
            this.lblLineSpace.TabIndex = 86;
            this.lblLineSpace.Text = "Line Spacing ";
            this.lblLineSpace.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRunningHeader
            // 
            this.lblRunningHeader.AccessibleName = "label17";
            this.lblRunningHeader.Location = new System.Drawing.Point(0, 293);
            this.lblRunningHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRunningHeader.Name = "lblRunningHeader";
            this.lblRunningHeader.Size = new System.Drawing.Size(116, 16);
            this.lblRunningHeader.TabIndex = 87;
            this.lblRunningHeader.Text = "Running Header";
            this.lblRunningHeader.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlLeading
            // 
            this.ddlLeading.AccessibleName = "ddlPageColumn";
            this.ddlLeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlLeading.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLeading.FormattingEnabled = true;
            this.ddlLeading.Location = new System.Drawing.Point(124, 260);
            this.ddlLeading.Margin = new System.Windows.Forms.Padding(4);
            this.ddlLeading.Name = "ddlLeading";
            this.ddlLeading.Size = new System.Drawing.Size(200, 24);
            this.ddlLeading.TabIndex = 11;
            this.ddlLeading.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
            // 
            // ddlRunningHead
            // 
            this.ddlRunningHead.AccessibleName = "ddlPageColumn";
            this.ddlRunningHead.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlRunningHead.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRunningHead.FormattingEnabled = true;
            this.ddlRunningHead.Location = new System.Drawing.Point(124, 289);
            this.ddlRunningHead.Margin = new System.Windows.Forms.Padding(4);
            this.ddlRunningHead.Name = "ddlRunningHead";
            this.ddlRunningHead.Size = new System.Drawing.Size(200, 24);
            this.ddlRunningHead.TabIndex = 12;
            this.ddlRunningHead.SelectedIndexChanged += new System.EventHandler(this.ddlRunningHead_SelectedIndexChanged);
            // 
            // lblRules
            // 
            this.lblRules.AccessibleName = "label17";
            this.lblRules.Location = new System.Drawing.Point(0, 351);
            this.lblRules.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRules.Name = "lblRules";
            this.lblRules.Size = new System.Drawing.Size(116, 16);
            this.lblRules.TabIndex = 88;
            this.lblRules.Text = "Divider Lines";
            this.lblRules.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlRules
            // 
            this.ddlRules.AccessibleName = "ddlPageColumn";
            this.ddlRules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlRules.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRules.FormattingEnabled = true;
            this.ddlRules.Location = new System.Drawing.Point(124, 347);
            this.ddlRules.Margin = new System.Windows.Forms.Padding(4);
            this.ddlRules.Name = "ddlRules";
            this.ddlRules.Size = new System.Drawing.Size(200, 24);
            this.ddlRules.TabIndex = 13;
            this.ddlRules.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
            // 
            // lblFont
            // 
            this.lblFont.AccessibleName = "label17";
            this.lblFont.Location = new System.Drawing.Point(0, 382);
            this.lblFont.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(116, 16);
            this.lblFont.TabIndex = 89;
            this.lblFont.Text = "BaseFont Size";
            this.lblFont.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlFontSize
            // 
            this.ddlFontSize.AccessibleName = "ddlPageColumn";
            this.ddlFontSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFontSize.FormattingEnabled = true;
            this.ddlFontSize.Location = new System.Drawing.Point(124, 377);
            this.ddlFontSize.Margin = new System.Windows.Forms.Padding(4);
            this.ddlFontSize.Name = "ddlFontSize";
            this.ddlFontSize.Size = new System.Drawing.Size(200, 24);
            this.ddlFontSize.TabIndex = 14;
            this.ddlFontSize.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
            // 
            // label4
            // 
            this.label4.AccessibleName = "lblPageColumn";
            this.label4.Location = new System.Drawing.Point(0, 234);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 16);
            this.label4.TabIndex = 90;
            this.label4.Text = "Pictures";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPageGutterWidth
            // 
            this.txtPageGutterWidth.AccessibleName = "txtPageGutterWidth";
            this.txtPageGutterWidth.Location = new System.Drawing.Point(124, 140);
            this.txtPageGutterWidth.Margin = new System.Windows.Forms.Padding(4);
            this.txtPageGutterWidth.MaxLength = 6;
            this.txtPageGutterWidth.Name = "txtPageGutterWidth";
            this.txtPageGutterWidth.Size = new System.Drawing.Size(57, 22);
            this.txtPageGutterWidth.TabIndex = 7;
            this.txtPageGutterWidth.Tag = "Gutter Width";
            this.txtPageGutterWidth.Enter += new System.EventHandler(this.SetGotFocusValue);
            this.txtPageGutterWidth.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPageGutterWidth_KeyUp);
            this.txtPageGutterWidth.Validated += new System.EventHandler(this.txtPageGutterWidth_Validated);
            // 
            // lblPageGutter
            // 
            this.lblPageGutter.AccessibleName = "lblPageGutter";
            this.lblPageGutter.Location = new System.Drawing.Point(0, 144);
            this.lblPageGutter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPageGutter.Name = "lblPageGutter";
            this.lblPageGutter.Size = new System.Drawing.Size(116, 16);
            this.lblPageGutter.TabIndex = 79;
            this.lblPageGutter.Text = "Column Gap";
            this.lblPageGutter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AccessibleName = "lblPagePageSize";
            this.label5.Location = new System.Drawing.Point(23, 55);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 25);
            this.label5.TabIndex = 78;
            this.label5.Text = "Page Margin";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPageOutside
            // 
            this.txtPageOutside.AccessibleName = "txtPageOutside";
            this.txtPageOutside.Location = new System.Drawing.Point(281, 47);
            this.txtPageOutside.Margin = new System.Windows.Forms.Padding(4);
            this.txtPageOutside.MaxLength = 6;
            this.txtPageOutside.Name = "txtPageOutside";
            this.txtPageOutside.Size = new System.Drawing.Size(43, 22);
            this.txtPageOutside.TabIndex = 3;
            this.txtPageOutside.Tag = "Outside";
            this.txtPageOutside.Enter += new System.EventHandler(this.SetGotFocusValue);
            this.txtPageOutside.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPageOutside_KeyUp);
            this.txtPageOutside.Validated += new System.EventHandler(this.txtPageOutside_Validated);
            // 
            // txtPageInside
            // 
            this.txtPageInside.AccessibleName = "txtPageInside";
            this.txtPageInside.Location = new System.Drawing.Point(167, 47);
            this.txtPageInside.Margin = new System.Windows.Forms.Padding(4);
            this.txtPageInside.MaxLength = 6;
            this.txtPageInside.Name = "txtPageInside";
            this.txtPageInside.Size = new System.Drawing.Size(43, 22);
            this.txtPageInside.TabIndex = 2;
            this.txtPageInside.Tag = "Inside";
            this.txtPageInside.Enter += new System.EventHandler(this.SetGotFocusValue);
            this.txtPageInside.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPageInside_KeyUp);
            this.txtPageInside.Validated += new System.EventHandler(this.txtPageInside_Validated);
            // 
            // txtPageTop
            // 
            this.txtPageTop.AccessibleName = "txtPageTop";
            this.txtPageTop.Location = new System.Drawing.Point(168, 79);
            this.txtPageTop.Margin = new System.Windows.Forms.Padding(4);
            this.txtPageTop.MaxLength = 6;
            this.txtPageTop.Name = "txtPageTop";
            this.txtPageTop.Size = new System.Drawing.Size(43, 22);
            this.txtPageTop.TabIndex = 4;
            this.txtPageTop.Tag = "Top";
            this.txtPageTop.Enter += new System.EventHandler(this.SetGotFocusValue);
            this.txtPageTop.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPageTop_KeyUp);
            this.txtPageTop.Validated += new System.EventHandler(this.txtPageTop_Validated);
            // 
            // txtPageBottom
            // 
            this.txtPageBottom.AccessibleName = "txtPageBottom";
            this.txtPageBottom.Location = new System.Drawing.Point(281, 79);
            this.txtPageBottom.Margin = new System.Windows.Forms.Padding(4);
            this.txtPageBottom.MaxLength = 6;
            this.txtPageBottom.Name = "txtPageBottom";
            this.txtPageBottom.Size = new System.Drawing.Size(43, 22);
            this.txtPageBottom.TabIndex = 5;
            this.txtPageBottom.Tag = "Bottom";
            this.txtPageBottom.Enter += new System.EventHandler(this.SetGotFocusValue);
            this.txtPageBottom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPageBottom_KeyUp);
            this.txtPageBottom.Validated += new System.EventHandler(this.txtPageBottom_Validated);
            // 
            // lblPageInside
            // 
            this.lblPageInside.AccessibleName = "lblPageInside";
            this.lblPageInside.Location = new System.Drawing.Point(113, 50);
            this.lblPageInside.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPageInside.Name = "lblPageInside";
            this.lblPageInside.Size = new System.Drawing.Size(53, 16);
            this.lblPageInside.TabIndex = 77;
            this.lblPageInside.Text = "Inside";
            this.lblPageInside.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPageOutside
            // 
            this.lblPageOutside.AccessibleName = "lblPageOutside";
            this.lblPageOutside.Location = new System.Drawing.Point(216, 50);
            this.lblPageOutside.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPageOutside.Name = "lblPageOutside";
            this.lblPageOutside.Size = new System.Drawing.Size(65, 16);
            this.lblPageOutside.TabIndex = 75;
            this.lblPageOutside.Text = "Outside";
            this.lblPageOutside.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPageBottom
            // 
            this.lblPageBottom.AccessibleName = "lblPageBottom";
            this.lblPageBottom.Location = new System.Drawing.Point(219, 82);
            this.lblPageBottom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPageBottom.Name = "lblPageBottom";
            this.lblPageBottom.Size = new System.Drawing.Size(63, 16);
            this.lblPageBottom.TabIndex = 73;
            this.lblPageBottom.Text = "Bottom";
            this.lblPageBottom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPageTop
            // 
            this.lblPageTop.AccessibleName = "lblPageTop";
            this.lblPageTop.Location = new System.Drawing.Point(133, 82);
            this.lblPageTop.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPageTop.Name = "lblPageTop";
            this.lblPageTop.Size = new System.Drawing.Size(35, 16);
            this.lblPageTop.TabIndex = 71;
            this.lblPageTop.Text = "Top";
            this.lblPageTop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlJustified
            // 
            this.ddlJustified.AccessibleName = "ddlPageColumn";
            this.ddlJustified.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlJustified.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlJustified.FormattingEnabled = true;
            this.ddlJustified.Location = new System.Drawing.Point(124, 170);
            this.ddlJustified.Margin = new System.Windows.Forms.Padding(4);
            this.ddlJustified.Name = "ddlJustified";
            this.ddlJustified.Size = new System.Drawing.Size(200, 24);
            this.ddlJustified.TabIndex = 8;
            this.ddlJustified.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
            this.ddlJustified.Enter += new System.EventHandler(this.SetGotFocusValue);
            // 
            // lblJustified
            // 
            this.lblJustified.AccessibleName = "lblPageColumn";
            this.lblJustified.Location = new System.Drawing.Point(0, 174);
            this.lblJustified.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblJustified.Name = "lblJustified";
            this.lblJustified.Size = new System.Drawing.Size(116, 16);
            this.lblJustified.TabIndex = 64;
            this.lblJustified.Text = "Justified";
            this.lblJustified.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlPageColumn
            // 
            this.ddlPageColumn.AccessibleName = "ddlPageColumn";
            this.ddlPageColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlPageColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPageColumn.FormattingEnabled = true;
            this.ddlPageColumn.Location = new System.Drawing.Point(124, 111);
            this.ddlPageColumn.Margin = new System.Windows.Forms.Padding(4);
            this.ddlPageColumn.Name = "ddlPageColumn";
            this.ddlPageColumn.Size = new System.Drawing.Size(200, 24);
            this.ddlPageColumn.TabIndex = 6;
            this.ddlPageColumn.SelectedIndexChanged += new System.EventHandler(this.ddlPageColumn_SelectedIndexChanged);
            this.ddlPageColumn.Enter += new System.EventHandler(this.SetGotFocusValue);
            // 
            // lblPageColumn
            // 
            this.lblPageColumn.AccessibleName = "lblPageColumn";
            this.lblPageColumn.Location = new System.Drawing.Point(0, 114);
            this.lblPageColumn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPageColumn.Name = "lblPageColumn";
            this.lblPageColumn.Size = new System.Drawing.Size(116, 16);
            this.lblPageColumn.TabIndex = 42;
            this.lblPageColumn.Text = "Columns";
            this.lblPageColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlPagePageSize
            // 
            this.ddlPagePageSize.AccessibleName = "ddlPagePageSize";
            this.ddlPagePageSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlPagePageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPagePageSize.FormattingEnabled = true;
            this.ddlPagePageSize.Location = new System.Drawing.Point(124, 14);
            this.ddlPagePageSize.Margin = new System.Windows.Forms.Padding(4);
            this.ddlPagePageSize.Name = "ddlPagePageSize";
            this.ddlPagePageSize.Size = new System.Drawing.Size(223, 24);
            this.ddlPagePageSize.TabIndex = 1;
            this.ddlPagePageSize.SelectedIndexChanged += new System.EventHandler(this.EditCSS);
            this.ddlPagePageSize.Enter += new System.EventHandler(this.SetGotFocusValue);
            // 
            // lblPagePageSize
            // 
            this.lblPagePageSize.AccessibleName = "lblPagePageSize";
            this.lblPagePageSize.Location = new System.Drawing.Point(0, 17);
            this.lblPagePageSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPagePageSize.Name = "lblPagePageSize";
            this.lblPagePageSize.Size = new System.Drawing.Size(116, 16);
            this.lblPagePageSize.TabIndex = 33;
            this.lblPagePageSize.Text = "Page Size";
            this.lblPagePageSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabMobile
            // 
            this.tabMobile.AutoScroll = true;
            this.tabMobile.Controls.Add(this.lblMobileOptionsSection);
            this.tabMobile.Controls.Add(this.lblGoBibleDescription);
            this.tabMobile.Controls.Add(this.btnBrowse);
            this.tabMobile.Controls.Add(this.label1);
            this.tabMobile.Controls.Add(this.ddlRedLetter);
            this.tabMobile.Controls.Add(this.label7);
            this.tabMobile.Controls.Add(this.ddlFiles);
            this.tabMobile.Controls.Add(this.label8);
            this.tabMobile.Controls.Add(this.pictureBox4);
            this.tabMobile.Controls.Add(this.mobileIcon);
            this.tabMobile.Location = new System.Drawing.Point(4, 25);
            this.tabMobile.Margin = new System.Windows.Forms.Padding(4);
            this.tabMobile.Name = "tabMobile";
            this.tabMobile.Padding = new System.Windows.Forms.Padding(4);
            this.tabMobile.Size = new System.Drawing.Size(381, 596);
            this.tabMobile.TabIndex = 2;
            this.tabMobile.Text = "Properties";
            this.tabMobile.UseVisualStyleBackColor = true;
            // 
            // lblMobileOptionsSection
            // 
            this.lblMobileOptionsSection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMobileOptionsSection.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lblMobileOptionsSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMobileOptionsSection.Location = new System.Drawing.Point(9, 64);
            this.lblMobileOptionsSection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMobileOptionsSection.Name = "lblMobileOptionsSection";
            this.lblMobileOptionsSection.Size = new System.Drawing.Size(348, 28);
            this.lblMobileOptionsSection.TabIndex = 68;
            this.lblMobileOptionsSection.Text = "Options";
            this.lblMobileOptionsSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGoBibleDescription
            // 
            this.lblGoBibleDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGoBibleDescription.Location = new System.Drawing.Point(64, 20);
            this.lblGoBibleDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGoBibleDescription.Name = "lblGoBibleDescription";
            this.lblGoBibleDescription.Size = new System.Drawing.Size(288, 32);
            this.lblGoBibleDescription.TabIndex = 66;
            this.lblGoBibleDescription.Text = "Change the settings for mobile content.";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(184, 164);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(91, 28);
            this.btnBrowse.TabIndex = 10;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label1
            // 
            this.label1.AccessibleName = "lblPageColumn";
            this.label1.Location = new System.Drawing.Point(25, 170);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Icon for Phone";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlRedLetter
            // 
            this.ddlRedLetter.AccessibleName = "";
            this.ddlRedLetter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlRedLetter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRedLetter.FormattingEnabled = true;
            this.ddlRedLetter.Location = new System.Drawing.Point(149, 130);
            this.ddlRedLetter.Margin = new System.Windows.Forms.Padding(4);
            this.ddlRedLetter.Name = "ddlRedLetter";
            this.ddlRedLetter.Size = new System.Drawing.Size(207, 24);
            this.ddlRedLetter.TabIndex = 4;
            this.ddlRedLetter.SelectedIndexChanged += new System.EventHandler(this.ddlRedLetter_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AccessibleName = "lblPageColumn";
            this.label7.Location = new System.Drawing.Point(25, 134);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 16);
            this.label7.TabIndex = 3;
            this.label7.Text = "Red Letter";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlFiles
            // 
            this.ddlFiles.AccessibleName = "";
            this.ddlFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFiles.FormattingEnabled = true;
            this.ddlFiles.Location = new System.Drawing.Point(149, 96);
            this.ddlFiles.Margin = new System.Windows.Forms.Padding(4);
            this.ddlFiles.Name = "ddlFiles";
            this.ddlFiles.Size = new System.Drawing.Size(207, 24);
            this.ddlFiles.TabIndex = 2;
            this.ddlFiles.SelectedIndexChanged += new System.EventHandler(this.ddlFiles_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AccessibleName = "lblPageColumn";
            this.label8.Location = new System.Drawing.Point(25, 100);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(116, 16);
            this.label8.TabIndex = 1;
            this.label8.Text = "Files Produced";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = global::SIL.PublishingSolution.Properties.Resources.cell;
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox4.Location = new System.Drawing.Point(9, 9);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(43, 43);
            this.pictureBox4.TabIndex = 67;
            this.pictureBox4.TabStop = false;
            // 
            // mobileIcon
            // 
            this.mobileIcon.Image = ((System.Drawing.Image)(resources.GetObject("mobileIcon.Image")));
            this.mobileIcon.Location = new System.Drawing.Point(149, 166);
            this.mobileIcon.Margin = new System.Windows.Forms.Padding(4);
            this.mobileIcon.Name = "mobileIcon";
            this.mobileIcon.Size = new System.Drawing.Size(27, 25);
            this.mobileIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.mobileIcon.TabIndex = 65;
            this.mobileIcon.TabStop = false;
            // 
            // tabOthers
            // 
            this.tabOthers.AutoScroll = true;
            this.tabOthers.Controls.Add(this.ddlReferences);
            this.tabOthers.Controls.Add(this.lblReferences);
            this.tabOthers.Controls.Add(this.picFonts);
            this.tabOthers.Controls.Add(this.pictureBox2);
            this.tabOthers.Controls.Add(this.ddlNonSILFont);
            this.tabOthers.Controls.Add(this.lblEpubFontsSection);
            this.tabOthers.Controls.Add(this.ddlMissingFont);
            this.tabOthers.Controls.Add(this.ddlDefaultAlignment);
            this.tabOthers.Controls.Add(this.lblNonSILFont);
            this.tabOthers.Controls.Add(this.lblEputLayoutSection);
            this.tabOthers.Controls.Add(this.lblMissingFont);
            this.tabOthers.Controls.Add(this.lblDefaultAlignment);
            this.tabOthers.Controls.Add(this.ddlDefaultFont);
            this.tabOthers.Controls.Add(this.lblDefaultFont);
            this.tabOthers.Controls.Add(this.lblPx);
            this.tabOthers.Controls.Add(this.chkIncludeFontVariants);
            this.tabOthers.Controls.Add(this.lblEpubDescription);
            this.tabOthers.Controls.Add(this.chkEmbedFonts);
            this.tabOthers.Controls.Add(this.txtMaxImageWidth);
            this.tabOthers.Controls.Add(this.ddlTocLevel);
            this.tabOthers.Controls.Add(this.lblTocLevel);
            this.tabOthers.Controls.Add(this.lblBaseFontSize);
            this.tabOthers.Controls.Add(this.lblMaxImageWidth);
            this.tabOthers.Controls.Add(this.txtBaseFontSize);
            this.tabOthers.Controls.Add(this.lblPt);
            this.tabOthers.Controls.Add(this.lblLineSpacing);
            this.tabOthers.Controls.Add(this.ddlChapterNumbers);
            this.tabOthers.Controls.Add(this.txtDefaultLineHeight);
            this.tabOthers.Controls.Add(this.lblChapterNumbers);
            this.tabOthers.Controls.Add(this.lblPct);
            this.tabOthers.Controls.Add(this.pictureBox1);
            this.tabOthers.Location = new System.Drawing.Point(4, 25);
            this.tabOthers.Margin = new System.Windows.Forms.Padding(4);
            this.tabOthers.Name = "tabOthers";
            this.tabOthers.Size = new System.Drawing.Size(381, 596);
            this.tabOthers.TabIndex = 3;
            this.tabOthers.Text = "Properties";
            this.tabOthers.UseVisualStyleBackColor = true;
            // 
            // ddlReferences
            // 
            this.ddlReferences.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlReferences.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlReferences.FormattingEnabled = true;
            this.ddlReferences.Location = new System.Drawing.Point(196, 292);
            this.ddlReferences.Margin = new System.Windows.Forms.Padding(4);
            this.ddlReferences.Name = "ddlReferences";
            this.ddlReferences.Size = new System.Drawing.Size(160, 24);
            this.ddlReferences.TabIndex = 14;
            this.ddlReferences.SelectedIndexChanged += new System.EventHandler(this.ddlReferences_SelectedIndexChanged);
            // 
            // lblReferences
            // 
            this.lblReferences.AutoSize = true;
            this.lblReferences.Location = new System.Drawing.Point(64, 295);
            this.lblReferences.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReferences.Name = "lblReferences";
            this.lblReferences.Size = new System.Drawing.Size(114, 17);
            this.lblReferences.TabIndex = 13;
            this.lblReferences.Text = "Add References:";
            // 
            // picFonts
            // 
            this.picFonts.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picFonts.BackgroundImage")));
            this.picFonts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picFonts.Location = new System.Drawing.Point(9, 358);
            this.picFonts.Margin = new System.Windows.Forms.Padding(4);
            this.picFonts.Name = "picFonts";
            this.picFonts.Size = new System.Drawing.Size(43, 36);
            this.picFonts.TabIndex = 38;
            this.picFonts.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::SIL.PublishingSolution.Properties.Resources.DocumentTools;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Location = new System.Drawing.Point(9, 96);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(43, 44);
            this.pictureBox2.TabIndex = 37;
            this.pictureBox2.TabStop = false;
            // 
            // ddlNonSILFont
            // 
            this.ddlNonSILFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlNonSILFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlNonSILFont.FormattingEnabled = true;
            this.ddlNonSILFont.Location = new System.Drawing.Point(184, 448);
            this.ddlNonSILFont.Margin = new System.Windows.Forms.Padding(4);
            this.ddlNonSILFont.Name = "ddlNonSILFont";
            this.ddlNonSILFont.Size = new System.Drawing.Size(172, 24);
            this.ddlNonSILFont.TabIndex = 20;
            this.ddlNonSILFont.SelectedIndexChanged += new System.EventHandler(this.ddlNonSILFont_SelectedIndexChanged);
            // 
            // lblEpubFontsSection
            // 
            this.lblEpubFontsSection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEpubFontsSection.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lblEpubFontsSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEpubFontsSection.Location = new System.Drawing.Point(9, 326);
            this.lblEpubFontsSection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEpubFontsSection.Name = "lblEpubFontsSection";
            this.lblEpubFontsSection.Size = new System.Drawing.Size(348, 28);
            this.lblEpubFontsSection.TabIndex = 36;
            this.lblEpubFontsSection.Text = "Embedded Fonts";
            this.lblEpubFontsSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ddlMissingFont
            // 
            this.ddlMissingFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlMissingFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlMissingFont.FormattingEnabled = true;
            this.ddlMissingFont.Location = new System.Drawing.Point(184, 415);
            this.ddlMissingFont.Margin = new System.Windows.Forms.Padding(4);
            this.ddlMissingFont.Name = "ddlMissingFont";
            this.ddlMissingFont.Size = new System.Drawing.Size(172, 24);
            this.ddlMissingFont.TabIndex = 18;
            this.ddlMissingFont.SelectedIndexChanged += new System.EventHandler(this.ddlMissingFont_SelectedIndexChanged);
            // 
            // ddlDefaultAlignment
            // 
            this.ddlDefaultAlignment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlDefaultAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDefaultAlignment.FormattingEnabled = true;
            this.ddlDefaultAlignment.Location = new System.Drawing.Point(196, 160);
            this.ddlDefaultAlignment.Margin = new System.Windows.Forms.Padding(4);
            this.ddlDefaultAlignment.Name = "ddlDefaultAlignment";
            this.ddlDefaultAlignment.Size = new System.Drawing.Size(160, 24);
            this.ddlDefaultAlignment.TabIndex = 6;
            this.ddlDefaultAlignment.SelectedIndexChanged += new System.EventHandler(this.ddlDefaultAlignment_SelectedIndexChanged);
            // 
            // lblNonSILFont
            // 
            this.lblNonSILFont.AutoSize = true;
            this.lblNonSILFont.Location = new System.Drawing.Point(64, 452);
            this.lblNonSILFont.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNonSILFont.Name = "lblNonSILFont";
            this.lblNonSILFont.Size = new System.Drawing.Size(114, 17);
            this.lblNonSILFont.TabIndex = 19;
            this.lblNonSILFont.Text = "If font is non-SIL:";
            // 
            // lblEputLayoutSection
            // 
            this.lblEputLayoutSection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEputLayoutSection.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.lblEputLayoutSection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEputLayoutSection.Location = new System.Drawing.Point(9, 64);
            this.lblEputLayoutSection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEputLayoutSection.Name = "lblEputLayoutSection";
            this.lblEputLayoutSection.Size = new System.Drawing.Size(348, 28);
            this.lblEputLayoutSection.TabIndex = 33;
            this.lblEputLayoutSection.Text = "Layout";
            this.lblEputLayoutSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMissingFont
            // 
            this.lblMissingFont.AutoSize = true;
            this.lblMissingFont.Location = new System.Drawing.Point(64, 418);
            this.lblMissingFont.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMissingFont.Name = "lblMissingFont";
            this.lblMissingFont.Size = new System.Drawing.Size(112, 17);
            this.lblMissingFont.TabIndex = 17;
            this.lblMissingFont.Text = "If font is missing:";
            // 
            // lblDefaultAlignment
            // 
            this.lblDefaultAlignment.AutoSize = true;
            this.lblDefaultAlignment.Location = new System.Drawing.Point(64, 164);
            this.lblDefaultAlignment.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefaultAlignment.Name = "lblDefaultAlignment";
            this.lblDefaultAlignment.Size = new System.Drawing.Size(105, 17);
            this.lblDefaultAlignment.TabIndex = 5;
            this.lblDefaultAlignment.Text = "Text Alignment:";
            // 
            // ddlDefaultFont
            // 
            this.ddlDefaultFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlDefaultFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDefaultFont.FormattingEnabled = true;
            this.ddlDefaultFont.Location = new System.Drawing.Point(184, 481);
            this.ddlDefaultFont.Margin = new System.Windows.Forms.Padding(4);
            this.ddlDefaultFont.Name = "ddlDefaultFont";
            this.ddlDefaultFont.Size = new System.Drawing.Size(172, 24);
            this.ddlDefaultFont.TabIndex = 22;
            this.ddlDefaultFont.SelectedIndexChanged += new System.EventHandler(this.ddlDefaultFont_SelectedIndexChanged);
            // 
            // lblDefaultFont
            // 
            this.lblDefaultFont.AutoSize = true;
            this.lblDefaultFont.Location = new System.Drawing.Point(64, 485);
            this.lblDefaultFont.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefaultFont.Name = "lblDefaultFont";
            this.lblDefaultFont.Size = new System.Drawing.Size(96, 17);
            this.lblDefaultFont.TabIndex = 21;
            this.lblDefaultFont.Text = "Fallback Font:";
            // 
            // lblPx
            // 
            this.lblPx.AutoSize = true;
            this.lblPx.Location = new System.Drawing.Point(292, 197);
            this.lblPx.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPx.Name = "lblPx";
            this.lblPx.Size = new System.Drawing.Size(22, 17);
            this.lblPx.TabIndex = 35;
            this.lblPx.Text = "px";
            // 
            // chkIncludeFontVariants
            // 
            this.chkIncludeFontVariants.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIncludeFontVariants.AutoSize = true;
            this.chkIncludeFontVariants.Location = new System.Drawing.Point(68, 386);
            this.chkIncludeFontVariants.Margin = new System.Windows.Forms.Padding(4);
            this.chkIncludeFontVariants.Name = "chkIncludeFontVariants";
            this.chkIncludeFontVariants.Size = new System.Drawing.Size(193, 21);
            this.chkIncludeFontVariants.TabIndex = 16;
            this.chkIncludeFontVariants.Text = "Also Embed Font Variants";
            this.chkIncludeFontVariants.UseVisualStyleBackColor = true;
            this.chkIncludeFontVariants.CheckedChanged += new System.EventHandler(this.chkIncludeFontVariants_CheckedChanged);
            // 
            // lblEpubDescription
            // 
            this.lblEpubDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEpubDescription.Location = new System.Drawing.Point(64, 20);
            this.lblEpubDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEpubDescription.Name = "lblEpubDescription";
            this.lblEpubDescription.Size = new System.Drawing.Size(288, 36);
            this.lblEpubDescription.TabIndex = 31;
            this.lblEpubDescription.Text = "Change the settings for e-book content.";
            // 
            // chkEmbedFonts
            // 
            this.chkEmbedFonts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkEmbedFonts.AutoSize = true;
            this.chkEmbedFonts.Location = new System.Drawing.Point(68, 358);
            this.chkEmbedFonts.Margin = new System.Windows.Forms.Padding(4);
            this.chkEmbedFonts.Name = "chkEmbedFonts";
            this.chkEmbedFonts.Size = new System.Drawing.Size(196, 21);
            this.chkEmbedFonts.TabIndex = 15;
            this.chkEmbedFonts.Text = "Embed Fonts in Document";
            this.chkEmbedFonts.UseVisualStyleBackColor = true;
            this.chkEmbedFonts.CheckedChanged += new System.EventHandler(this.chkEmbedFonts_CheckedChanged);
            // 
            // txtMaxImageWidth
            // 
            this.txtMaxImageWidth.Location = new System.Drawing.Point(224, 193);
            this.txtMaxImageWidth.Margin = new System.Windows.Forms.Padding(4);
            this.txtMaxImageWidth.MaxLength = 4;
            this.txtMaxImageWidth.Name = "txtMaxImageWidth";
            this.txtMaxImageWidth.Size = new System.Drawing.Size(59, 22);
            this.txtMaxImageWidth.TabIndex = 8;
            this.txtMaxImageWidth.Validated += new System.EventHandler(this.txtMaxImageWidth_Validated);
            // 
            // ddlTocLevel
            // 
            this.ddlTocLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlTocLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTocLevel.FormattingEnabled = true;
            this.ddlTocLevel.Location = new System.Drawing.Point(157, 225);
            this.ddlTocLevel.Margin = new System.Windows.Forms.Padding(4);
            this.ddlTocLevel.Name = "ddlTocLevel";
            this.ddlTocLevel.Size = new System.Drawing.Size(199, 24);
            this.ddlTocLevel.TabIndex = 10;
            this.ddlTocLevel.SelectedIndexChanged += new System.EventHandler(this.ddlTocLevel_SelectedIndexChanged);
            // 
            // lblTocLevel
            // 
            this.lblTocLevel.AutoSize = true;
            this.lblTocLevel.Location = new System.Drawing.Point(64, 229);
            this.lblTocLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTocLevel.Name = "lblTocLevel";
            this.lblTocLevel.Size = new System.Drawing.Size(79, 17);
            this.lblTocLevel.TabIndex = 9;
            this.lblTocLevel.Text = "TOC Level:";
            // 
            // lblBaseFontSize
            // 
            this.lblBaseFontSize.AutoSize = true;
            this.lblBaseFontSize.Location = new System.Drawing.Point(64, 100);
            this.lblBaseFontSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBaseFontSize.Name = "lblBaseFontSize";
            this.lblBaseFontSize.Size = new System.Drawing.Size(71, 17);
            this.lblBaseFontSize.TabIndex = 1;
            this.lblBaseFontSize.Text = "Font Size:";
            this.lblBaseFontSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMaxImageWidth
            // 
            this.lblMaxImageWidth.AutoSize = true;
            this.lblMaxImageWidth.Location = new System.Drawing.Point(64, 197);
            this.lblMaxImageWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaxImageWidth.Name = "lblMaxImageWidth";
            this.lblMaxImageWidth.Size = new System.Drawing.Size(152, 17);
            this.lblMaxImageWidth.TabIndex = 7;
            this.lblMaxImageWidth.Text = "Maximum Image Width:";
            this.lblMaxImageWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBaseFontSize
            // 
            this.txtBaseFontSize.Location = new System.Drawing.Point(196, 96);
            this.txtBaseFontSize.Margin = new System.Windows.Forms.Padding(4);
            this.txtBaseFontSize.MaxLength = 2;
            this.txtBaseFontSize.Name = "txtBaseFontSize";
            this.txtBaseFontSize.Size = new System.Drawing.Size(59, 22);
            this.txtBaseFontSize.TabIndex = 2;
            this.txtBaseFontSize.Validated += new System.EventHandler(this.txtBaseFontSize_Validated);
            // 
            // lblPt
            // 
            this.lblPt.AutoSize = true;
            this.lblPt.Location = new System.Drawing.Point(264, 100);
            this.lblPt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPt.Name = "lblPt";
            this.lblPt.Size = new System.Drawing.Size(20, 17);
            this.lblPt.TabIndex = 9;
            this.lblPt.Text = "pt";
            // 
            // lblLineSpacing
            // 
            this.lblLineSpacing.AutoSize = true;
            this.lblLineSpacing.Location = new System.Drawing.Point(64, 132);
            this.lblLineSpacing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLineSpacing.Name = "lblLineSpacing";
            this.lblLineSpacing.Size = new System.Drawing.Size(84, 17);
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
            this.ddlChapterNumbers.Location = new System.Drawing.Point(196, 258);
            this.ddlChapterNumbers.Margin = new System.Windows.Forms.Padding(4);
            this.ddlChapterNumbers.Name = "ddlChapterNumbers";
            this.ddlChapterNumbers.Size = new System.Drawing.Size(160, 24);
            this.ddlChapterNumbers.TabIndex = 12;
            this.ddlChapterNumbers.SelectedIndexChanged += new System.EventHandler(this.ddlChapterNumbers_SelectedIndexChanged);
            // 
            // txtDefaultLineHeight
            // 
            this.txtDefaultLineHeight.Location = new System.Drawing.Point(196, 128);
            this.txtDefaultLineHeight.Margin = new System.Windows.Forms.Padding(4);
            this.txtDefaultLineHeight.MaxLength = 3;
            this.txtDefaultLineHeight.Name = "txtDefaultLineHeight";
            this.txtDefaultLineHeight.Size = new System.Drawing.Size(59, 22);
            this.txtDefaultLineHeight.TabIndex = 4;
            this.txtDefaultLineHeight.Validated += new System.EventHandler(this.txtDefaultLineHeight_Validated);
            // 
            // lblChapterNumbers
            // 
            this.lblChapterNumbers.AutoSize = true;
            this.lblChapterNumbers.Location = new System.Drawing.Point(64, 262);
            this.lblChapterNumbers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChapterNumbers.Name = "lblChapterNumbers";
            this.lblChapterNumbers.Size = new System.Drawing.Size(123, 17);
            this.lblChapterNumbers.TabIndex = 11;
            this.lblChapterNumbers.Text = "Chapter Numbers:";
            this.lblChapterNumbers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPct
            // 
            this.lblPct.AutoSize = true;
            this.lblPct.Location = new System.Drawing.Point(264, 132);
            this.lblPct.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPct.Name = "lblPct";
            this.lblPct.Size = new System.Drawing.Size(20, 17);
            this.lblPct.TabIndex = 4;
            this.lblPct.Text = "%";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::SIL.PublishingSolution.Properties.Resources.epub_logo_color;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(9, 9);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(43, 48);
            this.pictureBox1.TabIndex = 32;
            this.pictureBox1.TabStop = false;
            // 
            // tabWeb
            // 
            this.tabWeb.Controls.Add(this.groupBox3);
            this.tabWeb.Controls.Add(this.groupBox2);
            this.tabWeb.Controls.Add(this.groupBox1);
            this.tabWeb.Location = new System.Drawing.Point(4, 25);
            this.tabWeb.Margin = new System.Windows.Forms.Padding(4);
            this.tabWeb.Name = "tabWeb";
            this.tabWeb.Size = new System.Drawing.Size(381, 596);
            this.tabWeb.TabIndex = 6;
            this.tabWeb.Text = "Properties";
            this.tabWeb.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.txtWebFtpFldrNme);
            this.groupBox3.Controls.Add(this.txtWebEmailID);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.txtWebAdminSiteNme);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.txtWebAdminPwd);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.txtWebAdminUsrNme);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.txtWebUrl);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Location = new System.Drawing.Point(13, 346);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(352, 230);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Website Details";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 201);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(113, 17);
            this.label19.TabIndex = 10;
            this.label19.Text = "Ftp Folder Name";
            // 
            // txtWebFtpFldrNme
            // 
            this.txtWebFtpFldrNme.Location = new System.Drawing.Point(135, 197);
            this.txtWebFtpFldrNme.Margin = new System.Windows.Forms.Padding(4);
            this.txtWebFtpFldrNme.MaxLength = 50;
            this.txtWebFtpFldrNme.Name = "txtWebFtpFldrNme";
            this.txtWebFtpFldrNme.Size = new System.Drawing.Size(203, 22);
            this.txtWebFtpFldrNme.TabIndex = 11;
            // 
            // txtWebEmailID
            // 
            this.txtWebEmailID.Location = new System.Drawing.Point(135, 164);
            this.txtWebEmailID.Margin = new System.Windows.Forms.Padding(4);
            this.txtWebEmailID.MaxLength = 50;
            this.txtWebEmailID.Name = "txtWebEmailID";
            this.txtWebEmailID.Size = new System.Drawing.Size(203, 22);
            this.txtWebEmailID.TabIndex = 9;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(9, 167);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(59, 17);
            this.label18.TabIndex = 8;
            this.label18.Text = "Email ID";
            // 
            // txtWebAdminSiteNme
            // 
            this.txtWebAdminSiteNme.Location = new System.Drawing.Point(135, 132);
            this.txtWebAdminSiteNme.Margin = new System.Windows.Forms.Padding(4);
            this.txtWebAdminSiteNme.MaxLength = 50;
            this.txtWebAdminSiteNme.Name = "txtWebAdminSiteNme";
            this.txtWebAdminSiteNme.Size = new System.Drawing.Size(203, 22);
            this.txtWebAdminSiteNme.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 135);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(63, 17);
            this.label14.TabIndex = 6;
            this.label14.Text = "Site Title";
            // 
            // txtWebAdminPwd
            // 
            this.txtWebAdminPwd.Location = new System.Drawing.Point(135, 100);
            this.txtWebAdminPwd.Margin = new System.Windows.Forms.Padding(4);
            this.txtWebAdminPwd.MaxLength = 25;
            this.txtWebAdminPwd.Name = "txtWebAdminPwd";
            this.txtWebAdminPwd.PasswordChar = '*';
            this.txtWebAdminPwd.Size = new System.Drawing.Size(203, 22);
            this.txtWebAdminPwd.TabIndex = 5;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(11, 103);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(112, 17);
            this.label15.TabIndex = 4;
            this.label15.Text = "Admin Password";
            // 
            // txtWebAdminUsrNme
            // 
            this.txtWebAdminUsrNme.Location = new System.Drawing.Point(135, 68);
            this.txtWebAdminUsrNme.Margin = new System.Windows.Forms.Padding(4);
            this.txtWebAdminUsrNme.MaxLength = 25;
            this.txtWebAdminUsrNme.Name = "txtWebAdminUsrNme";
            this.txtWebAdminUsrNme.Size = new System.Drawing.Size(203, 22);
            this.txtWebAdminUsrNme.TabIndex = 3;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(11, 70);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(116, 17);
            this.label16.TabIndex = 2;
            this.label16.Text = "Admin Username";
            // 
            // txtWebUrl
            // 
            this.txtWebUrl.Location = new System.Drawing.Point(16, 37);
            this.txtWebUrl.Margin = new System.Windows.Forms.Padding(4);
            this.txtWebUrl.MaxLength = 100;
            this.txtWebUrl.Name = "txtWebUrl";
            this.txtWebUrl.Size = new System.Drawing.Size(321, 22);
            this.txtWebUrl.TabIndex = 1;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 17);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(79, 17);
            this.label17.TabIndex = 0;
            this.label17.Text = "Website url";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtFtpPassword);
            this.groupBox2.Controls.Add(this.lblTargetFileLocation);
            this.groupBox2.Controls.Add(this.txtFtpFileLocation);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtFtpUsername);
            this.groupBox2.Location = new System.Drawing.Point(16, 5);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(352, 151);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "FTP Details";
            // 
            // txtFtpPassword
            // 
            this.txtFtpPassword.Location = new System.Drawing.Point(127, 107);
            this.txtFtpPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtFtpPassword.MaxLength = 50;
            this.txtFtpPassword.Name = "txtFtpPassword";
            this.txtFtpPassword.PasswordChar = '*';
            this.txtFtpPassword.Size = new System.Drawing.Size(208, 22);
            this.txtFtpPassword.TabIndex = 5;
            // 
            // lblTargetFileLocation
            // 
            this.lblTargetFileLocation.AutoSize = true;
            this.lblTargetFileLocation.Location = new System.Drawing.Point(12, 21);
            this.lblTargetFileLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTargetFileLocation.Name = "lblTargetFileLocation";
            this.lblTargetFileLocation.Size = new System.Drawing.Size(252, 17);
            this.lblTargetFileLocation.TabIndex = 0;
            this.lblTargetFileLocation.Text = "FTP Address (ex: ftp://ip address/path)";
            // 
            // txtFtpFileLocation
            // 
            this.txtFtpFileLocation.Location = new System.Drawing.Point(16, 42);
            this.txtFtpFileLocation.Margin = new System.Windows.Forms.Padding(4);
            this.txtFtpFileLocation.MaxLength = 500;
            this.txtFtpFileLocation.Name = "txtFtpFileLocation";
            this.txtFtpFileLocation.Size = new System.Drawing.Size(319, 22);
            this.txtFtpFileLocation.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 111);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "FTP Password";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 79);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "FTP Username";
            // 
            // txtFtpUsername
            // 
            this.txtFtpUsername.Location = new System.Drawing.Point(127, 75);
            this.txtFtpUsername.Margin = new System.Windows.Forms.Padding(4);
            this.txtFtpUsername.MaxLength = 1000;
            this.txtFtpUsername.Name = "txtFtpUsername";
            this.txtFtpUsername.Size = new System.Drawing.Size(208, 22);
            this.txtFtpUsername.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSqlPassword);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtSqlUsername);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtSqlDBName);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtSqlServerName);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Location = new System.Drawing.Point(16, 167);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(352, 170);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MySql Database Details";
            // 
            // txtSqlPassword
            // 
            this.txtSqlPassword.Location = new System.Drawing.Point(127, 135);
            this.txtSqlPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtSqlPassword.MaxLength = 50;
            this.txtSqlPassword.Name = "txtSqlPassword";
            this.txtSqlPassword.PasswordChar = '*';
            this.txtSqlPassword.Size = new System.Drawing.Size(208, 22);
            this.txtSqlPassword.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 139);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 17);
            this.label10.TabIndex = 6;
            this.label10.Text = "Password";
            // 
            // txtSqlUsername
            // 
            this.txtSqlUsername.Location = new System.Drawing.Point(127, 103);
            this.txtSqlUsername.Margin = new System.Windows.Forms.Padding(4);
            this.txtSqlUsername.MaxLength = 25;
            this.txtSqlUsername.Name = "txtSqlUsername";
            this.txtSqlUsername.Size = new System.Drawing.Size(208, 22);
            this.txtSqlUsername.TabIndex = 5;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 107);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 17);
            this.label11.TabIndex = 4;
            this.label11.Text = "User Name";
            // 
            // txtSqlDBName
            // 
            this.txtSqlDBName.Location = new System.Drawing.Point(127, 71);
            this.txtSqlDBName.Margin = new System.Windows.Forms.Padding(4);
            this.txtSqlDBName.MaxLength = 25;
            this.txtSqlDBName.Name = "txtSqlDBName";
            this.txtSqlDBName.Size = new System.Drawing.Size(208, 22);
            this.txtSqlDBName.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 74);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(110, 17);
            this.label12.TabIndex = 2;
            this.label12.Text = "Database Name";
            // 
            // txtSqlServerName
            // 
            this.txtSqlServerName.Location = new System.Drawing.Point(16, 41);
            this.txtSqlServerName.Margin = new System.Windows.Forms.Padding(4);
            this.txtSqlServerName.MaxLength = 100;
            this.txtSqlServerName.Name = "txtSqlServerName";
            this.txtSqlServerName.Size = new System.Drawing.Size(319, 22);
            this.txtSqlServerName.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 20);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(211, 17);
            this.label13.TabIndex = 0;
            this.label13.Text = "MySql Server name / IP Address";
            // 
            // tabPreview
            // 
            this.tabPreview.AutoScroll = true;
            this.tabPreview.Controls.Add(this.btnPrevious);
            this.tabPreview.Controls.Add(this.btnNext);
            this.tabPreview.Controls.Add(this.picPreview);
            this.tabPreview.Location = new System.Drawing.Point(4, 25);
            this.tabPreview.Margin = new System.Windows.Forms.Padding(4);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.Size = new System.Drawing.Size(381, 596);
            this.tabPreview.TabIndex = 4;
            this.tabPreview.Text = "Preview";
            this.tabPreview.UseVisualStyleBackColor = true;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Enabled = false;
            this.btnPrevious.Location = new System.Drawing.Point(281, 0);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(33, 28);
            this.btnPrevious.TabIndex = 12;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(317, 0);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(33, 28);
            this.btnNext.TabIndex = 11;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // picPreview
            // 
            this.picPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.picPreview.Location = new System.Drawing.Point(4, 30);
            this.picPreview.Margin = new System.Windows.Forms.Padding(4);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(347, 433);
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
            this.tabPicture.Location = new System.Drawing.Point(4, 25);
            this.tabPicture.Margin = new System.Windows.Forms.Padding(4);
            this.tabPicture.Name = "tabPicture";
            this.tabPicture.Size = new System.Drawing.Size(381, 596);
            this.tabPicture.TabIndex = 5;
            this.tabPicture.Text = "Pictures";
            this.tabPicture.UseVisualStyleBackColor = true;
            // 
            // ChkDontPicture
            // 
            this.ChkDontPicture.AutoSize = true;
            this.ChkDontPicture.Location = new System.Drawing.Point(21, 16);
            this.ChkDontPicture.Margin = new System.Windows.Forms.Padding(4);
            this.ChkDontPicture.Name = "ChkDontPicture";
            this.ChkDontPicture.Size = new System.Drawing.Size(175, 21);
            this.ChkDontPicture.TabIndex = 5;
            this.ChkDontPicture.Text = "Do not include pictures";
            this.ChkDontPicture.UseVisualStyleBackColor = true;
            // 
            // LblPicPosition
            // 
            this.LblPicPosition.AutoSize = true;
            this.LblPicPosition.Location = new System.Drawing.Point(31, 249);
            this.LblPicPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblPicPosition.Name = "LblPicPosition";
            this.LblPicPosition.Size = new System.Drawing.Size(106, 17);
            this.LblPicPosition.TabIndex = 4;
            this.LblPicPosition.Text = "Picture Position";
            // 
            // DdlPicPosition
            // 
            this.DdlPicPosition.FormattingEnabled = true;
            this.DdlPicPosition.Location = new System.Drawing.Point(145, 245);
            this.DdlPicPosition.Margin = new System.Windows.Forms.Padding(4);
            this.DdlPicPosition.Name = "DdlPicPosition";
            this.DdlPicPosition.Size = new System.Drawing.Size(160, 24);
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
            this.GrpPicture.Location = new System.Drawing.Point(4, 43);
            this.GrpPicture.Margin = new System.Windows.Forms.Padding(4);
            this.GrpPicture.Name = "GrpPicture";
            this.GrpPicture.Padding = new System.Windows.Forms.Padding(4);
            this.GrpPicture.Size = new System.Drawing.Size(345, 190);
            this.GrpPicture.TabIndex = 0;
            this.GrpPicture.TabStop = false;
            // 
            // Lblcm
            // 
            this.Lblcm.AutoSize = true;
            this.Lblcm.Location = new System.Drawing.Point(243, 36);
            this.Lblcm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Lblcm.Name = "Lblcm";
            this.Lblcm.Size = new System.Drawing.Size(26, 17);
            this.Lblcm.TabIndex = 6;
            this.Lblcm.Text = "cm";
            // 
            // RadSingleColumn
            // 
            this.RadSingleColumn.AutoSize = true;
            this.RadSingleColumn.Location = new System.Drawing.Point(69, 121);
            this.RadSingleColumn.Margin = new System.Windows.Forms.Padding(4);
            this.RadSingleColumn.Name = "RadSingleColumn";
            this.RadSingleColumn.Size = new System.Drawing.Size(119, 21);
            this.RadSingleColumn.TabIndex = 5;
            this.RadSingleColumn.TabStop = true;
            this.RadSingleColumn.Text = "Single Column";
            this.RadSingleColumn.UseVisualStyleBackColor = true;
            // 
            // RadEntirePage
            // 
            this.RadEntirePage.AutoSize = true;
            this.RadEntirePage.Location = new System.Drawing.Point(69, 145);
            this.RadEntirePage.Margin = new System.Windows.Forms.Padding(4);
            this.RadEntirePage.Name = "RadEntirePage";
            this.RadEntirePage.Size = new System.Drawing.Size(103, 21);
            this.RadEntirePage.TabIndex = 4;
            this.RadEntirePage.TabStop = true;
            this.RadEntirePage.Text = "Entire Page";
            this.RadEntirePage.UseVisualStyleBackColor = true;
            // 
            // RadWidthIf
            // 
            this.RadWidthIf.AutoSize = true;
            this.RadWidthIf.Checked = true;
            this.RadWidthIf.Location = new System.Drawing.Point(69, 65);
            this.RadWidthIf.Margin = new System.Windows.Forms.Padding(4);
            this.RadWidthIf.Name = "RadWidthIf";
            this.RadWidthIf.Size = new System.Drawing.Size(259, 21);
            this.RadWidthIf.TabIndex = 3;
            this.RadWidthIf.TabStop = true;
            this.RadWidthIf.Text = "Use this width if a width not specified";
            this.RadWidthIf.UseVisualStyleBackColor = true;
            // 
            // RadWidthAll
            // 
            this.RadWidthAll.AutoSize = true;
            this.RadWidthAll.Location = new System.Drawing.Point(69, 94);
            this.RadWidthAll.Margin = new System.Windows.Forms.Padding(4);
            this.RadWidthAll.Name = "RadWidthAll";
            this.RadWidthAll.Size = new System.Drawing.Size(209, 21);
            this.RadWidthAll.TabIndex = 2;
            this.RadWidthAll.TabStop = true;
            this.RadWidthAll.Text = "Use this width for all pictures";
            this.RadWidthAll.UseVisualStyleBackColor = true;
            // 
            // LblPictureWidth
            // 
            this.LblPictureWidth.AutoSize = true;
            this.LblPictureWidth.Location = new System.Drawing.Point(8, 36);
            this.LblPictureWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblPictureWidth.Name = "LblPictureWidth";
            this.LblPictureWidth.Size = new System.Drawing.Size(92, 17);
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
            this.SpinPicWidth.Location = new System.Drawing.Point(105, 33);
            this.SpinPicWidth.Margin = new System.Windows.Forms.Padding(4);
            this.SpinPicWidth.Name = "SpinPicWidth";
            this.SpinPicWidth.Size = new System.Drawing.Size(135, 22);
            this.SpinPicWidth.TabIndex = 0;
            this.SpinPicWidth.Value = new decimal(new int[] {
            400,
            0,
            0,
            131072});
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.RoyalBlue;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(125, 1);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(732, 28);
            this.label2.TabIndex = 1;
            this.label2.Text = "Stylesheets";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInfoCaption
            // 
            this.lblInfoCaption.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblInfoCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoCaption.ForeColor = System.Drawing.Color.White;
            this.lblInfoCaption.Location = new System.Drawing.Point(858, 1);
            this.lblInfoCaption.Margin = new System.Windows.Forms.Padding(0);
            this.lblInfoCaption.Name = "lblInfoCaption";
            this.lblInfoCaption.Size = new System.Drawing.Size(387, 28);
            this.lblInfoCaption.TabIndex = 2;
            this.lblInfoCaption.Text = "Css";
            this.lblInfoCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCss
            // 
            this.txtCss.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCss.Location = new System.Drawing.Point(4, 4);
            this.txtCss.Margin = new System.Windows.Forms.Padding(4);
            this.txtCss.Multiline = true;
            this.txtCss.Name = "txtCss";
            this.txtCss.ReadOnly = true;
            this.txtCss.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCss.Size = new System.Drawing.Size(369, 42);
            this.txtCss.TabIndex = 0;
            // 
            // TLPanelOuter
            // 
            this.TLPanelOuter.AutoScroll = true;
            this.TLPanelOuter.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.TLPanelOuter.ColumnCount = 3;
            this.TLPanelOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.TLPanelOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLPanelOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 413F));
            this.TLPanelOuter.Controls.Add(this.label2, 1, 0);
            this.TLPanelOuter.Controls.Add(this.TLPanel1, 0, 1);
            this.TLPanelOuter.Controls.Add(this.lblInfoCaption, 2, 0);
            this.TLPanelOuter.Controls.Add(this.TLPanel2, 1, 1);
            this.TLPanelOuter.Controls.Add(this.TLPanel3, 2, 1);
            this.TLPanelOuter.Controls.Add(this.lblType, 0, 0);
            this.TLPanelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLPanelOuter.Location = new System.Drawing.Point(0, 56);
            this.TLPanelOuter.Margin = new System.Windows.Forms.Padding(1);
            this.TLPanelOuter.Name = "TLPanelOuter";
            this.TLPanelOuter.RowCount = 2;
            this.TLPanelOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.TLPanelOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLPanelOuter.Size = new System.Drawing.Size(1272, 724);
            this.TLPanelOuter.TabIndex = 19;
            // 
            // TLPanel1
            // 
            this.TLPanel1.ColumnCount = 1;
            this.TLPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLPanel1.Controls.Add(this.panel1, 0, 1);
            this.TLPanel1.Controls.Add(this.panel2, 0, 0);
            this.TLPanel1.Location = new System.Drawing.Point(1, 30);
            this.TLPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.TLPanel1.Name = "TLPanel1";
            this.TLPanel1.RowCount = 2;
            this.TLPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.TLPanel1.Size = new System.Drawing.Size(121, 423);
            this.TLPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnScripture);
            this.panel1.Controls.Add(this.btnDictionary);
            this.panel1.Location = new System.Drawing.Point(4, 358);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(113, 59);
            this.panel1.TabIndex = 0;
            // 
            // btnScripture
            // 
            this.btnScripture.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnScripture.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnScripture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScripture.Location = new System.Drawing.Point(0, 28);
            this.btnScripture.Margin = new System.Windows.Forms.Padding(4);
            this.btnScripture.Name = "btnScripture";
            this.btnScripture.Size = new System.Drawing.Size(113, 28);
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
            this.btnDictionary.Location = new System.Drawing.Point(0, 0);
            this.btnDictionary.Margin = new System.Windows.Forms.Padding(4);
            this.btnDictionary.Name = "btnDictionary";
            this.btnDictionary.Size = new System.Drawing.Size(113, 28);
            this.btnDictionary.TabIndex = 0;
            this.btnDictionary.Text = "&Dictionary";
            this.btnDictionary.UseVisualStyleBackColor = true;
            this.btnDictionary.Click += new System.EventHandler(this.btnDictionary_Click);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.btnOthers);
            this.panel2.Controls.Add(this.btnWeb);
            this.panel2.Controls.Add(this.btnMobile);
            this.panel2.Controls.Add(this.btnPaper);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(4, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(113, 346);
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
            this.btnOthers.Location = new System.Drawing.Point(0, 258);
            this.btnOthers.Margin = new System.Windows.Forms.Padding(4);
            this.btnOthers.Name = "btnOthers";
            this.btnOthers.Size = new System.Drawing.Size(113, 86);
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
            this.btnWeb.Location = new System.Drawing.Point(0, 172);
            this.btnWeb.Margin = new System.Windows.Forms.Padding(4);
            this.btnWeb.Name = "btnWeb";
            this.btnWeb.Size = new System.Drawing.Size(113, 86);
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
            this.btnMobile.Location = new System.Drawing.Point(0, 86);
            this.btnMobile.Margin = new System.Windows.Forms.Padding(4);
            this.btnMobile.Name = "btnMobile";
            this.btnMobile.Size = new System.Drawing.Size(113, 86);
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
            this.btnPaper.Location = new System.Drawing.Point(0, 0);
            this.btnPaper.Margin = new System.Windows.Forms.Padding(4);
            this.btnPaper.Name = "btnPaper";
            this.btnPaper.Size = new System.Drawing.Size(113, 86);
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
            this.TLPanel2.Location = new System.Drawing.Point(129, 34);
            this.TLPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.TLPanel2.Name = "TLPanel2";
            this.TLPanel2.RowCount = 1;
            this.TLPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 613F));
            this.TLPanel2.Size = new System.Drawing.Size(724, 685);
            this.TLPanel2.TabIndex = 4;
            // 
            // TLPanel3
            // 
            this.TLPanel3.ColumnCount = 1;
            this.TLPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLPanel3.Controls.Add(this.panel3, 0, 1);
            this.TLPanel3.Controls.Add(this.txtCss, 0, 0);
            this.TLPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLPanel3.Location = new System.Drawing.Point(862, 34);
            this.TLPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.TLPanel3.Name = "TLPanel3";
            this.TLPanel3.RowCount = 2;
            this.TLPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TLPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 215F));
            this.TLPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.TLPanel3.Size = new System.Drawing.Size(405, 685);
            this.TLPanel3.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.tabControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(4, 54);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(397, 627);
            this.panel3.TabIndex = 20;
            // 
            // lblType
            // 
            this.lblType.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.ForeColor = System.Drawing.Color.White;
            this.lblType.Location = new System.Drawing.Point(1, 1);
            this.lblType.Margin = new System.Windows.Forms.Padding(0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(121, 28);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Dictionary";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConfigurationTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 780);
            this.Controls.Add(this.TLPanelOuter);
            this.Controls.Add(this.toolStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ConfigurationTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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
            this.tabDisplay.ResumeLayout(false);
            this.tabDisplay.PerformLayout();
            this.tabMobile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mobileIcon)).EndInit();
            this.tabOthers.ResumeLayout(false);
            this.tabOthers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFonts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabWeb.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.ComboBox ddlSense;
        private System.Windows.Forms.Label lblSenseLayout;
        private System.Windows.Forms.ComboBox ddlPicture;
        private System.Windows.Forms.Label lblLineSpace;
        private System.Windows.Forms.Label lblRunningHeader;
        private System.Windows.Forms.ComboBox ddlLeading;
        private System.Windows.Forms.ComboBox ddlRunningHead;
        private System.Windows.Forms.Label lblRules;
        private System.Windows.Forms.ComboBox ddlRules;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.ComboBox ddlFontSize;
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
        private System.Windows.Forms.ComboBox ddlFileProduceDict;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage tabOthers;
        private System.Windows.Forms.TabPage tabPreview;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label LblPageNumber;
        private System.Windows.Forms.ComboBox ddlPageNumber;
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
        private System.Windows.Forms.Label lblPct;
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
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
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
    }
}