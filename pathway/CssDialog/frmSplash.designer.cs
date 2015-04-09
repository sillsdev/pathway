namespace SIL.PublishingSolution
{
    partial class frmSplash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSplash));
            this.l10NSharpExtender1 = new L10NSharp.UI.L10NSharpExtender(this.components);
            this.tSplash = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCompany = new System.Windows.Forms.Label();
            this.lblGPL = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblVersionwithDate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSilPathway = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();

            // l10NSharpExtender1
            // 
            this.l10NSharpExtender1.LocalizationManagerId = "Pathway";
            this.l10NSharpExtender1.PrefixForNewItems = "frmSplash";

            // 
            // tSplash
            // 
            this.tSplash.Enabled = true;
            this.tSplash.Interval = 500;
            this.tSplash.Tick += new System.EventHandler(this.tSplash_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lblCompany);
            this.panel1.Controls.Add(this.lblGPL);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblVersionwithDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblSilPathway);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 296);
            this.panel1.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(318, 158);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 11);
            this.label5.TabIndex = 14;
            this.label5.Text = "®";
            // 
            // lblCompany
            // 
            this.lblCompany.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCompany.AutoSize = true;
            this.lblCompany.Location = new System.Drawing.Point(159, 215);
            this.lblCompany.Name = "lblCompany";
            this.l10NSharpExtender1.SetLocalizableToolTip(this.lblCompany, null);
            this.l10NSharpExtender1.SetLocalizationComment(this.lblCompany, null);
            this.l10NSharpExtender1.SetLocalizationPriority(this.lblCompany, L10NSharp.LocalizationPriority.High);
            this.l10NSharpExtender1.SetLocalizingId(this.lblCompany, "AboutPw.lblCompany");
            this.lblCompany.Size = new System.Drawing.Size(344, 13);
            this.lblCompany.TabIndex = 12;
            this.lblCompany.Text = "SIL International in collaboration with EC Group Datasoft Private Limited";
            this.lblCompany.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblGPL
            // 
            this.lblGPL.AutoSize = true;
            this.lblGPL.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l10NSharpExtender1.SetLocalizableToolTip(this.lblGPL, null);
            this.l10NSharpExtender1.SetLocalizationComment(this.lblGPL, null);
            this.l10NSharpExtender1.SetLocalizationPriority(this.lblGPL, L10NSharp.LocalizationPriority.High);
            this.l10NSharpExtender1.SetLocalizingId(this.lblGPL, "AboutPw.lblGPL");
            this.lblGPL.Location = new System.Drawing.Point(160, 186);
            this.lblGPL.Name = "lblGPL";
            this.lblGPL.Size = new System.Drawing.Size(135, 14);
            this.lblGPL.TabIndex = 11;
            this.lblGPL.Text = "Code license: GNU GPL v3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l10NSharpExtender1.SetLocalizableToolTip(this.label3, null);
            this.l10NSharpExtender1.SetLocalizationComment(this.label3, null);
            this.l10NSharpExtender1.SetLocalizationPriority(this.label3, L10NSharp.LocalizationPriority.High);
            this.l10NSharpExtender1.SetLocalizingId(this.label3, "AboutPw.label3");
            this.label3.Location = new System.Drawing.Point(28, 259);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 14);
            this.label3.TabIndex = 6;
            this.label3.Text = "Loading ....";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l10NSharpExtender1.SetLocalizableToolTip(this.label2, null);
            this.l10NSharpExtender1.SetLocalizationComment(this.label2, null);
            this.l10NSharpExtender1.SetLocalizationPriority(this.label2, L10NSharp.LocalizationPriority.High);
            this.l10NSharpExtender1.SetLocalizingId(this.label2, "AboutPw.label2");
            this.label2.Location = new System.Drawing.Point(160, 163);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(238, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "(C) 2008 - 2014 SIL International";
            // 
            // lblVersionwithDate
            // 
            this.lblVersionwithDate.AutoSize = true;
            this.lblVersionwithDate.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l10NSharpExtender1.SetLocalizableToolTip(this.lblVersionwithDate, null);
            this.l10NSharpExtender1.SetLocalizationComment(this.lblVersionwithDate, null);
            this.l10NSharpExtender1.SetLocalizationPriority(this.lblVersionwithDate, L10NSharp.LocalizationPriority.High);
            this.l10NSharpExtender1.SetLocalizingId(this.lblVersionwithDate, "AboutPw.lblVersionwithDate");
            this.lblVersionwithDate.Location = new System.Drawing.Point(159, 98);
            this.lblVersionwithDate.Name = "lblVersionwithDate";
            this.lblVersionwithDate.Size = new System.Drawing.Size(74, 14);
            this.lblVersionwithDate.TabIndex = 4;
            this.lblVersionwithDate.Text = "Version: 1.4.0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l10NSharpExtender1.SetLocalizableToolTip(this.label1, null);
            this.l10NSharpExtender1.SetLocalizationComment(this.label1, null);
            this.l10NSharpExtender1.SetLocalizationPriority(this.label1, L10NSharp.LocalizationPriority.High);
            this.l10NSharpExtender1.SetLocalizingId(this.label1, "AboutPw.label1");
            this.label1.Location = new System.Drawing.Point(155, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Configuration Tool";
            // 
            // lblSilPathway
            // 
            this.lblSilPathway.AutoSize = true;
            this.lblSilPathway.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l10NSharpExtender1.SetLocalizableToolTip(this.lblSilPathway, null);
            this.l10NSharpExtender1.SetLocalizationComment(this.lblSilPathway, null);
            this.l10NSharpExtender1.SetLocalizationPriority(this.lblSilPathway, L10NSharp.LocalizationPriority.High);
            this.l10NSharpExtender1.SetLocalizingId(this.lblSilPathway, "AboutPw.lblSilPathway");
            this.lblSilPathway.Location = new System.Drawing.Point(155, 33);
            this.lblSilPathway.Name = "lblSilPathway";
            this.lblSilPathway.Size = new System.Drawing.Size(94, 24);
            this.lblSilPathway.TabIndex = 1;
            this.lblSilPathway.Text = "Pathway";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::SIL.PublishingSolution.Properties.Resources._2014_sil_logo;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(19, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(123, 128);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // frmSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(513, 296);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSplash";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SIL International";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmSplash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private L10NSharp.UI.L10NSharpExtender l10NSharpExtender1;
        private System.Windows.Forms.Timer tSplash;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblVersionwithDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSilPathway;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblGPL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCompany;
    }
}