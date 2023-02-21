grammar centaur;

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
T_RETURN: 'return';
T_SYMBOL: [a-zA-Z_@]([a-zA-Z0-9_@])*;

WS : [ \t\r\n]+ -> skip ;

start
    : definition_list 
    ;
    
postfix_expression
	: primary_expression                                        #chainPostfix
	| postfix_expression T_AOPEN expression T_ACLOSE			#index
	| postfix_expression T_POPEN T_PCLOSE						#call
//	| postfix_expression T_POPEN argument_expression_list T_PCLOSE	{ $$ = node(CALL, $1, $3); }
	| ctx=postfix_expression T_DOT field=T_SYMBOL							#member
	| postfix_expression T_DEC									#decrement
	| postfix_expression T_INC									#increment
	;

mul_expression
	: postfix_expression                                                    #chainMul
	| mul_expression T_ASTERISK postfix_expression                          #arithMultiply
	| mul_expression T_SLASH postfix_expression                             #arithDivide
	| mul_expression T_MOD postfix_expression	                            #arithModulo
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
	: shift_expression
	| relational_expression T_LT shift_expression				
	| relational_expression T_GT shift_expression				
	| relational_expression T_LTE shift_expression				
	| relational_expression T_GTE shift_expression				
	;

equality_expression
	: relational_expression
	| equality_expression T_EQUAL relational_expression		    
	| equality_expression T_NOTEQUAL relational_expression		
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
	| T_BOPEN statement_list T_BCLOSE							
	;
    
jump_statement
	: T_RETURN T_SEMICOLON										
	| T_RETURN value=expression T_SEMICOLON							
	;
	
statement
    : compound_statement
    | expression_statement
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
    
function_definition
    : resultType=T_SYMBOL name=T_SYMBOL T_POPEN T_PCLOSE body=compound_statement
    ;

expression_statement
	: T_SEMICOLON										
	| expression T_SEMICOLON								
	;

definition
    : struct_definition
    | function_definition
    ;

definition_list
    : definition+
    ; 