using System.Windows.Forms;

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
			this.components = new System.ComponentModel.Container();
			this.l10NSharpExtender1 = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.btnexprtfolder = new System.Windows.Forms.Button();
			this.btnexprtCancel = new System.Windows.Forms.Button();
			this.btnexprtEpub3 = new System.Windows.Forms.Button();
			this.btnexprtEpub2 = new System.Windows.Forms.Button();
			this.IcnInfo = new System.Windows.Forms.PictureBox();
			this.lblMessage = new System.Windows.Forms.WebBrowser();
			this.grpExportType = new System.Windows.Forms.GroupBox();
			this.grpButtons = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.IcnInfo)).BeginInit();
			this.grpExportType.SuspendLayout();
			this.grpButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// l10NSharpExtender1
			// 
			this.l10NSharpExtender1.LocalizationManagerId = "Pathway";
			this.l10NSharpExtender1.PrefixForNewItems = "EpubExportTypeDlg";
			// 
			// btnexprtfolder
			// 
			this.btnexprtfolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnexprtfolder.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnexprtfolder, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnexprtfolder, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnexprtfolder, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnexprtfolder, "EpubExportTypeDlg.btnexprtfolder");
			this.btnexprtfolder.Location = new System.Drawing.Point(357, 13);
			this.btnexprtfolder.Name = "btnexprtfolder";
			this.btnexprtfolder.Size = new System.Drawing.Size(82, 33);
			this.btnexprtfolder.TabIndex = 3;
			this.btnexprtfolder.Text = "&Folder";
			this.btnexprtfolder.UseVisualStyleBackColor = true;
			this.btnexprtfolder.Click += new System.EventHandler(this.btnexprtfolder_Click);
			// 
			// btnexprtCancel
			// 
			this.btnexprtCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnexprtCancel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnexprtCancel, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnexprtCancel, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnexprtCancel, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnexprtCancel, "EpubExportTypeDlg.btnexprtCancel");
			this.btnexprtCancel.Location = new System.Drawing.Point(457, 13);
			this.btnexprtCancel.Name = "btnexprtCancel";
			this.btnexprtCancel.Size = new System.Drawing.Size(82, 33);
			this.btnexprtCancel.TabIndex = 4;
			this.btnexprtCancel.Text = "&Close";
			this.btnexprtCancel.UseVisualStyleBackColor = true;
			this.btnexprtCancel.Click += new System.EventHandler(this.btnexprtCancel_Click);
			// 
			// btnexprtEpub3
			// 
			this.btnexprtEpub3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnexprtEpub3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnexprtEpub3, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnexprtEpub3, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnexprtEpub3, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnexprtEpub3, "EpubExportTypeDlg.btnexprtEpub3");
			this.btnexprtEpub3.Location = new System.Drawing.Point(257, 13);
			this.btnexprtEpub3.Name = "btnexprtEpub3";
			this.btnexprtEpub3.Size = new System.Drawing.Size(82, 33);
			this.btnexprtEpub3.TabIndex = 6;
			this.btnexprtEpub3.Text = "Epub&3";
			this.btnexprtEpub3.UseVisualStyleBackColor = true;
			this.btnexprtEpub3.Click += new System.EventHandler(this.btnexprtEpub3_Click);
			// 
			// btnexprtEpub2
			// 
			this.btnexprtEpub2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnexprtEpub2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.btnexprtEpub2, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.btnexprtEpub2, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.btnexprtEpub2, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.btnexprtEpub2, "EpubExportTypeDlg.btnexprtEpub2");
			this.btnexprtEpub2.Location = new System.Drawing.Point(157, 13);
			this.btnexprtEpub2.Name = "btnexprtEpub2";
			this.btnexprtEpub2.Size = new System.Drawing.Size(82, 33);
			this.btnexprtEpub2.TabIndex = 7;
			this.btnexprtEpub2.Text = "Epub&2";
			this.btnexprtEpub2.UseVisualStyleBackColor = true;
			this.btnexprtEpub2.Click += new System.EventHandler(this.btnexprtEpub2_Click);
			// 
			// IcnInfo
			// 
			this.l10NSharpExtender1.SetLocalizableToolTip(this.IcnInfo, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.IcnInfo, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.IcnInfo, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.IcnInfo, "EpubExportTypeDlg.IcnInfo");
			this.IcnInfo.Location = new System.Drawing.Point(12, 33);
			this.IcnInfo.Name = "IcnInfo";
			this.IcnInfo.Size = new System.Drawing.Size(38, 35);
			this.IcnInfo.TabIndex = 0;
			this.IcnInfo.TabStop = false;
			// 
			// lblMessage
			// 
			this.lblMessage.AccessibleRole = System.Windows.Forms.AccessibleRole.Grip;
			this.lblMessage.AllowNavigation = false;
			this.lblMessage.AllowWebBrowserDrop = false;
			this.lblMessage.Dock = System.Windows.Forms.DockStyle.Right;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.lblMessage, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.lblMessage, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.lblMessage, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.lblMessage, "EpubExportTypeDlg.EpubExportTypeDlg.lblMessage");
			this.lblMessage.Location = new System.Drawing.Point(75, 16);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.ScrollBarsEnabled = false;
			this.lblMessage.Size = new System.Drawing.Size(472, 256);
			this.lblMessage.TabIndex = 4;
			this.lblMessage.WebBrowserShortcutsEnabled = false;
			this.lblMessage.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.lblMessage_DocumentCompleted);
			// 
			// grpExportType
			// 
			this.grpExportType.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.grpExportType.BackColor = System.Drawing.Color.White;
			this.grpExportType.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.grpExportType.Controls.Add(this.IcnInfo);
			this.grpExportType.Controls.Add(this.lblMessage);
			this.grpExportType.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpExportType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.grpExportType.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.l10NSharpExtender1.SetLocalizableToolTip(this.grpExportType, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.grpExportType, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.grpExportType, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this.grpExportType, "EpubExportTypeDlg.grpExportType");
			this.grpExportType.Location = new System.Drawing.Point(0, 0);
			this.grpExportType.Name = "grpExportType";
			this.grpExportType.Size = new System.Drawing.Size(550, 275);
			this.grpExportType.TabIndex = 5;
			this.grpExportType.TabStop = false;
			// 
			// grpButtons
			// 
			this.grpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.grpButtons.Controls.Add(this.btnexprtEpub2);
			this.grpButtons.Controls.Add(this.btnexprtEpub3);
			this.grpButtons.Controls.Add(this.btnexprtfolder);
			this.grpButtons.Controls.Add(this.btnexprtCancel);
			this.grpButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.grpButtons.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.grpButtons, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.grpButtons, null);
			this.l10NSharpExtender1.SetLocalizingId(this.grpButtons, "EpubExportTypeDlg.grpButtons");
			this.grpButtons.Location = new System.Drawing.Point(0, 275);
			this.grpButtons.Name = "grpButtons";
			this.grpButtons.Size = new System.Drawing.Size(550, 54);
			this.grpButtons.TabIndex = 0;
			this.grpButtons.TabStop = false;
			// 
			// EpubExportTypeDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(550, 329);
			this.Controls.Add(this.grpExportType);
			this.Controls.Add(this.grpButtons);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.l10NSharpExtender1.SetLocalizableToolTip(this, null);
			this.l10NSharpExtender1.SetLocalizationComment(this, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.High);
			this.l10NSharpExtender1.SetLocalizingId(this, "EpubExportTypeDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EpubExportTypeDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Open Epub File Type";
			this.Load += new System.EventHandler(this.EpubExportTypeDlg_Load);
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.IcnInfo)).EndInit();
			this.grpExportType.ResumeLayout(false);
			this.grpButtons.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion
        private L10NSharp.UI.L10NSharpExtender l10NSharpExtender1 = null;
        private System.Windows.Forms.Button btnexprtfolder;
		private System.Windows.Forms.Button btnexprtCancel;
        private System.Windows.Forms.Button btnexprtEpub3;
		private System.Windows.Forms.Button btnexprtEpub2;
		private PictureBox IcnInfo;
		private WebBrowser lblMessage;
		private GroupBox grpExportType;
		private GroupBox grpButtons;
    }
}