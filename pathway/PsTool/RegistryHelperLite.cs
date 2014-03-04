// --------------------------------------------------------------------------------------------
// <copyright file="RegistryHelperLite.cs" from='2010' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>TE Team</author>
// Last reviewed: 
// 
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace SIL.Tool
{
    public class RegistryHelperLite
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if a registry value exists.
		/// </summary>
		/// <param name="key">The base registry key of the key to check</param>
		/// <param name="subKey">Name of the group key, or string.Empty if there is no 
		/// groupKeyName.</param>
		/// <param name="regEntry">The name of the registry entry.</param>
		/// <param name="value">[out] value of the registry entry if it exists; null otherwise.</param>
		/// <returns><c>true</c> if the registry entry exists, otherwise <c>false</c></returns>
		/// ------------------------------------------------------------------------------------
		public static bool RegEntryExists(RegistryKey key, string subKey, string regEntry, out object value)
		{
			value = null;

            if (key == null)
                return false;

            if (string.IsNullOrEmpty(subKey))
			{
				value = key.GetValue(regEntry);
				if (value != null)
					return true;
				return false;
			}

			if (!KeyExists(key, subKey))
				return false;

			using (RegistryKey regSubKey = key.OpenSubKey(subKey))
			{
				Debug.Assert(regSubKey != null, "Should have caught this in the KeyExists call above");
				if (Array.IndexOf(regSubKey.GetValueNames(), regEntry) >= 0)
				{
					value = regSubKey.GetValue(regEntry);
					return true;
				}

				return false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if a registry key exists.
		/// </summary>
		/// <param name="key">The base registry key of the key to check</param>
		/// <param name="subKey">The key to check</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static bool KeyExists(RegistryKey key, string subKey)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			foreach (string s in key.GetSubKeyNames())
			{
				if (String.Compare(s, subKey, true) == 0)
					return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the registry key for the current application's company from the local machine
		/// settings. This is 'HKLM\Software\{Application.CompanyName}'
		/// NOTE: This key is not opened for write access because it will fail on 
		/// non-administrator logins.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static RegistryKey CompanyKeyLocalMachine
		{
			get
			{
				RegistryKey softwareKey = Registry.LocalMachine.OpenSubKey("Software");
				Debug.Assert(softwareKey != null);
				var silKey = softwareKey.OpenSubKey("SIL");
                if (silKey == null)
                    silKey = softwareKey.OpenSubKey(@"Wow6432Node\SIL");
			    return silKey;
			}
		}
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the registry key for the current application's company from the local machine
        /// settings. This is 'HKLM\Software\{Application.CompanyName}'
        /// NOTE: This key is not opened for write access because it will fail on 
        /// non-administrator logins.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static RegistryKey CompanyKeyCurrentUser
        {
            get
            {
                RegistryKey softwareKey = Registry.CurrentUser.OpenSubKey("Software");
                Debug.Assert(softwareKey != null);
                var silKey = softwareKey.OpenSubKey("SIL");
                if (silKey == null)
                    silKey = softwareKey.OpenSubKey(@"Wow6432Node\SIL");
                return silKey;
            }
        }
        /// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the registry key for the Paratext application from the local machine
		/// settings. This is 'HKLM\Software\ScrChecks\1.0'
		/// NOTE: This key is not opened for write access because it will fail on 
		/// non-administrator logins.
		/// </summary>
		/// ------------------------------------------------------------------------------------
        public static RegistryKey ParatextKey
        {
            get
            {
                RegistryKey softwareKey = Registry.LocalMachine.OpenSubKey("software");
                var scrChecksKey = softwareKey.OpenSubKey(@"ScrChecks\1.0");
                if (scrChecksKey == null)
                    scrChecksKey = softwareKey.OpenSubKey(@"Wow6432Node\ScrChecks\1.0");
                return scrChecksKey;
            }
        }
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the registry key for theWord application.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static RegistryKey TheWordKey
        {
            get
            {
                return Registry.ClassesRoot.OpenSubKey("theWordModule");
            }
        }
    }
}
