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
using System.Collections.Generic;

namespace SIL.PublishingSolution
{
    public class OdtStyles : OdtStyleBase
    {
        #region Private Variables

        //XmlTextWriter _writer;

        Dictionary<string, Dictionary<string, string>> _cssProperty = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, Dictionary<string, string>> _IDAllClass = new Dictionary<string, Dictionary<string, string>>();
        private Dictionary<string, string> _IDProperty = new Dictionary<string, string>();
        private Dictionary<string, string> _IDClass = new Dictionary<string, string>();
        //CSSTree _cssTree = new CSSTree();
        OOMapProperty mapProperty = new OOMapProperty();
        #endregion

        #region Constructor
        #endregion

        public Dictionary<string, Dictionary<string, string>> CreateOdtStyles(string projectPath, Dictionary<string, Dictionary<string, string>> cssProperty)
        {
            try
            {
                //CSSTree cssTree = new CSSTree();
                _cssProperty = cssProperty;
                CreateFile(projectPath);
                CreateRootCharacterStyleGroup();
                CreateRootParagraphStyleGroup();
                CreateParagraphStyle();  // CODE HERE
                CreateTOCStyle();
                CreateRootCellStyleGroup();
                CreateRootTableStyleGroup();
                CreateRootObjectStyleGroup();
                CreateTrapPreset();
                EndIDStyles();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return _IDAllClass;
        }

        private void CreateParagraphStyle()
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _cssProperty)
            {
                _writer.WriteStartElement("ParagraphStyle");
                //_writer.WriteAttributeString("Self", "ParagraphStyle/$ID/" + cssClass.Key);
                _writer.WriteAttributeString("Self", "ParagraphStyle/" + cssClass.Key);
                _writer.WriteAttributeString("Name", cssClass.Key);
                _writer.WriteAttributeString("Imported", "false");
                //_writer.WriteAttributeString("NextStyle", "ParagraphStyle/$ID/" + cssClass.Key);
                _writer.WriteAttributeString("NextStyle", "ParagraphStyle/" + cssClass.Key);
                _writer.WriteAttributeString("KeyboardShortcut", "0 0");
                //_writer.WriteAttributeString("FillColor", "Color/" + cssClass.Key);
                _IDProperty = mapProperty.IDProperty(cssClass.Value);

                _IDClass = new Dictionary<string, string>(); // note: ToDo seperate the process
                _IDAllClass[cssClass.Key] = _IDClass;


                foreach (KeyValuePair<string, string> property in _IDProperty)
                {
                    _IDClass[property.Key] = property.Value;
                    if (property.Key == "AppliedFont")
                    {
                        continue;
                    }
                    _writer.WriteAttributeString(property.Key, property.Value);
                }

                _writer.WriteStartElement("Properties");
                _writer.WriteStartElement("BasedOn");
                _writer.WriteAttributeString("type", "string");
                _writer.WriteString("$ID/[No paragraph style]");
                _writer.WriteEndElement();
                _writer.WriteStartElement("PreviewColor");
                _writer.WriteAttributeString("type", "enumeration");
                _writer.WriteString("Nothing");
                _writer.WriteEndElement();
                if (_IDProperty.ContainsKey("AppliedFont"))
                {
                    _writer.WriteStartElement("AppliedFont");
                    _writer.WriteAttributeString("type", "string");
                    _writer.WriteString(_IDProperty["AppliedFont"]);
                    _writer.WriteEndElement();
                }
                _writer.WriteEndElement();
                _writer.WriteEndElement();
            }
            _writer.WriteEndElement(); //End RootParagraphStyleGroup
        }


    }
}