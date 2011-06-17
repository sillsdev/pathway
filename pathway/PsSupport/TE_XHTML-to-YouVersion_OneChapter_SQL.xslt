<?xml version="1.0" encoding="UTF-8"?>
<!-- Transform one chapter from XHTML to HTML used by YouVersion. -->

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml"
    xmlns:fn="http://www.w3.org/2005/xpath-functions"
    exclude-result-prefixes="xhtml">

	<xsl:output method="text" encoding="UTF-8" />

    <xsl:strip-space elements="*"/>

	<!-- Use a key to speed up the processing of the spans for each verse.
			These spans may come from more than one paragraph.-->
	<xsl:key name="spans-by-verse"
			match="xhtml:span[(not(@class) or @class='Inscription' or @class='Words_Of_Christ') and
							not(parent::xhtml:div[@class='Section_Head']) and
							not(parent::xhtml:span[@class='Note_General_Paragraph'])]"
			use="generate-id(preceding::xhtml:span[@class='Verse_Number'][1])" />

	<xsl:variable name="bookCode">
		<xsl:value-of select="/xhtml:html/xhtml:body/xhtml:p[@class='scrBookCode']/text()"/>
	</xsl:variable>

	<xsl:variable name="bookName">
		<xsl:value-of select="/xhtml:html/xhtml:body/xhtml:p[@class='scrBookName']/text()"/>
	</xsl:variable>

	<xsl:variable name="langAbbr">
		<xsl:value-of select="/xhtml:html/xhtml:body/xhtml:p[@class='scrBookName']/@lang"/>
	</xsl:variable>

	<xsl:variable name="chapterNumber">
		<xsl:value-of select="/xhtml:html/xhtml:body/xhtml:div[@class='scrSection']//xhtml:span[@class='Chapter_Number']/text()"/>
	</xsl:variable>

	<xsl:variable name="langBookChapt">
		<xsl:value-of select="$langAbbr"/>
		<xsl:text>_</xsl:text>
		<xsl:value-of select="$bookCode"/>
		<xsl:text>_</xsl:text>
		<xsl:value-of select="$chapterNumber"/>
	</xsl:variable>

    <!-- Process the top element, html. -->
    <xsl:template match="xhtml:html">
		<xsl:apply-templates/>
    </xsl:template>

	<!-- Skip the head element. -->
	<xsl:template match="xhtml:head"/>

    <xsl:template match="xhtml:body">
		<xsl:text>-- ----------------------------
--  Table structure for "</xsl:text>
		<xsl:value-of select="$langBookChapt"/>
		<xsl:text>";
-- ----------------------------
DROP TABLE IF EXISTS "</xsl:text>
		<xsl:value-of select="$langBookChapt"/>
		<xsl:text>";
CREATE TABLE "</xsl:text>
		<xsl:value-of select="$langBookChapt"/>
		<xsl:text>" (
	"id" int4 DEFAULT NULL,
	"version" char(6) DEFAULT NULL,
	"book" char(7) DEFAULT NULL,
	"verse" float4 DEFAULT NULL,
	"unformatted" text DEFAULT NULL,
	"idxfti" tsvector DEFAULT NULL,
	"basichtml" text DEFAULT NULL
)
WITH (OIDS=FALSE);
ALTER TABLE "</xsl:text>
		<xsl:value-of select="$langBookChapt"/>
		<xsl:text>" OWNER TO "root";

-- ----------------------------
--  Records of "</xsl:text>
		<xsl:value-of select="$langBookChapt"/>
		<xsl:text>"
-- ----------------------------
BEGIN;</xsl:text>
		<!-- Process the verses. -->
		<xsl:apply-templates select="//xhtml:span[@class='Verse_Number']"/>
		<xsl:text>
COMMIT;</xsl:text>
    </xsl:template> <!-- xhtml:body -->

    <xsl:template match="xhtml:span[@class='Verse_Number']">
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
		</xsl:variable> <!-- verseNumber -->
		<!-- Collect the spans within this paragraph that apply to this verse. -->
		<xsl:variable name="verseSpans" select="key('spans-by-verse',$this-id)"/>
		<!-- Write out the record. -->
		<xsl:text>
INSERT INTO "</xsl:text>
		<xsl:value-of select="$langBookChapt"/>
		<xsl:text>" ("version", "book", "verse", "unformatted", "idxfti", "basichtml") VALUES ('</xsl:text>
		<xsl:value-of select="$langAbbr"/>
		<xsl:text>', '</xsl:text>
		<xsl:value-of select="$bookCode"/>
		<xsl:text>', '</xsl:text>
		<xsl:value-of select="$chapterNumber"/>
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
		<xsl:text>', '', '</xsl:text>
		<!-- Handle the spans for this verse in the "basichtml" form. -->
		<!-- First look for a section head that applies to this span. -->
		<xsl:if test="not(preceding-sibling::xhtml:span[not(@class)])
					and parent::xhtml:div/preceding-sibling::xhtml:div[1][@class='Section_Head']">
			<xsl:text>&lt;h2&gt;</xsl:text>
			<xsl:value-of select="parent::xhtml:div /
						preceding-sibling::xhtml:div[1][@class='Section_Head']/xhtml:span/text()"/>
			<xsl:text>&lt;/h2&gt;</xsl:text>
		</xsl:if>
		<xsl:text>&lt;span class="verse" id="</xsl:text>
		<xsl:value-of select="$bookCode"/>
		<xsl:text>_</xsl:text>
		<xsl:value-of select="$chapterNumber"/>
		<xsl:text>_</xsl:text>
		<xsl:value-of select="$verseNumber"/>
		<xsl:text>"&gt;&lt;strong class="verseno"&gt;</xsl:text>
		<xsl:value-of select="$verseNumber"/>
		<xsl:text>&lt;/strong&gt;&amp;nbsp;</xsl:text>
		<xsl:apply-templates select="$verseSpans" mode="basichtml"/>
		<xsl:text>&lt;/span&gt;');</xsl:text>
    </xsl:template> <!-- xhtml:span[@class='Verse_Number'] -->

	<xsl:template match="xhtml:span" mode="unformatted">
		<xsl:for-each select="text()">
			<xsl:copy/>
			<xsl:if test="not(position()=last())">
				<xsl:element name="br"/>
			</xsl:if>
		</xsl:for-each>
		<!-- If this is the end of the section, but not the end of the file, add a carriage return. -->
		<xsl:if test="not(following-sibling::xhtml:span[not(@class)]) and
						not(parent::xhtml:div/following-sibling::xhtml:div) and
						parent::xhtml:div/parent::xhtml:div/following-sibling[xhtml:div[@class='scrSection']]">
			<xsl:text>
</xsl:text>
		</xsl:if>
		<!-- If this is the end of the paragraph, add a carriage return. -->
		<xsl:if test="not(following-sibling::xhtml:span[not(@class)]) and
						(not(parent::xhtml:div/following-sibling::xhtml:div) or
							parent::xhtml:div/following-sibling::xhtml:div[1][@class='Paragraph' or @class='Line1' or
												@class='Line2' or @class='Paragraph_Continuation'])">
			<xsl:text>
</xsl:text>
		</xsl:if>
	</xsl:template> <!-- xhtml:span mode="unformatted" -->

	<xsl:template match="xhtml:span" mode="basichtml">
		<!-- First look for a section head that is within this span. -->
	<xsl:choose>
			<xsl:when test="not(preceding-sibling::xhtml:span[@class='Verse_Number'])
						and parent::xhtml:div/preceding-sibling::xhtml:div[1][@class='Section_Head']">
				<xsl:text>&lt;h2&gt;</xsl:text>
				<xsl:value-of select="parent::xhtml:div /
						preceding-sibling::xhtml:div[1][@class='Section_Head']/xhtml:span/text()"/>
				<xsl:text>&lt;/h2&gt;</xsl:text>
			</xsl:when>
		</xsl:choose>
		<xsl:for-each select="text()">
			<xsl:copy/>
			<xsl:if test="not(position()=last())">
				<xsl:element name="br"/>
			</xsl:if>
		</xsl:for-each>
		<!-- If this is the end of the section, but not the end of the file, add a carriage return. -->
		<xsl:if test="not(following-sibling::xhtml:span[not(@class)]) and
						not(parent::xhtml:div/following-sibling::xhtml:div) and
						parent::xhtml:div/parent::xhtml:div/following-sibling[xhtml:div[@class='scrSection']]">
			<xsl:text>
</xsl:text>
		</xsl:if>
		<!-- If this is the end of the paragraph, add an empty paragraph element. -->
		<xsl:if test="not(following-sibling::xhtml:span[not(@class)]) and
						(not(parent::xhtml:div/following-sibling::xhtml:div) or
							parent::xhtml:div/following-sibling::xhtml:div[1][@class='Paragraph' or @class='Line1' or
												@class='Line2' or @class='Paragraph_Continuation'])">
			<xsl:choose>
				<!-- When the next div has the class "Paragraph", add an empty paragraph element. -->
				<xsl:when test="parent::xhtml:div/following-sibling::xhtml:div[1][@class='Paragraph']">
					<xsl:text>&lt;p&gt; &lt;/p&gt;</xsl:text>
				</xsl:when>
				<!-- Otherwise add a carriage return. -->
				<xsl:otherwise>
					<xsl:text>
</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template> <!-- xhtml:span mode="basichtml" -->

	<!-- Skip the div if the class is 'Parallel_Passage_Reference' or 'pictureCenter'.
		We see no evidence of these being used by YouVersion.com. -->
	<xsl:template match="xhtml:div[@class='Parallel_Passage_Reference']"/>
<!--    <xsl:template match="xhtml:div[@class='pictureCenter']"/> -->
	<xsl:template match="xhtml:span[@class='Note_General_Paragraph']"/>

<!--	<xsl:template match="xhtml:div[@class='Paragraph' or @class='Paragraph_Continuation' or @class='Line1' or @class='Line2']"> -->

<!--	<xsl:template match="xhtml:div[@class='Paragraph' or @class='Line1' or @class='Line2' or @class='Paragraph_Continuation']/xhtml:span[not(@class)]"> -->
<!--	<xsl:template match="xhtml:div[@class='Paragraph']/xhtml:span[not(@class)]"> -->
		<!-- Replace curly quotes with straight quotes. -->
	<!--	<xsl:variable name="var1">
			<xsl:value-of select="translate(.,'“”','&quot;&quot;')" disable-output-escaping="yes"/>
		</xsl:variable>
		<xsl:call-template name="handleSpecialChars">
			<xsl:with-param name="string" select="$var1"/>
		</xsl:call-template>
		<xsl:value-of select="text()"/>
	</xsl:template> -->

	<!-- Footnotes are ignored for the YouVersion SQL files. -->
<!--	<xsl:template match="xhtml:span[@class='scrFootnoteMarker']">
		<xsl:element name="span">
			<xsl:attribute name="class">trans</xsl:attribute>
			<xsl:attribute name="title">
				<xsl:value-of select="following-sibling::xhtml:span[@class='Note_General_Paragraph'][1]/xhtml:span/text()"/>
			</xsl:attribute>
			<xsl:value-of select="following-sibling::xhtml:span[@class='Note_General_Paragraph'][1]/@title"/>
		</xsl:element> <- span ->
	</xsl:template> <- xhtml:span[@class='scrFootnoteMarker'] -->

	<!-- xsl:template match="xhtml:span[@class='Words_Of_Christ']">
		<xsl:element name="span">
			<xsl:attribute name="class">wordsofchrist</xsl:attribute>
			<xsl:value-of select="text()"/>
		</xsl:element> <- span ->
	</xsl:template> <- xhtml:span[@class='Words_Of_Christ'] -->
	<!-- Skip the div if the class is 'Parallel_Passage_Reference' or 'pictureCenter'.
		We see no evidence of these being used by YouVersion.com. -->
	<xsl:template match="xhtml:div[@class='Parallel_Passage_Reference']"/>
<!--    <xsl:template match="xhtml:div[@class='pictureCenter']"/> -->

	<!-- Skip the span if the class is 'See_In_Glossary'. This is not used by YouVersion. -->
<!--    <xsl:template match="xhtml:span[@class='See_In_Glossary']"/> -->

    <!-- Skip the following elements, which are already processed. -->
    <!-- xhtml:span[@class='scrBookName'] is handled via the variable 'bookName'. -->
    <xsl:template match="xhtml:p[@class='scrBookName']"/>
    <!-- xhtml:span[@class='scrBookCode'] is handled via the variable 'bookCode'. -->
    <xsl:template match="xhtml:p[@class='scrBookCode']"/>
    <!-- The chapter number is captured in the variable, 'chapterNumber'. -->
    <xsl:template match="xhtml:span[@class='Chapter_Number']"/>
    
    <!-- Special handling of text. -->
    <!-- xsl:template match="text()"> -->
		<!-- Replace curly quotes with straight quotes. -->
	<!--	<xsl:variable name="var1">
			<xsl:value-of select="translate(.,'“”','&quot;&quot;')" disable-output-escaping="yes"/>
		</xsl:variable>
		<xsl:call-template name="handleSpecialChars">
			<xsl:with-param name="string" select="$var1"/>
		</xsl:call-template>
    </xsl:template> -->

	<!-- Note: the ampersand character is '&#160;'. -->
	<xsl:template name="handleSpecialChars">
		<xsl:param name="string"/>
		<xsl:variable name="substringBeforeLT" select="substring-before($string,'&lt;')"/>
		<xsl:variable name="substringBeforeGT" select="substring-before($string,'&gt;')"/>
		<xsl:choose>
			<!-- When there is not a '&lt;'. -->
			<xsl:when test="string-length($substringBeforeLT)=0">
				<xsl:choose>
					<!-- If there is also not a '&gt;', just return the string. -->
					<xsl:when test="string-length($substringBeforeGT)=0">
						<xsl:value-of select="$string"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$substringBeforeGT"/>
						<xsl:text disable-output-escaping="yes">&#160;gt;</xsl:text>
						<xsl:call-template name="handleSpecialChars">
							<xsl:with-param name="string" select="substring-after($string,'&gt;')"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<!-- When there is not a '&gt;'. -->
			<xsl:when test="string-length($substringBeforeGT)=0">
				<xsl:choose>
					<!-- If there is also not a '&lt;', just return the string. -->
					<xsl:when test="string-length($substringBeforeLT)=0">
						<xsl:value-of select="$string"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$substringBeforeLT"/>
						<xsl:text disable-output-escaping="yes">&#160;lt;</xsl:text>
						<xsl:call-template name="handleSpecialChars">
							<xsl:with-param name="string" select="substring-after($string,'&lt;')"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<!-- Handle the shorter substring first. -->
				<xsl:choose>
					<xsl:when test="string-length(substringBeforeLT) &lt; string-length(substringBeforeGT)">
						<xsl:value-of select="$substringBeforeLT"/>
						<xsl:text disable-output-escaping="yes">&lt;</xsl:text>
						<xsl:call-template name="handleSpecialChars">
							<xsl:with-param name="string" select="substring-after($string,'&lt;')"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$substringBeforeGT"/>
						<xsl:text disable-output-escaping="yes">&gt;</xsl:text>
						<xsl:call-template name="handleSpecialChars">
							<xsl:with-param name="string" select="substring-after($string,'&gt;')"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template> <!-- handleSpecialChars -->

	<!-- Default element and attribute templates. -->
	<xsl:template match="*">
		<xsl:text>
Warning :: The element "</xsl:text><xsl:value-of select="name()"/><xsl:text> [class='</xsl:text><xsl:value-of select="@class"/><xsl:text>']", child of "</xsl:text><xsl:value-of select="name(..)"/><xsl:text> [class='</xsl:text><xsl:value-of select="../@class"/><xsl:text>']", has no matching template.</xsl:text>
	</xsl:template>
	<!-- default attribute template -->
	<xsl:template match="@*">
		<xsl:text>
Warning :: The attribute "</xsl:text><xsl:value-of select="name()"/><xsl:text>" for element "</xsl:text><xsl:value-of select="name(..)"/><xsl:text>" has no matching template.</xsl:text>
	</xsl:template>

</xsl:stylesheet>
