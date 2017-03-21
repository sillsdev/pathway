<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no"
	 doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"/>
	<xsl:param name="ws" select="'es'"/>
	<xsl:param name="userWs" select="'en'"/>
	<xsl:param name="dateTime" select="'4-June-2010'"/>
	<xsl:param name="user" select="'Lothers'"/>
	<xsl:param name="projName" select="'PROJNAME'"/>
	<xsl:param name="stylesheet" select="'usfm.sty'"/>	<!-- Paratext stylesheet name -->
	<xsl:param name="figurePath"/> <!-- Path to figures folder with a final directory separator character -->
	<xsl:param name="altFigurePath"/> <!-- Alternate path to figures folder with a final directory separator character -->
	<xsl:param name="fontName" select="'Charis SIL'"/>	<!-- Font used for the vernacular in the Paratext project -->
	<xsl:param name="fontSize" select="12"/>	<!-- Font size for the vernacular in the Paratext project -->
  <xsl:param name="langInfo" >en:English</xsl:param> <!-- langauge and script information in Paratext project -->

	<!-- The templates matching * and @* match and copy unhandled elements/attributes. -->
	<xsl:template match="*">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="@*">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match="/">
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="usfm|usx">
		<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="utf-8">
			<head>
				<title/>
				<link rel="stylesheet" href="{$projName}.css" type="text/css"/>
        <link rel="schema.DCTERMS" href="http://purl.org/dc/terms/" />
        <link rel="schema.DC" href="http://purl.org/dc/elements/1.1/" />
        <meta name="description" content="{$projName} exported by {$user} on {$dateTime}"/>
				<meta name="filename" content="{$projName}.xhtml"/>
				<meta name="stylesheet" content="{$stylesheet}"/>
				<meta name="fontName" content="{$fontName}"/>
				<meta name="fontSize" content="{$fontSize}"/>
        <meta name="dc.language" content="{$langInfo}" scheme="DCTERMS.RFC5646"/>
			</head>
			<body class="scrBody">
				<xsl:apply-templates/>
			</body>
		</html>
	</xsl:template>

	<!-- Define the book. -->
	<xsl:template match="book">
		<xsl:variable name="bookCode">
			<xsl:choose>
				<xsl:when test="@code">
					<xsl:value-of select="@code"/>
				</xsl:when>
				<xsl:when test="@id">
					<!-- Support old format for USX -->
					<xsl:value-of select="@id"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="bookInToc" select="normalize-space(para[@style='toc2'])"/>
		<xsl:variable name="bookHeading" select="normalize-space(para[@style='h'])"/>
		<xsl:variable name="bookTitle" select="normalize-space(para[@style='mt'])"/>
		<xsl:variable name="bookTitle1" select="normalize-space(para[@style='mt1'])"/>

		<div class="scrBook" xmlns="http://www.w3.org/1999/xhtml">
			<span class="scrBookName" lang="{$ws}">
				<!-- Find the name of the book in this fallback sequence: (1) book name specified in toc2 
					(2) book name in the heading, (3) main title, (4) three-letter book code -->
				<xsl:choose>
					<xsl:when test="string-length($bookInToc) != 0">
						<xsl:value-of select="$bookInToc"/>
					</xsl:when>
					<xsl:when test="string-length($bookHeading) != 0">
						<xsl:value-of select="$bookHeading"/>
					</xsl:when>
					<xsl:when test="string-length($bookTitle) != 0">
						<xsl:value-of select="$bookTitle"/>
					</xsl:when>
					<xsl:when test="string-length($bookTitle1) != 0">
						<xsl:value-of select="$bookTitle1"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$bookCode"/>
					</xsl:otherwise>
				</xsl:choose>
			</span>
			<span class="scrBookCode" lang="{$ws}"><xsl:value-of select="$bookCode"/></span>
			<div class = "Title_Main" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:choose>
					<xsl:when test="para[@style='mt' or @style='mt1']">
						<xsl:apply-templates select="para[@style='mt' or @style='mt1']" mode="title"/>
					</xsl:when>
					<xsl:otherwise>
						<span lang="{$ws}"/>
					</xsl:otherwise>
				</xsl:choose>
			</div>
			
			<xsl:apply-templates/>
		</div>
	</xsl:template>

	<!-- Convert the title to a form understood by Pathway -->
	<xsl:template match="para" mode="title">
		<xsl:apply-templates select="preceding-sibling::*[1][self::para][@style='mt2' or @style='mt3' or @style='mt4']" 
			mode="preceding-sibling-title"/>
		<!-- <span lang="{$ws}"  xmlns="http://www.w3.org/1999/xhtml"> -->
		<xsl:apply-templates/>
		<!-- </span> -->
		<xsl:apply-templates select="following-sibling::*[1][self::para][@style='mt2' or @style='mt3' or @style='mt4']" 
			mode="following-sibling-title"/>
	</xsl:template>
	
	<!-- Include any title paragraphs preceding the main title -->
	<xsl:template match="para" mode="preceding-sibling-title">
		<xsl:if test="count(preceding-sibling::*[1][@style='mt' or @style='mt1']) = 0">
			<xsl:apply-templates select="preceding-sibling::*[1][self::para][@style='mt2' or @style='mt3' or @style='mt4']" 
				mode="preceding-sibling-title"/>
			<xsl:call-template name="span-title"/>
		</xsl:if>
	</xsl:template>
	
	<!-- Include any title paragraphs following the main title -->
	<xsl:template match="para" mode="following-sibling-title">
		<xsl:call-template name="span-title"/>
		<xsl:apply-templates select="following-sibling::*[1][self::para][@style='mt2' or @style='mt3']" 
			mode="following-sibling-title"/>
	</xsl:template>
	
	<!-- For secondary or tertiary title style, convert it to a span -->
	<xsl:template name="span-title">
		<xsl:variable name="class">
			<xsl:choose>
				<xsl:when test="@style='mt2'">
					<xsl:value-of select="'Title_Secondary'"/>
				</xsl:when>
				<xsl:when test="@style='mt3' or @style='mt4'">
					<xsl:value-of select="'Title_Tertiary'"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		
		<span class="{$class}" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="."/>
		</span>
	</xsl:template>
	
	<!-- Attempt to delete extra title spans at body level -->
	<xsl:template match="body/span[@class='Title_Secondary' or @class='Title_Tertiary']"/>
	
	<!-- Convert all paragraph SFM style markers to styles understood by Pathway. -->
	<xsl:template match="para">
		<xsl:choose>
			<!-- Remove unneeded paragraphs. -->
			<xsl:when test="@style = 'h'"/>
			<xsl:when test="@style = 'rem'"/>
			<xsl:when test="@style = 'ide'"/>
			<xsl:when test="@style = 'restore'"/>
			<xsl:when test="@style = 'sts'"/>

			<!-- Scripture title markers are converted elsewhere. They include the following markers:
				mt, mt1, mt2, mt3, mt4
				They need to be deleted here. -->
			<xsl:when test="@style='mt' or @style='mt1' or @style='mt2' or @style='mt3' or @style='mt4'"/>
			
			<!-- Convert introduction styles. -->
			<xsl:when test="@style = 'im' or @style = 'im1' or @style = 'imt' or @style = 'imt1'">
				<h1 class="Intro_Title_Main" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'im2' or @style = 'imt2'">
				<h1 class="Intro_Title_Secondary" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'im3' or @style = 'imt3'">
				<h1 class="Intro_Title_Tertiary" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'is'">
				<h1 class="Intro_Section_Head" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'ip'">
				<p class="intropara" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'iq' or @style = 'iq1'">
				<p class="Intro_Citation_Line1" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'iq2'">
				<p class="Intro_Citation_Line2" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'imq'">
				<p class="Intro_Citation_Paragraph" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'ipr'">
				<p class="Intro_Cross-Reference" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'io' or @style = 'io1' or @style = 'ili' or @style = 'ili1'">
				<p class="Intro_List_Item1" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'io2' or @style = 'ili2'">
				<p class="Intro_List_Item2" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'io3' or @style = 'ili3'">
				<p class="Intro_List_Item3" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>

			<!-- Convert Scripture heading styles. -->
			<xsl:when test="@style = 's' or @style = 's1' or @style = 'cs' or @style = 'imte'">
				<h1 class="Section_Head" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'cl'">
				<h1 class="Chapter_Head" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'd'">
				<h1 class="Hebrew_Title" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'r'">
				<h1 class="Parallel_Passage_Reference" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'ms' or @style = 'ms1'">
				<h1 class="Section_Head_Major" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 's2' or @style = 'ms2'">
				<h1 class="Section_Head_Minor" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'qa'">
				<h1 class="Section_Head_Series" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'mr' or @style = 'sr'">
				<h1 class="Section_Range_Paragraph" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'sp'">
				<h1 class="Speech_Speaker" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<!-- No SFM mapping for:
					Variant_Section_Head
			-->

			<!-- Convert Scripture paragraph styles. -->
			<xsl:when test="@style = 'p'">
				<p class="Paragraph" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<!-- No SFM mapping for:
					Attribution
					Citation_Line1
					Citation_Line2
					Citation_Paragraph
			-->
			<xsl:when test="@style = 'cls'">
				<p class="Closing" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'lit'">
				<p class="Congregational_Response" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pr'">
				<p class="Cross-Reference" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'qc'">
				<p class="Doxology" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pmc'">
				<p class="Embedded_Text_Closing" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'qm' or @style = 'qm1'">
				<p class="Embedded_Text_Line1" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'qm2'">
				<p class="Embedded_Text_Line2" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'qm3'">
				<p class="Embedded_Text_Line3" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pmo'">
				<p class="Embedded_Text_Opening" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pm'">
				<p class="Embedded_Text_Paragraph" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'mi'">
				<p class="Embedded_Text_Paragraph_Continuation" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pmr'">
				<p class="Embedded_Text_Refrain" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pc'">
				<p class="Inscription_Paragraph" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'qs'">
				<p class="Interlude" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'q' or @style = 'q1'">
				<p class="Line1" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'q2'">
				<p class="Line2" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'q3'">
				<p class="Line3" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'li1' or @style = 'ph1'"> <!-- ph# is deprecated. li# is the recommended alternate. -->
				<p class="List_Item1" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'li2' or @style = 'ph2'">
				<p class="List_Item2" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'li3' or @style = 'ph3'">
				<p class="List_Item3" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pi1'">
				<p class="List_Item1_Additional" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pi2'">
				<p class="List_Item2_Additional" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'm'">
				<p class="Paragraph_Continuation" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'qr'">
				<p class="Refrain" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:call-template name="AddParaContent"/>
				</p>
			</xsl:when>
			<!-- No SFM mapping for:
					Speech_Line1
					Speech_Line2
			-->
			<xsl:when test="@style = 'b'">
				<p class="Stanza_Break" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
      <xsl:when test="starts-with(@style, 'tc')">
        <p class="Table_Cell" xmlns="http://www.w3.org/1999/xhtml">
          <xsl:apply-templates/>
        </p>
      </xsl:when>
      <xsl:when test="starts-with(@style, 'th')">
        <p class="Table_Cell_Head" xmlns="http://www.w3.org/1999/xhtml">
          <xsl:apply-templates/>
        </p>
      </xsl:when>
      <xsl:when test="@style = 'thr'">
				<p class="Table_Cell_Head_Last" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'tcr'">
				<p class="Table_Cell_Last" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'tr'">
				<p class="Table_Row" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<!-- No SFM mapping for:
				Variant_Paragraph
				Variant_Section_Tail
			-->

			<!-- Remove toc styles since they contain metadata that we don't want to display.  -->
			<xsl:when test="@style = 'toc1' or @style = 'toc2' or @style = 'toc3'"/>
			
			<!-- Mapping not available to Pathway style, so just use SFM. -->
			<xsl:otherwise>
				<p class="{@style}" xmlns="http://www.w3.org/1999/xhtml">
					<span lang="{$ws}">
						<xsl:apply-templates/>
					</span>
				</p>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Convert USX tables to XHTML format -->
	<xsl:template match="table">
		<xsl:choose>
			<xsl:when test="@style">
				<table style="{@style}" xmlns="http://www.w3.org/1999/xhtml">				
					<xsl:apply-templates/>
				</table>
			</xsl:when>
			<xsl:otherwise>
				<table xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</table>
			</xsl:otherwise>
		</xsl:choose>
		
	</xsl:template>
	
	<xsl:template match="row">
		<tr style="{@style}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:apply-templates/>
		</tr>
	</xsl:template>
	
	<xsl:template match="cell">
		<xsl:choose>
			<xsl:when test="@style = 'th1' or @style = 'th2' or @style = 'th3' or @style = 'th4' or @style = 'th5'">
				<th style="{@style}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</th>
			</xsl:when>
			<xsl:when test="@style = 'thr1' or @style = 'thr2' or @style = 'thr3' or @style = 'thr4' or @style = 'thr5'">
				<th style="{@style}" align="right" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</th>
			</xsl:when>
			<xsl:when test="@style = 'tc1' or @style = 'tc2' or @style = 'tc3' or @style = 'tc4' or @style = 'tc5'">
				<td style="{@style}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</td>
			</xsl:when>
			<xsl:when test="@style = 'tcr1' or @style = 'tcr2' or @style = 'tcr3' or @style = 'tcr4' or @style = 'tcr5'">
				<td style="{@style}" align="right" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</td>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<!-- Handle figure element -->
	<xsl:template match="figure">
		<xsl:variable name="bookCode">
			<xsl:choose>
				<xsl:when test="ancestor::book/@code">
					<xsl:value-of select="ancestor::book/@code"/>
				</xsl:when>
				<xsl:when test="ancestor::book/@id">
					<!-- Support old format for USX -->
					<xsl:value-of select="ancestor::book/@id"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		
		<xsl:variable name="figureNumber">
			<xsl:choose>
				<xsl:when test="ancestor::book/@code">
					<xsl:value-of select="count(preceding::figure[ancestor::book[@code=$bookCode]])+1"/>
				</xsl:when>
				<xsl:when test="ancestor::book/@id">
					<!-- Support old format for USX -->
					<xsl:value-of select="count(preceding::figure[ancestor::book[@id=$bookCode]])+1"/>
				</xsl:when>
			</xsl:choose>
			
		</xsl:variable> 
		
		<xsl:variable name="pictureLoc">
			<xsl:choose>
				<xsl:when test="@size = 'span'">
					<xsl:element name="pictureLoc">picturePage</xsl:element>
				</xsl:when>
				<xsl:otherwise> <!-- col -->
					<xsl:element name="pictureLoc">pictureColumn</xsl:element>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<div class="{$pictureLoc}" xmlns="http://www.w3.org/1999/xhtml">
			<img id="Figure-{$bookCode}-{$figureNumber}" class="picture" src="{$figurePath}{@file}" alt="{$altFigurePath}{@file}"/>
			<div lang="{$ws}" class="pictureCaption">
				<span lang="{$ws}">
					<xsl:value-of select="."/>
				</span>
				<span lang="{$ws}" class="reference">
					<xsl:text> (</xsl:text>
					<xsl:value-of select="@ref"/>
					<xsl:text>) </xsl:text>	
				</span>
			</div>
		</div>
	</xsl:template>
	
	<!-- Enclose paragraph text within a writing system. -->
	<xsl:template match="para/text()">
		<span lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="."/>
		</span>
	</xsl:template>

	<!-- Handle optional break  -->
	<xsl:template match="optbreak">
		<xsl:text>&#8204;</xsl:text>
	</xsl:template>
	
	<!-- Enclose text that can occur under <body> that is not yet within a writing system. -->
	<xsl:template match="usfm/text()|usx/text()">
		<span lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="."/>
		</span>
	</xsl:template>

  <!-- Convert character styles from SFM to styles understood by Pathway. -->
	<xsl:template match="char">
		<xsl:call-template name="char"/>
	</xsl:template>
	
	<xsl:template match="book/char">
		<xsl:if test="not(preceding-sibling::*[1][self::char])">
			<p class="Paragraph" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:apply-templates select="." mode="char"/>
			</p>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="char" mode="char">
		<xsl:call-template name="char"/>
		<xsl:apply-templates select="following-sibling::*[1][self::char]" mode="char"/>
	</xsl:template>
	
	<xsl:template name="char">
		<xsl:choose>
			<!-- Translation Editor character styles with no mapping to SFM:
					Abbreviation
					Alluded_Text
					Canonical_Reference
					Gloss
					Glossary_Definition
					Hyperlink
					Mentioned
					Notation_Tag
					So_Called
					Variant
			-->
			<xsl:when test="@style = 'bk'">
				<span class="Book_Title_In_Text" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'em' or @style = 'it'">
				<span class="Emphasis" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'tl'">
				<span class="Foreign" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'sig'">
				<span class="Hand" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'sc'">
				<span class="Inscription" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'imt2'">
				<span class="Intro_Title_Secondary" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'imt3'">
				<span class="Intro_Title_Tertiary" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'k'">
				<span class="Glossary_Keyvalue" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<a href="#" class="Glossaryvaluehref">
						<xsl:attribute name="id">
							<xsl:text>k_</xsl:text>	<xsl:value-of select="count(preceding::char[@style='k'])+1"/>
						</xsl:attribute>
						<xsl:apply-templates/>
					</a>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'nd'">
				<span class="Name_Of_God" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'ord'">
				<span class="Ordinal_Number_Ending" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'qt'">
				<span class="Quoted_Text" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'w'">
				<span class="See_In_Glossary" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
          <a href="#" class="Glossary_Key">
		          	<xsl:attribute name="id">
		          		<xsl:text>w_</xsl:text>	<xsl:value-of select="count(preceding::char[@style='w'])+1"/>
		          	</xsl:attribute>
            <xsl:choose>
              <xsl:when test="contains(text(),'|')">
                <xsl:attribute name="title">
                  <xsl:value-of select="substring-after(text(),'|')"/>
                </xsl:attribute>
                <xsl:value-of select="substring-before(text(),'|')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:apply-templates/>
              </xsl:otherwise>
            </xsl:choose>
		          </a>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'add'">
				<span class="Supplied" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'uw'">
				<span class="Untranslated_Word" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>

			<!-- TODO/REVIEW: Do these SFMs/styles need special handling (e.g. like chapter and verse where value is in an attribute)?
			Chapter_Number_Alternate (ca)
			Verse_Number_Alternate (va)
			Verse_Number_In_Note (fv, V)
			-->

			<!-- Footnote character styles -->
			<xsl:when test="@style = 'ft'">
				<span class="ft" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'xt'">
				<span class="xt" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'fm'">
				<span class="Note_Marker" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'fr' or @style = 'xo'">
				<span class="Note_Target_Reference" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'fk' or @style = 'xk'">
				<span class="Referenced_Text" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>
      <xsl:when test="@style = 'fq' or @style = 'xq'">
      	<span class="footnote_query" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
          <xsl:apply-templates/>
        </span>
      </xsl:when>
      <xsl:when test="@style = 'fqa'">
				<span class="footnote_querya" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:when>

			<!-- Character style does not have a mapping to Translation Editor style so use SFM as the style. -->
			<xsl:otherwise>
				<span class="{@style}" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</span>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Pull the chapter number into the paragraph content. -->
	<xsl:template name="AddParaContent">
		<xsl:apply-templates select="preceding-sibling::*[1]" mode="chapter"/>
		<xsl:apply-templates/>
	</xsl:template>
	
	<xsl:template match="*" mode="chapter"/>

	<xsl:template match="chapter" mode="chapter">
		<span class="Chapter_Number" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="@number"/>
		</span>
	</xsl:template>
	
	<!-- Search backward for a chapter, stopping the search if an intro or content para is encountered. -->
	<xsl:template match="para" mode="chapter">
		<xsl:choose>
			<!-- If we encounter a recognized content paragraph style, we don't need
			to continue searching for a chapter. Any prior chapter number should already
			have been included in a previous content paragraph. -->
			<xsl:when test="@style = 'p'"/>
			<xsl:when test="@style = 'cls'"/>
			<xsl:when test="@style = 'lit'"/>
			<xsl:when test="@style = 'pr'"/>
			<xsl:when test="@style = 'qc'"/>
			<xsl:when test="@style = 'pmc'"/>
			<xsl:when test="@style = 'qm' or @style = 'qm1'"/>
			<xsl:when test="@style = 'qm2'"/>
			<xsl:when test="@style = 'qm3'"/>
			<xsl:when test="@style = 'pmo'"/>
			<xsl:when test="@style = 'pm'"/>
			<xsl:when test="@style = 'mi'"/>
			<xsl:when test="@style = 'pmr'"/>
			<xsl:when test="@style = 'pc'"/>
			<xsl:when test="@style = 'qs'"/>
			<xsl:when test="@style = 'q' or @style = 'q1'"/>
			<xsl:when test="@style = 'q2'"/>
			<xsl:when test="@style = 'q3'"/>
			<xsl:when test="@style = 'li1' or @style = 'ph1'"/> <!-- ph# is deprecated. li# is the recommended alternate. -->
			<xsl:when test="@style = 'li2' or @style = 'ph2'"/>
			<xsl:when test="@style = 'li3' or @style = 'ph3'"/>
			<xsl:when test="@style = 'pi1'"/>
			<xsl:when test="@style = 'pi2'"/>
			<xsl:when test="@style = 'm'"/>
			<xsl:when test="@style = 'qr'"/>
			
			<!-- If we encounter an introduction style, we shouldn't find a chapter element earlier. -->
			<xsl:when test="starts-with(@style, 'i')"/>
			
			<!-- Otherwise, keep iterating backwards searching for a chapter number. -->
			<xsl:otherwise>
				<xsl:apply-templates select="preceding-sibling::*[1]" mode="chapter"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Remove chapter numbers from the top level. -->
	<xsl:template match="chapter"/>
	
	<!-- Handle verses within paragraphs -->
	<xsl:template match="verse">
		<span class="Verse_Number" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="@number"/>
		</span>
	</xsl:template>

	<!-- Convert footnotes, cross references and end notes. -->
	<xsl:template match="note">
		<!-- Number of footnotes thus far of all styles (used to make unique footnote reference) -->
		<xsl:variable name="bookCode">
			<xsl:choose>
				<xsl:when test="ancestor::book/@code">
					<xsl:value-of select="ancestor::book/@code"/>
				</xsl:when>
				<xsl:when test="ancestor::book/@id">
					<!-- Support old format for USX -->
					<xsl:value-of select="ancestor::book/@id"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="footnoteNumber">
			<xsl:choose>
				<xsl:when test="ancestor::book/@code">
					<xsl:value-of select="count(preceding::note[ancestor::book[@code=$bookCode]])+1"/>
				</xsl:when>
				<xsl:when test="ancestor::book/@id">
					<xsl:value-of select="count(preceding::note[ancestor::book[@id=$bookCode]])+1"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="footnoteCaller">
			<xsl:choose>
				<!-- Only set a footnote caller for footnotes and endnotes, not cross-references. -->
				<xsl:when test="@caller = '+'">
					<xsl:variable name="footnoteStyle" select="@style"/>
					<!-- For calculating the index, only count notes with the same style so they can have different numbering systems. -->
					<xsl:variable name="footnoteIndex">
						<xsl:choose>
							<xsl:when test="ancestor::book/@code">
								<xsl:value-of select="count(preceding::note[ancestor::book[@code=$bookCode]][@style=$footnoteStyle])+1"/>
							</xsl:when>
							<xsl:when test="ancestor::book/@id">
								<!-- Support old format for USX -->
								<xsl:value-of select="count(preceding::note[ancestor::book[@id=$bookCode]][@style=$footnoteStyle])+1"/>
							</xsl:when>
						</xsl:choose>
					</xsl:variable>
					<xsl:number format="a" value="$footnoteIndex"/>
				</xsl:when>
				<xsl:when test="not(@caller = '+' or @caller = '-')">
					<!-- Custom symbol specified so use whatever character is provided. -->
					<xsl:value-of select="@caller"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="footnoteStyle">
			<xsl:choose>
				<xsl:when test="@style = 'x'">
					<xsl:value-of select="'Note_CrossHYPHENReference_Paragraph'"/>
				</xsl:when>
				<xsl:when test="@style = 'fe'">
					<xsl:value-of select="'EndNote_General_Paragraph'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'Note_General_Paragraph'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<span class="scrFootnoteMarker" xmlns="http://www.w3.org/1999/xhtml">
			<a href="#Footnote-{$bookCode}-{$footnoteNumber}"/>
		</span>
		
		<xsl:choose>
			<xsl:when test="string-length($footnoteCaller) != 0">
				<span class="{$footnoteStyle}" id="Footnote-{$bookCode}-{$footnoteNumber}" title="{$footnoteCaller}" xmlns="http://www.w3.org/1999/xhtml">
					<!-- Handle template for character styles. -->
					<xsl:apply-templates/>
				</span>
			</xsl:when>
			<xsl:otherwise>
				<span class="{$footnoteStyle}" id="Footnote-{$bookCode}-{$footnoteNumber}" title="" xmlns="http://www.w3.org/1999/xhtml">
					<!-- Handle template for character styles. -->
					<xsl:apply-templates/>
				</span>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Enclose paragraph text for a note within a writing system, if it isn't already. -->
	<xsl:template match="note/text()">
		<span lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="."/>
		</span>
	</xsl:template>
	
</xsl:stylesheet>
