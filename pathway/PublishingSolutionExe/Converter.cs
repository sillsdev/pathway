// --------------------------------------------------------------------------------------------
// <copyright file="Converter.cs" from='2009' to='2009' company='SIL International'>
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
// Converter for Current Dictionary/Scripture
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using JWTools;
using System.Collections;
using SIL.Tool;
using SIL.Tool.Localization;

#endregion Using

namespace SIL.PublishingSolution
{
    #region Class Converter
    public partial class Converter : Form
    {
        #region private variable
        const string _projectExtension = ".de";
        string _oldFileName;
        string _newFileName;
        TreeNode _mySelectedNode;

        bool _cssEdited = false;
		private string _backendPath = Common.ProgInstall;
        PublicationInformation _projectInfo = new PublicationInformation();

        private string _currentTask = string.Empty;
        private Color _deSelectedColor = Color.LightSteelBlue;
        private Color _selectedColor = Color.FromArgb(255, 204, 102);
        private Color _borderColor = Color.Black;
        private Color _mouseOverColor = Color.Khaki;

        #endregion

        #region public variable
        public string ExportType;
        public string ProjectType;
        public string ProjectName;
        public bool ShowError;
        public bool ExcerptPreview;
        #endregion

        #region Constructor
        public Converter()
        {
            InitializeComponent();
        }
        #endregion

        #region Event(s)

        private void OpenOfficeConverter_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);     // Form Controls
            Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
            Common.HelpProv.SetHelpKeyword(this, "ProjectTree.htm");

            ScreenAdjustment();

            string showErr = _projectInfo.DEGetAttribute("Project", "ShowError");
            ShowError = (showErr == "False" ? false : true);

            if (Param.Value.ContainsKey(Param.InputType))
                Param.SetValue(Param.InputType, ProjectType);
            Param.LoadSettings();
            TaskAdjustment();
            _currentTask = Param.Value[Param.LastTask];
            if (_currentTask.Length == 0)
                _currentTask = "Final print";
            SetTask(_currentTask);
        }

        private void ScreenAdjustment()
        {
            webPreview.Dock = DockStyle.Fill;
            txtCSS.Dock = DockStyle.Fill;
            imagePreview.Dock = DockStyle.Fill;

            PanelTask.Dock = DockStyle.Fill;
            PanelFile.Dock = DockStyle.Fill;
            PanelShow(1);

            this.WindowState = FormWindowState.Maximized;

            webPreview.Visible = true;
            txtCSS.Visible = false;
            imagePreview.Visible = false;
        }

        /// <summary>
        /// 1 = Scripture, 2 = Roles, 3= Publishing Files
        /// </summary>
        /// <param name="index"></param>
        private void PanelShow(int index)
        {
            PanelTask.Visible = false;
            PanelFile.Visible = false;

            BtnScripture.BackColor = Color.LightSteelBlue;
            BtnPublishingFile.BackColor = Color.LightSteelBlue;
            if (index == 1)
            {
                PanelTask.Visible = true;
                BtnScripture.BackColor = Color.Orange;
            }
            else if (index == 3)
            {
                PanelFile.Visible = true;
                BtnPublishingFile.BackColor = Color.Orange;
            }
        }

        private void DictionaryExplorer_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    _newFileName = e.Label;
                    string folderPath = _projectInfo.FullPath.Substring(0, _projectInfo.FullPath.LastIndexOf(Path.DirectorySeparatorChar));
                    try
                    {
                        if (e.Node.Tag.ToString() == "F" || e.Node.Tag.ToString() == "FE")
                        {
                            string newName = Common.PathCombine(folderPath, _newFileName);
                            File.Move(Common.PathCombine(folderPath, _oldFileName), newName);
                            if (e.Node.ImageIndex == 7)
                            {
                                _projectInfo.DefaultCssFileWithPath = newName;
                            }
                            else if (Path.GetExtension(e.Node.Text) == ".xhtml")
                            {
                                _projectInfo.DefaultXhtmlFileWithPath = newName;
                            }
                        }
                        else
                        {
                            Directory.Move(_projectInfo.FullPath, Common.PathCombine(folderPath, _newFileName));
                        }
                    }
                    catch (Exception ex)  // Duplicate File/Folder Name handling
                    {
                        var msg = new[] { ex.Message };
                        LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                        e.CancelEdit = true;
                        return;
                    }

                    _projectInfo.XMLOperation(_mySelectedNode.Text, 'X', _newFileName, false, "Rename", "", true);


                }
                else
                {
                    /* Cancel the label edit action, inform the user, and 
                       place the node in edit mode again. */
                    e.CancelEdit = true;
                    LocDB.Message("errInvalidEntry", "Invalid Entry.\nThe label cannot be blank", null, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                    e.Node.BeginEdit();
                }
                this.DictionaryExplorer.LabelEdit = false;
            }
            this.DictionaryExplorer.LabelEdit = false;
        }

        private void DictionaryExplorer_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {

                if (_cssEdited)
                {
                    if (LocDB.Message("errFileEditedandWanttoSave", "The css file was edited - Do you want to Save it?", null, LocDB.MessageTypes.YN, LocDB.MessageDefault.First) == DialogResult.Yes)
                    {
                        SaveCSS();
                    }
                    _cssEdited = false;
                    CSSReadOnly(true);
                }
                else if (txtCSS.ReadOnly == false)
                {
                    CSSReadOnly(true);
                }

                _mySelectedNode = DictionaryExplorer.GetNodeAt(e.X, e.Y);
                if (_mySelectedNode == null)
                {
                    return;
                }

                // fullpath and Xpath Setting
                GetInfo();

                int[] menuShow = null;
                Common.FileType fileType = (Common.FileType)_mySelectedNode.Tag;
                switch (fileType)
                {
                    /* 0 - Add file
                     * 1 - Add folder
                     * 2 - Delete
                     * 3 - Rename
                     * 4 - Exclude in Project
                     * 5 - Include in Project
                     * 6 - Set as Default CSS
                     * 7 - Add Existing Folder
                     * 8 - Backup
                     * 9 - Open folder in Explorer
                     */

                    case Common.FileType.Project:   // Project Node
                        menuShow = new int[] { 0, 1, 7, 8, 9 };
                        break;

                    case Common.FileType.File: // File Node
                        string tempFile = _mySelectedNode.Text;
                        string ext = Path.GetExtension(tempFile).ToLower();
                        if (ext == ".css" || ext == ".xhtml" || ext == ".lift")
                        {
                            menuShow = new int[] { 2, 3, 4, 6 };
                        }
                        else
                        {
                            menuShow = new int[] { 2, 3, 4 };
                        }
                        break;

                    case Common.FileType.FileExcluded:  // File Excluded Node
                        menuShow = new int[] { 2, 3, 5 };
                        break;

                    case Common.FileType.Directory: // Directory Node
                        menuShow = new int[] { 0, 1, 2, 3, 4 };
                        break;
                    case Common.FileType.DirectoryExcluded: // Directory excluded Node
                        menuShow = new int[] { 0, 1, 2, 3, 5 };
                        break;

                }
                for (int i = 0; i < DictionaryExplorer.ContextMenuStrip.Items.Count; i++)
                {
                    DictionaryExplorer.ContextMenuStrip.Items[i].Visible = false;
                }

                foreach (int menuid in menuShow)
                {
                    DictionaryExplorer.ContextMenuStrip.Items[menuid].Visible = true;
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                return;
            }

        }

        /// <summary>
        /// Get project information
        /// </summary>
        private void GetInfo()
        {
            ArrayList extentionFileList = new ArrayList { ".jpg", ".jpeg", ".bmp", ".gif", ".ico", ".png" }; // Note: works
            // ".tif" - Note - Not able to show in webbrowser

            _projectInfo.SubPath = _mySelectedNode.FullPath;
            if (_projectInfo.SubPath.IndexOf(Path.DirectorySeparatorChar) >= 0)
            {
                string woProjectName_XPath = "";
                woProjectName_XPath = _projectInfo.SubPath.Substring(_projectInfo.SubPath.IndexOf(Path.DirectorySeparatorChar) + 1, _projectInfo.SubPath.Length - _projectInfo.SubPath.IndexOf(Path.DirectorySeparatorChar) - 1);
                _projectInfo.FullPath = Common.PathCombine(_projectInfo.DictionaryPath, woProjectName_XPath);
            }
            else
            {
                _projectInfo.FullPath = _projectInfo.DictionaryPath;
            }
            lblCaption.Text = "";
            txtCSS.Text = "";  // No preview
            string extension = Path.GetExtension(_mySelectedNode.Text).ToLower();


            if (extension == ".xhtml")
            {
                _projectInfo.DefaultXhtmlFileWithPath = _projectInfo.FullPath; //  xhtml
                _projectInfo.LinkedCSS = Common.GetLinkedCSS(_projectInfo.DefaultXhtmlFileWithPath);
            }
            else if (extentionFileList.Contains(extension))
            {
                if (File.Exists(_projectInfo.FullPath))
                {
                    try
                    {
                        imagePreview.Navigate(_projectInfo.FullPath);
                        imagePreview.Visible = true;
                        webPreview.Visible = false;
                        txtCSS.Visible = false;
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else if (extension == ".css")
            {
                if (File.Exists(_projectInfo.FullPath))
                {
                    txtCSS.Text = File.ReadAllText(_projectInfo.FullPath);
                    lblCaption.Text = Path.GetFileName(_projectInfo.FullPath);
                    txtCSS.Visible = true;
                    webPreview.Visible = false;
                    imagePreview.Visible = false;
                    return;
                }
            }
            else if (extension == ".xml")
            {
                _projectInfo.DefaultXhtmlFileWithPath = _projectInfo.FullPath; // xml for reversal 
            }
            webPreview.Visible = true;
            imagePreview.Visible = false;
            txtCSS.Visible = false;

        }

        private void removeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Remove_ExcludeNodes(true);
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Remove_ExcludeNodes(true);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Select the File Name (*.*) | *.*";
            openFile.ShowDialog();

            string filename = openFile.FileName;
            if (filename != "")
            {
                if (Path.GetExtension(filename) == ".xhtml")
                {
                    if (_projectInfo.FindFileTypeExist(DictionaryExplorer.Nodes[0], ".xhtml"))
                    {
                        LocDB.Message("errFileExistsRemoveManualy", "XHTML File Already Exist - Remove it Manually", null, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                        return;
                    }
                }
                else if (Path.GetExtension(filename) == ".css")
                {
                    filename = Common.MakeSingleCSS(filename, "");
                }

                if (_projectInfo.AddFileToXML(filename, "False", false, "", true, true))
                {
                    if (Path.GetExtension(filename) == ".xhtml")
                    {
                        _projectInfo.DefaultCssFileWithPath = Common.GetLinkedCSS(filename);
                        if (_projectInfo.DefaultCssFileWithPath != "")
                        {
                            _projectInfo.AddFileToXML(_projectInfo.DefaultCssFileWithPath, "False", false, "", true, true);
                        }
                        _projectInfo.AddImageFiles(filename, _projectInfo.ProjectName);
                    }
                }
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            int counter = 1;
            string folderName = Common.PathCombine(_projectInfo.FullPath, "NewFolder" + counter);
            try
            {
                while (Directory.Exists(folderName))
                {
                    counter++;
                    folderName = Common.PathCombine(_projectInfo.FullPath, "NewFolder" + counter);
                }

                _projectInfo.AddFolderToXML(folderName, "");

                _projectInfo.SortFolderFileTypes(_projectInfo.ProjectFileWithPath);


                _projectInfo.PopulateDicExplorer(DictionaryExplorer);
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                return;
            }

        }

        private void excludeInProjectToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Remove_ExcludeNodes(false);
        }

        private void includeInProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _projectInfo.XMLOperation(_mySelectedNode.Text, 'X', "False", false, "Include", "", true);
            _projectInfo.PopulateDicExplorer(DictionaryExplorer);
        }

        public void ShowAll(object sender, EventArgs e)
        {
            _projectInfo.HideFileInExplorer = !_projectInfo.HideFileInExplorer;
            _projectInfo.PopulateDicExplorer(DictionaryExplorer);
        }

        public void ShowCSS(object sender, EventArgs e)
        {
            _projectInfo._userRole = Param.UserRole;
            _projectInfo.PopulateDicExplorer(DictionaryExplorer);
        }

        private void Refresh_Click_1(object sender, EventArgs e)
        {
            DictionaryExplorer.Nodes[0].ExpandAll();
        }

        private void renameToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            if (_mySelectedNode != null)
            {
                DictionaryExplorer.SelectedNode = _mySelectedNode;
                _oldFileName = _mySelectedNode.Text;
                DictionaryExplorer.LabelEdit = true;
                if (!_mySelectedNode.IsEditing)
                {
                    _mySelectedNode.BeginEdit();
                }
            }
            else
            {
                LocDB.Message("errSelectFile", "Please Select the File", null, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
            }
        }

        private void setAsDefaultCSSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _projectInfo.XMLOperation(_mySelectedNode.Text, 'X', "False", false, "SetAsDefault", "", true);
            _projectInfo.PopulateDicExplorer(DictionaryExplorer);
        }

        private void copyFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var FB = new FolderBrowserDialog();
            if (FB.ShowDialog() == DialogResult.OK)
            {
                string sourceDirPath = FB.SelectedPath;
                var sourceDir = new DirectoryInfo(sourceDirPath);

                string desDirPath = Common.PathCombine(_projectInfo.DictionaryPath, sourceDir.Name);
                var destinationDir = new DirectoryInfo(desDirPath);
                _projectInfo.CopyDirectory(sourceDir, destinationDir, "");
                _projectInfo.PopulateDicExplorer(DictionaryExplorer);
            }
        }


        private void addExistingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var FB = new FolderBrowserDialog();
            if (FB.ShowDialog() == DialogResult.OK)
            {
                string _sourceDirPath = FB.SelectedPath;
                var sourceDir = new DirectoryInfo(_sourceDirPath);

                string destDirPath = Common.PathCombine(_projectInfo.DictionaryPath, sourceDir.Name);
                var destinationDir = new DirectoryInfo(destDirPath);
                _projectInfo.CopyDirectory(sourceDir, destinationDir, "");
                _projectInfo.PopulateDicExplorer(DictionaryExplorer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void backupToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var frmFont = new BackUp(_projectInfo.DictionaryPath, _projectInfo.ProjectInputType);
            CssTree cssTree = new CssTree();
            ArrayList fontList = cssTree.GetFontList(_projectInfo.DefaultCssFileWithPath);
            for (int i = 0; i < fontList.Count; i++)
            {
                frmFont.ChkListBox.Items.Add(fontList[i]);
                frmFont.ChkListBox.SetItemChecked(i, true);
            }
            if (frmFont.ShowDialog() == DialogResult.OK)
            {
                string backupPath = frmFont.BackupPath;
                try
                {
                    bool testing = Common.Testing;
                    Common.Testing = true;
                    backupPath = Common.GetNewFileName(backupPath, "zipBackup.zip");
                    var zip = new ZipFolder();
                    zip.CreateZip(_projectInfo.DictionaryPath, backupPath, 0); //call zip method
                    Common.Testing = testing;
                }
                catch
                {
                }
                var msg = new[] { backupPath };
                LocDB.Message("errFileBackUpTo", "File has been Backup to " + backupPath, msg, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
            }
        }
        #endregion

        #region private function
        /// <summary>
        /// Return the name of the stylesheet name in the link tag. Sets projectInfo.m_expCss as a side effect.
        /// </summary>
        /// <param name="xhtmlName">name of XHTML file to process</param>
        private string exportedCSS(string xhtmlName)
        {
            if (!File.Exists(xhtmlName))
            {
                return "";
            }

            StreamReader sr = File.OpenText(xhtmlName);
            if (!sr.EndOfStream)
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Match m = Regex.Match(s, "<link([^>]*)>", RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        Match m1 = Regex.Match(m.Groups[1].Value, "stylesheet", RegexOptions.IgnoreCase);
                        if (m1.Success)
                        {
                            Match m2 = Regex.Match(m.Groups[1].Value, "href=\"([^\"]*)", RegexOptions.IgnoreCase);
                            if (m2.Success)
                            {
                                sr.Dispose();
                                _projectInfo.LinkedCSS = m2.Groups[1].Value;
                                return _projectInfo.LinkedCSS;
                            }
                        }
                    }
                    Match m3 = Regex.Match(s, "</head>", RegexOptions.IgnoreCase);
                    if (m3.Success)
                    {
                        break;
                    }
                }
                sr.Dispose();
            }
            return "";
        }

        private bool ExistXHTMLCSSFile()
        {
            if (_projectInfo.DefaultXhtmlFileWithPath == null)
            {
                LocDB.Message("errCreateorOpenDict", "Please Create/Open Dictionary or Add XHTML File", null, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                return false;
            }
            if (_projectInfo.DefaultCssFileWithPath == null)
            {
                LocDB.Message("errSetDefaultCSS", "Please set default CSS file", null, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                return false;
            }
            return true;
        }
        private void LoadFile()
        {
            ArrayList cssFiles = new ArrayList();
            _projectInfo.ProjectFileWithPath = Common.PathCombine(_projectInfo.DictionaryPath, _projectInfo.ProjectName + _projectExtension);
            if (_projectInfo.ProjectMode == "new")
            {
                Directory.CreateDirectory(_projectInfo.DictionaryPath);
                _projectInfo.CreateProjectFile(DictionaryExplorer);
            }
            else
            {
                _projectInfo.OpenProjectFile(DictionaryExplorer);
            }
            lblProjectType.Text = _projectInfo.ProjectInputType;
            ProjectType = _projectInfo.ProjectInputType;
            this.Text = _projectInfo.ProjectName;
            _projectInfo.PopulateDicExplorer(DictionaryExplorer);
        }

        private void Remove_ExcludeNodes(bool Remove)
        {
            string removeOrDelete;
            if (_mySelectedNode == null)
            {
                LocDB.Message("errSelectFile", "Please Select the File", null, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                return;
            }

            if (_mySelectedNode.Text != _projectInfo.ProjectName)
            {
                Common.FileType fileType = (Common.FileType)_mySelectedNode.Tag;
                if (Remove)
                {
                    removeOrDelete = "Remove";
                    try
                    {
                        if (fileType == Common.FileType.Directory || fileType == Common.FileType.DirectoryExcluded)
                        {
                            var msg = new[] { _mySelectedNode.Text };
                            if (LocDB.Message("errContentsDelete", _mySelectedNode.Text + " and all its contents will be deleted permanently", msg, LocDB.MessageTypes.YN, LocDB.MessageDefault.First) == DialogResult.No)
                            {
                                return;
                            }

                            if (!Directory.Exists(_projectInfo.FullPath))
                            {
                                return;
                            }
                            if (Directory.Exists(_projectInfo.FullPath))
                            {
                                DirectoryInfo di = new DirectoryInfo(_projectInfo.FullPath);
                                Common.CleanDirectory(di);
                            }
                        }
                        else if (fileType == Common.FileType.File || fileType == Common.FileType.FileExcluded)
                        {
                            var msg = new[] { _mySelectedNode.Text.ToUpper() };
                            if (_mySelectedNode.ImageIndex == 7)
                            {
                                if (LocDB.Message("errRemoveDefaultCSS", _mySelectedNode.Text.ToUpper() + " is the default CSS file for this Project \n Do You want to remove anyway?", msg, LocDB.MessageTypes.YN, LocDB.MessageDefault.First) == DialogResult.No)
                                {
                                    return;
                                }
                                _projectInfo.DefaultCssFileWithPath = null;
                            }
                            else if (Path.GetExtension(_mySelectedNode.Text) == ".xhtml")
                            {
                                if (LocDB.Message("errRemoveDefaultXHTML", _mySelectedNode.Text.ToUpper() + " is the default XHTML file for this Project \n Do You want to remove anyway?", msg, LocDB.MessageTypes.YN, LocDB.MessageDefault.First) == DialogResult.No)
                                {
                                    return;
                                }
                                _projectInfo.DefaultXhtmlFileWithPath = null;
                            }
                            else if (LocDB.Message("errDeletePermanent", _mySelectedNode.Text + " will be deleted permanently", msg, LocDB.MessageTypes.YN, LocDB.MessageDefault.First) == DialogResult.No)
                            {
                                return;
                            }

                            if (File.Exists(_projectInfo.FullPath))
                            {
                                File.Delete(_projectInfo.FullPath);
                                txtCSS.Text = "";
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                else
                {
                    if (_mySelectedNode.Tag.ToString() == "F" || _mySelectedNode.Tag.ToString() == "FE")
                    {
                        var msg = new[] { _mySelectedNode.Text.ToUpper() };
                        if (_mySelectedNode.ImageIndex == 7)
                        {
                            if (LocDB.Message("errRemoveDefaultCSS", _mySelectedNode.Text.ToUpper() + " is the default CSS file for this Project \n Do You want to exclude anyway?", msg, LocDB.MessageTypes.YN, LocDB.MessageDefault.First) == DialogResult.No)
                            {
                                return;
                            }
                            _projectInfo.DefaultCssFileWithPath = null;
                        }
                        else if (Path.GetExtension(_mySelectedNode.Text) == ".xhtml")
                        {
                            if (LocDB.Message("errExcludeDefaultXHTML", _mySelectedNode.Text.ToUpper() + " is the default XHTML file for this Project \n Do You want to exclude anyway?", msg, LocDB.MessageTypes.YN, LocDB.MessageDefault.First) == DialogResult.No)
                            {
                                return;
                            }
                            _projectInfo.DefaultXhtmlFileWithPath = null;
                        }
                    }
                    removeOrDelete = "Exclude";
                }

                DictionaryExplorer.Nodes.Remove(_mySelectedNode);
                _projectInfo.XMLOperation(_mySelectedNode.Text, 'X', "False", false, removeOrDelete, "", true);
            }
        }
        #endregion

        #region public function
        /// <summary>
        /// 
        /// </summary>
        public void DictionarySetting()
        {
            if (ValidateInputFiles())
            {
                return;
            }
            var ds = new DictionarySetting(_projectInfo.DictionaryPath, _projectInfo.DefaultCssFileWithPath, false, _projectInfo.DefaultXhtmlFileWithPath, _projectInfo.ProjectFileWithPath);
            var dlgResult = ds.ShowDialog();
            _projectInfo.PopulateDicExplorer(DictionaryExplorer);
        }

        /// <summary>
        /// To validate Input Files(XHTML and CSS) are given.
        /// </summary>
        /// <returns>true when invalid else returns false</returns>
        private bool ValidateInputFiles()
        {
            bool invalidFile = false;
            if (_projectInfo.DefaultXhtmlFileWithPath == null)
            {
                LocDB.Message("errSelectValidXHTML", "Please select the valid XHTML file", null, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                invalidFile = true;
            }
            else if (_projectInfo.DefaultCssFileWithPath == null)
            {
                if (LocDB.Message("errNoDefaultCSSApplyTemplates", "No default css found, Do you want to apply styles from Template?", null, LocDB.MessageTypes.YN, LocDB.MessageDefault.First) == DialogResult.Yes)
                {
                    ShowTemplate(null, null);
                }
                invalidFile = true;
            }
            return invalidFile;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ScriptureSetting()
        {
            if (ValidateInputFiles())
            {
                return;
            }
            var ds = new ScriptureSetting(_projectInfo.DictionaryPath, _projectInfo.DefaultCssFileWithPath, false, _projectInfo.DefaultXhtmlFileWithPath, _projectInfo.ProjectFileWithPath);
            var dlgResult = ds.ShowDialog();
            _projectInfo.PopulateDicExplorer(DictionaryExplorer);
        }

        /// <summary>
        /// Call TaskPick form
        /// </summary>
        public void TaskPick()
        {
            webPreview.Dispose();
            txtCSS.Visible = true;
            Param.SetValue(Param.InputPath, _projectInfo.DictionaryPath);
            Param.SetValue(Param.CurrentInput, _projectInfo.DefaultXhtmlFileWithPath);
            Param.SetValue(Param.InputType, _projectInfo.ProjectInputType);
            var dlg = new ConfigureTasks { Task = _currentTask, ProjectName = _projectInfo.ProjectName };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _currentTask = dlg.Task;

            }
            TaskAdjustment();
            SetTask(_currentTask);
            if (Param.Value[Param.LastTask] != _currentTask)
            {
                Param.SetValue(Param.LastTask, _currentTask);
                Param.Write();
            }

            var sheet = Param.TaskSheet(_currentTask);
            string cssFile = Param.GetAttrByName("styles/paper/style", sheet, "file");
            if (cssFile == null)
            {
                return;
            }

            string cssFileName = Path.GetFileName(_projectInfo.DefaultCssFileWithPath);

            _projectInfo.AddFileToXML(Param.StylePath(cssFile), "True", true, "", false, true);
            var featureSheet = new FeatureSheet(cssFile);
            if (featureSheet.ReadToEnd())
                foreach (var feature in featureSheet.Features)
                    _projectInfo.AddFileToXML(Param.StylePath(feature), "False", true, "", false, false);
            _projectInfo.PopulateDicExplorer(DictionaryExplorer);

            string insertChar = "@import \"" + cssFileName + "\";";
            string newCSSFile = Common.PathCombine(_projectInfo.DictionaryPath, Path.GetFileName(cssFile));
            Common.FileInsertText(newCSSFile, insertChar);
            SetTask(_currentTask);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cssFile"></param>
        public void CopyImportCSS(string cssFile)
        {
            var allCSSFile = new ArrayList();
            string BaseCSSFile = cssFile;
            allCSSFile = Common.GetCSSFileNames(cssFile, BaseCSSFile);
            foreach (string file in allCSSFile)
            {
                string projectFolder = Common.PathCombine(_projectInfo.DictionaryPath, Path.GetFileName(file));
                File.Copy(file, projectFolder, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusProgressBar"></param>
        public void Export(ProgressBar statusProgressBar)
        {
            var cssTree = new CssParser();

            Common.Testing = false;
            _projectInfo.DictionaryOutputName = null;
            _projectInfo.ProgressBar = statusProgressBar;

            if (ValidateInputFiles())
            {
                return;
            }
            VerboseClass verboseClass = VerboseClass.GetInstance();
            if (verboseClass.ShowError)
            {
                ShowErrorCSSFile(cssTree);
                if (verboseClass.ErrorCount > 0)
                {

                }
            }

            if (_backendPath.Length == 0)
            {
                _backendPath = Common.GetPSApplicationPath();
            }

            Backend.Load(_backendPath);

            ExportDlg objExportDlg = new ExportDlg { ExportType = _projectInfo.ProjectInputType };
            if (objExportDlg.ShowDialog() == DialogResult.OK)
            {
                ExportType = objExportDlg.ExportType;
            }
            else
            {
                return;
            }

            try
            {
                Common.ShowMessage = true;
                _projectInfo.DictionaryOutputName = _projectInfo.ProjectName;
                _projectInfo.IsOpenOutput = true;
                Backend.Launch(ExportType, _projectInfo);
                _projectInfo.PopulateDicExplorer(DictionaryExplorer);

            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == 1155)
                {
                    var msg = new[] { "LibreOffice application from http://www.libreoffice.org site.\nAfter downloading and installing Libre Office, please consult release notes about how to change the macro security setting to enable the macro that creates the headers." };
                    LocDB.Message("errInstallFile", "Please install " + msg, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                    return;
                }
            }

            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                return;
            }
        }

        private void ShowErrorCSSFile(CssParser cssTree)
        {
            cssTree.GetErrorReport(_projectInfo.DefaultCssFileWithPath);
            //To show errors to user to edit and save the CSS file.
            if (cssTree.ErrorList.Count > 0)
            {
                var errForm = new CSSError(cssTree.ErrorList, Path.GetDirectoryName(_projectInfo.DefaultCssFileWithPath));
                errForm.ShowDialog();
                cssTree = new CssParser();
                TreeNode node = cssTree.BuildTree(_projectInfo.DefaultCssFileWithPath);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readOnly"></param>
        public void CSSReadOnly(bool readOnly)
        {
            txtCSS.ReadOnly = readOnly;
            if (readOnly) // disable edit
            {
                txtCSS.BackColor = Color.White;
            }
            else
            {
                txtCSS.BackColor = Color.LightYellow;  // for edit
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveCSS()
        {
            if (_cssEdited)
            {
                File.WriteAllText(_projectInfo.FullPath, txtCSS.Text);
            }
            CSSReadOnly(true);
            _cssEdited = false;
        }

        /// <summary>
        /// To load file in New Mode
        /// </summary>
        /// <param name="count">Count</param>
        /// <returns></returns>
        public string New(int count)
        {
            string projectName = null;
            NewPublication objNewDlg = new NewPublication();
            objNewDlg.ShowDialog();
            try
            {
                if (objNewDlg.Success)
                {
                    _projectInfo.TemplateCSS = objNewDlg.CSSTemplate; // Only set if a template is chosen
                    _projectInfo.DefaultXhtmlFileWithPath = objNewDlg.XhtmlFile;
                    _projectInfo.ProjectName = objNewDlg.ProjectName;
                    _projectInfo.DictionaryPath = Common.PathCombine(objNewDlg.DicPath, _projectInfo.ProjectName);
                    _projectInfo.ProjectMode = "new";
                    LoadFile();
                    this.Name = objNewDlg.ProjectName;
                    projectName = objNewDlg.ProjectName;
                }
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
            }
            return projectName;
        }

        /// <summary>
        /// To Open the Project
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            bool success = false;
            OpenFileDialog dlgOPen = new OpenFileDialog();
            dlgOPen.Filter = "Please Select the Project (*" + _projectExtension + ")" + "|*" + _projectExtension;
            if (dlgOPen.ShowDialog() != DialogResult.OK)
            {
                return success;
            }
            _projectInfo.DictionaryPath = Path.GetDirectoryName(dlgOPen.FileName);
            _projectInfo.ProjectName = Path.GetFileName(dlgOPen.FileName);
            ProjectName = _projectInfo.ProjectName;
            _projectInfo.ProjectName = _projectInfo.ProjectName.Replace(_projectExtension, string.Empty);
            _projectInfo.ProjectMode = "open";
            LoadFile();
            success = true;
            return success;
        }
        /// <summary>
        /// Filter for Entry and Sense 
        /// </summary>
        public void FilterForEntrySense(string entryOrSense)
        {
            EntrySenseFilter entrySenseFilter = new EntrySenseFilter(_projectInfo,entryOrSense);
            if (entrySenseFilter.ShowDialog() == DialogResult.OK)
            {
                bool filter = true;
                if (entrySenseFilter.FilterKey.ToLower() == "none")
                {
                    filter = false;
                }
                if (entryOrSense.ToLower() == "entry")
                {
                    _projectInfo.IsEntryFilter = filter;
                    _projectInfo.EntryFilterKey = entrySenseFilter.FilterKey;
                    _projectInfo.EntryFilterString = entrySenseFilter.FilterString;
                    _projectInfo.IsEntryFilterMatchCase = entrySenseFilter.IsFilterMatchCase;
                }
                else if (entryOrSense.ToLower() == "sense")
                {
                    _projectInfo.IsSenseFilter = filter;
                    _projectInfo.SenseFilterKey = entrySenseFilter.FilterKey;
                    _projectInfo.SenseFilterString = entrySenseFilter.FilterString;
                    _projectInfo.IsSenseFilterMatchCase = entrySenseFilter.IsFilterMatchCase;
                }
                else
                {
                    _projectInfo.IsLanguageFilter = filter;
                    _projectInfo.LanguageFilterKey = entrySenseFilter.FilterKey;
                    _projectInfo.LanguageFilterString = entrySenseFilter.FilterString;
                    _projectInfo.IsLanguageFilterMatchCase = entrySenseFilter.IsFilterMatchCase;
                }
            }
        }

        /// <summary>
        /// For Lift Language Sort
        /// </summary>
        /// <param name="sort">True/False</param>
        public void LanguageSort(bool sort)
        {
            _projectInfo.IsLanguageSort = sort;
        }

        /// <summary>
        /// For Lift Entry Sort
        /// </summary>
        /// <param name="sort">True/False</param>
        public void EntrySort(bool sort)
        {
            _projectInfo.IsEntrySort = sort;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="excerptPreview"></param>
        public void ShowPreview(object sender, EventArgs e, bool excerptPreview)
        {
            if (!ExistXHTMLCSSFile())
            {
                return;
            }
            try
            {
                webPreview.Dispose();
                webPreview = new WebBrowser();
                this.webPreview.Location = new Point(83, 120);
                this.webPreview.TabIndex = 40;
                this.webPreview.AccessibleName = "webPreview";
                webPreview.Dock = DockStyle.Fill;
                this.panel3.Controls.Add(this.webPreview);
                string mergedCSS = string.Empty;
                string outputFileName = Common.PathCombine(Path.GetDirectoryName(_projectInfo.DefaultXhtmlFileWithPath), "Pdfpreview.pdf");
                if (File.Exists(_projectInfo.DefaultCssFileWithPath))
                    mergedCSS = Common.MakeSingleCSS(_projectInfo.DefaultCssFileWithPath, "");
                string xhtmlPreviewFilePath = Preview.CreatePreviewFile(_projectInfo.DefaultXhtmlFileWithPath, mergedCSS, "preview", excerptPreview);
                Pdf pdf = new Pdf(xhtmlPreviewFilePath, mergedCSS);
                pdf.Create(outputFileName);
                if (!File.Exists(outputFileName))
                {
                    outputFileName = xhtmlPreviewFilePath;
                }
                var uri = "file:///" + outputFileName.Replace(@"\", "/").Replace(":", "|");
                webPreview.Navigate(uri);
                lblCaption.Text = ProjectType + " Preview";
                txtCSS.Visible = false;
                webPreview.Visible = true;
            }
            catch (Exception ex)
            {
                var msg = new[] { ex.Message };
                LocDB.Message("defErrMsg", ex.Message, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                return;
            }
        }

        /// <summary>
        /// Create error log file
        /// </summary>
        /// <param name="showError"> Boolean</param>
        public void ShowErrorLog(bool showError)
        {
            string showErr = showError ? "True" : "False";
            _projectInfo.DESetAttribute("//Project", "ShowError", showErr);
        }

        /// <summary>
        /// To show the Template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowTemplate(object sender, EventArgs e)
        {
            var frmTmlt = new Template(ProjectType);
            var dlgResult = frmTmlt.ShowDialog();
            string cssTemplate;
            if (dlgResult == DialogResult.OK)
            {
                if (frmTmlt.CssTemplate != null)
                {
                    cssTemplate = frmTmlt.CssTemplate;
                    _projectInfo.RemoveFile(cssTemplate);
                    _projectInfo.AddFileToXML(cssTemplate, "True", true, "", true, true);
                }
                ShowPreview(sender, e, ((frmMDIParent)MdiParent).ExcerptPreview());
            }
        }

        #endregion

        private void openFolderInWindowsExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", _projectInfo.DictionaryPath);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }

        private void Converter_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void txtCSS_TextChanged(object sender, EventArgs e)
        {
            if (txtCSS.ReadOnly == false)
            {
                _cssEdited = true;
            }
        }

        private void Converter_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            _projectInfo.HideFileInExplorer = !_projectInfo.HideFileInExplorer;
            _projectInfo.PopulateDicExplorer(DictionaryExplorer);
        }

        private void BtnScripture_Click(object sender, EventArgs e)
        {
            PanelShow(1);
        }

        private void BtnRole_Click(object sender, EventArgs e)
        {
            PanelShow(2);
        }

        private void BtnPublishingFile_Click(object sender, EventArgs e)
        {
            PanelShow(3);
        }

        private void ShowPreivew(string type)
        {
            if (string.IsNullOrEmpty(_projectInfo.DefaultXhtmlFileWithPath)) return;

            webPreview.Dispose();
            webPreview = new WebBrowser();
            this.webPreview.Location = new Point(83, 120);
            this.webPreview.TabIndex = 40;
            this.webPreview.AccessibleName = "webPreview";
            webPreview.Dock = DockStyle.Fill;
            this.panel3.Controls.Add(this.webPreview);

            Param.SetValue(Param.CurrentInput, _projectInfo.DefaultXhtmlFileWithPath);
            var preview = new Preview { Sheet = Param.TaskSheet(type), ParentForm = this };
            preview.DefaultCSS = _projectInfo.DefaultCssFileWithPath;
            string previewPdfFile = preview.CreatePreview();
            if (previewPdfFile != string.Empty)
            {
                webPreview.Navigate(previewPdfFile);
                webPreview.Visible = true;
                txtCSS.Visible = false;
            }
        }

        private void webPreview_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }


        private void TaskAdjustment()
        {
            PanelTask.Controls.Clear();

            //Task 
            List<string> tasks = Param.GetListofAttr("tasks/task", "name");
            List<string> taskIcon = Param.GetListofAttr("tasks/task", "icon");
            ImageList imageListTask = new ImageList { ImageSize = new Size(32, 32) };
            try
            {
                foreach (var iconName in taskIcon)
                {
					var icon = new Bitmap(Common.FromRegistry(iconName));
                    imageListTask.Images.Add(icon);
                }
            }
            catch
            {
            }

            int height = 58;
            int locationY = 36;
            for (int i = 0; i < tasks.Count; i++)
            {
                Button button = new Button()
                {
                    Name = tasks[i],
                    Text = tasks[i],
                    Image = imageListTask.Images[i]
                };
                button.Size = new Size(171, height);
                button.Location = new Point(0, locationY);
                button.ImageAlign = ContentAlignment.TopCenter;
                button.TextAlign = ContentAlignment.BottomCenter;


                button.FlatAppearance.BorderColor = _borderColor;
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.CheckedBackColor = _selectedColor;
                button.FlatAppearance.MouseDownBackColor = _selectedColor;
                button.FlatAppearance.MouseOverBackColor = _mouseOverColor;
                button.FlatStyle = FlatStyle.Flat;
                button.UseVisualStyleBackColor = true;

                locationY += height;
                button.Click += this.Task_Click;
                button.MouseEnter += this.Task_MouseEnter;
                button.MouseLeave += this.Task_MouseLeave;
                PanelTask.Controls.Add(button);
            }
        }
        private void Task_MouseEnter(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            SetDescription(bt.Text);
        }


        private void Task_MouseLeave(object sender, EventArgs e)
        {
            SetDescription(_currentTask);
        }
        private void Task_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            SetTask(bt.Text);
        }

        private void SetTask(string selectedTask)
        {
            foreach (Control control in PanelTask.Controls)
            {
                if (control is Button)
                {
                    Button task = (Button)control;
                    if (task.Text == selectedTask)
                    {
                        task.BackColor = _selectedColor;
                    }
                    else
                    {
                        task.BackColor = _deSelectedColor;
                    }
                }
            }
            _currentTask = selectedTask;
            ShowPreivew(_currentTask);
        }

        private void SetDescription(string focus)
        {
            var sheet = Param.GetAttrByName("tasks/task", focus, "style");
            lblTaskDesc.Text = Param.GetElemByName("styles/paper/style", sheet, "Description");
            lblTaskDesc.Visible = true;
        }
    }
    #endregion
}

    #region Class FlexString
    /// <summary>
    /// This Method is used to avoid the regex function in XSLT
    /// Sense Number returns the sense, homograph number, sense number.
    /// <returns>Mathched Pattern Value</returns>        
    /// </summary>
    public class FlexString
    {
        #region Public Function
        public string SenseNumber(string input, string checkMethod)
        {
            string returnValue = "";
            Match test;

            if (checkMethod == "sense")
            {
                //Mathches the String.
                test = Regex.Match(input, "([a-zA-Z]+)");
                returnValue = test.Groups[0].ToString();
            }
            else if (checkMethod == "homograph")  // homograph
            {
                test = Regex.Match(input, "([0-9])"); // without space - xhomographNumber
                returnValue = test.Groups[0].ToString();
            }
            else if (checkMethod == "sensenumber")
            {
                test = Regex.Match(input, "( [0-9]+)");   // with space - xsensenumber
                returnValue = test.Groups[0].ToString();
            }
            return returnValue;
        }
        #endregion Public Function
    }
    #endregion Class FlexString

