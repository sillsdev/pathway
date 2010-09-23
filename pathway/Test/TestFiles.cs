// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: TextFiles.cs
// Responsibility: Trihus
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using SIL.Tool;

namespace Test
{
    public class TestFiles
    {
        private string _inputPath;
        private string _outputPath;
        private string _expectedPath;

        public TestFiles(string testFilesBase)
        {
            string testPath = PathPart.Bin(Environment.CurrentDirectory, "/" + testFilesBase + "/TestFiles");
            _inputPath = Common.PathCombine(testPath, "input");
            _outputPath = Common.PathCombine(testPath, "output");
            _expectedPath = Common.PathCombine(testPath, "expected");
            if (Directory.Exists(_outputPath))
                Directory.Delete(_outputPath, true);
            Directory.CreateDirectory(_outputPath);
        }

        public string Input(string fileName)
        {
            return Common.PathCombine(_inputPath, fileName);
        }

        public string Copy(string fileName)
        {
            string output = Output(fileName);
            File.Copy(Input(fileName), output, true);
            return output;
        }

        public string Output(string fileName)
        {
            return Common.PathCombine(_outputPath, fileName);
        }

        public string Expected(string fileName)
        {
            return Common.PathCombine(_expectedPath, fileName);
        }
    }
}
