@import "%(link)s";

@media %(ddlMediaRules)<%(ddlMediaRules)s>
{
  .common
  {
    %(txtMediaDataFontSize)<font-size: %(txtMediaDataFontSize)s;>
    %(ddlMediaHeaderFontStyle)<font-style: %(ddlMediaHeaderFontStyle)s;>
    %(ddlMediaSize)<size: %(ddlMediaSize)s;>
    %(ddlMediaPosition)<position: %(ddlMediaPosition)s;>
   }
}

@page {
    %(txtPageWidth)<size: %(txtPageWidth)s %(txtPageHeight)s;>
    %(txtPageTop)<margin: %(txtPageTop)s %(txtPageOutside)s %(txtPageBottom)s %(txtPageInside)s;>
    /*%(chkPageCropMark)<marks: crop;>*/
    %(PageCropMark)<marks:%(PageCropMark)s;>
    %(ddlPagePaperSize)<-ps-paper-size:%(ddlPagePaperSize)s;>
    %(txtHeadingsString)<-ps-heading-string:"%(txtHeadingsString)s";>
   %(chkHeadingsHomonymNumber)<-ps-HeadingsHomonymNumber:%(chkHeadingsHomonymNumber)s;>
   %(chkHeadingsGuideWords)< -ps-HeadingsGuideWords:%(chkHeadingsGuideWords)s; >

/* Code for Document and Preparation Panel

    -ps-section1:Cover "%(Cover)s";
    -ps-section2:Title "%(Title)s";
    -ps-section3:Rights "%(Rights)s";
    -ps-section4:Introduction "%(Introduction)s";
    -ps-section5:Blank none;
    -ps-section6:List "%(List)s";
    -ps-section7:Orthography "%(Orthography)s";
    -ps-section8:Phonology "%(Phonology)s";
    -ps-section9:Grammar "%(Grammar)s";
    -ps-section10:Main main;
    -ps-section11:Index index;
    -ps-section12:Bibliography "%(Bibliography)s";

    -ps-step1:Filter Entries "%(Filter Entries)s";
    -ps-step2:Filter Senses "%(Filter Senses)s";
    -ps-step3:Filter Media "%(Filter Media)s";
    -ps-step4:Filter Text "%(Filter Text)s";
    -ps-step5:Make Minor Entries "%(Make Minor Entries)s";
    -ps-step6:Make Homographs "%(Make Homographs)s";
    -ps-step7:Sort Entries "%(Sort Entries)s";
    -ps-step8:Make Letter Headers "%(Make Letter Headers)s";
    -ps-step9:Make Grammatical Categories "%(Make Grammatical Categories)s";
    -ps-step10:Make Cross References "%(Make Cross References)s";
    -ps-step11:Order Writing Systems "%(Order Writing Systems)s";
    -ps-step12:Order Fields "%(Order Fields)s";
*/

    %(ddlHeadingsPageNumberLocation)s-left {
        content: string(guideword, first);
        %(txtHeadingsRunningFontSize)<font-size: %(txtHeadingsRunningFontSize)s; >
    }



    %(ddlHeadingsPageNumberLocation)s-center {
        content: counter(page);
        %(txtHeadingsRunningFontSize)<font-size: %(txtHeadingsRunningFontSize)s; >
    }



    %(ddlHeadingsPageNumberLocation)s-right {
        content: string(guideword, last);
        %(txtHeadingsRunningFontSize)<font-size: %(txtHeadingsRunningFontSize)s; >
    }


}
@page :first 
{
    %(ddlHeadingsPageNumberLocation)s-left 
    { 
       content: ''; 
       %(txtHeadingsRunningFontSize)<font-size: %(txtHeadingsRunningFontSize)s;>
    }
    %(ddlHeadingsPageNumberLocation)s-center 
    { 
       content: '';
       %(txtHeadingsRunningFontSize)<font-size: %(txtHeadingsRunningFontSize)s;>
    }
    %(ddlHeadingsPageNumberLocation)s-right 
    { 
      content: ''; 
      %(txtHeadingsRunningFontSize)<font-size: %(txtHeadingsRunningFontSize)s;>
    }
}

.entry {
%(txtEntriesHangingIndent)<text-indent: -%(txtEntriesHangingIndent)s; >
%(txtEntriesFontSize)<font-size: %(txtEntriesFontSize)s; >
%(txtEntriesHangingIndent)<margin-left: %(txtEntriesHangingIndent)s; // text-indent = -margin-left >
%(txtEntriesLineSpacing)<line-height: %(txtEntriesLineSpacing)s; >
%(txtEntriesSpaceBefore)<padding-top: %(txtEntriesSpaceBefore)s; >
%(txtEntriesSpaceAfter)<padding-bottom: %(txtEntriesSpaceAfter)s; >
}


%(chkHeadingsHomonymNumber)<
.xhomographnumber {

}
>


.dicBody {
}

.letHead {

%(txtHeadingsFontName)<font-family: "%(txtHeadingsFontName)s", serif;  >
%(txtHeadingsLetterFontSize)<font-size: %(txtHeadingsLetterFontSize)s; >
%(ddlHeadingsLetterFontStyle)<font-style: %(ddlHeadingsLetterFontStyle)s;  >
%(txtHeadingsLetterLineSpacing)<line-height: %(txtHeadingsLetterLineSpacing)s; >
%(txtHeadingsLetterSpaceBefore)<padding-top: %(txtHeadingsLetterSpaceBefore)s; >
%(txtHeadingsLetterSpaceAfter)<padding-bottom: %(txtHeadingsLetterSpaceAfter)s; >
%(chkHeadingsLetterDividerLine)<border-bottom: %(chkHeadingsLetterDividerLine)s;>
%(ddlHeadingsFontWeight)<font-weight: %(ddlHeadingsFontWeight)s; >
}



.letter { 
%(chkHeadingsLetterCentered)<text-align: %(chkHeadingsLetterCentered)s; >
%(ddlHeadingsFontWeight)<font-weight: %(ddlHeadingsFontWeight)s; >
%(txtHeadingsLetterFontSize)<font-size: %(txtHeadingsLetterFontSize)s; >
%(txtHeadingsFontName)<font-family: %(txtHeadingsFontName)s, serif;/* default Serif font */  >

}


.letData {

    %(chkPageVerticalRule)<column-rule: %(chkPageVerticalRule)s;>
    %(ddlPageColumn)<column-count: %(ddlPageColumn)s; -moz-column-count: %(ddlPageColumn)s;>
    %(txtPageGutterWidth)<column-gap: %(txtPageGutterWidth)s; -moz-column-gap: %(txtPageGutterWidth)s;>
    column-fill: balance;
}

.revAppendix { }
.revHeader {
    font-family: "Charis SIL", serif;
    font-size: 18pt;
    text-align: center;
    font-weight: bold;
}
/*
.revData {
	%(ddlIndexesColumns)<    column-count: %(ddlIndexesColumns)s;    -moz-column-count: %(ddlIndexesColumns)s;>
	%(txtIndexesGutterWidth)<    column-gap: %(txtIndexesGutterWidth)s;    -moz-column-gap: %(txtIndexesGutterWidth)s;>
    		column-fill: balance;
    	%(chkIndexesVerticalRuleInGutter)<column-rule: %(chkIndexesVerticalRuleInGutter)s;>
    		text-align: left;
}
*/
.revData {
	%(ddlIndexesColumns)<column-count: %(ddlIndexesColumns)s; -moz-column-count: %(ddlIndexesColumns)s;
    		column-fill: balance;
    		text-align: left;>
	%(txtIndexesGutterWidth)<column-gap: %(txtIndexesGutterWidth)s; -moz-column-gap: %(txtIndexesGutterWidth)s;>
	%(chkIndexesVerticalRuleInGutter)<column-rule: %(chkIndexesVerticalRuleInGutter)s;>
	}

.revEntry {
     %(txtEntriesHangingIndent)<text-indent: -%(txtEntriesHangingIndent)s; >
     %(txtEntriesHangingIndent)<margin-left: %(txtEntriesHangingIndent)s; // text-indent = -margin-left >
     %(txtEntriesFontSize)<font-size: %(txtEntriesFontSize)s; >
     font-family: "Charis SIL", serif;
 %(txtEntriesLineSpacing)<line-height: %(txtEntriesLineSpacing)s; >
 %(txtEntriesSpaceBefore)<padding-top: %(txtEntriesSpaceBefore)s; >
 %(txtEntriesSpaceAfter)<padding-bottom: %(txtEntriesSpaceAfter)s; >
}

.revSense { }
.revhomographnumber { 
    vertical-align: sub;
}
.revsensenumber { }

.sense {
    display: %(SensesSensesParagraph)s;
    text-indent: -10pt;
    margin-left: 10pt;
    padding-bottom: 1pt;
    font-size: 10pt;    /* inherited */
    font-weight: normal;
    line-height: 12pt;
}

.xsensenumber :after { %(txtSensesPunctuation)< content: "%(txtSensesPunctuation)s";> }

.xsensenumber:before {
     visibility: visible;
     font-size: 8pt;
     %(fontName)<font-family: "%(fontName)s";>
     %(txtSensesSymbols)<content:"%(txtSensesSymbols)s";>
}

.xsensenumber {
    visibility: %(xSenseVisible)s;
    %(xSenseVisibleFont)<font-size:%(xSenseVisibleFont)s;>
}  


%(txtIndexesSenseSeperator)<
.revSense + .revSense:before
{
content:"%(txtIndexesSenseSeperator)s";
}
>

.unknown
{
/* Text Tab */
%(txtDocumentBrowse)< Propery: %(txtDocumentBrowse)s; >
%(txtPreparationBrowse)< Propery: %(txtPreparationBrowse)s; >


/* Heading Tab */
%(chkHeadingsSectionCentered)<text-align: center;>
%(txtHeadingsMainTitleFontSize)< Propery: %(txtHeadingsMainTitleFontSize)s; >
%(txtHeadingsSectionFontSize)< Propery: %(txtHeadingsSectionFontSize)s; >



/* Entry Tab */

%(chkEntriesUseGloss)< Propery: %(chkEntriesUseGloss)s; >

/* field Tab */
%(ddlFieldsBasis)< Propery: %(ddlFieldsBasis)s; >
%(txtFieldsAfter)< Propery: %(txtFieldsAfter)s; >
%(txtFieldsBetween)< Propery: %(txtFieldsBetween)s; >
%(txtFieldsBefore)< Propery: %(txtFieldsBefore)s; >
%(ddlFieldsFieldStyle)< Propery: %(ddlFieldsFieldStyle)s; >
%(tbnFieldsStyles)< Propery: %(tbnFieldsStyles)s; >

/* Text Tab */
%(ddlTextTagFontName)< Propery: %(ddlTextTagFontName)s; >
%(ddlTextHyphenationLanguage)< Propery: %(ddlTextHyphenationLanguage)s; >
%(txtTextAutomaticVerticalJustification)< Propery: %(txtTextAutomaticVerticalJustification)s; >
%(txtTextKerning)< Propery: %(txtTextKerning)s; >
%(txtTextBetweenLettersLetterMaximum)< Propery: %(txtTextBetweenLettersLetterMaximum)s; >
%(txtTextBetweenLettersLetterMinimum)< Propery: %(txtTextBetweenLettersLetterMinimum)s; >
%(txtTextBetweenLettersLetterDesired)< Propery: %(txtTextBetweenLettersLetterDesired)s; >
%(txtTextBetweenWordsMaximum)< Propery: %(txtTextBetweenWordsMaximum)s; >
%(txtTextBetweenWordsMinimum)< Propery: %(txtTextBetweenWordsMinimum)s; >
%(txtTextBetweenWordsDesired)< Propery: %(txtTextBetweenWordsDesired)s; >
%(txtTextJustifiedParagraphs)< Propery: %(txtTextJustifiedParagraphs)s; >
%(ddlTextPreferredStyle)< Propery: %(ddlTextPreferredStyle)s; >
%(txtTextFontSize)< Propery: %(txtTextFontSize)s; >
%(txtTextTagFontColor)< Propery: %(txtTextTagFontColor)s; >
%(txtTextTagFontSize)< Propery: %(txtTextTagFontSize)s; >
%(chkTextIncludeLanguageTags)< Propery: %(chkTextIncludeLanguageTags)s; >
%(lblTextFontColor)< Propery: %(lblTextFontColor)s; >

/* Advance Tab */
%(ddlAdvancedComposer)< Propery: %(ddlAdvancedComposer)s; >
%(chkAdvancedOptionalMarginAlignment)< Propery: %(chkAdvancedOptionalMarginAlignment)s; >

}


%(chkTextIncludeLanguageTags)<
%(IncludeLanguageProperty)s
>