// --------------------------------------------------------------------------------------------
// <copyright file="frmSplash.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Splash screen with Project Info
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DesktopAnalytics;
using SIL.Reporting;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class frmSplash : Form
    {
        private int _counter = 0;
        public const string kCompany = "SIL";
        public const string kProduct = "Pathway";
        public DialogResult dr;
        public frmSplash()
        {
            InitializeComponent();
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

        private void tSplash_Tick(object sender, EventArgs e)
        {
            _counter++;
            if (_counter == 5)
            {
                tSplash.Stop();
                dr = DialogResult.OK;
                this.Close();
            }
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {

            if (Common.IsUnixOS())
            {
                this.label5.Location = new Point(label5.Location.X - 20, label5.Location.Y);
            }

            string version = Assembly.GetEntryAssembly().GetName().Version.ToString();

            string versionDate = String.Format("{0} ({1})", version, AssemblyFileDate);

            lblVersionwithDate.Text = "Version: " + versionDate;
            tSplash.Start();


        }

        /// <summary>
        /// The email address people should write to with problems (or new localizations?) for HearThis.
        /// </summary>
        public static string IssuesEmailAddress
        {
            get { return "pathway@sil.org"; }
        }

        /// ------------------------------------------------------------------------------------
        private static void SetUpErrorHandling()
        {
            if (ErrorReport.EmailAddress == null)
            {
                ExceptionHandler.Init();
                ErrorReport.EmailAddress = IssuesEmailAddress;
                ErrorReport.AddStandardProperties();
                ExceptionHandler.AddDelegate(ReportError);
            }
        }

        private static void ReportError(object sender, CancelExceptionHandlingEventArgs e)
        {
            Analytics.ReportException(e.Exception);
        }
    }
}
