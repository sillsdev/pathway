<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        addDicTocHeads.xsl
    # Purpose:     Add Main and Reversal Index headers to Epub Dictionary TOC (NCX)
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2012/04/05
    # Copyright:   (c) 2012 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
    xmlns:ncx="http://www.daisy.org/z3986/2005/ncx/">
    <xsl:param name="mainName">Main</xsl:param>
    <xsl:param name="revName">Reversal Index</xsl:param>

    <!-- Recursive copy template -->
    <xsl:template match="ncx:ncx | ncx:head | ncx:meta | ncx:docTitle | ncx:text | ncx:navMap | ncx:navLabel | ncx:content | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>

    <!-- Add Main header -->
    <xsl:template match="ncx:navPoint">
        <xsl:choose>
            <!-- Test for beginning of main section -->
            <xsl:when test="count(ncx:content[starts-with(@src,'Part')]) = 1 and not(starts-with(preceding-sibling::*[1]//@src,'Part')) and starts-with(following-sibling::*//@src,'Rev')">
                <xsl:copy>
                    <xsl:apply-templates select="@*"/>
                    <xsl:element name="navLabel" namespace="http://www.daisy.org/z3986/2005/ncx/">
                        <xsl:element name="text" namespace="http://www.daisy.org/z3986/2005/ncx/">
                            <xsl:value-of select="$mainName"/>
                        </xsl:element>
                    </xsl:element>
                    <xsl:element name="content" namespace="http://www.daisy.org/z3986/2005/ncx/">
                        <xsl:attribute name="src">
                            <xsl:value-of select="ncx:content/@src"/>
                            <xsl:text>#body</xsl:text>
                        </xsl:attribute>
                    </xsl:element>
                    <xsl:call-template name="SubCopy">
                        <xsl:with-param name="item" select="."/>
                        <xsl:with-param name="type">Part</xsl:with-param>
                    </xsl:call-template>
                </xsl:copy>
            </xsl:when>
            
            <!-- There is a main section but no reversal -->
            <xsl:when test="count(ncx:content[starts-with(@src,'Part')]) = 1 and not(starts-with(preceding-sibling::*[1]//@src,'Part'))">
                <xsl:call-template name="SubCopy">
                    <xsl:with-param name="item" select="."/>
                    <xsl:with-param name="type">Part</xsl:with-param>
                </xsl:call-template>
            </xsl:when>
            
            <!-- Test for beginning of reversal section -->
            <xsl:when test="count(ncx:content[starts-with(@src,'Rev')]) = 1 and starts-with(preceding-sibling::*[1]//@src,'Part')">
                <xsl:copy>
                    <xsl:apply-templates select="@*"/>
                    <xsl:element name="navLabel" namespace="http://www.daisy.org/z3986/2005/ncx/">
                        <xsl:element name="text" namespace="http://www.daisy.org/z3986/2005/ncx/">
                            <xsl:value-of select="$revName"/>
                        </xsl:element>
                    </xsl:element>
                    <xsl:element name="content" namespace="http://www.daisy.org/z3986/2005/ncx/">
                        <xsl:attribute name="src">
                            <xsl:value-of select="ncx:content/@src"/>
                            <xsl:text>#body</xsl:text>
                        </xsl:attribute>
                    </xsl:element>
                    <xsl:call-template name="SubCopy">
                        <xsl:with-param name="item" select="."/>
                        <xsl:with-param name="type">Rev</xsl:with-param>
                    </xsl:call-template>
                </xsl:copy>
            </xsl:when>
            
            <!-- Reversal section but no main section -->
            <xsl:when test="count(ncx:content[starts-with(@src,'Part')]) = 1 and not(starts-with(preceding-sibling::*[1]//@src,'Part'))">
                <xsl:call-template name="SubCopy">
                    <xsl:with-param name="item" select="."/>
                    <xsl:with-param name="type">Rev</xsl:with-param>
                </xsl:call-template>
            </xsl:when>

            <!-- Two cases: we are copying the nodes under main or rev or we are copying front matter -->
            <xsl:otherwise>
                <xsl:if test="starts-with(ncx:content/@src, 'File')">
                    <xsl:copy>
                        <xsl:apply-templates select="node() | @*"/>
                    </xsl:copy>
                </xsl:if>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="SubCopy">
        <xsl:param name="item"/>
        <xsl:param name="type"/>
        
        <xsl:if test="starts-with($item/ncx:content/@src, $type)">
            <xsl:copy-of select="$item"/>
            <xsl:call-template name="SubCopy">
                <xsl:with-param name="item" select="$item/following-sibling::*[1]"/>
                <xsl:with-param name="type" select="$type"/>
            </xsl:call-template>
        </xsl:if>
    </xsl:template>
    
</xsl:stylesheet>
