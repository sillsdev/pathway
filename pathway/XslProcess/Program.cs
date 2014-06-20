using System;
using System.Diagnostics;
using SIL.PublishingSolution;


namespace PSXslProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 4)
                {
                    throw new ArgumentException("XslProcess: Wrong number of arguments");
                }
                XsltProcess xsltProcess = new XsltProcess();
                xsltProcess.XsltTransform(args[0], args[1], args[2], args[3]);
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
                Environment.ExitCode = -1;
            }
        }
    }
}
