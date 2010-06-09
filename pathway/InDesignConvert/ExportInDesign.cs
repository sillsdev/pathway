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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ExportInDesign : IExportProcess  
    {
        #region Public Functions
        public string ExportType
        {
            get
            {
                return "InDesign Alpha";
            }
        }

        public bool Handle(string inputDataType)
        {
            bool returnValue = false;
            //inputDataType.ToLower() == "scripture"
            if (inputDataType.ToLower() == "dictionary")
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
            preProcessor.InsertHiddenChapterNumber();
            preProcessor.InsertHiddenVerseNumber();
            preProcessor.GetDefinitionLanguage();

            projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;
            projInfo.DefaultCssFileWithPath = preProcessor.ProcessedCss;

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CSSTree cssTree = new CSSTree();
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
            Dictionary<string, ArrayList> StyleName = inStory.CreateStory(Common.PathCombine(projInfo.TempOutputFolder, "Stories"), projInfo.DefaultXhtmlFileWithPath, idAllClass, cssTree.SpecificityClass, cssTree.CssClassOrder);

            InMasterSpread inMasterSpread = new InMasterSpread();
            ArrayList masterPageNames = inMasterSpread.CreateIDMasterSpread(Common.PathCombine(projInfo.TempOutputFolder, "MasterSpreads"), idAllClass);
            
            InSpread inSpread = new InSpread();
            inSpread.CreateIDSpread(Common.PathCombine(projInfo.TempOutputFolder, "Spreads"), idAllClass, StyleName["ColumnClass"]);

            InDesignMap inDesignMap = new InDesignMap();
            inDesignMap.CreateIDDesignMap(projInfo.TempOutputFolder, StyleName["ColumnClass"].Count, masterPageNames, StyleName["TextVariables"], StyleName["CrossRef"]);

            InPreferences inPreferences = new InPreferences();
            inPreferences.CreateIDPreferences(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), idAllClass);

            SubProcess.AfterProcess(projInfo.ProjectFileWithPath);

            Compress(projInfo.TempOutputFolder, projInfo.DefaultXhtmlFileWithPath);

            return true;
        }

        

        private void Compress(string sourceFolder, string DefaultXhtmlFileWithPath)
        {
            var mODT = new ZipFolder();

            string outputPathWithFileName = DefaultXhtmlFileWithPath.Replace(".xhtml", ".idml");
            mODT.CreateZip(sourceFolder, outputPathWithFileName, 0);
            try
            {
                Common.OpenOutput(outputPathWithFileName);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == 1155)
                {

                }
            }
        }

        #endregion
    }
}
