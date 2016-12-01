<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Thin Space Before Footnote.xsl
    # Purpose:     Add a thin space before the footnote marker
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2016/11/16
    # Copyright:   (c) 2016 SIL International
    # Licence:     <MIT>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0" xmlns:x="http://www.w3.org/1999/xhtml">
    
    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" 
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>
    
    <!-- Remove all links -->
    <xsl:template match="x:span">
        <xsl:choose>
            <xsl:when test="@class='scrFootnoteMarker'">
                <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                    <xsl:text disable-output-escaping="yes"><![CDATA[&#x202F;]]></xsl:text>
                </xsl:element>
                <xsl:copy>
                    <xsl:for-each select="@*">
                        <xsl:copy/>
                    </xsl:for-each>
                    <xsl:apply-templates/>
                </xsl:copy>
            </xsl:when>
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