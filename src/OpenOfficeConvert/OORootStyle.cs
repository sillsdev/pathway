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
// File: OORootStyle.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System.Xml;
using SIL.Tool;

namespace SIL.PublishingSolution
{
	public class GetRootStyle : XmlParser
	{
		public string RootStyle;

		public GetRootStyle(string xmlFullName) : base(xmlFullName)
		{
			RootStyle = string.Empty;
			Declare(XmlNodeType.Element, CaptureBodyClassLang);
			Parse();
			Close();
		}

		private void CaptureBodyClassLang(XmlReader rdr)
		{
			if (rdr.LocalName != "body") return;
			var bodyClass = rdr.GetAttribute("class");
			if (bodyClass != null) RootStyle = bodyClass;
			var bodyLang = rdr.GetAttribute("lang");
			if (bodyLang != null) RootStyle += "_." + bodyLang;
			Finished();
        }
    }
}
