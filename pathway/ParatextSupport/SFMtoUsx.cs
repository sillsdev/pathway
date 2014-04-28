// --------------------------------------------------------------------------------------------
// <copyright file="SFMtoUsx.cs" from='2009' to='2014' company='SIL International'>
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
// Convert SFM to USX format
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class SFMtoUsx
    {
        #region Private Variable

        private Stack<string> _allStyle = new Stack<string>();
        private Stack<string> _alltagName = new Stack<string>();

        private string _usxFullPath, _sfmFullPath;

        private XmlTextWriter _writer;
        private StreamReader _sfmFile;
        private short _noTagsOpen=0;

        private Dictionary<string, Dictionary<string, string>> _styleInfo =
            new Dictionary<string, Dictionary<string, string>>();

        private Dictionary<string, string> _cssProp;
        private Dictionary<string, string> _mapClassName = new Dictionary<string, string>();

        private string _tagName, _style, _number, _code, _caller, _content;
        private string _parentTagName = string.Empty, _parentStyleName = string.Empty;

        private string _desc, _file, _size, _loc, _copy, _ref;
        private bool _significant, _isEmptyNode, _isParaWritten;

        private const string Space = " ";
        private const string Bar = "|";

        private bool _isclassNameExist;
        private List<string> _xhtmlAttribute = new List<string>();


        #endregion

        /// <summary>
        /// Entry method to convert USX to SFM
        /// sty file is already set for this class.
        /// </summary>
        public void ConvertSFMtoUsx(string sfmFullPath, string usxFullPath)
        {
            _sfmFullPath = sfmFullPath;
            _usxFullPath = usxFullPath;
            OpenFile();
            ProcessSFM();
        }

        public void ConvertSFMtoUsx(XmlTextWriter xmlw, string sfmFullPath, string usxFullPath)
        {
            _writer = xmlw;
            _sfmFullPath = sfmFullPath;
            _usxFullPath = usxFullPath;
            OpenFileDirectText();
            ProcessSFM();
        }

        /// ------------------------------------------------------------------------
        /// <summary>
        /// Parses all the lines in an sty file converting the settings to 
        /// properties in a CSS.
        /// </summary>
        /// ------------------------------------------------------------------------
        private void ProcessSFM()
        {
            _styleInfo.Clear();
            if (!File.Exists(_sfmFullPath))
            {
                Debug.WriteLine(_sfmFullPath + " does not exist.");
                return;
            }

            string line;
            while ((line = _sfmFile.ReadLine()) != null)
            {
                ParseLine(line);
            }
            _sfmFile.Close();
            _writer.WriteEndElement();
            _writer.Flush();
            _writer.Close();
        }

        /// ------------------------------------------------------------
        /// <summary>
        /// Parses a line in an Paratext sty file.
        /// </summary>
        /// <param name="line">The line in a Paratext sty file.</param>
        /// ------------------------------------------------------------
        private void ParseLine(string line)
        {
            string value;
            string[] parse = line.Split('\\');
            foreach (string node in parse)
            {
                if (node.Trim().Length == 0) continue;

                string style = Common.LeftString(node, " ");
                string content = Common.LeftRemove(node, style).Trim();
                switch (style)
                {
                    case "id":
                        Book(style,content);
                        break;
                    case "h":
                        Para(style, content);
                        break;
                    case "c":
                        Chapter(style, content);
                        break;
                    case "v":
                        Verse(style, content);
                        break;
                    case "fig":
                        Figure(style, content);
                        break;
                    default:
                        Other(style, content);
                        break;
                }
            }
            CloseTag();

        }
        
        private void Para(string style, string content)
        {
            _writer.WriteStartElement("para");
            _writer.WriteAttributeString("style", style);
            if (content.Length > 0)
            {
                _writer.WriteString(content);
            }
            _writer.WriteEndElement();

        }

        private void Other(string style, string content)
        {

            if (style.Trim().Length == 0)
            {
                if (content.Length > 0)
                {
                    _writer.WriteString(content);
                }
            }
            else if (style.IndexOf('*') > 0)
            {
                _writer.WriteEndElement();
                _noTagsOpen--;
                if (content.Length > 0)
                {
                    _writer.WriteString(content);
                }
            }
            else
            {
                _writer.WriteStartElement("para");
                _writer.WriteAttributeString("style", style);
                _noTagsOpen++;
                if (content.Length > 0)
                {
                    _writer.WriteString(content);
                }
            }
        }

        private void CloseTag()
        {
            for(int i=1;i<= _noTagsOpen; i++ )
            {
                _writer.WriteEndElement();
            }
            _noTagsOpen = 0;
        }


        /// <summary>
        /// input:  <verse number="1" style="v">abc </verse>
        /// output: \v 1 abc
        /// </summary>
        private void Verse(string style, string content)
        {
            string number = Common.LeftString(content, " ");
            content = Common.RightString(content, " ");

            _writer.WriteStartElement("verse");
            _writer.WriteAttributeString("number", number);
            _writer.WriteAttributeString("style", style);

            if (content.Length > 0)
            {
                _writer.WriteString(content);
            }
            _noTagsOpen++;

        }

        /// <summary>
        /// Collect Figure Tag Information
        /// <figure style="fig" desc="Map of Israel and Moab during the time of Naomi and Ruth"
        /// file="Ruth-CS4_BW_ver2.pdf" size="col" loc="" copy="" ref="RUT 1.0"/>
        /// </summary>
        private void Figure(string style, string content)
        {
            string[] fig = content.Split('|');

            string desc = fig[0];
            string file = fig[1];
            string size = fig[2];
            string loc = fig[3];
            string copy = fig[4];
            string refer = fig[5];
            _writer.WriteStartElement("figure");
            _writer.WriteAttributeString("style", "fig");
            _writer.WriteAttributeString("desc", desc);
            _writer.WriteAttributeString("file", file);
            _writer.WriteAttributeString("size", size);
            _writer.WriteAttributeString("loc", loc);
            _writer.WriteAttributeString("copy", copy);
            _writer.WriteAttributeString("ref", refer);
            _writer.WriteEndElement();
        }

        private void Chapter(string style, string content)
        {
            _writer.WriteStartElement("chapter");
            _writer.WriteAttributeString("number", content);
            _writer.WriteAttributeString("style", style);
            _writer.WriteEndElement();

        }

        private void Book(string style, string content)
        {
            _writer.WriteStartElement("book");
            _writer.WriteAttributeString("code", content);
            _writer.WriteAttributeString("style", style);
            _writer.WriteString(content);
            _writer.WriteEndElement();

        }

        private void FindStartEnd(string data,string startStr,string endStr, out int start,out int end)
        {
            start = data.IndexOf(startStr);
            end = data.IndexOf(endStr) + endStr.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">This is Pathway</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="sourcePart"></param>
        /// <param name="extractPart"></param>
        private void BreakNode(string data, int start, int end, out string sourcePart,out string extractPart)
        {

            extractPart = data.Substring(start, end - start );
            sourcePart = data.Remove(start, end - start);
        }


        private string StackPop(Stack<string> stack)
        {
            string result = string.Empty;
            if (stack.Count > 0)
            {
                result = stack.Pop();
            }
            return result;
        }

        private string StackPeek(Stack<string> stack)
        {
            string result = string.Empty;
            if (stack.Count > 0)
            {
                result = stack.Peek();
            }
            return result;
        }

        /// <summary>
        /// Open usx and sfm file
        /// </summary>
        private void OpenFile()
        {
            _writer = new XmlTextWriter(_usxFullPath, null)
            {
                Formatting = Formatting.Indented
            };
            _writer.WriteStartDocument();
            _writer.WriteStartElement("USX");
            _sfmFile = new StreamReader(_sfmFullPath,true);
        }

        /// <summary>
        /// Open usx and sfm file
        /// </summary>
        private void OpenFileDirectText()
        {
            _writer.WriteStartDocument();
            _writer.WriteStartElement("usx");
            _sfmFile = new StreamReader(_sfmFullPath, true);
        }
    }
}