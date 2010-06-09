// --------------------------------------------------------------------------------------------
// <copyright file="BuilderBTE.cs" from='2009' to='2009' company='SIL International'>
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

namespace BuilderBTE
{
    public partial class BuilderBTE : Form
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the BuilderBTE class.
        /// </summary>
        public BuilderBTE()
        {
            InitializeComponent();
        }
        #endregion Constructor

        private const string RELEASE = "ReleaseBTE";

        #region Load
        /// <summary>
        /// Perform remainder of modifications necessary for building an instlaler.
        /// </summary>
        private void BuilderBTE_Load(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length < 2 || args[1] != RELEASE)
            {
                Close();
                Environment.Exit(0);
            }
            var instPath = Common.DirectoryPathReplace(Environment.CurrentDirectory + @"/../../../Installer/");
            var sub = new Substitution { TargetPath = instPath };

            //Update OOSUI
            var map = new Dictionary<string, string>();
            const string Readme = "ReadMePw.rtf";
            const string License = "License.rtf";
            const string HelpFile = "Pathway_Configuration_Tool_BTE.chm";
            const string Tutorial = "Pathway_Student_Manual_BTE.doc";
            const string Catalog = "UtilityCatalogIncludePublishingSolution.xml";
            //const string DeToolTransform = "xhtml2dex.xsl";
            map[Readme] = FileData.Get(instPath + Readme);
            map[License] = FileData.Get(instPath + License);
            sub.FileSubstitute("pathwayUI-tpl.wxs", map, "pathwayUI.wxs");

            //Calculate Files & Features
            BuilderBL.RemoveSubFolders(instPath + "../Files");
            BuilderBL.CopyRelaseFiles(instPath, "ConfigurationTool", "ConfigurationTool", RELEASE);
            BuilderBL.CopyFile(instPath, Readme, "../Files/ConfigurationTool");
            BuilderBL.CopyFile(instPath, Tutorial, "../Files/ConfigurationTool");
            BuilderBL.CopyFile(instPath, License, "../Files/ConfigurationTool");
            Directory.Delete(instPath + "../Files/ConfigurationTool/Help", true);
            BuilderBL.CopyFile(instPath, HelpFile, "../Files/ConfigurationTool/Help");
            BuilderBL.CopyRelaseFiles(instPath, "PsExport", "PsDll", RELEASE);
            BuilderBL.CopyFile(instPath, Catalog, "../Files/PsDll/Language Explorer/Configuration");
            BuilderBL.RemoveFiles(instPath, "../../PsExport/Dlls", "PsDll");
            BuilderBL.CopyTree(instPath, "../../PsSupport", "PathwaySupport");
            BuilderBL.CopyTree(instPath, "../../ConfigurationTool/Bin/ReleaseBTE/Backends", "PathwaySupport/Backends");
            //Directory.Delete(instPath + "../Files/PathwaySupport/DEXCTX", true);
            //Directory.Delete(instPath + "../Files/PathwaySupport/xetexPathway", true);
            Directory.Delete(instPath + "../Files/PathwaySupport/Template", true);
            Directory.Delete(instPath + "../Files/PathwaySupport/Help", true);
            BuilderBL.CopyFile(instPath , HelpFile, "../Files/PathwaySupport/Help");
            BuilderBL.RemoveFiles(instPath, "../NotPathway", "PathwaySupport");
            SubProcess.Run(instPath, "GenerateFilesSourcePw.js");
            BuilderBL.SetFilesNFeatures("ConfigurationTool", instPath, sub, map);
            BuilderBL.SetFilesNFeatures("PsDll", instPath, sub, map);
            BuilderBL.SetFilesNFeatures("Support", instPath, sub, map);
            sub.FileSubstitute("FilesPw-tpl.wxs", map, "Files.wxs");
            sub.FileSubstitute("FeaturesPw-tpl.wxs", map, "Features.wxs");

            //Build Installer
            if (!SubProcess.ExistsOnPath("candle.exe"))
            {
                MessageBox.Show("Candle.exe missing from path. Add wix binaries to path");
                Environment.Exit(-2);
            }
            SubProcess.Run(instPath, "BuildInstallerBTE.bat");

            string res = FileData.Get(instPath + "wixLink.log");
            if (res.Length > 0)
                MessageBox.Show(res, "Error Report");
            else
            {
                DateTime now = DateTime.Now;
                var curDate = now.ToString("yyyy-MM-d");
                string version = BuilderBL.GetCurrentVersion("ConfigurationTool");
                var target = string.Format("{0}SetupPwBTE-{1}-{2}.msi", instPath, version, curDate);
                if (File.Exists(target))
                    File.Delete(target);
                File.Move(instPath + "SetupPwBTE.msi", target);
                //MessageBox.Show("Build successfull", "Result Report");
            }
            Close();
            Environment.Exit(res.Length > 0 ? -1 : 0);
        }

        #endregion Load
    }
}
