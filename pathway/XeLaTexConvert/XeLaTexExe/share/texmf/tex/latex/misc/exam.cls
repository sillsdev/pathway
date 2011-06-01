% exam.cls
%
% A LaTeX2e document class for preparing exams.

%%% Copyright (c) 2000 Philip S. Hirschhorn
%
% This program may be distributed and/or modified under the
% conditions of the LaTeX Project Public License, either version 1.2
% of this license or (at your option) any later version.
% The latest version of this license is in
%   http://www.latex-project.org/lppl.txt
% and version 1.2 or later is part of all distributions of LaTeX 
% version 1999/12/01 or later.
%
% This program consists of the files exam.cls and examdoc.tex

% The user documentation for exam.cls is in the file examdoc.tex.


%%% Philip Hirschhorn
%%% Department of Mathematics
%%% Wellesley College
%%% Wellesley, MA 02481
%%% psh@poincare.wellesley.edu
%%% psh@math.mit.edu

% The newest version of this documentclass should always be available
% from my web page: http://www-math.mit.edu/~psh/


\def\fileversion{2.01}
\def\filedate{2000/10/10}

%--------------------------------------------------------------------
%--------------------------------------------------------------------
%
% The only change from version 2.0 to version 2.01 is that this
% documentclass (and its accompanying documentation) is now
% explicitly distributed under the LaTeX Project Public License.
%
%---------------------------------------------------------------------
%---------------------------------------------------------------------
% PLEASE DO NOT MAKE ANY CHANGES TO THIS FILE!
% 
% If you wish to make changes to this file, rename this file
% to something other than exam.cls BEFORE YOU MAKE THE CHANGES!
% 
% If there's some feature that you'd like that this file doesn't
% provide, tell me about it.
% 
%
%
%
%
% Thanks to:
%
% Piet van Oostrum, from whose excellent ``fancyheadings.sty'' we
% shamelessly stole most of the code for setting the headers and
% footers.
%
% Mate Wierdl <mw@wierdlmpc.msci.memphis.edu>, who contributed the
% code so that if the number of points is ``1'', then the default
% value of \pointname will print ``1 point'' instead of ``1 points''.
%
% Tom Brikowski <brikowi@utdallas.edu>, who contributed the code for
% making the number of points and number of questions available as
% macros (as well as the idea of putting the number of points in a 
% box, instead of in parentheses).  (I changed his code to make this
% all optional, so if there are errors there, it's my fault and not
% his.)
%
%--------------------------------------------------------------------
%--------------------------------------------------------------------

\NeedsTeXFormat{LaTeX2e}

\ProvidesClass{exam}[\filedate\space Version \fileversion\space by
  Philip Hirschhorn]

\RequirePackage{ifthen}

\DeclareOption*{%
  \PassOptionsToClass{\CurrentOption}{article}%
}
\ProcessOptions
\LoadClass{article}




%                         *****************
%                         ** PAGE LAYOUT **
%                         *****************


\setlength{\textwidth}{6.5in}
\setlength{\oddsidemargin}{0pt}
\setlength{\evensidemargin}{0pt}


\setlength{\headheight}{15pt}
\setlength{\headsep}{15pt}
\setlength{\topmargin}{0in}
\addtolength{\topmargin}{-\headheight}
\addtolength{\topmargin}{-\headsep}
\setlength{\textheight}{8.8in}
\setlength{\footskip}{29pt}
\setlength{\marginparwidth}{.5in}
\setlength{\marginparsep}{5pt}

%--------------------------------------------------------------------

%                          ****************
%                          ** EXTRAWIDTH **
%                          ****************

\newlength\@extrawidth

% We put the argument of \extrawidth into a length so that it will
% work correctly even if it's negative:

\def\extrawidth#1{%
  \@extrawidth=#1
  \advance \textwidth by \@extrawidth
  \divide\@extrawidth by 2
  \advance\oddsidemargin by -\@extrawidth
  \advance\evensidemargin by -\@extrawidth
}




%--------------------------------------------------------------------
%--------------------------------------------------------------------
%             Making room for large headers and footers

% The following are used to save the effect of any changes to
% \topmargin and \textheight caused by \extraheadheight or
% \extrafootheight commands.  They hold the value given in the most
% recent command.  We put them into a length so that it will work
% correctly even if the argument is negative: 

\newlength\@extrahead
\newlength\@extrafoot
\setlength{\@extrahead}{0in}
\setlength{\@extrafoot}{0in}


\def\extraheadheight{%
  \@ifnextchar[{\@doextraheads}%
               {\@setheadheight}%
}

\def\@doextraheads[#1]#2{%
  \@setheadheight{#1}%
  \@appendoutput{\ifnum\thepage=2\@setheadheight{#2}\fi}%
}


\def\extrafootheight{%
  \@ifnextchar[{\@doextrafeet}%
               {\@setfootheight}%
}

\def\@doextrafeet[#1]#2{%
  \@setfootheight{#1}%
  \@appendoutput{\ifnum\thepage=2\@setfootheight{#2}\fi}%
}

\def\@appendoutput#1{%
  \output=\expandafter{\the\output #1}%
}

%--------------------------------------------------------------------
%                 \setheadheight and \setfootheight:

\def\@setheadheight#1{%
  \begingroup % Avoid trouble from using \@temp and \@spaces
    % Reset the effect of the most recent change:
    \global\advance\topmargin by -\@extrahead
    \global\advance\textheight by \@extrahead
    % Save the newly set value:
    \def\@temp{#1}
    \def\@spaces{ }
    \ifx\@temp\@empty
      \global\@extrahead=0in
    \else
      \ifx\@temp\@spaces
        \global\@extrahead=0in
      \else
        \global\@extrahead=#1
      \fi
    \fi
    % Set the new values:
    \global\advance\topmargin by \@extrahead
    \global\advance\textheight by -\@extrahead
    % Make it take effect RIGHT NOW!:
    % (The following stuff isn't necessary if \@setheadheight is
    % executed only in the preamble or as we return from the output
    % routine, but we're leaving it in so that this will still work if
    % we use this at some random point in the middle of composing a
    % page). 
    \global\@colht=\textheight
    \global\@colroom=\textheight
    \global\vsize=\textheight
    \global\pagegoal=\textheight
  \endgroup
}

\def\@setfootheight#1{%
  \begingroup % Avoid trouble from using \@temp and \@spaces
    % Reset the effect of the most recent change:
    \global\advance\textheight by \@extrafoot
    % Save the newly set value:
    \def\@temp{#1}
    \def\@spaces{ }
    \ifx\@temp\@empty
      \global\@extrafoot=0in
    \else
      \ifx\@temp\@spaces
        \global\@extrafoot=0in
      \else
        \global\@extrafoot=#1
      \fi
    \fi
    % Set the new values:
    \global\advance\textheight by -\@extrafoot
    % Make it take effect RIGHT NOW!:
    % (The following stuff isn't necessary if \@setfootheight is
    % executed only in the preamble or as we return from the output
    % routine, but we're leaving it in so that this will still work if
    % we use this at some random point in the middle of composing a
    % page). 
    \global\@colht=\textheight
    \global\@colroom=\textheight
    \global\vsize=\textheight
    \global\pagegoal=\textheight
  \endgroup
}




%---------------------------------------------------------------------
%
%                      *************************
%                      ** HEADERS AND FOOTERS **
%                      *************************
% 
% The pagestyles available are head, foot, headandfoot, and empty.
% \pagestyle{head} prints the head, and gives an empty foot.
% \pagestyle{foot} prints the foot, and gives an empty head.
% \pagestyle{headandfoot} prints both the head and the foot.
% \pagestyle{empty} gives an empty head and an empty foot.
% 

%                            Pagestyles:

\def\ps@head{%
  \@dohead
  \@nofoot
}

\def\ps@headandfoot{%
  \@dohead
  \@dofoot
}

\def\ps@foot{%
  \@nohead
  \@dofoot
}

\def\ps@empty{%
  \@nohead
  \@nofoot
}



\def\@dohead{%
  \def\@oddhead{%
    \ifnum\value{page}=1
      \@fullhead
    \else
      \r@fullhead
    \fi
  }% @oddhead
  \let\@evenhead=\@oddhead
}

\def\@dofoot{%
  \def\@oddfoot{%
    \ifnum\value{page}=1
      \@fullfoot
    \else
      \r@fullfoot
    \fi
  }% @oddfoot
  \let\@evenfoot=\@oddfoot

}

\def\@nohead{%
  \def\@oddhead{}%
  \let\@evenhead=\@oddhead
}

\def\@nofoot{%
  \def\@oddfoot{}%
  \let\@evenfoot=\@oddfoot
}




%--------------------------------------------------------------------
%       \@fullhead, \r@fullhead, \@fullfoot, and \r@fullfoot:

\def\@fullhead{%
  \vbox to \headheight{%
    \vss
    \hbox to \textwidth{%
      \rm\rlap{\parbox[b]{\textwidth}{\raggedright\@lhead\strut}}%
        \hss\parbox[b]{\textwidth}{\centering\@chead\strut}\hss
        \llap{\parbox[b]{\textwidth}{\raggedleft\@rhead\strut}}%
    }% hbox
    \if@headrule
      \hrule
    \else
      % an invisible hrule, to keep positioning constant:
      \hrule width 0pt
    \fi
  }% vbox
}


\def\r@fullhead{%
  \vbox to \headheight{%
    \vss
    \hbox to \textwidth{%
      \rm\rlap{\parbox[b]{\textwidth}{\raggedright\r@lhead\strut}}%
        \hss\parbox[b]{\textwidth}{\centering\r@chead\strut}\hss
        \llap{\parbox[b]{\textwidth}{\raggedleft\r@rhead\strut}}%
    }% hbox
    \ifr@headrule
      \hrule
    \else
      % an invisible hrule, to keep positioning constant:
      \hrule width 0pt
    \fi
  }% vbox
}



% We arrange it so that the very top of first line of text in the
% foot is at a fixed position on the page, whether or not there's
% a footrule:

\def\@fullfoot{%
  \vbox to 0pt{%
    \if@footrule
      \hrule
    \else
      % an invisible hrule, to keep positioning constant:
      \hrule width 0pt
    \fi
    \vskip 3pt
    \hbox to \textwidth{%
      \rm\rlap{\parbox[t]{\textwidth}{\raggedright\@lfoot}}%
        \hss\parbox[t]{\textwidth}{\centering\@cfoot}\hss
        \llap{\parbox[t]{\textwidth}{\raggedleft\@rfoot}}%
    }% hbox
    \vss
  }% vbox
}


\def\r@fullfoot{%
  \vbox to 0pt{%
    \ifr@footrule
      \hrule
    \else
      % an invisible hrule, to keep positioning constant:
      \hrule width 0pt
    \fi
    \vskip 3pt
    \hbox to \textwidth{%
      \rm\rlap{\parbox[t]{\textwidth}{\raggedright\r@lfoot}}%
        \hss\parbox[t]{\textwidth}{\centering\r@cfoot}\hss
        \llap{\parbox[t]{\textwidth}{\raggedleft\r@rfoot}}%
    }% hbox
    \vss
  }% vbox
}


%--------------------------------------------------------------------
%--------------------------------------------------------------------
%
%            ********************************************
%            ** COMMANDS TO DEFINE HEADERS AND FOOTERS **
%            ********************************************
%
% \lhead[#1]{#2} sets the first page left head to #1, and the
%   running left head to #2
% 
% \lhead{#1} sets both the first page left head and the running
%   left head to #1
% 
% \chead, \rhead, \lfoot, \cfoot, and \rfoot work similarly.
% 
% 
% \@lhead is the left head for Page 1
% \r@lhead is the running left head
% (i.e., for all pages other than the first)
% 
% \@chead is the center head for Page 1
% \r@chead is the running center head
% (i.e., for all pages other than the first)
% 
% etc.
% 
% Alternative commands are:
% \firstpageheader{LEFT}{CENTER}{RIGHT}
% \runningheader{LEFT}{CENTER}{RIGHT}
% or
% \header{LEFT}{CENTER}{RIGHT}
% which is equivalent to the two commands
%          \firstpageheader{LEFT}{CENTER}{RIGHT}
%          \runningheader{LEFT}{CENTER}{RIGHT}
% 
% Alternative commands are:
% \firstpagefooter{LEFT}{CENTER}{RIGHT}
% \runningfoother{LEFT}{CENTER}{RIGHT}
% or
% \footer{LEFT}{CENTER}{RIGHT}
% which is equivalent to the two commands
%          \firstpagefooter{LEFT}{CENTER}{RIGHT}
%          \runningfoother{LEFT}{CENTER}{RIGHT}

\def\firstpageheader#1#2#3{%
  \def\@lhead{#1}%
  \def\@chead{#2}%
  \def\@rhead{#3}%
}

\def\runningheader#1#2#3{%
  \def\r@lhead{#1}%
  \def\r@chead{#2}%
  \def\r@rhead{#3}%
}

\def\header#1#2#3{%
  \firstpageheader{#1}{#2}{#3}%
  \runningheader{#1}{#2}{#3}%
}


\def\firstpagefooter#1#2#3{%
  \def\@lfoot{#1}%
  \def\@cfoot{#2}%
  \def\@rfoot{#3}%
}

\def\runningfooter#1#2#3{%
  \def\r@lfoot{#1}%
  \def\r@cfoot{#2}%
  \def\r@rfoot{#3}%
}

\def\footer#1#2#3{%
  \firstpagefooter{#1}{#2}{#3}%
  \runningfooter{#1}{#2}{#3}%
}



\def\lhead{\@ifnextchar[{\@xlhead}{\@ylhead}}
\def\@xlhead[#1]#2{\def\@lhead{#1}\def\r@lhead{#2}}
\def\@ylhead#1{\def\r@lhead{#1}\def\@lhead{#1}}

\def\chead{\@ifnextchar[{\@xchead}{\@ychead}}
\def\@xchead[#1]#2{\def\@chead{#1}\def\r@chead{#2}}
\def\@ychead#1{\def\r@chead{#1}\def\@chead{#1}}

\def\rhead{\@ifnextchar[{\@xrhead}{\@yrhead}}
\def\@xrhead[#1]#2{\def\@rhead{#1}\def\r@rhead{#2}}
\def\@yrhead#1{\def\r@rhead{#1}\def\@rhead{#1}}

\def\lfoot{\@ifnextchar[{\@xlfoot}{\@ylfoot}}
\def\@xlfoot[#1]#2{\def\@lfoot{#1}\def\r@lfoot{#2}}
\def\@ylfoot#1{\def\r@lfoot{#1}\def\@lfoot{#1}}

\def\cfoot{\@ifnextchar[{\@xcfoot}{\@ycfoot}}
\def\@xcfoot[#1]#2{\def\@cfoot{#1}\def\r@cfoot{#2}}
\def\@ycfoot#1{\def\r@cfoot{#1}\def\@cfoot{#1}}

\def\rfoot{\@ifnextchar[{\@xrfoot}{\@yrfoot}}
\def\@xrfoot[#1]#2{\def\@rfoot{#1}\def\r@rfoot{#2}}
\def\@yrfoot#1{\def\r@rfoot{#1}\def\@rfoot{#1}}


%                    Initialize head and foot:



\pagestyle{headandfoot}

\lhead{}
\chead{}
\rhead{}
\lfoot{}
\cfoot[]{Page \thepage}
\rfoot{}




%--------------------------------------------------------------------
%--------------------------------------------------------------------

%                      Headrules and footrules:

\newif\if@headrule
\newif\ifr@headrule

\def\firstpageheadrule{\@headruletrue}
\def\nofirstpageheadrule{\@headrulefalse}

\def\runningheadrule{\r@headruletrue}
\def\norunningheadrule{\r@headrulefalse}

\def\headrule{\@headruletrue\r@headruletrue}
\def\noheadrule{\@headrulefalse\r@headrulefalse}



\newif\if@footrule
\newif\ifr@footrule

\def\firstpagefootrule{\@footruletrue}
\def\nofirstpagefootrule{\@footrulefalse}

\def\runningfootrule{\r@footruletrue}
\def\norunningfootrule{\r@footrulefalse}

\def\footrule{\@footruletrue\r@footruletrue}
\def\nofootrule{\@footrulefalse\r@footrulefalse}



%                             Initialize:

\noheadrule
\nofootrule



%--------------------------------------------------------------------
%--------------------------------------------------------------------

%                \numpages, \iflastpage, and \oddeven
%     Also: \numpoints, \numquestions, \numparts, and \numsubparts

% Make the number of pages available as the macro \numpages,
% the number of points as \numpoints,
% the number of questions as \numquestions,
% the number of parts as \numparts, and
% the number of subparts as \numsubparts
\def\numpages{\pageref{@lastpage}}
\def\numpoints{\pageref{@numpoints}}
\def\numquestions{\pageref{@numquestions}}
\def\numparts{\pageref{@numparts}}
\def\numsubparts{\pageref{@numsubparts}}

%%%\let\@realenddocument=\enddocument
%%%\def\enddocument{\clearpage
%%%   \if@filesw 
%%%     {\advance\c@page-1 \immediate\write\@mainaux
%%%       {\string\newlabel{@lastpage}{{}{\arabic{page}}}}%
%%%     }
%%%   \fi
%%%   \@realenddocument
%%%}

\AtEndDocument{\clearpage
   \if@filesw 
     {\advance\c@page-1 \immediate\write\@mainaux
       {\string\newlabel{@lastpage}{{}{\arabic{page}}}}%
       \advance\c@page+1 % In case some other package looks at \c@page
       \immediate\write\@mainaux
       {\string\newlabel{@numpoints}{{}{\thenumpoints}}}%
       \immediate\write\@mainaux
       {\string\newlabel{@numquestions}{{}{\thenumquestions}}}%
       \immediate\write\@mainaux
       {\string\newlabel{@numparts}{{}{\thenumparts}}}%
       \immediate\write\@mainaux
       {\string\newlabel{@numsubparts}{{}{\thenumsubparts}}}%
     }
   \fi
   % Echo numbers of questions, parts, and subparts:
   \typeout{This exam contains \thenumquestions\space questions
     with \thenumparts\space parts and
     \thenumsubparts\space subparts.}
   % If counting points, echo total points:
   \if@printtotalpoints
     \typeout{This exam has a total of \thenumpoints\space points.}
   \fi
}


% We define \iflastpage so that it can safely be used
% in headers and footers:
\def\iflastpage#1#2{%
      \@ifundefined{r@@lastpage}{\def\@@lastpage{-1}}%
              {\edef\@@lastpage{\expandafter\@cdr\r@@lastpage\@nil}}%
      \ifnum\value{page}=\@@lastpage
        #1%
      \else
        #2%
      \fi
}


% The macro \oddeven takes two arguments.  If the page number is odd,
% then you get the first argument; otherwise, you get the second
% argument.
\def\oddeven#1#2{%
  \ifodd\value{page}%
    #1
  \else
    #2
  \fi
}







%---------------------------------------------------------------------
%
%                    ***************************
%                    ** QUESTION ENVIRONMENTS **
%                    ***************************
%
%
%
% We define the command \part only inside of a parts environment,
% so that we don't interfere with the meaning of the standard
% article documentclass command \part if that is used inside of a
% questions environment.  The commands \question and \subpart are
% defined everywhere inside of a questions environment.  (If the 
% user accidentally gives a \subpart command outside of a subparts
% environment, then a warning will be printed, and the \subpart
% command will be treated as either a \question command or a
% \part command (depending on the current environment).




% We use the counter name `partno' for the parts environment so that
% we will not interfere with the counter `part' used by the article
% document class.

%%%\@definecounter{question}
%%%\@definecounter{partno}
%%%\@definecounter{subpart}
\newcounter{question}
\newcounter{partno}
\newcounter{subpart}
\newcounter{numpoints}
\newcounter{numquestions}
\newcounter{numparts}
\newcounter{numsubparts}


\newenvironment{questions}%
  {\def\@queslevel{question}%
    \def\question{%
      \@checkqueslevel{question}%
      \addtocounter{numquestions}{1}%
      \@doitem
    }%
    \def\subpart{%
      \@checkqueslevel{subpart}%
      \addtocounter{numsubparts}{1}%
      \@doitem
    }%
    \list{\thequestion.}%
    {\usecounter{question}\def\makelabel##1{\hss\llap{##1}}%
      \def\thequestion{\arabic{question}}%
      \settowidth{\leftmargin}{10.\hskip\labelsep}
      \labelwidth\leftmargin\advance\labelwidth-\labelsep
      \partopsep=0pt
    }%
  }%
  {\endlist}

\newenvironment{parts}%
  {\def\@queslevel{part}%
     \def\part{%
       \@checkqueslevel{part}%
       \addtocounter{numparts}{1}%
       \@doitem
     }%
     \list{(\thepartno)}%
     {\usecounter{partno}\def\makelabel##1{\hss\llap{##1}}%
       \def\thepartno{\alph{partno}}%
       \settowidth{\leftmargin}{(m)\hskip\labelsep}
       \labelwidth\leftmargin\advance\labelwidth-\labelsep
       \topsep=0pt
       \partopsep=0pt
     }%
  }%
  {\endlist}

\newenvironment{subparts}%
  {\def\@queslevel{subpart}%
     \list{\thesubpart.}%
     {\usecounter{subpart}\def\makelabel##1{\hss\llap{##1}}%
       \def\thesubpart{\roman{subpart}}%
       \settowidth{\leftmargin}{vii.\hskip\labelsep}
       \labelwidth\leftmargin\advance\labelwidth-\labelsep
       \topsep=0pt
       \partopsep=0pt
     }%
  }%
  {\endlist}


\def\@checkqueslevel#1{%
   \begingroup
     \def\@temp{#1}%
     \ifx\@temp\@queslevel
       % Everything's fine; do nothing.
     \else
       \@warning{I found a #1 where I expected to find a \@queslevel}
     \fi
   \endgroup
}


\def\@doitem{\@ifnextchar[{\@readpoints}{\item\@setpoints}}

\def\@readpoints[#1]{%
  \edef\@points{#1}%
  \@placepointstrue
  \if@addpoints
    \addtocounter{numpoints}{\@points}
  \fi
  \item
  \@setpoints
}

\def\@setpoints{%
  % The \item command sets \everypar so that the first time we enter
  % horizontal mode (which will cause \everypar to be added to the
  % paragraph), the label (or labels, if, e.g., a \question begins
  % with a parts environment) will be placed on the page, a couple
  % of other bookkeeping chores are done, and then \everypar is
  % set to the empty token string (so that none of this will be
  % repeated for the following paragraphs).
  % 
  % We check to see if there are points to be placed for this item.
  % if so, we append either
  %   \llap{(\@points)\hskip\@totalleftmargin\hskip\marginpointssep}%
  %         \@placepointsfalse
  % or
  %   (\@points\@pointname)\enspace\@placepointsfalse
  % to \everypar, so that the number of points will be set whenever
  % we first enter horizontal mode, and the flag to set points will
  % be turned off.  (Actually, that's what we do if \if@boxedpoints is
  % false; if it's true, we do the analgoous thing with commands that
  % use \fbox instead of parentheses.)  (Resetting the flag is put
  % into \everypar so that if another \item is encounteres before we
  % actually enter horizontal mode, we'll put this back into the new
  % version of \everypar that will be created by that \item.)
  \if@placepoints
    \if@pointsinmargin
      \if@boxedpoints
        \everypar=\expandafter{\the\everypar
            \llap{\fbox{\@points\@marginpointname}\hskip\@totalleftmargin
                  \hskip\marginpointssep}%
            \@placepointsfalse}%
      \else
        \everypar=\expandafter{\the\everypar
            \llap{(\@points\@marginpointname)\hskip\@totalleftmargin
                  \hskip\marginpointssep}%
            \@placepointsfalse}%
      \fi
    \else
      \if@boxedpoints
        \everypar=\expandafter{\the\everypar
            \fbox{\@points\@pointname}\enspace\@placepointsfalse}%
      \else
        \everypar=\expandafter{\the\everypar
            (\@points\@pointname)\enspace\@placepointsfalse}%
      \fi
    \fi
  \fi
}% @setpoints

\newif\if@placepoints
\@placepointsfalse



% If the user says \pointsinmargin, then the distance from
% the right parentheses enclosing the points to the left margin
% will be \marginpointssep:
\newskip\marginpointssep
\setlength{\marginpointssep}{5pt}

\newif\if@pointsinmargin
\def\pointsinmargin{\global\@pointsinmargintrue}
\def\nopointsinmargin{\global\@pointsinmarginfalse}
\@pointsinmarginfalse

\newif\if@boxedpoints
\def\boxedpoints{\global\@boxedpointstrue}
\def\noboxedpoints{\global\@boxedpointsfalse}
\@boxedpointsfalse



\def\pointname#1{\gdef\@pointname{#1}}
% Initialize to leave a space, and then the word `points':
%%\pointname{ points}
% The following improvement was contributed by 
% Mate Wierdl <mw@wierdlmpc.msci.memphis.edu> 
% If the number of points is ``1'', then the default value of
% \pointname will print `` point'' instead of `` points'' (and this
% version of the command doesn't generate an error message if the
% points entry is something other than a number):
\pointname{ \ifthenelse{\equal{\@points}{1}}{point}{points}}

% If we used the following line instead, then you'd get an error
% message if the point value contained something other than a valid
% integer:
%\pointname{ \ifthenelse{\@points = 1}{point}{points}}

% \@marginpointname is used in place of \@pointname if
% \@pointsinmargin is true:
\def\marginpointname#1{\gdef\@marginpointname{#1}}
\marginpointname{}


% The following keeps track of whether the user has requested that we
% add up the points on the exam.  We make the default false so that
% users who put other than numbers into the points argument of a
% question (or part, or subpart) won't get error messages.
% We use \if@printtotalpoints as a flag to signal that we are counting
% points, so that we will know to print the total on the screen (and
% in the log file).  We use this separate flag so that the user can
% use both \addpoints and \noaddpoints to count some points and not
% others, but still have the total printed when we finish the file no
% matter what the state of \if@addpoints.
\newif\if@addpoints
\newif\if@printtotalpoints
\def\addpoints{\global\@addpointstrue\global\@printtotalpointstrue}
\def\noaddpoints{\global\@addpointsfalse}
\@addpointsfalse
\@printtotalpointsfalse

%--------------------------------------------------------------------
%--------------------------------------------------------------------

%                      \uplevel and \fullwidth:

% \uplevel is used to print text at the indentation level of the 
% enclosing environment.  For example, to precede a question with
% directions about how that question should be answered, you would
% say \uplevel{Answer this question correctly.}
% 
% \fullwidth is similar, but uses the full page of text on the page.

\long\def\uplevel#1{%
  \par\bigskip
  \vbox{%
 {}   \advance\leftskip\@totalleftmargin
    \advance\leftskip-\leftmargin
    #1\par
  }% vbox
  \nobreak
}

\long\def\fullwidth#1{%
  \par\bigskip
  \vbox{%
    \leftskip=0pt \rightskip=0pt
    #1\par
  }% vbox
  \nobreak
}

%---------------------------------------------------------------------


\endinput




%---------------------------------------------------------------------
%---------------------------------------------------------------------
%---------------------------------------------------------------------
%---------------------------------------------------------------------

