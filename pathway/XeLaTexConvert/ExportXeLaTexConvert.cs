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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using SIL.Tool;
using Test;

namespace SIL.PublishingSolution
{
    public class ExportXeLaTexConvert : IExportProcess  
    {
        #region Public Functions
        public string ExportType
        {
            get
            {
                return "XeLaTex";
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
            cssTree.OutputType = Common.OutputType.XETEX; 
            cssClass = cssTree.CreateCssProperty(projInfo.DefaultCssFileWithPath, true);

            string xetexFullFile = Path.Combine(projInfo.ProjectPath, fileName + ".tex");
            StreamWriter xetexFile = new StreamWriter(xetexFullFile);

            Dictionary<string, List<string>> classInlineStyle = new Dictionary<string, List<string>>();
            Dictionary<string, Dictionary<string, string>> xeTexAllClass = new Dictionary<string, Dictionary<string, string>>();
            XeLaTexStyles xeLaTexStyles = new XeLaTexStyles();
            classInlineStyle = xeLaTexStyles.CreateXeTexStyles(projInfo.ProjectPath, xetexFile, cssClass);

            XeLaTexContent xeLaTexContent = new XeLaTexContent();
            Dictionary<string, Dictionary<string, string>> newProperty = xeLaTexContent.CreateContent(projInfo.ProjectPath,cssClass, xetexFile, projInfo.DefaultXhtmlFileWithPath, classInlineStyle, cssTree.SpecificityClass, cssTree.CssClassOrder);

            CloseFile(xetexFile);

            ModifyXeLaTexStyles modifyXeLaTexStyles = new ModifyXeLaTexStyles();
            modifyXeLaTexStyles.ModifyStylesXML(projInfo.ProjectPath, xetexFile, newProperty, cssClass, xetexFullFile);

            //CallXeTex(Path.GetFileName(xetexFullFile));
            Dictionary<string, string> imgPath = new Dictionary<string, string>();
            if (newProperty.ContainsKey("ImagePath"))
            {
                imgPath = newProperty["ImagePath"];
            }
            CallXeTex(xetexFullFile, true, imgPath);
            return true;
        }

        public void CallXeTex(string xetexFullFile,bool openFile,Dictionary<string, string> ImageFilePath)
        {
            string workingXetex = Common.GetTempCopy("xelatexExe");
            string instPath = Common.PathCombine(workingXetex, "bin");
            string dest = Common.PathCombine(instPath,Path.GetFileName(xetexFullFile));
            const bool overwrite = true;
            File.Copy(xetexFullFile, dest, overwrite);


            foreach (KeyValuePair<string, string> keyValuePair in ImageFilePath)
            {

                string filePath = keyValuePair.Value;
                string toPath = Path.Combine(instPath, Path.GetFileName(filePath));
                File.Copy(filePath, toPath, true);
            }

            if (!File.Exists(dest))
            {
                MessageBox.Show("File is not copied");
                return;
            }

            string originalDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(instPath);
            const string name = "xelatex.exe";
            //string p1Output = string.Empty;
            string p1Error = string.Empty;
            using (Process p1 = new Process())
            {
            p1.StartInfo.FileName = name;
            if (xetexFullFile != null)
                    p1.StartInfo.Arguments = "-interaction=batchmode \"" + Path.GetFileName(xetexFullFile) + "\"";
                p1.StartInfo.RedirectStandardOutput = true;
            p1.StartInfo.RedirectStandardError = p1.StartInfo.RedirectStandardOutput;
            p1.StartInfo.UseShellExecute = !p1.StartInfo.RedirectStandardOutput;
            p1.Start();
                p1.WaitForExit();
                //p1Output = p1.StandardOutput.ReadToEnd();
                p1Error = p1.StandardError.ReadToEnd();
            }
            Directory.SetCurrentDirectory(originalDirectory);
            string texNameOnly = Path.GetFileNameWithoutExtension(xetexFullFile);
            string userFolder = Path.GetDirectoryName(xetexFullFile);
            string logFullName = CopyProcessResult(instPath, texNameOnly, ".log", userFolder);
            string pdfFullName = CopyProcessResult(instPath, texNameOnly, ".pdf", userFolder);
            if (openFile && File.Exists(pdfFullName))
            {
                Common.OpenOutput(pdfFullName);
            }
            //MessageBox.Show(p1Output, "XeTex output");
            MessageBox.Show(p1Error, "XeTex errors");
            MessageBox.Show(string.Format("Review {0} for conversion results.", logFullName), "XeTex log");
            const bool recursive = true;
            Directory.Delete(workingXetex, recursive);
        }

        protected static string CopyProcessResult(string instPath, string texNameOnly, string ext, string userFolder)
                {
            const bool overwrite = true;
            string logName = texNameOnly + ext;
            string tmpLogFullName = Path.Combine(instPath, logName);
            string logFullName = Path.Combine(userFolder, logName);
            if (File.Exists(tmpLogFullName))
                File.Copy(tmpLogFullName, logFullName, overwrite);
            return logFullName;
        }


        private void CloseFile(StreamWriter xetexFile)
        {
            xetexFile.WriteLine();
            xetexFile.WriteLine(@"\bye");
            xetexFile.Flush();
            xetexFile.Close();
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
