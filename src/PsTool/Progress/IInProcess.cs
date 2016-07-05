// --------------------------------------------------------------------------------------------
// <copyright file="IInProcess.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.   
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

using System.Windows.Forms;

namespace SIL.PublishingSolution
{
    public interface IInProcess
    {
        ProgressBar Bar();

        void AddToMaximum(int n);

        void PerformStep();
    }
}