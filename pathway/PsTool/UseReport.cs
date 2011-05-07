// --------------------------------------------------------------------------------------------
// <copyright file="UseReport.cs" from='2009' to='2009' company='SIL International'>
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
// Simple access to file contents
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace SIL.Tool
{
    public static class UseReport
    {
        /// <summary>ReportVariable - name of variable in registry to store count</summary>
        private static string _reportVariable = "launchCount";
        public static string ReportVariable
        {
            get
            {
                return _reportVariable;
            }
            set
            {
                _reportVariable = value;
            }
        }

        /// <summary>EmailAddress - where to send report</summary>
        private static string _emailAddress = "Pathway@sil.org";

        public static string EmailAddress
        {
            get
            {
                return _emailAddress;
            }
            set
            {
                Debug.Assert(value.Contains("@"));
                _emailAddress = value;
            }
        }

        /// <summary>WebSite - where to get updates</summary>
        private static string _webSite = "http://pathway.sil.org/";

        public static string WebSite
        {
            get
            {
                return _webSite;
            }
            set
            {
                Debug.Assert(value.StartsWith("http://"));
                _webSite = value;
            }
        }

        /// <summary>DiscussionGroup - where to get help from other users</summary>
        private static string _discussionGroup = "http://tech.groups.yahoo.com/group/typesetdictionary/";

        public static string DiscussionGroup
        {
            get
            {
                return _discussionGroup;
            }
            set
            {
                Debug.Assert(value.StartsWith("http://"));
                _discussionGroup = value;
            }
        }

        /// <summary>ProductName - Name of program for registry</summary>
        private static string _productName = Application.ProductName;

        public static string ProductName
        {
            set
            {
                Debug.Assert(string.IsNullOrEmpty(value));
                _productName = value;
                RegistryAccess.ProductName = value;
            }
            get
            {
                return _productName;
            }
        }

        /// <summary>
        /// This sends the email reports if appropriate.
        /// </summary>
        public static void Check()
        {
            IncrementLaunchCount();
            UsageReport(EmailAddress, string.Format("1. What do you hope {0} will do for you?%0A%0A2. What language are you work on?", ProductName), 1);
            UsageReport(EmailAddress, string.Format("1. Do you have suggestions to improve the program?%0A%0A2. What are you happy with?"), 10);
            UsageReport(EmailAddress, string.Format("1. What would you like to say to others about {0}?%0A%0A2. What languages have you used with {0}", ProductName), 40);
        }

        /// <summary>
        /// call this each time the application is launched if you have launch count-based reporting
        /// </summary>
        private static void IncrementLaunchCount()
        {
            int launchCount = 1 + RegistryAccess.GetIntRegistryValue(ReportVariable, 0);
            RegistryAccess.SetIntRegistryValue(ReportVariable, launchCount);
        }

        /// <summary>
        /// Generates an email with the <paramref name="topMessage">topMessage</paramref> if the launch count = <paramref name="noLaunches">noLaunches</paramref>.
        /// </summary>
        /// <param name="emailAddress">email address to send report to</param>
        /// <param name="topMessage">Message for report</param>
        /// <param name="noLaunches">number of launches at which report should be generated.</param>
        private static void UsageReport(string emailAddress, string topMessage, int noLaunches)
        {
            int launchCount = RegistryAccess.GetIntRegistryValue(ReportVariable, 0);

            if (launchCount == noLaunches)
            {
                // Set the Application label to the name of the app
                Assembly assembly = Assembly.GetExecutingAssembly();
                string version = Application.ProductVersion;
                if (assembly != null)
                {
                    object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                    version = (attributes != null && attributes.Length > 0) ?
                        ((AssemblyFileVersionAttribute)attributes[0]).Version : Application.ProductVersion;
                }

                string emailSubject = string.Format("{0} {1} Report {2} Launches", ProductName, version, launchCount);
                string emailBody = string.Format("<report app='{0}' version='{1}'><stat type='launches' value='{2}'/>%0A{3}%0A%0A%0A</report>%0A%0AWeb site: {4} (for latest version)%0ADiscussion Group: {5} (for help from other users)", ProductName, version, launchCount, topMessage, WebSite, DiscussionGroup);
                string body = emailBody.Replace(Environment.NewLine, "%0A").Replace("\"", "%22").Replace("&", "%26");

                Process p = new Process();
                p.StartInfo.FileName = String.Format("mailto:{0}?subject={1}&body={2}", emailAddress, emailSubject, body);
                p.Start();
            }
        }
    }
}
