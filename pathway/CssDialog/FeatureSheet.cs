// --------------------------------------------------------------------------------------------
// <copyright file="FeatureSheet.cs" from='2009' to='2014' company='SIL International'>
//      Copyright (C) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Stylepick FeatureSheet
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class FeatureSheet
    {
        public string Sheet { get; set; }
        public List<string> Features { get; set; }
        private string _data = string.Empty;

        public FeatureSheet()
        {
            Features = new List<string>();
        }

        public FeatureSheet(string sheet)
        {
            Features = new List<string>();
            Sheet = sheet;
        }

        /// <summary>
        /// To read style sheet upto End
        /// </summary>
        /// <returns></returns>
        public bool ReadToEnd()
        {
            Debug.Assert(!string.IsNullOrEmpty(Sheet) || Sheet.Length <= 0, "Style Sheet not set");
            if (Sheet.Length <= 0 || !File.Exists(Param.StylePath(Sheet))) return false;
            TextReader textReader = new StreamReader(Param.StylePath(Sheet));
            _data = textReader.ReadToEnd();
            textReader.Close();
            var ms = Regex.Matches(_data, "@import \"(.*)\"");
            if (_data.Contains("@page")) return false;
            foreach (Match m in ms)
            {
                var snippet = m.Groups[1].Value.ToLower();
				if (File.Exists(Common.FromRegistry(Common.PathCombine(Param.Value[Param.MasterSheetPath], snippet))) || 
                    File.Exists(Common.PathCombine(Param.Value[Param.OutputPath], snippet)))
                    Features.Add(snippet);
            }
            return true;
        }

        /// <summary>
        /// To save features
        /// </summary>
        /// <param name="tv"></param>
        public void SaveFeatures(TreeView tv)
        {
            Debug.Assert(Param.Value.ContainsKey(Param.InputType), "When Saving Features, settings not loaded!");
            Features.Clear();
            if (_data.Contains(Param.Value[Param.InputType]))
                Features.Add(Param.Value[Param.InputType] + ".css");
            foreach (TreeNode tn in tv.Nodes)
            {
                foreach (TreeNode node in tn.Nodes)
                {
                    if (!node.Checked) continue;
                    if (Param.GetTagFile((XmlNode)node.Tag) != "")
                        Features.Add(Param.GetTagFile((XmlNode)node.Tag));
                    break;
                }
            }
        }

        /// <summary>
        /// To write style sheet
        /// </summary>
        /// <returns></returns>
        public long Write()
        {
            Debug.Assert(!string.IsNullOrEmpty(Sheet), "Style Sheet not set");
            TextWriter tw = new StreamWriter(Sheet);
            long len = 0;
            foreach (var s in Features)
            {
                string line = string.Format("@import \"{0}\";\r\n", s);
                len += line.Length;
                tw.Write(line);
            }
            tw.Close();
            return len;
        }
    }
}
