using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using SIL.Tool;

namespace epubConvert
{
    public class EmbeddedFont
    {
        public static readonly Guid FontsFolder = new Guid("FD228CB7-AE11-4AE3-864C-16F3910AB8FE");

        private string _name, _weight, _style, _filename;
        private bool _serif, _silFont;

        public string Weight
        {
            get { return _weight; }
            protected set { _weight = value; }
        }
        public string Style
        {
            get { return _style; }
            protected set { _style = value; }
        }
        public string Filename
        {
            get { return _filename; }
            protected set { _filename = value; }
        }
        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        public bool Serif
        {
            get { return _serif; }
            protected set { _serif = value; }
        }

        public bool SILFont
        {
            get { return _silFont; }
            protected set { _silFont = value; }
        }

        /// <summary>
        /// Constructor for an Embedded Font. The constructor attempts to fill out the
        /// information on the given font family, including where the font is found on
        /// the current computer.
        /// </summary>
        /// <param name="name"></param>
        public EmbeddedFont(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }             
            Name = name;
            if (!SilFontLookup(name))
            {
                // This isn't an SIL font. Try to find the font info if we can
                SILFont = false;
                Weight = "normal";
                Style = "normal";
                Filename = FontInternals.GetFontFileName(Name, Style);
            }
        }

        #region private_methods
        /// <summary>
        /// Fills out the EmbeddedFont information if this happens to be an SIL font, or returns false if the
        /// font can't be found in the SIL lookup. This list is hard-coded from the SIL site
        /// (http://www.sil.org/computing/catalog/show_software_catalog.asp?by=cat&name=Font); if the list changes, 
        /// you will need to update this method AND the list in FontWarningDlg.cs (that populates the drop-down).
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns></returns>
        private bool SilFontLookup(string fontName)
        {
            bool bFound = false;
            switch (fontName)
            {
                case "Abyssinica":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "Abyssinica_SIL.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                case "Andika":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "AndBasR.ttf";
                    SILFont = true;
                    Serif = false;
                    break;
                case "Apparatus SIL":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "AppSILR.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                case "Charis SIL":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "CharisSILR.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                case "Dai Banna":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "DBSILBR.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                case "Doulos SIL":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "DoulosSILR.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                case "Ezra":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "SILEOT_0.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                case "Galatia":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "GalSILR201.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                case "Gentium":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "GenBasR.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                case "Lateef":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "LateefRegOT.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                case "Nuosu":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "NuosuSIL.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                case "Padauk":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "Padauk.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                case "Scheharazade":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "ScheherazadeRegOT.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                case "Sophia Nubian":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "SNR.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                case "Tai Heritage Pro":
                    bFound = true;
                    Weight = "normal";
                    Style = "normal";
                    Filename = "TaiHeritagePro.ttf";
                    SILFont = true;
                    Serif = true;
                    break;
                default:
                    break;
            }
            return bFound;
        }

        #endregion

        #region static_methods
        // older call
        [DllImport("shell32.dll")]
        private static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken,
            uint dwFlags, [Out] StringBuilder pszPath);

        // newer (Vista and later) call to get a known folder
        [DllImport("shell32.dll")]
        static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags,
            IntPtr hToken, out IntPtr pszPath);

        /// <summary>
        /// Returns the font folder path for this workstation, even on non-US locales.
        /// </summary>
        /// <returns></returns>
        public static string GetFontFolderPath()
        {
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
                StringBuilder sb = new StringBuilder();
                SHGetFolderPath(IntPtr.Zero, 0x0014, IntPtr.Zero, 0x0000, sb);
                return sb.ToString();
            }
        }
        #endregion
    }



}