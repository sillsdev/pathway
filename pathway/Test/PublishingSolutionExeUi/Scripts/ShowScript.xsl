<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ms="urn:schemas-microsoft-com:xslt">
<xsl:output method="html" encoding="UTF-8" indent="yes" />

<xsl:template match="accil">
<html>
<head>
<title>Test Script</title>
<style type="text/css">
 .indent {margin-left:2em}
 .indent2 {margin-left:4em}
 .tab3 {position:absolute; left:2in;}
</style>
</head>
<body>
<h1>Test Script</h1>
<xsl:call-template name="View" />

<xsl:for-each select="//include">
  <hr/>
  <h2><xsl:value-of select="@from"/></h2>
  <xsl:for-each select="document(@from)">
  <xsl:call-template name="View" />
  </xsl:for-each>
</xsl:for-each>

</body>
</html>
</xsl:template>

<xsl:template name="View" >
<h2>Applications</h2>
<ol>
<xsl:for-each select="//on-application">
 <li><xsl:value-of select="@title|@args|@source"/></li>
</xsl:for-each>
</ol>
<h2>Variables</h2>
<ol>
<xsl:for-each select="//var">
 <li><xsl:value-of select="@id"/> = 
   <xsl:for-each select="@set">
     <xsl:call-template name="expandText"/>
   </xsl:for-each>
 </li>
</xsl:for-each>
</ol>
<h2>Paths</h2>
<ol>
<xsl:for-each select="//@path">
 <li><xsl:call-template name="expandText"/></li>
</xsl:for-each>
</ol>

<h2>Dialogs</h2>
<ol>
<xsl:for-each select="//on-dialog">
 <li><xsl:value-of select="@title"/></li>
</xsl:for-each>
</ol>

<h2>Statistics</h2>
<table>
 <tr>
  <td>Instructions</td>
  <td>Applications</td>
  <td>Variables</td>
  <td>Paths</td>
 </tr>
 <tr>
  <td><xsl:value-of select="count(//registry|//on-desktop|//on-application|//on-startup|//on-dialog|//var|//monitor-time|//click|//hover-over|//select-text|//match-strings|//glimpse|//glimpse-extra|//if|//condition|//then|//else|//do-once|//beep|//soun|//insert)"/></td>
  <td><xsl:value-of select="count(//on-application)"/></td>
  <td><xsl:value-of select="count(//var)"/></td>
  <td><xsl:value-of select="count(//@path)"/></td>
 </tr>
</table>
</xsl:template>

<xsl:template name="expandText" ><!-- context must be a string -->
  <!-- go through the string and look for $... or $...; -->
  <xsl:choose>
  <xsl:when test="contains(.,'$')">
    <xsl:value-of select="substring-before(.,'$')"/><!-- gets the text in front of the first $ -->
  </xsl:when>
  <xsl:otherwise>
    <xsl:value-of select="."/>
  </xsl:otherwise>
  </xsl:choose>
  <xsl:variable name="tail" select="substring-after(.,'$')"/> <!-- gets the text after the first $ -->
  <xsl:if test="$tail !=''"><!-- if $tail is not empty, there's a variable reference -->
    <xsl:variable name="varName">
      <xsl:choose><!-- is there a ; in the tail?-->
      <xsl:when test="contains($tail,';')">
        <xsl:value-of select="substring-before($tail,';')"/><!-- it is the tail -->
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$tail"/><!-- it is the tail -->
      </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <a name="hi" title="{//var[@id=$varName]/@set}"><xsl:value-of select="concat('$',$varName)"/></a>
    <xsl:if test="contains($tail,';')">
      <xsl:text>;</xsl:text>
      <xsl:variable name="str" ><s><xsl:value-of select="substring-after($tail,';')"/></s></xsl:variable>
      <xsl:for-each select="ms:node-set($str)/s">
        <xsl:call-template name="expandText"/><!-- expand the rest of the $tail -->
      </xsl:for-each>
    </xsl:if>
  </xsl:if>


</xsl:template>

<xsl:template match="*" />

</xsl:stylesheet>
