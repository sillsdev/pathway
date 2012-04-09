<?xml version="1.0" encoding="UTF-8"?>
<!-- http://www.oxygenxml.com/forum/topic5956.html -->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <!-- Recursive copy template -->   
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>
    <!-- Always compute a sequential value for playOrder -->
    <xsl:template match="@playOrder">
        <xsl:attribute name="playOrder"><xsl:number count="*[@playOrder]" level="any"/></xsl:attribute>
    </xsl:template>
</xsl:stylesheet>
