<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        FixEpubToc.xsl
    # Purpose:     Fixes for Epub Toc to pass check
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2013/02/19
    # Copyright:   (c) 2013 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ncx="http://www.daisy.org/z3986/2005/ncx/"
    version="1.0">
    
    <xsl:variable name="ncx">http://www.daisy.org/z3986/2005/ncx/</xsl:variable>
    
    <!-- Recursive copy template -->   
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>
    
    <xsl:template match="ncx:navPoint">
        <xsl:if test="not((.//@src)[1] = '')">
            <xsl:copy>
                <xsl:for-each select="@*">
                    <xsl:choose>
                        <xsl:when test="local-name(.) = 'id'">
                            <xsl:call-template name="id"/>
                        </xsl:when>
                        <xsl:when test="local-name(.) = 'playOrder'">
                            <xsl:call-template name="playOrder"/>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:apply-templates select="."/>
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:for-each>
                <xsl:apply-templates/>
            </xsl:copy>
        </xsl:if>
    </xsl:template>
    
    <xsl:template name='id'>
        <xsl:attribute name="id">
            <xsl:text>np</xsl:text>
            <xsl:number count="ncx:navPoint" level="any"/>
        </xsl:attribute>
    </xsl:template>

    <xsl:template name='playOrder'>
        <xsl:attribute name="playOrder">
            <xsl:number count="*[@playOrder]" level="any"/>
        </xsl:attribute>
    </xsl:template>
    
</xsl:stylesheet>