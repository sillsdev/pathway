// --------------------------------------------------------------------------------------------
// <copyright file="WritingSystemSorterTester.cs" from='2009' to='2014' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

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
        private static string TestingDirectory = PathPart.Bin(Environment.CurrentDirectory, "/LiftPrepare/TestFiles/");
        private static string InputDirectory = TestingDirectory + @"Input/";
        private static string ActualOutputDirectory = TestingDirectory + @"Output/";
        private static string ExpectedOutputDirectory = TestingDirectory + @"Expected/";

        [Test]
        [Category("SkipOnTeamCity")]
        public void sortTest()
        {
            var liftReader = new LiftReader(InputDirectory+@"yi/yi.lift");
            var liftDocument = new LiftDocument();
            liftDocument.Load(liftReader);
            var wsorter = new LiftLangSorter(liftDocument);
            wsorter.sortWritingSystems();
        }

        [Test]
        public void replaceNodesTest()
        {
            var xmlReader = new XmlTextReader(InputDirectory + @"simple/replacenodes.xml");
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlReader);
            var nodesAsXmlNodeList = xmlDocument.SelectNodes("letters/*");
            var wssorter = new LiftLangSorter();
            var nodes = wssorter.getListFromXMLNodList(nodesAsXmlNodeList);
            nodes.Sort(new XmlNodeComparer());
            wssorter.replaceSortedNodes(nodes);
            var xmlWriter = new XmlTextWriter(ActualOutputDirectory + @"simple/replacenodes.xml", null);
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