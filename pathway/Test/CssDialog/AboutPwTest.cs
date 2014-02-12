// --------------------------------------------------------------------------------------------
// <copyright file="AboutPwTest.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
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

using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;
using NUnit.Framework;
using SIL.PublishingSolution;

namespace Test.CssDialog
{
    public class AboutPwTest : AboutPw
    {
        [Test]
        public void HelpImproveGetValueWindows7Test()
        {
            const string keyName = "SOFTWARE\\SIL\\Pathway";
            const string valueName = "HelpImprove";
            var cbHelpImprove = new CheckBox();
            RegistryKey subKey = Registry.CurrentUser.OpenSubKey(keyName);
            string curValue = null;
            if (subKey != null)
            {
                curValue = (string)subKey.GetValue(valueName);
                subKey.Close();
            }
            SetStringKeyValue(keyName, valueName, "Yes");
            HelpImproveGetValue(cbHelpImprove);
            Assert.True(cbHelpImprove.Checked);
            SetStringKeyValue(keyName, valueName, "No");
            HelpImproveGetValue(cbHelpImprove);
            Assert.False(cbHelpImprove.Checked);
            if (curValue == null)
            {
                Registry.CurrentUser.DeleteSubKey(keyName);
            }
            else
            {
                SetStringKeyValue(keyName, valueName, curValue);
            }
        }

        private static void SetStringKeyValue(string subKeyName, string valueName, string value)
        {
            RegistryKey subKey = Registry.CurrentUser.CreateSubKey(subKeyName);
            Debug.Assert(subKey != null);
            subKey.SetValue(valueName, value);
            subKey.Close();
        }
    }
}
