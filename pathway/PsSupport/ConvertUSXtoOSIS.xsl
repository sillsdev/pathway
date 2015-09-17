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
    <osis xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      xmlns="http://www.bibletechnologies.net/2003/OSIS/namespace"
      xsi:schemaLocation="http://www.bibletechnologies.net/2003/OSIS/namespace file:../osisCore.2.0_UBS_SIL_BestPractice.xsd">
      <osisText osisIDWork="thisWork" osisRefWork="bible" xml:lang="eng">
        <header>
          <work osisWork="thisWork">
            <scope>
              <xsl:value-of
                select="concat(descendant::book[1]/@code,'.',descendant::chapter[1]/@number,'.',descendant::verse[1]/@number)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of
                select="concat(descendant::book[last()]/@code,'.',descendant::chapter[last()]/@number,'.',descendant::verse[last()]/@number)"/>
            </scope>
          </work>
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
      <xsl:if test="descendant::para[@style='s2']">
        <xsl:value-of
          disable-output-escaping="yes"
          select="concat('&lt;','/div','&gt;')" />
      </xsl:if>
      <xsl:if test="descendant::para[@style='s1']">
        <xsl:value-of
          disable-output-escaping="yes"
          select="concat('&lt;','/div','&gt;','&lt;','/div','&gt;')" />
      </xsl:if>
      <xsl:if test="local-name(descendant::chapter[last()])='chapter'">
        <xsl:element name="chapter">
          <xsl:attribute name="eID">
            <xsl:value-of select="concat($bookCode,'.',descendant::chapter[last()]/@number)"/>
          </xsl:attribute>
        </xsl:element>
      </xsl:if>
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
      <xsl:when test="@style='mt' or @style='mt1' or @style='ms1'">
        <xsl:element name="title">
          <xsl:attribute name="type">
            <xsl:text>main</xsl:text>
          </xsl:attribute>
          <xsl:element name="title">
            <xsl:attribute name="level">
              <xsl:text>1</xsl:text>
            </xsl:attribute>
            <xsl:element name="hi">
              <xsl:attribute name="type">
                <xsl:text>bold</xsl:text>
              </xsl:attribute>
            <xsl:apply-templates/>
            </xsl:element>
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
          <xsl:for-each select="node()|text()">
            <xsl:choose>
              <xsl:when test="local-name()='verse'">
                <xsl:if test="local-name(preceding-sibling::*)='verse'">
                  <xsl:call-template name="VerseTemplate">
                    <xsl:with-param name="versenumber" select="preceding-sibling::verse[1]/@number"/>
                    <xsl:with-param name="booknumber" select="$BookID"/>
                    <xsl:with-param name="chapternumber" select="$chapternumber"/>
                    <xsl:with-param name="IDTypeattribute">
                      <xsl:text>eID</xsl:text>
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:if>
                <xsl:call-template name="VerseTemplate">
                  <xsl:with-param name="versenumber" select="@number"/>
                  <xsl:with-param name="booknumber" select="$BookID"/>
                  <xsl:with-param name="chapternumber" select="$chapternumber"/>
                  <xsl:with-param name="IDTypeattribute">
                    <xsl:text>sID</xsl:text>
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:when>
              <xsl:otherwise>
                <xsl:apply-templates select="current()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each>
          <xsl:if test="child::*[local-name()='verse']">
            <xsl:call-template name="VerseTemplate">
              <xsl:with-param name="versenumber" select="descendant::verse[last()]/@number"/>
              <xsl:with-param name="booknumber" select="$BookID"/>
              <xsl:with-param name="chapternumber" select="$chapternumber"/>
              <xsl:with-param name="IDTypeattribute">
                <xsl:text>eID</xsl:text>
              </xsl:with-param>
            </xsl:call-template>
          </xsl:if>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="VerseTemplate">
    <xsl:param name="versenumber"/>
    <xsl:param name="chapternumber"/>
    <xsl:param name="booknumber"/>
    <xsl:param name="IDTypeattribute"/>
    <xsl:param name="IDValue">
      <xsl:choose>
        <xsl:when test="contains($versenumber,'-')">
          <xsl:value-of
            select="concat($booknumber,'.',$chapternumber,'.',substring-before($versenumber,'-'))"/>
          <xsl:text>-</xsl:text>
          <xsl:value-of
            select="concat($booknumber,'.',$chapternumber,'.',substring-after($versenumber,'-'))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="concat($booknumber,'.',$chapternumber,'.',$versenumber)"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:param>
    <xsl:element name="verse">
      <xsl:attribute name="{$IDTypeattribute}">
        <xsl:value-of select="$IDValue"/>
      </xsl:attribute>
      <xsl:if test="$IDTypeattribute='sID'">
        <xsl:attribute name="osisID">
          <xsl:value-of select="translate($IDValue,'-',' ')"/>
        </xsl:attribute>
        <xsl:attribute name="subType">
          <xsl:if test="$versenumber = '1'">
            <xsl:text>x-first</xsl:text>
          </xsl:if>
          <xsl:if test="$versenumber != '1'">
            <xsl:text>x-embedded</xsl:text>
          </xsl:if>
        </xsl:attribute>
        <xsl:attribute name="n">
          <xsl:value-of select="$versenumber"/>
        </xsl:attribute>
      </xsl:if>
    </xsl:element>
  </xsl:template>
  <xsl:template match="para[@style='h']">
    <xsl:element name="title">
      <xsl:attribute name="short">
        <xsl:value-of select="text()"/>
      </xsl:attribute>
    </xsl:element>
  </xsl:template>
  <xsl:template match="para[@style='li1' or @style='pi' or @style='pi1']">
    <div>
      <list>
        <item style="{$li}">
          <xsl:if test="@style='pi' or @style='pi1'">
            <xsl:value-of 
              disable-output-escaping="yes"
              select="concat('&lt;p','&gt;')" />
          </xsl:if>
          <xsl:variable name="BookID">
            <xsl:value-of select="ancestor::book[1]/@code"/>
          </xsl:variable>
          <xsl:variable name="chapternumber">
            <xsl:value-of select="preceding-sibling::chapter[1]/@number"/>
          </xsl:variable>
          <xsl:for-each select="node()|text()">
            <xsl:choose>
              <xsl:when test="local-name()='verse'">
                <xsl:if test="local-name(preceding-sibling::*)='verse'">
                  <xsl:call-template name="VerseTemplate">
                    <xsl:with-param name="versenumber" select="preceding-sibling::verse[1]/@number"/>
                    <xsl:with-param name="booknumber" select="$BookID"/>
                    <xsl:with-param name="chapternumber" select="$chapternumber"/>
                    <xsl:with-param name="IDTypeattribute">
                      <xsl:text>eID</xsl:text>
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:if>
                <xsl:call-template name="VerseTemplate">
                  <xsl:with-param name="versenumber" select="@number"/>
                  <xsl:with-param name="booknumber" select="$BookID"/>
                  <xsl:with-param name="chapternumber" select="$chapternumber"/>
                  <xsl:with-param name="IDTypeattribute">
                    <xsl:text>sID</xsl:text>
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:when>
              <xsl:otherwise>
                <xsl:apply-templates select="current()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each>
          <xsl:if test="child::*[local-name()='verse']">
            <xsl:call-template name="VerseTemplate">
              <xsl:with-param name="versenumber" select="descendant::verse[last()]/@number"/>
              <xsl:with-param name="booknumber" select="$BookID"/>
              <xsl:with-param name="chapternumber" select="$chapternumber"/>
              <xsl:with-param name="IDTypeattribute">
                <xsl:text>eID</xsl:text>
              </xsl:with-param>
            </xsl:call-template>
          </xsl:if>
          <xsl:if test="following-sibling::*[1]/@style='li2' or following-sibling::*[1]/@style='pi2'">
            <xsl:apply-templates select="following-sibling::*[1]" mode="listitem"/>
          </xsl:if>
          <xsl:if test="@style='pi' or @style='pi1'">
            <xsl:value-of 
              disable-output-escaping="yes"
              select="concat('&lt;/p','&gt;')" />
          </xsl:if>
        </item>
      </list>
    </div>
  </xsl:template>
  <xsl:template match="para[@style='li2' or style='pi2']" mode="listitem">
    <list>
      <item>
        <xsl:if test="@style='pi2'">
          <xsl:value-of 
            disable-output-escaping="yes"
            select="concat('&lt;p','&gt;')" />
        </xsl:if>
        <xsl:variable name="BookID">
          <xsl:value-of select="ancestor::book[1]/@code"/>
        </xsl:variable>
        <xsl:variable name="chapternumber">
          <xsl:value-of select="preceding-sibling::chapter[1]/@number"/>
        </xsl:variable>
        <xsl:for-each select="node()|text()">
          <xsl:choose>
            <xsl:when test="local-name()='verse'">
              <xsl:if test="local-name(preceding-sibling::*)='verse'">
                <xsl:call-template name="VerseTemplate">
                  <xsl:with-param name="versenumber" select="preceding-sibling::verse[1]/@number"/>
                  <xsl:with-param name="booknumber" select="$BookID"/>
                  <xsl:with-param name="chapternumber" select="$chapternumber"/>
                  <xsl:with-param name="IDTypeattribute">
                    <xsl:text>eID</xsl:text>
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:if>
              <xsl:call-template name="VerseTemplate">
                <xsl:with-param name="versenumber" select="@number"/>
                <xsl:with-param name="booknumber" select="$BookID"/>
                <xsl:with-param name="chapternumber" select="$chapternumber"/>
                <xsl:with-param name="IDTypeattribute">
                  <xsl:text>sID</xsl:text>
                </xsl:with-param>
              </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="current()"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:for-each>
        <xsl:if test="child::*[local-name()='verse']">
          <xsl:call-template name="VerseTemplate">
            <xsl:with-param name="versenumber" select="descendant::verse[last()]/@number"/>
            <xsl:with-param name="booknumber" select="$BookID"/>
            <xsl:with-param name="chapternumber" select="$chapternumber"/>
            <xsl:with-param name="IDTypeattribute">
              <xsl:text>eID</xsl:text>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:if>
        <xsl:if test="following-sibling::*[1]/@style='li3' or following-sibling::*[1]/@style='pi3'">
          <xsl:apply-templates select="following-sibling::*[1]" mode="listitem"/>
        </xsl:if>
        <xsl:if test="@style='pi2'">
          <xsl:value-of 
            disable-output-escaping="yes"
            select="concat('&lt;/p','&gt;')" />
        </xsl:if>
      </item>
    </list>
  </xsl:template>
  <xsl:template match="para[@style='li3' or @style='pi3']" mode="listitem">
    <list>
      <item>
        <xsl:if test="@style='pi3'">
          <xsl:value-of 
            disable-output-escaping="yes"
            select="concat('&lt;p','&gt;')" />
        </xsl:if>
        <xsl:variable name="BookID">
          <xsl:value-of select="ancestor::book[1]/@code"/>
        </xsl:variable>
        <xsl:variable name="chapternumber">
          <xsl:value-of select="preceding-sibling::chapter[1]/@number"/>
        </xsl:variable>
        <xsl:for-each select="node()|text()">
          <xsl:choose>
            <xsl:when test="local-name()='verse'">
              <xsl:if test="local-name(preceding-sibling::*)='verse'">
                <xsl:call-template name="VerseTemplate">
                  <xsl:with-param name="versenumber" select="preceding-sibling::verse[1]/@number"/>
                  <xsl:with-param name="booknumber" select="$BookID"/>
                  <xsl:with-param name="chapternumber" select="$chapternumber"/>
                  <xsl:with-param name="IDTypeattribute">
                    <xsl:text>eID</xsl:text>
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:if>
              <xsl:call-template name="VerseTemplate">
                <xsl:with-param name="versenumber" select="@number"/>
                <xsl:with-param name="booknumber" select="$BookID"/>
                <xsl:with-param name="chapternumber" select="$chapternumber"/>
                <xsl:with-param name="IDTypeattribute">
                  <xsl:text>sID</xsl:text>
                </xsl:with-param>
              </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="current()"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:for-each>
        <xsl:if test="child::*[local-name()='verse']">
          <xsl:call-template name="VerseTemplate">
            <xsl:with-param name="versenumber" select="descendant::verse[last()]/@number"/>
            <xsl:with-param name="booknumber" select="$BookID"/>
            <xsl:with-param name="chapternumber" select="$chapternumber"/>
            <xsl:with-param name="IDTypeattribute">
              <xsl:text>eID</xsl:text>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:if>
        <xsl:if test="@style='pi3'">
          <xsl:value-of 
            disable-output-escaping="yes"
            select="concat('&lt;/p','&gt;')" />
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
  <xsl:template match="para[@style='mt2']">
      <title level="2">
        <hi type="bold">
          <xsl:apply-templates/>
        </hi>
      </title>
  </xsl:template>
  <xsl:template match="para[@style='s1']">
    <xsl:if test="preceding::para[@style='s1']">
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','/div','&gt;','&lt;','/div','&gt;')" />
    </xsl:if>
    <xsl:value-of
      disable-output-escaping="yes"
      select="concat('&lt;div type=','&quot;','majorSection','&quot; ','&gt;','&lt;div type=','&quot;','section','&quot; ','annotateType=','&quot;commentary&quot;','&gt;')" />
    <title placement="centerHead">
      <hi type="bold">
        <xsl:apply-templates/>
      </hi>
    </title>
  </xsl:template>

  <xsl:template match="para[@style='s2']">
    <xsl:if test="preceding::para[@style='s2']">
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','/div','&gt;')" />
    </xsl:if>
    <xsl:value-of
      disable-output-escaping="yes"
      select="concat('&lt;div type=','&quot;','subSection','&quot;','&gt;')" />

      <hi type="italic">
      <xsl:apply-templates/>
      </hi>

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
        <xsl:if test="following-sibling::*[1]/@style='q2'">
          <xsl:apply-templates select="following-sibling::*[1]" mode="q"/>
        </xsl:if>
      </l>
    </lg>
  </xsl:template>

  <!-- q2 -->
  <xsl:template match="para[@style='q2']"  mode="q">
    <lg>
      <l level="2">
        <xsl:apply-templates/>
        <xsl:if test="following-sibling::*[1]/@style='q3'">
          <xsl:apply-templates select="following-sibling::*[1]" mode="q"/>
        </xsl:if>
      </l>
    </lg>
  </xsl:template>

  <!-- q3 -->
  <xsl:template match="para[@style='q3']"  mode="q">
    <lg>
      <l level="3">
        <xsl:apply-templates/>
      </l>
    </lg>
  </xsl:template>

  <!-- qa -->
  <xsl:template match="para[@style='qa']">
    <lg>
      <l>
        <hi type="italic">
        <xsl:apply-templates/>
        </hi>
      </l>
    </lg>
  </xsl:template>

  <!-- sp -->
  <xsl:template match="para[@style='sp']">
    <speech>
      <speaker>
        <hi type="italic">
          <xsl:apply-templates/>
        </hi>
      </speaker>
    </speech>
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

  <!-- note -->
  <xsl:template match="note">
    <xsl:copy>
      <xsl:attribute name="type">
        <xsl:text>crossReference</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="n">
        <xsl:value-of select="@caller"/>
      </xsl:attribute>
      <xsl:apply-templates/>
    </xsl:copy>
  </xsl:template>
  
  <!-- bk -->
  <xsl:template match="char[@style='bk']">
    <xsl:element name="reference">
      <xsl:attribute name="type">
        <xsl:text>x-bookName</xsl:text>
      </xsl:attribute>
      <xsl:apply-templates/>
    </xsl:element>
  </xsl:template>
  
  <!-- xo -->
  <xsl:template match="char[@style='xo']">
    <xsl:element name="reference">
      <xsl:attribute name="type">
        <xsl:text>source</xsl:text>
      </xsl:attribute>
      <xsl:apply-templates/>
    </xsl:element>
  </xsl:template>
  
  <!-- iot -->
  <xsl:template match="para[@style='iot']">
    <div type="introduction" canonical="false">
      <div type="outline">
        <xsl:element name="title">
          <xsl:apply-templates/>
        </xsl:element>
      </div>
    </div>
  </xsl:template>
  
  <xsl:template match="para[@style='imt']">
    <div type="introduction" canonical="false">
      <div type="main">
        <xsl:element name="title">
          <xsl:apply-templates/>
        </xsl:element>
      </div>
    </div>
  </xsl:template>
  <xsl:template match="para[@style='li2' or @style='pi2']"/>
  <xsl:template match="para[@style='li3' or @style='pi3']"/>
  <xsl:template match="para[@style='toc1' or @style='toc2' or @style='toc3']"/>

</xsl:stylesheet>