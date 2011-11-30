<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        RemoveEmptyDiv.xsl
    # Purpose:     Filter to remove empty entries
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2011/11/30
    # Copyright:   (c) 2011 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0" xmlns:x="http://www.w3.org/1999/xhtml">
    
    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" 
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>
    
    <!-- Handle entry and letData element-->
    <xsl:template match="x:div[@class = 'entry']">
        <!-- Entries must have spans to be included -->
        <xsl:if test="count(.//x:span) &gt; 0">
            <xsl:copy>
                <xsl:for-each select="@*">
                    <xsl:copy/>
                </xsl:for-each>
                <xsl:apply-templates/>
            </xsl:copy>
        </xsl:if>
    </xsl:template>

    <!-- Copy unaffected non-span elements-->
    <xsl:template match="x:html | x:head | x:body | x:span | x:div[@class != 'entry'] | x:link | x:meta | x:a | x:img | x:title | x:style | comment()">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>
    
</xsl:stylesheet>