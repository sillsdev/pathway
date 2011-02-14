<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xhtml="http://www.w3.org/1999/xhtml"
	exclude-result-prefixes="xhtml"
	xmlns="http://www.w3.org/1999/xhtml">
	
	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="yes" 
				doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" />

	<!-- Copy all content that isn't explicitly processed by templates. -->
	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
		</xsl:copy>
	</xsl:template>

	<!-- Remove text (empty lines) at the root level. -->
	<xsl:strip-space elements="*"/>
	
	<!-- Move scrSections at the <body> level to a new columns div. -->
	<xsl:template match="xhtml:div[@class = 'scrSection'][ancestor::*[1][self::xhtml:body]]">
		<xsl:choose>
			<xsl:when test="not(preceding-sibling::*[1][self::xhtml:div][@class = 'scrSection'])">
					<xsl:call-template name="MoveScrSectionToColumns" />
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="MoveScrSectionToColumns">
		<div class="columns">
			<xsl:apply-templates select="." mode="MoveScrSectionToColumns" />
		</div>
	</xsl:template>

	<xsl:template match="xhtml:div" mode="MoveScrSectionToColumns">
		<div class="{@class}">
			<xsl:apply-templates />
		</div>
		<xsl:if test="following-sibling::*[1][self::xhtml:div][@class = 'scrSection']">
			<!-- Move any subsequent scrSection divs to the columns div. -->
			<xsl:apply-templates select="following-sibling::*[1][self::xhtml:div][@class = 'scrSection']" mode="MoveScrSectionToColumns" />
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>