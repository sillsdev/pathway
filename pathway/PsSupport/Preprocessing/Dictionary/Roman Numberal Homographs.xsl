<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Roman Numeral Homographs.xsl
    # Purpose:     
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2014/01/15
    # Copyright:   (c) 2014 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:x="http://www.w3.org/1999/xhtml"
    version="1.0">
    
    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.1//EN" 
        doctype-system="http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"/>

    <!-- Recursive copy template -->   
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>
    
    <xsl:template match="x:a/@shape"/>
    <xsl:template match="x:head/@profile"/>
    <xsl:template match="x:html/@version"/>
    
    <xsl:template match="*[@class='Homograph-Number']/text() | *[@class='xhomographnumber']/text()">
        <xsl:choose>
            <xsl:when test=".='1'"><xsl:text>I</xsl:text></xsl:when>
            <xsl:when test=".='2'"><xsl:text>II</xsl:text></xsl:when>
            <xsl:when test=".='3'"><xsl:text>III</xsl:text></xsl:when>
            <xsl:when test=".='4'"><xsl:text>IV</xsl:text></xsl:when>
            <xsl:when test=".='5'"><xsl:text>V</xsl:text></xsl:when>
            <xsl:when test=".='6'"><xsl:text>VI</xsl:text></xsl:when>
            <xsl:when test=".='7'"><xsl:text>VII</xsl:text></xsl:when>
            <xsl:when test=".='8'"><xsl:text>VIII</xsl:text></xsl:when>
            <xsl:when test=".='9'"><xsl:text>IX</xsl:text></xsl:when>
            <xsl:when test=".='10'"><xsl:text>X</xsl:text></xsl:when>
            <xsl:when test=".='11'"><xsl:text>XI</xsl:text></xsl:when>
            <xsl:when test=".='12'"><xsl:text>XII</xsl:text></xsl:when>
            <xsl:when test=".='13'"><xsl:text>XIII</xsl:text></xsl:when>
            <xsl:when test=".='14'"><xsl:text>XIV</xsl:text></xsl:when>
            <xsl:when test=".='15'"><xsl:text>XV</xsl:text></xsl:when>
            <xsl:when test=".='16'"><xsl:text>XVI</xsl:text></xsl:when>
            <xsl:when test=".='17'"><xsl:text>XVII</xsl:text></xsl:when>
            <xsl:when test=".='18'"><xsl:text>XVIII</xsl:text></xsl:when>
            <xsl:when test=".='19'"><xsl:text>XIX</xsl:text></xsl:when>
            <xsl:when test=".='20'"><xsl:text>XX</xsl:text></xsl:when>
            <xsl:otherwise><xsl:value-of select="."/></xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
</xsl:stylesheet>