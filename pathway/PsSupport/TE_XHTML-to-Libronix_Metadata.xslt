<?xml version="1.0" encoding="UTF-8"?>
<!-- Create the metadata (basic XML) file from the given XHTML file exported from TE . -->
<!-- Note: Eventually the metadata will come from Jim Albright's Visual Basic application. -->
<!-- Data types are needed for: copyright, í (for Spanish and Portuguese navigation-types),  -->

<!-- Note the use of "document('FileGuids.xml')" below. It is assumed that the file 'FileGuids.xml' is in the same directory as this XSLT file. -->

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml"
    xmlns:fn="http://www.w3.org/2005/xpath-functions"    
    exclude-result-prefixes="xhtml">

	<xsl:param name="currentYear" select="1959"/>

    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
    <xsl:strip-space elements="*"/>

	<xsl:variable name="CssFilenameAndExtension" select="xhtml:html/xhtml:head/xhtml:link[@rel='stylesheet']/@href" />
	<!-- Get the filename without the extension. -->
	<xsl:variable name="CssFilename" select="substring-before($CssFilenameAndExtension,'.css')" />

    <!-- Process the top element. -->
    <xsl:template match="xhtml:html">
		<!-- Jim created the element <scripture> for xhtml:html. I'm retaining the original <html> element.
        <xsl:element name="scripture">
            <xsl:apply-templates/>
        </xsl:element> -->
        <!-- example: <lbx-resource category="Resources" name="SL_BI_JBP" guid="LLS:B5481A5D-283B-4275-9DF6-4C2797D5BBCA" version="2010-04-02T23:54:14Z" creator="sil" owner="sil"> -->
        <xsl:element name="lbx-resource">
			<!-- TODO: find a way to supress the xml namespace "http://www.w3.org/1999/xhtml". -->
			<xsl:attribute name="category">Resources</xsl:attribute>
			<xsl:attribute name="name">
				<xsl:text>SL_BI_</xsl:text>
				<xsl:value-of select="$CssFilename" />
			</xsl:attribute>
			<!-- Note: The guid needs to come from Jim Albright's Visual Basic application.
				 For now, I have made a temporary file containing guids associated with filenames. -->
			<xsl:attribute name="guid">
				<xsl:text>LLS:</xsl:text>
				<xsl:value-of select="document('FileGuids.xml')//file[@name=$CssFilename]/@guid" />
			</xsl:attribute>
			<xsl:attribute name="version">2010-08-19T13:18:42Z</xsl:attribute>
			<xsl:attribute name="creator">sil</xsl:attribute>
			<xsl:attribute name="owner">sil</xsl:attribute>
			<!-- For now, only the <head> element in the exported XHTML file contains metadata. -->
			<xsl:apply-templates select="xhtml:head" />
        </xsl:element>
    </xsl:template> <!-- xhtml:html -->

	<!-- head -->
	<xsl:template match="xhtml:head">
		<!-- header -->
		<xsl:element name="header">
			<xsl:element name="title"><xsl:value-of select="xhtml:title/text()"/></xsl:element>
			<!-- TODO: Is a sort title, alternate title, or abbreviated title given for some resources? -->
        </xsl:element>
        <!-- metadata -->
        <xsl:variable name="lang">
			<xsl:value-of select="../xhtml:body[@class='scrBody']/xhtml:div[@class='scrBook'][1]/xhtml:span[@class='scrBookName']/@lang"/>
		</xsl:variable>
		<!-- Use a date stamp service to get today's date, to be used for "dc.date", and "dc.date.electronic". -->
		<!-- xsl:variable name="dateNow">
			<xsl:copy-of select="
document('http://xobjex.com/service/date.xsl')/date/utc/*"/>
		</xsl:variable -->
		<xsl:variable name="descriptionContent">
			<xsl:value-of select="../xhtml:head/xhtml:meta[@name='description']/@content"/>
		</xsl:variable>
        <!-- using SL_BI_JBP.xml as a template -->
        <xsl:element name="metadata">
			<xsl:element name="driver">
				<xsl:attribute name="guid">{63C914A0-40B7-11D3-9AF1-0050040AB2D6}</xsl:attribute>
				<!-- xsl:attribute name="version">2008-05-14T19:22:40Z</xsl:attribute>
				<xsl:attribute name="module">3.0.371.1</xsl:attribute -->
			</xsl:element>
			<xsl:element name="commerce-id">
				<xsl:attribute name="guid">LPT Draft Build</xsl:attribute>
			</xsl:element>
			<xsl:element name="alternate-id">
				<xsl:attribute name="guid">
					<xsl:text>LLS:SL_BI_</xsl:text>
					<xsl:value-of select="$CssFilename" />
				</xsl:attribute>
			</xsl:element>
			<xsl:element name="alternate-id">
				<xsl:attribute name="guid">
					<xsl:text>SL_BI_</xsl:text>
					<xsl:value-of select="$CssFilename" />
				</xsl:attribute>
			</xsl:element>
			<!-- dc-metadata -->
			<xsl:element name="dc-metadata">
				<xsl:element name="dc-element">
					<xsl:attribute name="name">dc.language:iso639</xsl:attribute>
					<xsl:value-of select="$lang"/>
				</xsl:element>
				<!-- Example metadata:
					<dc-element xml:lang="en" name="dc.title">The New Testament in Modern English|New Testament in Modern English</dc-element>
					<dc-element xml:lang="en" name="dc.title.alternate">Phillips Modern English version of the Bible</dc-element>
					<dc-element xml:lang="en" name="dc.author">Phillips, J.B., tr.</dc-element>
					<dc-element xml:lang="en" name="dc.title.abbrev">Phillips</dc-element>
					<dc-element xml:lang="en" name="dc.subject">Bible-N.T.-English</dc-element>
					<dc-element xml:lang="en" name="dc.publisher">Harper Collins</dc-element>
					<dc-element xml:lang="en" name="dc.pubplace"></dc-element>
				-->
				<xsl:element name="dc-element">
					<xsl:attribute name="xml:lang">
						<xsl:value-of select="$lang"/>
					</xsl:attribute>
					<xsl:attribute name="name">dc.title</xsl:attribute>
					<xsl:value-of select="xhtml:title/text()"/>
				</xsl:element>
				<!-- TODO: Determine dc.title.alternate, if it is available. -->
				<!-- TODO: Determine abbreviated title if available: "dc.title.abbrev". -->
				<xsl:element name="dc-element">
					<xsl:attribute name="xml:lang">
						<!-- xsl:value-of select="$lang"/ -->
						<xsl:text>en</xsl:text>
					</xsl:attribute>
					<xsl:attribute name="name">dc.subject</xsl:attribute>
					<xsl:text>Bible--N.T.--Bughotu.</xsl:text>
				</xsl:element>
				<xsl:element name="dc-element">
					<xsl:attribute name="xml:lang">
						<!-- xsl:value-of select="$lang"/ -->
						<xsl:text>en</xsl:text>
					</xsl:attribute>
					<xsl:attribute name="name">
						<xsl:text>dc.publisher</xsl:text>
					</xsl:attribute>
					<!-- TODO: Determine how to find the publisher. -->
					<xsl:text>SIL International</xsl:text>
				</xsl:element>
				<xsl:element name="dc-element">
					<xsl:attribute name="xml:lang">
						<!-- xsl:value-of select="$lang"/ -->
						<xsl:text>en</xsl:text>
					</xsl:attribute>
					<xsl:attribute name="name">
						<xsl:text>dc.publisher.place</xsl:text>
					</xsl:attribute>
					<!-- TODO: Determine how to find the publishing place. -->
					<xsl:text>Dallas</xsl:text>
				</xsl:element>
				<xsl:element name="dc-element">
					<xsl:attribute name="xml:lang">
						<!-- xsl:value-of select="$lang"/ -->
						<xsl:text>en</xsl:text>
					</xsl:attribute>
					<xsl:attribute name="name">dc.date</xsl:attribute>
					<!-- TODO: Determine how to find the date. -->
					<xsl:value-of select="$currentYear"/>
				</xsl:element>
				<xsl:element name="dc-element">
					<xsl:attribute name="xml:lang">
						<!-- xsl:value-of select="$lang"/ -->
						<xsl:text>en</xsl:text>
					</xsl:attribute>
					<xsl:attribute name="name">dc.date.electronic</xsl:attribute>
					<xsl:value-of select="$currentYear"/>
					<!-- Note: Jim A. used the value, "electronic © 2010". -->
				</xsl:element>
				<xsl:element name="dc-element">
					<xsl:attribute name="xml:lang">
						<!-- xsl:value-of select="$lang"/ -->
						<xsl:text>en</xsl:text>
					</xsl:attribute>
					<xsl:attribute name="name">dc.type:lbx-types</xsl:attribute>
					<xsl:text>text.monograph.bible</xsl:text>
				</xsl:element>
				<xsl:element name="dc-element">
					<xsl:attribute name="xml:lang">
						<!-- xsl:value-of select="$lang"/ -->
						<xsl:text>en</xsl:text>
					</xsl:attribute>
					<xsl:attribute name="name">dc.creator.corporatename</xsl:attribute>
					<!-- TODO: Determine how to find the creator. -->
					<xsl:text>SIL International</xsl:text>
				</xsl:element>
			</xsl:element> <!-- dc-metadata -->
			<!-- data-types -->
			<xsl:element name="data-types">
				<xsl:element name="data-type">
					<xsl:attribute name="name">bible</xsl:attribute>
					<xsl:attribute name="subtypes">yes</xsl:attribute>
					<xsl:attribute name="keylink">yes</xsl:attribute>
					<xsl:attribute name="breadth">90</xsl:attribute>
					<xsl:attribute name="depth">90</xsl:attribute>
					<xsl:attribute name="ease">90</xsl:attribute>
				</xsl:element> <!-- bible -->
				<xsl:element name="data-type">
					<xsl:attribute name="name">page</xsl:attribute>
					<xsl:attribute name="keylink">yes</xsl:attribute>
					<xsl:attribute name="breadth">50</xsl:attribute>
					<xsl:attribute name="depth">50</xsl:attribute>
					<xsl:attribute name="ease">50</xsl:attribute>
					<xsl:attribute name="relation">many-to-many</xsl:attribute>
				</xsl:element> <!-- page -->
				<xsl:element name="data-type">
					<xsl:attribute name="name">topic+topics</xsl:attribute>
					<xsl:attribute name="keylink">no</xsl:attribute>
					<xsl:attribute name="relation">many-to-many</xsl:attribute>
				</xsl:element> <!-- topic+topics -->
			</xsl:element> <!-- data-types -->
			<!-- active-data-types -->
			<xsl:element name="active-data-types">
				<xsl:element name="active-data-type">
					<xsl:attribute name="name">bible</xsl:attribute>
				</xsl:element>
				<xsl:element name="active-data-type">
					<xsl:attribute name="name">page</xsl:attribute>
				</xsl:element>
				<xsl:element name="active-data-type"/> <!-- spacer -->
				<xsl:element name="active-data-type">
					<xsl:attribute name="name">topic+topics</xsl:attribute>
				</xsl:element>
			</xsl:element>
			<!-- navigation-types -->
			<xsl:element name="navigation-types">
				<xsl:element name="navigation-type">
					<xsl:attribute name="name">article</xsl:attribute>
					<xsl:attribute name="default">yes</xsl:attribute>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>Article</xsl:text>
					</xsl:element>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">es</xsl:attribute>
						<xsl:text>Artículo</xsl:text>
					</xsl:element>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>Artikel</xsl:text>
					</xsl:element>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">pt</xsl:attribute>
						<xsl:text>Artigo</xsl:text>
					</xsl:element>
				</xsl:element>
				<xsl:element name="navigation-type">
					<xsl:attribute name="name">bible-book</xsl:attribute>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>Bible Book</xsl:text>
					</xsl:element>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">es</xsl:attribute>
						<xsl:text>Libro bíblico</xsl:text>
					</xsl:element>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>Bibelbuch</xsl:text>
					</xsl:element>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">pt</xsl:attribute>
						<xsl:text>Libro da Bíblia</xsl:text>
					</xsl:element>
				</xsl:element>
				<xsl:element name="navigation-type">
					<xsl:attribute name="name">bible-chapter</xsl:attribute>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>Bible Chapter</xsl:text>
					</xsl:element>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">es</xsl:attribute>
						<xsl:text>Capítulo bíblico</xsl:text>
					</xsl:element>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>Bibelkapitel</xsl:text>
					</xsl:element>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">pt</xsl:attribute>
						<xsl:text>Capítulo da Bíblia</xsl:text>
					</xsl:element>
				</xsl:element>
			</xsl:element> <!-- navigation-types -->
			<!-- controls -->
			<xsl:element name="controls">
				<xsl:element name="control">
					<xsl:attribute name="name">display</xsl:attribute>
				</xsl:element>
				<xsl:element name="control">
					<xsl:attribute name="name">locator</xsl:attribute>
				</xsl:element>
				<xsl:element name="control">
					<xsl:attribute name="name">contents</xsl:attribute>
					<xsl:element name="options">
						<xsl:element name="option">
							<xsl:attribute name="name">auto-sync</xsl:attribute>
						</xsl:element>
					</xsl:element>
				</xsl:element>
				<xsl:element name="control">
					<xsl:attribute name="name">popup</xsl:attribute>
				</xsl:element>
			</xsl:element> <!-- controls -->
			<!-- view-panes -->
			<xsl:element name="view-panes">
				<xsl:element name="view-pane">
					<xsl:attribute name="control">display</xsl:attribute>
				</xsl:element>
				<xsl:element name="view-pane">
					<xsl:attribute name="control">locator</xsl:attribute>
					<xsl:attribute name="visible">no</xsl:attribute>
				</xsl:element>
				<xsl:element name="view-pane">
					<xsl:attribute name="control">contents</xsl:attribute>
					<xsl:attribute name="visible">no</xsl:attribute>
				</xsl:element>
			</xsl:element> <!-- view-panes -->
			<!-- image -->
			<!-- TODO: Determine how to find the image. For now, use the default, Bible_1.jpg. -->
			<xsl:element name="image">
				<xsl:attribute name="type">front-cover</xsl:attribute>
				<!-- TODO: Determine how to make Bible_1.jpg available for bibles exported from TE. -->
				<xsl:attribute name="path">Images/Bible_1.jpg</xsl:attribute>
			</xsl:element>
			<!-- TODO: Determine how to find the copyright. -->
			<!-- copyright -->
			<xsl:element name="copyright">
				<xsl:attribute name="xml:lang">
						<!-- xsl:value-of select="$lang"/ -->
						<xsl:text>en</xsl:text>
				</xsl:attribute>
				<xsl:attribute name="type">standard</xsl:attribute>
				<!-- Note: The copyright date needs to be generated or to be a variable. -->
				<!-- "(c)" may need to be used, instead of "©". -->
				<xsl:text>Copyright (c) </xsl:text>
				<xsl:value-of select="$currentYear"/>
			</xsl:element> <!-- copyright -->
			<!-- about -->
			<xsl:element name="about">
				<xsl:attribute name="xml:lang">
					<!-- xsl:value-of select="$lang"/ -->
					<xsl:text>en</xsl:text>
				</xsl:attribute>
				<xsl:element name="div">
					<!-- The XSLT processor raises the error: "File C:\SIL\digpub\tw\TE_XHTML to Libronix\TE_XHTML-to-Libronix_Metadata.xslt: XSL transformation failed. Illegal attribute name. -->
					<!-- xsl:attribute name="xmlns">http://www.w3.org/1999/xhtml</xsl:attribute -->
					<xsl:element name="p">
						<xsl:value-of select="$descriptionContent"/>
						<!-- If the description does not end in a period, add one. -->
						<xsl:if test="not(substring($descriptionContent, (string-length($descriptionContent) - string-length($descriptionContent))+1) = '.')">
							<xsl:text>.</xsl:text>
						</xsl:if>
					</xsl:element>
				</xsl:element>
			</xsl:element> <!-- about -->
			<!-- Jim Albright gets the <marc-record> from the Library of Congress if they have it.
				Otherwise, he skips it. Books exported from TE, of course, have not been otherwise published. -->
			<!-- fields -->
			<xsl:element name="fields">
				<!-- field: bible -->
				<xsl:element name="field">
					<xsl:attribute name="name">bible</xsl:attribute>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>Bible Text</xsl:text>
					</xsl:element>
					<xsl:element name="alias">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>Bible</xsl:text>
					</xsl:element>
					<xsl:element name="desc">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>The actual text of the Bible verses, without introductions, headings, etc.</xsl:text>
					</xsl:element>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>Bibel Text</xsl:text>
					</xsl:element>
					<xsl:element name="alias">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>Bibel</xsl:text>
					</xsl:element>
					<xsl:element name="alias">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>BibelText</xsl:text>
					</xsl:element>
					<xsl:element name="desc">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>Der eigentliche Bibeltext, ohne Einleitungen, Überschriften etc.</xsl:text>
					</xsl:element>
				</xsl:element> <!-- field: bible -->
				<!-- field: surface -->
				<xsl:element name="field">
					<xsl:attribute name="name">surface</xsl:attribute>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>Surface Text</xsl:text>
					</xsl:element>
					<xsl:element name="alias">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>Surface</xsl:text>
					</xsl:element>
					<xsl:element name="alias">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>SurfaceText</xsl:text>
					</xsl:element>
					<xsl:element name="desc">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>Text that is visible using default view settings; in interlinears, the top line using default view settings.</xsl:text>
					</xsl:element>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>Oberflächen-Text</xsl:text>
					</xsl:element>
					<xsl:element name="alias">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>Oberfläche</xsl:text>
					</xsl:element>
					<xsl:element name="alias">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>OberflächenText</xsl:text>
					</xsl:element>
					<xsl:element name="desc">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>Text der angezeigt wird, wenn die Voreinstellungen für die Anzeige verwendet werden. In Interlinear-Ausgaben, die voreingestellte obere Zeile.</xsl:text>
					</xsl:element>
				</xsl:element> <!-- field: surface -->
				<!-- field: footnote -->
				<xsl:element name="field">
					<xsl:attribute name="name">footnote</xsl:attribute>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>Footnote Text</xsl:text>
					</xsl:element>
					<xsl:element name="alias">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>Footnote</xsl:text>
					</xsl:element>
					<xsl:element name="alias">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>FootnoteText</xsl:text>
					</xsl:element>
					<xsl:element name="desc">
						<xsl:attribute name="xml:lang">en</xsl:attribute>
						<xsl:text>Text that appears in a footnote.</xsl:text>
					</xsl:element>
					<xsl:element name="title">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>Fußnoten-Text</xsl:text>
					</xsl:element>
					<xsl:element name="alias">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>Fußnote</xsl:text>
					</xsl:element>
					<xsl:element name="alias">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>Fußnoten</xsl:text>
					</xsl:element>
					<xsl:element name="alias">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>FußnotenText</xsl:text>
					</xsl:element>
					<xsl:element name="desc">
						<xsl:attribute name="xml:lang">de</xsl:attribute>
						<xsl:text>Text, der in einer Fußnote steht.</xsl:text>
					</xsl:element>
				</xsl:element> <!-- field: footnote -->
			</xsl:element> <!-- fields -->
		</xsl:element> <!-- metadata -->
		<!-- build -->
		<xsl:element name="build">
			<!-- xsl:element name="properties"/ -->
			<xsl:element name="resource-files">
				<xsl:element name="stylesheets">
					<xsl:element name="file">
						<xsl:attribute name="src">
							<xsl:text>SL_BI_</xsl:text>
							<xsl:value-of select="$CssFilename" />
							<xsl:text>-Styles.xml</xsl:text>
						</xsl:attribute>
					</xsl:element>
				</xsl:element> <!-- stylesheets -->
				<xsl:element name="content">
					<xsl:element name="file">
						<xsl:attribute name="src">
							<xsl:text>SL_BI_</xsl:text>
							<xsl:value-of select="$CssFilename" />
							<xsl:text>-Content.xml</xsl:text>
						</xsl:attribute>
					</xsl:element>
				</xsl:element> <!-- content -->
				<xsl:element name="popups">
					<xsl:element name="file">
						<xsl:attribute name="src">
							<xsl:text>SL_BI_</xsl:text>
							<xsl:value-of select="$CssFilename" />
							<xsl:text>-Popups.xml</xsl:text>
						</xsl:attribute>
					</xsl:element>
				</xsl:element> <!-- popups -->
				<xsl:element name="baggage">
					<xsl:element name="file">
						<xsl:attribute name="src">
							<!-- TODO: determine how to find the image. -->
							<!-- Note: Does this path work? -->
							<xsl:text>Images/Bible_1.jpg</xsl:text>
						</xsl:attribute>
					</xsl:element>
				</xsl:element> <!-- baggage -->
			</xsl:element> <!-- resource-files -->
		</xsl:element> <!-- build -->
	</xsl:template> <!-- xhtml:head -->

	<!-- Default element and attribute templates. -->
	<xsl:template match="*">
		<xsl:comment>Warning :: The element "<xsl:value-of select="name()"/>", child of "<xsl:value-of select="name(..)"/>", has no matching template.</xsl:comment>
	</xsl:template>
	<!-- default attribute template -->
	<xsl:template match="@*">
		<xsl:comment>Warning :: The attribute "<xsl:value-of select="name()"/>" for element "<xsl:value-of select="name(..)"/>" has no matching template.</xsl:comment>
	</xsl:template>

</xsl:stylesheet>