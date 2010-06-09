namespace JWTools
{
    partial class DlgNewLanguage
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
            this.m_btnCancel = new System.Windows.Forms.Button();
            this.m_btnOK = new System.Windows.Forms.Button();
            this.m_lblAbbrev = new System.Windows.Forms.Label();
            this.m_textAbbrev = new System.Windows.Forms.TextBox();
            this.m_lblAbbrevInfo = new System.Windows.Forms.Label();
            this.m_lblNameInfo = new System.Windows.Forms.Label();
            this.m_textName = new System.Windows.Forms.TextBox();
            this.m_lblName = new System.Windows.Forms.Label();
            this.m_lblFontInfo = new System.Windows.Forms.Label();
            this.m_lblFont = new System.Windows.Forms.Label();
            this.m_comboFontName = new System.Windows.Forms.ComboBox();
            this.m_comboFontSize = new System.Windows.Forms.ComboBox();
            this.m_lblAbbrevError = new System.Windows.Forms.Label();
            this.m_lblNameError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_btnCancel
            // 
            this.m_btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_btnCancel.Location = new System.Drawing.Point(224, 248);
            this.m_btnCancel.AccessibleName = "m_btnCancel";
            this.m_btnCancel.Name = "m_btnCancel";
            this.m_btnCancel.Size = new System.Drawing.Size(75, 23);
            this.m_btnCancel.TabIndex = 16;
            this.m_btnCancel.Text = "Cancel";
            // 
            // m_btnOK
            // 
            this.m_btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_btnOK.Location = new System.Drawing.Point(136, 248);
            this.m_btnOK.AccessibleName = "m_btnOK";
            this.m_btnOK.Name = "m_btnOK";
            this.m_btnOK.Size = new System.Drawing.Size(75, 23);
            this.m_btnOK.TabIndex = 15;
            this.m_btnOK.Text = "Create";
            this.m_btnOK.Click += new System.EventHandler(this.m_btnOK_Click);
            // 
            // m_lblAbbrev
            // 
            this.m_lblAbbrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblAbbrev.Location = new System.Drawing.Point(12, 9);
            this.m_lblAbbrev.AccessibleName = "m_lblAbbrev";
            this.m_lblAbbrev.Name = "m_lblAbbrev";
            this.m_lblAbbrev.Size = new System.Drawing.Size(96, 23);
            this.m_lblAbbrev.TabIndex = 17;
            this.m_lblAbbrev.Text = "Abbreviation:";
            this.m_lblAbbrev.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_textAbbrev
            // 
            this.m_textAbbrev.Location = new System.Drawing.Point(116, 9);
            this.m_textAbbrev.AccessibleName = "m_textAbbrev";
            this.m_textAbbrev.Name = "m_textAbbrev";
            this.m_textAbbrev.Size = new System.Drawing.Size(294, 20);
            this.m_textAbbrev.TabIndex = 18;
            this.m_textAbbrev.TextChanged += new System.EventHandler(this.cmdAbbrevChanged);
            // 
            // m_lblAbbrevInfo
            // 
            this.m_lblAbbrevInfo.Location = new System.Drawing.Point(12, 32);
            this.m_lblAbbrevInfo.AccessibleName = "m_lblAbbrevInfo";
            this.m_lblAbbrevInfo.Name = "m_lblAbbrevInfo";
            this.m_lblAbbrevInfo.Size = new System.Drawing.Size(398, 23);
            this.m_lblAbbrevInfo.TabIndex = 19;
            this.m_lblAbbrevInfo.Text = "This is typically the Ethnologue\'s 3-letter code.";
            this.m_lblAbbrevInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_lblNameInfo
            // 
            this.m_lblNameInfo.Location = new System.Drawing.Point(12, 101);
            this.m_lblNameInfo.AccessibleName = "m_lblNameInfo";
            this.m_lblNameInfo.Name = "m_lblNameInfo";
            this.m_lblNameInfo.Size = new System.Drawing.Size(398, 49);
            this.m_lblNameInfo.TabIndex = 22;
            this.m_lblNameInfo.Text = "The name of the language, as it will appear in the user interface. You should use" +
                " the local name, rather than the English one (thus, for example, \"Espanol\" rathe" +
                "r than \"Spanish.\")";
            // 
            // m_textName
            // 
            this.m_textName.Location = new System.Drawing.Point(116, 78);
            this.m_textName.AccessibleName = "m_textName";
            this.m_textName.Name = "m_textName";
            this.m_textName.Size = new System.Drawing.Size(294, 20);
            this.m_textName.TabIndex = 21;
            this.m_textName.TextChanged += new System.EventHandler(this.cmdNameChanged);
            // 
            // m_lblName
            // 
            this.m_lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblName.Location = new System.Drawing.Point(12, 78);
            this.m_lblName.AccessibleName = "m_lblName";
            this.m_lblName.Name = "m_lblName";
            this.m_lblName.Size = new System.Drawing.Size(96, 23);
            this.m_lblName.TabIndex = 20;
            this.m_lblName.Text = "Name:";
            this.m_lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_lblFontInfo
            // 
            this.m_lblFontInfo.Location = new System.Drawing.Point(12, 196);
            this.m_lblFontInfo.AccessibleName = "m_lblFontInfo";
            this.m_lblFontInfo.Name = "m_lblFontInfo";
            this.m_lblFontInfo.Size = new System.Drawing.Size(398, 36);
            this.m_lblFontInfo.TabIndex = 24;
            this.m_lblFontInfo.Text = "You can leave this blank to have Windows choose; or you can pick a font that show" +
                "s this language better.";
            // 
            // m_lblFont
            // 
            this.m_lblFont.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblFont.Location = new System.Drawing.Point(12, 173);
            this.m_lblFont.AccessibleName = "m_lblFont";
            this.m_lblFont.Name = "m_lblFont";
            this.m_lblFont.Size = new System.Drawing.Size(96, 23);
            this.m_lblFont.TabIndex = 23;
            this.m_lblFont.Text = "Font && Size:";
            this.m_lblFont.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_comboFontName
            // 
            this.m_comboFontName.FormattingEnabled = true;
            this.m_comboFontName.Location = new System.Drawing.Point(116, 175);
            this.m_comboFontName.AccessibleName = "m_comboFontName";
            this.m_comboFontName.Name = "m_comboFontName";
            this.m_comboFontName.Size = new System.Drawing.Size(177, 21);
            this.m_comboFontName.TabIndex = 25;
            // 
            // m_comboFontSize
            // 
            this.m_comboFontSize.FormattingEnabled = true;
            this.m_comboFontSize.Location = new System.Drawing.Point(299, 175);
            this.m_comboFontSize.AccessibleName = "m_comboFontSize";
            this.m_comboFontSize.Name = "m_comboFontSize";
            this.m_comboFontSize.Size = new System.Drawing.Size(111, 21);
            this.m_comboFontSize.TabIndex = 26;
            // 
            // m_lblAbbrevError
            // 
            this.m_lblAbbrevError.ForeColor = System.Drawing.Color.Red;
            this.m_lblAbbrevError.Location = new System.Drawing.Point(12, 55);
            this.m_lblAbbrevError.AccessibleName = "m_lblAbbrevError";
            this.m_lblAbbrevError.Name = "m_lblAbbrevError";
            this.m_lblAbbrevError.Size = new System.Drawing.Size(398, 23);
            this.m_lblAbbrevError.TabIndex = 27;
            // 
            // m_lblNameError
            // 
            this.m_lblNameError.ForeColor = System.Drawing.Color.Red;
            this.m_lblNameError.Location = new System.Drawing.Point(12, 150);
            this.m_lblNameError.AccessibleName = "m_lblNameError";
            this.m_lblNameError.Name = "m_lblNameError";
            this.m_lblNameError.Size = new System.Drawing.Size(398, 23);
            this.m_lblNameError.TabIndex = 28;
            // 
            // DlgNewLanguage
            // 
            this.AcceptButton = this.m_btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.m_btnCancel;
            this.ClientSize = new System.Drawing.Size(422, 284);
            this.ControlBox = false;
            this.Controls.Add(this.m_lblNameError);
            this.Controls.Add(this.m_lblAbbrevError);
            this.Controls.Add(this.m_comboFontSize);
            this.Controls.Add(this.m_comboFontName);
            this.Controls.Add(this.m_lblFontInfo);
            this.Controls.Add(this.m_lblFont);
            this.Controls.Add(this.m_lblNameInfo);
            this.Controls.Add(this.m_textName);
            this.Controls.Add(this.m_lblName);
            this.Controls.Add(this.m_lblAbbrevInfo);
            this.Controls.Add(this.m_textAbbrev);
            this.Controls.Add(this.m_lblAbbrev);
            this.Controls.Add(this.m_btnCancel);
            this.Controls.Add(this.m_btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.AccessibleName = "DlgNewLanguage";
            this.Name = "DlgNewLanguage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Localize to a New Language";
            this.Load += new System.EventHandler(this.cmdLoad);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.cmdClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btnCancel;
        private System.Windows.Forms.Button m_btnOK;
        private System.Windows.Forms.Label m_lblAbbrev;
        private System.Windows.Forms.TextBox m_textAbbrev;
        private System.Windows.Forms.Label m_lblAbbrevInfo;
        private System.Windows.Forms.Label m_lblNameInfo;
        private System.Windows.Forms.TextBox m_textName;
        private System.Windows.Forms.Label m_lblName;
        private System.Windows.Forms.Label m_lblFontInfo;
        private System.Windows.Forms.Label m_lblFont;
        private System.Windows.Forms.ComboBox m_comboFontName;
        private System.Windows.Forms.ComboBox m_comboFontSize;
        private System.Windows.Forms.Label m_lblAbbrevError;
        private System.Windows.Forms.Label m_lblNameError;
    }
}
