// --------------------------------------------------------------------------------------------
// <copyright file="StyleAttribute.cs" from='2009' to='2014' company='SIL International'>
//      Copyright © 2014, SIL International. All Rights Reserved.   
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

using System.Globalization;

namespace SIL.PublishingSolution
{
    public class StyleAttribute
    {
        #region Private Variables

        private string _className;
        private string _name;
        private float _numericValue;
        private string _stringValue;
        private string _stringValueLower;
        private string _unit;
        private bool _font;

        #endregion

        #region Public Properties
        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public float NumericValue
        {
            get { return _numericValue; }
            set { _numericValue = value; }
        }
        public string StringValue
        {
            get { return _stringValue; }
            set { _stringValue = value; }
        }
        public string StringValueLower
        {
            get { return _stringValueLower; }
            set { _stringValueLower = value; }
        }
        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        public bool Font
        {
            // TD-504(Fonts need to be included in Project)
            get { return _font; }
            set { _font = value; }
        }
   
        #endregion
     
        #region Public Methods
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Store _attribute information.
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="attributeValue">_attribute value</param>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        public void SetAttribute(string attributeValue)
        {
            string chars = string.Empty;
            int counter;
            attributeValue = attributeValue.Trim();
            for (counter = 0; counter < attributeValue.Length; counter++)
            {
                char chr = char.Parse(attributeValue.Substring(counter, 1));
                var value = (int)chr;
                if (!((value >= 48 && value <= 57) || value == 46)) // 0 to 9 and decimal
                {
                    break;
                }
                chars = chars + chr;
            }
            ClassName = string.Empty;
            Name = string.Empty;
            if (chars.Length > 0)
            {
                NumericValue = float.Parse(chars);
            }
            else
            {
                StringValue = attributeValue.Substring(counter).Trim();
            }
            Unit = attributeValue.Substring(counter).Trim();
        }
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Store _attribute information.
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="attributeName">_attribute Name</param>
        /// <param name="attributeValue">_attribute value</param>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        public void SetAttribute(string attributeName, string attributeValue)
        {
            string chars = string.Empty;
            int counter;
            attributeValue = attributeValue.Trim();
            for (counter = 0; counter < attributeValue.Length; counter++)
            {
                char chr = char.Parse(attributeValue.Substring(counter, 1));
                var value = (int)chr;
                if (!((value >= 48 && value <= 57) || value == 46)) // 0 to 9 and decimal
                {
                    break;
                }
                chars = chars + chr;
            }
            ClassName = string.Empty;
            Name = attributeName;
            if (chars.Length > 0)
            {
                NumericValue = float.Parse(chars);
            }
            else
            {
                StringValue = attributeValue.Substring(counter).Trim();
            }
            Unit = attributeValue.Substring(counter).Trim();
        }
        /// -------------------------------------------------------------------------------------------
        /// <summary>
        /// Store _attribute information.
        /// 
        /// <list> 
        /// </list>
        /// </summary>
        /// <param name="className">Style name</param>
        /// <param name="attributeName">_attribute Name</param>
        /// <param name="attributeValue">_attribute value</param>
        /// <returns> </returns>
        /// -------------------------------------------------------------------------------------------
        public void SetAttribute(string className,string attributeName, string attributeValue)
        {
            string chars = string.Empty;
            int counter;
            attributeValue = attributeValue.Trim();
            for (counter = 0; counter < attributeValue.Length; counter++)
            {
                char chr = char.Parse(attributeValue.Substring(counter, 1));
                var value =  (int)chr;
                if ( !(( value>=48 && value<=57 ) || value==46)) // 0 to 9 and decimal
                {
                    break;
                }
                chars = chars + chr;
            }
            ClassName = className;
            Name = attributeName;
            if (chars.Length > 0)
            {
                NumericValue = float.Parse(chars, CultureInfo.GetCultureInfo("en-US"));
            }
            else
            {
                StringValue = attributeValue.Substring(counter).Trim();
            }
            Unit = attributeValue.Substring(counter).Trim();
        }
        #endregion
    }
}