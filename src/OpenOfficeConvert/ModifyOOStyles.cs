// --------------------------------------------------------------------------------------------
// <copyright file="ModifyLOStyles.cs" from='2009' to='2014' company='SIL International'>
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
// Modify the OOStyle file
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Win32;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    class ModifyLOStyles : LOStyles
    {
        private XmlDocument _styleXMLdoc;
        private XmlElement _root;
        private XmlNamespaceManager nsmgr;
        private string _projectPath;
        private string _xPath;
        private XmlElement _nameElement;
        private string _tagName;
        private bool _isHeadword;
        private ArrayList _textVariables = new ArrayList();
        Dictionary<string, string> _languageStyleName = new Dictionary<string, string>();
        Dictionary<string, Dictionary<string, string>> _childStyle = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, string> _parentClass;
        private Dictionary<string, ArrayList> _spellCheck = new Dictionary<string, ArrayList>();
        private List<string> _languageFont = new List<string>();
        private Dictionary<string, string> fontLangMap = new Dictionary<string, string>();


        public void ModifyStylesXML(string projectPath, Dictionary<string, Dictionary<string, string>> childStyle, List<string> usedStyleName, Dictionary<string, string> languageStyleName, string baseStyle, bool isHeadword, Dictionary<string, string> parentClass, string odmMTFont)
        {
            LoadAllProperty();
            LoadSpellCheck();
            CreateFontLanguageMap();
            
            _childStyle = childStyle;
            _projectPath = projectPath;
            _isHeadword = isHeadword;
            _languageStyleName = languageStyleName;
            _parentClass = parentClass;
            string styleFilePath = OpenIDStyles(); //todo change name
            SetHeaderFontName(styleFilePath, odmMTFont);
            nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
            nsmgr.AddNamespace("style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
            nsmgr.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");

            _root = _styleXMLdoc.DocumentElement;
            if (_root == null)
            {
                return;
            }

            const string paraStyle = "Empty";
            const string charStyle = "Empty";

            CreateStyle(paraStyle, charStyle, usedStyleName);
            _styleXMLdoc.Save(styleFilePath);

            AddFontDeclarative(styleFilePath, _languageFont);
        }

        public void SetHeaderFontName(string styleFilePath, string odmMTFont)
        {
            if (styleFilePath.ToLower().IndexOf("dictionary") > 0)
            {
                _styleXMLdoc = new XmlDocument();
                _styleXMLdoc.Load(styleFilePath);
                var nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
                nsmgr.AddNamespace("office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
                nsmgr.AddNamespace("style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
                string xPath = "//style:style[@style:name='MT1']/style:text-properties";
                XmlNode hdrtextPropNode = _styleXMLdoc.SelectSingleNode(xPath, nsmgr);
                if (hdrtextPropNode != null)
                {
                    if (_childStyle.Count == 0)
                    {
                        hdrtextPropNode.Attributes["style:font-name-complex"].Value = odmMTFont;
                        return;
                    }
                    if (_childStyle.ContainsKey("headword_entry_letData_dicBody") )
                    {
                        if (_childStyle["headword_entry_letData_dicBody"].ContainsKey("font-family"))
                        {
                            hdrtextPropNode.Attributes["style:font-name-complex"].Value =
                                _childStyle["headword_entry_letData_dicBody"]["font-family"];
                        }
                    }
                    else if(_childStyle.ContainsKey("headword_entry_letData_body")) 
                    {
                        if (_childStyle["headword_entry_letData_body"].ContainsKey("font-family"))
                        {
                            hdrtextPropNode.Attributes["style:font-name-complex"].Value =
                                _childStyle["headword_entry_letData_body"]["font-family"];
                        }
                    }
                }
            }
            else //Scripture
            {
                _styleXMLdoc = new XmlDocument();
                _styleXMLdoc.Load(styleFilePath);
                var nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
                nsmgr.AddNamespace("office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
                nsmgr.AddNamespace("style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
                string xPath = "//style:style[@style:name='MT1']/style:text-properties";
                XmlNode hdrtextPropNode = _styleXMLdoc.SelectSingleNode(xPath, nsmgr);
                if (hdrtextPropNode != null)
                {
                    if (_childStyle.ContainsKey("scrBookName_scrBook_scrBody"))
                    {
                        if (_childStyle["scrBookName_scrBook_scrBody"].ContainsKey("font-family"))
                            hdrtextPropNode.Attributes["style:font-name-complex"].Value =
                                _childStyle["scrBookName_scrBook_scrBody"]["font-family"];
                    }
                }
            }
        }

        public void AddFontDeclarative(string styleFilePath, List<string> font)
        {
            if (font.Count == 0) return;

            _styleXMLdoc = new XmlDocument();
            _styleXMLdoc.Load(styleFilePath);
            var nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
            nsmgr.AddNamespace("office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
            // if new stylename exists
            XmlElement root = _styleXMLdoc.DocumentElement;
            string style = "//office:font-face-decls";
            if (root != null)
            {
                XmlNode node = root.SelectSingleNode(style, nsmgr); // work
                if (node == null)
                {
                    return;
                }
                foreach (string s in font)
                {
                    XmlNode styleNode = node.FirstChild.CloneNode(true);
                    node.AppendChild(styleNode);
                    XmlAttributeCollection attrColl = styleNode.Attributes;
                    attrColl["style:name"].Value = s;
                    attrColl["svg:font-family"].Value = s;
                }
            }
            _styleXMLdoc.Save(styleFilePath);
        }

        private void CreateFontLanguageMap()
        {
            if (Common.Testing) return;
            string f7Path = Common.PathCombine(Common.GetFiledWorksPathVersion(), "Projects\\" + Common.databaseName + "\\WritingSystemStore");
            fontLangMap = Common.FillMappedFonts(f7Path, fontLangMap);

            if (fontLangMap.Count == 0)
            {
                string wsPath = Common.PathCombine(Common.GetAllUserAppPath(), "SIL/WritingSystemStore");
                fontLangMap = Common.FillMappedFonts(wsPath, fontLangMap);
            }
            if (fontLangMap.Count == 0)
            {
                fontLangMap = Common.FillMappedFonts(fontLangMap);
            }
        }



        private void CreateStyle(string paraStyle, string charStyle, List<string> usedStyleName)
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> className in _childStyle)
            {
                if (!usedStyleName.Contains(className.Key))
                    continue;
                _xPath = "//style:style[@style:name=\"" + paraStyle + "\"]";
                InsertNode(className);
            }
        }


        private void InsertNode(KeyValuePair<string, Dictionary<string, string>> className)
        {
            string newClassName = className.Key;

			if (_parentClass.Count == 0 || String.IsNullOrEmpty(_parentClass[newClassName]))
				return;

            string[] parent_Type = _parentClass[newClassName].Split('|');

            string familyType = parent_Type[1] == "div" ? "paragraph" : "text";

            XmlNode node = _root.SelectSingleNode(_xPath, nsmgr);
            if (node == null) return;
            XmlDocumentFragment styleNode = _styleXMLdoc.CreateDocumentFragment();
            styleNode.InnerXml = node.OuterXml;
            node.ParentNode.InsertAfter(styleNode, node);

            _nameElement = (XmlElement)node;

            string attribute = "style:name";
            SetAttribute(newClassName, attribute);

            attribute = "style:family";
            SetAttribute(familyType, attribute);

            attribute = "style:parent-style-name";
            SetAttribute(parent_Type[0], attribute);

            if (className.Value.ContainsKey("-ps-outline-level"))
            {
                attribute = "style:default-outline-level";
                SetAttribute(className.Value["-ps-outline-level"], attribute);
                SetAttribute("", "style:list-style-name");
                SetAttribute("", "style:master-page-name");
            }

            attribute = "master-page-name";
            if (newClassName.ToLower().IndexOf("coverimage") == 0)
            {
                SetAttribute("Cover_20_Page", attribute);
            }
            if (newClassName.IndexOf("title") == 0)
            {
                SetAttribute("Title_20_Page", attribute);
                className.Value.Add("page-number", "1");
            }
            else if (newClassName.IndexOf("copyright") == 0)
            {
                SetAttribute("CopyRight_20_Page", attribute);
            }
            else if (newClassName.IndexOf("tableofcontents") == 0)
            {
                SetAttribute("TOC_20_Page", attribute);
            }
            else if (newClassName.IndexOf("dummypage") == 0)
            {
                SetAttribute("Dummy_20_Page", attribute);
            }

            SetTagProperty(className.Key);
            AddParaTextNode(className, node, familyType);
        }

        private void SetAttribute(string newClassName, string attibute)
        {
            string ns = "style";
            SetAttributeNS(attibute, ns, newClassName);
        }

        private void SetAttributeNS(string attibute, string ns, string newClassName)
        {
            var nsmgr1 = new XmlNamespaceManager(_styleXMLdoc.NameTable);
            nsmgr1.AddNamespace("style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr1.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
            nsmgr1.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");

            XmlAttribute xmlAttrib = _styleXMLdoc.CreateAttribute(attibute, nsmgr1.LookupNamespace(ns));

            if (_nameElement.Attributes.GetNamedItem(attibute) != null)
            {
                XmlAttribute attibBackgroundColor = _nameElement.Attributes[attibute];
                _nameElement.Attributes.Remove(attibBackgroundColor);
            }
            xmlAttrib.Value = newClassName;
            _nameElement.Attributes.Append(xmlAttrib);
        }


        /// <summary>
        /// Moves Background color to text property from para property if it is span
        /// </summary>
        /// <param name="para"></param>
        /// <param name="text"></param>
        /// <param name="familyType"></param>
        private void BackgroundColorSpan(Dictionary<string, string> para, Dictionary<string, string> text,string familyType)
        {
            const string backColorKey = "fo:background-color";
            if (para.ContainsKey(backColorKey) && familyType == "text")
            {
                text[backColorKey] = para[backColorKey];
                para.Remove(backColorKey);
            }
        }

        private void AddParaTextNode(KeyValuePair<string, Dictionary<string, string>> className, XmlNode node, string familyType)
        {
            _paragraphProperty.Clear();
            _textProperty.Clear();
            _columnProperty.Clear();

            foreach (KeyValuePair<string, string> prop in className.Value)
            {
                string propName = prop.Key;
                if (_allParagraphProperty.ContainsKey(propName))
                {
                    if (!_paragraphProperty.ContainsKey(prop.Key))
                        _paragraphProperty[_allParagraphProperty[propName]  + prop.Key] = prop.Value;
                }
                else if (_allTextProperty.ContainsKey(propName))
                {
                    if (!_textProperty.ContainsKey(prop.Key))
                    {
                        string keyName = _allTextProperty[propName] + prop.Key;
                        _textProperty[keyName] = prop.Value.Trim();

                        // adding complex attribute
                        if (keyName == "fo:font-weight" || keyName == "fo:font-size" || keyName == "fo:font-family")
                        {
                            string propertyName = keyName;
                            propertyName = propertyName == "fo:font-family"
                                               ? "style:font-name"
                                               : propertyName.Replace("fo:", "style:");
                            _textProperty[propertyName + "-complex"] = prop.Value.Trim();
                        }
                    }

                }
                else if (_allColumnProperty.ContainsKey(propName)) // fullString property
                {
                    _columnProperty[_allColumnProperty[propName] + prop.Key] = prop.Value;
                }

            }

            BackgroundColorSpan(_paragraphProperty, _textProperty, familyType);
            DropCaps(_paragraphProperty, _textProperty);
            string _lang = string.Empty;
            if (_languageStyleName.ContainsKey(className.Key))
            {
                string language, country;
                Common.GetCountryCode(out language, out country, _languageStyleName[className.Key], _spellCheck);

                string lg = Common.RightString(className.Key,"_");
                if (lg.StartsWith("."))
                {
                    string lang = Common.LeftString(lg, "_");
                    _lang = lang.Replace(".", "");
                }
                if ( ! (_lang == string.Empty || _lang == "en"))
                    if (fontLangMap.ContainsKey(_lang))
                    {
                        string fname = fontLangMap[_lang];
                        if (!_textProperty.ContainsKey("fo:font-family") || (_textProperty.ContainsKey("fo:font-family") && _textProperty["fo:font-family"] != fname))
                        {
                            _textProperty["fo:font-family"] = fname;
                            _textProperty["style:font-name-complex"] = fname;
                            if (!_languageFont.Contains(fname))
                                _languageFont.Add(fname);
                        }
                    }
                _textProperty["fo:language"] = language;
                _textProperty["fo:country"] = country;
            }

            XmlNode paraNode = null;
            if(node.Attributes != null && (node.Attributes.Count > 0 && node.Attributes["style:family"].Value.ToLower() == "text"))
            {
                _paragraphProperty.Clear();
            }

            if (_paragraphProperty.Count > 0)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++) 
                {
                    if (node.ChildNodes[i].Name == "style:paragraph-properties")
                    {
                        paraNode = node.ChildNodes[i];
                    }
                }
                if (paraNode == null)
                {
                    paraNode = node.AppendChild(_styleXMLdoc.CreateElement("style:paragraph-properties", nsmgr.LookupNamespace("style")));
                    node.AppendChild(paraNode);
                }

                _nameElement = (XmlElement)paraNode;
                foreach (KeyValuePair<string, string> para in _paragraphProperty)
                {
                    string[] property = para.Key.Split(':');
                    string ns = property[0];
                    string prop = property[1];
                    SetAttributeNS(prop,ns,para.Value);
                }

                InsertOrphansForEntry(className.Key);
            }

            XmlNode textNode = null;
            if (_textProperty.Count > 0)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++) // find  Text node
                {
                    if (node.ChildNodes[i].Name == "style:text-properties")
                    {
                        textNode = node.ChildNodes[i];
                    }
                }
                if (textNode == null)
                {
                    textNode = node.AppendChild(_styleXMLdoc.CreateElement("style:text-properties", nsmgr.LookupNamespace("style")));
                    node.AppendChild(textNode);
                }

                _nameElement = (XmlElement)textNode;
                foreach (KeyValuePair<string, string> para in _textProperty)
                {
                    string[] property = para.Key.Split(':');
                    string ns = property[0];
                    string prop = property[1];
                    SetAttributeNS(prop, ns, para.Value);
                }
            }
            if (_columnProperty.Count > 0) // create a value XML file for content.xml with column property.
            {
                CreateColumnXMLFile(className.Key);
            }
        }

        
        /// <summary>
        /// For Header/Footer variables, all entry shouldn't be partial on page end.
        /// TD-2403
        /// </summary>
        /// <param name="className"></param>
        private void InsertOrphansForEntry(string className)
        {
            if (className.IndexOf("entry") == 0)
            {
                SetAttributeNS("orphans", "fo", "2");
            }
        }


        private void DropCaps(Dictionary<string, string> paragraphProperty, Dictionary<string, string> textProperty)
        {
            if (paragraphProperty.ContainsKey("fo:float") && paragraphProperty.ContainsKey("style:vertical-align"))
            {
                paragraphProperty.Clear();  // Remove all paragraph property
                if (textProperty.ContainsKey("fo:font-size"))
                {
                    textProperty.Remove("fo:font-size");
                    if (textProperty.ContainsKey("style:font-size-complex"))
                     textProperty.Remove("style:font-size-complex");
                }
            }
        }

        private void CreateColumnXMLFile(string className)
        {
            string path = Common.PathCombine(Path.GetTempPath(), "_" + className.Trim() + ".xml");

            XmlTextWriter writerCol = new XmlTextWriter(path, null);
            writerCol.Formatting = Formatting.Indented;
            writerCol.WriteStartDocument();

            try
            {
                writerCol.WriteStartElement("office:document-content");
                writerCol.WriteAttributeString("xmlns:office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
                writerCol.WriteAttributeString("xmlns:style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
                writerCol.WriteAttributeString("xmlns:fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
                writerCol.WriteAttributeString("xmlns:text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
                writerCol.WriteAttributeString("style:parent-style-name", "none");

                writerCol.WriteStartElement("style:style");
                writerCol.WriteAttributeString("style:name", "Sect_" + className.Trim());
                writerCol.WriteAttributeString("style:family", "section");

                writerCol.WriteStartElement("style:section-properties");
                if (_columnProperty.ContainsKey("text:dont-balance-text-columns"))
                {
                    writerCol.WriteAttributeString("text:dont-balance-text-columns", _columnProperty["text:dont-balance-text-columns"]);
                    _columnProperty.Remove("text:dont-balance-text-columns");
                }
                else
                {
                    writerCol.WriteAttributeString("text:dont-balance-text-columns", "false");
                }

                if (_sectionProperty != null && _sectionProperty.ContainsKey("style:writing-mode"))
                {
                    writerCol.WriteAttributeString("style:writing-mode", _sectionProperty["style:writing-mode"]);
                }

                writerCol.WriteAttributeString("style:editable", "false");

                writerCol.WriteStartElement("style:columns");


                string columnGap = "0pt";
                byte columnCount = 0;
                foreach (KeyValuePair<string, string> text in _columnProperty)
                {
                    if (text.Key == "fo:column-gap")
                    {
                        columnGap = text.Value;
                    }
                    else if (text.Key == "fo:column-count")
                    {
                        columnCount = (byte)Common.ConvertToInch(text.Value);
                    }
                }



                writerCol.WriteAttributeString("fo:column-count", columnCount.ToString());
                float pageWidth = 0;
                float relWidth = 0;
                float spacing = 0;
                float colWidth = 0;
                if (columnCount > 1)
                {
                    pageWidth = Common.ConvertToInch(_pageLayoutProperty["fo:page-width"]);
                    spacing = Common.ConvertToInch(columnGap) / 2;
                    relWidth = (pageWidth - (spacing * (columnCount * 2))) / columnCount;

                    if (columnGap.IndexOf("em") > 0 || columnGap.IndexOf("%") > 0) // Column Gap will be calculte in content.xml
                    {
                        colWidth = (pageWidth - Common.ConvertToInch(_pageLayoutProperty["fo:margin-left"])
                                        - Common.ConvertToInch(_pageLayoutProperty["fo:margin-left"])) / 2.0F;
                    }
                    else
                    {
                        colWidth = (pageWidth - Common.ConvertToInch(columnGap) - Common.ConvertToInch(_pageLayoutProperty["fo:margin-left"])
                                     - Common.ConvertToInch(_pageLayoutProperty["fo:margin-left"])) / 2.0F;
                    }
                    _styleName.ColumnWidth = colWidth; // for picture size calculation

                    string relWidthStr = relWidth.ToString() + "*";
                    for (int i = 1; i <= columnCount; i++)
                    {
                        writerCol.WriteStartElement("style:column");
                        if (i == 1)
                        {
                            writerCol.WriteAttributeString("style:rel-width", relWidthStr);
                            writerCol.WriteAttributeString("fo:start-indent", 0 + "in");
                            writerCol.WriteAttributeString("fo:end-indent", spacing + "in");
                        }
                        else if (i == columnCount)
                        {
                            writerCol.WriteAttributeString("style:rel-width", relWidthStr);
                            writerCol.WriteAttributeString("fo:start-indent", spacing + "in");
                            writerCol.WriteAttributeString("fo:end-indent", 0 + "in");
                        }
                        else
                        {
                            writerCol.WriteAttributeString("style:rel-width", relWidthStr);
                            writerCol.WriteAttributeString("fo:start-indent", spacing + "in");
                            writerCol.WriteAttributeString("fo:end-indent", spacing + "in");
                        }
                        writerCol.WriteEndElement();
                    }

                    if (columnGap.IndexOf("em") > 0 || columnGap.IndexOf("%") > 0 || columnGap.IndexOf("-") > -1)
                    {
                        Dictionary<string, string> pageProperties = new Dictionary<string, string>();
                        pageProperties["pageWidth"] = pageWidth.ToString();
                        pageProperties["columnCount"] = columnCount.ToString();
                        if (columnGap.IndexOf("em") > 0)
                            pageProperties["columnGap"] = columnGap.ToString();
                        else if (columnGap.IndexOf("%") > 0)
                        {
                            try
                            {
                                int convertToEm = int.Parse(columnGap.Replace("%", "")) / 100;
                                pageProperties["columnGap"] = "." + convertToEm.ToString() + columnGap.Replace("%", "em");

                            }
                            catch (Exception)
                            {
                                pageProperties["columnGap"] = "1em";

                            }
                        }
                        else if (columnGap.IndexOf("-") > -1)
                        {
                            pageProperties["columnGap"] = columnGap.Replace("-", "");
                        }
                        _styleName.ColumnGapEm["Sect_" + className.Trim()] = pageProperties;
                    }
                }

                if (_columnSep != null && _columnSep.Count > 0)
                {
                    writerCol.WriteStartElement("style:column-sep");
                    foreach (KeyValuePair<string, string> text in _columnSep)
                    {
                        writerCol.WriteAttributeString(text.Key, text.Value);
                    }
                    writerCol.WriteEndElement();
                }

                writerCol.WriteEndElement();
                writerCol.WriteEndElement();
                writerCol.WriteEndElement();

                writerCol.WriteEndElement();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                writerCol.WriteEndDocument();
                writerCol.Flush();
                writerCol.Close();
            }
        }

        private void LoadSpellCheck()
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
                string lang = "zxx";
                string coun = "none";
                string fName = Path.GetFileNameWithoutExtension(fileName);
                string[] lang_coun = fName.Substring(5).Split('-');
                if (lang_coun.Length == 2)
                {
                    lang = lang_coun[0];
                    coun = lang_coun[1];
                }
                else if (lang_coun.Length == 1)
                {
                    lang = lang_coun[0];
                    switch (lang)
                    { 
                        case "es":
                            coun = "ES";
                            break;
                        case "fr":
                            coun = "FR";
                            break;
                        case "it":
                            coun = "IT";
                            break;
                        case "de":
                            coun = "DE";
                            break;
                    }
                }

                if (_spellCheck.ContainsKey(lang))
                {
                    _spellCheck[lang].Add(coun);
                }
                else
                {
                    ArrayList arLang = new ArrayList();
                    arLang.Add(coun);
                    _spellCheck[lang] = arLang;
                }
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Create a Graphics style in style.xml, if style has parent.
        /// New style inherits its parent.
        /// float = floatProperty =  left/ right/ center
        /// display = displayproperty = none/ block
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="styleFilePath">syles.xml path</param>
        /// <param name="makeClassName">combination of child and parent "style name"</param>
        /// <param name="parentName">Parent "style name"</param>
        /// <param name="displayProperty">Left/Right</param>
        /// <returns>None</returns>
        /// -------------------------------------------------------------------------------------------
        public void CreateGraphicsStyle(string styleFilePath, string makeClassName, string parentName, string displayProperty)
        {
            const string className = "Graphics";
            _styleXMLdoc = Common.DeclareXMLDocument(true);
            _styleXMLdoc.Load(styleFilePath);

            var nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

            // if new stylename exists
            XmlElement root = _styleXMLdoc.DocumentElement;
            string style = "//st:style[@st:name='" + makeClassName + "']";
            if (root != null)
            {
                XmlNode node = root.SelectSingleNode(style, nsmgr); // work
                if (node != null)
                {
                    return;
                }
                style = "//st:style[@st:name='" + className + "']";
                node = root.SelectSingleNode(style, nsmgr); // work
				if (node == null)
				{
					return;
				}

                XmlDocumentFragment styleNode = _styleXMLdoc.CreateDocumentFragment();
				if (node != null && node.OuterXml != null)
	            {
		            styleNode.InnerXml = node.OuterXml;
		            node.ParentNode.InsertAfter(styleNode, node);
	            }

	            //        <style:style style:name="fr4" style:family="graphic" style:parent-style-name="Graphics">
                //    <style:graphic-properties fo:margin-left="0in" fo:margin-right="0in" fo:margin-top="0in"
                //        fo:margin-bottom="0in" style:run-through="foreground" style:wrap="none"
                //        style:vertical-pos="top" style:vertical-rel="paragraph-content"
                //        style:horizontal-pos="center" style:horizontal-rel="paragraph-content"
                //        style:mirror="none" fo:clip="rect(0in, 0in, 0in, 0in)" draw:luminance="0%"
                //        draw:contrast="0%" draw:red="0%" draw:green="0%" draw:blue="0%" draw:gamma="100%"
                //        draw:color-inversion="false" draw:image-opacity="100%" draw:color-mode="standard"/>
                //</style:style>

                _nameElement = (XmlElement)node;

                SetAttribute(makeClassName, "style:name");
                string wrap = "dynamic";
                if (displayProperty == "block")
                {
                    wrap = "none";
                    displayProperty = "center";
                }
                else if (displayProperty == "frame")
                {
                    wrap = "dynamic";
					displayProperty = "center";
                }

                _nameElement = (XmlElement)node.ChildNodes[1];
                SetAttribute("foreground", "style:run-through");
                SetAttribute(wrap, "style:wrap");
                SetAttribute("1", "style:number-wrapped-paragraphs");
                SetAttribute("false", "style:wrap-contour");
                SetAttribute("from-top", "style:vertical-pos");
                SetAttribute("paragraph-content", "style:vertical-rel");
                SetAttribute(displayProperty, "style:horizontal-pos");
                SetAttribute("paragraph", "style:horizontal-rel");
                SetAttribute("true", "style:flow-with-text");
            }
            _styleXMLdoc.Save(styleFilePath);
        }

        /// <summary>
        /// float = floatProperty =  left/ right/ center
        /// display = displayproperty = none/ block
        /// </summary>
        /// <param name="styleFilePath"></param>
        /// <param name="makeClassName"></param>
        /// <param name="parentName"></param>
        /// <param name="displayProperty"></param>
        /// <param name="graphicStyle"></param>
        public void CreateFrameStyle(string styleFilePath, string makeClassName, string parentName, string displayProperty, string graphicStyle)
        {
            // float = floatProperty =  left/ right/ center
            // display = displayproperty = none/ block

            const string className = "Frame";
            _styleXMLdoc = Common.DeclareXMLDocument(true);
            _styleXMLdoc.Load(styleFilePath);

            var nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

            // if new stylename exists
            XmlElement root = _styleXMLdoc.DocumentElement;
            string style = "//st:style[@st:name='" + makeClassName + "']";
            if (root != null)
            {
                XmlNode node = root.SelectSingleNode(style, nsmgr); // work
                if (node != null)
                {
                    return;
                }
                style = "//st:style[@st:name='" + className + "']";
                node = root.SelectSingleNode(style, nsmgr); // work
				if (node == null)
				{
					return;
				}
                XmlDocumentFragment styleNode = _styleXMLdoc.CreateDocumentFragment();
                styleNode.InnerXml = node.OuterXml;
                node.ParentNode.InsertAfter(styleNode, node);

                _nameElement = (XmlElement)node;
                SetAttribute(makeClassName, "style:name");
                SetAttribute(graphicStyle, "style:parent-style-name");
                string wrap = "dynamic";
                if (displayProperty == "block")
                {
                    wrap = "none";
                    displayProperty = "center";
                }
                else if (displayProperty == "frame")
                {
                    wrap = "dynamic";
                    displayProperty = "center";
                }

                _nameElement = (XmlElement)node.ChildNodes[1];
                SetAttribute("foreground", "style:run-through");
                SetAttribute(wrap, "style:wrap");
                SetAttribute("1", "style:number-wrapped-paragraphs");
                SetAttribute("false", "style:wrap-contour");
                SetAttribute("from-top", "style:vertical-pos");
                SetAttribute("paragraph-content", "style:vertical-rel");
                SetAttribute(displayProperty, "style:horizontal-pos");
                SetAttribute("paragraph", "style:horizontal-rel");
                SetAttribute("true", "style:flow-with-text");

            }
            _styleXMLdoc.Save(styleFilePath);
        }


        #region CreateGraphicsStyle(string styleFilePath, string makeClassName, string parentName, string position, string side)
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Create a Graphics style in style.xml, if style has parent.
        /// New style inherits its parent.
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="styleFilePath">syles.xml path</param>
        /// <param name="makeClassName">combination of child and parent "style name"</param>
        /// <param name="parentName">Parent "style name"</param>
        /// <param name="position">Left/Right</param>
        /// <param name="side">Left/Right</param>
        /// <returns>None</returns>
        /// -------------------------------------------------------------------------------------------
        public void CreateGraphicsStyleLogo(string styleFilePath, string makeClassName, string parentName, string position, string side)
        {
            const string className = "Graphics";
	        _styleXMLdoc = Common.DeclareXMLDocument(true);
            _styleXMLdoc.Load(styleFilePath);

            var nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

            // if new stylename exists
            XmlElement root = _styleXMLdoc.DocumentElement;
            string style = "//st:style[@st:name='" + makeClassName + "']";
            if (root != null)
            {
                XmlNode node = root.SelectSingleNode(style, nsmgr); // work
                if (node != null)
                {
                    return;
                }
                style = "//st:style[@st:name='" + className + "']";
                node = root.SelectSingleNode(style, nsmgr); // work

                XmlDocumentFragment styleNode = _styleXMLdoc.CreateDocumentFragment();
                styleNode.InnerXml = node.OuterXml;
                node.ParentNode.InsertAfter(styleNode, node);

                _nameElement = (XmlElement)node;
                SetAttribute(makeClassName, "style:name");
                if (side == "none" || side == "NoClear")
                {
                    if (position == "left")
                    {
                        side = "right";
                    }
                    else if (position == "right")
                    {
                        side = "left";
                    }
                }
                else if (side == "both")
                {
                    side = position;
                }

                if (position == "right" || position == "left" || position == "center")
                {
                    _nameElement = (XmlElement)node.ChildNodes[1];
                    SetAttribute( "foreground","style:run-through");
                    if (side == "Invalid")
                    {

                    }
                    else if (side == "right" || side == "left")
                    {
                        SetAttribute(side,"style:wrap");
                    }
                    else if (side == "center")
                    {
                        SetAttribute("none","style:wrap");
                    }
                    else
                    {
                        SetAttribute("parallel", "style:wrap");
                    }
                    SetAttribute("no-limit","style:number-wrapped-paragraphs");
                    SetAttribute("false", "style:wrap-contour");
                    SetAttribute( "from-top","style:vertical-pos");
                    SetAttribute("paragraph-content", "style:vertical-rel");
                    SetAttribute(position,"style:horizontal-pos" );
                    SetAttribute("paragraph", "style:horizontal-rel");
                    SetAttribute("true", "style:flow-with-text");
                }
                else if (position == "both")
                {
                    _nameElement = (XmlElement)node.ChildNodes[1];
                    if (side != "")
                    {
                        SetAttribute( side,"style:wrap");
                    }
                    else
                    {
                        SetAttribute("none","style:wrap");
                    }
                    SetAttribute("false", "style:wrap-contour");
                    SetAttribute("from-top", "style:vertical-pos");
                    SetAttribute("paragraph", "style:vertical-rel");
                    SetAttribute("right", "style:horizontal-pos");
                    SetAttribute("paragraph", "style:horizontal-rel");
                }
                else if (position == "top" || position == "bottom")
                {
                    _nameElement = (XmlElement)node.ChildNodes[1];
                    SetAttribute("none", "style:wrap");
                    SetAttribute("false", "style:wrap-contour");
                    SetAttribute(position, "style:vertical-pos");
                    SetAttribute("page-content", "style:vertical-rel");
                    SetAttribute("center", "style:horizontal-pos");
                    SetAttribute("page-content", "style:horizontal-rel");
                }
                else
                {
                    _nameElement = (XmlElement)node.ChildNodes[1];
                    SetAttribute( "top","style:vertical-pos");
                    SetAttribute("baseline", "style:vertical-rel");
                    SetAttribute("from-left", "style:horizontal-pos");
                    SetAttribute("paragraph","style:horizontal-rel");
                }
            }
            _styleXMLdoc.Save(styleFilePath);
        }

        public void CreateFrameStyleLogo(string styleFilePath, string makeClassName, string parentName, string position, string side, string graphicStyle)
        {

            const string className = "Frame";
            _styleXMLdoc = Common.DeclareXMLDocument(true);
            _styleXMLdoc.Load(styleFilePath);

            var nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

            // if new stylename exists
            XmlElement root = _styleXMLdoc.DocumentElement;
            string style = "//st:style[@st:name='" + makeClassName + "']";
            if (root != null)
            {
                XmlNode node = root.SelectSingleNode(style, nsmgr); // work
                if (node != null)
                {
                    return;
                }
                style = "//st:style[@st:name='" + className + "']";
                node = root.SelectSingleNode(style, nsmgr); // work

                XmlDocumentFragment styleNode = _styleXMLdoc.CreateDocumentFragment();
                styleNode.InnerXml = node.OuterXml;
                node.ParentNode.InsertAfter(styleNode, node);

                _nameElement = (XmlElement)node;
                SetAttribute(makeClassName, "style:name");
                SetAttribute(graphicStyle, "style:parent-style-name");
                if (side == "none" || side == "NoClear")
                {
                    if (position == "left")
                    {
                        side = "right";
                    }
                    else if (position == "right")
                    {
                        side = "left";
                    }
                }
                else if (side == "both")
                {
                    side = position;
                }

                if (position == "right" || position == "left" || position == "center")
                {
                    _nameElement = (XmlElement)node.ChildNodes[1];
                    SetAttribute("foreground", "style:run-through");
                    SetAttribute(side == "logo" ? "none" : "dynamic", "style:wrap");
                    SetAttribute("no-limit", "style:number-wrapped-paragraphs");
                    SetAttribute("false", "style:wrap-contour");
                    SetAttribute("from-top", "style:vertical-pos");
                    SetAttribute("paragraph-content", "style:vertical-rel");
                    SetAttribute(position, "style:horizontal-pos");
                    SetAttribute("paragraph", "style:horizontal-rel");
                    SetAttribute("true", "style:flow-with-text");
                }
                else if (position == "both")
                {
                    _nameElement = (XmlElement)node.ChildNodes[1];
                    if (side != "")
                    {
                        SetAttribute(side, "style:wrap");
                    }
                    else
                    {
                        SetAttribute("none", "style:wrap");
                    }
                    SetAttribute("false", "style:wrap-contour");
                    SetAttribute("from-top", "style:vertical-pos");
                    SetAttribute("paragraph", "style:vertical-rel");
                    SetAttribute("right", "style:horizontal-pos");
                    SetAttribute("paragraph", "style:horizontal-rel");
                }
                else if (position == "top" || position == "bottom")
                {
                    _nameElement = (XmlElement)node.ChildNodes[1];
                    SetAttribute("none", "style:wrap");
                    SetAttribute("false", "style:wrap-contour");
                    SetAttribute(position, "style:vertical-pos");
                    SetAttribute("page-content", "style:vertical-rel");
                    SetAttribute("center", "style:horizontal-pos");
                    SetAttribute("page-content", "style:horizontal-rel");
                }
                else
                {
                    _nameElement = (XmlElement)node.ChildNodes[1];
                    SetAttribute("top", "style:vertical-pos");
                    SetAttribute("baseline", "style:vertical-rel");
                    SetAttribute("from-left", "style:horizontal-pos");
                    SetAttribute("paragraph", "style:horizontal-rel");
                }
            }
            _styleXMLdoc.Save(styleFilePath);
        }
 
        /// <summary>
        /// Creates a Style for Drop caps according to no of Characters
        /// </summary>
        /// <example>Chapter No 1, 11, 111, 1111 etc., creates styles for each character   </example>
        /// <param name="styleFilePath">syles.xml path</param>
        /// <param name="className">Child "style name"</param>
        /// <param name="makeClassName">New Child "style name"</param>
        /// <param name="parentName">Parent "style name"</param>
        /// <param name="noOfChar">No of Character to be displayed as Drop Caps</param>
        public void CreateDropCapStyle(string styleFilePath, string className, string makeClassName, string parentName, int noOfChar)
        {
            _styleXMLdoc = new XmlDocument();
            _styleXMLdoc.Load(styleFilePath);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

            // if new stylename exists
            XmlElement root = _styleXMLdoc.DocumentElement;
            string style = "//st:style[@st:name='" + makeClassName + "']";
            if (root != null)
            {
                XmlNode node = root.SelectSingleNode(style, nsmgr); // work
                if (node != null)
                {
                    return;
                }

                style = "//st:style[@st:name='" + className + "']";
                node = root.SelectSingleNode(style, nsmgr);

                if (node == null) // class not exist in Styles.xml
                {
                    style = "//st:style[@st:name='Empty']";
                    node = root.SelectSingleNode(style, nsmgr);
                }

                XmlAttribute attribToBeChanged;
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.Name == "style:paragraph-properties")
                    {
                        foreach (XmlNode chNode in childNode.ChildNodes)
                        {
                            if (chNode.Name == "style:drop-cap")
                            {
                                if (chNode.Attributes.GetNamedItem("style:length") != null)
                                {
                                    attribToBeChanged = chNode.Attributes["style:length"];
                                    attribToBeChanged.Value = noOfChar.ToString();
                                }
                            }

                            var nameAttribute = childNode.Attributes["style:vertical-align"];
                            if (nameAttribute != null)
                            {
                                childNode.Attributes["style:vertical-align"].Value = "auto";
                            }
                        }
                    }
                }

                XmlNode remNode = null;
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.Name == "style:text-properties") // Removing all the Child properties of Text Node, to avoid in Paragraph property.
                    {
                        remNode = childNode;
                    }
                }
                if (remNode != null)
                {
                    node.RemoveChild(remNode);
                }


                XmlDocumentFragment styleNode = _styleXMLdoc.CreateDocumentFragment();
                styleNode.InnerXml = node.OuterXml;
                node.ParentNode.InsertAfter(styleNode, node);

                parentName = parentName.Replace("1", "");
                _nameElement = (XmlElement)node;
                SetAttribute(makeClassName, "style:name");
                SetAttribute(className, "style:parent-style-name");
                SetAttribute("", "style:master-page-name");
            }
            _styleXMLdoc.Save(styleFilePath);
        }
        #endregion


        #region SetColumnGap(string contentFilePath, Dictionary<string, Dictionary<string, string>> columnGapEm)
        /// <summary>
        /// Open Content.xml for updating column-gap em value
        /// </summary>
        /// <param name="contentFilePath">Content.xml file path</param>
        /// <param name="columnGapEm"> Column Gap Value</param>
        /// <returns>Column Property </returns>
        public Dictionary<string, XmlNode> SetColumnGap(string contentFilePath, Dictionary<string, Dictionary<string, string>> columnGapEm)
        {
            var columnGap = new Dictionary<string, XmlNode>();
            var columnGapBuilder = new StringBuilder();
            _styleXMLdoc = new XmlDocument();
            var nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in columnGapEm)
            {
                XmlNode newChild;
                XmlAttribute xmlAttrib;
                columnGapBuilder.Remove(0, columnGapBuilder.Length);
                Dictionary<string, string> columnValue = columnGapEm[kvp.Key];
                float columnGapValue = Common.ConvertToInch(columnValue["columnGap"]);
                float pageWidth = Common.ConvertToInch(columnValue["pageWidth"]);
                var columnCount = (int)Common.ConvertToInch(columnValue["columnCount"]);
                float spacing = columnGapValue / 2;
                float relWidth = (pageWidth - (spacing * (columnCount * 2))) / columnCount;
                XmlNode parentNode = _styleXMLdoc.CreateElement("dummy");
                for (int i = 1; i <= columnCount; i++)
                {
                    float startIndent;
                    float endIndent;
                    if (i == 1)
                    {
                        startIndent = 0.0F;
                        endIndent = spacing;
                    }
                    else if (i == columnCount)
                    {
                        startIndent = spacing;
                        endIndent = 0.0F;
                    }
                    else
                    {
                        startIndent = spacing;
                        endIndent = spacing;
                    }
                    newChild = _styleXMLdoc.CreateElement("style:column", nsmgr.LookupNamespace("st"));
                    xmlAttrib = _styleXMLdoc.CreateAttribute("style:rel-width", nsmgr.LookupNamespace("st"));
                    xmlAttrib.Value = relWidth + "*";
                    newChild.Attributes.Append(xmlAttrib);
                    xmlAttrib = _styleXMLdoc.CreateAttribute("fo:start-indent", nsmgr.LookupNamespace("fo"));
                    xmlAttrib.Value = startIndent + "in";
                    newChild.Attributes.Append(xmlAttrib);
                    xmlAttrib = _styleXMLdoc.CreateAttribute("fo:end-indent", nsmgr.LookupNamespace("fo"));
                    xmlAttrib.Value = endIndent + "in";
                    newChild.Attributes.Append(xmlAttrib);
                    parentNode.AppendChild(newChild);
                }
                columnGap[kvp.Key] = parentNode;
            }
            return columnGap;
        }
        #endregion

        private string OpenIDStyles()
        {
            string projType = "scripture";
            string targetFolder = Common.RightRemove(_projectPath, Path.DirectorySeparatorChar.ToString());
            string fileName = Path.GetFileNameWithoutExtension(_projectPath);
            string styleFilePath = Common.PathCombine(targetFolder, fileName + "styles.xml");

            _styleXMLdoc = new XmlDocument();
            _styleXMLdoc.Load(styleFilePath);
            return styleFilePath;
        }

        private void SetTagProperty(string newClassName)
        {
            _tagName = Common.IsTagClass(newClassName);
            if (_tagName != string.Empty)
            {
                if (_tagName == "ul" || _tagName == "ol")
                {
                    _nameElement.SetAttribute("text:level", "1");
                    _nameElement.SetAttribute("text:style-name", "Bullet_20_Symbols");
                    _nameElement.SetAttribute("style:num-suffix", ".");
                    _nameElement.SetAttribute("text:bullet-char", "1");
                }
            }
        }

   }
}