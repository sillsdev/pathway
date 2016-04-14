<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Map Solidus.xsl
    # Purpose:     map slash to none printing word break
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2016/4/14
    # Copyright:   (c) 2016 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0" xmlns:x="http://www.w3.org/1999/xhtml">

    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" 
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>

    <!-- Copy unaffected non-span elements-->
    <xsl:template match="node()|@*">
        <xsl:copy>
            <xsl:apply-templates select="node()|@*"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="text()[contains(.,'/')]">
        <xsl:value-of select="translate(., '/', '&#x200b;')"/>
    </xsl:template>

</xsl:stylesheet>