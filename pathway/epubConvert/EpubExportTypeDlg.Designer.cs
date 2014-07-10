namespace epubConvert
{
    partial class EpubExportTypeDlg
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
            this.btnexprtfolder = new System.Windows.Forms.Button();
            this.btnexprtCancel = new System.Windows.Forms.Button();
            this.grpExportType = new System.Windows.Forms.GroupBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.IcnInfo = new System.Windows.Forms.PictureBox();
            this.btnexprtEpub3 = new System.Windows.Forms.Button();
            this.btnexprtEpub2 = new System.Windows.Forms.Button();
            this.grpExportType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IcnInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnexprtfolder
            // 
            this.btnexprtfolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnexprtfolder.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnexprtfolder.Location = new System.Drawing.Point(286, 389);
            this.btnexprtfolder.Name = "btnexprtfolder";
            this.btnexprtfolder.Size = new System.Drawing.Size(89, 33);
            this.btnexprtfolder.TabIndex = 3;
            this.btnexprtfolder.Text = "&Folder";
            this.btnexprtfolder.UseVisualStyleBackColor = true;
            this.btnexprtfolder.Click += new System.EventHandler(this.btnexprtfolder_Click);
            // 
            // btnexprtCancel
            // 
            this.btnexprtCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnexprtCancel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnexprtCancel.Location = new System.Drawing.Point(386, 389);
            this.btnexprtCancel.Name = "btnexprtCancel";
            this.btnexprtCancel.Size = new System.Drawing.Size(89, 33);
            this.btnexprtCancel.TabIndex = 4;
            this.btnexprtCancel.Text = "&Cancel";
            this.btnexprtCancel.UseVisualStyleBackColor = true;
            this.btnexprtCancel.Click += new System.EventHandler(this.btnexprtCancel_Click);
            // 
            // grpExportType
            // 
            this.grpExportType.BackColor = System.Drawing.Color.White;
            this.grpExportType.Controls.Add(this.lblMessage);
            this.grpExportType.Controls.Add(this.IcnInfo);
            this.grpExportType.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpExportType.Location = new System.Drawing.Point(5, 0);
            this.grpExportType.Name = "grpExportType";
            this.grpExportType.Size = new System.Drawing.Size(489, 371);
            this.grpExportType.TabIndex = 5;
            this.grpExportType.TabStop = false;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(97, 33);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(250, 18);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "Do you want select the Epub type ?";
            // 
            // IcnInfo
            // 
            this.IcnInfo.Location = new System.Drawing.Point(26, 33);
            this.IcnInfo.Name = "IcnInfo";
            this.IcnInfo.Size = new System.Drawing.Size(50, 44);
            this.IcnInfo.TabIndex = 0;
            this.IcnInfo.TabStop = false;
            // 
            // btnexprtEpub3
            // 
            this.btnexprtEpub3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnexprtEpub3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnexprtEpub3.Location = new System.Drawing.Point(186, 389);
            this.btnexprtEpub3.Name = "btnexprtEpub3";
            this.btnexprtEpub3.Size = new System.Drawing.Size(89, 33);
            this.btnexprtEpub3.TabIndex = 6;
            this.btnexprtEpub3.Text = "Epub&3";
            this.btnexprtEpub3.UseVisualStyleBackColor = true;
            this.btnexprtEpub3.Click += new System.EventHandler(this.btnexprtEpub3_Click);
            // 
            // btnexprtEpub2
            // 
            this.btnexprtEpub2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnexprtEpub2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnexprtEpub2.Location = new System.Drawing.Point(86, 389);
            this.btnexprtEpub2.Name = "btnexprtEpub2";
            this.btnexprtEpub2.Size = new System.Drawing.Size(89, 33);
            this.btnexprtEpub2.TabIndex = 7;
            this.btnexprtEpub2.Text = "Epub&2";
            this.btnexprtEpub2.UseVisualStyleBackColor = true;
            this.btnexprtEpub2.Click += new System.EventHandler(this.btnexprtEpub2_Click);
            // 
            // EpubExportTypeDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 435);
            this.Controls.Add(this.btnexprtEpub2);
            this.Controls.Add(this.btnexprtEpub3);
            this.Controls.Add(this.grpExportType);
            this.Controls.Add(this.btnexprtCancel);
            this.Controls.Add(this.btnexprtfolder);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EpubExportTypeDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open Epub File Type";
            this.Load += new System.EventHandler(this.EpubExportTypeDlg_Load);
            this.grpExportType.ResumeLayout(false);
            this.grpExportType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IcnInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnexprtfolder;
        private System.Windows.Forms.Button btnexprtCancel;
        private System.Windows.Forms.GroupBox grpExportType;
        private System.Windows.Forms.Button btnexprtEpub3;
        private System.Windows.Forms.Button btnexprtEpub2;
        private System.Windows.Forms.PictureBox IcnInfo;
        private System.Windows.Forms.Label lblMessage;
    }
}