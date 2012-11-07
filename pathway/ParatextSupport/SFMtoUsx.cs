using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
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
            string word = Common.LeftString(line, " ");

            switch (word)
            {
                case "\\id":
                    Book(line);
                    break;
                case "\\h":
                    Para(line, word);
                    break;
                case "\\c":
                    Chapter(line);
                    break;
                case "\\v":
                    Verse(line, word);
                    break;
                default:
                    Para(line, word);
                    break;
            }
        }

        /// <summary>
        /// input:  <verse number="1" style="v" />abc </verse>
        /// output: \v 1 abc
        /// </summary>
        private void Verse(string line, string word)
        {
            word = word.Replace("\\", "");
            string data = Common.RightString(line, " ");
            string number = Common.LeftString(data, " ");
            data = Common.RightString(data, " ");

            _writer.WriteStartElement("verse");
            _writer.WriteAttributeString("number", number);
            _writer.WriteAttributeString("style", word);

            data = WriteFigure(data);
            if (data.Length > 0)
            {
                _writer.WriteString(data);
            }
            _writer.WriteEndElement();

        }



        /// <summary>
        /// Collect Figure Tag Information
        /// <figure style="fig" desc="Map of Israel and Moab during the time of Naomi and Ruth"
        /// file="Ruth-CS4_BW_ver2.pdf" size="col" loc="" copy="" ref="RUT 1.0">
        /// </summary>
        private string WriteFigure(string data)
        {

            int start,end;
            string sourcePart, extractPart;
            FindStartEnd(data, "\\fig", "\\fig*", out start, out end);

            if (start < 0 || end < 0)
            {
                return data;
            }
            BreakNode(data, start, end, out sourcePart, out extractPart);
            if (data.Length > 0)
            {
                _writer.WriteString(sourcePart);
                sourcePart = string.Empty;
            }

            extractPart = extractPart.Replace("\\fig*", "");
            extractPart = extractPart.Replace("\\fig", "");

            string[] fig = extractPart.Split('|');

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
            return sourcePart;
        }

        private void Para(string line, string word)
        {
            word = word.Replace("\\", "");
            string data = Common.RightString(line, " ");
            _writer.WriteStartElement("para");
            _writer.WriteAttributeString("style", word);
            if (data.Length > 0)
            {
                _writer.WriteString(data);
            }
            _writer.WriteEndElement();

        }

        private void Chapter(string line)
        {
            string data = Common.RightString(line, " ");
            _writer.WriteStartElement("chapter");
            _writer.WriteAttributeString("number", data);
            _writer.WriteAttributeString("style", "c");
            _writer.WriteEndElement();

        }

        private void Book(string line)
        {
            string data = Common.RightString(line, " ");
            _writer.WriteStartElement("book");
            _writer.WriteAttributeString("code", data);
            _writer.WriteAttributeString("style", "id");
            _writer.WriteString(data);
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
            //_writer.WriteAttributeString("version", "2.0");

            _sfmFile = new StreamReader(_sfmFullPath,true);
        }
    }
}