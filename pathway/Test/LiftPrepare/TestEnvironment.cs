using System;
using System.Collections.Generic;

using System.Text;

namespace Test.LiftPrepare
{
    public static class TestEnvironment
    {
        public static string TestingRoot = @"..\..\LiftPrepare\TestFiles\";
        public static string InputRoot = TestingRoot + @"Input\";
        public static string ActualOutputRoot = TestingRoot + @"Output\";
        public static string ExpectedOutputRoot = TestingRoot + @"Expected\";

        public static string FilterInnerXML = @"<value-of select=""'true'"" />";
    }
}