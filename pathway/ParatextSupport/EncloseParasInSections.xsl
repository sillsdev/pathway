<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xhtml="http://www.w3.org/1999/xhtml"
	exclude-result-prefixes="xhtml"
	xmlns="http://www.w3.org/1999/xhtml">
	
	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="yes" 
				doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"/>

	<!-- Copy all content that isn't explicitly processed by templates. -->
	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()"/>
		</xsl:copy>
	</xsl:template>
	
	<!-- Remove text (empty lines) at the root level. -->
	<xsl:strip-space elements="*"/>
	
	<!-- Create section divisions -->
	<xsl:template match="xhtml:h1">
		<xsl:choose>
			<xsl:when test="@class = 'Intro_Section_Head' or @class = 'Section_Head' or @class = 'Chapter_Head' or 
					@class = 'Hebrew_Title' or @class = 'Parallel_Passage_Reference' or @class = 'Section_Head_Major' or 
					@class = 'Section_Head_Minor' or @class = 'Section_Head_Series' or @class = 'Section_Range_Paragraph' or 
					@class = 'Speech_Speaker' or @class = 'Variant_Section_Head'">
						<xsl:call-template name="Section_Head"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:comment>Unrecognized Heading: {@class}</xsl:comment>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Copy heading and content paragraphs that follow a section head. -->
	<xsl:template match="xhtml:h1" mode="Section_Head">
		<div xmlns="http://www.w3.org/1999/xhtml">
			<xsl:apply-templates select="@*|node()"/>
		</div>
		
		<xsl:apply-templates select="following-sibling::*[1][self::xhtml:h1 or self::xhtml:p]" 
			mode="Section_Head"/>
	</xsl:template>
	
	<!-- When a Scripture content paragraph is preceded by an introduction content paragraph, 
		create a new section. -->
	<xsl:template match="xhtml:p[not(starts-with(@class, 'Intro_'))]
		[preceding-sibling::*[1][self::xhtml:p[starts-with(@class, 'Intro_')]]]">
		<xsl:call-template name="Section_Head"/>
	</xsl:template>
	
	<xsl:template match="xhtml:p" mode="Section_Head">
		<div xmlns="http://www.w3.org/1999/xhtml">
			<xsl:apply-templates select="@*|node()"/>
		</div>
		<xsl:choose>
			<xsl:when test="starts-with(@class, 'Intro_') and 
				following-sibling::*[1][self::xhtml:p[not(starts-with(@class, 'Intro_'))]]"/>
			<xsl:otherwise>
				<xsl:apply-templates select="following-sibling::*[1][self::xhtml:p]" mode="Section_Head"/>
			</xsl:otherwise>
		</xsl:choose>		
	</xsl:template>

	<!-- When a paragraph is immediately preceded by a div rather than an h1, this is an indication
		that an explicit section head indication is absent, so we need to create a new section. -->
	<xsl:template match="xhtml:p[preceding-sibling::*[1][self::xhtml:div]]">
		<xsl:call-template name="Section_Head"/>
	</xsl:template>
	
	<xsl:template name="Section_Head">
		<xsl:variable name="class">
			<xsl:choose>
				<xsl:when test="starts-with(@class, 'Intro_')">
					<xsl:value-of select="'scrIntroSection'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'scrSection'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		
		<div class="{$class}" xmlns="http://www.w3.org/1999/xhtml">
			<xsl:apply-templates select="." mode="Section_Head"/>
		</div>
	</xsl:template>
	
	<!-- Remove heading and content nodes at the root level. -->
	<!-- Only delete headings that are preceded by another heading. -->
	<xsl:template match="xhtml:h1[preceding-sibling::*[1][self::xhtml:h1]]"/>
	<xsl:template match="xhtml:p"/>
</xsl:stylesheet>
