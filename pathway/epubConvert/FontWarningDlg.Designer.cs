namespace epubConvert
{
    partial class FontWarningDlg
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
            this.icnWarning = new System.Windows.Forms.PictureBox();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.lblSubstituteSILFont = new System.Windows.Forms.Label();
            this.ddlSILFonts = new System.Windows.Forms.ComboBox();
            this.rdoConvertToSILFont = new System.Windows.Forms.RadioButton();
            this.rdoEmbedFont = new System.Windows.Forms.RadioButton();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtWarning = new System.Windows.Forms.TextBox();
            this.chkRepeatAction = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.icnWarning)).BeginInit();
            this.grpOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // icnWarning
            // 
            this.icnWarning.Location = new System.Drawing.Point(12, 12);
            this.icnWarning.Name = "icnWarning";
            this.icnWarning.Size = new System.Drawing.Size(32, 32);
            this.icnWarning.TabIndex = 1;
            this.icnWarning.TabStop = false;
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.lblSubstituteSILFont);
            this.grpOptions.Controls.Add(this.ddlSILFonts);
            this.grpOptions.Controls.Add(this.rdoConvertToSILFont);
            this.grpOptions.Controls.Add(this.rdoEmbedFont);
            this.grpOptions.Location = new System.Drawing.Point(12, 80);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(410, 75);
            this.grpOptions.TabIndex = 3;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "grpOptions";
            // 
            // lblSubstituteSILFont
            // 
            this.lblSubstituteSILFont.AutoSize = true;
            this.lblSubstituteSILFont.Location = new System.Drawing.Point(45, 46);
            this.lblSubstituteSILFont.Name = "lblSubstituteSILFont";
            this.lblSubstituteSILFont.Size = new System.Drawing.Size(101, 13);
            this.lblSubstituteSILFont.TabIndex = 3;
            this.lblSubstituteSILFont.Text = "lblSubstituteSILFont";
            this.lblSubstituteSILFont.Visible = false;
            // 
            // ddlSILFonts
            // 
            this.ddlSILFonts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSILFonts.Enabled = false;
            this.ddlSILFonts.FormattingEnabled = true;
            this.ddlSILFonts.Location = new System.Drawing.Point(191, 43);
            this.ddlSILFonts.Name = "ddlSILFonts";
            this.ddlSILFonts.Size = new System.Drawing.Size(213, 21);
            this.ddlSILFonts.TabIndex = 2;
            this.ddlSILFonts.SelectedIndexChanged += new System.EventHandler(this.ddlSILFonts_SelectedIndexChanged);
            // 
            // rdoConvertToSILFont
            // 
            this.rdoConvertToSILFont.AutoSize = true;
            this.rdoConvertToSILFont.Checked = true;
            this.rdoConvertToSILFont.Location = new System.Drawing.Point(7, 44);
            this.rdoConvertToSILFont.Name = "rdoConvertToSILFont";
            this.rdoConvertToSILFont.Size = new System.Drawing.Size(127, 17);
            this.rdoConvertToSILFont.TabIndex = 1;
            this.rdoConvertToSILFont.TabStop = true;
            this.rdoConvertToSILFont.Text = "rdoConvertToSILFont";
            this.rdoConvertToSILFont.UseVisualStyleBackColor = true;
            this.rdoConvertToSILFont.CheckedChanged += new System.EventHandler(this.rdoConvertToSILFont_CheckedChanged);
            // 
            // rdoEmbedFont
            // 
            this.rdoEmbedFont.AutoSize = true;
            this.rdoEmbedFont.Location = new System.Drawing.Point(7, 21);
            this.rdoEmbedFont.Name = "rdoEmbedFont";
            this.rdoEmbedFont.Size = new System.Drawing.Size(94, 17);
            this.rdoEmbedFont.TabIndex = 0;
            this.rdoEmbedFont.TabStop = true;
            this.rdoEmbedFont.Text = "rdoEmbedFont";
            this.rdoEmbedFont.UseVisualStyleBackColor = true;
            this.rdoEmbedFont.CheckedChanged += new System.EventHandler(this.rdoEmbedFont_CheckedChanged);
            // 
            // btn_OK
            // 
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.Location = new System.Drawing.Point(260, 168);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 4;
            this.btn_OK.Text = "&OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(341, 168);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtWarning
            // 
            this.txtWarning.BackColor = System.Drawing.SystemColors.Control;
            this.txtWarning.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtWarning.Location = new System.Drawing.Point(60, 12);
            this.txtWarning.Multiline = true;
            this.txtWarning.Name = "txtWarning";
            this.txtWarning.ReadOnly = true;
            this.txtWarning.Size = new System.Drawing.Size(362, 62);
            this.txtWarning.TabIndex = 6;
            // 
            // chkRepeatAction
            // 
            this.chkRepeatAction.AutoSize = true;
            this.chkRepeatAction.Location = new System.Drawing.Point(13, 172);
            this.chkRepeatAction.Name = "chkRepeatAction";
            this.chkRepeatAction.Size = new System.Drawing.Size(109, 17);
            this.chkRepeatAction.TabIndex = 7;
            this.chkRepeatAction.Text = "chkRepeatAction";
            this.chkRepeatAction.UseVisualStyleBackColor = true;
            this.chkRepeatAction.Visible = false;
            this.chkRepeatAction.CheckedChanged += new System.EventHandler(this.chkRepeatAction_CheckedChanged);
            // 
            // FontWarningDlg
            // 
            this.AcceptButton = this.btn_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 202);
            this.Controls.Add(this.chkRepeatAction);
            this.Controls.Add(this.txtWarning);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.icnWarning);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FontWarningDlg";
            this.ShowIcon = false;
            this.Text = "FontWarningDlg";
            this.Load += new System.EventHandler(this.FontWarningDlg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.icnWarning)).EndInit();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox icnWarning;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.RadioButton rdoConvertToSILFont;
        private System.Windows.Forms.RadioButton rdoEmbedFont;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox ddlSILFonts;
        private System.Windows.Forms.TextBox txtWarning;
        private System.Windows.Forms.CheckBox chkRepeatAction;
        private System.Windows.Forms.Label lblSubstituteSILFont;
    }
}