using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    class ModifyOOStyles : OOStyles
    {
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
        private bool _isHeadword;
        private ArrayList _textVariables = new ArrayList();
        Dictionary<string, string> _languageStyleName = new Dictionary<string, string>();
        Dictionary<string, Dictionary<string, string>> _childStyle = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, string> _parentClass;
        public ArrayList ModifyStylesXML(string projectPath, Dictionary<string, Dictionary<string, string>> childStyle, List<string> usedStyleName, Dictionary<string, string> languageStyleName, string baseStyle, bool isHeadword, Dictionary<string, string> parentClass)
        {
            LoadAllProperty();
            _childStyle = childStyle;
            _projectPath = projectPath;
            _isHeadword = isHeadword;
            _languageStyleName = languageStyleName;
            _parentClass = parentClass;
            string styleFilePath = OpenIDStyles(); //todo change name

            //nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
            //nsmgr.AddNamespace("idPkg", "http://ns.adobe.com/AdobeInDesign/idml/1.0/packaging");
            nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
            nsmgr.AddNamespace("style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

            _root = _styleXMLdoc.DocumentElement;
            if (_root == null)
            {
                return null;
            }

            string paraStyle = "Empty";// "$ID/NormalParagraphStyle";
            string charStyle = "Empty";//"$ID/NormalCharacterStyle";

            //if(baseStyle.Length > 0)
            //{
            //    paraStyle =  baseStyle;
            //}
            CreateStyle(paraStyle, charStyle, usedStyleName);
            _styleXMLdoc.Save(styleFilePath);
            return _textVariables;
        }

        private void CreateStyle(string paraStyle, string charStyle, List<string> usedStyleName)
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> className in _childStyle)
            {
                if (!usedStyleName.Contains(className.Key))
                    continue;
                //SetVisibilityColor(className);
                //_tagType = "paragraph";
                //_xPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + paraStyle + "\"]";
                //_xPath = "//st:style[@st:name=\"" + paraStyle + "\"]";
                _xPath = "//style:style[@style:name=\"" + paraStyle + "\"]";
                InsertNode(className);
                //_tagType = "text";
                //_xPath = "//st:style[@st:name=\"" + charStyle + "\"]";
                //InsertNode(className);
                //GetVariableClassName(className.Key);
            }
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
            string[] parent_Type = _parentClass[newClassName].Split('|');

            string familyType = parent_Type[1] == "div" ? "paragraph" : "text";

            XmlNode _node = _root.SelectSingleNode(_xPath, nsmgr);
            if (_node == null) return;
            XmlDocumentFragment styleNode = _styleXMLdoc.CreateDocumentFragment();
            styleNode.InnerXml = _node.OuterXml;
            _node.ParentNode.InsertAfter(styleNode, _node);

            _nameElement = (XmlElement)_node;
            _nameElement.SetAttribute("style:name", newClassName);
            _nameElement.SetAttribute("style:family", familyType);
            _nameElement.SetAttribute("style:parent-style-name", parent_Type[0]); 
            //style:family="paragraph" style:parent-style-name="none">
            AddParaTextNode(className, _node);


            //SetLanguage(className.Key);
            //SetBasedOn("None", newClassName);
            //SetAppliedFont(className.Value,newClassName);
            //SetLineHeight(className.Value, newClassName);
            //SetBaseLineShift(className.Value, newClassName);
            //SetTagNode();
        }

        private void AddParaTextNode(KeyValuePair<string, Dictionary<string, string>> className, XmlNode node)
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
                        _textProperty[keyName] = prop.Value;

                        // adding complex attribute
                        if (keyName == "fo:font-weight" || keyName == "fo:font-size" || keyName == "fo:font-family")
                        {
                            string propertyName = keyName;
                            propertyName = propertyName == "fo:font-family"
                                               ? "style:font-name"
                                               : propertyName.Replace("fo:", "style:");
                            _textProperty[propertyName + "-complex"] = prop.Value;
                        }
                    }

                }
                else if (_allColumnProperty.ContainsKey(propName)) // fullString property
                {
                    _columnProperty[_allColumnProperty[propName] + prop.Key] = prop.Value;
                }

            }

            XmlNode paraNode = null;
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

                XmlElement paraElement = (XmlElement)paraNode; ;
                foreach (KeyValuePair<string, string> para in _paragraphProperty)
                {
                    string[] property = para.Key.Split(':');
                    string ns = property[0];
                    string prop = property[1];
                    paraElement.SetAttribute(prop, nsmgr.LookupNamespace(ns), para.Value);
                }
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

                XmlElement textElement = (XmlElement)textNode;
                foreach (KeyValuePair<string, string> para in _textProperty)
                {
                    string[] property = para.Key.Split(':');
                    string ns = property[0];
                    //if (ns == "st") ns = "style";
                    string prop = property[1];
                    textElement.SetAttribute(prop, nsmgr.LookupNamespace(ns), para.Value);
                }
            }
            if (_columnProperty.Count > 0) // create a value XML file for content.xml with column property.
            {
                CreateColumnXMLFile(className.Key);
            }

        }

        private void CreateColumnXMLFile(string className)
        {
            try
            {

                //_styleName.SectionName.Add(className.Trim());
                string path = Common.PathCombine(Path.GetTempPath(), "_" + className.Trim() + ".xml");

                XmlTextWriter writerCol = new XmlTextWriter(path, null);
                writerCol.Formatting = Formatting.Indented;
                writerCol.WriteStartDocument();
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

                    if (columnGap.IndexOf("em") > 0 || columnGap.IndexOf("%") > 0)
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
                writerCol.WriteEndDocument();
                writerCol.Flush();
                writerCol.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void SetLanguage(string className)
        {
            if (_tagType == "paragraph") // Note - If needed apply only for paragraph style.
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
                        int pt = int.Parse(pointSize);
                        //int baseshift = pt - 12;
                        int baseshift = pt * 2 / 3;
                        int point = pt * 2/3;
                        nameElement.SetAttribute("BaselineShift", "-" + baseshift);
                        nameElement.SetAttribute("PointSize", "-" + point);
                    }
                
                }
            }
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
        public void CreateGraphicsStyle(string styleFilePath, string makeClassName, string parentName, string position, string side)
        {
            const string className = "Graphics";
            var doc = new XmlDocument { XmlResolver = null };
            doc.Load(styleFilePath);

            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

            // if new stylename exists
            XmlElement root = doc.DocumentElement;
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

                XmlDocumentFragment styleNode = doc.CreateDocumentFragment();
                styleNode.InnerXml = node.OuterXml;
                node.ParentNode.InsertAfter(styleNode, node);

                var nameElement = (XmlElement)node;
                nameElement.SetAttribute("style:name", makeClassName);
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
                    var nameGraphicElement = (XmlElement)node.ChildNodes[0];
                    nameGraphicElement.SetAttribute("style:run-through", "foreground");
                    if (side == "Invalid")
                    {

                    }
                    else if (side == "right" || side == "left")
                    {
                        nameGraphicElement.SetAttribute("style:wrap", side);
                    }
                    else if (side == "center")
                    {
                        nameGraphicElement.SetAttribute("style:wrap", "none");
                    }
                    else
                    {
                        nameGraphicElement.SetAttribute("style:wrap", "dynamic");
                    }
                    nameGraphicElement.SetAttribute("style:number-wrapped-paragraphs", "no-limit");
                    nameGraphicElement.SetAttribute("style:wrap-contour", "false");
                    nameGraphicElement.SetAttribute("style:vertical-pos", "from-top");
                    nameGraphicElement.SetAttribute("style:vertical-rel", "paragraph");
                    nameGraphicElement.SetAttribute("style:horizontal-pos", position);
                    nameGraphicElement.SetAttribute("style:horizontal-rel", "paragraph");
                    // this is for text flow
                    //nameGraphicElement.SetAttribute("style:flow-with-text", "true");
                }
                else if (position == "both")
                {
                    var nameGraphicElement = (XmlElement)node.ChildNodes[0];
                    if (side != "")
                    {
                        nameGraphicElement.SetAttribute("style:wrap", side);
                    }
                    else
                    {
                        nameGraphicElement.SetAttribute("style:wrap", "none");
                    }
                    nameGraphicElement.SetAttribute("style:wrap-contour", "false");
                    nameGraphicElement.SetAttribute("style:vertical-pos", "from-top");
                    nameGraphicElement.SetAttribute("style:vertical-rel", "paragraph");
                    nameGraphicElement.SetAttribute("style:horizontal-pos", "right");
                    nameGraphicElement.SetAttribute("style:horizontal-rel", "paragraph");
                }
                else if (position == "top")
                {
                    var nameGraphicElement = (XmlElement)node.ChildNodes[0];
                    nameGraphicElement.SetAttribute("style:wrap", "none");
                    nameGraphicElement.SetAttribute("style:wrap-contour", "false");
                    nameGraphicElement.SetAttribute("style:vertical-pos", position);
                    nameGraphicElement.SetAttribute("style:vertical-rel", "page-content");
                    nameGraphicElement.SetAttribute("style:horizontal-pos", "center");
                    nameGraphicElement.SetAttribute("style:horizontal-rel", "page-content");
                }
                else
                {
                    var nameGraphicElement = (XmlElement)node.ChildNodes[0];
                    nameGraphicElement.SetAttribute("style:vertical-pos", "top");
                    nameGraphicElement.SetAttribute("style:vertical-rel", "baseline");
                    nameGraphicElement.SetAttribute("style:horizontal-pos", "from-left");
                    nameGraphicElement.SetAttribute("style:horizontal-rel", "paragraph");
                }
            }
            doc.Save(styleFilePath);
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
            var doc = new XmlDocument();
            doc.Load(styleFilePath);
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

            // if new stylename exists
            XmlElement root = doc.DocumentElement;
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
                                //if (childNode.Attributes.GetNamedItem("style:lines") != null)  // if needed we can use this for no of lines.
                                //{
                                //    attribToBeChanged = childNode.Attributes["style:lines"];
                                //    attribToBeChanged.Value = noOfChar.ToString();
                                //}

                                if (chNode.Attributes.GetNamedItem("style:length") != null)
                                {
                                    attribToBeChanged = chNode.Attributes["style:length"];
                                    attribToBeChanged.Value = noOfChar.ToString();
                                }
                            }
                        }
                    }
                    if (childNode.Name == "style:text-properties") // Removing all the Child properties of Text Node, to avoid in Paragraph property.
                    {
                        node.RemoveChild(childNode);
                    }
                }
                XmlDocumentFragment styleNode = doc.CreateDocumentFragment();
                styleNode.InnerXml = node.OuterXml;
                node.ParentNode.InsertAfter(styleNode, node);

                parentName = parentName.Replace("1", "");
                var nameElement = (XmlElement)node;
                nameElement.SetAttribute("style:name", makeClassName);
                nameElement.SetAttribute("style:parent-style-name", parentName);
            }
            doc.Save(styleFilePath);
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
            var xmlDoc = new XmlDocument();
            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
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
                XmlNode parentNode = xmlDoc.CreateElement("dummy");
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
                    newChild = xmlDoc.CreateElement("style:column", nsmgr.LookupNamespace("st"));
                    xmlAttrib = xmlDoc.CreateAttribute("style:rel-width", nsmgr.LookupNamespace("st"));
                    xmlAttrib.Value = relWidth + "*";
                    newChild.Attributes.Append(xmlAttrib);
                    xmlAttrib = xmlDoc.CreateAttribute("fo:start-indent", nsmgr.LookupNamespace("fo"));
                    xmlAttrib.Value = startIndent + "in";
                    newChild.Attributes.Append(xmlAttrib);
                    xmlAttrib = xmlDoc.CreateAttribute("fo:end-indent", nsmgr.LookupNamespace("fo"));
                    xmlAttrib.Value = endIndent + "in";
                    newChild.Attributes.Append(xmlAttrib);
                    parentNode.AppendChild(newChild);
                }
                columnGap[kvp.Key] = parentNode;
            }
            return columnGap;
        }
        #endregion

        #region GetFontweight(string styleFilePath, Stack styleStack, StyleAttribute childAttribute)

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Calculate the Fontweight value based on it'textElement parent _attribute value
        /// </summary>
        /// <param name="styleFilePath">syles.xml path</param>
        /// <param name="styleStack">Parent list</param>
        /// <param name="childAttribute">Child _attribute information</param>
        /// <returns>absolute value of relavite parameter</returns>
        /// -------------------------------------------------------------------------------------------
        public string GetFontweight(string styleFilePath, Stack styleStack, StyleAttribute childAttribute)
        {
            string attributeName = childAttribute.Name;
            var parentAttribute = new StyleAttribute();
            float abs;
            string absValue = string.Empty;

            var doc = new XmlDocument();
            doc.Load(styleFilePath);

            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

            XmlNode node;
            XmlElement root = doc.DocumentElement;

            string MakeClassName = string.Empty;

            // Find parent value
            foreach (string parentClass in styleStack)
            {
                if (parentClass == "ClassEmpty")
                {
                    continue;
                }
                MakeClassName = MakeClassName + parentClass + "_";
                string style = "//st:style[@st:name='" + parentClass.Trim() + "']";
                if (root != null)
                {
                    node = root.SelectSingleNode(style, nsmgr); // work}
                    if (node != null)
                    {
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            XmlNode child = node.ChildNodes[i];
                            foreach (XmlAttribute attribute in child.Attributes)
                            {
                                if (attribute.Name == attributeName)
                                {
                                    parentAttribute.SetAttribute(attribute.Value);
                                    if (childAttribute.StringValue == "lighter")
                                    {
                                        abs = parentAttribute.NumericValue - 300;
                                        if (abs < 100)
                                        {
                                            abs = 100;
                                        }
                                        absValue = abs.ToString();
                                        return (absValue);
                                    }
                                    if (childAttribute.StringValue == "bolder")
                                    {
                                        abs = parentAttribute.NumericValue + 300;
                                        if (abs > 900)
                                        {
                                            abs = 900;
                                        }
                                        absValue = abs.ToString();
                                        return (absValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // Apply default value to Related value

            if (childAttribute.StringValue == "lighter")
            {
                absValue = "400"; // 0.5em -> 6pt
            }
            else if (childAttribute.StringValue == "bolder")
            {
                absValue = "700"; // 50% -> 6pt
                //abs = 0.0F * childAttribute.NumericValue / 100.0F; // 50% -> 0pt
            }
            return (absValue);
        }
        #endregion


        private string OpenIDStyles()
        {
            string projType = "scripture";
            //string targetFolder = Common.PathCombine(Common.GetTempFolderPath(), "InDesignFiles" + Path.DirectorySeparatorChar + projType);
            string targetFolder = Common.RightRemove(_projectPath, Path.DirectorySeparatorChar.ToString());
            //targetFolder = Common.PathCombine(targetFolder, "Resources");
            //string styleFilePath = Common.PathCombine(targetFolder, "Styles.xml");
            string fileName = Path.GetFileNameWithoutExtension(_projectPath);
            string styleFilePath = Common.PathCombine(targetFolder, fileName + "Styles.xml");

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