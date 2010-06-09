using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using NUnit.Framework;
using System.Xml;
using SIL.PublishingSolution;
using SIL.PublishingSolution.Sort;

namespace Test.LiftPrepare
{
    [TestFixture]
    public class WritingSystemSorterTester
    {
        private const string TestingDirectory = @"..\..\LiftPrepare\TestFiles\";
        private const string InputDirectory = TestingDirectory + @"Input\";
        private const string ActualOutputDirectory = TestingDirectory + @"Output\";
        private const string ExpectedOutputDirectory = TestingDirectory + @"Expected\";

        [Test]
        public void sortTest()
        {
            var liftReader = new LiftReader(InputDirectory+@"yi\yi.lift");
            var liftDocument = new LiftDocument();
            liftDocument.Load(liftReader);
            var wsorter = new LiftLangSorter(liftDocument);
            wsorter.sortWritingSystems();
        }

        [Test]
        public void replaceNodesTest()
        {
            var xmlReader = new XmlTextReader(InputDirectory + @"simple\replacenodes.xml");
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlReader);
            var nodesAsXmlNodeList = xmlDocument.SelectNodes("letters/*");
            var wssorter = new LiftLangSorter();
            var nodes = wssorter.getListFromXMLNodList(nodesAsXmlNodeList);
            nodes.Sort(new XmlNodeComparer());
            wssorter.replaceSortedNodes(nodes);
            var xmlWriter = new XmlTextWriter(ActualOutputDirectory + @"simple\replacenodes.xml", null);
            xmlDocument.Save(xmlWriter);
        }

        private class XmlNodeComparer : IComparer<XmlNode>
        {
            public int Compare(XmlNode x, XmlNode y)
            {
                return x.InnerText.CompareTo(y.InnerText);
            }
        }
    }
}