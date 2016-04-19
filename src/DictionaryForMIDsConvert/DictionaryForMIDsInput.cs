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
		private readonly bool _isLexicon;
	    private readonly bool _fw83;

		public DictionaryForMIDsInput(PublicationInformation projInfo)
		{
			Xml = LoadXmlDocument(projInfo);
			Nsmgr = GetNamespaceManager(Xml);
			_isLexicon = projInfo.IsLexiconSectionExist;
            _fw83 = Xml.SelectSingleNode("//@entryguid") != null;
		}

		~DictionaryForMIDsInput()
		{
			Xml.RemoveAll();
		}

		public XmlNodeList SelectNodes(string xpath)
		{
			return Xml.SelectNodes(xpath, Nsmgr);
		}

	    public bool Fw83()
	    {
	        return _fw83;
	    }

		public string VernacularIso()
		{

			if (_vernacularIso != null)
				return _vernacularIso;
			var vernLangPath = _isLexicon ? "//*[@class='headword']/@lang" : "//*[starts-with(@class,'reversal')]//@lang";
			var node = Xml.SelectSingleNode(vernLangPath, Nsmgr);
			if (node != null && node.InnerText != string.Empty)
			{
				_vernacularIso = node.InnerText;
			}
			else
			{
				if (node == null)
				{
					vernLangPath = _isLexicon ? "//*[@class='mainheadword']/*/@lang" : "//*[starts-with(@class,'reversal-form')]/@lang";
				}
				XmlNodeList nodes = Xml.SelectNodes(vernLangPath, Nsmgr);
				if (nodes.Count > 0)
				{
					_vernacularIso = nodes[0].Value;
				}
			}
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
			var analLangPath = _isLexicon ? "//*[@class='entry']//*[@id]//@lang" : "//*[@class='headref']/@lang";
		    if (_fw83)
		    {
                analLangPath = _isLexicon ? "//*[starts-with(@class,'defin')]//@lang" : "//*[@class='headref']//@lang";
		    }
			var node = Xml.SelectSingleNode(analLangPath, Nsmgr);

			if (node == null)
			{
				analLangPath = _isLexicon ? "//*[@class='entry']/*/*/@lang" : "//*[@class='headref']/@lang";
				node = Xml.SelectSingleNode(analLangPath, Nsmgr);
			}

			if (node != null && node.InnerText != string.Empty)
			{
				_analysisIso = node.InnerText;
			}
			else
			{
				_analysisIso = string.Empty;
			}
			return _analysisIso;
		}

		public string AnalysisName()
		{
			return Common.GetLanguageName(AnalysisIso());
		}

		protected static XmlDocument LoadXmlDocument(PublicationInformation projInfo)
		{
			var xml = new XmlDocument { XmlResolver = FileStreamXmlResolver.GetNullResolver() };
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
