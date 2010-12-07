<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml"
    exclude-result-prefixes="xhtml"
    xmlns="http://www.w3.org/1999/xhtml">
    
    <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="yes"
        doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"
		doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"/>
    
    <!-- Copy all content that isn't explicitly processed by templates. -->
    <xsl:template match="@*|node()">
        <xsl:copy>
            <xsl:apply-templates select="@*|node()" />
        </xsl:copy>
    </xsl:template>
    
    <!-- Remove text (empty lines) at the root level. -->
    <xsl:strip-space elements="*"/>
    
    <!-- Move sections under the scrBook division -->
    <xsl:template match="xhtml:body">
        <body class="scrBody" xmlns="http://www.w3.org/1999/xhtml">
            <div class="scrBook">
                <span class="scrBookName" lang="{xhtml:div[@class='scrBook']/xhtml:span[@class='scrBookName']/@lang}">
                    <xsl:value-of select="xhtml:div[@class='scrBook']/xhtml:span[@class='scrBookName']"/>
                </span>
                <span class="scrBookCode" lang="{xhtml:div[@class='scrBook']/xhtml:span[@class='scrBookCode']/@lang}">
                    <xsl:value-of select="xhtml:div[@class='scrBook']/xhtml:span[@class='scrBookCode']"/>
                </span>
                <xsl:copy-of select="xhtml:div[@class!='scrBook']"/>
            </div>
        </body>
    </xsl:template>

</xsl:stylesheet>
