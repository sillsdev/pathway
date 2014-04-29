// --------------------------------------------------------------------------------------------
// <copyright file="ParatextPathwayLink.cs" from='2009' to='2014' company='SIL International'>
//      Copyright ( c ) 2014, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// 
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class ParatextPathwayLink
    {
        private readonly Dictionary<string, object> _mXslParams;
        private readonly XslCompiledTransform _mCleanUsx = new XslCompiledTransform();
        private readonly XslCompiledTransform _mSeparateIntoBooks = new XslCompiledTransform();
        private readonly XslCompiledTransform _mUsxToXhtml = new XslCompiledTransform();
        private readonly XslCompiledTransform _mEncloseParasInSections = new XslCompiledTransform();

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="ParatextPathwayLink"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="xslParams">The parameters from Paratext for the XSLT.</param>
        /// ------------------------------------------------------------------------------------
        public ParatextPathwayLink(string databaseName, Dictionary<string, object> xslParams)
        {
            _mXslParams = xslParams;

            // If the writing system is undefined or set (by default) to English, add a writing system code 
            // that should not have a dictionary to prevent all words from being marked as misspelled.
            object strWs;
            if (_mXslParams.TryGetValue("ws", out strWs))
            {
                if ((string)strWs == "en")
                    _mXslParams["ws"] = "zxx";
            }
            else
            {
                Debug.Fail("Missing writing system parameter for XSLT");
                _mXslParams.Add("ws", "zxx");
            }
            if (!_mXslParams.ContainsKey("langInfo"))
            {
                _mXslParams.Add("langInfo", Common.ParaTextDcLanguage(databaseName));
            }
            LoadStyleSheets();
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        /// Loads the style sheets that are used to transform from Paratext USX to XHTML.
        /// </summary>
        /// ------------------------------------------------------------------------------------
        private void LoadStyleSheets()
        {
            // Create stylesheets
            _mCleanUsx.Load(XmlReader.Create(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "ParatextSupport.XML_without_line_breaks.xsl")));
            _mSeparateIntoBooks.Load(XmlReader.Create(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "ParatextSupport.SeparateIntoBooks.xsl")));
            _mUsxToXhtml.Load(XmlReader.Create(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "ParatextSupport.UsfxToXhtml.xsl")));
            _mEncloseParasInSections.Load(XmlReader.Create(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "ParatextSupport.EncloseParasInSections.xsl")));
        }
    }
}
