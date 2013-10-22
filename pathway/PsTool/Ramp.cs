// --------------------------------------------------------------------------------------------
// <copyright file="MergeCss.cs" from='2009' to='2009' company='SIL International'>
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
// Combines all css into a single file by implementing @import
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    /// <summary>
    /// Combines all css into a single file by implementing @import
    /// </summary>
    public class Ramp
    {

        #region Public Variables

        private string _rampId;
        private string _createdOn;
        private string _ready;
        private string _title;
        private string _broadType;
        private string _typeMode;
        private string _formatMedium;
        private string _descStage;
        private string _versionType;
        private string _typeScholarlyWork;
        private List<string> _subjectLanguage = new List<string>();
        private string _coverageSpacialRegionHas;
        private List<string> _coverageSpacialCountry = new List<string>();
        private string _subjectLanguageHas;
        private List<string> _languageScript = new List<string>();
        private string _formatExtentText;
        private string _formatExtentImages;
        private string _descSponsership;
        private string _descTableofContentsHas;
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
        private List<string> _languageIso = new List<string>();
        private List<string> _contributor = new List<string>();
        private Dictionary<string, string> _fileList = new Dictionary<string, string>();
        private string _folderPath = string.Empty;
        private string _outputExtension = string.Empty;
        private string _projInputType = string.Empty;
        #endregion

        #region Private variable
        private RampFile rampFile;
        private Dictionary<string, string> _isoLanguageCode = new Dictionary<string, string>();
        private Dictionary<string, string> _isoLanguageCodeandName = new Dictionary<string, string>();
        private Dictionary<string, string> _isoLanguageScriptandName = new Dictionary<string, string>();
        #endregion

        #region Property
        public string RampId
        {
            get { return _rampId; }
            set { _rampId = value; }
        }


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

        public string CreatedOn
        {
            get { return _createdOn; }
            set { _createdOn = value; }
        }

        public string Ready
        {
            get { return _ready; }
            set { _ready = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string BroadType
        {
            get { return _broadType; }
            set { _broadType = value; }
        }

        public string TypeMode
        {
            get { return _typeMode; }
            set { _typeMode = value; }
        }

        public string FormatMedium
        {
            get { return _formatMedium; }
            set { _formatMedium = value; }
        }

        public string DescStage
        {
            get { return _descStage; }
            set { _descStage = value; }
        }

        public string VersionType
        {
            get { return _versionType; }
            set { _versionType = value; }
        }

        public string TypeScholarlyWork
        {
            get { return _typeScholarlyWork; }
            set { _typeScholarlyWork = value; }
        }

        public string CoverageSpacialRegionHas
        {
            get { return _coverageSpacialRegionHas; }
            set { _coverageSpacialRegionHas = value; }
        }

        public string SubjectLanguageHas
        {
            get { return _subjectLanguageHas; }
            set { _subjectLanguageHas = value; }
        }

        public string FormatExtentText
        {
            get { return _formatExtentText; }
            set { _formatExtentText = value; }
        }

        public string FormatExtentImages
        {
            get { return _formatExtentImages; }
            set { _formatExtentImages = value; }
        }

        public string DescSponsership
        {
            get { return _descSponsership; }
            set { _descSponsership = value; }
        }

        public string DescTableofContentsHas
        {
            get { return _descTableofContentsHas; }
            set { _descTableofContentsHas = value; }
        }

        public string SilDomain
        {
            get { return _silDomain; }
            set { _silDomain = value; }
        }

        public string DomainSubTypeLing
        {
            get { return _domainSubTypeLing; }
            set { _domainSubTypeLing = value; }
        }

        public string RelRequiresHas
        {
            get { return _relRequiresHas; }
            set { _relRequiresHas = value; }
        }

        public string RelConformsto
        {
            get { return _relConformsto; }
            set { _relConformsto = value; }
        }

        public string SilSensitivityMetaData
        {
            get { return _silSensitivityMetaData; }
            set { _silSensitivityMetaData = value; }
        }

        public string SilSensitivityPresentation
        {
            get { return _silSensitivityPresentation; }
            set { _silSensitivityPresentation = value; }
        }

        public string Rights
        {
            get { return _rights; }
            set { _rights = value; }
        }

        public string SilSensitivitySource
        {
            get { return _silSensitivitySource; }
            set { _silSensitivitySource = value; }
        }

        public List<string> RampFile
        {
            get { return _rampFile; }
            set { _rampFile = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        //public Dictionary<string, string> IsoLanguageCode
        //{
        //    get { return _isoLanguageCode; }
        //    set { _isoLanguageCode = value; }
        //}

        //public Dictionary<string, string> IsoLanguageCodeandName
        //{
        //    get { return _isoLanguageCodeandName; }
        //    set { _isoLanguageCodeandName = value; }
        //}

        public string ProjInputType
        {
            get { return _projInputType; }
            set { _projInputType = value; }
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

        private void LoadLanguagefromXML()
        {
            string xmlFilePath = Common.PathCombine(Common.GetApplicationPath(), "RampLangCode.xml"); ;
            XmlDocument xDoc = Common.DeclareXMLDocument(false);
            xDoc.Load(xmlFilePath);
            const string twoLetterLangXPath = "//LanguageCode/IsoLanguageCodeTwoLetters/Language";
            const string threeLetterLangXPath = "//LanguageCode/IsoLanguageCodeThreeLetters/Language";
            const string scriptLangXPath = "//LanguageCode/LanguageScript/Language";
            XmlNodeList twoLetterLangList = xDoc.SelectNodes(twoLetterLangXPath);
            if(twoLetterLangList != null && twoLetterLangList.Count > 0)
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
        public void AddSubjLanguage(string lang)
        {
            if(lang.IndexOf(':') > 0)
            {
                SubjectLanguage.Add(lang);
            }
            else
            {
                string langCode = string.Empty;
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

        /// <summary>
        /// 
        /// </summary>
        public void AddLanguageIso(string langCollection)
        {
            string[] languageCode = langCollection.Split(',');
            foreach (string s in languageCode)
            {
                if(s.Trim().Length == 0)
                    continue;

                if(s.IndexOf(':') > 0)
                {
                    string[] val = s.Split(':');
                    string code = val[0];
                    string codeName = val[1];

                    if (code.Trim().Length == 2)
                    {
                        if (_isoLanguageCode.ContainsKey(code))
                        {
                            code = _isoLanguageCode[code];
                        }
                        //if (code.Trim().Length > 0 && _isoLanguageCodeandName.ContainsKey(code))
                        if (code.Trim().Length > 0)
                        {
                            string format = code + ":" + codeName; //"eng:English"
                            LanguageIso.Add(format);
                        }
                    }
                    else
                    {
                        LanguageIso.Add(s);
                    }
                }
                else
                {
                    if(s.Length == 3)
                    {
                        LanguageIso.Add(s + ":" + _isoLanguageCodeandName[s]);
                    }
                }
            }
        }

        public void AddCoverageSpacialCountry(string country)
        {
            CoverageSpacialCountry.Add(country);
        }

        public void AddLanguageScript(string langCollection)
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

        public void AddContributor(string contrib)
        {
            Contributor.Add(contrib);
        }

        public void AddSubject(string subj)
        {
            Subject.Add(subj);
        }

        public void AddRelRequires(string text)
        {
            RelRequires.Add(text);
        }

        public void AddRightsHolder(string value)
        {
            RightsHolder.Add(value);
        }

        public void AddFile(RampFile file)
        {
            //\" \": \"main.odm\", \"description\": \"Master document\", \"relationship\": \"presentation\", \"is_primary\": \"Y\", \"silPublic\": \"Y\"
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

            string metcFileName = Path.Combine(Path.GetDirectoryName(_folderPath), "temp.json");

            string text = File.ReadAllText(metcFileName);

            string rampEncodedKey = EncodeDecodeBase64(text, "encode");

            File.Delete(metcFileName);

            return rampEncodedKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputExtension"></param>
        private void SetRampData()
        {
            Param.LoadSettings();
            //ramp.RampId = "ykmb9i6zlh";
            CreatedOn = DateTime.Now.ToString("r");
            Ready = "Y";
            Title = Param.GetMetadataValue(Param.Title, Param.GetOrganization());
            BroadType = "wider_audience";
            TypeMode = "Text,Photograph,Software application";
            //FormatMedium = "Paper,Other";
            //DescStage = "rough_draft";
            //VersionType = "first";
            TypeScholarlyWork = "Book";
            AddSubjLanguage(Common.GetLanguageCode(_folderPath, _projInputType));
            //CoverageSpacialRegionHas = "Y";//
            //AddCoverageSpacialCountry("IN:India, Andhra Pradesh");//
            SubjectLanguageHas = "Y";//
            AddLanguageIso(Common.GetLanguageCodeList(_folderPath, _projInputType));
            AddLanguageScript(Common.GetLanguageScriptList(_folderPath, _projInputType));//
            //AddLanguageScript("Telu:Telugu");//
            //AddLanguageScript("Deva:Devanagari (Nagari)");//
            AddContributor(Param.GetMetadataValue(Param.Creator) + ",compiler");//
            //FormatExtentText = "8";
            //FormatExtentImages = GetImageCount(publicationInfo.DefaultXhtmlFileWithPath);//
            //DescSponsership = Param.GetOrganization();
            //DescTableofContentsHas = " ";//
            SilDomain = "LING:Linguistics";//
            DomainSubTypeLing = "lexicon (LING)";//
            AddSubject(Param.GetMetadataValue(Param.Subject) + ",eng");//
            //RelRequiresHas = "Y";
            //AddRelRequires("OFL");
            RelConformsto = "odf";
            AddRightsHolder(Param.GetMetadataValue(Param.CopyrightHolder));//
            //Rights = GetLicenseFileName();//
            SilSensitivityMetaData = "Public";
            SilSensitivityPresentation = "Public";
            SilSensitivitySource = "Insite users";

            if (_folderPath != null)
            {
                string[] files = Directory.GetFiles(Path.GetDirectoryName(_folderPath));
                foreach (string file in files)
                {
                    RampFile rFile = new RampFile();
                    rFile.FileName = Path.GetFileName(file);
                    string fileExtn = Path.GetExtension(file);
                    if (fileExtn == ".odm" || fileExtn == ".jar" || fileExtn == ".epub" || fileExtn == ".mybible")
                    {

                        rFile.FileDescription = Path.GetFileNameWithoutExtension(file) + " " + fileExtn.Replace(".", "") + " document";
                        rFile.FileRelationship = "presentation";
                        rFile.FileIsPrimary = "Y";
                        rFile.FileSilPublic = "Y";
                    }
                    else if (fileExtn == ".odt" || fileExtn == ".pdf" || fileExtn == ".jad" || fileExtn == ".nt")
                    {
                        rFile.FileDescription =  Path.GetFileNameWithoutExtension(file) + " " + fileExtn.Replace(".", "") + " document";
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        private void CreateRampJSON(string folderPath)
        {
            string metcFileName = Path.Combine(folderPath, "temp.json");

            var json = CreateRampFile(metcFileName);
            //CreateRampId(json);
            CreateRampCreatedOn(json);
            CreateRampReady(json);
            CreateRampTitle(json);
            CreateRampBroadType(json);
            CreateRampTypeMode(json);
            //CreateRampFormatMedium(json);
            //CreateRampDescStage(json);
            //CreateRampVersionType(json);
            CreateRampTypeScholarlyWork(json);
            CreateRampSubjectLanguage(json);
            //CreateRampCoverageSpacialRegionHas(json);
            //CreateRampCoverageSpacialCountry(json);
            CreateRampSubjectLanguageHas(json);
            CreateRampLanguageIso(json);
            CreateRampLanguageScript(json);
            CreateRampContributor(json);
            //CreateRampFormatExtentText(json);
            //CreateRampFormatExtentImages(json);
            //CreateRampDescSponsership(json);
            //CreateRampDescTableofContentsHas(json);
            CreateRampSilDomain(json);
            CreateRampDomainSubTypeLing(json);
            CreateRampSubject(json);
            //CreateRampRelRequiresHas(json);
            //CreateRampRelRequires(json);
            CreateRampRelConformsto(json);
            CreateRampRightsHolder(json);
            //CreateRampRights(json);
            CreateRampSilSensitivityMetaData(json);
            CreateRampSilSensitivityPresentation(json);
            CreateRampSilSensitivitySource(json);
            CreateRampFiles(json);
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
            if (Status.Trim().Length > 0)
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
            if (SilSensitivitySource.Trim().Length > 0)
            {
                json.WriteTag("sil.sensitivity.source");
                json.WriteText(SilSensitivitySource);
            }
        }

        private void CreateRampSilSensitivityPresentation(Json json)
        {
            if (SilSensitivityPresentation.Trim().Length > 0)
            {
                json.WriteTag("sil.sensitivity.presentation");
                json.WriteText(SilSensitivityPresentation);
            }
        }

        private void CreateRampSilSensitivityMetaData(Json json)
        {
            if (SilSensitivityMetaData.Trim().Length > 0)
            {
                json.WriteTag("sil.sensitivity.metadata");
                json.WriteText(SilSensitivityMetaData);
            }
        }

        private void CreateRampRights(Json json)
        {
            if (Rights.Trim().Length > 0)
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

        private void CreateRampRelConformsto(Json json)
        {
            if (RelConformsto.Trim().Length > 0)
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

        private void CreateRampRelRequiresHas(Json json)
        {
            if (RelRequiresHas.Trim().Length > 0)
            {
                json.WriteTag("relation.requires.has");
                json.WriteText(RelRequiresHas);
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
            if (DomainSubTypeLing.Trim().Length > 0)
            {
                json.WriteTag("type.domainSubtype.LING");
                json.WriteRaw(StringWithComma(DomainSubTypeLing));
            }
        }

        private void CreateRampSilDomain(Json json)
        {
            if (SilDomain.Trim().Length > 0)
            {
                json.WriteTag("dc.subject.silDomain");
                json.WriteRaw(StringWithComma(SilDomain));
            }
        }

        private void CreateRampDescTableofContentsHas(Json json)
        {
            if (DescTableofContentsHas.Trim().Length > 0)
            {
                json.WriteTag("description.tableofcontents.has");
                json.WriteText(DescTableofContentsHas);
            }
        }

        private void CreateRampDescSponsership(Json json)
        {
            if (DescSponsership.Trim().Length > 0)
            {
                json.WriteTag("dc.description.sponsorship");
                json.WriteText(DescSponsership);
            }
        }

        private void CreateRampFormatExtentImages(Json json)
        {
            if (FormatExtentImages.Trim().Length > 0)
            {
                json.WriteTag("format.extent.images");
                json.WriteText(FormatExtentText);
            }
        }

        private void CreateRampFormatExtentText(Json json)
        {
            if (FormatExtentText.Trim().Length > 0)
            {
                json.WriteTag("format.extent.text");
                json.WriteText(FormatExtentText);
            }
        }

        private void CreateRampContributor(Json json)
        {
            //" \": \"Mark Penny\", \"role\": \"researcher"
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

        private void CreateRampLanguageScript(Json json)
        {
            //" \": \"Latn: Latin"
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
            if (SubjectLanguageHas.Trim().Length > 0)
            {
                json.WriteTag("subject.subjectLanguage.has");
                json.WriteText(SubjectLanguageHas);
            }
        }

        private void CreateRampCoverageSpacialCountry(Json json)
        {
            //" \": \"IN: India\", \"place\": \"Andhra Pradesh\""
            if (CoverageSpacialCountry.Count > 0)
            {
                json.WriteTag("coverage.spatial.country");
                json.StartTag();
                for (int i = 0; i < CoverageSpacialCountry.Count; i++)
                {
                    string[] cntry = CoverageSpacialCountry[i].Split(',');
                    json.WriteTag(i.ToString());
                    json.StartTag();
                    json.WriteText(" \": \"" + cntry[0] + "\", \"place\": \"" + cntry[1]);
                    json.EndTag();
                    if (i < CoverageSpacialCountry.Count - 1)
                        json.WriteComma();
                }
                json.EndTag();
                json.WriteComma();
            }
        }

        private void CreateRampCoverageSpacialRegionHas(Json json)
        {
            if (CoverageSpacialRegionHas.Trim().Length > 0)
            {
                json.WriteTag("coverage.spatial.region.has");
                json.WriteText(CoverageSpacialRegionHas);
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
            if (TypeScholarlyWork.Trim().Length > 0)
            {
                json.WriteTag("dc.type.scholarlyWork");
                json.WriteText(TypeScholarlyWork);
            }
        }

        private void CreateRampVersionType(Json json)
        {
            if (VersionType.Trim().Length > 0)
            {
                json.WriteTag("version.type");
                json.WriteText(VersionType);
            }
        }

        private void CreateRampDescStage(Json json)
        {
            if (DescStage.Trim().Length > 0)
            {
                json.WriteTag("dc.description.stage");
                json.WriteText(DescStage);
            }
        }

        private void CreateRampFormatMedium(Json json)
        {
            if (FormatMedium.Trim().Length > 0)
            {
                json.WriteTag("dc.format.medium");
                json.WriteRaw(StringWithComma(FormatMedium));
            }
        }

        private void CreateRampTypeMode(Json json)
        {
            if (TypeMode.Trim().Length > 0)
            {
                json.WriteTag("dc.type.mode");
                json.WriteRaw(StringWithComma(TypeMode));
            }
        }

        private void CreateRampBroadType(Json json)
        {
            if (BroadType.Trim().Length > 0)
            {
                json.WriteTag("broad_type");
                json.WriteText(BroadType);
            }
        }

        private void CreateRampTitle(Json json)
        {
            if (Title.Trim().Length > 0)
            {
                json.WriteTag("dc.title");
                json.WriteText(Title);
            }
        }

        private void CreateRampReady(Json json)
        {
            if (Ready.Trim().Length > 0)
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
            if (CreatedOn.Trim().Length > 0)
            {
                json.WriteTag("created_at");
                json.WriteText(CreatedOn);
                //json.WriteText(DateTime.Now.ToString("r"));
            }
        }

        private void CreateRampId(Json json)
        {
            if (RampId.Length > 0)
            {
                json.WriteTag("id");
                json.WriteText(RampId);
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
            string licFileName = String.Empty;
            string licenseFileName = Path.GetFileName(Param.CopyrightPageFilename);
            XmlNode node =
                Param.GetItem("//features/feature[@name='Lic_SIL_x0020_International'/option[@file=' " + licenseFileName +
                              "']");
            if (node != null)
            {
                licFileName = node.Attributes["name"].Value;
            }
            return licFileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xhtmlFileName"></param>
        /// <returns></returns>
        public string GetImageCount(string xhtmlFileName)
        {
            string imageCount = "0";
            XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
            xmlDocument.Load(xhtmlFileName);
            XmlNodeList nodes = xmlDocument.SelectNodes("//img");
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
            dirCollection.Add(Path.Combine(Path.GetDirectoryName(_folderPath),"USX"));
            dirCollection.Add(Path.Combine(Path.GetDirectoryName(_folderPath),"SFM"));

            using (ZipFile zipFile = ZipFile.Create(Common.PathCombine(Path.GetDirectoryName(_folderPath), Param.GetMetadataValue(Param.Title)) + ".ramp"))
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
                    }
                }
                zipFile.CommitUpdate();
            }
            CleanUpFolder(filesCollection, Path.GetDirectoryName(_folderPath));
        }

        private void CleanUpFolder(List<string> files, string folderPath)
        {
            List<string> validExtension = new List<string>();
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);

            validExtension.AddRange(_outputExtension.Split(','));

            foreach (var file in files)
            {
                string ext = Path.GetExtension(file);

                if (!validExtension.Contains(ext))
                {
                    File.Delete(file);
                }
            }

            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                subfolder.Delete(true);
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
            string xmlSettings = Path.Combine(Path.GetDirectoryName(Param.SettingPath), "mets.xml");
            if (File.Exists(xmlSettings))
            {
                string xmlFileNewPath = Path.Combine(Path.GetDirectoryName(_folderPath), Path.GetFileName(xmlSettings));
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


        #endregion
    }
}
