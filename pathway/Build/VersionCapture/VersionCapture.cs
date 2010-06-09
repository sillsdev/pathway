using System;
using SIL.Tool;

namespace VersionCapture
{
    class VersionCapture
    {
        static void Main(string[] args)
        {
            var arg = args.Length > 0 ? args[0] : "";
            var fieldworksAssemblies = Common.DirectoryPathReplace(Environment.CurrentDirectory + @"/../Dlls" + arg);
            Common.ProgInstall = Common.DirectoryPathReplace(Environment.CurrentDirectory + @"/../../");
            Common.SupportFolder = "PsSupport";
            Common.SaveFieldworksVersions(fieldworksAssemblies);
        }
    }
}
