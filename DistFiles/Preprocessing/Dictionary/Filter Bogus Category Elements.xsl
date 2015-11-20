<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Filter Bogus Category Elements.xsl
    # Purpose:     Filter to remove ReversalIndexEntry_PartOfSpeech and PartOfSpeechLink
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2014/08/29
    # Copyright:   (c) 2014 SIL International
    # Licence:     <LPGL>
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
    
    <!-- Handle ReversalIndexEntry_PartOfSpeech and PartOfSpeechLink element-->
    
    <xsl:template match="x:ReversalIndexEntry_PartOfSpeech | x:PartOfSpeechLink">
        <xsl:apply-templates select="node() | @*"/>
    </xsl:template>

</xsl:stylesheet>