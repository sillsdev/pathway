using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using SIL.Tool;

namespace Test
{
    public class ValidateXMLFile
    {
        public string XPath;
        public string FileNameWithPath;
        public Dictionary<string, string> ClassProperty = new Dictionary<string, string>();
        public string Attrib;
        public string AttribNameSpace;
        public string ClassName;
        public string ChildClassName;
        public string ChildClassType;
        public bool ClassNameTrim;
        public bool GetInnerText;
        public bool GetOuterXml;
        private XmlNamespaceManager nsmgr;
        private XmlNode TempNode;
        private XmlDocument LoadedDoc;

        public ValidateXMLFile(string fileName)
        {
            Attrib = string.Empty;
            AttribNameSpace = string.Empty;
            ClassName = string.Empty;
            ChildClassName = string.Empty;
            ChildClassType = string.Empty;
            ClassNameTrim = true;
            GetInnerText = false;
            GetOuterXml = false;
            FileNameWithPath = fileName;
            LoadedDoc = GetDoc();
        }
        public ValidateXMLFile()
        {
        }
        public bool ValidateNodeAttribute()
        {
            string propertyValue;
            XmlNodeReader reader;
            bool match = true;

            XmlNode node = Common.GetXmlNode(FileNameWithPath, XPath);

            if (node == null)
            {
                match = false;
            }
            else
            {
                foreach (string propertyKey in ClassProperty.Keys)
                {
                    reader = new XmlNodeReader(node);
                    reader.Read();
                    propertyValue = reader.GetAttribute(propertyKey);
                    if (propertyValue != ClassProperty[propertyKey])
                    {
                        match = false;
                    }
                }
            }
            reader = null;
            ClassProperty.Clear();
            return match;
        }

        public bool ValidateNodeAttributesNS(bool paraAttrib)
        {
            string propertyValue = string.Empty;
            bool match = true;
            XPath = "//style:style[@style:name='" + ClassName + "']";
            XmlNode node = ValidateGetNodeNS();

            if (node == null)
            {
                match = false;
            }
            else
            {
                foreach (XmlNode xmlNode in node.ChildNodes)
                {
                    if (paraAttrib && xmlNode.Name == "style:paragraph-properties")
                    {
                        node = xmlNode;
                        break;
                    }
                    if (!paraAttrib && xmlNode.Name == "style:text-properties")
                    {
                        node = xmlNode;
                        break;
                    }

                }
                foreach (string propertyKey in ClassProperty.Keys)
                {

                    string ns = string.Empty;
                    string key = string.Empty;
                    if (propertyKey.IndexOf(':') > 0)
                    {
                        ns = Common.LeftString(propertyKey, ":");
                        key = Common.RightString(propertyKey, ":");
                    }

                    XmlNode att = node.Attributes.GetNamedItem(key, nsmgr.LookupNamespace(ns));
                    if (att != null)
                    {
                        propertyValue = att.Value;
                    }

                    if (propertyValue != ClassProperty[ns + ":" + key])
                    {
                        match = false;
                        break;
                    }
                }
            }

            ClassProperty.Clear();
            return match;
        }

        /// <summary>
        /// Validates the node against the searched xpath
        /// </summary>
        /// <param name="count">0 - parent itself, 1 - first child, 2- second child </param>
        /// <param name="xpath">path to search</param>
        /// <returns>true / false</returns>
        public bool ValidateNodeAttributesNS(int count, string xpath)
        {
            string propertyValue = string.Empty;
            bool match = true;
            
            XPath = "//style:style[@style:name='" + ClassName + "']";
            if (xpath.Length <= 0)
            {
                xpath = XPath;
                ClassName = string.Empty;
            }

            XmlNode node = GetNode(xpath);

            if (node == null)
            {
                match = false;
            }
            else
            {
                int counter = 0;
                if (count > 0)
                {
                    foreach (XmlNode xmlNode in node.ChildNodes)
                    {
                        counter++;
                        if (count == counter)
                        {
                            node = xmlNode;
                            break;
                        }
                    }
                }
                foreach (string propertyKey in ClassProperty.Keys)
                {
                    string ns = string.Empty;
                    string key = string.Empty;
                    if (propertyKey.IndexOf(':') > 0)
                    {
                        ns = Common.LeftString(propertyKey, ":");
                        key = Common.RightString(propertyKey, ":");
                    }

                    XmlNode att = GetAttibute(node, key, ns);
                    if (att != null)
                    {
                        propertyValue = att.Value;
                        if (propertyValue != ClassProperty[ns + ":" + key])
                        {
                            match = false;
                            break;
                        }
                    }
                }
            }

            ClassProperty.Clear();
            return match;
        }

        public XmlNode GetAttibute(XmlNode node, string key, string ns)
        {
            return node.Attributes.GetNamedItem(key, nsmgr.LookupNamespace(ns));
        }

        public bool ValidateNodeInnerXml(string xpath, string value)
        {
            bool match = true;
            XmlNode node = GetNode(xpath);

            if (node == null)
            {
                match = false;
            }
            else
            {
                string inner;
                if (GetOuterXml)
                {
                    inner = GetReplacedOuterXml(node);
                }
                else if (GetInnerText)
                {
                    inner = GetReplacedInnerText(node);
                }
                else
                {
                    inner = GetReplacedInnerXml(node);
                }

                if (inner != value)
                {
                    match = false;
                }
            }
            return match;
        }

        public bool ValidateOfficeTextNode(string paraSpan)
        {
            bool match = true;

            XmlNode node = GetOfficeNode();
            if (node == null)
            {
                match = false;
            }
            else
            {
                string subpath = paraSpan.ToLower() == "para"
                                     ? "//text:p[@text:style-name='" + ClassName + "']"
                                     : "//text:span[@text:style-name='" + ClassName + "']";
                XPath = subpath;
                node = ValidateGetNodeNS(node);
                if (node == null)
                {
                    match = false;
                }
            }
            return match;
        }

        public bool ValidateOfficeTextNode(int count,string value, string paraSpan)
        {
            bool match = true;

            XmlNode node = GetOfficeNode();
            if (node == null) 
            {
                match = false;
            }
            else
            {
                if (ClassName.Length > 0)
                {
                    string subpath = paraSpan.ToLower() == "para"
                                         ? "//text:p[@text:style-name='" + ClassName + "']"
                                         : "//text:span[@text:style-name='" + ClassName + "']";
                    XPath = subpath;
                }

                node = ValidateGetNodeNS(node);
                if (node != null)
                {
                    int counter = 0;
                    if (count > 0)
                    {
                        foreach (XmlNode xmlNode in node.ChildNodes)
                        {
                            counter++;
                            if (count == counter)
                            {
                                node = xmlNode;
                                break;
                            }
                        }
                    }

                        string inner;
                        if (GetOuterXml)
                        {
                            inner = GetReplacedOuterXml(node);
                        }
                        else if (GetInnerText)
                        {
                            inner = GetReplacedInnerText(node);
                        }
                        else
                        {
                            inner = GetReplacedInnerXml(node);
                        }

                        if (inner != value)
                        {
                            match = false;
                        }
                }
                else
                {
                    match = false;
                }
            }
            return match;
        }
        public bool ValidateOfficeTextNode(string value, string paraSpan)
        {
            bool match = true;

            XmlNode node = GetOfficeNode();
            if (node == null)
            {
                match = false;
            }
            else
            {
                string subpath = paraSpan.ToLower() == "para"
                                     ? "//text:p[@text:style-name='" + ClassName + "']"
                                     : "//text:span[@text:style-name='" + ClassName + "']";
                XPath = subpath;
                node = ValidateGetNodeNS(node);
                if (node != null)
                {
                    if (ChildClassName.Length > 0 && ChildClassType.Length > 0)
                    {
                        subpath = ChildClassType.ToLower() == "para"
                                     ? "//text:p[@text:style-name='" + ChildClassName + "']"
                                     : "//text:span[@text:style-name='" + ChildClassName + "']";
                        XPath = subpath;
                        node = ValidateGetNodeNS(node);
                        if (node != null)
                        {
                            string innerText = GetReplacedInnerXml(node);
                            if (innerText != value)
                            {
                                match = false;
                            }
                        }

                    }
                    else
                    {
                        string inner;
                        if (GetOuterXml)
                        {
                            inner = GetReplacedOuterXml(node);
                        }
                        else if (GetInnerText)
                        {
                            inner = GetReplacedInnerText(node);
                        }
                        else
                        {
                            inner = GetReplacedInnerXml(node);
                        }
                        if (inner != value)
                        {
                            match = false;
                        }
                    }
                }
                else
                {
                    match = false;
                }
            }
            return match;
        }

        public XmlNode GetOfficeNode()
        {
            string oldXpath = XPath;

            string xpath = "//office:text";
            XPath = xpath;
            XmlNode node = ValidateGetNodeNS();

            XPath = oldXpath;

            return node;
        }

        public bool ValidateNodeInnerXmlSubNode(string value)
        {
            bool match = true;

            XmlNode node = GetOfficeNode();
            if (node == null)
            {
                match = false;
            }
            else
            {
                node = GetSubNode(node);
                if (node != null)
                {
                    string inner;
                    if (GetOuterXml)
                    {
                        inner = GetReplacedOuterXml(node);
                    }
                    else if (GetInnerText)
                    {
                        inner = GetReplacedInnerText(node);
                    }
                    else
                    {
                        inner = GetReplacedInnerXml(node);
                    }
   
                    if (inner != value)
                    {
                        match = false;
                    }
                }
                else
                {
                    match = false;
                }
            }
            return match;
        }

        public XmlNode GetSubNode(XmlNode node)
        {
            XPath = "//text:p[@text:style-name='" + ClassName + "']";
            node = ValidateGetNodeNS(node);
            return node;
        }

        private string GetReplacedInnerXml(XmlNode returnValue)
        {
            string inner = returnValue.InnerXml;
            return GetReplaceString(inner);
        }

        private string GetReplacedOuterXml(XmlNode returnValue)
        {
            string outer = returnValue.OuterXml;
            return GetReplaceString(outer);
        }

        public string GetReplacedInnerText(XmlNode returnValue)
        {
            string inner = returnValue.InnerText;
            return GetReplaceString(inner);
        }

        public string GetReplaceString(string returnValue)
        {
            string inner = returnValue.Replace("\r\n", "");
            inner = inner.Replace("\t", "");
            if (ClassNameTrim) // If it is true then remove spaces in both sides
                inner = inner.Trim();
            return inner;
        }
        private XmlNode GetNode(string xpath)
        {
            XmlNode node;
            if (ClassName.Length > 0)
            {
                XPath = xpath + ClassName + "']";
            }
            else
            {
                XPath = xpath;
            }
            if (xpath.ToLower() != "samenode")
            {
                node = ValidateGetNodeNS();
                TempNode = node;
            }
            else
            {
                node = TempNode;
            }
            return node;
        }

        /// <summary>
        /// search the node with namespace
        /// </summary>
        /// <returns>the attributes value</returns>
        public XmlNode ValidateGetNodeNS()
        {
            XmlNode returnValue = null;
            if (LoadedDoc != null)
            {
                XmlElement root = LoadedDoc.DocumentElement;
                if (root != null)
                {
                    returnValue = root.SelectSingleNode(XPath, nsmgr); // work
                }
            }
            return returnValue;
        }

        /// <summary>
        /// search the node with namespace
        /// </summary>
        /// <returns>the attributes value</returns>
        public XmlNode ValidateGetNodeNS(XmlNode xnode)
        {
            XmlNode returnValue = xnode.SelectSingleNode(XPath, nsmgr); // work
            return returnValue;
        }
        private XmlDocument GetDoc()
        {
            //Openoffice search
            XmlDocument doc = null;
            if (File.Exists(FileNameWithPath))
            {
                doc = new XmlDocument();
                doc.Load(FileNameWithPath);

                nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("style", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
                nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
                nsmgr.AddNamespace("office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
                nsmgr.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
                nsmgr.AddNamespace("draw", "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");
                
            }
            return doc;
        }



        public bool IsNodeExists()
        {
            bool match = true;

            XmlNode node = Common.GetXmlNodeInDesignNamespace(FileNameWithPath, XPath);

            if (node == null)
            {
                match = false;
            }
            ClassProperty.Clear();
            return match;
        }

        public bool ValidateNodeValue()
        {
            bool match = true;
            XmlNode node = Common.GetXmlNode(FileNameWithPath, XPath);
            if (node == null)
            {
                match = false;
            }
            else
            {
                foreach (string propertyKey in ClassProperty.Keys)
                {
                    string value = node.ChildNodes[0].InnerText;
                    if (value != ClassProperty[propertyKey])
                    {
                        match = false;
                    }
                }
            }
            ClassProperty.Clear();
            return match;
        }

        public bool ValidateNodeContent(string outputStory, string content)
        {
            FileNameWithPath = Common.PathCombine(outputStory, "Story_1.xml");
            bool match = false;
            XmlNode node = Common.GetXmlNode(FileNameWithPath, XPath);
            if (node != null)
            {
                string value = string.Empty;
                if (node.HasChildNodes)
                {
                    value = node.ChildNodes[0].InnerText;
                }
                match = value == content;
            }
            ClassProperty.Clear();
            return match;
        }
        public bool ValidateNextNodeContent(string outputStory, string content)
        {
            FileNameWithPath = Common.PathCombine(outputStory, "Story_1.xml");
            bool match = false;
            XmlNode node = Common.GetXmlNode(FileNameWithPath, XPath);
            if (node != null)
            {
                string value = string.Empty;
                if (node.ParentNode.ChildNodes.Count > 1)
                {
                    value = node.NextSibling.InnerText;
                }

                match = value == content;
            }

            ClassProperty.Clear();
            return match;
        }

        //public bool ValidateNodeContent_OO(string outputStory, string content)
        //{
        //    FileNameWithPath = outputStory;
        //    bool match = false;
        //    XmlNode node = (FileNameWithPath, XPath);
        //    if (node != null)
        //    {
        //        string value = string.Empty;
        //        if (node.HasChildNodes)
        //        {
        //            value = node.ChildNodes[0].InnerText;
        //        }
        //        match = value == content;
        //    }
        //    ClassProperty.Clear();
        //    return match;
        //}

        //public static ArrayList GetXmlNodeList(string xmlFileNameWithPath, string xPath)
        //{
        //    ArrayList dataList = new ArrayList();
        //    XmlNode resultNode = GetXmlNode(xmlFileNameWithPath, xPath);
        //    if (resultNode != null)
        //    {
        //        foreach (XmlNode node in resultNode.ChildNodes)
        //        {
        //            dataList.Add(node.InnerText);
        //        }
        //    }
        //    return dataList;
        //}


    }
}
