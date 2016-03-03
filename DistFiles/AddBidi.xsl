<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        AddBidi.xsl
    # Purpose:     Add Bidi chars around rtl writing systems
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2014/03/27
    # Copyright:   (c) 2013 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:x="http://www.w3.org/1999/xhtml"
    version="1.0">
    
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

    <xsl:template match="*[@lang = 'ur']">
        <xsl:copy>
            <xsl:apply-templates select="@*"/>
            <xsl:for-each select="node()">
                <xsl:choose>
                    <!-- The last item in the current Fieldworks export is the space between fields -->
                    <xsl:when test="position() = last()">
                        <xsl:text disable-output-escaping="yes"><![CDATA[&#x200e;]]></xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:text disable-output-escaping="yes"><![CDATA[&#x200f;]]></xsl:text>
                    </xsl:otherwise>
                </xsl:choose>
                <xsl:apply-templates select="."/>
            </xsl:for-each>
        </xsl:copy>
    </xsl:template>
    
</xsl:stylesheet>