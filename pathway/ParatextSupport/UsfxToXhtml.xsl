<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="yes"
		doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"/>
	<xsl:param name="ws" select="'es'"/>
	<xsl:param name="userWs" select="'en'"/>
	<xsl:param name="dateTime" select="'4-June-2010'"/>
	<xsl:param name="user" select="'Lothers'"/>
	<xsl:param name="projName" select="PROJNAME"/>

	<!-- Get book identification -->
	<xsl:variable name="bookCode" select="usfm/book/@id"/>
	<xsl:variable name="bookInToc" select="normalize-space(usfm/para[@style='toc2'])"/>
	<xsl:variable name="bookHeading" select="normalize-space(usfm/para[@style='h'])"/>
	<xsl:variable name="bookTitle" select="normalize-space(usfm/para[@style='mt'])"/>
	<xsl:variable name="bookTitle1" select="normalize-space(usfm/para[@style='mt1'])"/>

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

	<xsl:template match="usfm">
		<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="utf-8" lang="utf-8">
			<head>
				<title/>
				<link rel="stylesheet" href="PROJNAME.css" type="text/css"/>
				<meta name="description" content="PROJNAME exported by {$user} on {$dateTime}"/>
				<meta name="filename" content="PROJNAME.xhtml"/>
			</head>
			<body class="scrBody">
				<xsl:apply-templates/>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="book">
		<div class="scrBook" xmlns="http://www.w3.org/1999/xhtml">
			<span class="scrBookName" lang="{$ws}">
				<!-- Find the name of the book in this fallback sequence: (1) book name specified in toc2 
					(2) book name in the heading, (3) main title, (4) three-letter book code -->
				<xsl:choose>
					<xsl:when test="string-length($bookInToc) = 0">
						<xsl:choose>
							<xsl:when test="string-length($bookHeading) = 0">
								<xsl:choose>
									<xsl:when
										test="string-length($bookTitle) = 0 and string-length($bookTitle1) = 0">
										<xsl:value-of select="$bookCode"/>
									</xsl:when>
									<xsl:otherwise>
										<!-- We may have a title in \mt or \mt1, but we include both since one of them will be empty. -->
										<xsl:value-of select="$bookTitle"/>
										<xsl:value-of select="$bookTitle1"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$bookHeading"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$bookInToc"/>
					</xsl:otherwise>
				</xsl:choose>
			</span>
		</div>
	</xsl:template>

	<xsl:template match="para">
		<xsl:choose>
			<!-- Convert all SFM style markers to Translation Editor styles. -->

			<!-- Remove 'h' paragraphs. -->
			<xsl:when test="@style = 'h'"/>

			<!-- Convert Scripture title styles -->
			<xsl:when test="@style = 'mt' or @style = 'mt1'">
				<div class="Title_Main" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</div>
			</xsl:when>
			<!-- mt2 and mt3 are paragraph styles, but they will need to become spans and be moved inside
			     the adjacent title division/paragraph. -->
			<xsl:when test="@style = 'mt2'">
				<span class="Title_Secondary" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'mt3'">
				<span class="Title_Tertiary" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>

			<!-- Convert introduction styles. -->
			<xsl:when test="@style = 'is'">
				<h1 class="Intro_Section_Head" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'ip'">
				<p class="Intro_Paragraph" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'iq1'">
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
			<xsl:when test="@style = 'io1'">
				<p class="Intro_List_Item1" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'io2'">
				<p class="Intro_List_Item2" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'io3'">
				<p class="Intro_List_Item3" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>

			<!-- Convert Scripture heading styles. -->
			<xsl:when test="@style = 's' or @style = 's1' or @style = 'cs'">
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
			<xsl:when test="@style = 'ms'">
				<h1 class="Section_Head_Major" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 's2'">
				<h1 class="Section_Head_Minor" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'qa'">
				<h1 class="Section_Head_Series" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</h1>
			</xsl:when>
			<xsl:when test="@style = 'mr'">
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
					<xsl:call-template name="MoveOrphanedChapter"/>
					<xsl:apply-templates/>
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
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'lit'">
				<p class="Congregational_Response" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pr'">
				<p class="Cross-Reference" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'qc'">
				<p class="Doxology" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pmc'">
				<p class="Embedded_Text_Closing" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'qm'">
				<p class="Embedded_Text_Line1" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'qm2'">
				<p class="Embedded_Text_Line2" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'qm3'">
				<p class="Embedded_Text_Line3" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pmo'">
				<p class="Embedded_Text_Opening" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pm'">
				<p class="Embedded_Text_Paragraph" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'mi'">
				<p class="Embedded_Text_Paragraph_Continuation" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pmr'">
				<p class="Embedded_Text_Refrain" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pc'">
				<p class="Inscription_Paragraph" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'qs'">
				<p class="Interlude" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'q1'">
				<p class="Line1" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'q2'">
				<p class="Line2" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'q3'">
				<p class="Line3" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'li1'">
				<p class="List_Item1" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pi1'">
				<p class="List_Item1_Additional" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'li2'">
				<p class="List_Item2" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'pi2'">
				<p class="List_Item2_Additional" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'li3'">
				<p class="List_Item3" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'm'">
				<p class="Paragraph_Continuation" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'qr'">
				<p class="Refrain" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
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
			<xsl:when test="@style = 'tc'">
				<p class="Table_Cell" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:apply-templates/>
				</p>
			</xsl:when>
			<xsl:when test="@style = 'th'">
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

			<!-- Mapping not available to TE style, so just use SFM. -->
			<xsl:otherwise>
				<p class="{@style}" xmlns="http://www.w3.org/1999/xhtml">
					<span lang="{$ws}">
						<xsl:apply-templates/>
					</span>
				</p>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Enclose paragraph text within a writing system. -->
	<xsl:template match="para/text()">
		<span lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="."/>
		</span>
	</xsl:template>

	<!-- Enclose text that can occur under <body> that is not yet within a writing system. -->
	<xsl:template match="usfm/text()">
		<span lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="."/>
		</span>
	</xsl:template>

	<!-- Convert character styles from SFM to Translation Editor styles. -->
	<xsl:template match="char">
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
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'em'">
				<span class="Emphasis" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'tl'">
				<span class="Foreign" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'sig'">
				<span class="Hand" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'sc'">
				<span class="Inscription" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'imt2'">
				<span class="Intro_Title_Secondary" lang="{$ws}"
					xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'imt3'">
				<span class="Intro_Title_Tertiary" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'k'">
				<span class="Key_Word" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'nd'">
				<span class="Name_Of_God" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'ord'">
				<span class="Ordinal_Number_Ending" lang="{$ws}"
					xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'qt'">
				<span class="Quoted_Text" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'w'">
				<span class="See_In_Glossary" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'add'">
				<span class="Supplied" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<!-- REVIEW: mt2 and mt3 are paragraph styles in USFX that need to be converted to character styles. -->
			<xsl:when test="@style = 'mt2'">
				<span class="Title_Secondary" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'mt3'">
				<span class="Title_Tertiary" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'uw'">
				<span class="Untranslated_Word" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>

			<!-- TODO/REVIEW: Do these SFMs/styles need special handling (e.g. like chapter and verse where value is in an attribute)?
			Chapter_Number_Alternate (ca)
			Verse_Number_Alternate (va)
			Verse_Number_In_Note (fv, V)
			-->

			<!-- TODO/REVIEW: Figure styles still need to be handled. -->
			<xsl:when test="@style = 'figdesc' or @style = 'cap'">
				<span class="Figure_Description" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'figcat' or @style = 'cat'">
				<span class="Figure_Filename" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'figlaypos'">
				<span class="Figure_Layout_Position" lang="{$ws}"
					xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'figrefrng'">
				<span class="Figure_Location" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'figcopy'">
				<span class="Figure_Copyright" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'figscale'">
				<span class="Figure_Scale" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'fig'">
				<span class="Figure_USFM_Parameters" lang="{$ws}"
					xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>

			<!-- Footnote character styles -->
			<xsl:when test="@style = 'ft' or @style = 'xt'">
				<span lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'fm'">
				<span class="Note_Marker" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'fr' or @style = 'xo'">
				<span class="Note_Target_Reference" lang="{$ws}"
					xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'fk' or @style = 'xk'">
				<span class="Referenced_Text" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>
			<xsl:when test="@style = 'fq' or @style = 'xq'">
				<span class="Alternate_Reading" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:when>

			<!-- Character style does not have a mapping to Translation Editor style so use SFM. -->
			<xsl:otherwise>
				<span class="{@style}" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
					<xsl:value-of select="."/>
				</span>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Move any chapters under html and followed by a paragraph into the following paragraph. -->
	<xsl:template name="MoveOrphanedChapter">
		<!--<xsl:comment>MoveOrphanedChapter called.</xsl:comment>-->
		<xsl:choose>
			<!-- REVIEW: How far back do we go? This will check if the element before the current paragraph is a chapter, 
			or the one before that (in case there is a single heading paragraph). The problem is if there are multiple 
			heading paragraphs.
			-->
			<xsl:when test="./verse[@number='1']">
				<xsl:choose>
					<xsl:when test="preceding-sibling::*[1][self::chapter]">
						<span class="Chapter_Number" lang="{$ws}"
							xmlns="http://www.w3.org/1999/xhtml">
							<xsl:value-of select="preceding-sibling::*[1][self::chapter]/@number"/>
						</span>
					</xsl:when>
					<xsl:when test="preceding-sibling::*[2][self::chapter]">
						<span class="Chapter_Number" lang="{$ws}"
							xmlns="http://www.w3.org/1999/xhtml">
							<xsl:value-of select="preceding-sibling::*[2][self::chapter]/@number"/>
						</span>
					</xsl:when>
					<xsl:when test="preceding-sibling::*[3][self::chapter]">
						<span class="Chapter_Number" lang="{$ws}"
							xmlns="http://www.w3.org/1999/xhtml">
							<xsl:value-of select="preceding-sibling::*[3][self::chapter]/@number"/>
						</span>
					</xsl:when>
					<xsl:when test="preceding-sibling::*[4][self::chapter]">
						<span class="Chapter_Number" lang="{$ws}"
							xmlns="http://www.w3.org/1999/xhtml">
							<xsl:value-of select="preceding-sibling::*[4][self::chapter]/@number"/>
						</span>
					</xsl:when>
					<xsl:when test="preceding-sibling::*[5][self::chapter]">
						<span class="Chapter_Number" lang="{$ws}"
							xmlns="http://www.w3.org/1999/xhtml">
							<xsl:value-of select="preceding-sibling::*[5][self::chapter]/@number"/>
						</span>
					</xsl:when>
				</xsl:choose>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<!-- Delete any chapters that are siblings of para (children of body). -->
	<xsl:template match="chapter[following-sibling::*[1][self::para]]">
		<!--<xsl:comment>Matched html/chapter</xsl:comment>-->
	</xsl:template>

	<!--
	<xsl:template match="chapter">
		<span class="Chapter_Number" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="@number"/>
		</span>
	</xsl:template>
	-->

	<xsl:template match="verse">
		<span class="Verse_Number" lang="{$ws}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:value-of select="@number"/>
		</span>
	</xsl:template>

	<xsl:template match="note">
		<xsl:variable name="footnoteNumber" select="count(preceding::note)+1"/>
		<xsl:variable name="footnoteCaller">
			<xsl:number format="a" value="$footnoteNumber"/>
		</xsl:variable>
		<span class="scrFootnoteMarker" xmlns="http://www.w3.org/1999/xhtml">
			<a href="#{$bookCode}-{$footnoteNumber}"/>
		</span>
		<span class="Note_General_Paragraph" id="{$bookCode}-{$footnoteNumber}"
			title="{$footnoteCaller}" xmlns="http://www.w3.org/1999/xhtml">
			<!-- Handle template for character styles. -->
			<xsl:apply-templates/>
		</span>
	</xsl:template>

</xsl:stylesheet>
