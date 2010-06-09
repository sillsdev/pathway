<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        pxhtml2xpw.xsl
    # Purpose:     Given the processed XMTML dictionary content, prepare ConTeXt content.
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2009/11/23
    # Changes from xhtml2dex.xsl - remove namespace def for x, use stringLength
    # RCS-ID:      $Id: xhtml2dex.xsl $
    # Copyright:   (c) 2009 SIL
    # Licence:     <MIT>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" 
    xmlns:fn="http://www.w3.org/2005/xpath-functions">  
    <xsl:output encoding="UTF-8" method="text" indent="no"/>
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
        <xsl:text>\Bdict
        </xsl:text>
        <xsl:apply-templates/>
        <xsl:text>\Edict
        </xsl:text>
    </xsl:template>
    
    <xsl:template match="div">
        <xsl:choose>
            <xsl:when test="@class='letter'">
                <xsl:text>\Bletter </xsl:text>
                <xsl:value-of select="$verFont"/>
                <xsl:value-of select="text()"/>
                <xsl:text>\Eletter
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="contains('#entry#subentry#', concat('#', @class, '#'))">
                <xsl:for-each select="div[@class='pictureRight']">
                    <xsl:text>\textimage{</xsl:text>
                    <xsl:variable name="srcFile" select="fn:data(img/@src)"/>
                    <xsl:value-of select="fn:substring($srcFile,1,fn:stringLength($srcFile)-4)"/>
                    <xsl:text>}</xsl:text>
                    <xsl:text>{</xsl:text>
                    <xsl:apply-templates select="div/span[@class='pictureLabel']"/>
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
    
    <xsl:template match="span">
        <xsl:choose>
            <xsl:when test="contains('#headword#headword-minor#headword-sub#' , concat('#', @class, '#'))">
                <xsl:text>\Bhw </xsl:text>
                <xsl:call-template name="xitem">
                    <xsl:with-param name="delim">
                        <xsl:text>, </xsl:text>
                    </xsl:with-param>
                </xsl:call-template>
                <xsl:text>\Ehw</xsl:text>
                <xsl:text>\marking[guidewords]{</xsl:text>
                <xsl:call-template name="xitem">
                    <xsl:with-param name="delim">
                        <xsl:text>, </xsl:text>
                    </xsl:with-param>
                </xsl:call-template>
                <xsl:text>}
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="contains('#pronunciation#pronunciation-minor#pronunciation-sub#', concat('#', @class, '#'))">
                <xsl:text>\Bpr </xsl:text>
                <xsl:call-template name="SpanNoLang"/>
                <xsl:text>\Epr
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='grammatical-info'">
                <xsl:text>\Bps </xsl:text>
                <xsl:for-each select="span">
                    <xsl:choose>
                        <xsl:when test="@class='partofspeech'">
                            <xsl:call-template name="SpanNoLang"/>
                        </xsl:when>
                        <xsl:when test="span[@class='slot-name']">
                            <xsl:apply-templates/>
                        </xsl:when>
                    </xsl:choose>
                </xsl:for-each>
                <xsl:text>\Eps
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="@class='slot-name'">
                <xsl:text>: </xsl:text>
                <xsl:call-template name="SpanNoLang"/>
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
            
            <xsl:when test="contains('#definition#definition_L2#definition-sub#LexEntryType-publishStemMinorEntryType-AbbreviationPub#LexEntryType-publishRootMinorEntryType-AbbreviationPub#', concat('#', @class, '#'))">
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
                    <xsl:when test="span">
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
            
            <xsl:when test="contains('#translation#translation_L2#', concat('#', @class, '#'))">
                <xsl:text>\Btr </xsl:text>
                <xsl:call-template name="xitem"/>
                <xsl:text>\Etr
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="contains('#lexref-type#crossref-type#', concat('#', @class, '#'))">
                <xsl:text>\Blr </xsl:text>
                <xsl:call-template name="SpanNoLang"/>
                <xsl:text>:</xsl:text>
                <xsl:text>\Elr </xsl:text>
            </xsl:when>
            
            <xsl:when test="contains('#lexref-targets#crossref#complexform-form#LexEntry-publishRootMinorPrimaryTarget-MLHeadWordPub#', concat('#', @class, '#'))">
                <xsl:text>\Bsx </xsl:text>
                <xsl:call-template name="xitem">
                    <xsl:with-param name="delim">
                        <xsl:text>, </xsl:text>
                    </xsl:with-param>
                </xsl:call-template>
                <xsl:text>\Esx
                </xsl:text>
            </xsl:when>
            
            <xsl:when test="contains('#pictureLabel#', concat('#', @class, '#'))">
                <xsl:call-template name="xitem"/>
            </xsl:when>
            
            <xsl:when test="contains('#sense-crossref#', concat('#', @class, '#'))">
                <xsl:call-template name="lang"/>
                <xsl:value-of select="normalize-space(text())"/>
                <xsl:apply-templates/>
            </xsl:when>
            
            <xsl:otherwise>
                <xsl:apply-templates/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="SpanNoLang">
        <xsl:choose>
            <xsl:when test="span">
                <!-- 6.0.4 -->
                <xsl:value-of select="normalize-space(span/text())"/>
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
            <xsl:when test="span[@class='xitem']">
                <xsl:for-each select="span">
                    <xsl:choose>
                        <xsl:when test="span[@class]">
                            <xsl:call-template name="lang"/>
                            <xsl:value-of select="text()"/>
                            <xsl:apply-templates/>
                        </xsl:when>
                        <xsl:when test="span">
                            <xsl:for-each select="span">
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
            <xsl:when test="normalize-space(node()[1])!=''">
                <xsl:call-template name="lang"/>
                <xsl:value-of select="normalize-space(node()[1])"/>
                <xsl:apply-templates/>
            </xsl:when>
            <xsl:when test="span[@class]">
                <xsl:apply-templates/>
            </xsl:when>
            <xsl:when test="span">
                <xsl:call-template name="lang"/>
                <xsl:value-of select="normalize-space(span/text())"/>
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
