namespace SIL.PublishingSolution
{
    partial class Booklet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Booklet));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSavedSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDefaultSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSettingsAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bookletBuilderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lstSection = new System.Windows.Forms.ListBox();
            this.lblBooklet = new System.Windows.Forms.Label();
            this.lblCreate = new System.Windows.Forms.Label();
            this.cmbBookletIn = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpConfigure = new System.Windows.Forms.GroupBox();
            this.upDownPage = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.picLayout = new System.Windows.Forms.PictureBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.btnExportNow = new System.Windows.Forms.Button();
            this.progressExport = new System.Windows.Forms.ProgressBar();
            this.label9 = new System.Windows.Forms.Label();
            this.lblExportTo = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblSelectData = new System.Windows.Forms.Label();
            this.rdoExportFrom = new System.Windows.Forms.RadioButton();
            this.rdoExistingFile = new System.Windows.Forms.RadioButton();
            this.lblSource = new System.Windows.Forms.Label();
            this.txtSectName = new System.Windows.Forms.TextBox();
            this.lblSectName = new System.Windows.Forms.Label();
            this.lblSectDetail = new System.Windows.Forms.Label();
            this.progressAll = new System.Windows.Forms.ProgressBar();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpConfigure.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownPage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLayout)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(703, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openSavedSettingsToolStripMenuItem,
            this.openDefaultSettingsToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveSettingsAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openSavedSettingsToolStripMenuItem
            // 
            this.openSavedSettingsToolStripMenuItem.Name = "openSavedSettingsToolStripMenuItem";
            this.openSavedSettingsToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+O";
            this.openSavedSettingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openSavedSettingsToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.openSavedSettingsToolStripMenuItem.Text = "&Open Saved Settings…";
            this.openSavedSettingsToolStripMenuItem.ToolTipText = "Open and load booklet settings you have saved previously";
            this.openSavedSettingsToolStripMenuItem.Click += new System.EventHandler(this.openSavedSettingsToolStripMenuItem_Click);
            // 
            // openDefaultSettingsToolStripMenuItem
            // 
            this.openDefaultSettingsToolStripMenuItem.Name = "openDefaultSettingsToolStripMenuItem";
            this.openDefaultSettingsToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.openDefaultSettingsToolStripMenuItem.Text = "Open &Default Settings";
            this.openDefaultSettingsToolStripMenuItem.ToolTipText = "Load the default booklet settings";
            this.openDefaultSettingsToolStripMenuItem.Click += new System.EventHandler(this.openDefaultSettingsToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+S";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.ToolTipText = "Save the current booklet settings";
            // 
            // saveSettingsAsToolStripMenuItem
            // 
            this.saveSettingsAsToolStripMenuItem.Name = "saveSettingsAsToolStripMenuItem";
            this.saveSettingsAsToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.saveSettingsAsToolStripMenuItem.Text = "Sa&ve Settings as…";
            this.saveSettingsAsToolStripMenuItem.ToolTipText = "Save the current booklet settings as";
            this.saveSettingsAsToolStripMenuItem.Click += new System.EventHandler(this.saveSettingsAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bookletBuilderToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // bookletBuilderToolStripMenuItem
            // 
            this.bookletBuilderToolStripMenuItem.Name = "bookletBuilderToolStripMenuItem";
            this.bookletBuilderToolStripMenuItem.ShortcutKeyDisplayString = "F1";
            this.bookletBuilderToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.bookletBuilderToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.bookletBuilderToolStripMenuItem.Text = "&Booklet Builder";
            this.bookletBuilderToolStripMenuItem.ToolTipText = "Help on using Booklet Builder";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.aboutToolStripMenuItem.Text = "&About ";
            this.aboutToolStripMenuItem.ToolTipText = "Display the version number and other facts about Booklet Builder";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // lstSection
            // 
            this.lstSection.FormattingEnabled = true;
            this.lstSection.Location = new System.Drawing.Point(15, 58);
            this.lstSection.Name = "lstSection";
            this.lstSection.Size = new System.Drawing.Size(250, 264);
            this.lstSection.TabIndex = 1;
            // 
            // lblBooklet
            // 
            this.lblBooklet.AutoSize = true;
            this.lblBooklet.Location = new System.Drawing.Point(19, 40);
            this.lblBooklet.Name = "lblBooklet";
            this.lblBooklet.Size = new System.Drawing.Size(87, 13);
            this.lblBooklet.TabIndex = 2;
            this.lblBooklet.Text = "Booklet Sections";
            // 
            // lblCreate
            // 
            this.lblCreate.AutoSize = true;
            this.lblCreate.Location = new System.Drawing.Point(47, 371);
            this.lblCreate.Name = "lblCreate";
            this.lblCreate.Size = new System.Drawing.Size(91, 13);
            this.lblCreate.TabIndex = 3;
            this.lblCreate.Text = "Create Booklet in:";
            // 
            // cmbBookletIn
            // 
            this.cmbBookletIn.FormattingEnabled = true;
            this.cmbBookletIn.Location = new System.Drawing.Point(144, 368);
            this.cmbBookletIn.Name = "cmbBookletIn";
            this.cmbBookletIn.Size = new System.Drawing.Size(121, 21);
            this.cmbBookletIn.TabIndex = 4;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(15, 327);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(59, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(78, 327);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(59, 23);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(189, 395);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(76, 23);
            this.btnCreate.TabIndex = 7;
            this.btnCreate.Text = "Create...";
            this.btnCreate.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grpConfigure);
            this.groupBox1.Controls.Add(this.rdoExportFrom);
            this.groupBox1.Controls.Add(this.rdoExistingFile);
            this.groupBox1.Controls.Add(this.lblSource);
            this.groupBox1.Controls.Add(this.txtSectName);
            this.groupBox1.Controls.Add(this.lblSectName);
            this.groupBox1.Location = new System.Drawing.Point(312, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(375, 333);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // grpConfigure
            // 
            this.grpConfigure.Controls.Add(this.upDownPage);
            this.grpConfigure.Controls.Add(this.label1);
            this.grpConfigure.Controls.Add(this.picLayout);
            this.grpConfigure.Controls.Add(this.btnBrowse);
            this.grpConfigure.Controls.Add(this.textBox2);
            this.grpConfigure.Controls.Add(this.comboBox4);
            this.grpConfigure.Controls.Add(this.comboBox3);
            this.grpConfigure.Controls.Add(this.comboBox2);
            this.grpConfigure.Controls.Add(this.btnExportNow);
            this.grpConfigure.Controls.Add(this.progressExport);
            this.grpConfigure.Controls.Add(this.label9);
            this.grpConfigure.Controls.Add(this.lblExportTo);
            this.grpConfigure.Controls.Add(this.label7);
            this.grpConfigure.Controls.Add(this.lblSelectData);
            this.grpConfigure.Location = new System.Drawing.Point(20, 117);
            this.grpConfigure.Name = "grpConfigure";
            this.grpConfigure.Size = new System.Drawing.Size(336, 205);
            this.grpConfigure.TabIndex = 15;
            this.grpConfigure.TabStop = false;
            this.grpConfigure.Text = "Configure Export";
            // 
            // upDownPage
            // 
            this.upDownPage.Location = new System.Drawing.Point(244, 136);
            this.upDownPage.Name = "upDownPage";
            this.upDownPage.Size = new System.Drawing.Size(76, 20);
            this.upDownPage.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Save as:";
            // 
            // picLayout
            // 
            this.picLayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLayout.Image = ((System.Drawing.Image)(resources.GetObject("picLayout.Image")));
            this.picLayout.Location = new System.Drawing.Point(302, 52);
            this.picLayout.Name = "picLayout";
            this.picLayout.Size = new System.Drawing.Size(18, 21);
            this.picLayout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLayout.TabIndex = 19;
            this.picLayout.TabStop = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(243, 106);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(77, 23);
            this.btnBrowse.TabIndex = 17;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(91, 106);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(140, 20);
            this.textBox2.TabIndex = 16;
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(91, 79);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(202, 21);
            this.comboBox4.TabIndex = 14;
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(91, 52);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(202, 21);
            this.comboBox3.TabIndex = 13;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(91, 22);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(202, 21);
            this.comboBox2.TabIndex = 12;
            // 
            // btnExportNow
            // 
            this.btnExportNow.Location = new System.Drawing.Point(244, 171);
            this.btnExportNow.Name = "btnExportNow";
            this.btnExportNow.Size = new System.Drawing.Size(76, 23);
            this.btnExportNow.TabIndex = 11;
            this.btnExportNow.Text = "Export Now";
            this.btnExportNow.UseVisualStyleBackColor = true;
            // 
            // progressExport
            // 
            this.progressExport.Location = new System.Drawing.Point(15, 171);
            this.progressExport.Name = "progressExport";
            this.progressExport.Size = new System.Drawing.Size(223, 23);
            this.progressExport.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 140);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(134, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "This section starts at page:";
            // 
            // lblExportTo
            // 
            this.lblExportTo.AutoSize = true;
            this.lblExportTo.Location = new System.Drawing.Point(12, 82);
            this.lblExportTo.Name = "lblExportTo";
            this.lblExportTo.Size = new System.Drawing.Size(52, 13);
            this.lblExportTo.TabIndex = 2;
            this.lblExportTo.Text = "Export to:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Select Layout:";
            // 
            // lblSelectData
            // 
            this.lblSelectData.AutoSize = true;
            this.lblSelectData.Location = new System.Drawing.Point(12, 25);
            this.lblSelectData.Name = "lblSelectData";
            this.lblSelectData.Size = new System.Drawing.Size(66, 13);
            this.lblSelectData.TabIndex = 0;
            this.lblSelectData.Text = "Select Data:";
            // 
            // rdoExportFrom
            // 
            this.rdoExportFrom.AutoSize = true;
            this.rdoExportFrom.Checked = true;
            this.rdoExportFrom.Location = new System.Drawing.Point(20, 87);
            this.rdoExportFrom.Name = "rdoExportFrom";
            this.rdoExportFrom.Size = new System.Drawing.Size(132, 17);
            this.rdoExportFrom.TabIndex = 14;
            this.rdoExportFrom.TabStop = true;
            this.rdoExportFrom.Text = "Export from <Program>";
            this.rdoExportFrom.UseVisualStyleBackColor = true;
            // 
            // rdoExistingFile
            // 
            this.rdoExistingFile.AutoSize = true;
            this.rdoExistingFile.Location = new System.Drawing.Point(20, 66);
            this.rdoExistingFile.Name = "rdoExistingFile";
            this.rdoExistingFile.Size = new System.Drawing.Size(80, 17);
            this.rdoExistingFile.TabIndex = 13;
            this.rdoExistingFile.Text = "Existing File";
            this.rdoExistingFile.UseVisualStyleBackColor = true;
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(17, 48);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(119, 13);
            this.lblSource.TabIndex = 12;
            this.lblSource.Text = "Source of Section data:";
            // 
            // txtSectName
            // 
            this.txtSectName.Location = new System.Drawing.Point(111, 17);
            this.txtSectName.Name = "txtSectName";
            this.txtSectName.Size = new System.Drawing.Size(245, 20);
            this.txtSectName.TabIndex = 11;
            // 
            // lblSectName
            // 
            this.lblSectName.AutoSize = true;
            this.lblSectName.Location = new System.Drawing.Point(17, 20);
            this.lblSectName.Name = "lblSectName";
            this.lblSectName.Size = new System.Drawing.Size(77, 13);
            this.lblSectName.TabIndex = 10;
            this.lblSectName.Text = "Section Name:";
            // 
            // lblSectDetail
            // 
            this.lblSectDetail.AutoSize = true;
            this.lblSectDetail.Location = new System.Drawing.Point(316, 41);
            this.lblSectDetail.Name = "lblSectDetail";
            this.lblSectDetail.Size = new System.Drawing.Size(78, 13);
            this.lblSectDetail.TabIndex = 9;
            this.lblSectDetail.Text = "Section Details";
            // 
            // progressAll
            // 
            this.progressAll.Location = new System.Drawing.Point(312, 394);
            this.progressAll.Name = "progressAll";
            this.progressAll.Size = new System.Drawing.Size(375, 23);
            this.progressAll.TabIndex = 10;
            // 
            // btnDown
            // 
            this.btnDown.BackgroundImage = global::SIL.PublishingSolution.Properties.Resources.down_icon;
            this.btnDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDown.Location = new System.Drawing.Point(271, 150);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(26, 26);
            this.btnDown.TabIndex = 12;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.BackgroundImage = global::SIL.PublishingSolution.Properties.Resources.up_icon;
            this.btnUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUp.Location = new System.Drawing.Point(271, 122);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(26, 26);
            this.btnUp.TabIndex = 11;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // Booklet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 433);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.progressAll);
            this.Controls.Add(this.lblSectDetail);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cmbBookletIn);
            this.Controls.Add(this.lblCreate);
            this.Controls.Add(this.lblBooklet);
            this.Controls.Add(this.lstSection);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Booklet";
            this.ShowIcon = false;
            this.Text = "Pathway Booklet Builder";
            this.Load += new System.EventHandler(this.Booklet_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpConfigure.ResumeLayout(false);
            this.grpConfigure.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownPage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLayout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSavedSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDefaultSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSettingsAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bookletBuilderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ListBox lstSection;
        private System.Windows.Forms.Label lblBooklet;
        private System.Windows.Forms.Label lblCreate;
        private System.Windows.Forms.ComboBox cmbBookletIn;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grpConfigure;
        private System.Windows.Forms.Label lblSelectData;
        private System.Windows.Forms.RadioButton rdoExportFrom;
        private System.Windows.Forms.RadioButton rdoExistingFile;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.TextBox txtSectName;
        private System.Windows.Forms.Label lblSectName;
        private System.Windows.Forms.Label lblSectDetail;
        private System.Windows.Forms.ProgressBar progressAll;
        private System.Windows.Forms.Button btnExportNow;
        private System.Windows.Forms.ProgressBar progressExport;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblExportTo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox picLayout;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.NumericUpDown upDownPage;
    }
}

