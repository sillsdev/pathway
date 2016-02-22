<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Letter Header Langauge.xsl
    # Purpose:     Add language attribute to letter header.
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2016/02/22
    # Copyright:   (c) 2016 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:x="http://www.w3.org/1999/xhtml"
    version="1.0">
    
    <xsl:param name="title"/>
    
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

    <xsl:template match="x:div[@class='letter']">
        <xsl:copy>
            <xsl:apply-templates select="@*"/>
            <xsl:attribute name="lang">
                <xsl:value-of select="(parent::*/following-sibling::*[1]//x:span/@lang)[1]"/>
            </xsl:attribute>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>

</xsl:stylesheet>