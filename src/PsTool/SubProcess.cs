// --------------------------------------------------------------------------------------------
// <copyright file="SubProcess.cs" from='2009' to='2014' company='SIL International'>
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
// Works with SubProcesses.
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

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
        private static Process myProcess = new Process();
        private static int elapsedTime;
        private static bool eventHandled;

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
        /// Override. Runs the process from the instPath folder. Waits until complete (with a timeout),
        /// then returns the output and error results as strings.
        /// </summary>
        /// <param name="instPath">Execution path</param>
        /// <param name="name">Name of file to execute</param>
        /// <param name="arg">Arguments (optional)</param>
        /// <param name="stdOut">Standard output results</param>
        /// <param name="stdErr">Standard error results</param>
        public static void RunWithoutWait(string instPath, string name, string arg, out string stdOut, out string stdErr)
        {
            string theCurrent = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(instPath);
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
                // copy the results
                stdErr = proc.StandardError.ReadToEnd();
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

        private static void Run(string instPath, string name, bool wait)
        {
            Run(instPath, name, null, wait);
        }

		public static ProcessWindowStyle WindowStyle = ProcessWindowStyle.Normal;
        public static void Run(string instPath, string name, string arg, bool wait)
        {
            // clean out the results of any previous runs
            LastError = String.Empty;
            ExitCode = 0;
		    if (string.IsNullOrEmpty(name)
		        || Path.IsPathRooted(name) && !File.Exists(name)
		        || !Path.IsPathRooted(name) && !File.Exists(Path.Combine(instPath, name)))
		    {
			    if (Common.Testing) return;
			    if (!ExistsOnPath(name)) throw new InvalidOperationException();
			}
			var info = new ProcessStartInfo(name)
                           {
                               CreateNoWindow = true,
                               RedirectStandardOutput = !string.IsNullOrEmpty(RedirectOutput),
                               RedirectStandardError = !string.IsNullOrEmpty(RedirectOutput),
                               UseShellExecute = string.IsNullOrEmpty(RedirectOutput),
							   WindowStyle = WindowStyle,
							   WorkingDirectory = instPath
                           };
            if (arg != null)
                info.Arguments = arg;
            Debug.Print("Run: Filename: {0}", info.FileName);
            //if (Common.Testing) return;
            using (Process p1 = Process.Start(info))
            {
                if (wait)
                {
					if (p1 == null)
	                {
						MessageBox.Show(string.Format("{0} process not available. Kindly install the required application", name));
		                return;
	                }

	                p1.WaitForExit();
                    ExitCode = p1.ExitCode;
                    if (!string.IsNullOrEmpty(RedirectOutput))
                    {
                        string result = p1.StandardOutput.ReadToEnd();
                        LastError = p1.StandardError.ReadToEnd();
                        result += LastError;
                        StreamWriter streamWriter = new StreamWriter(Common.PathCombine(instPath, RedirectOutput));
                        streamWriter.Write(result);
                        streamWriter.Close();
                        RedirectOutput = null;
                    }
                }
                p1.Close();
            }
        }

        public static void RunCommand(string instPath, string name, string arg, bool wait)
        {
            elapsedTime = 0;
            eventHandled = false;

            try
            {
                // Start a process to print a file and raise an event when done.
                myProcess.StartInfo.FileName = name;
                myProcess.StartInfo.Arguments = arg;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.EnableRaisingEvents = true;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.UseShellExecute = string.IsNullOrEmpty(RedirectOutput);
                myProcess.StartInfo.WorkingDirectory = instPath;

                myProcess.Exited += new EventHandler(myProcess_Exited);
                myProcess.Start();

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(RedirectOutput))
                {
                    string result = string.Empty;
                    StreamWriter streamWriter = new StreamWriter(Common.PathCombine(instPath, RedirectOutput));
                    var errorArgs = string.Format("An error occurred trying to print \"{0}\":" + "\n" + ex.Message, arg);
                    result += errorArgs;
                    streamWriter.Write(result);
                    streamWriter.Close();
                    RedirectOutput = null;
                }
                return;
            }

            // Wait for Exited event, but not more than 30 seconds.
            const int SLEEP_AMOUNT = 100;
            while (!eventHandled)
            {
                elapsedTime += SLEEP_AMOUNT;
                if (elapsedTime > 30000)
                {
                    break;
                }
                Thread.Sleep(SLEEP_AMOUNT);
            }
            myProcess.Close();
        }

        // Handle Exited event and display process information.
        private static void myProcess_Exited(object sender, System.EventArgs e)
        {

            eventHandled = true;
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
            string[] directories;
            string currentPath;
            if (Common.UsingMonoVM)
            {
                currentPath = Environment.GetEnvironmentVariable("PATH");
                if (String.IsNullOrEmpty(currentPath)) return false;
                directories = currentPath.Split(new[] { ':' });
            }
            else
            {
                currentPath = Environment.GetEnvironmentVariable("path");
                if (String.IsNullOrEmpty(currentPath)) return false;
                directories = currentPath.Split(new[] { ';' });
            }
            foreach (string directory in directories)
                try
                {
                    var myDirectory = directory.Replace("\"", "");
                    var dirInfo = new DirectoryInfo(myDirectory);
                    if (!dirInfo.Exists) continue;
                    var fileInfo = new FileInfo(Common.PathCombine(myDirectory, name));
                    if (fileInfo.IsReadOnly)
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

        #region JavaFullName(name)
        /// <summary>
        /// Identify Java program location
        /// </summary>
        public static string JavaFullName(string name)
        {
            string progFolder = GetLocation(name);
            if (string.IsNullOrEmpty(progFolder) || !File.Exists(Common.PathCombine(progFolder, "java.exe")))
            {
                var r = RegistryHelperLite.JarFile;
                Debug.Assert(r != null);
                r = r.OpenSubKey("shell");
                Debug.Assert(r != null);
                r = r.OpenSubKey("open");
                Debug.Assert(r != null);
                r = r.OpenSubKey("command");
                Debug.Assert(r != null);
                var c = (string)r.GetValue("");
                var pat = new Regex(@"""([^""]*)");
                var match = pat.Match(c);
                if (match.Success)
                {
                    progFolder = Path.GetDirectoryName(match.Groups[1].Value);
                }
            }
            return string.IsNullOrEmpty(progFolder)? string.Empty : Path.Combine(progFolder, "java.exe");
        }

        #endregion JavaLocation(name)

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
            RedirectOutput = string.Empty;
            const string BeforeProcess = "BeforePwConvert.bat";
            string processFolder = GetProcessFolder(outFullName);
            if (File.Exists(Common.PathCombine(processFolder, BeforeProcess)))
                Run(processFolder, BeforeProcess, '"' + outFullName + '"', true);
        }

        /// <summary>
        /// return folder name that should contain process if it exists
        /// </summary>
        private static string GetProcessFolder(string outFullName)
        {
            var folder = Path.GetDirectoryName(outFullName);
            var parent = Path.GetDirectoryName(folder);
            return Common.PathCombine(parent, "Process");
        }

        /// <summary>
        /// do anything that needs to be done after export but before convert
        /// </summary>
        public static void AfterProcess(string outFullName)
        {
            RedirectOutput = string.Empty;
            const string AfterProcess = "AfterPwConvert.bat";
            try
            {
                string processFolder = GetProcessFolder(outFullName);
                if (File.Exists(Common.PathCombine(processFolder, AfterProcess)))
                    Run(processFolder, AfterProcess, '"' + outFullName + '"', true);
            }
            catch (Exception)
            {
            }
        }
    }
}