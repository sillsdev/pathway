// --------------------------------------------------------------------------------------------
// <copyright file="Pdf.cs" from='2009' to='2014' company='SIL International'>
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
// Pdf represents a Pdf File that is created from an xhtml and css by Prince
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using SIL.Tool;
using PrinceXML.Wrapper;

namespace SIL.PublishingSolution
{
    /// <summary>
    /// Pdf represents a Pdf File that is created from an xhtml and css by Prince
    /// </summary>
    /// <exception cref="MISSINGPRINCE">
    /// Exception raised if PrinceXML is not installed
    /// </exception>
    public class Pdf
    {
        #region Fields
        /// <summary>
        /// Gets or sets xhtml file name
        /// </summary>
        public string Xhtml { get; set; }

        /// <summary>
        /// Gets or sets css file name
        /// </summary>
        public string Css { get; set; }
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Instantiate Pdf file creation classe
        /// </summary>
        public Pdf()
        {
        }

        /// <summary>
        /// Instantiate Pdf file creation classe and member variables
        /// </summary>
        /// <param name="xhtmlFile">file name for xhtml file</param>
        /// <param name="cssFile">file name for css file</param>
        public Pdf(string xhtmlFile, string cssFile)
        {
            Xhtml = xhtmlFile;
            Css = cssFile;
        }
        #endregion Constructors

        #region Create(outName)
        /// <summary>
        /// Write the Pdf file
        /// </summary>
        /// <param name="outName">Name to use for Pdf file</param>
        public void Create(string outName)
        {
            Debug.Assert(!string.IsNullOrEmpty(Xhtml), "xhtml not set");

            if (Common.UnixVersionCheck())
            {
                if (!File.Exists("/usr/bin/Prince"))
                    throw new MISSINGPRINCE();
                Common.RunCommand("Prince ", Xhtml + " " + Css + " -o " + outName, 1);
                return;
            }
            RegistryKey regPrinceKey;
            try
            {
                regPrinceKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MICROSOFT\WINDOWS\CURRENTVERSION\UNINSTALL\Prince_is1");
                if (regPrinceKey == null)
                    regPrinceKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MICROSOFT\WINDOWS\CURRENTVERSION\UNINSTALL\{3AC28E9C-8F06-4E2C-ADDA-726E2230A03A}");
                if (regPrinceKey == null)
                    regPrinceKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\MICROSOFT\WINDOWS\CURRENTVERSION\UNINSTALL\Prince_is1");
                if (regPrinceKey == null)
                    regPrinceKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\MICROSOFT\WINDOWS\CURRENTVERSION\UNINSTALL\{3AC28E9C-8F06-4E2C-ADDA-726E2230A03A}");

            }
            catch (Exception)
            {
                regPrinceKey = null;
            }
            if (regPrinceKey != null)
            {
                var princePath = regPrinceKey.GetValue("InstallLocation");
                var princeFullName = Common.PathCombine(princePath as string, "Engine/Bin/Prince.exe");
                var myPrince = new PrinceXML.Wrapper.Prince(princeFullName);
                if (File.Exists(Css))
                {
	                var mc = new MergeCss();
		            myPrince.StyleSheets.Add(mc.Make(Css, "Temp1.css"));
                }

                myPrince.Convert(Xhtml, outName);
            }
            else
            {
                throw new MISSINGPRINCE();
            }
        }
        #endregion Create(outName)

        #region Exception
        /// <summary>
        /// Exception to raise if PrinceXML not installed
        /// </summary>
        public class MISSINGPRINCE : ApplicationException
        {
        }
        #endregion Exception
    }
}
