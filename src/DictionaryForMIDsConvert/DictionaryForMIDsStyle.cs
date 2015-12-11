// --------------------------------------------------------------------------------------------
// <copyright file="DictionaryForMIDsStyle.cs" from='2013' to='2014' company='SIL International'>
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
// Stylepick FeatureSheet
// </remarks>
// --------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace SIL.PublishingSolution
{
    public class DictionaryForMIDsStyle
    {
        protected struct Style
        {
            public string DisplayText;
            public string FontColor;
            public string ContentStyle;
        } ;

        protected const int MaxStyles = 100;
        protected Style[] Styles = new Style[MaxStyles];
        public int NumStyles = 0;
        protected Dictionary<string, int> Index = new Dictionary<string,int>();

        public DictionaryForMIDsStyle()
        {
            Styles[NumStyles].DisplayText = "Default";
            Styles[NumStyles].FontColor = "128,0,0";
            Styles[NumStyles].ContentStyle = "plain";
            AddIndex(++NumStyles);
        }

        private void AddIndex(int styleNum)
        {
            var key = Styles[styleNum - 1].DisplayText;
            Index[key] = styleNum;
        }

        public string DisplayText(int n)
        {
            if (n > NumStyles)
                n = 1;
            return Styles[n - 1].DisplayText;
        }

        public string FontColor(int n)
        {
            if (n > NumStyles)
                n = 1;
            return Styles[n - 1].FontColor;
        }

        public string ContentStyle(int n)
        {
            if (n > NumStyles)
                n = 1;
            return Styles[n - 1].ContentStyle;
        }

        public int Add(string text, string color, string style)
        {
            var key = text;
            if (Index.ContainsKey(key))
            {
                var idx = Index[key];
                if (FontColor(idx) == color && ContentStyle(idx) == style)
                    return idx;
                int n = 2;
                while (Index.ContainsKey(text + n.ToString()))
                {
                    idx = Index[text + n.ToString()];
                    if (FontColor(idx) == color && ContentStyle(idx) == style)
                        return idx;
                }
                text += n.ToString();
            }
            if (NumStyles >= MaxStyles)
                throw new OverflowException("Content Styles");
            Styles[NumStyles].DisplayText = text;
            Styles[NumStyles].FontColor = color;
            Styles[NumStyles].ContentStyle = style;
            AddIndex(++NumStyles);
            return NumStyles;
        }
    }
}
