@import "%(link)s";

@page 
{
    	size: %(pageWidth)s %(pageHeight)s;
    	%(txtBasicsTop)<margin: %(txtBasicsTop)s %(txtBasicsOutside)s %(txtBasicsBottom)s %(txtBasicsInside)s;>
    	%(PageCropMark)<marks:%(PageCropMark)s;>
    	counter-increment: page;
    	%(ddlPagePaperSize)<-ps-paper-size:%(ddlBasicsPaperSize)s;>
    	%(txtHeadingsString)<-ps-heading-string:"%(txtHeadingsString)s";>
	%(ddlChapterPositionChapterNumbers)<-ps-positionchapternumbers-string: "%(ddlChapterPositionChapterNumbers)s";>
	%(txtChapterChapterString)<-ps-chapterstring-string: "%(txtChapterChapterString)s";>
	%(txtChapterPositionString)<-ps-chapterpositionstring-string: "%(txtChapterPositionString)s";>
 	-ps-IncludeVersenumber-string: "%(chkChapterIncludeVerseNumber)s";
	%(ddlOtherTableHeadersFontStyle)<-ps-TableHeadersFontStyle-string:"%(ddlOtherTableHeadersFontStyle)s";>
	-ps-ReferenceFormat-string:"%(ddlOtherReferenceFormat)s";
        -ps-BasicsVerticalRuleinGutter:"%(isVerticalRule)s";

	%(txtOtherNonConsecutiveReferenceSeparator)<-ps-NonConsecutiveReferenceSeparator-string:"%(txtOtherNonConsecutiveReferenceSeparator)s";>



	/* Code for Document Panel
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

        %(pageReference)< %(pageReference)s>

    	
	@footnotes 
	{
  		border-top: thin solid black;
  		padding: 0.3em 0;
  		margin-top: 0.6em;
  		margin-left: 2pi;
	}

}

@page :first {
    @top-center {
        content: '';
    }
    @bottom-right {
      content: '';
    }
    @bottom-center {
        content: counter(page);
    }
}

%(mirroredReference)< %(mirroredReference)s>

.columns
{
	%(ddlBasicsColumns)<column-count: %(ddlBasicsColumns)s; -moz-column-count: %(ddlBasicsColumns)s;>
	%(txtBasicsGutterWidth)<column-gap: %(txtBasicsGutterWidth)s; -moz-column-gap: %(txtBasicsGutterWidth)s;>
        %(chkBasicsVerticalRule)<column-rule: solid 1pt #aa0000;
	column-fill: balance;
	text-align: left;>
}

.Chapter_Number:before 
{
	%(txtChapterChapterNumbers)<content: %(txtChapterChapterNumbers)s;>
    	font-size: 14pt;
}
.Chapter_Number 
{
    	font-size: 200%;
    	%(ddlBasicsFontName)<font-family:%(ddlBasicsFontName)s;>
	%(chkChapterBold)<font-weight:%(chkChapterBold)s;>
    	%(chkChapterRaised)<vertical-align: %(chkChapterRaised)s;>
    	float: left;
    	string-set: chapterx content();
}

.parallel_passage_reference 
{
	%(txtMajorHeadingsSectionHeadingsReferenceFontSize)<font-size:%(txtMajorHeadingsSectionHeadingsReferenceFontSize)s;>
	%(ddlHeadingsMajorSectionHeadingsReferenceFontStyle)<font-style:%(ddlHeadingsMajorSectionHeadingsReferenceFontStyle)s;>
}

.Title_Main 
{
	%(txtHeadingsMainTitleFontSize)<font-size:%(txtHeadingsMainTitleFontSize)s;>
}

.Chapter_Head 
{
	%(txtHeadingsMajorSectionHeadingsFontSize)<font-size:%(txtHeadingsMajorSectionHeadingsFontSize)s;>
    	%(ddlHeadingsMajorSectionHeadingsFontStyle)<font-style:%(ddlHeadingsMajorSectionHeadingsFontStyle)s;
	font-weight: bold;>
    	%(chkMajorHeadingsSectionHeadingsCentered)<text-align:%(chkMajorHeadingsSectionHeadingsCentered)s;>
    	%(txtHeadingsMajorSectionHeadingsLineSpacing)<line-height:%(txtHeadingsMajorSectionHeadingsLineSpacing)s;>
    	%(txtHeadingsMajorSectionHeadingsSpaceBefore)<padding-top:%(txtHeadingsMajorSectionHeadingsSpaceBefore)s;>
	%(txtHeadingsMajorSectionHeadingsSpaceAfter)<padding-bottom:%(txtHeadingsMajorSectionHeadingsSpaceAfter)s;>
}


.Section_Head {
    	%(txtHeadingsSectionHeadingsFontSize)<font-size:%(txtHeadingsSectionHeadingsFontSize)s;>
	%(chkHeadingsSectionHeadingsCentered)<text-align:%(chkHeadingsSectionHeadingsCentered)s;>
    	%(ddlHeadingsSectionHeadingsFontStyle)<font-style:%(ddlHeadingsSectionHeadingsFontStyle)s;
	font-weight: bold;>
    	%(txtHeadingsSectionHeadingsLineSpacing)<line-height:%(txtHeadingsSectionHeadingsLineSpacing)s;>
	%(txtHeadingsSectionHeadingsSpaceBefore)<padding-top:%(txtHeadingsSectionHeadingsSpaceBefore)s;>
	%(txtHeadingsSectionHeadingsSpaceAfter)<padding-bottom:%(txtHeadingsSectionHeadingsSpaceAfter)s;>
}

.Section_Head_Minor
{
	%(txtHeadingsNewSectionHeadingsFontSize)<font-size:%(txtHeadingsNewSectionHeadingsFontSize)s;>
	%(chkHeadingsNewSectionHeadingsCentered)<text-align:%(chkHeadingsNewSectionHeadingsCentered)s;>
	%(ddlHeadingsNewSectionHeadingsFontStyle)<font-style:%(ddlHeadingsNewSectionHeadingsFontStyle)s;
	font-weight: bold;>
	%(txtHeadingsNewSectionHeadingsLineSpacing)<line-height:%(txtHeadingsNewSectionHeadingsLineSpacing)s;>
	%(txtHeadingsNewSectionHeadingsSpaceBefore)<padding-top:%(txtHeadingsNewSectionHeadingsSpaceBefore)s;>
	%(txtHeadingsNewSectionHeadingsSpaceAfter)<padding-bottom:%(txtHeadingsNewSectionHeadingsSpaceAfter)s;>
}

.Paragraph 
{
	%(ddlBasicsFontName)<font-family:"%(ddlBasicsFontName)s";>
	%(ddlBasicsFontStyle)<font-style:%(ddlBasicsFontStyle)s;>
	%(txtBasicsFontSize)<font-size:%(txtBasicsFontSize)s;>
	%(txtBasicsLineSpacing)<line-height:%(txtBasicsLineSpacing)s;>
	text-align:%(chkBasicsJustifyParagraphs)s;
        %(txtBasicsLineSpacing)<-ps-fixed-line-height:%(txtBasicsLineSpacing)s;>


}

.Paragraph_continuation 
{
	%(ddlBasicsFontName)<font-family:"%(ddlBasicsFontName)s";>
	%(ddlBasicsFontStyle)<font-style:%(ddlBasicsFontStyle)s;>
	%(txtBasicsFontSize)<font-size:%(txtBasicsFontSize)s;>
	%(txtBasicsLineSpacing)<line-height:%(txtBasicsLineSpacing)s;>
	text-align:%(chkBasicsJustifyParagraphs)s;
	%(txtBasicsLineSpacing)<-ps-fixed-line-height:%(txtBasicsLineSpacing)s;>

	%(txtTextSpacingBetweenWordsDesired)<word-spacing:%(txtTextSpacingBetweenWordsDesired)s;>
	%(txtTextSpacingBetweenLettersLetterDesired)<letter-spacing:%(txtTextSpacingBetweenLettersLetterDesired)s;>
}

.Intro_Paragraph
{
        %(ddlBasicsFontName)<font-family:"%(ddlBasicsFontName)s";>
	%(ddlBasicsFontStyle)<font-style:%(ddlBasicsFontStyle)s;>
	%(txtBasicsFontSize)<font-size:%(txtBasicsFontSize)s;>
	%(txtBasicsLineSpacing)<line-height:%(txtBasicsLineSpacing)s;>
	text-align:%(chkBasicsJustifyParagraphs)s;
	%(txtBasicsLineSpacing)<-ps-fixed-line-height:%(txtBasicsLineSpacing)s;>
}

.Line1 {
        %(ddlBasicsFontName)<font-family:"%(ddlBasicsFontName)s";>
	%(ddlBasicsFontStyle)<font-style:%(ddlBasicsFontStyle)s;>
	%(txtBasicsFontSize)<font-size:%(txtBasicsFontSize)s;>
	%(txtBasicsLineSpacing)<line-height:%(txtBasicsLineSpacing)s;>
	text-align:%(chkBasicsJustifyParagraphs)s;
	%(txtBasicsLineSpacing)<-ps-fixed-line-height:%(txtBasicsLineSpacing)s;>
}

.Line2 {
        %(ddlBasicsFontName)<font-family:"%(ddlBasicsFontName)s";>
	%(ddlBasicsFontStyle)<font-style:%(ddlBasicsFontStyle)s;>
	%(txtBasicsFontSize)<font-size:%(txtBasicsFontSize)s;>
	%(txtBasicsLineSpacing)<line-height:%(txtBasicsLineSpacing)s;>
	text-align:%(chkBasicsJustifyParagraphs)s;
	%(txtBasicsLineSpacing)<-ps-fixed-line-height:%(txtBasicsLineSpacing)s;>
}

.Line3 {
        %(ddlBasicsFontName)<font-family:"%(ddlBasicsFontName)s";>
	%(ddlBasicsFontStyle)<font-style:%(ddlBasicsFontStyle)s;>
	%(txtBasicsFontSize)<font-size:%(txtBasicsFontSize)s;>
	%(txtBasicsLineSpacing)<line-height:%(txtBasicsLineSpacing)s;>
	text-align:%(chkBasicsJustifyParagraphs)s;
	%(txtBasicsLineSpacing)<-ps-fixed-line-height:%(txtBasicsLineSpacing)s;>
}

.footnote 
{
	%(txtFootnotesFontSizeforNotes)<font-size:%(txtFootnotesFontSizeforNotes)s;>
  	%(txtFootnotesLineSpacingforNotes)<line-height:%(txtFootnotesLineSpacingforNotes)s;>
	%(chkFootnotesReferenceInFootnoteInside)<list-style-position:%(chkFootnotesReferenceInFootnoteInside)s;>
	%(ddlFootnotesReferenceInFootnoteFontStyle)<font-style:%(ddlFootnotesReferenceInFootnoteFontStyle)s;>
}

.footnote::footnote-call 
{
	%(ddlFootnotesFootnoteCallerFontStyle)<font-style:%(ddlFootnotesFootnoteCallerFontStyle)s;>	
}

.crossReference::footnote-call 
{
	%(ddlFootnotesCrossReferenceCallerFontStyle)<font-style:%(ddlFootnotesCrossReferenceCallerFontStyle)s;>
}
