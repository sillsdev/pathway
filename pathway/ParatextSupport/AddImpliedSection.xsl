<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xhtml="http://www.w3.org/1999/xhtml"
	exclude-result-prefixes="xhtml"
	xmlns="http://www.w3.org/1999/xhtml">

	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="yes"
				doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"
				doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd" />

	<!-- Copy all content that isn't explicitly processed by templates. -->
	<xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>

	<!-- Find the scrIntroSection -->
	<xsl:template match="xhtml:div[@class='scrIntroSection']">
		<div class="{@class}">
		<!-- REVIEW: In cases where there isn't a mapping with the standard format styles, this may not work as expected 
			(e.g. an unmapped intro sfm begins with "i" instead of "Intro_"). -->
		<xsl:for-each select="xhtml:div">
			<div class="@class" />
			<xsl:apply-templates select="." />
			<xsl:choose>
				<xsl:when test="substring(div/@class, 1, 6) = 'Intro_'">
					<xsl:comment>Move all paras from this element inside the following div class=scrSection with a new section</xsl:comment>
					<div class="{@class}">
						First para in list to move.
					</div>
				</xsl:when>
				<xsl:otherwise>
					<div class="{@class}">
						Not moving para
					</div>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
		</div>
	</xsl:template>
	
</xsl:stylesheet>
