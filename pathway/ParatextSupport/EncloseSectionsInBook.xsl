<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml"
    exclude-result-prefixes="xhtml"
    xmlns="http://www.w3.org/1999/xhtml">

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="yes"
        doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>

	<!-- Copy all content that isn't explicitly processed by templates. -->
	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
		</xsl:copy>
	</xsl:template>

	<!-- Remove text (empty lines) at the root level. -->
	<xsl:strip-space elements="*"/>

	<!-- Remove duplicated divisions. -->
	<xsl:template match="xhtml:div"/>

	<!-- Move div elements under the scrBook div element -->
	<xsl:template match="xhtml:div[@class='scrBook']">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()"/>
			<xsl:apply-templates select="following-sibling::*[1]" mode="scrBook"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="xhtml:div" mode="scrBook">
		<xsl:copy-of select="."/>
		<xsl:apply-templates select="following-sibling::*[1]" mode="scrBook"/>
	</xsl:template>

	<!-- Stop processing when another scrBook div element is encountered. -->
	<xsl:template match="xhtml:div[@class='scrBook']" mode="scrBook"/>
</xsl:stylesheet>