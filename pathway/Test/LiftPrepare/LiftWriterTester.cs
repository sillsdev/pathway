using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using NUnit.Framework;
using SIL.PublishingSolution;

namespace Test.LiftPrepare
{
    [TestFixture]
    public class LiftWriterTester
    {
        [Test]
        public void testLiftWriter()
        {
            var liftWriter = new LiftWriter(TestEnvironment.ActualOutputRoot + @"simple/testLiftWriter.lift");
            Assert.AreEqual(typeof(XmlTextWriter), liftWriter.GetType().BaseType);
        }
    }
}