// --------------------------------------------------------------------------------------------
// <copyright file="SfmToUsx.cs" from='2009' to='2014' company='SIL International'>
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
	public class SfmToUsx
	{
		#region Private Variable

		private string _usxFullPath, _sfmFullPath;

		private XmlTextWriter _writer;
		private StreamReader _sfmFile;
		private short _noTagsOpen = 0;
		private short _noParaOpen = 0;
		private bool _noParaContent = false;
		private bool _entryVerse = false;

		private Dictionary<string, Dictionary<string, string>> _styleInfo =
			new Dictionary<string, Dictionary<string, string>>();
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

		/// <summary>
		/// Converter used by PathwayB ... called by reflection
		/// </summary>
		// ReSharper disable UnusedMember.Global
		public void ConvertSfmToUsx(XmlTextWriter xmlw, string sfmFullPath, string usxFullPath)
		// ReSharper restore UnusedMember.Global
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
			_writer.WriteString("\r\n");
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
			string[] parse = line.Split('\\');

			foreach (string node in parse)
			{
				if (node.Trim().Length == 0) continue;

				string style = Common.LeftString(node, " ");
				string content = Common.LeftRemove(node, style).Trim();

				if (style.Contains("x*") || style.Contains("f*"))
				{
					style = Common.LeftString(node, "*") + "*";
					content = Common.LeftRemove(node, style).Trim();
				}

				if (!line.Substring(0, 1).Contains("\\"))
				{
					if (node.Length > 0)
					{
						_writer.WriteString(node);
					}
					continue;
				}

				switch (style)
				{
					case "id":
						Book(style, content);
						break;
					case "h":
						Para(style, content);
						break;
					case "f":
					case "x":
						Note(style, content);
						break;
					case "f*":
					case "x*":
						NoteWriteEndElement(content);
						break;
					case "fr":
					case "xo":
					case "ft":
					case "xt":
						Char(style, content);
						break;
					case "c":
						Chapter(style, content);
						break;
					case "p":
					case "q":
					case "q2":
						ParaVerse(style, content);
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

		private void NoteWriteEndElement(string content)
		{
			_writer.WriteEndElement();
			if (content.Length > 0)
			{
				_writer.WriteString(content);
			}
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

		private void Char(string style, string content)
		{
			_writer.WriteStartElement("char");
			_writer.WriteAttributeString("style", style);
			_writer.WriteAttributeString("closed", "false");
			if (content.Length > 0)
			{
				_writer.WriteString(content);
			}
			_writer.WriteEndElement();
		}

		private void Note(string style, string content)
		{
			string number = Common.LeftString(content, " ");
			string newcontent = Common.RightString(content, " ");

			_writer.WriteStartElement("note");
			_writer.WriteAttributeString("caller", number);
			_writer.WriteAttributeString("style", style);
			if (newcontent.Length > 0)
			{
				if (newcontent == "*")
				{
					return;
				}
				_writer.WriteString(newcontent);
			}
		}

		private void ParaVerse(string style, string content)
		{
			if (_noParaContent && _entryVerse)
			{
				for (int i = 1; i <= _noParaOpen; i++)
				{
					_writer.WriteEndElement();
				}
				_noParaOpen = 0;
				_noParaContent = false;
				_entryVerse = false;
			}

			if (_noParaContent)
			{
				for (int i = 1; i <= _noParaOpen; i++)
				{
					_writer.WriteEndElement();
				}
				_noParaOpen = 0;
				_noParaContent = false;
			}
			_writer.WriteString("\r\n");
			_writer.WriteStartElement("para");
			_writer.WriteAttributeString("style", style);
			_noParaOpen++;
			_noParaContent = true;
			if (content.Trim().Length > 0)
			{
				_writer.WriteString(content);
			}
		}

		private void Chapter(string style, string content)
		{
			if (_noParaContent && _entryVerse)
			{
				for (int i = 1; i <= _noParaOpen; i++)
				{
					_writer.WriteEndElement();
				}
				_noParaOpen = 0;
				_noParaContent = false;
				_entryVerse = false;
			}

			if (_noParaContent)
			{
				for (int i = 1; i <= _noParaOpen; i++)
				{
					_writer.WriteEndElement();
				}
				_noParaOpen = 0;
				_noParaContent = false;
			}
			_writer.WriteString("\r\n");
			_writer.WriteStartElement("chapter");
			_writer.WriteAttributeString("number", content);
			_writer.WriteAttributeString("style", style);
			_writer.WriteEndElement();

		}

		/// <summary>
		/// input:  <verse number="1" style="v">abc </verse>
		/// output: \v 1 abc
		/// </summary>
		private void Verse(string style, string content)
		{
			//string number = Common.LeftString(content, " ");
			//content = Common.RightString(content, " ");

			string number = Common.LeftString(content, " ");
			content = Common.LeftRemove(content, number).Trim();

			_writer.WriteStartElement("verse");
			_writer.WriteAttributeString("number", number);
			_writer.WriteAttributeString("style", style);
			_writer.WriteEndElement();
			if (content.Length > 1)
			{
				_entryVerse = true;
				_writer.WriteString(content);
			}
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
			else if (content.Length > 0)
			{
				if (_noParaContent && _entryVerse)
				{
					for (int i = 1; i <= _noParaOpen; i++)
					{
						_writer.WriteEndElement();
					}
					_noParaOpen = 0;
					_noParaContent = false;
					_entryVerse = false;
				}
				if (_noParaContent)
				{
					for (int i = 1; i <= _noParaOpen; i++)
					{
						_writer.WriteEndElement();
					}
					_noParaOpen = 0;
					_noParaContent = false;
				}
				_writer.WriteString("\r\n");
				_writer.WriteStartElement("para");
				_writer.WriteAttributeString("style", style);
				_noTagsOpen++;
				_writer.WriteString(content);
			}
		}

		private void CloseTag()
		{
			for (int i = 1; i <= _noTagsOpen; i++)
			{
				_writer.WriteEndElement();
			}
			_noTagsOpen = 0;
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
			_writer.WriteAttributeString("ref", loc);
			if (refer.Length > 0)
			{
				_writer.WriteString(refer);
			}
			_writer.WriteEndElement();
		}

		private void Book(string style, string content)
		{
			string number = Common.LeftString(content, " ");
			content = Common.RightString(content, " ");

			_writer.WriteString("\r\n");
			_writer.WriteStartElement("book");
			_writer.WriteAttributeString("code", number);
			_writer.WriteAttributeString("style", style);
			if (content.Length > 0)
			{
				_writer.WriteString(content);
			}
			_writer.WriteEndElement();
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
			_writer.WriteStartElement("usx");
			_writer.WriteAttributeString("version", "2.0");
			_sfmFile = new StreamReader(_sfmFullPath, true);
		}

		/// <summary>
		/// Open usx and sfm file (Used by 3 argument constructor)
		/// </summary>
		private void OpenFileDirectText()
		{
			_writer.WriteStartElement("usx");
			_writer.WriteAttributeString("version", "2.0");
			_sfmFile = new StreamReader(_sfmFullPath, true);
		}
	}
}