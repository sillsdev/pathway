<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml"
    exclude-result-prefixes="xhtml"
    xmlns="http://www.w3.org/1999/xhtml">

  <xsl:output method="xml" version="1.0" encoding="UTF-8" omit-xml-declaration="yes" indent="no"
      doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"/>

  <!-- Remove text (empty lines) at the root level. -->
  <xsl:strip-space elements="*"/>

  <!-- *** Named templates: IntroSection, ScrSection, h1, p *** -->
  <!-- Output an introduction section starting at element. -->
  <xsl:template name ="IntroSection">
    <xsl:param name="element" select="."/>
    <div class="scrIntroSection" xmlns="http://www.w3.org/1999/xhtml">
      <xsl:apply-templates select="$element" mode="IntroCopy"/>
    </div>
  </xsl:template>

  <!-- Output an Scripture section starting at element. -->
  <xsl:template name ="ScrSection">
    <xsl:param name="element" select="."/>
    <div class="scrSection" xmlns="http://www.w3.org/1999/xhtml">
      <xsl:apply-templates select="$element" mode="ScrCopy"/>
    </div>
  </xsl:template>

  <!-- Output ordinary paragraphs as div elements. -->
  <xsl:template name="h1">
    <div xmlns="http://www.w3.org/1999/xhtml">
      <xsl:apply-templates select="@*|node()"/>
    </div>
  </xsl:template>
  
  <xsl:template name="p">
    <div xmlns="http://www.w3.org/1999/xhtml">
      <xsl:apply-templates select="@*|node()"/>
    </div>
  </xsl:template>
  
  <!-- Copy all content that isn't explicitly processed by templates. -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()"/>
    </xsl:copy>
  </xsl:template>

  <!-- Create intro and Scripture structure within the book. -->
  <!-- Determine whether paragraphs and headers are introductions from their class name. -->
  <!-- Determine whether tables belong to the introduction or body from their relative location to paragraphs and headers. -->
  <xsl:template match="xhtml:div[@class='scrBook']">
    <xsl:copy>
      <xsl:copy-of select="@*"/>
      <!-- Copy the book name and the book code -->
      <xsl:copy-of select="xhtml:span"/>
      <!-- Copy the book title -->
      <xsl:copy-of select="xhtml:div"/>

      <!-- Handle any introduction section content that precede the first section heading. -->
      <xsl:apply-templates select="*[1]" mode="IntroSectionPrecedesHeading"/>
      
      <!-- Include any introduction sections that begin with headings -->
      <xsl:for-each select="xhtml:h1[starts-with(@class, 'Intro_')]">
        <xsl:if test="not(preceding-sibling::*[1][self::xhtml:h1][starts-with(@class, 'Intro_')])"> <!-- check last constraint -->
          <xsl:choose>
            <xsl:when test="@class = 'Intro_Section_Head' or @class = 'Intro_Title_Main' or @class = 'Intro_Title_Secondary' or
              @class = 'Intro_Title_Tertiary'">
              <xsl:call-template name="IntroSection"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:comment>Unrecognized Heading: {@class}</xsl:comment>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>
      </xsl:for-each>
      
      <!-- Scripture content -->
      <div class="columns" xmlns="http://www.w3.org/1999/xhtml">
        <!-- Handle any Scripture section content that precede the first section heading. -->
        <xsl:apply-templates select="*[1]" mode="ScrSectionPrecedesHeading"/>
        <!-- Include any Scripture sections that begin with headings -->
        <xsl:for-each select="xhtml:h1[not(starts-with(@class, 'Intro_'))]">
          <xsl:if test="not(preceding-sibling::*[1][self::xhtml:h1][not(starts-with(@class, 'Intro_'))])">
            <xsl:choose>
              <xsl:when test="@class = 'Section_Head' or @class = 'Chapter_Head' or @class = 'Hebrew_Title' or 
                @class = 'Parallel_Passage_Reference' or @class = 'Section_Head_Major' or 
                @class = 'Section_Head_Minor' or @class = 'Section_Head_Series' or @class = 'Section_Range_Paragraph' or 
                @class = 'Speech_Speaker' or @class = 'Variant_Section_Head'">
                <xsl:call-template name="ScrSection"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:comment>Unrecognized Heading: {@class}</xsl:comment>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:if>
        </xsl:for-each>
      </div>
    </xsl:copy>
  </xsl:template>

  <!-- Continue copying paragraphs and tables into an introduction section as long as we find introduction headings and paragraphs -->
  <xsl:template match="xhtml:h1" mode="IntroCopy">
    <xsl:call-template name="h1"/>
    <xsl:apply-templates select="following-sibling::*[1][((self::xhtml:h1 or self::xhtml:p) and 
      starts-with(@class, 'Intro_')) or self::xhtml:table]" mode="IntroCopy"/>
  </xsl:template>
  
  <!-- Copy heading and content paragraphs that follow a Scripture section head. -->
  <xsl:template match="xhtml:h1" mode="ScrCopy">
    <xsl:call-template name="h1"/>
    <xsl:apply-templates select="following-sibling::*[1][self::xhtml:h1 or self::xhtml:p or self::xhtml:table]"
        mode="ScrCopy"/>
  </xsl:template>

  <!-- Copy tables and paragraphs into an introduction section. -->
  <xsl:template match="xhtml:p" mode="IntroCopy">
    <xsl:call-template name="p"/>
    <xsl:apply-templates 
      select="following-sibling::*[1][(self::xhtml:p and starts-with(@class, 'Intro_')) or self::xhtml:table]" mode="IntroCopy"/>
  </xsl:template>
  
  <!-- Copy tables and paragraphs into a Scripture section. -->
  <xsl:template match="xhtml:p" mode="ScrCopy">
    <xsl:call-template name="p"/>
    <xsl:apply-templates select="following-sibling::*[1][self::xhtml:p or self::xhtml:table]" mode="ScrCopy"/>
  </xsl:template>
  
  <!-- Copy this table into an introduction section (and following tables and paragraphs as long as there are intro styles). -->
  <xsl:template match="xhtml:table" mode="IntroCopy">
    <xsl:copy-of select="."/>
    <xsl:apply-templates 
      select="following-sibling::*[1][(self::xhtml:p and starts-with(@class, 'Intro_')) or self::xhtml:table]" mode="IntroCopy"/>    
  </xsl:template>
  
  <!-- Copy this table into a Scripture section (and following tables and paragraphs). -->
  <xsl:template match="xhtml:table" mode="ScrCopy">
    <xsl:copy-of select="."/>
    <xsl:apply-templates select="following-sibling::*[1][self::xhtml:p or self::xhtml:table]" mode="ScrCopy"/>
  </xsl:template>
  
  <!-- *** Handle the first introduction paragraph (or table) at the root level. *** -->
  <xsl:template match="*" mode="IntroSectionPrecedesHeading"/>
  
  <xsl:template match="xhtml:span|xhtml:div" mode="IntroSectionPrecedesHeading">
    <xsl:apply-templates select="following-sibling::*[1]" mode="IntroSectionPrecedesHeading"/>
  </xsl:template>
  
  <xsl:template match="xhtml:p[starts-with(@class, 'Intro_')]" mode="IntroSectionPrecedesHeading">
    <xsl:call-template name="IntroSection"/>
  </xsl:template>
  
  <!-- *** Handle case where the first introduction element at the root level is a table rather than a paragraph *** -->
  <xsl:template match="xhtml:table" mode="IntroSectionPrecedesHeading">
    <xsl:apply-templates select="following-sibling::*[1]" mode="IntroTablePrecedesHeading">
      <xsl:with-param name="table" select="."/>
    </xsl:apply-templates>
  </xsl:template>
  
  <xsl:template match="*" mode="IntroTablePrecedesHeading"/>
  
  <xsl:template match="*[starts-with(@class, 'Intro_')]" mode="IntroTablePrecedesHeading">
    <xsl:param name="table"/>
    <xsl:call-template name="IntroSection">
      <xsl:with-param name="element" select="$table"/>
    </xsl:call-template>
  </xsl:template>
  
  <xsl:template match="xhtml:table" mode="IntroTablePrecedesHeading">
    <xsl:param name="table"/>
    <xsl:apply-templates select="following-sibling::*[1]" mode="IntroTablePrecedesHeading">
      <xsl:with-param name="table" select="$table"/>
    </xsl:apply-templates>
  </xsl:template>
  
  <!-- *** Handle the first Scripture paragraph (or table) at the root level. *** -->
  <xsl:template match="*" mode="ScrSectionPrecedesHeading"/>
  
  <xsl:template match="xhtml:span|xhtml:div" mode="ScrSectionPrecedesHeading">
    <xsl:apply-templates select="following-sibling::*[1]" mode="ScrSectionPrecedesHeading"/>
  </xsl:template>
  
  <xsl:template match="*[starts-with(@class, 'Intro_')]" mode="ScrSectionPrecedesHeading">
    <xsl:apply-templates select="following-sibling::*[1]" mode="ScrSectionPrecedesHeadingFollowsIntro"/>
  </xsl:template>

  <xsl:template match="xhtml:p[not(starts-with(@class, 'Intro_'))]" mode="ScrSectionPrecedesHeading">
    <xsl:call-template name="ScrSection"/>  
  </xsl:template>
  
  <xsl:template match="xhtml:table" mode="ScrSectionPrecedesHeading">
    <xsl:apply-templates select="following-sibling::*[1]" mode="ScrTablePrecedesHeading">
      <xsl:with-param name="table" select="."/>
    </xsl:apply-templates>
  </xsl:template>
  
  <!-- *** Handle case where the first Scripture element at the root level is a table rather than a paragraph *** -->
  <xsl:template match="*" mode="ScrTablePrecedesHeading"/>

  <xsl:template match="xhtml:table" mode="ScrTablePrecedesHeading">
    <xsl:param name="table"/>
    <xsl:apply-templates select="following-sibling::*[1]" mode="ScrTablePrecedesHeading">
      <xsl:with-param name="table" select="$table"/>
    </xsl:apply-templates>
  </xsl:template>

  <xsl:template match="xhtml:h1|xhtml:p" mode="ScrTablePrecedesHeading">
    <xsl:param name="table"/>
    <xsl:choose>
      <xsl:when test="starts-with(@class, 'Intro_')">
        <xsl:apply-templates select="following-sibling::*[1]" mode="ScrSectionPrecedesHeadingFollowsIntro"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="ScrSection">
          <xsl:with-param name="element" select="$table"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- *** Handle case where the first Scripture element at the root level immediately follows the introduction. *** -->
  <xsl:template match="*" mode="ScrSectionPrecedesHeadingFollowsIntro"/>

  <xsl:template match="*[starts-with(@class, 'Intro_')]|xhtml:table" mode="ScrSectionPrecedesHeadingFollowsIntro">
    <xsl:apply-templates select="following-sibling::*[1]" mode="ScrSectionPrecedesHeadingFollowsIntro"/>
  </xsl:template>

  <xsl:template match="xhtml:p[not(starts-with(@class, 'Intro_'))]" mode="ScrSectionPrecedesHeadingFollowsIntro">
    <xsl:call-template name="ScrSection"/>
  </xsl:template>
</xsl:stylesheet>
