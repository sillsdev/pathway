using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
using SIL.Tool;

namespace Test
{
    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Tests methods in the FileUtils class.
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    [TestFixture]
    public class FileUtilsTest
    {
        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// Tests the method IsFilePathValid.
        /// </summary>
        ///--------------------------------------------------------------------------------------
        [Test]
        public void IsFilePathValid()
        {
            // File names
            Assert.IsTrue(SIL.Tool.FileUtils.IsPathNameValid("regularFilename.test"));
            Assert.IsFalse(SIL.Tool.FileUtils.IsPathNameValid("|BadFilename|.test"));

            // Absolute and relative path names
            Assert.IsTrue(SIL.Tool.FileUtils.IsPathNameValid(@"\Tmp\Pictures\books.gif"));
            Assert.IsTrue(SIL.Tool.FileUtils.IsPathNameValid(@"Tmp\Pictures\books.gif"));

            // Path names with device
            Assert.IsTrue(SIL.Tool.FileUtils.IsPathNameValid(@"C:\Tmp\Pictures\books.gif"));
            Assert.IsFalse(SIL.Tool.FileUtils.IsPathNameValid(@"C\:Tmp\Pictures\books.gif"));
        }

        #region ActualFilePath tests
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Tests FileUtils.ActualFilePath with a file path which exists.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void ActualFilePath_ExactNameExists()
        {
            MockFileOS fileOs = new MockFileOS();
            fileOs.AddExistingFile("boo");
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            Assert.AreEqual("boo", SIL.Tool.FileUtils.ActualFilePath("boo"));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Tests FileUtils.ActualFilePath with a file path which exists.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void ActualFilePath_FileDoesNotExist()
        {
            MockFileOS fileOs = new MockFileOS();
            fileOs.AddExistingFile("flurp");
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            Assert.AreEqual("boo", SIL.Tool.FileUtils.ActualFilePath("boo"));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Tests FileUtils.ActualFilePath with an existing file whose path is a composed form
        /// of the given path.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void ActualFilePath_ComposedFilenameExists()
        {
            MockFileOS fileOs = new MockFileOS();
            fileOs.AddExistingFile("\u00e9");
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            Assert.AreEqual("\u00e9", SIL.Tool.FileUtils.ActualFilePath("\u0065\u0301")); // accented e
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Tests FileUtils.ActualFilePath with an existing file whose path is a decomposed form of
        /// the given path.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void ActualFilePath_DecomposedFilenameExists()
        {
            MockFileOS fileOs = new MockFileOS();
            fileOs.AddExistingFile("\u0065\u0301");
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            Assert.AreEqual("\u0065\u0301", SIL.Tool.FileUtils.ActualFilePath("\u00e9")); // accented e
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Tests FileUtils.ActualFilePath with an existing directory containing a file with
        /// different capitalization.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void ActualFilePath_DirectoryNameExactMatchFilenameExistsWithDifferentCase()
        {
            MockFileOS fileOs = new MockFileOS();
            fileOs.AddExistingFile("AbC");
            fileOs.m_existingDirectories.Add(@"c:\My Documents");
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            Assert.AreEqual(@"c:\My Documents\AbC", SIL.Tool.FileUtils.ActualFilePath(@"c:\My Documents\abc"));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Tests FileUtils.ActualFilePath with an existing directory whose path uses composed
        /// instead of decomposed characters containing a file with different capitalization.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void ActualFilePath_DirectoryNameComposedFilenameExistsWithDifferentCase()
        {
            MockFileOS fileOs = new MockFileOS();
            fileOs.AddExistingFile("AbC");
            fileOs.m_existingDirectories.Add("c:\\My Docum\u00e9nts");
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            Assert.AreEqual("c:\\My Docum\u00e9nts\\AbC", SIL.Tool.FileUtils.ActualFilePath("c:\\My Docum\u0065\u0301nts\\abc"));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Tests FileUtils.ActualFilePath with an existing directory whose path uses decomposed
        /// instead of composed characters containing a file with different capitalization.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void ActualFilePath_DirectoryNameDecomposedFilenameExistsWithDifferentCase()
        {
            MockFileOS fileOs = new MockFileOS();
            fileOs.AddExistingFile("AbC");
            fileOs.m_existingDirectories.Add("c:\\My Docum\u0065\u0301nts");
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            Assert.AreEqual("c:\\My Docum\u0065\u0301nts\\AbC", SIL.Tool.FileUtils.ActualFilePath("c:\\My Docum\u00e9nts\\abc"));
        }
        #endregion

        #region DetermineSfFileEncoding tests
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Jira number for this is TE-155
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void DetermineSfFileEncoding_UnicodeBOM()
        {
            MockFileOS fileOs = new MockFileOS();
            string filename = fileOs.MakeFile("\ufeff\\id EPH", Encoding.Unicode);
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);

            Assert.AreEqual(Encoding.Unicode, SIL.Tool.FileUtils.DetermineSfFileEncoding(filename));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Jira number for this is TE-155
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void DetermineSfFileEncoding_UnicodeNoBOM()
        {
            MockFileOS fileOs = new MockFileOS();
            string filename = fileOs.MakeFile(@"\id EPH", Encoding.Unicode);
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);

            Assert.AreEqual(Encoding.Unicode, SIL.Tool.FileUtils.DetermineSfFileEncoding(filename));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Jira number for this is TE-155
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void DetermineSfFileEncoding_UTF8BOM()
        {
            MockFileOS fileOs = new MockFileOS();
            string filename = fileOs.MakeFile("\ufeff\\id EPH", Encoding.UTF8);
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);

            Assert.AreEqual(Encoding.UTF8, SIL.Tool.FileUtils.DetermineSfFileEncoding(filename));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Jira number for this is TE-155
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void DetermineSfFileEncoding_UTF8NoBOM()
        {
            MockFileOS fileOs = new MockFileOS();
            string filename = fileOs.MakeFile(
                "\\id EPH\r\n\\ud 12/Aug/2002\r\n\\mt \u0782\u0785\u07a7\u0794\r\n\\c 1\r\n\\s \u0787\u0786\u078c\u07a6 \u0794\u0786\u078c\r\n\\p\r\n\\v 1\r\n\\vt \u078c\u0789\u0789\u0782\u0780\u07a2",
                Encoding.UTF8);
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);

            Assert.AreEqual(Encoding.UTF8, SIL.Tool.FileUtils.DetermineSfFileEncoding(filename));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Jira number for this is TE-155
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void DetermineSfFileEncoding_ASCII()
        {
            MockFileOS fileOs = new MockFileOS();
            string filename = fileOs.MakeFile("\\id EPH\r\n\\mt Ephesians\\c 1\\v 1", Encoding.ASCII);
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);

            Assert.AreEqual(Encoding.ASCII, SIL.Tool.FileUtils.DetermineSfFileEncoding(filename));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Jira number for this is TE-155
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void DetermineSfFileEncoding_BigEndianUnicodeBOM()
        {
            MockFileOS fileOs = new MockFileOS();
            string filename = fileOs.MakeFile("\ufeff\\id EPH", Encoding.BigEndianUnicode);
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);

            Assert.AreEqual(Encoding.BigEndianUnicode, SIL.Tool.FileUtils.DetermineSfFileEncoding(filename));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Jira number for this is TE-155
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void DetermineSfFileEncoding_BigEndianUnicodeNoBOM()
        {
            MockFileOS fileOs = new MockFileOS();
            string filename = fileOs.MakeFile(@"\id EPH", Encoding.BigEndianUnicode);
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);

            Assert.AreEqual(Encoding.BigEndianUnicode, SIL.Tool.FileUtils.DetermineSfFileEncoding(filename));
        }
        #endregion

        #region IsFileReadable Tests
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Jira number for this is TE-155
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void IsFileReadable_True()
        {
            MockFileOS fileOs = new MockFileOS();
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            string filename = fileOs.MakeFile("bumppiness");
            Assert.IsTrue(SIL.Tool.FileUtils.IsFileReadable(filename));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Jira number for this is TE-155
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void IsFileReadable_NonExistent()
        {
            MockFileOS fileOs = new MockFileOS();
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            Assert.IsFalse(SIL.Tool.FileUtils.IsFileReadable("Whatever.txt"));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Jira number for this is TE-155
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void IsFileReadable_OpenForWrite()
        {
            MockFileOS fileOs = new MockFileOS();
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            string filename = fileOs.MakeFile("bumppiness");
            fileOs.LockFile(filename);
            Assert.IsFalse(SIL.Tool.FileUtils.IsFileReadable(filename));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Ensures that IsFileReadableAndWriteable returns true for an existing (unlocked) file
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void IsFileReadableAndWritable_UnlockedFile()
        {
            MockFileOS fileOs = new MockFileOS();
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            string filename = fileOs.MakeFile("bumppiness");
            Assert.IsTrue(SIL.Tool.FileUtils.IsFileReadableAndWritable(filename));
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Ensures that IsFileReadableAndWriteable returns false for a file that is open for
        /// read and true if all open readers are closed
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void IsFileReadableAndWritable_OpenForRead()
        {
            MockFileOS fileOs = new MockFileOS();
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            string filename = fileOs.MakeFile("bumppiness", Encoding.UTF8);
            TextReader reader = fileOs.GetReader(filename, Encoding.UTF8);
            Stream stream = fileOs.OpenStreamForRead(filename);
            Assert.IsFalse(SIL.Tool.FileUtils.IsFileReadableAndWritable(filename));
            reader.Close();
            Assert.IsFalse(SIL.Tool.FileUtils.IsFileReadableAndWritable(filename));
            stream.Close();
            Assert.IsTrue(SIL.Tool.FileUtils.IsFileReadableAndWritable(filename));
        }
        #endregion

        #region MockFileOS tests
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Ensures that Delete fails if file is locked (open for write)
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        [Ignore]
        //[ExpectedException(ExceptionType = typeof(IOException),
            //ExpectedMessage = "File ReadMe.txt is locked (open for write).")]
        public void Delete_FailsIfOpenForWrite()
        {
            MockFileOS fileOs = new MockFileOS();
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            string filename = "ReadMe.txt";
            fileOs.AddFile(filename, "For more information, read this.", Encoding.ASCII);
            fileOs.LockFile(filename);
            fileOs.Delete(filename);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Ensures that Delete fails if file is locked (open for write)
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        [Ignore]

        //[ExpectedException(ExceptionType = typeof(IOException),
            //ExpectedMessage = "File ReadMe.txt is locked (open for read).")]
        public void Delete_FailsIfOpenForRead()
        {
            MockFileOS fileOs = new MockFileOS();
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            string filename = "ReadMe.txt";
            fileOs.AddFile(filename, "For more information, read this.", Encoding.ASCII);
            TextReader reader = fileOs.GetReader(filename, Encoding.ASCII);
            fileOs.Delete(filename);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Ensures that Delete fails if file does not exist
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        [Ignore]

        //[ExpectedException(ExceptionType = typeof(IOException),
            //ExpectedMessage = "File ReadMe.txt not found.")]
        public void Delete_FailsIfFileDoesNotExist()
        {
            MockFileOS fileOs = new MockFileOS();
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            fileOs.Delete("ReadMe.txt");
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Ensures that Delete removes a file correctly
        /// </summary>
        /// ------------------------------------------------------------------------------------
        [Test]
        public void Delete_Success()
        {
            MockFileOS fileOs = new MockFileOS();
            ReflectionHelperLite.SetField(typeof(SIL.Tool.FileUtils), "s_fileos", fileOs);
            string filename = fileOs.MakeFile("This file is going away.");
            fileOs.Delete(filename);
            Assert.IsFalse(fileOs.FileExists(filename));
        }
        #endregion
    }

    /// ----------------------------------------------------------------------------------------
    /// <summary>
    /// Mock version of IFileOS that lets us simulate existing files and directories.
    /// </summary>
    /// ----------------------------------------------------------------------------------------
    public class MockFileOS : SIL.Tool.IFileOS
    {
        internal enum FileLockType
        {
            /// <summary>Not locked. Use this if you just need to see if file exists.</summary>
            None,
            /// <summary>Read lock (can have other read-locks open)</summary>
            Read,
            /// <summary>Write or delete lock (exclusive)</summary>
            Write,
        }

        internal class MockFileInfo
        {
            internal string Contents;
            internal Encoding Encoding;
            private FileLockType m_hardLock = FileLockType.None;
            private List<IDisposable> m_streams;

            internal MockFileInfo(string contents, Encoding encoding)
            {
                Contents = contents;
                Encoding = encoding;
            }

            internal void AddStream(IDisposable stream)
            {
                if (stream is Stream || stream is StringReader)
                {
                    if (m_streams == null)
                        m_streams = new List<IDisposable>();
                    m_streams.Add(stream);
                }
                else
                    throw new ArgumentException("AddStream can only be called with a Stream or StringReader.");
            }

            internal FileLockType Lock
            {
                get
                {
                    if (m_hardLock == FileLockType.Write || m_streams == null)
                        return m_hardLock;
                    FileLockType worstLock = m_hardLock;
                    for (int i = 0; i < m_streams.Count; i++)
                    {
                        if (m_streams[i] is Stream)
                        {
                            Stream s = (Stream)m_streams[i];
                            if (s.CanWrite)
                                return FileLockType.Write;
                            if (s.CanRead)
                                worstLock = FileLockType.Read;
                            else
                                m_streams.RemoveAt(i--);
                        }
                        else if (m_streams[i] is StringReader)
                        {
                            StringReader sr = (StringReader)m_streams[i];
                            try
                            {
                                sr.Peek();
                                worstLock = FileLockType.Read;
                            }
                            catch (ObjectDisposedException)
                            {
                                m_streams.RemoveAt(i--);
                            }
                        }
                    }
                    return worstLock;
                }
                set { m_hardLock = value; }
            }
        }
        /// <summary>Add fake file names to this list to simulate existing files. The value
        /// is the file contents.</summary>
        private Dictionary<string, MockFileInfo> m_existingFiles = new Dictionary<string, MockFileInfo>();
        /// <summary>Add fake folder names to this list to simulate existing folders</summary>
        public List<string> m_existingDirectories = new List<string>();

        #region Public Methods to facilitate testing
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the given filename to the collection of files that should be considered to
        /// exist.
        /// </summary>
        /// <param name="filename">The filename (may or may not include path).</param>
        /// ------------------------------------------------------------------------------------
        public void AddExistingFile(string filename)
        {
            AddFile(filename, null, Encoding.UTF8);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Adds a new "temp" file (with fully-qualified path and name) to the collection of
        /// files and sets its contents so it can be read. Encoding will be UTF8.
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <returns>The name of the file</returns>
        /// ------------------------------------------------------------------------------------
        public string MakeFile(string contents)
        {
            return MakeFile(contents, Encoding.UTF8);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Adds a new "temp" file (with fully-qualified path and name) to the collection of
        /// files and sets its contents so it can be read.
        /// </summary>
        /// <param name="contents">The contents of the file</param>
        /// <returns>The name of the file</returns>
        /// <param name="encoding">File encoding</param>
        /// ------------------------------------------------------------------------------------
        public string MakeFile(string contents, Encoding encoding)
        {
            string filename = Common.PathCombine(Path.GetTempPath(), Path.GetRandomFileName());
            m_existingFiles[filename] = new MockFileInfo(contents, encoding);
            return filename;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Create a mocked Standard Format file with \id line and file contents as specified.
        /// No BOM. Encoding will be UTF8.
        /// </summary>
        /// <param name="sBookId">The book id (if set, this will cause \id line to be written
        /// as the first line of the fiel)</param>
        /// <param name="lines">Remaining lines of the file</param>
        /// ------------------------------------------------------------------------------------
        public string MakeSfFile(string sBookId, params string[] lines)
        {
            return MakeSfFile(false, sBookId, lines);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Create a mocked Standard Format file with \id line and file contents as specified.
        /// Encoding will be UTF8.
        /// </summary>
        /// <param name="fIncludeBOM">Indicates whether file contents should include the byte
        /// order mark</param>
        /// <param name="sBookId">The book id (if set, this will cause \id line to be written
        /// as the first line of the fiel)</param>
        /// <param name="lines">Remaining lines of the file</param>
        /// ------------------------------------------------------------------------------------
        public string MakeSfFile(bool fIncludeBOM, string sBookId, params string[] lines)
        {
            return MakeSfFile(Encoding.UTF8, fIncludeBOM, sBookId, lines);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Create a mocked Standard Format file with \id line and file contents as specified.
        /// </summary>
        /// <param name="encoding">File encoding</param>
        /// <param name="fIncludeBOM">Indicates whether file contents should include the byte
        /// order mark</param>
        /// <param name="sBookId">The book id (if set, this will cause \id line to be written
        /// as the first line of the fiel)</param>
        /// <param name="lines">Remaining lines of the file</param>
        /// ------------------------------------------------------------------------------------
        public string MakeSfFile(Encoding encoding, bool fIncludeBOM, string sBookId,
                                 params string[] lines)
        {
            return MakeFile(CreateFileContents(fIncludeBOM, sBookId, lines), encoding);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the given filename to the collection of files that should be considered to
        /// exist and set its contents so it can be read.
        /// </summary>
        /// <param name="filename">The filename (may or may not include path).</param>
        /// <param name="contents">The contents of the file</param>
        /// <param name="encoding">File encoding</param>
        /// ------------------------------------------------------------------------------------
        public void AddFile(string filename, string contents, Encoding encoding)
        {
            m_existingFiles[filename] = new MockFileInfo(contents, encoding);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Simulates getting an exclusive (write) file lock on the file having the given filename.
        /// </summary>
        /// <param name="filename">The filename (must have been added previously).</param>
        /// ------------------------------------------------------------------------------------
        public void LockFile(string filename)
        {
            m_existingFiles[filename].Lock = FileLockType.Write;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Create the simulated file contents, suitable for calling MakeFile or AddFile.
        /// </summary>
        /// <param name="fIncludeBOM">Indicates whether file contents should include the byte
        /// order mark</param>
        /// <param name="sBookId">The book id (if set, this will cause \id line to be written
        /// as the first line of the fiel)</param>
        /// <param name="lines">Remaining lines of the file</param>
        /// ------------------------------------------------------------------------------------
        public static string CreateFileContents(bool fIncludeBOM, string sBookId, params string[] lines)
        {
            StringBuilder bldr = new StringBuilder();
            if (fIncludeBOM)
                bldr.Append("\ufeff");
            if (!String.IsNullOrEmpty(sBookId))
            {
                bldr.Append(@"\id ");
                bldr.AppendLine(sBookId);
            }
            foreach (string sLine in lines)
                bldr.AppendLine(sLine);
            bldr.Length = bldr.Length - Environment.NewLine.Length;

            return bldr.ToString();
        }
        #endregion

        #region IFileOS Members
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Determines whether the specified file exists. Looks in m_existingFiles.Keys without
        /// aming any adjustment for case or differences in normalization.
        /// </summary>
        /// <param name="sPath">The file path.</param>
        /// <returns></returns>
        /// ------------------------------------------------------------------------------------
        public bool FileExists(string sPath)
        {
            // Can't use Contains because it takes care of normalization mismatches, but for
            // the purposes of these tests, we want to simulate an Operating System which doesn't
            // (e.g., MS Windows).
            foreach (string sExistingFile in m_existingFiles.Keys)
            {
                if (sExistingFile == sPath)
                    return true;
            }
            return false;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Determines whether the specified directory exists.
        /// </summary>
        /// <param name="sPath">The directory path.</param>
        /// <returns></returns>
        /// ------------------------------------------------------------------------------------
        public bool DirectoryExists(string sPath)
        {
            // Can't use Contains because it takes care of normalization mismatches, but for
            // the purposes of these tests, we want to simulate an Operating System which doesn't
            // (e.g., MS Windows).
            foreach (string sExistingDir in m_existingDirectories)
            {
                if (sExistingDir == sPath)
                    return true;
            }
            return false;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the files in the given directory.
        /// </summary>
        /// <param name="sPath">The directory path.</param>
        /// <returns>list of files</returns>
        /// ------------------------------------------------------------------------------------
        public string[] GetFilesInDirectory(string sPath)
        {
            // These next two lines look a little strange, but I think we do this to deal with
            // normalization issues.
            int iDir = m_existingDirectories.IndexOf(sPath);
            string existingDir = m_existingDirectories[iDir];

            string[] files = new string[m_existingFiles.Count];
            int i = 0;
            foreach (string file in m_existingFiles.Keys)
                files[i++] = Common.PathCombine(existingDir, file);
            return files;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Opens a memory stream to read m_FileContents using the encoding (m_FileEncoding)
        /// that was set at the time the file was added.
        /// </summary>
        /// <param name="filename">Not used</param>
        /// <returns>A stream with read access</returns>
        /// ------------------------------------------------------------------------------------
        public Stream OpenStreamForRead(string filename)
        {
            MockFileInfo finfo = GetFileInfo(filename, FileLockType.Read);
            string fileContents = finfo.Contents;
            byte[] contents = new byte[fileContents.Length * 4];
            MemoryStream stream = new MemoryStream(contents, true);
            StreamWriter sw = new StreamWriter(stream, finfo.Encoding);
            sw.Write(fileContents);
            sw.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            finfo.AddStream(stream);
            return stream;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Opens a TextReader on the given file
        /// </summary>
        /// <param name="filename">The fully-qualified filename</param>
        /// <param name="encoding">The encoding to use for interpreting the contents</param>
        /// ------------------------------------------------------------------------------------
        public TextReader GetReader(string filename, Encoding encoding)
        {
            MockFileInfo finfo = GetFileInfo(filename, FileLockType.Read);
            Assert.AreEqual(finfo.Encoding, encoding);
            StringReader reader = new StringReader(finfo.Contents.TrimStart('\ufeff'));
            finfo.AddStream(reader);
            return reader;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Determines whether the given file is reable and writeable.
        /// </summary>
        /// <param name="filename">The fully-qualified file name</param>
        /// <returns><c>true</c> if the file is reable and writeable; <c>false</c> if the file
        /// does not exist, is locked, is read-only, or has permissions set such that the user
        /// cannot read or write it.</returns>
        /// ------------------------------------------------------------------------------------
        public bool IsFileReadableAndWritable(string filename)
        {
            return FileExists(filename) && GetFileInfo(filename).Lock == FileLockType.None;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Deletes the given file.
        /// This should probably be made case-insensitive
        /// </summary>
        /// <param name="filename">The fully-qualified file name</param>
        /// ------------------------------------------------------------------------------------
        public void Delete(string filename)
        {
            GetFileInfo(filename, FileLockType.Write);
            m_existingFiles.Remove(GetKey(filename));
        }
        #endregion

        #region Private helper methods
        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the file info for the given filename (Using a case-insensitive lookup).
        /// </summary>
        /// <param name="filename">The fully-qualified file name</param>
        /// <returns>The internal file information if the file is found in the list of
        /// existing files</returns>
        /// <exception cref="IOException">File not found</exception>
        /// ------------------------------------------------------------------------------------
        private MockFileInfo GetFileInfo(string filename)
        {
            return GetFileInfo(filename, FileLockType.None);
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the file info for the given filename (Using a case-insensitive lookup).
        /// </summary>
        /// <param name="filename">The fully-qualified file name</param>
        /// <param name="lockNeeded">Indicates what type of access is needed. If a
        /// permission is needed that is not available, throws an IOException</param>
        /// <returns>The internal file information if the file is found in the list of
        /// existing files</returns>
        /// <exception cref="IOException">File not found or locked</exception>
        /// ------------------------------------------------------------------------------------
        private MockFileInfo GetFileInfo(string filename, FileLockType lockNeeded)
        {
            MockFileInfo finfo = m_existingFiles[GetKey(filename)];
            if (lockNeeded == FileLockType.None)
                return finfo;

            switch (finfo.Lock)
            {
                case FileLockType.Read:
                    if (lockNeeded == FileLockType.Write)
                        throw new IOException("File " + filename + " is locked (open for read).");
                    break;
                case FileLockType.Write:
                    throw new IOException("File " + filename + " is locked (open for write).");
            }
            return finfo;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the key for the file info for the given filename (Using a case-insensitive lookup).
        /// </summary>
        /// <param name="filename">The fully-qualified file name</param>
        /// <returns>The given filename or a correctly cased variation of it to serve as a key
        /// </returns>
        /// <exception cref="IOException">File not found</exception>
        /// ------------------------------------------------------------------------------------
        private string GetKey(string filename)
        {
            if (m_existingFiles.ContainsKey(filename))
                return filename;
            string filenameLower = filename.ToLowerInvariant();
            foreach (string fileKey in m_existingFiles.Keys)
            {
                if (fileKey.ToLowerInvariant() == filenameLower)
                    return fileKey;
            }
            throw new IOException("File " + filename + " not found.");
        }
        #endregion
    }
}