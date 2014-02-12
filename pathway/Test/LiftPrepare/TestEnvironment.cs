// --------------------------------------------------------------------------------------------
// <copyright file="TestEnvironment.cs" from='2009' to='2014' company='SIL International'>
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
using SIL.Tool;

namespace Test.LiftPrepare
{
    public static class TestEnvironment
    {
        public static string TestingRoot = PathPart.Bin(Environment.CurrentDirectory, "/LiftPrepare/TestFiles/");
        public static string InputRoot = Common.PathCombine(TestingRoot, "Input/");
        public static string ActualOutputRoot = Common.PathCombine(TestingRoot, "Output/");
        public static string ExpectedOutputRoot = Common.PathCombine(TestingRoot, "Expected/");

        public static string FilterInnerXML = @"<value-of select=""'true'"" />";
    }
}