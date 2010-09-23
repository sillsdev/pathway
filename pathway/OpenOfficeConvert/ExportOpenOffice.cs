// --------------------------------------------------------------------------------------------
// <copyright file="ExportOpenOffice.cs" from='2009' to='2009' company='SIL International'>
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
using System.Windows.Forms;
using System.IO;
using System.Xml;
using SIL.PublishingSolution.Sort;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public class ExportOpenOffice : IExportProcess
    {
        #region Public Functions

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

        private PublicationInformation publicationInfo;
        Dictionary<string, string> _dictLexiconPrepStepsFilenames = new Dictionary<string, string>();
        Dictionary<string, string> _dictSectionNames = new Dictionary<string, string>();
        Dictionary<string, Dictionary<string, string>> _dictStepFilenames = new Dictionary<string, Dictionary<string, string>>();
        SortedDictionary<int, Dictionary<string, string>> _dictSorderSection = new SortedDictionary<int, Dictionary<string, string>>();
        ArrayList _odtFiles = new ArrayList();
        string _endQuote = ")";
        string _singleQuote = "'";
        string _comma = ",";

        public bool Export(PublicationInformation publicationInformation)
        {
            publicationInfo = publicationInformation;
            if (Path.GetExtension(publicationInformation.DefaultXhtmlFileWithPath) == ".lift")
            {
                publicationInformation.DefaultXhtmlFileWithPath = TransformLiftToXhtml(publicationInformation);
            }

            Dictionary<string, string> dictSecName = new Dictionary<string, string>();

            Dictionary<string, Dictionary<string, string>> cssClass = new Dictionary<string, Dictionary<string, string>>();
            CssTree cssTree = new CssTree();
            cssClass = cssTree.CreateCssProperty(publicationInformation.DefaultCssFileWithPath, true);

            if(cssClass.ContainsKey("@page") && cssClass["@page"].ContainsKey("-ps-fileproduce"))
            {
                    publicationInformation.FileToProduce = cssClass["@page"]["-ps-fileproduce"];
            }

            if (publicationInformation.FromPlugin)
            {
                dictSecName = CreatePageDictionary(publicationInformation);
            }
            else
            {
                dictSecName = GetPageSectionSteps();
            }

            dictSecName = SplitXhtmlAsMultiplePart(publicationInformation, dictSecName);


            if (dictSecName.Count > 0)
            {
                ExportWithDocumentSections(publicationInfo.ProgressBar);
            }
            else
            {
                publicationInfo.DictionaryOutputName = publicationInfo.ProjectName;
                ExportOneSection(publicationInfo);
            }
            return true;
        }

        private Dictionary<string, string> SplitXhtmlAsMultiplePart(PublicationInformation publicationInformation, Dictionary<string, string> dictSecName)
        {
            if (publicationInformation.FileToProduce.ToLower() != "one")
            {
                List<string> fileNameWithPath = new List<string>();
                if(publicationInformation.FileToProduce.ToLower() == "one per book")
                {
                    fileNameWithPath = Common.SplitXhtmlFile(publicationInfo.DefaultXhtmlFileWithPath,
                                                             "scrbook", false);
                }
                else if(publicationInformation.FileToProduce.ToLower() == "one per letter")
                {
                    fileNameWithPath = Common.SplitXhtmlFile(publicationInfo.DefaultXhtmlFileWithPath,
                                                             "letHead", true);
                }
                 
                if (fileNameWithPath.Count > 0)
                {
                    dictSecName = CreateJoiningForSplited(fileNameWithPath);
                    publicationInformation.DictionaryOutputName = null;
                }
            }
            return dictSecName;
        }

        public void SetPublication(PublicationInformation pubInfo)
        {
            publicationInfo = pubInfo;
        }

        public string TransformLiftToXhtml(PublicationInformation pubInfo)
        {
            string entryFilterXpath = "true()";
            string senseFilterXpath = "true()";
            string languageFilterXpath = "true()";
            string liftSupportPath = Path.Combine(Common.GetPSApplicationPath(), "liftSupport");

            string inputFile = pubInfo.DefaultXhtmlFileWithPath;
            string xhtmlFile = inputFile;
            string transFormfile = Path.Combine(liftSupportPath, "liftTransform.xsl");
            string filterFile = Path.Combine(liftSupportPath, "liftEntryAndSenseFilter.xsl");
            string newTempXslfile = Path.Combine(Path.GetTempPath(), "tempFilter.xsl");
            string newTempXslfile1 = Path.Combine(Path.GetTempPath(), "tempFilter1.xsl");


            string languageSortFile = Path.Combine(Path.GetTempPath(), "langSortedFile.xhtml");
            try
            {
                // Transformation files

                // Filter starts
                if (pubInfo.IsSenseFilter)
                {
                    senseFilterXpath = GetSenseFilterXpath();
                }
                if (pubInfo.IsEntryFilter)
                {
                    entryFilterXpath = GetEntryFilterXpath();
                }
                if (pubInfo.IsLanguageFilter)
                {
                    languageFilterXpath = GetLanguageFilterXpath();
                }

                string xmlFile = Path.ChangeExtension(inputFile, "xml");
                string lXmlFile = Path.ChangeExtension(inputFile, "lxml");

                xhtmlFile = Path.ChangeExtension(inputFile, "xhtml");
                File.Copy(filterFile, newTempXslfile, true);

                //Replace the filter key strings and Transformation takes place
                ReplaceFilters("entry", newTempXslfile, entryFilterXpath);
                ReplaceFilters("sense", newTempXslfile, senseFilterXpath);

                Common.XsltProcess(inputFile, newTempXslfile, ".xml");

                if (pubInfo.IsLanguageFilter)
                {
                    ReplaceNamespace(xmlFile);
                    string langFilterFile = Path.Combine(liftSupportPath, "liftLangFilter.xsl");
                    File.Copy(langFilterFile, newTempXslfile1, true);

                    ReplaceFilters("language", newTempXslfile1, languageFilterXpath);
                    Common.XsltProcess(xmlFile, newTempXslfile1, ".lxml");
                    xmlFile = lXmlFile;
                }

                ReplaceNamespace(xmlFile);
                Common.XsltProcess(xmlFile, transFormfile, ".xhtml");

                if (pubInfo.IsLanguageSort)
                {
                    bool sorted = LiftSortWritingSys(xhtmlFile, languageSortFile);
                    if (sorted)
                    {
                        xhtmlFile = languageSortFile;
                    }
                }

                if (pubInfo.IsEntrySort)
                {
                    LiftPreparer lp = new LiftPreparer();
                    lp.loadLift(pubInfo.DefaultXhtmlFileWithPath);
                    lp.applySort();
                    lp.getCurrentLift();
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            //MessageBox.Show("Finished");
            return xhtmlFile;
        }

        public bool LiftSortWritingSys(string fileName, string outputfile)
        {
            bool returnValue = true;
            try
            {
                var liftDoc = new LiftDocument(fileName);
                var sorter = new LiftLangSorter(liftDoc);
                sorter.sortWritingSystems();
                var writer = new LiftWriter(outputfile);
                liftDoc.Save(writer);
                writer.Close();
            }
            catch (Exception)
            {
                returnValue = false;
            } 
            return returnValue;
        }
        private void ReplaceFilters(string key,string newTempXslfile, string filterXpath)
        {
            const string entryFilterParameter = "EntFltParam";
            const string senseFilterParameter = "SenseFltParam";
            const string langFilterParameter = "LangFltParam";

            string filterParameter = string.Empty;
            switch (key.ToLower())
            {
                case "entry":
                    filterParameter = entryFilterParameter;
                    break;
                case "sense":
                    filterParameter = senseFilterParameter;
                    break;
                case "language":
                    filterParameter = langFilterParameter;
                    break;

            }
            Common.ReplaceInFile(newTempXslfile, filterParameter, filterXpath);
        }

        private void ReplaceNamespace(string xmlFile)
        {
            string nameSpace = "xmlns=\"http://www.w3.org/1999/XSL/Transform\"";
            Common.ReplaceInFile(xmlFile, nameSpace, ""); // replacing the namespace with empty
        }


        private string GetFilterXpath(string mid, string forEntry)
        {
            string filterString = string.Empty;
            string searchKey = string.Empty;
            bool matchCase = false;
            string comma = _comma;
            string endQuote = _endQuote;
            if (forEntry.ToLower() == "entry" && publicationInfo.IsEntryFilter)
            {
                filterString = publicationInfo.EntryFilterString;
                searchKey = publicationInfo.EntryFilterKey;
                matchCase = publicationInfo.IsEntryFilterMatchCase;

            }
            else if (forEntry.ToLower() == "sense" && publicationInfo.IsSenseFilter)
            {
                filterString = publicationInfo.SenseFilterString;
                searchKey = publicationInfo.SenseFilterKey;
                matchCase = publicationInfo.IsSenseFilterMatchCase;
            }
            else if (forEntry.ToLower() == "language" && publicationInfo.IsLanguageFilter)
            {
                filterString = publicationInfo.LanguageFilterString;
                searchKey = publicationInfo.LanguageFilterKey;
                matchCase = publicationInfo.IsLanguageFilterMatchCase;
            }

            string searchKeyValue;

            //if (!publicationInfo.FilterMatchCase)
            if (!matchCase)
            {
                mid = SubstitueTranslateWithLowerCase(mid);
                string filterStringwithQuote = _singleQuote + filterString + _singleQuote;
                searchKeyValue = SubstitueTranslateWithLowerCase(filterStringwithQuote);
            }
            else // as it is - Match Case
            {
                searchKeyValue = _singleQuote + filterString + _singleQuote;
            }

            //switch (publicationInfo.SenseFilterKey.ToLower())
            switch (searchKey.ToLower())
            {
                case "at start":
                    searchKey = "starts-with(";
                    break;
                case "at end": // not working in 1.0 works in 2.0
                    //substring(length- e.length,e.length)
                    mid = "substring(" + mid + ", string-length(" + mid + ") - string-length(" + searchKeyValue + ") +1,string-length(" +
                          searchKeyValue + "))";
                    searchKey = "starts-with(";
                    break;
                case "anywhere":
                    searchKey = "contains(";
                    break;
                case "not equal":
                    endQuote = string.Empty;
                    searchKey = string.Empty;
                    comma = " != "; 
                    break;

                case "whole item":
                    endQuote = string.Empty;
                    searchKey = string.Empty;
                    comma = " = ";
                    break;
            }
            string filterXpath = searchKey + mid + comma + searchKeyValue + endQuote;
            return filterXpath;
        }

        private string GetSenseFilterXpath()
        {
            string mid = "$pentry/sense[$senseno]/definition//text"; // using variable for sense search
            string senseFilterXpath = GetFilterXpath(mid, "sense");
            return senseFilterXpath;
        }

        private string GetEntryFilterXpath()
        {
            const string mid = "lexical-unit/form/text";
            string filterXpath = GetFilterXpath(mid, "entry");
            return filterXpath;
        }

        private string GetLanguageFilterXpath()
        {
            const string mid = "@lang";
            string filterXpath = GetFilterXpath(mid, "language");
            return filterXpath;
        }

        private string SubstitueTranslateWithLowerCase(string input)
        {
            const string translateCommand = "translate(";
            string lowercase = _comma + "$lowercase";
            string uppercase = _comma + "$uppercase";
            input = translateCommand + input + uppercase + lowercase + _endQuote;
            return input;
        }

        private Dictionary<string, string> CreatePageDictionary(PublicationInformation projInfo)
        {
            Dictionary<string, string> dictSecName = new Dictionary<string, string>();
            if (projInfo.IsLexiconSectionExist && projInfo.IsReversalExist)
            {
                _dictSorderSection.Clear();
                dictSecName["Main"] = "main.xhtml";
                dictSecName["Reversal"] = "FlexRev.xhtml";
                _dictSorderSection[1] = dictSecName;
            }
            return dictSecName;
        }

        private Dictionary<string, string> CreateJoiningForSplited(List<string> fileNames)
        {
            Dictionary<string, string> dictSecName = new Dictionary<string, string>();
              if (fileNames.Count> 0)
            
            {
                bool mainAdded = false;
                int fileCount = 1;
                //_dictSorderSection.Clear();
                foreach (string s in fileNames)
                {
                    if (!mainAdded)
                    {
                        dictSecName["Main"] = s;
                        mainAdded = true;
                    }
                    else
                    { dictSecName["Main" + fileCount++] = s; }
                }
                _dictSorderSection[1] = dictSecName;
            }
            return dictSecName;
        }
        /// <summary>
        /// Get the Document Sections 
        /// </summary>
        private Dictionary<string, string> GetPageSectionSteps()
        {
            XmlDocument projXml = new XmlDocument();
            Dictionary<string, string> dictSecName = new Dictionary<string, string>();
            string xPath = "";
            XmlElement root = null;
            XmlNode tempNodeSearch = null;

            if (publicationInfo.ProjectFileWithPath == null)
                return dictSecName;
            projXml.Load(publicationInfo.ProjectFileWithPath);
            root = projXml.DocumentElement;

            xPath = "DocumentSettings/Sections";
            tempNodeSearch = root.SelectSingleNode(xPath);   // Default to Document Settings
            if (tempNodeSearch != null)
            {
                for (int i = 0; i < tempNodeSearch.ChildNodes.Count; i++)
                {
                    string attName = "";
                    string attValue = "";
                    int attNo = 0;
                    for (int j = 0; j < tempNodeSearch.ChildNodes[i].Attributes.Count; j++)
                    {
                        string attString = tempNodeSearch.ChildNodes[i].Attributes[j].Name.ToString();
                        if (attString == "Name")
                        {
                            attName = tempNodeSearch.ChildNodes[i].Attributes[j].Value.ToString(); ;
                        }
                        else if (attString == "Value")
                        {
                            attValue = tempNodeSearch.ChildNodes[i].Attributes[j].Value.ToString();
                        }
                        else if (attString == "No")
                        {
                            attNo = int.Parse(tempNodeSearch.ChildNodes[i].Attributes[j].Value);
                        }


                        if (tempNodeSearch.ChildNodes[i].HasChildNodes)
                        {
                            XmlNode childNodes = tempNodeSearch.ChildNodes[i];
                            for (int ii = 0; ii < childNodes.ChildNodes.Count; ii++)
                            {
                                string attChildName = "";
                                string attChildValue = "";
                                for (int jj = 0; jj < childNodes.ChildNodes[ii].Attributes.Count; jj++)
                                {
                                    string attChildString = childNodes.ChildNodes[ii].Attributes[jj].Name.ToString();
                                    if (attChildString == "Name")
                                    {
                                        attChildName = childNodes.ChildNodes[ii].Attributes[jj].Value.ToString(); ;
                                    }
                                    else if (attChildString == "Value")
                                    {
                                        attChildValue = childNodes.ChildNodes[ii].Attributes[jj].Value.ToString();
                                    }
                                }
                                if (attName == "Main")
                                {
                                    _dictLexiconPrepStepsFilenames[attChildName] = attChildValue;
                                }
                                else
                                {
                                    Dictionary<string, string> tempDict = new Dictionary<string, string>();
                                    tempDict[attChildName] = attChildValue;
                                    _dictStepFilenames[attName] = tempDict;
                                }
                            }
                        }
                    }
                    _dictSectionNames = new Dictionary<string, string>();
                    _dictSectionNames[attName] = attValue;
                    _dictSorderSection[attNo] = _dictSectionNames;
                    if (attValue.ToLower() != "none")
                    {
                        dictSecName[attName] = attValue;
                    }
                }
            }
            return dictSecName;
        }

        private void ExportWithDocumentSections(ProgressBar statusProgressBar)
        {
            Common.ShowMessage = false;
            _odtFiles.Clear();
            var exportProcess = new ExportOpenOffice();
            string LexiconFileName = string.Empty;

            foreach (KeyValuePair<int, Dictionary<string, string>> keyvalue in _dictSorderSection)
            {
                var Sections = keyvalue.Value;
                foreach (var subSection in Sections)
                {
                    if (subSection.Value != "none")
                    {
                        //string fileName = Path.GetFileName(subSection.Value);
                        string fileName = subSection.Value;
                        publicationInfo.AddFileToXML(fileName, "False", true, "", false, true);
                        fileName = Common.PathCombine(publicationInfo.DictionaryPath, Path.GetFileName(fileName));
                        string fileExt = Path.GetExtension(fileName);
                        string xslFileName = "";
                        bool generated = false;

                        switch (fileExt)
                        {
                            case ".xml":
                                if (subSection.Key != "Main")
                                {
                                    XslProcess(subSection, fileName, xslFileName, exportProcess, statusProgressBar);
                                }
                                break;
                            case ".xhtml":
                                if (subSection.Key == "Main")
                                {
                                    LexiconFileName = fileName;
                                }
                                publicationInfo.DefaultXhtmlFileWithPath = fileName;
                                publicationInfo.ProgressBar = statusProgressBar;
                                publicationInfo.IsOpenOutput = false;
                                generated = ExportOneSection(publicationInfo);
                                if (generated)
                                {
                                    string returnFileName = Path.GetFileName(fileName);
                                    _odtFiles.Add(Path.ChangeExtension(returnFileName, ".odt"));
                                }
                                break;

                            case ".lift":
                                if (subSection.Key == "Main")
                                {
                                    LexiconFileName = fileName;
                                }
                                XslProcess(subSection, fileName, xslFileName, exportProcess, statusProgressBar);

                                publicationInfo.DefaultXhtmlFileWithPath = fileName;
                                break;

                            case ".odt":
                                _odtFiles.Add(Path.GetFileName(fileName));
                                break;

                            default:
                                break;
                        }
                    }
                    else if (subSection.Key == "Blank")
                    {
                        _odtFiles.Add("Blank.odt");  // Blank ODT Files
                    }
                }
            }

            // Finally run the ODM file 
            Common.ShowMessage = true; // used to control MessageBox;
            publicationInfo.DefaultXhtmlFileWithPath = LexiconFileName;
            publicationInfo.DictionaryOutputName = publicationInfo.ProjectName;
            publicationInfo.ProgressBar = statusProgressBar;
            publicationInfo.FileSequence = _odtFiles;
            publicationInfo.IsOpenOutput = true;
            ExportOneSection(publicationInfo);
        }

        private void XslProcess(KeyValuePair<string, string> subSection, string fileName, string xslFileName, ExportOpenOffice exportOpenOffice, ProgressBar statusProgressBar)
        {
            bool generated;
            if (_dictStepFilenames.ContainsKey(subSection.Key))
            {
                var tempDict1 = new Dictionary<string, string>();
                tempDict1 = _dictStepFilenames[subSection.Key];
                foreach (KeyValuePair<string, string> tempString in tempDict1)
                {
                    xslFileName = tempString.Value;
                }
            }

            if (_dictLexiconPrepStepsFilenames.Count > 0)
            {
                xslFileName = _dictLexiconPrepStepsFilenames["Filter Entries"];
            }
            string returnFileName = Common.XsltProcess(fileName, xslFileName, ".xhtml");
            if (!Path.IsPathRooted(returnFileName))
            {
                var msg = new[] { returnFileName };
                LocDB.Message("defErrMsg", returnFileName, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
            }

            if (returnFileName != "")
            {
                //generated = exportOpenOffice.Export(statusProgressBar, projectInfo.DictionaryPath, returnFileName, projectInfo.DefaultCssFileWithPath, false, null, projectInfo.ProjectInputType);
                publicationInfo.ProgressBar = statusProgressBar;
                publicationInfo.IsOpenOutput = false;
                generated = ExportOneSection(publicationInfo);

                if (generated)
                {
                    returnFileName = Path.GetFileName(returnFileName);
                    _odtFiles.Add(Path.ChangeExtension(returnFileName, ".odt"));
                }
            }
        }


        /// <summary>
        /// Convert XHTML to ODT
        /// </summary>
        public bool ExportOneSection(PublicationInformation projInfo)
        {
            string defaultXhtml = projInfo.DefaultXhtmlFileWithPath;
            //string fileType = "odt";
            projInfo.OutputExtension = "odt";
            Common.OdType = Common.OdtType.OdtChild;
            bool returnValue = false;
            string strFromOfficeFolder = Common.PathCombine(Common.GetPSApplicationPath(), "OfficeFiles" + Path.DirectorySeparatorChar + projInfo.ProjectInputType);
            projInfo.TempOutputFolder = Common.PathCombine(Path.GetTempPath(), "OfficeFiles" + Path.DirectorySeparatorChar + projInfo.ProjectInputType);
            string strStylePath = Common.PathCombine(projInfo.TempOutputFolder, "styles.xml");
            string strContentPath = Common.PathCombine(projInfo.TempOutputFolder, "content.xml");
            CopyOfficeFolder(strFromOfficeFolder, projInfo.TempOutputFolder);
            //string strMacroPath = Common.PathCombine(projInfo.TempOutputFolder, "Basic/Standard/Module1.xml");
            string strMacroPath = Common.PathCombine(projInfo.TempOutputFolder, "Basic/Standard/Module1.xml");
            string outputFileName;
            string outputPath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            VerboseClass verboseClass = VerboseClass.GetInstance();
            if (projInfo.FileSequence != null && projInfo.FileSequence.Count > 1)
            {
                projInfo.OutputExtension = "odm";  // Master Document
                Common.OdType = Common.OdtType.OdtMaster;
                if (projInfo.DictionaryOutputName == null)
                    projInfo.DictionaryOutputName = projInfo.DefaultXhtmlFileWithPath.Replace(Path.GetExtension(projInfo.DefaultXhtmlFileWithPath), "");
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

            //Include FlexRev.css when XHTML is FlexRev.xhtml
            string FlexRev = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
            if (FlexRev.ToLower() == "flexrev")
            {
                string revCSS = Path.Combine(Path.GetDirectoryName(projInfo.DefaultCssFileWithPath), "FlexRev.css");
                if (File.Exists(revCSS))
                    projInfo.DefaultCssFileWithPath = revCSS;
            }

            // BEGIN Generate Styles.Xml File
            var sXML = new StylesXML();
            Styles styleName = sXML.CreateStyles(projInfo.DefaultCssFileWithPath, strStylePath, projInfo.DefaultXhtmlFileWithPath,
                                                 false);
            //To set Constent variables for User Desire
            IncludeTextinMacro(strMacroPath, styleName.ReferenceFormat, Common.PathCombine(projInfo.DictionaryPath, Path.GetFileNameWithoutExtension(projInfo.DictionaryPath)), projInfo.FinalOutput);

            // BEGIN Generate Meta.Xml File
            var metaXML = new MetaXML();
            metaXML.CreateMeta(projInfo);
            PreExportProcess preProcessor = new PreExportProcess(projInfo);
            // BEGIN Generate Content.Xml File 
            var cXML = new ContentXML();
            string fileName = Path.Combine(projInfo.DictionaryPath, Path.GetFileName(projInfo.DefaultXhtmlFileWithPath));
            preProcessor.GetTempFolderPath();
            preProcessor.ImagePreprocess();
            preProcessor.ReplaceSlashToREVERSE_SOLIDUS();
            if (projInfo.SwapHeadword)
                preProcessor.SwapHeadWordAndReversalForm();
            projInfo.DefaultXhtmlFileWithPath = preProcessor.ProcessedXhtml;

            //cXML.CreateContent(projInfo.ProgressBar, projInfo.DefaultXhtmlFileWithPath, strToOfficeFolder + Path.DirectorySeparatorChar,
            //                   styleName, fileType);
            projInfo.TempOutputFolder += Path.DirectorySeparatorChar;
            cXML.CreateContent(projInfo, styleName);

            if (projInfo.FileSequence != null && projInfo.FileSequence.Count > 1)
            {
                var ul = new Utility();
                ul.CreateMasterContents(strContentPath, projInfo.FileSequence);
            }
            
            if (projInfo.MoveStyleToContent) 
                MoveStylesToContent(strStylePath, strContentPath);

            var mODT = new ZipFolder();
            string fileNameNoPath = outputFileName + "." + projInfo.OutputExtension;
            mODT.CreateZip(projInfo.TempOutputFolder, fileNameNoPath, verboseClass.ErrorCount);

            try
            {
                if (File.Exists(fileNameNoPath))
                {
                    returnValue = true;
                    if (projInfo.IsOpenOutput)
                    {
                        Common.OpenOutput(fileNameNoPath);
                    }
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == 1155)
                {
                    var msg = new[] { "OpenOffice application from http://www.openoffice.org site.\nAfter downloading and installing Open Office, please consult release notes about how to change the macro security setting to enable the macro that creates the headers." };
                    LocDB.Message("errInstallFile", "Please install " + msg, msg, LocDB.MessageTypes.Error, LocDB.MessageDefault.First);
                    return false;
                }
            }
            projInfo.DefaultXhtmlFileWithPath = defaultXhtml ;
            //try
            //{
            //    File.Delete(processedXhtml);
            //}
            //catch(Exception ex)
            //{
            //    Console.Write(ex.Message);
            //}
            return returnValue;
        }

        private static void MoveStylesToContent(string strStylePath, string strContentPath)
        {
            string[] s =  {
             "GuideL",
             "GuideR",
             "headword",
             "headwordminor",
              "entry_letData",
             "letter",
             "homographnumber"
            };

            List<string> macroList = new List<string>();
            macroList.AddRange(s);

            var doc = new XmlDocument();
            doc.Load(strStylePath);
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
            nsmgr.AddNamespace("of", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");

            // if new stylename exists
            XmlElement root1 = doc.DocumentElement;
            string officeNode = "//of:styles";
            string allStyles = string.Empty;
            //string style = "//st:style[@st:name='" + makeClassName + "']";

            XmlNode node = root1.SelectSingleNode(officeNode, nsmgr); // work

            if (node != null)
            {
                int nodeCount = node.ChildNodes.Count;
                for (int i = 0; i < nodeCount; i++)
                {

                    XmlNode name = node.ChildNodes[i].Attributes.GetNamedItem("name", nsmgr.LookupNamespace("st"));
                    if (name != null)
                    {
                        if (name.Value.ToLower() != "entry_letdata" && name.Value.ToLower() != "Text_20_body")
                        {
                            allStyles += node.ChildNodes[i].OuterXml;
                            node.ChildNodes[i].ParentNode.RemoveChild(node.ChildNodes[i]);

                            i--;
                            nodeCount--;
                        }
                    }
                }
            }
            doc.Save(strStylePath);

            /////

            var docContent = new XmlDocument();
            docContent.Load(strContentPath);
            var nsmgr1 = new XmlNamespaceManager(docContent.NameTable);
            nsmgr1.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr1.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
            nsmgr1.AddNamespace("of", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");


            // if new stylename exists
            XmlElement root2 = docContent.DocumentElement;
            string officeNode1 = "//of:automatic-styles";
            if (root2 != null)
            {
                XmlNode node1 = root2.SelectSingleNode(officeNode1, nsmgr); 
                if (node1 != null)
                {
                    node1.InnerXml = node1.InnerXml + allStyles;
               }
                docContent.Save(strContentPath);
            }
        }

        private static void IncludeTextinMacro(string strMacroPath, string ReferenceFormat, string saveAsPath, string finalOutput)
        {
            var xmldoc = new XmlDocument { XmlResolver = null };
            xmldoc.Load(strMacroPath);
            XmlElement ele = xmldoc.DocumentElement;
            string autoMacro = "False";
            if (Param.Value.Count > 0)
            {
                if (Param.Value[Param.ExtraProcessing] != "ExtraProcessing")
                {
                    autoMacro = Param.Value[Param.ExtraProcessing];
                }
                if (ele != null)
                {
                    string seperator = "\n";
                    string line1 = "\n'Constant ReferenceFormat for User Desire\nConst ReferenceFormat = \"" +
                                   ReferenceFormat + "\"" + "\nConst AutoMacro = \"" + autoMacro + "\"";
                    string line2 = "\nConst OutputFormat = \"" + finalOutput + "\"" + "\nConst FilePath = \"" + saveAsPath + "\"" ;
                    string combined = line1 + line2 + seperator;

                    ele.InnerText = combined + ele.InnerText;
                }
            }
            xmldoc.Save(strMacroPath);
        }

        #endregion

        #region Private Functions
        /// <summary>
        /// To copy Temporary Office files to Environmental Temp Folder instead of keeping changes in Application Itself.
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destFolder"></param>
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
        #endregion
    }
}
