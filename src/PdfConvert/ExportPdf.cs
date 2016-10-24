// --------------------------------------------------------------------------------------------
// <copyright file="ExportPdf.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Export to pdf output
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
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
            {
                if (inputDataType.ToLower() == "dictionary" || inputDataType.ToLower() == "scripture")
                {
                    return true;
                }
            }
            else if (RegPrinceKey == null && Common.UnixVersionCheck())
            {
                if (Directory.Exists("/usr/lib/prince/bin"))
                {
                    return true;
                }
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
                        regPrinceKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MICROSOFT\WINDOWS\CURRENTVERSION\UNINSTALL\{3AC28E9C-8F06-4E2C-ADDA-726E2230A03A}");
                    if (regPrinceKey == null)
                        regPrinceKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\MICROSOFT\WINDOWS\CURRENTVERSION\UNINSTALL\Prince_is1");
                    if (regPrinceKey == null)
                        regPrinceKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\MICROSOFT\WINDOWS\CURRENTVERSION\UNINSTALL\{3AC28E9C-8F06-4E2C-ADDA-726E2230A03A}");

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
            bool isUnixOS = Common.UnixVersionCheck();
            //try
            //{
                var regPrinceKey = RegPrinceKey;
                if (regPrinceKey != null || isUnixOS)
                {
                    var curdir = Environment.CurrentDirectory;
                    projInfo.OutputExtension = "pdf";
                    PreExportProcess preProcessor = new PreExportProcess(projInfo);
                    if (isUnixOS)
                    {
                        projInfo.DefaultXhtmlFileWithPath =
                            Common.RemoveDTDForLinuxProcess(projInfo.DefaultXhtmlFileWithPath,"pdfconvert");
                    }
                    Environment.CurrentDirectory = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
                    preProcessor.IncludeHyphenWordsOnXhtml(preProcessor.ProcessedXhtml);
                    preProcessor.GetTempFolderPath();
                    preProcessor.ImagePreprocess(false, delegate(string s, string to) { ImageMods.ResizeImage(s, to, 1,1); });
                    preProcessor.ReplaceSlashToREVERSE_SOLIDUS();
                    if (projInfo.SwapHeadword)
                        preProcessor.SwapHeadWordAndReversalForm();
                    preProcessor.MovePictureAsLastChild(preProcessor.ProcessedXhtml);
                    preProcessor.SetNonBreakInVerseNumber(preProcessor.ProcessedXhtml);
                    preProcessor.ReplaceDoubleSlashToLineBreak(preProcessor.ProcessedXhtml);
                    preProcessor.MoveCallerToPrevText(preProcessor.ProcessedXhtml);
                    string tempFolder = Path.GetDirectoryName(preProcessor.ProcessedXhtml);
                    //string tempFolderName = Path.GetFileName(tempFolder);
                    var mc = new MergeCss { OutputLocation = tempFolder };

	                if (projInfo.IsReversalExist && File.Exists(projInfo.DefaultRevCssFileWithPath))
	                {
		                Common.CopyContent(projInfo.DefaultCssFileWithPath, projInfo.DefaultRevCssFileWithPath);
	                }

	                string mergedCSS = mc.Make(projInfo.DefaultCssFileWithPath, "Temp1.css");
                    preProcessor.ReplaceStringInCss(mergedCSS);
                    preProcessor.InsertPropertyInCSS(mergedCSS);
                    preProcessor.RemoveDeclaration(mergedCSS, ".pictureRight > .picture");
                    preProcessor.RemoveDeclaration(mergedCSS, "div.pictureLeft > img.picture");
                    preProcessor.HandleNewFieldworksChangeInCss(mergedCSS);
                    mergedCSS = preProcessor.RemoveTextIndent(mergedCSS);

                    if (isUnixOS)
                    {
                        Common.StreamReplaceInFile(mergedCSS, "Scheherazade Graphite Alpha", "Scheherazade");
                    }

                    Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
                    CssTree cssTree = new CssTree();
                    cssTree.OutputType = Common.OutputType.PDF;
                    cssClass = cssTree.CreateCssProperty(mergedCSS, true);
                    if (cssClass.ContainsKey("@page") && cssClass["@page"].ContainsKey("-ps-hide-versenumber-one"))
                    {
                        string value = cssClass["@page"]["-ps-hide-versenumber-one"];
                        if (value.ToLower() == "true")
                        {
                            preProcessor.RemoveVerseNumberOne(preProcessor.ProcessedXhtml, mergedCSS);
                        }
                    }
                    if (cssClass.ContainsKey("@page:left-top-left") && cssClass["@page:left-top-left"].ContainsKey("-ps-referenceformat"))
                    {
                        string value = cssClass["@page:left-top-left"]["-ps-referenceformat"];
                        if (value.ToLower().Contains("gen 1"))
                        {
                            ReplaceBookNametoBookCode(preProcessor.ProcessedXhtml);
                        }
                    }

                    string xhtmlFileName = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
                    string defaultCSS = Path.GetFileName(mergedCSS);
                    Common.SetDefaultCSS(preProcessor.ProcessedXhtml, defaultCSS);

                    _processedXhtml = preProcessor.ProcessedXhtml;
					if (projInfo.IsReversalExist)
					{
						var reversalFile = Path.GetDirectoryName(_processedXhtml);
						reversalFile = Common.PathCombine(reversalFile, "FlexRev.xhtml");
						Common.SetDefaultCSS(reversalFile, defaultCSS);
					}

                    if (!ExportPrince(projInfo, xhtmlFileName, isUnixOS, regPrinceKey, defaultCSS)) 
                        return false;

                    Environment.CurrentDirectory = curdir;
                    if (!projInfo.DefaultXhtmlFileWithPath.ToLower().Contains("local"))
                    {
                        //Copyright information added in PDF files
                        #pragma warning disable 168
                        Common.InsertCopyrightInPdf(Common.PathCombine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), xhtmlFileName + ".pdf"), "Prince XML", projInfo.ProjectInputType);
                        #pragma warning restore 168
                    }
                    else
                    {
                        string pdfFileName = xhtmlFileName + ".pdf";
                        pdfFileName = Common.PathCombine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), pdfFileName);
                        
                        if (!Common.Testing && File.Exists(pdfFileName))
                        {
                            // ReSharper disable RedundantAssignment
                            success = true;
                            // ReSharper restore RedundantAssignment
                            Process.Start(pdfFileName);
                        }
                    }
                    success = true;
                }
                else
                {
                    success = false;
                    if (Common.Testing)
                    {
                        success = true;
                    }
                }
            //}
            //catch (Exception)
            //{
            //    success = false;
            //}
            return success;
        }

	    private static bool ExportPrince(PublicationInformation projInfo, string xhtmlFileName, bool isUnixOS,
                                   RegistryKey regPrinceKey, string defaultCSS)
        {
            if (!isUnixOS)
            {
                Object princePath = regPrinceKey.GetValue("InstallLocation");
                _fullPrincePath = Common.PathCombine((string) princePath, "Engine/bin/prince.exe");
				var myPrince = new Prince(_fullPrincePath);
	            if (projInfo.IsReversalExist)
	            {
		            string[] xhtmlFiles = new string[2];
		            var reversalFile = Path.GetDirectoryName(_processedXhtml);
		            xhtmlFiles[0] = _processedXhtml;
		            xhtmlFiles[1] = Common.PathCombine(reversalFile, "FlexRev.xhtml");
		            myPrince.AddStyleSheet(defaultCSS);
					myPrince.ConvertMultiple(xhtmlFiles, xhtmlFileName + ".pdf");

	            }
	            else
	            {
		            if (File.Exists(_fullPrincePath))
		            {
			            myPrince.AddStyleSheet(defaultCSS);
			            myPrince.Convert(_processedXhtml, xhtmlFileName + ".pdf");

		            }
	            }
            }
            else
            {
                if (isUnixOS)
                {
                    if (!Directory.Exists("/usr/lib/prince/bin"))
                    {
                        return false;
                    }
                }
                Environment.CurrentDirectory = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
                Directory.SetCurrentDirectory(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath));

                string inputArguments;
	            if (projInfo.IsReversalExist)
				{
					var reversalFile = Path.GetDirectoryName(_processedXhtml);
					var reversalFilename = Common.PathCombine(reversalFile, "FlexRev.xhtml");
					inputArguments = "-s " + defaultCSS + " " + _processedXhtml + " " + reversalFilename + " " + " -o " + xhtmlFileName + ".pdf";
				}
				else
				{
					inputArguments = "-s " + defaultCSS + " " + _processedXhtml + " -o " + xhtmlFileName + ".pdf";	
				}
                
                using (Process p1 = new Process())
                {
                    p1.StartInfo.FileName = "prince";
                    if (File.Exists(_processedXhtml))
                    {
                        p1.StartInfo.Arguments = inputArguments;
                    }
                    p1.StartInfo.RedirectStandardOutput = true;
                    p1.StartInfo.RedirectStandardError = p1.StartInfo.RedirectStandardOutput;
                    p1.StartInfo.UseShellExecute = !p1.StartInfo.RedirectStandardOutput;
                    p1.Start();
                    p1.WaitForExit();
                }
            }
            return true;
        }


        /// <summary>
        /// Replace Bookname to Bookcode. if the css style have -ps-referenceFormat: Gen 1
        /// </summary>
        /// <param name="fileName"></param>
        public void ReplaceBookNametoBookCode(string fileName)
        {
            if (!File.Exists(fileName)) return;
            XmlDocument xDoc = Common.DeclareXMLDocument(true);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
            namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            xDoc.Load(fileName);
            string xPath = "//xhtml:span[@class='scrBookName']";
            XmlNodeList SectionNodeList = xDoc.SelectNodes(xPath, namespaceManager);
            if (SectionNodeList == null) return;
            for (int i = 0; i < SectionNodeList.Count; i++)
            {
                XmlNode paraNode = SectionNodeList[i].ParentNode;
                xPath = ".//xhtml:span[@class='scrBookCode']";
                XmlNodeList verseNodeList = paraNode.SelectNodes(xPath, namespaceManager);
                if (verseNodeList == null) return;
                if(verseNodeList.Count > 0)
                {
                    SectionNodeList.Item(i).InnerText = verseNodeList[0].InnerText;
                }
            }
            xDoc.Save(fileName);
        }
    }
}
