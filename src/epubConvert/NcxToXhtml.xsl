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
      	<div id="TOCPage" class="Contents">
      		<ul>
      			<xsl:apply-templates select="ncx:navMap/ncx:navPoint" />
      		</ul>
      	</div>
      </body>
    </html>
  </xsl:template>
	
	<xsl:template match="ncx:navPoint">
		<li>
			<xsl:element name="a">
				<xsl:attribute name="href">
					<xsl:value-of select="ncx:content/@src"/>
				</xsl:attribute>
				<xsl:value-of select="ncx:navLabel/ncx:text"/>
			</xsl:element>
			<xsl:if test="ncx:navPoint">
				<ul>
					<xsl:apply-templates select="ncx:navPoint"/>
				</ul>
			</xsl:if>
		</li>
	</xsl:template>

</xsl:stylesheet>
