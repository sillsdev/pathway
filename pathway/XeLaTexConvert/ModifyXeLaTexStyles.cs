using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ModifyXeLaTexStyles
    {
        #region Private Variables

        private XmlDocument _styleXMLdoc;
        private XmlNode _node;
        private XmlElement _root;
        private XmlNamespaceManager nsmgr;
        const string _styleSeperator = "_";
        private string _projectPath;
        private string _tagType;
        private string _xPath;
        private XmlElement _nameElement;
        private string _tagName;
        private string _pageStyleFormat;
        private string _xetexFullFile;
        private bool _isHeadword;
        private string _projectType;
        private ArrayList _textVariables = new ArrayList();
        private List<string> _mergedClass = new List<string>();
        Dictionary<string, string> _languageStyleName = new Dictionary<string, string>();
        Dictionary<string, Dictionary<string, string>> _childStyle = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> _tempStyle;
        Dictionary<string, Dictionary<string, string>> mergedStyle = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, Dictionary<string, string>> _cssClass = new Dictionary<string, Dictionary<string, string>>();
        XeLaTexMapProperty mapProperty = new XeLaTexMapProperty();
        string _firstString = string.Empty;
        string _lastString = string.Empty;
        private string _headWordStyleName = string.Empty;
        private string _tocChecked = "false";
        private string _coverImage = "false";
        private string _titleInCoverPage = "false";
        private string _copyrightInformation = "false";
        private string _includeBookTitleintheImage = "false";

        private string _copyrightInformationPagePath;
        private string _coverPageImagePath;
        private bool _xelatexDocumentOpenClosedRequired = false;
        private bool _copyrightTexCreated = false;
        private string _copyrightTexFilename = string.Empty;
        private string _reversalIndexTexFilename = string.Empty;
        private bool _reversalIndexExist = false;
        private bool _isMirrored = false;
        private Dictionary<string, string> _langFontDictionary;
        private Dictionary<string, Dictionary<string, string>> _tocList;
        public string ProjectType
        {
            get { return _projectType; }
            set { _projectType = value; }
        }

        public string TocChecked
        {
            get { return _tocChecked; }
            set { _tocChecked = value; }
        }

        public string CoverImage
        {
            get { return _coverImage; }
            set { _coverImage = value; }
        }

        public string TitleInCoverPage
        {
            get { return _titleInCoverPage; }
            set { _titleInCoverPage = value; }
        }

        public string CopyrightInformation
        {
            get { return _copyrightInformation; }
            set { _copyrightInformation = value; }
        }

        public string IncludeBookTitleintheImage
        {
            get { return _includeBookTitleintheImage; }
            set { _includeBookTitleintheImage = value; }
        }

        public string CopyrightInformationPagePath
        {
            get { return _copyrightInformationPagePath; }
            set { _copyrightInformationPagePath = value; }
        }

        public string CoverPageImagePath
        {
            get { return _coverPageImagePath; }
            set { _coverPageImagePath = value; }
        }

        public bool CopyrightTexCreated
        {
            get { return _copyrightTexCreated; }
            set { _copyrightTexCreated = value; }
        }

        public bool ReversalIndexExist
        {
            get { return _reversalIndexExist; }
            set { _reversalIndexExist = value; }
        }

        public string ReversalIndexTexFilename
        {
            get { return _reversalIndexTexFilename; }
            set { _reversalIndexTexFilename = value; }
        }

        public string CopyrightTexFilename
        {
            get { return _copyrightTexFilename; }
            set { _copyrightTexFilename = value; }
        }

        public bool XelatexDocumentOpenClosedRequired
        {
            get { return _xelatexDocumentOpenClosedRequired; }
            set { _xelatexDocumentOpenClosedRequired = value; }
        }


        public string Title { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Relation { get; set; }
        public string Coverage { get; set; }
        public string Rights { get; set; }
        public string Format { get; set; }
        public string Source { get; set; }

        public Dictionary<string, string> LangFontDictionary
        {
            get { return _langFontDictionary; }
            set { _langFontDictionary = value; }
        }

        #endregion

        public void ModifyStylesXML(string projectPath, StreamWriter xetexFile, Dictionary<string, Dictionary<string, string>> newProperty,
            Dictionary<string, Dictionary<string, string>> cssClass, string xetexFullFile, string pageStyleFormat, Dictionary<string, string> langFontDictionary)
        {
            _langFontDictionary = langFontDictionary;
            _projectPath = projectPath;
            _cssClass = cssClass;
            _xetexFullFile = xetexFullFile;
            _pageStyleFormat = pageStyleFormat;
            //foreach (KeyValuePair<string, Dictionary<string, string>> cssStyle in newProperty)
            //{
            //    MergeCssStyle(cssStyle.Key);
            //}
            ValidatePageType();
            GetTableofContent(newProperty);
            MapProperty();
        }

        private void ValidatePageType()
        {
            if (_cssClass.ContainsKey("@page:left-top-left"))
            {
                _isMirrored = true;
            }
        }

        private void GetTableofContent(Dictionary<string, Dictionary<string, string>> newProperty)
        {
            if (newProperty.ContainsKey("TableofContent"))
            {
                _tocList = newProperty;

                //if (_projectType != null && _projectType != "Scripture")
                //{
                //    _firstString = newProperty["TableofContent"]["first"];
                //    _lastString = newProperty["TableofContent"]["last"];
                //    _headWordStyleName = newProperty["TableofContent"]["stylename"];
                //}
            }
        }

        private void MapProperty()
        {
            string newFile1 = _xetexFullFile.Replace(".tex", "1.tex");
            string newFile2 = _xetexFullFile.Replace(".tex", "2.tex");

            File.Copy(_xetexFullFile, newFile1, true);

            StreamWriter sw = new StreamWriter(newFile2);

            string xeLaTexProp = "";
            List<string> includePackageList = new List<string>();
            List<string> xeLaTexProperty = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _cssClass)
            {
                if (cssClass.Key.IndexOf("h1") >= 0 ||
                    cssClass.Key.IndexOf("h2") >= 0 || cssClass.Key.IndexOf("h3") >= 0 ||
                    cssClass.Key.IndexOf("h4") >= 0 || cssClass.Key.IndexOf("h5") >= 0 ||
                    cssClass.Key.IndexOf("h6") >= 0) continue;
                List<string> inlineStyle = new List<string>();
                List<string> inlineInnerStyle = new List<string>();
                string replaceNumberInStyle = Common.ReplaceCSSClassName(cssClass.Key);
                string className = RemoveBody(replaceNumberInStyle);
                if (className.Length == 0) continue;
                xeLaTexProp = mapProperty.XeLaTexProperty(cssClass.Value, className, inlineStyle, includePackageList, inlineInnerStyle, _langFontDictionary);
                if (xeLaTexProp.Trim().Length > 0)
                {
                    xeLaTexProperty.Add(xeLaTexProp);
                }
            }

            if (!XelatexDocumentOpenClosedRequired)
            {
                string paperSize = GetPageStyle(_cssClass, _isMirrored);
                sw.WriteLine(@"\documentclass" + paperSize);

                double pageTopMargin = 0;
                double pageBottomMargin = 0;
                double pageLeftMargin = 0;
                double pageRightMargin = 0;
                if (_cssClass.ContainsKey("@page"))
                {
                    Dictionary<string, string> cssProp = _cssClass["@page"];
                    foreach (KeyValuePair<string, string> para in cssProp)
                    {
                        if (para.Key == "margin-top")
                        {
                            pageTopMargin = Convert.ToDouble(para.Value);
                        }
                        if (para.Key == "margin-bottom")
                        {
                            pageBottomMargin = Convert.ToDouble(para.Value);

                        }
                        if (para.Key == "margin-left")
                        {
                            pageLeftMargin = Convert.ToDouble(para.Value);

                        }
                        if (para.Key == "margin-right")
                        {
                            pageRightMargin = Convert.ToDouble(para.Value);

                        }

                        if (para.Key == "margin")
                        {
                            pageTopMargin = Convert.ToDouble(para.Value);
                            pageBottomMargin = Convert.ToDouble(para.Value);
                            pageLeftMargin = Convert.ToDouble(para.Value);
                            pageRightMargin = Convert.ToDouble(para.Value);
                            break;
                        }
                    }
                }

                foreach (var package in includePackageList)
                {
                    sw.WriteLine(package);
                }

                sw.WriteLine(@"\usepackage{float}");
                sw.WriteLine(@"\usepackage{grffile}");
                sw.WriteLine(@"\usepackage{graphicx}");
                sw.WriteLine(@"\usepackage{amssymb}");
                sw.WriteLine(@"\usepackage{fontspec}");
                sw.WriteLine(@"\usepackage{fancyhdr}");
                sw.WriteLine(@"\usepackage{multicol}");
                sw.WriteLine(@"\usepackage{calc}");
                sw.WriteLine(@"\usepackage{lettrine}");


                if (pageTopMargin != 0 || pageBottomMargin != 0 || pageLeftMargin != 0 || pageRightMargin != 0)
                {
                    pageTopMargin = (pageTopMargin / 28.346456693F) + 1.5;
                    pageBottomMargin = (pageBottomMargin / 28.346456693F) + 1.5;

                    pageLeftMargin = Convert.ToDouble(Common.UnitConverter(pageLeftMargin.ToString() + "pt", "cm"));
                    pageRightMargin = Convert.ToDouble(Common.UnitConverter(pageRightMargin.ToString() + "pt", "cm"));
                    sw.WriteLine(@"\usepackage[left=" + Math.Round(pageLeftMargin, 2) + "cm,right=" + Math.Round(pageRightMargin, 2) + "cm,top=" + Math.Round(pageTopMargin, 2) + "cm,bottom=" + Math.Round(pageBottomMargin, 2) + "cm,includeheadfoot]{geometry}");
                }
                else
                {
                    sw.WriteLine(@"\usepackage[left=3cm,right=3cm,top=3cm,bottom=3cm,includeheadfoot]{geometry}");
                }

                if (Convert.ToBoolean(CoverImage))
                    sw.WriteLine(@"\usepackage{eso-pic}");

                sw.WriteLine(_pageStyleFormat);

                sw.WriteLine(@"\begin{document} ");
                sw.WriteLine(@"\pagestyle{plain} ");
            }

            foreach (var prop in xeLaTexProperty)
            {
                sw.WriteLine(prop);
            }

            InsertFrontMatter(sw);

            if (Convert.ToBoolean(TocChecked))
                InsertTableOfContent(sw);

            sw.WriteLine(@"\pagestyle{fancy} ");
            sw.Flush();
            sw.Close();
            MergeFile(newFile1, newFile2);
        }

        private void MergeFile(string newFile1, string newFile2)
        {
            var fsw = new FileStream(_xetexFullFile, FileMode.Create, FileAccess.Write);
            var sw = new StreamWriter(fsw);

            FileStream fs1 = new FileStream(newFile2, FileMode.Open);
            StreamReader sr1 = new StreamReader(fs1);
            string line;
            while ((line = sr1.ReadLine()) != null)
            {
                sw.WriteLine(line);
            }
            sw.Flush();
            fs1.Close();
            sr1.Close();


            FileStream fs2 = new FileStream(newFile1, FileMode.Open);
            StreamReader sr2 = new StreamReader(fs2);
            while ((line = sr2.ReadLine()) != null)
            {
                sw.WriteLine(line);
            }
            sw.Flush();
            fs2.Close();
            sr2.Close();

            sw.Close();
            fsw.Close();

            try
            {
                File.Delete(newFile1);
                File.Delete(newFile2);
            }
            catch (Exception e) { }
        }


        private string GetPageStyle(Dictionary<string, Dictionary<string, string>> _cssClass, bool isMirrored)
        {
            string pageStyleText = "[a4paper]{article} ";

            double pageWidth = 0;
            double pageHeight = 0;
            if (_cssClass.ContainsKey("@page"))
            {
                Dictionary<string, string> cssProp = _cssClass["@page"];
                foreach (KeyValuePair<string, string> para in cssProp)
                {
                    if (para.Key == "page-width")
                    {
                        pageWidth = Convert.ToDouble(para.Value);
                    }
                    if (para.Key == "page-height")
                    {
                        pageHeight = Convert.ToDouble(para.Value);
                    }
                }
            }

            string paperSize = GetPaperSize(pageWidth, pageHeight);

            if (paperSize == "a4")
            {
                pageStyleText = "[a4paper]{article} ";
                if (isMirrored)
                {
                    pageStyleText = "[a4paper,twoside]{article} ";
                }
            }
            else if (paperSize == "a5")
            {
                pageStyleText = "[a5paper]{article} ";
                if (isMirrored)
                {
                    pageStyleText = "[a5paper,twoside]{article} ";
                }
            }
            else if (paperSize == "C5")
            {
                pageStyleText = "[c5paper]{article} ";
                if (isMirrored)
                {
                    pageStyleText = "[c5paper,twoside]{article} ";
                }
            }
            else if (paperSize == "a6")
            {
                pageStyleText = "[a6paper]{article} ";
                if (isMirrored)
                {
                    pageStyleText = "[a6paper,twoside]{article} ";
                }
            }
            //else if (paperSize == "Letter")
            //{
            //    pageStyleText = "[Letter]{article} ";
            //    if (isMirrored)
            //    {
            //        pageStyleText = "[HalfLetter,twoside]{article} ";
            //    }
            //}
            else if (paperSize == "halfletter")
            {
                pageStyleText = "[HalfLetter]{article} ";
                if (isMirrored)
                {
                    pageStyleText = "[HalfLetter,twoside]{article} ";
                }
            }
            else if (paperSize == "5.25in x 8.25in")
            {
                pageStyleText = "[gps1]{article} ";
                if (isMirrored)
                {
                    pageStyleText = "[gps1,twoside]{article} ";
                }
            }
            else if (paperSize == "5.8in x 8.7in")
            {
                pageStyleText = "[gps2]{article} ";
                if (isMirrored)
                {
                    pageStyleText = "[gps2,twoside]{article} ";
                }
            }
            else if (paperSize == "6in x 9in")
            {
                pageStyleText = @"[a4paper]{article}  \usepackage[margin=1in, paperwidth=6in, paperheight=9in]{geometry}";
                if (isMirrored)
                {
                    pageStyleText = @"[a4paper,twoside]{article}  \usepackage[margin=1in, paperwidth=6in, paperheight=9in]{geometry}";
                }
            }

            return pageStyleText;
            //if (Math.Round(pageWidth) == 595 && Math.Round(pageHeight) == 842) //A4 Size
            //    pageStyleText = "[a4paper]{article} ";
            //if (Math.Round(pageWidth) == 420 && Math.Round(pageHeight) == 595) //A5 Size
            //    pageStyleText = "[a5paper]{article} ";
            //if (Math.Round(pageWidth) == 459 && Math.Round(pageHeight) == 649) //C5 Size
            //    pageStyleText = "[c5paper]{article} ";
            ////\special{papersize=148mm,210mm}% it is A5 paper size, I got from Wikipedia.
            //if (Math.Round(pageWidth) == 298 && Math.Round(pageHeight) == 420) //A6 Size
            //    pageStyleText = "[a6paper]{article} ";
            //if (Math.Round(pageWidth) == 612 && Math.Round(pageHeight) == 792) //Letter
            //    pageStyleText = "[letter]{article} ";
            //if (Math.Round(pageWidth) == 396 && Math.Round(pageHeight) == 612) //Half Letter
            //    pageStyleText = "[halfletter]{article} ";
            //if (Math.Round(pageWidth) == 432 && Math.Round(pageHeight) == 648) //6in 9in paper
            //    pageStyleText = @"[a4paper]{article}  \usepackage[margin=1in, paperwidth=6in, paperheight=9in]{geometry}";
            //if (Math.Round(pageWidth) == 378 && Math.Round(pageHeight) == 594) //5.25in 8.25in paper
            //    pageStyleText = "[gps1]{article} ";
            //if (Math.Round(pageWidth) == 418 && Math.Round(pageHeight) == 626) //5.8in 8.7in paper
            //    pageStyleText = "[gps2]{article} ";
        }

        private string GetPaperSize(double paperWidth, double paperHeight)
        {
            string paperSize = "a4";

            if (Math.Round(paperWidth) == 612 && Math.Round(paperHeight) == 792)
            {
                paperSize = "Letter";
            }
            if (Math.Round(paperWidth) == 420 && Math.Round(paperHeight) == 595)
            {
                paperSize = "a5";
            }
            if (Math.Round(paperWidth) == 459 && Math.Round(paperHeight) == 649)
            {
                paperSize = "C5";
            }
            if (Math.Round(paperWidth) == 298 && Math.Round(paperHeight) == 420)
            {
                paperSize = "a6";
            }
            if (Math.Round(paperWidth) == 396 && Math.Round(paperHeight) == 612)
            {
                paperSize = "halfletter";
            }
            if (Math.Round(paperWidth) == 378 && Math.Round(paperHeight) == 594)
            {
                paperSize = "5.25in x 8.25in";
            }
            if (Math.Round(paperWidth) == 418 && Math.Round(paperHeight) == 626)
            {
                paperSize = "5.8in x 8.7in";
            }
            if (Math.Round(paperWidth) == 432 && Math.Round(paperHeight) == 648)
            {
                paperSize = "6in x 9in";
            }

            return paperSize;
        }

        private void InsertTableOfContent(StreamWriter sw)
        {
            String tableOfContent = string.Empty;
            if (_projectType.ToLower() == "dictionary")
            {
                if (_tocList.ContainsKey("TableofContent") && _tocList["TableofContent"].Count > 0)
                {
                    foreach (var tocSection in _tocList["TableofContent"])
                    {
                        if (tocSection.Key.Contains("PageStock"))
                        {
                            //tableOfContent += @"\addtocontents{toc}{\contentsline {section}{\numberline{} " + tocSection.Value + "}{\\pageref{" + tocSection.Key.Replace(" ", "") + "}}{}} \r\n ";
                            tableOfContent += "\r\n" + "\\addtocontents{toc}{\\protect \\contentsline{section}{" +
                                              tocSection.Value + " \\Large }{{\\protect \\pageref{" + tocSection.Key + "}}}{}}" +
                                              "\r\n";

                            //tableOfContent += "\r\n" + "\\addtocontents{toc}{\\protect \\contentsline{section}{ \\Large " +
                            //                  tocSection.Value + " \\Large }{{\\protect \\pageref{" + tocSection.Key + "}}}{}}" +
                            //                  "\r\n";
                        }
                    }
                }

                tableOfContent += "\r\n";
                tableOfContent += "\\newpage \r\n";
            }

            if (_projectType.ToLower() == "scripture")
            {

                if (_tocList.ContainsKey("TableofContent") && _tocList["TableofContent"].Count > 0)
                {
                    foreach (var tocSection in _tocList["TableofContent"])
                    {
                        if (tocSection.Key.Contains("PageStock"))
                        {
                            tableOfContent += "\r\n" + "\\addtocontents{toc}{\\protect \\contentsline{section}{" +
                                              tocSection.Value + "}{{\\protect \\pageref{" + tocSection.Key + "}}}{}}" +
                                              "\r\n";
                        }
                    }
                }
            }
            //tableOfContent += "\\thispagestyle{empty} \r\n";
            tableOfContent += "\\pagestyle{plain} \r\n";
            tableOfContent += "\\tableofcontents \r\n";
            //tableOfContent += "\\pagebreak[2] \r\n";
            tableOfContent += "\\newpage \r\n";
            tableOfContent += "\\setcounter{page}{1} \r\n";
            tableOfContent += "\\pagenumbering{arabic}  \r\n";
            // Common.FileInsertText(_xetexFullFile, tableOfContent);
            sw.WriteLine(tableOfContent);
        }

        private void InsertFrontMatter(StreamWriter sw)
        {
            string xeLaTexInstallationPath = string.Empty;
            String tableOfContent = string.Empty;
            if (Convert.ToBoolean(CoverImage) || Convert.ToBoolean(TitleInCoverPage))
            {
                xeLaTexInstallationPath = XeLaTexInstallation.GetXeLaTexDir();

                if (Common.IsUnixOS())
                {
                    xeLaTexInstallationPath = Path.GetDirectoryName(_xetexFullFile);
                }
                else
                {
                    xeLaTexInstallationPath = Common.PathCombine(xeLaTexInstallationPath, "bin");
                    xeLaTexInstallationPath = Common.PathCombine(xeLaTexInstallationPath, "win32");
                }
            }


            if (Convert.ToBoolean(CoverImage))
            {
                string destinctionPath = Common.PathCombine(xeLaTexInstallationPath, Path.GetFileName(CoverPageImagePath));
                if (CoverPageImagePath.Trim() != "")
                {
                    if (CoverPageImagePath != destinctionPath)
                        File.Copy(CoverPageImagePath, destinctionPath, true);

                    tableOfContent += "\\color{black} \r\n";
                    tableOfContent += "\\AddToShipoutPicture*{% \r\n";
                    tableOfContent +=
                        "\\put(0,0){\\rule{\\paperwidth}{\\paperheight}}{\\includegraphics[width=\\paperwidth, height=\\paperheight]{" +
                        Path.GetFileName(CoverPageImagePath) + "}}% \r\n";
                    tableOfContent += "} \r\n";
                    tableOfContent += "\\thispagestyle{empty} \r\n";
                }

                if (Convert.ToBoolean(IncludeBookTitleintheImage))
                {
                    tableOfContent += "\\font\\CoverPageHeading=\"Times New Roman/B\":color=000000 at 22pt \r\n";
                    tableOfContent += "\\vskip 60pt \r\n";
                    tableOfContent += "\\begin{center} \r\n";
                    tableOfContent += "\\CoverPageHeading{" + Param.GetMetadataValue(Param.Title) + "} \r\n";
                    tableOfContent += "\\end{center} \r\n";
                }

                tableOfContent += "\\newpage \r\n";
                tableOfContent += "\\newpage \r\n";
                tableOfContent += "\\thispagestyle{empty} \r\n";
                tableOfContent += "\\mbox{} \r\n";
            }

            if (Convert.ToBoolean(TitleInCoverPage))
            {
                string copyRightFilePath = Param.GetMetadataValue(Param.CopyrightPageFilename);

                string logoFileName = string.Empty;
                if (Param.GetOrganization().StartsWith("SIL"))
                {
                    if (ProjectType.ToLower() == "dictionary")
                    {
                        logoFileName = "sil-bw-logo.jpg";
                    }
                    else
                    {
                        logoFileName = "WBT_H_RGB_red.png";
                    }
                }
                else if (Param.GetOrganization().StartsWith("Wycliffe"))
                {
                    logoFileName = "WBT_H_RGB_red.png";
                }

                if (copyRightFilePath.Trim().Length != 0)
                {
                    copyRightFilePath = Path.GetDirectoryName(copyRightFilePath);
                }
                else
                {
                    string executablePath = Common.GetApplicationPath();
                    copyRightFilePath = Common.PathCombine(executablePath, "Copyrights");
                }


                copyRightFilePath = Path.Combine(copyRightFilePath, logoFileName);
                if (File.Exists(copyRightFilePath))
                {
                    if (Common.UnixVersionCheck())
                    {
                        string logoTitleFileName = logoFileName;
                        logoTitleFileName = Path.Combine(Path.GetTempPath(), logoTitleFileName);
                        if (File.Exists(copyRightFilePath))
                        {
                            File.Copy(copyRightFilePath, logoTitleFileName, true);
                            File.Copy(copyRightFilePath, Path.Combine(_projectPath, logoFileName), true);
                            File.Copy(copyRightFilePath, Path.Combine(xeLaTexInstallationPath, logoFileName), true);
                        }
                    }
                    else
                    {
                        if (logoFileName.IndexOf("gif") > 0)
                        {
                            try
                            {
                                // Load the image.

                                System.Drawing.Image image1 = System.Drawing.Image.FromFile(copyRightFilePath);
                                // Save the image in JPEG format.
                                logoFileName = logoFileName.Replace(".gif", ".jpg");
                                image1.Save(Path.Combine(Path.GetTempPath(), logoFileName), System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                            catch { }

                            if (File.Exists(Path.Combine(Path.GetTempPath(), logoFileName)))
                            {
                                File.Copy(Path.Combine(Path.GetTempPath(), logoFileName), Path.Combine(_projectPath, logoFileName), true);
                                File.Copy(Path.Combine(Path.GetTempPath(), logoFileName), Path.Combine(xeLaTexInstallationPath, logoFileName), true);
                            }
                        }
                        else
                        {
                            string logoTitleFileName = logoFileName;
                            logoTitleFileName = Path.Combine(Path.GetTempPath(), logoTitleFileName);
                            if (File.Exists(copyRightFilePath))
                            {
                                File.Copy(copyRightFilePath, logoTitleFileName, true);
                                File.Copy(copyRightFilePath, Path.Combine(_projectPath, logoFileName), true);

                                File.Copy(copyRightFilePath, Path.Combine(xeLaTexInstallationPath, logoFileName), true);
                            }
                        }
                    }

                }


                tableOfContent += "\\begin{titlepage}\r\n";
                tableOfContent += "\\begin{center}\r\n";
                tableOfContent += "\\textsc{\\LARGE " + Param.GetMetadataValue(Param.Title) + "}\\\\[1.5cm] \r\n";
                tableOfContent += "\\vspace{130 mm} \r\n";
                tableOfContent += "\\textsc{" + Param.GetMetadataValue(Param.Publisher) + "}\\\\[0.5cm] \r\n";
                if (logoFileName.Contains(".png"))
                {
                    tableOfContent += "\\includegraphics[width=0.15 \\textwidth]{./" + logoFileName + "}\\\\[1cm]    \r\n";
                }
                else
                {
                    tableOfContent += "\\includegraphics[width=0.10 \\textwidth]{./" + logoFileName + "}\\\\[1cm]    \r\n";
                }
                tableOfContent += "\\end{center} \r\n";
                tableOfContent += "\\end{titlepage} \r\n";

                tableOfContent += "\\newpage \r\n";
                tableOfContent += "\\newpage \r\n";
                //tableOfContent += "\\thispagestyle{empty} \r\n";
                tableOfContent += "\\mbox{} \r\n";

            }


            if (Convert.ToBoolean(CopyrightInformation))
            {
                tableOfContent += "\\pagenumbering{roman}  \r\n";
                tableOfContent += "\\setcounter{page}{3} \r\n";

                tableOfContent += "\\input{" + CopyrightTexFilename + "} \r\n";
                //tableOfContent += "\\thispagestyle{empty} \r\n";
                tableOfContent += "\\pagestyle{plain} \r\n";
                tableOfContent += "\\newpage \r\n";
                tableOfContent += "\\newpage \r\n";
                tableOfContent += "\\thispagestyle{empty} \r\n";
                tableOfContent += "\\mbox{} \r\n";
            }
            sw.WriteLine(tableOfContent);
            //Common.FileInsertText(_xetexFullFile, tableOfContent);
        }

        private void InsertReversalIndex(StreamWriter sw)
        {
            String ReversalIndexContent = string.Empty;

            if (ReversalIndexExist)
            {
                ReversalIndexContent += "\\input{" + ReversalIndexTexFilename + "} \r\n";
                //tableOfContent += "\\thispagestyle{empty} \r\n";
                ReversalIndexContent += "\\pagestyle{plain} \r\n";
                ReversalIndexContent += "\\newpage \r\n";
            }
            sw.WriteLine(ReversalIndexContent);
            //Common.FileInsertText(_xetexFullFile, ReversalIndexContent);
        }

        private string RemoveBody(string paraStyle)
        {
            //if (paraStyle.IndexOf("_body") == -1 && paraStyle != "@page")
            if (paraStyle.IndexOf("_") == -1 && paraStyle != "@page")
            {
                return string.Empty;
            }
            paraStyle = paraStyle.Replace("_body", "");
            string simplified = paraStyle.Replace("_", "");
            return simplified;
        }

        /// <summary>
        /// parentClassName = b_a
        /// step1: b_a = b 
        /// step2: b_a = merge a
        /// </summary>
        /// <param name="paraStyle"></param>
        private void MergeCssStyle(string paraStyle)
        {
            paraStyle = paraStyle.Replace("_body", "");
            string parentClass = paraStyle;
            string mergedClass = paraStyle.Replace("_", "");
            string[] parent = paraStyle.Split('_');
            if (parent.Length > 0)
            {
                string childClass = Common.LeftString(paraStyle, "_");
                parentClass = paraStyle.Replace(childClass + "_", "");
                parentClass = parentClass.Replace("_", "");
                if (_cssClass.ContainsKey(mergedClass))
                {
                    return;
                }
                _tempStyle = new Dictionary<string, string>();
                //Copy
                if (_cssClass.ContainsKey(childClass))
                {
                    foreach (KeyValuePair<string, string> property in _cssClass[childClass])
                    {
                        _tempStyle[property.Key] = property.Value;
                    }
                }
                //Merge
                if (_cssClass.ContainsKey(parentClass))
                {
                    foreach (KeyValuePair<string, string> property in _cssClass[parentClass])
                    {
                        if (!_tempStyle.ContainsKey(property.Key))
                            _tempStyle[property.Key] = property.Value;
                    }
                }
                _cssClass[mergedClass] = _tempStyle;
            }
            _mergedClass.Add(mergedClass);
        }

        private void SetVisibilityColor(KeyValuePair<string, Dictionary<string, string>> className)
        {
            if (className.Value.ContainsKey("visibility"))
            {
                if (className.Value["visibility"] == "hidden")
                {
                    className.Value["FillColor"] = "Color/Paper";
                    className.Value["StrokeColor"] = "Color/Paper";
                }
                className.Value.Remove("visibility");
            }
        }

        private void GetVariableClassName(string className)
        {
            if (className.IndexOf("TitleMain") == 5)
            {
                _textVariables.Add("TitleMain_" + className);
            }
            else if (className.IndexOf("hideChapterNumber_") == 0)
            {
                _textVariables.Add("ChapterNumber_" + className);
            }
            else if (className.IndexOf("hideVerseNumber_") == 0)
            {
                _textVariables.Add("hideVerseNumber_" + className);
            }
            //else if (className.IndexOf("headword") == 0)
            //{
            //    _textVariables.Add("Guideword_" + className);
            //}
            //else if (className.IndexOf("xhomographnumber") == 0)
            //{
            //    _textVariables.Add("HomoGraphNumber_" + className);
            //}
        }

        private void InsertNode(KeyValuePair<string, Dictionary<string, string>> className)
        {
            string newClassName = className.Key;
            //string parentClassName = Common.RightString(newClassName, _styleSeperator);

            _node = _root.SelectSingleNode(_xPath, nsmgr);
            //if (_node == null) return;
            XmlDocumentFragment styleNode = _styleXMLdoc.CreateDocumentFragment();
            styleNode.InnerXml = _node.OuterXml;
            _node.ParentNode.InsertAfter(styleNode, _node);

            newClassName = _tagType + "/" + className.Key;

            _nameElement = (XmlElement)_node;
            _nameElement.SetAttribute("Self", newClassName);
            _nameElement.SetAttribute("Name", className.Key);
            _nameElement.SetAttribute("NextStyle", newClassName);
            SetTagProperty(className.Key);
            foreach (KeyValuePair<string, string> property in className.Value)
            {
                if (property.Key == "Leading" || property.Key == "lang")
                {
                    continue;
                }
                _nameElement.SetAttribute(property.Key, property.Value);
            }

            SetLanguage(className.Key);
            SetBasedOn("None", newClassName);
            SetAppliedFont(className.Value, newClassName);
            SetLineHeight(className.Value, newClassName);
            SetBaseLineShift(className.Value, newClassName);
            SetTagNode();
        }

        private void SetLanguage(string className)
        {
            if (_tagType == "ParagraphStyle") // Note - If needed apply only for paragraph style.
            {
                if (_languageStyleName.ContainsKey(className)) // if lang style then write language for this tag.
                {
                    string lang = _languageStyleName[className];
                    WriteEntryLanguage(lang);
                }
            }
        }

        private void WriteEntryLanguage(string lang)
        {
            string language = string.Empty;
            switch (lang)
            {
                case "es":
                case "spa":
                    language = "$ID/Spanish: Castilian";
                    break;
                case "pt":
                case "por":
                    language = "$ID/Portuguese";
                    break;
                case "en":
                case "eng":
                    language = "$ID/English: USA";
                    break;
                case "bg":
                case "bul":
                    language = "$ID/Bulgarian";
                    break;
                case "ca":
                case "cat":
                    language = "$ID/Catalan";
                    break;
                case "da":
                case "dan":
                    language = "$ID/Danish";
                    break;
                case "nl":
                case "nld":
                    language = "$ID/Dutch";
                    break;
                case "fr":
                case "fra":
                    language = "$ID/French";
                    break;
                case "el":
                case "ell":
                    language = "$ID/Greek";
                    break;
                case "hu":
                case "hun":
                    language = "$ID/Hungarian";
                    break;
                case "it":
                case "ita":
                    language = "$ID/Italian";
                    break;
                case "pl":
                case "pol":
                    language = "$ID/Polish";
                    break;
                case "ru":
                case "rus":
                    language = "$ID/Russian";
                    break;
                case "sk":
                case "slk":
                    language = "$ID/Slovak";
                    break;
                case "sv":
                case "swe":
                    language = "$ID/Swedish";
                    break;
                case "tr":
                case "tur":
                    language = "$ID/Turkish";
                    break;
                case "uk":
                case "ukr":
                    language = "$ID/Ukrainian";
                    break;

                default:
                    language = "$ID/English: USA";
                    break;
            }
            _nameElement.SetAttribute("AppliedLanguage", language);
        }

        private void SetBasedOn(string parentStyle, string sourceClassName)
        {
            string style = "//" + _tagType + "[@Self='" + sourceClassName + "']/Properties/BasedOn";
            XmlNode nodeBasedOn = _root.SelectSingleNode(style, nsmgr);
            if (nodeBasedOn != null)
            {
                var nameElement = (XmlElement)nodeBasedOn;
                nameElement.SetAttribute("type", "object");
                nodeBasedOn.InnerText = _tagType + "/" + parentStyle;
            }
        }

        private void SetAppliedFont(Dictionary<string, string> className, string sourceClassName)
        {
            if (className.ContainsKey("AppliedFont"))
            {
                string style = "//" + _tagType + "[@Self='" + sourceClassName + "']/Properties/AppliedFont";
                XmlNode nodeAppliedFont = _root.SelectSingleNode(style, nsmgr);
                if (nodeAppliedFont != null)
                {
                    var nameElement = (XmlElement)nodeAppliedFont;
                    nameElement.SetAttribute("type", "string");
                    nodeAppliedFont.InnerText = className["AppliedFont"];
                }
            }
        }

        private void SetLineHeight(Dictionary<string, string> className, string sourceClassName)
        {
            if (className.ContainsKey("Leading"))
            {
                string style = "//" + _tagType + "[@Self='" + sourceClassName + "']/Properties/Leading";
                XmlNode nodeLeading = _root.SelectSingleNode(style, nsmgr);
                if (nodeLeading != null)
                {
                    var nameElement = (XmlElement)nodeLeading;
                    string propertyType = Common.GetLeadingType(className);
                    string text = className["Leading"];
                    if (sourceClassName.IndexOf("ParagraphStyle/ChapterNumber") >= 0 || sourceClassName.IndexOf("CharacterStyle/ChapterNumber") >= 0)
                    {
                        propertyType = "enumeration";
                        text = "Auto";
                    }
                    nameElement.SetAttribute("type", propertyType);
                    nodeLeading.InnerText = text;
                }
            }
        }
        private void SetBaseLineShift(Dictionary<string, string> className, string sourceClassName)
        {
            if (className.ContainsKey("BaselineShift"))
            {
                if (sourceClassName.IndexOf("CharacterStyle/ChapterNumber") >= 0) //if (sourceClassName.IndexOf("ParagraphStyle/ChapterNumber") >= 0 || sourceClassName.IndexOf("CharacterStyle/ChapterNumber") >= 0)
                {
                    string style = "//" + _tagType + "[@Self='" + sourceClassName + "']";
                    XmlNode baselineShift = _root.SelectSingleNode(style, nsmgr);
                    if (baselineShift != null)
                    {
                        var nameElement = (XmlElement)baselineShift;
                        string pointSize = className["PointSize"];
                        string point2 = Common.LeftString(pointSize, ".");
                        int pt = int.Parse(point2);
                        //int baseshift = pt - 12;
                        int baseshift = pt * 2 / 3;
                        int point = pt * 2 / 3;
                        nameElement.SetAttribute("BaselineShift", "-" + baseshift);
                        nameElement.SetAttribute("PointSize", "-" + point);
                    }

                }
            }
        }

        private string OpenIDStyles()
        {
            string projType = "scripture";
            //string targetFolder = Common.PathCombine(Common.GetTempFolderPath(), "InDesignFiles" + Path.DirectorySeparatorChar + projType);
            string targetFolder = Common.RightRemove(_projectPath, Path.DirectorySeparatorChar.ToString());
            targetFolder = Common.PathCombine(targetFolder, "Resources");
            string styleFilePath = Common.PathCombine(targetFolder, "Styles.xml");

            _styleXMLdoc = new XmlDocument();
            _styleXMLdoc.Load(styleFilePath);
            return styleFilePath;
        }

        private void SetTagProperty(string newClassName)
        {
            _tagName = Common.IsTagClass(newClassName);
            if (_tagName != string.Empty)
            {
                if (_tagName == "olFirst") // ol first line
                {
                    _nameElement.SetAttribute("SpaceBefore", "12");
                    _nameElement.SetAttribute("LeftIndent", "36");
                    _nameElement.SetAttribute("BulletsAndNumberingListType", "NumberedList");
                    _nameElement.SetAttribute("NumberingExpression", "^#.^.");
                    _nameElement.SetAttribute("BulletsTextAfter", "^.");
                    _nameElement.SetAttribute("NumberingContinue", "false");
                }
                else if (_tagName == "ol4Next") // ol rest of the line
                {
                    _nameElement.SetAttribute("LeftIndent", "36");
                    _nameElement.SetAttribute("BulletsAndNumberingListType", "NumberedList");
                    _nameElement.SetAttribute("NumberingExpression", "^#.^.");
                    _nameElement.SetAttribute("BulletsTextAfter", "^.");
                    _nameElement.SetAttribute("NumberingContinue", "true");
                }
                else if (_tagName == "ulFirst") // ul
                {
                    _nameElement.SetAttribute("SpaceBefore", "12");
                    _nameElement.SetAttribute("LeftIndent", "36");
                    _nameElement.SetAttribute("BulletsAndNumberingListType", "BulletList");
                    _nameElement.SetAttribute("BulletsTextAfter", "^.");
                }
                else if (_tagName == "ul4Next") // ul
                {
                    _nameElement.SetAttribute("LeftIndent", "36");
                    _nameElement.SetAttribute("BulletsAndNumberingListType", "BulletList");
                    _nameElement.SetAttribute("BulletsTextAfter", "^.");
                }
                else if (_tagName == "ul" || _tagName == "ol") // ul or ol
                {
                    _nameElement.SetAttribute("LeftIndent", "36");
                }
            }
        }

        private void SetTagNode()
        {
            if (_tagName.Length > 0)
            {
                if (_tagName == "olFirst" || _tagName == "ol4Next")
                {
                    string olNode = "<TabList type=\"list\">";
                    olNode += "<ListItem type=\"record\">";
                    olNode += "<Alignment type=\"enumeration\">LeftAlign</Alignment>";
                    olNode += "<AlignmentCharacter type=\"string\">.</AlignmentCharacter>";
                    olNode += "<Leader type=\"string\">";
                    olNode += "<Position type=\"unit\">0</Position>";
                    olNode += "</Leader>";
                    olNode += "</ListItem>";
                    olNode += "</TabList>";

                    XmlElement tabList = _styleXMLdoc.CreateElement("TabList");
                    tabList.InnerXml = olNode;
                    _nameElement.AppendChild(tabList);
                }
                else if (_tagName == "ulFirst" || _tagName == "ul4Next")
                {
                    string olNode = "<BulletChar BulletCharacterType=\"UnicodeOnly\" BulletCharacterValue=\"42\"/>";

                    XmlElement tabList = _styleXMLdoc.CreateElement("TabList");
                    tabList.InnerXml = olNode;
                    _nameElement.AppendChild(tabList);

                }
            }
        }
    }
}