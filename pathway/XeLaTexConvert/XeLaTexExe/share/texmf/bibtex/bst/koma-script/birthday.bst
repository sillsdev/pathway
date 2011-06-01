%%
%% This is file `birthday.bst',
%% generated with the docstrip utility.
%%
%% The original source files were:
%%
%% scrlettr.dtx  (with options: `birthday,bst')
%% 
%% BibTeX-Database to scrletter converter
%% by Axel Kielhorn 1996/06/06
%% little correction by Markus Kohm 1996/11/13
%% 
%% Based on:
%% BibTeX bibliography style `address'
%% Gerd Neugebauer 1994
%% 
%% This is only one way to use the address-database-layout defined by
%% Gerd Neugebauer.
%% Use this as an example for your own conversion program.
%% 
 % This file prints the entries with birthdays only.
 % The entries are sorted by month and inside one month by date.

ENTRY
  { name
    title
    organization
    city
    zip
    country
    street
    phone
    mobile
    fax
    url
    email
    note
    key
    birthday
    nbirthday
  }
  {}
  { b.month }

STRINGS { cur.month num.index alpha.index}

FUNCTION {get.names}
{ name empty$
  { organization empty$
    { "{}{}" }
    { organization "{}" * }
    if$
  }
  { "{" name #1 "{ll}" format.name$ "}" * *
    "{" name #1 "{ff}{ vv}" format.name$ "}" * * *
  }
  if$
}

FUNCTION {print.adrchr}
{ b.month cur.month =
  'skip$
  { newline$ "\adrchar{\convdate{" b.month "}}" * * write$ newline$
    b.month 'cur.month :=
  }
  if$
}

FUNCTION{address}
{
  birthday empty$
  'skip$
  {
    print.adrchr
    "\adrentry" write$
    get.names write$ newline$
    "{"
    street empty$
      { "" }
      { street " \\ " *}
      if$
    *
    zip empty$
      { "" }
      { zip }
      if$
    " " * *
    city empty$
      { "" }
      { city }
      if$
    "}{" * *
    phone empty$
      {
        mobile empty$
          { "" }
          { mobile }
        if$
      }{
        mobile empty$
          { phone }
          { phone "\\" mobile * * }
        if$
      }
      if$
    "}{}{}{}{" * *
    birthday empty$
      { "" }
      { birthday }
      if$
    "}{" * *
    key empty$
      { "" }
      { key }
      if$
    "}" * * write$ newline$
  }
  if$
}

FUNCTION{presort}
{nbirthday empty$
  { "" }
  { nbirthday }
  if$
  duplicate$
  'sort.key$ :=
  #1 #2 substring$
  'b.month :
}

READ

ITERATE {presort}

SORT

FUNCTION {begin.bib}
{
"\def\convdate#1{\ifcase#1 \or 1 \or 2 \or 3 \or 4 \or 5 "
"\or 6 \or 7 \or 8 \or 9 \or 10 \or 11 \or 12 \fi}" * 'num.index :=

"\def\convdate#1{\ifcase#1 \or Januar \or Februar \or "
" M{^^b a}rz \or April \or Mai \or Juni \or Juli "
"\or August \or September \or Oktober \or November \or Dezember \fi}"
* * 'alpha.index :=

 alpha.index write$
  newline$
  preamble$ empty$
    'skip$
    { preamble$ write$ newline$ }
  if$
  "00" 'cur.month :=
}

EXECUTE {begin.bib}

ITERATE {call.type$}


FUNCTION {end.bib}
{ newline$ }

EXECUTE {end.bib}
%% 
%%
%% End of file `birthday.bst'.
