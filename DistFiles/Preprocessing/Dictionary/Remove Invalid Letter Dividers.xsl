<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Remove Invalid Letter Dividers.xsl
    # Purpose:     Reverse questionmark and other letter dividers can be removed
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2017/08/09
    # Copyright:   (c) 2017 SIL International
    # Licence:     <MIT>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	version="1.0">

	<xsl:param name="remove">[?'"&#x201c;&#x201d;&#x201f;&#x2018;&#x2019;&#x00BF;</xsl:param>

	<!-- Recursive copy template (all except xml:space attributes) -->
	<xsl:template match="node() | @*">
		<xsl:if test="not(@class='letHead' or @class='letData')">
			<xsl:copy>
				<xsl:apply-templates select="node() | @*"/>
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<xsl:template match="*[@class='letHead']">
		<xsl:variable name="char1" select="substring(.//text()[1],1,1)"/>
		<xsl:if test="not(contains($remove,$char1)) or not(preceding-sibling::*[@class='letHead'])">
			<xsl:copy>
				<xsl:apply-templates select="node() | @*"/>
			</xsl:copy>
			<xsl:apply-templates select="following-sibling::*[1]" mode="data"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="*" mode="data">
		<xsl:copy>
			<xsl:apply-templates select="@*"/>
			<xsl:call-template name="CopyData">
				<xsl:with-param name="node" select="."/>
				<xsl:with-param name="letter" select="string(preceding-sibling::*[1]//text())"/>
			</xsl:call-template>
		</xsl:copy>
	</xsl:template>

	<xsl:template name="CopyData">
		<xsl:param name="node"/>
		<xsl:param name="letter"/>
		<xsl:apply-templates select="$node/node()"/>
		<xsl:variable name="followingHeader" select="string($node/following-sibling::*[1]//text())"/>
		<xsl:variable name="folHeadChar" select="substring($followingHeader,1,1)"/>
		<xsl:if test="normalize-space($followingHeader)!='' and (contains($remove, $folHeadChar) or $followingHeader=$letter)">
			<xsl:call-template name="CopyData">
				<xsl:with-param name="node" select="$node/following-sibling::*[2]"/>
				<xsl:with-param name="letter" select="$letter"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>