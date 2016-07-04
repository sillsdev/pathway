// --------------------------------------------------------------------------------------------
// <copyright file="Common.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (c) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
//
// <remarks>
// Library for Pathway
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Xsl;
using L10NSharp;
using Microsoft.Win32;
using Palaso.WritingSystems;
using Palaso.Xml;
using SIL.Tool.Localization;
using System.Reflection;
using Test;
using SIL.PublishingSolution;

#endregion Using

namespace SIL.Tool
{
	/// <summary>
	/// Common Library for Dictionary Express
	/// </summary>
	public static partial class Common
	{
		#region Public variable

		public enum OutputType
		{
			ODT,
			ODM,
			IDML,
			PDF,
			MOBILE,
			EPUB,
			XETEX,
			XELATEX
		};

		public static string SamplePath = string.Empty;
		public static List<string> BookNameCollection = new List<string>();
		public static string BookNameTag = string.Empty;

		public static string SepParent = "_";
		public static string SepAncestor = ".-";
		public static string sepPrecede = "-";
		public static string SepAttrib = "_.";
		public static string SepTag = ".";
		public static string SepPseudo = "..";
		public static string Space = " ";
		/* Non Breaking Space. It has differerence from normal space and the non breaking space */
		public static string NonBreakingSpace = Common.ConvertUnicodeToString("\\00a0");

		private static readonly ArrayList _units = new ArrayList();
		public static ErrorProvider _errProvider = new ErrorProvider();
		public static Font UIFont;
		public static OdtType OdType;
		public static double ColumnWidth = 0.0;
		public static string errorMessage = string.Empty;
		public static string databaseName = string.Empty;
		public static DateTime TimeStarted { get; set; }
		public static OutputType _outputType = OutputType.ODT;
		public static Dictionary<string, string> TempVariable = new Dictionary<string, string>();
		public const string kCompany = "SIL";
		public const string kProduct = "Pathway";
		/// <summary>
		/// The email address people should write to with problems (or new localizations?) for Pathway.
		/// </summary>
		public static string IssuesEmailAddress
		{
			get { return "pathway@sil.org"; }
		}
		public enum FileType
		{
			Directory,
			DirectoryExcluded,
			File,
			FileExcluded,
			Project
		}

		public enum OdtType
		{
			OdtChild,
			OdtMaster,
			OdtNoMaster
		}

		public enum ProjectType
		{
			Dictionary,
			Scripture
		}

		/// <summary>
		/// Returns whether this program is running under the mono VM environment. 
		/// ONLY USE THIS IF YOU ABSOLUTELY NEED CONDITIONAL CODE. 
		/// </summary>
		public static bool UsingMonoVM
		{
			get
			{
				Type t = Type.GetType("Mono.Runtime");
				return (t != null);
			}
		}

		#region LanguageCodeAndName()

		/// <summary>
		/// This method returns Language Code and Name Collection as Dictionary
		/// </summary>
		/// <returns> Language Code and Name Collection as Dictionary</returns>
		public static Dictionary<string, string> LanguageCodeAndName()
		{
			Dictionary<string, string> _myLanguageCodeAndName = new Dictionary<string, string>();
			_myLanguageCodeAndName.Add("AA", "Afar");
			_myLanguageCodeAndName.Add("AB", "Abkhazian");
			_myLanguageCodeAndName.Add("AF", "Afrikaans");
			_myLanguageCodeAndName.Add("AM", "Amharic");
			_myLanguageCodeAndName.Add("AR", "Arabic");
			_myLanguageCodeAndName.Add("AS", "Assamese");
			_myLanguageCodeAndName.Add("AY", "Aymara");
			_myLanguageCodeAndName.Add("AZ", "Azerbaijani");
			_myLanguageCodeAndName.Add("BA", "Bashkir");
			_myLanguageCodeAndName.Add("BE", "Byelorussian");
			_myLanguageCodeAndName.Add("BG", "Bulgarian");
			_myLanguageCodeAndName.Add("BH", "Bihari");
			_myLanguageCodeAndName.Add("BN", "Bengali");
			_myLanguageCodeAndName.Add("BO", "Tibetan");
			_myLanguageCodeAndName.Add("BR", "Breton");
			_myLanguageCodeAndName.Add("CA", "Catalan");
			_myLanguageCodeAndName.Add("CO", "Corsican");
			_myLanguageCodeAndName.Add("CS", "Czech");
			_myLanguageCodeAndName.Add("CY", "Welsh");
			_myLanguageCodeAndName.Add("DA", "Danish");
			_myLanguageCodeAndName.Add("DE", "German");
			_myLanguageCodeAndName.Add("DZ", "Bhutani");
			_myLanguageCodeAndName.Add("EL", "Greek");
			_myLanguageCodeAndName.Add("EN", "English");
			_myLanguageCodeAndName.Add("EO", "Esperanto");
			_myLanguageCodeAndName.Add("ES", "Spanish");
			_myLanguageCodeAndName.Add("ET", "Estonian");
			_myLanguageCodeAndName.Add("EU", "Basque");
			_myLanguageCodeAndName.Add("FA", "Persian");
			_myLanguageCodeAndName.Add("FI", "Finnish");
			_myLanguageCodeAndName.Add("FJ", "Fiji");
			_myLanguageCodeAndName.Add("FO", "Faeroese");
			_myLanguageCodeAndName.Add("FR", "French");
			_myLanguageCodeAndName.Add("FY", "Frisian");
			_myLanguageCodeAndName.Add("GA", "Irish");
			_myLanguageCodeAndName.Add("GD", "Gaelic");
			_myLanguageCodeAndName.Add("GL", "Galician");
			_myLanguageCodeAndName.Add("GN", "Guarani");
			_myLanguageCodeAndName.Add("GU", "Gujarati");
			_myLanguageCodeAndName.Add("HA", "Hausa");
			_myLanguageCodeAndName.Add("HI", "Hindi");
			_myLanguageCodeAndName.Add("HR", "Croatian");
			_myLanguageCodeAndName.Add("HU", "Hungarian");
			_myLanguageCodeAndName.Add("HY", "Armenian");
			_myLanguageCodeAndName.Add("IA", "Interlingua");
			_myLanguageCodeAndName.Add("IE", "Interlingue");
			_myLanguageCodeAndName.Add("IK", "Inupiak");
			_myLanguageCodeAndName.Add("IN", "Indonesian");
			_myLanguageCodeAndName.Add("IS", "Icelandic");
			_myLanguageCodeAndName.Add("IT", "Italian");
			_myLanguageCodeAndName.Add("IW", "Hebrew");
			_myLanguageCodeAndName.Add("JA", "Japanese");
			_myLanguageCodeAndName.Add("JI", "Yiddish");
			_myLanguageCodeAndName.Add("JW", "Javanese");
			_myLanguageCodeAndName.Add("KA", "Georgian");
			_myLanguageCodeAndName.Add("KK", "Kazakh");
			_myLanguageCodeAndName.Add("KL", "Greenlandic");
			_myLanguageCodeAndName.Add("KM", "Cambodian");
			_myLanguageCodeAndName.Add("KN", "Kannada");
			_myLanguageCodeAndName.Add("KO", "Korean");
			_myLanguageCodeAndName.Add("KS", "Kashmiri");
			_myLanguageCodeAndName.Add("KU", "Kurdish");
			_myLanguageCodeAndName.Add("KY", "Kirghiz");
			_myLanguageCodeAndName.Add("LA", "Latin");
			_myLanguageCodeAndName.Add("LN", "Lingala");
			_myLanguageCodeAndName.Add("LO", "Laothian");
			_myLanguageCodeAndName.Add("LT", "Lithuanian");
			_myLanguageCodeAndName.Add("LV", "Latvian");
			_myLanguageCodeAndName.Add("MG", "Malagasy");
			_myLanguageCodeAndName.Add("MI", "Maori");
			_myLanguageCodeAndName.Add("MK", "Macedonian");
			_myLanguageCodeAndName.Add("ML", "Malayalam");
			_myLanguageCodeAndName.Add("MN", "Mongolian");
			_myLanguageCodeAndName.Add("MO", "Moldavian");
			_myLanguageCodeAndName.Add("MR", "Marathi");
			_myLanguageCodeAndName.Add("MS", "Malay");
			_myLanguageCodeAndName.Add("MT", "Maltese");
			_myLanguageCodeAndName.Add("MY", "Burmese");
			_myLanguageCodeAndName.Add("NA", "Nauru");
			_myLanguageCodeAndName.Add("NE", "Nepali");
			_myLanguageCodeAndName.Add("NL", "Dutch");
			_myLanguageCodeAndName.Add("NO", "Norwegian");
			_myLanguageCodeAndName.Add("OC", "Occitan");
			_myLanguageCodeAndName.Add("OM", "Oromo");
			_myLanguageCodeAndName.Add("OR", "Oriya");
			_myLanguageCodeAndName.Add("PA", "Punjabi");
			_myLanguageCodeAndName.Add("PL", "Polish");
			_myLanguageCodeAndName.Add("PS", "Pashto");
			_myLanguageCodeAndName.Add("PT", "Portuguese");
			_myLanguageCodeAndName.Add("QU", "Quechua");
			_myLanguageCodeAndName.Add("RM", "Rhaeto-Romance");
			_myLanguageCodeAndName.Add("RN", "Kirundi");
			_myLanguageCodeAndName.Add("RO", "Romanian");
			_myLanguageCodeAndName.Add("RU", "Russian");
			_myLanguageCodeAndName.Add("RW", "Kinyarwanda");
			_myLanguageCodeAndName.Add("SA", "Sanskrit");
			_myLanguageCodeAndName.Add("SD", "Sindhi");
			_myLanguageCodeAndName.Add("SG", "Sangro");
			_myLanguageCodeAndName.Add("SH", "Serbo-Croatian");
			_myLanguageCodeAndName.Add("SI", "Singhalese");
			_myLanguageCodeAndName.Add("SK", "Slovak");
			_myLanguageCodeAndName.Add("SL", "Slovenian");
			_myLanguageCodeAndName.Add("SM", "Samoan");
			_myLanguageCodeAndName.Add("SN", "Shona");
			_myLanguageCodeAndName.Add("SO", "Somali");
			_myLanguageCodeAndName.Add("SQ", "Albanian");
			_myLanguageCodeAndName.Add("SR", "Serbian");
			_myLanguageCodeAndName.Add("SS", "Siswati");
			_myLanguageCodeAndName.Add("ST", "Sesotho");
			_myLanguageCodeAndName.Add("SU", "Sudanese");
			_myLanguageCodeAndName.Add("SV", "Swedish");
			_myLanguageCodeAndName.Add("SW", "Swahili");
			_myLanguageCodeAndName.Add("TA", "Tamil");
			_myLanguageCodeAndName.Add("TE", "Tegulu");
			_myLanguageCodeAndName.Add("TG", "Tajik");
			_myLanguageCodeAndName.Add("TH", "Thai");
			_myLanguageCodeAndName.Add("UK", "Ukrainian");
			_myLanguageCodeAndName.Add("UR", "Urdu");
			_myLanguageCodeAndName.Add("UZ", "Uzbek");
			_myLanguageCodeAndName.Add("VI", "Vietnamese");
			_myLanguageCodeAndName.Add("VO", "Volapuk");
			_myLanguageCodeAndName.Add("WO", "Wolof");
			_myLanguageCodeAndName.Add("XH", "Xhosa");
			_myLanguageCodeAndName.Add("YO", "Yoruba");
			_myLanguageCodeAndName.Add("ZH", "Chinese");
			_myLanguageCodeAndName.Add("ZU", "Zulu");

			return _myLanguageCodeAndName;
		}

		#endregion

		public static bool Testing; // To differentiate between Nunit test or from Application(UI or Flex).
		public static bool ShowMessage; // Show or Suppress MessageBox in Creating Zip Folder.

		#endregion

		#region AlwaysPreProcessingXsltFiles

		public static List<string> PreProcessingXsltFilesList()
		{
			List<string> lstFileName = new List<string>();
			lstFileName.Add("Add Columns FW83");
			lstFileName.Add("Letter Header Language");
			return lstFileName;
		}

		#endregion

		#region FillName(string cssFileWithPath)

		/// -------------------------------------------------------------------------------------------
		/// <summary>
		/// This method collects css files names into ArrayList based on base CSS File.
		/// </summary>
		/// <param name="cssFileWithPath">Its gets the file path of the CSS File</param>
		/// <param name="baseCssFileWithPath">prevents the base from being added multiple times</param>
		/// <returns>ArrayList contains CSS filenames which are used</returns>
		/// -------------------------------------------------------------------------------------------
		public static ArrayList GetCSSFileNames(string cssFileWithPath, string baseCssFileWithPath)
		{
			ArrayList arrayCSSFile = new ArrayList();
			if (!File.Exists(cssFileWithPath))
			{
				return arrayCSSFile;
			}
			string cssPath = Path.GetDirectoryName(cssFileWithPath);
			string strText;
			var fs = new FileStream(cssFileWithPath, FileMode.Open, FileAccess.Read);
			var sr = new StreamReader(fs);
			try
			{
				if (baseCssFileWithPath != cssFileWithPath)
				{
					arrayCSSFile.Add(cssFileWithPath);
				}
				while ((strText = sr.ReadLine()) != null)
				{
					if (strText.Contains("@import"))
					{
						string cssFile = strText.Substring((strText.IndexOf('"') + 1),
														   strText.LastIndexOf('"') - (strText.IndexOf('"') + 1));

						string executablePath = Path.GetDirectoryName(Application.ExecutablePath);

						if (executablePath.Contains("ReSharper") || executablePath.Contains("NUnit"))
						{
							//This code will work when this method call from NUnit Test case
						    executablePath = PathPart.Bin(Environment.CurrentDirectory, "/ConfigurationTool/TestFiles/input");
						}
						else if (executablePath.ToLower().Contains("fieldworks") ||
								 executablePath.ToLower().Contains("configurationtool") ||
								 executablePath.ToLower().Contains("testbed"))
						{
							executablePath = Common.GetPSApplicationPath();
							if (SamplePath == String.Empty)
							{
								SamplePath = "Styles//Dictionary";
							}
						}
						else if (executablePath.ToLower().Contains("paratext"))
						{
							executablePath = Common.GetPSApplicationPath();
							if (SamplePath == String.Empty)
							{
								SamplePath = "Styles//Scripture";
							}
						}

						if (!File.Exists(PathCombine(cssPath, cssFile)) && SamplePath.Length > 0)
						{
							cssPath = PathCombine(executablePath, SamplePath);
						}
						arrayCSSFile.AddRange(GetCSSFileNames(PathCombine(cssPath, cssFile), baseCssFileWithPath));
					}
				}
			}
			finally
			{
				sr.Close();
				fs.Close();
			}
			return arrayCSSFile;
		}

		#endregion

		#region GetTextDirection(string languageCode)

		public static string TextDirectionLanguageFile = null; //Set during testing

		/// <summary>
		/// Looks up the text direction for the specified language code in the appropriate .ldml file.
		/// This lookup will not work with Paratext, which does not yet use an .ldml file.
		/// </summary>
		/// <param name="language">ISO 639 language code.</param>
		/// <returns>Text direction (ltr or rtl), or ltr if not found.</returns>
		public static string GetTextDirection(string language)
		{
			string[] langCoun = language.Split('-');
			string direction = "ltr";
			try
			{
				if (AppDomain.CurrentDomain.FriendlyName.ToLower() == "paratext.exe" ||
					TextDirectionLanguageFile != null) // is paratext
				{
					string fileName = TextDirectionLanguageFile;
					if (fileName == null)
					{
						SettingsHelper settingsHelper = new SettingsHelper(Param.DatabaseName);
						fileName = settingsHelper.GetLanguageFilename();
					}
					foreach (string line in FileData.Get(fileName).Split(new[] { '\n' }))
					{
						if (line.StartsWith("RTL="))
						{
							direction = (line[4] == 'T') ? "rtl" : "ltr";
							break;
						}
					}
				}
				else
				{
					string wsPath;
					if (langCoun.Length < 2)
					{
						// try the language (no country code) (e.g, "en" for "en-US")
						wsPath = PathCombine(Common.GetLDMLPath(), langCoun[0] + ".ldml");
					}
					else
					{
						// try the whole language expression (e.g., "ggo-Telu-IN")
						wsPath = PathCombine(Common.GetLDMLPath(), language + ".ldml");
					}
					if (File.Exists(wsPath))
					{
						XmlDocument ldml = DeclareXMLDocument(false);
						ldml.Load(wsPath);
						var nsmgr = new XmlNamespaceManager(ldml.NameTable);
						nsmgr.AddNamespace("palaso", "urn://palaso.org/ldmlExtensions/v1");
						var node = ldml.SelectSingleNode("//orientation/@characters", nsmgr);
						if (node != null)
						{
							// get the text direction specified by the .ldml file
							direction = (node.Value.ToLower().Equals("right-to-left")) ? "rtl" : "ltr";
						}
					}
				}
			}
			catch
			{
				direction = "ltr";
			}
			return direction;
		}

		#endregion

		#region GetLanguageName(string languageCode)

		/// <summary>
		/// This method returns Language Name for particular Language Code
		/// </summary>
		/// <param name="languageCode">Language Code</param>
		/// <returns>Language Name</returns>
		public static string GetLanguageName(string languageCode)
		{
			// longhand way - go poking in the .ldml file
			if (string.IsNullOrEmpty(languageCode)) return string.Empty;
			string _languageName = string.Empty;
			Dictionary<string, string> LanguageCollection = LanguageCodeAndName();
			if (LanguageCollection.ContainsKey(languageCode.ToUpper()))
			{
				_languageName = LanguageCollection[languageCode.ToUpper()];
			}
			if (string.IsNullOrEmpty(_languageName))
			{
				// 2-digit lookup didn't return anything - look at the <palaso:languageName> element
				// in the .ldml file
				string[] langCoun = languageCode.Split('-');

				try
				{
					string wsPath;
					if (langCoun.Length < 2)
					{
						// try the language (no country code) (e.g, "en" for "en-US")
						wsPath = PathCombine(Common.GetLDMLPath(), langCoun[0] + ".ldml");
					}
					else
					{
						// try the whole language expression (e.g., "ggo-Telu-IN")
						wsPath = PathCombine(Common.GetLDMLPath(), languageCode + ".ldml");
					}
					if (File.Exists(wsPath))
					{
						XmlDocument ldml = DeclareXMLDocument(false);
						ldml.Load(wsPath);
						var nsmgr = new XmlNamespaceManager(ldml.NameTable);
						nsmgr.AddNamespace("palaso", "urn://palaso.org/ldmlExtensions/v1");
						var node = ldml.SelectSingleNode("//palaso:languageName/@value", nsmgr);
						if (node != null)
						{
							// found it! Set the return value
							_languageName = node.Value;
						}
					}
					else
					{
						// Paratext case (no .ldml file) - return a null string
						return string.Empty;
					}
				}
				catch (Exception e)
				{
					Debug.WriteLine(e);
				}

			}
			return _languageName;
		}

		#endregion

		#region LeftString(string fullString, string splitString)

		/// <summary>
		/// Example: LeftString("Entry_Letdata", "_Letdata") returns "Entry"
		/// </summary>
		/// <param name="fullString">Full String</param>
		/// <param name="splitString">Split String</param>
		/// <returns>Left portion of the Text</returns>
		public static string LeftString(string fullString, string splitString)
		{
			fullString = string.IsNullOrEmpty(fullString) ? string.Empty : fullString;
			splitString = string.IsNullOrEmpty(splitString) ? string.Empty : splitString;
			string result = fullString;

			if (fullString != string.Empty && splitString != string.Empty)
			{
				int startPos = fullString.IndexOf(splitString);
				result = startPos == -1 ? fullString : fullString.Substring(0, startPos);
			}
			return result;
		}

		#endregion

		#region ReplaceCSSClassName(string cssClassName)

		/// <summary>
		/// Example: ReplaceCSSClassName("Entry_string1") returns "Entry_stringb"
		/// </summary>
		/// <param name="cssClassName">CSS Class Name</param>
		/// <returns>Replace CSS Class Name</returns>
		public static string ReplaceCSSClassName(string cssClassName)
		{
			string result = string.Empty;
			foreach (char changeToChar in cssClassName)
			{
				char c = ' ';
				if (char.IsNumber(changeToChar))
				{
					int charValue = Convert.ToInt32(changeToChar) + 49;
					c = (char)charValue;
				}
				else
				{
					c = changeToChar;
				}
				result = result + c.ToString();
			}
			return result;
		}

		#endregion

		#region PublishingSolutionsEnvironmentReset()

		/// <summary>
		/// Remove all files saved in All Users\AppData, 
		/// (Executing this method when these files are expected to be present may cause a crash).
		/// Intended for use during testing.
		/// </summary>
		public static void PublishingSolutionsEnvironmentReset()
		{
			string appDataDir = Common.GetAllUserPath();
			DeleteDirectory(appDataDir);
		}

		#endregion

		#region CheckAndGetStyle

		/// <summary>
		/// Checks for tags and writes the styles to a dictionary variable
		/// </summary>
		/// <param name="xhtmlFileName">XHTML File Name</param>
		/// <param name="projectInputType">Project Input Type</param>
		public static void CheckAndGetStyle(string xhtmlFileName, string projectInputType)
		{
			if (projectInputType.ToLower() == "dictionary")
			{
				if (!File.Exists(xhtmlFileName))
					return;

				XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
				var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
				namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
				var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, ProhibitDtd = false };
				string styleName = string.Empty;
				using (XmlReader xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings))
				{
					while (xmlReader.Read())
					{
						if (xmlReader.IsStartElement() && xmlReader.Name == "div" && xmlReader["class"] == "letHead")
						{
							if (xmlReader.Read())
							{
								if (xmlReader.IsStartElement() && xmlReader.Name == "div" && xmlReader["class"] == "letter")
								{
									styleName = "letter_letHead_dicBody";
									break;
								}
								else if (xmlReader.IsStartElement() && xmlReader.Name == "span" && xmlReader["class"] == "letter")
								{
									styleName = "letHead_dicBody";
									break;
								}
							}
						}
					}
				}
				if (TempVariable.ContainsKey("TOCStyleName"))
				{
					TempVariable["TOCStyleName"] = styleName;
				}
				else
				{
					TempVariable.Add("TOCStyleName", styleName);
				}
			}
		}

		#endregion

		#region RightString(string fullString, string splitString)

		/// <summary>
		/// Example: RightString("Entry_Letdata_Letdata", "_") ==> "Letdata_Letdata"
		/// </summary>
		/// <param name="fullString">Full String</param>
		/// <param name="splitString">Split String</param>
		/// <returns>Right portion of the Text</returns>
		public static string RightString(string fullString, string splitString)
		{
			fullString = string.IsNullOrEmpty(fullString) ? string.Empty : fullString;
			splitString = string.IsNullOrEmpty(splitString) ? string.Empty : splitString;
			string result = fullString;

			if (fullString != string.Empty && splitString != string.Empty)
			{
				int startPos = fullString.IndexOf(splitString);
				result = (startPos == -1 || startPos == 0) ? fullString : fullString.Substring(startPos + 1);
			}
			return result;
		}

		/// <summary>
		/// To replace the symbol to text
		/// </summary>
		/// <param name="value">input symbol</param>
		/// <returns>Replaced text</returns>
		public static string ReplaceSymbolToText(string value)
		{
			if (value.IndexOf("&") >= 0)
			{
				value = value.Replace("&", "&amp;");
			}
			if (value.IndexOf("<") >= 0)
			{
				value = value.Replace("<", "&lt;");
			}
			if (value.IndexOf(">") >= 0)
			{
				value = value.Replace(">", "&gt;");
			}
			if (value.IndexOf("\"") >= 0)
			{
				value = value.Replace("\"", "&quot;");
			}
			if (value.IndexOf("'") >= 0)
			{
				value = value.Replace("'", "&apos;");
			}
			return value;
		}

		/// <summary>
		/// Returns the "main" language code in use by the document.
		/// </summary>
		/// <returns></returns>
		public static string GetLanguageCode(string xhtmlFileNameWithPath, string projectInputType)
		{
			return GetLanguageCode(xhtmlFileNameWithPath, projectInputType, false);
		}

		public static string GetLanguageCode(string xhtmlFileNameWithPath, string projectInputType, bool vernagular)
		{
			string languageCode = string.Empty;
			if (File.Exists(xhtmlFileNameWithPath))
			{
				if (Path.GetFileName(xhtmlFileNameWithPath).IndexOf("Preserve") == 0)
				{
					xhtmlFileNameWithPath = xhtmlFileNameWithPath.Replace(Path.GetFileName(xhtmlFileNameWithPath),
						Path.GetFileName(xhtmlFileNameWithPath)
							.Replace(
								"Preserve", ""));
				}
				// XMLReader used to get the Meta Elements and then if Vernacular, we get the Vernacular Language by
				// checking for the xpaths - "//*[@class='headword']/@*[local-name()='lang']", "//*[@class='headword']/*/@lang"
				// 1) @class can be equal to headword, mainheadword, headref, Paragraph  2) lang can also be as xml:lang
				List<string> langCodeList = new List<string>();
				var vernacularLang = string.Empty;
				var metaList = new List<KeyValuePair<string, string>>();
				var xmlReaderSettings = new XmlReaderSettings {XmlResolver = null, ProhibitDtd = false};
				string attribute = string.Empty;
				using (XmlReader xmlReader = XmlReader.Create(xhtmlFileNameWithPath, xmlReaderSettings))
				{
					while (xmlReader.Read())
					{
						if (xmlReader.IsStartElement())
						{
							if (xmlReader.Name == "meta")
							{
								if (xmlReader["name"] != null && xmlReader["content"] != null)
								{
									var aMetaItem = new KeyValuePair<string, string>(xmlReader["name"], xmlReader["content"]);
									metaList.Add(aMetaItem);
								}
								continue;
							}

							if (vernagular)
							{
								attribute = xmlReader["class"];
								if (attribute == "headword" || attribute == "mainheadword" || attribute == "headref" || attribute == "Paragraph")
								{
									attribute = xmlReader["lang"];
									if (attribute == null)
									{
										attribute = xmlReader["xml:lang"];
										if (attribute == null)
										{
											if (xmlReader.Read())
											{
												if (xmlReader.IsStartElement())
												{
													attribute = xmlReader["lang"];
													if (attribute != null)
													{
														vernacularLang = attribute;
														break;
													}
													else
													{
														attribute = xmlReader["xml:lang"];
														if (attribute != null)
														{
															vernacularLang = attribute;
															break;
														}
													}
												}
											}
										}
										else
										{
											vernacularLang = attribute;
											break;
										}
									}
									else
									{
										vernacularLang = attribute;
										break;
									}
								}
							}
						}
					}
				}

				int metaCount = 0;
				foreach (var metaItem in metaList)
				{
					string langName = metaItem.Key;
					string langContent = metaItem.Value;

					if (langName.ToLower() == "dc.language")
					{
						if (vernagular)
						{
							if (langContent.Length < vernacularLang.Length ||
								langContent.Substring(0, vernacularLang.Length) != vernacularLang)
							{
								continue;
							}
							if (langContent.Length >= vernacularLang.Length &&
								langContent.Substring(vernacularLang.Length, 1) != ":")
							{
								continue;
							}
						}

						if (!langCodeList.Contains(langContent))
						{
							langCodeList.Add(langContent);
							languageCode = languageCode + langContent;
							if (metaCount < metaList.Count - 1)
							{
								languageCode = languageCode + ";";
							}
						}
					}
					metaCount++;
				}
			}
			else
			{
				languageCode = "eng";
			}
			return languageCode;
		}

		/// <summary>
		/// Returns languageCode used in dictionary
		/// </summary>
		/// <returns></returns>
		public static string GetLanguageCodeList(string xhtmlFileNameWithPath, string projectInputType)
		{
			if (Path.GetFileName(xhtmlFileNameWithPath).IndexOf("Preserve") == 0)
			{
				xhtmlFileNameWithPath = xhtmlFileNameWithPath.Replace(Path.GetFileName(xhtmlFileNameWithPath),
																	  Path.GetFileName(xhtmlFileNameWithPath).Replace(
																		  "Preserve", ""));
			}

			string languageCode = string.Empty;
			List<string> langCodeList = new List<string>();
			XmlDocument xDoc = Common.DeclareXMLDocument(true);
			XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
			namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			xDoc.Load(xhtmlFileNameWithPath);
			XmlNodeList fontList = xDoc.GetElementsByTagName("meta");
			for (int i = 0; i < fontList.Count; i++)
			{
				string langName = fontList[i].Attributes["name"].Value;
				string langContent = fontList[i].Attributes["content"].Value;

				if (langName.ToLower().IndexOf("dc.language") != 0)
					continue;

				if (langContent.IndexOf(':') > 0)
				{
					if (!langCodeList.Contains(langContent))
					{
						langCodeList.Add(langContent);
						languageCode = languageCode + langContent;
						if (i < fontList.Count - 1)
						{
							languageCode = languageCode + ";";
						}
					}

				}
			}
			return languageCode;
		}

		public static List<string> GetInputBooks(string xhtmlFileNameWithPath)
		{
			List<string> books = new List<string>();
			if (Path.GetFileName(xhtmlFileNameWithPath).IndexOf("Preserve") == 0)
			{
				xhtmlFileNameWithPath = xhtmlFileNameWithPath.Replace(Path.GetFileName(xhtmlFileNameWithPath),
																	  Path.GetFileName(xhtmlFileNameWithPath).Replace(
																		  "Preserve", ""));
			}
			XmlDocument xDoc = Common.DeclareXMLDocument(true);
			XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
			namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			xDoc.Load(xhtmlFileNameWithPath);
			string xPath = "//xhtml:span[@class='scrBookCode']|//span[@class='scrBookCode']";
			XmlNodeList bookList = xDoc.SelectNodes(xPath, namespaceManager);
			if (bookList != null && bookList.Count > 0)
			{
				foreach (XmlNode book in bookList)
				{
					books.Add(book.InnerText);
				}
			}
			return books;
		}

		public static string GetFontList(string xhtmlFileNameWithPath, string projectInputType, string finalOutput)
		{
			if (Path.GetFileName(xhtmlFileNameWithPath).IndexOf("Preserve") == 0)
			{
				xhtmlFileNameWithPath = xhtmlFileNameWithPath.Replace(Path.GetFileName(xhtmlFileNameWithPath),
																	  Path.GetFileName(xhtmlFileNameWithPath).Replace(
																		  "Preserve", ""));
			}
			string fontList = string.Empty;
			if (finalOutput == ".epub")
			{
				List<string> langCodeList = new List<string>();
				string[] epubFile = Directory.GetFiles(Path.GetDirectoryName(xhtmlFileNameWithPath), "*.epub");
				string epubFileName = epubFile[0];
				string extractFolder = Common.PathCombine(Path.GetDirectoryName(epubFileName), "Test");
				if (File.Exists(epubFileName))
				{
					ZipUtil.UnZipFiles(epubFileName, extractFolder, "", false);
					if (Directory.Exists(Common.PathCombine(extractFolder, "OEBPS")))
					{
						string[] ttfFiles = Directory.GetFiles(Common.PathCombine(extractFolder, "OEBPS"), "*.ttf");
						if (ttfFiles.Length > 0)
						{
							for (int i = 0; i < ttfFiles.Length; i++)
							{
								string filename = Path.GetFileNameWithoutExtension(ttfFiles[i]);
								if (filename.IndexOf('-') > 0)
								{
									filename = filename.Substring(0, filename.IndexOf('-'));
								}

								if (!langCodeList.Contains(filename))
								{
									langCodeList.Add(filename);
									fontList = fontList + filename;
									if (i < ttfFiles.Length - 1)
									{
										fontList = fontList + ";";
									}
								}
							}
						}
					}
				}

				if (Directory.Exists(extractFolder))
				{
					DirectoryInfo di = new DirectoryInfo(extractFolder);
					Common.CleanDirectory(di);
				}
			}
			else
			{
				List<string> langCodeList = new List<string>();
				XmlDocument xDoc = Common.DeclareXMLDocument(true);
				XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
				namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
				xDoc.Load(xhtmlFileNameWithPath);
				XmlNodeList fontNodeList = xDoc.GetElementsByTagName("meta");
				for (int i = 0; i < fontNodeList.Count; i++)
				{
					if (fontNodeList[i].Attributes["scheme"] == null)
						continue;

					string langScheme = fontNodeList[i].Attributes["scheme"].Value;
					string langContent = fontNodeList[i].Attributes["content"].Value;

					if (langScheme.ToLower().IndexOf("language to font") != 0)
						continue;

					if (!langCodeList.Contains(langContent))
					{
						langCodeList.Add(langContent);
						fontList = fontList + langContent;
						if (i < fontNodeList.Count - 1)
						{
							fontList = fontList + ";";
						}
					}
				}
			}
			return fontList;
		}

		/// <summary>
		/// Returns language Script(font-name) used in dictionary
		/// </summary>
		/// <returns></returns>
		public static string GetLanguageScriptList(string xhtmlFileNameWithPath, string projectInputType)
		{
			if (Path.GetFileName(xhtmlFileNameWithPath).IndexOf("Preserve") == 0)
			{
				xhtmlFileNameWithPath = xhtmlFileNameWithPath.Replace(Path.GetFileName(xhtmlFileNameWithPath),
																	  Path.GetFileName(xhtmlFileNameWithPath).Replace(
																		  "Preserve", ""));
			}

			string languageCode = string.Empty;
			if (projectInputType.ToLower() == "dictionary")
			{

				List<string> langCodeList = new List<string>();
				XmlDocument xDoc = Common.DeclareXMLDocument(true);
				XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
				namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
				xDoc.Load(xhtmlFileNameWithPath);
				XmlNodeList fontList = xDoc.GetElementsByTagName("meta");
				for (int i = 0; i < fontList.Count; i++)
				{
					string langName = fontList[i].Attributes["name"].Value;
					string langContent = fontList[i].Attributes["content"].Value;

					if (langName.ToLower().IndexOf("dc.language") != 0)
						continue;

					if (langContent.IndexOf(':') > 0 && langContent.IndexOf('-') > 0)
					{
						if (!langCodeList.Contains(langContent))
						{
							langCodeList.Add(langContent);
							languageCode = languageCode + langContent;
							if (i < fontList.Count)
							{
								languageCode = languageCode + ",";
							}
						}

					}

				}
			}
			else
			{
				languageCode = Common.ParaTextEthnologueCodeName();
				if (languageCode.Length > 0 && languageCode.IndexOf('-') > 0)
				{
					string[] val = languageCode.Split('-');
					languageCode = val[1];
				}
			}
			return languageCode;
		}

		/// <summary>
		/// To replace the symbol to text
		/// </summary>
		/// <param name="value">input symbol</param>
		/// <returns>Replaced text</returns>
		public static string ReplaceSymbolToXelatexText(string value)
		{
			//Task TD-2666 (Unicode value is 2260 = ?) 
			if (value.IndexOf("2260") >= 0)
			{
				value = value.Replace("2260", "$\\neq$");
			}
			if (value.IndexOf(Common.ConvertUnicodeToString("\\2020")) >= 0)
			{
				value = value.Replace(Common.ConvertUnicodeToString("\\2020"), "$\\dagger$");
			}
			if (value.IndexOf(Common.ConvertUnicodeToString("\\2021")) >= 0)
			{
				value = value.Replace(Common.ConvertUnicodeToString("\\2021"), "$\\ddagger$");
			}
			if (value.IndexOf("201C") >= 0)
			{
				value = value.Replace("201C", "$" + Common.ConvertUnicodeToString("\\201c") + "$");
			}
			if (value.IndexOf("201D") >= 0)
			{
				value = value.Replace("201D", "$" + Common.ConvertUnicodeToString("\\201d") + "$");
			}
			if (value.IndexOf("2018") >= 0)
			{
				value = value.Replace("2018", "$" + Common.ConvertUnicodeToString("\\2018") + "$");
			}
			if (value.IndexOf("2019") >= 0)
			{
				value = value.Replace("2019", "$" + Common.ConvertUnicodeToString("\\2019") + "$");
			}
			if (value.IndexOf("#") >= 0)
			{
				value = value.Replace("#", "$\\sharp$");
			}
			if (value.IndexOf("2666") >= 0)
			{
				value = value.Replace("2666", Common.ConvertUnicodeToString("\\2666")) + " ";
			}
			if (value.IndexOf("2023") >= 0)
			{
				value = value.Replace("2023", Common.ConvertUnicodeToString("\\2665")) + " ";
			}
			if (value.IndexOf("25C6") >= 0)
			{
				value = value.Replace("25C6", Common.ConvertUnicodeToString("\\2666")) + " ";
			}
			if (value.IndexOf("25CF") >= 0 || value.IndexOf("274D") >= 0)
			{
				value = value.Replace(value, Common.ConvertUnicodeToString("\\" + value)) + " ";
			}
			return value;
		}

		#endregion

		#region RightRemove(string fullString, string splitString)

		/// <summary>
		/// Example: RightRemove("Entry_Letdata_Letdata", "_") ==> "Entry_Letdata"
		/// </summary>
		/// <param name="fullString">Full String</param>
		/// <param name="splitString">Split String</param>
		/// <returns>Remove right portion of the text</returns>
		public static string RightRemove(string fullString, string splitString)
		{
			fullString = string.IsNullOrEmpty(fullString) ? string.Empty : fullString;
			splitString = string.IsNullOrEmpty(splitString) ? string.Empty : splitString;
			string result = fullString;

			if (fullString != string.Empty && splitString != string.Empty)
			{
				int startPos = fullString.LastIndexOf(splitString);
				result = startPos == -1 ? fullString : fullString.Substring(0, startPos);
			}
			return result;
		}

		#endregion

		#region LeftRemove(string fullString, string splitString)

		/// <summary>
		/// Example: RightRemove("Entry_Letdata", "_") ==> "Letdata"
		/// </summary>
		/// <param name="fullString">Full String</param>
		/// <param name="splitString">Split String</param>
		/// <returns>Remove left portion of the text</returns>
		public static string LeftRemove(string fullString, string splitString)
		{
			fullString = string.IsNullOrEmpty(fullString) ? string.Empty : fullString;
			splitString = string.IsNullOrEmpty(splitString) ? string.Empty : splitString;
			string result = fullString;

			if (fullString != string.Empty && splitString != string.Empty)
			{
				int startPos = fullString.IndexOf(splitString) + splitString.Length;
				result = startPos == -1 ? fullString : fullString.Substring(startPos);
			}
			return result;
		}

		#endregion

		#region ValidateNumber(string number)

		/// <summary>
		/// Validate a given string value is numeric or not
		/// Valid Numbers: 2, 2.34, +34, -45.653 
		/// </summary>
		/// <param name="number">string</param>
		/// <returns>True/False</returns>

		public static bool ValidateNumber(string number)
		{
			bool result = true;

			if (string.IsNullOrEmpty(number))
			{
				result = false;
			}
			else
			{
				var rx = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
				if (!rx.IsMatch(number.Trim()))
				{
					result = false;
				}
			}
			return result;
		}

		#endregion

		#region ValidateNumber(string number)

		/// <summary>
		/// Validate a given string value is numeric or not
		/// Valid Numbers: 34 
		/// </summary>
		/// <param name="number">string</param>
		/// <returns>True/False</returns>

		public static bool ValidateInteger(string number)
		{
			bool result = true;

			if (string.IsNullOrEmpty(number))
			{
				result = false;
			}
			else
			{
				var rx = new Regex(@"^[0-9]+$");
				if (!rx.IsMatch(number.Trim()))
				{
					result = false;
				}
			}
			return result;
		}

		#endregion

		#region ValidateAlphabets(string stringValue)

		/// <summary>
		/// Validate a given string is alphabets or not 
		/// </summary>
		/// <param name="stringValue">string</param>
		/// <returns>True/False</returns>
		public static bool ValidateAlphabets(string stringValue)
		{
			bool result = true;
			if (string.IsNullOrEmpty(stringValue))
			{
				result = false;
			}
			else
			{
				var rx = new Regex(@"^[a-zA-Z]+$");
				if (!rx.IsMatch(stringValue.Trim()))
				{
					result = false;
				}
			}
			return result;
		}

		#endregion

		#region ValidateStartsWithAlphabet(string stringValue)

		/// <summary>
		/// Validate a given string is Starts with alphabets or not 
		/// </summary>
		/// <param name="stringValue">string</param>
		/// <returns>True/False</returns>
		public static bool ValidateStartsWithAlphabet(string stringValue)
		{
			bool result = !string.IsNullOrEmpty(stringValue.Trim());
			return result;
		}

		#endregion

		#region ConvertToInch(string attribute)

		/// <summary>
		/// Convert to Inch from cm / pt
		/// </summary>
		/// <param name="attribute">Value with Unit</param>
		/// <returns>Inch without unit</returns>
		public static float ConvertToInch(string attribute)
		{
			float attributeValue;
			try
			{
				int counter;
				attribute = attribute.Replace(" ", "");
				string attrib = GetNumericChar(attribute, out counter);
				attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US"));
				string attributeUnit = attribute.Substring(counter);
				if (attributeUnit == "cm")
				{
					attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US")) * 0.3937008F;
				}
				else if (attributeUnit == "pt")
				{
					attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US")) / 72F;
				}
			}
			catch
			{
				attributeValue = 0;
			}
			return attributeValue;
		}

		#endregion

		#region UnitConverter(string inputValue, string outputUnit)

		/// -------------------------------------------------------------------------------------------
		/// <summary>
		/// Unit Conversion One Unit to another Unit
		/// example UnitConverter("12px", "pt") => 8pt
		/// </summary>
		/// <param name="inputValue">User input value</param>
		/// <param name="outputUnit">Output Unit</param>
		/// <returns>Converted value From User input to output Unit</returns>
		/// -------------------------------------------------------------------------------------------
		public static float UnitConverterOO(string inputValue, string outputUnit)
		{
			float attributeValue;
			try
			{
				if (inputValue.IndexOf(outputUnit) > 0)
				{
					return float.Parse(inputValue.Replace(outputUnit, ""));
				}

				int counter;
				inputValue = inputValue.Replace(" ", "");
				string attrib = GetNumericChar(inputValue, out counter);
				attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US"));
				string attributeUnit = inputValue.Substring(counter) + "To" + outputUnit.ToLower();

				if (attributeUnit == "pcTopt")
				{
					attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US")) * 12;
				}
				else if (attributeUnit == "pxTopt")
				{
					attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US")) * 0.75F;
				}
				else if (attributeUnit == "inTopt")
				{
					attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US")) * 72F;
				}
				else if (attributeUnit == "cmTopt")
				{
					attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US")) * 28.346456693F;
				}
				else if (attributeUnit == "cmToin")
				{
					attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US")) * 0.3937008F;
				}
				else if (attributeUnit == "inTocm")
				{
					attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US")) / 0.3937008F;
				}
				else if (attributeUnit == "ptToin")
				{
					attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US")) / 72F;
				}
				else if (attributeUnit == "ptTocm")
				{
					attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US")) / 28.346456693F;
				}
				else if (attributeUnit == "pcToin")
				{
					attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US")) * 0.1666666667F;
				}

				else if (attributeUnit == "exToem")
				{
					attributeValue = float.Parse(attrib, CultureInfo.GetCultureInfo("en-US")) / 2F;
				}

			}
			catch
			{
				attributeValue = 0;
			}
			return attributeValue;
		}

		public static string UnitConverter(string inputValue, string outputUnit)
		{
			string attributeValue = string.Empty;

			if (string.IsNullOrEmpty(outputUnit)) return inputValue;

			try
			{
				if (inputValue.IndexOf(outputUnit) > 0)
				{
					return inputValue.Replace(outputUnit, "");
				}


				int counter;
				inputValue = inputValue.Replace(" ", "");
				float attrib = float.Parse(GetNumericChar(inputValue, out counter), CultureInfo.GetCultureInfo("en-US"));
				string attributeUnit = inputValue.Substring(counter) + "To" + outputUnit.ToLower();

				if (attributeUnit == "Topt")
				{
					attributeValue = attrib.ToString(CultureInfo.GetCultureInfo("en-US"));
				}
				else if (attributeUnit == "pcTopt")
				{
					attributeValue = (attrib * 12).ToString(CultureInfo.GetCultureInfo("en-US"));
				}
				else if (attributeUnit == "pxTopt")
				{
					attributeValue = (attrib * 0.75F).ToString(CultureInfo.GetCultureInfo("en-US"));
				}
				else if (attributeUnit == "inTopt")
				{
					attributeValue = (attrib * 72F).ToString(CultureInfo.GetCultureInfo("en-US"));
				}
				else if (attributeUnit == "cmTopt")
				{
					attributeValue = (attrib * 28.346456693F).ToString(CultureInfo.GetCultureInfo("en-US"));
				}
				else if (attributeUnit == "%Topt")
				{
					attributeValue = String.Format(CultureInfo.GetCultureInfo("en-US"), "{0}{1}", attrib, "%");
				}
				else if (attributeUnit == "emTopt")
				{
					attributeValue = String.Format(CultureInfo.GetCultureInfo("en-US"), "{0}{1}", (attrib * 100F), "%");
				}
				else if (attributeUnit == "cmToin")
				{
					attributeValue = (attrib * 0.3937008F).ToString(CultureInfo.GetCultureInfo("en-US"));
				}
				else if (attributeUnit == "inTocm")
				{
					attributeValue = (attrib / 0.3937008F).ToString(CultureInfo.GetCultureInfo("en-US"));
				}
				else if (attributeUnit == "ptToin")
				{
					attributeValue = (attrib / 72F).ToString(CultureInfo.GetCultureInfo("en-US"));
				}
				else if (attributeUnit == "ptTocm")
				{
					attributeValue = (attrib / 28.346456693F).ToString(CultureInfo.GetCultureInfo("en-US"));
				}
				else if (attributeUnit == "pcToin")
				{
					attributeValue = (attrib * 0.1666666667F).ToString(CultureInfo.GetCultureInfo("en-US"));
				}
				else if (attributeUnit == "ptTopc")
				{
					attributeValue = (attrib / 12).ToString(CultureInfo.GetCultureInfo("en-US"));
				}
				else if (attributeUnit == "exToem")
				{
					attributeValue = (attrib / 2F).ToString(CultureInfo.GetCultureInfo("en-US"));
				}
				else
				{
					attributeValue = string.Empty;
				}
			}
			catch
			{
			}
			return attributeValue;
		}

		public static bool IsAlpha(string input)
		{
			return Regex.IsMatch(input, "^[a-zA-Z]+$");
		}

		/// -------------------------------------------------------------------------------------------
		/// <summary>
		/// Unicode Conversion 
		/// </summary>
		/// <param name="parameter">input String</param>
		/// <returns>Unicode Character</returns>
		/// -------------------------------------------------------------------------------------------
		public static string UnicodeConversion(string parameter)
		{
			int count = 0;
			string result = string.Empty;
			try
			{
				if (parameter.Length > 0)
				{
					if (parameter.Trim() == "\"\'\"" || parameter.Trim() == "\" \'\"" || parameter.Trim() == "\"\' \""
						|| parameter.Trim() == @"'" || parameter.Trim() == @" '" || parameter.Trim() == @"' ")
					{
						return parameter.Replace("\"", "");
					}
					if (!(parameter[0] == '\"' || parameter[0] == '\''))
					{
						parameter = "'" + parameter + "'";
					}
					int strlen = parameter.Length;
					char quoteOpen = ' ';
					while (count < strlen)
					{
						// Handling Single / Double Quotes
						char c1 = parameter[count];
						Console.WriteLine(c1);
						if (parameter[count] == '\"' || parameter[count] == '\'')
						{
							if (parameter[count] == quoteOpen)
							{
								quoteOpen = ' ';
								count++;
								continue;
							}
							if (quoteOpen == ' ')
							{
								quoteOpen = parameter[count];
								count++;
								continue;
							}
						}

						if (parameter[count] == '\\')
						{
							string unicode = string.Empty;
							count++;
							int value = parameter[count];

							//if condition added to check any escape character precede with slash
							if (
								!((value > 47 && value < 58) || (value > 64 && value < 71) ||
								  (value > 96 && value < 103)))
							{
								result += parameter[count];
								count++;
								continue;
							}

							if (parameter[count] == 'u')
							{
								count++;
							}
							while (count < strlen)
							{
								value = parameter[count];
								if ((value > 47 && value < 58) || (value > 64 && value < 71) ||
									(value > 96 && value < 103))
								{
									unicode += parameter[count];
								}
								else
								{
									break;
								}
								count++;
							}
							if (_outputType == OutputType.XELATEX)
							{
								result += unicode;

							}
							else
							{
								// unicode convertion
								int decimalvalue = Convert.ToInt32(unicode, 16);
								var c = (char)decimalvalue;
								result += c.ToString();
							}
						}
						else
						{
							result += parameter[count];
							count++;
						}
					}
					if (quoteOpen != ' ')
					{
						result = "";
					}
					else
					{
						// Replace <, > and & character to &lt; &gt; &amp;
						result = result.Replace("&", "&amp;");
						result = result.Replace("<", "&lt;");
						result = result.Replace(">", "&gt;");
					}
				}
				return result;
			}
			catch (Exception)
			{
				return result;
			}
		}

		#region AssignValuePageUnit

		public static bool AssignValuePageUnit(object sender, EventArgs e)
		{
			try
			{
				//to Validate Units
				_units.Add("pt");
				_units.Add("pc");
				_units.Add("cm");
				_units.Add("in");

				var ctrl = ((Control)sender);
				string textValue = ConcateUnit(ctrl);
				ctrl.Text = textValue;
				// Page Tab
				if (ctrl.Name == "txtPageInside")
				{
					errorMessage = RangeValidate(ctrl.Text, ".25in", "1.575in");
				}
				else if (ctrl.Name == "txtPageOutside")
				{
					errorMessage = RangeValidate(ctrl.Text, ".25in", "1.575in");
				}
				else if (ctrl.Name == "txtPageTop")
				{
					errorMessage = RangeValidate(ctrl.Text, ".25in", "1.575in");
				}
				else if (ctrl.Name == "txtPageBottom")
				{
					errorMessage = RangeValidate(ctrl.Text, ".25in", "1.575in");
				}
				else if (ctrl.Name == "txtPageGutterWidth")
				{
					errorMessage = RangeValidate(ctrl.Text, "6pt", "1in");
				}

				if (errorMessage.Trim().Length != 0)
				{
					_errProvider.SetError(ctrl, errorMessage);
					return true;
				}
				_errProvider.SetError(ctrl, "");
				return false;
			}
			catch (Exception ex)
			{
				var msg = new[] { ex.Message };
				LocDB.Message("errInstlFile", ex.Message, msg, LocDB.MessageTypes.Error,
							  LocDB.MessageDefault.First);
			}
			return false;
		}

		#endregion AssignValuePageUnit

		/// <summary>
		/// To add unit value if the field only contains digits.
		/// </summary>
		/// <param name="ctrl">Control name</param>
		/// <returns>value with unit</returns>
		public static string ConcateUnit(Control ctrl)
		{
			string textValue = ctrl.Text.Trim();
			if (ctrl is TextBox && Common.ValidateNumber(textValue))
			{
				const string unit = "pt";
				int unitExist = textValue.IndexOf(unit);
				if (unitExist < 1)
				{
					textValue = textValue + unit;
				}
			}
			return textValue;
		}

		/// <summary>
		/// Function to validate Minimum and Maximum values
		/// </summary>
		/// <param name="controlValue">Control value</param>
		/// <param name="minValue">Minimum value</param>
		/// <param name="maxValue">Maximum value</param>
		/// <returns>string which contains error message</returns>
		public static string RangeValidate(string controlValue, string minValue, string maxValue)
		{
			string message = string.Empty;
			if (controlValue.Length >= 3)
			{
				string userUnit = controlValue.Substring(controlValue.Length - 2);
				if (userUnit == "in" || userUnit == "pt" || userUnit == "cm" || userUnit == "pc")
				{
					float userValue = float.Parse(controlValue.Substring(0, controlValue.Length - 2));
					float convertedMinValue = UnitConverterOO(minValue, userUnit);
					float convertedMaxValue = UnitConverterOO(maxValue, userUnit);
					if (userValue < convertedMinValue || userValue > convertedMaxValue)
					{
						message = "Enter a value between " + convertedMinValue + userUnit + " to " + convertedMaxValue +
								  userUnit + " inclusive.";
						return message;
					}
				}
				else
				{
					message = "Please enter the valid Input";
					return message;
				}
			}
			return message;
		}

		public static string UnitConverter(string inputValue)
		{
			string outputUnit = "pt";
			return UnitConverter(inputValue, outputUnit);
		}

		public static string GetNumericChar(string inputValue, out int counter)
		{
			if (string.IsNullOrEmpty(inputValue))
			{
				counter = 0;
				return string.Empty;
			}

			string attrib = string.Empty;
			for (counter = 0; counter < inputValue.Length; counter++)
			{
				char character = char.Parse(inputValue.Substring(counter, 1));
				var val = (int)character;
				if (!((val >= 48 && val <= 57) || val == 43 || val == 45 || val == 46)) // + - 0 to 9 and decimal
				{
					break;
				}
				attrib = attrib + character;
			}
			return attrib;
		}

		#endregion

		#region GetLargerSmaller(StyleAttribute parentFontsizeAttribute, string type)

		/// <summary>
		/// Calculate the font-size: larger; and font-size: smaller;
		/// </summary>
		/// <param name="parentFont">font-size</param>
		/// <param name="type">"larger"/"smaller"</param>
		/// <returns>absolute value of relavite parameter</returns>
		public static int GetLargerSmaller(float parentFont, string type)
		{
			var parentFontSize = (int)parentFont;

			int childFontSize = 0;
			if (type == "larger")
			{
				// 0pt to 23pt table value
				var numbers = new int[24]
                    {
                        0,
                        1, 2, 3, 4, 6,
                        8, 9, 10, 11, 12,
                        13, 14, 15, 16, 18,
                        20, 20, 22, 24, 24,
                        26, 27, 28
                    };
				if (parentFontSize < 0)
				{
					childFontSize = 14;
				}
				else if (parentFontSize <= 23)
				{
					childFontSize = numbers[parentFontSize];
				}
				else if (parentFontSize > 23) // 150%
				{
					childFontSize = (int)Math.Round(parentFontSize + parentFontSize / 2F);
				}
			}
			else if (type == "smaller")
			{
				// 0pt to 23pt table value
				var numbers = new int[35]
                    {
                        1,
                        1, 1, 2, 3, 4,
                        5, 6, 7, 7, 8,
                        9, 9, 11, 12, 12,
                        12, 13, 13, 14, 15,
                        16, 16, 17, 18, 18,
                        19, 20, 20, 21, 21,
                        22, 22, 23, 23
                    };
				if (parentFontSize < 0)
				{
					childFontSize = 9;
				}
				else if (parentFontSize <= 33) // from the above number[]
				{
					childFontSize = numbers[parentFontSize];
				}
				else if (parentFontSize > 34) // 66%
				{
					childFontSize = (int)Math.Round(parentFontSize * 0.66F);
				}
			}
			else
			{
				childFontSize = parentFontSize;
			}
			return (childFontSize);
		}

		#endregion

		#region GetExportType()

		#endregion

		#region GetReferenceFormat(Dictionary<string, Dictionary<string, string>> idAllClass, string refFormat)
		/// <summary>
		/// Get the Reference Format, what user selected in Configuration Tool
		/// </summary>
		/// <param name="idAllClass">All Class List</param>
		/// <param name="refFormat">Reference Format String</param>
		/// <returns></returns>
		public static string GetReferenceFormat(Dictionary<string, Dictionary<string, string>> idAllClass, string refFormat)
		{
			if (idAllClass.ContainsKey("ReferenceFormat"))
			{
				if (idAllClass["ReferenceFormat"].ContainsKey("@page:left"))
				{
					refFormat = idAllClass["ReferenceFormat"]["@page:left"];
				}
				else if (idAllClass["ReferenceFormat"].ContainsKey("@page"))
				{
					refFormat = idAllClass["ReferenceFormat"]["@page"];
				}
				else if (idAllClass["ReferenceFormat"].ContainsKey("@page:right"))
				{
					refFormat = idAllClass["ReferenceFormat"]["@page:right"];
				}
			}
			else if (idAllClass.ContainsKey("@page:left-top-left"))
			{
				refFormat = idAllClass["@page:left-top-left"]["-ps-referenceformat"];
			}
			else if (idAllClass.ContainsKey("@page-top-center"))
			{
				refFormat = idAllClass["@page-top-center"]["-ps-referenceformat"];
			}
			return refFormat;
		}
		#endregion

		public static string GetHeaderFontWeight(Dictionary<string, Dictionary<string, string>> _cssProperty) //TD-2815
		{
			string headerFontWeight = "regular";
			if (_cssProperty.ContainsKey("@page-top-center") &&
				_cssProperty["@page-top-center"].ContainsKey("font-weight"))
			{
				headerFontWeight = _cssProperty["@page-top-center"]["font-weight"];
			}
			else if (_cssProperty.ContainsKey("@page-top-right") &&
					 _cssProperty["@page-top-right"].ContainsKey("font-weight"))
			{
				headerFontWeight = _cssProperty["@page-top-right"]["font-weight"];
			}
			else if (_cssProperty.ContainsKey("@page-bottom-center") &&
					 _cssProperty["@page-bottom-center"].ContainsKey("font-weight"))
			{
				headerFontWeight = _cssProperty["@page-bottom-center"]["font-weight"];
			}
			else if (_cssProperty.ContainsKey("@page-bottom-right") &&
					 _cssProperty["@page-bottom-right"].ContainsKey("font-weight"))
			{
				headerFontWeight = _cssProperty["@page-bottom-right"]["font-weight"];
			}
			return headerFontWeight;
		}

		/// <summary>
		/// Method to set the header font-size based in the selection from the ConfigurationTool
		/// If user select font-size, it will goes as it is.
		/// If user select "Same as headword" in Dictionary, It copy the "headword" font size
		/// If user select "Same as body" in Dictionary,  It copy the "entry" font-size
		/// If user select "Same as section (\s)" in Scripture, It copy the "SectionHead" font size
		/// If user select "Same as body" in Scripture, It copy the "Paragraph" font-size
		/// </summary>
		/// <param name="_cssProperty">Css property collection</param>
		/// <param name="projectInputType">Dictionary/Scripture</param>
		/// <returns>font-size for header, by default it sets as 12</returns>
		public static string GetHeaderFontSize(Dictionary<string, Dictionary<string, string>> _cssProperty, string projectInputType) //TD-2815
		{
			const string defFontSize = "12pt";
			if (_cssProperty.ContainsKey("@page") && _cssProperty["@page"].ContainsKey("-ps-header-font-size"))
			{
				string headerFontSize = _cssProperty["@page"]["-ps-header-font-size"];
				int i;
				if (!int.TryParse(headerFontSize, out i))
				{
					string optionText = "same as headword";
					string clsName;
					if (projectInputType.ToLower() == "dictionary")
					{
						clsName = headerFontSize.ToLower() == optionText ? "headword" : "entry";
						if (_cssProperty.ContainsKey(clsName) && _cssProperty[clsName].ContainsKey("font-size"))
						{
							return _cssProperty[clsName]["font-size"] + "pt";
						}
					}
					else
					{
						optionText = "same as section (\\s)";
						clsName = headerFontSize.ToLower() == optionText ? "SectionHead" : "Paragraph";
						if (_cssProperty.ContainsKey(clsName) && _cssProperty[clsName].ContainsKey("font-size"))
						{
							return _cssProperty[clsName]["font-size"] + "pt";
						}
					}
				}
				else
				{
					return headerFontSize;
				}
			}
			return defFontSize;
		}

		#region StreamReplaceInFile(string filePath, string searchText, string replaceText)

		/// <summary>
		/// Stream-based version of ReplaceInFile. This uses a straight find/replace rather than a Regex
		/// (which only works on strings) - if you don't need a full regex, this will keep your memory consumption
		/// down.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="searchText"></param>
		/// <param name="replaceText"></param>
		/// <returns>true if an instance of the search string was found / replaced.</returns>
		public static bool StreamReplaceInFile(string filePath, string searchText, string replaceText)
		{
			if (!File.Exists(filePath)) return false;
			bool foundString = false;
			int len = searchText.Length;
			var buf = new byte[len];
			var reader = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4080, false);
			var tempName = Path.GetTempFileName();
			var writer = new FileStream(tempName, FileMode.Create);
			int next;
			while ((next = reader.ReadByte()) != -1)
			{
				byte b = (byte)next;
				if (b == searchText[0]) // first char in search text?
				{
					// yes - searchText.Length chars into a buffer and compare them
					long pos = reader.Position;
					buf[0] = b;
					if (reader.Read(buf, 1, (len - 1)) == -1)
					{
						// reached the end of file - write out what we hit and jump out of the while loop
						//                        writer.Write(new string(buf));
						continue;
					}
					string data = Encoding.UTF8.GetString(buf);
					if (String.Compare(searchText, data, true) == 0)
					{
						// found an instance of our search text - replace it with our replaceText
						foundString = true;
						Byte[] bytes = Encoding.UTF8.GetBytes(replaceText);
						writer.Write(bytes, 0, bytes.Length);
					}
					else
					{
						// not what we're looking for - just write it out
						reader.Position = pos;
						writer.WriteByte(b);
					}
				}
				else // not what we're looking for - just write it out
				{
					writer.WriteByte(b);
				}
			}
			reader.Close();
			writer.Close();
			// replace the original file with the new one
			if (foundString)
			{
				// at least one instance of the string was found - replace
				File.Copy(tempName, filePath, true);
			}
			File.Delete(tempName);
			return foundString;
		}

		#endregion

		#region GetNewFolderName(string filePath, string folderName, string UserFileName)

		/// <summary>
		/// Return the New Folder Name after checking it whether its existing.
		/// </summary>
		/// <param name="filePath">Path</param>
		/// <param name="folderName">Folder</param>
		/// <returns></returns>
		public static string GetNewFolderName(string filePath, string folderName)
		{
			int counter = 0;
			string userFileName = PathCombine(filePath, folderName);
			while (Directory.Exists(userFileName))
			{
				userFileName = PathCombine(filePath, folderName + ++counter);
			}
			return
				userFileName.Substring(userFileName.LastIndexOfAny(new char[2] { Path.DirectorySeparatorChar, ':' }) + 1);
		}

		#endregion

		#region GetNewFileName(string filePath, string file)

		/// <summary>
		/// Return the New File Name after checking it whether its existing.
		/// </summary>
		/// <param name="filePath">Path</param>
		/// <param name="fileName">File Name</param>
		/// <returns>New FileName</returns>
		public static string GetNewFileName(string filePath, string fileName)
		{
			int counter = 1;
			string file = Path.GetFileNameWithoutExtension(fileName);
			string fileExtension = Path.GetExtension(fileName);

			string preferedName = PathCombine(filePath, file + counter + fileExtension);
			while (File.Exists(preferedName))
			{
				preferedName = PathCombine(filePath, file + ++counter + fileExtension);
			}
			return preferedName;
		}

		#endregion

		#region ConvertUnicodeToStrin

		/// -------------------------------------------------------------------------------------------
		/// <summary>
		/// unicode to String Conversion 
		/// </summary>
		/// <param name="parameter">String</param>
		/// <returns>unicode Character</returns>
		/// -------------------------------------------------------------------------------------------
		public static string ConvertUnicodeToString(string parameter)
		{
			string result = string.Empty;
			if (string.IsNullOrEmpty(parameter))
			{
				return result;
			}

			int index = 0;
			if (!(parameter[0] == '\"' || parameter[0] == '\''))
			{
				parameter = "'" + parameter + "'";
			}
			int length = parameter.Length;
			char quoteOpen = ' ';
			while (index < length)
			{
				// Handling Single / Double Quotes
				char character = parameter[index];
				Console.WriteLine(character);
				if (parameter[index] == '\"' || parameter[index] == '\'')
				{
					if (parameter[index] == quoteOpen)
					{
						quoteOpen = ' ';
						index++;
						continue;
					}
					if (quoteOpen == ' ')
					{
						quoteOpen = parameter[index];
						index++;
						continue;
					}
				}

				if (parameter[index] == '\\')
				{
					string unicode = string.Empty;
					index++;
					if (parameter[index] == 'u')
					{
						index++;
					}
					while (index < length)
					{
						int value = parameter[index];
						if ((value > 47 && value < 58) || (value > 64 && value < 71) || (value > 96 && value < 103))
						{
							unicode += parameter[index];
						}
						else
						{
							break;
						}
						index++;
					}
					// unicode convertion
					int decimalValue = Convert.ToInt32(unicode, 16);
					var ch = (char)decimalValue;
					result += ch.ToString();
				}
				else
				{
					result += parameter[index];
					index++;
				}
			}
			if (quoteOpen != ' ')
			{
				result = "";
			}
			else
			{
				// Replace <, > and & character to &lt; &gt; &amp;
				result = result.Replace("&", "&amp;");
				result = result.Replace("<", "&lt;");
				result = result.Replace(">", "&gt;");

			}

			return result;
		}

		#endregion

		#region ConvertStringToUnicode

		/// <summary>
		/// To Convert the string / symbol to unicode value
		/// </summary>
		/// <param name="inputString">String value for example any unicode</param>
		/// <returns>unicode Value</returns>
		public static string ConvertStringToUnicode(string inputString)
		{
			string unicode = string.Empty;
			if (inputString != string.Empty)
			{
				TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(inputString);
				while (enumerator.MoveNext())
				{
					string textElement = enumerator.GetTextElement();
					int index = Char.ConvertToUtf32(textElement, 0);
					if (string.Format("{0:X}", index).Length == 2)
					{
						unicode = "\\" + "00" + string.Format("{0:X}", index);
					}
					else if (string.Format("{0:X}", index).Length == 3)
					{
						unicode = "\\" + "0" + string.Format("{0:X}", index);
					}
					else if (string.Format("{0:X}", index).Length == 4)
					{
						unicode = "\\" + string.Format("{0:X}", index);
					}
				}
			}
			return unicode;
		}

		#endregion

		#region isRightFieldworksVersion()

		/// <summary>
		/// Checks that version numbers of fieldworks assemblies agree with those available at compile time
		/// </summary>
		/// <returns>true if correct fieldworks assemblies are installed</returns>
		public static bool isRightFieldworksVersion()
		{
			try
			{
				foreach (string[] element in VersionElements())
				{
					string fileName = element[0];
					string version = element[1];
					string fullName = PathCombine(GetApplicationPath(), fileName);
					if (!File.Exists(fullName))
						continue;
					FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(fullName);
					// parse FieldworksVersions.txt items using en-US (current locale could parse differently)
					if (float.Parse(version.Substring(0, 3), CultureInfo.GetCultureInfo("en-US")) > 6.0)
						continue; // latest version uses reflection and no longer requires precise version agreement
					if (fileVersionInfo.FileVersion != version)
					{
						var sb = new StringBuilder();
						sb.AppendLine("isRightFieldworksVersion: file version mismatch -");
						sb.AppendLine(fullName + " / version: " + fileVersionInfo.FileVersion);
						sb.AppendLine("Expecting version: " + version);
						Debug.WriteLine(sb.ToString());
						return false;
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				return false;
			}
		}

		/// <summary>
		/// Enumerates the file and its version from the list of required compatible items
		/// </summary>
		public static IEnumerable VersionElements()
		{
			string fieldworksVersionPath = GetFieldworksVersionPath();
			string[] fieldworksVersions = FileData.Get(fieldworksVersionPath).Split(new[] { '\n' });
			foreach (string fieldworksVersion in fieldworksVersions)
			{
				if (fieldworksVersion == "") break;
				string[] element = fieldworksVersion.Trim().Split(new[] { ',' });
				yield return element;
			}
		}

		/// <summary>
		/// Full name of text file containing dlls and versions in the repository
		/// </summary>
		public static string GetFieldworksVersionPath()
		{
			return PathCombine(GetPSApplicationPath(), "FieldworksVersions.txt");
		}

		#endregion isRightFieldworksVersion()

		#region GetPSApplicationPath()

		public static string ProgInstall = string.Empty;
		public static string SupportFolder = string.Empty;

		/// <summary>
		/// Return the Local setting Path+ "SIL\Dictionary" 
		/// </summary>
		/// <returns>Dictionary Setting Path</returns>
		public static string GetPSApplicationPath()
		{
			if (ProgInstall == string.Empty)
				ProgInstall = GetApplicationPath();
			return SupportFolder == "" ? ProgInstall : PathCombine(ProgInstall, SupportFolder);
		}

		#endregion

		#region GetXmlNodeInDesignNamespace

		/// <summary>
		/// Returns XML Node in the file based on the xpath
		/// XmlNode = GetXmlNode("c:\en.xml", "\\book[id = 10]")
		/// </summary>
		/// <param name="xmlFileNameWithPath">File Name</param>
		/// <param name="xPath">Xpath for the XML Node</param>
		/// <returns></returns>
		public static XmlNode GetXmlNodeInDesignNamespace(string xmlFileNameWithPath, string xPath)
		{
			XmlDocument xmlDoc = DeclareXMLDocument(false);

			if (!File.Exists(xmlFileNameWithPath))
			{
				return null;
			}
			xmlDoc.Load(xmlFileNameWithPath);
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
			nsmgr.AddNamespace("idPkg", "http://ns.adobe.com/AdobeInDesign/idml/1.0/packaging");

			XmlElement root = xmlDoc.DocumentElement;
			if (root != null)
			{
				XmlNode returnNode = root.SelectSingleNode(xPath, nsmgr);
				return returnNode;
			}
			return null;
		}

		#endregion

		#region GetXmlNodeListInDesignNamespace

		/// <summary>
		/// Returns XMLNodeList 
		/// </summary>
		/// <param name="xmlFileNameWithPath">File Name</param>
		/// <param name="xPath">Xpath for the XML Node</param>
		/// <returns>Returns ArrayList Example: Apple, Ball</returns>
		public static XmlNodeList GetXmlNodeListInDesignNamespace(string xmlFileNameWithPath, string xPath)
		{
			XmlDocument xmlDoc = DeclareXMLDocument(false);

			XmlNodeList returnNode = null;
			if (!File.Exists(xmlFileNameWithPath))
			{
				return returnNode;
			}

			xmlDoc.Load(xmlFileNameWithPath);
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
			nsmgr.AddNamespace("idPkg", "http://ns.adobe.com/AdobeInDesign/idml/1.0/packaging");

			XmlElement root = xmlDoc.DocumentElement;
			if (root != null)
			{
				returnNode = root.SelectNodes(xPath, nsmgr);
				if (returnNode != null) return returnNode;
			}
			return returnNode;
		}

		#endregion

		#region GetLDMLPath

		public static string GetLDMLPath()
		{
			string path = string.Empty;
			object regObj;
			try
			{
				if (RegistryHelperLite.RegEntryExists(RegistryHelperLite.CompanyKeyCurrentUser,
													  "Pathway", "WritingSystemStore", out regObj))
				{
					Common.SupportFolder = "";
					return (string)regObj;
				}
				if (RegistryHelperLite.RegEntryExists(RegistryHelperLite.CompanyKeyLocalMachine,
													  "Pathway", "WritingSystemStore", out regObj))
				{
					Common.SupportFolder = "";
					return (string)regObj;
				}
				if (IsUnixOS())
				{
					return Common.PathCombine("/var/lib/fieldworks", "SIL/WritingSystemStore");
				}
				// fall back on the special environment folder (e.g., c:/ProgramData) - this directory depends on OS
				return Common.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
										  "SIL/WritingSystemStore");
			}
			catch
			{
				// fall back on the special environment folder (e.g., c:/ProgramData) - this directory depends on OS
				return Common.PathCombine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
										  "SIL/WritingSystemStore");
			}
		}

		#endregion

		#region GetApplicationPath

		public static string GetApplicationPath()
		{
			string pathwayDir = PathwayPath.GetPathwayDir();
			if (string.IsNullOrEmpty(pathwayDir))
				return Path.GetDirectoryName(Application.ExecutablePath);
			return pathwayDir;
		}

		public static string ProgBase = string.Empty;

		/// <summary>
		/// Calculates the path to the file based on the program directory set in the registry.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <returns>full path to the file</returns>
		public static string FromRegistry(string file)
		{
			if (Path.IsPathRooted(file))
				return file;
			if (string.IsNullOrEmpty(ProgBase))
			{
				ProgBase = PathwayPath.GetPathwayDir();
				if (string.IsNullOrEmpty(ProgBase))
				{
					if (!Testing)
					{
						Debug.Fail(
							@"Pathway directory is not specified in the registry (HKEY_LOCAL_MACHINE/SOFTWARE/SIL/PATHWAY/PathwayDir)");
						return FromProg(file);
					}

				}
			}
			return Common.PathCombine(ProgBase, file);
		}

		/// <summary>
		/// Calculates the path to the file based on where the program is installed
		/// </summary>
		/// <param name="s">path to file potentially relative to where program was installed</param>
		/// <returns>full path to file reference by s</returns>
		public static string FromProg(string s)
		{
			if (Path.IsPathRooted(s))
				return s;
			if (string.IsNullOrEmpty(ProgBase))
				ProgBase = Path.GetDirectoryName(Application.ExecutablePath);
			return Common.PathCombine(ProgBase, s);
		}

		#endregion

		#region GetFiledWorksPath()

		/// <summary>
		/// Return the Field Works Path 
		/// </summary>
		/// <returns>Field Works Path</returns>
		public static string GetFiledWorksPath()
		{
			return
				DirectoryPathReplace(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
									 @"/SIL/FieldWorks/");
		}

		#endregion

		#region GetFiledWorksPath Version()

		/// <summary>
		/// Return the Field Works Path 
		/// </summary>
		/// <returns>Field Works Path</returns>
		public static string GetFiledWorksPathVersion()
		{
			string executablePath = Path.GetDirectoryName(Application.ExecutablePath);
			if (executablePath.Contains("FieldWorks 7"))
				return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
					   @"/SIL/FieldWorks 7/";

			return GetFiledWorksPath();
		}

		#endregion

		public static string GetProductName()
		{
			return Application.ProductName;
		}

		public static void OpenOutput(string outputPathWithFileName)
		{
			try
			{
				if (!Testing && File.Exists(outputPathWithFileName))
				{
					Process.Start(outputPathWithFileName);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Sets the Font Size and Font Name to the Given Form
		/// and ForeColor to Systems WindowText.
		/// </summary>
		/// <param name="form">The Form to be Modified</param>
		public static void SetFont(Form form)
		{
			foreach (Control ctl in form.Controls)
			{
				if (ctl is Label || ctl is Button)
				{
					ctl.Font = UIFont;
					ctl.ForeColor = SystemColors.WindowText;
				}
			}
		}

		/// <summary>
		/// Get all user path
		/// </summary>
		/// <returns></returns>
		public static string GetAllUserPath()
		{
			string allUserPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			allUserPath += "/SIL/Pathway";
			return DirectoryPathReplace(allUserPath);
		}

		/// <summary>
		/// Get all user AppPath Alone
		/// </summary>
		/// <returns></returns>
		public static string GetAllUserAppPath()
		{
			string allUserPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			return DirectoryPathReplace(allUserPath);
		}

		/// <summary>
		/// Path.GetFileNameWithoutEx failed in this case "5.25x8.25in 11pt Justified_2010-12-17_0251.xhtml"
		/// </summary>
		public static string GetFileNameWithoutExtension(string fileNameWithPath)
		{
			string fileName = Path.GetFileName(fileNameWithPath);
			fileName = fileName.Substring(0, fileName.LastIndexOf('.'));
			return fileName;
		}

		/// <summary>
		/// ol.myclass or ol
		/// </summary>
		/// <param name="newClassName">ol.myclass,div.class</param>
		/// <returns>ol</returns>
		public static string IsTagClass(string newClassName)
		{
			ArrayList tagList = new ArrayList();
			tagList.Add("ol");
			tagList.Add("ul");
			tagList.Add("li");
			tagList.Add("p");
			tagList.Add("a");
			string result = string.Empty;
			string classNameWOParent = LeftString(newClassName, SepParent);
			if (tagList.Contains(classNameWOParent))
			{
				result = classNameWOParent;
			}
			else
			{
				int tagPos = classNameWOParent.IndexOf(Common.SepTag);
				if (tagPos > -1)
				{
					result = classNameWOParent.Substring(0, tagPos);
				}
			}
			return result;
		}

		/// <summary>
		/// Delete all files in the folder except output
		/// </summary>
		/// <param name="outputFolder">Export folder path</param>
		/// <param name="extension"> </param>
		/// <param name="nameLike"> </param>
		/// <param name="directory"> </param>
		public static void CleanupExportFolder(string outputFolder, string extension, string nameLike, string directory)
		{
			//#if (DEBUG)
			//            return;
			//#endif
			List<string> lstExtn = new List<string>();
			List<string> lstNamelike = new List<string>();
			List<string> lstDirecorylike = new List<string>();

			if (extension.Trim().Length > 0)
			{
				string[] extn = extension.Split(',');

				lstExtn.AddRange(extn);
			}

			if (nameLike.Trim().Length > 0)
			{
				string[] name = nameLike.Split(',');
				lstNamelike.AddRange(name);
			}

			if (directory.Trim().Length > 0)
			{
				string[] folder = directory.Split(',');
				lstDirecorylike.AddRange(folder);
			}


			outputFolder = Path.GetDirectoryName(outputFolder);
			string[] filePath = Directory.GetFiles(outputFolder);
			foreach (string file in filePath)
			{
				try
				{
					string type = Path.GetExtension(file).ToLower();
					if (lstExtn.Contains(type))
					{
						File.Delete(file);
					}

					string fi = Path.GetFileNameWithoutExtension(file).ToLower();
					foreach (string filelike in lstNamelike)
					{
						if (fi.Contains(filelike.Trim().ToLower()))
						{
							File.Delete(file);
						}
					}

					if (type == ".css" && !fi.ToLower().Contains("merged"))
					{
						File.Delete(file);
					}
				}
				catch
				{
				}
			}

			string[] FolderList = Directory.GetDirectories(outputFolder);
			foreach (string folderName in FolderList)
			{
				if (lstDirecorylike.Contains(Path.GetFileName(folderName)))
				{
					try
					{
						Directory.Delete(folderName, true);
					}
					catch
					{
					}
				}
			}


		}

		/// <summary>
		/// Inserts the text at the Top of the source File.
		/// </summary>
		/// <param name="sourceFile">Input source File</param>
		/// <param name="textToInsert">Text to Insert at the Top</param>
		public static void FileInsertText(string sourceFile, string textToInsert)
		{
			string cssFileName = Path.GetFileName(sourceFile);

			if (textToInsert.IndexOf(cssFileName) > 0)
				return;

			if (string.IsNullOrEmpty(sourceFile) || !File.Exists(sourceFile)) return;
			//To copy the file contents to Builder
			FileStream fs = File.OpenRead(sourceFile);
			var sr = new StreamReader(fs);
			var builder = new StringBuilder(sr.ReadToEnd());
			fs.Close();
			sr.Close();
			var fsWrite = new FileStream(sourceFile, FileMode.Create, FileAccess.ReadWrite);
			var writer = new StreamWriter(fsWrite);
			writer.WriteLine(textToInsert);
			writer.Write(builder.ToString());
			writer.Close();
			fsWrite.Close();
		}

		/// <summary>
		/// Make sure the path contains the proper / for the operating system.
		/// </summary>
		/// <param name="path">input path</param>
		/// <returns>normalized path</returns>
		public static string DirectoryPathReplace(string path)
		{
			if (string.IsNullOrEmpty(path)) return path;

			string returnPath = path.Replace('/', Path.DirectorySeparatorChar);
			returnPath = returnPath.Replace('\\', Path.DirectorySeparatorChar);
			return returnPath;

		}

		/// <summary>
		/// Make sure the path contains the proper / for the operating system.
		/// </summary>
		/// <param name="path">input path</param>
		/// <returns>normalized with "/" path</returns>
		public static string DirectoryPathReplaceWithSlash(string path)
		{
			if (string.IsNullOrEmpty(path)) return path;
			string returnPath = path.Replace('\\', '/');
			returnPath = returnPath.Replace(Path.DirectorySeparatorChar, '/');
			return returnPath;
		}

		/// <summary>
		/// Deletes the current Directory
		/// </summary>
		/// <param name="directoryPath">Directory name to be deleted</param>
		/// <returns>true/false based on success/failure</returns>
		public static bool DeleteDirectory(string directoryPath)
		{
			bool deleted = false;
			if (Directory.Exists(directoryPath))
			{
				try
				{
					DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
					bool isReadOnly = ((File.GetAttributes(directoryPath) & FileAttributes.ReadOnly) ==
									   FileAttributes.ReadOnly);
					if (isReadOnly)
					{
						dirInfo.Attributes = FileAttributes.Normal;
					}
					dirInfo.Delete(true);
					deleted = true;
				}
				catch (Exception ex)
				{
					Console.Write(ex.Message);
				}
			}
			return deleted;
		}


		public static void CleanDirectory(DirectoryInfo di)
		{
			try
			{
				if (di == null)
					return;

				foreach (FileInfo fi in di.GetFiles())
				{
					fi.IsReadOnly = false;
					fi.Delete();
				}

				foreach (FileSystemInfo fsEntry in di.GetFileSystemInfos())
				{
					CleanDirectory(fsEntry as DirectoryInfo);
					fsEntry.Delete();
				}
				WaitForDirectoryToBecomeEmpty(di);
			}
			catch
			{
			}
		}

		public static void CleanFile(DirectoryInfo di)
		{
			try
			{
				if (di == null)
					return;

				foreach (FileInfo fi in di.GetFiles())
				{
					fi.IsReadOnly = false;
					fi.Delete();
				}
			}
			catch
			{
			}
		}

		private static void WaitForDirectoryToBecomeEmpty(DirectoryInfo di)
		{
			for (int i = 0; i < 5; i++)
			{
				if (di.GetFileSystemInfos().Length == 0)
					return;
				Console.WriteLine(di.FullName + i);
				System.Threading.Thread.Sleep(50 * i);
			}
		}

		/// <summary>
		/// Deletes the directory using wildcard Ex: c:\temp\SilPathwaytmp*
		/// </summary>
		/// <param name="directoryPath">Directory name to be deleted Ex: c:\sil\</param>
		/// <param name="pattern">Wildcard pattern Ex: tmp*.*</param>
		public static void DeleteDirectoryWildCard(string directoryPath, string pattern)
		{
			foreach (string directory in Directory.GetDirectories(directoryPath, pattern))
			{
				try
				{
					DirectoryInfo di = new DirectoryInfo(directory);
					Common.CleanDirectory(di);
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Deletes the current File
		/// </summary>
		/// <param name="filePath">File name to be deleted</param>
		/// <returns>true/false based on success/failure</returns>
		public static bool DeleteFile(string filePath)
		{
			bool deleted = false;
			if (File.Exists(filePath))
			{
				try
				{
					File.Delete(filePath);
					deleted = true;
				}
				catch (Exception ex)
				{
					Console.Write(ex.Message);
				}
			}
			return deleted;
		}

		#region MakeSingleCSS(string fullPath)

		/// -------------------------------------------------------------------------------------------
		/// <summary>
		/// This method collects css files names into ArrayList based on base CSS File.
		/// </summary>
		/// <param name="fullPath">Its gets the file path of the CSS File</param>
		/// <param name="targetFileName">New CSS filename</param>
		/// <returns>Nothing but the ArrayList contains CSS filenames which are used</returns>
		/// -------------------------------------------------------------------------------------------
		public static string MakeSingleCSS(string fullPath, string targetFileName)
		{
			string cssFile = fullPath.Substring(fullPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
			string tempName = RandromString();

			if (targetFileName.Length > 1)
				cssFile = tempName + targetFileName;

			string mergepath = Path.GetTempPath();
			string mergedCSSfile = PathCombine(mergepath, cssFile);
			ArrayList arrayCSSFile = new ArrayList();
			string BaseCssFileWithPath = fullPath;
			arrayCSSFile = GetCSSFileNames(fullPath, BaseCssFileWithPath);
			arrayCSSFile.Add(BaseCssFileWithPath);
			RemovePreviousMirroredPage(arrayCSSFile);
			using (FileStream fs2 = new FileStream(mergedCSSfile, FileMode.Create, FileAccess.Write))
			{
				var sw2 = new StreamWriter(fs2);
				for (int i = 0; i <= arrayCSSFile.Count - 1; i++)
				{
					sw2.WriteLine("/* File Name: " + Path.GetFileName(arrayCSSFile[i].ToString()) + " */");
					string fstr;
					var fs = new FileStream(arrayCSSFile[i].ToString(), FileMode.Open, FileAccess.Read);
					var sr = new StreamReader(fs);
					while ((fstr = sr.ReadLine()) != null)
					{
						if (!fstr.Contains("@import"))
						{
							// To avoid the Mozilla Property
							if (!fstr.Contains("-moz"))
							{
								sw2.WriteLine(fstr);
							}
							else
							{
								string splitText = fstr.Substring(0, fstr.IndexOf("-moz") - 1);
								sw2.WriteLine(splitText);
							}
						}
					}
					sw2.WriteLine("");
					sr.Close();
					fs.Close();
				}
				sw2.Close();
				fs2.Close();
			}

			return mergedCSSfile;
		}

		public static string RandromString()
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var stringChars = new char[6];
			var random = new Random();

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}

			var finalString = new String(stringChars);

			return finalString;
		}

		public static void RemovePreviousMirroredPage(ArrayList arrayCssFile)
		{
			bool removeMirrorPage = false;
			bool removeEveryPage = false;
			bool removePageNumber = false;
			int count = arrayCssFile.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				string cssFile = arrayCssFile[i].ToString();
				//For Remove Mirrored Page
				if (cssFile.IndexOf("Running_Head_Every_Page") >= 0)
				{
					removeMirrorPage = true;
				}
				if (removeMirrorPage)
				{
					//1893
					if (cssFile.IndexOf("Running_Head_Mirrored") >= 0 || cssFile.IndexOf("PageNumber_TopInside") >= 0 ||
						cssFile.IndexOf("PageNumber_TopOutside") >= 0
						|| cssFile.IndexOf("PageNumber_TopCenter_Mirrored") >= 0 ||
						cssFile.IndexOf("PageNumber_BottomInside") >= 0 ||
						cssFile.IndexOf("PageNumber_BottomOutside") >= 0 ||
						cssFile.IndexOf("PageNumber_BottomCenter_Mirrored") >= 0)
					{
						arrayCssFile.RemoveAt(i);
					}
				}

				////For Remove Every Page TD-3307
				if (cssFile.IndexOf("Running_Head_Mirrored_Chapter") >= 0 ||
					cssFile.IndexOf("PageNumber_TopInside") >= 0 || cssFile.IndexOf("PageNumber_TopOutside") >= 0)
				{
					removeEveryPage = true;
				}
				if (removeEveryPage)
				{
					if (cssFile.IndexOf("PageNumber_TopCenter") >= 0)
					{
						arrayCssFile.RemoveAt(i);
					}
				}

				//For Page number
				if (cssFile.IndexOf("PageNumber_") >= 0 && cssFile.IndexOf("FirstPage") != -1)
				{
					if (removePageNumber)
					{
						arrayCssFile.RemoveAt(i);
					}
					removePageNumber = true;
				}

			}
		}

		/// <summary>
		/// Make sure the path contains the proper / for the operating system.
		/// </summary>
		/// <param name="path1"></param>
		/// <param name="path2"></param>
		/// <returns>normalized path</returns>
		public static string PathCombine(string path1, string path2)
		{
			path1 = DirectoryPathReplace(path1);
			path2 = DirectoryPathReplace(path2);
			if (path1 == null)
			{
				return path2;
			}
			else if (path2 == null)
			{
				return path1;
			}
			else
			{
				return Path.Combine(path1, path2);
			}
		}

		/// <summary>
		/// sort b a c as a b c
		/// </summary>
		/// <param name="multiClass">b a c</param>
		/// <returns>a b c</returns>
		public static string SortMutiClass(string multiClass)
		{
			string[] splitClass = multiClass.Split(' ');
			Array.Sort(splitClass);

			string result = string.Empty;

			foreach (string className in splitClass)
			{
				result = result + Common.Space + className;
			}
			return result.Trim();
		}

		#endregion

		public enum CalcType
		{
			Height,
			Width
		};
		/// -------------------------------------------------------------------------------------------
		/// <summary>
		/// To find the width of the image.
		/// </summary>
		/// <returns> </returns>
		/// -------------------------------------------------------------------------------------------
		public static string CalcDimension(string fromPath, ref string imgDimension, CalcType calcType)
		{
			double retValue = 0.0;
			try
			{
				if (File.Exists(fromPath))
				{
					Image fullimage = Image.FromFile(fromPath);
					double height = fullimage.Height;
					double width = fullimage.Width;
					fullimage.Dispose();

					if (calcType == CalcType.Width)
					{
						retValue = width / height * double.Parse(imgDimension, CultureInfo.GetCultureInfo("en-US"));
						if (ColumnWidth > 0 && retValue > ColumnWidth)
						{
							retValue = ColumnWidth * .9;
						}
					}
					else if (calcType == CalcType.Height)
					{
						int counter;
						string retValue1 = GetNumericChar(imgDimension, out counter);
						if (imgDimension.IndexOf("%") > 0)
						{
							double widthInPt = double.Parse(retValue1, CultureInfo.GetCultureInfo("en-US")) / 100 * width;
							if (widthInPt > ColumnWidth)
							{
								widthInPt = ColumnWidth;
							}
							imgDimension = widthInPt.ToString();
							retValue = height / width * widthInPt;
						}
						else
						{
							retValue = height / width * double.Parse(retValue1, CultureInfo.GetCultureInfo("en-US"));
						}
					}
				}
			}
			catch
			{
			}
			return retValue.ToString();
		}

		public static string GetLeadingType(Dictionary<string, string> Properties)
		{
			string propertyType = "unit";
			if (Properties.ContainsKey("Leading") && Properties["Leading"] == "Auto")
			{
				propertyType = "enumeration";
			}
			return propertyType;
		}

		#region SaveInFolder
		public const string SaveInFolderBase = "Publications";
		/// <summary>
		/// return the folder where publication data should be saved
		/// </summary>
		/// <param name="template">Template with $(xxx)s for each element</param>
		/// <param name="database">name of Database to use as element of path</param>
		/// <param name="layout">name of style sheet to use as an element of the path</param>
		/// <returns>dereferenced path name</returns>
		public static string GetSaveInFolder(string template, string database, string layout)
		{
			Dictionary<string, string> map = new Dictionary<string, string>();
			map["Documents"] = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			map["Base"] = SaveInFolderBase;
			map["CurrentProject"] = database;
			map["StyleSheet"] = layout;
			map["DateTime"] = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
			Substitution substitution = new Substitution();
			var del = new Substitution.MyDelegate(map);
			var result = substitution.DoSubstitute(template, @"\$\(([^)]*)\)s", RegexOptions.None, del.myValue);
			return DirectoryPathReplace(result);
		}

		/// <summary>
		/// Checks to see if user entered a custom SaveInFolder
		/// </summary>
		/// <param name="s">SaveInFolder value</param>
		/// <returns>True if today's date is not in the right place</returns>
		public static bool CustomSaveInFolder(string s)
		{
			var length = s.Length;
			if (length < 17)
				return true;
			return s.Substring(length - 17, 10) != DateTime.Now.ToString("yyyy-MM-dd");
		}
		#endregion SaveInFolder

		#region PathwayHelpFileDirectory
		/// <summary>
		/// PathwayHelpFileDirectory help file
		/// </summary>
		public static string PathwayHelpFileDirectory()
		{
			try
			{
				var helpFolder = FromRegistry("Help");
				var directoryInfo = new DirectoryInfo(helpFolder);
				var fileInfoList = directoryInfo.GetFiles("*.chm");
				return fileInfoList[0].FullName;
			}
			catch
			{

			}
			return string.Empty;
		}
		#endregion PathwayHelpFileDirectory

		#region PathwayStudentManualLaunch
		public static void PathwayStudentManualLaunch()
		{
			try
			{
				var pathwayFolder = FromRegistry("");
				var directoryInfo = new DirectoryInfo(pathwayFolder);
				var fileInfoList = directoryInfo.GetFiles("Pathway_Student_Manual_*.pdf");
				using (Process process = new Process())
				{
					process.StartInfo.FileName = fileInfoList[0].FullName;
					process.Start();
				}
			}
			catch
			{

			}
		}
		#endregion PathwayStudentManualLaunch

		public static string GetTextValue(object sender, out string controlName)
		{
			string attribValue = string.Empty;
			controlName = string.Empty;
			if (sender is TextBox)
			{
				TextBox myvalue = (TextBox)sender;
				attribValue = myvalue.Text;
				controlName = myvalue.Name;
			}
			else if (sender is ComboBox)
			{
				ComboBox myvalue = (ComboBox)sender;
				attribValue = myvalue.Text;
				controlName = myvalue.Name;
			}
			else if (sender is CheckBox)
			{
				CheckBox myvalue = (CheckBox)sender;
				attribValue = myvalue.Checked ? "Yes" : "No";
				controlName = myvalue.Name;
			}
			return attribValue;
		}

		public static Dictionary<string, string> FillMappedFonts(Dictionary<string, string> fontLangMapTemp)
		{
			var tempGenericFontFile = CopyXmlFileToTempDirectory("GenericFont.xml");

			string xPath = "//font-language-mapping";
			XmlNodeList fontList = GetXmlNodes(tempGenericFontFile, xPath);
			if (fontList != null && fontList.Count > 0)
			{
				foreach (XmlNode xmlNode in fontList)
				{
					if (xmlNode.Attributes != null)
						fontLangMapTemp[xmlNode.Attributes.GetNamedItem("name").Value] = xmlNode.InnerText;
				}
			}
			return fontLangMapTemp;
		}



		public static Dictionary<string, string> FillMappedFonts(string wsPath, Dictionary<string, string> fontLangMapTemp)
		{
			if (!Directory.Exists(wsPath)) return fontLangMapTemp;

			DirectoryInfo dir = new DirectoryInfo(wsPath);
			FileInfo[] files = dir.GetFiles("*.ldml");
			foreach (FileInfo file in files)
			{
				XmlDocument ldml = DeclareXMLDocument(false);
				ldml.Load(Common.PathCombine(wsPath, file.Name));
				var nsmgr = new XmlNamespaceManager(ldml.NameTable);
				nsmgr.AddNamespace("palaso", "urn://palaso.org/ldmlExtensions/v1");
				var node = ldml.SelectSingleNode("//special/palaso:defaultFontFamily/@value", nsmgr);
				if (node != null)
				{
					string fontname = file.Name.Replace(".ldml", "");
					if (!fontLangMapTemp.ContainsKey(fontname))
						fontLangMapTemp[fontname] = node.Value;
				}
			}
			return fontLangMapTemp;
		}

		public static void MigrateCustomSheet(string userSheet, string updatedSheet)
		{
			List<string> mediaType = new List<string>();
			mediaType.Add("paper");
			mediaType.Add("mobile");
			mediaType.Add("web");
			mediaType.Add("others");
			XmlDocument userSettings = new XmlDocument();
			XmlDocument installerSettings = new XmlDocument();
			string backUpFileName = "backUp_" + DateTime.Now.ToString("MM-dd-yyyy") + ".xml";
			string backUpFilePath = Common.PathCombine(Path.GetDirectoryName(userSheet), backUpFileName);
			File.Copy(userSheet, backUpFilePath, true);

			if (File.Exists(userSheet))
				File.Delete(userSheet);

			File.Copy(updatedSheet, userSheet, true);

			userSettings.Load(backUpFilePath);
			installerSettings.Load(userSheet);
			foreach (string media in mediaType)
			{
				string xPathUser = @"//stylePick/styles/" + media + "/style[@type='Custom']";
				XmlNodeList nodeList = userSettings.SelectNodes(xPathUser);

				string xPathInstaller = @"//stylePick/styles/" + media;
				XmlNode nodeInst = installerSettings.SelectSingleNode(xPathInstaller);
				XmlNodeList nodeInstList = installerSettings.SelectNodes(xPathUser);

				if (nodeList != null)
					foreach (XmlNode backupNode in nodeList)
					{
						bool isExist = false;
						string oldNodeName = backupNode.Attributes["name"].Value;
						if (nodeInstList != null)
							foreach (XmlNode newNode in nodeInstList)
							{
								string newNodeName = newNode.Attributes["name"].Value;
								if (newNodeName == oldNodeName)
								{
									isExist = true;
									break;
								}
							}
						if (!isExist)
						{
							XmlDocumentFragment docFrag = installerSettings.CreateDocumentFragment();
							docFrag.InnerXml = backupNode.OuterXml;
							nodeInst.AppendChild(docFrag);
						}
					}
			}
			installerSettings.Save(userSheet);
		}

		/// <summary>
		/// To copy Temporary Office files to Environmental Temp Folder instead of keeping changes in Application Itself.
		/// </summary>
		/// <param name="sourceFolder"></param>
		/// <param name="destFolder"></param>
		/// <param name="copySubFolder"> </param>
		public static void CopyFolderandSubFolder(string sourceFolder, string destFolder, bool copySubFolder)
		{
			if (Directory.Exists(destFolder))
			{
				DirectoryInfo di = new DirectoryInfo(destFolder);
				Common.CleanDirectory(di);
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

				if (copySubFolder)
				{
					string[] folders = Directory.GetDirectories(sourceFolder);
					foreach (string folder in folders)
					{
						if (folder != destFolder)
						{
							string name = Path.GetFileName(folder);
							string dest = Common.PathCombine(destFolder, name);
							if (name != ".svn")
							{
								CopyFolderandSubFolder(folder, dest, true);
							}
						}
					}
				}
			}
			catch
			{
				return;
			}
		}

		/// <summary>
		/// To copy HTML, Images and fonts to HTML5 folder and ignore the "ignoreExtns" files
		/// </summary>
		/// <param name="sourceFolder"></param>
		/// <param name="destFolder"></param>
		/// <param name="ignoreFiles"> </param>
		public static void CustomizedFileCopy(string sourceFolder, string destFolder, string ignoreFiles)
		{
			if (Directory.Exists(destFolder))
			{
				var di = new DirectoryInfo(destFolder);
				Common.CleanDirectory(di);
			}
			Directory.CreateDirectory(destFolder);
			string[] files = Directory.GetFiles(sourceFolder);
			try
			{
				bool isfileIgnore = false;
				string[] iFiles = ignoreFiles.Split(',');




				foreach (string file in files)
				{
					if (file == null) continue;
					foreach (var ifile in iFiles)
					{
						if (ifile.Trim() == Path.GetFileName(file))
						{
							isfileIgnore = true;
							break;
						}
						isfileIgnore = false;
					}
					if (isfileIgnore) continue;
					string name = Path.GetFileName(file);
					string dest = Common.PathCombine(destFolder, name);
					File.Copy(file, dest);
				}
			}
			catch
			{
				return;
			}
		}

		public static string ReplaceSeperators(string styleName)
		{
			if (styleName.Contains(SepPseudo))
				styleName = styleName.Replace(SepPseudo, "");

			if (styleName.Contains(" "))
				styleName = styleName.Replace(" ", "");

			if (styleName.Contains("-"))
				styleName = styleName.Replace("-", "");

			if (styleName.Contains(sepPrecede))
				styleName = styleName.Replace(sepPrecede, "");

			if (styleName.Contains(SepPseudo))
				styleName = styleName.Replace(SepPseudo, "");

			if (styleName.Contains(SepParent))
				styleName = styleName.Replace(SepParent, "");

			if (styleName.Contains(SepTag))
				styleName = styleName.Replace(SepTag, "");

			if (styleName.IndexOf("1") > 0)
				styleName = styleName.Replace("1", "ONE");

			return styleName;
		}

		public static string GetOsName()
		{
			OperatingSystem osInfo = Environment.OSVersion;

			switch (osInfo.Platform)
			{
				case System.PlatformID.Win32NT:
					switch (osInfo.Version.Major)
					{
						case 3:
							return "Windows NT 3.51";
							break;
						case 4:
							return "Windows NT 4.0";
							break;
						case 5:
							if (osInfo.Version.Minor == 0)
								return "Windows 2000";
							else
								return "Windows XP";
							break;
						case 6:
							if (osInfo.Version.Minor == 1)
								return "Windows7";
							return "Windows8";
					}
					break;

			}
			return osInfo.VersionString.ToString();
		}

		/// <summary>
		/// To get the font name based on the unicode value of the character.
		/// </summary>
		/// <param name="unicodeString">String to find the relevant font</param>
		/// <returns>font name</returns>
		public static string GetLanguageUnicode(string unicodeString)
		{
			if (unicodeString.Length <= 0) return "";
			int unicodeDecimal = 0;
			unicodeString = unicodeString.Trim();
			string fontName = string.Empty;
			string xmlFileNameWithPath = CopyXmlFileToTempDirectory("GenericFont.xml");
			string xPath = "//font-language-unicode-map";
			XmlNodeList fontList = GetXmlNodes(xmlFileNameWithPath, xPath);
			if (fontList != null && fontList.Count > 0)
			{
				bool isLanguageFound = false;
				foreach (char ch in unicodeString)
				{
					unicodeDecimal = (int)ch;
					foreach (XmlNode xmlNode in fontList)
					{
						if (xmlNode.Attributes != null)
						{
							int hexFrom = 0;
							int hexTo = 0;
							string rangeFrom = xmlNode.Attributes["From"].Value;
							if (rangeFrom.IndexOf("0x") == 0)
							{
								rangeFrom = rangeFrom.Replace("0x", "");
								hexFrom = int.Parse(rangeFrom, NumberStyles.HexNumber);
							}
							else
							{
								hexFrom = int.Parse(rangeFrom);
							}
							string rangeTo = xmlNode.Attributes["To"].Value;
							if (rangeTo.IndexOf("0x") == 0)
							{
								rangeTo = rangeTo.Replace("0x", "");
								hexTo = int.Parse(rangeTo, NumberStyles.HexNumber);
							}
							else
							{
								hexTo = int.Parse(rangeTo);
							}
							if (unicodeDecimal >= hexFrom && unicodeDecimal <= hexTo)
							{
								fontName = xmlNode.InnerText;
								isLanguageFound = true;
								break;
							}
						}
					}
					if (isLanguageFound)
					{
						break;
					}
				}
			}
			return fontName;
		}

		public static string SetPropertyValue(string propertyName, string propertyValue)
		{
			if (propertyName == "")
				propertyValue = propertyName + propertyValue;
			else
				propertyValue = propertyName + " " + propertyValue;

			if (propertyValue.IndexOf("em") == -1)
			{
				propertyValue = propertyValue + "pt";
			}

			return propertyValue;
		}

		public static string PercentageToEm(string propertyValue)
		{
			if (propertyValue.IndexOf("%") > 0)
			{
				propertyValue = propertyValue.Replace("%", "");
				float numericValue = Convert.ToInt32(propertyValue);
				numericValue = numericValue / 100;
				propertyValue = numericValue + "em";
			}
			else
			{
				propertyValue = propertyValue + "pt";
			}
			return propertyValue;
		}

		public static string ConvertTifftoImage(string pathwithFileName, string convertFormatType)
		{
			string fileName = Path.GetFileName(pathwithFileName);
			string fileNameWithOutExtension = Path.GetFileNameWithoutExtension(fileName);
			string fileOutputPath = pathwithFileName.Replace(".tif", "." + convertFormatType); //Common.PathCombine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileNameWithOutExtension)) + "." + convertFormatType;
			if (!File.Exists(fileOutputPath))
			{
				if (File.Exists(pathwithFileName))
				{
					using (var tiff = new Bitmap(pathwithFileName))
					{
						tiff.Save(fileOutputPath, ImageFormat.Jpeg);
					}
					pathwithFileName = pathwithFileName.Replace(".tif", "." + convertFormatType);
					VaryQualityLevel(pathwithFileName);
				}
				else
				{
					pathwithFileName = "";
				}
			}
			else
			{
				pathwithFileName = pathwithFileName.Replace(".tif", "." + convertFormatType);
			}
			return pathwithFileName;
		}

		private static void VaryQualityLevel(string imageFile)
		{
			try
			{
				// Get a bitmap.
				Bitmap bmp1 = new Bitmap(@"" + imageFile);

				//Or you do can use buil-in method
				//ImageCodecInfo jgpEncoder GetEncoderInfo("image/gif");//"image/jpeg",...
				ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

				// Create an Encoder object based on the GUID
				// for the Quality parameter category.
				System.Drawing.Imaging.Encoder myEncoder =
				System.Drawing.Imaging.Encoder.Quality;

				// Create an EncoderParameters object.
				// An EncoderParameters object has an array of EncoderParameter
				// objects. In this case, there is only one
				// EncoderParameter object in the array.
				EncoderParameters myEncoderParameters = new EncoderParameters(1);

				EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
				myEncoderParameters.Param[0] = myEncoderParameter;
				bmp1.Save(imageFile, jgpEncoder, myEncoderParameters);

				myEncoderParameter = new EncoderParameter(myEncoder, 100L);
				myEncoderParameters.Param[0] = myEncoderParameter;
				bmp1.Save(imageFile, jgpEncoder, myEncoderParameters);

				// Save the bitmap as a JPG file with zero quality level compression.
				myEncoderParameter = new EncoderParameter(myEncoder, 0L);
				myEncoderParameters.Param[0] = myEncoderParameter;
				bmp1.Save(imageFile, jgpEncoder, myEncoderParameters);
			}
			catch { }
		}

		private static ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
			foreach (ImageCodecInfo codec in codecs)
			{
				if (codec.FormatID == format.Guid)
					return codec;
			}
			return null;
		}

		public static bool IsUnixOS()
		{
			bool isRecentVersion = false;
			string getOSName = GetOsName();
			if (getOSName.IndexOf("Unix") >= 0)
			{
				isRecentVersion = true;
			}
			return isRecentVersion;
		}

		/// <summary>
		/// Method to get the font-name from the meta tag in the ".xhtml" input file based on key "scheme="Default Font"
		/// and append the fontname in beginning of the input css file.
		/// so, these fonts will be the default for the whole content.
		/// </summary>
		/// <param name="inputXhtmlFileName">Passing the input xhtml file</param>
		/// <param name="inputCssFileName">Passing input css file</param>
		/// <param name="isFLEX">FLEX / Paratext</param>
		/// <param name="inputRevCssFileName">Merged rev css to include the font list which used for mergedmain css file</param>
		public static void LanguageSettings(string inputXhtmlFileName, string inputCssFileName, bool isFLEX, string inputRevCssFileName)
		{
			StringBuilder newProperty = new StringBuilder();
			XmlDocument xdoc = DeclareXMLDocument(false);
			xdoc.Load(inputXhtmlFileName);
			XmlNodeList fontList = xdoc.GetElementsByTagName("meta");
			foreach (XmlNode fontName in fontList)
			{
				if (isFLEX && (fontName.OuterXml.IndexOf("scheme=\"Default Font\"") > 0 || fontName.OuterXml.IndexOf("scheme=\"language to font\"") > 0))
				{
					string fntName = fontName.Attributes["name"].Value;
					string fntContent = fontName.Attributes["content"].Value;
					newProperty.AppendLine("div[lang='" + fntName + "']{ font-family: \"" + fntContent + "\";}");
					newProperty.AppendLine("span[lang='" + fntName + "']{ font-family: \"" + fntContent + "\";}");
				}
				else if (!isFLEX && fontName.OuterXml.IndexOf("name=\"fontName\"") > 0)
				{
					string fntContent = fontName.Attributes["content"].Value;
					newProperty.AppendLine("div{ font-family: \"" + fntContent + "\";}");
					newProperty.AppendLine("span{ font-family: \"" + fntContent + "\";}");
				}
			}
			FileInsertText(inputCssFileName, newProperty.ToString());
			if (File.Exists(inputRevCssFileName))
			{ FileInsertText(inputRevCssFileName, newProperty.ToString()); }
		}

		/// <summary>
		/// Method to get the font-name from the ldml file which located in WritingSystemStore folder and create a css file
		/// and append the fontname in beginning of the input css file.
		/// so, these fonts will be the default for the whole content.
		/// </summary>
		/// <param name="inputCssFileName">Passing input css file</param>
		public static void TeLanguageSettings(string inputCssFileName)
		{
			StringBuilder newProperty = new StringBuilder();
			string teDefaultFilePath = Common.GetAllUserPath();
			teDefaultFilePath = Common.PathCombine(teDefaultFilePath, "TELanguage.css");
			if (File.Exists(teDefaultFilePath))
			{
				newProperty.Append(File.ReadAllText(teDefaultFilePath));
			}
			else
			{
				if (!Directory.Exists(GetLDMLPath())) return;
				string[] filePaths = Directory.GetFiles(GetLDMLPath(), "*.ldml");
				foreach (string filePath in filePaths)
				{
					string fileName = Path.GetFileNameWithoutExtension(filePath);
					XmlDocument xmlDocument = DeclareXMLDocument(false);
					xmlDocument.Load(filePath);
					XmlNode node =
						xmlDocument.SelectSingleNode(
							"/ldml/special[1]/*[namespace-uri()='urn://palaso.org/ldmlExtensions/v1' and local-name()='defaultFontFamily'][1]/@value");
					newProperty.AppendLine("div[lang='" + fileName + "']{ font-family: \"" + node.Value + "\";}");
					newProperty.AppendLine("span[lang='" + fileName + "']{ font-family: \"" + node.Value + "\";}");
				}

				using (StreamWriter sw = File.CreateText(teDefaultFilePath))
				{
					sw.Write(newProperty.ToString());
				}
			}
			FileInsertText(inputCssFileName, newProperty.ToString());
		}

		public static string GetValueFromRegistry(string subKey, string keyName)
		{
			// Opening the registry key

			RegistryKey rk = Registry.LocalMachine;
			// Open a subKey as read-only

			RegistryKey sk1 = rk.OpenSubKey(subKey);
			// If the RegistrySubKey doesn't exist -> (null)

			if (sk1 == null)
			{
				return null;
			}
			else
			{
				try
				{
					if (RegistryCanRead(keyName.ToUpper()) == false)
					{
						return null;
					}

					// If the RegistryKey exists I get its value
					return (string)sk1.GetValue(keyName.ToUpper());
				}
				catch (Exception e)
				{
					return null;
				}
			}
		}

		public static string GetValueFromRegistryFromCurrentUser(string subKey, string keyName)
		{
			try
			{

				// Opening the registry key

				RegistryKey rk = Registry.CurrentUser;
				// Open a subKey as read-only

				RegistryKey sk1 = rk.OpenSubKey(subKey);
				// If the RegistrySubKey doesn't exist -> (null)

				if (sk1 == null)
				{
					return null;
				}
				else
				{
					// If the RegistryKey exists I get its value
					return (string)sk1.GetValue(keyName.ToUpper());
				}

			}
			catch
			{
			}

			return string.Empty;
		}

		public static bool RunCommand(string szCmd, string szArgs, int wait)
		{
			if (szCmd == null) return false;
			using (var myproc = new Process())
			{
				myproc.EnableRaisingEvents = false;
				myproc.StartInfo.CreateNoWindow = true;
				myproc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				myproc.StartInfo.FileName = szCmd;
				myproc.StartInfo.Arguments = szArgs;

				if (!myproc.Start())
					return false;
				//Using WaitForExit( ) allows for the host program
				//to wait for the command its executing before it continues
				var exitCode = 0;
				if (wait == 1)
				{
					myproc.WaitForExit();
					exitCode = myproc.ExitCode;
				}
				myproc.Close();
				return exitCode == 0;
			}
		}

		/// <summary>
		/// Insert the License information from the SIL_License.xml to the exported PDF file.
		/// </summary>
		/// <param name="xhtmlFileName"></param>
		/// <param name="creatorTool"></param>
		/// <param name="inputType">dictionary or scripture - written to control file</param>
		/// <returns></returns>
		public static string InsertCopyrightInPdf(string xhtmlFileName, string creatorTool, string inputType)
		{
			string pdfFileName = xhtmlFileName;
			string executablePath = Path.GetDirectoryName(Application.ExecutablePath);
			if (executablePath.IndexOf("Configuration") > 0 || pdfFileName.ToLower().Contains("local"))
			{
				if (File.Exists(pdfFileName))
				{
					Common.OpenOutput(pdfFileName);
				}
				return pdfFileName;
			}
			//Copyright information added in PDF files
			try
			{
				string tempPdfFileName = string.Empty;
				if (creatorTool != "LibreOffice")
				{
					tempPdfFileName = Common.PathCombine(Path.GetDirectoryName(xhtmlFileName), Path.GetFileName(xhtmlFileName));
				}


				string getPsApplicationPath = Common.GetPSApplicationPath();
				string licenseXml = Common.PathCombine(getPsApplicationPath, "Copyrights");
				licenseXml = Common.PathCombine(licenseXml, "SIL_License.xml");
				string destLicenseXml = Common.PathCombine(Path.GetDirectoryName(xhtmlFileName), "SIL_License.xml");
				Param.LoadSettings();
				string organization;
				try
				{
					organization = Param.Value.ContainsKey("Organization") ? Param.Value["Organization"] : "SIL International";
				}
				catch (Exception)
				{
					organization = "SIL International";
				}
				string exportTitle = string.Empty;
				exportTitle = Param.GetMetadataValue(Param.Title, organization);

				CreateLicenseFileForRunningPdfApplyCopyright(Path.GetDirectoryName(xhtmlFileName), xhtmlFileName, exportTitle, creatorTool, inputType);

				if (File.Exists(destLicenseXml))
					File.Delete(destLicenseXml);

				string copyrightURL = string.Empty;
				if (creatorTool != "Prince XML")
				{
					copyrightURL = CopyrightUrlValue();
				}

				File.Copy(licenseXml, destLicenseXml);
				XmlDocument xDoc = Common.DeclareXMLDocument(true);
				xDoc.Load(destLicenseXml);

				XmlNamespaceManager nsmgr = new XmlNamespaceManager(xDoc.NameTable);
				nsmgr.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
				nsmgr.AddNamespace("xap", "http://ns.adobe.com/xap/1.0/");
				nsmgr.AddNamespace("cc", "http://creativecommons.org/ns#");
				nsmgr.AddNamespace("xapRights", "http://ns.adobe.com/xap/1.0/rights/");

				string xPath = "//xap:CreateDate";
				XmlElement root = xDoc.DocumentElement;
				if (root != null)
				{
					XmlNode returnNode = root.SelectSingleNode(xPath, nsmgr);
					returnNode.InnerText = DateTime.Now.Date.ToString();
				}

				xPath = "//xap:CreatorTool";
				root = xDoc.DocumentElement;
				if (root != null)
				{
					XmlNode returnNode = root.SelectSingleNode(xPath, nsmgr);
					returnNode.InnerText = creatorTool;
				}

				xPath = "//xap:ModifyDate";
				root = xDoc.DocumentElement;
				if (root != null)
				{
					XmlNode returnNode = root.SelectSingleNode(xPath, nsmgr);
					returnNode.InnerText = DateTime.Now.Date.ToString();
				}

				xPath = "//xap:MetadataDate";
				root = xDoc.DocumentElement;
				if (root != null)
				{
					XmlNode returnNode = root.SelectSingleNode(xPath, nsmgr);
					returnNode.InnerText = DateTime.Now.Date.ToString();
				}

				nsmgr.AddNamespace("pdf", "http://ns.adobe.com/pdf/1.3/");

				xPath = "//pdf:Producer";
				root = xDoc.DocumentElement;
				if (root != null)
				{
					XmlNode returnNode = root.SelectSingleNode(xPath, nsmgr);
					returnNode.InnerText = DateTime.Now.Date.ToString();
				}

				nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");

				xPath = "//dc:description/rdf:Alt/rdf:li";
				root = xDoc.DocumentElement;
				if (root != null)
				{
					XmlNode returnNode = root.SelectSingleNode(xPath, nsmgr);
					returnNode.InnerText = Param.GetMetadataValue(Param.Description, organization);
				}

				xPath = "//dc:creator/rdf:Seq/rdf:li";
				root = xDoc.DocumentElement;
				if (root != null)
				{
					XmlNode returnNode = root.SelectSingleNode(xPath, nsmgr);
					returnNode.InnerText = Param.GetMetadataValue(Param.Creator, organization);
				}

				xPath = "//dc:title/rdf:Alt/rdf:li";
				root = xDoc.DocumentElement;
				if (root != null)
				{
					XmlNode returnNode = root.SelectSingleNode(xPath, nsmgr);
					returnNode.InnerText = exportTitle;
				}

				xPath = "//dc:subject/rdf:Bag/rdf:li[3]";
				root = xDoc.DocumentElement;
				if (root != null)
				{
					XmlNode returnNode = root.SelectSingleNode(xPath, nsmgr);
					returnNode.InnerText = Param.Value["InputType"];
				}

				xPath = "//dc:rights/rdf:Alt/rdf:li";
				root = xDoc.DocumentElement;
				if (root != null)
				{
					XmlNode returnNode = root.SelectSingleNode(xPath, nsmgr);
					returnNode.InnerText = Common.ConvertUnicodeToString("\\00a9") + " " + organization + Common.ConvertUnicodeToString("\\00ae") + " " + DateTime.Now.Year;
				}

				xPath = "//cc:license";
				root = xDoc.DocumentElement;
				if (root != null)
				{
					XmlNode returnNode = root.SelectSingleNode(xPath, nsmgr);
					returnNode.InnerText = copyrightURL;
				}

				xPath = "//xapRights:WebStatement";
				root = xDoc.DocumentElement;
				if (root != null)
				{
					XmlNode returnNode = root.SelectSingleNode(xPath, nsmgr);
					returnNode.InnerText = copyrightURL;
				}
				xDoc.Save(destLicenseXml);

				string sourceJarFile = Common.PathCombine(getPsApplicationPath, "pdflicensemanager-2.3.jar");
				string destJarFile = Common.PathCombine(Path.GetDirectoryName(xhtmlFileName), "pdflicensemanager-2.3.jar");

				if (!File.Exists(destJarFile))
				{
					File.Copy(sourceJarFile, destJarFile, true);
				}

				string sourceExeFile = Common.PathCombine(getPsApplicationPath, "PdfLicense.exe");
				string destExeFile = Common.PathCombine(Path.GetDirectoryName(xhtmlFileName), "PdfLicense.exe");

				if (!File.Exists(destExeFile))
				{
					File.Copy(sourceExeFile, destExeFile, true);
				}

				string destPdfFile = Common.PathCombine(Path.GetDirectoryName(xhtmlFileName), Path.GetFileName(xhtmlFileName));
				string processFolder = Path.GetDirectoryName(destPdfFile);
				if (File.Exists(destExeFile) && creatorTool.ToLower().ToString() != "libreoffice")
				{
					SubProcess.RedirectOutput = processFolder;
					SubProcess.RunCommand(processFolder, destExeFile, "", true);
				}
			}
			catch { }
			// Ends the coding part for Copyright information added in PDF files

			return pdfFileName;
		}

		private static string CopyrightUrlValue()
		{
			string copyRightFilePath = Param.GetMetadataValue(Param.CopyrightPageFilename);
			if (copyRightFilePath.Trim().Length <= 0 && !File.Exists(copyRightFilePath))
			{
				return string.Empty;

			}
			if (File.Exists(copyRightFilePath))
			{
				if (Common.UnixVersionCheck())
				{
					string draftTempFileName = Path.GetFileName(copyRightFilePath);
					draftTempFileName = Common.PathCombine(Path.GetTempPath(), draftTempFileName);
					File.Copy(copyRightFilePath, draftTempFileName, true);
					return GetUrlFromCopyrightxhtmlFile(draftTempFileName);
				}
				else
				{
					return GetUrlFromCopyrightxhtmlFile(copyRightFilePath);
				}
			}
			else
			{
				return string.Empty;
			}

			return string.Empty;
		}

		private static string GetUrlFromCopyrightxhtmlFile(string xhtmlfileName)
		{
			XmlDocument xDoc = Common.DeclareXMLDocument(true);
			XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
			namespaceManager.AddNamespace("x", "http://www.w3.org/1999/xhtml");
			xDoc.Load(xhtmlfileName);
			XmlNode node;
			string xPath = "//x:a";
			try
			{
				node = xDoc.SelectSingleNode(xPath, namespaceManager);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				node = null;
			}
			if (node != null)
			{
				var atts = node.Attributes;
				if (atts != null)
				{
					if (atts["href"] != null)
						return (atts["href"].Value);
					else if (atts["xml:href"] != null)
						return (atts["xml:href"].Value);
				}
			}
			return string.Empty;
		}

		public static bool CheckExecutionPath()
		{
			return true;
		}

		/// <summary>
		/// Get the fontName from the Database.ssf file and insert it in css file
		/// </summary>
		public static string ParaTextFontName(string inputCssFileName)
		{
			string paraTextFontName = string.Empty;
			if (AppDomain.CurrentDomain.FriendlyName.ToLower() == "paratext.exe") // is paratext00
			{
				// read fontname from ssf
				StringBuilder newProperty = new StringBuilder();
				SettingsHelper settingsHelper = new SettingsHelper(Param.DatabaseName);
				string fileName = settingsHelper.GetSettingsFilename();
				string xPath = "//ScriptureText/DefaultFont";
				XmlNode xmlFont = Common.GetXmlNode(fileName, xPath);
				if (xmlFont == null || xmlFont.InnerText == string.Empty)
				{
					paraTextFontName = "Charis SIL";
				}
				else
				{
					paraTextFontName = xmlFont.InnerText;
				}
				if (inputCssFileName != string.Empty)
				{
					newProperty.AppendLine("div[lang='zxx']{ font-family: \"" + paraTextFontName + "\";}");
					newProperty.AppendLine("span[lang='zxx']{ font-family: \"" + paraTextFontName + "\";}");
					newProperty.AppendLine("@page{ font-family: \"" + paraTextFontName + "\";}");

					FileInsertText(inputCssFileName, newProperty.ToString());
				}
			}
			return paraTextFontName;
		}

		/// <summary>
		/// Get the LangCode from the Database.ssf file and insert it in css file
		/// </summary>
		public static string ParaTextEthnologueCodeName()
		{
			string paraLangCode = string.Empty;
			if (AppDomain.CurrentDomain.FriendlyName.ToLower() == "paratext.exe") // is paratext00
			{
				// read Language Code from ssf
				SettingsHelper settingsHelper = new SettingsHelper(Param.DatabaseName);
				string fileName = settingsHelper.GetSettingsFilename();
				string xPath = "//ScriptureText/EthnologueCode";
				XmlNode xmlLangCode = Common.GetXmlNode(fileName, xPath);
				if (xmlLangCode == null) return string.Empty;
				if (xmlLangCode != null && xmlLangCode.InnerText != string.Empty)
				{
					paraLangCode = xmlLangCode.InnerText;
				}
			}
			return paraLangCode;
		}

		/// <summary>
		/// Get the LangCode from the Database.ssf file and insert it in css file
		/// </summary>
		public static string ParaTextDcLanguage(string dataBaseName, bool hyphenlang)
		{
			string dcLanguage = string.Empty;
			if (AppDomain.CurrentDomain.FriendlyName.ToLower() == "paratext.exe") // is paratext00
			{
				// read Language Code from ssf
				SettingsHelper settingsHelper = new SettingsHelper(dataBaseName);
				string fileName = settingsHelper.GetSettingsFilename();
				string xPath = "//ScriptureText/EthnologueCode";
				XmlNode xmlLangCode = GetXmlNode(fileName, xPath);
				if (xmlLangCode != null && xmlLangCode.InnerText != string.Empty)
				{
					dcLanguage = xmlLangCode.InnerText;
				}
				xPath = "//ScriptureText/Language";
				XmlNode xmlLangNameNode = GetXmlNode(fileName, xPath);
				if (xmlLangNameNode != null && xmlLangNameNode.InnerText != string.Empty)
				{
					if (dcLanguage == string.Empty)
					{
						Dictionary<string, string> _languageCodes = new Dictionary<string, string>();
						_languageCodes = LanguageCodesfromXMLFile();
						if (_languageCodes.Count > 0)
						{
							foreach (
								var languageCode in
									_languageCodes.Where(
										languageCode => languageCode.Value.ToLower() == xmlLangNameNode.InnerText.ToLower()))
							{
								if (hyphenlang)
								{
									dcLanguage = _languageCodes.ContainsValue(languageCode.Key.ToLower())
										? _languageCodes.FirstOrDefault(x => x.Value == languageCode.Key.ToLower()).Key
										: languageCode.Key;
								}
								else
								{
									dcLanguage = languageCode.Key;
								}
								break;
							}
						}
					}
					dcLanguage += ":" + xmlLangNameNode.InnerText;
				}
			}
			return dcLanguage;
		}

		public static XmlDocument DeclareXMLDocument(bool applyWhiteSpace)
		{
			XmlDocument destDoc = null;

			if (UnixVersionCheck())
			{
				destDoc = new XmlDocument();
				destDoc.XmlResolver = FileStreamXmlResolver.GetNullResolver();
				if (applyWhiteSpace)
					destDoc.PreserveWhitespace = true;
			}
			else
			{
				if (applyWhiteSpace)
					destDoc = new XmlDocument { XmlResolver = null, PreserveWhitespace = true };
				else
					destDoc = new XmlDocument { XmlResolver = null };
			}
			return destDoc;
		}

		public static XmlTextReader DeclareXmlTextReader(string fileName, bool applyWhitespace)
		{
			XmlTextReader reader = null;

			if (UnixVersionCheck())
			{
				XmlUrlResolver resolver = new XmlUrlResolver();
				resolver.Credentials = CredentialCache.DefaultCredentials;

				if (applyWhitespace)
				{
					reader = new XmlTextReader(fileName);
					reader.XmlResolver = FileStreamXmlResolver.GetNullResolver();
					reader.WhitespaceHandling = WhitespaceHandling.Significant;
				}
				else
				{
					reader = new XmlTextReader(fileName);
					reader.XmlResolver = FileStreamXmlResolver.GetNullResolver();
				}
			}
			else
			{
				if (applyWhitespace)
					reader = new XmlTextReader(fileName)
						{
							XmlResolver = null,
							WhitespaceHandling = WhitespaceHandling.Significant
						};
				else
					reader = new XmlTextReader(fileName) { XmlResolver = null };

			}
			return reader;
		}

		public static bool UnixVersionCheck()
		{
			bool isRecentVersion = false;
			try
			{
				string getOSName = GetOsName();
				string majorVersion = string.Empty;
				if (getOSName.IndexOf("Unix") >= 0)
				{
					isRecentVersion = true;
					majorVersion = getOSName.Substring(0, 11);
				}
				if (majorVersion == "Unix 3.2.0.")
				{
					isRecentVersion = true;
				}
			}
			catch { }
			return isRecentVersion;
		}

		public static string RemoveDTDForLinuxProcess(string xhtmlFileNameWithPath, string exportType)
		{
			FileStream fs = new FileStream(xhtmlFileNameWithPath, FileMode.Open);
			StreamReader stream = new StreamReader(fs);

			string fileDir = Path.GetDirectoryName(xhtmlFileNameWithPath);
			string fileName = "Preserve" + Path.GetFileName(xhtmlFileNameWithPath);
			string Newfile = Common.PathCombine(fileDir, fileName);
			bool continueProcess = false;
			var fs2 = new FileStream(Newfile, FileMode.Create, FileAccess.Write);
			var sw2 = new StreamWriter(fs2);
			string line;
			while ((line = stream.ReadLine()) != null)
			{
				if (continueProcess)
				{
					sw2.WriteLine(line);
				}
				else
				{
					int htmlNodeStart = line.IndexOf("<html");
					if (htmlNodeStart >= 0)
					{
						int htmlNodeEnd = line.IndexOf(">", htmlNodeStart);
						string line1 = "<?xml version=\"1.0\" encoding=\"utf-8\"?> <!DOCTYPE html[]>";
						if (exportType == "epub")
						{
							line = line1 +
								   "<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:epub=\"http://www.idpf.org/2007/ops\" " +
								   line.Substring(htmlNodeEnd);
						}
						else
						{
							line = line1 + "<html " + line.Substring(htmlNodeEnd);
						}
						sw2.WriteLine(line);
						continueProcess = true;
					}
				}
			}
			stream.Close();
			sw2.Close();
			fs.Close();
			fs2.Close();

			File.Copy(xhtmlFileNameWithPath, xhtmlFileNameWithPath.Replace(".xhtml", "File.xhtml"), true);
			File.Copy(Newfile, Newfile.Replace("Preserve", ""), true);
			File.Delete(xhtmlFileNameWithPath.Replace(".xhtml", "File.xhtml"));
			File.Delete(Newfile);
			return xhtmlFileNameWithPath;
		}


		public static string AddingDTDForLinuxProcess(string xhtmlFileNameWithPath)
		{
			FileStream fs = new FileStream(xhtmlFileNameWithPath, FileMode.Open);
			StreamReader stream = new StreamReader(fs);

			string fileDir = Path.GetDirectoryName(xhtmlFileNameWithPath);
			string fileName = "Tmp" + Path.GetFileName(xhtmlFileNameWithPath);
			string Newfile = Common.PathCombine(fileDir, fileName);
			bool continueProcess = false;
			var fs2 = new FileStream(Newfile, FileMode.Create, FileAccess.Write);
			var sw2 = new StreamWriter(fs2);
			string line;
			while ((line = stream.ReadLine()) != null)
			{
				if (continueProcess)
				{
					sw2.WriteLine(line);
				}
				else
				{
					int htmlNodeStart = line.IndexOf("<html");
					if (htmlNodeStart >= 0)
					{
						int htmlNodeEnd = line.IndexOf(">", htmlNodeStart);
						string line1 = "<?xml version='1.0' encoding='utf-8'?>";
						line = line1 + "<html xmlns='http://www.w3.org/1999/xhtml'" + line.Substring(htmlNodeEnd);
						sw2.WriteLine(line);
						continueProcess = true;
					}
				}
			}
			stream.Close();
			sw2.Close();
			fs.Close();
			fs2.Close();

			File.Copy(xhtmlFileNameWithPath, xhtmlFileNameWithPath.Replace(".xhtml", "File.xhtml"), true);
			File.Copy(Newfile, Newfile.Replace("Tmp", ""), true);
			File.Delete(xhtmlFileNameWithPath.Replace(".xhtml", "File.xhtml"));
			File.Delete(Newfile);
			return xhtmlFileNameWithPath;
		}

		public static string ReadingCommandPromptOutputValue(string appName, string arguments)
		{
			string cmdOutputValue = string.Empty;
			System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
			psi.FileName = appName;
			psi.UseShellExecute = false;
			psi.RedirectStandardOutput = true;
			psi.Arguments = arguments;
			Process p = Process.Start(psi);
			string strOutput = p.StandardOutput.ReadToEnd();
			cmdOutputValue = strOutput;
			p.WaitForExit();
			return cmdOutputValue;
		}

		public static bool IsAlphanumeric(string value)
		{
			bool result = false;
			char[] arr = value.ToCharArray();
			for (int i = 0; i < arr.Length; i++)
			{
				if (char.IsLetter(arr[i]))
				{
					result = true;
				}
			}
			return result;
		}

		public static int GetNumberFromAlphaNumeric(string value)
		{
			string result = string.Empty;
			char[] arr = value.ToCharArray();
			for (int i = 0; i < arr.Length; i++)
			{
				if (!char.IsLetter(arr[i]))
				{
					result = result + arr[i].ToString();
				}
			}
			return int.Parse(result);
		}

		public static bool NodeExists(string fileName, string searchClassname)
		{
			bool result = false;
			XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
			XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			xmlDocument.Load(fileName);
			XmlNodeList nodes = xmlDocument.SelectNodes("//xhtml:span[@class='Note_Target_Reference']", namespaceManager);
			if (nodes.Count > 0)
			{
				result = true;
			}
			return result;
		}

		public static string GetParatextProjectPath()
		{
			var windowsIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
			if (windowsIdentity != null)
			{
				string userName = windowsIdentity.Name;
				string registryPath = "/home/" + userName + "/.config/paratext/registry/LocalMachine/software/scrchecks/1.0/settings_directory/";
				while (Directory.Exists(registryPath))
				{
					if (File.Exists(Common.PathCombine(registryPath, "values.xml")))
					{
						XmlDocument doc = new XmlDocument();
						doc.Load(Common.PathCombine(registryPath, "values.xml"));
						return doc.InnerText;
					}
				}
			}

			return string.Empty;
		}

		/// <summary>
		/// Determine picture width based on page with and number of columns
		/// </summary>
		/// <param name="cssClass">structure containng css information</param>
		/// <param name="inputType">dictionary or scripture - determines where to look for number of columns in css</param>
		/// <returns></returns>
		public static int GetPictureWidth(Dictionary<string, Dictionary<string, string>> cssClass, string inputType)
		{
			double pageMarginLeft = 0;
			double pageMarginRight = 0;
			double pageWidth = 325;
			int pictureWidth = 325;
			int count = 1;
			if (inputType.ToLower() == "scripture")
			{
				if (cssClass.ContainsKey("columns") && cssClass["columns"].ContainsKey("column-count"))
					count = Convert.ToInt16(cssClass["columns"]["column-count"], CultureInfo.GetCultureInfo("en-US"));
			}
			else
			{
				if (cssClass.ContainsKey("letData") && cssClass["letData"].ContainsKey("column-count"))
					count = Convert.ToInt16(cssClass["letData"]["column-count"], CultureInfo.GetCultureInfo("en-US"));
			}

			if (cssClass.ContainsKey("@page"))
			{
				try
				{
					if (cssClass["@page"].ContainsKey("page-width"))
						pageWidth = Convert.ToDouble(cssClass["@page"]["page-width"], CultureInfo.GetCultureInfo("en-US"));

					if (cssClass["@page"].ContainsKey("margin-left"))
						pageMarginLeft = Convert.ToDouble(cssClass["@page"]["margin-left"], CultureInfo.GetCultureInfo("en-US"));

					if (cssClass["@page"].ContainsKey("margin-right"))
						pageMarginRight = Convert.ToDouble(cssClass["@page"]["margin-right"], CultureInfo.GetCultureInfo("en-US"));

					pageWidth = pageWidth - (pageMarginLeft + pageMarginRight);
					pictureWidth = Convert.ToInt32(pageWidth);

					if (count > 1)
					{
						pictureWidth = pictureWidth / 2;
					}
				}
				catch (Exception)
				{
					pictureWidth = 325;
				}
			}
			return pictureWidth;
		}

		/// <summary>
		/// check the read permission has the registry
		/// </summary>
		/// <param name="registry">SOFTWARE\\Wow6432Node\\LibreOffice\\UNO\\InstallPath</param>
		/// <returns>true or false</returns>
		public static bool RegistryCanRead(string registry)
		{
			bool isPermission;
			try
			{
				RegistryPermission perm1 = new RegistryPermission(RegistryPermissionAccess.Read, registry);
				perm1.Demand();
				isPermission = true;
			}
			catch (System.Security.SecurityException ex)
			{
				isPermission = false;
			}
			return isPermission;
		}

		public static string GetLibreofficeVersion(string osName)
		{
			string libreofficeVersion = null;
			try
			{
				if (osName.Contains("Windows"))
				{
					libreofficeVersion = GetValueFromRegistry("SOFTWARE\\Wow6432Node\\LibreOffice\\UNO\\InstallPath", "");
					if (string.IsNullOrEmpty(libreofficeVersion))
					{ // Handle 32-bit Windows 7 and XP
						libreofficeVersion = GetValueFromRegistry("SOFTWARE\\LibreOffice\\UNO\\InstallPath", "");
					}
				}
			}
			catch { }
			return libreofficeVersion;
		}

		public static bool CreateLicenseFileForRunningPdfApplyCopyright(string tempDirectoryFolder, string workingDirectoryXhtmlFileName, string exportTitle, string creatorTool, string inputType)
		{
			bool isCreated = false;

			string allUserPath = GetAllUserPath();
			string fileLoc = Common.PathCombine(allUserPath, "License.txt");
			FileStream fs = null;
			if (!File.Exists(fileLoc))
			{
				using (fs = File.Create(fileLoc))
				{

				}
			}
			if (File.Exists(fileLoc))
			{
				using (StreamWriter sw = new StreamWriter(fileLoc))
				{
					sw.WriteLine(tempDirectoryFolder);
					sw.WriteLine(workingDirectoryXhtmlFileName);
					sw.WriteLine(exportTitle);
					sw.WriteLine(creatorTool);
					sw.WriteLine(inputType);
					sw.WriteLine("Common.Testing" + Common.Testing.ToString());
				}
				isCreated = true;
			}

			return isCreated;
		}

		public static string UpdateCopyrightYear(string textFromXML)
		{
			if (textFromXML.Trim().Length == 0) return string.Empty;

			string currentYear = Common.ConvertUnicodeToString("\\00a9") + " " + DateTime.Now.Year.ToString() + " SIL International" + Common.ConvertUnicodeToString("\\00ae") + ".";
			string[] value = textFromXML.Split(' ');
			if (value.Length > 1)
			{
				if (value[1].StartsWith("20") && value[1].ToString() != DateTime.Now.Year.ToString())
				{
					currentYear = textFromXML.Replace(value[1], DateTime.Now.Year.ToString());
				}
				else
				{
					return textFromXML;
				}
			}
			else
			{
				return textFromXML;
			}
			return currentYear;
		}

		public static Dictionary<string, string> LanguageCodesfromXMLFile()
		{
			Dictionary<string, string> _isoLanguage = new Dictionary<string, string>();

			string xmlFilePath = CopyXmlFileToTempDirectory("RampLangCode.xml");
			if (!File.Exists(xmlFilePath))
				return null;

			XmlDocument xDoc = Common.DeclareXMLDocument(true);
			xDoc.Load(xmlFilePath);
			const string twoLetterLangXPath = "//LanguageCode/IsoLanguageCodeTwoLetters/Language";
			const string threeLetterLangXPath = "//LanguageCode/IsoLanguageCodeThreeLetters/Language";
			const string scriptLangXPath = "//LanguageCode/LanguageScript/Language";

			XmlNodeList twoLetterLangList = xDoc.SelectNodes(twoLetterLangXPath);
			if (twoLetterLangList != null && twoLetterLangList.Count > 0)
			{
				foreach (XmlNode node in twoLetterLangList)
				{
					_isoLanguage.Add(node.Attributes["name"].Value, node.Attributes["value"].Value);
				}
			}

			XmlNodeList threeLetterLangList = xDoc.SelectNodes(threeLetterLangXPath);
			if (threeLetterLangList != null && threeLetterLangList.Count > 0)
			{
				foreach (XmlNode node in threeLetterLangList)
				{
					_isoLanguage.Add(node.Attributes["name"].Value, node.Attributes["value"].Value);
				}
			}

			XmlNodeList scriptLangList = xDoc.SelectNodes(scriptLangXPath);
			if (scriptLangList != null && scriptLangList.Count > 0)
			{
				foreach (XmlNode node in scriptLangList)
				{
					_isoLanguage.Add(node.Attributes["name"].Value, node.Attributes["scriptname"].Value);
				}
			}
			return _isoLanguage;
		}

		public static string CopyXmlFileToTempDirectory(string fileName)
		{
			string tempXmlFile = string.Empty;
			string psSupportPath = GetPSApplicationPath();
			string xmlFileNameWithPath = Common.PathCombine(psSupportPath, fileName);
			if (File.Exists(xmlFileNameWithPath))
			{
				string tempFolder = Common.PathCombine(Path.GetTempPath(), "SILTemp");
				if (Directory.Exists(tempFolder))
				{
					try
					{
						var di = new DirectoryInfo(tempFolder);
						Common.CleanDirectory(di);
					}
					catch
					{
						tempFolder = Common.PathCombine(Path.GetTempPath(),
														"SilPathWay" +
														Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
					}
				}
				Directory.CreateDirectory(tempFolder);
				tempXmlFile = Common.PathCombine(tempFolder, fileName);

				File.Copy(xmlFileNameWithPath, tempXmlFile, true);
			}
			return tempXmlFile;
		}

		/// <summary>
		/// For dictionary data, returns the language code for the definitions
		/// </summary>
		/// <returns></returns>
		public static void WriteDefaultLanguages(PublicationInformation projInfo, string cssFileName)
		{
			//Read Default Languages from XHTML
			//<meta name="ggo-Telu-IN" content="Gautami" scheme="language to font"/>
			try
			{
				string fontSize = string.Empty;
				Dictionary<string, string> xhtmlMetaLanguage = new Dictionary<string, string>();
				//var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
				//if (fileNameWithoutExtension != null)
				//{
				//    string fileName = fileNameWithoutExtension.ToLower();
				//    if (fileName != "main" && fileName != "main1")
				//        return;
				//}

				var xDoc = Common.DeclareXMLDocument(true);
				xDoc.Load(projInfo.DefaultXhtmlFileWithPath);
				XmlNodeList nodeList = xDoc.GetElementsByTagName("meta");
				if (nodeList.Count > 0)
				{
					if (projInfo.ProjectInputType.ToLower() == "dictionary")
					{
						for (int i = 0; i < nodeList.Count; i++)
						{
							XmlNode node = nodeList[i];
							if (node.Attributes != null)
							{
								if (node.Attributes["scheme"] == null) continue;
								string className = node.Attributes["scheme"].Value;
								if (className == "language to font")
								{
									xhtmlMetaLanguage.Add(node.Attributes["name"].Value, node.Attributes["content"].Value);
								}
							}
						}
					}
					else
					{
						for (int i = 0; i < nodeList.Count; i++)
						{
							XmlNode node = nodeList[i];
							if (node.Attributes != null)
							{
								if (node.Attributes["name"] == null) continue;
								string className = node.Attributes["name"].Value;
								if (className == "fontName")
								{
									xhtmlMetaLanguage.Add(node.Attributes["name"].Value, node.Attributes["content"].Value);
								}
								else if (className == "fontSize")
								{
									if (float.Parse(node.Attributes["content"].Value) < projInfo.DefaultFontSize)
										fontSize = node.Attributes["content"].Value;
								}
							}
						}
					}
				}

				//Write the default languages in the CSS file.
				if (xhtmlMetaLanguage.Count == 0) return;

				StringBuilder cssProperty = new StringBuilder();
				foreach (KeyValuePair<string, string> kvp in xhtmlMetaLanguage)
				{
					cssProperty.AppendLine("span[lang='" + kvp.Key + "'] {");
					cssProperty.AppendLine("font-family: \"" + kvp.Value + "\";");
					if (fontSize != string.Empty)
					{
						cssProperty.AppendLine("font-size: \"" + fontSize + "\";");
					}
					cssProperty.AppendLine("}");

				}

				FileInsertText(cssFileName, cssProperty.ToString());
			}
			catch { }
		}

		/// <summary>
		/// Replace symbols(/$#@!%&^**_)) to space in the input string
		/// </summary>
		public static string ReplaceSymbolToUnderline(string inputText)
		{
			Regex regex = new Regex(@"[^a-zA-Z0-9\s]", (RegexOptions)0);
			inputText = regex.Replace(inputText, "_");
			return inputText;
		}

		public static void ApplyXslt(string fileFullPath, XslCompiledTransform xslt)
		{
			var folder = Path.GetDirectoryName(fileFullPath);
			var name = Path.GetFileNameWithoutExtension(fileFullPath);
			var tempFullName = Common.PathCombine(folder, name) + "-1.xml";
			File.Copy(fileFullPath, tempFullName);

			XmlTextReader reader = Common.DeclareXmlTextReader(tempFullName, true);
			FileStream xmlFile = new FileStream(fileFullPath, FileMode.Create);
			XmlWriter writer = XmlWriter.Create(xmlFile, xslt.OutputSettings);
			xslt.Transform(reader, null, writer, null);
			xmlFile.Close();
			reader.Close();

			File.Delete(tempFullName);
		}

		public static string UsersXsl(string xslName)
		{
			var myPath = Common.PathCombine(GetAllUserPath(), xslName);
			if (File.Exists(myPath))
				return myPath;
			return FromRegistry(xslName);
		}

		public static string ExecuteWebAPIRequest(string apiRequestURL, string postData)
		{
			System.Net.WebRequest webRequest = System.Net.WebRequest.Create(apiRequestURL);
			webRequest.Method = "POST";
			webRequest.ContentType = "text/html; charset=utf-8";
			Stream reqStream = webRequest.GetRequestStream();
			byte[] postArray = Encoding.ASCII.GetBytes(postData);
			reqStream.Write(postArray, 0, postArray.Length);
			reqStream.Close();
			StreamReader sr = new StreamReader(webRequest.GetResponse().GetResponseStream());
			string responseContent = sr.ReadToEnd();
			return responseContent;
		}

		public static string HandleSpaceinLinuxPath(string filePath)
		{
			var name = new List<string>();
			string[] ss = filePath.Split('/');
			foreach (string variable in ss)
			{
				if (variable.IndexOf(' ') > 0)
				{
					name.Add(variable);
				}
			}

			if (name.Count > 0)
			{
				foreach (var variable in name)
				{
					filePath = filePath.Replace(variable, "'" + variable + "'");
				}
			}
			return filePath;
		}
		#region "Localization"
		public static void SaveLocalizationSettings(string setting, string fontname, string fontsize)
		{
			var xmlDoc = new XmlDocument();
			string fileName = PathCombine(GetAllUserAppPath(), @"SIL\Pathway\UserInterfaceLanguage.xml");
			if (File.Exists(fileName))
			{
				var content = File.ReadAllText(fileName);
				xmlDoc.LoadXml(content);
				var uiValueNode = xmlDoc.SelectSingleNode("//UILanguage/string");
				if (uiValueNode != null) uiValueNode.InnerText = setting;

				if (fontname != null && fontsize != null)
				{
					XmlNode fontNode = xmlDoc.SelectSingleNode("//UILanguage/fontstyle/font[@lang='" + setting + "']");
					if (fontNode != null && fontNode.Attributes != null)
					{
						fontNode.Attributes["name"].InnerText = fontname;
						fontNode.Attributes["size"].InnerText = fontsize;
					}
					else
					{
						fontNode = xmlDoc.SelectSingleNode("//UILanguage/fontstyle/font");
						var newChildElement = xmlDoc.CreateElement("font");
						newChildElement.SetAttribute("lang", setting);
						newChildElement.SetAttribute("name", fontname);
						newChildElement.SetAttribute("size", fontsize);
						if (fontNode != null && fontNode.ParentNode != null)
							fontNode.ParentNode.InsertAfter(newChildElement, fontNode);
					}
				}
				xmlDoc.Save(fileName);
			}
			else
			{
				CreateUserInterfaceLanguagexml();
			}
		}

		public static void CreateUserInterfaceLanguagexml()
		{
			string fileName = Common.PathCombine(Common.GetAllUserAppPath(), @"SIL\Pathway\UserInterfaceLanguage.xml");
			string fontName = "Microsoft Sans Serif";
			string fontSize = "8";
			if (IsUnixOS())
			{
				fontName = "Liberation Serif";
				fontSize = "8";
			}
			if (!File.Exists(fileName))
			{
				string getDirectoryName = Path.GetDirectoryName(fileName);

				if (Directory.Exists(getDirectoryName))
				{
					CreateLanguageFontSettings(fileName, fontName, fontSize);
				}
				else
				{
					Directory.CreateDirectory(getDirectoryName);
					CreateLanguageFontSettings(fileName, fontName, fontSize);
				}
			}
		}

		private static void CreateLanguageFontSettings(string fileName, string fontName, string fontSize)
		{
			using (XmlWriter writer = XmlWriter.Create(fileName))
			{
				writer.WriteStartElement("UILanguage");
				writer.WriteElementString("string", "en");
				writer.WriteStartElement("fontstyle");
				writer.WriteStartElement("font");
				writer.WriteAttributeString("lang", "en");
				writer.WriteAttributeString("name", fontName);
				writer.WriteAttributeString("size", fontSize);
				writer.WriteEndElement();
				writer.WriteEndElement();
				writer.Flush();
				writer.Close();
			}
		}

		public static string GetLocalizationSettings()
		{
			try
			{
				string fontName = "Microsoft Sans Serif";
				string fontSize = "8";
				if (IsUnixOS())
				{
					fontName = "Liberation Serif";
					fontSize = "8";
				}
				var fileName = PathCombine(GetAllUserAppPath(), @"SIL\Pathway\UserInterfaceLanguage.xml");
				if (!File.Exists(fileName))
				{
					SaveLocalizationSettings("en", fontName, fontSize);
				}

				if (File.Exists(fileName))
				{
					var content = File.ReadAllText(fileName);
					var doc = new XmlDocument();
					doc.LoadXml(content);
					var node = doc.SelectSingleNode("//string");
					return (node != null && node.InnerText.Trim().Length > 0) ? node.InnerText : "en";
				}
				else
				{
					return "en";
				}
			}
			catch (Exception)
			{
				return "en";
			}
		}

		private static string InstalledLocalizations()
		{
			string pathwayDirectory = PathwayPath.GetPathwayDir();
			var installedLocalizationsFolder = string.Empty;
			if (pathwayDirectory != null)
			{
				installedLocalizationsFolder = Path.Combine(pathwayDirectory, "localizations");
			}
			else
			{
				installedLocalizationsFolder = Path.Combine(Application.StartupPath, "localizations");
			}

			return installedLocalizationsFolder;
		}

		public static void SetupLocalization()
		{
			if (!Testing)
			{
				var namespacebeginnings = GetnamespacestoLocalize();
				if (namespacebeginnings == null) return;
				Assembly assembly = Assembly.GetExecutingAssembly();
				FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
				var companyName = fvi.CompanyName;
				var productName = fvi.ProductName;
				var productVersion = fvi.ProductVersion.Trim();

				//var installedStringFileFolder = FileLocator.GetDirectoryDistributedWithApplication("localization");
				var targetTmxFilePath = Path.Combine(kCompany, kProduct);
				string installedLocalizationsFolder = InstalledLocalizations();
				var desiredUiLangId = GetLocalizationSettings();
				if (desiredUiLangId == string.Empty)
					desiredUiLangId = "en";
				if (string.IsNullOrEmpty(productVersion))
				{
					productVersion = Application.ProductVersion;
				}

				try
				{
					L10NMngr = LocalizationManager.Create(desiredUiLangId, productName, productName, productVersion,
	installedLocalizationsFolder, targetTmxFilePath, null, IssuesEmailAddress, namespacebeginnings);
				}
				catch (Exception ex)
				{
					Console.WriteLine("==========================================");
					Console.WriteLine(ex.Message);
					Console.WriteLine("==========================================");
				}
				LocalizationManager.SetUILanguage(desiredUiLangId, true);
			}
		}

		public static void InitializeOtherProjects()
		{
			string pathwayDirectory = PathwayPath.GetPathwayDir();
			if (pathwayDirectory == null || !Directory.Exists(pathwayDirectory)) return;

			foreach (var file in Directory.GetFiles(pathwayDirectory, "*.*").Where(f => Regex.IsMatch(f, @"^.+\.(dll|exe)$")))
			{
				var fileInfo = new FileInfo(file);
				if ((fileInfo.Name == "PsTool.dll") || (fileInfo.Name.Contains("Convert")) ||
					(fileInfo.Name.Contains("Writer")) || (fileInfo.Name.Contains("Validator")))
				{
					try
					{
						using (var epubinstalleddirectory = File.OpenRead(Common.FromRegistry(fileInfo.FullName)))
						{

							var sAssembly = Assembly.LoadFrom(epubinstalleddirectory.Name);

							foreach (
								var stype in
									sAssembly.GetTypes()
										.Where(type => type.GetConstructors().Any(s => s.GetParameters().Length == 0)))
							{
								sAssembly.CreateInstance(stype.FullName);
							}

						}
					}
					catch
					{
					}
				}
			}
			GC.Collect();
		}

		public static string[] GetnamespacestoLocalize()
		{
			var namespacestoLocalize = new List<string>();
			var pathwayDirectory = PathwayPath.GetPathwayDir();
			if (pathwayDirectory == null || !Directory.Exists(pathwayDirectory))
				return new[] { "SIL.PublishingSolution" };
			foreach (var file in Directory.GetFiles(pathwayDirectory, "*.*").Where(f => Regex.IsMatch(f, @"^.+\.(dll|exe)$"))
				)
			{
				var fileInfo = new FileInfo(file);
				if ((fileInfo.Name == "PsTool.dll") || (fileInfo.Name.Contains("Convert")) ||
					(fileInfo.Name.Contains("Writer")) || (fileInfo.Name.Contains("Validator")))
				{
					using (var epubinstalleddirectory = File.OpenRead(Common.FromRegistry(fileInfo.FullName)))
					{

						var sAssembly = Assembly.LoadFrom(epubinstalleddirectory.Name);

						foreach (
							var stype in
								sAssembly.GetTypes()
									.Where(type => type.GetConstructors().Any(s => s.GetParameters().Length == 0)))
						{
							if (!namespacestoLocalize.Contains(stype.Namespace))
								namespacestoLocalize.Add(stype.Namespace);
						}

					}

				}
			}
			return namespacestoLocalize.Distinct().ToArray();
		}

		#region Localization Manager Access methods
		/// ------------------------------------------------------------------------------------
		public static LocalizationManager L10NMngr { get; set; }

		///// ------------------------------------------------------------------------------------
		//internal static void SaveOnTheFlyLocalizations()
		//{
		//    if (L10NMngr != null)
		//        L10NMngr.SaveOnTheFlyLocalizations();
		//}

		///// ------------------------------------------------------------------------------------
		//internal static void ReapplyLocalizationsToAllObjects(string localizationManagerID)
		//{
		//    LocalizationManager.ReapplyLocalizationsToAllObjectsInAllManagers();
		//    //if (L10NMngr != null)
		//    //    LocalizationManager.ReapplyLocalizationsToAllObjects(localizationManagerID);
		//}

		/// ------------------------------------------------------------------------------------
		internal static void RefreshToolTipsOnLocalizationManager()
		{
			if (L10NMngr != null)
				L10NMngr.RefreshToolTips();
		}

		/// ------------------------------------------------------------------------------------
		internal static string GetUILanguageId()
		{
			return LocalizationManager.UILanguageId;
		}

		#endregion

		#region Hyphenation Settings

		public static void EnableHyphenation()
		{
			try
			{
				OperatingSystem OS = Environment.OSVersion;
				if (OS.ToString().IndexOf("Microsoft") == 0)
				{
					string strFilePath;
					string strPath = @"SOFTWARE\Classes\.odt\LibreOffice.WriterDocument.1\ShellNew\";
					RegistryKey regKeyAppRoot = Registry.LocalMachine.OpenSubKey(strPath);
					strFilePath = (string)regKeyAppRoot.GetValue("FileName");
					strFilePath = strFilePath.Replace(@"template\shellnew\soffice.odt", @"extensions\");
					string[] files = Directory.GetFiles(strFilePath, "hyph*.dic", SearchOption.AllDirectories);
					if (files.Length > 0)
					{
						foreach (var lang in Param.HyphenLang.Split(','))
						{
							if (String.IsNullOrEmpty(lang))
								break;

							foreach (var file in files)
							{
								if (file.Split(new char[] { Convert.ToChar("_"), '.' })[1] ==
									lang.Substring(0, lang.IndexOf(":", StringComparison.Ordinal)))
								{
									Param.IsHyphen = true;
									Param.HyphenationSelectedLanguagelist.Add(lang);
								}
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}

		}

		#endregion
		public static string GetPalasoLanguageName(string lang)
		{
			var lpModel = new EthnologueLookup();
			var matchLang = lpModel.SuggestLanguages(lang.ToLower()).FirstOrDefault();
			string langname = null;
			if (matchLang != null)
				langname = matchLang.DesiredName;
			return langname ?? string.Empty;
		}
		#endregion


		public static Cursor UseWaitCursor()
		{
			var myCursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			return myCursor;
		}

		public static InProcess SetupProgressReporting(int steps, string exportMsg)
		{
			var inProcess = new InProcess(0, steps) { Text = exportMsg }; // create a progress bar with 7 steps (we'll add more below)
			inProcess.Text = LocalizationManager.GetString("ProgressTitle.InProcessWindow.Title", exportMsg, "");
			inProcess.Show();
			inProcess.ShowStatus = true;
			return inProcess;
		}

	}
}
