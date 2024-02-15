// ---------------------------------------------------------------------------------------------

#region // Copyright (c) 2016, SIL International. All Rights Reserved.

// <copyright from='2016' to='2017' company='SIL International'>
//		Copyright (c) 2017, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>

#endregion

//
// File: program.cs (from CssSimpler.cs)
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;
using SIL.PublishingSolution;
using Antlr.Runtime.Tree;
using SIL.Tool;

namespace CssSimpler
{
	public class Program
	{
		private static bool _showHelp;
		protected static bool OutputXml;
		private static int _verbosity;
		private static bool _makeBackup;
		private static bool _flatten;
		private static int _headerStyles;
		private static bool _embedStyles;
		private static bool _incMeta;
		private static bool _noXmlHeader;
		private static bool _divBlocks;
		private static string _decorateStyles;

		protected static readonly XslCompiledTransform XmlCss = new XslCompiledTransform();
		protected static readonly XslCompiledTransform SimplifyXhtml = new XslCompiledTransform();
		protected static List<string> UniqueClasses;
		public static bool ReportStatus;

		private static readonly XmlReaderSettings ReaderSettings = new XmlReaderSettings
		{
			XmlResolver = new NullResolver(),
			DtdProcessing = DtdProcessing.Ignore
		};

		private static readonly XsltSettings XsltSettings = new XsltSettings
		{
			EnableDocumentFunction = false,
			EnableScript = false
		};

		static void Main(string[] args)
		{
			// ReSharper disable AssignNullToNotNullAttribute
			XmlCss.Load(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"CssSimpler.XmlCss.xsl")));
			SimplifyXhtml.Load(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"CssSimpler.XhtmlSimplify.xsl"), ReaderSettings), XsltSettings, new NullResolver());
			// ReSharper enable AssignNullToNotNullAttribute
			// see: http://stackoverflow.com/questions/491595/best-way-to-parse-command-line-arguments-in-c
			var p = new OptionSet
			{
				{
					"x|xml", "produce XML output of CSS",
					v => OutputXml = !OutputXml
				},
				{
					"b|backup", "make a backup of the original CSS file",
					v => _makeBackup = !_makeBackup
				},
				{
					"v|verbose", "increase debug message verbosity",
					v =>
					{
						if (v != null) ++_verbosity;
					}
				},
				{
					"h|help", "show this message and exit",
					v => _showHelp = !_showHelp
				},
				{
					"f|flat", "flattens the hierarchy simplifies the css required for -secmrd",
					v => _flatten = !_flatten
				},
				{
					"s|structure", "Use header tags for structure (1-3)",
					v => _headerStyles += 1
				},
				{
					"e|embed", "embed styles in output",
					v => _embedStyles = !_embedStyles
				},
				//{
				//    "c|combine", "combine main and reversal(s) in a single output",
				//    v => _combineMainRev = !_combineMainRev
				//},
				{
					"m|meta", "include title and author meta data",
					v => _incMeta = !_incMeta
				},
				{
					"r|remove", "remove Xml header and DOCTYPE",
					v => _noXmlHeader = !_noXmlHeader
				},
				{
					"d|div", "replace span with div for display block",
					v => _divBlocks = !_divBlocks
				},
				{
					"p|prefix=", "prefix style names with value",
					v => { _decorateStyles = v; }
				},
				{
					"u|update", "report status",
					v => ReportStatus = !ReportStatus
				},
			};

			List<string> extra;
			try
			{
				extra = p.Parse(args);
				if (extra.Count == 0)
				{
					Console.WriteLine(@"Enter full file name to process");
					extra.Add(Console.ReadLine());
				}
			}
			catch (OptionException e)
			{
				Console.Write(@"SimpleCss: ");
				Console.WriteLine(e.Message);
				Console.WriteLine(@"Try `CssSimple --help' for more information.");
				return;
			}
			if (_showHelp || extra.Count != 1)
			{
				ShowHelp(p);
				return;
			}
			var lc = new LoadClasses(extra[0]);
			EntryReportInit();
			var styleSheet = lc.StyleSheet;
			MakeBackupIfNecessary(styleSheet, extra[0]);
			DebugWriteClassNames(lc.UniqueClasses);
			VerboseMessage("Clean up Stylesheet: {0}", styleSheet);
			var parser = new CssTreeParser();
			var xml = new XmlDocument();
			UniqueClasses = lc.UniqueClasses;
			if (!File.Exists(styleSheet))
			{
				var errName = styleSheet;
				var proposed = extra[0].Replace(".xhtml", ".css");
				styleSheet = proposed;
				if (!File.Exists(proposed))
				{
					proposed = proposed.Replace("_", "");
					styleSheet = proposed;
					if (!File.Exists(proposed))
					{
						proposed = Path.Combine(Path.GetDirectoryName(proposed), "main.css");
						if (!File.Exists(proposed)) throw new FileNotFoundException(errName);
						File.Copy(proposed, styleSheet, true);
					}
				}
			}
			LoadCssXml(parser, styleSheet, xml);
			var tmpXhtmlFullName = WriteSimpleXhtml(extra[0]);
			var tmp2Out = Path.GetTempFileName();
			// ReSharper disable once UnusedVariable
			var inlineStyle = new MoveInlineStyles(tmpXhtmlFullName, tmp2Out, styleSheet);
			xml.RemoveAll();
			UniqueClasses = null;
			LoadCssXml(parser, styleSheet, xml);
			// ReSharper disable once UnusedVariable
			var ps = new ProcessPseudo(tmp2Out, extra[0], xml);
			RemoveCssPseudo(styleSheet, xml);
			var tmp3Out = Path.GetTempFileName();
			if (_flatten)
			{
				var fs = new FlattenStyles(extra[0], tmp3Out, xml, _noXmlHeader, _decorateStyles);
				fs.Structure = _headerStyles;
				fs.DivBlocks = _divBlocks;
				MetaData(fs);
				fs.Parse();
				File.Copy(tmp3Out, extra[0], true);
				if (OutputXml) SavedXmlCssFileName = FlatXmlName(styleSheet);
				OutputFlattenedStylesheet(extra[0], styleSheet, fs);
			}
			try
			{
				File.Delete(tmpXhtmlFullName);
				File.Delete(tmp2Out);
				File.Delete(tmp3Out);
			}
			catch
			{
				// ignored
			}
		}

		private static string FlatXmlName(string styleSheet)
		{
			var xmlName = Path.GetFileNameWithoutExtension(styleSheet) + "Flat.xml";
			var folder = Path.GetDirectoryName(styleSheet);
			return !string.IsNullOrEmpty(folder) ? Path.Combine(folder, xmlName) : xmlName;
		}

		private static void EntryReportInit()
		{
			if (!ReportStatus) return;
			const int whenToReport = 500;
			var reportsPlanned = _entryCount*3/whenToReport;
			Console.WriteLine("mx{0}", reportsPlanned);
			System.Threading.Thread.Sleep(100);
			_nextEntryReport = _entryReportFreq = whenToReport;
			if (reportsPlanned <= 0 || _nextEntryReport <= 0) ReportStatus = false;
		}

		private static readonly List<string> EntryStarts = new List<string> {"entry", "minorentry", "reversalindexentry"};

		public static bool IsEntryClass(string myClass)
		{
			return EntryStarts.Any(myClass.StartsWith);
		}

		private static int _entryCount = 0;

		public static void EntryCounter(XmlReader r)
		{
			if (r.LocalName != "div") return;
			var myClass = r.GetAttribute("class");
			if (string.IsNullOrEmpty(myClass)) return;
			if (!IsEntryClass(myClass)) return;
			_entryCount += 1;
		}

		private static int _nextEntryReport = 0;
		private static int _entryReportFreq = 0;
		private static int _entryStatus = 0;

		public static void EntryReporter(XmlReader r)
		{
			if (!ReportStatus) return;
			if (r.LocalName != "div") return;
			var myClass = r.GetAttribute("class");
			if (string.IsNullOrEmpty(myClass)) return;
			if (!IsEntryClass(myClass)) return;
			_entryStatus += 1;
			if (_entryStatus > _nextEntryReport)
			{
				Console.WriteLine("st{0}", _entryStatus);
				System.Threading.Thread.Sleep(100);
				_nextEntryReport += _entryReportFreq;
			}
		}

		protected static void OutputFlattenedStylesheet(string outFullName, string styleSheet, FlattenStyles fs)
		{
			if (_embedStyles)
			{
				var ms = new MemoryStream();
				MapXmlCssToStream(fs.MakeFlatCss(), ms);
				ms.Seek(0, 0);
				var tmp4Out = Path.GetTempFileName();
				var es = new EmbedStyles(outFullName, tmp4Out, ms, _noXmlHeader);
				es.Parse();
				File.Copy(tmp4Out, outFullName, true);
				try
				{
					File.Delete(styleSheet);
					File.Delete(tmp4Out);
				}
				catch
				{
					// ignored
				}
			}
			else
			{
				WriteXmlAsCss(styleSheet, fs.MakeFlatCss());
			}
		}

		private static readonly XmlDocument SettingsXml = new XmlDocument();

		protected static void MetaData(FlattenStyles fs)
		{
			if (!_incMeta) return;
			var settingFullName = Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "SIL", "Pathway",
				"Dictionary", "DictionaryStyleSettings.xml");
			if (!File.Exists(settingFullName)) return;
			SettingsXml.Load(settingFullName);
			fs.TitleDefault = SetValue("Title");
			fs.AuthorDefault = SetValue("Creator");
		}


		private static string SetValue(string cls)
		{
			var valNode = SettingsXml.SelectSingleNode(string.Format("//Metadata/*[@name='{0}']/currentValue/text()", cls));
			if (valNode == null || string.IsNullOrEmpty(valNode.InnerText))
			{
				valNode = SettingsXml.SelectSingleNode(string.Format("//Metadata/*[@name='{0}']/defaultValue/text()", cls));
			}
			if (valNode != null && !string.IsNullOrEmpty(valNode.InnerText))
			{
				var val = valNode.InnerText;
				var pos = 0;
				var sb = new StringBuilder();
				foreach (Match match in Regex.Matches(val, @"_x([0-9A-F]{4})_"))
				{
					sb.Append(val.Substring(pos, match.Index - pos));
					sb.Append(Convert.ToChar(Convert.ToUInt32(match.Groups[1].Value, 16)));
					pos = match.Index + match.Length;
				}
				if (pos < val.Length)
				{
					sb.Append(val.Substring(pos));
				}
				return sb.ToString();
			}
			return null;
		}

		protected static void LoadCssXml(CssTreeParser parser, string styleSheet, XmlDocument xml)
		{
			ParseCssRemovingErrors(parser, styleSheet);
			var r = parser.Root;
			xml.LoadXml("<ROOT/>");
			AddSubTree(xml.DocumentElement, r, parser);
			ElaborateMultiSelectorRules(xml);
			if (!OutputXml) return;
			VerboseMessage("Writing XML stylesheet");
			WriteCssXml(styleSheet, xml);
		}

		protected static void ElaborateMultiSelectorRules(XmlDocument xml)
		{
			var emptyRules = xml.SelectNodes("//RULE[count(@term)=0]");
			Debug.Assert(emptyRules != null, "emptyRules != null");
			foreach (XmlElement emptyRule in emptyRules)
			{
				Debug.Assert(xml.DocumentElement != null, "xml.DocumentElement != null");
				xml.DocumentElement.RemoveChild(emptyRule);
			}
			var multiSelectors = xml.SelectNodes("//RULE/*[.=',']");
			Debug.Assert(multiSelectors != null, "multiSelectors != null");
			foreach (XmlElement multiSelector in multiSelectors)
			{
				var ruleNode = xml.CreateElement("RULE");
				var node = multiSelector;
				var targetClass = string.Empty;
				var lastClass = string.Empty;
				while (string.IsNullOrEmpty(targetClass) || string.IsNullOrEmpty(lastClass))
				{
					node = node.PreviousSibling as XmlElement;
					if (node == null) break;
					if (targetClass == string.Empty)
					{
						if (node.Name == "CLASS" || node.Name == "TAG")
						{
							targetClass = node.FirstChild.InnerText;
						}
					}
					if (!string.IsNullOrEmpty(lastClass) || node.Name != "CLASS") continue;
					var poposedClass = node.FirstChild.InnerText;
					lastClass = poposedClass;
				}
				ruleNode.Attributes.Append(xml.CreateAttribute("term"));
				ruleNode.Attributes.Append(xml.CreateAttribute("pos"));
				if (!string.IsNullOrEmpty(lastClass))
				{
					var lastClassAttr = xml.CreateAttribute("lastClass");
					lastClassAttr.InnerText = lastClass;
					ruleNode.Attributes.Append(lastClassAttr);
				}
				if (!string.IsNullOrEmpty(targetClass))
				{
					var targetAttr = xml.CreateAttribute("target");
					targetAttr.InnerText = targetClass;
					ruleNode.Attributes.Append(targetAttr);
				}
				node = multiSelector;
				while (true)
				{
					node = node.NextSibling as XmlElement;
					if (node == null) break;
					if (node.Name != "PROPERTY") continue;
					ruleNode.AppendChild(node.CloneNode(true));
				}
				node = multiSelector.PreviousSibling as XmlElement;
				while (true)
				{
					if (node == null) break;
					var previousNode = node.PreviousSibling as XmlElement;
					ruleNode.InsertBefore(node, ruleNode.FirstChild);
					node = previousNode;
				}
				ruleNode.Attributes["term"].InnerText = TermNodes(ruleNode).Count.ToString();
				var refRule = multiSelector.ParentNode as XmlElement;
				Debug.Assert(refRule != null, "refRule != null");
				refRule.Attributes["term"].InnerText = TermNodes(refRule).Count.ToString();
				Debug.Assert(xml.DocumentElement != null, "xml.DocumentElement != null");
				xml.DocumentElement.InsertBefore(ruleNode, refRule);
				Debug.Assert(multiSelector.ParentNode != null, "multiSelector.ParentNode != null");
				multiSelector.ParentNode.RemoveChild(multiSelector);
			}
			Debug.Assert(xml.DocumentElement != null, "xml.DocumentElement != null");
			var rules = xml.DocumentElement.ChildNodes;
			for (var index = 0; index < rules.Count; index += 1)
			{
				var rule = rules[index] as XmlElement;
				Debug.Assert(rule != null, "rule != null");
				try
				{
					rule.Attributes["pos"].InnerText = (index + 1).ToString();
				}
				catch (NullReferenceException)
				{
					if (rule.OwnerDocument != null)
					{
						var attr = rule.OwnerDocument.CreateAttribute("pos");
						attr.InnerText = (index + 1).ToString();
						if (rule.HasAttributes)
						{
							rule.Attributes.InsertBefore(attr, rule.Attributes[0]);
						}
						else
						{
							rule.Attributes.Append(attr);
						}
					}
				}
			}
		}

		private static XmlNodeList TermNodes(XmlElement ruleNode)
		{
			return
				ruleNode.SelectNodes(
					".//*[local-name() != 'name' and local-name() != 'value' and local-name() != 'unit' and local-name() != 'PROPERTY']");
		}

		protected static void ParseCssRemovingErrors(CssTreeParser parser, string styleSheet)
		{
			var error = true;
			var tryCount = 0;
			const int tryLimit = 20;
			while (error && tryCount < tryLimit)
			{
				try
				{
					parser.Parse(styleSheet);
					if (parser.Errors.Count > 0)
						throw new Antlr.Runtime.RecognitionException(string.Format("{0} errors in CSS", parser.Errors.Count));
					error = false;
				}
				catch (Exception)
				{
					var outFullPath = MakeOutFullPath(styleSheet, "Bak.css");
					File.Copy(styleSheet, outFullPath, true);
					RemoveError(parser.Errors, styleSheet, 0);
					tryCount += 1;
					try
					{
						parser.Parse(styleSheet);
						if (parser.Errors.Count > 0)
							throw new Antlr.Runtime.RecognitionException(string.Format("{0} errors in CSS", parser.Errors.Count));
						error = false;
					}
					catch (Exception)
					{
						error = true;
						File.Copy(outFullPath, styleSheet, true);
						RemoveError(parser.Errors, styleSheet, -1);
						tryCount += 1;
					}
					File.Delete(outFullPath);
				}
            }
			if (tryCount >= tryLimit) throw new Antlr.Runtime.RecognitionException(string.Format("{0} errors in {1}", parser.Errors.Count, styleSheet));
		}

        private static void RemoveError(List<string> errors, string styleSheet, int offset)
        {
	        if (errors == null || errors.Count == 0) return;
            List<int> lines = new List<int>();
            foreach (var error in errors)
            {
                var match = Regex.Match(error, @"(\d+)\:", RegexOptions.None);
                if (match.Success)
                {
                    lines.Add(int.Parse(match.Groups[1].Value) + offset);
                }
            }
            if (lines.Count > 0)
            {
                var sr = new StreamReader(styleSheet, Encoding.UTF8);
                var outFullPath = MakeOutFullPath(styleSheet, "Out.css");
	            var fw = new FileStream(outFullPath, FileMode.Create);
                var sw = new StreamWriter(fw, Encoding.UTF8);
                var rdline = 0;
                while (!sr.EndOfStream)
                {
                    rdline += 1;
                    if (lines.Contains(rdline))
                    {
                        while (!sr.EndOfStream)
                        {
                            var skipLine = sr.ReadLine();
                            Debug.Assert(skipLine != null, "skipLine != null");
                            if (skipLine.Trim().EndsWith("}")) break;
                            rdline += 1;
                        }
                    }
                    else
                    {
                        sw.WriteLine(sr.ReadLine());
                    }
                }
                sw.Close();
                fw.Close();
                sr.Close();
                File.Copy(outFullPath, styleSheet, true);
                try
                {
                    File.Delete(outFullPath);
                }
                catch
                {
                    // Try to delete the temporary file but don't crash if it doesn't work.
                }
            }
        }

		private static string MakeOutFullPath(string styleSheet, string tag)
		{
			var folder = Path.GetDirectoryName(styleSheet);
			var outName = Path.GetFileNameWithoutExtension(styleSheet) + tag;
			var outFullPath = folder != null ? Path.Combine(folder, outName) : outName;
			return outFullPath;
		}

		protected static string WriteSimpleXhtml(string xhtmlFullName)
        {
            if (string.IsNullOrEmpty(xhtmlFullName) || !File.Exists(xhtmlFullName))
                throw new ArgumentException(@"Missing Xhtml file: {0}", xhtmlFullName);
            var folder = Path.GetDirectoryName(xhtmlFullName);
            if (string.IsNullOrEmpty(folder))
                throw new ArgumentException(@"Xhtml name missing folder {0}", xhtmlFullName);
            var outfile = Path.Combine(folder, Path.GetFileNameWithoutExtension(xhtmlFullName) + "Out.xhtml");
            var ifs = new FileStream(xhtmlFullName, FileMode.Open, FileAccess.Read);
            var reader = XmlReader.Create(ifs, new XmlReaderSettings {DtdProcessing = DtdProcessing.Ignore});
            var writer = XmlWriter.Create(outfile, SimplifyXhtml.OutputSettings);
            SimplifyXhtml.Transform(reader, null, writer);
            writer.Close();
            reader.Close();
            ifs.Close();
            return outfile;
        }

        protected static void RemoveCssPseudo(string styleSheet, XmlDocument xml)
        {
            if (string.IsNullOrEmpty(styleSheet) || !File.Exists(styleSheet))
            {
                throw new ArgumentNullException("styleSheet");
            }
            VerboseMessage("Writing Stylesheet without pseudo rules: {0}", styleSheet);
            var ruleNodes = xml.SelectNodes("//RULE[PSEUDO]");
            Debug.Assert(ruleNodes != null, "ruleNodes != null");
            foreach (XmlElement ruleNode in ruleNodes)
            {
                var properties = ruleNode.SelectNodes("PROPERTY");
                Debug.Assert(properties != null, "properties != null");
                if (properties.Count <= 1)
                {
                    ruleNode.RemoveAll();
                }
                else
                {
	                var suffix = (ruleNode.SelectSingleNode("PSEUDO[name='before']") != null) ? "-pb" : "-pa";
                    var newClass = ruleNode.GetAttribute("lastClass") + suffix;
	                if (newClass == suffix) continue; /* previous versions of Fieldworks didn't have pseudo elements formed the same way */
                    ruleNode.Attributes["lastClass"].InnerText = newClass;
                    ruleNode.Attributes["target"].InnerText = newClass;
                    for (var count = ruleNode.ChildNodes.Count - 1; count >= 0; count -= 1)
                    {
                        var childNode = ruleNode.ChildNodes[count];
                        if (childNode.Name != "PROPERTY" || childNode.FirstChild.InnerText == "content")
                        {
                            ruleNode.RemoveChild(childNode);
                        }
                    }
                    Debug.Assert(ruleNode.OwnerDocument != null, "ruleNode.OwnerDocument != null");
                    var classNode = ruleNode.OwnerDocument.CreateElement("CLASS");
                    var nameNode = ruleNode.OwnerDocument.CreateElement("name");
                    nameNode.InnerText = newClass;
                    classNode.AppendChild(nameNode);
                    ruleNode.InsertBefore(classNode, ruleNode.FirstChild);
                }
            }
            WriteXmlAsCss(styleSheet, xml);
        }

        protected static void WriteXmlAsCss(string styleSheet, XmlDocument xml)
        {
            var cssFile = new FileStream(styleSheet, FileMode.Create);
            MapXmlCssToStream(xml, cssFile);
            cssFile.Close();
        }

	    protected static string SavedXmlCssFileName;
		private static void MapXmlCssToStream(XmlDocument xml, Stream cssFile)
        {
            var cssWriter = XmlWriter.Create(cssFile, XmlCss.OutputSettings);
            XmlCss.Transform(xml, null, cssWriter);
            cssWriter.Flush();
	        if (string.IsNullOrEmpty(SavedXmlCssFileName)) return;	// For testing we save XML representation of CSS
	        VerboseMessage("Writing XML stylesheet to " + SavedXmlCssFileName);
	        WriteCssXml(SavedXmlCssFileName, xml);
	        SavedXmlCssFileName = string.Empty;
        }

		private static void MakeBackupIfNecessary(string styleSheet, string xhtmlFullName)
        {
            if (_makeBackup)
            {
                var xhtmlFolder = Path.GetDirectoryName(xhtmlFullName);
                if (string.IsNullOrEmpty(xhtmlFolder))
                    throw  new ArgumentException("XHTML has no path name.");
                var xhtmlBackup = Path.Combine(xhtmlFolder,
                    Path.GetFileNameWithoutExtension(xhtmlFullName) + "Xhtml.bak");
                File.Copy(xhtmlFullName, xhtmlBackup, true);
                var folder = Path.GetDirectoryName(styleSheet);
                if (string.IsNullOrEmpty(folder))
                    throw new ArgumentException("stylesheet has no path name.");
                var backup = Path.Combine(folder, Path.GetFileNameWithoutExtension(styleSheet) + "Css.bak");
                File.Copy(styleSheet, backup, true);
            }
        }

        private static void DebugWriteClassNames(List<string> uniqueClasses)
        {
            if (_verbosity > 0)
            {
                var classNames = "Class names found: [";
                var firstClass = true;
                foreach (string uniqueClass in uniqueClasses)
                {
                    if (!firstClass)
                    {
                        classNames += ", ";
                    }
                    classNames += uniqueClass;
                    firstClass = false;
                }
                classNames += "]";
                VerboseMessage(classNames);
            }
        }

        protected static void WriteCssXml(string styleSheet, XmlDocument xml)
        {
	        var xmlName = Path.GetFileNameWithoutExtension(styleSheet) + ".xml";
			var folder = Path.GetDirectoryName(styleSheet);
			var fullName = !string.IsNullOrEmpty(folder)? Path.Combine(folder, xmlName): xmlName;
            var writerSettings = new XmlWriterSettings {Indent = true};
            var writer = XmlWriter.Create(fullName, writerSettings);
            xml.WriteTo(writer);
            writer.Close();
        }

        private static string _lastClass;
        private static string _target;
        private static bool _noData;
        private static int _term;

        protected static void AddSubTree(XmlNode n, CommonTree t, CssTreeParser ctp)
        {
            var argState = 0;
            var pos = 0;
            var term = 0;
            foreach (CommonTree child in ctp.Children(t))
            {
                var name = child.Text;
                var first = name.ToCharArray()[0];
                var second = '\0';
                if (name.Length > 1)
                {
                    second = name.ToCharArray()[1];
                }
                if (first >= 'A' && first <= 'Z' && second >= 'A' && second <= 'Z')
                {
                    Debug.Assert(n.OwnerDocument != null, "OwnerDocument != null");
                    var node = n.OwnerDocument.CreateElement(name);
                    if (name == "RULE") // later postion equals greater precedence
                    {
                        _lastClass = _target = "";
                        _noData = false;
                        _term = 0;
                    }
                    else if (name == "PROPERTY" && term == 0) // more terms equals greater precedence
                    {
                        term = _term;
                        var termAttr = n.OwnerDocument.CreateAttribute("term");
                        termAttr.Value = term.ToString();
	                    if (_target != null && _target.StartsWith("stxfin"))
	                    {
		                    termAttr.Value = "99";
	                    }
                        Debug.Assert(n.Attributes != null, "Attributes != null");
                        n.Attributes.Append(termAttr);
                    }
                    else
                    {
                        _term += 1;
                    }
                    n.AppendChild(node);
                    AddSubTree(node, child, ctp);
                    if (name == "RULE")
                    {
                        if (_noData)
                        {
                            Debug.Assert(node.ParentNode != null, "ParentNode != null");
                            node.ParentNode.RemoveChild(node);
                            continue;
                        }
                        pos += 1;
                        Debug.Assert(n.OwnerDocument != null, "OwnerDocument != null");
                        var posAttr = n.OwnerDocument.CreateAttribute("pos");
                        posAttr.Value = pos.ToString();
                        node.Attributes.Append(posAttr);
                        if (_lastClass != "")
                        {
                            var lastClassAttr = n.OwnerDocument.CreateAttribute("lastClass");
                            lastClassAttr.Value = _lastClass;
                            node.Attributes.Append(lastClassAttr);
                        }
                        if (_target != "")
                        {
                            var targetAttr = n.OwnerDocument.CreateAttribute("target");
                            targetAttr.Value = _target;
                            node.Attributes.Append(targetAttr);
                        }
                    }
                }
                else
                {
                    Debug.Assert(n.OwnerDocument != null, "OwnerDocument != null");
                    var node = n.OwnerDocument.CreateElement(argState == 0? "name": (argState & 1) == 1? "value": "unit");
                    argState += 1;
                    node.InnerText = name;
                    n.AppendChild(node);
                    if (n.Name == "CLASS")
                    {
                        if (UniqueClasses != null && !UniqueClasses.Contains(name))
                        {
                            _noData = true;
                        }
                        _target = name;
                        _lastClass = name;
                    }
                    if (n.Name == "TAG")
                    {
                        _target = name;
                    }
                    AddSubTree(node, child, ctp);
                }

            }

        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine(@"Usage: CssSimple [OPTIONS]+ FullInputFilePath.xhtml");
            Console.WriteLine(@"Simplify the input css.");
            Console.WriteLine();
            Console.WriteLine(@"Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        static void VerboseMessage(string format, params object[] args)
        {
            if (_verbosity > 0)
            {
                Console.Write(@"# ");
                Console.WriteLine(format, args);
            }
        }
    }
}
