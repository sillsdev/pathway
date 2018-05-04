// --------------------------------------------------------------------------------------------
// <copyright file="TableOfContent.cs" from='2009' to='2014' company='SIL International'>
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
// Creates the Table of Contents  
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Xml;

namespace SIL.PublishingSolution
{
    public class TableOfContent
    {
        public void CreateTOC(XmlTextWriter _writer, string inputType, string outlineLevel, string tocStyle)
        {
            _writer.WriteStartElement("text:p");
            _writer.WriteAttributeString("text:style-name", "P2"); //To give page break
            _writer.WriteEndElement();

            _writer.WriteStartElement("text:table-of-content");
            _writer.WriteAttributeString("text:style-name", "toc_revAppendix");
            _writer.WriteAttributeString("text:protected", "true");
            _writer.WriteAttributeString("text:name", "Table of Contents1");

            _writer.WriteStartElement("text:table-of-content-source");
            _writer.WriteAttributeString("text:outline-level", "2");
            _writer.WriteAttributeString("text:use-outline-level", "true");
            _writer.WriteAttributeString("text:use-index-marks", "true");
            _writer.WriteAttributeString("text:use-index-source-styles", "false");


            _writer.WriteStartElement("text:index-title-template");
            _writer.WriteAttributeString("text:style-name", "Contents_20_Heading");
            _writer.WriteString("Table of Contents");
            _writer.WriteEndElement();


            _writer.WriteStartElement("text:table-of-content-entry-template");
            _writer.WriteAttributeString("text:outline-level", "1");
            _writer.WriteAttributeString("text:style-name", "Contents_20_1");
            //TD-3106
            _writer.WriteStartElement("text:index-entry-link-start");
            _writer.WriteAttributeString("text:style-name", "Index_20_Link");
            _writer.WriteEndElement();

            _writer.WriteStartElement("text:index-entry-chapter");
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:index-entry-text");
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:index-entry-tab-stop");
            _writer.WriteAttributeString("style:type", "right");
            _writer.WriteAttributeString("style:leader-char", ".");
            _writer.WriteEndElement();
            _writer.WriteStartElement("text:index-entry-page-number");
            _writer.WriteEndElement();
            //TD-3106
            _writer.WriteStartElement("text:index-entry-link-end");
            _writer.WriteEndElement();

            _writer.WriteEndElement();

            _writer.WriteStartElement("text:index-source-styles");
            _writer.WriteAttributeString("text:outline-level", outlineLevel);//"1"
            _writer.WriteStartElement("text:index-source-style");

            _writer.WriteAttributeString("text:style-name", tocStyle); //TD-4017

            //if (inputType.ToLower() == "dictionary")
            //{
            //    //_writer.WriteAttributeString("text:style-name", "letter_letHead_dicBody");
            //    _writer.WriteAttributeString("text:style-name", "letter_letHead_body");
            //}
            //else if (inputType.ToLower() == "scripture")
            //{
            //    _writer.WriteAttributeString("text:style-name", "scrBook_scrBody");
            //}
            
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteEndElement();

            _writer.WriteStartElement("text:index-body");
            _writer.WriteStartElement("text:index-title");
            _writer.WriteAttributeString("text:style-name", "Sect3");
            _writer.WriteAttributeString("text:name", "Table of Contents1_Head");
            _writer.WriteStartElement("text:p");
            _writer.WriteAttributeString("text:style-name", "Contents_20_Heading");
            _writer.WriteString("Table of Contents");
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("text:p");
            _writer.WriteAttributeString("text:style-name", "Contents_20_1");
            _writer.WriteString("test");
            _writer.WriteStartElement("text:tab");
            _writer.WriteEndElement();
            _writer.WriteString("1");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();

            _writer.WriteStartElement("text:p");
            _writer.WriteAttributeString("text:style-name", "copyright_dicBody"); //To give page break
            _writer.WriteEndElement();
            
        }
    }
}
