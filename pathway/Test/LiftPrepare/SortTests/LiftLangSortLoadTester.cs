using NUnit.Framework;
using NMock2;
using SIL.PublishingSolution.Interfaces;
using SIL.PublishingSolution.Sort;


namespace Test.LiftPrepare.SortTests
{
    public class LiftLangSortLoadTester : LiftSortTester
    {
        private LiftLangSorter langSorter;

        [Test]
        public void testLoadSingleLang()
        {
            var mockLangSorterView = mocks.NewMock<ILangSorterView>();
            var additionalSortRules = "";

            Expect.Once.On(mockLangSorterView).
                Method("getLangSortingRules").
                WithNoArguments().
                Will(Return.CloneOf(additionalSortRules));
        }
    }
}