// --------------------------------------------------------------------------------------------
// <copyright file="BuildCtx.cs" from='2009' to='2009' company='SIL International'>
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
// Builder for the Bible Translation Edition Release
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Builder;
using SIL.Tool;

namespace BuildCtx
{
    public partial class BuildCtx : Form
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the BuildCtx class.
        /// </summary>
        public BuildCtx()
        {
            InitializeComponent();
        }
        #endregion Constructor

        #region Load
        /// <summary>
        /// Perform remainder of modifications necessary for building an instlaler.
        /// </summary>
        private void BuildCtx_Load(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length < 2 || args[1] != "ConTeXt")
            {
                Close();
                Environment.Exit(0);
            }
            var instPath = Common.DirectoryPathReplace(Environment.CurrentDirectory + @"/../../../Installer/");
            var sub = new Substitution { TargetPath = instPath };

            //Update PathwayUI
            var map = new Dictionary<string, string>();
            const string Readme = "ReadMeCtx.rtf";
            const string License = "License.rtf";
            map[Readme] = FileData.Get(instPath + Readme);
            map[License] = FileData.Get(instPath + License);
            sub.FileSubstitute("PwCtxUI-tpl.wxs", map);
            BuilderBL.UpdateInstallerDescription(instPath, "PwCtx-tpl.wxs", "2.0", null, "");

            //Calculate Files & Features
            BuilderBL.RemoveSubFolders(instPath + "../Files");
            BuilderBL.CopyTree(instPath, "../../PwCtx", "PwCtx");

            SubProcess.Run(instPath, "GenerateFilesSourceCtx.js");
            BuilderBL.SetFilesNFeatures("PwCtx", instPath, sub, map);
            sub.FileSubstitute("FilesCtx-tpl.wxs", map, "Files.wxs");
            sub.FileSubstitute("FeaturesCtx-tpl.wxs", map, "Features.wxs");

            //Build Installer
            if (!SubProcess.ExistsOnPath("candle.exe"))
            {
                MessageBox.Show("Candle.exe missing from path. Add wix binaries to path");
                Environment.Exit(-2);
            }
            SubProcess.Run(instPath, "BuildInstallerCtx.bat");

            string res = FileData.Get(instPath + "wixLink.log");
            if (res.Length > 0)
                MessageBox.Show(res, "Error Report");
            else
            {
                DateTime now = DateTime.Now;
                var curDate = now.ToString("yyyy-MM-d");
                string version = "2.0";
                var target = string.Format("{0}SetupCtx-{1}-{2}.msi", instPath, version, curDate);
                if (File.Exists(target))
                    File.Delete(target);
                File.Move(instPath + "SetupCtx.msi", target);
                //MessageBox.Show("Build successfull", "Result Report");
            }
            Close();
            Environment.Exit(res.Length > 0 ? -1 : 0);
        }

        #endregion Load
    }
}
