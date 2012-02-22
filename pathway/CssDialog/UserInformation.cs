// --------------------------------------------------------------------------------------------
// <copyright file="UserInformation.cs" from='2009' to='2009' company='SIL International'>
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
// UserInformation
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Microsoft.Win32;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class UserInformation
    {
        #region Private Variables

        private string userSystemGuid = string.Empty;
        private string oSName = string.Empty;
        private string oSServicePack = string.Empty;
        private string userSystemName = string.Empty;
        private string userIPAddress = string.Empty;
        private string pathwayVersion = string.Empty;
        private string tEVersion = string.Empty;
        private string javaVersion = string.Empty;
        private string libraofficeVersion = string.Empty;
        private string prince = string.Empty;
        private string xelatexVersion = string.Empty;
        private string indesignVersion = string.Empty;
        private string weSay = string.Empty;
        private string bloom = string.Empty;
        private string fontLists = string.Empty;
        private string browserList = string.Empty;
        private string geoLocation = string.Empty;
        private string systemCountry = string.Empty;
        private string language = string.Empty;
        private string frameworkVersion = string.Empty;
        private string paratext = string.Empty;

        #endregion

        #region Public Variables

        public string UserSystemGuid
        {
            get { return userSystemGuid; }
            set { userSystemGuid = value; }
        }

        public string OSName
        {
            get { return oSName; }
            set { oSName = value; }
        }

        public string OSServicePack
        {
            get { return oSServicePack; }
            set { oSServicePack = value; }
        }

        public string UserSystemName
        {
            get { return userSystemName; }
            set { userSystemName = value; }
        }

        public string UserIpAddress
        {
            get { return userIPAddress; }
            set { userIPAddress = value; }
        }

        public string PathwayVersion
        {
            get { return pathwayVersion; }
            set { pathwayVersion = value; }
        }

        public string TEVersion
        {
            get { return tEVersion; }
            set { tEVersion = value; }
        }

        public string JavaVersion
        {
            get { return javaVersion; }
            set { javaVersion = value; }
        }

        public string LibraofficeVersion
        {
            get { return libraofficeVersion; }
            set { libraofficeVersion = value; }
        }

        public string Prince
        {
            get { return prince; }
            set { prince = value; }
        }

        public string XelatexVersion
        {
            get { return xelatexVersion; }
            set { xelatexVersion = value; }
        }

        public string IndesignVersion
        {
            get { return indesignVersion; }
            set { indesignVersion = value; }
        }

        public string WeSay
        {
            get { return weSay; }
            set { weSay = value; }
        }

        public string Bloom
        {
            get { return bloom; }
            set { bloom = value; }
        }

        public string FontLists
        {
            get { return fontLists; }
            set { fontLists = value; }
        }

        public string BrowserList
        {
            get { return browserList; }
            set { browserList = value; }
        }

        public string GeoLocation
        {
            get { return geoLocation; }
            set { geoLocation = value; }
        }

        public string SystemCountry
        {
            get { return systemCountry; }
            set { systemCountry = value; }
        }

        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        public string FrameworkVersion
        {
            get { return frameworkVersion; }
            set { frameworkVersion = value; }
        }

        public string Paratext
        {
            get { return paratext; }
            set { paratext = value; }
        }

        #endregion

        #region Methods

        public void GetUserInformation()
        {
            try
            {
                oSName = GetOsName();
                userSystemGuid = GetUserSystemGuid(OSName);
                oSServicePack = GetOsVersion();
                userSystemName = GetMachineName();
                language = GetLanguage();
                //fontLists = GetFontLists();
                systemCountry = GetsystemCountry(Language);
                userIPAddress = GetUserIpAddress();
                javaVersion = GetJavaVersion(OSName);
                xelatexVersion = GetXelatexVersion(OSName);
                pathwayVersion = GetPathwayVersion(OSName);
                libraofficeVersion = GetLibraofficeVersion(OSName);
                paratext = GetParatext(OSName);
                tEVersion = GetTeVersion(OSName);
                prince = GetPrince(OSName);
                indesignVersion = GetIndesignVersion(OSName);
                browserList = GetBrowserList(OSName);
                frameworkVersion = GetFrameworkVersion(OSName);
                geoLocation = "Unknown";

                SetToPHP(userSystemGuid, oSName, oSServicePack, userSystemName, userIPAddress, pathwayVersion,
                         javaVersion, paratext, tEVersion, libraofficeVersion, prince, xelatexVersion, indesignVersion, weSay,
                         bloom, fontLists, browserList, systemCountry, geoLocation, language, frameworkVersion);

            }
            catch { }
        }

        private static string GetsystemCountry(string Language)
        {
            string systemCountry;
            systemCountry = Language.Substring(Language.IndexOf('(') + 1,
                                             Language.LastIndexOf(')') - Language.IndexOf('(') - 1);
            return systemCountry;
        }

        private static string GetLanguage()
        {
            string Language;
            Language = CultureInfo.CurrentCulture.EnglishName;
            return Language;
        }

        private static string GetFrameworkVersion(string osName)
        {
            string frameworkVersion = null;
            if (osName == "Windows7")
            {
                RegistryKey installedVersions =
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Wow6432Node\\Microsoft\\NET Framework Setup\\NDP");
                if (installedVersions != null)
                {
                    string[] versionNames = installedVersions.GetSubKeyNames();
                    frameworkVersion =
                        Convert.ToDouble(versionNames[versionNames.Length - 1].Remove(0, 1),
                                         CultureInfo.InvariantCulture)
                            .ToString(CultureInfo.InvariantCulture);
                }
            }
            else if (osName == "Windows XP")
            {
                RegistryKey installedVersions =
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\NET Framework Setup\\NDP");
                if (installedVersions != null)
                {
                    string[] versionNames = installedVersions.GetSubKeyNames();
                    frameworkVersion =
                        Convert.ToDouble(versionNames[versionNames.Length - 1].Remove(0, 1),
                                         CultureInfo.InvariantCulture)
                            .ToString(CultureInfo.InvariantCulture);
                }
            }
            return frameworkVersion;
        }

        private static string GetBrowserList(string osName)
        {
            string browserList = null;
            if (osName == "Windows7")
            {
                browserList = GetValueFromRegistryHTTPCLASSROOT("http\\shell\\open\\command", "");
                browserList = Common.RightRemove(browserList, ".exe");
                browserList = Common.LeftRemove(browserList, "\\");
            }
            else if (osName == "Windows XP")
            {
                browserList = GetValueFromRegistryHTTPCLASSROOT("http\\shell\\open\\command", "");
                browserList = Common.RightRemove(browserList, ".exe");
                browserList = Common.LeftRemove(browserList, "\\");
            }
            return browserList;
        }

        private static string GetIndesignVersion(string osName)
        {
            string indesignVersion = null;
            if (osName == "Windows7")
            {
                indesignVersion =
                    Common.GetValueFromRegistry("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\InDesign.exe", "Path");
                indesignVersion = Common.LeftRemove(indesignVersion, "Adobe\\");
            }
            else if (osName == "Windows XP")
            {
                indesignVersion =
                    Common.GetValueFromRegistry("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\InDesign.exe", "Path");
                indesignVersion = Common.LeftRemove(indesignVersion, "Adobe\\");
            }
            return indesignVersion;
        }

        private static string GetPrince(string osName)
        {
            string prince = null;
            if (osName == "Windows7")
            {
                prince =
                    Common.GetValueFromRegistry(
                        "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Prince_is1", "DisplayName");
            }
            else if (osName == "Windows XP")
            {
                prince = Common.GetValueFromRegistry("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Prince_is1",
                                                     "DisplayName");
            }
            return prince;
        }

        private static string GetTeVersion(string osName)
        {
            string teVersion = null;
            if (osName == "Windows7")
            {
                teVersion = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\SIL\\FieldWorks\\7.0", "RootCodeDir");
                teVersion = Common.RightRemove(teVersion, "\\");
                teVersion = Common.LeftRemove(teVersion, "SIL\\");
            }
            else if (osName == "Windows XP")
            {
                teVersion = Common.GetValueFromRegistry("SOFTWARE\\SIL\\FieldWorks\\7.0", "RootCodeDir");
                teVersion = Common.RightRemove(teVersion, "\\");
                teVersion = Common.LeftRemove(teVersion, "SIL\\");
            }
            return teVersion;
        }

        private static string GetParatext(string osName)
        {
            string paratext = null;
            if (osName == "Windows7")
            {
                paratext = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\ScrChecks\\1.0\\Program_Files_Directory_Ptw7", "");
                paratext = Common.RightRemove(paratext, "\\");
                paratext = Common.LeftRemove(paratext, "\\");
                paratext = Common.LeftRemove(paratext, "\\");
            }
            else if (osName == "Windows XP")
            {
                paratext = Common.GetValueFromRegistry("SOFTWARE\\ScrChecks\\1.0\\Program_Files_Directory_Ptw7", "");
                paratext = Common.RightRemove(paratext, "\\");
                paratext = Common.LeftRemove(paratext, "\\");
                paratext = Common.LeftRemove(paratext, "\\");
            }

            return paratext;
        }

        private static string GetLibraofficeVersion(string osName)
        {
            string libraofficeVersion = null;
            if (osName == "Windows7")
            {
                libraofficeVersion =
                    Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\LibreOffice\\UNO\\InstallPath",
                                                "");
            }
            else if (osName == "Windows XP")
            {
                libraofficeVersion = Common.GetValueFromRegistry("SOFTWARE\\SIL\\PathwayXeLaTeX",
                                                                 "");
            }
            return libraofficeVersion;
        }

        private static string GetPathwayVersion(string osName)
        {
            string pathwayVersion = null;
            /* Pathway Version  */
            //string appName = Assembly.GetAssembly(this.GetType()).Location;
            //AssemblyName assemblyName = AssemblyName.GetAssemblyName(appName);
            //PathwayVersion = assemblyName.Version.ToString();

            // Software / Microsoft / windows/ currentversion / installer / userdata / S-1-5-21-3328216688-2622995857-1421487914-1000 // Products // CA322A0D6F74A404FB17159D2EF6FE48 //  InstallProperties

            if (osName == "Windows7")
            {
                pathwayVersion = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\SIL\\Pathway", "PathwayDir");

                string appName = pathwayVersion + "CssDialog.dll";
                if (File.Exists(appName))
                {
                    AssemblyName assemblyName = AssemblyName.GetAssemblyName(appName);
                    pathwayVersion = assemblyName.Version.ToString();
                }
            }
            else if (osName == "Windows XP")
            {
                pathwayVersion = Common.GetValueFromRegistry("SOFTWARE\\SIL\\Pathway", "PathwayDir");
                string appName = pathwayVersion + "CssDialog.dll";
                if (File.Exists(appName))
                {
                    AssemblyName assemblyName = AssemblyName.GetAssemblyName(appName);
                    pathwayVersion = assemblyName.Version.ToString();
                }
            }
            return pathwayVersion;
        }

        private static string GetXelatexVersion(string osName)
        {
            string xelatexVersion = null;
            if (osName == "Windows7")
            {
                xelatexVersion = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\SIL\\PathwayXeLaTeX", "XeLaTexVer");
            }
            else if (osName == "Windows XP")
            {
                xelatexVersion = Common.GetValueFromRegistry("SOFTWARE\\SIL\\PathwayXeLaTeX", "XeLaTexVer");
            }
            return xelatexVersion;
        }

        private static string GetJavaVersion(string osName)
        {
            string javaVersion = string.Empty;
            /* Java Version  */
            if (osName == "Windows7")
            {
                javaVersion =
                    Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\JavaSoft\\Java Runtime Environment",
                                                "CurrentVersion");
            }
            else if (osName == "Windows XP")
            {
                javaVersion = Common.GetValueFromRegistry("SOFTWARE\\JavaSoft\\Java Runtime Environment",
                                                          "CurrentVersion");
            }
            return javaVersion;
        }

        private static string GetUserIpAddress()
        {
            string userIpAddress = string.Empty;
            IPHostEntry ip = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ipAddress in ip.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    userIpAddress = ipAddress.ToString();
                    return userIpAddress;
                }
            }

            return userIpAddress;
        }

        private static string GetMachineName()
        {
            return Environment.MachineName;
        }

        private string GetOsNameVersion()
        {
            return GetOsName() + " - " + GetOsVersion();
        }

        private static string GetUserSystemGuid(string osName)
        {
            string UserSystemGuid;
            UserSystemGuid = Guid.NewGuid().ToString();

            if (osName == "Windows7")
            {

                string getUserSystemGuid = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\SIL\\Pathway", "PathwayGUID");

                if (getUserSystemGuid == null)
                {
                    RegistryKey masterKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Wow6432Node\\SIL\\Pathway");
                    masterKey.SetValue("PathwayGUID", UserSystemGuid);
                    masterKey.Close();
                }
                else
                {
                    UserSystemGuid = getUserSystemGuid;
                }

            }
            else if (osName == "Windows XP")
            {
                string getUserSystemGuid = Common.GetValueFromRegistry("SOFTWARE\\SIL\\Pathway", "PathwayGUID");

                if (getUserSystemGuid == null)
                {
                    RegistryKey masterKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\SIL\\Pathway");
                    masterKey.SetValue("PathwayGUID", UserSystemGuid);
                    masterKey.Close();
                }
                else
                {
                    UserSystemGuid = getUserSystemGuid;
                }
            }

            return UserSystemGuid;
        }

        private static string GetValueFromRegistryHTTPCLASSROOT(string subKey, string keyName)
        {
            // Opening the registry key

            RegistryKey rk = Registry.ClassesRoot;
            // Open a subKey as read-only

            RegistryKey sk1 = rk.OpenSubKey(subKey);
            // If the RegistrySubKey doesn't exist -> (null)

            if (sk1 == null)
            {
                return null;
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    return (string)sk1.GetValue(keyName.ToUpper());
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        private string GetFontLists()
        {
            string listOfFont = string.Empty;
            // Create an instance of the InstalledFontCollection class
            InstalledFontCollection fonts = new InstalledFontCollection();
            //Loop through all the fonts in the system.
            foreach (FontFamily family in fonts.Families)
            {
                listOfFont += family.Name + ",";
            }
            return listOfFont;
        }

        public string GetOsName()
        {
            OperatingSystem osInfo = Environment.OSVersion;

            switch (osInfo.Platform)
            {
                case System.PlatformID.Win32NT:
                    switch (osInfo.Version.Major)
                    {
                        case 3:
                            return "Windows NT 3.51";
                            break;
                        case 4:
                            return "Windows NT 4.0";
                            break;
                        case 5:
                            if (osInfo.Version.Minor == 0)
                                return "Windows 2000";
                            else
                                return "Windows XP";
                            break;
                        case 6:
                            if (osInfo.Version.Minor == 1)
                                return "Windows7";
                            break;
                    }
                    break;

            }
            return osInfo.Platform.ToString();
        }

        public string GetOsVersion()
        {
            OperatingSystem osInfo = Environment.OSVersion;

            return osInfo.VersionString;
        }

        public string SetToPHP(string UserSystemGuid, string OSNameVersion, string OSServicePack, string UserSystemName, string UserIPAddress,
            string PathwayVersion, string JavaVersion, string ParatextVersion, string TEVersion, string LibraofficeVersion, string Prince, string XelatexVersion,
            string IndesignVersion, string WeSay, string Bloom, string FontLists, string BrowserList, string SystemCountry, string GeoLocation, string Language, string FrameworkVersion)
        {
            string uri = string.Empty;
            // This is where we will send it
            uri = "http://myphpapps.com.cws10.my-hosting-panel.com/configdb.php" + "?UserSystemGuid=" +
                             UserSystemGuid + "&OSNameVersion=" + OSNameVersion + "&OSServicePack=" + OSServicePack +
                             "&UserSystemName=" + UserSystemName + "&UserIPAddress=" + UserIPAddress +
                             "&PathwayVersion=" +
                             PathwayVersion + "&JavaVersion=" + JavaVersion + "&ParatextVersion=" + ParatextVersion +
                             "&TEVersion=" + TEVersion + "&LibraofficeVersion=" + LibraofficeVersion +
                             "&Prince=" + Prince + "&XelatexVersion=" + XelatexVersion + "&IndesignVersion=" +
                             IndesignVersion + "&WeSay=" + WeSay +
                             "&Bloom=" + Bloom + "&FontLists=" + FontLists + "&BrowserList=" + BrowserList +
                             "&SystemCountry=" + SystemCountry + "&GeoLocation=" + GeoLocation +
                             "&Language=" + Language + "&FrameworkVersion=" + FrameworkVersion + "";
            try
            {
                // This is what we are sending sample, don't remove or change
                string post_data = "&FrameworkVersionsss=sdf";
                // create a request
                HttpWebRequest request = (HttpWebRequest)
                                         WebRequest.Create(uri);
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";

                // turn our request string into a byte stream
                byte[] postBytes = Encoding.ASCII.GetBytes(post_data);

                // this is important - make sure you specify type this way
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postBytes.Length;
                Stream requestStream = request.GetRequestStream();

                // now send it
                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Close();

                // grab te response and print it out to the console along with the status code
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //Console.WriteLine(new StreamReader(response.GetResponseStream()).ReadToEnd());
                //Console.WriteLine(response.StatusCode);

                var consoleString = (new StreamReader(response.GetResponseStream()).ReadToEnd());
                var consoleStatusString = (response.StatusCode);
            }
            catch { }

            return uri;
        }

        private static void Search_For_Registry_Value(RegistryKey registryKey, string subKey, string search, bool getSubKey)
        {
            if (registryKey.ValueCount > 0)
            {
                foreach (var temp in registryKey.GetValueNames())
                {
                    if (temp.ToLower().Contains(search.ToLower()))
                    {
                        Console.WriteLine(String.Format("Match Found In Registry Key Value {0} Present At Location {1}", temp, registryKey.Name));
                    }
                }
            }
            
            if(getSubKey)
            {
                registryKey = registryKey.OpenSubKey(subKey);
            }

            

            if (registryKey.SubKeyCount > 0)
            {
                foreach (var temp in registryKey.GetSubKeyNames())
                {
                    try
                    {
                        if (registryKey.OpenSubKey(temp).SubKeyCount > 0)
                        {
                            Search_For_Registry_Value(registryKey.OpenSubKey(temp), subKey, search, false);
                        }
                    }
                    catch { }
                }
            }
        }

        #endregion
    }
}
