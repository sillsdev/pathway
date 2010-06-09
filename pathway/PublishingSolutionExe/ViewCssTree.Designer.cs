namespace SIL.DictionaryExpress
{
    partial class ViewCSSTree
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnLoadCss = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(26, 21);
            this.treeView1.AccessibleName = "treeView1";
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(348, 479);
            this.treeView1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(412, 63);
            this.btnClose.AccessibleName = "btnClose";
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(108, 23);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnLoadCss
            // 
            this.btnLoadCss.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadCss.Location = new System.Drawing.Point(412, 34);
            this.btnLoadCss.AccessibleName = "btnLoadCss";
            this.btnLoadCss.Name = "btnLoadCss";
            this.btnLoadCss.Size = new System.Drawing.Size(108, 23);
            this.btnLoadCss.TabIndex = 22;
            this.btnLoadCss.Text = "&Load CSS";
            this.btnLoadCss.UseVisualStyleBackColor = true;
            this.btnLoadCss.Click += new System.EventHandler(this.btnLoadCss_Click);
            // 
            // ViewCSSTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 524);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnLoadCss);
            this.Controls.Add(this.treeView1);
            this.AccessibleName = "ViewCSSTree";
            this.Name = "ViewCSSTree";
            this.Text = "ViewCSSTree";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnLoadCss;
    }
}
