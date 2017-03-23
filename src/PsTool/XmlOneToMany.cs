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
// File: XmlOneToMany.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace SIL.Tool
{
    public abstract class XmlOneToMany
    {
        #region Variables
        protected delegate void ParserMethod(XmlReader r);
        protected delegate void EndTagMethod(int dept, string name);
        private readonly Dictionary<XmlNodeType, List<ParserMethod>> _beforeNodeTypeMap = new Dictionary<XmlNodeType, List<ParserMethod>>();
        private readonly Dictionary<XmlNodeType, List<ParserMethod>> _firstChildNodeTypeMap = new Dictionary<XmlNodeType, List<ParserMethod>>();
        private readonly Dictionary<XmlNodeType, List<EndTagMethod>> _beforeEndNodeTypeMap = new Dictionary<XmlNodeType, List<EndTagMethod>>();
        private readonly Dictionary<XmlNodeType, List<ParserMethod>> _afterNodeTypeMap = new Dictionary<XmlNodeType, List<ParserMethod>>();
		private readonly Dictionary<string, string> ns = new Dictionary<string, string> ();
        private readonly StreamReader _sr;
        private readonly XmlReader _rdr;
        private FileStream _writerfile;
        private XmlWriter _wtr;
        public bool Writing;
        private bool _finish;
        protected bool SkipAttr;
        protected bool SkipNode;
        protected string Suffix = "-ps";
        protected string ReplaceLocalName;
        protected string ReplaceAttrValue;
        public bool NoXmlHeader;
        public string TitleDefault;
        public string AuthorDefault;
        public bool DebugPrint;
        private bool _doAttributes;
        private readonly StringBuilder _header = new StringBuilder();
        #endregion Variables

        #region Construct and capture file header
        public string Header
        {
            get { return _header.ToString(); }
            set
            {
                _header.Clear();
                _header.Append(value);
            }
        }

        protected XmlOneToMany(string xmlInFullName)
        {
			ns["xml"] = "http://www.w3.org/XML/1998/namespace";
			ns["xmlns"] = "";
            _sr = new StreamReader(xmlInFullName);
            _rdr = XmlReader.Create(_sr, MyReaderSettings());
            _wtr = XmlWriter.Create(_header);
            Writing = true;
			Parse(true, _rdr);
			_wtr.WriteEndElement();
			_wtr.WriteEndElement();
			_wtr.Close();
			Writing = false;
		}

		private StringReader _hsr;
        private XmlReader _headerRdr;

        public void WriteXmlHeader()
        {
            _hsr = new StringReader(_header.ToString());
            _headerRdr = XmlReader.Create(_hsr, MyReaderSettings());
            Writing = true;
            Parse(true, _headerRdr);
            _headerRdr.Close();
            _hsr.Close();
        }

        private static XmlReaderSettings MyReaderSettings()
        {
            return new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore, XmlResolver = new NullResolver() };
        }
        #endregion Construct and capture header

        protected string GetString()
        {
            return _rdr.ReadElementString();
        }

        #region NextWriter
        protected void NextWriter(string xmlOutFullName)
        {
            NextWriter(xmlOutFullName, NoXmlHeader);
        }

        protected void NextWriter(string xmlOutFullName, bool noXmlHeader)
        {
            NoXmlHeader = noXmlHeader;
            var wrtSettings = new XmlWriterSettings { OmitXmlDeclaration = noXmlHeader };
            _writerfile = File.Create(xmlOutFullName);
            _wtr = XmlWriter.Create(_writerfile, wrtSettings);
            Writing = true;
        }
        #endregion NextWriter

        #region Flow Control
        public bool EndOfFile()
        {
            return _rdr.EOF;
        }

        protected void Finished()
        {
            _finish = true;
        }

        protected long CurrentSize()
        {
            return _writerfile.Length;
        }

        public void Close()
        {
			_rdr.Close();
            _sr.Close();
        }
        #endregion Flow Control

        #region Parsing...
        public void Parse()
        {
            Parse(false, _rdr);
        }

        public void Parse(bool inHeader, XmlReader r)
        {
            _finish = SkipNode = SkipAttr = false;
            while (r.Read())
            {
                SkipAttr = SkipNode;
                BeforeProcessMethods(r);
                if (_finish)
                    break;
                switch ( r.NodeType )
                {
                    case XmlNodeType.Element:
                        if (!SkipNode)
                        {
                            if (DebugPrint)
                            {
                                var errClass = r.GetAttribute("class");
                                Debug.Print("start " + (Writing ? "writing " : "reading ") + r.LocalName + "." + (!string.IsNullOrEmpty(errClass)?errClass:""));
                            }
                            if (string.IsNullOrEmpty(ReplaceLocalName))
                            {
                                if (Writing)
                                {
                                    _wtr.WriteStartElement(r.Prefix, r.LocalName, r.NamespaceURI);
                                }
                            }
                            else
                            {
                                if (Writing)
                                {
                                    _wtr.WriteStartElement(r.Prefix, ReplaceLocalName, r.NamespaceURI);
                                }
                                ReplaceLocalName = null;
                            }
                        }
                        else
                        {
                            SkipAttr = true;
                        }
                        var empty = r.IsEmptyElement;
                        var depth = r.Depth;
                        var name = r.Name;
                        if (_doAttributes)
                        {
                            for (int attrIndex = r.AttributeCount; attrIndex > 0; attrIndex--)
                            {
                                r.MoveToNextAttribute();
                                SkipAttr = SkipNode;
                                BeforeProcessMethods(r);
                                if (!SkipAttr)
                                {
									if (r.Name.Contains(":"))
                                    {
                                        if (Writing)
                                        {
											var parts = r.Name.Split(':');
											if (parts[0] == "xmlns")
											{
												ns[parts[1]] = r.Value;
											}
											_wtr.WriteAttributeString(parts[0], parts[1], ns[parts[0]], r.Value);
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(ReplaceAttrValue))
                                        {
                                            if (Writing)
                                            {
                                                _wtr.WriteAttributeString(r.Name, r.Value);
                                            }
                                        }
                                        else
                                        {
                                            if (Writing)
                                            {
                                                _wtr.WriteAttributeString(r.Name, ReplaceAttrValue);
                                            }
                                            ReplaceAttrValue = null;
                                        }
                                    }
                                }
                                AfterProcessMethods(r);
                            }
                            SkipAttr = false;
                        }
                        else
                        {
                            if (!SkipAttr)
                            {
                                if (Writing)
                                {
                                    _wtr.WriteAttributes(r, true);
                                }
                            }
                        }
                        if (inHeader && name == "body") return;
                        FirstChildProcessMethods(XmlNodeType.Element);
                        if (empty)
                        {
                            BeforeEndProcessMethods(XmlNodeType.EndElement, depth, name);
                            if (name == "title" && !string.IsNullOrEmpty(TitleDefault))
                            {
                                InsertTitle();
                            }
                            else
                            {
                                if (!SkipNode)
                                {
                                    if (Writing)
                                    {
                                        _wtr.WriteEndElement();
                                    }
                                }
                            }
                        }
                        break;
                    case XmlNodeType.Text:
                        if (!SkipNode)
                        {
                            if (Writing)
                            {
                                _wtr.WriteString(r.Value);
                            }
                        }
                        break;
                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                        //Debug.Print("space");
                        if (Writing)
                        {
                            _wtr.WriteWhitespace(r.Value);
                        }
                        break;
                    case XmlNodeType.CDATA:
                        if (Writing)
                        {
                            _wtr.WriteCData(r.Value);
                        }
                        break;
                    case XmlNodeType.EntityReference:
                        if (Writing)
                        {
                            _wtr.WriteEntityRef(r.Name);
                        }
                        break;
                    case XmlNodeType.XmlDeclaration:
                        if (!NoXmlHeader)
                        {
                            if (Writing)
                            {
                                _wtr.WriteProcessingInstruction(r.Name, r.Value);
                                _wtr.WriteDocType("html", "-//W3C//DTD XHTML 1.1//EN", "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", @"
<!ATTLIST html lang CDATA #REQUIRED >
<!ATTLIST span lang CDATA #IMPLIED >
<!ATTLIST span entryguid CDATA #IMPLIED >
<!ATTLIST img alt CDATA #IMPLIED >
");
                            }
                        }
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        if (Writing)
                        {
                            _wtr.WriteProcessingInstruction(r.Name, r.Value);
                        }
                        break;
                    case XmlNodeType.DocumentType: // This code will never execute with DtdProcessing.Ignore (see instantiation)
                        break;
                    case XmlNodeType.Comment:
                        if (Writing)
                        {
                            _wtr.WriteComment(r.Value);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        //Debug.Print("End " + r.Name);
                        BeforeEndProcessMethods(r.NodeType, r.Depth, r.Name);
                        if (!SkipNode)
                        {
                            if (Writing)
                            {
                                try
                                {
                                    _wtr.WriteFullEndElement();
                                }
                                catch (InvalidOperationException)
                                {
                                    // We have already written enough end elements
                                }
                            }
                        }
                        break;
                }
                AfterProcessMethods(r);
                if (_finish)
                    break;
            }
            for (var depth = r.Depth; depth > 0; depth--)
            {
                BeforeEndProcessMethods(XmlNodeType.EndElement, r.Depth, r.Name);
                if (Writing)
                {
                    try
                    {
                        _wtr.WriteFullEndElement();
                    }
                    catch (InvalidOperationException)
                    {
                        // We have already written enough end elements
                    }
                }
            }
            if (Writing)
            {
                _wtr.Close();
                _writerfile.Close();
            }
            Writing = false;
        }
        #endregion Parsing...

        #region Insert while parsing...
        private void InsertTitle()
        {
            _wtr.WriteString(TitleDefault);
            _wtr.WriteEndElement();
            if (!string.IsNullOrEmpty(AuthorDefault))
            {
                InsertAuthor();
            }
        }

        private void InsertAuthor()
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
        #endregion Insert while parsing...

        #region Callbacks
        private void BeforeProcessMethods(XmlReader r)
        {
            if (!_beforeNodeTypeMap.ContainsKey(r.NodeType)) return;
            foreach (ParserMethod func in _beforeNodeTypeMap[r.NodeType])
            {
                Debug.Assert(func != null, "func != null");
                func(r);
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

        private void AfterProcessMethods(XmlReader r)
        {
            if (!_afterNodeTypeMap.ContainsKey(r.NodeType)) return;
            foreach (ParserMethod func in _afterNodeTypeMap[r.NodeType])
            {
                Debug.Assert(func != null, "func != null");
                func(r);
                if (_finish)
                    break;
            }
        }
        #endregion Callbacks

        #region Letter and Book headers
        protected void WriteLetterHeader(string letterLang, string letter, XmlReader r)
        {
            _wtr.WriteStartElement("", "div", r.NamespaceURI);
            _wtr.WriteAttributeString("class", "letHead");
            _wtr.WriteStartElement("", "span", r.NamespaceURI);
            _wtr.WriteAttributeString("class", "letter");
            _wtr.WriteAttributeString("lang", letterLang);
            _wtr.WriteValue(letter);
            _wtr.WriteEndElement();
            _wtr.WriteEndElement();
        }
		#endregion Letter header

		#region Embedded styles
		protected void WriteEmbddedStyle(string val)
        {
            _wtr.WriteStartElement("style", "http://www.w3.org/1999/xhtml");
            _wtr.WriteAttributeString("type", "text/css");
            _wtr.WriteRaw(val);
            _wtr.WriteEndElement();
        }
        #endregion Embedded styles

        #region Writing Content
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
                WriteClassAttr(myClass + Suffix);
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
        #endregion Writing Content

        #region Callback setup
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
        #endregion Callback setup
    }
}
