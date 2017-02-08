<?xml version="1.0"?>
<xsl:stylesheet version="1.0"
                exclude-result-prefixes="ncx xsl"
                xmlns="http://www.w3.org/1999/xhtml"
                xmlns:ncx="http://www.daisy.org/z3986/2005/ncx/"
                xmlns:epub="http://www.idpf.org/2007/ops"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="ncx:ncx">
    <html>
      <head>
        <title>
          <xsl:apply-templates select="/ncx:ncx/ncx:docTitle/ncx:text"/>
        </title>
      </head>
      <body>
        <xsl:apply-templates />
      </body>
    </html>
  </xsl:template>

  <xsl:template match="ncx:navMap">
    <div id="TOCPage" class="Contents">
      <xsl:copy-of select="@class"/>
      <xsl:choose>
        <xsl:when test="ncx:navLabel">
          <xsl:apply-templates select="ncx:navLabel" mode="heading"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:if test="self::ncx:navMap">
            <h1>Table of Contents</h1>
          </xsl:if>
        </xsl:otherwise>
      </xsl:choose>
      <ul>
        <xsl:apply-templates select="ncx:navPoint/ncx:navPoint|ncx:navPoint"/>
      </ul>
    </div>
  </xsl:template>

  <xsl:template match="ncx:navPoint">
    <xsl:text>&#10;</xsl:text>
    <xsl:choose>
      <xsl:when test="ncx:navPoint/ncx:navPoint">
        <li>
          <h2>
            <xsl:value-of select="ncx:navLabel"></xsl:value-of>
          </h2>
          <ul>
            <xsl:apply-templates select="ncx:navPoint"/>
          </ul>
        </li>
      </xsl:when>
      <xsl:otherwise>
        <li>
          <xsl:copy-of select="@class"/>

          <!-- every navPoint and pageTarget has to have a navLabel and content -->
          <a href="{ncx:content[1]/@src}">
            <xsl:apply-templates select="ncx:navLabel"/>
          </a>

          <!-- Only some n
            avPoints have more navPoints inside them for deep NCXes. pageTargets cannot nest. -->
          <xsl:if test="ncx:navPoint">
            <ul>
              <xsl:apply-templates select="ncx:navPoint"/>
            </ul>
          </xsl:if>
        </li>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="ncx:navLabel|ncx:text">
    <xsl:apply-templates/>
  </xsl:template>

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

  <xsl:template name="html-head">
    <head>

    </head>
  </xsl:template>

</xsl:stylesheet>
