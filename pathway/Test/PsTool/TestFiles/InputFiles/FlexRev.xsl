<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        FLExRev.xsl
    # Purpose:     Given the FLEx reversal XML , prepare XHTML.
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2008/08/29
    # RCS-ID:      $Id: FLExRev.xsl $
    # Copyright:   (c) 2008 SIL
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns="http://www.w3.org/1999/xhtml"
    xmlns:myObj="urn:reversal-conv">
    <xsl:output
        doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" 
        doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd" 
        encoding="UTF-8"  
        method="xhtml"
        media-type="text/xhtml" 
        indent="yes"/>
    
    <xsl:template match="/">
        <html xml:lang="utf-8" lang="utf-8">
            <head>
                <title>Reversal XHTML</title>
                <link type="text/css" rel="stylesheet" href="FLExRev.css"/>
            </head>
            <body class="revAppendix">
                <div class="revHeader">
                    <xsl:text>Reversal Appendix</xsl:text>
                </div>
                <div class="revData">
                    <xsl:apply-templates/>
                </div>
            </body>
        </html>
    </xsl:template>

    <xsl:template match="ReversalIndexEntry">
        <div class="revEntry">
            <xsl:apply-templates/>
        </div>
    </xsl:template>

    <xsl:template match="ReversalIndexEntry_ReversalForm">
        <span class="headword">
            <xsl:attribute name="lang">
                <xsl:value-of select="AUni/@ws"/>
            </xsl:attribute>
            <xsl:value-of select="AUni"/>
        </span>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="ReversalIndexEntry_ReferringSenses">
        <xsl:for-each select="Link">
            <span class="revSense">
                <xsl:attribute name="lang">
                    <xsl:value-of select="Alt/@ws"/>
                </xsl:attribute>
                <xsl:variable name="sense" select="myObj:SenseNumber(Alt/@sense,'sense')"/>
                <xsl:if test="$sense != ''">
                    <xsl:value-of select="$sense "/>
                </xsl:if>

                <xsl:variable name="homograph" select="myObj:SenseNumber(Alt/@sense,'homograph')"/>
                <xsl:if test="$homograph != ''">
                    <span class="revhomographnumber">
                        <xsl:value-of select="$homograph "/>
                    </span>
                </xsl:if>

                <xsl:variable name="sensenumber"
                    select="myObj:SenseNumber(Alt/@sense,'sensenumber')"/>
                <xsl:if test="$sensenumber != ''">
                    <span class="revsensenumber">
                        <xsl:value-of select="$sensenumber "/>
                    </span>
                </xsl:if>
            </span>
        </xsl:for-each>
    </xsl:template>

    <!-- The next construct removes stray text from apply-templates. Text must be explicitly processed. -->
    <xsl:template match="text()"/>
</xsl:stylesheet>
