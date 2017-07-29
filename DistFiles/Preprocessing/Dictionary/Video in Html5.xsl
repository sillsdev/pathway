<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Video in Epub.xsl
    # Purpose:     change <a> with video href to video
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2017/7/28
    # Copyright:   (c) 2017 SIL International
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

	<xsl:template match="x:a[contains(@href,'.mp4') or contains(@href,'.ogg') or contains(@href,'.avi') or contains(@href,'.3gp')]">
		<xsl:variable name="newId" select="generate-id(@href)"></xsl:variable>
		<xsl:element name="video" namespace="http://www.w3.org/1999/xhtml">
			<xsl:attribute name="id">
				<xsl:value-of select="$newId"/>
			</xsl:attribute>
			<xsl:attribute name="style">max-height:1in;</xsl:attribute>
			<xsl:attribute name="onclick">
				<xsl:text>document.getElementById('</xsl:text>
				<xsl:value-of select="$newId"/>
				<xsl:text>').play()</xsl:text>
			</xsl:attribute>
			<xsl:element name="source" namespace="http://www.w3.org/1999/xhtml">
				<xsl:attribute name="src">
					<xsl:value-of select="substring-before(@href,'\')"/>
					<xsl:text>/</xsl:text>
					<xsl:value-of select="substring-after(@href,'\')"/>
				</xsl:attribute>
			</xsl:element>
			<xsl:text>Missing </xsl:text>
			<xsl:value-of select="substring-after(@href, '\')"/>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>