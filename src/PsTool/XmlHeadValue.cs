// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2017, SIL International. All Rights Reserved.
// <copyright from='2017' to='2017' company='SIL International'>
//		Copyright (c) 2016, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: XmlHeadValue.cs
// Responsibility: Greg Trihus
// ---------------------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Xml;

namespace SIL.Tool
{
    public class XmlHeadValue : XmlParser
    {
	    public readonly string AttrFilter;
	    public readonly string FilterFor;
	    public readonly string TargetAttr;
	    public readonly List<string> Results = new List<string>();
	    private readonly string _tag;
	    private bool _inTag;

        public XmlHeadValue(string xmlFullName, string tag, string attrFilter = null, string filterFor = null, string targetAttr = null) : base(xmlFullName)
        {
	        _tag = tag;
	        AttrFilter = attrFilter;
	        FilterFor = filterFor;
	        TargetAttr = targetAttr;
            Declare(XmlNodeType.Element, FindTag);
            Declare(XmlNodeType.Text, CaptureValue);
			Declare(XmlNodeType.EndElement, CheckEnd);
            Parse();
        }

	    private void FindTag(XmlReader r)
        {
	        if (r.LocalName == _tag)
	        {
		        if (!string.IsNullOrEmpty(AttrFilter))
		        {
					var attrVal = r.GetAttribute(AttrFilter);
					if ((!string.IsNullOrEmpty(attrVal)? attrVal.ToLower(): "") != FilterFor) return;
		        }
		        if (!string.IsNullOrEmpty(TargetAttr))
		        {
					var result = r.GetAttribute(TargetAttr);
					if (!string.IsNullOrEmpty(result)) Results.Add(result);
				}
		        _inTag = true;
	        }
        }

        private void CaptureValue(XmlReader r)
        {
			if (_inTag && string.IsNullOrEmpty(TargetAttr)) Results.Add(r.Value);
        }

		private void CheckEnd(XmlReader r)
		{
			if (r.LocalName == _tag) _inTag = false;
			if (r.LocalName == "head") Finished();
		}
	}
}
