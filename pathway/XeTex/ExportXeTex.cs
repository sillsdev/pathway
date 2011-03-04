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
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ExportXeTex : IExportProcess  
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
            preProcessor.ReplaceInvalidTagtoSpan();
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

            Dictionary<string, Dictionary<string, string>> idAllClass = new Dictionary<string, Dictionary<string, string>>();
            XeTexStyles inStyles = new XeTexStyles();
            idAllClass = inStyles.CreateIDStyles(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), cssClass);

            XeTexContent inStory = new XeTexContent();
            Dictionary<string, ArrayList> StyleName = inStory.CreateStory(Common.PathCombine(projInfo.TempOutputFolder, "Stories"), projInfo.DefaultXhtmlFileWithPath, idAllClass, cssTree.SpecificityClass, cssTree.CssClassOrder);

            XeTexMap inDesignMap = new XeTexMap();
            inDesignMap.CreateIDDesignMap(projInfo.TempOutputFolder, StyleName["ColumnClass"].Count, StyleName["TextVariables"], StyleName["CrossRef"]);

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

                }
            }
        }

        #endregion
    }
}
