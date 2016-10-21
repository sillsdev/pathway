using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SIL.PublishingSolution
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmSplash objSplash = new frmSplash();
            objSplash.ShowDialog();
            if (objSplash.dr == DialogResult.OK)
            {
                Application.Run(new ConfigurationTool());
            }
        }
    }
}
