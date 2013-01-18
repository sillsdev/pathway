// $ANTLR 3.1.2 C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g 2013-01-18 15:58:55


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
		"STRING", 
		"IDENT", 
		"UNIT", 
		"NUM", 
		"COLOR", 
		"SL_COMMENT", 
		"COMMENT", 
		"WS", 
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

    public const int FUNCTION = 17;
    public const int CLASS = 21;
    public const int ATTRIB = 9;
    public const int HASVALUE = 13;
    public const int PSEUDO = 15;
    public const int MEDIA = 5;
    public const int ID = 20;
    public const int EOF = -1;
    public const int ATTRIBEQUAL = 12;
    public const int COLOR = 26;
    public const int REGION = 7;
    public const int IMPORT = 4;
    public const int T__51 = 51;
    public const int T__52 = 52;
    public const int T__53 = 53;
    public const int IDENT = 23;
    public const int COMMENT = 28;
    public const int T__50 = 50;
    public const int T__42 = 42;
    public const int T__43 = 43;
    public const int T__40 = 40;
    public const int T__41 = 41;
    public const int T__46 = 46;
    public const int T__47 = 47;
    public const int T__44 = 44;
    public const int RULE = 8;
    public const int T__45 = 45;
    public const int PARENTOF = 10;
    public const int BEGINSWITH = 14;
    public const int T__48 = 48;
    public const int T__49 = 49;
    public const int PRECEDES = 11;
    public const int NUM = 25;
    public const int TAG = 19;
    public const int T__30 = 30;
    public const int T__31 = 31;
    public const int UNIT = 24;
    public const int T__32 = 32;
    public const int WS = 29;
    public const int T__33 = 33;
    public const int PAGE = 6;
    public const int ANY = 18;
    public const int T__34 = 34;
    public const int T__35 = 35;
    public const int T__36 = 36;
    public const int T__37 = 37;
    public const int PROPERTY = 16;
    public const int T__38 = 38;
    public const int T__39 = 39;
    public const int SL_COMMENT = 27;
    public const int STRING = 22;

    // delegates
    // delegators



        public csst3Parser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public csst3Parser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();

             
       }
        
    protected ITreeAdaptor adaptor = new CommonTreeAdaptor();

    public ITreeAdaptor TreeAdaptor
    {
        get { return this.adaptor; }
        set {
    	this.adaptor = value;
    	}
    }

    override public string[] TokenNames {
		get { return csst3Parser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g"; }
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
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "stylesheet"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:45:1: stylesheet : ( importRule )* ( media | pageRule | ruleset )+ ;
    public csst3Parser.stylesheet_return stylesheet() // throws RecognitionException [1]
    {   
        csst3Parser.stylesheet_return retval = new csst3Parser.stylesheet_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        csst3Parser.importRule_return importRule1 = null;

        csst3Parser.media_return media2 = null;

        csst3Parser.pageRule_return pageRule3 = null;

        csst3Parser.ruleset_return ruleset4 = null;



        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:46:2: ( ( importRule )* ( media | pageRule | ruleset )+ )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:46:4: ( importRule )* ( media | pageRule | ruleset )+
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();

            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:46:4: ( importRule )*
            	do 
            	{
            	    int alt1 = 2;
            	    alt1 = dfa1.Predict(input);
            	    switch (alt1) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:46:4: importRule
            			    {
            			    	PushFollow(FOLLOW_importRule_in_stylesheet146);
            			    	importRule1 = importRule();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, importRule1.Tree);

            			    }
            			    break;

            			default:
            			    goto loop1;
            	    }
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements

            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:46:16: ( media | pageRule | ruleset )+
            	int cnt2 = 0;
            	do 
            	{
            	    int alt2 = 4;
            	    alt2 = dfa2.Predict(input);
            	    switch (alt2) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:46:17: media
            			    {
            			    	PushFollow(FOLLOW_media_in_stylesheet150);
            			    	media2 = media();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, media2.Tree);

            			    }
            			    break;
            			case 2 :
            			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:46:25: pageRule
            			    {
            			    	PushFollow(FOLLOW_pageRule_in_stylesheet154);
            			    	pageRule3 = pageRule();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, pageRule3.Tree);

            			    }
            			    break;
            			case 3 :
            			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:46:36: ruleset
            			    {
            			    	PushFollow(FOLLOW_ruleset_in_stylesheet158);
            			    	ruleset4 = ruleset();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, ruleset4.Tree);

            			    }
            			    break;

            			default:
            			    if ( cnt2 >= 1 ) goto loop2;
            		            EarlyExitException eee2 =
            		                new EarlyExitException(2, input);
            		            throw eee2;
            	    }
            	    cnt2++;
            	} while (true);

            	loop2:
            		;	// Stops C# compiler whinging that label 'loop2' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "stylesheet"

    public class importRule_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "importRule"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:49:1: importRule : ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) );
    public csst3Parser.importRule_return importRule() // throws RecognitionException [1]
    {   
        csst3Parser.importRule_return retval = new csst3Parser.importRule_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken string_literal5 = null;
        IToken string_literal6 = null;
        IToken STRING7 = null;
        IToken char_literal8 = null;
        IToken string_literal9 = null;
        IToken string_literal10 = null;
        IToken char_literal12 = null;
        csst3Parser.function_return function11 = null;


        CommonTree string_literal5_tree=null;
        CommonTree string_literal6_tree=null;
        CommonTree STRING7_tree=null;
        CommonTree char_literal8_tree=null;
        CommonTree string_literal9_tree=null;
        CommonTree string_literal10_tree=null;
        CommonTree char_literal12_tree=null;
        RewriteRuleTokenStream stream_30 = new RewriteRuleTokenStream(adaptor,"token 30");
        RewriteRuleTokenStream stream_32 = new RewriteRuleTokenStream(adaptor,"token 32");
        RewriteRuleTokenStream stream_31 = new RewriteRuleTokenStream(adaptor,"token 31");
        RewriteRuleTokenStream stream_STRING = new RewriteRuleTokenStream(adaptor,"token STRING");
        RewriteRuleSubtreeStream stream_function = new RewriteRuleSubtreeStream(adaptor,"rule function");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:50:2: ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) )
            int alt5 = 2;
            int LA5_0 = input.LA(1);

            if ( (LA5_0 == 30) )
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
                        new NoViableAltException("", 5, 1, input);

                    throw nvae_d5s1;
                }
            }
            else if ( (LA5_0 == 31) )
            {
                int LA5_2 = input.LA(2);

                if ( (LA5_2 == STRING) )
                {
                    alt5 = 1;
                }
                else if ( (LA5_2 == IDENT) )
                {
                    alt5 = 2;
                }
                else 
                {
                    NoViableAltException nvae_d5s2 =
                        new NoViableAltException("", 5, 2, input);

                    throw nvae_d5s2;
                }
            }
            else 
            {
                NoViableAltException nvae_d5s0 =
                    new NoViableAltException("", 5, 0, input);

                throw nvae_d5s0;
            }
            switch (alt5) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:50:4: ( '@import' | '@include' ) STRING ';'
                    {
                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:50:4: ( '@import' | '@include' )
                    	int alt3 = 2;
                    	int LA3_0 = input.LA(1);

                    	if ( (LA3_0 == 30) )
                    	{
                    	    alt3 = 1;
                    	}
                    	else if ( (LA3_0 == 31) )
                    	{
                    	    alt3 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d3s0 =
                    	        new NoViableAltException("", 3, 0, input);

                    	    throw nvae_d3s0;
                    	}
                    	switch (alt3) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:50:5: '@import'
                    	        {
                    	        	string_literal5=(IToken)Match(input,30,FOLLOW_30_in_importRule172);  
                    	        	stream_30.Add(string_literal5);


                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:50:17: '@include'
                    	        {
                    	        	string_literal6=(IToken)Match(input,31,FOLLOW_31_in_importRule176);  
                    	        	stream_31.Add(string_literal6);


                    	        }
                    	        break;

                    	}

                    	STRING7=(IToken)Match(input,STRING,FOLLOW_STRING_in_importRule180);  
                    	stream_STRING.Add(STRING7);

                    	char_literal8=(IToken)Match(input,32,FOLLOW_32_in_importRule182);  
                    	stream_32.Add(char_literal8);



                    	// AST REWRITE
                    	// elements:          STRING
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 50:41: -> ^( IMPORT STRING )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:50:44: ^( IMPORT STRING )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(IMPORT, "IMPORT"), root_1);

                    	    adaptor.AddChild(root_1, stream_STRING.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:51:4: ( '@import' | '@include' ) function ';'
                    {
                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:51:4: ( '@import' | '@include' )
                    	int alt4 = 2;
                    	int LA4_0 = input.LA(1);

                    	if ( (LA4_0 == 30) )
                    	{
                    	    alt4 = 1;
                    	}
                    	else if ( (LA4_0 == 31) )
                    	{
                    	    alt4 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d4s0 =
                    	        new NoViableAltException("", 4, 0, input);

                    	    throw nvae_d4s0;
                    	}
                    	switch (alt4) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:51:5: '@import'
                    	        {
                    	        	string_literal9=(IToken)Match(input,30,FOLLOW_30_in_importRule198);  
                    	        	stream_30.Add(string_literal9);


                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:51:17: '@include'
                    	        {
                    	        	string_literal10=(IToken)Match(input,31,FOLLOW_31_in_importRule202);  
                    	        	stream_31.Add(string_literal10);


                    	        }
                    	        break;

                    	}

                    	PushFollow(FOLLOW_function_in_importRule206);
                    	function11 = function();
                    	state.followingStackPointer--;

                    	stream_function.Add(function11.Tree);
                    	char_literal12=(IToken)Match(input,32,FOLLOW_32_in_importRule208);  
                    	stream_32.Add(char_literal12);



                    	// AST REWRITE
                    	// elements:          function
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 51:43: -> ^( IMPORT function )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:51:46: ^( IMPORT function )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(IMPORT, "IMPORT"), root_1);

                    	    adaptor.AddChild(root_1, stream_function.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "importRule"

    public class media_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "media"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:54:1: media : '@media' IDENT '{' ( pageRule | ruleset )+ '}' -> ^( MEDIA IDENT ( pageRule )* ( ruleset )* ) ;
    public csst3Parser.media_return media() // throws RecognitionException [1]
    {   
        csst3Parser.media_return retval = new csst3Parser.media_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken string_literal13 = null;
        IToken IDENT14 = null;
        IToken char_literal15 = null;
        IToken char_literal18 = null;
        csst3Parser.pageRule_return pageRule16 = null;

        csst3Parser.ruleset_return ruleset17 = null;


        CommonTree string_literal13_tree=null;
        CommonTree IDENT14_tree=null;
        CommonTree char_literal15_tree=null;
        CommonTree char_literal18_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_35 = new RewriteRuleTokenStream(adaptor,"token 35");
        RewriteRuleTokenStream stream_33 = new RewriteRuleTokenStream(adaptor,"token 33");
        RewriteRuleTokenStream stream_34 = new RewriteRuleTokenStream(adaptor,"token 34");
        RewriteRuleSubtreeStream stream_ruleset = new RewriteRuleSubtreeStream(adaptor,"rule ruleset");
        RewriteRuleSubtreeStream stream_pageRule = new RewriteRuleSubtreeStream(adaptor,"rule pageRule");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:55:2: ( '@media' IDENT '{' ( pageRule | ruleset )+ '}' -> ^( MEDIA IDENT ( pageRule )* ( ruleset )* ) )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:55:4: '@media' IDENT '{' ( pageRule | ruleset )+ '}'
            {
            	string_literal13=(IToken)Match(input,33,FOLLOW_33_in_media229);  
            	stream_33.Add(string_literal13);

            	IDENT14=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_media231);  
            	stream_IDENT.Add(IDENT14);

            	char_literal15=(IToken)Match(input,34,FOLLOW_34_in_media233);  
            	stream_34.Add(char_literal15);

            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:55:23: ( pageRule | ruleset )+
            	int cnt6 = 0;
            	do 
            	{
            	    int alt6 = 3;
            	    alt6 = dfa6.Predict(input);
            	    switch (alt6) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:55:24: pageRule
            			    {
            			    	PushFollow(FOLLOW_pageRule_in_media236);
            			    	pageRule16 = pageRule();
            			    	state.followingStackPointer--;

            			    	stream_pageRule.Add(pageRule16.Tree);

            			    }
            			    break;
            			case 2 :
            			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:55:35: ruleset
            			    {
            			    	PushFollow(FOLLOW_ruleset_in_media240);
            			    	ruleset17 = ruleset();
            			    	state.followingStackPointer--;

            			    	stream_ruleset.Add(ruleset17.Tree);

            			    }
            			    break;

            			default:
            			    if ( cnt6 >= 1 ) goto loop6;
            		            EarlyExitException eee6 =
            		                new EarlyExitException(6, input);
            		            throw eee6;
            	    }
            	    cnt6++;
            	} while (true);

            	loop6:
            		;	// Stops C# compiler whinging that label 'loop6' has no statements

            	char_literal18=(IToken)Match(input,35,FOLLOW_35_in_media244);  
            	stream_35.Add(char_literal18);



            	// AST REWRITE
            	// elements:          pageRule, IDENT, ruleset
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 55:49: -> ^( MEDIA IDENT ( pageRule )* ( ruleset )* )
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:55:52: ^( MEDIA IDENT ( pageRule )* ( ruleset )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(MEDIA, "MEDIA"), root_1);

            	    adaptor.AddChild(root_1, stream_IDENT.NextNode());
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:55:67: ( pageRule )*
            	    while ( stream_pageRule.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_pageRule.NextTree());

            	    }
            	    stream_pageRule.Reset();
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:55:77: ( ruleset )*
            	    while ( stream_ruleset.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_ruleset.NextTree());

            	    }
            	    stream_ruleset.Reset();

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "media"

    public class pageRule_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "pageRule"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:58:1: pageRule : '@page' ( IDENT )? ( pseudo )? '{' ( properties )? ( region )* '}' -> ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* ) ;
    public csst3Parser.pageRule_return pageRule() // throws RecognitionException [1]
    {   
        csst3Parser.pageRule_return retval = new csst3Parser.pageRule_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken string_literal19 = null;
        IToken IDENT20 = null;
        IToken char_literal22 = null;
        IToken char_literal25 = null;
        csst3Parser.pseudo_return pseudo21 = null;

        csst3Parser.properties_return properties23 = null;

        csst3Parser.region_return region24 = null;


        CommonTree string_literal19_tree=null;
        CommonTree IDENT20_tree=null;
        CommonTree char_literal22_tree=null;
        CommonTree char_literal25_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_35 = new RewriteRuleTokenStream(adaptor,"token 35");
        RewriteRuleTokenStream stream_36 = new RewriteRuleTokenStream(adaptor,"token 36");
        RewriteRuleTokenStream stream_34 = new RewriteRuleTokenStream(adaptor,"token 34");
        RewriteRuleSubtreeStream stream_region = new RewriteRuleSubtreeStream(adaptor,"rule region");
        RewriteRuleSubtreeStream stream_pseudo = new RewriteRuleSubtreeStream(adaptor,"rule pseudo");
        RewriteRuleSubtreeStream stream_properties = new RewriteRuleSubtreeStream(adaptor,"rule properties");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:3: ( '@page' ( IDENT )? ( pseudo )? '{' ( properties )? ( region )* '}' -> ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* ) )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:5: '@page' ( IDENT )? ( pseudo )? '{' ( properties )? ( region )* '}'
            {
            	string_literal19=(IToken)Match(input,36,FOLLOW_36_in_pageRule272);  
            	stream_36.Add(string_literal19);

            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:13: ( IDENT )?
            	int alt7 = 2;
            	int LA7_0 = input.LA(1);

            	if ( (LA7_0 == IDENT) )
            	{
            	    alt7 = 1;
            	}
            	switch (alt7) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:13: IDENT
            	        {
            	        	IDENT20=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_pageRule274);  
            	        	stream_IDENT.Add(IDENT20);


            	        }
            	        break;

            	}

            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:20: ( pseudo )?
            	int alt8 = 2;
            	int LA8_0 = input.LA(1);

            	if ( ((LA8_0 >= 44 && LA8_0 <= 45)) )
            	{
            	    alt8 = 1;
            	}
            	switch (alt8) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:20: pseudo
            	        {
            	        	PushFollow(FOLLOW_pseudo_in_pageRule277);
            	        	pseudo21 = pseudo();
            	        	state.followingStackPointer--;

            	        	stream_pseudo.Add(pseudo21.Tree);

            	        }
            	        break;

            	}

            	char_literal22=(IToken)Match(input,34,FOLLOW_34_in_pageRule280);  
            	stream_34.Add(char_literal22);

            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:32: ( properties )?
            	int alt9 = 2;
            	int LA9_0 = input.LA(1);

            	if ( (LA9_0 == IDENT) )
            	{
            	    alt9 = 1;
            	}
            	switch (alt9) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:32: properties
            	        {
            	        	PushFollow(FOLLOW_properties_in_pageRule282);
            	        	properties23 = properties();
            	        	state.followingStackPointer--;

            	        	stream_properties.Add(properties23.Tree);

            	        }
            	        break;

            	}

            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:44: ( region )*
            	do 
            	{
            	    int alt10 = 2;
            	    int LA10_0 = input.LA(1);

            	    if ( (LA10_0 == 37) )
            	    {
            	        alt10 = 1;
            	    }


            	    switch (alt10) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:44: region
            			    {
            			    	PushFollow(FOLLOW_region_in_pageRule285);
            			    	region24 = region();
            			    	state.followingStackPointer--;

            			    	stream_region.Add(region24.Tree);

            			    }
            			    break;

            			default:
            			    goto loop10;
            	    }
            	} while (true);

            	loop10:
            		;	// Stops C# compiler whining that label 'loop10' has no statements

            	char_literal25=(IToken)Match(input,35,FOLLOW_35_in_pageRule288);  
            	stream_35.Add(char_literal25);



            	// AST REWRITE
            	// elements:          region, properties, IDENT, pseudo
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 59:56: -> ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* )
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:59: ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(PAGE, "PAGE"), root_1);

            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:67: ( IDENT )*
            	    while ( stream_IDENT.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_IDENT.NextNode());

            	    }
            	    stream_IDENT.Reset();
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:74: ( pseudo )*
            	    while ( stream_pseudo.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_pseudo.NextTree());

            	    }
            	    stream_pseudo.Reset();
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:82: ( properties )*
            	    while ( stream_properties.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_properties.NextTree());

            	    }
            	    stream_properties.Reset();
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:59:94: ( region )*
            	    while ( stream_region.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_region.NextTree());

            	    }
            	    stream_region.Reset();

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "pageRule"

    public class region_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "region"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:62:1: region : '@' IDENT '{' ( properties )? '}' -> ^( REGION IDENT ( properties )* ) ;
    public csst3Parser.region_return region() // throws RecognitionException [1]
    {   
        csst3Parser.region_return retval = new csst3Parser.region_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal26 = null;
        IToken IDENT27 = null;
        IToken char_literal28 = null;
        IToken char_literal30 = null;
        csst3Parser.properties_return properties29 = null;


        CommonTree char_literal26_tree=null;
        CommonTree IDENT27_tree=null;
        CommonTree char_literal28_tree=null;
        CommonTree char_literal30_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_35 = new RewriteRuleTokenStream(adaptor,"token 35");
        RewriteRuleTokenStream stream_34 = new RewriteRuleTokenStream(adaptor,"token 34");
        RewriteRuleTokenStream stream_37 = new RewriteRuleTokenStream(adaptor,"token 37");
        RewriteRuleSubtreeStream stream_properties = new RewriteRuleSubtreeStream(adaptor,"rule properties");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:63:2: ( '@' IDENT '{' ( properties )? '}' -> ^( REGION IDENT ( properties )* ) )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:63:4: '@' IDENT '{' ( properties )? '}'
            {
            	char_literal26=(IToken)Match(input,37,FOLLOW_37_in_region319);  
            	stream_37.Add(char_literal26);

            	IDENT27=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_region321);  
            	stream_IDENT.Add(IDENT27);

            	char_literal28=(IToken)Match(input,34,FOLLOW_34_in_region323);  
            	stream_34.Add(char_literal28);

            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:63:18: ( properties )?
            	int alt11 = 2;
            	int LA11_0 = input.LA(1);

            	if ( (LA11_0 == IDENT) )
            	{
            	    alt11 = 1;
            	}
            	switch (alt11) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:63:18: properties
            	        {
            	        	PushFollow(FOLLOW_properties_in_region325);
            	        	properties29 = properties();
            	        	state.followingStackPointer--;

            	        	stream_properties.Add(properties29.Tree);

            	        }
            	        break;

            	}

            	char_literal30=(IToken)Match(input,35,FOLLOW_35_in_region328);  
            	stream_35.Add(char_literal30);



            	// AST REWRITE
            	// elements:          IDENT, properties
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 63:34: -> ^( REGION IDENT ( properties )* )
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:63:37: ^( REGION IDENT ( properties )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(REGION, "REGION"), root_1);

            	    adaptor.AddChild(root_1, stream_IDENT.NextNode());
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:63:53: ( properties )*
            	    while ( stream_properties.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_properties.NextTree());

            	    }
            	    stream_properties.Reset();

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "region"

    public class ruleset_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "ruleset"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:66:1: ruleset : selectors '{' ( properties )? '}' -> ^( RULE selectors ( properties )* ) ;
    public csst3Parser.ruleset_return ruleset() // throws RecognitionException [1]
    {   
        csst3Parser.ruleset_return retval = new csst3Parser.ruleset_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal32 = null;
        IToken char_literal34 = null;
        csst3Parser.selectors_return selectors31 = null;

        csst3Parser.properties_return properties33 = null;


        CommonTree char_literal32_tree=null;
        CommonTree char_literal34_tree=null;
        RewriteRuleTokenStream stream_35 = new RewriteRuleTokenStream(adaptor,"token 35");
        RewriteRuleTokenStream stream_34 = new RewriteRuleTokenStream(adaptor,"token 34");
        RewriteRuleSubtreeStream stream_selectors = new RewriteRuleSubtreeStream(adaptor,"rule selectors");
        RewriteRuleSubtreeStream stream_properties = new RewriteRuleSubtreeStream(adaptor,"rule properties");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:67:3: ( selectors '{' ( properties )? '}' -> ^( RULE selectors ( properties )* ) )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:67:5: selectors '{' ( properties )? '}'
            {
            	PushFollow(FOLLOW_selectors_in_ruleset353);
            	selectors31 = selectors();
            	state.followingStackPointer--;

            	stream_selectors.Add(selectors31.Tree);
            	char_literal32=(IToken)Match(input,34,FOLLOW_34_in_ruleset355);  
            	stream_34.Add(char_literal32);

            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:67:19: ( properties )?
            	int alt12 = 2;
            	int LA12_0 = input.LA(1);

            	if ( (LA12_0 == IDENT) )
            	{
            	    alt12 = 1;
            	}
            	switch (alt12) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:67:19: properties
            	        {
            	        	PushFollow(FOLLOW_properties_in_ruleset357);
            	        	properties33 = properties();
            	        	state.followingStackPointer--;

            	        	stream_properties.Add(properties33.Tree);

            	        }
            	        break;

            	}

            	char_literal34=(IToken)Match(input,35,FOLLOW_35_in_ruleset360);  
            	stream_35.Add(char_literal34);



            	// AST REWRITE
            	// elements:          selectors, properties
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 67:35: -> ^( RULE selectors ( properties )* )
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:67:38: ^( RULE selectors ( properties )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(RULE, "RULE"), root_1);

            	    adaptor.AddChild(root_1, stream_selectors.NextTree());
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:67:56: ( properties )*
            	    while ( stream_properties.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_properties.NextTree());

            	    }
            	    stream_properties.Reset();

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "ruleset"

    public class selectors_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "selectors"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:70:1: selectors : selector ( ',' selector )* ;
    public csst3Parser.selectors_return selectors() // throws RecognitionException [1]
    {   
        csst3Parser.selectors_return retval = new csst3Parser.selectors_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal36 = null;
        csst3Parser.selector_return selector35 = null;

        csst3Parser.selector_return selector37 = null;


        CommonTree char_literal36_tree=null;

        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:71:2: ( selector ( ',' selector )* )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:71:4: selector ( ',' selector )*
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();

            	PushFollow(FOLLOW_selector_in_selectors385);
            	selector35 = selector();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, selector35.Tree);
            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:71:13: ( ',' selector )*
            	do 
            	{
            	    int alt13 = 2;
            	    int LA13_0 = input.LA(1);

            	    if ( (LA13_0 == 38) )
            	    {
            	        alt13 = 1;
            	    }


            	    switch (alt13) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:71:14: ',' selector
            			    {
            			    	char_literal36=(IToken)Match(input,38,FOLLOW_38_in_selectors388); 
            			    		char_literal36_tree = (CommonTree)adaptor.Create(char_literal36);
            			    		adaptor.AddChild(root_0, char_literal36_tree);

            			    	PushFollow(FOLLOW_selector_in_selectors390);
            			    	selector37 = selector();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, selector37.Tree);

            			    }
            			    break;

            			default:
            			    goto loop13;
            	    }
            	} while (true);

            	loop13:
            		;	// Stops C# compiler whining that label 'loop13' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "selectors"

    public class selector_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "selector"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:74:1: selector : ( elem ( selectorOperation )* ( pseudo )? -> elem ( selectorOperation )* ( pseudo )* | pseudo -> ANY pseudo );
    public csst3Parser.selector_return selector() // throws RecognitionException [1]
    {   
        csst3Parser.selector_return retval = new csst3Parser.selector_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        csst3Parser.elem_return elem38 = null;

        csst3Parser.selectorOperation_return selectorOperation39 = null;

        csst3Parser.pseudo_return pseudo40 = null;

        csst3Parser.pseudo_return pseudo41 = null;


        RewriteRuleSubtreeStream stream_elem = new RewriteRuleSubtreeStream(adaptor,"rule elem");
        RewriteRuleSubtreeStream stream_pseudo = new RewriteRuleSubtreeStream(adaptor,"rule pseudo");
        RewriteRuleSubtreeStream stream_selectorOperation = new RewriteRuleSubtreeStream(adaptor,"rule selectorOperation");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:75:2: ( elem ( selectorOperation )* ( pseudo )? -> elem ( selectorOperation )* ( pseudo )* | pseudo -> ANY pseudo )
            int alt16 = 2;
            int LA16_0 = input.LA(1);

            if ( ((LA16_0 >= IDENT && LA16_0 <= UNIT) || (LA16_0 >= 41 && LA16_0 <= 43)) )
            {
                alt16 = 1;
            }
            else if ( ((LA16_0 >= 44 && LA16_0 <= 45)) )
            {
                alt16 = 2;
            }
            else 
            {
                NoViableAltException nvae_d16s0 =
                    new NoViableAltException("", 16, 0, input);

                throw nvae_d16s0;
            }
            switch (alt16) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:75:4: elem ( selectorOperation )* ( pseudo )?
                    {
                    	PushFollow(FOLLOW_elem_in_selector404);
                    	elem38 = elem();
                    	state.followingStackPointer--;

                    	stream_elem.Add(elem38.Tree);
                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:75:9: ( selectorOperation )*
                    	do 
                    	{
                    	    int alt14 = 2;
                    	    alt14 = dfa14.Predict(input);
                    	    switch (alt14) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:75:9: selectorOperation
                    			    {
                    			    	PushFollow(FOLLOW_selectorOperation_in_selector406);
                    			    	selectorOperation39 = selectorOperation();
                    			    	state.followingStackPointer--;

                    			    	stream_selectorOperation.Add(selectorOperation39.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop14;
                    	    }
                    	} while (true);

                    	loop14:
                    		;	// Stops C# compiler whining that label 'loop14' has no statements

                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:75:28: ( pseudo )?
                    	int alt15 = 2;
                    	int LA15_0 = input.LA(1);

                    	if ( ((LA15_0 >= 44 && LA15_0 <= 45)) )
                    	{
                    	    alt15 = 1;
                    	}
                    	switch (alt15) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:75:28: pseudo
                    	        {
                    	        	PushFollow(FOLLOW_pseudo_in_selector409);
                    	        	pseudo40 = pseudo();
                    	        	state.followingStackPointer--;

                    	        	stream_pseudo.Add(pseudo40.Tree);

                    	        }
                    	        break;

                    	}



                    	// AST REWRITE
                    	// elements:          elem, selectorOperation, pseudo
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 75:36: -> elem ( selectorOperation )* ( pseudo )*
                    	{
                    	    adaptor.AddChild(root_0, stream_elem.NextTree());
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:75:45: ( selectorOperation )*
                    	    while ( stream_selectorOperation.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_0, stream_selectorOperation.NextTree());

                    	    }
                    	    stream_selectorOperation.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:75:64: ( pseudo )*
                    	    while ( stream_pseudo.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_0, stream_pseudo.NextTree());

                    	    }
                    	    stream_pseudo.Reset();

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:76:4: pseudo
                    {
                    	PushFollow(FOLLOW_pseudo_in_selector426);
                    	pseudo41 = pseudo();
                    	state.followingStackPointer--;

                    	stream_pseudo.Add(pseudo41.Tree);


                    	// AST REWRITE
                    	// elements:          pseudo
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 76:11: -> ANY pseudo
                    	{
                    	    adaptor.AddChild(root_0, (CommonTree)adaptor.Create(ANY, "ANY"));
                    	    adaptor.AddChild(root_0, stream_pseudo.NextTree());

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "selector"

    public class selectorOperation_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "selectorOperation"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:79:1: selectorOperation : ( selectop )? elem -> ( selectop )* elem ;
    public csst3Parser.selectorOperation_return selectorOperation() // throws RecognitionException [1]
    {   
        csst3Parser.selectorOperation_return retval = new csst3Parser.selectorOperation_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        csst3Parser.selectop_return selectop42 = null;

        csst3Parser.elem_return elem43 = null;


        RewriteRuleSubtreeStream stream_elem = new RewriteRuleSubtreeStream(adaptor,"rule elem");
        RewriteRuleSubtreeStream stream_selectop = new RewriteRuleSubtreeStream(adaptor,"rule selectop");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:80:2: ( ( selectop )? elem -> ( selectop )* elem )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:80:4: ( selectop )? elem
            {
            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:80:4: ( selectop )?
            	int alt17 = 2;
            	int LA17_0 = input.LA(1);

            	if ( ((LA17_0 >= 39 && LA17_0 <= 40)) )
            	{
            	    alt17 = 1;
            	}
            	switch (alt17) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:80:4: selectop
            	        {
            	        	PushFollow(FOLLOW_selectop_in_selectorOperation444);
            	        	selectop42 = selectop();
            	        	state.followingStackPointer--;

            	        	stream_selectop.Add(selectop42.Tree);

            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_elem_in_selectorOperation447);
            	elem43 = elem();
            	state.followingStackPointer--;

            	stream_elem.Add(elem43.Tree);


            	// AST REWRITE
            	// elements:          elem, selectop
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 80:19: -> ( selectop )* elem
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:80:22: ( selectop )*
            	    while ( stream_selectop.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_selectop.NextTree());

            	    }
            	    stream_selectop.Reset();
            	    adaptor.AddChild(root_0, stream_elem.NextTree());

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "selectorOperation"

    public class selectop_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "selectop"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:83:1: selectop : ( '>' -> PARENTOF | '+' -> PRECEDES );
    public csst3Parser.selectop_return selectop() // throws RecognitionException [1]
    {   
        csst3Parser.selectop_return retval = new csst3Parser.selectop_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal44 = null;
        IToken char_literal45 = null;

        CommonTree char_literal44_tree=null;
        CommonTree char_literal45_tree=null;
        RewriteRuleTokenStream stream_40 = new RewriteRuleTokenStream(adaptor,"token 40");
        RewriteRuleTokenStream stream_39 = new RewriteRuleTokenStream(adaptor,"token 39");

        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:84:2: ( '>' -> PARENTOF | '+' -> PRECEDES )
            int alt18 = 2;
            int LA18_0 = input.LA(1);

            if ( (LA18_0 == 39) )
            {
                alt18 = 1;
            }
            else if ( (LA18_0 == 40) )
            {
                alt18 = 2;
            }
            else 
            {
                NoViableAltException nvae_d18s0 =
                    new NoViableAltException("", 18, 0, input);

                throw nvae_d18s0;
            }
            switch (alt18) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:84:4: '>'
                    {
                    	char_literal44=(IToken)Match(input,39,FOLLOW_39_in_selectop465);  
                    	stream_39.Add(char_literal44);



                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 84:8: -> PARENTOF
                    	{
                    	    adaptor.AddChild(root_0, (CommonTree)adaptor.Create(PARENTOF, "PARENTOF"));

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:85:11: '+'
                    {
                    	char_literal45=(IToken)Match(input,40,FOLLOW_40_in_selectop481);  
                    	stream_40.Add(char_literal45);



                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 85:16: -> PRECEDES
                    	{
                    	    adaptor.AddChild(root_0, (CommonTree)adaptor.Create(PRECEDES, "PRECEDES"));

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "selectop"

    public class properties_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "properties"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:88:1: properties : declaration ( ';' ( declaration )? )* -> ( declaration )+ ;
    public csst3Parser.properties_return properties() // throws RecognitionException [1]
    {   
        csst3Parser.properties_return retval = new csst3Parser.properties_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal47 = null;
        csst3Parser.declaration_return declaration46 = null;

        csst3Parser.declaration_return declaration48 = null;


        CommonTree char_literal47_tree=null;
        RewriteRuleTokenStream stream_32 = new RewriteRuleTokenStream(adaptor,"token 32");
        RewriteRuleSubtreeStream stream_declaration = new RewriteRuleSubtreeStream(adaptor,"rule declaration");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:89:2: ( declaration ( ';' ( declaration )? )* -> ( declaration )+ )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:89:4: declaration ( ';' ( declaration )? )*
            {
            	PushFollow(FOLLOW_declaration_in_properties497);
            	declaration46 = declaration();
            	state.followingStackPointer--;

            	stream_declaration.Add(declaration46.Tree);
            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:89:16: ( ';' ( declaration )? )*
            	do 
            	{
            	    int alt20 = 2;
            	    int LA20_0 = input.LA(1);

            	    if ( (LA20_0 == 32) )
            	    {
            	        alt20 = 1;
            	    }


            	    switch (alt20) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:89:17: ';' ( declaration )?
            			    {
            			    	char_literal47=(IToken)Match(input,32,FOLLOW_32_in_properties500);  
            			    	stream_32.Add(char_literal47);

            			    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:89:21: ( declaration )?
            			    	int alt19 = 2;
            			    	int LA19_0 = input.LA(1);

            			    	if ( (LA19_0 == IDENT) )
            			    	{
            			    	    alt19 = 1;
            			    	}
            			    	switch (alt19) 
            			    	{
            			    	    case 1 :
            			    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:89:21: declaration
            			    	        {
            			    	        	PushFollow(FOLLOW_declaration_in_properties502);
            			    	        	declaration48 = declaration();
            			    	        	state.followingStackPointer--;

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
            		;	// Stops C# compiler whining that label 'loop20' has no statements



            	// AST REWRITE
            	// elements:          declaration
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 89:36: -> ( declaration )+
            	{
            	    if ( !(stream_declaration.HasNext()) ) {
            	        throw new RewriteEarlyExitException();
            	    }
            	    while ( stream_declaration.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_declaration.NextTree());

            	    }
            	    stream_declaration.Reset();

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "properties"

    public class elem_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "elem"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:92:1: elem : ( ( IDENT | UNIT ) ( attrib )* -> ^( TAG ( IDENT )* ( UNIT )* ( attrib )* ) | '#' ( IDENT | UNIT ) ( attrib )* -> ^( ID ( IDENT )* ( UNIT )* ( attrib )* ) | '.' ( IDENT | UNIT ) ( attrib )* -> ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* ) | '*' ( attrib )* -> ^( ANY ( attrib )* ) );
    public csst3Parser.elem_return elem() // throws RecognitionException [1]
    {   
        csst3Parser.elem_return retval = new csst3Parser.elem_return();
        retval.Start = input.LT(1);

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
        csst3Parser.attrib_return attrib51 = null;

        csst3Parser.attrib_return attrib55 = null;

        csst3Parser.attrib_return attrib59 = null;

        csst3Parser.attrib_return attrib61 = null;


        CommonTree IDENT49_tree=null;
        CommonTree UNIT50_tree=null;
        CommonTree char_literal52_tree=null;
        CommonTree IDENT53_tree=null;
        CommonTree UNIT54_tree=null;
        CommonTree char_literal56_tree=null;
        CommonTree IDENT57_tree=null;
        CommonTree UNIT58_tree=null;
        CommonTree char_literal60_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_UNIT = new RewriteRuleTokenStream(adaptor,"token UNIT");
        RewriteRuleTokenStream stream_43 = new RewriteRuleTokenStream(adaptor,"token 43");
        RewriteRuleTokenStream stream_42 = new RewriteRuleTokenStream(adaptor,"token 42");
        RewriteRuleTokenStream stream_41 = new RewriteRuleTokenStream(adaptor,"token 41");
        RewriteRuleSubtreeStream stream_attrib = new RewriteRuleSubtreeStream(adaptor,"rule attrib");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:93:2: ( ( IDENT | UNIT ) ( attrib )* -> ^( TAG ( IDENT )* ( UNIT )* ( attrib )* ) | '#' ( IDENT | UNIT ) ( attrib )* -> ^( ID ( IDENT )* ( UNIT )* ( attrib )* ) | '.' ( IDENT | UNIT ) ( attrib )* -> ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* ) | '*' ( attrib )* -> ^( ANY ( attrib )* ) )
            int alt28 = 4;
            switch ( input.LA(1) ) 
            {
            case IDENT:
            case UNIT:
            	{
                alt28 = 1;
                }
                break;
            case 41:
            	{
                alt28 = 2;
                }
                break;
            case 42:
            	{
                alt28 = 3;
                }
                break;
            case 43:
            	{
                alt28 = 4;
                }
                break;
            	default:
            	    NoViableAltException nvae_d28s0 =
            	        new NoViableAltException("", 28, 0, input);

            	    throw nvae_d28s0;
            }

            switch (alt28) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:93:8: ( IDENT | UNIT ) ( attrib )*
                    {
                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:93:8: ( IDENT | UNIT )
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
                    	        new NoViableAltException("", 21, 0, input);

                    	    throw nvae_d21s0;
                    	}
                    	switch (alt21) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:93:9: IDENT
                    	        {
                    	        	IDENT49=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_elem528);  
                    	        	stream_IDENT.Add(IDENT49);


                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:93:17: UNIT
                    	        {
                    	        	UNIT50=(IToken)Match(input,UNIT,FOLLOW_UNIT_in_elem532);  
                    	        	stream_UNIT.Add(UNIT50);


                    	        }
                    	        break;

                    	}

                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:93:23: ( attrib )*
                    	do 
                    	{
                    	    int alt22 = 2;
                    	    alt22 = dfa22.Predict(input);
                    	    switch (alt22) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:93:23: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem535);
                    			    	attrib51 = attrib();
                    			    	state.followingStackPointer--;

                    			    	stream_attrib.Add(attrib51.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop22;
                    	    }
                    	} while (true);

                    	loop22:
                    		;	// Stops C# compiler whining that label 'loop22' has no statements



                    	// AST REWRITE
                    	// elements:          UNIT, attrib, IDENT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 93:31: -> ^( TAG ( IDENT )* ( UNIT )* ( attrib )* )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:93:34: ^( TAG ( IDENT )* ( UNIT )* ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(TAG, "TAG"), root_1);

                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:93:41: ( IDENT )*
                    	    while ( stream_IDENT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_IDENT.NextNode());

                    	    }
                    	    stream_IDENT.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:93:48: ( UNIT )*
                    	    while ( stream_UNIT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_UNIT.NextNode());

                    	    }
                    	    stream_UNIT.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:93:54: ( attrib )*
                    	    while ( stream_attrib.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_attrib.NextTree());

                    	    }
                    	    stream_attrib.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:94:4: '#' ( IDENT | UNIT ) ( attrib )*
                    {
                    	char_literal52=(IToken)Match(input,41,FOLLOW_41_in_elem558);  
                    	stream_41.Add(char_literal52);

                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:94:8: ( IDENT | UNIT )
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
                    	        new NoViableAltException("", 23, 0, input);

                    	    throw nvae_d23s0;
                    	}
                    	switch (alt23) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:94:9: IDENT
                    	        {
                    	        	IDENT53=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_elem561);  
                    	        	stream_IDENT.Add(IDENT53);


                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:94:17: UNIT
                    	        {
                    	        	UNIT54=(IToken)Match(input,UNIT,FOLLOW_UNIT_in_elem565);  
                    	        	stream_UNIT.Add(UNIT54);


                    	        }
                    	        break;

                    	}

                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:94:23: ( attrib )*
                    	do 
                    	{
                    	    int alt24 = 2;
                    	    alt24 = dfa24.Predict(input);
                    	    switch (alt24) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:94:23: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem568);
                    			    	attrib55 = attrib();
                    			    	state.followingStackPointer--;

                    			    	stream_attrib.Add(attrib55.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop24;
                    	    }
                    	} while (true);

                    	loop24:
                    		;	// Stops C# compiler whining that label 'loop24' has no statements



                    	// AST REWRITE
                    	// elements:          attrib, UNIT, IDENT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 94:31: -> ^( ID ( IDENT )* ( UNIT )* ( attrib )* )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:94:34: ^( ID ( IDENT )* ( UNIT )* ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(ID, "ID"), root_1);

                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:94:40: ( IDENT )*
                    	    while ( stream_IDENT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_IDENT.NextNode());

                    	    }
                    	    stream_IDENT.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:94:47: ( UNIT )*
                    	    while ( stream_UNIT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_UNIT.NextNode());

                    	    }
                    	    stream_UNIT.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:94:53: ( attrib )*
                    	    while ( stream_attrib.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_attrib.NextTree());

                    	    }
                    	    stream_attrib.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 3 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:95:4: '.' ( IDENT | UNIT ) ( attrib )*
                    {
                    	char_literal56=(IToken)Match(input,42,FOLLOW_42_in_elem591);  
                    	stream_42.Add(char_literal56);

                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:95:8: ( IDENT | UNIT )
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
                    	        new NoViableAltException("", 25, 0, input);

                    	    throw nvae_d25s0;
                    	}
                    	switch (alt25) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:95:9: IDENT
                    	        {
                    	        	IDENT57=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_elem594);  
                    	        	stream_IDENT.Add(IDENT57);


                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:95:17: UNIT
                    	        {
                    	        	UNIT58=(IToken)Match(input,UNIT,FOLLOW_UNIT_in_elem598);  
                    	        	stream_UNIT.Add(UNIT58);


                    	        }
                    	        break;

                    	}

                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:95:23: ( attrib )*
                    	do 
                    	{
                    	    int alt26 = 2;
                    	    alt26 = dfa26.Predict(input);
                    	    switch (alt26) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:95:23: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem601);
                    			    	attrib59 = attrib();
                    			    	state.followingStackPointer--;

                    			    	stream_attrib.Add(attrib59.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop26;
                    	    }
                    	} while (true);

                    	loop26:
                    		;	// Stops C# compiler whining that label 'loop26' has no statements



                    	// AST REWRITE
                    	// elements:          attrib, UNIT, IDENT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 95:31: -> ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:95:34: ^( CLASS ( IDENT )* ( UNIT )* ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(CLASS, "CLASS"), root_1);

                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:95:43: ( IDENT )*
                    	    while ( stream_IDENT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_IDENT.NextNode());

                    	    }
                    	    stream_IDENT.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:95:50: ( UNIT )*
                    	    while ( stream_UNIT.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_UNIT.NextNode());

                    	    }
                    	    stream_UNIT.Reset();
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:95:56: ( attrib )*
                    	    while ( stream_attrib.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_attrib.NextTree());

                    	    }
                    	    stream_attrib.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 4 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:96:4: '*' ( attrib )*
                    {
                    	char_literal60=(IToken)Match(input,43,FOLLOW_43_in_elem624);  
                    	stream_43.Add(char_literal60);

                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:96:8: ( attrib )*
                    	do 
                    	{
                    	    int alt27 = 2;
                    	    alt27 = dfa27.Predict(input);
                    	    switch (alt27) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:96:8: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem626);
                    			    	attrib61 = attrib();
                    			    	state.followingStackPointer--;

                    			    	stream_attrib.Add(attrib61.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop27;
                    	    }
                    	} while (true);

                    	loop27:
                    		;	// Stops C# compiler whining that label 'loop27' has no statements



                    	// AST REWRITE
                    	// elements:          attrib
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 96:16: -> ^( ANY ( attrib )* )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:96:19: ^( ANY ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(ANY, "ANY"), root_1);

                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:96:26: ( attrib )*
                    	    while ( stream_attrib.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_attrib.NextTree());

                    	    }
                    	    stream_attrib.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "elem"

    public class pseudo_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "pseudo"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:99:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );
    public csst3Parser.pseudo_return pseudo() // throws RecognitionException [1]
    {   
        csst3Parser.pseudo_return retval = new csst3Parser.pseudo_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal62 = null;
        IToken string_literal63 = null;
        IToken IDENT64 = null;
        IToken char_literal65 = null;
        IToken string_literal66 = null;
        csst3Parser.function_return function67 = null;


        CommonTree char_literal62_tree=null;
        CommonTree string_literal63_tree=null;
        CommonTree IDENT64_tree=null;
        CommonTree char_literal65_tree=null;
        CommonTree string_literal66_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_45 = new RewriteRuleTokenStream(adaptor,"token 45");
        RewriteRuleTokenStream stream_44 = new RewriteRuleTokenStream(adaptor,"token 44");
        RewriteRuleSubtreeStream stream_function = new RewriteRuleSubtreeStream(adaptor,"rule function");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:100:2: ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) )
            int alt31 = 2;
            alt31 = dfa31.Predict(input);
            switch (alt31) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:100:4: ( ':' | '::' ) IDENT
                    {
                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:100:4: ( ':' | '::' )
                    	int alt29 = 2;
                    	int LA29_0 = input.LA(1);

                    	if ( (LA29_0 == 44) )
                    	{
                    	    alt29 = 1;
                    	}
                    	else if ( (LA29_0 == 45) )
                    	{
                    	    alt29 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d29s0 =
                    	        new NoViableAltException("", 29, 0, input);

                    	    throw nvae_d29s0;
                    	}
                    	switch (alt29) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:100:5: ':'
                    	        {
                    	        	char_literal62=(IToken)Match(input,44,FOLLOW_44_in_pseudo650);  
                    	        	stream_44.Add(char_literal62);


                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:100:9: '::'
                    	        {
                    	        	string_literal63=(IToken)Match(input,45,FOLLOW_45_in_pseudo652);  
                    	        	stream_45.Add(string_literal63);


                    	        }
                    	        break;

                    	}

                    	IDENT64=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_pseudo655);  
                    	stream_IDENT.Add(IDENT64);



                    	// AST REWRITE
                    	// elements:          IDENT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 100:21: -> ^( PSEUDO IDENT )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:100:24: ^( PSEUDO IDENT )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(PSEUDO, "PSEUDO"), root_1);

                    	    adaptor.AddChild(root_1, stream_IDENT.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:101:4: ( ':' | '::' ) function
                    {
                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:101:4: ( ':' | '::' )
                    	int alt30 = 2;
                    	int LA30_0 = input.LA(1);

                    	if ( (LA30_0 == 44) )
                    	{
                    	    alt30 = 1;
                    	}
                    	else if ( (LA30_0 == 45) )
                    	{
                    	    alt30 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d30s0 =
                    	        new NoViableAltException("", 30, 0, input);

                    	    throw nvae_d30s0;
                    	}
                    	switch (alt30) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:101:5: ':'
                    	        {
                    	        	char_literal65=(IToken)Match(input,44,FOLLOW_44_in_pseudo671);  
                    	        	stream_44.Add(char_literal65);


                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:101:9: '::'
                    	        {
                    	        	string_literal66=(IToken)Match(input,45,FOLLOW_45_in_pseudo673);  
                    	        	stream_45.Add(string_literal66);


                    	        }
                    	        break;

                    	}

                    	PushFollow(FOLLOW_function_in_pseudo676);
                    	function67 = function();
                    	state.followingStackPointer--;

                    	stream_function.Add(function67.Tree);


                    	// AST REWRITE
                    	// elements:          function
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 101:24: -> ^( PSEUDO function )
                    	{
                    	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:101:27: ^( PSEUDO function )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(PSEUDO, "PSEUDO"), root_1);

                    	    adaptor.AddChild(root_1, stream_function.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "pseudo"

    public class attrib_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "attrib"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:104:1: attrib : '[' IDENT ( attribRelate ( STRING | IDENT ) )? ']' -> ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? ) ;
    public csst3Parser.attrib_return attrib() // throws RecognitionException [1]
    {   
        csst3Parser.attrib_return retval = new csst3Parser.attrib_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal68 = null;
        IToken IDENT69 = null;
        IToken STRING71 = null;
        IToken IDENT72 = null;
        IToken char_literal73 = null;
        csst3Parser.attribRelate_return attribRelate70 = null;


        CommonTree char_literal68_tree=null;
        CommonTree IDENT69_tree=null;
        CommonTree STRING71_tree=null;
        CommonTree IDENT72_tree=null;
        CommonTree char_literal73_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_47 = new RewriteRuleTokenStream(adaptor,"token 47");
        RewriteRuleTokenStream stream_46 = new RewriteRuleTokenStream(adaptor,"token 46");
        RewriteRuleTokenStream stream_STRING = new RewriteRuleTokenStream(adaptor,"token STRING");
        RewriteRuleSubtreeStream stream_attribRelate = new RewriteRuleSubtreeStream(adaptor,"rule attribRelate");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:105:2: ( '[' IDENT ( attribRelate ( STRING | IDENT ) )? ']' -> ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? ) )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:105:4: '[' IDENT ( attribRelate ( STRING | IDENT ) )? ']'
            {
            	char_literal68=(IToken)Match(input,46,FOLLOW_46_in_attrib697);  
            	stream_46.Add(char_literal68);

            	IDENT69=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_attrib699);  
            	stream_IDENT.Add(IDENT69);

            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:105:14: ( attribRelate ( STRING | IDENT ) )?
            	int alt33 = 2;
            	int LA33_0 = input.LA(1);

            	if ( ((LA33_0 >= 48 && LA33_0 <= 50)) )
            	{
            	    alt33 = 1;
            	}
            	switch (alt33) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:105:15: attribRelate ( STRING | IDENT )
            	        {
            	        	PushFollow(FOLLOW_attribRelate_in_attrib702);
            	        	attribRelate70 = attribRelate();
            	        	state.followingStackPointer--;

            	        	stream_attribRelate.Add(attribRelate70.Tree);
            	        	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:105:28: ( STRING | IDENT )
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
            	        	        new NoViableAltException("", 32, 0, input);

            	        	    throw nvae_d32s0;
            	        	}
            	        	switch (alt32) 
            	        	{
            	        	    case 1 :
            	        	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:105:29: STRING
            	        	        {
            	        	        	STRING71=(IToken)Match(input,STRING,FOLLOW_STRING_in_attrib705);  
            	        	        	stream_STRING.Add(STRING71);


            	        	        }
            	        	        break;
            	        	    case 2 :
            	        	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:105:38: IDENT
            	        	        {
            	        	        	IDENT72=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_attrib709);  
            	        	        	stream_IDENT.Add(IDENT72);


            	        	        }
            	        	        break;

            	        	}


            	        }
            	        break;

            	}

            	char_literal73=(IToken)Match(input,47,FOLLOW_47_in_attrib714);  
            	stream_47.Add(char_literal73);



            	// AST REWRITE
            	// elements:          attribRelate, IDENT, IDENT, STRING
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 105:51: -> ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? )
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:105:54: ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(ATTRIB, "ATTRIB"), root_1);

            	    adaptor.AddChild(root_1, stream_IDENT.NextNode());
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:105:70: ( attribRelate ( STRING )* ( IDENT )* )?
            	    if ( stream_attribRelate.HasNext() || stream_IDENT.HasNext() || stream_STRING.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_attribRelate.NextTree());
            	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:105:84: ( STRING )*
            	        while ( stream_STRING.HasNext() )
            	        {
            	            adaptor.AddChild(root_1, stream_STRING.NextNode());

            	        }
            	        stream_STRING.Reset();
            	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:105:92: ( IDENT )*
            	        while ( stream_IDENT.HasNext() )
            	        {
            	            adaptor.AddChild(root_1, stream_IDENT.NextNode());

            	        }
            	        stream_IDENT.Reset();

            	    }
            	    stream_attribRelate.Reset();
            	    stream_IDENT.Reset();
            	    stream_STRING.Reset();

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "attrib"

    public class attribRelate_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "attribRelate"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:108:1: attribRelate : ( '=' -> ATTRIBEQUAL | '~=' -> HASVALUE | '|=' -> BEGINSWITH );
    public csst3Parser.attribRelate_return attribRelate() // throws RecognitionException [1]
    {   
        csst3Parser.attribRelate_return retval = new csst3Parser.attribRelate_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal74 = null;
        IToken string_literal75 = null;
        IToken string_literal76 = null;

        CommonTree char_literal74_tree=null;
        CommonTree string_literal75_tree=null;
        CommonTree string_literal76_tree=null;
        RewriteRuleTokenStream stream_49 = new RewriteRuleTokenStream(adaptor,"token 49");
        RewriteRuleTokenStream stream_48 = new RewriteRuleTokenStream(adaptor,"token 48");
        RewriteRuleTokenStream stream_50 = new RewriteRuleTokenStream(adaptor,"token 50");

        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:109:2: ( '=' -> ATTRIBEQUAL | '~=' -> HASVALUE | '|=' -> BEGINSWITH )
            int alt34 = 3;
            switch ( input.LA(1) ) 
            {
            case 48:
            	{
                alt34 = 1;
                }
                break;
            case 49:
            	{
                alt34 = 2;
                }
                break;
            case 50:
            	{
                alt34 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d34s0 =
            	        new NoViableAltException("", 34, 0, input);

            	    throw nvae_d34s0;
            }

            switch (alt34) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:109:4: '='
                    {
                    	char_literal74=(IToken)Match(input,48,FOLLOW_48_in_attribRelate747);  
                    	stream_48.Add(char_literal74);



                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 109:9: -> ATTRIBEQUAL
                    	{
                    	    adaptor.AddChild(root_0, (CommonTree)adaptor.Create(ATTRIBEQUAL, "ATTRIBEQUAL"));

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:110:4: '~='
                    {
                    	string_literal75=(IToken)Match(input,49,FOLLOW_49_in_attribRelate757);  
                    	stream_49.Add(string_literal75);



                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 110:9: -> HASVALUE
                    	{
                    	    adaptor.AddChild(root_0, (CommonTree)adaptor.Create(HASVALUE, "HASVALUE"));

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 3 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:111:4: '|='
                    {
                    	string_literal76=(IToken)Match(input,50,FOLLOW_50_in_attribRelate766);  
                    	stream_50.Add(string_literal76);



                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 111:9: -> BEGINSWITH
                    	{
                    	    adaptor.AddChild(root_0, (CommonTree)adaptor.Create(BEGINSWITH, "BEGINSWITH"));

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "attribRelate"

    public class declaration_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "declaration"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:114:1: declaration : IDENT ':' args -> ^( PROPERTY IDENT args ) ;
    public csst3Parser.declaration_return declaration() // throws RecognitionException [1]
    {   
        csst3Parser.declaration_return retval = new csst3Parser.declaration_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken IDENT77 = null;
        IToken char_literal78 = null;
        csst3Parser.args_return args79 = null;


        CommonTree IDENT77_tree=null;
        CommonTree char_literal78_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_44 = new RewriteRuleTokenStream(adaptor,"token 44");
        RewriteRuleSubtreeStream stream_args = new RewriteRuleSubtreeStream(adaptor,"rule args");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:115:2: ( IDENT ':' args -> ^( PROPERTY IDENT args ) )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:115:4: IDENT ':' args
            {
            	IDENT77=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_declaration784);  
            	stream_IDENT.Add(IDENT77);

            	char_literal78=(IToken)Match(input,44,FOLLOW_44_in_declaration786);  
            	stream_44.Add(char_literal78);

            	PushFollow(FOLLOW_args_in_declaration788);
            	args79 = args();
            	state.followingStackPointer--;

            	stream_args.Add(args79.Tree);


            	// AST REWRITE
            	// elements:          args, IDENT
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 115:19: -> ^( PROPERTY IDENT args )
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:115:22: ^( PROPERTY IDENT args )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(PROPERTY, "PROPERTY"), root_1);

            	    adaptor.AddChild(root_1, stream_IDENT.NextNode());
            	    adaptor.AddChild(root_1, stream_args.NextTree());

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "declaration"

    public class args_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "args"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:118:1: args : expr ( ( ',' )? expr )* -> ( expr )* ;
    public csst3Parser.args_return args() // throws RecognitionException [1]
    {   
        csst3Parser.args_return retval = new csst3Parser.args_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal81 = null;
        csst3Parser.expr_return expr80 = null;

        csst3Parser.expr_return expr82 = null;


        CommonTree char_literal81_tree=null;
        RewriteRuleTokenStream stream_38 = new RewriteRuleTokenStream(adaptor,"token 38");
        RewriteRuleSubtreeStream stream_expr = new RewriteRuleSubtreeStream(adaptor,"rule expr");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:119:2: ( expr ( ( ',' )? expr )* -> ( expr )* )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:119:4: expr ( ( ',' )? expr )*
            {
            	PushFollow(FOLLOW_expr_in_args811);
            	expr80 = expr();
            	state.followingStackPointer--;

            	stream_expr.Add(expr80.Tree);
            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:119:9: ( ( ',' )? expr )*
            	do 
            	{
            	    int alt36 = 2;
            	    alt36 = dfa36.Predict(input);
            	    switch (alt36) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:119:10: ( ',' )? expr
            			    {
            			    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:119:10: ( ',' )?
            			    	int alt35 = 2;
            			    	int LA35_0 = input.LA(1);

            			    	if ( (LA35_0 == 38) )
            			    	{
            			    	    alt35 = 1;
            			    	}
            			    	switch (alt35) 
            			    	{
            			    	    case 1 :
            			    	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:119:10: ','
            			    	        {
            			    	        	char_literal81=(IToken)Match(input,38,FOLLOW_38_in_args814);  
            			    	        	stream_38.Add(char_literal81);


            			    	        }
            			    	        break;

            			    	}

            			    	PushFollow(FOLLOW_expr_in_args817);
            			    	expr82 = expr();
            			    	state.followingStackPointer--;

            			    	stream_expr.Add(expr82.Tree);

            			    }
            			    break;

            			default:
            			    goto loop36;
            	    }
            	} while (true);

            	loop36:
            		;	// Stops C# compiler whining that label 'loop36' has no statements



            	// AST REWRITE
            	// elements:          expr
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 119:22: -> ( expr )*
            	{
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:119:25: ( expr )*
            	    while ( stream_expr.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_expr.NextTree());

            	    }
            	    stream_expr.Reset();

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "args"

    public class expr_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "expr"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:122:1: expr : ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function );
    public csst3Parser.expr_return expr() // throws RecognitionException [1]
    {   
        csst3Parser.expr_return retval = new csst3Parser.expr_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken NUM83 = null;
        IToken IDENT85 = null;
        IToken COLOR86 = null;
        IToken STRING87 = null;
        csst3Parser.unit_return unit84 = null;

        csst3Parser.function_return function88 = null;


        CommonTree NUM83_tree=null;
        CommonTree IDENT85_tree=null;
        CommonTree COLOR86_tree=null;
        CommonTree STRING87_tree=null;

        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:123:2: ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function )
            int alt38 = 5;
            alt38 = dfa38.Predict(input);
            switch (alt38) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:123:4: ( NUM ( unit )? )
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:123:4: ( NUM ( unit )? )
                    	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:123:5: NUM ( unit )?
                    	{
                    		NUM83=(IToken)Match(input,NUM,FOLLOW_NUM_in_expr836); 
                    			NUM83_tree = (CommonTree)adaptor.Create(NUM83);
                    			adaptor.AddChild(root_0, NUM83_tree);

                    		// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:123:9: ( unit )?
                    		int alt37 = 2;
                    		alt37 = dfa37.Predict(input);
                    		switch (alt37) 
                    		{
                    		    case 1 :
                    		        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:123:9: unit
                    		        {
                    		        	PushFollow(FOLLOW_unit_in_expr838);
                    		        	unit84 = unit();
                    		        	state.followingStackPointer--;

                    		        	adaptor.AddChild(root_0, unit84.Tree);

                    		        }
                    		        break;

                    		}


                    	}


                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:124:4: IDENT
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	IDENT85=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_expr845); 
                    		IDENT85_tree = (CommonTree)adaptor.Create(IDENT85);
                    		adaptor.AddChild(root_0, IDENT85_tree);


                    }
                    break;
                case 3 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:125:4: COLOR
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	COLOR86=(IToken)Match(input,COLOR,FOLLOW_COLOR_in_expr850); 
                    		COLOR86_tree = (CommonTree)adaptor.Create(COLOR86);
                    		adaptor.AddChild(root_0, COLOR86_tree);


                    }
                    break;
                case 4 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:126:4: STRING
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	STRING87=(IToken)Match(input,STRING,FOLLOW_STRING_in_expr855); 
                    		STRING87_tree = (CommonTree)adaptor.Create(STRING87);
                    		adaptor.AddChild(root_0, STRING87_tree);


                    }
                    break;
                case 5 :
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:127:4: function
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_function_in_expr860);
                    	function88 = function();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, function88.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "expr"

    public class unit_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "unit"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:130:1: unit : ( '%' | UNIT ) ;
    public csst3Parser.unit_return unit() // throws RecognitionException [1]
    {   
        csst3Parser.unit_return retval = new csst3Parser.unit_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken set89 = null;

        CommonTree set89_tree=null;

        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:131:2: ( ( '%' | UNIT ) )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:131:4: ( '%' | UNIT )
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();

            	set89 = (IToken)input.LT(1);
            	if ( input.LA(1) == UNIT || input.LA(1) == 51 ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (CommonTree)adaptor.Create(set89));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "unit"

    public class function_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "function"
    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:134:1: function : IDENT '(' ( args )? ')' -> IDENT '(' ( args )* ')' ;
    public csst3Parser.function_return function() // throws RecognitionException [1]
    {   
        csst3Parser.function_return retval = new csst3Parser.function_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken IDENT90 = null;
        IToken char_literal91 = null;
        IToken char_literal93 = null;
        csst3Parser.args_return args92 = null;


        CommonTree IDENT90_tree=null;
        CommonTree char_literal91_tree=null;
        CommonTree char_literal93_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_52 = new RewriteRuleTokenStream(adaptor,"token 52");
        RewriteRuleTokenStream stream_53 = new RewriteRuleTokenStream(adaptor,"token 53");
        RewriteRuleSubtreeStream stream_args = new RewriteRuleSubtreeStream(adaptor,"rule args");
        try 
    	{
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:135:2: ( IDENT '(' ( args )? ')' -> IDENT '(' ( args )* ')' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:135:4: IDENT '(' ( args )? ')'
            {
            	IDENT90=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_function888);  
            	stream_IDENT.Add(IDENT90);

            	char_literal91=(IToken)Match(input,52,FOLLOW_52_in_function890);  
            	stream_52.Add(char_literal91);

            	// C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:135:14: ( args )?
            	int alt39 = 2;
            	int LA39_0 = input.LA(1);

            	if ( ((LA39_0 >= STRING && LA39_0 <= IDENT) || (LA39_0 >= NUM && LA39_0 <= COLOR)) )
            	{
            	    alt39 = 1;
            	}
            	switch (alt39) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:135:14: args
            	        {
            	        	PushFollow(FOLLOW_args_in_function892);
            	        	args92 = args();
            	        	state.followingStackPointer--;

            	        	stream_args.Add(args92.Tree);

            	        }
            	        break;

            	}

            	char_literal93=(IToken)Match(input,53,FOLLOW_53_in_function895);  
            	stream_53.Add(char_literal93);



            	// AST REWRITE
            	// elements:          52, 53, args, IDENT
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 135:24: -> IDENT '(' ( args )* ')'
            	{
            	    adaptor.AddChild(root_0, stream_IDENT.NextNode());
            	    adaptor.AddChild(root_0, stream_52.NextNode());
            	    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:135:37: ( args )*
            	    while ( stream_args.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_args.NextTree());

            	    }
            	    stream_args.Reset();
            	    adaptor.AddChild(root_0, stream_53.NextNode());

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "function"

    // Delegated rules


   	protected DFA1 dfa1;
   	protected DFA2 dfa2;
   	protected DFA6 dfa6;
   	protected DFA14 dfa14;
   	protected DFA22 dfa22;
   	protected DFA24 dfa24;
   	protected DFA26 dfa26;
   	protected DFA27 dfa27;
   	protected DFA31 dfa31;
   	protected DFA36 dfa36;
   	protected DFA38 dfa38;
   	protected DFA37 dfa37;
	private void InitializeCyclicDFAs()
	{
    	this.dfa1 = new DFA1(this);
    	this.dfa2 = new DFA2(this);
    	this.dfa6 = new DFA6(this);
    	this.dfa14 = new DFA14(this);
    	this.dfa22 = new DFA22(this);
    	this.dfa24 = new DFA24(this);
    	this.dfa26 = new DFA26(this);
    	this.dfa27 = new DFA27(this);
    	this.dfa31 = new DFA31(this);
    	this.dfa36 = new DFA36(this);
    	this.dfa38 = new DFA38(this);
    	this.dfa37 = new DFA37(this);












	}

    const string DFA1_eotS =
        "\x0c\uffff";
    const string DFA1_eofS =
        "\x0c\uffff";
    const string DFA1_minS =
        "\x01\x17\x0b\uffff";
    const string DFA1_maxS =
        "\x01\x2d\x0b\uffff";
    const string DFA1_acceptS =
        "\x01\uffff\x01\x02\x08\uffff\x01\x01\x01\uffff";
    const string DFA1_specialS =
        "\x0c\uffff}>";
    static readonly string[] DFA1_transitionS = {
            "\x02\x01\x05\uffff\x02\x0a\x01\uffff\x01\x01\x02\uffff\x01"+
            "\x01\x04\uffff\x05\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA1_eot = DFA.UnpackEncodedString(DFA1_eotS);
    static readonly short[] DFA1_eof = DFA.UnpackEncodedString(DFA1_eofS);
    static readonly char[] DFA1_min = DFA.UnpackEncodedStringToUnsignedChars(DFA1_minS);
    static readonly char[] DFA1_max = DFA.UnpackEncodedStringToUnsignedChars(DFA1_maxS);
    static readonly short[] DFA1_accept = DFA.UnpackEncodedString(DFA1_acceptS);
    static readonly short[] DFA1_special = DFA.UnpackEncodedString(DFA1_specialS);
    static readonly short[][] DFA1_transition = DFA.UnpackEncodedStringArray(DFA1_transitionS);

    protected class DFA1 : DFA
    {
        public DFA1(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 1;
            this.eot = DFA1_eot;
            this.eof = DFA1_eof;
            this.min = DFA1_min;
            this.max = DFA1_max;
            this.accept = DFA1_accept;
            this.special = DFA1_special;
            this.transition = DFA1_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 46:4: ( importRule )*"; }
        }

    }

    const string DFA2_eotS =
        "\x0b\uffff";
    const string DFA2_eofS =
        "\x01\x01\x0a\uffff";
    const string DFA2_minS =
        "\x01\x17\x0a\uffff";
    const string DFA2_maxS =
        "\x01\x2d\x0a\uffff";
    const string DFA2_acceptS =
        "\x01\uffff\x01\x04\x01\x01\x01\x02\x01\x03\x06\uffff";
    const string DFA2_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA2_transitionS = {
            "\x02\x04\x08\uffff\x01\x02\x02\uffff\x01\x03\x04\uffff\x05"+
            "\x04",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA2_eot = DFA.UnpackEncodedString(DFA2_eotS);
    static readonly short[] DFA2_eof = DFA.UnpackEncodedString(DFA2_eofS);
    static readonly char[] DFA2_min = DFA.UnpackEncodedStringToUnsignedChars(DFA2_minS);
    static readonly char[] DFA2_max = DFA.UnpackEncodedStringToUnsignedChars(DFA2_maxS);
    static readonly short[] DFA2_accept = DFA.UnpackEncodedString(DFA2_acceptS);
    static readonly short[] DFA2_special = DFA.UnpackEncodedString(DFA2_specialS);
    static readonly short[][] DFA2_transition = DFA.UnpackEncodedStringArray(DFA2_transitionS);

    protected class DFA2 : DFA
    {
        public DFA2(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 2;
            this.eot = DFA2_eot;
            this.eof = DFA2_eof;
            this.min = DFA2_min;
            this.max = DFA2_max;
            this.accept = DFA2_accept;
            this.special = DFA2_special;
            this.transition = DFA2_transition;

        }

        override public string Description
        {
            get { return "()+ loopback of 46:16: ( media | pageRule | ruleset )+"; }
        }

    }

    const string DFA6_eotS =
        "\x0a\uffff";
    const string DFA6_eofS =
        "\x0a\uffff";
    const string DFA6_minS =
        "\x01\x17\x09\uffff";
    const string DFA6_maxS =
        "\x01\x2d\x09\uffff";
    const string DFA6_acceptS =
        "\x01\uffff\x01\x03\x01\x01\x01\x02\x06\uffff";
    const string DFA6_specialS =
        "\x0a\uffff}>";
    static readonly string[] DFA6_transitionS = {
            "\x02\x03\x0a\uffff\x01\x01\x01\x02\x04\uffff\x05\x03",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA6_eot = DFA.UnpackEncodedString(DFA6_eotS);
    static readonly short[] DFA6_eof = DFA.UnpackEncodedString(DFA6_eofS);
    static readonly char[] DFA6_min = DFA.UnpackEncodedStringToUnsignedChars(DFA6_minS);
    static readonly char[] DFA6_max = DFA.UnpackEncodedStringToUnsignedChars(DFA6_maxS);
    static readonly short[] DFA6_accept = DFA.UnpackEncodedString(DFA6_acceptS);
    static readonly short[] DFA6_special = DFA.UnpackEncodedString(DFA6_specialS);
    static readonly short[][] DFA6_transition = DFA.UnpackEncodedStringArray(DFA6_transitionS);

    protected class DFA6 : DFA
    {
        public DFA6(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 6;
            this.eot = DFA6_eot;
            this.eof = DFA6_eof;
            this.min = DFA6_min;
            this.max = DFA6_max;
            this.accept = DFA6_accept;
            this.special = DFA6_special;
            this.transition = DFA6_transition;

        }

        override public string Description
        {
            get { return "()+ loopback of 55:23: ( pageRule | ruleset )+"; }
        }

    }

    const string DFA14_eotS =
        "\x0c\uffff";
    const string DFA14_eofS =
        "\x0c\uffff";
    const string DFA14_minS =
        "\x01\x17\x0b\uffff";
    const string DFA14_maxS =
        "\x01\x2d\x0b\uffff";
    const string DFA14_acceptS =
        "\x01\uffff\x01\x02\x03\uffff\x01\x01\x06\uffff";
    const string DFA14_specialS =
        "\x0c\uffff}>";
    static readonly string[] DFA14_transitionS = {
            "\x02\x05\x09\uffff\x01\x01\x03\uffff\x01\x01\x05\x05\x02\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA14_eot = DFA.UnpackEncodedString(DFA14_eotS);
    static readonly short[] DFA14_eof = DFA.UnpackEncodedString(DFA14_eofS);
    static readonly char[] DFA14_min = DFA.UnpackEncodedStringToUnsignedChars(DFA14_minS);
    static readonly char[] DFA14_max = DFA.UnpackEncodedStringToUnsignedChars(DFA14_maxS);
    static readonly short[] DFA14_accept = DFA.UnpackEncodedString(DFA14_acceptS);
    static readonly short[] DFA14_special = DFA.UnpackEncodedString(DFA14_specialS);
    static readonly short[][] DFA14_transition = DFA.UnpackEncodedStringArray(DFA14_transitionS);

    protected class DFA14 : DFA
    {
        public DFA14(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 14;
            this.eot = DFA14_eot;
            this.eof = DFA14_eof;
            this.min = DFA14_min;
            this.max = DFA14_max;
            this.accept = DFA14_accept;
            this.special = DFA14_special;
            this.transition = DFA14_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 75:9: ( selectorOperation )*"; }
        }

    }

    const string DFA22_eotS =
        "\x0d\uffff";
    const string DFA22_eofS =
        "\x0d\uffff";
    const string DFA22_minS =
        "\x01\x17\x0c\uffff";
    const string DFA22_maxS =
        "\x01\x2e\x0c\uffff";
    const string DFA22_acceptS =
        "\x01\uffff\x01\x02\x0a\uffff\x01\x01";
    const string DFA22_specialS =
        "\x0d\uffff}>";
    static readonly string[] DFA22_transitionS = {
            "\x02\x01\x09\uffff\x01\x01\x03\uffff\x08\x01\x01\x0c",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA22_eot = DFA.UnpackEncodedString(DFA22_eotS);
    static readonly short[] DFA22_eof = DFA.UnpackEncodedString(DFA22_eofS);
    static readonly char[] DFA22_min = DFA.UnpackEncodedStringToUnsignedChars(DFA22_minS);
    static readonly char[] DFA22_max = DFA.UnpackEncodedStringToUnsignedChars(DFA22_maxS);
    static readonly short[] DFA22_accept = DFA.UnpackEncodedString(DFA22_acceptS);
    static readonly short[] DFA22_special = DFA.UnpackEncodedString(DFA22_specialS);
    static readonly short[][] DFA22_transition = DFA.UnpackEncodedStringArray(DFA22_transitionS);

    protected class DFA22 : DFA
    {
        public DFA22(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 22;
            this.eot = DFA22_eot;
            this.eof = DFA22_eof;
            this.min = DFA22_min;
            this.max = DFA22_max;
            this.accept = DFA22_accept;
            this.special = DFA22_special;
            this.transition = DFA22_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 93:23: ( attrib )*"; }
        }

    }

    const string DFA24_eotS =
        "\x0d\uffff";
    const string DFA24_eofS =
        "\x0d\uffff";
    const string DFA24_minS =
        "\x01\x17\x0c\uffff";
    const string DFA24_maxS =
        "\x01\x2e\x0c\uffff";
    const string DFA24_acceptS =
        "\x01\uffff\x01\x02\x0a\uffff\x01\x01";
    const string DFA24_specialS =
        "\x0d\uffff}>";
    static readonly string[] DFA24_transitionS = {
            "\x02\x01\x09\uffff\x01\x01\x03\uffff\x08\x01\x01\x0c",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA24_eot = DFA.UnpackEncodedString(DFA24_eotS);
    static readonly short[] DFA24_eof = DFA.UnpackEncodedString(DFA24_eofS);
    static readonly char[] DFA24_min = DFA.UnpackEncodedStringToUnsignedChars(DFA24_minS);
    static readonly char[] DFA24_max = DFA.UnpackEncodedStringToUnsignedChars(DFA24_maxS);
    static readonly short[] DFA24_accept = DFA.UnpackEncodedString(DFA24_acceptS);
    static readonly short[] DFA24_special = DFA.UnpackEncodedString(DFA24_specialS);
    static readonly short[][] DFA24_transition = DFA.UnpackEncodedStringArray(DFA24_transitionS);

    protected class DFA24 : DFA
    {
        public DFA24(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 24;
            this.eot = DFA24_eot;
            this.eof = DFA24_eof;
            this.min = DFA24_min;
            this.max = DFA24_max;
            this.accept = DFA24_accept;
            this.special = DFA24_special;
            this.transition = DFA24_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 94:23: ( attrib )*"; }
        }

    }

    const string DFA26_eotS =
        "\x0d\uffff";
    const string DFA26_eofS =
        "\x0d\uffff";
    const string DFA26_minS =
        "\x01\x17\x0c\uffff";
    const string DFA26_maxS =
        "\x01\x2e\x0c\uffff";
    const string DFA26_acceptS =
        "\x01\uffff\x01\x02\x0a\uffff\x01\x01";
    const string DFA26_specialS =
        "\x0d\uffff}>";
    static readonly string[] DFA26_transitionS = {
            "\x02\x01\x09\uffff\x01\x01\x03\uffff\x08\x01\x01\x0c",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA26_eot = DFA.UnpackEncodedString(DFA26_eotS);
    static readonly short[] DFA26_eof = DFA.UnpackEncodedString(DFA26_eofS);
    static readonly char[] DFA26_min = DFA.UnpackEncodedStringToUnsignedChars(DFA26_minS);
    static readonly char[] DFA26_max = DFA.UnpackEncodedStringToUnsignedChars(DFA26_maxS);
    static readonly short[] DFA26_accept = DFA.UnpackEncodedString(DFA26_acceptS);
    static readonly short[] DFA26_special = DFA.UnpackEncodedString(DFA26_specialS);
    static readonly short[][] DFA26_transition = DFA.UnpackEncodedStringArray(DFA26_transitionS);

    protected class DFA26 : DFA
    {
        public DFA26(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 26;
            this.eot = DFA26_eot;
            this.eof = DFA26_eof;
            this.min = DFA26_min;
            this.max = DFA26_max;
            this.accept = DFA26_accept;
            this.special = DFA26_special;
            this.transition = DFA26_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 95:23: ( attrib )*"; }
        }

    }

    const string DFA27_eotS =
        "\x0d\uffff";
    const string DFA27_eofS =
        "\x0d\uffff";
    const string DFA27_minS =
        "\x01\x17\x0c\uffff";
    const string DFA27_maxS =
        "\x01\x2e\x0c\uffff";
    const string DFA27_acceptS =
        "\x01\uffff\x01\x02\x0a\uffff\x01\x01";
    const string DFA27_specialS =
        "\x0d\uffff}>";
    static readonly string[] DFA27_transitionS = {
            "\x02\x01\x09\uffff\x01\x01\x03\uffff\x08\x01\x01\x0c",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA27_eot = DFA.UnpackEncodedString(DFA27_eotS);
    static readonly short[] DFA27_eof = DFA.UnpackEncodedString(DFA27_eofS);
    static readonly char[] DFA27_min = DFA.UnpackEncodedStringToUnsignedChars(DFA27_minS);
    static readonly char[] DFA27_max = DFA.UnpackEncodedStringToUnsignedChars(DFA27_maxS);
    static readonly short[] DFA27_accept = DFA.UnpackEncodedString(DFA27_acceptS);
    static readonly short[] DFA27_special = DFA.UnpackEncodedString(DFA27_specialS);
    static readonly short[][] DFA27_transition = DFA.UnpackEncodedStringArray(DFA27_transitionS);

    protected class DFA27 : DFA
    {
        public DFA27(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 27;
            this.eot = DFA27_eot;
            this.eof = DFA27_eof;
            this.min = DFA27_min;
            this.max = DFA27_max;
            this.accept = DFA27_accept;
            this.special = DFA27_special;
            this.transition = DFA27_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 96:8: ( attrib )*"; }
        }

    }

    const string DFA31_eotS =
        "\x0b\uffff";
    const string DFA31_eofS =
        "\x0b\uffff";
    const string DFA31_minS =
        "\x01\x2c\x02\x17\x02\x22\x06\uffff";
    const string DFA31_maxS =
        "\x01\x2d\x02\x17\x02\x34\x06\uffff";
    const string DFA31_acceptS =
        "\x05\uffff\x01\x02\x01\x01\x04\uffff";
    const string DFA31_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA31_transitionS = {
            "\x01\x01\x01\x02",
            "\x01\x03",
            "\x01\x04",
            "\x01\x06\x03\uffff\x01\x06\x0d\uffff\x01\x05",
            "\x01\x06\x03\uffff\x01\x06\x0d\uffff\x01\x05",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA31_eot = DFA.UnpackEncodedString(DFA31_eotS);
    static readonly short[] DFA31_eof = DFA.UnpackEncodedString(DFA31_eofS);
    static readonly char[] DFA31_min = DFA.UnpackEncodedStringToUnsignedChars(DFA31_minS);
    static readonly char[] DFA31_max = DFA.UnpackEncodedStringToUnsignedChars(DFA31_maxS);
    static readonly short[] DFA31_accept = DFA.UnpackEncodedString(DFA31_acceptS);
    static readonly short[] DFA31_special = DFA.UnpackEncodedString(DFA31_specialS);
    static readonly short[][] DFA31_transition = DFA.UnpackEncodedStringArray(DFA31_transitionS);

    protected class DFA31 : DFA
    {
        public DFA31(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 31;
            this.eot = DFA31_eot;
            this.eof = DFA31_eof;
            this.min = DFA31_min;
            this.max = DFA31_max;
            this.accept = DFA31_accept;
            this.special = DFA31_special;
            this.transition = DFA31_transition;

        }

        override public string Description
        {
            get { return "99:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );"; }
        }

    }

    const string DFA36_eotS =
        "\x0a\uffff";
    const string DFA36_eofS =
        "\x0a\uffff";
    const string DFA36_minS =
        "\x01\x16\x09\uffff";
    const string DFA36_maxS =
        "\x01\x35\x09\uffff";
    const string DFA36_acceptS =
        "\x01\uffff\x01\x02\x03\uffff\x01\x01\x04\uffff";
    const string DFA36_specialS =
        "\x0a\uffff}>";
    static readonly string[] DFA36_transitionS = {
            "\x02\x05\x01\uffff\x02\x05\x05\uffff\x01\x01\x02\uffff\x01"+
            "\x01\x01\uffff\x01\x01\x01\x05\x0e\uffff\x01\x01",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA36_eot = DFA.UnpackEncodedString(DFA36_eotS);
    static readonly short[] DFA36_eof = DFA.UnpackEncodedString(DFA36_eofS);
    static readonly char[] DFA36_min = DFA.UnpackEncodedStringToUnsignedChars(DFA36_minS);
    static readonly char[] DFA36_max = DFA.UnpackEncodedStringToUnsignedChars(DFA36_maxS);
    static readonly short[] DFA36_accept = DFA.UnpackEncodedString(DFA36_acceptS);
    static readonly short[] DFA36_special = DFA.UnpackEncodedString(DFA36_specialS);
    static readonly short[][] DFA36_transition = DFA.UnpackEncodedStringArray(DFA36_transitionS);

    protected class DFA36 : DFA
    {
        public DFA36(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 36;
            this.eot = DFA36_eot;
            this.eof = DFA36_eof;
            this.min = DFA36_min;
            this.max = DFA36_max;
            this.accept = DFA36_accept;
            this.special = DFA36_special;
            this.transition = DFA36_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 119:9: ( ( ',' )? expr )*"; }
        }

    }

    const string DFA38_eotS =
        "\x0f\uffff";
    const string DFA38_eofS =
        "\x0f\uffff";
    const string DFA38_minS =
        "\x01\x16\x01\uffff\x01\x16\x0c\uffff";
    const string DFA38_maxS =
        "\x01\x1a\x01\uffff\x01\x35\x0c\uffff";
    const string DFA38_acceptS =
        "\x01\uffff\x01\x01\x01\uffff\x01\x03\x01\x04\x01\x05\x01\x02\x08"+
        "\uffff";
    const string DFA38_specialS =
        "\x0f\uffff}>";
    static readonly string[] DFA38_transitionS = {
            "\x01\x04\x01\x02\x01\uffff\x01\x01\x01\x03",
            "",
            "\x02\x06\x01\uffff\x02\x06\x05\uffff\x01\x06\x02\uffff\x01"+
            "\x06\x01\uffff\x02\x06\x0d\uffff\x01\x05\x01\x06",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA38_eot = DFA.UnpackEncodedString(DFA38_eotS);
    static readonly short[] DFA38_eof = DFA.UnpackEncodedString(DFA38_eofS);
    static readonly char[] DFA38_min = DFA.UnpackEncodedStringToUnsignedChars(DFA38_minS);
    static readonly char[] DFA38_max = DFA.UnpackEncodedStringToUnsignedChars(DFA38_maxS);
    static readonly short[] DFA38_accept = DFA.UnpackEncodedString(DFA38_acceptS);
    static readonly short[] DFA38_special = DFA.UnpackEncodedString(DFA38_specialS);
    static readonly short[][] DFA38_transition = DFA.UnpackEncodedStringArray(DFA38_transitionS);

    protected class DFA38 : DFA
    {
        public DFA38(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 38;
            this.eot = DFA38_eot;
            this.eof = DFA38_eof;
            this.min = DFA38_min;
            this.max = DFA38_max;
            this.accept = DFA38_accept;
            this.special = DFA38_special;
            this.transition = DFA38_transition;

        }

        override public string Description
        {
            get { return "122:1: expr : ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function );"; }
        }

    }

    const string DFA37_eotS =
        "\x0b\uffff";
    const string DFA37_eofS =
        "\x0b\uffff";
    const string DFA37_minS =
        "\x01\x16\x0a\uffff";
    const string DFA37_maxS =
        "\x01\x35\x0a\uffff";
    const string DFA37_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x08\uffff";
    const string DFA37_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA37_transitionS = {
            "\x02\x02\x01\x01\x02\x02\x05\uffff\x01\x02\x02\uffff\x01\x02"+
            "\x01\uffff\x02\x02\x0c\uffff\x01\x01\x01\uffff\x01\x02",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA37_eot = DFA.UnpackEncodedString(DFA37_eotS);
    static readonly short[] DFA37_eof = DFA.UnpackEncodedString(DFA37_eofS);
    static readonly char[] DFA37_min = DFA.UnpackEncodedStringToUnsignedChars(DFA37_minS);
    static readonly char[] DFA37_max = DFA.UnpackEncodedStringToUnsignedChars(DFA37_maxS);
    static readonly short[] DFA37_accept = DFA.UnpackEncodedString(DFA37_acceptS);
    static readonly short[] DFA37_special = DFA.UnpackEncodedString(DFA37_specialS);
    static readonly short[][] DFA37_transition = DFA.UnpackEncodedStringArray(DFA37_transitionS);

    protected class DFA37 : DFA
    {
        public DFA37(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 37;
            this.eot = DFA37_eot;
            this.eof = DFA37_eof;
            this.min = DFA37_min;
            this.max = DFA37_max;
            this.accept = DFA37_accept;
            this.special = DFA37_special;
            this.transition = DFA37_transition;

        }

        override public string Description
        {
            get { return "123:9: ( unit )?"; }
        }

    }

 

    public static readonly BitSet FOLLOW_importRule_in_stylesheet146 = new BitSet(new ulong[]{0x00003E12C1800000UL});
    public static readonly BitSet FOLLOW_media_in_stylesheet150 = new BitSet(new ulong[]{0x00003E1201800002UL});
    public static readonly BitSet FOLLOW_pageRule_in_stylesheet154 = new BitSet(new ulong[]{0x00003E1201800002UL});
    public static readonly BitSet FOLLOW_ruleset_in_stylesheet158 = new BitSet(new ulong[]{0x00003E1201800002UL});
    public static readonly BitSet FOLLOW_30_in_importRule172 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_31_in_importRule176 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_STRING_in_importRule180 = new BitSet(new ulong[]{0x0000000100000000UL});
    public static readonly BitSet FOLLOW_32_in_importRule182 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_30_in_importRule198 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_31_in_importRule202 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_function_in_importRule206 = new BitSet(new ulong[]{0x0000000100000000UL});
    public static readonly BitSet FOLLOW_32_in_importRule208 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_33_in_media229 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_media231 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_34_in_media233 = new BitSet(new ulong[]{0x00003E1201800000UL});
    public static readonly BitSet FOLLOW_pageRule_in_media236 = new BitSet(new ulong[]{0x00003E1A01800000UL});
    public static readonly BitSet FOLLOW_ruleset_in_media240 = new BitSet(new ulong[]{0x00003E1A01800000UL});
    public static readonly BitSet FOLLOW_35_in_media244 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_36_in_pageRule272 = new BitSet(new ulong[]{0x00003E1601800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_pageRule274 = new BitSet(new ulong[]{0x00003E1601800000UL});
    public static readonly BitSet FOLLOW_pseudo_in_pageRule277 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_34_in_pageRule280 = new BitSet(new ulong[]{0x0000002800800000UL});
    public static readonly BitSet FOLLOW_properties_in_pageRule282 = new BitSet(new ulong[]{0x0000002800000000UL});
    public static readonly BitSet FOLLOW_region_in_pageRule285 = new BitSet(new ulong[]{0x0000002800000000UL});
    public static readonly BitSet FOLLOW_35_in_pageRule288 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_37_in_region319 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_region321 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_34_in_region323 = new BitSet(new ulong[]{0x0000000800800000UL});
    public static readonly BitSet FOLLOW_properties_in_region325 = new BitSet(new ulong[]{0x0000000800000000UL});
    public static readonly BitSet FOLLOW_35_in_region328 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selectors_in_ruleset353 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_34_in_ruleset355 = new BitSet(new ulong[]{0x0000000800800000UL});
    public static readonly BitSet FOLLOW_properties_in_ruleset357 = new BitSet(new ulong[]{0x0000000800000000UL});
    public static readonly BitSet FOLLOW_35_in_ruleset360 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selector_in_selectors385 = new BitSet(new ulong[]{0x0000004000000002UL});
    public static readonly BitSet FOLLOW_38_in_selectors388 = new BitSet(new ulong[]{0x00003E1201800000UL});
    public static readonly BitSet FOLLOW_selector_in_selectors390 = new BitSet(new ulong[]{0x0000004000000002UL});
    public static readonly BitSet FOLLOW_elem_in_selector404 = new BitSet(new ulong[]{0x00003F9201800002UL});
    public static readonly BitSet FOLLOW_selectorOperation_in_selector406 = new BitSet(new ulong[]{0x00003F9201800002UL});
    public static readonly BitSet FOLLOW_pseudo_in_selector409 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_pseudo_in_selector426 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selectop_in_selectorOperation444 = new BitSet(new ulong[]{0x00000E0001800000UL});
    public static readonly BitSet FOLLOW_elem_in_selectorOperation447 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_39_in_selectop465 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_40_in_selectop481 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_declaration_in_properties497 = new BitSet(new ulong[]{0x0000000100000002UL});
    public static readonly BitSet FOLLOW_32_in_properties500 = new BitSet(new ulong[]{0x0000000100800002UL});
    public static readonly BitSet FOLLOW_declaration_in_properties502 = new BitSet(new ulong[]{0x0000000100000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_elem528 = new BitSet(new ulong[]{0x0000400000000002UL});
    public static readonly BitSet FOLLOW_UNIT_in_elem532 = new BitSet(new ulong[]{0x0000400000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem535 = new BitSet(new ulong[]{0x0000400000000002UL});
    public static readonly BitSet FOLLOW_41_in_elem558 = new BitSet(new ulong[]{0x0000000001800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_elem561 = new BitSet(new ulong[]{0x0000400000000002UL});
    public static readonly BitSet FOLLOW_UNIT_in_elem565 = new BitSet(new ulong[]{0x0000400000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem568 = new BitSet(new ulong[]{0x0000400000000002UL});
    public static readonly BitSet FOLLOW_42_in_elem591 = new BitSet(new ulong[]{0x0000000001800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_elem594 = new BitSet(new ulong[]{0x0000400000000002UL});
    public static readonly BitSet FOLLOW_UNIT_in_elem598 = new BitSet(new ulong[]{0x0000400000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem601 = new BitSet(new ulong[]{0x0000400000000002UL});
    public static readonly BitSet FOLLOW_43_in_elem624 = new BitSet(new ulong[]{0x0000400000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem626 = new BitSet(new ulong[]{0x0000400000000002UL});
    public static readonly BitSet FOLLOW_44_in_pseudo650 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_45_in_pseudo652 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_pseudo655 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_44_in_pseudo671 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_45_in_pseudo673 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_function_in_pseudo676 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_46_in_attrib697 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_attrib699 = new BitSet(new ulong[]{0x0007800000000000UL});
    public static readonly BitSet FOLLOW_attribRelate_in_attrib702 = new BitSet(new ulong[]{0x0000000000C00000UL});
    public static readonly BitSet FOLLOW_STRING_in_attrib705 = new BitSet(new ulong[]{0x0000800000000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_attrib709 = new BitSet(new ulong[]{0x0000800000000000UL});
    public static readonly BitSet FOLLOW_47_in_attrib714 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_48_in_attribRelate747 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_49_in_attribRelate757 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_50_in_attribRelate766 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_declaration784 = new BitSet(new ulong[]{0x0000100000000000UL});
    public static readonly BitSet FOLLOW_44_in_declaration786 = new BitSet(new ulong[]{0x0000000006C00000UL});
    public static readonly BitSet FOLLOW_args_in_declaration788 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expr_in_args811 = new BitSet(new ulong[]{0x0000004006C00002UL});
    public static readonly BitSet FOLLOW_38_in_args814 = new BitSet(new ulong[]{0x0000000006C00000UL});
    public static readonly BitSet FOLLOW_expr_in_args817 = new BitSet(new ulong[]{0x0000004006C00002UL});
    public static readonly BitSet FOLLOW_NUM_in_expr836 = new BitSet(new ulong[]{0x0008000001000002UL});
    public static readonly BitSet FOLLOW_unit_in_expr838 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_expr845 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_COLOR_in_expr850 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STRING_in_expr855 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_expr860 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_unit871 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_function888 = new BitSet(new ulong[]{0x0010000000000000UL});
    public static readonly BitSet FOLLOW_52_in_function890 = new BitSet(new ulong[]{0x0020000006C00000UL});
    public static readonly BitSet FOLLOW_args_in_function892 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_function895 = new BitSet(new ulong[]{0x0000000000000002UL});

}
