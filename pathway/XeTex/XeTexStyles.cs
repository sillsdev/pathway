// --------------------------------------------------------------------------------------------
// <copyright file="InStyles.cs" from='2009' to='2010' company='SIL International'>
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
// Creates the InDesign Styles file 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class XeTexStyles : XeTexStyleBase
    {
        #region Private Variables

        //XmlTextWriter _writer;

        Dictionary<string, Dictionary<string, string>> _cssProperty = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, Dictionary<string, string>> _IDAllClass = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, string> _IDProperty = new Dictionary<string, string>();
        private Dictionary<string, string> _IDClass = new Dictionary<string, string>();
        //CSSTree _cssTree = new CSSTree();
        XeTexMapProperty mapProperty = new XeTexMapProperty();
        private StreamWriter _xetexFile;
        private List<string> _inlineStyle;
        Dictionary<string, List<string>> _classInlineStyle = new Dictionary<string, List<string>>();
        //public InDesignStyles InDesignStyles;
        //public ArrayList _FootNote;
        #endregion


        public Dictionary<string, List<string>> CreateXeTexStyles(string projectPath, StreamWriter xetexFile, Dictionary<string, Dictionary<string, string>> cssProperty)
        {
            try
            {
                _xetexFile = xetexFile;
                _cssProperty = cssProperty;
                CreateStyle();  // CODE HERE
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            //return _IDAllClass;
            return _classInlineStyle;
        }

        private void CreateStyle()
        {
            //\font\hoefler="Hoefler Text/B:Letter Case=Small Caps" at 12pt
            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _cssProperty)
            {
                _inlineStyle = new List<string>();
                string xeTexProperty = mapProperty.XeTexProperty(cssClass.Value, cssClass.Key, _inlineStyle);
                if (_inlineStyle.Count > 0)
                {
                    _classInlineStyle[cssClass.Key] = _inlineStyle;
                }
                if (xeTexProperty.Trim().Length > 0)
                {
                    if(cssClass.Key == "letter")
                    _xetexFile.WriteLine(xeTexProperty);
                }
                //_IDClass = new Dictionary<string, string>(); // note: ToDo seperate the process
                //_IDAllClass[cssClass.Key] = _IDClass;

                //_xetexFile.WriteLine("nopagenumbers");

                //foreach (KeyValuePair<string, string> property in _IDProperty)
                //{
                //    if (property.Key == "AppliedFont")
                //    {
                //        _IDClass[property.Key] = property.Value;
                //        continue;
                //    }
                //    if (property.Key == "StrokeColor")
                //    {
                //        _IDClass[property.Key] = property.Value;
                //        InsertBackgroundColor(property.Value);
                //    }
                //    else
                //    {
                //        _IDClass[property.Key] = property.Value;
                //        _writer.WriteAttributeString(property.Key, property.Value);
                //    }
                //}

            }
            //_writer.WriteEndElement(); //End RootParagraphStyleGroup
        }

        private void DeleteRelativeInFootnote(KeyValuePair<string, Dictionary<string, string>> cssClass)
        {
            if (cssClass.Key.IndexOf("..footnote-cal") > 0 || cssClass.Key.IndexOf("..footnote-marker") > 0)
            {
                ArrayList removeProperty = new ArrayList();
                foreach (KeyValuePair<string, string> property in _IDProperty)
                {
                    if (property.Value.IndexOf("%") > 0)
                    {
                        removeProperty.Add(property.Key);
                    }
                }
                foreach (string property in removeProperty)
                {
                    _IDProperty.Remove(property);
                }
            }
        }

        private void PositionProperty()
        {
            //Note: Paragraph Margins are not Completed.
            //Note: Currently "left" and "right" are added to padding because of Indesign 
            //Note: which does not have property for Paragraph Margin.
            //Note: Also It does not support the Negative Values in Margin. Everything restricted within the frames.

                if (_IDProperty.ContainsKey("position") && (_IDProperty.ContainsKey("left") || _IDProperty.ContainsKey("right")) )
                {
                    _IDProperty.Remove("position");

                    if (_IDProperty.ContainsKey("left"))
                    {
                        if (_IDProperty.ContainsKey("LeftIndent"))
                        {
                            _IDProperty["LeftIndent"] = (int.Parse(_IDProperty["LeftIndent"]) 
                                                        + int.Parse(_IDProperty["left"])).ToString();
                        }
                        else
                        {
                            _IDProperty["LeftIndent"] = _IDProperty["left"];
                        }
                    }
                    else if (_IDProperty.ContainsKey("right"))
                    {
                        if (_IDProperty.ContainsKey("RightIndent"))
                        {
                            _IDProperty["RightIndent"] = (int.Parse(_IDProperty["RightIndent"]) 
                                                         + int.Parse(_IDProperty["right"])).ToString();
                        }
                        else
                        {
                            _IDProperty["RightIndent"] = _IDProperty["right"];
                        }
                    }
                }

                if (_IDProperty.ContainsKey("position"))
                {
                    _IDProperty.Remove("position");
                }
                if (_IDProperty.ContainsKey("left"))
                {
                    _IDProperty.Remove("left");
                }
                if (_IDProperty.ContainsKey("right"))
                {
                    _IDProperty.Remove("right");
                }
        }

        private void CreateParagraphProperty(string propertyName, string propertyType)
        {
            if (_IDProperty.ContainsKey(propertyName))
            {
                _writer.WriteStartElement(propertyName);
                _writer.WriteAttributeString("type", propertyType);
                _writer.WriteString(_IDProperty[propertyName]);
                _writer.WriteEndElement();
            }
        }

        private void InsertBackgroundColor(string propertyValue)
        {
            _IDClass["StrokeWeight"] = "1";
            _IDClass["StrokeColor"] = propertyValue;
            _IDClass["EndJoin"] = "BevelEndJoin";

            _writer.WriteAttributeString("StrokeWeight", "1");
            _writer.WriteAttributeString("StrokeColor", propertyValue);
            _writer.WriteAttributeString("EndJoin", "BevelEndJoin");
        }

        /// <summary>
        /// Increase font-size 250% for Subscript and Superscript
        /// </summary>
        /// <param name="isIncrease">to increase font-size even the property is not super/sub script</param>
        private void SuperscriptSubscriptIncreaseFontSize(bool isIncrease)
        {
            bool isSuperSub = false;
            if (_IDProperty.ContainsKey("Position") && (_IDProperty["Position"] == "Subscript" || _IDProperty["Position"] == "Superscript"))
            {
                isSuperSub = true;
            }

            if (isSuperSub || isIncrease) // increase font-size for superscipt & subscript
            {
                string newValue = "100%";
                if (_IDProperty.ContainsKey("PointSize"))
                {
                    string fontValue = _IDProperty["PointSize"];
                    int counter;
                    string retValue = Common.GetNumericChar(fontValue, out counter);
                    if (retValue.Length > 0)
                    {
                        float value = float.Parse(retValue) * 1.0F;
                        string unit = fontValue.Substring(counter);
                        newValue = value + unit;
                    }
                    else
                    {
                        if (fontValue == "larger" || fontValue == "smaller")
                        {
                            newValue = fontValue;
                        }
                    }
                }
                _IDProperty["PointSize"] = newValue;
            }
        }

    }
}