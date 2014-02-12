// --------------------------------------------------------------------------------------------
// <copyright file="CSSError.cs" from='2009' to='2009' company='SIL International'>
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
// Displays the Css Errors
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using JWTools;
using SIL.Tool;
using SIL.Tool.Localization;

namespace SIL.PublishingSolution
{
    public partial class CSSError : Form
    {

        #region Private Variables
        readonly Dictionary<string, ArrayList> ErrorList = new Dictionary<string, ArrayList>();
        private readonly string sourcePath = Path.GetTempPath();
        private string loadFileName = string.Empty;
        readonly ArrayList errorLine = new ArrayList();
        private string cssFile = string.Empty;
        #endregion

        public CSSError()
        {
            InitializeComponent();
        }
        /// <summary>
        /// To load error list and source path
        /// </summary>
        /// <param name="errList">List of Errors in CSS</param>
        /// <param name="sourcePath">Source Path</param>
        public CSSError(Dictionary<string, ArrayList> errList, string sourcePath)
        {
            InitializeComponent();
            ErrorList = errList;
            this.sourcePath = sourcePath;
        }

        private void CSSError_Load(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);     // Form Controls
            foreach (KeyValuePair<string, ArrayList> errList in ErrorList)
            {
                if (loadFileName == string.Empty)
                {
                    loadFileName = errList.Key;
                }
                lstErrorReport.Items.Add(errList.Key);
                foreach (string err in errList.Value)
                {
                    lstErrorReport.Items.Add(err);
                }
            }
            LoadErrorCSS(loadFileName, 0);
        }

        /// <summary>
        /// To load Filename and the error into the listbox.
        /// </summary>
        /// <param name="loadFile">CSS FileName</param>
        /// <param name="errorNo">Error number</param>
        private void LoadErrorCSS(string loadFile, int errorNo)
        {
            if (cssFile == loadFile)
            {
                ShowError(errorNo);
            }
            cssFile = loadFile;
            errorLine.Clear();
            int searchStart= 0;
            string searchText;
            lblCSSFileName.Text = loadFile;
            CSSWindow.LoadFile(Common.PathCombine(sourcePath, loadFile), RichTextBoxStreamType.PlainText);
            string richtextSt = CSSWindow.Text;
            string[] splitText = richtextSt.Split('\n');
            ArrayList error = ErrorList[loadFile];
            foreach (string errLine in error)
            {
                string lineMsg = errLine.Replace("line", "");
                int lineNo = int.Parse(Common.LeftString(lineMsg, ":"));
                if (lineNo == 0) continue;
                searchText = splitText[lineNo - 1];
                int position = CSSWindow.Find(searchText.Trim(), searchStart, RichTextBoxFinds.None);
                searchStart = position + searchText.Length;
                // highlight
                if (position >= 0)
                {
                    CSSWindow.SelectionStart = position;
                    CSSWindow.Select();
                    var newFont = new Font("Verdana", 10.0F, FontStyle.Bold);
                    CSSWindow.SelectionFont = newFont;
                    CSSWindow.SelectionBackColor = Color.Yellow;
                    errorLine.Add(position);
                }
            }
            ShowError(errorNo);
        }

        /// <summary>
        /// To show the error based on error number
        /// </summary>
        /// <param name="errorNumber">Error number to select</param>
        private void ShowError(int errorNumber)
        {
            if (errorNumber >= 0)
            {
                if (errorLine.Count <= errorNumber)
                    errorNumber -= 1;
                CSSWindow.Select(int.Parse(errorLine[errorNumber].ToString()), 1);
            }
            else
            {
                CSSWindow.Select(0, 1);
            }
        }

        /// <summary>
        /// Close the Error Window
        /// </summary>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// To load the file into the CSSWindow based on file select in the errorlist.
        /// </summary>
        private void lstErrorReport_Click(object sender, EventArgs e)
        {
            int currindex = lstErrorReport.SelectedIndex;
            for (int i = currindex; i >= 0; i--)
            {
                string cssFileName = lstErrorReport.Items[i].ToString();
                if (Common.LeftString(cssFileName, " ") != "line")
                {
                    LoadErrorCSS(cssFileName, currindex - i - 1);
                    break;
                }
            }
        }

        /// <summary>
        /// To save the file after error are corrected.
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            CSSWindow.SaveFile(Common.PathCombine(sourcePath, loadFileName), RichTextBoxStreamType.PlainText);

            LocDB.Message("msgSaveSucess", "File saved successfully", null, LocDB.MessageTypes.Info,
                          LocDB.MessageDefault.First);
        }

        private void CSSError_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG
            var dlg = new Localizer(LocDB.DB);
            dlg.ShowDialog();
#endif
        }

        private void CSSError_Activated(object sender, EventArgs e)
        {
            Common.SetFont(this);
        }
    }
}
