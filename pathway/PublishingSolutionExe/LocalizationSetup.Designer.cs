namespace SIL.PublishingSolution
{
    partial class LocalizationSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocalizationSetup));
            this.cmbLanguages = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.CmbFontName = new System.Windows.Forms.ComboBox();
            this.CmbFontSize = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CmdClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbLanguages
            // 
            this.cmbLanguages.AccessibleName = "cmbLanguages";
            this.cmbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguages.FormattingEnabled = true;
            this.cmbLanguages.Location = new System.Drawing.Point(177, 22);
            this.cmbLanguages.Name = "cmbLanguages";
            this.cmbLanguages.Size = new System.Drawing.Size(213, 21);
            this.cmbLanguages.TabIndex = 0;
            this.cmbLanguages.SelectedValueChanged += new System.EventHandler(this.cmbLanguages_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AccessibleName = "label1";
            this.label1.Location = new System.Drawing.Point(4, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Primary Language";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmdOK
            // 
            this.cmdOK.AccessibleName = "cmdOK";
            this.cmdOK.Location = new System.Drawing.Point(210, 112);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(87, 28);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "&Apply";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // CmbFontName
            // 
            this.CmbFontName.AccessibleName = "CmbFontName";
            this.CmbFontName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbFontName.FormattingEnabled = true;
            this.CmbFontName.Location = new System.Drawing.Point(177, 49);
            this.CmbFontName.Name = "CmbFontName";
            this.CmbFontName.Size = new System.Drawing.Size(213, 21);
            this.CmbFontName.TabIndex = 4;
            this.CmbFontName.SelectedIndexChanged += new System.EventHandler(this.CmbFontName_SelectedIndexChanged);
            // 
            // CmbFontSize
            // 
            this.CmbFontSize.AccessibleName = "CmbFontSize";
            this.CmbFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbFontSize.FormattingEnabled = true;
            this.CmbFontSize.Location = new System.Drawing.Point(177, 76);
            this.CmbFontSize.Name = "CmbFontSize";
            this.CmbFontSize.Size = new System.Drawing.Size(213, 21);
            this.CmbFontSize.TabIndex = 6;
            this.CmbFontSize.SelectedIndexChanged += new System.EventHandler(this.CmbFontSize_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AccessibleName = "label2";
            this.label2.Location = new System.Drawing.Point(4, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 19);
            this.label2.TabIndex = 7;
            this.label2.Text = "Font Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AccessibleName = "label3";
            this.label3.Location = new System.Drawing.Point(4, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "Font Size";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CmdClose
            // 
            this.CmdClose.AccessibleName = "CmdClose";
            this.CmdClose.Location = new System.Drawing.Point(303, 112);
            this.CmdClose.Name = "CmdClose";
            this.CmdClose.Size = new System.Drawing.Size(87, 28);
            this.CmdClose.TabIndex = 9;
            this.CmdClose.Text = "&Close";
            this.CmdClose.UseVisualStyleBackColor = true;
            this.CmdClose.Click += new System.EventHandler(this.CmdClose_Click);
            // 
            // LocalizationSetup
            // 
            this.AccessibleName = "LocalizationSetup";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 155);
            this.Controls.Add(this.CmdClose);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CmbFontSize);
            this.Controls.Add(this.CmbFontName);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbLanguages);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LocalizationSetup";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Localization Setup";
            this.Load += new System.EventHandler(this.LocalizationSetup_Load);
            this.DoubleClick += new System.EventHandler(this.LocalizationSetup_DoubleClick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbLanguages;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.ComboBox CmbFontName;
        private System.Windows.Forms.ComboBox CmbFontSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CmdClose;
    }
}
