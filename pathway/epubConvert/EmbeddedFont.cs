using System;
using System.Diagnostics;
using System.IO;
using SIL.Tool;

namespace epubConvert
{
    public class EmbeddedFont
    {
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

        public override string ToString()
        {
            return Name;
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
            Weight = "normal";
            Style = "normal";
            // edb 12/9/2010 BUGBUG:
            // need a way to query the font for its generic serif/sans-serif characteristics
            // this works for SIL fonts only because there's currently one sans-serif font (Andika)
            Serif = (name.Contains("Andika")? false: true);
            Filename = FontInternals.GetFontFileName(name, Style);
            if (Filename == null)
            {
                // this font isn't installed
                return;
            }
            SILFont = FontInternals.IsSILFont(Path.Combine(FontInternals.GetFontFolderPath(), Filename));
        }

        #region static_methods
        /// <summary>
        /// Returns whether the given font is installed on this system. This can either be the
        /// font Filename (e.g., "Arial.ttf") or the font family / user friendly name of a font.
        /// (The filename lookup is faster.)
        /// </summary>
        /// <param name="font">Font family or font filename</param>
        /// <returns>true if font is installed</returns>
        public static bool IsInstalled (string font)
        {
            if (font.EndsWith(".ttf") || font.EndsWith(".otf"))
            {
                // quick check - this font should reside in the fonts folder
                String fullFilename = Path.Combine(FontInternals.GetFontFolderPath(), font);
                return (File.Exists(fullFilename));
            }
            else
            {
                // longer check - instantiate an embedded font and test the filename
                var embeddedFont = new EmbeddedFont(font);
                if (embeddedFont.Filename == null)
                {
                    // not an SIL font, not on the system
                    return false;
                }
                String fullFilename = Path.Combine(FontInternals.GetFontFolderPath(), embeddedFont.Filename);
                return (File.Exists(fullFilename));
            }
        }

        #endregion
    }



}