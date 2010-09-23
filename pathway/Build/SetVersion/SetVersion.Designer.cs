namespace Builder
{
    partial class SetVersion
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
            this.Version = new System.Windows.Forms.TextBox();
            this.LbVersion = new System.Windows.Forms.Label();
            this.Notes = new System.Windows.Forms.Button();
            this.LbHistory = new System.Windows.Forms.Label();
            this.LvHistory = new System.Windows.Forms.ListView();
            this.clRevision = new System.Windows.Forms.ColumnHeader();
            this.chAuthor = new System.Windows.Forms.ColumnHeader();
            this.chDescription = new System.Windows.Forms.ColumnHeader();
            this.BtAccept = new System.Windows.Forms.Button();
            this.OldVersion = new System.Windows.Forms.Button();
            this.Dlls = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Version
            // 
            this.Version.Location = new System.Drawing.Point(12, 38);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(100, 20);
            this.Version.TabIndex = 0;
            // 
            // LbVersion
            // 
            this.LbVersion.AutoSize = true;
            this.LbVersion.Location = new System.Drawing.Point(12, 15);
            this.LbVersion.Name = "LbVersion";
            this.LbVersion.Size = new System.Drawing.Size(42, 13);
            this.LbVersion.TabIndex = 1;
            this.LbVersion.Text = "Version";
            // 
            // Notes
            // 
            this.Notes.Location = new System.Drawing.Point(15, 64);
            this.Notes.Name = "Notes";
            this.Notes.Size = new System.Drawing.Size(88, 23);
            this.Notes.TabIndex = 2;
            this.Notes.Text = "&Release Notes";
            this.Notes.UseVisualStyleBackColor = true;
            this.Notes.Click += new System.EventHandler(this.Notes_Click);
            // 
            // LbHistory
            // 
            this.LbHistory.AutoSize = true;
            this.LbHistory.Location = new System.Drawing.Point(136, 15);
            this.LbHistory.Name = "LbHistory";
            this.LbHistory.Size = new System.Drawing.Size(42, 13);
            this.LbHistory.TabIndex = 3;
            this.LbHistory.Text = "History:";
            // 
            // LvHistory
            // 
            this.LvHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clRevision,
            this.chAuthor,
            this.chDescription});
            this.LvHistory.Location = new System.Drawing.Point(139, 38);
            this.LvHistory.Name = "LvHistory";
            this.LvHistory.Size = new System.Drawing.Size(544, 149);
            this.LvHistory.TabIndex = 4;
            this.LvHistory.UseCompatibleStateImageBehavior = false;
            this.LvHistory.View = System.Windows.Forms.View.Details;
            // 
            // clRevision
            // 
            this.clRevision.Text = "Revision";
            // 
            // chAuthor
            // 
            this.chAuthor.Text = "Author";
            this.chAuthor.Width = 120;
            // 
            // chDescription
            // 
            this.chDescription.Text = "Description";
            this.chDescription.Width = 360;
            // 
            // BtAccept
            // 
            this.BtAccept.Location = new System.Drawing.Point(28, 93);
            this.BtAccept.Name = "BtAccept";
            this.BtAccept.Size = new System.Drawing.Size(75, 23);
            this.BtAccept.TabIndex = 5;
            this.BtAccept.Text = "&Accept";
            this.BtAccept.UseVisualStyleBackColor = true;
            this.BtAccept.Click += new System.EventHandler(this.BtAccept_Click);
            // 
            // OldVersion
            // 
            this.OldVersion.Location = new System.Drawing.Point(28, 122);
            this.OldVersion.Name = "OldVersion";
            this.OldVersion.Size = new System.Drawing.Size(75, 23);
            this.OldVersion.TabIndex = 7;
            this.OldVersion.Text = "Old Version";
            this.OldVersion.UseVisualStyleBackColor = true;
            this.OldVersion.Click += new System.EventHandler(this.OldVersion_Click);
            // 
            // Dlls
            // 
            this.Dlls.FormattingEnabled = true;
            this.Dlls.Location = new System.Drawing.Point(12, 166);
            this.Dlls.Name = "Dlls";
            this.Dlls.Size = new System.Drawing.Size(121, 21);
            this.Dlls.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "FieldWorks Dlls";
            // 
            // SetVersion
            // 
            this.AccessibleName = "SetVersion";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 206);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Dlls);
            this.Controls.Add(this.OldVersion);
            this.Controls.Add(this.BtAccept);
            this.Controls.Add(this.LvHistory);
            this.Controls.Add(this.LbHistory);
            this.Controls.Add(this.Notes);
            this.Controls.Add(this.LbVersion);
            this.Controls.Add(this.Version);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetVersion";
            this.Text = "SetVersion";
            this.Load += new System.EventHandler(this.Builder_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Version;
        private System.Windows.Forms.Label LbVersion;
        private System.Windows.Forms.Button Notes;
        private System.Windows.Forms.Label LbHistory;
        private System.Windows.Forms.ListView LvHistory;
        private System.Windows.Forms.ColumnHeader clRevision;
        private System.Windows.Forms.ColumnHeader chAuthor;
        private System.Windows.Forms.ColumnHeader chDescription;
        private System.Windows.Forms.Button BtAccept;
        private System.Windows.Forms.Button OldVersion;
        private System.Windows.Forms.ComboBox Dlls;
        private System.Windows.Forms.Label label1;
    }
}

