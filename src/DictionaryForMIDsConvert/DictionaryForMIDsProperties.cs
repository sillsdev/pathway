// --------------------------------------------------------------------------------------------
// <copyright file="DictionaryForMIDsProperties.cs" from='2013' to='2014' company='SIL International'>
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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    public class DictionaryForMidsProperties
    {
        private StreamWriter Sw { get; set; }

        #region Properties
        public string InfoText { private get; set; }
        private string DictionaryAbbreviation { get; set; }
        private int Language1NumberOfContentDeclarations { get; set; }
        #endregion Properties

        #region indexed by language

        private readonly Dictionary<int, string> _displayText = new Dictionary<int, string>();
        private readonly Dictionary<int, string> _filePostfix = new Dictionary<int, string>();
        #endregion indexed by language

        #region indexed by content (style number)

        private readonly DictionaryForMIDsStyle _styles;
        #endregion indexed by content (style number)

        public DictionaryForMidsProperties(PublicationInformation projInfo, DictionaryForMIDsStyle contentStyles)
        {
            var myPath = Path.GetDirectoryName(projInfo.DefaultXhtmlFileWithPath);
            Debug.Assert(myPath != null);
            _styles = contentStyles;
            Sw = new StreamWriter(Common.PathCombine(myPath, "DictionaryForMIDs.properties"));
            DictionaryAbbreviation = "SIL";
            Language1NumberOfContentDeclarations = _styles.NumStyles;
        }

        public void SetLanguage(int num, string iso, string name)
        {
            _displayText[num] = name;
            _filePostfix[num] = iso;
        }

        public void Write()
        {
            Sw.WriteLine("infoText:{0}", InfoText);
            Sw.WriteLine();
            Sw.WriteLine("dictionaryAbbreviation:{0}", DictionaryAbbreviation);
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
            Sw.WriteLine("language1DisplayText:{0}", _displayText[1]);
            Sw.WriteLine("language1FilePostfix:{0}", _filePostfix[1]);
            Sw.WriteLine();
            Sw.WriteLine("language1IsSearchable:true");
            Sw.WriteLine("language1GenerateIndex:true");
            Sw.WriteLine("language1HasSeparateDictionaryFile:false");
            Sw.WriteLine();
            Sw.WriteLine("language2DisplayText:{0}", _displayText[2]);
            Sw.WriteLine("language2FilePostfix:{0}", _filePostfix[2]);
            Sw.WriteLine();
            Sw.WriteLine("language2IsSearchable:true");
            Sw.WriteLine("language2GenerateIndex:true");
            Sw.WriteLine("language2HasSeparateDictionaryFile:false");
            Sw.WriteLine();
            Sw.WriteLine("language1NumberOfContentDeclarations:{0}", Language1NumberOfContentDeclarations);
            Sw.WriteLine("language2NumberOfContentDeclarations:1");
            for (int n = 1; n <= Language1NumberOfContentDeclarations; n++)
            {
                Sw.WriteLine("language1Content{0:D2}DisplayText:{1}", n, _styles.DisplayText(n));
                Sw.WriteLine("language1Content{0:D2}FontColour:{1}", n, _styles.FontColor(n));
                Sw.WriteLine("language1Content{0:D2}FontStyle:{1}", n, _styles.ContentStyle(n));
                Sw.WriteLine("language1Content{0:D2}DisplaySelectable:true", n);
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
