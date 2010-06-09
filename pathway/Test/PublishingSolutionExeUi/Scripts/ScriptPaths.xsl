<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="html" encoding="UTF-8" indent="yes" />

<xsl:template match="accil|include[not(parent::*)]">
<html>
<head>
<title>Script Paths to Variables</title>
</head>
<body>
 <h1>Script Paths to Variables</h1>

 <p>Included scripts</p>
 <xsl:apply-templates select=".//include">
   <xsl:sort select="@from"/>
 </xsl:apply-templates>

 <p><span style="color:green">Variables already defined</span> and <span style="color:red">not used or implicit ($fld; $ind; $row; $col;)</span></p>
 <xsl:apply-templates select="//var">
   <xsl:sort  select="@id"/>
 </xsl:apply-templates>

 <p>Possible variable definitions</p>
 <xsl:apply-templates select="//*" mode="possible">
   <xsl:sort select="@select"/>
 </xsl:apply-templates>

 <p>Instructions with "until" attribute</p>
 <xsl:apply-templates select="//*[@until]" mode="list">
   <xsl:sort  select="@wait" data-type="number"/>
 </xsl:apply-templates>

 <p>Instructions with "wait" attribute</p>
 <xsl:apply-templates select="//*[@wait]" mode="list">
   <xsl:sort  select="@wait" data-type="number"/>
 </xsl:apply-templates>

</body>
</html>
</xsl:template>

<xsl:template match="var">
 <xsl:variable name="ID" select="@id"/>
 <xsl:variable name="refs" select="count(//insert[contains(.,concat('$',$ID))]) + count(//@*[contains(.,concat('$',$ID))])"/>
<div>
 <xsl:value-of select="$refs"/><xsl:text> </xsl:text>
 <xsl:choose>
 <xsl:when test="$refs = '0'">
    <span style="color:red"><xsl:call-template name="varSig"/></span>
 </xsl:when>
 <xsl:otherwise>
    <span style="color:green"><xsl:call-template name="varSig"/></span>
 </xsl:otherwise>
 </xsl:choose>
</div>
</xsl:template>

<xsl:template name="varSig">
 <xsl:text>&lt;var id="</xsl:text><xsl:value-of select="@id"/>
 <xsl:if test="@set">
     <xsl:text>" set="</xsl:text><xsl:value-of select="@set"/>
 </xsl:if>
 <xsl:if test="@select">
     <xsl:text>" select="</xsl:text><xsl:value-of select="@select"/>
 </xsl:if>
 <xsl:if test="@add">
     <xsl:text>" add="</xsl:text><xsl:value-of select="@add"/>
 </xsl:if>
  <xsl:text>"/></xsl:text>
</xsl:template>

<xsl:template match="*" mode="possible">
<xsl:choose>
<xsl:when test="@select">
   <div>
      <xsl:text>&lt;var id="TBD" select="</xsl:text>
      <xsl:value-of select="@select"/>
      <xsl:text>"</xsl:text>
      <xsl:if test="@path">
         <xsl:text> add="</xsl:text>
         <xsl:value-of select="@path"/>
         <xsl:text>"</xsl:text>
      </xsl:if>
      <xsl:text>/></xsl:text>
   </div>
</xsl:when>
<xsl:when test="@path">
   <div>
      <xsl:text>&lt;var id="TBD" set="</xsl:text>
      <xsl:value-of select="@path"/>
      <xsl:text>"/></xsl:text>
   </div>
</xsl:when>
<xsl:otherwise>
</xsl:otherwise>
</xsl:choose>
</xsl:template>

<xsl:template match="include">
<div>
 <xsl:text>&lt;include from="</xsl:text>
 <a href="{@from}"><xsl:value-of select="@from"/></a>
 <xsl:text>" /></xsl:text>
</div>
</xsl:template>

<xsl:template match="*" mode="list">
<div>
 <xsl:text>&lt;</xsl:text><xsl:value-of select="name()"/>
 <xsl:apply-templates select="@*" mode="list"/>
 <xsl:choose>
 <xsl:when test="name()='insert'">
   <xsl:text>&gt;</xsl:text>
   <xsl:apply-templates select="."/>
   <xsl:text>&lt;/</xsl:text><xsl:value-of select="name()"/><xsl:text>&gt;</xsl:text>
 </xsl:when>
 <xsl:otherwise>
   <xsl:text>" /></xsl:text>
 </xsl:otherwise>
 </xsl:choose>
</div>
</xsl:template>

<xsl:template match="@*" mode="list">
 <xsl:text> </xsl:text>
 <xsl:value-of select="name()"/>
 <xsl:text>="</xsl:text>
 <xsl:value-of select="."/>
 <xsl:text>"</xsl:text>
</xsl:template>

</xsl:stylesheet>
