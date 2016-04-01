// $ANTLR 3.0.1 C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3 2016-03-21 14:26:56
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
		"SIBLING", 
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
		"'~'", 
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
    public const int PSEUDO = 16;
    public const int CLASS = 22;
    public const int ANY = 19;
    public const int COMMENT = 30;
    public const int IMPORT = 4;
    public const int HASVALUE = 14;
    public const int COLOR = 28;
    public const int MEDIA = 5;
    public const int RULE = 8;
    public const int ID = 21;
    public const int WS = 29;
    public const int EOF = -1;
    public const int SIBLING = 12;
    public const int UNIT = 26;
    public const int PROPERTY = 17;
    public const int NUM = 27;
    public const int EM = 23;
    public const int PAGE = 6;
    public const int FUNCTION = 18;
    public const int REGION = 7;
    public const int PARENTOF = 10;
    public const int ATTRIBEQUAL = 13;
    public const int LINE_COMMENT = 31;
    public const int IDENT = 25;
    public const int ATTRIB = 9;
    public const int STRING = 24;
    public const int TAG = 20;
    public const int BEGINSWITH = 15;
    
    
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
		get { return "C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3"; }
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:47:1: public stylesheet : ( importRule | media | pageRule | ruleset )+ ;
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
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:49:2: ( ( importRule | media | pageRule | ruleset )+ )
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:49:4: ( importRule | media | pageRule | ruleset )+
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();
            
            	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:49:4: ( importRule | media | pageRule | ruleset )+
            	int cnt1 = 0;
            	do 
            	{
            	    int alt1 = 5;
            	    switch ( input.LA(1) ) 
            	    {
            	    case 32:
            	    case 33:
            	    	{
            	        alt1 = 1;
            	        }
            	        break;
            	    case 35:
            	    	{
            	        alt1 = 2;
            	        }
            	        break;
            	    case 38:
            	    	{
            	        alt1 = 3;
            	        }
            	        break;
            	    case IDENT:
            	    case UNIT:
            	    case 44:
            	    case 45:
            	    case 46:
            	    case 47:
            	    case 48:
            	    	{
            	        alt1 = 4;
            	        }
            	        break;
            	    
            	    }
            	
            	    switch (alt1) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:49:5: importRule
            			    {
            			    	PushFollow(FOLLOW_importRule_in_stylesheet174);
            			    	importRule1 = importRule();
            			    	followingStackPointer_--;
            			    	
            			    	adaptor.AddChild(root_0, importRule1.Tree);
            			    
            			    }
            			    break;
            			case 2 :
            			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:49:18: media
            			    {
            			    	PushFollow(FOLLOW_media_in_stylesheet178);
            			    	media2 = media();
            			    	followingStackPointer_--;
            			    	
            			    	adaptor.AddChild(root_0, media2.Tree);
            			    
            			    }
            			    break;
            			case 3 :
            			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:49:26: pageRule
            			    {
            			    	PushFollow(FOLLOW_pageRule_in_stylesheet182);
            			    	pageRule3 = pageRule();
            			    	followingStackPointer_--;
            			    	
            			    	adaptor.AddChild(root_0, pageRule3.Tree);
            			    
            			    }
            			    break;
            			case 4 :
            			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:49:37: ruleset
            			    {
            			    	PushFollow(FOLLOW_ruleset_in_stylesheet186);
            			    	ruleset4 = ruleset();
            			    	followingStackPointer_--;
            			    	
            			    	adaptor.AddChild(root_0, ruleset4.Tree);
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt1 >= 1 ) goto loop1;
            		            EarlyExitException eee =
            		                new EarlyExitException(1, input);
            		            throw eee;
            	    }
            	    cnt1++;
            	} while (true);
            	
            	loop1:
            		;	// Stops C# compiler whinging that label 'loop1' has no statements

            
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:52:1: importRule : ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) );
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
        RewriteRuleTokenStream stream_34 = new RewriteRuleTokenStream(adaptor,"token 34");
        RewriteRuleTokenStream stream_STRING = new RewriteRuleTokenStream(adaptor,"token STRING");
        RewriteRuleTokenStream stream_32 = new RewriteRuleTokenStream(adaptor,"token 32");
        RewriteRuleSubtreeStream stream_function = new RewriteRuleSubtreeStream(adaptor,"rule function");
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:53:2: ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) )
            int alt4 = 2;
            int LA4_0 = input.LA(1);
            
            if ( (LA4_0 == 32) )
            {
                int LA4_1 = input.LA(2);
                
                if ( (LA4_1 == IDENT) )
                {
                    alt4 = 2;
                }
                else if ( (LA4_1 == STRING) )
                {
                    alt4 = 1;
                }
                else 
                {
                    NoViableAltException nvae_d4s1 =
                        new NoViableAltException("52:1: importRule : ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) );", 4, 1, input);
                
                    throw nvae_d4s1;
                }
            }
            else if ( (LA4_0 == 33) )
            {
                int LA4_2 = input.LA(2);
                
                if ( (LA4_2 == IDENT) )
                {
                    alt4 = 2;
                }
                else if ( (LA4_2 == STRING) )
                {
                    alt4 = 1;
                }
                else 
                {
                    NoViableAltException nvae_d4s2 =
                        new NoViableAltException("52:1: importRule : ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) );", 4, 2, input);
                
                    throw nvae_d4s2;
                }
            }
            else 
            {
                NoViableAltException nvae_d4s0 =
                    new NoViableAltException("52:1: importRule : ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) );", 4, 0, input);
            
                throw nvae_d4s0;
            }
            switch (alt4) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:53:4: ( '@import' | '@include' ) STRING ';'
                    {
                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:53:4: ( '@import' | '@include' )
                    	int alt2 = 2;
                    	int LA2_0 = input.LA(1);
                    	
                    	if ( (LA2_0 == 32) )
                    	{
                    	    alt2 = 1;
                    	}
                    	else if ( (LA2_0 == 33) )
                    	{
                    	    alt2 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d2s0 =
                    	        new NoViableAltException("53:4: ( '@import' | '@include' )", 2, 0, input);
                    	
                    	    throw nvae_d2s0;
                    	}
                    	switch (alt2) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:53:5: '@import'
                    	        {
                    	        	string_literal5 = (IToken)input.LT(1);
                    	        	Match(input,32,FOLLOW_32_in_importRule200); 
                    	        	stream_32.Add(string_literal5);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:53:17: '@include'
                    	        {
                    	        	string_literal6 = (IToken)input.LT(1);
                    	        	Match(input,33,FOLLOW_33_in_importRule204); 
                    	        	stream_33.Add(string_literal6);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	STRING7 = (IToken)input.LT(1);
                    	Match(input,STRING,FOLLOW_STRING_in_importRule208); 
                    	stream_STRING.Add(STRING7);

                    	char_literal8 = (IToken)input.LT(1);
                    	Match(input,34,FOLLOW_34_in_importRule210); 
                    	stream_34.Add(char_literal8);

                    	
                    	// AST REWRITE
                    	// elements:          STRING
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 53:41: -> ^( IMPORT STRING )
                    	{
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:53:44: ^( IMPORT STRING )
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
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:54:4: ( '@import' | '@include' ) function ';'
                    {
                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:54:4: ( '@import' | '@include' )
                    	int alt3 = 2;
                    	int LA3_0 = input.LA(1);
                    	
                    	if ( (LA3_0 == 32) )
                    	{
                    	    alt3 = 1;
                    	}
                    	else if ( (LA3_0 == 33) )
                    	{
                    	    alt3 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d3s0 =
                    	        new NoViableAltException("54:4: ( '@import' | '@include' )", 3, 0, input);
                    	
                    	    throw nvae_d3s0;
                    	}
                    	switch (alt3) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:54:5: '@import'
                    	        {
                    	        	string_literal9 = (IToken)input.LT(1);
                    	        	Match(input,32,FOLLOW_32_in_importRule226); 
                    	        	stream_32.Add(string_literal9);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:54:17: '@include'
                    	        {
                    	        	string_literal10 = (IToken)input.LT(1);
                    	        	Match(input,33,FOLLOW_33_in_importRule230); 
                    	        	stream_33.Add(string_literal10);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	PushFollow(FOLLOW_function_in_importRule234);
                    	function11 = function();
                    	followingStackPointer_--;
                    	
                    	stream_function.Add(function11.Tree);
                    	char_literal12 = (IToken)input.LT(1);
                    	Match(input,34,FOLLOW_34_in_importRule236); 
                    	stream_34.Add(char_literal12);

                    	
                    	// AST REWRITE
                    	// elements:          function
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 54:43: -> ^( IMPORT function )
                    	{
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:54:46: ^( IMPORT function )
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:57:1: media : '@media' IDENT '{' ( pageRule | ruleset )+ '}' -> ^( MEDIA IDENT ( pageRule )* ( ruleset )* ) ;
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
        RewriteRuleTokenStream stream_35 = new RewriteRuleTokenStream(adaptor,"token 35");
        RewriteRuleTokenStream stream_36 = new RewriteRuleTokenStream(adaptor,"token 36");
        RewriteRuleTokenStream stream_37 = new RewriteRuleTokenStream(adaptor,"token 37");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleSubtreeStream stream_pageRule = new RewriteRuleSubtreeStream(adaptor,"rule pageRule");
        RewriteRuleSubtreeStream stream_ruleset = new RewriteRuleSubtreeStream(adaptor,"rule ruleset");
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:58:2: ( '@media' IDENT '{' ( pageRule | ruleset )+ '}' -> ^( MEDIA IDENT ( pageRule )* ( ruleset )* ) )
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:58:4: '@media' IDENT '{' ( pageRule | ruleset )+ '}'
            {
            	string_literal13 = (IToken)input.LT(1);
            	Match(input,35,FOLLOW_35_in_media257); 
            	stream_35.Add(string_literal13);

            	IDENT14 = (IToken)input.LT(1);
            	Match(input,IDENT,FOLLOW_IDENT_in_media259); 
            	stream_IDENT.Add(IDENT14);

            	char_literal15 = (IToken)input.LT(1);
            	Match(input,36,FOLLOW_36_in_media261); 
            	stream_36.Add(char_literal15);

            	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:58:23: ( pageRule | ruleset )+
            	int cnt5 = 0;
            	do 
            	{
            	    int alt5 = 3;
            	    int LA5_0 = input.LA(1);
            	    
            	    if ( (LA5_0 == 38) )
            	    {
            	        alt5 = 1;
            	    }
            	    else if ( ((LA5_0 >= IDENT && LA5_0 <= UNIT) || (LA5_0 >= 44 && LA5_0 <= 48)) )
            	    {
            	        alt5 = 2;
            	    }
            	    
            	
            	    switch (alt5) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:58:24: pageRule
            			    {
            			    	PushFollow(FOLLOW_pageRule_in_media264);
            			    	pageRule16 = pageRule();
            			    	followingStackPointer_--;
            			    	
            			    	stream_pageRule.Add(pageRule16.Tree);
            			    
            			    }
            			    break;
            			case 2 :
            			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:58:35: ruleset
            			    {
            			    	PushFollow(FOLLOW_ruleset_in_media268);
            			    	ruleset17 = ruleset();
            			    	followingStackPointer_--;
            			    	
            			    	stream_ruleset.Add(ruleset17.Tree);
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt5 >= 1 ) goto loop5;
            		            EarlyExitException eee =
            		                new EarlyExitException(5, input);
            		            throw eee;
            	    }
            	    cnt5++;
            	} while (true);
            	
            	loop5:
            		;	// Stops C# compiler whinging that label 'loop5' has no statements

            	char_literal18 = (IToken)input.LT(1);
            	Match(input,37,FOLLOW_37_in_media272); 
            	stream_37.Add(char_literal18);

            	
            	// AST REWRITE
            	// elements:          IDENT, pageRule, ruleset
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 58:49: -> ^( MEDIA IDENT ( pageRule )* ( ruleset )* )
            	{
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:58:52: ^( MEDIA IDENT ( pageRule )* ( ruleset )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(MEDIA, "MEDIA"), root_1);
            	    
            	    adaptor.AddChild(root_1, stream_IDENT.Next());
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:58:67: ( pageRule )*
            	    while ( stream_pageRule.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_pageRule.Next());
            	    
            	    }
            	    stream_pageRule.Reset();
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:58:77: ( ruleset )*
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:61:1: pageRule : '@page' ( IDENT )* ( pseudo )* '{' ( properties )? ( region )* '}' -> ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* ) ;
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
        RewriteRuleTokenStream stream_36 = new RewriteRuleTokenStream(adaptor,"token 36");
        RewriteRuleTokenStream stream_37 = new RewriteRuleTokenStream(adaptor,"token 37");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_38 = new RewriteRuleTokenStream(adaptor,"token 38");
        RewriteRuleSubtreeStream stream_region = new RewriteRuleSubtreeStream(adaptor,"rule region");
        RewriteRuleSubtreeStream stream_pseudo = new RewriteRuleSubtreeStream(adaptor,"rule pseudo");
        RewriteRuleSubtreeStream stream_properties = new RewriteRuleSubtreeStream(adaptor,"rule properties");
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:3: ( '@page' ( IDENT )* ( pseudo )* '{' ( properties )? ( region )* '}' -> ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* ) )
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:5: '@page' ( IDENT )* ( pseudo )* '{' ( properties )? ( region )* '}'
            {
            	string_literal19 = (IToken)input.LT(1);
            	Match(input,38,FOLLOW_38_in_pageRule300); 
            	stream_38.Add(string_literal19);

            	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:13: ( IDENT )*
            	do 
            	{
            	    int alt6 = 2;
            	    int LA6_0 = input.LA(1);
            	    
            	    if ( (LA6_0 == IDENT) )
            	    {
            	        alt6 = 1;
            	    }
            	    
            	
            	    switch (alt6) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:13: IDENT
            			    {
            			    	IDENT20 = (IToken)input.LT(1);
            			    	Match(input,IDENT,FOLLOW_IDENT_in_pageRule302); 
            			    	stream_IDENT.Add(IDENT20);

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop6;
            	    }
            	} while (true);
            	
            	loop6:
            		;	// Stops C# compiler whinging that label 'loop6' has no statements

            	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:20: ( pseudo )*
            	do 
            	{
            	    int alt7 = 2;
            	    int LA7_0 = input.LA(1);
            	    
            	    if ( ((LA7_0 >= 47 && LA7_0 <= 48)) )
            	    {
            	        alt7 = 1;
            	    }
            	    
            	
            	    switch (alt7) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:20: pseudo
            			    {
            			    	PushFollow(FOLLOW_pseudo_in_pageRule305);
            			    	pseudo21 = pseudo();
            			    	followingStackPointer_--;
            			    	
            			    	stream_pseudo.Add(pseudo21.Tree);
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop7;
            	    }
            	} while (true);
            	
            	loop7:
            		;	// Stops C# compiler whinging that label 'loop7' has no statements

            	char_literal22 = (IToken)input.LT(1);
            	Match(input,36,FOLLOW_36_in_pageRule308); 
            	stream_36.Add(char_literal22);

            	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:32: ( properties )?
            	int alt8 = 2;
            	int LA8_0 = input.LA(1);
            	
            	if ( (LA8_0 == IDENT) )
            	{
            	    alt8 = 1;
            	}
            	switch (alt8) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:32: properties
            	        {
            	        	PushFollow(FOLLOW_properties_in_pageRule310);
            	        	properties23 = properties();
            	        	followingStackPointer_--;
            	        	
            	        	stream_properties.Add(properties23.Tree);
            	        
            	        }
            	        break;
            	
            	}

            	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:44: ( region )*
            	do 
            	{
            	    int alt9 = 2;
            	    int LA9_0 = input.LA(1);
            	    
            	    if ( (LA9_0 == 39) )
            	    {
            	        alt9 = 1;
            	    }
            	    
            	
            	    switch (alt9) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:44: region
            			    {
            			    	PushFollow(FOLLOW_region_in_pageRule313);
            			    	region24 = region();
            			    	followingStackPointer_--;
            			    	
            			    	stream_region.Add(region24.Tree);
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop9;
            	    }
            	} while (true);
            	
            	loop9:
            		;	// Stops C# compiler whinging that label 'loop9' has no statements

            	char_literal25 = (IToken)input.LT(1);
            	Match(input,37,FOLLOW_37_in_pageRule316); 
            	stream_37.Add(char_literal25);

            	
            	// AST REWRITE
            	// elements:          IDENT, properties, region, pseudo
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 62:56: -> ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* )
            	{
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:59: ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(PAGE, "PAGE"), root_1);
            	    
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:67: ( IDENT )*
            	    while ( stream_IDENT.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_IDENT.Next());
            	    
            	    }
            	    stream_IDENT.Reset();
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:74: ( pseudo )*
            	    while ( stream_pseudo.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_pseudo.Next());
            	    
            	    }
            	    stream_pseudo.Reset();
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:82: ( properties )*
            	    while ( stream_properties.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_properties.Next());
            	    
            	    }
            	    stream_properties.Reset();
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:62:94: ( region )*
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:65:1: region : '@' IDENT '{' ( properties )? '}' -> ^( REGION IDENT ( properties )* ) ;
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
        RewriteRuleTokenStream stream_36 = new RewriteRuleTokenStream(adaptor,"token 36");
        RewriteRuleTokenStream stream_37 = new RewriteRuleTokenStream(adaptor,"token 37");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_39 = new RewriteRuleTokenStream(adaptor,"token 39");
        RewriteRuleSubtreeStream stream_properties = new RewriteRuleSubtreeStream(adaptor,"rule properties");
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:66:2: ( '@' IDENT '{' ( properties )? '}' -> ^( REGION IDENT ( properties )* ) )
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:66:4: '@' IDENT '{' ( properties )? '}'
            {
            	char_literal26 = (IToken)input.LT(1);
            	Match(input,39,FOLLOW_39_in_region347); 
            	stream_39.Add(char_literal26);

            	IDENT27 = (IToken)input.LT(1);
            	Match(input,IDENT,FOLLOW_IDENT_in_region349); 
            	stream_IDENT.Add(IDENT27);

            	char_literal28 = (IToken)input.LT(1);
            	Match(input,36,FOLLOW_36_in_region351); 
            	stream_36.Add(char_literal28);

            	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:66:18: ( properties )?
            	int alt10 = 2;
            	int LA10_0 = input.LA(1);
            	
            	if ( (LA10_0 == IDENT) )
            	{
            	    alt10 = 1;
            	}
            	switch (alt10) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:66:18: properties
            	        {
            	        	PushFollow(FOLLOW_properties_in_region353);
            	        	properties29 = properties();
            	        	followingStackPointer_--;
            	        	
            	        	stream_properties.Add(properties29.Tree);
            	        
            	        }
            	        break;
            	
            	}

            	char_literal30 = (IToken)input.LT(1);
            	Match(input,37,FOLLOW_37_in_region356); 
            	stream_37.Add(char_literal30);

            	
            	// AST REWRITE
            	// elements:          IDENT, properties
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 66:34: -> ^( REGION IDENT ( properties )* )
            	{
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:66:37: ^( REGION IDENT ( properties )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(REGION, "REGION"), root_1);
            	    
            	    adaptor.AddChild(root_1, stream_IDENT.Next());
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:66:53: ( properties )*
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:69:1: ruleset : selectors '{' ( properties )? '}' -> ^( RULE selectors ( properties )* ) ;
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
        RewriteRuleTokenStream stream_36 = new RewriteRuleTokenStream(adaptor,"token 36");
        RewriteRuleTokenStream stream_37 = new RewriteRuleTokenStream(adaptor,"token 37");
        RewriteRuleSubtreeStream stream_selectors = new RewriteRuleSubtreeStream(adaptor,"rule selectors");
        RewriteRuleSubtreeStream stream_properties = new RewriteRuleSubtreeStream(adaptor,"rule properties");
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:70:3: ( selectors '{' ( properties )? '}' -> ^( RULE selectors ( properties )* ) )
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:70:5: selectors '{' ( properties )? '}'
            {
            	PushFollow(FOLLOW_selectors_in_ruleset381);
            	selectors31 = selectors();
            	followingStackPointer_--;
            	
            	stream_selectors.Add(selectors31.Tree);
            	char_literal32 = (IToken)input.LT(1);
            	Match(input,36,FOLLOW_36_in_ruleset383); 
            	stream_36.Add(char_literal32);

            	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:70:19: ( properties )?
            	int alt11 = 2;
            	int LA11_0 = input.LA(1);
            	
            	if ( (LA11_0 == IDENT) )
            	{
            	    alt11 = 1;
            	}
            	switch (alt11) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:70:19: properties
            	        {
            	        	PushFollow(FOLLOW_properties_in_ruleset385);
            	        	properties33 = properties();
            	        	followingStackPointer_--;
            	        	
            	        	stream_properties.Add(properties33.Tree);
            	        
            	        }
            	        break;
            	
            	}

            	char_literal34 = (IToken)input.LT(1);
            	Match(input,37,FOLLOW_37_in_ruleset388); 
            	stream_37.Add(char_literal34);

            	
            	// AST REWRITE
            	// elements:          selectors, properties
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 70:35: -> ^( RULE selectors ( properties )* )
            	{
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:70:38: ^( RULE selectors ( properties )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(RULE, "RULE"), root_1);
            	    
            	    adaptor.AddChild(root_1, stream_selectors.Next());
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:70:56: ( properties )*
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:73:1: selectors : selector ( ',' selector )* ;
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
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:74:2: ( selector ( ',' selector )* )
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:74:4: selector ( ',' selector )*
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();
            
            	PushFollow(FOLLOW_selector_in_selectors413);
            	selector35 = selector();
            	followingStackPointer_--;
            	
            	adaptor.AddChild(root_0, selector35.Tree);
            	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:74:13: ( ',' selector )*
            	do 
            	{
            	    int alt12 = 2;
            	    int LA12_0 = input.LA(1);
            	    
            	    if ( (LA12_0 == 40) )
            	    {
            	        alt12 = 1;
            	    }
            	    
            	
            	    switch (alt12) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:74:14: ',' selector
            			    {
            			    	char_literal36 = (IToken)input.LT(1);
            			    	Match(input,40,FOLLOW_40_in_selectors416); 
            			    	char_literal36_tree = (CommonTree)adaptor.Create(char_literal36);
            			    	adaptor.AddChild(root_0, char_literal36_tree);

            			    	PushFollow(FOLLOW_selector_in_selectors418);
            			    	selector37 = selector();
            			    	followingStackPointer_--;
            			    	
            			    	adaptor.AddChild(root_0, selector37.Tree);
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop12;
            	    }
            	} while (true);
            	
            	loop12:
            		;	// Stops C# compiler whinging that label 'loop12' has no statements

            
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:77:1: selector : ( elem ( selectorOperation )* ( pseudo )* -> elem ( selectorOperation )* ( pseudo )* | pseudo -> ANY pseudo );
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
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:78:2: ( elem ( selectorOperation )* ( pseudo )* -> elem ( selectorOperation )* ( pseudo )* | pseudo -> ANY pseudo )
            int alt15 = 2;
            int LA15_0 = input.LA(1);
            
            if ( ((LA15_0 >= IDENT && LA15_0 <= UNIT) || (LA15_0 >= 44 && LA15_0 <= 46)) )
            {
                alt15 = 1;
            }
            else if ( ((LA15_0 >= 47 && LA15_0 <= 48)) )
            {
                alt15 = 2;
            }
            else 
            {
                NoViableAltException nvae_d15s0 =
                    new NoViableAltException("77:1: selector : ( elem ( selectorOperation )* ( pseudo )* -> elem ( selectorOperation )* ( pseudo )* | pseudo -> ANY pseudo );", 15, 0, input);
            
                throw nvae_d15s0;
            }
            switch (alt15) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:78:4: elem ( selectorOperation )* ( pseudo )*
                    {
                    	PushFollow(FOLLOW_elem_in_selector432);
                    	elem38 = elem();
                    	followingStackPointer_--;
                    	
                    	stream_elem.Add(elem38.Tree);
                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:78:9: ( selectorOperation )*
                    	do 
                    	{
                    	    int alt13 = 2;
                    	    int LA13_0 = input.LA(1);
                    	    
                    	    if ( ((LA13_0 >= IDENT && LA13_0 <= UNIT) || (LA13_0 >= 41 && LA13_0 <= 46)) )
                    	    {
                    	        alt13 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt13) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:78:9: selectorOperation
                    			    {
                    			    	PushFollow(FOLLOW_selectorOperation_in_selector434);
                    			    	selectorOperation39 = selectorOperation();
                    			    	followingStackPointer_--;
                    			    	
                    			    	stream_selectorOperation.Add(selectorOperation39.Tree);
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop13;
                    	    }
                    	} while (true);
                    	
                    	loop13:
                    		;	// Stops C# compiler whinging that label 'loop13' has no statements

                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:78:28: ( pseudo )*
                    	do 
                    	{
                    	    int alt14 = 2;
                    	    int LA14_0 = input.LA(1);
                    	    
                    	    if ( ((LA14_0 >= 47 && LA14_0 <= 48)) )
                    	    {
                    	        alt14 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt14) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:78:28: pseudo
                    			    {
                    			    	PushFollow(FOLLOW_pseudo_in_selector437);
                    			    	pseudo40 = pseudo();
                    			    	followingStackPointer_--;
                    			    	
                    			    	stream_pseudo.Add(pseudo40.Tree);
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop14;
                    	    }
                    	} while (true);
                    	
                    	loop14:
                    		;	// Stops C# compiler whinging that label 'loop14' has no statements

                    	
                    	// AST REWRITE
                    	// elements:          pseudo, elem, selectorOperation
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 78:36: -> elem ( selectorOperation )* ( pseudo )*
                    	{
                    	    adaptor.AddChild(root_0, stream_elem.Next());
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:78:45: ( selectorOperation )*
                    	    while ( stream_selectorOperation.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_0, stream_selectorOperation.Next());
                    	    
                    	    }
                    	    stream_selectorOperation.Reset();
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:78:64: ( pseudo )*
                    	    while ( stream_pseudo.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_0, stream_pseudo.Next());
                    	    
                    	    }
                    	    stream_pseudo.Reset();
                    	
                    	}
                    	

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:79:4: pseudo
                    {
                    	PushFollow(FOLLOW_pseudo_in_selector454);
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
                    	// 79:11: -> ANY pseudo
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:82:1: selectorOperation : ( selectop )? elem -> ( selectop )* elem ;
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
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:83:2: ( ( selectop )? elem -> ( selectop )* elem )
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:83:4: ( selectop )? elem
            {
            	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:83:4: ( selectop )?
            	int alt16 = 2;
            	int LA16_0 = input.LA(1);
            	
            	if ( ((LA16_0 >= 41 && LA16_0 <= 43)) )
            	{
            	    alt16 = 1;
            	}
            	switch (alt16) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:83:4: selectop
            	        {
            	        	PushFollow(FOLLOW_selectop_in_selectorOperation472);
            	        	selectop42 = selectop();
            	        	followingStackPointer_--;
            	        	
            	        	stream_selectop.Add(selectop42.Tree);
            	        
            	        }
            	        break;
            	
            	}

            	PushFollow(FOLLOW_elem_in_selectorOperation475);
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
            	// 83:19: -> ( selectop )* elem
            	{
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:83:22: ( selectop )*
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:86:1: selectop : ( '>' -> PARENTOF | '+' -> PRECEDES | '~' -> SIBLING );
    public selectop_return selectop() // throws RecognitionException [1]
    {   
        selectop_return retval = new selectop_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal44 = null;
        IToken char_literal45 = null;
        IToken char_literal46 = null;
        
        CommonTree char_literal44_tree=null;
        CommonTree char_literal45_tree=null;
        CommonTree char_literal46_tree=null;
        RewriteRuleTokenStream stream_41 = new RewriteRuleTokenStream(adaptor,"token 41");
        RewriteRuleTokenStream stream_42 = new RewriteRuleTokenStream(adaptor,"token 42");
        RewriteRuleTokenStream stream_43 = new RewriteRuleTokenStream(adaptor,"token 43");
    
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:87:2: ( '>' -> PARENTOF | '+' -> PRECEDES | '~' -> SIBLING )
            int alt17 = 3;
            switch ( input.LA(1) ) 
            {
            case 41:
            	{
                alt17 = 1;
                }
                break;
            case 42:
            	{
                alt17 = 2;
                }
                break;
            case 43:
            	{
                alt17 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d17s0 =
            	        new NoViableAltException("86:1: selectop : ( '>' -> PARENTOF | '+' -> PRECEDES | '~' -> SIBLING );", 17, 0, input);
            
            	    throw nvae_d17s0;
            }
            
            switch (alt17) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:87:4: '>'
                    {
                    	char_literal44 = (IToken)input.LT(1);
                    	Match(input,41,FOLLOW_41_in_selectop493); 
                    	stream_41.Add(char_literal44);

                    	
                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 87:8: -> PARENTOF
                    	{
                    	    adaptor.AddChild(root_0, adaptor.Create(PARENTOF, "PARENTOF"));
                    	
                    	}
                    	

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:88:11: '+'
                    {
                    	char_literal45 = (IToken)input.LT(1);
                    	Match(input,42,FOLLOW_42_in_selectop509); 
                    	stream_42.Add(char_literal45);

                    	
                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 88:16: -> PRECEDES
                    	{
                    	    adaptor.AddChild(root_0, adaptor.Create(PRECEDES, "PRECEDES"));
                    	
                    	}
                    	

                    
                    }
                    break;
                case 3 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:89:11: '~'
                    {
                    	char_literal46 = (IToken)input.LT(1);
                    	Match(input,43,FOLLOW_43_in_selectop526); 
                    	stream_43.Add(char_literal46);

                    	
                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 89:15: -> SIBLING
                    	{
                    	    adaptor.AddChild(root_0, adaptor.Create(SIBLING, "SIBLING"));
                    	
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:92:1: properties : declaration ( ';' ( declaration )? )* -> ( declaration )+ ;
    public properties_return properties() // throws RecognitionException [1]
    {   
        properties_return retval = new properties_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal48 = null;
        declaration_return declaration47 = null;

        declaration_return declaration49 = null;
        
        
        CommonTree char_literal48_tree=null;
        RewriteRuleTokenStream stream_34 = new RewriteRuleTokenStream(adaptor,"token 34");
        RewriteRuleSubtreeStream stream_declaration = new RewriteRuleSubtreeStream(adaptor,"rule declaration");
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:93:2: ( declaration ( ';' ( declaration )? )* -> ( declaration )+ )
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:93:4: declaration ( ';' ( declaration )? )*
            {
            	PushFollow(FOLLOW_declaration_in_properties541);
            	declaration47 = declaration();
            	followingStackPointer_--;
            	
            	stream_declaration.Add(declaration47.Tree);
            	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:93:16: ( ';' ( declaration )? )*
            	do 
            	{
            	    int alt19 = 2;
            	    int LA19_0 = input.LA(1);
            	    
            	    if ( (LA19_0 == 34) )
            	    {
            	        alt19 = 1;
            	    }
            	    
            	
            	    switch (alt19) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:93:17: ';' ( declaration )?
            			    {
            			    	char_literal48 = (IToken)input.LT(1);
            			    	Match(input,34,FOLLOW_34_in_properties544); 
            			    	stream_34.Add(char_literal48);

            			    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:93:21: ( declaration )?
            			    	int alt18 = 2;
            			    	int LA18_0 = input.LA(1);
            			    	
            			    	if ( (LA18_0 == IDENT) )
            			    	{
            			    	    alt18 = 1;
            			    	}
            			    	switch (alt18) 
            			    	{
            			    	    case 1 :
            			    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:93:21: declaration
            			    	        {
            			    	        	PushFollow(FOLLOW_declaration_in_properties546);
            			    	        	declaration49 = declaration();
            			    	        	followingStackPointer_--;
            			    	        	
            			    	        	stream_declaration.Add(declaration49.Tree);
            			    	        
            			    	        }
            			    	        break;
            			    	
            			    	}

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop19;
            	    }
            	} while (true);
            	
            	loop19:
            		;	// Stops C# compiler whinging that label 'loop19' has no statements

            	
            	// AST REWRITE
            	// elements:          declaration
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 93:36: -> ( declaration )+
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:96:1: elem : ( ( IDENT | UNIT ) ( attrib )* -> ^( TAG ( IDENT )* ( UNIT )* ( attrib )* ) | '#' ( IDENT | UNIT ) ( attrib )* -> ^( ID ( IDENT )* ( UNIT )* ( attrib )* ) | '.' ( IDENT | UNIT ) ( attrib )* -> ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* ) | '*' ( attrib )* -> ^( ANY ( attrib )* ) );
    public elem_return elem() // throws RecognitionException [1]
    {   
        elem_return retval = new elem_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken IDENT50 = null;
        IToken UNIT51 = null;
        IToken char_literal53 = null;
        IToken IDENT54 = null;
        IToken UNIT55 = null;
        IToken char_literal57 = null;
        IToken IDENT58 = null;
        IToken UNIT59 = null;
        IToken char_literal61 = null;
        attrib_return attrib52 = null;

        attrib_return attrib56 = null;

        attrib_return attrib60 = null;

        attrib_return attrib62 = null;
        
        
        CommonTree IDENT50_tree=null;
        CommonTree UNIT51_tree=null;
        CommonTree char_literal53_tree=null;
        CommonTree IDENT54_tree=null;
        CommonTree UNIT55_tree=null;
        CommonTree char_literal57_tree=null;
        CommonTree IDENT58_tree=null;
        CommonTree UNIT59_tree=null;
        CommonTree char_literal61_tree=null;
        RewriteRuleTokenStream stream_44 = new RewriteRuleTokenStream(adaptor,"token 44");
        RewriteRuleTokenStream stream_45 = new RewriteRuleTokenStream(adaptor,"token 45");
        RewriteRuleTokenStream stream_46 = new RewriteRuleTokenStream(adaptor,"token 46");
        RewriteRuleTokenStream stream_UNIT = new RewriteRuleTokenStream(adaptor,"token UNIT");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleSubtreeStream stream_attrib = new RewriteRuleSubtreeStream(adaptor,"rule attrib");
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:97:2: ( ( IDENT | UNIT ) ( attrib )* -> ^( TAG ( IDENT )* ( UNIT )* ( attrib )* ) | '#' ( IDENT | UNIT ) ( attrib )* -> ^( ID ( IDENT )* ( UNIT )* ( attrib )* ) | '.' ( IDENT | UNIT ) ( attrib )* -> ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* ) | '*' ( attrib )* -> ^( ANY ( attrib )* ) )
            int alt27 = 4;
            switch ( input.LA(1) ) 
            {
            case IDENT:
            case UNIT:
            	{
                alt27 = 1;
                }
                break;
            case 44:
            	{
                alt27 = 2;
                }
                break;
            case 45:
            	{
                alt27 = 3;
                }
                break;
            case 46:
            	{
                alt27 = 4;
                }
                break;
            	default:
            	    NoViableAltException nvae_d27s0 =
            	        new NoViableAltException("96:1: elem : ( ( IDENT | UNIT ) ( attrib )* -> ^( TAG ( IDENT )* ( UNIT )* ( attrib )* ) | '#' ( IDENT | UNIT ) ( attrib )* -> ^( ID ( IDENT )* ( UNIT )* ( attrib )* ) | '.' ( IDENT | UNIT ) ( attrib )* -> ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* ) | '*' ( attrib )* -> ^( ANY ( attrib )* ) );", 27, 0, input);
            
            	    throw nvae_d27s0;
            }
            
            switch (alt27) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:97:8: ( IDENT | UNIT ) ( attrib )*
                    {
                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:97:8: ( IDENT | UNIT )
                    	int alt20 = 2;
                    	int LA20_0 = input.LA(1);
                    	
                    	if ( (LA20_0 == IDENT) )
                    	{
                    	    alt20 = 1;
                    	}
                    	else if ( (LA20_0 == UNIT) )
                    	{
                    	    alt20 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d20s0 =
                    	        new NoViableAltException("97:8: ( IDENT | UNIT )", 20, 0, input);
                    	
                    	    throw nvae_d20s0;
                    	}
                    	switch (alt20) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:97:9: IDENT
                    	        {
                    	        	IDENT50 = (IToken)input.LT(1);
                    	        	Match(input,IDENT,FOLLOW_IDENT_in_elem572); 
                    	        	stream_IDENT.Add(IDENT50);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:97:17: UNIT
                    	        {
                    	        	UNIT51 = (IToken)input.LT(1);
                    	        	Match(input,UNIT,FOLLOW_UNIT_in_elem576); 
                    	        	stream_UNIT.Add(UNIT51);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:97:23: ( attrib )*
                    	do 
                    	{
                    	    int alt21 = 2;
                    	    int LA21_0 = input.LA(1);
                    	    
                    	    if ( (LA21_0 == 49) )
                    	    {
                    	        alt21 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt21) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:97:23: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem579);
                    			    	attrib52 = attrib();
                    			    	followingStackPointer_--;
                    			    	
                    			    	stream_attrib.Add(attrib52.Tree);
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop21;
                    	    }
                    	} while (true);
                    	
                    	loop21:
                    		;	// Stops C# compiler whinging that label 'loop21' has no statements

                    	
                    	// AST REWRITE
                    	// elements:          IDENT, UNIT, attrib
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 97:31: -> ^( TAG ( IDENT )* ( UNIT )* ( attrib )* )
                    	{
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:97:34: ^( TAG ( IDENT )* ( UNIT )* ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(TAG, "TAG"), root_1);
                    	    
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:97:41: ( IDENT )*
                    	    while ( stream_IDENT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_IDENT.Next());
                    	    
                    	    }
                    	    stream_IDENT.Reset();
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:97:48: ( UNIT )*
                    	    while ( stream_UNIT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_UNIT.Next());
                    	    
                    	    }
                    	    stream_UNIT.Reset();
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:97:54: ( attrib )*
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
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:98:4: '#' ( IDENT | UNIT ) ( attrib )*
                    {
                    	char_literal53 = (IToken)input.LT(1);
                    	Match(input,44,FOLLOW_44_in_elem602); 
                    	stream_44.Add(char_literal53);

                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:98:8: ( IDENT | UNIT )
                    	int alt22 = 2;
                    	int LA22_0 = input.LA(1);
                    	
                    	if ( (LA22_0 == IDENT) )
                    	{
                    	    alt22 = 1;
                    	}
                    	else if ( (LA22_0 == UNIT) )
                    	{
                    	    alt22 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d22s0 =
                    	        new NoViableAltException("98:8: ( IDENT | UNIT )", 22, 0, input);
                    	
                    	    throw nvae_d22s0;
                    	}
                    	switch (alt22) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:98:9: IDENT
                    	        {
                    	        	IDENT54 = (IToken)input.LT(1);
                    	        	Match(input,IDENT,FOLLOW_IDENT_in_elem605); 
                    	        	stream_IDENT.Add(IDENT54);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:98:17: UNIT
                    	        {
                    	        	UNIT55 = (IToken)input.LT(1);
                    	        	Match(input,UNIT,FOLLOW_UNIT_in_elem609); 
                    	        	stream_UNIT.Add(UNIT55);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:98:23: ( attrib )*
                    	do 
                    	{
                    	    int alt23 = 2;
                    	    int LA23_0 = input.LA(1);
                    	    
                    	    if ( (LA23_0 == 49) )
                    	    {
                    	        alt23 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt23) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:98:23: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem612);
                    			    	attrib56 = attrib();
                    			    	followingStackPointer_--;
                    			    	
                    			    	stream_attrib.Add(attrib56.Tree);
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop23;
                    	    }
                    	} while (true);
                    	
                    	loop23:
                    		;	// Stops C# compiler whinging that label 'loop23' has no statements

                    	
                    	// AST REWRITE
                    	// elements:          UNIT, attrib, IDENT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 98:31: -> ^( ID ( IDENT )* ( UNIT )* ( attrib )* )
                    	{
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:98:34: ^( ID ( IDENT )* ( UNIT )* ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(ID, "ID"), root_1);
                    	    
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:98:40: ( IDENT )*
                    	    while ( stream_IDENT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_IDENT.Next());
                    	    
                    	    }
                    	    stream_IDENT.Reset();
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:98:47: ( UNIT )*
                    	    while ( stream_UNIT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_UNIT.Next());
                    	    
                    	    }
                    	    stream_UNIT.Reset();
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:98:53: ( attrib )*
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
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:99:4: '.' ( IDENT | UNIT ) ( attrib )*
                    {
                    	char_literal57 = (IToken)input.LT(1);
                    	Match(input,45,FOLLOW_45_in_elem635); 
                    	stream_45.Add(char_literal57);

                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:99:8: ( IDENT | UNIT )
                    	int alt24 = 2;
                    	int LA24_0 = input.LA(1);
                    	
                    	if ( (LA24_0 == IDENT) )
                    	{
                    	    alt24 = 1;
                    	}
                    	else if ( (LA24_0 == UNIT) )
                    	{
                    	    alt24 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d24s0 =
                    	        new NoViableAltException("99:8: ( IDENT | UNIT )", 24, 0, input);
                    	
                    	    throw nvae_d24s0;
                    	}
                    	switch (alt24) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:99:9: IDENT
                    	        {
                    	        	IDENT58 = (IToken)input.LT(1);
                    	        	Match(input,IDENT,FOLLOW_IDENT_in_elem638); 
                    	        	stream_IDENT.Add(IDENT58);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:99:17: UNIT
                    	        {
                    	        	UNIT59 = (IToken)input.LT(1);
                    	        	Match(input,UNIT,FOLLOW_UNIT_in_elem642); 
                    	        	stream_UNIT.Add(UNIT59);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:99:23: ( attrib )*
                    	do 
                    	{
                    	    int alt25 = 2;
                    	    int LA25_0 = input.LA(1);
                    	    
                    	    if ( (LA25_0 == 49) )
                    	    {
                    	        alt25 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt25) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:99:23: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem645);
                    			    	attrib60 = attrib();
                    			    	followingStackPointer_--;
                    			    	
                    			    	stream_attrib.Add(attrib60.Tree);
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop25;
                    	    }
                    	} while (true);
                    	
                    	loop25:
                    		;	// Stops C# compiler whinging that label 'loop25' has no statements

                    	
                    	// AST REWRITE
                    	// elements:          IDENT, attrib, UNIT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 99:31: -> ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* )
                    	{
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:99:34: ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(CLASS, "CLASS"), root_1);
                    	    
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:99:43: ( IDENT )*
                    	    while ( stream_IDENT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_IDENT.Next());
                    	    
                    	    }
                    	    stream_IDENT.Reset();
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:99:50: ( UNIT )*
                    	    while ( stream_UNIT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_UNIT.Next());
                    	    
                    	    }
                    	    stream_UNIT.Reset();
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:99:56: ( attrib )*
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
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:100:4: '*' ( attrib )*
                    {
                    	char_literal61 = (IToken)input.LT(1);
                    	Match(input,46,FOLLOW_46_in_elem668); 
                    	stream_46.Add(char_literal61);

                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:100:8: ( attrib )*
                    	do 
                    	{
                    	    int alt26 = 2;
                    	    int LA26_0 = input.LA(1);
                    	    
                    	    if ( (LA26_0 == 49) )
                    	    {
                    	        alt26 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt26) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:100:8: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem670);
                    			    	attrib62 = attrib();
                    			    	followingStackPointer_--;
                    			    	
                    			    	stream_attrib.Add(attrib62.Tree);
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop26;
                    	    }
                    	} while (true);
                    	
                    	loop26:
                    		;	// Stops C# compiler whinging that label 'loop26' has no statements

                    	
                    	// AST REWRITE
                    	// elements:          attrib
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 100:16: -> ^( ANY ( attrib )* )
                    	{
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:100:19: ^( ANY ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(ANY, "ANY"), root_1);
                    	    
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:100:26: ( attrib )*
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:103:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );
    public pseudo_return pseudo() // throws RecognitionException [1]
    {   
        pseudo_return retval = new pseudo_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal63 = null;
        IToken string_literal64 = null;
        IToken IDENT65 = null;
        IToken char_literal66 = null;
        IToken string_literal67 = null;
        function_return function68 = null;
        
        
        CommonTree char_literal63_tree=null;
        CommonTree string_literal64_tree=null;
        CommonTree IDENT65_tree=null;
        CommonTree char_literal66_tree=null;
        CommonTree string_literal67_tree=null;
        RewriteRuleTokenStream stream_47 = new RewriteRuleTokenStream(adaptor,"token 47");
        RewriteRuleTokenStream stream_48 = new RewriteRuleTokenStream(adaptor,"token 48");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleSubtreeStream stream_function = new RewriteRuleSubtreeStream(adaptor,"rule function");
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:104:2: ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) )
            int alt30 = 2;
            int LA30_0 = input.LA(1);
            
            if ( (LA30_0 == 47) )
            {
                int LA30_1 = input.LA(2);
                
                if ( (LA30_1 == IDENT) )
                {
                    int LA30_3 = input.LA(3);
                    
                    if ( (LA30_3 == 55) )
                    {
                        alt30 = 2;
                    }
                    else if ( (LA30_3 == 36 || LA30_3 == 40 || (LA30_3 >= 47 && LA30_3 <= 48) || LA30_3 == 56) )
                    {
                        alt30 = 1;
                    }
                    else 
                    {
                        NoViableAltException nvae_d30s3 =
                            new NoViableAltException("103:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );", 30, 3, input);
                    
                        throw nvae_d30s3;
                    }
                }
                else 
                {
                    NoViableAltException nvae_d30s1 =
                        new NoViableAltException("103:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );", 30, 1, input);
                
                    throw nvae_d30s1;
                }
            }
            else if ( (LA30_0 == 48) )
            {
                int LA30_2 = input.LA(2);
                
                if ( (LA30_2 == IDENT) )
                {
                    int LA30_3 = input.LA(3);
                    
                    if ( (LA30_3 == 55) )
                    {
                        alt30 = 2;
                    }
                    else if ( (LA30_3 == 36 || LA30_3 == 40 || (LA30_3 >= 47 && LA30_3 <= 48) || LA30_3 == 56) )
                    {
                        alt30 = 1;
                    }
                    else 
                    {
                        NoViableAltException nvae_d30s3 =
                            new NoViableAltException("103:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );", 30, 3, input);
                    
                        throw nvae_d30s3;
                    }
                }
                else 
                {
                    NoViableAltException nvae_d30s2 =
                        new NoViableAltException("103:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );", 30, 2, input);
                
                    throw nvae_d30s2;
                }
            }
            else 
            {
                NoViableAltException nvae_d30s0 =
                    new NoViableAltException("103:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );", 30, 0, input);
            
                throw nvae_d30s0;
            }
            switch (alt30) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:104:4: ( ':' | '::' ) IDENT
                    {
                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:104:4: ( ':' | '::' )
                    	int alt28 = 2;
                    	int LA28_0 = input.LA(1);
                    	
                    	if ( (LA28_0 == 47) )
                    	{
                    	    alt28 = 1;
                    	}
                    	else if ( (LA28_0 == 48) )
                    	{
                    	    alt28 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d28s0 =
                    	        new NoViableAltException("104:4: ( ':' | '::' )", 28, 0, input);
                    	
                    	    throw nvae_d28s0;
                    	}
                    	switch (alt28) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:104:5: ':'
                    	        {
                    	        	char_literal63 = (IToken)input.LT(1);
                    	        	Match(input,47,FOLLOW_47_in_pseudo694); 
                    	        	stream_47.Add(char_literal63);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:104:9: '::'
                    	        {
                    	        	string_literal64 = (IToken)input.LT(1);
                    	        	Match(input,48,FOLLOW_48_in_pseudo696); 
                    	        	stream_48.Add(string_literal64);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	IDENT65 = (IToken)input.LT(1);
                    	Match(input,IDENT,FOLLOW_IDENT_in_pseudo699); 
                    	stream_IDENT.Add(IDENT65);

                    	
                    	// AST REWRITE
                    	// elements:          IDENT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 104:21: -> ^( PSEUDO IDENT )
                    	{
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:104:24: ^( PSEUDO IDENT )
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
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:105:4: ( ':' | '::' ) function
                    {
                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:105:4: ( ':' | '::' )
                    	int alt29 = 2;
                    	int LA29_0 = input.LA(1);
                    	
                    	if ( (LA29_0 == 47) )
                    	{
                    	    alt29 = 1;
                    	}
                    	else if ( (LA29_0 == 48) )
                    	{
                    	    alt29 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d29s0 =
                    	        new NoViableAltException("105:4: ( ':' | '::' )", 29, 0, input);
                    	
                    	    throw nvae_d29s0;
                    	}
                    	switch (alt29) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:105:5: ':'
                    	        {
                    	        	char_literal66 = (IToken)input.LT(1);
                    	        	Match(input,47,FOLLOW_47_in_pseudo715); 
                    	        	stream_47.Add(char_literal66);

                    	        
                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:105:9: '::'
                    	        {
                    	        	string_literal67 = (IToken)input.LT(1);
                    	        	Match(input,48,FOLLOW_48_in_pseudo717); 
                    	        	stream_48.Add(string_literal67);

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	PushFollow(FOLLOW_function_in_pseudo720);
                    	function68 = function();
                    	followingStackPointer_--;
                    	
                    	stream_function.Add(function68.Tree);
                    	
                    	// AST REWRITE
                    	// elements:          function
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 105:24: -> ^( PSEUDO function )
                    	{
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:105:27: ^( PSEUDO function )
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:108:1: attrib : '[' IDENT ( attribRelate ( STRING | IDENT ) )? ']' -> ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? ) ;
    public attrib_return attrib() // throws RecognitionException [1]
    {   
        attrib_return retval = new attrib_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal69 = null;
        IToken IDENT70 = null;
        IToken STRING72 = null;
        IToken IDENT73 = null;
        IToken char_literal74 = null;
        attribRelate_return attribRelate71 = null;
        
        
        CommonTree char_literal69_tree=null;
        CommonTree IDENT70_tree=null;
        CommonTree STRING72_tree=null;
        CommonTree IDENT73_tree=null;
        CommonTree char_literal74_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_49 = new RewriteRuleTokenStream(adaptor,"token 49");
        RewriteRuleTokenStream stream_STRING = new RewriteRuleTokenStream(adaptor,"token STRING");
        RewriteRuleTokenStream stream_50 = new RewriteRuleTokenStream(adaptor,"token 50");
        RewriteRuleSubtreeStream stream_attribRelate = new RewriteRuleSubtreeStream(adaptor,"rule attribRelate");
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:109:2: ( '[' IDENT ( attribRelate ( STRING | IDENT ) )? ']' -> ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? ) )
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:109:4: '[' IDENT ( attribRelate ( STRING | IDENT ) )? ']'
            {
            	char_literal69 = (IToken)input.LT(1);
            	Match(input,49,FOLLOW_49_in_attrib741); 
            	stream_49.Add(char_literal69);

            	IDENT70 = (IToken)input.LT(1);
            	Match(input,IDENT,FOLLOW_IDENT_in_attrib743); 
            	stream_IDENT.Add(IDENT70);

            	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:109:14: ( attribRelate ( STRING | IDENT ) )?
            	int alt32 = 2;
            	int LA32_0 = input.LA(1);
            	
            	if ( ((LA32_0 >= 51 && LA32_0 <= 53)) )
            	{
            	    alt32 = 1;
            	}
            	switch (alt32) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:109:15: attribRelate ( STRING | IDENT )
            	        {
            	        	PushFollow(FOLLOW_attribRelate_in_attrib746);
            	        	attribRelate71 = attribRelate();
            	        	followingStackPointer_--;
            	        	
            	        	stream_attribRelate.Add(attribRelate71.Tree);
            	        	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:109:28: ( STRING | IDENT )
            	        	int alt31 = 2;
            	        	int LA31_0 = input.LA(1);
            	        	
            	        	if ( (LA31_0 == STRING) )
            	        	{
            	        	    alt31 = 1;
            	        	}
            	        	else if ( (LA31_0 == IDENT) )
            	        	{
            	        	    alt31 = 2;
            	        	}
            	        	else 
            	        	{
            	        	    NoViableAltException nvae_d31s0 =
            	        	        new NoViableAltException("109:28: ( STRING | IDENT )", 31, 0, input);
            	        	
            	        	    throw nvae_d31s0;
            	        	}
            	        	switch (alt31) 
            	        	{
            	        	    case 1 :
            	        	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:109:29: STRING
            	        	        {
            	        	        	STRING72 = (IToken)input.LT(1);
            	        	        	Match(input,STRING,FOLLOW_STRING_in_attrib749); 
            	        	        	stream_STRING.Add(STRING72);

            	        	        
            	        	        }
            	        	        break;
            	        	    case 2 :
            	        	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:109:38: IDENT
            	        	        {
            	        	        	IDENT73 = (IToken)input.LT(1);
            	        	        	Match(input,IDENT,FOLLOW_IDENT_in_attrib753); 
            	        	        	stream_IDENT.Add(IDENT73);

            	        	        
            	        	        }
            	        	        break;
            	        	
            	        	}

            	        
            	        }
            	        break;
            	
            	}

            	char_literal74 = (IToken)input.LT(1);
            	Match(input,50,FOLLOW_50_in_attrib758); 
            	stream_50.Add(char_literal74);

            	
            	// AST REWRITE
            	// elements:          IDENT, STRING, attribRelate, IDENT
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 109:51: -> ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? )
            	{
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:109:54: ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot(adaptor.Create(ATTRIB, "ATTRIB"), root_1);
            	    
            	    adaptor.AddChild(root_1, stream_IDENT.Next());
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:109:70: ( attribRelate ( STRING )* ( IDENT )* )?
            	    if ( stream_IDENT.HasNext() || stream_STRING.HasNext() || stream_attribRelate.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_attribRelate.Next());
            	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:109:84: ( STRING )*
            	        while ( stream_STRING.HasNext() )
            	        {
            	            adaptor.AddChild(root_1, stream_STRING.Next());
            	        
            	        }
            	        stream_STRING.Reset();
            	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:109:92: ( IDENT )*
            	        while ( stream_IDENT.HasNext() )
            	        {
            	            adaptor.AddChild(root_1, stream_IDENT.Next());
            	        
            	        }
            	        stream_IDENT.Reset();
            	    
            	    }
            	    stream_IDENT.Reset();
            	    stream_STRING.Reset();
            	    stream_attribRelate.Reset();
            	    
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:112:1: attribRelate : ( '=' -> ATTRIBEQUAL | '~=' -> HASVALUE | '|=' -> BEGINSWITH );
    public attribRelate_return attribRelate() // throws RecognitionException [1]
    {   
        attribRelate_return retval = new attribRelate_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal75 = null;
        IToken string_literal76 = null;
        IToken string_literal77 = null;
        
        CommonTree char_literal75_tree=null;
        CommonTree string_literal76_tree=null;
        CommonTree string_literal77_tree=null;
        RewriteRuleTokenStream stream_51 = new RewriteRuleTokenStream(adaptor,"token 51");
        RewriteRuleTokenStream stream_52 = new RewriteRuleTokenStream(adaptor,"token 52");
        RewriteRuleTokenStream stream_53 = new RewriteRuleTokenStream(adaptor,"token 53");
    
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:113:2: ( '=' -> ATTRIBEQUAL | '~=' -> HASVALUE | '|=' -> BEGINSWITH )
            int alt33 = 3;
            switch ( input.LA(1) ) 
            {
            case 51:
            	{
                alt33 = 1;
                }
                break;
            case 52:
            	{
                alt33 = 2;
                }
                break;
            case 53:
            	{
                alt33 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d33s0 =
            	        new NoViableAltException("112:1: attribRelate : ( '=' -> ATTRIBEQUAL | '~=' -> HASVALUE | '|=' -> BEGINSWITH );", 33, 0, input);
            
            	    throw nvae_d33s0;
            }
            
            switch (alt33) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:113:4: '='
                    {
                    	char_literal75 = (IToken)input.LT(1);
                    	Match(input,51,FOLLOW_51_in_attribRelate791); 
                    	stream_51.Add(char_literal75);

                    	
                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 113:9: -> ATTRIBEQUAL
                    	{
                    	    adaptor.AddChild(root_0, adaptor.Create(ATTRIBEQUAL, "ATTRIBEQUAL"));
                    	
                    	}
                    	

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:114:4: '~='
                    {
                    	string_literal76 = (IToken)input.LT(1);
                    	Match(input,52,FOLLOW_52_in_attribRelate801); 
                    	stream_52.Add(string_literal76);

                    	
                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 114:9: -> HASVALUE
                    	{
                    	    adaptor.AddChild(root_0, adaptor.Create(HASVALUE, "HASVALUE"));
                    	
                    	}
                    	

                    
                    }
                    break;
                case 3 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:115:4: '|='
                    {
                    	string_literal77 = (IToken)input.LT(1);
                    	Match(input,53,FOLLOW_53_in_attribRelate810); 
                    	stream_53.Add(string_literal77);

                    	
                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 115:9: -> BEGINSWITH
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:118:1: declaration : IDENT ':' args -> ^( PROPERTY IDENT args ) ;
    public declaration_return declaration() // throws RecognitionException [1]
    {   
        declaration_return retval = new declaration_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken IDENT78 = null;
        IToken char_literal79 = null;
        args_return args80 = null;
        
        
        CommonTree IDENT78_tree=null;
        CommonTree char_literal79_tree=null;
        RewriteRuleTokenStream stream_47 = new RewriteRuleTokenStream(adaptor,"token 47");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleSubtreeStream stream_args = new RewriteRuleSubtreeStream(adaptor,"rule args");
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:119:2: ( IDENT ':' args -> ^( PROPERTY IDENT args ) )
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:119:4: IDENT ':' args
            {
            	IDENT78 = (IToken)input.LT(1);
            	Match(input,IDENT,FOLLOW_IDENT_in_declaration828); 
            	stream_IDENT.Add(IDENT78);

            	char_literal79 = (IToken)input.LT(1);
            	Match(input,47,FOLLOW_47_in_declaration830); 
            	stream_47.Add(char_literal79);

            	PushFollow(FOLLOW_args_in_declaration832);
            	args80 = args();
            	followingStackPointer_--;
            	
            	stream_args.Add(args80.Tree);
            	
            	// AST REWRITE
            	// elements:          IDENT, args
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 119:19: -> ^( PROPERTY IDENT args )
            	{
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:119:22: ^( PROPERTY IDENT args )
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:122:1: args : expr ( ( ',' )? expr )* -> ( expr )* ;
    public args_return args() // throws RecognitionException [1]
    {   
        args_return retval = new args_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken char_literal82 = null;
        expr_return expr81 = null;

        expr_return expr83 = null;
        
        
        CommonTree char_literal82_tree=null;
        RewriteRuleTokenStream stream_40 = new RewriteRuleTokenStream(adaptor,"token 40");
        RewriteRuleSubtreeStream stream_expr = new RewriteRuleSubtreeStream(adaptor,"rule expr");
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:123:2: ( expr ( ( ',' )? expr )* -> ( expr )* )
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:123:4: expr ( ( ',' )? expr )*
            {
            	PushFollow(FOLLOW_expr_in_args855);
            	expr81 = expr();
            	followingStackPointer_--;
            	
            	stream_expr.Add(expr81.Tree);
            	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:123:9: ( ( ',' )? expr )*
            	do 
            	{
            	    int alt35 = 2;
            	    int LA35_0 = input.LA(1);
            	    
            	    if ( ((LA35_0 >= STRING && LA35_0 <= IDENT) || (LA35_0 >= NUM && LA35_0 <= COLOR) || LA35_0 == 40) )
            	    {
            	        alt35 = 1;
            	    }
            	    
            	
            	    switch (alt35) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:123:10: ( ',' )? expr
            			    {
            			    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:123:10: ( ',' )?
            			    	int alt34 = 2;
            			    	int LA34_0 = input.LA(1);
            			    	
            			    	if ( (LA34_0 == 40) )
            			    	{
            			    	    alt34 = 1;
            			    	}
            			    	switch (alt34) 
            			    	{
            			    	    case 1 :
            			    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:123:10: ','
            			    	        {
            			    	        	char_literal82 = (IToken)input.LT(1);
            			    	        	Match(input,40,FOLLOW_40_in_args858); 
            			    	        	stream_40.Add(char_literal82);

            			    	        
            			    	        }
            			    	        break;
            			    	
            			    	}

            			    	PushFollow(FOLLOW_expr_in_args861);
            			    	expr83 = expr();
            			    	followingStackPointer_--;
            			    	
            			    	stream_expr.Add(expr83.Tree);
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop35;
            	    }
            	} while (true);
            	
            	loop35:
            		;	// Stops C# compiler whinging that label 'loop35' has no statements

            	
            	// AST REWRITE
            	// elements:          expr
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	retval.tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
            	
            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 123:22: -> ( expr )*
            	{
            	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:123:25: ( expr )*
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:126:1: expr : ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function );
    public expr_return expr() // throws RecognitionException [1]
    {   
        expr_return retval = new expr_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken NUM84 = null;
        IToken IDENT86 = null;
        IToken COLOR87 = null;
        IToken STRING88 = null;
        unit_return unit85 = null;

        function_return function89 = null;
        
        
        CommonTree NUM84_tree=null;
        CommonTree IDENT86_tree=null;
        CommonTree COLOR87_tree=null;
        CommonTree STRING88_tree=null;
    
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:127:2: ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function )
            int alt37 = 5;
            switch ( input.LA(1) ) 
            {
            case NUM:
            	{
                alt37 = 1;
                }
                break;
            case IDENT:
            	{
                int LA37_2 = input.LA(2);
                
                if ( (LA37_2 == 55) )
                {
                    alt37 = 5;
                }
                else if ( ((LA37_2 >= STRING && LA37_2 <= IDENT) || (LA37_2 >= NUM && LA37_2 <= COLOR) || LA37_2 == 34 || LA37_2 == 37 || (LA37_2 >= 39 && LA37_2 <= 40) || LA37_2 == 56) )
                {
                    alt37 = 2;
                }
                else 
                {
                    NoViableAltException nvae_d37s2 =
                        new NoViableAltException("126:1: expr : ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function );", 37, 2, input);
                
                    throw nvae_d37s2;
                }
                }
                break;
            case COLOR:
            	{
                alt37 = 3;
                }
                break;
            case STRING:
            	{
                alt37 = 4;
                }
                break;
            	default:
            	    NoViableAltException nvae_d37s0 =
            	        new NoViableAltException("126:1: expr : ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function );", 37, 0, input);
            
            	    throw nvae_d37s0;
            }
            
            switch (alt37) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:127:4: ( NUM ( unit )? )
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    
                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:127:4: ( NUM ( unit )? )
                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:127:5: NUM ( unit )?
                    	{
                    		NUM84 = (IToken)input.LT(1);
                    		Match(input,NUM,FOLLOW_NUM_in_expr880); 
                    		NUM84_tree = (CommonTree)adaptor.Create(NUM84);
                    		adaptor.AddChild(root_0, NUM84_tree);

                    		// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:127:9: ( unit )?
                    		int alt36 = 2;
                    		int LA36_0 = input.LA(1);
                    		
                    		if ( (LA36_0 == UNIT || LA36_0 == 54) )
                    		{
                    		    alt36 = 1;
                    		}
                    		switch (alt36) 
                    		{
                    		    case 1 :
                    		        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:127:9: unit
                    		        {
                    		        	PushFollow(FOLLOW_unit_in_expr882);
                    		        	unit85 = unit();
                    		        	followingStackPointer_--;
                    		        	
                    		        	adaptor.AddChild(root_0, unit85.Tree);
                    		        
                    		        }
                    		        break;
                    		
                    		}

                    	
                    	}

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:128:4: IDENT
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    
                    	IDENT86 = (IToken)input.LT(1);
                    	Match(input,IDENT,FOLLOW_IDENT_in_expr889); 
                    	IDENT86_tree = (CommonTree)adaptor.Create(IDENT86);
                    	adaptor.AddChild(root_0, IDENT86_tree);

                    
                    }
                    break;
                case 3 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:129:4: COLOR
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    
                    	COLOR87 = (IToken)input.LT(1);
                    	Match(input,COLOR,FOLLOW_COLOR_in_expr894); 
                    	COLOR87_tree = (CommonTree)adaptor.Create(COLOR87);
                    	adaptor.AddChild(root_0, COLOR87_tree);

                    
                    }
                    break;
                case 4 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:130:4: STRING
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    
                    	STRING88 = (IToken)input.LT(1);
                    	Match(input,STRING,FOLLOW_STRING_in_expr899); 
                    	STRING88_tree = (CommonTree)adaptor.Create(STRING88);
                    	adaptor.AddChild(root_0, STRING88_tree);

                    
                    }
                    break;
                case 5 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:131:4: function
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    
                    	PushFollow(FOLLOW_function_in_expr904);
                    	function89 = function();
                    	followingStackPointer_--;
                    	
                    	adaptor.AddChild(root_0, function89.Tree);
                    
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:134:1: unit : ( '%' | UNIT ) ;
    public unit_return unit() // throws RecognitionException [1]
    {   
        unit_return retval = new unit_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken set90 = null;
        
        CommonTree set90_tree=null;
    
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:135:2: ( ( '%' | UNIT ) )
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:135:4: ( '%' | UNIT )
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();
            
            	set90 = (IToken)input.LT(1);
            	if ( input.LA(1) == UNIT || input.LA(1) == 54 ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, adaptor.Create(set90));
            	    errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse =
            	        new MismatchedSetException(null,input);
            	    RecoverFromMismatchedSet(input,mse,FOLLOW_set_in_unit915);    throw mse;
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
    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:138:1: function : ( IDENT '(' ( args )? ')' -> IDENT '(' ( args )* ')' | IDENT '(' ( selector )? ')' -> IDENT '(' ( selector )* ')' );
    public function_return function() // throws RecognitionException [1]
    {   
        function_return retval = new function_return();
        retval.start = input.LT(1);
        
        CommonTree root_0 = null;
    
        IToken IDENT91 = null;
        IToken char_literal92 = null;
        IToken char_literal94 = null;
        IToken IDENT95 = null;
        IToken char_literal96 = null;
        IToken char_literal98 = null;
        args_return args93 = null;

        selector_return selector97 = null;
        
        
        CommonTree IDENT91_tree=null;
        CommonTree char_literal92_tree=null;
        CommonTree char_literal94_tree=null;
        CommonTree IDENT95_tree=null;
        CommonTree char_literal96_tree=null;
        CommonTree char_literal98_tree=null;
        RewriteRuleTokenStream stream_55 = new RewriteRuleTokenStream(adaptor,"token 55");
        RewriteRuleTokenStream stream_56 = new RewriteRuleTokenStream(adaptor,"token 56");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleSubtreeStream stream_args = new RewriteRuleSubtreeStream(adaptor,"rule args");
        RewriteRuleSubtreeStream stream_selector = new RewriteRuleSubtreeStream(adaptor,"rule selector");
        try 
    	{
            // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:139:2: ( IDENT '(' ( args )? ')' -> IDENT '(' ( args )* ')' | IDENT '(' ( selector )? ')' -> IDENT '(' ( selector )* ')' )
            int alt40 = 2;
            alt40 = dfa40.Predict(input);
            switch (alt40) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:139:4: IDENT '(' ( args )? ')'
                    {
                    	IDENT91 = (IToken)input.LT(1);
                    	Match(input,IDENT,FOLLOW_IDENT_in_function932); 
                    	stream_IDENT.Add(IDENT91);

                    	char_literal92 = (IToken)input.LT(1);
                    	Match(input,55,FOLLOW_55_in_function934); 
                    	stream_55.Add(char_literal92);

                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:139:14: ( args )?
                    	int alt38 = 2;
                    	int LA38_0 = input.LA(1);
                    	
                    	if ( ((LA38_0 >= STRING && LA38_0 <= IDENT) || (LA38_0 >= NUM && LA38_0 <= COLOR)) )
                    	{
                    	    alt38 = 1;
                    	}
                    	switch (alt38) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:139:14: args
                    	        {
                    	        	PushFollow(FOLLOW_args_in_function936);
                    	        	args93 = args();
                    	        	followingStackPointer_--;
                    	        	
                    	        	stream_args.Add(args93.Tree);
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	char_literal94 = (IToken)input.LT(1);
                    	Match(input,56,FOLLOW_56_in_function939); 
                    	stream_56.Add(char_literal94);

                    	
                    	// AST REWRITE
                    	// elements:          56, args, IDENT, 55
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 139:24: -> IDENT '(' ( args )* ')'
                    	{
                    	    adaptor.AddChild(root_0, stream_IDENT.Next());
                    	    adaptor.AddChild(root_0, stream_55.Next());
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:139:37: ( args )*
                    	    while ( stream_args.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_0, stream_args.Next());
                    	    
                    	    }
                    	    stream_args.Reset();
                    	    adaptor.AddChild(root_0, stream_56.Next());
                    	
                    	}
                    	

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:140:4: IDENT '(' ( selector )? ')'
                    {
                    	IDENT95 = (IToken)input.LT(1);
                    	Match(input,IDENT,FOLLOW_IDENT_in_function955); 
                    	stream_IDENT.Add(IDENT95);

                    	char_literal96 = (IToken)input.LT(1);
                    	Match(input,55,FOLLOW_55_in_function957); 
                    	stream_55.Add(char_literal96);

                    	// C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:140:14: ( selector )?
                    	int alt39 = 2;
                    	int LA39_0 = input.LA(1);
                    	
                    	if ( ((LA39_0 >= IDENT && LA39_0 <= UNIT) || (LA39_0 >= 44 && LA39_0 <= 48)) )
                    	{
                    	    alt39 = 1;
                    	}
                    	switch (alt39) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:140:14: selector
                    	        {
                    	        	PushFollow(FOLLOW_selector_in_function959);
                    	        	selector97 = selector();
                    	        	followingStackPointer_--;
                    	        	
                    	        	stream_selector.Add(selector97.Tree);
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	char_literal98 = (IToken)input.LT(1);
                    	Match(input,56,FOLLOW_56_in_function962); 
                    	stream_56.Add(char_literal98);

                    	
                    	// AST REWRITE
                    	// elements:          56, 55, selector, IDENT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	retval.tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "token retval", (retval!=null ? retval.Tree : null));
                    	
                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 140:28: -> IDENT '(' ( selector )* ')'
                    	{
                    	    adaptor.AddChild(root_0, stream_IDENT.Next());
                    	    adaptor.AddChild(root_0, stream_55.Next());
                    	    // C:\\Users\\Trihus\\git\\SimpleCss5\\src\\SimpleCss5\\csst3.g3:140:41: ( selector )*
                    	    while ( stream_selector.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_0, stream_selector.Next());
                    	    
                    	    }
                    	    stream_selector.Reset();
                    	    adaptor.AddChild(root_0, stream_56.Next());
                    	
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
    // $ANTLR end function


   	protected DFA40 dfa40;
	private void InitializeCyclicDFAs()
	{
    	this.dfa40 = new DFA40(this);
	}

    static readonly short[] DFA40_eot = {
        -1, -1, -1, -1, -1, -1, -1
        };
    static readonly short[] DFA40_eof = {
        -1, -1, -1, -1, -1, -1, -1
        };
    static readonly int[] DFA40_min = {
        25, 55, 24, 24, 0, 0, 24
        };
    static readonly int[] DFA40_max = {
        25, 55, 56, 56, 0, 0, 56
        };
    static readonly short[] DFA40_accept = {
        -1, -1, -1, -1, 2, 1, -1
        };
    static readonly short[] DFA40_special = {
        -1, -1, -1, -1, -1, -1, -1
        };
    
    static readonly short[] dfa40_transition_null = null;

    static readonly short[] dfa40_transition0 = {
    	1
    	};
    static readonly short[] dfa40_transition1 = {
    	2
    	};
    static readonly short[] dfa40_transition2 = {
    	5, 6, 4, 5, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 5, 4, 4, 
    	    4, 4, 4, 4, 4, 4, 4, -1, -1, -1, -1, -1, 5, 5
    	};
    static readonly short[] dfa40_transition3 = {
    	5, 3, 4, 5, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 
    	    -1, -1, 4, 4, 4, 4, 4, -1, -1, -1, -1, -1, -1, -1, 5
    	};
    
    static readonly short[][] DFA40_transition = {
    	dfa40_transition0,
    	dfa40_transition1,
    	dfa40_transition3,
    	dfa40_transition2,
    	dfa40_transition_null,
    	dfa40_transition_null,
    	dfa40_transition2
        };
    
    protected class DFA40 : DFA
    {
        public DFA40(BaseRecognizer recognizer) 
        {
            this.recognizer = recognizer;
            this.decisionNumber = 40;
            this.eot = DFA40_eot;
            this.eof = DFA40_eof;
            this.min = DFA40_min;
            this.max = DFA40_max;
            this.accept     = DFA40_accept;
            this.special    = DFA40_special;
            this.transition = DFA40_transition;
        }
    
        override public string Description
        {
            get { return "138:1: function : ( IDENT '(' ( args )? ')' -> IDENT '(' ( args )* ')' | IDENT '(' ( selector )? ')' -> IDENT '(' ( selector )* ')' );"; }
        }
    
    }
    
 

    public static readonly BitSet FOLLOW_importRule_in_stylesheet174 = new BitSet(new ulong[]{0x0001F04B06000002UL});
    public static readonly BitSet FOLLOW_media_in_stylesheet178 = new BitSet(new ulong[]{0x0001F04B06000002UL});
    public static readonly BitSet FOLLOW_pageRule_in_stylesheet182 = new BitSet(new ulong[]{0x0001F04B06000002UL});
    public static readonly BitSet FOLLOW_ruleset_in_stylesheet186 = new BitSet(new ulong[]{0x0001F04B06000002UL});
    public static readonly BitSet FOLLOW_32_in_importRule200 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_33_in_importRule204 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_STRING_in_importRule208 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_34_in_importRule210 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_32_in_importRule226 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_33_in_importRule230 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_function_in_importRule234 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_34_in_importRule236 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_35_in_media257 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_media259 = new BitSet(new ulong[]{0x0000001000000000UL});
    public static readonly BitSet FOLLOW_36_in_media261 = new BitSet(new ulong[]{0x0001F04006000000UL});
    public static readonly BitSet FOLLOW_pageRule_in_media264 = new BitSet(new ulong[]{0x0001F06006000000UL});
    public static readonly BitSet FOLLOW_ruleset_in_media268 = new BitSet(new ulong[]{0x0001F06006000000UL});
    public static readonly BitSet FOLLOW_37_in_media272 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_38_in_pageRule300 = new BitSet(new ulong[]{0x0001801002000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_pageRule302 = new BitSet(new ulong[]{0x0001801002000000UL});
    public static readonly BitSet FOLLOW_pseudo_in_pageRule305 = new BitSet(new ulong[]{0x0001801000000000UL});
    public static readonly BitSet FOLLOW_36_in_pageRule308 = new BitSet(new ulong[]{0x000000A002000000UL});
    public static readonly BitSet FOLLOW_properties_in_pageRule310 = new BitSet(new ulong[]{0x000000A000000000UL});
    public static readonly BitSet FOLLOW_region_in_pageRule313 = new BitSet(new ulong[]{0x000000A000000000UL});
    public static readonly BitSet FOLLOW_37_in_pageRule316 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_39_in_region347 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_region349 = new BitSet(new ulong[]{0x0000001000000000UL});
    public static readonly BitSet FOLLOW_36_in_region351 = new BitSet(new ulong[]{0x0000002002000000UL});
    public static readonly BitSet FOLLOW_properties_in_region353 = new BitSet(new ulong[]{0x0000002000000000UL});
    public static readonly BitSet FOLLOW_37_in_region356 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selectors_in_ruleset381 = new BitSet(new ulong[]{0x0000001000000000UL});
    public static readonly BitSet FOLLOW_36_in_ruleset383 = new BitSet(new ulong[]{0x0000002002000000UL});
    public static readonly BitSet FOLLOW_properties_in_ruleset385 = new BitSet(new ulong[]{0x0000002000000000UL});
    public static readonly BitSet FOLLOW_37_in_ruleset388 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selector_in_selectors413 = new BitSet(new ulong[]{0x0000010000000002UL});
    public static readonly BitSet FOLLOW_40_in_selectors416 = new BitSet(new ulong[]{0x0001F00006000000UL});
    public static readonly BitSet FOLLOW_selector_in_selectors418 = new BitSet(new ulong[]{0x0000010000000002UL});
    public static readonly BitSet FOLLOW_elem_in_selector432 = new BitSet(new ulong[]{0x0001FE0006000002UL});
    public static readonly BitSet FOLLOW_selectorOperation_in_selector434 = new BitSet(new ulong[]{0x0001FE0006000002UL});
    public static readonly BitSet FOLLOW_pseudo_in_selector437 = new BitSet(new ulong[]{0x0001800000000002UL});
    public static readonly BitSet FOLLOW_pseudo_in_selector454 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selectop_in_selectorOperation472 = new BitSet(new ulong[]{0x0000700006000000UL});
    public static readonly BitSet FOLLOW_elem_in_selectorOperation475 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_41_in_selectop493 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_42_in_selectop509 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_43_in_selectop526 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_declaration_in_properties541 = new BitSet(new ulong[]{0x0000000400000002UL});
    public static readonly BitSet FOLLOW_34_in_properties544 = new BitSet(new ulong[]{0x0000000402000002UL});
    public static readonly BitSet FOLLOW_declaration_in_properties546 = new BitSet(new ulong[]{0x0000000400000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_elem572 = new BitSet(new ulong[]{0x0002000000000002UL});
    public static readonly BitSet FOLLOW_UNIT_in_elem576 = new BitSet(new ulong[]{0x0002000000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem579 = new BitSet(new ulong[]{0x0002000000000002UL});
    public static readonly BitSet FOLLOW_44_in_elem602 = new BitSet(new ulong[]{0x0000000006000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_elem605 = new BitSet(new ulong[]{0x0002000000000002UL});
    public static readonly BitSet FOLLOW_UNIT_in_elem609 = new BitSet(new ulong[]{0x0002000000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem612 = new BitSet(new ulong[]{0x0002000000000002UL});
    public static readonly BitSet FOLLOW_45_in_elem635 = new BitSet(new ulong[]{0x0000000006000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_elem638 = new BitSet(new ulong[]{0x0002000000000002UL});
    public static readonly BitSet FOLLOW_UNIT_in_elem642 = new BitSet(new ulong[]{0x0002000000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem645 = new BitSet(new ulong[]{0x0002000000000002UL});
    public static readonly BitSet FOLLOW_46_in_elem668 = new BitSet(new ulong[]{0x0002000000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem670 = new BitSet(new ulong[]{0x0002000000000002UL});
    public static readonly BitSet FOLLOW_47_in_pseudo694 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_48_in_pseudo696 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_pseudo699 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_47_in_pseudo715 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_48_in_pseudo717 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_function_in_pseudo720 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_49_in_attrib741 = new BitSet(new ulong[]{0x0000000002000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_attrib743 = new BitSet(new ulong[]{0x003C000000000000UL});
    public static readonly BitSet FOLLOW_attribRelate_in_attrib746 = new BitSet(new ulong[]{0x0000000003000000UL});
    public static readonly BitSet FOLLOW_STRING_in_attrib749 = new BitSet(new ulong[]{0x0004000000000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_attrib753 = new BitSet(new ulong[]{0x0004000000000000UL});
    public static readonly BitSet FOLLOW_50_in_attrib758 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_51_in_attribRelate791 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_52_in_attribRelate801 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_53_in_attribRelate810 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_declaration828 = new BitSet(new ulong[]{0x0000800000000000UL});
    public static readonly BitSet FOLLOW_47_in_declaration830 = new BitSet(new ulong[]{0x000000001B000000UL});
    public static readonly BitSet FOLLOW_args_in_declaration832 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expr_in_args855 = new BitSet(new ulong[]{0x000001001B000002UL});
    public static readonly BitSet FOLLOW_40_in_args858 = new BitSet(new ulong[]{0x000000001B000000UL});
    public static readonly BitSet FOLLOW_expr_in_args861 = new BitSet(new ulong[]{0x000001001B000002UL});
    public static readonly BitSet FOLLOW_NUM_in_expr880 = new BitSet(new ulong[]{0x0040000004000002UL});
    public static readonly BitSet FOLLOW_unit_in_expr882 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_expr889 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_COLOR_in_expr894 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STRING_in_expr899 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_expr904 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_unit915 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_function932 = new BitSet(new ulong[]{0x0080000000000000UL});
    public static readonly BitSet FOLLOW_55_in_function934 = new BitSet(new ulong[]{0x010000001B000000UL});
    public static readonly BitSet FOLLOW_args_in_function936 = new BitSet(new ulong[]{0x0100000000000000UL});
    public static readonly BitSet FOLLOW_56_in_function939 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_function955 = new BitSet(new ulong[]{0x0080000000000000UL});
    public static readonly BitSet FOLLOW_55_in_function957 = new BitSet(new ulong[]{0x0101F00006000000UL});
    public static readonly BitSet FOLLOW_selector_in_function959 = new BitSet(new ulong[]{0x0100000000000000UL});
    public static readonly BitSet FOLLOW_56_in_function962 = new BitSet(new ulong[]{0x0000000000000002UL});

}
}