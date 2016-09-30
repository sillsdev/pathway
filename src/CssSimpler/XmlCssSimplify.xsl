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
    <xsl:template match="RULE/*[position() > 1 and local-name() != 'TAG' and following-sibling::*[name=parent::*/@lastClass]]">
        <xsl:choose>
            <!-- remove pictures span -->
            <xsl:when test="name='pictures'"/>
            <xsl:when test="local-name() = 'PARENTOF' and following-sibling::*[1]/name = 'pictures'"/>
            <xsl:when test="local-name() = 'PARENTOF' and preceding-sibling::*[1]/name = 'pictures'"/>
            <!-- sequence is actually of clusters since translation follows the example sentence -->
            <xsl:when test="name='example' and local-name(following-sibling::*[1]) = 'PRECEDES'">
                <xsl:copy>
                    <xsl:element name="name">complete</xsl:element>
                </xsl:copy>
            </xsl:when>
            <!-- sub senses can format differently so we need this hierarchy -->
            <xsl:when test="name='senses' or name='subentries' or contains(name,'mainentrysubentries') or name='complexformsnotsubentries' or name='complexformentryrefs' or name='referencedentries' or name='minimallexreferences'">
                <xsl:copy>
                    <xsl:apply-templates select="node() | @*"/>
                </xsl:copy>
            </xsl:when>
            <!-- In order to retain sense hierarchy -->
            <xsl:when test="local-name(.) = 'PARENTOF' and following-sibling::*[1]/name='senses' or local-name(following-sibling::*[1]) = 'PARENTOF' and following-sibling::*[2]/name='senses' or local-name(.) = 'PARENTOF' and preceding-sibling::*[1]/name='senses' or local-name(preceding-sibling::*[1]) = 'PARENTOF'">
               <xsl:copy>
                  <xsl:apply-templates select="node() | @*"/>
               </xsl:copy>
            </xsl:when>
            <!-- These two when clauses retain the selector for in between text -->
            <xsl:when test="local-name(.) = 'PRECEDES' or local-name(following-sibling::*[1]) = 'PRECEDES' or local-name(preceding-sibling::*[1]) = 'PRECEDES'">
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
    
    <!-- This plus the when clause above change example to complete when necessary -->
    <xsl:template match="name[text()='example' and local-name(parent::*/preceding-sibling::*[1]) = 'PRECEDES']">
        <xsl:copy>
            <xsl:text>complete</xsl:text>
        </xsl:copy>
    </xsl:template>

    <!-- Remove tag class combinations -->
    <xsl:template match="RULE/*[local-name()='TAG' and local-name(following-sibling::*[1])='CLASS']"/>

    <!-- Used with indent to pretty print results -->
    <xsl:template match="text()[normalize-space(.)='']"/>
</xsl:stylesheet>