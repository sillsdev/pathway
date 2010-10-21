set base=..\..
if exist %base%\csst3.g goto anyCpu
set base=..\..\..
:anyCpu
set classpath=%base%\jars\antlr-3.1.3.jar;%base%\jars\antlr-runtime-3.2.jar;%base%\jars\stringtemplate-3.2.1.jar;%base%\jars\antlr-2.7.7.jar
java org.antlr.Tool %base%\csst3.g