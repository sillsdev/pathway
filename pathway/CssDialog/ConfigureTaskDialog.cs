using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public partial class ConfigureTaskDialog : Form
    {
        private readonly string _buttonType = "Add";
        private readonly string _value = "Add";
        public string[] prevValues;
        public string Task;
        public ConfigureTaskDialog(string buttonType, string value)
        {
            _value = value;
            _buttonType = buttonType;
            InitializeComponent();
        }

        private void ConfigureTaskDialog_Load(object sender, EventArgs e)
        {
            LoadStyleName();
            if (_buttonType == "Modify")
            {
                txtTaskName.Text = _value;
                CmbStyleName.SelectedItem = Param.GetAttrByName("tasks/task", _value, "style");
                prevValues = new[] { txtTaskName.Text, CmbStyleName.SelectedItem.ToString() };
            }
        }

        private void LoadStyleName()
        {
            CmbStyleName.Items.Clear();
            var styleList = Param.GetListofAttr("styles/paper/style", "name");
            CmbStyleName.Items.AddRange(Param.FilterStyles(styleList).ToArray());
        }

        private void ValidateTaskName(string newName, string xPath)
        {
            List<string> styleList = Param.GetListofAttr(xPath, "name");
            if (styleList.Contains(newName))
            {
                if (_buttonType == "Modify")
                {
                    Param.RemoveXMLNode(Param.SettingOutputPath,
                                        "/stylePick/tasks/task[@name='" + prevValues[0] + "']");
                    AddNewTaskName();
                }
                else
                {
                    LocDB.Message("errFileAlreadyExitsNoOverwrite", "File Already Exists, Please give the new name.",
                  null, LocDB.MessageTypes.Info, LocDB.MessageDefault.First);
                    txtTaskName.Text = "";
                    txtTaskName.Focus();
                }
            }
            else
            {
                AddNewTaskName();
            }
        }

        private void AddNewTaskName()
        {
            var task = Param.InsertKind("task", txtTaskName.Text);
            string styleName = CmbStyleName.Text;
            if (CmbStyleName.SelectedItem != null)
            {
                styleName = CmbStyleName.SelectedItem.ToString();
            }
            Param.SetAttrValue(task, "style", styleName);
            Param.SetAttrValue(task, "icon", "Graphic/userTask.png");
            Param.Write();
        }

        private void ReplaceXMLNode(string xPath, string[] values)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(Param.SettingOutputPath);
            XmlNode matchnode = null;
            XmlNodeList nodeList = xDoc.GetElementsByTagName(xPath);
            if (_buttonType == "Modify")
            {
                foreach (XmlNode o in nodeList)
                {
                    XmlAttributeCollection attr = o.Attributes;
                    if (attr[0].Value == values[0] || attr[1].Value == values[1])
                    {
                        matchnode = o;
                        break;
                    }
                }
            }
            XmlElement newElement = xDoc.CreateElement(xPath);
            newElement.Attributes.Append(xDoc.CreateAttribute("name")).InnerText = txtTaskName.Text;
            newElement.Attributes.Append(xDoc.CreateAttribute("style")).InnerText = CmbStyleName.SelectedItem.ToString();
            if(_buttonType == "Add")
            {
                nodeList.Item(nodeList.Count).AppendChild(newElement);
            }
            else
            {
                if (matchnode != null) matchnode.ParentNode.ReplaceChild(newElement, matchnode);    
            }
            xDoc.Save(Param.SettingOutputPath);
            Param.LoadSettings();
        }

        private void BtOk_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                string xPath = "tasks/task";
                ValidateTaskName(txtTaskName.Text, xPath);
                Task = txtTaskName.Text;
                Close();
            }
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtTaskName.Text))
            {
                txtTaskName.Focus();
                return false;
            }
            if (CmbStyleName.SelectedIndex < 0)
            {
                CmbStyleName.Focus();
                return false;
            }

            
            if (_buttonType == "Modify")
            {
                if (prevValues[0] == txtTaskName.Text && prevValues[1] == CmbStyleName.SelectedItem.ToString())
                {
                    //return false;
                    this.Close();
                }
            }
            return true;
        }
    }
}
