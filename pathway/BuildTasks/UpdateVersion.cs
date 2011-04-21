// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2011, SIL International. All Rights Reserved.
// <copyright from='2011' to='2011' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: UpdateVersion.cs
// Responsibility: Trihus
// ---------------------------------------------------------------------------------------------
using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;

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

        #region Template
        private string _Template;
        [Required]
        public string Template
        {
            get { return _Template; }
            set { _Template = value; }
        }
        #endregion Template
        #endregion Properties

        public override bool Execute()
        {
            var instPath = Environment.CurrentDirectory;
            var sub = new Substitution { TargetPath = instPath };
            var map = new Dictionary<string, string>();
            map["Product"] = _product;
            map["PwVer"] = _version;
            sub.FileSubstitute(_Template, map);
            FileData.MoveToWix(_Template.Replace("-tpl", ""));
            return true;
        }
    }
}
