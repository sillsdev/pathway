using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace PrincessConvert
{
    partial class Main : System.Windows.Forms.Form
    {

        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        //Required by the Windows Form Designer

        private System.ComponentModel.IContainer components;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle39 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle40 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DocumentLocalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DocumentURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DocumentMultipleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.StylesheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GPSStylesheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.PageSetupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PrintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DocumentSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.StylesheetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.GPSStylesheetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.GeneratedStylesheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.PDFToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.INIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.LogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PDFDisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.DebuggingModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PicturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.SelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.InsertHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
            this.RemoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingPlus60MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingPlus40MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingPlus35MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingPlus30MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.TrackingPlus25MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingPlus20MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingPlus15MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingPlus10MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingPlus05MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingNoneMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingMinus05MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingMinus10MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingMinus15MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingMinus20MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingMinus25MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.TrackingMinus30MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingMinus35MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrackingMinus40MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LicenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ContentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutPrinceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AxAcroPDF1 = new AxAcroPDFLib.AxAcroPDF();
            this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnChangePictureSettings = new System.Windows.Forms.Button();
            this.tbFeedback = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnToggleToolbar = new System.Windows.Forms.ToolStripButton();
            this.FullWidthToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnConvert = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnStart = new System.Windows.Forms.ToolStripButton();
            this.tsbtnPrevious = new System.Windows.Forms.ToolStripButton();
            this.tstbCurrentPageNumber = new System.Windows.Forms.ToolStripTextBox();
            this.tsbtnCurrent = new System.Windows.Forms.ToolStripButton();
            this.tsbtnNext = new System.Windows.Forms.ToolStripButton();
            this.tsbtnEnd = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bWkrPrince = new System.ComponentModel.BackgroundWorker();
            this.PageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.dgvFilesToProcess = new System.Windows.Forms.DataGridView();
            this.OpenFileDialogXML = new System.Windows.Forms.OpenFileDialog();
            this.FolderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.TreeView1 = new System.Windows.Forms.TreeView();
            this.btnAlternateSelector = new System.Windows.Forms.Button();
            this.tbAlternateSelector = new System.Windows.Forms.TextBox();
            this.tbURL = new System.Windows.Forms.TextBox();
            this.lblURL = new System.Windows.Forms.Label();
            this.pnlURL = new System.Windows.Forms.Panel();
            this.OK_Button = new System.Windows.Forms.Button();
            this.Cancel_Button = new System.Windows.Forms.Button();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.cbColorizeTracking = new System.Windows.Forms.CheckBox();
            this.trackingPlus05 = new System.Windows.Forms.Button();
            this.trackingMinus05 = new System.Windows.Forms.Button();
            this.minus25 = new System.Windows.Forms.Button();
            this.minus40 = new System.Windows.Forms.Button();
            this.minus35 = new System.Windows.Forms.Button();
            this.Button14 = new System.Windows.Forms.Button();
            this.minus30 = new System.Windows.Forms.Button();
            this.minus20 = new System.Windows.Forms.Button();
            this.minus15 = new System.Windows.Forms.Button();
            this.minus10 = new System.Windows.Forms.Button();
            this.minus05 = new System.Windows.Forms.Button();
            this.none = new System.Windows.Forms.Button();
            this.plus05 = new System.Windows.Forms.Button();
            this.plus10 = new System.Windows.Forms.Button();
            this.plus15 = new System.Windows.Forms.Button();
            this.plus20 = new System.Windows.Forms.Button();
            this.plus25 = new System.Windows.Forms.Button();
            this.plus30 = new System.Windows.Forms.Button();
            this.plus35 = new System.Windows.Forms.Button();
            this.plus40 = new System.Windows.Forms.Button();
            this.plus60 = new System.Windows.Forms.Button();
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.MenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AxAcroPDF1)).BeginInit();
            this.ToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilesToProcess)).BeginInit();
            this.pnlURL.SuspendLayout();
            this.Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.ViewToolStripMenuItem,
            this.LogToolStripMenuItem,
            this.ConfigurationToolStripMenuItem,
            this.PicturesToolStripMenuItem,
            this.TrackingToolStripMenuItem,
            this.HelpToolStripMenuItem});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(1028, 24);
            this.MenuStrip1.TabIndex = 1;
            this.MenuStrip1.Text = "MenuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DocumentLocalToolStripMenuItem,
            this.DocumentURLToolStripMenuItem,
            this.DocumentMultipleToolStripMenuItem,
            this.ToolStripMenuItem1,
            this.StylesheetToolStripMenuItem,
            this.GPSStylesheetToolStripMenuItem,
            this.ToolStripMenuItem7,
            this.PageSetupToolStripMenuItem,
            this.PrintToolStripMenuItem,
            this.ToolStripMenuItem2,
            this.ExitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.FileToolStripMenuItem.Text = "File";
            // 
            // DocumentLocalToolStripMenuItem
            // 
            this.DocumentLocalToolStripMenuItem.Name = "DocumentLocalToolStripMenuItem";
            this.DocumentLocalToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.DocumentLocalToolStripMenuItem.Text = "Document local ...";
            this.DocumentLocalToolStripMenuItem.Click += new System.EventHandler(this.DocumentLocalToolStripMenuItem_Click);
            // 
            // DocumentURLToolStripMenuItem
            // 
            this.DocumentURLToolStripMenuItem.Name = "DocumentURLToolStripMenuItem";
            this.DocumentURLToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.DocumentURLToolStripMenuItem.Text = "Document URL ...";
            this.DocumentURLToolStripMenuItem.Click += new System.EventHandler(this.DocumentInternetToolStripMenuItem_Click);
            // 
            // DocumentMultipleToolStripMenuItem
            // 
            this.DocumentMultipleToolStripMenuItem.Name = "DocumentMultipleToolStripMenuItem";
            this.DocumentMultipleToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.DocumentMultipleToolStripMenuItem.Text = "Combine local documents ...";
            this.DocumentMultipleToolStripMenuItem.Click += new System.EventHandler(this.DocumentMultipleToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(206, 6);
            // 
            // StylesheetToolStripMenuItem
            // 
            this.StylesheetToolStripMenuItem.Name = "StylesheetToolStripMenuItem";
            this.StylesheetToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.StylesheetToolStripMenuItem.Text = "Stylesheet ...";
            this.StylesheetToolStripMenuItem.Click += new System.EventHandler(this.StylesheetToolStripMenuItem_Click);
            // 
            // GPSStylesheetToolStripMenuItem
            // 
            this.GPSStylesheetToolStripMenuItem.Name = "GPSStylesheetToolStripMenuItem";
            this.GPSStylesheetToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.GPSStylesheetToolStripMenuItem.Text = "GPS stylesheet ...";
            this.GPSStylesheetToolStripMenuItem.Click += new System.EventHandler(this.GPSStylesheetToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem7
            // 
            this.ToolStripMenuItem7.Name = "ToolStripMenuItem7";
            this.ToolStripMenuItem7.Size = new System.Drawing.Size(206, 6);
            // 
            // PageSetupToolStripMenuItem
            // 
            this.PageSetupToolStripMenuItem.Name = "PageSetupToolStripMenuItem";
            this.PageSetupToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.PageSetupToolStripMenuItem.Text = "Page setup ...";
            this.PageSetupToolStripMenuItem.Click += new System.EventHandler(this.PageSetupToolStripMenuItem_Click);
            // 
            // PrintToolStripMenuItem
            // 
            this.PrintToolStripMenuItem.Name = "PrintToolStripMenuItem";
            this.PrintToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.PrintToolStripMenuItem.Text = "Print ...";
            this.PrintToolStripMenuItem.Click += new System.EventHandler(this.PrintToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem2
            // 
            this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
            this.ToolStripMenuItem2.Size = new System.Drawing.Size(206, 6);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.ExitToolStripMenuItem.Text = "Exit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // ViewToolStripMenuItem
            // 
            this.ViewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DocumentSourceToolStripMenuItem,
            this.ToolStripMenuItem3,
            this.StylesheetToolStripMenuItem1,
            this.GPSStylesheetToolStripMenuItem1,
            this.GeneratedStylesheetToolStripMenuItem,
            this.ToolStripMenuItem4,
            this.PDFToolStripMenuItem1,
            this.ToolStripMenuItem5,
            this.INIToolStripMenuItem,
            this.ToolStripMenuItem6});
            this.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem";
            this.ViewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.ViewToolStripMenuItem.Text = "View";
            // 
            // DocumentSourceToolStripMenuItem
            // 
            this.DocumentSourceToolStripMenuItem.Name = "DocumentSourceToolStripMenuItem";
            this.DocumentSourceToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.DocumentSourceToolStripMenuItem.Text = "Document source";
            // 
            // ToolStripMenuItem3
            // 
            this.ToolStripMenuItem3.Name = "ToolStripMenuItem3";
            this.ToolStripMenuItem3.Size = new System.Drawing.Size(175, 6);
            // 
            // StylesheetToolStripMenuItem1
            // 
            this.StylesheetToolStripMenuItem1.Name = "StylesheetToolStripMenuItem1";
            this.StylesheetToolStripMenuItem1.Size = new System.Drawing.Size(178, 22);
            this.StylesheetToolStripMenuItem1.Text = "Stylesheet";
            // 
            // GPSStylesheetToolStripMenuItem1
            // 
            this.GPSStylesheetToolStripMenuItem1.Name = "GPSStylesheetToolStripMenuItem1";
            this.GPSStylesheetToolStripMenuItem1.Size = new System.Drawing.Size(178, 22);
            this.GPSStylesheetToolStripMenuItem1.Text = "GPS stylesheet";
            // 
            // GeneratedStylesheetToolStripMenuItem
            // 
            this.GeneratedStylesheetToolStripMenuItem.Name = "GeneratedStylesheetToolStripMenuItem";
            this.GeneratedStylesheetToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.GeneratedStylesheetToolStripMenuItem.Text = "Generated stylesheet";
            // 
            // ToolStripMenuItem4
            // 
            this.ToolStripMenuItem4.Name = "ToolStripMenuItem4";
            this.ToolStripMenuItem4.Size = new System.Drawing.Size(175, 6);
            // 
            // PDFToolStripMenuItem1
            // 
            this.PDFToolStripMenuItem1.Name = "PDFToolStripMenuItem1";
            this.PDFToolStripMenuItem1.Size = new System.Drawing.Size(178, 22);
            this.PDFToolStripMenuItem1.Text = "PDF";
            this.PDFToolStripMenuItem1.Click += new System.EventHandler(this.PDFToolStripMenuItem1_Click);
            // 
            // ToolStripMenuItem5
            // 
            this.ToolStripMenuItem5.Name = "ToolStripMenuItem5";
            this.ToolStripMenuItem5.Size = new System.Drawing.Size(175, 6);
            // 
            // INIToolStripMenuItem
            // 
            this.INIToolStripMenuItem.Name = "INIToolStripMenuItem";
            this.INIToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.INIToolStripMenuItem.Text = "INI";
            // 
            // ToolStripMenuItem6
            // 
            this.ToolStripMenuItem6.Name = "ToolStripMenuItem6";
            this.ToolStripMenuItem6.Size = new System.Drawing.Size(175, 6);
            // 
            // LogToolStripMenuItem
            // 
            this.LogToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurrentToolStripMenuItem,
            this.HistoryToolStripMenuItem});
            this.LogToolStripMenuItem.Name = "LogToolStripMenuItem";
            this.LogToolStripMenuItem.Size = new System.Drawing.Size(36, 20);
            this.LogToolStripMenuItem.Text = "Log";
            // 
            // CurrentToolStripMenuItem
            // 
            this.CurrentToolStripMenuItem.Name = "CurrentToolStripMenuItem";
            this.CurrentToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.CurrentToolStripMenuItem.Text = "Current";
            // 
            // HistoryToolStripMenuItem
            // 
            this.HistoryToolStripMenuItem.Name = "HistoryToolStripMenuItem";
            this.HistoryToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.HistoryToolStripMenuItem.Text = "History";
            this.HistoryToolStripMenuItem.Click += new System.EventHandler(this.HistoryToolStripMenuItem_Click);
            // 
            // ConfigurationToolStripMenuItem
            // 
            this.ConfigurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SettingsToolStripMenuItem,
            this.PDFDisplayToolStripMenuItem,
            this.ToolStripMenuItem8,
            this.DebuggingModeToolStripMenuItem});
            this.ConfigurationToolStripMenuItem.Name = "ConfigurationToolStripMenuItem";
            this.ConfigurationToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.ConfigurationToolStripMenuItem.Text = "Configuration";
            // 
            // SettingsToolStripMenuItem
            // 
            this.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem";
            this.SettingsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.SettingsToolStripMenuItem.Text = "Settings ...";
            this.SettingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItem_Click);
            // 
            // PDFDisplayToolStripMenuItem
            // 
            this.PDFDisplayToolStripMenuItem.Name = "PDFDisplayToolStripMenuItem";
            this.PDFDisplayToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.PDFDisplayToolStripMenuItem.Text = "PDF display ...";
            this.PDFDisplayToolStripMenuItem.Click += new System.EventHandler(this.PDFDisplayToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem8
            // 
            this.ToolStripMenuItem8.Name = "ToolStripMenuItem8";
            this.ToolStripMenuItem8.Size = new System.Drawing.Size(151, 6);
            // 
            // DebuggingModeToolStripMenuItem
            // 
            this.DebuggingModeToolStripMenuItem.Name = "DebuggingModeToolStripMenuItem";
            this.DebuggingModeToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.DebuggingModeToolStripMenuItem.Text = "Debugging mode";
            // 
            // PicturesToolStripMenuItem
            // 
            this.PicturesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FolderToolStripMenuItem,
            this.ToolStripMenuItem12,
            this.SelectToolStripMenuItem,
            this.ToolStripMenuItem11,
            this.InsertHereToolStripMenuItem,
            this.ToolStripMenuItem13,
            this.RemoveToolStripMenuItem,
            this.RemoveAllToolStripMenuItem});
            this.PicturesToolStripMenuItem.Name = "PicturesToolStripMenuItem";
            this.PicturesToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.PicturesToolStripMenuItem.Text = "Picture";
            // 
            // FolderToolStripMenuItem
            // 
            this.FolderToolStripMenuItem.Name = "FolderToolStripMenuItem";
            this.FolderToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.FolderToolStripMenuItem.Text = "Folder ...";
            // 
            // ToolStripMenuItem12
            // 
            this.ToolStripMenuItem12.Name = "ToolStripMenuItem12";
            this.ToolStripMenuItem12.Size = new System.Drawing.Size(163, 6);
            // 
            // SelectToolStripMenuItem
            // 
            this.SelectToolStripMenuItem.Name = "SelectToolStripMenuItem";
            this.SelectToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.SelectToolStripMenuItem.Text = "Change location ...";
            // 
            // ToolStripMenuItem11
            // 
            this.ToolStripMenuItem11.Name = "ToolStripMenuItem11";
            this.ToolStripMenuItem11.Size = new System.Drawing.Size(163, 6);
            // 
            // InsertHereToolStripMenuItem
            // 
            this.InsertHereToolStripMenuItem.Name = "InsertHereToolStripMenuItem";
            this.InsertHereToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.InsertHereToolStripMenuItem.Text = "Insert before ...";
            this.InsertHereToolStripMenuItem.Click += new System.EventHandler(this.InsertHereToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem13
            // 
            this.ToolStripMenuItem13.Name = "ToolStripMenuItem13";
            this.ToolStripMenuItem13.Size = new System.Drawing.Size(163, 6);
            // 
            // RemoveToolStripMenuItem
            // 
            this.RemoveToolStripMenuItem.Name = "RemoveToolStripMenuItem";
            this.RemoveToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.RemoveToolStripMenuItem.Text = "Delete ...";
            this.RemoveToolStripMenuItem.Click += new System.EventHandler(this.RemoveToolStripMenuItem_Click);
            // 
            // RemoveAllToolStripMenuItem
            // 
            this.RemoveAllToolStripMenuItem.Name = "RemoveAllToolStripMenuItem";
            this.RemoveAllToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.RemoveAllToolStripMenuItem.Text = "Delete all";
            this.RemoveAllToolStripMenuItem.Click += new System.EventHandler(this.RemoveAllToolStripMenuItem_Click);
            // 
            // TrackingToolStripMenuItem
            // 
            this.TrackingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TrackingPlus60MenuItem,
            this.TrackingPlus40MenuItem,
            this.TrackingPlus35MenuItem,
            this.TrackingPlus30MenuItem,
            this.ToolStripMenuItem9,
            this.TrackingPlus25MenuItem,
            this.TrackingPlus20MenuItem,
            this.TrackingPlus15MenuItem,
            this.TrackingPlus10MenuItem,
            this.TrackingPlus05MenuItem,
            this.TrackingNoneMenuItem,
            this.TrackingMinus05MenuItem,
            this.TrackingMinus10MenuItem,
            this.TrackingMinus15MenuItem,
            this.TrackingMinus20MenuItem,
            this.TrackingMinus25MenuItem,
            this.ToolStripMenuItem10,
            this.TrackingMinus30MenuItem,
            this.TrackingMinus35MenuItem,
            this.TrackingMinus40MenuItem});
            this.TrackingToolStripMenuItem.Name = "TrackingToolStripMenuItem";
            this.TrackingToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.TrackingToolStripMenuItem.Text = "Tracking";
            // 
            // TrackingPlus60MenuItem
            // 
            this.TrackingPlus60MenuItem.BackColor = System.Drawing.Color.Red;
            this.TrackingPlus60MenuItem.Name = "TrackingPlus60MenuItem";
            this.TrackingPlus60MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingPlus60MenuItem.Text = "+60 pt";
            // 
            // TrackingPlus40MenuItem
            // 
            this.TrackingPlus40MenuItem.BackColor = System.Drawing.Color.DarkViolet;
            this.TrackingPlus40MenuItem.Name = "TrackingPlus40MenuItem";
            this.TrackingPlus40MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingPlus40MenuItem.Text = "+40 pt";
            // 
            // TrackingPlus35MenuItem
            // 
            this.TrackingPlus35MenuItem.BackColor = System.Drawing.Color.Magenta;
            this.TrackingPlus35MenuItem.Name = "TrackingPlus35MenuItem";
            this.TrackingPlus35MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingPlus35MenuItem.Text = "+35 pt";
            // 
            // TrackingPlus30MenuItem
            // 
            this.TrackingPlus30MenuItem.BackColor = System.Drawing.Color.Violet;
            this.TrackingPlus30MenuItem.Name = "TrackingPlus30MenuItem";
            this.TrackingPlus30MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingPlus30MenuItem.Text = "+30 pt";
            // 
            // ToolStripMenuItem9
            // 
            this.ToolStripMenuItem9.Name = "ToolStripMenuItem9";
            this.ToolStripMenuItem9.Size = new System.Drawing.Size(104, 6);
            // 
            // TrackingPlus25MenuItem
            // 
            this.TrackingPlus25MenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(104)))), ((int)(((byte)(238)))));
            this.TrackingPlus25MenuItem.Name = "TrackingPlus25MenuItem";
            this.TrackingPlus25MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingPlus25MenuItem.Text = "+25 pt";
            // 
            // TrackingPlus20MenuItem
            // 
            this.TrackingPlus20MenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(165)))), ((int)(((byte)(255)))));
            this.TrackingPlus20MenuItem.Name = "TrackingPlus20MenuItem";
            this.TrackingPlus20MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingPlus20MenuItem.Text = "+20 pt";
            // 
            // TrackingPlus15MenuItem
            // 
            this.TrackingPlus15MenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(143)))), ((int)(((byte)(224)))), ((int)(((byte)(226)))));
            this.TrackingPlus15MenuItem.Name = "TrackingPlus15MenuItem";
            this.TrackingPlus15MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingPlus15MenuItem.Text = "+15 pt";
            // 
            // TrackingPlus10MenuItem
            // 
            this.TrackingPlus10MenuItem.BackColor = System.Drawing.Color.Lime;
            this.TrackingPlus10MenuItem.Name = "TrackingPlus10MenuItem";
            this.TrackingPlus10MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingPlus10MenuItem.Text = "+10 pt";
            // 
            // TrackingPlus05MenuItem
            // 
            this.TrackingPlus05MenuItem.BackColor = System.Drawing.Color.GreenYellow;
            this.TrackingPlus05MenuItem.Name = "TrackingPlus05MenuItem";
            this.TrackingPlus05MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingPlus05MenuItem.Text = "+05 pt";
            // 
            // TrackingNoneMenuItem
            // 
            this.TrackingNoneMenuItem.BackColor = System.Drawing.Color.White;
            this.TrackingNoneMenuItem.Name = "TrackingNoneMenuItem";
            this.TrackingNoneMenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingNoneMenuItem.Text = "none";
            // 
            // TrackingMinus05MenuItem
            // 
            this.TrackingMinus05MenuItem.BackColor = System.Drawing.Color.Yellow;
            this.TrackingMinus05MenuItem.Name = "TrackingMinus05MenuItem";
            this.TrackingMinus05MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingMinus05MenuItem.Text = "-05 pt";
            // 
            // TrackingMinus10MenuItem
            // 
            this.TrackingMinus10MenuItem.BackColor = System.Drawing.Color.Gold;
            this.TrackingMinus10MenuItem.Name = "TrackingMinus10MenuItem";
            this.TrackingMinus10MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingMinus10MenuItem.Text = "-10 pt";
            // 
            // TrackingMinus15MenuItem
            // 
            this.TrackingMinus15MenuItem.BackColor = System.Drawing.Color.SandyBrown;
            this.TrackingMinus15MenuItem.Name = "TrackingMinus15MenuItem";
            this.TrackingMinus15MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingMinus15MenuItem.Text = "-15 pt";
            // 
            // TrackingMinus20MenuItem
            // 
            this.TrackingMinus20MenuItem.BackColor = System.Drawing.Color.Orange;
            this.TrackingMinus20MenuItem.Name = "TrackingMinus20MenuItem";
            this.TrackingMinus20MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingMinus20MenuItem.Text = "-20 pt";
            // 
            // TrackingMinus25MenuItem
            // 
            this.TrackingMinus25MenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.TrackingMinus25MenuItem.Name = "TrackingMinus25MenuItem";
            this.TrackingMinus25MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingMinus25MenuItem.Text = "-25 pt";
            // 
            // ToolStripMenuItem10
            // 
            this.ToolStripMenuItem10.Name = "ToolStripMenuItem10";
            this.ToolStripMenuItem10.Size = new System.Drawing.Size(104, 6);
            // 
            // TrackingMinus30MenuItem
            // 
            this.TrackingMinus30MenuItem.BackColor = System.Drawing.Color.Lavender;
            this.TrackingMinus30MenuItem.Name = "TrackingMinus30MenuItem";
            this.TrackingMinus30MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingMinus30MenuItem.Text = "-30 pt";
            // 
            // TrackingMinus35MenuItem
            // 
            this.TrackingMinus35MenuItem.BackColor = System.Drawing.Color.Gainsboro;
            this.TrackingMinus35MenuItem.Name = "TrackingMinus35MenuItem";
            this.TrackingMinus35MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingMinus35MenuItem.Text = "-35 pt";
            // 
            // TrackingMinus40MenuItem
            // 
            this.TrackingMinus40MenuItem.BackColor = System.Drawing.Color.Silver;
            this.TrackingMinus40MenuItem.Name = "TrackingMinus40MenuItem";
            this.TrackingMinus40MenuItem.Size = new System.Drawing.Size(107, 22);
            this.TrackingMinus40MenuItem.Text = "-40 pt";
            // 
            // HelpToolStripMenuItem
            // 
            this.HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LicenseToolStripMenuItem,
            this.ContentsToolStripMenuItem,
            this.AboutPrinceToolStripMenuItem});
            this.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
            this.HelpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.HelpToolStripMenuItem.Text = "Help";
            // 
            // LicenseToolStripMenuItem
            // 
            this.LicenseToolStripMenuItem.Name = "LicenseToolStripMenuItem";
            this.LicenseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.LicenseToolStripMenuItem.Text = "License";
            this.LicenseToolStripMenuItem.Click += new System.EventHandler(this.LicenseToolStripMenuItem_Click);
            // 
            // ContentsToolStripMenuItem
            // 
            this.ContentsToolStripMenuItem.Name = "ContentsToolStripMenuItem";
            this.ContentsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ContentsToolStripMenuItem.Text = "Contents";
            this.ContentsToolStripMenuItem.Click += new System.EventHandler(this.ContentsToolStripMenuItem_Click);
            // 
            // AboutPrinceToolStripMenuItem
            // 
            this.AboutPrinceToolStripMenuItem.Name = "AboutPrinceToolStripMenuItem";
            this.AboutPrinceToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.AboutPrinceToolStripMenuItem.Text = "About Princess";
            this.AboutPrinceToolStripMenuItem.Click += new System.EventHandler(this.AboutPrinceToolStripMenuItem_Click);
            // 
            // AxAcroPDF1
            // 
            this.AxAcroPDF1.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.AxAcroPDF1.Enabled = true;
            this.AxAcroPDF1.Location = new System.Drawing.Point(108, 50);
            this.AxAcroPDF1.Name = "AxAcroPDF1";
            this.AxAcroPDF1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("AxAcroPDF1.OcxState")));
            this.AxAcroPDF1.Size = new System.Drawing.Size(800, 647);
            this.AxAcroPDF1.TabIndex = 2;
            // 
            // OpenFileDialog1
            // 
            this.OpenFileDialog1.FileName = "OpenFileDialog1";
            // 
            // btnChangePictureSettings
            // 
            this.btnChangePictureSettings.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnChangePictureSettings.Location = new System.Drawing.Point(474, 65);
            this.btnChangePictureSettings.Name = "btnChangePictureSettings";
            this.btnChangePictureSettings.Size = new System.Drawing.Size(117, 23);
            this.btnChangePictureSettings.TabIndex = 13;
            this.btnChangePictureSettings.Text = "Picture settings";
            this.btnChangePictureSettings.UseVisualStyleBackColor = false;
            this.btnChangePictureSettings.Click += new System.EventHandler(this.btnChangePictureSettings_Click);
            // 
            // tbFeedback
            // 
            this.tbFeedback.BackColor = System.Drawing.Color.Yellow;
            this.tbFeedback.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFeedback.Location = new System.Drawing.Point(114, 49);
            this.tbFeedback.Multiline = true;
            this.tbFeedback.Name = "tbFeedback";
            this.tbFeedback.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tbFeedback.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbFeedback.Size = new System.Drawing.Size(800, 647);
            this.tbFeedback.TabIndex = 14;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(700, 54);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Close     X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripSeparator4,
            this.ToolStripSeparator5,
            this.tsbtnToggleToolbar,
            this.FullWidthToolStripButton,
            this.ToolStripSeparator3,
            this.tsbtnConvert,
            this.ToolStripSeparator1,
            this.tsbtnStart,
            this.tsbtnPrevious,
            this.tstbCurrentPageNumber,
            this.tsbtnCurrent,
            this.tsbtnNext,
            this.tsbtnEnd,
            this.ToolStripSeparator2});
            this.ToolStrip1.Location = new System.Drawing.Point(0, 24);
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Size = new System.Drawing.Size(1028, 25);
            this.ToolStrip1.TabIndex = 21;
            this.ToolStrip1.Text = "toggle keyboard shortcuts";
            // 
            // ToolStripSeparator4
            // 
            this.ToolStripSeparator4.Name = "ToolStripSeparator4";
            this.ToolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripSeparator5
            // 
            this.ToolStripSeparator5.Name = "ToolStripSeparator5";
            this.ToolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtnToggleToolbar
            // 
            this.tsbtnToggleToolbar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnToggleToolbar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnToggleToolbar.Name = "tsbtnToggleToolbar";
            this.tsbtnToggleToolbar.Size = new System.Drawing.Size(80, 22);
            this.tsbtnToggleToolbar.Text = "Toggle toolbar";
            // 
            // FullWidthToolStripButton
            // 
            this.FullWidthToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FullWidthToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FullWidthToolStripButton.Name = "FullWidthToolStripButton";
            this.FullWidthToolStripButton.Size = new System.Drawing.Size(56, 22);
            this.FullWidthToolStripButton.Text = "Full width";
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtnConvert
            // 
            this.tsbtnConvert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnConvert.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnConvert.Name = "tsbtnConvert";
            this.tsbtnConvert.Size = new System.Drawing.Size(50, 22);
            this.tsbtnConvert.Text = "Convert";
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtnStart
            // 
            this.tsbtnStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnStart.Name = "tsbtnStart";
            this.tsbtnStart.Size = new System.Drawing.Size(23, 22);
            this.tsbtnStart.Text = "Start";
            // 
            // tsbtnPrevious
            // 
            this.tsbtnPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnPrevious.Name = "tsbtnPrevious";
            this.tsbtnPrevious.Size = new System.Drawing.Size(23, 22);
            this.tsbtnPrevious.Text = "Previous";
            // 
            // tstbCurrentPageNumber
            // 
            this.tstbCurrentPageNumber.Name = "tstbCurrentPageNumber";
            this.tstbCurrentPageNumber.Size = new System.Drawing.Size(30, 25);
            this.tstbCurrentPageNumber.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tsbtnCurrent
            // 
            this.tsbtnCurrent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnCurrent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnCurrent.Name = "tsbtnCurrent";
            this.tsbtnCurrent.Size = new System.Drawing.Size(46, 22);
            this.tsbtnCurrent.Text = "current";
            this.tsbtnCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsbtnNext
            // 
            this.tsbtnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnNext.Name = "tsbtnNext";
            this.tsbtnNext.Size = new System.Drawing.Size(23, 22);
            this.tsbtnNext.Text = "Next";
            // 
            // tsbtnEnd
            // 
            this.tsbtnEnd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnEnd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnEnd.Name = "tsbtnEnd";
            this.tsbtnEnd.Size = new System.Drawing.Size(23, 22);
            this.tsbtnEnd.Text = "End";
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bWkrPrince
            // 
            this.bWkrPrince.WorkerReportsProgress = true;
            this.bWkrPrince.WorkerSupportsCancellation = true;
            // 
            // dgvFilesToProcess
            // 
            this.dgvFilesToProcess.AllowUserToAddRows = false;
            dataGridViewCellStyle37.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.dgvFilesToProcess.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle37;
            this.dgvFilesToProcess.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFilesToProcess.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewCellStyle38.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle38.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle38.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle38.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle38.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle38.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle38.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFilesToProcess.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle38;
            this.dgvFilesToProcess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle39.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle39.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle39.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle39.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle39.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle39.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle39.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilesToProcess.DefaultCellStyle = dataGridViewCellStyle39;
            this.dgvFilesToProcess.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvFilesToProcess.Location = new System.Drawing.Point(85, 82);
            this.dgvFilesToProcess.Name = "dgvFilesToProcess";
            dataGridViewCellStyle40.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle40.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle40.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle40.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle40.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle40.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle40.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFilesToProcess.RowHeadersDefaultCellStyle = dataGridViewCellStyle40;
            this.dgvFilesToProcess.Size = new System.Drawing.Size(400, 200);
            this.dgvFilesToProcess.TabIndex = 24;
            // 
            // OpenFileDialogXML
            // 
            this.OpenFileDialogXML.FileName = "OpenFileDialog2";
            this.OpenFileDialogXML.Multiselect = true;
            // 
            // TreeView1
            // 
            this.TreeView1.Font = new System.Drawing.Font("Arial Unicode MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TreeView1.FullRowSelect = true;
            this.TreeView1.Location = new System.Drawing.Point(797, 50);
            this.TreeView1.Name = "TreeView1";
            this.TreeView1.Size = new System.Drawing.Size(231, 644);
            this.TreeView1.TabIndex = 28;
            // 
            // btnAlternateSelector
            // 
            this.btnAlternateSelector.Location = new System.Drawing.Point(676, 25);
            this.btnAlternateSelector.Name = "btnAlternateSelector";
            this.btnAlternateSelector.Size = new System.Drawing.Size(98, 23);
            this.btnAlternateSelector.TabIndex = 29;
            this.btnAlternateSelector.Text = "Alternate selector";
            this.btnAlternateSelector.UseVisualStyleBackColor = true;
            // 
            // tbAlternateSelector
            // 
            this.tbAlternateSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAlternateSelector.Location = new System.Drawing.Point(806, 4);
            this.tbAlternateSelector.Name = "tbAlternateSelector";
            this.tbAlternateSelector.Size = new System.Drawing.Size(173, 20);
            this.tbAlternateSelector.TabIndex = 30;
            // 
            // tbURL
            // 
            this.tbURL.Location = new System.Drawing.Point(22, 69);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(370, 20);
            this.tbURL.TabIndex = 32;
            this.tbURL.Text = "http://www.";
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(19, 31);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(308, 13);
            this.lblURL.TabIndex = 31;
            this.lblURL.Text = "Please give the URL in the form of:  http://www.princexml.com/";
            // 
            // pnlURL
            // 
            this.pnlURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlURL.Controls.Add(this.OK_Button);
            this.pnlURL.Controls.Add(this.Cancel_Button);
            this.pnlURL.Controls.Add(this.tbURL);
            this.pnlURL.Controls.Add(this.lblURL);
            this.pnlURL.Location = new System.Drawing.Point(119, 93);
            this.pnlURL.Name = "pnlURL";
            this.pnlURL.Size = new System.Drawing.Size(422, 189);
            this.pnlURL.TabIndex = 33;
            // 
            // OK_Button
            // 
            this.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OK_Button.Location = new System.Drawing.Point(263, 150);
            this.OK_Button.Name = "OK_Button";
            this.OK_Button.Size = new System.Drawing.Size(67, 23);
            this.OK_Button.TabIndex = 33;
            this.OK_Button.Text = "OK";
            // 
            // Cancel_Button
            // 
            this.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_Button.Location = new System.Drawing.Point(336, 150);
            this.Cancel_Button.Name = "Cancel_Button";
            this.Cancel_Button.Size = new System.Drawing.Size(67, 23);
            this.Cancel_Button.TabIndex = 34;
            this.Cancel_Button.Text = "Cancel";
            // 
            // Panel1
            // 
            this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel1.Controls.Add(this.cbColorizeTracking);
            this.Panel1.Controls.Add(this.trackingPlus05);
            this.Panel1.Controls.Add(this.trackingMinus05);
            this.Panel1.Controls.Add(this.minus25);
            this.Panel1.Controls.Add(this.minus40);
            this.Panel1.Controls.Add(this.minus35);
            this.Panel1.Controls.Add(this.Button14);
            this.Panel1.Controls.Add(this.minus30);
            this.Panel1.Controls.Add(this.minus20);
            this.Panel1.Controls.Add(this.minus15);
            this.Panel1.Controls.Add(this.minus10);
            this.Panel1.Controls.Add(this.minus05);
            this.Panel1.Controls.Add(this.none);
            this.Panel1.Controls.Add(this.plus05);
            this.Panel1.Controls.Add(this.plus10);
            this.Panel1.Controls.Add(this.plus15);
            this.Panel1.Controls.Add(this.plus20);
            this.Panel1.Controls.Add(this.plus25);
            this.Panel1.Controls.Add(this.plus30);
            this.Panel1.Controls.Add(this.plus35);
            this.Panel1.Controls.Add(this.plus40);
            this.Panel1.Controls.Add(this.plus60);
            this.Panel1.Location = new System.Drawing.Point(2, 52);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(106, 645);
            this.Panel1.TabIndex = 34;
            // 
            // cbColorizeTracking
            // 
            this.cbColorizeTracking.AutoSize = true;
            this.cbColorizeTracking.Checked = true;
            this.cbColorizeTracking.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbColorizeTracking.Location = new System.Drawing.Point(4, 8);
            this.cbColorizeTracking.Name = "cbColorizeTracking";
            this.cbColorizeTracking.Size = new System.Drawing.Size(104, 17);
            this.cbColorizeTracking.TabIndex = 23;
            this.cbColorizeTracking.Text = "Colorize tracking";
            this.cbColorizeTracking.UseVisualStyleBackColor = true;
            // 
            // trackingPlus05
            // 
            this.trackingPlus05.BackColor = System.Drawing.Color.GreenYellow;
            this.trackingPlus05.Location = new System.Drawing.Point(1, 578);
            this.trackingPlus05.Name = "trackingPlus05";
            this.trackingPlus05.Size = new System.Drawing.Size(102, 29);
            this.trackingPlus05.TabIndex = 22;
            this.trackingPlus05.Text = "add 05     ALT +";
            this.trackingPlus05.UseVisualStyleBackColor = false;
            // 
            // trackingMinus05
            // 
            this.trackingMinus05.BackColor = System.Drawing.Color.Yellow;
            this.trackingMinus05.Location = new System.Drawing.Point(1, 607);
            this.trackingMinus05.Name = "trackingMinus05";
            this.trackingMinus05.Size = new System.Drawing.Size(102, 29);
            this.trackingMinus05.TabIndex = 21;
            this.trackingMinus05.Text = "minus 05   ALT -";
            this.trackingMinus05.UseVisualStyleBackColor = false;
            // 
            // minus25
            // 
            this.minus25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.minus25.Location = new System.Drawing.Point(1, 443);
            this.minus25.Name = "minus25";
            this.minus25.Size = new System.Drawing.Size(102, 29);
            this.minus25.TabIndex = 20;
            this.minus25.Text = "-25";
            this.minus25.UseVisualStyleBackColor = false;
            // 
            // minus40
            // 
            this.minus40.BackColor = System.Drawing.Color.Silver;
            this.minus40.Location = new System.Drawing.Point(1, 530);
            this.minus40.Name = "minus40";
            this.minus40.Size = new System.Drawing.Size(102, 29);
            this.minus40.TabIndex = 19;
            this.minus40.Text = "-40";
            this.minus40.UseVisualStyleBackColor = false;
            // 
            // minus35
            // 
            this.minus35.BackColor = System.Drawing.Color.Gainsboro;
            this.minus35.Location = new System.Drawing.Point(1, 501);
            this.minus35.Name = "minus35";
            this.minus35.Size = new System.Drawing.Size(102, 29);
            this.minus35.TabIndex = 17;
            this.minus35.Text = "-35";
            this.minus35.UseVisualStyleBackColor = false;
            // 
            // Button14
            // 
            this.Button14.BackColor = System.Drawing.Color.DarkViolet;
            this.Button14.Location = new System.Drawing.Point(14, 931);
            this.Button14.Name = "Button14";
            this.Button14.Size = new System.Drawing.Size(102, 29);
            this.Button14.TabIndex = 16;
            this.Button14.Text = "-25";
            this.Button14.UseVisualStyleBackColor = false;
            // 
            // minus30
            // 
            this.minus30.BackColor = System.Drawing.Color.Lavender;
            this.minus30.Location = new System.Drawing.Point(1, 472);
            this.minus30.Name = "minus30";
            this.minus30.Size = new System.Drawing.Size(102, 29);
            this.minus30.TabIndex = 15;
            this.minus30.Text = "-30";
            this.minus30.UseVisualStyleBackColor = false;
            // 
            // minus20
            // 
            this.minus20.BackColor = System.Drawing.Color.Orange;
            this.minus20.Location = new System.Drawing.Point(1, 414);
            this.minus20.Name = "minus20";
            this.minus20.Size = new System.Drawing.Size(102, 29);
            this.minus20.TabIndex = 14;
            this.minus20.Text = "-20";
            this.minus20.UseVisualStyleBackColor = false;
            // 
            // minus15
            // 
            this.minus15.BackColor = System.Drawing.Color.SandyBrown;
            this.minus15.Location = new System.Drawing.Point(1, 385);
            this.minus15.Name = "minus15";
            this.minus15.Size = new System.Drawing.Size(102, 29);
            this.minus15.TabIndex = 13;
            this.minus15.Text = "-15";
            this.minus15.UseVisualStyleBackColor = false;
            // 
            // minus10
            // 
            this.minus10.BackColor = System.Drawing.Color.Gold;
            this.minus10.Location = new System.Drawing.Point(1, 356);
            this.minus10.Name = "minus10";
            this.minus10.Size = new System.Drawing.Size(102, 29);
            this.minus10.TabIndex = 12;
            this.minus10.Text = "-10";
            this.minus10.UseVisualStyleBackColor = false;
            // 
            // minus05
            // 
            this.minus05.BackColor = System.Drawing.Color.Yellow;
            this.minus05.Location = new System.Drawing.Point(1, 327);
            this.minus05.Name = "minus05";
            this.minus05.Size = new System.Drawing.Size(102, 29);
            this.minus05.TabIndex = 11;
            this.minus05.Text = "-05";
            this.minus05.UseVisualStyleBackColor = false;
            // 
            // none
            // 
            this.none.BackColor = System.Drawing.Color.White;
            this.none.Location = new System.Drawing.Point(1, 298);
            this.none.Name = "none";
            this.none.Size = new System.Drawing.Size(102, 29);
            this.none.TabIndex = 10;
            this.none.Text = "none";
            this.none.UseVisualStyleBackColor = false;
            // 
            // plus05
            // 
            this.plus05.BackColor = System.Drawing.Color.GreenYellow;
            this.plus05.Location = new System.Drawing.Point(1, 269);
            this.plus05.Name = "plus05";
            this.plus05.Size = new System.Drawing.Size(102, 29);
            this.plus05.TabIndex = 9;
            this.plus05.Text = "+05";
            this.plus05.UseVisualStyleBackColor = false;
            // 
            // plus10
            // 
            this.plus10.BackColor = System.Drawing.Color.Lime;
            this.plus10.Location = new System.Drawing.Point(1, 240);
            this.plus10.Name = "plus10";
            this.plus10.Size = new System.Drawing.Size(102, 29);
            this.plus10.TabIndex = 8;
            this.plus10.Text = "+10";
            this.plus10.UseVisualStyleBackColor = false;
            // 
            // plus15
            // 
            this.plus15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(143)))), ((int)(((byte)(224)))), ((int)(((byte)(226)))));
            this.plus15.Location = new System.Drawing.Point(1, 211);
            this.plus15.Name = "plus15";
            this.plus15.Size = new System.Drawing.Size(102, 29);
            this.plus15.TabIndex = 7;
            this.plus15.Text = "+15";
            this.plus15.UseVisualStyleBackColor = false;
            // 
            // plus20
            // 
            this.plus20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(165)))), ((int)(((byte)(255)))));
            this.plus20.Location = new System.Drawing.Point(1, 182);
            this.plus20.Name = "plus20";
            this.plus20.Size = new System.Drawing.Size(102, 29);
            this.plus20.TabIndex = 6;
            this.plus20.Text = "+20";
            this.plus20.UseVisualStyleBackColor = false;
            // 
            // plus25
            // 
            this.plus25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(104)))), ((int)(((byte)(238)))));
            this.plus25.Location = new System.Drawing.Point(1, 153);
            this.plus25.Name = "plus25";
            this.plus25.Size = new System.Drawing.Size(102, 29);
            this.plus25.TabIndex = 5;
            this.plus25.Text = "+25";
            this.plus25.UseVisualStyleBackColor = false;
            // 
            // plus30
            // 
            this.plus30.BackColor = System.Drawing.Color.Violet;
            this.plus30.Location = new System.Drawing.Point(1, 124);
            this.plus30.Name = "plus30";
            this.plus30.Size = new System.Drawing.Size(102, 29);
            this.plus30.TabIndex = 4;
            this.plus30.Text = "+30";
            this.plus30.UseVisualStyleBackColor = false;
            // 
            // plus35
            // 
            this.plus35.BackColor = System.Drawing.Color.Magenta;
            this.plus35.Location = new System.Drawing.Point(1, 95);
            this.plus35.Name = "plus35";
            this.plus35.Size = new System.Drawing.Size(102, 29);
            this.plus35.TabIndex = 3;
            this.plus35.Text = "+35";
            this.plus35.UseVisualStyleBackColor = false;
            // 
            // plus40
            // 
            this.plus40.BackColor = System.Drawing.Color.DarkViolet;
            this.plus40.Location = new System.Drawing.Point(1, 66);
            this.plus40.Name = "plus40";
            this.plus40.Size = new System.Drawing.Size(102, 29);
            this.plus40.TabIndex = 1;
            this.plus40.Text = "+40";
            this.plus40.UseVisualStyleBackColor = false;
            // 
            // plus60
            // 
            this.plus60.BackColor = System.Drawing.Color.Red;
            this.plus60.Location = new System.Drawing.Point(1, 37);
            this.plus60.Name = "plus60";
            this.plus60.Size = new System.Drawing.Size(102, 29);
            this.plus60.TabIndex = 0;
            this.plus60.Text = "+60";
            this.plus60.UseVisualStyleBackColor = false;
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Location = new System.Drawing.Point(151, 88);
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(163, 23);
            this.ProgressBar1.TabIndex = 35;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 716);
            this.Controls.Add(this.ProgressBar1);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.pnlURL);
            this.Controls.Add(this.tbAlternateSelector);
            this.Controls.Add(this.btnAlternateSelector);
            this.Controls.Add(this.TreeView1);
            this.Controls.Add(this.ToolStrip1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnChangePictureSettings);
            this.Controls.Add(this.AxAcroPDF1);
            this.Controls.Add(this.MenuStrip1);
            this.Controls.Add(this.dgvFilesToProcess);
            this.Controls.Add(this.tbFeedback);
            this.MainMenuStrip = this.MenuStrip1;
            this.Name = "Main";
            this.Text = "MainMenu";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.main_keypress);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AxAcroPDF1)).EndInit();
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilesToProcess)).EndInit();
            this.pnlURL.ResumeLayout(false);
            this.pnlURL.PerformLayout();
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.MenuStrip MenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ViewToolStripMenuItem;
        internal AxAcroPDFLib.AxAcroPDF AxAcroPDF1;
        internal System.Windows.Forms.ToolStripMenuItem DocumentLocalToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem StylesheetToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem2;
        internal System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem DocumentSourceToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem3;
        internal System.Windows.Forms.ToolStripMenuItem StylesheetToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem4;
        internal System.Windows.Forms.ToolStripMenuItem PDFToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem5;
        internal System.Windows.Forms.ToolStripMenuItem INIToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem6;
        internal System.Windows.Forms.ToolStripMenuItem TrackingToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingPlus15MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingNoneMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingMinus10MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem PicturesToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ConfigurationToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem LogToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem HelpToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingMinus15MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingMinus20MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingMinus25MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingMinus30MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingMinus35MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingMinus40MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem FolderToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem SelectToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem CurrentToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem HistoryToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ContentsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem LicenseToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem AboutPrinceToolStripMenuItem;
        internal System.Windows.Forms.OpenFileDialog OpenFileDialog1;
        internal System.Windows.Forms.Button btnChangePictureSettings;
        internal System.Windows.Forms.ToolStripMenuItem SettingsToolStripMenuItem;
        internal System.Windows.Forms.TextBox tbFeedback;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.ToolStripMenuItem InsertHereToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem7;
        internal System.Windows.Forms.ToolStripMenuItem PrintToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem RemoveToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem RemoveAllToolStripMenuItem;
        internal System.Windows.Forms.ToolStrip ToolStrip1;
        internal System.Windows.Forms.ToolStripButton tsbtnPrevious;
        internal System.Windows.Forms.ToolStripButton tsbtnStart;
        internal System.Windows.Forms.ToolStripButton tsbtnCurrent;
        internal System.Windows.Forms.ToolStripButton tsbtnNext;
        internal System.Windows.Forms.ToolStripButton tsbtnEnd;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator1;
        internal System.Windows.Forms.ToolStripButton tsbtnToggleToolbar;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator4;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator5;
        internal System.Windows.Forms.ToolStripButton tsbtnConvert;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator2;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
        internal System.Windows.Forms.ToolStripMenuItem TrackingPlus20MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingPlus25MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingPlus10MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingPlus05MenuItem;
        internal System.ComponentModel.BackgroundWorker bWkrPrince;
        internal System.Windows.Forms.ToolStripMenuItem PageSetupToolStripMenuItem;
        internal System.Windows.Forms.PageSetupDialog PageSetupDialog1;
        internal System.Windows.Forms.DataGridView dgvFilesToProcess;
        internal System.Windows.Forms.ToolStripTextBox tstbCurrentPageNumber;
        internal System.Windows.Forms.ToolStripMenuItem DocumentMultipleToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem PDFDisplayToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingPlus40MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingPlus35MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingPlus30MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem TrackingMinus05MenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem8;
        internal System.Windows.Forms.ToolStripMenuItem DebuggingModeToolStripMenuItem;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem9;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem10;
        internal System.Windows.Forms.OpenFileDialog OpenFileDialogXML;
        internal System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog1;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem12;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem11;
        internal System.Windows.Forms.ToolStripSeparator ToolStripMenuItem13;
        internal System.Windows.Forms.ToolStripMenuItem GPSStylesheetToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem GPSStylesheetToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem GeneratedStylesheetToolStripMenuItem;
        internal System.Windows.Forms.TreeView TreeView1;
        internal System.Windows.Forms.Button btnAlternateSelector;
        internal System.Windows.Forms.TextBox tbAlternateSelector;
        internal System.Windows.Forms.ToolStripButton FullWidthToolStripButton;
        internal System.Windows.Forms.ToolStripMenuItem TrackingPlus60MenuItem;
        internal System.Windows.Forms.ToolStripMenuItem DocumentURLToolStripMenuItem;
        internal System.Windows.Forms.TextBox tbURL;
        internal System.Windows.Forms.Label lblURL;
        internal System.Windows.Forms.Panel pnlURL;
        internal System.Windows.Forms.Button OK_Button;
        internal System.Windows.Forms.Button Cancel_Button;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.Button plus60;
        internal System.Windows.Forms.Button plus40;
        internal System.Windows.Forms.Button plus35;
        internal System.Windows.Forms.Button minus40;
        internal System.Windows.Forms.Button minus35;
        internal System.Windows.Forms.Button Button14;
        internal System.Windows.Forms.Button minus30;
        internal System.Windows.Forms.Button minus20;
        internal System.Windows.Forms.Button minus15;
        internal System.Windows.Forms.Button minus10;
        internal System.Windows.Forms.Button minus05;
        internal System.Windows.Forms.Button none;
        internal System.Windows.Forms.Button plus05;
        internal System.Windows.Forms.Button plus10;
        internal System.Windows.Forms.Button plus15;
        internal System.Windows.Forms.Button plus20;
        internal System.Windows.Forms.Button plus25;
        internal System.Windows.Forms.Button plus30;
        internal System.Windows.Forms.Button minus25;
        internal System.Windows.Forms.Button trackingPlus05;
        internal System.Windows.Forms.Button trackingMinus05;
        internal System.Windows.Forms.CheckBox cbColorizeTracking;
        internal System.Windows.Forms.ProgressBar ProgressBar1;

    }
}