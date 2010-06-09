using System.Xml;

namespace SIL.PublishingSolution
{
    public class LiftDocument
    {
        private XmlDocument liftAsXML;

        public LiftDocument(string pathToLiftDocument)
        {
            liftAsXML = new XmlDocument();
            liftAsXML.Load(pathToLiftDocument);
        }

        public LiftDocument(XmlDocument liftAsXML)
        {
            this.liftAsXML = liftAsXML;
        }

        public LiftDocument()
        {
            
        }

        public XmlNode getLiftNode()
        {
            return liftAsXML.SelectSingleNode("lift");
        }

        public LiftEntries getEntries()
        {
            return new LiftEntries(liftAsXML.SelectNodes("lift/entry"));
        }

        public void Load(LiftReader reader)
        {
            instantiateLiftAsXMLIfNeeded();
            liftAsXML.Load(reader);
        }

        public XmlDocument asXmlDocument()
        {
            instantiateLiftAsXMLIfNeeded();
            return liftAsXML;
        }

        public XmlNodeList nodesWithLangAttributes()
        {
            return liftAsXML.SelectNodes("/lift/entry/descendant::*[@lang]");
        }

        public void Save(LiftWriter writer)
        {
            liftAsXML.Save(writer);
        }

        private void instantiateLiftAsXMLIfNeeded()
        {
            if (liftAsXML == null)
            {
                liftAsXML = new XmlDocument();
            }
        }
    }
}