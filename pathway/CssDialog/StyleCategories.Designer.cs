namespace SIL.PublishingSolution
{
    partial class StyleCategories
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
            this.DgStyleCategories = new System.Windows.Forms.DataGridView();
            this.BtSave = new System.Windows.Forms.Button();
            this.BtCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DgStyleCategories)).BeginInit();
            this.SuspendLayout();
            // 
            // DgStyleCategories
            // 
            this.DgStyleCategories.AccessibleName = "DgStyleCategories";
            this.DgStyleCategories.AllowUserToAddRows = false;
            this.DgStyleCategories.AllowUserToDeleteRows = false;
            this.DgStyleCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgStyleCategories.Location = new System.Drawing.Point(12, 12);
            this.DgStyleCategories.Name = "DgStyleCategories";
            this.DgStyleCategories.Size = new System.Drawing.Size(750, 457);
            this.DgStyleCategories.TabIndex = 0;
            this.DgStyleCategories.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgStyleCategories_CellClick);
            // 
            // BtSave
            // 
            this.BtSave.AccessibleName = "BtSave";
            this.BtSave.Location = new System.Drawing.Point(687, 478);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(75, 23);
            this.BtSave.TabIndex = 7;
            this.BtSave.Text = "&Save";
            this.BtSave.UseVisualStyleBackColor = true;
            this.BtSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.AccessibleName = "BtCancel";
            this.BtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtCancel.Location = new System.Drawing.Point(606, 478);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(75, 23);
            this.BtCancel.TabIndex = 8;
            this.BtCancel.Text = "&Cancel";
            this.BtCancel.UseVisualStyleBackColor = true;
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // StyleCategories
            // 
            this.AcceptButton = this.BtSave;
            this.AccessibleName = "StyleCategories";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtCancel;
            this.ClientSize = new System.Drawing.Size(774, 512);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtSave);
            this.Controls.Add(this.DgStyleCategories);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StyleCategories";
            this.ShowIcon = false;
            this.Text = "Style Categories";
            this.Load += new System.EventHandler(this.StyleCategories_Load);
            this.DoubleClick += new System.EventHandler(this.StyleCategories_DoubleClick);
            this.Activated += new System.EventHandler(this.StyleCategories_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.DgStyleCategories)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DgStyleCategories;
        private System.Windows.Forms.Button BtSave;
        private System.Windows.Forms.Button BtCancel;
    }
}
