// --------------------------------------------------------------------------------------------
// <copyright file="InGraphic.cs" from='2009' to='2014' company='SIL International'>
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
// Creates the InDesign Graphic file 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class InGraphic
    {
        #region Private Variables
        XmlTextWriter _writer;
        CssTree _cssTree = new CssTree();
        InMapProperty mapProperty = new InMapProperty();
        Dictionary<string, Dictionary<string, string>> _cssProperty = new Dictionary<string, Dictionary<string, string>>();
        ArrayList _cssBorderColor = new ArrayList();
        #endregion

        #region Constructor
        public InGraphic()
        {

        }
        #endregion


        public bool CreateIDGraphic(string projectPath, Dictionary<string, Dictionary<string, string>> cssProperty, ArrayList cssBorderColor)
        {
            try
            {
                _cssProperty = cssProperty;
                _cssBorderColor = cssBorderColor;
                StartIDGraphic(projectPath);
                CreateColor();
                CreateInk();
                CreatePastedSmoothShade();
                CreateSwatch();
                CreateGradient();
                CreateStrokeStyle();
                EndIDGraphic();

                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return false;
        }

        private void EndIDGraphic()
        {
            _writer.WriteEndElement();
            _writer.WriteEndDocument();
            _writer.Flush();
            _writer.Close();
        }

        private void CreateStrokeStyle()
        {
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/Triple_Stroke");
            _writer.WriteAttributeString("Name", "$ID/Triple_Stroke");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/ThickThinThick");
            _writer.WriteAttributeString("Name", "$ID/ThickThinThick");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/ThinThickThin");
            _writer.WriteAttributeString("Name", "$ID/ThinThickThin");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/ThickThick");
            _writer.WriteAttributeString("Name", "$ID/ThickThick");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/ThickThin");
            _writer.WriteAttributeString("Name", "$ID/ThickThin");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/ThinThick");
            _writer.WriteAttributeString("Name", "$ID/ThinThick");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/ThinThin");
            _writer.WriteAttributeString("Name", "$ID/ThinThin");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/Japanese Dots");
            _writer.WriteAttributeString("Name", "$ID/Japanese Dots");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/White Diamond");
            _writer.WriteAttributeString("Name", "$ID/White Diamond");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/Left Slant Hash");
            _writer.WriteAttributeString("Name", "$ID/Left Slant Hash");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/Right Slant Hash");
            _writer.WriteAttributeString("Name", "$ID/Right Slant Hash");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/Straight Hash");
            _writer.WriteAttributeString("Name", "$ID/Straight Hash");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/Wavy");
            _writer.WriteAttributeString("Name", "$ID/Wavy");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/Canned Dotted");
            _writer.WriteAttributeString("Name", "$ID/Canned Dotted");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/Canned Dashed 3x2");
            _writer.WriteAttributeString("Name", "$ID/Canned Dashed 3x2");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/Canned Dashed 4x4");
            _writer.WriteAttributeString("Name", "$ID/Canned Dashed 4x4");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/Dashed");
            _writer.WriteAttributeString("Name", "$ID/Dashed");
            _writer.WriteEndElement();
            _writer.WriteStartElement("StrokeStyle");
            _writer.WriteAttributeString("Self", "StrokeStyle/$ID/Solid");
            _writer.WriteAttributeString("Name", "$ID/Solid");
            _writer.WriteEndElement();
        }

        private void CreateGradient()
        {
            _writer.WriteStartElement("Gradient");
            _writer.WriteAttributeString("Self", "Gradient/u7e");
            _writer.WriteAttributeString("Type", "Linear");
            _writer.WriteAttributeString("Name", "$ID/");
            _writer.WriteAttributeString("ColorEditable", "true");
            _writer.WriteAttributeString("ColorRemovable", "true");
            _writer.WriteAttributeString("Visible", "false");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteStartElement("GradientStop");
            _writer.WriteAttributeString("Self", "u7eGradientStop0");
            _writer.WriteAttributeString("StopColor", "Color/u7f");
            _writer.WriteAttributeString("Location", "0");
            _writer.WriteEndElement();
            _writer.WriteStartElement("GradientStop");
            _writer.WriteAttributeString("Self", "u7eGradientStop1");
            _writer.WriteAttributeString("StopColor", "Color/Black");
            _writer.WriteAttributeString("Location", "100");
            _writer.WriteAttributeString("Midpoint", "50");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void CreateSwatch()
        {
            _writer.WriteStartElement("Swatch");
            _writer.WriteAttributeString("Self", "Swatch/None");
            _writer.WriteAttributeString("Name", "None");
            _writer.WriteAttributeString("ColorEditable", "false");
            _writer.WriteAttributeString("ColorRemovable", "false");
            _writer.WriteAttributeString("Visible", "true");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
        }

        private void CreatePastedSmoothShade()
        {
            _writer.WriteStartElement("PastedSmoothShade");
            _writer.WriteAttributeString("Self", "PastedSmoothShade/u80");
            _writer.WriteAttributeString("ContentsVersion", "0");
            _writer.WriteAttributeString("ContentsType", "ConstantShade");
            _writer.WriteAttributeString("SpotColorList", "");
            _writer.WriteAttributeString("ContentsEncoding", "Ascii64Encoding");
            _writer.WriteAttributeString("ContentsMatrix", "1 0 0 1 0 0");
            _writer.WriteAttributeString("Name", "$ID/");
            _writer.WriteAttributeString("ColorEditable", "true");
            _writer.WriteAttributeString("ColorRemovable", "true");
            _writer.WriteAttributeString("Visible", "false");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteStartElement("Properties");
            _writer.WriteStartElement("Contents");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }

        private void CreateInk()
        {
            _writer.WriteStartElement("Ink");
            _writer.WriteAttributeString("Self", "Ink/$ID/Process Cyan");
            _writer.WriteAttributeString("Name", "$ID/Process Cyan");
            _writer.WriteAttributeString("Angle", "75");
            _writer.WriteAttributeString("ConvertToProcess", "false");
            _writer.WriteAttributeString("Frequency", "70");
            _writer.WriteAttributeString("NeutralDensity", "0.61");
            _writer.WriteAttributeString("PrintInk", "true");
            _writer.WriteAttributeString("TrapOrder", "1");
            _writer.WriteAttributeString("InkType", "Normal");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Ink");
            _writer.WriteAttributeString("Self", "Ink/$ID/Process Magenta");
            _writer.WriteAttributeString("Name", "$ID/Process Magenta");
            _writer.WriteAttributeString("Angle", "15");
            _writer.WriteAttributeString("ConvertToProcess", "false");
            _writer.WriteAttributeString("Frequency", "70");
            _writer.WriteAttributeString("NeutralDensity", "0.76");
            _writer.WriteAttributeString("PrintInk", "true");
            _writer.WriteAttributeString("TrapOrder", "2");
            _writer.WriteAttributeString("InkType", "Normal");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Ink");
            _writer.WriteAttributeString("Self", "Ink/$ID/Process Yellow");
            _writer.WriteAttributeString("Name", "$ID/Process Yellow");
            _writer.WriteAttributeString("Angle", "0");
            _writer.WriteAttributeString("ConvertToProcess", "false");
            _writer.WriteAttributeString("Frequency", "70");
            _writer.WriteAttributeString("NeutralDensity", "0.16");
            _writer.WriteAttributeString("PrintInk", "true");
            _writer.WriteAttributeString("TrapOrder", "3");
            _writer.WriteAttributeString("InkType", "Normal");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Ink");
            _writer.WriteAttributeString("Self", "Ink/$ID/Process Black");
            _writer.WriteAttributeString("Name", "$ID/Process Black");
            _writer.WriteAttributeString("Angle", "45");
            _writer.WriteAttributeString("ConvertToProcess", "false");
            _writer.WriteAttributeString("Frequency", "70");
            _writer.WriteAttributeString("NeutralDensity", "1.7");
            _writer.WriteAttributeString("PrintInk", "true");
            _writer.WriteAttributeString("TrapOrder", "4");
            _writer.WriteAttributeString("InkType", "Normal");
            _writer.WriteEndElement();
        }

        private void CreateColor()
        {
            CreateColorStatic();
            foreach (string className in _cssProperty.Keys)
            {
                Dictionary<string, string> classValues = _cssProperty[className];
                if (classValues.ContainsKey("color"))
                {
                    CreateColorwithValues(classValues["color"]);
                }
                if (classValues.ContainsKey("background-color"))
                {
                    CreateColorwithValues(classValues["background-color"]);
                }
                if (classValues.ContainsKey("column-rule-color"))
                {
                    CreateColorwithValues(classValues["column-rule-color"]);
                }
                if (_cssBorderColor.Count > 0)
                {
                    foreach (string colorValue in _cssBorderColor)
                    {
                        CreateColorwithValues(colorValue);
                    }
                    _cssBorderColor.Clear();
                }
            }
        }

        private void CreateColorwithValues(string colorValues)
        {
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/" + colorValues);
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "RGB");
            _writer.WriteAttributeString("ColorValue", mapProperty.ConvertHexToDec(colorValues));
            _writer.WriteAttributeString("ColorOverride", "Specialblack");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", colorValues);
            _writer.WriteAttributeString("ColorEditable", "true");
            _writer.WriteAttributeString("ColorRemovable", "true");
            _writer.WriteAttributeString("Visible", "true");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
        }


        private void CreateColorStatic()
        {
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/Black");
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "0 0 0 100");
            _writer.WriteAttributeString("ColorOverride", "Specialblack");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "Black");
            _writer.WriteAttributeString("ColorEditable", "false");
            _writer.WriteAttributeString("ColorRemovable", "false");
            _writer.WriteAttributeString("Visible", "true");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/C=0 M=0 Y=100 K=0");
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "0 0 100 0");
            _writer.WriteAttributeString("ColorOverride", "Normal");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "C=0 M=0 Y=100 K=0");
            _writer.WriteAttributeString("ColorEditable", "true");
            _writer.WriteAttributeString("ColorRemovable", "true");
            _writer.WriteAttributeString("Visible", "true");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/C=0 M=100 Y=0 K=0");
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "0 100 0 0");
            _writer.WriteAttributeString("ColorOverride", "Normal");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "C=0 M=100 Y=0 K=0");
            _writer.WriteAttributeString("ColorEditable", "true");
            _writer.WriteAttributeString("ColorRemovable", "true");
            _writer.WriteAttributeString("Visible", "true");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/C=100 M=0 Y=0 K=0");
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "100 0 0 0");
            _writer.WriteAttributeString("ColorOverride", "Normal");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "C=100 M=0 Y=0 K=0");
            _writer.WriteAttributeString("ColorEditable", "true");
            _writer.WriteAttributeString("ColorRemovable", "true");
            _writer.WriteAttributeString("Visible", "true");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/C=100 M=90 Y=10 K=0");
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "100 90 10 0");
            _writer.WriteAttributeString("ColorOverride", "Normal");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "C=100 M=90 Y=10 K=0");
            _writer.WriteAttributeString("ColorEditable", "true");
            _writer.WriteAttributeString("ColorRemovable", "true");
            _writer.WriteAttributeString("Visible", "true");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/C=15 M=100 Y=100 K=0");
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "15 100 100 0");
            _writer.WriteAttributeString("ColorOverride", "Normal");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "C=15 M=100 Y=100 K=0");
            _writer.WriteAttributeString("ColorEditable", "true");
            _writer.WriteAttributeString("ColorRemovable", "true");
            _writer.WriteAttributeString("Visible", "true");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/C=75 M=5 Y=100 K=0");
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "75 5 100 0");
            _writer.WriteAttributeString("ColorOverride", "Normal");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "C=75 M=5 Y=100 K=0");
            _writer.WriteAttributeString("ColorEditable", "true");
            _writer.WriteAttributeString("ColorRemovable", "true");
            _writer.WriteAttributeString("Visible", "true");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/Cyan");
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "100 0 0 0");
            _writer.WriteAttributeString("ColorOverride", "Hiddenreserved");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "Cyan");
            _writer.WriteAttributeString("ColorEditable", "false");
            _writer.WriteAttributeString("ColorRemovable", "false");
            _writer.WriteAttributeString("Visible", "false");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/Magenta");
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "0 100 0 0");
            _writer.WriteAttributeString("ColorOverride", "Hiddenreserved");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "Magenta");
            _writer.WriteAttributeString("ColorEditable", "false");
            _writer.WriteAttributeString("ColorRemovable", "false");
            _writer.WriteAttributeString("Visible", "false");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/Paper");
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "0 0 0 0");
            _writer.WriteAttributeString("ColorOverride", "Specialpaper");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "Paper");
            _writer.WriteAttributeString("ColorEditable", "true");
            _writer.WriteAttributeString("ColorRemovable", "false");
            _writer.WriteAttributeString("Visible", "true");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/Registration");
            _writer.WriteAttributeString("Model", "Registration");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "100 100 100 100");
            _writer.WriteAttributeString("ColorOverride", "Specialregistration");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "Registration");
            _writer.WriteAttributeString("ColorEditable", "false");
            _writer.WriteAttributeString("ColorRemovable", "false");
            _writer.WriteAttributeString("Visible", "true");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/Yellow");
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "0 0 100 0");
            _writer.WriteAttributeString("ColorOverride", "Hiddenreserved");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "Yellow");
            _writer.WriteAttributeString("ColorEditable", "false");
            _writer.WriteAttributeString("ColorRemovable", "false");
            _writer.WriteAttributeString("Visible", "false");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/u7d");
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "0 0 0 100");
            _writer.WriteAttributeString("ColorOverride", "Normal");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "$ID/");
            _writer.WriteAttributeString("ColorEditable", "true");
            _writer.WriteAttributeString("ColorRemovable", "true");
            _writer.WriteAttributeString("Visible", "false");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
            _writer.WriteStartElement("Color");
            _writer.WriteAttributeString("Self", "Color/u7f");
            _writer.WriteAttributeString("Model", "Process");
            _writer.WriteAttributeString("Space", "CMYK");
            _writer.WriteAttributeString("ColorValue", "0 0 0 0");
            _writer.WriteAttributeString("ColorOverride", "Normal");
            _writer.WriteAttributeString("AlternateSpace", "NoAlternateColor");
            _writer.WriteAttributeString("AlternateColorValue", "");
            _writer.WriteAttributeString("Name", "$ID/");
            _writer.WriteAttributeString("ColorEditable", "true");
            _writer.WriteAttributeString("ColorRemovable", "true");
            _writer.WriteAttributeString("Visible", "false");
            _writer.WriteAttributeString("SwatchCreatorID", "7937");
            _writer.WriteEndElement();
        }

        private void StartIDGraphic(string projectPath)
        {
            string styleXMLWithPath = Common.PathCombine(projectPath, "Graphic.xml");
            _writer = new XmlTextWriter(styleXMLWithPath, null) { Formatting = Formatting.Indented };
            _writer.WriteStartDocument();
            _writer.WriteStartElement("idPkg:Graphic");
            _writer.WriteAttributeString("xmlns:idPkg", "http://ns.adobe.com/AdobeInDesign/idml/1.0/packaging");
            _writer.WriteAttributeString("DOMVersion", "6.0");
        }
    }
}
