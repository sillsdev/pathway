// --------------------------------------------------------------------------------------------
// <copyright file="FileSplit.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Sankar Venkat</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
//
// <remarks>
//
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using

using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion Using


namespace SIL.Tool
{
    public class FileSplit
    {
        TextWriter _splitFile;
        string _fileFullPath;
        int _fileNumber;
        StringBuilder _headerFile;
        List<string> _filenameLists;

        public List<string> SplitFile(string fileFullPath)
        {
            _splitFile = null;
            _headerFile = new StringBuilder();
            _fileNumber = 0;
            _filenameLists = new List<string>();

            if (!File.Exists(fileFullPath))
            {
                return _filenameLists;
            }
            _fileFullPath = fileFullPath;
            StreamReader file = new StreamReader(fileFullPath);
            string line;

            //Header
            while ((line = file.ReadLine()) != null)
            {
                _headerFile.AppendLine(line);
                if (line.IndexOf("dicBody") > 0 && line.IndexOf("class=\"dicBody\">") > 0)
                {
                    break;
                }
            }

            while ((line = file.ReadLine()) != null)
            {
                ParseLine(line);
            }
            file.Close();
            if (_splitFile != null)
            {
                _splitFile.Close();
            }
            return _filenameLists;
        }

        private void ParseLine(string line)
        {
            if (line.IndexOf("letHead") > 0 && line.IndexOf("class=\"letHead\">") > 0)
            {
                CloseFile();
                CreateFile();
            }

            if (_splitFile != null)
            {
                _splitFile.WriteLine(line);
            }
        }

        private void CreateFile()
        {
            _fileNumber++;
            string fileName = "main" + _fileNumber + ".xhtml";

            string directory = Path.GetDirectoryName(_fileFullPath);

            fileName = Common.PathCombine(directory, fileName);

            _splitFile = new StreamWriter(fileName);

            _splitFile.WriteLine(_headerFile);

            _filenameLists.Add(fileName);
        }

        private void CloseFile()
        {
            if (_splitFile != null)
            {
                _splitFile.WriteLine("</body>");
                _splitFile.WriteLine("</html>");
                _splitFile.Close();
            }
        }
    }
}

