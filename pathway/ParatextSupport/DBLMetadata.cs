using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using SIL.Tool;

namespace SIL.PublishingSolution
{
	public class DBLMetadata
	{
	    public string StyFullPath { get; set; }
        private string _cssFullPath;
        private Dictionary<string, Dictionary<string, string>> _styleInfo = new Dictionary<string, Dictionary<string, string>>();
		private Dictionary<string, string> _cssProp;
        protected XmlTextWriter _writer;

		/// ------------------------------------------------------------
		/// <summary>
		/// Convert a Paratext sty file to a CSS.
		/// </summary>
		/// <param name="database">The settings for the Paratext database.
		/// </param>
		/// <param name="cssFullPath">The XML full path.</param>
        /// ------------------------------------------------------------
		public void CreateMetadata(string database, string cssFullPath)
        {

            //StyFullPath = styFullPath;
            _cssFullPath = cssFullPath;
            FindStyFile(database);
            CreateFile();
		    WriteXML();
		    //ParseFile();
		    //WriteCSS();
            CloseFile();
        }

        /// <summary>
        /// Override to convert the sty file to XML, assuming the
        /// sty file is already set for this class.
        /// </summary>
        /// <param name="cssFullPath"></param>
        public void CreateMetadata(string cssFullPath)
        {
            _cssFullPath = cssFullPath;
            CreateFile();
            WriteXML();
            //ParseFile();
            //WriteCSS();
            CloseFile();
        }


        /// ------------------------------------------------------------------------
        /// <summary>
        /// Write Metadata.xml
        /// </summary>
        /// ------------------------------------------------------------------------
        private void WriteXML()
        {
            WriteBookNames(); // Dynamic

            StaticCode1();

            OTBooks();
            DCBooks();
            NTBooks();

            StaticCode2();

            OTBooks();
            NTBooks();
            
            StaticCode3();
        }

	    

	    private void NTBooks()
	    {
	        _writer.WriteStartElement("division");
	        _writer.WriteAttributeString("id", "NT");
	        _writer.WriteStartElement("books");
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "MAT");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "MRK");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "LUK");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JHN");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ACT");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ROM");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1CO");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2CO");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "GAL");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "EPH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "PHP");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "COL");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1TH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2TH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1TI");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2TI");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "TIT");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "PHM");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "HEB");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JAS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1PE");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2PE");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1JN");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2JN");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "3JN");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JUD");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "REV");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	    }

	    private void DCBooks()
	    {
	        _writer.WriteStartElement("division");
	        _writer.WriteAttributeString("id", "DC");
	        _writer.WriteStartElement("books");
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "TOB");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JDT");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ESG");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "WIS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "SIR");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "BAR");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "LJE");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1MA");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2MA");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1ES");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2ES");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "MAN");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	    }

	    private void OTBooks()
	    {
	        _writer.WriteStartElement("division");
	        _writer.WriteAttributeString("id", "OT");
	        _writer.WriteStartElement("books");
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "GEN");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "EXO");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "LEV");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "NUM");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "DEU");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JOS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JDG");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "RUT");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1SA");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2SA");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1KI");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2KI");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1CH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2CH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "EZR");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "NEH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "EST");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JOB");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "PSA");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "PRO");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ECC");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "SNG");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ISA");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JER");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "LAM");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "EZK");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "DAN");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "HOS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JOL");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "AMO");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "OBA");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JON");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "MIC");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "NAM");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "HAB");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ZEP");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "HAG");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ZEC");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "MAL");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	    }

	    private void WriteBookNames()
	    {
	        _writer.WriteStartElement("bookNames");
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "GEN");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("GENESIS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Genesis");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Gn");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "EXO");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("EXODUS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Exodus");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ex");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "LEV");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("LEVITICUS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Leviticus");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Lv");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "NUM");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("NUMBERS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Numbers");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Nu");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "DEU");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("DEUTERONOMY");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Deuteronomy");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Dt");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JOS");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of JOSHUA");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Joshua");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Js");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JDG");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of JUDGES");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Judges");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Jg");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "RUT");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of RUTH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Ruth");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ru");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1SA");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The First Book of SAMUEL");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("1 Samuel");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("1 S");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2SA");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Second Book of SAMUEL");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("2 Samuel");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("2 S");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1KI");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The First Book of KINGS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("1 Kings");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("1 K");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2KI");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Second Book of KINGS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("2 Kings");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("2 K");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1CH");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The First Book of CHRONICLES");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("1 Chronicles");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("1 Ch");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2CH");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Second Book of CHRONICLES");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("2 Chronicles");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("2 Ch");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "EZR");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of EZRA");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Ezra");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ezra");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "NEH");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of NEHEMIAH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Nehemiah");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ne");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "EST");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of ESTHER");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Esther");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Es");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JOB");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of JOB");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Job");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Job");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "PSA");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("PSALMS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Psalm");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ps");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "PRO");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("PROVERBS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Proverbs");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Pr");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ECC");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("ECCLESIASTES");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Ecclesiastes");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ec");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "SNG");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("SONG OF SONGS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Song of Songs");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Sgs");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ISA");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of ISAIAH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Isaiah");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Is");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JER");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of JEREMIAH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Jeremiah");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Jr");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "LAM");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("LAMENTATIONS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Lamentations");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Lm");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "EZK");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of EZEKIEL");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Ezekiel");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ez");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "DAN");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of DANIEL");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Daniel");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Dn");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "HOS");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of HOSEA");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Hosea");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ho");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JOL");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of JOEL");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Joel");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Jl");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "AMO");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of AMOS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Amos");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Am");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "OBA");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of OBADIAH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Obadiah");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ob");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JON");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of JONAH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Jonah");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Jon");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "MIC");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of MICAH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Micah");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Mic");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "NAM");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of NAHUM");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Nahum");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Nh");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "HAB");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of HABAKKUK");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Habakkuk");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Hb");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ZEP");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of ZEPHANIAH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Zephaniah");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Zep");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "HAG");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of HAGGAI");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Haggai");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Hg");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ZEC");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of ZECHARIAH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Zechariah");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Zec");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "MAL");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of MALACHI");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Malachi");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ml");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "MAT");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Gospel according to MATTHEW");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Matthew");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Mt");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "MRK");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Gospel according to MARK");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Mark");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Mk");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "LUK");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Gospel according to LUKE");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Luke");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Lk");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JHN");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Gospel according to JOHN");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("John");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Jn");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ACT");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("THE ACTS of the Apostles");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Acts");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ac");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ROM");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("Paul's Letter to the ROMANS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Romans");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ro");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1CO");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("Paul's First Letter to the CORINTHIANS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("1 Corinthians");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("1 Co");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2CO");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("Paul's Second Letter to the CORINTHIANS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("2 Corinthians");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("2 Co");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "GAL");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("Paul's Letter to the GALATIANS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Galatians");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ga");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "EPH");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("Paul's Letter to the EPHESIANS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Ephesians");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Eph");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "PHP");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("Paul's Letter to the PHILIPPIANS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Philippians");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Phil");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "COL");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("Paul's Letter to the COLOSSIANS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Colossians");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Col");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1TH");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("Paul's First Letter to the THESSALONIANS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("1 Thessalonians");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("1 Th");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2TH");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("Paul's Second Letter to the THESSALONIANS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("2 Thessalonians");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("2 Th");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1TI");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("Paul's First Letter to TIMOTHY");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("1 Timothy");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("1 Ti");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2TI");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("Paul's Second Letter to TIMOTHY");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("2 Timothy");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("2 Ti");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "TIT");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("Paul's Letter to TITUS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Titus");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Titus");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "PHM");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("Paul's Letter to PHILEMON");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Philemon");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Phm");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "HEB");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Letter to the HEBREWS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Hebrews");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("He");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JAS");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Letter from JAMES");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("James");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Jas");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1PE");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The First Letter from PETER");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("1 Peter");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("1 P");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2PE");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Second Letter from PETER");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("2 Peter");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("2 P");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1JN");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The First Letter of JOHN");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("1 John");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("1 Jn");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2JN");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Second Letter of JOHN");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("2 John");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("2 Jn");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "3JN");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Third Letter of JOHN");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("3 John");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("3 Jn");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JUD");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Letter from JUDE");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Jude");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Jd");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "REV");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("THE REVELATION to John");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Revelation");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Rev");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "TOB");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of TOBIT");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Tobit");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Tb");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "JDT");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of JUDITH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Judith");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Jdt");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "ESG");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of ESTHER");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Esther (Greek)");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Gk Est");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "WIS");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("THE WISDOM OF SOLOMON");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("The Wisdom of Solomon");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ws");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "SIR");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("SIRACH THE WISDOM OF JESUS, SON OF SIRACH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Sirach");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Si");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "BAR");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of BARUCH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Baruch");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Ba");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "LJE");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Letter of JEREMIAH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Letter of Jeremiah");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Let Jer");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "S3Y");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("THE PRAYER OF AZARIAH AND THE SONG OF THE THREE YOUNG MEN");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Song of Three Young Men");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("S of 3 Y");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "SUS");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Book of SUSANNA");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Susanna");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Su");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "BEL");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("BEL AND THE DRAGON");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("Bel and the Dragon");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Bel");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1MA");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The First Book of the MACCABEES");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("1 Maccabees");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("1 Macc");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2MA");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Second Book of the MACCABEES");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("2 Maccabees");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("2 Macc");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "1ES");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The First Book of ESDRAS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("1 Esdras");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("1 Esd");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "2ES");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("The Second Book of ESDRAS");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("2 Esdras");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("2 Esd");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("book");
	        _writer.WriteAttributeString("code", "MAN");
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "long");
	        _writer.WriteString("THE PRAYER OF MANASSEH");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "short");
	        _writer.WriteString("The Prayer of Manasseh");
	        _writer.WriteEndElement();
	        _writer.WriteStartElement("name");
	        _writer.WriteAttributeString("type", "abbr");
	        _writer.WriteString("Man");
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	        _writer.WriteEndElement();
	    }

	    private void CloseFile()
	    {
	        _writer.WriteEndDocument();
	        _writer.Flush();
	        _writer.Close();
	    }


	    /// ------------------------------------------------------------
		/// <summary>
		/// Finds the sty file for a Paratext project.
		/// </summary>
		/// <param name="database">The settings for the Paratext database.
		/// </param>
		/// ------------------------------------------------------------
		private void FindStyFile(string database)
        {
            string ssfFile = database + ".ssf";
            string ssfFullPath = string.Empty;

            if (Common.GetOsName() == "Windows7")
            {
                ssfFullPath = Common.GetValueFromRegistry("SOFTWARE\\Wow6432Node\\ScrChecks\\1.0\\Settings_Directory", "");
            }
            else if (Common.GetOsName() == "Windows XP")
            {
                ssfFullPath = Common.GetValueFromRegistry("SOFTWARE\\ScrChecks\\1.0\\Settings_Directory", "");
            }
            else if (Common.GetOsName() == "Unix") // todo: add linux code
            {
                ssfFullPath = Common.GetValueFromRegistry("SOFTWARE\\ScrChecks\\1.0\\Settings_Directory", "");
            }

            ssfFullPath = ssfFullPath + ssfFile;
           
            bool isStylesheet = false;

            if(!File.Exists(ssfFullPath))
            {
                Debug.WriteLine(ssfFile + " does not exist.");
                return;
            }

            var reader = new XmlTextReader(ssfFullPath) { XmlResolver = null };
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "StyleSheet") // Is class name null
                {
                    isStylesheet = true;
                }
                else if (reader.NodeType == XmlNodeType.Text && isStylesheet)
                {
                    ssfFile = reader.Value;
                    break;
                }
            }
            reader.Close();
            StyFullPath = Common.PathCombine(Path.GetDirectoryName(ssfFullPath), ssfFile);
        }


	   

	    /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
	    private string RemoveMultiClass(string className)
	    {
	        int pos = className.IndexOf("...", 1);
	        if (pos > 0)
	        {
	            className = className.Substring(0, pos);
	        }
	        return className;
	    }

	    /// ------------------------------------------------------------
		/// <summary>
		/// Gets the value from a line in the sty file.
		/// </summary>
		/// <param name="line">A line from the sty file which should
		/// contain property values for a style.</param>
		/// <returns></returns>
        /// ------------------------------------------------------------
		private string PropertyValue(string line)
        {
            string propertyVal;
            try
            {
                propertyVal = Common.RightString(line, " ").ToLower();
            }
            catch (Exception)
            {
                propertyVal = string.Empty;
            }

            return propertyVal;
        }

		/// ------------------------------------------------------------
		/// <summary>
		/// Writes the Cascading Style Sheet given properties determined
		/// from the Paratext sty file.
		/// </summary>
        /// ------------------------------------------------------------
		public void WriteCSS()
		{
            foreach (KeyValuePair<string, Dictionary<string, string>> cssClass in _styleInfo)
            {
                _writer.WriteStartElement("style");
                _writer.WriteAttributeString("id", cssClass.Key);
                _writer.WriteAttributeString("publishable", "true");
                _writer.WriteAttributeString("versetext", "true");
                foreach (KeyValuePair<string, string> property in cssClass.Value)
                {
                    if (cssClass.Key == "\\Name" || cssClass.Key == "\\Description")
                    {
                        _writer.WriteStartElement(cssClass.Key);
                        _writer.WriteString(property.Value);
                        _writer.WriteEndElement();
                        continue;
                    }

                    _writer.WriteStartElement("property");
                    _writer.WriteAttributeString("name", property.Key);
                    _writer.WriteString(property.Value);
                    _writer.WriteEndElement();
                }
                _writer.WriteEndElement();

            }
            _writer.WriteEndElement();
		    _writer.WriteEndDocument();
            _writer.Flush();
		    _writer.Close();
		}

            /// <summary>
            /// Convert paratext unit to css unit
            /// </summary>
            /// <param name="value">0/1/2</param>
            /// <returns>100%/150%/200%</returns>
        private string LineSpace(string value)
        {
		    string result = string.Empty; 
            if (value == "0")
            {
                result = "100%";
            }
            else if(value == "1")
            {
                result = "150%";
            }
            else if (value == "2")
            {
                result = "200%";
            }
		    return result;
        }

        private void CreateFile()
        {
            _writer = new XmlTextWriter(_cssFullPath, null);
            _writer.Formatting = Formatting.Indented;
            _writer.WriteStartDocument();

            _writer.WriteStartElement("DBLMetadata");
            _writer.WriteAttributeString("xmlns:ns0", "http://purl.org/dc/xmlns/2008/09/01/dc-ds-xml/");
            _writer.WriteAttributeString("id", "2880c78491b2f8ce");
            _writer.WriteAttributeString("revision", "3");
            _writer.WriteAttributeString("type", "text");
            _writer.WriteAttributeString("typeVersion", "1.2");
            _writer.WriteAttributeString("xml:base", "http://purl.org/ubs/metadata/dc/terms/");
            _writer.WriteStartElement("identification");
            _writer.WriteStartElement("name");
            _writer.WriteAttributeString("ns0:propertyURI", "title");
            _writer.WriteString("Good News Translation (US Version)");
            _writer.WriteEndElement();
            _writer.WriteStartElement("nameLocal");
            _writer.WriteString("Good News Translation (US Version)");
            _writer.WriteEndElement();
            _writer.WriteStartElement("abbreviation");
            _writer.WriteString("DBLTEST");
            _writer.WriteEndElement();
            _writer.WriteStartElement("abbreviationLocal");
            _writer.WriteString("DBLTEST");
            _writer.WriteEndElement();
            _writer.WriteStartElement("scope");
            _writer.WriteAttributeString("ns0:propertyURI", "title/scriptureScope");
            _writer.WriteString("Bible with Deuterocanon");
            _writer.WriteEndElement();
            _writer.WriteStartElement("description");
            _writer.WriteAttributeString("ns0:propertyURI", "description");
            _writer.WriteString("English: Good News Translation (US Version) Bible with Deuterocanon");
            _writer.WriteEndElement();
            _writer.WriteStartElement("dateCompleted");
            _writer.WriteAttributeString("ns0:propertyURI", "date");
            _writer.WriteAttributeString("ns0:sesURI", "http://purl.org/dc/terms/W3CDTF");
            _writer.WriteString("1992-10-01");
            _writer.WriteEndElement();
            _writer.WriteStartElement("systemId");
            _writer.WriteAttributeString("type", "tms");
            _writer.WriteAttributeString("ns0:propertyURI", "identifier/tmsID");
            _writer.WriteString("800ffc90-61c7-4eee-9d2e-3b846d211d34");
            _writer.WriteEndElement();
            _writer.WriteStartElement("systemId");
            _writer.WriteAttributeString("type", "paratext");
            _writer.WriteAttributeString("ns0:propertyURI", "identifier/ptxID");
            _writer.WriteString("2880c78491b2f8ce8b3a4e0b2c2ef4610f4ffabe");
            _writer.WriteEndElement();
            _writer.WriteStartElement("bundleProducer");
            _writer.WriteString("Paratext/7.3.0.0");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("confidential");
            _writer.WriteAttributeString("ns0:propertyURI", "accessRights/confidential");
            _writer.WriteString("false");
            _writer.WriteEndElement();
            _writer.WriteStartElement("agencies");
            _writer.WriteStartElement("etenPartner");
            _writer.WriteString("UBS");
            _writer.WriteEndElement();
            _writer.WriteStartElement("creator");
            _writer.WriteAttributeString("ns0:propertyURI", "creator");
            _writer.WriteString("Canadian Bible Society");
            _writer.WriteEndElement();
            _writer.WriteStartElement("publisher");
            _writer.WriteAttributeString("ns0:propertyURI", "publisher");
            _writer.WriteString("Canadian Bible Society");
            _writer.WriteEndElement();
            _writer.WriteStartElement("contributor");
            _writer.WriteAttributeString("ns0:propertyURI", "contributor");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("language");
            _writer.WriteStartElement("iso");
            _writer.WriteAttributeString("ns0:propertyURI", "language/iso");
            _writer.WriteAttributeString("ns0:sesURI", "http://purl.org/dc/terms/ISO639-3");
            _writer.WriteString("eng");
            _writer.WriteEndElement();
            _writer.WriteStartElement("name");
            _writer.WriteAttributeString("ns0:propertyURI", "subject/subjectLanguage");
            _writer.WriteAttributeString("ns0:sesURI", "http://purl.org/dc/terms/ISO639-3");
            _writer.WriteString("English");
            _writer.WriteEndElement();
            _writer.WriteStartElement("ldml");
            _writer.WriteAttributeString("ns0:propertyURI", "language/ldml");
            _writer.WriteString("en");
            _writer.WriteEndElement();
            _writer.WriteStartElement("rod");
            _writer.WriteAttributeString("ns0:propertyURI", "language/rod");
            _writer.WriteEndElement();
            _writer.WriteStartElement("script");
            _writer.WriteAttributeString("ns0:propertyURI", "language/script");
            _writer.WriteString("Latin");
            _writer.WriteEndElement();
            _writer.WriteStartElement("scriptDirection");
            _writer.WriteAttributeString("ns0:propertyURI", "language/scriptDirection");
            _writer.WriteString("LTR");
            _writer.WriteEndElement();
            _writer.WriteStartElement("numerals");
            _writer.WriteAttributeString("ns0:propertyURI", "language/numerals");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("country");
            _writer.WriteStartElement("iso");
            _writer.WriteAttributeString("ns0:propertyURI", "coverage/spatial");
            _writer.WriteAttributeString("ns0:sesURI", "http://purl.org/dc/terms/ISO3166");
            _writer.WriteString("US");
            _writer.WriteEndElement();
            _writer.WriteStartElement("name");
            _writer.WriteAttributeString("ns0:propertyURI", "subject/subjectCountry");
            _writer.WriteString("United States");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("type");
            _writer.WriteStartElement("translationType");
            _writer.WriteAttributeString("ns0:propertyURI", "type/translationType");
            _writer.WriteString("Revision");
            _writer.WriteEndElement();
            _writer.WriteStartElement("audience");
            _writer.WriteAttributeString("ns0:propertyURI", "audience");
            _writer.WriteString("Common");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }
        
        private void StaticCode1()
        {
            _writer.WriteStartElement("contents");
            _writer.WriteAttributeString("ns0:propertyURI", "tableOfContents");
            _writer.WriteStartElement("bookList");
            _writer.WriteAttributeString("id", "default");
            _writer.WriteStartElement("name");
            _writer.WriteString("Good News Translation (US Version)");
            _writer.WriteEndElement();
            _writer.WriteStartElement("nameLocal");
            _writer.WriteString("Good News Translation (US Version)");
            _writer.WriteEndElement();
            _writer.WriteStartElement("abbreviation");
            _writer.WriteString("DBLTEST");
            _writer.WriteEndElement();
            _writer.WriteStartElement("abbreviationLocal");
            _writer.WriteString("DBLTEST");
            _writer.WriteEndElement();
            _writer.WriteStartElement("description");
            _writer.WriteEndElement();
            _writer.WriteStartElement("range");
            _writer.WriteString("Bible with DC (Anglican Tradition)");
            _writer.WriteEndElement();
            _writer.WriteStartElement("tradition");
            _writer.WriteString("Bible with Anglican DCs");
            _writer.WriteEndElement();
        }

        private void StaticCode2()
        {
            _writer.WriteEndElement();
            _writer.WriteStartElement("bookList");
            _writer.WriteStartElement("name");
            _writer.WriteString("Good News Translation");
            _writer.WriteEndElement();
            _writer.WriteStartElement("nameLocal");
            _writer.WriteString("Good News Translation");
            _writer.WriteEndElement();
            _writer.WriteStartElement("abbreviation");
            _writer.WriteString("GNT");
            _writer.WriteEndElement();
            _writer.WriteStartElement("abbreviationLocal");
            _writer.WriteString("GNT");
            _writer.WriteEndElement();
            _writer.WriteStartElement("description");
            _writer.WriteString("NT + OT");
            _writer.WriteEndElement();
            _writer.WriteStartElement("range");
            _writer.WriteString("Protestant Bible (66 books)");
            _writer.WriteEndElement();
            _writer.WriteStartElement("tradition");
            _writer.WriteString("Western Protestant order");
            _writer.WriteEndElement();
        }

        private void StaticCode3()
        {
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("contact");
            _writer.WriteStartElement("rightsHolder");
            _writer.WriteAttributeString("ns0:propertyURI", "rightsHolder");
            _writer.WriteString("American Bible Society");
            _writer.WriteEndElement();
            _writer.WriteStartElement("rightsHolderLocal");
            _writer.WriteAttributeString("ns0:propertyURI", "rightsHolder/contactLocal");
            _writer.WriteString("American Bible Society");
            _writer.WriteEndElement();
            _writer.WriteStartElement("rightsHolderAbbreviation");
            _writer.WriteAttributeString("ns0:propertyURI", "rightsHolder/contactAbbreviation");
            _writer.WriteString("ABS");
            _writer.WriteEndElement();
            _writer.WriteStartElement("rightsHolderURL");
            _writer.WriteAttributeString("ns0:propertyURI", "rightsHolder/contactURL");
            _writer.WriteString("http://www.americanbible.org/");
            _writer.WriteEndElement();
            _writer.WriteStartElement("rightsHolderFacebook");
            _writer.WriteAttributeString("ns0:propertyURI", "rightsHolder/contactFacebook");
            _writer.WriteString("https://www.facebook.com/americanbible");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("copyright");
            _writer.WriteStartElement("statement");
            _writer.WriteAttributeString("contentType", "xhtml");
            _writer.WriteAttributeString("ns0:propertyURI", "rights");
            _writer.WriteStartElement("p");
            _writer.WriteString(
                "Good News Translation (Today’s English Version, Second Edition) © 1992 American Bible Society. All rights reserved.");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("promotion");
            _writer.WriteStartElement("promoVersionInfo");
            _writer.WriteAttributeString("contentType", "xhtml");
            _writer.WriteAttributeString("ns0:propertyURI", "description/pubPromoVersionInfo");
            _writer.WriteStartElement("p");
            _writer.WriteString(
                "Good News Translation with Deuterocanonicals/Apocrypha. Scripture taken from the Good News Translation (r) (Today's English Version, Second Edition). Copyright (c) 1992 American Bible Society. Used by permission.");
            _writer.WriteEndElement();
            _writer.WriteStartElement("p");
            _writer.WriteString(
                "This is a Digital Bible Library demonstration bundle. It should not be used for any publication purposes");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("promoEmail");
            _writer.WriteAttributeString("contentType", "xhtml");
            _writer.WriteAttributeString("ns0:propertyURI", "description/pubPromoEmail");
            _writer.WriteStartElement("p");
            _writer.WriteString("Hi YouVersion friend,");
            _writer.WriteEndElement();
            _writer.WriteStartElement("p");
            _writer.WriteString(
                "Nice work downloading the English Good News Translation in the Bible App! Now you'll have anytime, anywhere access to God's Word on your mobile device—even if you're outside of service coverage or not connected to the Internet. It also means faster service whenever you read that version since it's stored on your device. Enjoy!");
            _writer.WriteEndElement();
            _writer.WriteStartElement("p");
            _writer.WriteString(
                "This download was made possible by American Bible Society. We really appreciate their passion for making the Bible available to millions of people around the world. Because of their generosity, YouVersion users like you can open up the Bible and hear from God no matter where you are. You can learn more about the great things American Bible Society is doing on many fronts by visiting");
            _writer.WriteStartElement("a");
            _writer.WriteAttributeString("href", "http://www.americanbible.org/");
            _writer.WriteString("www.americanbible.org.");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("p");
            _writer.WriteString(
                "Again, we're glad you downloaded the English Good News Translation and hope it enriches your interaction with God's Word.");
            _writer.WriteEndElement();
            _writer.WriteStartElement("p");
            _writer.WriteString("Sincerely,");
            _writer.WriteEndElement();
            _writer.WriteStartElement("p");
            _writer.WriteString("Your Friends at YouVersion");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("archiveStatus");
            _writer.WriteStartElement("archivistName");
            _writer.WriteAttributeString("ns0:propertyURI", "contributor/archivist");
            _writer.WriteString("Jeff Klassen");
            _writer.WriteEndElement();
            _writer.WriteStartElement("dateArchived");
            _writer.WriteAttributeString("ns0:propertyURI", "dateSubmitted");
            _writer.WriteAttributeString("ns0:sesURI", "http://purl.org/dc/terms/W3CDTF");
            _writer.WriteString("2012-03-12T17:51:32.7907868+00:00");
            _writer.WriteEndElement();
            _writer.WriteStartElement("dateUpdated");
            _writer.WriteAttributeString("ns0:propertyURI", "modified");
            _writer.WriteAttributeString("ns0:sesURI", "http://purl.org/dc/terms/W3CDTF");
            _writer.WriteString("2012-03-17T14:21:02.8293104+00:00");
            _writer.WriteEndElement();
            _writer.WriteStartElement("comments");
            _writer.WriteAttributeString("ns0:propertyURI", "abstract");
            _writer.WriteString("DBL example bundle");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
            _writer.WriteStartElement("format");
            _writer.WriteAttributeString("ns0:propertyURI", "format");
            _writer.WriteAttributeString("ns0:sesURI", "http://purl.org/dc/terms/IMT");
            _writer.WriteString("text/xml");
            _writer.WriteEndElement();
            _writer.WriteEndElement();
        }        
	}
}
