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
// Displays the properties of the Current Application
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Windows.Forms;

namespace SIL.PublishingSolution
{
    partial class AboutBooklet : Form
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the AboutDE class.
        /// </summary>
        public AboutBooklet()
        {
            InitializeComponent();
        }
        #endregion

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        /// <summary>
        /// Gets AssemblyFileVersion set by builder
        /// </summary>
        public string AssemblyFileVersion
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyFileVersionAttribute) attributes[0]).Version;
            }
        }

        /// <summary>
        /// Gets AssemblyDescription 
        /// </summary>
        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
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
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
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
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
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
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
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
        private void AboutDE_Load(object sender, EventArgs e)
        {
            Text = String.Format("About {0}", AssemblyTitle);
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = String.Format("Version {0}", AssemblyFileVersion);
            labelCopyright.Text = AssemblyCopyright;
            labelCompanyName.Text = AssemblyCompany;
            textBoxDescription.Text = AssemblyDescription;
        }

        /// <summary>
        /// Close dialog on ok button
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        private void AboutDE_DoubleClick(object sender, EventArgs e)
        {
        //#if DEBUG
            //var dlg = new Localizer(LocDB.DB);
            //dlg.ShowDialog();
        //#endif
        }

        private void AboutDE_Activated(object sender, EventArgs e)
        {
            //Common.SetFont(this);

        }
    }
}
