// --------------------------------------------------------------------------------------------
// <copyright file="LiftWriterTester.cs" from='2009' to='2014' company='SIL International'>
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