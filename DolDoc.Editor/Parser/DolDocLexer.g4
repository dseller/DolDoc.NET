lexer grammar DolDocLexer;

ESC_DOLLAR: '$$';
DOLLAR: '$' -> skip, pushMode(DOLDOC_MODE);
CONTENT: .+?;

mode DOLDOC_MODE;
END_DOLLAR: '$' -> popMode;

T_COLOR
	: 'BLACK'
	| 'BLUE' 
	| 'GREEN' 
	| 'CYAN' 
	| 'RED' 
	| 'PURPLE'
	| 'BROWN'
	| 'LTGRAY'
	| 'DKGRAY'
	| 'LTBLUE'
	| 'LTGREEN'
	| 'LTCYAN'
	| 'LTRED'
	| 'LTPURPLE'
	| 'YELLOW'
	| 'WHITE';
T_SYMBOL: [a-zA-Z][a-zA-Z0-9_]*;
T_INTEGER: '-'? [0-9]+;
T_COMMA: ',';
T_STRING: '"' (~'"' | '\\' '"' | '\\' '\\')* '"';
T_EQ: '=';
T_PLUS: '+';
T_MINUS: '-';
 