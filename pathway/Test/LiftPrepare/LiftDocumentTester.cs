using System.IO;
using NUnit.Framework;
using SIL.PublishingSolution;

namespace Test.LiftPrepare
{
    [TestFixture]
    public class LiftDocumentTester
    {
        LiftDocument liftDocument = new LiftDocument();

        [TestFixtureSetUp]
        public void Setup()
        {
            if (!Directory.Exists(TestEnvironment.ActualOutputRoot))
                Directory.CreateDirectory(TestEnvironment.ActualOutputRoot);
            var simplePath = TestEnvironment.ActualOutputRoot + @"simple";
            if (!Directory.Exists(simplePath))
                Directory.CreateDirectory(simplePath);
        }

        [Test]
        public void testGetLiftNode()
        {
            var liftReader = new LiftReader(TestEnvironment.InputRoot + @"simple\validLift.lift");
            liftDocument.Load(liftReader);
            var liftRootNode = liftDocument.getLiftNode();
            StringAssert.AreEqualIgnoringCase("lift",liftRootNode.Name);
        }

        [Test]
        public void testGetEntries()
        {
            var liftReader = new LiftReader(TestEnvironment.InputRoot + @"simple\liftEntries.lift");
            liftDocument.Load(liftReader);
            var liftEntries = liftDocument.getEntries();
            Assert.AreEqual(10, liftEntries.Count());
            foreach (var liftEntry in liftEntries)
            {
                Assert.AreEqual(typeof(LiftEntry), liftEntry.GetType());
            }
            
        }
    }
}