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
