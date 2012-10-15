// --------------------------------------------------------------------------------------------
// <copyright file="SubProcess.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Works with SubProcesses.
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Resources;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SIL.Tool
{
    /// <summary>
    /// Works with SubProcesses.
    /// </summary>
    public static class SubProcess
    {
        public static int ExitCode;
        public static string RedirectOutput;
        public static string LastError;

        #region RunProcess

        /// <summary>
        /// Override. Runs the process from the instPath folder. Waits until complete (with a timeout),
        /// then returns the output and error results as strings.
        /// </summary>
        /// <param name="instPath">Execution path</param>
        /// <param name="name">Name of file to execute</param>
        /// <param name="arg">Arguments (optional)</param>
        /// <param name="stdOut">Standard output results</param>
        /// <param name="stdErr">Standard error results</param>
        public static void Run(string instPath, string name, string arg, out string stdOut, out string stdErr)
        {
            const int timeout = 60;
            string theCurrent = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(instPath);
            Process p1 = new Process();
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = name,
                    Arguments = arg,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false
                }
            };
            try
            {
                // attempt to run the process
                proc.Start();
                proc.WaitForExit
                    (
                        timeout * 100 * 60
                    );
                // copy the results
                stdErr = proc.StandardError.ReadToEnd();
                proc.WaitForExit();
                stdOut = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
            }
            catch (Exception e)
            {
                // exception thrown during the program's execution --
                // copy the exception into stdErr
                var sb = new StringBuilder();
                sb.AppendLine(e.Message);
                sb.AppendLine(e.StackTrace);
                stdErr = sb.ToString();
                stdOut = string.Empty;
            } 
            // restore the current directory and return
            Directory.SetCurrentDirectory(theCurrent);
        }

        /// <summary>
        /// Runs process name from the instPath folder. Waits until complete before returning.
        /// </summary>
        public static void Run(string instPath, string name)
        {
            Run(instPath, name, true);
        }

        public static void Run(string instPath, string name, bool wait)
        {
            Run(instPath, name, null, wait);
        }

        public static void Run(string instPath, string name, string arg, bool wait)
        {
            // clean out the results of any previous runs
            LastError = String.Empty;
            ExitCode = 0;
            var info = new ProcessStartInfo(name)
                           {
                               CreateNoWindow = true,
                               RedirectStandardOutput = !string.IsNullOrEmpty(RedirectOutput),
                               RedirectStandardError = !string.IsNullOrEmpty(RedirectOutput),
                               UseShellExecute = string.IsNullOrEmpty(RedirectOutput),
                               WorkingDirectory = instPath
                           };
            if (arg != null)
                info.Arguments = arg;
            using (Process p1 = Process.Start(info))
            {
                if (wait)
                {
                    if (p1.Id <= 0)
                        throw new MissingSatelliteAssemblyException(name);
                    p1.WaitForExit();
                    ExitCode = p1.ExitCode;
                    if (!string.IsNullOrEmpty(RedirectOutput))
                    {
                        string result = p1.StandardOutput.ReadToEnd();
                        LastError = p1.StandardError.ReadToEnd();
                        result += LastError;
                        StreamWriter streamWriter = new StreamWriter(RedirectOutput);
                        streamWriter.Write(result);
                        streamWriter.Close();
                        RedirectOutput = null;
                    }
                }
                p1.Close();
            }
        }
        #endregion RunProcess

        public static string Location = "";

        #region ExistsOnPath(string name)
        /// <summary>
        /// Checks if program name esists on system path
        /// </summary>
        /// <param name="name">name of program or file to search for</param>
        /// <returns>true if found</returns>
        public static bool ExistsOnPath(string name)
        {
            Location = "";
            string [] directories;
			string currentPath;
            if (Common.UsingMonoVM)
			{
				currentPath = Environment.GetEnvironmentVariable("PATH");
				if (String.IsNullOrEmpty(currentPath)) return false;
				directories = currentPath.Split(new [] { ':' });
			}
			else
			{
				currentPath = Environment.GetEnvironmentVariable("path");
				if (String.IsNullOrEmpty(currentPath)) return false;
				directories = currentPath.Split(new [] { ';' });
			}
            foreach (string directory in directories)
                try
                {
                    var myDirectory = directory.Replace("\"", "");
                    if (!Directory.Exists(myDirectory)) continue;
                    if (File.Exists(Path.Combine(myDirectory, name)))
                    {
                        Location = myDirectory;
                        return true;
                    }
                }
                catch (Exception e)
                {
                    e.Source = directory + @"\" + name;
                    throw;
                }
                return false;
        }
        #endregion ExistsOnPath(string name)

        #region GetLocation(string name)
        public static string GetLocation(string name)
        {
            if (Location != "")
                return Location;
            if (ExistsOnPath(name))
                return Location;
            return "";
        }
        #endregion GetLocation(string name)

        /// <summary>
        /// do anything that needs to be done after export but before convert
        /// </summary>
        public static void BeforeProcess(string outFullName)
        {
            const string BeforeProcess = "BeforePwConvert.bat";
            string processFolder = GetProcessFolder(outFullName);
            if (File.Exists(Path.Combine(processFolder, BeforeProcess)))
                Run(processFolder, BeforeProcess, '"' + outFullName + '"', true);
        }

        /// <summary>
        /// return folder name that should contain process if it exists
        /// </summary>
        private static string GetProcessFolder(string outFullName)
        {
            var folder = Path.GetDirectoryName(outFullName);
            var parent = Path.GetDirectoryName(folder);
            return Path.Combine(parent, "Process");
        }

        /// <summary>
        /// do anything that needs to be done after export but before convert
        /// </summary>
        public static void AfterProcess(string outFullName)
        {
            const string AfterProcess = "AfterPwConvert.bat";
            try
            {
                string processFolder = GetProcessFolder(outFullName);
                if (File.Exists(Path.Combine(processFolder, AfterProcess)))
                    Run(processFolder, AfterProcess, '"' + outFullName + '"', true);
            }
            catch (Exception)
            {
            }
        }
    }
}