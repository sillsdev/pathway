<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        FixLangCode.xsl
    # Purpose:     Update language code
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2012/03/01
    # Copyright:   (c) 2012 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0" xmlns:x="http://www.w3.org/1999/xhtml">
    
    <xsl:param name="lang">xyz</xsl:param>
    
    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" 
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>
    
    <!-- Copy unaffected non-span elements-->
    <xsl:template match="x:html | x:head | x:body | x:span | x:a | x:div | x:link | x:meta | x:img | x:title | x:style | comment()">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:choose>
                    <xsl:when test="local-name()='lang' and . = 'zxx'">
                        <xsl:attribute name="lang">
                            <xsl:value-of select="$lang"/>
                        </xsl:attribute>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:copy/>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:for-each>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>

</xsl:stylesheet>