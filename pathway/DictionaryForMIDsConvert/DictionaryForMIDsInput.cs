// --------------------------------------------------------------------------------------------
// <copyright file="DictionaryForMIDsInput.cs" from='2013' to='2014' company='SIL International'>
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
// Stylepick FeatureSheet
// </remarks>
// --------------------------------------------------------------------------------------------
using System.Diagnostics;
using System.IO;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class DictionaryForMIDsInput
    {
        private string _vernacularIso;
        private string _analysisIso;

        protected XmlNamespaceManager Nsmgr;
        protected XmlDocument Xml;
        private bool IsLexicon;

        public DictionaryForMIDsInput(PublicationInformation projInfo)
        {
            Xml = LoadXmlDocument(projInfo);
            Nsmgr = GetNamespaceManager(Xml);
            IsLexicon = projInfo.IsLexiconSectionExist;
        }

        ~DictionaryForMIDsInput()
        {
            Xml.RemoveAll();
        }

        public XmlNodeList SelectNodes(string xpath)
        {
            return Xml.SelectNodes(xpath, Nsmgr);
        }

        public string VernacularIso()
        {
            if (_vernacularIso != null)
                return _vernacularIso;
            var vernLangPath = IsLexicon? "//*[@class='headword']/@lang": "//*[starts-with(@class,'reversal-form')]/@lang";
            var node = Xml.SelectSingleNode(vernLangPath, Nsmgr);
            Debug.Assert(node != null);
            _vernacularIso = node.InnerText;
            return _vernacularIso;
        }

        public string VernacularName()
        {
            return Common.GetLanguageName(VernacularIso());
        }

        public string AnalysisIso()
        {
            if (_analysisIso != null)
                return _analysisIso;
            var analLangPath = IsLexicon? "//*[@class='entry']//*[@id]//@lang": "//*[@class='headref']/@lang";
            var node = Xml.SelectSingleNode(analLangPath, Nsmgr);
            Debug.Assert(node != null);
            _analysisIso = node.InnerText;
            return _analysisIso;
        }

        public string AnalysisName()
        {
            return Common.GetLanguageName(AnalysisIso());
        }

        protected static XmlDocument LoadXmlDocument(PublicationInformation projInfo)
        {
            var xml = new XmlDocument {XmlResolver = FileStreamXmlResolver.GetNullResolver()};
            var streamReader = new StreamReader(projInfo.DefaultXhtmlFileWithPath);
            xml.Load(streamReader);
            streamReader.Close();
            return xml;
        }

        protected static XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDocument, string defaultNs)
        {
            var root = xmlDocument.DocumentElement;
            Debug.Assert(root != null, "Missing xml document");
            var nsManager = new XmlNamespaceManager(xmlDocument.NameTable);
            foreach (XmlAttribute attribute in root.Attributes)
            {
                if (attribute.Name == "xmlns")
                {
                    nsManager.AddNamespace(defaultNs, attribute.Value);
                    continue;
                }
                var namePart = attribute.Name.Split(':');
                if (namePart[0] == "xmlns")
                    nsManager.AddNamespace(namePart[1], attribute.Value);
            }
            return nsManager;
        }

        protected static XmlNamespaceManager GetNamespaceManager(XmlDocument xmlDocument)
        {
            return GetNamespaceManager(xmlDocument, "xhtml");
        }
    }
}
