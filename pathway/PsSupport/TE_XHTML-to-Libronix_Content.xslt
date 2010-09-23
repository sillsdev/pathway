<?xml version="1.0" encoding="UTF-8"?>
<!-- Create the Libronix main file from the given XHTML file exported from TE . -->

<!-- Note the use of "document('FileGuids.xml')" below. It is assumed that the file 'FileGuids.xml' is in the same directory as this XSLT file. -->

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xhtml="http://www.w3.org/1999/xhtml"
    xmlns:fn="http://www.w3.org/2005/xpath-functions"
    exclude-result-prefixes="xhtml">

    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

    <xsl:strip-space elements="*"/>

	<xsl:variable name="CssFilenameAndExtension" select="xhtml:html/xhtml:head/xhtml:link[@rel='stylesheet']/@href" />
	<!-- Get the filename without the extension. -->
	<xsl:variable name="CssFilename" select="substring-before($CssFilenameAndExtension,'.css')" />

	<xsl:variable name="lang">
		<xsl:value-of select="//xhtml:body[@class='scrBody']/xhtml:div[@class='scrBook'][1]/xhtml:span[@class='scrBookName']/@lang"/>
	</xsl:variable>

	<!-- Collection of book names from the given file. -->
	<xsl:variable name="scrBookNames" select="//xhtml:body[@class='scrBody']/xhtml:div[@class='scrBook']/xhtml:span[@class='scrBookName']"/>

	<!-- ToDo: [Steve] Modify TE export to XHTML to include the Book Code for a given Bible book.
		See "New Testament book codes" in the help files for FW Translation Editor. Also for Old Testament book codes. -->
	<xsl:template name="getBookNumberFromCode">
		<xsl:param name="bookCode"/>
		<xsl:choose>
			<!-- Book codes for Old Testament books. -->
			<xsl:when test="$bookCode = 'GEN' "><xsl:text>1</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'EXO' "><xsl:text>2</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'LEV' "><xsl:text>3</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'NUM' "><xsl:text>4</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'DEU' "><xsl:text>5</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JOS' "><xsl:text>6</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JDG' "><xsl:text>7</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'RUT' "><xsl:text>8</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1SA' "><xsl:text>9</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2SA' "><xsl:text>10</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1KI' "><xsl:text>11</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2KI' "><xsl:text>12</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1CH' "><xsl:text>13</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2CH' "><xsl:text>14</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'EZR' "><xsl:text>15</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'NEH' "><xsl:text>16</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'EST' "><xsl:text>17</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JOB' "><xsl:text>18</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'PSA' "><xsl:text>19</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'PRO' "><xsl:text>20</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'ECC' "><xsl:text>21</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'SNG' "><xsl:text>22</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'ISA' "><xsl:text>23</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JER' "><xsl:text>24</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'LAM' "><xsl:text>25</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'EZK' "><xsl:text>26</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'DAN' "><xsl:text>27</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'HOS' "><xsl:text>28</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JOL' "><xsl:text>29</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'AMO' "><xsl:text>30</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'OBA' "><xsl:text>31</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JON' "><xsl:text>32</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'MIC' "><xsl:text>33</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'NAM' "><xsl:text>34</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'HAB' "><xsl:text>35</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'ZEP' "><xsl:text>36</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'HAG' "><xsl:text>37</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'ZEC' "><xsl:text>38</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'MAL' "><xsl:text>39</xsl:text></xsl:when>
			<!-- Book codes for New Testament books. -->
			<xsl:when test="$bookCode = 'MAT' "><xsl:text>61</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'MRK' "><xsl:text>62</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'LUK' "><xsl:text>63</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JHN' "><xsl:text>64</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'ACT' "><xsl:text>65</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'ROM' "><xsl:text>66</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1CO' "><xsl:text>67</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2CO' "><xsl:text>68</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'GAL' "><xsl:text>69</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'EPH' "><xsl:text>70</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'PHP' "><xsl:text>71</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'COL' "><xsl:text>72</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1TH' "><xsl:text>73</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2TH' "><xsl:text>74</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1TI' "><xsl:text>75</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2TI' "><xsl:text>76</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'TIT' "><xsl:text>77</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'PHM' "><xsl:text>78</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'HEB' "><xsl:text>79</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JAS' "><xsl:text>80</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1PE' "><xsl:text>81</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2PE' "><xsl:text>82</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1JN' "><xsl:text>83</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2JN' "><xsl:text>84</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '3JN' "><xsl:text>85</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JUD' "><xsl:text>86</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'REV' "><xsl:text>87</xsl:text></xsl:when>
			<!-- Return an empty string by default. -->
			<xsl:otherwise>
				<xsl:message>
					<xsl:text>The book code "</xsl:text>
					<xsl:value-of select="$bookCode"/>
					<xsl:text>" is not recognized.</xsl:text>
				</xsl:message>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template> <!-- getBookNumberFromCode -->

	<!-- Book abbreviations come from the Libronix resource, "The Book Design Process (Contractor). Or, see "http://www.logos.com/support/lbs/booknames". -->

	<!-- This is also dependent on Steve's modifying TE export to XHTML to include the Book Code for a given Bible book. -->
	<xsl:template name="getBookAbbrevFromCode">
		<xsl:param name="bookCode"/>
		<xsl:choose>
			<xsl:when test="$bookCode = 'GEN' "><xsl:text>Ge</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'EXO' "><xsl:text>Ex</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'LEV' "><xsl:text>Le</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'NUM' "><xsl:text>Nu</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'DEU' "><xsl:text>Dt</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JOS' "><xsl:text>Jos</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JDG' "><xsl:text>Jdg</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'RUT' "><xsl:text>Ru</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1SA' "><xsl:text>1Sa</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2SA' "><xsl:text>2Sa</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1KI' "><xsl:text>1Ki</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2KI' "><xsl:text>2Ki</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1CH' "><xsl:text>1Ch</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2CH' "><xsl:text>2Ch</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'EZR' "><xsl:text>Ezr</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'NEH' "><xsl:text>Ne</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'EST' "><xsl:text>Es</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JOB' "><xsl:text>Job</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'PSA' "><xsl:text>Ps</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'PRO' "><xsl:text>Pr</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'ECC' "><xsl:text>Ec</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'SNG' "><xsl:text>So</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'ISA' "><xsl:text>Is</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JER' "><xsl:text>Je</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'LAM' "><xsl:text>La</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'EZK' "><xsl:text>Eze</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'DAN' "><xsl:text>Da</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'HOS' "><xsl:text>Ho</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JOL' "><xsl:text>Joe</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'AMO' "><xsl:text>Am</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'OBA' "><xsl:text>Ob</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JON' "><xsl:text>Jon</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'MIC' "><xsl:text>Mic</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'NAM' "><xsl:text>Na</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'HAB' "><xsl:text>Hab</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'ZEP' "><xsl:text>Zep</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'HAG' "><xsl:text>Hag</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'ZEC' "><xsl:text>Zec</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'MAL' "><xsl:text>Mal</xsl:text></xsl:when>
			<!-- Specify codes for other Old Testament books. -->
			<xsl:when test="$bookCode = 'MAT' "><xsl:text>Mt</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'MRK' "><xsl:text>Mk</xsl:text></xsl:when>
			<!-- The Bughotu scriptures from TE also use "MAK" as a code for Mark. -->
			<xsl:when test="$bookCode = 'MAK' "><xsl:text>Mk</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'LUK' "><xsl:text>Lk</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JHN' "><xsl:text>Jn</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'ACT' "><xsl:text>Ac</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'ROM' "><xsl:text>Ro</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1CO' "><xsl:text>1Co</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2CO' "><xsl:text>2Co</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'GAL' "><xsl:text>Ga</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'EPH' "><xsl:text>Eph</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'PHP' "><xsl:text>Php</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'COL' "><xsl:text>Col</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1TH' "><xsl:text>1Th</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2TH' "><xsl:text>2Th</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1TI' "><xsl:text>1Ti</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2TI' "><xsl:text>2Ti</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'TIT' "><xsl:text>Tt</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'PHM' "><xsl:text>Phm</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'HEB' "><xsl:text>Heb</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JAS' "><xsl:text>Jas</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1PE' "><xsl:text>1Pe</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2PE' "><xsl:text>2Pe</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '1JN' "><xsl:text>1Jn</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '2JN' "><xsl:text>2Jn</xsl:text></xsl:when>
			<xsl:when test="$bookCode = '3JN' "><xsl:text>3Jn</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'JUD' "><xsl:text>Jud</xsl:text></xsl:when>
			<xsl:when test="$bookCode = 'REV' "><xsl:text>Re</xsl:text></xsl:when>
			<!-- Return an empty string by default. -->
			<xsl:otherwise>
				<xsl:message>
					<xsl:text>The book code "</xsl:text>
					<xsl:value-of select="$bookCode"/>
					<xsl:text>" is not recognized.</xsl:text>
				</xsl:message>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template> <!-- getBookAbbrevFromCode -->

    <!-- Process the top element. -->
    <xsl:template match="xhtml:html">
        <xsl:element name="logos-resource-content">
			<!-- xsl:attribute name="xmlns:xsi">http://www.w3.org/2001/XMLSchema-instance</xsl:attribute> -->
			<!-- xsi:noNamespaceSchemaLocation="x:\Schema\content.xsd">  -->
			<xsl:attribute name="xml:space">preserve</xsl:attribute>
			<xsl:attribute name="xml:lang">
				<xsl:value-of select="$lang" />
			</xsl:attribute>
			<xsl:element name="articles">
				<!-- The first article should be a title page. -->
				<xsl:element name="article">
					<xsl:attribute name="id">TITLE</xsl:attribute>
					<xsl:element name="toc-entry">
						<xsl:attribute name="level">1</xsl:attribute>
						<xsl:element name="span">
							<xsl:attribute name="xml:lang">
								<xsl:value-of select="$lang"/>
							</xsl:attribute>
							<xsl:value-of select="xhtml:head/xhtml:title/text()"/>
						</xsl:element>
					</xsl:element> <!-- toc-entry -->
					<xsl:element name="p">
						<xsl:attribute name="class">Title_Main</xsl:attribute>
						<xsl:attribute name="xml:lang">
							<xsl:value-of select="$lang" />
						</xsl:attribute>
						<xsl:value-of select="xhtml:head/xhtml:title/text()"/>
					</xsl:element>
				</xsl:element> <!-- article -->
				<xsl:apply-templates/>
			</xsl:element> <!-- articles -->
        </xsl:element> <!-- logos-resource-content -->
    </xsl:template> <!-- xhtml:html -->

	<!-- skip <head>, which is processed with the metadata -->
	<xsl:template match="xhtml:head"/>
	
	<!-- body[@class='scrBody'] -->
	<xsl:template match="xhtml:body">
		<xsl:apply-templates/>
	</xsl:template>
	
	<!-- div[@class='scrBook'] (book of the Bible) -->
	<!-- children of div[@class='scrBook']:
			<span class="scrBookName" lang="bgt">Sen Matiu Ke Risoa</span>
			<div class="Title_Main">
				<span lang="bgt">Sen Matiu Ke Risoa</span>
			</div>
			<div class="scrIntroSection">
			<div class="scrIntroSection">
			<div class="columns">
	-->
	<xsl:template match="xhtml:div[@class='scrBook']">
		<!-- ToDo: Does the span[@class='scrBookName'] need to be processed, since its contents are the same as div[@class='Title_Main']/span? -->
		<xsl:variable name="title" select="xhtml:div[@class='Title_Main']/xhtml:span/text()"/>
		<xsl:variable name="bookCode" select="xhtml:span[@class='scrBookCode']/text()"/>
		<xsl:element name="article">
			<xsl:attribute name="id">
				<xsl:text>CH</xsl:text>
				<xsl:call-template name="getBookNumberFromCode">
					<xsl:with-param name="bookCode" select="$bookCode"/>
				</xsl:call-template>
				<xsl:text>.0</xsl:text>
			</xsl:attribute>
			<xsl:element name="toc-entry">
				<xsl:attribute name="level">2</xsl:attribute>
				<xsl:element name="span">
					<xsl:attribute name="xml:lang">
						<xsl:value-of select="$lang"/>
					</xsl:attribute>
					<xsl:value-of select="$title"/>
				</xsl:element>
			</xsl:element> <!-- toc-entry -->
			<!-- Process the Title_Main here. -->
			<xsl:element name="p">
				<xsl:attribute name="class">Title_Main</xsl:attribute>
				<xsl:attribute name="xml:lang">
					<xsl:value-of select="$lang"/>
				</xsl:attribute>
				<xsl:element name="sync">
					<xsl:attribute name="ref">
						<xsl:text>bible.</xsl:text>
						<xsl:call-template name="getBookNumberFromCode">
							<xsl:with-param name="bookCode" select="$bookCode"/>
						</xsl:call-template>
						<xsl:text>.1.1</xsl:text>
					</xsl:attribute>
				</xsl:element>
				<xsl:value-of select="xhtml:span/text()"/>
			</xsl:element> <!-- p -->
		</xsl:element> <!-- article -->
		<!-- First, process the content before the verses contained in div[@class='columns']. -->
		<xsl:apply-templates select="child::*[not(@class='columns')]"/>
		<!-- Then, process the verses. -->
		<xsl:apply-templates select="xhtml:div[@class='columns']"/>
	</xsl:template> <!-- div[@class='scrBook'] -->

	<!-- skip <span[@class='scrBookName']> -->
	<xsl:template match="xhtml:span[@class='scrBookName']"/>
	<xsl:template match="xhtml:span[@class='scrBookCode']"/>
	
	<!-- skip div[@class='Title_Main'] -->
	<!-- Process the Title_Main in the template for div[@class='scrBook']. -->
	<xsl:template match="xhtml:div[@class='Title_Main']"/>

	<!-- div[@class='scrIntroSection'] -->
	<!-- Pick the first sibling. -->
	<xsl:template match="xhtml:div[@class='scrIntroSection'][not(preceding-sibling::xhtml:div[@class='scrIntroSection'])]">
		<xsl:variable name="title" select="xhtml:div[@class='Intro_Section_Head']/xhtml:span/text()"/>
		<xsl:element name="article">
			<xsl:attribute name="id">
				<xsl:text>INTRO</xsl:text>
				<xsl:call-template name="getBookNumberFromCode">
					<xsl:with-param name="bookCode" select="preceding-sibling::xhtml:span[@class='scrBookCode']/text()"/>
				</xsl:call-template>
				<xsl:text>.1</xsl:text>
			</xsl:attribute>
			<xsl:element name="toc-entry">
				<xsl:attribute name="level">3</xsl:attribute>
				<!-- xsl:attribute name="main">no</xsl:attribute -->
				<xsl:element name="span">
					<xsl:attribute name="xml:lang">
						<xsl:value-of select="$lang"/>
					</xsl:attribute>
					<xsl:value-of select="$title"/>
				</xsl:element>
			</xsl:element> <!-- toc-entry -->
			<xsl:apply-templates/>
		</xsl:element> <!-- article -->
		<!-- Then process the following siblings. -->
		<xsl:if test="following-sibling::xhtml:div[@class='scrIntroSection']">
			<xsl:call-template name="ProcessIntroSiblings">
				<xsl:with-param name="IntroSibling" select="following-sibling::xhtml:div[@class='scrIntroSection']"/>
				<xsl:with-param name="Number">2</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template> <!-- div[@class='scrIntroSection'] -->

	<!-- Skip siblings following div[@class='scrIntroSection']. -->
	<xsl:template match="xhtml:div[@class='scrIntroSection'][preceding-sibling::xhtml:div[@class='scrIntroSection']]"/>

	<!-- Process the siblings following div[@class='scrIntroSection']. -->
	<xsl:template name="ProcessIntroSiblings">
		<xsl:param name="IntroSibling"/>
		<xsl:param name="Number"/>
		<xsl:variable name="title" select="$IntroSibling/xhtml:div[@class='Intro_Section_Head']/xhtml:span/text()"/>
		<xsl:element name="article">
			<xsl:attribute name="id">
				<xsl:text>INTRO</xsl:text>
				<xsl:call-template name="getBookNumberFromCode">
					<xsl:with-param name="bookCode" select="preceding-sibling::xhtml:span[@class='scrBookCode']/text()"/>
				</xsl:call-template>
				<xsl:text>.</xsl:text>
				<xsl:value-of select="$Number"/>
			</xsl:attribute>
			<xsl:element name="toc-entry">
				<xsl:attribute name="level">3</xsl:attribute>
				<!-- xsl:attribute name="main">no</xsl:attribute -->
				<xsl:element name="span">
					<xsl:attribute name="xml:lang">
						<xsl:value-of select="$lang"/>
					</xsl:attribute>
					<xsl:value-of select="$title"/>
				</xsl:element>
			</xsl:element> <!-- toc-entry -->
			<xsl:apply-templates select="$IntroSibling/child::*"/>
		</xsl:element> <!-- article -->
		<xsl:if test="$IntroSibling/following-sibling::xhtml:div[@class='scrIntroSection']">
			<xsl:call-template name="ProcessIntroSiblings">
				<xsl:with-param name="IntroSibling">
					<xsl:value-of select="$IntroSibling/following-sibling::xhtml:div[@class='scrIntroSection']"/>
				</xsl:with-param>
				<xsl:with-param name="Number" select="$Number + 1"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template> <!-- ProcessIntroSiblings -->

	<!-- div[@class='Intro_Section_Head'] -->
	<xsl:template match="xhtml:div[@class='Intro_Section_Head']">
		<xsl:element name="p">
			<xsl:attribute name="class">Intro_Section_Head</xsl:attribute>
			<xsl:attribute name="xml:lang">
				<xsl:value-of select="$lang"/>
			</xsl:attribute>
			<xsl:value-of select="xhtml:span/text()"/>
		</xsl:element>
	</xsl:template> <!-- div[@class='Intro_Section_Head'] -->

	<!-- div[@class='Intro_Paragraph'] -->
	<xsl:template match="xhtml:div[@class='Intro_Paragraph']">
		<xsl:element name="p">
			<xsl:attribute name="class">Intro_Paragraph</xsl:attribute>
			<xsl:attribute name="xml:lang">
				<xsl:value-of select="$lang"/>
			</xsl:attribute>
			<xsl:value-of select="xhtml:span/text()"/>
		</xsl:element>
	</xsl:template> <!-- div[@class='Intro_Paragraph'] -->

	<!-- div[@class='Intro_List_Item1'] -->
	<xsl:template match="xhtml:div[@class='Intro_List_Item1']">
		<xsl:element name="p">
			<xsl:attribute name="class">Intro_List_Item1</xsl:attribute>
			<xsl:attribute name="xml:lang">
				<xsl:value-of select="$lang"/>
			</xsl:attribute>
			<xsl:value-of select="xhtml:span/text()"/>
		</xsl:element>
	</xsl:template> <!-- div[@class='Intro_List_Item1'] -->

	<!-- div[@class='columns'] -->
	<xsl:template match="xhtml:div[@class='columns']">
		<xsl:variable name="title" select="preceding-sibling::xhtml:div[@class='Title_Main']/xhtml:span/text()"/>
		<xsl:apply-templates select="xhtml:div[@class='scrSection']"/>
	</xsl:template> <!-- div[@class='columns'] -->

	<!-- div[@class='scrSection'] -->
	<xsl:template match="xhtml:div[@class='scrSection']">
		<xsl:variable name="title">
			<!-- Find the title as a Section_Head_Major or as a Section_Head. -->
			 <xsl:choose>
				<xsl:when test="xhtml:div[@class='Section_Head_Major']">
					<xsl:value-of select="xhtml:div[@class='Section_Head_Major']/xhtml:span/text()"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="xhtml:div[@class='Section_Head']/xhtml:span/text()"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable> <!-- title -->
		<xsl:variable name="bookTitle" select="ancestor::xhtml:div[@class='scrBook']/xhtml:div[@class='Title_Main']/xhtml:span/text()"/>
		<xsl:element name="article">
			<xsl:attribute name="id">
				<xsl:text>SEC</xsl:text>
				<xsl:call-template name="getBookNumberFromCode">
					<xsl:with-param name="bookCode" select="ancestor::xhtml:div[@class='columns']/preceding-sibling::xhtml:span[@class='scrBookCode']/text()"/>
				</xsl:call-template>
				<xsl:text>.</xsl:text>
				<xsl:choose>
					<xsl:when test="descendant::xhtml:span[@class='Chapter_Number']">
						<xsl:variable name="chapterNode" select="descendant::xhtml:span[@class='Chapter_Number'][1]"/>
						<xsl:value-of select="$chapterNode/text()"/>
						<xsl:text>.</xsl:text>
						<xsl:value-of select="$chapterNode/following-sibling::xhtml:span[@class='Verse_Number'][1]/text()"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="chapterNode" select="child::*/preceding::xhtml:span[@class='Chapter_Number'][1]"/>
						<xsl:value-of select="$chapterNode/text()"/>
						<xsl:text>.</xsl:text>
						<xsl:value-of select="descendant::xhtml:span[@class='Verse_Number'][1]/text()"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:element name="toc-entry">
				<xsl:attribute name="level">3</xsl:attribute>
				<xsl:element name="span">
					<xsl:attribute name="xml:lang">
						<xsl:value-of select="$lang"/>
					</xsl:attribute>
					<xsl:value-of select="$title"/>
				</xsl:element>
			</xsl:element> <!-- toc-entry -->
			<xsl:apply-templates/>
		</xsl:element> <!-- article -->
	</xsl:template> <!-- div[@class='scrSection'] -->

	<!-- div[@class='Parallel_Passage_Reference'] -->
	<xsl:template match="xhtml:div[@class='Parallel_Passage_Reference']">
		<xsl:variable name="reference" select="xhtml:span/text()"/>
		<xsl:element name="p">
			<xsl:attribute name="class">Parallel_Passage_Reference</xsl:attribute>
			<xsl:attribute name="xml:lang">
				<xsl:value-of select="$lang"/>
			</xsl:attribute>
			<xsl:call-template name="showReferences">
				<xsl:with-param name="refString" select="$reference"/>
			</xsl:call-template>
			<!-- xsl:value-of select="$reference"/ -->
		</xsl:element>
	</xsl:template> <!-- div[@class='Parallel_Passage_Reference'] -->

<!--Examples of references to books not included in the gospels:
Text (Buka Tango 1:18-19)
Text (Luk 24:50-53; Buka Tango 1:9-11)
Text (Mak 14:22-26; Luk 22:14-23; 1 Korin 11:23-25)
Text (Mak 16:14-18; Luk 24:36-49; Jon 20:19-23; Buka Tango 1:6-8)
Text (Mak 16:19-20; Buka Tango 1:9-11)
Text (Matiu 26:26-30; Luk 22:14-20; I Korin 11:23-25)
Text (Matiu 28:16-20; Mak 16:14-18; Jon 20:19-23; Buka Tango 1:6-8)

A method of getting the book code for a given reference, provided the reference is from a book that is contained in the given file:
//div[@class='scrBook'][contains(span[@class='scrBookName'],'Luk')]/span[@class='scrBookCode']/text() -->

	<xsl:template name="showReferences">
		<xsl:param name="refString"/>
		<xsl:choose>
			<!-- If the refString starts with a "(", write it out and process the rest of the string. -->
			<xsl:when test="starts-with($refString,'(')">
				<xsl:text>(</xsl:text>
				<xsl:call-template name="showReferences">
					<xsl:with-param name="refString" select="substring-after($refString,'(')"/>
				</xsl:call-template>
			</xsl:when>
			<!-- If the refString starts with a space, write it out and process the rest of the string. -->
			<xsl:when test="starts-with($refString,' ')">
				<xsl:text> </xsl:text>
				<xsl:call-template name="showReferences">
					<xsl:with-param name="refString" select="substring-after($refString,' ')"/>
				</xsl:call-template>
			</xsl:when>
			<!-- If the refString starts contains a semicolon, write out "Bible:" and the string before the semicolon. Then process the rest of the string. -->
			<xsl:when test="contains($refString,';')">
				<!-- Get the Bible book name for the given reference. -->
				<xsl:variable name="bookNameFromRefString">
					<xsl:call-template name="getBookNameFromRefString">
						<xsl:with-param name="refString" select="substring-before($refString,';')"/>
					</xsl:call-template>
				</xsl:variable>
				<!-- Get the book code needed to make the reference a hot link. -->
				<xsl:variable name="bookCode" select="$scrBookNames[contains(text(),$bookNameFromRefString)]/following-sibling::xhtml:span[@class='scrBookCode']/text()"/>
				<xsl:variable name="bookAbbrev">
					<xsl:call-template name="getBookAbbrevFromCode">
						<xsl:with-param name="bookCode" select="$bookCode"/>
					</xsl:call-template>
				</xsl:variable> <!-- bookAbbrev -->
				<xsl:variable name="substringBeforeSemicolon" select="substring-before($refString,';')"/>
				<!-- xsl:comment>
					<xsl:text>showReferences: refString="</xsl:text>
					<xsl:value-of select="$refString"/>
					<xsl:text>", bookNameFromRefString="</xsl:text>
					<xsl:value-of select="$bookNameFromRefString"/>
					<xsl:text>", bookCode="</xsl:text>
					<xsl:value-of select="$bookCode"/>
					<xsl:text>", bookAbbrev="</xsl:text>
					<xsl:value-of select="$bookAbbrev"/>
					<xsl:text>"</xsl:text>
				</xsl:comment -->
				<xsl:choose>
					<!-- If the refString contains a comma, process each side of the comma. -->
					<xsl:when test="contains($substringBeforeSemicolon,',')">
						<xsl:variable name="substringAfterName" select="substring-after($substringBeforeSemicolon,concat($bookNameFromRefString,' '))"/>
						<xsl:variable name="chapter" select="substring-before($substringAfterName,':')"/>
						<!-- Process the references on both sides of the comma. -->
						<xsl:call-template name="getRefsFromStringWithComma">
							<xsl:with-param name="bookNameFromRefString" select="$bookNameFromRefString"/>
							<xsl:with-param name="bookAbbrev" select="$bookAbbrev"/>
							<xsl:with-param name="chapter" select="$chapter"/>
							<xsl:with-param name="substringAfterName" select="$substringAfterName"/>
							<xsl:with-param name="showBookName" select="true()"/>
						</xsl:call-template>
						<xsl:text>;</xsl:text>
						<xsl:call-template name="showReferences">
							<xsl:with-param name="refString" select="substring-after($refString,';')"/>
						</xsl:call-template>
					</xsl:when> <!-- contains($substringBeforeSemicolon,',') -->
					<xsl:otherwise>
						<!-- Only make a <data> element if the book abbrev is not empty. -->
						<xsl:choose>
							<xsl:when test="string-length($bookAbbrev) > 0">
								<xsl:element name="data">
									<xsl:attribute name="ref">
										<xsl:text>Bible:</xsl:text>
										<!-- TODO: use bookAbbrev if available. -->
										<xsl:choose>
											<xsl:when test="string-length($bookAbbrev)>0">
												<xsl:value-of select="$bookAbbrev"/>
												<xsl:variable name="substringAfterName" select="substring-after($refString,$bookNameFromRefString)"/>
												<xsl:call-template name="removeLCLetters">
													<xsl:with-param name="passageString" select="substring-before($substringAfterName,';')"/>
												</xsl:call-template>
												<!-- xsl:value-of select="substring-before($substringAfterName,';')"/ -->
											</xsl:when> <!-- string-length($bookAbbrev)>0 -->
											<xsl:otherwise>
												<xsl:call-template name="removeLCLetters">
													<xsl:with-param name="passageString" select="substring-before($refString,';')"/>
												</xsl:call-template>
												<!-- xsl:value-of select="substring-before($refString,';')"/ -->
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute> <!-- ref -->
									<xsl:value-of select="substring-before($refString,';')"/>
								</xsl:element> <!-- data -->
							</xsl:when> <!-- string-length($bookAbbrev) > 0 -->
							<xsl:otherwise>
								<xsl:value-of select="substring-before($refString,';')"/>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:text>;</xsl:text>
						<xsl:call-template name="showReferences">
							<xsl:with-param name="refString" select="substring-after($refString,';')"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when> <!-- contains($refString,';') -->
			<xsl:when test="contains($refString,')')">
				<!-- Get the Bible book name for the given reference. -->
				<xsl:variable name="bookNameFromRefString">
					<xsl:call-template name="getBookNameFromRefString">
						<xsl:with-param name="refString" select="substring-before($refString,')')"/>
					</xsl:call-template>
				</xsl:variable>
				<!-- Get the book code needed to make the reference a hot link. -->
				<xsl:variable name="bookCode" select="$scrBookNames[contains(text(),$bookNameFromRefString)]/following-sibling::xhtml:span[@class='scrBookCode']/text()"/>
				<xsl:variable name="bookAbbrev">
					<xsl:call-template name="getBookAbbrevFromCode">
						<xsl:with-param name="bookCode" select="$bookCode"/>
					</xsl:call-template>
				</xsl:variable>
			<!--	<xsl:comment>
					<xsl:text>showReferences: refString="</xsl:text>
					<xsl:value-of select="$refString"/>
					<xsl:text>", bookNameFromRefString="</xsl:text>
					<xsl:value-of select="$bookNameFromRefString"/>
					<xsl:text>", bookAbbrev="</xsl:text>
					<xsl:value-of select="$bookAbbrev"/>
					<xsl:text>"</xsl:text>
				</xsl:comment> -->
				<xsl:choose>
					<!-- If the refString contains a comma, process each side of the comma. -->
					<xsl:when test="contains($refString,',')">
						<xsl:variable name="substringAfterName" select="substring-after($refString,concat($bookNameFromRefString,' '))"/>
						<xsl:variable name="chapter" select="substring-before($substringAfterName,':')"/>
						<!-- xsl:comment>
							<xsl:text>showReferences: substringAfterName="</xsl:text>
							<xsl:value-of select="$substringAfterName"/>
							<xsl:text>", chapter="</xsl:text>
							<xsl:value-of select="$chapter"/>
							<xsl:text>"</xsl:text>
						</xsl:comment -->
						<!-- Process the references on both sides of the comma. -->
						<xsl:call-template name="getRefsFromStringWithComma">
							<xsl:with-param name="bookNameFromRefString" select="$bookNameFromRefString"/>
							<xsl:with-param name="bookAbbrev" select="$bookAbbrev"/>
							<xsl:with-param name="chapter" select="$chapter"/>
							<xsl:with-param name="substringAfterName" select="$substringAfterName"/>
							<xsl:with-param name="showBookName" select="true()"/>
						</xsl:call-template>
					</xsl:when>
					<xsl:otherwise>
						<!-- Only make a <data> element if the book abbrev is not empty. -->
						<xsl:choose>
							<xsl:when test="string-length($bookAbbrev) > 0">
								<xsl:element name="data">
									<xsl:attribute name="ref">
										<xsl:text>Bible:</xsl:text>
										<!-- TODO: use bookAbbrev if available. -->
										<xsl:variable name="chaptVerseString">
											<xsl:choose>
												<xsl:when test="string-length($bookAbbrev)>0">
													<xsl:value-of select="$bookAbbrev"/>
													<xsl:variable name="substringAfterName" select="substring-after($refString,$bookNameFromRefString)"/>
													<xsl:value-of select="substring-before($substringAfterName,')')"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="substring-before($refString,';')"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:variable> <!-- chaptVerseString -->
										<!-- Remove subverse letters, e.g., 'a', 'b', etc. -->
										<xsl:call-template name="removeLCLetters">
											<xsl:with-param name="passageString" select="$chaptVerseString"/>
										</xsl:call-template>
									</xsl:attribute> <!-- ref -->
									<xsl:value-of select="substring-before($refString,')')"/>
								</xsl:element> <!-- data -->
							</xsl:when> <!-- string-length($bookAbbrev) > 0 -->
							<xsl:otherwise>
								<xsl:value-of select="substring-before($refString,')')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:text>)</xsl:text>
			</xsl:when> <!-- contains($refString,')') -->
			<xsl:otherwise/> <!-- TODO: Is something needed here? -->
		</xsl:choose>
	</xsl:template> <!-- name="showReferences" -->
	
	<!-- Remove subverse letters, e.g., 'a', 'b', etc., and return the string. -->
	<xsl:template name="removeLCLetters">
		<xsl:param name="passageString"/>
		<!-- Replace 'b-' with '-'. -->
		<xsl:variable name="string1">
			<xsl:choose>
				<xsl:when test="contains($passageString,'b-')">
					<xsl:value-of select="substring-before($passageString,'b-')"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring-after($passageString,'b-')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$passageString"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable> <!-- string1 -->
		<!-- Remove ending 'a'. -->
		<xsl:variable name="string2">
			<xsl:choose>
				<xsl:when test="substring($string1, (string-length($string1)) ) = 'a' ">
					<xsl:value-of select="substring($string1, 1, (string-length($string1)-1))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$string1"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable> <!-- string2 -->
		<xsl:value-of select="$string2"/>
	</xsl:template> <!-- removeLCLetters -->

	<xsl:template name="getBookNameFromRefString">
		<xsl:param name="refString"/>
		<xsl:variable name="substringAfterSpace" select="substring-after($refString,' ')"/>
		<!-- Output the string before the space. -->
		<xsl:value-of select="substring-before($refString,' ')"/>
		<!-- Look for more parts to the book name. -->
		<xsl:if test="not(contains('1234567890', substring($substringAfterSpace,1,1)))">
			<!-- If a space separates two parts of the book name, include it here. -->
			<xsl:text> </xsl:text>
			<xsl:call-template name="getBookNameFromRefString">
				<xsl:with-param name="refString" select="$substringAfterSpace"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template> <!-- name="getBookNameFromRefString" -->

	<!-- Process the references on both sides of the comma. -->
	<xsl:template name="getRefsFromStringWithComma">
		<xsl:param name="bookNameFromRefString"/>
		<xsl:param name="bookAbbrev"/>
		<xsl:param name="chapter"/>
		<xsl:param name="substringAfterName"/> <!-- does not begin with space -->
		<xsl:param name="showBookName"/>
		<xsl:choose>
			<!-- Process the reference before the comma. -->
			<xsl:when test="contains($substringAfterName,',')">
				<xsl:variable name="substringBeforeComma" select="substring-before($substringAfterName,',')"/>
				<!-- xsl:comment>
					<xsl:text>getRefsFromStringWithComma: process "</xsl:text>
					<xsl:value-of select="$substringBeforeComma"/>
					<xsl:text>"</xsl:text>
				</xsl:comment -->
				<xsl:variable name="chapterFromString">
					<xsl:choose>
						<xsl:when test="contains($substringBeforeComma,':' )">
							<xsl:value-of select="substring-before($substringBeforeComma,':' )"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$chapter"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable> <!-- chapterFromString -->
				<xsl:variable name="verseString">
					<xsl:choose>
						<xsl:when test="contains($substringBeforeComma,':' )">
							<xsl:value-of select="substring-after($substringBeforeComma,':' )"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$substringBeforeComma"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable> <!-- verseString -->
				<!-- xsl:comment>
					<xsl:text>getRefsFromStringWithComma: chapter="</xsl:text>
					<xsl:value-of select="$chapterFromString"/>
					<xsl:text>", verse="</xsl:text>
					<xsl:value-of select="$verseString"/>
					<xsl:text>"</xsl:text>
				</xsl:comment -->
				<xsl:if test="not($showBookName)">
					<xsl:text>, </xsl:text>
				</xsl:if>
				<xsl:element name="data">
					<xsl:attribute name="ref">
						<xsl:text>Bible:</xsl:text>
						<!-- Use bookAbbrev if available. -->
						<xsl:choose>
							<xsl:when test="string-length($bookAbbrev)>0">
								<xsl:value-of select="$bookAbbrev"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$bookNameFromRefString"/>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:text> </xsl:text>
						<xsl:value-of select="$chapterFromString"/>
						<xsl:text>:</xsl:text>
						<xsl:call-template name="removeLCLetters">
							<xsl:with-param name="passageString" select="$verseString"/>
						</xsl:call-template>
						<!-- xsl:value-of select="$verseString"/ -->
					</xsl:attribute> <!-- ref -->
					<xsl:if test="$showBookName">
						<xsl:value-of select="$bookNameFromRefString"/>
						<xsl:text> </xsl:text>
					</xsl:if>
					<xsl:if test="contains($substringBeforeComma,':')">
						<xsl:value-of select="$chapterFromString"/>
						<xsl:text>:</xsl:text>
					</xsl:if>
					<xsl:value-of select="$verseString"/>
				</xsl:element> <!-- data -->
				<!-- Process the reference following the comma. -->
				<xsl:variable name="substringBeforeBracket">
					<xsl:choose>
						<xsl:when test="contains($substringAfterName,')')">
							<xsl:value-of select="substring-before($substringAfterName,')')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$substringAfterName"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable> <!-- substringBeforeBracket -->
				<xsl:call-template name="getRefsFromStringWithComma">
					<xsl:with-param name="bookNameFromRefString" select="$bookNameFromRefString"/>
					<xsl:with-param name="bookAbbrev" select="$bookAbbrev"/>
					<xsl:with-param name="chapter" select="$chapterFromString"/>
					<xsl:with-param name="substringAfterName" select="substring-after($substringBeforeBracket,', ')"/>
					<xsl:with-param name="showBookName" select="false()"/>
				</xsl:call-template>
			</xsl:when> <!-- contains($substringAfterName,',') -->
			<!-- Process the remaining reference. -->
			<xsl:otherwise>
				<!-- xsl:comment>
					<xsl:text>getRefsFromStringWithComma (otherwise): process "</xsl:text>
					<xsl:value-of select="$substringAfterName"/>
					<xsl:text>"</xsl:text>
				</xsl:comment -->
				<xsl:variable name="chapterFromString">
					<xsl:choose>
						<xsl:when test="contains($substringAfterName,':' )">
							<xsl:value-of select="substring-before($substringAfterName,':' )"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$chapter"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable> <!-- chapterFromString -->
				<xsl:variable name="substringAfterSlashes">
					<xsl:choose>
						<xsl:when test="starts-with($substringAfterName,'//' )">
							<xsl:value-of select="substring-after($substringAfterName,'// ' )"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$substringAfterName"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable> <!-- substringAfterSlashes -->
				<xsl:variable name="verseString">
					<xsl:choose>
						<xsl:when test="contains($substringAfterSlashes,':' )">
							<!-- Remove lower-case subverse letters, e.g., 'a', 'b', etc. -->
							<xsl:call-template name="removeLCLetters">
								<xsl:with-param name="passageString" select="substring-after($substringAfterSlashes,':' )"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<!-- Remove lower-case subverse letters, e.g., 'a', 'b', etc. -->
							<xsl:call-template name="removeLCLetters">
								<xsl:with-param name="passageString" select="$substringAfterSlashes"/>
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable> <!-- verseString -->
				<xsl:text>, </xsl:text>
				<xsl:if test="starts-with($substringAfterName,'//')">
					<xsl:text> // </xsl:text>
				</xsl:if>
				<xsl:element name="data">
					<xsl:attribute name="ref">
						<xsl:text>Bible:</xsl:text>
						<!-- Use bookAbbrev if available. -->
						<xsl:choose>
							<xsl:when test="string-length($bookAbbrev)>0">
								<xsl:value-of select="$bookAbbrev"/>
								<xsl:text> </xsl:text>
								<xsl:value-of select="$chapterFromString"/>
								<xsl:text>:</xsl:text>
								<xsl:value-of select="$verseString"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$bookNameFromRefString"/>
								<xsl:text> </xsl:text>
								<xsl:value-of select="$chapterFromString"/>
								<xsl:text>:</xsl:text>
								<xsl:value-of select="$verseString"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute> <!-- ref -->
					<xsl:value-of select="$substringAfterSlashes"/>
				</xsl:element> <!-- data -->
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template> <!-- getRefsFromStringWithComma -->

	<!-- div[@class='Section_Head'] -->
	<xsl:template match="xhtml:div[@class='Section_Head']">
		<xsl:variable name="heading" select="xhtml:span/text()"/>
		<xsl:if test="string-length($heading)>0">
			<!-- xsl:element name="toc-entry">
				<xsl:attribute name="level">3</xsl:attribute>
				<xsl:element name="span">
					<xsl:attribute name="xml:lang">
						<xsl:value-of select="$lang"/>
					</xsl:attribute>
					<xsl:value-of select="$heading"/>
				</xsl:element>
			</xsl:element> <!- toc-entry -->
			<xsl:element name="p">
				<xsl:attribute name="class">Section_Head</xsl:attribute>
				<xsl:attribute name="xml:lang">
					<xsl:value-of select="$lang"/>
				</xsl:attribute>
				<xsl:value-of select="$heading"/>
			</xsl:element>
		</xsl:if>
	</xsl:template> <!-- div[@class='Section_Head'] -->

	<!-- div[@class='Section_Head_Major'] -->
	<xsl:template match="xhtml:div[@class='Section_Head_Major']">
		<xsl:variable name="heading" select="xhtml:span/text()"/>
		<xsl:if test="string-length($heading)>0">
			<!-- xsl:element name="toc-entry">
				<xsl:attribute name="level">3</xsl:attribute>
				<xsl:element name="span">
					<xsl:attribute name="xml:lang">
						<xsl:value-of select="$lang"/>
					</xsl:attribute>
					<xsl:value-of select="$heading"/>
				</xsl:element>
			</xsl:element> <!- toc-entry -->
			<xsl:element name="p">
				<xsl:attribute name="class">Section_Head_Major</xsl:attribute>
				<xsl:attribute name="xml:lang">
					<xsl:value-of select="$lang"/>
				</xsl:attribute>
				<xsl:value-of select="$heading"/>
			</xsl:element>
		</xsl:if>
	</xsl:template> <!-- div[@class='Section_Head_Major'] -->

	<!-- div[@class='Section_Head_Minor'] -->
	<xsl:template match="xhtml:div[@class='Section_Head_Minor']">
		<xsl:variable name="heading" select="xhtml:span/text()"/>
		<xsl:if test="string-length($heading)>0">
			<xsl:element name="toc-entry">
				<xsl:attribute name="level">3</xsl:attribute>
				<!-- xsl:attribute name="main">no</xsl:attribute -->
				<xsl:element name="span">
					<xsl:attribute name="xml:lang">
						<xsl:value-of select="$lang"/>
					</xsl:attribute>
					<xsl:value-of select="$heading"/>
				</xsl:element>
			</xsl:element> <!-- toc-entry -->
			<xsl:element name="p">
				<xsl:attribute name="class">Section_Head_Minor</xsl:attribute>
				<xsl:attribute name="xml:lang">
					<xsl:value-of select="$lang"/>
				</xsl:attribute>
				<xsl:value-of select="$heading"/>
			</xsl:element>
		</xsl:if>
	</xsl:template> <!-- div[@class='Section_Head_Minor'] -->
	
	<!-- span[@class='scrFootnoteMarker'] -->
	<!-- This is always immediately followed by either a <span class="Note_General_Paragraph"> or a <span class="Note_CrossHYPHENReference_Paragraph">. -->
	<xsl:template match="xhtml:span[@class='scrFootnoteMarker']">
		<!-- The reference begins with '#'. -->
		<xsl:variable name="href">
			<xsl:choose>
				<xsl:when test="starts-with(xhtml:a/@href,'#')">
					<xsl:value-of select="substring(xhtml:a/@href,2)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="xhtml:a/@href"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:element name="popup">
			<xsl:attribute name="pos">
				<!-- The matching <span> for the footnote uses all capital letters.
					Therefore, change all small letters to capitol letters for the popup reference. -->
				<xsl:value-of select="translate($href,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
			</xsl:attribute>
			<xsl:element name="sup">
				<!-- Get the superscripted text. -->
				<!-- span class="Note_General_Paragraph" title="†" -->
				<!-- <span class="Note_CrossHYPHENReference_Paragraph">/span -->
				<xsl:choose>
					<!-- Is this a 'Note_General_Paragraph'? -->
					<xsl:when test="following-sibling::xhtml:span[1]/@class='Note_General_Paragraph' ">
						<!-- xsl:value-of select="following-sibling::xhtml:span/@title" / -->
						<!-- TODO: TE uses a cross, '†', here, rather than an asterisk. -->
						<xsl:text>*</xsl:text>
					</xsl:when>
					<!-- If not, it is a 'Note_CrossHYPHENReference_Paragraph'. -->
					<xsl:otherwise>
						<!-- TODO: TE uses a double-box symbol here, rather than an asterisk. -->
						<xsl:text>*</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:element> <!-- sup -->
		</xsl:element> <!-- popup -->
		<!-- If the next div has the class '\f*', process the text now. -->
		<xsl:if test="following-sibling::xhtml:span[1][@class='Note_General_Paragraph' or @class='Note_CrossHYPHENReference_Paragraph'][count(following-sibling::*)=0]">
			<xsl:if test="parent::*/following-sibling::xhtml:div[1][@class='\f*']">
				<xsl:apply-templates select="parent::*/following-sibling::xhtml:div[1][@class='\f*']/child::*"/>
			</xsl:if>
		</xsl:if>
		<!-- For a FW7 file, if the next div has the class 'REVERSE_SOLIDUSfASTERISK', process the text now. -->
		<xsl:if test="following-sibling::xhtml:span[1][@class='Note_General_Paragraph' or @class='Note_CrossHYPHENReference_Paragraph'][count(following-sibling::*)=0]">
			<xsl:if test="parent::*/following-sibling::xhtml:div[1][@class='REVERSE_SOLIDUSfASTERISK']">
				<xsl:apply-templates select="parent::*/following-sibling::xhtml:div[1][@class='REVERSE_SOLIDUSfASTERISK']/child::*"/>
			</xsl:if>
		</xsl:if>
	</xsl:template> <!-- span[@class='scrFootnoteMarker'] -->

	<!-- span[@class='Verse_Number'] -->
	<xsl:template match="xhtml:span[@class='Verse_Number']">
        <xsl:variable name="startGenerateID" select="generate-id()"/>
		<xsl:variable name="title" select="ancestor::xhtml:div[@class='columns']/preceding-sibling::xhtml:div[@class='Title_Main']/xhtml:span/text()"/>
		<xsl:variable name="verseNumber" select="text()"/>
		<xsl:variable name="bookCode" select="ancestor::xhtml:div[@class='scrBook']/xhtml:span[@class='scrBookCode']"/>
		<xsl:element name="ms">
			<xsl:attribute name="ref">
				<xsl:text>Bible:</xsl:text>
				<xsl:call-template name="getBookAbbrevFromCode">
					<xsl:with-param name="bookCode" select="$bookCode"/>
				</xsl:call-template>
				<xsl:text> </xsl:text>
				<xsl:value-of select="preceding::xhtml:span[@class='Chapter_Number'][1]/text()"/>
				<xsl:text>:</xsl:text>
				<xsl:value-of select="$verseNumber"/>
			</xsl:attribute>
		</xsl:element> <!-- ms -->
		<!-- TODO: Should the first chapter "1" be formatted differently? -->
			<xsl:element name="sup">
				<xsl:value-of select="text()"/>
			</xsl:element>
	</xsl:template> <!-- span[@class='Verse_Number'] -->
	
	<!-- xhtml:span -->
	<xsl:template match="xhtml:span">
		<xsl:element name="field-start">
			<xsl:attribute name="name">bible</xsl:attribute>
		</xsl:element>
		<xsl:value-of select="text()"/>
		<xsl:element name="field-end">
			<xsl:attribute name="name">bible</xsl:attribute>			
		</xsl:element>
	</xsl:template> <!-- span -->

	<!-- div[@class='Paragraph'] -->
	<xsl:template match="xhtml:div[@class='Paragraph']">
		<xsl:element name="p">
			<xsl:attribute name="class">Paragraph</xsl:attribute>
			<xsl:attribute name="xml:lang">
				<xsl:value-of select="$lang"/>
			</xsl:attribute>
			<xsl:apply-templates/>
		</xsl:element>
	</xsl:template> <!-- div[@class='Paragraph'] -->

	<!-- div[@class='Paragraph_Continuation'] -->
	<xsl:template match="xhtml:div[@class='Paragraph_Continuation']">
		<xsl:element name="p">
			<xsl:attribute name="class">Paragraph_Continuation</xsl:attribute>
			<xsl:attribute name="xml:lang">
				<xsl:value-of select="$lang"/>
			</xsl:attribute>
			<xsl:apply-templates/>
		</xsl:element>
	</xsl:template> <!-- div[@class='Paragraph_Continuation'] -->

	<!-- div[@class='Line1'] -->
	<xsl:template match="xhtml:div[@class='Line1']">
		<xsl:element name="p">
			<xsl:attribute name="class">Line1</xsl:attribute>
			<xsl:attribute name="xml:lang">
				<xsl:value-of select="$lang"/>
			</xsl:attribute>
			<xsl:apply-templates/>
		</xsl:element>
	</xsl:template> <!-- div[@class='Line1'] -->

	<!-- div[@class='pictureCenter'] -->
	<xsl:template match="xhtml:div[@class='pictureCenter']">
		<xsl:apply-templates/>
	</xsl:template> <!-- div[@class='pictureCenter'] -->

	<!-- img -->
	<!-- Note: For the Bughotu images, I needed to resize them to be 15-25% of the original size. -->
	<xsl:template match="xhtml:img">
		<xsl:variable name="image">
			<xsl:call-template name="getImageName">
				<xsl:with-param name="fullPath" select="@src"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:element name="img">
			<!-- The compiler gives the following error: Unexpected attribute on element <img>: class. -->
			<!-- xsl:attribute name="class">
				<xsl:value-of select="parent::*/@class"/>
			</xsl:attribute -->
			<xsl:attribute name="src">
				<xsl:text>Figures/</xsl:text>
				<xsl:call-template name="getImageName">
					<xsl:with-param name="fullPath" select="@src"/>
				</xsl:call-template>
				<!-- Ignore the img attribute "alt". -->
			</xsl:attribute> <!-- src -->
			<xsl:attribute name="style">width:fit</xsl:attribute>
		</xsl:element>
	</xsl:template> <!-- img -->

	<xsl:template name="getImageName">
		<xsl:param name="fullPath"/>
		<xsl:choose>
			<xsl:when test="contains($fullPath,'/')">
				<xsl:call-template name="getImageName">
					<xsl:with-param name="fullPath" select="substring-after($fullPath,'/')"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$fullPath"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- div[@class='pictureCaption'] -->
	<xsl:template match="xhtml:div[@class='pictureCaption']">
		<xsl:element name="span">
			<xsl:attribute name="class">pictureCaption</xsl:attribute>
			<xsl:attribute name="xml:lang">
				<xsl:value-of select="xhtml:span/@lang"/>
			</xsl:attribute>
			<xsl:value-of select="xhtml:span/text()"/>
		</xsl:element>
	</xsl:template> <!-- div[@class='Line1'] -->

	<!-- Skip these for now. -->
	<xsl:template match="xhtml:span[@class='Chapter_Number']"/>
	<xsl:template match="xhtml:span[@class='Note_CrossHYPHENReference_Paragraph']"/>
	<xsl:template match="xhtml:span[@class='Note_General_Paragraph']"/>
	<!-- <div @class='\f*'> is processed with the associated footnote. -->
	<xsl:template match="xhtml:div[@class='\f*']"/>
	<xsl:template match="xhtml:div[@class='REVERSE_SOLIDUSfASTERISK']"/> <!-- Added for FW7 files. -->

	<!-- Default element and attribute templates. -->
	<xsl:template match="*">
		<xsl:comment>Warning :: The element "<xsl:value-of select='name()'/>" with class "<xsl:value-of select='@class'/>", child of "<xsl:value-of select="name(..)"/>" with class "<xsl:value-of select='.././@class'/>", has no matching template.</xsl:comment>
	</xsl:template>
	<!-- default attribute template -->
	<xsl:template match="@*">
		<xsl:comment>Warning :: The attribute "<xsl:value-of select="name()"/>" for element "<xsl:value-of select="name(..)"/>"  has no matching template.</xsl:comment>
	</xsl:template>

</xsl:stylesheet>