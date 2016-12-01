/**********************************************************************************************
 * Dll:     JWTools
 * File:    JW_Xml.cs
 * Author:  John Wimbish
 * Created: 19 Dec 2003
 * Purpose: Provides a shorthand for frequent xml operations.
 * Legal:   Copyright (c) 2005-08, John S. Wimbish. All Rights Reserved.  
 *********************************************************************************************/
#region Using
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Text;
#endregion


namespace JWTools
{
    #region CLASS: XmlField
    public class XmlField
		#region Documentation
		/* Used to output a field (level) of xml. Supports several different tag formats:
		 * 
		 *	 (a) <tag>data</tag>
		 * 
		 *	 (b) <tag attr1=data1 attr2=data2>   (no end tag)
		 * 
		 *	 (c) <tag attr1=data1 attr2=data2>data</tag>
		 * 
		 *	 (d) <tag>
		 *          data
		 *	     </tag>
		 * 
		 * Each level of tags is indented automatically (with two spaces) for readability.
		 * 
		 * To use:
		 * 
		 * At the topmost level, use the constructor to create an XmlField. Then pass this
		 * XmlField down to the other levels. Anytime a new field is needed, create it with
		 * the GetDaughterXmlField method; in this manner the indentation will be properly
		 * handled.
		 * 
		 * Start the field's data with the Begin() method. When done, use the End() method
		 * to put in the end tag. The WriteDataLine() method is for writing data that 
		 * appears between the two tags. 
		 * 
		 * The following method would be found in an object. It receives the parent field,
		 * and the first thing done is to create an XmlField for this daughter object. 
		 * 
		 * 		public void Write(XmlField xmlParent)
		 *      {
		 *          XmlField xml = xmlParent.GetDaughterXmlField("MyTag", true);
		 *          xml.Begin( xml.GetAttrString("Height", Height.ToString()) );
		 *          xml.WriteDataLine("Here is my data.");
		 *          m_wnd.Write(xml);
		 *          xml.End();
		 *      }
		 * 
		 * This results in
		 * 
		 *      <MyTag Height="20">
		 *      Here is my data.
		 *      </MyTag>
		 * 
		 * The OneLiner methods are used for tags where we want it all on one line. Thus:
		 *     xml.OneLiner( xml.GetAttrString("Height", Height.ToString()), "Here is my data");
		 * results in:
		 *     <MyTag Height="20">Here is my data</MyTag>
		 * 
		 * It is possible to not have an end tag for those tags which only have attributes, e.g.,
		 *     <MyTag Height="20">
		 * To do this, set  bUsesEndTag to false in the GetDaughterXmlField method. (Note that
		 * at the first level, the constructor does not allow this option, as we assume that
		 * the top level will always contain other levels.)
		 */
		#endregion
	{
		// Implementational Attributes -------------------------------------------------------
		TextWriter m_writer;
		string m_sTag;
		int    m_nIndent;
		bool   m_bEnded;
		bool   m_bUsesEndTag;
		#region Attr{g} string Padding - returns a string containing the correct amt of leading spaces
		private string Padding
		{
			get
			{
				return new string(' ', m_nIndent * 2);
			}
		}
		#endregion

		// Constructors ----------------------------------------------------------------------
		#region Constructor(writer, sTag) - use this only at the first level
		public XmlField(TextWriter writer, string sTag)
		{
			m_writer = writer;
			m_sTag = sTag;
			m_nIndent = 0;
			m_bEnded = false;
			m_bUsesEndTag = true;
		}
		#endregion
		#region protected Constructor(writer, sTag, nIndent, bUsesEndTag) - used by GetDaughterXmlField
		protected XmlField(TextWriter writer, string sTag, int nIndent, bool bUsesEndTag)
		{
			m_writer = writer;
			m_sTag = sTag;
			m_nIndent = nIndent;
			m_bEnded = false;
			m_bUsesEndTag = bUsesEndTag;
		}
		#endregion
		#region Method: XmlField GetDaughterXmlField(sTag, bUsesEndTag) - constructs one with increased indent
		public XmlField GetDaughterXmlField(string sTag, bool bUsesEndTag)
		{
			Debug.Assert(true == m_bUsesEndTag);
			Debug.Assert(false == m_bEnded);
			return new XmlField(m_writer, sTag, m_nIndent + 1, bUsesEndTag);
		}
		#endregion

		// Output methods --------------------------------------------------------------------
		#region Method: void OneLiner(sData) - produces "<tag>my data</tag>"
		public void OneLiner(string sData)
		{
			Debug.Assert(true == m_bUsesEndTag);
			Debug.Assert(false == m_bEnded);
			m_writer.WriteLine( Padding + "<" + m_sTag + ">" +
                _AmpersandsAndSuch(sData) + 
                "</" + m_sTag + ">");
			m_bEnded = true;
		}
		#endregion
		#region Method: void OneLiner(sValues, sData) - produces "<tag name="John">my data</tag>"
		public void OneLiner(string sValues, string sData)
		{
			Debug.Assert(true == m_bUsesEndTag);
			Debug.Assert(false == m_bEnded);

            string sPad = " ";
            if (!string.IsNullOrEmpty(sValues) && sValues[0] == ' ')
                sPad = "";

            m_writer.WriteLine(Padding + "<" + m_sTag + sPad + sValues + ">" + 
                _AmpersandsAndSuch( sData ) + 
                "</" + m_sTag + ">");
			m_bEnded = true;
		}
		#endregion
		#region Method: void Begin() - produces "<tag>"
		public void Begin()
		{
			Debug.Assert(true == m_bUsesEndTag);
			Debug.Assert(false == m_bEnded);
			m_writer.WriteLine( Padding + "<" + m_sTag + ">");
		}
		#endregion
		#region Method: void Begin(string sValues) - produces "<tag name="John">"
		public void Begin(string sValues)
		{
			Debug.Assert(false == m_bEnded);
			m_writer.WriteLine( Padding + "<" + m_sTag + sValues + ">");
		}
		#endregion
		#region Method: void WriteDataLine(sDataLine) - writes out a string of data (e.g., between tags)
		public void WriteDataLine(string sDataLine)
		{
			Debug.Assert(true == m_bUsesEndTag);
			Debug.Assert(false == m_bEnded);
			m_writer.WriteLine( Padding + sDataLine );
		}
		#endregion
		#region Method: void End() - writes "</tag>" (or nothing if no end-tag desired)
		public void End()
		{
			Debug.Assert(false == m_bEnded);
			if (true == m_bUsesEndTag)
				m_writer.WriteLine( Padding + "</" + m_sTag + ">");
			m_bEnded = true;
		}
		#endregion
        #region Method: string _AmpersandsAndSuch(string sIn)
        string _AmpersandsAndSuch(string sIn)
        {
            XmlReplace[] replacements = XmlReplace.Replacements;

            string sOut = "";
            foreach (char ch in sIn)
            {
                bool bReplaced = false;
                foreach (XmlReplace xr in replacements)
                {
                    if (xr.m_chMemory == ch)
                    {
                        sOut += xr.m_sXML;
                        bReplaced = true;
                        break;
                    }
                }

                if (!bReplaced)
                    sOut += ch;
            }
            return sOut;
        }
        #endregion
        #region Method: string GetAttrString(sAttr, sValue) - produces " name="John""
        public string GetAttrString(string sAttr, string sValue)
		{
            string sData = _AmpersandsAndSuch(sValue);
            return " " + sAttr + "=\"" + sData + "\"";
		}
		#endregion
        #region Method: string GetAttrString(sAttr, sValue) - produces " amount="10""
        public string GetAttrString(string sAttr, int nValue)
        {
            string sValue = nValue.ToString();
            string sData = _AmpersandsAndSuch(sValue);
            return " " + sAttr + "=\"" + sData + "\"";
        }
        #endregion
        #region Method: void WriteBlankLine() - desirable sometimes for aesthetic reasons
		public void WriteBlankLine()
		{
			m_writer.WriteLine("");
		}
		#endregion
    }
    #endregion

    #region CLASS: XmlReplace
    public class XmlReplace
    {
        public string m_sXML;
        public char m_chMemory;

        #region Constructor(sXML, chMemory)
        public XmlReplace(string _sXML, char _chMemory)
        {
            m_sXML = _sXML;
            m_chMemory = _chMemory;
        }
        #endregion

        #region SAttr{g}: XmlReplace[] Replacements - the actual set of replacements
        static public XmlReplace[] Replacements
        {
            get
            {
                return new XmlReplace[] 
                    { 
                        new XmlReplace( "{n}", '\n' ),
                        new XmlReplace( "&amp;", '&' ),
                        new XmlReplace( "&quot;", '\"' ),
                        new XmlReplace( "&lt;", '<' ),
                        new XmlReplace( "&gt;", '>' )
                    };
            }
        }
        #endregion
    };
    #endregion

    #region CLASS: XmlRead
    public class XmlRead
		#region Documentation
		/* Supports reading xml data. There are a series of methods to read through the
		 * file, and then another set of methods to retrieve values from a line that has
		 * been read in. As an example of the syntax:
		 * 
		 *		while (xml.ReadNextLineUntilEndTag("MyTag") )  // reads to "</MyTag>"
		 *		{
		 *			if (xml.IsTag( "AnotherTag" ))             // if line has <AnotherTag
		 *			{
		 *				string sValue = xml.GetValue("Name");  // Extract: Name="Hello"
		 *			}
		 *		}
		 */
		#endregion
	{
		// Implementational Attributes -------------------------------------------------------
		TextReader m_reader;
		#region Attr{g}: string CurrentLine
		public string CurrentLine
		{
			get { return m_sLine; }
		}
		string m_sLine;           // Current line
		#endregion

		// Scaffolding -----------------------------------------------------------------------
		#region Constructor(TextReader reader)
		public XmlRead(TextReader reader)
		{
			m_reader = reader;
		}
		#endregion

		// Read Methods (advances the StreamReader) ------------------------------------------
		#region Method: bool ReadToTag(XmlTag) - reads until the <XmlTag> is found
		public bool ReadToTag(string XmlTag)
		{
			while ( ReadNextLine() )
			{
				if (IsTag(XmlTag))
					return true;
			}
			return false;
		}
		#endregion
		#region Method: bool ReadNextLine() - sets m_sLine to the current line
		public bool ReadNextLine()
		{
			m_sLine = m_reader.ReadLine();
			return (null != m_sLine);
		}
		#endregion
		#region bool ReadNextLineUntilEndTag(string sEndTag) - returns next line, stops at EndTag
		public bool ReadNextLineUntilEndTag(string sEndTag)
		{
			m_sLine = m_reader.ReadLine();
			if (IsClosingTag(sEndTag) )
				return false;
			return (null != m_sLine);
		}
		#endregion

		// Tests and Data Retrieval ----------------------------------------------------------
		#region public bool IsTag(sTag) - T if sLine has sTag in it
		public bool IsTag(string sTag)
		{
			return (sTag == GetTag());
		}
		#endregion
		#region public static string GetTag() - retrieves the tag from the line
		public string GetTag()
		{
			// No leading spaces
			string sLine = m_sLine.Trim();

			// Move past the opening angle
			int i = 0;
			if(i < sLine.Length && sLine[i] == '<')
				++i;

			// Copy in the tag
			string sTag = "";
			while( i < sLine.Length && sLine[i] != ' ' && sLine[i]!='>')
			{
				sTag += sLine[i];
				++i;
			}
			return sTag;
		}
		#endregion
		#region public string GetValue(sAttr) - returns the value for an attribute
		public string GetValue(string sAttr)
		{
			string sValue = "";

            // Locate the attribute. We can't do IndexOf, because we must only check outside of
            // quotes.
            int i = 0;
            bool bWithinQuotes = false;
            int iEnd = m_sLine.Length - sAttr.Length;
            for (; i < iEnd; i++)
            {
                if (m_sLine[i] == '\"')
                    bWithinQuotes = !bWithinQuotes;

                if (!bWithinQuotes && m_sLine.Substring(i, sAttr.Length) == sAttr)
                    break;
            }
            if (i == iEnd)
                return null;

            // Move past any, e.g., white space
			while (i < m_sLine.Length && m_sLine[i] != '\"')
				++i;

            // Move past the opening quote mark
			if (i < m_sLine.Length && m_sLine[i] == '\"')
				++i;

            // Collect the data
			while( i < m_sLine.Length && m_sLine[i] != '\"')
			{
                sValue += m_sLine[i];
                i++;
            }

            // Special characters
            string sOut = _AmpersandsAndSuch(sValue);

            return sOut;
		}
		#endregion
        #region Method: string _AmpersandsAndSuch(string sIn)
        string _AmpersandsAndSuch(string sIn)
        {
            string sOut = "";

            XmlReplace[] replacements = XmlReplace.Replacements;

            int i = 0;
            while (i < sIn.Length)
            {
                bool bReplaced = false;
                foreach (XmlReplace xr in replacements)
                {
                    int n = xr.m_sXML.Length;
                    if (i < sIn.Length - n && sIn.Substring(i, n) == xr.m_sXML)
                    {
                        sOut += xr.m_chMemory;
                        i += n;
                        bReplaced = true;
                        break;
                    }
                }

                // Everything else
                if (!bReplaced)
                {
                    sOut += sIn[i];
                    i++;
                }
            }

            return sOut;
        }
        #endregion
        #region Method: string GetOneLinerData()
        public string GetOneLinerData()
		{
			string sRight = m_sLine.Substring(m_sLine.IndexOf('>') + 1);
			string sData  = sRight.Substring(0, sRight.IndexOf("</"));
            string sOut = _AmpersandsAndSuch(sData);
            return sOut;
		}
		#endregion
		#region public bool IsClosingTag(sTag) - T if sLine has the closing sTag in it
		public bool IsClosingTag(string sTag)
		{
			if (sTag.Length > 0 && sTag[0] != '/')
				sTag = "/" + sTag;
			return (sTag == GetTag());
		}
		#endregion
    }
    #endregion

    #region CLASS: SfRead
    public class SfRead
	{
		// Attributes ------------------------------------------------------------------------
		StreamReader m_reader = null;
		public string Marker;
		public string Data;
		public bool   AtEndOfFile = false;
		private bool  m_bSuppressReadOnce = false;
		public int LineNumber = 0;


        // Scaffolding -----------------------------------------------------------------------
        #region Constructor(StreamReader)
        public SfRead(StreamReader r)
        {
            m_reader = r;
            LineNumber = 0;
        }
        #endregion
        #region Constructor(sPathName, sFileFilter, enc) - opens the reader
        public SfRead(ref string sPathName, string sFileFilter)
		{
			m_reader = JW_Util.OpenStreamReader(ref sPathName, sFileFilter);
			LineNumber = 0;
		}
		#endregion
		#region Method: public void Close() - closes the StreamReader
		public void Close()
		{
			m_reader.Close();
		}
		#endregion

		// Data Retrieval --------------------------------------------------------------------
		#region Method: void SuppressReadNextTime() - pushes the current field so it is, in effect, read again
		public void SuppressReadNextTime()
		{
			m_bSuppressReadOnce = true;
		}
		#endregion
		#region Method: ReadNextField() - places values into Marker and Data
		public bool ReadNextField()
		{
			// Option to not do the read; this is helpful when returning from a call stack,
			// e.g., where the called method reads up through a \mkr, but the caller
			// needs to also process that \mkr as well.
			if (m_bSuppressReadOnce)
			{
				m_bSuppressReadOnce = false;
				return true;
			}

			// Reset
			Marker = "";
			Data   = "";

			// Read until we get a line with a field marker in it
			string sLine = "";
			while ( (sLine = m_reader.ReadLine()) != null)
			{
				++LineNumber;

				if (sLine.Length > 1 && sLine[0] == '\\')
					break;

			}
			if (sLine == null)
			{
				AtEndOfFile = true;
				return false;
			}

			// Extract the marker and the first line of data
			if (sLine[0] == '\\' && sLine[1] != '\0')
			{
				int i = 1;
				while(i < sLine.Length && sLine[i] != ' ')
				{
					Marker += sLine[i];
					++i;
				}
				while(i < sLine.Length && sLine[i] == ' ')
					++i;
				Data = sLine.Substring(i).Trim();
			}

			// Append any additional lines up until the next marker
			int nPeek;
			while ( (nPeek = m_reader.Peek()) != -1)
			{
				if (nPeek == '\\')
					break;

				string sAppend = m_reader.ReadLine();
				Data = Data.Trim() + (" " + sAppend );
				++LineNumber;
			}
			Data = Data.Trim();

			// End of file
			if ("" == Marker)
			{
				AtEndOfFile = true;
				return false;
			}

			// Still more to be read
			return true;
		}
		#endregion
    }
    #endregion

    #region CLASS: SfWrite
    public class SfWrite
	{
		// Attributes ------------------------------------------------------------------------
		private TextWriter m_writer = null;
		private int m_cMarkerPad = 7;
		private bool m_bUseMkrPadding = false;
		private int m_cMaxLineLength = 60;

		// Scaffolding -----------------------------------------------------------------------
		#region Constructor(sPathName)
		public SfWrite(string sPathName)
		{
			m_writer = JW_Util.GetTextWriter(sPathName);
		}
		#endregion
		#region Method: void Close() - closes the writer; no more writing is possible
		public void Close()
		{
			m_writer.Close();
			m_writer = null;
		}
		#endregion

		// Methods ---------------------------------------------------------------------------
		#region Method: string PrepareFullMarker(string sMarker) - turns "mkr" into "\mkr "
		private string PrepareFullMarker(string sMarker)
		{
			// No blank spaces in the marker for starters
			sMarker = sMarker.Trim();

			// Make sure we have a non-null marker
			if (sMarker.Length == 0)
				throw new Exception("Empty marker");
			if (sMarker.Length == 1 && sMarker[0] == '\\')
				throw new Exception("Marker is only a backslash.");

			// Make sure the marker begins with a backslash
			if (sMarker[0] != '\\')
				sMarker = "\\" + sMarker;

			// Make sure the marker ends with a blank space
			sMarker = sMarker + " ";

			// Option to pad the marker to achieve the length
			if (m_bUseMkrPadding)
			{
				while (sMarker.Length < m_cMarkerPad)
				{
					sMarker = sMarker + " ";
				}
			}

			// Return the result
			return sMarker;
		}
		#endregion
		#region Method: ArrayList PrepareData(sData)
		private ArrayList PrepareData(string sData)
		{
			// Destination for the field once broken down into lines
			ArrayList rgLines = new ArrayList();

			// We use the bBlankFound boolean to determine when to split a line. We only
			// want to split after blanks, so that blank(s) always appear at the end of
			// a line, not at the beginning.
			bool bBlankFound = false;

			// We use sLine to build each individual line; when it is full, we add it to the
			// rgLines array, and start again with an empty sLine. We use "i" as the counter
			// to determine when it is full.
			string sLine = "";

			// Loop through each character in the input stream, breaking it down into lines
			foreach( char c in sData)
			{
				// If the line is long enough, break it.
				if (sLine.Length > m_cMaxLineLength)
				{
					// First look for a blank space; signal that we have found one
					if ( c == ' ')
						bBlankFound = true;

					// If we are now at a non-blank space, and have previously found 
					// a blank, then we are ready to break the line. We do this by adding
					// it to the array, and then re-setting our temp line back to empty.
					if ( c != ' ' && bBlankFound)
					{
						rgLines.Add(sLine.TrimEnd());
						sLine = "";
						bBlankFound = false;
					}
				}

				sLine += c;
			}

			// Add the final line (which is likely a partial)
			if (0 != sLine.Length)
				rgLines.Add(sLine);

			// Return the result
			return rgLines;
		}
		#endregion
		#region Method: void Write(sMarker, string sData, bWrapLines)
        public void Write(string sMarker, string sData, bool bWrapLines)
		{
            // Add the backslash and trailing space to the marker
			string sFullMarker = PrepareFullMarker(sMarker);

            // Option 1 - All on a single line
            if (!bWrapLines)
            {
                m_writer.WriteLine(sFullMarker + sData);
                return;
            }

            // Option 2 - Split into short lines
			// Prepare the data for output
			ArrayList rgLines = PrepareData(sData);

			// Write out the first line (which includes the marker)
			string sFirstLine = sFullMarker;
			if (rgLines.Count > 0)
				sFirstLine += rgLines[0];
			m_writer.WriteLine(sFirstLine);

            // Write out the remaining lines
			for(int i = 1; i < rgLines.Count; i++)
				m_writer.WriteLine(rgLines[i]);
		}
		#endregion
		#region Method: void WriteBlankLine() - puts out a blank line
		public void WriteBlankLine()
		{
			m_writer.WriteLine("");
		}
		#endregion
    }
    #endregion

    // WE WANT TO EVENTUALLY MAKE THIS OBSOLETE THROUGH ALL PROJECTS
	#region HEADED FOR OBSOLESCENCE - Class JW_Xml
	public class JW_Xml
	{
		#region public static string GetTag(sLine) - retrieves the tag from the xml marker
		static public string GetTag(string sLine)
		{
			// No leading spaces
			sLine = sLine.Trim();

			// Move past the opening angle
			int i = 0;
			if(i < sLine.Length && sLine[i] == '<')
				++i;

			// Copy in the tag
			string sTag = "";
			while( i < sLine.Length && sLine[i] != ' ' && sLine[i]!='>')
			{
				sTag += sLine[i];
				++i;
			}
			return sTag;
		}
		#endregion
		#region public static bool IsTag(sTag, sLine) - T if sLine has sTag in it
		static public bool IsTag(string sTag, string sLine)
		{
			return (sTag == GetTag(sLine));
		}
		#endregion
		#region public static bool IsColsingTag(sTag, sLine) - T if sLine has the closing sTag in it
		static public bool IsClosingTag(string sTag, string sLine)
		{
			if (sTag.Length > 0 && sTag[0] != '/')
				sTag = "/" + sTag;
			return (sTag == GetTag(sLine));
		}
		#endregion
		#region public static string GetValue(sAttr, sLine) - returns the value for an attribute
		static public string GetValue(string sAttr, string sLine)
		{
			string sValue = "";
			int i = sLine.IndexOf(sAttr);
			while (i < sLine.Length && sLine[i] != '\"')
				++i;
			while (i < sLine.Length && sLine[i] == '\"')
				++i;
			while( i < sLine.Length && sLine[i] != '\"')
			{
				sValue += sLine[i];
				i++;
			}
			return sValue;
		}
		#endregion

		public static string GetAttrValueString(string sAttr, string sValue)
		{
			return sAttr + "=\"" + sValue + "\" ";
		}


		public static string BeginTag(string sTag, string sValues)
		{
			m_nIndent += 2;
			return (new string(' ', m_nIndent * 2) + "<" + sTag + " " + sValues + ">");
		}
		public static string BeginTag(string sTag)
		{
			m_nIndent += 2;
			return (new string(' ', m_nIndent * 2) + "<" + sTag + ">");
		}
		public static string EndTag(string sTag)
		{
			m_nIndent -= 2;
			Debug.Assert(m_nIndent >= 0);
			return (new string(' ', (m_nIndent + 2) * 2) + "</" + sTag + ">");
		}


		private static int m_nIndent = 0;
	}
	#endregion

    #region CLASS: JW_Util
    public class JW_Util
	{
		#region Method: static void TextWriter GetTextWriter(string sPath)
		public static TextWriter GetTextWriter(string sPath)
		{
            // Delete any existing file, so that we can avoid the Read-Only
            // problems that have been happening in Timor.
            try
            {
                File.Delete(sPath);
            }
            catch (Exception)
            {
            }

            // Create a new file to write to
			StreamWriter w = new StreamWriter(sPath, false, Encoding.UTF8);
			TextWriter tw = TextWriter.Synchronized(w);
			return tw;
		}
		#endregion
		#region Method: static void TextReader GetTextReader(ref sPath, sFileFilter,)
		public static TextReader GetTextReader(ref string sPath, string sFileFilter)
		{
			StreamReader r = OpenStreamReader(ref sPath, sFileFilter);
			TextReader tr = TextReader.Synchronized(r);
			return tr;
		}
		#endregion

		#region Method: StreamReader OpenStreamReader(...) - provides opportunity to browse
		static public StreamReader OpenStreamReader(
			ref string sPathName, 
			string sFileFilter)
		{
			if (!File.Exists(sPathName))
			{
				// Tell the user, and see if he wants to browse for it
				string sMessage = "Unable to locate file \n\"" + sPathName +
					".\" \n\nDo you want to browse for it?";
				DialogResult result = MessageBox.Show( Form.ActiveForm, sMessage, 
					"File Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
				if (result != DialogResult.Yes)
					throw new IOException("Unable to locate file" + sPathName);

				// Browse for the file
				OpenFileDialog dlg = new OpenFileDialog();

				// We only want to return a single file
				dlg.Multiselect = false;

				// Filter on the desired type of files
				if (null != sFileFilter && sFileFilter.Length > 0)
				{
					dlg.Filter = sFileFilter;
					dlg.FilterIndex = 0;
				}

				// Retrieve Dialog Title from resources
				dlg.Title = "Browse for " + Path.GetFileName(sPathName);

				// Get the default pathname
				if (sPathName.Length > 0)
				{
					dlg.InitialDirectory = Path.GetDirectoryName(sPathName);
					dlg.FileName = Path.GetFileName(sPathName);
				}

				// Browse for the file
				if (DialogResult.OK != dlg.ShowDialog(Form.ActiveForm))
					throw new IOException("Unable to locate file" + sPathName);
				sPathName = dlg.FileName;
			}

            return new StreamReader(sPathName, Encoding.UTF8);
		}
		#endregion

		#region Method: static void CreateBackup(string sPathName, string sNewExtension)
		static public void CreateBackup(string sPathName, string sNewExtension)
		{
			string sBackupPathName = Path.ChangeExtension(sPathName, sNewExtension);
			if (File.Exists(sPathName))
			{
				try
				{
					if (File.Exists(sBackupPathName))
						File.Delete(sBackupPathName);
					File.Move(sPathName, sBackupPathName);
				}
				catch (Exception)
				{}
			}
		}
		#endregion
		#region Method: static void RestoreFromBackup(string sPathName, string sExtension)
		static public void RestoreFromBackup(string sPathName, string sExtension)
		{
			try
			{
				// Give time for the OS to finish any cleanup it is doing
				Thread.Sleep(1000);

				// Copy the backup onto the filename (provided one exists, of course)
				string sBackupPathName = Path.ChangeExtension(sPathName, sExtension);
				if (File.Exists(sBackupPathName))
					File.Copy(sBackupPathName, sPathName, true);
			}
			catch (Exception)
			{}
		}
		#endregion
        // Commented for Moma - Compatible
//        #region Method: static long GetFreeDiskSpace(string sDrive)
//        static public long GetFreeDiskSpace(string sDrive)
//            // Borrowed from a message by Roman Rodov, 5mar04, on the CodeGuru
//            // message board. 
//        {
//            long lFreeSpace = 0L;

//            // Get the management class holding Logical Drive information
//            ManagementClass mcDriveClass = new ManagementClass("Win32_LogicalDisk");

//            // Enumerate all logical drives available
////			Console.WriteLine("GetFreeDiskSpace thread exits here...");
//            ManagementObjectCollection mocDrives = mcDriveClass.GetInstances();
//            foreach(ManagementObject moDrive in mocDrives)
//            {
//                // sDeviceId will hold the drive name eg "C:"
//                String sDeviceId = moDrive.Properties["DeviceId"].Value.ToString() + "\\";
//                if (sDeviceId == sDrive)
//                {
//                    PropertyData pd = moDrive.Properties["FreeSpace"];
//                    lFreeSpace = long.Parse(pd.Value.ToString());
//                    break;
//                }
//            }

//            // Done
//            mocDrives.Dispose();
//            mcDriveClass.Dispose();

//            return lFreeSpace;
//        }
//        #endregion

		// XML Utilities ---------------------------------------------------------------------
		const char c_chDelim = '\"';
		const char c_chBlank = ' ';
		#region Method: static string XmlGetStringAttr(string sAttr, string s)
		static public string XmlGetStringAttr(string sAttr, string s)
			// Given a string s, such as
			//    <SR ID="3425" Gloss="return home" Form="pulang">
			// if "Form" is requested, returns
			//    pulang
		{
			// Find the Attribute within the string
			int i = s.IndexOf(sAttr);
			if (-1 == i)
				return "";

			// Move to the opening delimiter
			while (i < s.Length && s[i] != c_chDelim)
				++i;
			if (i < s.Length && s[i] == c_chDelim)
				++i;

			// Extract the value
			string sValue = "";
			while( i < s.Length && s[i] != c_chDelim)
			{
				sValue += s[i];
				i++;
			}
			return sValue;
		}
		#endregion
		#region Method: static int XmlGetIntAttr(string sAttr, string s)
		static public int XmlGetIntAttr(string sAttr, string s)
		{
			string sValue = XmlGetStringAttr(sAttr,s);

			try
			{
				int nValue = Convert.ToInt16(sValue);
				return nValue;
			}
			catch(Exception)
			{
				return 0;
			}
		}
		#endregion
		#region Method: static int XmlGetBoolAttr(string sAttr, string s)
		static public bool XmlGetBoolAttr(string sAttr, string s)
		{
			string sValue = XmlGetStringAttr(sAttr,s);

			if (sValue.ToLower() == "true")
				return true;

			return false;
		}
		#endregion
		#region Method: static string[] XmlGetEmbeddedObjects(string sAttr, string s)
		static public string[] XmlGetEmbeddedObjects(string sAttr, string s)
		{
			// The sTag will start each new embedded object
			string sTag = "<" + sAttr + " ";

			// Find the first one; if there isn't one, then return an empty array
			int i = s.IndexOf(sTag);
			if (-1 == i)
				return new string[0];
			s = s.Substring(i);

			// We'll temporarily put the answers in an array list
			ArrayList a = new ArrayList();

			// Extract all of the strings. We assume that they are all here in a row,
			// together. If this turns out not to be the case, then we'll need to
			// rework the logic to use s.IndexOf(sTag) to find subsequent ones.
			while (s.Length > 0 && s.IndexOf(sTag) == 0)
			{
				int nLen = XmlDistanceToClosingBrace(s);

				string sObject = s.Substring(0, nLen + 1);

				a.Add(sObject);

				s = s.Substring(nLen + 1);

				while (s.Length > 0 && s[0] == ' ')
					s = s.Substring(1);
			}

			// Convert to a string array
			string[] v = new string[ a.Count ];
			for(int k=0; k<a.Count; k++)
				v[k] = (string)a[k];
			return v;
		}
		#endregion
		#region Method: static int XmlDistanceToClosingBrace(string s)
		static public int XmlDistanceToClosingBrace(string s)
		{
			// We should be sitting at an opening brace
			if (s.Length == 0 || s[0] != '<')
				return 0;

			// Use this to ignore any '>' that might be in a field
			bool bIsInField = false;

			// Use this to skip over any embedded data
			int cDepth = 0;

			// Loop through the line until we find it
			int i = 1;
			while( i < s.Length )
			{
				// We've found the closing bracket
				if (s[i] == '>' &&  !bIsInField && cDepth == 0)
					break;

				if ( s[i] == c_chDelim )
					bIsInField = !bIsInField;

				if ( s[i] == '<')
					cDepth++;
				if (s[i] == '>')
					cDepth--;

				i++;
			}
			return i;
		}
		#endregion

        #region SMethod: string XmlValue(string sAttr, string sValue, bool bSpaceAfter)
        static public string XmlValue(string sAttr, string sValue, bool bSpaceAfter)
		{
			Debug.Assert(sAttr.Length > 0);
			Debug.Assert(sAttr[ sAttr.Length - 1] != '=');

			string s = sAttr + "=" + c_chDelim + sValue + c_chDelim;

			if (bSpaceAfter)
				s += c_chBlank;

			return s;
        }
        #endregion
        #region SMethod: string XmlValue(string sAttr, int nValue, bool bSpaceAfter)
        static public string XmlValue(string sAttr, int nValue, bool bSpaceAfter)
		{
			return XmlValue(sAttr, nValue.ToString(), bSpaceAfter );
        }
        #endregion
        #region SMethod: string XmlValue(string sAttr, bool bValue, bool bSpaceAfter)
        static public string XmlValue(string sAttr, bool bValue, bool bSpaceAfter)
		{
			return XmlValue(sAttr,(bValue ? "true" : "false" ), bSpaceAfter );
        }
        #endregion

        #region SMethod: Encoding GetUnicodeFileEncoding(String FileName)
        public static Encoding GetUnicodeFileEncoding(String FileName)
            // Return the Encoding of a text file.  Return Encoding.Default if no Unicode
            // BOM (byte order mark) is found.
        {
            Encoding enc = null;

            FileInfo info = new FileInfo(FileName);

            FileStream stream = null;

            try
            {
                stream = info.OpenRead();

                Encoding[] UnicodeEncodings = { Encoding.BigEndianUnicode, Encoding.Unicode, Encoding.UTF8 };

                for (int i = 0; enc == null && i < UnicodeEncodings.Length; i++)
                {
                    stream.Position = 0;

                    byte[] Preamble = UnicodeEncodings[i].GetPreamble();

                    bool PreamblesAreEqual = true;

                    for (int j = 0; PreamblesAreEqual && j < Preamble.Length; j++)
                    {
                        PreamblesAreEqual = Preamble[j] == stream.ReadByte();
                    }

                    if (PreamblesAreEqual)
                    {
                        enc = UnicodeEncodings[i];
                    }
                }
            }
            catch (System.IO.IOException)
            {
                return null;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            if (enc == null)
            {
                enc = Encoding.Default;
            }

            return enc;
        }
    #endregion
    }
    #endregion

}
