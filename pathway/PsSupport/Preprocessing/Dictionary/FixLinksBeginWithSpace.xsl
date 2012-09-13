<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        FixLinksBeginSpace.xsl
    # Purpose:     If link begins with space move space before link
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2012/08/28
    # Copyright:   (c) 2011 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0" xmlns:x="http://www.w3.org/1999/xhtml">
    
    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" 
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>
    
    <!-- Copy unaffected elements-->
    <xsl:template match="* | comment()">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>
    
    <!-- Handle link element-->
    <xsl:template match="x:a">
        <!-- Anchors must reference a node to be included -->
        <xsl:choose>
            <!-- test to see if link begins with a span containing a single space -->
            <xsl:when test="*/*[text() = ' ']">
                <!-- put xml:space="preserve" before link -->
                <xsl:copy-of select="*/*[1]"/>
                <xsl:copy>
                    <!-- copy link attributes -->
                    <xsl:for-each select="@*">
                        <xsl:copy/>
                    </xsl:for-each>
                    <!-- copy span of link -->
                    <xsl:for-each select="*">
                        <xsl:copy>
                            <!-- include class and language attributes -->
                            <xsl:for-each select="@*">
                                <xsl:copy/>
                            </xsl:for-each>
                            <!-- copy any other spans (there weren't any in the examples I saw) -->
                            <xsl:for-each select="*">
                                <xsl:if test="position() &gt; 1">
                                    <xsl:apply-templates/>
                                </xsl:if>
                            </xsl:for-each>
                            <!-- copy text of the value of the span -->
                            <xsl:value-of select="."/>
                        </xsl:copy>
                    </xsl:for-each>
                </xsl:copy>
            </xsl:when>
            <!-- all links that don't begin with a span of a single space get copied as is -->
            <xsl:otherwise>
                <xsl:apply-templates/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

</xsl:stylesheet>