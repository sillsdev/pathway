<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        Combine Relations.xsl
    # Purpose:     Combine sequences of Ant Cf and Syn w/ single abbr
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2012/10/17
    # Updates:     2012/10/18 gt-refactored to make type copy simpler
    #              2012/10/18 gt-add hyperlink for cross references
    #              2012/10/19 gt-subscript homonyn numbers
    #              2012/10/22 gt-Flex period in class name bug work around
    #              2012/10/26 gt-test for missing picture sense number
    #              2012/12/05 gt-shorten path to image
    #              2015/10/22 gt-allow sets of synonyms, add derivatives
    #                           -handle sub-entries
    # Copyright:   (c) 2012 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
    xmlns:x="http://www.w3.org/1999/xhtml">
    <xsl:variable name="xhtml">http://www.w3.org/1999/xhtml</xsl:variable>
    <xsl:variable name="headwords" select="//*[@class='headword']/text()"/>
    <!-- Recursive copy template -->
    <xsl:template match="node() | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="x:img/@src">
        <xsl:attribute name="src">
            <xsl:value-of select="substring-after(.,'/LinkedFiles/')"/>
        </xsl:attribute>
    </xsl:template>

    <xsl:template match="*[@class = 'pictureCaption']">
        <xsl:copy>
            <xsl:if test="count(*) = 0">
                <xsl:message terminate="yes">Please ensure Picture Sense # is checked before export</xsl:message>
            </xsl:if>
            <xsl:apply-templates/>
        </xsl:copy>
    </xsl:template>

    <!-- Removes periods from style name (Flex bug work around) -->
    <xsl:template match="@class">
        <xsl:choose>
            <xsl:when test="contains(.,'definition1_L2')">
                <xsl:attribute name="class">definition</xsl:attribute>
            </xsl:when>
            <xsl:when test="contains(.,'relations1')">
                <xsl:attribute name="class">relations</xsl:attribute>
            </xsl:when>
            <xsl:when test="contains(.,'lexref-type1.0')">
                <xsl:attribute name="class">lexref-type</xsl:attribute>
            </xsl:when>
            <xsl:when test="contains(.,'lexref-targets1.0')">
                <xsl:attribute name="class">lexref-targets</xsl:attribute>
            </xsl:when>
            <xsl:when test="contains(.,'sense-crossref1.0.0')">
                <xsl:attribute name="class">sense-crossref</xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
                <xsl:attribute name="class">
                    <xsl:value-of select="translate(.,'.','')"/>
                </xsl:attribute>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <!-- Matches any element with the relations class -->
    <xsl:template match="*[starts-with(@class,'relations')] | *[starts-with(@class,'complexformrefs')]">
        <xsl:copy>
            <!-- Copies the attributes (like class name) -->
            <xsl:for-each select="@*">
                <xsl:apply-templates select="."/>
            </xsl:for-each>
            <xsl:choose>
                <!-- When there are multiple relations on a single entry -->
                <xsl:when test="*[@class= 'xitem']">
                    <!-- Process syn then ant then cf -->
                    <xsl:call-template name="relation">
                        <xsl:with-param name="rel">syn</xsl:with-param>
                    </xsl:call-template>
                    <xsl:call-template name="relation">
                        <xsl:with-param name="rel">ant</xsl:with-param>
                    </xsl:call-template>
                    <xsl:call-template name="relation">
                        <xsl:with-param name="rel">cf</xsl:with-param>
                    </xsl:call-template>
                    <xsl:call-template name="relation">
                        <xsl:with-param name="rel">der.</xsl:with-param>
                    </xsl:call-template>
                </xsl:when>
                <!-- For a single relation on the entry just copy it. -->
                <xsl:otherwise>
                    <xsl:apply-templates/>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:copy>
    </xsl:template>

    <!-- Each kind of relation (syn, ant, and cf) -->
    <xsl:template name="relation">
        <xsl:param name="rel"/>
        <!-- Check if the type exists -->
        <xsl:if test=".//text() = $rel">
            <!-- Recreate the xitem node -->
            <xsl:element name="span" namespace="{$xhtml}">
                <xsl:attribute name="class">xitem</xsl:attribute>
                <!-- Copies the abbreviation from the input -->
                <xsl:apply-templates select="(*[.//text()=$rel]/*[starts-with(@class,'lexref-type')])[1]"/>
                <xsl:apply-templates select="(*[.//text()=$rel]/*[starts-with(@class,'complexform-type')])[1]"/>
                <!-- Select xitems of this type -->
                <xsl:for-each select="*[.//text() = $rel]">
                    <xsl:choose>
                        <xsl:when test="count(*[starts-with(@class,'lexref-targets')]/x:a) + count(*[starts-with(@class,'complexform-type')]) > 0">
                            <!-- Copy the target hyperlink -->
                            <xsl:apply-templates select="*[starts-with(@class,'lexref-targets')]/x:a"/>
                            <xsl:apply-templates select="*[starts-with(@class,'complexform-form')]"/>
                        </xsl:when>
                        <xsl:when test="count(*[starts-with(@class,'lexref-targets')]/x:span/x:a) >0">
                            <xsl:for-each select="*[starts-with(@class,'lexref-targets')]/x:span[x:a]">
                                <xsl:apply-templates select="x:a"/>
                                <!-- If not the last item insert medial punctuation -->
                                <xsl:if test="position() != last()">
                                    <xsl:element name="span" namespace="{$xhtml}">
                                        <xsl:attribute name="lang">en</xsl:attribute>
                                        <xsl:attribute name="xml:space">preserve</xsl:attribute>
                                        <xsl:text>, </xsl:text>
                                    </xsl:element>
                                </xsl:if>
                            </xsl:for-each>
                        </xsl:when>
                    </xsl:choose>
                    <xsl:choose>
                        <xsl:when test="position() != last()">
                            <!-- If not the last item insert medial punctuation -->
                            <xsl:element name="span" namespace="{$xhtml}">
                                <xsl:attribute name="lang">en</xsl:attribute>
                                <xsl:attribute name="xml:space">preserve</xsl:attribute>
                                <xsl:text>, </xsl:text>
                            </xsl:element>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:apply-templates select="following-sibling::*[1]"/>
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:for-each>
            </xsl:element>
        </xsl:if>
    </xsl:template>

    <!-- Add hyperlink for cross reference text to do a search for the text -->
    <xsl:template match="*[@class='Dictionary-CrossReferences']">
        <xsl:choose>
            <xsl:when test="text() = $headwords">
                <xsl:element name="a" namespace="{$xhtml}">
                    <xsl:attribute name="href">
                        <xsl:text>http://pacoh.webonary.org/?s=</xsl:text>
                        <xsl:value-of select="text()"/>
                        <xsl:text disable-output-escaping="yes"><![CDATA[&search=Search&tax=-1]]></xsl:text>
                    </xsl:attribute>
                    <!-- Inside the hyperlink we will put the original node -->
                    <xsl:copy>
                        <xsl:for-each select="@*">
                            <xsl:copy/>
                        </xsl:for-each>
                        <xsl:value-of select="text()"/>
                    </xsl:copy>
                </xsl:element>
            </xsl:when>
            <xsl:otherwise>
                <!-- If link not found, insert original element -->
                <xsl:copy>
                    <xsl:for-each select="@*">
                        <xsl:copy/>
                    </xsl:for-each>
                    <xsl:value-of select="text()"/>
                </xsl:copy>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <!-- Subscript homograph number -->
    <!--xsl:template match="*[@class='xhomographnumber']">
        <xsl:copy>
            <xsl:for-each select="@*">
                <xsl:copy/>
            </xsl:for-each>
            <xsl:text disable-output-escaping="yes"><![CDATA[&#x208]]></xsl:text>
            <xsl:value-of select="text()"/>
            <xsl:text>;</xsl:text>
        </xsl:copy>
    </xsl:template-->

</xsl:stylesheet>