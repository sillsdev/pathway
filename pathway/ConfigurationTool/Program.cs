using System;
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
            string arg = string.Empty;
            string media = string.Empty;
            string sheet = string.Empty;
            if(args.Length > 0)
            {
                arg = args[0];
            }
            if (args.Length > 1)
            {
                media = args[1];
            }
            if (args.Length > 2)
            {
                sheet = args[2];
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConfigurationTool(arg, media, sheet));
        }
    }
}
