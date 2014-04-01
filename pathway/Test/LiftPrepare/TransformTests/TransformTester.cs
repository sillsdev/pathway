// --------------------------------------------------------------------------------------------
// <copyright file="TransformTester.cs" from='2009' to='2014' company='SIL International'>
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
        [Category("ShortTest")]
        [Category("SkipOnTeamCity")]
        public void testTransform()
        {
            var documentToTransform = new LiftDocument(TestEnvironment.InputRoot + @"buang/buang-small.lift");
            var transformedDocument = liftTransformer.applyTo(documentToTransform);
            var writer = new XmlTextWriter(TestEnvironment.ActualOutputRoot + @"buang-small.xhtml",null);
            transformedDocument.Save(writer);
        }
    }
}