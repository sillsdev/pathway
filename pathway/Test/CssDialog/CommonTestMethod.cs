// --------------------------------------------------------------------------------------------
// <copyright file="CommonTestMethod.cs" from='2009' to='2014' company='SIL International'>
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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Test.CssDialog
{
    static class CommonTestMethod
    {
        // Holds traceListners
        private static readonly IList traceListeners = new List<TraceListener>();

        public static void DisableDebugAsserts()
        {
            if (Debug.Listeners.Count <= 0)
                return;
            traceListeners.Clear();
            for (var i = Debug.Listeners.Count; i-- != 0; )
            {
                traceListeners.Add(Debug.Listeners[0]);
                Debug.Listeners.RemoveAt(0);
            }
        }

        public static void EnableDebugAsserts()
        {
            if (Debug.Listeners.Count > 0)
                return;
            for (var i = traceListeners.Count; i-- != 0; )
            {
                TraceListener traceListener = traceListeners[0] as TraceListener;
                Debug.Assert(traceListener != null);
                Debug.Listeners.Add(traceListener);
                traceListeners.RemoveAt(0);
            }
        }
    }
}