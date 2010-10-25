// --------------------------------------------------------------------------------------------
// <copyright file="Utility.cs" from='2009' to='2009' company='SIL International'>
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
// Utility Class is used by Styles.cs
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.PublishingSolution;
using SIL.Tool;
using SIL.Tool.Localization;

#endregion Using
namespace SIL.PublishingSolution
{
    #region Class Utility
    /// <summary>
    /// Utility Class is used by Styles.cs
    /// </summary>
    public class Utility
    {
        #region Private Variables
        private string _parentValue;
        private string _makeClassName;
        private string _childName;
        private string _parentName;
        private bool _missingLang;
        #endregion

        #region Public Properties

        public string ChildName
        {
            get { return _childName; }
            set { _childName = value; }
        }
        public bool MissingLang
        {
            get { return _missingLang; }
            set { _missingLang = value; }
        }
        public string ParentName
        {
            get { return _parentName; }
            set { _parentName = value; }
        }
        public string ParentValue
        {
            get { return _parentValue; }
            set { _parentValue = value; }
        }
        public string MakeClassName
        {
            get
            {
                if (_makeClassName == null)
                { return string.Empty; }
                return _makeClassName;
            }
            set { _makeClassName = value; }
        }

        #endregion

        // Public Methods

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

            MakeClassName = string.Empty;

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

            MakeClassName = "Empty";
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

        #region GetFontsize(string styleFilePath, Stack styleStack, StyleAttribute childAttribute)
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Calculate the absolute value based on it'textElement parent _attribute value
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="styleFilePath">syles.xml path</param>
        /// <param name="styleStack">Parent list</param>
        /// <param name="childAttribute">Child _attribute information</param>
        /// <returns>absolute value of relavite parameter</returns>
        /// -------------------------------------------------------------------------------------------
        public string GetFontsize(string styleFilePath, Stack styleStack, StyleAttribute childAttribute)
        {
            string attributeName = childAttribute.Name;
            var parentAttribute = new StyleAttribute();
            float abs = 0F;
            string absValue;

            var doc = new XmlDocument();
            try
            {
                doc.Load(styleFilePath);
            }
            catch (XmlException e)
            {
                var msg = new[] { e.Message, styleFilePath };
                LocDB.Message("errProcessXHTML", styleFilePath + " is Not Valid. " + "\n" + e.Message, msg, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                return "";
            }

            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

            XmlNode node;
            XmlElement root = doc.DocumentElement;

            MakeClassName = string.Empty;

            if (childAttribute.Unit == "em")
            {
                attributeName = "fo:font-size";
            }

            // Find relative value(%)
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
                                    if (childAttribute.Unit == "%")
                                    {
                                        abs = parentAttribute.NumericValue * childAttribute.NumericValue / 100.0F;
                                        // 0.5em = 50%
                                        absValue = abs + "pt";
                                        return (absValue);
                                    }
                                    if (childAttribute.Unit == "em")
                                    {
                                        abs = parentAttribute.NumericValue * childAttribute.NumericValue;
                                        // 0.5em = 50%
                                        absValue = abs + "pt";
                                        return (absValue);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // Apply default value to Related value

            MakeClassName = "Empty";
            if (childAttribute.Unit == "em")
            {
                abs = 12.0F * childAttribute.NumericValue; // 0.5em -> 6pt
            }
            else if (childAttribute.Unit == "%")
            {
                abs = 12.0F * childAttribute.NumericValue / 100.0F; // 50% -> 6pt
            }
            absValue = abs + "pt";
            return (absValue);
        }
        #endregion

        #region GetFontsizeInsideSpan(string styleFilePath, float parentFontsize, StyleAttribute childAttribute)
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Calculate em value from the childAttribute
        /// Update absolute value in styles.xml, If absolute font-size found in the childAttribute.
        /// Update relative value in styles.xml, If relative font-size found in the childAttribute.
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="styleFilePath">syles.xml path</param>
        /// <param name="parentFontsize">Parent Node'textElement Font Size</param>
        /// <param name="childAttribute">Child _attribute information</param>
        /// <returns>true, if absolute value found in childAttribute </returns>
        /// -------------------------------------------------------------------------------------------
        public string GetFontsizeInsideSpan(string styleFilePath, float parentFontsize, StyleAttribute childAttribute)
        {
            //string className = childAttribute.ClassName;
            //string attributeName = childAttribute.Name;
            var insideSpanAttribute = new StyleAttribute();

            float abs = parentFontsize * childAttribute.NumericValue; // Get from parent

            var doc = new XmlDocument();
            doc.Load(styleFilePath);

            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

            XmlElement root = doc.DocumentElement;
            string style = "//st:style[@st:name='" + childAttribute.ClassName + "']";
            if (root != null)
            {
                XmlNode node = root.SelectSingleNode(style, nsmgr);
                if (node != null)  // Get font-size inside the current class
                {
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        XmlNode child = node.ChildNodes[i];
                        XmlNode nodeFontSize = child.Attributes.GetNamedItem("fo:font-size");
                        if (nodeFontSize != null)
                        {
                            insideSpanAttribute.SetAttribute(nodeFontSize.Value);

                            if (insideSpanAttribute.Unit == "pt") // font-size: 10pt; line-space: 1em;
                            {
                                abs = insideSpanAttribute.NumericValue * childAttribute.NumericValue;
                            }
                            else if (insideSpanAttribute.Unit == "%")
                            {
                                abs = parentFontsize * insideSpanAttribute.NumericValue / 100.0F; // font-size: 100%; line-space: 1em;
                                abs = abs * childAttribute.NumericValue;
                            }
                            else if (insideSpanAttribute.Unit == "em") // font-size: 2em; line-space: 1em;
                            {
                                abs = parentFontsize * insideSpanAttribute.NumericValue;
                                abs = abs * childAttribute.NumericValue;
                            }
                        }
                    }
                }
            }
            string absValue = abs + "pt";
            return absValue;
        }
        #endregion

        #region CreateStyleWithNewValue(string styleFilePath, string className, string makeClassName, IDictionary<string, string> makeAttribute, string parentName, string familyType)
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Create a new style in style.xml, if style has parent.
        /// New style inherits its parent.
        /// calling for relative values
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="styleFilePath">syles.xml path</param>
        /// <param name="className">Child "style name"</param>
        /// <param name="makeClassName">combination of child and parent "style name"</param>
        /// <param name="makeAttribute">_attribute list to be override</param>
        /// <param name="parentName">Parent "style name"</param>
        /// <param name="familyType">Paragraph/Text</param>
        /// <param name="backgroundClassName">background class name</param>
        /// <returns>None</returns>
        /// -------------------------------------------------------------------------------------------
        public void CreateStyleWithNewValue(string styleFilePath, string className, string makeClassName, IDictionary<string, string> makeAttribute, string parentName, string familyType, ArrayList backgroundClassName)
        {
            var doc = new XmlDocument();
            doc.Load(styleFilePath);
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
            nsmgr.AddNamespace("of", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");


            // if new stylename exists
            XmlElement root = doc.DocumentElement;
            string style = "//st:style[@st:name='" + makeClassName + "']";
            if (root != null)
            {
                XmlNode node = root.SelectSingleNode(style, nsmgr); // work

                if (node != null && parentName != "")
                {
                    return;
                }

                style = "//st:style[@st:name='" + className + "']";
                node = root.SelectSingleNode(style, nsmgr); // work

                if (node == null)
                {
                    style = "//st:style[@st:name='Empty']";
                    node = root.SelectSingleNode(style, nsmgr); // work
                }

                XmlDocumentFragment styleNode = doc.CreateDocumentFragment();
                if (_missingLang) // set language to the Node when lang style missing
                {
                    // Source style not found 
                    if (node == null)
                    {
                        style = "//st:style[@st:name='Empty']";
                        node = root.SelectSingleNode(style, nsmgr); // work
                    }

                    RemoveBgColor(className, familyType, backgroundClassName, node);

                    styleNode.InnerXml = node.OuterXml;
                    node.ParentNode.InsertAfter(styleNode, node);
                    //var nameElement = (XmlElement)node;
                    //nameElement.SetAttribute("style:name", makeClassName);
                    //nameElement.SetAttribute("style:family", familyType);

                    //nameElement.SetAttribute("style:parent-style-name", parentName);

                    XmlAttributeCollection attrColl = node.Attributes;
                    attrColl["style:name"].Value = makeClassName;
                    attrColl["style:family"].Value = familyType;
                    attrColl["style:parent-style-name"].Value = parentName;

                    XmlNode textNode = null;
                    for (int i = 0; i < node.ChildNodes.Count; i++) // find  Text node
                    {
                        if (node.ChildNodes[i].Name == "style:text-properties")
                        {
                            textNode = node.ChildNodes[i];
                        }
                    }
                    if (textNode == null)
                    {
                        textNode = node.AppendChild(doc.CreateElement("style:text-properties", nsmgr.LookupNamespace("st")));
                    }
                    textNode.Attributes.Append(doc.CreateAttribute("fo:language", nsmgr.LookupNamespace("fo"))).InnerText = makeAttribute["fo:language"];
                    textNode.Attributes.Append(doc.CreateAttribute("fo:country", nsmgr.LookupNamespace("fo"))).InnerText = makeAttribute["fo:country"];
                }
                else // Source style already exists and copied to new style and overrides 
                {

                    styleNode.InnerXml = node.OuterXml;
                    node.ParentNode.InsertAfter(styleNode, node);

                    //var nameElement = (XmlElement)node;
                    //nameElement.SetAttribute("style:name", makeClassName);
                    //nameElement.SetAttribute("style:family", familyType);

                    //nameElement.SetAttribute("style:parent-style-name", parentName);
                    XmlAttributeCollection attrColl = node.Attributes;
                    attrColl["style:name"].Value = makeClassName;
                    attrColl["style:family"].Value = familyType;
                    attrColl["style:parent-style-name"].Value = parentName;

                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        XmlNode child = node.ChildNodes[i];
                        foreach (XmlAttribute attribute in child.Attributes)
                        {
                            if (makeAttribute.ContainsKey(attribute.Name))
                            {
                                attribute.Value = makeAttribute[attribute.Name];
                            }
                        }
                    }
                }

                doc.Save(styleFilePath);

                // copy "fullString" style to "paragraph" style for Display: Block. Example: Buangx.xhtml & Buangx.css
                if (familyType == "text")
                {
                    doc.Load(styleFilePath);
                    nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
                    nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

                    root = doc.DocumentElement;
                    style = "//st:style[@st:name='" + makeClassName + "']";
                    if (root != null) node = root.SelectSingleNode(style, nsmgr);

                    styleNode = doc.CreateDocumentFragment();
                    styleNode.InnerXml = node.OuterXml;
                    node.ParentNode.InsertAfter(styleNode, node);

                    var nameElement = (XmlElement)node;
                    nameElement.SetAttribute("style:family", "paragraph");
                    doc.Save(styleFilePath);
                }
            }
        }
        #endregion

        #region CreateStyle(string styleFilePath, string className, string makeClassName, string parentName, string familyType, ArrayList backgroundClassName)
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Copy style node. (from "_className" to "MakeClassName")
        /// set parent node as "parentName"
        /// calling for NON relative values
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="styleFilePath">syles.xml path</param>
        /// <param name="className">Child "style name"</param>
        /// <param name="makeClassName">combination of child and parent "style name"</param>
        /// <param name="parentName">Parent "style name"</param>
        /// <param name="familyType">Paragraph/Text</param>
        /// <param name="backgroundClassName">Background Color</param>
        /// <param name="isDropCap">DropCap enabled or not</param>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        public void CreateStyle(string styleFilePath, string className, string makeClassName, string parentName, string familyType, ArrayList backgroundClassName, bool isDropCap)
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
                    if (className.IndexOf("_.") >= 0)
                    {
                        className = Common.LeftString(className, "_.");
                    }
                    style = "//st:style[@st:name='" + className + "']";
                    node = root.SelectSingleNode(style, nsmgr);
                }


                if (node == null) // class not exist in Styles.xml
                {
                    style = "//st:style[@st:name='Empty']";
                    node = root.SelectSingleNode(style, nsmgr);
                }

                RemoveBgColor(className, familyType, backgroundClassName, node);

                XmlDocumentFragment styleNode = doc.CreateDocumentFragment();
                styleNode.InnerXml = node.OuterXml;
                node.ParentNode.InsertAfter(styleNode, node);

                //var nameElement = (XmlElement)node;
                //nameElement.SetAttribute("style:name", makeClassName);
                //nameElement.SetAttribute("style:family", familyType);

                //nameElement.SetAttribute("style:parent-style-name", parentName);

                XmlAttributeCollection attrColl = node.Attributes;
                attrColl["style:name"].Value = makeClassName;
                attrColl["style:family"].Value = familyType;
                attrColl["style:parent-style-name"].Value = parentName;
                doc.Save(styleFilePath);

                // copy "fullString" style to "paragraph" style for Display: Block. Example: Buangx.xhtml & Buangx.css
                if (familyType == "text")
                {
                    doc.Load(styleFilePath);
                    nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
                    nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

                    root = doc.DocumentElement;
                    style = "//st:style[@st:name='" + makeClassName + "']";
                    if (root != null) node = root.SelectSingleNode(style, nsmgr);
                    styleNode = doc.CreateDocumentFragment();
                    styleNode.InnerXml = node.OuterXml;
                    node.ParentNode.InsertAfter(styleNode, node);

                    //nameElement = (XmlElement)node;
                    //nameElement.SetAttribute("style:family", "paragraph");
                    attrColl = node.Attributes;
                    attrColl["style:family"].Value = "paragraph";
                    doc.Save(styleFilePath);
                }
            }
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


        private static void RemoveBgColor(string className, string familyType, ArrayList backgroundClassName, XmlNode node)
        {
            XmlAttribute attribStyleFamily = node.Attributes["style:family"];
            XmlNode paraNode = node.FirstChild;
            if (attribStyleFamily.Value == "paragraph" && familyType == "text")
            {
                if (backgroundClassName.Contains(className) && paraNode != null)
                {
                    if (paraNode.Attributes.GetNamedItem("fo:background-color") != null)
                    {
                        XmlAttribute attibBackgroundColor = paraNode.Attributes["fo:background-color"];
                        XmlNode textNode = node.LastChild;

                        if (textNode.Attributes.GetNamedItem("fo:background-color") != null)
                        {
                            XmlAttribute attibBackgroundColorText = textNode.Attributes["fo:background-color"];
                            attibBackgroundColorText.Value = attibBackgroundColor.Value;
                        }
                        else
                        {
                            XmlNode att = attibBackgroundColor.Clone();
                            XmlAttribute attibBackgroundColorText1 = att.Attributes["fo:background-color"];
                            textNode.Attributes.Append(attibBackgroundColorText1);
                        }

                        //textNode.Attributes.Append(attibBackgroundColor);
                        node.RemoveChild(paraNode);
                        node.AppendChild(textNode);
                    }
                }
            }
        }

        /// <summary>
        /// Remove the paragraph style if the character style has the same name.
        /// </summary>
        /// <param name="styleFilePath">styles xml file path</param>
        /// <param name="unUsedParagraphStyle">List of unused paragraph style</param>
        public void RemoveUnUsedStyle(string styleFilePath, List<string> unUsedParagraphStyle)
        {
            if (unUsedParagraphStyle.Count == 0)
                return;

            var doc = new XmlDocument();
            doc.Load(styleFilePath);
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("of", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");


            // if new stylename exists
            XmlElement root = doc.DocumentElement;
            string style = "//st:style[@st:family='paragraph']";
            if (root != null)
            {
                XmlNodeList node = root.SelectNodes(style, nsmgr);
                if (node != null)
                {
                    int nodeCount = node.Count;
                    for (int i = 0; i < nodeCount; i++)
                    {
                        XmlNode name = node[i].Attributes.GetNamedItem("name", nsmgr.LookupNamespace("st"));
                        if (name != null && unUsedParagraphStyle.Contains(name.Value))
                        {
                            node[i].ParentNode.RemoveChild(node[i]);

                            string style1 = "//st:style[@st:name='" + name.Value + "'] | st:style[@st:family='text']";
                            XmlNode nodeText = root.SelectSingleNode(style1, nsmgr);
                            if (nodeText != null)
                            {
                                if (nodeText.ChildNodes.Count > 1)
                                    nodeText.RemoveChild(nodeText.FirstChild);
                            }
                            nodeCount--;
                        }
                    }
                }
            }
            doc.Save(styleFilePath);
        }

        /// <summary>
        /// Adding Hyphenation to the Particular Paragraph.
        /// </summary>
        /// <param name="styleFilePath">syles.xml path</param>
        /// <param name="className">StyleName in which Hyphenation to be added</param>
        public void CreateStyleHyphenate(string styleFilePath, string className)
        {
            var doc = new XmlDocument();
            doc.Load(styleFilePath);
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");

            string style = "//st:style[@st:name='" + className + "']";
            XmlElement root = doc.DocumentElement;
            if (root != null)
            {
                XmlNode node = root.SelectSingleNode(style, nsmgr);

                if (node == null) // class not exist in Styles.xml
                {
                    return;
                }

                XmlAttribute attribStyleFamily = node.Attributes["style:family"];
                if (attribStyleFamily.Value == "paragraph")
                {

                    if (node.ChildNodes.Count > 1)
                    {
                        XmlNode paraNode = node.FirstChild.NextSibling;
                        if (paraNode.Attributes.GetNamedItem("fo:hyphenate") != null)
                        {
                            XmlNode attribHyphen = paraNode.Attributes.GetNamedItem("fo:hyphenate");
                            if (string.Compare(attribHyphen.Value, "true", true) >= 0)
                            {
                                return;
                            }
                            //attribHyphen.Value = "true";  // to Overwrite the false property to true , use this.
                        }
                        else
                        {
                            XmlAttribute newAttrib = doc.CreateAttribute("fo:hyphenate", nsmgr.LookupNamespace("fo"));
                            newAttrib.Value = "true";
                            paraNode.Attributes.Append(newAttrib);
                        }
                    }
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
        #endregion

        #region GetNewChildName(string styleFilePath, Stack styleStack, string child, bool isRelative)
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Create new Parent "style name"
        /// </summary>
        /// <param name="styleFilePath"></param>
        /// <param name="styleStack">Parent tree stored in styleStack stack </param>
        /// <param name="child">child "style name"</param>
        /// <param name="isRelative">True/False </param>
        /// <returns>New "style name".</returns>
        /// -------------------------------------------------------------------------------------------
        public string GetNewChildName(string styleFilePath, Stack styleStack, string child, bool isRelative)
        {
            // Handling missing Language in .CSS file
            if (child.LastIndexOf("_.") > 0)
            {
                var doc = new XmlDocument();
                doc.Load(styleFilePath);
                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
                nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
                XmlElement root = doc.DocumentElement;
                string style = "//st:style[@st:name='" + child + "']";
                if (root != null)
                {
                    XmlNode node = root.SelectSingleNode(style, nsmgr);
                    if (node == null) // class not exist in Styles.xml
                    {
                        _missingLang = true;
                    }
                }
                // END. Handled missing Language in .CSS file
            }
            ParentName = string.Empty;
            foreach (string parentClass in styleStack)
            {
                // parentName = parentClass;
                if (parentClass != "ClassEmpty")
                {
                    ParentName = parentClass;
                    ChildName = child + "_" + parentClass;
                    return child;
                }
            }
            if (isRelative)
            {
                ChildName = child + "_";
            }
            else
            {
                ChildName = child;
            }
            return child;
        }
        #endregion

        #region IsStyleExist(string styleFilePath, string styleName)
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Find the CssClassName in Styles.xml
        /// </summary>
        /// <param name="styleFilePath">Path of Styles.xml</param>
        /// <param name="styleName">Class Name</param>
        /// <returns>True/False</returns>
        /// -------------------------------------------------------------------------------------------
        public bool IsStyleExist(string styleFilePath, string styleName)
        {
            var doc = new XmlDocument();
            doc.Load(styleFilePath);
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
            XmlElement root = doc.DocumentElement;
            string style = "//st:style[@st:name='" + styleName + "']";
            if (root != null)
            {
                XmlNode node = root.SelectSingleNode(style, nsmgr);
                if (node != null) // class not exist in Styles.xml
                {
                    return true;
                }
            }
            return false;
        }

        public void GraphicContentChange(string contentFilePath, ArrayList graphicNames)
        {
            if (graphicNames.Count == 0) return;
            //MessageBox.Show("Entered");
            var doc = new XmlDocument { PreserveWhitespace = true, XmlResolver = null };
            string file;
            if (!Common.Testing)
            {
                file = Path.Combine(contentFilePath, "content.xml");
            }
            else
            {
                file = contentFilePath + "content.xml";
            }
            doc.Load(file);

            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("draw", "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");

            string makeClassName = graphicNames[0].ToString();
            // if new stylename exists
            XmlElement root = doc.DocumentElement;
            //<draw:frame draw:style-name="Graphics0" 
            string style = "//draw:frame[@draw:style-name='" + makeClassName + "']";
            if (root != null)
            {
                XmlNode node = root.SelectSingleNode(style, nsmgr); // work

                if (node == null)
                {
                    return;
                }

                XmlDocumentFragment styleNode = doc.CreateDocumentFragment();
                styleNode.InnerXml = node.FirstChild.FirstChild.OuterXml;
                //styleNode.RemoveChild(styleNode.FirstChild);
                //node.ParentNode.InsertAfter(styleNode, node);
                node.ParentNode.ReplaceChild(styleNode, node);
            }

            doc.Save(file);
        }
    }
        #endregion
    // End - Public Methods
}
    #endregion


