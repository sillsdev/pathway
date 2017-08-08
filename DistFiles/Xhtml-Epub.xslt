<?xml version="1.0" encoding="UTF-8"?>
<!-- Transform Xhtml file of FieldWorks into a "flatter" Epub Xhtml schema -->
<!-- For Scripture the output file is neither chapter nor section-centric. -->
<!-- From Larry W.'s TE_XHTML-to-Phone_XHTML.xslt -->

<xsl:transform version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml"
    xmlns="http://www.w3.org/1999/xhtml"
    xmlns:fn="http://www.w3.org/2005/xpath-functions"
    exclude-result-prefixes="xhtml">
    
	<xsl:output method="xml" version="1.0" encoding="UTF-8" doctype-public="-//W3C//DTD XHTML 1.1//EN" doctype-system="http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd" indent="yes"  />

	<xsl:strip-space elements="*"/>

	<!-- Use a key to speed up the processing of the verses for each chapter. -->
	<xsl:key name="verses-by-chapter" match="xhtml:span[@class='Verse_Number']"
			use="generate-id(preceding::xhtml:span[@class='Chapter_Number'][1])" />
	
	<!-- the main language of this document -->
	<xsl:variable name="docLanguage" select="xhtml:html/@lang" />
	
	<!--Straight copy for these elements. -->
	<xsl:template match="xhtml:head | xhtml:title | xhtml:link | xhtml:a | xhtml:table | xhtml:tr | xhtml:th | xhtml:td | xhtml:em | xhtml:br | xhtml:ul | xhtml:li">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:apply-templates/>
		</xsl:copy>
	</xsl:template>
	
	<!--Straight copy for these elements. -->
	<xsl:template match="xhtml:html">
		<xsl:copy>
			<xsl:for-each select="@*[not(name() = 'lang' )]">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:apply-templates/>
		</xsl:copy>
	</xsl:template>
	
	<!-- create an ID for the body element so we can link to it -->
	<xsl:template match="xhtml:body">
		<xsl:copy>
			<xsl:attribute name="id"><xsl:text>body</xsl:text></xsl:attribute>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:apply-templates/>
		</xsl:copy>
	</xsl:template>
	
	<!-- Special processing for a couple divs -->
	<xsl:template match="xhtml:div|xhtml:audio|xhtml:video|xhtml:source">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:apply-templates/>
			<!-- Section div reference processing -->
			<!-- end section reference processing -->
		</xsl:copy>
	</xsl:template>

	<!-- image processing -->
	<xsl:template match="xhtml:img">
		<xsl:copy>
			<!-- The image munging in the preprocessor leaves some old image references - scrape them out here -->
			<xsl:attribute name="src"><xsl:value-of select="@src"/></xsl:attribute>
			<xsl:attribute name="alt"><xsl:value-of select="@src"/></xsl:attribute>
			<xsl:attribute name="longdesc"><xsl:value-of select="@src"/></xsl:attribute>
			<xsl:if test="@class!=''">
				<xsl:attribute name="class"><xsl:value-of select="@class"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates/>
		</xsl:copy>
	</xsl:template>

	<!-- skip these elements altogether -->
	<xsl:template match="xhtml:span[@class='Chapter_Number']" />
	<xsl:template match="xhtml:span[@class='Note_General_Paragraph']/xhtml:span" />
	<xsl:template match="xhtml:span[@class='Note_CrossHYPHENReference_Paragraph']/xhtml:span" />
	<xsl:template match="xhtml:meta" />
	<!-- <xsl:template match="xhtml:span[../@class='Title_Main']" /> -->

  <!-- Move the chapter number outside the paragraph div, so that it can be placed in the margin or drop-capped -->
  <xsl:template match="xhtml:div[xhtml:span[@class='Chapter_Number']]">
    <xsl:if test="(count(descendant::xhtml:span[@class='Chapter_Number'])) > 0">
      <xsl:element name="div">
        <xsl:attribute name="class">
          <xsl:text>Chapter_Number</xsl:text>
        </xsl:attribute>
        <xsl:attribute name="id">
          <xsl:value-of select=".//xhtml:span[@class='Chapter_Number']/@id"/>
        </xsl:attribute>
        <xsl:value-of select="descendant::xhtml:span[@class='Chapter_Number']"/>
      </xsl:element>
    </xsl:if>
    <xsl:element name="div">
      <xsl:for-each select="@*">
        <xsl:copy/>
      </xsl:for-each>
      <xsl:apply-templates/>
    </xsl:element>
  </xsl:template>
	
	<!-- write out the contents of these elements, but not the elements themselves -->
	<xsl:template match="xhtml:div[@class='scrSection']">
		<xsl:apply-templates/>
		<!-- secondary Section div reference processing -->
		<!-- end secondary section reference processing -->
	</xsl:template>
	<xsl:template match="xhtml:div[@class='columns']" >
		<xsl:apply-templates />
	</xsl:template>
	<xsl:template match="xhtml:div[@class='Title_Main']" >
		<xsl:apply-templates />
	</xsl:template>
	
	<!-- Process the chapters. -->
	<xsl:template match="xhtml:span[@class='Chapter_Number']" mode="process">
	        	<!-- Process all text between here and the matching chapter end. -->
		<xsl:element name="div">
	            		<xsl:variable name="startGenerateID" select="generate-id()"/>
			<xsl:variable name="verses" select="key('verses-by-chapter',$startGenerateID)"/>
			<xsl:attribute name="class"><xsl:text>chapter</xsl:text></xsl:attribute>
			<xsl:attribute name="title"><xsl:value-of select="."/></xsl:attribute>
	            		<xsl:apply-templates select="$verses" mode="processVerses"/>
	        	</xsl:element>
	</xsl:template>
    
	<!-- Title section(s) for each book -->
	<!-- The spans in these divs are some combination of the main title and/or secondary / tertiary titles. Just handle the main title here, as it doesn't have a class for the span text. -->
	<xsl:template match="xhtml:div[@class='Title_Main']/xhtml:span[not(@class)]">
		<xsl:element name="div">
			<xsl:attribute name="class"><xsl:text>Title_Main</xsl:text></xsl:attribute>
			<xsl:if test="@lang != '' and @lang !=docLanguage">
				<xsl:attribute name="xml:lang"><xsl:value-of select="@lang"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="."/>
		</xsl:element>
	</xsl:template>

	<xsl:template match="xhtml:div[@class='Title_Main']/xhtml:span[@class='Title_Secondary']">
		<xsl:element name="div">
			<xsl:attribute name="class"><xsl:text>Title_Secondary</xsl:text></xsl:attribute>
			<xsl:value-of select="."/>
		</xsl:element>
	</xsl:template>

	<xsl:template match="xhtml:div[@class='Title_Main']/xhtml:span[@class='Title_Tertiary']">
		<xsl:element name="div">
			<xsl:attribute name="class"><xsl:text>Title_Tertiary</xsl:text></xsl:attribute>
			<xsl:value-of select="."/>
		</xsl:element>
	</xsl:template>
	
	<!-- Straight copy for front matter items -->
	<xsl:template match="xhtml:div[@class='Front_Matter'] | xhtml:div[@class='Front_Matter']/xhtml:p | xhtml:div/xhtml:h1 | xhtml:div/xhtml:h2 | xhtml:div[@class='Cover'] | xhtml:div[@class='Title'] | xhtml:div[@class='Title']/xhtml:p | xhtml:div[@class='Copyright']">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:apply-templates/>
		</xsl:copy>
	</xsl:template>
	
	<!-- inner text for scriptures - they don't have a class, so we'll assign them to class "scrText" -->
	<xsl:template match="xhtml:div[@class='Paragraph']/xhtml:span[not(@class)]">
		<xsl:element name="span">
			<xsl:attribute name="class"><xsl:text>scrText</xsl:text></xsl:attribute>
			<xsl:if test="@lang != '' and @lang !=docLanguage">
				<xsl:attribute name="xml:lang"><xsl:value-of select="@lang"/></xsl:attribute>
			</xsl:if>
			<xsl:for-each select="@*[not(local-name() = 'lang' )]">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:apply-templates></xsl:apply-templates>
		</xsl:element>
	</xsl:template>
	
	<!-- span processing -->  
	<!-- .epub uses xml:lang instead of lang (it uses an xml mimetype). -->
	<xsl:template match="xhtml:span">
		<xsl:if test="count(child::*) > 0 or string-length(normalize-space(./text())) > 0">
			<xsl:copy>
				<xsl:if test="@lang != '' and @lang !=docLanguage">
					<xsl:attribute name="xml:lang"><xsl:value-of select="@lang"/></xsl:attribute>
				</xsl:if>
				<xsl:for-each select="@*">
					<xsl:copy/>
				</xsl:for-each>
				<!-- if this is a reversal index entry (for a dictionary), add an ID -->
				<xsl:if test="@class='ReversalIndexEntry_Self'">
					<xsl:attribute name="id"><xsl:value-of select="generate-id()"/></xsl:attribute>
				</xsl:if>
				<!-- if this is a Chapter_Number span, add an ID with Book and Chapter -->
				<xsl:if test="@class = 'Chapter_Number'">
				<!--	<xsl:attribute name="id"><xsl:value-of select="../../../../xhtml:span[1]"/><xsl:value-of select="."/></xsl:attribute> -->
					<xsl:attribute name="id"><xsl:text>id</xsl:text><xsl:value-of select="../../../../xhtml:span[@class='scrBookCode']"/><xsl:text>_</xsl:text><xsl:value-of select="."/></xsl:attribute>
				</xsl:if>
				<xsl:if test="@class='Note_General_Paragraph'">
					<xsl:element name="a">
						<xsl:attribute name="href"><xsl:text>#FN_</xsl:text><xsl:value-of select="@id"/></xsl:attribute>
						<xsl:text>[</xsl:text><xsl:value-of select="@title"/><xsl:text>]</xsl:text>
					</xsl:element>
				</xsl:if>
				<xsl:if test="@class='Note_CrossHYPHENReference_Paragraph'">
					<xsl:element name="a">
						<xsl:attribute name="href"><xsl:text>#FN_</xsl:text><xsl:value-of select="@id"/></xsl:attribute>
						<xsl:text>[*]</xsl:text>
					</xsl:element>
				</xsl:if>
				<!-- Fwr-2550 & LT-10828 make reference upper case if necessary -->
				<xsl:if test="@class='scrFootnoteMarker'">
					<xsl:element name="a">
						<xsl:attribute name="href">
							<xsl:variable name="myRef">
								<xsl:text>#</xsl:text>
								<xsl:value-of select="following-sibling::node()/@id"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="./xhtml:a/@href != $myRef">
									<xsl:value-of select="translate(./xhtml:a/@href,'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="./xhtml:a/@href"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</xsl:element>
				</xsl:if>
				<!-- if this is a Verse_Number span, add an ID with Book, Chapter and Verse -->
				<!-- Note that the chapter is found by selecting "previous...[1]" - that selects the first item, counting backwards
				       (i.e. the previous node). Ugh. -->
				<xsl:if test="@class = 'Verse_Number'">
				<!--	<xsl:attribute name="id"><xsl:text>id</xsl:text><xsl:value-of select="../../../../xhtml:span[@class='scrBookCode']"/><xsl:text>_</xsl:text><xsl:value-of select="preceding::xhtml:span[@class='Chapter_Number'][1]"/><xsl:text>_</xsl:text><xsl:value-of select="."/></xsl:attribute> -->
					<!-- (sanitized to replace commas and colons in the verse with dashes and spaces with underscore) -->
          <xsl:variable name="verseNum" select="translate(./text(),' &#160;','__')" />
					<xsl:attribute name="id"><xsl:text>id</xsl:text><xsl:value-of select="../../../../xhtml:span[@class='scrBookCode']"/><xsl:text>_</xsl:text><xsl:value-of select="preceding::xhtml:span[@class='Chapter_Number'][1]"/><xsl:text>_</xsl:text>
						<xsl:text>vrs_</xsl:text>
            <xsl:value-of select="count(preceding::xhtml:span[@class = 'Verse_Number'])+1"/>
          </xsl:attribute>
				</xsl:if>
				<xsl:if test="count(@class) = 0 or @class != 'scrFootnoteMarker'"> <!-- FWR-2550 we handled child above -->
					<xsl:apply-templates/>
				</xsl:if>
			</xsl:copy>
		</xsl:if>
	</xsl:template>

	<!-- Default element and attribute templates. -->
	<xsl:template match="*">
		<xsl:comment>Warning :: The element "<xsl:value-of select="name()"/>", child of "<xsl:value-of select="name(..)"/>", has no matching template.</xsl:comment>
	</xsl:template>
	<!-- default attribute template -->
	<xsl:template match="@*">
		<xsl:comment>Warning :: The attribute "<xsl:value-of select="name()"/>" for element "<xsl:value-of select="name(..)"/>" has no matching template.</xsl:comment>
	</xsl:template>

</xsl:transform>