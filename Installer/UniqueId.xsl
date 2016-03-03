<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        UniqueId.xsl
    # Purpose:     Modify Component Ids to make them unique
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2015/11/25
    # Copyright:   (c) 2015 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:wix2="http://schemas.microsoft.com/wix/2003/01/wi"
    version="1.0">
    
    <xsl:output method="xml" indent="yes" encoding="UTF-8"/>
    
    <xsl:template match="wix2:Component | wix2:ComponentRef | wix2:File">
        <xsl:variable name="name" select="local-name()"/>
        <xsl:variable name="Id" select="translate(@Id,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')"/>
        <xsl:variable name="cnt" select="count(preceding::*[local-name()=$name][translate(@Id,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')=$Id])"/>
        <xsl:copy>
            <xsl:choose>
                <xsl:when test="$cnt = 0">
                    <xsl:apply-templates select="@Id"/>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:attribute name="Id">
                        <xsl:value-of select="@Id"/>
                        <xsl:text>_</xsl:text>
                        <xsl:value-of select="$cnt + 1"/>
                    </xsl:attribute>
                </xsl:otherwise>
            </xsl:choose>
            <xsl:for-each select="@*[local-name() != 'Id']">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:apply-templates select="node()"/>
        </xsl:copy>
    </xsl:template>
    
    <!-- Recursive copy template -->   
    <xsl:template match="*[local-name() != 'Component' and local-name() != 'ComponentRef' and local-name() != 'File'] | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>
    
    <xsl:template match="text()"/>
</xsl:stylesheet>