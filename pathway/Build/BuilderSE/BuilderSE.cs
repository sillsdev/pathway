// --------------------------------------------------------------------------------------------
// <copyright file="BuilderSE.cs" from='2009' to='2009' company='SIL International'>
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

namespace BuilderSE
{
    public partial class BuilderSE : Form
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the BuilderSE class.
        /// </summary>
        public BuilderSE()
        {
            InitializeComponent();
        }
        #endregion Constructor

        private const string CORPORATE = "CorporateSE";

        #region Load
        /// <summary>
        /// Perform remainder of modifications necessary for building an instlaler.
        /// </summary>
        private void BuilderSE_Load(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length < 2 || (args[1] != "ReleaseSE" && args[1] != CORPORATE))
            {
                Close();
                Environment.Exit(0);
            }
            // These lines used to get FieldWorks Version file.
            Common.SupportFolder = "";
            Common.ProgInstall = PathPart.Bin(Environment.CurrentDirectory, "/../../PsSupport");

            var instPath = PathPart.Bin(Environment.CurrentDirectory, "/../Installer/");
            var sub = new Substitution { TargetPath = instPath };

            //Update PathwayUI
            var map = new Dictionary<string, string>();
            const string Readme = "ReadMePw.rtf";
            const string License = "License.rtf";
            const string HelpFile = "Pathway_Configuration_Tool_SE.chm";
            const string Catalog = "UtilityCatalogIncludePublishingSolution.xml";
            //const string DeToolTransform = "xhtml2dex.xsl";
            map[Readme] = FileData.Get(instPath + Readme);
            map[License] = FileData.Get(instPath + License);
            sub.FileSubstitute("pathwayUI-tpl.wxs", map, "pathwayUI.wxs");

            //Calculate Files & Features
            BuilderBL.RemoveSubFolders(instPath + "../Files");
            BuilderBL.DoBatch(instPath, "ConfigurationTool", "postBuild.bat", args[1]);
            BuilderBL.CopyRelaseFiles(instPath, "ConfigurationTool", "ConfigurationTool", args[1]);
            BuilderBL.CopyFile(instPath, Readme, "../Files/ConfigurationTool");
            BuilderBL.CopyFile(instPath, License, "../Files/ConfigurationTool");
            Directory.Delete(instPath + "../Files/ConfigurationTool/Help", true);
            BuilderBL.CopyFile(instPath, HelpFile, "../Files/ConfigurationTool/Help");
            Directory.Delete(instPath + "../Files/ConfigurationTool/Styles/Scripture", true);
            //Directory.Delete(instPath + "../Files/ConfigurationTool/InDesign/Scripture", true);
            Directory.Delete(instPath + "../Files/ConfigurationTool/OfficeFiles/Scripture", true);
            Directory.Delete(instPath + "../Files/ConfigurationTool/Samples/Scripture", true);
            File.Delete(instPath + "../Files/ConfigurationTool/ScriptureStyleSettings.xml");
            BuilderBL.CopyRelaseFiles(instPath, "PsExport", "PsDll", args[1]);
            BuilderBL.CopyFile(instPath, Catalog, "../Files/PsDll/Language Explorer/Configuration");
            BuilderBL.RemoveFiles(instPath, "../../PsExport/Dlls", "PsDll");
            BuilderBL.CopyTree(instPath, "../../PsSupport", "PathwaySupport");
            Directory.Delete(instPath + "../Files/PathwaySupport/GoBible", true);
            Directory.Delete(instPath + "../Files/PathwaySupport/Template", true);
            Directory.Delete(instPath + "../Files/PathwaySupport/Help", true);
            BuilderBL.CopyFile(instPath, HelpFile, "../Files/PathwaySupport/Help");
            BuilderBL.RemoveFiles(instPath, "../NotPathway", "PathwaySupport");
            Directory.Delete(instPath + "../Files/PathwaySupport/Styles/Scripture", true);
            Directory.Delete(instPath + "../Files/PathwaySupport/InDesignFiles/Scripture", true);
            Directory.Delete(instPath + "../Files/PathwaySupport/OfficeFiles/Scripture", true);
            Directory.Delete(instPath + "../Files/PathwaySupport/Samples/Scripture", true);
            File.Delete(instPath + "../Files/PathwaySupport/ScriptureStyleSettings.xml");
            File.Delete(instPath + "../Files/PathwaySupport/TE_XHTML-to-Phone_XHTML.xslt");
            File.Delete(instPath + "../Files/PathwaySupport/TE_XHTML-to-Libronix_Content.xslt");
            File.Delete(instPath + "../Files/PathwaySupport/TE_XHTML-to-Libronix_Metadata.xslt");
            File.Delete(instPath + "../Files/PathwaySupport/TE_XHTML-to-Libronix_Popups.xslt");
            File.Delete(instPath + "../Files/PathwaySupport/TE_XHTML-to-Libronix_Styles.xslt");
            File.Delete(instPath + "../Files/PathwaySupport/pxhtml2xpw-scr.xsl");
            if (args[1] == CORPORATE)
            {
                Directory.Delete(instPath + "../Files/PathwaySupport/xetexPathway", true);
                Directory.Delete(instPath + "../Files/PathwaySupport/Wordpress", true);
                BuilderBL.RemoveFiles(instPath, "../NotCorporate", "PsDll");
                BuilderBL.RemoveFiles(instPath, "../NotCorporate", "ConfigurationTool");
                //Directory.CreateDirectory(Common.PathCombine(instPath, "../Files/PwCtw"));
            }
            //else
            //{
            //    BuilderBL.CopyTree(instPath, "../../PwCtx", "PwCtx");
            //}

            SubProcess.Run(instPath, "GenerateFilesSourcePw.js");
            BuilderBL.SetFilesNFeatures("ConfigurationTool", instPath, sub, map);
            BuilderBL.SetFilesNFeatures("PsDll", instPath, sub, map);
            BuilderBL.SetFilesNFeatures("Support", instPath, sub, map);
            //BuilderBL.SetFilesNFeatures("PwCtx", instPath, sub, map);
            sub.FileSubstitute("FilesPw-tpl.wxs", map, "Files.wxs");
            sub.FileSubstitute("FeaturesPw-tpl.wxs", map, "Features.wxs");

            //Build Installer
            if (!SubProcess.ExistsOnPath("candle.exe"))
            {
                MessageBox.Show("Candle.exe missing from path. Add wix binaries to path");
                Environment.Exit(-2);
            }
            SubProcess.Run(instPath, "BuildInstallerSE.bat");

            string res = FileData.Get(instPath + "wixLink.log");
            if (res.Length > 0)
                MessageBox.Show(res, "Error Report");
            else
            {
                DateTime now = DateTime.Now;
                var curDate = now.ToString("yyyy-MM-d");
                string version = BuilderBL.GetCurrentVersion("ConfigurationTool");
                string test = (args[1].Substring(0, 1) == "R") ? "Test" : "";
                var target = string.Format("{0}SetupPwSE{1}-{2}-{3}-Fw{4}.msi", instPath, test, version, curDate, BuilderBL.PublicFieldWorksVersion());
                if (File.Exists(target))
                    File.Delete(target);
                File.Move(instPath + "SetupPwSE.msi", target);
                //MessageBox.Show("Build successfull", "Result Report");
            }
            Close();
            Environment.Exit(res.Length > 0 ? -1 : 0);
        }

        #endregion Load
    }
}
