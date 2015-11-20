<xsl:stylesheet version="1.0" xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xml="http://www.w3.org/XML/1998/namespace"
    xmlns:epub="http://www.idpf.org/2007/ops"
    exclude-result-prefixes="xhtml xsl xs xml">
  <xsl:output method="html" encoding="utf-8" indent="no"/>
  <!-- New root for Html5-->
  <xsl:template match="xhtml:html">
    <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
    <xsl:text>&#xa;</xsl:text>
    <html xmlns="http://www.w3.org/1999/xhtml" xmlns:epub="http://www.idpf.org/2007/ops">
      <xsl:if test="@xml:lang">
        <xsl:attribute name="xml:lang">
          <xsl:value-of select="@xml:lang"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="@dir">
        <xsl:attribute name="dir">
          <xsl:value-of select="@dir"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates></xsl:apply-templates>
    </html>
  </xsl:template>

  <!-- template for string replace from old one to new one-->
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
  <!-- Recursive copy template -->
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
  </xsl:template>
  <xsl:template match="@xml:lang">
    <xsl:attribute name="lang">
      <xsl:value-of select="."/>
    </xsl:attribute>
  </xsl:template>
  <!-- Retains empty space in Span Tags -->
  <xsl:template match="xhtml:span">
    <xsl:choose>
      <xsl:when test="not(string(.))">
        <span>
          <xsl:text>&#160;</xsl:text>
        </span>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy>
          <xsl:apply-templates select="node()|@*"/>
        </xsl:copy>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- in and out variable declaration for a string replace -->
  <xsl:variable name="in">
    <xsl:text>.xhtml</xsl:text>
  </xsl:variable>
  <xsl:variable name="out">
    <xsl:text>.html</xsl:text>
  </xsl:variable>
  <!-- Replacement from .xhtml url to .html-->
  <xsl:template match="@href">
    <xsl:attribute name="href">
      <xsl:call-template name="string-replace-all">
        <xsl:with-param name="text" select="." />
        <xsl:with-param name="replace" select="$in" />
        <xsl:with-param name="by" select="$out" />
      </xsl:call-template>
    </xsl:attribute>
  </xsl:template>
  <!-- Ignore these elements -->
  <xsl:template match="@longdesc|@profile">
  </xsl:template>
  <xsl:template name="html-head">
    <head>

    </head>
  </xsl:template>

  <!-- identity templates -->
  <xsl:template match="*[not(node())]">
    <xsl:copy>
      <xsl:apply-templates select="@*"/>
      <xsl:text> </xsl:text>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="script[not(node())]">
    <xsl:copy>
      <xsl:apply-templates select="@*"/>
      <xsl:text>//</xsl:text>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="*">
    <xsl:copy>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates select="node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="area[not(node())]|base[not(node())]|
        basefont[not(node())]|bgsound[not(node())]|br[not(node())]|
        col[not(node())]|frame[not(node())]|hr[not(node())]|
        img[not(node())]|input[not(node())]|isindex[not(node())]|
        keygen[not(node())]|link[not(node())]|meta[not(node())]|
        param[not(node())]">
    <!-- identity without closing tags -->
    <xsl:copy>
      <xsl:apply-templates select="@*"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="@*|text()|comment()|processing-instruction()">
    <xsl:copy/>
  </xsl:template>
  <!-- Ignore these elements -->
  <xsl:template match="xhtml:link[@rel='schema.DCTERMS']"/>
  <xsl:template match="xhtml:link[@rel='schema.DC']"/>
</xsl:stylesheet>
