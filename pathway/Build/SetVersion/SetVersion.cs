// --------------------------------------------------------------------------------------------
// <copyright file="SetVersion.cs" from='2009' to='2009' company='SIL International'>
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
// Sets the version for the product, in Release Mode
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.ObjectModel;
//using SharpSvn;
using SIL.Tool;
using Test;

namespace Builder
{
    public partial class SetVersion : Form
    {
        #region variables
        /// <summary>Index of the revision part of the currentVersion</summary>
        private int revIndex;

        /// <summary>The product name appears in installer window and in control panel once installed</summary>
        private static string _prodName;

        /// <summary>Readme file name</summary>
        private static string _readMe;

        /// <summary>Installer main program</summary>
        private static string _installerMain;

        /// <summary>Path to folders of Dlls for Fw6.0 that contain Flex interfaces</summary>
        private static string _dllBase;
        #endregion variables

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SetVersion class.
        /// </summary>
        public SetVersion()
        {
            InitializeComponent();
        }
        #endregion Constructor

        #region Load
        /// <summary>
        /// Load version number control and private variable
        /// </summary>
        private void Builder_Load(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length < 2 || args[1].Length < 4 || (args[1].Substring(0, 3) != "Rel" && args[1].Substring(0, 4) != "Corp"))
            {
                Close();
                Environment.Exit(0);
            }
            switch (args[1])
            {
                case "ReleaseBTE":
                    _prodName = @"Pathway BTE ";
                    _readMe = @"ReadMePw.rtf";
                    _installerMain = @"pathwayBTE-tpl.wxs";
                    break;
                case "ReleaseSE":
                    _prodName = @"Pathway SE ";
                    _readMe = @"ReadMePw.rtf";
                    _installerMain = @"pathwaySE-tpl.wxs";
                    break;
                case "CorporateBTE":
                    _prodName = @"Pathway BTE Corporate ";
                    _readMe = @"ReadMePw.rtf";
                    _installerMain = @"pathwayBTE-tpl.wxs";
                    break;
                case "CorporateSE":
                    _prodName = @"Pathway SE Corporate ";
                    _readMe = @"ReadMePw.rtf";
                    _installerMain = @"pathwaySE-tpl.wxs";
                    break;
                case "Release7BTE":
                    _prodName = @"Pathway BTE ";
                    _readMe = @"ReadMePw.rtf";
                    _installerMain = @"pathway7BTE-tpl.wxs";
                    Dlls.Enabled = false;
                    break;
                case "Release7SE":
                    _prodName = @"Pathway SE ";
                    _readMe = @"ReadMePw.rtf";
                    _installerMain = @"pathway7SE-tpl.wxs";
                    Dlls.Enabled = false;
                    break;
                case "Corporate7BTE":
                    _prodName = @"Pathway BTE Corporate ";
                    _readMe = @"ReadMePw.rtf";
                    _installerMain = @"pathway7BTE-tpl.wxs";
                    Dlls.Enabled = false;
                    break;
                case "Corporate7SE":
                    _prodName = @"Pathway SE Corporate ";
                    _readMe = @"ReadMePw.rtf";
                    _installerMain = @"pathway7SE-tpl.wxs";
                    Dlls.Enabled = false;
                    break;
                default:
                    _prodName = @"PublishingSolution ";
                    _readMe = @"ReadMe.rtf";
                    _installerMain = @"OOS-tpl.wxs";
                    break;
            }
            // These lines used to save FieldWorks Version file.
            Common.SupportFolder = "";
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, "/../../PsSupport");

            var myVersionName = PathPart.Bin(Environment.CurrentDirectory, "/../../ConfigurationTool/Properties/AssemblyInfo.cs");
            var m = Regex.Match(FileData.Get(myVersionName), @".assembly. AssemblyFileVersion..([0-9.]+)...");
            BuilderBL.CurrentVersion = m.Groups[1].Value;
            revIndex = BuilderBL.CurrentVersion.LastIndexOf('.') + 1;
            Version.Text = BuilderBL.CurrentVersion;

            // Query Svn and propose version # with latest svn version (give info on changes for updating release notes)
            var curRevNumber = int.Parse(BuilderBL.CurrentVersion.Substring(revIndex));

            // Load Options for Dlls
            if (!Dlls.Enabled)
            {
                const string fwVer = "FieldworksVersions.txt";
                string instPath = BuilderBL.GetInstPath();
                File.Copy(Path.Combine(instPath, fwVer), Common.PathCombine(instPath, "../../PsSupport/" + fwVer), true);
            }
            else
            {
                _dllBase = PathPart.Bin(Environment.CurrentDirectory, "/../../PsExport");
                DirectoryInfo directoryInfo = new DirectoryInfo(_dllBase);
                foreach (DirectoryInfo folder in directoryInfo.GetDirectories(@"Dlls*"))
                {
                    FileInfo fileInfo = new FileInfo(Path.Combine(folder.FullName, "BasicUtils.dll"));
                    if (fileInfo.Exists)
                    {
                        FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(fileInfo.FullName);
                        object obj = string.Format("{0} - {1}", folder.Name, fileVersionInfo.FileVersion);
                        Dlls.Items.Add(obj);
                        if (folder.Name == "Dlls")
                            Dlls.SelectedItem = obj;
                    }
                }
            }
           
            //try
            //{
                //var rev = new SvnClient();
                //var uri = new Uri("https://svn.sil.org/btai");
                //// ReSharper disable RedundantAssignment
                //var myLog = new Collection<SvnLogEventArgs>();
                //// ReSharper restore RedundantAssignment
                //if (rev.GetLog(uri, out myLog))
                //{
                //    var count = myLog.Count;
                //    Version.Text = BuilderBL.CurrentVersion.Substring(0, revIndex) + count;
                //    for (int n = 0; n < count; n++)
                //    {
                //        var revNumber = count - n;
                //        if (curRevNumber < count && revNumber < curRevNumber)
                //            break;
                //        var itm = myLog[n];
                //        var lvItem = new ListViewItem();
                //        lvItem.SubItems[0].Text = lvItem.Text = revNumber.ToString();
                //        lvItem.SubItems.Add(new ListViewItem.ListViewSubItem { Text = itm.Author });
                //        lvItem.SubItems.Add(new ListViewItem.ListViewSubItem { Text = itm.LogMessage });
                //        LvHistory.Items.Add(lvItem);
                //    }
                //}
            //}
            //catch (Exception)
            //{
            //}
        }
        #endregion Load

        #region Button Events
        /// <summary>
        /// Button updates Notes file with new version number (and allows user to make other changes)
        /// </summary>
        private void Notes_Click(object sender, EventArgs e)
        {
            if (!BuilderBL.VersionValidate(Version.Text))
                return;
            string fwVer = ".";
            if (Dlls.Enabled)
            {
                fwVer = BuilderBL.SelectDlls(_dllBase, (string)Dlls.SelectedItem);
                Common.SaveFieldworksVersions(Path.Combine(_dllBase, "Dlls601"));
            }
            string instPath = BuilderBL.GetInstPath();
            BuilderBL.UpdateVersion(instPath + @"../..", Version.Text);
            BuilderBL.UpdateInstallerDescription(instPath, _installerMain, Version.Text, fwVer, _prodName);
            BuilderBL.UpdateReadme(instPath, _readMe, _prodName, Version.Text);
            Close();
            Environment.Exit(0);
        }

        /// <summary>
        /// Accepting means major version not updated so readme and installer description doesn't need updating.
        /// </summary>
        private void BtAccept_Click(object sender, EventArgs e)
        {
            if (!BuilderBL.VersionValidate(Version.Text))
                return;
            if (Version.Text.Substring(0, revIndex) != BuilderBL.CurrentVersion.Substring(0, revIndex))
            {
                MessageBox.Show("Elements of version number other than revision have changed\r\nPlease update release notes");
                return;
            }
            string fwVer = ".";
            if (Dlls.Enabled)
            {
                fwVer = BuilderBL.SelectDlls(_dllBase, (string)Dlls.SelectedItem);
                Common.SaveFieldworksVersions(Path.Combine(_dllBase, "Dlls601"));
            }
            string instPath = BuilderBL.GetInstPath();
            BuilderBL.UpdateVersion(instPath + @"../..", Version.Text);
            BuilderBL.UpdateInstallerDescription(instPath, _installerMain, Version.Text, fwVer, _prodName);
            Close();
            Environment.Exit(0);
        }

        /// <summary>
        /// Build without updating the version numbers in all modules.
        /// </summary>
        private void OldVersion_Click(object sender, EventArgs e)
        {
            if (!BuilderBL.VersionValidate(Version.Text))
                return;
            if (Version.Text.Substring(0, revIndex) != BuilderBL.CurrentVersion.Substring(0, revIndex))
            {
                MessageBox.Show("Elements of version number other than revision have changed\r\nPlease update release notes");
                return;
            }
            string fwVer = ".";
            if (Dlls.Enabled)
            {
                fwVer = BuilderBL.SelectDlls(_dllBase, (string)Dlls.SelectedItem);
                Common.SaveFieldworksVersions(Path.Combine(_dllBase, "Dlls601"));
            }
            string instPath = BuilderBL.GetInstPath();
            BuilderBL.UpdateInstallerDescription(instPath, _installerMain, Version.Text, fwVer, _prodName);
            Close();
            Environment.Exit(0);
        }
        #endregion Button Events
    }
}
