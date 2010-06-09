namespace SIL.PublishingSolution
{
    partial class Converter
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

                while (true)
                {
                    System.Windows.Forms.Application.DoEvents();
                    try
                    {
                        base.Dispose(disposing);
                        break;
                    }
                    catch { }
                }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Converter));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.excludeInProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.includeInProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAsDefaultCSSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addExistingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderInWindowsExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excludeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtCSS = new System.Windows.Forms.TextBox();
            this.webPreview = new System.Windows.Forms.WebBrowser();
            this.lblCaption = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnShowAll = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.imagePreview = new System.Windows.Forms.WebBrowser();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblTaskDesc = new System.Windows.Forms.Label();
            this.BtnScripture = new System.Windows.Forms.Button();
            this.BtnPublishingFile = new System.Windows.Forms.Button();
            this.PanelLeftTop = new System.Windows.Forms.Panel();
            this.PanelTask = new System.Windows.Forms.Panel();
            this.LblScripture = new System.Windows.Forms.Label();
            this.PanelFile = new System.Windows.Forms.TableLayoutPanel();
            this.DictionaryExplorer = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblProjectType = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel4.SuspendLayout();
            this.PanelLeftTop.SuspendLayout();
            this.PanelTask.SuspendLayout();
            this.PanelFile.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.AccessibleName = "contextMenuStrip1";
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem6,
            this.deleteToolStripMenuItem1,
            this.renameToolStripMenuItem1,
            this.excludeInProjectToolStripMenuItem,
            this.includeInProjectToolStripMenuItem,
            this.setAsDefaultCSSToolStripMenuItem,
            this.addExistingFolderToolStripMenuItem,
            this.backupToolStripMenuItem,
            this.openFolderInWindowsExplorerToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(234, 224);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.AccessibleName = "toolStripMenuItem4";
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(233, 22);
            this.toolStripMenuItem4.Text = "Add File";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.AccessibleName = "toolStripMenuItem6";
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(233, 22);
            this.toolStripMenuItem6.Text = "Add New Folder";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.AccessibleName = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(233, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            this.deleteToolStripMenuItem1.Click += new System.EventHandler(this.deleteToolStripMenuItem1_Click);
            // 
            // renameToolStripMenuItem1
            // 
            this.renameToolStripMenuItem1.AccessibleName = "renameToolStripMenuItem1";
            this.renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            this.renameToolStripMenuItem1.Size = new System.Drawing.Size(233, 22);
            this.renameToolStripMenuItem1.Text = "Rename";
            this.renameToolStripMenuItem1.Click += new System.EventHandler(this.renameToolStripMenuItem1_Click_1);
            // 
            // excludeInProjectToolStripMenuItem
            // 
            this.excludeInProjectToolStripMenuItem.AccessibleName = "excludeInProjectToolStripMenuItem";
            this.excludeInProjectToolStripMenuItem.Name = "excludeInProjectToolStripMenuItem";
            this.excludeInProjectToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.excludeInProjectToolStripMenuItem.Text = "Exclude from Project";
            this.excludeInProjectToolStripMenuItem.Click += new System.EventHandler(this.excludeInProjectToolStripMenuItem_Click_1);
            // 
            // includeInProjectToolStripMenuItem
            // 
            this.includeInProjectToolStripMenuItem.AccessibleName = "includeInProjectToolStripMenuItem";
            this.includeInProjectToolStripMenuItem.Name = "includeInProjectToolStripMenuItem";
            this.includeInProjectToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.includeInProjectToolStripMenuItem.Text = "Include in Project";
            this.includeInProjectToolStripMenuItem.Click += new System.EventHandler(this.includeInProjectToolStripMenuItem_Click);
            // 
            // setAsDefaultCSSToolStripMenuItem
            // 
            this.setAsDefaultCSSToolStripMenuItem.AccessibleName = "setAsDefaultCSSToolStripMenuItem";
            this.setAsDefaultCSSToolStripMenuItem.Name = "setAsDefaultCSSToolStripMenuItem";
            this.setAsDefaultCSSToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.setAsDefaultCSSToolStripMenuItem.Text = "Set as default ";
            this.setAsDefaultCSSToolStripMenuItem.Click += new System.EventHandler(this.setAsDefaultCSSToolStripMenuItem_Click);
            // 
            // addExistingFolderToolStripMenuItem
            // 
            this.addExistingFolderToolStripMenuItem.AccessibleName = "addExistingFolderToolStripMenuItem";
            this.addExistingFolderToolStripMenuItem.Name = "addExistingFolderToolStripMenuItem";
            this.addExistingFolderToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.addExistingFolderToolStripMenuItem.Text = "Add Existing Folder";
            this.addExistingFolderToolStripMenuItem.Click += new System.EventHandler(this.addExistingFolderToolStripMenuItem_Click);
            // 
            // backupToolStripMenuItem
            // 
            this.backupToolStripMenuItem.AccessibleName = "backupToolStripMenuItem";
            this.backupToolStripMenuItem.Name = "backupToolStripMenuItem";
            this.backupToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.backupToolStripMenuItem.Text = "Backup ";
            this.backupToolStripMenuItem.Click += new System.EventHandler(this.backupToolStripMenuItem_Click_1);
            // 
            // openFolderInWindowsExplorerToolStripMenuItem
            // 
            this.openFolderInWindowsExplorerToolStripMenuItem.AccessibleName = "openFolderInWindowsExplorerToolStripMenuItem";
            this.openFolderInWindowsExplorerToolStripMenuItem.Name = "openFolderInWindowsExplorerToolStripMenuItem";
            this.openFolderInWindowsExplorerToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.openFolderInWindowsExplorerToolStripMenuItem.Text = "Open Folder in Windows Explorer";
            this.openFolderInWindowsExplorerToolStripMenuItem.Click += new System.EventHandler(this.openFolderInWindowsExplorerToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "FilAdded.ico");
            this.imageList1.Images.SetKeyName(1, "FileRemoved.ICO");
            this.imageList1.Images.SetKeyName(2, "file.ico");
            this.imageList1.Images.SetKeyName(3, "Project.ico");
            this.imageList1.Images.SetKeyName(4, "folder.ico");
            this.imageList1.Images.SetKeyName(5, "folderopen.ico");
            this.imageList1.Images.SetKeyName(6, "folderremoved.ico");
            this.imageList1.Images.SetKeyName(7, "My Fonts_32.png");
            this.imageList1.Images.SetKeyName(8, "FilDefault.bmp");
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.AccessibleName = "addToolStripMenuItem";
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.AccessibleName = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.AccessibleName = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            // 
            // excludeToolStripMenuItem
            // 
            this.excludeToolStripMenuItem.AccessibleName = "excludeToolStripMenuItem";
            this.excludeToolStripMenuItem.Name = "excludeToolStripMenuItem";
            this.excludeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.excludeToolStripMenuItem.Text = "Exclude";
            // 
            // txtCSS
            // 
            this.txtCSS.AccessibleName = "txtCSS";
            this.txtCSS.BackColor = System.Drawing.Color.White;
            this.txtCSS.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCSS.Location = new System.Drawing.Point(7, 14);
            this.txtCSS.Multiline = true;
            this.txtCSS.Name = "txtCSS";
            this.txtCSS.ReadOnly = true;
            this.txtCSS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCSS.Size = new System.Drawing.Size(59, 67);
            this.txtCSS.TabIndex = 40;
            this.txtCSS.WordWrap = false;
            this.txtCSS.TextChanged += new System.EventHandler(this.txtCSS_TextChanged);
            // 
            // webPreview
            // 
            this.webPreview.AccessibleName = "webPreview";
            this.webPreview.Location = new System.Drawing.Point(62, 98);
            this.webPreview.MinimumSize = new System.Drawing.Size(20, 20);
            this.webPreview.Name = "webPreview";
            this.webPreview.Size = new System.Drawing.Size(47, 55);
            this.webPreview.TabIndex = 40;
            this.webPreview.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webPreview_DocumentCompleted);
            // 
            // lblCaption
            // 
            this.lblCaption.AccessibleName = "lblCaption";
            this.lblCaption.AutoSize = true;
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaption.ForeColor = System.Drawing.Color.White;
            this.lblCaption.Location = new System.Drawing.Point(4, 6);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(67, 18);
            this.lblCaption.TabIndex = 41;
            this.lblCaption.Text = "ShowPreview";
            // 
            // btnRefresh
            // 
            this.btnRefresh.AccessibleName = "btnRefresh";
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(6, 35);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(40, 29);
            this.btnRefresh.TabIndex = 13;
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.btnRefresh, "Refresh");
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.Refresh_Click_1);
            // 
            // btnShowAll
            // 
            this.btnShowAll.AccessibleName = "btnShowAll";
            this.btnShowAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnShowAll.Image = ((System.Drawing.Image)(resources.GetObject("btnShowAll.Image")));
            this.btnShowAll.Location = new System.Drawing.Point(52, 35);
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new System.Drawing.Size(46, 29);
            this.btnShowAll.TabIndex = 12;
            this.btnShowAll.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnShowAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.btnShowAll, "Show All");
            this.btnShowAll.UseVisualStyleBackColor = true;
            this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 185F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(690, 556);
            this.tableLayoutPanel1.TabIndex = 44;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(187, 2);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(501, 552);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel3.Controls.Add(this.imagePreview);
            this.panel3.Controls.Add(this.webPreview);
            this.panel3.Controls.Add(this.txtCSS);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 37);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(495, 512);
            this.panel3.TabIndex = 45;
            // 
            // imagePreview
            // 
            this.imagePreview.AccessibleName = "imagePreview";
            this.imagePreview.Location = new System.Drawing.Point(88, 14);
            this.imagePreview.MinimumSize = new System.Drawing.Size(20, 20);
            this.imagePreview.Name = "imagePreview";
            this.imagePreview.Size = new System.Drawing.Size(47, 55);
            this.imagePreview.TabIndex = 41;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel2.Controls.Add(this.lblCaption);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.ForeColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(495, 29);
            this.panel2.TabIndex = 45;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoScroll = true;
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.panel4, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.PanelLeftTop, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 193F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(179, 550);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lblTaskDesc);
            this.panel4.Controls.Add(this.BtnScripture);
            this.panel4.Controls.Add(this.BtnPublishingFile);
            this.panel4.Location = new System.Drawing.Point(3, 360);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(172, 187);
            this.panel4.TabIndex = 3;
            // 
            // lblTaskDesc
            // 
            this.lblTaskDesc.AccessibleName = "lbTaskStyle";
            this.lblTaskDesc.Location = new System.Drawing.Point(-3, 0);
            this.lblTaskDesc.Name = "lblTaskDesc";
            this.lblTaskDesc.Size = new System.Drawing.Size(174, 103);
            this.lblTaskDesc.TabIndex = 44;
            this.lblTaskDesc.Text = "Publication Task";
            this.lblTaskDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTaskDesc.Visible = false;
            // 
            // BtnScripture
            // 
            this.BtnScripture.BackColor = System.Drawing.Color.LightSteelBlue;
            this.BtnScripture.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnScripture.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Orange;
            this.BtnScripture.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Khaki;
            this.BtnScripture.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnScripture.ForeColor = System.Drawing.Color.Black;
            this.BtnScripture.Location = new System.Drawing.Point(-1, 146);
            this.BtnScripture.Name = "BtnScripture";
            this.BtnScripture.Size = new System.Drawing.Size(172, 35);
            this.BtnScripture.TabIndex = 2;
            this.BtnScripture.Text = "Task";
            this.BtnScripture.UseVisualStyleBackColor = false;
            this.BtnScripture.Click += new System.EventHandler(this.BtnScripture_Click);
            // 
            // BtnPublishingFile
            // 
            this.BtnPublishingFile.BackColor = System.Drawing.Color.LightSteelBlue;
            this.BtnPublishingFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPublishingFile.ForeColor = System.Drawing.Color.Black;
            this.BtnPublishingFile.Location = new System.Drawing.Point(-1, 115);
            this.BtnPublishingFile.Name = "BtnPublishingFile";
            this.BtnPublishingFile.Size = new System.Drawing.Size(172, 35);
            this.BtnPublishingFile.TabIndex = 1;
            this.BtnPublishingFile.Text = "Publishing Files";
            this.BtnPublishingFile.UseVisualStyleBackColor = false;
            this.BtnPublishingFile.Click += new System.EventHandler(this.BtnPublishingFile_Click);
            // 
            // PanelLeftTop
            // 
            this.PanelLeftTop.BackColor = System.Drawing.SystemColors.Control;
            this.PanelLeftTop.Controls.Add(this.PanelTask);
            this.PanelLeftTop.Controls.Add(this.PanelFile);
            this.PanelLeftTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelLeftTop.Location = new System.Drawing.Point(3, 3);
            this.PanelLeftTop.Name = "PanelLeftTop";
            this.PanelLeftTop.Size = new System.Drawing.Size(173, 351);
            this.PanelLeftTop.TabIndex = 43;
            this.PanelLeftTop.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelLeftTop_Paint);
            // 
            // PanelTask
            // 
            this.PanelTask.AutoScroll = true;
            this.PanelTask.BackColor = System.Drawing.Color.LightSteelBlue;
            this.PanelTask.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTask.Controls.Add(this.LblScripture);
            this.PanelTask.Location = new System.Drawing.Point(36, 114);
            this.PanelTask.Name = "PanelTask";
            this.PanelTask.Size = new System.Drawing.Size(165, 345);
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
            this.LblScripture.Size = new System.Drawing.Size(163, 31);
            this.LblScripture.TabIndex = 5;
            this.LblScripture.Text = "Task";
            this.LblScripture.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PanelFile
            // 
            this.PanelFile.BackColor = System.Drawing.Color.LightSteelBlue;
            this.PanelFile.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.PanelFile.ColumnCount = 1;
            this.PanelFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PanelFile.Controls.Add(this.DictionaryExplorer, 0, 1);
            this.PanelFile.Controls.Add(this.panel1, 0, 0);
            this.PanelFile.Location = new System.Drawing.Point(6, 5);
            this.PanelFile.Margin = new System.Windows.Forms.Padding(2);
            this.PanelFile.Name = "PanelFile";
            this.PanelFile.RowCount = 2;
            this.PanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.PanelFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.PanelFile.Size = new System.Drawing.Size(165, 107);
            this.PanelFile.TabIndex = 0;
            // 
            // DictionaryExplorer
            // 
            this.DictionaryExplorer.AccessibleName = "DictionaryExplorer";
            this.DictionaryExplorer.ContextMenuStrip = this.contextMenuStrip1;
            this.DictionaryExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DictionaryExplorer.ImageIndex = 0;
            this.DictionaryExplorer.ImageList = this.imageList1;
            this.DictionaryExplorer.Location = new System.Drawing.Point(4, 77);
            this.DictionaryExplorer.Name = "DictionaryExplorer";
            this.DictionaryExplorer.SelectedImageIndex = 0;
            this.DictionaryExplorer.Size = new System.Drawing.Size(157, 26);
            this.DictionaryExplorer.TabIndex = 9;
            this.DictionaryExplorer.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.DictionaryExplorer_AfterLabelEdit);
            this.DictionaryExplorer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DictionaryExplorer_MouseDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblProjectType);
            this.panel1.Controls.Add(this.btnShowAll);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(159, 68);
            this.panel1.TabIndex = 45;
            // 
            // lblProjectType
            // 
            this.lblProjectType.AccessibleName = "lblProjectType";
            this.lblProjectType.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblProjectType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblProjectType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProjectType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectType.ForeColor = System.Drawing.Color.White;
            this.lblProjectType.Location = new System.Drawing.Point(0, 0);
            this.lblProjectType.Name = "lblProjectType";
            this.lblProjectType.Size = new System.Drawing.Size(159, 31);
            this.lblProjectType.TabIndex = 43;
            this.lblProjectType.Text = "Type";
            this.lblProjectType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Converter
            // 
            this.AccessibleName = "Converter";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(690, 556);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Converter";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Converter";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.OpenOfficeConverter_Load);
            this.DoubleClick += new System.EventHandler(this.Converter_DoubleClick);
            this.Activated += new System.EventHandler(this.Converter_Activated);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.PanelLeftTop.ResumeLayout(false);
            this.PanelTask.ResumeLayout(false);
            this.PanelFile.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excludeToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem excludeInProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem includeInProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAsDefaultCSSToolStripMenuItem;
        private System.Windows.Forms.TextBox txtCSS;
        private System.Windows.Forms.WebBrowser webPreview;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem addExistingFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderInWindowsExplorerToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.WebBrowser imagePreview;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblTaskDesc;
        private System.Windows.Forms.Button BtnScripture;
        private System.Windows.Forms.Button BtnPublishingFile;
        private System.Windows.Forms.Panel PanelLeftTop;
        private System.Windows.Forms.Panel PanelTask;
        private System.Windows.Forms.Label LblScripture;
        private System.Windows.Forms.TableLayoutPanel PanelFile;
        private System.Windows.Forms.TreeView DictionaryExplorer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblProjectType;
        private System.Windows.Forms.Button btnShowAll;
        private System.Windows.Forms.Button btnRefresh;
    }
}
