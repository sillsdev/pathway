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
// File: XmlCopy.cs (from SimpleCss5.cs)
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace CssSimpler
{
    public abstract class XmlCopy
    {
        protected delegate void ParserMethod(XmlReader r);
        private readonly Dictionary<XmlNodeType, List<ParserMethod>> _beforeNodeTypeMap = new Dictionary<XmlNodeType, List<ParserMethod>>();
        private readonly Dictionary<XmlNodeType, List<ParserMethod>> _beforeEndNodeTypeMap = new Dictionary<XmlNodeType, List<ParserMethod>>();
        private readonly Dictionary<XmlNodeType, List<ParserMethod>> _afterNodeTypeMap = new Dictionary<XmlNodeType, List<ParserMethod>>();
        private readonly XmlReader _rdr;
        private readonly XmlWriter _wtr;
        private bool _finish;
        protected bool SkipAttr;
        private bool _doAttributes;

        protected XmlCopy(string xmlInFullName, string xmlOutFullName)
        {
            var settings = new XmlReaderSettings(){DtdProcessing = DtdProcessing.Ignore};
            _rdr = XmlReader.Create(new StreamReader(xmlInFullName), settings);
            _wtr = XmlWriter.Create(xmlOutFullName);
        }

        protected void Parse()
        {
            while (_rdr.Read())
            {
                BeforeProcessMethods();
                if (_finish)
                    break;
                switch ( _rdr.NodeType )
                {
                    case XmlNodeType.Element:
                        _wtr.WriteStartElement( _rdr.Prefix, _rdr.LocalName, _rdr.NamespaceURI );
                        var empty = _rdr.IsEmptyElement;
                        if (_doAttributes)
                        {
                            for (int attrIndex = _rdr.AttributeCount; attrIndex > 0; attrIndex--)
                            {
                                _rdr.MoveToNextAttribute();
                                BeforeProcessMethods();
                                if (!SkipAttr)
                                {
                                    _wtr.WriteAttributeString(_rdr.Name, _rdr.Value);
                                }
                                else
                                {
                                    SkipAttr = false;
                                }
                                AfterProcessMethods();
                            }
                        }
                        else
                        {
                            _wtr.WriteAttributes( _rdr, true );
                        }
                        if (empty)
                        {
                            BeforeEndProcessMethods();
                            _wtr.WriteEndElement();
                        }
                        break;
                    case XmlNodeType.Text:
                        _wtr.WriteString( _rdr.Value );
                        break;
                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                        _wtr.WriteWhitespace(_rdr.Value);
                        break;
                    case XmlNodeType.CDATA:
                        _wtr.WriteCData( _rdr.Value );
                        break;
                    case XmlNodeType.EntityReference:
                        _wtr.WriteEntityRef(_rdr.Name);
                        break;
                    case XmlNodeType.XmlDeclaration:
                        _wtr.WriteProcessingInstruction(_rdr.Name, _rdr.Value);
                        _wtr.WriteDocType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", @"
<!ATTLIST html lang CDATA #REQUIRED >
<!ATTLIST span lang CDATA #IMPLIED >
<!ATTLIST span entryguid CDATA #IMPLIED >
<!ATTLIST img alt CDATA #IMPLIED >
");
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        _wtr.WriteProcessingInstruction( _rdr.Name, _rdr.Value );
                        break;
                    case XmlNodeType.DocumentType: // This code will never execute with DtdProcessing.Ignore (see instantiation)
                        var sysid = _rdr.GetAttribute("SYSTEM");
                        Debug.Assert(sysid != null, "sysid != null");
                        _wtr.WriteDocType( _rdr.Name, _rdr.GetAttribute( "PUBLIC" ), sysid, _rdr.Value );
                        break;
                    case XmlNodeType.Comment:
                        _wtr.WriteComment( _rdr.Value );
                        break;
                    case XmlNodeType.EndElement:
                        BeforeEndProcessMethods();
                        _wtr.WriteFullEndElement();
                        break;
                }
                AfterProcessMethods();
                if (_finish)
                    break;
            }
            for (var depth = _rdr.Depth; depth > 0; depth--)
            {
                BeforeEndProcessMethods();
                _wtr.WriteFullEndElement();
            }
            _wtr.Close();
            _rdr.Close();
        }

        private void BeforeProcessMethods()
        {
            if (!_beforeNodeTypeMap.ContainsKey(_rdr.NodeType)) return;
            foreach (ParserMethod func in _beforeNodeTypeMap[_rdr.NodeType])
            {
                Debug.Assert(func != null, "func != null");
                func(_rdr);
                if (_finish)
                    break;
            }
        }

        private void BeforeEndProcessMethods()
        {
            if (!_beforeEndNodeTypeMap.ContainsKey(_rdr.NodeType)) return;
            foreach (ParserMethod func in _beforeEndNodeTypeMap[_rdr.NodeType])
            {
                Debug.Assert(func != null, "func != null");
                func(_rdr);
                if (_finish)
                    break;
            }
        }

        private void AfterProcessMethods()
        {
            if (!_afterNodeTypeMap.ContainsKey(_rdr.NodeType)) return;
            foreach (ParserMethod func in _afterNodeTypeMap[_rdr.NodeType])
            {
                Debug.Assert(func != null, "func != null");
                func(_rdr);
                if (_finish)
                    break;
            }
        }

        protected void WriteContent(string val, string myClass)
        {
            //var ns = new XmlNamespaceManager(_rdr.NameTable);
            _wtr.WriteStartElement("span", "http://www.w3.org/1999/xhtml");
            WriteAttr(myClass + "-ps");
            if (val.Contains(" "))
            {
                _wtr.WriteAttributeString("xml", "space", "http://www.w3.org/XML/1998/namespace", "preserve");
            }
            _wtr.WriteValue(val);
            _wtr.WriteEndElement();
        }

        protected void WriteAttr(string myClass)
        {
            _wtr.WriteAttributeString("class", myClass);
        }

        protected void Finished()
        {
            _finish = true;
        }

        protected void DeclareBefore(XmlNodeType nodeType, ParserMethod parserMethod)
        {
            if (_beforeNodeTypeMap.ContainsKey(nodeType))
            {
                _beforeNodeTypeMap[nodeType].Add(parserMethod);
            }
            else
            {
                _beforeNodeTypeMap[nodeType] = new List<ParserMethod>() { parserMethod };
            }
            if (nodeType == XmlNodeType.Attribute)
            {
                _doAttributes = true;
            }
        }

        protected void DeclareBeforeEnd(XmlNodeType nodeType, ParserMethod parserMethod)
        {
            if (_beforeEndNodeTypeMap.ContainsKey(nodeType))
            {
                _beforeEndNodeTypeMap[nodeType].Add(parserMethod);
            }
            else
            {
                _beforeEndNodeTypeMap[nodeType] = new List<ParserMethod>() { parserMethod };
            }
            if (nodeType == XmlNodeType.Attribute)
            {
                _doAttributes = true;
            }
        }

        protected void DeclareAfter(XmlNodeType nodeType, ParserMethod parserMethod)
        {
            if (_afterNodeTypeMap.ContainsKey(nodeType))
            {
                _afterNodeTypeMap[nodeType].Add(parserMethod);
            }
            else
            {
                _afterNodeTypeMap[nodeType] = new List<ParserMethod>() { parserMethod };
            }
            if (nodeType == XmlNodeType.Attribute)
            {
                _doAttributes = true;
            }
        }
    }
}
