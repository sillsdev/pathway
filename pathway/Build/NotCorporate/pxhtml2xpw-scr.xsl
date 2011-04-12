<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        xhtml2xpw-scripture-tpl.xsl
    # Purpose:     Given the XMTML Scripture content, prepare XPWtool TeX content.
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2010/2/15
    # Changes from xhtml2xpw-scripture.xsl - remove namespace def for x, use stringLength
    # RCS-ID:      $Id: xhtml2xpw-scripture-tpl.xsl $
    # Copyright:   (c) 2010 SIL
    # Licence:     <MIT>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" 
    xmlns:fn="http://www.w3.org/2005/xpath-functions">  
    <xsl:output encoding="UTF-8" method="text" indent="no" />
    <xsl:param name="ver" select="'bzh'"/>
    <xsl:param name="l1" select="'en'"/>
    <xsl:param name="l2" select="'tpi'"/>
    <xsl:param name="l3" select="'bzh-fonipa'"/>
    <xsl:param name="l4" select="'cmn'"/>
    <xsl:param name="verFont" select="'\fonta '"/>
    <xsl:param name="l1Font" select="'\fonta '"/>
    <xsl:param name="l2Font" select="'\fontb '"/>
    <xsl:param name="l3Font" select="'\fonta '"/>
    <xsl:param name="l4Font" select="'\fonte '"/>
    
    <xsl:template match="/">
        <xsl:apply-templates/>
    </xsl:template>
    
    <xsl:template match="html">
        <xsl:apply-templates/>
    </xsl:template>
    
    <xsl:template match="body">
        <xsl:apply-templates/>
    </xsl:template>
    
    <xsl:template match="div">
        <xsl:choose>
            <xsl:when test="@class='scrBook'">
                <xsl:text>\startBook{</xsl:text>
                <xsl:value-of select="span[@class='scrBookName']/text()"/>
                <xsl:text>}
</xsl:text>
                <xsl:apply-templates/>
                <xsl:text>\stopBook{</xsl:text>
                <xsl:value-of select="span[@class='scrBookName']/text()"/>
                <xsl:text>}
</xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='Title_Main'">
                <xsl:text>\BStm </xsl:text>
                <xsl:value-of select="$verFont"/>
                <xsl:value-of select="span[@class='Title_Secondary']/text()"/>
                <xsl:text>\EStm
</xsl:text>
                <xsl:text>\BStm </xsl:text>
                <xsl:value-of select="$verFont"/>
                <xsl:value-of select="span[2]/text()"/>
                <xsl:text>\EStm
</xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='scrIntroSection'">
                <xsl:choose>
                    <xsl:when test="div[@class='Intro_List_Item1']">
                        <xsl:if test="div[@class='Intro_Section_Head']">
                            <xsl:text>\BSish </xsl:text>
                            <xsl:value-of select="$verFont"/>
                            <xsl:value-of select="div[@class='Intro_Section_Head']/span/text()"/>
                            <xsl:text>\ESish
</xsl:text>
                        </xsl:if>
                        <xsl:text>\BSilist
</xsl:text>
                        <xsl:for-each select="div[@class='Intro_List_Item1']">
                            <xsl:text>\BSilisti </xsl:text>
                            <xsl:value-of select="$verFont"/>
                            <xsl:value-of select="span/text()" disable-output-escaping="yes"/>
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
                <xsl:call-template name="para" />
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
                <xsl:call-template name="para" />
            </xsl:when>
            
            <xsl:when test="@class='Paragraph1'">
                <xsl:call-template name="para" >
                    <xsl:with-param name="tag">
                        <xsl:text>ipar </xsl:text>
                    </xsl:with-param>
                </xsl:call-template>
            </xsl:when>
            
            <xsl:when test="@class='Section_Head_Major'">
                <xsl:text>\BSshm{</xsl:text>
                <xsl:value-of select="span/text()"/>
                <xsl:text>}\ESshm
</xsl:text>
            </xsl:when>
            
            <xsl:otherwise>
                <xsl:apply-templates/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template match="span">
        <xsl:choose>
            <xsl:when test="@class='Chapter_Number'">
                <xsl:text>\BScnum{</xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>}\EScnum\startChapter{</xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>}
</xsl:text>
            </xsl:when>
            
            <xsl:when test="contains('#Verse_Number#Verse_Number1#', concat('#', @class, '#'))">
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
                <xsl:value-of select="text()" disable-output-escaping="yes"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="para">
        <xsl:param name="tag">
            <xsl:text>par </xsl:text>
        </xsl:param>
        <xsl:for-each select="div[@class='pictureCenter']">
            <xsl:text> \textimage{</xsl:text>
            <xsl:variable name="srcFile" select="fn:data(img/@src)"/>
            <xsl:value-of select="fn:substring($srcFile,1,fn:stringLength($srcFile)-4)"/>
            <xsl:text>}</xsl:text>
            <xsl:text>{</xsl:text>
            <xsl:value-of select="div[@class='pictureCaption']/span"/>
            <xsl:text>}
            </xsl:text>
        </xsl:for-each>
        
        <xsl:text>\BS</xsl:text>
        <xsl:value-of select="$tag"/>
        <xsl:apply-templates/>
        <xsl:text>\ES</xsl:text>
        <xsl:value-of select="$tag"/>
        <xsl:text>
</xsl:text>
    </xsl:template>
    
    <xsl:template name="lang">
        <xsl:choose>
            <xsl:when test="@lang=$ver">
                <xsl:value-of select="$verFont"/>
            </xsl:when>
            <xsl:when test="@lang=$l1">
                <xsl:value-of select="$l1Font"/>
            </xsl:when>
            <xsl:when test="@lang=$l2">
                <xsl:value-of select="$l2Font"/>
            </xsl:when>
            <xsl:when test="@lang=$l3">
                <xsl:value-of select="$l3Font"/>
            </xsl:when>
            <xsl:when test="@lang=$l4">
                <xsl:value-of select="$l4Font"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:text>\fonta </xsl:text>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <!-- The next construct removes stray text from apply-templates. Text must be explicitly processed. -->
    <xsl:template match="text()"></xsl:template>
</xsl:stylesheet>
