// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2008, SIL International. All Rights Reserved.
// <copyright from='2003' to='2008' company='SIL International'>
//		Copyright (c) 2008, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// File: ReflectionHelper.cs
// Responsibility: TE Team
// 
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
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
        /// Dynamically find an assembly and create an object of the nameed class using a public
        /// constructor.
        /// </summary>
        /// <param name="assemblyName">Relative to the location of the reflection helper assemptly</param>
        /// <param name="className1"> fully qualified!!</param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// ------------------------------------------------------------------------------------
        static public Object CreateObject(string assemblyName, string className1, object[] args)
        {
            return CreateObject(assemblyName, className1, BindingFlags.Public, args);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Dynamically find an assembly and create an object of the nameed class.
        /// </summary>
        /// <param name="assemblyName">Relative to the location of the reflection helper assemptly</param>
        /// <param name="className1">fully qualified!!</param>
        /// <param name="addlBindingFlags">The additional binding flags which can be used to
        /// indicate whether to look for a public or non-public constructor, etc.
        /// (BindingFlags.Instance is always included).</param>
        /// <param name="args">The arguments to the constructor.</param>
        /// <returns></returns>
        /// ------------------------------------------------------------------------------------
        static public Object CreateObject(string assemblyName, string className1,
                                          BindingFlags addlBindingFlags, object[] args)
        {
            Assembly assembly;
            string location = Assembly.GetExecutingAssembly().Location;
            string assemblyPath = Common.PathCombine(Path.GetDirectoryName(location), assemblyName);
            try
            {
                assembly = Assembly.LoadFrom(assemblyPath);
            }
            catch
            {
                // That didn't work, so try the bare DLL name and let the OS try to find it. This
                // is needed for tests because each DLL gets shadow-copied in its own temp folder.
                assemblyPath = assemblyName;
                assembly = Assembly.LoadFrom(assemblyPath);
            }

            string className = className1.Trim();
            Object thing = null;
            try
            {
                //make the object
                //Object thing = assembly.CreateInstance(className);
                thing = assembly.CreateInstance(className, false,
                                                BindingFlags.Instance | addlBindingFlags, null, args, null, null);
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                string message = CouldNotCreateObjectMsg(assemblyPath, className);

                Exception inner = err;

                while (inner != null)
                {
                    message += "\r\nInner exception message = " + inner.Message;
                    inner = inner.InnerException;
                }
                throw new ConfigurationErrorsException(message);
            }

            if (thing == null)
            {
                // Bizarrely, CreateInstance is not specified to throw an exception if it can't
                // find the specified class. But we want one.
                throw new ConfigurationErrorsException(CouldNotCreateObjectMsg(assemblyPath, className));
            }
            return thing;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// ------------------------------------------------------------------------------------
        static string CouldNotCreateObjectMsg(string assemblyPath, string className)
        {
            return "ReflectionHelper found the DLL " + assemblyPath	+
                   " but could not create the class: "	+ className +
                   ". If there are no 'InnerExceptions' below, then make sure capitalization is correct and that you include the name space (e.g. XCore.Ticker).";
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Calls a method specified on the specified binding.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static void CallMethod(object binding, string methodName, params object[] args)
        {
            GetResult(binding, methodName, args);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the result of calling a method on the specified binding.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static object GetResult(object binding, string methodName, params object[] args)
        {
            return Invoke(binding, methodName, args, BindingFlags.InvokeMethod);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the specified property on the specified binding.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static void SetProperty(object binding, string propertyName, object args)
        {
            Invoke(binding, propertyName, new object[] { args }, BindingFlags.SetProperty);
        }

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
        /// Gets the specified property on the specified binding.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        public static object GetProperty(object binding, string propertyName)
        {
            return Invoke(binding, propertyName, null, BindingFlags.GetProperty);
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