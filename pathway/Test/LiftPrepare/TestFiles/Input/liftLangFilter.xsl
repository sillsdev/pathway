<?xml version="1.0" encoding="ISO-8859-1"?>
<!--        #############################################################
	# Name:        liftFilt.xsl
	# Purpose:     Given the LIFT XML dictionary content, filter and prepare XHTML.
	#
	# Author:      David Robbins <david_robbins@sil.org>
	#
	# Created:     2009/0608
	# RCS-ID:      $Id: liftFilter.xsl $
	# Copyright:   (c) 2009 SIL
	# Licence:     <LPGL>
	################################################################-->
<stylesheet version="1.0" xmlns="http://www.w3.org/1999/XSL/Transform">
	<output encoding="UTF-8" method="xml"  media-type="text/xml" indent="yes" />
	<!--
	Filter Templates

	The following templates act as psuedo-functions. Each contains a switch 
	statement that enables filtering of elements at a given node level or in the 
	tree below it.

	Use <value-of select="'true'"/> to filter, or remove, an entry
	and <value-of select="'false'"/> to ensure it remains.

	The most specific conditions should go first, as those will be given priority.

	Entry Sample Conditions
	 
	Only Certain Parts of Speech:
	sense/grammatical-info/@value = 'Noun'

	Start's with 'b':
	starts-with({lexical-unit/form/text}, 'b')
	-->
	<template name="filterLang">
		<param name="lang"/>
		<choose>
			<when test = "contains($lang, 'tpi')">
				<value-of select="'true'"/>
			</when>
			
			<!--<when test="$lang = 'ii'"><value-of select="'true'"/></when>-->			
			<when test="false()"><value-of select="'true'"/></when>
			<otherwise><value-of select="'false'"/></otherwise>
		</choose>
	</template>
	
	<!-- Layout Templates -->
	
	<template match="lift">
		<element name="lift">
			<apply-templates select="@*"/>
			<apply-templates/>
		</element>		
	</template>

	<template match="*[descendant::*[@lang]]">
		<element name="{local-name(.)}">
			<apply-templates select="@*"/>
			<apply-templates/>
		</element>
	</template>

	<template match="*[not(descendant::*[@lang])]">
		<copy-of select="."/>
	</template>

	<template match="*[@lang]">
		<variable name="langFiltered">
			<call-template name="filterLang">
				<with-param name="lang" select="@lang"/>
			</call-template>
		</variable>
		<if test="$langFiltered = 'false'">
			<element name="{local-name(.)}">
				<apply-templates select="@*"/>
				<apply-templates/>
			</element>
		</if>
	</template>

	<template match="//@*">
		<attribute name="{local-name(.)}">
			<value-of select="."/>
		</attribute>
	</template>

</stylesheet>