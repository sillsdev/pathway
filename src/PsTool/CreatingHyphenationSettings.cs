using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

namespace SIL.Tool
{
    public class CreatingHyphenationSettings
    {
        public static void CreateHyphenationFile(string exportType)
        {
            if(string.IsNullOrEmpty(Common.databaseName))
                return;

            string hyphenxmlfile = Common.PathCombine(Common.GetAllUserPath(),exportType);
            hyphenxmlfile = Common.PathCombine(hyphenxmlfile, "Hyphenation.xml");
            if (File.Exists(hyphenxmlfile))
            {
                var xdoc=new XmlDocument();
                xdoc.Load(hyphenxmlfile);
                string xpath = "//projname[@id=\"" + Common.databaseName + "\"]";
                XmlNode xexistprojnode = xdoc.SelectSingleNode(xpath);
                if (xexistprojnode != null)
                {
                    if (xexistprojnode.Attributes != null)
                    {
                        xexistprojnode.Attributes["enable"].Value = Param.HyphenEnable.ToString();
                        xexistprojnode.Attributes["languages"].Value = Param.HyphenLang;
                    }
                    xdoc.Save(hyphenxmlfile);
                }
                else
                {
                    const string xrootpath = "*";
                    XmlNode xrootNode = xdoc.SelectSingleNode(xrootpath);
                    WriteProjectNode(xdoc, xrootNode);
                    xdoc.Save(hyphenxmlfile);
                }
            }
            else
            {
				var folder = Path.GetDirectoryName(hyphenxmlfile);
				if (!string.IsNullOrEmpty(folder) && !Directory.Exists(folder)) Directory.CreateDirectory(folder);
				var xdoc =new XmlDocument();
                XmlNode xrootNode = xdoc.CreateElement("hyphen");
                xdoc.AppendChild(xrootNode);
                WriteProjectNode(xdoc, xrootNode);
                xdoc.Save(hyphenxmlfile);
            }
        }

        private static void WriteProjectNode(XmlDocument xdoc, XmlNode xrootNode)
        {
            XmlNode xprojNode = xdoc.CreateElement("projname");
            XmlAttribute xprojAttr = xdoc.CreateAttribute("id");
            xprojAttr.Value = Common.databaseName;
            if (xprojNode.Attributes != null)
            {
                xprojNode.Attributes.Append(xprojAttr);
                xrootNode.AppendChild(xprojNode);

                XmlAttribute xcheckAttr = xdoc.CreateAttribute("enable");
                xcheckAttr.Value = Param.HyphenEnable.ToString();
                xprojNode.Attributes.Append(xcheckAttr);

                XmlAttribute xlangAttr = xdoc.CreateAttribute("languages");
                xlangAttr.Value = Param.HyphenLang;
                xprojNode.Attributes.Append(xlangAttr);
            }
        }

        public static void ReadHyphenationSettings(string projName, string exportType)
        {
            Param.HyphenEnable = false;
            if (string.IsNullOrEmpty(projName))
                return;

            string hyphenxmlfile = Common.PathCombine(Common.GetAllUserPath(), exportType);
            hyphenxmlfile = Common.PathCombine(hyphenxmlfile, "Hyphenation.xml");
            if (File.Exists(hyphenxmlfile))
            {
                var xDoc = new XmlDocument();
                xDoc.Load(hyphenxmlfile);
                string xPath = "//projname[@id=\"" + projName + "\"]";
                XmlNode xExistProjnode = xDoc.SelectSingleNode(xPath);
                if (xExistProjnode != null)
                {
                    if (xExistProjnode.Attributes != null)
                    {
                        Param.HyphenEnable = (Param.IsHyphen) && Convert.ToBoolean(xExistProjnode.Attributes["enable"].Value);
                    }
                }
            }
        }
    }
}
