// --------------------------------------------------------------------------------------------
// <copyright file="LiftEntrySortLoadTester.cs" from='2009' to='2014' company='SIL International'>
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