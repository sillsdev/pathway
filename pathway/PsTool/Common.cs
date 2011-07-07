// --------------------------------------------------------------------------------------------
// <copyright file="Common.cs" from='2009' to='2009' company='SIL International'>
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
// Library for Dictionary Express
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Microsoft.Win32;
using SIL.Tool.Localization;

#endregion Using

namespace SIL.Tool
{
    /// <summary>
    /// Common Library for Dictionary Express
    /// </summary>
    public static partial class Common
    {
        #region Public variable
        public enum Action { New, Delete, Edit, Copy };
        public enum ProjType { Dictionary, Scripture };
        public enum OutputType { ODT, ODM, IDML , PDF , MOBILE, EPUB, XETEX, XELATEX};
        public static string SamplePath = string.Empty;
        #endregion

        //public static string SepParent = "-";
        //public static string SepAncestor = "--";
        //public static string sepPrecede = "_";
        //public static string SepAttrib = "_.";
        //public static string SepTag = ".";
        //public static string SepPseudo = "..";


        public static string SepParent = "_";
        public static string SepAncestor = ".-";
        public static string sepPrecede = "-";
        public static string SepAttrib = "_.";
        public static string SepTag = ".";
        public static string SepPseudo = "..";
        public static string Space = " ";

        static readonly ArrayList _units = new ArrayList();
        public static ErrorProvider _errProvider = new ErrorProvider();

        public static HelpProvider HelpProv = new HelpProvider();
        public static Font UIFont;
        public static OdtType OdType;
        public static double ColumnWidth = 0.0;
        public static string errorMessage = string.Empty;
        public static string databaseName = string.Empty;
        public static DateTime TimeStarted { get; set; }
        public static OutputType _outputType = OutputType.ODT;
        public enum FileType
        {
            Directory, DirectoryExcluded, File, FileExcluded, Project
        }

        public enum OdtType
        {
            OdtChild, OdtMaster, OdtNoMaster
        }
        public enum ProjectType
        {
            Dictionary, Scripture
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
        public static bool fromPlugin; // To differentiate between Plugin or from UI


        #region FillName(string cssFileWithPath)
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// This method collects css files names into ArrayList based on base CSS File.
        /// <param name="cssFileWithPath">Its gets the file path of the CSS File</param>
        /// <returns>ArrayList contains CSS filenames which are used</returns>
        /// -------------------------------------------------------------------------------------------
        public static ArrayList GetCSSFileNames(string cssFileWithPath, string BaseCssFileWithPath)
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
                if (BaseCssFileWithPath != cssFileWithPath)
                {
                    arrayCSSFile.Add(cssFileWithPath);
                }
                while ((strText = sr.ReadLine()) != null)
                {
                    if (strText.Contains("@import"))
                    {
                        string cssFile = strText.Substring((strText.IndexOf('"') + 1), strText.LastIndexOf('"') - (strText.IndexOf('"') + 1));
                        //if (File.Exists(PathCombine(cssPath, cssFile)))
                        //{
                        //    arrayCSSFile.AddRange(GetCSSFileNames(PathCombine(cssPath, cssFile), BaseCssFileWithPath));
                        //}
                        if (!File.Exists(PathCombine(cssPath, cssFile)) && SamplePath.Length > 0)
                        {
                            string executablePath = Path.GetDirectoryName(Application.ExecutablePath);
                            if (executablePath.Contains("ReSharper") || executablePath.Contains("NUnit"))
                            {
                                //This code will work when this method call from NUnit Test case
                                int binFolderPart = Environment.CurrentDirectory.IndexOf(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar);
                                executablePath = DirectoryPathReplace(Environment.CurrentDirectory.Substring(0, binFolderPart) + "/ConfigurationTool/TestFiles/input");
                            }
                            else if (executablePath.Contains("FieldWorks 7") || executablePath.Contains("FieldWorks"))
                            {
                                //Change the path which have the default styles
                                string folderName = Path.GetDirectoryName(executablePath);
                                executablePath = PathCombine(folderName, "Pathway7");
                            }
                            else if (executablePath.Contains("Paratext 7"))
                            {
                                //Change the path which have the default styles
                                string folderName = LeftString(executablePath, "Paratext 7");
                                executablePath = DirectoryPathReplace(PathCombine(folderName, "SIL\\Pathway7"));
                            }
                            cssPath = PathCombine(executablePath, SamplePath);
                        }
                        arrayCSSFile.AddRange(GetCSSFileNames(PathCombine(cssPath, cssFile), BaseCssFileWithPath));
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
        /// <summary>
        /// Looks up the text direction for the specified language code in the appropriate .ldml file.
        /// This lookup will not work with Paratext, which does not yet use an .ldml file.
        /// </summary>
        /// <param name="language">ISO 639 language code.</param>
        /// <returns>Text direction (ltr or rtl), or ltr if not found.</returns>
        public static string GetTextDirection(string language)
        {
            string[] langCoun = language.Split('-');
            string direction;

            try
            {
                string wsPath;
                if (langCoun.Length < 2)
                {
                    // try the language (no country code) (e.g, "en" for "en-US")
                    wsPath = Common.PathCombine(Common.GetAllUserAppPath(), "SIL/WritingSystemStore/" + langCoun[0] + ".ldml");
                }
                else
                {
                    // try the whole language expression (e.g., "ggo-Telu-IN")
                    wsPath = Common.PathCombine(Common.GetAllUserAppPath(), "SIL/WritingSystemStore/" + language + ".ldml");
                }
                if (File.Exists(wsPath))
                {
                    var ldml = new XmlDocument { XmlResolver = null };
                    ldml.Load(wsPath);
                    var nsmgr = new XmlNamespaceManager(ldml.NameTable);
                    nsmgr.AddNamespace("palaso", "urn://palaso.org/ldmlExtensions/v1");
                    var node = ldml.SelectSingleNode("//orientation/@characters", nsmgr);
                    if (node != null)
                    {
                        // get the text direction specified by the .ldml file
                        direction = (node.Value.ToLower().Equals("right-to-left")) ? "rtl" : "ltr";
                    }
                    else
                    {
                        direction = "ltr";
                    }
                }
                else
                {
                    direction = "ltr";
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
                        wsPath = PathCombine(GetAllUserAppPath(), "SIL/WritingSystemStore/" + langCoun[0] + ".ldml");
                    }
                    else
                    {
                        // try the whole language expression (e.g., "ggo-Telu-IN")
                        wsPath = PathCombine(GetAllUserAppPath(), "SIL/WritingSystemStore/" + languageCode + ".ldml");
                    }
                    if (File.Exists(wsPath))
                    {
                        var ldml = new XmlDocument { XmlResolver = null };
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

        #region PublishingSolutionsEnvironmentReset()
        /// <summary>
        /// Remove all files saved in All Users\AppData, 
        /// (Executing this method when these files are expected to be present may cause a crash).
        /// Intended for use during testing.
        /// </summary>
        public static void PublishingSolutionsEnvironmentReset()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string psSettings = appDataDir + @"/SIL/Pathway";
            //string psSettings = PathCombine(appDataDir,"SIL/Pathway");
            //if (Directory.Exists(psSettings))
            //    Directory.Delete(psSettings, true);
            DeleteDirectory(psSettings);
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
            bool result = true;
            if (string.IsNullOrEmpty(stringValue.Trim()))
            {
                result = false;
            }
            //else
            //{
            //    var rx = new Regex(@"^[a-zA-Z]");
            //    if (!rx.IsMatch(stringValue.Trim()))
            //    {
            //        result = false;
            //    }
            //}
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
                attributeValue = float.Parse(attrib);
                string attributeUnit = attribute.Substring(counter);
                if (attributeUnit == "cm")
                {
                    attributeValue = float.Parse(attrib) * 0.3937008F;
                }
                else if (attributeUnit == "pt")
                {
                    attributeValue = float.Parse(attrib) / 72F;
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
        /// <example UnitConverter("12px", "pt") => 8pt
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
                attributeValue = float.Parse(attrib);
                string attributeUnit = inputValue.Substring(counter) + "To" + outputUnit.ToLower();

                //if (inputValue.Substring(counter) == outputUnit.ToLower())
                //{
                //    return attributeValue;
                //}

                if (attributeUnit == "pcTopt")
                {
                    attributeValue = float.Parse(attrib) * 12;
                }
                else if (attributeUnit == "pxTopt")
                {
                    attributeValue = float.Parse(attrib) * 0.75F;
                }
                else if (attributeUnit == "inTopt")
                {
                    attributeValue = float.Parse(attrib) * 72F;
                }
                else if (attributeUnit == "cmTopt")
                {
                    attributeValue = float.Parse(attrib) * 28.346456693F;
                }
                else if (attributeUnit == "cmToin")
                {
                    attributeValue = float.Parse(attrib) * 0.3937008F;
                }
                else if (attributeUnit == "inTocm")
                {
                    attributeValue = float.Parse(attrib) / 0.3937008F;
                }
                else if (attributeUnit == "ptToin")
                {
                    attributeValue = float.Parse(attrib) / 72F;
                }
                else if (attributeUnit == "ptTocm")
                {
                    attributeValue = float.Parse(attrib) / 28.346456693F;
                }
                else if (attributeUnit == "pcToin")
                {
                    attributeValue = float.Parse(attrib) * 0.1666666667F;
                }

                else if (attributeUnit == "exToem")
                {
                    attributeValue = float.Parse(attrib) / 2F;
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
                float attrib = float.Parse(GetNumericChar(inputValue, out counter));
                string attributeUnit = inputValue.Substring(counter) + "To" + outputUnit.ToLower();

                if (attributeUnit == "Topt")
                {
                    attributeValue = attrib.ToString();
                }
                else if (attributeUnit == "pcTopt")
                {
                    attributeValue = (attrib * 12).ToString();
                }
                else if (attributeUnit == "pxTopt")
                {
                    attributeValue = (attrib * 0.75F).ToString();
                }
                else if (attributeUnit == "inTopt")
                {
                    attributeValue = (attrib * 72F).ToString();
                }
                else if (attributeUnit == "cmTopt")
                {
                    attributeValue = (attrib * 28.346456693F).ToString();
                }
                else if (attributeUnit == "%Topt")
                {
                    attributeValue = attrib + "%";
                }
                else if (attributeUnit == "emTopt")
                {
                    attributeValue = (attrib * 100F) + "%";
                }
                else if (attributeUnit == "cmToin")
                {
                    attributeValue = (attrib * 0.3937008F).ToString();
                }
                else if (attributeUnit == "inTocm")
                {
                    attributeValue = (attrib / 0.3937008F).ToString();
                }
                else if (attributeUnit == "ptToin")
                {
                    attributeValue = (attrib / 72F).ToString();
                }
                else if (attributeUnit == "ptTocm")
                {
                    attributeValue = (attrib / 28.346456693F).ToString();
                }
                else if (attributeUnit == "pcToin")
                {
                    attributeValue = (attrib * 0.1666666667F).ToString();
                }
                else if (attributeUnit == "ptTopc")
                {
                    attributeValue = (attrib / 12).ToString();
                }
                else if (attributeUnit == "exToem")
                {
                    attributeValue = (attrib / 2F).ToString();
                }
                else
                {
                    attributeValue = string.Empty;
                }
            }
            catch { }
            return attributeValue;
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
                    //if (parameter.Trim() == "\"\'\"" || parameter.Trim() == "\" \'\"")
                    //{
                    //    return parameter.Replace();
                    //}
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
                            if (!((value > 47 && value < 58) || (value > 64 && value < 71) || (value > 96 && value < 103)))
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
                                if ((value > 47 && value < 58) || (value > 64 && value < 71) || (value > 96 && value < 103))
                                {
                                    unicode += parameter[count];
                                }
                                else
                                {
                                    break;
                                }
                                count++;
                            }
                            if (_outputType == OutputType.XETEX)
                            {
                                result += unicode;
                                result = "\\char \"" + result;

                            }
                            else
                            {
                                // unicode convertion
                                int decimalvalue = Convert.ToInt32(unicode, 16);
                                var c = (char) decimalvalue;
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
                //MessageBox.Show(ex.Message, "Dictionary Setting: AssignValueUnit", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        message = "Enter a value between " + convertedMinValue + userUnit + " to " + convertedMaxValue + userUnit + " inclusive.";
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
                var numbers = new int[24] {0,
                                             1, 2, 3, 4, 6,
                                             8, 9, 10, 11, 12,
                                             13, 14, 15, 16, 18,
                                             20, 20, 22, 24, 24,
                                             26, 27, 28 };
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
                var numbers = new int[35] {1,
                                             1, 1, 2, 3, 4,
                                             5, 6, 7, 7, 8,
                                             9, 9, 11, 12, 12,
                                             12, 13, 13, 14, 15,
                                             16, 16, 17, 18, 18,
                                             19, 20, 20, 21, 21,
                                             22, 22, 23, 23};
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
            //string absValue = childFontSize + "pt";
            return (childFontSize);
        }
        #endregion

        #region GetExportType()
        /// <summary>
        /// Returns the export file format
        /// </summary>
        /// <returns>Output File Formats</returns>
        public static ArrayList GetExportType()
        {
            var exportType = new ArrayList { "OpenOffice Document", "PDF" };
            return exportType;
        }
        #endregion

        /// <summary>
        /// Returns whether the given executable is installed on the system. For Windows, this is based on the
        /// items in the Installer / Uninstall areas of the registry - if the program was just copied over, it
        /// will not show up as actually installed on your system.
        /// </summary>
        /// <param name="ExecutableName">Display name (without extension) of the program</param>
        /// <returns></returns>
        public static bool IsProgramInstalled(string ExecutableName)
        {
            // first check - HKCR\Installer\Products
            const string installRegistryKey = @"Installer\Products";
            using (var key = Registry.ClassesRoot.OpenSubKey(installRegistryKey))
            {
                foreach (string subkeyName in key.GetSubKeyNames())
                {
                    try
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkeyName))
                        {
                            if (subkey.GetValue("ProductName").ToString().ToLower().IndexOf(ExecutableName.ToLower()) != -1)
                            {
                                return true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // If there was a problem getting the subkey, continue to the next key
                        continue;
                    }
                }
            }

            // second check - HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall
            const string uninstallRegistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (var key = Registry.LocalMachine.OpenSubKey(uninstallRegistryKey))
            {
                foreach (string subkeyName in key.GetSubKeyNames())
                {
                    try
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkeyName))
                        {
                            if (subkey.GetValue("DisplayName").ToString().ToLower().Equals(ExecutableName.ToLower()))
                            {
                                return true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // If there was a problem getting the subkey, continue to the next key
                        continue;
                    }
                }
            }

            return false;
        }

        #region ReplaceInFile(string filePath, string searchText, string replaceText)
        /// <summary>
        /// Function to replace a fullString in a existing file.
        /// </summary>
        /// <param name="filePath">File path of the XHTML file</param>
        /// <param name="searchText">Text to be search</param>
        /// <param name="replaceText">Text to be replace</param>
        static public void ReplaceInFile(string filePath, string searchText, string replaceText)
        {
            if (!File.Exists(filePath)) return;
            var reader = new StreamReader(filePath);
            var content = new StringBuilder();
            content.Append(reader.ReadToEnd());
            reader.Close();
            var contentWriter = new StringBuilder();
            contentWriter.Append(Regex.Replace(content.ToString(), searchText, replaceText));
            var writer = new StreamWriter(filePath);
            writer.Write(contentWriter);
            writer.Close();
        }
        #endregion

        #region StreamReplaceInFile(string filePath, string searchText, string replaceText)
        /// <summary>
        /// Stream-based version of ReplaceInFile. This uses a straight find/replace rather than a Regex
        /// (which only works on strings) - if you don't need a full regex, this will keep your memory consumption
        /// down.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="searchText"></param>
        /// <param name="replaceText"></param>
        static public void StreamReplaceInFile(string filePath, string searchText, string replaceText)
        {
            if (!File.Exists(filePath)) return;
            bool foundString = false;
            var reader = new FileStream(filePath,FileMode.Open);
            var writer = new FileStream(filePath + ".tmp", FileMode.Create);
            int next;
            while ((next = reader.ReadByte()) != -1)
            {
                byte b = (byte)next;
                if (b == searchText[0]) // first char in search text?
                {
                    // yes - searchText.Length chars into a buffer and compare them
                    int len = searchText.Length;
                    long pos = reader.Position;
                    byte[] buf = new byte[len];
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
                File.Copy(filePath + ".tmp", filePath, true);
            }
            // delete the temp file
            File.Delete(filePath + ".tmp");
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
            return userFileName.Substring(userFileName.LastIndexOfAny(new char[2] { Path.DirectorySeparatorChar, ':' }) + 1);
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

        #region ConvertUnicodeToString

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// unicode to String Conversion 
        /// <summary>
        /// <list> 
        /// </list>
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
        /// <param name="inputString">String value for example "‡"</param>
        /// <returns>unicode Value</returns>
        public static string ConvertStringToUnicode(string inputString)
        {
            string unicode = string.Empty;
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
            return unicode;
        }

        #endregion

        #region string GetTempCopy(string name)
        /// <summary>
        /// Makes a copy of folder in a writable location
        /// </summary>
        /// <returns>full path to folder</returns>
        public static string GetTempCopy(string name)
        {
            var tempFolder = Path.GetTempPath();
            var folder = Path.Combine(tempFolder, name);
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);
            FolderTree.Copy(Common.FromRegistry(name), folder);
            return folder;
        }
        #endregion string GetXeTeXToolFolder()

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
                        //MessageBox.Show(sb.ToString()); // EDB troubleshooting only... remove
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
        /// Creates a text file with names and version numbers
        /// </summary>
        /// <param name="dlls">path to assemblies to be included for compatibility testing</param>
        public static void SaveFieldworksVersions(string dlls)
        {
            string fieldworksVersionPath = GetFieldworksVersionPath();
            var writer = new StreamWriter(fieldworksVersionPath);
            var di = new DirectoryInfo(dlls);
            foreach (FileInfo fileInfo in di.GetFiles())
            {
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(fileInfo.FullName);
                writer.WriteLine(string.Format("{0},{1}", fileInfo.Name, fileVersionInfo.FileVersion));
            }
            writer.Close();
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
            var xmlDoc = new XmlDocument { XmlResolver = null };

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
            var xmlDoc = new XmlDocument { XmlResolver = null };

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
                    if(!Testing)
                    {
                    Debug.Fail(@"Pathway directory is not specified in the registry (HKEY_LOCAL_MACHINE/SOFTWARE/SIL/PATHWAY/PathwayDir)");
return FromProg(file);
                    }
                    
                }
            }
            return Path.Combine(ProgBase, file);
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
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"/SIL/FieldWorks/";
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
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"/SIL/FieldWorks 7/";
        
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
            string allUserPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            allUserPath += "/SIL/Pathway";
            return DirectoryPathReplace(allUserPath);
        }

        /// <summary>
        /// Get all user AppPath Alone
        /// </summary>
        /// <returns></returns>
        public static string GetAllUserAppPath()
        {
            string allUserPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            return DirectoryPathReplace(allUserPath);
        }

        /// <summary>
        /// Get all user path
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationDataPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return path;
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
            var fsWrite = new FileStream(sourceFile, FileMode.Create, FileAccess.ReadWrite);
            var writer = new StreamWriter(fsWrite);
            writer.WriteLine(textToInsert);

            writer.Write(builder.ToString());
            writer.Close();
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
        /// Cleans up the output directory of all "temporary" files created during the export process.
        /// This is determined by comparing the timestamp on each file to the DateTime we stored in the
        /// PrintVia.On_OK() method. Files with an earlier timestamp will be ignored, except for the .de file
        /// (that gets copied over as part of the export process).
        /// </summary>
        /// <param name="outputFolder">Output directory to clean up</param>
        /// <param name="keepFilename">This is the permanent output file you want to keep (.odt, .epub, etc.)</param>
        public static void CleanupOutputDirectory(string outputFolder, string keepFilename)
        {
            var al = new ArrayList();
            al.Add(keepFilename);
            CleanupOutputDirectory(outputFolder, al);
        }

        public static void CleanupOutputDirectory(string outputFolder, string keepFilename, string keepJad)
        {
            var al = new ArrayList();
            al.Add(keepFilename);
            al.Add(keepJad);
            CleanupOutputDirectory(outputFolder, al);
        }

        /// <summary>
        /// Cleans up the output directory of all "temporary" files created during the export process.
        /// This is determined by comparing the timestamp on each file to the DateTime we stored in the
        /// PrintVia.On_OK() method. Files with an earlier timestamp will be ignored, except for the .de file
        /// (that gets copied over as part of the export process).
        /// This override allows for multiple files to be saved
        /// </summary>
        /// <param name="outputFolder">Output directory to clean up</param>
        /// <param name="keepFilenames">ArrayList of string elements; these are the files to keep.</param>
        public static void CleanupOutputDirectory(string outputFolder, ArrayList keepFilenames)
        {
            var outputFiles = Directory.GetFiles(outputFolder);
            foreach (var outputFile in outputFiles)
            {
                if (!keepFilenames.Contains(outputFile))
                {
                    try
                    {
                        // Did we modify this file during our export? If so, delete it
                        if (File.GetLastWriteTime(outputFile).CompareTo(TimeStarted) > 0)
                        {
                            File.Delete(outputFile);
                        }
                        // delete the Scripture.de / Dictionary.de file as well
                        else if (outputFile.EndsWith(".de"))
                        {
                            File.Delete(outputFile);
                        }
                    }
                    catch (Exception)
                    {
                        // problem with this file - just continue with the next one
                        continue;
                    }
                }
            }
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
                    Directory.Delete(directoryPath, true);
                    deleted = true;
                }
                catch(Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return deleted;
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
            if (targetFileName.Length > 1)
                cssFile = targetFileName;

            string mergepath = Path.GetTempPath();
            string mergedCSSfile = PathCombine(mergepath, cssFile);
            ArrayList arrayCSSFile = new ArrayList();
            string BaseCssFileWithPath = fullPath;
            arrayCSSFile = GetCSSFileNames(fullPath, BaseCssFileWithPath);
            //if(arrayCSSFile.Count == 0) return BaseCssFileWithPath; //
            arrayCSSFile.Add(BaseCssFileWithPath);
            RemovePreviousMirroredPage(arrayCSSFile);
            FileStream fs2;
            try
            {
                fs2 = new FileStream(mergedCSSfile, FileMode.Create, FileAccess.Write);
            }
            catch
            {
                mergedCSSfile = PathCombine(mergepath, "tempcssfile.css");
                fs2 = new FileStream(mergedCSSfile, FileMode.Create, FileAccess.Write);
            }
            var sw2 = new StreamWriter(fs2);
            //for (int i = arrayCSSFile.Count - 1; i >= 0; i--)
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
            return mergedCSSfile;
        }

        public static void RemovePreviousMirroredPage(ArrayList arrayCssFile)
        {
            bool removeMirrorPage = false;
            bool removePageNumber = false;
            int count = arrayCssFile.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                string cssFile = arrayCssFile[i].ToString();
                //For Mirrored Page
                if (cssFile.IndexOf("Running_Head_Every_Page.css") >= 0)
                {
                    removeMirrorPage = true;
                }
                if (removeMirrorPage)
                {
                    if (cssFile.IndexOf("Running_Head_Mirrored.css") >= 0)
                    {
                        arrayCssFile.RemoveAt(i);
                    }
                }
                //For Page number
                if (cssFile.IndexOf("PageNumber_") >= 0)
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
            //if (path1 == null) throw new ArgumentNullException("path1");
            //if (path2 == null) throw new ArgumentNullException("path2");
            path1 = DirectoryPathReplace(path1);
            path2 = DirectoryPathReplace(path2);
            return Path.Combine(path1, path2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outPath"></param>
        /// <returns></returns>
        public static bool CreateDirectory(string outPath)
        {
            bool returnValue = true;
            try
            {
                if (!Directory.Exists(outPath))
                {
                    Directory.CreateDirectory(outPath);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Sorry! You might not have permission to use this resource.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                returnValue = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                returnValue = false;
            }
            return returnValue;
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

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// To find the width of the image.
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param> </param>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        public static string CalcDimension(string fromPath, ref string imgDimension, char Type)
        {
            double retValue = 0.0;
            bool result;
            try
            {
                if (File.Exists(fromPath))
                {
                    Image fullimage = Image.FromFile(fromPath);
                    double height = fullimage.Height;
                    double width = fullimage.Width;
                    fullimage.Dispose();

                    if (Type == 'W') // Find width
                    {
                        retValue = width / height * double.Parse(imgDimension);
                        if (ColumnWidth > 0 && retValue > ColumnWidth)
                        {
                            retValue = ColumnWidth * .9;
                        }
                    }
                    else if (Type == 'H') // Find height
                    {
                        if (imgDimension.IndexOf("%") > 0)
                        {
                            int counter;
                            string retValue1 = GetNumericChar(imgDimension, out counter);
                            double widthInPt = double.Parse(retValue1) / 100 * width;
                            if (widthInPt > ColumnWidth)
                            {
                                widthInPt = ColumnWidth;
                            }
                            imgDimension = widthInPt.ToString();
                            retValue = height / width * widthInPt;
                        }
                        else
                            retValue = height / width * double.Parse(imgDimension);
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
            map["DateTime"] = DateTime.Now.ToString("yyyy-MM-dd_hhmm");
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
            if (length < 15)
                return true;
            return s.Substring(length - 15, 10) != DateTime.Now.ToString("yyyy-MM-dd");
        }
        #endregion SaveInFolder

        #region PathwayHelpSetup
        /// <summary>
        /// setup help file
        /// </summary>
        public static void PathwayHelpSetup()
        {
            try
            {
                var helpFolder = FromRegistry("Help");
                var directoryInfo = new DirectoryInfo(helpFolder);
                var fileInfoList = directoryInfo.GetFiles("*.chm");
                HelpProv.HelpNamespace = fileInfoList[0].FullName;
            }
            catch
            {
            }

        }
        #endregion PathwayHelpSetup

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
            string PsSupportPath = GetPSApplicationPath();
            string xmlFileNameWithPath = PathCombine(PsSupportPath, "GenericFont.xml");
            string xPath = "//font-language-mapping";
            XmlNodeList fontList = GetXmlNodes(xmlFileNameWithPath, xPath);
            if (fontList != null && fontList.Count > 0)
            {
                foreach (XmlNode xmlNode in fontList)
                {
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
                var ldml = new XmlDocument { XmlResolver = null };
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
            string backUpFilePath = Path.Combine(Path.GetDirectoryName(userSheet), backUpFileName);
            File.Copy(userSheet, backUpFilePath, true);
            File.Copy(updatedSheet, userSheet, true);

            userSettings.Load(backUpFilePath);
            installerSettings.Load(userSheet);
            foreach (string media in mediaType)
            {
                string xPathUser = @"//stylePick/styles/" + media + "/style[@type='Custom']";
                XmlNodeList nodeList = userSettings.SelectNodes(xPathUser);

                string xPathInstaller = @"//stylePick/styles/" + media;
                XmlNode nodeInst = installerSettings.SelectSingleNode(xPathInstaller);
                if (nodeList != null)
                    foreach (XmlNode node in nodeList)
                    {
                        XmlDocumentFragment docFrag = installerSettings.CreateDocumentFragment();
                        docFrag.InnerXml = node.OuterXml;
                        nodeInst.AppendChild(docFrag);
                    }
            }
            installerSettings.Save(userSheet);
        }

        /// <summary>
        /// To copy Temporary Office files to Environmental Temp Folder instead of keeping changes in Application Itself.
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destFolder"></param>
        public static void CopyOfficeFolder(string sourceFolder, string destFolder)
        {
            if (Directory.Exists(destFolder))
            {
                //Directory.Delete(destFolder, true);
                DeleteDirectory(destFolder);
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

        public static string ReplaceSeperators(string styleName)
        {
            if (styleName.IndexOf(SepPseudo) > 0)
                styleName = styleName.Replace(SepPseudo, "");

            if (styleName.IndexOf(" ") > 0)
                styleName = styleName.Replace(" ", "");

            if (styleName.IndexOf(sepPrecede) > 0)
                styleName = styleName.Replace(sepPrecede, "");

            if (styleName.IndexOf(SepPseudo) > 0)
                styleName = styleName.Replace(SepPseudo, "");

            if (styleName.IndexOf(SepParent) > 0)
                styleName = styleName.Replace(SepParent, "");

            if (styleName.IndexOf(SepTag) > 0)
                styleName = styleName.Replace(SepTag, "");

            if (styleName.IndexOf("1") > 0)
                styleName = styleName.Replace("1", "ONE");

            return styleName;
        }

	public static string GetOsName()
{
OperatingSystem osInfo = Environment.OSVersion;
return osInfo.Platform.ToString();

}
        ///// <summary>
        ///// If the user selected page style is "Every Page", this method will remove the "@Page:left" and 
        ///// "@page:right" tag from the  CSS file.
        ///// </summary>
        ///// <param name="filePath">CSS file to remove the @page:left and @page:right</param>
        //public static void RemovePageLeftPageRightClass(string filePath)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string line;

        //    if (File.Exists(filePath))
        //    {
        //        string tempFile = Path.Combine(Path.GetDirectoryName(filePath), "_temp.css");
        //        File.Copy(filePath, tempFile, true);
        //        StreamReader file = null;
        //        try
        //        {

        //            int braces = 0;
        //            bool countBraces = false;

        //            file = new StreamReader(tempFile);
        //            while ((line = file.ReadLine()) != null)
        //            {
        //                if ((line.IndexOf("@page :left") > -1) ||
        //                    (line.IndexOf("@page :right") > -1))
        //                {
        //                    countBraces = true;
        //                }


        //                if (countBraces)
        //                {
        //                    if (line.IndexOf("{") > -1)
        //                    {
        //                        braces++;
        //                    }
        //                    if (line.IndexOf("}") > -1)
        //                    {
        //                        braces--;
        //                    }

        //                    if (braces == 0)
        //                    {
        //                        countBraces = false;
        //                    }
        //                    continue;
        //                }

        //                sb.AppendLine(line);
        //            }
        //            File.WriteAllText(filePath, sb.ToString());
        //            File.Delete(tempFile);
        //        }
        //        finally
        //        {
        //            if (file != null)
        //                file.Close();
        //        }
        //    }
        //}


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
            //unicodeDecimal = (int)unicodeString[0];
            string fontName = string.Empty;
            string PsSupportPath = GetPSApplicationPath();
            string xmlFileNameWithPath = PathCombine(PsSupportPath, "GenericFont.xml");
            string xPath = "//font-language-unicode-map";
            XmlNodeList fontList = GetXmlNodes(xmlFileNameWithPath, xPath);
            if (fontList != null && fontList.Count > 0)
            {
                bool isLanguageFound = false;
                int numberOfCharCheck = 1;
                foreach (char ch in unicodeString)
                {
                    //if (numberOfCharCheck++ > 5) break;
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

    }
}