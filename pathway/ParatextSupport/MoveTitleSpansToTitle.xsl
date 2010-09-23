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

	<!-- Move any spans that are part of the title into the title div -->
	<xsl:template match="xhtml:div[@class='Title_Main']">
		<div class="Title_Main">
			<!-- Include any preceding secondary or tertiary titles in this title. -->
			<xsl:call-template name="MoveSpansBeforeTitle" />

			<!-- Include main title paragraph -->
			<xsl:apply-templates select="node()" />

			<!-- Include any following secondary or tertiary titles in this title. -->
			<xsl:call-template name="MoveSpansAfterTitle" />
		</div>
	</xsl:template>

	<xsl:template name="MoveSpansAfterTitle">
		<!--<xsl:comment>Called MoveSpansAfterTitle</xsl:comment>-->
		<xsl:if test="following-sibling::*[1][self::node()][@class='Title_Secondary'] or following-sibling::*[1][self::node()][@class='Title_Tertiary']">
			<span class="{following-sibling::*[1][self::node()]/@class}" lang="{following-sibling::*[1][self::node()]/@lang}" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:value-of select="following-sibling::*[1][self::node()]"/>
			</span>
		</xsl:if>
		<xsl:if test="following-sibling::*[2][self::node()][@class='Title_Secondary'] or following-sibling::*[2][self::node()][@class='Title_Tertiary']">
			<span class="{following-sibling::*[2][self::node()]/@class}" lang="{following-sibling::*[1][self::node()]/@lang}" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:value-of select="following-sibling::*[2][self::node()]"/>
			</span>
		</xsl:if>
	</xsl:template>

	<xsl:template name="MoveSpansBeforeTitle">
		<!-- Called MoveSpansBeforeTitle: using self::node() because self::span would not work -->
		<xsl:if test="preceding::*[2][self::node()][@class='Title_Secondary'] or preceding::*[2][self::node()][@class='Title_Tertiary']">
			<span class="{preceding::*[2][self::node()]/@class}" lang="{following-sibling::*[2][self::node()]/@lang}" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:value-of select="preceding::*[2][self::node()]"/>
			</span>
		</xsl:if>
		<xsl:if test="preceding::*[1][self::node()][@class='Title_Secondary'] or preceding::*[1][self::node()][@class='Title_Tertiary']">
			<span class="{preceding::*[1][self::node()]/@class}" lang="{following-sibling::*[1][self::node()]/@lang}" xmlns="http://www.w3.org/1999/xhtml">
				<xsl:value-of select="preceding::*[1][self::node()]"/>
			</span>
		</xsl:if>
	</xsl:template>

	<!-- Remove title spans from the <body> element. -->
	<xsl:template match="xhtml:span[@class='Title_Secondary'][ancestor::*[1][self::xhtml:body]]">
		<!--<xsl:comment>Deleting Secondary title span from body.</xsl:comment>-->
	</xsl:template>
	<xsl:template match="xhtml:span[@class='Title_Tertiary'][ancestor::*[1][self::xhtml:body]]">
		<!--<xsl:comment>Deleting Tertiary title span from body.</xsl:comment>-->
	</xsl:template>

</xsl:stylesheet>
