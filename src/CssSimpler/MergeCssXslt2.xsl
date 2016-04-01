<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        MergeCssXslt2.xsl
    # Purpose:     Merge rules with same class name
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2016/3/16
    # Copyright:   (c) 2016 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0">

    <xsl:output method="xml" encoding="utf-8"/>
    
    <!-- Copy nodes -->
    <xsl:template match="node()|@*">
        <xsl:copy>
            <xsl:apply-templates select="node()|@*"/>
        </xsl:copy>
    </xsl:template>
    
    <xsl:template match="xsl:template">
        <xsl:variable name="class" select=".//*[@name='class']"/>
        <xsl:choose>
            <xsl:when test="following-sibling::*//*[@name='class'] = $class"/>
            <xsl:when test="preceding-sibling::*//*[@name='class'] = $class">
                <xsl:copy>
                    <xsl:choose>
                        <xsl:when test="contains(@match, '[preceding-')">
                            <xsl:attribute name="{local-name(@match)}">
                                <xsl:value-of select="substring-before(@match,'[preceding-')"/>
                            </xsl:attribute>
                        </xsl:when>
                        <xsl:when test="contains(@match,'last()')">
                            <xsl:attribute name="{local-name(@match)}">
                                <xsl:value-of select="substring-before(@match,'[last()]')"/>
                            </xsl:attribute>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:apply-templates select="@*"/>
                        </xsl:otherwise>
                    </xsl:choose>
                    <xsl:element name="xsl:choose">
                        <xsl:call-template name="ConditionedContent"/>
                        <xsl:for-each select="preceding-sibling::*[.//*[@name='class']=$class]">
                            <xsl:call-template name="ConditionedContent"/>
                        </xsl:for-each>
                    </xsl:element>
                    <xsl:apply-templates select="*[local-name() = 'copy']"/>
                    <xsl:if test="local-name(*[2])='element' OR preceding-sibling::*[.//text()=$class][xsl:element]">
                        <xsl:element name="xsl:choose">
                            <xsl:call-template name="ConditionedContent">
                                <xsl:with-param name="node" select="*[2]"/>
                            </xsl:call-template>
                            <xsl:for-each select="preceding-sibling::*[.//*[@name='class']=$class]">
                                <xsl:call-template name="ConditionedContent">
                                    <xsl:with-param name="node" select="*[2]"/>
                                </xsl:call-template>
                            </xsl:for-each>
                        </xsl:element>
                    </xsl:if>
                </xsl:copy>
            </xsl:when>
            <xsl:otherwise>
                <xsl:copy>
                    <xsl:apply-templates select="node()|@*"/>
                </xsl:copy>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <xsl:template name="ConditionedContent">
        <xsl:param name="node" select="*[1]"/>
        <xsl:if test="local-name($node) = 'element'">
            <xsl:choose>
                <xsl:when test="contains(@match,'preceding-')">
                    <xsl:element name="xsl:when">
                        <xsl:attribute name="test">
                            <xsl:text>preceding-</xsl:text>
                            <xsl:variable name="precVal" select="substring-after(@match,'preceding-')"/>
                            <xsl:value-of
                                select="substring($precVal,1,string-length($precVal)-1)"/>
                        </xsl:attribute>
                        <xsl:apply-templates select="$node"/>
                    </xsl:element>
                </xsl:when>
                <xsl:when test="contains(@match,'last()')">
                    <xsl:element name="xsl:when">
                        <xsl:attribute name="test">position()=last()</xsl:attribute>
                        <xsl:apply-templates select="$node"/>
                    </xsl:element>
                </xsl:when>
                <xsl:when test="contains(@match,'first()')">
                    <xsl:element name="xsl:when">
                        <xsl:attribute name="test">position()=1</xsl:attribute>
                        <xsl:apply-templates select="$node"/>
                    </xsl:element>
                </xsl:when>
            </xsl:choose>
        </xsl:if>
    </xsl:template>    
</xsl:stylesheet>