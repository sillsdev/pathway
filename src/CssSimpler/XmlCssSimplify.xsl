<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        XmlCssSimplify.xsl
    # Purpose:     Simplify Css in Xml format
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2016/3/23
    # Copyright:   (c) 2016 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0">

    <xsl:output encoding="utf-8" method="xml" indent="yes"/>

    <!-- Recursive copy template -->
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>

    <!-- Remove rules with div at root -->
    <xsl:template match="RULE[*[1][local-name()='TAG'][name='div']]"/>

    <!-- Put before and after on parent to eliminate need of first and last child >
    <xsl:template match="TAG[following-sibling::*[1][name='last-child']]"/>
    <xsl:template match="*[name='last-child']"/>
    <xsl:template match="TAG[following-sibling::*[1][name='first-child']]"/>
    <xsl:template match="*[name='first-child']"/ -->

    <!-- Eliminates part of selector -->
    <xsl:template match="RULE/*[position() > 1][following-sibling::*[name=parent::*/@lastClass]]">
        <xsl:choose>
            <!-- sequence is actually of clusters since translation follows the example sentence -->
            <xsl:when test="name='example' and local-name(following-sibling::*[1]) = 'PRECEDES'">
                <xsl:copy>
                    <xsl:element name="name">complete</xsl:element>
                </xsl:copy>
            </xsl:when>
            <!-- sub senses can format differently so we need this hierarchy -->
            <xsl:when test="name='senses'">
                <xsl:copy>
                    <xsl:apply-templates select="node() | @*"/>
                </xsl:copy>
            </xsl:when>
            <!-- These two when clauses retain the selector for in between text -->
            <xsl:when test="local-name(.) = 'PRECEDES' and following-sibling::*[1]/name=parent::*/@lastClass">
                <xsl:copy>
                    <xsl:apply-templates select="node() | @*"/>
                </xsl:copy>
            </xsl:when>
            <xsl:when test="local-name(following-sibling::*) = 'PRECEDES' and following-sibling::*[2]/name=parent::*/@lastClass">
                <xsl:copy>
                    <xsl:apply-templates select="node() | @*"/>
                </xsl:copy>
            </xsl:when>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template match="name[text()='example' and local-name(parent::*/preceding-sibling::*[1]) = 'PRECEDES']">
        <xsl:copy>
            <xsl:text>complete</xsl:text>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="RULE/*[local-name()='TAG' and local-name(following-sibling::*[1])='CLASS']"/>

    <!-- Handles :not() by simplifying selector -->
    <xsl:template match="*[following-sibling::*[3][name='not']]">
        <xsl:apply-templates select="following-sibling::*[1]"/>
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
        <xsl:element name="PSEUDO">
            <xsl:element name="name">
                <xsl:text>before</xsl:text>
            </xsl:element>
        </xsl:element>
    </xsl:template>
    <xsl:template match="*[following-sibling::*[2][name='not']]"/>
    <xsl:template match="*[following-sibling::*[1][name='not']]"/>
    <xsl:template match="*[name='not']"/>
    <xsl:template match="*[preceding-sibling::*[1][name='not']]"/>

    <!-- Used with indent to pretty print results -->
    <xsl:template match="text()[normalize-space(.)='']"/>
</xsl:stylesheet>