namespace SIL.PublishingSolution
{
    partial class Contents1
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
            this.BtnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ChkFilterReversal = new System.Windows.Forms.CheckBox();
            this.btnXHTMLBrowse = new System.Windows.Forms.Button();
            this.TxtLocation = new System.Windows.Forms.TextBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.TxtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtnExistingDirectory = new System.Windows.Forms.Button();
            this.TxtExistingDirectory = new System.Windows.Forms.TextBox();
            this.lblExistingDirectory = new System.Windows.Forms.Label();
            this.ChkExistingDictionary = new System.Windows.Forms.CheckBox();
            this.ChkFilterLexicon = new System.Windows.Forms.CheckBox();
            this.ChkFilterGrammar = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnOk
            // 
            this.BtnOk.AccessibleName = "BtnOk";
            this.BtnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.BtnOk.ForeColor = System.Drawing.SystemColors.WindowText;
            this.BtnOk.Location = new System.Drawing.Point(458, 251);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(69, 24);
            this.BtnOk.TabIndex = 15;
            this.BtnOk.Text = "&Ok";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnSectionFilterContinue_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleName = "btnCancel";
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnCancel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnCancel.Location = new System.Drawing.Point(539, 251);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 24);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnSectionFilterCancel_Click);
            // 
            // ChkFilterReversal
            // 
            this.ChkFilterReversal.AccessibleName = "ChkFilterReversal";
            this.ChkFilterReversal.AutoSize = true;
            this.ChkFilterReversal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ChkFilterReversal.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ChkFilterReversal.Location = new System.Drawing.Point(128, 120);
            this.ChkFilterReversal.Name = "ChkFilterReversal";
            this.ChkFilterReversal.Size = new System.Drawing.Size(106, 17);
            this.ChkFilterReversal.TabIndex = 13;
            this.ChkFilterReversal.Text = "Include Reversal";
            this.ChkFilterReversal.UseVisualStyleBackColor = true;
            this.ChkFilterReversal.CheckedChanged += new System.EventHandler(this.ChkFilterReversal_CheckedChanged);
            // 
            // btnXHTMLBrowse
            // 
            this.btnXHTMLBrowse.AccessibleName = "btnXHTMLBrowse";
            this.btnXHTMLBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnXHTMLBrowse.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnXHTMLBrowse.Location = new System.Drawing.Point(540, 69);
            this.btnXHTMLBrowse.Name = "btnXHTMLBrowse";
            this.btnXHTMLBrowse.Size = new System.Drawing.Size(69, 23);
            this.btnXHTMLBrowse.TabIndex = 18;
            this.btnXHTMLBrowse.Text = "&Browse...";
            this.btnXHTMLBrowse.UseVisualStyleBackColor = true;
            this.btnXHTMLBrowse.Click += new System.EventHandler(this.btnXHTMLBrowse_Click);
            // 
            // TxtLocation
            // 
            this.TxtLocation.AccessibleName = "TxtLocation";
            this.TxtLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtLocation.ForeColor = System.Drawing.SystemColors.WindowText;
            this.TxtLocation.Location = new System.Drawing.Point(128, 71);
            this.TxtLocation.Name = "TxtLocation";
            this.TxtLocation.Size = new System.Drawing.Size(406, 20);
            this.TxtLocation.TabIndex = 22;
            this.TxtLocation.TextChanged += new System.EventHandler(this.TxtLocation_TextChanged);
            // 
            // lblLocation
            // 
            this.lblLocation.AccessibleName = "lblLocation";
            this.lblLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblLocation.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblLocation.Location = new System.Drawing.Point(12, 74);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(107, 17);
            this.lblLocation.TabIndex = 23;
            this.lblLocation.Text = "Publication Location";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtName
            // 
            this.TxtName.AccessibleName = "TxtName";
            this.TxtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.TxtName.Location = new System.Drawing.Point(128, 45);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(188, 20);
            this.TxtName.TabIndex = 24;
            this.TxtName.TextChanged += new System.EventHandler(this.TxtName_TextChanged);
            // 
            // label1
            // 
            this.label1.AccessibleName = "label1";
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label1.Location = new System.Drawing.Point(12, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 17);
            this.label1.TabIndex = 25;
            this.label1.Text = "Publication Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AccessibleName = "label3";
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label3.Location = new System.Drawing.Point(12, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(415, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Specify the name for the new publication and where you would like the file to be " +
                "stored";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleName = "groupBox1";
            this.groupBox1.Controls.Add(this.BtnExistingDirectory);
            this.groupBox1.Controls.Add(this.TxtExistingDirectory);
            this.groupBox1.Controls.Add(this.lblExistingDirectory);
            this.groupBox1.Controls.Add(this.ChkExistingDictionary);
            this.groupBox1.Location = new System.Drawing.Point(15, 166);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(607, 79);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            // 
            // BtnExistingDirectory
            // 
            this.BtnExistingDirectory.AccessibleName = "BtnExistingDirectory";
            this.BtnExistingDirectory.Enabled = false;
            this.BtnExistingDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.BtnExistingDirectory.ForeColor = System.Drawing.SystemColors.WindowText;
            this.BtnExistingDirectory.Location = new System.Drawing.Point(524, 43);
            this.BtnExistingDirectory.Name = "BtnExistingDirectory";
            this.BtnExistingDirectory.Size = new System.Drawing.Size(69, 23);
            this.BtnExistingDirectory.TabIndex = 31;
            this.BtnExistingDirectory.Text = "&Browse...";
            this.BtnExistingDirectory.UseVisualStyleBackColor = true;
            this.BtnExistingDirectory.Click += new System.EventHandler(this.BtnExistingDirectory_Click);
            // 
            // TxtExistingDirectory
            // 
            this.TxtExistingDirectory.AccessibleName = "TxtExistingDirectory";
            this.TxtExistingDirectory.Enabled = false;
            this.TxtExistingDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.TxtExistingDirectory.ForeColor = System.Drawing.SystemColors.WindowText;
            this.TxtExistingDirectory.Location = new System.Drawing.Point(113, 45);
            this.TxtExistingDirectory.Name = "TxtExistingDirectory";
            this.TxtExistingDirectory.Size = new System.Drawing.Size(404, 20);
            this.TxtExistingDirectory.TabIndex = 32;
            this.TxtExistingDirectory.TextChanged += new System.EventHandler(this.TxtExistingDirectory_TextChanged);
            // 
            // lblExistingDirectory
            // 
            this.lblExistingDirectory.AccessibleName = "lblExistingDirectory";
            this.lblExistingDirectory.Enabled = false;
            this.lblExistingDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblExistingDirectory.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lblExistingDirectory.Location = new System.Drawing.Point(6, 46);
            this.lblExistingDirectory.Name = "lblExistingDirectory";
            this.lblExistingDirectory.Size = new System.Drawing.Size(104, 17);
            this.lblExistingDirectory.TabIndex = 33;
            this.lblExistingDirectory.Text = "Existing Publication";
            this.lblExistingDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChkExistingDictionary
            // 
            this.ChkExistingDictionary.AccessibleName = "ChkExistingDictionary";
            this.ChkExistingDictionary.AutoSize = true;
            this.ChkExistingDictionary.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ChkExistingDictionary.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ChkExistingDictionary.Location = new System.Drawing.Point(113, 20);
            this.ChkExistingDictionary.Name = "ChkExistingDictionary";
            this.ChkExistingDictionary.Size = new System.Drawing.Size(182, 17);
            this.ChkExistingDictionary.TabIndex = 30;
            this.ChkExistingDictionary.Text = "Get data from existing publication";
            this.ChkExistingDictionary.UseVisualStyleBackColor = true;
            this.ChkExistingDictionary.CheckedChanged += new System.EventHandler(this.ChkExistingDictionary_CheckedChanged);
            // 
            // ChkFilterLexicon
            // 
            this.ChkFilterLexicon.AccessibleName = "ChkFilterLexicon";
            this.ChkFilterLexicon.AutoSize = true;
            this.ChkFilterLexicon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ChkFilterLexicon.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ChkFilterLexicon.Location = new System.Drawing.Point(128, 97);
            this.ChkFilterLexicon.Name = "ChkFilterLexicon";
            this.ChkFilterLexicon.Size = new System.Drawing.Size(165, 17);
            this.ChkFilterLexicon.TabIndex = 32;
            this.ChkFilterLexicon.Text = "Include Configured Dictionary";
            this.ChkFilterLexicon.UseVisualStyleBackColor = true;
            this.ChkFilterLexicon.CheckedChanged += new System.EventHandler(this.ChkFilterLexicon_CheckedChanged);
            // 
            // ChkFilterGrammar
            // 
            this.ChkFilterGrammar.AccessibleName = "ChkFilterGrammar";
            this.ChkFilterGrammar.AutoSize = true;
            this.ChkFilterGrammar.Enabled = false;
            this.ChkFilterGrammar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ChkFilterGrammar.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ChkFilterGrammar.Location = new System.Drawing.Point(128, 143);
            this.ChkFilterGrammar.Name = "ChkFilterGrammar";
            this.ChkFilterGrammar.Size = new System.Drawing.Size(106, 17);
            this.ChkFilterGrammar.TabIndex = 33;
            this.ChkFilterGrammar.Text = "Include Grammar";
            this.ChkFilterGrammar.UseVisualStyleBackColor = true;
            // 
            // Contents1
            // 
            this.AccessibleName = "Contents";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 282);
            this.Controls.Add(this.ChkFilterGrammar);
            this.Controls.Add(this.ChkFilterLexicon);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnXHTMLBrowse);
            this.Controls.Add(this.TxtLocation);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.ChkFilterReversal);
            this.Name = "Contents1";
            this.ShowIcon = false;
            this.Text = "Choose Contents";
            this.Load += new System.EventHandler(this.Contents_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox ChkFilterReversal;
        private System.Windows.Forms.Button btnXHTMLBrowse;
        private System.Windows.Forms.TextBox TxtLocation;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.TextBox TxtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BtnExistingDirectory;
        private System.Windows.Forms.TextBox TxtExistingDirectory;
        private System.Windows.Forms.Label lblExistingDirectory;
        private System.Windows.Forms.CheckBox ChkExistingDictionary;
        private System.Windows.Forms.CheckBox ChkFilterLexicon;
        private System.Windows.Forms.CheckBox ChkFilterGrammar;

    }
}
