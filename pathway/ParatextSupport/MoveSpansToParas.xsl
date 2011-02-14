<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xhtml="http://www.w3.org/1999/xhtml"
	exclude-result-prefixes="xhtml"
	xmlns="http://www.w3.org/1999/xhtml">

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="yes" 
				doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" />

	<!-- Copy all content that isn't explicitly processed by templates. -->
	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
		</xsl:copy>
	</xsl:template>

	<!-- Remove text (empty lines) at the root level. -->
	<xsl:strip-space elements="*"/>
	
	<!-- Move any spans that are immediate children of <body> to a new paragraph. -->
	<xsl:template match="xhtml:span[ancestor::*[1][self::xhtml:body]]">
		<!-- Beginning with the first span in a (potential) series... -->
		<xsl:if test="not(preceding-sibling::*[1][self::xhtml:span])">
			<p class="Paragraph" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:apply-templates select="." mode="MoveSpansToPara" />
			</p>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="xhtml:span" mode="MoveSpansToPara">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()"/>
		</xsl:copy>

		<!-- Move any following spans (without parent paragraphs) to this paragraph. -->
		<xsl:apply-templates select="following-sibling::*[1][self::xhtml:span]" mode="MoveSpansToPara" />
	</xsl:template>
</xsl:stylesheet>
