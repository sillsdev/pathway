// --------------------------------------------------------------------------------------------
// <copyright file="InSpread.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Creates the InDesign Spread file 
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Collections;
using System.Globalization;
using System.Collections.Generic;

namespace SIL.PublishingSolution
{
    public class InSpread : InSpreadBase
    {

        #region Private Variables
        string _pageClass = "@page";
        private int pagesPerSpread = 1;
        private ArrayList _textFrameColumnClass;
        private int _page = 1;
        private bool _useMasterGrid = false;
        Dictionary<string, Dictionary<string, string>> _idAllClass = new Dictionary<string, Dictionary<string, string>>();
        #endregion

        public bool CreateIDSpread(string projectPath, Dictionary<string, Dictionary<string, string>> idAllClass, ArrayList textFrameColumnClass)
        {
            _idAllClass = idAllClass;
            _textFrameColumnClass = textFrameColumnClass;
            int noOfTextFrames = textFrameColumnClass.Count;
            const int pageCount = 3;

            if (_idAllClass.ContainsKey("@page") || _idAllClass.ContainsKey("@page:first") || _idAllClass.ContainsKey("@page:left") || _idAllClass.ContainsKey("@page:right"))
            {
                _useMasterGrid = true;
            }
            for (int spread = 1; spread <= pageCount; spread++)
            {
                CreateaFile(projectPath, spread);
                CreateSpread(spread);
                CreateFlattenerPreference(); // Static code
                CreatePage();
                if (spread == 1)
                {
                    CreateTextFrame(noOfTextFrames);
                }
                CloseFile(); // Static code
                GetPagesPerSpread();
            }
            return true;
        }

        private void GetPagesPerSpread()
        {
            if (_idAllClass["@page"]["FacingPages"] == "true")
            {
                pagesPerSpread = 2;
            }
        }

        private void CreateSpread(int spread)
        {
            _writer.WriteStartElement("Spread");
            _writer.WriteAttributeString("Self", "Spread_" + spread.ToString());
            _writer.WriteAttributeString("PageTransitionType", "None");
            _writer.WriteAttributeString("PageTransitionDirection", "NotApplicable");
            _writer.WriteAttributeString("PageTransitionDuration", "Medium");
            _writer.WriteAttributeString("FlattenerOverride", "Default");
            _writer.WriteAttributeString("ShowMasterItems", "true");
            _writer.WriteAttributeString("PageCount", pagesPerSpread.ToString());
            _writer.WriteAttributeString("BindingLocation", (pagesPerSpread - 1).ToString());
            _writer.WriteAttributeString("AllowPageShuffle", "true");
            _writer.WriteAttributeString("ItemTransform", "1 0 0 1 0 0");
        }

        private void CreatePage()
        {
            int counter;
            for (counter = 1; counter <= pagesPerSpread; counter++)
            {
                GetPageClassName();
                StartPage();
                SetMarginPreferenceForPage();
                EndPage();
                _page++;
            }
            _page = _page - counter + 1;
        }

        private void CreateTextFrame(int noOfTextFrames)
        {
            int counter;
            for (counter = 1; counter <= noOfTextFrames; counter++)
            {
                GetPageClassName();
                StartTextFrame(counter);
                CreatePathPointArray();
                CreateTextFramePreference(_textFrameColumnClass[counter-1].ToString());
                EndTextFrame();
                _page++;
            }
        }

        private void EndTextFrame()
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
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void StartTextFrame(int counter)
        {
            _writer.WriteStartElement("TextFrame");
            _writer.WriteAttributeString("Self", "TF" + counter.ToString());
            _writer.WriteAttributeString("ParentStory", counter.ToString());
            _writer.WriteAttributeString("PreviousTextFrame", "n");
            _writer.WriteAttributeString("NextTextFrame", "n");
            _writer.WriteAttributeString("ContentType", "TextType");
            _writer.WriteAttributeString("GradientFillStart", "0 0");
            _writer.WriteAttributeString("GradientFillLength", "0");
            _writer.WriteAttributeString("GradientFillAngle", "0");
            _writer.WriteAttributeString("GradientStrokeStart", "0 0");
            _writer.WriteAttributeString("GradientStrokeLength", "0");
            _writer.WriteAttributeString("GradientStrokeAngle", "0");
            _writer.WriteAttributeString("ItemLayer", "ua2");
            _writer.WriteAttributeString("Locked", "false");
            _writer.WriteAttributeString("LocalDisplaySetting", "Default");
            _writer.WriteAttributeString("GradientFillHiliteLength", "0");
            _writer.WriteAttributeString("GradientFillHiliteAngle", "0");
            _writer.WriteAttributeString("GradientStrokeHiliteLength", "0");
            _writer.WriteAttributeString("GradientStrokeHiliteAngle", "0");
            _writer.WriteAttributeString("AppliedObjectStyle", "ObjectStyle/$ID/[Normal Text Frame]");
            if (_pageClass == "@page:left")
            {
                _writer.WriteAttributeString("ItemTransform", "1 0 0 1 -306 0");
            }
            else if (_pageClass == "@page:right")
            {
                _writer.WriteAttributeString("ItemTransform", "1 0 0 1 306 0");
            }
            else
            {
            _writer.WriteAttributeString("ItemTransform", "1 0 0 1 0 0");
            }
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("PathGeometry");
            _writer.WriteStartElement("GeometryPathType");
            _writer.WriteAttributeString("PathOpen", "false");
        }

        private void CreatePathPointArray()
        {
            _writer.WriteStartElement("PathPointArray");
            Dictionary<string, string> classValues = _idAllClass[_pageClass];
            float xPos = (float.Parse(classValues["Page-Width"], CultureInfo.GetCultureInfo("en-US")) -
                          (float.Parse(classValues["Margin-Left"], CultureInfo.GetCultureInfo("en-US")) +
                          float.Parse(classValues["Margin-Right"], CultureInfo.GetCultureInfo("en-US")))) / 2;
            float yPos = (float.Parse(classValues["Page-Height"], CultureInfo.GetCultureInfo("en-US")) -
                          (float.Parse(classValues["Margin-Top"], CultureInfo.GetCultureInfo("en-US")) +
                          float.Parse(classValues["Margin-Bottom"], CultureInfo.GetCultureInfo("en-US")))) / 2;
            if (_useMasterGrid)
            {
                yPos = yPos - 35f;
            }

            _writer.WriteStartElement("PathPointType");
            _writer.WriteAttributeString("Anchor", "-" + xPos + " -" + yPos);
            _writer.WriteAttributeString("LeftDirection", "-" + xPos + " -" + yPos);
            _writer.WriteAttributeString("RightDirection", "-" + xPos + " -" + yPos);
            _writer.WriteEndElement();
            _writer.WriteStartElement("PathPointType");
            _writer.WriteAttributeString("Anchor", "-" + xPos + " " + yPos);
            _writer.WriteAttributeString("LeftDirection", "-" + xPos + " " + yPos);
            _writer.WriteAttributeString("RightDirection", "-" + xPos + " " + yPos);
            _writer.WriteEndElement();
            _writer.WriteStartElement("PathPointType");
            _writer.WriteAttributeString("Anchor", xPos + " " + yPos);
            _writer.WriteAttributeString("LeftDirection", xPos + " " + yPos);
            _writer.WriteAttributeString("RightDirection", xPos + " " + yPos);
            _writer.WriteEndElement();
            _writer.WriteStartElement("PathPointType");
            _writer.WriteAttributeString("Anchor", xPos + " -" + yPos);
            _writer.WriteAttributeString("LeftDirection", xPos + " -" + yPos);
            _writer.WriteAttributeString("RightDirection", xPos + " -" + yPos);
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();

        }

        private void CreateTextFramePreference(string columnClass)
        {
            _writer.WriteStartElement("TextFramePreference");
            SetColumnCount(columnClass);
            SetVerticalJustify();
            _writer.WriteAttributeString("TextColumnFixedWidth", "495");
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
            _writer.WriteEndElement(); //End of TextFramePreference
        }

        private void SetVerticalJustify()
        {
            if (_idAllClass.ContainsKey("@page"))
            {

                if (_idAllClass["@page"].ContainsKey("VerticalJustification"))
                {
                    _writer.WriteAttributeString("VerticalJustification", _idAllClass["@page"]["VerticalJustification"]);
                }
            }
        }

        private void SetColumnCount(string columnClass)
        {
            if (_idAllClass.ContainsKey(columnClass))
            {

                if (_idAllClass[columnClass].ContainsKey("TextColumnCount"))
                {
                    _writer.WriteAttributeString("TextColumnCount", _idAllClass[columnClass]["TextColumnCount"]);
                }
                if (_idAllClass[columnClass].ContainsKey("TextColumnGutter"))
                {
                    _writer.WriteAttributeString("TextColumnGutter", _idAllClass[columnClass]["TextColumnGutter"]);
                }
            }
        }

        private void GetPageClassName()
        {
            if (_page == 1)
            {
                _pageClass = "@page:first";
            }
            else if (pagesPerSpread == 2)
            {
                if (_page % 2 == 0)
                {
                    _pageClass = "@page:left";
                }
                else
                {
                    _pageClass = "@page";
                }
            }
            else
            {
                _pageClass = "@page";
            }

        }

        private void EndPage()
        {
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

        private void StartPage()
        {
            _writer.WriteStartElement("Page");
            _writer.WriteAttributeString("Self", "page" + _page);
            _writer.WriteAttributeString("Name", _page.ToString());
            _writer.WriteAttributeString("AppliedTrapPreset", "TrapPreset/$ID/kDefaultTrapStyleName");
            string masterSpreadName = "ua3";
            if (_pageClass == "@page:first")
            {
                masterSpreadName = "MasterFirst";
            }
            else if (_pageClass == "@page")
            {
                masterSpreadName = "MasterAll";
            }
            else if (_pageClass == "@page:left" && (_page % 2 == 0))
            {
                masterSpreadName = "MasterLeft";
            }
            else if (_pageClass == "@page" && (_page % 2 == 1))
            {
                masterSpreadName = "MasterRight";
            }
            _writer.WriteAttributeString("AppliedMaster", masterSpreadName);
            _writer.WriteAttributeString("OverrideList", "");
            _writer.WriteAttributeString("TabOrder", "");
            _writer.WriteAttributeString("GridStartingPoint", "TopOutside");
            _writer.WriteAttributeString("UseMasterGrid", _useMasterGrid.ToString());
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("Descriptor");
            _writer.WriteAttributeString("type", "list");
            _writer.WriteStartElement("ListItem");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ListItem");
            _writer.WriteAttributeString("type", "enumeration");
            _writer.WriteString("Arabic");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ListItem");
            _writer.WriteAttributeString("type", "boolean");
            _writer.WriteString("true");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ListItem");
            _writer.WriteAttributeString("type", "boolean");
            _writer.WriteString("false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ListItem");
            _writer.WriteAttributeString("type", "long");
            _writer.WriteString(_page.ToString());
            _writer.WriteEndElement();
            _writer.WriteStartElement("ListItem");
            _writer.WriteAttributeString("type", "string");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void SetMarginPreferenceForPage()
        {
            Dictionary<string, string> pageProperty = _idAllClass[_pageClass];
            _writer.WriteStartElement("MarginPreference");
            foreach (string propertyName in pageProperty.Keys)
            {
                switch (propertyName)
                {
                    case "Margin-Top":
                        string top = pageProperty[propertyName];
                        if(_useMasterGrid)
                        {
                            top = (float.Parse(_idAllClass[_pageClass]["Margin-Top"], CultureInfo.GetCultureInfo("en-US"))).ToString();
                        }
                        _writer.WriteAttributeString("Top", top);
                        break;
                    case "Margin-Bottom":
                        string bottom = pageProperty[propertyName];
                        if (_useMasterGrid)
                        {
                            bottom = (float.Parse(_idAllClass[_pageClass]["Margin-Bottom"], CultureInfo.GetCultureInfo("en-US"))).ToString();
                        }
                        _writer.WriteAttributeString("Bottom", bottom);
                        break;
                    case "Margin-Left":
                        _writer.WriteAttributeString("Left", pageProperty[propertyName]);
                        break;
                    case "Margin-Right":
                        _writer.WriteAttributeString("Right", pageProperty[propertyName]);
                        break;
                    case "column-gap":
                        _writer.WriteAttributeString("ColumnGutter", "12");
                        break;
                    case "column-count":
                        _writer.WriteAttributeString("TextColumnCount", pageProperty[propertyName]);
                        break;
                    case "Page-Width":
                        float colPosition = float.Parse(pageProperty[propertyName], CultureInfo.GetCultureInfo("en-US")) 
                            - (float.Parse(pageProperty["Margin-Top"], CultureInfo.GetCultureInfo("en-US")) 
                            + float.Parse(pageProperty["Margin-Right"], CultureInfo.GetCultureInfo("en-US")));
                        _writer.WriteAttributeString("ColumnsPositions", "0 " + colPosition);
                        break;
                }
            }
            _writer.WriteAttributeString("ColumnDirection", "Horizontal");
            _writer.WriteEndElement();
        }


    }
}