// --------------------------------------------------------------------------------------------
// <copyright file="AboutDE.cs" from='2009' to='2009' company='SIL International'>
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
// Displays the properties of the calling Assembly (Pathway). 
// Note: the copyright info is coming from the AssemblyInfo.cs in /Pathway/CssDialog/Properties.
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using JWTools;
using Microsoft.Win32;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    partial class AboutPw : Form
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the AboutDE class.
        /// </summary>
        public AboutPw()
        {
            InitializeComponent();
        }
        #endregion

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                //object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetCallingAssembly().CodeBase);
                //System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        /// <summary>
        /// Gets AssemblyFileVersion set by builder
        /// </summary>
        public string AssemblyFileVersion
        {
            get
            {
                //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                //object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyFileVersionAttribute) attributes[0]).Version;
                //return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Gets AssemblyFileDate set by builder
        /// </summary>
        public string AssemblyFileDate
        {
            get
            {
                // first, see if we can get at the last creation date for the calling assembly
                if (File.Exists(Assembly.GetCallingAssembly().Location))
                {
                    return File.GetCreationTime(Assembly.GetCallingAssembly().Location).ToShortDateString();
                }
                // try 2: fallback on the last creation date for the executable path
                return File.GetCreationTime(Application.ExecutablePath).ToShortDateString();
            }
        }

        /// <summary>
        /// Gets AssemblyDescription 
        /// </summary>
        public string AssemblyDescription
        {
            get
            {
                //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                //object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        /// <summary>
        /// Gets AssemblyProduct Name
        /// </summary>
        public string AssemblyProduct
        {
            get
            {
                //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                //object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <summary>
        /// Gets Copyright text.
        /// </summary>
        public string AssemblyCopyright
        {
            get
            {
                //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                //object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        /// <summary>
        /// Gets Company name
        /// </summary>
        public string AssemblyCompany
        {
            get
            {
                //object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                //object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                object[] attributes = Assembly.GetCallingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Load values into dialog controls
        /// </summary>
        private void AboutPw_Load(object sender, EventArgs e)
        {
            //LocDB.Localize(this, null);     // Form Controls

            Text = String.Format("About {0}", AssemblyProduct);
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = String.Format("Version {0} ({1})", AssemblyFileVersion, AssemblyFileDate);
            labelCopyright.Text = AssemblyCopyright;
            labelCompanyName.Text = AssemblyCompany;
            textBoxDescription.Text = AssemblyDescription;

            HelpImproveGetValue(Common.GetOsName(), chkbHelpImprove);
        }

        /// <summary>
        /// Close dialog on ok button
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            HelpImproveSetValue(Common.GetOsName(), chkbHelpImprove);
            Close();
        }

        private static void HelpImproveSetValue(string osName, CheckBox chbHelpImprove)
        {
            string helpImproveValue = "False";

            if (chbHelpImprove.Checked)
                helpImproveValue = "True";

            if (osName == "Windows7")
            {
                string registryValue = Common.GetValueFromRegistryFromCurrentUser("SOFTWARE\\Wow6432Node\\SIL\\Pathway",
                                                                       "HelpImprove");

                if (registryValue == null)
                {
                    RegistryKey masterKey =
                        Registry.CurrentUser.CreateSubKey("SOFTWARE\\Wow6432Node\\SIL\\Pathway");
                    masterKey.SetValue("HelpImprove", helpImproveValue);
                    masterKey.Close();
                }
                else
                {
                    RegistryKey masterKey =
                        Registry.CurrentUser.CreateSubKey("SOFTWARE\\Wow6432Node\\SIL\\Pathway");
                    masterKey.SetValue("HelpImprove", helpImproveValue);
                    masterKey.Close();
                }
            }
            else if (osName == "Windows XP")
            {
                string registryValue = Common.GetValueFromRegistryFromCurrentUser("SOFTWARE\\SIL\\Pathway", "HelpImprove");

                if (registryValue == null)
                {
                    RegistryKey masterKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\SIL\\Pathway");
                    masterKey.SetValue("HelpImprove", helpImproveValue);
                    masterKey.Close();
                }
                else
                {
                    RegistryKey masterKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\SIL\\Pathway");
                    masterKey.SetValue("HelpImprove", helpImproveValue);
                    masterKey.Close();
                }
            }
        }

        private static void HelpImproveGetValue(string osName, CheckBox cbHelpImprove)
        {
            string registryValue = null;

            if (osName == "Windows7")
            {
                registryValue = Common.GetValueFromRegistryFromCurrentUser("SOFTWARE\\Wow6432Node\\SIL\\Pathway", "HelpImprove");
                if (registryValue == null)
                {
                    cbHelpImprove.Checked = false;
                }
            }
            else if (osName == "Windows XP")
            {
                registryValue = Common.GetValueFromRegistryFromCurrentUser("SOFTWARE\\SIL\\Pathway", "HelpImprove");

                if (registryValue == null)
                {
                    cbHelpImprove.Checked = false;
                }
            }

            if (registryValue != null)
            {
                if (registryValue == "True")
                {
                    cbHelpImprove.Checked = true;
                }
                else
                {
                    cbHelpImprove.Checked = false;
                }
            }
        }

        #endregion

        private void AboutPw_DoubleClick(object sender, EventArgs e)
        {
        #if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
        #endif
        }

        private void AboutPw_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);

        }
    }
}
