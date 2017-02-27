<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Remove Invalid Letter Dividers.xsl
    # Purpose:     Punctuation before letters or ligatures add extra dividers
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2017/02/27
    # Copyright:   (c) 2017 SIL International
    # Licence:     <MIT>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
    xmlns:x="http://www.w3.org/1999/xhtml">

    <xsl:param name="badChar">[?'"&#x201c;&#x201d;&#x201f;&#x2018;&#x2019;</xsl:param>

    <!-- Recursive copy template (all except xml:space attributes) -->
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="*[@class='letHead']">
        <xsl:variable name="char1" select="substring(*,1,1)"/>
        <xsl:variable name="char2" select="substring((following-sibling::*[1]//text())[1],2,1)"/>
        <xsl:variable name="charOe" select="substring((following-sibling::*[1]//text())[1],1,1)"/>
        <xsl:variable name="prevLetHead" select="preceding-sibling::*[@class='letHead'][1]"/>
        <xsl:variable name="prevHeadword" select="preceding-sibling::*[1]"/>
        <xsl:variable name="prevChar1" select="substring($prevHeadword,1,1)"/>
        <xsl:variable name="prevChar2" select="substring($prevHeadword,2,1)"/>
        <xsl:choose>
            <xsl:when test="contains($badChar, $char1) and contains($prevLetHead, $char2)"/>
            <xsl:when test="contains($badChar, $char1)">
                <xsl:copy>
                    <xsl:apply-templates select="@*"/>
                    <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                        <xsl:apply-templates select="*/@*"/>
                        <xsl:value-of select="translate($char2,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
                        <xsl:text> </xsl:text>
                        <xsl:value-of select="$char2"/>
                    </xsl:element>
                </xsl:copy>
            </xsl:when>
            <xsl:when test="*/text() = preceding-sibling::*[@class='letHead']/*/text()"/>
            <xsl:when test="count($prevHeadword) > 0 and contains($badChar, $prevChar1) and contains(*/text(), $prevChar2)"/>
            <xsl:when test="$charOe = 'œ' and count($prevHeadword) > 0 and $prevChar1 = 'o'"/>
            <xsl:when test="$charOe = 'œ'">
                <xsl:copy>
                    <xsl:apply-templates select="@*"/>
                    <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                        <xsl:apply-templates select="*/@*"/>
                        <xsl:text>O o</xsl:text>
                    </xsl:element>
                </xsl:copy>
            </xsl:when>
            <xsl:when test="count($prevHeadword) > 0 and $prevChar1 = 'œ' and $char1 = 'O'"/>
            <xsl:otherwise>
                <xsl:copy>
                    <xsl:apply-templates select="node() | @*"/>
                </xsl:copy>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

</xsl:stylesheet>