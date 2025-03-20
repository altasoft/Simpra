pushd .

cd G4

..\antlr.bat -Dlanguage=CSharp -visitor -o ..\ANTLR\ SimpraLexer.g4 SimpraParser.g4


popd