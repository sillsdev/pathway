// $ANTLR 3.0.1 C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3 2016-02-19 09:53:02
namespace SIL.PublishingSolution.Compiler
{

using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;




using Antlr.Runtime.Tree;

public class csst3Parser : Parser 
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"IMPORT", 
		"MEDIA", 
		"PAGE", 
		"REGION", 
		"RULE", 
		"ATTRIB", 
		"PARENTOF", 
		"PRECEDES", 
		"ATTRIBEQUAL", 
		"HASVALUE", 
		"BEGINSWITH", 
		"PSEUDO", 
		"PROPERTY", 
		"FUNCTION", 
		"ANY", 
		"TAG", 
		"ID", 
		"CLASS", 
		"EM", 
		"STRING", 
		"IDENT", 
		"UNIT", 
		"NUM", 
		"COLOR", 
		"WS", 
		"COMMENT", 
		"LINE_COMMENT", 
		"'@import'", 
		"'@include'", 
		"';'", 
		"'@media'", 
		"'{'", 
		"'}'", 
		"'@page'", 
		"'@'", 
		"','", 
		"'>'", 
		"'+'", 
		"'#'", 
		"'.'", 
		"'*'", 
		"':'", 
		"'::'", 
		"'['", 
		"']'", 
		"'='", 
		"'~='", 
		"'|='", 
		"'%'", 
		"'('", 
		"')'"
    };

    public const int PRECEDES = 11;
    public const int PSEUDO = 15;
    public const int CLASS = 21;
    public const int ANY = 18;
    public const int COMMENT = 29;
    public const int IMPORT = 4;
    public const int HASVALUE = 13;
    public const int COLOR = 27;
    public const int MEDIA = 5;
    public const int RULE = 8;
    public const int ID = 20;
    public const int WS = 28;
    public const int EOF = -1;
    public const int UNIT = 25;
    public const int PROPERTY = 16;
    public const int NUM = 26;
    public const int EM = 22;
    public const int PAGE = 6;
    public const int FUNCTION = 17;
    public const int REGION = 7;
    public const int PARENTOF = 10;
    public const int ATTRIBEQUAL = 12;
    public const int LINE_COMMENT = 30;
    public const int IDENT = 24;
    public const int ATTRIB = 9;
    public const int STRING = 23;
    public const int TAG = 19;
    public const int BEGINSWITH = 14;
    
    
        public csst3Parser(ITokenStream input) 
    		: base(input)
    	{
    		InitializeCyclicDFAs();
        }
        
    protected ITreeAdaptor adaptor = new CommonTreeAdaptor();
    
    public ITreeAdaptor TreeAdaptor
    {
        get { return this.adaptor; }
        set { this.adaptor = value; }
    }

    override public string[] TokenNames
	{
		get { return tokenNames; }
	}

    override public string GrammarFileName
	{
		get { return "C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3"; }
	}

    
    private System.Collections.Generic.List<System.String> errors = new System.Collections.Generic.List<System.String>();
    override public void DisplayRecognitionError(System.String[] tokenNames, RecognitionException e) {
        System.String hdr = GetErrorHeader(e);
        System.String msg = GetErrorMessage(e, tokenNames);
        errors.Add(hdr + " " + msg);
    }
    public System.Collections.Generic.List<System.String> GetErrors() {
        return errors;
    }


    public class stylesheet_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start stylesheet
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:46:1: public stylesheet : ( importRule )* ( media | pageRule | ruleset )+ ;
    public stylesheet_return stylesheet() // throws RecognitionException [1]
    {   
        stylesheet_return retval = new stylesheet_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        importRule_return importRule1 = null;

        media_return media2 = null;

        pageRule_return pageRule3 = null;

        ruleset_return ruleset4 = null;
        
        
    
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:48:2: ( ( importRule )* ( media | pageRule | ruleset )+ )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:48:4: ( importRule )* ( media | pageRule | ruleset )+
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();
            
            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:48:4: ( importRule )*
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);
            	    
            	    if ( ((LA1_0 >= 31 && LA1_0 <= 32)) )
            	    {
            	        alt1 = 1;
            	    }
            	    
            	
            	    switch (alt1) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:48:4: importRule
            			    {
            			    	PushFollow(FOLLOW_importRule_in_stylesheet169);
            			    	importRule1 = importRule();
            			    	followingStackPointer_--;
            			    	
            			    	adaptor.AddChild(root_0, importRule1.Tree);
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop1;
            	    }
            	} while (true);
            	
            	loop1:
            		;	// Stops C# compiler whinging that label 'loop1' has no statements

            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:48:16: ( media | pageRule | ruleset )+
            	int cnt2 = 0;
            	do 
            	{
            	    int alt2 = 4;
            	    switch ( input.LA(1) ) 
            	    {
            	    case 34:
            	    	{
            	        alt2 = 1;
            	        }
            	        break;
            	    case 37:
            	    	{
            	        alt2 = 2;
            	        }
            	        break;
            	    case IDENT:
            	    case UNIT:
            	    case 42:
            	    case 43:
            	    case 44:
            	    case 45:
            	    case 46:
            	    	{
            	        alt2 = 3;
            	        }
            	        break;
            	    
            	    }
            	
            	    switch (alt2) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:48:17: media
            			    {
            			    	PushFollow(FOLLOW_media_in_stylesheet173);
            			    	media2 = media();
            			    	followingStackPointer_--;
            			    	
            			    	adaptor.AddChild(root_0, media2.Tree);
            			    
            			    }
            			    break;
            			case 2 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:48:25: pageRule
            			    {
            			    	PushFollow(FOLLOW_pageRule_in_stylesheet177);
            			    	pageRule3 = pageRule();
            			    	followingStackPointer_--;
            			    	
            			    	adaptor.AddChild(root_0, pageRule3.Tree);
            			    
            			    }
            			    break;
            			case 3 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:48:36: ruleset
            			    {
            			    	PushFollow(FOLLOW_ruleset_in_stylesheet181);
            			    	ruleset4 = ruleset();
            			    	followingStackPointer_--;
            			    	
            			    	adaptor.AddChild(root_0, ruleset4.Tree);
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt2 >= 1 ) goto loop2;
            		            EarlyExitException eee =
            		                new EarlyExitException(2, input);
            		            throw eee;
            	    }
            	    cnt2++;
            	} while (true);
            	
            	loop2:
            		;	// Stops C# compiler whinging that label 'loop2' has no statements

            
            }
    
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end stylesheet

    public class importRule_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start importRule
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:51:1: importRule : ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) );
    public importRule_return importRule() // throws RecognitionException [1]
    {   
        importRule_return retval = new importRule_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken string_literal5 = null;
        IToken string_literal6 = null;
        IToken STRING7 = null;
        IToken char_literal8 = null;
        IToken string_literal9 = null;
        IToken string_literal10 = null;
        IToken char_literal12 = null;
        function_return function11 = null;
        
        
        CommonTree string_literal5_tree=null;
        CommonTree string_literal6_tree=null;
        CommonTree STRING7_tree=null;
        CommonTree char_literal8_tree=null;
        CommonTree string_literal9_tree=null;
        CommonTree string_literal10_tree=null;
        CommonTree char_literal12_tree=null;
        RewriteRuleTokenStream stream_33 = new RewriteRuleTokenStream(adaptor,"token 33");
        RewriteRuleTokenStream stream_STRING = new RewriteRuleTokenStream(adaptor,"token STRING");
        RewriteRuleTokenStream stream_31 = new RewriteRuleTokenStream(adaptor,"token 31");
        RewriteRuleTokenStream stream_32 = new RewriteRuleTokenStream(adaptor,"token 32");
        RewriteRuleSubtreeStream stream_function = new RewriteRuleSubtreeStream(adaptor,"rule function");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:52:2: ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) )
            int alt5 = 2;
            int LA5_0 = input.LA(1);
            
            if ( (LA5_0 == 31) )
            {
                int LA5_1 = input.LA(2);
                
                if ( (LA5_1 == IDENT) )
                {
                    alt5 = 2;
                }
                else if ( (LA5_1 == STRING) )
                {
                    alt5 = 1;
                }
                else 
                {
                    NoViableAltException nvae_d5s1 =
                        new NoViableAltException("51:1: importRule : ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) );", 5, 1, input);
                
                    throw nvae_d5s1;
                }
            }
            else if ( (LA5_0 == 32) )
            {
                int LA5_2 = input.LA(2);
                
                if ( (LA5_2 == IDENT) )
                {
                    alt5 = 2;
                }
                else if ( (LA5_2 == STRING) )
                {
                    alt5 = 1;
                }
                else 
                {
                    NoViableAltException nvae_d5s2 =
                        new NoViableAltException("51:1: importRule : ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) );", 5, 2, input);
                
                    throw nvae_d5s2;
                }
            }
            else 
            {
                NoViableAltException nvae_d5s0 =
                    new NoViableAltException("51:1: importRule : ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) );", 5, 0, input);
            
                throw nvae_d5s0;
            }
            switch (alt5) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:52:4: ( '@import' | '@include' ) STRING ';'
                    {
                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:52:4: ( '@import' | '@include' )
                    	int alt3 = 2;
                    	int LA3_0 = input.LA(1);
                    	
                    	if ( (LA3_0 == 31) )
                    	{
                    	    alt3 = 1;
                    	}
                    	else if ( (LA3_0 == 32) )
                    	{
                    	    alt3 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d3s0 =
                    	        new NoViableAltException("52:4: ( '@import' | '@include' )", 3, 0, input);
                    	
                    	    throw nvae_d3s0;
                    	}
                    	switch (alt3) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:52:5: '@import'
                    	        {
                    	        	string_literal5 = (IToken)input.LT(1);
                    	        	Match(input,31,FOLLOW_31_in_importRule195); 
                    	        	stream_31.Add(string_literal5);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:52:17: '@include'
                    	        {
                    	        	string_literal6 = (IToken)input.LT(1);
                    	        	Match(input,32,FOLLOW_32_in_importRule199); 
                    	        	stream_32.Add(string_literal6);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	STRING7 = (IToken)input.LT(1);
                    	Match(input,STRING,FOLLOW_STRING_in_importRule203); 
                    	stream_STRING.Add(STRING7);

                    	char_literal8 = (IToken)input.LT(1);
                    	Match(input,33,FOLLOW_33_in_importRule205); 
                    	stream_33.Add(char_literal8);

                    	
                    	// AST REWRITE
                    	// elements:          STRING
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 52:41: -> ^( IMPORT STRING )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:52:44: ^( IMPORT STRING )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(IMPORT, "IMPORT"), root_1);
                    	    
                    	    adaptor.AddChild(root_1, stream_STRING.Next());
                    	    
                    	    adaptor.AddChild(root_0, root_1);
                    	    }
                    	
                    	}
                    	

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:53:4: ( '@import' | '@include' ) function ';'
                    {
                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:53:4: ( '@import' | '@include' )
                    	int alt4 = 2;
                    	int LA4_0 = input.LA(1);
                    	
                    	if ( (LA4_0 == 31) )
                    	{
                    	    alt4 = 1;
                    	}
                    	else if ( (LA4_0 == 32) )
                    	{
                    	    alt4 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d4s0 =
                    	        new NoViableAltException("53:4: ( '@import' | '@include' )", 4, 0, input);
                    	
                    	    throw nvae_d4s0;
                    	}
                    	switch (alt4) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:53:5: '@import'
                    	        {
                    	        	string_literal9 = (IToken)input.LT(1);
                    	        	Match(input,31,FOLLOW_31_in_importRule221); 
                    	        	stream_31.Add(string_literal9);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:53:17: '@include'
                    	        {
                    	        	string_literal10 = (IToken)input.LT(1);
                    	        	Match(input,32,FOLLOW_32_in_importRule225); 
                    	        	stream_32.Add(string_literal10);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	PushFollow(FOLLOW_function_in_importRule229);
                    	function11 = function();
                    	followingStackPointer_--;
                    	
                    	stream_function.Add(function11.Tree);
                    	char_literal12 = (IToken)input.LT(1);
                    	Match(input,33,FOLLOW_33_in_importRule231); 
                    	stream_33.Add(char_literal12);

                    	
                    	// AST REWRITE
                    	// elements:          function
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 53:43: -> ^( IMPORT function )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:53:46: ^( IMPORT function )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(IMPORT, "IMPORT"), root_1);
                    	    
                    	    adaptor.AddChild(root_1, stream_function.Next());
                    	    
                    	    adaptor.AddChild(root_0, root_1);
                    	    }
                    	
                    	}
                    	

                    
                    }
                    break;
            
            }
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end importRule

    public class media_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start media
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:56:1: media : '@media' IDENT '{' ( pageRule | ruleset )+ '}' -> ^( MEDIA IDENT ( pageRule )* ( ruleset )* ) ;
    public media_return media() // throws RecognitionException [1]
    {   
        media_return retval = new media_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken string_literal13 = null;
        IToken IDENT14 = null;
        IToken char_literal15 = null;
        IToken char_literal18 = null;
        pageRule_return pageRule16 = null;

        ruleset_return ruleset17 = null;
        
        
        CommonTree string_literal13_tree=null;
        CommonTree IDENT14_tree=null;
        CommonTree char_literal15_tree=null;
        CommonTree char_literal18_tree=null;
        RewriteRuleTokenStream stream_34 = new RewriteRuleTokenStream(adaptor,"token 34");
        RewriteRuleTokenStream stream_35 = new RewriteRuleTokenStream(adaptor,"token 35");
        RewriteRuleTokenStream stream_36 = new RewriteRuleTokenStream(adaptor,"token 36");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleSubtreeStream stream_pageRule = new RewriteRuleSubtreeStream(adaptor,"rule pageRule");
        RewriteRuleSubtreeStream stream_ruleset = new RewriteRuleSubtreeStream(adaptor,"rule ruleset");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:57:2: ( '@media' IDENT '{' ( pageRule | ruleset )+ '}' -> ^( MEDIA IDENT ( pageRule )* ( ruleset )* ) )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:57:4: '@media' IDENT '{' ( pageRule | ruleset )+ '}'
            {
            	string_literal13 = (IToken)input.LT(1);
            	Match(input,34,FOLLOW_34_in_media252); 
            	stream_34.Add(string_literal13);

            	IDENT14 = (IToken)input.LT(1);
            	Match(input,IDENT,FOLLOW_IDENT_in_media254); 
            	stream_IDENT.Add(IDENT14);

            	char_literal15 = (IToken)input.LT(1);
            	Match(input,35,FOLLOW_35_in_media256); 
            	stream_35.Add(char_literal15);

            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:57:23: ( pageRule | ruleset )+
            	int cnt6 = 0;
            	do 
            	{
            	    int alt6 = 3;
            	    int LA6_0 = input.LA(1);
            	    
            	    if ( (LA6_0 == 37) )
            	    {
            	        alt6 = 1;
            	    }
            	    else if ( ((LA6_0 >= IDENT && LA6_0 <= UNIT) || (LA6_0 >= 42 && LA6_0 <= 46)) )
            	    {
            	        alt6 = 2;
            	    }
            	    
            	
            	    switch (alt6) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:57:24: pageRule
            			    {
            			    	PushFollow(FOLLOW_pageRule_in_media259);
            			    	pageRule16 = pageRule();
            			    	followingStackPointer_--;
            			    	
            			    	stream_pageRule.Add(pageRule16.Tree);
            			    
            			    }
            			    break;
            			case 2 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:57:35: ruleset
            			    {
            			    	PushFollow(FOLLOW_ruleset_in_media263);
            			    	ruleset17 = ruleset();
            			    	followingStackPointer_--;
            			    	
            			    	stream_ruleset.Add(ruleset17.Tree);
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt6 >= 1 ) goto loop6;
            		            EarlyExitException eee =
            		                new EarlyExitException(6, input);
            		            throw eee;
            	    }
            	    cnt6++;
            	} while (true);
            	
            	loop6:
            		;	// Stops C# compiler whinging that label 'loop6' has no statements

            	char_literal18 = (IToken)input.LT(1);
            	Match(input,36,FOLLOW_36_in_media267); 
            	stream_36.Add(char_literal18);

            	
            	// AST REWRITE
            	// elements:          ruleset, pageRule, IDENT
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 57:49: -> ^( MEDIA IDENT ( pageRule )* ( ruleset )* )
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:57:52: ^( MEDIA IDENT ( pageRule )* ( ruleset )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(MEDIA, "MEDIA"), root_1);
            	    
            	    adaptor.AddChild(root_1, stream_IDENT.Next());
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:57:67: ( pageRule )*
            	    while ( stream_pageRule.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_pageRule.Next());
            	    
            	    }
            	    stream_pageRule.Reset();
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:57:77: ( ruleset )*
            	    while ( stream_ruleset.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_ruleset.Next());
            	    
            	    }
            	    stream_ruleset.Reset();
            	    
            	    adaptor.AddChild(root_0, root_1);
            	    }
            	
            	}
            	

            
            }
    
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end media

    public class pageRule_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start pageRule
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:60:1: pageRule : '@page' ( IDENT )* ( pseudo )* '{' ( properties )? ( region )* '}' -> ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* ) ;
    public pageRule_return pageRule() // throws RecognitionException [1]
    {   
        pageRule_return retval = new pageRule_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken string_literal19 = null;
        IToken IDENT20 = null;
        IToken char_literal22 = null;
        IToken char_literal25 = null;
        pseudo_return pseudo21 = null;

        properties_return properties23 = null;

        region_return region24 = null;
        
        
        CommonTree string_literal19_tree=null;
        CommonTree IDENT20_tree=null;
        CommonTree char_literal22_tree=null;
        CommonTree char_literal25_tree=null;
        RewriteRuleTokenStream stream_35 = new RewriteRuleTokenStream(adaptor,"token 35");
        RewriteRuleTokenStream stream_36 = new RewriteRuleTokenStream(adaptor,"token 36");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_37 = new RewriteRuleTokenStream(adaptor,"token 37");
        RewriteRuleSubtreeStream stream_region = new RewriteRuleSubtreeStream(adaptor,"rule region");
        RewriteRuleSubtreeStream stream_pseudo = new RewriteRuleSubtreeStream(adaptor,"rule pseudo");
        RewriteRuleSubtreeStream stream_properties = new RewriteRuleSubtreeStream(adaptor,"rule properties");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:3: ( '@page' ( IDENT )* ( pseudo )* '{' ( properties )? ( region )* '}' -> ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* ) )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:5: '@page' ( IDENT )* ( pseudo )* '{' ( properties )? ( region )* '}'
            {
            	string_literal19 = (IToken)input.LT(1);
            	Match(input,37,FOLLOW_37_in_pageRule295); 
            	stream_37.Add(string_literal19);

            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:13: ( IDENT )*
            	do 
            	{
            	    int alt7 = 2;
            	    int LA7_0 = input.LA(1);
            	    
            	    if ( (LA7_0 == IDENT) )
            	    {
            	        alt7 = 1;
            	    }
            	    
            	
            	    switch (alt7) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:13: IDENT
            			    {
            			    	IDENT20 = (IToken)input.LT(1);
            			    	Match(input,IDENT,FOLLOW_IDENT_in_pageRule297); 
            			    	stream_IDENT.Add(IDENT20);

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop7;
            	    }
            	} while (true);
            	
            	loop7:
            		;	// Stops C# compiler whinging that label 'loop7' has no statements

            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:20: ( pseudo )*
            	do 
            	{
            	    int alt8 = 2;
            	    int LA8_0 = input.LA(1);
            	    
            	    if ( ((LA8_0 >= 45 && LA8_0 <= 46)) )
            	    {
            	        alt8 = 1;
            	    }
            	    
            	
            	    switch (alt8) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:20: pseudo
            			    {
            			    	PushFollow(FOLLOW_pseudo_in_pageRule300);
            			    	pseudo21 = pseudo();
            			    	followingStackPointer_--;
            			    	
            			    	stream_pseudo.Add(pseudo21.Tree);
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop8;
            	    }
            	} while (true);
            	
            	loop8:
            		;	// Stops C# compiler whinging that label 'loop8' has no statements

            	char_literal22 = (IToken)input.LT(1);
            	Match(input,35,FOLLOW_35_in_pageRule303); 
            	stream_35.Add(char_literal22);

            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:32: ( properties )?
            	int alt9 = 2;
            	int LA9_0 = input.LA(1);
            	
            	if ( (LA9_0 == IDENT) )
            	{
            	    alt9 = 1;
            	}
            	switch (alt9) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:32: properties
            	        {
            	        	PushFollow(FOLLOW_properties_in_pageRule305);
            	        	properties23 = properties();
            	        	followingStackPointer_--;
            	        	
            	        	stream_properties.Add(properties23.Tree);
            	        
            	        }
            	        break;
            	
            	}

            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:44: ( region )*
            	do 
            	{
            	    int alt10 = 2;
            	    int LA10_0 = input.LA(1);
            	    
            	    if ( (LA10_0 == 38) )
            	    {
            	        alt10 = 1;
            	    }
            	    
            	
            	    switch (alt10) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:44: region
            			    {
            			    	PushFollow(FOLLOW_region_in_pageRule308);
            			    	region24 = region();
            			    	followingStackPointer_--;
            			    	
            			    	stream_region.Add(region24.Tree);
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop10;
            	    }
            	} while (true);
            	
            	loop10:
            		;	// Stops C# compiler whinging that label 'loop10' has no statements

            	char_literal25 = (IToken)input.LT(1);
            	Match(input,36,FOLLOW_36_in_pageRule311); 
            	stream_36.Add(char_literal25);

            	
            	// AST REWRITE
            	// elements:          region, pseudo, IDENT, properties
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 61:56: -> ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* )
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:59: ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(PAGE, "PAGE"), root_1);
            	    
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:67: ( IDENT )*
            	    while ( stream_IDENT.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_IDENT.Next());
            	    
            	    }
            	    stream_IDENT.Reset();
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:74: ( pseudo )*
            	    while ( stream_pseudo.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_pseudo.Next());
            	    
            	    }
            	    stream_pseudo.Reset();
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:82: ( properties )*
            	    while ( stream_properties.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_properties.Next());
            	    
            	    }
            	    stream_properties.Reset();
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:61:94: ( region )*
            	    while ( stream_region.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_region.Next());
            	    
            	    }
            	    stream_region.Reset();
            	    
            	    adaptor.AddChild(root_0, root_1);
            	    }
            	
            	}
            	

            
            }
    
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end pageRule

    public class region_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start region
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:64:1: region : '@' IDENT '{' ( properties )? '}' -> ^( REGION IDENT ( properties )* ) ;
    public region_return region() // throws RecognitionException [1]
    {   
        region_return retval = new region_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal26 = null;
        IToken IDENT27 = null;
        IToken char_literal28 = null;
        IToken char_literal30 = null;
        properties_return properties29 = null;
        
        
        CommonTree char_literal26_tree=null;
        CommonTree IDENT27_tree=null;
        CommonTree char_literal28_tree=null;
        CommonTree char_literal30_tree=null;
        RewriteRuleTokenStream stream_35 = new RewriteRuleTokenStream(adaptor,"token 35");
        RewriteRuleTokenStream stream_36 = new RewriteRuleTokenStream(adaptor,"token 36");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_38 = new RewriteRuleTokenStream(adaptor,"token 38");
        RewriteRuleSubtreeStream stream_properties = new RewriteRuleSubtreeStream(adaptor,"rule properties");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:65:2: ( '@' IDENT '{' ( properties )? '}' -> ^( REGION IDENT ( properties )* ) )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:65:4: '@' IDENT '{' ( properties )? '}'
            {
            	char_literal26 = (IToken)input.LT(1);
            	Match(input,38,FOLLOW_38_in_region342); 
            	stream_38.Add(char_literal26);

            	IDENT27 = (IToken)input.LT(1);
            	Match(input,IDENT,FOLLOW_IDENT_in_region344); 
            	stream_IDENT.Add(IDENT27);

            	char_literal28 = (IToken)input.LT(1);
            	Match(input,35,FOLLOW_35_in_region346); 
            	stream_35.Add(char_literal28);

            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:65:18: ( properties )?
            	int alt11 = 2;
            	int LA11_0 = input.LA(1);
            	
            	if ( (LA11_0 == IDENT) )
            	{
            	    alt11 = 1;
            	}
            	switch (alt11) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:65:18: properties
            	        {
            	        	PushFollow(FOLLOW_properties_in_region348);
            	        	properties29 = properties();
            	        	followingStackPointer_--;
            	        	
            	        	stream_properties.Add(properties29.Tree);
            	        
            	        }
            	        break;
            	
            	}

            	char_literal30 = (IToken)input.LT(1);
            	Match(input,36,FOLLOW_36_in_region351); 
            	stream_36.Add(char_literal30);

            	
            	// AST REWRITE
            	// elements:          properties, IDENT
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 65:34: -> ^( REGION IDENT ( properties )* )
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:65:37: ^( REGION IDENT ( properties )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(REGION, "REGION"), root_1);
            	    
            	    adaptor.AddChild(root_1, stream_IDENT.Next());
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:65:53: ( properties )*
            	    while ( stream_properties.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_properties.Next());
            	    
            	    }
            	    stream_properties.Reset();
            	    
            	    adaptor.AddChild(root_0, root_1);
            	    }
            	
            	}
            	

            
            }
    
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end region

    public class ruleset_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start ruleset
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:68:1: ruleset : selectors '{' ( properties )? '}' -> ^( RULE selectors ( properties )* ) ;
    public ruleset_return ruleset() // throws RecognitionException [1]
    {   
        ruleset_return retval = new ruleset_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal32 = null;
        IToken char_literal34 = null;
        selectors_return selectors31 = null;

        properties_return properties33 = null;
        
        
        CommonTree char_literal32_tree=null;
        CommonTree char_literal34_tree=null;
        RewriteRuleTokenStream stream_35 = new RewriteRuleTokenStream(adaptor,"token 35");
        RewriteRuleTokenStream stream_36 = new RewriteRuleTokenStream(adaptor,"token 36");
        RewriteRuleSubtreeStream stream_selectors = new RewriteRuleSubtreeStream(adaptor,"rule selectors");
        RewriteRuleSubtreeStream stream_properties = new RewriteRuleSubtreeStream(adaptor,"rule properties");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:69:3: ( selectors '{' ( properties )? '}' -> ^( RULE selectors ( properties )* ) )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:69:5: selectors '{' ( properties )? '}'
            {
            	PushFollow(FOLLOW_selectors_in_ruleset376);
            	selectors31 = selectors();
            	followingStackPointer_--;
            	
            	stream_selectors.Add(selectors31.Tree);
            	char_literal32 = (IToken)input.LT(1);
            	Match(input,35,FOLLOW_35_in_ruleset378); 
            	stream_35.Add(char_literal32);

            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:69:19: ( properties )?
            	int alt12 = 2;
            	int LA12_0 = input.LA(1);
            	
            	if ( (LA12_0 == IDENT) )
            	{
            	    alt12 = 1;
            	}
            	switch (alt12) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:69:19: properties
            	        {
            	        	PushFollow(FOLLOW_properties_in_ruleset380);
            	        	properties33 = properties();
            	        	followingStackPointer_--;
            	        	
            	        	stream_properties.Add(properties33.Tree);
            	        
            	        }
            	        break;
            	
            	}

            	char_literal34 = (IToken)input.LT(1);
            	Match(input,36,FOLLOW_36_in_ruleset383); 
            	stream_36.Add(char_literal34);

            	
            	// AST REWRITE
            	// elements:          properties, selectors
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 69:35: -> ^( RULE selectors ( properties )* )
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:69:38: ^( RULE selectors ( properties )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(RULE, "RULE"), root_1);
            	    
            	    adaptor.AddChild(root_1, stream_selectors.Next());
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:69:56: ( properties )*
            	    while ( stream_properties.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_properties.Next());
            	    
            	    }
            	    stream_properties.Reset();
            	    
            	    adaptor.AddChild(root_0, root_1);
            	    }
            	
            	}
            	

            
            }
    
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end ruleset

    public class selectors_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start selectors
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:72:1: selectors : selector ( ',' selector )* ;
    public selectors_return selectors() // throws RecognitionException [1]
    {   
        selectors_return retval = new selectors_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal36 = null;
        selector_return selector35 = null;

        selector_return selector37 = null;
        
        
        CommonTree char_literal36_tree=null;
    
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:73:2: ( selector ( ',' selector )* )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:73:4: selector ( ',' selector )*
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();
            
            	PushFollow(FOLLOW_selector_in_selectors408);
            	selector35 = selector();
            	followingStackPointer_--;
            	
            	adaptor.AddChild(root_0, selector35.Tree);
            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:73:13: ( ',' selector )*
            	do 
            	{
            	    int alt13 = 2;
            	    int LA13_0 = input.LA(1);
            	    
            	    if ( (LA13_0 == 39) )
            	    {
            	        alt13 = 1;
            	    }
            	    
            	
            	    switch (alt13) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:73:14: ',' selector
            			    {
            			    	char_literal36 = (IToken)input.LT(1);
            			    	Match(input,39,FOLLOW_39_in_selectors411); 
            			    	char_literal36_tree = (CommonTree)adaptor.Create(char_literal36);
            			    	adaptor.AddChild(root_0, char_literal36_tree);

            			    	PushFollow(FOLLOW_selector_in_selectors413);
            			    	selector37 = selector();
            			    	followingStackPointer_--;
            			    	
            			    	adaptor.AddChild(root_0, selector37.Tree);
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop13;
            	    }
            	} while (true);
            	
            	loop13:
            		;	// Stops C# compiler whinging that label 'loop13' has no statements

            
            }
    
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end selectors

    public class selector_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start selector
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:76:1: selector : ( elem ( selectorOperation )* ( pseudo )* -> elem ( selectorOperation )* ( pseudo )* | pseudo -> ANY pseudo );
    public selector_return selector() // throws RecognitionException [1]
    {   
        selector_return retval = new selector_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        elem_return elem38 = null;

        selectorOperation_return selectorOperation39 = null;

        pseudo_return pseudo40 = null;

        pseudo_return pseudo41 = null;
        
        
        RewriteRuleSubtreeStream stream_elem = new RewriteRuleSubtreeStream(adaptor,"rule elem");
        RewriteRuleSubtreeStream stream_selectorOperation = new RewriteRuleSubtreeStream(adaptor,"rule selectorOperation");
        RewriteRuleSubtreeStream stream_pseudo = new RewriteRuleSubtreeStream(adaptor,"rule pseudo");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:77:2: ( elem ( selectorOperation )* ( pseudo )* -> elem ( selectorOperation )* ( pseudo )* | pseudo -> ANY pseudo )
            int alt16 = 2;
            int LA16_0 = input.LA(1);
            
            if ( ((LA16_0 >= IDENT && LA16_0 <= UNIT) || (LA16_0 >= 42 && LA16_0 <= 44)) )
            {
                alt16 = 1;
            }
            else if ( ((LA16_0 >= 45 && LA16_0 <= 46)) )
            {
                alt16 = 2;
            }
            else 
            {
                NoViableAltException nvae_d16s0 =
                    new NoViableAltException("76:1: selector : ( elem ( selectorOperation )* ( pseudo )* -> elem ( selectorOperation )* ( pseudo )* | pseudo -> ANY pseudo );", 16, 0, input);
            
                throw nvae_d16s0;
            }
            switch (alt16) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:77:4: elem ( selectorOperation )* ( pseudo )*
                    {
                    	PushFollow(FOLLOW_elem_in_selector427);
                    	elem38 = elem();
                    	followingStackPointer_--;
                    	
                    	stream_elem.Add(elem38.Tree);
                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:77:9: ( selectorOperation )*
                    	do 
                    	{
                    	    int alt14 = 2;
                    	    int LA14_0 = input.LA(1);
                    	    
                    	    if ( ((LA14_0 >= IDENT && LA14_0 <= UNIT) || (LA14_0 >= 40 && LA14_0 <= 44)) )
                    	    {
                    	        alt14 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt14) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:77:9: selectorOperation
                    			    {
                    			    	PushFollow(FOLLOW_selectorOperation_in_selector429);
                    			    	selectorOperation39 = selectorOperation();
                    			    	followingStackPointer_--;
                    			    	
                    			    	stream_selectorOperation.Add(selectorOperation39.Tree);
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop14;
                    	    }
                    	} while (true);
                    	
                    	loop14:
                    		;	// Stops C# compiler whinging that label 'loop14' has no statements

                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:77:28: ( pseudo )*
                    	do 
                    	{
                    	    int alt15 = 2;
                    	    int LA15_0 = input.LA(1);
                    	    
                    	    if ( ((LA15_0 >= 45 && LA15_0 <= 46)) )
                    	    {
                    	        alt15 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt15) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:77:28: pseudo
                    			    {
                    			    	PushFollow(FOLLOW_pseudo_in_selector432);
                    			    	pseudo40 = pseudo();
                    			    	followingStackPointer_--;
                    			    	
                    			    	stream_pseudo.Add(pseudo40.Tree);
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop15;
                    	    }
                    	} while (true);
                    	
                    	loop15:
                    		;	// Stops C# compiler whinging that label 'loop15' has no statements

                    	
                    	// AST REWRITE
                    	// elements:          elem, selectorOperation, pseudo
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 77:36: -> elem ( selectorOperation )* ( pseudo )*
                    	{
                    	    adaptor.AddChild(root_0, stream_elem.Next());
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:77:45: ( selectorOperation )*
                    	    while ( stream_selectorOperation.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_0, stream_selectorOperation.Next());
                    	    
                    	    }
                    	    stream_selectorOperation.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:77:64: ( pseudo )*
                    	    while ( stream_pseudo.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_0, stream_pseudo.Next());
                    	    
                    	    }
                    	    stream_pseudo.Reset();
                    	
                    	}
                    	

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:78:4: pseudo
                    {
                    	PushFollow(FOLLOW_pseudo_in_selector449);
                    	pseudo41 = pseudo();
                    	followingStackPointer_--;
                    	
                    	stream_pseudo.Add(pseudo41.Tree);
                    	
                    	// AST REWRITE
                    	// elements:          pseudo
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 78:11: -> ANY pseudo
                    	{
                    	    adaptor.AddChild(root_0, adaptor.Create(ANY, "ANY"));
                    	    adaptor.AddChild(root_0, stream_pseudo.Next());
                    	
                    	}
                    	

                    
                    }
                    break;
            
            }
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end selector

    public class selectorOperation_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start selectorOperation
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:81:1: selectorOperation : ( selectop )? elem -> ( selectop )* elem ;
    public selectorOperation_return selectorOperation() // throws RecognitionException [1]
    {   
        selectorOperation_return retval = new selectorOperation_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        selectop_return selectop42 = null;

        elem_return elem43 = null;
        
        
        RewriteRuleSubtreeStream stream_elem = new RewriteRuleSubtreeStream(adaptor,"rule elem");
        RewriteRuleSubtreeStream stream_selectop = new RewriteRuleSubtreeStream(adaptor,"rule selectop");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:82:2: ( ( selectop )? elem -> ( selectop )* elem )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:82:4: ( selectop )? elem
            {
            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:82:4: ( selectop )?
            	int alt17 = 2;
            	int LA17_0 = input.LA(1);
            	
            	if ( ((LA17_0 >= 40 && LA17_0 <= 41)) )
            	{
            	    alt17 = 1;
            	}
            	switch (alt17) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:82:4: selectop
            	        {
            	        	PushFollow(FOLLOW_selectop_in_selectorOperation467);
            	        	selectop42 = selectop();
            	        	followingStackPointer_--;
            	        	
            	        	stream_selectop.Add(selectop42.Tree);
            	        
            	        }
            	        break;
            	
            	}

            	PushFollow(FOLLOW_elem_in_selectorOperation470);
            	elem43 = elem();
            	followingStackPointer_--;
            	
            	stream_elem.Add(elem43.Tree);
            	
            	// AST REWRITE
            	// elements:          elem, selectop
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 82:19: -> ( selectop )* elem
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:82:22: ( selectop )*
            	    while ( stream_selectop.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_selectop.Next());
            	    
            	    }
            	    stream_selectop.Reset();
            	    adaptor.AddChild(root_0, stream_elem.Next());
            	
            	}
            	

            
            }
    
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end selectorOperation

    public class selectop_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start selectop
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:85:1: selectop : ( '>' -> PARENTOF | '+' -> PRECEDES );
    public selectop_return selectop() // throws RecognitionException [1]
    {   
        selectop_return retval = new selectop_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal44 = null;
        IToken char_literal45 = null;
        
        CommonTree char_literal44_tree=null;
        CommonTree char_literal45_tree=null;
        RewriteRuleTokenStream stream_40 = new RewriteRuleTokenStream(adaptor,"token 40");
        RewriteRuleTokenStream stream_41 = new RewriteRuleTokenStream(adaptor,"token 41");
    
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:86:2: ( '>' -> PARENTOF | '+' -> PRECEDES )
            int alt18 = 2;
            int LA18_0 = input.LA(1);
            
            if ( (LA18_0 == 40) )
            {
                alt18 = 1;
            }
            else if ( (LA18_0 == 41) )
            {
                alt18 = 2;
            }
            else 
            {
                NoViableAltException nvae_d18s0 =
                    new NoViableAltException("85:1: selectop : ( '>' -> PARENTOF | '+' -> PRECEDES );", 18, 0, input);
            
                throw nvae_d18s0;
            }
            switch (alt18) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:86:4: '>'
                    {
                    	char_literal44 = (IToken)input.LT(1);
                    	Match(input,40,FOLLOW_40_in_selectop488); 
                    	stream_40.Add(char_literal44);

                    	
                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 86:8: -> PARENTOF
                    	{
                    	    adaptor.AddChild(root_0, adaptor.Create(PARENTOF, "PARENTOF"));
                    	
                    	}
                    	

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:87:11: '+'
                    {
                    	char_literal45 = (IToken)input.LT(1);
                    	Match(input,41,FOLLOW_41_in_selectop504); 
                    	stream_41.Add(char_literal45);

                    	
                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 87:16: -> PRECEDES
                    	{
                    	    adaptor.AddChild(root_0, adaptor.Create(PRECEDES, "PRECEDES"));
                    	
                    	}
                    	

                    
                    }
                    break;
            
            }
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end selectop

    public class properties_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start properties
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:90:1: properties : declaration ( ';' ( declaration )? )* -> ( declaration )+ ;
    public properties_return properties() // throws RecognitionException [1]
    {   
        properties_return retval = new properties_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal47 = null;
        declaration_return declaration46 = null;

        declaration_return declaration48 = null;
        
        
        CommonTree char_literal47_tree=null;
        RewriteRuleTokenStream stream_33 = new RewriteRuleTokenStream(adaptor,"token 33");
        RewriteRuleSubtreeStream stream_declaration = new RewriteRuleSubtreeStream(adaptor,"rule declaration");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:91:2: ( declaration ( ';' ( declaration )? )* -> ( declaration )+ )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:91:4: declaration ( ';' ( declaration )? )*
            {
            	PushFollow(FOLLOW_declaration_in_properties520);
            	declaration46 = declaration();
            	followingStackPointer_--;
            	
            	stream_declaration.Add(declaration46.Tree);
            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:91:16: ( ';' ( declaration )? )*
            	do 
            	{
            	    int alt20 = 2;
            	    int LA20_0 = input.LA(1);
            	    
            	    if ( (LA20_0 == 33) )
            	    {
            	        alt20 = 1;
            	    }
            	    
            	
            	    switch (alt20) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:91:17: ';' ( declaration )?
            			    {
            			    	char_literal47 = (IToken)input.LT(1);
            			    	Match(input,33,FOLLOW_33_in_properties523); 
            			    	stream_33.Add(char_literal47);

            			    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:91:21: ( declaration )?
            			    	int alt19 = 2;
            			    	int LA19_0 = input.LA(1);
            			    	
            			    	if ( (LA19_0 == IDENT) )
            			    	{
            			    	    alt19 = 1;
            			    	}
            			    	switch (alt19) 
            			    	{
            			    	    case 1 :
            			    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:91:21: declaration
            			    	        {
            			    	        	PushFollow(FOLLOW_declaration_in_properties525);
            			    	        	declaration48 = declaration();
            			    	        	followingStackPointer_--;
            			    	        	
            			    	        	stream_declaration.Add(declaration48.Tree);
            			    	        
            			    	        }
            			    	        break;
            			    	
            			    	}

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop20;
            	    }
            	} while (true);
            	
            	loop20:
            		;	// Stops C# compiler whinging that label 'loop20' has no statements

            	
            	// AST REWRITE
            	// elements:          declaration
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 91:36: -> ( declaration )+
            	{
            	    if ( !(stream_declaration.HasNext()) ) {
            	        throw new RewriteEarlyExitException();
            	    }
            	    while ( stream_declaration.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_declaration.Next());
            	    
            	    }
            	    stream_declaration.Reset();
            	
            	}
            	

            
            }
    
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end properties

    public class elem_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start elem
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:94:1: elem : ( ( IDENT | UNIT ) ( attrib )* -> ^( TAG ( IDENT )* ( UNIT )* ( attrib )* ) | '#' ( IDENT | UNIT ) ( attrib )* -> ^( ID ( IDENT )* ( UNIT )* ( attrib )* ) | '.' ( IDENT | UNIT ) ( attrib )* -> ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* ) | '*' ( attrib )* -> ^( ANY ( attrib )* ) );
    public elem_return elem() // throws RecognitionException [1]
    {   
        elem_return retval = new elem_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken IDENT49 = null;
        IToken UNIT50 = null;
        IToken char_literal52 = null;
        IToken IDENT53 = null;
        IToken UNIT54 = null;
        IToken char_literal56 = null;
        IToken IDENT57 = null;
        IToken UNIT58 = null;
        IToken char_literal60 = null;
        attrib_return attrib51 = null;

        attrib_return attrib55 = null;

        attrib_return attrib59 = null;

        attrib_return attrib61 = null;
        
        
        CommonTree IDENT49_tree=null;
        CommonTree UNIT50_tree=null;
        CommonTree char_literal52_tree=null;
        CommonTree IDENT53_tree=null;
        CommonTree UNIT54_tree=null;
        CommonTree char_literal56_tree=null;
        CommonTree IDENT57_tree=null;
        CommonTree UNIT58_tree=null;
        CommonTree char_literal60_tree=null;
        RewriteRuleTokenStream stream_44 = new RewriteRuleTokenStream(adaptor,"token 44");
        RewriteRuleTokenStream stream_UNIT = new RewriteRuleTokenStream(adaptor,"token UNIT");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_42 = new RewriteRuleTokenStream(adaptor,"token 42");
        RewriteRuleTokenStream stream_43 = new RewriteRuleTokenStream(adaptor,"token 43");
        RewriteRuleSubtreeStream stream_attrib = new RewriteRuleSubtreeStream(adaptor,"rule attrib");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:95:2: ( ( IDENT | UNIT ) ( attrib )* -> ^( TAG ( IDENT )* ( UNIT )* ( attrib )* ) | '#' ( IDENT | UNIT ) ( attrib )* -> ^( ID ( IDENT )* ( UNIT )* ( attrib )* ) | '.' ( IDENT | UNIT ) ( attrib )* -> ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* ) | '*' ( attrib )* -> ^( ANY ( attrib )* ) )
            int alt28 = 4;
            switch ( input.LA(1) ) 
            {
            case IDENT:
            case UNIT:
            	{
                alt28 = 1;
                }
                break;
            case 42:
            	{
                alt28 = 2;
                }
                break;
            case 43:
            	{
                alt28 = 3;
                }
                break;
            case 44:
            	{
                alt28 = 4;
                }
                break;
            	default:
            	    NoViableAltException nvae_d28s0 =
            	        new NoViableAltException("94:1: elem : ( ( IDENT | UNIT ) ( attrib )* -> ^( TAG ( IDENT )* ( UNIT )* ( attrib )* ) | '#' ( IDENT | UNIT ) ( attrib )* -> ^( ID ( IDENT )* ( UNIT )* ( attrib )* ) | '.' ( IDENT | UNIT ) ( attrib )* -> ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* ) | '*' ( attrib )* -> ^( ANY ( attrib )* ) );", 28, 0, input);
            
            	    throw nvae_d28s0;
            }
            
            switch (alt28) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:95:8: ( IDENT | UNIT ) ( attrib )*
                    {
                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:95:8: ( IDENT | UNIT )
                    	int alt21 = 2;
                    	int LA21_0 = input.LA(1);
                    	
                    	if ( (LA21_0 == IDENT) )
                    	{
                    	    alt21 = 1;
                    	}
                    	else if ( (LA21_0 == UNIT) )
                    	{
                    	    alt21 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d21s0 =
                    	        new NoViableAltException("95:8: ( IDENT | UNIT )", 21, 0, input);
                    	
                    	    throw nvae_d21s0;
                    	}
                    	switch (alt21) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:95:9: IDENT
                    	        {
                    	        	IDENT49 = (IToken)input.LT(1);
                    	        	Match(input,IDENT,FOLLOW_IDENT_in_elem551); 
                    	        	stream_IDENT.Add(IDENT49);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:95:17: UNIT
                    	        {
                    	        	UNIT50 = (IToken)input.LT(1);
                    	        	Match(input,UNIT,FOLLOW_UNIT_in_elem555); 
                    	        	stream_UNIT.Add(UNIT50);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:95:23: ( attrib )*
                    	do 
                    	{
                    	    int alt22 = 2;
                    	    int LA22_0 = input.LA(1);
                    	    
                    	    if ( (LA22_0 == 47) )
                    	    {
                    	        alt22 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt22) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:95:23: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem558);
                    			    	attrib51 = attrib();
                    			    	followingStackPointer_--;
                    			    	
                    			    	stream_attrib.Add(attrib51.Tree);
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop22;
                    	    }
                    	} while (true);
                    	
                    	loop22:
                    		;	// Stops C# compiler whinging that label 'loop22' has no statements

                    	
                    	// AST REWRITE
                    	// elements:          IDENT, UNIT, attrib
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 95:31: -> ^( TAG ( IDENT )* ( UNIT )* ( attrib )* )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:95:34: ^( TAG ( IDENT )* ( UNIT )* ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(TAG, "TAG"), root_1);
                    	    
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:95:41: ( IDENT )*
                    	    while ( stream_IDENT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_IDENT.Next());
                    	    
                    	    }
                    	    stream_IDENT.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:95:48: ( UNIT )*
                    	    while ( stream_UNIT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_UNIT.Next());
                    	    
                    	    }
                    	    stream_UNIT.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:95:54: ( attrib )*
                    	    while ( stream_attrib.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_attrib.Next());
                    	    
                    	    }
                    	    stream_attrib.Reset();
                    	    
                    	    adaptor.AddChild(root_0, root_1);
                    	    }
                    	
                    	}
                    	

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:96:4: '#' ( IDENT | UNIT ) ( attrib )*
                    {
                    	char_literal52 = (IToken)input.LT(1);
                    	Match(input,42,FOLLOW_42_in_elem581); 
                    	stream_42.Add(char_literal52);

                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:96:8: ( IDENT | UNIT )
                    	int alt23 = 2;
                    	int LA23_0 = input.LA(1);
                    	
                    	if ( (LA23_0 == IDENT) )
                    	{
                    	    alt23 = 1;
                    	}
                    	else if ( (LA23_0 == UNIT) )
                    	{
                    	    alt23 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d23s0 =
                    	        new NoViableAltException("96:8: ( IDENT | UNIT )", 23, 0, input);
                    	
                    	    throw nvae_d23s0;
                    	}
                    	switch (alt23) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:96:9: IDENT
                    	        {
                    	        	IDENT53 = (IToken)input.LT(1);
                    	        	Match(input,IDENT,FOLLOW_IDENT_in_elem584); 
                    	        	stream_IDENT.Add(IDENT53);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:96:17: UNIT
                    	        {
                    	        	UNIT54 = (IToken)input.LT(1);
                    	        	Match(input,UNIT,FOLLOW_UNIT_in_elem588); 
                    	        	stream_UNIT.Add(UNIT54);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:96:23: ( attrib )*
                    	do 
                    	{
                    	    int alt24 = 2;
                    	    int LA24_0 = input.LA(1);
                    	    
                    	    if ( (LA24_0 == 47) )
                    	    {
                    	        alt24 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt24) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:96:23: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem591);
                    			    	attrib55 = attrib();
                    			    	followingStackPointer_--;
                    			    	
                    			    	stream_attrib.Add(attrib55.Tree);
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop24;
                    	    }
                    	} while (true);
                    	
                    	loop24:
                    		;	// Stops C# compiler whinging that label 'loop24' has no statements

                    	
                    	// AST REWRITE
                    	// elements:          UNIT, IDENT, attrib
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 96:31: -> ^( ID ( IDENT )* ( UNIT )* ( attrib )* )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:96:34: ^( ID ( IDENT )* ( UNIT )* ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(ID, "ID"), root_1);
                    	    
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:96:40: ( IDENT )*
                    	    while ( stream_IDENT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_IDENT.Next());
                    	    
                    	    }
                    	    stream_IDENT.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:96:47: ( UNIT )*
                    	    while ( stream_UNIT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_UNIT.Next());
                    	    
                    	    }
                    	    stream_UNIT.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:96:53: ( attrib )*
                    	    while ( stream_attrib.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_attrib.Next());
                    	    
                    	    }
                    	    stream_attrib.Reset();
                    	    
                    	    adaptor.AddChild(root_0, root_1);
                    	    }
                    	
                    	}
                    	

                    
                    }
                    break;
                case 3 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:97:4: '.' ( IDENT | UNIT ) ( attrib )*
                    {
                    	char_literal56 = (IToken)input.LT(1);
                    	Match(input,43,FOLLOW_43_in_elem614); 
                    	stream_43.Add(char_literal56);

                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:97:8: ( IDENT | UNIT )
                    	int alt25 = 2;
                    	int LA25_0 = input.LA(1);
                    	
                    	if ( (LA25_0 == IDENT) )
                    	{
                    	    alt25 = 1;
                    	}
                    	else if ( (LA25_0 == UNIT) )
                    	{
                    	    alt25 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d25s0 =
                    	        new NoViableAltException("97:8: ( IDENT | UNIT )", 25, 0, input);
                    	
                    	    throw nvae_d25s0;
                    	}
                    	switch (alt25) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:97:9: IDENT
                    	        {
                    	        	IDENT57 = (IToken)input.LT(1);
                    	        	Match(input,IDENT,FOLLOW_IDENT_in_elem617); 
                    	        	stream_IDENT.Add(IDENT57);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:97:17: UNIT
                    	        {
                    	        	UNIT58 = (IToken)input.LT(1);
                    	        	Match(input,UNIT,FOLLOW_UNIT_in_elem621); 
                    	        	stream_UNIT.Add(UNIT58);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:97:23: ( attrib )*
                    	do 
                    	{
                    	    int alt26 = 2;
                    	    int LA26_0 = input.LA(1);
                    	    
                    	    if ( (LA26_0 == 47) )
                    	    {
                    	        alt26 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt26) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:97:23: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem624);
                    			    	attrib59 = attrib();
                    			    	followingStackPointer_--;
                    			    	
                    			    	stream_attrib.Add(attrib59.Tree);
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop26;
                    	    }
                    	} while (true);
                    	
                    	loop26:
                    		;	// Stops C# compiler whinging that label 'loop26' has no statements

                    	
                    	// AST REWRITE
                    	// elements:          attrib, IDENT, UNIT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 97:31: -> ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:97:34: ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(CLASS, "CLASS"), root_1);
                    	    
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:97:43: ( IDENT )*
                    	    while ( stream_IDENT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_IDENT.Next());
                    	    
                    	    }
                    	    stream_IDENT.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:97:50: ( UNIT )*
                    	    while ( stream_UNIT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_UNIT.Next());
                    	    
                    	    }
                    	    stream_UNIT.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:97:56: ( attrib )*
                    	    while ( stream_attrib.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_attrib.Next());
                    	    
                    	    }
                    	    stream_attrib.Reset();
                    	    
                    	    adaptor.AddChild(root_0, root_1);
                    	    }
                    	
                    	}
                    	

                    
                    }
                    break;
                case 4 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:98:4: '*' ( attrib )*
                    {
                    	char_literal60 = (IToken)input.LT(1);
                    	Match(input,44,FOLLOW_44_in_elem647); 
                    	stream_44.Add(char_literal60);

                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:98:8: ( attrib )*
                    	do 
                    	{
                    	    int alt27 = 2;
                    	    int LA27_0 = input.LA(1);
                    	    
                    	    if ( (LA27_0 == 47) )
                    	    {
                    	        alt27 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt27) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:98:8: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem649);
                    			    	attrib61 = attrib();
                    			    	followingStackPointer_--;
                    			    	
                    			    	stream_attrib.Add(attrib61.Tree);
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop27;
                    	    }
                    	} while (true);
                    	
                    	loop27:
                    		;	// Stops C# compiler whinging that label 'loop27' has no statements

                    	
                    	// AST REWRITE
                    	// elements:          attrib
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 98:16: -> ^( ANY ( attrib )* )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:98:19: ^( ANY ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(ANY, "ANY"), root_1);
                    	    
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:98:26: ( attrib )*
                    	    while ( stream_attrib.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_attrib.Next());
                    	    
                    	    }
                    	    stream_attrib.Reset();
                    	    
                    	    adaptor.AddChild(root_0, root_1);
                    	    }
                    	
                    	}
                    	

                    
                    }
                    break;
            
            }
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end elem

    public class pseudo_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start pseudo
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:101:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );
    public pseudo_return pseudo() // throws RecognitionException [1]
    {   
        pseudo_return retval = new pseudo_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal62 = null;
        IToken string_literal63 = null;
        IToken IDENT64 = null;
        IToken char_literal65 = null;
        IToken string_literal66 = null;
        function_return function67 = null;
        
        
        CommonTree char_literal62_tree=null;
        CommonTree string_literal63_tree=null;
        CommonTree IDENT64_tree=null;
        CommonTree char_literal65_tree=null;
        CommonTree string_literal66_tree=null;
        RewriteRuleTokenStream stream_45 = new RewriteRuleTokenStream(adaptor,"token 45");
        RewriteRuleTokenStream stream_46 = new RewriteRuleTokenStream(adaptor,"token 46");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleSubtreeStream stream_function = new RewriteRuleSubtreeStream(adaptor,"rule function");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:102:2: ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) )
            int alt31 = 2;
            int LA31_0 = input.LA(1);
            
            if ( (LA31_0 == 45) )
            {
                int LA31_1 = input.LA(2);
                
                if ( (LA31_1 == IDENT) )
                {
                    int LA31_3 = input.LA(3);
                    
                    if ( (LA31_3 == 53) )
                    {
                        alt31 = 2;
                    }
                    else if ( (LA31_3 == 35 || LA31_3 == 39 || (LA31_3 >= 45 && LA31_3 <= 46)) )
                    {
                        alt31 = 1;
                    }
                    else 
                    {
                        NoViableAltException nvae_d31s3 =
                            new NoViableAltException("101:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );", 31, 3, input);
                    
                        throw nvae_d31s3;
                    }
                }
                else 
                {
                    NoViableAltException nvae_d31s1 =
                        new NoViableAltException("101:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );", 31, 1, input);
                
                    throw nvae_d31s1;
                }
            }
            else if ( (LA31_0 == 46) )
            {
                int LA31_2 = input.LA(2);
                
                if ( (LA31_2 == IDENT) )
                {
                    int LA31_3 = input.LA(3);
                    
                    if ( (LA31_3 == 53) )
                    {
                        alt31 = 2;
                    }
                    else if ( (LA31_3 == 35 || LA31_3 == 39 || (LA31_3 >= 45 && LA31_3 <= 46)) )
                    {
                        alt31 = 1;
                    }
                    else 
                    {
                        NoViableAltException nvae_d31s3 =
                            new NoViableAltException("101:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );", 31, 3, input);
                    
                        throw nvae_d31s3;
                    }
                }
                else 
                {
                    NoViableAltException nvae_d31s2 =
                        new NoViableAltException("101:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );", 31, 2, input);
                
                    throw nvae_d31s2;
                }
            }
            else 
            {
                NoViableAltException nvae_d31s0 =
                    new NoViableAltException("101:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );", 31, 0, input);
            
                throw nvae_d31s0;
            }
            switch (alt31) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:102:4: ( ':' | '::' ) IDENT
                    {
                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:102:4: ( ':' | '::' )
                    	int alt29 = 2;
                    	int LA29_0 = input.LA(1);
                    	
                    	if ( (LA29_0 == 45) )
                    	{
                    	    alt29 = 1;
                    	}
                    	else if ( (LA29_0 == 46) )
                    	{
                    	    alt29 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d29s0 =
                    	        new NoViableAltException("102:4: ( ':' | '::' )", 29, 0, input);
                    	
                    	    throw nvae_d29s0;
                    	}
                    	switch (alt29) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:102:5: ':'
                    	        {
                    	        	char_literal62 = (IToken)input.LT(1);
                    	        	Match(input,45,FOLLOW_45_in_pseudo673); 
                    	        	stream_45.Add(char_literal62);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:102:9: '::'
                    	        {
                    	        	string_literal63 = (IToken)input.LT(1);
                    	        	Match(input,46,FOLLOW_46_in_pseudo675); 
                    	        	stream_46.Add(string_literal63);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	IDENT64 = (IToken)input.LT(1);
                    	Match(input,IDENT,FOLLOW_IDENT_in_pseudo678); 
                    	stream_IDENT.Add(IDENT64);

                    	
                    	// AST REWRITE
                    	// elements:          IDENT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 102:21: -> ^( PSEUDO IDENT )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:102:24: ^( PSEUDO IDENT )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(PSEUDO, "PSEUDO"), root_1);
                    	    
                    	    adaptor.AddChild(root_1, stream_IDENT.Next());
                    	    
                    	    adaptor.AddChild(root_0, root_1);
                    	    }
                    	
                    	}
                    	

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:103:4: ( ':' | '::' ) function
                    {
                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:103:4: ( ':' | '::' )
                    	int alt30 = 2;
                    	int LA30_0 = input.LA(1);
                    	
                    	if ( (LA30_0 == 45) )
                    	{
                    	    alt30 = 1;
                    	}
                    	else if ( (LA30_0 == 46) )
                    	{
                    	    alt30 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d30s0 =
                    	        new NoViableAltException("103:4: ( ':' | '::' )", 30, 0, input);
                    	
                    	    throw nvae_d30s0;
                    	}
                    	switch (alt30) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:103:5: ':'
                    	        {
                    	        	char_literal65 = (IToken)input.LT(1);
                    	        	Match(input,45,FOLLOW_45_in_pseudo694); 
                    	        	stream_45.Add(char_literal65);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:103:9: '::'
                    	        {
                    	        	string_literal66 = (IToken)input.LT(1);
                    	        	Match(input,46,FOLLOW_46_in_pseudo696); 
                    	        	stream_46.Add(string_literal66);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	PushFollow(FOLLOW_function_in_pseudo699);
                    	function67 = function();
                    	followingStackPointer_--;
                    	
                    	stream_function.Add(function67.Tree);
                    	
                    	// AST REWRITE
                    	// elements:          function
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 103:24: -> ^( PSEUDO function )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:103:27: ^( PSEUDO function )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(PSEUDO, "PSEUDO"), root_1);
                    	    
                    	    adaptor.AddChild(root_1, stream_function.Next());
                    	    
                    	    adaptor.AddChild(root_0, root_1);
                    	    }
                    	
                    	}
                    	

                    
                    }
                    break;
            
            }
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end pseudo

    public class attrib_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start attrib
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:106:1: attrib : '[' IDENT ( attribRelate ( STRING | IDENT ) )? ']' -> ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? ) ;
    public attrib_return attrib() // throws RecognitionException [1]
    {   
        attrib_return retval = new attrib_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal68 = null;
        IToken IDENT69 = null;
        IToken STRING71 = null;
        IToken IDENT72 = null;
        IToken char_literal73 = null;
        attribRelate_return attribRelate70 = null;
        
        
        CommonTree char_literal68_tree=null;
        CommonTree IDENT69_tree=null;
        CommonTree STRING71_tree=null;
        CommonTree IDENT72_tree=null;
        CommonTree char_literal73_tree=null;
        RewriteRuleTokenStream stream_47 = new RewriteRuleTokenStream(adaptor,"token 47");
        RewriteRuleTokenStream stream_48 = new RewriteRuleTokenStream(adaptor,"token 48");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_STRING = new RewriteRuleTokenStream(adaptor,"token STRING");
        RewriteRuleSubtreeStream stream_attribRelate = new RewriteRuleSubtreeStream(adaptor,"rule attribRelate");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:107:2: ( '[' IDENT ( attribRelate ( STRING | IDENT ) )? ']' -> ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? ) )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:107:4: '[' IDENT ( attribRelate ( STRING | IDENT ) )? ']'
            {
            	char_literal68 = (IToken)input.LT(1);
            	Match(input,47,FOLLOW_47_in_attrib720); 
            	stream_47.Add(char_literal68);

            	IDENT69 = (IToken)input.LT(1);
            	Match(input,IDENT,FOLLOW_IDENT_in_attrib722); 
            	stream_IDENT.Add(IDENT69);

            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:107:14: ( attribRelate ( STRING | IDENT ) )?
            	int alt33 = 2;
            	int LA33_0 = input.LA(1);
            	
            	if ( ((LA33_0 >= 49 && LA33_0 <= 51)) )
            	{
            	    alt33 = 1;
            	}
            	switch (alt33) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:107:15: attribRelate ( STRING | IDENT )
            	        {
            	        	PushFollow(FOLLOW_attribRelate_in_attrib725);
            	        	attribRelate70 = attribRelate();
            	        	followingStackPointer_--;
            	        	
            	        	stream_attribRelate.Add(attribRelate70.Tree);
            	        	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:107:28: ( STRING | IDENT )
            	        	int alt32 = 2;
            	        	int LA32_0 = input.LA(1);
            	        	
            	        	if ( (LA32_0 == STRING) )
            	        	{
            	        	    alt32 = 1;
            	        	}
            	        	else if ( (LA32_0 == IDENT) )
            	        	{
            	        	    alt32 = 2;
            	        	}
            	        	else 
            	        	{
            	        	    NoViableAltException nvae_d32s0 =
            	        	        new NoViableAltException("107:28: ( STRING | IDENT )", 32, 0, input);
            	        	
            	        	    throw nvae_d32s0;
            	        	}
            	        	switch (alt32) 
            	        	{
            	        	    case 1 :
            	        	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:107:29: STRING
            	        	        {
            	        	        	STRING71 = (IToken)input.LT(1);
            	        	        	Match(input,STRING,FOLLOW_STRING_in_attrib728); 
            	        	        	stream_STRING.Add(STRING71);

            	        	        
            	        	        }
            	        	        break;
            	        	    case 2 :
            	        	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:107:38: IDENT
            	        	        {
            	        	        	IDENT72 = (IToken)input.LT(1);
            	        	        	Match(input,IDENT,FOLLOW_IDENT_in_attrib732); 
            	        	        	stream_IDENT.Add(IDENT72);

            	        	        
            	        	        }
            	        	        break;
            	        	
            	        	}

            	        
            	        }
            	        break;
            	
            	}

            	char_literal73 = (IToken)input.LT(1);
            	Match(input,48,FOLLOW_48_in_attrib737); 
            	stream_48.Add(char_literal73);

            	
            	// AST REWRITE
            	// elements:          IDENT, IDENT, attribRelate, STRING
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 107:51: -> ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? )
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:107:54: ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(ATTRIB, "ATTRIB"), root_1);
            	    
            	    adaptor.AddChild(root_1, stream_IDENT.Next());
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:107:70: ( attribRelate ( STRING )* ( IDENT )* )?
            	    if ( stream_IDENT.HasNext() || stream_attribRelate.HasNext() || stream_STRING.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_attribRelate.Next());
            	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:107:84: ( STRING )*
            	        while ( stream_STRING.HasNext() )
            	        {
            	            adaptor.AddChild(root_1, stream_STRING.Next());
            	        
            	        }
            	        stream_STRING.Reset();
            	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:107:92: ( IDENT )*
            	        while ( stream_IDENT.HasNext() )
            	        {
            	            adaptor.AddChild(root_1, stream_IDENT.Next());
            	        
            	        }
            	        stream_IDENT.Reset();
            	    
            	    }
            	    stream_IDENT.Reset();
            	    stream_attribRelate.Reset();
            	    stream_STRING.Reset();
            	    
            	    adaptor.AddChild(root_0, root_1);
            	    }
            	
            	}
            	

            
            }
    
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end attrib

    public class attribRelate_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start attribRelate
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:110:1: attribRelate : ( '=' -> ATTRIBEQUAL | '~=' -> HASVALUE | '|=' -> BEGINSWITH );
    public attribRelate_return attribRelate() // throws RecognitionException [1]
    {   
        attribRelate_return retval = new attribRelate_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal74 = null;
        IToken string_literal75 = null;
        IToken string_literal76 = null;
        
        CommonTree char_literal74_tree=null;
        CommonTree string_literal75_tree=null;
        CommonTree string_literal76_tree=null;
        RewriteRuleTokenStream stream_49 = new RewriteRuleTokenStream(adaptor,"token 49");
        RewriteRuleTokenStream stream_50 = new RewriteRuleTokenStream(adaptor,"token 50");
        RewriteRuleTokenStream stream_51 = new RewriteRuleTokenStream(adaptor,"token 51");
    
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:111:2: ( '=' -> ATTRIBEQUAL | '~=' -> HASVALUE | '|=' -> BEGINSWITH )
            int alt34 = 3;
            switch ( input.LA(1) ) 
            {
            case 49:
            	{
                alt34 = 1;
                }
                break;
            case 50:
            	{
                alt34 = 2;
                }
                break;
            case 51:
            	{
                alt34 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d34s0 =
            	        new NoViableAltException("110:1: attribRelate : ( '=' -> ATTRIBEQUAL | '~=' -> HASVALUE | '|=' -> BEGINSWITH );", 34, 0, input);
            
            	    throw nvae_d34s0;
            }
            
            switch (alt34) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:111:4: '='
                    {
                    	char_literal74 = (IToken)input.LT(1);
                    	Match(input,49,FOLLOW_49_in_attribRelate770); 
                    	stream_49.Add(char_literal74);

                    	
                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 111:9: -> ATTRIBEQUAL
                    	{
                    	    adaptor.AddChild(root_0, adaptor.Create(ATTRIBEQUAL, "ATTRIBEQUAL"));
                    	
                    	}
                    	

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:112:4: '~='
                    {
                    	string_literal75 = (IToken)input.LT(1);
                    	Match(input,50,FOLLOW_50_in_attribRelate780); 
                    	stream_50.Add(string_literal75);

                    	
                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 112:9: -> HASVALUE
                    	{
                    	    adaptor.AddChild(root_0, adaptor.Create(HASVALUE, "HASVALUE"));
                    	
                    	}
                    	

                    
                    }
                    break;
                case 3 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:113:4: '|='
                    {
                    	string_literal76 = (IToken)input.LT(1);
                    	Match(input,51,FOLLOW_51_in_attribRelate789); 
                    	stream_51.Add(string_literal76);

                    	
                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 113:9: -> BEGINSWITH
                    	{
                    	    adaptor.AddChild(root_0, adaptor.Create(BEGINSWITH, "BEGINSWITH"));
                    	
                    	}
                    	

                    
                    }
                    break;
            
            }
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end attribRelate

    public class declaration_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start declaration
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:116:1: declaration : IDENT ':' args -> ^( PROPERTY IDENT args ) ;
    public declaration_return declaration() // throws RecognitionException [1]
    {   
        declaration_return retval = new declaration_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken IDENT77 = null;
        IToken char_literal78 = null;
        args_return args79 = null;
        
        
        CommonTree IDENT77_tree=null;
        CommonTree char_literal78_tree=null;
        RewriteRuleTokenStream stream_45 = new RewriteRuleTokenStream(adaptor,"token 45");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleSubtreeStream stream_args = new RewriteRuleSubtreeStream(adaptor,"rule args");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:117:2: ( IDENT ':' args -> ^( PROPERTY IDENT args ) )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:117:4: IDENT ':' args
            {
            	IDENT77 = (IToken)input.LT(1);
            	Match(input,IDENT,FOLLOW_IDENT_in_declaration807); 
            	stream_IDENT.Add(IDENT77);

            	char_literal78 = (IToken)input.LT(1);
            	Match(input,45,FOLLOW_45_in_declaration809); 
            	stream_45.Add(char_literal78);

            	PushFollow(FOLLOW_args_in_declaration811);
            	args79 = args();
            	followingStackPointer_--;
            	
            	stream_args.Add(args79.Tree);
            	
            	// AST REWRITE
            	// elements:          IDENT, args
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 117:19: -> ^( PROPERTY IDENT args )
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:117:22: ^( PROPERTY IDENT args )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(PROPERTY, "PROPERTY"), root_1);
            	    
            	    adaptor.AddChild(root_1, stream_IDENT.Next());
            	    adaptor.AddChild(root_1, stream_args.Next());
            	    
            	    adaptor.AddChild(root_0, root_1);
            	    }
            	
            	}
            	

            
            }
    
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end declaration

    public class args_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start args
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:120:1: args : expr ( ( ',' )? expr )* -> ( expr )* ;
    public args_return args() // throws RecognitionException [1]
    {   
        args_return retval = new args_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal81 = null;
        expr_return expr80 = null;

        expr_return expr82 = null;
        
        
        CommonTree char_literal81_tree=null;
        RewriteRuleTokenStream stream_39 = new RewriteRuleTokenStream(adaptor,"token 39");
        RewriteRuleSubtreeStream stream_expr = new RewriteRuleSubtreeStream(adaptor,"rule expr");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:121:2: ( expr ( ( ',' )? expr )* -> ( expr )* )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:121:4: expr ( ( ',' )? expr )*
            {
            	PushFollow(FOLLOW_expr_in_args834);
            	expr80 = expr();
            	followingStackPointer_--;
            	
            	stream_expr.Add(expr80.Tree);
            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:121:9: ( ( ',' )? expr )*
            	do 
            	{
            	    int alt36 = 2;
            	    int LA36_0 = input.LA(1);
            	    
            	    if ( ((LA36_0 >= STRING && LA36_0 <= IDENT) || (LA36_0 >= NUM && LA36_0 <= COLOR) || LA36_0 == 39) )
            	    {
            	        alt36 = 1;
            	    }
            	    
            	
            	    switch (alt36) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:121:10: ( ',' )? expr
            			    {
            			    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:121:10: ( ',' )?
            			    	int alt35 = 2;
            			    	int LA35_0 = input.LA(1);
            			    	
            			    	if ( (LA35_0 == 39) )
            			    	{
            			    	    alt35 = 1;
            			    	}
            			    	switch (alt35) 
            			    	{
            			    	    case 1 :
            			    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:121:10: ','
            			    	        {
            			    	        	char_literal81 = (IToken)input.LT(1);
            			    	        	Match(input,39,FOLLOW_39_in_args837); 
            			    	        	stream_39.Add(char_literal81);

            			    	        
            			    	        }
            			    	        break;
            			    	
            			    	}

            			    	PushFollow(FOLLOW_expr_in_args840);
            			    	expr82 = expr();
            			    	followingStackPointer_--;
            			    	
            			    	stream_expr.Add(expr82.Tree);
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop36;
            	    }
            	} while (true);
            	
            	loop36:
            		;	// Stops C# compiler whinging that label 'loop36' has no statements

            	
            	// AST REWRITE
            	// elements:          expr
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 121:22: -> ( expr )*
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:121:25: ( expr )*
            	    while ( stream_expr.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_expr.Next());
            	    
            	    }
            	    stream_expr.Reset();
            	
            	}
            	

            
            }
    
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end args

    public class expr_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start expr
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:124:1: expr : ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function );
    public expr_return expr() // throws RecognitionException [1]
    {   
        expr_return retval = new expr_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken NUM83 = null;
        IToken IDENT85 = null;
        IToken COLOR86 = null;
        IToken STRING87 = null;
        unit_return unit84 = null;

        function_return function88 = null;
        
        
        CommonTree NUM83_tree=null;
        CommonTree IDENT85_tree=null;
        CommonTree COLOR86_tree=null;
        CommonTree STRING87_tree=null;
    
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:125:2: ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function )
            int alt38 = 5;
            switch ( input.LA(1) ) 
            {
            case NUM:
            	{
                alt38 = 1;
                }
                break;
            case IDENT:
            	{
                int LA38_2 = input.LA(2);
                
                if ( (LA38_2 == 53) )
                {
                    alt38 = 5;
                }
                else if ( ((LA38_2 >= STRING && LA38_2 <= IDENT) || (LA38_2 >= NUM && LA38_2 <= COLOR) || LA38_2 == 33 || LA38_2 == 36 || (LA38_2 >= 38 && LA38_2 <= 39) || LA38_2 == 54) )
                {
                    alt38 = 2;
                }
                else 
                {
                    NoViableAltException nvae_d38s2 =
                        new NoViableAltException("124:1: expr : ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function );", 38, 2, input);
                
                    throw nvae_d38s2;
                }
                }
                break;
            case COLOR:
            	{
                alt38 = 3;
                }
                break;
            case STRING:
            	{
                alt38 = 4;
                }
                break;
            	default:
            	    NoViableAltException nvae_d38s0 =
            	        new NoViableAltException("124:1: expr : ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function );", 38, 0, input);
            
            	    throw nvae_d38s0;
            }
            
            switch (alt38) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:125:4: ( NUM ( unit )? )
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    
                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:125:4: ( NUM ( unit )? )
                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:125:5: NUM ( unit )?
                    	{
                    		NUM83 = (IToken)input.LT(1);
                    		Match(input,NUM,FOLLOW_NUM_in_expr859); 
                    		NUM83_tree = (CommonTree)adaptor.Create(NUM83);
                    		adaptor.AddChild(root_0, NUM83_tree);

                    		// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:125:9: ( unit )?
                    		int alt37 = 2;
                    		int LA37_0 = input.LA(1);
                    		
                    		if ( (LA37_0 == UNIT || LA37_0 == 52) )
                    		{
                    		    alt37 = 1;
                    		}
                    		switch (alt37) 
                    		{
                    		    case 1 :
                    		        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:125:9: unit
                    		        {
                    		        	PushFollow(FOLLOW_unit_in_expr861);
                    		        	unit84 = unit();
                    		        	followingStackPointer_--;
                    		        	
                    		        	adaptor.AddChild(root_0, unit84.Tree);
                    		        
                    		        }
                    		        break;
                    		
                    		}

                    	
                    	}

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:126:4: IDENT
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    
                    	IDENT85 = (IToken)input.LT(1);
                    	Match(input,IDENT,FOLLOW_IDENT_in_expr868); 
                    	IDENT85_tree = (CommonTree)adaptor.Create(IDENT85);
                    	adaptor.AddChild(root_0, IDENT85_tree);

                    
                    }
                    break;
                case 3 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:127:4: COLOR
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    
                    	COLOR86 = (IToken)input.LT(1);
                    	Match(input,COLOR,FOLLOW_COLOR_in_expr873); 
                    	COLOR86_tree = (CommonTree)adaptor.Create(COLOR86);
                    	adaptor.AddChild(root_0, COLOR86_tree);

                    
                    }
                    break;
                case 4 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:128:4: STRING
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    
                    	STRING87 = (IToken)input.LT(1);
                    	Match(input,STRING,FOLLOW_STRING_in_expr878); 
                    	STRING87_tree = (CommonTree)adaptor.Create(STRING87);
                    	adaptor.AddChild(root_0, STRING87_tree);

                    
                    }
                    break;
                case 5 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:129:4: function
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    
                    	PushFollow(FOLLOW_function_in_expr883);
                    	function88 = function();
                    	followingStackPointer_--;
                    	
                    	adaptor.AddChild(root_0, function88.Tree);
                    
                    }
                    break;
            
            }
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end expr

    public class unit_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start unit
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:132:1: unit : ( '%' | UNIT ) ;
    public unit_return unit() // throws RecognitionException [1]
    {   
        unit_return retval = new unit_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken set89 = null;
        
        CommonTree set89_tree=null;
    
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:133:2: ( ( '%' | UNIT ) )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:133:4: ( '%' | UNIT )
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();
            
            	set89 = (IToken)input.LT(1);
            	if ( input.LA(1) == UNIT || input.LA(1) == 52 ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, adaptor.Create(set89));
            	    errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse =
            	        new MismatchedSetException(null,input);
            	    RecoverFromMismatchedSet(input,mse,FOLLOW_set_in_unit894);    throw mse;
            	}

            
            }
    
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end unit

    public class function_return : ParserRuleReturnScope 
    {
        internal CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        }
    };
    
    // $ANTLR start function
    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:136:1: function : IDENT '(' ( args )? ')' -> IDENT '(' ( args )* ')' ;
    public function_return function() // throws RecognitionException [1]
    {   
        function_return retval = new function_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken IDENT90 = null;
        IToken char_literal91 = null;
        IToken char_literal93 = null;
        args_return args92 = null;
        
        
        CommonTree IDENT90_tree=null;
        CommonTree char_literal91_tree=null;
        CommonTree char_literal93_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_53 = new RewriteRuleTokenStream(adaptor,"token 53");
        RewriteRuleTokenStream stream_54 = new RewriteRuleTokenStream(adaptor,"token 54");
        RewriteRuleSubtreeStream stream_args = new RewriteRuleSubtreeStream(adaptor,"rule args");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:137:2: ( IDENT '(' ( args )? ')' -> IDENT '(' ( args )* ')' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:137:4: IDENT '(' ( args )? ')'
            {
            	IDENT90 = (IToken)input.LT(1);
            	Match(input,IDENT,FOLLOW_IDENT_in_function911); 
            	stream_IDENT.Add(IDENT90);

            	char_literal91 = (IToken)input.LT(1);
            	Match(input,53,FOLLOW_53_in_function913); 
            	stream_53.Add(char_literal91);

            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:137:14: ( args )?
            	int alt39 = 2;
            	int LA39_0 = input.LA(1);
            	
            	if ( ((LA39_0 >= STRING && LA39_0 <= IDENT) || (LA39_0 >= NUM && LA39_0 <= COLOR)) )
            	{
            	    alt39 = 1;
            	}
            	switch (alt39) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:137:14: args
            	        {
            	        	PushFollow(FOLLOW_args_in_function915);
            	        	args92 = args();
            	        	followingStackPointer_--;
            	        	
            	        	stream_args.Add(args92.Tree);
            	        
            	        }
            	        break;
            	
            	}

            	char_literal93 = (IToken)input.LT(1);
            	Match(input,54,FOLLOW_54_in_function918); 
            	stream_54.Add(char_literal93);

            	
            	// AST REWRITE
            	// elements:          args, IDENT, 54, 53
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 137:24: -> IDENT '(' ( args )* ')'
            	{
            	    adaptor.AddChild(root_0, stream_IDENT.Next());
            	    adaptor.AddChild(root_0, stream_53.Next());
            	    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:137:37: ( args )*
            	    while ( stream_args.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_args.Next());
            	    
            	    }
            	    stream_args.Reset();
            	    adaptor.AddChild(root_0, stream_54.Next());
            	
            	}
            	

            
            }
    
            retval.stop = input.LT(-1);
            
            	retval.tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, retval.start, retval.stop);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end function


	private void InitializeCyclicDFAs()
	{
	}

 

    public static readonly BitSet FOLLOW_importRule_in_stylesheet169 = new BitSet(new ulong[]{0x00007C2583000000UL});
    public static readonly BitSet FOLLOW_media_in_stylesheet173 = new BitSet(new ulong[]{0x00007C2403000002UL});
    public static readonly BitSet FOLLOW_pageRule_in_stylesheet177 = new BitSet(new ulong[]{0x00007C2403000002UL});
    public static readonly BitSet FOLLOW_ruleset_in_stylesheet181 = new BitSet(new ulong[]{0x00007C2403000002UL});
    public static readonly BitSet FOLLOW_31_in_importRule195 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_32_in_importRule199 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_STRING_in_importRule203 = new BitSet(new ulong[]{0x0000000200000000UL});
    public static readonly BitSet FOLLOW_33_in_importRule205 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_31_in_importRule221 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_32_in_importRule225 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_function_in_importRule229 = new BitSet(new ulong[]{0x0000000200000000UL});
    public static readonly BitSet FOLLOW_33_in_importRule231 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_34_in_media252 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_media254 = new BitSet(new ulong[]{0x0000000800000000UL});
    public static readonly BitSet FOLLOW_35_in_media256 = new BitSet(new ulong[]{0x00007C2003000000UL});
    public static readonly BitSet FOLLOW_pageRule_in_media259 = new BitSet(new ulong[]{0x00007C3003000000UL});
    public static readonly BitSet FOLLOW_ruleset_in_media263 = new BitSet(new ulong[]{0x00007C3003000000UL});
    public static readonly BitSet FOLLOW_36_in_media267 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_37_in_pageRule295 = new BitSet(new ulong[]{0x0000600801000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_pageRule297 = new BitSet(new ulong[]{0x0000600801000000UL});
    public static readonly BitSet FOLLOW_pseudo_in_pageRule300 = new BitSet(new ulong[]{0x0000600800000000UL});
    public static readonly BitSet FOLLOW_35_in_pageRule303 = new BitSet(new ulong[]{0x0000005001000000UL});
    public static readonly BitSet FOLLOW_properties_in_pageRule305 = new BitSet(new ulong[]{0x0000005000000000UL});
    public static readonly BitSet FOLLOW_region_in_pageRule308 = new BitSet(new ulong[]{0x0000005000000000UL});
    public static readonly BitSet FOLLOW_36_in_pageRule311 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_38_in_region342 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_region344 = new BitSet(new ulong[]{0x0000000800000000UL});
    public static readonly BitSet FOLLOW_35_in_region346 = new BitSet(new ulong[]{0x0000001001000000UL});
    public static readonly BitSet FOLLOW_properties_in_region348 = new BitSet(new ulong[]{0x0000001000000000UL});
    public static readonly BitSet FOLLOW_36_in_region351 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selectors_in_ruleset376 = new BitSet(new ulong[]{0x0000000800000000UL});
    public static readonly BitSet FOLLOW_35_in_ruleset378 = new BitSet(new ulong[]{0x0000001001000000UL});
    public static readonly BitSet FOLLOW_properties_in_ruleset380 = new BitSet(new ulong[]{0x0000001000000000UL});
    public static readonly BitSet FOLLOW_36_in_ruleset383 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selector_in_selectors408 = new BitSet(new ulong[]{0x0000008000000002UL});
    public static readonly BitSet FOLLOW_39_in_selectors411 = new BitSet(new ulong[]{0x00007C0003000000UL});
    public static readonly BitSet FOLLOW_selector_in_selectors413 = new BitSet(new ulong[]{0x0000008000000002UL});
    public static readonly BitSet FOLLOW_elem_in_selector427 = new BitSet(new ulong[]{0x00007F0003000002UL});
    public static readonly BitSet FOLLOW_selectorOperation_in_selector429 = new BitSet(new ulong[]{0x00007F0003000002UL});
    public static readonly BitSet FOLLOW_pseudo_in_selector432 = new BitSet(new ulong[]{0x0000600000000002UL});
    public static readonly BitSet FOLLOW_pseudo_in_selector449 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selectop_in_selectorOperation467 = new BitSet(new ulong[]{0x00001C0003000000UL});
    public static readonly BitSet FOLLOW_elem_in_selectorOperation470 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_40_in_selectop488 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_41_in_selectop504 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_declaration_in_properties520 = new BitSet(new ulong[]{0x0000000200000002UL});
    public static readonly BitSet FOLLOW_33_in_properties523 = new BitSet(new ulong[]{0x0000000201000002UL});
    public static readonly BitSet FOLLOW_declaration_in_properties525 = new BitSet(new ulong[]{0x0000000200000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_elem551 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_UNIT_in_elem555 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem558 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_42_in_elem581 = new BitSet(new ulong[]{0x0000000003000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_elem584 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_UNIT_in_elem588 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem591 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_43_in_elem614 = new BitSet(new ulong[]{0x0000000003000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_elem617 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_UNIT_in_elem621 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem624 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_44_in_elem647 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem649 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_45_in_pseudo673 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_46_in_pseudo675 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_pseudo678 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_45_in_pseudo694 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_46_in_pseudo696 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_function_in_pseudo699 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_47_in_attrib720 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_attrib722 = new BitSet(new ulong[]{0x000F000000000000UL});
    public static readonly BitSet FOLLOW_attribRelate_in_attrib725 = new BitSet(new ulong[]{0x0000000001800000UL});
    public static readonly BitSet FOLLOW_STRING_in_attrib728 = new BitSet(new ulong[]{0x0001000000000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_attrib732 = new BitSet(new ulong[]{0x0001000000000000UL});
    public static readonly BitSet FOLLOW_48_in_attrib737 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_49_in_attribRelate770 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_50_in_attribRelate780 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_51_in_attribRelate789 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_declaration807 = new BitSet(new ulong[]{0x0000200000000000UL});
    public static readonly BitSet FOLLOW_45_in_declaration809 = new BitSet(new ulong[]{0x000000000D800000UL});
    public static readonly BitSet FOLLOW_args_in_declaration811 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expr_in_args834 = new BitSet(new ulong[]{0x000000800D800002UL});
    public static readonly BitSet FOLLOW_39_in_args837 = new BitSet(new ulong[]{0x000000000D800000UL});
    public static readonly BitSet FOLLOW_expr_in_args840 = new BitSet(new ulong[]{0x000000800D800002UL});
    public static readonly BitSet FOLLOW_NUM_in_expr859 = new BitSet(new ulong[]{0x0010000002000002UL});
    public static readonly BitSet FOLLOW_unit_in_expr861 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_expr868 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_COLOR_in_expr873 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STRING_in_expr878 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_expr883 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_unit894 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_function911 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_function913 = new BitSet(new ulong[]{0x004000000D800000UL});
    public static readonly BitSet FOLLOW_args_in_function915 = new BitSet(new ulong[]{0x0040000000000000UL});
    public static readonly BitSet FOLLOW_54_in_function918 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}