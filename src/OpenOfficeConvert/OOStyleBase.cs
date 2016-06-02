// --------------------------------------------------------------------------------------------
// <copyright file="OOStyleBase.cs" from='2009' to='2014' company='SIL International'>
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
// Creates the OOStyle base file
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Win32;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class LOStyleBase
    {
        #region protected Variable
        protected PublicationInformation _projInfo = new PublicationInformation();
        protected XmlTextWriter _writer;
        public OldStyles _styleName = new OldStyles();
        protected Dictionary<string, string> _allParagraphProperty;
        protected Dictionary<string, string> _allTextProperty;
        protected Dictionary<string, string> _allPageLayoutProperty;
        protected Dictionary<string, string> _allColumnProperty;
        protected Dictionary<string, string> _paragraphProperty = new Dictionary<string, string>();
        protected Dictionary<string, string> _textProperty = new Dictionary<string, string>();
        protected Dictionary<string, string> _pageLayoutProperty;
        protected ArrayList _baseTagName = new ArrayList();  // for insert tagName
        protected ArrayList _allTagName = new ArrayList();  // for all tagName
        protected Dictionary<string, string>[] _pageHeaderFooter;
        private Dictionary<string, Dictionary<string, string>> _tagProperty = new Dictionary<string, Dictionary<string, string>>();
        protected VerboseClass _verboseWriter = VerboseClass.GetInstance();

        protected Dictionary<string, string> _columnProperty  = new Dictionary<string, string>();
        protected Dictionary<string, string> _sectionProperty;
        protected Dictionary<string, string> _firstPageLayoutProperty;
        protected Dictionary<string, string> _leftPageLayoutProperty;
        protected Dictionary<string, string> _rightPageLayoutProperty;
        protected Dictionary<string, string> _columnSep;
        protected ArrayList _firstPageContentNone = new ArrayList();
        #endregion

        #region public Variable
        public bool isMirrored = false; //TD-410
        public static string HeaderRule; // TD-1007(Add a ruling line to the bottom of the header.)
        #endregion



        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// create attributes collection
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <returns></returns>
        /// -------------------------------------------------------------------------------------------
        protected void InitializeObject(string outputFile)
        {
            // Creating new Objects
            _columnProperty = new Dictionary<string, string>();
            _sectionProperty = new Dictionary<string, string>();
            _columnSep = new Dictionary<string, string>();
            _pageHeaderFooter = new Dictionary<string, string>[24];
            _paragraphProperty = new Dictionary<string, string>();
            _textProperty = new Dictionary<string, string>();
            isMirrored = false;

            _styleName.SpellCheck = new Dictionary<string, ArrayList>();
            _styleName.AttribAncestor = new Dictionary<string, ArrayList>();
            _styleName.AttribLangBeforeList = new Dictionary<string, string>(); //PseudoLang
            _styleName.BackgroundColor = new ArrayList();
            _styleName.BorderProperty = new Dictionary<string, string>();
            _styleName.ClassContent = new Dictionary<string, string>();
            _styleName.ClearProperty = new Dictionary<string, string>();
            _styleName.ColumnGapEm = new Dictionary<string, Dictionary<string, string>>();
            _styleName.ContentCounter = new Dictionary<string, int>();
            _styleName.ContentCounterReset = new Dictionary<string, string>();
            _styleName.CounterParent = new Dictionary<string, Dictionary<string, string>>();
            _styleName.CssClassName = new Dictionary<string, IDictionary<string, string>>();
            _styleName.DisplayBlock = new ArrayList();
            _styleName.DisplayFootNote = new ArrayList();
            _styleName.DisplayInline = new ArrayList();
            _styleName.FloatAlign = new Dictionary<string, string>();
            _styleName.FootNoteCall = new Dictionary<string, string>();
            _styleName.FootNoteMarker = new Dictionary<string, string>();
            _styleName.FootNoteSeperator = new Dictionary<string, string>();
            _styleName.ImageSize = new Dictionary<string, ArrayList>();
            _styleName.IsMacroEnable = false;
            _styleName.MasterDocument = new ArrayList();
            _styleName.PseudoAncestorBefore = new Dictionary<string, string>();
            _styleName.PseudoAttrib = new Dictionary<string, string>();
            _styleName.PseudoClass = new ArrayList();
            _styleName.PseudoClassAfter = new Dictionary<string, string>();
            _styleName.PseudoClassBefore = new Dictionary<string, string>();
            _styleName.PseudoPosition = new ArrayList();
            _styleName.SectionName = new ArrayList();
            _styleName.TagAttrib = new Dictionary<string, string>();
            _styleName.ListType = new Dictionary<string, string>();
            _styleName.WhiteSpace = new ArrayList();
            _styleName.ImageSource = new Dictionary<string, Dictionary<string, string>>();
            _styleName.AllCSSName = new ArrayList();
            _styleName.DropCap = new ArrayList();
            _styleName.ReplaceSymbolToText = new Dictionary<string, string>();
            _styleName.ReferenceFormat = "";
            _styleName.PrecedeClass = new Dictionary<string, string>();
            _styleName.IsAutoWidthforCaption = false;
            _styleName.VisibilityClassName = new Dictionary<string, string>();
            

            _styleName.PseudoWithoutStyles = new List<string>();

            for (int i = 0; i <= 23; i++)
            {
                _pageHeaderFooter[i] = new Dictionary<string, string>();
            }
        }

        protected void LoadAllProperty()
        {
            // Inialize style properties
            _allParagraphProperty = new Dictionary<string, string>();
            _allColumnProperty = new Dictionary<string, string>();
            _allTextProperty = new Dictionary<string, string>();
            _allPageLayoutProperty = new Dictionary<string, string>();
            _pageLayoutProperty = new Dictionary<string, string>();
            _firstPageLayoutProperty = new Dictionary<string, string>();
            _styleName.ColumnGapEm = new Dictionary<string, Dictionary<string, string>>();

            try
            {
                _allParagraphProperty.Add("text-align", "fo:");
                _allParagraphProperty.Add("text-indent", "fo:");
                _allParagraphProperty.Add("margin-left", "fo:");
                _allParagraphProperty.Add("margin-right", "fo:");
                _allParagraphProperty.Add("margin-top", "fo:");
                _allParagraphProperty.Add("margin-bottom", "fo:");
                _allParagraphProperty.Add("auto-text-indent", "style:");
                _allParagraphProperty.Add("padding", "fo:");
                _allParagraphProperty.Add("background-color", "fo:");
                _allParagraphProperty.Add("break-before", "fo:");
                _allParagraphProperty.Add("line-spacing", "style:");
                _allParagraphProperty.Add("line-height", "fo:");
                _allParagraphProperty.Add("line-height-at-least", "style:");
                _allParagraphProperty.Add("border-bottom", "fo:");
                _allParagraphProperty.Add("border-top", "fo:");
                _allParagraphProperty.Add("border-left", "fo:");
                _allParagraphProperty.Add("border-right", "fo:");
                _allParagraphProperty.Add("padding-left", "fo:");
                _allParagraphProperty.Add("padding-right", "fo:");
                _allParagraphProperty.Add("padding-bottom", "fo:");
                _allParagraphProperty.Add("padding-top", "fo:");
                _allParagraphProperty.Add("vertical-align", "style:");
                _allParagraphProperty.Add("writing-mode", "style:");
                _allParagraphProperty.Add("widows", "fo:");
                _allParagraphProperty.Add("orphans", "fo:");
                _allParagraphProperty.Add("break-after", "fo:");
                _allParagraphProperty.Add("hyphenation-ladder-count", "fo:"); //TD-345
                _allParagraphProperty.Add("keep-with-next", "fo:");
                _allParagraphProperty.Add("keep-together", "fo:");
                _allParagraphProperty.Add("float", "fo:"); //TD-416
                _allParagraphProperty.Add("page-number", "style:");

                _allTextProperty.Add("font-weight", "fo:");
                _allTextProperty.Add("font-size", "fo:");
                _allTextProperty.Add("font-family", "fo:");
                _allTextProperty.Add("font-style", "fo:");
                _allTextProperty.Add("font-variant", "fo:");
                _allTextProperty.Add("font", "fo:");
                _allTextProperty.Add("text-indent", "fo:");
                _allTextProperty.Add("text-transform", "fo:");
                _allTextProperty.Add("letter-spacing", "fo:");
                _allTextProperty.Add("word-spacing", "fo:");
                _allTextProperty.Add("color", "fo:");
                _allTextProperty.Add("text-line-through-style", "style:");
				_allTextProperty.Add("text-line-through-type", "style:");
                _allTextProperty.Add("text-decoration", "style:");
                _allTextProperty.Add("text-underline-style", "style:");
                _allTextProperty.Add("background-color", "fo:");
                _allTextProperty.Add("text-position", "style:");
                //TD-172
                _allTextProperty.Add("display", "text:");
                _allTextProperty.Add("country", "fo:");
                _allTextProperty.Add("language", "fo:");
                //TD-345
                _allTextProperty.Add("hyphenate", "fo:");
                _allTextProperty.Add("hyphenation-remain-char-count", "fo:");
                _allTextProperty.Add("hyphenation-push-char-count", "fo:");

                _allColumnProperty.Add("column-count", "fo:");
                _allColumnProperty.Add("column-gap", "fo:");
                _allColumnProperty.Add("column-fill", "fo:");
                _allColumnProperty.Add("column-rule", "style:");
                _allColumnProperty.Add("column-rule-width", "style:");
                _allColumnProperty.Add("column-rule-style", "style:");
                _allColumnProperty.Add("column-rule-color", "style:");
                _allColumnProperty.Add("dont-balance-text-columns", "text:");

                _allPageLayoutProperty.Add("page-width", "fo:");
                _allPageLayoutProperty.Add("page-height", "fo:");
                _allPageLayoutProperty.Add("num-format", "style:");
                _allPageLayoutProperty.Add("print-orientation", "style:");
                _allPageLayoutProperty.Add("margin-top", "fo:");
                _allPageLayoutProperty.Add("margin-right", "fo:");
                _allPageLayoutProperty.Add("margin-bottom", "fo:");
                _allPageLayoutProperty.Add("margin-left", "fo:");
                _allPageLayoutProperty.Add("writing-mode", "style:");
                _allPageLayoutProperty.Add("footnote-max-height", "style:");
                _allPageLayoutProperty.Add("background-color", "fo:");
                _allPageLayoutProperty.Add("border-bottom", "fo:");
                _allPageLayoutProperty.Add("border-top", "fo:");
                _allPageLayoutProperty.Add("border-top-style", "fo:");
                _allPageLayoutProperty.Add("border-top-width", "fo:");
                _allPageLayoutProperty.Add("border-top-color", "fo:");
                _allPageLayoutProperty.Add("border-left", "fo:");
                _allPageLayoutProperty.Add("border-right", "fo:");
                _allPageLayoutProperty.Add("padding-top", "fo:");
                _allPageLayoutProperty.Add("padding-bottom", "fo:");
                _allPageLayoutProperty.Add("padding-left", "fo:");
                _allPageLayoutProperty.Add("padding-right", "fo:");

                _pageLayoutProperty.Add("fo:page-width", "8.5in");
                _pageLayoutProperty.Add("fo:page-height", "11in");
                _pageLayoutProperty.Add("style:num-format", "1");
                _pageLayoutProperty.Add("style:print-orientation", "portrait");
                _pageLayoutProperty.Add("fo:margin-top", "0.7874in");
                _pageLayoutProperty.Add("fo:margin-right", "0.7874in");
                _pageLayoutProperty.Add("fo:margin-bottom", "0.7874in");
                _pageLayoutProperty.Add("fo:margin-left", "0.7874in");
                _pageLayoutProperty.Add("style:writing-mode", "lr-tb");
                _pageLayoutProperty.Add("style:footnote-max-height", "0in");

                // Add all tag property
                _baseTagName.Add("h1");
                _baseTagName.Add("h2");
                _baseTagName.Add("h3");
                _baseTagName.Add("h4");
                _baseTagName.Add("h5");
                _baseTagName.Add("h6");
                _baseTagName.Add("ol");
                _baseTagName.Add("ul");
                _baseTagName.Add("li");
                _baseTagName.Add("p");

                _baseTagName.Add("a");  // Anchor Tag

                _allTagName.AddRange(_baseTagName);

                Dictionary<string, string> tagProp = new Dictionary<string, string>();
                tagProp.Add("style:line-spacing", "12pt");
                tagProp.Add("fo:font-weight", "700");
                tagProp.Add("fo:font-size", "24pt");
                _tagProperty["h1"] = tagProp;

                tagProp = new Dictionary<string, string>();
                tagProp.Add("style:line-spacing", "8pt");
                tagProp.Add("fo:font-weight", "700");
                tagProp.Add("fo:font-size", "18pt");
                _tagProperty["h2"] = tagProp;

                tagProp = new Dictionary<string, string>();
                tagProp.Add("style:line-spacing", "7pt");
                tagProp.Add("fo:font-weight", "700");
                tagProp.Add("fo:font-size", "14pt");
                _tagProperty["h3"] = tagProp;

                tagProp = new Dictionary<string, string>();
                tagProp.Add("style:line-spacing", "6pt");
                tagProp.Add("fo:font-weight", "700");
                tagProp.Add("fo:font-size", "12pt");
                _tagProperty["h4"] = tagProp;

                tagProp = new Dictionary<string, string>();
                tagProp.Add("style:line-spacing", "5.5pt");
                tagProp.Add("fo:font-weight", "700");
                tagProp.Add("fo:font-size", "10pt");
                _tagProperty["h5"] = tagProp;

                tagProp = new Dictionary<string, string>();
                tagProp.Add("style:line-spacing", "5.5pt");
                tagProp.Add("fo:font-weight", "700");
                tagProp.Add("fo:font-size", "8pt");
                _tagProperty["h6"] = tagProp;

                tagProp = new Dictionary<string, string>();
                _tagProperty["ol"] = tagProp;
                _tagProperty["ul"] = tagProp;
                _tagProperty["li"] = tagProp;

                _tagProperty["p"] = tagProp;

                tagProp = new Dictionary<string, string>();
                _tagProperty["a"] = tagProp; // Anchor Tag

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }

        protected void LoadSpellCheck()
        {
            const string sKey = @"SOFTWARE\classes\.odt";
            RegistryKey key;
            try
            {
                key = Registry.LocalMachine.OpenSubKey(sKey);
            }
            catch (Exception)
            {
                key = null;
            }
            // Check to see if Open Office Installed
            if (key == null)
                return;
            object value = key.GetValue("");
            if (value == null)
                return;
            string documentType = value.ToString();

            string sKey2 = string.Format(@"SOFTWARE\Classes\{0}\shell\open\command", documentType);
            RegistryKey key2;
            try
            {
                key2 = Registry.LocalMachine.OpenSubKey(sKey2);
            }
            catch (Exception)
            {
                key2 = null;
            }
            if (key2 == null)
                return;

            string launchCommand = key2.GetValue("").ToString();
            Match m = Regex.Match(launchCommand, "\"(.*)program");

            string spellPath = Common.PathCombine("share", "autocorr");
            string openOfficePath = Common.PathCombine(m.Groups[1].Value, "basis");
            openOfficePath = Directory.Exists(openOfficePath)
                                 ? Common.PathCombine(openOfficePath, spellPath)
                                 : Common.PathCombine(m.Groups[1].Value, spellPath);
            if (!Directory.Exists(openOfficePath)) return;

            string[] spellFiles = Directory.GetFiles(openOfficePath, "acor_*.dat");
            foreach (string fileName in spellFiles)
            {
                string fName = Path.GetFileNameWithoutExtension(fileName);
                string[] lang_coun = fName.Substring(5).Split('-');
                if (lang_coun.Length == 2)
                {
                    string lang = lang_coun[0];
                    string coun = lang_coun[1];

                    if (_styleName.SpellCheck.ContainsKey(lang))
                    {
                        _styleName.SpellCheck[lang].Add(coun);
                    }
                    else
                    {
                        ArrayList arLang = new ArrayList();
                        arLang.Add(coun);
                        _styleName.SpellCheck[lang] = arLang;
                    }
                }
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate first block of Styles.xml
        /// </summary>
        /// <param name="targetFile">content.xml</param>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        protected void CreateODTStyles(string targetFile)
        {
            try
            {
                _writer = new XmlTextWriter(targetFile, null);
                _writer.Formatting = Formatting.Indented;
                _writer.WriteStartDocument();

                //office:document-content Attributes.
                _writer.WriteStartElement("office:document-styles");
                _writer.WriteAttributeString("xmlns:office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
                _writer.WriteAttributeString("xmlns:style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
                _writer.WriteAttributeString("xmlns:text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
                _writer.WriteAttributeString("xmlns:table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
                _writer.WriteAttributeString("xmlns:draw", "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");
                _writer.WriteAttributeString("xmlns:fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
                _writer.WriteAttributeString("xmlns:xlink", "http://www.w3.org/1999/xlink");
                _writer.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
                _writer.WriteAttributeString("xmlns:meta", "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
                _writer.WriteAttributeString("xmlns:number", "urn:oasis:names:tc:opendocument:xmlns:datastyle:1.0");
                _writer.WriteAttributeString("xmlns:svg", "urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0");
                _writer.WriteAttributeString("xmlns:chart", "urn:oasis:names:tc:opendocument:xmlns:chart:1.0");
                _writer.WriteAttributeString("xmlns:dr3d", "urn:oasis:names:tc:opendocument:xmlns:dr3d:1.0");
                _writer.WriteAttributeString("xmlns:math", "http://www.w3.org/1998/Math/MathML");
                _writer.WriteAttributeString("xmlns:form", "urn:oasis:names:tc:opendocument:xmlns:form:1.0");
                _writer.WriteAttributeString("xmlns:script", "urn:oasis:names:tc:opendocument:xmlns:script:1.0");
                _writer.WriteAttributeString("xmlns:ooo", "http://openoffice.org/2004/office");
                _writer.WriteAttributeString("xmlns:ooow", "http://openoffice.org/2004/writer");
                _writer.WriteAttributeString("xmlns:oooc", "http://openoffice.org/2004/calc");
                _writer.WriteAttributeString("xmlns:dom", "http://www.w3.org/2001/xml-events");
                _writer.WriteAttributeString("xmlns:rpt", "http://openoffice.org/2005/report");
                _writer.WriteAttributeString("xmlns:of", "urn:oasis:names:tc:opendocument:xmlns:of:1.2");
                _writer.WriteAttributeString("xmlns:xhtml", "http://www.w3.org/1999/xhtml");
                _writer.WriteAttributeString("xmlns:grddl", "http://www.w3.org/2003/g/data-view#");
                _writer.WriteAttributeString("office:version", "1.2");
                _writer.WriteAttributeString("grddl:transformation", "http://docs.oasis-open.org/office/1.2/xslt/odf2rdf.xsl");

                //office:font-face-decls Attributes.
                _writer.WriteStartElement("office:font-face-decls");
                _writer.WriteStartElement("style:font-face");
                _writer.WriteAttributeString("style:name", _projInfo.DefaultFontName);
                _writer.WriteAttributeString("svg:font-family", "'" + _projInfo.DefaultFontName + "'");
                _writer.WriteAttributeString("style:font-family-generic", "roman");
                _writer.WriteAttributeString("style:font-pitch", "variable");
                _writer.WriteEndElement();
                _writer.WriteStartElement("style:font-face");
                _writer.WriteAttributeString("style:name", "Yi plus Phonetics");
                _writer.WriteAttributeString("svg:font-family", "'Yi plus Phonetics'");
                _writer.WriteEndElement();


                ////TD-2682
                _writer.WriteStartElement("style:font-face");
                _writer.WriteAttributeString("style:name", "Charis SIL");
                _writer.WriteAttributeString("svg:font-family", "'Charis SIL'");
                _writer.WriteAttributeString("style:font-adornments", "Regular");
                _writer.WriteAttributeString("style:font-pitch", "variable");
                _writer.WriteEndElement();

                //TD-2815
                _writer.WriteStartElement("style:font-face");
                _writer.WriteAttributeString("style:name", _projInfo.HeaderFontName);
                _writer.WriteAttributeString("svg:font-family", "'" + _projInfo.HeaderFontName + "'");
                _writer.WriteAttributeString("style:font-pitch", "variable");
                _writer.WriteEndElement();


                //TD-2566
                _writer.WriteStartElement("style:font-face");
                _writer.WriteAttributeString("style:name", "Gautami1");
                _writer.WriteAttributeString("svg:font-family", "'Gautami'");
                _writer.WriteAttributeString("style:font-pitch", "variable");
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:font-face");
                _writer.WriteAttributeString("style:name", "Times New Roman");
                _writer.WriteAttributeString("svg:font-family", "'" + "Times New Roman" + "'");
                _writer.WriteAttributeString("style:font-family-generic", "roman");
                _writer.WriteAttributeString("style:font-pitch", "variable");
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:font-face");
                _writer.WriteAttributeString("style:name", "GenericFont");
                _writer.WriteAttributeString("svg:font-family", "'Arial Unicode MS'");
                _writer.WriteAttributeString("style:font-pitch", "variable");
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:font-face");
                _writer.WriteAttributeString("style:name", "Arial");
                _writer.WriteAttributeString("svg:font-family", "'Arial'");
                _writer.WriteAttributeString("style:font-family-generic", "swiss");
                _writer.WriteAttributeString("style:font-pitch", "variable");
                _writer.WriteEndElement();
                _writer.WriteStartElement("style:font-face");
                _writer.WriteAttributeString("style:name", "Lucida Sans Unicode");
                _writer.WriteAttributeString("svg:font-family", "'Lucida Sans Unicode'");
                _writer.WriteAttributeString("style:font-family-generic", "system");
                _writer.WriteAttributeString("style:font-pitch", "variable");
                _writer.WriteEndElement();
                _writer.WriteStartElement("style:font-face");
                _writer.WriteAttributeString("style:name", "MS Mincho");
                _writer.WriteAttributeString("svg:font-family", "'MS Mincho'");
                _writer.WriteAttributeString("style:font-family-generic", "system");
                _writer.WriteAttributeString("style:font-pitch", "variable");
                _writer.WriteEndElement();
                _writer.WriteStartElement("style:font-face");
                _writer.WriteAttributeString("style:name", "Tahoma");
                _writer.WriteAttributeString("svg:font-family", "'Tahoma'");
                _writer.WriteAttributeString("style:font-family-generic", "system");
                _writer.WriteAttributeString("style:font-pitch", "variable");
                _writer.WriteEndElement();
                _writer.WriteStartElement("style:font-face");
                _writer.WriteAttributeString("style:name", "Scheherazade Graphite Alpha");
                _writer.WriteAttributeString("svg:font-family", "'Scheherazade Graphite Alpha'");
                _writer.WriteEndElement();
                string fontName = string.Empty;
                string xmlFileNameWithPath = Common.CopyXmlFileToTempDirectory("GenericFont.xml");
                if (File.Exists(xmlFileNameWithPath))
                {
                    string xPath = "//font-language-unicode-map";
                    XmlNodeList fontList = Common.GetXmlNodes(xmlFileNameWithPath, xPath);
                    if (fontList != null && fontList.Count > 0)
                    {
                        foreach (XmlNode xmlNode in fontList)
                        {
                            if (xmlNode.Attributes != null)
                            {
                                fontName = xmlNode.InnerText.Trim();
                                _writer.WriteStartElement("style:font-face");
                                _writer.WriteAttributeString("style:name", fontName);
                                fontName = "'" + xmlNode.InnerText.Trim() + "'";
                                _writer.WriteAttributeString("svg:font-family", fontName);
                                _writer.WriteAttributeString("style:font-pitch", "variable");
                                _writer.WriteEndElement();
                            }
                        }
                    }
                }
                _writer.WriteEndElement();

                //office:styles Attributes.
                _writer.WriteStartElement("office:styles");

                // for Empty Class
                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "Empty");
                _writer.WriteAttributeString("style:family", "text");
                _writer.WriteAttributeString("style:parent-style-name", "none");
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "hide");
                _writer.WriteAttributeString("style:family", "paragraph");
                _writer.WriteAttributeString("style:parent-style-name", "Heading_20_9");
                _writer.WriteAttributeString("style:class", "text");
                _writer.WriteStartElement("style:text-properties");
                _writer.WriteAttributeString("text:display", "true");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "GuideL");
                _writer.WriteAttributeString("style:family", "paragraph");
                _writer.WriteAttributeString("style:parent-style-name", "Heading_20_9");
                _writer.WriteAttributeString("style:default-outline-level", "9");
                _writer.WriteStartElement("style:text-properties");
                _writer.WriteAttributeString("text:display", "true");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "GuideR");
                _writer.WriteAttributeString("style:family", "paragraph");
                _writer.WriteAttributeString("style:parent-style-name", "Heading_20_10");
                _writer.WriteAttributeString("style:default-outline-level", "10");
                _writer.WriteStartElement("style:text-properties");
                _writer.WriteAttributeString("text:display", "true");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "Header");
                _writer.WriteAttributeString("style:family", "paragraph");
                _writer.WriteAttributeString("style:parent-style-name", "Standard");//Standard
                _writer.WriteAttributeString("style:class", "extra");

                _writer.WriteEndElement();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Add Tag in styles.xml, if missing in CSS
        /// 
        /// </summary>
        /// <returns> null </returns>
        /// -------------------------------------------------------------------------------------------
        protected void AddTagStyle()
        {
            foreach (string tagName in _baseTagName)
            {
                switch (tagName)
                {
                    case "h1":
                        _writer.WriteStartElement("style:style");
                        _writer.WriteAttributeString("style:name", "h1");
                        _writer.WriteAttributeString("style:family", "paragraph");
                        _writer.WriteAttributeString("style:parent-style-name", "none");
                        _writer.WriteStartElement("style:paragraph-properties");
                        _writer.WriteAttributeString("fo:margin-top", "0.1in");
                        _writer.WriteAttributeString("fo:margin-bottom", "0.1in");
                        _writer.WriteAttributeString("style:line-spacing", "100%");
                        _writer.WriteEndElement();
                        _writer.WriteStartElement("style:text-properties");
                        _writer.WriteAttributeString("fo:font-weight", "700");
                        _writer.WriteAttributeString("fo:font-size", "24pt");
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        break;
                    case "h2":
                        _writer.WriteStartElement("style:style");
                        _writer.WriteAttributeString("style:name", "h2");
                        _writer.WriteAttributeString("style:family", "paragraph");
                        _writer.WriteAttributeString("style:parent-style-name", "none");
                        _writer.WriteStartElement("style:paragraph-properties");
                        _writer.WriteAttributeString("fo:margin-top", "0.1in");
                        _writer.WriteAttributeString("fo:margin-bottom", "0.1in");
                        _writer.WriteAttributeString("style:line-spacing", "100%");
                        _writer.WriteEndElement();
                        _writer.WriteStartElement("style:text-properties");
                        _writer.WriteAttributeString("fo:font-weight", "700");
                        _writer.WriteAttributeString("fo:font-size", "18pt");
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        break;
                    case "h3":
                        _writer.WriteStartElement("style:style");
                        _writer.WriteAttributeString("style:name", "h3");
                        _writer.WriteAttributeString("style:family", "paragraph");
                        _writer.WriteAttributeString("style:parent-style-name", "none");
                        _writer.WriteStartElement("style:paragraph-properties");
                        _writer.WriteAttributeString("fo:margin-top", "0.1in");
                        _writer.WriteAttributeString("fo:margin-bottom", "0.1in");
                        _writer.WriteAttributeString("style:line-spacing", "100%");
                        _writer.WriteEndElement();
                        _writer.WriteStartElement("style:text-properties");
                        _writer.WriteAttributeString("fo:font-weight", "700");
                        _writer.WriteAttributeString("fo:font-size", "14pt");
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        break;
                    case "h4":
                        _writer.WriteStartElement("style:style");
                        _writer.WriteAttributeString("style:name", "h4");
                        _writer.WriteAttributeString("style:family", "paragraph");
                        _writer.WriteAttributeString("style:parent-style-name", "none");
                        _writer.WriteStartElement("style:paragraph-properties");
                        _writer.WriteAttributeString("fo:margin-top", "0.1in");
                        _writer.WriteAttributeString("fo:margin-bottom", "0.1in");
                        _writer.WriteAttributeString("style:line-spacing", "100%");
                        _writer.WriteEndElement();
                        _writer.WriteStartElement("style:text-properties");
                        _writer.WriteAttributeString("fo:font-weight", "700");
                        _writer.WriteAttributeString("fo:font-size", "12pt");
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        break;
                    case "h5":
                        _writer.WriteStartElement("style:style");
                        _writer.WriteAttributeString("style:name", "h5");
                        _writer.WriteAttributeString("style:family", "paragraph");
                        _writer.WriteAttributeString("style:parent-style-name", "none");
                        _writer.WriteStartElement("style:paragraph-properties");
                        _writer.WriteAttributeString("fo:margin-top", "0.1in");
                        _writer.WriteAttributeString("fo:margin-bottom", "0.1in");
                        _writer.WriteAttributeString("style:line-spacing", "100%");
                        _writer.WriteEndElement();
                        _writer.WriteStartElement("style:text-properties");
                        _writer.WriteAttributeString("fo:font-weight", "700");
                        _writer.WriteAttributeString("fo:font-size", "10pt");
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        break;
                    case "h6":
                        _writer.WriteStartElement("style:style");
                        _writer.WriteAttributeString("style:name", "h6");
                        _writer.WriteAttributeString("style:family", "paragraph");
                        _writer.WriteAttributeString("style:parent-style-name", "none");
                        _writer.WriteStartElement("style:paragraph-properties");
                        _writer.WriteAttributeString("fo:margin-top", "0.1in");
                        _writer.WriteAttributeString("fo:margin-bottom", "0.1in");
                        _writer.WriteAttributeString("style:line-spacing", "100%");
                        _writer.WriteEndElement();
                        _writer.WriteStartElement("style:text-properties");
                        _writer.WriteAttributeString("fo:font-weight", "700");
                        _writer.WriteAttributeString("fo:font-size", "8pt");
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        break;
                    case "ol":
                        _writer.WriteStartElement("text:list-style");
                        _writer.WriteAttributeString("style:name", "ol");
                        _writer.WriteStartElement("text:list-level-style-number");
                        _writer.WriteAttributeString("text:level", "1");
                        _writer.WriteAttributeString("text:style-name", "Numbering_20_Symbols");
                        _writer.WriteAttributeString("style:num-suffix", ".");
                        _writer.WriteAttributeString("style:num-format", "1");
                        _writer.WriteStartElement("style:list-level-properties");
                        _writer.WriteAttributeString("text:list-level-position-and-space-mode", "label-alignment");
                        _writer.WriteStartElement("style:list-level-label-alignment");
                        _writer.WriteAttributeString("text:label-followed-by", "listtab");
                        _writer.WriteAttributeString("text:list-tab-stop-position", "0.5in");
                        _writer.WriteAttributeString("fo:text-indent", "-0.25in");
                        _writer.WriteAttributeString("fo:margin-left", "0.5in");
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        break;
                    case "ul":
                        _writer.WriteStartElement("text:list-style");
                        _writer.WriteAttributeString("style:name", "ul");
                        _writer.WriteStartElement("text:list-level-style-bullet");
                        _writer.WriteAttributeString("text:level", "1");
                        _writer.WriteAttributeString("text:style-name", "Bullet_20_Symbols");
                        _writer.WriteAttributeString("style:num-suffix", ".");
                        _writer.WriteAttributeString("text:bullet-char", Common.ConvertUnicodeToString("\\2022"));
                        _writer.WriteStartElement("style:list-level-properties");
                        _writer.WriteAttributeString("text:list-level-position-and-space-mode", "label-alignment");
                        _writer.WriteStartElement("style:list-level-label-alignment");
                        _writer.WriteAttributeString("text:label-followed-by", "listtab");
                        _writer.WriteAttributeString("text:list-tab-stop-position", "0.5in");
                        _writer.WriteAttributeString("fo:text-indent", "-0.25in");
                        _writer.WriteAttributeString("fo:margin-left", "0.5in");
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();

                        _writer.WriteStartElement("text:list-style");
                        _writer.WriteAttributeString("style:name", "ul");
                        _writer.WriteStartElement("text:list-level-style-bullet");
                        _writer.WriteAttributeString("text:level", "2");
                        _writer.WriteAttributeString("text:style-name", "Bullet_20_Symbols");
                        _writer.WriteAttributeString("style:num-suffix", ".");
                        _writer.WriteAttributeString("text:bullet-char", Common.ConvertUnicodeToString("\\2022"));
                        _writer.WriteStartElement("style:list-level-properties");
                        _writer.WriteAttributeString("text:list-level-position-and-space-mode", "label-alignment");
                        _writer.WriteStartElement("style:list-level-label-alignment");
                        _writer.WriteAttributeString("text:label-followed-by", "listtab");
                        _writer.WriteAttributeString("text:list-tab-stop-position", "0.5in");
                        _writer.WriteAttributeString("fo:text-indent", "-0.25in");
                        _writer.WriteAttributeString("fo:margin-left", "1.905cm");
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        break;
                    case "li":
                        _writer.WriteStartElement("style:style");
                        _writer.WriteAttributeString("style:name", tagName);
                        _writer.WriteAttributeString("style:family", "paragraph");
                        _writer.WriteAttributeString("style:parent-style-name", "none");
                        _writer.WriteAttributeString("style:list-style-name", "ol");
                        _writer.WriteStartElement("style:paragraph-properties");
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        break;
                    case "p":
                        _writer.WriteStartElement("style:style");
                        _writer.WriteAttributeString("style:name", "p");
                        _writer.WriteAttributeString("style:family", "paragraph");
                        _writer.WriteAttributeString("style:parent-style-name", "none");
                        _writer.WriteStartElement("style:paragraph-properties");
                        _writer.WriteAttributeString("fo:margin-top", "0.1in");
                        _writer.WriteAttributeString("fo:margin-bottom", "0.1in");
                        _writer.WriteAttributeString("style:line-spacing", "100%");
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        break;
                    case "a":  // Anchor Tag
                        _writer.WriteStartElement("style:style");
                        _writer.WriteAttributeString("style:name", "a");
                        _writer.WriteAttributeString("style:family", "text");
                        _writer.WriteAttributeString("style:parent-style-name", "none");
                        _writer.WriteStartElement("style:text-properties");
                        _writer.WriteAttributeString("style:text-underline-style", "solid"); // underline
                        _writer.WriteAttributeString("fo:color", "#0000ff"); // color blue
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                        break;
                }
            }
            _baseTagName.Clear();
        }



    }
}