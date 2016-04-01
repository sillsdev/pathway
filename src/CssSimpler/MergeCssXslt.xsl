<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        MergeCss-Xslt.xsl
    # Purpose:     Merge rules with same match point
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2016/3/16
    # Copyright:   (c) 2016 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0">

    <xsl:output method="xml" encoding="utf-8"/>
    
    <!-- Copy nodes -->
    <xsl:template match="node()|@*">
        <xsl:copy>
            <xsl:apply-templates select="node()|@*"/>
        </xsl:copy>
    </xsl:template>
    
    <xsl:template match="xsl:template">
        <xsl:variable name="match" select="@match"/>
        <xsl:choose>
            <xsl:when test="following-sibling::*/@match = $match"/>
            <xsl:when test="preceding-sibling::*/@match = $match">
                <xsl:copy>
                    <xsl:apply-templates select="@*"/>
                    <xsl:choose>
                        <xsl:when test="local-name(*[1]) != 'element'">
                            <xsl:apply-templates select="preceding-sibling::*[@match=$match][local-name(*[1])='element']/*[1]"/>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:apply-templates select="*[1]"/>
                        </xsl:otherwise>
                    </xsl:choose>
                    <xsl:apply-templates select="*[local-name() = 'copy']"/>
                    <xsl:choose>
                        <xsl:when test="local-name(*[2]) != 'element'">
                            <xsl:apply-templates select="preceding-sibling::*[@match=$match][local-name(*[2])='element']/*[2]"/>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:apply-templates select="*[2]"/>
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:copy>
            </xsl:when>
            <xsl:otherwise>
                <xsl:copy>
                    <xsl:apply-templates select="node()|@*"/>
                </xsl:copy>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>    
</xsl:stylesheet>