@REM DfM-Creator v 0.5 (GPL) 18-Feb-2013
@REM Merged the command line versions of DictdToDictionaryForMIDs, DictionaryGeneration, BitmapFontGenerator and JarCreator into DfM-Creator.
@REM Its now possible to invoke the CLI versions of the above mentioned tools from the command line by calling DfM-Creator in either of the following forms:
@REM    java -jar DfM-Creator.jar -DictdToDictionaryForMIDs
@REM    java -jar DfM-Creator.jar -DictionaryGeneration INPUT_DICTIONARY_FILE OUTPUT_DIRECTORY PROPERTY_DIRECTORY
@REM    java -jar DfM-Creator.jar -JarCreator DICTIONARY_DIRECTORY EMPTY_JAR_DIRECTORY OUTPUT_DIRECTORY
@REM    java -jar DfM-Creator.jar -FontGenerator
@if '%1' == '' ECHO USAGE: go inputName
@if '%1' == '' GOTO Done
@set base=%CD%
@if NOT '%2' == '' set base=%2
java -jar DfM-Creator.jar -DictionaryGeneration "%1" "%base%" "%base%"
java -jar DfM-Creator.jar -JarCreator "%base%\dictionary" "%base%\Empty_Jar-Jad" "%base%"
:Done
