<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        MoveFRT.xsl
    # Purpose:     Move front matter to front
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2014/02/04
    # Copyright:   (c) 2013 SIL International
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

    <xsl:template match="*[@class='scrBody']">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:apply-templates select="*[*[@class='scrBookCode']/text() = 'FRT']"/>
            <xsl:apply-templates select="*[*[@class='scrBookCode']/text() != 'FRT']"/>
        </xsl:copy>
    </xsl:template>
    
</xsl:stylesheet>