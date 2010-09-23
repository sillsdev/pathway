<?xml version="1.0" encoding="UTF-8"?>
<!-- Create the Libronix popups file from the given XHTML file exported from TE . -->

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml"
    xmlns:fn="http://www.w3.org/2005/xpath-functions"
    exclude-result-prefixes="xhtml">

    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

    <xsl:strip-space elements="*"/>

	<!-- Note: If scripture references can be discovered, they can be marked in the same way as the following example:
		<jump pos="jaa.114:jaa.EXO.3.2" class="green">Eksodas 3:2</jump> .
	-->

    <!-- Process the top element. -->
    <xsl:template match="xhtml:html">
        <xsl:element name="logos-resource-popups">
			<!-- xsl:attribute name="xmlns:xsi">http://www.w3.org/2001/XMLSchema-instance</xsl:attribute> -->
			<!-- xsi:noNamespaceSchemaLocation="x:\Schema\content.xsd">  -->
			<xsl:attribute name="xml:space">preserve</xsl:attribute>
			<xsl:attribute name="xml:lang">
				<xsl:value-of select="//xhtml:span[@class='scrBookName']/@lang"/>
			</xsl:attribute>
			<xsl:element name="popup-articles">
				<xsl:apply-templates select="//xhtml:span[@class='scrFootnoteMarker']"/>
			</xsl:element>
        </xsl:element> <!-- logos-resource-popups -->
    </xsl:template> <!-- xhtml:html -->

	<!-- There are two types of popups (footnotes):
		span class="Note_CrossHYPHENReference_Paragraph"
				id="FFE9776C5-20FA-46E8-A46B-8A926222C517" title="" 
		span class="Note_General_Paragraph"
				id="F00B24511-E96D-48FD-8CF1-23F8273CA3A3" title="†" 
	-->

	<xsl:template match="xhtml:span[@class='scrFootnoteMarker']">
		<xsl:variable name="href" select="child::xhtml:a/@href"/>
		<xsl:element name="popup-article">
			<xsl:attribute name="id">
				<xsl:value-of select="translate(substring($href,2),'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
			</xsl:attribute>
			<xsl:apply-templates select="following-sibling::*[1]"/>
		</xsl:element> <!-- popup-article -->
	</xsl:template> <!-- span[@class='scrFootnoteMarker'] -->

	<xsl:template match="xhtml:span[@class='Note_General_Paragraph' or @class='Note_CrossHYPHENReference_Paragraph']">
		<xsl:variable name="hrefOfPrecedingSibling" select="substring(preceding-sibling::*[1]/xhtml:a/@href,2)"/>
		<xsl:choose>
			<xsl:when test="@id = translate($hrefOfPrecedingSibling,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')">
				<xsl:element name="p">
					<xsl:attribute name="class">
						<xsl:value-of select="@class"/>
					</xsl:attribute>
					<!-- Process the enclosed spans. -->
					<xsl:apply-templates select="child::*"/>
				</xsl:element> <!-- p -->
			</xsl:when>
			<xsl:otherwise>
				<xsl:comment>
					<xsl:text>Warning: the preceding scrFootnmteMarker,s href [</xsl:text>
					<xsl:value-of select="$hrefOfPrecedingSibling"/>
					<xsl:text>] is not the same as the id [</xsl:text>
					<xsl:value-of select="@id"/>
					<xsl:text>] of this </xsl:text>
					<xsl:value-of select="@class"/>
					<xsl:text>.</xsl:text>
				</xsl:comment>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template> <!-- span[@class='Note_General_Paragraph'] -->

	<xsl:template match="xhtml:span[parent::xhtml:span[@class='Note_General_Paragraph' or @class='Note_CrossHYPHENReference_Paragraph']]">
		<xsl:element name="span">
			<xsl:if test="@class">
				<xsl:attribute name="class">
					<xsl:value-of select="@class"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="xml:lang">
				<xsl:value-of select="@lang" />
			</xsl:attribute>
			<xsl:value-of select="text()"/>
		</xsl:element> <!-- span -->
	</xsl:template> <!-- span with parent span[@class='Note_General_Paragraph'] or span[@class='Note_CrossHYPHENReference_Paragraph'] -->

	<!-- span[@class='scrFootnoteMarker'] -->
	<!-- This is always immediately followed by either a <span class="Note_General_Paragraph"> or a <span class="Note_CrossHYPHENReference_Paragraph">. -->
<!--	<xsl:template match="xhtml:span[@class='scrFootnoteMarker']">
		<xsl:element name="sup"> -->
			<!-- The reference begins with '#'. -->
		<!--	<xsl:variable name="href">
				<xsl:choose>
					<xsl:when test="starts-with(xhtml:a/@ref,'#')">
						<xsl:value-of select="substring-after(xhtml:a/@href,'#')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="xhtml:a/@href"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:element name="popup">
				<xsl:attribute name="pos"> -->
					<!-- The matching <span> for the footnote uses all capital letters.
						Therefore, change all small letters to capitol letters for the popup reference. -->
				<!--	<xsl:value-of select="translate($href,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
				</xsl:attribute>
			</xsl:element> --> <!-- popup -->
			<!-- Get the superscripted text. -->
			<!-- span class="Note_General_Paragraph" title="†" -->
			<!-- <span class="Note_CrossHYPHENReference_Paragraph">/span -->
		<!--	<xsl:choose> -->
				<!-- Is this a 'Note_General_Paragraph'? -->
			<!--	<xsl:when test="following-sibling::xhtml:span[1]/@class='Note_General_Paragraph' "> -->
					<!-- xsl:value-of select="following-sibling::xhtml:span/@title" / -->
					<!-- TODO: TE uses a cross, '†', here, rather than an asterisk. -->
				<!--	<xsl:text>*</xsl:text>
				</xsl:when> -->
				<!-- If not, it is a 'Note_CrossHYPHENReference_Paragraph'. -->
			<!--	<xsl:otherwise> -->
					<!-- TODO: TE uses a double-box symbol here, rather than an asterisk. -->
			<!--		<xsl:text>*</xsl:text>
				</xsl:otherwise>
			</xsl:choose> -->
			<!-- If the next div has the class '\f*', process the text now. -->
		<!--	<xsl:if test="following-sibling::xhtml:span[1][@class='Note_General_Paragraph' or @class='Note_CrossHYPHENReference_Paragraph'][count(following-sibling::*)=0]">
				<xsl:if test="parent::*/following-sibling::xhtml:div[1][@class='\f*']">
					<xsl:apply-templates select="parent::*/following-sibling::xhtml:div[1][@class='\f*']/child::*"/>
				</xsl:if>
			</xsl:if>
		</xsl:element> --> <!-- sup -->
<!--	</xsl:template> --> <!-- span[@class='scrFootnoteMarker'] -->

	<!-- Skip these for now. -->
<!--	<xsl:template match="xhtml:span[@class='Chapter_Number']"/> -->

	<!-- Default element template. -->
	<xsl:template match="*">
		<xsl:comment>Warning :: The element "<xsl:value-of select='name()'/>" with class "<xsl:value-of select='@class'/>", child of "<xsl:value-of select="name(..)"/>" with class "<xsl:value-of select='.././@class'/>", has no matching template.</xsl:comment>
	</xsl:template>
	<!-- Default attribute template. -->
	<xsl:template match="@*">
		<xsl:comment>Warning :: The attribute "<xsl:value-of select="name()"/>" for element "<xsl:value-of select="name(..)"/>"  has no matching template.</xsl:comment>
	</xsl:template>

</xsl:stylesheet>