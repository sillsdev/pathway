namespace SIL.PublishingSolution
{
    partial class ConfigureStylesheetDialog
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
            this.TxtStyleSheetName = new System.Windows.Forms.TextBox();
            this.BtOk = new System.Windows.Forms.Button();
            this.BtBrowse = new System.Windows.Forms.Button();
            this.BtCancel = new System.Windows.Forms.Button();
            this.TxtCSSFileName = new System.Windows.Forms.TextBox();
            this.TxtDescription = new System.Windows.Forms.TextBox();
            this.LblStyleName = new System.Windows.Forms.Label();
            this.LblCSSFileName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TxtStyleSheetName
            // 
            this.TxtStyleSheetName.Location = new System.Drawing.Point(135, 16);
            this.TxtStyleSheetName.Name = "TxtStyleSheetName";
            this.TxtStyleSheetName.Size = new System.Drawing.Size(267, 20);
            this.TxtStyleSheetName.TabIndex = 0;
            this.TxtStyleSheetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtStyleSheetName_KeyPress);
            // 
            // BtOk
            // 
            this.BtOk.Location = new System.Drawing.Point(327, 185);
            this.BtOk.Name = "BtOk";
            this.BtOk.Size = new System.Drawing.Size(75, 23);
            this.BtOk.TabIndex = 1;
            this.BtOk.Text = "&Ok";
            this.BtOk.UseVisualStyleBackColor = true;
            this.BtOk.Click += new System.EventHandler(this.BtOk_Click);
            // 
            // BtBrowse
            // 
            this.BtBrowse.Location = new System.Drawing.Point(408, 43);
            this.BtBrowse.Name = "BtBrowse";
            this.BtBrowse.Size = new System.Drawing.Size(75, 23);
            this.BtBrowse.TabIndex = 2;
            this.BtBrowse.Text = "&Browse";
            this.BtBrowse.UseVisualStyleBackColor = true;
            this.BtBrowse.Click += new System.EventHandler(this.BtBrowse_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.Location = new System.Drawing.Point(408, 185);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(75, 23);
            this.BtCancel.TabIndex = 3;
            this.BtCancel.Text = "&Cancel";
            this.BtCancel.UseVisualStyleBackColor = true;
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // TxtCSSFileName
            // 
            this.TxtCSSFileName.Enabled = false;
            this.TxtCSSFileName.Location = new System.Drawing.Point(135, 44);
            this.TxtCSSFileName.Name = "TxtCSSFileName";
            this.TxtCSSFileName.ReadOnly = true;
            this.TxtCSSFileName.Size = new System.Drawing.Size(267, 20);
            this.TxtCSSFileName.TabIndex = 4;
            // 
            // TxtDescription
            // 
            this.TxtDescription.AcceptsReturn = true;
            this.TxtDescription.Location = new System.Drawing.Point(135, 76);
            this.TxtDescription.Multiline = true;
            this.TxtDescription.Name = "TxtDescription";
            this.TxtDescription.Size = new System.Drawing.Size(348, 86);
            this.TxtDescription.TabIndex = 5;
            // 
            // LblStyleName
            // 
            this.LblStyleName.AutoSize = true;
            this.LblStyleName.Location = new System.Drawing.Point(17, 19);
            this.LblStyleName.Name = "LblStyleName";
            this.LblStyleName.Size = new System.Drawing.Size(112, 13);
            this.LblStyleName.TabIndex = 6;
            this.LblStyleName.Text = "New Stylesheet Name";
            // 
            // LblCSSFileName
            // 
            this.LblCSSFileName.AutoSize = true;
            this.LblCSSFileName.Location = new System.Drawing.Point(54, 48);
            this.LblCSSFileName.Name = "LblCSSFileName";
            this.LblCSSFileName.Size = new System.Drawing.Size(75, 13);
            this.LblCSSFileName.TabIndex = 7;
            this.LblCSSFileName.Text = "CSS FileName";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(69, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Description";
            // 
            // ConfigureStylesheetDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 223);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LblCSSFileName);
            this.Controls.Add(this.LblStyleName);
            this.Controls.Add(this.TxtDescription);
            this.Controls.Add(this.TxtCSSFileName);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtBrowse);
            this.Controls.Add(this.BtOk);
            this.Controls.Add(this.TxtStyleSheetName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigureStylesheetDialog";
            this.ShowIcon = false;
            this.Text = "Configure Stylesheet Dialog";
            this.Load += new System.EventHandler(this.ConfigureStylesheetDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TxtStyleSheetName;
        private System.Windows.Forms.Button BtOk;
        private System.Windows.Forms.Button BtBrowse;
        private System.Windows.Forms.Button BtCancel;
        private System.Windows.Forms.TextBox TxtCSSFileName;
        private System.Windows.Forms.TextBox TxtDescription;
        private System.Windows.Forms.Label LblStyleName;
        private System.Windows.Forms.Label LblCSSFileName;
        private System.Windows.Forms.Label label3;
    }
}