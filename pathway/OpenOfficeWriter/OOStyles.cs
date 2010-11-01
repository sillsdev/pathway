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
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class OOStyles : OOStyleBase
    {
        Dictionary<string, Dictionary<string, string>> _cssProperty = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, Dictionary<string, string>> _OOAllClass = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> _OOClass = new Dictionary<string, string>();
        private Dictionary<string, string> _OOProperty = new Dictionary<string, string>();
        OOMapProperty mapProperty = new OOMapProperty();

        public Dictionary<string, Dictionary<string, string>> CreateStyles(PublicationInformation projInfo, Dictionary<string, Dictionary<string, string>> cssProperty, string outputFileName)
        {
            
            try
            {
                string outputFile = Path.GetFileNameWithoutExtension(projInfo.DefaultXhtmlFileWithPath);
                string strStylePath = Common.PathCombine(projInfo.TempOutputFolder, outputFileName);
                _cssProperty = cssProperty;
                InitializeObject(outputFile); // Creates new Objects
                LoadAllProperty();  // Loads all properties
                LoadSpellCheck();
                CreateODTStyles(strStylePath); 
                CreateCssStyle(); // Code Here
                CreatePageStyle();

                //var cssTree = new CssParser();
                //TreeNode node = cssTree.BuildTree(sourceFile);
                ////To show errors to user to edit and save the CSS file.
                ////if (cssTree.ErrorList.Count > 0)
                ////{
                ////    var errForm = new CSSError(cssTree.ErrorList, Path.GetDirectoryName(sourceFile));
                ////    errForm.ShowDialog();
                ////    cssTree = new CSSParser();
                ////    node = cssTree.BuildTree(sourceFile);
                ////}
                /// 
                AddTagStyle(); // Add missing tags in styles.xml (h1,h2,..)
                CloseODTStyles();  // Close Styles.xml for odt
                //MergeTag(); // Merge tags in styles.xml (h1,h2,..)
               
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return _OOAllClass;
        }

        private void CreatePageStyle()
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _cssProperty)
            {
                if(!cssClass.Key.StartsWith("@page"))
                {
                    continue;
                }

            }
        }

        /// <summary>
        //  <style:style style:name="a" style:family="paragraph" style:parent-style-name="none"> *
        //  <style:paragraph-properties fo:text-align="left" /> 
        //  <style:text-properties fo:font-size="24pt" style:font-size-complex="24pt" fo:color="#ff0000" />
        //  </style:style>
        /// </summary>
        private void CreateCssStyle()
        {

            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _cssProperty)
            {
                if (cssClass.Key.StartsWith("@page"))
                {
                    continue;
                }

                string className = cssClass.Key;
                string familyType = "paragraph";
                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", className);
                _writer.WriteAttributeString("style:family", familyType); // "paragraph" will override by ContentXML.cs
                _writer.WriteAttributeString("style:parent-style-name", "none");

                _paragraphProperty.Clear();
                _textProperty.Clear();
                _OOClass = new Dictionary<string, string>();
                _OOProperty = mapProperty.IDProperty(cssClass.Value);
                foreach (KeyValuePair<string, string> property in _OOProperty)
                {
                    _OOClass[property.Key] = property.Value; // for future usage

                    string propName = property.Key;
                    if (_allParagraphProperty.ContainsKey(propName))
                    {
                        if (!_paragraphProperty.ContainsKey(property.Key))
                        {
                            string prefix = _allParagraphProperty[property.Key].ToString();
                            _paragraphProperty[prefix + property.Key] = property.Value;
                        }
                    }
                    else if (_allTextProperty.ContainsKey(propName))
                    {
                        if (!_textProperty.ContainsKey(property.Key))
                        {
                            string prefix = _allTextProperty[property.Key].ToString();
                            _textProperty[prefix + property.Key] = property.Value;

                        }
                    }
                }
                _OOAllClass[cssClass.Key] = _OOClass;

                if (_paragraphProperty.Count > 0)
                {
                    _writer.WriteStartElement("style:paragraph-properties");
                    foreach (KeyValuePair<string, string> para in _paragraphProperty)
                    {
                        _writer.WriteAttributeString(para.Key, para.Value);
                    }
                    _writer.WriteEndElement();
                }

                if (_textProperty.Count > 0)
                {
                    _writer.WriteStartElement("style:text-properties");
                    foreach (KeyValuePair<string, string> text in _textProperty)
                    {
                        _writer.WriteAttributeString(text.Key, text.Value);
                    }
                    if (_paragraphProperty.ContainsKey("fo:background-color"))
                    {
                        _writer.WriteAttributeString("fo:background-color", _paragraphProperty["fo:background-color"]);
                    }
                    _writer.WriteEndElement();
                }
                _writer.WriteEndElement();
                
            }
        }

        ///// -------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Generate last block of Styles.xml
        ///// 
        ///// <list> 
        ///// </list>
        ///// </summary>
        ///// <returns> </returns>
        ///// -------------------------------------------------------------------------------------------
        //private void CloseODTStyles()
        //{
        //    try
        //    {
        //        //All PageProperty information is assinged to First PageProperty, when First PageProperty is not found
        //        for (int i = 0; i <= 5; i++)
        //        {
        //            if (_firstPageContentNone.Contains(i))
        //            {
        //                continue; // no need of copy. for content : normal or none;
        //            }

        //            if (_pageHeaderFooter[i].Count == 0) // Only copy @page values when equivalent element is empty in @PageProperty:first
        //            {
        //                _pageHeaderFooter[i] = _pageHeaderFooter[i + 6];
        //            }
        //        }

        //        // "graphic"
        //        _writer.WriteStartElement("style:default-style");
        //        _writer.WriteAttributeString("style:family", "graphic");
        //        _writer.WriteStartElement("style:graphic-properties");
        //        _writer.WriteAttributeString("draw:shadow-offset-x", "0.1181in");
        //        _writer.WriteAttributeString("draw:shadow-offset-y", "0.1181in");
        //        _writer.WriteAttributeString("draw:start-line-spacing-horizontal", "0.1114in");
        //        _writer.WriteAttributeString("draw:start-line-spacing-vertical", "0.1114in");
        //        _writer.WriteAttributeString("draw:end-line-spacing-horizontal", "0.1114in");
        //        _writer.WriteAttributeString("draw:end-line-spacing-vertical", "0.1114in");
        //        //_writer.WriteAttributeString("style:flow-with-text", "false");
        //        _writer.WriteAttributeString("style:flow-with-text", "true");
        //        _writer.WriteEndElement();

        //        _writer.WriteStartElement("style:paragraph-properties");
        //        _writer.WriteAttributeString("style:text-autospace", "ideograph-alpha");
        //        _writer.WriteAttributeString("style:line-break", "strict");
        //        _writer.WriteAttributeString("style:writing-mode", "lr-tb");
        //        _writer.WriteAttributeString("style:font-independent-line-spacing", "false");

        //        _writer.WriteStartElement("style:tab-stops");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();

        //        _writer.WriteStartElement("style:text-properties");
        //        _writer.WriteAttributeString("style:use-window-font-color", "true");
        //        _writer.WriteAttributeString("fo:font-size", "12pt");
        //        //_writer.WriteAttributeString("fo:language", "en");
        //        //_writer.WriteAttributeString("fo:country", "US");
        //        _writer.WriteAttributeString("fo:language", "none");
        //        _writer.WriteAttributeString("fo:country", "none");
        //        _writer.WriteAttributeString("style:letter-kerning", "true");
        //        _writer.WriteAttributeString("style:font-size-asian", "12pt");
        //        _writer.WriteAttributeString("style:language-asian", "zxx");
        //        _writer.WriteAttributeString("style:country-asian", "none");
        //        _writer.WriteAttributeString("style:font-size-complex", "12pt");
        //        _writer.WriteAttributeString("style:language-complex", "zxx");
        //        _writer.WriteAttributeString("style:country-complex", "none");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();

        //        //office:styles Attributes.
        //        //// "paragraph"
        //        _writer.WriteStartElement("style:default-style");
        //        _writer.WriteAttributeString("style:family", "paragraph");
        //        _writer.WriteStartElement("style:paragraph-properties");
        //        _writer.WriteAttributeString("fo:hyphenation-ladder-count", "no-limit");
        //        _writer.WriteAttributeString("style:text-autospace", "ideograph-alpha");
        //        _writer.WriteAttributeString("style:punctuation-wrap", "hanging");
        //        _writer.WriteAttributeString("style:line-break", "strict");
        //        _writer.WriteAttributeString("style:tab-stop-distance", "0.4925in");
        //        _writer.WriteAttributeString("style:writing-mode", "page");
        //        _writer.WriteEndElement();

        //        _writer.WriteStartElement("style:text-properties");
        //        _writer.WriteAttributeString("style:use-window-font-color", "true");
        //        _writer.WriteAttributeString("style:font-name", "Times New Roman");
        //        _writer.WriteAttributeString("fo:font-size", "12pt");
        //        //_writer.WriteAttributeString("fo:language", "en");
        //        //_writer.WriteAttributeString("fo:country", "US");
        //        _writer.WriteAttributeString("fo:language", "none");
        //        _writer.WriteAttributeString("fo:country", "none");
        //        _writer.WriteAttributeString("style:letter-kerning", "true");
        //        _writer.WriteAttributeString("style:font-name-asian", "Yi plus Phonetics");
        //        _writer.WriteAttributeString("style:font-size-asian", "12pt");
        //        _writer.WriteAttributeString("style:language-asian", "none");
        //        _writer.WriteAttributeString("style:country-asian", "none");
        //        _writer.WriteAttributeString("style:font-name-complex", "Tahoma");
        //        _writer.WriteAttributeString("style:font-size-complex", "12pt");
        //        _writer.WriteAttributeString("style:language-complex", "zxx");
        //        _writer.WriteAttributeString("style:country-complex", "none");
        //        _writer.WriteAttributeString("fo:hyphenate", "false");
        //        _writer.WriteAttributeString("fo:hyphenation-remain-char-count", "2");
        //        _writer.WriteAttributeString("fo:hyphenation-push-char-count", "2");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();

        //        //office:styles Attributes.
        //        //// "table"
        //        _writer.WriteStartElement("style:default-style");
        //        _writer.WriteAttributeString("style:family", "table");
        //        _writer.WriteStartElement("style:table-properties");
        //        _writer.WriteAttributeString("table:border-model", "collapsing");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();

        //        //office:styles Attributes.
        //        //// "table-row"
        //        _writer.WriteStartElement("style:default-style");
        //        _writer.WriteAttributeString("style:family", "table-row");
        //        _writer.WriteStartElement("style:table-row-properties");
        //        _writer.WriteAttributeString("fo:keep-together", "auto");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();

        //        //office:styles Attributes.
        //        //// "Standard"
        //        _writer.WriteStartElement("style:style");
        //        _writer.WriteAttributeString("style:name", "Standard");
        //        _writer.WriteAttributeString("style:family", "paragraph");
        //        _writer.WriteAttributeString("style:class", "text");
        //        _writer.WriteEndElement();

        //        //office:styles Attributes.
        //        //// "Header"
        //        _writer.WriteStartElement("style:style");
        //        _writer.WriteAttributeString("style:name", "Header");
        //        _writer.WriteAttributeString("style:family", "paragraph");
        //        _writer.WriteAttributeString("style:parent-style-name", "Standard");
        //        _writer.WriteAttributeString("style:next-style-name", "Text_20_body");
        //        _writer.WriteAttributeString("style:class", "text");

        //        _writer.WriteStartElement("style:paragraph-properties");//style:paragraph-properties
        //        _writer.WriteAttributeString("fo:margin-top", "0.0005in");
        //        _writer.WriteAttributeString("fo:margin-bottom", "0.0835in");
        //        _writer.WriteAttributeString("fo:keep-with-next", "always");

        //        InsertHeaderRule();

        //        _writer.WriteAttributeString("text:number-lines", "false");
        //        _writer.WriteAttributeString("text:line-number", "0");

        //        _writer.WriteStartElement("style:tab-stops");//style:tab-stops
        //        _writer.WriteStartElement("style:tab-stop");//style:tab-stop

        //        // Finds the PageProperty Header's center.(GuideWord or PageNo)
        //        float borderLeft;
        //        if (_pageLayoutProperty.ContainsKey("fo:border-left"))
        //        {
        //            string[] parameters = _pageLayoutProperty["fo:border-left"].Split(' ');
        //            string pageBorderLeft = "0pt";
        //            foreach (string param in parameters)
        //            {
        //                if (Common.ValidateNumber(param[0].ToString() + "1"))
        //                {
        //                    pageBorderLeft = param;
        //                    break;
        //                }
        //            }
        //            borderLeft = Common.ConvertToInch(pageBorderLeft);
        //        }
        //        else
        //        {
        //            borderLeft = 0F;
        //        }

        //        float width = Common.ConvertToInch(_pageLayoutProperty["fo:page-width"]);
        //        float left = Common.ConvertToInch(_pageLayoutProperty["fo:margin-left"]);
        //        float right = Common.ConvertToInch(_pageLayoutProperty["fo:margin-left"]);
        //        float mid = width / 2F - left - borderLeft;
        //        float rightGuide = (width - left - right);

        //        _writer.WriteAttributeString("style:position", mid.ToString() + "in");

        //        _writer.WriteAttributeString("style:type", "center");
        //        _writer.WriteEndElement();

        //        _writer.WriteStartElement("style:tab-stop");//style:tab-stop
        //        _writer.WriteAttributeString("style:position", rightGuide.ToString() + "in");
        //        _writer.WriteAttributeString("style:type", "right");
        //        _writer.WriteEndElement();

        //        _writer.WriteEndElement();//style:tab-stops
        //        _writer.WriteEndElement();//style:paragraph-properties
        //        _writer.WriteEndElement();


        //        //// "Footer"
        //        _writer.WriteStartElement("style:style");
        //        _writer.WriteAttributeString("style:name", "Footer");
        //        _writer.WriteAttributeString("style:family", "paragraph");
        //        _writer.WriteAttributeString("style:parent-style-name", "Standard");
        //        _writer.WriteAttributeString("style:next-style-name", "Text_20_body");
        //        _writer.WriteAttributeString("style:class", "text");

        //        _writer.WriteStartElement("style:paragraph-properties");//style:paragraph-properties
        //        _writer.WriteAttributeString("fo:margin-top", "0.0005in");
        //        _writer.WriteAttributeString("fo:margin-bottom", "0.0835in");
        //        _writer.WriteAttributeString("fo:keep-with-next", "always");

        //        _writer.WriteAttributeString("text:number-lines", "false");
        //        _writer.WriteAttributeString("text:line-number", "0");

        //        _writer.WriteStartElement("style:tab-stops");//style:tab-stops

        //        _writer.WriteStartElement("style:tab-stop");//style:tab-stop
        //        _writer.WriteAttributeString("style:position", mid.ToString() + "in");
        //        _writer.WriteAttributeString("style:type", "center");
        //        _writer.WriteEndElement();

        //        _writer.WriteStartElement("style:tab-stop");//style:tab-stop
        //        _writer.WriteAttributeString("style:position", rightGuide.ToString() + "in");
        //        _writer.WriteAttributeString("style:type", "right");
        //        _writer.WriteEndElement();

        //        _writer.WriteEndElement();//style:tab-stops
        //        _writer.WriteEndElement();//style:paragraph-properties
        //        _writer.WriteEndElement();//Footer style

        //        //office:styles Attributes.
        //        //// "Text_20_body"
        //        _writer.WriteStartElement("style:style");
        //        _writer.WriteAttributeString("style:name", "Text_20_body");
        //        _writer.WriteAttributeString("style:display-name", "Text body");
        //        _writer.WriteAttributeString("style:family", "paragraph");
        //        _writer.WriteAttributeString("style:parent-style-name", "Standard");
        //        _writer.WriteAttributeString("style:class", "text");
        //        _writer.WriteStartElement("style:paragraph-properties");
        //        _writer.WriteAttributeString("fo:margin-top", "0in");
        //        _writer.WriteAttributeString("fo:margin-bottom", "0.0835in");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();

        //        //office:styles Attributes.
        //        //// "List"
        //        _writer.WriteStartElement("style:style");
        //        _writer.WriteAttributeString("style:name", "List");
        //        _writer.WriteAttributeString("style:family", "paragraph");
        //        _writer.WriteAttributeString("style:parent-style-name", "Text_20_body");
        //        _writer.WriteAttributeString("style:class", "list");
        //        _writer.WriteStartElement("style:text-properties");
        //        _writer.WriteAttributeString("style:font-name-asian", "Yi plus Phonetics");
        //        _writer.WriteAttributeString("style:font-name-complex", "Tahoma1");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();

        //        //office:styles Attributes.
        //        //// "Caption"
        //        _writer.WriteStartElement("style:style");
        //        _writer.WriteAttributeString("style:name", "Caption");
        //        _writer.WriteAttributeString("style:family", "paragraph");
        //        _writer.WriteAttributeString("style:parent-style-name", "Standard");
        //        _writer.WriteAttributeString("style:class", "extra");
        //        _writer.WriteStartElement("style:paragraph-properties");
        //        _writer.WriteAttributeString("fo:margin-top", "0.0835in");
        //        _writer.WriteAttributeString("fo:margin-bottom", "0.0835in");
        //        _writer.WriteAttributeString("text:number-lines", "false");
        //        _writer.WriteAttributeString("text:line-number", "0");
        //        _writer.WriteEndElement();
        //        _writer.WriteStartElement("style:text-properties");
        //        _writer.WriteAttributeString("fo:font-size", "12pt");
        //        _writer.WriteAttributeString("fo:font-style", "italic");
        //        _writer.WriteAttributeString("style:font-name-asian", "Yi plus Phonetics");
        //        _writer.WriteAttributeString("style:font-size-asian", "12pt");
        //        _writer.WriteAttributeString("style:font-style-asian", "italic");
        //        _writer.WriteAttributeString("style:font-name-complex", "Tahoma1");
        //        _writer.WriteAttributeString("style:font-size-complex", "12pt");
        //        _writer.WriteAttributeString("style:font-style-complex", "italic");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();

        //        //office:styles Attributes.
        //        //// "index"
        //        _writer.WriteStartElement("style:style");
        //        _writer.WriteAttributeString("style:name", "Index");
        //        _writer.WriteAttributeString("style:family", "paragraph");
        //        _writer.WriteAttributeString("style:parent-style-name", "Standard");
        //        _writer.WriteAttributeString("style:class", "index");
        //        _writer.WriteStartElement("style:paragraph-properties");
        //        _writer.WriteAttributeString("text:number-lines", "false");
        //        _writer.WriteAttributeString("text:line-number", "0");
        //        _writer.WriteEndElement();
        //        _writer.WriteStartElement("style:text-properties");
        //        _writer.WriteAttributeString("style:font-name-asian", "Yi plus Phonetics");
        //        _writer.WriteAttributeString("style:font-name-complex", "Tahoma1");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();

        //        _writer.WriteStartElement("style:style");
        //        _writer.WriteAttributeString("style:name", "Graphics");
        //        _writer.WriteAttributeString("style:family", "graphic");
        //        _writer.WriteStartElement("style:graphic-properties");
        //        _writer.WriteAttributeString("text:anchor-type", "paragraph");
        //        _writer.WriteAttributeString("svg:x", "0in");
        //        _writer.WriteAttributeString("svg:y", "0in");
        //        if (!isMirrored)
        //        {
        //            _writer.WriteAttributeString("style:mirror", "none");
        //        }
        //        _writer.WriteAttributeString("fo:clip", "rect(0in 0in 0in 0in)");
        //        _writer.WriteAttributeString("draw:luminance", "0%");
        //        _writer.WriteAttributeString("draw:contrast", "0%");
        //        _writer.WriteAttributeString("draw:red", "0%");
        //        _writer.WriteAttributeString("draw:green", "0%");
        //        _writer.WriteAttributeString("draw:blue", "0%");
        //        _writer.WriteAttributeString("draw:gamma", "100%");
        //        _writer.WriteAttributeString("draw:color-inversion", "false");
        //        _writer.WriteAttributeString("draw:image-opacity", "100%");
        //        _writer.WriteAttributeString("draw:color-mode", "standard");
        //        _writer.WriteAttributeString("style:wrap", "none");
        //        _writer.WriteAttributeString("style:vertical-pos", "top");
        //        _writer.WriteAttributeString("style:vertical-rel", "paragraph");
        //        _writer.WriteAttributeString("style:horizontal-pos", "center");
        //        _writer.WriteAttributeString("style:horizontal-rel", "paragraph");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();

        //        //office:styles Attributes.
        //        //// fullString:outline-style
        //        _writer.WriteStartElement("text:outline-style");
        //        _writer.WriteStartElement("text:outline-level-style");
        //        _writer.WriteAttributeString("text:level", "1");
        //        _writer.WriteAttributeString("style:num-format", "");
        //        _writer.WriteStartElement("style:list-level-properties");
        //        _writer.WriteAttributeString("text:min-label-distance", "0.15in");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();
        //        _writer.WriteStartElement("text:outline-level-style");
        //        _writer.WriteAttributeString("text:level", "2");
        //        _writer.WriteAttributeString("style:num-format", "");
        //        _writer.WriteStartElement("style:list-level-properties");
        //        _writer.WriteAttributeString("text:min-label-distance", "0.15in");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();
        //        _writer.WriteStartElement("text:outline-level-style");
        //        _writer.WriteAttributeString("text:level", "3");
        //        _writer.WriteAttributeString("style:num-format", "");
        //        _writer.WriteStartElement("style:list-level-properties");
        //        _writer.WriteAttributeString("text:min-label-distance", "0.15in");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();
        //        _writer.WriteStartElement("text:outline-level-style");
        //        _writer.WriteAttributeString("text:level", "4");
        //        _writer.WriteAttributeString("style:num-format", "");
        //        _writer.WriteStartElement("style:list-level-properties");
        //        _writer.WriteAttributeString("text:min-label-distance", "0.15in");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();
        //        _writer.WriteStartElement("text:outline-level-style");
        //        _writer.WriteAttributeString("text:level", "5");
        //        _writer.WriteAttributeString("style:num-format", "");
        //        _writer.WriteStartElement("style:list-level-properties");
        //        _writer.WriteAttributeString("text:min-label-distance", "0.15in");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();
        //        _writer.WriteStartElement("text:outline-level-style");
        //        _writer.WriteAttributeString("text:level", "6");
        //        _writer.WriteAttributeString("style:num-format", "");
        //        _writer.WriteStartElement("style:list-level-properties");
        //        _writer.WriteAttributeString("text:min-label-distance", "0.15in");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();
        //        _writer.WriteStartElement("text:outline-level-style");
        //        _writer.WriteAttributeString("text:level", "7");
        //        _writer.WriteAttributeString("style:num-format", "");
        //        _writer.WriteStartElement("style:list-level-properties");
        //        _writer.WriteAttributeString("text:min-label-distance", "0.15in");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();
        //        _writer.WriteStartElement("text:outline-level-style");
        //        _writer.WriteAttributeString("text:level", "8");
        //        _writer.WriteAttributeString("style:num-format", "");
        //        _writer.WriteStartElement("style:list-level-properties");
        //        _writer.WriteAttributeString("text:min-label-distance", "0.15in");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();
        //        _writer.WriteStartElement("text:outline-level-style");
        //        _writer.WriteAttributeString("text:level", "9");
        //        _writer.WriteAttributeString("style:num-format", "");
        //        _writer.WriteStartElement("style:list-level-properties");
        //        _writer.WriteAttributeString("text:min-label-distance", "0.15in");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();
        //        _writer.WriteStartElement("text:outline-level-style");
        //        _writer.WriteAttributeString("text:level", "10");
        //        _writer.WriteAttributeString("style:num-format", "");
        //        _writer.WriteStartElement("style:list-level-properties");
        //        _writer.WriteAttributeString("text:min-label-distance", "0.15in");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();

        //        //office:styles Attributes.
        //        //// fullString:notes-configuration footnote
        //        _writer.WriteStartElement("text:notes-configuration");
        //        _writer.WriteAttributeString("text:note-class", "footnote");
        //        _writer.WriteAttributeString("text:citation-style-name", "Footnote Symbol");
        //        _writer.WriteAttributeString("text:citation-body-style-name", "Footnote anchor");
        //        _writer.WriteAttributeString("style:num-format", "1");
        //        _writer.WriteAttributeString("text:start-value", "0");
        //        _writer.WriteAttributeString("text:footnotes-position", "page");
        //        _writer.WriteAttributeString("text:start-numbering-at", "document");
        //        _writer.WriteEndElement();

        //        //office:styles Attributes.
        //        //// fullString:notes-configuration endnote
        //        _writer.WriteStartElement("text:notes-configuration");
        //        _writer.WriteAttributeString("text:note-class", "endnote");
        //        _writer.WriteAttributeString("style:num-format", "i");
        //        _writer.WriteAttributeString("text:start-value", "0");
        //        _writer.WriteEndElement();

        //        //office:styles Attributes.
        //        //// fullString:linenumbering-configuration
        //        _writer.WriteStartElement("text:linenumbering-configuration");
        //        _writer.WriteAttributeString("text:number-lines", "false");
        //        _writer.WriteAttributeString("text:offset", "0.1965in");
        //        _writer.WriteAttributeString("style:num-format", "1");
        //        _writer.WriteAttributeString("text:number-position", "left");
        //        _writer.WriteAttributeString("text:increment", "5");
        //        _writer.WriteEndElement();
        //        _writer.WriteEndElement();

        //        ODTPageFooter(); // Creating Footer Information for OpenOffice Document.

        //        _writer.WriteStartElement("office:master-styles"); // pm1

        //        ///STANDARD CODE PART
        //        _writer.WriteStartElement("style:master-page");
        //        _writer.WriteAttributeString("style:name", "Standard");
        //        _writer.WriteAttributeString("style:page-layout-name", "pm1");
        //        _writer.WriteStartElement("style:header");
        //        _writer.WriteStartElement("text:p");
        //        _writer.WriteAttributeString("text:style-name", "Header");
        //        //Right page Contents
        //        if (_pageHeaderFooter[18].Count > 0 || _pageHeaderFooter[19].Count > 0 || _pageHeaderFooter[20].Count > 0)
        //        {
        //            if (_pageHeaderFooter[18].ContainsKey("content"))
        //            {
        //                _writer.WriteStartElement("text:span");
        //                _writer.WriteAttributeString("text:style-name", "AllHeaderPageLeft");
        //                FillHeaderFooter(_pageHeaderFooter[18]["content"], 18);
        //                _writer.WriteEndElement();
        //            }
        //            _writer.WriteStartElement("text:tab");
        //            _writer.WriteEndElement();
        //            if (_pageHeaderFooter[19].ContainsKey("content"))
        //            {
        //                _writer.WriteStartElement("text:span");
        //                _writer.WriteAttributeString("text:style-name", "AllHeaderPageNumber");
        //                FillHeaderFooter(_pageHeaderFooter[19]["content"], 19);
        //                _writer.WriteEndElement();
        //            }
        //            _writer.WriteStartElement("text:tab");
        //            _writer.WriteEndElement();
        //            if (_pageHeaderFooter[20].ContainsKey("content"))
        //            {
        //                _writer.WriteStartElement("text:span");
        //                _writer.WriteAttributeString("text:style-name", "AllHeaderPageRight");
        //                FillHeaderFooter(_pageHeaderFooter[20]["content"], 20);
        //                _writer.WriteEndElement();
        //            }
        //        }
        //        else
        //        {
        //            if (isMirrored)
        //            {
        //                //If no content in right, loads from allpage contents
        //                SetAllPageHeader();
        //            }
        //        }
        //        _writer.WriteEndElement(); // Close if p
        //        _writer.WriteEndElement(); // Close of header

        //        if (isMirrored)
        //        {
        //            //header-left only created when page is set to mirrored
        //            _writer.WriteStartElement("style:header-left");
        //            _writer.WriteStartElement("text:p");
        //            _writer.WriteAttributeString("text:style-name", "Header");
        //            if (_pageHeaderFooter[12].Count > 0 || _pageHeaderFooter[13].Count > 0 || _pageHeaderFooter[14].Count > 0)
        //            {
        //                if (_pageHeaderFooter[12].ContainsKey("content"))
        //                {
        //                    _writer.WriteStartElement("text:span");
        //                    _writer.WriteAttributeString("text:style-name", "AllHeaderPageLeft");
        //                    FillHeaderFooter(_pageHeaderFooter[12]["content"], 12);
        //                    _writer.WriteEndElement();
        //                }
        //                _writer.WriteStartElement("text:tab");
        //                _writer.WriteEndElement();
        //                if (_pageHeaderFooter[13].ContainsKey("content"))
        //                {
        //                    _writer.WriteStartElement("text:span");
        //                    _writer.WriteAttributeString("text:style-name", "AllHeaderPageNumber");
        //                    FillHeaderFooter(_pageHeaderFooter[13]["content"], 13);
        //                    _writer.WriteEndElement();
        //                }
        //                _writer.WriteStartElement("text:tab");
        //                _writer.WriteEndElement();
        //                if (_pageHeaderFooter[14].ContainsKey("content"))
        //                {
        //                    _writer.WriteStartElement("text:span");
        //                    _writer.WriteAttributeString("text:style-name", "AllHeaderPageRight");
        //                    FillHeaderFooter(_pageHeaderFooter[14]["content"], 14);
        //                    _writer.WriteEndElement();
        //                }
        //            }
        //            else
        //            {
        //                SetAllPageHeader();
        //            }
        //            _writer.WriteEndElement(); // Close of p
        //            _writer.WriteEndElement(); // Close of Header-left
        //        }

        //        _writer.WriteStartElement("style:footer");
        //        _writer.WriteStartElement("text:p");
        //        _writer.WriteAttributeString("text:style-name", "Footer");
        //        if (_pageHeaderFooter[21].Count > 0 || _pageHeaderFooter[22].Count > 0 || _pageHeaderFooter[23].Count > 0)
        //        {
        //            if (_pageHeaderFooter[21].ContainsKey("content"))
        //            {
        //                _writer.WriteStartElement("text:span");
        //                _writer.WriteAttributeString("text:style-name", "AllFooterPageLeft");
        //                FillHeaderFooter(_pageHeaderFooter[21]["content"], 21);
        //                _writer.WriteEndElement();
        //            }
        //            _writer.WriteStartElement("text:tab");
        //            _writer.WriteEndElement();
        //            if (_pageHeaderFooter[22].ContainsKey("content"))
        //            {
        //                _writer.WriteStartElement("text:span");
        //                _writer.WriteAttributeString("text:style-name", "AllFooterPageNumber");
        //                FillHeaderFooter(_pageHeaderFooter[22]["content"], 22);
        //                _writer.WriteEndElement();
        //            }
        //            _writer.WriteStartElement("text:tab");
        //            _writer.WriteEndElement();
        //            if (_pageHeaderFooter[23].ContainsKey("content"))
        //            {
        //                _writer.WriteStartElement("text:span");
        //                _writer.WriteAttributeString("text:style-name", "AllFooterPageRight");
        //                FillHeaderFooter(_pageHeaderFooter[23]["content"], 23);
        //                _writer.WriteEndElement();
        //            }
        //        }
        //        else
        //        {
        //            if (isMirrored)
        //            {
        //                //If no content in right, loads from allpage contents
        //                SetAllPageFooter();
        //            }
        //        }
        //        _writer.WriteEndElement(); // Close of p
        //        _writer.WriteEndElement(); // Close if footer

        //        _writer.WriteStartElement("style:footer-left");
        //        _writer.WriteStartElement("text:p");
        //        _writer.WriteAttributeString("text:style-name", "Footer");
        //        if (_pageHeaderFooter[15].Count > 0 || _pageHeaderFooter[15].Count > 0 || _pageHeaderFooter[17].Count > 0)
        //        {
        //            if (_pageHeaderFooter[15].ContainsKey("content"))
        //            {
        //                _writer.WriteStartElement("text:span");
        //                _writer.WriteAttributeString("text:style-name", "AllFooterPageLeft");
        //                FillHeaderFooter(_pageHeaderFooter[15]["content"], 15);
        //                _writer.WriteEndElement();
        //            }
        //            _writer.WriteStartElement("text:tab");
        //            _writer.WriteEndElement();
        //            if (_pageHeaderFooter[16].ContainsKey("content"))
        //            {
        //                _writer.WriteStartElement("text:span");
        //                _writer.WriteAttributeString("text:style-name", "AllFooterPageNumber");
        //                FillHeaderFooter(_pageHeaderFooter[16]["content"], 16);
        //                _writer.WriteEndElement();
        //            }
        //            _writer.WriteStartElement("text:tab");
        //            _writer.WriteEndElement();
        //            if (_pageHeaderFooter[17].ContainsKey("content"))
        //            {
        //                _writer.WriteStartElement("text:span");
        //                _writer.WriteAttributeString("text:style-name", "AllFooterPageRight");
        //                FillHeaderFooter(_pageHeaderFooter[17]["content"], 17);
        //                _writer.WriteEndElement();
        //            }
        //        }
        //        else
        //        {
        //            if (isMirrored)
        //            {
        //                SetAllPageFooter();
        //            }
        //        }
        //        _writer.WriteEndElement(); // Close of p
        //        _writer.WriteEndElement(); // Close of Footer-Left

        //        _writer.WriteEndElement(); // Close of Master page Standard

        //        ///STANDARD CODE PART ENDS

        //        ////XHTML CODE PART START

        //        _writer.WriteStartElement("style:master-page");
        //        _writer.WriteAttributeString("style:name", "XHTML"); // All PageProperty
        //        _writer.WriteAttributeString("style:page-layout-name", "pm2");
        //        if (!isMirrored)
        //        {
        //            /* Begin AllPage Header */
        //            if (_pageHeaderFooter[6].Count > 0 || _pageHeaderFooter[7].Count > 0 || _pageHeaderFooter[8].Count > 0)
        //            {
        //                _writer.WriteStartElement("style:header");
        //                _writer.WriteStartElement("text:p");
        //                _writer.WriteAttributeString("text:style-name", "Header");
        //                SetAllPageHeader();
        //                _writer.WriteEndElement();
        //                _writer.WriteEndElement();
        //            }
        //            /* Begin AllPage Footer */
        //            if (_pageHeaderFooter[9].Count > 0 || _pageHeaderFooter[10].Count > 0 || _pageHeaderFooter[11].Count > 0)
        //            {
        //                _writer.WriteStartElement("style:footer");
        //                _writer.WriteStartElement("text:p");
        //                _writer.WriteAttributeString("text:style-name", "Footer");
        //                SetAllPageFooter();
        //                _writer.WriteEndElement(); // close of p
        //                _writer.WriteEndElement(); // Close of Footer
        //            }
        //        }
        //        _writer.WriteEndElement(); // Close of Master Page

        //        ////XHTML CODE PART ENDS

        //        //// First CODE PART START

        //        _writer.WriteStartElement("style:master-page");
        //        _writer.WriteAttributeString("style:name", "First_20_Page");
        //        _writer.WriteAttributeString("style:display-name", "First Page"); // First PageProperty
        //        if (isMirrored)
        //        {
        //            _writer.WriteAttributeString("style:page-layout-name", "pm2");
        //            _writer.WriteAttributeString("style:next-style-name", "Standard");
        //        }
        //        else
        //        {
        //            _writer.WriteAttributeString("style:page-layout-name", "pm3");
        //            _writer.WriteAttributeString("style:next-style-name", "XHTML");
        //        }
        //        /*Begin Firstpage Header */
        //        if (_pageHeaderFooter[0].Count > 0 || _pageHeaderFooter[1].Count > 0 || _pageHeaderFooter[2].Count > 0)
        //        {
        //            if (IsContentAvailable(0) || IsCropMarkChecked)
        //            {
        //                _writer.WriteStartElement("style:header");
        //                _writer.WriteStartElement("text:p");
        //                _writer.WriteAttributeString("text:style-name", "Header");
        //                if (IsCropMarkChecked)
        //                {
        //                    foreach (KeyValuePair<string, string> para in _firstPageLayoutProperty)
        //                    {
        //                        _pageLayoutProperty[para.Key] = para.Value;
        //                    }

        //                    foreach (KeyValuePair<string, string> para in _pageLayoutProperty)
        //                    {
        //                        _writer.WriteAttributeString(para.Key, para.Value);
        //                    }
        //                    AddHeaderCropMarks(_pageLayoutProperty);
        //                }
        //                if (_pageHeaderFooter[0].ContainsKey("content"))
        //                {
        //                    _writer.WriteStartElement("text:span");
        //                    _writer.WriteAttributeString("text:style-name", "HeaderPageLeft");
        //                    FillHeaderFooter(_pageHeaderFooter[0]["content"], 0);
        //                    _writer.WriteEndElement();
        //                }
        //                _writer.WriteStartElement("text:tab");
        //                _writer.WriteEndElement();
        //                if (_pageHeaderFooter[1].ContainsKey("content"))
        //                {
        //                    _writer.WriteStartElement("text:span");
        //                    _writer.WriteAttributeString("text:style-name", "HeaderPageNumber");
        //                    FillHeaderFooter(_pageHeaderFooter[1]["content"], 1);
        //                    _writer.WriteEndElement();
        //                }
        //                _writer.WriteStartElement("text:tab");
        //                _writer.WriteEndElement();
        //                if (_pageHeaderFooter[2].ContainsKey("content"))
        //                {
        //                    _writer.WriteStartElement("text:span");
        //                    _writer.WriteAttributeString("text:style-name", "HeaderPageRight");
        //                    FillHeaderFooter(_pageHeaderFooter[2]["content"], 2);
        //                    _writer.WriteEndElement();
        //                }
        //                _writer.WriteEndElement(); // Close of p
        //                _writer.WriteEndElement(); // Close of Header
        //            }
        //        }

        //        /*Begin Firstpage Footer */
        //        if (_pageHeaderFooter[3].Count > 0 || _pageHeaderFooter[4].Count > 0 || _pageHeaderFooter[5].Count > 0)
        //        {
        //            if (IsContentAvailable(3))
        //            {
        //                _writer.WriteStartElement("style:footer");
        //                _writer.WriteStartElement("text:p");
        //                _writer.WriteAttributeString("text:style-name", "Footer");

        //                if (_pageHeaderFooter[3].ContainsKey("content"))
        //                {
        //                    _writer.WriteStartElement("text:span");
        //                    _writer.WriteAttributeString("text:style-name", "FooterPageLeft");
        //                    FillHeaderFooter(_pageHeaderFooter[3]["content"], 3);
        //                    _writer.WriteEndElement();
        //                }
        //                _writer.WriteStartElement("text:tab");
        //                _writer.WriteEndElement();

        //                if (_pageHeaderFooter[4].ContainsKey("content"))
        //                {
        //                    _writer.WriteStartElement("text:span");
        //                    _writer.WriteAttributeString("text:style-name", "FooterPageNumber");
        //                    FillHeaderFooter(_pageHeaderFooter[4]["content"], 4);
        //                    _writer.WriteEndElement();
        //                }

        //                _writer.WriteStartElement("text:tab");
        //                _writer.WriteEndElement();

        //                if (_pageHeaderFooter[5].ContainsKey("content"))
        //                {
        //                    _writer.WriteStartElement("text:span");
        //                    _writer.WriteAttributeString("text:style-name", "FooterPageRight");
        //                    FillHeaderFooter(_pageHeaderFooter[5]["content"], 5);
        //                    _writer.WriteEndElement();
        //                }

        //                _writer.WriteEndElement(); // Close of p
        //                _writer.WriteEndElement(); // Close of Footer
        //            }
        //            /*End Firstpage Footer */
        //        }
        //        _writer.WriteEndElement(); // Close of Master Page
        //        //// First CODE PART ENDS
        //        _writer.WriteEndDocument();
        //        _writer.Flush();
        //        _writer.Close();

        //        try
        //        {
        //            if (_verboseWriter.ShowError && _verboseWriter.ErrorWritten)  // error file closing
        //            {
        //                _verboseWriter.WriteError("</table>");
        //                _verboseWriter.WriteError("</body>");
        //                _verboseWriter.WriteError("</html>");
        //                _verboseWriter.Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.Write(ex.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //        _writer.Flush();
        //        _writer.Close();
        //    }
        //}
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate last block of Styles.xml
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        private void CloseODTStyles()
        {
            try
            {
                // "graphic"
                _writer.WriteStartElement("style:default-style");
                _writer.WriteAttributeString("style:family", "graphic");
                _writer.WriteStartElement("style:graphic-properties");
                _writer.WriteAttributeString("draw:shadow-offset-x", "0.1181in");
                _writer.WriteAttributeString("draw:shadow-offset-y", "0.1181in");
                _writer.WriteAttributeString("draw:start-line-spacing-horizontal", "0.1114in");
                _writer.WriteAttributeString("draw:start-line-spacing-vertical", "0.1114in");
                _writer.WriteAttributeString("draw:end-line-spacing-horizontal", "0.1114in");
                _writer.WriteAttributeString("draw:end-line-spacing-vertical", "0.1114in");
                //_writer.WriteAttributeString("style:flow-with-text", "false");
                _writer.WriteAttributeString("style:flow-with-text", "true");
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:paragraph-properties");
                _writer.WriteAttributeString("style:text-autospace", "ideograph-alpha");
                _writer.WriteAttributeString("style:line-break", "strict");
                _writer.WriteAttributeString("style:writing-mode", "lr-tb");
                _writer.WriteAttributeString("style:font-independent-line-spacing", "false");

                _writer.WriteStartElement("style:tab-stops");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:text-properties");
                _writer.WriteAttributeString("style:use-window-font-color", "true");
                _writer.WriteAttributeString("fo:font-size", "12pt");
                //_writer.WriteAttributeString("fo:language", "en");
                //_writer.WriteAttributeString("fo:country", "US");
                _writer.WriteAttributeString("fo:language", "none");
                _writer.WriteAttributeString("fo:country", "none");
                _writer.WriteAttributeString("style:letter-kerning", "true");
                _writer.WriteAttributeString("style:font-size-asian", "12pt");
                _writer.WriteAttributeString("style:language-asian", "zxx");
                _writer.WriteAttributeString("style:country-asian", "none");
                _writer.WriteAttributeString("style:font-size-complex", "12pt");
                _writer.WriteAttributeString("style:language-complex", "zxx");
                _writer.WriteAttributeString("style:country-complex", "none");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                //office:styles Attributes.
                //// "paragraph"
                _writer.WriteStartElement("style:default-style");
                _writer.WriteAttributeString("style:family", "paragraph");
                _writer.WriteStartElement("style:paragraph-properties");
                _writer.WriteAttributeString("fo:hyphenation-ladder-count", "no-limit");
                _writer.WriteAttributeString("style:text-autospace", "ideograph-alpha");
                _writer.WriteAttributeString("style:punctuation-wrap", "hanging");
                _writer.WriteAttributeString("style:line-break", "strict");
                _writer.WriteAttributeString("style:tab-stop-distance", "0.4925in");
                _writer.WriteAttributeString("style:writing-mode", "page");
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:text-properties");
                _writer.WriteAttributeString("style:use-window-font-color", "true");
                _writer.WriteAttributeString("style:font-name", "Times New Roman");
                _writer.WriteAttributeString("fo:font-size", "12pt");
                //_writer.WriteAttributeString("fo:language", "en");
                //_writer.WriteAttributeString("fo:country", "US");
                _writer.WriteAttributeString("fo:language", "none");
                _writer.WriteAttributeString("fo:country", "none");
                _writer.WriteAttributeString("style:letter-kerning", "true");
                _writer.WriteAttributeString("style:font-name-asian", "Yi plus Phonetics");
                _writer.WriteAttributeString("style:font-size-asian", "12pt");
                _writer.WriteAttributeString("style:language-asian", "none");
                _writer.WriteAttributeString("style:country-asian", "none");
                _writer.WriteAttributeString("style:font-name-complex", "Tahoma");
                _writer.WriteAttributeString("style:font-size-complex", "12pt");
                _writer.WriteAttributeString("style:language-complex", "zxx");
                _writer.WriteAttributeString("style:country-complex", "none");
                _writer.WriteAttributeString("fo:hyphenate", "false");
                _writer.WriteAttributeString("fo:hyphenation-remain-char-count", "2");
                _writer.WriteAttributeString("fo:hyphenation-push-char-count", "2");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                //office:styles Attributes.
                //// "table"
                _writer.WriteStartElement("style:default-style");
                _writer.WriteAttributeString("style:family", "table");
                _writer.WriteStartElement("style:table-properties");
                _writer.WriteAttributeString("table:border-model", "collapsing");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                //office:styles Attributes.
                //// "table-row"
                _writer.WriteStartElement("style:default-style");
                _writer.WriteAttributeString("style:family", "table-row");
                _writer.WriteStartElement("style:table-row-properties");
                _writer.WriteAttributeString("fo:keep-together", "auto");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                //office:styles Attributes.
                //// "Standard"
                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "Standard");
                _writer.WriteAttributeString("style:family", "paragraph");
                _writer.WriteAttributeString("style:class", "text");
                _writer.WriteEndElement();

                //office:styles Attributes.
                //// "Header"
                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "Header");
                _writer.WriteAttributeString("style:family", "paragraph");
                _writer.WriteAttributeString("style:parent-style-name", "Standard");
                _writer.WriteAttributeString("style:next-style-name", "Text_20_body");
                _writer.WriteAttributeString("style:class", "text");

                _writer.WriteStartElement("style:paragraph-properties");//style:paragraph-properties
                _writer.WriteAttributeString("fo:margin-top", "0.0005in");
                _writer.WriteAttributeString("fo:margin-bottom", "0.0835in");
                _writer.WriteAttributeString("fo:keep-with-next", "always");

                

                _writer.WriteAttributeString("text:number-lines", "false");
                _writer.WriteAttributeString("text:line-number", "0");

                _writer.WriteStartElement("style:tab-stops");//style:tab-stops
                _writer.WriteStartElement("style:tab-stop");//style:tab-stop

                // Finds the PageProperty Header's center.(GuideWord or PageNo)
                float borderLeft;
                if (_pageLayoutProperty.ContainsKey("fo:border-left"))
                {
                    string[] parameters = _pageLayoutProperty["fo:border-left"].Split(' ');
                    string pageBorderLeft = "0pt";
                    foreach (string param in parameters)
                    {
                        if (Common.ValidateNumber(param[0].ToString() + "1"))
                        {
                            pageBorderLeft = param;
                            break;
                        }
                    }
                    borderLeft = Common.ConvertToInch(pageBorderLeft);
                }
                else
                {
                    borderLeft = 0F;
                }

                float width = Common.ConvertToInch(_pageLayoutProperty["fo:page-width"]);
                float left = Common.ConvertToInch(_pageLayoutProperty["fo:margin-left"]);
                float right = Common.ConvertToInch(_pageLayoutProperty["fo:margin-left"]);
                float mid = width / 2F - left - borderLeft;
                float rightGuide = (width - left - right);

                _writer.WriteAttributeString("style:position", mid.ToString() + "in");

                _writer.WriteAttributeString("style:type", "center");
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:tab-stop");//style:tab-stop
                _writer.WriteAttributeString("style:position", rightGuide.ToString() + "in");
                _writer.WriteAttributeString("style:type", "right");
                _writer.WriteEndElement();

                _writer.WriteEndElement();//style:tab-stops
                _writer.WriteEndElement();//style:paragraph-properties
                _writer.WriteEndElement();


                //// "Footer"
                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "Footer");
                _writer.WriteAttributeString("style:family", "paragraph");
                _writer.WriteAttributeString("style:parent-style-name", "Standard");
                _writer.WriteAttributeString("style:next-style-name", "Text_20_body");
                _writer.WriteAttributeString("style:class", "text");

                _writer.WriteStartElement("style:paragraph-properties");//style:paragraph-properties
                _writer.WriteAttributeString("fo:margin-top", "0.0005in");
                _writer.WriteAttributeString("fo:margin-bottom", "0.0835in");
                _writer.WriteAttributeString("fo:keep-with-next", "always");

                _writer.WriteAttributeString("text:number-lines", "false");
                _writer.WriteAttributeString("text:line-number", "0");

                _writer.WriteStartElement("style:tab-stops");//style:tab-stops

                _writer.WriteStartElement("style:tab-stop");//style:tab-stop
                _writer.WriteAttributeString("style:position", mid.ToString() + "in");
                _writer.WriteAttributeString("style:type", "center");
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:tab-stop");//style:tab-stop
                _writer.WriteAttributeString("style:position", rightGuide.ToString() + "in");
                _writer.WriteAttributeString("style:type", "right");
                _writer.WriteEndElement();

                _writer.WriteEndElement();//style:tab-stops
                _writer.WriteEndElement();//style:paragraph-properties
                _writer.WriteEndElement();//Footer style

                //office:styles Attributes.
                //// "Text_20_body"
                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "Text_20_body");
                _writer.WriteAttributeString("style:display-name", "Text body");
                _writer.WriteAttributeString("style:family", "paragraph");
                _writer.WriteAttributeString("style:parent-style-name", "Standard");
                _writer.WriteAttributeString("style:class", "text");
                _writer.WriteStartElement("style:paragraph-properties");
                _writer.WriteAttributeString("fo:margin-top", "0in");
                _writer.WriteAttributeString("fo:margin-bottom", "0.0835in");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                //office:styles Attributes.
                //// "List"
                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "List");
                _writer.WriteAttributeString("style:family", "paragraph");
                _writer.WriteAttributeString("style:parent-style-name", "Text_20_body");
                _writer.WriteAttributeString("style:class", "list");
                _writer.WriteStartElement("style:text-properties");
                _writer.WriteAttributeString("style:font-name-asian", "Yi plus Phonetics");
                _writer.WriteAttributeString("style:font-name-complex", "Tahoma1");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                //office:styles Attributes.
                //// "Caption"
                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "Caption");
                _writer.WriteAttributeString("style:family", "paragraph");
                _writer.WriteAttributeString("style:parent-style-name", "Standard");
                _writer.WriteAttributeString("style:class", "extra");
                _writer.WriteStartElement("style:paragraph-properties");
                _writer.WriteAttributeString("fo:margin-top", "0.0835in");
                _writer.WriteAttributeString("fo:margin-bottom", "0.0835in");
                _writer.WriteAttributeString("text:number-lines", "false");
                _writer.WriteAttributeString("text:line-number", "0");
                _writer.WriteEndElement();
                _writer.WriteStartElement("style:text-properties");
                _writer.WriteAttributeString("fo:font-size", "12pt");
                _writer.WriteAttributeString("fo:font-style", "italic");
                _writer.WriteAttributeString("style:font-name-asian", "Yi plus Phonetics");
                _writer.WriteAttributeString("style:font-size-asian", "12pt");
                _writer.WriteAttributeString("style:font-style-asian", "italic");
                _writer.WriteAttributeString("style:font-name-complex", "Tahoma1");
                _writer.WriteAttributeString("style:font-size-complex", "12pt");
                _writer.WriteAttributeString("style:font-style-complex", "italic");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                //office:styles Attributes.
                //// "index"
                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "Index");
                _writer.WriteAttributeString("style:family", "paragraph");
                _writer.WriteAttributeString("style:parent-style-name", "Standard");
                _writer.WriteAttributeString("style:class", "index");
                _writer.WriteStartElement("style:paragraph-properties");
                _writer.WriteAttributeString("text:number-lines", "false");
                _writer.WriteAttributeString("text:line-number", "0");
                _writer.WriteEndElement();
                _writer.WriteStartElement("style:text-properties");
                _writer.WriteAttributeString("style:font-name-asian", "Yi plus Phonetics");
                _writer.WriteAttributeString("style:font-name-complex", "Tahoma1");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                _writer.WriteStartElement("style:style");
                _writer.WriteAttributeString("style:name", "Graphics");
                _writer.WriteAttributeString("style:family", "graphic");
                _writer.WriteStartElement("style:graphic-properties");
                _writer.WriteAttributeString("text:anchor-type", "paragraph");
                _writer.WriteAttributeString("svg:x", "0in");
                _writer.WriteAttributeString("svg:y", "0in");
                if (!isMirrored)
                {
                    _writer.WriteAttributeString("style:mirror", "none");
                }
                _writer.WriteAttributeString("fo:clip", "rect(0in 0in 0in 0in)");
                _writer.WriteAttributeString("draw:luminance", "0%");
                _writer.WriteAttributeString("draw:contrast", "0%");
                _writer.WriteAttributeString("draw:red", "0%");
                _writer.WriteAttributeString("draw:green", "0%");
                _writer.WriteAttributeString("draw:blue", "0%");
                _writer.WriteAttributeString("draw:gamma", "100%");
                _writer.WriteAttributeString("draw:color-inversion", "false");
                _writer.WriteAttributeString("draw:image-opacity", "100%");
                _writer.WriteAttributeString("draw:color-mode", "standard");
                _writer.WriteAttributeString("style:wrap", "none");
                _writer.WriteAttributeString("style:vertical-pos", "top");
                _writer.WriteAttributeString("style:vertical-rel", "paragraph");
                _writer.WriteAttributeString("style:horizontal-pos", "center");
                _writer.WriteAttributeString("style:horizontal-rel", "paragraph");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                //office:styles Attributes.
                //// fullString:outline-style
                _writer.WriteStartElement("text:outline-style");
                _writer.WriteStartElement("text:outline-level-style");
                _writer.WriteAttributeString("text:level", "1");
                _writer.WriteAttributeString("style:num-format", "");
                _writer.WriteStartElement("style:list-level-properties");
                _writer.WriteAttributeString("text:min-label-distance", "0.15in");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteStartElement("text:outline-level-style");
                _writer.WriteAttributeString("text:level", "2");
                _writer.WriteAttributeString("style:num-format", "");
                _writer.WriteStartElement("style:list-level-properties");
                _writer.WriteAttributeString("text:min-label-distance", "0.15in");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteStartElement("text:outline-level-style");
                _writer.WriteAttributeString("text:level", "3");
                _writer.WriteAttributeString("style:num-format", "");
                _writer.WriteStartElement("style:list-level-properties");
                _writer.WriteAttributeString("text:min-label-distance", "0.15in");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteStartElement("text:outline-level-style");
                _writer.WriteAttributeString("text:level", "4");
                _writer.WriteAttributeString("style:num-format", "");
                _writer.WriteStartElement("style:list-level-properties");
                _writer.WriteAttributeString("text:min-label-distance", "0.15in");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteStartElement("text:outline-level-style");
                _writer.WriteAttributeString("text:level", "5");
                _writer.WriteAttributeString("style:num-format", "");
                _writer.WriteStartElement("style:list-level-properties");
                _writer.WriteAttributeString("text:min-label-distance", "0.15in");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteStartElement("text:outline-level-style");
                _writer.WriteAttributeString("text:level", "6");
                _writer.WriteAttributeString("style:num-format", "");
                _writer.WriteStartElement("style:list-level-properties");
                _writer.WriteAttributeString("text:min-label-distance", "0.15in");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteStartElement("text:outline-level-style");
                _writer.WriteAttributeString("text:level", "7");
                _writer.WriteAttributeString("style:num-format", "");
                _writer.WriteStartElement("style:list-level-properties");
                _writer.WriteAttributeString("text:min-label-distance", "0.15in");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteStartElement("text:outline-level-style");
                _writer.WriteAttributeString("text:level", "8");
                _writer.WriteAttributeString("style:num-format", "");
                _writer.WriteStartElement("style:list-level-properties");
                _writer.WriteAttributeString("text:min-label-distance", "0.15in");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteStartElement("text:outline-level-style");
                _writer.WriteAttributeString("text:level", "9");
                _writer.WriteAttributeString("style:num-format", "");
                _writer.WriteStartElement("style:list-level-properties");
                _writer.WriteAttributeString("text:min-label-distance", "0.15in");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteStartElement("text:outline-level-style");
                _writer.WriteAttributeString("text:level", "10");
                _writer.WriteAttributeString("style:num-format", "");
                _writer.WriteStartElement("style:list-level-properties");
                _writer.WriteAttributeString("text:min-label-distance", "0.15in");
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                //office:styles Attributes.
                //// fullString:notes-configuration footnote
                _writer.WriteStartElement("text:notes-configuration");
                _writer.WriteAttributeString("text:note-class", "footnote");
                _writer.WriteAttributeString("text:citation-style-name", "Footnote Symbol");
                _writer.WriteAttributeString("text:citation-body-style-name", "Footnote anchor");
                _writer.WriteAttributeString("style:num-format", "1");
                _writer.WriteAttributeString("text:start-value", "0");
                _writer.WriteAttributeString("text:footnotes-position", "page");
                _writer.WriteAttributeString("text:start-numbering-at", "document");
                _writer.WriteEndElement();

                //office:styles Attributes.
                //// fullString:notes-configuration endnote
                _writer.WriteStartElement("text:notes-configuration");
                _writer.WriteAttributeString("text:note-class", "endnote");
                _writer.WriteAttributeString("style:num-format", "i");
                _writer.WriteAttributeString("text:start-value", "0");
                _writer.WriteEndElement();

                //office:styles Attributes.
                //// fullString:linenumbering-configuration
                _writer.WriteStartElement("text:linenumbering-configuration");
                _writer.WriteAttributeString("text:number-lines", "false");
                _writer.WriteAttributeString("text:offset", "0.1965in");
                _writer.WriteAttributeString("style:num-format", "1");
                _writer.WriteAttributeString("text:number-position", "left");
                _writer.WriteAttributeString("text:increment", "5");
                _writer.WriteEndElement();
                _writer.WriteEndElement();

                

                _writer.WriteStartElement("office:master-styles"); // pm1

                ///STANDARD CODE PART
                _writer.WriteStartElement("style:master-page");
                _writer.WriteAttributeString("style:name", "Standard");
                _writer.WriteAttributeString("style:page-layout-name", "pm1");
                _writer.WriteStartElement("style:header");
                _writer.WriteStartElement("text:p");
                _writer.WriteAttributeString("text:style-name", "Header");
                //Right page Contents
                if (_pageHeaderFooter[18].Count > 0 || _pageHeaderFooter[19].Count > 0 || _pageHeaderFooter[20].Count > 0)
                {
                    if (_pageHeaderFooter[18].ContainsKey("content"))
                    {
                        _writer.WriteStartElement("text:span");
                        _writer.WriteAttributeString("text:style-name", "AllHeaderPageLeft");
                        ////FillHeaderFooter(_pageHeaderFooter[18]["content"], 18);
                        _writer.WriteEndElement();
                    }
                    _writer.WriteStartElement("text:tab");
                    _writer.WriteEndElement();
                    if (_pageHeaderFooter[19].ContainsKey("content"))
                    {
                        _writer.WriteStartElement("text:span");
                        _writer.WriteAttributeString("text:style-name", "AllHeaderPageNumber");
                        ////FillHeaderFooter(_pageHeaderFooter[19]["content"], 19);
                        _writer.WriteEndElement();
                    }
                    _writer.WriteStartElement("text:tab");
                    _writer.WriteEndElement();
                    if (_pageHeaderFooter[20].ContainsKey("content"))
                    {
                        _writer.WriteStartElement("text:span");
                        _writer.WriteAttributeString("text:style-name", "AllHeaderPageRight");
                        //FillHeaderFooter(_pageHeaderFooter[20]["content"], 20);
                        _writer.WriteEndElement();
                    }
                }
                else
                {
                    if (isMirrored)
                    {
                        //If no content in right, loads from allpage contents
                        //SetAllPageHeader();
                    }
                }
                _writer.WriteEndElement(); // Close if p
                _writer.WriteEndElement(); // Close of header

                if (isMirrored)
                {
                    //header-left only created when page is set to mirrored
                    _writer.WriteStartElement("style:header-left");
                    _writer.WriteStartElement("text:p");
                    _writer.WriteAttributeString("text:style-name", "Header");
                    if (_pageHeaderFooter[12].Count > 0 || _pageHeaderFooter[13].Count > 0 || _pageHeaderFooter[14].Count > 0)
                    {
                        if (_pageHeaderFooter[12].ContainsKey("content"))
                        {
                            _writer.WriteStartElement("text:span");
                            _writer.WriteAttributeString("text:style-name", "AllHeaderPageLeft");
                            //FillHeaderFooter(_pageHeaderFooter[12]["content"], 12);
                            _writer.WriteEndElement();
                        }
                        _writer.WriteStartElement("text:tab");
                        _writer.WriteEndElement();
                        if (_pageHeaderFooter[13].ContainsKey("content"))
                        {
                            _writer.WriteStartElement("text:span");
                            _writer.WriteAttributeString("text:style-name", "AllHeaderPageNumber");
                            ////FillHeaderFooter(_pageHeaderFooter[13]["content"], 13);
                            _writer.WriteEndElement();
                        }
                        _writer.WriteStartElement("text:tab");
                        _writer.WriteEndElement();
                        if (_pageHeaderFooter[14].ContainsKey("content"))
                        {
                            _writer.WriteStartElement("text:span");
                            _writer.WriteAttributeString("text:style-name", "AllHeaderPageRight");
                            ////FillHeaderFooter(_pageHeaderFooter[14]["content"], 14);
                            _writer.WriteEndElement();
                        }
                    }
                    else
                    {
                        ////SetAllPageHeader();
                    }
                    _writer.WriteEndElement(); // Close of p
                    _writer.WriteEndElement(); // Close of Header-left
                }

                _writer.WriteStartElement("style:footer");
                _writer.WriteStartElement("text:p");
                _writer.WriteAttributeString("text:style-name", "Footer");
                if (_pageHeaderFooter[21].Count > 0 || _pageHeaderFooter[22].Count > 0 || _pageHeaderFooter[23].Count > 0)
                {
                    if (_pageHeaderFooter[21].ContainsKey("content"))
                    {
                        _writer.WriteStartElement("text:span");
                        _writer.WriteAttributeString("text:style-name", "AllFooterPageLeft");
                        //FillHeaderFooter(_pageHeaderFooter[21]["content"], 21);
                        _writer.WriteEndElement();
                    }
                    _writer.WriteStartElement("text:tab");
                    _writer.WriteEndElement();
                    if (_pageHeaderFooter[22].ContainsKey("content"))
                    {
                        _writer.WriteStartElement("text:span");
                        _writer.WriteAttributeString("text:style-name", "AllFooterPageNumber");
                        //FillHeaderFooter(_pageHeaderFooter[22]["content"], 22);
                        _writer.WriteEndElement();
                    }
                    _writer.WriteStartElement("text:tab");
                    _writer.WriteEndElement();
                    if (_pageHeaderFooter[23].ContainsKey("content"))
                    {
                        _writer.WriteStartElement("text:span");
                        _writer.WriteAttributeString("text:style-name", "AllFooterPageRight");
                        //FillHeaderFooter(_pageHeaderFooter[23]["content"], 23);
                        _writer.WriteEndElement();
                    }
                }
                else
                {
                    if (isMirrored)
                    {
                        //If no content in right, loads from allpage contents
                        //SetAllPageFooter();
                    }
                }
                _writer.WriteEndElement(); // Close of p
                _writer.WriteEndElement(); // Close if footer

                _writer.WriteStartElement("style:footer-left");
                _writer.WriteStartElement("text:p");
                _writer.WriteAttributeString("text:style-name", "Footer");
                if (_pageHeaderFooter[15].Count > 0 || _pageHeaderFooter[15].Count > 0 || _pageHeaderFooter[17].Count > 0)
                {
                    if (_pageHeaderFooter[15].ContainsKey("content"))
                    {
                        _writer.WriteStartElement("text:span");
                        _writer.WriteAttributeString("text:style-name", "AllFooterPageLeft");
                        //FillHeaderFooter(_pageHeaderFooter[15]["content"], 15);
                        _writer.WriteEndElement();
                    }
                    _writer.WriteStartElement("text:tab");
                    _writer.WriteEndElement();
                    if (_pageHeaderFooter[16].ContainsKey("content"))
                    {
                        _writer.WriteStartElement("text:span");
                        _writer.WriteAttributeString("text:style-name", "AllFooterPageNumber");
                        //FillHeaderFooter(_pageHeaderFooter[16]["content"], 16);
                        _writer.WriteEndElement();
                    }
                    _writer.WriteStartElement("text:tab");
                    _writer.WriteEndElement();
                    if (_pageHeaderFooter[17].ContainsKey("content"))
                    {
                        _writer.WriteStartElement("text:span");
                        _writer.WriteAttributeString("text:style-name", "AllFooterPageRight");
                        //FillHeaderFooter(_pageHeaderFooter[17]["content"], 17);
                        _writer.WriteEndElement();
                    }
                }
                else
                {
                    if (isMirrored)
                    {
                        //SetAllPageFooter();
                    }
                }
                _writer.WriteEndElement(); // Close of p
                _writer.WriteEndElement(); // Close of Footer-Left

                _writer.WriteEndElement(); // Close of Master page Standard

                ///STANDARD CODE PART ENDS

                ////XHTML CODE PART START

                _writer.WriteStartElement("style:master-page");
                _writer.WriteAttributeString("style:name", "XHTML"); // All PageProperty
                _writer.WriteAttributeString("style:page-layout-name", "pm2");
                if (!isMirrored)
                {
                    /* Begin AllPage Header */
                    if (_pageHeaderFooter[6].Count > 0 || _pageHeaderFooter[7].Count > 0 || _pageHeaderFooter[8].Count > 0)
                    {
                        _writer.WriteStartElement("style:header");
                        _writer.WriteStartElement("text:p");
                        _writer.WriteAttributeString("text:style-name", "Header");
                        ////SetAllPageHeader();
                        _writer.WriteEndElement();
                        _writer.WriteEndElement();
                    }
                    /* Begin AllPage Footer */
                    if (_pageHeaderFooter[9].Count > 0 || _pageHeaderFooter[10].Count > 0 || _pageHeaderFooter[11].Count > 0)
                    {
                        _writer.WriteStartElement("style:footer");
                        _writer.WriteStartElement("text:p");
                        _writer.WriteAttributeString("text:style-name", "Footer");
                        ////SetAllPageFooter();
                        _writer.WriteEndElement(); // close of p
                        _writer.WriteEndElement(); // Close of Footer
                    }
                }
                _writer.WriteEndElement(); // Close of Master Page

                ////XHTML CODE PART ENDS

                //// First CODE PART START

                _writer.WriteStartElement("style:master-page");
                _writer.WriteAttributeString("style:name", "First_20_Page");
                _writer.WriteAttributeString("style:display-name", "First Page"); // First PageProperty
                if (isMirrored)
                {
                    _writer.WriteAttributeString("style:page-layout-name", "pm2");
                    _writer.WriteAttributeString("style:next-style-name", "Standard");
                }
                else
                {
                    _writer.WriteAttributeString("style:page-layout-name", "pm3");
                    _writer.WriteAttributeString("style:next-style-name", "XHTML");
                }
                /*Begin Firstpage Header */
                if (_pageHeaderFooter[0].Count > 0 || _pageHeaderFooter[1].Count > 0 || _pageHeaderFooter[2].Count > 0)
                {
                    if (IsCropMarkChecked)
                    {
                        _writer.WriteStartElement("style:header");
                        _writer.WriteStartElement("text:p");
                        _writer.WriteAttributeString("text:style-name", "Header");
                        if (IsCropMarkChecked)
                        {
                            foreach (KeyValuePair<string, string> para in _firstPageLayoutProperty)
                            {
                                _pageLayoutProperty[para.Key] = para.Value;
                            }

                            foreach (KeyValuePair<string, string> para in _pageLayoutProperty)
                            {
                                _writer.WriteAttributeString(para.Key, para.Value);
                            }
                            //AddHeaderCropMarks(_pageLayoutProperty);
                        }
                        if (_pageHeaderFooter[0].ContainsKey("content"))
                        {
                            _writer.WriteStartElement("text:span");
                            _writer.WriteAttributeString("text:style-name", "HeaderPageLeft");
                            //FillHeaderFooter(_pageHeaderFooter[0]["content"], 0);
                            _writer.WriteEndElement();
                        }
                        _writer.WriteStartElement("text:tab");
                        _writer.WriteEndElement();
                        if (_pageHeaderFooter[1].ContainsKey("content"))
                        {
                            _writer.WriteStartElement("text:span");
                            _writer.WriteAttributeString("text:style-name", "HeaderPageNumber");
                            //FillHeaderFooter(_pageHeaderFooter[1]["content"], 1);
                            _writer.WriteEndElement();
                        }
                        _writer.WriteStartElement("text:tab");
                        _writer.WriteEndElement();
                        if (_pageHeaderFooter[2].ContainsKey("content"))
                        {
                            _writer.WriteStartElement("text:span");
                            _writer.WriteAttributeString("text:style-name", "HeaderPageRight");
                            //FillHeaderFooter(_pageHeaderFooter[2]["content"], 2);
                            _writer.WriteEndElement();
                        }
                        _writer.WriteEndElement(); // Close of p
                        _writer.WriteEndElement(); // Close of Header
                    }
                }

                /*Begin Firstpage Footer */
                if (_pageHeaderFooter[3].Count > 0 || _pageHeaderFooter[4].Count > 0 || _pageHeaderFooter[5].Count > 0)
                {
                    if (false)
                    {
                        _writer.WriteStartElement("style:footer");
                        _writer.WriteStartElement("text:p");
                        _writer.WriteAttributeString("text:style-name", "Footer");

                        if (_pageHeaderFooter[3].ContainsKey("content"))
                        {
                            _writer.WriteStartElement("text:span");
                            _writer.WriteAttributeString("text:style-name", "FooterPageLeft");
                            //FillHeaderFooter(_pageHeaderFooter[3]["content"], 3);
                            _writer.WriteEndElement();
                        }
                        _writer.WriteStartElement("text:tab");
                        _writer.WriteEndElement();

                        if (_pageHeaderFooter[4].ContainsKey("content"))
                        {
                            _writer.WriteStartElement("text:span");
                            _writer.WriteAttributeString("text:style-name", "FooterPageNumber");
                            //FillHeaderFooter(_pageHeaderFooter[4]["content"], 4);
                            _writer.WriteEndElement();
                        }

                        _writer.WriteStartElement("text:tab");
                        _writer.WriteEndElement();

                        if (_pageHeaderFooter[5].ContainsKey("content"))
                        {
                            _writer.WriteStartElement("text:span");
                            _writer.WriteAttributeString("text:style-name", "FooterPageRight");
                            //FillHeaderFooter(_pageHeaderFooter[5]["content"], 5);
                            _writer.WriteEndElement();
                        }

                        _writer.WriteEndElement(); // Close of p
                        _writer.WriteEndElement(); // Close of Footer
                    }
                    /*End Firstpage Footer */
                }
                _writer.WriteEndElement(); // Close of Master Page
                //// First CODE PART ENDS
                _writer.WriteEndDocument();
                _writer.Flush();
                _writer.Close();

                try
                {
                    if (_verboseWriter.ShowError && _verboseWriter.ErrorWritten)  // error file closing
                    {
                        _verboseWriter.WriteError("</table>");
                        _verboseWriter.WriteError("</body>");
                        _verboseWriter.WriteError("</html>");
                        _verboseWriter.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                _writer.Flush();
                _writer.Close();
            }
        }







        ///// <summary>
        ///// Increase font-size 250% for Subscript and Superscript
        ///// </summary>
        ///// <param name="isIncrease">to increase font-size even the property is not super/sub script</param>
        //private void SuperscriptSubscriptIncreaseFontSize(bool isIncrease)
        //{
        //    bool isSuperSub = false;
        //    if (_IDProperty.ContainsKey("Position") && (_IDProperty["Position"] == "Subscript" || _IDProperty["Position"] == "Superscript"))
        //    {
        //        isSuperSub  = true;
        //    }

        //    if (isSuperSub || isIncrease) // increase font-size for superscipt & subscript
        //    {
        //        string newValue = "100%";
        //        if (_IDProperty.ContainsKey("PointSize"))
        //        {
        //            string fontValue = _IDProperty["PointSize"];
        //            int counter;
        //            string retValue = Common.GetNumericChar(fontValue, out counter);
        //            if (retValue.Length > 0)
        //            {
        //                float value = float.Parse(retValue) * 2.5F;
        //                string unit = fontValue.Substring(counter);
        //                newValue = value + unit;
        //            }
        //            else
        //            {
        //                if (fontValue == "larger" || fontValue == "smaller")
        //                {
        //                    newValue = fontValue;
        //                }
        //            }
        //        }
        //        _IDProperty["PointSize"] = newValue;
        //    }
        //}

        //private void CreateParagraphStyle()
        //{
        //    foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _cssProperty)
        //    {
        //        Writer.WriteStartElement("ParagraphStyle");
        //        Writer.WriteAttributeString("Self", "ParagraphStyle/" + cssClass.Key);
        //        Writer.WriteAttributeString("Name", cssClass.Key);
        //        Writer.WriteAttributeString("Imported", "false");
        //        Writer.WriteAttributeString("NextStyle", "ParagraphStyle/" + cssClass.Key);
        //        Writer.WriteAttributeString("KeyboardShortcut", "0 0");
        //        _IDProperty = mapProperty.IDProperty(cssClass.Value);
        //        SuperscriptSubscriptIncreaseFontSize(false);
        //        PositionProperty();

        //        _IDClass = new Dictionary<string, string>(); // note: ToDo seperate the process
        //        _IDAllClass[cssClass.Key] = _IDClass;

        //        foreach (KeyValuePair<string, string> property in _IDProperty)
        //        {
        //            if (property.Key == "AppliedFont")
        //            {
        //                _IDClass[property.Key] = property.Value;
        //                continue;
        //            }
        //            if (property.Key == "StrokeColor")
        //            {
        //                _IDClass[property.Key] = property.Value;
        //                InsertBackgroundColor(property.Value);
        //            }
        //            else
        //            {
        //                _IDClass[property.Key] = property.Value;
        //                Writer.WriteAttributeString(property.Key, property.Value);
        //            }
        //        }
        //        Writer.WriteStartElement("Properties");
        //        Writer.WriteStartElement("BasedOn");
        //        Writer.WriteAttributeString("type", "string");
        //        Writer.WriteString("$ID/[No paragraph style]");
        //        Writer.WriteEndElement();
        //        Writer.WriteStartElement("PreviewColor");
        //        Writer.WriteAttributeString("type", "enumeration");
        //        Writer.WriteString("Nothing");
        //        Writer.WriteEndElement();
        //        CreateParagraphProperty("AppliedFont", "string");
        //        string propertyType = Common.GetLeadingType(_IDProperty);
        //        CreateParagraphProperty("Leading", propertyType);
        //        Writer.WriteEndElement();

        //        Writer.WriteEndElement();
        //    }
        //    Writer.WriteEndElement(); //End RootParagraphStyleGroup
        //}

        //private void CreateCharacterStyle()
        //{
        //    foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _cssProperty)
        //    {
        //        if (cssClass.Value.ContainsKey("display") && (cssClass.Value["display"] == "footnote"
        //                                                       || cssClass.Value["display"] == "prince-footnote"))
        //        {

        //            Writer.WriteStartElement("CharacterStyle");
        //            Writer.WriteAttributeString("Self", "CharacterStyle/" + cssClass.Key);
        //            Writer.WriteAttributeString("Name", cssClass.Key);
        //            Writer.WriteAttributeString("Imported", "false");
        //            Writer.WriteAttributeString("NextStyle", "CharacterStyle/" + cssClass.Key);
        //            Writer.WriteAttributeString("KeyboardShortcut", "0 0");
        //            _IDProperty = mapProperty.IDProperty(cssClass.Value);
        //            SuperscriptSubscriptIncreaseFontSize(true);
        //            _IDClass = new Dictionary<string, string>(); // note: ToDo seperate the process
        //            _IDAllClass[cssClass.Key] = _IDClass;

        //            foreach (KeyValuePair<string, string> property in _IDProperty)
        //            {
        //                if (property.Key == "AppliedFont")
        //                {
        //                    _IDClass[property.Key] = property.Value;
        //                    continue;
        //                }
        //                if (property.Key == "StrokeColor")
        //                {
        //                    _IDClass[property.Key] = property.Value;
        //                    InsertBackgroundColor(property.Value);
        //                }
        //                else
        //                {
        //                    _IDClass[property.Key] = property.Value;
        //                    Writer.WriteAttributeString(property.Key, property.Value);
        //                }
        //            }
        //            Writer.WriteStartElement("Properties");
        //            Writer.WriteStartElement("BasedOn");
        //            Writer.WriteAttributeString("type", "string");
        //            Writer.WriteString("$ID/[No paragraph style]");
        //            Writer.WriteEndElement();
        //            Writer.WriteStartElement("PreviewColor");
        //            Writer.WriteAttributeString("type", "enumeration");
        //            Writer.WriteString("Nothing");
        //            Writer.WriteEndElement();
        //            Writer.WriteStartElement("Leading");
        //            Writer.WriteAttributeString("type", "enumeration");
        //            Writer.WriteString("Auto");
        //            Writer.WriteEndElement();
        //            Writer.WriteEndElement();
        //            Writer.WriteEndElement();
        //        }
        //    }
        //    Writer.WriteEndElement(); //End of RootCharacterStyleGroup
        //}



        //private void PositionProperty()
        //{
        //    //Note: Paragraph Margins are not Completed.
        //    //Note: Currently "left" and "right" are added to padding because of Indesign 
        //    //Note: which does not have property for Paragraph Margin.
        //    //Note: Also It does not support the Negative Values in Margin. Everything restricted within the frames.

        //        if (_IDProperty.ContainsKey("position") && (_IDProperty.ContainsKey("left") || _IDProperty.ContainsKey("right")) )
        //        {
        //            _IDProperty.Remove("position");

        //            if (_IDProperty.ContainsKey("left"))
        //            {
        //                if (_IDProperty.ContainsKey("LeftIndent"))
        //                {
        //                    _IDProperty["LeftIndent"] = (int.Parse(_IDProperty["LeftIndent"]) 
        //                                                + int.Parse(_IDProperty["left"])).ToString();
        //                }
        //                else
        //                {
        //                    _IDProperty["LeftIndent"] = _IDProperty["left"];
        //                }
        //            }
        //            else if (_IDProperty.ContainsKey("right"))
        //            {
        //                if (_IDProperty.ContainsKey("RightIndent"))
        //                {
        //                    _IDProperty["RightIndent"] = (int.Parse(_IDProperty["RightIndent"]) 
        //                                                 + int.Parse(_IDProperty["right"])).ToString();
        //                }
        //                else
        //                {
        //                    _IDProperty["RightIndent"] = _IDProperty["right"];
        //                }
        //            }
        //        }

        //        if (_IDProperty.ContainsKey("position"))
        //        {
        //            _IDProperty.Remove("position");
        //        }
        //        if (_IDProperty.ContainsKey("left"))
        //        {
        //            _IDProperty.Remove("left");
        //        }
        //        if (_IDProperty.ContainsKey("right"))
        //        {
        //            _IDProperty.Remove("right");
        //        }
        //}

        //private void CreateParagraphProperty(string propertyName, string propertyType)
        //{
        //    if (_IDProperty.ContainsKey(propertyName))
        //    {
        //        Writer.WriteStartElement(propertyName);
        //        Writer.WriteAttributeString("type", propertyType);
        //        Writer.WriteString(_IDProperty[propertyName]);
        //        Writer.WriteEndElement();
        //    }
        //}

        //private void InsertBackgroundColor(string propertyValue)
        //{
        //    _IDClass["StrokeWeight"] = "1";
        //    _IDClass["StrokeColor"] = propertyValue;
        //    _IDClass["EndJoin"] = "BevelEndJoin";

        //    Writer.WriteAttributeString("StrokeWeight", "1");
        //    Writer.WriteAttributeString("StrokeColor", propertyValue);
        //    Writer.WriteAttributeString("EndJoin", "BevelEndJoin");
        //}
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Merge Standard tag property(h1) to css tag property(h1.entry)
        /// <list> 
        /// </list>
        /// </summary>
        /// <returns>Dictionary with TAG Properties.</returns>
        /// -------------------------------------------------------------------------------------------
        protected void MergeTag()
        {
            Dictionary<string, string> tagProperty = new Dictionary<string, string>();
            OOUtility util = new OOUtility();
            XmlDocument doc = new XmlDocument();
            doc.Load(_styleFilePath);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("fo", "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
            XmlElement root = doc.DocumentElement;
            try
            {
                string[] _aeName = new string[2];
                bool isPropertyExist;
                //foreach (string tag in _tagName) // for each tag like h1, h2 etc.
                foreach (string tag in _aeName) // for each tag like h1, h2 etc.
                {
                    string style = "//st:style[@st:name='" + tag + "']"; // Find class
                    XmlNode node = root.SelectSingleNode(style, nsmgr);

                    XmlNode paragraphNode = null;
                    XmlNode textNode = null;
                    for (int i = 0; i < node.ChildNodes.Count; i++) // find paragraph & Text node
                    {
                        if (node.ChildNodes[i].Name == "style:paragraph-properties")
                        {
                            paragraphNode = node.ChildNodes[i];
                        }
                        else if (node.ChildNodes[i].Name == "style:text-properties")
                        {
                            textNode = node.ChildNodes[i];
                        }
                    }

                    string[] tagClass = tag.Split('.'); // tagClass[0] - className & tagClass[1] - TagName
                    string tagClassN = string.Empty;
                    if (tagClass.Length == 1)
                    {
                        tagClassN = tagClass[0];
                    }
                    else if (tagClass.Length == 2)
                    {
                        tagClassN = tagClass[1];
                    }
                    tagProperty = _tagProperty[tagClassN];

                    foreach (KeyValuePair<string, string> prop in tagProperty) // font-size: etc.
                    {
                        isPropertyExist = false;

                        for (int i = 0; i < node.ChildNodes.Count; i++) // open paragraph & Text node
                        {
                            XmlNode child = node.ChildNodes[i];

                            foreach (XmlAttribute attribute in child.Attributes) // open it'line attributes
                            {
                                if (prop.Key == attribute.Name)
                                {
                                    isPropertyExist = true;
                                    i = 10; // exit two loops
                                    break;
                                }
                            }
                        }
                        if (isPropertyExist == false)
                        {
                            string propertyName = prop.Key.Substring(prop.Key.IndexOf(':') + 1);
                            if (_allParagraphProperty.ContainsKey(propertyName)) // add property in Paragraph node
                            {
                                if (paragraphNode == null)
                                {
                                    paragraphNode = node.InsertBefore(doc.CreateElement("style:paragraph-properties", nsmgr.LookupNamespace("st").ToString()), node.FirstChild);
                                }
                                paragraphNode.Attributes.Append(doc.CreateAttribute(prop.Key, nsmgr.LookupNamespace("st").ToString())).InnerText = prop.Value;
                            }
                            else if (_allTextProperty.ContainsKey(propertyName)) // add fullString in Paragraph node
                            {
                                if (textNode == null)
                                {
                                    textNode = node.AppendChild(doc.CreateElement("style:text-properties", nsmgr.LookupNamespace("st").ToString()));
                                }
                                textNode.Attributes.Append(doc.CreateAttribute(prop.Key, nsmgr.LookupNamespace("fo").ToString())).InnerText = prop.Value;
                            }
                        }
                    }

                    if (tagClassN == "div")
                    {
                        XmlDocumentFragment styleNode = doc.CreateDocumentFragment();
                        styleNode.InnerXml = node.OuterXml;
                        node.ParentNode.InsertAfter(styleNode, node);

                        XmlElement nameElement = (XmlElement)node;
                        nameElement.SetAttribute("style:name", tagClass[0]);
                    }
                }
                doc.Save(_styleFilePath);
            }
            catch
            {
            }
        }

       
 
    }
}