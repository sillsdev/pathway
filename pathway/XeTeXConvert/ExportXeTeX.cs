// --------------------------------------------------------------------------------------------
// <copyright file="ExportXeTeX.cs" from='2009' to='2009' company='SIL International'>
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ExportXeTeX : IExportProcess
    {
        protected static string _inputType;
        protected static PostscriptLanguage _postscriptLanguage = new PostscriptLanguage();

        #region property string ExportType
        /// <summary>Text to appear in drop down list.</summary>
        public string ExportType
        {
            get
            {
                return "XeTeX Alpha";
            }
        }
        #endregion property string ExportType

        #region bool Handle(string inputDataType)
        /// <summary>
        /// The calling program identifies the kind of data
        /// </summary>
        /// <param name="inputDataType">dictionary or scripture</param>
        /// <returns>true if this backend can handle the data</returns>
        public bool Handle(string inputDataType)
        {
            bool returnValue = false;
            string dataType = inputDataType.ToLower();
            if (dataType == "dictionary" || dataType == "scripture")
            {
                returnValue = true;
            }
            if (string.IsNullOrEmpty(PathwayPath.GetCtxDir()))
            {
                returnValue = false;
            }
            return returnValue;
        }
        #endregion bool Handle(string inputDataType)

        #region bool Launch(string exportType, PublicationInformation publicationInformation)
        /// <summary>
        /// Entry point for XeTeX export
        /// </summary>
        /// <param name="exportType">scripture / dictionary</param>
        /// <param name="publicationInformation">structure with other necessary information about project.</param>
        /// <returns></returns>
        public bool Launch(string exportType, PublicationInformation publicationInformation)
        {
            //if (!Handle(exportType))
            //    return false;
            _inputType = exportType.ToLower();
            return Export(publicationInformation);
        }
        #endregion bool Launch(string exportType, PublicationInformation publicationInformation)

        /// <summary>
        /// Entry point for XeTeX converter
        /// </summary>
        /// <param name="projInfo">values passed including xhtml and css names</param>
        /// <returns>true if succeeds</returns>
        public bool Export(PublicationInformation projInfo)
        {
            bool success;
            try
            {
                _inputType = projInfo.ProjectInputType.ToLower();
                var curdir = Environment.CurrentDirectory;
                Environment.CurrentDirectory = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
                IPreExportProcess data = PreProcess(projInfo);
                string fileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
                string xetexToolFolder = GetTempCopy("xetexPathway");
                string dataPath = Common.PathCombine(xetexToolFolder, "data");
                SetupSettings(data, dataPath);
                CreateTexInput(data.ProcessedXhtml, fileName, dataPath);
                AddImages(Path.GetDirectoryName(data.ProcessedXhtml), dataPath);
                if (!SetupStartScript(xetexToolFolder))
                    return false;
                if (!Common.Testing)
                {
                    SubProcess.Run(xetexToolFolder, "startXPWtool.bat");
                    File.Copy(Common.PathCombine(xetexToolFolder, "XPWtool.pdf"), fileName + ".pdf", true);
                    _postscriptLanguage.SaveCache();
                    Process.Start(fileName + ".pdf");
                }
                Environment.CurrentDirectory = curdir;
                success = true;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                success = false;
            }
            return success;
        }

        #region SetupSettings(string mergedCSS, string dataPath)
        /// <summary>
        /// Get settings from CSS and put into XPWtool settings file
        /// </summary>
        /// <param name="data">structure with data including style sheet</param>
        /// <param name="dataPath">folder where settings template exists and settings file will be created</param>
        protected static void SetupSettings(IPreExportProcess data, string dataPath)
        {
            string fontSize = "12";
            string columns = "2";
            string align = "left";
            bool rtl = false;
            string pageSize = "letter";
            CssTree cssTree = new CssTree();
            Dictionary<string, Dictionary<string, string>> cssProperty = cssTree.CreateCssProperty(data.ProcessedCss, true);
            if (_inputType == "dictionary")
            {
                if (cssProperty.ContainsKey("entry") && cssProperty["entry"].ContainsKey("font-size"))
                    fontSize = cssProperty["entry"]["font-size"];
                _postscriptLanguage.ClassPostscriptName("letter", "Regular", cssProperty);
                if (cssProperty.ContainsKey("@page") && cssProperty["@page"].ContainsKey("page-width") && cssProperty["@page"].ContainsKey("page-height"))
                {
                    string pageDimensions = cssProperty["@page"]["page-width"] + "x" + cssProperty["@page"]["page-height"];
                    pageSize = cssTree.PageSize(pageDimensions);
                }
                if (cssProperty.ContainsKey("letData"))
                    columns = cssProperty["letData"]["column-count"];
                if (cssProperty.ContainsKey("entry"))
                {
                    align = cssProperty["entry"]["text-align"];
                    rtl = cssProperty["entry"].ContainsKey("direction");
                    if (rtl && cssProperty["entry"]["direction"] != "rtl")
                        rtl = false;
                }
            }
            else
            {
                fontSize = cssProperty["Paragraph"]["font-size"];
                _postscriptLanguage.ClassPostscriptName("Paragraph", "Regular", cssProperty);
                string pageDimensions = cssProperty["@page"]["width"] + "x" + cssProperty["@page"]["height"];
                pageSize = cssTree.PageSize(pageDimensions);
                columns = cssProperty["columns"]["column-count"];
                align = cssProperty["scrSection"]["text-align"];
                rtl = cssProperty["Paragraph"].ContainsKey("direction");
                if (rtl && cssProperty["Paragraph"]["direction"] != "rtl")
                    rtl = false;
            }
            _postscriptLanguage.AccumulatePostscriptNames(cssProperty);
            var sub = new Substitution { TargetPath = dataPath };
            var map = new Dictionary<string, string>();
            map["Font2"] = _postscriptLanguage.TexFont(2);
            map["Font3"] = _postscriptLanguage.TexFont(3);
            map["Font4"] = _postscriptLanguage.TexFont(4);
            map["Font5"] = _postscriptLanguage.TexFont(5);
            map["G2"] = _postscriptLanguage.IsGraphite(2);
            map["G3"] = _postscriptLanguage.IsGraphite(3);
            map["G4"] = _postscriptLanguage.IsGraphite(4);
            map["G5"] = _postscriptLanguage.IsGraphite(5);
            map["FontSize"] = fontSize;
            map["PaperSize"] = pageSize;
            map["Columns"] = columns == "2"? "double": "single";
            map["Ragged"] = align == "justify"? "": "T";
            map["RTL"] = rtl? "T": "";
            map["IndexTab"] = "";
            map["InputType"] = _inputType;
            sub.FileSubstitute("XPWsettings-tpl.txt", map);
        }
        #endregion SetupSettings(string mergedCSS, string dataPath)

        #region CreateTexInput(string processedXhtml, string fileName, string dataPath)
        /// <summary>
        /// Create Tex Input file and insert in temp folder location
        /// </summary>
        /// <param name="processedXhtml">xhtml that has been pre-processed</param>
        /// <param name="fileName">xslt creates a file with this name</param>
        /// <param name="dataPath">path where resulting input file is to be placed.</param>
        protected void CreateTexInput(string processedXhtml, string fileName, string dataPath)
        {
            string xslTemplateName = _inputType == "dictionary" ? "pxhtml2xpw-dict.xsl" : "pxhtml2xpw-scr.xsl";
			string xsltFullName = Common.FromRegistry(xslTemplateName);
            var xsltMap = new Dictionary<string, string>();
            LoadLanguage(processedXhtml, xsltMap);
            Common.XsltProcess(processedXhtml, xsltFullName, ".txt", xsltMap);
            string processFolder = Path.GetDirectoryName(processedXhtml);
            string texFullName = Common.PathCombine(processFolder, fileName + ".txt");
            File.Copy(texFullName, Common.PathCombine(dataPath, "Input.txt"), true);
        }
        #endregion CreateTexInput(string processedXhtml, string fileName, string dataPath)

        #region string GetTempCopy(string name)
        /// <summary>
        /// Makes a copy of folder in a writable location
        /// </summary>
        /// <returns>full path to folder</returns>
        protected static string GetTempCopy(string name)
        {
            var tempFolder = Path.GetTempPath();
            var folder = Path.Combine(tempFolder, name);
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);
			FolderTree.Copy(Common.FromRegistry(name), folder);
            return folder;
        }
        #endregion string GetXeTeXToolFolder()

        #region string GetMergedCSS(PublicationInformation projInfo)
        /// <summary>
        /// Preprocess css file merging all embedded css
        /// </summary>
        /// <param name="projInfo"></param>
        /// <returns>mergedCSS</returns>
        protected IPreExportProcess PreProcess(PublicationInformation projInfo)
        {
            IPreExportProcess preProcessor = new PreExportProcess(projInfo);
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
            return preProcessor;
        }
        #endregion string GetMergedCSS(PublicationInformation projInfo)

        #region SetupStartScript(string deToolFolder)
        /// <summary>
        /// Creates the startXPWtool.bat from the template
        /// </summary>
        /// <param name="deToolFolder">folder for XPWtool</param>
        protected static bool SetupStartScript(string deToolFolder)
        {
            var xCtxFolder = PathwayPath.GetCtxDir();
            if (string.IsNullOrEmpty(xCtxFolder))
                return false;
            if (xCtxFolder.EndsWith("\\"))
                xCtxFolder = xCtxFolder.Substring(0, xCtxFolder.Length - 1);
            _postscriptLanguage.RestoreCache();
            var sub = new Substitution { TargetPath = deToolFolder };
            var map = new Dictionary<string, string>();
            map["TexDrive"] = xCtxFolder.Substring(0, 1);
            map["TexFolder"] = xCtxFolder;
            map["ToolDrive"] = deToolFolder.Substring(0, 1);
            map["ToolFolder"] = deToolFolder;
            sub.FileSubstitute("startXPWtool-tpl.bat", map);
            return true;
        }
        #endregion SetupStartScript(string deToolFolder)

        #region AddImages(string processFolder, string dataPath)
        /// <summary>
        /// Copies images and converts to jpg
        /// </summary>
        /// <param name="processFolder">folder containing images after pre-process</param>
        /// <param name="dataPath">parent of image path for storing images</param>
        protected static void AddImages(string processFolder, string dataPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(processFolder);
            var imageFolder = Common.PathCombine(dataPath, "image");
            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                switch (Path.GetExtension(fileInfo.Name))
                {
                    case ".jpg":
                        File.Copy(fileInfo.FullName, Common.PathCombine(imageFolder, fileInfo.Name));
                        continue;
                    case ".tif":
                    case ".png":
                    case ".bmp":
                        var imageName = Path.GetFileNameWithoutExtension(fileInfo.Name) + ".jpg";
                        using (FileStream fileStream = new FileStream(Common.PathCombine(imageFolder, imageName), FileMode.CreateNew))
                        {
                            var image = Image.FromFile(fileInfo.FullName);
                            image.Save(fileStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        continue;
                    default:
                        continue;
                }
            }
        }
        #endregion AddImages(string processFolder, string dataPath)

        #region LoadLanguage(string processedXhtml, Dictionary<string, string> xsltMap)
        /// <summary>
        /// Determines language codes
        /// </summary>
        /// <param name="processedXhtml">xhtml input</param>
        /// <param name="xsltMap">dictionary where language tags will be stored</param>
        protected static void LoadLanguage(string processedXhtml, Dictionary<string, string> xsltMap)
        {
            XmlDocument xhtml = new XmlDocument {XmlResolver = null};
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xhtml.NameTable);
            //namespaceManager.AddNamespace("x", "http://www.w3.org/1999/xhtml");
            namespaceManager.AddNamespace("x", "");
            xhtml.Load(processedXhtml);
            ArrayList langs = new ArrayList();
            if (_inputType == "dictionary")
            {
                langs.Add(xhtml.SelectSingleNode("//x:span[@class='headword']/@lang", namespaceManager).Value);
                langs.Add(xhtml.SelectSingleNode("//x:span[@class='partofspeech']/@lang", namespaceManager).Value);
                _postscriptLanguage.SetLangClass(langs[0].ToString(), "headword");
                _postscriptLanguage.SetLangClass(langs[1].ToString(), "xitem_." + langs[1]);
            }
            else
            {
                langs.Add(xhtml.SelectSingleNode("//x:div[@class='Paragraph']/x:span/@lang", namespaceManager).Value);
                _postscriptLanguage.SetLangClass(langs[0].ToString(), "Paragraph");
            }
            XmlNodeList langNodes = xhtml.SelectNodes("//@lang", namespaceManager);
            Debug.Assert(langNodes != null);
            foreach (XmlNode xmlNode in langNodes)
                if (!langs.Contains(xmlNode.Value))
                {
                    langs.Add(xmlNode.Value);
                    _postscriptLanguage.SetLangClass(xmlNode.Value, "xitem_." + xmlNode.Value);
                }
            xsltMap["ver"] = langs[0].ToString();
            xsltMap["verFont"] = _postscriptLanguage.LangXpwFont(xsltMap["ver"]);
            for (int i = 1; i < langs.Count; i++)
            {
                string lang = langs[i].ToString();
                xsltMap["l" + i] = lang;
                xsltMap["l" + i + "Font"] = _postscriptLanguage.LangXpwFont(lang);
            }
        }
        #endregion LoadLanguage(string processedXhtml, Dictionary<string, string> xsltMap)
    }
}
