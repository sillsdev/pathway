using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
//Imports System.Messaging
using System.Threading;
using System.Xml;
using str = Microsoft.VisualBasic.Strings;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;

namespace PrincessConvert
{
    public partial class multipleFiles : Form
    {
        private bool blnFileAlreadyOpen;
        private int filenum = 1;
        Main main = new Main();
        public multipleFiles()
        {
            InitializeComponent();
        }

        private void multipleFiles_Load(object sender, EventArgs e)
        {
            //        Main.sDocumentsFolder = FolderBrowserDialog1.SelectedPath()

            main.readIniFile();
            tbTotalNumberOfInputFilesToProcess.Text = Main.iNumberOfFilesToProcess.ToString();
            if (Main.sDocumentsFolder == "#none#")
            {
                lblDocumentsFolderName.Text = Main.sDocumentsFolder;
                // skip
            }
            else
            {
                displayFiles();
            }
            disableButtons();
            main.displayInfo();
            main.writeIniFile();
            //displayFilesInFolder()

        }

        private void displayFiles()
        {
            lblDocumentsFolderName.Text = Main.sDocumentsFolder;
            //   If Main.blnOutputMultipleFiles = True Then
            // rbMultipleOutputFiles.Checked = True
            //
            //        Else
            //        rbSingleOutputFile.Checked = True
            //        End If
            //lbFiles.Items = 
            //     getFilesToPutInList()
            //        tbTotalNumberOfInputFilesToProcess.Text = lbFiles.Items.Count
        }
        public object verifyFileIsHTML(string fileToVerify)
        {
            // see that file exists and that it starts with '<html' 
            string line1 = null;
            string line2 = null;
            string line3 = null;
            try
            {
                blnFileAlreadyOpen = false;
                FileSystem.FileOpen(filenum, fileToVerify, OpenMode.Input, OpenAccess.Read, OpenShare.Shared,1);
                // see if either first or second or third line of file starts with '<html' or '<HTML'
                line1 = Main.inputLine();
                line2 = Main.inputLine();
                line3 = Main.inputLine();
                if (str.InStr(line1, "<html", CompareMethod.Text) > 0 | str.InStr(line1, "<HTML", CompareMethod.Text) > 0 | str.InStr(line2, "<html", CompareMethod.Text) > 0 | str.InStr(line2, "<HTML", CompareMethod.Text) > 0 | str.InStr(line3, "<html", CompareMethod.Text) > 0 | str.InStr(line3, "<HTML", CompareMethod.Text) > 0)
                {
                    FileSystem.FileClose(filenum);
                    return true;
                }
                FileSystem.FileClose(filenum);
            }
            catch (Exception ex)
            {
                // file is open in another program
                // please close file in other program and try again
                FileSystem.FileClose(filenum);
                blnFileAlreadyOpen = true;
                return true;
            }
            return false;
        }
        private void getFilesToPutInList()
        {
            try
            {
                if (lbFiles.Items.Count > 0)
                {
                    // already loaded files
                }
                else
                {
                    foreach (string fileName_loopVariable in Directory.GetFiles(Main.sDocumentsFolder))
                    {
                        string fileName = fileName_loopVariable;
                        lbFiles.Items.Add(Main.getFileNameWithExtensionFromFullName(fileName));
                    }
                }

            }
            catch (Exception ex)
            {
                //       Main.aFiles = Directory.GetFiles(Main.sDocumentsFolder)

            }
            //Dim x
            //x = Main.aFiles

            // addFilesTOlbFiles()

        }

        private void btnCancel_Click(System.Object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.Close();
        }

        private void btnBrowse_Click(System.Object sender, System.EventArgs e)
        {
            //  Erase Main.aFiles
            lbFiles.Items.Clear();
            Main.iNumberOfFilesToProcess = 0;
            FolderBrowserDialog1.SelectedPath = Main.sDocumentsFolder;
            FolderBrowserDialog1.Description = "Document folder containing files to process";
            FolderBrowserDialog1.ShowDialog();
            Main.sDocumentsFolder = FolderBrowserDialog1.SelectedPath;
            getFilesToPutInList();
            displayFiles();
            tbTotalNumberOfInputFilesToProcess.Text = lbFiles.Items.Count.ToString();
            main.writeIniFile();
        }

        public void addFile(string filename)
        {
            try
            {
                lbFiles.Items.Add(Main.getFileNameWithExtensionFromFullName(filename));

            }
            catch (Exception ex)
            {
                MessageBox.Show("problem adding file");

            }

            //row.Item("Name") = getFileNameWithoutExtensionFromFullName(filename)
            //row.Item("Path") = filename
        }

        private void btnRemoveSelectedFiles_Click(System.Object sender, System.EventArgs e)
        {
            // lbFiles.Items.Remove(lbFiles.SelectedItems)

            while (!(lbFiles.SelectedIndex == -1))
            {
                lbFiles.Items.RemoveAt(lbFiles.SelectedIndex);
            }

            //        lbFiles.Items.Remove(lbFiles.SelectedItems)
            tbTotalNumberOfInputFilesToProcess.Text = lbFiles.Items.Count.ToString();
            lbFiles.Update();
        }

        private void btnUpStart_Click(System.Object sender, System.EventArgs e)
        {
            string temp = lbFiles.SelectedItem.ToString();
            if (lbFiles.SelectedIndex >= 1)
            {
                lbFiles.Sorted = false;
                lbFiles.Items.Insert(0, temp);
                lbFiles.Items.RemoveAt(lbFiles.SelectedIndex);
                lbFiles.SelectedItem = temp;
            }
            else
            {
                // do nothing
            }

        }

        private void btnUp_Click(System.Object sender, System.EventArgs e)
        {
            string temp = lbFiles.SelectedItem.ToString();
            if (lbFiles.SelectedIndex == -1)
            {
                // skip
            }
            else
            {
                if (lbFiles.SelectedIndex >= 1)
                {
                    lbFiles.Sorted = false;
                    lbFiles.Items.Insert(lbFiles.SelectedIndex - 1, temp);
                    lbFiles.Items.RemoveAt(lbFiles.SelectedIndex);
                    lbFiles.SelectedItem = temp;
                    lbFiles.Update();
                }
                else
                {
                    // do nothing
                }
            }

        }

        private void btnDown_Click(System.Object sender, System.EventArgs e)
        {
            string temp = lbFiles.SelectedItem.ToString(); ;
            if (lbFiles.SelectedIndex == -1)
            {
                //skip
            }
            else
            {
                if (lbFiles.SelectedIndex < lbFiles.Items.Count - 1)
                {
                    lbFiles.Sorted = false;
                    lbFiles.Items.Insert(lbFiles.SelectedIndex + 2, temp);
                    lbFiles.Items.RemoveAt(lbFiles.SelectedIndex);
                    lbFiles.SelectedItem = temp;
                }
                else
                {
                    // do nothing
                }

            }

        }

        private void btnDownEnd_Click(System.Object sender, System.EventArgs e)
        {
            string temp = lbFiles.SelectedItem.ToString(); 
            if (lbFiles.SelectedIndex < lbFiles.Items.Count & lbFiles.SelectedIndex > -1)
            {
                lbFiles.Sorted = false;
                lbFiles.Items.Insert(lbFiles.Items.Count, temp);
                lbFiles.Items.RemoveAt(lbFiles.SelectedIndex);
                lbFiles.SelectedItem = temp;
                lbFiles.Update();
            }
            else
            {
                // do nothing
            }
        }

        private void btnSort_Click(System.Object sender, System.EventArgs e)
        {
            lbFiles.Sorted = true;
            //  lbFiles.SelectedIndex = 1
        }

        public bool verifyCopyExists()
        {

            foreach (string item_loopVariable in lbFiles.Items)
            {
                string item = item_loopVariable;
                if (File.Exists(item + ".copy"))
                {
                    // skip
                }
                else
                {
                    MessageBox.Show("Be sure that you have processed each of these files individually" + Constants.vbCrLf + "before trying to join them together in one PDF document." + Constants.vbCrLf + Constants.vbCrLf + "Please process " + Constants.vbCrLf + item + Constants.vbCrLf + "before trying to combine work.", "Missing processed file", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

            }


            return true;

        }
        private void btnOK_Click(System.Object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //   Dim x
            //   x = Main.aFiles.Count
            //   lbFiles.Items.CopyTo(Main.aFiles, 1)
            Main.iNumberOfFilesToProcess = lbFiles.Items.Count;
            int x = lbFiles.Items.Count;


            if (verifyCopyExists())
            {

                if (Main.iNumberOfFilesToProcess > 1)
                {
                    // set name of document file to the folder name
                    main.blnProcessMultipleInputFiles = true;
                    Main.blnProcessURL = false;
                    Main.sDocumentFileName = Main.sDocumentsFolder;
                    Main.sInputFileName = Main.getFileNameWithExtensionFromFullName(Main.sDocumentsFolder);
                }
                else
                {
                    // process normally
                }

                if (lbFiles.Items.Count == 0)
                {
                    MessageBox.Show("Missing file names in multiple list box.", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //           MessageBox.Show("Files = " + lbFiles.Items.Count.ToString, "OK", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                }

                Int16 iNumber = default(Int16);
                foreach (string item_loopVariable in lbFiles.Items)
                {
                    string item = item_loopVariable;
                    Main.sMultipleFileNames[iNumber] = item;
                    iNumber += 1;
                }
                
                main.turnOnOffEditing();
                main.writeIniFile();

                main.displayInfo();
                this.Cursor = Cursors.Default;
                this.Close();
                MessageBox.Show("Ready to convert? -- click 'Convert' on the menu bar." + Constants.vbCrLf + "To select a style to apply first, click 'File/Stylesheet...", "", MessageBoxButtons.OK, MessageBoxIcon.Question);
            }
            else
            {
                this.Cursor = Cursors.Default;
                this.Close();
            }
        }

        private void lbFiles_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            if (lbFiles.SelectedIndex == -1)
            {
                disableButtons();

            }
            else
            {
                enableButtons();
            }
        }

        //  Private Sub lbfiles_changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbFiles.KeyPress
        //     If Keys.Delete Then
        //        MessageBox.Show("xxxx", "ooo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        //   Else
        //'
        //  End If
        // End Sub
        private void disableButtons()
        {
            btnDown.Enabled = false;
            btnUp.Enabled = false;
            btnDownEnd.Enabled = false;
            btnUpStart.Enabled = false;
            btnRemoveSelectedFiles.Enabled = false;

        }
        private void enableButtons()
        {
            btnDown.Enabled = true;
            btnUp.Enabled = true;
            btnDownEnd.Enabled = true;
            btnUpStart.Enabled = true;
            btnRemoveSelectedFiles.Enabled = true;

        }
    }
}
