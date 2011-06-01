%%
%% This is file `email.bst',
%% generated with the docstrip utility.
%%
%% The original source files were:
%%
%% scrlettr.dtx  (with options: `email,bst')
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
  { adr.char }

INTEGERS { cur.adr.char }

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
{ adr.char chr.to.int$ cur.adr.char >
  { newline$ "\adrchar{" adr.char "}" * * write$ newline$
    adr.char chr.to.int$ 'cur.adr.char :=
  }
  'skip$
  if$
}

FUNCTION{address}
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
    email empty$
     { "" }
     { email }
     if$
    "}{}{}{}{" * *
    key empty$
      { "" }
      { key }
      if$
    "}" * * write$ newline$
}

FUNCTION{presort}
{
  name empty$
  { organization empty$
    { key empty$
      { "No name, organization, or key to sort " cite$ * warning$
        "???"
      }
      { key }
      if$
    }
    { organization }
    if$
  }
  { name #1 "{ll},{f }{v }{, j}" format.name$ }
  if$
  duplicate$
  'sort.key$ :=
  #1 #1 substring$
  'adr.char :% the adr.char is the first letter of the sort.key
}

READ

ITERATE {presort}

SORT

FUNCTION {begin.bib}
{
  "\def\sbscrpt{\_}  \catcode`\_ \active  \let_=\sbscrpt"
  write$ newline$
  preamble$ empty$
    'skip$
    { preamble$ write$ newline$ }
  if$
  #0 'cur.adr.char :
}

EXECUTE {begin.bib}

ITERATE {call.type$}


FUNCTION {end.bib}
{ newline$ }

EXECUTE {end.bib}
%% 
%%
%% End of file `email.bst'.
