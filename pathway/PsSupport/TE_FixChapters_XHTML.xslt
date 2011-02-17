<?xml version="1.0" encoding="UTF-8"?>
<!-- Fix the XHTML file exported from TE such that chapters start in a new section. -->

<xsl:transform version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml"
    xmlns="http://www.w3.org/1999/xhtml"
    xmlns:fn="http://www.w3.org/2005/xpath-functions"
    exclude-result-prefixes="xhtml">
    
    <xsl:output method="xml" version="1.0" encoding="UTF-8" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN" doctype-system="http://www.w3.org/TR/xhtml11/DTD/xhtml1-strict.dtd" indent="yes"/>

    <xsl:strip-space elements="*"/>

    <!-- Process the top element, 'html'. The children include 'head' and 'body'. -->
    <xsl:template match="xhtml:html">
		<xsl:copy>
			<!-- Do not copy the erroneous html/@lang, which should be html/@xml:lang. -->
			<xsl:for-each select="@*[not(local-name() = 'lang' )]">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:attribute name="xml:lang"><xsl:value-of select="@xml:lang"/></xsl:attribute>
			<xsl:apply-templates/>
		</xsl:copy>
    </xsl:template>    

    <xsl:template match="xhtml:head">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:copy-of select="child::*"/>
		</xsl:copy>
    </xsl:template>

	<!-- All the children of 'body' are 'div' elements with class 'scrBook'. -->
    <xsl:template match="xhtml:body">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:apply-templates/>
		</xsl:copy>
    </xsl:template>

    <xsl:template match="xhtml:body/xhtml:div">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:apply-templates/>
		</xsl:copy>
    </xsl:template>

	<!-- By default, copy the 'span' elements. -->
    <xsl:template match="xhtml:span">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:copy-of select="node()"/>
		</xsl:copy>
    </xsl:template>

	<!-- Appropriately handle chapter numbers in 'div' elements that are not section heads nor parallel passage references. -->
    <xsl:template match="xhtml:div[not(@class='Section_Head') and not(@class='Parallel_Passage_Reference')]">
		<xsl:if test="child::*[1][name()='span' and @class='Chapter_Number'] and preceding-sibling::*[1][@class='Paragraph' or @class='Paragraph_Continuation' or @class='Line1' or @class='Line2']">
			<xsl:text disable-output-escaping="yes">
                &lt;/div&gt;
                &lt;div class="scrSection"&gt;
                    </xsl:text>
		</xsl:if>
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:apply-templates/>
		</xsl:copy>
    </xsl:template>

	<!-- Default template for 'div' elements. -->
    <xsl:template match="xhtml:div">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:apply-templates/>
		</xsl:copy>
    </xsl:template>

    <xsl:template match="xhtml:img">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:copy-of select="child::*"/>
		</xsl:copy>
    </xsl:template>

	<!-- Skip the attribute 'shape' for element 'a'. -->
	<xsl:template match="xhtml:a/@shape"/> <!-- Doesn't seem to work. -->

    <!-- Special handling of text. -->
    <xsl:template match="text()">
		<!-- Replace curly quotes with straight quotes. -->
		<xsl:value-of select="translate(.,'“”','&quot;&quot;')"/>
    </xsl:template>
    
    <!-- Process the chapters. -->
    <xsl:template match="xhtml:span[@class='Chapter_Number']">
        <xsl:variable name="reportProgress" select="fn:abs(.)"/>
        <xsl:variable name="currentChapter" select="."/>
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:apply-templates/> <!-- Actually there are no children. -->
		</xsl:copy>
    </xsl:template>

	<!-- Default element and attribute templates. -->
	<xsl:template match="*">
		<xsl:comment>Warning :: The element "<xsl:value-of select="name()"/>", child of "<xsl:value-of select="name(..)"/>", has no matching template.</xsl:comment>
	</xsl:template>
	<!-- default attribute template -->
	<xsl:template match="@*">
		<xsl:comment>Warning :: The attribute "<xsl:value-of select="name()"/>" for element "<xsl:value-of select="name(..)"/>" has no matching template.</xsl:comment>
	</xsl:template>

</xsl:transform>