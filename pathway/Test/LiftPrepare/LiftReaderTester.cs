// --------------------------------------------------------------------------------------------
// <copyright file="LiftReaderTester.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
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