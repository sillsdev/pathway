<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        xhtml2xpw-scripture.xsl
    # Purpose:     Given the XMTML Scripture content, prepare XPWtool TeX content.
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2010/2/15
    # RCS-ID:      $Id: xhtml2xpw-scripture.xsl $
    # Copyright:   (c) 2010 SIL
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0" 
    xmlns:x="http://www.w3.org/1999/xhtml" xmlns:fn="http://www.w3.org/2005/xpath-functions">  
    <xsl:output encoding="UTF-8" method="text" indent="no" />
    <xsl:param name="ver" select="'bgt'"/>
    <xsl:param name="l1" select="'xxx'"/>
    <xsl:param name="l2" select="'xxx'"/>
    
    <xsl:template match="/">
        <xsl:apply-templates/>
    </xsl:template>
    
    <xsl:template match="x:html">
        <xsl:apply-templates/>
    </xsl:template>
    
    <xsl:template match="x:body">
        <xsl:text>\define\vernac\langone
\enablemode[scripture] 
</xsl:text>
        <xsl:apply-templates/>
    </xsl:template>
    
    <xsl:template match="x:div">
        <xsl:choose>
            <xsl:when test="@class='scrBook'">
                <xsl:text>\startBook{</xsl:text>
                <xsl:value-of select="x:span[@class='scrBookName']/text()"/>
                <xsl:text>}
</xsl:text>
                <xsl:apply-templates/>
                <xsl:text>\stopBook{</xsl:text>
                <xsl:value-of select="x:span[@class='scrBookName']/text()"/>
                <xsl:text>}
</xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='Title_Main'">
                <xsl:text>\BStm \vermac </xsl:text>
                <xsl:value-of select="x:span[@class='Title_Secondary']/text()"/>
                <xsl:text>\EStm
</xsl:text>
                <xsl:text>\BStm \vermac </xsl:text>
                <xsl:value-of select="x:span[2]/text()"/>
                <xsl:text>\EStm
</xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='scrIntroSection'">
                <xsl:choose>
                    <xsl:when test="x:div[@class='Intro_List_Item1']">
                        <xsl:if test="x:div[@class='Intro_Section_Head']">
                            <xsl:text>\BSish \vermac </xsl:text>
                            <xsl:value-of select="x:div[@class='Intro_Section_Head']/x:span/text()"/>
                            <xsl:text>\ESish
</xsl:text>
                        </xsl:if>
                        <xsl:text>\BSilist
</xsl:text>
                        <xsl:for-each select="x:div[@class='Intro_List_Item1']">
                            <xsl:text>\BSilisti \vermac </xsl:text>
                            <xsl:value-of select="x:span/text()"/>
                            <xsl:text>\ESilisti
</xsl:text>
                        </xsl:for-each>
                        <xsl:text>\ESilist
</xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:apply-templates/>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:when>
            
            <xsl:when test="@class='Intro_Section_Head'">
                <xsl:text>\BSish </xsl:text>
                <xsl:apply-templates/>
                <xsl:text>\ESish
</xsl:text>
            </xsl:when>

            <xsl:when test="@class='Intro_Paragraph'">
                <xsl:call-template name="para"/>
            </xsl:when>
            
            <xsl:when test="@class='columns'">
                <xsl:text>\Bduocol
</xsl:text>
                <xsl:apply-templates/>
                <xsl:text>\Bduocol
</xsl:text>
            </xsl:when>

            <xsl:when test="@class='scrSection'">
                <xsl:text>\BSsec
</xsl:text>
                <xsl:apply-templates/>
                <xsl:text>\BSsec
</xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='Paragraph'">
                <xsl:call-template name="para"/>
            </xsl:when>
            
            <xsl:when test="@class='Section_Head_Major'">
                <xsl:text>\BSshm{</xsl:text>
                <xsl:value-of select="x:span/text()"/>
                <xsl:text>}\ESshm
</xsl:text>
            </xsl:when>
            
            <xsl:otherwise>
                <xsl:apply-templates/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template match="x:span">
        <xsl:choose>
            <xsl:when test="@class='Chapter_Number'">
                <xsl:text>\BScnum{</xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>}\EScnum\startChapter{</xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>}
</xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='Verse_Number'">
                <xsl:text>\startVerse{</xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>}\BSvnum{</xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>}\ESvnum
</xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='scrBookName'"/>
            
            <xsl:otherwise>
                <xsl:call-template name="lang"/>
                <xsl:value-of select="text()"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="para">
        <xsl:for-each select="x:div[@class='pictureCenter']">
            <xsl:text>\colimage{</xsl:text>
            <xsl:variable name="srcFile" select="fn:data(x:img/@src)"/>
            <xsl:value-of select="fn:substring($srcFile,1,fn:string-length($srcFile)-4)"/>
            <xsl:text>}</xsl:text>
            <xsl:text>{</xsl:text>
            <xsl:value-of select="x:div[@class='pictureCaption']/x:span"/>
            <xsl:text>}</xsl:text>
        </xsl:for-each>
        
        <xsl:text>\BSipar </xsl:text>
        <xsl:apply-templates/>
        <xsl:text>\ESipar
        </xsl:text>
    </xsl:template>
    
    <xsl:template name="lang">
        <xsl:choose>
            <xsl:when test="@lang=$ver">
                <xsl:text>\vernac </xsl:text>
            </xsl:when>
            <xsl:when test="@lang=$l1">
                <xsl:text>\langone </xsl:text>
            </xsl:when>
            <xsl:when test="@lang=$l2">
                <xsl:text>\langtwo </xsl:text>
            </xsl:when>
        </xsl:choose>
    </xsl:template>

    <!-- The next construct removes stray text from apply-templates. Text must be explicitly processed. -->
    <xsl:template match="text()"></xsl:template>
</xsl:stylesheet>
