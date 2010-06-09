// --------------------------------------------------------------------------------------------
// <copyright file="PopupForm.cs" from='2009' to='2009' company='SIL International'>
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
// Popup Form used in Dictionary Settings.
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Windows.Forms;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    /// <summary
    /// Popup Form used in Dictionary Settings.
    /// </summary>
    public partial class PopupForm : Form
    {
        /// <summary>
        /// Returns the SectionName to Parent Form
        /// </summary>
        public string SectionName;

        /// <summary>
        /// Returns the FileName to Parent Form
        /// </summary>
        public string FileName;

        /// <summary>
        /// Returns the filter for OpenDialog
        /// </summary>
        private string filter;

        /// <summary>
        /// Stores the section/step Name
        /// </summary>
        private string sName;

        /// <summary>
        /// Stores the File Name
        /// </summary>
        private string sFileName;

        /// <summary>
        /// Stores the Addition/Edit option value
        /// </summary>
        private bool add;

        /// <summary>
        /// Stores the value of arriving from section/step 
        /// </summary>
        private bool fromSection;

        /// <summary>
        /// Popupform stores the Initial values to local variables.
        /// </summary>
        /// <param name="sectionName">Section Name </param>
        /// <param name="fileName"> File Name</param>
        /// <param name="addition">Add/Edit</param>
        /// <param name="filter">File filter</param>
        /// <param name="fromSection">From section or step</param>
        public PopupForm(string sectionName, string fileName, bool addition, string filter, bool fromSection)
        {
            InitializeComponent();
            sName = sectionName;
            sFileName = fileName;
            this.filter = filter;
            add = addition;
            this.fromSection = fromSection;
        }

        #region Events
        /// <summary>
        /// Ok Click to return to parent form
        /// </summary>
        /// <param name="sender"> sender object </param>
        /// <param name="e">e Event Argument</param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (txtReturnName.Text.Trim() == string.Empty)
            {
                //MessageBox.Show("Please Type the Section/Step Name");
                txtReturnName.Focus();
            }
            else
            {
                SectionName = txtReturnName.Text;
            }
            FileName = txtReturnFileName.Text;
        }

        /// <summary>
        /// Cancel Click - return to parent form without file names
        /// </summary>
        /// <param name="sender"> sender object</param>
        /// <param name="e">e Event</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"> sender object </param>
        /// <param name="e">e Event Argument</param>
        private void PopupForm_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);     // Form Controls
            txtReturnFileName.Text = sFileName;
            txtReturnName.Text = sName;
            if (!add)
            {
                txtReturnName.ReadOnly = true;
                lblNote.Visible = false;
            }
            
            if (fromSection)
            {
                Text = lblName.Text = "Section ";
            }
            else
            {
                Text = lblName.Text = "Step ";
                lblNote.Visible = false;
            }
        }

        /// <summary>
        /// File Open for Browse
        /// </summary>
        /// <param name="sender"> sender object </param>
        /// <param name="e">e Event Argument</param>
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Choose file name";
            ofd.Filter = "*." + filter + "|*." + filter;
 
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtReturnFileName.Text = ofd.FileName;
            }
        }
        #endregion Events

        private void PopupForm_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void PopupForm_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }
    }
}
