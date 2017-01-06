#region // Copyright (C) 2014, SIL International. All Rights Reserved.
// <copyright file="MySwordSqliteTest.cs" from='2014' to='2014' company='SIL International'>
//		Copyright (C) 2014, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------

using NUnit.Framework;
using SIL.PublishingSolution;

namespace Test.theWordConvert
{
    [TestFixture]
    public class MySwordSqliteTest: MySwordSqlite
    {
        [Test]
        public void ParseDateIfNecessaryTest()
        {
            const string key = "version.date";
            const string inValue = "2014.7.14";
            DbParams[key] = inValue;
            var actual = ParseDateIfNecessary(key, inValue);
            Assert.AreEqual("date('2014-07-14')", actual);
        }

        [Test]
        public void ParseDateIfNecessaryUserTest()
        {
            const string key = "version.date";
            const string inValue = "2014-07-14";
            DbParams[key] = inValue;
            var actual = ParseDateIfNecessary(key, inValue);
            Assert.AreEqual("date('2014-07-14')", actual);
        }

        [Test]
        public void ParseDateIfNecessaryYearTest()
        {
            const string key = "publish.date";
            const string inValue = "2009";
            DbParams[key] = inValue;
            var actual = ParseDateIfNecessary(key, inValue);
            Assert.AreEqual("date('2009-01-01')", actual);
        }

    }
}
