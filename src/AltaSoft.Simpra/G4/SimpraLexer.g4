// $antlr-format on

lexer grammar SimpraLexer;

COMMENT: '#' ~[\r\n]* -> skip;
BLOCK_COMMENT: '/*' .*? '*/' -> skip;

BOOLEAN: 'true' | 'false';
NUMBER: [0-9]+ ('.' [0-9]*)?;

PASCALSTRING: '\'' ( ~('\'' | '\\'))* '\'';
CSTRING: '"' (~('\\' | '"'))* '"';

PERCENT: '%';
ON_OFF: 'on' | 'off';

WS: [ \t\r\n]+ -> skip;

// Keywords
LET: 'let';
WHEN: 'when';
IF: 'if';
THEN: 'then';
ELSE: 'else';
END: 'end';
RETURN: 'return';

// Operators
IS: 'is';
IS_NOT: 'is not';

IN: 'in';
ANY_IN: 'any in';
ALL_IN: 'all in';

NOT_IN: 'not in';
ANY_NOT_IN: 'any not in';
ALL_NOT_IN: 'all not in';

NOT: 'not';
MIN: 'min';
MAX: 'max';

MATCHES: 'matches';
LIKE: 'like';
HAS: 'has';
VALUE: 'value';

AND: 'and';
OR: 'or';

EQ: '=';
PLUS: '+';
MINUS: '-';
MULT: '*';
DIV: '/';
IDIV: '//';
LT: '<';
LE: '<=';
GT: '>';
GE: '>=';

PLUS_EQ: '+=';
MINUS_EQ: '-=';

// Symbols
LPAREN: '(';
RPAREN: ')';
LBRACK: '[';
RBRACK: ']';
COMMA: ',';
DOT: '.';

// Identifier
IDENTIFIER: [a-zA-Z] [a-zA-Z_0-9]*;

DIRECTIVE: [$][a-z] [a-z_0-9]*;

// Error token
ERROR: . -> channel(HIDDEN);