<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        FixEpub.xsl
    # Purpose:     Fixes for Epub to pass check
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2013/02/19
    # Copyright:   (c) 2013 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml"
    version="1.0">

    <xsl:variable name="xhtml">http://www.w3.org/1999/xhtml</xsl:variable>

    <xsl:output method="xml"  doctype-public="-//W3C//DTD XHTML 1.1//EN"
        doctype-system="http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"/>

    <!-- Recursive copy template -->
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="@lang">
        <xsl:attribute name="xml:lang">
            <xsl:value-of select="."/>
        </xsl:attribute>
    </xsl:template>

    <xsl:template match="@id">
        <xsl:attribute name="id">
            <xsl:variable name="apos">&apos;</xsl:variable>
            <xsl:value-of select="translate(translate(.,$apos,''),' ','_')"/>
        </xsl:attribute>
    </xsl:template>

    <xsl:template match="@entryguid">
        <!-- Omit the attribute entryguid -->
    </xsl:template>

    <xsl:template match="@href">
        <xsl:attribute name="href">
            <xsl:variable name="apos">&apos;</xsl:variable>
            <xsl:value-of select="translate(translate(.,$apos,''),' ','_')"/>
        </xsl:attribute>
    </xsl:template>

    <xsl:template match="@onclick">
        <xsl:attribute name="onclick">
            <xsl:text>document.getElementById('</xsl:text>
            <xsl:variable name="arg" select="substring(.,26,string-length(.)-34)"/>
            <xsl:variable name="apos">&apos;</xsl:variable>
            <xsl:variable name="argWoSpQt" select="translate(translate($arg,$apos,''),' ','_')"/>
            <xsl:choose>
                <xsl:when test="contains($argWoSpQt, '-')">
                    <xsl:value-of select="substring-before($argWoSpQt, '-')"/>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select="$argWoSpQt"/>
                </xsl:otherwise>
            </xsl:choose>
            <xsl:text>').play()</xsl:text>
        </xsl:attribute>
    </xsl:template>

    <xsl:template match="/xhtml:html/xhtml:body/xhtml:span"></xsl:template>
</xsl:stylesheet>