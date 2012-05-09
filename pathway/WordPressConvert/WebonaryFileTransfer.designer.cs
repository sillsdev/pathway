namespace SIL.PublishingSolution
{
    partial class WebonaryFileTransfer
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
            this.txtSourceFileLocation = new System.Windows.Forms.TextBox();
            this.lblSourceFileLocation = new System.Windows.Forms.Label();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPickFile = new System.Windows.Forms.Button();
            this.directoryDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTargetFileLocation = new System.Windows.Forms.TextBox();
            this.lblTargetFileLocation = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSqlPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSqlUsername = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSqlDBName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSqlServerName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtWebFtpFldrNme = new System.Windows.Forms.TextBox();
            this.txtWebEmailID = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtWebAdminSiteNme = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtWebAdminPwd = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtWebAdminUsrNme = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtWebUrl = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSourceFileLocation
            // 
            this.txtSourceFileLocation.Location = new System.Drawing.Point(12, 234);
            this.txtSourceFileLocation.MaxLength = 1000;
            this.txtSourceFileLocation.Name = "txtSourceFileLocation";
            this.txtSourceFileLocation.Size = new System.Drawing.Size(520, 20);
            this.txtSourceFileLocation.TabIndex = 3;
            this.txtSourceFileLocation.Text = "E:\\";
            // 
            // lblSourceFileLocation
            // 
            this.lblSourceFileLocation.AutoSize = true;
            this.lblSourceFileLocation.Location = new System.Drawing.Point(9, 209);
            this.lblSourceFileLocation.Name = "lblSourceFileLocation";
            this.lblSourceFileLocation.Size = new System.Drawing.Size(104, 13);
            this.lblSourceFileLocation.TabIndex = 2;
            this.lblSourceFileLocation.Text = "Source File Location";
            // 
            // btnTransfer
            // 
            this.btnTransfer.Location = new System.Drawing.Point(363, 183);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(97, 30);
            this.btnTransfer.TabIndex = 4;
            this.btnTransfer.Text = "Proceed";
            this.btnTransfer.UseVisualStyleBackColor = true;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(466, 183);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(97, 30);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPickFile
            // 
            this.btnPickFile.Location = new System.Drawing.Point(538, 232);
            this.btnPickFile.Name = "btnPickFile";
            this.btnPickFile.Size = new System.Drawing.Size(25, 23);
            this.btnPickFile.TabIndex = 10;
            this.btnPickFile.Text = "...";
            this.btnPickFile.UseVisualStyleBackColor = true;
            this.btnPickFile.Click += new System.EventHandler(this.btnPickFile_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 30);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(551, 23);
            this.progressBar.TabIndex = 11;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(186, 93);
            this.txtPassword.MaxLength = 50;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(158, 20);
            this.txtPassword.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "FTP Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(12, 93);
            this.txtUsername.MaxLength = 1000;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(158, 20);
            this.txtUsername.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "FTP Username";
            // 
            // txtTargetFileLocation
            // 
            this.txtTargetFileLocation.Location = new System.Drawing.Point(12, 43);
            this.txtTargetFileLocation.MaxLength = 500;
            this.txtTargetFileLocation.Name = "txtTargetFileLocation";
            this.txtTargetFileLocation.Size = new System.Drawing.Size(521, 20);
            this.txtTargetFileLocation.TabIndex = 24;
            // 
            // lblTargetFileLocation
            // 
            this.lblTargetFileLocation.AutoSize = true;
            this.lblTargetFileLocation.Location = new System.Drawing.Point(9, 27);
            this.lblTargetFileLocation.Name = "lblTargetFileLocation";
            this.lblTargetFileLocation.Size = new System.Drawing.Size(251, 13);
            this.lblTargetFileLocation.TabIndex = 23;
            this.lblTargetFileLocation.Text = "FTP Address / Server IP (ex : ftp://ip address/path)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSqlPassword);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtSqlUsername);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtSqlDBName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSqlServerName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 388);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(551, 94);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MySql Database Details";
            // 
            // txtSqlPassword
            // 
            this.txtSqlPassword.Location = new System.Drawing.Point(433, 56);
            this.txtSqlPassword.MaxLength = 50;
            this.txtSqlPassword.Name = "txtSqlPassword";
            this.txtSqlPassword.PasswordChar = '*';
            this.txtSqlPassword.Size = new System.Drawing.Size(100, 20);
            this.txtSqlPassword.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(430, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Password";
            // 
            // txtSqlUsername
            // 
            this.txtSqlUsername.Location = new System.Drawing.Point(313, 56);
            this.txtSqlUsername.MaxLength = 25;
            this.txtSqlUsername.Name = "txtSqlUsername";
            this.txtSqlUsername.Size = new System.Drawing.Size(111, 20);
            this.txtSqlUsername.TabIndex = 21;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(310, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "User Name";
            // 
            // txtSqlDBName
            // 
            this.txtSqlDBName.Location = new System.Drawing.Point(186, 56);
            this.txtSqlDBName.MaxLength = 25;
            this.txtSqlDBName.Name = "txtSqlDBName";
            this.txtSqlDBName.Size = new System.Drawing.Size(120, 20);
            this.txtSqlDBName.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(183, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Database Name";
            // 
            // txtSqlServerName
            // 
            this.txtSqlServerName.Location = new System.Drawing.Point(12, 56);
            this.txtSqlServerName.MaxLength = 100;
            this.txtSqlServerName.Name = "txtSqlServerName";
            this.txtSqlServerName.Size = new System.Drawing.Size(158, 20);
            this.txtSqlServerName.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(164, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "My Sql Server name / IP Address";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPassword);
            this.groupBox2.Controls.Add(this.lblTargetFileLocation);
            this.groupBox2.Controls.Add(this.txtTargetFileLocation);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtUsername);
            this.groupBox2.Location = new System.Drawing.Point(12, 260);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(551, 122);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "FTP Details";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.txtWebFtpFldrNme);
            this.groupBox3.Controls.Add(this.txtWebEmailID);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.txtWebAdminSiteNme);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.txtWebAdminPwd);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.txtWebAdminUsrNme);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.txtWebUrl);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Location = new System.Drawing.Point(600, 232);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(312, 250);
            this.groupBox3.TabIndex = 33;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Website Details";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(17, 170);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(85, 13);
            this.label19.TabIndex = 26;
            this.label19.Text = "Ftp Folder Name";
            // 
            // txtWebFtpFldrNme
            // 
            this.txtWebFtpFldrNme.Location = new System.Drawing.Point(104, 167);
            this.txtWebFtpFldrNme.MaxLength = 50;
            this.txtWebFtpFldrNme.Name = "txtWebFtpFldrNme";
            this.txtWebFtpFldrNme.Size = new System.Drawing.Size(170, 20);
            this.txtWebFtpFldrNme.TabIndex = 25;
            // 
            // txtWebEmailID
            // 
            this.txtWebEmailID.Location = new System.Drawing.Point(105, 140);
            this.txtWebEmailID.MaxLength = 50;
            this.txtWebEmailID.Name = "txtWebEmailID";
            this.txtWebEmailID.Size = new System.Drawing.Size(169, 20);
            this.txtWebEmailID.TabIndex = 23;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(15, 143);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(46, 13);
            this.label18.TabIndex = 24;
            this.label18.Text = "Email ID";
            // 
            // txtWebAdminSiteNme
            // 
            this.txtWebAdminSiteNme.Location = new System.Drawing.Point(103, 114);
            this.txtWebAdminSiteNme.MaxLength = 50;
            this.txtWebAdminSiteNme.Name = "txtWebAdminSiteNme";
            this.txtWebAdminSiteNme.Size = new System.Drawing.Size(171, 20);
            this.txtWebAdminSiteNme.TabIndex = 3;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 117);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "Site Name";
            // 
            // txtWebAdminPwd
            // 
            this.txtWebAdminPwd.Location = new System.Drawing.Point(103, 88);
            this.txtWebAdminPwd.MaxLength = 25;
            this.txtWebAdminPwd.Name = "txtWebAdminPwd";
            this.txtWebAdminPwd.PasswordChar = '*';
            this.txtWebAdminPwd.Size = new System.Drawing.Size(171, 20);
            this.txtWebAdminPwd.TabIndex = 2;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(16, 91);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(85, 13);
            this.label15.TabIndex = 20;
            this.label15.Text = "Admin Password";
            // 
            // txtWebAdminUsrNme
            // 
            this.txtWebAdminUsrNme.Location = new System.Drawing.Point(103, 62);
            this.txtWebAdminUsrNme.MaxLength = 25;
            this.txtWebAdminUsrNme.Name = "txtWebAdminUsrNme";
            this.txtWebAdminUsrNme.Size = new System.Drawing.Size(171, 20);
            this.txtWebAdminUsrNme.TabIndex = 1;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(16, 64);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(87, 13);
            this.label16.TabIndex = 18;
            this.label16.Text = "Admin Username";
            // 
            // txtWebUrl
            // 
            this.txtWebUrl.Location = new System.Drawing.Point(103, 38);
            this.txtWebUrl.MaxLength = 100;
            this.txtWebUrl.Name = "txtWebUrl";
            this.txtWebUrl.Size = new System.Drawing.Size(172, 20);
            this.txtWebUrl.TabIndex = 0;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(17, 41);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(60, 13);
            this.label17.TabIndex = 16;
            this.label17.Text = "Website url";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Please wait...";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 88);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 35;
            // 
            // WebonaryFileTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(588, 120);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnPickFile);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnTransfer);
            this.Controls.Add(this.txtSourceFileLocation);
            this.Controls.Add(this.lblSourceFileLocation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WebonaryFileTransfer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WordPress Export";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.WebonaryFileTransfer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSourceFileLocation;
        private System.Windows.Forms.Label lblSourceFileLocation;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPickFile;
        private System.Windows.Forms.FolderBrowserDialog directoryDialog;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTargetFileLocation;
        private System.Windows.Forms.Label lblTargetFileLocation;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSqlPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSqlUsername;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSqlDBName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSqlServerName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtWebFtpFldrNme;
        private System.Windows.Forms.TextBox txtWebEmailID;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtWebAdminSiteNme;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtWebAdminPwd;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtWebAdminUsrNme;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtWebUrl;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblStatus;
    }
}