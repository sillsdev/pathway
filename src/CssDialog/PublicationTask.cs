// --------------------------------------------------------------------------------------------
// <copyright file="PublicationTask.cs" from='2009' to='2014' company='SIL International'>
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
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public partial class PublicationTask : Form
    {
        #region private variable
        private string _currentTask = string.Empty;
        private Color _deSelectedColor = Color.LightSteelBlue;
        private Color _selectedColor = Color.FromArgb(255, 204, 102);  
        private Color _borderColor = Color.Black;
        private Color _mouseOverColor = Color.Khaki;
        #endregion

        #region public variable
        public string cssFile;
        public bool ExcerptPreview;
        #endregion

        public string Css
        {
            get
            {
                return Param.StylePath(Param.TaskSheet(_currentTask));
            }
        }

        public string InputPath { get; set; }

        public string CurrentInput { get; set; }

        public string InputType { get; set; }

        public PublicationTask()
        {
            InitializeComponent();
        }


        /// <summary>
        /// To load default values
        /// </summary>
        public void DoLoad()
        {
            if (Param.Value.ContainsKey(Param.InputType))
                Param.SetValue(Param.InputType, InputType);
            Param.LoadSettings();
            Param.SetValue(Param.InputPath, InputPath);
            Param.SetValue(Param.CurrentInput, CurrentInput);
            Param.SetValue(Param.InputType, InputType);

            _currentTask = Param.Value[Param.LastTask];
            if (_currentTask.Length == 0)
                _currentTask = "Final print";
            Param.GetAttrByName("tasks/task", _currentTask, "style");
            if(!Common.Testing)
                SetTask(_currentTask);
            Param.LoadSettings();
        }


        private void BtOK_Click(object sender, EventArgs e)
        {
            DoAccept();
            Close();
        }
        /// <summary>
        /// To accept the changes
        /// </summary>
        public void DoAccept()
        {
            string selectedStyle = Param.TaskSheet(_currentTask);

			cssFile = Common.FromRegistry(Param.StylePath(Param.StyleFile[selectedStyle]));
            if (Param.Value[Param.LastTask] != _currentTask)
            {
                Param.SetValue(Param.LastTask, _currentTask);
                Param.Write();
            }
            DialogResult = DialogResult.OK;
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtPreview_Click(object sender, EventArgs e)
        {
            var dlg = new Preview { Sheet = Param.TaskSheet(_currentTask), ParentForm = this };
            dlg.Show();
        }


        private void SetDescription(string focus)
        {
            var sheet = Param.GetAttrByName("tasks/task", focus, "style");
            lblTaskDesc.Text = Param.GetElemByName("styles/paper/style", sheet, "Description");
            lblTaskDesc.Visible = true;
        }

        private void ShowConfigure(string role)
        {
            BtConfigure.Visible = role != "Output User";
        }

        private void PublicationTask_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new JWTools.Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void PublicationTask_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }

        private void PublicationTask_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);
            Param.SetupHelp(this);
            try
            {
                if (Param.Value.ContainsKey(Param.InputType))
                    Param.SetValue(Param.InputType, "");
                Param.LoadSettings();
                ScreenAdjustment();
                BtConfigure.Visible = Param.UserRole != "Output User";

                if (Param.Value.ContainsKey(Param.InputType))
                    Param.SetValue(Param.InputType, InputType);
                Param.LoadSettings();
                TaskAdjustment();

                DoLoad();
            }
            catch (InvalidStyleSettingsException err)
            {
                var msg = new[] { err.FullFilePath };
                LocDB.Message("errNotValidXml", err.ToString(), msg, LocDB.MessageTypes.Warning, LocDB.MessageDefault.First);
                return;
            }

        }
        private void ScreenAdjustment()
        {
            List<string> roles = Param.GetListofAttr("roles/role", "name");
            List<string> roleIcon = Param.GetListofAttr("roles/role", "icon");
            ImageList imageList = new ImageList { ImageSize = new Size(32, 32) }; 
            try
            {
                foreach (var iconName in roleIcon)
                {
					var icon = new Bitmap(Common.FromRegistry(iconName));
                    imageList.Images.Add(icon);
                }
            }
            catch
            {
            } 

            int height = 58;
            int locationY = 36;
            for (int i = 0; i < roles.Count; i++)
            {
                Button button = new Button()
                                    {
                                        Name = roles[i],
                                        Text = roles[i],
                                        Image = imageList.Images[i]
                                    };
                button.Size = new Size(175, height);
                button.Location =  new Point(0,locationY);
                button.ImageAlign = ContentAlignment.TopCenter;
                button.TextAlign = ContentAlignment.BottomCenter;


                button.FlatAppearance.BorderColor = _borderColor;
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.CheckedBackColor = _selectedColor;
                button.FlatAppearance.MouseDownBackColor = _selectedColor;
                button.FlatAppearance.MouseOverBackColor = _mouseOverColor;
                button.FlatStyle = FlatStyle.Flat;
                button.UseVisualStyleBackColor = true;

                locationY += height;
                button.Click += this.Role_Click;
                PanelRole.Controls.Add(button);

            }
            PanelTask.Dock = DockStyle.Fill;
            PanelRole.Dock = DockStyle.Fill;
            PanelShow(1);

       }

        private void TaskAdjustment()
        {
            //Task 
            List<string> tasks = Param.GetListofAttr("tasks/task", "name");
            List<string> taskIcon = Param.GetListofAttr("tasks/task", "icon");
            ImageList imageListTask = new ImageList { ImageSize = new Size(32, 32) };
            Bitmap icon;
            foreach (var iconName in taskIcon)
            {
            try
            {
				icon = new Bitmap(Common.FromRegistry(iconName));
                }
                catch (Exception)
                {
					icon = new Bitmap(Common.FromRegistry("Graphic/userTask.png"));
                }
                    imageListTask.Images.Add(icon);
                }


            int height = 58;
            int locationY = 36;
            for (int i = 0; i < tasks.Count; i++)
            {
                Button button = new Button()
                {
                    Name = tasks[i],
                    Text = tasks[i],
                };

                if (imageListTask.Images.Count > i)
                    button.Image = imageListTask.Images[i];

                button.Size = new Size(175, height);
                button.Location = new Point(0, locationY);
                button.ImageAlign = ContentAlignment.TopCenter;
                button.TextAlign = ContentAlignment.BottomCenter;


                button.FlatAppearance.BorderColor = _borderColor;
                button.FlatAppearance.BorderSize = 0;
                button.FlatAppearance.CheckedBackColor = _selectedColor;
                button.FlatAppearance.MouseDownBackColor = _selectedColor;
                button.FlatAppearance.MouseOverBackColor = _mouseOverColor;
                button.FlatStyle = FlatStyle.Flat;
                button.UseVisualStyleBackColor = true;

                locationY += height;
                button.Click += this.Task_Click;
                button.MouseEnter += this.Task_MouseEnter;
                button.MouseLeave += this.Task_MouseLeave;
                PanelTask.Controls.Add(button);
            }
       }

        private void Role_Click(object sender, EventArgs e)
        {
            Button bt = (Button) sender;
            Param.UserRole = bt.Text;
            ShowConfigure(bt.Text);
            foreach(Control control in PanelRole.Controls)
            {
                if (control is Button)
                {
                    Button role = (Button) control;
                    if (role.Text == bt.Text)
                    {
                        role.BackColor = _selectedColor;
                    }
                    else
                    {
                        role.BackColor = _deSelectedColor;
                    }
                }
            }
        }

        private void Task_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            SetTask(bt.Text);
        }

        private void SetTask(string selectedTask)
        {
            foreach (Control control in PanelTask.Controls)
            {
                if (control is Button)
                {
                    Button task = (Button)control;
                    if (task.Text == selectedTask)
                    {
                        task.BackColor = _selectedColor;
                    }
                    else
                    {
                        task.BackColor = _deSelectedColor;
                    }
                }
            }
            _currentTask = selectedTask;
            ShowPreivew();
        }

        private void PanelShow(int index)
        {
            PanelTask.Visible = false;
            PanelRole.Visible = false;

            BtnTask.BackColor = _deSelectedColor;
            BtnRole.BackColor = _deSelectedColor;
            if (index == 1)
            {
                PanelTask.Visible = true;
                BtnTask.BackColor = _selectedColor;
            }
            else if (index == 2)
            {
                PanelRole.Visible = true;
                BtnRole.BackColor = _selectedColor;
            }
        }

        private void BtnTask_Click(object sender, EventArgs e)
        {
            PanelShow(1);
        }

        private void BtnRole_Click(object sender, EventArgs e)
        {
            PanelShow(2);
        }

        private void ShowPreivew()
        {

            foreach (Control s in this.Controls)
            {
                if (s is WebBrowser && s.Name == "webPreview")
                {
                    s.Dispose();
                    break;
                }
            }

            WebBrowser webPreview = new WebBrowser
                                        {
                                            Name = "webPreview",
                                            AccessibleName = "webPreview",
                                            Location = new Point(185, 3),
                                            Size = new Size(554, 516),
                                            TabIndex = 40
                                        };
            this.Controls.Add(webPreview);

            Param.SetValue(Param.CurrentInput, CurrentInput);
            Preview preview = new Preview {};
            preview.Sheet = Param.TaskSheet(_currentTask);
            preview.ParentForm = this;
            string previewPdfFile = preview.CreatePreview();
            if (previewPdfFile != string.Empty)
            {
                webPreview.Navigate(previewPdfFile);
                webPreview.Visible = true;
            }
        }

        private void Task_MouseEnter(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            SetDescription(bt.Text);
        }


        private void Task_MouseLeave(object sender, EventArgs e)
        {
            SetDescription(_currentTask);
        }
   }
}
