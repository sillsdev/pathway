namespace SIL.PublishingSolution
{
    partial class BackUp
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
            this.ChkListBox = new System.Windows.Forms.CheckedListBox();
            this.Btn_BackUp = new System.Windows.Forms.Button();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtBackupPath = new System.Windows.Forms.TextBox();
            this.lblXHTML = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ChkListBox
            // 
            this.ChkListBox.AccessibleName = "ChkListBox";
            this.ChkListBox.CheckOnClick = true;
            this.ChkListBox.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkListBox.FormattingEnabled = true;
            this.ChkListBox.Location = new System.Drawing.Point(11, 95);
            this.ChkListBox.Name = "ChkListBox";
            this.ChkListBox.Size = new System.Drawing.Size(379, 293);
            this.ChkListBox.TabIndex = 0;
            // 
            // Btn_BackUp
            // 
            this.Btn_BackUp.AccessibleName = "Btn_BackUp";
            this.Btn_BackUp.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Btn_BackUp.Enabled = false;
            this.Btn_BackUp.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_BackUp.Location = new System.Drawing.Point(233, 394);
            this.Btn_BackUp.Name = "Btn_BackUp";
            this.Btn_BackUp.Size = new System.Drawing.Size(75, 23);
            this.Btn_BackUp.TabIndex = 1;
            this.Btn_BackUp.Text = "B&ackup";
            this.Btn_BackUp.UseVisualStyleBackColor = true;
            this.Btn_BackUp.Click += new System.EventHandler(this.button1_Click);
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.AccessibleName = "Btn_Cancel";
            this.Btn_Cancel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Cancel.Location = new System.Drawing.Point(314, 394);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.Btn_Cancel.TabIndex = 2;
            this.Btn_Cancel.Text = "&Cancel";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // label1
            // 
            this.label1.AccessibleName = "label1";
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label1.Location = new System.Drawing.Point(13, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "Include the following fonts in backup folder";
            // 
            // btnBrowse
            // 
            this.btnBrowse.AccessibleName = "btnBrowse";
            this.btnBrowse.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(321, 35);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(69, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtBackupPath
            // 
            this.txtBackupPath.AccessibleName = "txtBackupPath";
            this.txtBackupPath.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBackupPath.Location = new System.Drawing.Point(11, 35);
            this.txtBackupPath.Name = "txtBackupPath";
            this.txtBackupPath.Size = new System.Drawing.Size(304, 22);
            this.txtBackupPath.TabIndex = 5;
            this.txtBackupPath.TextChanged += new System.EventHandler(this.txtBackupPath_TextChanged);
            // 
            // lblXHTML
            // 
            this.lblXHTML.AccessibleName = "lblXHTML";
            this.lblXHTML.AutoSize = true;
            this.lblXHTML.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXHTML.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblXHTML.Location = new System.Drawing.Point(13, 15);
            this.lblXHTML.Name = "lblXHTML";
            this.lblXHTML.Size = new System.Drawing.Size(127, 14);
            this.lblXHTML.TabIndex = 6;
            this.lblXHTML.Text = "Select Backup Path";
            this.lblXHTML.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BackUp
            // 
            this.AccessibleName = "BackUp";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 430);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtBackupPath);
            this.Controls.Add(this.lblXHTML);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Btn_Cancel);
            this.Controls.Add(this.Btn_BackUp);
            this.Controls.Add(this.ChkListBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BackUp";
            this.ShowIcon = false;
            this.Text = "Backup";
            this.Load += new System.EventHandler(this.BackUp_Load);
            this.DoubleClick += new System.EventHandler(this.Backup_DoubleClick);
            this.Activated += new System.EventHandler(this.BackUp_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckedListBox ChkListBox;
        private System.Windows.Forms.Button Btn_BackUp;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtBackupPath;
        private System.Windows.Forms.Label lblXHTML;
    }
}
