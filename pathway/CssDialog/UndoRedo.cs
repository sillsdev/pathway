// --------------------------------------------------------------------------------------------
// <copyright file="UndoRedo.cs" from='2009' to='2014' company='SIL International'>
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
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace SIL.Tool
{

    #region ModifyData Class
    public class ModifyData
    {
        public Common.Action Action;
        public string FileName;
        public string ControlText;
        public string EditStyleName;

        public ModifyData()
        {
        }
        public ModifyData(Common.Action action, string fileName, string editStyleName, string controlText)
        {
            Action = action;
            FileName = fileName; // Control Name or File Name
            ControlText = controlText;
            EditStyleName = editStyleName;
        }
    } 
    #endregion

    public class UndoRedo
    {
        private object _undoButton;
        private object _redoButton;
        private Stack _UndoStack = new Stack();
        private Stack _RedoStack = new Stack();
        private ModifyData modifyData;
        public string SettingOutputPath = string.Empty;

        public string CurrentControl = string.Empty;
        public string PreviousControl = string.Empty;

        public UndoRedo(object undo, object redo)
        {
            _undoButton = undo;
            _redoButton = redo;
        }

        private void EnableRedoUndo()
        {
            if(_undoButton is ToolStripButton)
            {
                var button = (ToolStripButton)_undoButton;
                button.Enabled = (_UndoStack.Count > 0);
            }
            if (_redoButton is ToolStripButton)
            {
                var button = (ToolStripButton)_redoButton;
                button.Enabled = (_RedoStack.Count > 0);
            }
        }

        public void Reset()
        {
            _UndoStack.Clear();
            _RedoStack.Clear();
            EnableRedoUndo();
        }

        public bool Set(Common.Action action, string editStyleName, string controlName,string previousText, string currentText)
        {
            string fileName = string.Empty;
            bool isEdit = false;
            if (action == Common.Action.Edit)
            {
                fileName = controlName;
                if(PreviousControl != CurrentControl)
                {
                    modifyData = new ModifyData(action, fileName, editStyleName, previousText);
                    _UndoStack.Push(modifyData);
                    isEdit = true;
                }
            }
            else
            {
                fileName = BackupForUndo(); // add , delete or copy
            }
            modifyData = new ModifyData(action, fileName, editStyleName, currentText);
            _UndoStack.Push(modifyData);
            EnableRedoUndo();
            return isEdit;
        }
        public ModifyData Undo(Common.Action action, string editStyleName, string controlName, string controlText)
        {

            ModifyData undoData = new ModifyData();

            if (_UndoStack.Count == 0)
            {
                return null;
            }
            undoData = (ModifyData) _UndoStack.Pop();
            _RedoStack.Push(undoData);
            if (undoData.Action != Common.Action.Edit)
            {
                string fileName = undoData.FileName;
                File.Copy(fileName, SettingOutputPath, true);
            }
            EnableRedoUndo();
            return undoData;
        }
        public ModifyData JustPopUp_Undo()
        {
            if (_UndoStack.Count == 0)
            {
                return new ModifyData();
            }
            ModifyData undoData = (ModifyData)_UndoStack.Pop();
            _RedoStack.Push(undoData);
            return undoData;
        }
        public ModifyData Redo()
        {
            ModifyData undoData = new ModifyData();

            if (_RedoStack.Count == 0)
            {
                return null;
            }
            undoData = (ModifyData)_RedoStack.Pop();
            if (undoData == null) 
                return null;
            if (undoData.Action != Common.Action.Edit) // add or del
            {
                string fileName = undoData.FileName;
                File.Copy(fileName, SettingOutputPath, true);
            }
            _UndoStack.Push(undoData);
            EnableRedoUndo();
            return undoData;
    }
    
        private string BackupForUndo()
        {
            string fileName = string.Empty;
            string file = SettingOutputPath;
            string tempFile = Path.GetTempFileName();
            if (File.Exists(file))
            {
                File.Copy(file, tempFile, true);
                fileName = tempFile;
            }
            return fileName;
        }
    }
}
