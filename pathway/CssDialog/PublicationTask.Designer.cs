namespace SIL.PublishingSolution
{
    partial class PublicationTask
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
            this.BtConfigure = new System.Windows.Forms.Button();
            this.BtPreview = new System.Windows.Forms.Button();
            this.BtOK = new System.Windows.Forms.Button();
            this.BtCancel = new System.Windows.Forms.Button();
            this.PanelRole = new System.Windows.Forms.Panel();
            this.lblRole = new System.Windows.Forms.Label();
            this.PanelSideBar = new System.Windows.Forms.TableLayoutPanel();
            this.PanelDesc = new System.Windows.Forms.Panel();
            this.lblTaskDesc = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.BtnTask = new System.Windows.Forms.Button();
            this.BtnRole = new System.Windows.Forms.Button();
            this.PanelLeftTop = new System.Windows.Forms.Panel();
            this.PanelTask = new System.Windows.Forms.Panel();
            this.LblScripture = new System.Windows.Forms.Label();
            this.PanelRole.SuspendLayout();
            this.PanelSideBar.SuspendLayout();
            this.PanelDesc.SuspendLayout();
            this.panel4.SuspendLayout();
            this.PanelLeftTop.SuspendLayout();
            this.PanelTask.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtConfigure
            // 
            this.BtConfigure.AccessibleName = "BtConfigure";
            this.BtConfigure.Location = new System.Drawing.Point(495, 546);
            this.BtConfigure.Name = "BtConfigure";
            this.BtConfigure.Size = new System.Drawing.Size(75, 23);
            this.BtConfigure.TabIndex = 2;
            this.BtConfigure.Text = "Co&nfigure...";
            this.BtConfigure.UseVisualStyleBackColor = true;
            this.BtConfigure.Visible = false;
            this.BtConfigure.Click += new System.EventHandler(this.BtStyles_Click);
            // 
            // BtPreview
            // 
            this.BtPreview.AccessibleName = "BtPreview";
            this.BtPreview.Location = new System.Drawing.Point(343, 546);
            this.BtPreview.Name = "BtPreview";
            this.BtPreview.Size = new System.Drawing.Size(44, 23);
            this.BtPreview.TabIndex = 3;
            this.BtPreview.Text = "&Preview...";
            this.BtPreview.UseVisualStyleBackColor = true;
            this.BtPreview.Visible = false;
            this.BtPreview.Click += new System.EventHandler(this.BtPreview_Click);
            // 
            // BtOK
            // 
            this.BtOK.AccessibleName = "BtOK";
            this.BtOK.Location = new System.Drawing.Point(576, 546);
            this.BtOK.Name = "BtOK";
            this.BtOK.Size = new System.Drawing.Size(75, 23);
            this.BtOK.TabIndex = 4;
            this.BtOK.Text = "&OK";
            this.BtOK.UseVisualStyleBackColor = true;
            this.BtOK.Click += new System.EventHandler(this.BtOK_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.AccessibleName = "BtCancel";
            this.BtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtCancel.Location = new System.Drawing.Point(657, 546);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(75, 23);
            this.BtCancel.TabIndex = 9;
            this.BtCancel.Text = "&Cancel";
            this.BtCancel.UseVisualStyleBackColor = true;
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // PanelRole
            // 
            this.PanelRole.BackColor = System.Drawing.Color.LightSteelBlue;
            this.PanelRole.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelRole.Controls.Add(this.lblRole);
            this.PanelRole.Location = new System.Drawing.Point(114, 50);
            this.PanelRole.Name = "PanelRole";
            this.PanelRole.Size = new System.Drawing.Size(122, 345);
            this.PanelRole.TabIndex = 44;
            // 
            // lblRole
            // 
            this.lblRole.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblRole.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRole.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRole.ForeColor = System.Drawing.Color.White;
            this.lblRole.Location = new System.Drawing.Point(0, 0);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(120, 31);
            this.lblRole.TabIndex = 5;
            this.lblRole.Text = "Role";
            this.lblRole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PanelSideBar
            // 
            this.PanelSideBar.BackColor = System.Drawing.Color.LightSteelBlue;
            this.PanelSideBar.ColumnCount = 1;
            this.PanelSideBar.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.PanelSideBar.Controls.Add(this.PanelDesc, 0, 1);
            this.PanelSideBar.Controls.Add(this.panel4, 0, 2);
            this.PanelSideBar.Controls.Add(this.PanelLeftTop, 0, 0);
            this.PanelSideBar.Location = new System.Drawing.Point(0, 0);
            this.PanelSideBar.Name = "PanelSideBar";
            this.PanelSideBar.RowCount = 3;
            this.PanelSideBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PanelSideBar.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.PanelSideBar.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.PanelSideBar.Size = new System.Drawing.Size(179, 588);
            this.PanelSideBar.TabIndex = 45;
            // 
            // PanelDesc
            // 
            this.PanelDesc.AutoScroll = true;
            this.PanelDesc.Controls.Add(this.lblTaskDesc);
            this.PanelDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelDesc.Location = new System.Drawing.Point(3, 468);
            this.PanelDesc.Name = "PanelDesc";
            this.PanelDesc.Size = new System.Drawing.Size(173, 41);
            this.PanelDesc.TabIndex = 46;
            // 
            // lblTaskDesc
            // 
            this.lblTaskDesc.AccessibleName = "lbTaskStyle";
            this.lblTaskDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTaskDesc.Location = new System.Drawing.Point(0, 0);
            this.lblTaskDesc.Name = "lblTaskDesc";
            this.lblTaskDesc.Size = new System.Drawing.Size(173, 41);
            this.lblTaskDesc.TabIndex = 6;
            this.lblTaskDesc.Text = "Publication Task";
            this.lblTaskDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTaskDesc.Visible = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.BtnTask);
            this.panel4.Controls.Add(this.BtnRole);
            this.panel4.Location = new System.Drawing.Point(3, 515);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(172, 70);
            this.panel4.TabIndex = 3;
            // 
            // BtnTask
            // 
            this.BtnTask.BackColor = System.Drawing.Color.LightSteelBlue;
            this.BtnTask.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnTask.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Orange;
            this.BtnTask.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Khaki;
            this.BtnTask.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnTask.ForeColor = System.Drawing.Color.Black;
            this.BtnTask.Location = new System.Drawing.Point(0, 0);
            this.BtnTask.Name = "BtnTask";
            this.BtnTask.Size = new System.Drawing.Size(172, 35);
            this.BtnTask.TabIndex = 2;
            this.BtnTask.Text = "Task";
            this.BtnTask.UseVisualStyleBackColor = false;
            this.BtnTask.Click += new System.EventHandler(this.BtnTask_Click);
            // 
            // BtnRole
            // 
            this.BtnRole.BackColor = System.Drawing.Color.LightSteelBlue;
            this.BtnRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnRole.ForeColor = System.Drawing.Color.Black;
            this.BtnRole.Location = new System.Drawing.Point(0, 33);
            this.BtnRole.Name = "BtnRole";
            this.BtnRole.Size = new System.Drawing.Size(172, 35);
            this.BtnRole.TabIndex = 1;
            this.BtnRole.Text = "Role";
            this.BtnRole.UseVisualStyleBackColor = false;
            this.BtnRole.Click += new System.EventHandler(this.BtnRole_Click);
            // 
            // PanelLeftTop
            // 
            this.PanelLeftTop.BackColor = System.Drawing.Color.LightSteelBlue;
            this.PanelLeftTop.Controls.Add(this.PanelRole);
            this.PanelLeftTop.Controls.Add(this.PanelTask);
            this.PanelLeftTop.Location = new System.Drawing.Point(3, 3);
            this.PanelLeftTop.Name = "PanelLeftTop";
            this.PanelLeftTop.Size = new System.Drawing.Size(172, 459);
            this.PanelLeftTop.TabIndex = 43;
            // 
            // PanelTask
            // 
            this.PanelTask.BackColor = System.Drawing.Color.LightSteelBlue;
            this.PanelTask.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTask.Controls.Add(this.LblScripture);
            this.PanelTask.Location = new System.Drawing.Point(4, 22);
            this.PanelTask.Name = "PanelTask";
            this.PanelTask.Size = new System.Drawing.Size(105, 400);
            this.PanelTask.TabIndex = 42;
            // 
            // LblScripture
            // 
            this.LblScripture.BackColor = System.Drawing.Color.RoyalBlue;
            this.LblScripture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblScripture.Dock = System.Windows.Forms.DockStyle.Top;
            this.LblScripture.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblScripture.ForeColor = System.Drawing.Color.White;
            this.LblScripture.Location = new System.Drawing.Point(0, 0);
            this.LblScripture.Name = "LblScripture";
            this.LblScripture.Size = new System.Drawing.Size(103, 31);
            this.LblScripture.TabIndex = 5;
            this.LblScripture.Text = "Task";
            this.LblScripture.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PublicationTask
            // 
            this.AcceptButton = this.BtOK;
            this.AccessibleName = "PublicationTask";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtCancel;
            this.ClientSize = new System.Drawing.Size(744, 589);
            this.Controls.Add(this.PanelSideBar);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtOK);
            this.Controls.Add(this.BtPreview);
            this.Controls.Add(this.BtConfigure);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PublicationTask";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose Publication Task";
            this.Load += new System.EventHandler(this.PublicationTask_Load);
            this.DoubleClick += new System.EventHandler(this.PublicationTask_DoubleClick);
            this.Activated += new System.EventHandler(this.PublicationTask_Activated);
            this.PanelRole.ResumeLayout(false);
            this.PanelSideBar.ResumeLayout(false);
            this.PanelDesc.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.PanelLeftTop.ResumeLayout(false);
            this.PanelTask.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtConfigure;
        private System.Windows.Forms.Button BtPreview;
        private System.Windows.Forms.Button BtOK;
        private System.Windows.Forms.Button BtCancel;
        private System.Windows.Forms.Panel PanelRole;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.TableLayoutPanel PanelSideBar;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button BtnTask;
        private System.Windows.Forms.Button BtnRole;
        private System.Windows.Forms.Panel PanelLeftTop;
        private System.Windows.Forms.Panel PanelTask;
        private System.Windows.Forms.Label LblScripture;
        private System.Windows.Forms.Label lblTaskDesc;
        private System.Windows.Forms.Panel PanelDesc;
    }
}

