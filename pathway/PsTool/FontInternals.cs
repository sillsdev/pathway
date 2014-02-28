// --------------------------------------------------------------------------------------------
// <copyright file="FontInternals.cs" from='2010' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Code in this module taken from
// http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/d3ec5763-b7e8-4a29-9c50-1d6576ae6eae
// see also: http://scripts.sil.org/cms/scrIpts/page.php?site_id=nrsi&item_id=IWS-Chapter08
// and http://www.microsoft.com/typography/otspec/otff.htm
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace SIL.Tool
{
    [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
    struct TT_OFFSET_TABLE
    {
        public ushort uMajorVersion;
        public ushort uMinorVersion;
        public ushort uNumOfTables;
        public ushort uSearchRange;
        public ushort uEntrySelector;
        public ushort uRangeShift;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
    struct SFNT
    {
        public byte szSfnt1;
        public byte szSfnt2;
        public byte szSfnt3;
        public byte szSfnt4;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
    struct OT_OFFSET_TABLE
    {
        public ushort uNumOfTables;
        public ushort uSearchRange;
        public ushort uEntrySelector;
        public ushort uRangeShift;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
    struct TTC_HEADER_1
    {
        public ushort uMajorVersion;
        public ushort uMinorVersion;
        public UInt32 uNumFonts;  //C# uint = spec ulong
    }
    [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
    struct TT_TABLE_DIRECTORY
    {
        public char szTag1;
        public char szTag2;
        public char szTag3;
        public char szTag4;
        public uint uCheckSum; //Check sum
        public uint uOffset; //Offset from beginning of file
        public uint uLength; //length of the table in bytes
    }
    [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
    struct TT_NAME_TABLE_HEADER
    {
        public ushort uFSelector;
        public ushort uNRCount;
        public ushort uStorageOffset;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 0x1)]
    struct TT_NAME_RECORD
    {
        public ushort uPlatformID;
        public ushort uEncodingID;
        public ushort uLanguageID;
        public ushort uNameID;
        public ushort uStringLength;
        public ushort uStringOffset;
    }

    public static class FontInternals
    {
        // Windows only - Guid for Fonts folder
        public static readonly Guid FontsFolder = new Guid("FD228CB7-AE11-4AE3-864C-16F3910AB8FE");

        private static TT_OFFSET_TABLE ttOffsetTable;
        private static SFNT sFnt;
        private static OT_OFFSET_TABLE otOffsetTable;
        private static TTC_HEADER_1 ttcHeader;
        private static UInt32 uIntOffset;
        private static UInt32[] ttcOffsets;
        private static TT_TABLE_DIRECTORY tblDir;
        private static TT_NAME_TABLE_HEADER ttNTHeader;
        private static TT_NAME_RECORD ttNMRecord;

        public static string GetPostscriptName(string familyName, string style)
        {
            string fontName = GetFontFileName(familyName, style);
            return GetPostscriptName(fontName);
        }

        public static string GetPostscriptName(string fontFullName)
        {
            if (!File.Exists(fontFullName)) return string.Empty;

            FileStream fs = new FileStream(fontFullName, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            TT_OFFSET_TABLE ttResult = GetOffsetTable(r);

            //ttf Must be maj =1 minor = 0
            int numTables = 0;
            if (ttResult.uMajorVersion == 1 && ttResult.uMinorVersion == 0)
            {
                numTables = ttResult.uNumOfTables;
            }
            else
            {
                fs.Position = 0L;
                SFNT sFnt = GetSfnt(r);
                string szSfnt = string.Format("{0}{1}{2}{3}", (char)sFnt.szSfnt1, (char)sFnt.szSfnt2, (char)sFnt.szSfnt3, (char)sFnt.szSfnt4);
                switch (szSfnt)
                {
                    case "OTTO":
                        OT_OFFSET_TABLE otResult = GetOpenTypeOffsetTable(r);
                        numTables = otResult.uNumOfTables;
                        break;
                    case "ttcf":
                        TTC_HEADER_1 ttcHeader = GetTtcHeader(r);
                        if ((ttcHeader.uMajorVersion != 1 && ttcHeader.uMajorVersion != 2) || ttcHeader.uMinorVersion != 0)
                            return string.Empty;
                        GetTtcOffsets(r, ttcHeader.uNumFonts);
                        fs.Position = long.Parse(ttcOffsets[0].ToString());
                        ttResult = GetOffsetTable(r);
                        if (ttResult.uMajorVersion == 1 && ttResult.uMinorVersion == 0)
                        {
                            numTables = ttResult.uNumOfTables;
                        }
                        else
                        {
                            return string.Empty;
                        }
                        break;
                    default:
                        return string.Empty;
                }
            }

            bool bFound = false;
            TT_TABLE_DIRECTORY tbName = new TT_TABLE_DIRECTORY();
            for (int i = 0; i < numTables; i++)
            {
                tbName = GetNameTable(r);
                string szName = tbName.szTag1.ToString() + tbName.szTag2.ToString() + tbName.szTag3.ToString() + tbName.szTag4.ToString();
                if (szName != null)
                {
                    if (szName == "name")
                    {
                        bFound = true;
                        tbName.uLength = BigEndianValue(tbName.uLength);
                        tbName.uOffset = BigEndianValue(tbName.uOffset);
                        break;
                    }
                }
            }
            if (bFound)
            {
                fs.Position = tbName.uOffset;
                TT_NAME_TABLE_HEADER ttNTResult = GetNameTableHeader(r);
                for (int i = 0; i < ttNTResult.uNRCount; i++)
                {
                    TT_NAME_RECORD ttNMResult = GetNameRecord(r);
                    const int PostscriptNameId = 6;
                    if (ttNMResult.uNameID == PostscriptNameId)
                    {
                        fs.Position = tbName.uOffset + ttNMResult.uStringOffset + ttNTResult.uStorageOffset;
                        char[] szResult = r.ReadChars(ttNMResult.uStringLength);
                        string result = "";
                        // Usually the uEncodingID will tell us whether we're using single or double-byte encoding,
                        // but sometimes it lies. Verify by testing the first character in the array for '\0'
                        int uId = ttNMResult.uEncodingID;
                        if (szResult[0] == '\0')
                        {
                            uId = 3;
                        }
                        for (int j = 0; j < ttNMResult.uStringLength; j++)
                        {
                            switch (uId)
                            {
                                case 0: // SIL Fonts use this encoding
                                    result += szResult[j];
                                    break;
                                case 3: // Windows Fonts use this encoding (first byte is NUL)
                                    j++;
                                    if (j < ttNMResult.uStringLength)
                                    {
                                        result += szResult[j];
                                    }
                                    break;
                            }
                        }
                        return result;
                    }
                }
            }
            return string.Empty;
        }

        private static TT_NAME_RECORD GetNameRecord(BinaryReader r)
        {
            byte[] btNMRecord = r.ReadBytes(Marshal.SizeOf(ttNMRecord));
            btNMRecord = BigEndian(btNMRecord);
            IntPtr ptrNMRecord = Marshal.AllocHGlobal(btNMRecord.Length);
            Marshal.Copy(btNMRecord, 0x0, ptrNMRecord, btNMRecord.Length);
            TT_NAME_RECORD ttNMResult = (TT_NAME_RECORD)Marshal.PtrToStructure(ptrNMRecord, typeof(TT_NAME_RECORD));
            Marshal.FreeHGlobal(ptrNMRecord);
            return ttNMResult;
        }

        private static TT_NAME_TABLE_HEADER GetNameTableHeader(BinaryReader r)
        {
            byte[] btNTHeader = r.ReadBytes(Marshal.SizeOf(ttNTHeader));
            btNTHeader = BigEndian(btNTHeader);
            IntPtr ptrNTHeader = Marshal.AllocHGlobal(btNTHeader.Length);
            Marshal.Copy(btNTHeader, 0x0, ptrNTHeader, btNTHeader.Length);
            TT_NAME_TABLE_HEADER ttNTResult = (TT_NAME_TABLE_HEADER)Marshal.PtrToStructure(ptrNTHeader, typeof(TT_NAME_TABLE_HEADER));
            Marshal.FreeHGlobal(ptrNTHeader);
            return ttNTResult;
        }

        private static TT_TABLE_DIRECTORY GetNameTable(BinaryReader r)
        {
            TT_TABLE_DIRECTORY tbName;
            if (Common.UsingMonoVM)
            {
                // char size is 1 byte in TTF file for Mono?
                // try copying the structure over manually
                byte[] bNameTable = r.ReadBytes(4); // chars
                tbName.szTag1 = Convert.ToChar(bNameTable[0]);
                tbName.szTag2 = Convert.ToChar(bNameTable[1]);
                tbName.szTag3 = Convert.ToChar(bNameTable[2]);
                tbName.szTag4 = Convert.ToChar(bNameTable[3]);
                tbName.uCheckSum = r.ReadUInt32();
                tbName.uOffset = r.ReadUInt32();
                tbName.uLength = r.ReadUInt32();
            }
            else
            {
                byte[] bNameTable = r.ReadBytes(Marshal.SizeOf(tblDir));
                IntPtr ptrName = Marshal.AllocHGlobal(bNameTable.Length);
                Marshal.Copy(bNameTable, 0x0, ptrName, bNameTable.Length);
                tbName = (TT_TABLE_DIRECTORY)Marshal.PtrToStructure(ptrName, typeof(TT_TABLE_DIRECTORY));
                Marshal.FreeHGlobal(ptrName);
            }
            return tbName;
        }

        private static TT_OFFSET_TABLE GetOffsetTable(BinaryReader r)
        {
            byte[] buff = r.ReadBytes(Marshal.SizeOf(ttOffsetTable));
            buff = BigEndian(buff);
            IntPtr ptr = Marshal.AllocHGlobal(buff.Length);
            Marshal.Copy(buff, 0x0, ptr, buff.Length);
            TT_OFFSET_TABLE ttResult = (TT_OFFSET_TABLE)Marshal.PtrToStructure(ptr, typeof(TT_OFFSET_TABLE));
            Marshal.FreeHGlobal(ptr);
            return ttResult;
        }

        private static SFNT GetSfnt(BinaryReader r)
        {
            byte[] buff = r.ReadBytes(Marshal.SizeOf(sFnt));
            IntPtr ptr = Marshal.AllocHGlobal(buff.Length);
            Marshal.Copy(buff, 0x0, ptr, buff.Length);
            sFnt = (SFNT)Marshal.PtrToStructure(ptr, typeof(SFNT));
            Marshal.FreeHGlobal(ptr);
            return sFnt;
        }

        private static OT_OFFSET_TABLE GetOpenTypeOffsetTable(BinaryReader r)
        {
            byte[] buff = r.ReadBytes(Marshal.SizeOf(otOffsetTable));
            buff = BigEndian(buff);
            IntPtr ptr = Marshal.AllocHGlobal(buff.Length);
            Marshal.Copy(buff, 0x0, ptr, buff.Length);
            OT_OFFSET_TABLE otResult = (OT_OFFSET_TABLE)Marshal.PtrToStructure(ptr, typeof(OT_OFFSET_TABLE));
            Marshal.FreeHGlobal(ptr);
            return otResult;
        }

        private static TTC_HEADER_1 GetTtcHeader(BinaryReader r)
        {
            byte[] buff = r.ReadBytes(Marshal.SizeOf(ttcHeader));
            IntPtr ptr = Marshal.AllocHGlobal(buff.Length);
            Marshal.Copy(buff, 0x0, ptr, buff.Length);
            TTC_HEADER_1 ttcResult = (TTC_HEADER_1)Marshal.PtrToStructure(ptr, typeof(TTC_HEADER_1));
            Marshal.FreeHGlobal(ptr);
            ttcResult.uMajorVersion = BigEndianValue(ttcResult.uMajorVersion);
            ttcResult.uMinorVersion = BigEndianValue(ttcResult.uMinorVersion);
            ttcResult.uNumFonts = BigEndianValue(ttcResult.uNumFonts);
            return ttcResult;
        }

        private static UInt32[] GetTtcOffsets(BinaryReader r, UInt32 numFonts)
        {
            ttcOffsets = new UInt32[numFonts];
            byte[] buff = r.ReadBytes(Marshal.SizeOf(uIntOffset) * (int)numFonts);
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(uIntOffset));
            for (int i = 0; i < numFonts; i++)
            {
                Marshal.Copy(buff, i * Marshal.SizeOf(uIntOffset), ptr, Marshal.SizeOf(uIntOffset));
                ttcOffsets[i] = (UInt32)Marshal.PtrToStructure(ptr, typeof(UInt32));
                ttcOffsets[i] = BigEndianValue(ttcOffsets[i]);
            }
            Marshal.FreeHGlobal(ptr);
            return ttcOffsets;
        }

        private static byte[] BigEndian(byte[] bLittle)
        {
            byte[] bBig = new byte[bLittle.Length];
            for (int y = 0; y < (bLittle.Length - 1); y += 2)
            {
                byte b1, b2;
                b1 = bLittle[y];
                b2 = bLittle[y + 1];
                bBig[y] = b2;
                bBig[y + 1] = b1;
            }
            return bBig;
        }

        private static UInt16 BigEndianValue(UInt16 val)
        {
            byte[] btValue = BitConverter.GetBytes(val);
            Array.Reverse(btValue);
            return BitConverter.ToUInt16(btValue, 0);
        }

        private static UInt32 BigEndianValue(UInt32 val)
        {
            byte[] btValue = BitConverter.GetBytes(val);
            Array.Reverse(btValue);
            return BitConverter.ToUInt32(btValue, 0);
        }

        public static string[] GetInstalledFontFiles()
        {
            string fontFolder = GetFontFolderPath();
            return Directory.GetFiles(fontFolder, "*.ttf", SearchOption.AllDirectories);
        }

        public static string GetFontFileName(string familyName, string style)
        {
            // Linux lookup
            if (Common.UsingMonoVM)
            {
                // Linux fonts are not listed in the registry; instead, linux (and mono) use the fontconfig library to
                // provide support. 
                // To find the font, we'll make a call to fc-list (one of the fontconfig commands) for the font, and parse the
                // results for the filename
                const string prog = "fc-list";
                var args = new StringBuilder();
                args.Append("-v \"");
                args.Append(familyName);
                if (style.Length == 0 || style.ToLower().Equals("normal"))
                {
                    args.Append(":style=Regular\"");
                }
                else
                {
                    args.Append(":style=");
                    args.Append(style);
                    args.Append("\"");
                }
                string stdOut = string.Empty;
                string stdErr = string.Empty;
                SubProcess.Run(Directory.GetCurrentDirectory(), prog, args.ToString(), out stdOut, out stdErr);
                if (stdOut.Length < 1)
                {
                    return string.Empty;
                }
                // call returned successfully -- read the results
                var start = stdOut.IndexOf("file: \"");
                if (start > 0)
                {
                    start += 7;
                    var stop = stdOut.IndexOf("\"", start);
                    if (stop > 0)
                    {
                        return stdOut.Substring(start, (stop - start));
                    }
                    return string.Empty;
                }

                return string.Empty;
            }

            // Windows lookup
            RegistryKey fontsKey;
            Dictionary<string, string> fontFileNames = new Dictionary<string, string>();
            try
            {
                fontsKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MICROSOFT\WINDOWS NT\CURRENTVERSION\Fonts\");
                string[] fontNames = fontsKey.GetValueNames();
                for (int i = 0; i < fontNames.Length; i++)
                {
                    if (fontNames[i].IndexOf(familyName) < 0) continue;
                    string fontFileName = fontsKey.GetValue(fontNames[i]) as string;
                    fontFileNames[fontNames[i]] = fontFileName;
                }
            }
            catch (Exception)
            {
            }
            string resultFileName = string.Empty;
            switch (fontFileNames.Count)
            {
                case 1:
                    string[] values = new string[1];
                    fontFileNames.Values.CopyTo(values, 0);
                    resultFileName = values[0];
                    break;
                case 0:
                    return null;
                default:
                    string pattern = string.Format("{0} {1} (", familyName, style);
                    resultFileName = SearchFontFileNames(pattern, fontFileNames);
                    if (resultFileName != string.Empty) break;
                    pattern = string.Format("{0} (", familyName);
                    resultFileName = SearchFontFileNames(pattern, fontFileNames);
                    if (resultFileName != string.Empty) break;
                    resultFileName = SearchFontFileNames(familyName, fontFileNames);
                    break;
            }
            return Common.PathCombine(GetFontFolderPath(), resultFileName);
        }

        private static string SearchFontFileNames(string partialName, Dictionary<string, string> fontFileNames)
        {
            foreach (string key in fontFileNames.Keys)
            {
                if (key.IndexOf(partialName) < 0) continue;
                return fontFileNames[key];
            }
            return string.Empty;
        }

        public static bool IsGraphite(string familyName, string style)
        {
            string fontName = GetFontFileName(familyName, style);
            return IsGraphite(fontName);
        }

        public static bool IsGraphite(string fontFullName)
        {
            if (!File.Exists(fontFullName))
            {
                return false;
            }
            FileStream fs = new FileStream(fontFullName, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            TT_OFFSET_TABLE ttResult = GetOffsetTable(r);

            //Must be maj =1 minor = 0
            if (ttResult.uMajorVersion != 1 || ttResult.uMinorVersion != 0)
                return false;

            TT_TABLE_DIRECTORY tbName = new TT_TABLE_DIRECTORY();
            for (int i = 0; i < ttResult.uNumOfTables; i++)
            {
                tbName = GetNameTable(r);
                string szName = tbName.szTag1.ToString() + tbName.szTag2.ToString() + tbName.szTag3.ToString() + tbName.szTag4.ToString();
                if (szName != null)
                {
                    if (szName == "Silf")
                        return true;
                }
            }
            return false;
        }

        public static string GetFontCopyright(string fontFullName)
        {
            if (!File.Exists(fontFullName))
            {
                return string.Empty;
            }
            FileStream fs = new FileStream(fontFullName, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            TT_OFFSET_TABLE ttResult = GetOffsetTable(r);

            //Must be maj =1 minor = 0
            if (ttResult.uMajorVersion != 1 || ttResult.uMinorVersion != 0)
                return string.Empty;

            TT_TABLE_DIRECTORY tbName = new TT_TABLE_DIRECTORY();
            bool bFound = false;
            for (int i = 0; i < ttResult.uNumOfTables; i++)
            {
                tbName = GetNameTable(r);
                string szName = tbName.szTag1.ToString() + tbName.szTag2.ToString() + tbName.szTag3.ToString() + tbName.szTag4.ToString();
                if (szName != null)
                {
                    if (szName == "name")
                    {
                        bFound = true;
                        tbName.uLength = BigEndianValue(tbName.uLength);
                        tbName.uOffset = BigEndianValue(tbName.uOffset);
                        break;
                    }
                }
            }
            if (bFound)
            {
                try
                {
                    fs.Position = tbName.uOffset;
                    TT_NAME_TABLE_HEADER ttNTResult = GetNameTableHeader(r);
                    string result = "";
                    for (int i = 0; i < ttNTResult.uNRCount; i++)
                    {
                        TT_NAME_RECORD ttNMResult = GetNameRecord(r);
                        const int CopyrightId = 0;
                        if (ttNMResult.uNameID == CopyrightId)
                        {
                            fs.Position = tbName.uOffset + ttNMResult.uStringOffset + ttNTResult.uStorageOffset;
                            char[] szResult = r.ReadChars(ttNMResult.uStringLength);
                            // Usually the uEncodingID will tell us whether we're using single or double-byte encoding,
                            // but sometimes it lies. Verify by testing the first character in the array for '\0'
                            int uId = ttNMResult.uEncodingID;
                            if (szResult[0] == '\0')
                            {
                                uId = 3;
                            }
                            for (int j = 0; j < ttNMResult.uStringLength; j++)
                            {
                                switch (uId)
                                {
                                    case 0: // SIL Fonts use this encoding (but sometimes lie)
                                        result += szResult[j];
                                        break;
                                    case 3: // Windows Fonts use this encoding (first byte is NUL)
                                        j++;
                                        if (j < ttNMResult.uStringLength)
                                        {
                                            result += szResult[j];
                                        }
                                        break;
                                }
                            }
                            break;
                        }
                    }
                    return result;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns whether this font is licensed under a freely redistributable license. Currently
        /// this is a best-effort method, as the copyright string (and name) don't always give us
        /// a clear picture.
        /// </summary>
        /// <param name="fontFullName"></param>
        /// <returns></returns>
        public static bool IsFreeFont(string fontFullName)
        {
            Dictionary<string, string> _dt = new Dictionary<string, string>();
            using (InstalledFontCollection col = new InstalledFontCollection())
            {
                foreach (FontFamily fa in col.Families)
                {
                    _dt.Add(fa.Name,fa.Name);
                }
            }

            // try looking at the copyright
            CheckUserFileAccessRights rights = new CheckUserFileAccessRights(fontFullName);
            if (rights.canRead())
            {
                string copyright = GetFontCopyright(fontFullName);
                if (copyright.Length > 0)
                {
                    if (copyright.Contains("SIL") || copyright.Contains("Summer Institute of Linguistics"))
                    {
                        // SIL fonts
                        return true;
                    }
                    // GPL / OFL license
                    if (copyright.ToLower().Contains("general public license") ||
                        copyright.ToLower().Contains("open font license"))
                    {
                        return true;
                    }
                    // redistributable Creative Commons licenses
                    if (copyright.ToLower().Contains("cc by-sa") || copyright.ToLower().Contains("cc by ") ||
                        copyright.ToLower().Contains("cc by-nd") || copyright.ToLower().Contains("cc by-nc") ||
                        copyright.ToLower().Contains("cc by-nc-sa") || copyright.ToLower().Contains("cc by-nc-nd") ||
                        copyright.ToLower().Contains("cc0"))
                    {
                        return true;
                    }
                }
            }
            // TODO: known free fonts that don't match the copyright string tests above can be tested here by looking for their
            // font names. This will probably be done on an as-needed basis.

            // can't determine - return false
            return false;
        }

        /// <summary>
        /// Returns whether this font was created by SIL International.
        /// </summary>
        /// <param name="fontFullName"></param>
        /// <returns></returns>
        public static bool IsSILFont(string fontFullName)
        {
            CheckUserFileAccessRights rights = new CheckUserFileAccessRights(fontFullName);
            if (rights.canRead())
            {
                string result = GetFontCopyright(fontFullName);
                if (result != "")
                {
                    // we got something out of the CopyrightId slot - does it contain "SIL" or "Summer Institute of Linguistics"?
                    if (result.Contains("SIL") || result.Contains("Summer Institute of Linguistics"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns whether the given font is installed on this system. This can either be the
        /// font Filename (e.g., "Arial.ttf") or the font family / user friendly name of a font.
        /// (The filename lookup is faster.)
        /// </summary>
        /// <param name="font">Font family or font filename</param>
        /// <returns>true if font is installed</returns>
        public static bool IsInstalled(string font)
        {
            // first check - filename lookup in the font folder
            if (font.EndsWith(".ttf") || font.EndsWith(".otf"))
            {
                if (Common.UsingMonoVM)
                {
                    // mono - find the font filename using linux "find"
                    var args = new StringBuilder();
                    args.Append("-name \"");
                    args.Append(font);
                    args.Append("\"");
                    string stdOut = string.Empty;
                    string stdErr = string.Empty;
                    SubProcess.Run("/", "find", args.ToString(), out stdOut, out stdErr);
                    // if stdOut returns something, the file is installed
                    return (stdOut.Length > 0);
                }
                // Windows / first check - this font should reside in the fonts folder
                String fullFilename = Common.PathCombine(GetFontFolderPath(), font);
                return (File.Exists(fullFilename));
            }
            // second check - look up the filename and see if the file exists
            string filename = GetFontFileName(font, "normal");
            if (filename != null)
            {
                if (File.Exists(filename)) return true;
            }
            // third check - look through the InstalledFontCollection
            var ifc = new InstalledFontCollection();
            foreach (FontFamily family in ifc.Families)
            {
                if (family.Name.Contains(font)) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the font folder path for this workstation, even on non-US locales. 
        /// </summary>
        /// <returns></returns>
        public static string GetFontFolderPath()
        {
            if (Common.UsingMonoVM)
            {
                return "/usr/share/fonts/truetype";
            }
            OperatingSystem osInfo = Environment.OSVersion;
            if (osInfo.Platform == PlatformID.Win32NT && osInfo.Version.Major >= 6)
            {
                // Vista or later - support for newer SHGetKnownFolderPath
                var ptr = IntPtr.Zero;
                try
                {
                    var ret = SHGetKnownFolderPath(FontsFolder, 0x0000, IntPtr.Zero, out ptr);
                    if (ret != 0)
                    {
                        throw Marshal.GetExceptionForHR(ret);
                    }
                    return Marshal.PtrToStringUni(ptr);

                }
                finally
                {
                    Marshal.FreeCoTaskMem(ptr);
                }
            }
            else
            {
                // older SHGetFolderPath
                var sb = new StringBuilder();
                SHGetFolderPath(IntPtr.Zero, 0x0014, IntPtr.Zero, 0x0000, sb);
                return sb.ToString();
            }
        }

        #region Windows_Interop_Methods
        // older call
        [DllImport("shell32.dll")]
        private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken,
            uint dwFlags, [Out] StringBuilder pszPath);

        // newer (Vista and later) call to get a known folder
        [DllImport("shell32.dll")]
        static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags,
            IntPtr hToken, out IntPtr pszPath);
        #endregion
    }
}
