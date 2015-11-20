using System;
using System.Windows.Forms;

namespace PrincessConvert
{
    public partial class AboutBox1 : Form
    {
        public AboutBox1()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutBox1_Load(object sender, EventArgs e)
        {
            string ApplicationTitle;
            ApplicationTitle = Main.sProgramName;
            Text = Main.sProgramName;
        //' Initialize all of the text displayed on the About Box.
        //' TODO: Customize the application's assembly information in the "Application" pane of the project 
        //'    properties dialog (under the "Project" menu).
            LabelProductName.Text = Main.sProgramName;
            LabelVersion.Text = "Version: " + Main.sProgramVersionFull;
            LabelCopyright.Text = Main.sProgramCopyright;
            LabelCompanyName.Text = Main.sProgramCopyrightHoldersName;
            TextBoxDescription.Text = Main.sProgramDescription;

        }
    }
}
