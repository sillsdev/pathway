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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace CssSimpler
{
    public abstract class XmlCopy
    {
        protected delegate void ParserMethod(XmlReader r);
        protected delegate void EndTagMethod(int dept, string name);
        private readonly Dictionary<XmlNodeType, List<ParserMethod>> _beforeNodeTypeMap = new Dictionary<XmlNodeType, List<ParserMethod>>();
        private readonly Dictionary<XmlNodeType, List<ParserMethod>> _firstChildNodeTypeMap = new Dictionary<XmlNodeType, List<ParserMethod>>();
        private readonly Dictionary<XmlNodeType, List<EndTagMethod>> _beforeEndNodeTypeMap = new Dictionary<XmlNodeType, List<EndTagMethod>>();
        private readonly Dictionary<XmlNodeType, List<ParserMethod>> _afterNodeTypeMap = new Dictionary<XmlNodeType, List<ParserMethod>>();
        private readonly StreamReader _sr;
        private readonly XmlReader _rdr;
        private readonly XmlWriter _wtr;
        private bool _finish;
        protected bool SkipAttr;
        private bool _doAttributes;

        protected XmlCopy(string xmlInFullName, string xmlOutFullName)
        {
            var settings = new XmlReaderSettings(){DtdProcessing = DtdProcessing.Ignore, XmlResolver = new NullResolver()};
            _sr = new StreamReader(xmlInFullName);
            _rdr = XmlReader.Create(_sr, settings);
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
                        var depth = _rdr.Depth;
                        var name = _rdr.Name;
                        if (_doAttributes)
                        {
                            for (int attrIndex = _rdr.AttributeCount; attrIndex > 0; attrIndex--)
                            {
                                _rdr.MoveToNextAttribute();
                                BeforeProcessMethods();
                                if (!SkipAttr)
                                {
                                    if (_rdr.Name.Length > 4 && _rdr.Name.Substring(0, 4) == "xml:")
                                    {
                                        _wtr.WriteAttributeString("xml", _rdr.Name.Substring(4), "http://www.w3.org/XML/1998/namespace", _rdr.Value);
                                    }
                                    else
                                    {
                                        _wtr.WriteAttributeString(_rdr.Name, _rdr.Value);
                                    }
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
                        FirstChildProcessMethods(XmlNodeType.Element);
                        if (empty)
                        {
                            BeforeEndProcessMethods(XmlNodeType.EndElement, depth, name);
                            _wtr.WriteEndElement();
                        }
                        break;
                    case XmlNodeType.Text:
                        if (_rdr.Value == "a cabo")
                        {
                            Debug.Print("pause");
                        }
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
                        break;
                    case XmlNodeType.Comment:
                        _wtr.WriteComment( _rdr.Value );
                        break;
                    case XmlNodeType.EndElement:
                        BeforeEndProcessMethods(_rdr.NodeType, _rdr.Depth, _rdr.Name);
                        _wtr.WriteFullEndElement();
                        break;
                }
                AfterProcessMethods();
                if (_finish)
                    break;
            }
            for (var depth = _rdr.Depth; depth > 0; depth--)
            {
                BeforeEndProcessMethods(XmlNodeType.EndElement, _rdr.Depth, _rdr.Name);
                _wtr.WriteFullEndElement();
            }
            _wtr.Close();
            _rdr.Close();
            _sr.Close();
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

        private void FirstChildProcessMethods(XmlNodeType type)
        {
            if (!_firstChildNodeTypeMap.ContainsKey(type)) return;
            foreach (ParserMethod func in _firstChildNodeTypeMap[type])
            {
                Debug.Assert(func != null, "func != null");
                func(_rdr);
                if (_finish)
                    break;
            }
        }

        private void BeforeEndProcessMethods(XmlNodeType type, int depth, string name)
        {
            if (!_beforeEndNodeTypeMap.ContainsKey(type)) return;
            foreach (EndTagMethod func in _beforeEndNodeTypeMap[type])
            {
                Debug.Assert(func != null, "func != null");
                func(depth, name);
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
            if (!string.IsNullOrEmpty(myClass))
            {
            WriteAttr(myClass + "-ps");
            }
            if (val.Contains(" "))
            {
                _wtr.WriteAttributeString("xml", "space", "http://www.w3.org/XML/1998/namespace", "preserve");
            }
            if (val.Contains(@"\"))
            {
                WriteValueEmbedEntities(val);
            }
            else
            {
                _wtr.WriteValue(val);
            }
            _wtr.WriteEndElement();
        }

        private void WriteValueEmbedEntities(string val)
        {
            var matches = Regex.Matches(val, @"\\([0-9ABCDEF]+)", RegexOptions.IgnoreCase);
            var position = 0;
            foreach (Match match in matches)
            {
                var index = match.Groups[0].Index;
                if (index > position)
                {
                    _wtr.WriteValue(val.Substring(position, index - position));
                    position = index;
                }
                _wtr.WriteCharEntity((char) Convert.ToInt32(match.Groups[1].ToString(), 16));
                position += match.Length;
            }
            if (position < val.Length)
            {
                _wtr.WriteValue(val.Substring(position));
            }
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

        protected void DeclareFirstChild(XmlNodeType nodeType, ParserMethod parserMethod)
        {
            if (_firstChildNodeTypeMap.ContainsKey(nodeType))
            {
                _firstChildNodeTypeMap[nodeType].Add(parserMethod);
            }
            else
            {
                _firstChildNodeTypeMap[nodeType] = new List<ParserMethod>() { parserMethod };
            }
            if (nodeType == XmlNodeType.Attribute)
            {
                _doAttributes = true;
            }
        }

        protected void DeclareBeforeEnd(XmlNodeType nodeType, EndTagMethod endTagMethod)
        {
            if (_beforeEndNodeTypeMap.ContainsKey(nodeType))
            {
                _beforeEndNodeTypeMap[nodeType].Add(endTagMethod);
            }
            else
            {
                _beforeEndNodeTypeMap[nodeType] = new List<EndTagMethod>() { endTagMethod };
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
