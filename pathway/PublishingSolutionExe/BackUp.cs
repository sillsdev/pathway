// --------------------------------------------------------------------------------------------
// <copyright file="BackUp.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Backup of the Current Dictionary 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Windows.Forms;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public partial class BackUp : Form
    {
        public string BackupPath;
        public BackUp()
        {
            InitializeComponent();
        }

        private readonly string ProjectPath;
        private readonly string ProjectType;

        public BackUp(string ProjectPath, string ProjectType)
        {
            InitializeComponent();
            this.ProjectPath = ProjectPath;
            this.ProjectType = ProjectType;

        }



        private void BackUp_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);     // Form Controls
            Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
            Common.HelpProv.SetHelpKeyword(this, "Archiving.htm");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            BackupPath = folderBrowser.SelectedPath;
            txtBackupPath.Text = BackupPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                BackUpFontsinCSS();
            }
            catch { }
        }

        /// <summary>
        /// Method to copy the fonts which are used in the input files.
        /// </summary>
        private void BackUpFontsinCSS()
        {
            string destPath = Common.PathCombine(ProjectPath, "fonts");
            if (Directory.Exists(destPath))
            {
                DirectoryInfo di = new DirectoryInfo(destPath);
                Common.CleanDirectory(di);
            }
            Directory.CreateDirectory(destPath);

            string windowsFontPath = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.System)) + "\\fonts\\";
            for (int i = 0; i < ChkListBox.CheckedItems.Count; i++)
            {
                string selItem = ChkListBox.CheckedItems[i].ToString();
                if (File.Exists(Common.PathCombine(windowsFontPath, selItem)) && !File.Exists(Common.PathCombine(destPath, selItem)))
                {
                    File.Copy(Common.PathCombine(windowsFontPath, selItem), Common.PathCombine(destPath, selItem));
                }
            }
        }

        /// <summary>
        /// Method to copy the settings file(DictionaryStyleSettings.xml/ScriptureSettings.xml) 
        /// and User *.css files from the Alluser path.
        /// </summary>
        private void BackUpUserSettingFiles()
        {
            string destPath = Common.PathCombine(ProjectPath, "SettingFiles");
            if (Directory.Exists(destPath))
            {
                DirectoryInfo di = new DirectoryInfo(destPath);
                Common.CleanDirectory(di);
            }
            Directory.CreateDirectory(destPath);

            string sourcePath = Common.GetAllUserPath();
            string[] filePaths = Directory.GetFiles(Common.PathCombine(sourcePath, ProjectType));
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                if (fileName.IndexOf(".css") > 0 || fileName.IndexOf(".xml") > 0 || fileName.IndexOf(".xsd") > 0)
                {
                    File.Copy(filePath, Common.PathCombine(destPath, fileName));
                }
            }
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BackUp_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }

        /// <summary>
        /// To validate the given path is valid or not Valid
        /// </summary>
        private void ValidateDirectoryLocation()
        {
            var location = txtBackupPath.Text;
            var isLocation = Directory.Exists(location);
            Btn_BackUp.Enabled = isLocation;
        }

        private void txtBackupPath_TextChanged(object sender, EventArgs e)
        {
            ValidateDirectoryLocation();
        }


        private void Backup_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }
    }
}
