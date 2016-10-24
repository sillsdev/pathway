namespace SIL.PublishingSolution
{
    partial class OrganizationNameDialog
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

            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOrganization = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // l10NSharpExtender1
            // 
            this.l10NSharpExtender1.LocalizationManagerId = "Pathway";
            this.l10NSharpExtender1.PrefixForNewItems = "OrganizationNameDialog";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(106, 67);
            this.btnOK.Name = "btnOK";
            this.l10NSharpExtender1.SetLocalizableToolTip(this.btnOK, null);
            this.l10NSharpExtender1.SetLocalizationComment(this.btnOK, null);
            this.l10NSharpExtender1.SetLocalizationPriority(this.btnOK, L10NSharp.LocalizationPriority.High);
            this.l10NSharpExtender1.SetLocalizingId(this.btnOK, "OrganizationNameDialog.btnOK");
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(187, 67);
            this.btnCancel.Name = "btnCancel";
            this.l10NSharpExtender1.SetLocalizableToolTip(this.btnCancel, null);
            this.l10NSharpExtender1.SetLocalizationComment(this.btnCancel, null);
            this.l10NSharpExtender1.SetLocalizationPriority(this.btnCancel, L10NSharp.LocalizationPriority.High);
            this.l10NSharpExtender1.SetLocalizingId(this.btnCancel, "OrganizationNameDialog.btnCancel");
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.l10NSharpExtender1.SetLocalizableToolTip(this.label1, null);
            this.l10NSharpExtender1.SetLocalizationComment(this.label1, null);
            this.l10NSharpExtender1.SetLocalizationPriority(this.label1, L10NSharp.LocalizationPriority.High);
            this.l10NSharpExtender1.SetLocalizingId(this.label1, "OrganizationNameDialog.label1");
            this.label1.Size = new System.Drawing.Size(187, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Type in the &name of your organization:";
            // 
            // txtOrganization
            // 
            this.txtOrganization.Location = new System.Drawing.Point(12, 35);
            this.txtOrganization.Name = "txtOrganization";
            this.txtOrganization.Size = new System.Drawing.Size(250, 20);
            this.txtOrganization.TabIndex = 2;
            // 
            // OrganizationNameDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(274, 102);
            this.Controls.Add(this.txtOrganization);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrganizationNameDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Organization Name";
            this.Load += new System.EventHandler(this.OrganizationNameDialog_Load);
            this.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).EndInit();
            this.PerformLayout();

        }

        #endregion

        private L10NSharp.UI.L10NSharpExtender l10NSharpExtender1 = null;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOrganization;
    }
}