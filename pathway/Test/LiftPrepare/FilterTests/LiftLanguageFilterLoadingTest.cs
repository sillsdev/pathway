// --------------------------------------------------------------------------------------------
// <copyright file="LiftLanguageFilterLoadingTest.cs" from='2009' to='2014' company='SIL International'>
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

using NUnit.Framework;
using NMock2;
using SIL.PublishingSolution.Filter;
using SIL.PublishingSolution.Interfaces;


namespace Test.LiftPrepare.FilterTests
{
    [TestFixture]
    public class LiftLanguageFilterLoadingTest : LiftFilterTester
    {
        protected ILangFilterView mockLanguageFilterView;
        protected const string languageFilterXpath1 = @"contians($lang, 'PIN')";
        protected const string languageFilterXpath2 = @"$lang = 'ii'";

        [Test]
        public void loadSingleLanguageFilter()
        {
            prepareMockLangFilterView(languageFilterXpath1);
            loadLangFiltersThroughMockView();
            assertLangFilterLoaded(languageFilterXpath1);
        }

        [Test]
        public void loadMultipleLanguageFilters()
        {
            prepareMockLangFilterView(languageFilterXpath1, languageFilterXpath2);
            loadLangFiltersThroughMockView();
            assertLangFilterLoaded(languageFilterXpath1);
            assertLangFilterLoaded(languageFilterXpath2);
        }

        private void loadLangFiltersThroughMockView()
        {
            filter.loadLanguageFilterTemplate();
            filter.insertLanguageFilters(mockLanguageFilterView.getLanguageFilters());
        }

        private void prepareMockLangFilterView(params string[] filterXPaths)
        {
            var languageFilters = new LiftLangFilters();
            foreach (var filterXPath in filterXPaths)
            {
                languageFilters.add(filterXPath,true,"lang");
            }
            mockLanguageFilterView = mocks.NewMock<ILangFilterView>();
            Expect.Once.On(mockLanguageFilterView).
                Method("getLanguageFilters").
                WithNoArguments().
                Will(Return.CloneOf(languageFilters));
        }

        protected void assertLangFilterLoaded(string xpath)
        {
            var filterNodes = filter.SelectNodes(@"//*[name()='template' and @name='filterLang']/descendant::*[name()='when']");
            assertContainsFilter(xpath, filterNodes);
        }
    }
}