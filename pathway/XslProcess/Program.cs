using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using SIL.PublishingSolution;


namespace PSXslProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 4)
            {
                return;
            }
            XsltProcess xsltProcess = new XsltProcess();
            xsltProcess.XsltTransform(args[0], args[1], args[2], args[3]);
        }
    }
}
