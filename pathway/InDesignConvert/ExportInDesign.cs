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

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
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
            preProcessor.PreserveSpace();
            preProcessor.ImagePreprocess();
            preProcessor.InsertFrontMatter(preProcessor.GetCreatedTempFolderPath, true);
            preProcessor.ReplaceInvalidTagtoSpan("_AllComplexFormEntryBackRefs|LexEntryRef_PrimaryLexemes", "span");
            preProcessor.InsertHiddenChapterNumber();
            preProcessor.InsertHiddenVerseNumber();
            preProcessor.GetDefinitionLanguage();

            

            string fileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
            projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;
            projInfo.DefaultCssFileWithPath = preProcessor.ProcessedCss;
            projInfo.ProjectPath = Path.GetDirectoryName(preProcessor.ProcessedXhtml);

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
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

            if (projInfo.IsOpenOutput)
                Launch(ldmlFullName);

            return true;
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
