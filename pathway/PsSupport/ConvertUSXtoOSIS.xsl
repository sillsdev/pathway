<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="yes"
	 doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"/>
    <xsl:param name="xmlLang"></xsl:param>
    <xsl:param name="ws" select="'es'"/>
    <xsl:param name="li" select="'font-family:12pt;'"/>
	<!-- The templates matching * and @* match and copy unhandled elements/attributes. -->
	<xsl:template match="*">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="@*">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match="/">
		<xsl:apply-templates/>
	</xsl:template>
    
    <xsl:template match="usfm|usx">
        <osis xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://www.bibletechnologies.net/2003/OSIS/namespace" xsi:schemaLocation="http://www.bibletechnologies.net/2003/OSIS/namespace file:../osisCore.2.0_UBS_SIL_BestPractice.xsd">
            <osisText osisIDWork="thisWork" osisRefWork="bible" xml:lang="{$xmlLang}">
                <header>
                    <work osisWork="thisWork" />
                </header>
                <xsl:apply-templates/>
            </osisText>
        </osis>
    </xsl:template>
    
    <!-- Define the book. -->
    <xsl:template match="book">
        <xsl:variable name="bookCode">
            <xsl:choose>
                <xsl:when test="@code">
                    <xsl:value-of select="@code"/>
                </xsl:when>
                <xsl:when test="@id">
                    <!-- Support old format for USX -->
                    <xsl:value-of select="@id"/>
                </xsl:when>
            </xsl:choose>
        </xsl:variable>
        <xsl:variable name="bookInToc" select="normalize-space(para[@style='toc2'])"/>
        <xsl:variable name="bookHeading" select="normalize-space(para[@style='h'])"/>
        <xsl:variable name="bookTitle" select="normalize-space(para[@style='mt'])"/>
        <xsl:variable name="bookTitle1" select="normalize-space(para[@style='mt1'])"/>
        
        <div type="book" osisID="{$bookCode}">
            <xsl:apply-templates/>
        </div>
    </xsl:template>
    <xsl:template match="para">
    <xsl:choose>
        <xsl:when test="@style='mt' or @style='mt1'">
            <xsl:element name="title">
                <xsl:attribute name="type">
                    <xsl:text>main</xsl:text>
                </xsl:attribute>
                <xsl:element name="title">
                    <xsl:attribute name="level">
                        <xsl:text>1</xsl:text>
                    </xsl:attribute>
                    <xsl:apply-templates/>
                </xsl:element>
            </xsl:element>
        </xsl:when>
        <xsl:when test="@style='rem'">
            <xsl:comment>
                <xsl:text>\</xsl:text>
                <xsl:value-of select="@style"/>
                <xsl:text> </xsl:text>
                <xsl:copy-of select="text()"/>
            </xsl:comment>
         </xsl:when>
        <xsl:otherwise>
            <p>
                <xsl:apply-templates/>
            </p>
        </xsl:otherwise>
    </xsl:choose>
    </xsl:template>
    <xsl:template match="para[@style='h']">
        <xsl:element name="title">
            <xsl:attribute name="short">
                <xsl:value-of select="text()"/>
            </xsl:attribute>
        </xsl:element>
    </xsl:template>
    <xsl:template match="para[@style='li1']">
        <div>
        <list>
            <item style="{$li}">
                <xsl:apply-templates/>
                <xsl:if test="following-sibling::*[1]/@style='li2'">
                    <xsl:apply-templates select="following-sibling::*[1]" mode="listitem"/>
                </xsl:if>
            </item>
        </list>
        </div>
    </xsl:template>
    <xsl:template match="para[@style='li2']" mode="listitem">
        <list>
            <item>
                <xsl:apply-templates/>
                <xsl:if test="following-sibling::*[1]/@style='li3'">
                    <xsl:apply-templates select="following-sibling::*[1]" mode="listitem"/>
                </xsl:if>
            </item>
        </list>
    </xsl:template>
    <xsl:template match="para[@style='li3']" mode="listitem">
        <list>
            <item>
                <xsl:apply-templates/>
            </item>
        </list>
    </xsl:template>
    <xsl:template match="para[@style='s1']">
        <div type="section">
            <title>
                <xsl:apply-templates/>
            </title>
        </div>
    </xsl:template>
    <xsl:template match="para[@style='s2']">
        <div type="subSection">
            <title>
                <xsl:apply-templates/>
            </title>
        </div>
    </xsl:template>
    
    <xsl:template match="para[@style='d']">
        <div align="center">
            <hi type="italic">
                <xsl:apply-templates/>
            </hi>
        </div>
    </xsl:template>
    
    <xsl:template match="para[@style='li2']"/>
    <xsl:template match="para[@style='li3']"/>
    <xsl:template match="para[@style='toc1' or @style='toc2' or @style='toc3']"/>
</xsl:stylesheet>
