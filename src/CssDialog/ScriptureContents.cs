// --------------------------------------------------------------------------------------------
// <copyright file="ScriptureContents.cs" from='2009' to='2014' company='SIL International'>
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
// Satisfies Flex / TE interface and directs execution to appropriate class in PublishingSolution
// </remarks>
// --------------------------------------------------------------------------------------------

using System.IO;
using System.Windows.Forms;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ScriptureContents : Form, IScriptureContents
    {
        private static IScriptureContents _realClass;

        public string Format = null;

        public ScriptureContents()
        {
			Common.ProgBase = Common.GetPSApplicationPath();
            _realClass = new ExportThroughPathway();
            _realClass.DatabaseName = this.DatabaseName;
            ((ExportThroughPathway)_realClass).InputType = "Scripture";
        }

		#region Properties

        public string DatabaseName 
        { 
            set
            {
                _realClass.DatabaseName = value;
            }
            get
            {
                return _realClass.DatabaseName;
            }
        }

        public bool ExistingPublication
        {
           get
            {
                return _realClass.ExistingPublication;
            }
        }

        public string OutputLocationPath
        {
            get
            {
                return _realClass.OutputLocationPath;
            }
        }

        public string ExistingLocationPath
        {
            get
            {
                return _realClass.ExistingLocationPath;
            }
        }

        public string PublicationName
        {
            set
            {
                _realClass.PublicationName = value;
            }
            get
            {
                return _realClass.PublicationName;
            }
        }
        #endregion Properties

        public new DialogResult ShowDialog()
        {
            System.Windows.Forms.DialogResult dialogResult = _realClass.ShowDialog();
            if (dialogResult != DialogResult.Cancel)
            {
                Format = ((ExportThroughPathway) _realClass).Format;
                DirectoryInfo directoryInfo = new DirectoryInfo(_realClass.OutputLocationPath);
                directoryInfo.Create();
            }
            return dialogResult;
        }

    }
}
