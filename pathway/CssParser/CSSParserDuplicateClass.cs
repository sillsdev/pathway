// --------------------------------------------------------------------------------------------
// <copyright file="CSSParser.cs" from='2009' to='2009' company='SIL International'>
//      Copyright © 2009, SIL International. All Rights Reserved.   
//    
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright> 
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed: 
// 
// <remarks>
// Css Parser
// </remarks>
// --------------------------------------------------------------------------------------------

#region Using

using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using ANTLR_CSS;
using Antlr.Runtime.Tree;
using System.Windows.Forms;
using SIL.Tool;

#endregion

namespace SIL.PublishingSolution
{
    public class CssParserDuplicateClass
    {
        #region Properties
        public string ErrorText { get; private set; }
        public Dictionary<string, ArrayList> ErrorList = new Dictionary<string, ArrayList>(); // For Error Report
        #endregion Properties

        #region Private Variables
        private bool _isReCycle;
        // Page References
        private bool _isRefLocAdded;
        private bool _isPageNumLocAdded;
        // Page: first References
        private bool _isRefLocAddedF;
        private bool _isPageNumLocAddedF;
        // Page: left References
        private bool _isRefLocAddedL;
        private bool _isPageNumLocAddedL;
        // Page: right References
        private bool _isRefLocAddedR;
        private bool _isPageNumLocAddedR;
        string _filePath = string.Empty;
        string _mergepath = string.Empty;
        readonly TreeNode _nodeTemp = new TreeNode();
        readonly TreeNode _nodeFinal = new TreeNode("ROOT");
        readonly ArrayList _checkRuleNode = new ArrayList();
        readonly ArrayList _checkMediaNode = new ArrayList();
        readonly Dictionary<string, ArrayList> _pageRegionInfo = new Dictionary<string, ArrayList>();
        readonly Dictionary<string, ArrayList> _pagePropertyInfo = new Dictionary<string, ArrayList>();

        readonly ArrayList _checkPageName = new ArrayList();

        #endregion

        #region Constant Variables
        const string _pageSeperator = "~";
        #endregion

        #region Public Variable
        public struct Rule
        {
            public string ClassName;
            public string PseudoName;
            public bool IsPseudo;
            public int NodeCount;
            public bool IsClassContent;
            public bool HasProperty;
        }
        #endregion

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Parsing the CSS file when each file having @import.
        /// </summary>
        /// <param name="inputCSSPath">Accepts file path of the CSS file</param>
        /// <returns>returns TreeNode</returns>
        /// -------------------------------------------------------------------------------------------
        public TreeNode BuildTree(string inputCSSPath)
        {
            var emptyTree = new TreeNode();
            if (inputCSSPath.Length <= 0 || !File.Exists(inputCSSPath)) return emptyTree;

            _filePath = Path.GetDirectoryName(inputCSSPath);
            _mergepath = Common.PathCombine(Path.GetTempPath(), "_MergedCSS.css");
            try
            {
                string BaseCssFileWithPath = inputCSSPath;
                ArrayList arrayCSSFile = Common.GetCSSFileNames(inputCSSPath, BaseCssFileWithPath);
                arrayCSSFile.Add(BaseCssFileWithPath);
                GetErrorReport(inputCSSPath);
                string file = Common.MakeSingleCSS(inputCSSPath, "_MergedCSS.css");
                var fileSize = new FileInfo(_mergepath);
                if (fileSize.Length > 0)
                {
                    string tempCSS = _mergepath;
                    ParseCSS(tempCSS);
                    //File.Delete(tempCSS);
                    return _nodeFinal;
                }
                return emptyTree;
            }
            catch
            {
                return emptyTree;
            }
        }


        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// This function parsing the CSS file and creates a TreeNode nodeTemp.
        /// </summary>
        /// <param name="path">Its gets the file path of the CSS File</param>
        /// -------------------------------------------------------------------------------------------
        private void ParseCSS(string path)
        {
            var ctp = new CssTreeParser();
            try
            {
                ctp.Parse(path);
                ErrorText = ctp.ErrorText();
            }
            catch (Exception)
            {
                ErrorText = ctp.ErrorText();
                throw;
            }
            CommonTree r = ctp.Root;
            _nodeTemp.Nodes.Clear();

            if (r.Text != "nil" && r.Text != null)
            {
                _nodeTemp.Text = "nil";
                AddSubTree(_nodeTemp, r, ctp);
            }
            else
            {
                string rootNode = r.Text ?? "nil";
                _nodeTemp.Text = rootNode;
                foreach (CommonTree child in ctp.Children(r))
                {
                    AddSubTree(_nodeTemp, child, ctp);
                }
            }

            ////if (r.Text != "nil")
            ////{
            ////    _nodeTemp.Nodes.Add("nil");
            ////    AddSubTree(_nodeTemp, r, ctp);
            ////}
            ////else
            ////{
            //if (r.Text != "nil")
            //{
            //    _nodeTemp.Text = "nil";
            //}
            //_nodeTemp.Text = r.Text;
            //foreach (CommonTree child in ctp.Children(r))
            //{
            //    AddSubTree(_nodeTemp, child, ctp);
            //}
            ////}

            // To validate the nodes in nodeTemp has copied to nodeFine
            if (_isReCycle == false)
            {
                _nodeFinal.Nodes.Clear();
                MakeTreeNode(_nodeTemp, _nodeFinal, false);
                // To traverse the node second time.
                if (_isReCycle)
                {
                    MakeTreeNode(_nodeFinal, _nodeFinal, true);
                }
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// This method adds each node,subnode,child in Treenode named as nodeTemp.
        /// </summary>
        /// <param name="n">Its gets the node element from the TreeTemp</param>
        /// <param name="t">Its gets the parent of the current tree</param>
        /// <param name="ctp">input CSSTreeParser</param>
        /// -------------------------------------------------------------------------------------------
        private static void AddSubTree(TreeNode n, CommonTree t, CssTreeParser ctp)
        {
            TreeNode nodeTemp = n.Nodes.Add(t.Text);
            foreach (CommonTree child in ctp.Children(t))
                AddSubTree(nodeTemp, child, ctp);
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// To avoiding duplication of nodes based on Inheritance.
        /// </summary>    
        /// <param name="fromNode">Source TreeNode</param>
        /// <param name="toNode">Destination TreeNode</param>
        /// <param name="status">By default it is false for first time and true for second time</param>
        /// -------------------------------------------------------------------------------------------
        private void MakeTreeNode(TreeNode fromNode, TreeNode toNode, bool status)
        {
            // Filter the Duplicate Classes from NodeTemp Treenode
            foreach (TreeNode node in fromNode.Nodes)
            {
                if (node.Text == "RULE")
                {
                    bool commaRule = false;
                    foreach (TreeNode chkNode in node.Nodes)
                    {
                        if (chkNode.Text == ",")
                        {
                            ParseCommaRule(node, toNode);
                            commaRule = true;
                            break;
                        }
                    }
                    if (!commaRule)
                    {
                        ParseRule(node, toNode);
                    }
                }
                else if (node.Text == "PAGE")
                {
                    if (status == false)
                    {
                        ParsePage(node, toNode);
                    }
                }
                else if (node.Text == "MEDIA")
                {
                    if (status == false)
                    {
                        if (node.FirstNode.Text.ToUpper() == "PRINT")
                        {
                            ParseMedia(node, toNode);
                        }
                        else
                        {
                            toNode.Nodes.Add((TreeNode)node.Clone());
                        }
                    }
                }
            }

            if (status == false)
            {
                _isReCycle = true;
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// This method inherits the page properties which having same classname in CSS file and added to the TreeNode.
        /// </summary>
        /// <param name="node">nodes from nodeTemp(treenode)</param>
        /// <param name="nodeFine">nodes added to nodeFine(treenode)</param>
        /// -------------------------------------------------------------------------------------------
        private void ParseMedia(TreeNode node, TreeNode nodeFine)
        {
            string mediaType = node.FirstNode.Text;
            int cnt = _checkMediaNode.Count;
            bool isAttribPropertyWritten = false;
            if (cnt == 0)
            {
                foreach (TreeNode mainNode in node.Nodes)
                {
                    string attribName = string.Empty;
                    string attribValue = string.Empty;
                    if (mainNode.Text == "RULE")
                    {
                        foreach (TreeNode regionNode in mainNode.Nodes)
                        {
                            if (regionNode.Text == "ANY")
                            {
                                if (regionNode.FirstNode.Text == "ATTRIB")
                                {
                                    attribName = regionNode.FirstNode.FirstNode.Text;
                                    if (regionNode.FirstNode.Nodes.Count > 1)
                                    {
                                        attribValue = regionNode.FirstNode.LastNode.Text.Replace("\"", "");
                                        attribValue = attribValue.Replace("\'", "");
                                    }
                                }
                                _checkMediaNode.Add(mediaType + attribName + attribValue);
                            }
                            else if (regionNode.Text == "PROPERTY")
                            {
                                _checkMediaNode.Add(mediaType + attribName + attribValue + regionNode.FirstNode.Text);
                            }
                        }
                    }
                }
                nodeFine.Nodes.Add((TreeNode)node.Clone());
            }
            else
            {
                mediaType = node.FirstNode.Text;
                foreach (TreeNode mainNode in node.Nodes)
                {
                    string attribName = string.Empty;
                    string attribValue = string.Empty;
                    if (mainNode.Text == "RULE")
                    {
                        foreach (TreeNode regionNode in mainNode.Nodes)
                        {
                            if (regionNode.Text == "ANY")
                            {
                                if (regionNode.FirstNode.Text == "ATTRIB")
                                {
                                    attribName = regionNode.FirstNode.FirstNode.Text;
                                    if (regionNode.FirstNode.Nodes.Count > 1)
                                    {
                                        attribValue = regionNode.FirstNode.LastNode.Text.Replace("\"", "");
                                    }
                                }
                                if (!_checkMediaNode.Contains(mediaType + attribName + attribValue))
                                {
                                    _checkMediaNode.Add(mediaType + attribName + attribValue);
                                    nodeFine.LastNode.Nodes.Add(mainNode);
                                    isAttribPropertyWritten = true;
                                }
                                else
                                {
                                    foreach (TreeNode RTNode in mainNode.Nodes)
                                    {
                                        if (RTNode.Text == "PROPERTY")
                                        {
                                            InsertMediaProperty(mediaType + attribName + attribValue, mediaType + attribName + attribValue + RTNode.FirstNode.Text, RTNode);
                                        }
                                    }
                                }
                            }
                            else if (regionNode.Text == "PROPERTY" && isAttribPropertyWritten == false)
                            {
                                if (!_checkMediaNode.Contains(mediaType + attribName + attribValue + regionNode.FirstNode.Text))
                                {
                                    _checkMediaNode.Add(mediaType + attribName + attribValue + regionNode.FirstNode.Text);
                                    nodeFine.LastNode.Nodes.Add((TreeNode)regionNode.Clone());
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// To check the node has the search string, if yes remove node from the Tree
        /// </summary>
        /// <param name="regNode">Region node to be search</param>
        /// <param name="_isRefLocAdded">bool value of Reference Node</param>
        /// <param name="_isPageNumLocAdded">bool value of PageNumber Node</param>
        private static void NodeCheckInRegion(TreeNode regNode, ref bool _isRefLocAdded, ref bool _isPageNumLocAdded)
        {
            foreach (TreeNode c in regNode.Nodes)
            {
                if (c.Text == "PROPERTY")
                {
                    if (c.Nodes.Count > 20 && (c.FirstNode.Text == "content"))
                    {
                        if (_isRefLocAdded)
                            c.Remove();
                        _isRefLocAdded = true;
                    }
                    else if (c.Nodes.Count > 5 && (c.Nodes[3].Text == "page"))
                    {
                        if (_isPageNumLocAdded)
                            c.Remove();
                        _isPageNumLocAdded = true;
                    }
                }
            }
        }


        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// This method inherits the properties which having same classname in CSS file and added to the TreeNode.
        /// </summary>
        /// <param name="node">nodes from nodeTemp(treenode)</param>
        /// <param name="nodeFine">nodes added to nodeFine(treenode)</param>
        /// -------------------------------------------------------------------------------------------
        /// 
        private void ParsePage(TreeNode node, TreeNode nodeFine)
        {
            string pageName = GetPageName(node);
            if (!_checkPageName.Contains(pageName))
            {
                GetPageContains(node);
                nodeFine.Nodes.Add((TreeNode)node.Clone());
                _checkPageName.Add(pageName);
            }
            else
            {
                string duppageName = GetPageName(node);
                foreach (TreeNode sNode in _nodeFinal.Nodes)
                {
                    if(sNode.Text == "PAGE")
                    {
                        pageName = GetPageName(sNode);
                        if(pageName == duppageName)
                        {
                            foreach (TreeNode dupNode in node.Nodes)
                            {
                                ArrayList regtemp;
                                ArrayList temp;
                                if (dupNode.Text == "REGION" && _pagePropertyInfo.ContainsKey(pageName + "." + dupNode.FirstNode.Text))
                                {
                                    regtemp = _pagePropertyInfo[pageName + "." + dupNode.FirstNode.Text];
                                    foreach (TreeNode prpNode in dupNode.Nodes)
                                    {
                                        if (prpNode.Text == "PROPERTY" && !regtemp.Contains(prpNode.FirstNode.Text))
                                        {
                                            foreach (TreeNode pNode in sNode.Nodes)
                                            {
                                                if (pNode.Text == "REGION" && pNode.FirstNode.Text == dupNode.FirstNode.Text)
                                                {
                                                    foreach (TreeNode mNode in pNode.Nodes)
                                                    {
                                                        if (mNode.Text == "PROPERTY" && prpNode.Text == "PROPERTY" && mNode.FirstNode.Text == prpNode.FirstNode.Text)
                                                        {
                                                            pNode.Nodes.Remove(mNode);
                                                        }
                                                    }
                                                    pNode.Nodes.Add(prpNode);
                                                }
                                            }                                            
                                            ArrayList tempList = GetPropertyNames(dupNode.Parent);
                                            _pagePropertyInfo[pageName + "." + dupNode.FirstNode.Text] = tempList;
                                        }
                                        else
                                        {
                                            foreach (TreeNode pNode in sNode.Nodes)
                                            {
                                                if(pNode.Text == "REGION" && pNode.FirstNode.Text == dupNode.FirstNode.Text)
                                                {
                                                    foreach (TreeNode propNode in pNode.Nodes)
                                                    {
                                                        if (prpNode.Text == "PROPERTY" && propNode.Text == "PROPERTY" && propNode.FirstNode.Text == prpNode.FirstNode.Text)
                                                        {
                                                            pNode.Nodes.Remove(propNode);
                                                            pNode.Nodes.Add(prpNode);
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                        
                                        }
                                    }
                                }
                                else if (dupNode.Text == "PROPERTY" && _pagePropertyInfo.ContainsKey(pageName))
                                {
                                    temp = _pagePropertyInfo[pageName];

                                    if (!temp.Contains(dupNode.FirstNode.Text))
                                    {
                                        sNode.Nodes.Add(dupNode);
                                        ArrayList tempList = GetPropertyNames(dupNode.Parent);
                                        _pagePropertyInfo[pageName] = tempList;
                                    }
                                    else
                                    {
                                        foreach (TreeNode pNode in sNode.Nodes)
                                        {
                                            if (pNode.Text == "PROPERTY" && pNode.FirstNode.Text == dupNode.FirstNode.Text)
                                            {
                                                sNode.Nodes.Remove(pNode);
                                                sNode.Nodes.Add(dupNode);
                                            }
                                        }
                                    }
                                }
                                else if (dupNode.Text == "REGION")
                                {
                                    ArrayList t = GetPropertyNames(dupNode);
                                    _pagePropertyInfo[pageName + "." + dupNode.FirstNode.Text] = t;
                                    sNode.Nodes.Add(dupNode);
                                }
                                else if (dupNode.Text == "PROPERTY")
                                {
                                    ArrayList t = GetPropertyNames(dupNode);
                                    _pagePropertyInfo[pageName] = t;
                                    sNode.Nodes.Add(dupNode);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ParsePage_OLD(TreeNode node, TreeNode nodeFine)
        {
            string pageName = GetPageName(node);
            if (!_pageRegionInfo.ContainsKey(pageName))
            {
                GetPageContains(node);
                ArrayList tempPropertyList = GetPageInfo(pageName, _pagePropertyInfo);
                _pageRegionInfo.Add(pageName, tempPropertyList);
                nodeFine.Nodes.Add((TreeNode)node.Clone());
            }
            else
            {
                ArrayList tempPropertyList = GetPageInfo(pageName, _pagePropertyInfo);
                ArrayList tempRegionList = GetPageInfo(pageName, _pageRegionInfo);
                foreach (TreeNode propNode in node.Nodes)
                {
                    string propertyName;
                    if (propNode.Text == "PROPERTY")
                    {
                        if (tempPropertyList.Contains(propNode.FirstNode.Text))
                        {
                            propertyName = propNode.FirstNode.Text;
                            InsertPageProperty(pageName, propertyName, propNode, null);
                            InsertNewPageProperty(nodeFine, pageName, tempPropertyList, propNode, _pagePropertyInfo);
                        }
                        else
                        {
                            InsertNewPageProperty(nodeFine, pageName, tempPropertyList, propNode, _pagePropertyInfo);
                        }
                    }
                    else if (propNode.Text == "REGION")
                    {
                        if (tempRegionList.Contains(propNode.FirstNode.Text))
                        {
                            foreach (TreeNode regionNode in propNode.Nodes)
                            {
                                if (regionNode.Text == "PROPERTY")
                                {
                                    if (_isRefLocAdded && (regionNode.FirstNode.NextNode.Text != "string"))
                                    {
                                        propertyName = regionNode.FirstNode.Text;
                                        InsertPageProperty(regionNode.Parent.FirstNode.Text, propertyName,
                                                           regionNode, pageName);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (pageName.IndexOf("first") > 0)
                            {
                                NodeCheckInRegion(propNode, ref _isRefLocAddedF, ref _isPageNumLocAddedF);
                            }
                            else if (pageName.IndexOf("left") > 0)
                            {
                                NodeCheckInRegion(propNode, ref _isRefLocAddedL, ref _isPageNumLocAddedL);
                                _isRefLocAdded = true;
                                _isPageNumLocAdded = true;
                            }
                            else if (pageName.IndexOf("right") > 0)
                            {
                                NodeCheckInRegion(propNode, ref _isRefLocAddedR, ref _isPageNumLocAddedR);
                                _isRefLocAdded = true;
                                _isPageNumLocAdded = true;
                            }
                            else
                            {
                                NodeCheckInRegion(propNode, ref _isRefLocAdded, ref _isPageNumLocAdded);
                            }
                            InsertNewPageProperty(nodeFine, pageName, tempRegionList, propNode, _pageRegionInfo);
                        }
                    }
                }
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// To copy the arraylist value from the pageIngfo[pageName]
        /// </summary>
        /// <param name="pageName">current Page name</param>
        /// <param name="pageInfo">Collection of Page Property</param>
        /// <returns>returns ArrayList</returns>
        /// -------------------------------------------------------------------------------------------
        private static ArrayList GetPageInfo(string pageName, IDictionary<string, ArrayList> pageInfo)
        {
            var tempList = new ArrayList();
            if (pageInfo.ContainsKey(pageName))
            {
                tempList.AddRange(pageInfo[pageName]);
            }
            return tempList;
        }

        private ArrayList GetPropertyNames(TreeNode node)
        {
            ArrayList temp = new ArrayList();
            foreach (TreeNode propNode in node.Nodes)
            {
                if (propNode.Text == "PROPERTY")
                {
                    temp.Add(propNode.FirstNode.Text);
                }
            }
            return temp;
        }

        private void GetPageContains_OLD(TreeNode node)
        {
            string pageName = "PAGE";
            ArrayList regionExists;
            foreach (TreeNode item in node.Nodes)
            {
                switch (item.Text)
                {
                    case "PSEUDO":
                        pageName = pageName + _pageSeperator + item.FirstNode.Text;
                        break;
                    case "REGION":
                        regionExists = new ArrayList();
                        if (_pageRegionInfo.ContainsKey(pageName))
                        {
                            regionExists.AddRange(_pageRegionInfo[pageName]);
                        }
                        regionExists.Add(item.FirstNode.Text);
                        if (pageName.IndexOf("first") > 0)
                        {
                            NodeCheckInRegion(item, ref _isRefLocAddedF, ref _isPageNumLocAddedF);
                        }
                        else if (pageName.IndexOf("left") > 0)
                        {
                            if (_isRefLocAdded)
                                _isRefLocAddedL = true;
                            if (_isPageNumLocAdded)
                                _isPageNumLocAddedL = true;
                            NodeCheckInRegion(item, ref _isRefLocAddedL, ref _isPageNumLocAddedL);
                            _isRefLocAdded = true;
                            _isPageNumLocAdded = true;
                        }
                        else if (pageName.IndexOf("right") > 0)
                        {
                            if (_isRefLocAdded)
                                _isRefLocAddedR = true;
                            if (_isPageNumLocAdded)
                                _isPageNumLocAddedR = true;
                            NodeCheckInRegion(item, ref _isRefLocAddedR, ref _isPageNumLocAddedR);
                            _isRefLocAdded = true;
                            _isPageNumLocAdded = true;
                        }
                        else
                        {
                            NodeCheckInRegion(item, ref _isRefLocAdded, ref _isPageNumLocAdded);
                        }
                        break;
                    case "PROPERTY":
                        regionExists = new ArrayList();
                        if (_pagePropertyInfo.ContainsKey(pageName))
                        {
                            regionExists.AddRange(_pagePropertyInfo[pageName]);
                        }
                        regionExists.Add(item.FirstNode.Text);
                        _pagePropertyInfo[pageName] = regionExists;
                        break;
                    default:
                        break;
                }
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// To collect the page details from the Page node and store in Arraylist
        /// </summary>
        /// <param name="node">current Page node</param>
        /// -------------------------------------------------------------------------------------------
        private void GetPageContains(TreeNode node)
        {
            string pageName = "PAGE";
            ArrayList regionExists;
            foreach (TreeNode item in node.Nodes)
            {
                switch (item.Text)
                {
                    case "PSEUDO":
                        pageName = pageName + _pageSeperator + item.FirstNode.Text;
                        break;
                    case "REGION":
                        regionExists = new ArrayList();
                        foreach (TreeNode PropNode in item.Nodes)
                        {
                            if(PropNode.Text == "PROPERTY")
                            {
                                if (_pagePropertyInfo.ContainsKey(pageName + "." + item.FirstNode.Text))
                                {
                                    regionExists.AddRange(_pagePropertyInfo[pageName + "." + item.FirstNode.Text]);
                                }
                                regionExists.Add(PropNode.FirstNode.Text);
                            }
                        }
                        _pagePropertyInfo[pageName + "." + item.FirstNode.Text] = regionExists;                       
                        break;
                    case "PROPERTY":
                        regionExists = new ArrayList();
                        if (_pagePropertyInfo.ContainsKey(pageName))
                        {
                            regionExists.AddRange(_pagePropertyInfo[pageName]);
                        }
                        regionExists.Add(item.FirstNode.Text);
                        _pagePropertyInfo[pageName] = regionExists;
                        break;
                    default:
                        break;
                }
            }
        }
        /// ------------------------------------------------------------------------------------------- 
        /// <summary>
        /// To insert new page property in the Page node
        /// </summary>
        /// <param name="nodeFine">nodeFine(Main node)</param>
        /// <param name="pageName">Pagename of the node</param>
        /// <param name="tempList">ArrayList which contains existing property</param>
        /// <param name="propNode">Property Node</param>
        /// <param name="pageProperty">current Page Node</param>
        /// -------------------------------------------------------------------------------------------
        private static void InsertNewPageProperty(TreeNode nodeFine, string pageName, ArrayList tempList, TreeNode propNode, IDictionary<string, ArrayList> pageProperty)
        {
            tempList.Add(propNode.FirstNode.Text);
            pageProperty[pageName] = tempList;
            foreach (TreeNode item in nodeFine.Nodes)
            {
                if (item.Text == "PAGE")
                {
                    string currPage = GetPageName(item);
                    if (currPage == pageName)
                    {
                        item.Nodes.Add((TreeNode)propNode.Clone());
                    }
                }
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// To get the page name with Pseudo value like page, page~first, page~left, page~right
        /// </summary>
        /// <param name="node">current Page node</param>
        /// <returns>returns pagename</returns>
        /// ------------------------------------------------------------------------------------------- 
        private static string GetPageName(TreeNode node)
        {
            string pageName = "PAGE";
            if (node.FirstNode != null && node.FirstNode.Text == "PSEUDO")
            {
                pageName = pageName + _pageSeperator + node.FirstNode.FirstNode.Text;
            }
            return pageName;
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// This method inherits the properties which having same classname in CSS file and added to the TreeNode.
        /// </summary>
        /// <param name="node">nodes from nodeTemp(treenode)</param>
        /// <param name="nodeFine">nodes added to nodeFine(treenode)</param>
        /// -------------------------------------------------------------------------------------------
        private void ParseRule(TreeNode node, TreeNode nodeFine)
        {
            foreach (TreeNode childNode in node.Nodes)
            {
                var newRuleNode = new Rule();
                GetRuleContains(node, ref newRuleNode);
                if (ValidateFirstNode(childNode))
                {
                    if (_isReCycle == false)
                    {
                        if (newRuleNode.HasProperty)
                        {
                            if (_checkRuleNode.Contains(newRuleNode.ClassName))
                            {
                                InsertNewRuleProperty(node, newRuleNode.ClassName, 'u', true);
                            }
                            else
                            {
                                _checkRuleNode.Add(newRuleNode.ClassName);
                                nodeFine.Nodes.Add((TreeNode)node.Clone());
                                break;
                            }
                        }
                        //else
                        //{
                        //    if (!_checkRuleNode.Contains(newRuleNode.ClassName))
                        //    {
                        //        _checkRuleNode.Add(newRuleNode.ClassName);
                        //        nodeFine.Nodes.Add((TreeNode)node.Clone());
                        //    }
                        //}
                    }
                    else
                    {
                        string[] parentClass = newRuleNode.ClassName.Trim().Split('.');
                        if (parentClass.Length > 2 && _checkRuleNode.Contains("." + parentClass[1]))
                        {
                            InsertNewRuleProperty(node, "." + parentClass[1], 'd', false);
                        }
                        else if (parentClass.Length > 2 && _checkRuleNode.Contains("." + parentClass[parentClass.Length - 1]))
                        {
                            if (parentClass[parentClass.Length - 1].IndexOf("=") > 0 && parentClass[parentClass.Length - 1].IndexOf(":") > 0)
                            {
                                parentClass[parentClass.Length - 1] = parentClass[parentClass.Length - 1].Substring(0, parentClass[parentClass.Length - 1].IndexOf("="));
                                InsertNewRuleProperty(node, "." + parentClass[parentClass.Length - 1], 'd', false);
                            }
                            else if (parentClass[parentClass.Length - 1].IndexOf(":") > 0)
                            {
                                parentClass[parentClass.Length - 1] = parentClass[parentClass.Length - 1].Substring(0, parentClass[parentClass.Length - 1].IndexOf(":"));
                                InsertNewRuleProperty(node, "." + parentClass[parentClass.Length - 1], 'd', false);
                            }
                            //InsertNewRuleProperty(node, "." + parentClass[parentClass.Length - 1], 'd', false);
                        }
                        else if (parentClass.Length >= 2)
                        {
                            //if (parentClass[1].IndexOf("=") > 0)
                            //{
                            //    parentClass[1] = parentClass[1].Substring(0, parentClass[1].IndexOf("="));
                            //    InsertNewRuleProperty(node, "." + parentClass[1], 'd', false);
                            //}
                            //else if (parentClass[1].IndexOf(":") > 0)
                            if (parentClass[1].IndexOf(":") > 0)
                            {
                                parentClass[1] = parentClass[1].Substring(0, parentClass[1].IndexOf(":"));
                                InsertNewRuleProperty(node, "." + parentClass[1], 'd', false);
                            }
                            else if (parentClass[parentClass.Length - 1].IndexOf("=") > 0 && parentClass[parentClass.Length - 1].IndexOf(":") > 0)
                            {
                                parentClass[parentClass.Length - 1] = parentClass[parentClass.Length - 1].Substring(0, parentClass[parentClass.Length - 1].IndexOf("="));
                                InsertNewRuleProperty(node, "." + parentClass[parentClass.Length - 1], 'd', false);
                            }
                            else if (parentClass[parentClass.Length - 1].IndexOf(":") > 0)
                            {
                                parentClass[parentClass.Length - 1] = parentClass[parentClass.Length - 1].Substring(0, parentClass[parentClass.Length - 1].IndexOf(":"));
                                InsertNewRuleProperty(node, "." + parentClass[parentClass.Length - 1], 'd', false);
                                if (parentClass[1].IndexOf('>') > 0)
                                {
                                    parentClass[1] = parentClass[1].Replace(">", "");
                                    InsertNewRuleProperty(node, "." + parentClass[1], 'd', false);
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }

        private static ArrayList GetPropertyList(TreeNode node)
        {
            var propL = new ArrayList();
            foreach (TreeNode PropNode in node.Nodes)
            {
                if (PropNode.Text == "PROPERTY")
                {
                    propL.Add(PropNode.FirstNode.Text);
                }
            }
            return propL;
        }

        private void InsertNewRuleProperty(TreeNode repNode, string className, char dir, bool isSameClass)
        {
            var repRuleNode = new Rule();
            GetRuleContains(repNode, ref repRuleNode);
            ArrayList repProperty;
            foreach (TreeNode RuleNode in _nodeFinal.Nodes)
            {
                var newRuleNode = new Rule();
                GetRuleContains(RuleNode, ref newRuleNode);
                if (newRuleNode.ClassName == className)
                {
                    if (dir == 'u')
                    {
                        repProperty = GetPropertyList(RuleNode);
                        foreach (TreeNode childNode in repNode.Nodes)
                        {
                            //if (childNode.Text == "PROPERTY")
                            //{
                            //    if (isSameClass || !repProperty.Contains(childNode.FirstNode.Text))
                            //    {
                            //        RuleNode.Nodes.Add(childNode);
                            //    }
                            //}
                            if (childNode.Text == "PROPERTY" && isSameClass)
                            {
                                if (!repProperty.Contains(childNode.FirstNode.Text))
                                {
                                    RuleNode.Nodes.Add(childNode);
                                }
                                else
                                {
                                    ReplaceRuleNode(childNode);
                                }
                            }
                        }
                    }
                    else
                    {
                        repProperty = GetPropertyList(repNode);
                        foreach (TreeNode childNode in RuleNode.Nodes)
                        {
                            if (childNode.Text == "PROPERTY")
                            {
                                if (isSameClass || !repProperty.Contains(childNode.FirstNode.Text))
                                {
                                    repNode.Nodes.Add(childNode);
                                }
                            }
                        }
                    }
                }
            }
        }

        private string GetRuleClassname(TreeNode node)
        {
            string className = string.Empty;
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Text == "CLASS")
                {
                    childNode.FirstNode.Text = childNode.FirstNode.Text.Replace("_", "").Replace("-", "");
                    className += "." + childNode.FirstNode.Text;
                    if (childNode.Nodes.Count > 1)
                    {
                        for (int i = 0; i < childNode.Nodes.Count; i++)
                        {
                            if (childNode.Nodes[i].Text == "ATTRIB")
                            {
                                className += "=" +
                                             childNode.Nodes[i].LastNode.Text.Replace("'", "").Replace(
                                                 "\"", "");
                            }
                            else if (childNode.Nodes[i].Text == "HASVALUE")
                            {
                                className += "~" +
                                             childNode.Nodes[i].LastNode.Text.Replace("'", "").Replace(
                                                 "\"", "");
                            }
                        }
                    }
                }
                else if (childNode.Text == "TAG")
                {
                    className += " " + childNode.FirstNode.Text;
                    if (childNode.Nodes.Count > 1)
                    {
                        for (int i = 0; i < childNode.Nodes.Count; i++)
                        {
                            if (childNode.Nodes[i].Text == "ATTRIB")
                            {
                                className += "=" +
                                             childNode.Nodes[i].LastNode.Text.Replace("'", "").Replace(
                                                 "\"", "");
                            }
                            else if (childNode.Nodes[i].Text == "HASVALUE")
                            {
                                className += "~" +
                                             childNode.Nodes[i].LastNode.Text.Replace("'", "").Replace(
                                                 "\"", "");
                            }
                        }
                    }
                }
                else if (childNode.Text == "ANY")
                {
                    className += "*" + childNode.FirstNode.Text;
                }
                else if (childNode.Text == "PARENTOF")
                {
                    className += ">";
                }
                else if (childNode.Text == "PRECEDES")
                {
                    className += "+";
                }
                else if (childNode.Text == "PSEUDO")
                {
                    className += ":" + childNode.FirstNode.Text;
                }
            }
            return className;
        }

        private void ReplaceRuleNode(TreeNode newNode)
        {
            foreach (TreeNode snode in _nodeFinal.Nodes)
            {
                if (snode.Text == "RULE" && GetRuleClassname(snode) == GetRuleClassname(newNode.Parent))
                {
                    foreach (TreeNode PNode in snode.Nodes)
                    {
                        if(PNode.Text == "PROPERTY" && PNode.FirstNode.Text == newNode.FirstNode.Text)
                        {
                            PNode.Remove();
                            snode.Nodes.Add(newNode);
                        }
                    }
                    
                }
            }
        }

        private static bool ValidateFirstNode(TreeNode childNode)
        {
            return childNode.Text == "CLASS" || childNode.Text == "TAG" || childNode.Text == "ANY";
        }


        /// -------------------------------------------------------------------------------------------
        /// <summary>
        ///  TO handle Multi-line synatx
        ///  .p1:before,
        ///  .p2:before,
        ///  .p3:before {content: "text"};
        /// </summary>
        /// <param name="node">Current TreeNode</param>
        /// <param name="nodeFine">Main TreeNode</param>
        /// -------------------------------------------------------------------------------------------
        private void ParseCommaRule(TreeNode node, TreeNode nodeFine)
        {
            var propertyNode = new TreeNode();
            var ruleNode = new TreeNode();
            foreach (TreeNode propNode in node.Nodes)
            {
                if (propNode.Text == "PROPERTY")
                {
                    propertyNode.Nodes.Add((TreeNode)propNode.Clone());
                }
            }
            foreach (TreeNode subNode in node.Nodes)
            {
                if (subNode.Text == ",")
                {
                    foreach (TreeNode item in propertyNode.Nodes)
                    {
                        ruleNode.Nodes.Add((TreeNode)item.Clone());
                    }
                    ruleNode.Text = "RULE";
                    var newRule = new Rule();
                    GetRuleContains(ruleNode, ref newRule);
                    ParseRule((TreeNode)ruleNode.Clone(), nodeFine);
                    ruleNode.Nodes.Clear();
                }
                else
                {
                    ruleNode.Nodes.Add((TreeNode)subNode.Clone());
                }
            }
            //ruleNode.Text = "RULE";
            //var newRule1 = new Rule();
            //GetRuleContains(ruleNode, ref newRule1);
            //_checkRuleNode.Add(newRule1.ClassName);
            ParseRule((TreeNode)ruleNode.Clone(), nodeFine);
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// To insert the new page property which are not existing in the current node.
        /// </summary>
        /// <param name="parentName">Class name of the current treeenode</param>
        /// <param name="propertyName">Property name of the current treenode</param>
        /// <param name="propNode">Treenode of the current node</param>
        /// -------------------------------------------------------------------------------------------
        private void InsertMediaProperty(string parentName, string propertyName, TreeNode propNode)
        {
            foreach (TreeNode nodeFinalNode in _nodeFinal.Nodes)
            {
                if (nodeFinalNode.Text == "MEDIA")
                {
                    string mediaType = nodeFinalNode.FirstNode.Text;
                    foreach (TreeNode mainNode in nodeFinalNode.Nodes)
                    {
                        string attribName = string.Empty;
                        string attribValue = string.Empty;
                        if (mainNode.Text == "RULE")
                        {
                            foreach (TreeNode regionNode in mainNode.Nodes)
                            {
                                if (regionNode.Text == "ANY" && regionNode.FirstNode.Text == "ATTRIB")
                                {
                                    attribName = regionNode.FirstNode.FirstNode.Text;
                                    if (regionNode.FirstNode.Nodes.Count > 1)
                                    {
                                        attribValue = regionNode.FirstNode.LastNode.Text.Replace("\"", "");
                                        attribValue = attribValue.Replace("\'", "");
                                    }
                                }
                                else if (regionNode.Text == "PROPERTY")
                                {
                                    if (parentName == mediaType + attribName + attribValue && !_checkMediaNode.Contains(propertyName))
                                    {
                                        _checkMediaNode.Add(propertyName);
                                        mainNode.Nodes.Add(propNode);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// To insert the new page property which are not existing in the current node.
        /// </summary>
        /// <param name="parentName">Class name of the current treeenode</param>
        /// <param name="propertyName">Property name of the current treenode</param>
        /// <param name="propNode">Treenode of the current node</param>
        /// <param name="regionPage">Treenode of the Page</param>
        /// -------------------------------------------------------------------------------------------
        private void InsertPageProperty(string parentName, string propertyName, TreeNode propNode, string regionPage)
        {
            bool propertyFound = false;
            bool regionPropertyFound = false;
            foreach (TreeNode subNode in _nodeFinal.Nodes)
            {
                if (subNode.Text == "PAGE")
                {
                    string pageName = GetPageName(subNode);
                    foreach (TreeNode item in subNode.Nodes)
                    {
                        if (item.Text == "REGION" && item.FirstNode.Text == parentName && regionPage == pageName)
                        {
                            AddProperty(propertyName, propNode, item, regionPropertyFound);
                            regionPropertyFound = true;
                            break;
                        }
                        if ((!regionPropertyFound) && item.Text == "PROPERTY" && pageName == parentName)
                        {
                            if (item.Nodes.Count > 0 && item.Nodes[0].Text == propertyName)
                            {
                                propertyFound = true;
                                break;
                            }
                        }
                    }
                    if ((!propertyFound) && (!regionPropertyFound))
                    {
                        foreach (TreeNode item in _nodeFinal.Nodes)
                        {
                            if (item.Text == "PAGE")
                            {
                                string currPage = GetPageName(item);
                                if (currPage == parentName)
                                {
                                    item.Nodes.Add((TreeNode)propNode.Clone());
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void AddProperty(string propertyName, TreeNode mainNode, TreeNode node, bool propertyFound)
        {
            foreach (TreeNode subNode in node.Nodes)
            {
                propertyFound = false;
                if (subNode.Text == "PROPERTY" && subNode.Nodes[0].Text == propertyName)
                {
                    propertyFound = true;
                }
                if (propertyFound)
                    break;
            }
            if (propertyFound == false)
            {
                node.Nodes.Add(mainNode);
            }
            return;
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Unicode Conversion 
        /// </summary>
        /// <param name="parameter">input String</param>
        /// <returns>Unicode Character</returns>
        /// -------------------------------------------------------------------------------------------
        public string UnicodeConversion(string parameter)
        {
            int count = 0;
            string result = string.Empty;
            try
            {
            if (parameter.Length > 0)
            {
                if (!(parameter[0] == '\"' || parameter[0] == '\''))
                {
                    parameter = "'" + parameter + "'";
                }
                int strlen = parameter.Length;
                char quoteOpen = ' ';
                while (count < strlen)
                {
                    // Handling Single / Double Quotes
                    char c1 = parameter[count];
                    Console.WriteLine(c1);
                    if (parameter[count] == '\"' || parameter[count] == '\'')
                    {
                        if (parameter[count] == quoteOpen)
                        {
                            quoteOpen = ' ';
                            count++;
                            continue;
                        }
                        if (quoteOpen == ' ')
                        {
                            quoteOpen = parameter[count];
                            count++;
                            continue;
                        }
                    }

                    if (parameter[count] == '\\')
                    {
                        string unicode = string.Empty;
                        count++;
                        if (parameter[count] == 'u')
                        {
                            count++;
                        }
                        while (count < strlen)
                        {
                            int value = parameter[count];
                            if ((value > 47 && value < 58) || (value > 64 && value < 71) || (value > 96 && value < 103))
                            {
                                unicode += parameter[count];
                            }
                            else
                            {
                                break;
                            }
                            count++;
                        }
                        // unicode convertion
                        int decimalvalue = Convert.ToInt32(unicode, 16);
                        var c = (char) decimalvalue;
                        result += c.ToString();
                    }
                    else
                    {
                        result += parameter[count];
                        count++;
                    }
                }
                if (quoteOpen != ' ')
                {
                    result = "";
                }
                else
                {
                    // Replace <, > and & character to &lt; &gt; &amp;
                    result = result.Replace("&", "&amp;");
                    result = result.Replace("<", "&lt;");
                    result = result.Replace(">", "&gt;");
                }
            }
            return result;
        }
            catch (Exception)
            {
               return result;
            }
        }

        /// <summary>
        /// To list the errors found in CSS inout file and error's are added into ErrorList.
        /// </summary>
        /// <param name="path">Input CSS Filepath</param>
        public void GetErrorReport(string path)
        {
            if(!File.Exists(path)) return;

            var ctp = new CssTreeParser();
            try
            {
                ctp.Parse(path);
                ctp.ValidateError();
                if (ctp.Errors.Count > 0)
                {
                    ErrorText += ctp.ErrorText() + "\r\n";
                    string fileName = Path.GetFileName(path);
                    if (ErrorList.ContainsKey(fileName))
                    {
                        ErrorList[fileName].Add(ctp.Errors);
                    }
                    else
                    {
                        var err = new ArrayList();
                        err.AddRange(ctp.Errors);
                        ErrorList[fileName] = err;
                    }
                }
            }
            catch (Exception)
            {
                ErrorText += ctp.Errors;
                throw;
            }
            finally
            {
                //File.Delete(Path.GetTempPath() + Path.GetFileName(path));
            }

        }

        private void GetRuleContains(TreeNode node, ref Rule getRuleInfo)
        {
            getRuleInfo.NodeCount = node.Nodes.Count;
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Text == "CLASS")
                {
                    childNode.FirstNode.Text = childNode.FirstNode.Text.Replace("_", "").Replace("-", "");
                    getRuleInfo.ClassName += "." + childNode.FirstNode.Text;
                    if (childNode.Nodes.Count > 1)
                    {
                        for (int i = 0; i < childNode.Nodes.Count; i++)
                        {
                            if (childNode.Nodes[i].Text == "ATTRIB")
                            {
                                getRuleInfo.ClassName += "=" +
                                                         childNode.Nodes[i].LastNode.Text.Replace("'", "").Replace(
                                                             "\"", "");
                            }
                            else if (childNode.Nodes[i].Text == "HASVALUE")
                            {
                                getRuleInfo.ClassName += "~" +
                                                         childNode.Nodes[i].LastNode.Text.Replace("'", "").Replace(
                                                             "\"", "");
                            }
                        }
                    }
                }
                else if (childNode.Text == "TAG")
                {
                    getRuleInfo.ClassName += " " + childNode.FirstNode.Text;
                    if (childNode.Nodes.Count > 1)
                    {
                        for (int i = 0; i < childNode.Nodes.Count; i++)
                        {
                            if (childNode.Nodes[i].Text == "ATTRIB")
                            {
                                getRuleInfo.ClassName += "=" +
                                                         childNode.Nodes[i].LastNode.Text.Replace("'", "").Replace(
                                                             "\"", "");
                            }
                            else if (childNode.Nodes[i].Text == "HASVALUE")
                            {
                                getRuleInfo.ClassName += "~" +
                                                         childNode.Nodes[i].LastNode.Text.Replace("'", "").Replace(
                                                             "\"", "");
                            }
                        }
                    }
                }
                else if (childNode.Text == "ANY")
                {
                    getRuleInfo.ClassName += "*" + childNode.FirstNode.Text;
                }
                else if (childNode.Text == "PARENTOF")
                {
                    getRuleInfo.ClassName += ">";
                }
                else if (childNode.Text == "PRECEDES")
                {
                    getRuleInfo.ClassName += "+";
                }
                else if (childNode.Text == "PSEUDO")
                {
                    getRuleInfo.PseudoName = childNode.FirstNode.Text;
                    getRuleInfo.ClassName += ":" + getRuleInfo.PseudoName;

                    getRuleInfo.IsPseudo = true;
                }
                else if (childNode.Text == "PROPERTY")
                {
                    getRuleInfo.HasProperty = true;
                    if (childNode.FirstNode.Text == "content")
                    {
                        if (!getRuleInfo.IsPseudo)
                        {
                            getRuleInfo.IsClassContent = true;
                        }
                        for (int i = 0; i < childNode.Nodes.Count; i++)
                        {
                            if (childNode.Nodes[i].Text.IndexOf("'") >= 0 || childNode.Nodes[i].Text.IndexOf("\"") >= 0)
                            {
                                childNode.Nodes[i].Text = childNode.Nodes[i].Text.Replace("'", "").Replace(
                                    "\"", "");
                                childNode.Nodes[i].Text = UnicodeConversion(childNode.Nodes[i].Text);
                            }
                        }
                    }

                }
            }
        }
    }
}