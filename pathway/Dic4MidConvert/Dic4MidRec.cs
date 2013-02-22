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
using System.Text;
using System.Xml;

namespace SIL.PublishingSolution
{
    public class Dic4MidRec
    {
        public string Rec { get; set; }
        protected bool UnIndexedData = false;

        public Dic4MidRec()
        {
            Rec = string.Empty;
        }

        public void AddHeadword(XmlNode sense)
        {
            var entry = sense.ParentNode;
            RenderNode(entry.FirstChild);
        }

        public void AddB4Sense(XmlNode sense)
        {
            throw new NotImplementedException();
        }

        public void AddSense(XmlNode sense)
        {
            throw new NotImplementedException();
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
                if (nodeClass != null && nodeClass.InnerText.Substring(0, p.Length) == p)
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
                    RenderNode(node);
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

        private void RenderTextNode(XmlNode xmlNode)
        {
            //AddStyleTag(xmlNode);
            //AddBefore(xmlNode);
            Rec += xmlNode.InnerText;
            //AddAfter(xmlNode);
        }

        private void AddStyleTag(XmlNode xmlNode)
        {
            throw new NotImplementedException();
        }

        private void AddBefore(XmlNode xmlNode)
        {
            throw new NotImplementedException();
        }

        private void AddContent(XmlNode xmlNode)
        {
            throw new NotImplementedException();
        }

        private void AddAfter(XmlNode xmlNode)
        {
            throw new NotImplementedException();
        }

    }
}
