// --------------------------------------------------------------------------------------------
// <copyright file="ExportOdt.cs" from='2009' to='2009' company='SIL International'>
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

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    //public class ExportOdt : PreExportProcess, IExportProcess 
    public class ExportOdt : IExportProcess 
    {
        public string ExportType
        {
            get
            {
                return "OpenOffice";
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

        public bool Export(PublicationInformation projInfo)
        {
            string fileType = "odt";
            Common.OdType = Common.OdtType.OdtChild;
            bool returnValue = false;
            string strFromOfficeFolder = Common.PathCombine(Common.GetPSApplicationPath(), "OfficeFiles" + Path.DirectorySeparatorChar + projInfo.ProjectInputType);
            string strToOfficeFolder = Common.PathCombine(Path.GetTempPath(), "OfficeFiles" + Path.DirectorySeparatorChar + projInfo.ProjectInputType);
            string strStylePath = Common.PathCombine(strToOfficeFolder, "styles.xml");
            string strContentPath = Common.PathCombine(strToOfficeFolder, "content.xml");
            CopyOfficeFolder(strFromOfficeFolder, strToOfficeFolder);
            string strMacroPath = Common.PathCombine(strToOfficeFolder, "Basic/Standard/Module1.xml");
            string outputFileName;
            string outputPath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);

            if (projInfo.FileSequence != null && projInfo.FileSequence.Count > 1)
            {
                //var ul = new Utility();
                //ul.CreateMasterContents(strContentPath, FileSequence);
                fileType = "odm";  // Master Document
                Common.OdType = Common.OdtType.OdtMaster;
                outputFileName = Common.PathCombine(outputPath, projInfo.DictionaryOutputName); // OdtMaster is created in Dictionary Name
            }
            else
            {
                // All other OdtChild files are created in the name of Xhtml or xml file Names.
                if (projInfo.DictionaryOutputName == null)
                {
                    outputFileName = projInfo.DefaultXhtmlFileWithPath.Replace(Path.GetExtension(projInfo.DefaultXhtmlFileWithPath), "");
                }
                else
                {
                    outputFileName = Common.PathCombine(outputPath, projInfo.DictionaryOutputName);
                    Common.OdType = Common.OdtType.OdtNoMaster; // to all the Page property process
                }
            }

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CSSTree cssTree = new CSSTree();
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);

            Dictionary<string, Dictionary<string, string>> OdtAllClass = new Dictionary<string, Dictionary<string, string>>();
            OdtStyles odtStyles = new OdtStyles();
            OdtAllClass = odtStyles.CreateOdtStyles(strToOfficeFolder, cssClass);

            // todo: the following macro
            //IncludeTextinMacro(strMacroPath, styleName.ReferenceFormat);

            // BEGIN Generate Content.Xml File 
            OdtContent odtContent = new OdtContent();
            PreExportProcess preProcessor = new PreExportProcess(projInfo);
            preProcessor.GetTempFolderPath();
            preProcessor.ImagePreprocess();
            preProcessor.SwapHeadWordAndReversalForm();
            odtContent.CreateContent(strToOfficeFolder + Path.DirectorySeparatorChar, preProcessor.ProcessedXhtml, OdtAllClass, fileType);
            //odtContent.CreateContent(progressBar, processedXhtmlFileWithPath, strToOfficeFolder + Path.DirectorySeparatorChar,
            //                   styleName, fileType);

            if (projInfo.FileSequence != null && projInfo.FileSequence.Count > 1)
            {
                ModifyOdtStyles modifyOdtStyles = new ModifyOdtStyles();
                modifyOdtStyles.CreateMasterContents(strContentPath, projInfo.FileSequence);
            }
            VerboseClass verboseClass = VerboseClass.GetInstance();
            var mODT = new ZipFolder();
            string fileNameNoPath = outputFileName + "." + fileType;
            mODT.CreateZip(strToOfficeFolder, fileNameNoPath, verboseClass.ErrorCount);

            if (verboseClass.ErrorCount > 0)
            {
                string errFileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath) + "_err.html";
                Process.Start(errFileName);
            }

            try
            {
                if (File.Exists(fileNameNoPath))
                {
                    returnValue = true;
                    if (projInfo.IsOpenOutput)
                    {
                        Process.Start(fileNameNoPath);
                    }
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == 1155)
                {
                    var msg = new[] { "OpenOffice application from http://www.openoffice.org site.\nAfter downloading and installing Open Office, please consult release notes about how to change the macro security setting to enable the macro that creates the headers." };
                    // Todo: handle the following line
                    //LocDB.Message("errInstallFile", "Please install " + msg, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                    return false;
                }
            }
            return returnValue;
        }

        private void CopyOfficeFolder(string sourceFolder, string destFolder)
        {
            if (Directory.Exists(destFolder))
            {
                Directory.Delete(destFolder, true);
            }
            Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            try
            {
                foreach (string file in files)
                {
                    string name = Path.GetFileName(file);
                    string dest = Common.PathCombine(destFolder, name);
                    File.Copy(file, dest);
                }

                string[] folders = Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = Path.GetFileName(folder);
                    string dest = Common.PathCombine(destFolder, name);
                    if (name != ".svn")
                    {
                        CopyOfficeFolder(folder, dest);
                    }
                }
            }
            catch
            {
                return;
            }
        }


        private static void IncludeTextinMacro(string strMacroPath, string ReferenceFormat)
        {
            var xmldoc = new XmlDocument { XmlResolver = null };
            xmldoc.Load(strMacroPath);
            XmlElement ele = xmldoc.DocumentElement;
            if (ele != null)
            {
                ele.InnerText = "\n'Constant ReferenceFormat for User Desire\nConst ReferenceFormat = \"" + ReferenceFormat + "\"" + ele.InnerText;
            }
            xmldoc.Save(strMacroPath);
        }

    }
}
