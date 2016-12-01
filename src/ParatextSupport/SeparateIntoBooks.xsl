<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no"
        doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"/>
    
    <!-- Remove text (empty lines) at the root level. -->
    <xsl:strip-space elements="*"/>

    <xsl:template match = "*"/>
    
    <xsl:template match = "usfm|usx">
        <xsl:copy>
            <xsl:apply-templates select = "*|node()"/>
        </xsl:copy>
    </xsl:template>
    
    <xsl:template match = "book">
        <xsl:if test="not(local-name(following-sibling::*[1]) = 'book')">
            <xsl:copy>
                <xsl:copy-of select="@*"/>
                <xsl:apply-templates select="following-sibling::*[1][not(self::book)]" mode="book"/>
            </xsl:copy>
        </xsl:if>
    </xsl:template>

    <xsl:template match = "*" mode="book">
        <xsl:copy-of select="."/>
        <xsl:apply-templates select="following-sibling::*[1][not(self::book)]" mode="book"/>
    </xsl:template>
    
    <!-- Remove books in their original location. -->
    <xsl:template match = "book" mode="book"/>
</xsl:stylesheet>