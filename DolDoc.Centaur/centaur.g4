grammar centaur;

LINE_COMMENT: '//' ~[\r\n]* -> channel(HIDDEN);
T_INTEGER: [0-9]+;
T_FLOAT: [0-9]+'.'[0-9]+;
T_COMMA: ',';
T_STRING: '"' (~'"' | '\\' '"' | '\\' '\\')* '"';
T_IF: 'if';
T_FOR: 'for';
T_ELSE: 'else';
T_TRUE: 'true';
T_FALSE: 'false';
T_NULL: 'null';
T_WHILE: 'while';
T_PLUS: '+';
T_SEMICOLON: ';';
T_SLASH: '/';
T_MOD: '%';
T_ASTERISK: '*';
T_MINUS: '-';
T_POPEN: '(';
T_PCLOSE: ')';
T_BOPEN: '{';
T_BCLOSE: '}';
T_AOPEN: '[';
T_ACLOSE: ']';
T_ASSIGN: '=';
T_NEW: 'new';
T_STRUCT: 'struct';
T_DOT: '.';
T_DEC: '--';
T_INC: '++';
T_SHL: '<<';
T_SHR: '>>';
T_LT: '<';
T_GT: '>';
T_LTE: '<=';
T_GTE: '>=';
T_EQ: '==';
T_NEQ: '!=';
T_RETURN: 'return';
T_SYMBOL: [a-zA-Z_@]([a-zA-Z0-9_@])*;

WS : [ \t\r\n]+ -> skip ;

start
    : definition_list 
    ;
    
argument_list: ((expression) | (T_COMMA expression))+;
    
postfix_expression
	: primary_expression                                        #chainPostfix
	| postfix_expression T_AOPEN expression T_ACLOSE			#index
	| name=postfix_expression T_POPEN args=argument_list? T_PCLOSE	#call
	| ctx=postfix_expression T_DOT field=T_SYMBOL				#member
	| postfix_expression T_DEC									#decrement
	| postfix_expression T_INC									#increment
	;

mul_expression
	: postfix_expression                                                    #chainMul
	| left=mul_expression T_ASTERISK right=postfix_expression                          #arithMultiply
	| left=mul_expression T_SLASH right=postfix_expression                             #arithDivide
	| left=mul_expression T_MOD right=postfix_expression	                            #arithModulo
	;

add_expression
	: mul_expression                                                        #chainAdd
	| left=add_expression T_PLUS right=mul_expression		                #arithAdd
	| left=add_expression T_MINUS right=mul_expression		                #arithSubtract
	;
	
shift_expression
	: add_expression                                                        #chainShift
	| left=shift_expression T_SHL right=add_expression				        #arithShiftLeft
	| left=shift_expression T_SHR right=add_expression				        #arithShiftRight
	;

relational_expression
	: shift_expression                                                      #chainRel
	| left=relational_expression T_LT right=shift_expression				            #lessThan
	| left=relational_expression T_GT right=shift_expression				            #greaterThan
	| left=relational_expression T_LTE right=shift_expression				            #lessThanOrEqual
	| left=relational_expression T_GTE right=shift_expression				            #greaterThanOrEqual
	;

equality_expression
	: relational_expression                                                 #chainEquality
	| left=equality_expression T_EQ right=relational_expression		                #equals
	| left=equality_expression T_NEQ right=relational_expression		                #notEquals
	;

bitwise_and_expression
	: equality_expression
	| bitwise_and_expression T_BAND equality_expression			
	;

bitwise_or_expression
	: bitwise_and_expression
	| bitwise_or_expression T_BOR bitwise_and_expression		
	;

logical_and_expression
	: bitwise_or_expression
    | logical_and_expression T_AND bitwise_or_expression        
	;

logical_or_expression
	: logical_and_expression                               
	| logical_or_expression T_OR logical_and_expression		
	;

unary_expression
	: postfix_expression
	//| unary_operator cast_expression							{ $$ = node(UNARY, $1, $2); }
	;	
	
assignment_expression
	: logical_or_expression                                   #chainAssignment
	| target=unary_expression T_ASSIGN source=assignment_expression		  #assign	
	;
	
expression
	: assignment_expression
	;

primary_expression
	: T_SYMBOL                                  #symbol
	| T_INTEGER                                 #constInteger
	| T_STRING                                  #constString
	| T_FLOAT                                   #constFloat
    | T_NULL                                    #constNull
	| T_TRUE                                    #constTrue
	| T_FALSE                                   #constFalse
	| T_NEW type=T_SYMBOL T_POPEN T_PCLOSE      #newObj
    ;
    
declaration
	: type=T_SYMBOL name=T_SYMBOL T_SEMICOLON                                    #var
	| type=T_SYMBOL name=T_SYMBOL T_ASSIGN value=expression T_SEMICOLON          #varAssign
	;
	
compound_statement
	: T_BOPEN T_BCLOSE											
	| T_BOPEN statements=statement_list T_BCLOSE							
	;
    
jump_statement
	: T_RETURN T_SEMICOLON										
	| T_RETURN value=expression T_SEMICOLON							
	;
	
selection_statement
    : T_IF T_POPEN expr=expression T_PCLOSE body=compound_statement   #if
    | T_IF T_POPEN expr=expression T_PCLOSE body=compound_statement T_ELSE elseBody=compound_statement #ifElse
    ;
	
statement
    : compound_statement
    | expression_statement
    | selection_statement
    | jump_statement
    ;
    
statement_or_declaration
	: declaration
	| statement
	;
    
statement_list
	: statement_or_declaration+
	;
	
	
struct_field: type=T_SYMBOL name=T_SYMBOL T_SEMICOLON;
struct_definition: T_STRUCT name=T_SYMBOL T_BOPEN fields=struct_field+ T_BCLOSE;
    
parameter: type=T_SYMBOL name=T_SYMBOL;
parameter_list: ((parameter) | (T_COMMA parameter))+;
    
function_definition
    : resultType=T_SYMBOL name=T_SYMBOL T_POPEN T_PCLOSE body=compound_statement
    | resultType=T_SYMBOL name=T_SYMBOL T_POPEN parameters=parameter_list T_PCLOSE body=compound_statement
    ;

expression_statement
	: T_SEMICOLON										
	| e=expression T_SEMICOLON								
	;

definition
    : struct_definition
    | function_definition
    | declaration
    ;

definition_list
    : definition+
    ; 