<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        FixInflectionExport.xsl
    # Purpose:     Flex has an error when outputting inflection in 8.2
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2016/01/19
    # Copyright:   (c) 2016 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:x="http://www.w3.org/1999/xhtml"
    version="1.0">

    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>

    <!-- Recursive copy template -->
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="x:a/@shape"/>
    <xsl:template match="x:head/@profile"/>
    <xsl:template match="x:html/@version"/>

    <xsl:template match="x:MoStemMsa_FeaturesTSS">
        <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
            <xsl:attribute name="lang">
                <xsl:value-of select=".//*/@ws"/>
            </xsl:attribute>
            <xsl:value-of select="//x:Run"/>
        </xsl:element>
    </xsl:template>

</xsl:stylesheet>