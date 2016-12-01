// --------------------------------------------------------------------------------------------
// <copyright file="OOUtility.cs" from='2009' to='2014' company='SIL International'>
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
// Utility Class is used by Styles.cs
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using
using System;
using System.Collections;
using System.IO;
using System.Xml;
using SIL.Tool;

#endregion Using
namespace SIL.PublishingSolution
{
    #region Class OOUtility
    /// <summary>
    /// Utility Class is used by Styles.cs
    /// </summary>
    public class OOUtility
    {
        #region Private Variables
	
		#pragma warning disable 649
		private string _parentName;
		#pragma warning restore 649

	    #endregion

        #region Public Properties

        public string ParentName
        {
            get { return _parentName; }
        }

        #endregion

        // Public Methods
        #region CreateMasterContents(string styleFilePath, ArrayList odtFiles)
        public void CreateMasterContents(string styleFilePath, ArrayList odtFiles)
        {
            var doc = new XmlDocument();
            doc.Load(styleFilePath);
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
            nsmgr.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
            nsmgr.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
          

            // if new stylename exists
            XmlElement root = doc.DocumentElement;
            string style = "//office:text";
            if (root != null)
            {

                XmlNode node = root.SelectSingleNode(style, nsmgr); // work
                if (node != null)
                {
                    for (int i = 0; i < odtFiles.Count; i++) // ODM - ODT 
                    {
                        string outputFile = odtFiles[i].ToString();
                        outputFile = Path.ChangeExtension(outputFile, "odt");
                        XmlNode newNode;
                        newNode = doc.CreateNode("element", "text:section", nsmgr.LookupNamespace("text"));
                        //attribute
                        XmlAttribute xmlAttrib = doc.CreateAttribute("text:style-name", nsmgr.LookupNamespace("text"));
                        xmlAttrib.Value = "SectODM";
                        newNode.Attributes.Append(xmlAttrib);

                        xmlAttrib = doc.CreateAttribute("text:name", nsmgr.LookupNamespace("text"));
                        xmlAttrib.Value = outputFile;
                        newNode.Attributes.Append(xmlAttrib);

                        xmlAttrib = doc.CreateAttribute("text:protected", nsmgr.LookupNamespace("text"));
                        xmlAttrib.Value = "false";
                        newNode.Attributes.Append(xmlAttrib);


                        XmlNode newNode1 = doc.CreateNode("element", "text:section-source", nsmgr.LookupNamespace("text"));
                        //attribute
                        XmlAttribute xmlAttrib1 = doc.CreateAttribute("xlink:href", nsmgr.LookupNamespace("xlink"));
                        xmlAttrib1.Value = "../" + outputFile;
                        newNode1.Attributes.Append(xmlAttrib1);


                        xmlAttrib1 = doc.CreateAttribute("text:filter-name", nsmgr.LookupNamespace("text"));
                        xmlAttrib1.Value = "writer8";
                        newNode1.Attributes.Append(xmlAttrib1);

                        newNode.AppendChild(newNode1);
                        node.AppendChild(newNode);
                    }
                }
            }

            doc.Save(styleFilePath);

        }
        #endregion

        #region GraphicContentChange(string contentFilePath,ArrayList graphicNames)
        public void GraphicContentChange(string contentFilePath,ArrayList graphicNames)
        {
            if(graphicNames.Count == 0) return;
            XmlDocument doc = Common.DeclareXMLDocument(true);
           
            string file;
            if (!Common.Testing)
            {
                file = Common.PathCombine(contentFilePath, "content.xml");
            }
            else
            {
                file = contentFilePath + "content.xml";
            }
            doc.Load(file);

            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("st", "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
            nsmgr.AddNamespace("draw", "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");

            foreach (string graphicName in graphicNames)
            {
                string makeClassName = graphicName; // graphicNames[0].ToString();
                if (String.IsNullOrEmpty(makeClassName)) return;

                XmlElement root = doc.DocumentElement;
                //<draw:frame draw:style-name="Graphics0" 
                string style = "//draw:frame[@draw:style-name='" + makeClassName + "']";
                if (root != null)
                {
                    XmlNode node = root.SelectSingleNode(style, nsmgr); // work
                    if (node == null)
                    {
                        return;
                    }

                    XmlDocumentFragment styleNode = doc.CreateDocumentFragment();
                    styleNode.InnerXml = node.FirstChild.FirstChild.OuterXml;
                    node.ParentNode.ReplaceChild(styleNode, node);
                }
            }
            doc.Save(file);
        }
        }
        #endregion
        // End - Public Methods
    }
    #endregion


