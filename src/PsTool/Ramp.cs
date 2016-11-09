// --------------------------------------------------------------------------------------------
// <copyright file="ramp.cs" from='2013' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. 
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Prepare RAMP packaging putting meta data into JSON format.
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;

namespace SIL.Tool
{
    /// <summary>
    /// Combines all css into a single file by implementing @import
    /// </summary>
    public class Ramp
    {
        #region Private variable

        private string _createdOn;
        private string _modifiedDate;
        private string _ready;
        private string _title;
        private string _broadType;
        private string _typeMode;
        private string _formatMedium;
        private string _descStage;
        private string _versionType;
        private string _typeScholarlyWork;
        private List<string> _subjectLanguage = new List<string>();
	    private List<string> _coverageSpacialCountry = new List<string>();
        private string _subjectLanguageHas;
        private List<string> _languageScript = new List<string>();
        private string _formatExtentText;
        private string _formatExtentImages;
	    private string _silDomain;
        private string _domainSubTypeLing;
        private List<string> _subject = new List<string>();
        private string _relRequiresHas;
        private List<string> _relRequires = new List<string>();
        private string _relConformsto;
        private List<string> _rightsHolder = new List<string>();
        private string _rights;
        private string _silSensitivityMetaData;
        private string _silSensitivityPresentation;
        private string _silSensitivitySource;
        private List<string> _rampFile = new List<string>();
        private string _status;
        private string _rampDescriptionHas;
        private string _rampDescription;
        private string _vernacularmaterialsType;
        private string _typeScriptureType;
        private string _titleScriptureScope;
        private string _helperVernacularContent;
        private string _publisher;
        private List<string> _languageIso = new List<string>();
        private List<string> _contributor = new List<string>();
        protected Dictionary<string, string> _fileList = new Dictionary<string, string>();
        protected string _folderPath = string.Empty;
        protected string _outputExtension = string.Empty;
        protected string _projInputType = string.Empty;
        private string _outputFileTitle = string.Empty;
        protected Dictionary<string, string> _isoLanguageCode = new Dictionary<string, string>();
        protected Dictionary<string, string> _isoLanguageCodeandName = new Dictionary<string, string>();
        protected Dictionary<string, string> _isoLanguageScriptandName = new Dictionary<string, string>();
        protected Dictionary<string, string> _oldTestament = new Dictionary<string, string>();
        protected Dictionary<string, string> _newTestament = new Dictionary<string, string>();
        protected Dictionary<string, string> _apocryphaTestament = new Dictionary<string, string>();

        #endregion

        #region Public Variables
        public List<string> LanguageIso
        {
            get { return _languageIso; }
            set { _languageIso = value; }
        }

        public List<string> Contributor
        {
            get { return _contributor; }
            set { _contributor = value; }
        }

        public List<string> LanguageScript
        {
            get { return _languageScript; }
            set { _languageScript = value; }
        }

        public List<string> Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public List<string> RelRequires
        {
            get { return _relRequires; }
            set { _relRequires = value; }
        }

        public List<string> RightsHolder
        {
            get { return _rightsHolder; }
            set { _rightsHolder = value; }
        }

        public List<string> CoverageSpacialCountry
        {
            get { return _coverageSpacialCountry; }
            set { _coverageSpacialCountry = value; }
        }

        public List<string> SubjectLanguage
        {
            get { return _subjectLanguage; }
            set { _subjectLanguage = value; }
        }

        protected string CreatedOn
        {
            get { return _createdOn; }
            private set { _createdOn = value; }
        }

        private string Ready
        {
            get { return _ready; }
            set { _ready = value; }
        }

        private string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        protected string BroadType
        {
            get { return _broadType; }
            private set { _broadType = value; }
        }

        protected string TypeMode
        {
            get { return _typeMode; }
            private set { _typeMode = value; }
        }

        private string FormatMedium
        {
            get { return _formatMedium; }
            set { _formatMedium = value; }
        }

        private string DescStage
        {
            get { return _descStage; }
            set { _descStage = value; }
        }

        private string VersionType
        {
            get { return _versionType; }
            set { _versionType = value; }
        }

        private string TypeScholarlyWork
        {
            get { return _typeScholarlyWork; }
            set { _typeScholarlyWork = value; }
        }

        private string SubjectLanguageHas
        {
            get { return _subjectLanguageHas; }
            set { _subjectLanguageHas = value; }
        }

        private string FormatExtentText
        {
            get { return _formatExtentText; }
            set { _formatExtentText = value; }
        }

        protected string FormatExtentImages
        {
            get { return _formatExtentImages; }
            private set { _formatExtentImages = value; }
        }

        private string SilDomain
        {
            get { return _silDomain; }
            set { _silDomain = value; }
        }

        private string DomainSubTypeLing
        {
            get { return _domainSubTypeLing; }
            set { _domainSubTypeLing = value; }
        }

        private string RelRequiresHas
        {
            get { return _relRequiresHas; }
            set { _relRequiresHas = value; }
        }

        private string RelConformsto
        {
            get { return _relConformsto; }
            set { _relConformsto = value; }
        }

        private string SilSensitivityMetaData
        {
            get { return _silSensitivityMetaData; }
            set { _silSensitivityMetaData = value; }
        }

        private string SilSensitivityPresentation
        {
            get { return _silSensitivityPresentation; }
            set { _silSensitivityPresentation = value; }
        }

        protected string Rights
        {
            get { return _rights; }
            private set { _rights = value; }
        }

        private string SilSensitivitySource
        {
            get { return _silSensitivitySource; }
            set { _silSensitivitySource = value; }
        }

        public List<string> RampFile
        {
            get { return _rampFile; }
            set { _rampFile = value; }
        }

        private string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string RampDescriptionHas
        {
            get { return _rampDescriptionHas; }
            set { _rampDescriptionHas = value; }
        }

        public string RampDescription
        {
            get { return _rampDescription; }
            set { _rampDescription = value; }
        }

        public string ProjInputType
        {
            get { return _projInputType; }
            set { _projInputType = value; }
        }

        public string VernacularmaterialsType
        {
            get { return _vernacularmaterialsType; }
            set { _vernacularmaterialsType = value; }
        }

        public string TypeScriptureType
        {
            get { return _typeScriptureType; }
            set { _typeScriptureType = value; }
        }

        public string TitleScriptureScope
        {
            get { return _titleScriptureScope; }
            set { _titleScriptureScope = value; }
        }

        public string HelperVernacularContent
        {
            get { return _helperVernacularContent; }
            set { _helperVernacularContent = value; }
        }

        public string ModifiedDate
        {
            get { return _modifiedDate; }
            set { _modifiedDate = value; }
        }

        public string Publisher
        {
            get { return _publisher; }
            set { _publisher = value; }
        }

        public string OutputFileTitle
        {
            get { return _outputFileTitle; }
            set { _outputFileTitle = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="outputExtension"></param>
        /// <param name="inputType"> </param>
        public void Create(string folderPath, string outputExtension, string inputType)
        {
            if (IsFromTestBed())
            {
                return;
            }

            _projInputType = inputType;
            _folderPath = folderPath;
            _outputExtension = outputExtension;
            LoadLanguagefromXML();
            SetRampData();
            string encodedKey = GetEncodedKey();
            UpdateMetsXML(encodedKey);
            CompressToRamp();
        }

        protected void LoadLanguagefromXML()
        {
            string xmlFilePath = Common.CopyXmlFileToTempDirectory("RampLangCode.xml");
            if (!File.Exists(xmlFilePath))
                return;

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
                    _isoLanguageCode.Add(node.Attributes["name"].Value, node.Attributes["value"].Value);
                }
            }
            XmlNodeList threeLetterLangList = xDoc.SelectNodes(threeLetterLangXPath);
            if (threeLetterLangList != null && threeLetterLangList.Count > 0)
            {
                foreach (XmlNode node in threeLetterLangList)
                {
                    _isoLanguageCodeandName.Add(node.Attributes["name"].Value, node.Attributes["value"].Value);
                }
            }
            XmlNodeList scriptLangList = xDoc.SelectNodes(scriptLangXPath);
            if (scriptLangList != null && scriptLangList.Count > 0)
            {
                foreach (XmlNode node in scriptLangList)
                {
                    _isoLanguageScriptandName.Add(node.Attributes["name"].Value, node.Attributes["scriptname"].Value);
                }
            }
        }

        private bool IsFromTestBed()
        {
            bool result = false;
            if (Common.Testing) return true;
            string exeAssemblyName = Assembly.GetEntryAssembly().GetName().Name;
            if (exeAssemblyName == "TestBed" || exeAssemblyName == "ConfigurationTool")
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void AddSubjLanguage(string langList)
        {
            foreach (string lang in langList.Split(new[] { ';' }))
            {
                var colonPosition = lang.IndexOf(':');
                if (colonPosition > 0)
                {
                    var langCode = JustLangCode(lang.Substring(0, colonPosition).Trim());
                    var langName = JustLangName(lang.Substring(colonPosition + 1).Trim());
                    if (langCode.Length == 2 && _isoLanguageCode.ContainsKey(langCode))
                    {
                        langCode = _isoLanguageCode[langCode];
                    }
                    string format = langCode + ":" + langName;
                    SubjectLanguage.Add(format);
                }
                else
                {
                    string langCode = JustLangCode(lang);
                    if (lang.Length == 2 && _isoLanguageCode.ContainsKey(lang))
                    {
                        langCode = _isoLanguageCode[lang];
                    }
                    if (langCode.Trim().Length > 0 && _isoLanguageCodeandName.ContainsKey(langCode))
                    {
                        string format = langCode + ":" + _isoLanguageCodeandName[langCode]; //"eng:English"
                        SubjectLanguage.Add(format);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void AddLanguageIso(string langCollection)
        {
            string[] languageCode = langCollection.Split(';');
            foreach (string s in languageCode)
            {
                if (s.Trim().Length == 0)
                    continue;

                if (s.IndexOf(':') > 0)
                {
                    string[] val = s.Split(':');
                    string code = val[0];
                    string codeName = val[1];

                    code = JustLangCode(code);

                    if (code.Trim().Length == 2)
                    {
                        if (_isoLanguageCode.ContainsKey(code))
                        {
                            code = _isoLanguageCode[code];
                        }
                    }
                    codeName = JustLangName(codeName);

                    if (code.Trim().Length > 0)
                    {
                        string format = code + ":" + codeName; //"eng:English"
                        if (!LanguageIso.Contains(format))
                        {
                            LanguageIso.Add(format);
                        }
                    }
                }
                else
                {
                    if (s.Length == 3)
                    {
                        LanguageIso.Add(s + ":" + _isoLanguageCodeandName[s]);
                    }
                }
            }
        }

        private static string JustLangName(string codeName)
        {
            var parenthesisPosition = codeName.IndexOf('(');
            if (parenthesisPosition > 0)
            {
                codeName = codeName.Substring(0, parenthesisPosition).Trim();
            }
            return codeName;
        }

        private static string JustLangCode(string code)
        {
            var dashPosition = code.IndexOf('-');
            if (dashPosition > 0)
            {
                code = code.Substring(0, dashPosition);
            }
            return code;
        }

        protected void AddLanguageScript(string langCollection)
        {
            string[] languageCode = langCollection.Split(',');
            foreach (string s in languageCode)
            {
                if (s.Trim().Length == 0)
                    continue;

                if (s.IndexOf(':') > 0 && s.IndexOf('-') > 0)
                {
                    string[] val = s.Split('-');
                    if (_isoLanguageScriptandName.ContainsKey(val[1]))
                    {
                        LanguageScript.Add(val[1] + ":" + _isoLanguageScriptandName[val[1]]);
                    }
                }
                else if (_isoLanguageScriptandName.ContainsKey(s))
                {
                    LanguageScript.Add(s + ":" + _isoLanguageScriptandName[s]);
                }
            }
        }

        private void AddContributor(string contrib)
        {
            Contributor.Add(contrib);
        }

        private void AddSubject(string subj)
        {
            Subject.Add(subj);
        }

        private void AddRelRequires(string text)
        {
            string[] fontList = text.Split(';');
            foreach (string font in fontList)
            {
                if (font.Trim().Length > 0)
                    RelRequires.Add(font);
            }
        }

        private void AddRightsHolder(string value)
        {
            RightsHolder.Add(value);
        }

        private void AddFile(RampFile file)
        {
            string newFile = string.Empty;
            if (file.FileName != null)
            {
                newFile = " \": \"" + file.FileName;
            }
            if (file.FileDescription != null)
            {
                newFile = newFile + "\", \"description\": \"" + file.FileDescription;
            }
            if (file.FileRelationship != null)
            {
                newFile = newFile + "\", \"relationship\": \"" + file.FileRelationship;
            }
            if (file.FileIsPrimary != null)
            {
                newFile = newFile + "\", \"is_primary\": \"" + file.FileIsPrimary;
            }
            if (file.FileSilPublic != null)
            {
                newFile = newFile + "\", \"silPublic\": \"" + file.FileSilPublic;
            }
            RampFile.Add(newFile);

            if (file.FileName != null)
            {
                string isPrimary = string.Empty;
                if (file.FileIsPrimary != null)
                {
                    isPrimary = "preferred";
                }
                _fileList.Add(file.FileName, isPrimary);
            }
        }

        #endregion Public Methods

        #region Private Methods
        private string GetEncodedKey()
        {
            CreateRampJSON(Path.GetDirectoryName(_folderPath));

            string metcFileName = Common.PathCombine(Path.GetDirectoryName(_folderPath), "temp.json");

            string text = File.ReadAllText(metcFileName);

            string rampEncodedKey = EncodeDecodeBase64(text, "encode");

            File.Delete(metcFileName);

            return rampEncodedKey;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void SetRampData()
        {
            Param.UnLoadValues();
            Param.LoadSettings();
            Param.SetValue(Param.InputType, _projInputType);
            Param.LoadSettings();
            //ramp.RampId = "ykmb9i6zlh";
            CreatedOn = DateTime.Now.ToString("r");
            Ready = "Y";
            Title = Param.GetMetadataValue(Param.Title, Param.GetOrganization());
            BroadType = _projInputType.ToLower() == "dictionary" ? "wider_audience" : "vernacular";
            TypeMode = "Text";
            FormatMedium = "Paper";
            DescStage = "rough_draft";
            VersionType = "first";
            IncludeScriptureProperty();
            TypeScholarlyWork = "Book";
            AddSubjLanguage(Common.GetLanguageCode(_folderPath, _projInputType, true));
            //CoverageSpacialRegionHas = "Y";//
            //AddCoverageSpacialCountry("IN:India, Andhra Pradesh");//
            SubjectLanguageHas = "Y";
            AddLanguageIso(Common.GetLanguageCodeList(_folderPath, _projInputType));
            AddLanguageScript(Common.GetLanguageScriptList(_folderPath, _projInputType));
            string role = _projInputType.ToLower() == "scripture" ? "translator" : "compiler";
            AddContributor(Param.GetMetadataValue(Param.Creator).Replace(",", "--") + "," + role);
            FormatExtentText = GetNumberOfPdfPages(_folderPath);
            FormatExtentImages = GetImageCount(_folderPath);//
            if (Int32.Parse(FormatExtentImages) > 0)
            {
                TypeMode = TypeMode + ",Graphic";
            }
            ModifiedDate = DateTime.Now.ToString("yyyy-MM-dd");
            Publisher = Param.GetMetadataValue(Param.Publisher);
            //DescSponsership = Param.GetOrganization();
            //DescTableofContentsHas = " ";//
            SilDomain = "LING:Linguistics";
            DomainSubTypeLing = "lexicon (LING)";
            AddSubject(Param.GetMetadataValue(Param.Subject) + ",eng");
            RelRequiresHas = "Y";
            AddRelRequires(Common.GetFontList(_folderPath, _projInputType, _outputExtension));
            RelConformsto = "TTF";
            AddRightsHolder(Param.GetMetadataValue(Param.Publisher));
            Rights = GetLicenseFileName();//
            SilSensitivityMetaData = "Public";
            SilSensitivityPresentation = "Public";
            SilSensitivitySource = "Insite users";
            RampDescription = Param.GetMetadataCurrentValue(Param.Description);
            if (RampDescription != null && RampDescription.Trim().Length > 0)
            {
                RampDescriptionHas = "Y";
            }

            if (_folderPath != null)
            {
                string[] files = Directory.GetFiles(Path.GetDirectoryName(_folderPath));
                foreach (string file in files)
                {
                    RampFile rFile = new RampFile();
                    rFile.FileName = Path.GetFileName(file);
                    string fileExtn = Path.GetExtension(file);
                    if (fileExtn == ".odm" || fileExtn == ".jar" || fileExtn == ".epub" || fileExtn == ".mybible" || fileExtn == ".ldml")
                    {

                        rFile.FileDescription = Path.GetFileNameWithoutExtension(file) + " " + fileExtn.Replace(".", "") + " document";
                        rFile.FileRelationship = "presentation";
                        rFile.FileIsPrimary = "Y";
                        rFile.FileSilPublic = "Y";
                    }
                    else if (fileExtn == ".odt" || fileExtn == ".pdf" || fileExtn == ".jad" || fileExtn == ".nt")
                    {
                        rFile.FileDescription = Path.GetFileNameWithoutExtension(file) + " " + fileExtn.Replace(".", "") + " document";
                        rFile.FileRelationship = "presentation";
                        rFile.FileSilPublic = "Y";
                        if (_outputExtension.IndexOf(".odt") == 0 || _outputExtension.IndexOf(".pdf") == 0 || fileExtn == ".nt")
                        {
                            rFile.FileIsPrimary = "Y";
                        }
                    }
                    else if (fileExtn == ".xhtml")
                    {
                        rFile.FileDescription = Path.GetFileNameWithoutExtension(file) + " XHTML file";
                        rFile.FileRelationship = "source";
                    }
                    else if (fileExtn == ".css")
                    {
                        rFile.FileDescription = Path.GetFileNameWithoutExtension(file) + " stylesheet";
                        rFile.FileRelationship = "source";
                    }
                    AddFile(rFile);
                }
            }
            Status = "ready";
        }



        private void IncludeScriptureProperty()
        {
            if (_projInputType.ToLower() == "scripture")
            {
                VernacularmaterialsType = "scripture";
                TypeScriptureType = "Bible text selection";
                TitleScriptureScope = GetScriptureScope(_folderPath);
                HelperVernacularContent = "shell_none";
            }
        }

        private string GetScriptureScope(string filename)
        {
            string scriptureScope = string.Empty;
            List<string> books = Common.GetInputBooks(filename);

            string wholeOldTestament = CheckOldTestament(books);
            if (wholeOldTestament.Length > 0)
            {
                scriptureScope = scriptureScope.Trim().Length > 0
                                     ? scriptureScope + "," + wholeOldTestament
                                     : wholeOldTestament;
            }

            string wholeNewTestament = CheckNewTestament(books);
            if (wholeNewTestament.Length > 0)
            {
                scriptureScope = scriptureScope.Trim().Length > 0
                                     ? scriptureScope + "," + wholeNewTestament
                                     : wholeNewTestament;
            }

            string apocryphaTestament = CheckApocrypha(books);
            if (apocryphaTestament.Length > 0)
            {
                scriptureScope = scriptureScope.Trim().Length > 0
                                     ? scriptureScope + "," + apocryphaTestament
                                     : apocryphaTestament;
            }

            if (scriptureScope.IndexOf("WOT:Old Testament") > 0 && scriptureScope.IndexOf("WNT:New Testament") > 0 && scriptureScope.IndexOf("WAP:Apocrypha") > 0)
            {
                scriptureScope = "WBI:Bible";
            }

            if (scriptureScope.IndexOf("WOT:Old Testament") >= 0 || scriptureScope.IndexOf("WNT:New Testament") >= 0 || scriptureScope.IndexOf("WAP:Apocrypha") >= 0
                || scriptureScope.IndexOf("WBI:Bible") >= 0)
            {
                TypeScriptureType = "Bible text complete";
            }
            return scriptureScope;
        }


        private string CheckOldTestament(List<string> books)
        {
            bool result = true;
            string booklist = string.Empty;
            LoadOldTestament();
            foreach (KeyValuePair<string, string> book in _oldTestament)
            {
                if(!books.Contains(book.Key))
                {
                    result = false;
                    break;
                }
            }

            if (result)
            {
                booklist = "WOT:Old Testament";
            }
            else
            {
                for (int i = 0; i < books.Count; i++)
                {
                    if (_oldTestament.ContainsKey(books[i]))
                    {
                        booklist = booklist + books[i] + ":" + _oldTestament[books[i]];
                        if (i < books.Count - 1)
                            booklist = booklist + ",";
                    }

                }
            }
            return booklist;
        }

        private string CheckNewTestament(List<string> books)
        {
            bool result = true;
            string booklist = string.Empty;
            LoadNewTestament();
            foreach (KeyValuePair<string, string> book in _newTestament)
            {
                if (!books.Contains(book.Key))
                {
                    result = false;
                    break;
                }
            }

            if (result)
            {
                booklist = "WNT:New Testament";
            }
            else
            {
                for (int i = 0; i < books.Count; i++)
                {
                    if (_newTestament.ContainsKey(books[i]))
                    {
                        booklist = booklist + books[i] + ":" + _newTestament[books[i]];
                        if (i < books.Count - 1)
                            booklist = booklist + ",";
                    }
                    
                }
            }
            return booklist;
        }

        private string CheckApocrypha(List<string> books)
        {
            bool result = true;
            string booklist = string.Empty;
            LoadApocrypha();
            foreach (KeyValuePair<string, string> book in _apocryphaTestament)
            {
                if (!books.Contains(book.Key))
                {
                    result = false;
                    break;
                }
            }

            if (result)
            {
                booklist = "WAP:Apocrypha";
            }
            else
            {
                for (int i = 0; i < books.Count; i++)
                {
                    if (_apocryphaTestament.ContainsKey(books[i]))
                    {
                        booklist = booklist + books[i] + ":" + _apocryphaTestament[books[i]];
                        if (i < books.Count - 1)
                            booklist = booklist + ",";
                    }

                }
            }
            return booklist;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadApocrypha()
        {
            _apocryphaTestament.Add("1ES", "1 Esdras");
            _apocryphaTestament.Add("2ES", "2 Esdras");
            _apocryphaTestament.Add("TOB", "Tobit");
            _apocryphaTestament.Add("JDT", "Judith");
            _apocryphaTestament.Add("ESG", "Additions to Esther");
            _apocryphaTestament.Add("WIS", "Wisdom");
            _apocryphaTestament.Add("SIR", "Ecclesiasticus");
            _apocryphaTestament.Add("BAR", "Baruch");
            _apocryphaTestament.Add("LJE", "Letter of Jeremiah");
            _apocryphaTestament.Add("SYM", "Song of the Three Young Men");
            _apocryphaTestament.Add("PAZ", "Prayer of Azariah");
            _apocryphaTestament.Add("SUS", "Susanna");
            _apocryphaTestament.Add("BEL", "Bel and the Dragon");
            _apocryphaTestament.Add("MAN", "Prayer of Manasseh");
            _apocryphaTestament.Add("1MA", "1 Maccabees");
            _apocryphaTestament.Add("2MA", "2 Maccabees");
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadOldTestament()
        {
            _oldTestament.Add("GEN", "Genesis");
            _oldTestament.Add("EXO", "Exodus");
            _oldTestament.Add("LEV", "Leviticus");
            _oldTestament.Add("NUM", "Numbers");
            _oldTestament.Add("DEU", "Deuteronomy");
            _oldTestament.Add("JOS", "Joshua");
            _oldTestament.Add("JDG", "Judges");
            _oldTestament.Add("RUT", "Ruth");
            _oldTestament.Add("1SA", "1 Samuel");
            _oldTestament.Add("2SA", "2 Samuel");
            _oldTestament.Add("1KI", "1 Kings");
            _oldTestament.Add("2KI", "2 Kings");
            _oldTestament.Add("1CH", "1 Chronicles");
            _oldTestament.Add("2CH", "2 Chronicles");
            _oldTestament.Add("EZR", "Ezra");
            _oldTestament.Add("NEH", "Nehemiah");
            _oldTestament.Add("EST", "Esther");
            _oldTestament.Add("JOB", "Job");
            _oldTestament.Add("PSA", "Psalms");
            _oldTestament.Add("PRO", "Proverbs");
            _oldTestament.Add("ECC", "Ecclesiastes");
            _oldTestament.Add("SNG", "Song of  Solomon");
            _oldTestament.Add("ISA", "Isaiah");
            _oldTestament.Add("JER", "Jeremiah");
            _oldTestament.Add("LAM", "Lamentations");
            _oldTestament.Add("EZK", "Ezekiel");
            _oldTestament.Add("DAN", "Daniel");
            _oldTestament.Add("HOS", "Hosea");
            _oldTestament.Add("JOL", "Joel");
            _oldTestament.Add("AMO", "Amos");
            _oldTestament.Add("OBA", "Obadiah");
            _oldTestament.Add("JON", "Jonah");
            _oldTestament.Add("MIC", "Micah");
            _oldTestament.Add("NAM", "Nahum");
            _oldTestament.Add("HAB", "Habakkuk");
            _oldTestament.Add("ZEP", "Zephaniah");
            _oldTestament.Add("HAG", "Haggai");
            _oldTestament.Add("ZEC", "Zechariah");
            _oldTestament.Add("MAL", "Malachi");
        }

        private void LoadNewTestament()
        {
            _newTestament.Add("MAT", "Matthew");
            _newTestament.Add("MRK", "Mark");
            _newTestament.Add("LUK", "Luke");
            _newTestament.Add("JHN", "John");
            _newTestament.Add("ACT", "Acts");
            _newTestament.Add("ROM", "Romans");
            _newTestament.Add("1CO", "1 Corinthians");
            _newTestament.Add("2CO", "2 Corinthians");
            _newTestament.Add("GAL", "Galatians");
            _newTestament.Add("EPH", "Ephesians");
            _newTestament.Add("PHP", "Philippians");
            _newTestament.Add("COL", "Colossians");
            _newTestament.Add("1TH", "1 Thessalonians");
            _newTestament.Add("2TH", "2 Thessalonians");
            _newTestament.Add("1TI", "1 Timothy");
            _newTestament.Add("2TI", "2 Timothy");
            _newTestament.Add("TIT", "Titus");
            _newTestament.Add("PHM", "Philemon");
            _newTestament.Add("HEB", "Hebrews");
            _newTestament.Add("JAS", "James");
            _newTestament.Add("1PE", "1 Peter");
            _newTestament.Add("2PE", "2 Peter");
            _newTestament.Add("1JN", "1 John");
            _newTestament.Add("2JN", "2 John");
            _newTestament.Add("3JN", "3 John");
            _newTestament.Add("JUD", "Jude");
            _newTestament.Add("REV", "Revelation");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        private void CreateRampJSON(string folderPath)
        {
            string metcFileName = Common.PathCombine(folderPath, "temp.json");

            var json = CreateRampFile(metcFileName);
            //CreateRampId(json);
            CreateRampCreatedOn(json);
            CreateRampReady(json);
            CreateRampTitle(json);
            CreateRampBroadType(json);
            CreateRampTypeMode(json);
            CreateRampFormatMedium(json);
            CreateRampDescStage(json);
            CreateRampVersionType(json);
            CreateVernacularMaterialType(json);
            CreateTypeScriptureType(json);
            CreateRampScriptureScope(json);
            CreateRampHelperVernacularContent(json);
            CreateRampTypeScholarlyWork(json);
            CreateRampSubjectLanguage(json);
            //CreateRampCoverageSpacialRegionHas(json);
            //CreateRampCoverageSpacialCountry(json);
            CreateRampSubjectLanguageHas(json);
            CreateRampLanguageIso(json);
            CreateRampLanguageScript(json);
            CreateRampContributor(json);
            CreateRampModifiedDate(json);
            CreateRampPublisher(json);
            CreateRampFormatExtentText(json);
            CreateRampFormatExtentImages(json);
            //CreateRampDescSponsership(json);
            //CreateRampDescTableofContentsHas(json);
            CreateRampSilDomain(json);
            CreateRampDomainSubTypeLing(json);
            CreateRampSubject(json);
            //CreateRampRelRequiresHas(json);
            CreateRampRelRequires(json);
            CreateRampRelConformsto(json);
            CreateRampRightsHolder(json);
            CreateRampRights(json);
            CreateRampSilSensitivityMetaData(json);
            CreateRampSilSensitivityPresentation(json);
            CreateRampSilSensitivitySource(json);
            CreateRampFiles(json);
            CreateRampDescriptionHas(json);
            CreateRampDescription(json);
            CreateRampStatus(json);
            CloseRampFile(json);
        }

        private static void CloseRampFile(Json json)
        {
            json.EndTag();
            json.Close();
        }

        private void CreateRampStatus(Json json)
        {
            if (Status != null && Status.Trim().Length > 0)
            {
                json.WriteTag("status");
                json.WriteTextNoComma(Status);
            }
        }

        private void CreateRampFiles(Json json)
        {
            if (RampFile.Count > 0)
            {
                json.WriteTag("files");
                json.StartTag();
                for (int i = 0; i < RampFile.Count; i++)
                {
                    json.WriteTag(i.ToString(CultureInfo.InvariantCulture));
                    json.StartTag();
                    json.WriteText(RampFile[i]);
                    json.EndTag();
                    if (i < RampFile.Count - 1)
                        json.WriteComma();
                }
                json.EndTag();
                json.WriteComma();
            }
        }

        private void CreateRampSilSensitivitySource(Json json)
        {
            if (SilSensitivitySource != null && SilSensitivitySource.Trim().Length > 0)
            {
                json.WriteTag("sil.sensitivity.source");
                json.WriteText(SilSensitivitySource);
            }
        }

        private void CreateRampSilSensitivityPresentation(Json json)
        {
            if (SilSensitivityPresentation != null && SilSensitivityPresentation.Trim().Length > 0)
            {
                json.WriteTag("sil.sensitivity.presentation");
                json.WriteText(SilSensitivityPresentation);
            }
        }

        private void CreateRampSilSensitivityMetaData(Json json)
        {
            if (SilSensitivityMetaData != null && SilSensitivityMetaData.Trim().Length > 0)
            {
                json.WriteTag("sil.sensitivity.metadata");
                json.WriteText(SilSensitivityMetaData);
            }
        }

        private void CreateRampRights(Json json)
        {
            if (Rights != null && Rights.Trim().Length > 0)
            {
                json.WriteTag("dc.rights");
                json.WriteText(Rights);
            }
        }

        private void CreateRampRightsHolder(Json json)
        {
            if (RightsHolder.Count > 0)
            {
                json.WriteTag("dc.rightsHolder");
                json.StartTag();
                for (int i = 0; i < RightsHolder.Count; i++)
                {
                    json.WriteTag(i.ToString());
                    json.StartTag();
                    json.WriteText(" \": \"" + RightsHolder[i]);
                    json.EndTag();
                    if (i < RightsHolder.Count - 1)
                        json.WriteComma();
                }
                json.EndTag();
                json.WriteComma();
            }
        }

        private void CreateRampPublisher(Json json)
        {
            if (Publisher != null && Publisher.Trim().Length > 0)
            {
                json.WriteTag("dc.publisher");
                json.WriteText(Publisher);

                if (Publisher.Trim().ToLower() == "wycliffe bible translators")
                {
                    json.WriteTag("dc.identifier.uri");
                    json.WriteText("www.wycliffe.org");
                }
            }
        }

        private void CreateRampRelConformsto(Json json)
        {
            if (RelConformsto != null && RelConformsto.Trim().Length > 0)
            {
                json.WriteTag("dc.relation.conformsto");
                json.WriteText(RelConformsto);
            }
        }

        private void CreateRampRelRequires(Json json)
        {
            //" \": \"OFL"
            if (RelRequires.Count > 0)
            {
                CreateRampRelRequiresHas(json);

                json.WriteTag("dc.relation.requires");
                json.StartTag();
                for (int i = 0; i < RelRequires.Count; i++)
                {
                    json.WriteTag(i.ToString());
                    json.StartTag();
                    json.WriteText(" \": \"" + RelRequires[i]);
                    json.EndTag();
                    if (i < RelRequires.Count - 1)
                        json.WriteComma();
                }
                json.EndTag();
                json.WriteComma();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        private void CreateRampRelRequiresHas(Json json)
        {
            if (RelRequiresHas != null && RelRequiresHas.Trim().Length > 0)
            {
                if (RelRequires.Count > 0)
                {
                    json.WriteTag("relation.requires.has");
                    json.WriteText(RelRequiresHas);
                }
            }
        }

        private void CreateRampSubject(Json json)
        {
            //\" \": \"foreign languages and literature; dictionary;lexicon;\", \"lang\": \"eng\"
            if (Subject.Count > 0)
            {
                json.WriteTag("dc.subject");
                json.StartTag();
                for (int i = 0; i < Subject.Count; i++)
                {
                    string[] subject = Subject[i].Split(',');
                    json.WriteTag(i.ToString());
                    json.StartTag();
                    json.WriteText(" \": \"" + subject[0] + "\", \"lang\": \"" + subject[1]);
                    json.EndTag();
                    if (i < Subject.Count - 1)
                        json.WriteComma();
                }
                json.EndTag();
                json.WriteComma();
            }
        }

        private void CreateRampDomainSubTypeLing(Json json)
        {
            if (DomainSubTypeLing != null && DomainSubTypeLing.Trim().Length > 0)
            {
                json.WriteTag("type.domainSubtype.LING");
                json.WriteRaw(StringWithComma(DomainSubTypeLing));
            }
        }

        private void CreateRampSilDomain(Json json)
        {
            if (SilDomain != null && SilDomain.Trim().Length > 0)
            {
                json.WriteTag("dc.subject.silDomain");
                json.WriteRaw(StringWithComma(SilDomain));
            }
        }

        private void CreateRampFormatExtentImages(Json json)
        {
            if (FormatExtentImages != null && FormatExtentImages.Trim().Length > 0)
            {
                json.WriteTag("format.extent.images");
                json.WriteText(FormatExtentImages);
            }
        }

        private void CreateRampFormatExtentText(Json json)
        {
            if (FormatExtentText != null && FormatExtentText.Trim().Length > 0)
            {
                json.WriteTag("format.extent.text");
                json.WriteText(FormatExtentText);
            }
        }

        private void CreateRampContributor(Json json)
        {
            if (Contributor.Count > 0)
            {
                json.WriteTag("dc.contributor");
                json.StartTag();
                for (int i = 0; i < Contributor.Count; i++)
                {
                    string[] contributors = Contributor[i].Split(',');
                    json.WriteTag(i.ToString());
                    json.StartTag();
                    json.WriteText(" \": \"" + contributors[0] + "\", \"role\": \"" + contributors[1]);
                    json.EndTag();
                    if (i < Contributor.Count - 1)
                        json.WriteComma();
                }
                json.EndTag();
                json.WriteComma();
            }
        }

        private void CreateRampModifiedDate(Json json)
        {
            if (ModifiedDate != null && ModifiedDate.Trim().Length > 0)
            {
                json.WriteTag("dc.date.modified");
                json.WriteText(ModifiedDate);
            }
        }

        private void CreateRampLanguageScript(Json json)
        {
            if (LanguageScript.Count > 0)
            {
                json.WriteTag("dc.language.script");
                json.StartTag();
                for (int i = 0; i < LanguageScript.Count; i++)
                {
                    json.WriteTag(i.ToString());
                    json.StartTag();
                    json.WriteText(" \": \"" + LanguageScript[i]);
                    json.EndTag();
                    if (i < LanguageScript.Count - 1)
                        json.WriteComma();
                }
                json.EndTag();
                json.WriteComma();
            }
        }

        private void CreateRampLanguageIso(Json json)
        {
            if (LanguageIso.Count > 0)
            {
                json.WriteTag("dc.language.iso");
                json.StartTag();
                for (int i = 0; i < LanguageIso.Count; i++)
                {
                    json.WriteTag(i.ToString());
                    json.StartTag();
                    json.WriteText("dialect\" : \"\", \" \": \"" + LanguageIso[i]);
                    json.EndTag();
                    if (i < LanguageIso.Count - 1)
                        json.WriteComma();
                }
                json.EndTag();
                json.WriteComma();
            }
        }

        private void CreateRampSubjectLanguageHas(Json json)
        {
            if (SubjectLanguageHas != null && SubjectLanguageHas.Trim().Length > 0)
            {
                json.WriteTag("subject.subjectLanguage.has");
                json.WriteText(SubjectLanguageHas);
            }
        }

        private void CreateRampSubjectLanguage(Json json)
        {
            if (SubjectLanguage.Count > 0)
            {
                json.WriteTag("dc.subject.subjectLanguage");
                json.StartTag();
                for (int i = 0; i < SubjectLanguage.Count; i++)
                {
                    json.WriteTag(i.ToString());
                    json.StartTag();
                    json.WriteText("dialect\" : \"\", \" \": \"" + SubjectLanguage[i]);
                    json.EndTag();
                    if (i < SubjectLanguage.Count - 1)
                        json.WriteComma();
                }
                json.EndTag();
                json.WriteComma();
            }
        }

        private void CreateRampTypeScholarlyWork(Json json)
        {
            if (TypeScholarlyWork != null && TypeScholarlyWork.Trim().Length > 0)
            {
                json.WriteTag("dc.type.scholarlyWork");
                json.WriteText(TypeScholarlyWork);
            }
        }

        private void CreateVernacularMaterialType(Json json)
        {
            if (VernacularmaterialsType != null && VernacularmaterialsType.Trim().Length > 0)
            {
                json.WriteTag("ramp.vernacularmaterialstype");
                json.WriteText(VernacularmaterialsType);
            }
        }

        private void CreateTypeScriptureType(Json json)
        {
            if (TypeScriptureType != null && TypeScriptureType.Trim().Length > 0)
            {
                json.WriteTag("dc.type.scriptureType");
                json.WriteText(TypeScriptureType);
            }

        }

        private void CreateRampScriptureScope(Json json)
        {
            if (TitleScriptureScope != null && TitleScriptureScope.Trim().Length > 0)
            {
                json.WriteTag("dc.title.scriptureScope");
                json.WriteRaw(StringWithComma(TitleScriptureScope));
            }
        }

        private void CreateRampHelperVernacularContent(Json json)
        {
            if (HelperVernacularContent != null && HelperVernacularContent.Trim().Length > 0)
            {
                json.WriteTag("helper.subject.vernacularContent");
                json.WriteRaw(StringWithComma(HelperVernacularContent));
            }
        }

        private void CreateRampVersionType(Json json)
        {
            if (VersionType != null && VersionType.Trim().Length > 0)
            {
                json.WriteTag("version.type");
                json.WriteText(VersionType);
            }
        }

        private void CreateRampDescStage(Json json)
        {
            if (DescStage != null && DescStage.Trim().Length > 0)
            {
                json.WriteTag("dc.description.stage");
                json.WriteText(DescStage);
            }
        }

        private void CreateRampFormatMedium(Json json)
        {
            if (FormatMedium != null && FormatMedium.Trim().Length > 0)
            {
                json.WriteTag("dc.format.medium");
                json.WriteRaw(StringWithComma(FormatMedium));
            }
        }

        private void CreateRampTypeMode(Json json)
        {
            if (TypeMode != null && TypeMode.Trim().Length > 0)
            {
                json.WriteTag("dc.type.mode");
                json.WriteRaw(StringWithComma(TypeMode));
            }
        }

        private void CreateRampBroadType(Json json)
        {
            if (BroadType != null && BroadType.Trim().Length > 0)
            {
                json.WriteTag("broad_type");
                json.WriteText(BroadType);
            }
        }

        private void CreateRampTitle(Json json)
        {
            if (Title != null && Title.Trim().Length > 0)
            {
                json.WriteTag("dc.title");
                json.WriteText(Title);
            }
        }

        private void CreateRampReady(Json json)
        {
            if (Ready != null && Ready.Trim().Length > 0)
            {
                json.WriteTag("ramp.is_ready");
                json.WriteText(Ready);
            }
        }

        private static Json CreateRampFile(string metcFileName)
        {
            Json json = new Json();
            json.Create(metcFileName);
            json.StartTag();
            return json;
        }

        private void CreateRampCreatedOn(Json json)
        {
            if (CreatedOn != null && CreatedOn.Trim().Length > 0)
            {
                json.WriteTag("created_at");
                json.WriteText(CreatedOn);
            }
        }

        private void CreateRampDescriptionHas(Json json)
        {
            if (RampDescriptionHas != null && RampDescriptionHas.Trim().Length > 0)
            {
                json.WriteTag("description.has");
                json.WriteText(RampDescriptionHas);
            }
        }

        private void CreateRampDescription(Json json)
        {
            if (RampDescription != null && RampDescription.Trim().Length > 0)
            {
                json.WriteTag("dc.description");
                json.StartTag();
                int i = 0;
                json.WriteTag(i.ToString());
                json.StartTag();
                json.WriteText(" \" : \"" + RampDescription.Replace("\n", " ").Replace("\r", "") + "\", \"lang\": \"eng");
                json.EndTag();
                json.EndTag();
                json.WriteComma();
            }
        }

        /// <summary>
        /// Converts "Text,Photograph,Software application" to "[ \"Text\", \"Photograph\", \"Software application\"]"
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string StringWithComma(string text)
        {
            string result = string.Empty;
            string[] values = text.Split(',');
            result = "[";
            for (int i = 0; i < values.Length; i++)
            {
                if(values[i].Trim().Length == 0) continue;
                result = result + "\"" + values[i];
                if (i < values.Length - 1)
                    result = result + "\", ";
            }
            result = result + "\"]";
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputText">string to encode / decode</param>
        /// <param name="operation">encode/decode</param>
        /// <returns></returns>
        private string EncodeDecodeBase64(string inputText, string operation)
        {
            string convertedText = string.Empty;
            if (operation == "encode")
            {
                byte[] bytesToEncode = Encoding.UTF8.GetBytes(inputText);
                convertedText = Convert.ToBase64String(bytesToEncode);
            }
            else if (operation == "decode")
            {
                byte[] decodedBytes = Convert.FromBase64String(inputText);
                convertedText = Encoding.UTF8.GetString(decodedBytes);
            }
            return convertedText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetLicenseFileName()
        {
            Param.LoadSettings();
            string firstPart = Param.GetMetadataValue(Param.CopyrightHolder);
            _outputFileTitle = Param.GetMetadataValue(Param.Title);
            firstPart = Common.UpdateCopyrightYear(firstPart);
            string secondPart = GetLicenseInformation(Param.GetMetadataValue(Param.CopyrightPageFilename));
            return firstPart + " " + secondPart.Replace("\r", "").Replace("\n", "").Replace("\t", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string GetLicenseInformation(string filename)
        {
            string text = string.Empty;
            string licenseXml = Common.GetPSApplicationPath();
            if(File.Exists(filename))
            {
                if(filename.ToLower().IndexOf("pathway7") > 0)
                {
                    if (!filename.Contains("Copyrights"))
                    {
                        licenseXml = Common.PathCombine(licenseXml, "Copyrights");
                    }
                    licenseXml = Common.PathCombine(licenseXml, filename);
					if (!File.Exists(licenseXml))
					{
						licenseXml = Common.PathCombine(Path.GetDirectoryName(Common.AssemblyPath), "Copyrights");
					}
                }
                else
                {
                    licenseXml = filename;
                }
                try
                {
                    XmlDocument xDoc = Common.DeclareXMLDocument(true);
                    var namespaceManager = new XmlNamespaceManager(xDoc.NameTable);
                    namespaceManager.AddNamespace("x", "http://www.w3.org/1999/xhtml");
                    xDoc.Load(licenseXml);
                    const string xPath = "//x:div[@id='LicenseInformation']";
                    XmlNodeList nodeList = xDoc.SelectNodes(xPath, namespaceManager);
                    if (nodeList != null && nodeList.Count > 0)
                    {
                        if (nodeList[nodeList.Count - 1] != null)
                        {
                            text = nodeList[nodeList.Count - 1].InnerText;
                        }
                    }
                }
                catch
                {
                    text = string.Empty;
                }
            }
            else
            {
                text = string.Empty;
            }
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xhtmlFileName"></param>
        /// <returns></returns>
        public string GetImageCount(string xhtmlFileName)
        {
            string imageCount = "0";
            XmlDocument xmlDocument = Common.DeclareXMLDocument(true);
            xmlDocument.Load(xhtmlFileName);
            XmlNodeList nodes = xmlDocument.GetElementsByTagName("img");
            if (nodes.Count > 0)
            {
                imageCount = nodes.Count.ToString();
            }
            return imageCount;
        }



        /// <summary>
        /// 
        /// </summary>
        private void CompressToRamp()
        {
            List<string> filesCollection = new List<string>();
            filesCollection.AddRange(Directory.GetFiles(Path.GetDirectoryName(_folderPath)));

            List<string> dirCollection = new List<string>();
            dirCollection.Add(Common.PathCombine(Path.GetDirectoryName(_folderPath), "USX"));
            dirCollection.Add(Common.PathCombine(Path.GetDirectoryName(_folderPath), "SFM"));
            dirCollection.Add(Common.PathCombine(Path.GetDirectoryName(_folderPath), "Pictures"));

            if (_outputFileTitle == string.Empty)
            {
                _outputFileTitle = "Default Title";
            }
            _outputFileTitle = Common.ReplaceSymbolToUnderline(_outputFileTitle);

            using (ZipFile zipFile = ZipFile.Create(Common.PathCombine(Path.GetDirectoryName(_folderPath), _outputFileTitle) + ".ramp"))
            {
                zipFile.NameTransform = new ZipNameTransform(Path.GetDirectoryName(_folderPath));
                zipFile.BeginUpdate();
                foreach (string file in filesCollection)
                {
                    zipFile.Add(file, CompressionMethod.Stored);
                }

                foreach (var dirPath in dirCollection)
                {
                    if (Directory.Exists(dirPath))
                    {
                        zipFile.AddDirectory(dirPath);
                        List<string> subfilesCollection = new List<string>();
                        subfilesCollection.AddRange(Directory.GetFiles(dirPath));
                        foreach (string subfile in subfilesCollection)
                        {
                            zipFile.Add(subfile, CompressionMethod.Stored);
                        }
                    }
                }
                zipFile.CommitUpdate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encodedKey"></param>
        private void UpdateMetsXML(string encodedKey)
        {
            string fileGroup = CreateFileGroup();
            string structMap = CreateStructMap();

            Param.LoadSettings();
            string xmlSettings = Common.PathCombine(Path.GetDirectoryName(Param.SettingPath), "mets.xml");
            if (File.Exists(xmlSettings))
            {
                string xmlFileNewPath = Common.PathCombine(Path.GetDirectoryName(_folderPath), Path.GetFileName(xmlSettings));
                if (File.Exists(xmlFileNewPath))
                {
                    File.Delete(xmlFileNewPath);
                }
                File.Copy(xmlSettings, xmlFileNewPath);

                File.WriteAllText(xmlFileNewPath, File.ReadAllText(xmlFileNewPath).Replace("<binData>abc1defg", "<binData>" + encodedKey));
                File.WriteAllText(xmlFileNewPath, File.ReadAllText(xmlFileNewPath).Replace("<file>ecgroupFileUpdate1</file>", fileGroup));
                File.WriteAllText(xmlFileNewPath, File.ReadAllText(xmlFileNewPath).Replace("<div>ecgroupFileUpdate2</div>", structMap));
            }


        }

        /// <summary>
        /// <file GROUPID="sword-mets-fgid-0" ID="sword-mets-file-0">
        /// <FLocat LOCTYPE="URL" xlink:href="main.odm" USE="preferred"/>
        /// </file>
        /// </summary>
        /// <returns></returns>
        private string CreateFileGroup()
        {
            StringBuilder fileGrp = new StringBuilder();
            int i = 0;
            foreach (KeyValuePair<string, string> data in _fileList)
            {
                string firstLine = string.Format("<file GROUPID=\"sword-mets-fgid-{0}\" ID=\"sword-mets-file-{0}\">", i);
                fileGrp.AppendLine(firstLine);
                string secondLine = string.Format("<FLocat LOCTYPE=\"URL\" xlink:href=\"{0}\" USE=\"{1}\"/>", data.Key, data.Value);
                fileGrp.AppendLine(secondLine);
                fileGrp.AppendLine("</file>");
                i++;
            }
            return fileGrp.ToString();
        }

        /// <summary>
        ///  <div TYPE="File" ID="sword-mets-div-2">
        ///  <fptr FILEID="sword-mets-file-0"/>
        ///   </div>
        /// </summary>
        /// <returns></returns>
        private string CreateStructMap()
        {
            StringBuilder structMap = new StringBuilder();
            for (int i = 0; i < _fileList.Count; i++)
            {
                string firstLine = string.Format("<div TYPE=\"File\" ID=\"sword-mets-div-{0}\">", i + 2);
                structMap.AppendLine(firstLine);
                string secondLine = string.Format("<fptr FILEID=\"sword-mets-file-{0}\"/>", i);
                structMap.AppendLine(secondLine);
                structMap.AppendLine("</div>");
            }
            return structMap.ToString();
        }

        private string GetNumberOfPdfPages(string fileName)
        {
            string pageCount = "0";
            if (Path.GetExtension(fileName).ToLower() != "pdf")
                return pageCount;

            using (StreamReader sr = new StreamReader(File.OpenRead(fileName)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());
                pageCount = matches.Count.ToString();
            }
            return pageCount;
        }

        #endregion
    }
}
