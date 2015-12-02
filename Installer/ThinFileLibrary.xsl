<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        ThinFileLibrary.xsl
    # Purpose:     Remove nodes no longer used
    #              (I removed Files\ConfigurationTool\ from input)
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2015/12/02
    # Copyright:   (c) 2015 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    version="1.0">
    <xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>
    
    <xsl:template match="File[starts-with(@Path, 'File')]"/>
    
    <!-- Recursive copy template -->   
    <xsl:template match="* | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="text()"/>
    
</xsl:stylesheet>