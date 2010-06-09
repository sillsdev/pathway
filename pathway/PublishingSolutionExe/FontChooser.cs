// --------------------------------------------------------------------------------------------
// <copyright file="FontChooser.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Font Choosing while taking the Backup the project
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Windows.Forms;

namespace SIL.DictionaryExpress
{
    public partial class FontChooser : Form
    {
        public string BackupPath;
        public FontChooser()
        {
            InitializeComponent();
        }

        public FontChooser(string ProjectPath)
        {
            InitializeComponent();
            this.ProjectPath = ProjectPath;

        }

        private readonly string ProjectPath;

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            BackupPath = folderBrowser.SelectedPath;
            txtBackupPath.Text = BackupPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if(txtBackupPath.Text.Length <= 0)
                return;

            string destPath = Path.Combine(ProjectPath, "fonts");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            string windowsFontPath = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.System))+ "\\fonts\\";
            for (int i = 0; i < ChkListBox.CheckedItems.Count; i++)
            {
                string selItem = ChkListBox.CheckedItems[i].ToString();
                if (File.Exists(Path.Combine(windowsFontPath, selItem)) && !File.Exists(Path.Combine(destPath, selItem)))
                {
                    File.Copy(Path.Combine(windowsFontPath, selItem), Path.Combine(destPath, selItem));
                }
            }
        }
    }
}
