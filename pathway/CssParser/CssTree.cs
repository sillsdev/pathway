using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class CssTree
    {
        private Dictionary<string, Dictionary<string, string>> _cssClass = new Dictionary<string, Dictionary<string, string>>();
        //private Dictionary<string, string> _cssProperty = new Dictionary<string, string>();
        private Dictionary<string, string> _cssProperty;
        private ClassInfo _classInfo;
        private VerboseClass _verboseWriter = VerboseClass.GetInstance();
        MakeProperty _mapProperty = new MakeProperty();
        public ArrayList CssClassOrder = new ArrayList();
        public Dictionary<string, ArrayList> SpecificityClass = new Dictionary<string, ArrayList>();
        private string _baseClassName;
        private int _specificityWeightage;
        public ArrayList cssBorderColor;
        private bool _setDefaultPageValue = true;

        public Dictionary<string, Dictionary<string, string>> CreateCssProperty(string cssSourceFile, bool setDefaultPageValue)
        {
            var cssTree = new CssParserDuplicateClass();
            TreeNode node = cssTree.BuildTree(cssSourceFile);
            //To show errors to user to edit and save the CSS file.
            //node = CssErrorHandler(cssTree, cssSourceFile, node);  // Error Handling.
            _cssClass.Clear();
            ProcessCSSTree(node);
            SetLeftRightFirstPage(setDefaultPageValue);
            cssBorderColor = _mapProperty.CssBorderColor;
            SetDefaultTagProperty();
            return _cssClass;
        }

        private void SetDefaultTagProperty()
        {
            Dictionary<string, Dictionary<string, string>> defaultTagProperty =
                new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> propertyName;

            //h1 to h6
            propertyName = new Dictionary<string, string>();
            propertyName["font-weight"] = "bold";
            propertyName["padding-top"] = "12";
            propertyName["font-size"] = "24";
            defaultTagProperty["h1"] = propertyName;

            propertyName = new Dictionary<string, string>();
            propertyName["font-weight"] = "bold";
            propertyName["padding-top"] = "8";
            propertyName["font-size"] = "18";
            defaultTagProperty["h2"] = propertyName;

            propertyName = new Dictionary<string, string>();
            propertyName["font-weight"] = "bold";
            propertyName["padding-top"] = "7";
            propertyName["font-size"] = "14";
            defaultTagProperty["h3"] = propertyName;

            propertyName = new Dictionary<string, string>();
            propertyName["font-weight"] = "bold";
            propertyName["padding-top"] = "6";
            propertyName["font-size"] = "12";
            defaultTagProperty["h4"] = propertyName;

            propertyName = new Dictionary<string, string>();
            propertyName["font-weight"] = "bold";
            propertyName["padding-top"] = "5.5";
            propertyName["font-size"] = "10";
            defaultTagProperty["h5"] = propertyName;

            propertyName = new Dictionary<string, string>();
            propertyName["font-weight"] = "bold";
            propertyName["padding-top"] = "5.5";
            propertyName["font-size"] = "8";
            defaultTagProperty["h6"] = propertyName;

            //para
            propertyName = new Dictionary<string, string>();
            propertyName["padding-top"] = "12";
            propertyName["padding-bottom"] = "12";
            defaultTagProperty["p"] = propertyName;


            foreach (KeyValuePair<string, Dictionary<string, string>> pair in defaultTagProperty)
            {
                if (!_cssClass.ContainsKey(pair.Key)) // h1
                {
                    _cssClass[pair.Key] = defaultTagProperty[pair.Key];
                    //for Specificity
                    _classInfo = new ClassInfo();
                    _classInfo.Tag.SetClassAttrib(pair.Key, _classInfo.Tag.Attribute);
                    _classInfo.CoreClass.SetClassAttrib(string.Empty, _classInfo.Tag.Attribute);
                    _classInfo.TagName = _classInfo.Tag.ClassName; 
                    _classInfo.SpecificityWeightage = 1;
                    _classInfo.StyleName = pair.Key;
                    //CssClassOrder.Add(_classInfo.CoreClass.ClassName);
                    SetSpecificityClass(pair.Key, _classInfo);

                }
                else
                {
                    foreach (KeyValuePair<string, string> property in pair.Value)
                    {
                        if (!_cssClass[pair.Key].ContainsKey(property.Key))
                        {
                            _cssClass[pair.Key][property.Key] = property.Value;
                        }
                    }
                }
            }
        }

        
        private void SetLeftRightFirstPage(bool setDefaultPageValue)
        {
            _setDefaultPageValue = setDefaultPageValue;
            SetDefaultPage();
            ArrayList pageClass = new ArrayList();
            foreach (KeyValuePair<string, Dictionary<string, string>> className in _cssClass)
            {
                string clsName = className.Key;
                if (clsName.IndexOf("@page-") >= 0 || clsName == "@page")
                {
                    pageClass.Add(clsName);
                }
            }

            foreach (string className in pageClass)
            {
                try
                {
                    string firstPage = className.Insert(5, ":first");
                    AppendDictionary(className, firstPage);
                    string leftPage = className.Insert(5, ":left");
                    if (IsPageContains(leftPage))
                        AppendDictionary(className, leftPage);
                    string rightPage = className.Insert(5, ":right");
                    if (IsPageContains(rightPage))
                        AppendDictionary(className, rightPage);
                }
                catch
                {
                }
            }
        }

        private bool IsPageContains(string pagename)
        {
            foreach (string className in _cssClass.Keys)
            {
                if ((className.IndexOf(pagename) == 0) || (className.IndexOf(pagename + "-") == 0))
                {
                    return true;
                }
            }
            return false;
        }

        private void SetDefaultPage()
        {
            string page = "@page";
            _cssProperty = new Dictionary<string, string>();
            if (_setDefaultPageValue)
            {
                //_cssProperty["height"] = "792";
                //_cssProperty["width"] = "612";
                _cssProperty["page-height"] = "792";
                _cssProperty["page-width"] = "612";
                _cssProperty["margin-left"] = "56.7";
                _cssProperty["margin-right"] = "56.7";
                _cssProperty["margin-top"] = "56.7";
                _cssProperty["margin-bottom"] = "56.7";
                _cssProperty["mirror"] = "false";
                _cssProperty["-ps-fileproduce"] = "One";
            }

            if (_setDefaultPageValue && !_cssClass.ContainsKey(page))
            {
                _cssClass[page] = _cssProperty;
            }
            else
            {
                foreach (KeyValuePair<string, string> property in _cssProperty)
                {
                    if (!_cssClass[page].ContainsKey(property.Key))
                    {
                        _cssClass[page][property.Key] = property.Value;
                    }
                }
            }
        }

        private void AppendDictionary(string clsName, string appendTo)
        {
            if (_cssClass.ContainsKey("@page") && _cssClass["@page"].ContainsKey("mirror") &&_cssClass["@page"]["mirror"] != "true")
            {
                if (appendTo.IndexOf(":left") > 0 || appendTo.IndexOf(":right") > 0)
                {
                    _cssClass["@page"]["mirror"] = "true";
                }
            }
            if (!_cssClass.ContainsKey(appendTo))
            {
                _cssProperty = new Dictionary<string, string>();
                _cssProperty = _cssClass[clsName];
                _cssClass[appendTo] = _cssProperty;
            }
            else
            {
                foreach (KeyValuePair<string, string> property in _cssClass[clsName])
                {
                    if (!_cssClass[appendTo].ContainsKey(property.Key))
                    {
                        _cssClass[appendTo][property.Key] = property.Value;
                    }

                }
            }
        }

        //private TreeNode CssErrorHandler(CSSParser.CSSParser cssTree, string cssSourceFile, TreeNode node)
        //{
        //if (cssTree.ErrorList.Count > 0)
        //{
        //    var errForm = new CSSError(cssTree.ErrorList, Path.GetDirectoryName(cssSourceFile));
        //    errForm.ShowDialog();
        //    cssTree = new CSSParser.CSSParser();
        //    node = cssTree.BuildTree(cssSourceFile);
        //}
        //return node;
        //}


        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate Styles.xml body from Antlr tree
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="node">Antlr XMLNode</param>
        /// <returns></returns>
        /// -------------------------------------------------------------------------------------------
        private void ProcessCSSTree(TreeNode node)
        {
            try
            {
                foreach (TreeNode child in node.Nodes)
                {
                    if (child.Text == "RULE") // Handle Class and Property
                    {
                        ClassAndProperty(child);
                    }
                    else if (child.Text == "PAGE") // Handle @page class
                    {
                        Page(child);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void Page(TreeNode tree)
        {
            StyleAttribute _attributeInfo = new StyleAttribute();
            string pageName = "@page";
            string pseudoName = "@page";
            string regionName;
            try
            {
                _classInfo = new ClassInfo();
                foreach (TreeNode node in tree.Nodes)
                {
                    switch (node.Text)
                    {
                        case "PSEUDO":
                            pseudoName = pageName + ":" + node.FirstNode.Text;
                            break;
                        case "REGION":
                            regionName = pseudoName + "-" + node.FirstNode.Text;
                            foreach (TreeNode property in node.Nodes)
                            {
                                if (property.Text == "PROPERTY")
                                {
                                    Property(property, regionName);
                                }
                            } 
                            break;
                        case "PROPERTY":
                            Property(node, pseudoName);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }


        private void ClassAndProperty(TreeNode tree)
        {
            string styleName = string.Empty;
            string tagName = string.Empty;
            string tagStyleName = string.Empty;
            _specificityWeightage = 0;
            ClassAttrib clsAttrib = new ClassAttrib();
            bool isAncestor = false;
            try
            {
                _classInfo = new ClassInfo();
                foreach (TreeNode node in tree.Nodes)
                {
                    switch (node.Text)
                    {
                        case "TAG":
                            _classInfo.Tag = ClassNode(node);
                            _classInfo.CoreClass.SetClassAttrib(string.Empty, _classInfo.Tag.Attribute);
                            _classInfo.TagName = Common.LeftString(_classInfo.Tag.ClassName,Common.SepAttrib); 
                            tagName = GetFirstChild(node) + Common.SepTag;
                            tagStyleName = _classInfo.Tag.ClassName;
                            tagStyleName = GetImageAttrib(tagStyleName);
                            _specificityWeightage += 1;
                            break;
                        case "CLASS":
                            if (isAncestor)
                            {
                                _classInfo.Ancestor = clsAttrib;
                                styleName = Common.SepAncestor + styleName;
                                isAncestor = false;
                            }
                            isAncestor = true;
                            clsAttrib = ClassNode(node);
                            string tagClassName = tagName + clsAttrib.ClassName;
                            styleName = tagClassName + styleName;
                            clsAttrib.ClassName = _baseClassName;
                            _specificityWeightage += 10;
                            break;
                        case "PARENTOF":
                            _classInfo.ParentPrecede = _classInfo.Precede;
                            _classInfo.Precede.Clear();
                            _classInfo.parent.Add(clsAttrib);
                            styleName = Common.SepParent + styleName;
                            isAncestor = false;
                            break;
                        case "PRECEDES":
                            _classInfo.Precede = clsAttrib;
                            styleName = Common.sepPrecede + styleName;
                            isAncestor = false;
                            break;
                        case "PSEUDO":
                            _classInfo.Pseudo = GetPseudo(node);
                            styleName = styleName + Common.SepPseudo + _classInfo.Pseudo;
                            break;
                        case "PROPERTY":
                            if (clsAttrib.ClassName == string.Empty) // tag (div,span,..)without className}
                            {
                                styleName = tagStyleName;
                            }
                            else
                            {
                                _classInfo.CoreClass = clsAttrib;
                            }
                            Property(node, styleName);
                            break;
                    }
                }
                _classInfo.SpecificityWeightage = _specificityWeightage;
                _classInfo.StyleName = styleName;
                CssClassOrder.Add(_classInfo.CoreClass.ClassName);
                SetSpecificityClass(_baseClassName,_classInfo);

                //// Todo :remove the PseduoBefore class
                //if(_classInfo.Pseudo.Length > 0 )
                //  PseduoBefore.SelectorClass[className] =_classInfo;

                //_styleName.AllCSSName.Add(className);
                
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private string GetImageAttrib(string styleName)
        {
            if(_classInfo.Tag.ClassName.EndsWith("src"))
            {
                styleName = _classInfo.Tag.ClassName.ToLower().Replace("_.src","");
            }
            return styleName;
        }

        private void SetSpecificityClass(string baseClassName, ClassInfo classInfo)
        {

            if (!SpecificityClass.ContainsKey(baseClassName))
            {
                SpecificityClass[baseClassName] = new ArrayList();
                SpecificityClass[baseClassName].Add(classInfo);
            }
            else
            {
                int insertPos = 0;
                foreach (ClassInfo clsInfo in SpecificityClass[baseClassName])
                {
                    if (_specificityWeightage >= clsInfo.SpecificityWeightage)
                    {
                        break;
                    }
                    insertPos++;
                }
                SpecificityClass[baseClassName].Insert(insertPos, classInfo);
            }
        }

        private string GetPseudo(TreeNode node)
        {
            string pseudoName = string.Empty;
            try
            {
                pseudoName = node.FirstNode.Text;
                if (pseudoName.ToLower() == "contains")
                {
                    string contain = node.Nodes[2].Text;
                    contain = contain.Replace("'", "");
                    contain = contain.Replace("\"", "");
                    _classInfo.Contains = contain;
                }
                return (pseudoName);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return (pseudoName);
            }
        }

        private void Property(TreeNode node, string clsName)
        {
            StyleAttribute _attributeInfo;
            _attributeInfo = Properties(node);
            _attributeInfo.ClassName = clsName;
            
            if (_attributeInfo.Name.ToLower() == "content")
            {
                //_attributeInfo.StringValue = Common.UnicodeConversion(_attributeInfo.StringValue);
                //_attributeInfo.StringValue = _attributeInfo.StringValue.Replace("'", "");
                //_attributeInfo.StringValue = _attributeInfo.StringValue.Replace("\"", "");
                _attributeInfo.StringValue = ReplaceCountertoPipeLine(_attributeInfo.StringValue);
                //if (_classInfo.Pseudo == "before")
                //{
                //    _attributeInfo.StringValue = _attributeInfo.StringValue + " ";
                //}
                //else if (_classInfo.Pseudo == "after")
                //{
                //    _attributeInfo.StringValue = " " + _attributeInfo.StringValue;
                //}
                _classInfo.Content = _attributeInfo.StringValue;

                if (_classInfo.Content.ToLower() == "normal") _classInfo.Content = string.Empty;
            }
            //else
            //{
            //CreateFullyQualifiedName(_attributeInfo);
            CreateFullyQualifiedClassName(_attributeInfo);

            AddProperty(_attributeInfo);
            //}
        }

        private void CreateFullyQualifiedName(StyleAttribute _attributeInfo)
        {
            if (_attributeInfo.ClassName == "@page")
            {
                if (_attributeInfo.Name == "margin-left" || _attributeInfo.Name == "margin-right" ||
                    _attributeInfo.Name == "margin-top" || _attributeInfo.Name == "margin-bottom" || _attributeInfo.Name == "margin")
                {
                    _attributeInfo.Name = "page-" + _attributeInfo.Name;
                }
            }
        }

        static private void CreateFullyQualifiedClassName(StyleAttribute _attributeInfo)
        {
            if (_attributeInfo.ClassName.IndexOf("@page") == -1)
            {
                if (_attributeInfo.Name == "margin-left" || _attributeInfo.Name == "margin-right" ||
                    _attributeInfo.Name == "margin-top" || _attributeInfo.Name == "margin-bottom" || _attributeInfo.Name == "margin")
                {
                    _attributeInfo.Name = "class-" + _attributeInfo.Name;
                }
            }
        }
        private string ReplaceCountertoPipeLine(string value)
        {
            string result = value;
            if(value.IndexOf("counter") >= 0)
            {
                value = value.Replace(",", "");
                value = value.Replace(")", ")|");
                result = value.Replace("counter", "|counter");
            }
            return result;
        }

        private ClassAttrib ClassNode(TreeNode classNode)
        {
            ClassAttrib clsAtt = new ClassAttrib();
            ArrayList clsAttrib = new ArrayList();

            ClassAttribute classAttribute;
            ArrayList classCollection = new ArrayList();

            string className = GetFirstChild(classNode);
            _baseClassName = className;

            foreach (TreeNode node in classNode.Nodes)
            {
                if (node.Text == "ATTRIB")
                {
                    classAttribute = GetAttribValue(node);
                    classCollection.Add(classAttribute);
                }
            }

            foreach (ClassAttribute classAttrib in classCollection)
            {
                if (string.Compare(classAttrib.AttributeSeperator, "HASVALUE") == 0)
                {
                    classAttrib.AttributeValue = classAttrib.AttributeValue.Replace("-", "");
                    classAttrib.AttributeValue = classAttrib.AttributeValue.Replace("_", "");
                    className = className + Common.SepAncestor + classAttrib.AttributeValue;
                    _baseClassName = _baseClassName + Common.Space + classAttrib.AttributeValue;
                    _baseClassName = Common.SortMutiClass(_baseClassName);
                    _specificityWeightage += 10;
                }
                else if (string.Compare(classAttrib.AttributeSeperator, "ATTRIBEQUAL") == 0)
                {
                    string attribute;
                    classAttrib.AttributeValue = classAttrib.AttributeValue.Replace("\"", "");
                    if (classAttrib.Name == "lang")
                    {
                        attribute = classAttrib.AttributeValue; // remove string "lang"
                    }
                    else if (classAttrib.Name == "src")
                    {
                        attribute = "src"; 
                        className = classAttrib.AttributeValue;
                    }

                    else
                    {
                        attribute = classAttrib.Name + classAttrib.AttributeValue;
                    }
                    className = className + Common.SepAttrib + attribute;
                    clsAttrib.Add(attribute);
                    _specificityWeightage += 10;
                }
                else if (classAttrib.AttributeSeperator.Length == 0)
                {
                    className = className + Common.SepAttrib + classAttrib.Name;
                    clsAttrib.Add(classAttrib.Name);
                    _specificityWeightage += 10;
                }
            }
            clsAtt.SetClassAttrib(className, clsAttrib);
            return clsAtt;
        }

        /// <summary>
        /// To replace the symbol to text
        /// </summary>
        /// <param name="value">input symbol</param>
        /// <returns>Replaced text</returns>
        private void ReplaceSymbolToText(string value)
        {
            // refer stylesXML.cs
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Map _attributeInfo to MapPropertys.cs
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="styleAttributeInfo">StyleAttribute styleAttributeInfo</param>
        /// <returns>StyleAttribute _attributeInfo</returns>
        /// -------------------------------------------------------------------------------------------        
        /// 
        private void AddProperty(StyleAttribute styleAttributeInfo)
        {
            try
            {

                if (!_cssClass.ContainsKey(styleAttributeInfo.ClassName))
                {
                    _cssProperty = new Dictionary<string, string>();
                    _cssClass[styleAttributeInfo.ClassName] = _cssProperty;
                }
                else
                {
                    _cssProperty = _cssClass[styleAttributeInfo.ClassName];
                }

                Dictionary<string, string> getProperty = new Dictionary<string, string>();
                getProperty = _mapProperty.CreateProperty(styleAttributeInfo);

                foreach (var prop in getProperty)
                {
                    _cssProperty[prop.Key] = prop.Value.Replace("\"","");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _verboseWriter.WriteError(styleAttributeInfo.ClassName, styleAttributeInfo.Name, ex.Message, styleAttributeInfo.StringValue);
            }
            //return styleAttributeInfo;
        }

        #region SplitValue

        private void SplitValue(string attributeStringValue, string keyName)
        {
            string attribVal = string.Empty;
            try
            {
                int attribCount = 0;
                string[] parameter = attributeStringValue.Split(',');
                foreach (string param in parameter)
                {
                    attribVal = attribVal + param;
                    if (param == "0" || Common.ValidateAlphabets(param) == true || param.IndexOf('#') > -1)
                    {
                        attribCount++;
                        if (attribCount != parameter.Length)
                        {
                            attribVal = attribVal + "+";
                        }
                    }
                }
                if (attribCount == 1)
                {
                    string[] splitPlus = attribVal.Split('+');
                    //_styleName.BorderProperty[keyName] = splitPlus[0];
                }
                else if (attribCount == 2)
                {
                    string[] splitPlus = attribVal.Split('+');
                    //_styleName.BorderProperty[keyName] = splitPlus[0] + "," + splitPlus[1] + ","
                    //+ splitPlus[0] + "," + splitPlus[1];
                }
                else if (attribCount == 3)
                {
                    string[] splitPlus = attribVal.Split('+');
                    //_styleName.BorderProperty[keyName] = splitPlus[0] + "," + splitPlus[1] + ","
                    //+ splitPlus[2] + "," + splitPlus[1];
                }
                else if (attribCount == 4)
                {
                    string[] splitPlus = attribVal.Split('+');
                    //_styleName.BorderProperty[keyName] = splitPlus[0] + "," + splitPlus[1] + ","
                    //+ splitPlus[2] + "," + splitPlus[3];
                }
            }
            catch
            {
                //_styleName.BorderProperty[keyName] = "0pt";
            }
        }
        #endregion


        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Get className from the node
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="tree">Antlr Tree</param>
        /// <param name="tree">Antlr tree collection</param>
        /// <returns>class Name</returns>
        /// -------------------------------------------------------------------------------------------        
        /// 
        private string GetClassName(TreeNode tree)
        {
            string className = string.Empty;
            try
            {
                //TODO - Remove NEST classNames
                className = tree.FirstNode.Text;
                return (className);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return (className);
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate property nodes
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="tree">Css tree</param>
        /// <returns></returns>
        /// -------------------------------------------------------------------------------------------        
        /// 
        private StyleAttribute Properties(TreeNode tree)
        {
            ArrayList unit = new ArrayList();
            unit.Add("ex");
            unit.Add("px");
            unit.Add("pc");

            StyleAttribute attribute = new StyleAttribute();
            attribute.Name = string.Empty;
            try
            {
                StringBuilder attributeVal = new StringBuilder("");
                string numericProperty = string.Empty;
                int numericPropertyPosition = 0;
                foreach (TreeNode node in tree.Nodes)
                {
                    if (attribute.Name == string.Empty)
                    {
                        attribute.Name = node.Text;
                    }
                    else
                    {
                        if (unit.Contains(node.Text))
                        {
                            if (Common.ValidateNumber(numericProperty))
                            {
                                string targetUnit = "pt";
                                if (node.Text == "ex")
                                {
                                    targetUnit = "em";
                                }
                                string propertyWithUnit = numericProperty + node.Text;
                                float convertedValue = Common.UnitConverterOO(propertyWithUnit, targetUnit);

                                string convertedValueWithUnit = convertedValue.ToString() + "," + targetUnit + ",";

                                //attributeVal = attributeVal.Remove( (0, numericPropertyPosition);
                                attributeVal = attributeVal.Remove(numericPropertyPosition, attributeVal.Length - numericPropertyPosition);

                                attributeVal = attributeVal.Append(convertedValueWithUnit);
                                numericPropertyPosition = 0;
                            }
                        }
                        else
                        {
                            numericProperty = node.Text;
                            numericPropertyPosition = attributeVal.Length;
                            attributeVal = attributeVal.Append(node.Text + ",");
                        }
                    }
                }

                int len = attributeVal.Length;
                if (len > 0)
                {
                    attributeVal.Remove(len - 1, 1);
                    attribute.StringValue = attributeVal.ToString();
                }
                else
                {
                    attribute.StringValue = "0";

                }

                return (attribute);

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                if (_verboseWriter.ShowError)
                {
                    _verboseWriter.WriteError("", "", ex.Message + "</BR>", "");
                }
                return (attribute);
            }
            //TODO Invalid Value
        }

        private string GetFirstChild(TreeNode tree)
        {
            string firstChild = string.Empty;
            try
            {
                firstChild = tree.FirstNode.Text;
                return (firstChild);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return (firstChild);
            }
        }

        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Get GetAttributes Values from the node
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="tree">Antlr Tree</param>
        /// <returns>ATTRIB</returns>
        /// -------------------------------------------------------------------------------------------        
        /// 
        private ClassAttribute GetAttribValue(TreeNode tree)
        {
            try
            {
                ClassAttribute classAttribute = new ClassAttribute();
                if (tree.Nodes.Count == 1)
                {
                    classAttribute.SetAttribute(tree.FirstNode.Text);
                }
                else if (tree.Nodes.Count == 3)
                {
                    classAttribute.SetAttribute(tree.Nodes[0].Text, tree.Nodes[1].Text, tree.Nodes[2].Text);
                }
                return (classAttribute);
            }
            catch
            {
                return (null);
            }
        }

        public ArrayList GetFontList(string cssFileWithPath)
        {
            ArrayList fontName = new ArrayList();
            ArrayList fontList = new ArrayList();
            var cssClass = new Dictionary<string, Dictionary<string, string>>();
            cssClass = CreateCssProperty(cssFileWithPath, true);

            foreach (var className in cssClass)
            {
                if (cssClass[className.Key].ContainsKey("font-family"))
                {
                    if (!fontName.Contains(cssClass[className.Key]["font-family"]))
                        fontName.Add(cssClass[className.Key]["font-family"]);
                }
            }
            RegistryKey fontsKey;
            try
            {
                fontsKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\MICROSOFT\WINDOWS NT\CURRENTVERSION\Fonts\");

                string[] fontNames = fontsKey.GetValueNames();
                foreach (string font in fontName)
                {
                    for (int i = 0; i < fontNames.Length; i++)
                    {
                        if (fontNames[i].IndexOf(font) < 0) continue;
                        Object fName = fontsKey.GetValue(fontNames[i]);
                        fontList.Add(fName);
                    }
                }
                fontList.Sort();
            }
            catch (Exception)
            {
            }
            return fontList;
        }

        public string PageSize(string dimension)
        {
            var sizes = new Dictionary<string, string>
            {
            {"595x842", "a4"},
            {"420x595", "a5"},
            {"298x420", "a6"},
            {"459x649", "c5"},
            {"378x594", "gps1"},
            {"418x626", "gps2"},
            {"396x612", "halfletter"},
            {"612x792", "letter"},
            {"432x648", "middle"},
            };
            if (sizes.ContainsKey(dimension))
                return sizes[dimension];
            return "letter";
        }
    }
}