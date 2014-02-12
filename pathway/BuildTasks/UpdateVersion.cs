// --------------------------------------------------------------------------------------------
// <copyright file="UpdateVersion.cs" from='2009' to='2014' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BuildTasks
{
    public class UpdateVersion : Task
    {
        #region Properties
        #region Product
        private string _product;
        [Required]
        public string Product
        {
            get { return _product; }
            set { _product = value; }
        }
        #endregion Product

        #region Version
        private string _version;
        [Required]
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }
        #endregion Version

        #region FieldworksVersion
        private string _fieldworksVersion;
        public string FieldworksVersion
        {
            get { return _fieldworksVersion; }
            set { _fieldworksVersion = value; }
        }
        #endregion FieldworksVersion

        #region BuildVersion
        private string _buildVersion;
        public string BuildVersion
        {
            get { return _buildVersion; }
            set { _buildVersion = value; }
        }
        #endregion BuildVersion

        #region Template
        private string _template;
        [Required]
        public string Template
        {
            get { return _template; }
            set { _template = value; }
        }
        #endregion Template

        #region HelpFile
        private string _helpFile;
        [Required]
        public string HelpFile
        {
            get { return _helpFile; }
            set { _helpFile = value; }
        }
        #endregion HelpFile
        #endregion Properties

        public override bool Execute()
        {
            var instPath = Environment.CurrentDirectory;
            var sub = new Substitution { TargetPath = instPath };
            var map = new Dictionary<string, string>();
            map["PwVer"] = _version;
            if (!string.IsNullOrEmpty(_buildVersion))
            {
                var exp = new Regex(@"([0-9]+\.[0-9]+\.[0-9]+)\.[0-9]+");
                var match = exp.Match(_buildVersion);
                if (match.Success)
                    map["PwVer"] = match.Groups[1].Value;
            }
            if (!string.IsNullOrEmpty(_fieldworksVersion))
                map["FwVer"] = _fieldworksVersion;
            map["Product"] = _product;
            map["HelpFile"] = _helpFile;
            sub.FileSubstitute(_template, map);
            FileData.MoveToWix(_template.Replace("-tpl", ""));
            return true;
        }
    }
}
