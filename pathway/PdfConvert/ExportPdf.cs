// --------------------------------------------------------------------------------------------
// <copyright file="ExportPdf.cs" from='2009' to='2009' company='SIL International'>
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
// Stylepick FeatureSheet
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using SIL.Tool;


namespace SIL.PublishingSolution
{
    public class ExportPdf : IExportProcess 
    {
        public string ExportType
        {
            get
            {
                return "Pdf";
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
        /// Entry point for InDesign export
        /// </summary>
        /// <param name="exportType">scripture / dictionary</param>
        /// <param name="publicationInformation">structure with other necessary information about project.</param>
        /// <returns></returns>
        public bool Launch(string exportType, PublicationInformation publicationInformation)
        {
            return Export(publicationInformation);
        }

        public bool Export(PublicationInformation projInfo)
        {
            bool success;
            try
            {
                RegistryKey regPrinceKey;
                try
                {
                    regPrinceKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MICROSOFT\WINDOWS\CURRENTVERSION\UNINSTALL\Prince_is1");
                }
                catch (Exception)
                {
                    regPrinceKey = null;
                }
                if (regPrinceKey != null)
                {
                    var curdir = Environment.CurrentDirectory;
                    PreExportProcess preProcessor = new PreExportProcess(projInfo);
                    Environment.CurrentDirectory = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
                    preProcessor.GetTempFolderPath();
                    preProcessor.ImagePreprocess();
                    preProcessor.ReplaceSlashToREVERSE_SOLIDUS();
                    if (projInfo.SwapHeadword)
                        preProcessor.SwapHeadWordAndReversalForm();
                    string tempFolder = Path.GetDirectoryName(preProcessor.ProcessedXhtml);
                    string tempFolderName = Path.GetFileName(tempFolder);
                    var mc = new MergeCss { OutputLocation = tempFolderName };
                    string mergedCSS = mc.Make(projInfo.DefaultCssFileWithPath);
                    preProcessor.ReplaceStringInCss(mergedCSS);
                    preProcessor.SetDropCapInCSS(mergedCSS);

                    string xhtmlFileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
                    //if (!String.IsNullOrEmpty(projInfo.DictionaryOutputName))
                    //    xhtmlFileName = Path.GetFileNameWithoutExtension(projInfo.DictionaryOutputName); 
                    string defaultCSS = Path.GetFileName(mergedCSS);
                    Common.SetDefaultCSS(preProcessor.ProcessedXhtml, defaultCSS);
                    Object princePath = regPrinceKey.GetValue("InstallLocation");
                    var myPrince = new Prince(princePath + "Engine/Bin/Prince.exe");
                    myPrince.AddStyleSheet(defaultCSS);
                    myPrince.Convert(preProcessor.ProcessedXhtml, xhtmlFileName + ".pdf");
                    if (!Common.Testing)
                        Process.Start(xhtmlFileName + ".pdf");
                    Environment.CurrentDirectory = curdir;
                    success = true;
                }
                else
                {
                    //if (Common.Testing) return;
                    //var msg = new[] { "PrinceXML not installed in this system" };
                    //LocDB.Message("defErrMsg", "PrinceXML not installed in this system", msg, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                    success = false;
                }
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }
    }
}
