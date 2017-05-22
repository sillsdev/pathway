// --------------------------------------------------------------------------------------------
// <copyright file="ConfigurationToolBL.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Configuration Tool Business Layer
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using L10NSharp;
using SilTools;
using SIL.Tool;


namespace SIL.PublishingSolution
{
	public class ConfigurationToolBL
	{
		#region Private Variables

		private string _cssPath;
		private string _loadType;
		private Dictionary<string, Dictionary<string, string>> _cssClass =
							new Dictionary<string, Dictionary<string, string>>();
		Dictionary<string, string> standardSize = new Dictionary<string, string>();
		public string Caption = "Pathway Configuration Tool";
		public TraceSwitch _traceOnBL = new TraceSwitch("General", "Trace level for application");
		private List<string> _propertyValue = new List<string>();
		private List<string> _groupPropertyValue = new List<string>();
		private bool _isUnixOS = false;
		private int _cToolPnlOtherFormatTop = 0;
		private bool _validateWebInput = false;
		#endregion

		#region Public Variable

		public DataSet DataSetForGrid = new DataSet();
		public string MediaType = string.Empty;
		public string StyleName = string.Empty;
		public int SelectedRowIndex = 0;
		public string AttribFile = "file";
		public string AttribName = "name";
		public string AttribType = "type";
		public string AttribShown = "shown";
		public string AttribApproved = "approvedBy";
		public string AttribPreviewFile1 = "previewfile1";
		public string AttribPreviewFile2 = "previewfile2";
		public string AttrFtpAddrs = "ftpaddress";
		public string AttrFtpUid = "ftpuserid";
		public string AttrFtpPwd = "ftppwd";
		public string AttrDbSerName = "dbservername";
		public string AttrDbName = "dbname";
		public string AttrDbUid = "dbuserid";
		public string AttrDbPwd = "dbpwd";
		public string inputTypeBL = string.Empty;
		public string ElementDesc = "description";
		public string ElementAvailable = "available";
		public string ElementComment = "comment";
		public string TypeStandard = "Standard";
		public string TypeCustom = "Custom";
		public string PreviousStyleName = string.Empty;
		public string NewStyleName = string.Empty;
		public string PreviousValue = string.Empty;
		public string FileName = string.Empty;
		public string FileType = string.Empty;
		public string PreviewFileName1 = string.Empty;
		public string PreviewFileName2 = string.Empty;
		public bool AddMode1;
		public enum ScreenMode { Load, New, View, Edit, Delete, SaveAs, Modify };
		public ScreenMode _screenMode;
	    public string _TitleText = string.Empty;

		CssTree cssTree = new CssTree();

		//Column Index
		public int ColumnName = 0;
		public int ColumnDescription = 1;
		public int ColumnComment = 2;
		public int ColumnType = 3;
		public int ColumnShown = 4;
		public int ColumnApprovedBy = 5;
		public int ColumnFile = 6;
		public int PreviewFile1 = 7;
		public int PreviewFile2 = 8;
		protected bool IsUnixOs
		{
			get { return _isUnixOS; }
			set { _isUnixOS = value; }
		}

		#endregion

		#region Protected Variables
		protected readonly ArrayList _cssNames = new ArrayList();
		ErrorProvider _errProvider = new ErrorProvider();
		protected bool _isCreatePreview1;
		protected string _caption = "Pathway Configuration Tool";
		protected string _redoUndoBufferValue = string.Empty;
		protected Color _selectedColor = SystemColors.InactiveBorder; //Color.FromArgb(255, 204, 102);
		protected Color _selectedInputTypeColor = Color.Orange;
		protected Color _deSelectedColor = SystemColors.Control;
		protected string _styleName;
		private string _previousStyleName;
		protected string _fileProduce = "One";
		protected bool _fixedLineHeight = false;
		protected bool _includeImage = true;
		protected bool _pageBreak = false;
        protected bool _centerTitleHeader = false;
		protected string _tocLevel = "2 - Book and Chapter";
		protected string _embedFonts = "Yes";
		protected string _includeFontVariants = "Yes";
		public string MediaTypeEXE;
		public string StyleEXE = string.Empty;
		protected string _lastSelectedLayout = string.Empty;
		protected string _selectedStyle;
		TabPage tabDisplay = new TabPage();
		TabPage tabmob = new TabPage();
		TabPage tabothers = new TabPage();
		TabPage tabweb = new TabPage();
		TabPage tabpreview = new TabPage();
		TabPage tabDict4Mids = new TabPage();
		protected TraceSwitch _traceOn = new TraceSwitch("General", "Trace level for application");
		public ConfigurationTool cTool;
		Dictionary<string, string> pageDict = new Dictionary<string, string>();
		protected string _includeFootnoteCaller;
		protected string _includeXRefCaller;
		protected bool _hideVerseNumberOne;
		protected bool _splitFileByLetter = false;
		protected bool _disableWidowOrphan;

		#endregion
		
		#region Properties

		public string MarginLeft
		{
			get
			{
				string task = "@page";
				string key = "margin-left";
				return GetValue(task, key, "0");
			}
		}

		public string MarginRight
		{
			get
			{
				string task = "@page";
				string key = "margin-right";
				return GetValue(task, key, "0");
			}
		}

		public string MarginTop
		{
			get
			{
				string task = "@page";
				string key = "margin-top";
				return GetValue(task, key, "0");
			}
		}

		public string CustomFootnoteCaller
		{
			get
			{
				string task = "@page";
				string key = "-ps-custom-footnote-caller";
				return GetValue(task, key, "Default");
			}
		}

		public string CustomXRefCaller
		{
			get
			{
				string task = "@page";
				string key = "-ps-custom-xref-caller";
				return GetValue(task, key, "Default");
			}
		}

		public string HideVerseNumberOne
		{
			get
			{
				string task = "@page";
				string key = "-ps-hide-versenumber-one";
				return GetValue(task, key, "False");
			}
		}

		public string HideSpaceVerseNumber
		{
			get
			{
				string task = "@page";
				string key = "-ps-hide-space-versenumber";
				return GetValue(task, key, "False");
			}
		}

		public string MarginBottom
		{
			get
			{
				string task = "@page";
				string key = "margin-bottom";
				return GetValue(task, key, "0");
			}
		}

		public string RunningHeader
		{
			get
			{
				string defaultValue = string.Empty;
				string task = "@page:left-top-left"; // Page left top left
				string key = "content";
				string result = GetValue(task, key, "false");
				if (_loadType == "Dictionary")
				{
					if (result.IndexOf("page") > 0 || result.IndexOf("guideword") > 0)
					{
						return "Mirrored";
					}
					if (result.ToLower() == "none")
					{
						return "None";
					}
					defaultValue = "Every Page";
				}
				else
				{
					if (result.IndexOf("bookname") > 0 || result.IndexOf("chapter") > 0 || result.IndexOf("page") > 0 || result.IndexOf("guideword") > 0)
					{
						return "Mirrored";
					}
					if (result.ToLower() == "none")
					{
						return "None";
					}
					defaultValue = "Every Page";
				}
				return defaultValue;
			}
		}

		public string ReferenceFormat
		{
			get
			{
				string defaultValue = string.Empty;

				string key = "-ps-referenceformat";
				string task = string.Empty;
				string result = string.Empty;

				if (GetDdlRunningHead().ToLower() == "mirrored")
				{
					task = "@page-top-center";
					result = GetPageValue(task, key, "false");
					if (result.Length > 0)
					{
						if (!ComboBoxContains(result, cTool.DdlReferenceFormat))
							return result;
					}

					task = "@page:left-top-left$@page:right-top-right";
					result = GetPageValue(task, key, "false");
					if (result.Length > 0)
					{
						if (!ComboBoxContains(result, cTool.DdlReferenceFormat))
							return result;
					}

					defaultValue = "Genesis 1-2";
				}
				else if (GetDdlRunningHead().ToLower() == "every page")
				{
					task = "@page-top-left$@page-top-right";
					result = GetPageValue(task, key, "false");
					if (result.Length > 0)
					{
						if (!ComboBoxContains(result, cTool.DdlReferenceFormat))
							return result;
					}

					defaultValue = "Genesis 1:1-2:1";
				}
				else if (GetDdlRunningHead().ToLower() == "none")
				{
					defaultValue = "None";
				}


				return defaultValue;
			}
		}

		public string PageNumber
		{
			get
			{
				const string defaultValue = "Top Center";
				if (_loadType == "Dictionary")
				{
					foreach (string srchKey in pageDict.Keys)
					{
						const string key = "content";
						string result = GetValue(srchKey, key, "false");
						if (result.IndexOf("page") > 0 || result == "none")
						{
							string pageNumberValue = pageDict[srchKey];
							if (!ComboBoxContains(pageNumberValue, cTool.DdlPageNumber))
								return pageDict[srchKey];
						}
					}
				}
				else
				{
					foreach (string srchKey in pageDict.Keys)
					{
						const string key = "content";
						string result = GetValue(srchKey, key, "false");
						if (result.IndexOf("page") > 0)
						{
							string pageNumberValue = pageDict[srchKey];
							if (!ComboBoxContains(pageNumberValue, cTool.DdlPageNumber))
								return pageDict[srchKey];
						}
					}
				}
				return defaultValue;
			}
		}

		public string ColumnCount
		{
			get
			{
				string task = "letData";
				if (_loadType == "Scripture")
				{
					task = "columns";
				}
				string key = "column-count";
				return GetValue(task, key, "1");
			}
		}

		public string ColumnRule
		{
			get
			{
				string task = "letData";
				if (_loadType == "Scripture")
				{
					task = "columns";
				}
				string key = "column-rule-width";
				string result = GetValue(task, key, "0");
				return result != "0" ? "Yes" : "No";
			}
		}

		public string GutterWidth
		{
			get
			{
				string task = "letData";
				if (_loadType == "Scripture")
				{
					task = "columns";
				}
				string key = "column-gap";
				string result = GetValue(task, key, "18");
				return result.Length > 0 ? result : "18";
			}
		}
		//TD-3607
		public string GuidewordLength
		{
			get
			{
				string task = "guidewordLength";
				string key = "guideword-length";
				string result = GetValue(task, key, "99");
				return Convert.ToInt32(result) > 0 ? result : "99";
			}
		}

		public string PageSize
		{
			get
			{
				string task = "@page";
				string key = "page-width";
				string key1 = "page-height";
				string width = GetValue(task, key, "612");
				width = Math.Round(double.Parse(width, CultureInfo.GetCultureInfo("en-US"))).ToString();
				string height = GetValue(task, key1, "792");
				height = Math.Round(double.Parse(height, CultureInfo.GetCultureInfo("en-US"))).ToString();
				string pageSize = PageSize1(width, height);
				return pageSize;
			}
		}

		public string Leading
		{
			get
			{
				string task = "entry";
				if (_loadType == "Scripture")
				{
					task = "Paragraph";
				}
				string key = "line-height";
				return GetValue(task, key, "No Change");
			}
		}

		public string FontSize
		{
			get
			{
				string task = "entry";
				if (_loadType == "Scripture")
				{
					task = "Paragraph";
				}
				string key = "font-size";
				return GetValue(task, key, "No Change");
			}
		}

        public string HeaderFontSize
        {
            get
            {
                string task = "@page";
                string key = "-ps-header-font-size";
	            string defValue = "Same as headword";
				if (_loadType == "Scripture")
				{
					defValue = "Same as section (\\s)";
				}
				return GetValue(task, key, defValue);
            }
        }

		public string JustifyUI
		{
			get
			{
				string task = "entry";
				if (_loadType == "Scripture")
				{
					task = "scrSection";
				}
				string key = "text-align";
				string align = GetValue(task, key, "No");
				if (align == "justify")
				{
					return "Yes";
				}
				return "No";
			}
		}

		public string VerticalJustify
		{
			get
			{
				string task = "@page";
				string key = "-ps-vertical-justification";
				string align = GetValue(task, key, "Top");
				return align;
			}
		}



		public string Picture
		{
			get
			{
				string result;
				string task = "pictureRight";
				string key = "display";
				string display = GetValue(task, key, "Yes");
				if (display == "block")
				{
					result = "Yes";
				}
				else if (display == "none")
				{
					result = "No";
				}
				else
				{
					result = "Yes";
				}
				return result;
			}
		}

		public string Sense
		{
			get
			{
				string task = "sense";
				if (_cssClass.ContainsKey(task) && _cssClass[task].ContainsKey("class-margin-left"))
				{
					return "Bullet";
				}
				return "No change";
			}
		}

		public string FileProduced
		{
			get
			{
				string task = "@page";
				string key = "-ps-fileproduce";
				string file = GetValue(task, key, "One");
				if (file != null)
					return file.Replace("\"", "");
				else
				{
					return "One";
				}
			}
		}

		public bool FixedLineHeight
		{
			get
			{
				string task = "@page";
				string key = "-ps-fixed-line-height";
				if (_cssClass.ContainsKey("@page") && _cssClass[task].ContainsKey(key))
				{
					return Convert.ToBoolean(_cssClass["@page"][key]);
				}
				return false;
			}
		}

		public bool DisableWidowOrphan
		{
			get
			{
				string task = "@page";
				string key = "-ps-disable-widow-orphan";
				if (_cssClass.ContainsKey("@page") && _cssClass[task].ContainsKey(key))
				{
					return Convert.ToBoolean(_cssClass["@page"][key]);
				}
				return false;
			}
		}

		public bool SplitFileByLetter
		{
			get
			{
				string task = "@page";
				string key = "-ps-split-file-by-letter";
				if (_cssClass.ContainsKey("@page") && _cssClass[task].ContainsKey(key))
				{
					return Convert.ToBoolean(_cssClass["@page"][key]);
				}
				return false;
			}
		}

        public bool CenterTitleHeader
        {
            get
            {
                string task = "@page";
                string key = "-ps-center-title-header";
                if (_cssClass.ContainsKey("@page") && _cssClass[task].ContainsKey(key))
                {
                    return Convert.ToBoolean(_cssClass["@page"][key]);
                }
                return false;
            }
        }

		#endregion

		#region Constructor
		public ConfigurationToolBL()
		{
			standardSize["595x842"] = "A4";
			standardSize["420x595"] = "A5";
			standardSize["499x709"] = "B5";
			standardSize["459x649"] = "C5";
			standardSize["298x420"] = "A6";
			standardSize["612x792"] = "Letter";
			standardSize["396x612"] = "Half letter";
			standardSize["378x594"] = "5.25in x 8.25in";
			standardSize["418x626"] = "5.8in x 8.7in";
			standardSize["432x648"] = "6in x 9in";
			standardSize["468x648"] = "6.5in x 9in on letter";

			_screenMode = ScreenMode.Load;

			//Mirrored
			pageDict.Add("@page:none-none", "None");
			pageDict.Add("@page:left-top-right", "Top Inside Margin");
			pageDict.Add("@page:left-top-left", "Top Outside Margin");
			pageDict.Add("@page:left-top-center", "Top Center");
			pageDict.Add("@page:left-bottom-right", "Bottom Inside Margin");
			pageDict.Add("@page:left-bottom-left", "Bottom Outside Margin");
			pageDict.Add("@page:left-bottom-center", "Bottom Center");
			//Every Page
			pageDict.Add("@page-top-left", "Top Left Margin");
			pageDict.Add("@page-top-right", "Top Right Margin");
			pageDict.Add("@page-top-center", "Top Center");
			pageDict.Add("@page-bottom-right", "Bottom Right Margin");
			pageDict.Add("@page-bottom-left", "Bottom Left Margin");
			pageDict.Add("@page-bottom-center", "Bottom Center");
			_caption = LocalizationManager.GetString("ConfigurationToolBL.MessageBoxCaption.projectname", "Pathway Configuration Tool", "");
			ColumnHeaderAddLocalization();
		}

		public ConfigurationToolBL(ConfigurationTool ctool)
			: this()
		{
			cTool = ctool;
		}
		private static void ColumnHeaderAddLocalization()
		{
			if (!Common.Testing)
			{
				Common.SetupLocalization();
				InitializeGridColumnHeader();
			}
		}

		#endregion

		#region Methods

		public void ConfigurationTool_LoadBL()
		{
			IsUnixOs = Common.UnixVersionCheck();
			_screenMode = ScreenMode.Load;
			_lastSelectedLayout = StyleEXE;
			Trace.WriteLineIf(_traceOn.Level == TraceLevel.Verbose, "ConfigurationTool_Load");

			if (cTool.TabControl1.TabPages["tabdisplay"] != null)
				tabDisplay = cTool.TabControl1.TabPages["tabdisplay"];
			if (cTool.TabControl1.TabPages["tabPreview"] != null)
				tabpreview = cTool.TabControl1.TabPages["tabPreview"];
			if (cTool.TabControl1.TabPages.Count > 2)
			{
				if (cTool.TabControl1.TabPages["tabmobile"] != null)
					tabmob = cTool.TabControl1.TabPages["tabmobile"];
				if (cTool.TabControl1.TabPages["tabothers"] != null)
					tabothers = cTool.TabControl1.TabPages["tabothers"];
				if (cTool.TabControl1.TabPages["tabweb"] != null)
					tabweb = cTool.TabControl1.TabPages["tabweb"];
				if (cTool.TabControl1.TabPages["tabDict4Mids"] != null)
					tabDict4Mids = cTool.TabControl1.TabPages["tabDict4Mids"];

				string[] removeTabs = { "tabmobile", "tabothers", "tabweb", "tabPicture", "tabDict4Mids" };

				foreach (var removeTab in removeTabs)
				{
					if (cTool.TabControl1.TabPages[removeTab] != null)
					{
						cTool.TabControl1.TabPages.Remove(cTool.TabControl1.TabPages[removeTab]);
					}
				}
			}

			if (IsUnixOs)
			{
				cTool.WindowState = FormWindowState.Maximized;
			}
			else
			{
				cTool.MinimumSize = new Size(497, 183);
				cTool.Width = Screen.PrimaryScreen.WorkingArea.Size.Width;
				cTool.Width = cTool.Width < 1175 ? cTool.Width : 1175;
			}
			if (cTool.IsExportOptionFromFlexOrParatext)
			{
				cTool.BtnScripture.Visible = false;
				cTool.BtnDictionary.Visible = false;
			}
			string[] files = Directory.GetFiles(Common.AssemblyPath, "Paratext*.dll", SearchOption.AllDirectories);
			if (files.Length <= 0)
			{
				cTool.BtnScripture.Visible = false;
				inputTypeBL = "Dictionary";
			}
			cTool.LoadSettings();
			SetInputTypeButton();
			ShowInputTypeButton();
			CreateGridColumn();
			LoadParam(); // Load DictionaryStyleSettings / ScriptureStyleSettings
			ShowDataInGrid();

			SetPreviousLayoutSelect(cTool.StylesGrid);
			PopulateFeatureSheet(); //For TD-1194 // Load Default Values
			SetMediaType();

			// Window title (includes the version and edition (BTE / SE))
			var sb = new StringBuilder();
			sb.Append("Pathway Configuration Tool");
			sb.Append(File.Exists(Common.FromRegistry("ScriptureStyleSettings.xml")) ? " - BTE " : " - SE ");
			sb.Append(AssemblyFileVersion);
			cTool.Text = sb.ToString();
			SetFocusToName();
			SetMenuToolStrip();
			//For the task TD-1481
			cTool.BtnOthers.Enabled = true;

			_screenMode = ScreenMode.View;
			ShowInfoValue();
			_screenMode = ScreenMode.Edit;
		}

		private static void InitializeGridColumnHeader()
		{
			if (Common.L10NMngr != null)
			{
				Common.L10NMngr.AddString("ConfigurationTool.Column.Name", "Name", "", "", "");
				Common.L10NMngr.AddString("ConfigurationTool.Column.Description", "Description", "", "", "");
				Common.L10NMngr.AddString("ConfigurationTool.Column.Comment", "Comment", "", "", "");
				Common.L10NMngr.AddString("ConfigurationTool.Column.Type", "Type", "", "", "");
				Common.L10NMngr.AddString("ConfigurationTool.Column.Shown", "Shown", "", "", "");
			}
		}

		public void SetClassReference(ConfigurationTool configTool)
		{
			cTool = configTool;
		}

		protected void SetInputTypeButton()
		{
			Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: SetInputTypeButton");
			if (inputTypeBL.ToLower() == "scripture")
			{
				cTool.BtnScripture.BackColor = _selectedInputTypeColor;
				cTool.BtnDictionary.BackColor = _deSelectedColor;
				cTool.BtnScripture.Focus();
				cTool.LblSenseLayout.Visible = false;
				cTool.DdlSense.Visible = false;
				cTool.BtnPaper.Enabled = true;
				cTool.BtnWeb.Enabled = false;
				cTool.BtnOthers.Enabled = true;
				cTool.BtnMobile.Enabled = true;
			}
			else
			{
				cTool.BtnDictionary.BackColor = _selectedInputTypeColor;
				cTool.BtnScripture.BackColor = _deSelectedColor;
				cTool.BtnDictionary.Focus();
				cTool.LblSenseLayout.Visible = true;
				cTool.DdlSense.Visible = true;
				cTool.BtnPaper.Enabled = true;
				cTool.BtnWeb.Enabled = true;
				cTool.BtnOthers.Enabled = true;
				cTool.BtnMobile.Enabled = true;
			}
		}

		/// <summary>
		/// For TD-1745, To hidden the Dictionary and Scripture button in ConfigurationTool when it called from FLEX/Paratext
		/// </summary>
		protected void ShowInputTypeButton()
		{
			Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: SetInputTypeButton");
			string inpType = cTool.InputType;
			if (inpType.Length > 0)
			{
				if (inpType.ToLower() == "scripture")
				{
					btnScripture_ClickBL();
				}
				else
				{
					btnDictionary_ClickBL();
				}				
			}			
		}

		public string AssemblyFileVersion
		{
			get
			{
				//string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
				
				//return  version;

				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyFileVersionAttribute)attributes[0]).Version;
			}
		}

		/// <summary>
		/// Method to add the file location to copy the attached files.
		/// </summary>
		/// <param name="projType">Dictionary / Scipture</param>
		/// <param name="MailBody">Existing content</param>
		/// <returns></returns>
		protected string GetMailBody(string projType, string MailBody)
		{
			if (projType.Length <= 0) return MailBody;
			MailBody += "Copy the settings file (" + projType + "StyleSettings.xml and StyleSettings.xsd) to the path" + "%0D";
			MailBody += "-------------------------------------------------------------------------------------------------------------" + "%0D%0A";
			if (!IsUnixOs)
			{
				MailBody += "Windows Vista or Windows 7 and 8:" + "%0D%0A";
				MailBody += @"C:\Users\All Users\Application Data\SIL\Pathway\" + projType + "%0D" + "%0D%0A";
				MailBody += "Windows XP:" + "%0D%0A";
				MailBody += @"C:\Documents and Settings\All Users\Application Data\SIL\Pathway\" + projType + "%0D%0A";
			}
			else
			{
				MailBody += "Ubuntu 12.04 (precise) or later version:" + "%0D%0A";
				MailBody += @"~/.local/share/SIL/Pathway/" + projType + "%0D%0A" + "%0D%0A";
			}

			MailBody += "Copy the all css files to the path" + "%0D%0A";
			MailBody += "------------------------------------------" + "%0D%0A";

			if (!IsUnixOs)
			{
				MailBody += "Windows Vista or Windows 7 and 8:" + "%0D%0A";
				MailBody += @"C:\Users\All Users\Application Data\SIL\Pathway\" + projType + "%0D" + "%0D%0A";

				MailBody += "Windows XP:" + "%0D%0A";
				MailBody += @"C:\Documents and Settings\All Users\Application Data\SIL\Pathway\" + projType + "%0D%0A";
			}
			else
			{
				MailBody += "Ubuntu 12.04 (precise) or later version:" + "%0D%0A";
				MailBody += @"~/.local/share/SIL/Pathway/" + projType + "%0D%0A";
			}
			return MailBody;
		}

		protected static string GetProjType()
		{
			string projType = string.Empty;
			if (Param.Value["InputType"] != null)
			{
				projType = Param.Value["InputType"];
			}
			return projType;
		}

		protected void SetFocusToName()
		{
			Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: SetFocusToName");
			cTool.TabControl1.SelectedTab = cTool.TabControl1.TabPages[0];
			cTool.TxtName.Focus();
		}

		protected void ShowDataInGrid()
		{
			Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: ShowDataInGrid");
			_selectedStyle = Param.Value["LayoutSelected"];
			cTool.TabControl1.SelectedTab = cTool.TabControl1.TabPages[0];
			cTool.LblType.Text = inputTypeBL;
			ShowStyleInGrid(cTool.StylesGrid, _cssNames);
			GridColumnWidth(cTool.StylesGrid);
			PreviousValue = cTool.TxtName.Text;
		}

		protected void SetSideBar()
		{
			cTool.BtnPaper.BackColor = _deSelectedColor;
			cTool.BtnMobile.BackColor = _deSelectedColor;
			cTool.BtnWeb.BackColor = _deSelectedColor;
			cTool.BtnOthers.BackColor = _deSelectedColor;

			cTool.BtnPaper.FlatAppearance.BorderSize = 0;
			cTool.BtnMobile.FlatAppearance.BorderSize = 0;
			cTool.BtnWeb.FlatAppearance.BorderSize = 0;
			cTool.BtnOthers.FlatAppearance.BorderSize = 0;
			if (MediaType == "paper")
			{
				cTool.BtnPaper.BackColor = _selectedColor;
				cTool.BtnPaper.FlatAppearance.BorderSize = 1;
				cTool.TsPreview.Enabled = true;
			}
			else if (MediaType == "mobile")
			{
				cTool.BtnMobile.BackColor = _selectedColor;
				cTool.BtnMobile.FlatAppearance.BorderSize = 1;
				cTool.TsPreview.Enabled = false;
			}
			else if (MediaType == "web")
			{
				cTool.BtnWeb.BackColor = _selectedColor;
				cTool.BtnWeb.FlatAppearance.BorderSize = 1;
				cTool.TsPreview.Enabled = false;
			}
			else if (MediaType == "others")
			{
				cTool.BtnOthers.BackColor = _selectedColor;
				cTool.BtnOthers.FlatAppearance.BorderSize = 1;
				cTool.TsPreview.Enabled = true;
			}
		}

		public void WriteCss()
		{
			if (MediaType.ToLower() == "web" && IsPropertyModified() == true)
			{
				if (ValidateWebAttributes())
				{
					_validateWebInput = true;
					XmlNodeList baseNodeList = Param.GetItems("//styles/" + MediaType + "/style[@name='" + StyleName + "']/styleProperty");
					HashUtilities hashUtil = new HashUtilities();
					hashUtil.Key = "%:#@?,*&";
					hashUtil.Salt = "$%^&*#$%";
					foreach (XmlNode baseNode in baseNodeList)
					{
						string attribName = baseNode.Attributes["name"].Value.ToLower();
						SetAttributesForWebProperties(attribName, baseNode, hashUtil);
					}
					Param.Write();
				}
				else
				{
					_validateWebInput = false;
					return;
				}
			}

			if (IsPropertyModified() == false || (FileType.ToLower() == "standard")) return;
			if (MediaType.ToLower() != "web")
			{
				try
				{
					string path = Param.Value["UserSheetPath"]; // all user path
					string file = Common.PathCombine(path, FileName);
					string importStatement = string.Empty;

					if (File.Exists(file))
					{
						//Reading the existing file for 1st Line (@import statement)
						var sr = new StreamReader(file);
						while ((importStatement = sr.ReadLine()) != null)
						{
							if (importStatement.Contains("@import"))
							{
								break;
							}
						}
						sr.Close();
					}

					//Start Writing the Changes
					var writeCss = new StreamWriter(file);
					if (!string.IsNullOrEmpty(importStatement))
						writeCss.WriteLine(importStatement);

					// changes for paper media
					if (MediaType == "paper")
					{
						SetAttributesForPaperProperties(writeCss);
					}

					if (MediaType.ToLower() == "others")
					{
						SetAttributesForOtherProperties(writeCss);
					}
					// write out the changes
					writeCss.Flush();
					writeCss.Close();

					PreviewFileName1 = "";
					PreviewFileName2 = "";

					if (cTool.StylesGrid.Rows.Count >= SelectedRowIndex)
					{
						if (PreviewFileName1.Trim().Length > 0 && PreviewFileName2.Trim().Length > 0)
						{
							cTool.StylesGrid[PreviewFile1, SelectedRowIndex].Value = PreviewFileName1;
							cTool.StylesGrid[PreviewFile2, SelectedRowIndex].Value = PreviewFileName2;
							XmlNode baseNode =
								Param.GetItem("//styles/" + MediaType + "/style[@name='" + StyleName + "']");
							Param.SetAttrValue(baseNode, "previewfile1", PreviewFileName1);
							Param.SetAttrValue(baseNode, "previewfile2", PreviewFileName1);
							Param.Write();
						}
					}
				}
				catch (Exception ex)
				{
					var confirmationStringMessage = LocalizationManager.GetString("ConfigurationToolBL.NoDuplicateStyleName.Message",
						"Sorry, your recent changes cannot be saved because Pathway cannot find the stylesheet file '{0}'", "");
					confirmationStringMessage = string.Format(confirmationStringMessage, ex.Message);
					var caption = LocalizationManager.GetString("ConfigurationToolBL.MessageBoxCaption.Caption", _caption, "");
					Utils.MsgBox(confirmationStringMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
									MessageBoxDefaultButton.Button1);
				}
			}
			_screenMode = ScreenMode.Edit;
		}

		private void SetAttributesForPaperProperties(StreamWriter writeCss)
		{
			var value = new Dictionary<string, string>();
			string attribute = string.Empty;
			string key = string.Empty;
			if(cTool.DdlJustified.SelectedItem != null)
			{
				attribute = "Justified";
				key = ((ComboBoxItem) cTool.DdlJustified.SelectedItem).Value;
				WriteAtImport(writeCss, attribute, key);
			}

			if (cTool.DdlVerticalJustify.SelectedItem != null)
			{
				attribute = "VerticalJustify";
				key = ((ComboBoxItem) cTool.DdlVerticalJustify.SelectedItem).Value;
				WriteAtImport(writeCss, attribute, key);
			}

			if (cTool.DdlPagePageSize.SelectedItem != null)
			{
				attribute = "Page Size";
				key = ((ComboBoxItem) cTool.DdlPagePageSize.SelectedItem).Value;
				WriteAtImport(writeCss, attribute, key);
			}

			if (cTool.DdlPageColumn.SelectedItem != null)
			{
				attribute = "Columns";
				key = ((ComboBoxItem) cTool.DdlPageColumn.SelectedItem).Value;
				WriteAtImport(writeCss, attribute, key);
			}

			if (cTool.DdlFontSize.SelectedItem != null)
			{
				attribute = "Font Size";
				key = ((ComboBoxItem) cTool.DdlFontSize.SelectedItem).Value;
				WriteAtImport(writeCss, attribute, key);
			}

			if (cTool.DdlLeading.SelectedItem != null)
			{
				attribute = "Leading";
				key = ((ComboBoxItem) cTool.DdlLeading.SelectedItem).Value;
				WriteAtImport(writeCss, attribute, key);
			}

			if (cTool.DdlPicture.SelectedItem != null)
			{
				attribute = "Pictures";
				key = ((ComboBoxItem) cTool.DdlPicture.SelectedItem).Value;
				WriteAtImport(writeCss, attribute, key);
			}

			if (cTool.DdlRunningHead.SelectedItem != null)
			{
				attribute = "Running Head";
				key = ((ComboBoxItem) cTool.DdlRunningHead.SelectedItem).Value;
				WriteAtImport(writeCss, attribute, key);
			}

			if (cTool.DdlHeaderFontSize.SelectedItem != null)
			{
				attribute = "Header Size";
				key = ((ComboBoxItem) cTool.DdlHeaderFontSize.SelectedItem).Value;
				WriteAtImport(writeCss, attribute, key);
			}

			if (inputTypeBL.ToLower() == "scripture" && cTool.DdlReferenceFormat.SelectedItem != null)
			{
				attribute = "Reference Format";
				key = ((ComboBoxItem)cTool.DdlReferenceFormat.SelectedItem).Value;
				WriteAtImport(writeCss, attribute, key);
			}

			if (cTool.DdlPageNumber.SelectedItem != null)
			{
				attribute = "Page Number";
				key = ((ComboBoxItem) cTool.DdlPageNumber.SelectedItem).Value;
				WriteAtImport(writeCss, attribute, key);
			}

			if (cTool.DdlRules.SelectedItem != null)
			{
				attribute = "Rules";
				key = ((ComboBoxItem) cTool.DdlRules.SelectedItem).Value;
				WriteAtImport(writeCss, attribute, key);
			}

			if (inputTypeBL.ToLower() == "dictionary" && cTool.DdlSense.Items.Count > 0 && cTool.DdlSense.SelectedItem != null)
			{
				attribute = "Sense";
				key = ((ComboBoxItem)cTool.DdlSense.SelectedItem).Value;
				WriteAtImport(writeCss, attribute, key);
			}

			//Writing TextBox Values into Css
			if (cTool.TxtPageGutterWidth.Text.Length > 0)
			{
				value["column-gap"] = cTool.TxtPageGutterWidth.Text;
				if (_loadType == "Scripture")
				{
					WriteCssClass(writeCss, "columns", value);
				}
				else
				{
					WriteCssClass(writeCss, "letData", value);
				}
			}
			value.Clear();
			//TD-3607
			if (cTool.TxtGuidewordLength.Text.Length > 0 && inputTypeBL.ToLower() == "dictionary")
			{
				int a;
				if (int.TryParse(cTool.TxtGuidewordLength.Text, out a))
				{
					value["guideword-length"] = Convert.ToInt16(cTool.TxtGuidewordLength.Text).ToString();
				}
				WriteCssClass(writeCss, "guidewordLength", value);
			}


			value.Clear();
			value["margin-top"] = cTool.TxtPageTop.Text;
			value["margin-right"] = cTool.TxtPageOutside.Text;
			value["margin-bottom"] = cTool.TxtPageBottom.Text;
			value["margin-left"] = cTool.TxtPageInside.Text;
			var fileProduce = string.Empty;
			if (cTool.DdlFileProduceDict.SelectedItem != null)
			{
				fileProduce = ((ComboBoxItem) cTool.DdlFileProduceDict.SelectedItem).Value;
			}
			value["-ps-fileproduce"] = "\"" + fileProduce + "\"";
			value["-ps-fixed-line-height"] = "\"" + _fixedLineHeight + "\"";
			value["-ps-split-file-by-letter"] = "\"" + _splitFileByLetter + "\"";
			value["-ps-center-title-header"] = "\"" + _centerTitleHeader + "\"";
			var hFontSizeValue = string.Empty;
			if (cTool.DdlHeaderFontSize.SelectedItem != null)
			{
				hFontSizeValue = ((ComboBoxItem) cTool.DdlHeaderFontSize.SelectedItem).Value;
			}
			value["-ps-header-font-size"] = "\"" + hFontSizeValue + "\"";
			if (inputTypeBL.ToLower() == "scripture")
			{
				value["-ps-custom-footnote-caller"] = "\"" + cTool.TxtFnCallerSymbol.Text + "\"";
				value["-ps-custom-XRef-caller"] = "\"" + cTool.TxtXrefCusSymbol.Text + "\"";
				value["-ps-hide-versenumber-one"] = "\"" + cTool.ChkTurnOffFirstVerse.Checked + "\"";
				value["-ps-hide-space-versenumber"] = "\"" + cTool.ChkHideSpaceVerseNo.Checked + "\"";
			}
			value["-ps-disable-widow-orphan"] = "\"" + _disableWidowOrphan + "\"";
			WriteCssClass(writeCss, "page", value);

			if (cTool.DdlRunningHead.SelectedItem != null)
			{
			if (((ComboBoxItem)cTool.DdlRunningHead.SelectedItem).Value.ToLower() == "mirrored")
			{
				value.Clear();
				value["margin-right"] = cTool.TxtPageInside.Text;
				value["margin-left"] = cTool.TxtPageOutside.Text;
				WriteCssClass(writeCss, "page :left", value);

				value.Clear();
				value["margin-right"] = cTool.TxtPageOutside.Text;
				value["margin-left"] = cTool.TxtPageInside.Text;
				WriteCssClass(writeCss, "page :right", value);
			}
			}

			if (_centerTitleHeader)
			{
				if (cTool.DdlRunningHead.SelectedItem != null)
				{
				if (((ComboBoxItem)cTool.DdlRunningHead.SelectedItem).Value.ToLower() == "mirrored")
				{
					SetPageTopCenter(value);
					WriteCssClass(writeCss, "page :left-top-center", value);

					SetPageTopCenter(value);
					WriteCssClass(writeCss, "page :right-top-center", value);
				}
				else
				{
					SetPageTopCenter(value);
					WriteCssClass(writeCss, "page -top-center", value);
				}
			}
		}
		}

		private void SetAttributesForOtherProperties(StreamWriter writeCss)
		{
			string alignment = string.Empty;

			if (cTool.DdlJustified.SelectedItem != null)
			{
				if (cTool.DdlDefaultAlignment.SelectedItem != null)
				{
					alignment = ((ComboBoxItem)cTool.DdlDefaultAlignment.SelectedItem).Value.ToString(CultureInfo.InvariantCulture);
				}
			}

			string epubStyleProperties = "body {  \r\n font-size:" + cTool.TxtBaseFontSize.Text + "pt; \r\n" +
				" line-height:" + cTool.TxtDefaultLineHeight.Text + "%; \r\n" + "} \r\n";

			string imageWidth = "img{ \r\n width:" + cTool.TxtMaxImageWidth.Text + "px; \r\n }";

			if (inputTypeBL.ToLower() == "dictionary")
			{
				string otherStyleProperties = ".entry { line-height:" + cTool.TxtDefaultLineHeight.Text + "%; \r\n" + "} \r\n";
				otherStyleProperties = otherStyleProperties + "\r\n .entry { line-height:" + cTool.TxtDefaultLineHeight.Text +
				                       "%; \r\n" + "} \r\n";
				otherStyleProperties = otherStyleProperties + "\r\n .entry .picture { line-height:" +
				                       cTool.TxtDefaultLineHeight.Text + "%; \r\n" + "} \r\n";
				otherStyleProperties = otherStyleProperties + "\r\n .entry .subentries .subentry { line-height:" +
				                       cTool.TxtDefaultLineHeight.Text + "%; \r\n" + "} \r\n";
				otherStyleProperties = otherStyleProperties + "\r\n .minorentrycomplex { line-height:" +
				                       cTool.TxtDefaultLineHeight.Text + "%; \r\n" + "} \r\n";
				otherStyleProperties = otherStyleProperties + "\r\n .minorentryvariant { line-height:" +
				                       cTool.TxtDefaultLineHeight.Text + "%; \r\n" + "} \r\n";
				otherStyleProperties = otherStyleProperties + "\r\n .mainentrysubentries .picture { line-height:" +
				                       cTool.TxtDefaultLineHeight.Text + "%; \r\n" + "} \r\n";
				otherStyleProperties = otherStyleProperties + "\r\n .xhomographnumber { line-height:" +
				                       cTool.TxtDefaultLineHeight.Text + "%; \r\n" + "} \r\n";
				otherStyleProperties = otherStyleProperties + "\r\n .letter { line-height:" + cTool.TxtDefaultLineHeight.Text +
				                       "%; \r\n" + "} \r\n";

				
				if (alignment.ToLower() == "right")
					writeCss.WriteLine("@import \"" + "epub_DictionaryRightAlign.css" + "\";");

				writeCss.WriteLine(otherStyleProperties);
			}
			else
			{
				if (alignment.ToLower() == "right")
					writeCss.WriteLine("@import \"" + "epub_ScriptureRightAlign.css" + "\";");
			}

			writeCss.WriteLine(imageWidth);			
			writeCss.WriteLine(epubStyleProperties);
		}

	    private void SetPageTopCenter(Dictionary<string, string> value)
	    {
	        value.Clear();
	        value["content"] = "\"HeaderTitleLable\"";
            string fontSize = cTool.DdlHeaderFontSize.SelectedItem.ToString();
	        if (Regex.IsMatch(cTool.DdlHeaderFontSize.SelectedItem.ToString(), @"^\d+$"))
	        {
                fontSize = fontSize + "pt";
	        }
            value["font-size"] = fontSize;
	        value["font-weight"] = "bold";
	    }

	    private void SetAttributesForWebProperties(string attribName, XmlNode baseNode, HashUtilities hashUtil)
		{
			switch (attribName)
			{
				case "ftpaddress":
					baseNode.Attributes["value"].Value = cTool.TxtFtpAddress.Text;
					break;
				case "ftpuserid":
					baseNode.Attributes["value"].Value = cTool.TxtFtpUsername.Text;
					break;
				case "ftppwd":
					if (cTool.TxtFtpPassword.Text.Trim().Length > 0)
					{
						baseNode.Attributes["value"].Value = hashUtil.Encrypt(cTool.TxtFtpPassword.Text);
					}
					else
					{
						baseNode.Attributes["value"].Value = "";
					}
					break;
				case "dbservername":
					baseNode.Attributes["value"].Value = cTool.TxtSqlServerName.Text;
					break;
				case "dbname":
					baseNode.Attributes["value"].Value = cTool.TxtSqlDBName.Text;
					break;
				case "dbuserid":
					baseNode.Attributes["value"].Value = cTool.TxtSqlUsername.Text;
					break;
				case "dbpwd":
					if (cTool.TxtSqlPassword.Text.Trim().Length > 0)
					{
						baseNode.Attributes["value"].Value = hashUtil.Encrypt(cTool.TxtSqlPassword.Text);
					}
					else
					{
						baseNode.Attributes["value"].Value = "";
					}
					break;
				case "weburl":
					baseNode.Attributes["value"].Value = cTool.TxtWebUrl.Text;
					break;
				case "webadminusrnme":
					baseNode.Attributes["value"].Value = cTool.TxtWebAdminUsrNme.Text;
					break;
				case "webadminpwd":
					if (cTool.TxtWebAdminPwd.Text.Trim().Length > 0)
					{
						baseNode.Attributes["value"].Value = hashUtil.Encrypt(cTool.TxtWebAdminPwd.Text);
					}
					else
					{
						baseNode.Attributes["value"].Value = "";
					}
					break;
				case "webadminsitenme":
					baseNode.Attributes["value"].Value = cTool.TxtWebAdminSiteNme.Text;
					break;
				case "webemailid":
					baseNode.Attributes["value"].Value = cTool.TxtWebEmailId.Text;
					break;
				case "webftpfldrnme":
					baseNode.Attributes["value"].Value = cTool.TxtWebFtpFldrNme.Text;
					break;
				case "comment":
					baseNode.Attributes["value"].Value = cTool.TxtComment.Text;
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// transfers grid row values to InfoPanel
		///
		/// 
		/// </summary>
		protected void ShowInfoValue()
		{
			if (!(_screenMode == ScreenMode.View || _screenMode == ScreenMode.Edit))
				return;

			Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: ShowInfoValue");
			if (cTool.StylesGrid.RowCount > 0)
			{
				StyleName = cTool.StylesGrid[ColumnName, SelectedRowIndex].Value.ToString();
				if (cTool.StylesGrid[3, SelectedRowIndex].Value.ToString().ToLower() == "standard")
				{
					PreviewFileName1 = cTool.StylesGrid[PreviewFile1, SelectedRowIndex].Value.ToString();
					PreviewFileName2 = cTool.StylesGrid[PreviewFile2, SelectedRowIndex].Value.ToString();
				}
				else
				{
					PreviewFileName1 = "PreviewMessage.jpg";
					PreviewFileName2 = "PreviewMessage.jpg";
				}
				cTool.TxtName.Text = StyleName;
				SetInfoCaption(StyleName);
				FileName = cTool.StylesGrid[ColumnFile, SelectedRowIndex].Value.ToString();
				FileType = cTool.StylesGrid[ColumnType, SelectedRowIndex].Value.ToString();
				cTool.TxtCss.Text = cTool.StylesGrid[ColumnDescription, SelectedRowIndex].Value.ToString();
				cTool.TxtDesc.Text = cTool.StylesGrid[ColumnDescription, SelectedRowIndex].Value.ToString();
				cTool.TxtComment.Text = cTool.StylesGrid[ColumnComment, SelectedRowIndex].Value.ToString();
				bool check = cTool.StylesGrid[ColumnShown, SelectedRowIndex].Value.ToString().ToLower() == "yes" ? true : false;
				cTool.ChkAvailable.Checked = check;
				cTool.TxtApproved.Text = cTool.StylesGrid[ColumnApprovedBy, SelectedRowIndex].Value.ToString();
				string type = cTool.StylesGrid[ColumnType, SelectedRowIndex].Value.ToString();
				if (type == TypeStandard)
				{
					EnableDisablePanel(false);
					cTool.TxtApproved.Visible = true;
					cTool.LblApproved.Visible = true;
				}
				else
				{
					EnableDisablePanel(true);
					if (cTool.TxtName.Text.ToLower() == "oneweb") { cTool.TsDelete.Enabled = false; }
					if (cTool.BtnMobile.Text.ToLower() == "dictformids") { EnableDisablePanel(false); }
					cTool.TxtApproved.Visible = false;
					cTool.LblApproved.Visible = false;
				}
			}
			if (cTool.TabControl1.SelectedIndex == 1)
				ShowCSSValue();
			else if (cTool.TabControl1.SelectedTab.Text == "Preview")
				ShowPreview(1);
			_screenMode = ScreenMode.Edit;
		}

		/// <summary>
		/// Fills Values in Display property Tab and Properties Tab
		/// </summary>
		protected void ShowCSSValue()
		{
			_screenMode = ScreenMode.View;
			_errProvider.Clear();
			if (cTool.TxtName.Text.Length <= 0) return;
			string path = Param.StylePath(cTool.TxtName.Text);
			ParseCSS(path, Param.Value["InputType"]);

			double left = MarginLeft.Length > 0
							  ? Math.Round(double.Parse(MarginLeft, CultureInfo.GetCultureInfo("en-US")), 0)
							  : 0;
			cTool.TxtPageInside.Text = left + "pt";
			double right = MarginRight.Length > 0 ? Math.Round(double.Parse(MarginRight, CultureInfo.GetCultureInfo("en-US")), 0) : 0;
			cTool.TxtPageOutside.Text = right + "pt";
			double top = MarginTop.Length > 0 ? Math.Round(double.Parse(MarginTop, CultureInfo.GetCultureInfo("en-US")), 0) : 0;
			cTool.TxtPageTop.Text = top + "pt";
			double bottom = MarginBottom.Length > 0 ? Math.Round(double.Parse(MarginBottom, CultureInfo.GetCultureInfo("en-US")), 0) : 0;
			cTool.TxtPageBottom.Text = bottom + "pt";

			cTool.TxtPageGutterWidth.Text = GutterWidth;
			if (GutterWidth.IndexOf('%') == -1)
			{
				if (cTool.TxtPageGutterWidth.Text.Length > 0)
					cTool.TxtPageGutterWidth.Text = cTool.TxtPageGutterWidth.Text + "pt";
			}

			cTool.DdlPageColumn.SelectedItem = (ComboBoxItem)cTool.DdlPageColumn.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == ColumnCount);
			cTool.DdlFontSize.SelectedItem = (ComboBoxItem)cTool.DdlFontSize.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == FontSize);
			cTool.DdlLeading.SelectedItem = (ComboBoxItem)cTool.DdlLeading.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == Leading);
			cTool.ChkDisableWO.Checked = DisableWidowOrphan;
			cTool.ChkFixedLineHeight.Checked = FixedLineHeight;
			cTool.DdlPicture.SelectedItem = (ComboBoxItem)cTool.DdlPicture.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == Picture);
			cTool.DdlJustified.SelectedItem = (ComboBoxItem)cTool.DdlJustified.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == JustifyUI);
			cTool.DdlPagePageSize.SelectedItem = (ComboBoxItem)cTool.DdlPagePageSize.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == PageSize);
			cTool.DdlRunningHead.SelectedItem = (ComboBoxItem)cTool.DdlRunningHead.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == RunningHeader);
            cTool.DdlHeaderFontSize.SelectedItem = (ComboBoxItem)cTool.DdlHeaderFontSize.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == HeaderFontSize);
			cTool.ChkSplitFileByLetter.Checked = SplitFileByLetter;
		    cTool.ChkCenterTitleHeader.Checked = CenterTitleHeader;

			string pageType = GetDdlRunningHead();
			DdlRunningHeadSelectedIndexChangedBl(pageType);
			if (pageType != "None")
			{
				cTool.TxtGuidewordLength.Text = GuidewordLength;
			}
			SetScriptureValues();

			cTool.DdlPageNumber.SelectedItem = (ComboBoxItem)cTool.DdlPageNumber.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == PageNumber);
			cTool.DdlRules.SelectedItem = (ComboBoxItem)cTool.DdlRules.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == ColumnRule);
			cTool.DdlSense.SelectedItem = (ComboBoxItem)cTool.DdlSense.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == Sense);
			cTool.DdlVerticalJustify.SelectedItem = (ComboBoxItem)cTool.DdlVerticalJustify.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == VerticalJustify);
			try
			{
				if (inputTypeBL.ToLower() == "scripture" && MediaType.ToLower() == "mobile")
				{
					string filePath = string.Empty;
					if (File.Exists(Param.SettingOutputPath))
					{
						filePath = Param.SettingOutputPath;
					}
					else if (File.Exists(Param.SettingPath))
					{
						filePath = Param.SettingPath;
					}
					XmlNodeList baseNode1 = Param.GetItems("//styles/" + MediaType + "/style[@name='" + StyleName + "']/styleProperty");
					ShowMoblieCss(baseNode1);
					SetMobileSummary(null, null);
				}
				else if (MediaType.ToLower() == "others")
				{
					XmlNodeList baseNode1 = Param.GetItems("//styles/" + MediaType + "/style[@name='" + StyleName + "']/styleProperty");
					// show/hide epub UI controls based on the input type
					SetEpubUIControls(inputTypeBL == "Scripture");

					ShowOthersCss(baseNode1);
					SetOthersSummary(null, null);
				}
				else if (MediaType.ToLower() == "web")
				{
					XmlNodeList baseNode1 = Param.GetItems("//styles/" + MediaType + "/style[@name='" + StyleName + "']/styleProperty");
					HashUtilities hashUtil = new HashUtilities();
					hashUtil.Key = "%:#@?,*&";
					hashUtil.Salt = "$%^&*#$%";
					// show/hide web UI controls based on the input type
					SetWebUIControls(inputTypeBL == "Dictionary");

					ShowWebCss(baseNode1, hashUtil);
					SetWebSummary(null, null);
				}
				else
				{
					cTool.DdlFileProduceDict.SelectedItem = (ComboBoxItem)cTool.DdlFileProduceDict.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == FileProduced);
					ShowCssSummary();
					if (cTool.StylesGrid.RowCount > 0)
					{
						cTool.TxtCss.Text = cTool.StylesGrid[ColumnDescription, SelectedRowIndex].Value.ToString();
					}
				}
				SavePropertyValue();
			}
			catch
			{
			}
			_screenMode = ScreenMode.Edit;
		}

		private void SetScriptureValues()
		{
			if (inputTypeBL.ToLower() == "scripture")
			{
				cTool.DdlReferenceFormat.SelectedItem = (ComboBoxItem)cTool.DdlReferenceFormat.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == ReferenceFormat);
				if (CustomFootnoteCaller.ToLower() == "default")
				{
					cTool.ChkIncludeCusFnCaller.Checked = false;
					cTool.TxtFnCallerSymbol.Text = "";
				}
				else
				{
					cTool.ChkIncludeCusFnCaller.Checked = true;
					cTool.TxtFnCallerSymbol.Text = CustomFootnoteCaller;
				}
				if (CustomXRefCaller.ToLower() == "default")
				{
					cTool.ChkXrefCusSymbol.Checked = false;
					cTool.TxtXrefCusSymbol.Text = "";
				}
				else
				{
					cTool.ChkXrefCusSymbol.Checked = true;
					cTool.TxtXrefCusSymbol.Text = CustomXRefCaller;
				}
				cTool.ChkTurnOffFirstVerse.Checked = bool.Parse(HideVerseNumberOne);
				cTool.ChkHideSpaceVerseNo.Checked = bool.Parse(HideSpaceVerseNumber);
			}
		}

		private void ShowMoblieCss(XmlNodeList baseNode1)
		{
			foreach (XmlNode VARIABLE in baseNode1)
			{
				string attribName = VARIABLE.Attributes["name"].Value;
				string attribValue = VARIABLE.Attributes["value"].Value;
				if (attribName.ToLower() == "fileproduced")
				{
					cTool.DdlFiles.SelectedItem = (ComboBoxItem)cTool.DdlFiles.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
				}
				else if (attribName.ToLower() == "redletter")
				{
					cTool.DdlRedLetter.SelectedItem = (ComboBoxItem)cTool.DdlRedLetter.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
				}
				else if (attribName.ToLower() == "language")
				{
					cTool.DdlLanguage.SelectedItem = (ComboBoxItem)cTool.DdlLanguage.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
				}
			}
		}

		private void ShowOthersCss(XmlNodeList baseNode1)
		{
			foreach (XmlNode VARIABLE in baseNode1)
			{
				string attribName = VARIABLE.Attributes["name"].Value.ToLower();
				string attribValue = VARIABLE.Attributes["value"].Value;
				switch (attribName)
				{
					case "embedfonts":
						cTool.ChkEmbedFonts.Checked = (attribValue == "Yes") ? true : false;
						bool bEnabled = cTool.ChkEmbedFonts.Checked;
						cTool.ChkIncludeFontVariants.Enabled = bEnabled;
						cTool.DdlDefaultFont.Enabled = bEnabled;
						cTool.DdlMissingFont.Enabled = bEnabled;
						cTool.DdlNonSILFont.Enabled = bEnabled;
						break;
					case "includefontvariants":
						cTool.ChkIncludeFontVariants.Checked = (attribValue == "Yes") ? true : false;
						break;
					case "includeimage":
						cTool.ChkIncludeImage.Checked = (attribValue == "Yes") ? true : false;
						break;
					case "pagebreak":
						cTool.ChkPageBreaks.Checked = (attribValue == "Yes") ? true : false;
						break;
					case "maximagewidth":
						cTool.TxtMaxImageWidth.Text = attribValue;
						break;
					case "toclevel":
						cTool.DdlTocLevel.SelectedItem = (ComboBoxItem)cTool.DdlTocLevel.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
						break;
					case "basefontsize":
						cTool.TxtBaseFontSize.Text = attribValue;
						break;
					case "defaultlineheight":
						cTool.TxtDefaultLineHeight.Text = attribValue;
						break;
					case "defaultalignment":
						cTool.DdlDefaultAlignment.SelectedItem = (ComboBoxItem)cTool.DdlDefaultAlignment.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
						break;
					case "chapternumbers":
						cTool.DdlChapterNumbers.SelectedItem = (ComboBoxItem)cTool.DdlChapterNumbers.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
						break;
					case "references":
						cTool.DdlReferences.SelectedItem = (ComboBoxItem)cTool.DdlReferences.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
						break;
					case "defaultfont":
						cTool.DdlDefaultFont.SelectedItem = (ComboBoxItem)cTool.DdlDefaultFont.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
						break;
					case "missingfont":
						cTool.DdlMissingFont.SelectedItem = (ComboBoxItem)cTool.DdlMissingFont.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
						break;
					case "nonsilfont":
						cTool.DdlNonSILFont.SelectedItem = (ComboBoxItem)cTool.DdlNonSILFont.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
						break;
					default:
						break;
				}
			}
		}

		private void ShowWebCss(XmlNodeList baseNode1, HashUtilities hashUtil)
		{
			foreach (XmlNode VARIABLE in baseNode1)
			{
				string attribName = VARIABLE.Attributes["name"].Value.ToLower();
				string attribValue = VARIABLE.Attributes["value"].Value;
				switch (attribName)
				{
					case "ftpaddress":
						cTool.TxtFtpAddress.Text = attribValue;
						break;
					case "ftpuserid":
						cTool.TxtFtpUsername.Text = attribValue;
						break;
					case "ftppwd":
						if (attribValue.Trim().Length > 0)
						{
							cTool.TxtFtpPassword.Text = hashUtil.Decrypt(attribValue);
						}
						else
						{
							cTool.TxtFtpPassword.Text = "";
						}
						break;
					case "dbservername":
						cTool.TxtSqlServerName.Text = attribValue;
						break;
					case "dbname":
						cTool.TxtSqlDBName.Text = attribValue;
						break;
					case "dbuserid":
						cTool.TxtSqlUsername.Text = attribValue;
						break;
					case "dbpwd":
						if (attribValue.Trim().Length > 0)
						{
							cTool.TxtSqlPassword.Text = hashUtil.Decrypt(attribValue);
						}
						else
						{
							cTool.TxtSqlPassword.Text = "";
						}
						break;
					case "weburl":
						cTool.TxtWebUrl.Text = attribValue;
						break;
					case "webadminusrnme":
						cTool.TxtWebAdminUsrNme.Text = attribValue;
						break;
					case "webadminpwd":
						if (attribValue.Trim().Length > 0)
						{
							cTool.TxtWebAdminPwd.Text = hashUtil.Decrypt(attribValue);
						}
						else
						{
							cTool.TxtWebAdminPwd.Text = "";
						}
						break;
					case "webadminsitenme":
						cTool.TxtWebAdminSiteNme.Text = attribValue;
						break;
					case "webemailid":
						cTool.TxtWebEmailId.Text = attribValue;
						break;
					case "webftpfldrnme":
						cTool.TxtWebFtpFldrNme.Text = attribValue;
						break;
					case "comment":
						cTool.TxtComment.Text = attribValue;
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Save property Initial Value
		/// </summary>
		private void SavePropertyValue()
		{
			Control.ControlCollection ctls = cTool.TabControl1.TabPages[1].Controls;
			_propertyValue.Clear();
			_groupPropertyValue.Clear();
			foreach (Control control in ctls)
			{
				if (control.GetType().Name == "Label")
					continue;

				if (control.GetType().Name == "GroupBox")
				{
					foreach (Control subControl in control.Controls)
					{
						_groupPropertyValue.Add(subControl.Text);
					}
				}

				if (control.GetType().Name == "TableLayoutPanel")
				{
					foreach (Control subControl in control.Controls)
					{
						_groupPropertyValue.Add(subControl.Text);
					}
				}
				string val = control.Text;
				_propertyValue.Add(val);
			}
		}

		private string CopiedToTempLanguageXMLFile(string languageXmlFile)
		{
			string fileName = "Languages.xml";
			string xmlFileNameWithPath = languageXmlFile;
			string tempFolder = Common.PathCombine(Path.GetTempPath(), "SILTemp");
			if (Directory.Exists(tempFolder))
			{
				try
				{
					DirectoryInfo di = new DirectoryInfo(tempFolder);
					Common.CleanDirectory(di);
				}
				catch
				{
					tempFolder = Common.PathCombine(Path.GetTempPath(),
													"SilPathWay" + Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
				}
			}
			Directory.CreateDirectory(tempFolder);
			string tempFile = Common.PathCombine(tempFolder, fileName);

			File.Copy(xmlFileNameWithPath, tempFile, true);
			return tempFile;
		}

		private void LoadData()
		{
			XmlDocument xDoc = Common.DeclareXMLDocument(false);
			string executablePath = Common.GetApplicationPath();
			executablePath = PathwayPath.GetSupportPath(executablePath, @"GoBible\Localizations\Languages.xml", true);
			executablePath = CopiedToTempLanguageXMLFile(executablePath);
			if (!File.Exists(executablePath)) return;
			xDoc.Load(executablePath);
			string XPath = "//Languages/language";
			XmlNodeList languageList = xDoc.SelectNodes(XPath);

			if (languageList.Count > 0)
			{
				cTool.DdlLanguage.Items.Clear();
				foreach (XmlNode language in languageList)
				{
					cTool.DdlLanguage.Items.Add(new ComboBoxItem(language.InnerText, language.InnerText));
				}
				cTool.DdlLanguage.Sorted = true;
			}
		}

		/// <summary>
		/// Fills Values in Display property Tab
		/// 
		/// </summary>
		protected void PopulateFeatureSheet()
		{

			if (inputTypeBL.ToLower() == "scripture")
			{
				LoadData();
			}
			//cTool.DdlTocLevel.Items.Clear();
			ClearDropdownListItems();
			Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: PopulateFeatureSheet");
			// populate the font drop-down if needed
			if (cTool.DdlDefaultFont.Items.Count == 0)
			{
				string[] files = FontInternals.GetInstalledFontFiles();
				PrivateFontCollection pfc = new PrivateFontCollection();
				foreach (var file in files)
				{
					if (FontInternals.IsSILFont(file))
					{
						pfc.AddFontFile(file);
					}
				}
				foreach (var fontFamily in pfc.Families)
				{
					cTool.DdlDefaultFont.Items.Add(new ComboBoxItem(fontFamily.GetName(0), fontFamily.GetName(0)));
					cTool.DdlDefaultFont.SelectedIndex = 0;
				}
			}
			TreeView TvFeatures = new TreeView();
			PopulateFeatureLists(TvFeatures);
			try
			{
				foreach (TreeNode tn in TvFeatures.Nodes)
				{
					string task = tn.Text;
					foreach (TreeNode ctn in tn.Nodes)
					{
						try
						{
							PopulateFeatureItemsInDropDownctrl(task, ctn);
						}
						catch{}
						//Exception handle for linux environment Treenode control for handling null exception raised when localization language is changed.
					}
				}
			}
			catch
			{
			}
		}

		private void ClearDropdownListItems()
		{
			cTool.DdlPagePageSize.Items.Clear();
			cTool.DdlPageColumn.Items.Clear();
			cTool.DdlLeading.Items.Clear();
			cTool.DdlFontSize.Items.Clear();
			cTool.DdlRunningHead.Items.Clear();
			cTool.DdlPageNumber.Items.Clear();
			cTool.DdlRules.Items.Clear();
			cTool.DdlPicture.Items.Clear();
			cTool.DdlSense.Items.Clear();
			cTool.DdlJustified.Items.Clear();
			cTool.DdlVerticalJustify.Items.Clear();
			cTool.DdlFiles.Items.Clear();
			cTool.DdlFileProduceDict.Items.Clear();
			cTool.DdlRedLetter.Items.Clear();
			cTool.DdlChapterNumbers.Items.Clear();
			cTool.DdlReferences.Items.Clear();
			cTool.DdlDefaultAlignment.Items.Clear();
			cTool.DdlMissingFont.Items.Clear();
			cTool.DdlNonSILFont.Items.Clear();
			cTool.DdlTocLevel.Items.Clear();
            cTool.DdlHeaderFontSize.Items.Clear();
		}

		private void PopulateFeatureItemsInDropDownctrl(string task, TreeNode ctn)
		{
			string enText = ctn.Text;
			try
			{
				ctn.Text = LocalizeItems.LocalizeItem(task, ctn.Text);
			}
			catch{}

			switch (task)
			{
				case "Page Size":
					if (ComboBoxContains(enText, cTool.DdlPagePageSize))
						cTool.DdlPagePageSize.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "Columns":
					if (ComboBoxContains(enText, cTool.DdlPageColumn))
						cTool.DdlPageColumn.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "Leading":
					if (ComboBoxContains(enText, cTool.DdlLeading))
						cTool.DdlLeading.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "Font Size":
					if (ComboBoxContains(enText, cTool.DdlFontSize))
						cTool.DdlFontSize.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "Running Head":
					if (ComboBoxContains(enText, cTool.DdlRunningHead))
						cTool.DdlRunningHead.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "Page Number":
					if (ComboBoxContains(enText, cTool.DdlPageNumber))
						cTool.DdlPageNumber.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "Rules":
					if (ComboBoxContains(enText, cTool.DdlRules))
						cTool.DdlRules.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "Pictures":
					if (ComboBoxContains(enText, cTool.DdlPicture))
						cTool.DdlPicture.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "Sense":
					if (ComboBoxContains(enText, cTool.DdlSense))
						cTool.DdlSense.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "Justified":
					if (ComboBoxContains(enText, cTool.DdlJustified))
						cTool.DdlJustified.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "VerticalJustify":
					if (ComboBoxContains(enText, cTool.DdlVerticalJustify))
						cTool.DdlVerticalJustify.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "FileProduced":
					if (ComboBoxContains(enText, cTool.DdlFiles))
						cTool.DdlFiles.Items.Add(new ComboBoxItem(enText, ctn.Text));
					if (ComboBoxContains(enText, cTool.DdlFileProduceDict))
						cTool.DdlFileProduceDict.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;
				case "RedLetter":
					if (ComboBoxContains(enText, cTool.DdlRedLetter))
						cTool.DdlRedLetter.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "ChapterNumbers":
					if (ComboBoxContains(enText, cTool.DdlChapterNumbers))
						cTool.DdlChapterNumbers.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "References":
					if (ComboBoxContains(enText, cTool.DdlReferences))
						cTool.DdlReferences.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "DefaultAlignment":
					if (ComboBoxContains(enText, cTool.DdlDefaultAlignment))
						cTool.DdlDefaultAlignment.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "MissingFont":
					if (ComboBoxContains(enText, cTool.DdlMissingFont))
						cTool.DdlMissingFont.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "NonSILFont":
					if (ComboBoxContains(enText, cTool.DdlNonSILFont))
						cTool.DdlNonSILFont.Items.Add(new ComboBoxItem(enText, ctn.Text));
					break;

				case "TOCLevel":
					if (ComboBoxContains(enText, cTool.DdlTocLevel))
					{
						cTool.DdlTocLevel.Items.Add(new ComboBoxItem(enText, ctn.Text));
						cTool.DdlTocLevel.SelectedIndex = 0;
					}
					break;

                case "Header Size":
                    if (ComboBoxContains(enText, cTool.DdlHeaderFontSize))
                        cTool.DdlHeaderFontSize.Items.Add(new ComboBoxItem(enText, ctn.Text));
                    break;
			}
		}

		private bool ComboBoxContains(string enText, ComboBox cbBox)
		{
			return cbBox.Items.OfType<ComboBoxItem>().All(s => s.Value != enText);
		}


		/// <summary>
		/// Fill the Tab Control values into Text Box
		/// </summary>
		public void ShowCssSummary()
		{
			try
			{
				if (inputTypeBL.ToLower() == "dictionary" && MediaType.ToLower() == "mobile")
				{
					cTool.TxtCss.Text = @"No custom properties for DictionaryForMIDs";
					return;
				}

				string leading = string.Empty;
				if (cTool.DdlLeading.SelectedItem != null)
					leading = (((ComboBoxItem)cTool.DdlLeading.SelectedItem).Value.Length > 0) ? " Line Spacing " + ((ComboBoxItem)cTool.DdlLeading.SelectedItem).Value + ", " : " ";

				string fontSize = string.Empty;
				if (cTool.DdlFontSize.SelectedItem != null)
					fontSize = (((ComboBoxItem)cTool.DdlFontSize.SelectedItem).Value.Length > 0) ? " Base FontSize - " + ((ComboBoxItem)cTool.DdlFontSize.SelectedItem).Value + ", " : "";

                string headerSize = string.Empty;
                if (cTool.DdlHeaderFontSize.SelectedItem != null)
                    headerSize = (((ComboBoxItem)cTool.DdlHeaderFontSize.SelectedItem).Value.Length > 0) ? " Header Size - " + ((ComboBoxItem)cTool.DdlHeaderFontSize.SelectedItem).Value + ", " : "";

				string rules = string.Empty;
				if (cTool.DdlRules.SelectedItem != null)
					rules = ((ComboBoxItem)cTool.DdlRules.SelectedItem).Value == "Yes" ? "With Divider Lines, " : "Without Divider Lines, ";

				string justified = string.Empty;
				if (cTool.DdlJustified.SelectedItem != null)
					justified = ((ComboBoxItem)cTool.DdlJustified.SelectedItem).Value == "Yes" ? "Justified, " : "";

				string picture = string.Empty;
				if (cTool.DdlPicture.SelectedItem != null)
					picture = ((ComboBoxItem)cTool.DdlPicture.SelectedItem).Value == "Yes" ? "With picture " : "Without picture ";

				string pageSize = string.Empty;
				if (cTool.DdlPagePageSize.SelectedItem != null)
					pageSize = (((ComboBoxItem)cTool.DdlPagePageSize.SelectedItem).Value.Length > 0) ? ((ComboBoxItem)cTool.DdlPagePageSize.SelectedItem).Value + "," : "";

				string pageColumn = string.Empty;
				if (cTool.DdlPageColumn.SelectedItem != null)
					pageColumn = (((ComboBoxItem)cTool.DdlPageColumn.SelectedItem).Value.Length > 0) ? ((ComboBoxItem)cTool.DdlPageColumn.SelectedItem).Value + " Column(s), " : "";

				string gutter = string.Empty;
				gutter = cTool.TxtPageGutterWidth.Text.Length > 0 ? "Column Gap - " + cTool.TxtPageGutterWidth.Text + ", " : "";

				string marginTop = string.Empty;
				marginTop = cTool.TxtPageTop.Text.Length > 0 ? "Margin Top - " + cTool.TxtPageTop.Text + ", " : "";

				string marginBottom = string.Empty;
				marginBottom = cTool.TxtPageBottom.Text.Length > 0 ? "Margin Bottom - " + cTool.TxtPageBottom.Text + ", " : "";

				string marginInside = string.Empty;
				marginInside = cTool.TxtPageInside.Text.Length > 0 ? "Margin Inside - " + cTool.TxtPageInside.Text + ", " : "";

				string marginOutside = string.Empty;
				marginOutside = cTool.TxtPageOutside.Text.Length > 0 ? "Margin Outside - " + cTool.TxtPageOutside.Text + ", " : "";

				string sense = string.Empty;
				if (cTool.DdlSense.SelectedItem != null)
					sense = (((ComboBoxItem)cTool.DdlSense.SelectedItem).Value.Length > 0) ? " Sense Layout - " + ((ComboBoxItem)cTool.DdlSense.SelectedItem).Value + "," : "";

				string RunningHead = string.Empty;
				if (cTool.DdlRunningHead.SelectedItem != null)
					RunningHead = (((ComboBoxItem)cTool.DdlRunningHead.SelectedItem).Value.Length > 0) ? " Running Head - " + ((ComboBoxItem)cTool.DdlRunningHead.SelectedItem).Value + "," : "";

				string combined = pageSize + " " +
								  pageColumn + "  " +
								  gutter + " " +
								  marginTop + " " +
								  marginBottom + " " +
								  marginInside + " " +
								  marginOutside + " " +
								  leading + " " +
								  fontSize + " " +
								  RunningHead + " " +
								  rules + " " +
								  justified + " " +
								  sense + " " +
                                  headerSize + " " +
								  picture + ".";
				cTool.TxtCss.Text = combined;
			}
			catch { }
		}

		protected void EnableToolStripButtons(bool enable)
		{
			string selectedTypeValue = cTool.StylesGrid[ColumnType, SelectedRowIndex].Value.ToString();
			if (selectedTypeValue != TypeStandard)
			{
				cTool.TsNew.Enabled = enable;
				cTool.TsDelete.Enabled = enable;
				cTool.TsSaveAs.Enabled = enable;
			}
		}

		/// <summary>
		/// Enable or Disable the Panel controls
		/// </summary>
		/// <param name="IsEnable">True or False</param>
		protected void EnableDisablePanel(bool IsEnable)
		{
			cTool.TsDelete.Enabled = IsEnable;
			cTool.TabDisplay.Enabled = IsEnable;
			cTool.TabMobile.Enabled = IsEnable;
			cTool.TabOthers.Enabled = IsEnable;
			cTool.TxtName.Enabled = IsEnable;
			cTool.TxtDesc.Enabled = IsEnable;
			cTool.TxtComment.Enabled = IsEnable;
			cTool.TxtApproved.Enabled = IsEnable;
			cTool.ChkAvailable.Enabled = true;
		}

		protected void setDefaultInputType()
		{
			if (!string.IsNullOrEmpty(inputTypeBL))
			{
				Param.SetValue(Param.InputType, inputTypeBL); // last input type
				Param.Write();
				Param.CopySchemaIfNecessary();
			}
		}

		protected void setLastSelectedLayout()
		{
			try
			{
				//Check and move
				string layoutName = cTool.TxtName.Text;
				if (layoutName.Length == 0)
					layoutName = _lastSelectedLayout;

				StyleEXE = layoutName;
				if (!string.IsNullOrEmpty(StyleEXE))
				{
					Param.SetValue(Param.LayoutSelected, StyleEXE); // last layout
					Param.Write();
				}
			}
			catch
			{
			}
		}

		public void SideBar()
		{
			try
			{
				_screenMode = ScreenMode.View;
				WriteMedia();
				setLastSelectedLayout();
				LoadParam();
				SetSideBar();
				ShowDataInGrid();
				SetPropertyTab();
				ShowInfoValue();
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
			}
		}

		/// <summary>
		/// Display the appropriate property tab (mobile, others or paper/web).
		/// </summary>
		protected void SetPropertyTab()
		{
			if (cTool.TabControl1.TabCount > 1)
				cTool.TabControl1.TabPages.Remove(cTool.TabControl1.TabPages[1]);
			if (cTool.TabControl1.TabPages.ContainsKey("tabPreview"))
				cTool.TabControl1.TabPages.Remove(cTool.TabControl1.TabPages["tabPreview"]);
			switch (MediaType)
			{
				case "mobile":
					if (inputTypeBL.ToLower() == "dictionary")
					{
						cTool.TabControl1.TabPages.Insert(1, tabDict4Mids);
						return;
					}
					cTool.TabControl1.TabPages.Insert(1, tabmob);
					XmlNodeList baseNode1 = Param.GetItems("//styles/" + MediaType + "/style[@name='" + StyleName + "']/styleProperty");
					SetMobileProperty(baseNode1);
					SetMobileSummary(null, null);
					break;
				case "others":
					cTool.TabControl1.TabPages.Insert(1, tabothers);
					cTool.TabControl1.TabPages.Insert(2, tabpreview);

					XmlNodeList baseNode = Param.GetItems("//styles/" + MediaType + "/style[@name='" + StyleName + "']/styleProperty");
					// show/hide chapter numbers and references UI
					SetEpubUIControls(inputTypeBL == "Scripture");

					SetOthersProperty(baseNode);
					SetOthersSummary(null, null);
					break;
				case "web":
					HashUtilities hashUtil = new HashUtilities();
					hashUtil.Key = "%:#@?,*&";
					hashUtil.Salt = "$%^&*#$%";
					cTool.TabControl1.TabPages.Insert(1, tabweb);

					XmlNodeList baseNode2 = Param.GetItems("//styles/" + MediaType + "/style[@name='" + StyleName + "']/styleProperty");
					// show/hide chapter numbers and references UI
					SetEpubUIControls(inputTypeBL == "Scripture");

					SetWebProperty(baseNode2, hashUtil);
					SetWebSummary(null, null);
					break;
				default:
					// web, paper
					cTool.TabControl1.TabPages.Add(tabDisplay);
					cTool.TabControl1.TabPages.Add(tabpreview);
					ShowCssSummary();
					break;
			}

			if (Common.GetOsName().IndexOf("Windows") >= 0) // Hide Preview if LibreOffice not exist
			{
				string regEntryWin7 = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\LibreOffice\\UNO\\InstallPath", "");
				string regEntryWinXP = Common.GetValueFromRegistry("SOFTWARE\\LibreOffice\\UNO\\InstallPath", "");
				if (regEntryWin7 == null && regEntryWinXP == null)
				{
					if (cTool.TabControl1.TabPages.ContainsKey("tabPreview"))
						cTool.TabControl1.TabPages.Remove(cTool.TabControl1.TabPages["tabPreview"]);
				}
			}
		}

		private void SetMobileProperty(XmlNodeList baseNode1)
		{
			foreach (XmlNode VARIABLE in baseNode1)
			{
				string attribName = VARIABLE.Attributes["name"].Value;
				string attribValue = VARIABLE.Attributes["value"].Value;
				if (attribName.ToLower() == "fileproduced")
				{
					cTool.DdlFiles.SelectedItem = (ComboBoxItem)cTool.DdlFiles.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
				}
				else if (attribName.ToLower() == "redletter")
				{
					cTool.DdlRedLetter.SelectedItem = (ComboBoxItem)cTool.DdlRedLetter.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
				}
				else if (attribName.ToLower() == "language")
				{
					cTool.DdlLanguage.SelectedItem = (ComboBoxItem)cTool.DdlLanguage.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
				}
			}
		}

		private void SetOthersProperty(XmlNodeList baseNode)
		{
			foreach (XmlNode VARIABLE in baseNode)
			{
				string attribName = VARIABLE.Attributes["name"].Value.ToLower();
				string attribValue = VARIABLE.Attributes["value"].Value;
				switch (attribName)
				{
					case "embedfonts":
						cTool.ChkEmbedFonts.Checked = (attribValue == "Yes") ? true : false;
						bool bEnabled = cTool.ChkEmbedFonts.Checked;
						cTool.ChkIncludeFontVariants.Enabled = bEnabled;
						cTool.DdlDefaultFont.Enabled = bEnabled;
						cTool.DdlMissingFont.Enabled = bEnabled;
						cTool.DdlNonSILFont.Enabled = bEnabled;
						break;
					case "includefontvariants":
						cTool.ChkIncludeFontVariants.Checked = (attribValue == "Yes") ? true : false;
						break;
					case "includeimage":
						cTool.ChkIncludeImage.Checked = (attribValue == "Yes") ? true : false;
						break;
					case "pagebreak":
						cTool.ChkPageBreaks.Checked = (attribValue == "Yes") ? true : false;
						break;
					case "maximagewidth":
						cTool.TxtMaxImageWidth.Text = attribValue;
						break;
					case "basefontsize":
						cTool.TxtBaseFontSize.Text = attribValue;
						break;
					case "defaultlineheight":
						cTool.TxtDefaultLineHeight.Text = attribValue;
						break;
					case "defaultalignment":
						cTool.DdlDefaultAlignment.SelectedItem = (ComboBoxItem)cTool.DdlDefaultAlignment.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
						break;
					case "chapternumbers":
						cTool.DdlChapterNumbers.SelectedItem = (ComboBoxItem)cTool.DdlChapterNumbers.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
						break;
					case "references":
						cTool.DdlReferences.SelectedItem = (ComboBoxItem)cTool.DdlReferences.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
						break;
					case "defaultfont":
						cTool.DdlDefaultFont.SelectedItem = (ComboBoxItem)cTool.DdlDefaultFont.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
						break;
					case "missingfont":
						cTool.DdlMissingFont.SelectedItem = (ComboBoxItem)cTool.DdlMissingFont.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
						break;
					case "nonsilfont":
						cTool.DdlNonSILFont.SelectedItem = (ComboBoxItem)cTool.DdlNonSILFont.Items.OfType<ComboBoxItem>().SingleOrDefault(s => s.Value == attribValue);
						break;
					default:
						break;
				}
			}
		}

		private void SetWebProperty(XmlNodeList baseNode2, HashUtilities hashUtil)
		{
			foreach (XmlNode VARIABLE in baseNode2)
			{
				string attribName = VARIABLE.Attributes["name"].Value.ToLower();
				string attribValue = VARIABLE.Attributes["value"].Value;
				switch (attribName)
				{
					case "ftpaddress":
						cTool.TxtFtpAddress.Text = attribValue;
						break;
					case "ftpuserid":
						cTool.TxtFtpUsername.Text = attribValue;
						break;
					case "ftppwd":
						if (attribValue.Trim().Length > 0)
						{
							cTool.TxtFtpPassword.Text = hashUtil.Decrypt(attribValue);
						}
						break;
					case "dbservername":
						cTool.TxtSqlServerName.Text = attribValue;
						break;
					case "dbname":
						cTool.TxtSqlDBName.Text = attribValue;
						break;
					case "dbuserid":
						cTool.TxtSqlUsername.Text = attribValue;
						break;
					case "dbpwd":
						if (attribValue.Trim().Length > 0)
						{
							cTool.TxtSqlPassword.Text = hashUtil.Decrypt(attribValue);
						}
						break;
					case "weburl":
						cTool.TxtWebUrl.Text = attribValue;
						break;
					case "webadminusrnme":
						cTool.TxtWebAdminUsrNme.Text = attribValue;
						break;
					case "webadminpwd":
						if (attribValue.Trim().Length > 0)
						{
							cTool.TxtWebAdminPwd.Text = hashUtil.Decrypt(attribValue);
						}
						break;
					case "webadminsitenme":
						cTool.TxtWebAdminSiteNme.Text = attribValue;
						break;
					case "webemailid":
						cTool.TxtWebEmailId.Text = attribValue;
						break;
					case "webftpfldrnme":
						cTool.TxtWebFtpFldrNme.Text = attribValue;
						break;
					case "comment":
						cTool.TxtComment.Text = attribValue;
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Helper method to show or hide elements on the epub properties tab based on
		/// whether or not the user is editing a scripture stylesheet.
		/// </summary>
		/// <param name="showScriptureControls"></param>
		private void SetEpubUIControls(bool showScriptureControls)
		{

			// show/hide chapter numbers and references UI
			cTool.LblChapterNumbers.Visible = showScriptureControls;
			cTool.DdlChapterNumbers.Visible = showScriptureControls;
			cTool.LblReferences.Visible = showScriptureControls;
			cTool.DdlReferences.Visible = showScriptureControls;
			// set position for embedded font controls (they need to move up if the other items are hidden)

			if (inputTypeBL.ToLower() == "scripture")
			{
				cTool.ChkPageBreaks.Visible = false;
				cTool.LblEpubFontsSection.Top = (showScriptureControls) ? cTool.DdlReferences.Bottom + 10 : cTool.DdlTocLevel.Bottom + 10;
			}
			else
			{
				cTool.ChkPageBreaks.Visible = true;
				cTool.LblEpubFontsSection.Top = (showScriptureControls) ? cTool.DdlReferences.Bottom + 30 : cTool.DdlTocLevel.Bottom + 30;
			}


			cTool.PicFonts.Top = cTool.LblEpubFontsSection.Bottom + 3;
			cTool.ChkEmbedFonts.Top = cTool.PicFonts.Top;
			cTool.ChkIncludeFontVariants.Top = cTool.ChkEmbedFonts.Bottom + 6;
			cTool.DdlMissingFont.Top = cTool.ChkIncludeFontVariants.Bottom + 6;
			cTool.LblMissingFont.Top = cTool.DdlMissingFont.Top + 3;
			cTool.DdlNonSILFont.Top = cTool.DdlMissingFont.Bottom + 6;
			cTool.LblNonSILFont.Top = cTool.DdlNonSILFont.Top + 3;
			cTool.DdlDefaultFont.Top = cTool.DdlNonSILFont.Bottom + 6;
			cTool.LblDefaultFont.Top = cTool.DdlDefaultFont.Top + 3;
		}

		/// <summary>
		/// Helper method to show or hide elements on the epub properties tab based on
		/// whether or not the user is editing a scripture stylesheet.
		/// </summary>
		/// <param name="showScriptureControls"></param>
		private void SetWebUIControls(bool showScriptureControls)
		{
			// show/hide chapter numbers and references UI
			cTool.TxtFtpAddress.Visible = showScriptureControls;
			cTool.TxtFtpUsername.Visible = showScriptureControls;
			cTool.TxtFtpPassword.Visible = showScriptureControls;
			cTool.TxtSqlServerName.Visible = showScriptureControls;
			cTool.TxtSqlDBName.Visible = showScriptureControls;
			cTool.TxtSqlUsername.Visible = showScriptureControls;
			cTool.TxtSqlPassword.Visible = showScriptureControls;
		}

		//Note - Check This
		protected void IsLayoutSelectedStyle()
		{
			if (_selectedStyle == PreviousValue) // LayoutSelected Value
			{
				Param.SetValue(Param.LayoutSelected, StyleName);
			}
		}

		#region ValidateStartsWithAlphabet(string stringValue)
		/// <summary>
		/// Validate a given string is Starts with alphabets or not 
		/// </summary>
		/// <param name="stringValue">string</param>
		/// <returns>True/False</returns>
		protected bool ValidateStyleName(string stringValue)
		{
			var validateStringMessage = string.Empty;
			bool valid = true;
			if (!Common.ValidateStartsWithAlphabet(stringValue))
			{
				validateStringMessage = LocalizationManager.GetString("ConfigurationToolBL.ValidateStyleName.Message1", "Style name should not be empty. Please enter the valid name", "");
			}
			else
			{
				foreach (char ch in stringValue)
				{
					if (!(ch == '-' || ch == '_' || ch == ' ' || ch == '.' || ch == '(' || ch == ')' ||
						(ch >= 48 && ch <= 57) || (ch >= 65 && ch <= 90) || (ch >= 97 && ch <= 122)))
					{
						validateStringMessage = LocalizationManager.GetString("ConfigurationToolBL.ValidateStyleName.Message2", "Please avoid {0} in the Style name", "");
						validateStringMessage = string.Format(validateStringMessage, ch);
						break;
					}
				}
			}
			if (validateStringMessage != string.Empty)
			{
				Utils.MsgBox(validateStringMessage, _caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
				valid = false;
			}
			return valid;
		}
		#endregion
		protected bool NoDuplicateStyleName()
		{
			bool result = true;
			if (cTool.TxtName.Text.ToLower() != PreviousStyleName.ToLower()) //Is new styleName?
			{
				// Add - Check whether the same name already exist
				// Edit- Check it, while the name changed in Stylename textBox.
				if (IsNameExists(cTool.StylesGrid, cTool.TxtName.Text))
				{
					var confirmationStringMessage = LocalizationManager.GetString("ConfigurationToolBL.NoDuplicateStyleName.Message", "Stylesheet Name [{0}] already exists", "");
					confirmationStringMessage = string.Format(confirmationStringMessage, cTool.TxtName.Text);
					Utils.MsgBox(confirmationStringMessage, _caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
					result = false;
				}
				cTool._previousTxtName = cTool.TxtName.Text;
			}
			return result;
		}

		protected static int GetPageSize(string paperSize, string dimension)
		{
			int pageWidth = 612;
			int pageHeight = 792;
			switch (paperSize)
			{
				case "A4":
					pageWidth = 595;
					pageHeight = 842;
					break;
				case "A5":
					pageWidth = 420;
					pageHeight = 595;
					break;
				case "C5":
					pageWidth = 459;
					pageHeight = 649;
					break;
				case "A6":
					pageWidth = 298;
					pageHeight = 420;
					break;
				case "Half letter":
					pageWidth = 396;
					pageHeight = 612;
					break;
				case "5.25in x 8.25in":
					pageWidth = 378;
					pageHeight = 594;
					break;
				case "5.8in x 8.7in":
					pageWidth = 418;
					pageHeight = 626;
					break;
				case "6in x 9in":
					pageWidth = 432;
					pageHeight = 648;
					break;
                case "6.6in x 9in":
                    pageWidth = 468;
                    pageHeight = 648;
                    break;
            }

			if (dimension == "Height")
			{
				return pageHeight;
			}
			return pageWidth;
		}

		protected static int GetDefaultValue(string value)
		{
			if (value.Length > 0 && Common.ValidateNumber(value.Replace("pt", "")))
			{
				return int.Parse(value.Replace("pt", ""));
			}
			return 0;

		}

		/// <summary>
		/// When the configurationtool is run from EXE, the mediatype has changed 
		/// by Prinvia dialog selection
		/// </summary>
		protected void SetMediaType()
		{
			Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: SetMediaType");
			try
			{
				if (MediaTypeEXE.Length != 0)
				{
					MediaType = MediaTypeEXE.ToLower();
					SideBar();
					SelectRow(cTool.StylesGrid, _lastSelectedLayout);
				}
				else
				{
					SetSideBar();
					SetPropertyTab();
				}
			}
			catch { }
		}

		/// <summary>
		/// removes selected row in grid
		/// </summary>
		public void RemoveXMLNode(string styleName)
		{
			string fileName;
			if (File.Exists(Param.SettingOutputPath))
				fileName = Param.SettingOutputPath;
			else
				fileName = Param.SettingPath;

			string xPath = "//styles/" + MediaType + "/style[@name='" + styleName + "']";
			XmlNode removableNode = Param.GetItem(xPath);
			if (removableNode == null) return;
			string cssFile = removableNode.Attributes["file"].Value;
			Param.RemoveXMLNode(fileName, xPath);
			try
			{
				// .Css file deletion
				string path = Param.Value["UserSheetPath"];
				string file = Common.PathCombine(path, cssFile);
				if (File.Exists(file))
					File.Delete(file);
			}
			catch
			{
			}
		}

		/// <summary>
		/// Load InputType from StyleSettings.xml. Ex: Scripture or Dictionary
		/// </summary>
		public string LoadInputType()
		{
			string inputType = "Dictionary";
			string settingPath = Common.LeftString(Param.Value["OutputPath"], "Pathway");
			string xmlPath = Common.PathCombine(settingPath, Common.PathCombine("Pathway", "StyleSettings.xml"));
			if (!File.Exists(xmlPath))
			{
				settingPath = Path.GetDirectoryName(Param.SettingPath);
				xmlPath = Common.PathCombine(settingPath, "StyleSettings.xml");
			}
			if (!File.Exists(xmlPath)) return inputType;

			XmlDocument xmlDoc = Common.DeclareXMLDocument(false);
			xmlDoc.Load(xmlPath);

			string xPath = "//settings/property[@name='InputType']";

			var node = xmlDoc.SelectSingleNode(xPath);
			if (node != null)
			{
				inputType = node.Attributes["value"].Value;
			}
			return inputType;
		}

		protected void ParseCSS(string cssPath, string loadType)
		{
			_cssPath = cssPath;
			_loadType = loadType;
			cssTree = new CssTree();
			Common.SamplePath = Param.Value["SamplePath"].Replace("Samples", "Styles");
			if (string.IsNullOrEmpty(_cssPath)) return;
			_cssClass = cssTree.CreateCssProperty(_cssPath, true);
		}

		public string GetValue(string task, string key, string defaultValue)
		{
			if (_cssClass.ContainsKey(task) && _cssClass[task].ContainsKey(key))
			{
				string result = _cssClass[task][key].Replace("'", "");

				if (result.Length == 0)
					return defaultValue;
				else
					return result;
			}
			return defaultValue;
		}

		public string GetPageValue(string task, string key, string defaultValue)
		{
			string[] srchRegion = task.Split('$');
			for (int i = 0; i < srchRegion.Length; i++)
			{
				if (_cssClass.ContainsKey(srchRegion[i]) && _cssClass[srchRegion[i]].ContainsKey(key))
				{
					return _cssClass[srchRegion[i]][key];
				}
			}
			return string.Empty;
		}

		public string PageSize1(string width, string height)
		{
			string dims = width + "x" + height;
			string pageSize = standardSize.ContainsKey(dims) ? standardSize[dims] : "Custom";
			return pageSize;
		}

		public string GetNewStyleName(ArrayList cssNames, string mode)
		{
			string preferedName = "CustomSheet-1";
			if (mode == "new")
			{
				if (cssNames.Count > 0)
				{
					preferedName = GetNewStyleCount(cssNames, preferedName);
				}
			}
			else
			{
				preferedName = StyleName;
				if (StyleName.IndexOf('(') == -1 || StyleName.IndexOf("(epub)(") == -1)
				{
					preferedName = "Copy of " + StyleName;
				}
				if (cssNames.Count > 0)
				{
					if (cssNames.Contains(preferedName))
					{
						preferedName = GetDirCount(cssNames, preferedName);
					}
				}
			}
			return preferedName;
		}

		protected static string GetNewStyleCount(ArrayList cssNames, string preferedName)
		{
			int temp = 1;
			int max = 0;
			foreach (string styleName in cssNames)
			{
				if (styleName.IndexOf("Copy") >= 0)
					continue;

				string[] ss = styleName.Split('-');
				try
				{
					temp = int.Parse(ss[1]);
				}
				catch
				{
				}
				if (max < temp) { max = temp; }
				preferedName = ss[0] + "-" + (max + 1);
			}
			if (cssNames.Contains(preferedName))
			{
				preferedName = GetNewStyleCount(cssNames, preferedName);
			}
			return preferedName;
		}

		protected static string GetDirCount(ArrayList cssNames, string preferedName)
		{
			int oldValue = 1;
			if (preferedName.IndexOf('(') == -1)
			{
				preferedName = preferedName.Replace("Copy of", "Copy(2) of");
				if (!cssNames.Contains(preferedName))
				{
					return preferedName;
				}
			}
			int startPos = preferedName.IndexOf('(');
			int endPos = preferedName.IndexOf(')');
			if (startPos > 0)
			{
				if (!int.TryParse(preferedName.Substring(startPos + 1, ((endPos - 1) - (startPos))), out oldValue))
				{
					// We wanted a stylesheet name with a parenthesis in it, but just the first copy -
					// change it to Copy (2) of...
					preferedName = preferedName.Replace("Copy of", "Copy(2) of");
					if (!cssNames.Contains(preferedName))
					{
						return preferedName;
					}
					oldValue = 1;
				}
			}
			int newValue = oldValue + 1;
			preferedName = preferedName.Replace(oldValue.ToString(), newValue.ToString());
			if (cssNames.Contains(preferedName))
			{
				preferedName = GetDirCount(cssNames, preferedName);
			}
			return preferedName;
		}

		/// <summary>
		/// When data is typed in the info tab, the entry on the grid is updated
		/// </summary>
		public void UpdateGrid(Control myControl, DataGridView grid)
		{
			if (SelectedRowIndex >= 0)
			{
				switch (myControl.Name)
				{
					case "txtDesc":
						grid[ColumnDescription, SelectedRowIndex].Value = myControl.Text;
						break;
					case "txtComment":
						grid[ColumnComment, SelectedRowIndex].Value = myControl.Text;
						break;
					case "txtName":
						if (_screenMode == ScreenMode.Modify)
							grid[ColumnName, SelectedRowIndex].Value = myControl.Text;
						break;
					default:
						CheckBox checkBox = (CheckBox)myControl;
						grid[ColumnShown, SelectedRowIndex].Value = checkBox.Checked ? "Yes" : "No";
						break;
				}
				_screenMode = ScreenMode.Modify;
			}
		}

		public bool CopyStyle(DataGridView grid, ArrayList cssNames)
		{
			var currentRow = grid.Rows[SelectedRowIndex];
			if (currentRow == null) return false;
			//var currentDescription = currentRow.Cells[ColumnDescription].Value.ToString();
			// var currentApprovedBy = grid[AttribApproved, SelectedRowIndex].Value.ToString();
			var currentApprovedBy = grid[5, SelectedRowIndex].Value.ToString();
			string type = grid[ColumnType, SelectedRowIndex].Value.ToString();
			PreviousStyleName = GetNewStyleName(cssNames, "copy");
			if (PreviousStyleName.Length > 50)
			{
				var confirmationStringMessage = LocalizationManager.GetString("ConfigurationToolBL.CopyStyle.Message", "Names of styles should not be greater than 50 characters.", "");
				Utils.MsgBox(confirmationStringMessage, Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}
			var currentDescription = "Based on " + currentApprovedBy + " stylesheet " + StyleName;
			Param.SaveSheet(PreviousStyleName, Param.StylePath(FileName), currentDescription, type);
			XmlNode baseNode = Param.GetItem("//styles/" + MediaType + "/style[@name='" + PreviousStyleName + "']");
			Param.SetAttrValue(baseNode, AttribType, TypeCustom);
			//To add StyleProperty for Mobile media
			if (inputTypeBL.ToLower() == "scripture" && MediaType.ToLower() == "mobile")
			{
				XmlNodeList mobileBaseNode = Param.GetItems("//styles/" + MediaType + "/style[@name='" + grid[0, SelectedRowIndex].Value.ToString() + "']/styleProperty");
				foreach (XmlNode stylePropertyNode in mobileBaseNode)
				{
					baseNode.AppendChild(stylePropertyNode.Clone());
				}
			}
			// others (epub)
			if (MediaType.ToLower() == "others")
			{
				XmlNodeList otherBaseNode = Param.GetItems("//styles/" + MediaType + "/style[@name='" + grid[0, SelectedRowIndex].Value.ToString() + "']/styleProperty");
				foreach (XmlNode stylePropertyNode in otherBaseNode)
				{
					baseNode.AppendChild(stylePropertyNode.Clone());
				}
			}
			//web
			if (inputTypeBL.ToLower() == "scripture" && MediaType.ToLower() == "web")
			{
				XmlNodeList webBaseNode = Param.GetItems("//styles/" + MediaType + "/style[@name='" + grid[0, SelectedRowIndex].Value.ToString() + "']/styleProperty");
				foreach (XmlNode stylePropertyNode in webBaseNode)
				{
					baseNode.AppendChild(stylePropertyNode.Clone());
				}
			}
			Param.Write();
			return true;
		}

		protected void AddStyleInXML(DataGridView grid, ArrayList cssNames)
		{
			var currentRow = grid.Rows[SelectedRowIndex];
			if (currentRow == null) return;
			var currentDescription = currentRow.Cells[ColumnDescription].Value.ToString();
			string type = grid[ColumnType, SelectedRowIndex].Value.ToString();
			NewStyleName = GetNewStyleName(cssNames, "new");
			Param.SaveSheet(NewStyleName, Param.StylePath(StyleName), currentDescription, type);
			XmlNode baseNode = Param.GetItem("//styles/" + MediaType + "/style[@name='" + NewStyleName + "']");
			Param.SetAttrValue(baseNode, AttribType, TypeCustom);
			Param.Write();
		}

		/// <summary>
		///
		/// </summary>
		protected void LoadParam()
		{
			Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationToolBL: LoadParam");
			Param.SetValue(Param.InputType, inputTypeBL); // Dictionary or Scripture
			Param.SetLoadType = inputTypeBL;
			Param.LoadSettings();
			MediaType = Param.MediaType;
		}

		public void WriteAtImport(StreamWriter writeCss, string attribute, string key)
		{
			if (key.Length == 0) return;
			if (Param.featureList.ContainsKey(attribute))
			{
				var values = Param.featureList[attribute];
				string fileName = values[key];
				if (attribute.ToLower() == "page number")
				{
					string pageType = GetDdlRunningHead();
					fileName = GetPageNumberImport(pageType, key);
				}
				if (fileName.IndexOf("_Paged_") > 0 && ((ComboBoxItem)cTool.DdlRunningHead.SelectedItem).Value.ToLower() == "mirrored")
				{
					fileName = fileName.Replace("_Paged_", "_Mirrored_");
				}
				writeCss.WriteLine("@import \"" + fileName + "\";");
			}
		}

		public string GetPageNumberImport(string pageType, string pos)
		{
			string xPath = string.Empty;
			Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: PopulatePageNumberFeature");

			xPath = "//features/feature[@name='Page Number']/option[@type='" + pageType + "' or @type= 'Both']";

			XmlNodeList pageNumList = Param.GetItems(xPath);
			try
			{
				foreach (XmlNode node in pageNumList)
				{
					if (node.Attributes != null)
						if (node.Attributes["name"].Value == pos)
						{
							return node.Attributes["file"].Value;
						}
				}
			}
			catch { }
			return "PageNumber_None.css";
		}


		public void PopulateFeatureLists(TreeView TvFeatures)
		{
			string defaultSheet = Param.DefaultValue.ContainsKey(Param.LayoutSelected) ? Param.DefaultValue[Param.LayoutSelected] : string.Empty;
			if (defaultSheet.Length == 0) return;
			var featureSheet = new FeatureSheet(Param.StylePath(defaultSheet));
			featureSheet.ReadToEnd();
			Param.LoadFeatures("features/feature", TvFeatures, TvFeatures.Enabled ? featureSheet.Features : null);
		}

		public void WriteCssClass(StreamWriter writeCss, string className, Dictionary<string, string> value)
		{
			string precedeChar = className.ToLower().IndexOf("page") == 0 ? "@" : ".";
			if (value.Count > 0)
			{
                if (className.IndexOf("-") > 0)
                {
                    var clsName = className.Split('-');
                    writeCss.WriteLine(precedeChar + clsName[0] + "{");
                    writeCss.WriteLine(precedeChar + clsName[1] + "-" + clsName[2] + "{");
                    foreach (var pair in value)
                    {
                        if (pair.Value.Length > 0)
                            writeCss.WriteLine(pair.Key + ":" + pair.Value + ";");
                    }
                    writeCss.WriteLine("}");
                    writeCss.WriteLine("}");
                }
                else
                {

                    writeCss.WriteLine(precedeChar + className + "{");
                    foreach (var pair in value)
                    {
                        if (pair.Value.Length > 0)
                            writeCss.WriteLine(pair.Key + ":" + pair.Value + ";");
                    }
                    writeCss.WriteLine("}");
                }
			}
		}

		public string CreateCssFile(string fileName)
		{
			string success = string.Empty;
			try
			{
				string path = Param.Value["UserSheetPath"]; // all user path
				string file = Common.PathCombine(path, fileName);
				string importStatement = "@import \"Default.css\";";
				StreamWriter writeCss = new StreamWriter(file);
				writeCss.WriteLine(importStatement);
				writeCss.Flush();
				writeCss.Close();
			}
			catch (Exception ex)
			{
				success = ex.Message;
			}
			return success;
		}

		public void WriteMedia()
		{
			XmlNode baseNode = Param.GetItem("//categories/category[@name = \"Media\"]");
			if (MediaType.Length <= 0)
				MediaType = Param.GetAttrByName("//categories/category", "Media", "select").ToLower();
			Param.SetAttrValue(baseNode, "select", MediaType);
			Param.Write();
		}

		public void AddNew(string styleName)
		{
			XmlNode baseNode = Param.GetItem("//styles/" + MediaType + "/style");
			if (baseNode == null) return;

			XmlNode copyNode = baseNode.Clone();

			Param.SetAttrValue(copyNode, AttribName, styleName); // style Name
			Param.SetAttrValue(copyNode, AttribFile, FileName); // css file name

			Param.SetAttrValue(copyNode, AttribType, TypeCustom); // custom
			Param.SetNodeText(copyNode, ElementDesc, ""); // description
			Param.SetNodeText(copyNode, ElementComment, ""); // comment
			baseNode.ParentNode.AppendChild(copyNode);

			Param.Write();
		}

		public bool SelectRow(DataGridView grid, string sheet)
		{
			_propertyValue.Clear();
			bool result = false;
			for (int i = 0; i < grid.Rows.Count; i++)
			{
				if (grid.Rows[i].Cells[ColumnName].Value.ToString() == sheet)
				{
					grid.ClearSelection();
					grid.Rows[i].Selected = true;
					SelectedRowIndex = i;
					result = true;
					break;
				}
			}
			return result;
		}

		public string SetPreviousLayoutSelect(DataGridView grid)
		{
			Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationToolBL: SetPreviousLayoutSelect");
			string lastLayout = string.Empty;
			bool selectedNotExist = true;

			if (Param.Value.ContainsKey(Param.LayoutSelected))
			{
				lastLayout = StyleEXE.Length == 0 ? Param.Value[Param.LayoutSelected] : StyleEXE;

				if (lastLayout.Trim().Length == 0)
				{
					lastLayout = Param.Value[Param.LayoutSelected];
				}

				if (SelectRow(grid, lastLayout))
				{
					selectedNotExist = false;
				}
			}
			if (selectedNotExist && Param.DefaultValue.ContainsKey(Param.LayoutSelected))
			{
				lastLayout = StyleEXE.Length > 0 ? StyleEXE : Param.DefaultValue[Param.LayoutSelected];

				SelectRow(grid, lastLayout);
			}
			return lastLayout;
		}

		public void GridColumnWidth(DataGridView grid)
		{
			XmlNodeList columns = Param.GetItems("//column-width/column");
			int i = 0;
			foreach (XmlNode xmlNode in columns)
			{
				string value = xmlNode.Attributes["width"].Value;
				int width = int.Parse(value);
				if (grid.ColumnCount > 0)
				{
					grid.Columns[i].Width = width;
				}
				i++;
			}
		}

		public bool WriteAttrib(string key, object sender)
		{
			bool result = false;

			string file;
			string attribValue = Common.GetTextValue(sender, out file);
			if (attribValue.Trim().Length == 0) return false;
			if (PreviousValue == attribValue)
			{
				return result;
			}

			string searchStyleName = StyleName;
			XmlNode baseNode = Param.GetItem("//styles/" + MediaType + "/style[@name='" + searchStyleName + "']");

			if (baseNode == null) return result;

			if (key == AttribName)
			{
				Param.SetAttrValue(baseNode, key, attribValue);
				Param.SetAttrValue(baseNode, AttribFile, FileName);
			}
			else if (key == ElementDesc || key == ElementComment)
			{
				Param.SetNodeText(baseNode, key, attribValue);
			}
			else
			{
				Param.SetAttrValue(baseNode, key, attribValue);
			}

			Param.Write();
			return true;
		}

		/// <summary>
		/// checks whether stylesheet name already exists
		/// </summary>
		/// <returns>return true or false</returns>
		public bool IsNameExists(DataGridView grid, string styleName)
		{
			bool result = false;
			if (PreviousValue.ToLower() == styleName.ToLower()) return result;
			styleName = styleName.Trim().ToLower();
			for (int row = 0; row < grid.Rows.Count - 1; row++)
			{
				if (grid.Rows[row].Selected)
				{
					continue; // do not compare with current selection.
				}

				if (grid[ColumnName, row].Value.ToString().ToLower() == styleName)
				{
					result = true;
				}
			}

			if (cTool != null)
				if (cTool.TabControl1.SelectedIndex != 0)
					return result;

			XmlNodeList xmlNodeList = Param.GetItems("//styles//style");
			if (xmlNodeList != null)
			{
				foreach (XmlNode xmlNode in xmlNodeList)
				{
					XmlNode xn = xmlNode.Attributes.GetNamedItem("name");

					if (xn != null && xn.Value.ToLower() == styleName.ToLower())
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public void SaveColumnWidth(string columnIndex, string columnWidth)
		{
			XmlNode baseNode = Param.GetItem("//column-width/column[@name = \"" + columnIndex + "\"]");
			Param.SetAttrValue(baseNode, "width", columnWidth);
			Param.Write();
		}

		/// <summary>
		/// ClearTab Property Tab controls
		/// </summary>
		public void ClearPropertyTab(TabPage tabPage)
		{
			if (tabPage == null) return;

			foreach (Control ctl in tabPage.Controls)
			{
				if (ctl is TextBox)
				{
					var textBox = (TextBox)ctl;
					textBox.Text = "";
				}
				else if (ctl is ComboBox)
				{
					var comboBox = (ComboBox)ctl;
					comboBox.Items.Clear();
					comboBox.SelectedIndex = -1;
				}
			}
		}

		/// <summary>
		/// Loads styles from Settings XML files
		/// </summary>
		public void ShowStyleInGrid(DataGridView grid, ArrayList cssNames)
		{
			if (DataSetForGrid.Tables.Count > 0 && DataSetForGrid.Tables.Contains("Styles"))
			{
				DataSetForGrid.Tables["Styles"].Clear();

				DataRow row;
				XmlNodeList cats = Param.GetItems("//styles/" + MediaType + "/style");
				foreach (XmlNode xml in cats)
				{
					XmlAttribute name = xml.Attributes[AttribName];
					XmlAttribute file = xml.Attributes[AttribFile];
					XmlAttribute type = xml.Attributes[AttribType];
					XmlAttribute shown = xml.Attributes[AttribShown];
					XmlAttribute approvedBy = xml.Attributes[AttribApproved];
					XmlAttribute previewFile1 = xml.Attributes[AttribPreviewFile1];
					XmlAttribute previewFile2 = xml.Attributes[AttribPreviewFile2];

					XmlNode xml1 = xml.SelectSingleNode(ElementDesc);
					string desc = string.Empty;
					if (xml1 != null)
						desc = xml1.InnerText;

					xml1 = xml.SelectSingleNode(ElementComment);
					string comment = string.Empty;
					if (xml1 != null)
						comment = xml1.InnerText;


					row = DataSetForGrid.Tables["Styles"].NewRow();
					row["Name"] = name != null ? name.Value : string.Empty; //name.Value;

					if (row["Name"].ToString().IndexOf("Copy") >= 0 || row["Name"].ToString().IndexOf("Custom") >= 0)
					{
						if (!cssNames.Contains(row["Name"]))
							cssNames.Add(row["Name"]);
					}
					row["File"] = file.Value;
					row["Description"] = desc;
					row["Comment"] = comment;
					row["Type"] = type != null ? type.Value : TypeStandard;
					row["Shown"] = shown != null ? shown.Value : "Yes"; // shown.Value;
					row["ApprovedBy"] = approvedBy != null ? approvedBy.Value : string.Empty; //approvedBy.Value;
					row["previewFile1"] = previewFile1 != null && previewFile1.Value != null ? previewFile1.Value : string.Empty;
					row["previewFile2"] = previewFile2 != null && previewFile2.Value != null ? previewFile2.Value : string.Empty;
					DataSetForGrid.Tables["Styles"].Rows.Add(row);
				}
				grid.DataSource = DataSetForGrid.Tables["Styles"];
				grid.Refresh();

				for (int i = 0; i < grid.Columns.Count; i++)
				{
					grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
				}

				ColumnHeaderLocalization(grid);
				if (grid.Columns.Count > 0)
				{
					grid.Columns[5].Visible = false; // Hiding the ApprovedBy column
					grid.Columns[6].Visible = false; // Hiding the File Name
					grid.Columns[7].Visible = false; // Preview File 1
					grid.Columns[8].Visible = false; // Preview File 2

				}

				if (grid.SelectedRows.Count <= 0 && IsUnixOs)
				{
					Param.LoadSettings();
					SetPreviousLayoutSelect(grid);
					if (grid.SelectedRows.Count == 0 || grid.SelectedRows.Count == -1)
					{
						grid.ClearSelection();
						grid.Rows[0].Selected = true;
						SelectedRowIndex = 0;
						//_screenMode = ScreenMode.View;
						ShowInfoValue();
					}
				}
			}
		}

		private static void ColumnHeaderLocalization(DataGridView grid)
		{
			if (!Common.Testing)
			{
				if (grid.Columns.Count > 0)
				{
					grid.Columns[0].HeaderText = LocalizationManager.GetString("ConfigurationTool.Column.Name",
						grid.Columns[0].HeaderText);
					grid.Columns[1].HeaderText = LocalizationManager.GetString("ConfigurationTool.Column.Description",
						grid.Columns[1].HeaderText);
					grid.Columns[2].HeaderText = LocalizationManager.GetString("ConfigurationTool.Column.Comment",
						grid.Columns[2].HeaderText);
					grid.Columns[3].HeaderText = LocalizationManager.GetString("ConfigurationTool.Column.Type",
						grid.Columns[3].HeaderText);
					grid.Columns[4].HeaderText = LocalizationManager.GetString("ConfigurationTool.Column.Shown",
						grid.Columns[4].HeaderText);
				}
			}
		}

		public bool CopyCustomStyleToSend(string folderPath)
		{
			bool directoryCreated = false;
			directoryCreated = BackUpCSSFile(directoryCreated, folderPath);
			BackUpUserSettingFiles(folderPath);
			return directoryCreated;
		}

		public string GenerateStylesString()
		{
			string path = "Styles";
			return path;
		}

		protected bool BackUpCSSFile(bool directoryCreated, string folderPath)
		{
			XmlNodeList cats = Param.GetItems("//styles/" + MediaType + "/style");
			foreach (XmlNode xml in cats)
			{
				XmlAttribute type = xml.Attributes[AttribType];
				XmlAttribute file = xml.Attributes[AttribFile];
				if (file != null)
				{
					string path = Common.PathCombine(Path.GetDirectoryName(Param.SettingPath), Common.PathCombine(GenerateStylesString(), Param.Value["InputType"]));
					if (type != null && type.Value == TypeCustom)
					{
						string OutputPath = Path.GetDirectoryName(Path.GetDirectoryName(Param.SettingOutputPath));
						path = Common.PathCombine(OutputPath, Param.Value["InputType"]);
					}

					string fromFile = Common.PathCombine(path, file.Value);
					if (File.Exists(fromFile))
					{
						if (!directoryCreated)
						{
							if (Directory.Exists(folderPath))
							{
								DirectoryInfo di = new DirectoryInfo(folderPath);
								Common.CleanDirectory(di);
							}
							Directory.CreateDirectory(folderPath);
							directoryCreated = true;
						}

						string toFile = Common.PathCombine(folderPath, file.Value);
						File.Copy(fromFile, toFile, true);
					}
				}
			}
			return directoryCreated;
		}

		//<summary>
		//Method to copy the settings file(DictionaryStyleSettings.xml/ScriptureSettings.xml) 
		//from the Alluser path.
		//</summary>
		protected static void BackUpUserSettingFiles(string toPath)
		{
			string projType = Param.Value["InputType"];
			string sourcePath = Common.PathCombine(Common.GetAllUserPath(), projType);
			if (!Directory.Exists(sourcePath)) return;
			string[] filePaths = Directory.GetFiles(sourcePath);
			foreach (string filePath in filePaths)
			{
				string fileName = Path.GetFileName(filePath);
				if (fileName.IndexOf(".xml") > 0 || fileName.IndexOf(".xsd") > 0)
				{
					if (File.Exists(filePath) && Directory.Exists(toPath))
					{
						File.Copy(filePath, Common.PathCombine(toPath, fileName));
					}
				}
			}
		}

		/// <summary>
		/// creating datasource for grid
		/// </summary>
		/// <returns>returns DataSet</returns>
		public void CreateGridColumn()
		{
			Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationToolBL: CreateGridColumn");
			string tableName = "Styles";
			DataTable table = new DataTable(tableName);
			DataColumn column = new DataColumn
									{
										DataType = Type.GetType("System.String"),
										ColumnName = "Name",
										Caption = "Name",
										ReadOnly = false,
										Unique = false,
										MaxLength = 50,
									};
			table.Columns.Add(column);
			// Create Description column.
			column = new DataColumn
						 {
							 DataType = Type.GetType("System.String"),
							 ColumnName = "Description",
							 Caption = "Description",
							 ReadOnly = false,
							 Unique = false,
							 MaxLength = 250
						 };
			table.Columns.Add(column);

			// Create Comment column.
			column = new DataColumn
						 {
							 DataType = Type.GetType("System.String"),
							 ColumnName = "Comment",
							 Caption = "Comment",
							 ReadOnly = false,
							 Unique = false,
							 MaxLength = 250
						 };
			table.Columns.Add(column);

			// Create Type column.
			column = new DataColumn
						 {
							 DataType = Type.GetType("System.String"),
							 ColumnName = "Type",
							 Caption = "Type",
							 ReadOnly = false,
							 Unique = false,
							 MaxLength = 10
						 };
			table.Columns.Add(column);

			// Create Shown column.
			column = new DataColumn
						 {
							 DataType = Type.GetType("System.String"),
							 ColumnName = "Shown",
							 Caption = "Shown",
							 MaxLength = 10,
							 ReadOnly = false,
							 Unique = false
						 };
			table.Columns.Add(column);

			// Create Approvedby column.
			column = new DataColumn
						 {
							 DataType = Type.GetType("System.String"),
							 ColumnName = "Approvedby",
							 Caption = "Approvedby",
							 ReadOnly = false,
							 Unique = false,
							 MaxLength = 10
						 };
			table.Columns.Add(column);

			// Create File column.
			column = new DataColumn
						 {
							 DataType = Type.GetType("System.String"),
							 ColumnName = "File",
							 Caption = "File",
							 ReadOnly = false,
							 Unique = false,
							 MaxLength = 100
						 };
			table.Columns.Add(column);

			// Create PreviewFile1 column.
			column = new DataColumn
						 {
							 DataType = Type.GetType("System.String"),
							 ColumnName = "PreviewFile1",
							 Caption = "PreviewFile1",
							 ReadOnly = false,
							 Unique = false,
							 MaxLength = 150
						 };
			table.Columns.Add(column);

			// Create PreviewFile2 column.
			column = new DataColumn
						 {
							 DataType = Type.GetType("System.String"),
							 ColumnName = "PreviewFile2",
							 Caption = "PreviewFile2",
							 ReadOnly = false,
							 Unique = false,
							 MaxLength = 150
						 };
			table.Columns.Add(column);

			// Instantiate the DataSet variable.
			//_dataSet = new DataSet();
			// Add the new DataTable to the DataSet.
			DataSetForGrid.Clear();
			DataSetForGrid.Tables.Clear();
			DataSetForGrid.Tables.Add(table);
		}

		/// <summary>
		/// This for Nunit Test - no need of 
		/// </summary>
		public void SetCssDictionartyToTest()
		{
			Dictionary<string, string> attrib = new Dictionary<string, string>();
			attrib["column-count"] = "2";
			_cssClass["letData"] = attrib;

			attrib = new Dictionary<string, string>();
			attrib["text-align"] = "justify";
			_cssClass["entry"] = attrib;

		}

		public void SetGotFocusValueBL(object sender)
		{
			try
			{
				string control;
				PreviousValue = Common.GetTextValue(sender, out control);
				_redoUndoBufferValue = PreviousValue;
				_previousStyleName = PreviousValue;

			}
			catch { }
		}

		public void ShowOthersSummaryBL()
		{
			var sb = new StringBuilder();
			sb.Append(cTool.TxtBaseFontSize.Text);
			sb.Append("pt Font, ");
			sb.Append(cTool.TxtDefaultLineHeight.Text);
			sb.Append("% Line Height");
			if (cTool.DdlJustified.SelectedItem != null)
			{
				if (cTool.DdlDefaultAlignment.SelectedItem != null)
				{
					sb.Append(", Alignment: ");
					sb.Append(((ComboBoxItem)cTool.DdlDefaultAlignment.SelectedItem).Value.ToString(CultureInfo.InvariantCulture));
				}
			}

			cTool.TxtCss.Text = sb.ToString();
		}

		public void ShowWebSummaryBL()
		{
			var sb = new StringBuilder();
			sb.Append(cTool.TxtName.Text);
			sb.Append(", ");
			cTool.TxtCss.Text = sb.ToString();
		}


		public void ShowMobileSummaryBL()
		{
			if (inputTypeBL.ToLower() == "dictionary")
			{
				cTool.TxtCss.Text = @"No custom properties for DictionaryForMIDs";
			}
			string comma = ", ";
			string red = string.Empty;
			if (cTool.DdlRedLetter.SelectedItem != null)
			{
				if(((ComboBoxItem)cTool.DdlRedLetter.SelectedItem).Value.Length > 0)
				{
					if(((ComboBoxItem)cTool.DdlRedLetter.SelectedItem).Value.ToLower() == "yes")
					{
						red = " Red Letter  ";
					}
				}
			}
			if (red.Length == 0)
				comma = "";
			string files = "";
			if (cTool.DdlFiles.SelectedItem != null)
			{
				if (((ComboBoxItem)cTool.DdlFiles.SelectedItem).Value.Length > 0)
				{
					files = " Numbers of files produced -  " + ((ComboBoxItem)cTool.DdlFiles.SelectedItem).Value + comma;
				}
			}
			string combined =
				files + " " +
				red;

			cTool.TxtCss.Text = combined;
		}

		public void ValidatePageHeightMarginsBL(object sender)
		{
			int marginTop = GetDefaultValue(cTool.TxtPageTop.Text);
			int marginBottom = GetDefaultValue(cTool.TxtPageBottom.Text);
			int expectedHeight = GetPageSize(((ComboBoxItem)cTool.DdlPagePageSize.SelectedItem).Value, "Height") / 4;
			int outputHeight = marginTop + marginBottom;

			Control ctrl = ((Control)sender);
			string errMessage = outputHeight > expectedHeight ? "The combination of the margin-Top and margin-bottom should not exceed a quarter of the page height." : "";

			_errProvider.SetError(ctrl, errMessage);
			if (errMessage.Length == 0)
			{
				_errProvider.SetError(cTool.TxtPageTop, errMessage);
				_errProvider.SetError(cTool.TxtPageBottom, errMessage);
			}
			ShowCssSummary();
		}

		public void ValidatePageWidthMarginsBL(object sender)
		{
			int marginLeft = GetDefaultValue(cTool.TxtPageInside.Text);
			int marginRight = GetDefaultValue(cTool.TxtPageOutside.Text);
			int columnGap = GetDefaultValue(cTool.TxtPageGutterWidth.Text);
			int expectedWidth = GetPageSize(((ComboBoxItem)cTool.DdlPagePageSize.SelectedItem).Value, "Width") / 2;
			int outputWidth = marginLeft + columnGap + marginRight;

			Control ctrl = ((Control)sender);
			string errMessage = outputWidth > expectedWidth ? "The combination of the column gap, left and right margins should not exceed half of the page width." : "";
			_errProvider.SetError(ctrl, errMessage);
			_errProvider.SetError(cTool.TxtPageInside, errMessage);
			_errProvider.SetError(cTool.TxtPageOutside, errMessage);
			_errProvider.SetError(cTool.TxtPageGutterWidth, errMessage);
			ShowCssSummary();
		}
		#endregion

		#region Event Method
		public void btnBrowse_ClickBL()
		{
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = "png (*.png) |*.png";
			openFile.ShowDialog();

			string filename = openFile.FileName;
			if (filename != "")
			{
				try
				{
					Image iconImage = Image.FromFile(filename);
					double height = iconImage.Height;
					double width = iconImage.Width;
					if (height != 20 || width != 20)
					{
						var confirmationStringMessage = LocalizationManager.GetString("ConfigurationToolBL.BrowseButton.Message", "Please choose the icon with 20 x 20 px.", "");
						Utils.MsgBox(confirmationStringMessage, _caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
						return;
					}
					string userPath = (Param.Value["UserSheetPath"]);
					string imgFileName = Path.GetFileName(filename);
					string toPath = Common.PathCombine(userPath, imgFileName);
					File.Copy(filename, toPath, true);
					Param.UpdateMobileAtrrib("Icon", toPath, StyleName);
					cTool.MobileIcon.Image = iconImage;
				}
				catch { }
			}
		}

		public void ddlRedLetter_SelectedIndexChangedBL(object sender, EventArgs e)
		{
			try
			{
				Param.UpdateMobileAtrrib("RedLetter", ((ComboBoxItem)cTool.DdlRedLetter.SelectedItem).Value, StyleName);
				SetMobileSummary(sender, e);
			}
			catch { }
		}

		public void ddlFiles_SelectedIndexChangedBL(object sender, EventArgs e)
		{
			try
			{
				_fileProduce = cTool.DdlFiles.Text;
				Param.UpdateMobileAtrrib("FileProduced", ((ComboBoxItem)cTool.DdlFiles.SelectedItem).Value, StyleName);
				SetMobileSummary(sender, e);
			}
			catch { }
		}

		public void ddlLanguage_SelectedIndexChangedBL(object sender, EventArgs e)
		{
			try
			{
				_fileProduce = cTool.DdlLanguage.Text;
				Param.UpdateMobileAtrrib("Language", ((ComboBoxItem)cTool.DdlLanguage.SelectedItem).Value, StyleName);
				SetMobileSummary(sender, e);
			}
			catch { }
		}

		public void ddlTocLevel_SelectedIndexChangedBL(object sender, EventArgs e)
		{
			try
			{
				_tocLevel = cTool.DdlTocLevel.Text;
				Param.UpdateOthersAtrrib("TOCLevel", ((ComboBoxItem)cTool.DdlTocLevel.SelectedItem).Value, StyleName);
				SetOthersSummary(sender, e);
			}
			catch { }
		}

		public void txtMaxImageWidth_ValidatedBL(object sender)
		{
			try
			{
				Param.UpdateOthersAtrrib("MaxImageWidth", cTool.TxtMaxImageWidth.Text, StyleName);
			}
			catch { }
		}

		public void txtBaseFontSize_ValidatedBL(object sender)
		{
			try
			{
				Param.UpdateOthersAtrrib("BaseFontSize", cTool.TxtBaseFontSize.Text, StyleName);
			}
			catch { }
		}

		public void txtDefaultLineHeight_ValidatedBL(object sender)
		{
			try
			{
				Param.UpdateOthersAtrrib("DefaultLineHeight", cTool.TxtDefaultLineHeight.Text, StyleName);
			}
			catch { }
		}

		public void ddlDefaultAlignment_SelectedIndexChangedBL(object sender, EventArgs e)
		{
			try
			{
				Param.UpdateOthersAtrrib("DefaultAlignment", ((ComboBoxItem)cTool.DdlDefaultAlignment.SelectedItem).Value, StyleName);
				SetOthersSummary(sender, e);
			}
			catch { }
		}

		public void ddlChapterNumbers_SelectedIndexChangedBL(object sender, EventArgs e)
		{
			try
			{
				Param.UpdateOthersAtrrib("ChapterNumbers", ((ComboBoxItem)cTool.DdlChapterNumbers.SelectedItem).Value, StyleName);
				SetOthersSummary(sender, e);
			}
			catch { }
		}

		public void ddlReferences_SelectedIndexChangedBL(object sender, EventArgs e)
		{
			try
			{
				Param.UpdateOthersAtrrib("References", ((ComboBoxItem)cTool.DdlReferences.SelectedItem).Value, StyleName);
				SetOthersSummary(sender, e);
			}
			catch { }
		}

		public void ddlDefaultFont_SelectedIndexChangedBL(object sender, EventArgs e)
		{
			try
			{
				Param.UpdateOthersAtrrib("DefaultFont", ((ComboBoxItem)cTool.DdlDefaultFont.SelectedItem).Value, StyleName);
				SetOthersSummary(sender, e);
			}
			catch { }
		}

		public void ddlMissingFont_SelectedIndexChangedBL(object sender, EventArgs e)
		{
			try
			{
				Param.UpdateOthersAtrrib("MissingFont", ((ComboBoxItem)cTool.DdlMissingFont.SelectedItem).Value, StyleName);
				SetOthersSummary(sender, e);
			}
			catch { }
		}

		public void ddlNonSILFont_SelectedIndexChangedBL(object sender, EventArgs e)
		{
			try
			{
				Param.UpdateOthersAtrrib("NonSILFont", ((ComboBoxItem)cTool.DdlNonSILFont.SelectedItem).Value, StyleName);
				SetOthersSummary(sender, e);
			}
			catch { }
		}

		public void chkIncludeFontVariants_CheckedChangedBL(object sender, EventArgs e)
		{
			try
			{
				Param.UpdateOthersAtrrib("IncludeFontVariants", cTool.ChkIncludeFontVariants.Checked ? "Yes" : "No", StyleName);
				SetOthersSummary(sender, e);
			}
			catch { }
		}

		public void chkEmbedFonts_CheckedChangedBL(object sender, EventArgs e)
		{
			try
			{
				Param.UpdateOthersAtrrib("EmbedFonts", cTool.ChkEmbedFonts.Checked ? "Yes" : "No", StyleName);
				bool bEnabled = cTool.ChkEmbedFonts.Checked;
				cTool.ChkIncludeFontVariants.Enabled = bEnabled;
				cTool.DdlDefaultFont.Enabled = bEnabled;
				cTool.DdlMissingFont.Enabled = bEnabled;
				cTool.DdlNonSILFont.Enabled = bEnabled;
				SetOthersSummary(sender, e);
			}
			catch { }
		}

		public void tsPreview_ClickBL()
		{
			try
			{
				CreatePreviewFile();

				if (cTool.StylesGrid[3, SelectedRowIndex].Value.ToString().ToLower() != "standard")
				{
					return;
				}

				if (File.Exists(PreviewFileName1) || File.Exists(PreviewFileName2))
				{
					PreviewConfig preview = new PreviewConfig(PreviewFileName1,
															  PreviewFileName2)
												{
													Text = ("Preview - " + StyleName)
												};
					preview.Icon = cTool.Icon;
					preview.ShowDialog();
				}
			}
			catch { }
		}

		private void CreatePreviewFile()
		{
			try
			{
				string settingPath = Path.GetDirectoryName(Param.SettingPath);
				string inputPath = Common.PathCombine(settingPath, "Styles");
				inputPath = Common.PathCombine(inputPath, Param.Value["InputType"]);
				string stylenamePath = Common.PathCombine(inputPath, "Preview");
				string selectedTypeValue = cTool.StylesGrid[ColumnType, SelectedRowIndex].Value.ToString();
				if (selectedTypeValue != TypeStandard)
				{
					PreviewFileName1 = Common.PathCombine(Path.GetDirectoryName(Param.StylePath(FileName)), Path.GetFileNameWithoutExtension(Param.StylePath(FileName)) + ".pdf");
					bool isPreviewFileExist = File.Exists(PreviewFileName1) || File.Exists(PreviewFileName2);

					if (isPreviewFileExist == false || IsPropertyModified())
					{
						WriteCss();
						ShowCSSValue();
						_screenMode = ScreenMode.Edit;
						string cssMergeFullFileName = Param.StylePath(FileName);
						string PsSupportPath = Common.PathCombine(Common.LeftString(cssMergeFullFileName, "Pathway"),
															"Pathway");
						string PsSupportPathfrom = Common.GetApplicationPath();
						string previewFile = _loadType + "Preview.xhtml";
						string xhtmlPreviewFilePath = Common.PathCombine(PsSupportPath, previewFile);
						string xhtmlPreviewFile_fromPath = Common.PathCombine(PsSupportPathfrom, previewFile);
						if (!File.Exists(xhtmlPreviewFilePath))
						{
							if (File.Exists(xhtmlPreviewFile_fromPath))
							{
								File.Copy(xhtmlPreviewFile_fromPath, xhtmlPreviewFilePath);
							}
						}

						if (!(File.Exists(xhtmlPreviewFilePath) && File.Exists(cssMergeFullFileName)))
						{
							return;
						}

						PublicationInformation ps = new PublicationInformation();
						ps.DefaultXhtmlFileWithPath = xhtmlPreviewFilePath;
						ps.DefaultCssFileWithPath = cssMergeFullFileName;
						string fileName = Path.GetTempFileName();
						ps.ProjectName = fileName;
						ps.DictionaryOutputName = fileName;
						ps.DictionaryPath = Path.GetDirectoryName(xhtmlPreviewFilePath);
						ps.ProjectInputType = _loadType;

						bool success = Common.ShowPdfPreview(ps);

						if (!success)
						{
							var confirmationStringMessage = LocalizationManager.GetString("ConfigurationToolBL.CreatePreviewFile.Message", "Sorry a preview of this stylesheet is not available. Please install PrinceXML or LibreOffice to enable the preview.", "");
							Utils.MsgBox(confirmationStringMessage, _caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
						}

						PreviewFileName1 = Common.PathCombine(stylenamePath, "PreviewMessage.jpg");
						PreviewFileName2 = Common.PathCombine(stylenamePath, "PreviewMessage.jpg");
					}
					else
					{
						Process.Start(PreviewFileName1);
					}
				}
				else
				{
					PreviewFileName1 = Common.PathCombine(stylenamePath, Path.GetFileName(PreviewFileName1));
					PreviewFileName2 = Common.PathCombine(stylenamePath, Path.GetFileName(PreviewFileName2));
				}
			}
			catch { }
		}

		/// <summary>
		/// Comparing the loaded values in property values vs changed property values
		/// Except the Label controls
		/// </summary>
		/// <returns></returns>
		private bool IsPropertyModified()
		{
			if (_propertyValue.Count == 0 && _groupPropertyValue.Count == 0)
				return false;

			bool propertyModified = false;
			int i = 0;
			int g = 0;
			Control.ControlCollection ctls = cTool.TableLayoutPanel2.Controls; //cTool.TabControl1.TabPages[1].Controls
			foreach (Control control in ctls)
			{
				if (control.GetType().Name == "Label") continue;

				if (_groupPropertyValue.Count > 0 && _groupPropertyValue.Count >= g && control.GetType().Name == "GroupBox")
				{
					foreach (Control subControl in control.Controls)
					{
						string subValue = subControl.Text;
						if (_groupPropertyValue[g++] != subValue)
						{
							return true;
						}
					}
				}

				if (_groupPropertyValue.Count > 0 && _groupPropertyValue.Count >= g && control.GetType().Name == "TableLayoutPanel")
				{
					foreach (Control subControl in control.Controls)
					{
						string subValue = subControl.Text;
						if (_groupPropertyValue[g++] != subValue)
						{
							return true;
						}
					}
				}

				string val = control.Text;
				if (_propertyValue.Count > 0 && _propertyValue.Count >= i && _propertyValue[i++] != val)
				{
					propertyModified = true;
					break;
				}
			}
			return propertyModified;
		}

		public void CreateToolTip()
		{
			ToolTip toolTip = new ToolTip();
			toolTip.ShowAlways = true;
			toolTip.SetToolTip(cTool.BtnPrevious, "Show Page 1");
			toolTip.SetToolTip(cTool.BtnNext, "Show Page 2");
		}

		public void ddlPageColumn_SelectedIndexChangedBL(object sender, EventArgs e)
		{
			try
			{
				if (cTool.TxtPageGutterWidth.Text.Length == 0)
					cTool.TxtPageGutterWidth.Text = "18pt";
				cTool.TxtPageGutterWidth.Enabled = true;
			}
			catch { }
		}

		public void txtName_KeyUpBL()
		{
			try
			{
				UpdateGrid(cTool.TxtName, cTool.StylesGrid);
				_redoUndoBufferValue = cTool.TxtName.Text;
			}
			catch { }
		}

		public void txtComment_KeyUpBL()
		{
			try
			{
				UpdateGrid(cTool.TxtComment, cTool.StylesGrid);
				_redoUndoBufferValue = cTool.TxtComment.Text;
			}
			catch { }
		}

		public void txtDesc_KeyUpBL()
		{
			try
			{
				UpdateGrid(cTool.TxtDesc, cTool.StylesGrid);
				_redoUndoBufferValue = cTool.TxtDesc.Text;

			}
			catch { }
		}

		public void stylesGrid_ColumnWidthChangedBL(DataGridViewColumnEventArgs e)
		{
			try
			{
				string columnIndex = e.Column.Index.ToString();
				string columnWidth = e.Column.Width.ToString();
				SaveColumnWidth(columnIndex, columnWidth);
			}
			catch { }
		}

		public void stylesGrid_RowEnterBL(DataGridViewCellEventArgs e)
		{
			try
			{
				if (IsUnixOs)
				{
					if (cTool.TabControl1.SelectedTab.Text == "Preview")
					{
						SelectedRowIndex = e.RowIndex;
						ShowInfoValue();
					}
					else
					{
						if (_screenMode == ScreenMode.Modify || _screenMode == ScreenMode.Edit) // Add or Edit
						{
							FileName = cTool.StylesGrid[ColumnFile, SelectedRowIndex].Value.ToString();
							WriteCss();
						}
						_screenMode = ScreenMode.View;
						SelectedRowIndex = e.RowIndex;
						ShowInfoValue();
					}
				}
			}
			catch { }
		}

		public void txtApproved_ValidatedBL(object sender)
		{
			try
			{
				WriteAttrib(AttribApproved, sender);
				EnableToolStripButtons(true);

			}
			catch { }
		}

		public void DdlRunningHeadSelectedIndexChangedBl(string pageType)
		{
			if (pageType == "None")
			{
				cTool.TxtGuidewordLength.Text = "0";
				cTool.TxtGuidewordLength.Enabled = false;
                cTool.ChkCenterTitleHeader.Checked = false;
			    cTool.ChkCenterTitleHeader.Enabled = false;
			    cTool.DdlHeaderFontSize.Enabled = false;
			}
			else
			{
				cTool.TxtGuidewordLength.Text = "99";
				cTool.TxtGuidewordLength.Enabled = true;
                cTool.ChkCenterTitleHeader.Enabled = true;
                cTool.DdlHeaderFontSize.Enabled = true;
			}
			string xPath = string.Empty;
			Trace.WriteLineIf(_traceOnBL.Level == TraceLevel.Verbose, "ConfigurationTool: PopulatePageNumberFeature");
			xPath = "//features/feature[@name='Page Number']/option[@type='" + pageType + "' or @type= 'Both']";

			XmlNodeList pageNumList = Param.GetItems(xPath);
			cTool.DdlPageNumber.Items.Clear();
			cTool.DdlReferenceFormat.Items.Clear();

			ReloadPageNumberLocList(GetDdlRunningHead(), cTool);

			if (cTool.DdlPageNumber.Items.Count > 0)
				cTool.DdlPageNumber.SelectedIndex = 0;

			xPath = "//features/feature[@name='Reference Format']/option[@type='" + pageType + "' or @type= 'Both']";
			pageNumList = Param.GetItems(xPath);
			foreach (XmlNode node in pageNumList)
			{
				if (node.Attributes != null)
				{
					string value = node.Attributes["name"].Value;
					if (ComboBoxContains(value, cTool.DdlReferenceFormat))
					{
						string enText = value;
						value = LocalizeItems.LocalizeItem("Reference Format", value);
						cTool.DdlReferenceFormat.Items.Add(new ComboBoxItem(enText, value));
					}

				}
			}
			if (cTool.DdlReferenceFormat.Items.Count > 0)
				cTool.DdlReferenceFormat.SelectedIndex = 0;
		}

		public void chkAvailable_CheckedChangedBL(object sender)
		{
			try
			{
				WriteAttrib(ElementAvailable, sender);
				UpdateGrid(cTool.ChkAvailable, cTool.StylesGrid);
			}
			catch { }
		}

		public void txtComment_ValidatedBL(object sender, bool modified)
		{
			try
			{
				if (modified)
				{
					WriteAttrib(ElementComment, sender);
				}

			}
			catch { }
		}

		public void chkAvailable_ValidatedBL(object sender)
		{
			try
			{
				WriteAttrib(AttribShown, sender);
				EnableToolStripButtons(true);
			}
			catch { }
		}

		public void chkFixedLineHeight_CheckedChangedBL()
		{
			try
			{
				_fixedLineHeight = cTool.ChkFixedLineHeight.Checked;
			}
			catch { }
		}

        public void chkCenterTitleHeader_CheckStateChangedBL()
        {
            try
            {
	            if (cTool != null)
	            {
		            _centerTitleHeader = cTool.ChkCenterTitleHeader.Checked;
		            ReloadPageNumberLocList(GetDdlRunningHead(), cTool);
	            }
            }
            catch { }
        }

        /// <summary>
        /// Based on checkbox "Center title in Header" selection, The "Top Center option will be removed/added
        /// </summary>
		public void ReloadPageNumberLocList(string pageType, ConfigurationTool cTool)
	    {
	        //string pageType = GetDdlRunningHead();
	        string xPath = "//features/feature[@name='Page Number']/option[@type='" + pageType + "' or @type= 'Both']";
            const string titlePath = "//Metadata/meta[@name='Title']/defaultValue";
	        XmlNodeList pageNumList = Param.GetItems(xPath);
	        if (!Common.Testing)
		        _TitleText = Param.GetItem(titlePath).InnerText;
			cTool.DdlPageNumber.Items.Clear();
	        foreach (XmlNode node in pageNumList)
	        {
	            if (node.Attributes != null)
	            {
	                string value = node.Attributes["name"].Value;
	                if (ComboBoxContains(value, cTool.DdlPageNumber))
	                {
	                    string enText = value;
	                    value = LocalizeItems.LocalizeItem("Page Number", value);
						if ((_centerTitleHeader && enText.ToLower() == "top center") || 
							(cTool.DdlRunningHead.SelectedItem.ToString().ToLower() == "mirrored" && enText.ToLower() == "top outside margin"))
	                        continue;
						if (enText.ToLower() == "none" && cTool.DdlRunningHead.Text.ToLower() == "none")
						{
							value = LocalizeItems.LocalizeItem("Reference Format", value);
							cTool.DdlReferenceFormat.Items.Add(new ComboBoxItem(enText, value));
							cTool.DdlPageNumber.Items.Add(new ComboBoxItem(enText, value));
						}
						else if (enText.ToLower() != "none")
						{
							cTool.DdlPageNumber.Items.Add(new ComboBoxItem(enText, value));
						}
	                }
	            }
	        }
			if(cTool.DdlPageNumber.Items.Count > 0)
				cTool.DdlPageNumber.SelectedIndex = 0;
	    }

	    public void chkIncludeImage_CheckedChangedBL(object sender, EventArgs e)
		{
			try
			{
				_includeImage = cTool.ChkIncludeImage.Checked;
				cTool.LblMaxImageWidth.Enabled = _includeImage;
				cTool.TxtMaxImageWidth.Enabled = _includeImage;
				cTool.LblPx.Enabled = _includeImage;
				Param.UpdateOthersAtrrib("IncludeImage", cTool.ChkIncludeImage.Checked ? "Yes" : "No", StyleName);
				SetOthersSummary(sender, e);
			}
			catch { }
		}

		public void chkPageBreaks_CheckedChangedBL(object sender, EventArgs e)
		{
			try
			{
				Param.UpdateOthersAtrrib("PageBreak", cTool.ChkPageBreaks.Checked ? "Yes" : "No", StyleName);
				SetOthersSummary(sender, e);
			}
			catch { }
		}

		public void chkHideSpaceVerseNo_CheckStateChangedBL(object sender, EventArgs e)
		{
			try
			{

			}
			catch { }
		}

		public void chkIncludeCusFnCaller_CheckedChangedBL(object sender, EventArgs e)
		{
			try
			{
				cTool.TxtFnCallerSymbol.Enabled = cTool.ChkIncludeCusFnCaller.Checked;
				if (cTool.ChkIncludeCusFnCaller.Checked == false)
					cTool.TxtFnCallerSymbol.Text = "";
				_includeFootnoteCaller = cTool.TxtFnCallerSymbol.Text;
			}
			catch { }
		}

		public void chkSplitFileByLetter_CheckStateChangedBL(object sender, EventArgs e)
		{
			try
			{
				_splitFileByLetter = cTool.ChkSplitFileByLetter.Checked;
			}
			catch { }
		}

		public void chkDisableWO_CheckStateChangedBL(object sender, EventArgs e)
		{
			try
			{
				_disableWidowOrphan = cTool.ChkDisableWO.Checked;
			}
			catch { }
		}

		public void txtFnCallerSymbol_KeyUpBL()
		{
			try
			{
				_includeFootnoteCaller = cTool.TxtFnCallerSymbol.Text;
			}
			catch { }
		}

		public void TxtXRefCusSymbol_KeyUpBL()
		{
			try
			{
				_includeXRefCaller = cTool.TxtXrefCusSymbol.Text;
			}
			catch { }
		}

		public void chkXrefCusSymbol_CheckStateChangedBL(object sender, EventArgs e)
		{
			try
			{
				cTool.TxtXrefCusSymbol.Enabled = cTool.ChkXrefCusSymbol.Checked;
				if (cTool.ChkXrefCusSymbol.Checked == false)
					cTool.TxtXrefCusSymbol.Text = "";
				_includeXRefCaller = cTool.TxtXrefCusSymbol.Text;
			}
			catch { }
		}


		public void txtDesc_ValidatedBL(object sender, bool modified)
		{
			try
			{
				if (modified)
				{
					WriteAttrib(ElementDesc, sender);
				}

			}
			catch { }
		}

		public void txtName_ValidatingBL(object sender)
		{
			if (_screenMode != ScreenMode.Modify) return;
			try
			{
				cTool.TxtName.Text = cTool.TxtName.Text.Trim();
				if (cTool._previousTxtName == cTool.TxtName.Text) return;
				PreviousStyleName = cTool._previousTxtName;
				bool isNoDuplicateStyleName = NoDuplicateStyleName();
				bool isValidateStyleName = ValidateStyleName(cTool.TxtName.Text);

				if (!isNoDuplicateStyleName || !isValidateStyleName)
				{
					cTool.TxtName.Text = _previousStyleName;
					cTool.StylesGrid.Rows[SelectedRowIndex].Cells[0].Value = _previousStyleName;
					cTool.TxtName.Focus();
					return;
				}
				string styleName = cTool.TxtName.Text;
				FileName = cTool.StylesGrid[ColumnFile, SelectedRowIndex].Value.ToString();
				cTool.StylesGrid[ColumnFile, SelectedRowIndex].Value = FileName;

				if (_screenMode == ScreenMode.New) // Add
				{
					Param.StyleFile[styleName] = FileName;
					string errMsg = CreateCssFile(FileName);
					if (errMsg.Length > 0)
					{
						var confirmationStringMessage = LocalizationManager.GetString("ConfigurationToolBL.NameValidate.Message", "Sorry, your recent changes cannot be saved because Pathway cannot find the stylesheet file '{0}'", "");
						confirmationStringMessage = String.Format(confirmationStringMessage, errMsg);
						Utils.MsgBox(confirmationStringMessage, _caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
					}
					AddNew(cTool.TxtName.Text);
					EnableToolStripButtons(true);
					ShowStyleInGrid(cTool.StylesGrid, _cssNames);
					SelectRow(cTool.StylesGrid, PreviousValue);
					ShowInfoValue();
				}
				else if (_screenMode == ScreenMode.Modify)
				{
					if (PreviousValue == styleName)
					{
						return;
					}
					string path = Param.Value["UserSheetPath"];
					string fromFile = Common.PathCombine(path, PreviousValue + ".css");
					string toFile = Common.PathCombine(path, FileName);
					if (File.Exists(fromFile))
						try
						{
							File.Move(fromFile, toFile);
						}
						catch
						{
						}
					Param.StyleFile[styleName] = FileName;
					WriteAttrib(AttribName, cTool.TxtName);
					_cssNames.Remove(PreviousValue);
					EnableToolStripButtons(true);
					IsLayoutSelectedStyle();
				}
				StyleName = cTool.TxtName.Text;
				SetInfoCaption(cTool.TxtName.Text);
			}
			catch { }
		}

		private void SetInfoCaption(string txtName)
		{
			int width = cTool.LblInfoCaption.Width / 11;
			if (txtName.Length > width)
			{
				cTool.LblInfoCaption.Text = txtName.Remove(width) + "...";
			}
			else
			{
				cTool.LblInfoCaption.Text = txtName;
			}
		}

		public void btnScripture_ClickBL()
		{
			try
			{
				ShowCSSValue();
				WriteCss();
				cTool.DdlTocLevel.Items.Clear(); // clear / repopulate this dropdown
				setLastSelectedLayout();
				WriteMedia();
				inputTypeBL = "Scripture";
				SetInputTypeButton();
				LoadParam();
				ClearPropertyTab(cTool.TabDisplay);
				ClearPropertyTab(tabmob);
				ClearPropertyTab(tabothers);
				ClearPropertyTab(tabweb);
				PopulateFeatureSheet(); //For TD-1194 // Load Default Values
				ShowCSSValue();
				SetPreviousLayoutSelect(cTool.StylesGrid);
				SetSideBar();
				ShowDataInGrid();
				SetPropertyTab();
			}
			catch
			{
			}
		}

		public void btnDictionary_ClickBL()
		{
			try
			{
				ShowCSSValue();
				WriteCss();
				cTool.DdlTocLevel.Items.Clear(); // clear / repopulate this dropdown
				setLastSelectedLayout();
				WriteMedia();
				inputTypeBL = "Dictionary";
				SetInputTypeButton();
				LoadParam();
				ClearPropertyTab(cTool.TabDisplay);
				PopulateFeatureSheet(); //For TD-1194 // Load Default Values
				ShowCSSValue();
				SetPreviousLayoutSelect(cTool.StylesGrid);
				SetSideBar();
				ShowDataInGrid();
				SetPropertyTab();
			}
			catch
			{
			}
		}

		public void ConfigurationTool_FormClosingBL()
		{
			try
			{
				setLastSelectedLayout();
				setDefaultInputType();
				Common.SaveInputType(inputTypeBL);
				WriteCss();
				StyleEXE = cTool.TxtName.Text;
			}
			catch { }
		}

		public void txtPageTop_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				Common.AssignValuePageUnit(cTool.TxtPageTop, null);
				_errProvider = Common._errProvider;
				if (_errProvider.GetError(cTool.TxtPageTop) != "")
				{
					_errProvider.SetError(cTool.TxtPageTop, _errProvider.GetError(cTool.TxtPageTop));
				}
				else
				{
					ValidatePageHeightMargins(sender, e);
				}
			}
			catch { }
		}

		public void txtPageOutside_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				Common.AssignValuePageUnit(cTool.TxtPageOutside, null);
				_errProvider = Common._errProvider;
				if (_errProvider.GetError(cTool.TxtPageOutside) != "")
				{
					_errProvider.SetError(cTool.TxtPageOutside, _errProvider.GetError(cTool.TxtPageOutside));
				}
				else
				{
					ValidatePageWidthMargins(sender, e);
				}
			}
			catch { }
		}

		private void ValidatePageWidthMargins(object sender, EventArgs e)
		{
			ValidatePageWidthMarginsBL(sender);
		}

		private void ValidatePageHeightMargins(object sender, EventArgs e)
		{
			ValidatePageHeightMarginsBL(sender);
		}

		private void SetMobileSummary(object sender, EventArgs e)
		{
			ShowMobileSummaryBL();
		}

		private void SetOthersSummary(object sender, EventArgs e)
		{
			ShowOthersSummaryBL();
		}

		private void SetWebSummary(object sender, EventArgs e)
		{
			ShowWebSummaryBL();
		}

		public void txtPageInside_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				Common.AssignValuePageUnit(cTool.TxtPageInside, null);
				_errProvider = Common._errProvider;
				if (_errProvider.GetError(cTool.TxtPageInside) != "")
				{
					_errProvider.SetError(cTool.TxtPageInside, _errProvider.GetError(cTool.TxtPageInside));
				}
				else
				{
					ValidatePageWidthMargins(sender, e);
				}
			}
			catch { }
		}

		public void txtPageGutterWidth_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				Common.AssignValuePageUnit(cTool.TxtPageGutterWidth, null);
				_errProvider = Common._errProvider;
				if (_errProvider.GetError(cTool.TxtPageGutterWidth) != "")
				{
					_errProvider.SetError(cTool.TxtPageGutterWidth, _errProvider.GetError(cTool.TxtPageGutterWidth));
				}
				else
				{
					ValidatePageWidthMargins(sender, e);
				}
			}
			catch { }
		}

		public void tsDefault_ClickBL()
		{
			try
			{
				string formText = LocalizationManager.GetString("ExportThroughPathway.Dialog", "Set Defaults", "");
				// EDB (2 May 2011): TD-2344 / replace with Export Through Pathway dlg
				var dlg = new ExportThroughPathway(formText);
				dlg.InputType = inputTypeBL;
				dlg.DatabaseName = "{Project_Name}";
				dlg.Media = MediaType;
				dlg.ShowDialog();
			}
			catch { }
		}

		public void tsNew_ClickBL()
		{
			try
			{
				_screenMode = ScreenMode.New;
				AddStyleInXML(cTool.StylesGrid, _cssNames);
				ShowStyleInGrid(cTool.StylesGrid, _cssNames);
				SelectRow(cTool.StylesGrid, NewStyleName);
				WriteCss();
				cTool.TabControl1.SelectedIndex = 0;
				cTool.PicPreview.Visible = false;
				cTool.BtnPrevious.Visible = false;
				cTool.BtnNext.Visible = false;

				string seletedLayout = string.Empty;
				cTool.StylesGrid.Rows[cTool.StylesGrid.Rows.Count - 1].Selected = true;
				seletedLayout = cTool.StylesGrid.Rows[cTool.StylesGrid.Rows.Count - 1].Cells[0].Value.ToString();
				_lastSelectedLayout = seletedLayout;
				_previousStyleName = seletedLayout;
				cTool.TxtName.Text = seletedLayout;
				setLastSelectedLayout();

				_screenMode = ScreenMode.View;
				SelectedRowIndex = cTool.StylesGrid.Rows.Count - 1;
				ShowInfoValue();
				cTool.TxtName.Select();
			}
			catch { }
		}

		public void tsSend_ClickBL()
		{
			WriteCss();

			if (!_validateWebInput)
			{
				_validateWebInput = true;
				return;
			}

			string tempfolder = Path.GetTempPath();
			string folderName = Path.GetFileNameWithoutExtension(Path.GetTempFileName());
			string folderPath = Common.PathCombine(tempfolder, folderName);
			bool directoryCreated = CopyCustomStyleToSend(folderPath);
			if (directoryCreated)
			{
				try
				{
					ZipFolder zf = new ZipFolder();
					string projType = GetProjType();
					string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
					string zipFileName = Path.GetFileNameWithoutExtension(Path.GetTempFileName());
					string zipOutput = Common.PathCombine(path, zipFileName + ".zip");
					zf.CreateZip(folderPath, zipOutput, 0);
					const string MailTo = "Pathway@sil.org";
					string MailSubject = projType + " Style Sheets and Setting file";
					string MailBody = "(Please attach the exported " + "%20" + zipOutput + " with this mail.)" +
									  "%0D%0A" + "%0D%0A";
					try
					{
						MailBody += "Extract the zip folder content to an appropriate folder on your hard drive." +
									"%0D%0A" + "%0D%0A";
						MailBody = GetMailBody(projType, MailBody);
					}
					catch
					{
					}

					Process.Start(string.Format("mailto:{0}?Subject={1}&Body={2}", MailTo,
										   MailSubject, MailBody));
				}
				catch (Exception ex)
				{

					Utils.MsgBox(ex.Message, _caption);
				}
			}
		}

		public void txtPageBottom_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				Common.AssignValuePageUnit(cTool.TxtPageBottom, null);
				_errProvider = Common._errProvider;
				if (_errProvider.GetError(cTool.TxtPageBottom) != "")
				{
					_errProvider.SetError(cTool.TxtPageBottom, _errProvider.GetError(cTool.TxtPageBottom));
				}
				else
				{
					ValidatePageHeightMargins(sender, e);
				}
			}
			catch { }
		}

		private void SetMenuToolStrip()
		{
			if (IsUnixOs)
			{
				Font cFont = new System.Drawing.Font("Charis SIL", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				Size cSize = new System.Drawing.Size(36, 49);
				const ContentAlignment contentBc = System.Drawing.ContentAlignment.BottomCenter;
				const ToolStripTextDirection toolStripTextDirection = ToolStripTextDirection.Horizontal;
				const ContentAlignment contentTc = System.Drawing.ContentAlignment.TopCenter;

				cTool.TsNew.Font = cFont;
				cTool.TsNew.Size = cSize;
				cTool.TsNew.TextAlign = contentBc;
				cTool.TsNew.TextDirection = toolStripTextDirection;
				cTool.TsNew.ImageAlign = contentTc;

				cTool.TsSaveAs.Font = cFont;
				cTool.TsSaveAs.Size = cSize;
				cTool.TsSaveAs.TextAlign = contentBc;
				cTool.TsSaveAs.TextDirection = toolStripTextDirection;
				cTool.TsSaveAs.ImageAlign = contentTc;

				cTool.TsDelete.Font = cFont;
				cTool.TsDelete.Size = cSize;
				cTool.TsDelete.TextAlign = contentBc;
				cTool.TsDelete.TextDirection = toolStripTextDirection;
				cTool.TsDelete.ImageAlign = contentTc;

				cTool.TsUndo.Font = cFont;
				cTool.TsUndo.Size = cSize;
				cTool.TsUndo.TextAlign = contentBc;
				cTool.TsUndo.TextDirection = toolStripTextDirection;
				cTool.TsUndo.ImageAlign = contentTc;

				cTool.TsRedo.Font = cFont;
				cTool.TsRedo.Size = cSize;
				cTool.TsRedo.TextAlign = contentBc;
				cTool.TsRedo.TextDirection = toolStripTextDirection;
				cTool.TsRedo.ImageAlign = contentTc;

				cTool.TsPreview.Font = cFont;
				cTool.TsPreview.Size = cSize;
				cTool.TsPreview.TextAlign = contentBc;
				cTool.TsPreview.TextDirection = toolStripTextDirection;
				cTool.TsPreview.ImageAlign = contentTc;

				cTool.TsDefault.Font = cFont;
				cTool.TsDefault.Size = cSize;
				cTool.TsDefault.TextAlign = contentBc;
				cTool.TsDefault.TextDirection = toolStripTextDirection;
				cTool.TsDefault.ImageAlign = contentTc;

				cTool.TsReset.Font = cFont;
				cTool.TsReset.Size = cSize;
				cTool.TsReset.TextAlign = contentBc;
				cTool.TsReset.TextDirection = toolStripTextDirection;
				cTool.TsReset.ImageAlign = contentTc;

				cTool.TsSend.Font = cFont;
				cTool.TsSend.Size = cSize;
				cTool.TsSend.TextAlign = contentBc;
				cTool.TsSend.TextDirection = toolStripTextDirection;
				cTool.TsSend.ImageAlign = contentTc;

				cTool.ToolStripHelpButton.Font = cFont;
				cTool.ToolStripHelpButton.Size = cSize;
				cTool.ToolStripHelpButton.TextAlign = contentBc;
				cTool.ToolStripHelpButton.TextDirection = toolStripTextDirection;
				cTool.ToolStripHelpButton.ImageAlign = contentTc;
			}
		}

		public void SetModifyMode(bool setEdited)
		{
			if (_screenMode == ScreenMode.Edit || setEdited)
			{
				_screenMode = ScreenMode.Modify;
				setEdited = false;
			}
		}

		public void ConfigurationTool_KeyUpBL(object sender, KeyEventArgs e)
		{
			try
			{
				if (cTool.StylesGrid.Focused && e.KeyCode == Keys.Delete)
				{
					if (cTool.TsDelete.Enabled)
					{
						cTool.tsDelete_Click(sender, null);
					}
				}
				else if (e.KeyCode == Keys.F1)
				{
					CallHelp(new Label());
				}

				//Show Version when Ctrl+F12
				if (e.Control && e.KeyCode == Keys.F12)
				{
					cTool.Text = "Pathway Configuration Tool - " + AssemblyFileVersion;
				}
			}
			catch { }
		}

		public void HelpButton_Clicked(Control ctrl)
		{
			CallHelp(ctrl);
		}

		private void CallHelp(Control ctrl)
		{
			ShowHelp.ShowHelpTopicKeyPress(ctrl, "Overview.htm", _isUnixOS);
		}

		public void StudentManual()
		{
			Common.PathwayStudentManualLaunch();
		}

		public void AboutDialog()
		{
			var aboutPw = new AboutPw();
			aboutPw.ShowDialog();
		}

		public void tsDelete_ClickBL()
		{
			_screenMode = ScreenMode.Delete;
			string name = cTool.TxtName.Text;
			string caption = "Delete Stylesheet";
			var confirmationStringMessage = LocalizationManager.GetString("ConfigurationToolBL.TabDeleteClick.Message1", "Are you sure you want to delete the {0} stylesheet?", "");
			confirmationStringMessage = string.Format(confirmationStringMessage, name);
			if (!Common.Testing)
			{
				DialogResult result = Utils.MsgBox(confirmationStringMessage, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
													  MessageBoxDefaultButton.Button2);
				if (result != DialogResult.OK) return;
			}
			try
			{
				if (SelectedRowIndex >= 0)
				{
					int currentRowIndex = SelectedRowIndex;
					string selectedTypeValue = cTool.StylesGrid[ColumnType, SelectedRowIndex].Value.ToString();
					if (selectedTypeValue != TypeStandard)
					{
						_cssNames.Remove(StyleName);
						RemoveXMLNode(StyleName);
						LoadParam();
						ShowDataInGrid();
						SetPropertyTab();
						SelectedRowIndex = currentRowIndex;
						if (currentRowIndex == cTool.StylesGrid.Rows.Count) // Is last row?
							SelectedRowIndex = SelectedRowIndex - 1;
						cTool.StylesGrid.ClearSelection();
						cTool.StylesGrid.Rows[SelectedRowIndex].Selected = true;
						string selectedLayout = cTool.StylesGrid.Rows[SelectedRowIndex].Cells[0].Value.ToString();
						_lastSelectedLayout = selectedLayout;
						_previousStyleName = selectedLayout;
						cTool.TxtName.Text = selectedLayout;
						setLastSelectedLayout();
						_screenMode = ScreenMode.View;
						ShowInfoValue();
						cTool.TxtName.Select();
						ConfigurationTool_LoadBL();
					}
					else
					{
						confirmationStringMessage = LocalizationManager.GetString("ConfigurationToolBL.TabDeleteClick.Message2", "Factory style sheet can not be deleted", "");
						Utils.MsgBox(confirmationStringMessage, _caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				else
				{
					confirmationStringMessage = LocalizationManager.GetString("ConfigurationToolBL.TabDeleteClick.Message3", "Please select a style sheet to delete", "");
					Utils.MsgBox(confirmationStringMessage, _caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch
			{
				_screenMode = ScreenMode.Edit;
				ShowInfoValue();
			}
			PreviousStyleName = cTool.StylesGrid.Rows[SelectedRowIndex].Cells[0].Value.ToString();
			ShowCSSValue();
			WriteCss();
		}

		public void chkTurnOffFirstVerse_CheckStateChangedBL(object sender, EventArgs e)
		{
			try
			{
				_hideVerseNumberOne = cTool.ChkTurnOffFirstVerse.Checked;
			}
			catch { }
		}

		public void tabControl1_SelectedIndexChangedBL()
		{
			if (cTool.TabControl1.SelectedIndex == 0) // css properties
			{
				WriteCss();
			}
			else if (cTool.TabControl1.SelectedIndex == 1) // css properties
			{
				if (inputTypeBL.ToLower() == "dictionary")
				{
					cTool.PnlGuidewordLength.Visible = true;
					cTool.PnlReferenceFormat.Visible = false;
					_cToolPnlOtherFormatTop = cTool.PnlOtherFormat.Top;
					cTool.PnlOtherFormat.Top = cTool.PnlGuidewordLength.Location.Y + cTool.PnlGuidewordLength.Height;
				}
				else
				{
					GuidewordPnlAlignment();
					if (_cToolPnlOtherFormatTop > 0)
					{
						cTool.PnlReferenceFormat.Visible = true;
						cTool.PnlReferenceFormat.Top = cTool.PnlGuidewordLength.Top;
						cTool.PnlOtherFormat.Top = cTool.PnlReferenceFormat.Location.Y + cTool.PnlReferenceFormat.Height;

					}
				}

				ShowCSSValue();
				if (cTool.BtnPaper.Enabled && cTool.TabControl1.TabPages[1].Enabled)
				{
					txtPageInside_ValidatedBL(cTool.TxtPageInside, null);
					txtPageOutside_ValidatedBL(cTool.TxtPageOutside, null);
					txtPageTop_ValidatedBL(cTool.TxtPageTop, null);
					txtPageBottom_ValidatedBL(cTool.TxtPageBottom, null);
					txtPageGutterWidth_ValidatedBL(cTool.TxtPageGutterWidth, null);
				}
			}
			else if (cTool.TabControl1.SelectedTab.Text == "Preview")
			{
				ShowPreview(1);
			}
		}

		private void GuidewordPnlAlignment()
		{
			cTool.PnlGuidewordLength.Visible = false;
			cTool.PnlReferenceFormat.Top = cTool.PnlGuidewordLength.Top;
			cTool.PnlOtherFormat.Top = cTool.PnlReferenceFormat.Location.Y + cTool.PnlReferenceFormat.Height;
		}

		public void tsSaveAs_ClickBL()
		{
			try
			{
				_screenMode = ScreenMode.SaveAs;
				StyleName = cTool.TxtName.Text;
				if (CopyStyle(cTool.StylesGrid, _cssNames))
				{
					ShowStyleInGrid(cTool.StylesGrid, _cssNames);
					string seletedLayout = string.Empty;
					cTool.StylesGrid.Rows[cTool.StylesGrid.Rows.Count - 1].Selected = true;
					seletedLayout = cTool.StylesGrid.Rows[cTool.StylesGrid.Rows.Count - 1].Cells[0].Value.ToString();
					_lastSelectedLayout = seletedLayout;
					_previousStyleName = seletedLayout;
					cTool.TxtName.Text = seletedLayout;
					setLastSelectedLayout();
					SelectRow(cTool.StylesGrid, PreviousStyleName);
					WriteCss();

					_screenMode = ScreenMode.View;
					SelectedRowIndex = cTool.StylesGrid.Rows.Count - 1;
					ShowInfoValue();
					cTool.TxtName.Focus();
				}
			}
			catch { }
		}

		public void tsReset_ClickBL()
		{
			var confirmationStringMessage = "Settings files cannot be reset.";
			try
			{
				confirmationStringMessage = LocalizationManager.GetString("ConfigurationToolBL.TabResetClick1.Message",
					"Are you sure you want to remove all custom style sheets and restore \r\n settings to their initial values? (This cannot be undone.)", "");
				const string caption = "Reset Settings";
				if (!Common.Testing)
				{
					DialogResult result = Utils.MsgBox(confirmationStringMessage, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
														  MessageBoxDefaultButton.Button2);
					if (result == DialogResult.No)
						return;
					else
					{
						if (cTool.StylesGrid.Rows.Count > 0)
							cTool.StylesGrid.Rows[0].Selected = true;
					}
				}

				string allUsersPath = Common.GetAllUserPath();
				if (Directory.Exists(allUsersPath))
				{
					DirectoryInfo di = new DirectoryInfo(allUsersPath);
					Common.CleanDirectory(di);
				}
				SelectedRowIndex = 0;
				inputTypeBL = cTool.InputType;
				ConfigurationTool_LoadBL();
				confirmationStringMessage = LocalizationManager.GetString("ConfigurationToolBL.TabResetClick2.Message",
					"Settings files are reset successfully", "");
			}
			catch { }
			Utils.MsgBox(confirmationStringMessage, _caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public void txtBaseFontSize_LeaveBL(object sender)
		{
			try
			{
				if (ValidateEPubFontSize(Convert.ToDouble(((TextBox)sender).Text)) == false)
					MessageBox.Show("Please enter values from 6.0 to 28.0", string.Empty, MessageBoxButtons.OK);
			}
			catch { }
		}

		public void ShowPreview(int page)
		{
			var myCursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;

			string preview;

			if (!File.Exists(PreviewFileName1) || _screenMode == ScreenMode.Modify || _screenMode == ScreenMode.Edit)
			{
				CreatePreviewFile();
				cTool.PicPreview.Visible = false;
			}
			cTool.PicPreview.SizeMode = PictureBoxSizeMode.StretchImage;
			if (File.Exists(PreviewFileName1))
			{
				cTool.BtnPrevious.Visible = true;
				cTool.BtnNext.Visible = true;
			}
			else
			{
				cTool.BtnPrevious.Visible = false;
				cTool.BtnNext.Visible = false;
			}

			if (page == 1)
			{
				preview = PreviewFileName1;
				cTool.BtnPrevious.Enabled = false;
				cTool.BtnNext.Enabled = true;
			}
			else
			{
				preview = PreviewFileName2;
				cTool.BtnPrevious.Enabled = true;
				cTool.BtnNext.Enabled = false;
			}

			ShowPreviewBasedFileType(preview);
			Cursor.Current = myCursor;
		}

		private void ShowPreviewBasedFileType(string preview)
		{
			if (FileType.ToLower() == "custom")
			{
				if (File.Exists(preview))
				{
					cTool.PicPreview.Visible = true;
					cTool.PicPreview.Image = Image.FromFile(preview);
				}
				else
				{
					ShowCustomPreviewImage();
				}
			}
			else
			{
				if (File.Exists(preview))
				{
					cTool.PicPreview.Visible = true;
					cTool.PicPreview.Image = Image.FromFile(preview);
				}
				else
				{
					cTool.BtnPrevious.Visible = false;
					cTool.BtnNext.Visible = false;
				}
			}
		}

		private void ShowCustomPreviewImage()
		{
			string preview;
			string pathwayDirectory = Common.AssemblyPath;
			pathwayDirectory = Common.PathCombine(pathwayDirectory, "Styles");

			if (!Directory.Exists(pathwayDirectory))
			{
				pathwayDirectory = Path.GetDirectoryName(Common.AssemblyPath);
				pathwayDirectory = Common.PathCombine(pathwayDirectory, "Styles");
			}

			pathwayDirectory = Common.PathCombine(pathwayDirectory, inputTypeBL);
			pathwayDirectory = Common.PathCombine(pathwayDirectory, "Preview");
			preview = Common.PathCombine(pathwayDirectory, "PreviewMessage.jpg");
			if (File.Exists(preview))
			{
				cTool.PicPreview.Visible = true;
				cTool.PicPreview.Image = Image.FromFile(preview);
				cTool.BtnPrevious.Visible = false;
				cTool.BtnNext.Visible = false;
			}
		}

		private string GetDdlRunningHead()
		{
			string pageType = string.Empty;
			if (cTool.DdlRunningHead.SelectedIndex != -1)
			{
				pageType = ((ComboBoxItem)cTool.DdlRunningHead.SelectedItem).Value;
			}
			else
			{
				if (cTool.DdlRunningHead.Items.Count > 0)
					pageType = ((ComboBoxItem)cTool.DdlRunningHead.Items[0]).Value;
				//pageType = cTool.DdlRunningHead.Items[0].ToString();
			}
			return pageType;
		}

		public void stylesGrid_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				if (_screenMode == ScreenMode.Modify || _screenMode == ScreenMode.Edit) // Add or Edit
				{
					WriteCss();
				}

				_screenMode = ScreenMode.View;
				if (cTool.StylesGrid.CurrentRow != null)
					SelectedRowIndex = cTool.StylesGrid.CurrentRow.Index;

				ShowInfoValue();
			}
			catch { }
		}

		public void txtFtpFileLocation_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				if (cTool.TxtFtpAddress.Text.Trim() != string.Empty)
				{
					bool result = Regex.IsMatch(cTool.TxtFtpAddress.Text, @"(((ftp|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?");
					if (!result)
					{
						_errProvider = Common._errProvider;
						_errProvider.SetError(cTool.TxtFtpAddress, "Ftp address is not valid entry.");
						cTool.TxtFtpAddress.Focus();
					}
					else
					{
						_errProvider.SetError(cTool.TxtFtpAddress, "");
					}
				}
				else
				{
					_errProvider.SetError(cTool.TxtFtpAddress, "");
				}
			}
			catch { }
		}

		public void txtWebUrl_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				if (cTool.TxtWebUrl.Text.Trim() != string.Empty)
				{
					bool result = Regex.IsMatch(cTool.TxtWebUrl.Text, @"^(http|https)://?");
					if (!result)
					{
						_errProvider = Common._errProvider;
						_errProvider.SetError(cTool.TxtWebUrl, "Website URL address is not valid entry.");
						cTool.TxtWebUrl.Focus();
					}
					else
					{
						_errProvider.SetError(cTool.TxtWebUrl, "");
					}
				}
				else
				{
					_errProvider.SetError(cTool.TxtWebUrl, "");
				}
			}
			catch { }
		}

		public void txtSqlUsername_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				if (cTool.TxtSqlUsername.Text.Trim() != string.Empty)
				{
					int result = cTool.TxtSqlUsername.Text.Length;
					if (result <= 5)
					{
						_errProvider = Common._errProvider;
						_errProvider.SetError(cTool.TxtSqlUsername, "Sql username should be minimum 5 characters.");
						cTool.TxtSqlUsername.Focus();
					}
					else
					{
						_errProvider.SetError(cTool.TxtSqlUsername, "");
					}
				}
				else
				{
					_errProvider.SetError(cTool.TxtSqlUsername, "");
				}
			}
			catch { }
		}

		public void txtSqlPassword_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				if (cTool.TxtSqlPassword.Text.Trim() != string.Empty)
				{
					int result = cTool.TxtSqlPassword.Text.Length;
					if (result <= 8)
					{
						_errProvider = Common._errProvider;
						_errProvider.SetError(cTool.TxtSqlPassword, "Sql password should be minimum 8 characters.");
						cTool.TxtSqlPassword.Focus();
					}
					else
					{
						_errProvider.SetError(cTool.TxtSqlPassword, "");
					}
				}
				else
				{
					_errProvider.SetError(cTool.TxtSqlPassword, "");
				}
			}
			catch { }
		}

		public void txtWebAdminUsrNme_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				if (cTool.TxtWebAdminUsrNme.Text.Trim() != string.Empty)
				{
					int result = cTool.TxtWebAdminUsrNme.Text.Length;
					if (result <= 5)
					{
						_errProvider = Common._errProvider;
						_errProvider.SetError(cTool.TxtWebAdminUsrNme, "Web admin username should be minimum 5 characters.");
						cTool.TxtWebAdminUsrNme.Focus();
					}
					else
					{
						_errProvider.SetError(cTool.TxtWebAdminUsrNme, "");
					}
				}
				else
				{
					_errProvider.SetError(cTool.TxtWebAdminUsrNme, "");
				}
			}
			catch { }
		}

		public void txtWebAdminPwd_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				if (cTool.TxtWebAdminPwd.Text.Trim() != string.Empty)
				{
					int result = cTool.TxtWebAdminPwd.Text.Length;
					if (result <= 8)
					{
						_errProvider = Common._errProvider;
						_errProvider.SetError(cTool.TxtWebAdminPwd, "Web admin password should be minimum 8 characters.");
						cTool.TxtWebAdminPwd.Focus();
					}
					else
					{
						_errProvider.SetError(cTool.TxtWebAdminPwd, "");
					}
				}
				else
				{
					_errProvider.SetError(cTool.TxtWebAdminPwd, "");
				}
			}
			catch { }
		}

		public void txtFtpUsername_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				if (cTool.TxtFtpUsername.Text.Trim() != string.Empty)
				{
					int result = cTool.TxtFtpUsername.Text.Length;
					if (result <= 5)
					{
						_errProvider = Common._errProvider;
						_errProvider.SetError(cTool.TxtFtpUsername, "Ftp username should be minimum 5 characters.");
						cTool.TxtFtpUsername.Focus();
					}
					else
					{
						_errProvider.SetError(cTool.TxtFtpUsername, "");
					}
				}
				else
				{
					_errProvider.SetError(cTool.TxtFtpUsername, "");
				}
			}
			catch { }
		}

		public void txtFtpPassword_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				if (cTool.TxtFtpPassword.Text.Trim() != string.Empty)
				{
					int result = cTool.TxtFtpPassword.Text.Length;
					if (result <= 8)
					{
						_errProvider = Common._errProvider;
						_errProvider.SetError(cTool.TxtFtpPassword, "Ftp password should be minimum 8 characters.");
						cTool.TxtFtpPassword.Focus();
					}
					else
					{
						_errProvider.SetError(cTool.TxtFtpPassword, "");
					}
				}
				else
				{
					_errProvider.SetError(cTool.TxtFtpPassword, "");
				}
			}
			catch { }
		}

		public void txtWebEmailID_ValidatedBL(object sender, EventArgs e)
		{
			try
			{
				if (cTool.TxtWebEmailId.Text.Trim() != string.Empty)
				{
					bool isEmail = Regex.IsMatch(cTool.TxtWebEmailId.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
					if (!isEmail)
					{
						_errProvider = Common._errProvider;
						_errProvider.SetError(cTool.TxtWebEmailId, "Email id is not valid entry.");
						cTool.TxtWebEmailId.Focus();
					}
					else
					{
						_errProvider.SetError(cTool.TxtWebEmailId, "");
					}
				}
				else
				{
					_errProvider.SetError(cTool.TxtWebEmailId, "");
				}
			}
			catch { }
		}

		private bool ValidateWebAttributes()
		{
			if (!ValidateFtpAddress())
			{
				return false;
			}
			if (!ValidateWebUrl())
			{
				return false;
			}
			if (!ValidateSqlUsername())
			{
				return false;
			}
			if (!ValidateSqlPassword())
			{
				return false;
			}
			if (!ValidateWebAdminUsrNme())
			{
				return false;
			}
			if (!ValidateWebAdminPwd())
			{
				return false;
			}
			if (!ValidateFtpUsername())
			{
				return false;
			}
			if (!ValidateFtpPassword())
			{
				return false;
			}
			if (!ValidateWebEmailId())
			{
				return false;
			}

			return true;
		}

		private bool ValidateFtpAddress()
		{
			bool result = true;
			if (cTool.TxtFtpAddress.Text.Trim() != string.Empty)
			{
				result = Regex.IsMatch(cTool.TxtFtpAddress.Text, @"(((ftp|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?");
				if (!result)
				{
					cTool.TxtFtpAddress.Focus();
				}
			}
			return result;
		}

		private bool ValidateWebUrl()
		{
			bool result = true;
			if (cTool.TxtWebUrl.Text.Trim() != string.Empty)
			{
				result = Regex.IsMatch(cTool.TxtWebUrl.Text, @"^(http|https)://?");
				if (!result)
				{
					cTool.TxtWebUrl.Focus();
				}
			}
			return result;
		}

		private bool ValidateSqlUsername()
		{
			bool result = true;
			if (cTool.TxtSqlUsername.Text.Trim() != string.Empty)
			{
				int textLength = cTool.TxtSqlUsername.Text.Length;
				if (textLength <= 5)
				{
					cTool.TxtSqlUsername.Focus();
					result = false;
				}
			}
			return result;
		}

		private bool ValidateSqlPassword()
		{
			bool result = true;
			if (cTool.TxtSqlPassword.Text.Trim() != string.Empty)
			{
				int textLength = cTool.TxtSqlPassword.Text.Length;
				if (textLength <= 8)
				{
					cTool.TxtSqlPassword.Focus();
					result = false;
				}
			}
			return result;
		}

		private bool ValidateWebAdminUsrNme()
		{
			bool result = true;
			if (cTool.TxtWebAdminUsrNme.Text.Trim() != string.Empty)
			{
				int textLength = cTool.TxtWebAdminUsrNme.Text.Length;
				if (textLength <= 5)
				{
					cTool.TxtWebAdminUsrNme.Focus();
					result = false;
				}
			}
			return result;
		}

		private bool ValidateWebAdminPwd()
		{
			bool result = true;
			if (cTool.TxtWebAdminPwd.Text.Trim() != string.Empty)
			{
				int textLength = cTool.TxtWebAdminPwd.Text.Length;
				if (textLength <= 8)
				{
					cTool.TxtWebAdminPwd.Focus();
					result = false;
				}
			}
			return result;
		}

		private bool ValidateFtpUsername()
		{
			bool result = true;
			if (cTool.TxtFtpUsername.Text.Trim() != string.Empty)
			{
				int textLength = cTool.TxtFtpUsername.Text.Length;
				if (textLength <= 5)
				{
					cTool.TxtFtpUsername.Focus();
					result = false;
				}
			}
			return result;
		}

		private bool ValidateFtpPassword()
		{
			bool result = true;
			if (cTool.TxtFtpPassword.Text.Trim() != string.Empty)
			{
				int textLength = cTool.TxtFtpPassword.Text.Length;
				if (textLength <= 8)
				{
					cTool.TxtFtpPassword.Focus();
					result = false;
				}
			}
			return result;
		}

		private bool ValidateWebEmailId()
		{
			bool result = true;
			if (cTool.TxtWebEmailId.Text.Trim() != string.Empty)
			{
				bool isEmail = Regex.IsMatch(cTool.TxtWebEmailId.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
				if (!isEmail)
				{
					cTool.TxtWebEmailId.Focus();
					result = false;
				}
			}
			return result;
		}

		/// <summary>
		/// Validate whether the given value is not < 6.0 and not > 28.0
		/// Valid Numbers: 7, 8.5 etc
		/// </summary>
		/// <param name="fSize">Double</param>
		/// <returns>True/False</returns>

		private bool ValidateEPubFontSize(Double fSize)
		{
			bool result = false;

			if (fSize >= 6.0 && fSize <= 28.0)
			{
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		#endregion
	}
}
