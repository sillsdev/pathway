<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        RemoveLetterDividers2.xsl
    # Purpose:     Filter to remove letter headers & leave columns
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2012/5/21
    # Copyright:   (c) 2011 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0" xmlns:x="http://www.w3.org/1999/xhtml">
    
    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" 
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>
    
    <!-- Handle letHead, letter, and letData element-->
    <xsl:template match="x:div[@class = 'letHead' or @class = 'letData']" >
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="x:div[@class = 'letter']" />
    
    <!-- Add structure for body -->
    <xsl:template match="x:body">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:element name="div" namespace="http://www.w3.org/1999/xhtml">
                <xsl:attribute name="class">letHead</xsl:attribute>
                <xsl:element name="div" namespace="http://www.w3.org/1999/xhtml">
                    <xsl:attribute name="class">letter</xsl:attribute>
                    <xsl:text>A - Z</xsl:text>
                </xsl:element>
            </xsl:element>
            <xsl:element name="div" namespace="http://www.w3.org/1999/xhtml">
                <xsl:attribute name="class">letData</xsl:attribute>
                <xsl:apply-templates/>
            </xsl:element>
        </xsl:copy>
    </xsl:template>
    
    <!-- Copy unaffected non-span elements-->
    <xsl:template match="x:html | x:head | x:span | x:div[@class != 'letHead' and @class != 'letter' and @class != 'letData'] | 
        x:link | x:meta | x:a | x:img | x:title | x:style | comment()">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>
    
</xsl:stylesheet>