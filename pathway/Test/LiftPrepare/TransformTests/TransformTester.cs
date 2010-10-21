using System.IO;
using System.Xml;
using NUnit.Framework;
using SIL.PublishingSolution;
using SIL.PublishingSolution.Transform;

namespace Test.LiftPrepare.TransformTests
{
    [TestFixture]
    public class TransformTester
    {
        LiftTransformer liftTransformer = new LiftTransformer();

        [TestFixtureSetUp]
        public void Setup()
        {
            if (!Directory.Exists(TestEnvironment.ActualOutputRoot))
                Directory.CreateDirectory(TestEnvironment.ActualOutputRoot);
        }

        [Test]
        public void testTransform()
        {
            var documentToTransform = new LiftDocument(TestEnvironment.InputRoot + @"buang/buang-small.lift");
            var transformedDocument = liftTransformer.applyTo(documentToTransform);
            var writer = new XmlTextWriter(TestEnvironment.ActualOutputRoot + @"buang-small.xhtml",null);
            transformedDocument.Save(writer);
        }
    }
}