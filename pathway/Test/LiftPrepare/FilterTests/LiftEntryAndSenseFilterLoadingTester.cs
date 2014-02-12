// --------------------------------------------------------------------------------------------
// <copyright file="LiftEntryAndSenseFilterLoadingTester.cs" from='2009' to='2014' company='SIL International'>
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
using NMock2;
using SIL.PublishingSolution.Filter;
using SIL.PublishingSolution.Interfaces;

namespace Test.LiftPrepare.FilterTests
{
    [TestFixture]
    public class LiftEntryAndSenseFilterLoadingTester : LiftFilterTester
    {
        private IEntryAndSenseFilterView mockEntryAndSenseFilterView;
        private const string entryFilterXpath = @"starts-with({lexical-unit/form/text}, 'b')";
        private const string senseFilterXpath = @"contains('grammatical-info/@value', 'oun')";

        [Test]
        public void loadSingleEntryFilter()
        {
            var entryAndSenseFilters = setupEntryAndSenseFilters("entry");
            loadEntryAndSenseFiltersThroughMockView(entryAndSenseFilters);
            assertEntryFilterLoaded(entryFilterXpath);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void loadSingleSenseFilter()
        {
            var entryAndSenseFilters = setupEntryAndSenseFilters("sense");
            loadEntryAndSenseFiltersThroughMockView(entryAndSenseFilters);
            assertSenseFilterLoaded(senseFilterXpath);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void loadingEntryAndSenseFilter()
        {
            var entryAndSenseFilters = setupEntryAndSenseFilters("entriesandsenses");
            loadEntryAndSenseFiltersThroughMockView(entryAndSenseFilters);
            assertEntryFilterLoaded(entryFilterXpath);
            assertSenseFilterLoaded(senseFilterXpath);
            mocks.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void loadingInvalidEntryAndSenseFilter()
        {
            Assert.Throws<LiftFilterChooseStatementException>(
                delegate { setupEntryAndSenseFilters(""); } );
            //var entryAndSenseFilters = setupEntryAndSenseFilters("");
            //loadEntryAndSenseFiltersThroughMockView(entryAndSenseFilters);
            //mocks.VerifyAllExpectationsHaveBeenMet();
        }

        private void loadEntryAndSenseFiltersThroughMockView(LiftEntryAndSenseFilters filters)
        {
            mockEntryAndSenseFilterView = mocks.NewMock<IEntryAndSenseFilterView>();
            Expect.Once.On(mockEntryAndSenseFilterView).
                Method("getLiftEntryAndSenseFilters").
                WithNoArguments().
                Will(Return.CloneOf(filters));
            filter.loadEntryAndSenseTemplate();
            filter.insertEntryAndSenseFilters(mockEntryAndSenseFilterView.getLiftEntryAndSenseFilters());
        }

        private LiftEntryAndSenseFilters setupEntryAndSenseFilters(string type)
        {
            var entryAndSenseFilters = new LiftEntryAndSenseFilters();
            switch (type)
            {
                case "entry":
                    {
                        entryAndSenseFilters.add(setupEntryAndSenseFilter(entryFilterXpath, true, "entry"));
                        break;
                    }
                case "sense":
                    {
                        entryAndSenseFilters.add(setupEntryAndSenseFilter(senseFilterXpath, true, "sense"));
                        break;
                    }
                case "entriesandsenses":
                    {
                        entryAndSenseFilters.add(setupEntryAndSenseFilter(senseFilterXpath, true, "sense"));
                        entryAndSenseFilters.add(setupEntryAndSenseFilter(entryFilterXpath, true, "entry"));
                        break;
                    }
                default:
                    {
                        entryAndSenseFilters.add(setupEntryAndSenseFilter(@"true()", true, "monkey"));
                        break;
                    }
            }
            return entryAndSenseFilters;
        }

        private LiftFilterChooseStatement setupEntryAndSenseFilter(string xpath, bool filtered, string applyTo)
        {
            return new LiftFilterChooseStatement(xpath,filtered,applyTo);
        }

        private void assertEntryFilterLoaded(string xpath)
        {
            var filterNodes = filter.SelectNodes(@"//*[name()='template' and @name='filterEntry']/descendant::*[name()='when']");
            assertContainsFilter(xpath, filterNodes);
        }

        private void assertSenseFilterLoaded(string xpath)
        {
            var filterNodes = filter.SelectNodes(@"//*[name()='template' and @name='filterSense']/descendant::*[name()='when']");
            assertContainsFilter(xpath, filterNodes);
        }
    }
}