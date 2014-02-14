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
    
    <xsl:template match="*[@class='Homograph-Number'] | *[@class='xhomographnumber']">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:if test="local-name() != 'class'">
                    <xsl:copy/>
                </xsl:if>
            </xsl:for-each>
            <xsl:choose>
                <xsl:when test="@class = 'Homograph-Number'">
                    <xsl:attribute name="class">My-Homograph-Number</xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:attribute name="class">My-xhomographnumber</xsl:attribute>
                </xsl:otherwise>
            </xsl:choose>
            <xsl:apply-templates select="node()" mode="n"/>
        </xsl:copy>
    </xsl:template>
    
    <xsl:template match="text()" mode="n">
        <xsl:choose>
            <xsl:when test=".='1'"><xsl:text>&#xA0;I</xsl:text></xsl:when>
            <xsl:when test=".='2'"><xsl:text>&#xA0;II</xsl:text></xsl:when>
            <xsl:when test=".='3'"><xsl:text>&#xA0;III</xsl:text></xsl:when>
            <xsl:when test=".='4'"><xsl:text>&#xA0;IV</xsl:text></xsl:when>
            <xsl:when test=".='5'"><xsl:text>&#xA0;V</xsl:text></xsl:when>
            <xsl:when test=".='6'"><xsl:text>&#xA0;VI</xsl:text></xsl:when>
            <xsl:when test=".='7'"><xsl:text>&#xA0;VII</xsl:text></xsl:when>
            <xsl:when test=".='8'"><xsl:text>&#xA0;VIII</xsl:text></xsl:when>
            <xsl:when test=".='9'"><xsl:text>&#xA0;IX</xsl:text></xsl:when>
            <xsl:when test=".='10'"><xsl:text>&#xA0;X</xsl:text></xsl:when>
            <xsl:when test=".='11'"><xsl:text>&#xA0;XI</xsl:text></xsl:when>
            <xsl:when test=".='12'"><xsl:text>&#xA0;XII</xsl:text></xsl:when>
            <xsl:when test=".='13'"><xsl:text>&#xA0;XIII</xsl:text></xsl:when>
            <xsl:when test=".='14'"><xsl:text>&#xA0;XIV</xsl:text></xsl:when>
            <xsl:when test=".='15'"><xsl:text>&#xA0;XV</xsl:text></xsl:when>
            <xsl:when test=".='16'"><xsl:text>&#xA0;XVI</xsl:text></xsl:when>
            <xsl:when test=".='17'"><xsl:text>&#xA0;XVII</xsl:text></xsl:when>
            <xsl:when test=".='18'"><xsl:text>&#xA0;XVIII</xsl:text></xsl:when>
            <xsl:when test=".='19'"><xsl:text>&#xA0;XIX</xsl:text></xsl:when>
            <xsl:when test=".='20'"><xsl:text>&#xA0;XX</xsl:text></xsl:when>
            <xsl:otherwise><xsl:value-of select="."/></xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
</xsl:stylesheet>