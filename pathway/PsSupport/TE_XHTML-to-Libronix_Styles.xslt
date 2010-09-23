<?xml version="1.0" encoding="UTF-8"?>
<!-- Create the Libronix stylesheet file from ? from TE . -->
<!-- Note: Use a font size greater than 9 pts for proper display as a Logos resource. -->

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml"
    xmlns:fn="http://www.w3.org/2005/xpath-functions"
    exclude-result-prefixes="xhtml">

    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

    <xsl:strip-space elements="*"/>
    
    <xsl:template match="xhtml:html">
		<xsl:element name="logos-resource-stylesheet">
			<xsl:element name="classes">
				<xsl:call-template name="createStyles"/>
			</xsl:element>
		</xsl:element> <!-- logos-resource-stylesheet -->
    </xsl:template>
    
    <!-- All of the elements for the main exported file that have the attribute "class" are paragraphs. The values for the "class" attribute include:
		Intro_List_Item1, Intro_Paragraph, Intro_Section_Head, Line1,
		Paragraph, Paragraph_Continuation, Parallel_Passage_Reference,
		pictureCaption, pictureCenter, Section_Head, Section_Head_Major,
		Section_Head_Minor, and Title_Main. -->
    <xsl:template name="createStyles">

		<!-- The CSS file specifies the style for ".Emphasis" as:
.Emphasis {
    font-family: "Times New Roman", serif;	/* default Serif font */
    font-style: italic;	/* cascaded environment */
    text-indent: 0pt;
    margin-left: 0pt;
} -->
		<xsl:element name="class">
			<xsl:attribute name="name">Emphasis</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-family: Times New Roman; font-style: italic; text-indent: 0pt; margin-left: 0pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for "Intro_List_Item1" as:
.Intro_List_Item1 {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 9pt;	/* cascaded environment */
    text-indent: -36pt;
    margin-left: 36pt;
} -->
		<xsl:element name="class">
			<xsl:attribute name="name">Intro_List_Item1</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-size: 12pt; font-family: Charis SIL; text-indent: -36pt; margin-left: 36pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for "Intro_Paragraph" as:
.Intro_Paragraph {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 9pt;	/* cascaded environment */
    text-indent: 0pt;
    margin-left: 0pt;
    line-height: 11pt;
} -->
		<xsl:element name="class">
			<xsl:attribute name="name">Intro_Paragraph</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-size: 12pt; font-family: Charis SIL; text-indent: 0pt; margin-left: 0pt; line-height: 11pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for ".Intro_Section_Head" as:
.Intro_Section_Head {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 8pt;	/* cascaded environment */
    font-weight: bold;	/* cascaded environment */
    font-style: normal;	/* cascaded environment */
    /*text-align: leading;*/
    text-align: left;
    text-indent: 0pt;
    margin-left: 0pt;
    line-height: 11pt;
} -->
		<xsl:element name="class">
			<xsl:attribute name="name">Intro_Section_Head</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-size: 11pt; font-family: Charis SIL; text-align: left; text-indent: 0pt; margin-left: 0pt; line-height: 11pt; font-weight: bold</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for ".Line1" as:
.Line1 {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 10pt;	/* cascaded environment */
    /*text-align: leading;*/
    text-align: left;
    text-indent: -36pt;
    margin-left: 36pt;
    padding-left: 0pt;
} -->
		<xsl:element name="class">
			<xsl:attribute name="name">Line1</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-size: 12pt; font-family: Charis SIL; text-align: left; text-indent: -36pt; margin-left: 36pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for ".Note_CrossHYPHENReference_Paragraph" as:
.Note_CrossHYPHENReference_Paragraph {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 8pt;	/* cascaded environment */
    text-indent: 0pt;
    margin-left: 0pt;
    display: inline;
    display: footnote;
    display: prince-footnote;
    position: footnote;
    list-style-position: inside;
} -->
		<xsl:element name="class">
			<xsl:attribute name="name">Note_CrossHYPHENReference_Paragraph</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-size: 11pt; font-family: Charis SIL; text-indent: 0pt; margin-left: 0pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for ".Note_General_Paragraph" as:
.Note_General_Paragraph {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 8pt;	/* cascaded environment */
    text-indent: 0pt;
    margin-left: 0pt;
    line-height: normal;
    display: inline;
    display: footnote;
    display: prince-footnote;
    position: footnote;
    list-style-position: inside;
} -->
		<xsl:element name="class">
			<xsl:attribute name="name">Note_General_Paragraph</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-size: 11pt; font-family: Charis SIL; text-indent: 0pt; margin-left: 0pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for ".Paragraph" as:
.Paragraph {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 10pt;	/* cascaded environment */
    text-indent: 12pt;
    margin-left: 0pt;
} -->
		<xsl:element name="class">
			<xsl:attribute name="name">Paragraph</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-size: 12pt; font-family: Charis SIL; text-indent: 12pt; margin-left: 0pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for ".Paragraph_Continuation" as:
.Paragraph_Continuation {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 10pt;	/* cascaded environment */
    text-indent: 0pt;
    margin-left: 0pt;
} -->
		<xsl:element name="class">
			<xsl:attribute name="name">Paragraph_Continuation</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-size: 12pt; font-family: Charis SIL; text-indent: 0pt; margin-left: 0pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for ".Parallel_Passage_Reference" as:
.Parallel_Passage_Reference {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 9pt;	/* cascaded environment */
    font-weight: normal;	/* cascaded environment */
    font-style: italic;	/* cascaded environment */
    text-indent: 0pt;
    margin-left: 0pt;
    padding-bottom: 4pt;
    padding-top: -4pt;
} -->
<!-- Note: Example styles from Logos Styles XML do not include padding-bottom and padding-top. -->
		<xsl:element name="class">
			<xsl:attribute name="name">Parallel_Passage_Reference</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-size: 12pt; font-family: Charis SIL; font-weight: normal; font-style: italic; text-indent: 0pt; margin-left: 0pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for ".pictureCaption" as:
.pictureCaption {
} -->
		<xsl:element name="class">
			<xsl:attribute name="name">pictureCaption</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>text-indent: 0pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for ".pictureCenter" as:
.pictureCenter {
    float: center;
    margin: 0pt 0pt 4pt 4pt;
    padding: 2pt;
    text-indent: 0pt;
    text-align: center;
} -->
<!-- Note: Example styles from Logos Styles XML do not include float and padding (except for cells). -->
		<xsl:element name="class">
			<xsl:attribute name="name">pictureCenter</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>text-indent: 0pt; text-align: center; margin-top: 0pt; margin-right: 0pt; margin-bottom: 4pt; margin-left: 4pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for ".Section_Head" as:
.Section_Head {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 9pt;	/* cascaded environment */
    font-weight: bold;	/* cascaded environment */
    font-style: normal;	/* cascaded environment */
    text-align: center;
    text-indent: 0pt;
    margin-left: 0pt;
    padding-bottom: 4pt;
    padding-top: 8pt;
} -->
<!-- Note: Example styles from Logos Styles XML do not include padding-bottom and padding-top. -->
		<xsl:element name="class">
			<xsl:attribute name="name">Section_Head</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-size: 14pt; font-family: Charis SIL; font-weight: bold; font-style: normal; text-align: center; text-indent: 0pt; margin-left: 0pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for ".Section_Head_Major" as:
.Section_Head_Major {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 9pt;	/* cascaded environment */
    font-weight: bold;	/* cascaded environment */
    font-style: normal;	/* cascaded environment */
    text-align: center;
    text-indent: 0pt;
    margin-left: 0pt;
} -->
		<xsl:element name="class">
			<xsl:attribute name="name">Section_Head_Major</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-size: 14pt; font-family: Charis SIL; font-weight: bold; font-style: normal; text-align: center; text-indent: 0pt; margin-left: 0pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for ".Section_Head_Minor" as:
.Section_Head_Minor {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 9pt;	/* cascaded environment */
    font-weight: normal;	/* cascaded environment */
    font-style: italic;	/* cascaded environment */
    text-indent: 0pt;
    margin-left: 0pt;
} -->
		<xsl:element name="class">
			<xsl:attribute name="name">Section_Head_Minor</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-size: 14pt; font-family: Charis SIL; font-weight: normal; font-style: italic; text-indent: 0pt; margin-left: 0pt</xsl:text>
			</xsl:attribute>
		</xsl:element>

		<!-- The CSS file specifies the style for ".Title_Main" as:
.Title_Main {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 20pt;	/* cascaded environment */
    font-weight: bold;	/* cascaded environment */
    font-style: normal;	/* cascaded environment */
    text-align: center;
    text-indent: 0pt;
    margin-left: 0pt;
    line-height: 24pt;
    padding-bottom: 12pt;
    padding-top: 36pt;
} -->
<!--
<class name="Title_Main" style="direction: ltr; font-class: BodyText; font-family: Charis SIL; font-size: 20pt; font-style: none; font-weight: bold; text-align: center;" />
 -->
<!-- Note: Example styles from Logos Styles XML do not include line-height, padding-bottom and padding-top. -->
		<xsl:element name="class">
			<xsl:attribute name="name">Title_Main</xsl:attribute>
			<xsl:attribute name="style">
				<xsl:text>font-size: 20pt; font-family: Charis SIL; font-weight: bold; font-style: none; text-align: center; text-indent: 0pt; margin-left: 0pt</xsl:text>
			</xsl:attribute>
		</xsl:element>
    </xsl:template>

<!-- Example classes from GNB-Styles.xml:
	<class name="Headingsbold" usage="3026" style="font-class: Headings; font-family: Arial; font-weight: bold; " />
	<class name="Headings10pt" usage="2790" style="font-class: Headings; font-family: Arial; font-size: 10pt; " />
	<class name="BodyTextnone" usage="293" style="font-class: BodyText; font-family: Times New Roman; font-weight: none; " />
	<class name="Headingsboldsmallcaps" usage="152" style="font-class: Headings; font-family: Arial; font-variant: small-caps; font-weight: bold; " />
	<class name="Headingsitalic" usage="103" style="font-class: Headings; font-family: Arial; font-style: italic; " />
	<class name="BodyTextnoneitalic" usage="71" style="font-class: BodyText; font-family: Times New Roman; font-style: italic; font-weight: none; " />
	<class name="Headings18ptbold" usage="66" style="font-class: Headings; font-family: Arial; font-size: 18pt; font-weight: bold; " />
	<class name="italic" usage="24" style="font-style: italic; " />
	<class name="none" usage="17" style="font-style: none; " />
	<class name="BodyTextnonesmallcaps" usage="2" style="font-class: BodyText; font-family: Times New Roman; font-variant: small-caps; font-weight: none; " />

	<class name="Normal" usage="17224" style="direction: ltr; font-class: BodyText; font-family: Times New Roman; font-size: 12pt; text-align: left; text-indent: 18pt; " />
	<class name="NormalPlain" usage="16322" style="text-indent: none; " />
	<class name="ParaL1080F-360" usage="13108" style="margin-left: 54pt; text-indent: -18pt; " />
	<class name="ParaL1080F-1080Ts1" usage="7810" style="margin-left: 54pt; tab-stops: left/18pt; text-indent: -54pt; " />
	<class name="ParaL1080F-720" usage="4400" style="margin-left: 54pt; text-indent: -36pt; " />
	<class name="ParaCT180B90" usage="4256" style="margin-bottom: 4.50pt; margin-top: 9pt; text-align: center; text-indent: none; " />
	<class name="ParaT180L1080F-1080Ts1" usage="2082" style="margin-left: 54pt; margin-top: 9pt; tab-stops: left/18pt; text-indent: -54pt; " />
	<class name="ParaCT180" usage="1272" style="margin-top: 9pt; text-align: center; text-indent: none; " />
	<class name="ParaCB90" usage="1256" style="margin-bottom: 4.50pt; text-align: center; text-indent: none; " />
	<class name="ParaL360F-360" usage="666" style="margin-left: 18pt; text-indent: -18pt; " />
	<class name="ParaL1080F-1080Ts1_2" usage="614" style="margin-left: 54pt; tab-stops: left/36pt; text-indent: -54pt; " />
	<class name="ParaC" usage="132" style="text-align: center; text-indent: none; " />
	<class name="ParaCT480B180" usage="132" style="margin-bottom: 9pt; margin-top: 24pt; text-align: center; text-indent: none; " />
	<class name="ParaR" usage="132" style="text-align: right; text-indent: none; " />
	<class name="ParaT180" usage="126" style="margin-top: 9pt; text-indent: none; " />
	<class name="ParaL720F-360Ts1" usage="118" style="margin-left: 36pt; tab-stops: left/36pt; text-indent: -18pt; " />
	<class name="ParaT180L1080F-720" usage="114" style="margin-left: 54pt; margin-top: 9pt; text-indent: -36pt; " />
	<class name="ParaL1800F-1440Ts1" usage="64" style="margin-left: 90pt; tab-stops: left/90pt; text-indent: -72pt; " />
	<class name="ParaL360F360" usage="52" style="margin-left: 18pt; " />
	<class name="ParaB180L1080F-360" usage="46" style="margin-bottom: 9pt; margin-left: 54pt; text-indent: -18pt; " />
	<class name="ParaB180L1080F-720" usage="44" style="margin-bottom: 9pt; margin-left: 54pt; text-indent: -36pt; " />
	<class name="ParaL2520F-2160Ts1" usage="42" style="margin-left: 126pt; tab-stops: left/108pt; text-indent: -108pt; " />
	<class name="ParaL2160F-1800Ts1" usage="25" style="margin-left: 108pt; tab-stops: left/108pt; text-indent: -90pt; " />

	<class name="row001" usage="45" />
	<class name="row002" usage="20" style="cell-padding: 5.40pt; " />
-->
    	
	<!-- From the CSS file (for Bughotu, in this case):
		The overall font family as: font-family: "Arial", sans-serif;   /* default Sans-Serif font */.
		The default direction as: direction: ltr; .

		Specific classes:
			<class name="Normal" style="direction: ltr; font-class: BodyText; font-family: Times New Roman; font-size: 12pt; text-align: left; text-indent: 18pt; " />
	-->
	<!--
Example styles from Logos Styles XML:
cell-padding: 5.40pt
direction: ltr
font-class: BodyText, Headings
font-family: Arial, Times New Roman
font-size: 10pt, 12pt, 18pt
font-style: italic, none
font-variant: small-caps
font-weight: bold, none
margin-bottom: 4.50pt, 9pt
margin-left: 18pt, 36pt, 54pt, 90pt, 108pt, 126pt
margin-top: 9pt, 24pt
tab-stops: left/18pt, left/36pt, left/90pt, left/108pt
text-align: center, left, right
text-indent: -108pt, -90pt, -72pt, -54pt, -36pt, -18pt, 18pt, none
-->
<!-- From the CSS file:
.REVERSE_SOLIDUSfASTERISK {
    font-family: "Charis SIL", serif;	/* cascaded environment */
    font-size: 10pt;	/* cascaded environment */
    text-indent: 0pt;
    margin-left: 0pt;
}
.Chapter_Number {
}
.columns {
    column-count: 2; -moz-column-count: 2;
    column-gap: .5cm; -moz-column-gap: .5cm;
}
.Inscription {
    font-family: "Times New Roman", serif;	/* default Serif font */
    text-indent: 0pt;
    margin-left: 0pt;
}
.Note_CrossHYPHENReference_Paragraph::footnote-call {
    color: purple;
    content: attr(title);
    font-size: 6pt;
    vertical-align: super;
    line-height: none;
}
.Note_CrossHYPHENReference_Paragraph::footnote-marker {
    font-size: 10pt;
    font-weight: bold;
    content: string(chapter) ':' string(verse) ' = ';
    /* content: string(footnoteMarker); font-size: 6pt; vertical-align: super; color: purple; */
    text-align: left;
}
.Note_General_Paragraph::footnote-call {
    color: purple;
    content: attr(title);
    font-size: 6pt;
    vertical-align: super;
    line-height: none;
}
.Note_General_Paragraph::footnote-marker {
    font-size: 10pt;
    font-weight: bold;
    content: string(chapter) ':' string(verse) ' = ';
    /* content: string(footnoteMarker); font-size: 6pt; vertical-align: super; color: purple; */
    text-align: left;
}
.picture {
    height: 1.0in;
}
.scrBody {
}
.scrBook {
    column-count: 1;
    clear: both;
}
.scrBookName {
    string-set: bookname content();
    display: none;
}
.scrFootnoteMarker {
}
.scrIntroSection {
    text-align: left;
}
.scrSection {
    text-align: left;
}
.Verse_Number {
} -->


	<!-- Default element template. -->
	<xsl:template match="*">
		<xsl:comment>Warning :: The element "<xsl:value-of select='name()'/>" with class "<xsl:value-of select='@class'/>", child of "<xsl:value-of select="name(..)"/>" with class "<xsl:value-of select='.././@class'/>", has no matching template.</xsl:comment>
	</xsl:template>
	<!-- Default attribute template. -->
	<xsl:template match="@*">
		<xsl:comment>Warning :: The attribute "<xsl:value-of select="name()"/>" for element "<xsl:value-of select="name(..)"/>"  has no matching template.</xsl:comment>
	</xsl:template>

</xsl:stylesheet>