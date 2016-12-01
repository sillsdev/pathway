/**********************************************************************************************
 * Dll:     JWTools
 * File:    JW_Localization.cs
 * Author:  John Wimbish
 * Created: 12 May 2007
 * Purpose: Localization system.
 * Legal:   Copyright (c) 2005-08, John S. Wimbish. All Rights Reserved.  
 *********************************************************************************************/
#region Using
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.IO;
using JWTools;
#endregion

namespace SIL.Tool.Localization
{
    public class LocAlternate
    {
        // Attrs -----------------------------------------------------------------------------
        #region Attr{g/s}: string Value - the string in the language
        public string Value
        {
            get
            {
                Debug.Assert(null != m_sValue);
                return m_sValue;
            }
            set
            {
                m_sValue = value;
            }
        }
        string m_sValue = "";
        #endregion
        #region Attr{g/s}: string ShortcutKey
        public string ShortcutKey
        {
            get
            {
                return m_sShortcutKey;
            }
            set
            {
                m_sShortcutKey = value;
            }
        }
        string m_sShortcutKey = "";
        #endregion
        #region Attr{g/s}: string ToolTip
        public string ToolTip
        {
            get
            {
                return m_sToolTip;
            }
            set
            {
                m_sToolTip = value;
            }
        }
        string m_sToolTip = "";
        #endregion

        // Scaffolding -----------------------------------------------------------------------
        #region Constructor(sValue, sKey, sTip)
        public LocAlternate(string sValue, string sKey, string sTip)
        {
            m_sValue = sValue;
            m_sShortcutKey = ((null != sKey) ? sKey : "");
            m_sToolTip = ((null != sTip) ? sTip : "");
        }
        #endregion

        // I/O -------------------------------------------------------------------------------
        #region CONSTANTS
        public const string c_sTag = "Alt";
        public const string c_sID = "ID";
        public const string c_sValue = "Value";
        public const string c_sKey = "Key";
        public const string c_sTip = "Tip";
        #endregion
        #region Method: void WriteXML(XmlField xmlParent, string sLanguageID)
        public void WriteXML(XmlField xmlParent, string sLanguageID)
        {
            // Nothing to write if completely empty
            bool bHasValue = !string.IsNullOrEmpty(Value);
            bool bHasKey = !string.IsNullOrEmpty(ShortcutKey);
            bool bHasTip = !string.IsNullOrEmpty(ToolTip);
            if (!bHasValue && !bHasKey && !bHasTip)
                return;

            // Beginning tag <Item> contains the ID
            XmlField xml = xmlParent.GetDaughterXmlField(c_sTag, true);

            // The ID for the language, e.g., "sp", "inz"
            string s = xml.GetAttrString(c_sID, sLanguageID);

            // The value
            if (bHasValue)
                s += xml.GetAttrString(c_sValue, Value);

            // Shortcut if present
            if (bHasKey)
                s += xml.GetAttrString(c_sKey, ShortcutKey);

            // Tooltip if present
            if (bHasTip)
                s += xml.GetAttrString(c_sTip, ToolTip);

            // Write it out
            xml.OneLiner(s, "");
        }
        #endregion
        #region SMethod: LocAlternate ReadXML(XmlRead xml)
        static public LocAlternate ReadXML(XmlRead xml)
        {
            // Collect the parts
            string sValue = xml.GetValue(c_sValue);
            string sShortcutKey = xml.GetValue(c_sKey);
            string sToolTip = xml.GetValue(c_sTip);

            // Create and return the class
            LocAlternate alt = new LocAlternate(sValue, sShortcutKey, sToolTip);
            return alt;
        }
        #endregion

        // Determine if Needs Attention ------------------------------------------------------
        #region Method: bool EndsWithColon(s)
        bool EndsWithColon(string s)
        {
            if (s.Length > 0 && s[s.Length - 1] == ':')
                return true;
            return false;
        }
        #endregion
        #region Method: bool EndsWithEllipsis(s)
        bool EndsWithEllipsis(string s)
        {
            if (s.Length < 3)
                return false;

            if (s.Substring(s.Length - 3) == "...")
                return true;

            return false;
        }
        #endregion
        #region Method: int ParameterCount(s)
        int ParameterCount(string s)
        {
            int c = 0;

            for (int i = 0; i < s.Length - 2; i++)
            {
                char ch1 = s[i];
                char ch2 = s[i + 1];
                char ch3 = s[i + 2];
                if (ch1 == '{' && Char.IsDigit(ch2) && ch3 == '}')
                    c++;
            }

            return c;
        }
        #endregion
        #region Method: bool HasAmpersand(string s)
        bool HasAmpersand(string s)
        {
            if (s.IndexOf('&') != -1)
                return true;
            return false;
        }
        #endregion
        #region Method: string NeedsAttention(LocItem item)
        public string NeedsAttention(LocItem item)
            // Returns empty string if OK, otherwise, a string indicating the problem
        {
            // Is there a value?
            if (string.IsNullOrEmpty(Value))
                return "There is no Value in this language";

            // Colon handled?
            if (EndsWithColon(item.English) && !EndsWithColon(Value))
                return "The Value needs to end with a colon \":\")";
            if (!EndsWithColon(item.English) && EndsWithColon(Value))
                return "The Value should not end with a colon \":\")";

            // Ellipsis handled?
            if (EndsWithEllipsis(item.English) && !EndsWithEllipsis(Value))
                return "The Value needs to end with a ellipsis \"...\")";
            if (!EndsWithEllipsis(item.English) && EndsWithEllipsis(Value))
                return "The Value should not end with a ellipsis \"...\")";

            // Parameter count
            if (ParameterCount(item.English) != ParameterCount(Value))
            {
                return "The English and the Value need to have the same number of " +
                       "parameters, e.g., {0}, {1}, etc.";
            }

            // Ampersands
            if (HasAmpersand(item.English) && !HasAmpersand(Value))
            {
                return "This Value needs an ampersand to indicate which letter is the " +
                       "shortcut within the menu.";
            }

            return "";
        }
        #endregion
    }

    public class LocItem
    {
        // Attrs -----------------------------------------------------------------------------
        #region Attr{g}: string ID - a unique identifier of the string
        public string ID
        {
            get
            {
                Debug.Assert(null != m_sID && m_sID.Length > 0);
                return m_sID;
            }
        }
        string m_sID;
        #endregion
        #region Attr{g/s}: string English - the string in English, always available
        public string English
        {
            get
            {
                Debug.Assert(null != m_sEnglish && m_sEnglish.Length > 0);
                return m_sEnglish;
            }
            set
            {
                m_sEnglish = value;
            }
        }
        string m_sEnglish = "";
        #endregion
        #region Attr{g/s}: string Information - additional instructions for the localizer
        public string Information
        {
            get
            {
                return m_sInformation;
            }
            set
            {
                m_sInformation = value;
            }
        }
        string m_sInformation = "";
        #endregion
        #region Attr{g/s}: bool CanHaveShortcutKey
        public bool CanHaveShortcutKey
        {
            get
            {
                return m_bCanHaveShortcutKey;
            }
            set
            {
                m_bCanHaveShortcutKey = value;
            }
        }
        bool m_bCanHaveShortcutKey = false;
        #endregion
        #region Attr{g/s}: string ShortcutKey - the string in English, always available
        public string ShortcutKey
        {
            get
            {
                return m_sShortcutKey;
            }
            set
            {
                m_sShortcutKey = value;
            }
        }
        string m_sShortcutKey = "";
        #endregion
        #region Attr{g/s}: bool CanHaveToolTip
        public bool CanHaveToolTip
        {
            get
            {
                if (!string.IsNullOrEmpty(ToolTip))
                    return true;

                return m_bCanHaveToolTip;
            }
            set
            {
                m_bCanHaveToolTip = value;
            }
        }
        bool m_bCanHaveToolTip = false;
        #endregion
        #region Attr{g/s}: string ToolTip
        public string ToolTip
        {
            get
            {
                return m_sToolTip;
            }
            set
            {
                m_sToolTip = value;
            }
        }
        string m_sToolTip = "";
        #endregion

        // Alternaties in each language (other than English) ---------------------------------
        #region Attr{g}: LocAlternate[] Alternates
        public LocAlternate[] Alternates
        {
            get
            {
                Debug.Assert(null != m_vAlternates);
                return m_vAlternates;
            }
        }
        LocAlternate[] m_vAlternates;
        #endregion

        #region Method: LocAlternate GetAlternate(int iIndex)
        public LocAlternate GetAlternate(int iIndex)
        {
            // Make certain the index is within range
            if (iIndex < 0 || iIndex >= Alternates.Length)
                return null;

            // Retrieve the Alternate
            return Alternates[iIndex];
        }
        #endregion
        #region Method: void AddAlternate(iIndex, LocAlternate)
        public void AddAlternate(int iIndex, LocAlternate alt)
        {
            // Extend the vector if we need to
            if (iIndex >= Alternates.Length )
            {
                LocAlternate[] v = new LocAlternate[iIndex + 1];
                for (int i = 0; i < Alternates.Length; i++)
                    v[i] = Alternates[i];
                m_vAlternates = v;
            }

            // Place the value in the appropriate position
            Alternates[iIndex] = alt;
        }
        #endregion

        // Access, with Primary and Secondary languages --------------------------------------
        #region VAttr{g}: string AltValue
        public string AltValue
        {
            get
            {
                // If the primary language is null, then English was intended
                if (null == LocDB.DB.PrimaryLanguage)
                    return English;

                // Otherwise, go for one of our localized languages
                if (null != LocDB.DB.PrimaryLanguage)
                {
                    LocAlternate alt = GetAlternate(LocDB.DB.PrimaryLanguage.Index);
                    if (null != alt)
                        return alt.Value;
                }

                // If the secondary language is null, then English was intended
                if (null == LocDB.DB.SecondaryLanguage)
                    return English;

                // Otherwise, go for one of our localized languages
                if (null != LocDB.DB.SecondaryLanguage)
                {
                    LocAlternate alt = GetAlternate(LocDB.DB.SecondaryLanguage.Index);
                    if (null != alt)
                        return alt.Value;
                }

                // If here, then the localization did not exist for either Primary or Secondary
                return English;
            }
        }
        #endregion
        #region VAttr{g}: string AltShortcutKey
        public string AltShortcutKey
        {
            get
            {
                // If the primary language is null, then English was intended
                if (null == LocDB.DB.PrimaryLanguage)
                    return ShortcutKey;

                // Otherwise, go for one of our localized languages
                if (null != LocDB.DB.PrimaryLanguage)
                {
                    LocAlternate alt = GetAlternate(LocDB.DB.PrimaryLanguage.Index);
                    if (null != alt && !string.IsNullOrEmpty(alt.ShortcutKey))
                        return alt.ShortcutKey;
                }

                // If the secondary language is null, then English was intended
                if (null == LocDB.DB.SecondaryLanguage)
                    return ShortcutKey;

                // Otherwise, go for one of our localized languages
                if (null != LocDB.DB.SecondaryLanguage)
                {
                    LocAlternate alt = GetAlternate(LocDB.DB.SecondaryLanguage.Index);
                    if (null != alt && !string.IsNullOrEmpty(alt.ShortcutKey))
                        return alt.ShortcutKey;
                }

                // If here, then the localization did not exist for either Primary or Secondary
                return ShortcutKey;
            }
        }
        #endregion
        #region VAttr{g}: string AltToolTip
        public string AltToolTip
        {
            get
            {
                // If the primary language is null, then English was intended
                if (null == LocDB.DB.PrimaryLanguage)
                    return ToolTip;

                // Otherwise, go for one of our localized languages
                if (null != LocDB.DB.PrimaryLanguage)
                {
                    LocAlternate alt = GetAlternate(LocDB.DB.PrimaryLanguage.Index);
                    if (null != alt && !string.IsNullOrEmpty(alt.ToolTip))
                        return alt.ToolTip;
                }

                // If the secondary language is null, then English was intended
                if (null == LocDB.DB.SecondaryLanguage)
                    return ToolTip;

                // Otherwise, go for one of our localized languages
                if (null != LocDB.DB.SecondaryLanguage)
                {
                    LocAlternate alt = GetAlternate(LocDB.DB.SecondaryLanguage.Index);
                    if (null != alt && !string.IsNullOrEmpty(alt.ToolTip))
                        return alt.ToolTip;
                }

                // If here, then the localization did not exist for either Primary or Secondary
                return ToolTip;
            }
        }
        #endregion

        // I/O -------------------------------------------------------------------------------
        #region DOC
        /* XML Format (eventually I want the Alts in their own file)
         * 
         * <Item ID="m_menuNew" Key="true" Tip="true">
         *    <Info>"Menu command to create a new project."</Info>
         *    <En>"&Amp;New..."</En>
         *    <Key>Ctrl+N</Key>
         *    <Tip Title="New Project">Create a blank, new project</Tip>
         *    <Alt lang="inz" Value="&amp;Baru..." Key="Ctrl+B" TipTitle="Proyek Baru" TipText="Cipta proyeck yang baru"></Alt>
         *    <Alt lang="sp" Value="&amp;Nuevo..." Key="Ctrl+N"></Alt>
         *    <Alt lang="swh" Value="&amp;Mpya..."></Alt>
         * </Item>
         */
        #endregion
        #region CONSTANTS
        public const string c_sTag = "Item";
        public const string c_sID = "ID";
        public const string c_sEnglish = "English";
        public const string c_sInformation = "Info";
        public const string c_sTip = "Tip";
        public const string c_sKey = "Key";
        #endregion
        #region Method: void WriteXML(XmlField xmlParent)
        public void WriteXML(XmlField xmlParent)
        {
            // Beginning tag <Item> contains the ID
            XmlField xml = xmlParent.GetDaughterXmlField(c_sTag, true);
            string s = xml.GetAttrString(c_sID, ID);
            if (CanHaveShortcutKey)
                s += xml.GetAttrString(c_sKey, "true");
            if (CanHaveToolTip)
                s += xml.GetAttrString(c_sTip, "true");
            xml.Begin(s);

            // Add the English
            xml.GetDaughterXmlField(c_sEnglish, true).OneLiner(English);

            // Add the Information, if any
            if (!string.IsNullOrEmpty(Information))
                xml.GetDaughterXmlField(c_sInformation, true).OneLiner(Information);

            // Add the shortcut key, if any
            if (!string.IsNullOrEmpty(ShortcutKey))
                xml.GetDaughterXmlField(c_sKey, true).OneLiner(ShortcutKey);

            // Add the Tooltip, if any
            if (!string.IsNullOrEmpty(ToolTip))
                xml.GetDaughterXmlField(c_sTip, true).OneLiner(ToolTip);

            // End Tag </Item>
            xml.End();
        }
        #endregion
        #region SMethod: LocItem ReadXML(XmlRead xml)
        static public LocItem ReadXML(XmlRead xml)
        {
            // Collect the ID from the Tag line, and create an item from it
            string sID = xml.GetValue(c_sID);
            LocItem item = new LocItem(sID);

            string sCanHaveKey = xml.GetValue(c_sKey);
            if (!string.IsNullOrEmpty(sCanHaveKey) && sCanHaveKey == "true")
                item.CanHaveShortcutKey = true;

            string sCanHaveTip = xml.GetValue(c_sTip);
            if (!string.IsNullOrEmpty(sCanHaveTip) && sCanHaveTip == "true")
                item.CanHaveToolTip = true;

            // Loop through the other lines for the remaining data
            while (xml.ReadNextLineUntilEndTag(c_sTag))
            {
                if (xml.IsTag(c_sInformation))
                    item.Information = xml.GetOneLinerData();

                if (xml.IsTag(c_sEnglish))
                    item.English = xml.GetOneLinerData();

                if (xml.IsTag(c_sKey))
                    item.ShortcutKey = xml.GetOneLinerData();

                if (xml.IsTag(c_sTip))
                    item.ToolTip = xml.GetOneLinerData();
            }

            return item;
        }
        #endregion
        #region Method: void WriteLanguageData(XmlField xmlParent, LocLanguage)
        public void WriteLanguageData(XmlField xmlParent, LocLanguage lang)
        {
            // Initialize the Item field
            XmlField xml = xmlParent.GetDaughterXmlField(c_sTag, true);
            string s = xml.GetAttrString(c_sID, ID);
            xml.Begin(s);

            // Write the languge data
            LocAlternate alt = GetAlternate(lang.Index);
            if (null != alt)
                alt.WriteXML(xml, lang.ID);

            // Done
            xml.End();
        }
        #endregion
        #region Method: void ReadLanguageData(XmlRead, LocLanguage)
        public void ReadLanguageData(XmlRead xml, LocLanguage lang)
        {
            while (xml.ReadNextLineUntilEndTag(c_sTag))
            {
                if (xml.IsTag(LocAlternate.c_sTag))
                {
                    string sLanguageID = xml.GetValue(LocAlternate.c_sID);
                    if (sLanguageID != lang.ID)
                        return;

                    LocAlternate alt = LocAlternate.ReadXML(xml);
                    AddAlternate(lang.Index, alt);
                }
            }
        }
        #endregion

        // Scaffolding -----------------------------------------------------------------------
        #region Constructor(sID)
        public LocItem(string _sID)
        {
            m_sID = _sID;

            m_vAlternates = new LocAlternate[0];
        }
        #endregion
    }

    public class LocGroup
    {
        // Attrs -----------------------------------------------------------------------------
        #region Attr{g}: string ID - a unique identifier of the Group
        public string ID
        {
            get
            {
                Debug.Assert(null != m_sID && m_sID.Length > 0);
                return m_sID;
            }
        }
        string m_sID;
        #endregion
        #region Attr{g/s}: string Description - a description for the Group
        public string Description
        {
            get
            {
                return m_sDescription;
            }
            set
            {
                m_sDescription = value;
            }
        }
        string m_sDescription = "";
        #endregion
        #region Attr{g}: string Title - The title of the group....user-friendly in the database
        public string Title
        {
            get
            {
                Debug.Assert(null != m_sTitle && m_sTitle.Length > 0);
                return m_sTitle;
            }
        }
        string m_sTitle;
        #endregion
        #region Attr{g/s}: bool TranslatorAudience - If F, Advisor is the primary audience for these terms
        public bool TranslatorAudience
        {
            get
            {
                return m_bTranslatorAudience;
            }
            set
            {
                m_bTranslatorAudience = value;
            }
        }
        bool m_bTranslatorAudience;
        #endregion

        // Items -----------------------------------------------------------------------------
        #region Attr{g}: LocItem[] Items
        public LocItem[] Items
        {
            get
            {
                Debug.Assert(null != m_vItems);
                return m_vItems;
            }
        }
        LocItem[] m_vItems = null;
        #endregion
        #region Method: LocItem AppendItem(LocItem)
        public LocItem AppendItem(LocItem item)
        {
            LocItem[] v = new LocItem[Items.Length + 1];

            for (int i = 0; i < Items.Length; i++)
                v[i] = Items[i];

            v[Items.Length] = item;

            m_vItems = v;

            return item;
        }
        #endregion
        #region Method: LocItem Find(sID)
        public LocItem Find(string sID)
        {
            foreach (LocItem item in Items)
            {
                if (item.ID == sID)
                    return item;
            }

            return null;
        }
        #endregion
        #region Method: LocItem FindRecursively(string sID)
        public LocItem FindRecursively(string sID)
        {
            // First, look through the items in this group
            LocItem item = Find(sID);
            if (null != item)
                return item;

            // If unsuccessful, look through the groups owned by this group, and so on
            foreach (LocGroup group in Groups)
            {
                item = group.FindRecursively(sID);
                if (null != item)
                    return item;
            }

            // Unsucessful
            return null;
        }
        #endregion
        #region Method: LocItem FindOrAddItem(sItemID, sEnglish)
        public LocItem FindOrAddItem(string sItemID, string sEnglish)
        {
            LocItem item = Find(sItemID);
            if (null == item)
            {
                item = new LocItem(sItemID);
                item.English = sEnglish;
                AppendItem(item);
            }
            Debug.Assert(null != item);
            return item;
        }
        #endregion

        // Subgroups -------------------------------------------------------------------------
        #region Attr{g}: LocGroup[] Groups
        public LocGroup[] Groups
        {
            get
            {
                Debug.Assert(null != m_vGroups);
                return m_vGroups;
            }
        }
        LocGroup[] m_vGroups = null;
        #endregion
        #region Method: LocGroup FindGroup(sID)
        public LocGroup FindGroup(string sID)
        {
            foreach (LocGroup group in Groups)
            {
                if (group.ID == sID)
                    return group;
            }
            return null;
        }
        #endregion
        #region Method: void _AppendGroup(LocGroup group)
        void _AppendGroup(LocGroup group)
        {
            if (null == group)
                return;

            LocGroup[] v = new LocGroup[Groups.Length + 1];
            for (int i = 0; i < Groups.Length; i++)
                v[i] = Groups[i];
            v[Groups.Length] = group;
            m_vGroups = v;
        }
        #endregion
        #region Method: LocGroup FindOrAddGroup(sGroupID)
        public LocGroup FindOrAddGroup(string sGroupID)
        {
            LocGroup group = FindGroup(sGroupID);

            if (null == group)
            {
                group = new LocGroup(sGroupID, sGroupID);
                _AppendGroup(group);
            }

            Debug.Assert(null != group);

            return group;
        }
        #endregion

        // I/O -------------------------------------------------------------------------------
        public const string c_sTag = "Group";
        public const string c_sID = "ID";
        public const string c_sTitle = "Title";
        public const string c_sDescription = "Des";
        public const string c_sTranslatorAudience = "Translator";
        #region Method: void WriteXML(XmlField xmlParent)
        public void WriteXML(XmlField xmlParent)
        {
            XmlField xml = xmlParent.GetDaughterXmlField(c_sTag, true);

            string s = xml.GetAttrString(c_sID, ID);

            s += xml.GetAttrString(c_sTitle, Title);

            if (!string.IsNullOrEmpty(Description))
                s += xml.GetAttrString(c_sDescription, Description);

            s += xml.GetAttrString(c_sTranslatorAudience,
                                   (TranslatorAudience) ? "true" : "false");

            xml.Begin(s);

            foreach (LocItem item in Items)
                item.WriteXML(xml);

            foreach (LocGroup sub in Groups)
                sub.WriteXML(xml);

            xml.End();
        }
        #endregion
        #region SMethod: LocGroup ReadXML(LocDB db, XmlRead xml)
        static public LocGroup ReadXML(LocDB db, XmlRead xml)
        {
            string sID = xml.GetValue(c_sID);
            string sTitle = xml.GetValue(c_sTitle);
            string sDescription = xml.GetValue(c_sDescription);
            string sTranslatorAudience = xml.GetValue(c_sTranslatorAudience);

            LocGroup group = new LocGroup(sID, sTitle);
            group.Description = sDescription;
            group.TranslatorAudience = (sTranslatorAudience == "true") ? true : false;

            while (xml.ReadNextLineUntilEndTag(c_sTag))
            {//yesu - stoped here
                if (xml.IsTag(LocItem.c_sTag))
                {
                    LocItem item = LocItem.ReadXML(xml);
                    group.AppendItem(item);
                }

                if (xml.IsTag(LocGroup.c_sTag))
                {
                    LocGroup sub = LocGroup.ReadXML(db, xml);
                    group._AppendGroup(sub);
                }
            }

            return group;
        }
        #endregion
        #region Method: void WriteLanguageData(xmlParent, LocLanguage)
        public void WriteLanguageData(XmlField xmlParent, LocLanguage lang)
        {
            XmlField xml = xmlParent.GetDaughterXmlField(c_sTag, true);

            string s = xml.GetAttrString(c_sID, ID);

            xml.Begin(s);

            foreach (LocItem item in Items)
                item.WriteLanguageData(xml, lang);

            foreach (LocGroup sub in Groups)
                sub.WriteLanguageData(xml, lang);

            xml.End();
        }
        #endregion
        #region Method: void ReadLanguageData(XmlRead, LocLanguage)
        public void ReadLanguageData(XmlRead xml, LocLanguage lang)
        {
            while (xml.ReadNextLineUntilEndTag(c_sTag))
            {
                if (xml.IsTag(LocItem.c_sTag))
                {
                    string sItemID = xml.GetValue(LocItem.c_sID);
                    LocItem item = Find(sItemID);
                    if (null != item)
                        item.ReadLanguageData(xml, lang);
                }

                if (xml.IsTag(LocGroup.c_sTag))
                {
                    string sGroupID = xml.GetValue(LocGroup.c_sID);
                    LocGroup group = FindGroup(sGroupID);
                    if (null != group)
                        group.ReadLanguageData(xml, lang);
                }
            }
        }
        #endregion

        // Scaffolding -----------------------------------------------------------------------
        #region Constructor(sID, sTitle)
        public LocGroup(string _sID, string _sTitle)
        {
            m_sID = _sID;
            m_sTitle = _sTitle;
            m_vItems = new LocItem[0];
            m_vGroups = new LocGroup[0];
        }
        #endregion
    }

    public class LocLanguage
    {
        // Attrs -----------------------------------------------------------------------------
        #region Attr{g}: string ID - the identifier of the language, e.g., Eng, Inz, Sp.
        public string ID
        {
            get
            {
                Debug.Assert(null != m_sID && m_sID.Length > 0);
                return m_sID;
            }
        }
        string m_sID;
        #endregion
        #region Attr{g}: string Name - the name of the language, e.g., "Bahasa Indonesia"
        public string Name
        {
            get
            {
                Debug.Assert(null != m_sName && m_sName.Length > 0);
                return m_sName;
            }
        }
        string m_sName;
        #endregion
        #region Attr{g}: int Index - which Alternative in LocItem to retrieve
        public int Index
        {
            get
            {
                return m_iIndex;
            }
        }
        int m_iIndex;
        #endregion
        #region Attr{g}: string FontName - the font for the UI in this language, or empty if Windows chooses
        public string FontName
        {
            get
            {
                return m_sFontName;
            }
            set
            {
                m_sFontName = value;
            }
        }
        string m_sFontName = "";
        #endregion
        #region Attr{g}: int FontSize - the font size fo the UI in this language, or empty if Windows chooses
        public int FontSize
        {
            get
            {
                return m_nFontSize;
            }
            set
            {
                m_nFontSize = value;
            }
        }
        int m_nFontSize = 0;
        #endregion

        // Scaffolding -----------------------------------------------------------------------
        #region Constructor(sID, sName, iIndex)
        public LocLanguage(string _sID, string _sName, int _iIndex)
        {
            m_sID = _sID;
            m_sName = _sName;
            m_iIndex = _iIndex;
        }
        #endregion

        // I/O -------------------------------------------------------------------------------
        public const string c_sTag = "Language";
        public const string c_sID = "ID";
        public const string c_sName = "Name";
        public const string c_sFontName = "Font";
        public const string c_sFontSize = "Size";
        #region Method: void WriteXML(XmlField xmlParent)
        public void WriteXML(XmlField xmlParent)
        {
            XmlField xml = xmlParent.GetDaughterXmlField(c_sTag, true);

            string s = xml.GetAttrString(c_sID, ID);
            s += xml.GetAttrString(c_sName, Name);

            if (!string.IsNullOrEmpty(FontName))
                s += xml.GetAttrString(c_sFontName, FontName);

            if (0 != FontSize)
                s += xml.GetAttrString(c_sFontSize, FontSize.ToString());

            xml.OneLiner(s, "");
        }
        #endregion
        #region SMethod: LocLanguage ReadXML(iIndex, XmlRead xml)
        static public LocLanguage ReadXML(int iIndex, XmlRead xml)
        {
            string sID = xml.GetValue(c_sID);
            string sName = xml.GetValue(c_sName);

            LocLanguage language = new LocLanguage(sID, sName, iIndex);

            string sFontName = xml.GetValue(c_sFontName);
            if (!string.IsNullOrEmpty(sFontName))
                language.FontName = sFontName;

            string sFontSize = xml.GetValue(c_sFontSize);
            if (!string.IsNullOrEmpty(sFontSize))
            {
                try
                {
                    int n = Convert.ToInt16(sFontSize);
                    if (n != 0)
                        language.FontSize = n;
                }
                catch (Exception)
                {
                }
            }


            return language;
        }
        #endregion
    }

    public class LocDB
    {
        // Groups ----------------------------------------------------------------------------
        #region Attr{g}: LocGroup[] Groups
        public LocGroup[] Groups
        {
            get
            {
                Debug.Assert(null != m_vGroups);
                return m_vGroups;
            }
        }
        LocGroup[] m_vGroups = null;
        #endregion
        #region Method: LocGroup FindGroup(sID)
        public LocGroup FindGroup(string sID)
        {
            foreach (LocGroup group in Groups)
            {
                if (group.ID == sID)
                    return group;

                LocGroup sub = group.FindGroup(sID);
                if (null != sub)
                    return sub;
            }
            return null;
        }
        #endregion
        #region Method: LocGroup ParseAndAddGroup(string sTitle)
        LocGroup ParseAndAddGroup(string sTitle)
        {
            // Build the ID from the string, getting rid of digits, whitespace and punctuation
            string sID = "";
            foreach (char c in sTitle)
            {
                if (Char.IsDigit(c))
                    continue;
                if (Char.IsWhiteSpace(c))
                    continue;
                if (Char.IsPunctuation(c))
                    continue;

                sID += c;
            }
            if (sID.Length == 0)
                return null;

            // Don't bother if it is the Language record (the first record in the database, generally)
            if (sID == c_Languages)
                return null;

            // See if we already have this group
            LocGroup group = FindGroup(sID);
            if (null != group)
                return group;

            // If not, create a new one and add it to the vector
            group = new LocGroup(sID, sTitle);
            AppendGroup(group);
            return group;
        }
        #endregion
        #region Method: void AddGroup(LocGroup group)
        void AppendGroup(LocGroup group)
        {
            if (null == group)
                return;

            LocGroup[] v = new LocGroup[Groups.Length + 1];
            for (int i = 0; i < Groups.Length; i++)
                v[i] = Groups[i];
            v[Groups.Length] = group;
            m_vGroups = v;
        }
        #endregion
        #region Method: LocGroup FindOrAddGroup(sGroupID)
        LocGroup FindOrAddGroup(string sGroupID)
        {
            LocGroup group = FindGroup(sGroupID);
            if (null == group)
            {
                group = new LocGroup(sGroupID, sGroupID);
                AppendGroup(group);
            }
            return group;
        }
        #endregion

        // Special Groups --------------------------------------------------------------------
        const string c_Strings = "Strings";
        const string c_Menus = "Menus";
        const string c_Toolbars = "ToolbarText";
        const string c_Messages = "Messages";
        #region Attr{g}: LocGroup Strings - the Group that contains misc strings
        LocGroup Strings
        {
            get
            {
                if (null == m_Strings)
                    m_Strings = FindGroup(c_Strings);

                Debug.Assert(null != m_Strings);

                return m_Strings;
            }
        }
        LocGroup m_Strings = null;
        #endregion      
        #region Attr{g}: LocGroup Menus - the Group that contains the menus
        LocGroup Menus
        {
            get
            {
                if (null == m_Menus)
                    m_Menus = FindGroup(c_Menus);

                Debug.Assert(null != m_Menus);

                return m_Menus;
            }
        }
        LocGroup m_Menus;
        #endregion
        #region Attr{g}: LocGroup Toolbars - the Group that contains the Toolbars
        LocGroup Toolbars
        {
            get
            {
                if (null == m_Toolbars)
                    m_Toolbars = FindGroup(c_Toolbars);

                Debug.Assert(null != m_Toolbars);

                return m_Toolbars;
            }
        }
        LocGroup m_Toolbars;
        #endregion
        #region Attr{g}: LocGroup Messages - the Group that contains the Messages
        LocGroup Messages
        {
            get
            {
                if (null == m_Messages)
                    m_Messages = FindGroup(c_Messages);

                Debug.Assert(null != m_Messages);

                return m_Messages;
            }
        }
        LocGroup m_Messages;
        #endregion

        // Languages -------------------------------------------------------------------------
        const string c_Languages = "Languages";  // the first record in the languages database
        const string c_mkrEnglish = "eng";
        #region Attr{g}: LocLanguage Languages
        public LocLanguage[] Languages
        {
            get
            {
                Debug.Assert(null != m_vLanguages);
                return m_vLanguages;
            }
        }
        LocLanguage[] m_vLanguages = null;
        #endregion
        #region Method: LocLanguage FindLanguage(sID)
        public LocLanguage FindLanguage(string sID)
        {
            foreach (LocLanguage lang in Languages)
            {
                if (lang.ID == sID)
                    return lang;
            }
            return null;
        }
        #endregion
        #region Method: LocLanguage FindLanguageByName(sLanguageName)
        public LocLanguage FindLanguageByName(string sLanguageName)
        {
            foreach (LocLanguage lang in Languages)
            {
                if (lang.Name == sLanguageName)
                    return lang;
            }
            return null;
        }
        #endregion
        #region Method: int GetIndexForLanguage(string sLanguageName)
        public int GetIndexForLanguage(string sLanguageName)
        {
            for (int i = 0; i < Languages.Length; i++)
            {
                if (sLanguageName == Languages[i].Name)
                    return i;
            }
            return -1;
        }
        #endregion
        #region Method: void ParseAndAddLanguage(string s)
        void ParseAndAddLanguage(string s)
        {
            // We'll parse the ID and Name into these variables
            string sID = "";
            string sName = "";

            // Work through the string
            bool bIDFinished = false;
            foreach (char c in s)
            {
                if (c == ' ' && !bIDFinished)
                {
                    bIDFinished = true;
                    continue;
                }

                if (bIDFinished)
                    sName += c;
                else
                    sID += c;
            }
            sID = sID.Trim();
            sName = sName.Trim();
            if (sID.Length == 0 || sName.Length == 0)
                return;

            // Don't add if it is English
            if (c_mkrEnglish == sID)
                return;

            // If the language is already present, we're done
            if (FindLanguage(sID) != null)
                return;

            // Otherwise, create and add the new one
            LocLanguage lang = new LocLanguage(sID, sName, Languages.Length);
            AppendLanguage(lang);
        }
        #endregion
        #region Method: void AppendLanguage(LocLanguage lang)
        void AppendLanguage(LocLanguage lang)
        {
            LocLanguage[] v = new LocLanguage[Languages.Length + 1];
            for (int i = 0; i < Languages.Length; i++)
                v[i] = Languages[i];
            v[Languages.Length] = lang;
            m_vLanguages = v;
        }
        #endregion
        #region Method: LocLanguage AppendLanguage(string sID, string sName)
        public LocLanguage AppendLanguage(string sID, string sName)
        {
            LocLanguage lang = new LocLanguage(sID, sName, Languages.Length);
            AppendLanguage(lang);
            return lang;
        }
        #endregion

        #region Attr{g/s}: LocLanguage PrimaryLanguage
        public LocLanguage PrimaryLanguage
        {
            get
            {
                return m_langPrimary;
            }
            set
            {
                m_langPrimary = value;
            }
        }
        LocLanguage m_langPrimary = null;
        #endregion
        #region Attr{g/s}: LocLanguage SecondaryLanguage
        public LocLanguage SecondaryLanguage
        {
            get
            {
                return m_langSecondary;
            }
            set
            {
                m_langSecondary = value;
            }

        }
        LocLanguage m_langSecondary = null;
        #endregion
        #region VAttr{g}: bool PrimaryIsEnglish
        public bool PrimaryIsEnglish
        {
            get
            {
                if (PrimaryLanguage == null)
                    return true;
                return false;
            }
        }
        #endregion

        #region Method: void SetSecondary(string sName)
        public void SetSecondary(string sName)
        {
            SecondaryLanguage = null;

            foreach (LocLanguage lang in Languages)
            {
                if (lang.Name == sName)
                    SecondaryLanguage = lang;
            }

            SetToRegistry();
        }
        #endregion

        #region Attr{g}: string[] LanguageChoices - includes English
        public string[] LanguageChoices
        {
            get
            {
                string[] v = new string[Languages.Length + 1];

                for (int i = 0; i < Languages.Length; i++)
                    v[i] = Languages[i].Name;

                v[Languages.Length] = LocItem.c_sEnglish;

                return v;
            }
        }
        #endregion

        // Registry --------------------------------------------------------------------------
        const string c_keyPrimary = "PrimaryUI";
        const string c_keySecondary = "SecondaryUI";
        #region Method: void GetFromRegistry()
        public void GetFromRegistry()
        {
            string sPrimary = JW_Registry.GetValue(c_keyPrimary, "");
            string sSecondary = JW_Registry.GetValue(c_keySecondary, "");

            foreach (LocLanguage lang in Languages)
            {
                if (lang.Name == sPrimary)
                    PrimaryLanguage = lang;

                if (lang.Name == sSecondary)
                    SecondaryLanguage = lang;
            }
        }
        #endregion
        #region Method: void SetToRegistry()
        public void SetToRegistry()
        {
            if (null != PrimaryLanguage)
                JW_Registry.SetValue(c_keyPrimary, PrimaryLanguage.Name);
            else
                JW_Registry.SetValue(c_keyPrimary, "");

            if (null != SecondaryLanguage)
                JW_Registry.SetValue(c_keySecondary, SecondaryLanguage.Name);
            else
                JW_Registry.SetValue(c_keySecondary, "");
        }
        #endregion

        // I/O -------------------------------------------------------------------------------
        #region SAttr{g/s}: string BaseName - Name of the file containing the basic information
        static public string BaseName
        {
            get
            {
                Debug.Assert(!string.IsNullOrEmpty(s_sBaseName));
                return s_sBaseName;
            }
            set
            {
                s_sBaseName = value;
            }
        }
        static string s_sBaseName = "PsLocalization.xml";
        #endregion
        #region Attr{g}: string BasePath - the file containing the basic information
        string BasePath
        {
            get
            {
                Debug.Assert(!string.IsNullOrEmpty(s_sBasePath));
                return s_sBasePath;
            }
        }
        static string s_sBasePath;
        #endregion
        #region Attr{g}: string DataFolder - the folder containing all of the loc files
        string DataFolder
        {
            get
            {
                Debug.Assert(!string.IsNullOrEmpty(s_sDataFolder));
                return s_sDataFolder;
            }
        }
        string s_sDataFolder;
        #endregion
        const string c_sTag = "LocDB";
        #region Method: void WriteXML(XmlField xmlParent)
        public void WriteXML()
        {
            // Open a Text Writer to save to
            TextWriter writer = JW_Util.GetTextWriter(BasePath);

            XmlField xml = new XmlField(writer, c_sTag);
            xml.Begin();

            foreach (LocGroup group in Groups)
                group.WriteXML(xml);

            xml.End();

            // Done
            writer.Close();

            // Write the language alternatives
            foreach (LocLanguage lang in Languages)
                WriteLanguageData(lang);
        }
        #endregion
        #region Method: void ReadXML()
        public void ReadXML()
        {
            // First, read in the Base data
            string sPathName = BasePath;
            if (!File.Exists(sPathName))
            {
                MessageBox.Show(string.Format("Please Copy the File {0} to the Loc Folder", BaseName));
                return;
            }

            TextReader reader = JW_Util.OpenStreamReader(ref sPathName, "*.xml");
            XmlRead xml = new XmlRead(reader);

            while (xml.ReadNextLineUntilEndTag(c_sTag))
            {
                // LocGroup
                if (xml.IsTag(LocGroup.c_sTag))
                {
                    LocGroup group = LocGroup.ReadXML(this, xml);
                    AppendGroup(group);
                }
            }

            reader.Close();

            // Now, read in the languages in the localization folder
            string[] sPaths = Directory.GetFiles(this.DataFolder, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (string s in sPaths)
            {
                if (s == BasePath)
                    continue;
                ReadLanguageData(s);
            }

        }
        #endregion
        #region Method: void WriteLanguageData(LocLanguage lang)
        void WriteLanguageData(LocLanguage lang)
        {
            // Build the language name
            string sPath = DataFolder + Path.DirectorySeparatorChar + lang.ID + ".xml";

            // Save a backup in the current folder (See Bug0282.)
            JW_Util.CreateBackup(sPath, ".bak");

            // Open the xml writer
            TextWriter w = JW_Util.GetTextWriter(sPath);
            XmlField xml = new XmlField(w, c_sTag);
            xml.Begin();

            // Write out the language information
            lang.WriteXML(xml);

            // Write out the group's data
            foreach (LocGroup group in Groups)
                group.WriteLanguageData(xml, lang);

            // Done
            xml.End();
            w.Close();
        }
        #endregion
        #region Method: void ReadLanguageData(string sPath)
        void ReadLanguageData(string sPath)
        {
            StreamReader r = new StreamReader(sPath, Encoding.UTF8);
            TextReader tr = TextReader.Synchronized(r);
            XmlRead xml = new XmlRead(tr);

            LocLanguage language = null;

            while (xml.ReadNextLineUntilEndTag(c_sTag))
            {
                if (xml.IsTag(LocLanguage.c_sTag))
                {
                    language = LocLanguage.ReadXML(Languages.Length, xml);
                    AppendLanguage(language);
                }

                // LocGroup
                if (null != language && xml.IsTag(LocGroup.c_sTag))
                {
                    string sGroupID = xml.GetValue(LocGroup.c_sID);
                    LocGroup group = FindGroup(sGroupID);
                    if (null != group)
                        group.ReadLanguageData(xml, language);
                }

            }
            tr.Close();
        }
        #endregion

        // Scaffolding -----------------------------------------------------------------------
        #region SMethod: void Initialize(string sPathApplicationFolder)
        static public void Initialize(string sPathApplicationFolder)
        {
            if (null == s_LocDB)
                s_LocDB = new LocDB(sPathApplicationFolder);
            Debug.Assert(null != s_LocDB);
        }
        #endregion
        #region SAttr{g}: LocDB DB
        static public LocDB DB
        {
            get
            {
                Debug.Assert(null != s_LocDB);
                return s_LocDB;
            }
        }
        static private LocDB s_LocDB = null;
        #endregion
        #region private Constructor(sPathApplicationFolder) - do not call (called by the Initialize Method above
        private LocDB(string sPathApplicationFolder)
        {
            // Initialize the vectors
            m_vGroups = new LocGroup[0];
            m_vLanguages = new LocLanguage[0];

            // Folder containing the localization data
            s_sDataFolder = sPathApplicationFolder + Path.DirectorySeparatorChar + "Loc";
            if (!Directory.Exists(DataFolder))
                Directory.CreateDirectory(DataFolder);

            // The main localization file
            s_sBasePath = DataFolder + Path.DirectorySeparatorChar + BaseName;

            // Read in the file
            ReadXML();

            // Retrieve the current settings from the registry
            GetFromRegistry();
        }
        #endregion
        #region SMethod: string AppTitle - Gets AssemblyProduct Name
        static public string SetAppTitle()
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(
                typeof (AssemblyProductAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            s_AppTitle = ((AssemblyProductAttribute) attributes[0]).Product;
            return s_AppTitle;
        }
        private static string s_AppTitle = "Pathway";
        #endregion

        // Messages ----------------------------C:\Users\JWimbish\Documents\Visual Studio 2005\Projects\OurWord\trunk\OurWord\Language.cs----------------------------------------------
        #region Method: string Insert(string sBase, string[] vsInsert)
        static public string Insert(string sBase, string[] v)
        {
            if (null == v || v.Length == 0)
                return sBase;

            string sReturn = sBase;

            for (int i = 0; i < v.Length; i++)
            {
                int iPos = sReturn.IndexOf("{" + i.ToString() + "}");
                if (iPos >= 0)
                {
                    string sFirst = sReturn.Substring(0, iPos);
                    string sLast = sReturn.Substring(iPos + 3);
                    sReturn = sFirst + v[i] + sLast;
                }
            }

            return sReturn;
        }
        #endregion
        public enum MessageTypes { Warning, WarningYN, WarningYNC,YN, Info, Error, InfoOkCancel, };
        public enum MessageDefault { First, Second, Third};
        #region SMethod: bool Message(sID, sDefaultEnglish, vsInsertions, MessageType,MessageDefaultButton)
        public static DialogResult Message(
            string sID, 
            string sDefaultEnglish, 
            string[] vsInsertions, 
            MessageTypes MessageType, MessageDefault msgDefaultButton)
        {
            s_sLastMessageID = sID;
            
            // Retrieve the localized form of the message
            LocItem item = DB.Messages.Find(sID);
            if (null == item)
            {
                item = new LocItem(sID);
                item.English = sDefaultEnglish;
                DB.Messages.AppendItem(item);
            }
            string sMessageText = item.AltValue;
            

            // Not Used
            //MessageBoxButtons.YesNoCancel
            //MessageBoxIcon.None
            


            // Perform the insertions
            sMessageText = Insert(sMessageText, vsInsertions);

            // Decide which button(s) and which icon to show in the message box
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1;
            switch (msgDefaultButton)
            {
                    //case MessageDefault.First:
                    //defaultButton = MessageBoxDefaultButton.Button1;
                    //break;
                case MessageDefault.Second:
                    defaultButton = MessageBoxDefaultButton.Button2;
                    break;
                case MessageDefault.Third:
                    defaultButton = MessageBoxDefaultButton.Button3;
                    break;

            }
            

            switch (MessageType)
            {
                case MessageTypes.Warning:
                    buttons = MessageBoxButtons.OK;
                    icon = MessageBoxIcon.Warning;
                    break;
                case MessageTypes.WarningYN:
                    buttons = MessageBoxButtons.YesNo;
                    icon = MessageBoxIcon.Warning;
                    break;
                case MessageTypes.YN:
                    buttons = MessageBoxButtons.YesNo;
                    icon = MessageBoxIcon.Question;
                    break;
                case MessageTypes.Info:
                    buttons = MessageBoxButtons.OK;
                    icon = MessageBoxIcon.Information;
                    break;
                case MessageTypes.Error:
                    buttons = MessageBoxButtons.OK;
                    icon = MessageBoxIcon.Error;
                    break;
                case MessageTypes.WarningYNC:
                    buttons = MessageBoxButtons.YesNoCancel;
                    icon = MessageBoxIcon.Question;
                    break;

                    
            }

            // Finally, we can show the message
            DialogResult result = MessageBox.Show(Form.ActiveForm,
                                                  sMessageText, s_AppTitle, buttons, icon, defaultButton);
            //return (result == DialogResult.Yes);
            return result;
        }

        public static DialogResult Message(
            string sID,
            string sApplicationOpenError,
            string sDefaultEnglish,
            string[] vsInsertions,
            MessageTypes MessageType, MessageDefault msgDefaultButton)
        {
            s_sLastMessageID = sID;

            // Retrieve the localized form of the message
            LocItem item = DB.Messages.Find(sID);
            if (null == item)
            {
                item = new LocItem(sID);
                item.English = sDefaultEnglish;
                DB.Messages.AppendItem(item);
            }
            string sMessageText = item.AltValue;


            // Not Used
            //MessageBoxButtons.YesNoCancel
            //MessageBoxIcon.None



            // Perform the insertions
            sMessageText = Insert(sMessageText, vsInsertions);

            // Decide which button(s) and which icon to show in the message box
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1;
            switch (msgDefaultButton)
            {
                //case MessageDefault.First:
                //defaultButton = MessageBoxDefaultButton.Button1;
                //break;
                case MessageDefault.Second:
                    defaultButton = MessageBoxDefaultButton.Button2;
                    break;
                case MessageDefault.Third:
                    defaultButton = MessageBoxDefaultButton.Button3;
                    break;

            }


            switch (MessageType)
            {
                case MessageTypes.Warning:
                    buttons = MessageBoxButtons.OK;
                    icon = MessageBoxIcon.Warning;
                    break;
                case MessageTypes.WarningYN:
                    buttons = MessageBoxButtons.YesNo;
                    icon = MessageBoxIcon.Warning;
                    break;
                case MessageTypes.YN:
                    buttons = MessageBoxButtons.YesNo;
                    icon = MessageBoxIcon.Question;
                    break;
                case MessageTypes.Info:
                    buttons = MessageBoxButtons.OK;
                    icon = MessageBoxIcon.Information;
                    break;
                case MessageTypes.Error:
                    buttons = MessageBoxButtons.OK;
                    icon = MessageBoxIcon.Error;
                    break;
                case MessageTypes.WarningYNC:
                    buttons = MessageBoxButtons.YesNoCancel;
                    icon = MessageBoxIcon.Question;
                    break;


            }

            // Finally, we can show the message
            DialogResult result = MessageBox.Show(Form.ActiveForm, sApplicationOpenError + " \n" + sMessageText, s_AppTitle, buttons, icon, defaultButton);
            //return (result == DialogResult.Yes);
            return result;
        }
        #endregion
        #region SAttr{g/s}: bool SuppressMessages - turns off displaying the msgs (e..g, for testing)
        static public bool SuppressMessages
        {
            get 
            { 
                return s_bSupressMessages; 
            }
            set 
            { 
                s_bSupressMessages = value; 
            }
        }
        static bool s_bSupressMessages = false;
        #endregion
        #region SAttr{g}: string LastMessageID
        static public string LastMessageID
        {
            get
            {
                return s_sLastMessageID;
            }
        }
        static string s_sLastMessageID = "";
        #endregion
        #region Method: void Reset() - clear out previous messages remembering
        static public void Reset()
        {
            s_sLastMessageID = "";
            SuppressMessages = false;
        }
        #endregion

        // Localize forms, user controls, tool strips; groupings of controls ----------------
        #region SMethod: void Localize(UserControl, PropertyBag)
        //static public void Localize(UserControl uc, PropertyBag bag)
        //{
        //    // Replace the appropriate PropertySpec attrs with their localized forms
        //    foreach (PropertySpec ps in bag.Properties)
        //    {
        //        // The Property's Name
        //        if (!ps.DontLocalizeName)
        //            ps.Name = GetValue(uc, ps.ID, ps.Name, null);

        //        // The Property's Help / Description
        //        if (!ps.DontLocalizeHelp)
        //            ps.Description = GetToolTip(uc, ps.ID, ps.Name, ps.Description);

        //        // The Property's Category, if applicable
        //        if (!string.IsNullOrEmpty(ps.Category) && !ps.DontLocalizeCategory)
        //            ps.Category = GetValue(uc, ps.Category, ps.Category, null);

        //        // The Property's string enumerations, if applicable
        //        if (null != ps.EnumValues && !ps.DontLocalizeEnums)
        //        {
        //            // YesNoPropertySpec is located in Common area
        //            if (ps as YesNoPropertySpec != null)
        //            {
        //                ps.EnumValues[0] = GetValue(null, "kYes", "Yes", null, null);
        //                ps.EnumValues[1] = GetValue(null, "kNo", "No", null, null);
        //            }

        //            // All others
        //            else
        //            {
        //                for (int i = 0; i < ps.EnumValues.Length; i++)
        //                {
        //                    // Default item is just appended to the property
        //                    string sItem = ps.ID + "_" + i.ToString();

        //                    // For an enumeration, we use, e.g., kLeft, kCentered, etc.
        //                    EnumPropertySpec eps = ps as EnumPropertySpec;
        //                    if (null != eps)
        //                        sItem = "option_" + eps.EnumIDs[i];

        //                    string sEnumValue = ps.EnumValues[i];

        //                    ps.EnumValues[i] = GetValue(uc, sItem, sEnumValue, null);

        //                    if ((string)ps.DefaultValue == sEnumValue)
        //                        ps.DefaultValue = GetValue(uc, sItem, sEnumValue, null);
        //                }
        //            }
        //        }

        //        // Sub-Bags (e.g., the FontBag in the StyleSheet)
        //        PropertyBag SubBag = ps.DefaultValue as PropertyBag;
        //        if (null != SubBag)
        //            Localize(uc, SubBag);
        //    }
        //}
        #endregion
        #region SMethod: void Localize(ToolStrip)
        static public void Localize(ToolStrip strip)
        {
            // yesu menu items.
            foreach (ToolStripItem tsi in strip.Items)
                Localize(tsi);
        }
        #endregion
        #region SMethod: void Localize(ToolStripItem)
        static public void Localize(ToolStripItem tsi)
        {
            // Certain controls are not localized
            if (tsi as ToolStripSeparator != null)
                return;

            // Certain controls are not localized   // yesu Added.
            if (tsi as ToolStripLabel != null)
                return;

            // The ID is the name of the item
            string sItemID = tsi.Name;
            if (string.IsNullOrEmpty(sItemID))
                return;

            // Calculate/retrieve the group ID
            string[] vGroupID = _GetGroupID(tsi);

            // Get the ToolStripItem's text value
            tsi.Text = GetValue(
                vGroupID,
                sItemID,
                tsi.Text,
                null,
                null);

            // Get its tooltip
            tsi.ToolTipText = GetToolTip(
                vGroupID,
                sItemID,
                tsi.Text,
                (DB.PrimaryIsEnglish ? tsi.ToolTipText : null));

            // Get its Shortcut key
            ToolStripMenuItem mi = tsi as ToolStripMenuItem;
            if (null != mi)
            {
                // The ShortcutKeyDisplayString does not always work; we'll just fix
                // the ones that are "Ctrl+"
                if (mi.ShortcutKeyDisplayString == null && mi.ShortcutKeys != Keys.None)
                {
                    if ( (mi.ShortcutKeys & Keys.Control ) == Keys.Control)
                    {
                        string s = "Ctrl+";
                        s += mi.ShortcutKeys.ToString()[0];
                        mi.ShortcutKeyDisplayString = s;
                    }
                }

                mi.ShortcutKeys = GetShortcutKey(
                    vGroupID,
                    sItemID,
                    tsi.Text,
                    (DB.PrimaryIsEnglish ? mi.ShortcutKeyDisplayString : null));
            }
            
            // Recurse if this is a drop-down item
            ToolStripDropDownItem tsiDropDown = tsi as ToolStripDropDownItem;
            if (tsiDropDown != null)
            {
                foreach (ToolStripItem tsiChild in tsiDropDown.DropDownItems)
                    Localize(tsiChild);
            }
        }
        #endregion
        #region SMethod: void Localize(ColumnHeader col)
        static void Localize(ColumnHeader col)
        {
            // The Name ID would ideally be the name of the control, but the
            // Name comes in as "" at runtime, regardless of what is placed
            // into the VS editor. So I use an index value instead. Lazy of MS.
            string sItemID = col.ListView.Name + "_col" + col.DisplayIndex.ToString();
            Debug.Assert(!string.IsNullOrEmpty(sItemID));

            string[] vGroupID = _GetGroupID(col);

            col.Text = GetValue(
                vGroupID,
                sItemID,
                col.Text,
                null,
                null);
        }
        #endregion
        #region SMethod: void Localize(ListView, vExclude)
        static public void Localize(ListView lv, Control[] vExclude)
        {
            // Check through the exclude list
            if (_Exclude(lv, vExclude))
                return;

            // All we want to do here is to localize the column headers
            foreach (ColumnHeader col in lv.Columns)
                Localize(col);
        }
        #endregion
        #region SMethod: void Localize(Control, vExclude) - includes UserControls
        static public void Localize(Control ctrl, Control[] vExclude)
        {
            // Recurse for any childrem. Do this prior to the Exclude test, because we don't want an 
            // exclusion to also exclude the children. Don't do UserControls, because we expect them
            // to handle localization in their own onLoad handlers (otherwise, the loc info gets into
            // the xml file twice.) Also don't do PropertyGrids, because we are using PropertyBags
            // instead.
            foreach (Control subCtrl in ctrl.Controls)
            {
                if (subCtrl as UserControl == null && subCtrl as PropertyGrid == null)
                {
                    // For some reason, the compiler is not calling the right version of
                    // localize, so I have to force it via a type cast.
                    if (null != subCtrl as ListView)
                        Localize(subCtrl as ListView, vExclude);
                    else
                        Localize(subCtrl, vExclude);
                }
            }

            // Check through the exclude list
            if (_Exclude(ctrl, vExclude))
                return;

            // If the control has no text, then it isn't meant to be localized (e..g,
            // a combo box.)
            if (string.IsNullOrEmpty(ctrl.Text))
                return;

            // Get the address of the containing group
            string[] vGroupID = _GetGroupID(ctrl);

            // The name of the control is the ItemID
            string sItemID = ctrl.Name;
            if (sItemID == null || sItemID.Length == 0)
                return;

            // Get the control's text  // yesu - This is the vital task
            ctrl.Text = GetValue(
                vGroupID,
                sItemID,
                ctrl.Text,
                null,
                null);
        }
        #endregion
        #region SMethod: void Localize(Form, vExclude)

        static public void Localize(Form form, Control[] vExclude)
        {
            // Localize the form's title
            form.Text = GetValue(form, "Title", form.Text);

            // Loop through the controls in the form
            foreach (Control ctrl in form.Controls)
                Localize(ctrl, vExclude);
        }
        #endregion
        #region SMethod: void Localize(MenuStrip)
        static public void Localize(MenuStrip strip)
        {
            // yesu menu items.
            foreach (ToolStripMenuItem tsi in strip.Items)
            {
                Localize(tsi);
                if (tsi.HasDropDownItems)
                {
                    foreach (var tsiChild in tsi.DropDownItems)
                    {
                        if (tsiChild is ToolStripMenuItem)
                        {
                            Localize((ToolStripMenuItem)tsiChild);
                        }
                    }
                }
            }

        }
        static public void Localize(ToolStripMenuItem tsi)
        {
            // The ID is the name of the item
            string sItemID = tsi.Name;
            if (string.IsNullOrEmpty(sItemID))
                return;

            // Calculate/retrieve the group ID
            string[] vGroupID = _GetGroupID(tsi);

            // Get the ToolStripMenuItem's text value
            tsi.Text = GetValue(
                vGroupID,
                sItemID,
                tsi.Text,
                null,
                null);

            // Get its tooltip
            tsi.ToolTipText = GetToolTip(
                vGroupID,
                sItemID,
                tsi.Text,
                (DB.PrimaryIsEnglish ? tsi.ToolTipText : null));

            // Get its Shortcut key
            ToolStripMenuItem mi = tsi as ToolStripMenuItem;
            if (null != mi)
            {
                // The ShortcutKeyDisplayString does not always work; we'll just fix
                // the ones that are "Ctrl+"
                if (mi.ShortcutKeyDisplayString == null && mi.ShortcutKeys != Keys.None)
                {
                    if ((mi.ShortcutKeys & Keys.Control) == Keys.Control)
                    {
                        string s = "Ctrl+";
                        s += mi.ShortcutKeys.ToString()[0];
                        mi.ShortcutKeyDisplayString = s;
                    }
                }

                mi.ShortcutKeys = GetShortcutKey(
                    vGroupID,
                    sItemID,
                    tsi.Text,
                    (DB.PrimaryIsEnglish ? mi.ShortcutKeyDisplayString : null));
            }

            // Recurse if this is a drop-down item
            //ToolStripMenuItem tsiDropDown = tsi as ToolStripMenuItem;
            //if (tsiDropDown != null)
            //{
            //    foreach (ToolStripMenuItem tsiChild in tsiDropDown.DropDownItems)
            //        Localize(tsiChild);
            //}
        }

        #endregion
 
        // Private helper methods for Localize and Retrieval Methods -------------------------
        #region SMethod: string[] _GetGroupID(Object obj)
        static string[] _GetGroupID(Object obj)
        {
            string sErrorMessage = " must have a name to serve as the GroupID for localization";

            Debug.Assert(null != obj);

            // Form
            Form form = obj as Form;
            if (null != form)
            {
                Debug.Assert(!string.IsNullOrEmpty(form.Name), "Form" + sErrorMessage);
                return new string[] { form.Name };
            }

            // User Control
            UserControl uc = obj as UserControl;
            if (null != uc)
            {
                string[] vFormGroupID = _GetGroupID(uc.ParentForm);
                Debug.Assert(!string.IsNullOrEmpty(uc.Name), "User Control" + sErrorMessage);
                return new string[] {
                                        vFormGroupID[0],
                                        vFormGroupID[0] + "_" + uc.Name
                                    };
            }

            // ToolStrip
            ToolStrip toolstrip = obj as ToolStrip;
            if (null != toolstrip)
            {
                // If we are nested, then get the top-level ToolStrip
                if (null != toolstrip.Parent as ToolStrip)
                    return _GetGroupID(toolstrip.Parent);

                // Otherwise, we are at the top level, so return its name
                Debug.Assert(!string.IsNullOrEmpty(toolstrip.Name), "ToolStrip" + sErrorMessage);
                return new string[] { toolstrip.Name };
            }

            // ToolStripItem
            ToolStripItem tsi = obj as ToolStripItem;
            if (null != tsi)
            {
                // Work up the ownership chain while we encounter ToolStripItems
                if (null != tsi.OwnerItem && (tsi.OwnerItem as ToolStripOverflowButton == null))
                    return _GetGroupID(tsi.OwnerItem);
                // Once we get here, we should be at the owning ToolStrip
                Debug.Assert(null != tsi.Owner);
                return _GetGroupID(tsi.Owner); 
            }

            // ColumnHeader
            ColumnHeader col = obj as ColumnHeader;
            if (null != col)
            {
                ListView lv = col.ListView;
                if (null != lv)
                    return _GetGroupID( lv.Parent );
            }

            // Other type of control
            Control ctrl = obj as Control;
            if (null != ctrl)
                return _GetGroupID(ctrl.Parent);

            Debug.Assert(false);
            return null;
        }
        #endregion
        #region SMethod: bool _Exclude(Control ctrl, Control[] vExcludeList)
        static bool _Exclude(Control ctrl, Control[] vExcludeList)
        {
            if (null == vExcludeList)
                return false;

            foreach (Control ctrlExclude in vExcludeList)
            {
                if (ctrlExclude == ctrl)
                    return true;
            }
            return false;
        }
        #endregion
        #region SMethod: bool _Exclude(string sItem, string[] vExcludeList)
        static bool _Exclude(string sItem, string[] vsExcludeList)
        {
            if (null == vsExcludeList)
                return false;

            foreach (string s in vsExcludeList)
            {
                if (s == sItem)
                    return true;
            }
            return false;
        }
        #endregion

        // Retrieval of strings from the proper alternate of the LocItem ---------------------
        #region SMethod: LocItem GetLocItem(vGroupID, sItemID, sEnglish)
        static LocItem GetLocItem(
            string[] vGroupID, 
            string sItemID,
            string sEnglish
            )
        {
            // Default to not being able to find it
            LocItem item = null;

            // First, see if it is in the vGroupID path
            LocGroup group = null;
            if (vGroupID != null)
            {
                foreach (string sGroupID in vGroupID)
                {
                    if (group == null)
                        group = DB.FindOrAddGroup(sGroupID);
                    else
                        group = group.FindOrAddGroup(sGroupID);
                }
                item = group.Find(sItemID);
            }

            // If not, see if it is defined in the Strings area
            if (null == item)
            {
                LocGroup gStrings = DB.FindGroup(c_Strings);
                if (gStrings != null) // added newly to avoid null
                {
                    item = gStrings.FindRecursively(sItemID);
                }
            }

            // If not, add it to the vGroupID group
            if (null == item && null != group)
            {
                item = group.FindOrAddItem(sItemID, sEnglish);
            }

            return item;
        }
        #endregion
        // Retrieve a Value ------------------------------------------------------------------
        #region SMethod: string GetValue(vGroupID, sItemID, sEnglish, sLanguage, vsInsert) - Workhorse method
        /// <summary>
        /// Looks up the string according to its GroupID/ItemID, and returns its localized value. 
        /// </summary>
        /// <param name="vGroupID">The path to the group containing the localization item</param>
        /// <param name="sItemID">The ID of the localization item</param>
        /// <param name="sEnglish">The English value, should the language value not be found</param>
        /// <param name="sLanguage">If non-null, the method will return the localization in the 
        /// language requested. Otherwise, the return value is in accordance with the Primary and 
        /// Secondary languages.</param>
        /// <param name="vsInsert">If non-null, these values replace {0}, {1}, ..., in the value string.</param>
        /// <returns>The localized string value, or English if it does not exist in the
        /// requested language.</returns>
        /// <remarks>This is the workhorse method that other methods (with simplified parameters) 
        /// should call. If the item does not exist in the database, then one is added.</remarks>
        static public string GetValue( 
            string[] vGroupID, 
            string sItemID, 
            string sEnglish,
            string sLanguage,
            string[] vsInsert)
        {
            // Find (or add) the item, either in vGroupID, or in the Strings hierarchy
            LocItem item = GetLocItem(vGroupID, sItemID, sEnglish);
            Debug.Assert(null != item);

            // If a language was specified, then attempt to find it; returning English otherwise.
            // We use this, e.g., for the FileNameLanguage
            // TODO: We are assuming that vsInsert is not desired for these, based on current
            //   usage in OurWord. Hence the assertion.
            //if (!string.IsNullOrEmpty(sLanguage))
            if (!string.IsNullOrEmpty(sLanguage))
            {
                Debug.Assert(null == vsInsert); // See the TODO above.
                LocLanguage language = LocDB.DB.FindLanguageByName(sLanguage);
                if (null == language)
                    return sEnglish;
                LocAlternate alt = item.GetAlternate(language.Index);
                if (null == alt || string.IsNullOrEmpty(alt.Value))
                    return sEnglish;
                return alt.Value;
            }

            // Was an insertion desired?
            if (null != vsInsert)
                return Insert(item.AltValue, vsInsert);

            // Otherwise, we want to retrieve the string according to the Primary and
            // Secondary languages, as set up in the settings.
            return item.AltValue;
        }
        #endregion
        #region SMethod: string GetValue(Form, sItemID, sEnglish) - gets GroupID from Form
        static public string GetValue(Form form, string sItemID, string sEnglish)
        {
            // Call the workhorse
            return GetValue(
                _GetGroupID(form),
                sItemID,
                sEnglish,
                null,
                null);
        }
        #endregion
        #region SMethod: string GetValue(UserControl, sItemID, sEnglish, vsInsert) - gets GroupID from the UC
        static public string GetValue(UserControl uc, string sItemID, string sEnglish, string[] vsInsert)
        {
            // Call the workhorse
            return GetValue(
                _GetGroupID(uc),
                sItemID,
                sEnglish,
                null,
                vsInsert);
        }
        #endregion
        // Retrieve a ToolTip ----------------------------------------------------------------
        #region SMethod: string GetToolTip(vGroupID, sItemID, sEnglish, sEnglishToolTip) - Workhorse method
        static public string GetToolTip(
            string[] vGroupID,
            string sItemID,
            string sEnglish,
            string sEnglishToolTip)
        {
            // Find (or add) the item, either in vGroupID, or in the Strings hierarchy
            LocItem item = GetLocItem(vGroupID, sItemID, sEnglish);
            Debug.Assert(null != item);

            // Make sure the item has the default English tooltip
            if (string.IsNullOrEmpty(item.ToolTip) & !string.IsNullOrEmpty(sEnglishToolTip))
                item.ToolTip = sEnglishToolTip;

            // Otherwise, we want to retrieve the string according to the Primary and
            // Secondary languages, as set up in the settings.
            return item.AltToolTip;
        }
        #endregion
        #region SMethod: string GetToolTip(UserControl, sItemID, sEnglish, sEnglishToolTip) - gets GroupID from the UC
        static public string GetToolTip(UserControl uc, string sItemID, string sEnglish, string sEnglishToolTip)
        {
            // Call the workhorse
            return GetToolTip(
                _GetGroupID(uc),
                sItemID,
                sEnglish,
                sEnglishToolTip);
        }
        #endregion
        // Retrieve a ShortcutKey ------------------------------------------------------------
        #region SMethod: Keys GetShortcutKey(vGroupID, sItemID, sEnglish, sEnglishShortcutKey) - Workhorse method
        static public Keys GetShortcutKey(
            string[] vGroupID,
            string sItemID,
            string sEnglish,
            string sEnglishShortcutKey)
        {
            // Find (or add) the item, either in vGroupID, or in the Strings hierarchy
            LocItem item = GetLocItem(vGroupID, sItemID, sEnglish);
            Debug.Assert(null != item);

            // Make sure the item has the default English Shortcut Key
            if (string.IsNullOrEmpty(item.ShortcutKey) && !string.IsNullOrEmpty(sEnglishShortcutKey))
                item.ShortcutKey = sEnglishShortcutKey;

            // Otherwise, we want to retrieve the string according to the Primary and
            // Secondary languages, as set up in the settings.
            string sKeys = item.AltShortcutKey;

            // Convert to the Keys type
            try
            {
                KeysConverter converter = new KeysConverter();
                if (!string.IsNullOrEmpty(sKeys))
                {
                    Keys k = (Keys)converter.ConvertFromString(sKeys);
                    return k;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Shortcut key conversion problem: " + e.Message);
            }
            return Keys.None;
        }
        #endregion



    }
}