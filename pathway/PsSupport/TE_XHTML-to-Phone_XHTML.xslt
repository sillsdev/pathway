<?xml version="1.0" encoding="UTF-8"?>
<!-- Transform XHTML file exported from TE into a simpler XHTML to be used in making a phone app. -->
<!-- ?xml version="1.0"? -->
<!-- From Jim A's xhtml2bcv.xsl -->
<!-- Modified April 14, 2010. -->
<!-- Modified April 30, 2010: added <div class="Line2">. -->
<!-- Modified June 7, 2010: replace curly quotes with straight quotes in span text. -->
<!-- Modiifed June 30, 2010: use Muenchian grouping to speed up the transformation.
		 Added the processing of span with @class='Inscription'. -->

<!-- Note Jim's use of xsl:transform, rather than xsl:stylesheet. They are interchangeable. -->
<xsl:transform version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml"
    xmlns="http://www.w3.org/1999/xhtml"
    xmlns:fn="http://www.w3.org/2005/xpath-functions"
    exclude-result-prefixes="xhtml">
    
    <xsl:output method="xml" version="1.0" encoding="UTF-8" doctype-public="-//W3C//DTD XHTML 1.1//EN" doctype-system="http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd" indent="yes"/>

    <xsl:strip-space elements="*"/>

	<!-- Use a key to speed up the processing of the verses for each chapter. -->
	<xsl:key name="verses-by-chapter" match="xhtml:span[@class='Verse_Number']"
			use="generate-id(preceding::xhtml:span[@class='Chapter_Number'][1])" />
	<!-- Use a key to speed up the processing of the spans for each verse. -->
	<xsl:key name="spans-by-verse" match="xhtml:span[not(@class) or @class='Inscription' or @class='Quoted_Text' or @class='Words_Of_Christ']"
			use="generate-id(preceding::xhtml:span[@class='Verse_Number'][1])" />

    <!-- Process the top element. -->
    <xsl:template match="xhtml:html">
		<!-- Jim created the element <scripture> for xhtml:html. I'm retaining the original <html> element.
        <xsl:element name="scripture">
            <xsl:apply-templates/>  
        </xsl:element> -->
		<xsl:copy>
			<!-- Do not copy the erroneous html/@lang, which should be html/@xml:lang. -->
			<xsl:for-each select="@*[not(local-name() = 'lang' )]">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:attribute name="xml:lang"><xsl:value-of select="@xml:lang"/></xsl:attribute>
			<xsl:apply-templates/>
		</xsl:copy>
    </xsl:template>    

    <!-- Jim tosses out the head. I'm keeping it for now.    
    <xsl:template match="xhtml:head"/ -->
    <xsl:template match="xhtml:head">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:apply-templates/>
		</xsl:copy>
    </xsl:template>    

	<!-- Children of <head>. -->
	<xsl:template match="xhtml:title | xhtml:link | xhtml:meta">
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:apply-templates/>
		</xsl:copy>
	</xsl:template>

	<!-- span for Section_Head or Section_Head_Major or Line1 -->
	<!-- 30Apr2010 Skip section heads, since GoBible does not handle these yet.
	<xsl:template match="xhtml:span[parent::xhtml:div/@class='Section_Head' or
		parent::xhtml:div/@class='Section_Head_Major' or parent::xhtml:div/@class='Line1']" -->
	<xsl:template match="xhtml:span[parent::xhtml:div/@class='Line1' or parent::xhtml:div/@class='Line2']">
		<!-- Skip this span if the value is an empty string. -->
		<xsl:variable name="spanValue" select="normalize-space(./text())"/>
		<xsl:if test="string-length($spanValue) > 0">
			<xsl:element name="span">
				<xsl:variable name="lang" select="normalize-space(@lang)"/>
				<xsl:attribute name="class"><xsl:value-of select="parent::*/@class"/></xsl:attribute>
				<xsl:if test="string-length($lang) > 0">
					<xsl:attribute name="xml:lang"><xsl:value-of select="$lang"/></xsl:attribute>
				</xsl:if>
				<xsl:text> </xsl:text>
				<xsl:value-of select="."/>
			</xsl:element>
		</xsl:if>
	</xsl:template>

    <!-- For the body section, process the books. -->
    <xsl:template match="xhtml:body">
        <xsl:variable name="books" select="//xhtml:div[@class='scrBook']"/>
        <!-- I'm retaining the element <body class='scrBody'>, and am creating the element <div class='testament'> here. -->
		<xsl:copy>
			<xsl:for-each select="@*">
				<xsl:copy/>
			</xsl:for-each>
			<xsl:element name="div">
				<xsl:attribute name="class"><xsl:text>testament</xsl:text></xsl:attribute>
				<xsl:apply-templates select="$books"/>
			</xsl:element>
		</xsl:copy>
    </xsl:template>
    
    <!-- Process the books. -->
    <xsl:template match="xhtml:div[@class='scrBook']" >
        <xsl:element name="div">
			<!-- There are various ways of getting the book name. Another alternative is to use div[@class='Title_Main']/span. -->
            <!-- xsl:attribute name="name">
                <xsl:value-of  select="xhtml:span[@class='scrBookName']"/>
            </xsl:attribute -->
            <xsl:attribute name="class"><xsl:text>scrBook</xsl:text></xsl:attribute>
            <xsl:attribute name="title">
                <xsl:value-of select="descendant::xhtml:span[@class='scrBookName']"/>
            </xsl:attribute>
            <!-- Process the chapters. -->
            <xsl:variable name="chapters" select="descendant::xhtml:span[@class='Chapter_Number']"/>       
            <xsl:apply-templates select="$chapters" mode="process"/>
        </xsl:element>
    </xsl:template>
    
    <!-- Process the chapters. -->
    <xsl:template match="xhtml:span[@class='Chapter_Number']" mode="process">
        <!-- Process all text between here and the matching chapter end. -->
        <!-- In this example we will toss out all formatting. In a more general approach we would convert all paragraph formatting into mileposts. -->
        <xsl:variable name="reportProgress" select="fn:abs(.)"/>
        <xsl:variable name="currentChapter" select="."/>
		<xsl:element name="div">
            <xsl:variable name="startGenerateID" select="generate-id()"/>
            <!-- Collect all the verses to be processed for the given chapter. -->
            <!-- xsl:variable name="verses" select="following::xhtml:span[@class='Verse_Number']
							[generate-id(preceding::xhtml:span[@class='Chapter_Number'][1])=$startGenerateID]"/ -->
				<!-- [preceding::xhtml:span[@class='Chapter_Number'][1]=$currentChapter] -->
			<xsl:variable name="verses" select="key('verses-by-chapter',$startGenerateID)"/>
			<xsl:attribute name="class"><xsl:text>chapter</xsl:text></xsl:attribute>
			<xsl:attribute name="title"><xsl:value-of select="."/></xsl:attribute>
            <xsl:apply-templates select="$verses" mode="processVerses"/>
        </xsl:element>
    </xsl:template>
    
    <!-- Process the verses for a given chapter. -->
    <xsl:template match="xhtml:span[@class='Verse_Number']" mode="processVerses">
        <!-- Process all text between here and the matching verse end. -->
        <!-- In this example we will toss out all formatting. In a more general approach we would convert all paragraph formatting into mileposts. -->
		<xsl:element name="div">
			<xsl:attribute name="class"><xsl:text>verse</xsl:text></xsl:attribute>
			<xsl:attribute name="title"><xsl:value-of select="."/></xsl:attribute>
            <!-- Use the unique identifier (generate-id()) for the verse to gather the corresponding text.  -->
            <xsl:variable name="startGenerateID" select="generate-id()"/>
            <!-- xsl:variable name="verseSelected"
					select="following::xhtml:span[not(name()=span[@class='Verse_Number'])]
							[generate-id(preceding::xhtml:span[@class='Verse_Number'][1])=$startGenerateID]" / -->
			<xsl:variable name="verseSelected" select="key('spans-by-verse',$startGenerateID)"/>
            <xsl:apply-templates select="$verseSelected"/>        
        </xsl:element>
    </xsl:template>

    <!-- Skip the following elements, which are already processed. -->  
    <xsl:template match="xhtml:span[@class='Chapter_Number']"/>
    <xsl:template match="xhtml:span[@class='Verse_Number']"/>
    <xsl:template match="xhtml:span[@class='scrBookName']"/>

	<!-- Elements that are not implemented for GoBible. -->
	<xsl:template match="xhtml:span[@class='scrFootnoteMarker']"/> <!-- This eliminates the element "a". -->
	<!-- Footnote/popup; we might want this later. -->
	<xsl:template match="xhtml:span[@class='Note_General_Paragraph']"/>
	<xsl:template match="xhtml:span[@class='Note_General_Paragraph']/xhtml:span"/>
	<!-- Cross reference; we might want this later. -->
	<xsl:template match="xhtml:span[@class='Note_CrossHYPHENReference_Paragraph']"/>
	<xsl:template match="xhtml:span[@class='Note_CrossHYPHENReference_Paragraph']/xhtml:span"/>
	<xsl:template match="xhtml:div[@class='scrIntroSection']"/>
	<xsl:template match="xhtml:div[@class='Parallel_Passage_Reference']"/>
	<xsl:template match="xhtml:div[@class='pictureCenter']"/>
	<xsl:template match="xhtml:div[@class='pictureColumn']"/>
	<xsl:template match="xhtml:div[@class='pictureCaption']/xhtml:span"/>
	<xsl:template match="xhtml:img"/>
	<!-- 30Apr2010 - skip span for Section_Head and Section_Head_Major ... -->
	<xsl:template match="xhtml:span[parent::xhtml:div/@class='Section_Head']"/>
	<xsl:template match="xhtml:span[parent::xhtml:div/@class='Section_Head_Major']"/>
	<xsl:template match="xhtml:span[parent::xhtml:div/@class='Section_Head_Minor']"/>
	<xsl:template match="xhtml:span[parent::xhtml:div/@class='Parallel_Passage_Reference']"/>


    <!-- copy -->  
    <xsl:template match="xhtml:span">
		<!-- I added the test that if a <span> has no children and no value, do not copy it. -->
		<xsl:if test="count(child::*) > 0 or string-length(normalize-space(./text())) > 0">
			<xsl:copy>
				<!-- Change span/@lang to span/@xml:lang. -->
				<xsl:for-each select="@*[not(local-name() = 'lang' )]">
					<xsl:copy/>
				</xsl:for-each>
				<xsl:attribute name="xml:lang"><xsl:value-of select="@lang"/></xsl:attribute>
				<xsl:apply-templates/>
			</xsl:copy>
		</xsl:if>
    </xsl:template>
    
    <!-- Special handling of text. -->
    <xsl:template match="text()">
		<!-- Replace curly quotes with straight quotes. -->
		<xsl:value-of select="translate(.,'“”','&quot;&quot;')"/>
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