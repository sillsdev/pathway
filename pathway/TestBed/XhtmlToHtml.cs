// --------------------------------------------------------------------------------------------
// <copyright file="XhtmlToHtml.cs" from='2009' to='2014' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace SIL.Tool
{
    public class XhtmlToHtml
    {
        private XmlTextReader _reader;
        private XmlTextWriter _writer;
        private bool _isNewBook = false;
        bool _headXML = true;
        private int _bookNumber = 1;
        private string _fileDir;
        private string _tagName;
        public Dictionary<int, string> _indexPage = new Dictionary<int, string>();

        /// <summary>
        /// Convert XHTML file to HTML file
        /// </summary>
        /// <param name="xhtmlFile">Xhtml file path</param>
        public void Convert(string xhtmlFile)
        {
            OpenFile(xhtmlFile);
            ProcessXhtml();
            CreateIndexPage();
        }

        private void ProcessXhtml()
        {
            while (_reader.Read())
            {
                switch (_reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (_headXML) // skip previous parts of <body> tag
                        {
                            if (_reader.Name == "body")
                            {
                                _headXML = false;
                                continue;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        StartElement();
                        break;

                    case XmlNodeType.Text:
                        Write(); 
                        break;

                    case XmlNodeType.Whitespace:

                    case XmlNodeType.SignificantWhitespace:

                        _writer.WriteWhitespace(_reader.Value);

                        break;

                    case XmlNodeType.CDATA:

                        _writer.WriteCData(_reader.Value);

                        break;

                    case XmlNodeType.EntityReference:

                        _writer.WriteEntityRef(_reader.Name);

                        break;

                    case XmlNodeType.XmlDeclaration:

                    case XmlNodeType.ProcessingInstruction:

                        _writer.WriteProcessingInstruction(_reader.Name, _reader.Value);

                        break;

                    case XmlNodeType.DocumentType:

                        _writer.WriteDocType(_reader.Name, _reader.GetAttribute("PUBLIC"), _reader.GetAttribute("SYSTEM"), _reader.Value);

                        break;

                    case XmlNodeType.Comment:

                        _writer.WriteComment(_reader.Value);

                        break;

                    case XmlNodeType.EndElement:
                        EndElement(); 
                        break;

                }
            }
            _writer.Close();
            _reader.Close();
        }

        private void StartElement()
        {
            string className = GetClassName();

            if (className == "scrBook" || className == "scrBookName")
            {
                _isNewBook = true;
                return;
            }


            _writer.WriteStartElement(_tagName);
            _writer.WriteAttributes(_reader, true);

        }

        private string GetClassName()
        {
            string className = string.Empty;
            _tagName = _reader.Name;
            if (_reader.HasAttributes)
            {
                while (_reader.MoveToNextAttribute())
                {
                    if (_reader.Name == "class")
                    {
                        className = _reader.Value;
                    }
                }
            }
            return className;
        }

        private void Write()
        {
            string content = _reader.Value;
            if (_isNewBook)
            {
                CloseBook();
                CreateBook(content);
                _isNewBook = false;
                return;
            }

            _writer.WriteString(content);


        }

        private void EndElement()
        {
            if (_headXML) // skip previous parts of <body> tag
            {
                return;
            }
            if (!_isNewBook)
            {
                _writer.WriteEndElement();
            }
        }

        private void CreateBook(string title)
        {
            string newBook = Common.PathCombine(_fileDir, _bookNumber.ToString() + ".html");
            _writer = new XmlTextWriter(newBook, null);
            _writer.WriteStartElement("html");
                _writer.WriteStartElement("head");
                    _writer.WriteStartElement("meta");
                    _writer.WriteAttributeString("charset", "utf-8");
                    _writer.WriteEndElement();

                    _writer.WriteStartElement("link");
                    _writer.WriteAttributeString("rel", "stylesheet");
                    _writer.WriteAttributeString("type", "text/css");
                    _writer.WriteAttributeString("href", "../chapter.css");
                    _writer.WriteEndElement();
                    _writer.WriteStartElement("title");
                    _writer.WriteString(title);
                    _writer.WriteEndElement();
                _writer.WriteEndElement();

            _writer.WriteStartElement("body");
            _writer.WriteAttributeString("class", "index chapter");
                _writer.WriteStartElement("h1");
                _writer.WriteAttributeString("class", "index_title");
                _writer.WriteString(title);
                _writer.WriteEndElement();
            _writer.WriteStartElement("p");
                _writer.WriteStartElement("a");
                _writer.WriteAttributeString("href", "index.html");
                _writer.WriteString("Up");
                _writer.WriteEndElement();
            _writer.WriteStartElement("div");
            _writer.WriteAttributeString("class", "chapter_heading");
            _writer.WriteString(_bookNumber.ToString());

            _indexPage[_bookNumber] = title + ".html";
            _bookNumber++;
        }


        private void CreateIndexPage()
        {
            string newBook = Common.PathCombine(_fileDir,"index.html");
            _writer = new XmlTextWriter(newBook, null);
            _writer.WriteStartElement("html");
            _writer.WriteStartElement("head");
            _writer.WriteStartElement("meta");
            _writer.WriteAttributeString("charset", "utf-8");
            _writer.WriteEndElement();
            _writer.WriteStartElement("title");
            _writer.WriteString("1 Corintios");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("body");
            _writer.WriteAttributeString("class", "index book");
            _writer.WriteStartElement("h1");
            _writer.WriteAttributeString("class", "index_title");
            _writer.WriteString("1 Corintios");
            _writer.WriteEndElement();
            _writer.WriteStartElement("p");
            _writer.WriteStartElement("a");
            _writer.WriteAttributeString("href", "../index.html");
            _writer.WriteString("Up");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("p");
            for(int i= 1;i < _bookNumber; i++)
            {
                _writer.WriteStartElement("a");
                _writer.WriteAttributeString("href", i.ToString()+".html");
                _writer.WriteString(i.ToString());
                _writer.WriteEndElement();
                _writer.WriteString(" ");
            }
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.Close();
        }

        private void CloseBook()
        {
            if (_bookNumber>1)
            {
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.Close();
            }
        }

        private void OpenFile(string filePath)
        {
            XmlTextReader _reader = Common.DeclareXmlTextReader(filePath, true);
            _fileDir = Path.GetDirectoryName(filePath);
        }
    }
}
