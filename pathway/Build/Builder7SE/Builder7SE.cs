// --------------------------------------------------------------------------------------------
// <copyright file="Builder7SE.cs" from='2009' to='2009' company='SIL International'>
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
using Test;

namespace Builder7SE
{
    public partial class Builder7SE : Form
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the Builder7SE class.
        /// </summary>
        public Builder7SE()
        {
            InitializeComponent();
        }
        #endregion Constructor

        private const string CORPORATE = "Corporate7SE";

        #region Load
        /// <summary>
        /// Perform remainder of modifications necessary for building an instlaler.
        /// </summary>
        private void Builder7SE_Load(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length < 2 || (args[1] != "Release7SE" && args[1] != CORPORATE))
            {
                Close();
                Environment.Exit(0);
            }
            var instPath = PathPart.Bin(Environment.CurrentDirectory, "/../Installer/");
            var sub = new Substitution { TargetPath = instPath };

            //Update OOSUI
            var map = new Dictionary<string, string>();
            const string Readme = "ReadMePw.rtf";
            const string License = "License.rtf";
            const string HelpFile = "Pathway_Configuration_Tool_SE.chm";
            const string Tutorial = "Pathway_Student_Manual_SE.doc";
            //const string Catalog = "UtilityCatalogIncludePublishingSolution.xml";
            map[Readme] = FileData.Get(instPath + Readme);
            map[License] = FileData.Get(instPath + License);
            sub.FileSubstitute("pathwayUI-tpl.wxs", map, "pathwayUI.wxs");

            //Calculate Files & Features
            BuilderBL.RemoveSubFolders(instPath + "../Files");
            BuilderBL.DoBatch(instPath, "ConfigurationTool", "postBuild.bat", args[1]);
            BuilderBL.CopyRelaseFiles(instPath, "ConfigurationTool", "ConfigurationTool", args[1]);
            BuilderBL.CopyFile(instPath, Readme, "../Files/ConfigurationTool");
            BuilderBL.CopyFile(instPath, Tutorial, "../Files/ConfigurationTool");
            BuilderBL.CopyFile(instPath, License, "../Files/ConfigurationTool");
            BuilderBL.CopyTree(instPath, "../../PsSupport", "ConfigurationTool");
            Directory.Delete(instPath + "../Files/ConfigurationTool/Help", true);
            BuilderBL.CopyFile(instPath, HelpFile, "../Files/ConfigurationTool/Help");
            BuilderBL.CopyRelaseFiles(instPath, "PsExport", "ConfigurationTool", args[1]);

            BuilderBL.RemoveFiles(instPath, "../NotPathway", "ConfigurationTool");
            Directory.Delete(instPath + "../Files/ConfigurationTool/GoBible", true);
            Directory.Delete(instPath + "../Files/ConfigurationTool/Template", true);
            Directory.Delete(instPath + "../Files/ConfigurationTool/Styles/Scripture", true);
            Directory.Delete(instPath + "../Files/ConfigurationTool/InDesignFiles/Scripture", true);
            Directory.Delete(instPath + "../Files/ConfigurationTool/OfficeFiles/Scripture", true);
            Directory.Delete(instPath + "../Files/ConfigurationTool/Samples/Scripture", true);
            File.Delete(instPath + "../Files/ConfigurationTool/ScriptureStyleSettings.xml");
            File.Delete(instPath + "../Files/ConfigurationTool/TE_XHTML-to-Phone_XHTML.xslt");
            File.Delete(instPath + "../Files/ConfigurationTool/TE_XHTML-to-Libronix_Content.xslt");
            File.Delete(instPath + "../Files/ConfigurationTool/TE_XHTML-to-Libronix_Metadata.xslt");
            File.Delete(instPath + "../Files/ConfigurationTool/TE_XHTML-to-Libronix_Popups.xslt");
            File.Delete(instPath + "../Files/ConfigurationTool/TE_XHTML-to-Libronix_Styles.xslt");
            File.Delete(instPath + "../Files/ConfigurationTool/pxhtml2xpw-scr.xsl");
            if (args[1] == CORPORATE)
            {
                Directory.Delete(instPath + "../Files/ConfigurationTool/xetexPathway", true);
                Directory.Delete(instPath + "../Files/ConfigurationTool/Wordpress", true);
                BuilderBL.RemoveFiles(instPath, "../NotCorporate", "ConfigurationTool");
                //Directory.CreateDirectory(Common.PathCombine(instPath, "../Files/PwCtw"));
            }
            //else
            //{
            //    BuilderBL.CopyTree(instPath, "../../PwCtx", "PwCtx");
            //}

            BuilderBL.ZeroCheck(Common.PathCombine(instPath, "../Files"));
            SubProcess.Run(instPath, "GenerateFilesSource7Pw.js");
            BuilderBL.SetFilesNFeatures("ConfigurationTool", instPath, sub, map);
            sub.FileSubstitute("Files7Pw-tpl.wxs", map, "Files.wxs");
            sub.FileSubstitute("Features7Pw-tpl.wxs", map, "Features.wxs");

            //Build Installer
            if (!SubProcess.ExistsOnPath("candle.exe"))
            {
                MessageBox.Show("Candle.exe missing from path. Add wix binaries to path");
                Environment.Exit(-2);
            }
            SubProcess.Run(instPath, "BuildInstaller7SE.bat");

            string res = FileData.Get(instPath + "wixLink.log");
            if (res.Length > 0)
                MessageBox.Show(res, "Error Report");
            else
            {
                DateTime now = DateTime.Now;
                var curDate = now.ToString("yyyy-MM-d");
                string version = BuilderBL.GetCurrentVersion("ConfigurationTool");
                //var fwVer = Path.GetFileName(Environment.GetEnvironmentVariable("FwBase"));
                //if (fwVer.ToLower() == "fww")
                //    fwVer = "Fw7";
                //var target = string.Format("{0}Setup7PwSE-{1}-{2}-{3}.msi", instPath, version, curDate, fwVer);
                string test = (args[1].Substring(0, 1) == "R") ? "Test" : "";
                var target = string.Format("{0}SetupPw7SE{1}-{2}-{3}.msi", instPath, test, version, curDate);
                if (File.Exists(target))
                    File.Delete(target);
                File.Move(instPath + "SetupPw7SE.msi", target);
            }
            Close();
            Environment.Exit(res.Length > 0 ? -1 : 0);
        }

        #endregion Load
    }
}
