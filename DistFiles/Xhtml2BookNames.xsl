<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Xhtml2BookNames.xsl
    # Purpose:     Transform XHTML to booknames
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2014/08/20
    # Copyright:   (c) 2014 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0">
    
    <xsl:output indent="yes"/>
    
    <xsl:template match="/">
        <xsl:element name="BookNames">
            <xsl:apply-templates select="//*[@class='scrBook']" mode="b"/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="*" mode="b">
        <xsl:element name="book">
            <xsl:attribute name="code">
                <xsl:value-of select="*[@class='scrBookCode']"/>
            </xsl:attribute>
            <xsl:attribute name="abbr">
                <xsl:value-of select="*[@class='scrBookCode']"/>
            </xsl:attribute>
            <xsl:attribute name="short">
                <xsl:value-of select="*[@class='scrBookName']"/>
            </xsl:attribute>
            <xsl:attribute name="long">
                <xsl:value-of select="*[@class='Title_Main']/*[count(@class) = 0]"/>
            </xsl:attribute>
        </xsl:element>
    </xsl:template>
</xsl:stylesheet>