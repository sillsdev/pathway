// --------------------------------------------------------------------------------------------
// <copyright file="Dic4MidRec.cs" from='2013' to='2013' company='SIL International'>
//      Copyright © 2013, SIL International. All Rights Reserved.   
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
using System.Xml;

namespace SIL.PublishingSolution
{
    public class Dic4MidRec
    {
        public string Rec { get; set; }
        public Dictionary<string, Dictionary<string, string>> CssClass;
        public Dic4MidStyle Styles;
        protected int CurStyle = 1;
        protected bool Styled = false;

        public Dic4MidRec()
        {
            Rec = string.Empty;
        }

        public void AddHeadword(XmlNode sense)
        {
            var entry = sense.ParentNode;
            Debug.Assert(entry != null);
            RenderNode(entry.FirstChild);
        }

        public void AddB4Sense(XmlNode sense)
        {
            throw new NotImplementedException();
        }

        public void AddSense(XmlNode sense)
        {
            Rec += "{{";
            RenderNode(sense);
            if (Styled)
                Rec += "]";
            Rec += "}}";
        }

        public void AddAfterSense(XmlNode sense)
        {
            throw new NotImplementedException();
        }

        public void AddReversal(XmlNode sense)
        {
            Rec += "\t";
            RenderChildClass(sense, "definition");
        }

        private void RenderChildClass(XmlNode sense, string p)
        {
            foreach (XmlNode node in sense.ChildNodes)
            {
                var nodeClass = node.Attributes.GetNamedItem("class");
                if (nodeClass != null && nodeClass.InnerText.Length >= p.Length &&  nodeClass.InnerText.Substring(0, p.Length) == p)
                {
                    if (node.ChildNodes.Count > 1)
                        RenderNode(node.FirstChild);
                    else
                        RenderNode(node);
                }
            }
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
            AddStyleTag(node);
            Rec += node.InnerText;
        }

        public void AddStyleTag(XmlNode node)
        {
            if (CssClass == null)
                return;
            var className = GetClassName(node);
            if (className == null)
                return;
            var fontStyle = "plain";
            if (CssClass[className].ContainsKey("font-style"))
                if (CssClass[className]["font-style"] != "normal")
                    fontStyle = CssClass[className]["font-style"];
            var fontColor = "128,0,0";
            if (CssClass[className].ContainsKey("color"))
            {
                var val = CssClass[className]["color"];
                var red = int.Parse(val.Substring(1, 2));
                var green = int.Parse(val.Substring(3, 2));
                var blue = int.Parse(val.Substring(5, 2));
                fontColor = string.Format("{0},{1},{2}", red, green, blue);
            }
            Debug.Assert(Styles != null);
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

        private string GetClassName(XmlNode node)
        {
            XmlNode classNode = node;
            string className = null;
            while (classNode != null)
            {
                while (classNode.Attributes == null || classNode.Attributes.GetNamedItem("class") == null)
                    classNode = classNode.ParentNode;
                className = classNode.Attributes.GetNamedItem("class").InnerText.Replace("-", "");
                if (CssClass.ContainsKey(className))
                    break;
                classNode = classNode.ParentNode;
            }
            return className;
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
            if (node.Attributes == null || node.Attributes.GetNamedItem("class") == null)
                return;
            var className = node.Attributes.GetNamedItem("class").InnerText.Replace("-", "") + suffix;
            if (!CssClass.ContainsKey(className))
                return;
            var properties = CssClass[className];
            if (!properties.ContainsKey("content"))
                return;
            Rec += properties["content"];
        }
    }
}
