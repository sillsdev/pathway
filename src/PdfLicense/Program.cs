using SIL.Tool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace PdfLicense
{
    public class Program
    {
        public static int ExitCode;
	    private static readonly List<string> ReadLicenseFilesBylines = new List<string>();

	    private static void Main()
        {

            var allUserPath = GetAllUserPath();
            ReadPathinLicenseFile(allUserPath);
            if (ReadLicenseFilesBylines.Count <= 0)
            {
                return;
            }
            var creatorTool = ReadLicenseFilesBylines[3];
            var getPsApplicationPath = GetPsApplicationPath();
			getPsApplicationPath = Path.Combine(getPsApplicationPath, "Export");
            getPsApplicationPath = Path.Combine(getPsApplicationPath, "ApplyPDFLicenseInfo.exe");
			if (getPsApplicationPath.StartsWith ("/")) {
				getPsApplicationPath = @"/usr/bin/ApplyPDFLicenseInfo";
			}
            if (File.Exists(getPsApplicationPath))
            {
	            //RunCommand(getPsApplicationPath, "", creatorTool.ToLower() == "libreoffice" ? 0 : 1);
	            RunCommand(getPsApplicationPath, "", 0);
            }
		}

        #region GetPSApplicationPath()

        public static string ProgInstall = string.Empty;
        public static string SupportFolder = string.Empty;
        /// <summary>
        /// Return the Local setting Path+ "SIL\Dictionary"
        /// </summary>
        /// <returns>Dictionary Setting Path</returns>
        public static string GetPsApplicationPath()
        {
            if (ProgInstall == string.Empty)
                ProgInstall = GetApplicationPath();
            return SupportFolder == "" ? ProgInstall : PathCombine(ProgInstall, SupportFolder);
        }
        #endregion

        public static string GetApplicationPath()
        {
            var pathwayDir = GetPathwayDir();
            return string.IsNullOrEmpty(pathwayDir) ? Directory.GetCurrentDirectory() : pathwayDir;
        }

        public static string GetPathwayDir()
        {
	        return RegistryHelperLite.FallbackStringValue("SIL/Pathway", "PathwayDir");
        }

        public static bool RunCommand(string szCmd, string szArgs, int wait)
        {
            if (szCmd == null) return false;
	        using (var myproc = new Process
	        {
		        EnableRaisingEvents = false,
		        StartInfo =
		        {
			        CreateNoWindow = true,
			        WindowStyle = ProcessWindowStyle.Hidden,
			        FileName = szCmd,
			        Arguments = szArgs
		        }
	        })
	        {
		        if (!myproc.Start()) return false;
		        //Using WaitForExit( ) allows for the host program
		        //to wait for the command its executing before it continues
		        if (wait == 1) myproc.WaitForExit();
		        else myproc.Close();

		        return true;
	        }
		}

        private static void ReadPathinLicenseFile(string allUserPath)
        {
            var fileLoc = PathCombine(allUserPath, "License.txt");

	        if (!File.Exists(fileLoc)) return;
	        using (var reader = new StreamReader(fileLoc))
	        {
		        string line;

		        while ((line = reader.ReadLine()) != null)
		        {
			        ReadLicenseFilesBylines.Add(line);
		        }

		        reader.Close();
	        }
        }

        /// <summary>
        /// Get all user path
        /// </summary>
        /// <returns></returns>
        public static string GetAllUserPath()
        {
            string allUserPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            allUserPath += "/SIL/Pathway";
            return DirectoryPathReplace(allUserPath);
        }

        /// <summary>
        /// Make sure the path contains the proper / for the operating system.
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns>normalized path</returns>
        public static string PathCombine(string path1, string path2)
        {
            path1 = DirectoryPathReplace(path1);
            path2 = DirectoryPathReplace(path2);
            if (path1 == null)
            {
                return path2;
            }

	        return path2 == null ? path1 : Path.Combine(path1, path2);
        }

        /// <summary>
        /// Make sure the path contains the proper / for the operating system.
        /// </summary>
        /// <param name="path">input path</param>
        /// <returns>normalized path</returns>
        public static string DirectoryPathReplace(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;

            var returnPath = path.Replace('/', Path.DirectorySeparatorChar);
            returnPath = returnPath.Replace('\\', Path.DirectorySeparatorChar);
            return returnPath;
        }
    }
}
