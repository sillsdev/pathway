namespace SIL.PublishingSolution
{
    partial class GlobalSettings
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
            this.BtOk = new System.Windows.Forms.Button();
            this.BtCancel = new System.Windows.Forms.Button();
            this.TlSettings = new System.Windows.Forms.TableLayoutPanel();
            this.BtReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtOk
            // 
            this.BtOk.AccessibleName = "BtOk";
            this.BtOk.Location = new System.Drawing.Point(402, 321);
            this.BtOk.Name = "BtOk";
            this.BtOk.Size = new System.Drawing.Size(75, 23);
            this.BtOk.TabIndex = 1;
            this.BtOk.Text = "&OK";
            this.BtOk.UseVisualStyleBackColor = true;
            this.BtOk.Click += new System.EventHandler(this.BtOk_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.AccessibleName = "BtCancel";
            this.BtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtCancel.Location = new System.Drawing.Point(321, 321);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(75, 23);
            this.BtCancel.TabIndex = 2;
            this.BtCancel.Text = "&Cancel";
            this.BtCancel.UseVisualStyleBackColor = true;
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // TlSettings
            // 
            this.TlSettings.AccessibleName = "TlSettings";
            this.TlSettings.AutoScroll = true;
            this.TlSettings.ColumnCount = 2;
            this.TlSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlSettings.Location = new System.Drawing.Point(12, 12);
            this.TlSettings.Name = "TlSettings";
            this.TlSettings.RowCount = 2;
            this.TlSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TlSettings.Size = new System.Drawing.Size(465, 300);
            this.TlSettings.TabIndex = 3;
            // 
            // BtReset
            // 
            this.BtReset.AccessibleName = "BtReset";
            this.BtReset.Location = new System.Drawing.Point(12, 321);
            this.BtReset.Name = "BtReset";
            this.BtReset.Size = new System.Drawing.Size(75, 23);
            this.BtReset.TabIndex = 0;
            this.BtReset.Text = "&Reset";
            this.BtReset.UseVisualStyleBackColor = true;
            this.BtReset.Visible = false;
            // 
            // GlobalSettings
            // 
            this.AcceptButton = this.BtOk;
            this.AccessibleName = "GlobalSettings";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtCancel;
            this.ClientSize = new System.Drawing.Size(490, 357);
            this.Controls.Add(this.BtReset);
            this.Controls.Add(this.TlSettings);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GlobalSettings";
            this.Text = "Global Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.DoubleClick += new System.EventHandler(this.Settings_DoubleClick);
            this.Activated += new System.EventHandler(this.GlobalSettings_Activated);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtOk;
        private System.Windows.Forms.Button BtCancel;
        private System.Windows.Forms.TableLayoutPanel TlSettings;
        private System.Windows.Forms.Button BtReset;
    }
}
