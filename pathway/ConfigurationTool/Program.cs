using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SIL.PublishingSolution
{
    static class Program
    {
        private static readonly TraceSwitch TraceOn = new TraceSwitch("General", "Trace level for application");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Trace.Listeners.Add(new TextWriterTraceListener("ConfigurationToolTraceMessages.txt"));
            //Trace.AutoFlush = true;
            //Trace.Indent();
            //Trace.WriteLineIf(TraceOn.Level == TraceLevel.Verbose, "Entering Main");
            //string arg = string.Empty;
            //string media = string.Empty;
            //string sheet = string.Empty;
            //if(args.Length > 0)
            //{
            //    arg = args[0];
            //}
            //if (args.Length > 1)
            //{
            //    media = args[1];
            //}
            //if (args.Length > 2)
            //{
            //    sheet = args[2];
            //}
            //Trace.WriteLineIf(TraceOn.Level == TraceLevel.Verbose, "Main 1");
            //Application.EnableVisualStyles();
            //Trace.WriteLineIf(TraceOn.Level == TraceLevel.Verbose, "Main 2");
            //Application.SetCompatibleTextRenderingDefault(false);
            //Trace.WriteLineIf(TraceOn.Level == TraceLevel.Verbose, "Main 3");
            //Application.Run(new ConfigurationTool(arg, media, sheet));
            //Trace.WriteLineIf(TraceOn.Level == TraceLevel.Verbose, "Main 4");

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
