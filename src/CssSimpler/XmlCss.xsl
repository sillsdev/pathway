<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        XmlCss.xsl
    # Purpose:     Output Xml as Css
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2016/3/10
    # Copyright:   (c) 2016 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0">

    <xsl:output encoding="UTF-8" method="text"/>

    <!-- Recursive template -->
    <xsl:template match="node() | @*">
        <xsl:apply-templates select="node() | @*"/>
    </xsl:template>

    <xsl:template match="RULE">
        <xsl:if test="count(child::PROPERTY) > 0">
            <xsl:apply-templates select="*[local-name() != 'PROPERTY']" mode="inRule"/>
            <xsl:text>{</xsl:text>
            <xsl:apply-templates select="PROPERTY" mode="inRule"/>
            <xsl:text>&#13;&#10;}&#13;&#10;</xsl:text>
        </xsl:if>
    </xsl:template>

    <xsl:template match="TAG/name" mode="inRule">
        <xsl:value-of select="."/>
        <xsl:if test="not(following-sibling::*[1][local-name()='ATTRIB'] or parent::*/following-sibling::*[1][local-name()='CLASS' or local-name()='PSEUDO'])">
            <xsl:text> </xsl:text>
        </xsl:if>
    </xsl:template>

    <xsl:template match="CLASS/name" mode="inRule">
        <xsl:text>.</xsl:text>
        <xsl:value-of select="."/>
        <xsl:if test="not(following-sibling::*[1][local-name()='ATTRIB'] or parent::*/following-sibling::*[1][local-name()='PSEUDO'])">
            <xsl:text> </xsl:text>
        </xsl:if>
    </xsl:template>

    <xsl:template match="ATTRIB" mode="inRule">
        <xsl:text>[</xsl:text>
        <xsl:apply-templates select="*" mode="inRule"/>
        <xsl:text>]</xsl:text>
    </xsl:template>

    <xsl:template match="ATTRIB/name" mode="inRule">
        <xsl:value-of select="."/>
    </xsl:template>

    <xsl:template match="BEGINSWITH" mode="inRule">
        <xsl:text>|=</xsl:text>
    </xsl:template>

    <xsl:template match="PARENTOF" mode="inRule">
        <xsl:text disable-output-escaping="yes">&gt; </xsl:text>
    </xsl:template>

    <xsl:template match="PRECEDES" mode="inRule">
        <xsl:text>+ </xsl:text>
    </xsl:template>

    <xsl:template match="SIBLING" mode="inRule">
        <xsl:text>~ </xsl:text>
    </xsl:template>

    <xsl:template match="PSEUDO/name" mode="inRule">
        <xsl:text>:</xsl:text>
        <xsl:value-of select="."/>
    </xsl:template>

    <xsl:template match="PROPERTY" mode="inRule">
        <xsl:text>&#13;&#10;   </xsl:text>
        <xsl:value-of select="name"/>
        <xsl:text>:</xsl:text>
        <xsl:apply-templates select="*[local-name() != 'name']" mode="inRule"/>
        <xsl:text>;</xsl:text>
    </xsl:template>

    <xsl:template match="value" mode="inRule">
        <xsl:if test="not(starts-with(text(),'('))">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:value-of select="text()"/>
    </xsl:template>

    <xsl:template match="unit" mode="inRule">
        <xsl:if test="contains(text(),'serif')">
            <xsl:text>,</xsl:text>
        </xsl:if>
        <xsl:value-of select="text()"/>
    </xsl:template>

    <xsl:template match="text()" mode="inRule"/>
</xsl:stylesheet>