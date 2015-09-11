<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="yes"
    doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"/>
  <xsl:param name="xmlLang"/>
  <xsl:param name="ws" select="'es'"/>
  <xsl:param name="li" select="'font-family:12pt;'"/>
  <!-- The templates matching * and @* match and copy unhandled elements/attributes. -->
  <xsl:template match="*">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="@*">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="/">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="usx|usfm|USX">
    <osis xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://www.bibletechnologies.net/2003/OSIS/namespace" xsi:schemaLocation="http://www.bibletechnologies.net/2003/OSIS/namespace file:../osisCore.2.0_UBS_SIL_BestPractice.xsd">
      <osisText osisIDWork="thisWork" osisRefWork="bible" xml:lang="eng">
        <header>
          <work osisWork="thisWork" />
        </header>
        <xsl:apply-templates/>
      </osisText>
    </osis>
  </xsl:template>

  <!-- Define the book. -->
  <xsl:template match="book">
    <xsl:variable name="bookCode">
      <xsl:choose>
        <xsl:when test="@code">
          <xsl:value-of select="@code"/>
        </xsl:when>
        <xsl:when test="@id">
          <!-- Support old format for USX -->
          <xsl:value-of select="@id"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="bookInToc" select="normalize-space(para[@style='toc2'])"/>
    <xsl:variable name="bookHeading" select="normalize-space(para[@style='h'])"/>
    <xsl:variable name="bookTitle" select="normalize-space(para[@style='mt'])"/>
    <xsl:variable name="bookTitle1" select="normalize-space(para[@style='mt1'])"/>
     <div type="book" osisID="{$bookCode}">
        <xsl:apply-templates/>
     </div>    
  </xsl:template>
  <xsl:template match="chapter">
    <xsl:variable name="BookID">
      <xsl:value-of select="ancestor::book[1]/@code"/>
    </xsl:variable>
    <xsl:variable name="chapternumber">
      <xsl:value-of select="@number"/>
    </xsl:variable>
    <xsl:element name="chapter">
      <xsl:attribute name="n">
        <xsl:value-of select="$chapternumber"/>
      </xsl:attribute>
      <xsl:attribute name="osisID">
        <xsl:value-of select="concat($BookID,'.',$chapternumber)"/>
      </xsl:attribute>
      <xsl:attribute name="sID">
        <xsl:value-of select="concat($BookID,'.',$chapternumber)"/>
      </xsl:attribute>
    </xsl:element>
  </xsl:template>
  <xsl:template match="para">
    <xsl:choose>
      <xsl:when test="@style='mt' or @style='mt1'">
        <xsl:element name="title">
          <xsl:attribute name="type">
            <xsl:text>main</xsl:text>
          </xsl:attribute>
          <xsl:element name="title">
            <xsl:attribute name="level">
              <xsl:text>1</xsl:text>
            </xsl:attribute>
            <xsl:apply-templates/>
          </xsl:element>
        </xsl:element>
      </xsl:when>
      <xsl:when test="@style='rem'">
        <xsl:comment>
          <xsl:text>\</xsl:text>
          <xsl:value-of select="@style"/>
          <xsl:text> </xsl:text>
          <xsl:copy-of select="text()"/>
        </xsl:comment>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="p">
          <xsl:variable name="BookID">
            <xsl:value-of select="ancestor::book[1]/@code"/>
          </xsl:variable>
          <xsl:variable name="chapternumber">
            <xsl:value-of select="preceding-sibling::chapter[1]/@number"/>
          </xsl:variable>
          <xsl:for-each select="@*|node()|text()">
            <xsl:choose>
              <xsl:when test="local-name()='verse'">
                <xsl:if test="local-name(preceding-sibling::*)='verse'">
                  <xsl:element name="verse">
                    <xsl:attribute name="eID">
                      <xsl:value-of
                        select="concat($BookID,'.',$chapternumber,'.',preceding-sibling::verse[1]/@number)"
                      />
                    </xsl:attribute>
                  </xsl:element>
                </xsl:if>
                <xsl:variable name="versenumber">
                  <xsl:value-of select="@number"/>
                </xsl:variable>
                <xsl:element name="verse">
                  <xsl:attribute name="n">
                    <xsl:value-of select="$versenumber"/>
                  </xsl:attribute>
                  <xsl:attribute name="sID">
                    <xsl:value-of select="concat($BookID,'.',$chapternumber,'.',$versenumber)"/>
                  </xsl:attribute>
                  <xsl:attribute name="osisID">
                    <xsl:value-of select="concat($BookID,'.',$chapternumber,'.',$versenumber)"/>
                  </xsl:attribute>
                  <xsl:attribute name="subType">
                    <xsl:if test="$versenumber = '1'">
                      <xsl:text>x-first</xsl:text>  
                    </xsl:if>
                    <xsl:if test="$versenumber != '1'">
                      <xsl:text>x-embedded</xsl:text>  
                    </xsl:if>                    
                  </xsl:attribute>
                </xsl:element>
              </xsl:when>
              <xsl:otherwise>
                <xsl:apply-templates select="current()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each>
          <xsl:if test="child::*[local-name()='verse']">
            <!--<xsl:text> true</xsl:text>-->
            <xsl:element name="verse">
              <xsl:attribute name="eID">
                <xsl:value-of
                  select="concat($BookID,'.',$chapternumber,'.',descendant::verse[last()]/@number)"
                />
              </xsl:attribute>              
            </xsl:element>
          </xsl:if>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="para[@style='h']">
    <xsl:element name="title">
      <xsl:attribute name="short">
        <xsl:value-of select="text()"/>
      </xsl:attribute>
    </xsl:element>
  </xsl:template>
  <xsl:template match="para[@style='li1']">
    <div>
      <list>
        <item style="{$li}">
          <xsl:variable name="BookID">
            <xsl:value-of select="ancestor::book[1]/@code"/>
          </xsl:variable>
          <xsl:variable name="chapternumber">
            <xsl:value-of select="preceding-sibling::chapter[1]/@number"/>
          </xsl:variable>
          <xsl:for-each select="@*|node()|text()">
            <xsl:choose>
              <xsl:when test="local-name()='verse'">
                <xsl:if test="local-name(preceding-sibling::*)='verse'">
                  <xsl:element name="verse">
                    <xsl:attribute name="eID">
                      <xsl:value-of
                        select="concat($BookID,'.',$chapternumber,'.',preceding-sibling::verse[1]/@number)"
                      />
                    </xsl:attribute>
                  </xsl:element>
                </xsl:if>
                <xsl:variable name="versenumber">
                  <xsl:value-of select="@number"/>
                </xsl:variable>
                <xsl:element name="verse">
                  <xsl:attribute name="n">
                    <xsl:value-of select="$versenumber"/>
                  </xsl:attribute>
                  <xsl:attribute name="sID">
                    <xsl:value-of select="concat($BookID,'.',$chapternumber,'.',$versenumber)"/>
                  </xsl:attribute>
                  <xsl:attribute name="osisID">
                    <xsl:value-of select="concat($BookID,'.',$chapternumber,'.',$versenumber)"/>
                  </xsl:attribute>
                </xsl:element>
              </xsl:when>
              <xsl:otherwise>
                <xsl:apply-templates select="current()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each>
          <xsl:if test="child::*[local-name()='verse']">
            <!--<xsl:text> true</xsl:text>-->
            <xsl:element name="verse">
              <xsl:attribute name="eID">
                <xsl:value-of
                  select="concat($BookID,'.',$chapternumber,'.',descendant::verse[last()]/@number)"
                />
              </xsl:attribute>
            </xsl:element>
          </xsl:if>
          <xsl:if test="following-sibling::*[1]/@style='li2'">
            <xsl:apply-templates select="following-sibling::*[1]" mode="listitem"/>
          </xsl:if>
        </item>
      </list>
    </div>
  </xsl:template>
  <xsl:template match="para[@style='li2']" mode="listitem">
    <list>
      <item>
        <xsl:variable name="BookID">
          <xsl:value-of select="ancestor::book[1]/@code"/>
        </xsl:variable>
        <xsl:variable name="chapternumber">
          <xsl:value-of select="preceding-sibling::chapter[1]/@number"/>
        </xsl:variable>
        <xsl:for-each select="@*|node()|text()">
          <xsl:choose>
            <xsl:when test="local-name()='verse'">
              <xsl:if test="local-name(preceding-sibling::*)='verse'">
                <xsl:element name="verse">
                  <xsl:attribute name="eID">
                    <xsl:value-of
                      select="concat($BookID,'.',$chapternumber,'.',preceding-sibling::verse[1]/@number)"
                    />
                  </xsl:attribute>
                </xsl:element>
              </xsl:if>
              <xsl:variable name="versenumber">
                <xsl:value-of select="@number"/>
              </xsl:variable>
              <xsl:element name="verse">
                <xsl:attribute name="n">
                  <xsl:value-of select="$versenumber"/>
                </xsl:attribute>
                <xsl:attribute name="sID">
                  <xsl:value-of select="concat($BookID,'.',$chapternumber,'.',$versenumber)"/>
                </xsl:attribute>
                <xsl:attribute name="osisID">
                  <xsl:value-of select="concat($BookID,'.',$chapternumber,'.',$versenumber)"/>
                </xsl:attribute>
              </xsl:element>
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="current()"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:for-each>
        <xsl:if test="child::*[local-name()='verse']">
          <!--<xsl:text> true</xsl:text>-->
          <xsl:element name="verse">
            <xsl:attribute name="eID">
              <xsl:value-of
                select="concat($BookID,'.',$chapternumber,'.',descendant::verse[last()]/@number)"/>
            </xsl:attribute>
          </xsl:element>
        </xsl:if>
        <xsl:if test="following-sibling::*[1]/@style='li3'">
          <xsl:apply-templates select="following-sibling::*[1]" mode="listitem"/>
        </xsl:if>
      </item>
    </list>
  </xsl:template>
  <xsl:template match="para[@style='li3']" mode="listitem">
    <list>
      <item>
        <xsl:variable name="BookID">
          <xsl:value-of select="ancestor::book[1]/@code"/>
        </xsl:variable>
        <xsl:variable name="chapternumber">
          <xsl:value-of select="preceding-sibling::chapter[1]/@number"/>
        </xsl:variable>
        <xsl:for-each select="@*|node()|text()">
          <xsl:choose>
            <xsl:when test="local-name()='verse'">
              <xsl:if test="local-name(preceding-sibling::*)='verse'">
                <xsl:element name="verse">
                  <xsl:attribute name="eID">
                    <xsl:value-of
                      select="concat($BookID,'.',$chapternumber,'.',preceding-sibling::verse[1]/@number)"
                    />
                  </xsl:attribute>
                </xsl:element>
              </xsl:if>
              <xsl:variable name="versenumber">
                <xsl:value-of select="@number"/>
              </xsl:variable>
              <xsl:element name="verse">
                <xsl:attribute name="n">
                  <xsl:value-of select="$versenumber"/>
                </xsl:attribute>
                <xsl:attribute name="sID">
                  <xsl:value-of select="concat($BookID,'.',$chapternumber,'.',$versenumber)"/>
                </xsl:attribute>
                <xsl:attribute name="osisID">
                  <xsl:value-of select="concat($BookID,'.',$chapternumber,'.',$versenumber)"/>
                </xsl:attribute>
              </xsl:element>
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="current()"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:for-each>
        <xsl:if test="child::*[local-name()='verse']">
          <xsl:element name="verse">
            <xsl:attribute name="eID">
              <xsl:value-of
                select="concat($BookID,'.',$chapternumber,'.',descendant::verse[last()]/@number)"/>
            </xsl:attribute>
          </xsl:element>
        </xsl:if>
      </item>
    </list>
  </xsl:template>
  <xsl:template match="para[@style='r']">
    <p>
      <title level="2">
        <hi type="italic">
          <xsl:apply-templates/>
        </hi>
      </title>
    </p>
  </xsl:template>
  <xsl:template match="para[@style='mt1']|para[@style='ms1']">
    <title type="main">
        <title level="2">
          <hi type="bold">
            <xsl:apply-templates/>
          </hi>
        </title>
      </title>
  </xsl:template>
  <xsl:template match="para[@style='mt2']">
    <p>
      <title level="4" placement="centerHead">        
          <hi type="bold">
            <xsl:apply-templates/>
          </hi>        
      </title>
    </p>
  </xsl:template>
  <xsl:template match="para[@style='s1']">
    <div type="section">
      <title>
        <hi type="bold">
          <xsl:apply-templates/>
        </hi>
      </title>
    </div>
  </xsl:template>
  <xsl:template match="para[@style='s2']">
    <div type="subSection">
      <title>
        <xsl:apply-templates/>
      </title>
    </div>
  </xsl:template>
  <xsl:template match="para[@style='nd']">
    <seg>
      <divineName>
        <xsl:apply-templates/>
      </divineName>
    </seg>
  </xsl:template>
  <xsl:template match="para[@style='d']">
    <div align="center">
      <hi type="italic">
        <xsl:apply-templates/>
      </hi>
    </div>
  </xsl:template>
  <!-- fq -->
  <xsl:template match="char[@style='fq' and @closed='false']">
    <hi type="italic">
      <xsl:apply-templates/>
    </hi>
  </xsl:template>
  <!-- qs -->
  <xsl:template match="char[@style='qs' and @closed='false']">
    <hi type="italic">
      <xsl:apply-templates/>
    </hi>
  </xsl:template>  
  <!-- r -->
  <xsl:template match="para[@style='r']">
    <title level="2">
      <hi type="italic">
        <xsl:apply-templates/>
      </hi>
    </title>
  </xsl:template>
  
  <!-- pi -->
  <xsl:template match="para[@style='pi']">
    <list>
      <item>
        <p>
          <xsl:apply-templates/>
        </p>
      </item>
    </list>
  </xsl:template>  
  <!-- nd -->
  <xsl:template match="char[@style='nd']">
    <seg>
      <divineName>
        <xsl:apply-templates/>
      </divineName>
    </seg>
  </xsl:template>
  
  <!-- b -->
  <xsl:template match="para[@style='b']">
    <br/>
  </xsl:template>
  <!-- ide -->
  <xsl:template match="para[@style='ide']"> </xsl:template>

  <!-- q -->
  <xsl:template match="para[@style='q']">
    <lg>
      <l level="1">
        <xsl:apply-templates/>
      </l>
    </lg>
  </xsl:template>  
  <xsl:template match="para[@style='mr']">
    <div type="majorSection">
      <title type="scope">
        <reference>
          <xsl:apply-templates/>
        </reference>
      </title>
    </div>
  </xsl:template>
  <xsl:template match="para[@style='li2']"/>
  <xsl:template match="para[@style='li3']"/>
  <xsl:template match="para[@style='toc1' or @style='toc2' or @style='toc3']"/>
</xsl:stylesheet>
