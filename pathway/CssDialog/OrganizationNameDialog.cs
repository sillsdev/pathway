using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SIL.PublishingSolution
{
    public partial class OrganizationNameDialog : Form
    {
        public string Organization
        {
            get
            {
                string newText = txtOrganization.Text.Replace("<", "");
                newText = newText.Replace(">", "");
                return newText.Trim();
            }
            set { txtOrganization.Text = value; }
        }

        public OrganizationNameDialog()
        {
            InitializeComponent();
        }

        private void OrganizationNameDialog_Load(object sender, EventArgs e)
        {
            // set focus on the Organization field
            txtOrganization.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Close out with an OK result
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Close out with a Cancel result
            DialogResult = DialogResult.Cancel;
            Close();
        }

    }
}
