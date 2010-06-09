// --------------------------------------------------------------------------------------------
// <copyright file="ViewCssTree.cs" from='2009' to='2009' company='SIL International'>
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
// View the Tree of Css class and properties
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SIL.DictionaryExpress.CSSParser;

namespace SIL.DictionaryExpress
{
    #region Class ViewCSSTree
    public partial class ViewCSSTree : Form
    {
        #region Constructor
        public ViewCSSTree()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void btnLoadCss_Click(object sender, EventArgs e)
        {
            string strCSSFileName;
            OpenFileDialog dlgOpenCSS = new OpenFileDialog();
            dlgOpenCSS.DefaultExt = "CSS";
            dlgOpenCSS.Filter = "Cascading Style Sheet (*.css)|*.css";
            if (dlgOpenCSS.ShowDialog() == DialogResult.OK)
            {
                strCSSFileName = dlgOpenCSS.FileName;
            }
            else
            {
                MessageBox.Show("Please select a valid CSS File Name");
                return;
            }

            SIL.DictionaryExpress.CSSParser.CSSParser clsBL = new SIL.DictionaryExpress.CSSParser.CSSParser();
            TreeNode node = clsBL.BuildTree(dlgOpenCSS.FileName);
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add((TreeNode)node.Clone());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
    #endregion
}
