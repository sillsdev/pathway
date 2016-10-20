// --------------------------------------------------------------------------------------------
// <copyright file="SelectOrganizationDialog.cs" from='2009' to='2014' company='SIL International'>
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
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public partial class SelectOrganizationDialog : Form
    {
        private static string _helpTopic = string.Empty;
        private XmlNodeList _organizations;
        private string _inputType;

        public string Organization
        {
            get { return ddlOrganization.Text; }
            set { ddlOrganization.Text = value; }
        }

        public SelectOrganizationDialog(string inputType)
        {
            _inputType = inputType;
			Common.SetupLocalization();
            InitializeComponent();
            _helpTopic = "User_Interface/Dialog_boxes/Select_Your_Organization_dialog_box.htm";
        }

        private void btnOther_Click(object sender, EventArgs e)
        {
            var dlg = new OrganizationNameDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string organizationName = dlg.Organization;
                if (organizationName.Trim().Length == 0)
                {
                    ddlOrganization.SelectedIndex = 0;
                    return;
                }
                // add the organization if it's not already in the list
                if (!ddlOrganization.Items.Contains(organizationName))
                {
                    // not in our list - add it and select it
                    ddlOrganization.SelectedIndex = ddlOrganization.Items.Add(dlg.Organization);
                }
                else
                {
                    // already in our list - just select it
                    Organization = organizationName;
                }
            }
        }

        /// <summary>
        /// Displays the Select Organization dialog.
        /// This dialog is designed to only come up once, when the user first runs Pathway. The
        /// Organization value in this case is assumed to be empty, since they haven't specified
        /// their organization yet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectOrganizationDialog_Load(object sender, EventArgs e)
        {
            // Load User Interface Collection Parameters
            Param.LoadSettings();
	        _organizations = Param.GetItems("//stylePick/Organizations/Organization");
            foreach (var org in _organizations)
            {
                var node = (XmlNode)org;
                if (node.FirstChild == null) continue;
                if ((_inputType == "Dictionary") && (node.FirstChild.Value.Contains("Bible")))
                {
                    // UBS, PBT - don't display in SE version
                    continue;
                }
                ddlOrganization.Items.Add(node.FirstChild.Value);
            }
            // select the first item in the list
            if (ddlOrganization.Items.Count > 0)
            {
                ddlOrganization.SelectedIndex = 0;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Save the current organization (add it if necessary)
            XmlNode node = Param.GetItem("//stylePick/settings/property[@name='Organization']");
            if (node == null)
            {
                // node doesn't exist yet - create it now
                XmlNode baseNode = Param.GetItem("//stylePick/settings");
                var childNode = Param.xmlMap.CreateNode(XmlNodeType.Element, "property", "");
                Param.AddAttrValue(childNode, "name", "Organization");
                Param.AddAttrValue(childNode, "value", Organization);
                baseNode.AppendChild(childNode);
                Param.Write();
            }
            else
            {
                // attribute is there - just set the value
                Param.SetValue("Organization", Organization);
                Param.Write();
            }
            // Check to see if this organization is in the list of Organizations
            bool bFound = false;
            foreach (var org in _organizations)
            {
                node = (XmlNode)org;
                if (node.FirstChild == null) continue;
                if (Organization == (node.FirstChild.Value))
                {
                    bFound = true;
                    break;
                }
            }
            if (!bFound)
            {
                // The current Organization isn't in the Organizations list - add it now
                XmlNode baseNode = Param.GetItem("//stylePick/Organizations");
                var childNode = Param.xmlMap.CreateNode(XmlNodeType.Element, "Organization", "");
                childNode.InnerText = Organization;
                baseNode.AppendChild(childNode);
                Param.Write();
            }
            // close out the dialog with an OK result
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Don't save anything - just close
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            ShowHelp.ShowHelpTopicKeyPress(this, _helpTopic, Common.IsUnixOS());
        }
    }
}
