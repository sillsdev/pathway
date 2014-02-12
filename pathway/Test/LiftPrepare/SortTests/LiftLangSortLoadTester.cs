// --------------------------------------------------------------------------------------------
// <copyright file="LiftLangSortLoadTester.cs" from='2009' to='2014' company='SIL International'>
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