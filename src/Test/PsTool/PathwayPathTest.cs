// --------------------------------------------------------------------------------------------
// <copyright file="PathwayPathTest.cs" from='2009' to='2014' company='SIL International'>
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
// Test methods of FlexDePlugin
// </remarks>
// --------------------------------------------------------------------------------------------

using System.IO;
using SIL.Tool;
using NUnit.Framework;
using System.Windows.Forms;

namespace Test.PsTool
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Pathway Path changes from FieldWorks 6.0.4 to FieldWorks 7 but both should point to
	/// the folder with the program files.
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public class PathwayPathTest
    {
        ///<summary>
        ///Values of PathwayDir:
        ///AppDir = Fw6.0.4 WinXP Configuration tool: C:\Program Files\SIL\Pathway
        ///AppDir = Fw6.0.4 WinXP FieldWorks (Flex / TE): C:\Program Files\SIL\FieldWorks
        ///AppDir = Fw6.0.4 Win7 Configuration tool: C:\Program Files (x86)\SIL\Pathway
        ///AppDir = Fw6.0.4 Win7 FieldWorks (Flex / TE): C:\Program Files (x86)\SIL\FieldWorks
        ///??       Fw6.0.4 developer (resharper/NUnit) Configuration tool: $(btaiRoot)PublishingSolution\ConfigurationTool\bin\Debug
        ///??       Fw6.0.4 developer (resharper/NUnit) FieldWorks (Flex / TE): $(btaiRoot)PublishingSolution\PsExport\bin\Debug
        ///RegKey = Fw7 / Paratext 7.1 WinXP Configuration tool: C:\Program Files\SIL\Pathway7
        ///RegKey = Fw7 / Paratext 7.1 WinXP FieldWorks (Flex / TE): C:\Program Files\SIL\Pathway7
        ///RegKey = Fw7 / Paratext 7.1 Win7 Configuration tool: C:\Program Files (x86)\SIL\Pathway7
        ///RegKey = Fw7 / Paratext 7.1 Win7 FieldWorks (Flex / TE): C:\Program Files (x86)\SIL\Pathway7
        ///??       Fw7 / Paratext 7.1 developer (resharper/NUnit) Configuration tool: $(btaiRoot)PublishingSolution\ConfigurationTool\bin\Debug
        ///??       Fw7 / Paratext 7.1 developer (resharper/NUnit) FieldWorks (Flex / TE): $(btaiRoot)PublishingSolution\PsExport\bin\Debug
        ///</summary>
        [Test]
        [Category("ShortTest")]
        public void GetPathwayDirTest()
        {
			string actual = Common.AssemblyPath;
            if (Common.IsUnixOS())
            {
				Assert.IsTrue(actual.Substring(1) == @"usr/lib/pathway" || actual.Contains("Export") || actual.Contains("ReSharper") || actual.Contains("NUnit"));
            }
            else
            {
                Assert.IsTrue(actual.Replace(" (x86)", "").Substring(1) == @":\Program Files\SIL\Pathway\" || actual.Contains("Export") || actual.Contains("NUnit"));
            }
        }
    }
}
