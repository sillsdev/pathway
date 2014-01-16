<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Section Subheaders.xsl
    # Purpose:     Add section sub header with the name of the
	#              writing systems.
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2013/12/17
    # Copyright:   (c) 2013 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:x="http://www.w3.org/1999/xhtml"
    version="1.0">
    
    <xsl:param name="title"/>
    
    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.1//EN" 
        doctype-system="http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"/>

    <!-- Recursive copy template -->   
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>
    
    <xsl:template match="x:a/@shape"/>
    <xsl:template match="x:head/@profile"/>
    <xsl:template match="x:html/@version"/>
    
    <xsl:template match="x:body/*[1]/*[1]">
        <xsl:choose>
            <xsl:when test="count(following::*[contains(@class,'reversal-form')][1])">
                <!-- Reversal -->
                <xsl:variable name="l1" select="following::*[@class='headref'][1]/@lang"/>
                <xsl:variable name="l2" select="following::*[contains(@class,'reversal-form')][1]/@lang"/>
                <xsl:variable name="l1Name" select="//x:meta[substring-before(@content, ':') = $l1]"/>
                <xsl:variable name="l2Name" select="//x:meta[substring-before(@content, ':') = $l2]"/>
                <xsl:copy>
                    <xsl:apply-templates select="@*"/>
                    <xsl:value-of select="substring-after($l2Name/@content, ':')"/>
                    <xsl:text> &#x2014; </xsl:text> <!-- em-dash -->
                    <xsl:value-of select="substring-after($l1Name/@content, ':')"/>
                    <xsl:element name="br" namespace="http://www.w3.org/1999/xhtml"/>
                    <xsl:text>&#x2028;&#x2029;&#13;&#10;</xsl:text> <!-- paragraph separator -->
                    <xsl:value-of select="text()"/>
                </xsl:copy>
            </xsl:when>
            <xsl:otherwise>
                <xsl:variable name="l1" select="following::*[@class='headword'][1]/@lang"/>
                <xsl:variable name="l2" select="following::*[@class='definition'][1]/@lang"/>
                <xsl:variable name="l1Name" select="//x:meta[substring-before(@content, ':') = $l1]"/>
                <xsl:variable name="l2Name" select="//x:meta[substring-before(@content, ':') = $l2]"/>
                <xsl:copy>
                    <xsl:apply-templates select="@*"/>
                    <xsl:value-of select="substring-after($l1Name/@content, ':')"/>
                    <xsl:text> &#x2014; </xsl:text> <!-- em-dash -->
                    <xsl:value-of select="substring-after($l2Name/@content, ':')"/>
                    <xsl:element name="br" namespace="http://www.w3.org/1999/xhtml"/>
                    <xsl:text>&#x2028;&#x2029;&#13;&#10;</xsl:text> <!-- paragraph separator -->
                    <xsl:value-of select="text()"/>
                </xsl:copy>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

</xsl:stylesheet>