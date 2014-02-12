// --------------------------------------------------------------------------------------------
// <copyright file="PreviewConfig.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class PreviewConfig : Form
    {
        private string previewFileName1 = string.Empty;
        private string previewFileName2 = string.Empty;

        public PreviewConfig()
        {
            InitializeComponent();
        }

        public PreviewConfig(string file1,string file2 )
        {
            InitializeComponent();
            previewFileName1 = file1;
            previewFileName2 = file2;
        }

        private void PreviewConfig_Load(object sender, EventArgs e)
        {
            int height = this.Height;
            int width = this.Width;
            int top = 3;
            pictureBox1.Size = new Size(width - 70, height);
            pictureBox1.Location = new Point(1, top);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            if (File.Exists(previewFileName1))
            pictureBox1.Image = Image.FromFile(previewFileName1);

            top = height + 25;
            pictureBox2.Size = new Size(width - 70, height);
            pictureBox2.Location = new Point(1, top);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            if (File.Exists(previewFileName2))
            pictureBox2.Image = Image.FromFile(previewFileName2);
        }
    }
}
