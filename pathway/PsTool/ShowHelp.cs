// --------------------------------------------------------------------------------------------
// <copyright file="Json.cs" from='2009' to='2014' company='SIL International'>
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
// Create Show Help file
// </remarks>
// --------------------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace SIL.Tool
{
    public static class ShowHelp
    {
        public static HelpProvider HelpProv = new HelpProvider();

        public static void ShowHelpTopic(Control ctrl, string helpKeyword, bool isLinux, bool isKeyPress)
        {
            if (isLinux && isKeyPress)
            {
                helpKeyword = ModifySlashForLinuxProcess(helpKeyword);
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "chmsee";
                startInfo.Arguments = Common.PathwayHelpFileDirectory() + "::" + helpKeyword;
                Process.Start(startInfo);
            }
            else
            {
                HelpProv.HelpNamespace = Common.PathwayHelpFileDirectory();
                HelpProv.SetHelpNavigator(ctrl, HelpNavigator.Topic);
                HelpProv.SetHelpKeyword(ctrl, helpKeyword);
            }
        }

        public static void ShowHelpTopicKeyPress(Control ctrl, string helpKeyword, bool isLinux)
        {
            if (isLinux)
            {
                helpKeyword = ModifySlashForLinuxProcess(helpKeyword);
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "chmsee";
                startInfo.Arguments = Common.PathwayHelpFileDirectory() + "::" + helpKeyword;
                Process.Start(startInfo);
            }
            else
            {
                HelpProv.HelpNamespace = Common.PathwayHelpFileDirectory();
                HelpProv.SetHelpNavigator(ctrl, HelpNavigator.Topic);
                HelpProv.SetHelpKeyword(ctrl, helpKeyword);
                Help.ShowHelp(new Label(), HelpProv.HelpNamespace, HelpProv.GetHelpNavigator(ctrl), HelpProv.GetHelpKeyword(ctrl));
            }
        }

        private static string ModifySlashForLinuxProcess(string helpKeyword)
        {
            string returnValue = string.Empty;
            returnValue = helpKeyword.Replace(@"\", @"//");
            return returnValue;
        }
    }
}
