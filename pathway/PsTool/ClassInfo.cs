// --------------------------------------------------------------------------------------------
// <copyright file="SelectorClass.cs" from='2009' to='2009' company='SIL International'>
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
// Used for Stylesxml
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Collections;

namespace SIL.PublishingSolution
{
    /// <summary>
    /// Storing classes and attributes
    /// .x [class~='y'] [lang = 'en']
    /// </summary>
    public class ClassAttrib
    {
        public ClassAttrib()
        {
            _className = string.Empty;
        }
        /// <summary>
        /// storing classname example: .x [class~='y']
        /// </summary>
        private string _className;
        //private string _tagName;
        /// <summary>
        /// storing attribname example: [lang = 'en']
        /// </summary>
        private ArrayList _attribute = new ArrayList();

        public string ClassName
        {
            set { _className = value; }
            get { return _className; }
        }

        public ArrayList Attribute
        {
            get { return _attribute; }
        }
        public void SetClassAttrib(string className, ArrayList attribute)
        {
            _className = className;
            _attribute.Clear();
            if (attribute != null) 
                _attribute.AddRange(attribute);
        }
        public void Clear()
        {
            _className = string.Empty;
            _attribute.Clear();
        }
    }

    public class ClassInfo
    {
        //private string styleName;
        private ClassAttrib coreClass = new ClassAttrib();
        private string tagName = string.Empty;
        private ClassAttrib tag = new ClassAttrib();
        private ClassAttrib precede = new ClassAttrib();
        public ArrayList parent = new ArrayList();
        //private ClassAttrib parent = new ClassAttrib();
        private ClassAttrib ancestor = new ClassAttrib();
        private ClassAttrib parentPrecede = new ClassAttrib();
        public ClassAttrib CoreClass
        {
            set { coreClass.SetClassAttrib(value.ClassName, value.Attribute); }
            get { return coreClass; }
        }
        public ClassAttrib Tag
        {
            set { tag.SetClassAttrib(value.ClassName, value.Attribute); }
            get { return tag; }
        }
        public ClassAttrib Precede
        {
            set { precede.SetClassAttrib(value.ClassName, value.Attribute); }
            get { return precede; }
        }

        public ClassAttrib Ancestor
        {
            set { ancestor.SetClassAttrib(value.ClassName, value.Attribute); }
            get { return ancestor; }
        }

        public ClassAttrib ParentPrecede
        {
            set { parentPrecede.SetClassAttrib(value.ClassName, value.Attribute); }
            get { return parentPrecede; }
        }
        public string TagName
        {
            get { return tagName; }
            set { tagName = value; }
        }

        private string _styleName;
        private string _content;
        private string _contains;
        private string _pseudo;
        private int _specificityWeightage;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        public string Contains
        {
            get { return _contains; }
            set { _contains = value; }
        }
        public string Pseudo
        {
            get
            {
                if (_pseudo != null)
                    return _pseudo;
                else
                    return "null";
            }
            set { _pseudo = value; }
        }
        public int SpecificityWeightage
        {
            get { return _specificityWeightage; }
            set { _specificityWeightage = value; }
        }
        public string StyleName
        {
            get { return _styleName; }
            set { _styleName = value; }
        }
    }
}