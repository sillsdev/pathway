/**********************************************************************************************
 * Dll:     JWTools
 * File:    Localizer.cs
 * Author:  John Wimbish
 * Created: 22 Aug 2007
 * Purpose: Localization tool.
 * Legal:   Copyright (c) 2005-08, John S. Wimbish. All Rights Reserved.  
 *********************************************************************************************/
#region Using
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using SIL.Tool;
using SIL.Tool.Localization;

#endregion

namespace JWTools
{
    public partial class Localizer : Form
    {
        // Content Attrs ---------------------------------------------------------------------
        #region Attr{g}: LocDB DB
        LocDB DB
        {
            get
            {
                Debug.Assert(null != m_DB);
                return m_DB;
            }
        }
        LocDB m_DB;
        #endregion
        #region Attr{g/s}: LocGroup Group
        LocGroup Group
        {
            get
            {
                return m_Group;
            }
            set
            {
                // Set the group value
                m_Group = value;
                if (null == m_Group)
                    return;

                // Display the first item
                if (Group.Items.Length == 0)
                    Item = null;
                else if (!FilterThoseNeedingAttentionOnly)
                    Item = Group.Items[0];
                else
                {
                    foreach (LocItem item in Group.Items)
                    {
                        if (ItemNeedsAttention(item))
                        {
                            Item = item;
                            return;
                        }
                    }
                    Item = null;
                }

            }
        }
        LocGroup m_Group;
        #endregion
        #region Attr{g/s}: LocItem Item
        LocItem Item
        {
            get
            {
                return m_Item;
            }
            set
            {
                m_Item = value;
                if (null != m_Item)
                    PopulateItemControls();
            }
        }
        LocItem m_Item;
        #endregion
        #region VAttr{g/s}: int iLanguage - the current language we're localizing
        int iLanguage
        {
            get
            {
                return m_iLanguage;
            }
            set
            {
                m_iLanguage = value;
            }
        }
        int m_iLanguage = -1;
        #endregion

        // Filters ---------------------------------------------------------------------------
        const string c_ShowAll = "Show All";
        const string c_ThoseNeedingAttention = "All Needing Attention";
        const string c_TranslatorItems = "Translator Items";
        const string c_TranslatorItemsNeedingAttention = "Translator Items Needing Attention";
        const string c_AdvisorItems = "Advisor Items";
        const string c_AdvisorItemsNeedingAttention = "Advisor Items Needing Attention";

        // Dialog Controls -------------------------------------------------------------------
        #region VAttr{g}: TreeView Tree
        TreeView Tree
        {
            get
            {
                return m_tree;
            }
        }
        #endregion
        #region VAttr{g}: ToolStripComboBox FilterCombo
        ToolStripComboBox FilterCombo
        {
            get
            {
                return m_comboFilter;
            }
        }
        #endregion
        #region VAttr{g}: ToolStripComboBox LanguageCombo
        ToolStripComboBox LanguageCombo
        {
            get
            {
                return m_comboLanguage;
            }
        }
        #endregion
        #region VAttr{g/s}: string ItemYourLanguage
        string ItemYourLanguage
        {
            get
            {
                string sIn = m_textYourLanguage.Text;
                string sOut = "";

                for (int i = 0; i < sIn.Length; )
                {
                    if (i < sIn.Length - 3 && sIn.Substring(i, 3) == "{n}")
                    {
                        sOut += '\n';
                        i += 3;
                    }
                    else if (sIn[i] == '\n') // Eat any new \n's that were introduced
                    {
                        i++;
                    }
                    else
                    {
                        sOut += sIn[i];
                        i++;
                    }
                }

                return sOut;
            }
            set
            {
                string s = "";

                foreach (char ch in value)
                {
                    if (ch == '\n')
                        s += "{n}";
                    else
                        s += ch;
                }
                m_textYourLanguage.Text = s;
            }
        }
        #endregion

        // VAttrs ----------------------------------------------------------------------------
        #region VAttr{g}: int iItem - the position of Item in the Group.Items vector
        int iItem
        {
            get
            {
                for (int i = 0; i < Group.Items.Length; i++)
                {
                    if (Group.Items[i] == Item)
                        return i;
                }
                return -1;
            }
        }
        #endregion
        #region VAttr{g}: bool FilterThoseNeedingAttentionOnly
        bool FilterThoseNeedingAttentionOnly
        {
            get
            {
                if (FilterCombo.Text == c_ThoseNeedingAttention)
                    return true;
                return false;
            }
        }
        #endregion
        #region VAttr{g}: LocLanguage Language
        LocLanguage Language
        {
            get
            {
                return DB.Languages[iLanguage];
            }
        }
        #endregion

        // Scaffolding -----------------------------------------------------------------------
        #region Constructor(LocDB)
        public Localizer(LocDB _DB)
        {
            m_DB = _DB;

            InitializeComponent();

            LocLanguage language = _DB.PrimaryLanguage;
            if (null != language)
                m_iLanguage = language.Index;
            else if (_DB.Languages.Length > 0)
                m_iLanguage = 0;
        }
        #endregion

        // Internal Methods ------------------------------------------------------------------
        #region Method: bool ItemNeedsAttention(LocItem item)
        bool ItemNeedsAttention(LocItem item)
        {
            LocAlternate alt = item.GetAlternate(iLanguage);
            if (null == alt)
                return true;
            bool bNeedsAttention = !string.IsNullOrEmpty(alt.NeedsAttention(item));
            return bNeedsAttention;
        }
        #endregion
        #region Method: bool GroupNeedsAttention(LocGroup group)
        bool GroupNeedsAttention(LocGroup group)
        {
            foreach (LocItem item in group.Items)
            {
                if (ItemNeedsAttention(item))
                    return true;
            }

            foreach (LocGroup sub in group.Groups)
            {
                if (GroupNeedsAttention(sub))
                    return true;
            }

            return false;
        }
        #endregion
        #region Method: bool ShouldDisplayInTree(LocGroup group)
        bool ShouldDisplayInTree(LocGroup group)
        {
            // Filter Option: Show All
            if (FilterCombo.Text == c_ShowAll)
                return true;

            // Filter Option: All Needing Attention
            if (FilterCombo.Text == c_ThoseNeedingAttention)
                return GroupNeedsAttention(group);

            // Filter Option: Translator Items
            if (FilterCombo.Text == c_TranslatorItems)
                return group.TranslatorAudience;

            // Filter Option: Translator Items Needing Attention
            if (FilterCombo.Text == c_TranslatorItemsNeedingAttention)
                return group.TranslatorAudience && GroupNeedsAttention(group);

            // Filter Option: Advisor Items
            if (FilterCombo.Text == c_AdvisorItems)
                return !group.TranslatorAudience;

            // Filter Option: Advisor Items Needing Attention
            if (FilterCombo.Text == c_AdvisorItemsNeedingAttention)
                return !group.TranslatorAudience && GroupNeedsAttention(group);

            // Unknown filter
            return true;
        }
        #endregion
        #region Method: bool ShouldDisplayInTree(LocItem item)
        bool ShouldDisplayInTree(LocItem item)
        // Assume the parent Group has already been filtered
        {
            // Filter Option: All Needing Attention
            if (FilterCombo.Text == c_ThoseNeedingAttention)
                return ItemNeedsAttention(item);

            // Filter Option: Advisor Items
            if (FilterCombo.Text == c_TranslatorItemsNeedingAttention)
                return ItemNeedsAttention(item);

            // Filter Option: Advisor Items Needing Attention
            if (FilterCombo.Text == c_AdvisorItemsNeedingAttention)
                return ItemNeedsAttention(item);

            // Unknown filter
            return true;
        }
        #endregion
        #region Method: void CalculateCounts()
        int m_cItemsRemaining;
        int m_cItemsTotal;

        void _CalculateCounts(LocGroup group)
        {
            if (!ShouldDisplayInTree(group))
                return;

            foreach (LocGroup sub in group.Groups)
                _CalculateCounts(sub);

            foreach (LocItem item in group.Items)
            {
                if (!ShouldDisplayInTree(item))
                    continue;

                ++m_cItemsTotal;

                if (ItemNeedsAttention(item))
                    ++m_cItemsRemaining;
            }

        }

        void CalculateCounts()
        {
            m_cItemsRemaining = 0;
            m_cItemsTotal = 0;

            foreach (LocGroup group in DB.Groups)
                _CalculateCounts(group);

            // Update the status
            m_lblRemaining.Text = m_cItemsRemaining.ToString() +
                " Remaining of " +
                m_cItemsTotal.ToString();

        }
        #endregion

        #region Method: void PopulateTree()
        void _PopulateTree(TreeNodeCollection nodes, LocGroup group)
        {
            // Determine whether this node is needed
            if (!ShouldDisplayInTree(group))
                return;

            // Add the node for this group
            TreeNode node = nodes.Add(group.Title);
            node.Name = group.ID;
            node.ToolTipText = group.Title;
            node.Tag = group;
            node.ForeColor = (GroupNeedsAttention(group) ? Color.Red : Color.Navy);

            // Recurse to add any subnodes for the subgroups
            foreach (LocGroup sub in group.Groups)
                _PopulateTree(node.Nodes, sub);

            // Add the items 
            foreach (LocItem item in group.Items)
            {
                if (!ShouldDisplayInTree(item))
                    continue;

                // Create the node with this text
                TreeNode nodeItem = node.Nodes.Add(item.English);

                // The node's name is the item's unique ID
                nodeItem.Name = item.ID;

                // The tooltip hels when everything can't fit on the screen
                nodeItem.ToolTipText = item.English;

                // We'll use the tag to get back to the correct item
                nodeItem.Tag = item;

                // The forecolor highlights which items need work
                nodeItem.ForeColor = (ItemNeedsAttention(item) ? Color.Red : Color.Navy);
            }
            node.Collapse(true);
        }
        void PopulateTree()
        {
            // Don't do this if we don't yet have a language selected (e.g., potentially
            // being called during cmdLoad
            if (-1 == iLanguage)
                return;

            LocGroup FirstGroup = null;

            Tree.Nodes.Clear();
            foreach (LocGroup group in DB.Groups)
            {
                _PopulateTree(Tree.Nodes, group);

                if (ShouldDisplayInTree(group) && null == FirstGroup)
                    FirstGroup = group;
            }

            if (null == FirstGroup)
                FirstGroup = DB.Groups[0];

            Group = FirstGroup;

            CalculateCounts();
        }
        #endregion
        #region Method: void ClearItemControls()
        void ClearItemControls()
        {
            m_rtbInfo.Text = "";
            ItemYourLanguage = "";
            m_comboShortcutKey.Text = "";
            m_textToolTip.Text = "";
        }
        #endregion
        #region Method: _TextToRtfBox(string sIn)
        string _TextToRtfBox(string sIn)
        {
            string sOut = "";

            foreach (char ch in sIn)
            {
                switch (ch)
                {
                    case '\n':
                        sOut += ("\\" + "{n" + "\\" + "}");
                        break;
                    case '{':
                        sOut += "\\{";
                        break;
                    case '}':
                        sOut += "\\}";
                        break;
                    default:
                        sOut += ch;
                        break;
                }
            }

            return sOut;
        }
        #endregion
        #region Method: void PopulateItemControls()
        void PopulateItemControls()
        {
            // Start with completely empty controls
            ClearItemControls();

            if (null == Item)
                return;

            // We'll build the contents of the Info Rich Text Box here
            string sRTF = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033";
            sRTF += "{\\fonttbl" +
                "{\\f0\\fswiss\\fprq2\\fcharset0 Arial;}" +
                "{\\f1\\fswiss\\fprq2\\fcharset0 Arial Narrow;}" +
                "{\\f2\\fswiss\\fprq2\\fcharset0 Microsoft Sans Serif;}" +
                "}";
            sRTF += "{\\colortbl ;\\red0\\green0\\blue128;}";

            // First Paragraph: Group Title and Group Description
            sRTF += "\\pard{\\b\\f0\\fs22 " + Group.Title + ": }";
            sRTF += "{\\f2\\fs16 " + Group.Description + "}\\par";

            // Skip a line
            sRTF += "{\\f2\\fs16 }\\par";

            // Item to be translated intro
            sRTF += "\\pard{\\b\\f0\\fs22 Item to be Translated: }";
            sRTF += "\\par";

            // Item ID
            sRTF += "\\pard{\\f0\\fs14 Internal ID:  }";
            sRTF += "{\\f2\\fs14 (" + Item.ID + ")}";
            sRTF += "\\par";

            // Item Description
            sRTF += "\\pard{\\f0\\fs16 Description:  }";
            sRTF += "{\\f2\\fs16 " + Item.Information + "}";
            sRTF += "\\par";

            // Item in English
            sRTF += "{\\f2\\fs16 }\\par";
            sRTF += "\\pard{\\f2\\fs18 English:  }";
            sRTF += "{\\f0\\fs28\\b\\cf1 " + _TextToRtfBox(Item.English) + "}";
            sRTF += "\\par";

            // Item Shortcut Key
            if (Item.CanHaveShortcutKey)
            {
                string s = Item.ShortcutKey;
                if (string.IsNullOrEmpty(s))
                    s = "(none)";
                sRTF += "{\\f2\\fs16 }\\par";
                sRTF += "\\pard{\\f2\\fs18 Shortcut key:  }";
                sRTF += "{\\f0\\fs28\\b\\cf1 " + s + "}";
            }

            // Item Tooltip
            if (Item.CanHaveToolTip)
            {
                string s = Item.ToolTip;
                if (string.IsNullOrEmpty(s))
                    s = "(none)";
                sRTF += "{\\f2\\fs16 }\\par";
                sRTF += "\\pard{\\f2\\fs18 ToolTip:  }";
                sRTF += "{\\f0\\fs28\\b\\cf1 " + s + "}";
            }

            // Place into the control
            sRTF += "}";
            m_rtbInfo.Rtf = sRTF;

            // What's currently in the language
            LocAlternate altYours = Item.GetAlternate(iLanguage);
            ItemYourLanguage = ((null == altYours) ? "" : altYours.Value);

            // Shortcut Key, if any
            if (Item.CanHaveShortcutKey)
            {
                if (null != altYours)
                    m_comboShortcutKey.Text = altYours.ShortcutKey;
                m_comboShortcutKey.Enabled = true;
                m_lblShortcutKey.Enabled = true;
            }
            else
            {
                m_comboShortcutKey.Text = "";
                m_comboShortcutKey.Enabled = false;
                m_lblShortcutKey.Enabled = false;
            }

            // Tooltip, if any
            if (Item.CanHaveToolTip)
            {
                if (null != altYours)
                    m_textToolTip.Text = altYours.ToolTip;
                m_textToolTip.Enabled = true;
                m_lblToolTip.Enabled = true;
            }
            else
            {
                m_textToolTip.Text = "";
                m_textToolTip.Enabled = false;
                m_lblToolTip.Enabled = false;
            }

            // Status
            m_lblRemaining.Text = "";

            // Update controls & status
            UpdateNeedsAttentionStatus();
            CalculateCounts();
        }
        #endregion
        #region Method: void HarvestChanges()
        void HarvestChanges()
        {
            if (null == Item || -1 == iLanguage)
                return;

            LocAlternate alt = new LocAlternate(ItemYourLanguage, null, null);

            Item.AddAlternate(iLanguage, alt);
            alt.ShortcutKey = m_comboShortcutKey.Text;
            alt.ToolTip = m_textToolTip.Text;
        }
        #endregion
        #region Method: void PopulateLanguageCombo()
        void PopulateLanguageCombo()
        {
            LanguageCombo.Items.Clear();

            if (LocDB.DB.Languages.Length > 0)
            {
                foreach (LocLanguage lang in LocDB.DB.Languages)
                    LanguageCombo.Items.Add(lang.Name);
                //LanguageCombo.Text = LocDB.DB.Languages[iLanguage].Name;
                LanguageCombo.Text = LocDB.DB.Languages[0].Name; // yesu
            }
            else
                LanguageCombo.Enabled = false;
        }
        #endregion
        #region Method: void SetFont()
        void SetFont()
        {
            // Default font is the one windows provides
            Font font = SystemFonts.DialogFont;

            // Override if necessary
            if (!string.IsNullOrEmpty(Language.FontName) || Language.FontSize != 0)
            {
                string sFontName = Language.FontName;
                if (string.IsNullOrEmpty(sFontName))
                    sFontName = SystemFonts.DialogFont.Name;

                float fSize = Language.FontSize;
                if (fSize == 0)
                    fSize = SystemFonts.DialogFont.Size;

                font = new Font(sFontName, fSize);
            }

            m_textToolTip.Font = font;
            m_textYourLanguage.Font = font;
            m_labelYourLanguage.Font = font;
        }
        #endregion
        #region Method: void UpdateNeedsAttentionStatus()
        void UpdateNeedsAttentionStatus()
        // Assumes changes have been Harvested to the LocAlternative
        {
            if (null == Item)
                return;
            LocAlternate alt = new LocAlternate(
                m_textYourLanguage.Text,
                m_comboShortcutKey.Text,
                m_textToolTip.Text);

            // Do the test
            string sNeedsMessage = alt.NeedsAttention(Item);
            bool bNeedsAttention = !string.IsNullOrEmpty(sNeedsMessage);

            // Update the tree node
            if (null != Tree.SelectedNode && null != Tree.SelectedNode.Tag as LocItem)
                Tree.SelectedNode.ForeColor = (bNeedsAttention ? Color.Red : Color.Navy);

            // Update the message
            m_labelNeedsAttention.Text = sNeedsMessage;
        }
        #endregion

        // Command Handlers ------------------------------------------------------------------
        #region Cmd: cmdLoad
        private void cmdLoad(object sender, EventArgs e)
        {
            LocDB.Localize(this, null);     // Form Controls
            ShowHelp.ShowHelpTopic(this, "Localization.htm", Common.IsUnixOS(), false);

            // Populate the Filter Combo
            FilterCombo.Items.Add(c_ShowAll);
            FilterCombo.Items.Add(c_ThoseNeedingAttention);
            FilterCombo.Items.Add(c_TranslatorItems);
            FilterCombo.Items.Add(c_TranslatorItemsNeedingAttention);
            FilterCombo.Items.Add(c_AdvisorItems);
            FilterCombo.Items.Add(c_AdvisorItemsNeedingAttention);
            FilterCombo.Text = c_ShowAll;

            // Populate the Language Combo
            PopulateLanguageCombo();

            // Populate the Tree
            PopulateTree();

            // Populate the Shortcut Key Combo
            for (int i = 0; i < 26; i++)
                m_comboShortcutKey.Items.Add("Ctrl+" + ((char)((int)'A' + i)).ToString());

            // Initialize the form to the first item
            if (Tree.Nodes.Count > 0)
                Tree.SelectedNode = Tree.Nodes[0];
        }
        #endregion
        #region Cmd: cmdNextItem
        private void cmdNextItem(object sender, EventArgs e)
        {
            HarvestChanges();

            // Go to the next item if we can
            TreeNode nodeNext = Tree.SelectedNode.NextNode;
            if (null != nodeNext)
            {
                Tree.SelectedNode = nodeNext;
                return;
            }

            // Else, go to the next group if we can
            TreeNode nodeParent = Tree.SelectedNode.Parent;
            if (null != nodeParent)
            {
                nodeNext = nodeParent.NextNode;
                if (null != nodeNext)
                    Tree.SelectedNode = nodeNext.FirstNode;
            }
        }
        #endregion
        #region Cmd: cmdPreviousItem
        private void cmdPreviousItem(object sender, EventArgs e)
        {
            HarvestChanges();

            // Go to the previous item if we can
            TreeNode nodePrev = Tree.SelectedNode.PrevNode;
            if (null != nodePrev)
            {
                Tree.SelectedNode = nodePrev;
                return;
            }
            // Else, go to the previous group if we can
            TreeNode nodeParent = Tree.SelectedNode.Parent;
            if (null != nodeParent)
            {
                nodePrev = nodeParent.PrevNode;
                if (null != nodePrev)
                    Tree.SelectedNode = nodePrev.LastNode;
            }
        }
        #endregion
        #region Cmd: cmdTreeSelChanged
        private void cmdTreeSelChanged(object sender, TreeViewEventArgs e)
        {
            // Save any edits that have been done
            HarvestChanges();

            // Get the node's item, if any
            LocItem item = Tree.SelectedNode.Tag as LocItem;

            // If no item, then clear the controls
            if (null == item)
            {
                ClearItemControls();

                LocGroup g = Tree.SelectedNode.Tag as LocGroup;
                if (null != g)
                    Group = g;

                return;
            }

            // Get the group from this node's parent
            LocGroup group = Tree.SelectedNode.Parent.Tag as LocGroup;
            if (null == group)
                return;
            Group = group;

            // Otherwise, populate the controls
            Item = item;
        }
        #endregion
        #region Cmd: cmdFilterChanged
        private void cmdFilterChanged(object sender, EventArgs e)
        {
            HarvestChanges();
            PopulateTree();
        }
        #endregion
        #region Cmd: cmdLanguageChanged
        private void cmdLanguageChanged(object sender, EventArgs e)
        {
            HarvestChanges();
            string sName = LanguageCombo.Text;
            iLanguage = LocDB.DB.GetIndexForLanguage(sName);
            PopulateTree();
            m_labelYourLanguage.Text = sName;
        }
        #endregion
        #region Cmd: cmdClosing
        private void cmdClosing(object sender, FormClosingEventArgs e)
        {
            // Save any edits that have been done
            HarvestChanges();
            DialogResult result = MessageBox.Show(
                "Do you want to save your changes permanently?",
                 Common.GetProductName() + " Localizer",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Cancel)
                e.Cancel = true;

            if (result == DialogResult.Yes)
            {
                LocDB.DB.WriteXML();
                DialogResult = DialogResult.OK;
            }
            else
                DialogResult = DialogResult.Abort;
        }
        #endregion
        #region Cmd: cmdClose - Close button pressed
        private void cmdClose(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #region Cmd: cmdSave - write the LocDB to xml file
        private void cmdSave(object sender, EventArgs e)
        {
            HarvestChanges();
            LocDB.DB.WriteXML();
        }
        #endregion
        #region Cmd: cmdNewLanguage - add a new language to the LocDB
        private void cmdNewLanguage(object sender, EventArgs e)
        {
            DlgNewLanguage dlg = new DlgNewLanguage();
            if (DialogResult.Cancel == dlg.ShowDialog(this))
                return;

            // Create the new LocLanguage object
            LocLanguage language = LocDB.DB.AppendLanguage(dlg.Abbreviation, dlg.LanguageName);
            language.FontName = dlg.FontName;
            language.FontSize = dlg.FontSize;

            // Add the language to our combo box
            PopulateLanguageCombo();

            // Force an update of the interface to the new language
            FilterCombo.Text = c_ShowAll;
            LanguageCombo.Text = dlg.LanguageName;
            m_labelYourLanguage.Text = dlg.LanguageName;
            SetFont();
        }
        #endregion
        #region Cmd: cmdLanguageValueChanged - update the color of the tree control
        private void cmdLanguageValueChanged(object sender, EventArgs e)
        {
            UpdateNeedsAttentionStatus();
        }
        #endregion
        #region Cmd: cmdProperties - edit the Language properties (name, font, etc.)
        private void cmdProperties(object sender, EventArgs e)
        {
            // Get the current language object
            LocLanguage language = LocDB.DB.FindLanguageByName(m_comboLanguage.Text);
            if (null == language)
                return;

            DlgNewLanguage dlg = new DlgNewLanguage();
            dlg.SetAsPropertiesMode(language.ID, language.Name);
            if (DialogResult.Cancel == dlg.ShowDialog(this))
                return;

            language.FontName = dlg.FontName;
            language.FontSize = dlg.FontSize;

            PopulateLanguageCombo();
            LanguageCombo.Text = dlg.LanguageName;
            m_labelYourLanguage.Text = dlg.LanguageName;
            SetFont();
        }
        #endregion
        #region Cmd: cmdEditDescriptions
        private void cmdEditDescriptions(object sender, EventArgs e)
        {
            //HarvestChanges();

            //DlgEditDescriptions dlg = new DlgEditDescriptions();

            //dlg.GroupDescription = Group.Description;
            //dlg.ItemDescription = Item.Information;

            //dlg.ShowDialog(this);

            //Group.Description = dlg.GroupDescription;
            //Item.Information = dlg.ItemDescription;
            //PopulateItemControls();

        }
        #endregion
    }
}