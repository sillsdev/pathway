// --------------------------------------------------------------------------------------------
// <copyright file="DictionaryForMIDsRec.cs" from='2013' to='2014' company='SIL International'>
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
// Stylepick FeatureSheet
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Xml;

namespace SIL.PublishingSolution
{
    public class DictionaryForMIDsRec
    {
        public string Rec { get; set; }
        public Dictionary<string, Dictionary<string, string>> CssClass;
        public DictionaryForMIDsStyle Styles;
        protected int CurStyle = 1;
        protected bool Styled = false;
        protected bool DisableStyles = false;

        public DictionaryForMIDsRec()
        {
            Rec = string.Empty;
        }

        public void AddHeadword(XmlNode sense)
        {
            var headword = GetHeadwordOfSense(sense);
            DisableStyles = true;
            if (headword != null)
                RenderNode(headword);
            DisableStyles = false;
        }

        private static XmlNode GetHeadwordOfSense(XmlNode sense)
        {
            var entry = GetEntry(sense);
            var headword = entry.FirstChild;
            // pictures use div and headwords use span. Text nodes have no local name
            while (headword != null && headword.LocalName != "span")
            {
                headword = headword.NextSibling;
            }
            return headword;
        }

        private static XmlNode senses;
        private static XmlNode GetEntry(XmlNode sense)
        {
            var entry = sense;
            while (entry.Attributes == null ||
                   entry.Attributes.GetNamedItem("class") == null ||
                   entry.Attributes.GetNamedItem("class").InnerText != "entry" &&
                   entry.Attributes.GetNamedItem("class").InnerText != "reversalindexentry")
            {
                senses = entry;
                entry = entry.ParentNode;
            }
            return entry;
        }

        public void AddBeforeSense(XmlNode sense)
        {
            Rec += "{{";
            StartStyles();
            var field = GetHeadwordOfSense(sense).NextSibling;
            while (field != senses)
            {
                RenderNode(field);
                field = field.NextSibling;
            }
        }

        public void AddSense(XmlNode sense)
        {
            RenderNode(sense);
        }

        public void AddAfterSense(XmlNode sense)
        {
            GetEntry(sense);
            var field = senses.NextSibling;
            while (field != null && field.Attributes != null && field.Attributes.GetNamedItem("id") != null)
            {
                field = field.NextSibling;
            }
            while (field != null)
            {
                RenderNode(field);
                field = field.NextSibling;
            }
            EndStyles();
            Rec += "}}";
        }

        public void AddReversal(XmlNode sense, string className)
        {
            DisableStyles = true;
            Rec += "\t";
            RenderChildClass(sense, className);
            DisableStyles = false;
        }

        private void StartStyles()
        {
            Styled = false;
            CurStyle = 1;
        }

        private void EndStyles()
        {
            if (Styled)
                Rec += "]";
            Styled = false;
        }

        private void RenderChildClass(XmlNode sense, string className)
        {
            RenderNode(GetDefinition(sense, className));
        }

        public static bool HasChildClass(XmlNode sense, string className)
        {
            var node = GetDefinition(sense, className);
            return node != null;
        }

        private static XmlNode GetDefinition(XmlNode sense, string className)
        {
            var classPath = string.Format(@".//*[starts-with(@class, '{0}')]/*[normalize-space() != '']", className);
            XmlNode node = sense.SelectSingleNode(classPath);
            return node;
        }

        private void RenderNode(XmlNode xmlNode)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.NodeType)
                {
                case XmlNodeType.Element:
                    AddBefore(node);
                    RenderNode(node);
                    AddAfter(node);
                    break;
                case XmlNodeType.Text:
                    RenderTextNode(node);
                    break;
                case XmlNodeType.SignificantWhitespace:
                    RenderTextNode(node);
                    break;
                default:
                    throw new InvalidEnumArgumentException();
                }
            }
        }

        private void RenderTextNode(XmlNode node)
        {
            if (IsHomographNumber(node))
                return;

            AddStyleTag(node);
            Rec += Quote(node.InnerText);
        }

        private bool IsHomographNumber(XmlNode node)
        {
            bool resultFound = node.ParentNode != null && (node.ParentNode.Attributes != null && node.ParentNode.Attributes["class"] != null 
                && node.ParentNode.Attributes["class"].Value.IndexOf("xhomographnumber", StringComparison.Ordinal) == 0);
            return resultFound;
        }

        private string Quote(string p)
        {
            return p.Replace("[", @"\[").Replace("]", @"\]").Replace("{", @"\{").Replace("}", @"\}");
        }

        public void AddStyleTag(XmlNode node)
        {
            if (CssClass == null || DisableStyles)
                return;
            var className = GetClassName(node, "font-style");
            var fontStyle = "plain";
            if (className != null)
            {
                if (CssClass[className]["font-style"] != "normal")
                    fontStyle = CssClass[className]["font-style"];
            }
            className = GetClassName(node, "color");
            var fontColor = "128,0,0";
            if (className != null)
            {
                var val = CssClass[className]["color"];
                var red = int.Parse(val.Substring(1, 2), NumberStyles.AllowHexSpecifier);
                var green = int.Parse(val.Substring(3, 2), NumberStyles.AllowHexSpecifier);
                var blue = int.Parse(val.Substring(5, 2), NumberStyles.AllowHexSpecifier);
                fontColor = string.Format("{0},{1},{2}", red, green, blue);
            }
            Debug.Assert(Styles != null);
            className = GetClassName(node, null);
	        if (className == null)
	        {
		        string incrementStyle = "style" + Convert.ToString(CurStyle + 1);
				className = incrementStyle;
	        }
	        var styleNum = Styles.Add(className, fontColor, fontStyle);
		    if (styleNum != CurStyle)
		    {
			    if (Styled)
				    Rec += "]";
			    Rec += string.Format("[{0:D2}", styleNum);
			    CurStyle = styleNum;
			    Styled = true;
		    }
	        
        }

        private string GetClassName(XmlNode node, string property)
        {
            XmlNode classNode = node;
            string className;
            while (classNode != null)
            {
                if (classNode.Attributes != null)
                {
                    if (classNode.Attributes.GetNamedItem("xml:lang") != null && property != null)
                    {
                        className = "xitem_." + classNode.Attributes.GetNamedItem("xml:lang").InnerText;
                        if (CssClass.ContainsKey(className) && CssClass[className].ContainsKey(property))
                            return className;
                    }
                    if (classNode.Attributes.GetNamedItem("lang") != null && property != null)
                    {
                        className = "xitem_." + classNode.Attributes.GetNamedItem("lang").InnerText;
                        if (CssClass.ContainsKey(className) && CssClass[className].ContainsKey(property))
                            return className;
                    }
                    if (classNode.Attributes.GetNamedItem("class") != null)
                    {
                        className = classNode.Attributes.GetNamedItem("class").InnerText.Replace("-", "");
                        if (CssClass.ContainsKey(className))
                        {
                            if (property == null || CssClass[className].ContainsKey(property))
                                return className;
                        }
                    }
                }
                classNode = classNode.ParentNode;
            }
            return null;
        }

        private void AddBefore(XmlNode node)
        {
            BeforeOrAfterContent(node, "..before");
        }

        public void AddAfter(XmlNode node)
        {
            BeforeOrAfterContent(node, "..after");
        }

        private void BeforeOrAfterContent(XmlNode node, string suffix)
        {
            if (CssClass == null)
                return;
            if (node.Attributes == null || node.Attributes.GetNamedItem("class") == null)
                return;
            var className = node.Attributes.GetNamedItem("class").InnerText.Replace("-", "") + suffix;
            if (!CssClass.ContainsKey(className))
                return;
            var properties = CssClass[className];
            if (!properties.ContainsKey("content"))
                return;
            Rec += Quote(properties["content"]);
        }
    }
}
