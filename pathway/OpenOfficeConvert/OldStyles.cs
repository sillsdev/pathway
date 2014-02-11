// --------------------------------------------------------------------------------------------
// <copyright file="Styles.cs" from='2009' to='2009' company='SIL International'>
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
// Styles Type Initialization for stylesxml 
// </remarks>
// --------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

namespace SIL.PublishingSolution
{
    public struct OldStyles
    {
        public IDictionary<string, IDictionary<string, string>> CssClassName;
        public ArrayList SectionName;
        public Dictionary<string, string> PseudoClassAfter;
        public Dictionary<string, string> PseudoClassBefore;
        public Dictionary<string, string> PseudoAncestorBefore;
        public Dictionary<string, string> PseudoAttrib;
        public Dictionary<string, string> FloatAlign;
        public Dictionary<string, ArrayList> ImageSize;
        public Dictionary<string, string> AttribLangBeforeList;
        public Dictionary<string, ArrayList> AttribAncestor;
        public bool IsMacroEnable;
        public ArrayList BackgroundColor;
        public Dictionary<string, Dictionary<string, string>> ColumnGapEm;
        public ArrayList WhiteSpace;
        public ArrayList DisplayBlock;
        public ArrayList DisplayInline;
        public ArrayList DisplayNone;
        public Dictionary<string, int> ContentCounter;
        public Dictionary<string, string> ContentCounterReset;
        public Dictionary<string, Dictionary<string, string>> CounterParent;
        public Dictionary<string, string> ClearProperty;
        public ArrayList PseudoClass;
        public ArrayList PseudoPosition;
        public float ColumnWidth;
        public Dictionary<string, string> ClassContent; // TD_204(like Class { Content: ''})
        public Dictionary<string, string> PrecedeClass; 

        public ArrayList MasterDocument;
        public Dictionary<string, string> TagAttrib;
        public Dictionary<string, string> BorderProperty;
        public Dictionary<string, string> ListType;

        public ArrayList DisplayFootNote;
        public Dictionary<string, string> FootNoteCall;
        public Dictionary<string, string> FootNoteMarker;
        public Dictionary<string, string> FootNoteSeperator;
        public Dictionary<string, ArrayList> SpellCheck;

        public Dictionary<string, string> ClassContainsSelector; //TD-351[Implement :contains("Lamutua")]
        public Dictionary<string, Dictionary<string, string>> ImageSource;
        public ArrayList AllCSSName;
        public ArrayList DropCap;
        public Dictionary<string, string> ReplaceSymbolToText; //TD-479( create property to substitute quote characters)

        public bool IsVerseNumberInclude;
        public string ReferenceFormat;

        public bool IsAutoWidthforCaption;
        public ArrayList UsedFontsList; //TD-504

        public Dictionary<string, string> VisibilityClassName;

        public List<string> PseudoWithoutStyles;
    }
}
