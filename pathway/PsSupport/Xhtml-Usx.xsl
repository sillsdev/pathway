<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Xhtml-Usx.xsl
    # Purpose:     Transform XHTML to USX so we can make theWord/MySword
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2014/08/15
    # Copyright:   (c) 2013 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0">
    
    <xsl:param name="code">JHN</xsl:param>
    <xsl:param name="map">StyleMap.xml</xsl:param>
    
    <xsl:variable name="mapDoc" select="document($map)//style"/>
    
    <xsl:output indent="no"/>
    
    <xsl:template match="/">
        <xsl:element name="usx">
            <xsl:attribute name="version">2.0</xsl:attribute>
            <xsl:element name="book">
                <xsl:attribute name="code">
                    <xsl:value-of select="$code"/>
                </xsl:attribute>
                <xsl:text>from </xsl:text>
                <xsl:value-of select="//*[@name='linkedFilesRootDir']/@content"/>
            </xsl:element>
            <xsl:apply-templates select="//*[@class='scrBook']"/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="*[@class='scrBook']">
        <xsl:if test="*[@class='scrBookCode']/text() = $code">
            <xsl:apply-templates select="*"/>
        </xsl:if>
    </xsl:template>
    
    <xsl:template match="*[@class='scrBookCode']"/>

    <xsl:template match="*[@class='scrBookName']">
        <xsl:element name="para">
            <xsl:attribute name="style">h</xsl:attribute>
            <xsl:value-of select="text()"/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="*[@class='Title_Main']">
        <xsl:apply-templates select="*[@class]"/>
        <xsl:element name="para">
            <xsl:attribute name="style">mt</xsl:attribute>
            <xsl:value-of select="*[not(@class)]"/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="*[@class='Title_Secondary']">
        <xsl:apply-templates select="*"/>
        <xsl:element name="para">
            <xsl:attribute name="style">mt2</xsl:attribute>
            <xsl:value-of select="text()"/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="*[@class='scrIntroSection']">
        <xsl:apply-templates select="*"/>
    </xsl:template>
    
    <xsl:template match="*[@class='Intro_Paragraph']">
        <xsl:element name="para">
            <xsl:attribute name="style">ip</xsl:attribute>
            <xsl:value-of select=".//text()"/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="*[@class='columns']">
        <xsl:for-each select=".//*[@class='Chapter_Number']">
            <xsl:element name="chapter">
                <xsl:attribute name="number">
                    <xsl:value-of select="text()"/>
                </xsl:attribute>
                <xsl:attribute name="style">c</xsl:attribute>
            </xsl:element>
            <xsl:apply-templates select="parent::*" mode="v"/>
        </xsl:for-each>
    </xsl:template>
    
    <xsl:template match="*" mode="p">
        <xsl:variable name="prevWithClass" select="(ancestor-or-self::*[@class])[last()]"/>
        <xsl:choose>
            <xsl:when test="$prevWithClass/@class = 'Section_Head'">
                <xsl:apply-templates select="preceding::*[1]" mode="p"/>
                <xsl:element name="para">
                    <xsl:attribute name="style">s</xsl:attribute>
                    <xsl:value-of select="text()"/>
                </xsl:element>
            </xsl:when>
            <xsl:when test="$prevWithClass/@class = 'Section_Head_Minor'">
                <xsl:apply-templates select="preceding::*[1]" mode="p"/>
                <xsl:element name="para">
                    <xsl:attribute name="style">s2</xsl:attribute>
                    <xsl:value-of select="text()"/>
                </xsl:element>
            </xsl:when>
            <xsl:when test="$prevWithClass/@class = 'Parallel_Passage_Reference'">
                <xsl:apply-templates select="preceding::*[1]" mode="p"/>
                <xsl:element name="para">
                    <xsl:attribute name="style">r</xsl:attribute>
                    <xsl:value-of select="text()"/>
                </xsl:element>
            </xsl:when>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template match="*" mode="v">
        <xsl:variable name="class" select="@class"/>
        <xsl:variable name="pstyle" select="$mapDoc[@te = $class and @level='p']"/>
        <xsl:choose>
            <xsl:when test="count($pstyle) != 0">
                <xsl:apply-templates select="preceding::*[1]" mode="p"/>
                <xsl:element name="para">
                    <xsl:attribute name="style">
                        <xsl:value-of select="$pstyle/@pt"/>
                    </xsl:attribute>
                    <xsl:apply-templates select="node()" mode="v"/>
                </xsl:element>
                <xsl:apply-templates select="following::*[1]" mode="v"/>
            </xsl:when>
            <!-- Character Styles -->
            <xsl:when test="@class = 'Quoted_Text'">
                <xsl:element name="char">
                    <xsl:attribute name="style">qt</xsl:attribute>
                    <xsl:value-of select="text()"/>
                </xsl:element>
            </xsl:when>
            <xsl:when test="@class = 'See_In_Glossary'">
                <xsl:element name="char">
                    <xsl:attribute name="style">w</xsl:attribute>
                    <xsl:value-of select="text()"/>
                </xsl:element>
            </xsl:when>
            <xsl:when test="@class = 'Words_Of_Christ'">
                <xsl:element name="char">
                    <xsl:attribute name="style">wj</xsl:attribute>
                    <xsl:value-of select="text()"/>
                </xsl:element>
            </xsl:when>
            <xsl:when test="@class = 'Chapter_Number'"/>
            <xsl:when test="@class = 'Verse_Number'">
                <xsl:element name="verse">
                    <xsl:attribute name="number">
                        <xsl:value-of select="text()"/>
                    </xsl:attribute>
                    <xsl:attribute name="style">v</xsl:attribute>
                </xsl:element>
            </xsl:when>
            <xsl:when test="@class = 'scrSection'">
                <xsl:apply-templates select="child::*[1]" mode="v"/>
            </xsl:when>
            <xsl:when test="@class = 'Section_Head' or @class = 'Section_Head_Minor' or @class = 'Parallel_Passage_Reference'">
                <xsl:apply-templates select="following::*[1]" mode="v"/>
            </xsl:when>
            <xsl:when test="@class = 'scrFootnoteMarker'"/>
            <xsl:when test="@class = 'Note_General_Paragraph'">
                <xsl:element name="note">
                    <xsl:attribute name="caller">+</xsl:attribute>
                    <xsl:attribute name="style">f</xsl:attribute>
                    <xsl:element name="char">
                        <xsl:attribute name="style">fr</xsl:attribute>
                        <xsl:attribute name="closed">false</xsl:attribute>
                        <xsl:value-of select="(preceding::*[@class = 'Chapter_Number'])[last()]/text()"/>
                        <xsl:text>:</xsl:text>
                        <xsl:value-of select="(preceding::*[@class = 'Verse_Number'])[last()]/text()"/>
                    </xsl:element>
                    <xsl:element name="char">
                        <xsl:attribute name="style">ft</xsl:attribute>
                        <xsl:attribute name="closed">false</xsl:attribute>
                        <xsl:value-of select=".//text()"/>
                    </xsl:element>
                </xsl:element>
            </xsl:when>
            <xsl:when test="@class = 'pictureCenter'">
                <xsl:element name="figure">
                    <xsl:attribute name="style">fig</xsl:attribute>
                    <xsl:attribute name="desc">
                        <xsl:value-of select="normalize-space(.//*[@class = 'pictureCaption']//text())"/>
                    </xsl:attribute>
                    <xsl:attribute name="file">
                        <xsl:value-of select=".//@src"/>
                    </xsl:attribute>
                    <xsl:attribute name="ref">
                        <xsl:value-of select="(preceding::*[@class = 'scrBookName'])[last()]/text()"/>
                        <xsl:text> </xsl:text>
                        <xsl:value-of select="(preceding::*[@class = 'Chapter_Number'])[last()]/text()"/>
                        <xsl:text>:</xsl:text>
                        <xsl:value-of select="(preceding::*[@class = 'Verse_Number'])[last()]/text()"/>
                    </xsl:attribute>
                </xsl:element>
            </xsl:when>
            <xsl:when test="@class = 'scrBook'"/>
            <xsl:otherwise>
                <xsl:if test="count(@class) != 0">
                    <xsl:message>
                        <xsl:text>Unrecognized class = </xsl:text> 
                        <xsl:value-of select="@class"/>
                    </xsl:message>
                </xsl:if>
                <xsl:value-of select="text()"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
</xsl:stylesheet>