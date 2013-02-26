// --------------------------------------------------------------------------------------------
// <copyright file="Dic4MidProperties.cs" from='2013' to='2013' company='SIL International'>
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class Dic4MidProperties
    {
        protected StreamWriter Sw { get; set; }

        #region Properties
        public string InfoText { get; set; }
        public string DictionaryAbbreviation { get; set; }
        public int NumberOfAvailableLanguages { get; set; }
        public int Language1NumberOfContentDeclarations { get; set; }
        #endregion Properties

        #region indexed by language
        public readonly Dictionary<int, string> DisplayText = new Dictionary<int, string>();
        public readonly Dictionary<int, string> FilePostfix = new Dictionary<int, string>();
        #endregion indexed by language

        #region indexed by content (style number)
        public Dic4MidStyle Styles = new Dic4MidStyle();
        #endregion indexed by content (style number)

        public Dic4MidProperties(PublicationInformation projInfo)
        {
            var myPath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            Debug.Assert(myPath != null);
            Sw = new StreamWriter(Path.Combine(myPath, "DictionaryForMIDs.properties"));
            DictionaryAbbreviation = "SIL";
            NumberOfAvailableLanguages = 2;
            Language1NumberOfContentDeclarations = 1;
        }

        public void SetLanguage(int num, string iso, string name)
        {
            DisplayText[num] = name;
            FilePostfix[num] = iso;
        }

        public void Write()
        {
            Sw.WriteLine(string.Format("infoText:{0}", InfoText));
            Sw.WriteLine();
            Sw.WriteLine(string.Format("dictionaryAbbreviation:{0}", DictionaryAbbreviation));
            Sw.WriteLine("numberOfAvailableLanguages:2");
            Sw.WriteLine();
            Sw.WriteLine(@"indexFileSeparationCharacter:'\t'");
            Sw.WriteLine(@"searchListFileSeparationCharacter:'\t'");
            Sw.WriteLine(@"dictionaryFileSeparationCharacter:'\t'");
            Sw.WriteLine(@"dictionaryGenerationSeparatorCharacter:'\t'");
            Sw.WriteLine();
            Sw.WriteLine("dictionaryGenerationInputCharEncoding:UTF-8");
            Sw.WriteLine("indexCharEncoding:UTF-8");
            Sw.WriteLine("searchListCharEncoding:UTF-8");
            Sw.WriteLine("dictionaryCharEncoding:UTF-8");
            Sw.WriteLine();
            Sw.WriteLine("dictionaryGenerationOmitParFromIndex:true");
            Sw.WriteLine();
            Sw.WriteLine(string.Format("language1DisplayText:{0}", DisplayText[1]));
            Sw.WriteLine(string.Format("language1FilePostfix:{0}", FilePostfix[1]));
            Sw.WriteLine();
            Sw.WriteLine("language1IsSearchable:true");
            Sw.WriteLine("language1GenerateIndex:true");
            Sw.WriteLine("language1HasSeparateDictionaryFile:false");
            Sw.WriteLine();
            Sw.WriteLine(string.Format("language2DisplayText:{0}", DisplayText[2]));
            Sw.WriteLine(string.Format("language2FilePostfix:{0}", FilePostfix[2]));
            Sw.WriteLine();
            Sw.WriteLine("language2IsSearchable:true");
            Sw.WriteLine("language2GenerateIndex:true");
            Sw.WriteLine("language2HasSeparateDictionaryFile:false");
            Sw.WriteLine();
            Sw.WriteLine(string.Format("language1NumberOfContentDeclarations:{0}", Language1NumberOfContentDeclarations));
            Sw.WriteLine("language2NumberOfContentDeclarations:1");
            for (int n = 1; n <= Language1NumberOfContentDeclarations; n++)
            {
                Sw.WriteLine(string.Format("language1Content{0:D2}DisplayText:{1}", n, Styles.DisplayText(n)));
                Sw.WriteLine(string.Format("language1Content{0:D2}FontColour:{1}", n, Styles.FontColor(n)));
                Sw.WriteLine(string.Format("language1Content{0:D2}FontStyle:{1}", n, Styles.ContentStyle(n)));
                Sw.WriteLine(string.Format("language1Content{0:D2}DisplaySelectable:true", n));
            }
            Sw.WriteLine();
            Sw.WriteLine("language2Content01DisplayText:Gloss");
            Sw.WriteLine("language2Content01FontColour:128,0,0");
            Sw.WriteLine("language2Content01FontStyle:plain");
            Sw.WriteLine("language2Content01DisplaySelectable:true");
        }

        public void Close()
        {
            Sw.Close();
        }
    }
}
