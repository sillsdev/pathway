<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Add File Prefix to source.xsl
    # Purpose:     Add file:/ prefix
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2016/6/6
    # Copyright:   (c) 2016 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:x="http://www.w3.org/1999/xhtml"
    version="1.0">

    <xsl:output encoding="UTF-8" method="xml" />

    <xsl:template match="/">
        <xsl:text disable-output-escaping="yes"><![CDATA[<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"[
<!ATTLIST html lang CDATA #REQUIRED >
<!ATTLIST span lang CDATA #IMPLIED >
<!ATTLIST span entryguid CDATA #IMPLIED >
<!ATTLIST img alt CDATA #IMPLIED >
<!ELEMENT audio (source) >
<!ATTLIST audio id CDATA #IMPLIED >
<!ELEMENT source EMPTY >
<!ATTLIST source src CDATA #REQUIRED >
]>]]></xsl:text>
        <xsl:apply-templates select="node()|@*"/>
    </xsl:template>

    <!-- Recursive template -->
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="x:a/@shape"/>
    <xsl:template match="x:head/@profile"/>
    <xsl:template match="x:html/@version"/>

    <xsl:template match="x:audio//@src">
        <xsl:attribute name="src">
            <xsl:if test="not(contains(@src,':/'))">
                <xsl:text>file:/</xsl:text>
            </xsl:if>
            <xsl:value-of select="translate(.,'\','/')"/>
        </xsl:attribute>
    </xsl:template>

    <xsl:template match="x:img//@src">
        <xsl:attribute name="src">
            <xsl:value-of select="translate(.,'\','/')"/>
        </xsl:attribute>
    </xsl:template>

    <xsl:template match="x:a//@href[.='#']">
        <xsl:attribute name="href">
            <!-- Adding the file:/ helps PrinceXml but breaks FireFox -->
            <xsl:text>file:/</xsl:text>
            <xsl:value-of select="translate(preceding::x:audio[1]//@src,'\','/')"/>
        </xsl:attribute>
        <xsl:attribute name="style">
            <xsl:text>font-family:'Segoe UI Symbol', Symbola;</xsl:text>
        </xsl:attribute>
    </xsl:template>
</xsl:stylesheet>