// --------------------------------------------------------------------------------------------
// <copyright file="LiftDocumentTester.cs" from='2009' to='2014' company='SIL International'>
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
        [Category("SkipOnTeamCity")]
        public void testGetLiftNode()
        {
            var liftReader = new LiftReader(TestEnvironment.InputRoot + @"simple/validLift.lift");
            liftDocument.Load(liftReader);
            var liftRootNode = liftDocument.getLiftNode();
            StringAssert.AreEqualIgnoringCase("lift",liftRootNode.Name);
        }

        [Test]
        public void testGetEntries()
        {
            var liftReader = new LiftReader(TestEnvironment.InputRoot + @"simple/liftEntries.lift");
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