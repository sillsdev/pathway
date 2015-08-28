using System.Drawing;

namespace SIL.PublishingSolution
{
    partial class ExportThroughPathway
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportThroughPathway));
			this.l10NSharpExtender1 = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.lnkIP = new System.Windows.Forms.LinkLabel();
			this.chkIP = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.txtRights = new System.Windows.Forms.TextBox();
			this.lblRights = new System.Windows.Forms.Label();
			this.txtBookTitle = new System.Windows.Forms.TextBox();
			this.txtCreator = new System.Windows.Forms.TextBox();
			this.lblDescription = new System.Windows.Forms.Label();
			this.lblCreator = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.lblPublisher = new System.Windows.Forms.Label();
			this.lblBookTitle = new System.Windows.Forms.Label();
			this.txtPublisher = new System.Windows.Forms.TextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.chkTOC = new System.Windows.Forms.CheckBox();
			this.lnkChooseCopyright = new System.Windows.Forms.LinkLabel();
			this.ddlCopyrightStatement = new System.Windows.Forms.ComboBox();
			this.rdoCustomCopyright = new System.Windows.Forms.RadioButton();
			this.rdoStandardCopyright = new System.Windows.Forms.RadioButton();
			this.chkCoverImageTitle = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.chkTitlePage = new System.Windows.Forms.CheckBox();
			this.chkCoverImage = new System.Windows.Forms.CheckBox();
			this.btnBrowseColophon = new System.Windows.Forms.Button();
			this.txtColophonFile = new System.Windows.Forms.TextBox();
			this.chkColophon = new System.Windows.Forms.CheckBox();
			this.btnCoverImage = new System.Windows.Forms.Button();
			this.imgCoverImage = new System.Windows.Forms.PictureBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkLbPreprocess = new System.Windows.Forms.CheckedListBox();
			this.grpInclude = new System.Windows.Forms.GroupBox();
			this.chkConfiguredDictionary = new System.Windows.Forms.CheckBox();
			this.chkReversalIndexes = new System.Windows.Forms.CheckBox();
			this.chkGrammarSketch = new System.Windows.Forms.CheckBox();
			this.chkOOReduceStyleNames = new System.Windows.Forms.CheckBox();
			this.btnBrowseSaveInFolder = new System.Windows.Forms.Button();
			this.txtSaveInFolder = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.tpHyphenation = new System.Windows.Forms.TabPage();
			this.chkHyphen = new System.Windows.Forms.CheckBox();
			this.gbHyphnSettings = new System.Windows.Forms.GroupBox();
			this.clbHyphenlang = new System.Windows.Forms.CheckedListBox();
			this.lblHyphenFonts = new System.Windows.Forms.Label();
			this.btnHelpShow = new System.Windows.Forms.Button();
			this.BtnBrwsLayout = new System.Windows.Forms.Button();
			this.btnMoreLessOptions = new System.Windows.Forms.Button();
			this.ddlStyle = new System.Windows.Forms.ComboBox();
			this.ddlLayout = new System.Windows.Forms.ComboBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).BeginInit();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgCoverImage)).BeginInit();
			this.tabPage3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.grpInclude.SuspendLayout();
			this.tpHyphenation.SuspendLayout();
			this.gbHyphnSettings.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// l10NSharpExtender1
			// 
			this.l10NSharpExtender1.LocalizationManagerId = "Pathway";
			this.l10NSharpExtender1.PrefixForNewItems = "ExportThroughPathway";
			// 
			// lnkIP
			// 
			this.lnkIP.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lnkIP, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lnkIP, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.lnkIP, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.lnkIP, "ExportThroughPathway.lnkIP");
			this.lnkIP.Location = new System.Drawing.Point(115, 398);
			this.lnkIP.Name = "lnkIP";
			this.lnkIP.Size = new System.Drawing.Size(121, 13);
			this.lnkIP.TabIndex = 8;
			this.lnkIP.TabStop = true;
			this.lnkIP.Text = "Intellectual Property Info";
			this.lnkIP.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkIP_LinkClicked);
			// 
			// chkIP
			// 
			this.chkIP.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkIP, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkIP, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.chkIP, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.chkIP, "ExportThroughPathway.chkIP");
			this.chkIP.Location = new System.Drawing.Point(99, 365);
			this.chkIP.Name = "chkIP";
			this.chkIP.Size = new System.Drawing.Size(286, 30);
			this.chkIP.TabIndex = 7;
			this.chkIP.Text = "I have complied with my organization\'s Intellectual Property (copyright) and Arch" +
    "iving policies.";
			this.chkIP.UseVisualStyleBackColor = true;
			this.chkIP.CheckedChanged += new System.EventHandler(this.chkIP_CheckedChanged);
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label2.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label2, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label2, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.label2, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.label2, "ExportThroughPathway.label2");
			this.label2.Location = new System.Drawing.Point(34, 38);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Stylesheet:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.label1.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label1, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.label1, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.label1, "ExportThroughPathway.label1");
			this.label1.Location = new System.Drawing.Point(30, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Destination:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// btnHelp
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnHelp, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnHelp, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnHelp, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnHelp, "ExportThroughPathway.btnHelp");
			this.btnHelp.Location = new System.Drawing.Point(310, 426);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(75, 23);
			this.btnHelp.TabIndex = 11;
			this.btnHelp.Text = "&Help";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnCancel, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnCancel, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnCancel, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnCancel, "ExportThroughPathway.btnCancel");
			this.btnCancel.Location = new System.Drawing.Point(229, 426);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnOK, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnOK, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnOK, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnOK, "ExportThroughPathway.btnOK");
			this.btnOK.Location = new System.Drawing.Point(148, 426);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 9;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// tabPage1
			// 
			this.tabPage1.AutoScroll = true;
			this.tabPage1.Controls.Add(this.label7);
			this.tabPage1.Controls.Add(this.txtRights);
			this.tabPage1.Controls.Add(this.lblRights);
			this.tabPage1.Controls.Add(this.txtBookTitle);
			this.tabPage1.Controls.Add(this.txtCreator);
			this.tabPage1.Controls.Add(this.lblDescription);
			this.tabPage1.Controls.Add(this.lblCreator);
			this.tabPage1.Controls.Add(this.txtDescription);
			this.tabPage1.Controls.Add(this.lblPublisher);
			this.tabPage1.Controls.Add(this.lblBookTitle);
			this.tabPage1.Controls.Add(this.txtPublisher);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tabPage1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tabPage1, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.tabPage1, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.tabPage1, "ExportThroughPathway.tabPage1");
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(365, 238);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Publication Info";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label7, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label7, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.label7, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.label7, "ExportThroughPathway.label7");
			this.label7.Location = new System.Drawing.Point(6, 10);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(334, 18);
			this.label7.TabIndex = 59;
			this.label7.Text = "Describe this publication:";
			// 
			// txtRights
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtRights, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtRights, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.txtRights, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.txtRights, "ExportThroughPathway.txtRights");
			this.txtRights.Location = new System.Drawing.Point(129, 159);
			this.txtRights.Multiline = true;
			this.txtRights.Name = "txtRights";
			this.txtRights.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtRights.Size = new System.Drawing.Size(230, 45);
			this.txtRights.TabIndex = 15;
			// 
			// lblRights
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblRights, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblRights, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.lblRights, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.lblRights, "ExportThroughPathway.lblRights");
			this.lblRights.Location = new System.Drawing.Point(0, 159);
			this.lblRights.Name = "lblRights";
			this.lblRights.Size = new System.Drawing.Size(129, 34);
			this.lblRights.TabIndex = 14;
			this.lblRights.Text = "Copyright Holder";
			this.lblRights.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtBookTitle
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtBookTitle, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtBookTitle, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtBookTitle, "ExportThroughPathway.ExportThroughPathway.txtBookTitle");
			this.txtBookTitle.Location = new System.Drawing.Point(129, 31);
			this.txtBookTitle.MaxLength = 32;
			this.txtBookTitle.Name = "txtBookTitle";
			this.txtBookTitle.Size = new System.Drawing.Size(230, 20);
			this.txtBookTitle.TabIndex = 7;
			// 
			// txtCreator
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtCreator, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtCreator, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtCreator, "ExportThroughPathway.ExportThroughPathway.txtCreator");
			this.txtCreator.Location = new System.Drawing.Point(129, 107);
			this.txtCreator.Name = "txtCreator";
			this.txtCreator.Size = new System.Drawing.Size(230, 20);
			this.txtCreator.TabIndex = 11;
			// 
			// lblDescription
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblDescription, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblDescription, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.lblDescription, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.lblDescription, "ExportThroughPathway.lblDescription");
			this.lblDescription.Location = new System.Drawing.Point(15, 59);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(114, 14);
			this.lblDescription.TabIndex = 8;
			this.lblDescription.Text = "Description";
			this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblCreator
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblCreator, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblCreator, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.lblCreator, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.lblCreator, "ExportThroughPathway.lblCreator");
			this.lblCreator.Location = new System.Drawing.Point(18, 110);
			this.lblCreator.Name = "lblCreator";
			this.lblCreator.Size = new System.Drawing.Size(111, 12);
			this.lblCreator.TabIndex = 10;
			this.lblCreator.Text = "Creator";
			this.lblCreator.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtDescription
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtDescription, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtDescription, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.txtDescription, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.txtDescription, "ExportThroughPathway.txtDescription");
			this.txtDescription.Location = new System.Drawing.Point(129, 57);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDescription.Size = new System.Drawing.Size(230, 44);
			this.txtDescription.TabIndex = 9;
			// 
			// lblPublisher
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblPublisher, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblPublisher, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.lblPublisher, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.lblPublisher, "ExportThroughPathway.lblPublisher");
			this.lblPublisher.Location = new System.Drawing.Point(18, 138);
			this.lblPublisher.Name = "lblPublisher";
			this.lblPublisher.Size = new System.Drawing.Size(111, 15);
			this.lblPublisher.TabIndex = 12;
			this.lblPublisher.Text = "Publisher";
			this.lblPublisher.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblBookTitle
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblBookTitle, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblBookTitle, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.lblBookTitle, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.lblBookTitle, "ExportThroughPathway.lblBookTitle");
			this.lblBookTitle.Location = new System.Drawing.Point(15, 31);
			this.lblBookTitle.Name = "lblBookTitle";
			this.lblBookTitle.Size = new System.Drawing.Size(114, 20);
			this.lblBookTitle.TabIndex = 6;
			this.lblBookTitle.Text = "Book Title";
			this.lblBookTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtPublisher
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtPublisher, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtPublisher, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtPublisher, "ExportThroughPathway.ExportThroughPathway.txtPublisher");
			this.txtPublisher.Location = new System.Drawing.Point(129, 133);
			this.txtPublisher.Name = "txtPublisher";
			this.txtPublisher.Size = new System.Drawing.Size(230, 20);
			this.txtPublisher.TabIndex = 13;
			// 
			// tabPage2
			// 
			this.tabPage2.AutoScroll = true;
			this.tabPage2.Controls.Add(this.chkTOC);
			this.tabPage2.Controls.Add(this.lnkChooseCopyright);
			this.tabPage2.Controls.Add(this.ddlCopyrightStatement);
			this.tabPage2.Controls.Add(this.rdoCustomCopyright);
			this.tabPage2.Controls.Add(this.rdoStandardCopyright);
			this.tabPage2.Controls.Add(this.chkCoverImageTitle);
			this.tabPage2.Controls.Add(this.label5);
			this.tabPage2.Controls.Add(this.chkTitlePage);
			this.tabPage2.Controls.Add(this.chkCoverImage);
			this.tabPage2.Controls.Add(this.btnBrowseColophon);
			this.tabPage2.Controls.Add(this.txtColophonFile);
			this.tabPage2.Controls.Add(this.chkColophon);
			this.tabPage2.Controls.Add(this.btnCoverImage);
			this.tabPage2.Controls.Add(this.imgCoverImage);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tabPage2, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tabPage2, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.tabPage2, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.tabPage2, "ExportThroughPathway.tabPage2");
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(365, 238);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Front Matter";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// chkTOC
			// 
			this.chkTOC.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkTOC, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkTOC, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.chkTOC, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.chkTOC, "ExportThroughPathway.chkTOC");
			this.chkTOC.Location = new System.Drawing.Point(1, 210);
			this.chkTOC.Name = "chkTOC";
			this.chkTOC.Size = new System.Drawing.Size(110, 17);
			this.chkTOC.TabIndex = 66;
			this.chkTOC.Text = "Table of Contents";
			this.chkTOC.UseVisualStyleBackColor = true;
			// 
			// lnkChooseCopyright
			// 
			this.lnkChooseCopyright.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lnkChooseCopyright, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lnkChooseCopyright, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.lnkChooseCopyright, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.lnkChooseCopyright, "ExportThroughPathway.lnkChooseCopyright");
			this.lnkChooseCopyright.Location = new System.Drawing.Point(44, 189);
			this.lnkChooseCopyright.Name = "lnkChooseCopyright";
			this.lnkChooseCopyright.Size = new System.Drawing.Size(179, 13);
			this.lnkChooseCopyright.TabIndex = 65;
			this.lnkChooseCopyright.TabStop = true;
			this.lnkChooseCopyright.Text = "Help me choose a rights statement...";
			this.lnkChooseCopyright.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkChooseCopyright_LinkClicked);
			// 
			// ddlCopyrightStatement
			// 
			this.ddlCopyrightStatement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlCopyrightStatement.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlCopyrightStatement, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlCopyrightStatement, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlCopyrightStatement, "ExportThroughPathway.ExportThroughPathway.ddlCopyrightStatement");
			this.ddlCopyrightStatement.Location = new System.Drawing.Point(190, 133);
			this.ddlCopyrightStatement.Name = "ddlCopyrightStatement";
			this.ddlCopyrightStatement.Size = new System.Drawing.Size(154, 21);
			this.ddlCopyrightStatement.TabIndex = 64;
			this.ddlCopyrightStatement.SelectedIndexChanged += new System.EventHandler(this.ddlCopyrightStatement_SelectedIndexChanged);
			// 
			// rdoCustomCopyright
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.rdoCustomCopyright, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.rdoCustomCopyright, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.rdoCustomCopyright, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.rdoCustomCopyright, "ExportThroughPathway.rdoCustomCopyright");
			this.rdoCustomCopyright.Location = new System.Drawing.Point(10, 160);
			this.rdoCustomCopyright.Name = "rdoCustomCopyright";
			this.rdoCustomCopyright.Size = new System.Drawing.Size(82, 22);
			this.rdoCustomCopyright.TabIndex = 63;
			this.rdoCustomCopyright.TabStop = true;
			this.rdoCustomCopyright.Text = "Custom:";
			this.rdoCustomCopyright.UseVisualStyleBackColor = true;
			this.rdoCustomCopyright.CheckedChanged += new System.EventHandler(this.rdoCustomCopyright_CheckedChanged);
			// 
			// rdoStandardCopyright
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.rdoStandardCopyright, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.rdoStandardCopyright, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.rdoStandardCopyright, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.rdoStandardCopyright, "ExportThroughPathway.rdoStandardCopyright");
			this.rdoStandardCopyright.Location = new System.Drawing.Point(10, 125);
			this.rdoStandardCopyright.Name = "rdoStandardCopyright";
			this.rdoStandardCopyright.Size = new System.Drawing.Size(174, 34);
			this.rdoStandardCopyright.TabIndex = 62;
			this.rdoStandardCopyright.TabStop = true;
			this.rdoStandardCopyright.Text = "Standard Rights Statement:";
			this.rdoStandardCopyright.UseVisualStyleBackColor = true;
			this.rdoStandardCopyright.CheckedChanged += new System.EventHandler(this.rdoStandardCopyright_CheckedChanged);
			// 
			// chkCoverImageTitle
			// 
			this.chkCoverImageTitle.AutoSize = true;
			this.chkCoverImageTitle.Enabled = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkCoverImageTitle, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkCoverImageTitle, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.chkCoverImageTitle, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.chkCoverImageTitle, "ExportThroughPathway.chkCoverImageTitle");
			this.chkCoverImageTitle.Location = new System.Drawing.Point(10, 64);
			this.chkCoverImageTitle.Name = "chkCoverImageTitle";
			this.chkCoverImageTitle.Size = new System.Drawing.Size(192, 17);
			this.chkCoverImageTitle.TabIndex = 61;
			this.chkCoverImageTitle.Text = "Include the book\'s title in the image";
			this.chkCoverImageTitle.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label5, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label5, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.label5, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.label5, "ExportThroughPathway.label5");
			this.label5.Location = new System.Drawing.Point(-3, 10);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(203, 13);
			this.label5.TabIndex = 60;
			this.label5.Text = "Add the following pages to the document:";
			// 
			// chkTitlePage
			// 
			this.chkTitlePage.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkTitlePage, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkTitlePage, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.chkTitlePage, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.chkTitlePage, "ExportThroughPathway.chkTitlePage");
			this.chkTitlePage.Location = new System.Drawing.Point(1, 87);
			this.chkTitlePage.Name = "chkTitlePage";
			this.chkTitlePage.Size = new System.Drawing.Size(182, 17);
			this.chkTitlePage.TabIndex = 18;
			this.chkTitlePage.Text = "Title Page (page with book\'s title)";
			this.chkTitlePage.UseVisualStyleBackColor = true;
			// 
			// chkCoverImage
			// 
			this.chkCoverImage.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkCoverImage, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkCoverImage, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.chkCoverImage, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.chkCoverImage, "ExportThroughPathway.chkCoverImage");
			this.chkCoverImage.Location = new System.Drawing.Point(1, 34);
			this.chkCoverImage.Name = "chkCoverImage";
			this.chkCoverImage.Size = new System.Drawing.Size(117, 17);
			this.chkCoverImage.TabIndex = 16;
			this.chkCoverImage.Text = "Cover Image Page:";
			this.chkCoverImage.UseVisualStyleBackColor = true;
			this.chkCoverImage.CheckedChanged += new System.EventHandler(this.chkCoverImage_CheckedChanged);
			// 
			// btnBrowseColophon
			// 
			this.btnBrowseColophon.Enabled = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnBrowseColophon, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnBrowseColophon, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnBrowseColophon, "ExportThroughPathway.ExportThroughPathway.btnBrowseColophon");
			this.btnBrowseColophon.Location = new System.Drawing.Point(317, 160);
			this.btnBrowseColophon.Name = "btnBrowseColophon";
			this.btnBrowseColophon.Size = new System.Drawing.Size(27, 23);
			this.btnBrowseColophon.TabIndex = 21;
			this.btnBrowseColophon.Text = "...";
			this.btnBrowseColophon.UseVisualStyleBackColor = true;
			this.btnBrowseColophon.Click += new System.EventHandler(this.btnBrowseColophon_Click);
			// 
			// txtColophonFile
			// 
			this.txtColophonFile.Enabled = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtColophonFile, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtColophonFile, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtColophonFile, "ExportThroughPathway.ExportThroughPathway.txtColophonFile");
			this.txtColophonFile.Location = new System.Drawing.Point(98, 162);
			this.txtColophonFile.Name = "txtColophonFile";
			this.txtColophonFile.Size = new System.Drawing.Size(212, 20);
			this.txtColophonFile.TabIndex = 20;
			// 
			// chkColophon
			// 
			this.chkColophon.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkColophon, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkColophon, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.chkColophon, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.chkColophon, "ExportThroughPathway.chkColophon");
			this.chkColophon.Location = new System.Drawing.Point(1, 110);
			this.chkColophon.Name = "chkColophon";
			this.chkColophon.Size = new System.Drawing.Size(150, 17);
			this.chkColophon.TabIndex = 19;
			this.chkColophon.Text = "Copyright Infomation Page";
			this.chkColophon.UseVisualStyleBackColor = true;
			this.chkColophon.CheckedChanged += new System.EventHandler(this.chkColophon_CheckedChanged);
			// 
			// btnCoverImage
			// 
			this.btnCoverImage.Enabled = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnCoverImage, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnCoverImage, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnCoverImage, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnCoverImage, "ExportThroughPathway.btnCoverImage");
			this.btnCoverImage.Location = new System.Drawing.Point(206, 30);
			this.btnCoverImage.Name = "btnCoverImage";
			this.btnCoverImage.Size = new System.Drawing.Size(92, 23);
			this.btnCoverImage.TabIndex = 17;
			this.btnCoverImage.Text = "&Select...";
			this.btnCoverImage.UseVisualStyleBackColor = true;
			this.btnCoverImage.Click += new System.EventHandler(this.btnCoverImage_Click);
			// 
			// imgCoverImage
			// 
			this.imgCoverImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.imgCoverImage.Enabled = false;
			this.imgCoverImage.Image = ((System.Drawing.Image)(resources.GetObject("imgCoverImage.Image")));
			this.imgCoverImage.InitialImage = ((System.Drawing.Image)(resources.GetObject("imgCoverImage.InitialImage")));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.imgCoverImage, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.imgCoverImage, null);
			this.l10NSharpExtender1.SetLocalizingId(this.imgCoverImage, "ExportThroughPathway.ExportThroughPathway.imgCoverImage");
			this.imgCoverImage.Location = new System.Drawing.Point(168, 26);
			this.imgCoverImage.Name = "imgCoverImage";
			this.imgCoverImage.Size = new System.Drawing.Size(32, 32);
			this.imgCoverImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.imgCoverImage.TabIndex = 54;
			this.imgCoverImage.TabStop = false;
			// 
			// tabPage3
			// 
			this.tabPage3.AutoScroll = true;
			this.tabPage3.Controls.Add(this.groupBox1);
			this.tabPage3.Controls.Add(this.grpInclude);
			this.tabPage3.Controls.Add(this.chkOOReduceStyleNames);
			this.tabPage3.Controls.Add(this.btnBrowseSaveInFolder);
			this.tabPage3.Controls.Add(this.txtSaveInFolder);
			this.tabPage3.Controls.Add(this.label8);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tabPage3, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tabPage3, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.tabPage3, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.tabPage3, "ExportThroughPathway.tabPage3");
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(365, 238);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Processing Options";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkLbPreprocess);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.groupBox1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.groupBox1, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.groupBox1, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.groupBox1, "ExportThroughPathway.groupBox1");
			this.groupBox1.Location = new System.Drawing.Point(8, 154);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(347, 81);
			this.groupBox1.TabIndex = 31;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Preprocessing Transformation:";
			// 
			// chkLbPreprocess
			// 
			this.chkLbPreprocess.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkLbPreprocess, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkLbPreprocess, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.chkLbPreprocess, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.chkLbPreprocess, "ExportThroughPathway.chkLbPreprocess");
			this.chkLbPreprocess.Location = new System.Drawing.Point(10, 19);
			this.chkLbPreprocess.Name = "chkLbPreprocess";
			this.chkLbPreprocess.Size = new System.Drawing.Size(332, 49);
			this.chkLbPreprocess.TabIndex = 0;
			// 
			// grpInclude
			// 
			this.grpInclude.Controls.Add(this.chkConfiguredDictionary);
			this.grpInclude.Controls.Add(this.chkReversalIndexes);
			this.grpInclude.Controls.Add(this.chkGrammarSketch);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.grpInclude, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.grpInclude, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.grpInclude, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.grpInclude, "ExportThroughPathway.grpInclude");
			this.grpInclude.Location = new System.Drawing.Point(8, 80);
			this.grpInclude.Name = "grpInclude";
			this.grpInclude.Size = new System.Drawing.Size(347, 68);
			this.grpInclude.TabIndex = 30;
			this.grpInclude.TabStop = false;
			this.grpInclude.Text = "Select data to include:";
			// 
			// chkConfiguredDictionary
			// 
			this.chkConfiguredDictionary.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkConfiguredDictionary, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkConfiguredDictionary, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.chkConfiguredDictionary, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.chkConfiguredDictionary, "ExportThroughPathway.chkConfiguredDictionary");
			this.chkConfiguredDictionary.Location = new System.Drawing.Point(6, 19);
			this.chkConfiguredDictionary.Name = "chkConfiguredDictionary";
			this.chkConfiguredDictionary.Size = new System.Drawing.Size(127, 17);
			this.chkConfiguredDictionary.TabIndex = 27;
			this.chkConfiguredDictionary.Text = "Configured Dictionary";
			this.chkConfiguredDictionary.UseVisualStyleBackColor = true;
			this.chkConfiguredDictionary.CheckedChanged += new System.EventHandler(this.chkConfiguredDictionary_CheckedChanged);
			// 
			// chkReversalIndexes
			// 
			this.chkReversalIndexes.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkReversalIndexes, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkReversalIndexes, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.chkReversalIndexes, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.chkReversalIndexes, "ExportThroughPathway.chkReversalIndexes");
			this.chkReversalIndexes.Location = new System.Drawing.Point(6, 45);
			this.chkReversalIndexes.Name = "chkReversalIndexes";
			this.chkReversalIndexes.Size = new System.Drawing.Size(108, 17);
			this.chkReversalIndexes.TabIndex = 28;
			this.chkReversalIndexes.Text = "Reversal Indexes";
			this.chkReversalIndexes.UseVisualStyleBackColor = true;
			this.chkReversalIndexes.CheckedChanged += new System.EventHandler(this.chkReversalIndexes_CheckedChanged);
			// 
			// chkGrammarSketch
			// 
			this.chkGrammarSketch.AutoSize = true;
			this.chkGrammarSketch.Enabled = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkGrammarSketch, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkGrammarSketch, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.chkGrammarSketch, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.chkGrammarSketch, "ExportThroughPathway.chkGrammarSketch");
			this.chkGrammarSketch.Location = new System.Drawing.Point(167, 45);
			this.chkGrammarSketch.Name = "chkGrammarSketch";
			this.chkGrammarSketch.Size = new System.Drawing.Size(105, 17);
			this.chkGrammarSketch.TabIndex = 29;
			this.chkGrammarSketch.Text = "Grammar Sketch";
			this.chkGrammarSketch.UseVisualStyleBackColor = true;
			this.chkGrammarSketch.Visible = false;
			this.chkGrammarSketch.CheckedChanged += new System.EventHandler(this.chkGrammarSketch_CheckedChanged);
			// 
			// chkOOReduceStyleNames
			// 
			this.chkOOReduceStyleNames.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkOOReduceStyleNames, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkOOReduceStyleNames, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.chkOOReduceStyleNames, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.chkOOReduceStyleNames, "ExportThroughPathway.chkOOReduceStyleNames");
			this.chkOOReduceStyleNames.Location = new System.Drawing.Point(14, 52);
			this.chkOOReduceStyleNames.Name = "chkOOReduceStyleNames";
			this.chkOOReduceStyleNames.Size = new System.Drawing.Size(319, 17);
			this.chkOOReduceStyleNames.TabIndex = 26;
			this.chkOOReduceStyleNames.Text = "Replace styles with direct formatting in OpenOffice/LibreOffice";
			this.chkOOReduceStyleNames.UseVisualStyleBackColor = true;
			// 
			// btnBrowseSaveInFolder
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnBrowseSaveInFolder, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnBrowseSaveInFolder, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnBrowseSaveInFolder, "ExportThroughPathway.ExportThroughPathway.btnBrowseSaveInFolder");
			this.btnBrowseSaveInFolder.Location = new System.Drawing.Point(332, 17);
			this.btnBrowseSaveInFolder.Name = "btnBrowseSaveInFolder";
			this.btnBrowseSaveInFolder.Size = new System.Drawing.Size(24, 23);
			this.btnBrowseSaveInFolder.TabIndex = 24;
			this.btnBrowseSaveInFolder.Text = "...";
			this.btnBrowseSaveInFolder.UseVisualStyleBackColor = true;
			this.btnBrowseSaveInFolder.Click += new System.EventHandler(this.btnBrowseSaveInFolder_Click);
			// 
			// txtSaveInFolder
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.txtSaveInFolder, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.txtSaveInFolder, null);
			this.l10NSharpExtender1.SetLocalizingId(this.txtSaveInFolder, "ExportThroughPathway.ExportThroughPathway.txtSaveInFolder");
			this.txtSaveInFolder.Location = new System.Drawing.Point(92, 22);
			this.txtSaveInFolder.Name = "txtSaveInFolder";
			this.txtSaveInFolder.Size = new System.Drawing.Size(237, 20);
			this.txtSaveInFolder.TabIndex = 23;
			this.txtSaveInFolder.Text = "C:\\Users\\brommerse\\Documents\\Publications\\Nkonya\\Scripture";
			// 
			// label8
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.label8, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.label8, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.label8, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.label8, "ExportThroughPathway.label8");
			this.label8.Location = new System.Drawing.Point(5, 25);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(80, 14);
			this.label8.TabIndex = 22;
			this.label8.Text = "Save in Folder :";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tpHyphenation
			// 
			this.tpHyphenation.Controls.Add(this.chkHyphen);
			this.tpHyphenation.Controls.Add(this.gbHyphnSettings);
			this.tpHyphenation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.tpHyphenation, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.tpHyphenation, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.tpHyphenation, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.tpHyphenation, "ExportThroughPathway.tpHyphenation");
			this.tpHyphenation.Location = new System.Drawing.Point(4, 22);
			this.tpHyphenation.Margin = new System.Windows.Forms.Padding(2);
			this.tpHyphenation.Name = "tpHyphenation";
			this.tpHyphenation.Padding = new System.Windows.Forms.Padding(2);
			this.tpHyphenation.Size = new System.Drawing.Size(365, 238);
			this.tpHyphenation.TabIndex = 3;
			this.tpHyphenation.Text = "Hyphenation";
			this.tpHyphenation.UseVisualStyleBackColor = true;
			// 
			// chkHyphen
			// 
			this.chkHyphen.AutoSize = true;
			this.chkHyphen.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.chkHyphen, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.chkHyphen, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.chkHyphen, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.chkHyphen, "ExportThroughPathway.chkHyphen");
			this.chkHyphen.Location = new System.Drawing.Point(10, 24);
			this.chkHyphen.Margin = new System.Windows.Forms.Padding(2);
			this.chkHyphen.Name = "chkHyphen";
			this.chkHyphen.Size = new System.Drawing.Size(122, 17);
			this.chkHyphen.TabIndex = 2;
			this.chkHyphen.Text = "Enable Hyphenation";
			this.chkHyphen.UseVisualStyleBackColor = true;
			this.chkHyphen.CheckedChanged += new System.EventHandler(this.chkHyphen_CheckedChanged);
			// 
			// gbHyphnSettings
			// 
			this.gbHyphnSettings.Controls.Add(this.clbHyphenlang);
			this.gbHyphnSettings.Controls.Add(this.lblHyphenFonts);
			this.gbHyphnSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.gbHyphnSettings, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.gbHyphnSettings, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.gbHyphnSettings, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.gbHyphnSettings, "ExportThroughPathway.gbHyphnSettings");
			this.gbHyphnSettings.Location = new System.Drawing.Point(10, 58);
			this.gbHyphnSettings.Margin = new System.Windows.Forms.Padding(2);
			this.gbHyphnSettings.Name = "gbHyphnSettings";
			this.gbHyphnSettings.Padding = new System.Windows.Forms.Padding(2);
			this.gbHyphnSettings.Size = new System.Drawing.Size(345, 169);
			this.gbHyphnSettings.TabIndex = 0;
			this.gbHyphnSettings.TabStop = false;
			this.gbHyphnSettings.Text = "Language Settings";
			// 
			// clbHyphenlang
			// 
			this.clbHyphenlang.Font = new System.Drawing.Font("Charis SIL", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.clbHyphenlang.FormattingEnabled = true;
			this.clbHyphenlang.HorizontalScrollbar = true;
			this.clbHyphenlang.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.clbHyphenlang, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.clbHyphenlang, null);
			this.l10NSharpExtender1.SetLocalizingId(this.clbHyphenlang, "ExportThroughPathway.ExportThroughPathway.clbHyphenlang");
			this.clbHyphenlang.Location = new System.Drawing.Point(114, 27);
			this.clbHyphenlang.Margin = new System.Windows.Forms.Padding(2);
			this.clbHyphenlang.MultiColumn = true;
			this.clbHyphenlang.Name = "clbHyphenlang";
			this.clbHyphenlang.Size = new System.Drawing.Size(250, 104);
			this.clbHyphenlang.Sorted = true;
			this.clbHyphenlang.TabIndex = 1;
			// 
			// lblHyphenFonts
			// 
			this.lblHyphenFonts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblHyphenFonts, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblHyphenFonts, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.lblHyphenFonts, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.lblHyphenFonts, "ExportThroughPathway.lblHyphenFonts");
			this.lblHyphenFonts.Location = new System.Drawing.Point(4, 30);
			this.lblHyphenFonts.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblHyphenFonts.Name = "lblHyphenFonts";
			this.lblHyphenFonts.Size = new System.Drawing.Size(107, 26);
			this.lblHyphenFonts.TabIndex = 0;
			this.lblHyphenFonts.Text = "Include Language";
			this.lblHyphenFonts.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// btnHelpShow
			// 
			this.btnHelpShow.AccessibleName = "Preview";
			this.btnHelpShow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnHelpShow.Image = global::SIL.PublishingSolution.Properties.Resources.Help_Image;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnHelpShow, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnHelpShow, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnHelpShow, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnHelpShow, "ExportThroughPathway.btnHelpShow");
			this.btnHelpShow.Location = new System.Drawing.Point(283, 3);
			this.btnHelpShow.Name = "btnHelpShow";
			this.btnHelpShow.Size = new System.Drawing.Size(24, 24);
			this.btnHelpShow.TabIndex = 12;
			this.btnHelpShow.UseVisualStyleBackColor = true;
			this.btnHelpShow.Click += new System.EventHandler(this.btnHelpShow_Click);
			// 
			// BtnBrwsLayout
			// 
			this.BtnBrwsLayout.AccessibleName = "Preview";
			this.BtnBrwsLayout.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnBrwsLayout.BackgroundImage")));
			this.BtnBrwsLayout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.BtnBrwsLayout, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.BtnBrwsLayout, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.BtnBrwsLayout, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.BtnBrwsLayout, "ExportThroughPathway.BtnBrwsLayout");
			this.BtnBrwsLayout.Location = new System.Drawing.Point(283, 33);
			this.BtnBrwsLayout.Name = "BtnBrwsLayout";
			this.BtnBrwsLayout.Size = new System.Drawing.Size(24, 24);
			this.BtnBrwsLayout.TabIndex = 4;
			this.BtnBrwsLayout.UseVisualStyleBackColor = true;
			this.BtnBrwsLayout.Click += new System.EventHandler(this.BtnBrwsLayout_Click);
			// 
			// btnMoreLessOptions
			// 
			this.btnMoreLessOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.btnMoreLessOptions.Image = global::SIL.PublishingSolution.Properties.Resources.go_up;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnMoreLessOptions, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnMoreLessOptions, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnMoreLessOptions, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnMoreLessOptions, "ExportThroughPathway.btnMoreLessOptions");
			this.btnMoreLessOptions.Location = new System.Drawing.Point(99, 66);
			this.btnMoreLessOptions.Name = "btnMoreLessOptions";
			this.btnMoreLessOptions.Size = new System.Drawing.Size(75, 23);
			this.btnMoreLessOptions.TabIndex = 5;
			this.btnMoreLessOptions.Text = "Less";
			this.btnMoreLessOptions.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			this.btnMoreLessOptions.UseVisualStyleBackColor = true;
			this.btnMoreLessOptions.Click += new System.EventHandler(this.btnMoreLessOptions_Click);
			// 
			// ddlStyle
			// 
			this.ddlStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlStyle.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlStyle, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlStyle, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlStyle, "ExportThroughPathway.ExportThroughPathway.ddlStyle");
			this.ddlStyle.Location = new System.Drawing.Point(99, 33);
			this.ddlStyle.Name = "ddlStyle";
			this.ddlStyle.Size = new System.Drawing.Size(178, 21);
			this.ddlStyle.TabIndex = 3;
			this.ddlStyle.SelectedIndexChanged += new System.EventHandler(this.ddlStyle_SelectedIndexChanged);
			// 
			// ddlLayout
			// 
			this.ddlLayout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlLayout.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlLayout, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlLayout, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlLayout, "ExportThroughPathway.ExportThroughPathway.ddlLayout");
			this.ddlLayout.Location = new System.Drawing.Point(99, 3);
			this.ddlLayout.Name = "ddlLayout";
			this.ddlLayout.Size = new System.Drawing.Size(178, 21);
			this.ddlLayout.TabIndex = 1;
			this.ddlLayout.SelectedIndexChanged += new System.EventHandler(this.ddlLayout_SelectedIndexChanged);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tpHyphenation);
			this.tabControl1.Location = new System.Drawing.Point(12, 95);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(373, 264);
			this.tabControl1.TabIndex = 6;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.28571F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.71429F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.BtnBrwsLayout, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.btnHelpShow, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.ddlLayout, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.ddlStyle, 1, 1);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 5);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(323, 60);
			this.tableLayoutPanel1.TabIndex = 13;
			// 
			// ExportThroughPathway
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(397, 467);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.btnMoreLessOptions);
			this.Controls.Add(this.lnkIP);
			this.Controls.Add(this.chkIP);
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.l10NSharpExtender1.SetLocalizableToolTip(this, null);
			this.l10NSharpExtender1.SetLocalizationComment(this, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this, "ExportThroughPathway.WindowTitle");
			this.MaximizeBox = false;
			this.Name = "ExportThroughPathway";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Export Through Pathway";
			this.Load += new System.EventHandler(this.ExportThroughPathway_Load);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ExportThroughPathway_KeyUp);
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).EndInit();
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgCoverImage)).EndInit();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.grpInclude.ResumeLayout(false);
			this.grpInclude.PerformLayout();
			this.tpHyphenation.ResumeLayout(false);
			this.tpHyphenation.PerformLayout();
			this.gbHyphnSettings.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private L10NSharp.UI.L10NSharpExtender l10NSharpExtender1 = null;
        private System.Windows.Forms.Button btnMoreLessOptions;
        private System.Windows.Forms.LinkLabel lnkIP;
        private System.Windows.Forms.CheckBox chkIP;
        private System.Windows.Forms.ComboBox ddlStyle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ddlLayout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRights;
        private System.Windows.Forms.Label lblRights;
        private System.Windows.Forms.TextBox txtBookTitle;
        private System.Windows.Forms.TextBox txtCreator;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblCreator;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblPublisher;
        private System.Windows.Forms.Label lblBookTitle;
        private System.Windows.Forms.TextBox txtPublisher;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkTitlePage;
        private System.Windows.Forms.CheckBox chkCoverImage;
        private System.Windows.Forms.Button btnBrowseColophon;
        private System.Windows.Forms.TextBox txtColophonFile;
        private System.Windows.Forms.CheckBox chkColophon;
        private System.Windows.Forms.Button btnCoverImage;
        private System.Windows.Forms.PictureBox imgCoverImage;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox chkOOReduceStyleNames;
        private System.Windows.Forms.Button btnBrowseSaveInFolder;
        private System.Windows.Forms.TextBox txtSaveInFolder;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkGrammarSketch;
        private System.Windows.Forms.CheckBox chkReversalIndexes;
        private System.Windows.Forms.CheckBox chkConfiguredDictionary;
        private System.Windows.Forms.Button BtnBrwsLayout;
        private System.Windows.Forms.CheckBox chkCoverImageTitle;
        private System.Windows.Forms.ComboBox ddlCopyrightStatement;
        private System.Windows.Forms.RadioButton rdoCustomCopyright;
        private System.Windows.Forms.RadioButton rdoStandardCopyright;
        private System.Windows.Forms.LinkLabel lnkChooseCopyright;
        private System.Windows.Forms.GroupBox grpInclude;
        private System.Windows.Forms.CheckBox chkTOC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox chkLbPreprocess;
        private System.Windows.Forms.Button btnHelpShow;
        private System.Windows.Forms.TabPage tpHyphenation;
        private System.Windows.Forms.GroupBox gbHyphnSettings;
        private System.Windows.Forms.Label lblHyphenFonts;
        private System.Windows.Forms.CheckedListBox clbHyphenlang;
        private System.Windows.Forms.CheckBox chkHyphen;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}