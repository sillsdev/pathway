<xsl:stylesheet version="2.0" xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xml="http://www.w3.org/XML/1998/namespace"
    xmlns:epub="http://www.idpf.org/2007/ops"
    exclude-result-prefixes="xhtml xsl xs xml epub">
    <xsl:output method="html" encoding="utf-8" indent="yes"/>
    <xsl:template match="/">
        <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
        <xsl:text>&#xa;</xsl:text>
        <xsl:apply-templates></xsl:apply-templates>
    </xsl:template>
    <xsl:template match="xhtml:div[@id='TOCPage']">
        <xsl:element name="nav">
            <xsl:attribute name="epub:type">
                <xsl:text>toc</xsl:text>
            </xsl:attribute>
            <xsl:apply-templates></xsl:apply-templates>
        </xsl:element>
    </xsl:template>
    <xsl:template match="xhtml:ul">
        <xsl:element name="ol">
            <xsl:attribute name="epub:type"><xsl:text>list</xsl:text></xsl:attribute>
            <xsl:attribute name="class"><xsl:text>letter</xsl:text></xsl:attribute>
            <xsl:apply-templates></xsl:apply-templates>
        </xsl:element>
    </xsl:template>
    <xsl:template match="node()|@*">
        <xsl:copy>
            <xsl:apply-templates select="node()|@*"/>
        </xsl:copy>
    </xsl:template>
    <xsl:template match="xhtml:h2"/>
</xsl:stylesheet>
