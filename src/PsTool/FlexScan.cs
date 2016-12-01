// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2016, SIL International. All Rights Reserved.
// <copyright from='2016' to='2016' company='SIL International'>
//		Copyright (c) 2016, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: FlexScan.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System.Xml;

namespace SIL.Tool
{
    public class FlexScan : XmlParser
    {
        public string[] VernWs;
        public string[] AnalWs;

        public FlexScan(string xmlFullName) : base(xmlFullName)
        {
            VernWs = new string[0];
            AnalWs = new string[0];
            Declare(XmlNodeType.Element, CaptureLanguageTag);
            Declare(XmlNodeType.Text, CaptureLanguageTagValue);
            Parse();
        }

        private bool _inVern;
        private bool _inAnal;

        private void CaptureLanguageTag(XmlReader r)
        {
            switch (r.LocalName)
            {
                case "CurVernWss":
                    _inVern = true;
                    break;
                case "CurAnalysisWss":
                    _inAnal = true;
                    break;
                case "Uni":
                    break;
                default:
                    _inVern = _inAnal = false;
                    if (VernWs.Length > 0 && AnalWs.Length > 0)
                    {
                        Finished();
                    }
                    break;
            }
        }

        private void CaptureLanguageTagValue(XmlReader r)
        {
            if (_inVern)
            {
                VernWs = r.Value.Split(' ');
            }
            else if (_inAnal)
            {
                AnalWs = r.Value.Split(' ');
            }
        }

    }
}
