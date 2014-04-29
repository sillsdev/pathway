// --------------------------------------------------------------------------------------------
// <copyright file="ReflectionHelper.cs" from='2009' to='2014' company='SIL International'>
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

// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Diagnostics;

namespace SIL.Tool
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public class ReflectionHelperLite
    {
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the specified field (i.e. member variable) on the specified binding.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static void SetField(object binding, string fieldName, object args)
        {
            Invoke(binding, fieldName, new object[] { args }, BindingFlags.SetField);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the specified field (i.e. member variable) on the specified binding.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static object GetField(object binding, string fieldName)
        {
            return Invoke(binding, fieldName, null, BindingFlags.GetField);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the specified member variable or property (specified by name) on the
        /// specified binding.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private static object Invoke(object binding, string name, object[] args, BindingFlags flags)
        {
            // If binding is a Type then assume we're invoking a static method, property
            // or field. Otherwise invoke an instance method, property or field.
            flags |= (BindingFlags.NonPublic | BindingFlags.Public |
                      (binding is Type ? BindingFlags.Static : BindingFlags.Instance));

            // If necessary, go up the inheritance chain until the name
            // of the method, property or field is found.
            Type type = (binding is Type ? binding as Type : binding.GetType());
            while (type.GetMember(name, flags).Length == 0 && type.BaseType != null)
                type = type.BaseType;

            return type.InvokeMember(name, flags, null, binding, args);
        }
    }
}