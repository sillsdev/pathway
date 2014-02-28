// --------------------------------------------------------------------------------------------
// <copyright file="CheckUserFileAccessRights.cs" from='2009' to='2014' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;

namespace SIL.Tool
{
    public class CheckUserFileAccessRights
    {

        private string _path;
        private System.Security.Principal.WindowsIdentity _principal;

        private bool _denyAppendData = false;
        private bool _denyChangePermissions = false;
        private bool _denyCreateDirectories = false;
        private bool _denyCreateFiles = false;
        private bool _denyDelete = false;
        private bool _denyDeleteSubdirectoriesAndFiles = false;
        private bool _denyExecuteFile = false;
        private bool _denyFullControl = false;
        private bool _denyListDirectory = false;
        private bool _denyModify = false;
        private bool _denyRead = false;
        private bool _denyReadAndExecute = false;
        private bool _denyReadAttributes = false;
        private bool _denyReadData = false;
        private bool _denyReadExtendedAttributes = false;
        private bool _denyReadPermissions = false;
        private bool _denySynchronize = false;
        private bool _denyTakeOwnership = false;
        private bool _denyTraverse = false;
        private bool _denyWrite = false;
        private bool _denyWriteAttributes = false;
        private bool _denyWriteData = false;
        private bool _denyWriteExtendedAttributes = false;

        private bool _allowAppendData = false;
        private bool _allowChangePermissions = false;
        private bool _allowCreateDirectories = false;
        private bool _allowCreateFiles = false;
        private bool _allowDelete = false;
        private bool _allowDeleteSubdirectoriesAndFiles = false;
        private bool _allowExecuteFile = false;
        private bool _allowFullControl = false;
        private bool _allowListDirectory = false;
        private bool _allowModify = false;
        private bool _allowRead = false;
        private bool _allowReadAndExecute = false;
        private bool _allowReadAttributes = false;
        private bool _allowReadData = false;
        private bool _allowReadExtendedAttributes = false;
        private bool _allowReadPermissions = false;
        private bool _allowSynchronize = false;
        private bool _allowTakeOwnership = false;
        private bool _allowTraverse = false;
        private bool _allowWrite = false;
        private bool _allowWriteAttributes = false;
        private bool _allowWriteData = false;
        private bool _allowWriteExtendedAttributes = false;

        public bool canAppendData() { return !_denyAppendData && _allowAppendData; }
        public bool canChangePermissions()
        { return !_denyChangePermissions && _allowChangePermissions; }
        public bool canCreateDirectories()
        { return !_denyCreateDirectories && _allowCreateDirectories; }
        public bool canCreateFiles() { return !_denyCreateFiles && _allowCreateFiles; }
        public bool canDelete() { return !_denyDelete && _allowDelete; }
        public bool canDeleteSubdirectoriesAndFiles()
        {
            return !_denyDeleteSubdirectoriesAndFiles &&
              _allowDeleteSubdirectoriesAndFiles;
        }
        public bool canExecuteFile() { return !_denyExecuteFile && _allowExecuteFile; }
        public bool canFullControl() { return !_denyFullControl && _allowFullControl; }
        public bool canListDirectory()
        { return !_denyListDirectory && _allowListDirectory; }
        public bool canModify() { return !_denyModify && _allowModify; }
        public bool canRead() { return !_denyRead && _allowRead; }
        public bool canReadAndExecute()
        { return !_denyReadAndExecute && _allowReadAndExecute; }
        public bool canReadAttributes()
        { return !_denyReadAttributes && _allowReadAttributes; }
        public bool canReadData() { return !_denyReadData && _allowReadData; }
        public bool canReadExtendedAttributes()
        {
            return !_denyReadExtendedAttributes &&
              _allowReadExtendedAttributes;
        }
        public bool canReadPermissions()
        { return !_denyReadPermissions && _allowReadPermissions; }
        public bool canSynchronize() { return !_denySynchronize && _allowSynchronize; }
        public bool canTakeOwnership()
        { return !_denyTakeOwnership && _allowTakeOwnership; }
        public bool canTraverse() { return !_denyTraverse && _allowTraverse; }
        public bool canWrite() { return !_denyWrite && _allowWrite; }
        public bool canWriteAttributes()
        { return !_denyWriteAttributes && _allowWriteAttributes; }
        public bool canWriteData() { return !_denyWriteData && _allowWriteData; }
        public bool canWriteExtendedAttributes()
        {
            return !_denyWriteExtendedAttributes &&
              _allowWriteExtendedAttributes;
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Simple accessor
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><returns></returns></span>
        public System.Security.Principal.WindowsIdentity getWindowsIdentity()
        {
            return _principal;
        }
        /// <span class="code-SummaryComment"><summary></span>
        /// Simple accessor
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><returns></returns></span>
        public String getPath()
        {
            return _path;
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Convenience constructor assumes the current user
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="path"></param></span>
        public CheckUserFileAccessRights(string path) :
            this(path, System.Security.Principal.WindowsIdentity.GetCurrent()) { }


        /// <span class="code-SummaryComment"><summary></span>
        /// Supply the path to the file or directory and a user or group. 
        /// Access checks are done
        /// during instantiation to ensure we always have a valid object
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="path"></param></span>
        /// <span class="code-SummaryComment"><param name="principal"></param></span>
        public CheckUserFileAccessRights(string path,
            System.Security.Principal.WindowsIdentity principal)
        {
            this._path = path;
            this._principal = principal;

            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(_path);
                AuthorizationRuleCollection acl = fi.GetAccessControl().GetAccessRules
                            (true, true, typeof(SecurityIdentifier));
                for (int i = 0; i < acl.Count; i++)
                {
                    System.Security.AccessControl.FileSystemAccessRule rule =
                           (System.Security.AccessControl.FileSystemAccessRule)acl[i];
                    if (_principal.User.Equals(rule.IdentityReference))
                    {
                        if (System.Security.AccessControl.AccessControlType.Deny.Equals
                                (rule.AccessControlType))
                        {
                            if (contains(FileSystemRights.AppendData, rule))
                                _denyAppendData = true;
                            if (contains(FileSystemRights.ChangePermissions, rule))
                                _denyChangePermissions = true;
                            if (contains(FileSystemRights.CreateDirectories, rule))
                                _denyCreateDirectories = true;
                            if (contains(FileSystemRights.CreateFiles, rule))
                                _denyCreateFiles = true;
                            if (contains(FileSystemRights.Delete, rule))
                                _denyDelete = true;
                            if (contains(FileSystemRights.DeleteSubdirectoriesAndFiles,
                                   rule)) _denyDeleteSubdirectoriesAndFiles = true;
                            if (contains(FileSystemRights.ExecuteFile, rule))
                                _denyExecuteFile = true;
                            if (contains(FileSystemRights.FullControl, rule))
                                _denyFullControl = true;
                            if (contains(FileSystemRights.ListDirectory, rule))
                                _denyListDirectory = true;
                            if (contains(FileSystemRights.Modify, rule))
                                _denyModify = true;
                            if (contains(FileSystemRights.Read, rule)) _denyRead = true;
                            if (contains(FileSystemRights.ReadAndExecute, rule))
                                _denyReadAndExecute = true;
                            if (contains(FileSystemRights.ReadAttributes, rule))
                                _denyReadAttributes = true;
                            if (contains(FileSystemRights.ReadData, rule))
                                _denyReadData = true;
                            if (contains(FileSystemRights.ReadExtendedAttributes, rule))
                                _denyReadExtendedAttributes = true;
                            if (contains(FileSystemRights.ReadPermissions, rule))
                                _denyReadPermissions = true;
                            if (contains(FileSystemRights.Synchronize, rule))
                                _denySynchronize = true;
                            if (contains(FileSystemRights.TakeOwnership, rule))
                                _denyTakeOwnership = true;
                            if (contains(FileSystemRights.Traverse, rule))
                                _denyTraverse = true;
                            if (contains(FileSystemRights.Write, rule)) _denyWrite = true;
                            if (contains(FileSystemRights.WriteAttributes, rule))
                                _denyWriteAttributes = true;
                            if (contains(FileSystemRights.WriteData, rule))
                                _denyWriteData = true;
                            if (contains(FileSystemRights.WriteExtendedAttributes, rule))
                                _denyWriteExtendedAttributes = true;
                        }
                        else if (System.Security.AccessControl.AccessControlType.
                                 Allow.Equals(rule.AccessControlType))
                        {
                            if (contains(FileSystemRights.AppendData, rule))
                                _allowAppendData = true;
                            if (contains(FileSystemRights.ChangePermissions, rule))
                                _allowChangePermissions = true;
                            if (contains(FileSystemRights.CreateDirectories, rule))
                                _allowCreateDirectories = true;
                            if (contains(FileSystemRights.CreateFiles, rule))
                                _allowCreateFiles = true;
                            if (contains(FileSystemRights.Delete, rule))
                                _allowDelete = true;
                            if (contains(FileSystemRights.DeleteSubdirectoriesAndFiles,
                                  rule)) _allowDeleteSubdirectoriesAndFiles = true;
                            if (contains(FileSystemRights.ExecuteFile, rule))
                                _allowExecuteFile = true;
                            if (contains(FileSystemRights.FullControl, rule))
                                _allowFullControl = true;
                            if (contains(FileSystemRights.ListDirectory, rule))
                                _allowListDirectory = true;
                            if (contains(FileSystemRights.Modify, rule))
                                _allowModify = true;
                            if (contains(FileSystemRights.Read, rule)) _allowRead = true;
                            if (contains(FileSystemRights.ReadAndExecute, rule))
                                _allowReadAndExecute = true;
                            if (contains(FileSystemRights.ReadAttributes, rule))
                                _allowReadAttributes = true;
                            if (contains(FileSystemRights.ReadData, rule))
                                _allowReadData = true;
                            if (contains(FileSystemRights.ReadExtendedAttributes, rule))
                                _allowReadExtendedAttributes = true;
                            if (contains(FileSystemRights.ReadPermissions, rule))
                                _allowReadPermissions = true;
                            if (contains(FileSystemRights.Synchronize, rule))
                                _allowSynchronize = true;
                            if (contains(FileSystemRights.TakeOwnership, rule))
                                _allowTakeOwnership = true;
                            if (contains(FileSystemRights.Traverse, rule))
                                _allowTraverse = true;
                            if (contains(FileSystemRights.Write, rule))
                                _allowWrite = true;
                            if (contains(FileSystemRights.WriteAttributes, rule))
                                _allowWriteAttributes = true;
                            if (contains(FileSystemRights.WriteData, rule))
                                _allowWriteData = true;
                            if (contains(FileSystemRights.WriteExtendedAttributes, rule))
                                _allowWriteExtendedAttributes = true;
                        }
                    }
                }

                IdentityReferenceCollection groups = _principal.Groups;
                for (int j = 0; j < groups.Count; j++)
                {
                    for (int i = 0; i < acl.Count; i++)
                    {
                        System.Security.AccessControl.FileSystemAccessRule rule =
                            (System.Security.AccessControl.FileSystemAccessRule)acl[i];
                        if (groups[j].Equals(rule.IdentityReference))
                        {
                            if (System.Security.AccessControl.AccessControlType.
                                Deny.Equals(rule.AccessControlType))
                            {
                                if (contains(FileSystemRights.AppendData, rule))
                                    _denyAppendData = true;
                                if (contains(FileSystemRights.ChangePermissions, rule))
                                    _denyChangePermissions = true;
                                if (contains(FileSystemRights.CreateDirectories, rule))
                                    _denyCreateDirectories = true;
                                if (contains(FileSystemRights.CreateFiles, rule))
                                    _denyCreateFiles = true;
                                if (contains(FileSystemRights.Delete, rule))
                                    _denyDelete = true;
                                if (contains(FileSystemRights.
                                    DeleteSubdirectoriesAndFiles, rule))
                                    _denyDeleteSubdirectoriesAndFiles = true;
                                if (contains(FileSystemRights.ExecuteFile, rule))
                                    _denyExecuteFile = true;
                                if (contains(FileSystemRights.FullControl, rule))
                                    _denyFullControl = true;
                                if (contains(FileSystemRights.ListDirectory, rule))
                                    _denyListDirectory = true;
                                if (contains(FileSystemRights.Modify, rule))
                                    _denyModify = true;
                                if (contains(FileSystemRights.Read, rule))
                                    _denyRead = true;
                                if (contains(FileSystemRights.ReadAndExecute, rule))
                                    _denyReadAndExecute = true;
                                if (contains(FileSystemRights.ReadAttributes, rule))
                                    _denyReadAttributes = true;
                                if (contains(FileSystemRights.ReadData, rule))
                                    _denyReadData = true;
                                if (contains(FileSystemRights.
                                        ReadExtendedAttributes, rule))
                                    _denyReadExtendedAttributes = true;
                                if (contains(FileSystemRights.ReadPermissions, rule))
                                    _denyReadPermissions = true;
                                if (contains(FileSystemRights.Synchronize, rule))
                                    _denySynchronize = true;
                                if (contains(FileSystemRights.TakeOwnership, rule))
                                    _denyTakeOwnership = true;
                                if (contains(FileSystemRights.Traverse, rule))
                                    _denyTraverse = true;
                                if (contains(FileSystemRights.Write, rule))
                                    _denyWrite = true;
                                if (contains(FileSystemRights.WriteAttributes, rule))
                                    _denyWriteAttributes = true;
                                if (contains(FileSystemRights.WriteData, rule))
                                    _denyWriteData = true;
                                if (contains(FileSystemRights.
                                        WriteExtendedAttributes, rule))
                                    _denyWriteExtendedAttributes = true;
                            }
                            else if (System.Security.AccessControl.AccessControlType.
                                   Allow.Equals(rule.AccessControlType))
                            {
                                if (contains(FileSystemRights.AppendData, rule))
                                    _allowAppendData = true;
                                if (contains(FileSystemRights.ChangePermissions, rule))
                                    _allowChangePermissions = true;
                                if (contains(FileSystemRights.CreateDirectories, rule))
                                    _allowCreateDirectories = true;
                                if (contains(FileSystemRights.CreateFiles, rule))
                                    _allowCreateFiles = true;
                                if (contains(FileSystemRights.Delete, rule))
                                    _allowDelete = true;
                                if (contains(FileSystemRights.
                                    DeleteSubdirectoriesAndFiles, rule))
                                    _allowDeleteSubdirectoriesAndFiles = true;
                                if (contains(FileSystemRights.ExecuteFile, rule))
                                    _allowExecuteFile = true;
                                if (contains(FileSystemRights.FullControl, rule))
                                    _allowFullControl = true;
                                if (contains(FileSystemRights.ListDirectory, rule))
                                    _allowListDirectory = true;
                                if (contains(FileSystemRights.Modify, rule))
                                    _allowModify = true;
                                if (contains(FileSystemRights.Read, rule))
                                    _allowRead = true;
                                if (contains(FileSystemRights.ReadAndExecute, rule))
                                    _allowReadAndExecute = true;
                                if (contains(FileSystemRights.ReadAttributes, rule))
                                    _allowReadAttributes = true;
                                if (contains(FileSystemRights.ReadData, rule))
                                    _allowReadData = true;
                                if (contains(FileSystemRights.
                                    ReadExtendedAttributes, rule))
                                    _allowReadExtendedAttributes = true;
                                if (contains(FileSystemRights.ReadPermissions, rule))
                                    _allowReadPermissions = true;
                                if (contains(FileSystemRights.Synchronize, rule))
                                    _allowSynchronize = true;
                                if (contains(FileSystemRights.TakeOwnership, rule))
                                    _allowTakeOwnership = true;
                                if (contains(FileSystemRights.Traverse, rule))
                                    _allowTraverse = true;
                                if (contains(FileSystemRights.Write, rule))
                                    _allowWrite = true;
                                if (contains(FileSystemRights.WriteAttributes, rule))
                                    _allowWriteAttributes = true;
                                if (contains(FileSystemRights.WriteData, rule))
                                    _allowWriteData = true;
                                if (contains(FileSystemRights.WriteExtendedAttributes,
                                    rule)) _allowWriteExtendedAttributes = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Deal with IO exceptions if you want
                //throw e;
                AccessDenied();
            }
        }

        private void AccessDenied()
        {
            _denyAppendData = false;
            _denyChangePermissions = false;
            _denyCreateDirectories = false;
            _denyCreateFiles = false;
            _denyDelete = false;
            _denyDeleteSubdirectoriesAndFiles = false;
            _denyExecuteFile = false;
            _denyFullControl = false;
            _denyListDirectory = false;
            _denyModify = false;
            _denyRead = false;
            _denyReadAndExecute = false;
            _denyReadAttributes = false;
            _denyReadData = false;
            _denyReadExtendedAttributes = false;
            _denyReadPermissions = false;
            _denySynchronize = false;
            _denyTakeOwnership = false;
            _denyTraverse = false;
            _denyWrite = false;
            _denyWriteAttributes = false;
            _denyWriteData = false;
            _denyWriteExtendedAttributes = false;
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Simply displays all allowed rights
        /// 
        /// Useful if say you want to test for write access and find
        /// it is false;
        /// <span class="code-SummaryComment"><xmp></span>
        /// UserFileAccessRights rights = new UserFileAccessRights(txtLogPath.Text);
        /// System.IO.FileInfo fi = new System.IO.FileInfo(txtLogPath.Text);
        /// if (rights.canWrite() && rights.canRead()) {
        ///     lblLogMsg.Text = "R/W access";
        /// } else {
        ///     if (rights.canWrite()) {
        ///        lblLogMsg.Text = "Only Write access";
        ///     } else if (rights.canRead()) {
        ///         lblLogMsg.Text = "Only Read access";
        ///     } else {
        ///         lblLogMsg.CssClass = "error";
        ///         lblLogMsg.Text = rights.ToString()
        ///     }
        /// }
        /// 
        /// <span class="code-SummaryComment"></xmp></span>
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><returns></returns></span>
        public override String ToString()
        {
            string str = "";

            if (canAppendData())
            {
                if (!String.IsNullOrEmpty(str)) str +=
","; str += "AppendData";
            }
            if (canChangePermissions())
            {
                if (!String.IsNullOrEmpty(str)) str +=
","; str += "ChangePermissions";
            }
            if (canCreateDirectories())
            {
                if (!String.IsNullOrEmpty(str)) str +=
","; str += "CreateDirectories";
            }
            if (canCreateFiles())
            {
                if (!String.IsNullOrEmpty(str)) str +=
","; str += "CreateFiles";
            }
            if (canDelete())
            {
                if (!String.IsNullOrEmpty(str)) str +=
   ","; str += "Delete";
            }
            if (canDeleteSubdirectoriesAndFiles())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "DeleteSubdirectoriesAndFiles";
            }
            if (canExecuteFile())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "ExecuteFile";
            }
            if (canFullControl())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "FullControl";
            }
            if (canListDirectory())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "ListDirectory";
            }
            if (canModify())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "Modify";
            }
            if (canRead())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "Read";
            }
            if (canReadAndExecute())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "ReadAndExecute";
            }
            if (canReadAttributes())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "ReadAttributes";
            }
            if (canReadData())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "ReadData";
            }
            if (canReadExtendedAttributes())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "ReadExtendedAttributes";
            }
            if (canReadPermissions())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "ReadPermissions";
            }
            if (canSynchronize())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "Synchronize";
            }
            if (canTakeOwnership())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "TakeOwnership";
            }
            if (canTraverse())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "Traverse";
            }
            if (canWrite())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "Write";
            }
            if (canWriteAttributes())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "WriteAttributes";
            }
            if (canWriteData())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "WriteData";
            }
            if (canWriteExtendedAttributes())
            {
                if (!String.IsNullOrEmpty(str))
                    str += ","; str += "WriteExtendedAttributes";
            }
            if (String.IsNullOrEmpty(str))
                str = "None";
            return str;
        }

        /// <span class="code-SummaryComment"><summary></span>
        /// Convenience method to test if the right exists within the given rights
        /// <span class="code-SummaryComment"></summary></span>
        /// <span class="code-SummaryComment"><param name="right"></param></span>
        /// <span class="code-SummaryComment"><param name="rule"></param></span>
        /// <span class="code-SummaryComment"><returns></returns></span>
        public bool contains(System.Security.AccessControl.FileSystemRights right,
            System.Security.AccessControl.FileSystemAccessRule rule)
        {
            return (((int)right & (int)rule.FileSystemRights) == (int)right);
        }
    }
}