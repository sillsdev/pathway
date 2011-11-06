using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NUnit.Framework;
using RevHomographNum;
using SIL.Tool;

namespace Test
{
    [TestFixture]
    public class WorkArounds
    {
        private TestFiles _tf;
        
        [TestFixtureSetUp]
        public void Setup()
        {
            _tf = new TestFiles("PsExport");
        }

        [Test]
        public void AddHomographAndSenseNumClassNamesTest()
        {
            const string testData = "FlexRev.xhtml";
            var FlexRevFullName = _tf.Copy(testData);
            Common.StreamReplaceInFile(FlexRevFullName, "class=\"headword\"", "class=\"headref\"");
            AddHomographAndSenseNumClassNames.Execute(FlexRevFullName, FlexRevFullName);
            var actual = new XmlDocument {XmlResolver = null};
            actual.Load(FlexRevFullName);
            var nodes = actual.SelectNodes("//*[@class='revhomographnumber']");
            Assert.AreEqual(8,nodes.Count);
        }
    }
}
