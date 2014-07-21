<xsl:stylesheet version="2.0" xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns="http://www.w3.org/1999/xhtml" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xml="http://www.w3.org/XML/1998/namespace"
    xmlns:epub="http://www.idpf.org/2007/ops"
    exclude-result-prefixes="xhtml xsl xs xml epub">
  <xsl:output method="html" encoding="utf-8"/>
  <xsl:template match="/">
    <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
    <xsl:text>&#xa;</xsl:text>
    <xsl:apply-templates></xsl:apply-templates>
  </xsl:template>
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
  </xsl:template>
  <xsl:template match="xhtml:body">
    <body id="coverbody" class="scrBody">
      <section class="cover cover-rw Cover-rw" style="text-align:center;" epub:type="cover">
        <xsl:apply-templates/>
      </section>
    </body>

  </xsl:template>
</xsl:stylesheet>
