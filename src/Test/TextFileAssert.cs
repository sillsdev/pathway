// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: TextFileAssert.cs
// Responsibility: Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.IO;
using NUnit.Framework;

namespace Test
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// compare text files while ignoring line endings
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public class TextFileAssert
    {
        public static void AreEqualEx(string expectPath, string outputPath, ArrayList ex, string msg)
        {
            try
           {
                Int32 line = 0;
                StreamReader expectStream = new StreamReader(expectPath);
                StreamReader outputStream = new StreamReader(outputPath);
                while (!expectStream.EndOfStream)
                {
                    line += 1;
                    var expectLine = expectStream.ReadLine();
                    var outputLine = outputStream.ReadLine();
                    if (ex != null && ex.Contains(line)) continue;
                    if (expectLine != outputLine)
						Assert.Fail(msg + ":" + expectLine);
                }
                if (!outputStream.EndOfStream)
                    Assert.Fail(msg);
                expectStream.Close();
                outputStream.Close();
            }
            catch (Exception)
            {
                Assert.Fail(msg);
            }
        }

        private static void CheckLineAreEqualEx(string expectPath, string outputPath, ArrayList ex, string msg)
        {
            if (!File.Exists(expectPath) || !File.Exists(outputPath))
            {
                Assert.Fail(expectPath + " File missing");
            }
            try
            {
                Int32 line = 0;
                StreamReader expectStream = new StreamReader(expectPath);
                StreamReader outputStream = new StreamReader(outputPath);
                while (!expectStream.EndOfStream)
                {
                    line += 1;
                    var expectLine = expectStream.ReadLine();
                    var outputLine = outputStream.ReadLine();
                    if (ex != null && ex.Contains(line))
                    {
                        if (expectLine != outputLine)
                        {
                            Assert.Fail(msg);
                        }
                    }
                }
                if (!outputStream.EndOfStream)
                    Assert.Fail(msg);
                expectStream.Close();
                outputStream.Close();
            }
            catch (Exception)
            {
                Assert.Fail(msg);
            }
        }

        public static void CheckLineAreEqualEx(string expectPath, string outputPath, ArrayList ex)
        {
            CheckLineAreEqualEx(expectPath, outputPath, ex, null);
        }
        public static void AreEqualEx(string expectPath, string outputPath, ArrayList ex)
        {
            AreEqualEx(expectPath, outputPath, ex, null);
        }
        public static void AreEqual(string expectPath, string outputPath, string msg)
        {
            AreEqualEx(expectPath, outputPath, null, msg);
        }
        public static void AreEqual(string expectPath, string outputPath)
        {
            AreEqualEx(expectPath, outputPath, null, null);
        }
    }
}
