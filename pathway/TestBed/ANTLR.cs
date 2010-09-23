using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Antlr.Runtime.Tree;
using ANTLR_CSS;
using SIL.PublishingSolution;

namespace TestBed
{
    public partial class ANTLR : Form
    {
        public ANTLR()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            TreeNode _nodeTemp = new TreeNode();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = "CSS";
            dlg.Filter = "Cascading Style Sheet (*.css)|*.css";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string path = dlg.FileName;
                var ctp = new CssTreeParser();
                try
                {
                    ctp.Parse(path);
                }
                catch (Exception)
                {
                    throw;
                }
                CommonTree r = ctp.Root;
                _nodeTemp.Nodes.Clear();
                if (r.Text != "nil" && r.Text != null)
                {
                    _nodeTemp.Text = "nil";
                    AddSubTree(_nodeTemp, r, ctp);
                }
                else
                {
                    string rootNode = r.Text ?? "nil";
                    _nodeTemp.Text = rootNode;
                    foreach (CommonTree child in ctp.Children(r))
                    {
                        AddSubTree(_nodeTemp, child, ctp);
                    }
                }


                ////if (r.Text != "nil")
                ////{
                ////    _nodeTemp.Nodes.Add("nil");
                ////    AddSubTree(_nodeTemp, r, ctp);
                ////}
                ////else
                ////{
                //_nodeTemp.Text = r.Text;
                //if (r.Text != "nil" || r.Text == "")
                //{
                //    _nodeTemp.Text = "nil";
                //}
                //foreach (CommonTree child in ctp.Children(r))
                //{
                //    AddSubTree(_nodeTemp, child, ctp);
                //}
                ////}
            }
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add((TreeNode)_nodeTemp.Clone());
        }

        private static void AddSubTree(TreeNode n, CommonTree t, CssTreeParser ctp)
        {
            TreeNode nodeTemp = n.Nodes.Add(t.Text);
            foreach (CommonTree child in ctp.Children(t))
                AddSubTree(nodeTemp, child, ctp);
        }
    }
}
