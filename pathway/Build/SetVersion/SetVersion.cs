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
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using SharpSvn;
using SIL.Tool;

namespace Builder
{
    public partial class SetVersion : Form
    {
        #region variables
        /// <summary>
        /// Index of the revision part of the currentVersion
        /// </summary>
        private int revIndex;

        /// <summary>
        /// Constant to contain the product name
        /// </summary>
        private const string ProdName = @"Pathway ";
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
            if (args.Length < 2 || args[1] != "Release")
            {
                Close();
                Environment.Exit(0);
            }
            var myVersionName = Environment.CurrentDirectory + @"/../../../../PublishingSolutionExe/Properties/AssemblyInfo.cs";
            var m = Regex.Match(FileData.Get(myVersionName), @".assembly. AssemblyFileVersion..([0-9.]+)...");
            BuilderBL.CurrentVersion = m.Groups[1].Value;
            revIndex = BuilderBL.CurrentVersion.LastIndexOf('.') + 1;
            Version.Text = BuilderBL.CurrentVersion;

            // Query Svn and propose version # with latest svn version (give info on changes for updating release notes)
            var curRevNumber = int.Parse(BuilderBL.CurrentVersion.Substring(revIndex));
            var rev = new SvnClient();
            var uri = new Uri("https://svn.sil.org/btai");
            // ReSharper disable RedundantAssignment
            var myLog = new Collection<SvnLogEventArgs>();
            // ReSharper restore RedundantAssignment
            if (rev.GetLog(uri, out myLog))
            {
                var count = myLog.Count;
                Version.Text = BuilderBL.CurrentVersion.Substring(0, revIndex) + count;
                for (int n = 0; n < count; n++)
                {
                    var revNumber = count - n;
                    if (curRevNumber < count && revNumber < curRevNumber)
                        break;
                    var itm = myLog[n];
                    var lvItem = new ListViewItem();
                    lvItem.SubItems[0].Text = lvItem.Text = revNumber.ToString();
                    lvItem.SubItems.Add(new ListViewItem.ListViewSubItem { Text = itm.Author });
                    lvItem.SubItems.Add(new ListViewItem.ListViewSubItem { Text = itm.LogMessage });
                    LvHistory.Items.Add(lvItem);
                }
            }
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
            string instPath = BuilderBL.GetInstPath();
            BuilderBL.UpdateVersion(instPath + @"../..", Version.Text);
            BuilderBL.UpdateInstallerDescription(instPath, "oos.wxs", Version.Text);
            BuilderBL.UpdateReadme(instPath, "ReadMe.rtf", ProdName, Version.Text);
            Close();
            Environment.Exit(0);
        }

        /// <summary>
        /// Accepting means major version not updated so readme and oos doesn't need updating.
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
            string instPath = BuilderBL.GetInstPath();
            BuilderBL.UpdateVersion(instPath + @"../..", Version.Text);
            Close();
            Environment.Exit(0);
        }
        #endregion Button Events
    }
}
