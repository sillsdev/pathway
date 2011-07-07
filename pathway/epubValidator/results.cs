// --------------------------------------------------------------------------------------
// <copyright file="results.cs" from='2010' to='2011' company='SIL International'>
//      Copyright © 2010, 2011 SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Erik Brommers</author>
// <email>erik_brommers@sil.org</email>
// Last reviewed:
// 
// <remarks>
// epub Validator. This program calls the open source epubcheck utility to validate
// the given epub file. epubcheck is hosted on Google Code at: 
// http://code.google.com/p/epubcheck/
// </remarks>
// --------------------------------------------------------------------------------------
using System;
using System.Windows.Forms;

namespace epubValidator
{
    public partial class frmResults : Form
    {
        public string ValidationResults;

        public frmResults()
        {
            InitializeComponent();
        }

        private void frmResults_Load(object sender, EventArgs e)
        {
            textBox1.Text = ValidationResults;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://code.google.com/p/epubcheck/wiki/Errors");
        }

        private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x1')
            {
                ((TextBox)sender).SelectAll();
                e.Handled = true;
            }
        }
    }
}
