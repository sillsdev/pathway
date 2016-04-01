<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        ContentCss-Xslt.xsl
    # Purpose:     Convert Content CSS XML to XSLT
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2016/3/11
    # Copyright:   (c) 2016 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:x="http://www.w3.org/1999/xhtml"
    version="1.0">

    <xsl:output encoding="UTF-8" method="xml" indent="yes"/>

    <!-- Recursive template -->
    <xsl:template match="node() | @*">
        <xsl:apply-templates select="node() | @*"/>
    </xsl:template>

    <xsl:template match="/">
        <xsl:text disable-output-escaping="yes"><![CDATA[
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:x="http://www.w3.org/1999/xhtml" version="1.0">
]]></xsl:text>
            <xsl:apply-templates/>
        <xsl:text disable-output-escaping="yes"><![CDATA[
</xsl:stylesheet>
]]></xsl:text>
    </xsl:template>

    <xsl:template match="RULE[.//name[text()='content']]">
        <xsl:element name="xsl:template">
            <xsl:attribute name="match">
                <xsl:apply-templates select="child::*[1]" mode="sel"/>
                <xsl:if test="child::PSEUDO[name='last-child']">
                    <xsl:text>[last()]</xsl:text>
                </xsl:if>
                <xsl:if test="child::PSEUDO[name='first-child']">
                    <xsl:text>[first()]</xsl:text>
                </xsl:if>
            </xsl:attribute>
            <xsl:if test="child::PSEUDO/name='after'">
                <xsl:element name="xsl:copy">
                    <xsl:element name="xsl:apply-templates">
                        <xsl:attribute name="select">node() | @*</xsl:attribute>
                    </xsl:element>
                </xsl:element>
            </xsl:if>
            <xsl:element name="xsl:element">
                <xsl:attribute name="name">span</xsl:attribute>
                <xsl:attribute name="namespace">http://www.w3.org/1999/xhtml</xsl:attribute>
                <xsl:element name="xsl:attribute">
                    <xsl:attribute name="name">class</xsl:attribute>
                    <xsl:value-of select="@lastClass"/>
                    <xsl:text>-ps</xsl:text>
                </xsl:element>
                <xsl:variable name="val" select="PROPERTY[name='content']/value"/>
                <xsl:variable name="qt" select="substring($val,1,1)"/>
                <xsl:element name="xsl:text">
                    <xsl:value-of select="substring-before(substring-after($val,$qt),$qt)"/>
                </xsl:element>
            </xsl:element>
            <xsl:if test="child::PSEUDO/name='before'">
                <xsl:element name="xsl:copy">
                    <xsl:element name="xsl:apply-templates">
                        <xsl:attribute name="select">node() | @*</xsl:attribute>
                    </xsl:element>
                </xsl:element>
            </xsl:if>
        </xsl:element>
    </xsl:template>

    <xsl:template match="CLASS | TAG" mode="sel">
        <xsl:if test="not(local-name(following-sibling::*[1])='PRECEDES')">
            <xsl:apply-templates select="." mode="des"/>
            <xsl:if test="local-name(preceding-sibling::*[1])='PRECEDES'">
                <xsl:text>[preceding-sibling::</xsl:text>
                <xsl:apply-templates select="preceding-sibling::*[2]" mode="des"/>
                <xsl:text>]</xsl:text>
            </xsl:if>
            <xsl:call-template name="struct"/>
        </xsl:if>
        <xsl:apply-templates select="following-sibling::*[1]" mode="sel"/>
    </xsl:template>

    <xsl:template match="CLASS" mode="des">
        <xsl:text>*[@class='</xsl:text>
        <xsl:value-of select="name"/>
        <xsl:text>']</xsl:text>
    </xsl:template>

    <xsl:template match="TAG" mode="des">
        <xsl:text>x:</xsl:text>
        <xsl:value-of select="name"/>
    </xsl:template>

    <xsl:template name="struct">
        <xsl:variable name="nextNode" select="local-name(following-sibling::*[1])"/>
        <xsl:choose>
            <xsl:when test="$nextNode='PARENTOF'">
                <xsl:text>/</xsl:text>
            </xsl:when>
            <xsl:when test="$nextNode='PSEUDO'"/>
            <xsl:when test="$nextNode='PROPERTY'"/>
            <xsl:otherwise>//</xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <xsl:template match="PARENTOF | PRECEDES" mode="sel">
        <xsl:apply-templates select="following-sibling::*[1]" mode="sel"/>
    </xsl:template>

    <xsl:template match="PSEUDO | PROPERTY" mode="sel"/>

</xsl:stylesheet>