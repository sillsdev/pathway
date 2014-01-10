using System.IO;
using System.Xml; 
using System.Windows.Forms;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class BookletBL
    {
        string _sectionPath = Path.GetDirectoryName(Param.SettingPath);
        private bool _isEdited;

        public void OpenDefaultSetting(ListBox lstSection)
        {
            string bookletSettingFullPath = Common.PathCombine(_sectionPath,"BookletSettings.xml");
            XmlNodeList nodeList = Common.GetXmlNodes(bookletSettingFullPath, "//sections");
            foreach (XmlNode data in nodeList)
            {
                lstSection.Items.Add(data.Attributes["name"].Value);
            } 
        }

        public void OpenSavedSetting(ListBox lstSection)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Pathway Booklet (*.bkl)|*.bkl";
            openFile.ShowDialog();

            string fileName = openFile.FileName;
            if (File.Exists(fileName))
            {
                lstSection.Items.Clear();
                XmlNodeList nodeList = Common.GetXmlNodes(fileName, "//sections");
                foreach (XmlNode data in nodeList)
                {
                    lstSection.Items.Add(data.Attributes["name"].Value);
                }
            }
        }

        public void MoveUp(ListBox lstSection)
        {
            int selectedIndex = lstSection.SelectedIndex;
            if(selectedIndex == -1 || selectedIndex==0) return;

            // Swap ListBox data between selectedIndex & selectedIndex - 1
            string temp = lstSection.Items[selectedIndex].ToString();
            lstSection.Items[selectedIndex] = lstSection.Items[selectedIndex - 1].ToString();
            lstSection.Items[selectedIndex - 1] = temp;

            lstSection.SelectedIndex = selectedIndex - 1;
            _isEdited = true;
        }

        public void MoveDown(ListBox lstSection)
        {
            int selectedIndex = lstSection.SelectedIndex;
            if (selectedIndex == -1 || selectedIndex == lstSection.Items.Count -1) return;

            // Swap ListBox data between selectedIndex & selectedIndex + 1
            string temp = lstSection.Items[selectedIndex].ToString();
            lstSection.Items[selectedIndex] = lstSection.Items[selectedIndex + 1].ToString();
            lstSection.Items[selectedIndex + 1] = temp;

            lstSection.SelectedIndex = selectedIndex + 1;
            _isEdited = true;
        }

        public void SaveAsSetting(ListBox lstSection)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                DefaultExt = "bkl",
                Filter = "Pathway Booklet (*.bkl)|*.bkl",
                OverwritePrompt = true,
                AddExtension = true
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            XmlTextWriter  xmlTextWriter = Common.CreateXMLFile(dlg.FileName);
            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("booklet");
            xmlTextWriter.WriteStartElement("sections");
            
            //lstSection
            for (int i = 0; i < lstSection.Items.Count; i++)
            {

                xmlTextWriter.WriteStartElement("section");
                xmlTextWriter.WriteAttributeString("name", lstSection.Items[i].ToString());
                xmlTextWriter.WriteEndElement();
            }
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();
            
            xmlTextWriter.Close();

            _isEdited = false;
        }

        public void AddSection(ListBox lstSection, string section)
        {
            lstSection.Items.Add(section);
        }

        public void RemoveSection(ListBox lstSection)
        {
            int selectedIndex = lstSection.SelectedIndex;
            if (selectedIndex == -1)
            {
                MessageBox.Show("Please select a Booklet Section", "Select a Booklet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string msg = "Do you want to remove the section " + lstSection.Items[selectedIndex];

            DialogResult result = MessageBox.Show(msg, "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.OK)
            {
                lstSection.Items.RemoveAt(lstSection.SelectedIndex);
            }
        }

        public void ShowAbout()
        {
            AboutBooklet aboutBooklet = new AboutBooklet();
            aboutBooklet.ShowDialog();
        }

        public void Exit()
        {
            // Save before close
           
        }
    }
}
