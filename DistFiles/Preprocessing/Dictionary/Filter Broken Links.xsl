<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        RemoveBrokenLinks.xsl
    # Purpose:     Filter to remove anchors to no where
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2011/11/30
    # Updated:    2013/06/27 - GT improve copy.
    # Copyright:   (c) 2011 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0" xmlns:x="http://www.w3.org/1999/xhtml">
    
    <xsl:key name="ids" match="@id" use="."/>

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
    
    <!-- Handle entry and letData element-->
    <xsl:template match="x:a">
        <!-- Anchors must reference a node to be included -->
        <xsl:variable name="myRef">
            <xsl:value-of select="substring(@href,2)"/>
        </xsl:variable>
        <xsl:choose>
            <xsl:when test="key('ids', $myRef)">
                <xsl:copy>
                    <xsl:apply-templates select="node()|@*"/>
                </xsl:copy>
            </xsl:when>
            <xsl:otherwise>
                <xsl:apply-templates/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

</xsl:stylesheet>