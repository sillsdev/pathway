<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Reset columns using letter headers.xsl
    # Purpose:     Use current letter headers to reset letData
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2017/02/28
    # Copyright:   (c) 2017 SIL International
    # Licence:     <MIT>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0" xmlns:x="http://www.w3.org/1999/xhtml">
    
    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" 
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>
    
    <!-- Recursive copy template -->   
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>
    
    <xsl:template match="x:a/@shape"/>
    <xsl:template match="x:head/@profile"/>
    <xsl:template match="x:html/@version"/>
    
    <xsl:template match="*[@class='letHead']">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
        <xsl:element name="div" namespace="http://www.w3.org/1999/xhtml">
            <xsl:attribute name="class">letData</xsl:attribute>
            <xsl:if test="following-sibling::*[1]/@class!='letData'">
                <xsl:apply-templates select="following::*[1]" mode="inCol"/>
            </xsl:if>
            <xsl:if test="following-sibling::*[1]/@class='letData'">
                <xsl:apply-templates select="following::*[1]/*[1]" mode="inCol"/>
            </xsl:if>
        </xsl:element>
    </xsl:template>
    
    <!-- Remove current letData and its contents -->
    <xsl:template match="*[@class='letData']"/>
    
    <xsl:template match="*" mode="inCol">
        <xsl:if test="not(@class='letHead')">
            <xsl:copy>
                <xsl:apply-templates select="node() | @*"/>
            </xsl:copy>
            <xsl:if test="following::*[1]/@class!='letData'">
                <xsl:apply-templates select="following::*[1]" mode="inCol"/>
            </xsl:if>
            <xsl:if test="following::*[1]/@class='letData'">
                <xsl:apply-templates select="following::*[1]/*[1]" mode="inCol"/>
            </xsl:if>
        </xsl:if>
    </xsl:template>
    
</xsl:stylesheet>