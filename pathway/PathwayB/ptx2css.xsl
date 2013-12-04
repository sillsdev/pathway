<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method="text"/>
    
    <xsl:template match="stylesheet">
        <xsl:text>body {&#13;&#10;</xsl:text>
        <xsl:apply-templates select="property"/>
        <xsl:text>}&#13;&#10;</xsl:text>
        <xsl:apply-templates select="style"/>
    </xsl:template>
    
    <xsl:template match="style">
        <xsl:text>.</xsl:text>
        <xsl:value-of select="@id"/>
        <xsl:text> {&#13;&#10;</xsl:text>
        <xsl:apply-templates select="property"/>
        <xsl:text>}&#13;&#10;</xsl:text>
    </xsl:template>
    
    <xsl:template match="property">
        <xsl:text>    </xsl:text>
        <xsl:value-of select="@name"/>
        <xsl:text>:</xsl:text>
        <xsl:choose>
            <xsl:when test="contains(text(), ' ')">
                <xsl:text>"</xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>"</xsl:text>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="text()"/>
            </xsl:otherwise>
        </xsl:choose>
        <xsl:value-of select="@unit"/>
        <xsl:text>;&#13;&#10;</xsl:text>
    </xsl:template>
</xsl:stylesheet>