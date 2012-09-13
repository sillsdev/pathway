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
        private static string _fullPrincePath;
        private static string _processedXhtml;

        #region Properties
        #region ExportType
        public string ExportType
        {
            get
            {
                return "Pdf (Using Prince)";
            }
        }
        #endregion ExportType

        #region Handle
        public bool Handle(string inputDataType)
        {
            bool returnValue = false;
            if (RegPrinceKey != null)
                if (inputDataType.ToLower() == "dictionary" || inputDataType.ToLower() == "scripture")
                {
                    returnValue = true;
                }
            return returnValue;
        }
        #endregion Handle

        #region RegPrinceKey
        public static RegistryKey RegPrinceKey
        {
            get
            {
                RegistryKey regPrinceKey;
                try
                {
                    regPrinceKey =
                        Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MICROSOFT\WINDOWS\CURRENTVERSION\UNINSTALL\Prince_is1");
                    if (regPrinceKey == null)
                        regPrinceKey =
                            Registry.LocalMachine.OpenSubKey(
                                @"SOFTWARE\Wow6432Node\MICROSOFT\WINDOWS\CURRENTVERSION\UNINSTALL\Prince_is1");
                }
                catch (Exception)
                {
                    regPrinceKey = null;
                }
                return regPrinceKey;
            }
        }
        #endregion RegPrinceKey
        #endregion Properties

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
                var regPrinceKey = RegPrinceKey;
                if (regPrinceKey != null || Common.UnixVersionCheck())
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
                    string mergedCSS = mc.Make(projInfo.DefaultCssFileWithPath, "Temp1.css");
                    preProcessor.ReplaceStringInCss(mergedCSS);
                    preProcessor.SetDropCapInCSS(mergedCSS);

                    string xhtmlFileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
                    string defaultCSS = Path.GetFileName(mergedCSS);
                    Common.SetDefaultCSS(preProcessor.ProcessedXhtml, defaultCSS);
                    _processedXhtml = preProcessor.ProcessedXhtml;
                    if (!Common.UnixVersionCheck())
                    {
                        Object princePath = regPrinceKey.GetValue("InstallLocation");
                        _fullPrincePath = Common.PathCombine((string)princePath, "Engine/Bin/Prince.exe");
                        var myPrince = new Prince(_fullPrincePath);
                        myPrince.AddStyleSheet(defaultCSS);
                        myPrince.Convert(_processedXhtml, xhtmlFileName + ".pdf");
                    }
                    else
                        Common.RunCommand("Prince ", _processedXhtml + " " + defaultCSS + " -o " + xhtmlFileName + ".pdf", 1);
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
