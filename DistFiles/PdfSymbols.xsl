<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Pdf Symbols.xsl
    # Purpose:     Add font information to symbols
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2017/7/31
    # Copyright:   (c) 2017 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:x="http://www.w3.org/1999/xhtml"
    version="1.0">

	<xsl:param name="avPath"/>

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
    	<xsl:if test="not(preceding-sibling::*[@class='thumbnail']) or @class='captionContent'">
    		<xsl:copy>
    			<xsl:apply-templates select="node() | @*"/>
    		</xsl:copy>
    	</xsl:if>
    </xsl:template>

    <xsl:template match="x:a/@shape"/>
    <xsl:template match="x:head/@profile"/>
    <xsl:template match="x:html/@version"/>
	
	<!-- make sure there is space for pictures -->
	<xsl:template match="*[@class='entry' and *[@class='picture' or @class='thumbnail']]">
		<xsl:copy>
			<xsl:apply-templates select="@*"/>
			<xsl:attribute name="style">min-height:1.2in;</xsl:attribute>
			<xsl:apply-templates select="node()"/>
		</xsl:copy>
	</xsl:template>
	
	<!-- structure for pictures -->
	<xsl:template match="*[@class='thumbnail' and parent::*[@class='entry']]">
		<xsl:element name="div" namespace="http://www.w3.org/1999/xhtml">
			<xsl:attribute name="class">picture</xsl:attribute>
			<xsl:copy>
				<xsl:apply-templates select="@*"/>
			</xsl:copy>
			<xsl:call-template name="captions">
				<xsl:with-param name="next" select="following-sibling::*[1]"/>
			</xsl:call-template>
		</xsl:element>
	</xsl:template>
	
	<!-- structure for captions -->
	<xsl:template name="captions">
		<xsl:param name="next"/>
		<xsl:choose>
			<xsl:when test="local-name($next) = 'span'">
				<xsl:element name="div" namespace="http://www.w3.org/1999/xhtml">
					<xsl:attribute name="class">captionContent</xsl:attribute>
					<xsl:apply-templates select="$next" mode="doCopy"/>
				</xsl:element>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<!-- copy captions -->
	<xsl:template match="node() | @*" mode="doCopy">
		<xsl:copy>
			<xsl:apply-templates select="node() | @*"/>
		</xsl:copy>
	</xsl:template>
	
	<!-- Use eternal sound file -->
	<xsl:template match="x:a[local-name(preceding-sibling::*[1]) = 'audio']">
		<xsl:copy>
			<xsl:apply-templates select="@class"/>
			<xsl:attribute name="href">
				<xsl:value-of select="$avPath"/>
				<xsl:text>/</xsl:text>
				<xsl:variable name="link" select="preceding-sibling::*[1]//@src"/>
				<xsl:value-of select="translate(translate($link,' ','_'),'\','/')"/>
			</xsl:attribute>
			<xsl:apply-templates select="node()"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="x:audio|x:video"/>

	<!-- Use eternal video file -->
	<xsl:template match="x:a[contains(@href,'AudioVisual')]">
		<xsl:copy>
			<xsl:apply-templates select="@class"/>
			<xsl:attribute name="href">
				<xsl:value-of select="$avPath"/>
				<xsl:text>/</xsl:text>
				<xsl:variable name="link" select="@href"/>
				<xsl:value-of select="translate(translate($link,' ','_'),'\','/')"/>
			</xsl:attribute>
			<xsl:apply-templates select="node()"/>
		</xsl:copy>
	</xsl:template>

	<!-- Add font family for symbols -->
	<xsl:template match="text()[contains(.,'&#x1F3A5;') or contains(.,'&#x1F50A;')]">
		<xsl:choose>
			<xsl:when test="contains(.,'&#x1F50A;')">
				<xsl:value-of select="substring-before(.,'&#x1F50A;')"/>
				<xsl:text disable-output-escaping="yes"><![CDATA[<span style="font-family:Segoe UI Symbol,Symbola">&#x1F50A;</span>]]></xsl:text>
				<xsl:value-of select="substring-after(.,'&#x1F50A;')"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="substring-before(.,'&#x1F3A5;')"/>
				<xsl:text disable-output-escaping="yes"><![CDATA[<span style="font-family:Segoe UI Symbol,Symbola">&#x1F3A5;</span>]]></xsl:text>
				<xsl:value-of select="substring-after(.,'&#x1F3A5;')"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>