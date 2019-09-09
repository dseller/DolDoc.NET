grammar doldoc;

options {
  language=CSharp3;
}

tokens {
  DOLLAR = '$';
  PLUS = '+';
  MINUS = '-';
  EQ = '=';
  COMMA = ',';
  QUOTE = '"';
}

@lexer::namespace { DolDoc.Core.Parser }
@parser::namespace { DolDoc.Core.Parser }

String	:	'"' (~('"'))* '"';
Identifier :	('A'..'Z')+;
Number	:	('0'..'9')+;
Anything:	. ;

public document returns [IList<DocumentNode> nodes = new List<DocumentNode>(); ]
	:	(n=document_node { nodes.Add($n.node); })+
	;
	
document_node returns [DocumentNode node]
	:	DOLLAR DOLLAR						{ $node = new StringNode("$"); }
	|	command 						{ $node = $command.cmd; }
	|	.							{ $node = new StringNode(input.LT(1).Text); }
	; 

command	returns [Command cmd]
	:	DOLLAR Identifier flag_list? argument_list? DOLLAR	{ $cmd = new Command($Identifier.text, $flag_list.flags, $argument_list.args); }
	;
	
argument_expr
	:	String
	|	Number	
	|	Identifier
	;
	
	
argument returns [Argument arg]
	:	COMMA (Identifier EQ)? argument_expr			{ $arg = new Argument($Identifier.text, $argument_expr.text); }
	;
	
argument_list returns [IList<Argument> args = new List<Argument>(); ]
	:	(a=argument { args.Add($a.arg); })+
	;
	
flag_status returns [bool status]
	:	PLUS							{ $status = true; }
	|	MINUS							{ $status = false; }
	;

flag returns [Flag flag]
	:	flag_status Identifier					{ $flag = new Flag($Identifier.text, $flag_status.status); }
	;
	
flag_list returns [IList<Flag> flags = new List<Flag>(); ]
	:	(f=flag { flags.Add($f.flag); })+
	;
	
