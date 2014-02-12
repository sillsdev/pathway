// --------------------------------------------------------------------------------------------
// <copyright file="LiftEntryTester.cs" from='2009' to='2014' company='SIL International'>
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

using System;
using System.Collections.Generic;

using System.Text;
using NMock2;
using NUnit.Framework;
using SIL.PublishingSolution;

namespace Test.LiftPrepare
{
    [TestFixture]
    public class LiftEntryTester
    {
        private LiftEntries entries;
        private LiftDocument document;
        private string[] expectedKeys;
        

        [SetUp]
        public void setup()
        {
            var liftReader = new LiftReader(TestEnvironment.InputRoot + @"simple/liftEntries.lift");
            document = new LiftDocument();
            document.Load(liftReader);
            entries = document.getEntries();
        }

        [Test]
        public void testGetKey()
        {
            prepareExpectedKeys();
            assertGeneratedKeysMatchExpectedKeys();
        }

        private void prepareExpectedKeys()
        {
            expectedKeys = new string[10];
            expectedKeys[0] = @"abat-3";
            expectedKeys[1] = @"srapa-prefix";
            expectedKeys[2] = @"brush";
            expectedKeys[3] = @"hairbrush";
            expectedKeys[4] = @"paintbrush";
            expectedKeys[5] = @"utan-inclefixor-1";
            expectedKeys[6] = @"hete";
            expectedKeys[7] = @"apu";
            expectedKeys[8] = @"opon";
            expectedKeys[9] = @"jɔ̃de";
        }

        private void assertGeneratedKeysMatchExpectedKeys()
        {
            for (int i = 0; i < entries.Count(); i++)
            {
                StringAssert.AreEqualIgnoringCase(expectedKeys[i], entries[i].getKey());
            }
        }
    }
}