// --------------------------------------------------------------------------------------
// <copyright file="EmbeddedFont.cs" from='2009' to='2011' company='SIL International'>
//      Copyright © 2010, 2011 SIL International. All Rights Reserved.
//
//      Distributable under the terms of specified in the LICENSING.txt file.
// </copyright>
// <author>Erik Brommers</author>
// <email>erik_brommers@sil.org</email>
// Last reviewed:
// 
// <remarks>
// Embedded Font processing for .epub files
//
// .epub files allow you to include fonts for more accurate rendering of non-western
// code points in e-book readers. This class allows us to keep track of the details of
// a font -- whether it's freely redistributable, what the font's filename is, whether
// it has a bold or italic variant -- so that we can build up font lists in the 
// exportepub class.
// </remarks>
// --------------------------------------------------------------------------------------

using System;
using SIL.Tool;

namespace epubConvert
{
    public class EmbeddedFont
    {
        private string _name, _filename, _italicFontname, _boldFontname;
        private bool _serif, _canRedistribute, _hasItalic, _hasBold;
		
        /// <summary>
        /// Name of the font file, including the path.
        /// </summary>
        public string Filename
        {
            get { return _filename; }
            protected set { _filename = value; }
        }
        /// <summary>
        /// Font family name
        /// </summary>
        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }
        /// <summary>
        /// Name of the italic variant font file, including the path.
        /// </summary>
        public string ItalicFilename
        {
            get { return _italicFontname; }
            protected set { _italicFontname = value; }
        }
        /// <summary>
        /// Name of the bold variant font file, including the path.
        /// </summary>
        public string BoldFilename
        {
            get { return _boldFontname;  }
            protected set { _boldFontname = value; }
        }
        /// <summary>
        /// Returns whether this font is a serif font. (See notes in the constructor - this is hard coded for now)
        /// </summary>
        public bool Serif
        {
            get { return _serif; }
            protected set { _serif = value; }
        }
        /// <summary>
        /// Returns whether this font has a license that allows us to embed it freely. SIL fonts fall under this category,
        /// as do fonts with GPL and OFL licensing. This flag looks at the license agreement embedded in the Copyright
        /// field of the font itself.
        /// </summary>
        public bool CanRedistribute
        {
            get { return _canRedistribute; }
            protected set { _canRedistribute = value; }
        }
        /// <summary>
        /// Returns whether this font family has an Italic font variant installed on this system.
        /// </summary>
        public bool HasItalic
        {
            get { return _hasItalic; }
            protected set { _hasItalic = value;  }
        }
        /// <summary>
        /// Returns whether this font family has a Bold font family font installed on this system.
        /// </summary>
        public bool HasBold
        {
            get { return _hasBold; }
            protected set { _hasBold = value; }
        }
        /// <summary>
        /// Returns the font family name.
        /// </summary>
        /// <returns></returns>
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
            // edb 12/9/2010 BUGBUG:
            // need a way to query the font for its generic serif/sans-serif characteristics
            // this works for SIL fonts only because there's currently one sans-serif font (Andika)
            Serif = (name.Contains("Andika")? false: true);
			Filename = FontInternals.GetFontFileName(name, "normal");
            ItalicFilename = FontInternals.GetFontFileName(name, "Italic");
            HasItalic = (ItalicFilename == null) ? false : true;
            BoldFilename = FontInternals.GetFontFileName(name, "Bold");
            HasBold = (BoldFilename == null) ? false : true;
            if (Filename == null)
            {
                // this font isn't installed
                return;
            }
            CanRedistribute = FontInternals.IsFreeFont(Filename);
        }
    }



}