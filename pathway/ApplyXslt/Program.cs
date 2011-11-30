// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2011, SIL International. All Rights Reserved.
// <copyright from='2011' to='2011' company='SIL International'>
//		Copyright (c) 2011, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: ApplyXslt.cs
// Responsibility: Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using SIL.Tool;

namespace ApplyXslt
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int i = 0;
                string inName = null, outName = null;
                var xsltName = new List<string>();
                bool keep = false;
                bool status = false;
                if (args.Length == 0)
                {
                    Usage();
                    Environment.Exit(0);
                }
                while (i < args.Length)
                {
                    switch (args[i++])
                    {
                        case "-IN":
                        case "--input":
                        case "-i":
                            inName = args[i++];
                            break;
                        case "-XSL":
                        case "--xslt":
                        case "-x":
                            xsltName.Add(args[i++]);
                            break;
                        case "-OUT":
                        case "--output":
                        case "-o":
                            outName = args[i++];
                            break;
                        case "--keep":
                        case "-k":
                            keep = !keep;
                            break;
                        case "--status":
                        case "-s":
                            status = !status;
                            break;
                        default:
                            Usage();
                            throw new ArgumentException("Invalid command line argument: " + args[i - 1]);
                    }
                }
                if (inName == null || xsltName.Count == 0)
                {
                    Usage();
                    throw new ArgumentException("Missing required argument.");
                }
                string result = null;
                if (keep)
                    File.Copy(inName, Path.GetFileNameWithoutExtension(inName) + "-0.xhtml", true);
                for (int n = 1; n <= xsltName.Count; n ++)
                {
                    var defaultExtension = string.Format("-{0}.xhtml", n);
                    result = Common.XsltProcess(inName, xsltName[n-1], defaultExtension);
                    if (status)
                        Console.WriteLine(string.Format("Result {0} is {1}", n, result));
                    if (!keep && n > 1)
                        File.Delete(inName);
                    inName = result;
                }
                if (result != null && outName != null && outName != result)
                {
                    File.Copy(result, outName, true);
                    if (!keep)
                        File.Delete(result);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(Exception err)
            {
                Console.WriteLine("ApplyXslt encountered an error while processing: " + err.Message);
                if (err.StackTrace != null)
                {
                    Console.WriteLine(err.StackTrace);
                }
                Environment.Exit(-1);
            }
            Environment.Exit(0);
        }

        private static string usageMessage = @"
ApplyXslt -i input.xhtml -x transform.xslt [-o output.xhtml] [-k] [-s]
-IN, --input, -i   Name of input file (required)
-XSL, --xslt, -x   Name of XSLT transform (required)
-OUT, --output, -o Name of output file (optional)
--keep, -k         keep intermediate files
--status, -s       report progress
";
        static void Usage()
        {
            Console.WriteLine(usageMessage);
        }
    }
}
