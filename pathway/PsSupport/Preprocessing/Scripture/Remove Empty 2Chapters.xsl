<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Remove Empty 2Chapters.xsl
    # Purpose:     Filter to remove empty chapters
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2012/11/15
    # Copyright:   (c) 2012 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0" xmlns:x="http://www.w3.org/1999/xhtml">
    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" 
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>

    <!-- Copy unaffected non-span elements-->
    <xsl:template match="* | comment()">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>
    
    <!--xsl:template match="//x:body/x:div/x:span[@class='Chapter_Number']"/-->
    <xsl:template match="*[@class='Chapter_Number']">
        <xsl:variable name="next" select="following-sibling::*[1]"/>
        <xsl:if test="local-name($next) = 'span'">
            <xsl:if test="$next/@class = 'Verse_Number'">
                <xsl:copy>
                    <xsl:for-each select="@*">
                        <xsl:copy/>
                    </xsl:for-each>
                    <xsl:apply-templates/>
                </xsl:copy>
            </xsl:if>
        </xsl:if>
    </xsl:template>
    
</xsl:stylesheet>