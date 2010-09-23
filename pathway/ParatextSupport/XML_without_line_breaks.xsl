<xsl:stylesheet  version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	
  <!-- XML_without_line_breaks.xsl 2010-03-25 -->

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="no" />

  <!-- Copy all attributes and nodes, and then define more specific template rules. -->
	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()" />
		</xsl:copy>
	</xsl:template>

	<!-- Remove text nodes that consist only of white space. -->
  <xsl:strip-space elements="*" />

  <!-- Remove text nodes that consist only of line break (in case strip-space leaves them). -->
  <xsl:template match="text()[.='&#xA;']" />
  <xsl:template match="text()[.='&#xD;']" />
  <xsl:template match="text()[.='&#xD;&#xA;']" />

</xsl:stylesheet>
