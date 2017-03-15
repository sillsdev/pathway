<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        ConvertNestedDiv.xsl
    # Purpose:     Convert Nested Div to Span to avoid xhtml errors
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2011/11/30
    # Copyright:   (c) 2011 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0" xmlns:x="http://www.w3.org/1999/xhtml">

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

    <xsl:template match="x:div">
        <!-- divs may not be nested in spans -->
        <xsl:choose>
            <xsl:when test="count(ancestor::x:span) &gt; 0 and @class != 'picturesofsense'">
                <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                    <xsl:for-each select="@*">
                        <xsl:copy/>
                    </xsl:for-each>
                    <xsl:apply-templates/>
                </xsl:element>
            </xsl:when>
            <xsl:otherwise>
                <xsl:copy>
                    <xsl:for-each select="@*">
                        <xsl:copy/>
                    </xsl:for-each>
                    <xsl:apply-templates/>
                </xsl:copy>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <xsl:template match="x:span[@id]">
        <!-- spans must have unique ids -->
        <xsl:variable name="myId">
            <xsl:value-of select="@id"/>
        </xsl:variable>
        <xsl:choose>
            <xsl:when test="count(//*[@id = $myId]) &gt; 1">
                <xsl:copy>
                    <xsl:for-each select="@*">
                        <xsl:if test="local-name() != 'id'">
                            <xsl:copy/>
                        </xsl:if>
                    </xsl:for-each>
                    <xsl:apply-templates/>
                </xsl:copy>
            </xsl:when>
            <xsl:otherwise>
                <xsl:copy>
                    <xsl:for-each select="@*">
                        <xsl:copy/>
                    </xsl:for-each>
                    <xsl:apply-templates/>
                </xsl:copy>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

</xsl:stylesheet>