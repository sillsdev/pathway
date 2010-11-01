// --------------------------------------------------------------------------------------------
// <copyright file="Builder.cs" from='2009' to='2009' company='SIL International'>
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
// Builder for the Project
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SIL.Tool;
using Test;

namespace Builder
{
    public partial class Builder : Form
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Builder class.
        /// </summary>
        public Builder()
        {
            InitializeComponent();
        }
        #endregion Constructor

        private const string RELEASE = "Release";

        #region Load
        /// <summary>
        /// Perform remainder of modifications necessary for building an instlaler.
        /// </summary>
        private void Builder_Load(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length < 2 || args[1] != RELEASE)
            {
                Close();
                Environment.Exit(0);
            }
            // These lines used to get FieldWorks Version file.
            Common.SupportFolder = "";
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, "/../../PsSupport");

            var instPath = PathPart.Bin(Environment.CurrentDirectory, "/../Installer/");
            var sub = new Substitution { TargetPath = instPath };

            //Update OOSUI
            var map = new Dictionary<string, string>();
            const string Readme = "ReadMe.rtf";
            const string License = "License.rtf";
            //const string Catalog = "UtilityCatalogIncludePublishingSolution.xml";
            //const string DeToolTransform = "xhtml2dex.xsl";
            map[Readme] = FileData.Get(instPath + Readme);
            map[License] = FileData.Get(instPath + License);
            sub.FileSubstitute("OosUI-tpl.wxs", map, "OosUI.wxs");

            //Calculate Files & Features
            BuilderBL.RemoveSubFolders(instPath + @"../Files");
            BuilderBL.DoBatch(instPath, "PublishingSolutionExe", "postBuild.bat", args[1]);
            BuilderBL.DoBatch(instPath, "PsExport", "CopyFwDlls.bat", args[1]);
            BuilderBL.CopyRelaseFiles(instPath, "PublishingSolutionExe", "PublishingSolution", RELEASE);
            BuilderBL.CopyFile(instPath, Readme, @"../Files/PublishingSolution");
            BuilderBL.CopyFile(instPath, License, @"../Files/PublishingSolution");
            //BuilderBL.CopyRelaseFiles(instPath, "PsExport", "PsDll", RELEASE);
            //BuilderBL.CopyFile(instPath, Catalog, @"../Files/PsDll/Language Explorer/Configuration");
            //BuilderBL.RemoveFiles(instPath, @"../../PsExport/Dlls", "PsDll");
            BuilderBL.CopyTree(instPath, @"../../PsSupport", "PathwaySupport");
            //BuilderBL.CopyTree(instPath, "../../PublishingSolutionExe/Bin/Release/Backends", "PathwaySupport/Backends");
            //CopyTree(instPath, "../../../XeTeX/DEXCTX", "PathwaySupport/DEXCTX");
            //File.Copy(Common.PathCombine(instPath + "../../../XeTeX", DeToolTransform), Common.PathCombine(instPath + "../Files/PathwaySupport", DeToolTransform), true);
            SubProcess.Run(instPath, "GenerateFilesSource.js");
            BuilderBL.SetFilesNFeatures("PublishingSolution", instPath, sub, map);
            //BuilderBL.SetFilesNFeatures("PsDll", instPath, sub, map);
            BuilderBL.SetFilesNFeatures("Support", instPath, sub, map);
            sub.FileSubstitute("Files-tpl.wxs", map, "Files.wxs");
            sub.FileSubstitute("Features-tpl.wxs", map, "Features.wxs");

            //Build Installer
            if (!SubProcess.ExistsOnPath("candle.exe"))
            {
                MessageBox.Show("Candle.exe missing from path. Add wix binaries to path");
                Environment.Exit(-2);
            }
            SubProcess.Run(instPath, "BuildInstaller.bat");

            string res = FileData.Get(instPath + "wixLink.log");
            if (res.Length > 0)
                MessageBox.Show(res, "Error Report");
            else
            {
                DateTime now = DateTime.Now;
                var curDate = now.ToString("yyyy-MM-d");
                string version = BuilderBL.GetCurrentVersion("PublishingSolutionExe");
                var target = string.Format("{0}SetupPs-{1}-{2}-Fw{3}.msi", instPath, version, curDate, BuilderBL.PublicFieldWorksVersion());
                if (File.Exists(target))
                    File.Delete(target);
                File.Move(instPath + "SetupOos.msi", target);
                //MessageBox.Show("Build successfull", "Result Report");
            }
            Close();
            Environment.Exit(res.Length > 0 ? -1 : 0);
        }
        #endregion Load
    }
}
