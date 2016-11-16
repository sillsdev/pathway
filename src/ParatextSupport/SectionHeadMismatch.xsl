<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml" exclude-result-prefixes="xhtml"
    xmlns="http://www.w3.org/1999/xhtml">
    
    <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no"
        doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"/>
    <xsl:strip-space elements="*"/>
    <!-- Recursive copy template -->
    <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>
    
    <xsl:template match="xhtml:p[@class='Paragraph']">
        <xsl:if test="not(preceding-sibling::node()[1][self::xhtml:h1][starts-with(@class, 'Section')]) and (child::node()[1][self::xhtml:span][starts-with(@class,'Chapter_Number')])">
            <h1 class="Section_Head"/>
        </xsl:if>
        <xsl:copy>
            <xsl:apply-templates select="@*|node()" />
        </xsl:copy>
    </xsl:template>
</xsl:stylesheet>
    
