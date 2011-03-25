using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ModifyXeTexStyles
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
        private string _xetexFullFile;
        private bool _isHeadword;
        private ArrayList _textVariables = new ArrayList();
        private List<string> _mergedClass = new List<string>();
        Dictionary<string, string> _languageStyleName = new Dictionary<string, string>();
        Dictionary<string, Dictionary<string, string>> _childStyle = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> _tempStyle;
        Dictionary<string, Dictionary<string, string>> mergedStyle = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, Dictionary<string, string>> _cssClass = new Dictionary<string, Dictionary<string, string>>();
        XeTexMapProperty mapProperty = new XeTexMapProperty();

        public void ModifyStylesXML(string projectPath, StreamWriter xetexFile, Dictionary<string, Dictionary<string, string>> newProperty, 
            Dictionary<string, Dictionary<string, string>> cssClass, string xetexFullFile)
        {
            _projectPath = projectPath;
            _cssClass = cssClass;
            _xetexFullFile = xetexFullFile;
            //foreach (KeyValuePair<string, Dictionary<string, string>> cssStyle in newProperty)
            //{
            //    MergeCssStyle(cssStyle.Key);
            //}
            MapProperty();
        }

        private void MapProperty()
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _cssClass)
            {
                if (cssClass.Key.IndexOf("h1") >= 0 ||
                    cssClass.Key.IndexOf("h2") >= 0 || cssClass.Key.IndexOf("h3") >= 0 ||
                    cssClass.Key.IndexOf("h4") >= 0 || cssClass.Key.IndexOf("h5") >= 0 ||
                    cssClass.Key.IndexOf("h6") >= 0) continue;

                List<string> _inlineStyle = new List<string>();
                
                string className = RemoveBody(cssClass.Key);
                if (className.Length == 0 ) continue; 

                string xeTexProperty = mapProperty.XeTexProperty(cssClass.Value, className, _inlineStyle);

                if (xeTexProperty.Trim().Length > 0)
                {
                    Common.FileInsertText(_xetexFullFile, xeTexProperty);
                    //_xetexFile.WriteLine(xeTexProperty);
                }


            }
        }

        private string RemoveBody(string paraStyle)
        {
            if (paraStyle.IndexOf("_body") == -1 && paraStyle != "@page")
            {
                return string.Empty;
            }
            paraStyle = paraStyle.Replace("_body", "");
            string simplified = paraStyle.Replace("_", "");
            return simplified;
        }

        /// <summary>
        /// parentClassName = b_a
        /// step1: b_a = b 
        /// step2: b_a = merge a
        /// </summary>
        /// <param name="paraStyle"></param>
        private void MergeCssStyle(string paraStyle)
        {
            paraStyle = paraStyle.Replace("_body", "");
            string parentClass = paraStyle;
            string mergedClass = paraStyle.Replace("_", "");
            string[] parent = paraStyle.Split('_');
            if (parent.Length > 0)
            {
                string childClass = Common.LeftString(paraStyle, "_");
                parentClass = paraStyle.Replace(childClass + "_", "");
                parentClass = parentClass.Replace("_", "");
                if (_cssClass.ContainsKey(mergedClass))
                {
                    return;
                }
                _tempStyle = new Dictionary<string, string>();
                //Copy
                if (_cssClass.ContainsKey(childClass))
                {
                    foreach (KeyValuePair<string, string> property in _cssClass[childClass])
                    {
                        _tempStyle[property.Key] = property.Value;
                    }
                }
                //Merge
                if (_cssClass.ContainsKey(parentClass))
                {
                    foreach (KeyValuePair<string, string> property in _cssClass[parentClass])
                    {
                        if (!_tempStyle.ContainsKey(property.Key))
                            _tempStyle[property.Key] = property.Value;
                    }
                }
                _cssClass[mergedClass] = _tempStyle;
            }
            _mergedClass.Add(mergedClass);
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

            _node = _root.SelectSingleNode(_xPath, nsmgr);
            //if (_node == null) return;
            XmlDocumentFragment styleNode = _styleXMLdoc.CreateDocumentFragment();
            styleNode.InnerXml = _node.OuterXml;
            _node.ParentNode.InsertAfter(styleNode, _node);

            newClassName = _tagType + "/" + className.Key;

            _nameElement = (XmlElement)_node;
            _nameElement.SetAttribute("Self", newClassName);
            _nameElement.SetAttribute("Name", className.Key);
            _nameElement.SetAttribute("NextStyle", newClassName);
            SetTagProperty(className.Key);
            foreach (KeyValuePair<string, string> property in className.Value)
            {
                if (property.Key == "Leading" || property.Key == "lang")
                {
                    continue;
                }
                _nameElement.SetAttribute(property.Key, property.Value);
            }

            SetLanguage(className.Key);
            SetBasedOn("None", newClassName);
            SetAppliedFont(className.Value, newClassName);
            SetLineHeight(className.Value, newClassName);
            SetBaseLineShift(className.Value, newClassName);
            SetTagNode();
        }

        private void SetLanguage(string className)
        {
            if (_tagType == "ParagraphStyle") // Note - If needed apply only for paragraph style.
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
                        string point2 = Common.LeftString(pointSize, ".");
                        int pt = int.Parse(point2);
                        //int baseshift = pt - 12;
                        int baseshift = pt * 2 / 3;
                        int point = pt * 2 / 3;
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
            targetFolder = Common.PathCombine(targetFolder, "Resources");
            string styleFilePath = Common.PathCombine(targetFolder, "Styles.xml");

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