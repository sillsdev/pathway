<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        addDicTocHeads.xsl
    # Purpose:     Add Main and Reversal Index headers to Epub Dictionary TOC (NCX)
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2012/04/05
    # Copyright:   (c) 2012 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
    xmlns:ncx="http://www.daisy.org/z3986/2005/ncx/">
    <xsl:param name="mainName">Main</xsl:param>
    <xsl:param name="revName">Reversal Index</xsl:param>
    
    <!-- Recursive copy template -->
    <xsl:template match="ncx:ncx | ncx:head | ncx:meta | ncx:docTitle | ncx:text | ncx:navMap | ncx:navLabel | ncx:content | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>
    
    <!-- Add Main header -->
    <xsl:template match="ncx:navPoint">
        <xsl:param name="subCopy" select="No"/>
        <xsl:choose>
            <!-- Test for beginning of main section -->
            <xsl:when test="ncx:content[@src='PartFile00001_.xhtml']">
                <xsl:copy>
                    <xsl:apply-templates select="@*"/>
                    <xsl:element name="navLabel" namespace="http://www.daisy.org/z3986/2005/ncx/">
                        <xsl:element name="text" namespace="http://www.daisy.org/z3986/2005/ncx/">
                            <xsl:value-of select="$mainName"/>
                        </xsl:element>
                    </xsl:element>
                    <xsl:element name="content" namespace="http://www.daisy.org/z3986/2005/ncx/">
                        <xsl:attribute name="src">PartFile00001_.xhtml#body</xsl:attribute>
                    </xsl:element>
                    <xsl:for-each select=". | following-sibling::node()">
                        <xsl:if test="contains(ncx:content/@src,'Part')">
                            <xsl:copy>
                                <xsl:apply-templates select="node() | @*">
                                    <xsl:with-param name="subCopy">Yes</xsl:with-param>
                                </xsl:apply-templates>
                            </xsl:copy>
                        </xsl:if>
                    </xsl:for-each>
                </xsl:copy>
            </xsl:when>
            
			<xsl:when test="ncx:content[@src='PartFile00001_01.xhtml']">
                <xsl:copy>
                    <xsl:apply-templates select="@*"/>
                    <xsl:element name="navLabel" namespace="http://www.daisy.org/z3986/2005/ncx/">
                        <xsl:element name="text" namespace="http://www.daisy.org/z3986/2005/ncx/">
                            <xsl:value-of select="$mainName"/>
                        </xsl:element>
                    </xsl:element>
                    <xsl:element name="content" namespace="http://www.daisy.org/z3986/2005/ncx/">
                        <xsl:attribute name="src">PartFile00001_01.xhtml#body</xsl:attribute>
                    </xsl:element>
                    <xsl:for-each select=". | following-sibling::node()">
                        <xsl:if test="contains(ncx:content/@src,'Part')">
                            <xsl:copy>
                                <xsl:apply-templates select="node() | @*">
                                    <xsl:with-param name="subCopy">Yes</xsl:with-param>
                                </xsl:apply-templates>
                            </xsl:copy>
                        </xsl:if>
                    </xsl:for-each>
                </xsl:copy>
            </xsl:when>
            <!-- Test for beginning of reversal section -->
            <xsl:when test="ncx:content[@src='RevIndex00001_.xhtml']">
                <xsl:copy>
                    <xsl:apply-templates select="@*"/>
                    <xsl:element name="navLabel" namespace="http://www.daisy.org/z3986/2005/ncx/">
                        <xsl:element name="text" namespace="http://www.daisy.org/z3986/2005/ncx/">
                            <xsl:value-of select="$revName"/>
                        </xsl:element>
                    </xsl:element>
                    <xsl:element name="content" namespace="http://www.daisy.org/z3986/2005/ncx/">
                        <xsl:attribute name="src">RevIndex00001_.xhtml#body</xsl:attribute>
                    </xsl:element>
                    <xsl:for-each select=". | following-sibling::node()">
                        <xsl:if test="contains(ncx:content/@src,'Rev')">
                            <xsl:copy>
                                <xsl:apply-templates select="node() | @*">
                                    <xsl:with-param name="subCopy">Yes</xsl:with-param>
                                </xsl:apply-templates>
                            </xsl:copy>
                        </xsl:if>
                    </xsl:for-each>
                </xsl:copy>
            </xsl:when>

			<xsl:when test="ncx:content[@src='RevIndex00001_01.xhtml']">
                <xsl:copy>
                    <xsl:apply-templates select="@*"/>
                    <xsl:element name="navLabel" namespace="http://www.daisy.org/z3986/2005/ncx/">
                        <xsl:element name="text" namespace="http://www.daisy.org/z3986/2005/ncx/">
                            <xsl:value-of select="$revName"/>
                        </xsl:element>
                    </xsl:element>
                    <xsl:element name="content" namespace="http://www.daisy.org/z3986/2005/ncx/">
                        <xsl:attribute name="src">RevIndex00001_01.xhtml#body</xsl:attribute>
                    </xsl:element>
                    <xsl:for-each select=". | following-sibling::node()">
                        <xsl:if test="contains(ncx:content/@src,'Rev')">
                            <xsl:copy>
                                <xsl:apply-templates select="node() | @*">
                                    <xsl:with-param name="subCopy">Yes</xsl:with-param>
                                </xsl:apply-templates>
                            </xsl:copy>
                        </xsl:if>
                    </xsl:for-each>
                </xsl:copy>
            </xsl:when>
            <!-- Two cases: we are copying the nodes under main or rev or we are copying front matter -->
            <xsl:otherwise>
                <xsl:if test="$subCopy = 'Yes' or starts-with(ncx:content/@src, 'File')">
                    <xsl:copy>
                        <xsl:apply-templates select="node() | @*">
                            <xsl:with-param name="subCopy">
                                <xsl:value-of select="$subCopy"/>
                            </xsl:with-param>
                        </xsl:apply-templates>
                    </xsl:copy>
                </xsl:if>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
</xsl:stylesheet>
