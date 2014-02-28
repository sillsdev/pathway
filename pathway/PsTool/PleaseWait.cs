// --------------------------------------------------------------------------------------------
// <copyright file="PleaseWait.cs" from='2009' to='2014' company='SIL International'>
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
using System.Windows.Forms;

namespace SIL.Tool
{
    public partial class PleaseWait : Form
    {
        public PleaseWait()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Opacity = this.Opacity - 0.1;
            if (this.Opacity < 0.5)
            {
                this.Close();
            }
        }
    }
}
