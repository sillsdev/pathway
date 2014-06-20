@echo OFF
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
@set txtFileName=\\main.txt

@if NOT '%2' == '' set base=%2

echo java -jar DfM-Creator.jar -DictionaryGeneration "%1 %base%%txtFileName%" "%1 %base%" "%1 %base%"
echo java -jar DfM-Creator.jar -JarCreator "%1 %base%\\Dictionary\\" "%1 %base%\\Empty_Jar-Jad\\" "%1 %base%"

@if '%2' == '' GOTO LABELWITHOUTSPACE

@if NOT '%2' == '' GOTO LABELWITHSPACE


:LABELWITHOUTSPACE
	java -jar DfM-Creator.jar -DictionaryGeneration "%1\\%txtFileName%" "%1" "%1"
	java -jar DfM-Creator.jar -JarCreator "%1\\Dictionary\\" "%1\\Empty_Jar-Jad\\" "%1"
	GOTO Done
	
:LABELWITHSPACE
	java -jar DfM-Creator.jar -DictionaryGeneration "%1 %base%%txtFileName%" "%1 %base%" "%1 %base%"
	java -jar DfM-Creator.jar -JarCreator "%1 %base%\\Dictionary\\" "%1 %base%\\Empty_Jar-Jad\\" "%1 %base%"
	GOTO Done

:Done
	Exit