// --------------------------------------------------------------------------------------------
// <copyright file="InMasterSpread.cs" from='2009' to='2014' company='SIL International'>
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
// Creates the InMasterSread file 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace SIL.PublishingSolution
{
    public class InMasterSpread:InMasterSpreadBase
    {
        #region Private Variables
        private string _pageClass = "@page";
        private float _x1;
        private float yt1;
        private float ytr;
        private float ybl;
        private float yt2;
        ArrayList _masterPageList = new ArrayList();
        private string _projectPath = string.Empty;
        readonly ArrayList pageNames = new ArrayList { "@page:first", "@page", "@page:left", "@page:right" };
        Dictionary<string, Dictionary<string, string>> _cssProperty = new Dictionary<string, Dictionary<string, string>>();
        ArrayList _headwordStyles = new ArrayList();
        #endregion

        public ArrayList CreateIDMasterSpread(string projectPath, Dictionary<string, Dictionary<string, string>> cssProperty, ArrayList headwordStyles)
        {
            try
            {
                _cssProperty = cssProperty;
                _projectPath = projectPath;
                _headwordStyles = headwordStyles;
                for (int i = 0; i < pageNames.Count; i++)
                {
                    string masterFileName = string.Empty;
                    string masterPrefix = string.Empty;
                    string masterName = string.Empty;
                    _pageClass = pageNames[i].ToString();
                    switch (_pageClass)
                    {
                        case "@page:first":
                            if (!_cssProperty.ContainsKey("@page:first"))
                                break;
                            masterFileName = "MasterSpread_First";
                            masterPrefix = "F";
                            masterName = "FirstPage";
                            break;
                        case "@page":
                            if (!_cssProperty.ContainsKey("@page"))
                                break;
                            masterFileName = "MasterSpread_All";
                            masterPrefix = "A";
                            masterName = "AllPage";
                            break;
                        case "@page:left":
                            if (!_cssProperty.ContainsKey("@page:left"))
                                break;
                            masterFileName = "MasterSpread_Left";
                            masterPrefix = "L";
                            masterName = "LeftPage";
                            break;
                        case "@page:right":
                            if (!_cssProperty.ContainsKey("@page:right"))
                                break;
                            masterFileName = "MasterSpread_Right";
                            masterPrefix = "R";
                            masterName = "RightPage";
                            break;
                    }
                    if (masterFileName.Length > 0)
                    {
                        _masterPageList.Add(masterFileName);
                        CreateFile(projectPath, masterFileName);
                        CreateMasterSpread(masterFileName, masterPrefix, masterName);
                        CreatePage();
                        CreateTextFrame();
                        CloseFile();
                    }
                }
                return _masterPageList;
            }
            catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
                return _masterPageList;
            }
        }

        private void CreateTextFrame()
        {
            string headerFooterWidth = GetHeaderFooterWidth();
            ArrayList ReferenceNames = new ArrayList { "top-left", "top-center", "top-right", "bottom-left", "bottom-center", "bottom-right" };

            float frameHeight = 24f;//20.0f
            Dictionary<string, string> classValues = _cssProperty[_pageClass];

            float halfWidth = float.Parse(classValues["Page-Width"], CultureInfo.GetCultureInfo("en-US")) / 2;
            _x1 = halfWidth - float.Parse(classValues["Margin-Left"], CultureInfo.GetCultureInfo("en-US"));

            float halfHeight = float.Parse(classValues["Page-Height"], CultureInfo.GetCultureInfo("en-US")) / 2;
            yt1 = halfHeight - float.Parse(classValues["Margin-Top"], CultureInfo.GetCultureInfo("en-US"));

            ytr = yt1 += float.Parse(classValues["Margin-Top"], CultureInfo.GetCultureInfo("en-US")) / 2;  // 4 inch = place header in 2 inch - Heade to center
            yt2 = ybl = yt1 + frameHeight;

            float y = halfHeight - float.Parse(classValues["Margin-Bottom"], CultureInfo.GetCultureInfo("en-US"));
            float yb1 = y + float.Parse(classValues["Margin-Bottom"], CultureInfo.GetCultureInfo("en-US")) / 2;  // 4 inch = place header in 2 inch - Heade to center
            float yb2 = yb1 + frameHeight;

            float x2 = 22;// _x1 - float.Parse(headerFooterWidth);
            float x3 = 22;// float.Parse(headerFooterWidth) - (_x1 - float.Parse(headerFooterWidth));//x2 40
            float x4 = _x1 + 1;// (_x1 - float.Parse(headerFooterWidth));// x3 + float.Parse(headerFooterWidth); + 18

            //      x1,y1       x2,y2    
            //
            //      x2,y1       x2,y2
            //

            float[,] sign = {
                                  {-1 * _x1, -1 * yt1, -1 * _x1, -1 * ybl, -1 * x2, -1 * yt2, -1 * x2, -1 * ytr},
                                  {-1 * x2, -1 * ytr, -1 * x2, -1 * yt2, 1 * x3, -1 * yt2, 1 * x3, -1 * ytr},
                                  {1 * x3, -1 * yt1, 1 * x3, -1 * ybl, 1 * x4, -1 * yt2, 1 * x4, -1 * ytr},
      
                                  {-1 * _x1, 1 * yb1, -1 * _x1, 1 * yb2, -1 * x2, 1 * yb2, -1 * x2, 1 * yb1},
                                  {1 * x3, 1 * yb1, 1 * x3, 1 * yb2, -1 * x2, 1 * yb2, -1 * x2, 1 * yb1},
                                  {1 * x3, 1 * yb1, 1 * x3, 1 * yb2, 1 * x4, 1 * yb2, 1 * x4, 1 * yb1}
                              };
            for (int i = 0; i < ReferenceNames.Count; i++)
            {
                string parentStory = "u19c";
                string className = _pageClass + "-" + ReferenceNames[i];
                if (_cssProperty.ContainsKey(className))
                {
                    Dictionary<string, string> Properties = _cssProperty[className];
                    if (Properties.ContainsKey("content"))
                    {
                        string newStory = string.Empty;
                        var inStoryHeaderFooter = new InStoryHeaderFooter();
                        string folderPath = _projectPath.Replace("MasterSpreads", "Stories");
                        string storyName = GetStoryName(_pageClass, ReferenceNames[i].ToString());
                        string property = Properties["content"].Replace("|", "");

                        string format = GetReferenceFormat(property);

                        if (property.IndexOf("counter") >= 0)
                        {
                            newStory = inStoryHeaderFooter.CreateStoryforHeaderFooter(folderPath, storyName, "PageCount", className, _headwordStyles);
                        }
                        else if (format == "BF CF:VF-CL:VL" || format == "BF CF-CL" || format == "BF CF"
                            || format == "BL CL" || format == "BF CF:VF" || format == "BL CL:VL" || format == "GF" || format == "GL")
                        {
                            newStory = inStoryHeaderFooter.CreateStoryforHeaderFooter(folderPath, storyName, format, className, _headwordStyles);
                        }
                        if (newStory.Length > 0)
                        {
                            if (!_masterPageList.Contains(newStory))
                                _masterPageList.Add(newStory);
                            parentStory = newStory.Replace("Story_", "").Replace(".xml", "");
                        }
                    }
                }
                string FrameName = ReferenceNames[i].ToString().Replace("-", "");
                CreateMainFrameStaticMethod1(FrameName, parentStory);
                CreateReferenceFrameProperties(sign[i, 0], sign[i, 1], sign[i, 2], sign[i, 3], sign[i, 4],
                                               sign[i, 5], sign[i, 6], sign[i, 7]);

                CreateReferenceFramePreferenceforReferences(headerFooterWidth);

                CreateMainFrameStaticMethod2();
            }
        }

        private static string GetReferenceFormat(string property)
        {
            string result = property.Replace("string(booknamefirst)", "BF");
            result = result.Replace("string(booknamelast)", "BL");
            result = result.Replace("string(chapterfirst)", "CF");
            result = result.Replace("string(chapterlast)", "CL");
            result = result.Replace("string(versefirst)", "VF");
            result = result.Replace("string(verselast)", "VL");
            result = result.Replace("string(booknamelast)", "BL");
            result = result.Replace("string(booknamelast)", "BL");
            result = result.Replace("string(booknamelast)", "BL");
            result = result.Replace("string(guidewordfirst)", "GF");
            result = result.Replace("string(guidewordlast)", "GL");
            result = result.Replace(" : ", ":");
            result = result.Replace(" - ", "-");
            return result;
        }

        private string GetStoryName(string pageName, string position)
        {
            string result = "u19c";
            string pageType = "a";
            if (pageName.IndexOf(":") > 0)
            {
                string[] pseudoType = pageName.Split(':');
                pageType = pseudoType[1].Substring(0, 1);
            }
            switch (position)
            {
                case "top-left":
                    result = "utl" + pageType;
                    break;
                case "top-center":
                    result = "utc" + pageType;
                    break;
                case "top-right":
                    result = "utr" + pageType;
                    break;
                case "bottom-left":
                    result = "ubl" + pageType;
                    break;
                case "bottom-center":
                    result = "ubc" + pageType;
                    break;
                case "bottom-right":
                    result = "ubr" + pageType;
                    break;
            }
            return result;
        }

        // Header footer Frames
        private void CreateReferenceFrameProperties(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("PathGeometry");
            _writer.WriteStartElement("GeometryPathType");
            _writer.WriteAttributeString("PathOpen", "false");
            _writer.WriteStartElement("PathPointArray");
            _writer.WriteStartElement("PathPointType"); // Top-Left
            //x3 = 396;
            //x4 = 396;
            _writer.WriteAttributeString("Anchor", x1 + " " + y1);
            _writer.WriteAttributeString("LeftDirection", x1 + " " + y1);
            _writer.WriteAttributeString("RightDirection", x1 + " " + y1);
            _writer.WriteEndElement();
            _writer.WriteStartElement("PathPointType"); //Bottom-Left
            _writer.WriteAttributeString("Anchor", x2 + " " + y2);
            _writer.WriteAttributeString("LeftDirection", x2 + " " + y2);
            _writer.WriteAttributeString("RightDirection", x2 + " " + y2);
            _writer.WriteEndElement();
            _writer.WriteStartElement("PathPointType"); //Bottom-Right
            _writer.WriteAttributeString("Anchor", x3 + " " + y3);
            _writer.WriteAttributeString("LeftDirection", x3 + " " + y3);
            _writer.WriteAttributeString("RightDirection", x3 + " " + y3);
            _writer.WriteEndElement();
            _writer.WriteStartElement("PathPointType"); //Top-Right
            _writer.WriteAttributeString("Anchor", x4 + " " + y4);
            _writer.WriteAttributeString("LeftDirection", x4 + " " + y4);
            _writer.WriteAttributeString("RightDirection", x4 + " " + y4);
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("Label");
            _writer.WriteStartElement("KeyValuePair");
            _writer.WriteAttributeString("Key", "Label");
            _writer.WriteAttributeString("Value", "Master");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteEndElement();
        }

        private void CreateReferenceFramePreferenceforReferences(string headerFooterWidth)
        {
            _writer.WriteStartElement("TextFramePreference");
            _writer.WriteAttributeString("TextColumnFixedWidth", headerFooterWidth);
            _writer.WriteAttributeString("VerticalJustification", "TopAlign");//CenterAlign
            _writer.WriteEndElement();
        }


        private string GetColumnPosition()
        {
            float colPosition = float.Parse(_cssProperty[_pageClass]["Page-Width"], CultureInfo.GetCultureInfo("en-US"))
                - (float.Parse(_cssProperty[_pageClass]["Margin-Top"], CultureInfo.GetCultureInfo("en-US"))
                + float.Parse(_cssProperty[_pageClass]["Margin-Right"], CultureInfo.GetCultureInfo("en-US")));
            return colPosition.ToString();
        }

        private string GetHeaderFooterWidth()
        {
            float pageWidthNoMargin = (float.Parse(_cssProperty[_pageClass]["Page-Width"], CultureInfo.GetCultureInfo("en-US")) -
                                       (float.Parse(_cssProperty[_pageClass]["Margin-Left"], CultureInfo.GetCultureInfo("en-US")) +
                                       float.Parse(_cssProperty[_pageClass]["Margin-Right"], CultureInfo.GetCultureInfo("en-US"))));
            return (pageWidthNoMargin / 3F).ToString();
        }

        private void CreateMasterSpread(string pageName, string prefix, string baseName)
        {
            _writer.WriteStartElement("MasterSpread");
            _writer.WriteAttributeString("Self", pageName);
            _writer.WriteAttributeString("ItemTransform", "1 0 0 1 0 0");
            _writer.WriteAttributeString("Name", prefix + "-" + baseName);
            _writer.WriteAttributeString("NamePrefix", prefix);
            _writer.WriteAttributeString("BaseName", baseName);
            _writer.WriteAttributeString("ShowMasterItems", "true");
            _writer.WriteAttributeString("PageCount", "1");
            _writer.WriteAttributeString("OverriddenPageItemProps", "");
        }

        private void CreatePage()
        {
            _writer.WriteStartElement("Page");
            _writer.WriteAttributeString("Self", "page1"); //TODO
            _writer.WriteAttributeString("Name", "F");
            _writer.WriteAttributeString("AppliedTrapPreset", "TrapPreset/$ID/kDefaultTrapStyleName");
            _writer.WriteAttributeString("AppliedMaster", "n");
            _writer.WriteAttributeString("OverrideList", "");
            _writer.WriteAttributeString("TabOrder", "");
            _writer.WriteAttributeString("GridStartingPoint", "TopOutside");
            _writer.WriteAttributeString("UseMasterGrid", "true");
            CreateMarginPreferenceforMasterPage();
            _writer.WriteStartElement("GridDataInformation");
            _writer.WriteAttributeString("FontStyle", "Regular");
            _writer.WriteAttributeString("PointSize", "12");
            _writer.WriteAttributeString("CharacterAki", "0");
            _writer.WriteAttributeString("LineAki", "9");
            _writer.WriteAttributeString("HorizontalScale", "100");
            _writer.WriteAttributeString("VerticalScale", "100");
            _writer.WriteAttributeString("LineAlignment", "LeftOrTopLineJustify");
            _writer.WriteAttributeString("GridAlignment", "AlignEmCenter");
            _writer.WriteAttributeString("CharacterAlignment", "AlignEmCenter");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("AppliedFont");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteString("Times New Roman");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        public void CreateMainFrameStaticMethod1(string frameName, string parentStoryName)
        {
            _writer.WriteStartElement("TextFrame");
            _writer.WriteAttributeString("Self", frameName);
            _writer.WriteAttributeString("ParentStory", parentStoryName);
            _writer.WriteAttributeString("PreviousTextFrame", "u104");
            _writer.WriteAttributeString("NextTextFrame", "n");
            _writer.WriteAttributeString("ContentType", "TextType");
            _writer.WriteAttributeString("AllowOverrides", "true");
            _writer.WriteAttributeString("StrokeWeight", "1");
            _writer.WriteAttributeString("GradientFillStart", "0 0");
            _writer.WriteAttributeString("GradientFillLength", "0");
            _writer.WriteAttributeString("GradientFillAngle", "0");
            _writer.WriteAttributeString("GradientStrokeStart", "0 0");
            _writer.WriteAttributeString("GradientStrokeLength", "0");
            _writer.WriteAttributeString("GradientStrokeAngle", "0");
            _writer.WriteAttributeString("ItemLayer", "ub6");
            _writer.WriteAttributeString("Locked", "false");
            _writer.WriteAttributeString("LocalDisplaySetting", "Default");
            _writer.WriteAttributeString("GradientFillHiliteLength", "0");
            _writer.WriteAttributeString("GradientFillHiliteAngle", "0");
            _writer.WriteAttributeString("GradientStrokeHiliteLength", "0");
            _writer.WriteAttributeString("GradientStrokeHiliteAngle", "0");
            _writer.WriteAttributeString("AppliedObjectStyle", "ObjectStyle/$ID/[None]");
            if (frameName.IndexOf("bottom") == 0)
                _writer.WriteAttributeString("ItemTransform", "1 0 0 1 0 -21.25977");//0 -7.5
            else
                _writer.WriteAttributeString("ItemTransform", "1 0 0 1 0 21");//0
        }

        private void CreateMarginPreferenceforMasterPage()
        {
            float top = float.Parse(_cssProperty[_pageClass]["Margin-Top"], CultureInfo.GetCultureInfo("en-US"));
            float bottom = float.Parse(_cssProperty[_pageClass]["Margin-Bottom"], CultureInfo.GetCultureInfo("en-US"));

            _writer.WriteStartElement("MarginPreference");
            _writer.WriteAttributeString("ColumnCount", "1");
            _writer.WriteAttributeString("ColumnGutter", "12");
            _writer.WriteAttributeString("Top", top.ToString());
            _writer.WriteAttributeString("Bottom", bottom.ToString());
            _writer.WriteAttributeString("Left", _cssProperty[_pageClass]["Margin-Left"]);
            _writer.WriteAttributeString("Right", _cssProperty[_pageClass]["Margin-Right"]);
            _writer.WriteAttributeString("ColumnDirection", "Horizontal");
            _writer.WriteAttributeString("ColumnsPositions", "0 " + GetColumnPosition());
            _writer.WriteEndElement();
        }
    }
}
