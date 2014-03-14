// --------------------------------------------------------------------------------------------
// <copyright file="RegistryAccess.cs" from='2009' to='2014' company='SIL International'>
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
// Simple access to file contents
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Diagnostics;
using Microsoft.Win32;

namespace PdfLicense
{
    /// <summary>
    /// A class for managing registry access.
    /// </summary>
    public class RegistryAccess
    {
        private const string SOFTWARE_KEY = "Software";

        /// <summary>ProductName - Name of program for registry</summary>
        //private static string _productName = Application.ProductName;
        private static string _productName = "Pathway";

        public static string ProductName
        {
            set
            {
                Debug.Assert(string.IsNullOrEmpty(value));
                _productName = value;
            }
            get
            {
                return _productName;
            }
        }

        /// --------------------------------------------------------------------------------
        /// <summary>
        /// Method for retrieving a Registry Value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        /// --------------------------------------------------------------------------------
        static public string GetStringRegistryValue(string key, string defaultValue)
        {
            return (string)GetRegistryValue(key, defaultValue);
        }

        /// --------------------------------------------------------------------------------
        /// <summary>
        /// Method for retrieving a Registry Value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        /// --------------------------------------------------------------------------------
        static public int GetIntRegistryValue(string key, int defaultValue)
        {
            return (int)GetRegistryValue(key, defaultValue);
        }

        /// --------------------------------------------------------------------------------
        /// <summary>
        /// Method for retrieving a Registry Value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        /// --------------------------------------------------------------------------------
        static private object GetRegistryValue(string key, object defaultValue)
        {
            RegistryKey rkCompany;
            RegistryKey rkApplication;
            // The generic Company Name is SIL International, but in the registry we want this
            // to use SIL. If we want to keep a generic approach, we probably need another member
            // variable
            // for ShortCompanyName, or something similar.
            //rkCompany = Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY, false).OpenSubKey(Application.CompanyName, false);
            try
            {
                rkCompany = Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY, false).OpenSubKey("SIL", false);
            }
            catch (System.Exception)
            {
                rkCompany = null;
            }
            if (rkCompany != null)
            {
                rkApplication = rkCompany.OpenSubKey(ProductName, false);
                if (rkApplication != null)
                {
                    foreach (string sKey in rkApplication.GetValueNames())
                    {
                        if (sKey == key)
                        {
                            return rkApplication.GetValue(sKey);
                        }
                    }
                }
            }
            return defaultValue;
        }

        /// --------------------------------------------------------------------------------
        /// <summary>
        /// Method for storing a Registry Value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="stringValue">The string value.</param>
        /// --------------------------------------------------------------------------------
        static public void SetStringRegistryValue(string key, string stringValue)
        {
            SetRegistryValue(key, stringValue);
        }

        /// --------------------------------------------------------------------------------
        /// <summary>
        /// Method for storing a Registry Value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The value.</param>
        /// --------------------------------------------------------------------------------
        static public void SetIntRegistryValue(string key, int val)
        {
            SetRegistryValue(key, val);
        }

        private static void SetRegistryValue(string key, object val)
        {
            RegistryKey rkSoftware;
            RegistryKey rkCompany;
            RegistryKey rkApplication;

            rkSoftware = Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY, true);
            // The generic Company Name is SIL International, but in the registry we want this to use
            // SIL. If we want to keep a generic approach, we probably need another member variable
            // for ShortCompanyName, or something similar.
            try
            {
                rkCompany = rkSoftware.CreateSubKey("SIL");
            }
            catch (System.Exception)
            {
                rkCompany = null;
            }
            if (rkCompany != null)
            {
                rkApplication = rkCompany.CreateSubKey(ProductName);
                if (rkApplication != null)
                {
                    rkApplication.SetValue(key, val);
                }
            }

        }
    }
}
