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
using System.Threading;
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
            _inputPath = Common.PathCombine(testPath, "Input");
            _outputPath = Common.PathCombine(testPath, "output");
	        if (testFilesBase == "XhtmlExport" && Common.UsingMonoVM)
	        {
		        _expectedPath = Common.PathCombine(testPath, "ExpectedLinux");
	        }
	        else
	        {
		        _expectedPath = Common.PathCombine(testPath, "Expected");
	        }
	        if (Directory.Exists(_outputPath))
            {
                Directory.Delete(_outputPath, true);
                Thread.Sleep(1000);
            }
            Directory.CreateDirectory(_outputPath);
        }

        public string Input(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return _inputPath;
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
            if (string.IsNullOrEmpty(fileName))
                return _outputPath;
            return Common.PathCombine(_outputPath, fileName);
        }

        public string SubOutput(string test, string fileName)
        {
            var subPath = Common.PathCombine(_outputPath, test);
            if (!Directory.Exists(subPath))
            {
                Directory.CreateDirectory(subPath);
                Thread.Sleep(1000);
            }
            if (string.IsNullOrEmpty(fileName))
                return subPath;
            return Common.PathCombine(subPath, fileName);
        }

        public string Expected(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return _expectedPath;
            return Common.PathCombine(_expectedPath, fileName);
        }

        public string SubExpected(string test, string fileName)
        {
            var subPath = Common.PathCombine(_expectedPath, test);
            if (string.IsNullOrEmpty(fileName))
                return subPath;
            return Common.PathCombine(subPath, fileName);
        }
    }
}
