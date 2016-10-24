// --------------------------------------------------------------------------------------------
// <copyright file="UserInformation.cs" from='2009' to='2014' company='SIL International'>
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
// UserInformation
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
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

        public void GetUserInformation(bool sendUserInfo)
        {
            oSName = GetOsName();
            oSServicePack = GetOsVersion();
            language = GetLanguage();
            systemCountry = GetsystemCountry(Language);
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
            userSystemGuid = GetUserSystemGuid(OSName);
            if (sendUserInfo)
            {
                SetToPHP(userSystemGuid, oSName, oSServicePack, pathwayVersion, javaVersion, paratext, tEVersion, libraofficeVersion, prince, xelatexVersion,
                         indesignVersion, weSay, bloom, fontLists, browserList, systemCountry, geoLocation, language, frameworkVersion);
            }

        }

        public bool CheckInternetAvailability(string ipAddress)
        {
            Ping ping = new Ping();
            PingReply pingStatus = ping.Send(IPAddress.Parse(ipAddress));
            if (pingStatus.Status == IPStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private static string GetsystemCountry(string Language)
        {
            try
            {
                string systemCountry;
                systemCountry = Language.Substring(Language.IndexOf('(') + 1,
                                                 Language.LastIndexOf(')') - Language.IndexOf('(') - 1);
                return systemCountry;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string GetLanguage()
        {
            try
            {
                string language = CultureInfo.CurrentCulture.EnglishName;
                return language;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string GetFrameworkVersion(string osName)
        {
            try
            {
                string frameworkVersion = null;
                if (osName.Contains("Windows"))
                {
                    RegistryKey installedVersions =
                        Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Wow6432Node\\Microsoft\\NET Framework Setup\\NDP") ??
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
            catch
            {
                return string.Empty;
            }
        }

        private static string GetBrowserList(string osName)
        {
            try
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
            catch
            {
                return string.Empty;
            }
        }

        private static string GetIndesignVersion(string osName)
        {
            try
            {
                string indesignVersion = null;
                if (osName == "Windows7")
                {
                    indesignVersion =
                        Common.GetValueFromRegistry(
                            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\InDesign.exe", "Path");
                    indesignVersion = Common.LeftRemove(indesignVersion, "Adobe\\");
                }
                else if (osName == "Windows XP")
                {
                    indesignVersion =
                        Common.GetValueFromRegistry(
                            "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\InDesign.exe", "Path");
                    indesignVersion = Common.LeftRemove(indesignVersion, "Adobe\\");
                }
                return indesignVersion;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string GetPrince(string osName)
        {
            try
            {
                string prince = null;
                if (osName.Contains("Windows"))
                {
                    prince =
                        Common.GetValueFromRegistry(
                            "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Prince_is1", "DisplayName");
                    if (string.IsNullOrEmpty(prince))
                    { // Handle 32-bit Windows 7 and XP
                        prince = Common.GetValueFromRegistry("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Prince_is1", "DisplayName");
                    }
                }
                return prince;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string GetTeVersion(string osName)
        {
            try
            {
                string teVersion = null;
                if (osName.Contains("Windows"))
                {
                    teVersion = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\SIL\\FieldWorks\\7.0", "RootCodeDir");
                    if (string.IsNullOrEmpty(teVersion))
                    { // Handle 32-bit Windows 7 and XP
                        teVersion = Common.GetValueFromRegistry("SOFTWARE\\SIL\\FieldWorks\\7.0", "RootCodeDir");
                    }
                    teVersion = Common.RightRemove(teVersion, "\\");
                    teVersion = Common.LeftRemove(teVersion, "SIL\\");
                }
                return teVersion;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string GetParatext(string osName)
        {
            string paratext = null;
            try
            {
                if (osName.Contains("Windows"))
                {
                    paratext =
                        Common.GetValueFromRegistry(
                            "SOFTWARE\\Wow6432Node\\ScrChecks\\1.0\\Program_Files_Directory_Ptw7", "");
                    if (string.IsNullOrEmpty(paratext))
                    { // Handle 32-bit Windows 7 and XP
                        paratext = Common.GetValueFromRegistry("SOFTWARE\\ScrChecks\\1.0\\Program_Files_Directory_Ptw7", "");
                    }
                    paratext = Common.RightRemove(paratext, "\\");
                    paratext = Common.LeftRemove(paratext, "\\");
                    paratext = Common.LeftRemove(paratext, "\\");
                }
            }
            catch {}
            return paratext;
        }

        private static string GetLibraofficeVersion(string osName)
        {
            string libreofficeVersion = null;
            try
            {
                if (osName.Contains("Windows"))
                {
                    libreofficeVersion =
                        Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\LibreOffice\\UNO\\InstallPath",
                                                    "");
                    if (string.IsNullOrEmpty(libreofficeVersion))
                    { // Handle 32-bit Windows 7 and XP
                        libreofficeVersion =
                            Common.GetValueFromRegistry("SOFTWARE\\LibreOffice\\UNO\\InstallPath", "");
                    }
                }
            }
            catch {}
            return libreofficeVersion;
        }

        private static string GetPathwayVersion(string osName)
        {
            string pathwayVersion = null;
            try
            {
                if (osName.Contains("Windows"))
                {
                    pathwayVersion = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\SIL\\Pathway", "PathwayDir");
                    if (string.IsNullOrEmpty(pathwayVersion))
                    { // Handle 32-bit Windows 7 and XP
                        pathwayVersion = Common.GetValueFromRegistry("SOFTWARE\\SIL\\Pathway", "PathwayDir");
                    }

                    string appName = pathwayVersion + "CssDialog.dll";
                    if (File.Exists(appName))
                    {
                        AssemblyName assemblyName = AssemblyName.GetAssemblyName(appName);
                        pathwayVersion = assemblyName.Version.ToString();
                    }
                }
            }
            catch {}
            return pathwayVersion;
        }

        private static string GetXelatexVersion(string osName)
        {
            string xelatexVersion = null;
            try
            {
                if (osName.Contains("Windows"))
                {
                    xelatexVersion = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\SIL\\PathwayXeLaTeX", "XeLaTexVer");
                    if (string.IsNullOrEmpty(xelatexVersion))
                    { // Handle 32-bit Windows 7 and XP
                        xelatexVersion = Common.GetValueFromRegistry("SOFTWARE\\SIL\\PathwayXeLaTeX", "XeLaTexVer");
                    }
                }
            }
            catch {}
            return xelatexVersion;
        }

        private static string GetJavaVersion(string osName)
        {
            string javaVersion = string.Empty;
            try
            {
                if (osName.Contains("Windows"))
                {
                    javaVersion =
                        Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\JavaSoft\\Java Runtime Environment",
                                                    "CurrentVersion");
                    if (string.IsNullOrEmpty(javaVersion))
                    { // Handle 32-bit Windows 7 and XP
                        javaVersion = Common.GetValueFromRegistry("SOFTWARE\\JavaSoft\\Java Runtime Environment", "CurrentVersion");
                    }
                }
            }
            catch {}
            return javaVersion;
        }

        private static string GetMachineName()
        {
            try
            {
                return Environment.MachineName;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GetUserSystemGuid(string osName)
        {
            try
            {
                string UserSystemGuid;
                UserSystemGuid = GetUserEncryptedCustomGUID();
                try
                {
                    if (osName.Contains("Windows"))
                    {

                        string getUserSystemGuid = Common.GetValueFromRegistryFromCurrentUser("SOFTWARE\\Wow6432Node\\SIL\\Pathway",
                                                                               "PathwayGUID");
                        if (string.IsNullOrEmpty(getUserSystemGuid))
                        { // Handle 32-bit Windows 7 and XP
                            getUserSystemGuid = Common.GetValueFromRegistryFromCurrentUser("SOFTWARE\\SIL\\Pathway", "PathwayGUID");
                        }

                        if (getUserSystemGuid == null)
                        {
                            RegistryKey masterKey;
                            try
                            {
                                masterKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Wow6432Node\\SIL\\Pathway");
                            }
                            catch (Exception) // Handle Windows 7 32-bit
                            {
                                masterKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\SIL\\Pathway");
                            }
                            masterKey.SetValue("PathwayGUID", UserSystemGuid);
                            masterKey.Close();
                        }
                        else
                        {
                            UserSystemGuid = getUserSystemGuid;
                        }

                    }
                    else
                    {
                        string getUserSystemGuid = Common.GetValueFromRegistryFromCurrentUser("SOFTWARE\\SIL\\Pathway", "PathwayGUID");
                        if (getUserSystemGuid == null)
                        {
                            RegistryKey masterKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\SIL\\Pathway");
                            masterKey.SetValue("PathwayGUID", UserSystemGuid);
                            masterKey.Close();
                        }
                        else
                        {
                            UserSystemGuid = getUserSystemGuid;
                        }
                    }
                }
                catch { }
                return UserSystemGuid;
            }
            catch
            {
                return string.Empty;
            }
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
					Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        public string GetOsName()
        {
            try
            {
                OperatingSystem osInfo = Environment.OSVersion;
	            var versionString = osInfo.Platform.ToString();
                switch (osInfo.Platform)
                {
                    case System.PlatformID.Win32NT:
                        switch (osInfo.Version.Major)
                        {
                            case 3:
                                versionString = "Windows NT 3.51";
                                break;
                            case 4:
								versionString = "Windows NT 4.0";
                                break;
                            case 5:
                                versionString = osInfo.Version.Minor == 0 ? "Windows 2000" : "Windows XP";
                                break;
                            case 6:
		                        versionString = osInfo.Version.Minor == 1 ? "Windows7" : "Windows8";
								break;
                        }
                        break;
                }
                return versionString;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetOsVersion()
        {
            try
            {
                OperatingSystem osInfo = Environment.OSVersion;
                return osInfo.VersionString;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string SetToPHP(string UserSystemGuid, string OSNameVersion, string OSServicePack, string PathwayVersion, string JavaVersion, string ParatextVersion, string TEVersion, string LibraofficeVersion, string Prince, string XelatexVersion,
            string IndesignVersion, string WeSay, string Bloom, string FontLists, string BrowserList, string SystemCountry, string GeoLocation, string Language, string FrameworkVersion)
        {
            string uri = string.Empty;
            // This is where we will send it
            uri = "http://myphpapps.com.cws10.my-hosting-panel.com/configdb.php" + "?UserSystemGuid=" +
                             UserSystemGuid + "&OSNameVersion=" + OSNameVersion + "&OSServicePack=" + OSServicePack +
                             "&PathwayVersion=" + PathwayVersion + "&JavaVersion=" + JavaVersion + "&ParatextVersion=" + ParatextVersion +
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
            }
            catch { }

            return uri;
        }


        public string GetUserEncryptedCustomGUID()
        {
            string getUserData = string.Empty;
            string uri = string.Empty;
            // This is where we will send it
            uri = "http://myphpapps.com.cws10.my-hosting-panel.com/getUserIP.php" + "?UserSystemGuid=" + UserSystemGuid + "";
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

                var consoleString = (new StreamReader(response.GetResponseStream()).ReadToEnd());
                var consoleStatusString = (response.StatusCode);

                string getIpValue = consoleString;
                getIpValue = Common.RightRemove(getIpValue, "IP");
                getIpValue = Common.LeftRemove(getIpValue, "IP");
                userSystemName = GetMachineName();
                getUserData = userSystemName + "$" + getIpValue;
                getUserData = CalculateSHA1(getUserData, Encoding.UTF8);
            }
            catch { }

            return getUserData;
        }

        /// <summary>
        /// Calculates SHA1 hash
        /// </summary>
        /// <param name="text">input string</param>
        /// <param name="enc">Character encoding</param>
        /// <returns>SHA1 hash</returns>
        public static string CalculateSHA1(string text, Encoding enc)
        {
            // Convert the input string to a byte array
            byte[] buffer = enc.GetBytes(text);

            // In doing your test, you won't want to re-initialize like this every time you test a
            // string.
            SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();

            // The replace won't be necessary for your tests so long as you are consistent in what
            // you compare.    
            string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }

        #endregion
    }
}
