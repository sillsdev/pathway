using System.Windows.Forms;

namespace SIL.PublishingSolution
{
    partial class UILanguageDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UILanguageDialog));
			this.l10NSharpExtender1 = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.lblUserInterface = new System.Windows.Forms.Label();
			this.lblFontName = new System.Windows.Forms.Label();
			this.lblFontSize = new System.Windows.Forms.Label();
			this.ddlUILanguage = new System.Windows.Forms.ComboBox();
			this.ddlFontName = new System.Windows.Forms.ComboBox();
			this.ddlFontSize = new System.Windows.Forms.ComboBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rtbPreview = new System.Windows.Forms.RichTextBox();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// l10NSharpExtender1
			// 
			this.l10NSharpExtender1.LocalizationManagerId = "Pathway";
			this.l10NSharpExtender1.PrefixForNewItems = "UILanguageDialog";
			// 
			// lblUserInterface
			// 
			this.lblUserInterface.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.lblUserInterface, 2);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblUserInterface, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblUserInterface, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblUserInterface, "UILanguageDialog.lblUserInterface");
			this.lblUserInterface.Location = new System.Drawing.Point(3, 0);
			this.lblUserInterface.Name = "lblUserInterface";
			this.lblUserInterface.Size = new System.Drawing.Size(128, 13);
			this.lblUserInterface.TabIndex = 0;
			this.lblUserInterface.Text = "User Interface Language:";
			// 
			// lblFontName
			// 
			this.lblFontName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFontName.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblFontName, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblFontName, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblFontName, "UILanguageDialog.lblFontName");
			this.lblFontName.Location = new System.Drawing.Point(24, 51);
			this.lblFontName.Name = "lblFontName";
			this.lblFontName.Size = new System.Drawing.Size(28, 13);
			this.lblFontName.TabIndex = 3;
			this.lblFontName.Text = "Font";
			// 
			// lblFontSize
			// 
			this.lblFontSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblFontSize.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblFontSize, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblFontSize, null);
			this.l10NSharpExtender1.SetLocalizingId(this.lblFontSize, "UILanguageDialog.lblFontSize");
			this.lblFontSize.Location = new System.Drawing.Point(3, 82);
			this.lblFontSize.Name = "lblFontSize";
			this.lblFontSize.Size = new System.Drawing.Size(49, 13);
			this.lblFontSize.TabIndex = 5;
			this.lblFontSize.Text = "Font size";
			// 
			// ddlUILanguage
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.ddlUILanguage, 2);
			this.ddlUILanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlUILanguage.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlUILanguage, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlUILanguage, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlUILanguage, "UILanguageDialog.UILanguageDialog.ddlUILanguage");
			this.ddlUILanguage.Location = new System.Drawing.Point(3, 23);
			this.ddlUILanguage.Name = "ddlUILanguage";
			this.ddlUILanguage.Size = new System.Drawing.Size(222, 21);
			this.ddlUILanguage.TabIndex = 1;
			this.ddlUILanguage.SelectedIndexChanged += new System.EventHandler(this.ddlUILanguage_SelectedIndexChanged);
			// 
			// ddlFontName
			// 
			this.ddlFontName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlFontName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlFontName.IntegralHeight = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlFontName, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlFontName, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlFontName, "UILanguageDialog.UILanguageDialog.ddlFontName");
			this.ddlFontName.Location = new System.Drawing.Point(58, 54);
			this.ddlFontName.Name = "ddlFontName";
			this.ddlFontName.Size = new System.Drawing.Size(167, 21);
			this.ddlFontName.TabIndex = 2;
			this.ddlFontName.SelectedIndexChanged += new System.EventHandler(this.ddlFontName_SelectedIndexChanged);
			// 
			// ddlFontSize
			// 
			this.ddlFontSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlFontSize.FormattingEnabled = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.ddlFontSize, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.ddlFontSize, null);
			this.l10NSharpExtender1.SetLocalizingId(this.ddlFontSize, "UILanguageDialog.UILanguageDialog.ddlFontSize");
			this.ddlFontSize.Location = new System.Drawing.Point(58, 85);
			this.ddlFontSize.Name = "ddlFontSize";
			this.ddlFontSize.Size = new System.Drawing.Size(167, 21);
			this.ddlFontSize.TabIndex = 3;
			this.ddlFontSize.SelectedIndexChanged += new System.EventHandler(this.ddlFontSize_SelectedIndexChanged);
			// 
			// btnOk
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnOk, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnOk, null);
			this.l10NSharpExtender1.SetLocalizingId(this.btnOk, "UILanguageDialog.UILanguageDialog.btnOk");
			this.btnOk.Location = new System.Drawing.Point(246, 163);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 28);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "&Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnCancel, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnCancel, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnCancel, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnCancel, "UILanguageDialog.UILanguageDialog.btnCancel");
			this.btnCancel.Location = new System.Drawing.Point(328, 163);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 28);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rtbPreview);
			this.l10NSharpExtender1.SetLocalizableToolTip(this.groupBox1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.groupBox1, null);
			this.l10NSharpExtender1.SetLocalizingId(this.groupBox1, "UILanguageDialog.groupBox1");
			this.groupBox1.Location = new System.Drawing.Point(246, 17);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(157, 100);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Preview";
			// 
			// rtbPreview
			// 
			this.rtbPreview.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.rtbPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtbPreview.Location = new System.Drawing.Point(6, 18);
			this.rtbPreview.Name = "rtbPreview";
			this.rtbPreview.Size = new System.Drawing.Size(145, 73);
			this.rtbPreview.TabIndex = 8;
			this.rtbPreview.Text = "";
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.linkLabel1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.linkLabel1, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.linkLabel1, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.linkLabel1, "UILanguageDialog.UILanguageDialog.linkLabel1");
			this.linkLabel1.Location = new System.Drawing.Point(6, 136);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(231, 13);
			this.linkLabel1.TabIndex = 2;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "I want to localize Pathway for another language";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.lblUserInterface, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.ddlUILanguage, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblFontName, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.ddlFontName, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.ddlFontSize, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblFontSize, 0, 3);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(228, 112);
			this.tableLayoutPanel1.TabIndex = 12;
			// 
			// UILanguageDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(415, 197);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.l10NSharpExtender1.SetLocalizableToolTip(this, null);
			this.l10NSharpExtender1.SetLocalizationComment(this, null);
			this.l10NSharpExtender1.SetLocalizingId(this, "UILanguageDialog.WindowTitle");
			this.Name = "UILanguageDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "User Interface Language";
			this.Load += new System.EventHandler(this.UILanguageDialog_Load);
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUserInterface;
        private System.Windows.Forms.ComboBox ddlUILanguage;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label lblFontName;
        private System.Windows.Forms.ComboBox ddlFontName;
        private System.Windows.Forms.Label lblFontSize;
        private System.Windows.Forms.ComboBox ddlFontSize;
        private System.Windows.Forms.RichTextBox rtbPreview;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private L10NSharp.UI.L10NSharpExtender l10NSharpExtender1;
        private GroupBox groupBox1;
		private TableLayoutPanel tableLayoutPanel1;
    }
}