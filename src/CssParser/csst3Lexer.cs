// $ANTLR 3.0.1 C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3 2016-03-10 11:03:32
namespace SIL.PublishingSolution.Compiler
{

using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



public class csst3Lexer : Lexer 
{
    public const int PRECEDES = 11;
    public const int PSEUDO = 16;
    public const int CLASS = 22;
    public const int COLOR = 28;
    public const int MEDIA = 5;
    public const int ID = 21;
    public const int Tokens = 57;
    public const int UNIT = 26;
    public const int PROPERTY = 17;
    public const int EM = 23;
    public const int PAGE = 6;
    public const int FUNCTION = 18;
    public const int REGION = 7;
    public const int PARENTOF = 10;
    public const int T32 = 32;
    public const int LINE_COMMENT = 31;
    public const int T34 = 34;
    public const int T33 = 33;
    public const int T36 = 36;
    public const int T35 = 35;
    public const int T38 = 38;
    public const int T37 = 37;
    public const int T39 = 39;
    public const int ANY = 19;
    public const int COMMENT = 30;
    public const int T41 = 41;
    public const int IMPORT = 4;
    public const int HASVALUE = 14;
    public const int T40 = 40;
    public const int T43 = 43;
    public const int T42 = 42;
    public const int T45 = 45;
    public const int T44 = 44;
    public const int T47 = 47;
    public const int T46 = 46;
    public const int T49 = 49;
    public const int T48 = 48;
    public const int RULE = 8;
    public const int WS = 29;
    public const int EOF = -1;
    public const int SIBLING = 12;
    public const int NUM = 27;
    public const int T50 = 50;
    public const int T52 = 52;
    public const int T51 = 51;
    public const int ATTRIBEQUAL = 13;
    public const int T54 = 54;
    public const int T53 = 53;
    public const int IDENT = 25;
    public const int T56 = 56;
    public const int T55 = 55;
    public const int ATTRIB = 9;
    public const int STRING = 24;
    public const int TAG = 20;
    public const int BEGINSWITH = 15;
    const int HIDDEN = Hidden;

    public csst3Lexer() 
    {
		InitializeCyclicDFAs();
    }
    public csst3Lexer(ICharStream input) 
		: base(input)
	{
		InitializeCyclicDFAs();
    }
    
    override public string GrammarFileName
    {
    	get { return "C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3";} 
    }

    // $ANTLR start T32 
    public void mT32() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T32;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:9:5: ( '@import' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:9:7: '@import'
            {
            	Match("@import"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T32

    // $ANTLR start T33 
    public void mT33() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T33;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:10:5: ( '@include' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:10:7: '@include'
            {
            	Match("@include"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T33

    // $ANTLR start T34 
    public void mT34() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T34;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:11:5: ( ';' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:11:7: ';'
            {
            	Match(';'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T34

    // $ANTLR start T35 
    public void mT35() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T35;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:12:5: ( '@media' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:12:7: '@media'
            {
            	Match("@media"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T35

    // $ANTLR start T36 
    public void mT36() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T36;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:13:5: ( '{' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:13:7: '{'
            {
            	Match('{'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T36

    // $ANTLR start T37 
    public void mT37() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T37;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:14:5: ( '}' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:14:7: '}'
            {
            	Match('}'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T37

    // $ANTLR start T38 
    public void mT38() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T38;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:15:5: ( '@page' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:15:7: '@page'
            {
            	Match("@page"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T38

    // $ANTLR start T39 
    public void mT39() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T39;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:16:5: ( '@' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:16:7: '@'
            {
            	Match('@'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T39

    // $ANTLR start T40 
    public void mT40() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T40;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:17:5: ( ',' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:17:7: ','
            {
            	Match(','); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T40

    // $ANTLR start T41 
    public void mT41() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T41;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:18:5: ( '>' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:18:7: '>'
            {
            	Match('>'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T41

    // $ANTLR start T42 
    public void mT42() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T42;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:19:5: ( '+' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:19:7: '+'
            {
            	Match('+'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T42

    // $ANTLR start T43 
    public void mT43() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T43;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:20:5: ( '~' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:20:7: '~'
            {
            	Match('~'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T43

    // $ANTLR start T44 
    public void mT44() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T44;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:21:5: ( '#' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:21:7: '#'
            {
            	Match('#'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T44

    // $ANTLR start T45 
    public void mT45() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T45;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:22:5: ( '.' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:22:7: '.'
            {
            	Match('.'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T45

    // $ANTLR start T46 
    public void mT46() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T46;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:23:5: ( '*' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:23:7: '*'
            {
            	Match('*'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T46

    // $ANTLR start T47 
    public void mT47() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T47;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:24:5: ( ':' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:24:7: ':'
            {
            	Match(':'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T47

    // $ANTLR start T48 
    public void mT48() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T48;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:25:5: ( '::' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:25:7: '::'
            {
            	Match("::"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T48

    // $ANTLR start T49 
    public void mT49() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T49;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:26:5: ( '[' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:26:7: '['
            {
            	Match('['); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T49

    // $ANTLR start T50 
    public void mT50() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T50;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:27:5: ( ']' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:27:7: ']'
            {
            	Match(']'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T50

    // $ANTLR start T51 
    public void mT51() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T51;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:28:5: ( '=' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:28:7: '='
            {
            	Match('='); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T51

    // $ANTLR start T52 
    public void mT52() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T52;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:29:5: ( '~=' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:29:7: '~='
            {
            	Match("~="); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T52

    // $ANTLR start T53 
    public void mT53() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T53;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:30:5: ( '|=' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:30:7: '|='
            {
            	Match("|="); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T53

    // $ANTLR start T54 
    public void mT54() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T54;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:31:5: ( '%' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:31:7: '%'
            {
            	Match('%'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T54

    // $ANTLR start T55 
    public void mT55() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T55;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:32:5: ( '(' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:32:7: '('
            {
            	Match('('); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T55

    // $ANTLR start T56 
    public void mT56() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T56;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:33:5: ( ')' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:33:7: ')'
            {
            	Match(')'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T56

    // $ANTLR start UNIT 
    public void mUNIT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = UNIT;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:11: ( 'em' | 'px' | 'cm' | 'mm' | 'in' | 'pt' | 'pc' | 'ex' | 'deg' | 'rad' | 'grad' | 'ms' | 's' | 'hz' | 'khz' )
            int alt1 = 15;
            switch ( input.LA(1) ) 
            {
            case 'e':
            	{
                int LA1_1 = input.LA(2);
                
                if ( (LA1_1 == 'x') )
                {
                    alt1 = 8;
                }
                else if ( (LA1_1 == 'm') )
                {
                    alt1 = 1;
                }
                else 
                {
                    NoViableAltException nvae_d1s1 =
                        new NoViableAltException("142:1: UNIT : ( 'em' | 'px' | 'cm' | 'mm' | 'in' | 'pt' | 'pc' | 'ex' | 'deg' | 'rad' | 'grad' | 'ms' | 's' | 'hz' | 'khz' );", 1, 1, input);
                
                    throw nvae_d1s1;
                }
                }
                break;
            case 'p':
            	{
                switch ( input.LA(2) ) 
                {
                case 'c':
                	{
                    alt1 = 7;
                    }
                    break;
                case 'x':
                	{
                    alt1 = 2;
                    }
                    break;
                case 't':
                	{
                    alt1 = 6;
                    }
                    break;
                	default:
                	    NoViableAltException nvae_d1s2 =
                	        new NoViableAltException("142:1: UNIT : ( 'em' | 'px' | 'cm' | 'mm' | 'in' | 'pt' | 'pc' | 'ex' | 'deg' | 'rad' | 'grad' | 'ms' | 's' | 'hz' | 'khz' );", 1, 2, input);
                
                	    throw nvae_d1s2;
                }
            
                }
                break;
            case 'c':
            	{
                alt1 = 3;
                }
                break;
            case 'm':
            	{
                int LA1_4 = input.LA(2);
                
                if ( (LA1_4 == 'm') )
                {
                    alt1 = 4;
                }
                else if ( (LA1_4 == 's') )
                {
                    alt1 = 12;
                }
                else 
                {
                    NoViableAltException nvae_d1s4 =
                        new NoViableAltException("142:1: UNIT : ( 'em' | 'px' | 'cm' | 'mm' | 'in' | 'pt' | 'pc' | 'ex' | 'deg' | 'rad' | 'grad' | 'ms' | 's' | 'hz' | 'khz' );", 1, 4, input);
                
                    throw nvae_d1s4;
                }
                }
                break;
            case 'i':
            	{
                alt1 = 5;
                }
                break;
            case 'd':
            	{
                alt1 = 9;
                }
                break;
            case 'r':
            	{
                alt1 = 10;
                }
                break;
            case 'g':
            	{
                alt1 = 11;
                }
                break;
            case 's':
            	{
                alt1 = 13;
                }
                break;
            case 'h':
            	{
                alt1 = 14;
                }
                break;
            case 'k':
            	{
                alt1 = 15;
                }
                break;
            	default:
            	    NoViableAltException nvae_d1s0 =
            	        new NoViableAltException("142:1: UNIT : ( 'em' | 'px' | 'cm' | 'mm' | 'in' | 'pt' | 'pc' | 'ex' | 'deg' | 'rad' | 'grad' | 'ms' | 's' | 'hz' | 'khz' );", 1, 0, input);
            
            	    throw nvae_d1s0;
            }
            
            switch (alt1) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:13: 'em'
                    {
                    	Match("em"); 

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:18: 'px'
                    {
                    	Match("px"); 

                    
                    }
                    break;
                case 3 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:23: 'cm'
                    {
                    	Match("cm"); 

                    
                    }
                    break;
                case 4 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:28: 'mm'
                    {
                    	Match("mm"); 

                    
                    }
                    break;
                case 5 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:33: 'in'
                    {
                    	Match("in"); 

                    
                    }
                    break;
                case 6 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:38: 'pt'
                    {
                    	Match("pt"); 

                    
                    }
                    break;
                case 7 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:43: 'pc'
                    {
                    	Match("pc"); 

                    
                    }
                    break;
                case 8 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:48: 'ex'
                    {
                    	Match("ex"); 

                    
                    }
                    break;
                case 9 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:53: 'deg'
                    {
                    	Match("deg"); 

                    
                    }
                    break;
                case 10 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:59: 'rad'
                    {
                    	Match("rad"); 

                    
                    }
                    break;
                case 11 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:65: 'grad'
                    {
                    	Match("grad"); 

                    
                    }
                    break;
                case 12 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:72: 'ms'
                    {
                    	Match("ms"); 

                    
                    }
                    break;
                case 13 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:77: 's'
                    {
                    	Match('s'); 
                    
                    }
                    break;
                case 14 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:81: 'hz'
                    {
                    	Match("hz"); 

                    
                    }
                    break;
                case 15 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:142:86: 'khz'
                    {
                    	Match("khz"); 

                    
                    }
                    break;
            
            }
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end UNIT

    // $ANTLR start IDENT 
    public void mIDENT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = IDENT;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:145:2: ( ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' ) ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )* | '-' ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' ) ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )* )
            int alt4 = 2;
            int LA4_0 = input.LA(1);
            
            if ( ((LA4_0 >= 'A' && LA4_0 <= 'Z') || LA4_0 == '_' || (LA4_0 >= 'a' && LA4_0 <= 'z') || (LA4_0 >= '\u0100' && LA4_0 <= '\uFFFE')) )
            {
                alt4 = 1;
            }
            else if ( (LA4_0 == '-') )
            {
                alt4 = 2;
            }
            else 
            {
                NoViableAltException nvae_d4s0 =
                    new NoViableAltException("144:1: IDENT : ( ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' ) ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )* | '-' ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' ) ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )* );", 4, 0, input);
            
                throw nvae_d4s0;
            }
            switch (alt4) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:145:4: ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' ) ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )*
                    {
                    	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || (input.LA(1) >= '\u0100' && input.LA(1) <= '\uFFFE') ) 
                    	{
                    	    input.Consume();
                    	
                    	}
                    	else 
                    	{
                    	    MismatchedSetException mse =
                    	        new MismatchedSetException(null,input);
                    	    Recover(mse);    throw mse;
                    	}

                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:146:3: ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )*
                    	do 
                    	{
                    	    int alt2 = 2;
                    	    int LA2_0 = input.LA(1);
                    	    
                    	    if ( (LA2_0 == '-' || (LA2_0 >= '0' && LA2_0 <= '9') || (LA2_0 >= 'A' && LA2_0 <= 'Z') || LA2_0 == '_' || (LA2_0 >= 'a' && LA2_0 <= 'z') || (LA2_0 >= '\u0100' && LA2_0 <= '\uFFFE')) )
                    	    {
                    	        alt2 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt2) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:
                    			    {
                    			    	if ( input.LA(1) == '-' || (input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || (input.LA(1) >= '\u0100' && input.LA(1) <= '\uFFFE') ) 
                    			    	{
                    			    	    input.Consume();
                    			    	
                    			    	}
                    			    	else 
                    			    	{
                    			    	    MismatchedSetException mse =
                    			    	        new MismatchedSetException(null,input);
                    			    	    Recover(mse);    throw mse;
                    			    	}

                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop2;
                    	    }
                    	} while (true);
                    	
                    	loop2:
                    		;	// Stops C# compiler whinging that label 'loop2' has no statements

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:147:4: '-' ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' ) ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )*
                    {
                    	Match('-'); 
                    	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || (input.LA(1) >= '\u0100' && input.LA(1) <= '\uFFFE') ) 
                    	{
                    	    input.Consume();
                    	
                    	}
                    	else 
                    	{
                    	    MismatchedSetException mse =
                    	        new MismatchedSetException(null,input);
                    	    Recover(mse);    throw mse;
                    	}

                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:148:3: ( '_' | '-' | 'a' .. 'z' | 'A' .. 'Z' | '\\u0100' .. '\\ufffe' | '0' .. '9' )*
                    	do 
                    	{
                    	    int alt3 = 2;
                    	    int LA3_0 = input.LA(1);
                    	    
                    	    if ( (LA3_0 == '-' || (LA3_0 >= '0' && LA3_0 <= '9') || (LA3_0 >= 'A' && LA3_0 <= 'Z') || LA3_0 == '_' || (LA3_0 >= 'a' && LA3_0 <= 'z') || (LA3_0 >= '\u0100' && LA3_0 <= '\uFFFE')) )
                    	    {
                    	        alt3 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt3) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:
                    			    {
                    			    	if ( input.LA(1) == '-' || (input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || (input.LA(1) >= '\u0100' && input.LA(1) <= '\uFFFE') ) 
                    			    	{
                    			    	    input.Consume();
                    			    	
                    			    	}
                    			    	else 
                    			    	{
                    			    	    MismatchedSetException mse =
                    			    	        new MismatchedSetException(null,input);
                    			    	    Recover(mse);    throw mse;
                    			    	}

                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop3;
                    	    }
                    	} while (true);
                    	
                    	loop3:
                    		;	// Stops C# compiler whinging that label 'loop3' has no statements

                    
                    }
                    break;
            
            }
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end IDENT

    // $ANTLR start STRING 
    public void mSTRING() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = STRING;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:153:2: ( '\"' ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\"' | '\\n' | '\\r' | '\\\\' ) )* '\"' | '\\'' ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\\'' | '\\n' | '\\r' | '\\\\' ) )* '\\'' )
            int alt7 = 2;
            int LA7_0 = input.LA(1);
            
            if ( (LA7_0 == '\"') )
            {
                alt7 = 1;
            }
            else if ( (LA7_0 == '\'') )
            {
                alt7 = 2;
            }
            else 
            {
                NoViableAltException nvae_d7s0 =
                    new NoViableAltException("152:1: STRING : ( '\"' ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\"' | '\\n' | '\\r' | '\\\\' ) )* '\"' | '\\'' ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\\'' | '\\n' | '\\r' | '\\\\' ) )* '\\'' );", 7, 0, input);
            
                throw nvae_d7s0;
            }
            switch (alt7) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:153:4: '\"' ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\"' | '\\n' | '\\r' | '\\\\' ) )* '\"'
                    {
                    	Match('\"'); 
                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:153:8: ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\"' | '\\n' | '\\r' | '\\\\' ) )*
                    	do 
                    	{
                    	    int alt5 = 3;
                    	    int LA5_0 = input.LA(1);
                    	    
                    	    if ( (LA5_0 == '\\') )
                    	    {
                    	        alt5 = 1;
                    	    }
                    	    else if ( ((LA5_0 >= '\u0000' && LA5_0 <= '\t') || (LA5_0 >= '\u000B' && LA5_0 <= '\f') || (LA5_0 >= '\u000E' && LA5_0 <= '!') || (LA5_0 >= '#' && LA5_0 <= '[') || (LA5_0 >= ']' && LA5_0 <= '\uFFFE')) )
                    	    {
                    	        alt5 = 2;
                    	    }
                    	    
                    	
                    	    switch (alt5) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:153:10: ( '\\\\' ~ ( '\\n' ) )
                    			    {
                    			    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:153:10: ( '\\\\' ~ ( '\\n' ) )
                    			    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:153:11: '\\\\' ~ ( '\\n' )
                    			    	{
                    			    		Match('\\'); 
                    			    		if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\uFFFE') ) 
                    			    		{
                    			    		    input.Consume();
                    			    		
                    			    		}
                    			    		else 
                    			    		{
                    			    		    MismatchedSetException mse =
                    			    		        new MismatchedSetException(null,input);
                    			    		    Recover(mse);    throw mse;
                    			    		}

                    			    	
                    			    	}

                    			    
                    			    }
                    			    break;
                    			case 2 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:154:17: ~ ( '\"' | '\\n' | '\\r' | '\\\\' )
                    			    {
                    			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '!') || (input.LA(1) >= '#' && input.LA(1) <= '[') || (input.LA(1) >= ']' && input.LA(1) <= '\uFFFE') ) 
                    			    	{
                    			    	    input.Consume();
                    			    	
                    			    	}
                    			    	else 
                    			    	{
                    			    	    MismatchedSetException mse =
                    			    	        new MismatchedSetException(null,input);
                    			    	    Recover(mse);    throw mse;
                    			    	}

                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop5;
                    	    }
                    	} while (true);
                    	
                    	loop5:
                    		;	// Stops C# compiler whinging that label 'loop5' has no statements

                    	Match('\"'); 
                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:156:4: '\\'' ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\\'' | '\\n' | '\\r' | '\\\\' ) )* '\\''
                    {
                    	Match('\''); 
                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:156:9: ( ( '\\\\' ~ ( '\\n' ) ) | ~ ( '\\'' | '\\n' | '\\r' | '\\\\' ) )*
                    	do 
                    	{
                    	    int alt6 = 3;
                    	    int LA6_0 = input.LA(1);
                    	    
                    	    if ( (LA6_0 == '\\') )
                    	    {
                    	        alt6 = 1;
                    	    }
                    	    else if ( ((LA6_0 >= '\u0000' && LA6_0 <= '\t') || (LA6_0 >= '\u000B' && LA6_0 <= '\f') || (LA6_0 >= '\u000E' && LA6_0 <= '&') || (LA6_0 >= '(' && LA6_0 <= '[') || (LA6_0 >= ']' && LA6_0 <= '\uFFFE')) )
                    	    {
                    	        alt6 = 2;
                    	    }
                    	    
                    	
                    	    switch (alt6) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:156:11: ( '\\\\' ~ ( '\\n' ) )
                    			    {
                    			    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:156:11: ( '\\\\' ~ ( '\\n' ) )
                    			    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:156:12: '\\\\' ~ ( '\\n' )
                    			    	{
                    			    		Match('\\'); 
                    			    		if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\uFFFE') ) 
                    			    		{
                    			    		    input.Consume();
                    			    		
                    			    		}
                    			    		else 
                    			    		{
                    			    		    MismatchedSetException mse =
                    			    		        new MismatchedSetException(null,input);
                    			    		    Recover(mse);    throw mse;
                    			    		}

                    			    	
                    			    	}

                    			    
                    			    }
                    			    break;
                    			case 2 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:157:17: ~ ( '\\'' | '\\n' | '\\r' | '\\\\' )
                    			    {
                    			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '&') || (input.LA(1) >= '(' && input.LA(1) <= '[') || (input.LA(1) >= ']' && input.LA(1) <= '\uFFFE') ) 
                    			    	{
                    			    	    input.Consume();
                    			    	
                    			    	}
                    			    	else 
                    			    	{
                    			    	    MismatchedSetException mse =
                    			    	        new MismatchedSetException(null,input);
                    			    	    Recover(mse);    throw mse;
                    			    	}

                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    goto loop6;
                    	    }
                    	} while (true);
                    	
                    	loop6:
                    		;	// Stops C# compiler whinging that label 'loop6' has no statements

                    	Match('\''); 
                    
                    }
                    break;
            
            }
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end STRING

    // $ANTLR start NUM 
    public void mNUM() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = NUM;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:162:2: ( ( '-' )? ( '0' .. '9' )+ ( '.' ( '0' .. '9' )+ )? | ( '-' )? '.' ( '0' .. '9' )+ )
            int alt14 = 2;
            switch ( input.LA(1) ) 
            {
            case '-':
            	{
                int LA14_1 = input.LA(2);
                
                if ( (LA14_1 == '.') )
                {
                    alt14 = 2;
                }
                else if ( ((LA14_1 >= '0' && LA14_1 <= '9')) )
                {
                    alt14 = 1;
                }
                else 
                {
                    NoViableAltException nvae_d14s1 =
                        new NoViableAltException("161:1: NUM : ( ( '-' )? ( '0' .. '9' )+ ( '.' ( '0' .. '9' )+ )? | ( '-' )? '.' ( '0' .. '9' )+ );", 14, 1, input);
                
                    throw nvae_d14s1;
                }
                }
                break;
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
            	{
                alt14 = 1;
                }
                break;
            case '.':
            	{
                alt14 = 2;
                }
                break;
            	default:
            	    NoViableAltException nvae_d14s0 =
            	        new NoViableAltException("161:1: NUM : ( ( '-' )? ( '0' .. '9' )+ ( '.' ( '0' .. '9' )+ )? | ( '-' )? '.' ( '0' .. '9' )+ );", 14, 0, input);
            
            	    throw nvae_d14s0;
            }
            
            switch (alt14) 
            {
                case 1 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:162:4: ( '-' )? ( '0' .. '9' )+ ( '.' ( '0' .. '9' )+ )?
                    {
                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:162:4: ( '-' )?
                    	int alt8 = 2;
                    	int LA8_0 = input.LA(1);
                    	
                    	if ( (LA8_0 == '-') )
                    	{
                    	    alt8 = 1;
                    	}
                    	switch (alt8) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:162:4: '-'
                    	        {
                    	        	Match('-'); 
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:162:9: ( '0' .. '9' )+
                    	int cnt9 = 0;
                    	do 
                    	{
                    	    int alt9 = 2;
                    	    int LA9_0 = input.LA(1);
                    	    
                    	    if ( ((LA9_0 >= '0' && LA9_0 <= '9')) )
                    	    {
                    	        alt9 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt9) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:162:10: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    if ( cnt9 >= 1 ) goto loop9;
                    		            EarlyExitException eee =
                    		                new EarlyExitException(9, input);
                    		            throw eee;
                    	    }
                    	    cnt9++;
                    	} while (true);
                    	
                    	loop9:
                    		;	// Stops C# compiler whinging that label 'loop9' has no statements

                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:162:21: ( '.' ( '0' .. '9' )+ )?
                    	int alt11 = 2;
                    	int LA11_0 = input.LA(1);
                    	
                    	if ( (LA11_0 == '.') )
                    	{
                    	    alt11 = 1;
                    	}
                    	switch (alt11) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:162:22: '.' ( '0' .. '9' )+
                    	        {
                    	        	Match('.'); 
                    	        	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:162:26: ( '0' .. '9' )+
                    	        	int cnt10 = 0;
                    	        	do 
                    	        	{
                    	        	    int alt10 = 2;
                    	        	    int LA10_0 = input.LA(1);
                    	        	    
                    	        	    if ( ((LA10_0 >= '0' && LA10_0 <= '9')) )
                    	        	    {
                    	        	        alt10 = 1;
                    	        	    }
                    	        	    
                    	        	
                    	        	    switch (alt10) 
                    	        		{
                    	        			case 1 :
                    	        			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:162:27: '0' .. '9'
                    	        			    {
                    	        			    	MatchRange('0','9'); 
                    	        			    
                    	        			    }
                    	        			    break;
                    	        	
                    	        			default:
                    	        			    if ( cnt10 >= 1 ) goto loop10;
                    	        		            EarlyExitException eee =
                    	        		                new EarlyExitException(10, input);
                    	        		            throw eee;
                    	        	    }
                    	        	    cnt10++;
                    	        	} while (true);
                    	        	
                    	        	loop10:
                    	        		;	// Stops C# compiler whinging that label 'loop10' has no statements

                    	        
                    	        }
                    	        break;
                    	
                    	}

                    
                    }
                    break;
                case 2 :
                    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:163:4: ( '-' )? '.' ( '0' .. '9' )+
                    {
                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:163:4: ( '-' )?
                    	int alt12 = 2;
                    	int LA12_0 = input.LA(1);
                    	
                    	if ( (LA12_0 == '-') )
                    	{
                    	    alt12 = 1;
                    	}
                    	switch (alt12) 
                    	{
                    	    case 1 :
                    	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:163:4: '-'
                    	        {
                    	        	Match('-'); 
                    	        
                    	        }
                    	        break;
                    	
                    	}

                    	Match('.'); 
                    	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:163:13: ( '0' .. '9' )+
                    	int cnt13 = 0;
                    	do 
                    	{
                    	    int alt13 = 2;
                    	    int LA13_0 = input.LA(1);
                    	    
                    	    if ( ((LA13_0 >= '0' && LA13_0 <= '9')) )
                    	    {
                    	        alt13 = 1;
                    	    }
                    	    
                    	
                    	    switch (alt13) 
                    		{
                    			case 1 :
                    			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:163:14: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 
                    			    
                    			    }
                    			    break;
                    	
                    			default:
                    			    if ( cnt13 >= 1 ) goto loop13;
                    		            EarlyExitException eee =
                    		                new EarlyExitException(13, input);
                    		            throw eee;
                    	    }
                    	    cnt13++;
                    	} while (true);
                    	
                    	loop13:
                    		;	// Stops C# compiler whinging that label 'loop13' has no statements

                    
                    }
                    break;
            
            }
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end NUM

    // $ANTLR start COLOR 
    public void mCOLOR() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = COLOR;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:167:2: ( '#' ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' )+ )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:167:4: '#' ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' )+
            {
            	Match('#'); 
            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:167:8: ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' )+
            	int cnt15 = 0;
            	do 
            	{
            	    int alt15 = 2;
            	    int LA15_0 = input.LA(1);
            	    
            	    if ( ((LA15_0 >= '0' && LA15_0 <= '9') || (LA15_0 >= 'A' && LA15_0 <= 'F') || (LA15_0 >= 'a' && LA15_0 <= 'f')) )
            	    {
            	        alt15 = 1;
            	    }
            	    
            	
            	    switch (alt15) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:
            			    {
            			    	if ( (input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'F') || (input.LA(1) >= 'a' && input.LA(1) <= 'f') ) 
            			    	{
            			    	    input.Consume();
            			    	
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse =
            			    	        new MismatchedSetException(null,input);
            			    	    Recover(mse);    throw mse;
            			    	}

            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt15 >= 1 ) goto loop15;
            		            EarlyExitException eee =
            		                new EarlyExitException(15, input);
            		            throw eee;
            	    }
            	    cnt15++;
            	} while (true);
            	
            	loop15:
            		;	// Stops C# compiler whinging that label 'loop15' has no statements

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end COLOR

    // $ANTLR start WS 
    public void mWS() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = WS;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:172:4: ( ( ' ' | '\\t' | '\\r' | '\\n' | '\\f' )+ )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:172:6: ( ' ' | '\\t' | '\\r' | '\\n' | '\\f' )+
            {
            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:172:6: ( ' ' | '\\t' | '\\r' | '\\n' | '\\f' )+
            	int cnt16 = 0;
            	do 
            	{
            	    int alt16 = 2;
            	    int LA16_0 = input.LA(1);
            	    
            	    if ( ((LA16_0 >= '\t' && LA16_0 <= '\n') || (LA16_0 >= '\f' && LA16_0 <= '\r') || LA16_0 == ' ') )
            	    {
            	        alt16 = 1;
            	    }
            	    
            	
            	    switch (alt16) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:
            			    {
            			    	if ( (input.LA(1) >= '\t' && input.LA(1) <= '\n') || (input.LA(1) >= '\f' && input.LA(1) <= '\r') || input.LA(1) == ' ' ) 
            			    	{
            			    	    input.Consume();
            			    	
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse =
            			    	        new MismatchedSetException(null,input);
            			    	    Recover(mse);    throw mse;
            			    	}

            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt16 >= 1 ) goto loop16;
            		            EarlyExitException eee =
            		                new EarlyExitException(16, input);
            		            throw eee;
            	    }
            	    cnt16++;
            	} while (true);
            	
            	loop16:
            		;	// Stops C# compiler whinging that label 'loop16' has no statements

            	channel=HIDDEN;
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end WS

    // $ANTLR start COMMENT 
    public void mCOMMENT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = COMMENT;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:176:2: ( '/*' ( . )* '*/' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:176:4: '/*' ( . )* '*/'
            {
            	Match("/*"); 

            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:176:9: ( . )*
            	do 
            	{
            	    int alt17 = 2;
            	    int LA17_0 = input.LA(1);
            	    
            	    if ( (LA17_0 == '*') )
            	    {
            	        int LA17_1 = input.LA(2);
            	        
            	        if ( (LA17_1 == '/') )
            	        {
            	            alt17 = 2;
            	        }
            	        else if ( ((LA17_1 >= '\u0000' && LA17_1 <= '.') || (LA17_1 >= '0' && LA17_1 <= '\uFFFE')) )
            	        {
            	            alt17 = 1;
            	        }
            	        
            	    
            	    }
            	    else if ( ((LA17_0 >= '\u0000' && LA17_0 <= ')') || (LA17_0 >= '+' && LA17_0 <= '\uFFFE')) )
            	    {
            	        alt17 = 1;
            	    }
            	    
            	
            	    switch (alt17) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:176:9: .
            			    {
            			    	MatchAny(); 
            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop17;
            	    }
            	} while (true);
            	
            	loop17:
            		;	// Stops C# compiler whinging that label 'loop17' has no statements

            	Match("*/"); 

            	channel=HIDDEN;
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end COMMENT

    // $ANTLR start LINE_COMMENT 
    public void mLINE_COMMENT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = LINE_COMMENT;
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:180:2: ( '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n' )
            // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:180:4: '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n'
            {
            	Match("//"); 

            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:180:9: (~ ( '\\n' | '\\r' ) )*
            	do 
            	{
            	    int alt18 = 2;
            	    int LA18_0 = input.LA(1);
            	    
            	    if ( ((LA18_0 >= '\u0000' && LA18_0 <= '\t') || (LA18_0 >= '\u000B' && LA18_0 <= '\f') || (LA18_0 >= '\u000E' && LA18_0 <= '\uFFFE')) )
            	    {
            	        alt18 = 1;
            	    }
            	    
            	
            	    switch (alt18) 
            		{
            			case 1 :
            			    // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:180:9: ~ ( '\\n' | '\\r' )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '\uFFFE') ) 
            			    	{
            			    	    input.Consume();
            			    	
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse =
            			    	        new MismatchedSetException(null,input);
            			    	    Recover(mse);    throw mse;
            			    	}

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop18;
            	    }
            	} while (true);
            	
            	loop18:
            		;	// Stops C# compiler whinging that label 'loop18' has no statements

            	// C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:180:23: ( '\\r' )?
            	int alt19 = 2;
            	int LA19_0 = input.LA(1);
            	
            	if ( (LA19_0 == '\r') )
            	{
            	    alt19 = 1;
            	}
            	switch (alt19) 
            	{
            	    case 1 :
            	        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:180:23: '\\r'
            	        {
            	        	Match('\r'); 
            	        
            	        }
            	        break;
            	
            	}

            	Match('\n'); 
            	channel=HIDDEN;
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end LINE_COMMENT

    override public void mTokens() // throws RecognitionException 
    {
        // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:8: ( T32 | T33 | T34 | T35 | T36 | T37 | T38 | T39 | T40 | T41 | T42 | T43 | T44 | T45 | T46 | T47 | T48 | T49 | T50 | T51 | T52 | T53 | T54 | T55 | T56 | UNIT | IDENT | STRING | NUM | COLOR | WS | COMMENT | LINE_COMMENT )
        int alt20 = 33;
        int LA20_0 = input.LA(1);
        
        if ( (LA20_0 == '@') )
        {
            switch ( input.LA(2) ) 
            {
            case 'i':
            	{
                int LA20_37 = input.LA(3);
                
                if ( (LA20_37 == 'm') )
                {
                    alt20 = 1;
                }
                else if ( (LA20_37 == 'n') )
                {
                    alt20 = 2;
                }
                else 
                {
                    NoViableAltException nvae_d20s37 =
                        new NoViableAltException("1:1: Tokens : ( T32 | T33 | T34 | T35 | T36 | T37 | T38 | T39 | T40 | T41 | T42 | T43 | T44 | T45 | T46 | T47 | T48 | T49 | T50 | T51 | T52 | T53 | T54 | T55 | T56 | UNIT | IDENT | STRING | NUM | COLOR | WS | COMMENT | LINE_COMMENT );", 20, 37, input);
                
                    throw nvae_d20s37;
                }
                }
                break;
            case 'm':
            	{
                alt20 = 4;
                }
                break;
            case 'p':
            	{
                alt20 = 7;
                }
                break;
            	default:
                	alt20 = 8;
                	break;}
        
        }
        else if ( (LA20_0 == ';') )
        {
            alt20 = 3;
        }
        else if ( (LA20_0 == '{') )
        {
            alt20 = 5;
        }
        else if ( (LA20_0 == '}') )
        {
            alt20 = 6;
        }
        else if ( (LA20_0 == ',') )
        {
            alt20 = 9;
        }
        else if ( (LA20_0 == '>') )
        {
            alt20 = 10;
        }
        else if ( (LA20_0 == '+') )
        {
            alt20 = 11;
        }
        else if ( (LA20_0 == '~') )
        {
            int LA20_8 = input.LA(2);
            
            if ( (LA20_8 == '=') )
            {
                alt20 = 21;
            }
            else 
            {
                alt20 = 12;}
        }
        else if ( (LA20_0 == '#') )
        {
            int LA20_9 = input.LA(2);
            
            if ( ((LA20_9 >= '0' && LA20_9 <= '9') || (LA20_9 >= 'A' && LA20_9 <= 'F') || (LA20_9 >= 'a' && LA20_9 <= 'f')) )
            {
                alt20 = 30;
            }
            else 
            {
                alt20 = 13;}
        }
        else if ( (LA20_0 == '.') )
        {
            int LA20_10 = input.LA(2);
            
            if ( ((LA20_10 >= '0' && LA20_10 <= '9')) )
            {
                alt20 = 29;
            }
            else 
            {
                alt20 = 14;}
        }
        else if ( (LA20_0 == '*') )
        {
            alt20 = 15;
        }
        else if ( (LA20_0 == ':') )
        {
            int LA20_12 = input.LA(2);
            
            if ( (LA20_12 == ':') )
            {
                alt20 = 17;
            }
            else 
            {
                alt20 = 16;}
        }
        else if ( (LA20_0 == '[') )
        {
            alt20 = 18;
        }
        else if ( (LA20_0 == ']') )
        {
            alt20 = 19;
        }
        else if ( (LA20_0 == '=') )
        {
            alt20 = 20;
        }
        else if ( (LA20_0 == '|') )
        {
            alt20 = 22;
        }
        else if ( (LA20_0 == '%') )
        {
            alt20 = 23;
        }
        else if ( (LA20_0 == '(') )
        {
            alt20 = 24;
        }
        else if ( (LA20_0 == ')') )
        {
            alt20 = 25;
        }
        else if ( (LA20_0 == 'e') )
        {
            switch ( input.LA(2) ) 
            {
            case 'm':
            	{
                int LA20_48 = input.LA(3);
                
                if ( (LA20_48 == '-' || (LA20_48 >= '0' && LA20_48 <= '9') || (LA20_48 >= 'A' && LA20_48 <= 'Z') || LA20_48 == '_' || (LA20_48 >= 'a' && LA20_48 <= 'z') || (LA20_48 >= '\u0100' && LA20_48 <= '\uFFFE')) )
                {
                    alt20 = 27;
                }
                else 
                {
                    alt20 = 26;}
                }
                break;
            case 'x':
            	{
                int LA20_49 = input.LA(3);
                
                if ( (LA20_49 == '-' || (LA20_49 >= '0' && LA20_49 <= '9') || (LA20_49 >= 'A' && LA20_49 <= 'Z') || LA20_49 == '_' || (LA20_49 >= 'a' && LA20_49 <= 'z') || (LA20_49 >= '\u0100' && LA20_49 <= '\uFFFE')) )
                {
                    alt20 = 27;
                }
                else 
                {
                    alt20 = 26;}
                }
                break;
            	default:
                	alt20 = 27;
                	break;}
        
        }
        else if ( (LA20_0 == 'p') )
        {
            switch ( input.LA(2) ) 
            {
            case 'x':
            	{
                int LA20_50 = input.LA(3);
                
                if ( (LA20_50 == '-' || (LA20_50 >= '0' && LA20_50 <= '9') || (LA20_50 >= 'A' && LA20_50 <= 'Z') || LA20_50 == '_' || (LA20_50 >= 'a' && LA20_50 <= 'z') || (LA20_50 >= '\u0100' && LA20_50 <= '\uFFFE')) )
                {
                    alt20 = 27;
                }
                else 
                {
                    alt20 = 26;}
                }
                break;
            case 't':
            	{
                int LA20_51 = input.LA(3);
                
                if ( (LA20_51 == '-' || (LA20_51 >= '0' && LA20_51 <= '9') || (LA20_51 >= 'A' && LA20_51 <= 'Z') || LA20_51 == '_' || (LA20_51 >= 'a' && LA20_51 <= 'z') || (LA20_51 >= '\u0100' && LA20_51 <= '\uFFFE')) )
                {
                    alt20 = 27;
                }
                else 
                {
                    alt20 = 26;}
                }
                break;
            case 'c':
            	{
                int LA20_52 = input.LA(3);
                
                if ( (LA20_52 == '-' || (LA20_52 >= '0' && LA20_52 <= '9') || (LA20_52 >= 'A' && LA20_52 <= 'Z') || LA20_52 == '_' || (LA20_52 >= 'a' && LA20_52 <= 'z') || (LA20_52 >= '\u0100' && LA20_52 <= '\uFFFE')) )
                {
                    alt20 = 27;
                }
                else 
                {
                    alt20 = 26;}
                }
                break;
            	default:
                	alt20 = 27;
                	break;}
        
        }
        else if ( (LA20_0 == 'c') )
        {
            int LA20_22 = input.LA(2);
            
            if ( (LA20_22 == 'm') )
            {
                int LA20_53 = input.LA(3);
                
                if ( (LA20_53 == '-' || (LA20_53 >= '0' && LA20_53 <= '9') || (LA20_53 >= 'A' && LA20_53 <= 'Z') || LA20_53 == '_' || (LA20_53 >= 'a' && LA20_53 <= 'z') || (LA20_53 >= '\u0100' && LA20_53 <= '\uFFFE')) )
                {
                    alt20 = 27;
                }
                else 
                {
                    alt20 = 26;}
            }
            else 
            {
                alt20 = 27;}
        }
        else if ( (LA20_0 == 'm') )
        {
            switch ( input.LA(2) ) 
            {
            case 'm':
            	{
                int LA20_54 = input.LA(3);
                
                if ( (LA20_54 == '-' || (LA20_54 >= '0' && LA20_54 <= '9') || (LA20_54 >= 'A' && LA20_54 <= 'Z') || LA20_54 == '_' || (LA20_54 >= 'a' && LA20_54 <= 'z') || (LA20_54 >= '\u0100' && LA20_54 <= '\uFFFE')) )
                {
                    alt20 = 27;
                }
                else 
                {
                    alt20 = 26;}
                }
                break;
            case 's':
            	{
                int LA20_55 = input.LA(3);
                
                if ( (LA20_55 == '-' || (LA20_55 >= '0' && LA20_55 <= '9') || (LA20_55 >= 'A' && LA20_55 <= 'Z') || LA20_55 == '_' || (LA20_55 >= 'a' && LA20_55 <= 'z') || (LA20_55 >= '\u0100' && LA20_55 <= '\uFFFE')) )
                {
                    alt20 = 27;
                }
                else 
                {
                    alt20 = 26;}
                }
                break;
            	default:
                	alt20 = 27;
                	break;}
        
        }
        else if ( (LA20_0 == 'i') )
        {
            int LA20_24 = input.LA(2);
            
            if ( (LA20_24 == 'n') )
            {
                int LA20_56 = input.LA(3);
                
                if ( (LA20_56 == '-' || (LA20_56 >= '0' && LA20_56 <= '9') || (LA20_56 >= 'A' && LA20_56 <= 'Z') || LA20_56 == '_' || (LA20_56 >= 'a' && LA20_56 <= 'z') || (LA20_56 >= '\u0100' && LA20_56 <= '\uFFFE')) )
                {
                    alt20 = 27;
                }
                else 
                {
                    alt20 = 26;}
            }
            else 
            {
                alt20 = 27;}
        }
        else if ( (LA20_0 == 'd') )
        {
            int LA20_25 = input.LA(2);
            
            if ( (LA20_25 == 'e') )
            {
                int LA20_57 = input.LA(3);
                
                if ( (LA20_57 == 'g') )
                {
                    int LA20_67 = input.LA(4);
                    
                    if ( (LA20_67 == '-' || (LA20_67 >= '0' && LA20_67 <= '9') || (LA20_67 >= 'A' && LA20_67 <= 'Z') || LA20_67 == '_' || (LA20_67 >= 'a' && LA20_67 <= 'z') || (LA20_67 >= '\u0100' && LA20_67 <= '\uFFFE')) )
                    {
                        alt20 = 27;
                    }
                    else 
                    {
                        alt20 = 26;}
                }
                else 
                {
                    alt20 = 27;}
            }
            else 
            {
                alt20 = 27;}
        }
        else if ( (LA20_0 == 'r') )
        {
            int LA20_26 = input.LA(2);
            
            if ( (LA20_26 == 'a') )
            {
                int LA20_58 = input.LA(3);
                
                if ( (LA20_58 == 'd') )
                {
                    int LA20_68 = input.LA(4);
                    
                    if ( (LA20_68 == '-' || (LA20_68 >= '0' && LA20_68 <= '9') || (LA20_68 >= 'A' && LA20_68 <= 'Z') || LA20_68 == '_' || (LA20_68 >= 'a' && LA20_68 <= 'z') || (LA20_68 >= '\u0100' && LA20_68 <= '\uFFFE')) )
                    {
                        alt20 = 27;
                    }
                    else 
                    {
                        alt20 = 26;}
                }
                else 
                {
                    alt20 = 27;}
            }
            else 
            {
                alt20 = 27;}
        }
        else if ( (LA20_0 == 'g') )
        {
            int LA20_27 = input.LA(2);
            
            if ( (LA20_27 == 'r') )
            {
                int LA20_59 = input.LA(3);
                
                if ( (LA20_59 == 'a') )
                {
                    int LA20_69 = input.LA(4);
                    
                    if ( (LA20_69 == 'd') )
                    {
                        int LA20_71 = input.LA(5);
                        
                        if ( (LA20_71 == '-' || (LA20_71 >= '0' && LA20_71 <= '9') || (LA20_71 >= 'A' && LA20_71 <= 'Z') || LA20_71 == '_' || (LA20_71 >= 'a' && LA20_71 <= 'z') || (LA20_71 >= '\u0100' && LA20_71 <= '\uFFFE')) )
                        {
                            alt20 = 27;
                        }
                        else 
                        {
                            alt20 = 26;}
                    }
                    else 
                    {
                        alt20 = 27;}
                }
                else 
                {
                    alt20 = 27;}
            }
            else 
            {
                alt20 = 27;}
        }
        else if ( (LA20_0 == 's') )
        {
            int LA20_28 = input.LA(2);
            
            if ( (LA20_28 == '-' || (LA20_28 >= '0' && LA20_28 <= '9') || (LA20_28 >= 'A' && LA20_28 <= 'Z') || LA20_28 == '_' || (LA20_28 >= 'a' && LA20_28 <= 'z') || (LA20_28 >= '\u0100' && LA20_28 <= '\uFFFE')) )
            {
                alt20 = 27;
            }
            else 
            {
                alt20 = 26;}
        }
        else if ( (LA20_0 == 'h') )
        {
            int LA20_29 = input.LA(2);
            
            if ( (LA20_29 == 'z') )
            {
                int LA20_61 = input.LA(3);
                
                if ( (LA20_61 == '-' || (LA20_61 >= '0' && LA20_61 <= '9') || (LA20_61 >= 'A' && LA20_61 <= 'Z') || LA20_61 == '_' || (LA20_61 >= 'a' && LA20_61 <= 'z') || (LA20_61 >= '\u0100' && LA20_61 <= '\uFFFE')) )
                {
                    alt20 = 27;
                }
                else 
                {
                    alt20 = 26;}
            }
            else 
            {
                alt20 = 27;}
        }
        else if ( (LA20_0 == 'k') )
        {
            int LA20_30 = input.LA(2);
            
            if ( (LA20_30 == 'h') )
            {
                int LA20_62 = input.LA(3);
                
                if ( (LA20_62 == 'z') )
                {
                    int LA20_70 = input.LA(4);
                    
                    if ( (LA20_70 == '-' || (LA20_70 >= '0' && LA20_70 <= '9') || (LA20_70 >= 'A' && LA20_70 <= 'Z') || LA20_70 == '_' || (LA20_70 >= 'a' && LA20_70 <= 'z') || (LA20_70 >= '\u0100' && LA20_70 <= '\uFFFE')) )
                    {
                        alt20 = 27;
                    }
                    else 
                    {
                        alt20 = 26;}
                }
                else 
                {
                    alt20 = 27;}
            }
            else 
            {
                alt20 = 27;}
        }
        else if ( ((LA20_0 >= 'A' && LA20_0 <= 'Z') || LA20_0 == '_' || (LA20_0 >= 'a' && LA20_0 <= 'b') || LA20_0 == 'f' || LA20_0 == 'j' || LA20_0 == 'l' || (LA20_0 >= 'n' && LA20_0 <= 'o') || LA20_0 == 'q' || (LA20_0 >= 't' && LA20_0 <= 'z') || (LA20_0 >= '\u0100' && LA20_0 <= '\uFFFE')) )
        {
            alt20 = 27;
        }
        else if ( (LA20_0 == '-') )
        {
            int LA20_32 = input.LA(2);
            
            if ( ((LA20_32 >= 'A' && LA20_32 <= 'Z') || LA20_32 == '_' || (LA20_32 >= 'a' && LA20_32 <= 'z') || (LA20_32 >= '\u0100' && LA20_32 <= '\uFFFE')) )
            {
                alt20 = 27;
            }
            else if ( (LA20_32 == '.' || (LA20_32 >= '0' && LA20_32 <= '9')) )
            {
                alt20 = 29;
            }
            else 
            {
                NoViableAltException nvae_d20s32 =
                    new NoViableAltException("1:1: Tokens : ( T32 | T33 | T34 | T35 | T36 | T37 | T38 | T39 | T40 | T41 | T42 | T43 | T44 | T45 | T46 | T47 | T48 | T49 | T50 | T51 | T52 | T53 | T54 | T55 | T56 | UNIT | IDENT | STRING | NUM | COLOR | WS | COMMENT | LINE_COMMENT );", 20, 32, input);
            
                throw nvae_d20s32;
            }
        }
        else if ( (LA20_0 == '\"' || LA20_0 == '\'') )
        {
            alt20 = 28;
        }
        else if ( ((LA20_0 >= '0' && LA20_0 <= '9')) )
        {
            alt20 = 29;
        }
        else if ( ((LA20_0 >= '\t' && LA20_0 <= '\n') || (LA20_0 >= '\f' && LA20_0 <= '\r') || LA20_0 == ' ') )
        {
            alt20 = 31;
        }
        else if ( (LA20_0 == '/') )
        {
            int LA20_36 = input.LA(2);
            
            if ( (LA20_36 == '*') )
            {
                alt20 = 32;
            }
            else if ( (LA20_36 == '/') )
            {
                alt20 = 33;
            }
            else 
            {
                NoViableAltException nvae_d20s36 =
                    new NoViableAltException("1:1: Tokens : ( T32 | T33 | T34 | T35 | T36 | T37 | T38 | T39 | T40 | T41 | T42 | T43 | T44 | T45 | T46 | T47 | T48 | T49 | T50 | T51 | T52 | T53 | T54 | T55 | T56 | UNIT | IDENT | STRING | NUM | COLOR | WS | COMMENT | LINE_COMMENT );", 20, 36, input);
            
                throw nvae_d20s36;
            }
        }
        else 
        {
            NoViableAltException nvae_d20s0 =
                new NoViableAltException("1:1: Tokens : ( T32 | T33 | T34 | T35 | T36 | T37 | T38 | T39 | T40 | T41 | T42 | T43 | T44 | T45 | T46 | T47 | T48 | T49 | T50 | T51 | T52 | T53 | T54 | T55 | T56 | UNIT | IDENT | STRING | NUM | COLOR | WS | COMMENT | LINE_COMMENT );", 20, 0, input);
        
            throw nvae_d20s0;
        }
        switch (alt20) 
        {
            case 1 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:10: T32
                {
                	mT32(); 
                
                }
                break;
            case 2 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:14: T33
                {
                	mT33(); 
                
                }
                break;
            case 3 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:18: T34
                {
                	mT34(); 
                
                }
                break;
            case 4 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:22: T35
                {
                	mT35(); 
                
                }
                break;
            case 5 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:26: T36
                {
                	mT36(); 
                
                }
                break;
            case 6 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:30: T37
                {
                	mT37(); 
                
                }
                break;
            case 7 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:34: T38
                {
                	mT38(); 
                
                }
                break;
            case 8 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:38: T39
                {
                	mT39(); 
                
                }
                break;
            case 9 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:42: T40
                {
                	mT40(); 
                
                }
                break;
            case 10 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:46: T41
                {
                	mT41(); 
                
                }
                break;
            case 11 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:50: T42
                {
                	mT42(); 
                
                }
                break;
            case 12 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:54: T43
                {
                	mT43(); 
                
                }
                break;
            case 13 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:58: T44
                {
                	mT44(); 
                
                }
                break;
            case 14 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:62: T45
                {
                	mT45(); 
                
                }
                break;
            case 15 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:66: T46
                {
                	mT46(); 
                
                }
                break;
            case 16 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:70: T47
                {
                	mT47(); 
                
                }
                break;
            case 17 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:74: T48
                {
                	mT48(); 
                
                }
                break;
            case 18 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:78: T49
                {
                	mT49(); 
                
                }
                break;
            case 19 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:82: T50
                {
                	mT50(); 
                
                }
                break;
            case 20 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:86: T51
                {
                	mT51(); 
                
                }
                break;
            case 21 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:90: T52
                {
                	mT52(); 
                
                }
                break;
            case 22 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:94: T53
                {
                	mT53(); 
                
                }
                break;
            case 23 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:98: T54
                {
                	mT54(); 
                
                }
                break;
            case 24 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:102: T55
                {
                	mT55(); 
                
                }
                break;
            case 25 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:106: T56
                {
                	mT56(); 
                
                }
                break;
            case 26 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:110: UNIT
                {
                	mUNIT(); 
                
                }
                break;
            case 27 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:115: IDENT
                {
                	mIDENT(); 
                
                }
                break;
            case 28 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:121: STRING
                {
                	mSTRING(); 
                
                }
                break;
            case 29 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:128: NUM
                {
                	mNUM(); 
                
                }
                break;
            case 30 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:132: COLOR
                {
                	mCOLOR(); 
                
                }
                break;
            case 31 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:138: WS
                {
                	mWS(); 
                
                }
                break;
            case 32 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:141: COMMENT
                {
                	mCOMMENT(); 
                
                }
                break;
            case 33 :
                // C:\\Users\\Trihus\\git\\pathway\\src\\CssParser\\csst3.g3:1:149: LINE_COMMENT
                {
                	mLINE_COMMENT(); 
                
                }
                break;
        
        }
    
    }


	private void InitializeCyclicDFAs()
	{
	}

 
    
}
}