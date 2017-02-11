<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:xhtml="http://www.w3.org/1999/xhtml"
  xmlns:ncx="http://www.daisy.org/z3986/2005/ncx/">
  <xsl:output method="html" encoding="UTF-8" indent="yes"/>

  <!-- Recursive copy template -->   
  <xsl:template match="node() | @*">
      <xsl:apply-templates select="node() | @*"/>
  </xsl:template>
  
  <!-- update root -->
  <xsl:template match="/">
    <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
    <xsl:element name="html" namespace="http://www.w3.org/1999/xhtml">
      <xsl:element name="head" namespace="http://www.w3.org/1999/xhtml">
        <xsl:element name="title">Table of Contents</xsl:element>
      </xsl:element>
      <xsl:element name="body" namespace="http://www.w3.org/1999/xhtml">
        <xsl:element name="li" namespace="http://www.w3.org/1999/xhtml">
          <xsl:element name="a" namespace="http://www.w3.org/1999/xhtml">
            <xsl:attribute name="href">index.html</xsl:attribute>
            <xsl:text>Table of Contents</xsl:text>
          </xsl:element>
        </xsl:element>
        <xsl:apply-templates select="//ncx:navMap/ncx:navPoint"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="ncx:navPoint">
    <xsl:param name="level" select="2"/>
    
    <xsl:element name="li" namespace="http://www.w3.org/1999/xhtml">
      <xsl:attribute name="id">
        <xsl:value-of select="@id"/>
      </xsl:attribute>
      <xsl:element name="a" namespace="http://www.w3.org/1999/xhtml">
        <xsl:attribute name="href">
          <xsl:value-of select="ncx:content/@src"/>
        </xsl:attribute>
        <xsl:value-of select="ncx:navLabel/ncx:text"/>
        <xsl:if test="count(ncx:navPoint) > 0">
          <xsl:element name="span" namespace="http://www.w3.org/1999/xhtml">
            <xsl:attribute name="class">fa arrow</xsl:attribute>
          </xsl:element>
        </xsl:if>
      </xsl:element>
      <xsl:if test="count(ncx:navPoint) > 0">
        <xsl:element name="ul" namespace="http://www.w3.org/1999/xhtml">
          <xsl:choose>
            <xsl:when test="$level = 3">
              <xsl:attribute name="class">nav nav-third-level</xsl:attribute>
            </xsl:when>
            <xsl:when test="$level = 4">
              <xsl:attribute name="class">nav nav-fourth-level</xsl:attribute>
            </xsl:when>
            <xsl:when test="$level = 5">
              <xsl:attribute name="class">nav nav-fifth-level</xsl:attribute>
            </xsl:when>
          </xsl:choose>
          <xsl:apply-templates select="ncx:navPoint">
            <xsl:with-param name="level" select="$level + 1"/>
          </xsl:apply-templates>
        </xsl:element>
      </xsl:if>
    </xsl:element>
  </xsl:template>
  
</xsl:stylesheet>