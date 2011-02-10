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
			<xsl:call-template name="MoveSpansToPara" />
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="MoveSpansToPara">
		<p class="Paragraph">
			<xsl:apply-templates select="." mode="MoveSpansToPara" />
		</p>
	</xsl:template>
	
	<xsl:template match="xhtml:span" mode="MoveSpansToPara">
		<!-- <xsl:comment>Moving span to paragraph</xsl:comment> -->
		<span>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates />
		</span>
		<xsl:choose>
			<xsl:when test="following-sibling::*[1][self::xhtml:span]">
				<!-- Move any following spans (without parent paragraphs) to this paragraph. -->
				<!-- <xsl:comment>Moving following-sibling to paragraph</xsl:comment> -->
				<xsl:apply-templates select="following-sibling::*[1][self::xhtml:span]" mode="MoveSpansToPara" />
			</xsl:when>
			<xsl:otherwise>
				<!-- <xsl:comment>Copying following-sibling</xsl:comment> -->
				<xsl:copy>
					<xsl:apply-templates select="@*|node()" />
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
