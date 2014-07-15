namespace epubConvert
{
    partial class EpubValidateTypeDlg
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
            this.btnBoth = new System.Windows.Forms.Button();
            this.btnNeither = new System.Windows.Forms.Button();
            this.grpExportType = new System.Windows.Forms.GroupBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.IcnInfo = new System.Windows.Forms.PictureBox();
            this.btnValidateEpub3 = new System.Windows.Forms.Button();
            this.btnValidateEpub2 = new System.Windows.Forms.Button();
            this.grpExportType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IcnInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBoth
            // 
            this.btnBoth.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBoth.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBoth.Location = new System.Drawing.Point(286, 389);
            this.btnBoth.Name = "btnBoth";
            this.btnBoth.Size = new System.Drawing.Size(89, 33);
            this.btnBoth.TabIndex = 3;
            this.btnBoth.Text = "&Both";
            this.btnBoth.UseVisualStyleBackColor = true;
            this.btnBoth.Click += new System.EventHandler(this.btnBoth_Click);
            // 
            // btnNeither
            // 
            this.btnNeither.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnNeither.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNeither.Location = new System.Drawing.Point(386, 389);
            this.btnNeither.Name = "btnNeither";
            this.btnNeither.Size = new System.Drawing.Size(89, 33);
            this.btnNeither.TabIndex = 4;
            this.btnNeither.Text = "&Neither";
            this.btnNeither.UseVisualStyleBackColor = true;
            this.btnNeither.Click += new System.EventHandler(this.btnNeither_Click);
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
            this.lblMessage.Location = new System.Drawing.Point(69, 33);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(250, 18);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "Do you want select the Epub validate type ?";

            // 
            // IcnInfo
            // 
            this.IcnInfo.Location = new System.Drawing.Point(26, 33);
            this.IcnInfo.Name = "IcnInfo";
            this.IcnInfo.Size = new System.Drawing.Size(38, 35);
            this.IcnInfo.TabIndex = 0;
            this.IcnInfo.TabStop = false;
            // 
            // btnValidateEpub3
            // 
            this.btnValidateEpub3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnValidateEpub3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidateEpub3.Location = new System.Drawing.Point(186, 389);
            this.btnValidateEpub3.Name = "btnValidateEpub3";
            this.btnValidateEpub3.Size = new System.Drawing.Size(89, 33);
            this.btnValidateEpub3.TabIndex = 6;
            this.btnValidateEpub3.Text = "Epub&3";
            this.btnValidateEpub3.UseVisualStyleBackColor = true;
            this.btnValidateEpub3.Click += new System.EventHandler(this.btnValidateEpub3_Click);
            // 
            // btnValidateEpub2
            // 
            this.btnValidateEpub2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnValidateEpub2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidateEpub2.Location = new System.Drawing.Point(86, 389);
            this.btnValidateEpub2.Name = "btnValidateEpub2";
            this.btnValidateEpub2.Size = new System.Drawing.Size(89, 33);
            this.btnValidateEpub2.TabIndex = 7;
            this.btnValidateEpub2.Text = "Epub&2";
            this.btnValidateEpub2.UseVisualStyleBackColor = true;
            this.btnValidateEpub2.Click += new System.EventHandler(this.btnValidateEpub2_Click);
            // 
            // EpubExportTypeDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 435);
            this.Controls.Add(this.btnValidateEpub2);
            this.Controls.Add(this.btnValidateEpub3);
            this.Controls.Add(this.grpExportType);
            this.Controls.Add(this.btnNeither);
            this.Controls.Add(this.btnBoth);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EpubValidateTypeDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open Validate Type";
            this.Load += new System.EventHandler(this.EpubValidateTypeDlg_Load);
            this.grpExportType.ResumeLayout(false);
            this.grpExportType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IcnInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBoth;
        private System.Windows.Forms.Button btnNeither;
        private System.Windows.Forms.GroupBox grpExportType;
        private System.Windows.Forms.Button btnValidateEpub3;
        private System.Windows.Forms.Button btnValidateEpub2;
        private System.Windows.Forms.PictureBox IcnInfo;
        private System.Windows.Forms.Label lblMessage;
    }
}