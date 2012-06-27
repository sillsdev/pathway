<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    
    <!-- Recursive copy template (all except xml:space attributes) -->   
    <xsl:template match="node() | @*">
        <xsl:if test="not(name() = 'xml:space')">
            <xsl:copy>
                <xsl:apply-templates select="node() | @*"/>
            </xsl:copy>
        </xsl:if>
    </xsl:template>
    
</xsl:stylesheet>