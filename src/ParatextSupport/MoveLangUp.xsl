<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	version="1.0">

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no"
		doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"/>

	<!-- The templates matching node() and @* match and copy unhandled elements/attributes. -->
	<xsl:template match="node()|@*">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()"/>
		</xsl:copy>
	</xsl:template>

	<!-- Paratext files are all a single language, put the first one on the body -->
	<xsl:template match="*[@class='scrBody']">
		<xsl:copy>
			<xsl:apply-templates select="@*"/>
			<xsl:attribute name="lang">
				<xsl:value-of select=".//*[@lang][1]/@lang"/>
			</xsl:attribute>
			<xsl:apply-templates select="node()"/>
		</xsl:copy>
	</xsl:template>

	<!-- Remove @lang from lower tags -->
	<xsl:template match="@lang"/>

	<!-- Remove spans that only have a language attribute -->
	<xsl:template match="*[local-name()='span' and not (@class)]">
		<xsl:apply-templates select="node()"/>
	</xsl:template>
</xsl:stylesheet>