using NUnit.Framework;
using SIL.PublishingSolution;

namespace Test.LiftPrepare
{
    [TestFixture]
    public class LiftReaderTester
    {
        private LiftReader liftReader;

        [Test]
        public void testValidLift()
        {
            liftReader = new LiftReader(TestEnvironment.InputRoot + @"simple/validLift.lift"); 
        }

        [Test]
        public void testInvalidLift()
        {
            Assert.Throws<Environ.InvalidLiftException>(
                delegate
                    {
                        new LiftReader(TestEnvironment.InputRoot + @"simple/invalidLift.lift");
                    }
                );
        }
    }
}