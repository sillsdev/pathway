namespace SIL.PublishingSolution
{
    partial class SelectOrganizationDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOther = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblOrganization = new System.Windows.Forms.Label();
            this.ddlOrganization = new System.Windows.Forms.ComboBox();
            this.lblOther = new System.Windows.Forms.Label();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(307, 127);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOther
            // 
            this.btnOther.Location = new System.Drawing.Point(307, 79);
            this.btnOther.Name = "btnOther";
            this.btnOther.Size = new System.Drawing.Size(75, 23);
            this.btnOther.TabIndex = 4;
            this.btnOther.Text = "O&ther...";
            this.btnOther.UseVisualStyleBackColor = true;
            this.btnOther.Click += new System.EventHandler(this.btnOther_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(226, 127);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblOrganization
            // 
            this.lblOrganization.AutoSize = true;
            this.lblOrganization.Location = new System.Drawing.Point(12, 50);
            this.lblOrganization.Name = "lblOrganization";
            this.lblOrganization.Size = new System.Drawing.Size(84, 13);
            this.lblOrganization.TabIndex = 1;
            this.lblOrganization.Text = "My o&rganization:";
            // 
            // ddlOrganization
            // 
            this.ddlOrganization.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlOrganization.FormattingEnabled = true;
            this.ddlOrganization.Location = new System.Drawing.Point(102, 47);
            this.ddlOrganization.Name = "ddlOrganization";
            this.ddlOrganization.Size = new System.Drawing.Size(280, 21);
            this.ddlOrganization.TabIndex = 2;
            // 
            // lblOther
            // 
            this.lblOther.Location = new System.Drawing.Point(12, 79);
            this.lblOther.Name = "lblOther";
            this.lblOther.Size = new System.Drawing.Size(253, 30);
            this.lblOther.TabIndex = 3;
            this.lblOther.Text = "If your organization is not on the above list, click the Other... button:";
            // 
            // lblInstructions
            // 
            this.lblInstructions.Location = new System.Drawing.Point(12, 12);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(370, 31);
            this.lblInstructions.TabIndex = 11;
            this.lblInstructions.Text = "Select the organization you are representing, Pathway will automatically fill in " +
                "copyright statements, etc. based on your selection.";
            // 
            // SelectOrganizationDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(394, 162);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOther);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblOrganization);
            this.Controls.Add(this.ddlOrganization);
            this.Controls.Add(this.lblOther);
            this.Controls.Add(this.lblInstructions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectOrganizationDialog";
            this.Text = "Select Your Organization";
            this.Load += new System.EventHandler(this.SelectOrganizationDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOther;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblOrganization;
        private System.Windows.Forms.ComboBox ddlOrganization;
        private System.Windows.Forms.Label lblOther;
        private System.Windows.Forms.Label lblInstructions;
    }
}