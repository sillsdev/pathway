grammar csst3;
// See setup information: http://www.antlr.org/wiki/display/ANTLR3/Antlr3CSharpReleases
options {
        output=AST;
        ASTLabelType=CommonTree;
		}

tokens {
	IMPORT;
	MEDIA;
	PAGE;
	REGION;
	RULE;
	ATTRIB;
	PARENTOF;
	PRECEDES;
	SIBLING;
	ATTRIBEQUAL;
	HASVALUE;
	BEGINSWITH;
	PSEUDO;
	PROPERTY;
	FUNCTION;
	ANY;
	TAG;
	ID;
	CLASS;
	EM;
}

public
stylesheet
	: (importRule | media | pageRule | ruleset)+
	;

importRule
	: ('@import' | '@include')  STRING ';' -> ^( IMPORT STRING )
	| ('@import' | '@include')  function ';' -> ^( IMPORT function )
	;

media
	: '@media' IDENT '{' (pageRule | ruleset)+ '}' -> ^( MEDIA IDENT pageRule* ruleset* )
	;

pageRule
 	: '@page' IDENT* pseudo* '{' properties? region* '}' -> ^( PAGE IDENT* pseudo* properties* region* )
	;

region
	: '@' IDENT '{' properties? '}' -> ^( REGION IDENT properties* )
	;

ruleset
 	: selectors '{' properties? '}' -> ^( RULE selectors properties* )
	;
	
selectors
	: selector (',' selector)*
	;
	
selector
	: elem selectorOperation* pseudo* ->  elem selectorOperation* pseudo*
	| pseudo -> ANY pseudo 
	;

selectorOperation
	: selectop? elem -> selectop* elem
	;

selectop
	: '>' -> PARENTOF
        | '+' -> PRECEDES
        | '~' -> SIBLING
	;

properties
	: declaration (';' declaration?)* ->  declaration+
	;
	
elem
	:     (IDENT | UNIT) attrib* -> ^( TAG IDENT* UNIT* attrib* )
	| '#' (IDENT | UNIT) attrib* -> ^( ID IDENT* UNIT* attrib* )
	| '.' (IDENT | UNIT) attrib* -> ^( CLASS IDENT* UNIT* attrib* )
	| '*' attrib* -> ^( ANY attrib* )
	;

pseudo
	: (':'|'::') IDENT -> ^( PSEUDO IDENT )
	| (':'|'::') function -> ^( PSEUDO function )
	;

attrib
	: '[' IDENT (attribRelate (STRING | IDENT))? ']' -> ^( ATTRIB IDENT (attribRelate STRING* IDENT*)? )
	;
	
attribRelate
	: '='  -> ATTRIBEQUAL
	| '~=' -> HASVALUE
	| '|=' -> BEGINSWITH
	;	
  
declaration
	: IDENT ':' args -> ^( PROPERTY IDENT args )
	;

args
	: expr (','? expr)* -> expr*
	;

expr
	: (NUM unit?)
	| IDENT
	| COLOR
	| STRING
	| function
	;

unit
	: ('%'| UNIT)
	;
	
function
	: IDENT '(' selector? ')' -> IDENT '(' selector* ')' 
	| IDENT '(' args? ')' -> IDENT '(' args* ')'
	;
	
UNIT      :	'em'|'px'|'cm'|'mm'|'in'|'pt'|'pc'|'ex'|'deg'|'rad'|'grad'|'ms'|'s'|'hz'|'khz';	

IDENT
	:	('_' | 'a'..'z'| 'A'..'Z' | '\u00A0'..'\ufffe' ) 
		('_' | '-' | 'a'..'z'| 'A'..'Z' | '\u00A0'..'\ufffe' | '0'..'9')*
	|	'-' ('_' | 'a'..'z'| 'A'..'Z' | '\u00A0'..'\ufffe' ) 
		('_' | '-' | 'a'..'z'| 'A'..'Z' | '\u00A0'..'\ufffe' | '0'..'9')*
	;

// string literals
STRING
	:	'"' ( ('\\' ~('\n'))
	            |  ~( '"' | '\n' | '\r' | '\\' ) 
	            )* '"'
	|	'\'' ( ('\\' ~('\n'))
	             | ~( '\'' | '\n' | '\r' | '\\' ) 
	             )* '\''
	;

NUM
	:	'-'? ('0'..'9')+ ('.' ('0'..'9')+)?
	|	'-'? '.' ('0'..'9')+
	;

COLOR
	:	'#' ('0'..'9'|'a'..'f'|'A'..'F')+
	;


// Whitespace -- ignored See: http://www.antlr.org/pipermail/antlr-interest/2010-December/040362.html
WS	: (' '|'\t'|'\r'|'\n'|'\f')+  {$channel=HIDDEN;} // {System.Console.WriteLine("WS");}
	;
// multiple-line comments
COMMENT
	:	'/*' .* '*/' {$channel=HIDDEN;}
	;
// Single-line comments
LINE_COMMENT
	:	'//' ~('\n'|'\r')* '\r'? '\n' {$channel=HIDDEN;}
	;

