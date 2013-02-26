// --------------------------------------------------------------------------------------------
// <copyright file="Dic4MidStyle.cs" from='2013' to='2013' company='SIL International'>
//      Copyright © 2013, SIL International. All Rights Reserved.   
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

namespace SIL.PublishingSolution
{
    public class Dic4MidStyle
    {
        protected struct Style
        {
            public string DisplayText;
            public string FontColor;
            public string ContentStyle;
        } ;

        protected const int NumStyles = 100;
        protected Style[] Styles = new Style[NumStyles];
        protected int CurStyles = 0;

        public Dic4MidStyle()
        {
            Styles[CurStyles].DisplayText = "Default";
            Styles[CurStyles].FontColor = "128,0,0";
            Styles[CurStyles].ContentStyle = "plain";
            CurStyles++;
        }

        public string DisplayText(int n)
        {
            if (n >= CurStyles)
                n = 1;
            return Styles[n - 1].DisplayText;
        }

        public string FontColor(int n)
        {
            if (n >= CurStyles)
                n = 1;
            return Styles[n - 1].FontColor;
        }

        public string ContentStyle(int n)
        {
            if (n >= CurStyles)
                n = 1;
            return Styles[n - 1].ContentStyle;
        }

        public int Add(string text, string color, string style)
        {
            if (CurStyles >= NumStyles)
                throw new OverflowException("Content Styles");
            Styles[CurStyles].DisplayText = text;
            Styles[CurStyles].FontColor = color;
            Styles[CurStyles].ContentStyle = style;
            return ++CurStyles;
        }
    }
}
