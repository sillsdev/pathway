grammar csst3;
options {
	    output=AST;
	    ASTLabelType=CommonTree;
        language=CSharp;
	    k=4;
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
}

//@lexer::namespace {SIL.PublishingSolution.Compiler}
//@parser::namespace {SIL.PublishingSolution.Compiler}

@members {
private System.Collections.Generic.List<System.String> errors = new System.Collections.Generic.List<System.String>();
override public void DisplayRecognitionError(System.String[] tokenNames, RecognitionException e) {
    System.String hdr = GetErrorHeader(e);
    System.String msg = GetErrorMessage(e, tokenNames);
    errors.Add(hdr + " " + msg);
}
public System.Collections.Generic.List<System.String> GetErrors() {
    return errors;
}
}

stylesheet
	: importRule* (media | pageRule | ruleset)+
	;

importRule
	: ('@import' | '@include')  STRING ';' -> ^( IMPORT STRING )
	| ('@import' | '@include')  function ';' -> ^( IMPORT function )
	;

media
	: '@media' IDENT '{' (pageRule | ruleset)+ '}' -> ^( MEDIA IDENT pageRule* ruleset* )
	;

pageRule
 	: '@page' IDENT? pseudo? '{' properties? region* '}' -> ^( PAGE IDENT* pseudo* properties* region* )
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
	: elem selectorOperation* pseudo? ->  elem selectorOperation* pseudo*
	| pseudo -> ANY pseudo 
	;

selectorOperation
	: selectop? elem -> selectop* elem
	;

selectop
	: '>' -> PARENTOF
        | '+'  -> PRECEDES
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
	: IDENT '(' args? ')' -> IDENT '(' args* ')'
	;
	
UNIT      :	'em'|'px'|'cm'|'mm'|'in'|'pt'|'pc'|'ex'|'deg'|'rad'|'grad'|'ms'|'s'|'hz'|'khz';	

IDENT
	:	('_' | 'a'..'z'| 'A'..'Z' | '\u0100'..'\ufffe' ) 
		('_' | '-' | 'a'..'z'| 'A'..'Z' | '\u0100'..'\ufffe' | '0'..'9')*
	|	'-' ('_' | 'a'..'z'| 'A'..'Z' | '\u0100'..'\ufffe' ) 
		('_' | '-' | 'a'..'z'| 'A'..'Z' | '\u0100'..'\ufffe' | '0'..'9')*
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
	:	'-' (('0'..'9')* '.')? ('0'..'9')+
	|	(('0'..'9')* '.')? ('0'..'9')+
	;

COLOR
	:	'#' ('0'..'9'|'a'..'f'|'A'..'F')+
	;

// Single-line comments
SL_COMMENT
	:	'//'
		(~('\n'|'\r'))* ('\n'|'\r'('\n')?)
		{$channel=HIDDEN;}
	;
	
// multiple-line comments
COMMENT
	:	'/*' .* '*/' { $channel = HIDDEN; }
	;

// Whitespace -- ignored
WS	: ( ' ' | '\t' | '\r' | '\n' | '\f' )+ { $channel = HIDDEN; }
	;

