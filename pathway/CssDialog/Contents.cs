// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright file="Contents.cs" from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.   
//    
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
#endregion
// 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Satisfies Flex / TE interface and directs execution to appropriate class in PublishingSolution
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Windows.Forms;
using SIL.FieldWorks.Common.FwUtils;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class Contents : Form, IExportContents
    {
        private static IExportContents _realClass;
        public Contents()
        {
            Common.SupportFolder = "PathwaySupport";
            Param.ProgBase = Common.GetPSApplicationPath();
            _realClass = new PrintVia();
            ((PrintVia)_realClass).InputType = "Dictionary";
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

        public bool ExportMain
        {
            set
            {
                _realClass.ExportMain = value;
            }
            get
            {
                return _realClass.ExportMain;
            }
        }

        public bool ExportReversal
        {
            set
            {
                _realClass.ExportReversal = value;
            }
            get
            {
                return _realClass.ExportReversal;
            }
        }

        public bool ExportGrammar
        {
            set
            {
                _realClass.ExportGrammar = value;
            }
            get
            {
                return _realClass.ExportGrammar;
            }
        }

        public bool ReversalExists
        {
            set
            {
                _realClass.ReversalExists = value;
            }
        }

        public bool GrammarExists
        {
            set
            {
                _realClass.GrammarExists = value;
            }
        }

        public bool ExistingDirectoryInput
        {
           get
            {
                return _realClass.ExistingDirectoryInput;
            }
        }

        public string OutputLocationPath
        {
            get
            {
                return _realClass.OutputLocationPath;
            }
        }

        public string ExistingDirectoryLocationPath
        {
            get
            {
                return _realClass.ExistingDirectoryLocationPath;
            }
        }

        public string DictionaryName
        {
            get
            {
                return _realClass.DictionaryName;
            }
        }
        #endregion Properties

        public DialogResult ShowDialog()
        {
            return _realClass.ShowDialog();
        }

    }
}
