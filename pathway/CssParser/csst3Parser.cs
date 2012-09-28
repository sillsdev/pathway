// $ANTLR 3.2 Sep 23, 2009 12:02:23 C:\\git\\pathway\\pathway\\CssParser\\csst3.g 2012-09-26 17:16:56


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
		"'px'", 
		"'cm'", 
		"'mm'", 
		"'in'", 
		"'pt'", 
		"'pc'", 
		"'em'", 
		"'ex'", 
		"'deg'", 
		"'rad'", 
		"'grad'", 
		"'ms'", 
		"'s'", 
		"'hz'", 
		"'khz'", 
		"'('", 
		"')'"
    };

    public const int FUNCTION = 17;
    public const int T__66 = 66;
    public const int CLASS = 21;
    public const int ATTRIB = 9;
    public const int T__67 = 67;
    public const int T__29 = 29;
    public const int T__64 = 64;
    public const int T__65 = 65;
    public const int T__62 = 62;
    public const int T__63 = 63;
    public const int HASVALUE = 13;
    public const int PSEUDO = 15;
    public const int MEDIA = 5;
    public const int ID = 20;
    public const int T__61 = 61;
    public const int ATTRIBEQUAL = 12;
    public const int EOF = -1;
    public const int T__60 = 60;
    public const int COLOR = 25;
    public const int REGION = 7;
    public const int T__55 = 55;
    public const int T__56 = 56;
    public const int T__57 = 57;
    public const int T__58 = 58;
    public const int IMPORT = 4;
    public const int T__51 = 51;
    public const int T__52 = 52;
    public const int T__53 = 53;
    public const int T__54 = 54;
    public const int T__59 = 59;
    public const int IDENT = 23;
    public const int COMMENT = 27;
    public const int T__50 = 50;
    public const int T__42 = 42;
    public const int T__43 = 43;
    public const int T__40 = 40;
    public const int T__41 = 41;
    public const int T__46 = 46;
    public const int T__47 = 47;
    public const int RULE = 8;
    public const int T__44 = 44;
    public const int BEGINSWITH = 14;
    public const int PARENTOF = 10;
    public const int T__45 = 45;
    public const int T__48 = 48;
    public const int T__49 = 49;
    public const int PRECEDES = 11;
    public const int NUM = 24;
    public const int TAG = 19;
    public const int T__30 = 30;
    public const int T__31 = 31;
    public const int T__32 = 32;
    public const int WS = 28;
    public const int ANY = 18;
    public const int PAGE = 6;
    public const int T__33 = 33;
    public const int T__34 = 34;
    public const int T__35 = 35;
    public const int T__36 = 36;
    public const int PROPERTY = 16;
    public const int T__37 = 37;
    public const int T__38 = 38;
    public const int T__39 = 39;
    public const int SL_COMMENT = 26;
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
		get { return "C:\\git\\pathway\\pathway\\CssParser\\csst3.g"; }
    }


    private System.Collections.Generic.List<String> errors = new System.Collections.Generic.List<String>();
    override public void DisplayRecognitionError(String[] tokenNames,
                                        RecognitionException e) {
        String hdr = GetErrorHeader(e);
        String msg = GetErrorMessage(e, tokenNames);
        errors.Add(hdr + " " + msg);
    }
    public System.Collections.Generic.List<String> GetErrors() {
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:42:1: stylesheet : ( importRule )* ( media | pageRule | ruleset )+ ;
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
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:43:2: ( ( importRule )* ( media | pageRule | ruleset )+ )
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:43:4: ( importRule )* ( media | pageRule | ruleset )+
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();

            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:43:4: ( importRule )*
            	do 
            	{
            	    int alt1 = 2;
            	    alt1 = dfa1.Predict(input);
            	    switch (alt1) 
            		{
            			case 1 :
            			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:43:4: importRule
            			    {
            			    	PushFollow(FOLLOW_importRule_in_stylesheet129);
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

            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:43:16: ( media | pageRule | ruleset )+
            	int cnt2 = 0;
            	do 
            	{
            	    int alt2 = 4;
            	    alt2 = dfa2.Predict(input);
            	    switch (alt2) 
            		{
            			case 1 :
            			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:43:17: media
            			    {
            			    	PushFollow(FOLLOW_media_in_stylesheet133);
            			    	media2 = media();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, media2.Tree);

            			    }
            			    break;
            			case 2 :
            			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:43:25: pageRule
            			    {
            			    	PushFollow(FOLLOW_pageRule_in_stylesheet137);
            			    	pageRule3 = pageRule();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, pageRule3.Tree);

            			    }
            			    break;
            			case 3 :
            			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:43:36: ruleset
            			    {
            			    	PushFollow(FOLLOW_ruleset_in_stylesheet141);
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
            		;	// Stops C# compiler whining that label 'loop2' has no statements


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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:46:1: importRule : ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) );
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
        RewriteRuleTokenStream stream_31 = new RewriteRuleTokenStream(adaptor,"token 31");
        RewriteRuleTokenStream stream_STRING = new RewriteRuleTokenStream(adaptor,"token STRING");
        RewriteRuleTokenStream stream_29 = new RewriteRuleTokenStream(adaptor,"token 29");
        RewriteRuleSubtreeStream stream_function = new RewriteRuleSubtreeStream(adaptor,"rule function");
        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:47:2: ( ( '@import' | '@include' ) STRING ';' -> ^( IMPORT STRING ) | ( '@import' | '@include' ) function ';' -> ^( IMPORT function ) )
            int alt5 = 2;
            int LA5_0 = input.LA(1);

            if ( (LA5_0 == 29) )
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
            else if ( (LA5_0 == 30) )
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
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:47:4: ( '@import' | '@include' ) STRING ';'
                    {
                    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:47:4: ( '@import' | '@include' )
                    	int alt3 = 2;
                    	int LA3_0 = input.LA(1);

                    	if ( (LA3_0 == 29) )
                    	{
                    	    alt3 = 1;
                    	}
                    	else if ( (LA3_0 == 30) )
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
                    	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:47:5: '@import'
                    	        {
                    	        	string_literal5=(IToken)Match(input,29,FOLLOW_29_in_importRule155);  
                    	        	stream_29.Add(string_literal5);


                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:47:17: '@include'
                    	        {
                    	        	string_literal6=(IToken)Match(input,30,FOLLOW_30_in_importRule159);  
                    	        	stream_30.Add(string_literal6);


                    	        }
                    	        break;

                    	}

                    	STRING7=(IToken)Match(input,STRING,FOLLOW_STRING_in_importRule163);  
                    	stream_STRING.Add(STRING7);

                    	char_literal8=(IToken)Match(input,31,FOLLOW_31_in_importRule165);  
                    	stream_31.Add(char_literal8);



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
                    	// 47:41: -> ^( IMPORT STRING )
                    	{
                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:47:44: ^( IMPORT STRING )
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
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:48:4: ( '@import' | '@include' ) function ';'
                    {
                    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:48:4: ( '@import' | '@include' )
                    	int alt4 = 2;
                    	int LA4_0 = input.LA(1);

                    	if ( (LA4_0 == 29) )
                    	{
                    	    alt4 = 1;
                    	}
                    	else if ( (LA4_0 == 30) )
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
                    	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:48:5: '@import'
                    	        {
                    	        	string_literal9=(IToken)Match(input,29,FOLLOW_29_in_importRule181);  
                    	        	stream_29.Add(string_literal9);


                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:48:17: '@include'
                    	        {
                    	        	string_literal10=(IToken)Match(input,30,FOLLOW_30_in_importRule185);  
                    	        	stream_30.Add(string_literal10);


                    	        }
                    	        break;

                    	}

                    	PushFollow(FOLLOW_function_in_importRule189);
                    	function11 = function();
                    	state.followingStackPointer--;

                    	stream_function.Add(function11.Tree);
                    	char_literal12=(IToken)Match(input,31,FOLLOW_31_in_importRule191);  
                    	stream_31.Add(char_literal12);



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
                    	// 48:43: -> ^( IMPORT function )
                    	{
                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:48:46: ^( IMPORT function )
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:51:1: media : '@media' IDENT '{' ( pageRule | ruleset )+ '}' -> ^( MEDIA IDENT ( pageRule )* ( ruleset )* ) ;
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
        RewriteRuleTokenStream stream_32 = new RewriteRuleTokenStream(adaptor,"token 32");
        RewriteRuleTokenStream stream_33 = new RewriteRuleTokenStream(adaptor,"token 33");
        RewriteRuleTokenStream stream_34 = new RewriteRuleTokenStream(adaptor,"token 34");
        RewriteRuleSubtreeStream stream_ruleset = new RewriteRuleSubtreeStream(adaptor,"rule ruleset");
        RewriteRuleSubtreeStream stream_pageRule = new RewriteRuleSubtreeStream(adaptor,"rule pageRule");
        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:52:2: ( '@media' IDENT '{' ( pageRule | ruleset )+ '}' -> ^( MEDIA IDENT ( pageRule )* ( ruleset )* ) )
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:52:4: '@media' IDENT '{' ( pageRule | ruleset )+ '}'
            {
            	string_literal13=(IToken)Match(input,32,FOLLOW_32_in_media212);  
            	stream_32.Add(string_literal13);

            	IDENT14=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_media214);  
            	stream_IDENT.Add(IDENT14);

            	char_literal15=(IToken)Match(input,33,FOLLOW_33_in_media216);  
            	stream_33.Add(char_literal15);

            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:52:23: ( pageRule | ruleset )+
            	int cnt6 = 0;
            	do 
            	{
            	    int alt6 = 3;
            	    int LA6_0 = input.LA(1);

            	    if ( (LA6_0 == 35) )
            	    {
            	        alt6 = 1;
            	    }
            	    else if ( (LA6_0 == IDENT || (LA6_0 >= 40 && LA6_0 <= 44)) )
            	    {
            	        alt6 = 2;
            	    }


            	    switch (alt6) 
            		{
            			case 1 :
            			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:52:24: pageRule
            			    {
            			    	PushFollow(FOLLOW_pageRule_in_media219);
            			    	pageRule16 = pageRule();
            			    	state.followingStackPointer--;

            			    	stream_pageRule.Add(pageRule16.Tree);

            			    }
            			    break;
            			case 2 :
            			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:52:35: ruleset
            			    {
            			    	PushFollow(FOLLOW_ruleset_in_media223);
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
            		;	// Stops C# compiler whining that label 'loop6' has no statements

            	char_literal18=(IToken)Match(input,34,FOLLOW_34_in_media227);  
            	stream_34.Add(char_literal18);



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
            	// 52:49: -> ^( MEDIA IDENT ( pageRule )* ( ruleset )* )
            	{
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:52:52: ^( MEDIA IDENT ( pageRule )* ( ruleset )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(MEDIA, "MEDIA"), root_1);

            	    adaptor.AddChild(root_1, stream_IDENT.NextNode());
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:52:67: ( pageRule )*
            	    while ( stream_pageRule.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_pageRule.NextTree());

            	    }
            	    stream_pageRule.Reset();
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:52:77: ( ruleset )*
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:55:1: pageRule : '@page' ( IDENT )* ( pseudo )* '{' ( properties )? ( region )* '}' -> ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* ) ;
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
        RewriteRuleTokenStream stream_33 = new RewriteRuleTokenStream(adaptor,"token 33");
        RewriteRuleTokenStream stream_34 = new RewriteRuleTokenStream(adaptor,"token 34");
        RewriteRuleSubtreeStream stream_region = new RewriteRuleSubtreeStream(adaptor,"rule region");
        RewriteRuleSubtreeStream stream_pseudo = new RewriteRuleSubtreeStream(adaptor,"rule pseudo");
        RewriteRuleSubtreeStream stream_properties = new RewriteRuleSubtreeStream(adaptor,"rule properties");
        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:3: ( '@page' ( IDENT )* ( pseudo )* '{' ( properties )? ( region )* '}' -> ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* ) )
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:5: '@page' ( IDENT )* ( pseudo )* '{' ( properties )? ( region )* '}'
            {
            	string_literal19=(IToken)Match(input,35,FOLLOW_35_in_pageRule255);  
            	stream_35.Add(string_literal19);

            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:13: ( IDENT )*
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
            			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:13: IDENT
            			    {
            			    	IDENT20=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_pageRule257);  
            			    	stream_IDENT.Add(IDENT20);


            			    }
            			    break;

            			default:
            			    goto loop7;
            	    }
            	} while (true);

            	loop7:
            		;	// Stops C# compiler whining that label 'loop7' has no statements

            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:20: ( pseudo )*
            	do 
            	{
            	    int alt8 = 2;
            	    int LA8_0 = input.LA(1);

            	    if ( ((LA8_0 >= 43 && LA8_0 <= 44)) )
            	    {
            	        alt8 = 1;
            	    }


            	    switch (alt8) 
            		{
            			case 1 :
            			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:20: pseudo
            			    {
            			    	PushFollow(FOLLOW_pseudo_in_pageRule260);
            			    	pseudo21 = pseudo();
            			    	state.followingStackPointer--;

            			    	stream_pseudo.Add(pseudo21.Tree);

            			    }
            			    break;

            			default:
            			    goto loop8;
            	    }
            	} while (true);

            	loop8:
            		;	// Stops C# compiler whining that label 'loop8' has no statements

            	char_literal22=(IToken)Match(input,33,FOLLOW_33_in_pageRule263);  
            	stream_33.Add(char_literal22);

            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:32: ( properties )?
            	int alt9 = 2;
            	int LA9_0 = input.LA(1);

            	if ( (LA9_0 == IDENT) )
            	{
            	    alt9 = 1;
            	}
            	switch (alt9) 
            	{
            	    case 1 :
            	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:32: properties
            	        {
            	        	PushFollow(FOLLOW_properties_in_pageRule265);
            	        	properties23 = properties();
            	        	state.followingStackPointer--;

            	        	stream_properties.Add(properties23.Tree);

            	        }
            	        break;

            	}

            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:44: ( region )*
            	do 
            	{
            	    int alt10 = 2;
            	    int LA10_0 = input.LA(1);

            	    if ( (LA10_0 == 36) )
            	    {
            	        alt10 = 1;
            	    }


            	    switch (alt10) 
            		{
            			case 1 :
            			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:44: region
            			    {
            			    	PushFollow(FOLLOW_region_in_pageRule268);
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

            	char_literal25=(IToken)Match(input,34,FOLLOW_34_in_pageRule271);  
            	stream_34.Add(char_literal25);



            	// AST REWRITE
            	// elements:          properties, pseudo, IDENT, region
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 56:56: -> ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* )
            	{
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:59: ^( PAGE ( IDENT )* ( pseudo )* ( properties )* ( region )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(PAGE, "PAGE"), root_1);

            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:67: ( IDENT )*
            	    while ( stream_IDENT.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_IDENT.NextNode());

            	    }
            	    stream_IDENT.Reset();
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:74: ( pseudo )*
            	    while ( stream_pseudo.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_pseudo.NextTree());

            	    }
            	    stream_pseudo.Reset();
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:82: ( properties )*
            	    while ( stream_properties.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_properties.NextTree());

            	    }
            	    stream_properties.Reset();
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:56:94: ( region )*
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:59:1: region : '@' IDENT '{' ( properties )? '}' -> ^( REGION IDENT ( properties )* ) ;
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
        RewriteRuleTokenStream stream_36 = new RewriteRuleTokenStream(adaptor,"token 36");
        RewriteRuleTokenStream stream_33 = new RewriteRuleTokenStream(adaptor,"token 33");
        RewriteRuleTokenStream stream_34 = new RewriteRuleTokenStream(adaptor,"token 34");
        RewriteRuleSubtreeStream stream_properties = new RewriteRuleSubtreeStream(adaptor,"rule properties");
        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:60:2: ( '@' IDENT '{' ( properties )? '}' -> ^( REGION IDENT ( properties )* ) )
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:60:4: '@' IDENT '{' ( properties )? '}'
            {
            	char_literal26=(IToken)Match(input,36,FOLLOW_36_in_region302);  
            	stream_36.Add(char_literal26);

            	IDENT27=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_region304);  
            	stream_IDENT.Add(IDENT27);

            	char_literal28=(IToken)Match(input,33,FOLLOW_33_in_region306);  
            	stream_33.Add(char_literal28);

            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:60:18: ( properties )?
            	int alt11 = 2;
            	int LA11_0 = input.LA(1);

            	if ( (LA11_0 == IDENT) )
            	{
            	    alt11 = 1;
            	}
            	switch (alt11) 
            	{
            	    case 1 :
            	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:60:18: properties
            	        {
            	        	PushFollow(FOLLOW_properties_in_region308);
            	        	properties29 = properties();
            	        	state.followingStackPointer--;

            	        	stream_properties.Add(properties29.Tree);

            	        }
            	        break;

            	}

            	char_literal30=(IToken)Match(input,34,FOLLOW_34_in_region311);  
            	stream_34.Add(char_literal30);



            	// AST REWRITE
            	// elements:          properties, IDENT
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 60:34: -> ^( REGION IDENT ( properties )* )
            	{
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:60:37: ^( REGION IDENT ( properties )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(REGION, "REGION"), root_1);

            	    adaptor.AddChild(root_1, stream_IDENT.NextNode());
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:60:53: ( properties )*
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:63:1: ruleset : selectors '{' ( properties )? '}' -> ^( RULE selectors ( properties )* ) ;
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
        RewriteRuleTokenStream stream_33 = new RewriteRuleTokenStream(adaptor,"token 33");
        RewriteRuleTokenStream stream_34 = new RewriteRuleTokenStream(adaptor,"token 34");
        RewriteRuleSubtreeStream stream_selectors = new RewriteRuleSubtreeStream(adaptor,"rule selectors");
        RewriteRuleSubtreeStream stream_properties = new RewriteRuleSubtreeStream(adaptor,"rule properties");
        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:64:3: ( selectors '{' ( properties )? '}' -> ^( RULE selectors ( properties )* ) )
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:64:5: selectors '{' ( properties )? '}'
            {
            	PushFollow(FOLLOW_selectors_in_ruleset336);
            	selectors31 = selectors();
            	state.followingStackPointer--;

            	stream_selectors.Add(selectors31.Tree);
            	char_literal32=(IToken)Match(input,33,FOLLOW_33_in_ruleset338);  
            	stream_33.Add(char_literal32);

            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:64:19: ( properties )?
            	int alt12 = 2;
            	int LA12_0 = input.LA(1);

            	if ( (LA12_0 == IDENT) )
            	{
            	    alt12 = 1;
            	}
            	switch (alt12) 
            	{
            	    case 1 :
            	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:64:19: properties
            	        {
            	        	PushFollow(FOLLOW_properties_in_ruleset340);
            	        	properties33 = properties();
            	        	state.followingStackPointer--;

            	        	stream_properties.Add(properties33.Tree);

            	        }
            	        break;

            	}

            	char_literal34=(IToken)Match(input,34,FOLLOW_34_in_ruleset343);  
            	stream_34.Add(char_literal34);



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
            	// 64:35: -> ^( RULE selectors ( properties )* )
            	{
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:64:38: ^( RULE selectors ( properties )* )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(RULE, "RULE"), root_1);

            	    adaptor.AddChild(root_1, stream_selectors.NextTree());
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:64:56: ( properties )*
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:67:1: selectors : selector ( ',' selector )* ;
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
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:68:2: ( selector ( ',' selector )* )
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:68:4: selector ( ',' selector )*
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();

            	PushFollow(FOLLOW_selector_in_selectors368);
            	selector35 = selector();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, selector35.Tree);
            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:68:13: ( ',' selector )*
            	do 
            	{
            	    int alt13 = 2;
            	    int LA13_0 = input.LA(1);

            	    if ( (LA13_0 == 37) )
            	    {
            	        alt13 = 1;
            	    }


            	    switch (alt13) 
            		{
            			case 1 :
            			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:68:14: ',' selector
            			    {
            			    	char_literal36=(IToken)Match(input,37,FOLLOW_37_in_selectors371); 
            			    		char_literal36_tree = (CommonTree)adaptor.Create(char_literal36);
            			    		adaptor.AddChild(root_0, char_literal36_tree);

            			    	PushFollow(FOLLOW_selector_in_selectors373);
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:71:1: selector : ( elem ( selectorOperation )* ( pseudo )? -> elem ( selectorOperation )* ( pseudo )* | pseudo -> ANY pseudo );
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
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:72:2: ( elem ( selectorOperation )* ( pseudo )? -> elem ( selectorOperation )* ( pseudo )* | pseudo -> ANY pseudo )
            int alt16 = 2;
            int LA16_0 = input.LA(1);

            if ( (LA16_0 == IDENT || (LA16_0 >= 40 && LA16_0 <= 42)) )
            {
                alt16 = 1;
            }
            else if ( ((LA16_0 >= 43 && LA16_0 <= 44)) )
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
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:72:4: elem ( selectorOperation )* ( pseudo )?
                    {
                    	PushFollow(FOLLOW_elem_in_selector387);
                    	elem38 = elem();
                    	state.followingStackPointer--;

                    	stream_elem.Add(elem38.Tree);
                    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:72:9: ( selectorOperation )*
                    	do 
                    	{
                    	    int alt14 = 2;
                    	    alt14 = dfa14.Predict(input);
                    	    switch (alt14) 
                    		{
                    			case 1 :
                    			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:72:9: selectorOperation
                    			    {
                    			    	PushFollow(FOLLOW_selectorOperation_in_selector389);
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

                    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:72:28: ( pseudo )?
                    	int alt15 = 2;
                    	int LA15_0 = input.LA(1);

                    	if ( ((LA15_0 >= 43 && LA15_0 <= 44)) )
                    	{
                    	    alt15 = 1;
                    	}
                    	switch (alt15) 
                    	{
                    	    case 1 :
                    	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:72:28: pseudo
                    	        {
                    	        	PushFollow(FOLLOW_pseudo_in_selector392);
                    	        	pseudo40 = pseudo();
                    	        	state.followingStackPointer--;

                    	        	stream_pseudo.Add(pseudo40.Tree);

                    	        }
                    	        break;

                    	}



                    	// AST REWRITE
                    	// elements:          selectorOperation, pseudo, elem
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 72:36: -> elem ( selectorOperation )* ( pseudo )*
                    	{
                    	    adaptor.AddChild(root_0, stream_elem.NextTree());
                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:72:45: ( selectorOperation )*
                    	    while ( stream_selectorOperation.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_0, stream_selectorOperation.NextTree());

                    	    }
                    	    stream_selectorOperation.Reset();
                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:72:64: ( pseudo )*
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
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:73:4: pseudo
                    {
                    	PushFollow(FOLLOW_pseudo_in_selector409);
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
                    	// 73:11: -> ANY pseudo
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:76:1: selectorOperation : ( selectop )? elem -> ( selectop )* elem ;
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
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:77:2: ( ( selectop )? elem -> ( selectop )* elem )
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:77:4: ( selectop )? elem
            {
            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:77:4: ( selectop )?
            	int alt17 = 2;
            	int LA17_0 = input.LA(1);

            	if ( ((LA17_0 >= 38 && LA17_0 <= 39)) )
            	{
            	    alt17 = 1;
            	}
            	switch (alt17) 
            	{
            	    case 1 :
            	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:77:4: selectop
            	        {
            	        	PushFollow(FOLLOW_selectop_in_selectorOperation427);
            	        	selectop42 = selectop();
            	        	state.followingStackPointer--;

            	        	stream_selectop.Add(selectop42.Tree);

            	        }
            	        break;

            	}

            	PushFollow(FOLLOW_elem_in_selectorOperation430);
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
            	// 77:19: -> ( selectop )* elem
            	{
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:77:22: ( selectop )*
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:80:1: selectop : ( '>' -> PARENTOF | '+' -> PRECEDES );
    public csst3Parser.selectop_return selectop() // throws RecognitionException [1]
    {   
        csst3Parser.selectop_return retval = new csst3Parser.selectop_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal44 = null;
        IToken char_literal45 = null;

        CommonTree char_literal44_tree=null;
        CommonTree char_literal45_tree=null;
        RewriteRuleTokenStream stream_39 = new RewriteRuleTokenStream(adaptor,"token 39");
        RewriteRuleTokenStream stream_38 = new RewriteRuleTokenStream(adaptor,"token 38");

        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:81:2: ( '>' -> PARENTOF | '+' -> PRECEDES )
            int alt18 = 2;
            int LA18_0 = input.LA(1);

            if ( (LA18_0 == 38) )
            {
                alt18 = 1;
            }
            else if ( (LA18_0 == 39) )
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
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:81:4: '>'
                    {
                    	char_literal44=(IToken)Match(input,38,FOLLOW_38_in_selectop448);  
                    	stream_38.Add(char_literal44);



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
                    	// 81:8: -> PARENTOF
                    	{
                    	    adaptor.AddChild(root_0, (CommonTree)adaptor.Create(PARENTOF, "PARENTOF"));

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:82:11: '+'
                    {
                    	char_literal45=(IToken)Match(input,39,FOLLOW_39_in_selectop464);  
                    	stream_39.Add(char_literal45);



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
                    	// 82:16: -> PRECEDES
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:85:1: properties : declaration ( ';' ( declaration )? )* -> ( declaration )+ ;
    public csst3Parser.properties_return properties() // throws RecognitionException [1]
    {   
        csst3Parser.properties_return retval = new csst3Parser.properties_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal47 = null;
        csst3Parser.declaration_return declaration46 = null;

        csst3Parser.declaration_return declaration48 = null;


        CommonTree char_literal47_tree=null;
        RewriteRuleTokenStream stream_31 = new RewriteRuleTokenStream(adaptor,"token 31");
        RewriteRuleSubtreeStream stream_declaration = new RewriteRuleSubtreeStream(adaptor,"rule declaration");
        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:86:2: ( declaration ( ';' ( declaration )? )* -> ( declaration )+ )
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:86:4: declaration ( ';' ( declaration )? )*
            {
            	PushFollow(FOLLOW_declaration_in_properties480);
            	declaration46 = declaration();
            	state.followingStackPointer--;

            	stream_declaration.Add(declaration46.Tree);
            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:86:16: ( ';' ( declaration )? )*
            	do 
            	{
            	    int alt20 = 2;
            	    int LA20_0 = input.LA(1);

            	    if ( (LA20_0 == 31) )
            	    {
            	        alt20 = 1;
            	    }


            	    switch (alt20) 
            		{
            			case 1 :
            			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:86:17: ';' ( declaration )?
            			    {
            			    	char_literal47=(IToken)Match(input,31,FOLLOW_31_in_properties483);  
            			    	stream_31.Add(char_literal47);

            			    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:86:21: ( declaration )?
            			    	int alt19 = 2;
            			    	int LA19_0 = input.LA(1);

            			    	if ( (LA19_0 == IDENT) )
            			    	{
            			    	    alt19 = 1;
            			    	}
            			    	switch (alt19) 
            			    	{
            			    	    case 1 :
            			    	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:86:21: declaration
            			    	        {
            			    	        	PushFollow(FOLLOW_declaration_in_properties485);
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
            	// 86:36: -> ( declaration )+
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:89:1: elem : ( IDENT ( attrib )* -> ^( TAG IDENT ( attrib )* ) | '#' IDENT ( attrib )* -> ^( ID IDENT ( attrib )* ) | '.' IDENT ( attrib )* -> ^( CLASS IDENT ( attrib )* ) | '*' ( attrib )* -> ^( ANY ( attrib )* ) );
    public csst3Parser.elem_return elem() // throws RecognitionException [1]
    {   
        csst3Parser.elem_return retval = new csst3Parser.elem_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken IDENT49 = null;
        IToken char_literal51 = null;
        IToken IDENT52 = null;
        IToken char_literal54 = null;
        IToken IDENT55 = null;
        IToken char_literal57 = null;
        csst3Parser.attrib_return attrib50 = null;

        csst3Parser.attrib_return attrib53 = null;

        csst3Parser.attrib_return attrib56 = null;

        csst3Parser.attrib_return attrib58 = null;


        CommonTree IDENT49_tree=null;
        CommonTree char_literal51_tree=null;
        CommonTree IDENT52_tree=null;
        CommonTree char_literal54_tree=null;
        CommonTree IDENT55_tree=null;
        CommonTree char_literal57_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_42 = new RewriteRuleTokenStream(adaptor,"token 42");
        RewriteRuleTokenStream stream_41 = new RewriteRuleTokenStream(adaptor,"token 41");
        RewriteRuleTokenStream stream_40 = new RewriteRuleTokenStream(adaptor,"token 40");
        RewriteRuleSubtreeStream stream_attrib = new RewriteRuleSubtreeStream(adaptor,"rule attrib");
        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:90:2: ( IDENT ( attrib )* -> ^( TAG IDENT ( attrib )* ) | '#' IDENT ( attrib )* -> ^( ID IDENT ( attrib )* ) | '.' IDENT ( attrib )* -> ^( CLASS IDENT ( attrib )* ) | '*' ( attrib )* -> ^( ANY ( attrib )* ) )
            int alt25 = 4;
            switch ( input.LA(1) ) 
            {
            case IDENT:
            	{
                alt25 = 1;
                }
                break;
            case 40:
            	{
                alt25 = 2;
                }
                break;
            case 41:
            	{
                alt25 = 3;
                }
                break;
            case 42:
            	{
                alt25 = 4;
                }
                break;
            	default:
            	    NoViableAltException nvae_d25s0 =
            	        new NoViableAltException("", 25, 0, input);

            	    throw nvae_d25s0;
            }

            switch (alt25) 
            {
                case 1 :
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:90:8: IDENT ( attrib )*
                    {
                    	IDENT49=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_elem510);  
                    	stream_IDENT.Add(IDENT49);

                    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:90:14: ( attrib )*
                    	do 
                    	{
                    	    int alt21 = 2;
                    	    alt21 = dfa21.Predict(input);
                    	    switch (alt21) 
                    		{
                    			case 1 :
                    			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:90:14: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem512);
                    			    	attrib50 = attrib();
                    			    	state.followingStackPointer--;

                    			    	stream_attrib.Add(attrib50.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop21;
                    	    }
                    	} while (true);

                    	loop21:
                    		;	// Stops C# compiler whining that label 'loop21' has no statements



                    	// AST REWRITE
                    	// elements:          IDENT, attrib
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 90:22: -> ^( TAG IDENT ( attrib )* )
                    	{
                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:90:25: ^( TAG IDENT ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(TAG, "TAG"), root_1);

                    	    adaptor.AddChild(root_1, stream_IDENT.NextNode());
                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:90:38: ( attrib )*
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
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:91:4: '#' IDENT ( attrib )*
                    {
                    	char_literal51=(IToken)Match(input,40,FOLLOW_40_in_elem531);  
                    	stream_40.Add(char_literal51);

                    	IDENT52=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_elem533);  
                    	stream_IDENT.Add(IDENT52);

                    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:91:14: ( attrib )*
                    	do 
                    	{
                    	    int alt22 = 2;
                    	    alt22 = dfa22.Predict(input);
                    	    switch (alt22) 
                    		{
                    			case 1 :
                    			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:91:14: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem535);
                    			    	attrib53 = attrib();
                    			    	state.followingStackPointer--;

                    			    	stream_attrib.Add(attrib53.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop22;
                    	    }
                    	} while (true);

                    	loop22:
                    		;	// Stops C# compiler whining that label 'loop22' has no statements



                    	// AST REWRITE
                    	// elements:          attrib, IDENT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 91:22: -> ^( ID IDENT ( attrib )* )
                    	{
                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:91:25: ^( ID IDENT ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(ID, "ID"), root_1);

                    	    adaptor.AddChild(root_1, stream_IDENT.NextNode());
                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:91:37: ( attrib )*
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
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:92:4: '.' IDENT ( attrib )*
                    {
                    	char_literal54=(IToken)Match(input,41,FOLLOW_41_in_elem554);  
                    	stream_41.Add(char_literal54);

                    	IDENT55=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_elem556);  
                    	stream_IDENT.Add(IDENT55);

                    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:92:14: ( attrib )*
                    	do 
                    	{
                    	    int alt23 = 2;
                    	    alt23 = dfa23.Predict(input);
                    	    switch (alt23) 
                    		{
                    			case 1 :
                    			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:92:14: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem558);
                    			    	attrib56 = attrib();
                    			    	state.followingStackPointer--;

                    			    	stream_attrib.Add(attrib56.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop23;
                    	    }
                    	} while (true);

                    	loop23:
                    		;	// Stops C# compiler whining that label 'loop23' has no statements



                    	// AST REWRITE
                    	// elements:          IDENT, attrib
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 92:22: -> ^( CLASS IDENT ( attrib )* )
                    	{
                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:92:25: ^( CLASS IDENT ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(CLASS, "CLASS"), root_1);

                    	    adaptor.AddChild(root_1, stream_IDENT.NextNode());
                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:92:40: ( attrib )*
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
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:93:4: '*' ( attrib )*
                    {
                    	char_literal57=(IToken)Match(input,42,FOLLOW_42_in_elem577);  
                    	stream_42.Add(char_literal57);

                    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:93:8: ( attrib )*
                    	do 
                    	{
                    	    int alt24 = 2;
                    	    alt24 = dfa24.Predict(input);
                    	    switch (alt24) 
                    		{
                    			case 1 :
                    			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:93:8: attrib
                    			    {
                    			    	PushFollow(FOLLOW_attrib_in_elem579);
                    			    	attrib58 = attrib();
                    			    	state.followingStackPointer--;

                    			    	stream_attrib.Add(attrib58.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop24;
                    	    }
                    	} while (true);

                    	loop24:
                    		;	// Stops C# compiler whining that label 'loop24' has no statements



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
                    	// 93:16: -> ^( ANY ( attrib )* )
                    	{
                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:93:19: ^( ANY ( attrib )* )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(ANY, "ANY"), root_1);

                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:93:26: ( attrib )*
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:96:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );
    public csst3Parser.pseudo_return pseudo() // throws RecognitionException [1]
    {   
        csst3Parser.pseudo_return retval = new csst3Parser.pseudo_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal59 = null;
        IToken string_literal60 = null;
        IToken IDENT61 = null;
        IToken char_literal62 = null;
        IToken string_literal63 = null;
        csst3Parser.function_return function64 = null;


        CommonTree char_literal59_tree=null;
        CommonTree string_literal60_tree=null;
        CommonTree IDENT61_tree=null;
        CommonTree char_literal62_tree=null;
        CommonTree string_literal63_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_43 = new RewriteRuleTokenStream(adaptor,"token 43");
        RewriteRuleTokenStream stream_44 = new RewriteRuleTokenStream(adaptor,"token 44");
        RewriteRuleSubtreeStream stream_function = new RewriteRuleSubtreeStream(adaptor,"rule function");
        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:97:2: ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) )
            int alt28 = 2;
            alt28 = dfa28.Predict(input);
            switch (alt28) 
            {
                case 1 :
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:97:4: ( ':' | '::' ) IDENT
                    {
                    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:97:4: ( ':' | '::' )
                    	int alt26 = 2;
                    	int LA26_0 = input.LA(1);

                    	if ( (LA26_0 == 43) )
                    	{
                    	    alt26 = 1;
                    	}
                    	else if ( (LA26_0 == 44) )
                    	{
                    	    alt26 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d26s0 =
                    	        new NoViableAltException("", 26, 0, input);

                    	    throw nvae_d26s0;
                    	}
                    	switch (alt26) 
                    	{
                    	    case 1 :
                    	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:97:5: ':'
                    	        {
                    	        	char_literal59=(IToken)Match(input,43,FOLLOW_43_in_pseudo603);  
                    	        	stream_43.Add(char_literal59);


                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:97:9: '::'
                    	        {
                    	        	string_literal60=(IToken)Match(input,44,FOLLOW_44_in_pseudo605);  
                    	        	stream_44.Add(string_literal60);


                    	        }
                    	        break;

                    	}

                    	IDENT61=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_pseudo608);  
                    	stream_IDENT.Add(IDENT61);



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
                    	// 97:21: -> ^( PSEUDO IDENT )
                    	{
                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:97:24: ^( PSEUDO IDENT )
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
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:98:4: ( ':' | '::' ) function
                    {
                    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:98:4: ( ':' | '::' )
                    	int alt27 = 2;
                    	int LA27_0 = input.LA(1);

                    	if ( (LA27_0 == 43) )
                    	{
                    	    alt27 = 1;
                    	}
                    	else if ( (LA27_0 == 44) )
                    	{
                    	    alt27 = 2;
                    	}
                    	else 
                    	{
                    	    NoViableAltException nvae_d27s0 =
                    	        new NoViableAltException("", 27, 0, input);

                    	    throw nvae_d27s0;
                    	}
                    	switch (alt27) 
                    	{
                    	    case 1 :
                    	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:98:5: ':'
                    	        {
                    	        	char_literal62=(IToken)Match(input,43,FOLLOW_43_in_pseudo624);  
                    	        	stream_43.Add(char_literal62);


                    	        }
                    	        break;
                    	    case 2 :
                    	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:98:9: '::'
                    	        {
                    	        	string_literal63=(IToken)Match(input,44,FOLLOW_44_in_pseudo626);  
                    	        	stream_44.Add(string_literal63);


                    	        }
                    	        break;

                    	}

                    	PushFollow(FOLLOW_function_in_pseudo629);
                    	function64 = function();
                    	state.followingStackPointer--;

                    	stream_function.Add(function64.Tree);


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
                    	// 98:24: -> ^( PSEUDO function )
                    	{
                    	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:98:27: ^( PSEUDO function )
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:101:1: attrib : '[' IDENT ( attribRelate ( STRING | IDENT ) )? ']' -> ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? ) ;
    public csst3Parser.attrib_return attrib() // throws RecognitionException [1]
    {   
        csst3Parser.attrib_return retval = new csst3Parser.attrib_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal65 = null;
        IToken IDENT66 = null;
        IToken STRING68 = null;
        IToken IDENT69 = null;
        IToken char_literal70 = null;
        csst3Parser.attribRelate_return attribRelate67 = null;


        CommonTree char_literal65_tree=null;
        CommonTree IDENT66_tree=null;
        CommonTree STRING68_tree=null;
        CommonTree IDENT69_tree=null;
        CommonTree char_literal70_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_45 = new RewriteRuleTokenStream(adaptor,"token 45");
        RewriteRuleTokenStream stream_46 = new RewriteRuleTokenStream(adaptor,"token 46");
        RewriteRuleTokenStream stream_STRING = new RewriteRuleTokenStream(adaptor,"token STRING");
        RewriteRuleSubtreeStream stream_attribRelate = new RewriteRuleSubtreeStream(adaptor,"rule attribRelate");
        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:102:2: ( '[' IDENT ( attribRelate ( STRING | IDENT ) )? ']' -> ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? ) )
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:102:4: '[' IDENT ( attribRelate ( STRING | IDENT ) )? ']'
            {
            	char_literal65=(IToken)Match(input,45,FOLLOW_45_in_attrib650);  
            	stream_45.Add(char_literal65);

            	IDENT66=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_attrib652);  
            	stream_IDENT.Add(IDENT66);

            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:102:14: ( attribRelate ( STRING | IDENT ) )?
            	int alt30 = 2;
            	int LA30_0 = input.LA(1);

            	if ( ((LA30_0 >= 47 && LA30_0 <= 49)) )
            	{
            	    alt30 = 1;
            	}
            	switch (alt30) 
            	{
            	    case 1 :
            	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:102:15: attribRelate ( STRING | IDENT )
            	        {
            	        	PushFollow(FOLLOW_attribRelate_in_attrib655);
            	        	attribRelate67 = attribRelate();
            	        	state.followingStackPointer--;

            	        	stream_attribRelate.Add(attribRelate67.Tree);
            	        	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:102:28: ( STRING | IDENT )
            	        	int alt29 = 2;
            	        	int LA29_0 = input.LA(1);

            	        	if ( (LA29_0 == STRING) )
            	        	{
            	        	    alt29 = 1;
            	        	}
            	        	else if ( (LA29_0 == IDENT) )
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
            	        	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:102:29: STRING
            	        	        {
            	        	        	STRING68=(IToken)Match(input,STRING,FOLLOW_STRING_in_attrib658);  
            	        	        	stream_STRING.Add(STRING68);


            	        	        }
            	        	        break;
            	        	    case 2 :
            	        	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:102:38: IDENT
            	        	        {
            	        	        	IDENT69=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_attrib662);  
            	        	        	stream_IDENT.Add(IDENT69);


            	        	        }
            	        	        break;

            	        	}


            	        }
            	        break;

            	}

            	char_literal70=(IToken)Match(input,46,FOLLOW_46_in_attrib667);  
            	stream_46.Add(char_literal70);



            	// AST REWRITE
            	// elements:          IDENT, attribRelate, IDENT, STRING
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 102:51: -> ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? )
            	{
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:102:54: ^( ATTRIB IDENT ( attribRelate ( STRING )* ( IDENT )* )? )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(ATTRIB, "ATTRIB"), root_1);

            	    adaptor.AddChild(root_1, stream_IDENT.NextNode());
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:102:70: ( attribRelate ( STRING )* ( IDENT )* )?
            	    if ( stream_IDENT.HasNext() || stream_attribRelate.HasNext() || stream_STRING.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_attribRelate.NextTree());
            	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:102:84: ( STRING )*
            	        while ( stream_STRING.HasNext() )
            	        {
            	            adaptor.AddChild(root_1, stream_STRING.NextNode());

            	        }
            	        stream_STRING.Reset();
            	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:102:92: ( IDENT )*
            	        while ( stream_IDENT.HasNext() )
            	        {
            	            adaptor.AddChild(root_1, stream_IDENT.NextNode());

            	        }
            	        stream_IDENT.Reset();

            	    }
            	    stream_IDENT.Reset();
            	    stream_attribRelate.Reset();
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:105:1: attribRelate : ( '=' -> ATTRIBEQUAL | '~=' -> HASVALUE | '|=' -> BEGINSWITH );
    public csst3Parser.attribRelate_return attribRelate() // throws RecognitionException [1]
    {   
        csst3Parser.attribRelate_return retval = new csst3Parser.attribRelate_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal71 = null;
        IToken string_literal72 = null;
        IToken string_literal73 = null;

        CommonTree char_literal71_tree=null;
        CommonTree string_literal72_tree=null;
        CommonTree string_literal73_tree=null;
        RewriteRuleTokenStream stream_49 = new RewriteRuleTokenStream(adaptor,"token 49");
        RewriteRuleTokenStream stream_48 = new RewriteRuleTokenStream(adaptor,"token 48");
        RewriteRuleTokenStream stream_47 = new RewriteRuleTokenStream(adaptor,"token 47");

        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:106:2: ( '=' -> ATTRIBEQUAL | '~=' -> HASVALUE | '|=' -> BEGINSWITH )
            int alt31 = 3;
            switch ( input.LA(1) ) 
            {
            case 47:
            	{
                alt31 = 1;
                }
                break;
            case 48:
            	{
                alt31 = 2;
                }
                break;
            case 49:
            	{
                alt31 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d31s0 =
            	        new NoViableAltException("", 31, 0, input);

            	    throw nvae_d31s0;
            }

            switch (alt31) 
            {
                case 1 :
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:106:4: '='
                    {
                    	char_literal71=(IToken)Match(input,47,FOLLOW_47_in_attribRelate700);  
                    	stream_47.Add(char_literal71);



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
                    	// 106:9: -> ATTRIBEQUAL
                    	{
                    	    adaptor.AddChild(root_0, (CommonTree)adaptor.Create(ATTRIBEQUAL, "ATTRIBEQUAL"));

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:107:4: '~='
                    {
                    	string_literal72=(IToken)Match(input,48,FOLLOW_48_in_attribRelate710);  
                    	stream_48.Add(string_literal72);



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
                    	// 107:9: -> HASVALUE
                    	{
                    	    adaptor.AddChild(root_0, (CommonTree)adaptor.Create(HASVALUE, "HASVALUE"));

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 3 :
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:108:4: '|='
                    {
                    	string_literal73=(IToken)Match(input,49,FOLLOW_49_in_attribRelate719);  
                    	stream_49.Add(string_literal73);



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
                    	// 108:9: -> BEGINSWITH
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:111:1: declaration : IDENT ':' args -> ^( PROPERTY IDENT args ) ;
    public csst3Parser.declaration_return declaration() // throws RecognitionException [1]
    {   
        csst3Parser.declaration_return retval = new csst3Parser.declaration_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken IDENT74 = null;
        IToken char_literal75 = null;
        csst3Parser.args_return args76 = null;


        CommonTree IDENT74_tree=null;
        CommonTree char_literal75_tree=null;
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleTokenStream stream_43 = new RewriteRuleTokenStream(adaptor,"token 43");
        RewriteRuleSubtreeStream stream_args = new RewriteRuleSubtreeStream(adaptor,"rule args");
        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:112:2: ( IDENT ':' args -> ^( PROPERTY IDENT args ) )
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:112:4: IDENT ':' args
            {
            	IDENT74=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_declaration737);  
            	stream_IDENT.Add(IDENT74);

            	char_literal75=(IToken)Match(input,43,FOLLOW_43_in_declaration739);  
            	stream_43.Add(char_literal75);

            	PushFollow(FOLLOW_args_in_declaration741);
            	args76 = args();
            	state.followingStackPointer--;

            	stream_args.Add(args76.Tree);


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
            	// 112:19: -> ^( PROPERTY IDENT args )
            	{
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:112:22: ^( PROPERTY IDENT args )
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:115:1: args : expr ( ( ',' )? expr )* -> ( expr )* ;
    public csst3Parser.args_return args() // throws RecognitionException [1]
    {   
        csst3Parser.args_return retval = new csst3Parser.args_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal78 = null;
        csst3Parser.expr_return expr77 = null;

        csst3Parser.expr_return expr79 = null;


        CommonTree char_literal78_tree=null;
        RewriteRuleTokenStream stream_37 = new RewriteRuleTokenStream(adaptor,"token 37");
        RewriteRuleSubtreeStream stream_expr = new RewriteRuleSubtreeStream(adaptor,"rule expr");
        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:116:2: ( expr ( ( ',' )? expr )* -> ( expr )* )
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:116:4: expr ( ( ',' )? expr )*
            {
            	PushFollow(FOLLOW_expr_in_args764);
            	expr77 = expr();
            	state.followingStackPointer--;

            	stream_expr.Add(expr77.Tree);
            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:116:9: ( ( ',' )? expr )*
            	do 
            	{
            	    int alt33 = 2;
            	    alt33 = dfa33.Predict(input);
            	    switch (alt33) 
            		{
            			case 1 :
            			    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:116:10: ( ',' )? expr
            			    {
            			    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:116:10: ( ',' )?
            			    	int alt32 = 2;
            			    	int LA32_0 = input.LA(1);

            			    	if ( (LA32_0 == 37) )
            			    	{
            			    	    alt32 = 1;
            			    	}
            			    	switch (alt32) 
            			    	{
            			    	    case 1 :
            			    	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:116:10: ','
            			    	        {
            			    	        	char_literal78=(IToken)Match(input,37,FOLLOW_37_in_args767);  
            			    	        	stream_37.Add(char_literal78);


            			    	        }
            			    	        break;

            			    	}

            			    	PushFollow(FOLLOW_expr_in_args770);
            			    	expr79 = expr();
            			    	state.followingStackPointer--;

            			    	stream_expr.Add(expr79.Tree);

            			    }
            			    break;

            			default:
            			    goto loop33;
            	    }
            	} while (true);

            	loop33:
            		;	// Stops C# compiler whining that label 'loop33' has no statements



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
            	// 116:22: -> ( expr )*
            	{
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:116:25: ( expr )*
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:119:1: expr : ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function );
    public csst3Parser.expr_return expr() // throws RecognitionException [1]
    {   
        csst3Parser.expr_return retval = new csst3Parser.expr_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken NUM80 = null;
        IToken IDENT82 = null;
        IToken COLOR83 = null;
        IToken STRING84 = null;
        csst3Parser.unit_return unit81 = null;

        csst3Parser.function_return function85 = null;


        CommonTree NUM80_tree=null;
        CommonTree IDENT82_tree=null;
        CommonTree COLOR83_tree=null;
        CommonTree STRING84_tree=null;

        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:120:2: ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function )
            int alt35 = 5;
            alt35 = dfa35.Predict(input);
            switch (alt35) 
            {
                case 1 :
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:120:4: ( NUM ( unit )? )
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:120:4: ( NUM ( unit )? )
                    	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:120:5: NUM ( unit )?
                    	{
                    		NUM80=(IToken)Match(input,NUM,FOLLOW_NUM_in_expr789); 
                    			NUM80_tree = (CommonTree)adaptor.Create(NUM80);
                    			adaptor.AddChild(root_0, NUM80_tree);

                    		// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:120:9: ( unit )?
                    		int alt34 = 2;
                    		alt34 = dfa34.Predict(input);
                    		switch (alt34) 
                    		{
                    		    case 1 :
                    		        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:120:9: unit
                    		        {
                    		        	PushFollow(FOLLOW_unit_in_expr791);
                    		        	unit81 = unit();
                    		        	state.followingStackPointer--;

                    		        	adaptor.AddChild(root_0, unit81.Tree);

                    		        }
                    		        break;

                    		}


                    	}


                    }
                    break;
                case 2 :
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:121:4: IDENT
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	IDENT82=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_expr798); 
                    		IDENT82_tree = (CommonTree)adaptor.Create(IDENT82);
                    		adaptor.AddChild(root_0, IDENT82_tree);


                    }
                    break;
                case 3 :
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:122:4: COLOR
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	COLOR83=(IToken)Match(input,COLOR,FOLLOW_COLOR_in_expr803); 
                    		COLOR83_tree = (CommonTree)adaptor.Create(COLOR83);
                    		adaptor.AddChild(root_0, COLOR83_tree);


                    }
                    break;
                case 4 :
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:123:4: STRING
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	STRING84=(IToken)Match(input,STRING,FOLLOW_STRING_in_expr808); 
                    		STRING84_tree = (CommonTree)adaptor.Create(STRING84);
                    		adaptor.AddChild(root_0, STRING84_tree);


                    }
                    break;
                case 5 :
                    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:124:4: function
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_function_in_expr813);
                    	function85 = function();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, function85.Tree);

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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:127:1: unit : ( '%' | 'px' | 'cm' | 'mm' | 'in' | 'pt' | 'pc' | 'em' | 'ex' | 'deg' | 'rad' | 'grad' | 'ms' | 's' | 'hz' | 'khz' ) ;
    public csst3Parser.unit_return unit() // throws RecognitionException [1]
    {   
        csst3Parser.unit_return retval = new csst3Parser.unit_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken set86 = null;

        CommonTree set86_tree=null;

        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:128:2: ( ( '%' | 'px' | 'cm' | 'mm' | 'in' | 'pt' | 'pc' | 'em' | 'ex' | 'deg' | 'rad' | 'grad' | 'ms' | 's' | 'hz' | 'khz' ) )
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:128:4: ( '%' | 'px' | 'cm' | 'mm' | 'in' | 'pt' | 'pc' | 'em' | 'ex' | 'deg' | 'rad' | 'grad' | 'ms' | 's' | 'hz' | 'khz' )
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();

            	set86 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= 50 && input.LA(1) <= 65) ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (CommonTree)adaptor.Create(set86));
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
    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:131:1: function : IDENT '(' ( args )? ')' -> IDENT '(' ( args )* ')' ;
    public csst3Parser.function_return function() // throws RecognitionException [1]
    {   
        csst3Parser.function_return retval = new csst3Parser.function_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken IDENT87 = null;
        IToken char_literal88 = null;
        IToken char_literal90 = null;
        csst3Parser.args_return args89 = null;


        CommonTree IDENT87_tree=null;
        CommonTree char_literal88_tree=null;
        CommonTree char_literal90_tree=null;
        RewriteRuleTokenStream stream_67 = new RewriteRuleTokenStream(adaptor,"token 67");
        RewriteRuleTokenStream stream_66 = new RewriteRuleTokenStream(adaptor,"token 66");
        RewriteRuleTokenStream stream_IDENT = new RewriteRuleTokenStream(adaptor,"token IDENT");
        RewriteRuleSubtreeStream stream_args = new RewriteRuleSubtreeStream(adaptor,"rule args");
        try 
    	{
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:132:2: ( IDENT '(' ( args )? ')' -> IDENT '(' ( args )* ')' )
            // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:132:4: IDENT '(' ( args )? ')'
            {
            	IDENT87=(IToken)Match(input,IDENT,FOLLOW_IDENT_in_function868);  
            	stream_IDENT.Add(IDENT87);

            	char_literal88=(IToken)Match(input,66,FOLLOW_66_in_function870);  
            	stream_66.Add(char_literal88);

            	// C:\\git\\pathway\\pathway\\CssParser\\csst3.g:132:14: ( args )?
            	int alt36 = 2;
            	int LA36_0 = input.LA(1);

            	if ( ((LA36_0 >= STRING && LA36_0 <= COLOR)) )
            	{
            	    alt36 = 1;
            	}
            	switch (alt36) 
            	{
            	    case 1 :
            	        // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:132:14: args
            	        {
            	        	PushFollow(FOLLOW_args_in_function872);
            	        	args89 = args();
            	        	state.followingStackPointer--;

            	        	stream_args.Add(args89.Tree);

            	        }
            	        break;

            	}

            	char_literal90=(IToken)Match(input,67,FOLLOW_67_in_function875);  
            	stream_67.Add(char_literal90);



            	// AST REWRITE
            	// elements:          67, 66, args, IDENT
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 132:24: -> IDENT '(' ( args )* ')'
            	{
            	    adaptor.AddChild(root_0, stream_IDENT.NextNode());
            	    adaptor.AddChild(root_0, stream_66.NextNode());
            	    // C:\\git\\pathway\\pathway\\CssParser\\csst3.g:132:37: ( args )*
            	    while ( stream_args.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_args.NextTree());

            	    }
            	    stream_args.Reset();
            	    adaptor.AddChild(root_0, stream_67.NextNode());

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
   	protected DFA14 dfa14;
   	protected DFA21 dfa21;
   	protected DFA22 dfa22;
   	protected DFA23 dfa23;
   	protected DFA24 dfa24;
   	protected DFA28 dfa28;
   	protected DFA33 dfa33;
   	protected DFA35 dfa35;
   	protected DFA34 dfa34;
	private void InitializeCyclicDFAs()
	{
    	this.dfa1 = new DFA1(this);
    	this.dfa2 = new DFA2(this);
    	this.dfa14 = new DFA14(this);
    	this.dfa21 = new DFA21(this);
    	this.dfa22 = new DFA22(this);
    	this.dfa23 = new DFA23(this);
    	this.dfa24 = new DFA24(this);
    	this.dfa28 = new DFA28(this);
    	this.dfa33 = new DFA33(this);
    	this.dfa35 = new DFA35(this);
    	this.dfa34 = new DFA34(this);
	}

    const string DFA1_eotS =
        "\x0b\uffff";
    const string DFA1_eofS =
        "\x0b\uffff";
    const string DFA1_minS =
        "\x01\x17\x0a\uffff";
    const string DFA1_maxS =
        "\x01\x2c\x0a\uffff";
    const string DFA1_acceptS =
        "\x01\uffff\x01\x02\x07\uffff\x01\x01\x01\uffff";
    const string DFA1_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA1_transitionS = {
            "\x01\x01\x05\uffff\x02\x09\x01\uffff\x01\x01\x02\uffff\x01"+
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
            get { return "()* loopback of 43:4: ( importRule )*"; }
        }

    }

    const string DFA2_eotS =
        "\x0a\uffff";
    const string DFA2_eofS =
        "\x01\x01\x09\uffff";
    const string DFA2_minS =
        "\x01\x17\x09\uffff";
    const string DFA2_maxS =
        "\x01\x2c\x09\uffff";
    const string DFA2_acceptS =
        "\x01\uffff\x01\x04\x01\x01\x01\x02\x01\x03\x05\uffff";
    const string DFA2_specialS =
        "\x0a\uffff}>";
    static readonly string[] DFA2_transitionS = {
            "\x01\x04\x08\uffff\x01\x02\x02\uffff\x01\x03\x04\uffff\x05"+
            "\x04",
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
            get { return "()+ loopback of 43:16: ( media | pageRule | ruleset )+"; }
        }

    }

    const string DFA14_eotS =
        "\x0b\uffff";
    const string DFA14_eofS =
        "\x0b\uffff";
    const string DFA14_minS =
        "\x01\x17\x0a\uffff";
    const string DFA14_maxS =
        "\x01\x2c\x0a\uffff";
    const string DFA14_acceptS =
        "\x01\uffff\x01\x02\x03\uffff\x01\x01\x05\uffff";
    const string DFA14_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA14_transitionS = {
            "\x01\x05\x09\uffff\x01\x01\x03\uffff\x01\x01\x05\x05\x02\x01",
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
            get { return "()* loopback of 72:9: ( selectorOperation )*"; }
        }

    }

    const string DFA21_eotS =
        "\x0c\uffff";
    const string DFA21_eofS =
        "\x0c\uffff";
    const string DFA21_minS =
        "\x01\x17\x0b\uffff";
    const string DFA21_maxS =
        "\x01\x2d\x0b\uffff";
    const string DFA21_acceptS =
        "\x01\uffff\x01\x02\x09\uffff\x01\x01";
    const string DFA21_specialS =
        "\x0c\uffff}>";
    static readonly string[] DFA21_transitionS = {
            "\x01\x01\x09\uffff\x01\x01\x03\uffff\x08\x01\x01\x0b",
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

    static readonly short[] DFA21_eot = DFA.UnpackEncodedString(DFA21_eotS);
    static readonly short[] DFA21_eof = DFA.UnpackEncodedString(DFA21_eofS);
    static readonly char[] DFA21_min = DFA.UnpackEncodedStringToUnsignedChars(DFA21_minS);
    static readonly char[] DFA21_max = DFA.UnpackEncodedStringToUnsignedChars(DFA21_maxS);
    static readonly short[] DFA21_accept = DFA.UnpackEncodedString(DFA21_acceptS);
    static readonly short[] DFA21_special = DFA.UnpackEncodedString(DFA21_specialS);
    static readonly short[][] DFA21_transition = DFA.UnpackEncodedStringArray(DFA21_transitionS);

    protected class DFA21 : DFA
    {
        public DFA21(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 21;
            this.eot = DFA21_eot;
            this.eof = DFA21_eof;
            this.min = DFA21_min;
            this.max = DFA21_max;
            this.accept = DFA21_accept;
            this.special = DFA21_special;
            this.transition = DFA21_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 90:14: ( attrib )*"; }
        }

    }

    const string DFA22_eotS =
        "\x0c\uffff";
    const string DFA22_eofS =
        "\x0c\uffff";
    const string DFA22_minS =
        "\x01\x17\x0b\uffff";
    const string DFA22_maxS =
        "\x01\x2d\x0b\uffff";
    const string DFA22_acceptS =
        "\x01\uffff\x01\x02\x09\uffff\x01\x01";
    const string DFA22_specialS =
        "\x0c\uffff}>";
    static readonly string[] DFA22_transitionS = {
            "\x01\x01\x09\uffff\x01\x01\x03\uffff\x08\x01\x01\x0b",
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
            get { return "()* loopback of 91:14: ( attrib )*"; }
        }

    }

    const string DFA23_eotS =
        "\x0c\uffff";
    const string DFA23_eofS =
        "\x0c\uffff";
    const string DFA23_minS =
        "\x01\x17\x0b\uffff";
    const string DFA23_maxS =
        "\x01\x2d\x0b\uffff";
    const string DFA23_acceptS =
        "\x01\uffff\x01\x02\x09\uffff\x01\x01";
    const string DFA23_specialS =
        "\x0c\uffff}>";
    static readonly string[] DFA23_transitionS = {
            "\x01\x01\x09\uffff\x01\x01\x03\uffff\x08\x01\x01\x0b",
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

    static readonly short[] DFA23_eot = DFA.UnpackEncodedString(DFA23_eotS);
    static readonly short[] DFA23_eof = DFA.UnpackEncodedString(DFA23_eofS);
    static readonly char[] DFA23_min = DFA.UnpackEncodedStringToUnsignedChars(DFA23_minS);
    static readonly char[] DFA23_max = DFA.UnpackEncodedStringToUnsignedChars(DFA23_maxS);
    static readonly short[] DFA23_accept = DFA.UnpackEncodedString(DFA23_acceptS);
    static readonly short[] DFA23_special = DFA.UnpackEncodedString(DFA23_specialS);
    static readonly short[][] DFA23_transition = DFA.UnpackEncodedStringArray(DFA23_transitionS);

    protected class DFA23 : DFA
    {
        public DFA23(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 23;
            this.eot = DFA23_eot;
            this.eof = DFA23_eof;
            this.min = DFA23_min;
            this.max = DFA23_max;
            this.accept = DFA23_accept;
            this.special = DFA23_special;
            this.transition = DFA23_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 92:14: ( attrib )*"; }
        }

    }

    const string DFA24_eotS =
        "\x0c\uffff";
    const string DFA24_eofS =
        "\x0c\uffff";
    const string DFA24_minS =
        "\x01\x17\x0b\uffff";
    const string DFA24_maxS =
        "\x01\x2d\x0b\uffff";
    const string DFA24_acceptS =
        "\x01\uffff\x01\x02\x09\uffff\x01\x01";
    const string DFA24_specialS =
        "\x0c\uffff}>";
    static readonly string[] DFA24_transitionS = {
            "\x01\x01\x09\uffff\x01\x01\x03\uffff\x08\x01\x01\x0b",
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
            get { return "()* loopback of 93:8: ( attrib )*"; }
        }

    }

    const string DFA28_eotS =
        "\x0f\uffff";
    const string DFA28_eofS =
        "\x0f\uffff";
    const string DFA28_minS =
        "\x01\x2b\x02\x17\x02\x21\x0a\uffff";
    const string DFA28_maxS =
        "\x01\x2c\x02\x17\x02\x42\x0a\uffff";
    const string DFA28_acceptS =
        "\x05\uffff\x01\x02\x01\x01\x08\uffff";
    const string DFA28_specialS =
        "\x0f\uffff}>";
    static readonly string[] DFA28_transitionS = {
            "\x01\x01\x01\x02",
            "\x01\x03",
            "\x01\x04",
            "\x01\x06\x03\uffff\x01\x06\x05\uffff\x02\x06\x15\uffff\x01"+
            "\x05",
            "\x01\x06\x03\uffff\x01\x06\x05\uffff\x02\x06\x15\uffff\x01"+
            "\x05",
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

    static readonly short[] DFA28_eot = DFA.UnpackEncodedString(DFA28_eotS);
    static readonly short[] DFA28_eof = DFA.UnpackEncodedString(DFA28_eofS);
    static readonly char[] DFA28_min = DFA.UnpackEncodedStringToUnsignedChars(DFA28_minS);
    static readonly char[] DFA28_max = DFA.UnpackEncodedStringToUnsignedChars(DFA28_maxS);
    static readonly short[] DFA28_accept = DFA.UnpackEncodedString(DFA28_acceptS);
    static readonly short[] DFA28_special = DFA.UnpackEncodedString(DFA28_specialS);
    static readonly short[][] DFA28_transition = DFA.UnpackEncodedStringArray(DFA28_transitionS);

    protected class DFA28 : DFA
    {
        public DFA28(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 28;
            this.eot = DFA28_eot;
            this.eof = DFA28_eof;
            this.min = DFA28_min;
            this.max = DFA28_max;
            this.accept = DFA28_accept;
            this.special = DFA28_special;
            this.transition = DFA28_transition;

        }

        override public string Description
        {
            get { return "96:1: pseudo : ( ( ':' | '::' ) IDENT -> ^( PSEUDO IDENT ) | ( ':' | '::' ) function -> ^( PSEUDO function ) );"; }
        }

    }

    const string DFA33_eotS =
        "\x0a\uffff";
    const string DFA33_eofS =
        "\x0a\uffff";
    const string DFA33_minS =
        "\x01\x16\x09\uffff";
    const string DFA33_maxS =
        "\x01\x43\x09\uffff";
    const string DFA33_acceptS =
        "\x01\uffff\x01\x02\x03\uffff\x01\x01\x04\uffff";
    const string DFA33_specialS =
        "\x0a\uffff}>";
    static readonly string[] DFA33_transitionS = {
            "\x04\x05\x05\uffff\x01\x01\x02\uffff\x01\x01\x01\uffff\x01"+
            "\x01\x01\x05\x1d\uffff\x01\x01",
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

    static readonly short[] DFA33_eot = DFA.UnpackEncodedString(DFA33_eotS);
    static readonly short[] DFA33_eof = DFA.UnpackEncodedString(DFA33_eofS);
    static readonly char[] DFA33_min = DFA.UnpackEncodedStringToUnsignedChars(DFA33_minS);
    static readonly char[] DFA33_max = DFA.UnpackEncodedStringToUnsignedChars(DFA33_maxS);
    static readonly short[] DFA33_accept = DFA.UnpackEncodedString(DFA33_acceptS);
    static readonly short[] DFA33_special = DFA.UnpackEncodedString(DFA33_specialS);
    static readonly short[][] DFA33_transition = DFA.UnpackEncodedStringArray(DFA33_transitionS);

    protected class DFA33 : DFA
    {
        public DFA33(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 33;
            this.eot = DFA33_eot;
            this.eof = DFA33_eof;
            this.min = DFA33_min;
            this.max = DFA33_max;
            this.accept = DFA33_accept;
            this.special = DFA33_special;
            this.transition = DFA33_transition;

        }

        override public string Description
        {
            get { return "()* loopback of 116:9: ( ( ',' )? expr )*"; }
        }

    }

    const string DFA35_eotS =
        "\x0f\uffff";
    const string DFA35_eofS =
        "\x0f\uffff";
    const string DFA35_minS =
        "\x01\x16\x01\uffff\x01\x16\x0c\uffff";
    const string DFA35_maxS =
        "\x01\x19\x01\uffff\x01\x43\x0c\uffff";
    const string DFA35_acceptS =
        "\x01\uffff\x01\x01\x01\uffff\x01\x03\x01\x04\x01\x05\x01\x02\x08"+
        "\uffff";
    const string DFA35_specialS =
        "\x0f\uffff}>";
    static readonly string[] DFA35_transitionS = {
            "\x01\x04\x01\x02\x01\x01\x01\x03",
            "",
            "\x04\x06\x05\uffff\x01\x06\x02\uffff\x01\x06\x01\uffff\x02"+
            "\x06\x1c\uffff\x01\x05\x01\x06",
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

    static readonly short[] DFA35_eot = DFA.UnpackEncodedString(DFA35_eotS);
    static readonly short[] DFA35_eof = DFA.UnpackEncodedString(DFA35_eofS);
    static readonly char[] DFA35_min = DFA.UnpackEncodedStringToUnsignedChars(DFA35_minS);
    static readonly char[] DFA35_max = DFA.UnpackEncodedStringToUnsignedChars(DFA35_maxS);
    static readonly short[] DFA35_accept = DFA.UnpackEncodedString(DFA35_acceptS);
    static readonly short[] DFA35_special = DFA.UnpackEncodedString(DFA35_specialS);
    static readonly short[][] DFA35_transition = DFA.UnpackEncodedStringArray(DFA35_transitionS);

    protected class DFA35 : DFA
    {
        public DFA35(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 35;
            this.eot = DFA35_eot;
            this.eof = DFA35_eof;
            this.min = DFA35_min;
            this.max = DFA35_max;
            this.accept = DFA35_accept;
            this.special = DFA35_special;
            this.transition = DFA35_transition;

        }

        override public string Description
        {
            get { return "119:1: expr : ( ( NUM ( unit )? ) | IDENT | COLOR | STRING | function );"; }
        }

    }

    const string DFA34_eotS =
        "\x0b\uffff";
    const string DFA34_eofS =
        "\x0b\uffff";
    const string DFA34_minS =
        "\x01\x16\x0a\uffff";
    const string DFA34_maxS =
        "\x01\x43\x0a\uffff";
    const string DFA34_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x08\uffff";
    const string DFA34_specialS =
        "\x0b\uffff}>";
    static readonly string[] DFA34_transitionS = {
            "\x04\x02\x05\uffff\x01\x02\x02\uffff\x01\x02\x01\uffff\x02"+
            "\x02\x0c\uffff\x10\x01\x01\uffff\x01\x02",
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

    static readonly short[] DFA34_eot = DFA.UnpackEncodedString(DFA34_eotS);
    static readonly short[] DFA34_eof = DFA.UnpackEncodedString(DFA34_eofS);
    static readonly char[] DFA34_min = DFA.UnpackEncodedStringToUnsignedChars(DFA34_minS);
    static readonly char[] DFA34_max = DFA.UnpackEncodedStringToUnsignedChars(DFA34_maxS);
    static readonly short[] DFA34_accept = DFA.UnpackEncodedString(DFA34_acceptS);
    static readonly short[] DFA34_special = DFA.UnpackEncodedString(DFA34_specialS);
    static readonly short[][] DFA34_transition = DFA.UnpackEncodedStringArray(DFA34_transitionS);

    protected class DFA34 : DFA
    {
        public DFA34(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 34;
            this.eot = DFA34_eot;
            this.eof = DFA34_eof;
            this.min = DFA34_min;
            this.max = DFA34_max;
            this.accept = DFA34_accept;
            this.special = DFA34_special;
            this.transition = DFA34_transition;

        }

        override public string Description
        {
            get { return "120:9: ( unit )?"; }
        }

    }

 

    public static readonly BitSet FOLLOW_importRule_in_stylesheet129 = new BitSet(new ulong[]{0x00001F0960800000UL});
    public static readonly BitSet FOLLOW_media_in_stylesheet133 = new BitSet(new ulong[]{0x00001F0900800002UL});
    public static readonly BitSet FOLLOW_pageRule_in_stylesheet137 = new BitSet(new ulong[]{0x00001F0900800002UL});
    public static readonly BitSet FOLLOW_ruleset_in_stylesheet141 = new BitSet(new ulong[]{0x00001F0900800002UL});
    public static readonly BitSet FOLLOW_29_in_importRule155 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_30_in_importRule159 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_STRING_in_importRule163 = new BitSet(new ulong[]{0x0000000080000000UL});
    public static readonly BitSet FOLLOW_31_in_importRule165 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_29_in_importRule181 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_30_in_importRule185 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_function_in_importRule189 = new BitSet(new ulong[]{0x0000000080000000UL});
    public static readonly BitSet FOLLOW_31_in_importRule191 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_32_in_media212 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_media214 = new BitSet(new ulong[]{0x0000000200000000UL});
    public static readonly BitSet FOLLOW_33_in_media216 = new BitSet(new ulong[]{0x00001F0900800000UL});
    public static readonly BitSet FOLLOW_pageRule_in_media219 = new BitSet(new ulong[]{0x00001F0D00800000UL});
    public static readonly BitSet FOLLOW_ruleset_in_media223 = new BitSet(new ulong[]{0x00001F0D00800000UL});
    public static readonly BitSet FOLLOW_34_in_media227 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_35_in_pageRule255 = new BitSet(new ulong[]{0x00001F0B00800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_pageRule257 = new BitSet(new ulong[]{0x00001F0B00800000UL});
    public static readonly BitSet FOLLOW_pseudo_in_pageRule260 = new BitSet(new ulong[]{0x00001F0B00800000UL});
    public static readonly BitSet FOLLOW_33_in_pageRule263 = new BitSet(new ulong[]{0x0000001400800000UL});
    public static readonly BitSet FOLLOW_properties_in_pageRule265 = new BitSet(new ulong[]{0x0000001400000000UL});
    public static readonly BitSet FOLLOW_region_in_pageRule268 = new BitSet(new ulong[]{0x0000001400000000UL});
    public static readonly BitSet FOLLOW_34_in_pageRule271 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_36_in_region302 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_region304 = new BitSet(new ulong[]{0x0000000200000000UL});
    public static readonly BitSet FOLLOW_33_in_region306 = new BitSet(new ulong[]{0x0000000400800000UL});
    public static readonly BitSet FOLLOW_properties_in_region308 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_34_in_region311 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selectors_in_ruleset336 = new BitSet(new ulong[]{0x0000000200000000UL});
    public static readonly BitSet FOLLOW_33_in_ruleset338 = new BitSet(new ulong[]{0x0000000400800000UL});
    public static readonly BitSet FOLLOW_properties_in_ruleset340 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_34_in_ruleset343 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selector_in_selectors368 = new BitSet(new ulong[]{0x0000002000000002UL});
    public static readonly BitSet FOLLOW_37_in_selectors371 = new BitSet(new ulong[]{0x00001F0900800000UL});
    public static readonly BitSet FOLLOW_selector_in_selectors373 = new BitSet(new ulong[]{0x0000002000000002UL});
    public static readonly BitSet FOLLOW_elem_in_selector387 = new BitSet(new ulong[]{0x00001FC900800002UL});
    public static readonly BitSet FOLLOW_selectorOperation_in_selector389 = new BitSet(new ulong[]{0x00001FC900800002UL});
    public static readonly BitSet FOLLOW_pseudo_in_selector392 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_pseudo_in_selector409 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_selectop_in_selectorOperation427 = new BitSet(new ulong[]{0x0000070000800000UL});
    public static readonly BitSet FOLLOW_elem_in_selectorOperation430 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_38_in_selectop448 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_39_in_selectop464 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_declaration_in_properties480 = new BitSet(new ulong[]{0x0000000080000002UL});
    public static readonly BitSet FOLLOW_31_in_properties483 = new BitSet(new ulong[]{0x0000000080800002UL});
    public static readonly BitSet FOLLOW_declaration_in_properties485 = new BitSet(new ulong[]{0x0000000080000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_elem510 = new BitSet(new ulong[]{0x0000200000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem512 = new BitSet(new ulong[]{0x0000200000000002UL});
    public static readonly BitSet FOLLOW_40_in_elem531 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_elem533 = new BitSet(new ulong[]{0x0000200000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem535 = new BitSet(new ulong[]{0x0000200000000002UL});
    public static readonly BitSet FOLLOW_41_in_elem554 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_elem556 = new BitSet(new ulong[]{0x0000200000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem558 = new BitSet(new ulong[]{0x0000200000000002UL});
    public static readonly BitSet FOLLOW_42_in_elem577 = new BitSet(new ulong[]{0x0000200000000002UL});
    public static readonly BitSet FOLLOW_attrib_in_elem579 = new BitSet(new ulong[]{0x0000200000000002UL});
    public static readonly BitSet FOLLOW_43_in_pseudo603 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_44_in_pseudo605 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_pseudo608 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_43_in_pseudo624 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_44_in_pseudo626 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_function_in_pseudo629 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_45_in_attrib650 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_IDENT_in_attrib652 = new BitSet(new ulong[]{0x0003C00000000000UL});
    public static readonly BitSet FOLLOW_attribRelate_in_attrib655 = new BitSet(new ulong[]{0x0000000000C00000UL});
    public static readonly BitSet FOLLOW_STRING_in_attrib658 = new BitSet(new ulong[]{0x0000400000000000UL});
    public static readonly BitSet FOLLOW_IDENT_in_attrib662 = new BitSet(new ulong[]{0x0000400000000000UL});
    public static readonly BitSet FOLLOW_46_in_attrib667 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_47_in_attribRelate700 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_48_in_attribRelate710 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_49_in_attribRelate719 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_declaration737 = new BitSet(new ulong[]{0x0000080000000000UL});
    public static readonly BitSet FOLLOW_43_in_declaration739 = new BitSet(new ulong[]{0x0000000003C00000UL});
    public static readonly BitSet FOLLOW_args_in_declaration741 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expr_in_args764 = new BitSet(new ulong[]{0x0000002003C00002UL});
    public static readonly BitSet FOLLOW_37_in_args767 = new BitSet(new ulong[]{0x0000000003C00000UL});
    public static readonly BitSet FOLLOW_expr_in_args770 = new BitSet(new ulong[]{0x0000002003C00002UL});
    public static readonly BitSet FOLLOW_NUM_in_expr789 = new BitSet(new ulong[]{0xFFFC000000000002UL,0x0000000000000003UL});
    public static readonly BitSet FOLLOW_unit_in_expr791 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_expr798 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_COLOR_in_expr803 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STRING_in_expr808 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_expr813 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_unit824 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IDENT_in_function868 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000000004UL});
    public static readonly BitSet FOLLOW_66_in_function870 = new BitSet(new ulong[]{0x0000000003C00000UL,0x0000000000000008UL});
    public static readonly BitSet FOLLOW_args_in_function872 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000000000008UL});
    public static readonly BitSet FOLLOW_67_in_function875 = new BitSet(new ulong[]{0x0000000000000002UL});

}
