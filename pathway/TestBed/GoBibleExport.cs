using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestBed
{
    public partial class GoBibleExport : Form
    {
        public GoBibleExport()
        {
            InitializeComponent();
        }

        private void GoBibleExport_Load(object sender, EventArgs e)
        {
            ddlFiles.Items.Add("One");
            ddlFiles.Items.Add("One per Book");
            ddlFiles.SelectedIndex = 0;

            ddlRedLetter.Items.Add("Yes");
            ddlRedLetter.Items.Add("No");
            ddlRedLetter.SelectedIndex = 0;

            txtInformation.Text = "Bible text export from";
        }
    }
}
