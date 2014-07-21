<xsl:stylesheet version="2.0" xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xml="http://www.w3.org/XML/1998/namespace"
xmlns:epub="http://www.idpf.org/2007/ops"
exclude-result-prefixes="xhtml xsl xs xml">
<xsl:output method="html" encoding="utf-8"/>
    <xsl:template match="xhtml:html">
        <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
        <xsl:text>&#xa;</xsl:text>
        <html xmlns="http://www.w3.org/1999/xhtml" xmlns:epub="http://www.idpf.org/2007/ops" lang="en" xml:lang="en">
        <xsl:apply-templates></xsl:apply-templates>
        </html>
    </xsl:template>
    <xsl:template name="string-replace-all">
        <xsl:param name="text" />
        <xsl:param name="replace" />
        <xsl:param name="by" />
        <xsl:choose>
            <xsl:when test="contains($text, $replace)">
                <xsl:value-of select="substring-before($text,$replace)" />
                <xsl:value-of select="$by" />
                <xsl:call-template name="string-replace-all">
                    <xsl:with-param name="text"
                        select="substring-after($text,$replace)" />
                    <xsl:with-param name="replace" select="$replace" />
                    <xsl:with-param name="by" select="$by" />
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="$text" />
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    <xsl:template match="node()|@*">
        <xsl:copy>
            <xsl:apply-templates select="node()|@*"/>
        </xsl:copy>
    </xsl:template>
    <xsl:template match="@xml:lang">
        <xsl:attribute name="lang">
            <xsl:value-of select="."/>
        </xsl:attribute>
    </xsl:template>
    <xsl:variable name="in"><xsl:text>.xhtml</xsl:text></xsl:variable>
    <xsl:variable name="out"><xsl:text>.html</xsl:text></xsl:variable>
    <xsl:template match="@href">
        <xsl:attribute name="href">
                        <xsl:call-template name="string-replace-all">
                    <xsl:with-param name="text" select="." />
                    <xsl:with-param name="replace" select="$in" />
                    <xsl:with-param name="by" select="$out" />
                </xsl:call-template>
        </xsl:attribute>
    </xsl:template>
    <xsl:template match="@longdesc|@profile">
    </xsl:template>
    <xsl:template name="html-head">
        <head>
            
        </head>  
    </xsl:template>
</xsl:stylesheet>
