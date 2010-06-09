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