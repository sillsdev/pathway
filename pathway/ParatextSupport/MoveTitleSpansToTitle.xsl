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
	
	<!-- Remove chapter elements that were transformed to spans and moved inside <p> elements 
		in the previous transformation. -->
	<xsl:template match="chapter"/>
		
	<!-- Move any spans that are part of the title into the title div -->
	<xsl:template match="xhtml:div[@class='Title_Main']">
		<xsl:copy>
			<xsl:apply-templates select="@*"/>
			<!-- Include any preceding secondary or tertiary titles in this title. -->
			<xsl:apply-templates select="preceding-sibling::*[1][self::xhtml:span][@class='Title_Secondary' or @class='Title_Tertiary']" mode="preceding-sibling-span-title"/>

			<!-- Include main title paragraph -->
			<xsl:apply-templates select="node()"/>

			<!-- Include any following secondary or tertiary titles in this title. -->
			<xsl:apply-templates select="following-sibling::*[1][self::xhtml:span][@class='Title_Secondary' or @class='Title_Tertiary']" mode="following-sibling-span-title"/>
		</xsl:copy>
	</xsl:template>

	<!-- Move spans preceding the main title. -->
	<xsl:template match="xhtml:span" mode="preceding-sibling-span-title">
		<xsl:apply-templates select="preceding-sibling::*[1][self::xhtml:span][@class='Title_Secondary' or @class='Title_Tertiary']" mode="preceding-sibling-span-title"/>
		<xsl:copy-of select="."/>
	</xsl:template>

	<!-- Move spans following the main title. -->
	<xsl:template match="xhtml:span" mode="following-sibling-span-title">
		<xsl:copy-of select="."/>
		<xsl:apply-templates select="following-sibling::*[1][self::xhtml:span][@class='Title_Secondary' or @class='Title_Tertiary']" mode="following-sibling-span-title"/>
	</xsl:template>
	
	<!-- Remove title spans from the <body> element. -->
	<xsl:template match="xhtml:body/xhtml:span[@class='Title_Secondary' or @class='Title_Tertiary']"/>

</xsl:stylesheet>
