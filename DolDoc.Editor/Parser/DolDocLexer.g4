lexer grammar DolDocLexer;

ESC_DOLLAR: '$$';
DOLLAR: '$' -> skip, pushMode(DOLDOC_MODE);
CONTENT: .+?;

mode DOLDOC_MODE;
END_DOLLAR: '$' -> popMode;

T_COLOR: 'RED' | 'BLUE' | 'GREEN';
T_SYMBOL: [a-zA-Z][a-zA-Z0-9_]+;
T_INTEGER: '-'? [0-9]+;
T_COMMA: ',';
T_STRING: '"' (~'"' | '\\' '"')* '"';
T_EQ: '=';
T_PLUS: '+';
T_MINUS: '-';
 