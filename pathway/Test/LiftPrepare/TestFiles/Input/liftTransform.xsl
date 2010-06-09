<?xml version="1.0" encoding="ISO-8859-1"?>
<!--        #############################################################
	# Name:        liftTransform.xsl
	# Purpose:     Given the LIFT XML dictionary content, transforms it 
	#			   to an XHTML dictionary.
	#
	# Author:      David Robbins <dave_robbins@sil.org>
	#
	# Created:     2009/0608
	# RCS-ID:      $Id: liftTransform.xsl $
	# Copyright:   (c) 2009 SIL
	# Licence:     <LPGL>
	################################################################-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"
		doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"
		encoding="UTF-8" method="xml"  media-type="text/xhtml" indent="yes" />
		
	<xsl:variable name="head">
		<head>
		<title></title>
		<link rel="stylesheet" type="text/css" href="../../Input/liftTransform.css"/>
		</head>
	</xsl:variable>
	
	<xsl:variable name="seperatedEntries">
		<xsl:call-template name="recursiveCategorize"><xsl:with-param name="entry" select="//entry[1]"/></xsl:call-template>
	</xsl:variable>

	<!-- Layout Templates -->
	
	<xsl:template name="recursiveCategorize">
		<xsl:param name="entry"/>
		<xsl:variable name="firstLetter" select="substring(translate($entry/lexical-unit/form/text, '=',''),1,1)"/>
		<xsl:call-template name="letHead">
			<xsl:with-param name="letter" select="$firstLetter"/>
		</xsl:call-template>
		<div class="letData">
			<xsl:call-template name="entry"><xsl:with-param name="entry" select="$entry"/></xsl:call-template>
			<xsl:for-each select="$entry/following::entry[substring(translate(lexical-unit/form/text, '=',''),1,1) = $firstLetter]">
				<xsl:call-template name="entry"><xsl:with-param name="entry" select="."/></xsl:call-template>
			</xsl:for-each>
		</div>
		<xsl:if test="$entry/following::*">
			<xsl:call-template name="recursiveCategorize">
				<xsl:with-param name="entry" select="$entry/following::entry[not(substring(translate(lexical-unit/form/text, '=',''),1,1) = $firstLetter)][1]"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="letHead">
		<xsl:param name="letter"/>
		<div class="letHead">
			<div class="letter"><xsl:value-of select="$letter" /></div>
		</div>
	</xsl:template>

	<xsl:template match="lift">
		<html>
			<xsl:copy-of select="$head"/>
			<body> 
				<xsl:copy-of select="$seperatedEntries"/>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="header"></xsl:template>

	<xsl:template name="entry">
		<xsl:param name="entry"/>
		<div class="entry" id="{$entry/@id}">
			<xsl:apply-templates select="$entry/sense/illustration"/>
			<xsl:apply-templates select="$entry/lexical-unit/form"/>
			<span class="pronunciations">
				<xsl:apply-templates select="$entry/pronunciation"/>
			</span>
			<xsl:if test="$entry/variant">
				<span class="variantrefs">
					<xsl:apply-templates select="$entry/variant"/>
				</span>
			</xsl:if>
			<span class="senses">
				<xsl:apply-templates select="$entry/sense"/>
			</span>
		</div>
	</xsl:template>

	<xsl:template match="sense">
		<span class="sense" id="{../@id}">
			<xsl:apply-templates select="grammatical-info"/>
			<!-- If there is no definition, display the gloss in its place.-->
			<xsl:if test="not(./definition)">
				<xsl:for-each select="./gloss">
					<span class="definition" lang="{@lang}">
						<xsl:value-of select="text"/>
					</span>
				</xsl:for-each>
			</xsl:if>
			<span class="definition">
				<xsl:apply-templates select="definition/form"/>
			</span>
			<span class="examples">
				<xsl:apply-templates select="example"/>
			</span>
			<!--Not all traits are necessarily semantic domains. e.g. Scientific Names-->
			<xsl:if test="contains(./trait/@name, 'semantic-domain')">
				<span class="semantic-domains">
					<xsl:for-each select="./trait[contains(@name, 'semantic-domain')]">
						<span class='xitem'>
							<span class="semantic-domain-abbr" lang="en"><xsl:value-of select="substring-before(@value, ' ')"/></span>
							<span class="semantic-domain-name" lang="en"><xsl:value-of select="substring-after(@value, ' ')"/></span>
						</span>
					</xsl:for-each>
				</span>
			</xsl:if>
			<xsl:apply-templates select="sense/note"/>
		</span>
	</xsl:template>

	<xsl:template match="illustration">
		<!--Becuse LIFT allows for optional <span></span> blocks in label/form/text, the following logic is necessary.-->
		<xsl:choose>
			<xsl:when test="./label/form/text/span">
				<xsl:variable name="lang" select="./label/form/text/span/@lang"/>
				<xsl:variable name="label" select="./label/form/text/span"/>
				<div class="pictureRight">
					<img class="picture" src="{concat('Pictures/', ./@href)}" alt="{$label}"/>
					<div class="pictureCaption">
						<span class="pictureSense"/>
						<span class="pictureLabel" lang="{$lang}">
							<span lang="{$lang}"><xsl:value-of select="$label"/></span>
						</span>
					</div>
				</div>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="lang" select="./label/form/@lang"/>
				<xsl:variable name="label" select="./label/form/text"/>
				<div class="pictureRight">
					<img class="picture" src="{concat('Pictures/', ./@href)}" alt="{$label}"/>
					<div class="pictureCaption">
						<span class="pictureSense"/>
						<span class="pictureLabel" lang="{$lang}">
							<span lang="{$lang}"><xsl:value-of select="$label"/></span>
						</span>
					</div>
				</div>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="pronunciation">
		<span class="pronunciation" lang="{form/@lang}"><xsl:value-of select="form/text"/></span>
	</xsl:template>

	<xsl:template match="example">
		<span class="xitem">
			<xsl:apply-templates select="./form"/>
			<span class="translations">
				<xsl:apply-templates select="translation/form"/>
			</span>
		</span>
	</xsl:template>

	<xsl:template match="example/form">
		<span class="example" lang="{@lang}"><xsl:value-of select="text"/></span>
	</xsl:template>

	<xsl:template match="translation/form">
		<span class="translation" lang="{@lang}"><xsl:value-of select="text"/></span>
	</xsl:template>

	<xsl:template match="grammatical-info">
		<span class="grammatical-info">
			<span class="partofspeech" lang="en">
				<xsl:choose>
					<xsl:when test="@value = 'Verb'">V</xsl:when>
					<xsl:when test="@value = 'Adverb'">Adv</xsl:when>
					<xsl:when test="@value = 'Adjective'">Adj</xsl:when>
					<xsl:when test="@value = 'Noun'">N</xsl:when>
					<xsl:when test="@value = 'Compound noun'">Cn</xsl:when>
					<xsl:when test="@value = 'Inalienable noun'">N(inal)</xsl:when>
					<xsl:when test="@value = 'Class 1 verbs'">C1</xsl:when>
					<xsl:when test="@value = 'Conjunction'">Conj</xsl:when>
					<xsl:when test="@value = 'call'">Call</xsl:when>
					<xsl:when test="@value = 'verb'">v</xsl:when>
					<xsl:when test="@value = 'adverb'">adv</xsl:when>
					<xsl:when test="@value = 'adjective'">adj</xsl:when>
					<xsl:when test="@value = 'noun'">n</xsl:when>
					<xsl:when test="@value = 'compound noun'">cn</xsl:when>
					<xsl:when test="@value = 'inalienable noun'">n(inal)</xsl:when>
					<xsl:when test="@value = 'class 1 verbs'">c1</xsl:when>
					<xsl:when test="@value = 'conjunction'">conj</xsl:when>
					<xsl:when test="@value = 'pronoun'">pron</xsl:when>
					<xsl:otherwise><xsl:value-of select="@value"/></xsl:otherwise>
				</xsl:choose>
			</span>
		</span>
	</xsl:template>

	<xsl:template match="variant">
		<span class="xitem">
			<span class="variantref-form" lang="form/@lang"><xsl:value-of select="form/text"/></span>
		</span>
	</xsl:template>

	<xsl:template match="definition/form">
		<span class="xitem" lang="{@lang}">
			<xsl:value-of select="text"/>
		</span>
	</xsl:template>

	<xsl:template match="sense/note">
		<span class="encyclopedic-info" lang="{form/@lang}">
			<xsl:value-of select="form/text"/>
		</span>
	</xsl:template>

	<xsl:template match="lexical-unit/form">
		<span class="headword" lang="{@lang}">
			<xsl:value-of select="text"/>
		</span>
	</xsl:template>

</xsl:stylesheet>