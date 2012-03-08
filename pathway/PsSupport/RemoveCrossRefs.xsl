<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        RemoveCrossRefs.xsl
    # Purpose:     Filter to remove cross references from Scripture
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2012/3/7
    # Copyright:   (c) 2011 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0" xmlns:x="http://www.w3.org/1999/xhtml">
    
    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" 
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>
    
    <!-- Remove all links -->
    <xsl:template match="x:span">
        <xsl:choose>
            <xsl:when test="@class='Note_CrossHYPHENReference_Paragraph'"/>
            <xsl:when test="following-sibling::node()[2]/@class='Note_CrossHYPHENReference_Paragraph'"/>
            <xsl:otherwise>
                <xsl:copy>
                    <xsl:for-each select="@*">
                        <xsl:copy/>
                    </xsl:for-each>
                    <xsl:apply-templates/>
                </xsl:copy>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <!-- Copy unaffected non-span elements-->
    <xsl:template match="x:html | x:head | x:body | x:div | x:link | x:meta | x:img | x:title | x:style | x:a | comment()">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>
    
</xsl:stylesheet>