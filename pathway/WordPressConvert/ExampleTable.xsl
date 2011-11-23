<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        ExampleTable.xsl
    # Purpose:     Format examples into tables
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2011/11/02
    # Copyright:   (c) 2011 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0" xmlns:x="http://www.w3.org/1999/xhtml">
    <xsl:param name="GraphicPath">http://pathway.sil.org/cherokeedemo/wp-content/icons/</xsl:param>
    
    <xsl:output encoding="UTF-8" method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" 
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>

    <!-- Copy header and process body -->
    <xsl:template match="x:html">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:copy-of select="x:head"/>
            <xsl:apply-templates select="x:body"/>
        </xsl:copy>
    </xsl:template>
    
    <!-- The table will be created at the body level -->
    <xsl:template match="x:body">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>
    
    <!-- Copy all div's except entry, letHead and letData will require special processing -->
    <xsl:template match="x:div[@class != 'entry' and @class != 'letHead' and @class != 'letData']">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>
    
    <!-- Copy spans and anchors (in general) -->
    <xsl:template match="x:span | x:a">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>
    
    <!-- We want to remove the letHead. -->
    <xsl:template match="x:div[@class = 'letHead']">
    </xsl:template>
    
    <!-- We want to remove the letData level of the hierarchy. -->
    <xsl:template match="x:div[@class = 'letData']">
        <xsl:apply-templates select="node()"/>
    </xsl:template>

    <!-- entry div's put out the headword, pronunciation, part of speech and definition -->
    <xsl:template match="x:div[@class = 'entry']">
        <xsl:if test="count(x:span[@class='headword']) &gt; 0">
            <xsl:copy>
                <xsl:for-each select="@*">
                    <xsl:copy/>
                </xsl:for-each>
                <xsl:copy-of select="x:span[@class='headword']"/>
                <xsl:element name="table" namespace="http://www.w3.org/1999/xhtml">
                    <xsl:attribute name="border">0</xsl:attribute>
                    <xsl:attribute name="style">width:100%;margin-left:0pt;text-indent:0pt;</xsl:attribute>
                    <!-- xsl:attribute name="valign">top</xsl:attribute -->
                    <xsl:element name="tr" namespace="http://www.w3.org/1999/xhtml">
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:if test="count(preceding-sibling::x:div) + count(parent::node()/preceding-sibling::x:div[@class='letData'])&gt; 0">
                                <xsl:element name="a" namespace="http://www.w3.org/1999/xhtml">
                                    <xsl:attribute name="href">
                                        <xsl:text>#</xsl:text>
                                        <xsl:if test="count(preceding-sibling::x:div) &gt; 0">
                                            <xsl:value-of select="preceding-sibling::x:div[1]/@id"/>
                                        </xsl:if>
                                        <xsl:if test="count(preceding-sibling::x:div) = 0">
                                            <xsl:if test="count(parent::node()/preceding-sibling::x:div[@class='letData'])&gt; 0">
                                                <xsl:value-of select="parent::node()/preceding-sibling::x:div[@class='letData'][1]/x:div[last()]/@id"/>
                                            </xsl:if>
                                        </xsl:if>
                                    </xsl:attribute>
                                    <xsl:element name="img" namespace="http://www.w3.org/1999/xhtml">
                                        <xsl:attribute name="src">
                                            <xsl:value-of select="$GraphicPath"/>
                                            <xsl:text>ArrowLeft.png</xsl:text>
                                        </xsl:attribute>
                                        <xsl:attribute name="width">64</xsl:attribute>
                                        <xsl:attribute name="height">64</xsl:attribute>
                                        <xsl:attribute name="border">0</xsl:attribute>
                                        <xsl:attribute name="alt">
                                            <xsl:text>&lt;&#x2014;</xsl:text>
                                        </xsl:attribute>
                                    </xsl:element>
                                </xsl:element>
                            </xsl:if>
                            <xsl:text>&#x2002;</xsl:text>
                        </xsl:element>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:text>&#x2002;</xsl:text>
                        </xsl:element>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:attribute name="style">text-align:right;</xsl:attribute>
                            <xsl:text>&#x2002;</xsl:text>
                            <xsl:if test="count(following-sibling::x:div) + count(parent::node()/following-sibling::x:div[@class='letData']) &gt; 0">
                                <xsl:element name="a" namespace="http://www.w3.org/1999/xhtml">
                                    <xsl:attribute name="href">
                                        <xsl:text>#</xsl:text>
                                        <xsl:if test="count(following-sibling::x:div) &gt; 0">
                                            <xsl:value-of select="following-sibling::x:div[1]/@id"/>
                                        </xsl:if>
                                        <xsl:if test="count(following-sibling::x:div) = 0">
                                            <xsl:if test="count(parent::node()/following-sibling::x:div[@class='letData']) &gt; 0">
                                                <xsl:value-of select="parent::node()/following-sibling::x:div[@class='letData'][1]/x:div[1]/@id"/>
                                            </xsl:if>
                                        </xsl:if>
                                    </xsl:attribute>
                                    <xsl:element name="img" namespace="http://www.w3.org/1999/xhtml">
                                        <xsl:attribute name="src">
                                            <xsl:value-of select="$GraphicPath"/>
                                            <xsl:text>ArrowRight.png</xsl:text>
                                        </xsl:attribute>
                                        <xsl:attribute name="width">64</xsl:attribute>
                                        <xsl:attribute name="height">64</xsl:attribute>
                                        <xsl:attribute name="border">0</xsl:attribute>
                                        <xsl:attribute name="alt">
                                            <xsl:text>&#x2014;&gt;</xsl:text>
                                        </xsl:attribute>
                                    </xsl:element>
                                </xsl:element>
                            </xsl:if>
                            <xsl:if test="count(following-sibling::x:div) = 0">
                                <xsl:if test="count(parent::node()/following-sibling::x:div[@class='letData'][1]) &gt; 0">
                                    
                                </xsl:if>
                            </xsl:if>
                            <!-- parent::node()/following-sibling::x:div[@class='letData'][1]/x:div[1]/@id -->
                        </xsl:element>
                    </xsl:element>
                    <xsl:element name="tr" namespace="http://www.w3.org/1999/xhtml">
                        <!-- xsl:attribute name="width">100%</xsl:attribute -->
                        <xsl:attribute name="style">border:2pt solid red;border-bottom:0;color:red;</xsl:attribute>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:attribute name="class">col1top</xsl:attribute>
                            <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                                <xsl:attribute name="class">stemhd</xsl:attribute>
                                <xsl:text>English entry:</xsl:text>
                            </xsl:element>
                        </xsl:element>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:attribute name="style">text-align:left;</xsl:attribute>
                            <xsl:attribute name="class">col2data</xsl:attribute>
                            <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                                <xsl:attribute name="class">glo</xsl:attribute>
                                <xsl:copy-of select="x:span/x:span/x:span[@class='definition']"/>
                            </xsl:element>
                        </xsl:element>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:attribute name="class">col2data</xsl:attribute>
                            <xsl:text>&#x2002;</xsl:text>
                        </xsl:element>
                    </xsl:element>
                    <xsl:element name="tr" namespace="http://www.w3.org/1999/xhtml">
                        <!-- xsl:attribute name="width">100%</xsl:attribute -->
                        <xsl:attribute name="style">border:2pt solid red;border-top:0;border-bottom:0;color:red;</xsl:attribute>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:attribute name="class">col1top</xsl:attribute>
                            <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                                <xsl:attribute name="class">stemhd</xsl:attribute>
                                <xsl:text>Cherokee:</xsl:text>
                            </xsl:element>
                        </xsl:element>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:attribute name="style">text-align:left;</xsl:attribute>
                            <xsl:attribute name="class">col2data</xsl:attribute>
                            <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                                <xsl:attribute name="class">def</xsl:attribute>
                                <xsl:copy-of select="x:span[@class='LexEntry-publishRootPara-CitationFormPub_L2']"/>
                            </xsl:element>
                        </xsl:element>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:attribute name="class">col2data</xsl:attribute>
                            <xsl:text>&#x2002;</xsl:text>
                        </xsl:element>
                    </xsl:element>
                    <xsl:element name="tr" namespace="http://www.w3.org/1999/xhtml">
                        <!-- xsl:attribute name="width">100%</xsl:attribute -->
                        <xsl:attribute name="style">border:2pt solid red;border-top:0;color:red;</xsl:attribute>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:attribute name="class">col1top</xsl:attribute>
                            <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                                <xsl:attribute name="class">stemhd</xsl:attribute>
                                <xsl:text>Stem:</xsl:text>
                            </xsl:element>
                        </xsl:element>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:attribute name="style">text-align:left;</xsl:attribute>
                            <xsl:attribute name="class">col2data</xsl:attribute>
                            <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                                <xsl:attribute name="class">stm</xsl:attribute>
                                <xsl:copy-of select="x:span[@class='headword']"/>
                                <xsl:copy-of select="x:span/x:span/x:span[@class='grammatical-info']"/>
                            </xsl:element>
                        </xsl:element>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:attribute name="class">col2data</xsl:attribute>
                            <xsl:text>&#x2002;</xsl:text>
                        </xsl:element>
                    </xsl:element>
                    <xsl:element name="tr" namespace="http://www.w3.org/1999/xhtml">
                        <!-- xsl:attribute name="width">100%</xsl:attribute -->
                        <xsl:attribute name="style">border:0;</xsl:attribute>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:attribute name="class">col1head</xsl:attribute>
                            <xsl:text>Syllabary</xsl:text>
                        </xsl:element>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:attribute name="class">col2head</xsl:attribute>
                            <xsl:text>Phonetics</xsl:text>
                        </xsl:element>
                        <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:attribute name="class">col2head</xsl:attribute>
                            <xsl:text>English</xsl:text>
                        </xsl:element>
                    </xsl:element>
                    <xsl:apply-templates select="x:span/x:span/x:span[@class='examples']" mode="example"/>
                    <xsl:apply-templates select="x:div[@class='subentries']/x:span/x:a" mode="subentry"/>
                    <xsl:if test="count(x:span/x:span/x:span[@class='LexSense-publishRoot-Semantic--Field']) &gt; 0">
                        <xsl:element name="tr" namespace="http://www.w3.org/1999/xhtml">
                            <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                                <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                                    <xsl:attribute name="class">semantic-domains</xsl:attribute>
                                    <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                                        <xsl:attribute name="class">semantic-domain-name</xsl:attribute>
                                        <xsl:attribute name="lang">
                                            <xsl:value-of select="x:span/x:span/x:span[@class='LexSense-publishRoot-Semantic--Field']/@lang"/>
                                        </xsl:attribute>
                                        <xsl:value-of select="x:span/x:span/x:span[@class='LexSense-publishRoot-Semantic--Field']//text()"/>
                                    </xsl:element>
                                </xsl:element>
                            </xsl:element>
                        </xsl:element>
                    </xsl:if>
                </xsl:element>
            </xsl:copy>
        </xsl:if>
    </xsl:template>
    
    <!-- Example list or individal examples at the entry level -->
    <xsl:template match="x:span/x:span[@class='xitem'] | x:span[x:span[@class!='xitem']]" mode="example">
        <xsl:if test="count(x:span) &gt; 0">
            <xsl:element name="tr" namespace="http://www.w3.org/1999/xhtml">
                <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                    <xsl:attribute name="class">col1data</xsl:attribute>
                    <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                        <xsl:attribute name="class">syll</xsl:attribute>
                        <xsl:copy-of select="x:span/x:span[@lang='chr']"/>
                    </xsl:element>
                </xsl:element>
                <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                    <xsl:attribute name="class">col2data</xsl:attribute>
                    <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                        <xsl:attribute name="class">exam</xsl:attribute>
                        <xsl:if test="count(x:span/x:span[@lang='chr-x-acc']) &gt; 0">
                            <xsl:copy-of select="x:span/x:span[@lang='chr-x-acc']"/>
                        </xsl:if>
                        <xsl:if test="count(x:span/x:span[@lang='chr-x-acc']) = 0">
                            <xsl:copy-of select="x:span/x:span[@lang='chr-QM-x-sp']"/>
                        </xsl:if>
                    </xsl:element>
                </xsl:element>
                <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                    <xsl:attribute name="class">col2data</xsl:attribute>
                    <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                        <xsl:attribute name="class">tran</xsl:attribute>
                        <xsl:copy-of select="x:span/x:span[@class='translation'][@lang='en']"/>
                    </xsl:element>
                </xsl:element>
            </xsl:element>
        </xsl:if>
    </xsl:template>
    
    <!-- Subentries -->
    <xsl:template match="x:div/x:a" mode="subentry">
        <xsl:if test="count(x:span[@class='headword-sub']) &gt; 0">
            <xsl:element name="tr" namespace="http://www.w3.org/1999/xhtml">
                <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                    <xsl:attribute name="class">col1head</xsl:attribute>
                    <xsl:if test="x:span/x:span[@class='entry-type-abbr-sub']//text() = 'PRC'">
                        <xsl:text>Present Continuous</xsl:text>
                    </xsl:if>
                    <xsl:if test="x:span/x:span[@class='entry-type-abbr-sub']//text() = 'INC'">
                        <xsl:text>Incompletive</xsl:text>
                    </xsl:if>
                    <xsl:if test="x:span/x:span[@class='entry-type-abbr-sub']//text() = 'IMM'">
                        <xsl:text>Immediate</xsl:text>
                    </xsl:if>
                    <xsl:if test="x:span/x:span[@class='entry-type-abbr-sub']//text() = 'CMP'">
                        <xsl:text>Completive</xsl:text>
                    </xsl:if>
                    <xsl:if test="x:span/x:span[@class='entry-type-abbr-sub']//text() = 'DVN'">
                        <xsl:text>Deverbal Noun</xsl:text>
                    </xsl:if>
                    <xsl:if test="x:span/x:span[@class='entry-type-abbr-sub']//text() = 'pl'">
                        <xsl:text>Plural</xsl:text>
                    </xsl:if>
                </xsl:element>
                <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                    <xsl:attribute name="class">col2head</xsl:attribute>
                    <xsl:text>&#x2002;</xsl:text>
                </xsl:element>
                <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                    <xsl:attribute name="class">col2head</xsl:attribute>
                    <xsl:text>&#x2002;</xsl:text>
                </xsl:element>
            </xsl:element>
            <xsl:apply-templates select="x:span/x:span/x:span[@class='examples-sub']" mode="subexample"/>
        </xsl:if>
    </xsl:template>

    <!-- Subentry examples -->
    <!-- x:span/x:span[@class='xitem'] | x:span[x:span[@class!='xitem']] -->
    <xsl:template match="x:span/x:span[@class='xitem'] | x:span[x:span[@class!='xitem']]" mode="subexample">
        <xsl:if test="count(x:span) &gt; 0">
            <xsl:element name="tr" namespace="http://www.w3.org/1999/xhtml">
                <xsl:attribute name="class">
                    <xsl:text>tableRow</xsl:text>
                    <xsl:value-of select="ancestor-or-self::node()/x:span/x:span[@class='entry-type-abbr-sub']//text()"/>
                </xsl:attribute>
                <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                    <xsl:attribute name="class">col1data</xsl:attribute>
                    <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                        <xsl:attribute name="class">syll</xsl:attribute>
                        <xsl:copy-of select="x:span/x:span[@lang='chr']"/>
                    </xsl:element>
                </xsl:element>
                <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                    <xsl:attribute name="class">col2data</xsl:attribute>
                    <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                        <xsl:attribute name="class">exam</xsl:attribute>
                        <xsl:if test="count(x:span/x:span[@lang='chr-x-acc']) &gt; 0">
                            <xsl:copy-of select="x:span/x:span[@lang='chr-x-acc']"/>
                        </xsl:if>
                        <xsl:if test="count(x:span/x:span[@lang='chr-x-acc']) = 0">
                            <xsl:copy-of select="x:span/x:span[@lang='chr-QM-x-sp']"/>
                        </xsl:if>
                    </xsl:element>
                </xsl:element>
                <xsl:element name="td" namespace="http://www.w3.org/1999/xhtml">
                    <xsl:attribute name="class">col2data</xsl:attribute>
                    <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
                        <xsl:attribute name="class">tran</xsl:attribute>
                        <xsl:copy-of select="x:span/x:span[@class='translation-sub'][@lang='en']"/>
                    </xsl:element>
                </xsl:element>
            </xsl:element>
        </xsl:if>
    </xsl:template>
    
</xsl:stylesheet>