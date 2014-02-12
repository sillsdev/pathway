// --------------------------------------------------------------------------------------------
// <copyright file="InPreferences.cs" from='2009' to='2010' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
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

using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class InPreferencesBase
    {
        #region Private Variables
        public XmlTextWriter _writer;
        #endregion

        public void StaticMethod3()
        {
            CreateGridPreference();
            CreateGuidePreference();
            CreateMarginPreference();
            CreatePasteboardPreference();
            CreateViewPreference();
            CreatePrintPreference();
            CreatePrintBookletOption();
            CreatePrintBookletPrintPreference();
            CreateIndexOptions();
            CreateIndexHeaderSetting();
            CreatePageItemDefault();
            CreateFrameFittingOption();
            CreateButtonPreference();
            CreateTinDocumentDataObject();
            CreateLayoutGridDataInformation();
            CreateStoryGridDataInformation();
            CreateCjkGridPreference();
            CreateMojikumiUiPreference();
            CreateChapterNumberPreference();
            CloseFile();
        }

        public void StaticMethod1(string projectPath)
        {
            CreateFile(projectPath);
            CreateDataMergeOption();
            CreateLayoutAdjustmentPreference();
            CreateXMLImportPreference();
            CreateXMLExportPreference();
            CreateXMLPreference();
            CreateExportForWebPreference();
            CreateTransparencyPreference();
            CreateTransparencyDefaultContainerObject();
            CreateStoryPreference();
            CreateTextFramePreference();
            CreateTextPreference();
            CreateTextDefault();
            CreateDictionaryPreference();
            CreateAnchoredObjectDefault();
            CreateAnchoredObjectSetting();
            CreateBaselineFrameGridOption();
        }
        public void StaticMethod2()
        {
            CreateTextWrapPreference();
        }


        public void CreateFile(string targetPath)
        {
            string preferencesXMLWithPath = Common.PathCombine(targetPath, "Preferences.xml");
            _writer = new XmlTextWriter(preferencesXMLWithPath, null) { Formatting = Formatting.Indented };
            _writer.WriteStartDocument();
            _writer.WriteStartElement("idPkg:Preferences");
            _writer.WriteAttributeString("xmlns:idPkg", "http://ns.adobe.com/AdobeInDesign/idml/1.0/packaging");
            _writer.WriteAttributeString("DOMVersion", "6.0");
        }

        public void CloseFile()
        {
            _writer.WriteEndElement();
            _writer.WriteEndDocument();
            _writer.Flush();
            _writer.Close();
        }

        public void CreateChapterNumberPreference()
        {
            _writer.WriteStartElement("ChapterNumberPreference");
            _writer.WriteAttributeString("ChapterNumber", "1");
            _writer.WriteAttributeString("ChapterNumberSource", "ContinueFromPreviousDocument");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("ChapterNumberFormat");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("1, 2, 3, 4...");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateMojikumiUiPreference()
        {
            _writer.WriteStartElement("MojikumiUiPreference");
            _writer.WriteAttributeString("MojikumiUiSettings", "16383");
            _writer.WriteEndElement();
        }

        public void CreateCjkGridPreference()
        {
            _writer.WriteStartElement("CjkGridPreference");
            _writer.WriteAttributeString("ShowAllLayoutGrids", "false");
            _writer.WriteAttributeString("ShowAllFrameGrids", "true");
            _writer.WriteAttributeString("MinimumScale", "50");
            _writer.WriteAttributeString("SnapToLayoutGrid", "false");
            _writer.WriteAttributeString("ColorEveryNthCell", "10");
            _writer.WriteAttributeString("SingleLineColorMode", "true");
            _writer.WriteAttributeString("ICFMode", "false");
            _writer.WriteAttributeString("UseCircularCells", "false");
            _writer.WriteAttributeString("ShowCharacterCount", "true");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("LayoutGridColorIndex");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("GridGreen");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateStoryGridDataInformation()
        {
            _writer.WriteStartElement("StoryGridDataInformation");
            _writer.WriteAttributeString("FontStyle", "Regular");
            _writer.WriteAttributeString("PointSize", "12");
            _writer.WriteAttributeString("CharacterAki", "0");
            _writer.WriteAttributeString("LineAki", "9");
            _writer.WriteAttributeString("HorizontalScale", "100");
            _writer.WriteAttributeString("VerticalScale", "100");
            _writer.WriteAttributeString("LineAlignment", "LeftOrTopLineJustify");
            _writer.WriteAttributeString("GridAlignment", "AlignEmCenter");
            _writer.WriteAttributeString("CharacterAlignment", "AlignEmCenter");
            _writer.WriteAttributeString("GridView", "GridViewEnum");
            _writer.WriteAttributeString("CharacterCountLocation", "BottomAlign");
            _writer.WriteAttributeString("CharacterCountSize", "9.21259842519685");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("AppliedFont");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Times New Roman");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateLayoutGridDataInformation()
        {
            _writer.WriteStartElement("LayoutGridDataInformation");
            _writer.WriteAttributeString("FontStyle", "Regular");
            _writer.WriteAttributeString("PointSize", "12");
            _writer.WriteAttributeString("CharacterAki", "0");
            _writer.WriteAttributeString("LineAki", "9");
            _writer.WriteAttributeString("HorizontalScale", "100");
            _writer.WriteAttributeString("VerticalScale", "100");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("AppliedFont");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Times New Roman");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateTinDocumentDataObject()
        {
            _writer.WriteStartElement("TinDocumentDataObject");
            _writer.WriteStartElement("Properties");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateButtonPreference()
        {
            _writer.WriteStartElement("ButtonPreference");
            _writer.WriteAttributeString("Name", "$ID/");
            _writer.WriteEndElement();
        }

        public void CreateFrameFittingOption()
        {
            _writer.WriteStartElement("FrameFittingOption");
            _writer.WriteAttributeString("LeftCrop", "0");
            _writer.WriteAttributeString("TopCrop", "0");
            _writer.WriteAttributeString("RightCrop", "0");
            _writer.WriteAttributeString("BottomCrop", "0");
            _writer.WriteAttributeString("FittingOnEmptyFrame", "None");
            _writer.WriteAttributeString("FittingAlignment", "TopLeftAnchor");
            _writer.WriteEndElement();
        }

        public void CreatePageItemDefault()
        {
            _writer.WriteStartElement("PageItemDefault");
            _writer.WriteAttributeString("AppliedGraphicObjectStyle", "ObjectStyle/$ID/[Normal Graphics Frame]");
            _writer.WriteAttributeString("AppliedTextObjectStyle", "ObjectStyle/$ID/[Normal Text Frame]");
            _writer.WriteAttributeString("AppliedGridObjectStyle", "ObjectStyle/$ID/[Normal Grid]");
            _writer.WriteAttributeString("FillColor", "Swatch/None");
            _writer.WriteAttributeString("FillTint", "-1");
            _writer.WriteAttributeString("StrokeWeight", "1");
            _writer.WriteAttributeString("MiterLimit", "4");
            _writer.WriteAttributeString("EndCap", "ButtEndCap");
            _writer.WriteAttributeString("EndJoin", "MiterEndJoin");
            _writer.WriteAttributeString("StrokeType", "StrokeStyle/$ID/Solid");
            _writer.WriteAttributeString("LeftLineEnd", "None");
            _writer.WriteAttributeString("RightLineEnd", "None");
            _writer.WriteAttributeString("StrokeColor", "Swatch/None");
            _writer.WriteAttributeString("StrokeTint", "-1");
            _writer.WriteAttributeString("CornerOption", "None");
            _writer.WriteAttributeString("CornerRadius", "12");
            _writer.WriteAttributeString("GradientFillAngle", "0");
            _writer.WriteAttributeString("GradientStrokeAngle", "0");
            _writer.WriteAttributeString("GapColor", "Swatch/None");
            _writer.WriteAttributeString("GapTint", "-1");
            _writer.WriteAttributeString("StrokeAlignment", "CenterAlignment");
            _writer.WriteAttributeString("Nonprinting", "false");
            _writer.WriteEndElement();
        }

        public void CreateIndexHeaderSetting()
        {
            _writer.WriteStartElement("IndexHeaderSetting");
            _writer.WriteAttributeString("HeaderSetName", "$ID/");
            _writer.WriteAttributeString("HeaderSetLanguage", "256");
            _writer.WriteAttributeString("IndexHeaderSetHandler", "77882");
            _writer.WriteAttributeString("IndexHeaderSetGroupValue", "0");
            _writer.WriteAttributeString("IndexHeaderSetGroupOptionValue", "0");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("ListOfIndexHeaderGroup");
            _writer.WriteStartElement("IndexHeaderGroupType");
            _writer.WriteAttributeString("InternalName", "kIndexGroup_Symbol");
            _writer.WriteAttributeString("UIString", "$ID/kIndexGroup_Symbol");
            _writer.WriteAttributeString("DocumentString", "$ID/");
            _writer.WriteAttributeString("Visibility", "false");
            _writer.WriteStartElement("SectionHeaderArray");
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_Symbol");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("IndexHeaderGroupType");
            _writer.WriteAttributeString("InternalName", "$ID/IDX_Basic");
            _writer.WriteAttributeString("UIString", "$ID/kIndexGroup_Alphabet");
            _writer.WriteAttributeString("DocumentString", "$ID/");
            _writer.WriteAttributeString("Visibility", "false");
            _writer.WriteStartElement("SectionHeaderArray");
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/A");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_A");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/B");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_B");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/C");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_C");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/D");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_D");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/E");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_E");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/F");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_F");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/G");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_G");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/H");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_H");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/I");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_I");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/J");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_J");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/K");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_K");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/L");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_L");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/M");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_M");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/N");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_N");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/O");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_O");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/P");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_P");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/Q");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_Q");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/R");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_R");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/S");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_S");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/T");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_T");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/U");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_U");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/V");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_V");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/W");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_W");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/X");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_X");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/Y");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_Y");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SectionHeaderType");
            _writer.WriteAttributeString("SortingHeaderString", "$ID/Z");
            _writer.WriteAttributeString("DocumentHeaderString", "$ID/");
            _writer.WriteAttributeString("UIHeaderString", "$ID/kIndexSection_Z");
            _writer.WriteAttributeString("Language", "256");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateIndexOptions()
        {
            _writer.WriteStartElement("IndexOptions");
            _writer.WriteAttributeString("Title", "Index");
            _writer.WriteAttributeString("TitleStyle", "ParagraphStyle/$ID/[No paragraph style]");
            _writer.WriteAttributeString("ReplaceExistingIndex", "true");
            _writer.WriteAttributeString("IncludeBookDocuments", "false");
            _writer.WriteAttributeString("IncludeHiddenEntries", "false");
            _writer.WriteAttributeString("IndexFormat", "NestedFormat");
            _writer.WriteAttributeString("IncludeSectionHeadings", "true");
            _writer.WriteAttributeString("IncludeEmptyIndexSections", "false");
            _writer.WriteAttributeString("Level1Style", "ParagraphStyle/$ID/[No paragraph style]");
            _writer.WriteAttributeString("Level2Style", "ParagraphStyle/$ID/[No paragraph style]");
            _writer.WriteAttributeString("Level3Style", "ParagraphStyle/$ID/[No paragraph style]");
            _writer.WriteAttributeString("Level4Style", "ParagraphStyle/$ID/[No paragraph style]");
            _writer.WriteAttributeString("SectionHeadingStyle", "ParagraphStyle/$ID/[No paragraph style]");
            _writer.WriteAttributeString("PageNumberStyle", "CharacterStyle/$ID/[No character style]");
            _writer.WriteAttributeString("CrossReferenceStyle", "CharacterStyle/$ID/[No character style]");
            _writer.WriteAttributeString("CrossReferenceTopicStyle", "CharacterStyle/$ID/[No character style]");
            _writer.WriteAttributeString("FollowingTopicSeparator", "  ");
            _writer.WriteAttributeString("BetweenEntriesSeparator", "; ");
            _writer.WriteAttributeString("PageRangeSeparator", "^=");
            _writer.WriteAttributeString("BetweenPageNumbersSeparator", ", ");
            _writer.WriteAttributeString("BeforeCrossReferenceSeparator", ". ");
            _writer.WriteAttributeString("EntryEndSeparator", "");
            _writer.WriteEndElement();
        }

        public void CreatePrintBookletPrintPreference()
        {
            _writer.WriteStartElement("PrintBookletPrintPreference");
            _writer.WriteAttributeString("PrinterList", "");
            _writer.WriteAttributeString("PPDList", "");
            _writer.WriteAttributeString("PaperSizeList", "");
            _writer.WriteAttributeString("ScreeningList", "");
            _writer.WriteAttributeString("PrintFile", "");
            _writer.WriteAttributeString("Copies", "1");
            _writer.WriteAttributeString("Collating", "false");
            _writer.WriteAttributeString("ReverseOrder", "false");
            _writer.WriteAttributeString("PrintNonprinting", "false");
            _writer.WriteAttributeString("PrintBlankPages", "false");
            _writer.WriteAttributeString("PrintGuidesGrids", "false");
            _writer.WriteAttributeString("PaperOffset", "0");
            _writer.WriteAttributeString("PaperGap", "0");
            _writer.WriteAttributeString("PaperTransverse", "false");
            _writer.WriteAttributeString("PrintPageOrientation", "Portrait");
            _writer.WriteAttributeString("PagePosition", "UpperLeft");
            _writer.WriteAttributeString("ScaleMode", "ScaleWidthHeight");
            _writer.WriteAttributeString("ScaleWidth", "100");
            _writer.WriteAttributeString("ScaleHeight", "100");
            _writer.WriteAttributeString("ScaleProportional", "true");
            _writer.WriteAttributeString("PrintLayers", "VisiblePrintableLayers");
            _writer.WriteAttributeString("AllPrinterMarks", "false");
            _writer.WriteAttributeString("CropMarks", "false");
            _writer.WriteAttributeString("BleedMarks", "false");
            _writer.WriteAttributeString("RegistrationMarks", "false");
            _writer.WriteAttributeString("ColorBars", "false");
            _writer.WriteAttributeString("PageInformationMarks", "false");
            _writer.WriteAttributeString("MarkLineWeight", "P25pt");
            _writer.WriteAttributeString("MarkOffset", "6");
            _writer.WriteAttributeString("UseDocumentBleedToPrint", "true");
            _writer.WriteAttributeString("BleedTop", "0");
            _writer.WriteAttributeString("BleedBottom", "0");
            _writer.WriteAttributeString("BleedInside", "0");
            _writer.WriteAttributeString("BleedOutside", "0");
            _writer.WriteAttributeString("BleedChain", "false");
            _writer.WriteAttributeString("ColorOutput", "CompositeRGB");
            _writer.WriteAttributeString("TextAsBlack", "false");
            _writer.WriteAttributeString("Trapping", "Off");
            _writer.WriteAttributeString("Flip", "None");
            _writer.WriteAttributeString("Negative", "false");
            _writer.WriteAttributeString("CompositeAngle", "45");
            _writer.WriteAttributeString("CompositeFrequency", "70");
            _writer.WriteAttributeString("SimulateOverprint", "false");
            _writer.WriteAttributeString("PrintCyan", "true");
            _writer.WriteAttributeString("CyanAngle", "75");
            _writer.WriteAttributeString("CyanFrequency", "70");
            _writer.WriteAttributeString("PrintMagenta", "true");
            _writer.WriteAttributeString("MagentaAngle", "15");
            _writer.WriteAttributeString("MagentaFrequency", "70");
            _writer.WriteAttributeString("PrintYellow", "true");
            _writer.WriteAttributeString("YellowAngle", "0");
            _writer.WriteAttributeString("YellowFrequency", "70");
            _writer.WriteAttributeString("PrintBlack", "true");
            _writer.WriteAttributeString("BlackAngle", "45");
            _writer.WriteAttributeString("BlackFrequency", "70");
            _writer.WriteAttributeString("SendImageData", "OptimizedSubsampling");
            _writer.WriteAttributeString("FontDownloading", "Complete");
            _writer.WriteAttributeString("DownloadPPDFonts", "true");
            _writer.WriteAttributeString("PostScriptLevel", "Level2");
            _writer.WriteAttributeString("DataFormat", "Binary");
            _writer.WriteAttributeString("SourceSpace", "UseDocument");
            _writer.WriteAttributeString("Intent", "AbsoluteColorimetric");
            _writer.WriteAttributeString("PreserveColorNumbers", "false");
            _writer.WriteAttributeString("OPIImageReplacement", "false");
            _writer.WriteAttributeString("OmitEPS", "false");
            _writer.WriteAttributeString("OmitPDF", "false");
            _writer.WriteAttributeString("OmitBitmaps", "false");
            _writer.WriteAttributeString("FlattenerPresetName", "$ID/kFlSt_MediumDefaultName");
            _writer.WriteAttributeString("IgnoreSpreadOverrides", "false");
            _writer.WriteAttributeString("BitmapPrinting", "false");
            _writer.WriteAttributeString("BitmapResolution", "300");
            _writer.WriteAttributeString("DeviceType", "1");
            _writer.WriteAttributeString("PrintTo", "0");
            _writer.WriteAttributeString("PPDFile", "$ID/");
            _writer.WriteAttributeString("PrintToDisk", "false");
            _writer.WriteAttributeString("PrintRecord",
                                         "$ID/IE5JVwMAAABXTURX+AMAAEgAUAAgAEwAYQBzAGUAcgBKAGUAdAAgADEAMQAwADAAIAAoAE0AUwAp"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABBAAG3AAcA0PvgAUBAAEA6gpvCGQAAQAPAFgCAQABAFgC"
                                         + "AwABAEwAZQB0AHQAZQByAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAAAAAAAAQAAAAIAAAABAAAA /////wAAAAAAAAAA"
                                         + "AAAAAAAAAABESU5VIgDgABwDAAA3EiPnAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAAB"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEA"
                                         + "AAAAAAAAAAAAAOAAAABTTVRKAAAAABAA0ABIAFAAIABMAGEAcwBlAHIASgBlAHQAIAAxADEAMAAw"
                                         + "ACAAKABNAFMAKQAAAElucHV0QmluAE9wdGlvbjEAUkVTRExMAFVuaXJlc0RMTABPcmllbnRhdGlv"
                                         + "bgBQT1JUUkFJVABSZXNvbHV0aW9uAE9wdGlvbjEAUGFwZXJTaXplAExFVFRFUgBFY29ub21vZGUA"
                                         + "T3B0aW9uMQBSRVQAT3B0aW9uMQBIYWxmdG9uZQBIVF9QQVRTSVpFX0FVVE8AAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAbU5EV1IAAAAEAA0AIwABAHcAaQBuAHMAcABvAG8AbAAAAEgAUAAgAEwAYQBzAGUAcgBK"
                                         + "AGUAdAAgADEAMQAwADAAIAAoAE0AUwApAAAATgBlADAAMwA6AAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                         + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA = ");
            _writer.WriteAttributeString("PrintResolution", "600");
            _writer.WriteAttributeString("PaperSizeSelector", "$ID/AQA=");
            _writer.WriteAttributeString("PaperHeightRange", "0 0");
            _writer.WriteAttributeString("PaperWidthRange", "0 0");
            _writer.WriteAttributeString("PaperOffsetRange", "0 0");
            _writer.WriteAttributeString("SeparationScreening", "$ID/");
            _writer.WriteAttributeString("CompositeScreening", "$ID/kDefault");
            _writer.WriteAttributeString("SpotAngle", "45");
            _writer.WriteAttributeString("SpotFrequency", "70");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("Printer");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("HP LaserJet 1100 (MS)");
            _writer.WriteEndElement();
            _writer.WriteStartElement("PPD");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("$ID/");
            _writer.WriteEndElement();
            _writer.WriteStartElement("PaperSize");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Letter");
            _writer.WriteEndElement();
            _writer.WriteStartElement("PaperHeight");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Auto");
            _writer.WriteEndElement();
            _writer.WriteStartElement("PaperWidth");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Auto");
            _writer.WriteEndElement();
            _writer.WriteStartElement("MarkType");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Default");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Screening");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Default");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Profile");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("UseDocument");
            _writer.WriteEndElement();
            _writer.WriteStartElement("CRD");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Default");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ActivePrinterPreset");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Default");
            _writer.WriteEndElement();
            _writer.WriteStartElement("PaperSizeRect");
            _writer.WriteAttributeString("Left", "0");
            _writer.WriteAttributeString("Top", "0");
            _writer.WriteAttributeString("Right", "612");
            _writer.WriteAttributeString("Bottom", "792");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ImageablePaperSizeRect");
            _writer.WriteAttributeString("Left", "18");
            _writer.WriteAttributeString("Top", "14.399999618530273");
            _writer.WriteAttributeString("Right", "594");
            _writer.WriteAttributeString("Bottom", "777.5999755859375");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreatePrintBookletOption()
        {
            _writer.WriteStartElement("PrintBookletOption");
            _writer.WriteAttributeString("BookletType", "TwoUpSaddleStitch");
            _writer.WriteAttributeString("SpaceBetweenPages", "0");
            _writer.WriteAttributeString("BleedBetweenPages", "0");
            _writer.WriteAttributeString("Creep", "0");
            _writer.WriteAttributeString("SignatureSize", "SignatureSize4");
            _writer.WriteAttributeString("TopMargin", "36");
            _writer.WriteAttributeString("BottomMargin", "36");
            _writer.WriteAttributeString("LeftMargin", "36");
            _writer.WriteAttributeString("RightMargin", "36");
            _writer.WriteAttributeString("AutoAdjustMargins", "true");
            _writer.WriteAttributeString("MarginsUniformSize", "false");
            _writer.WriteAttributeString("PrintBlankPrinterSpreads", "true");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("PageRange");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("AllPages");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreatePrintPreference()
        {
            _writer.WriteStartElement("PrintPreference");
            _writer.WriteAttributeString("PrintFile", "");
            _writer.WriteAttributeString("Copies", "1");
            _writer.WriteAttributeString("Collating", "false");
            _writer.WriteAttributeString("ReverseOrder", "false");
            _writer.WriteAttributeString("Sequence", "All");
            _writer.WriteAttributeString("PrintSpreads", "false");
            _writer.WriteAttributeString("PrintMasterPages", "false");
            _writer.WriteAttributeString("PrintNonprinting", "false");
            _writer.WriteAttributeString("PrintBlankPages", "false");
            _writer.WriteAttributeString("PrintGuidesGrids", "false");
            _writer.WriteAttributeString("PaperOffset", "0");
            _writer.WriteAttributeString("PaperGap", "0");
            _writer.WriteAttributeString("PaperTransverse", "false");
            _writer.WriteAttributeString("PrintPageOrientation", "Portrait");
            _writer.WriteAttributeString("PagePosition", "UpperLeft");
            _writer.WriteAttributeString("ScaleMode", "ScaleWidthHeight");
            _writer.WriteAttributeString("ScaleWidth", "100");
            _writer.WriteAttributeString("ScaleHeight", "100");
            _writer.WriteAttributeString("ScaleProportional", "true");
            _writer.WriteAttributeString("Thumbnails", "false");
            _writer.WriteAttributeString("ThumbnailsPerPage", "K1x2");
            _writer.WriteAttributeString("Tile", "false");
            _writer.WriteAttributeString("TilingType", "Auto");
            _writer.WriteAttributeString("TilingOverlap", "108");
            _writer.WriteAttributeString("AllPrinterMarks", "false");
            _writer.WriteAttributeString("CropMarks", "false");
            _writer.WriteAttributeString("BleedMarks", "false");
            _writer.WriteAttributeString("RegistrationMarks", "false");
            _writer.WriteAttributeString("ColorBars", "false");
            _writer.WriteAttributeString("PageInformationMarks", "false");
            _writer.WriteAttributeString("MarkLineWeight", "P25pt");
            _writer.WriteAttributeString("MarkOffset", "6");
            _writer.WriteAttributeString("UseDocumentBleedToPrint", "true");
            _writer.WriteAttributeString("BleedTop", "0");
            _writer.WriteAttributeString("BleedBottom", "0");
            _writer.WriteAttributeString("BleedInside", "0");
            _writer.WriteAttributeString("BleedOutside", "0");
            _writer.WriteAttributeString("IncludeSlugToPrint", "false");
            _writer.WriteAttributeString("ColorOutput", "CompositeRGB");
            _writer.WriteAttributeString("TextAsBlack", "false");
            _writer.WriteAttributeString("Trapping", "Off");
            _writer.WriteAttributeString("Flip", "None");
            _writer.WriteAttributeString("Negative", "false");
            _writer.WriteAttributeString("CompositeAngle", "45");
            _writer.WriteAttributeString("CompositeFrequency", "70");
            _writer.WriteAttributeString("SimulateOverprint", "false");
            _writer.WriteAttributeString("PrintCyan", "true");
            _writer.WriteAttributeString("CyanAngle", "75");
            _writer.WriteAttributeString("CyanFrequency", "70");
            _writer.WriteAttributeString("PrintMagenta", "true");
            _writer.WriteAttributeString("MagentaAngle", "15");
            _writer.WriteAttributeString("MagentaFrequency", "70");
            _writer.WriteAttributeString("PrintYellow", "true");
            _writer.WriteAttributeString("YellowAngle", "0");
            _writer.WriteAttributeString("YellowFrequency", "70");
            _writer.WriteAttributeString("PrintBlack", "true");
            _writer.WriteAttributeString("BlackAngle", "45");
            _writer.WriteAttributeString("BlackFrequency", "70");
            _writer.WriteAttributeString("SendImageData", "OptimizedSubsampling");
            _writer.WriteAttributeString("FontDownloading", "Complete");
            _writer.WriteAttributeString("DownloadPPDFonts", "true");
            _writer.WriteAttributeString("PostScriptLevel", "Level2");
            _writer.WriteAttributeString("DataFormat", "Binary");
            _writer.WriteAttributeString("SourceSpace", "UseDocument");
            _writer.WriteAttributeString("Intent", "AbsoluteColorimetric");
            _writer.WriteAttributeString("OPIImageReplacement", "false");
            _writer.WriteAttributeString("OmitEPS", "false");
            _writer.WriteAttributeString("OmitPDF", "false");
            _writer.WriteAttributeString("OmitBitmaps", "false");
            _writer.WriteAttributeString("FlattenerPresetName", "$ID/kFlSt_MediumDefaultName");
            _writer.WriteAttributeString("IgnoreSpreadOverrides", "false");
            _writer.WriteAttributeString("DeviceType", "1");
            _writer.WriteAttributeString("PrintTo", "0");
            _writer.WriteAttributeString("PPDFile", "$ID/");
            _writer.WriteAttributeString("PrintToDisk", "false");
            _writer.WriteAttributeString("PrintRecord","$ID/IE5JVwMAAABXTURX+AMAAEgAUAAgAEwAYQBzAGUAcgBKAGUAdAAgADEAMQAwADAAIAAoAE0AUwAp"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABBAAG3AAcA0PvgAUBAAEA6gpvCGQAAQAPAFgCAQABAFgC"
                                                       + "AwABAEwAZQB0AHQAZQByAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAAAAAAAAQAAAAIAAAABAAAA /////wAAAAAAAAAA"
                                                       + "AAAAAAAAAABESU5VIgDgABwDAAA3EiPnAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAAB"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEA"
                                                       + "AAAAAAAAAAAAAOAAAABTTVRKAAAAABAA0ABIAFAAIABMAGEAcwBlAHIASgBlAHQAIAAxADEAMAAw"
                                                       + "ACAAKABNAFMAKQAAAElucHV0QmluAE9wdGlvbjEAUkVTRExMAFVuaXJlc0RMTABPcmllbnRhdGlv"
                                                       + "bgBQT1JUUkFJVABSZXNvbHV0aW9uAE9wdGlvbjEAUGFwZXJTaXplAExFVFRFUgBFY29ub21vZGUA"
                                                       + "T3B0aW9uMQBSRVQAT3B0aW9uMQBIYWxmdG9uZQBIVF9QQVRTSVpFX0FVVE8AAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAbU5EV1IAAAAEAA0AIwABAHcAaQBuAHMAcABvAG8AbAAAAEgAUAAgAEwAYQBzAGUAcgBK"
                                                       + "AGUAdAAgADEAMQAwADAAIAAoAE0AUwApAAAATgBlADAAMwA6AAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
                                                       + "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA = ");
            _writer.WriteAttributeString("PrintResolution", "600");
            _writer.WriteAttributeString("PaperSizeSelector", "$ID/AQA=");
            _writer.WriteAttributeString("PaperHeightRange", "0 0");
            _writer.WriteAttributeString("PaperWidthRange", "0 0");
            _writer.WriteAttributeString("PaperOffsetRange", "0 0");
            _writer.WriteAttributeString("SeparationScreening", "$ID/");
            _writer.WriteAttributeString("CompositeScreening", "$ID/kDefault");
            _writer.WriteAttributeString("SpotAngle", "45");
            _writer.WriteAttributeString("SpotFrequency", "70");
            _writer.WriteAttributeString("BleedChain", "false");
            _writer.WriteAttributeString("PreserveColorNumbers", "false");
            _writer.WriteAttributeString("BitmapPrinting", "false");
            _writer.WriteAttributeString("BitmapResolution", "300");
            _writer.WriteAttributeString("PrintLayers", "VisiblePrintableLayers");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("ActivePrinterPreset");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Default");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Printer");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("HP LaserJet 1100 (MS)");
            _writer.WriteEndElement();
            _writer.WriteStartElement("PPD");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("$ID/");
            _writer.WriteEndElement();
            _writer.WriteStartElement("PaperSize");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Letter");
            _writer.WriteEndElement();
            _writer.WriteStartElement("PaperHeight");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Auto");
            _writer.WriteEndElement();
            _writer.WriteStartElement("PaperWidth");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Auto");
            _writer.WriteEndElement();
            _writer.WriteStartElement("MarkType");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Default");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Screening");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Default");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Profile");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("UseDocument");
            _writer.WriteEndElement();
            _writer.WriteStartElement("CRD");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Default");
            _writer.WriteEndElement();
            _writer.WriteStartElement("PageRange");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("AllPages");
            _writer.WriteEndElement();
            _writer.WriteStartElement("PaperSizeRect");
            _writer.WriteAttributeString("Left", "0");
            _writer.WriteAttributeString("Top", "0");
            _writer.WriteAttributeString("Right", "612");
            _writer.WriteAttributeString("Bottom", "792");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ImageablePaperSizeRect");
            _writer.WriteAttributeString("Left", "18");
            _writer.WriteAttributeString("Top", "14.399999618530273");
            _writer.WriteAttributeString("Right", "594");
            _writer.WriteAttributeString("Bottom", "777.5999755859375");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateViewPreference()
        {
            _writer.WriteStartElement("ViewPreference");
            _writer.WriteAttributeString("GuideSnaptoZone", "4");
            _writer.WriteAttributeString("CursorKeyIncrement", "1");
            _writer.WriteAttributeString("HorizontalMeasurementUnits", "Picas");
            _writer.WriteAttributeString("VerticalMeasurementUnits", "Picas");
            _writer.WriteAttributeString("RulerOrigin", "SpreadOrigin");
            _writer.WriteAttributeString("ShowRulers", "true");
            _writer.WriteAttributeString("ShowFrameEdges", "true");
            _writer.WriteAttributeString("TypographicMeasurementUnits", "Points");
            _writer.WriteAttributeString("TextSizeMeasurementUnits", "Points");
            _writer.WriteAttributeString("PrintDialogMeasurementUnits", "Inches");
            _writer.WriteAttributeString("LineMeasurementUnits", "Points");
            _writer.WriteAttributeString("PointsPerInch", "72");
            _writer.WriteAttributeString("HorizontalCustomPoints", "12");
            _writer.WriteAttributeString("VerticalCustomPoints", "12");
            _writer.WriteAttributeString("ShowNotes", "true");
            _writer.WriteEndElement();
        }

        public void CreatePasteboardPreference()
        {
            _writer.WriteStartElement("PasteboardPreference");
            _writer.WriteAttributeString("MinimumSpaceAboveAndBelow", "72");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("PreviewBackgroundColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("LightGray");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BleedGuideColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Fiesta");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SlugGuideColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("GridBlue");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateMarginPreference()
        {
            _writer.WriteStartElement("MarginPreference");
            _writer.WriteAttributeString("ColumnCount", "1");
            _writer.WriteAttributeString("ColumnGutter", "12");
            _writer.WriteAttributeString("Top", "36");
            _writer.WriteAttributeString("Bottom", "36");
            _writer.WriteAttributeString("Left", "36");
            _writer.WriteAttributeString("Right", "36");
            _writer.WriteAttributeString("ColumnDirection", "Horizontal");
            _writer.WriteEndElement();
        }

        public void CreateGuidePreference()
        {
            _writer.WriteStartElement("GuidePreference");
            _writer.WriteAttributeString("GuidesInBack", "false");
            _writer.WriteAttributeString("GuidesShown", "true");
            _writer.WriteAttributeString("GuidesLocked", "false");
            _writer.WriteAttributeString("GuidesSnapto", "true");
            _writer.WriteAttributeString("RulerGuidesViewThreshold", "5");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("RulerGuidesColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Cyan");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateGridPreference()
        {
            _writer.WriteStartElement("GridPreference");
            _writer.WriteAttributeString("DocumentGridShown", "false");
            _writer.WriteAttributeString("DocumentGridSnapto", "false");
            _writer.WriteAttributeString("HorizontalGridlineDivision", "72");
            _writer.WriteAttributeString("VerticalGridlineDivision", "72");
            _writer.WriteAttributeString("HorizontalGridSubdivision", "8");
            _writer.WriteAttributeString("VerticalGridSubdivision", "8");
            _writer.WriteAttributeString("GridsInBack", "true");
            _writer.WriteAttributeString("BaselineGridShown", "false");
            _writer.WriteAttributeString("BaselineStart", "36");
            _writer.WriteAttributeString("BaselineDivision", "12");
            _writer.WriteAttributeString("BaselineViewThreshold", "75");
            _writer.WriteAttributeString("BaselineGridRelativeOption", "TopOfPageOfBaselineGridRelativeOption");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("GridColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("LightGray");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BaselineColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("LightBlue");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateTextWrapPreference()
        {
            _writer.WriteStartElement("TextWrapPreference");
            _writer.WriteAttributeString("Inverse", "false");
            _writer.WriteAttributeString("ApplyToMasterPageOnly", "false");
            _writer.WriteAttributeString("TextWrapSide", "BothSides");
            _writer.WriteAttributeString("TextWrapMode", "None");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("TextWrapOffset");
            _writer.WriteAttributeString("Top", "0");
            _writer.WriteAttributeString("Left", "0");
            _writer.WriteAttributeString("Bottom", "0");
            _writer.WriteAttributeString("Right", "0");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("ContourOption");
            _writer.WriteAttributeString("ContourType", "SameAsClipping");
            _writer.WriteAttributeString("IncludeInsideEdges", "false");
            _writer.WriteAttributeString("ContourPathName", "$ID/");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateBaselineFrameGridOption()
        {
            _writer.WriteStartElement("BaselineFrameGridOption");
            _writer.WriteAttributeString("UseCustomBaselineFrameGrid", "false");
            _writer.WriteAttributeString("StartingOffsetForBaselineFrameGrid", "0");
            _writer.WriteAttributeString("BaselineFrameGridRelativeOption", "TopOfInset");
            _writer.WriteAttributeString("BaselineFrameGridIncrement", "12");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("BaselineFrameGridColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("LightBlue");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateAnchoredObjectSetting()
        {
            _writer.WriteStartElement("AnchoredObjectSetting");
            _writer.WriteAttributeString("AnchoredPosition", "Anchored");
            _writer.WriteAttributeString("SpineRelative", "false");
            _writer.WriteAttributeString("LockPosition", "false");
            _writer.WriteAttributeString("PinPosition", "false");
            _writer.WriteAttributeString("AnchorPoint", "TopCenterAnchor");
            //CenterAlign, RightAlign , LeftAlign
            _writer.WriteAttributeString("HorizontalAlignment", "CenterAlign");
            _writer.WriteAttributeString("HorizontalReferencePoint", "ColumnEdge");
            _writer.WriteAttributeString("VerticalAlignment", "CenterAlign");
            _writer.WriteAttributeString("VerticalReferencePoint", "LineBaseline");
            //InLIne  //AboveLine
            _writer.WriteAttributeString("AnchorXoffset", "0");
            _writer.WriteAttributeString("AnchorYoffset", "0");
            _writer.WriteAttributeString("AnchorSpaceAbove", "4");

            _writer.WriteEndElement();

        }

        public void CreateAnchoredObjectDefault()
        {
            _writer.WriteStartElement("AnchoredObjectDefault");
            _writer.WriteAttributeString("AnchorContent", "Unassigned");
            _writer.WriteAttributeString("InitialAnchorHeight", "72");
            _writer.WriteAttributeString("InitialAnchorWidth", "72");
            _writer.WriteAttributeString("AnchoredParagraphStyle", "ParagraphStyle/$ID/[No paragraph style]");
            _writer.WriteAttributeString("AnchoredObjectStyle", "ObjectStyle/$ID/[None]");
            _writer.WriteEndElement();
        }

        public void CreateDictionaryPreference()
        {
            _writer.WriteStartElement("DictionaryPreference");
            _writer.WriteAttributeString("Composition", "Both");
            _writer.WriteAttributeString("MergeUserDictionary", "false");
            _writer.WriteAttributeString("RecomposeWhenChanged", "true");
            _writer.WriteEndElement();
        }

        public void CreateTextDefault()
        {
            _writer.WriteStartElement("TextDefault");
            _writer.WriteAttributeString("FirstLineIndent", "0");
            _writer.WriteAttributeString("LeftIndent", "0");
            _writer.WriteAttributeString("RightIndent", "0");
            _writer.WriteAttributeString("SpaceBefore", "0");
            _writer.WriteAttributeString("SpaceAfter", "0");
            _writer.WriteAttributeString("Justification", "LeftAlign");
            _writer.WriteAttributeString("SingleWordJustification", "FullyJustified");
            _writer.WriteAttributeString("AutoLeading", "120");
            _writer.WriteAttributeString("DropCapLines", "0");
            _writer.WriteAttributeString("DropCapCharacters", "0");
            _writer.WriteAttributeString("KeepLinesTogether", "false"); 
            _writer.WriteAttributeString("KeepAllLinesTogether", "false");
            _writer.WriteAttributeString("KeepWithNext", "0");
            _writer.WriteAttributeString("KeepFirstLines", "2");
            _writer.WriteAttributeString("KeepLastLines", "2");
            _writer.WriteAttributeString("StartParagraph", "Anywhere");
            _writer.WriteAttributeString("Composer", "HL Composer");
            _writer.WriteAttributeString("MinimumWordSpacing", "80");
            _writer.WriteAttributeString("MaximumWordSpacing", "133");
            _writer.WriteAttributeString("DesiredWordSpacing", "100");
            _writer.WriteAttributeString("MinimumLetterSpacing", "0");
            _writer.WriteAttributeString("MaximumLetterSpacing", "0");
            _writer.WriteAttributeString("DesiredLetterSpacing", "0");
            _writer.WriteAttributeString("MinimumGlyphScaling", "100");
            _writer.WriteAttributeString("MaximumGlyphScaling", "100");
            _writer.WriteAttributeString("DesiredGlyphScaling", "100");
            _writer.WriteAttributeString("RuleAbove", "false");
            _writer.WriteAttributeString("RuleAboveOverprint", "false");
            _writer.WriteAttributeString("RuleAboveLineWeight", "1");
            _writer.WriteAttributeString("RuleAboveTint", "-1");
            _writer.WriteAttributeString("RuleAboveOffset", "0");
            _writer.WriteAttributeString("RuleAboveLeftIndent", "0");
            _writer.WriteAttributeString("RuleAboveRightIndent", "0");
            _writer.WriteAttributeString("RuleAboveWidth", "ColumnWidth");
            _writer.WriteAttributeString("RuleAboveGapTint", "-1");
            _writer.WriteAttributeString("RuleAboveGapOverprint", "false");
            _writer.WriteAttributeString("RuleBelow", "false");
            _writer.WriteAttributeString("RuleBelowLineWeight", "1");
            _writer.WriteAttributeString("RuleBelowTint", "-1");
            _writer.WriteAttributeString("RuleBelowOffset", "0");
            _writer.WriteAttributeString("RuleBelowLeftIndent", "0");
            _writer.WriteAttributeString("RuleBelowRightIndent", "0");
            _writer.WriteAttributeString("RuleBelowWidth", "ColumnWidth");
            _writer.WriteAttributeString("RuleBelowGapTint", "-1");
            _writer.WriteAttributeString("HyphenateCapitalizedWords", "true");
            _writer.WriteAttributeString("Hyphenation", "true");
            _writer.WriteAttributeString("HyphenateBeforeLast", "2");
            _writer.WriteAttributeString("HyphenateAfterFirst", "2");
            _writer.WriteAttributeString("HyphenateWordsLongerThan", "5");
            _writer.WriteAttributeString("HyphenateLadderLimit", "3");
            _writer.WriteAttributeString("HyphenationZone", "36");
            _writer.WriteAttributeString("HyphenWeight", "5");
            _writer.WriteAttributeString("AppliedParagraphStyle", "ParagraphStyle/$ID/NormalParagraphStyle");
            _writer.WriteAttributeString("AppliedCharacterStyle", "CharacterStyle/$ID/[No character style]");
            _writer.WriteAttributeString("FontStyle", "Regular");
            _writer.WriteAttributeString("PointSize", "12");
            _writer.WriteAttributeString("KerningMethod", "$ID/Metrics");
            _writer.WriteAttributeString("Tracking", "0");
            _writer.WriteAttributeString("Capitalization", "Normal");
            _writer.WriteAttributeString("Position", "Normal");
            _writer.WriteAttributeString("Underline", "false");
            _writer.WriteAttributeString("StrikeThru", "false");
            _writer.WriteAttributeString("Ligatures", "true");
            _writer.WriteAttributeString("NoBreak", "false");
            _writer.WriteAttributeString("HorizontalScale", "100");
            _writer.WriteAttributeString("VerticalScale", "100");
            _writer.WriteAttributeString("BaselineShift", "0");
            _writer.WriteAttributeString("Skew", "0");
            _writer.WriteAttributeString("FillTint", "-1");
            _writer.WriteAttributeString("StrokeTint", "-1");
            _writer.WriteAttributeString("StrokeWeight", "1");
            _writer.WriteAttributeString("OverprintStroke", "false");
            _writer.WriteAttributeString("OverprintFill", "false");
            _writer.WriteAttributeString("OTFFigureStyle", "Default");
            _writer.WriteAttributeString("OTFOrdinal", "false");
            _writer.WriteAttributeString("OTFFraction", "false");
            _writer.WriteAttributeString("OTFDiscretionaryLigature", "false");
            _writer.WriteAttributeString("OTFTitling", "false");
            _writer.WriteAttributeString("OTFContextualAlternate", "true");
            _writer.WriteAttributeString("OTFSwash", "false");
            _writer.WriteAttributeString("UnderlineTint", "-1");
            _writer.WriteAttributeString("UnderlineGapTint", "-1");
            _writer.WriteAttributeString("UnderlineOverprint", "false");
            _writer.WriteAttributeString("UnderlineGapOverprint", "false");
            _writer.WriteAttributeString("UnderlineOffset", "-9999");
            _writer.WriteAttributeString("UnderlineWeight", "-9999");
            _writer.WriteAttributeString("StrikeThroughTint", "-1");
            _writer.WriteAttributeString("StrikeThroughGapTint", "-1");
            _writer.WriteAttributeString("StrikeThroughOverprint", "false");
            _writer.WriteAttributeString("StrikeThroughGapOverprint", "false");
            _writer.WriteAttributeString("StrikeThroughOffset", "-9999");
            _writer.WriteAttributeString("StrikeThroughWeight", "-9999");
            _writer.WriteAttributeString("FillColor", "Color/Black");
            _writer.WriteAttributeString("StrokeColor", "Swatch/None");
            _writer.WriteAttributeString("AppliedLanguage", "$ID/English: USA");
            _writer.WriteAttributeString("LastLineIndent", "0");
            _writer.WriteAttributeString("HyphenateLastWord", "true");
            _writer.WriteAttributeString("OTFSlashedZero", "false");
            _writer.WriteAttributeString("OTFHistorical", "false");
            _writer.WriteAttributeString("OTFStylisticSets", "0");
            _writer.WriteAttributeString("GradientFillLength", "-1");
            _writer.WriteAttributeString("GradientFillAngle", "0");
            _writer.WriteAttributeString("GradientStrokeLength", "-1");
            _writer.WriteAttributeString("GradientStrokeAngle", "0");
            _writer.WriteAttributeString("GradientFillStart", "0 0");
            _writer.WriteAttributeString("GradientStrokeStart", "0 0");
            _writer.WriteAttributeString("RuleBelowOverprint", "false");
            _writer.WriteAttributeString("RuleBelowGapOverprint", "false");
            _writer.WriteAttributeString("DropcapDetail", "0");
            _writer.WriteAttributeString("HyphenateAcrossColumns", "true");
            _writer.WriteAttributeString("KeepRuleAboveInFrame", "false");
            _writer.WriteAttributeString("IgnoreEdgeAlignment", "false");
            _writer.WriteAttributeString("OTFMark", "true");
            _writer.WriteAttributeString("OTFLocale", "true");
            _writer.WriteAttributeString("PositionalForm", "None");
            _writer.WriteAttributeString("ParagraphDirection", "LeftToRightDirection");
            _writer.WriteAttributeString("ParagraphJustification", "DefaultJustification");
            _writer.WriteAttributeString("MiterLimit", "4");
            _writer.WriteAttributeString("StrokeAlignment", "OutsideAlignment");
            _writer.WriteAttributeString("EndJoin", "MiterEndJoin");
            _writer.WriteAttributeString("OTFOverlapSwash", "false");
            _writer.WriteAttributeString("OTFStylisticAlternate", "false");
            _writer.WriteAttributeString("OTFJustificationAlternate", "false");
            _writer.WriteAttributeString("OTFStretchedAlternate", "false");
            _writer.WriteAttributeString("CharacterDirection", "DefaultDirection");
            _writer.WriteAttributeString("KeyboardDirection", "DefaultDirection");
            _writer.WriteAttributeString("DigitsType", "DefaultDigits");
            _writer.WriteAttributeString("Kashidas", "DefaultKashidas");
            _writer.WriteAttributeString("DiacriticPosition", "OpentypePosition");
            _writer.WriteAttributeString("XOffsetDiacritic", "0");
            _writer.WriteAttributeString("YOffsetDiacritic", "0");
            _writer.WriteAttributeString("ParagraphBreakType", "Anywhere");
            _writer.WriteAttributeString("PageNumberType", "AutoPageNumber");
            _writer.WriteAttributeString("AppliedNamedGrid", "n");
            _writer.WriteAttributeString("CharacterAlignment", "AlignEmCenter");
            _writer.WriteAttributeString("Tsume", "0");
            _writer.WriteAttributeString("LeadingAki", "-1");
            _writer.WriteAttributeString("TrailingAki", "-1");
            _writer.WriteAttributeString("CharacterRotation", "0");
            _writer.WriteAttributeString("Jidori", "0");
            _writer.WriteAttributeString("ShataiMagnification", "0");
            _writer.WriteAttributeString("ShataiDegreeAngle", "4500");
            _writer.WriteAttributeString("ShataiAdjustRotation", "false");
            _writer.WriteAttributeString("ShataiAdjustTsume", "true");
            _writer.WriteAttributeString("Tatechuyoko", "false");
            _writer.WriteAttributeString("TatechuyokoXOffset", "0");
            _writer.WriteAttributeString("TatechuyokoYOffset", "0");
            _writer.WriteAttributeString("KentenTint", "-1");
            _writer.WriteAttributeString("KentenStrokeTint", "-1");
            _writer.WriteAttributeString("KentenWeight", "-1");
            _writer.WriteAttributeString("KentenOverprintFill", "Auto");
            _writer.WriteAttributeString("KentenOverprintStroke", "Auto");
            _writer.WriteAttributeString("KentenKind", "None");
            _writer.WriteAttributeString("KentenPlacement", "0");
            _writer.WriteAttributeString("KentenAlignment", "AlignKentenCenter");
            _writer.WriteAttributeString("KentenPosition", "AboveRight");
            _writer.WriteAttributeString("KentenFontSize", "-1");
            _writer.WriteAttributeString("KentenXScale", "100");
            _writer.WriteAttributeString("KentenYScale", "100");
            _writer.WriteAttributeString("KentenCustomCharacter", "");
            _writer.WriteAttributeString("KentenCharacterSet", "CharacterInput");
            _writer.WriteAttributeString("RubyTint", "-1");
            _writer.WriteAttributeString("RubyWeight", "-1");
            _writer.WriteAttributeString("RubyOverprintFill", "Auto");
            _writer.WriteAttributeString("RubyOverprintStroke", "Auto");
            _writer.WriteAttributeString("RubyStrokeTint", "-1");
            _writer.WriteAttributeString("RubyFontSize", "-1");
            _writer.WriteAttributeString("RubyOpenTypePro", "true");
            _writer.WriteAttributeString("RubyXScale", "100");
            _writer.WriteAttributeString("RubyYScale", "100");
            _writer.WriteAttributeString("RubyType", "PerCharacterRuby");
            _writer.WriteAttributeString("RubyAlignment", "RubyJIS");
            _writer.WriteAttributeString("RubyPosition", "AboveRight");
            _writer.WriteAttributeString("RubyXOffset", "0");
            _writer.WriteAttributeString("RubyYOffset", "0");
            _writer.WriteAttributeString("RubyParentSpacing", "RubyParent121Aki");
            _writer.WriteAttributeString("RubyAutoAlign", "true");
            _writer.WriteAttributeString("RubyOverhang", "false");
            _writer.WriteAttributeString("RubyAutoScaling", "false");
            _writer.WriteAttributeString("RubyParentScalingPercent", "66");
            _writer.WriteAttributeString("RubyParentOverhangAmount", "RubyOverhangOneRuby");
            _writer.WriteAttributeString("Warichu", "false");
            _writer.WriteAttributeString("WarichuSize", "50");
            _writer.WriteAttributeString("WarichuLines", "2");
            _writer.WriteAttributeString("WarichuLineSpacing", "0");
            _writer.WriteAttributeString("WarichuAlignment", "Auto");
            _writer.WriteAttributeString("WarichuCharsAfterBreak", "2");
            _writer.WriteAttributeString("WarichuCharsBeforeBreak", "2");
            _writer.WriteAttributeString("OTFProportionalMetrics", "false");
            _writer.WriteAttributeString("OTFHVKana", "false");
            _writer.WriteAttributeString("OTFRomanItalics", "false");
            _writer.WriteAttributeString("ScaleAffectsLineHeight", "false");
            _writer.WriteAttributeString("CjkGridTracking", "false");
            _writer.WriteAttributeString("GlyphForm", "None");
            _writer.WriteAttributeString("GridAlignFirstLineOnly", "false");
            _writer.WriteAttributeString("GridAlignment", "None");
            _writer.WriteAttributeString("GridGyoudori", "0");
            _writer.WriteAttributeString("AutoTcy", "0");
            _writer.WriteAttributeString("AutoTcyIncludeRoman", "false");
            _writer.WriteAttributeString("KinsokuType", "KinsokuPushInFirst");
            _writer.WriteAttributeString("KinsokuHangType", "None");
            _writer.WriteAttributeString("BunriKinshi", "true");
            _writer.WriteAttributeString("Rensuuji", "true");
            _writer.WriteAttributeString("RotateSingleByteCharacters", "false");
            _writer.WriteAttributeString("LeadingModel", "LeadingModelAkiBelow");
            _writer.WriteAttributeString("RubyAutoTcyDigits", "0");
            _writer.WriteAttributeString("RubyAutoTcyIncludeRoman", "false");
            _writer.WriteAttributeString("RubyAutoTcyAutoScale", "true");
            _writer.WriteAttributeString("TreatIdeographicSpaceAsSpace", "false");
            _writer.WriteAttributeString("AllowArbitraryHyphenation", "false");
            _writer.WriteAttributeString("ParagraphGyoudori", "false");
            _writer.WriteAttributeString("BulletsAndNumberingListType", "NoList");
            _writer.WriteAttributeString("NumberingExpression", "^#.^t");
            _writer.WriteAttributeString("BulletsTextAfter", "^t");
            _writer.WriteAttributeString("NumberingLevel", "1");
            _writer.WriteAttributeString("NumberingContinue", "true");
            _writer.WriteAttributeString("NumberingStartAt", "1");
            _writer.WriteAttributeString("NumberingApplyRestartPolicy", "true");
            _writer.WriteAttributeString("BulletsAlignment", "LeftAlign");
            _writer.WriteAttributeString("NumberingAlignment", "LeftAlign");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("BalanceRaggedLines");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("NoBalancing");
            _writer.WriteEndElement();
            _writer.WriteStartElement("RuleAboveColor");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Text Color");
            _writer.WriteEndElement();
            _writer.WriteStartElement("RuleAboveGapColor");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("Swatch/None");
            _writer.WriteEndElement();
            _writer.WriteStartElement("RuleAboveType");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("StrokeStyle/$ID/Solid");
            _writer.WriteEndElement();
            _writer.WriteStartElement("RuleBelowColor");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Text Color");
            _writer.WriteEndElement();
            _writer.WriteStartElement("RuleBelowGapColor");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("Swatch/None");
            _writer.WriteEndElement();
            _writer.WriteStartElement("RuleBelowType");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("StrokeStyle/$ID/Solid");
            _writer.WriteEndElement();
            _writer.WriteStartElement("AppliedFont");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Times New Roman");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Leading");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Auto");
            _writer.WriteEndElement();
            _writer.WriteStartElement("UnderlineColor");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Text Color");
            _writer.WriteEndElement();
            _writer.WriteStartElement("UnderlineGapColor");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("Swatch/None");
            _writer.WriteEndElement();
            _writer.WriteStartElement("UnderlineType");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("StrokeStyle/$ID/Solid");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrikeThroughColor");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Text Color");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrikeThroughGapColor");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("Swatch/None");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrikeThroughType");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("StrokeStyle/$ID/Solid");
            _writer.WriteEndElement();
            _writer.WriteStartElement("KentenFillColor");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Text Color");
            _writer.WriteEndElement();
            _writer.WriteStartElement("KentenStrokeColor");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Text Color");
            _writer.WriteEndElement();
            _writer.WriteStartElement("KentenFont");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("$ID/");
            _writer.WriteEndElement();
            _writer.WriteStartElement("KentenFontStyle");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Nothing");
            _writer.WriteEndElement();
            _writer.WriteStartElement("RubyFill");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Text Color");
            _writer.WriteEndElement();
            _writer.WriteStartElement("RubyStroke");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Text Color");
            _writer.WriteEndElement();
            _writer.WriteStartElement("RubyFont");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("$ID/");
            _writer.WriteEndElement();
            _writer.WriteStartElement("RubyFontStyle");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Nothing");
            _writer.WriteEndElement();
            _writer.WriteStartElement("KinsokuSet");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Nothing");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Mojikumi");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Nothing");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BulletChar");
            _writer.WriteAttributeString("BulletCharacterType", "UnicodeOnly");
            _writer.WriteAttributeString("BulletCharacterValue", "8226");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BulletsFont");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("$ID/");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BulletsFontStyle");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Nothing");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BulletsCharacterStyle");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("CharacterStyle/$ID/[No character style]");
            _writer.WriteEndElement();
            _writer.WriteStartElement("NumberingCharacterStyle");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("CharacterStyle/$ID/[No character style]");
            _writer.WriteEndElement();
            _writer.WriteStartElement("AppliedNumberingList");
            _writer.WriteAttributeString("type", "object");
            _writer.WriteString("NumberingList/$ID/[Default]");
            _writer.WriteEndElement();
            _writer.WriteStartElement("NumberingFormat");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("1, 2, 3, 4...");
            _writer.WriteEndElement();
            _writer.WriteStartElement("NumberingRestartPolicies");
            _writer.WriteAttributeString("RestartPolicy", "AnyPreviousLevel");
            _writer.WriteAttributeString("LowerLevel", "0");
            _writer.WriteAttributeString("UpperLevel", "0");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateTextPreference()
        {
            _writer.WriteStartElement("TextPreference");
            _writer.WriteAttributeString("TypographersQuotes", "true");
            _writer.WriteAttributeString("HighlightHjViolations", "false");
            _writer.WriteAttributeString("HighlightKeeps", "false");
            _writer.WriteAttributeString("HighlightSubstitutedGlyphs", "false");
            _writer.WriteAttributeString("HighlightCustomSpacing", "false");
            _writer.WriteAttributeString("HighlightSubstitutedFonts", "true");
            _writer.WriteAttributeString("UseOpticalSize", "true");
            _writer.WriteAttributeString("UseParagraphLeading", "false");
            _writer.WriteAttributeString("SuperscriptSize", "58.3");
            _writer.WriteAttributeString("SuperscriptPosition", "33.3");
            _writer.WriteAttributeString("SubscriptSize", "58.3");
            _writer.WriteAttributeString("SubscriptPosition", "33.3");
            _writer.WriteAttributeString("SmallCap", "70");
            _writer.WriteAttributeString("LeadingKeyIncrement", "2");
            _writer.WriteAttributeString("BaselineShiftKeyIncrement", "2");
            _writer.WriteAttributeString("KerningKeyIncrement", "20");
            _writer.WriteAttributeString("ShowInvisibles", "false");
            _writer.WriteAttributeString("JustifyTextWraps", "false");
            _writer.WriteAttributeString("AbutTextToTextWrap", "true");
            _writer.WriteAttributeString("ZOrderTextWrap", "false");
            _writer.WriteAttributeString("LinkTextFilesWhenImporting", "false");
            _writer.WriteAttributeString("HighlightKinsoku", "false");
            _writer.WriteAttributeString("UseNewVerticalScaling", "false");
            _writer.WriteAttributeString("UseCidMojikumi", "false");
            _writer.WriteAttributeString("EnableStylePreviewMode", "false");
            _writer.WriteAttributeString("SmartTextReflow", "true");
            _writer.WriteAttributeString("AddPages", "EndOfStory");
            _writer.WriteAttributeString("LimitToMasterTextFrames", "true");
            _writer.WriteAttributeString("PreserveFacingPageSpreads", "false");
            _writer.WriteAttributeString("DeleteEmptyPages", "false");
            _writer.WriteEndElement();
        }

        public void CreateTextFramePreference()
        {
            _writer.WriteStartElement("TextFramePreference");
            _writer.WriteAttributeString("TextColumnCount", "1");
            _writer.WriteAttributeString("TextColumnGutter", "12");
            _writer.WriteAttributeString("TextColumnFixedWidth", "144");
            _writer.WriteAttributeString("UseFixedColumnWidth", "false");
            _writer.WriteAttributeString("FirstBaselineOffset", "AscentOffset");
            _writer.WriteAttributeString("MinimumFirstBaselineOffset", "0");
            _writer.WriteAttributeString("VerticalJustification", "TopAlign");
            _writer.WriteAttributeString("VerticalThreshold", "0");
            _writer.WriteAttributeString("IgnoreWrap", "false");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("InsetSpacing");
            _writer.WriteAttributeString("type", "list");
            _writer.WriteStartElement("ListItem");
            _writer.WriteAttributeString("type", "unit");
            _writer.WriteString("0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ListItem");
            _writer.WriteAttributeString("type", "unit");
            _writer.WriteString("0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ListItem");
            _writer.WriteAttributeString("type", "unit");
            _writer.WriteString("0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ListItem");
            _writer.WriteAttributeString("type", "unit");
            _writer.WriteString("0");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateStoryPreference()
        {
            _writer.WriteStartElement("StoryPreference");
            _writer.WriteAttributeString("OpticalMarginAlignment", "false");
            _writer.WriteAttributeString("OpticalMarginSize", "12");
            _writer.WriteAttributeString("FrameType", "TextFrameType");
            _writer.WriteAttributeString("StoryOrientation", "Horizontal");
            _writer.WriteAttributeString("StoryDirection", "LeftToRightDirection");
            _writer.WriteEndElement();
        }

        public void CreateTransparencyDefaultContainerObject()
        {
            _writer.WriteStartElement("TransparencyDefaultContainerObject");
            _writer.WriteStartElement("TransparencySetting");
            _writer.WriteStartElement("BlendingSetting");
            _writer.WriteAttributeString("BlendMode", "Normal");
            _writer.WriteAttributeString("Opacity", "100");
            _writer.WriteAttributeString("KnockoutGroup", "false");
            _writer.WriteAttributeString("IsolateBlending", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("DropShadowSetting");
            _writer.WriteAttributeString("Mode", "None");
            _writer.WriteAttributeString("BlendMode", "Multiply");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("XOffset", "7");
            _writer.WriteAttributeString("YOffset", "7");
            _writer.WriteAttributeString("Size", "5");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("Spread", "0");
            _writer.WriteAttributeString("UseGlobalLight", "false");
            _writer.WriteAttributeString("KnockedOut", "true");
            _writer.WriteAttributeString("HonorOtherEffects", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("FeatherSetting");
            _writer.WriteAttributeString("Mode", "None");
            _writer.WriteAttributeString("Width", "9");
            _writer.WriteAttributeString("CornerType", "Diffusion");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("ChokeAmount", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("InnerShadowSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("BlendMode", "Multiply");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("Angle", "120");
            _writer.WriteAttributeString("Distance", "7");
            _writer.WriteAttributeString("UseGlobalLight", "false");
            _writer.WriteAttributeString("ChokeAmount", "0");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("OuterGlowSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("BlendMode", "Screen");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("Technique", "Softer");
            _writer.WriteAttributeString("Spread", "0");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteEndElement();
            _writer.WriteStartElement("InnerGlowSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("BlendMode", "Screen");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("Technique", "Softer");
            _writer.WriteAttributeString("Spread", "0");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("Source", "EdgeSourced");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BevelAndEmbossSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("Style", "InnerBevel");
            _writer.WriteAttributeString("Technique", "SmoothContour");
            _writer.WriteAttributeString("Depth", "100");
            _writer.WriteAttributeString("Direction", "Up");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("Soften", "0");
            _writer.WriteAttributeString("Angle", "120");
            _writer.WriteAttributeString("Altitude", "30");
            _writer.WriteAttributeString("UseGlobalLight", "false");
            _writer.WriteAttributeString("HighlightColor", "n");
            _writer.WriteAttributeString("HighlightBlendMode", "Screen");
            _writer.WriteAttributeString("HighlightOpacity", "75");
            _writer.WriteAttributeString("ShadowColor", "n");
            _writer.WriteAttributeString("ShadowBlendMode", "Multiply");
            _writer.WriteAttributeString("ShadowOpacity", "75");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SatinSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("BlendMode", "Multiply");
            _writer.WriteAttributeString("Opacity", "50");
            _writer.WriteAttributeString("Angle", "120");
            _writer.WriteAttributeString("Distance", "7");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("InvertEffect", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("DirectionalFeatherSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("LeftWidth", "0");
            _writer.WriteAttributeString("RightWidth", "0");
            _writer.WriteAttributeString("TopWidth", "0");
            _writer.WriteAttributeString("BottomWidth", "0");
            _writer.WriteAttributeString("ChokeAmount", "0");
            _writer.WriteAttributeString("Angle", "0");
            _writer.WriteAttributeString("FollowShapeMode", "LeadingEdge");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("GradientFeatherSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("Type", "Linear");
            _writer.WriteAttributeString("Angle", "0");
            _writer.WriteAttributeString("Length", "0");
            _writer.WriteAttributeString("GradientStart", "0 0");
            _writer.WriteAttributeString("HiliteAngle", "0");
            _writer.WriteAttributeString("HiliteLength", "0");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeTransparencySetting");
            _writer.WriteStartElement("BlendingSetting");
            _writer.WriteAttributeString("BlendMode", "Normal");
            _writer.WriteAttributeString("Opacity", "100");
            _writer.WriteAttributeString("KnockoutGroup", "false");
            _writer.WriteAttributeString("IsolateBlending", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("DropShadowSetting");
            _writer.WriteAttributeString("Mode", "None");
            _writer.WriteAttributeString("BlendMode", "Multiply");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("XOffset", "7");
            _writer.WriteAttributeString("YOffset", "7");
            _writer.WriteAttributeString("Size", "5");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("Spread", "0");
            _writer.WriteAttributeString("UseGlobalLight", "false");
            _writer.WriteAttributeString("KnockedOut", "true");
            _writer.WriteAttributeString("HonorOtherEffects", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("FeatherSetting");
            _writer.WriteAttributeString("Mode", "None");
            _writer.WriteAttributeString("Width", "9");
            _writer.WriteAttributeString("CornerType", "Diffusion");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("ChokeAmount", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("InnerShadowSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("BlendMode", "Multiply");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("Angle", "120");
            _writer.WriteAttributeString("Distance", "7");
            _writer.WriteAttributeString("UseGlobalLight", "false");
            _writer.WriteAttributeString("ChokeAmount", "0");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("OuterGlowSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("BlendMode", "Screen");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("Technique", "Softer");
            _writer.WriteAttributeString("Spread", "0");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteEndElement();
            _writer.WriteStartElement("InnerGlowSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("BlendMode", "Screen");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("Technique", "Softer");
            _writer.WriteAttributeString("Spread", "0");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("Source", "EdgeSourced");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BevelAndEmbossSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("Style", "InnerBevel");
            _writer.WriteAttributeString("Technique", "SmoothContour");
            _writer.WriteAttributeString("Depth", "100");
            _writer.WriteAttributeString("Direction", "Up");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("Soften", "0");
            _writer.WriteAttributeString("Angle", "120");
            _writer.WriteAttributeString("Altitude", "30");
            _writer.WriteAttributeString("UseGlobalLight", "false");
            _writer.WriteAttributeString("HighlightColor", "n");
            _writer.WriteAttributeString("HighlightBlendMode", "Screen");
            _writer.WriteAttributeString("HighlightOpacity", "75");
            _writer.WriteAttributeString("ShadowColor", "n");
            _writer.WriteAttributeString("ShadowBlendMode", "Multiply");
            _writer.WriteAttributeString("ShadowOpacity", "75");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SatinSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("BlendMode", "Multiply");
            _writer.WriteAttributeString("Opacity", "50");
            _writer.WriteAttributeString("Angle", "120");
            _writer.WriteAttributeString("Distance", "7");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("InvertEffect", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("DirectionalFeatherSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("LeftWidth", "0");
            _writer.WriteAttributeString("RightWidth", "0");
            _writer.WriteAttributeString("TopWidth", "0");
            _writer.WriteAttributeString("BottomWidth", "0");
            _writer.WriteAttributeString("ChokeAmount", "0");
            _writer.WriteAttributeString("Angle", "0");
            _writer.WriteAttributeString("FollowShapeMode", "LeadingEdge");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("GradientFeatherSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("Type", "Linear");
            _writer.WriteAttributeString("Angle", "0");
            _writer.WriteAttributeString("Length", "0");
            _writer.WriteAttributeString("GradientStart", "0 0");
            _writer.WriteAttributeString("HiliteAngle", "0");
            _writer.WriteAttributeString("HiliteLength", "0");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("FillTransparencySetting");
            _writer.WriteStartElement("BlendingSetting");
            _writer.WriteAttributeString("BlendMode", "Normal");
            _writer.WriteAttributeString("Opacity", "100");
            _writer.WriteAttributeString("KnockoutGroup", "false");
            _writer.WriteAttributeString("IsolateBlending", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("DropShadowSetting");
            _writer.WriteAttributeString("Mode", "None");
            _writer.WriteAttributeString("BlendMode", "Multiply");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("XOffset", "7");
            _writer.WriteAttributeString("YOffset", "7");
            _writer.WriteAttributeString("Size", "5");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("Spread", "0");
            _writer.WriteAttributeString("UseGlobalLight", "false");
            _writer.WriteAttributeString("KnockedOut", "true");
            _writer.WriteAttributeString("HonorOtherEffects", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("FeatherSetting");
            _writer.WriteAttributeString("Mode", "None");
            _writer.WriteAttributeString("Width", "9");
            _writer.WriteAttributeString("CornerType", "Diffusion");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("ChokeAmount", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("InnerShadowSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("BlendMode", "Multiply");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("Angle", "120");
            _writer.WriteAttributeString("Distance", "7");
            _writer.WriteAttributeString("UseGlobalLight", "false");
            _writer.WriteAttributeString("ChokeAmount", "0");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("OuterGlowSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("BlendMode", "Screen");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("Technique", "Softer");
            _writer.WriteAttributeString("Spread", "0");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteEndElement();
            _writer.WriteStartElement("InnerGlowSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("BlendMode", "Screen");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("Technique", "Softer");
            _writer.WriteAttributeString("Spread", "0");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("Source", "EdgeSourced");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BevelAndEmbossSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("Style", "InnerBevel");
            _writer.WriteAttributeString("Technique", "SmoothContour");
            _writer.WriteAttributeString("Depth", "100");
            _writer.WriteAttributeString("Direction", "Up");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("Soften", "0");
            _writer.WriteAttributeString("Angle", "120");
            _writer.WriteAttributeString("Altitude", "30");
            _writer.WriteAttributeString("UseGlobalLight", "false");
            _writer.WriteAttributeString("HighlightColor", "n");
            _writer.WriteAttributeString("HighlightBlendMode", "Screen");
            _writer.WriteAttributeString("HighlightOpacity", "75");
            _writer.WriteAttributeString("ShadowColor", "n");
            _writer.WriteAttributeString("ShadowBlendMode", "Multiply");
            _writer.WriteAttributeString("ShadowOpacity", "75");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SatinSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("BlendMode", "Multiply");
            _writer.WriteAttributeString("Opacity", "50");
            _writer.WriteAttributeString("Angle", "120");
            _writer.WriteAttributeString("Distance", "7");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("InvertEffect", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("DirectionalFeatherSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("LeftWidth", "0");
            _writer.WriteAttributeString("RightWidth", "0");
            _writer.WriteAttributeString("TopWidth", "0");
            _writer.WriteAttributeString("BottomWidth", "0");
            _writer.WriteAttributeString("ChokeAmount", "0");
            _writer.WriteAttributeString("Angle", "0");
            _writer.WriteAttributeString("FollowShapeMode", "LeadingEdge");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("GradientFeatherSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("Type", "Linear");
            _writer.WriteAttributeString("Angle", "0");
            _writer.WriteAttributeString("Length", "0");
            _writer.WriteAttributeString("GradientStart", "0 0");
            _writer.WriteAttributeString("HiliteAngle", "0");
            _writer.WriteAttributeString("HiliteLength", "0");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("ContentTransparencySetting");
            _writer.WriteStartElement("BlendingSetting");
            _writer.WriteAttributeString("BlendMode", "Normal");
            _writer.WriteAttributeString("Opacity", "100");
            _writer.WriteAttributeString("KnockoutGroup", "false");
            _writer.WriteAttributeString("IsolateBlending", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("DropShadowSetting");
            _writer.WriteAttributeString("Mode", "None");
            _writer.WriteAttributeString("BlendMode", "Multiply");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("XOffset", "7");
            _writer.WriteAttributeString("YOffset", "7");
            _writer.WriteAttributeString("Size", "5");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("Spread", "0");
            _writer.WriteAttributeString("UseGlobalLight", "false");
            _writer.WriteAttributeString("KnockedOut", "true");
            _writer.WriteAttributeString("HonorOtherEffects", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("FeatherSetting");
            _writer.WriteAttributeString("Mode", "None");
            _writer.WriteAttributeString("Width", "9");
            _writer.WriteAttributeString("CornerType", "Diffusion");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("ChokeAmount", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("InnerShadowSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("BlendMode", "Multiply");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("Angle", "120");
            _writer.WriteAttributeString("Distance", "7");
            _writer.WriteAttributeString("UseGlobalLight", "false");
            _writer.WriteAttributeString("ChokeAmount", "0");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("OuterGlowSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("BlendMode", "Screen");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("Technique", "Softer");
            _writer.WriteAttributeString("Spread", "0");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteEndElement();
            _writer.WriteStartElement("InnerGlowSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("BlendMode", "Screen");
            _writer.WriteAttributeString("Opacity", "75");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("Technique", "Softer");
            _writer.WriteAttributeString("Spread", "0");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("Source", "EdgeSourced");
            _writer.WriteEndElement();
            _writer.WriteStartElement("BevelAndEmbossSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("Style", "InnerBevel");
            _writer.WriteAttributeString("Technique", "SmoothContour");
            _writer.WriteAttributeString("Depth", "100");
            _writer.WriteAttributeString("Direction", "Up");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("Soften", "0");
            _writer.WriteAttributeString("Angle", "120");
            _writer.WriteAttributeString("Altitude", "30");
            _writer.WriteAttributeString("UseGlobalLight", "false");
            _writer.WriteAttributeString("HighlightColor", "n");
            _writer.WriteAttributeString("HighlightBlendMode", "Screen");
            _writer.WriteAttributeString("HighlightOpacity", "75");
            _writer.WriteAttributeString("ShadowColor", "n");
            _writer.WriteAttributeString("ShadowBlendMode", "Multiply");
            _writer.WriteAttributeString("ShadowOpacity", "75");
            _writer.WriteEndElement();
            _writer.WriteStartElement("SatinSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("EffectColor", "n");
            _writer.WriteAttributeString("BlendMode", "Multiply");
            _writer.WriteAttributeString("Opacity", "50");
            _writer.WriteAttributeString("Angle", "120");
            _writer.WriteAttributeString("Distance", "7");
            _writer.WriteAttributeString("Size", "7");
            _writer.WriteAttributeString("InvertEffect", "false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("DirectionalFeatherSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("LeftWidth", "0");
            _writer.WriteAttributeString("RightWidth", "0");
            _writer.WriteAttributeString("TopWidth", "0");
            _writer.WriteAttributeString("BottomWidth", "0");
            _writer.WriteAttributeString("ChokeAmount", "0");
            _writer.WriteAttributeString("Angle", "0");
            _writer.WriteAttributeString("FollowShapeMode", "LeadingEdge");
            _writer.WriteAttributeString("Noise", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("GradientFeatherSetting");
            _writer.WriteAttributeString("Applied", "false");
            _writer.WriteAttributeString("Type", "Linear");
            _writer.WriteAttributeString("Angle", "0");
            _writer.WriteAttributeString("Length", "0");
            _writer.WriteAttributeString("GradientStart", "0 0");
            _writer.WriteAttributeString("HiliteAngle", "0");
            _writer.WriteAttributeString("HiliteLength", "0");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateTransparencyPreference()
        {
            _writer.WriteStartElement("TransparencyPreference");
            _writer.WriteAttributeString("BlendingSpace", "CMYK");
            _writer.WriteAttributeString("GlobalLightAngle", "120");
            _writer.WriteAttributeString("GlobalLightAltitude", "30");
            _writer.WriteEndElement();
        }

        public void CreateExportForWebPreference()
        {
            _writer.WriteStartElement("ExportForWebPreference");
            _writer.WriteAttributeString("CopyFormattedImages", "false");
            _writer.WriteAttributeString("CopyOptimizedImages", "false");
            _writer.WriteAttributeString("CopyOriginalImages", "false");
            _writer.WriteAttributeString("ImageConversion", "Automatic");
            _writer.WriteAttributeString("GIFOptionsPalette", "AdaptivePalette");
            _writer.WriteAttributeString("GIFOptionsInterlaced", "false");
            _writer.WriteAttributeString("JPEGOptionsQuality", "Medium");
            _writer.WriteAttributeString("JPEGOptionsFormat", "BaselineEncoding");
            _writer.WriteEndElement();
        }

        public void CreateXMLPreference()
        {
            _writer.WriteStartElement("XMLPreference");
            _writer.WriteAttributeString("DefaultStoryTagName", "Story");
            _writer.WriteAttributeString("DefaultTableTagName", "Table");
            _writer.WriteAttributeString("DefaultCellTagName", "Cell");
            _writer.WriteAttributeString("DefaultImageTagName", "Image");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("DefaultStoryTagColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("BrickRed");
            _writer.WriteEndElement();
            _writer.WriteStartElement("DefaultTableTagColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("DarkBlue");
            _writer.WriteEndElement();
            _writer.WriteStartElement("DefaultCellTagColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("GrassGreen");
            _writer.WriteEndElement();
            _writer.WriteStartElement("DefaultImageTagColor");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Violet");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateXMLExportPreference()
        {
            _writer.WriteStartElement("XMLExportPreference");
            _writer.WriteAttributeString("ViewAfterExport", "false");
            _writer.WriteAttributeString("ExportFromSelected", "false");
            _writer.WriteAttributeString("FileEncoding", "UTF8");
            _writer.WriteAttributeString("Ruby", "false");
            _writer.WriteAttributeString("ExcludeDtd", "true");
            _writer.WriteAttributeString("CopyOriginalImages", "false");
            _writer.WriteAttributeString("CopyOptimizedImages", "false");
            _writer.WriteAttributeString("CopyFormattedImages", "false");
            _writer.WriteAttributeString("ImageConversion", "Automatic");
            _writer.WriteAttributeString("GIFOptionsPalette", "AdaptivePalette");
            _writer.WriteAttributeString("GIFOptionsInterlaced", "false");
            _writer.WriteAttributeString("JPEGOptionsQuality", "Medium");
            _writer.WriteAttributeString("JPEGOptionsFormat", "BaselineEncoding");
            _writer.WriteAttributeString("AllowTransform", "false");
            _writer.WriteAttributeString("CharacterReferences", "false");
            _writer.WriteAttributeString("ExportUntaggedTablesFormat", "CALS");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("PreferredBrowser");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Nothing");
            _writer.WriteEndElement();
            _writer.WriteStartElement("TransformFilename");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("StylesheetInXML");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateXMLImportPreference()
        {
            _writer.WriteStartElement("XMLImportPreference");
            _writer.WriteAttributeString("ImportToSelected", "true");
            _writer.WriteAttributeString("ImportStyle", "MergeImport");
            _writer.WriteAttributeString("CreateLinkToXML", "false");
            _writer.WriteAttributeString("RepeatTextElements", "true");
            _writer.WriteAttributeString("IgnoreUnmatchedIncoming", "false");
            _writer.WriteAttributeString("ImportTextIntoTables", "true");
            _writer.WriteAttributeString("IgnoreWhitespace", "false");
            _writer.WriteAttributeString("RemoveUnmatchedExisting", "false");
            _writer.WriteAttributeString("AllowTransform", "false");
            _writer.WriteAttributeString("ImportCALSTables", "true");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("TransformFilename");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("StylesheetInXML");
            _writer.WriteEndElement();
            _writer.WriteStartElement("TransformParameters");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateLayoutAdjustmentPreference()
        {
            _writer.WriteStartElement("LayoutAdjustmentPreference");
            _writer.WriteAttributeString("EnableLayoutAdjustment", "false");
            _writer.WriteAttributeString("SnapZone", "2");
            _writer.WriteAttributeString("AllowGraphicsToResize", "true");
            _writer.WriteAttributeString("AllowRulerGuidesToMove", "true");
            _writer.WriteAttributeString("IgnoreRulerGuideAlignments", "false");
            _writer.WriteAttributeString("IgnoreObjectOrLayerLocks", "true");
            _writer.WriteEndElement();
        }

        public void CreateDataMergeOption()
        {
            _writer.WriteStartElement("DataMergeOption");
            _writer.WriteAttributeString("FittingOption", "Proportional");
            _writer.WriteAttributeString("CenterImage", "false");
            _writer.WriteAttributeString("LinkImages", "true");
            _writer.WriteAttributeString("RemoveBlankLines", "false");
            _writer.WriteAttributeString("CreateNewDocument", "false");
            _writer.WriteAttributeString("DocumentSize", "50");
            _writer.WriteEndElement();
        }
    }
}