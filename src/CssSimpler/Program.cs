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
// File: program.cs (from CssSimpler.cs)
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using SIL.PublishingSolution;
using Antlr.Runtime.Tree;
using Mono.Options;

namespace CssSimpler
{
    public class Program
    {
        private static bool _showHelp;
        private static bool _outputXml;
        private static int _verbosity;
        private static bool _makeBackup;

        protected static readonly XslCompiledTransform XmlCss = new XslCompiledTransform();
        protected static readonly XslCompiledTransform SimplifyXmlCss = new XslCompiledTransform();
        protected static readonly XslCompiledTransform SimplifyXhtml = new XslCompiledTransform();
        protected static List<string> UniqueClasses;

        static void Main(string[] args)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            XmlCss.Load(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"CssSimpler.XmlCss.xsl")));
            SimplifyXmlCss.Load(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream(
				"CssSimpler.XmlCssSimplify.xsl")));
            SimplifyXhtml.Load(XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "CssSimpler.XhtmlSimplify.xsl")));
            // ReSharper restore AssignNullToNotNullAttribute
            // see: http://stackoverflow.com/questions/491595/best-way-to-parse-command-line-arguments-in-c
            var p = new OptionSet
            {
                {
                    "x|xml", "produce XML output of CSS",
                    v => _outputXml = v != null
                },
                {
                    "b|backup", "make a backup of the original CSS file",
                    v => _makeBackup = v != null
                },
                {
                    "v", "increase debug message verbosity",
                    v => { if (v != null) ++_verbosity; }
                },
                {
                    "h|help", "show this message and exit",
                    v => _showHelp = v != null
                },
            };

            List<string> extra;
            try
            {
                extra = p.Parse(args);
                if (extra.Count == 0)
                {
                    Console.WriteLine("Enter full file name to process");
                    extra.Add(Console.ReadLine());
                }
            }
            catch (OptionException e)
            {
                Console.Write("SimpleCss: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `CssSimple --help' for more information.");
                return;
            }
            if (_showHelp || extra.Count != 1)
            {
                ShowHelp(p);
                return;
            }
            var lc = new LoadClasses(extra[0]);
            DebugWriteClassNames(lc.UniqueClasses);
            Debug("Stylesheet: {0}", lc.StyleSheet);
            var parser = new CssTreeParser();
            parser.Parse(lc.StyleSheet);
            var r = parser.Root;
            var xml = new XmlDocument();
            xml.LoadXml("<ROOT/>");
            UniqueClasses = lc.UniqueClasses;
            AddSubTree(xml.DocumentElement, r, parser);
            if (_outputXml)
            {
                Debug("Writing XML stylesheet");
                WriteCssXml(lc.StyleSheet, xml);
            }
            MakeBaskupIfNecessary(lc.StyleSheet, extra[0]);
            WriteSimpleCss(lc.StyleSheet, xml); //reloads xml with simplified version
            WriteSimpleXhtml(extra[0]);
            //var contClass = new Dictionary<string, List<XmlNode>>();
            //GetContTargets(xml, contClass);
            //var outName = extra[0].Replace(".xhtml", "Out.xhtml");
            //var pc = new ProcessContent(extra[0], outName, contClass);
        }

        protected static void WriteSimpleXhtml(string xhtmlFullName)
        {
            if (string.IsNullOrEmpty(xhtmlFullName) || !File.Exists(xhtmlFullName))
                throw new ArgumentException("Missing Xhtml file: {0}", xhtmlFullName);
            var folder = Path.GetDirectoryName(xhtmlFullName);
            if (string.IsNullOrEmpty(folder))
                throw new ArgumentException("Xhtml name missing folder {0}", xhtmlFullName);
            var outfile = Path.Combine(folder, Path.GetFileNameWithoutExtension(xhtmlFullName) + "Out.xhtml");
            var ifs = new FileStream(xhtmlFullName, FileMode.Open, FileAccess.Read);
            var reader = XmlReader.Create(ifs, new XmlReaderSettings {DtdProcessing = DtdProcessing.Ignore});
            var writer = XmlWriter.Create(outfile, SimplifyXhtml.OutputSettings);
            SimplifyXhtml.Transform(reader, null, writer);
            writer.Close();
            reader.Close();
            ifs.Close();
            File.Copy(outfile, xhtmlFullName, true);
            File.Delete(outfile);
        }

        protected static void WriteSimpleCss(string styleSheet, XmlDocument xml)
        {
			if (string.IsNullOrEmpty(styleSheet) || !File.Exists(styleSheet))
	        {
				throw new ArgumentNullException("styleSheet");
	        }
            var cssFile = new FileStream(styleSheet, FileMode.Create);
            var cssWriter = XmlWriter.Create(cssFile, XmlCss.OutputSettings);
            Debug("Writing Simple Stylesheet: {0}", styleSheet);
            var memory = new MemoryStream();
            SimplifyXmlCss.Transform(xml, null, memory);
            memory.Flush();
            memory.Seek(0, 0);
            var cssReader = XmlReader.Create(memory, null);
            XmlCss.Transform(cssReader, null, cssWriter);
            cssFile.Close();
            xml.RemoveAll();
            memory.Seek(0, 0);
            xml.Load(memory);
        }

        private static void MakeBaskupIfNecessary(string styleSheet, string xhtmlFullName)
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
                Debug(classNames);
            }
        }

        //private static void GetContTargets(XmlDocument xml, Dictionary<string, List<XmlNode>> contClass)
        //{
        //    foreach (XmlNode contProp in xml.SelectNodes("//PROPERTY[name='content']"))
        //    {
        //        var target = contProp.ParentNode.Attributes["lastClass"];
        //        if (contClass.ContainsKey(target.InnerText))
        //        {
        //            contClass[target.InnerText].Add(contProp);
        //        }
        //        else
        //        {
        //            contClass[target.InnerText] = new List<XmlNode> { contProp };
        //        }
        //    }
        //}

        protected static void WriteCssXml(string styleSheet, XmlDocument xml)
        {
            var folder = Path.GetDirectoryName(styleSheet);
	        if (string.IsNullOrEmpty(folder))
	        {
		        return;
		        //throw new ArgumentException("stylesheet has no path name");
	        }
	        var fullName = Path.Combine(folder, Path.GetFileNameWithoutExtension(styleSheet) + ".xml");
            var writerSettings = new XmlWriterSettings {Indent = true};
            var writer = XmlWriter.Create(fullName, writerSettings);
            xml.WriteTo(writer);
            writer.Close();
        }

        private static string _lastClass;
        private static string _target;
        private static bool _noData;
        private static int _term;
        private static readonly List<string> NeedHigher = new List<string> { "form", "sensenumber", "headword", "name", "writingsystemprefix", "xitem", "configtarget", "configtargets", "abbreviation" };

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
                    System.Diagnostics.Debug.Assert(n.OwnerDocument != null, "OwnerDocument != null");
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
                        System.Diagnostics.Debug.Assert(n.Attributes != null, "Attributes != null");
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
                            System.Diagnostics.Debug.Assert(node.ParentNode != null, "ParentNode != null");
                            node.ParentNode.RemoveChild(node);
                            continue;
                        }
                        pos += 1;
                        System.Diagnostics.Debug.Assert(n.OwnerDocument != null, "OwnerDocument != null");
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
                    System.Diagnostics.Debug.Assert(n.OwnerDocument != null, "OwnerDocument != null");
                    var node = n.OwnerDocument.CreateElement(argState == 0? "name": (argState & 1) == 1? "value": "unit");
                    argState += 1;
                    node.InnerText = name;
                    n.AppendChild(node);
                    if (n.Name == "CLASS")
                    {
                        if (!UniqueClasses.Contains(name))
                        {
                            _noData = true;
                        }
                        _target = name;
                        if (!NeedHigher.Contains(name))
                        {
                            _lastClass = name;
                        }
                        else
                        {
                            Debug("skipping: {0}", name);
                        }
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
            Console.WriteLine("Usage: CssSimple [OPTIONS]+ FullInputFilePath.xhtml");
            Console.WriteLine("Simplify the input css.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        static void Debug(string format, params object[] args)
        {
            if (_verbosity > 0)
            {
                Console.Write("# ");
                Console.WriteLine(format, args);
            }
        }
    }
}
