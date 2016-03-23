<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Add Columns.xsl
    # Purpose:     Add "letData" div so columns can be added
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2016/3/9
    # Copyright:   (c) 2016 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
    xmlns:x="http://www.w3.org/1999/xhtml">

    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>

    <!-- Recursive copy template -->
    <xsl:template match="node() | @*">
        <xsl:if test="not(@class='letData')">
            <xsl:copy>
                <xsl:apply-templates select="node() | @*"/>
            </xsl:copy>
        </xsl:if>
    </xsl:template>

    <xsl:template match="x:a/@shape"/>
    <xsl:template match="x:head/@profile"/>
    <xsl:template match="x:html/@version"/>

    <xsl:template match="*[@class='letHead']">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
        <xsl:element name="div" namespace="http://www.w3.org/1999/xhtml">
            <xsl:attribute name="class">letData</xsl:attribute>
            <xsl:apply-templates select="following-sibling::*[1]" mode="inCol"/>
        </xsl:element>
    </xsl:template>

    <xsl:template match="*" mode="inCol">
        <xsl:if test="not(@class='letHead')">
            <xsl:if test="not(@class='letData')">
                <xsl:copy>
                    <xsl:apply-templates select="node() | @*"/>
                </xsl:copy>
            </xsl:if>
            <xsl:apply-templates select="following-sibling::*[1]" mode="inCol"/>
        </xsl:if>
    </xsl:template>

    <xsl:template
        match="*[@class='entry'] | *[@class='reversalindexentry'] |*[starts-with(@class,'minorentry')]"/>

</xsl:stylesheet>
