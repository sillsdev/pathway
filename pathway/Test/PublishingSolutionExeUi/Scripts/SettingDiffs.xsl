<?xml version="1.0" encoding="UTF-8"?>
<?xml-stylesheet type="text/xsl" href="SettingDiffs.xsl"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
<xsl:output method="html" encoding="UTF-8" indent="yes" />

<xsl:variable name="old" select="document('Settings0.xml')"/>
<xsl:variable name="new" select="document('Settings.xml')"/>

<xsl:template match="xsl:stylesheet">
<html>
<head>
<title>Setting differences</title>
</head>
<body>
 <h1>Setting differences</h1>

<xsl:variable name="changed" select="count($new/ArrayOfProperty/Property[value != $old/ArrayOfProperty/Property[name=current()/name]/value])"/>
<h2>Changed Properties <xsl:value-of select="$changed"/></h2>
<table border="1">
 <tr><th>Property</th><th>Type</th><th>Old Value</th><th>New Value</th><th>Persist</th><th>Dispose</th></tr>
 <xsl:apply-templates select="$new/ArrayOfProperty/Property" mode="changed">
   <xsl:sort select="name"/>
 </xsl:apply-templates>
</table>

<h2>Static Properties <xsl:value-of select="count($new/ArrayOfProperty/Property) - $changed"/></h2>
<table border="1">
 <tr><th>Property</th><th>Type</th><th>Old Value</th><th>New Value</th><th>Persist</th><th>Dispose</th></tr>
 <xsl:apply-templates select="$new/ArrayOfProperty/Property">
   <xsl:sort select="name"/>
 </xsl:apply-templates>
</table>

</body>
</html>
</xsl:template>

<xsl:template match="Property" mode="changed">
  <xsl:variable name="val" select="$old/ArrayOfProperty/Property[name=current()/name]/value"/>
  <xsl:if test="not($val) or $val != value">
    <xsl:call-template name="showProperty">
       <xsl:with-param name="color" select="'magenta'"/>
    </xsl:call-template>
  </xsl:if>
</xsl:template>

<xsl:template match="Property">
  <xsl:variable name="val" select="$old/ArrayOfProperty/Property[name=current()/name]/value"/>
  <xsl:if test="$val and $val = value">
    <xsl:call-template name="showProperty"/>
  </xsl:if>
</xsl:template>

<xsl:template name="showProperty">
<xsl:param name="color" select="'white'"/>
<tr>
  <td><xsl:value-of select="name"/></td>
  <td><xsl:value-of select="value/@xsi:type"/></td>
  <xsl:variable name="val" select="$old/ArrayOfProperty/Property[name=current()/name]/value"/>
  <td><xsl:value-of select="concat('&amp;nbsp;',$val)" disable-output-escaping="yes"/></td>
  <td><span style="color:{$color}"><xsl:value-of select="value"/></span></td>
  <td><xsl:value-of select="doPersist"/></td>
  <td><xsl:value-of select="doDispose"/></td>
</tr> 
</xsl:template>

<xsl:template match="text()">
<span class="para">
<xsl:value-of select="." />
</span>
</xsl:template>

</xsl:stylesheet>
