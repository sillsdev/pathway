// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2016, SIL International. All Rights Reserved.
// <copyright from='2016' to='2016' company='SIL International'>
//		Copyright (c) 2016, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: EmbedStyles.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System.IO;
using System.Xml;
using SIL.Tool;

namespace CssSimpler
{
    public class EmbedStyles : XmlCopy
    {
        private int _headNode;
        private Stream _xmlStream;

        public EmbedStyles(string input, string output, Stream xmlStream, bool noXmlHeader)
            : base(input, output, noXmlHeader)
        {
            _xmlStream = xmlStream;
            DeclareBefore(XmlNodeType.Element, FindStyleSheetLink);
            DeclareBeforeEnd(XmlNodeType.EndElement, InsertStyles);
            DeclareAfter(XmlNodeType.Element, StopSkipping);
        }

        private void FindStyleSheetLink(XmlReader r)
        {
            if (r.Name == "head")
            {
                _headNode = r.Depth;
            }
            else if (r.Name == "link")
            {
                var relAttr = r.GetAttribute("rel");
                if (relAttr != null && relAttr == "stylesheet")
                {
                    SkipNode = true;
                }
            }
        }

        private void StopSkipping(XmlReader r)
        {
            SkipNode = false;
        }

        private void InsertStyles(int depth, string name)
        {
            if (depth == _headNode)
            {
                var sr = new StreamReader(_xmlStream);
                WriteEmbddedStyle(sr.ReadToEnd());
                sr.Close();
                _headNode = -1;
            }
        }

    }
}
