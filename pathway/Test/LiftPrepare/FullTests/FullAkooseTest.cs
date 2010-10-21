using System.IO;
using NUnit.Framework;
using SIL.PublishingSolution;

namespace Test.LiftPrepare.FullTests
{
    [TestFixture]
    public class FullAkooseTest
    {
        private LiftPreparer liftPreparer;

        [TestFixtureSetUp]
        public void Setup()
        {
            if (!Directory.Exists(TestEnvironment.ActualOutputRoot))
                Directory.CreateDirectory(TestEnvironment.ActualOutputRoot);
            var akoosePath = TestEnvironment.ActualOutputRoot + @"akoose";
            if (!Directory.Exists(akoosePath))
                Directory.CreateDirectory(akoosePath);
        }

        [Test]
        public void testFullAkooseTransform()
        {
            liftPreparer = new LiftPreparer(TestEnvironment.ActualOutputRoot + @"akoose/");
            liftPreparer.loadLift(TestEnvironment.InputRoot + @"akoose/akoose-small.lift");

            
        }
    }
}