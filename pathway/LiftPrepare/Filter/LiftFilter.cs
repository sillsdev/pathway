using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace SIL.PublishingSolution.Filter
{
    public class LiftFilter : XmlDocument
    {
        public LiftFilter(string uri)
        {
            Load(uri);
        }

        public LiftFilter()
        {
        }

        public void loadEntryAndSenseTemplate()
        {
            Load(Environ.pathToFilterTemplates + @"liftEntryAndSenseFilter.xsl");
        }

        public void loadLanguageFilterTemplate()
        {
            Load(Environ.pathToFilterTemplates + @"liftLangFilter.xsl");
        }

        public void insertEntryAndSenseFilters(LiftEntryAndSenseFilters filters)
        {
            var entryFilterChoose = SelectSingleNode("//*[name()='template' and @name='filterEntry']/descendant::*[name()='choose']");
            var senseFilterChoose = SelectSingleNode("//*[name()='template' and @name='filterSense']/descendant::*[name()='choose']");
            foreach (LiftFilterChooseStatement filter in filters)
            {
                if (filter.applyTo == "sense")
                {
                    var node = filter.getFilterAsXmlNode(this);
                    senseFilterChoose.PrependChild(node.FirstChild);
                }
                else
                {
                    var node = filter.getFilterAsXmlNode(this);
                    entryFilterChoose.PrependChild(node.FirstChild);
                }
            }
        }

        public void insertLanguageFilters(LiftLangFilters filters)
        {
            var languageFilterChoose = SelectSingleNode("//*[name()='template' and @name='filterLang']/descendant::*[name()='choose']");
            foreach (LiftFilterChooseStatement filter in filters)
            {
                var node = filter.getFilterAsXmlNode(this);
                languageFilterChoose.PrependChild(node.FirstChild);
            }
        }

        public LiftDocument applyTo(LiftDocument documentToFilter)
        {
            var transformer = new XslCompiledTransform();
            transformer.Load(this);
            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);
            transformer.Transform(documentToFilter.asXmlDocument(), null, writer);
            writer.Close();
            var reader = new StringReader(buffer.ToString());
            var doc = new XmlDocument();
            doc.Load(reader);
            return new LiftDocument(doc);
        }
    }
}