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
                        _textProperty[_allTextProperty[propName]  + prop.Key] = prop.Value;
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