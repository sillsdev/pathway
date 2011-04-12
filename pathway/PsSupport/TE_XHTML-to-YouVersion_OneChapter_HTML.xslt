<?xml version="1.0" encoding="UTF-8"?>
<!-- Transform one chapter from XHTML to HTML used by YouVersion. -->

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml"
    xmlns:fn="http://www.w3.org/2005/xpath-functions"
    exclude-result-prefixes="xhtml">

	<xsl:output method="html" version="1.0" encoding="UTF-8" indent="yes"/>

    <xsl:strip-space elements="*"/>

	<!-- Use the OSIS book code, not the TE book code. -->
	<xsl:variable name="bookCode">
		<xsl:value-of select="/xhtml:html/xhtml:body/xhtml:p[@class='scrBookCode']/text()"/>
		<!-- xsl:call-template name="getOsisBookCode">
			<xsl:with-param name="TEBookCode" select="/xhtml:html/xhtml:body/xhtml:p[@class='scrBookCode']/text()"/>
		</xsl:call-template -->
	</xsl:variable>

	<xsl:variable name="bookName">
		<xsl:value-of select="/xhtml:html/xhtml:body/xhtml:p[@class='scrBookName']/text()"/>
	</xsl:variable>

	<xsl:variable name="chapterNumber">
		<xsl:value-of select="/xhtml:html/xhtml:body/xhtml:div[@class='scrSection']//xhtml:span[@class='Chapter_Number']/text()"/>
	</xsl:variable>

    <!-- Process the top element, html. -->
    <xsl:template match="xhtml:html">
		<xsl:apply-templates/>
    </xsl:template>

	<!-- Skip the head element. -->
	<xsl:template match="xhtml:head"/>

    <xsl:template match="xhtml:body">
		<xsl:element name="div">
			<xsl:element name="h1">
				<xsl:attribute name="class">
					<xsl:value-of select="$bookCode"/>
					<xsl:text>_</xsl:text>
					<xsl:value-of select="$chapterNumber"/>
				</xsl:attribute>
				<xsl:attribute name="lang">
					<xsl:value-of select="xhtml:p/@lang"/>
				</xsl:attribute>
				<xsl:value-of select="$bookName"/>
				<xsl:text> </xsl:text>
				<xsl:value-of select="$chapterNumber"/>
			</xsl:element>
			<xsl:apply-templates/>
		</xsl:element>
    </xsl:template>

	<!-- Process the chapter. -->
    <xsl:template match="xhtml:div[@class='scrSection']">
		<xsl:apply-templates/>
    </xsl:template> <!-- xhtml:div[@class='scrSection'] -->

	<xsl:template match="xhtml:div[@class='Section_Head']">
		<xsl:if test="string-length(xhtml:span/text()) > 0">
			<xsl:element name="h2">
				<xsl:attribute name="lang">
					<xsl:value-of select="xhtml:span/@lang"/>
				</xsl:attribute>
				<xsl:value-of select="xhtml:span/text()"/>
			</xsl:element>
		</xsl:if>
	</xsl:template>

	<xsl:template match="xhtml:div[@class='Paragraph' or @class='Paragraph_Continuation' or @class='Line1' or @class='Line2']">
		<xsl:choose>
			<!-- Skip paragraphs that only contain a picture. -->
			<xsl:when test="count(child::*)=1 and xhtml:div[@class='pictureCenter']"></xsl:when>
			<xsl:otherwise>
				<xsl:element name="p">
					<!-- Set the attribute "class" to the appropriate value based on @class. -->
					<xsl:choose>
						<xsl:when test="@class='Line1' ">
							<xsl:attribute name="class">Line1</xsl:attribute>
						</xsl:when>
						<xsl:when test="@class='Line2' ">
							<xsl:attribute name="class">Line2</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:if test="xhtml:span[not(@class)]/@lang">
								<xsl:attribute name="lang">
									<xsl:value-of select="xhtml:span[not(@class)]/@lang"/>
								</xsl:attribute>
							</xsl:if>
						</xsl:otherwise>
					</xsl:choose>
					<!-- Handle the value of the paragraph. -->
					<xsl:choose>
						<!-- Look for paragraphs which contain verses or a chapter. -->
						<xsl:when test="xhtml:span[@class='Chapter_Number' or @class='Verse_Number']">
							<!-- Handle span elements that come before the first span[@class='Verse_Number']. -->
							<xsl:apply-templates select="xhtml:span[@class='Verse_Number'][1]/preceding-sibling::*[not(@class='Chapter_Number')]" 
								mode="isolatedSpan"/>
							<!-- Then handle the spans associated with verses. -->
							<xsl:apply-templates select="xhtml:span[@class='Verse_Number']">
								<xsl:with-param name="chapterNumber" select="$chapterNumber"/>
							</xsl:apply-templates>
						</xsl:when>
						<!-- Otherwise handle paragraphs which do not contain verses nor a chapter. -->
						<xsl:otherwise>
							<xsl:apply-templates select="*"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:element> <!-- p -->
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template> <!-- xhtml:div[@class='Paragraph' or @class='Paragraph_Continuation' or @class='Line1'] -->

	<xsl:template match="xhtml:div[@class='Paragraph' or @class='Line1' or @class='Line2' or @class='Paragraph_Continuation']/xhtml:span[not(@class)]">
		<xsl:value-of select="text()"/>
	</xsl:template>

    <xsl:template match="xhtml:span[@class='Verse_Number']">
		<xsl:variable name="this-id" select="generate-id(.)"/>
		<xsl:element name="span">
			<xsl:attribute name="class">
				<xsl:text>verse </xsl:text>
				<xsl:value-of select="$bookCode"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="$chapterNumber"/>
				<xsl:text>_</xsl:text>
				<xsl:value-of select="text()"/>
			</xsl:attribute>
			<xsl:element name="strong">
				<xsl:value-of select="text()"/>
			</xsl:element>
			<xsl:text> </xsl:text>
			<!-- Handle footnotes. -->
			<xsl:apply-templates select="following-sibling::xhtml:span[not(@class) or @class='Inscription' or @class='scrFootnoteMarker' or @class='Words_Of_Christ']
					[$this-id = generate-id(preceding-sibling::xhtml:span[@class='Verse_Number'][1])]"/>
		</xsl:element> <!-- span -->
    </xsl:template> <!-- xhtml:span[@class='Verse_Number'] -->

	<xsl:template match="xhtml:span[@class='scrFootnoteMarker']">
		<xsl:element name="span">
			<xsl:attribute name="class">trans</xsl:attribute>
			<xsl:attribute name="title">
				<xsl:value-of select="following-sibling::xhtml:span[@class='Note_General_Paragraph'][1]/xhtml:span/text()"/>
			</xsl:attribute>
			<xsl:value-of select="following-sibling::xhtml:span[@class='Note_General_Paragraph'][1]/@title"/>
		</xsl:element> <!-- span -->
	</xsl:template> <!-- xhtml:span[@class='scrFootnoteMarker'] -->

	<xsl:template match="xhtml:span[@class='Words_Of_Christ']">
		<xsl:element name="span">
			<xsl:attribute name="class">wordsofchrist</xsl:attribute>
			<xsl:value-of select="text()"/>
		</xsl:element> <!-- span -->
	</xsl:template> <!-- xhtml:span[@class='Words_Of_Christ'] -->

	<!-- Get the Osis book code that corresponds to the given TE book code. -->
	<!-- xsl:template name="getOsisBookCode">
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
	</xsl:template --> <!-- getOsisBookCode -->

	<!-- Skip the div if the class is 'Parallel_Passage_Reference' or 'pictureCenter'.
		We see no evidence of these being used by YouVersion.com. -->
	<xsl:template match="xhtml:div[@class='Parallel_Passage_Reference']"/>
    <xsl:template match="xhtml:div[@class='pictureCenter']"/>
	<!-- Skip the span if the class is 'See_In_Glossary'. This is not used by YouVersion. -->
    <xsl:template match="xhtml:span[@class='See_In_Glossary']"/>

    <!-- Skip the following elements, which are already processed. -->
    <!-- xhtml:span[@class='scrBookName'] is handled via the variable 'bookName'. -->
    <xsl:template match="xhtml:p[@class='scrBookName']"/>
    <!-- xhtml:span[@class='scrBookCode'] is handled via the variable 'bookCode'. -->
    <xsl:template match="xhtml:p[@class='scrBookCode']"/>
    
    <!-- Special handling of text. -->
    <xsl:template match="text()">
		<!-- Replace curly quotes with straight quotes. -->
		<xsl:value-of select="translate(.,'“”','&quot;&quot;')"/>
    </xsl:template>

	<!-- Default element and attribute templates. -->
	<xsl:template match="*">
		<xsl:comment>Warning :: The element "<xsl:value-of select="name()"/> [class='<xsl:value-of select="@class"/>']", child of "<xsl:value-of select="name(..)"/> [class='<xsl:value-of select="../@class"/>']", has no matching template.</xsl:comment>
	</xsl:template>
	<!-- default attribute template -->
	<xsl:template match="@*">
		<xsl:comment>Warning :: The attribute "<xsl:value-of select="name()"/>" for element "<xsl:value-of select="name(..)"/>" has no matching template.</xsl:comment>
	</xsl:template>

</xsl:stylesheet>
