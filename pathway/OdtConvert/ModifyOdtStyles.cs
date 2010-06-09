using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    class ModifyOdtStyles
    {
        private XmlDocument _styleXMLdoc;
        private XmlNode _node;
        private XmlElement _root;
        private XmlNamespaceManager nsmgr;
        const string _styleSeperator = "_";
        private string _projectPath;
        private string _tagType;
        private string _xPath;

        Dictionary<string, Dictionary<string, string>> _childStyle = new Dictionary<string, Dictionary<string, string>>();

        public void ModifyStylesXML(string projectPath, Dictionary<string, Dictionary<string, string>> childStyle)
        {
            _childStyle = childStyle;
            _projectPath = projectPath;
            string styleFilePath = OpenIDStyles();

            nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
            nsmgr.AddNamespace("idPkg", "http://ns.adobe.com/AdobeInDesign/idml/1.0/packaging");

            _root = _styleXMLdoc.DocumentElement;
            if (_root == null)
            {
                return;
            }

            foreach (KeyValuePair<string, Dictionary<string, string>> className in _childStyle)
            {
                _tagType = "ParagraphStyle";
                _xPath = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + "$ID/NormalParagraphStyle" + "\"]";
                InsertNode(className);
                _tagType = "CharacterStyle";
                _xPath = "//RootCharacterStyleGroup/CharacterStyle[@Name = \"" + "$ID/NormalCharacterStyle" + "\"]";
                InsertNode(className);
            }
            _styleXMLdoc.Save(styleFilePath);
        }

        private void InsertNode(KeyValuePair<string, Dictionary<string, string>> className)
        {
            string newClassName = className.Key;
            string parentClassName = Common.RightString(newClassName, _styleSeperator);

            _node = _root.SelectSingleNode(_xPath, nsmgr);

            XmlDocumentFragment styleNode = _styleXMLdoc.CreateDocumentFragment();
            styleNode.InnerXml = _node.OuterXml;
            _node.ParentNode.InsertAfter(styleNode, _node);

            newClassName = _tagType + "/" + className.Key;

            var nameElement = (XmlElement)_node;
            nameElement.SetAttribute("Self", newClassName);
            nameElement.SetAttribute("Name", className.Key);
            nameElement.SetAttribute("NextStyle", newClassName);
            foreach (KeyValuePair<string, string> property in className.Value)
            {
                nameElement.SetAttribute(property.Key, property.Value);
            }
            SetBasedOn(parentClassName, newClassName);

            if (className.Value.ContainsKey("AppliedFont"))
            {
                SetAppliedFont(newClassName, className.Value["AppliedFont"]);
            }
        }

        public void ModifyStylesXMLOLD(string projectPath, Dictionary<string, Dictionary<string, string>> childStyle)
        {
            _childStyle = childStyle;
            _projectPath = projectPath;
            string styleFilePath = OpenIDStyles();

            nsmgr = new XmlNamespaceManager(_styleXMLdoc.NameTable);
            nsmgr.AddNamespace("idPkg", "http://ns.adobe.com/AdobeInDesign/idml/1.0/packaging");

            _root = _styleXMLdoc.DocumentElement;
            if (_root == null)
            {
                return;
            }

            foreach (KeyValuePair<string, Dictionary<string, string>> className in _childStyle)
            {
                string newClassName = className.Key;
                string sourceClassName = Common.LeftString(newClassName, _styleSeperator);
                string parentClassName = Common.RightString(newClassName, _styleSeperator);

                string style = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + newClassName + "\"]";
                _node = _root.SelectSingleNode(style, nsmgr);

                // Is Child Exist 
                if (_node != null && parentClassName != "")
                {
                    continue;
                }


                style = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + parentClassName + "\"]";
                _node = _root.SelectSingleNode(style, nsmgr);

                // Is Parent Missing
                if (_node == null)
                {
                    //style = "//st:style[@st:name='Empty']";  // TODO selecting dummy style
                    //_node = _root.SelectSingleNode(style, nsmgr);
                    return;
                }

                style = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + sourceClassName + "\"]";
                _node = _root.SelectSingleNode(style, nsmgr);

                // Is Source Missing
                if (_node == null)
                {
                    sourceClassName = parentClassName;
                    style = "//RootParagraphStyleGroup/ParagraphStyle[@Name = \"" + sourceClassName + "\"]";
                    _node = _root.SelectSingleNode(style, nsmgr);
                }

                XmlDocumentFragment styleNode = _styleXMLdoc.CreateDocumentFragment();
                styleNode.InnerXml = _node.OuterXml;
                _node.ParentNode.InsertAfter(styleNode, _node);

                newClassName = "ParagraphStyle/" + className.Key;

                var nameElement = (XmlElement)_node;
                nameElement.SetAttribute("Self", newClassName);
                nameElement.SetAttribute("Name", className.Key);
                nameElement.SetAttribute("NextStyle", newClassName);
                foreach (KeyValuePair<string, string> property in className.Value)
                {
                    nameElement.SetAttribute(property.Key, property.Value);
                }
                SetBasedOn(parentClassName, newClassName);
            }
            _styleXMLdoc.Save(styleFilePath);
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

        private void SetAppliedFont(string sourceClassName, string fontFamily)
        {
            string style = "//" + _tagType + "[@Self='" + sourceClassName + "']/Properties/AppliedFont";
            XmlNode nodeAppliedFont = _root.SelectSingleNode(style, nsmgr);
            if (nodeAppliedFont != null)
            {
                var nameElement = (XmlElement)nodeAppliedFont;
                nameElement.SetAttribute("type", "string");
                nodeAppliedFont.InnerText = fontFamily;
            }
        }

        private string OpenIDStyles()
        {
            string projType = "scripture";
            //string targetFolder = Common.PathCombine(Common.GetTempFolderPath(), "InDesignFiles" + Path.DirectorySeparatorChar + projType);
            string targetFolder = Common.RightRemove(_projectPath, "/");
            //targetFolder = Common.PathCombine(targetFolder, "Resources");
            string styleFilePath = Common.PathCombine(targetFolder, "Styles.xml");

            _styleXMLdoc = new XmlDocument();
            _styleXMLdoc.Load(styleFilePath);
            return styleFilePath;
        }

        public void CreateMasterContents(string styleFilePath, ArrayList odtFiles)
        {
            var doc = new XmlDocument();
            doc.Load(styleFilePath);
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            //nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            //nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
            nsmgr.AddNamespace("office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
            nsmgr.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
            nsmgr.AddNamespace("xlink", "http://www.w3.org/1999/xlink");


            // if new stylename exists
            XmlElement root = doc.DocumentElement;
            //string style = "//st:style[@st:name='" + makeClassName + "']";
            string style = "//office:text";
            if (root != null)
            {

                XmlNode node = root.SelectSingleNode(style, nsmgr); // work
                if (node != null)
                {
                    for (int i = 0; i < odtFiles.Count; i++) // ODM - ODT 
                    {
                        string outputFile = odtFiles[i].ToString();
                        outputFile = Path.ChangeExtension(outputFile, "odt");
                        //Path.GetFileNameWithoutExtension(outputFile);
                        //outputFile = Common.PathCombine(outputFile)
                        //string outputFile = odtFiles[i].ToString().Replace("xhtml", "odt");
                        XmlNode newNode;
                        newNode = doc.CreateNode("element", "text:section", nsmgr.LookupNamespace("text"));
                        //attribute
                        XmlAttribute xmlAttrib = doc.CreateAttribute("text:style-name", nsmgr.LookupNamespace("text"));
                        xmlAttrib.Value = "SectODM";
                        newNode.Attributes.Append(xmlAttrib);

                        xmlAttrib = doc.CreateAttribute("text:name", nsmgr.LookupNamespace("text"));
                        xmlAttrib.Value = outputFile;
                        newNode.Attributes.Append(xmlAttrib);

                        xmlAttrib = doc.CreateAttribute("text:protected", nsmgr.LookupNamespace("text"));
                        xmlAttrib.Value = "false";
                        newNode.Attributes.Append(xmlAttrib);


                        XmlNode newNode1 = doc.CreateNode("element", "text:section-source", nsmgr.LookupNamespace("text"));
                        //attribute
                        XmlAttribute xmlAttrib1 = doc.CreateAttribute("xlink:href", nsmgr.LookupNamespace("xlink"));
                        xmlAttrib1.Value = "../" + outputFile;
                        newNode1.Attributes.Append(xmlAttrib1);


                        xmlAttrib1 = doc.CreateAttribute("text:filter-name", nsmgr.LookupNamespace("text"));
                        xmlAttrib1.Value = "writer8";
                        newNode1.Attributes.Append(xmlAttrib1);

                        newNode.AppendChild(newNode1);
                        node.AppendChild(newNode);
                    }
                }
            }

            doc.Save(styleFilePath);

        }


    }
}
