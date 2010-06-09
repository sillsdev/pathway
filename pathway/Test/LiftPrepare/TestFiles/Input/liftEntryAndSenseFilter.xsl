<?xml version="1.0" encoding="ISO-8859-1"?>
<!--        #############################################################
	# Name:        liftEntryFilter.xsl
	# Purpose:     Given the LIFT XML dictionary content, filter entries and senses based on customizable criteria.
	#
	# Author:      David Robbins <dave_robbins@sil.org>
	#
	# Created:     2009/0608
	# RCS-ID:      $Id: liftFilter.xsl $
	# Copyright:   (c) 2009 SIL
	# Licence:     <LPGL>
	################################################################-->
<stylesheet version="1.0" xmlns="http://www.w3.org/1999/XSL/Transform" >
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

	<template name="filterEntry">
		<param name="entry"/>
		<choose>
			<when test="false()"><value-of select="'true'" /></when>
			<otherwise><value-of select="'false'"/></otherwise>
		</choose>
	</template>

	<template name="filterSense">
		<param name="sense"/>
		<choose>
			<when test="false()"><value-of select="'true'" /></when>
			<otherwise><value-of select="'false'"/></otherwise>
		</choose>
	</template>
	
	<!--
	Layout Templates
	-->
	
	<template match="lift">
		<element name="lift">
			<apply-templates select="@*"/>
			<apply-templates/>
		</element>		
	</template>

	<template match="entry">
		<variable name="entryFiltered">
			<call-template name="filterEntry">
				<with-param name="entry" select="."/>
			</call-template>
		</variable>
		<if test="$entryFiltered = 'false'">
			<!-- Don't print the entry if there are no un-filtered senses-->
			<variable name="senses">
				<apply-templates  select="sense" />
			</variable>
			<if test="not($senses = '')">
				<element name="entry">
					<apply-templates select="./*[not(sense)]"/>
					<copy-of select="$senses" />
				</element>
			</if>
		</if>
	</template>

	<template match="sense">
		<variable name="senseFiltered">
			<call-template name="filterSense">
				<with-param name="sense" select="."/>
			</call-template>
		</variable>
		<if test="$senseFiltered = 'false'">
			<copy-of select="."/>
		</if>
	</template>

	<template match="*/*[not(entry)]">
		<copy-of select="."/>
	</template>

	<template match="*/*[not(sense)]">
		<copy-of select="."/>
	</template>

	<template match="//@*">
		<attribute name="{local-name(.)}">
			<value-of select="."/>
		</attribute>
	</template>
	
</stylesheet>