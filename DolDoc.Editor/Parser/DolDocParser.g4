parser grammar DolDocParser;

options {
	tokenVocab = DolDocLexer;
}

start: chunk_list EOF;
chunk_list: chunk*;
chunk: content | doldoc_command;
content: (CONTENT|ed=ESC_DOLLAR {((CommonToken)$ed).Text = "$";})+;

flag: a=(T_PLUS | T_MINUS) value=T_SYMBOL;
flag_list: flag+?;
string: T_STRING;
color: T_COLOR;
integer: T_INTEGER;

argument: color | string | integer;
named_argument: key=T_SYMBOL T_EQ value=argument;
argument_list: (T_COMMA (argument|named_argument))+;

doldoc_command: cmd=T_SYMBOL flags=flag_list? args=argument_list? END_DOLLAR;
