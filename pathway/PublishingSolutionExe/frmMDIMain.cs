// --------------------------------------------------------------------------------------------
// <copyright file="frmMDIMain.cs" from='2009' to='2009' company='SIL International'>
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
// The application form.
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using JWTools;
using System.IO;
using System.Xml;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    #region public partial class frmMDIParent
    /// <summary>
    /// The application form.
    /// </summary>
    public partial class frmMDIParent : Form
    {
        #region Private Variable
        /// <summary>
        /// Create unique default dictionary folder names by adding a count
        /// </summary>
        int _count;

        /// <summary>
        /// Base path for help file
        /// </summary>
        readonly string _hlpPath = Common.PathCombine(Environment.CurrentDirectory, "Help");
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the frmMDIParent class.
        /// </summary>
        public frmMDIParent()
        {
            InitializeComponent();
           
        }
        #endregion

        #region Form Load
        /// <summary>
        /// Form Load (localize controls)
        /// </summary>
        /// <param name="sender">object sending event (unused)</param>
        /// <param name="e">arguments of event (unused)</param>
        private void frmMDIParent_Load(object sender, EventArgs e)
        {
            Common.fromPlugin = false;
            ExcerptPreview_Click(sender,e);
            try
            {
                SettingsValidation(Param.SettingPath);
                Param.LoadSettings();
                RemoveSettingsFile();
            }
            catch (InvalidStyleSettingsException err)
            {
                MessageBox.Show(string.Format(err.ToString(), err.FullFilePath), "Pathway", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            Param.SetFontNameSize(); // Global Font Name and Size for all UI Forms

            if (Param.Value.Count > 0)
            {
//                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
//                {
//                    //this.Icon = new Icon("Graphic/BOOK.ico");
//                }
//                else
//                {
//                    using (MemoryStream memoryStream = new MemoryStream())
//                    {
////                        var image = Image.FromFile("Graphic/book.png");
////                        image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Icon);
////                        memoryStream.Position = 0;
////                        this.Icon = new Icon(memoryStream);
//                    }
//                }
                JW_Registry.RootKey = @"SOFTWARE\The Seed Company\Dictionary Express!";
                LocDB.SetAppTitle();
                LocDB.BaseName = "PsLocalization.xml";
                var folderPath = Param.Value[Param.OutputPath];
                var localizationPath = Common.PathCombine(folderPath, "Loc");
                if (!Directory.Exists(localizationPath))
                {
                    Directory.CreateDirectory(localizationPath);
                    File.Copy(@"Loc/" + LocDB.BaseName, Common.PathCombine(localizationPath, LocDB.BaseName));
                }
                LocDB.Initialize(folderPath);
                LocDB.Localize(this, null); // Form Controls
                LocDB.Localize(menuMain); // Menu Controls
                LocDB.Localize(toolStripMain); // Toolstrip controls
            }

            SetRole();

            bool isChildFormExist = false;
            UpdateMenu(isChildFormExist);
            try
            {
                string helpPath = Common.PathCombine(_hlpPath, "PsDoc.chm");
                Common.HelpProv.HelpNamespace = helpPath;
            }
            catch
            {
            }
        }

        private void SettingsValidation(string filePath)
        {
            var versionControl = new SettingsVersionControl();
            var Validator = new SettingsValidator();
            if (File.Exists(filePath))
            {
                versionControl.UpdateSettingsFile(filePath);
                bool isValid = Validator.ValidateSettingsFile(filePath, false);
                if (!isValid)
                {
                    this.Close();
                }
            }
        }

        private void SetRole()
        {
            List<string> roles = Param.GetListofAttr("roles/role", "name");
            List<string> roleIcon = Param.GetListofAttr("roles/role", "icon");
            string currentRole = Param.UserRole; //.GetRole();

            if (currentRole.Length == 0)
                currentRole = "Output User";
            
            if (roles.Count <= 0 && roleIcon.Count <= 0)
            {
                roleIcon.Add("Graphic/user.png");
                roles.Add(currentRole);
            }
            StatusRole.Text = currentRole;
            Param.UserRole = currentRole;
            try
            {
                // Image List
                ImageList imageList = new ImageList { ImageSize = new Size(32, 32) };
                foreach (var iconName in roleIcon)
                {
					var icon = new Bitmap(Common.FromRegistry(iconName));
                    imageList.Images.Add(icon);
                }
            }
            catch
            {
            } 
            menuRole.DropDownItems.Clear();
            for (int i = 0; i < roleIcon.Count; i++)
            {
                ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(roles[i])
                                                          {
                                                              Name = roles[i],
                                                              //Image = imageList.Images[i]
                                                          };
                toolStripMenuItem.Click += this.MenuRoleChild_Click;
                menuRole.DropDownItems.Add(toolStripMenuItem);
            }
            SetRoleChecked(currentRole);
        }

        private void RemoveSettingsFile()
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                string[] projType = new[] {"Dictionary", "Scripture"};
                foreach (string pType in projType)
                {
                    string settingsPath = Common.PathCombine(Param.Value["OutputPath"].Replace('/', Path.DirectorySeparatorChar), pType);
                    if (Directory.Exists(settingsPath))
                        Directory.Delete(settingsPath, true);
                }
            }
        }

        #endregion Form Load

        #region Tool strip events
        /// <summary>
        /// Edit CSS tool strip event
        /// </summary>
        private void tsEdit_Click(object sender, EventArgs e)
        {
            editToolStripMenuItem1_Click(sender, e);
        }

        /// <summary>
        /// Save tool strip event
        /// </summary>
        private void tsSave_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Exit tool strip event
        /// </summary>
        private void tsExit_Click(object sender, EventArgs e)
        {
            exitToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Close a sub-form tool strip event
        /// </summary>
        private void tsClose_Click(object sender, EventArgs e)
        {
            closeToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// Export button tool strip event
        /// </summary>
        private void tsExport_Click(object sender, EventArgs e)
        {
            toolStripMenuItem7_Click(sender, e);
        }

        /// <summary>
        /// Create a new sub form tool strip event
        /// </summary>
        private void tsNew_Click(object sender, EventArgs e)
        {
            toolStripMenuItem2_Click(sender, e);
        }

        /// <summary>
        /// Open tool strip event
        /// </summary>
        private void tsOpen_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click(sender, e);
        }
        
        /// <summary>
        /// Mail reply button tool strip event
        /// </summary>
        private void tsMail_Click(object sender, EventArgs e)
        {
            toolStripMenuItem3_Click(sender, e);
        }

        /// <summary>
        /// Help tool strip event
        /// </summary>
        /// <param name="sender">object sending event (unused)</param>
        /// <param name="e">arguments of event (unused)</param>
        private void tsHelp_Click(object sender, EventArgs e)
        {
            contentsToolStripMenuItem_Click(sender, e);
        }

        /// <summary>
        /// ShowPreview tool strip event
        /// </summary>
        private void tsPreview_Click(object sender, EventArgs e)
        {
            toolStripMenuItem6_Click(sender, e);
        }
        #endregion Tool strip events

        #region Menu events (not on tool strip)
        /// <summary>
        /// Load Help contents
        /// </summary>
        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
            Common.HelpProv.SetHelpKeyword(this, "Introduction.htm");
            SendKeys.Send("{F1}");
        }

        /// <summary>
        /// Display License in separate window
        /// </summary>
        private void menuLicense_Click(object sender, EventArgs e)
        {
            SubProcess.Run(Path.GetDirectoryName(Application.ExecutablePath), "License.rtf", false);
        }

        /// <summary>
        /// Display Release notes in separate window
        /// </summary>
        private void menuReleaseNotes_Click(object sender, EventArgs e)
        {
			SubProcess.Run(Common.FromRegistry("Help"), "SetupPs.rtf", false);
        }

        /// <summary>
        /// Display ReadMe in separate window
        /// </summary>
        private void menuReadMe_Click(object sender, EventArgs e)
        {
            SubProcess.Run(Path.GetDirectoryName(Application.ExecutablePath), "ReadMe.rtf", false);
        }

        /// <summary>
        /// Display instructions on how to install Flex Utility (This item is obsolete since installer does this).
        /// </summary>
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
            Common.HelpProv.SetHelpKeyword(this, "FlexAdd-on.htm");
            SendKeys.Send("{F1}");
        }

        /// <summary>
        /// Exit application menu item (by closing this form)
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UseReport.Check();
            Close();
        }

        /// <summary>
        /// About dialog menu item
        /// </summary>
        private void aboutDictionaryExpressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutDE = new AboutPs();
            aboutDE.ShowDialog();
        }

        /// <summary>
        /// Cascade sub forms menu item
        /// </summary>
        private void cascadeWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        /// <summary>
        /// Vertically tile sub forms menu item
        /// </summary>
        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        /// <summary>
        /// Horizontally tile sub forms menu item
        /// </summary>
        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        /// <summary>
        /// Localization menu item
        /// </summary>
        private void menuLocalization_Click(object sender, EventArgs e)
        {
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
        }

        /// <summary>
        /// Localization setup menu item
        /// </summary>
        private void menuLocalizationSetup_Click(object sender, EventArgs e)
        {
            var setup = new LocalizationSetup();
            setup.ShowDialog();
            if(setup.Localization)
            {
                LocDB.Localize(menuMain); // Menu Controls
            }
        }

        /// <summary>
        /// Apply style sheet menu item
        /// </summary>
        private void menuApplyStyle_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.ShowTemplate(sender, e);
            }
        }

        /// <summary>
        /// Backup project menu item
        /// </summary>
        private void backupProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.backupToolStripMenuItem_Click_1(sender, e);
            }
        }
        #endregion Menu events (not on tool strip)

        #region Menu events (on tool strip)
        /// <summary>
        /// Open menu item
        /// </summary>
        protected void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var objConverter = new Converter
                                   {
                                       MdiParent = this, 
                                       ExcerptPreview = menuExcerptPreview.Checked
                                   };
            _count++;
            bool success = objConverter.Open();
            if (success)
            {
                objConverter.Show();
                objConverter.Text = Path.GetFileNameWithoutExtension(objConverter.ProjectName);
                RefreshWindowMenu();
                objConverter.ShowPreview(sender, e, menuExcerptPreview.Checked);

                string currentRole = Param.UserRole; //.GetRole();
                //string currentRole = "System manager";// For Testing
                SetRoleChecked(currentRole);
                ShowCSSBasedOnRole(sender, e);
            }
        }

        /// <summary>
        /// To show / hide css files based on user role
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowCSSBasedOnRole(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.ShowCSS(sender, e);
            }
        }

        private void SetRoleChecked(string currentRole)
        {
            foreach (ToolStripMenuItem menuItem in menuRole.DropDownItems)
            {
                menuItem.Checked = menuItem.Text == currentRole;
            }
            StatusRole.Text = "Role - " + currentRole;

            //To Visible the Edit CSS and Save CSS based on role
            bool isSystemDesigner = false;
            if (currentRole == "System Designer")
            {
                isSystemDesigner = true;
            }
            if (currentRole == "Output User")
            {
                menuConfigureTasks.Enabled = false;
                tsConfigureTasks.Enabled = false;
            }
            else
            {
                menuConfigureTasks.Enabled = true;
                tsConfigureTasks.Enabled = true;
            }

            menuEditCSS.Visible = isSystemDesigner;
            menuSaveCSS.Visible = isSystemDesigner;


        }

        protected void MenuRoleChild_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;
            Param.UserRole = menuItem.Text;
            //Param.SetRole(menuItem.Text);
            SetRoleChecked(menuItem.Text);
            ShowCSSBasedOnRole(sender, e);
        }

        private void RefreshWindowMenu()
        {
            if (this.ActiveMdiChild != null)
            {
                Form activeChild = this.ActiveMdiChild;

                //ActivateMdiChild(null);
                ActivateMdiChild(activeChild);
            }
        }

        /// <summary>
        /// Close sub form menu item
        /// </summary>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmChild = ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.Close();
            }
        }

        /// <summary>
        /// New sub form menu item
        /// </summary>
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var objConverter = new Converter
                                   {
                                       MdiParent = this,
                                       ExcerptPreview = menuExcerptPreview.Checked
                                   };
            _count++;
            //objConverter.Text = "Dictionary" + _count;
            objConverter.Text = "";
            string projectName = objConverter.New(_count);
            if (projectName != null)
            {
                objConverter.Show();
                objConverter.ShowPreview(sender, e, menuExcerptPreview.Checked);
                ShowCSSBasedOnRole(sender, e);
            }
        }

        /// <summary>
        /// Send email response menu item
        /// </summary>
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            const string MailTo = "Pathway@sil.org";
            const string MailSubject = "Suggestions / Feedback";
            System.Diagnostics.Process.Start(string.Format("mailto:{0}?Subject={1}", MailTo, MailSubject));
        }

        /// <summary>
        /// Export menu item
        /// </summary>
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.Export(statusProgressBar.ProgressBar);
            }
        }

        /// <summary>
        /// ShowPreview menu item
        /// </summary>
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.ShowPreview(sender, e, menuExcerptPreview.Checked);
            }
        }

        /// <summary>
        /// Format menu item
        /// </summary>
        private void menuDicFormat_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.TaskPick();
            }
        }

        /// <summary>
        /// Save menu item
        /// </summary>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.SaveCSS();
            }
        }

        /// <summary>
        /// Edit CSS menu item
        /// </summary>
        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.CSSReadOnly(false);
            }
        }

        #endregion Menu events (on tool strip)

        private void menushowErrorLog_Click(object sender, EventArgs e)
        {
            
            menushowErrorLog.Checked = !menushowErrorLog.Checked;
            SetVerbose();

            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.ShowErrorLog(menushowErrorLog.Checked);
            }
        }

        private void SetVerbose()
        {
            VerboseClass verboseClass = VerboseClass.GetInstance();
            verboseClass.ShowError = menushowErrorLog.Checked;
            if (!menushowErrorLog.Checked)
            {
                verboseClass.ErrorCount = 0;
            }
        }

        private void frmMDIParent_MdiChildActivate(object sender, EventArgs e)
        {
            bool isChildFormExist = false;
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                menushowErrorLog.Checked = frmChild.ShowError;
                SetVerbose();
                isChildFormExist = true;
            }
            UpdateMenu(isChildFormExist);
        }

        private void frmMDIParent_Activated(object sender, EventArgs e)
        {
            Common.HelpProv.SetHelpNavigator(this, HelpNavigator.Topic);
            Common.HelpProv.SetHelpKeyword(this, "Introduction.htm");
        }

        private void tsStylePick_Click(object sender, EventArgs e)
        {
            menuDicFormat_Click(sender, e);
            
        }

        private void UpdateMenu(bool isChildFormExist)
        {
            menuExport.Enabled = isChildFormExist;
            menuClose.Enabled = isChildFormExist;
            menuEdit.Enabled = isChildFormExist;
            menuSave.Enabled = isChildFormExist;
            menuConfigureTasks.Enabled = isChildFormExist;
            menuApplyStyle.Enabled = isChildFormExist;
            menuPreview.Enabled = isChildFormExist;
            menuBackup.Enabled = isChildFormExist;

            tsPreview.Enabled = isChildFormExist;
            tsExport.Enabled = isChildFormExist;
            tsClose.Enabled = isChildFormExist;
            tsConfigureTasks.Enabled = isChildFormExist;
            tsEdit.Enabled = isChildFormExist;
            tsSave.Enabled = isChildFormExist;
        }

        private void frmMDIParent_DoubleClick(object sender, EventArgs e)
        {

#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void menuLargeFiles_Click(object sender, EventArgs e)
        {
            try
            {
                string PsSupportPath = Common.PathCombine(Common.GetPSApplicationPath(), "Help");
                PsSupportPath = Common.PathCombine(PsSupportPath, "creating_large_docs_OpenOffice.pdf");
                Process.Start(PsSupportPath);
            }
            catch (FileNotFoundException)
            {

            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == 1155)
                {
                    var msg = new[] { "Acorbat reader software to read this file" };
                    LocDB.Message("errInstallFile", "Please install " + msg, msg, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                }
            }

        }

        private void menuEditCSS_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.CSSReadOnly(false);
            }
        }

        private void menuSaveCSS_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.SaveCSS();
            }
        }

        private void ExcerptPreview_Click(object sender, EventArgs e)
        {
            menuExcerptPreview.Checked = !menuExcerptPreview.Checked;
            string previewText = "Preview : ";
                if(menuExcerptPreview.Checked)
                {
                    previewText += "Excerpt";
                }
                else
                {
                    previewText += "Full";
                }
            StatusPreview.Text = previewText;
        }

        public bool ExcerptPreview()
        {
            return menuExcerptPreview.Checked;
        }

        private void menuConfigureDictionaryView_Click(object sender, EventArgs e)
        {
            var dlg = new ConfigureDictionaryView();
            dlg.ShowDialog();
        }

        private void entryFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.FilterForEntrySense("entry");
            }

        }

        private void senseFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.FilterForEntrySense("sense");
            }

        }

        private void setLanguageSortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                setLanguageSortToolStripMenuItem.Checked = !setLanguageSortToolStripMenuItem.Checked;
                frmChild.LanguageSort(setLanguageSortToolStripMenuItem.Checked);
            }

        }

        private void languageFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                frmChild.FilterForEntrySense("language");
            }
        }

        private void entrySortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmChild = (Converter)ActiveMdiChild;
            if (frmChild != null)
            {
                entrySortToolStripMenuItem.Checked = !entrySortToolStripMenuItem.Checked;
                frmChild.EntrySort(entrySortToolStripMenuItem.Checked);
            }
        }



    }
    #endregion
}
