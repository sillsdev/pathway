
using NMock2;
using NUnit.Framework;
using SIL.PublishingSolution.Interfaces;
using SIL.PublishingSolution.Sort;

namespace Test.LiftPrepare.SortTests
{
    public class LiftEntrySortLoadTester : LiftSortTester
    {
        LiftEntrySorter sorter = new LiftEntrySorter();

        [Test]
        public void testLoadingSingleRule()
        {
            var rule = new string[1]{@"&B < b < A < a"};
            var mockEntrySortView = mocks.NewMock<IEntrySortView>();
            Expect.Once.On(mockEntrySortView).
                Method("getEntrySortingRules").
                WithNoArguments().
                Will(Return.CloneOf(rule));
            sorter.addRules(mockEntrySortView.getEntrySortingRules());
            StringAssert.Contains(rule[0], sorter.getRules());
            mocks.VerifyAllExpectationsHaveBeenMet();
        }
    }
}