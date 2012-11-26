<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        RemoveExtraLetterHeaders1.xsl
    # Purpose:     Letter headers for secondary distinctions are removed.
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2012/11/26
    # Copyright:   (c) 2012 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
    xmlns:x="http://www.w3.org/1999/xhtml">
    
    <xsl:param name="from">ÀàÁá</xsl:param>
    <xsl:param name="to">AaAa</xsl:param>
    
    <!-- Recursive copy template (all except xml:space attributes) -->   
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>
    
    <!-- Select desired headers -->
    <xsl:template match="x:div[@class='letHead']">
        <xsl:variable name="cur" select="translate(*/text(),$from,$to)"/>
        <xsl:variable name="prev" select="translate(preceding-sibling::*[2]/*/text(),$from,$to)"/>
        <xsl:if test="not($cur = $prev)">
            <xsl:copy>
                <xsl:apply-templates select="node() | @*"/>
            </xsl:copy>
        </xsl:if>
    </xsl:template>

    <!-- Combine data for equivalent headers -->
    <xsl:template match="x:div[@class='letData']">
        <xsl:variable name="cur" select="translate(preceding-sibling::*[1]/*/text(),$from,$to)"/>
        <xsl:variable name="prev" select="translate(preceding-sibling::*[3]/*/text(),$from,$to)"/>
        <xsl:if test="not($cur = $prev)">
            <xsl:copy>
                <xsl:apply-templates select="node() | @*"/>
                <xsl:for-each select="following-sibling::*[@class='letData']">
                    <xsl:variable name="test" select="translate(preceding-sibling::*[@class='letHead'][1]/*/text(),$from,$to)"/>
                    <xsl:if test="$cur = $test">
                        <xsl:apply-templates select="*"/>
                    </xsl:if>
                </xsl:for-each>
            </xsl:copy>
        </xsl:if>
    </xsl:template>
</xsl:stylesheet>