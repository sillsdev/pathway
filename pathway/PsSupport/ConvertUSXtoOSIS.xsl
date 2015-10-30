<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="no" indent="yes"
    doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"/>
  <xsl:param name="xmlLang"/>
  <xsl:param name="ws" select="'es'"/>
  <xsl:param name="li" select="'font-family:12pt;text-indent: -.375pt;'"/>
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

  <xsl:template name="string-replace-all">
    <xsl:param name="text"/>
    <xsl:param name="replace"/>
    <xsl:param name="by"/>
    <xsl:choose>
      <xsl:when test="contains($text,$replace)">
        <xsl:value-of select="substring-before($text,$replace)"/>
        <xsl:value-of select="$by"/>
        <xsl:call-template name="string-replace-all">
          <xsl:with-param name="text" select="substring-after($text,$replace)"/>
          <xsl:with-param name="replace" select="$replace"/>
          <xsl:with-param name="by" select="$by"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$text"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="usx|usfm|USX">
    <osis xmlns="http://www.bibletechnologies.net/2003/OSIS/namespace" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.bibletechnologies.net/2003/OSIS/namespace http://www.bibletechnologies.net/osisCore.2.1.1.xsd">
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
    <div type="book" osisID="{$bookCode}" canonical="true">
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
      <xsl:variable name="Lastchapter" select="descendant::chapter[last()]/@number"/>
      <xsl:variable name="LastVerseChapter" select="descendant::verse[last()]/preceding::chapter[1]/@number"/>
      <xsl:if test="$Lastchapter=$LastVerseChapter">
        <xsl:call-template name="VerseTemplate">
          <xsl:with-param name="versenumber" select="descendant::verse[last()]/@number"/>
          <xsl:with-param name="booknumber" select="@code"/>
          <xsl:with-param name="chapternumber" select="$Lastchapter"/>
          <xsl:with-param name="IDTypeattribute">
            <xsl:text>eID</xsl:text>
          </xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <xsl:if test="descendant::chapter">
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
    <xsl:if test="preceding-sibling::chapter">
      <xsl:if test="preceding::verse/ancestor::book/@code=$BookID">
        <xsl:call-template name="VerseTemplate">
          <xsl:with-param name="versenumber" select="preceding::verse[1]/@number"/>
          <xsl:with-param name="booknumber" select="$BookID"/>
          <xsl:with-param name="chapternumber" select="preceding-sibling::chapter[1]/@number"/>
          <xsl:with-param name="IDTypeattribute">
            <xsl:text>eID</xsl:text>
          </xsl:with-param>
        </xsl:call-template>
      </xsl:if>
      <xsl:element name="chapter">
        <xsl:attribute name="eID">
          <xsl:value-of select="concat($BookID,'.',preceding-sibling::chapter[1]/@number)"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
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
  <xsl:template match="para" name="GeneralPara">
    <xsl:choose>
      <xsl:when test="@style='mt' or @style='mt5' or @style='ms1'">
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
           <xsl:for-each select="node()|text()">
                <xsl:apply-templates select="current()"/>
          </xsl:for-each>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="verse">
          <xsl:variable name="BookID">
            <xsl:value-of select="ancestor::book[1]/@code"/>
          </xsl:variable>
          <xsl:variable name="chapternumber">
      <xsl:value-of select="preceding::chapter[1]/@number"/>
          </xsl:variable>
    <xsl:if test="preceding::verse/ancestor::book/@code=$BookID">
                  <xsl:call-template name="VerseTemplate">
        <xsl:with-param name="versenumber" select="preceding::verse[1]/@number"/>
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
    <xsl:choose>
      <xsl:when test="contains($IDValue,'-')">
        <xsl:element name="verse">
          <xsl:if test="$IDTypeattribute='sID'">
            <xsl:attribute name="{$IDTypeattribute}">
              <xsl:value-of select="substring-before($IDValue,'-')"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="$IDTypeattribute='eID'">
            <xsl:attribute name="{$IDTypeattribute}">
              <xsl:value-of select="substring-after($IDValue,'-')"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="$IDTypeattribute='sID'">
            <xsl:attribute name="osisID">
              <xsl:value-of select="substring-before($IDValue,'-')"/>
            </xsl:attribute>
            <xsl:attribute name="n">
              <xsl:value-of select="substring-before($versenumber,'-')"/>
            </xsl:attribute>
          </xsl:if>
        </xsl:element>
        <xsl:if test="$IDTypeattribute='sID'">
          <span><hi type="super">-</hi></span>
          <xsl:element name="verse">
            <xsl:attribute name="eID">
              <xsl:value-of select="substring-before($IDValue,'-')"/>
            </xsl:attribute>
          </xsl:element>
          <xsl:element name="verse">
            <xsl:attribute name="{$IDTypeattribute}">
              <xsl:value-of select="substring-after($IDValue,'-')"/>
            </xsl:attribute>
            <xsl:attribute name="osisID">
              <xsl:value-of select="substring-after($IDValue,'-')"/>
            </xsl:attribute>
            <xsl:attribute name="n">
              <xsl:value-of select="substring-after($versenumber,'-')"/>
            </xsl:attribute>
          </xsl:element>
        </xsl:if>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="verse">
          <xsl:attribute name="{$IDTypeattribute}">
            <xsl:value-of select="$IDValue"/>
          </xsl:attribute>
          <xsl:if test="$IDTypeattribute='sID'">
            <xsl:attribute name="osisID">
              <xsl:value-of select="translate($IDValue,'-',' ')"/>
            </xsl:attribute>
            <xsl:attribute name="n">
              <xsl:value-of select="$versenumber"/>
            </xsl:attribute>
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
  <xsl:template match="para[@style='li1' or @style='pi' or @style='pi1' or @style='io1']">
    <div>
      <list>
        <item style="{$li}">
          <xsl:if test="@style='pi' or @style='pi1'">
            <xsl:value-of 
              disable-output-escaping="yes"
              select="concat('&lt;p','&gt;')" />
          </xsl:if>
          <xsl:apply-templates/>
          <xsl:if test="following-sibling::*[1]/@style='li2' or following-sibling::*[1]/@style='pi2' or following-sibling::*[1]/@style='io2'">
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
  <xsl:template match="para[@style='li2' or style='pi2' or @style='io2']" mode="listitem">
    <list>
      <item>
        <xsl:if test="@style='pi2'">
          <xsl:value-of 
            disable-output-escaping="yes"
            select="concat('&lt;p','&gt;')" />
        </xsl:if>
        <xsl:apply-templates/>
        <xsl:if test="following-sibling::*[1]/@style='li3' or following-sibling::*[1]/@style='pi3' or following-sibling::*[1]/@style='io3'">
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
  <xsl:template match="para[@style='li3' or @style='pi3' or @style='io3']" mode="listitem">
    <list>
      <item>
        <xsl:if test="@style='pi3'">
          <xsl:value-of 
            disable-output-escaping="yes"
            select="concat('&lt;p','&gt;')" />
        </xsl:if>
        <xsl:apply-templates/>
        <xsl:if test="@style='pi3'">
          <xsl:value-of 
            disable-output-escaping="yes"
            select="concat('&lt;/p','&gt;')" />
        </xsl:if>
      </item>
    </list>
  </xsl:template>
  <xsl:template match="para[@style='s']">
   <div type="section">
      <title>
       <hi type="bold">
          <xsl:apply-templates/>
        </hi>
      </title>
    </div>
  </xsl:template>
  <xsl:template match="para[@style='r']">
    <div type="subsection">     
        <hi type="italic">
          <xsl:apply-templates/>
        </hi>        
    </div>
  </xsl:template>
  <xsl:template match="para[@style='mt1']">
    <div type="section">
      <title>
        <hi type="bold">
          <xsl:apply-templates/>
        </hi>
      </title>
    </div>
  </xsl:template>
  <xsl:template match="para[@style='mt2']">
    <div type="section">
      <title>
        <hi type="italic">
          <xsl:apply-templates/>
        </hi>
      </title>
    </div>
  </xsl:template>
  <xsl:template match="char[@style='w']">    
     <xsl:text>*</xsl:text>      
     <xsl:apply-templates/>
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

    <!-- d -->
    <xsl:template match="para[@style='d']">
      <div align="left">
        <div align="center">
          <hi type="italic">
            <xsl:apply-templates/>
          </hi>
        </div>
      </div>
    </xsl:template>

  <!-- fq -->
  <xsl:template match="char[@style='fq' and @closed='false']">
    <xsl:if test="not(following-sibling::char[@style='nd'])">
      <hi type="italic">
        <xsl:apply-templates/>
      </hi>
    </xsl:if>
  </xsl:template>
  <!-- fqa -->
  <xsl:template match="char[@style='fqa' and @closed='false']">
    <xsl:if test="not(following-sibling::char[@style='nd'])">
      <hi type="italic">
        <xsl:apply-templates/>
      </hi>
    </xsl:if>
  </xsl:template>
  <!-- qs -->
  <xsl:template match="char[@style='qs' and @closed='false']">
    <hi type="italic">
      <xsl:apply-templates/>
    </hi>
  </xsl:template>
  
  <!-- nd -->
  <xsl:template match="char[@style='nd']">
    <xsl:if test="preceding-sibling::char[@style='fq']">
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','hi type=&quot;italic&quot;','&gt;')" />
    </xsl:if>
    <seg>
      <divineName>
        <xsl:apply-templates/>
      </divineName>
    </seg>
    <xsl:if test="preceding-sibling::char[@style='fq']">
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','/hi','&gt;')" />
    </xsl:if>
  </xsl:template>

  <!-- b -->
  <xsl:template match="para[@style='b']">
    <br/>
  </xsl:template>
  <!-- ide -->
  <xsl:template match="para[@style='ide']"> </xsl:template>

  <!-- q, q1 -->
  <xsl:template match="para[@style='q' or @style='q1']">
    <xsl:if test="not(preceding-sibling::para[1][starts-with(@style,'q')])">
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','p type=&quot;embedded&quot;','&gt;')" />
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','lg','&gt;')" />
    </xsl:if>
    <l level="1" >
      <xsl:text> &#xA0;&#xA0;</xsl:text><xsl:apply-templates/>
    </l>
    <xsl:if test="not(following-sibling::para[1][starts-with(@style,'q')])">
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','/lg','&gt;')" />
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','/p','&gt;')" />
    </xsl:if>
  </xsl:template>

  <!-- q2 -->
  <xsl:template match="para[@style='q2']">
    <l level="2" ><xsl:text>&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;</xsl:text>
        <xsl:apply-templates/>
    </l>
    <xsl:if test="not(following-sibling::para[1][starts-with(@style,'q')])">
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','/lg','&gt;')" />
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','/p','&gt;')" />
    </xsl:if>
  </xsl:template>

  <!-- q3 -->
  <xsl:template match="para[@style='q3']">
    <l level="3"><xsl:text>&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;&#xA0;</xsl:text>
        <xsl:apply-templates/>
    </l>
      <xsl:if test="not(following-sibling::para[1][starts-with(@style,'q')])">
        <xsl:value-of
          disable-output-escaping="yes"
          select="concat('&lt;','/lg &gt;')" />
        <xsl:value-of
          disable-output-escaping="yes"
          select="concat('&lt;','/p','&gt;')" />
      </xsl:if>
  </xsl:template>

  <!-- qa -->
  <xsl:template match="para[@style='qa']">
    <xsl:if test="not(preceding-sibling::para[1][starts-with(@style,'q')])">
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','p type=&quot;embedded&quot;','&gt;')" />
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','lg','&gt;')" />
    </xsl:if>
      <l type="acrostic">
        <hi type="italic">
          <xsl:text>&#xA0;&#xA0;</xsl:text><xsl:apply-templates/>
        </hi>
      </l>
    <xsl:if test="not(following-sibling::para[1][starts-with(@style,'q')])">
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','/lg','&gt;')" />
      <xsl:value-of
        disable-output-escaping="yes"
        select="concat('&lt;','/p','&gt;')" />
    </xsl:if>
  </xsl:template>

  <!-- qt -->
  <xsl:template match="char[@style='qt']">
    <seg type="otPassage">
      <span><hi type="italic"><xsl:apply-templates/></hi></span>
    </seg>
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
	<xsl:if test="@style='x'">
        <xsl:attribute name="type">
          <xsl:text>crossReference</xsl:text>
        </xsl:attribute>
      </xsl:if>
	  <xsl:if test="@style='f'">
        <xsl:attribute name="placement">
          <xsl:text>x-foot</xsl:text>
        </xsl:attribute>
      </xsl:if>
      <xsl:attribute name="n">
        <xsl:value-of select="@caller"/>
      </xsl:attribute>
      <xsl:apply-templates/>
    </xsl:copy>
  </xsl:template>
  
<!-- fr -->
  <xsl:template match="char[@style='fr']">
    <xsl:element name="reference">
      <xsl:attribute name="osisRef">
        <xsl:value-of select="normalize-space(text())"/>
      </xsl:attribute>
      <xsl:attribute name="type">
        <xsl:text>source</xsl:text>
      </xsl:attribute>
      <xsl:apply-templates/>
    </xsl:element>
  </xsl:template>

  <!-- ft -->
  <xsl:template match="char[@style='ft']">
    <xsl:apply-templates/>
  </xsl:template>

  <!-- add, bdit -->
  <xsl:template match="char[@style='add' or @style='bdit']">
  <span>
    <hi type="bold">
      <hi type="italic">
        <xsl:apply-templates/>
      </hi>
    </hi>
	</span>
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
  
  <!-- figure -->
  <xsl:template match="figure[@style='fig']">
    <xsl:element name="figure">
      <xsl:attribute name="src">
        <xsl:value-of select="concat(substring-before(@file,'.'),'.jpg')"/>
      </xsl:attribute>
      <xsl:attribute name="size">
        <xsl:value-of select="@size"/>
      </xsl:attribute>
      <xsl:attribute name="osisRef">
        <xsl:value-of select="@ref"/>
      </xsl:attribute>
      <xsl:attribute name="alt">
        <xsl:text>No Image</xsl:text>
      </xsl:attribute>
      <xsl:element name="caption">
        <xsl:value-of select="text()"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <!-- ord -->
  <xsl:template match="char[@style='ord']">
   <span>
    <hi type="super">
      <xsl:apply-templates/>
    </hi>
   </span>
  </xsl:template>

  <!-- pn, pd -->
  <xsl:template match="char[@style='pn' or @style='bd']">
    <span> <hi type="bold">
      <xsl:apply-templates/>
    </hi></span>
  </xsl:template>

  <!-- sig, sls, em, t1, dc -->
  <xsl:template match="char[@style='sig' or @style='sls' or @style='em' or @style='tl' or @style='dc' or @style='it']">
    <span> <hi type="italic">
      <xsl:apply-templates/>
    </hi></span>
  </xsl:template>

   <!-- sc -->
  <xsl:template match="char[@style='sc']">
    <span>
    <hi type="small-caps">
      <xsl:apply-templates/>
    </hi></span>
  </xsl:template>

  <!-- wj -->
  <xsl:template match="char[@style='wj']">
    <q who="Jesus">
      <xsl:apply-templates/>
    </q>
  </xsl:template>

  <!-- lit -->
  <xsl:template match="para[@style='lit']">
    <div align="left">
      <div align="right">
        <hi type="bold">
          <xsl:apply-templates/>
        </hi>
      </div>
    </div>
  </xsl:template>

  <!-- period -->
  <xsl:template match="text()">
    <xsl:choose>
      <xsl:when test="contains(current(),'.')">
        <xsl:call-template name="string-replace-all">
          <xsl:with-param name="text" select="current()"/>
          <xsl:with-param name="replace">
            <xsl:text>.</xsl:text>
          </xsl:with-param>
          <xsl:with-param name="by" select="'. '"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="current()"/>
      </xsl:otherwise>
    </xsl:choose>
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