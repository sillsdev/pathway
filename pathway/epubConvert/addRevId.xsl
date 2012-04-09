<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        AddRevId.xsl
    # Purpose:     Add id attribute to reversal entries for use in table of contents.
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2012/04/05
    # Copyright:   (c) 2012 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
    xmlns:x="http://www.w3.org/1999/xhtml">
    <!-- Recursive copy template -->   
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>
    <!-- Always compute a sequential value for playOrder -->
    <xsl:template match="x:div[@class='entry']">
        <xsl:element name="div" namespace="http://www.w3.org/1999/xhtml">
            <xsl:attribute name="class">entry</xsl:attribute>
            <xsl:attribute name="id">
                <xsl:text>rev</xsl:text>
                <xsl:number count="x:div[@class='entry']" level="any"/>
            </xsl:attribute>
            <xsl:apply-templates/>
        </xsl:element>
    </xsl:template>
</xsl:stylesheet>
