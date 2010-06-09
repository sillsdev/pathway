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
	<output encoding="UTF-8" method="xml" media-type="text/xml" indent="yes"/>
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

	<variable name="lowercase">abcdefghijklmnopqrstuvwxyz</variable>
	<variable name="uppercase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</variable>
	<template match="lift">
		<element name="lift">
			<call-template name="LiftRoot"> </call-template>
			<apply-templates/>
		</element>
	</template>
	<template name="LiftRoot">
		<attribute name="producer">
			<value-of select="'SIL.FLEx 2.5.0.39988'"/>
		</attribute>
		<attribute name="version">
			<value-of select="0.13"/>
		</attribute>
		
	</template>
	<template name="fnsense">
		<param name="pentry"/>
		<param name="selection"></param>
		<variable name="sexist">
			<!-- <for-each select="$pentry//form[@lang]"> -->
			<for-each select="$selection"> 
				<choose>
					<!-- <when test="@lang  = 'en'  "> -->					
					<when test="LangFltParam ">
						<value-of select="'true'"/>
					</when>
				</choose>
			</for-each>
		</variable>
		<value-of select="$sexist"/>
	</template>
	
	<template match="entry">
		<variable name="senseFiltered1">
			<call-template name="fnsense">
				<with-param name="pentry" select="."/>
				<with-param name="selection" select=".//sense/definition/form"></with-param>
			</call-template>
		</variable>
		<if test="starts-with($senseFiltered1,'true' )">
			<element name="entry">
				<copy-of select="@*"/>
				<apply-templates select="./*[not(sense)]"/>
			</element>
		</if>
	</template>

	<template match="*/*[not(sense)]">

		<if test="local-name() != 'sense' ">
			<copy-of select="."/>
		</if>

		<if test="local-name() = 'sense' ">

			<variable name="senseDefinition">
				<call-template name="fnsense">
					<with-param name="pentry" select="./definition"/>
					<with-param name="selection" select=".//form"></with-param>
				</call-template>
			</variable>
			
			<if test="starts-with($senseDefinition,'true' )">
			<element name="sense">
				<copy-of select="@*"/>
				<element name="definition">
					<copy-of select="./definition[@*]"/>
					<call-template name="printdefinition">
						<with-param name="pentry" select=".//definition/form"/>
					</call-template>
				</element>
				<call-template name="notdefinition">
					<with-param name="pentry" select="."/>
				</call-template>
			</element>
			</if>
		</if>
	</template>
	<template name="notdefinition">
		<param name="pentry"/>

		<for-each select="$pentry/*">
			<if test="local-name() != 'definition' ">
				<copy-of select="."/>
			</if>
		</for-each>
	</template>
	
	<template name="printdefinition">
		<param name="pentry"/>
		<for-each select="$pentry">
			<!--
				<if test="@lang != 'en' ">
				<copy-of select="."/>
				</if>
				<if test="@lang = 'en' ">
			-->
			
			<if test="LangFltParam ">
				<copy-of select="."/>
			</if>
		</for-each>
	</template>
	<template name="CopyAttribs">
		<param name="attrib"></param>
		
		<attribute name="$attrib//{local-name(.)}">
			<value-of select="."/>
		</attribute>
	</template>
</stylesheet>
