// --------------------------------------------------------------------------------------------
// <copyright file="FullAkooseTest.cs" from='2009' to='2014' company='SIL International'>
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