// --------------------------------------------------------------------------------------
// <copyright file="Program.cs" from='2010' to='2011' company='SIL International'>
//      Copyright ( c ) 2010, 2011 SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Erik Brommers</author>
// <email>erik_brommers@sil.org</email>
// Last reviewed:
// 
// <remarks>
// epub Validator. This program calls the open source epubcheck utility to validate
// the given epub file. epubcheck is hosted on Google Code at: 
// http://code.google.com/p/epubcheck/
// </remarks>
// --------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace epubValidator
{
    public static class Program
    {
        /// <summary>
        /// Returns whether this program is running under the mono VM environment. 
        /// ONLY USE THIS IF YOU ABSOLUTELY NEED CONDITIONAL CODE. 
        /// </summary>
        public static bool UsingMonoVM
        {
            get
            {
                Type t = Type.GetType("Mono.Runtime");
                return (t != null);
            }
        }

        public static String[] args;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            args = Environment.GetCommandLineArgs();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ValidationDialog());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Path and filename for Java.exe (or null if not found</returns>
        public static string GetJavaExe()
        {
            var name = "java.exe";
            string fullName = "";
            var currentPath = Environment.GetEnvironmentVariable("path");
            if (!String.IsNullOrEmpty(currentPath))
            {
                string[] directories = currentPath.Split(new[] { ';' });
                foreach (string directory in directories)
                {
                    try
                    {
                        var myDirectory = directory.Replace("\"", "");
                        if (!Directory.Exists(myDirectory)) continue;
                        fullName = Path.Combine(myDirectory, name);
                        if (File.Exists(fullName))
                        {
                            return fullName;
                        }
                    }
                    catch (Exception e)
                    {
                        e.Source = directory + @"\" + name;
                        throw;
                    }
                }
            }
            // either no path defined (not likely) or Java.exe isn't on the path
            return name;
        }

        /// <summary>
        /// Calls epubcheck to validata the file. The results (error or normal output) are passed back in the
        /// string return value.
        /// </summary>
        /// <param name="Filename">Full path / filename of .epub file to validate</param>
        /// <returns>Results of the epubcheck run</returns>
        public static string ValidateFile(string Filename)
        {
            if (Filename == null)
            {
                return "No filename specified.";
            }
            if (File.Exists(Filename))
            {
                var sb = new StringBuilder();
                string errorMessage = "", outputMessage = "";
                try
                {
                    var progFullName = GetJavaExe();
                    if (progFullName.Length > 0)
                    {
                        if (progFullName.EndsWith(".exe"))
                        {
                            progFullName = progFullName.Substring(0, progFullName.Length - 4);
                        }
                        var strAppDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                        // jar file to execute (epubcheck)
                        sb.Append("-jar");
                        sb.Append(" \"");
                        // if we're in linux-land, add a leading slash
                        if (UsingMonoVM)
                        {
                            sb.Append(Path.DirectorySeparatorChar);
                        }
                        sb.Append(strAppDir.Substring(6)); // Remove the leading file:/ from the CodeBase result
                        sb.Append(Path.DirectorySeparatorChar);
                        sb.Append("epubcheck-4.0.1");
                        sb.Append(Path.DirectorySeparatorChar);
						sb.Append("epubcheck.jar");
						sb.Append("\" ");
	                    sb.Append(" -e "); // Include only error and fatal severity messages in output
                        // filename to run it against (the .epub file)
                        sb.Append("\"");
                        sb.Append(Filename);
                        sb.Append("\"");
                        var procArgs = sb.ToString();
                        const int timeout = 60;
                        var proc = new Process
                        {
                            StartInfo =
                            {
                                FileName = progFullName,
                                Arguments = procArgs,
                                RedirectStandardError = true,
                                RedirectStandardOutput = true,
                                WindowStyle = ProcessWindowStyle.Hidden,
                                UseShellExecute = false
                            }
                        };

                        proc.Start();

                        proc.WaitForExit
                            (
                                timeout * 100
                            );

                        errorMessage = proc.StandardError.ReadToEnd();
                        proc.WaitForExit();

                        outputMessage = proc.StandardOutput.ReadToEnd();
                        proc.WaitForExit();
                    } // java progFullName.Length > 0
                    else
                    {
                        sb.AppendLine("Unable to locate Java on this computer's search path. Java is required to run the .epub Validation tool.");
                        sb.AppendLine("You can install Java from the following website: http://www.java.com/en/download/index.jsp");
                        errorMessage = sb.ToString();
                    }
                }
                catch (System.ComponentModel.Win32Exception w32e)
                {
                    return "No errors or warnings detected";
                }
                catch (Exception e)
                {
                    sb.AppendLine("The .epub Validator encountered an error while attempting to validate the file.");
                    sb.AppendLine();
                    sb.AppendLine("Exception Message:");
                    sb.AppendLine(e.Message);
                    if (e.InnerException != null)
                    {
                        sb.AppendLine("Inner Exception:");
                        sb.AppendLine(e.InnerException.ToString());
                    }
                    errorMessage = sb.ToString();
                }
                if (errorMessage.Length > 0) return errorMessage;
                if (outputMessage.Length > 0) return outputMessage;
                return null;
            }

            //filename isn't a valid file
            return ("Invalid filename:" + Filename);
        }
    }
}
