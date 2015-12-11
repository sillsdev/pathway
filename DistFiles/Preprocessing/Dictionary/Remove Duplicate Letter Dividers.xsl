<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Remove Duplicate Letter Dividers.xsl
    # Purpose:     Letter headers for secondary distinctions
	#              and letter headers for ignored hyphen in reversal
	#              are removed
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2014/01/16
    # Copyright:   (c) 2012 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
    xmlns:x="http://www.w3.org/1999/xhtml">
    
    <xsl:param name="from">ÀàÁáÈèÉéÌìÍíÒòÓóÙùÚú&#x0971;-</xsl:param>
    <xsl:param name="to">AaAaEeEeIiIiOoOoUuUu..</xsl:param>
    
    <!-- Recursive copy template (all except xml:space attributes) -->   
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>
    
    <!-- Select desired headers -->
    <xsl:template match="*[@class='letHead']">
        <xsl:variable name="cur" select="translate(*/text(),$from,$to)"/>
        <xsl:variable name="prev" select="preceding-sibling::*[@class='letHead']/*/text()"/>
        <xsl:if test="not($cur = $prev) and not(starts-with($cur, '.'))">
            <xsl:copy>
                <xsl:apply-templates select="node() | @*"/>
            </xsl:copy>
            <xsl:apply-templates select="following-sibling::*[1]" mode="data">
                <xsl:with-param name="cur" select="$cur"/>
                <xsl:with-param name="first" select="true()"/>
            </xsl:apply-templates>
        </xsl:if>
    </xsl:template>
    
    <xsl:template match="*" mode="data">
        <xsl:param name="cur"/>
        <xsl:param name="first" select="false()"/>
        <xsl:choose>
            <xsl:when test="$first">
                <xsl:copy>
                    <xsl:apply-templates select="node() | @*"/>
                    <xsl:call-template name="NextData">
                        <xsl:with-param name="cur" select="$cur"/>
                    </xsl:call-template>
                </xsl:copy>
            </xsl:when>
            <xsl:otherwise>
                <xsl:apply-templates select="*"/>
                <xsl:call-template name="NextData">
                    <xsl:with-param name="cur" select="$cur"/>
                </xsl:call-template>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="NextData">
        <xsl:param name="cur"/>
        <xsl:variable name="next" select="translate(following-sibling::*[1]/*/text(), $from, $to)"/>
        <xsl:if test="$next = $cur or starts-with($next, '.')">
            <xsl:apply-templates select="following-sibling::*[2]" mode="data">
                <xsl:with-param name="cur" select="$cur"/>
            </xsl:apply-templates>
        </xsl:if>
    </xsl:template>
    
    <xsl:template match="*[@class='letData']"/>

</xsl:stylesheet>