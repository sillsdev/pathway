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
                return "XeTex";
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
            //preProcessor.GetTempFolderPath();
            //preProcessor.PreserveSpace();
            //preProcessor.ImagePreprocess();
            //preProcessor.ReplaceInvalidTagtoSpan();
            //preProcessor.InsertHiddenChapterNumber();
            //preProcessor.InsertHiddenVerseNumber();
            //preProcessor.GetDefinitionLanguage();

            string fileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
            projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;
            projInfo.DefaultCssFileWithPath = preProcessor.ProcessedCss;
            projInfo.ProjectPath = Path.GetDirectoryName(preProcessor.ProcessedXhtml);

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);
            
            Dictionary<string, Dictionary<string, string>> xeTexAllClass = new Dictionary<string, Dictionary<string, string>>();
            XeTexStyles xeTexStyles = new XeTexStyles();
            xeTexAllClass = xeTexStyles.CreateXeTexStyles(Common.PathCombine(projInfo.TempOutputFolder, "Resources"), cssClass);

            XeTexContent xeTexContent = new XeTexContent();
            Dictionary<string, ArrayList> StyleName = xeTexContent.CreateStory(Common.PathCombine(projInfo.TempOutputFolder, "Stories"), projInfo.DefaultXhtmlFileWithPath, xeTexAllClass, cssTree.SpecificityClass, cssTree.CssClassOrder);

            SubProcess.AfterProcess(projInfo.ProjectFileWithPath);

            //if (projInfo.IsOpenOutput)
            //    Launch("ldmlFullName");

            return true;
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
