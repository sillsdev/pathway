<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        xhtml2dex.xsl
    # Purpose:     Given the XMTML dictionary content, prepare DEtool TeX content.
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2009/11/23
    # Changes from xhtml2dex.xsl - use stringLength instead of string-length
    # RCS-ID:      $Id: xhtml2dex.xsl $
    # Copyright:   (c) 2009 SIL
    # Licence:     <MIT>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" 
    xmlns:fn="http://www.w3.org/2005/xpath-functions"
    xmlns:x="http://www.w3.org/1999/xhtml">  
    <xsl:output encoding="UTF-8" method="text" indent="no"/>
    <xsl:param name="ver" select="'bzh'"/>
    <xsl:param name="l1" select="'en'"/>
    <xsl:param name="l2" select="'tpi'"/>
    
    <xsl:template match="/">
        <xsl:apply-templates/>
    </xsl:template>
    
    <xsl:template match="x:html">
        <xsl:apply-templates/>
    </xsl:template>
    
    <xsl:template match="x:body">
        <xsl:text>\Bdict
        </xsl:text>
        <xsl:apply-templates/>
        <xsl:text>\Edict
        </xsl:text>
    </xsl:template>
    
    <xsl:template match="x:div">
        <xsl:choose>
            <xsl:when test="@class='letter'">
                <xsl:text>\Bletter </xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>\Eletter
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='entry'">
                <xsl:for-each select="x:div[@class='pictureRight']">
                    <xsl:text>\colimage{</xsl:text>
                    <xsl:variable name="srcFile" select="fn:data(x:img/@src)"/>
                    <xsl:value-of select="fn:substring($srcFile,1,fn:stringLength($srcFile)-4)"/>
                    <xsl:text>}</xsl:text>
                    <xsl:text>{</xsl:text>
                    <xsl:value-of select="x:div/x:span[@class='pictureLabel']/x:span"/>
                    <xsl:text>}</xsl:text>
                </xsl:for-each>
                
                <xsl:text>\Bentry
                </xsl:text>
                <xsl:apply-templates/>
                <xsl:text>\Eentry
                    
                </xsl:text>
            </xsl:when>
            
            <xsl:otherwise>
                <xsl:apply-templates/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template match="x:span">
        <xsl:choose>
            <xsl:when test="@class='headword'">
                <xsl:text>\Bhw </xsl:text>
                <xsl:call-template name="lang"/>
                <xsl:value-of select="text()"/>
                <xsl:apply-templates/>
                <xsl:text>\Ehw</xsl:text>
                <xsl:text>\marking[guidewords]{</xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>}
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='pronunciation'">
                <xsl:text>\Bpr </xsl:text>
                <xsl:call-template name="SpanNoLang"/>
                <xsl:text>\Epr
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='partofspeech'">
                <xsl:text>\Bps </xsl:text>
                <xsl:call-template name="SpanNoLang"/>
                <xsl:text>\Eps
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='xhomographnumber'">
                <xsl:text>\Bhn{</xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>}\Ehn </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='xsensenumber'">
                <xsl:text>\Bsn </xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>\Esn
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='definition'">
                <xsl:text>\Bde </xsl:text>
                <xsl:call-template name="xitem">
                    <xsl:with-param name="delim">
                        <xsl:text> / </xsl:text>
                    </xsl:with-param>
                </xsl:call-template>
                <xsl:text>\Ede
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='example'">
                <xsl:text>\Bex </xsl:text>
                <xsl:choose>
                    <xsl:when test="x:span">
                        <xsl:call-template name="xitem"/>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:call-template name="lang"/>
                        <xsl:value-of select="normalize-space(text())"/>
                    </xsl:otherwise>
                </xsl:choose>
                <xsl:text>\Eex
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='translation'">
                <xsl:text>\Btr </xsl:text>
                <xsl:call-template name="xitem"/>
                <xsl:text>\Etr
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='lexref-type'">
                <xsl:text>\Blr </xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>:</xsl:text>
                <xsl:text>\Elr </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='lexref-targets'">
                <xsl:text>\Bsx </xsl:text>
                <xsl:call-template name="xitem">
                    <xsl:with-param name="delim">
                        <xsl:text>, </xsl:text>
                    </xsl:with-param>
                </xsl:call-template>
                <xsl:text>\Esx
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='sense-crossref'">
                <xsl:call-template name="lang"/>
                <xsl:value-of select="normalize-space(text())"/>
                <xsl:apply-templates/>
            </xsl:when>
            
            <xsl:when test="@class='headword-minor'">
                <xsl:text>\Bhw </xsl:text>
                <xsl:call-template name="lang"/>
                <xsl:value-of select="text()"/>
                <xsl:apply-templates/>
                <xsl:text>\Ehw</xsl:text>
                <xsl:text>\marking[guidewords]{</xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>}
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='pronunciation-minor'">
                <xsl:text>\Bpr </xsl:text>
                <xsl:value-of select="text()"/>
                <xsl:text>\Epr
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='LexEntryType-publishStemMinorEntryType-AbbreviationPub'">
                <xsl:text>\Bde </xsl:text>
                <xsl:call-template name="lang"/>
                <xsl:value-of select="text()"/>
                <xsl:text>\Ede
                </xsl:text>
            </xsl:when>
            
            <xsl:otherwise>
                <xsl:apply-templates/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="SpanNoLang">
        <xsl:choose>
            <xsl:when test="x:span">
                <!-- 6.0.4 -->
                <xsl:value-of select="normalize-space(x:span/text())"/>
            </xsl:when>
            <xsl:otherwise>
                <!-- 6.0.3 -->
                <xsl:value-of select="normalize-space(text())"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="xitem">
        <xsl:param name="delim">
            <xsl:text xml:space="preserve"> </xsl:text>
        </xsl:param>
        <xsl:choose>
            <xsl:when test="x:span[@class='xitem']">
                <xsl:for-each select="x:span">
                    <xsl:choose>
                        <xsl:when test="x:span[@class]">
                            <xsl:apply-templates/>
                        </xsl:when>
                        <xsl:when test="x:span">
                            <xsl:for-each select="x:span">
                                <xsl:call-template name="lang"/>
                                <xsl:value-of select="normalize-space(text())"/>
                                <xsl:if test="not(position()=last())">
                                    <xsl:value-of select="$delim"/>
                                </xsl:if>
                            </xsl:for-each>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:call-template name="lang"/>
                            <xsl:value-of select="normalize-space(text())"/>
                        </xsl:otherwise>
                    </xsl:choose>
                    <xsl:if test="not(position()=last())">
                        <xsl:value-of select="$delim"/>
                    </xsl:if>
                </xsl:for-each>
            </xsl:when>
            <xsl:when test="x:span[@class]">
                <xsl:apply-templates/>
            </xsl:when>
            <xsl:when test="x:span">
                <xsl:call-template name="lang"/>
                <xsl:value-of select="normalize-space(x:span/text())"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:call-template name="lang"/>
                <xsl:value-of select="normalize-space(text())"/>
            </xsl:otherwise>
        </xsl:choose>
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
