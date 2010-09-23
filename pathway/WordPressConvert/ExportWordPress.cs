// --------------------------------------------------------------------------------------------
// <copyright file="WordPressConvert.cs" from='2010' to='2010' company='SIL International'>
//      Copyright © 2010, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Create Wordpress blog 
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Windows.Forms;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ExportWordPress : IExportProcess
    {
        #region property string ExportType
        /// <summary>Text to appear in drop down list.</summary>
        public string ExportType
        {
            get
            {
                return "WordPress Alpha";
            }
        }
        #endregion property string ExportType

        #region bool Handle(string inputDataType)
        /// <summary>
        /// The calling program identifies the kind of data
        /// </summary>
        /// <param name="inputDataType">dictionary or scripture</param>
        /// <returns>true if this backend can handle the data</returns>
        public bool Handle(string inputDataType)
        {
            bool returnValue = false;
            string dataType = inputDataType.ToLower();
            if (dataType == "dictionary")
            {
                returnValue = true;
            }
            return returnValue;
        }
        #endregion bool Handle(string inputDataType)

        #region bool Export(PublicationInformation projInfo)
        /// <summary>
        /// Entry point for WordPress converter
        /// </summary>
        /// <param name="projInfo">values passed including xhtml and css names</param>
        /// <returns>true if succeeds</returns>
        public bool Export(PublicationInformation projInfo)
        {
            try
            {
                var xhtml = projInfo.DefaultXhtmlFileWithPath;
                const string prog = "WordPress.bat";
                var processFolder = Common.FromRegistry("WordPress");
                var progFullPath = Common.PathCombine(processFolder, prog);
                var args = string.Format(@"""{0}""", xhtml);
                SubProcess.Run(processFolder, progFullPath, args, true);
                if (projInfo.IsOpenOutput)
                {
                    string dataResult = Common.PathCombine(Path.GetDirectoryName(xhtml), "data.sql");
                    string msg = string.Format("Please import the file {0} to your WordPress MySql database. Would you like more details?", dataResult);
                    DialogResult dialogResult = MessageBox.Show(msg, "WordPress Export", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                        SubProcess.Run(processFolder, @"""WordPress site setup.txt""");
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion bool Export(PublicationInformation projInfo)
    }
}
