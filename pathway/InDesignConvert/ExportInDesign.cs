// --------------------------------------------------------------------------------------------
// <copyright file="ExportProcess.cs" from='2009' to='2009' company='SIL International'>
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
// Export process used to Export the ODT and Prince PDF output
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public class ExportInDesign : IExportProcess  
    {
        #region Public Functions
        public string ExportType
        {
            get
            {
                return "InDesign";
            }
        }

        public bool Handle(string inputDataType)
        {
            bool returnValue = false;
            if (inputDataType.ToLower() == "dictionary" || inputDataType.ToLower() == "scripture")
            {
                returnValue = true;
            }
            return returnValue;

        }   

        /// <summary>
        /// Convert XHTML to ODT
        /// </summary>
        public bool Export(PublicationInformation projInfo)
        {
            PreExportProcess preProcessor = new PreExportProcess(projInfo);
            preProcessor.GetTempFolderPath();
            //preProcessor.RemoveEmptySpanHeadword(preProcessor.ProcessedXhtml);
            preProcessor.InsertEmptyHeadwordForReversal(preProcessor.ProcessedXhtml);
            MergeProcessInXHTMLforMasterPage(preProcessor.ProcessedXhtml);
            preProcessor.PreserveSpace();
            preProcessor.ImagePreprocess();
            //preProcessor.InsertFrontMatter(preProcessor.GetCreatedTempFolderPath, true);
            //preProcessor.InsertInDesignFrontMatterContent(projInfo.DefaultXhtmlFileWithPath);

            preProcessor.ReplaceInvalidTagtoSpan("_AllComplexFormEntryBackRefs|LexEntryRef_PrimaryLexemes", "span");
            preProcessor.InsertHiddenChapterNumber();
            preProcessor.InsertHiddenVerseNumber();
            preProcessor.GetDefinitionLanguage();
            var exportTitle = GetExportTitle();
            string fileName = exportTitle.ToString();
            if (exportTitle.ToString() == string.Empty)
            {
                fileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
            }
            projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;
            projInfo.DefaultCssFileWithPath = preProcessor.ProcessedCss;
            projInfo.ProjectPath = Path.GetDirectoryName(preProcessor.ProcessedXhtml);

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);

            
            cssClass = MergeProcessInCSSforMasterPage(projInfo.DefaultCssFileWithPath, cssClass);

            //return false;
            preProcessor.InsertEmptyXHomographNumber(cssClass);

            //To insert the variable for macro use
            InInsertMacro insertMacro = new InInsertMacro();
            insertMacro.InsertMacroVariable(projInfo, cssClass);

            Dictionary<string, Dictionary<string, string>> idAllClass = new Dictionary<string, Dictionary<string, string>>();
            InStyles inStyles = new InStyles();
            idAllClass = inStyles.CreateIDStyles(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), cssClass);

            InGraphic inGraphic = new InGraphic();
            inGraphic.CreateIDGraphic(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), cssClass, cssTree.cssBorderColor);

            InStory inStory = new InStory();
            Dictionary<string, ArrayList> StyleName = inStory.CreateStory(projInfo, idAllClass, cssTree.SpecificityClass, cssTree.CssClassOrder);

            InMasterSpread inMasterSpread = new InMasterSpread();
            ArrayList masterPageNames = inMasterSpread.CreateIDMasterSpread(Common.PathCombine(projInfo.TempOutputFolder, "MasterSpreads"), idAllClass, StyleName["TextVariables"]);
            
            InSpread inSpread = new InSpread();
            inSpread.CreateIDSpread(Common.PathCombine(projInfo.TempOutputFolder, "Spreads"), idAllClass, StyleName["ColumnClass"]);

            InDesignMap inDesignMap = new InDesignMap();
            inDesignMap.CreateIDDesignMap(projInfo.TempOutputFolder, StyleName["ColumnClass"].Count, masterPageNames, StyleName["TextVariables"], StyleName["CrossRef"], projInfo.ProjectInputType);

            InMetaData inMetaData = new InMetaData();
            inMetaData.SetDateTimeinMetaDataXML(projInfo.TempOutputFolder);

            InPreferences inPreferences = new InPreferences();
            inPreferences.CreateIDPreferences(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), idAllClass);

            SubProcess.AfterProcess(projInfo.ProjectFileWithPath);

            string ldmlFullName = Common.PathCombine(projInfo.DictionaryPath, fileName + ".idml");
            Compress(projInfo.TempOutputFolder, ldmlFullName);

            Common.CleanupExportFolder(ldmlFullName, ".tmp,.de", "layout", string.Empty);
            //CreateRAMP(projInfo);
            if (projInfo.IsOpenOutput)
                Launch(ldmlFullName);

            
            return true;
        }

        private static string GetExportTitle()
        {
            Param.LoadSettings();
            string organization;
            try
            {
                organization = Param.Value.ContainsKey("Organization") ? Param.Value["Organization"] : "SIL International";
            }
            catch (Exception)
            {
                organization = "SIL International";
            }
            string exportTitle = string.Empty;
            exportTitle = Param.GetMetadataValue(Param.Title, organization);
            return exportTitle;
        }

        private void CreateRAMP(PublicationInformation projInfo)
        {
            Ramp ramp = new Ramp();
            ramp.Create(projInfo.DefaultXhtmlFileWithPath, ".ldml");
        }

        private Dictionary<string, Dictionary<string, string>> MergeProcessInCSSforMasterPage(string fileName, Dictionary<string, Dictionary<string, string>> cssClass)
        {
            Dictionary<string, Dictionary<string, string>> mergedCssClass = cssClass;
            if(cssClass.Count > 0)
            {
                string flexCSs = Common.PathCombine(Path.GetDirectoryName(fileName), "FlexRev.css");
                Dictionary<string, Dictionary<string, string>> cssClass1 = new Dictionary<string, Dictionary<string, string>>();
                CssTree cssTree = new CssTree();
                cssClass1 = cssTree.CreateCssProperty(flexCSs, true);
                foreach (string clsName in cssClass1.Keys)
                {
                    if(!mergedCssClass.ContainsKey(clsName))
                    {
                        mergedCssClass.Add(clsName, cssClass1[clsName]);
                    }
                }
                Dictionary<string, string> pageBreakProperty = new Dictionary<string, string>();
                pageBreakProperty.Add("page-break-after", "always");
                mergedCssClass.Add("mergeBreak", pageBreakProperty);
            }
            return mergedCssClass;
        }

        private void MergeProcessInXHTMLforMasterPage(string xhtmlFileName)
        {
            string projectFolder = Path.GetDirectoryName(xhtmlFileName);
            string[] fileNames = Directory.GetFiles(projectFolder, "*.xhtml");
            if (ValidateXHTMLFiles(fileNames))
            {
                string mainFileName = Common.PathCombine(projectFolder, "Main.xhtml");
                string flexRevFileName = Common.PathCombine(projectFolder, "FlexRev.xhtml");
                if(File.Exists(flexRevFileName))
                {
                    XmlDocument xDocRev = Common.DeclareXMLDocument(false);
                    XmlNamespaceManager namespaceManagerRev = new XmlNamespaceManager(xDocRev.NameTable);
                    namespaceManagerRev.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
                    xDocRev.Load(flexRevFileName);
                    XmlNodeList divNodesRev = xDocRev.SelectNodes("//xhtml:body/xhtml:div", namespaceManagerRev);
                    if(divNodesRev != null)
                    {
                        XmlDocument xDocMain = Common.DeclareXMLDocument(false);
                        XmlNamespaceManager namespaceManagerMain = new XmlNamespaceManager(xDocMain.NameTable);
                        namespaceManagerMain.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
                        xDocMain.Load(mainFileName);
                        XmlNode bodyNodeMain = xDocMain.SelectSingleNode("//xhtml:body", namespaceManagerMain);
                        if (bodyNodeMain != null)
                        {
                            XmlDocumentFragment styleNode = xDocMain.CreateDocumentFragment();
                            styleNode.InnerXml = "<div class=\"mergeBreak\"></div>";
                            bodyNodeMain.AppendChild(styleNode);
                            for (int i = 0; i < divNodesRev.Count; i++)
                            {
                                XmlNode importNode = xDocMain.ImportNode(divNodesRev[i].CloneNode(true), true);
                                bodyNodeMain.AppendChild(importNode);
                            }
                        }
                        xDocMain.Save(mainFileName);
                    }
                }
            }
        }


        private bool ValidateXHTMLFiles(string[] fileNames)
        {
            bool result = false;
            if(fileNames.Length > 1)
            {
                foreach (string fileName in fileNames)
                {
                    if(Path.GetFileNameWithoutExtension(fileName).IndexOf("main") == 0)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }



        private void Compress(string sourceFolder, string ldmlFullName)
        {
            var mODT = new ZipFolder();
            //string outputPathWithFileName = DefaultXhtmlFileWithPath.Replace(".xhtml", ".idml");
            mODT.CreateZip(sourceFolder, ldmlFullName, 0);
        }

        private void Launch(string ldmlFullName)
        {
            try
            {
                Common.OpenOutput(ldmlFullName);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == 1155)
                {
                    string installedLocation = string.Empty;

                    if (File.Exists(ldmlFullName))
                    {
                        installedLocation = ldmlFullName;
                        
                        var msg = new[] { "Indesign application." };
                        LocDB.Message("errInstallFile", "The output has been save in " + installedLocation, "Please install " + msg, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                    }
                }
            }
        }

        #endregion
    }
}
