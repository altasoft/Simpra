@echo off
REM Make sure antlr-4.13.2-complete.jar is in the same directory as this .bat file!
java -cp "%~dp0antlr-4.13.2-complete.jar" org.antlr.v4.Tool %*


