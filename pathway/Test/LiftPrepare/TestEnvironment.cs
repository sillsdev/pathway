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