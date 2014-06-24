#region // Copyright (C) 2014, SIL International. All Rights Reserved.
// --------------------------------------------------------------------------------------------
// <copyright file="ReferenceFonts.cs" from='2009' to='2014' company='SIL International'>
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

using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using SIL.Tool;
using epubConvert;

namespace SIL.PublishingSolution
{
    public class ReferenceFontsClass
    {
        private Exportepub parent;
        private  Dictionary<string, EmbeddedFont> _embeddedFonts;  // font information for this export
        private  Dictionary<string, string> _langFontDictionary; // languages and font names in use for this export

        public ReferenceFontsClass(Exportepub exportepub, Dictionary<string, EmbeddedFont> embeddedFonts, Dictionary<string, string> langFontDictionary)
        {
            parent = exportepub;
            _embeddedFonts = embeddedFonts;
            _langFontDictionary = langFontDictionary;
        }

        private string IncludeQuoteOnFontName(string fontname)
        {
            if (fontname.Trim().IndexOf(' ') > 0)
            {
                fontname = "'" + fontname + "'";
            }
            return fontname;
        }

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
            if (parent.EmbedFonts)
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
                    if (parent.IncludeFontVariants)
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
                    if (parent.IncludeFontVariants)
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

                if (parent.IncludeFontVariants)
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
            if (parent.InputType == "scripture")
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
                sb.Append((parent.ChapterNumbers == "Drop Cap") ? "4%;" : "5pt;");
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
            sb.Append(" src : url('");
            sb.Append(Path.GetFileName(embeddedFont.BoldFilename));
            sb.AppendLine("');");
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
            sb.Append(" src : url('");
            sb.Append(Path.GetFileName(embeddedFont.ItalicFilename));
            sb.AppendLine("');");
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
            sb.Append(" src : url('");
            sb.Append(Path.GetFileName(embeddedFont.Filename));
            sb.AppendLine("');");
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
            xdoc.Load(xhtmlFileName);
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

    }
}
