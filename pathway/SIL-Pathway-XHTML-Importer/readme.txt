=== Plugin Name ===
SIL-Pathway-XHTML-Importer
Contributors: Steve Miller, Philip Perry, and Greg Trihus, SIL International
Tags: import, dictionary, multilingual, bilingual, FieldWorks, FLEx, Pathway
Requires at least: 3.0
Tested up to: 
Stable tag: 0.0

== Description ==

Imports an XHTML file and a CSS file of dictionary data exported by FieldWorks FLEx.

= What it does: =



== Installation ==

Installation Instructions:

1. Download the plugin and unzip it.
2. Put the SIL-Pathway-XHTML-Importerdirectory into your wp-content/plugins/ directory.
3. Go to the Plugins page in your WordPress Administration area, find the plugin, and click 'Activate'.
	
== Known Issues ==

1. The export files from FLEX can be relatively large. The file we're working with now is 6.6 MB, which the default settings of PHP may not handle. We intend to allow the importer to split such files into smaller pieces, but until then, you may have to change the following settings in php.ini:

	max_execution_time
	

2. Sometimes the importer doesn't alert you when it is finished.

3. Some tags are still being displayed.
	
== Frequently Asked Questions ==

== Screenshots ==

== Support ==


