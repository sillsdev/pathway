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

namespace SIL.Tool
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
        private bool _noXmlHeader;
        protected bool SkipAttr;
        protected bool SkipNode;
	    protected string StyleDecorate;
		protected readonly List<string> DecorateExceptions = new List<string> {"letHead", "letter", "letData", "reversalindexentry"};
        protected string Suffix = "-ps";
        protected string ReplaceLocalName;
		public string SpaceClass = "sp";
		public string TitleDefault;
        public string AuthorDefault;
        public bool DebugPrint;
        private bool _doAttributes;

        protected XmlCopy(string xmlInFullName, string xmlOutFullName, bool noXmlHeader)
        {
            var settings = new XmlReaderSettings {DtdProcessing = DtdProcessing.Ignore, XmlResolver = new NullResolver()};
            _sr = new StreamReader(xmlInFullName);
            _rdr = XmlReader.Create(_sr, settings);
            _noXmlHeader = noXmlHeader;
            var wrtSettings = new XmlWriterSettings {OmitXmlDeclaration = noXmlHeader};
            _wtr = XmlWriter.Create(xmlOutFullName, wrtSettings);
        }

        public void Parse()
        {
            while (_rdr.Read())
            {
                SkipAttr = SkipNode;
                BeforeProcessMethods();
                if (_finish)
                    break;
                switch ( _rdr.NodeType )
                {
                    case XmlNodeType.Element:
                        if (!SkipNode)
                        {
                            if (DebugPrint)
                            {
                                Debug.Print("start " + _rdr.LocalName);
                            }
                            if (string.IsNullOrEmpty(ReplaceLocalName))
                            {
                                _wtr.WriteStartElement(_rdr.Prefix, _rdr.LocalName, _rdr.NamespaceURI);
                            }
                            else
                            {
                                _wtr.WriteStartElement(_rdr.Prefix, ReplaceLocalName, _rdr.NamespaceURI);
                                ReplaceLocalName = null;
                            }
                        }
                        else
                        {
                            SkipAttr = true;
                        }
                        var empty = _rdr.IsEmptyElement;
                        var depth = _rdr.Depth;
                        var name = _rdr.Name;
                        if (_doAttributes)
                        {
                            for (int attrIndex = _rdr.AttributeCount; attrIndex > 0; attrIndex--)
                            {
                                _rdr.MoveToNextAttribute();
                                SkipAttr = SkipNode;
                                BeforeProcessMethods();
                                if (!SkipAttr)
                                {
                                    if (_rdr.Name.Length > 4 && _rdr.Name.Substring(0, 4) == "xml:")
                                    {
                                        _wtr.WriteAttributeString("xml", _rdr.Name.Substring(4), "http://www.w3.org/XML/1998/namespace", _rdr.Value);
                                    }
                                    else
                                    {
	                                    if (_rdr.Name == "class" && !DecorateExceptions.Contains(_rdr.Value))
	                                    {
											_wtr.WriteAttributeString(_rdr.Name, _rdr.Value + StyleDecorate);
										}
	                                    else
	                                    {
											_wtr.WriteAttributeString(_rdr.Name, _rdr.Value);
										}
									}
                                }
                                AfterProcessMethods();
                            }
                            SkipAttr = false;
                        }
                        else
                        {
                            if (!SkipAttr)
                            {
                                _wtr.WriteAttributes(_rdr, true);
                            }
                        }
                        FirstChildProcessMethods(XmlNodeType.Element);
                        if (empty)
                        {
                            BeforeEndProcessMethods(XmlNodeType.EndElement, depth, name);
                            if (name == "title" && !string.IsNullOrEmpty(TitleDefault))
                            {
                                AddTitle();
                            }
                            else
                            {
                                if (!SkipNode)
                                {
                                    _wtr.WriteEndElement();
                                }
                            }
                        }
                        break;
                    case XmlNodeType.Text:
                        if (!SkipNode)
                        {
                            _wtr.WriteString(_rdr.Value);
                        }
                        break;
                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                        //Debug.Print("space");
		                if (!string.IsNullOrEmpty(SpaceClass) && _rdr.Depth > 1)
		                {
							_wtr.WriteStartElement("span", "http://www.w3.org/1999/xhtml");
							WriteClassAttr(SpaceClass);
							_wtr.WriteAttributeString("xml", "space", "http://www.w3.org/XML/1998/namespace", "preserve");
							_wtr.WriteWhitespace(_rdr.Value);
							_wtr.WriteEndElement();
						}
						else
		                {
							_wtr.WriteWhitespace(_rdr.Value);
						}
						break;
                    case XmlNodeType.CDATA:
                        _wtr.WriteCData( _rdr.Value );
                        break;
                    case XmlNodeType.EntityReference:
                        _wtr.WriteEntityRef(_rdr.Name);
                        break;
                    case XmlNodeType.XmlDeclaration:
                        if (!_noXmlHeader)
                        {
                            _wtr.WriteProcessingInstruction(_rdr.Name, _rdr.Value);
                            _wtr.WriteDocType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", @"
<!ATTLIST html lang CDATA #REQUIRED >
<!ATTLIST span lang CDATA #IMPLIED >
<!ATTLIST span entryguid CDATA #IMPLIED >
<!ATTLIST img alt CDATA #IMPLIED >
");
                        }
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
                        //Debug.Print("End " + _rdr.Name);
                        BeforeEndProcessMethods(_rdr.NodeType, _rdr.Depth, _rdr.Name);
                        if (!SkipNode)
                        {
                            _wtr.WriteFullEndElement();
                        }
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

        private void AddTitle()
        {
            _wtr.WriteString(TitleDefault);
            _wtr.WriteEndElement();
            if (!string.IsNullOrEmpty(AuthorDefault))
            {
                AddAuthor();
            }
        }

        private void AddAuthor()
        {
            _wtr.WriteStartElement(_rdr.Prefix, "meta", _rdr.NamespaceURI);
            _wtr.WriteAttributeString("name", "author");
            _wtr.WriteAttributeString("content", AuthorDefault);
            _wtr.WriteEndElement();
            _wtr.WriteStartElement(_rdr.Prefix, "meta", _rdr.NamespaceURI);
            _wtr.WriteAttributeString("name", "DC.author");
            _wtr.WriteAttributeString("content", AuthorDefault);
            _wtr.WriteEndElement();
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

        protected void WriteEmbddedStyle(string val)
        {
            _wtr.WriteStartElement("style", "http://www.w3.org/1999/xhtml");
            _wtr.WriteAttributeString("type", "text/css");
            _wtr.WriteRaw(val);
            _wtr.WriteEndElement();
        }

        protected void WriteContent(string val, string myClass)
        {
            WriteContent(val, myClass, "");
        }

        protected void WriteContent(string val, string myClass, string myLang)
        {
            WriteContent(val, myClass, myLang, true);
        }

        protected void WriteContent(string val, string myClass, string myLang, bool quotedEntities)
        {
            var localName = string.IsNullOrEmpty(ReplaceLocalName) ? "span" : ReplaceLocalName;
            ReplaceLocalName = null;
            _wtr.WriteStartElement(localName, "http://www.w3.org/1999/xhtml");
            if (!string.IsNullOrEmpty(myClass))
            {
	            var decoration = DecorateExceptions.Contains(myClass) ? "" : StyleDecorate;
				WriteClassAttr(myClass + Suffix + decoration);
            }
            if (!string.IsNullOrEmpty(myLang))
            {
                WriteLangAttr(myLang);
            }
            if (val.StartsWith(" ") || val.EndsWith(" "))
            {
                _wtr.WriteAttributeString("xml", "space", "http://www.w3.org/XML/1998/namespace", "preserve");
            }
            if (quotedEntities && val.Contains(@"\"))
            {
                WriteValueEmbedEntities(val);
            }
            else
            {
                _wtr.WriteValue(val);
            }
            _wtr.WriteEndElement();
            //if (val == "car area for suitcases")
            //{
            //    Debug.Print("found it");
            //}
        }

        protected void WriteValueEmbedEntities(string val)
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

        protected void WriteClassAttr(string myClass)
        {
            _wtr.WriteAttributeString("class", myClass);
        }

        protected void WriteLangAttr(string myLang)
        {
            _wtr.WriteAttributeString("lang", myLang);
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
