#region // Copyright (C) 2014, SIL International. All Rights Reserved.
// --------------------------------------------------------------------------------------------
// <copyright file="EpubFont.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed:
//
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------
#endregion

#region using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using SIL.PublishingSolution;
using SIL.Tool;
using epubConvert.Properties;
using SilTools;

#endregion using

namespace epubConvert
{
	public class EpubFont
	{
		#region private data
		private readonly Exportepub _parent;
		private Dictionary<string, EmbeddedFont> _embeddedFonts;  // font information for this export
		private Dictionary<string, string> _langFontDictionary; // languages and font names in use for this export
		public int LanguageCount { get { return _langFontDictionary.Count; } }

		public EpubFont(Exportepub exportepub)
		{
			_parent = exportepub;
		}
		#endregion private data

		#region IEnumerable<string> LanguageCodes()
		public IEnumerable<string> LanguageCodes()
		{
			foreach (var name in _langFontDictionary.Keys)
			{
				yield return name;
			}
		}
		#endregion IEnumerable<string> LanguageCodes()

		#region IEnumerable<EmbeddedFont> EmbeddedFonts()
		public IEnumerable<EmbeddedFont> EmbeddedFonts()
		{
			foreach (var embeddedFont in _embeddedFonts.Values)
			{
				yield return embeddedFont;
			}
		}
		#endregion IEnumerable<EmbeddedFont> EmbeddedFonts()

		#region string[] InitializeLangArray(PublicationInformation projInfo)
		public string[] InitializeLangArray(PublicationInformation projInfo)
		{
			_langFontDictionary = new Dictionary<string, string>();
			_embeddedFonts = new Dictionary<string, EmbeddedFont>();
			BuildLanguagesList(projInfo.DefaultXhtmlFileWithPath);
			var langArray = new string[_langFontDictionary.Keys.Count];
			_langFontDictionary.Keys.CopyTo(langArray, 0);
			return langArray;
		}

		/// <summary>
		/// Parses the specified file and sets the internal languages list to all the languages found in the file.
		/// </summary>
		/// <param name="xhtmlFileName">File name to parse</param>
		private void BuildLanguagesList(string xhtmlFileName)
		{
			XmlDocument xmlDocument = Common.DeclareXMLDocument(false);
			var namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
			namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			var xmlReaderSettings = new XmlReaderSettings { XmlResolver = null, DtdProcessing = DtdProcessing.Parse }; //Common.DeclareXmlReaderSettings(false);
			var xmlReader = XmlReader.Create(xhtmlFileName, xmlReaderSettings);
			xmlDocument.Load(xmlReader);
			xmlReader.Close();
			// should only be one of these after splitting out the chapters.
			XmlNodeList nodes = xmlDocument.SelectNodes("//@lang", namespaceManager);
			if (nodes != null && nodes.Count > 0)
			{
				foreach (XmlNode node in nodes)
				{
					string value;
					if (_langFontDictionary.TryGetValue(node.Value, out value))
					{
						// already have this item in our list - continue
						continue;
					}
					if (node.Value.ToLower() == "utf-8")
					{
						// TE-9078 "utf-8" showing up as language in html tag - remove when fixed
						continue;
					}
					// add an entry for this language in the list (the * gets overwritten in BuildFontsList())
					_langFontDictionary.Add(node.Value, "*");
				}
			}
			// now go check to see if we're working on scripture or dictionary data
			nodes = xmlDocument.SelectNodes("//xhtml:span[@class='headword']", namespaceManager);

			if (nodes == null || nodes.Count == 0)
			{
				nodes = xmlDocument.SelectNodes("//span[@class='headword']", namespaceManager);
			}
			if (nodes != null && nodes.Count == 0)
			{
				// not in this file - this might be scripture?
				nodes = xmlDocument.SelectNodes("//xhtml:span[@class='scrBookName']", namespaceManager);
				if (nodes == null || nodes.Count == 0)
				{
					nodes = xmlDocument.SelectNodes("//span[@class='scrBookName']", namespaceManager);
				}
				if (nodes != null && nodes.Count > 0)
					_parent.InputType = "scripture";
			}
			else
			{
				_parent.InputType = "dictionary";
			}
		}
		#endregion string[] InitializeLangArray(PublicationInformation projInfo)

		#region void BuildFontsList()
		/// <summary>
		/// Returns the font families for the languages in _langFontDictionary.
		/// </summary>
		public void BuildFontsList()
		{
			// modifying the _langFontDictionary dictionary - let's make an array copy for the iteration
			int numLangs = _langFontDictionary.Keys.Count;
			var langs = new string[numLangs];
			_langFontDictionary.Keys.CopyTo(langs, 0);
			foreach (var language in langs)
			{
				string[] langCoun = language.Split('-');

				try
				{
					// When no hyphen use entire value but when there is a hyphen, look for first part
					var langTarget = langCoun.Length < 2 ? langCoun[0] : language;
					string wsPath = Common.PathCombine(Common.GetLDMLPath(), langTarget + ".ldml");
					if (File.Exists(wsPath))
					{
						var ldml = Common.DeclareXMLDocument(false);
						ldml.Load(wsPath);
						var nsmgr = new XmlNamespaceManager(ldml.NameTable);
						nsmgr.AddNamespace("palaso", "urn://palaso.org/ldmlExtensions/v1");
						var node = ldml.SelectSingleNode("//palaso:defaultFontFamily/@value", nsmgr);
						if (node != null)
						{
							// build the font information and return
							_langFontDictionary[language] = node.Value; // set the font used by this language
							_embeddedFonts[node.Value] = new EmbeddedFont(node.Value);
						}
					}
					else if (AppDomain.CurrentDomain.FriendlyName.ToLower() == "paratext.exe") // is paratext
					{
						var settingsHelper = new SettingsHelper(Param.DatabaseName);
						string fileName = settingsHelper.GetSettingsFilename();
						const string xPath = "//ScriptureText/DefaultFont";
						XmlNode xmlFont = Common.GetXmlNode(fileName, xPath);
						if (xmlFont != null)
						{
							// get the text direction specified by the .ssf file
							_langFontDictionary[language] = xmlFont.InnerText; // set the font used by this language
							_embeddedFonts[xmlFont.InnerText] = new EmbeddedFont(xmlFont.InnerText);
						}
					}
					else
					{
						// Paratext case (no .ldml file) - fall back on Charis
						_langFontDictionary[language] = "Charis SIL"; // set the font used by this language
						_embeddedFonts["Charis SIL"] = new EmbeddedFont("Charis SIL");

					}
				}
				catch
				{
				}
			}
		}
		#endregion void BuildFontsList()

		#region void ReferenceFonts(string cssFile, IPublicationInformation projInfo)
		/// <summary>
		/// Inserts links in the CSS file to the fonts used by the writing systems:
		/// - If the fonts are embedded, adds a @font-face declaration referencing the .ttf file
		///   that's found in the archive
		/// - Sets the font-family for the body:lang selector to the referenced font
		/// </summary>
		/// <param name="cssFile"></param>
		/// <param name="projInfo">Project information - used to find path to reversal file.</param>
		public void ReferenceFonts(string cssFile, IPublicationInformation projInfo)
		{
			if (!File.Exists(cssFile)) return;
			// read in the CSS file
			string mainTextDirection = "ltr";
			var reader = new StreamReader(cssFile);
			string content = reader.ReadToEnd();
			reader.Close();
			var sb = new StringBuilder();
			// write a timestamp for field troubleshooting
			WriteProductNameAndTimeStamp(sb);
			// If we're embedding the fonts, build the @font-face elements))))
			if (_parent.EmbedFonts)
			{
				foreach (var embeddedFont in _embeddedFonts.Values)
				{
					if (embeddedFont.Filename == null)
					{
						WriteMissingFontMessage(sb, embeddedFont);
						continue;
					}
					WriteFontDeclarationBlock(sb, embeddedFont);
					// if we're also embedding the font variants (bold, italic), reference them now
					if (_parent.IncludeFontVariants)
					{
						// Italic version
						if (embeddedFont.HasItalic)
						{
							WriteItalicVariantDeclarationBlock(sb, embeddedFont);
						}
						// Bold version
						if (embeddedFont.HasBold)
						{
							WriteBoldVariantDeclarationBlock(sb, embeddedFont);
						}
					}
				}
			}
			// add :lang pseudo-elements for each language and set them to the proper font
			bool firstLang = true;
			foreach (var language in _langFontDictionary)
			{
				var languageKey = language.Key;
				var languageName = language.Value;
				EmbeddedFont embeddedFont;
				// If this is the first language in the loop (i.e., the main language),
				// set the font for the body element
				if (firstLang)
				{
					mainTextDirection = Common.GetTextDirection(languageKey);
					embeddedFont = WriteMainLanguageDeclarationBlock(mainTextDirection, sb, languageName);
					if (_parent.IncludeFontVariants)
					{
						// Italic version
						if (embeddedFont != null)
							if (embeddedFont.HasItalic)
							{
								embeddedFont = WriteItalicLanguageFondDeclarationBlock(sb, languageName);
							}
						// Bold version
						if (embeddedFont != null)
							if (embeddedFont.HasBold)
							{
								WriteBoldLanguageFontDeclarationBlock(sb, languageName);
							}
					}

					var revFile = Common.PathCombine(Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath), "FlexRev.xhtml");

					if (File.Exists(revFile))
					{
						string reverseSenseNumberFont = GetLanguageForReversalNumber(revFile, languageKey);
						WriteReversalFontDeclaration(sb, languageName, languageKey, reverseSenseNumberFont);
					}

					// finished processing - clear the flag
					firstLang = false;
				}

				// set the font for the *:lang(xxx) pseudo-element
				embeddedFont = WriteGenericLanguageDeclarationBlock(sb, languageKey, languageName);

				if (_parent.IncludeFontVariants)
				{
					// italic version
					if (embeddedFont != null)
						if (embeddedFont.HasItalic)
						{
							embeddedFont = WriteItalicClassesDeclarationBlock(sb, languageKey, languageName);
						}
					// bold version
					if (embeddedFont != null)
						if (embeddedFont.HasBold)
						{
							WriteBoldClassesDeclarationBlock(sb, languageKey, languageName);
						}
				}
			}

			sb.AppendLine("/* end auto-generated font info */");
			sb.AppendLine();
			RemovesImportStatementIfItExists(content, sb);
			WriteUpdatedCssFile(cssFile, sb);
			AddDirectionAndPaddingForScripture(cssFile, mainTextDirection, sb);
		}

		private static void WriteUpdatedCssFile(string cssFile, StringBuilder sb)
		{
			var writer = new StreamWriter(cssFile);
			writer.Write(sb.ToString());
			writer.Close();
		}

		private static void RemovesImportStatementIfItExists(string content, StringBuilder sb)
		{
			// nuke the @import statement (we're going off one CSS file here)
			//string contentNoImport = content.Substring(content.IndexOf(';') + 1);
			//sb.Append(contentNoImport);
			// remove the @import statement IF it exists in the css file
			sb.Append(content.StartsWith("@import") ? content.Substring(content.IndexOf(';') + 1) : content);
		}

		private void AddDirectionAndPaddingForScripture(string cssFile, string mainTextDirection, StringBuilder sb)
		{
			// Now that we know the text direction, we can add some padding info for the chapter numbers
			// (Scripture only)
			if (_parent.InputType.ToLower() == "scripture")
			{
				var mainDirection = mainTextDirection.ToLower().Equals("ltr") ? "left" : "right";
				sb.Length = 0; // reset the stringbuilder
				sb.AppendLine(".Chapter_Number {");
				sb.Append("float: ");
				sb.Append(mainDirection);
				sb.AppendLine(";");
				sb.Append("padding-right: 5pt; padding-");
				sb.Append(mainDirection);
				sb.Append(": ");
				sb.Append((_parent.ChapterNumbers == "Drop Cap") ? "4%;" : "5pt;");
				Common.StreamReplaceInFile(cssFile, ".Chapter_Number {", sb.ToString());
			}
		}

		private void WriteBoldClassesDeclarationBlock(StringBuilder sb, string languageKey, string languageName)
		{
			// dictionary
			sb.Append(".headword:lang(");
			sb.Append(languageKey);
			sb.Append("), .headword-minor:lang(");
			sb.Append(languageKey);
			sb.Append("), .LexSense-publishStemMinorPrimaryTarget-OwnerOutlinePub:lang(");
			sb.Append(languageKey);
			sb.Append("), .LexEntry-publishStemMinorPrimaryTarget-MLHeadWordPub:lang(");
			sb.Append(languageKey);
			sb.Append("), .xsensenumber:lang(");
			sb.Append(languageKey);
			sb.Append("), .complexform-form:lang(");
			sb.Append(languageKey);
			sb.Append("), .crossref:lang(");
			sb.Append(languageKey);
			sb.Append("), .LexEntry-publishStemComponentTarget-MLHeadWordPub:lang(");
			sb.Append(languageKey);
			sb.Append("), .LexEntry-publishStemMinorPrimaryTarget-MLHeadWordPub:lang(");
			sb.Append(languageKey);
			sb.Append("), .LexSense-publishStemComponentTarget-OwnerOutlinePub:lang(");
			sb.Append(languageKey);
			sb.Append("), .LexSense-publishStemMinorPrimaryTarget-OwnerOutlinePub:lang(");
			sb.Append(languageKey);
			sb.Append("), .sense-crossref:lang(");
			sb.Append(languageKey);
			sb.Append("), .crossref-headword:lang(");
			sb.Append(languageKey);
			sb.Append("), .reversal-form:lang(");
			sb.Append(languageKey);
			sb.Append("), .Alternate_Reading:lang(");
			// scripture
			sb.Append(languageKey);
			sb.Append("), .Section_Head:lang(");
			sb.Append(languageKey);
			sb.Append("), .Section_Head_Minor:lang(");
			sb.Append(languageKey);
			sb.Append("), .Inscription:lang(");
			sb.Append(languageKey);
			sb.Append("), .Intro_Section_Head:lang(");
			sb.Append(languageKey);
			sb.Append("), .Section_Head_Major:lang(");
			sb.Append(languageKey);
			sb.Append("), .iot:lang(");
			sb.Append(languageKey);
			sb.Append("), .revsensenumber:lang(");
			sb.Append(languageKey);
			sb.AppendLine(") {");
			sb.Append("font-family: ");
			sb.Append(IncludeQuoteOnFontName(languageName));
			sb.Append(", ");
			EmbeddedFont embeddedFont;
			if (_embeddedFonts.TryGetValue(languageName, out embeddedFont))
			{
				sb.AppendLine((embeddedFont.Serif) ? "Times, serif;" : "Arial, sans-serif;");
			}
			else
			{
				// fall back on a serif font if we can't find it (shouldn't happen)
				sb.AppendLine("Times, serif;");
			}
			sb.AppendLine("}");
			//return embeddedFont;
		}

		private EmbeddedFont WriteItalicClassesDeclarationBlock(StringBuilder sb, string languageKey, string languageName)
		{
			// dictionary
			sb.Append(".partofspeech:lang(");
			sb.Append(languageKey);
			sb.Append("), .example:lang(");
			sb.Append(languageKey);
			sb.Append("), .grammatical-info:lang(");
			sb.Append(languageKey);
			sb.Append("), .lexref-type:lang(");
			sb.Append(languageKey);
			// scripture
			sb.Append("), .parallel_passage_reference:lang(");
			sb.Append(languageKey);
			sb.Append("), .Parallel_Passage_Reference:lang(");
			sb.Append(languageKey);
			sb.Append("), .Emphasis:lang(");
			sb.Append(languageKey);
			sb.Append("), .pictureCaption:lang(");
			sb.Append(languageKey);
			sb.Append("), .Section_Range_Paragraph:lang(");
			sb.Append(languageKey);
			sb.Append("), .revsensenumber:lang(");
			sb.Append(languageKey);
			sb.AppendLine(") {");
			sb.Append("font-family: ");
			sb.Append(IncludeQuoteOnFontName(languageName));
			sb.Append(", ");
			EmbeddedFont embeddedFont;
			if (_embeddedFonts.TryGetValue(languageName, out embeddedFont))
			{
				sb.AppendLine((embeddedFont.Serif) ? "Times, serif;" : "Arial, sans-serif;");
			}
			else
			{
				// fall back on a serif font if we can't find it (shouldn't happen)
				sb.AppendLine("Times, serif;");
			}
			sb.AppendLine("}");
			return embeddedFont;
		}

		private EmbeddedFont WriteGenericLanguageDeclarationBlock(StringBuilder sb, string languageKey, string languageName)
		{
			EmbeddedFont embeddedFont;
			sb.Append("*:lang(");
			sb.Append(languageKey);
			sb.AppendLine(") {");
			sb.Append("font-family: ");
			sb.Append(IncludeQuoteOnFontName(languageName));
			sb.Append(", ");
			if (_embeddedFonts.TryGetValue(languageName, out embeddedFont))
			{
				sb.AppendLine((embeddedFont.Serif) ? "Times, serif;" : "Arial, sans-serif;");
			}
			else
			{
				// fall back on a serif font if we can't find it (shouldn't happen)
				sb.AppendLine("Times, serif;");
			}
			// also insert the text direction for this language
			sb.Append("direction: ");
			sb.Append(Common.GetTextDirection(languageKey));
			sb.AppendLine(";");
			sb.AppendLine("}");
			return embeddedFont;
		}

		private static void WriteReversalFontDeclaration(StringBuilder sb, string mainLanguageName, string languageKey, string reverseSenseNumberFont)
		{
			sb.Append(".revsensenumber {");
			sb.Append("font-family: '");
			if (languageKey == reverseSenseNumberFont)
			{
				sb.Append(mainLanguageName);
			}
			sb.Append("';}");
		}

		private void WriteBoldLanguageFontDeclarationBlock(StringBuilder sb, string mainLanguageName)
		{

			sb.Append(
				".headword, .headword-minor, .LexSense-publishStemMinorPrimaryTarget-OwnerOutlinePub, ");
			sb.Append(".LexEntry-publishStemMinorPrimaryTarget-MLHeadWordPub, .xsensenumber");
			sb.Append(
				".complexform-form, .crossref, .LexEntry-publishStemComponentTarget-MLHeadWordPub, ");
			sb.Append(
				".LexEntry-publishStemMinorPrimaryTarget-MLHeadWordPub, .LexSense-publishStemComponentTarget-OwnerOutlinePub, ");
			sb.Append(".LexSense-publishStemMinorPrimaryTarget-OwnerOutlinePub, .sense-crossref, ");
			sb.Append(".crossref-headword, .reversal-form, ");
			sb.Append(".Alternate_Reading, .Section_Head_Minor, ");
			sb.AppendLine(".Inscription, .Intro_Section_Head, .Section_Head_Major, .iot {");
			if (mainLanguageName.ToLower() == "charis sil")
			{
				sb.Append("font-family: '" + mainLanguageName.Trim());
				sb.Append("-b");
				sb.Append("', ");
			}
			else
			{
				sb.Append("font-family: ");
				sb.Append(IncludeQuoteOnFontName(mainLanguageName));
				sb.Append(", ");
			}
			EmbeddedFont embeddedFont;
			if (_embeddedFonts.TryGetValue(mainLanguageName, out embeddedFont))
			{
				sb.AppendLine((embeddedFont.Serif) ? "Times, serif;" : "Arial, sans-serif;");
			}
			else
			{
				// fall back on a serif font if we can't find it (shouldn't happen)
				sb.AppendLine("Times, serif;");
			}
			sb.AppendLine("}");
			//return embeddedFont;
		}

		private EmbeddedFont WriteItalicLanguageFondDeclarationBlock(StringBuilder sb, string mainLanguageName)
		{
			sb.Append(".partofspeech, .example, .grammatical-info, .lexref-type, ");
			sb.Append(".parallel_passage_reference, .Parallel_Passage_Reference, ");
			sb.AppendLine(".Emphasis, .pictureCaption, .Section_Range_Paragraph {");
			if (mainLanguageName.ToLower() == "charis sil")
			{
				sb.Append("font-family: '" + mainLanguageName.Trim());
				sb.Append("-i");
				sb.Append("', ");
			}
			else
			{
				sb.Append("font-family: ");
				sb.Append(IncludeQuoteOnFontName(mainLanguageName));
				sb.Append(", ");
			}
			EmbeddedFont embeddedFont;
			if (_embeddedFonts.TryGetValue(mainLanguageName, out embeddedFont))
			{
				sb.AppendLine((embeddedFont.Serif) ? "Times, serif;" : "Arial, sans-serif;");
			}
			else
			{
				// fall back on a serif font if we can't find it (shouldn't happen)
				sb.AppendLine("Times, serif;");
			}
			sb.AppendLine("}");
			return embeddedFont;
		}

		private EmbeddedFont WriteMainLanguageDeclarationBlock(string mainTextDirection, StringBuilder sb, string mainLanguageName)
		{
			EmbeddedFont embeddedFont;
			sb.AppendLine("/* default language font info */");
			sb.AppendLine("body {");
			sb.Append("font-family: ");
			sb.Append(IncludeQuoteOnFontName(mainLanguageName));
			sb.Append(", ");
			if (_embeddedFonts.TryGetValue(mainLanguageName, out embeddedFont))
			{
				sb.AppendLine((embeddedFont.Serif) ? "Times, serif;" : "Arial, sans-serif;");
			}
			else
			{
				// fall back on a serif font if we can't find it (shouldn't happen)
				sb.AppendLine("Times, serif;");
			}
			// also insert the text direction for this language
			sb.Append("direction: ");
			sb.Append(mainTextDirection);
			sb.AppendLine(";");
			sb.AppendLine("}");
			return embeddedFont;
		}

		private static void WriteBoldVariantDeclarationBlock(StringBuilder sb, EmbeddedFont embeddedFont)
		{
			sb.AppendLine("@font-face {");
			sb.Append(" font-family : \"");
			sb.Append(embeddedFont.Name + "\"");
			sb.AppendLine(";");
			sb.AppendLine(" font-weight : bold;");
			sb.AppendLine(" font-style : normal;");
			sb.AppendLine(" font-variant : normal;");
			sb.AppendLine(" font-size : all;");

			if (!String.IsNullOrEmpty(Path.GetFileName(embeddedFont.BoldFilename)))
			{
				sb.Append(" src : url('");
				sb.Append(Path.GetFileName(embeddedFont.BoldFilename));
				sb.AppendLine("');");
			}
			sb.AppendLine("}");
		}

		private static void WriteItalicVariantDeclarationBlock(StringBuilder sb, EmbeddedFont embeddedFont)
		{
			sb.AppendLine("@font-face {");
			sb.Append(" font-family : \"");
			sb.Append(embeddedFont.Name + "\"");
			sb.AppendLine(";");
			sb.AppendLine(" font-weight : normal;");
			sb.AppendLine(" font-style : italic;");
			sb.AppendLine(" font-variant : normal;");
			sb.AppendLine(" font-size : all;");
			if (!String.IsNullOrEmpty(Path.GetFileName(embeddedFont.ItalicFilename)))
			{
				sb.Append(" src : url('");
				sb.Append(Path.GetFileName(embeddedFont.ItalicFilename));
				sb.AppendLine("');");
			}
			sb.AppendLine("}");
		}

		private static void WriteFontDeclarationBlock(StringBuilder sb, EmbeddedFont embeddedFont)
		{
			sb.AppendLine("@font-face {");
			sb.Append(" font-family : ");
			sb.Append("\"" + embeddedFont.Name + "\"");
			sb.AppendLine(";");
			sb.AppendLine(" font-weight : normal;");
			sb.AppendLine(" font-style : normal;");
			sb.AppendLine(" font-variant : normal;");
			sb.AppendLine(" font-size : all;");
			if (!String.IsNullOrEmpty(Path.GetFileName(embeddedFont.Filename)))
			{
				sb.Append(" src : url('");
				sb.Append(Path.GetFileName(embeddedFont.Filename));
				sb.AppendLine("');");
			}
			sb.AppendLine("}");
		}

		private static void WriteMissingFontMessage(StringBuilder sb, EmbeddedFont embeddedFont)
		{
			sb.Append("/* missing embedded font: ");
			sb.Append(embeddedFont.Name);
			sb.AppendLine(" */");
		}

		private static void WriteProductNameAndTimeStamp(StringBuilder sb)
		{
			sb.Append("/* font info - added by ");
			sb.Append(Application.ProductName);
			sb.Append(" (");
			sb.Append(Assembly.GetCallingAssembly().FullName);
			sb.AppendLine(") */");
		}


		private string GetLanguageForReversalNumber(string xhtmlFileName, string languageCode)
		{
			string language = languageCode;
			XmlDocument xdoc = Common.DeclareXMLDocument(false);
			var namespaceManager = new XmlNamespaceManager(xdoc.NameTable);
			namespaceManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
			var xr = XmlReader.Create (xhtmlFileName, new XmlReaderSettings{ DtdProcessing = DtdProcessing.Ignore });
			xdoc.Load(xr);
			xr.Close ();
			// now go check to see if we're working on scripture or dictionary data
			XmlNodeList nodes = xdoc.SelectNodes("//xhtml:span[@class='revsensenumber']", namespaceManager);
			if (nodes == null || nodes.Count == 0)
			{
				nodes = xdoc.SelectNodes("//span[@class='revsensenumber']", namespaceManager);
			}
			if (nodes != null && nodes.Count > 0)
			{
				for (int i = 0; i < nodes.Count; i++)
				{
					var xmlAttributeCollection = nodes[i].Attributes;
					if (xmlAttributeCollection != null)
						if (xmlAttributeCollection["lang"] != null)
						{
							if (xmlAttributeCollection["lang"].Value == language)
							{
								language = xmlAttributeCollection["lang"].Value;
								break;
							}
						}

					if (xmlAttributeCollection != null)
						if (xmlAttributeCollection["xml:lang"] != null)
						{
							if (xmlAttributeCollection["xml:lang"].Value == language)
							{
								language = xmlAttributeCollection["xml:lang"].Value;
								break;
							}
						}
				}
			}
			return language;
		}

		private string IncludeQuoteOnFontName(string fontname)
		{
			if (fontname.Trim().IndexOf(' ') > 0)
			{
				fontname = "'" + fontname + "'";
			}
			return fontname;
		}
		#endregion void ReferenceFonts(string cssFile, IPublicationInformation projInfo)

		#region bool EmbedAllFonts(string[] langArray, string contentFolder)
		/// <summary>
		/// Handles font embedding for the .epub file. The fonts are verified before they are copied over, to
		/// make sure they (1) exist on the system and (2) are SIL produced. For the latter, the user is able
		/// to embed them anyway if they click that they have the appropriate rights (it's an honor system approach).
		/// </summary>
		/// <param name="langArray"></param>
		/// <param name="contentFolder"></param>
		/// <returns></returns>
		public bool EmbedAllFonts(string[] langArray, string contentFolder)
		{
			var nonSilFonts = NonSilFonts();
			// If there are any non-SIL fonts in use, show the Font Warning Dialog
			// (possibly multiple times) and replace our embedded font items if needed
			// (if we're running a test, skip the dialog and just embed the font)
			if (nonSilFonts.Count > 0 && !Common.Testing)
			{
				if (!AskUserAboutEachFont(langArray, nonSilFonts))
				{
					return false; // User Cancels operation
				}
			}

			CopyFonts(contentFolder);

			// clean up
			if (nonSilFonts.Count > 0)
			{
				nonSilFonts.Clear();
			}
			return true;
		}

		private Dictionary<EmbeddedFont, string> NonSilFonts()
		{
			var nonSilFonts = new Dictionary<EmbeddedFont, string>();
			// Build the list of non-SIL fonts in use
			foreach (var embeddedFont in _embeddedFonts)
			{
				if (!embeddedFont.Value.CanRedistribute)
				{
					foreach (var language in _langFontDictionary.Keys)
					{
						if (_langFontDictionary[language].Equals(embeddedFont.Key))
						{
							// add this language to the list of langs that use this font
							string langs;
							if (nonSilFonts.TryGetValue(embeddedFont.Value, out langs))
							{
								// existing entry - add this language to the list of langs that use this font
								var sbName = new StringBuilder();
								sbName.Append(langs);
								sbName.Append(", ");
								sbName.Append(language);
								// set the value
								nonSilFonts[embeddedFont.Value] = sbName.ToString();
							}
							else
							{
								// new entry
								nonSilFonts.Add(embeddedFont.Value, language);
							}
						}
					}
				}
			}
			return nonSilFonts;
		}

		private bool AskUserAboutEachFont(string[] langArray, Dictionary<EmbeddedFont, string> nonSilFonts)
		{
			var dlg = new FontWarningDlg { RepeatAction = false, RemainingIssues = nonSilFonts.Count - 1 };
			// Handle the cases where the user wants to automatically process non-SIL / missing fonts
			if (_parent.NonSilFont == FontHandling.CancelExport)
			{
				// TODO: implement message box
				// Give the user a message indicating there's a non-SIL font in their writing system, and
				// to go fix the problem. Don't let them continue with the export.
				return false;
			}
			if (_parent.NonSilFont != FontHandling.PromptUser)
			{
				dlg.RepeatAction = true; // the handling picks up below...
				dlg.SelectedFont = _parent.DefaultFont;
			}
			foreach (var nonSilFont in nonSilFonts)
			{
				dlg.MyEmbeddedFont = nonSilFont.Key.Name;
				dlg.Languages = nonSilFont.Value;
				bool isMissing = (string.IsNullOrEmpty(nonSilFont.Key.Filename));
				bool isManualProcess = ((isMissing == false && _parent.NonSilFont == FontHandling.PromptUser) || (isMissing == true && _parent.MissingFont == FontHandling.PromptUser));
				if (dlg.RepeatAction)
				{
					// user wants to repeat the last action - if the last action
					// was to change the font, change this one as well
					// (this is also where the automatic FontHandling takes place)
					if ((!dlg.UseFontAnyway() && !nonSilFont.Key.Name.Equals(dlg.SelectedFont) && isManualProcess) || // manual "repeat this action" for non-SIL AND missing fonts
						(isMissing == false && _parent.NonSilFont == FontHandling.SubstituteDefaultFont && !nonSilFont.Key.Name.Equals(_parent.DefaultFont)) || // automatic for non-SIL fonts
						(isMissing == true && _parent.MissingFont == FontHandling.SubstituteDefaultFont && !nonSilFont.Key.Name.Equals(_parent.DefaultFont))) // automatic for missing fonts
					{
						// the user has chosen a different (SIL) font -
						// create a new EmbeddedFont and add it to the list
						_embeddedFonts.Remove(nonSilFont.Key.Name);
						var newFont = new EmbeddedFont(dlg.SelectedFont);
						_embeddedFonts[dlg.SelectedFont] = newFont; // set index value adds if it doesn't exist
						// also update the references in _langFontDictionary
						foreach (var lang in langArray)
						{
							if (_langFontDictionary[lang] == nonSilFont.Key.Name)
							{
								_langFontDictionary[lang] = dlg.SelectedFont;
							}
						}
					}
					// the UseFontAnyway checkbox (and FontHandling.EmbedFont) cases fall through here -
					// The current non-SIL font is ignored and embedded below
					continue;
				}
				// sanity check - are there any SIL fonts installed?
				int count = dlg.BuildSILFontList();
				if (count == 0)
				{
					// No SIL fonts found (returns a DialogResult.Abort):
					// tell the user there are no SIL fonts installed, and allow them to Cancel
					// and install the fonts now
					if (Utils.MsgBox(Resources.NoSILFontsMessage, Resources.NoSILFontsTitle,
										 MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
						== DialogResult.Cancel)
					{
						// user cancelled the operation - Cancel out of the whole .epub export
						return false;
					}
					// user clicked OK - leave the embedded font list alone and continue the export
					// (presumably the user has the proper rights to this font, even though it isn't
					// an SIL font)
					break;
				}
				// show the dialog
				DialogResult result = dlg.ShowDialog();
				if (result == DialogResult.OK)
				{
					if (!dlg.UseFontAnyway() && !nonSilFont.Key.Name.Equals(dlg.SelectedFont))
					{
						// the user has chosen a different (SIL) font -
						// create a new EmbeddedFont and add it to the list
						_embeddedFonts.Remove(nonSilFont.Key.Name);
						var newFont = new EmbeddedFont(dlg.SelectedFont);
						_embeddedFonts[dlg.SelectedFont] = newFont; // set index value adds if it doesn't exist
						// also update the references in _langFontDictionary
						foreach (var lang in langArray)
						{
							if (_langFontDictionary[lang] == nonSilFont.Key.Name)
							{
								_langFontDictionary[lang] = dlg.SelectedFont;
							}
						}
					}
				}
				else if (result == DialogResult.Cancel)
				{
					// User cancelled - Cancel out of the whole .epub export
					return false;
				}
				// decrement the remaining issues for the next dialog display
				dlg.RemainingIssues--;
			}
			return true;
		}

		private void CopyFonts(string contentFolder)
		{
			// copy all the fonts over
			foreach (var embeddedFont in _embeddedFonts.Values)
			{
				if (embeddedFont.Filename == null)
				{
					Debug.WriteLine("ERROR: embedded font " + embeddedFont.Name + " is not installed - skipping");
					continue;
				}
				string dest = Common.PathCombine(contentFolder, Path.GetFileName(embeddedFont.Filename));
				if (embeddedFont.Filename != string.Empty && File.Exists(embeddedFont.Filename) && !Common.IsFileReadOnly(embeddedFont.Filename))
				{
					File.Copy(embeddedFont.Filename, dest, true);

					if (_parent.IncludeFontVariants)
					{
						// italic
						if (embeddedFont.HasItalic && embeddedFont.ItalicFilename.Trim().Length > 0 &&
							embeddedFont.ItalicFilename != embeddedFont.Filename)
						{
							dest = Common.PathCombine(contentFolder, Path.GetFileName(embeddedFont.ItalicFilename));
							if (!File.Exists(dest) && !Common.IsFileReadOnly(embeddedFont.BoldFilename))
							{
								File.Copy(embeddedFont.ItalicFilename, dest, true);
							}
						}
						// bold
						if (embeddedFont.HasBold && embeddedFont.BoldFilename.Trim().Length > 0 &&
							embeddedFont.BoldFilename != embeddedFont.Filename)
						{
							dest = Common.PathCombine(contentFolder, Path.GetFileName(embeddedFont.BoldFilename));
							if (!File.Exists(dest) && !Common.IsFileReadOnly(embeddedFont.BoldFilename))
							{
								File.Copy(embeddedFont.BoldFilename, dest, true);
							}
						}
					}
				}
			}
		}
		#endregion bool EmbedAllFonts(string[] langArray, string contentFolder)

	}
}
