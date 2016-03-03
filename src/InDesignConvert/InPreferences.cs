// --------------------------------------------------------------------------------------------
// <copyright file="InPreferences.cs" from='2009' to='2014' company='SIL International'>
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
// Creates the InDesign Preferences file 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Collections;

namespace SIL.PublishingSolution
{
    public class InPreferences : InPreferencesBase
    {
        #region Private Variables
        Dictionary<string, Dictionary<string, string>> _idAllClass = new Dictionary<string, Dictionary<string, string>>();
        #endregion

        public bool CreateIDPreferences(string projectPath, Dictionary<string, Dictionary<string, string>> idAllClass)
        {
            try
            {
                _idAllClass = idAllClass;
                StaticMethod1(projectPath);
                CreateFootnoteOption();
                StaticMethod2();
                CreateDocumentPreference();
                StaticMethod3();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return false;
        }

        public void CreateFootnoteOption()
        {
            _writer.WriteStartElement("FootnoteOption");
            _writer.WriteAttributeString("StartAt", "1");
            _writer.WriteAttributeString("Prefix", "");
            _writer.WriteAttributeString("Suffix", "");
            _writer.WriteAttributeString("FootnoteTextStyle", "ParagraphStyle/$ID/NormalParagraphStyle");
            _writer.WriteAttributeString("FootnoteMarkerStyle", "CharacterStyle/$ID/[No character style]");
            _writer.WriteAttributeString("SeparatorText", "^t");
            _writer.WriteAttributeString("SpaceBetween", "0");
            _writer.WriteAttributeString("Spacer", "0");
            _writer.WriteAttributeString("FootnoteFirstBaselineOffset", "LeadingOffset");
            _writer.WriteAttributeString("FootnoteMinimumFirstBaselineOffset", "0");
            _writer.WriteAttributeString("EosPlacement", "false");
            _writer.WriteAttributeString("NoSplitting", "true");
            _writer.WriteAttributeString("RuleOn", "true");
            _writer.WriteAttributeString("RuleLineWeight", "1");
            _writer.WriteAttributeString("RuleTint", "100");
            _writer.WriteAttributeString("RuleGapTint", "100");
            _writer.WriteAttributeString("RuleGapOverprint", "false");
            _writer.WriteAttributeString("RuleOverprint", "false");
            _writer.WriteAttributeString("RuleLeftIndent", "0");
            _writer.WriteAttributeString("RuleWidth", "72");
            _writer.WriteAttributeString("RuleOffset", "0");
            _writer.WriteAttributeString("ContinuingRuleOn", "true");
            _writer.WriteAttributeString("ContinuingRuleLineWeight", "1");
            _writer.WriteAttributeString("ContinuingRuleTint", "100");
            _writer.WriteAttributeString("ContinuingRuleGapTint", "100");
            _writer.WriteAttributeString("ContinuingRuleOverprint", "false");
            _writer.WriteAttributeString("ContinuingRuleGapOverprint", "false");
            _writer.WriteAttributeString("ContinuingRuleLeftIndent", "0");
            _writer.WriteAttributeString("ContinuingRuleWidth", "288");
            _writer.WriteAttributeString("ContinuingRuleOffset", "0");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("FootnoteNumberingStyle");
            _writer.WriteAttributeString("type", "enumeration");
            string numType = "Symbols";
            if (_idAllClass.ContainsKey("autonum"))
            {
                numType = "Arabic";
            }
            _writer.WriteString(numType);
            _writer.WriteEndElement();
            _writer.WriteStartElement("RestartNumbering");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("DontRestart");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ShowPrefixSuffix");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("NoPrefixSuffix");
            _writer.WriteEndElement();
            _writer.WriteStartElement("MarkerPositioning");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("SuperscriptMarker");
            _writer.WriteEndElement();
            _writer.WriteStartElement("RuleType");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("StrokeStyle/$ID/Solid");
            _writer.WriteEndElement();
            _writer.WriteStartElement("RuleColor");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("Color/Black");
            _writer.WriteEndElement();
            _writer.WriteStartElement("RuleGapColor");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("Swatch/None");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ContinuingRuleType");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("StrokeStyle/$ID/Solid");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ContinuingRuleColor");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("Color/Black");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ContinuingRuleGapColor");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("Swatch/None");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void CreateDocumentPreference()
        {
            _writer.WriteStartElement("DocumentPreference");
            _writer.WriteAttributeString("PageHeight", _idAllClass["@page"]["Page-Height"]);
            _writer.WriteAttributeString("PageWidth", _idAllClass["@page"]["Page-Width"]);
            _writer.WriteAttributeString("PagesPerDocument", "1");
            _writer.WriteAttributeString("FacingPages", _idAllClass["@page"]["FacingPages"]);
            _writer.WriteAttributeString("DocumentBleedTopOffset", "0");
            _writer.WriteAttributeString("DocumentBleedBottomOffset", "0");
            _writer.WriteAttributeString("DocumentBleedInsideOrLeftOffset", "0");
            _writer.WriteAttributeString("DocumentBleedOutsideOrRightOffset", "0");
            _writer.WriteAttributeString("DocumentBleedUniformSize", "true");
            _writer.WriteAttributeString("SlugTopOffset", "0");
            _writer.WriteAttributeString("SlugBottomOffset", "0");
            _writer.WriteAttributeString("SlugInsideOrLeftOffset", "0");
            _writer.WriteAttributeString("SlugRightOrOutsideOffset", "0");
            _writer.WriteAttributeString("DocumentSlugUniformSize", "false");
            _writer.WriteAttributeString("PreserveLayoutWhenShuffling", "true");
            _writer.WriteAttributeString("AllowPageShuffle", "true");
            _writer.WriteAttributeString("OverprintBlack", "true");
            _writer.WriteAttributeString("PageBinding", "LeftToRight");
            _writer.WriteAttributeString("ColumnDirection", "Horizontal");
            _writer.WriteAttributeString("ColumnGuideLocked", "true");
            if (_idAllClass.ContainsKey("@page") || _idAllClass.ContainsKey("@page:first") || _idAllClass.ContainsKey("@page:left") || _idAllClass.ContainsKey("@page:right"))
            {
                _writer.WriteAttributeString("MasterTextFrame", "true");
            }
            else
            {
                _writer.WriteAttributeString("MasterTextFrame", "false");
            }
            _writer.WriteAttributeString("SnippetImportUsesOriginalLocation", "false");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("ColumnGuideColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Violet");
            _writer.WriteEndElement();
            _writer.WriteStartElement("MarginGuideColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Magenta");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }
    }
}