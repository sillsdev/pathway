<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        AudioExampleFilter.xsl
    # Purpose:     Filter to remove entries and examples without audio
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2011/11/17
    # Copyright:   (c) 2011 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0" xmlns:x="http://www.w3.org/1999/xhtml">
    
    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" 
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>
    
    <!-- Handle entry and letData element-->
    <xsl:template match="x:div[@class = 'entry' or @class = 'letData']">
        <!-- Entries must have audio to be included -->
        <xsl:if test="count(.//text()[contains(.,'audio')]) &gt; 0">
            <xsl:copy>
                <xsl:for-each select="@*">
                    <xsl:copy/>
                </xsl:for-each>
                <xsl:apply-templates/>
            </xsl:copy>
        </xsl:if>
    </xsl:template>

    <!-- Handle example span elements-->
    <xsl:template match="x:span[@class = 'examples' or @class = 'examples-sub']">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:for-each select="*">
                <!-- Examples must have audio to be included -->
                <xsl:if test="count(.//text()[contains(.,'audio')]) &gt; 0">
                    <xsl:copy>
                        <xsl:for-each select="@*">
                            <xsl:copy/>
                        </xsl:for-each>
                        <xsl:apply-templates/>
                    </xsl:copy>
                </xsl:if>
            </xsl:for-each>
        </xsl:copy>
    </xsl:template>
    
    <!-- Copy unaffected div elements-->
    <xsl:template match="x:div[@class != 'entry' and @class != 'letData']">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>
    
    <!-- Copy unaffected span elements-->
    <xsl:template match="x:span[@class != 'examples' and @class != 'examples-sub']">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>
    
    <!-- Copy unaffected non-span elements-->
    <xsl:template match="x:html | x:head | x:body | x:link | x:meta | x:a | x:img | x:title | x:style | comment()">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>
    
</xsl:stylesheet>