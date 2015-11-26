<?xml version="1.0" encoding="UTF-8"?>
<!-- #############################################################
    # Name:        FileChanges.xsl
    # Purpose:     Changes old Wix Files to new ones
    #
    # Author:      Greg Trihus <greg_trihus@sil.org>
    #
    # Created:     2015/11/25
    # Copyright:   (c) 2015 SIL International
    # Licence:     <LPGL>
    ################################################################-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:wix2="http://schemas.microsoft.com/wix/2003/01/wi"
    xmlns:wix3="http://schemas.microsoft.com/wix/2006/wi"
    version="1.0">
    
    <xsl:param name="Files">file:/C:/Users/Trihus/Desktop/wixFIles/ReleaseBTE/Files.wxs</xsl:param>
    <xsl:variable name="FilesDoc" select="document($Files)"/>
    <xsl:output method="xml" indent="yes"/>
    
    <!-- Recursive copy template -->   
    <xsl:template match="*[local-name() != 'Wix' and local-name() !=  'Fragment' and local-name() != 'Directory' and local-name() != 'Component' and local-name() != 'File' and local-name() != 'Feature' and local-name() != 'ComponentRef'] | @*">
        <xsl:copy>
            <xsl:apply-templates select="node() | @*"/>
        </xsl:copy>
    </xsl:template>
    
    <xsl:template match="wix2:Wix">
        <xsl:element name="Wix" namespace="http://schemas.microsoft.com/wix/2006/wi">
            <xsl:apply-templates select="$FilesDoc/*/*"/>
            <xsl:apply-templates/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="wix2:Fragment">
        <xsl:element name="Fragment" namespace="http://schemas.microsoft.com/wix/2006/wi">
            <xsl:apply-templates select="node()"/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="wix2:Directory">
        <xsl:element name="Directory" namespace="http://schemas.microsoft.com/wix/2006/wi">
            <xsl:call-template name="ProcessAttributes"/>
            <xsl:apply-templates select="node()"/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="wix2:Component">
        <xsl:choose>
            <xsl:when test="contains(.//@Source,'Wordpress')"/>
            <xsl:when test="contains(.//@Source,'WordPress')"/>
            <xsl:when test="contains(.//@Source,'L10NSharp.pdb')"/>
            <xsl:when test="contains(.//@Source,'MySql.Data.dll')"/>
            <xsl:otherwise>
                <xsl:element name="Component" namespace="http://schemas.microsoft.com/wix/2006/wi">
                    <xsl:call-template name="ProcessAttributes"/>
                    <xsl:apply-templates select="node()"/>
                </xsl:element>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template match="wix2:File">
        <xsl:element name="File" namespace="http://schemas.microsoft.com/wix/2006/wi">
            <xsl:call-template name="ProcessAttributes"/>
            <xsl:apply-templates select="node()"/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="wix2:Feature">
        <xsl:element name="ComponentGroup" namespace="http://schemas.microsoft.com/wix/2006/wi">
            <xsl:attribute name="Id">Application</xsl:attribute>
            <xsl:element name="ComponentRef" namespace="http://schemas.microsoft.com/wix/2006/wi">
                <xsl:attribute name="Id">AddShortcutApp</xsl:attribute>
            </xsl:element>
            <xsl:element name="ComponentRef" namespace="http://schemas.microsoft.com/wix/2006/wi">
                <xsl:attribute name="Id">RegistryEntries</xsl:attribute>
            </xsl:element>
            <xsl:apply-templates select="node()"/>
        </xsl:element>
    </xsl:template>
    
    <xsl:template match="wix2:ComponentRef">
        <xsl:variable name="Id" select="@Id"/>
        <xsl:variable name="Component" select="$FilesDoc//*[local-name()='Component'][@Id=$Id]"/>
        <xsl:if test="count($Component) != 0">
            <xsl:choose>
                <xsl:when test="contains($Component//@Source, 'Wordpress')"/>
                <xsl:when test="contains($Component//@Source, 'WordPress')"/>
                <xsl:when test="contains($Component//@Source, 'L10NSharp.pdb')"/>
                <xsl:when test="contains($Component//@Source, 'MySql.Data.dll')"/>
                <xsl:otherwise>
                    <xsl:element name="ComponentRef" namespace="http://schemas.microsoft.com/wix/2006/wi">
                        <xsl:apply-templates select="node() |@*"/>
                    </xsl:element>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:if>
    </xsl:template>
    
    <xsl:template name="ProcessAttributes">
        <xsl:for-each select="@*">
            <xsl:choose>
                <xsl:when test=". = 'INSTALLDIR'">
                    <xsl:attribute name="Id">APPLICATIONFOLDER</xsl:attribute>
                </xsl:when>
                <xsl:when test=". = 'Pathway7'">
                    <xsl:attribute name="Name">Pathway</xsl:attribute>
                </xsl:when>
                <xsl:when test="local-name() = 'SourceName'"/>
                <xsl:when test="local-name() = 'LongSource'"/>
                <xsl:when test="local-name() = 'LongName'"/>
                <xsl:when test="local-name() = 'Name'">
                    <xsl:attribute name="Name">
                        <xsl:choose>
                            <xsl:when test="count(parent::*/@LongName) = 1">
                                <xsl:value-of select="parent::*/@LongName"/>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="."/>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:attribute>
                </xsl:when>
                <xsl:when test="local-name() = 'Source'">
                    <xsl:attribute name="Source">
                        <xsl:choose>
                            <xsl:when test="starts-with(.,'..\Files\ConfigurationTool\')">
                                <xsl:value-of select="concat('..\output\Release\',substring-after(., '..\Files\ConfigurationTool\'))"/>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="."/>
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:copy/>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:for-each>
    </xsl:template>
    
    <xsl:template match="text()"/>
</xsl:stylesheet>