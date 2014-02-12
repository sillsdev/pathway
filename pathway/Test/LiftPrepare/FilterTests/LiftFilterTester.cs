// --------------------------------------------------------------------------------------------
// <copyright file="LiftFilterTester.cs" from='2009' to='2014' company='SIL International'>
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
using NUnit.Framework;
using NMock2;
using System.Xml;

using SIL.PublishingSolution.Filter;

namespace Test.LiftPrepare.FilterTests
{
    [TestFixture]
    public abstract class LiftFilterTester
    {
        protected Mockery mocks;
        protected LiftFilter filter;
        protected XmlNodeList filterNodes;

        [SetUp]
        public void setup()
        {
            mocks = new Mockery();
            filter = new LiftFilter();
        }

        protected void assertContainsFilter(string xpath, XmlNodeList filters)
        {
            foreach (XmlNode filter in filters)
            {
                if (filter.Attributes["test"].Value == xpath && filter.InnerXml == TestEnvironment.FilterInnerXML)
                {
                    return;
                }
            }
            Assert.Fail("Filter " + xpath + " not found.");
        }
    }
}