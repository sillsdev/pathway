// --------------------------------------------------------------------------------------------
// <copyright file="LiftDocument.cs" from='2009' to='2014' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

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