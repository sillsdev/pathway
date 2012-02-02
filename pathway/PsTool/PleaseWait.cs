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
