// --------------------------------------------------------------------------------------------
// <copyright file="Form1.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Displays the results of parsing a css file.
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using SIL.PublishingSolution;

namespace TestBed
{
    /// <summary>
    /// Displays the results of parsing a css file.
    /// </summary>
    public partial class frmTreeView : Form
    {
        /// <summary>
        /// Results are stored here
        /// </summary>
        TreeNode resultNode = new TreeNode();

        /// <summary>
        /// Constructor to initialize form
        /// </summary>
        public frmTreeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Browse button allows user to enter file and then produces results.
        /// </summary>
        /// <param name="sender">originator of message</param>
        /// <param name="e">arguments for message</param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = "CSS";
            dlg.Filter = "Cascading Style Sheet (*.css)|*.css";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var clsBL = new CSSParser();
                TreeNode node = clsBL.BuildTree(dlg.FileName);
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add((TreeNode)node.Clone());

                textBox1.Text = clsBL.ErrorText;

                CSSinput.Text = dlg.FileName;
            }
        }

        private void frmTreeView_DoubleClick(object sender, EventArgs e)
        {
            FlexPluginTest test = new FlexPluginTest();
            test.ShowDialog();
        }

        private StreamWriter sw;
        private void BtSave_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog
                          {
                              DefaultExt = "css",
                              Filter = "Cascading Style Sheet (*.css)|*.css",
                              OverwritePrompt = true,
                              AddExtension = true
                          };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            sw = new StreamWriter(dlg.FileName);
            foreach (TreeNode node in treeView1.Nodes[0].Nodes)
                WriteNode(node, "");
            sw.Close();
            var res = MessageBox.Show("Open File?", "Saved", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res != DialogResult.Yes) return;
            Process.Start(dlg.FileName);
        }
        
        private void WriteNode(TreeNode node, string leader)
        {
            const string INDENT = "    ";
            switch (node.Text)
            {
                case "PAGE":
                    sw.Write(string.Format("{0}@page ", leader));
                    var idx = 0;
                    if (node.Nodes[0].Text == "PSEUDO")
                    {
                        sw.Write(string.Format(":{0} ", node.Nodes[0].Nodes[0].Text));
                        idx++;
                    }
                    sw.WriteLine("{");
                    for (; idx < node.Nodes.Count; idx++)
                        WriteNode(node.Nodes[idx], leader + INDENT);
                    if (leader.Length > 0)
                        sw.Write(leader);
                    sw.WriteLine("}");
                    break;
                case "REGION":
                    sw.WriteLine(string.Format("{0}@{1} {2}", leader, node.Nodes[0].Text, "{"));
                    for (var k = 1; k < node.Nodes.Count; k++ )
                        WriteNode(node.Nodes[k], leader + INDENT);
                    if (leader.Length > 0)
                        sw.Write(leader);
                    sw.WriteLine("}");
                    break;
                case "MEDIA":
                    sw.WriteLine(string.Format("{0}@media {1} {2}", leader, node.Nodes[0].Text, "{"));
                    for (var k = 1; k < node.Nodes.Count; k++)
                        WriteNode(node.Nodes[k], leader + INDENT);
                    if (leader.Length > 0)
                        sw.Write(leader);
                    sw.WriteLine("}");
                    break;
                case "PROPERTY":
                    sw.WriteLine(WriteProperty(leader, node));
                    break;
                case "ATTRIB":
                    sw.Write("[");
                    foreach (TreeNode child in node.Nodes)
                        if (child.Text == "ATTRIBEQUAL")
                            sw.Write("=");
                        else
                            sw.Write(child.Text);
                    sw.Write("] ");
                    break;
                case "RULE":
                    var begun = false;
                    var props = new List<string>();
                    foreach (TreeNode child in node.Nodes)
                    {
                        switch (child.Text)
                        {
                            case "CLASS":
                                sw.Write(string.Format("{0}.{1} ", leader, child.Nodes[0].Text));
                                for (var i = 1; i < child.Nodes.Count; i++ )
                                    WriteNode(child.Nodes[i], leader);
                                break;
                            case "TAG":
                                sw.Write(string.Format("{0}{1} ", leader, child.Nodes[0].Text));
                                for (var i = 1; i < child.Nodes.Count; i++)
                                    WriteNode(child.Nodes[i], leader);
                                break;
                            case "PARENTOF":
                                sw.Write("> ");
                                break;
                            case "PRECEDES":
                                sw.Write("+ ");
                                break;
                            case "ANY":
                                sw.Write("* ");
                                break;
                            case "PSEUDO":
                                sw.Write(string.Format(":{0} ", child.Nodes[0].Text));
                                break;
                            case "PROPERTY":
                                if (!begun)
                                {
                                    sw.WriteLine("{");
                                    begun = true;
                                }
                                props.Add(WriteProperty(leader + INDENT, child));
                                break;
                            default:
                                sw.Write(string.Format("{0} ", child.Text));
                                break;
                        }
                    }
                    SortedProps(props);
                    if (leader.Length > 0)
                        sw.Write(leader);
                    if (!begun)
                        sw.Write("{ ");
                    sw.WriteLine("}");
                    break;
            }
        }

        private readonly List<string> noSpace = new List<string> {"(", ")", "%","px","cm","mm","in","pt","pc","em","ex","deg","rad","grad","ms","s","hz","khz"};
        private string WriteProperty(string leader, TreeNode node)
        {
            var line = string.Empty;
            var pName = node.Nodes[0].Text;
            line += string.Format("{0}   {1}:", leader, pName);
            if (pName == "content" && node.Nodes.Count == 2 && node.Nodes[1].Text != "''" && node.Nodes[1].Text != "none")
                line += string.Format(" \"{0}\"", node.Nodes[1].Text);
            else
                for (var idx = 1; idx < node.Nodes.Count; idx++ )
                {
                    var str = node.Nodes[idx].Text;
                    if (!noSpace.Contains(str))
                        line +=" ";
                    line += str;
                }
            line += ";";
            return line;
        }

        private void SortedProps(IEnumerable<string> props)
        {
            var propName = new List<string>();
            var propNode = new Dictionary<string, string>();
            foreach (var itm in props)
            {
                var pName = PropName(itm);
                if (!propName.Contains(pName))
                    propName.Add(pName);
                propNode[pName] = itm;
            }
            propName.Sort();
            foreach (var prop in propName)
                sw.WriteLine(propNode[prop]);
        }

        private static string PropName(string itm)
        {
            var line = itm.Trim();
            var i = line.IndexOf(':');
            return line.Substring(0, i);
        }
    }
}