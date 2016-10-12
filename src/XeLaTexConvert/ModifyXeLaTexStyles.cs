// --------------------------------------------------------------------------------------------
// <copyright file="ModifyXeLaTexStyles.cs" from='2009' to='2014' company='SIL International'>
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
//
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using SIL.Tool;

namespace SIL.PublishingSolution
{
	public class ModifyXeLaTexStyles
	{
		#region Private Variables

		private bool _createPageNumber;
		private string _projectPath;
		private string _pageStyleFormat;
		private string _xetexFullFile;
		private string _projectType;
		Dictionary<string, Dictionary<string, string>> _cssClass = new Dictionary<string, Dictionary<string, string>>();

		XeLaTexMapProperty mapProperty = new XeLaTexMapProperty();
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
		private List<string> _xeLaTexPropertyFontStyleList = new List<string>();
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
			set { _reversalIndexExist = value; }
		}

		public string ReversalIndexTexFilename
		{
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

		public List<string> XeLaTexPropertyFontStyleList
		{
			get { return _xeLaTexPropertyFontStyleList; }
			set { _xeLaTexPropertyFontStyleList = value; }
		}

		#endregion

		public void ModifyStylesXML(string projectPath, StreamWriter xetexFile, Dictionary<string, Dictionary<string, string>> newProperty,
			Dictionary<string, Dictionary<string, string>> cssClass, string xetexFullFile, string pageStyleFormat, Dictionary<string, string> langFontDictionary, bool createPageNumber)
		{
			_createPageNumber = createPageNumber;
			_langFontDictionary = langFontDictionary;
			_projectPath = projectPath;
			_cssClass = cssClass;
			_xetexFullFile = xetexFullFile;
			_pageStyleFormat = pageStyleFormat;
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
			}
		}

		private void MapProperty()
		{
			string newFile1 = _xetexFullFile.Replace(".tex", "1.tex");
			string newFile2 = _xetexFullFile.Replace(".tex", "2.tex");

			File.Copy(_xetexFullFile, newFile1, true);

			StreamWriter sw = new StreamWriter(newFile2);

			List<string> includePackageList = new List<string>();

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
				string xeLaTexProp = mapProperty.XeLaTexProperty(cssClass.Value, className, inlineStyle, includePackageList, inlineInnerStyle, _langFontDictionary);
				if (xeLaTexProp.Trim().Length > 0 && !_xeLaTexPropertyFontStyleList.Contains(xeLaTexProp))
				{
					_xeLaTexPropertyFontStyleList.Add(xeLaTexProp);
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

				sw.WriteLine(@"\usepackage{float}");
				sw.WriteLine(@"\usepackage{grffile}");
				sw.WriteLine(@"\usepackage{graphicx}");
				sw.WriteLine(@"\usepackage{amssymb}");
				sw.WriteLine(@"\usepackage{fontspec}");
				sw.WriteLine(@"\usepackage{fancyhdr}");
				sw.WriteLine(@"\usepackage{multicol}");
				sw.WriteLine(@"\usepackage{calc}");
				sw.WriteLine(@"\usepackage{lettrine}");
				sw.WriteLine(@"\usepackage{alphalph}");
				if (pageTopMargin != 0 || pageBottomMargin != 0 || pageLeftMargin != 0 || pageRightMargin != 0)
				{
					pageTopMargin = (pageTopMargin * 0.03514598035);
					pageBottomMargin = (pageBottomMargin * 0.03514598035);

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

				foreach (var package in includePackageList)
				{
					sw.WriteLine(package);
				}

				sw.WriteLine(_pageStyleFormat);

				sw.WriteLine(@"\parindent=0pt");
				sw.WriteLine(@"\parskip=\medskipamount");
				sw.WriteLine(@"\begin{document}");
				sw.WriteLine(@"\pagestyle{plain}");
				sw.WriteLine(@"\sloppy");
				sw.WriteLine(@"\setlength{\parfillskip}{0pt plus 1fil}");
			}

			foreach (var prop in _xeLaTexPropertyFontStyleList)
			{
				sw.WriteLine(prop);
			}

			InsertFrontMatter(sw);

			if (Convert.ToBoolean(TocChecked))
			{
				InsertTableOfContent(sw);
			}
			else
			{
				InsertContentApplyFormat(sw);
			}



			if (_cssClass.ContainsKey("@page:left-top-left"))
			{
				Dictionary<string, string> pagePrty = _cssClass["@page:left-top-left"];
				if (pagePrty.ContainsKey("content") && pagePrty["content"].Replace("'", "").Equals("none"))
				{
					sw.WriteLine(@"\pagestyle{plain} ");
				}
				else
				{
					sw.WriteLine(@"\pagestyle{fancy} ");
				}
			}
			else
			{
				sw.WriteLine(@"\pagestyle{fancy} ");
			}
			sw.Flush();
			sw.Close();
			MergeFile(newFile1, newFile2);
		}

		private void InsertContentApplyFormat(StreamWriter sw)
		{
			String tableOfContent = string.Empty;
			tableOfContent += "\\mbox{} \r\n";
			tableOfContent += "\\newpage \r\n";
			tableOfContent += "\\newpage \r\n";
			if (_createPageNumber)
			{
				tableOfContent += "\\setcounter{page}{1} \r\n";
				if (_cssClass.ContainsKey("@page:none-none"))
					tableOfContent += "\\pagenumbering{gobble} ";
				else
					tableOfContent += "\\pagenumbering{arabic} ";
			}
			sw.WriteLine(tableOfContent);
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
			catch (Exception)
			{

			}
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
							tableOfContent += "\r\n" + "\\addtocontents{toc}{\\protect \\contentsline{section}{" +
											  tocSection.Value + " \\Large }{{\\protect \\pageref{" + tocSection.Key + "}}}{}}" +
											  "\r\n";
						}
					}
				}

				tableOfContent += "\r\n";
				tableOfContent += "\\newpage \r\n";
				sw.WriteLine(tableOfContent);
				InsertContentApplyFormat(sw);
				tableOfContent = "";
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
				tableOfContent += "\r\n";
				tableOfContent += "\\newpage \r\n";
				InsertContentApplyFormat(sw);
			}
			tableOfContent += "\\pagestyle{plain} \r\n";
			tableOfContent += "\\tableofcontents \r\n";
			tableOfContent += "\\newpage \r\n";
			sw.WriteLine(tableOfContent);
		}

		private void InsertFrontMatter(StreamWriter sw)
		{
			bool isLinux = Common.UnixVersionCheck();
			string xeLaTexInstallationPath = string.Empty;
			String tableOfContent = string.Empty;
			string titleFontName = string.Empty;
			string titleFontSize = string.Empty;
			string styleCoverPage = string.Empty;

			string fontName = string.Empty;
			var fontSize = string.Empty;

			Param.LoadUiLanguageFontInfo();
			string languageCode = Common.GetLocalizationSettings();
			if (Param.UiLanguageFontSettings != null)
			{
				if (Param.UiLanguageFontSettings.ContainsKey(languageCode))
				{
					Dictionary<string, string> fontInfo = Param.UiLanguageFontSettings[languageCode];
					foreach (KeyValuePair<string, string> fontValue in fontInfo)
					{
						fontName = Convert.ToString(fontValue.Key);
						fontSize = Convert.ToString(fontValue.Value);
					}
				}
			}
			if (String.IsNullOrEmpty(fontName) || String.IsNullOrEmpty(fontSize))
			{
				styleCoverPage = "CoverPageTitle";
				GetCoverPageTitleFontStyle(styleCoverPage, ref titleFontName, ref titleFontSize);
			}
			else
			{
				titleFontName = fontName;
				titleFontSize = fontSize.ToString();
			}
			
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
					if (File.Exists(CoverPageImagePath) &&  CoverPageImagePath != destinctionPath && Directory.Exists(xeLaTexInstallationPath)))
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
					tableOfContent += "\\font\\CoverPageTitle=\"" + titleFontName + "/B\":color=000000 at 22pt \r\n";
					tableOfContent += "\\font\\pFrontMatterdiv=\"" + titleFontName + "/B\":color=000000 at " + titleFontSize + "pt \r\n";
					tableOfContent += "\\vskip 60pt \r\n";
					tableOfContent += "\\begin{center} \r\n";
					tableOfContent += "\\CoverPageTitle{" + Param.GetMetadataValue(Param.Title) + "} \r\n";
					tableOfContent += "\\end{center} \r\n";
				}
				else
				{
					tableOfContent += "\\font\\CoverPageTitle=\"" + titleFontName + "/B\":color=000000 at 22pt \r\n";
					tableOfContent += "\\vskip 60pt \r\n";
					tableOfContent += "\\begin{center} \r\n";
					tableOfContent += "\\CoverPageTitle{" + " " + "} \r\n";
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
					logoFileName = ProjectType.ToLower() == "dictionary" ? "2014_sil_logo.png" : "WBT_H_RGB_red.png";
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


				copyRightFilePath = Common.PathCombine(copyRightFilePath, logoFileName);
				if (File.Exists(copyRightFilePath))
				{
					if (isLinux)
					{
						string logoTitleFileName = logoFileName;
						logoTitleFileName = Common.PathCombine(Path.GetTempPath(), logoTitleFileName);
						if (File.Exists(copyRightFilePath))
						{
							File.Copy(copyRightFilePath, logoTitleFileName, true);
							File.Copy(copyRightFilePath, Common.PathCombine(_projectPath, logoFileName), true);
							File.Copy(copyRightFilePath, Common.PathCombine(xeLaTexInstallationPath, logoFileName), true);
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
								image1.Save(Common.PathCombine(Path.GetTempPath(), logoFileName), System.Drawing.Imaging.ImageFormat.Jpeg);
							}
							catch { }

							if (File.Exists(Common.PathCombine(Path.GetTempPath(), logoFileName)))
							{
								File.Copy(Common.PathCombine(Path.GetTempPath(), logoFileName), Common.PathCombine(_projectPath, logoFileName), true);
								File.Copy(Common.PathCombine(Path.GetTempPath(), logoFileName), Common.PathCombine(xeLaTexInstallationPath, logoFileName), true);
							}
						}
						else
						{
							string logoTitleFileName = logoFileName;
							logoTitleFileName = Common.PathCombine(Path.GetTempPath(), logoTitleFileName);
							if (File.Exists(copyRightFilePath))
							{
								File.Copy(copyRightFilePath, logoTitleFileName, true);
								File.Copy(copyRightFilePath, Common.PathCombine(_projectPath, logoFileName), true);

								if (Directory.Exists(xeLaTexInstallationPath))
									File.Copy(copyRightFilePath, Common.PathCombine(xeLaTexInstallationPath, logoFileName), true);

							}
						}
					}

				}
				tableOfContent += "\\begin{titlepage}\r\n";
				tableOfContent += "\\begin{center}\r\n";
				tableOfContent += "\\textsc{\\LARGE \\CoverPageTitle{" + Param.GetMetadataValue(Param.Title) + "}}\\\\[1.5cm] \r\n";
				tableOfContent += "\\vspace{110 mm} \r\n";
				tableOfContent += "\\textsc{ \\CoverPageTitle{" + Param.GetMetadataValue(Param.Publisher).Replace("&", @"\&") + "}}\\\\[0.5cm] \r\n";
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
			}


			if (Convert.ToBoolean(CopyrightInformation))
			{
				if (_cssClass.ContainsKey("@page:none-none"))
					tableOfContent += "\\pagenumbering{gobble}  \r\n";
				else
					tableOfContent += "\\pagenumbering{roman}  \r\n";
				tableOfContent += "\\setcounter{page}{3} \r\n";
				tableOfContent += "\\input{" + CopyrightTexFilename + "} \r\n";
				tableOfContent += "\\pagestyle{plain} \r\n";
				tableOfContent += "\\newpage \r\n";
			}
			else
			{
				if (tableOfContent != string.Empty)
				{
					if (_cssClass.ContainsKey("@page:none-none"))
						tableOfContent += "\\pagenumbering{gobble}  \r\n";
					else
						tableOfContent += "\\pagenumbering{roman}  \r\n";
					tableOfContent += "\\setcounter{page}{3} \r\n";
					tableOfContent += "\\pagestyle{plain} \r\n";
					tableOfContent += "\\newpage \r\n";
					tableOfContent += "\\newpage \r\n";
					tableOfContent += "\\thispagestyle{empty} \r\n";
					tableOfContent += "\\mbox{} \r\n";
				}
			}
			sw.WriteLine(tableOfContent);
		}

		private void GetCoverPageTitleFontStyle(string styleCoverPage, ref string titleFontName, ref string titleFontSize)
		{
			if (_cssClass.ContainsKey(styleCoverPage))
			{
				Dictionary<string, string> cssProp = _cssClass[styleCoverPage];
				foreach (KeyValuePair<string, string> para in cssProp)
				{
					if (para.Key == "font-family")
					{
						titleFontName = Convert.ToString(para.Value);
					}

					if (para.Key == "font-size")
					{
						titleFontSize = Convert.ToString(para.Value);
					}
				}
			}
		}

		private string RemoveBody(string paraStyle)
		{
			if (paraStyle.IndexOf("_") == -1 && paraStyle != "@page")
			{
				return string.Empty;
			}
			paraStyle = paraStyle.Replace("_body", "");
			string simplified = paraStyle.Replace("_", "");
			return simplified;
		}
	}
}