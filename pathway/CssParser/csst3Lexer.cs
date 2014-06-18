// $ANTLR 3.1.2 C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g 2013-01-18 15:58:55


using System;
using Antlr.Runtime;
using IList = System.Collections.IList;
using ArrayList = System.Collections.ArrayList;
using Stack = Antlr.Runtime.Collections.StackList;


public class csst3Lexer : Lexer
{
    public const int FUNCTION = 17;
    public const int CLASS = 21;
    public const int ATTRIB = 9;
    public const int HASVALUE = 13;
    public const int PSEUDO = 15;
    public const int MEDIA = 5;
    public const int ID = 20;
    public const int ATTRIBEQUAL = 12;
    public const int EOF = -1;
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
    public const int ANY = 18;
    public const int PAGE = 6;
    public const int WS = 29;
    public const int T__33 = 33;
    public const int T__34 = 34;
    public const int T__35 = 35;
    public const int T__36 = 36;
    public const int PROPERTY = 16;
    public const int T__37 = 37;
    public const int T__38 = 38;
    public const int T__39 = 39;
    public const int SL_COMMENT = 27;
    public const int STRING = 22;

    // delegates
    // delegators

    public csst3Lexer()
    {
        InitializeCyclicDFAs();
    }
    public csst3Lexer(ICharStream input)
        : this(input, null)
    {
    }
    public csst3Lexer(ICharStream input, RecognizerSharedState state)
        : base(input, state)
    {
        InitializeCyclicDFAs();

    }

    override public string GrammarFileName
    {
        get { return "C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g"; }
    }

    // $ANTLR start "T__30"
    public void mT__30() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__30;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:7:7: ( '@import' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:7:9: '@import'
            {
                Match("@import");


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__30"

    // $ANTLR start "T__31"
    public void mT__31() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__31;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:8:7: ( '@include' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:8:9: '@include'
            {
                Match("@include");


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__31"

    // $ANTLR start "T__32"
    public void mT__32() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__32;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:9:7: ( ';' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:9:9: ';'
            {
                Match(';');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__32"

    // $ANTLR start "T__33"
    public void mT__33() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__33;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:10:7: ( '@media' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:10:9: '@media'
            {
                Match("@media");


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__33"

    // $ANTLR start "T__34"
    public void mT__34() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__34;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:11:7: ( '{' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:11:9: '{'
            {
                Match('{');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__34"

    // $ANTLR start "T__35"
    public void mT__35() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__35;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:12:7: ( '}' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:12:9: '}'
            {
                Match('}');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__35"

    // $ANTLR start "T__36"
    public void mT__36() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__36;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:13:7: ( '@page' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:13:9: '@page'
            {
                Match("@page");


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__36"

    // $ANTLR start "T__37"
    public void mT__37() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__37;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:14:7: ( '@' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:14:9: '@'
            {
                Match('@');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__37"

    // $ANTLR start "T__38"
    public void mT__38() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__38;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:15:7: ( ',' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:15:9: ','
            {
                Match(',');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__38"

    // $ANTLR start "T__39"
    public void mT__39() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__39;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:16:7: ( '>' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:16:9: '>'
            {
                Match('>');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__39"

    // $ANTLR start "T__40"
    public void mT__40() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__40;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:17:7: ( '+' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:17:9: '+'
            {
                Match('+');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__40"

    // $ANTLR start "T__41"
    public void mT__41() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__41;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:18:7: ( '#' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:18:9: '#'
            {
                Match('#');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__41"

    // $ANTLR start "T__42"
    public void mT__42() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__42;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:19:7: ( '.' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:19:9: '.'
            {
                Match('.');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__42"

    // $ANTLR start "T__43"
    public void mT__43() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__43;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:20:7: ( '*' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:20:9: '*'
            {
                Match('*');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__43"

    // $ANTLR start "T__44"
    public void mT__44() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__44;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:21:7: ( ':' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:21:9: ':'
            {
                Match(':');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__44"

    // $ANTLR start "T__45"
    public void mT__45() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__45;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:22:7: ( '::' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:22:9: '::'
            {
                Match("::");


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__45"

    // $ANTLR start "T__46"
    public void mT__46() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__46;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:23:7: ( '[' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:23:9: '['
            {
                Match('[');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__46"

    // $ANTLR start "T__47"
    public void mT__47() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__47;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:24:7: ( ']' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:24:9: ']'
            {
                Match(']');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__47"

    // $ANTLR start "T__48"
    public void mT__48() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__48;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:25:7: ( '=' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:25:9: '='
            {
                Match('=');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__48"

    // $ANTLR start "T__49"
    public void mT__49() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__49;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:26:7: ( '~=' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:26:9: '~='
            {
                Match("~=");


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__49"

    // $ANTLR start "T__50"
    public void mT__50() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__50;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:27:7: ( '|=' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:27:9: '|='
            {
                Match("|=");


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__50"

    // $ANTLR start "T__51"
    public void mT__51() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__51;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:28:7: ( '%' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:28:9: '%'
            {
                Match('%');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__51"

    // $ANTLR start "T__52"
    public void mT__52() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__52;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:29:7: ( '(' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:29:9: '('
            {
                Match('(');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__52"

    // $ANTLR start "T__53"
    public void mT__53() // throws RecognitionException [2]
    {
        try
        {
            int _type = T__53;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:30:7: ( ')' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:30:9: ')'
            {
                Match(')');

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "T__53"

    // $ANTLR start "UNIT"
    public void mUNIT() // throws RecognitionException [2]
    {
        try
        {
            int _type = UNIT;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:11: ( 'em' | 'px' | 'cm' | 'mm' | 'in' | 'pt' | 'pc' | 'ex' | 'deg' | 'rad' | 'grad' | 'ms' | 's' | 'hz' | 'khz' )
            int alt1 = 15;
            alt1 = dfa1.Predict(input);
            switch (alt1)
            {
                case 1:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:13: 'em'
                    {
                        Match("em");


                    }
                    break;
                case 2:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:18: 'px'
                    {
                        Match("px");


                    }
                    break;
                case 3:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:23: 'cm'
                    {
                        Match("cm");


                    }
                    break;
                case 4:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:28: 'mm'
                    {
                        Match("mm");


                    }
                    break;
                case 5:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:33: 'in'
                    {
                        Match("in");


                    }
                    break;
                case 6:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:38: 'pt'
                    {
                        Match("pt");


                    }
                    break;
                case 7:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:43: 'pc'
                    {
                        Match("pc");


                    }
                    break;
                case 8:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:48: 'ex'
                    {
                        Match("ex");


                    }
                    break;
                case 9:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:53: 'deg'
                    {
                        Match("deg");


                    }
                    break;
                case 10:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:59: 'rad'
                    {
                        Match("rad");


                    }
                    break;
                case 11:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:65: 'grad'
                    {
                        Match("grad");


                    }
                    break;
                case 12:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:72: 'ms'
                    {
                        Match("ms");


                    }
                    break;
                case 13:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:77: 's'
                    {
                        Match('s');

                    }
                    break;
                case 14:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:81: 'hz'
                    {
                        Match("hz");


                    }
                    break;
                case 15:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:138:86: 'khz'
                    {
                        Match("khz");


                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "UNIT"

    // $ANTLR start "IDENT"
    public void mIDENT() // throws RecognitionException [2]
    {
        try
        {
            int _type = IDENT;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:141:2: ( ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' ) ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )* | '-' ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' ) ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )* )
            int alt4 = 2;
            int LA4_0 = input.LA(1);

            if (((LA4_0 >= 'A' && LA4_0 <= 'Z') || LA4_0 == '_' || (LA4_0 >= 'a' && LA4_0 <= 'z') || (LA4_0 >= '\u0100' && LA4_0 <= '\uFFFE')))
            {
                alt4 = 1;
            }
            else if ((LA4_0 == '-'))
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
                case 1:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:141:4: ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' ) ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )*
                    {
                        if ((input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || (input.LA(1) >= '\u0100' && input.LA(1) <= '\uFFFE'))
                        {
                            input.Consume();

                        }
                        else
                        {
                            MismatchedSetException mse = new MismatchedSetException(null, input);
                            Recover(mse);
                            throw mse;
                        }

                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:142:3: ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )*
                        do
                        {
                            int alt2 = 2;
                            int LA2_0 = input.LA(1);

                            if ((LA2_0 == '-' || (LA2_0 >= '0' && LA2_0 <= '9') || (LA2_0 >= 'A' && LA2_0 <= 'Z') || LA2_0 == '_' || (LA2_0 >= 'a' && LA2_0 <= 'z') || (LA2_0 >= '\u0100' && LA2_0 <= '\uFFFE')))
                            {
                                alt2 = 1;
                            }


                            switch (alt2)
                            {
                                case 1:
                                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:
                                    {
                                        if (input.LA(1) == '-' || (input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || (input.LA(1) >= '\u0100' && input.LA(1) <= '\uFFFE'))
                                        {
                                            input.Consume();

                                        }
                                        else
                                        {
                                            MismatchedSetException mse = new MismatchedSetException(null, input);
                                            Recover(mse);
                                            throw mse;
                                        }


                                    }
                                    break;

                                default:
                                    goto loop2;
                            }
                        } while (true);

                    loop2:
                        ;	// Stops C# compiler whining that label 'loop2' has no statements


                    }
                    break;
                case 2:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:143:4: '-' ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' ) ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )*
                    {
                        Match('-');
                        if ((input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || (input.LA(1) >= '\u0100' && input.LA(1) <= '\uFFFE'))
                        {
                            input.Consume();

                        }
                        else
                        {
                            MismatchedSetException mse = new MismatchedSetException(null, input);
                            Recover(mse);
                            throw mse;
                        }

                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:144:3: ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )*
                        do
                        {
                            int alt3 = 2;
                            int LA3_0 = input.LA(1);

                            if ((LA3_0 == '-' || (LA3_0 >= '0' && LA3_0 <= '9') || (LA3_0 >= 'A' && LA3_0 <= 'Z') || LA3_0 == '_' || (LA3_0 >= 'a' && LA3_0 <= 'z') || (LA3_0 >= '\u0100' && LA3_0 <= '\uFFFE')))
                            {
                                alt3 = 1;
                            }


                            switch (alt3)
                            {
                                case 1:
                                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:
                                    {
                                        if (input.LA(1) == '-' || (input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || (input.LA(1) >= '\u0100' && input.LA(1) <= '\uFFFE'))
                                        {
                                            input.Consume();

                                        }
                                        else
                                        {
                                            MismatchedSetException mse = new MismatchedSetException(null, input);
                                            Recover(mse);
                                            throw mse;
                                        }


                                    }
                                    break;

                                default:
                                    goto loop3;
                            }
                        } while (true);

                    loop3:
                        ;	// Stops C# compiler whining that label 'loop3' has no statements


                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "IDENT"

    // $ANTLR start "STRING"
    public void mSTRING() // throws RecognitionException [2]
    {
        try
        {
            int _type = STRING;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:149:2: ( '\"' ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\"' | '\\n' | '\\r' | '\\\\' ) )* '\"' | '\\'' ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\\'' | '\\n' | '\\r' | '\\\\' ) )* '\\'' )
            int alt7 = 2;
            int LA7_0 = input.LA(1);

            if ((LA7_0 == '\"'))
            {
                alt7 = 1;
            }
            else if ((LA7_0 == '\''))
            {
                alt7 = 2;
            }
            else
            {
                NoViableAltException nvae_d7s0 =
                    new NoViableAltException("", 7, 0, input);

                throw nvae_d7s0;
            }
            switch (alt7)
            {
                case 1:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:149:4: '\"' ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\"' | '\\n' | '\\r' | '\\\\' ) )* '\"'
                    {
                        Match('\"');
                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:149:8: ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\"' | '\\n' | '\\r' | '\\\\' ) )*
                        do
                        {
                            int alt5 = 3;
                            int LA5_0 = input.LA(1);

                            if ((LA5_0 == '\\'))
                            {
                                alt5 = 1;
                            }
                            else if (((LA5_0 >= '\u0000' && LA5_0 <= '\t') || (LA5_0 >= '\u000B' && LA5_0 <= '\f') || (LA5_0 >= '\u000E' && LA5_0 <= '!') || (LA5_0 >= '#' && LA5_0 <= '[') || (LA5_0 >= ']' && LA5_0 <= '\uFFFF')))
                            {
                                alt5 = 2;
                            }


                            switch (alt5)
                            {
                                case 1:
                                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:149:10: ( '\\\\' ~ ( '\\n' ) )
                                    {
                                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:149:10: ( '\\\\' ~ ( '\\n' ) )
                                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:149:11: '\\\\' ~ ( '\\n' )
                                        {
                                            Match('\\');
                                            if ((input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\uFFFF'))
                                            {
                                                input.Consume();

                                            }
                                            else
                                            {
                                                MismatchedSetException mse = new MismatchedSetException(null, input);
                                                Recover(mse);
                                                throw mse;
                                            }


                                        }


                                    }
                                    break;
                                case 2:
                                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:150:17: ~ ( '\"' | '\\n' | '\\r' | '\\\\' )
                                    {
                                        if ((input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '!') || (input.LA(1) >= '#' && input.LA(1) <= '[') || (input.LA(1) >= ']' && input.LA(1) <= '\uFFFF'))
                                        {
                                            input.Consume();

                                        }
                                        else
                                        {
                                            MismatchedSetException mse = new MismatchedSetException(null, input);
                                            Recover(mse);
                                            throw mse;
                                        }


                                    }
                                    break;

                                default:
                                    goto loop5;
                            }
                        } while (true);

                    loop5:
                        ;	// Stops C# compiler whining that label 'loop5' has no statements

                        Match('\"');

                    }
                    break;
                case 2:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:152:4: '\\'' ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\\'' | '\\n' | '\\r' | '\\\\' ) )* '\\''
                    {
                        Match('\'');
                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:152:9: ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\\'' | '\\n' | '\\r' | '\\\\' ) )*
                        do
                        {
                            int alt6 = 3;
                            int LA6_0 = input.LA(1);

                            if ((LA6_0 == '\\'))
                            {
                                alt6 = 1;
                            }
                            else if (((LA6_0 >= '\u0000' && LA6_0 <= '\t') || (LA6_0 >= '\u000B' && LA6_0 <= '\f') || (LA6_0 >= '\u000E' && LA6_0 <= '&') || (LA6_0 >= '(' && LA6_0 <= '[') || (LA6_0 >= ']' && LA6_0 <= '\uFFFF')))
                            {
                                alt6 = 2;
                            }


                            switch (alt6)
                            {
                                case 1:
                                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:152:11: ( '\\\\' ~ ( '\\n' ) )
                                    {
                                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:152:11: ( '\\\\' ~ ( '\\n' ) )
                                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:152:12: '\\\\' ~ ( '\\n' )
                                        {
                                            Match('\\');
                                            if ((input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\uFFFF'))
                                            {
                                                input.Consume();

                                            }
                                            else
                                            {
                                                MismatchedSetException mse = new MismatchedSetException(null, input);
                                                Recover(mse);
                                                throw mse;
                                            }


                                        }


                                    }
                                    break;
                                case 2:
                                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:153:17: ~ ( '\\'' | '\\n' | '\\r' | '\\\\' )
                                    {
                                        if ((input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '&') || (input.LA(1) >= '(' && input.LA(1) <= '[') || (input.LA(1) >= ']' && input.LA(1) <= '\uFFFF'))
                                        {
                                            input.Consume();

                                        }
                                        else
                                        {
                                            MismatchedSetException mse = new MismatchedSetException(null, input);
                                            Recover(mse);
                                            throw mse;
                                        }


                                    }
                                    break;

                                default:
                                    goto loop6;
                            }
                        } while (true);

                    loop6:
                        ;	// Stops C# compiler whining that label 'loop6' has no statements

                        Match('\'');

                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "STRING"

    // $ANTLR start "NUM"
    public void mNUM() // throws RecognitionException [2]
    {
        try
        {
            int _type = NUM;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:158:2: ( '-' ( ( '0' .. '9' )* '.' )? ( '0' .. '9' )+ | ( ( '0' .. '9' )* '.' )? ( '0' .. '9' )+ )
            int alt14 = 2;
            int LA14_0 = input.LA(1);

            if ((LA14_0 == '-'))
            {
                alt14 = 1;
            }
            else if ((LA14_0 == '.' || (LA14_0 >= '0' && LA14_0 <= '9')))
            {
                alt14 = 2;
            }
            else
            {
                NoViableAltException nvae_d14s0 =
                    new NoViableAltException("", 14, 0, input);

                throw nvae_d14s0;
            }
            switch (alt14)
            {
                case 1:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:158:4: '-' ( ( '0' .. '9' )* '.' )? ( '0' .. '9' )+
                    {
                        Match('-');
                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:158:8: ( ( '0' .. '9' )* '.' )?
                        int alt9 = 2;
                        alt9 = dfa9.Predict(input);
                        switch (alt9)
                        {
                            case 1:
                                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:158:9: ( '0' .. '9' )* '.'
                                {
                                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:158:9: ( '0' .. '9' )*
                                    do
                                    {
                                        int alt8 = 2;
                                        int LA8_0 = input.LA(1);

                                        if (((LA8_0 >= '0' && LA8_0 <= '9')))
                                        {
                                            alt8 = 1;
                                        }


                                        switch (alt8)
                                        {
                                            case 1:
                                                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:158:10: '0' .. '9'
                                                {
                                                    MatchRange('0', '9');

                                                }
                                                break;

                                            default:
                                                goto loop8;
                                        }
                                    } while (true);

                                loop8:
                                    ;	// Stops C# compiler whining that label 'loop8' has no statements

                                    Match('.');

                                }
                                break;

                        }

                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:158:27: ( '0' .. '9' )+
                        int cnt10 = 0;
                        do
                        {
                            int alt10 = 2;
                            int LA10_0 = input.LA(1);

                            if (((LA10_0 >= '0' && LA10_0 <= '9')))
                            {
                                alt10 = 1;
                            }


                            switch (alt10)
                            {
                                case 1:
                                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:158:28: '0' .. '9'
                                    {
                                        MatchRange('0', '9');

                                    }
                                    break;

                                default:
                                    if (cnt10 >= 1) goto loop10;
                                    EarlyExitException eee10 =
                                        new EarlyExitException(10, input);
                                    throw eee10;
                            }
                            cnt10++;
                        } while (true);

                    loop10:
                        ;	// Stops C# compiler whinging that label 'loop10' has no statements


                    }
                    break;
                case 2:
                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:159:4: ( ( '0' .. '9' )* '.' )? ( '0' .. '9' )+
                    {
                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:159:4: ( ( '0' .. '9' )* '.' )?
                        int alt12 = 2;
                        alt12 = dfa12.Predict(input);
                        switch (alt12)
                        {
                            case 1:
                                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:159:5: ( '0' .. '9' )* '.'
                                {
                                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:159:5: ( '0' .. '9' )*
                                    do
                                    {
                                        int alt11 = 2;
                                        int LA11_0 = input.LA(1);

                                        if (((LA11_0 >= '0' && LA11_0 <= '9')))
                                        {
                                            alt11 = 1;
                                        }


                                        switch (alt11)
                                        {
                                            case 1:
                                                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:159:6: '0' .. '9'
                                                {
                                                    MatchRange('0', '9');

                                                }
                                                break;

                                            default:
                                                goto loop11;
                                        }
                                    } while (true);

                                loop11:
                                    ;	// Stops C# compiler whining that label 'loop11' has no statements

                                    Match('.');

                                }
                                break;

                        }

                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:159:23: ( '0' .. '9' )+
                        int cnt13 = 0;
                        do
                        {
                            int alt13 = 2;
                            int LA13_0 = input.LA(1);

                            if (((LA13_0 >= '0' && LA13_0 <= '9')))
                            {
                                alt13 = 1;
                            }


                            switch (alt13)
                            {
                                case 1:
                                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:159:24: '0' .. '9'
                                    {
                                        MatchRange('0', '9');

                                    }
                                    break;

                                default:
                                    if (cnt13 >= 1) goto loop13;
                                    EarlyExitException eee13 =
                                        new EarlyExitException(13, input);
                                    throw eee13;
                            }
                            cnt13++;
                        } while (true);

                    loop13:
                        ;	// Stops C# compiler whinging that label 'loop13' has no statements


                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "NUM"

    // $ANTLR start "COLOR"
    public void mCOLOR() // throws RecognitionException [2]
    {
        try
        {
            int _type = COLOR;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:163:2: ( '#' ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' )+ )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:163:4: '#' ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' )+
            {
                Match('#');
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:163:8: ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' )+
                int cnt15 = 0;
                do
                {
                    int alt15 = 2;
                    int LA15_0 = input.LA(1);

                    if (((LA15_0 >= '0' && LA15_0 <= '9') || (LA15_0 >= 'A' && LA15_0 <= 'F') || (LA15_0 >= 'a' && LA15_0 <= 'f')))
                    {
                        alt15 = 1;
                    }


                    switch (alt15)
                    {
                        case 1:
                            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:
                            {
                                if ((input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'F') || (input.LA(1) >= 'a' && input.LA(1) <= 'f'))
                                {
                                    input.Consume();

                                }
                                else
                                {
                                    MismatchedSetException mse = new MismatchedSetException(null, input);
                                    Recover(mse);
                                    throw mse;
                                }


                            }
                            break;

                        default:
                            if (cnt15 >= 1) goto loop15;
                            EarlyExitException eee15 =
                                new EarlyExitException(15, input);
                            throw eee15;
                    }
                    cnt15++;
                } while (true);

            loop15:
                ;	// Stops C# compiler whinging that label 'loop15' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "COLOR"

    // $ANTLR start "SL_COMMENT"
    public void mSL_COMMENT() // throws RecognitionException [2]
    {
        try
        {
            int _type = SL_COMMENT;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:168:2: ( '//' (~ ( '\\n' | '\\r' ) )* ( '\\n' | '\\r' ( '\\n' )? ) )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:168:4: '//' (~ ( '\\n' | '\\r' ) )* ( '\\n' | '\\r' ( '\\n' )? )
            {
                Match("//");

                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:169:3: (~ ( '\\n' | '\\r' ) )*
                do
                {
                    int alt16 = 2;
                    int LA16_0 = input.LA(1);

                    if (((LA16_0 >= '\u0000' && LA16_0 <= '\t') || (LA16_0 >= '\u000B' && LA16_0 <= '\f') || (LA16_0 >= '\u000E' && LA16_0 <= '\uFFFF')))
                    {
                        alt16 = 1;
                    }


                    switch (alt16)
                    {
                        case 1:
                            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:169:4: ~ ( '\\n' | '\\r' )
                            {
                                if ((input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '\uFFFF'))
                                {
                                    input.Consume();

                                }
                                else
                                {
                                    MismatchedSetException mse = new MismatchedSetException(null, input);
                                    Recover(mse);
                                    throw mse;
                                }


                            }
                            break;

                        default:
                            goto loop16;
                    }
                } while (true);

            loop16:
                ;	// Stops C# compiler whining that label 'loop16' has no statements

                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:169:19: ( '\\n' | '\\r' ( '\\n' )? )
                int alt18 = 2;
                int LA18_0 = input.LA(1);

                if ((LA18_0 == '\n'))
                {
                    alt18 = 1;
                }
                else if ((LA18_0 == '\r'))
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
                    case 1:
                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:169:20: '\\n'
                        {
                            Match('\n');

                        }
                        break;
                    case 2:
                        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:169:25: '\\r' ( '\\n' )?
                        {
                            Match('\r');
                            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:169:29: ( '\\n' )?
                            int alt17 = 2;
                            int LA17_0 = input.LA(1);

                            if ((LA17_0 == '\n'))
                            {
                                alt17 = 1;
                            }
                            switch (alt17)
                            {
                                case 1:
                                    // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:169:30: '\\n'
                                    {
                                        Match('\n');

                                    }
                                    break;

                            }


                        }
                        break;

                }

                _channel = HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "SL_COMMENT"

    // $ANTLR start "COMMENT"
    public void mCOMMENT() // throws RecognitionException [2]
    {
        try
        {
            int _type = COMMENT;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:175:2: ( '/*' ( . )* '*/' )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:175:4: '/*' ( . )* '*/'
            {
                Match("/*");

                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:175:9: ( . )*
                do
                {
                    int alt19 = 2;
                    int LA19_0 = input.LA(1);

                    if ((LA19_0 == '*'))
                    {
                        int LA19_1 = input.LA(2);

                        if ((LA19_1 == '/'))
                        {
                            alt19 = 2;
                        }
                        else if (((LA19_1 >= '\u0000' && LA19_1 <= '.') || (LA19_1 >= '0' && LA19_1 <= '\uFFFF')))
                        {
                            alt19 = 1;
                        }


                    }
                    else if (((LA19_0 >= '\u0000' && LA19_0 <= ')') || (LA19_0 >= '+' && LA19_0 <= '\uFFFF')))
                    {
                        alt19 = 1;
                    }


                    switch (alt19)
                    {
                        case 1:
                            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:175:9: .
                            {
                                MatchAny();

                            }
                            break;

                        default:
                            goto loop19;
                    }
                } while (true);

            loop19:
                ;	// Stops C# compiler whining that label 'loop19' has no statements

                Match("*/");

                _channel = HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "COMMENT"

    // $ANTLR start "WS"
    public void mWS() // throws RecognitionException [2]
    {
        try
        {
            int _type = WS;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:179:4: ( ( ' ' | '\\t' | '\\r' | '\\n' | '\\f' )+ )
            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:179:6: ( ' ' | '\\t' | '\\r' | '\\n' | '\\f' )+
            {
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:179:6: ( ' ' | '\\t' | '\\r' | '\\n' | '\\f' )+
                int cnt20 = 0;
                do
                {
                    int alt20 = 2;
                    int LA20_0 = input.LA(1);

                    if (((LA20_0 >= '\t' && LA20_0 <= '\n') || (LA20_0 >= '\f' && LA20_0 <= '\r') || LA20_0 == ' '))
                    {
                        alt20 = 1;
                    }


                    switch (alt20)
                    {
                        case 1:
                            // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:
                            {
                                if ((input.LA(1) >= '\t' && input.LA(1) <= '\n') || (input.LA(1) >= '\f' && input.LA(1) <= '\r') || input.LA(1) == ' ')
                                {
                                    input.Consume();

                                }
                                else
                                {
                                    MismatchedSetException mse = new MismatchedSetException(null, input);
                                    Recover(mse);
                                    throw mse;
                                }


                            }
                            break;

                        default:
                            if (cnt20 >= 1) goto loop20;
                            EarlyExitException eee20 =
                                new EarlyExitException(20, input);
                            throw eee20;
                    }
                    cnt20++;
                } while (true);

            loop20:
                ;	// Stops C# compiler whinging that label 'loop20' has no statements

                _channel = HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally
        {
        }
    }
    // $ANTLR end "WS"

    override public void mTokens() // throws RecognitionException 
    {
        // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:8: ( T__30 | T__31 | T__32 | T__33 | T__34 | T__35 | T__36 | T__37 | T__38 | T__39 | T__40 | T__41 | T__42 | T__43 | T__44 | T__45 | T__46 | T__47 | T__48 | T__49 | T__50 | T__51 | T__52 | T__53 | UNIT | IDENT | STRING | NUM | COLOR | SL_COMMENT | COMMENT | WS )
        int alt21 = 32;
        alt21 = dfa21.Predict(input);
        switch (alt21)
        {
            case 1:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:10: T__30
                {
                    mT__30();

                }
                break;
            case 2:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:16: T__31
                {
                    mT__31();

                }
                break;
            case 3:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:22: T__32
                {
                    mT__32();

                }
                break;
            case 4:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:28: T__33
                {
                    mT__33();

                }
                break;
            case 5:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:34: T__34
                {
                    mT__34();

                }
                break;
            case 6:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:40: T__35
                {
                    mT__35();

                }
                break;
            case 7:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:46: T__36
                {
                    mT__36();

                }
                break;
            case 8:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:52: T__37
                {
                    mT__37();

                }
                break;
            case 9:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:58: T__38
                {
                    mT__38();

                }
                break;
            case 10:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:64: T__39
                {
                    mT__39();

                }
                break;
            case 11:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:70: T__40
                {
                    mT__40();

                }
                break;
            case 12:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:76: T__41
                {
                    mT__41();

                }
                break;
            case 13:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:82: T__42
                {
                    mT__42();

                }
                break;
            case 14:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:88: T__43
                {
                    mT__43();

                }
                break;
            case 15:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:94: T__44
                {
                    mT__44();

                }
                break;
            case 16:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:100: T__45
                {
                    mT__45();

                }
                break;
            case 17:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:106: T__46
                {
                    mT__46();

                }
                break;
            case 18:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:112: T__47
                {
                    mT__47();

                }
                break;
            case 19:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:118: T__48
                {
                    mT__48();

                }
                break;
            case 20:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:124: T__49
                {
                    mT__49();

                }
                break;
            case 21:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:130: T__50
                {
                    mT__50();

                }
                break;
            case 22:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:136: T__51
                {
                    mT__51();

                }
                break;
            case 23:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:142: T__52
                {
                    mT__52();

                }
                break;
            case 24:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:148: T__53
                {
                    mT__53();

                }
                break;
            case 25:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:154: UNIT
                {
                    mUNIT();

                }
                break;
            case 26:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:159: IDENT
                {
                    mIDENT();

                }
                break;
            case 27:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:165: STRING
                {
                    mSTRING();

                }
                break;
            case 28:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:172: NUM
                {
                    mNUM();

                }
                break;
            case 29:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:176: COLOR
                {
                    mCOLOR();

                }
                break;
            case 30:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:182: SL_COMMENT
                {
                    mSL_COMMENT();

                }
                break;
            case 31:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:193: COMMENT
                {
                    mCOMMENT();

                }
                break;
            case 32:
                // C:\\Users\\Trihus\\git\\pathway\\pathway\\CssParser\\csst3.g:1:201: WS
                {
                    mWS();

                }
                break;

        }

    }


    protected DFA1 dfa1;
    protected DFA9 dfa9;
    protected DFA12 dfa12;
    protected DFA21 dfa21;
    private void InitializeCyclicDFAs()
    {
        this.dfa1 = new DFA1(this);
        this.dfa9 = new DFA9(this);
        this.dfa12 = new DFA12(this);
        this.dfa21 = new DFA21(this);




    }

    const string DFA1_eotS =
        "\x13\uffff";
    const string DFA1_eofS =
        "\x13\uffff";
    const string DFA1_minS =
        "\x01\x63\x01\x6d\x01\x63\x01\uffff\x01\x6d\x0e\uffff";
    const string DFA1_maxS =
        "\x01\x73\x02\x78\x01\uffff\x01\x73\x0e\uffff";
    const string DFA1_acceptS =
        "\x03\uffff\x01\x03\x01\uffff\x01\x05\x01\x09\x01\x0a\x01\x0b\x01" +
        "\x0d\x01\x0e\x01\x0f\x01\x01\x01\x08\x01\x02\x01\x06\x01\x07\x01" +
        "\x04\x01\x0c";
    const string DFA1_specialS =
        "\x13\uffff}>";
    static readonly string[] DFA1_transitionS = {
            "\x01\x03\x01\x06\x01\x01\x01\uffff\x01\x08\x01\x0a\x01\x05"+
            "\x01\uffff\x01\x0b\x01\uffff\x01\x04\x02\uffff\x01\x02\x01\uffff"+
            "\x01\x07\x01\x09",
            "\x01\x0c\x0a\uffff\x01\x0d",
            "\x01\x10\x10\uffff\x01\x0f\x03\uffff\x01\x0e",
            "",
            "\x01\x11\x05\uffff\x01\x12",
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
            get { return "138:1: UNIT : ( 'em' | 'px' | 'cm' | 'mm' | 'in' | 'pt' | 'pc' | 'ex' | 'deg' | 'rad' | 'grad' | 'ms' | 's' | 'hz' | 'khz' );"; }
        }

    }

    const string DFA9_eotS =
        "\x01\uffff\x01\x03\x02\uffff";
    const string DFA9_eofS =
        "\x04\uffff";
    const string DFA9_minS =
        "\x02\x2e\x02\uffff";
    const string DFA9_maxS =
        "\x02\x39\x02\uffff";
    const string DFA9_acceptS =
        "\x02\uffff\x01\x01\x01\x02";
    const string DFA9_specialS =
        "\x04\uffff}>";
    static readonly string[] DFA9_transitionS = {
            "\x01\x02\x01\uffff\x0a\x01",
            "\x01\x02\x01\uffff\x0a\x01",
            "",
            ""
    };

    static readonly short[] DFA9_eot = DFA.UnpackEncodedString(DFA9_eotS);
    static readonly short[] DFA9_eof = DFA.UnpackEncodedString(DFA9_eofS);
    static readonly char[] DFA9_min = DFA.UnpackEncodedStringToUnsignedChars(DFA9_minS);
    static readonly char[] DFA9_max = DFA.UnpackEncodedStringToUnsignedChars(DFA9_maxS);
    static readonly short[] DFA9_accept = DFA.UnpackEncodedString(DFA9_acceptS);
    static readonly short[] DFA9_special = DFA.UnpackEncodedString(DFA9_specialS);
    static readonly short[][] DFA9_transition = DFA.UnpackEncodedStringArray(DFA9_transitionS);

    protected class DFA9 : DFA
    {
        public DFA9(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 9;
            this.eot = DFA9_eot;
            this.eof = DFA9_eof;
            this.min = DFA9_min;
            this.max = DFA9_max;
            this.accept = DFA9_accept;
            this.special = DFA9_special;
            this.transition = DFA9_transition;

        }

        override public string Description
        {
            get { return "158:8: ( ( '0' .. '9' )* '.' )?"; }
        }

    }

    const string DFA12_eotS =
        "\x01\uffff\x01\x03\x02\uffff";
    const string DFA12_eofS =
        "\x04\uffff";
    const string DFA12_minS =
        "\x02\x2e\x02\uffff";
    const string DFA12_maxS =
        "\x02\x39\x02\uffff";
    const string DFA12_acceptS =
        "\x02\uffff\x01\x01\x01\x02";
    const string DFA12_specialS =
        "\x04\uffff}>";
    static readonly string[] DFA12_transitionS = {
            "\x01\x02\x01\uffff\x0a\x01",
            "\x01\x02\x01\uffff\x0a\x01",
            "",
            ""
    };

    static readonly short[] DFA12_eot = DFA.UnpackEncodedString(DFA12_eotS);
    static readonly short[] DFA12_eof = DFA.UnpackEncodedString(DFA12_eofS);
    static readonly char[] DFA12_min = DFA.UnpackEncodedStringToUnsignedChars(DFA12_minS);
    static readonly char[] DFA12_max = DFA.UnpackEncodedStringToUnsignedChars(DFA12_maxS);
    static readonly short[] DFA12_accept = DFA.UnpackEncodedString(DFA12_acceptS);
    static readonly short[] DFA12_special = DFA.UnpackEncodedString(DFA12_specialS);
    static readonly short[][] DFA12_transition = DFA.UnpackEncodedStringArray(DFA12_transitionS);

    protected class DFA12 : DFA
    {
        public DFA12(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 12;
            this.eot = DFA12_eot;
            this.eof = DFA12_eof;
            this.min = DFA12_min;
            this.max = DFA12_max;
            this.accept = DFA12_accept;
            this.special = DFA12_special;
            this.transition = DFA12_transition;

        }

        override public string Description
        {
            get { return "159:4: ( ( '0' .. '9' )* '.' )?"; }
        }

    }

    const string DFA21_eotS =
        "\x01\uffff\x01\x28\x06\uffff\x01\x2a\x01\x2b\x01\uffff\x01\x2d" +
        "\x08\uffff\x08\x1f\x01\x3a\x02\x1f\x0f\uffff\x09\x3a\x03\x1f\x01" +
        "\uffff\x01\x3a\x01\x1f\x04\uffff\x02\x3a\x01\x1f\x02\x3a";
    const string DFA21_eofS =
        "\x46\uffff";
    const string DFA21_minS =
        "\x01\x09\x01\x69\x06\uffff\x02\x30\x01\uffff\x01\x3a\x08\uffff" +
        "\x01\x6d\x01\x63\x02\x6d\x01\x6e\x01\x65\x01\x61\x01\x72\x01\x2d" +
        "\x01\x7a\x01\x68\x01\uffff\x01\x2e\x02\uffff\x01\x2a\x01\uffff\x01" +
        "\x6d\x08\uffff\x09\x2d\x01\x67\x01\x64\x01\x61\x01\uffff\x01\x2d" +
        "\x01\x7a\x04\uffff\x02\x2d\x01\x64\x02\x2d";
    const string DFA21_maxS =
        "\x01\ufffe\x01\x70\x06\uffff\x01\x66\x01\x39\x01\uffff\x01\x3a" +
        "\x08\uffff\x02\x78\x01\x6d\x01\x73\x01\x6e\x01\x65\x01\x61\x01\x72" +
        "\x01\ufffe\x01\x7a\x01\x68\x01\uffff\x01\ufffe\x02\uffff\x01\x2f" +
        "\x01\uffff\x01\x6e\x08\uffff\x09\ufffe\x01\x67\x01\x64\x01\x61\x01" +
        "\uffff\x01\ufffe\x01\x7a\x04\uffff\x02\ufffe\x01\x64\x02\ufffe";
    const string DFA21_acceptS =
        "\x02\uffff\x01\x03\x01\x05\x01\x06\x01\x09\x01\x0a\x01\x0b\x02" +
        "\uffff\x01\x0e\x01\uffff\x01\x11\x01\x12\x01\x13\x01\x14\x01\x15" +
        "\x01\x16\x01\x17\x01\x18\x0b\uffff\x01\x1a\x01\uffff\x01\x1b\x01" +
        "\x1c\x01\uffff\x01\x20\x01\uffff\x01\x04\x01\x07\x01\x08\x01\x1d" +
        "\x01\x0c\x01\x0d\x01\x10\x01\x0f\x0c\uffff\x01\x19\x02\uffff\x01" +
        "\x1e\x01\x1f\x01\x01\x01\x02\x05\uffff";
    const string DFA21_specialS =
        "\x46\uffff}>";
    static readonly string[] DFA21_transitionS = {
            "\x02\x24\x01\uffff\x02\x24\x12\uffff\x01\x24\x01\uffff\x01"+
            "\x21\x01\x08\x01\uffff\x01\x11\x01\uffff\x01\x21\x01\x12\x01"+
            "\x13\x01\x0a\x01\x07\x01\x05\x01\x20\x01\x09\x01\x23\x0a\x22"+
            "\x01\x0b\x01\x02\x01\uffff\x01\x0e\x01\x06\x01\uffff\x01\x01"+
            "\x1a\x1f\x01\x0c\x01\uffff\x01\x0d\x01\uffff\x01\x1f\x01\uffff"+
            "\x02\x1f\x01\x16\x01\x19\x01\x14\x01\x1f\x01\x1b\x01\x1d\x01"+
            "\x18\x01\x1f\x01\x1e\x01\x1f\x01\x17\x02\x1f\x01\x15\x01\x1f"+
            "\x01\x1a\x01\x1c\x07\x1f\x01\x03\x01\x10\x01\x04\x01\x0f\u0081"+
            "\uffff\ufeff\x1f",
            "\x01\x25\x03\uffff\x01\x26\x02\uffff\x01\x27",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x0a\x29\x07\uffff\x06\x29\x1a\uffff\x06\x29",
            "\x0a\x22",
            "",
            "\x01\x2c",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x2e\x0a\uffff\x01\x2f",
            "\x01\x32\x10\uffff\x01\x31\x03\uffff\x01\x30",
            "\x01\x33",
            "\x01\x34\x05\uffff\x01\x35",
            "\x01\x36",
            "\x01\x37",
            "\x01\x38",
            "\x01\x39",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x3b",
            "\x01\x3c",
            "",
            "\x01\x22\x01\uffff\x0a\x22\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "",
            "",
            "\x01\x3e\x04\uffff\x01\x3d",
            "",
            "\x01\x3f\x01\x40",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x41",
            "\x01\x42",
            "\x01\x43",
            "",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x44",
            "",
            "",
            "",
            "",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x45",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f",
            "\x01\x1f\x02\uffff\x0a\x1f\x07\uffff\x1a\x1f\x04\uffff\x01"+
            "\x1f\x01\uffff\x1a\x1f\u0085\uffff\ufeff\x1f"
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
            get { return "1:1: Tokens : ( T__30 | T__31 | T__32 | T__33 | T__34 | T__35 | T__36 | T__37 | T__38 | T__39 | T__40 | T__41 | T__42 | T__43 | T__44 | T__45 | T__46 | T__47 | T__48 | T__49 | T__50 | T__51 | T__52 | T__53 | UNIT | IDENT | STRING | NUM | COLOR | SL_COMMENT | COMMENT | WS );"; }
        }

    }



}
