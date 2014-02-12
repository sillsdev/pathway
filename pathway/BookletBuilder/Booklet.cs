// --------------------------------------------------------------------------------------------
// <copyright file="Booklet.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Create Booklet builder
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SIL.PublishingSolution

{
    public partial class Booklet : Form
    {
        public Booklet()
        {
            InitializeComponent();
        }

        private BookletBL bookletBL = new BookletBL();

        private void Booklet_Load(object sender, EventArgs e)
        {

        }

        private void openSavedSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bookletBL.OpenSavedSetting(lstSection);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bookletBL.ShowAbout();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            bookletBL.MoveUp(lstSection);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            bookletBL.MoveDown(lstSection);
        }

        private void openDefaultSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bookletBL.OpenDefaultSetting(lstSection);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bookletBL.Exit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bookletBL.AddSection(lstSection, txtSectName.Text);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
           bookletBL.RemoveSection(lstSection);
        }

        private void saveSettingsAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bookletBL.SaveAsSetting(lstSection);
        }
    }
}
