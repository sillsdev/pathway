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
            bool match;
            XmlNode node = Common.GetXmlNode(FileNameWithPath, XPath);
            if (node == null)
            {
                match = false;
            }
            else
            {
                string value = string.Empty;
                if(node.HasChildNodes)
                {
                    value = node.ChildNodes[0].InnerText;
                }

                if (value == content)
                {
                    match = true;
                }
                else
                {
                    match = false;
                }
            }
            ClassProperty.Clear();
            return match;
        }
        public bool ValidateNextNodeContent(string outputStory, string content)
        {
            FileNameWithPath = Common.PathCombine(outputStory, "Story_1.xml");
            bool match;
            XmlNode node = Common.GetXmlNode(FileNameWithPath, XPath);
            if (node == null)
            {
                match = false;
            }
            else
            {
                string value = string.Empty;
                if (node.ParentNode.ChildNodes.Count > 1)
                {
                    value = node.NextSibling.InnerText;
                }

                if (value == content)
                {
                    match = true;
                }
                else
                {
                    match = false;
                }
            }
            ClassProperty.Clear();
            return match;
        }
    }
}
