<?xml version="1.0" encoding="UTF-8"?>
<!-- Transform XHTML file exported from TE into a SQL file to be used in making a YouVersion resource. The verses will be output as unformatted text. -->
<xsl:transform version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns:fn="http://www.w3.org/2005/xpath-functions"
	exclude-result-prefixes="xhtml">

	<xsl:output method="text" encoding="UTF-8"/>

	<xsl:strip-space elements="*"/>

	<!-- Use a key to speed up the processing of the verses for each chapter. -->
	<xsl:key name="verses-by-chapter" match="xhtml:span[@class='Verse_Number']"
		use="generate-id(preceding::xhtml:span[@class='Chapter_Number'][1])"/>
	<!-- Use a key to speed up the processing of the spans for each verse.
			These spans may come from more than one paragraph.-->
	<xsl:key name="spans-by-verse"
		match="xhtml:span[(not(@class) or @class='Inscription' or @class='Words_Of_Christ') and
							not(parent::xhtml:div[@class='Section_Head']) and
							not(parent::xhtml:div[@class='Title_Main']) and
							not(parent::*/parent::xhtml:div[@class='scrIntroSection']) and
							not(parent::xhtml:span[@class='Note_General_Paragraph'])]"
		use="generate-id(preceding::xhtml:span[@class='Verse_Number'][1])"/>

	<xsl:variable name="langAbbr">
		<xsl:value-of
			select="/xhtml:html/xhtml:body/xhtml:div[@class='scrBook']/xhtml:span[@class='scrBookName']/@lang"
		/>
	</xsl:variable>

	<!-- Process the top element. -->
	<xsl:template match="xhtml:html">
		<xsl:apply-templates/>
	</xsl:template>

	<!-- Skip the head. -->
	<xsl:template match="xhtml:head"/>

	<xsl:template match="xhtml:body">
		<xsl:text>CREATE TABLE verses (id integer primary key, book char(7), verse real, unformatted text);</xsl:text>
		<xsl:apply-templates/>
		<xsl:text>
CREATE INDEX verse_lookup_index on verses (book, verse);</xsl:text>
	</xsl:template>
	<!-- xhtml:body -->

	<!-- Process the books. -->
	<xsl:template match="xhtml:div[@class='scrBook']">
		<!-- Use the OSIS book code, not the TE book code. -->
		<xsl:variable name="bookCode">
			<xsl:call-template name="getOsisBookCode">
				<xsl:with-param name="TEBookCode" select="xhtml:span[@class='scrBookCode']/text()"/>
			</xsl:call-template>
		</xsl:variable>
		<!-- Process the chapters for this given book. -->
		<xsl:apply-templates select="descendant::xhtml:span[@class='Chapter_Number']">
			<xsl:with-param name="bookCode" select="$bookCode"/>
		</xsl:apply-templates>
	</xsl:template>

	<!-- Process the chapters. -->
	<xsl:template match="xhtml:span[@class='Chapter_Number']">
		<xsl:param name="bookCode"/>
		<xsl:variable name="reportProgress" select="fn:abs(.)"/>
		<xsl:variable name="currentChapter" select="."/>
		<xsl:variable name="startGenerateID" select="generate-id()"/>
		<xsl:variable name="verses" select="key('verses-by-chapter',$startGenerateID)"/>
		<!-- Process the verses for this given chapter. -->
		<xsl:apply-templates select="$verses">
			<xsl:with-param name="bookCode" select="$bookCode"/>
			<xsl:with-param name="currentChapter" select="$currentChapter"/>
		</xsl:apply-templates>
	</xsl:template>
	<!-- xhtml:span[@class='Chapter_Number'] -->

	<xsl:template match="xhtml:span[@class='Verse_Number']">
		<xsl:param name="bookCode"/>
		<xsl:param name="currentChapter"/>
		<xsl:variable name="this-id" select="generate-id(.)"/>
		<xsl:variable name="verseNumber">
			<xsl:choose>
				<!-- If there is a range of verses, take the first verse number. -->
				<xsl:when test="contains(text(),'-')">
					<xsl:value-of select="substring-before(text(),'-')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="text()"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<!-- verseNumber -->
		<!-- Collect the spans within this paragraph that apply to this verse. -->
		<xsl:variable name="verseSpans" select="key('spans-by-verse',$this-id)"/>
		<!-- Write out the record. -->
		<xsl:text>
insert into verses (book, verse, unformatted) values ('</xsl:text>
		<xsl:value-of select="$bookCode"/>
		<xsl:text>', '</xsl:text>
		<xsl:value-of select="$currentChapter"/>
		<xsl:choose>
			<xsl:when test="$verseNumber &lt; 10">
				<xsl:text>.00</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>.0</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:value-of select="$verseNumber"/>
		<xsl:text>', '</xsl:text>
		<!-- Handle the spans for this verse in the "unformatted" form. -->
		<xsl:apply-templates select="$verseSpans" mode="unformatted"/>
		<xsl:text>');</xsl:text>
	</xsl:template>
	<!-- xhtml:span[@class='Verse_Number'] -->

	<!-- Process the main title. -->
	<xsl:template match="xhtml:div[@class='Title_Main']/xhtml:span[@class='Title_Secondary']">
		<xsl:if test="string-length(text())>0">
			<xsl:text disable-output-escaping="yes">&lt;h2&gt;</xsl:text>
			<xsl:value-of select="text()"/>
			<xsl:text disable-output-escaping="yes">&lt;/h2&gt;</xsl:text>
		</xsl:if>
	</xsl:template>
	<!-- Title_Secondary -->
	<xsl:template match="xhtml:div[@class='Title_Main']/xhtml:span[@class='Title_Tertiary']">
		<xsl:if test="string-length(text())>0">
			<xsl:text disable-output-escaping="yes">&lt;h3&gt;</xsl:text>
			<xsl:value-of select="text()"/>
			<xsl:text disable-output-escaping="yes">&lt;/h3&gt;</xsl:text>
		</xsl:if>
	</xsl:template>
	<!-- Title_Tertiary -->
	<xsl:template match="xhtml:div[@class='Title_Main']/xhtml:span[not(@class)]">
		<xsl:if test="string-length(text())>0">
			<xsl:text disable-output-escaping="yes">&lt;h1&gt;</xsl:text>
			<xsl:value-of select="text()"/>
			<xsl:text disable-output-escaping="yes">&lt;/h1&gt;</xsl:text>
		</xsl:if>
	</xsl:template>
	<!-- Main Title span with no @class. -->

	<xsl:template match="xhtml:span" mode="unformatted">
		<xsl:value-of select="text()"/>
		<!-- If this is the end of the section, but not the end of the file, add a carriage return. -->
		<xsl:if
			test="not(following-sibling::xhtml:span[not(@class)]) and
						not(parent::xhtml:div/following-sibling::xhtml:div) and
						parent::xhtml:div/parent::xhtml:div/following-sibling[xhtml:div[@class='scrSection']]">
			<xsl:text>
</xsl:text>
		</xsl:if>
		<!-- If this is the end of the paragraph, add a carriage return. -->
		<xsl:if
			test="not(following-sibling::xhtml:span[not(@class)]) and
						(not(parent::xhtml:div/following-sibling::xhtml:div) or
							parent::xhtml:div/following-sibling::xhtml:div[1][@class='Paragraph' or @class='Line1' or
												@class='Line2' or @class='Paragraph_Continuation'])">
			<xsl:text>
</xsl:text>
		</xsl:if>
	</xsl:template>
	<!-- xhtml:span mode="unformatted" -->

	<xsl:template name="specifyChapter">
		<!-- get chapter number from previous section -->
		<xsl:choose>
			<xsl:when test="xhtml:div[@class='Paragraph']/xhtml:span[@class='Chapter_Number']">
				<xsl:value-of
					select="xhtml:div[@class='Paragraph']/xhtml:span[@class='Chapter_Number']/text()"
				/>
			</xsl:when>
			<xsl:otherwise>
				<!-- Get the chapter number from the closest previous div[@class='scrSection']. -->
				<xsl:value-of
					select="preceding-sibling::xhtml:div[@class='scrSection' and xhtml:div[@class='Paragraph']/xhtml:span[@class='Chapter_Number']][1]/xhtml:div[@class='Paragraph']/xhtml:span[@class='Chapter_Number']/text()"
				/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Get the Osis book code that corresponds to the given TE book code. -->
	<xsl:template name="getOsisBookCode">
		<xsl:param name="TEBookCode"/>
		<xsl:choose>
			<xsl:when test="$TEBookCode='MAT' ">Matt</xsl:when>
			<xsl:when test="$TEBookCode='MRK' ">Mark</xsl:when>
			<xsl:when test="$TEBookCode='LUK' ">Luke</xsl:when>
			<xsl:when test="$TEBookCode='JHN' ">John</xsl:when>
			<xsl:when test="$TEBookCode='ACT' ">Acts</xsl:when>
			<xsl:when test="$TEBookCode='ROM' ">Rom</xsl:when>
			<xsl:when test="$TEBookCode='1CO' ">1Cor</xsl:when>
			<xsl:when test="$TEBookCode='2CO' ">2Cor</xsl:when>
			<xsl:when test="$TEBookCode='GAL' ">Gal</xsl:when>
			<xsl:when test="$TEBookCode='EPH' ">Eph</xsl:when>
			<xsl:when test="$TEBookCode='PHP' ">Phil</xsl:when>
			<xsl:when test="$TEBookCode='COL' ">Col</xsl:when>
			<xsl:when test="$TEBookCode='1TH' ">1Thess</xsl:when>
			<xsl:when test="$TEBookCode='2TH' ">2Thess</xsl:when>
			<xsl:when test="$TEBookCode='1TI' ">1Tim</xsl:when>
			<xsl:when test="$TEBookCode='2TI' ">2Tim</xsl:when>
			<xsl:when test="$TEBookCode='TIT' ">Titus</xsl:when>
			<xsl:when test="$TEBookCode='PHM' ">Phlm</xsl:when>
			<xsl:when test="$TEBookCode='HEB' ">Heb</xsl:when>
			<xsl:when test="$TEBookCode='JAS' ">Jas</xsl:when>
			<xsl:when test="$TEBookCode='1PE' ">1Pet</xsl:when>
			<xsl:when test="$TEBookCode='2PE' ">2Pet</xsl:when>
			<xsl:when test="$TEBookCode='1JN' ">1John</xsl:when>
			<xsl:when test="$TEBookCode='2JN' ">2John</xsl:when>
			<xsl:when test="$TEBookCode='3JN' ">3John</xsl:when>
			<xsl:when test="$TEBookCode='JUD' ">Jude</xsl:when>
			<xsl:when test="$TEBookCode='REV' ">Rev</xsl:when>
			<xsl:otherwise>
				<xsl:comment>
					<xsl:text> Warning: TE Book Code "</xsl:text>
					<xsl:value-of select="$TEBookCode"/>
					<xsl:text>" is not recognized. </xsl:text>
				</xsl:comment>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- getOsisBookCode -->

	<!-- Skip the div if the class is 'Parallel_Passage_Reference' or 'pictureCenter'.
		We see no evidence of these being used by YouVersion.com. -->
	<xsl:template match="xhtml:div[@class='Parallel_Passage_Reference']"/>
	<xsl:template match="xhtml:div[@class='pictureCenter']"/>
	<!-- Skip span with class "Note_General_Paragraph". -->
	<xsl:template match="xhtml:span[@class='Note_General_Paragraph']"/>

	<!-- Special handling of text. -->
	<xsl:template match="text()">
		<!-- Replace curly quotes with straight quotes. -->
		<xsl:value-of select="translate(.,'“”','&quot;&quot;')"/>
	</xsl:template>

	<!-- Default element and attribute templates. -->
	<xsl:template match="*">
		<xsl:comment>Warning :: The element "<xsl:value-of select="name()"/> [class='<xsl:value-of
				select="@class"/>']", child of "<xsl:value-of select="name(..)"/>
				[class='<xsl:value-of select="../@class"/>']", has no matching
		template.</xsl:comment>
	</xsl:template>
	<!-- default attribute template -->
	<xsl:template match="@*">
		<xsl:comment>Warning :: The attribute "<xsl:value-of select="name()"/>" for element
				"<xsl:value-of select="name(..)"/>" has no matching template.</xsl:comment>
	</xsl:template>

</xsl:transform>
