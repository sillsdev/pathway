<stylesheet version="1.0" xmlns="http://www.w3.org/1999/XSL/Transform">
	<output encoding="UTF-8" method="xml" media-type="text/xml" indent="yes"/>

	<variable name="lowercase">abcdefghijklmnopqrstuvwxyz</variable>
	<variable name="uppercase">ABCDEFGHIJKLMNOPQRSTUVWXYZ</variable>

	<template match="lift">
		<element name="lift">
			<call-template name="LiftRoot"> </call-template>
			<apply-templates/>
		</element>
	</template>

	<template name="filterEntry">
		<param name="entry"/>
		<choose>
			<when test="EntFltParam">
				<value-of select="'true'"/>
			</when>
			<otherwise>
				<value-of select="'false'"/>
			</otherwise>
		</choose>
	</template>


	<template match="entry">
		<variable name="senseFiltered1">
			<call-template name="fnsense">
				<with-param name="pentry" select="."/>
			</call-template>
		</variable>

		<if test="starts-with($senseFiltered1,'true' )">
			<variable name="entryFiltered">
				<call-template name="filterEntry">
					<with-param name="entry" select="."/>
				</call-template>
			</variable>
			<if test="$entryFiltered = 'true'">
				<element name="entry">
					<apply-templates select="./*[not(sense)]"/>

					<call-template name="fnsenseprint">
						<with-param name="pentry" select="."/>
					</call-template>
				</element>
			</if>
		</if>

	</template>

	<template name="fnsense">
		<param name="pentry"/>
		<variable name="sexist">
			<for-each select="$pentry//sense">
				<variable name="senseno" select="position() "/>

				<choose>
					<when test="SenseFltParam">
						<value-of select="'true'"/>
					</when>
				</choose>
			</for-each>
		</variable>
		<value-of select="$sexist"/>
	</template>

	<template name="fnsenseprint">
		<param name="pentry"/>
		<for-each select="$pentry//sense">
			<variable name="senseno" select="position() "/>
			<if test="SenseFltParam">
				<copy-of select="$pentry/sense[$senseno]"/>
			</if>
		</for-each>
	</template>

	<template match="*/*[not(sense)]">

		<if test="local-name() != 'sense' ">
			<copy-of select="."/>
		</if>
	</template>

	<template name="LiftRoot">
		<attribute name="producer">
			<value-of select="'SIL.FLEx 2.5.0.39988'"/>
		</attribute>
		<attribute name="version">
			<value-of select="0.13"/>
		</attribute>

	</template>

</stylesheet>
