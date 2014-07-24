<?xml version="1.0"?>
<xsl:stylesheet version="1.0"
    exclude-result-prefixes="ncx xsl"
    xmlns="http://www.w3.org/1999/xhtml"
    xmlns:ncx="http://www.daisy.org/z3986/2005/ncx/"
    xmlns:epub="http://www.idpf.org/2007/ops"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" omit-xml-declaration="yes" indent="no"/>
  <xsl:template match="ncx:ncx">
    <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
    <xsl:text>&#xa;</xsl:text>
    <html>
      <head>
        <title>
          <xsl:apply-templates select="/ncx:ncx/ncx:docTitle/ncx:text" mode="header"/>
        </title>
        <xsl:text></xsl:text>
        <xsl:apply-templates select="ncx:head" mode="head"/>
      </head>
      <body>
        <xsl:apply-templates />
      </body>
    </html>
  </xsl:template>
  <xsl:template match="ncx:head" mode="head">
    <link type="text/css" rel="stylesheet" href="book.css"/>
  </xsl:template>
  <xsl:template match="ncx:navMap" >
    <nav id="toc" epub:type="toc">
      <xsl:copy-of select="@class"/>
      <xsl:choose>
        <xsl:when test="ncx:navLabel">
          <!--<xsl:apply-templates select="ncx:navLabel" mode="heading"/>-->
          <xsl:copy-of select="local-name(child::*)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:if test="self::ncx:navMap">
            <h1>Table of Contents</h1>
          </xsl:if>
        </xsl:otherwise>
      </xsl:choose>
      <ol>
        <xsl:apply-templates select="ncx:navPoint|ncx:navLabel"/>
      </ol>
    </nav>
  </xsl:template>

  <xsl:template match="ncx:navPoint">
    <xsl:text>&#10;</xsl:text>
    <li>
      <xsl:copy-of select="@id|@class"/>

      <!-- every navPoint and pageTarget has to have a navLabel and content -->
      <a>
        <xsl:attribute name="href">
          <xsl:apply-templates select="ncx:content[1]/@src"/>
        </xsl:attribute>
        <xsl:apply-templates select="ncx:navLabel"/>
      </a>

      <!-- Only some navPoints have more navPoints inside them for deep NCXes. pageTargets cannot nest. -->
      <xsl:if test="ncx:navPoint">
        <xsl:text>&#10;</xsl:text>
        <ol>
          <xsl:apply-templates select="ncx:navPoint"/>
        </ol>
      </xsl:if>
    </li>
  </xsl:template>

  <xsl:template match="ncx:navLabel|ncx:text" mode="header">
    <xsl:apply-templates/>

  </xsl:template>
  <xsl:template match="ncx:navLabel|ncx:text">
    <xsl:value-of select="."/>

  </xsl:template>
  <xsl:template match="@xmlns"></xsl:template>

  <!-- Ignore these elements -->
  <xsl:template match="ncx:head|
        ncx:docAuthor|
        ncx:docTitle|
        ncx:pageList/ncx:navLabel"/>
  <xsl:template match="ncx:head/text()|
        ncx:docAuthor/text()|
        ncx:docTitle/text()|
        ncx:navLabel/text()"/>

  <!-- Default rule to catch omissions -->
  <xsl:template match="*">
    <xsl:message terminate="yes">
      ERROR: <xsl:value-of select="name(.)"/> not matched!
    </xsl:message>
  </xsl:template>
  <xsl:template name="string-replace-all">
    <xsl:param name="text" />
    <xsl:param name="replace" />
    <xsl:param name="by" />
    <xsl:choose>
      <xsl:when test="contains($text, $replace)">
        <xsl:value-of select="substring-before($text,$replace)" />
        <xsl:value-of select="$by" />
        <xsl:call-template name="string-replace-all">
          <xsl:with-param name="text"
              select="substring-after($text,$replace)" />
          <xsl:with-param name="replace" select="$replace" />
          <xsl:with-param name="by" select="$by" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$text" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:variable name="in">
    <xsl:text>.xhtml</xsl:text>
  </xsl:variable>
  <xsl:variable name="out">
    <xsl:text>.html</xsl:text>
  </xsl:variable>
  <xsl:template match="ncx:content[1]/@src">
    <xsl:call-template name="string-replace-all">
      <xsl:with-param name="text" select="." />
      <xsl:with-param name="replace" select="$in" />
      <xsl:with-param name="by" select="$out" />
    </xsl:call-template>
  </xsl:template>
  <xsl:template name="html-head">
    <head>

    </head>
  </xsl:template>

</xsl:stylesheet>
