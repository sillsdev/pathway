<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        theWord.xsl
    # Purpose:     process USX into theWord format
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2015/07/21
    # Copyright:   (c) 2013 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0">
    
    <xsl:param name="missing">(-)</xsl:param>
    <xsl:param name="refPunc">.</xsl:param>
    <xsl:param name="refMaterial"></xsl:param>
    <xsl:param name="bridgePunc">-</xsl:param>
    <xsl:param name="sequencePunc">,</xsl:param>
    <xsl:param name="bookSequencePunc">;</xsl:param>
    <xsl:param name="bookNames">BookNames.xml</xsl:param>
    <xsl:param name="glossary">GLO.usx</xsl:param>
    <xsl:param name="versification">vrs.xml</xsl:param>
    <xsl:param name="noStar" select="false()"/>
    <xsl:param name="noSaltillo" select="false()"/>
    <xsl:param name="rtl" select="false()"/>
    <xsl:param name="refPref">
        <xsl:text disable-output-escaping="yes"><![CDATA[tw://bible.*?]]></xsl:text>
    </xsl:param>
    
    <xsl:output  encoding="UTF-8" method="text"/>
    
    <xsl:variable name="code" select="//book/@code"/>
    <xsl:variable name="verseRefs" select="document($versification)//bk"/>
    <xsl:variable name="vrs" select="$verseRefs[@code=$code]"/>
    <xsl:variable name="bookNamesBook" select="document($bookNames)//book"/>
    <xsl:variable name="glossaryKey" select="document($glossary)//para[*/@style='k']"/>
    
    <xsl:template match="/">
        <xsl:choose>
            <xsl:when test="count(//para[@style='toc1'])">
                <xsl:for-each select="//para[@style='toc1']">
                    <xsl:call-template name="FormatHeader"/>
                </xsl:for-each>
            </xsl:when>
            <xsl:otherwise>
                <xsl:for-each select="//para[starts-with(@style, 'mt')]">
                    <xsl:call-template name="FormatHeader"/>
                </xsl:for-each>
            </xsl:otherwise>
        </xsl:choose>
        <xsl:call-template name="nextChapter">
            <xsl:with-param name="cvData" select="$vrs/text()"/>
        </xsl:call-template>
    </xsl:template>

    <xsl:template name="FormatHeader">
        <xsl:choose>
            <xsl:when test="@style='mt2'">
                <xsl:text disable-output-escaping="yes"><![CDATA[<TS2><font color=teal size=-1><b>]]></xsl:text>
                <xsl:value-of select="."/>
                <xsl:text disable-output-escaping="yes"><![CDATA[</b></font><Ts>]]></xsl:text>
            </xsl:when>
            <xsl:otherwise>
                <xsl:text disable-output-escaping="yes"><![CDATA[<TS1><font color=teal>]]></xsl:text>
                <xsl:value-of select="."/>
                <xsl:text disable-output-escaping="yes"><![CDATA[</font><Ts>]]></xsl:text>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="nextChapter">
        <xsl:param name="cvData"/>
        <xsl:choose>
            <xsl:when test="contains($cvData, ' ')">
                <xsl:call-template name="chapter">
                    <xsl:with-param name="data" select="substring-before($cvData, ' ')"/>
                </xsl:call-template>
                <xsl:call-template name="nextChapter">
                    <xsl:with-param name="cvData" select="substring-after($cvData, ' ')"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:call-template name="chapter">
                    <xsl:with-param name="data" select="$cvData"/>
                </xsl:call-template>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="chapter">
        <xsl:param name="data"/>
        <xsl:variable name="thisC" select="substring-before($data, ':')"/>
        <xsl:variable name="cVtotal" select="substring-after($data, ':')"/>
        <xsl:call-template name="vIter">
            <xsl:with-param name="c" select="$thisC"/>
            <xsl:with-param name="v" select="1"/>
            <xsl:with-param name="total" select="$cVtotal"/>
        </xsl:call-template>
    </xsl:template>
    
    <xsl:template name="vIter">
        <xsl:param name="c"/>
        <xsl:param name="v"/>
        <xsl:param name="total"/>
        <xsl:call-template name="VerseLookup">
            <xsl:with-param name="c" select="$c"/>
            <xsl:with-param name="v" select="$v"/>
        </xsl:call-template>
        <xsl:if test="$v &lt; $total">
            <xsl:call-template name="vIter">
                <xsl:with-param name="c" select="$c"/>
                <xsl:with-param name="v" select="$v + 1"/>
                <xsl:with-param name="total" select="$total"/>
            </xsl:call-template>
        </xsl:if>
    </xsl:template>
    
    <xsl:template name="VerseLookup">
        <xsl:param name="c"/>
        <xsl:param name="v"/>
        <xsl:variable name="verse" select="//verse[@number=string($v)][preceding::chapter[1]/@number=$c]"/>
        <!-- Modern translations have 3JN 14 and REV 12:18 but KJV doesn't have it so we combine if necessary. -->
        <xsl:choose>
            <xsl:when test="$code = '3JN' and $v = 14 and count($verse) = 1">
                <xsl:call-template name="CombineVerses">
                    <xsl:with-param name="c" select="$c"/>
                    <xsl:with-param name="v" select="$v"/>
                    <xsl:with-param name="verse" select="$verse"/>
                    <xsl:with-param name="nextVerse" select="string(15)"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="$code = 'REV' and $c = '12' and $v = 17 and count($verse) = 1">
                <xsl:call-template name="CombineVerses">
                    <xsl:with-param name="c" select="$c"/>
                    <xsl:with-param name="v" select="$v"/>
                    <xsl:with-param name="verse" select="$verse"/>
                    <xsl:with-param name="nextVerse" select="string(18)"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:apply-templates select="$verse" mode="v"/>
            </xsl:otherwise>
        </xsl:choose>
        <xsl:if test="count($verse) = 0">
            <xsl:call-template name="VerseBridgeLookup">
                <xsl:with-param name="c" select="$c"/>
                <xsl:with-param name="v" select="$v"/>
                <xsl:with-param name="missing" select="$missing"/>
            </xsl:call-template>
        </xsl:if>
    </xsl:template>
    
    <xsl:template name="VerseBridgeLookup">
        <xsl:param name="c"/>
        <xsl:param name="v"/>
        <xsl:param name="missing"/>
        <!-- verse bridges -->
        <xsl:variable name="bridgeVerse" select="//verse[starts-with(@number,concat(string($v), $bridgePunc))][preceding::chapter[1]/@number=$c]"/>
        <xsl:apply-templates select="$bridgeVerse" mode="v">
            <xsl:with-param name="bridge" select="$bridgeVerse/@number"/>
        </xsl:apply-templates>
        <xsl:variable name="seqVerse" select="//verse[starts-with(@number,concat(string($v), $sequencePunc))][preceding::chapter[1]/@number=$c]"/>
        <xsl:apply-templates select="$seqVerse" mode="v">
            <xsl:with-param name="bridge" select="$seqVerse/@number"/>
        </xsl:apply-templates>
        <xsl:variable name="partVerse" select="//verse[starts-with(@number,concat(string($v), 'a'))][preceding::chapter[1]/@number=$c]"/>
        <xsl:if test="count($partVerse) != 0">
            <xsl:call-template name="CombineVerses">
                <xsl:with-param name="c" select="$c"/>
                <xsl:with-param name="v" select="concat($v, 'a')"/>
                <xsl:with-param name="verse" select="$partVerse"/>
                <xsl:with-param name="nextVerse" select="concat($v, 'b')"/>
            </xsl:call-template>
        </xsl:if>
        <xsl:if test="count($bridgeVerse) + count($seqVerse) + count($partVerse) = 0">
            <!-- missing verses -->
            <xsl:value-of select="$missing"/>
            <xsl:variable name="verse" select="//verse[@number=string($v+1)][preceding::chapter[1]/@number=$c]"/>
            <xsl:if test="count($verse) != 0 and count($verse/preceding-sibling::*) = 0">
                <xsl:variable name="parentStyle" select="$verse/parent::*[1]/@style"/>
                <xsl:choose>
                    <xsl:when test="starts-with($parentStyle, 'q')">
                        <xsl:text disable-output-escaping="yes"><![CDATA[<CI>]]></xsl:text>
                    </xsl:when>
                    <xsl:when test="starts-with($parentStyle, 'p')">
                        <xsl:text disable-output-escaping="yes"><![CDATA[<CM>]]></xsl:text>
                    </xsl:when>
                </xsl:choose>
            </xsl:if>
            <xsl:text>&#13;&#10;</xsl:text>
        </xsl:if>
    </xsl:template>

    <xsl:template name="CombineVerses">
        <xsl:param name="c"/>
        <xsl:param name="v"/>
        <xsl:param name="verse"/>
        <xsl:param name="nextVerse"/>
        <xsl:variable name="nextVerseNode" select="//verse[@number=$nextVerse][preceding::chapter[1]/@number=$c]"/>
        <xsl:choose>
            <xsl:when test="count($nextVerseNode) = 1">
                <xsl:text disable-output-escaping="yes"><![CDATA[<sup>(]]></xsl:text>
                <xsl:if test="$rtl">
                    <xsl:text disable-output-escaping="yes"><![CDATA[<rtl>]]></xsl:text>
                </xsl:if>
                <xsl:value-of select="$v"/>
                <xsl:if test="$rtl">
                    <xsl:text disable-output-escaping="yes"><![CDATA[</rtl>]]></xsl:text>
                </xsl:if>
                <xsl:value-of select="$bridgePunc"/>
                <xsl:value-of select="$nextVerse"/>
                <xsl:text disable-output-escaping="yes"><![CDATA[)</sup> ]]></xsl:text>
                <xsl:apply-templates select="$verse" mode="v">
                    <xsl:with-param name="combine" select="1"/>
                </xsl:apply-templates>
                <xsl:text disable-output-escaping="yes"><![CDATA[ <sup>]]></xsl:text>
                <xsl:value-of select="$nextVerse"/>
                <xsl:text disable-output-escaping="yes"><![CDATA[</sup> ]]></xsl:text>
                <xsl:apply-templates select="$nextVerseNode" mode="v"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:apply-templates select="$verse" mode="v"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template match="*" mode="v">
        <xsl:param name="combine" select="0"/>
        <xsl:param name="bridge"/>
        <xsl:apply-templates select="preceding::*[1]" mode="s"/>
        <xsl:choose>
            <!-- Not first verse in Paragraph -->
            <xsl:when test="count(preceding-sibling::verse)">
                <xsl:call-template name="BridgeVerseNumbers">
                    <xsl:with-param name="bridge" select="$bridge"/>
                </xsl:call-template>
                <xsl:apply-templates select="following::node()[1]" mode="t"/>
            </xsl:when>
            <!-- It is the first verse in the Paragraph -->
            <xsl:otherwise>
                <xsl:call-template name="FirstVerseInParagraph">
                    <xsl:with-param name="bridge" select="$bridge"/>
                </xsl:call-template>
            </xsl:otherwise>
        </xsl:choose>
        <!-- book ends with paragraph end -->
        <xsl:if test="count(following::*) = 0">
            <xsl:text disable-output-escaping="yes"><![CDATA[<CM>]]></xsl:text>
        </xsl:if>
        <xsl:if test="$combine = 0">
            <xsl:text>&#13;&#10;</xsl:text>
        </xsl:if>
    </xsl:template>
    
    <!-- Recurse backwards to previous verse (or start of chapter), process elements as the come off the stack -->
    <xsl:template match="*" mode="s">
        <xsl:choose>
            <xsl:when test="local-name() = 'verse' or count(child::verse) != 0"/>
            <xsl:when test="local-name() = 'chapter'"/>
            <xsl:otherwise>
                <xsl:apply-templates select="preceding::*[1]/ancestor-or-self::para" mode="s"/>
                <xsl:choose>
                    <xsl:when test="@style = 's' or @style = 's1' or @style = 's2' or starts-with(@style,'ms')">
                        <xsl:call-template name="SectionHead"/>
                    </xsl:when>
                    <xsl:when test="@style = 'r'">
                        <xsl:call-template name="ParallelReference"/>
                    </xsl:when>
                    <xsl:when test="@style = 'mr' or @style = 'd' or @style = 'qa'">
                        <xsl:call-template name="OtherReference"/>
                    </xsl:when>
                </xsl:choose>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <xsl:template name="SectionHead">
        <xsl:text disable-output-escaping="yes"><![CDATA[<TS1>]]></xsl:text>
        <xsl:if test="@style = 's2'">
            <xsl:text disable-output-escaping="yes"><![CDATA[<font size=-1>]]></xsl:text>
        </xsl:if>
        <xsl:for-each select="child::node()">
            <xsl:choose>
                <xsl:when test="@style='f'">
                    <xsl:text disable-output-escaping="yes"><![CDATA[<RF q=]]></xsl:text>
                    <xsl:value-of select="@caller"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[>]]></xsl:text>
                    <xsl:value-of select="normalize-space(*[@style = 'ft'])"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[<Rf>]]></xsl:text>
                </xsl:when>
                <xsl:when test="@style = 'it'">
                    <xsl:if test="normalize-space(preceding-sibling::node()[1]) != ''">
                        <xsl:text> </xsl:text>
                    </xsl:if>
                    <xsl:text disable-output-escaping="yes"><![CDATA[<i>]]></xsl:text>
                    <xsl:value-of select="normalize-space(.)"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[</i>]]></xsl:text>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:if test="normalize-space(preceding-sibling::node()[1]) != ''">
                        <xsl:text> </xsl:text>
                    </xsl:if>
                    <xsl:value-of select="normalize-space(.)"/>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:for-each>
        <xsl:if test="@style = 's2'">
            <xsl:text disable-output-escaping="yes"><![CDATA[</font>]]></xsl:text>
        </xsl:if>
        <xsl:text disable-output-escaping="yes"><![CDATA[<Ts>]]></xsl:text>
    </xsl:template>
    
    <xsl:template name="ParallelReference">
        <xsl:text disable-output-escaping="yes"><![CDATA[<TS3><i>]]></xsl:text>
        <xsl:variable name="normText" select="normalize-space(text())"/>
        <xsl:choose>
            <xsl:when test="starts-with($normText, '(')">
                <xsl:text>(</xsl:text>
                <xsl:call-template name="RemoveRefMaterial">
                    <xsl:with-param name="text" select="substring-before(substring-after($normText, '('), ')')"/>
                </xsl:call-template>
                <xsl:text>)</xsl:text>
            </xsl:when>
            <xsl:otherwise>
                <xsl:call-template name="RemoveRefMaterial">
                    <xsl:with-param name="text" select="$normText"/>
                </xsl:call-template>
            </xsl:otherwise>
        </xsl:choose>
        <xsl:text disable-output-escaping="yes"><![CDATA[</i><Ts>]]></xsl:text>
    </xsl:template>
    
    <xsl:template name="OtherReference">
        <xsl:text disable-output-escaping="yes"><![CDATA[<TS3><i>]]></xsl:text>
        <xsl:value-of select="normalize-space(text())"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[</i><Ts>]]></xsl:text>
    </xsl:template>
    
    <xsl:template name="FirstVerseInParagraph">
        <xsl:param name="bridge"/>
        <xsl:choose>
            <!-- verse at q1 indent -->
            <xsl:when
                test="parent::node()/@style = 'q' or parent::node()/@style = 'q1' or parent::node()/@style = 'li' or parent::node()/@style = 'li1' or parent::node()/@style = 'qm' or parent::node()/@style = 'qm1'">
                <xsl:text disable-output-escaping="yes"><![CDATA[<PI>]]></xsl:text>
                <xsl:call-template name="BridgeVerseNumbers">
                    <xsl:with-param name="bridge" select="$bridge"/>
                </xsl:call-template>
                <xsl:apply-templates select="following::node()[1]" mode="t">
                    <xsl:with-param name="indent" select="3"/>
                    <xsl:with-param name="bullet" select="contains(parent::node()/@style, 'li')"/>
                </xsl:apply-templates>
            </xsl:when>
            <!-- verse at q2 indent -->
            <xsl:when
                test="starts-with(parent::node()/@style, 'q') or starts-with(parent::node()/@style, 'li')">
                <xsl:text disable-output-escaping="yes"><![CDATA[<PI2>]]></xsl:text>
                <xsl:call-template name="BridgeVerseNumbers">
                    <xsl:with-param name="bridge" select="$bridge"/>
                </xsl:call-template>
                <xsl:apply-templates select="following::node()[1]" mode="t">
                    <xsl:with-param name="indent" select="4"/>
                    <xsl:with-param name="bullet" select="contains(parent::node()/@style, 'li')"/>
                </xsl:apply-templates>
            </xsl:when>
            <!-- normal verse -->
            <xsl:otherwise>
                <xsl:call-template name="BridgeVerseNumbers">
                    <xsl:with-param name="bridge" select="$bridge"/>
                </xsl:call-template>
                <xsl:apply-templates select="following::node()[1]" mode="t"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="BridgeVerseNumbers">
        <xsl:param name="bridge"/>
        <xsl:if test="$bridge != ''">
            <xsl:text disable-output-escaping="yes"><![CDATA[<sup>(]]></xsl:text>
            <xsl:choose>
                <xsl:when test="$rtl">
                    <xsl:text disable-output-escaping="yes"><![CDATA[<rtl>]]></xsl:text>
                    <xsl:value-of select="substring-before($bridge, $bridgePunc)"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[</rtl>]]></xsl:text>
                    <xsl:value-of select="$bridgePunc"/>
                    <xsl:value-of select="substring-after($bridge, $bridgePunc)"/>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select="$bridge"/>
                </xsl:otherwise>
            </xsl:choose>
            <xsl:text disable-output-escaping="yes"><![CDATA[)</sup> ]]></xsl:text>
        </xsl:if>
    </xsl:template>
    
    <!-- This mode handles the text nodes. -->
    <xsl:template match="node()" mode="t">
        <xsl:param name="space" select="0"/>
        <xsl:param name="indent" select="0"/>
        <xsl:param name="bullet" select="false()"/>
        <xsl:param name="embedded" select="false()"/>
        <xsl:choose>
            <xsl:when test="local-name() = '' and normalize-space() != ''">
                <xsl:call-template name="NormalText">
                    <xsl:with-param name="indent" select="$indent"/>
                    <xsl:with-param name="space" select="$space"/>
                    <xsl:with-param name="bullet" select="$bullet"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="@style = 'it' or @style = 'em' or @style='tl' or @style = 'dc' or @style = 'sls'">
                <xsl:call-template name="Italic-char">
                    <xsl:with-param name="space" select="$space"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="@style = 'bd' or @style = 'bk' or @style = 'add' or @style = 'pn'">
                <xsl:call-template name="Bold-char">
                    <xsl:with-param name="space" select="$space"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="@style = 'bdit'">
                <xsl:call-template name="BoldItalic-char">
                    <xsl:with-param name="space" select="$space"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="@style = 'sc'">
                <xsl:call-template name="SmallCap-char">
                    <xsl:with-param name="space" select="$space"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="@style = 'sup' or @style = 'ord'">
                <xsl:call-template name="Sup-char">
                    <xsl:with-param name="space" select="$space"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="@style = 'nd'">
                <xsl:call-template name="NameOfDiety">
                    <xsl:with-param name="space" select="$space"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="@style = 'w'">
                <xsl:call-template name="GlossaryWord">
                    <xsl:with-param name="space" select="$space"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="@style = 'sp'">
                <xsl:call-template name="Speaker"/>
            </xsl:when>
            <xsl:when test="@style = 'wj'">
                <xsl:call-template name="WordsOfJesus">
                    <xsl:with-param name="space" select="$space"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="@style = 'm' or @style = 'b' or @style = 'tr'">
                <xsl:if test="not(contains(preceding-sibling::verse[1]/@number | preceding::verse[1]/@number, $bridgePunc))">
                    <xsl:text disable-output-escaping="yes"><![CDATA[<CL>]]></xsl:text>
                </xsl:if>
                <xsl:apply-templates select="(child::node() | following::node())[1]" mode="t"/>
            </xsl:when>
            <xsl:when test="@style = 'no' or @style = 'qt'">
                <xsl:call-template name="Normal-char">
                    <xsl:with-param name="space" select="$space"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="starts-with(@style, 'p') or @style = 'sig'">
                <xsl:if test="not(contains(preceding-sibling::verse[1]/@number | preceding::verse[1]/@number, $bridgePunc))">
                    <xsl:text disable-output-escaping="yes"><![CDATA[<CM>]]></xsl:text>
                </xsl:if>
                <xsl:apply-templates select="(child::node() | following::node())[1]" mode="t"/>
            </xsl:when>
            <xsl:when test="@style = 'q' or @style = 'q1' or @style = 'li' or @style = 'li1' or @style = 'qm' or @style = 'qm1'">
                <xsl:call-template name="Quote-1">
                    <xsl:with-param name="indent" select="$indent"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="@style = 'q2' or @style = 'q3' or @style = 'q4' or starts-with(@style, 'li')">
                <xsl:call-template name="Quote-2">
                    <xsl:with-param name="indent" select="$indent"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="@style = 'f' or @style = 'ef'">
                <xsl:call-template name="Footnotes">
                    <xsl:with-param name="space" select="$space"/>
                    <xsl:with-param name="indent" select="$indent"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="@style = 'x'">
                <xsl:call-template name="CrossReferences">
                    <xsl:with-param name="caller" select="@caller"/>
                    <xsl:with-param name="text" select="*[@style='xt']/text()"/>
                    <xsl:with-param name="space" select="$space"/>
                    <xsl:with-param name="indent" select="$indent"/>
                </xsl:call-template>
            </xsl:when>
            <!-- stop at verse or chapter -->
            <xsl:when test="local-name() = 'verse'"/>
            <xsl:when test="local-name() = 'chapter'">
                <xsl:text disable-output-escaping="yes"><![CDATA[<CM>]]></xsl:text>
            </xsl:when>
            <!-- include contents -->                        
            <xsl:when test="local-name() = 'table'">
                <xsl:apply-templates select="(child::node() | following::node())[1]" mode="t"/>
            </xsl:when>
            <xsl:when test="starts-with(@style, 'tc')">
                <xsl:if test="preceding-sibling::*[contains(@style, 'tc')]">
                    <xsl:text> &#x2014; </xsl:text> <!-- em-dash -->
                </xsl:if>
                <xsl:apply-templates select="(child::node() | following::node())[1]" mode="t"/>
            </xsl:when>
            <xsl:when test="local-name() = 'optbreak'">
                <xsl:text> </xsl:text>
                <xsl:apply-templates select="(child::node() | following::node())[1]" mode="t"/>
            </xsl:when>
            <!-- skip all other content -->                        
            <xsl:otherwise>
                <xsl:apply-templates select="following::node()[1]" mode="t">
                    <xsl:with-param name="space" select="$space"/>
                    <xsl:with-param name="indent" select="$indent"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:apply-templates>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="NormalText">
        <xsl:param name="indent"/> <!-- 1=q (or q1), 2=q2 (or q3-4), 3=continue q, 4=continue q2 -->
        <xsl:param name="space"/>
        <xsl:param name="bullet"/>
        <xsl:param name="embedded" select="false()"/>
        <xsl:if test="$indent = 1">
            <xsl:text disable-output-escaping="yes"><![CDATA[<PI>]]></xsl:text>
        </xsl:if>
        <xsl:if test="$indent = 2">
            <xsl:text disable-output-escaping="yes"><![CDATA[<PI2>]]></xsl:text>
        </xsl:if>
        <xsl:if test="$bullet">
            <xsl:text>&#x2022; </xsl:text> <!-- bullet -->
        </xsl:if>
        <xsl:if test="$space = 1">
            <!-- check if text begins with a space -->
            <xsl:if test="contains(., ' ') and string-length(substring-before(., ' ')) = 0">
                <xsl:text> </xsl:text>
            </xsl:if>
        </xsl:if>
        <xsl:call-template name="OutputText"/>
        <xsl:variable name="precChar" select="substring(., string-length(.))"></xsl:variable>
        <xsl:choose>
            <xsl:when test="$embedded">
                <xsl:apply-templates select="(child::node() | following-sibling::node())[1]" mode="t">
                    <xsl:with-param name="space" select="$precChar != '&#x2018;' and $precChar != '&#x201c;'"/>
                    <xsl:with-param name="indent" select="$indent"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:apply-templates>
            </xsl:when>
            <xsl:otherwise>
                <xsl:apply-templates select="(child::node() | following::node())[1]" mode="t">
                    <xsl:with-param name="space" select="$precChar != '&#x2018;' and $precChar != '&#x201c;'"/>
                    <xsl:with-param name="indent" select="$indent"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:apply-templates>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="EmbeddedText">
        <xsl:param name="embedded"/>
        <xsl:choose>
            <xsl:when test="count(child::node()) &lt; 2">
                <xsl:call-template name="OutputText"/>
            </xsl:when>
            <xsl:when test="child::node()[1]/@style">
                <xsl:apply-templates select="child::node()[1]" mode="t">
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:apply-templates>
            </xsl:when>
            <xsl:otherwise>
                <xsl:for-each select="child::node()[1]">
                    <xsl:call-template name="OutputText"/>
                </xsl:for-each>
                <xsl:variable name="precChar" select="substring(child::node()[1], string-length(child::node()[1]))"/>
                <xsl:apply-templates select="child::node()[2]" mode="t">
                    <xsl:with-param name="space" select="$precChar != '&#x2018;' and $precChar != '&#x201c;'"/>
                    <xsl:with-param name="embedded" select="true()"/>
                </xsl:apply-templates>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="OutputText">
        <xsl:choose>
            <xsl:when test="$noStar = false() and $noSaltillo = false()">
                <xsl:value-of select="normalize-space(.)"/>
            </xsl:when>
            <xsl:when test="$noStar = false() and $noSaltillo != false()">
                <xsl:variable name="apos">&apos;</xsl:variable>
                <xsl:value-of select="normalize-space(translate(., '&#xa78c;', $apos))"/>
            </xsl:when>
            <xsl:when test="$noStar != false() and $noSaltillo = false()">
                <xsl:value-of select="normalize-space(translate(., '*', ''))"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:variable name="apos">&apos;</xsl:variable>
                <xsl:value-of select="normalize-space(translate(., '&#xa78c;*', $apos))"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <xsl:template name="Normal-char">
        <xsl:param name="space"/>
        <xsl:param name="embedded"/>
        <xsl:if test="$space = 1">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:call-template name="OutputText"/>
        <xsl:apply-templates select="following::node()[1]" mode="t">
            <xsl:with-param name="space" select="1"/>
            <xsl:with-param name="embedded" select="$embedded"/>
        </xsl:apply-templates>
    </xsl:template>
    
    <xsl:template name="Italic-char">
        <xsl:param name="space"/>
        <xsl:param name="embedded"/>
        <xsl:if test="$space = 1">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:text disable-output-escaping="yes"><![CDATA[<i>]]></xsl:text>
        <xsl:call-template name="EmbeddedText"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[</i>]]></xsl:text>
        <xsl:apply-templates select="following::node()[1]" mode="t">
            <xsl:with-param name="space" select="1"/>
            <xsl:with-param name="embedded" select="$embedded"/>
        </xsl:apply-templates>
    </xsl:template>
    
    <xsl:template name="NameOfDiety">
        <xsl:param name="space"/>
        <xsl:param name="embedded"/>
        <xsl:if test="$space = 1">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:text disable-output-escaping="yes"><![CDATA[<font color=green>]]></xsl:text>
        <xsl:call-template name="OutputText"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[</font>]]></xsl:text>
        <xsl:apply-templates select="following::node()[1]" mode="t">
            <xsl:with-param name="space" select="1"/>
            <xsl:with-param name="embedded" select="$embedded"/>
        </xsl:apply-templates>
    </xsl:template>
    
    <xsl:template name="WordsOfJesus">
        <xsl:param name="space"/>
        <xsl:param name="embedded"/>
        <xsl:if test="$space = 1">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:text disable-output-escaping="yes"><![CDATA[<font color=red>]]></xsl:text>
        <xsl:call-template name="EmbeddedText"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[</font>]]></xsl:text>
        <xsl:apply-templates select="following::node()[1]" mode="t">
            <xsl:with-param name="space" select="1"/>
            <xsl:with-param name="embedded" select="$embedded"/>
        </xsl:apply-templates>
    </xsl:template>
    
    <xsl:template name="GlossaryWord">
        <xsl:param name="space"/>
        <xsl:param name="embedded"/>
        <xsl:if test="$space = 1">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:variable name="glossaryLookup" select="./text()"/>
        <xsl:choose>
            <xsl:when test="contains($glossaryLookup, '|')">
                <xsl:call-template name="GlossaryFormatter">
                    <xsl:with-param name="surfaceForm" select="substring-before($glossaryLookup, '|')"/>
                    <xsl:with-param name="glossaryLookup" select="substring-after($glossaryLookup,'|')"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:call-template name="GlossaryFormatter">
                    <xsl:with-param name="surfaceForm" select="$glossaryLookup"/>
                    <xsl:with-param name="glossaryLookup" select="$glossaryLookup"/>
                </xsl:call-template>
            </xsl:otherwise>
        </xsl:choose>
        <xsl:apply-templates select="following::node()[1]" mode="t">
            <xsl:with-param name="space" select="1"/>
            <xsl:with-param name="embedded" select="$embedded"/>
        </xsl:apply-templates>
    </xsl:template>

    <xsl:template name="GlossaryFormatter">
        <xsl:param name="surfaceForm"/>
        <xsl:param name="glossaryLookup"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[<font color=blue>]]></xsl:text>
        <xsl:value-of select="$surfaceForm"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[</font>]]></xsl:text>
        <xsl:variable name="glossaryEntry" select="$glossaryKey[*[@style='k'] = $glossaryLookup]"/>
        <xsl:if test="count($glossaryEntry) != 0">
            <xsl:text disable-output-escaping="yes"><![CDATA[<RF q=*>]]></xsl:text>
            <xsl:for-each select="$glossaryEntry/node()[position() &gt; 2]">
                <xsl:choose>
                    <xsl:when test="@style = 'tl'">
                        <xsl:text disable-output-escaping="yes"><![CDATA[<i>]]></xsl:text>
                        <xsl:call-template name="OutputText"/>
                        <xsl:text disable-output-escaping="yes"><![CDATA[</i>]]></xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:call-template name="OutputText"/>
                    </xsl:otherwise>
                </xsl:choose>
                <xsl:text> </xsl:text>
            </xsl:for-each>
            <xsl:text disable-output-escaping="yes"><![CDATA[<Rf>]]></xsl:text>
        </xsl:if>
    </xsl:template>
    
    <xsl:template name="SmallCap-char">
        <xsl:param name="space"/>
        <xsl:param name="embedded"/>
        <xsl:if test="$space = 1">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:text disable-output-escaping="yes"><![CDATA[<font size=-1>]]></xsl:text>
        <xsl:call-template name="EmbeddedText"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[</font>]]></xsl:text>
        <xsl:apply-templates select="following::node()[1]" mode="t">
            <xsl:with-param name="space" select="1"/>
            <xsl:with-param name="embedded" select="$embedded"/>
        </xsl:apply-templates>
    </xsl:template>
    
    <xsl:template name="Bold-char">
        <xsl:param name="space"/>
        <xsl:param name="embedded"/>
        <xsl:if test="$space = 1">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:text disable-output-escaping="yes"><![CDATA[<b>]]></xsl:text>
        <xsl:call-template name="EmbeddedText"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[</b>]]></xsl:text>
        <xsl:apply-templates select="following::node()[1]" mode="t">
            <xsl:with-param name="space" select="1"/>
            <xsl:with-param name="embedded" select="$embedded"/>
        </xsl:apply-templates>
    </xsl:template>
    
    <xsl:template name="BoldItalic-char">
        <xsl:param name="space"/>
        <xsl:param name="embedded"/>
        <xsl:if test="$space = 1">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:text disable-output-escaping="yes"><![CDATA[<b><i>]]></xsl:text>
        <xsl:call-template name="EmbeddedText"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[</i></b>]]></xsl:text>
        <xsl:apply-templates select="following::node()[1]" mode="t">
            <xsl:with-param name="space" select="1"/>
            <xsl:with-param name="embedded" select="$embedded"/>
        </xsl:apply-templates>
    </xsl:template>
    
    <xsl:template name="Sup-char">
        <xsl:param name="space"/>
        <xsl:param name="embedded"/>
        <xsl:if test="$space = 1">
            <xsl:text> </xsl:text>
        </xsl:if>
        <xsl:text disable-output-escaping="yes"><![CDATA[<sup>]]></xsl:text>
        <xsl:call-template name="OutputText"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[</sup>]]></xsl:text>
        <xsl:apply-templates select="following::node()[1]" mode="t">
            <xsl:with-param name="space" select="1"/>
            <xsl:with-param name="embedded" select="$embedded"/>
        </xsl:apply-templates>
    </xsl:template>
    
    <xsl:template name="Quote-1">
        <xsl:param name="indent"/>
        <xsl:param name="embedded"/>
        <xsl:if test="not(contains(preceding-sibling::verse[1]/@number | preceding::verse[1]/@number, $bridgePunc))">
            <xsl:text disable-output-escaping="yes"><![CDATA[<CI>]]></xsl:text>
        </xsl:if>
        <xsl:choose>
            <xsl:when test="$indent = 1 or $indent = 3">
                <xsl:apply-templates select="(child::node() | following::node())[1]" mode="t">
                    <xsl:with-param name="indent" select="3"/>
                    <xsl:with-param name="bullet" select="contains(@style, 'li')"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:apply-templates>
            </xsl:when>
            <xsl:otherwise>
                <xsl:apply-templates select="(child::node() | following::node())[1]" mode="t">
                    <xsl:with-param name="indent" select="1"/>
                    <xsl:with-param name="bullet" select="contains(@style, 'li')"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:apply-templates>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="Quote-2">
        <xsl:param name="indent"/>
        <xsl:param name="embedded"/>
        <xsl:if test="not(contains(preceding-sibling::verse[1]/@number | preceding::verse[1]/@number, $bridgePunc))">
            <xsl:text disable-output-escaping="yes"><![CDATA[<CI>]]></xsl:text>
        </xsl:if>
        <xsl:choose>
            <xsl:when test="$indent = 2 or $indent = 4">
                <xsl:apply-templates select="(child::node() | following::node())[1]" mode="t">
                    <xsl:with-param name="indent" select="4"/>
                    <xsl:with-param name="bullet" select="contains(@style, 'li')"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:apply-templates>
            </xsl:when>
            <xsl:otherwise>
                <xsl:apply-templates select="(child::node() | following::node())[1]" mode="t">
                    <xsl:with-param name="indent" select="2"/>
                    <xsl:with-param name="bullet" select="contains(@style, 'li')"/>
                    <xsl:with-param name="embedded" select="$embedded"/>
                </xsl:apply-templates>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="Speaker">
        <xsl:text disable-output-escaping="yes"><![CDATA[<CM><PI0>]]></xsl:text>
        <xsl:apply-templates select="(child::node() | following::node())[1]" mode="t"/>
    </xsl:template>
    
    <xsl:template name="Footnotes">
        <xsl:param name="space"/>
        <xsl:param name="indent"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[<RF q=]]></xsl:text>
        <xsl:value-of select="@caller"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[>]]></xsl:text>
        <xsl:for-each select="child::node()">
            <xsl:choose>
                <xsl:when test="@style = 'fr'"/>
                <xsl:when test="@style = 'it' or @style = 'fq'">
                    <xsl:if test="normalize-space(preceding-sibling::*[@style !='fr']/text()) != ''">
                        <xsl:text> </xsl:text>
                    </xsl:if>
                    <xsl:text disable-output-escaping="yes"><![CDATA[<i>]]></xsl:text>
                    <xsl:call-template name="EmbeddedText"/>
                    <xsl:text disable-output-escaping="yes"><![CDATA[</i>]]></xsl:text>
                    <xsl:if test="starts-with(following-sibling::node()[1], ' ') or substring(., string-length(.)) = ' '">
                        <xsl:text> </xsl:text>
                    </xsl:if>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:call-template name="EmbeddedText">
                        <xsl:with-param name="embedded" select="true()"/>
                    </xsl:call-template>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:for-each>
        <xsl:text disable-output-escaping="yes"><![CDATA[<Rf>]]></xsl:text>
        <xsl:apply-templates select="following::node()[1]" mode="t">
            <xsl:with-param name="space" select="$space"/>
            <xsl:with-param name="indent" select="$indent"/>
        </xsl:apply-templates>
    </xsl:template>
    
    <xsl:template name="CrossReferences">
        <xsl:param name="caller">*</xsl:param>
        <xsl:param name="text"/>
        <xsl:param name="space"/>
        <xsl:param name="indent"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[<RF q=]]></xsl:text>
        <xsl:value-of select="@caller"/>
        <xsl:text disable-output-escaping="yes"><![CDATA[>]]></xsl:text>
        <xsl:call-template name="RemoveRefMaterial">
            <xsl:with-param name="text" select="$text"/>
        </xsl:call-template>
        <xsl:text disable-output-escaping="yes"><![CDATA[<Rf>]]></xsl:text>
        <xsl:apply-templates select="following::node()[1]" mode="t">
            <xsl:with-param name="space" select="$space"/>
            <xsl:with-param name="indent" select="$indent"/>
        </xsl:apply-templates>
    </xsl:template>

    <xsl:template name="RemoveRefMaterial">
        <xsl:param name="text"/>
        <xsl:choose>
            <xsl:when test="starts-with($text, $refMaterial)">
                <xsl:value-of select="$refMaterial"/>
                <xsl:call-template name="CrossReferenceIter">
                    <xsl:with-param name="textLeft"
                        select="normalize-space(substring-after($text,$refMaterial))"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:call-template name="CrossReferenceIter">
                    <xsl:with-param name="textLeft" select="normalize-space($text)"/>
                </xsl:call-template>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="CrossReferenceIter">
        <xsl:param name="textLeft"/>
        <xsl:param name="book" select="./preceding::book"/>
        <xsl:choose>
            <xsl:when test="contains($textLeft, $bookSequencePunc)">
                <xsl:call-template name="CrossRefVerseListIter">
                    <xsl:with-param name="ref" select="substring-before($textLeft, $bookSequencePunc)"/>
                    <xsl:with-param name="book" select="$book"/>
                    <xsl:with-param name="remains" select="normalize-space(substring-after($textLeft, $bookSequencePunc))"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:call-template name="CrossRefVerseListIter">
                    <xsl:with-param name="ref" select="$textLeft"/>
                    <xsl:with-param name="book" select="$book"/>
                </xsl:call-template>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="CrossRefVerseListIter">
        <xsl:param name="ref"/>
        <xsl:param name="book"/>
        <xsl:param name="chap"/>
        <xsl:param name="remains" />
        <xsl:choose>
            <xsl:when test="contains($ref, $sequencePunc)">
                <xsl:call-template name="ReferenceFindBook">
                    <xsl:with-param name="ref" select="substring-before($ref, $sequencePunc)"/>
                    <xsl:with-param name="book" select="$book"/>
                    <xsl:with-param name="chap" select="$chap"/>
                    <xsl:with-param name="verseListLeft" select="normalize-space(substring-after($ref, $sequencePunc))"/>
                    <xsl:with-param name="refsLeft" select="$remains"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:call-template name="ReferenceFindBook">
                    <xsl:with-param name="ref" select="$ref"/>
                    <xsl:with-param name="book" select="$book"/>
                    <xsl:with-param name="chap" select="$chap"/>
                    <xsl:with-param name="refsLeft" select="$remains"/>
                </xsl:call-template>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="ReferenceFindBook">
        <xsl:param name="ref"/>
        <xsl:param name="book"/>
        <xsl:param name="chap"/>
        <xsl:param name="verseListLeft" />
        <xsl:param name="refsLeft"/>
        <xsl:param name="bookSoFar"/>
        <xsl:variable name="refAbbr" select="concat($bookSoFar, substring-before(substring-after($ref, $bookSoFar), ' '))"/>
        <xsl:variable name="refBook" select="$bookNamesBook[@short=$refAbbr] | $bookNamesBook[@abbr=$refAbbr]"/>
        <xsl:choose>
            <!-- No book abbr, continue in the same book -->
            <xsl:when test="not(contains($ref, ' '))">
                <xsl:call-template name="ReferenceFindChapter">
                    <xsl:with-param name="ref" select="$ref"/>
                    <xsl:with-param name="refCV" select="$ref"/>
                    <xsl:with-param name="refBook" select="$book"/>
                    <xsl:with-param name="chap" select="$chap"/>
                    <xsl:with-param name="verseListLeft" select="$verseListLeft"/>
                </xsl:call-template>
                <xsl:if test="$refsLeft != ''">
                    <xsl:text>; </xsl:text>
                    <xsl:call-template name="CrossReferenceIter">
                        <xsl:with-param name="textLeft" select="$refsLeft"/>
                        <xsl:with-param name="book" select="$book"/>
                    </xsl:call-template>
                </xsl:if>
            </xsl:when>
            <!-- New book abbrevation given -->    
            <xsl:when test="count($refBook) = 1">
                <xsl:call-template name="ReferenceFindChapter">
                    <xsl:with-param name="ref" select="$ref"/>
                    <xsl:with-param name="refCV" select="substring($ref, string-length($refAbbr) + 2)"/>
                    <xsl:with-param name="refBook" select="$refBook"/>
                    <xsl:with-param name="chap" select="$chap"/>
                    <xsl:with-param name="verseListLeft" select="$verseListLeft"/>
                </xsl:call-template>
                <xsl:if test="$refsLeft != ''">
                    <xsl:text>; </xsl:text>
                    <xsl:call-template name="CrossReferenceIter">
                        <xsl:with-param name="textLeft" select="$refsLeft"/>
                        <xsl:with-param name="book" select="$refBook"/>
                    </xsl:call-template>
                </xsl:if>
            </xsl:when>
        	<xsl:when test="contains($refMaterial, $refAbbr)">
        		<xsl:value-of select="$refAbbr"/>
        		<xsl:text> </xsl:text>
        		<xsl:call-template name="ReferenceFindBook">
        			<xsl:with-param name="ref" select="normalize-space(substring-after($ref, $refAbbr))"/>
        			<xsl:with-param name="book" select="$book"/>
        			<xsl:with-param name="chap" select="$chap"/>
        			<xsl:with-param name="verseListLeft" select="$verseListLeft"/>
        			<xsl:with-param name="refsLeft" select="$refsLeft"/>
        			<xsl:with-param name="bookSoFar" select="$bookSoFar"/>
        		</xsl:call-template>
        	</xsl:when>
        	<!-- book name is a digit followed by a space and then the rest of book abbr -->
        	<xsl:when test="contains(substring-after($ref, concat($refAbbr, ' ')), ' ')">
        		<xsl:call-template name="ReferenceFindBook">
        			<xsl:with-param name="book" select="$book"/>
        			<xsl:with-param name="chap" select="$chap"/>
        			<xsl:with-param name="ref" select="$ref"/>
        			<xsl:with-param name="refsLeft" select="$refsLeft"/>
        			<xsl:with-param name="verseListLeft" select="$verseListLeft"/>
        			<xsl:with-param name="bookSoFar" select="concat($refAbbr, ' ')"/>
        		</xsl:call-template>
        	</xsl:when>
        	<xsl:when test="normalize-space($bookSoFar) != '' and contains($refMaterial, $bookSoFar)">
        		<xsl:value-of select="$bookSoFar"/>
        		<xsl:text> </xsl:text>
        		<xsl:call-template name="ReferenceFindBook">
        			<xsl:with-param name="ref" select="normalize-space(substring-after($ref, $bookSoFar))"/>
        			<xsl:with-param name="book" select="$book"/>
        			<xsl:with-param name="chap" select="$chap"/>
        			<xsl:with-param name="verseListLeft" select="$verseListLeft"/>
        			<xsl:with-param name="refsLeft" select="$refsLeft"/>
        		</xsl:call-template>
        	</xsl:when>
        	<xsl:otherwise>
                <xsl:message terminate="no">
                    <xsl:text>Book abbreviation </xsl:text>
                    <xsl:value-of select="$refAbbr"/>
                    <xsl:text> not found in </xsl:text>
                    <xsl:value-of select="$bookNames"/>
                    <xsl:text> at </xsl:text>
                    <xsl:value-of select="./preceding::book/@code"/>
                    <xsl:text> </xsl:text>
                    <xsl:value-of select="./*/text()[1]"/>
                </xsl:message>
                <xsl:value-of select="$ref"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="ReferenceFindChapter">
        <xsl:param name="ref"/>
        <xsl:param name="refCV"/>
        <xsl:param name="refBook"/>
        <xsl:param name="chap"/>
        <xsl:param name="verseListLeft"/>
        <xsl:variable name="refCode" select="$refBook/@code"/>
        <xsl:variable name="c" select="substring-before($refCV, $refPunc)"/>
        <xsl:choose>
            <!-- Found Chapter -->
            <xsl:when test="$c != ''">
                <xsl:call-template name="Reference">
                    <xsl:with-param name="ref" select="$ref"/>
                    <xsl:with-param name="refBook" select="$refBook"/>
                    <xsl:with-param name="c" select="$c"/>
                    <xsl:with-param name="v" select="substring-after($refCV, $refPunc)"/>
                </xsl:call-template>
                <xsl:if test="$verseListLeft != ''">
                    <xsl:text>, </xsl:text>
                    <xsl:call-template name="CrossRefVerseListIter">
                        <xsl:with-param name="ref" select="$verseListLeft"/>
                        <xsl:with-param name="book" select="$refBook"/>
                        <xsl:with-param name="chap" select="$c"/>
                    </xsl:call-template>
                </xsl:if>
            </xsl:when>
            <!-- Single chapter book -->
            <xsl:when test="$chap = '' and not(contains($verseRefs[@code = $refCode]/text(), ' '))">
                <xsl:call-template name="Reference">
                    <xsl:with-param name="ref" select="$ref"/>
                    <xsl:with-param name="refBook" select="$refBook"/>
                    <xsl:with-param name="c" select="1"/>
                    <xsl:with-param name="v" select="$refCV"/>
                </xsl:call-template>
                <xsl:if test="$verseListLeft != ''">
                    <xsl:text>, </xsl:text>
                    <xsl:call-template name="CrossRefVerseListIter">
                        <xsl:with-param name="ref" select="$verseListLeft"/>
                        <xsl:with-param name="book" select="$refBook"/>
                        <xsl:with-param name="chap" select="1"/>
                    </xsl:call-template>
                </xsl:if>
            </xsl:when>
            <!-- Chapter range (chapter without verse number -->
            <xsl:when test="$chap = ''">
                <xsl:call-template name="Reference">
                    <xsl:with-param name="ref" select="$ref"/>
                    <xsl:with-param name="refBook" select="$refBook"/>
                    <xsl:with-param name="c" select="$refCV"/>
                    <xsl:with-param name="v" select="1"/>
                </xsl:call-template>
                <xsl:if test="$verseListLeft != ''">
                    <xsl:text>, </xsl:text>
                    <xsl:call-template name="CrossRefVerseListIter">
                        <xsl:with-param name="ref" select="$verseListLeft"/>
                        <xsl:with-param name="book" select="$refBook"/>
                        <xsl:with-param name="chap" select="$refCV"/>
                    </xsl:call-template>
                </xsl:if>
            </xsl:when>
            <!-- Continue verse list in previously given chapter -->
            <xsl:otherwise>
                <xsl:call-template name="Reference">
                    <xsl:with-param name="ref" select="$ref"/>
                    <xsl:with-param name="refBook" select="$refBook"/>
                    <xsl:with-param name="c" select="$chap"/>
                    <xsl:with-param name="v" select="$refCV"/>
                </xsl:call-template>
                <xsl:if test="$verseListLeft != ''">
                    <xsl:text>, </xsl:text>
                    <xsl:call-template name="CrossRefVerseListIter">
                        <xsl:with-param name="ref" select="$verseListLeft"/>
                        <xsl:with-param name="book" select="$refBook"/>
                        <xsl:with-param name="chap" select="$chap"/>
                    </xsl:call-template>
                </xsl:if>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template name="Reference">
        <xsl:param name="ref"/>
        <xsl:param name="refBook"/>
        <xsl:param name="c"/>
        <xsl:param name="v"/>
        <xsl:variable name="refCode" select="$refBook/@code"/>
        <xsl:variable name="b" select="count($verseRefs[@code=$refCode]/preceding-sibling::*) + 1"/>
        <xsl:choose>
            <xsl:when test="$c != ''">
                <xsl:text disable-output-escaping="yes"><![CDATA[<a href="]]></xsl:text>
                <xsl:value-of select="$refPref"/>
                <xsl:value-of select="$b"/>
                <xsl:text>.</xsl:text>
                <xsl:value-of select="$c"/>
                <xsl:text>.</xsl:text>
                <xsl:value-of select="$v"/>
                <xsl:text disable-output-escaping="yes"><![CDATA[">]]></xsl:text>
                <xsl:value-of select="$ref"/>
                <xsl:text disable-output-escaping="yes"><![CDATA[</a>]]></xsl:text>
            </xsl:when>
            <xsl:otherwise>
                <xsl:message terminate="no">
                    <xsl:text>Reference punctuation (</xsl:text>
                    <xsl:value-of select="$refPunc"/>
                    <xsl:text>) not found in reference </xsl:text>
                    <xsl:value-of select="$ref"/>
                </xsl:message>
                <xsl:value-of select="$ref"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
</xsl:stylesheet>