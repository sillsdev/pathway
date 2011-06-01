%%
%% This is file `amsru.bst',
%% generated with the docstrip utility.
%%
%% The original source files were:
%%
%% amsxport.dtx  (with options: `bst,plain')
%% 
%%% ====================================================================
%%% @BibTeX-source-file{
%%%   filename  = "amsxport.dtx",
%%%   version   = "2.00",
%%%   date      = "2004/06/28",
%%%   time      = "14:50:12 EDT",
%%%   author    = "Michael J Downes",
%%%   address   = "American Mathematical Society,
%%%                Publications Technical Group,
%%%                PO Box 6248,
%%%                Providence, RI 02940,
%%%                USA",
%%%   email     = "tech-support at ams.org",
%%%   URL       = "http://www.ams.org/",
%%%   abstract  = "The BibTeX style file amsxport.bst produces .bbl
%%%                entries where most of the database structure is
%%%                retained. This is intended for processing by a smart
%%%                bibliography package such as the amsrefs package.",
%%%   checksum  = "40425 1822 6396 51551",
%%%   docstring = "The checksum field, produced by Robert Solovay's
%%%                checksum utility, gives CRC-16 checksum, lines,
%%%                words, and characters.",
%%%  }
%%% ====================================================================
ENTRY
  {
    address      institution  number       status
    archive      isbn         organization subtitle
    author       issn         pages        title
    booktitle    journal      part         translator
    chapter      key          pii          type
    date         language     preprint     url
    edition      meeting      publisher    volume
    editor       month        review       xid
    eprint       mrnumber     school       year
    howpublished note         series
  }
  { }
  { label }
MACRO {jan} {"-01"}
MACRO {feb} {"-02"}
MACRO {mar} {"-03"}
MACRO {apr} {"-04"}
MACRO {may} {"-05"}
MACRO {jun} {"-06"}
MACRO {jul} {"-07"}
MACRO {aug} {"-08"}
MACRO {sep} {"-09"}
MACRO {oct} {"-10"}
MACRO {nov} {"-11"}
MACRO {dec} {"-12"}
STRINGS { p s t l }
INTEGERS { len ptr }

FUNCTION {NOT} { { #0 } { #1 } if$ }

FUNCTION {AND} { 'skip$ { pop$ #0 } if$ }

FUNCTION {OR} { { pop$ #1 } 'skip$ if$ }

FUNCTION {TRUE} { #1 }

FUNCTION {FALSE} { #0 }

FUNCTION {incr.ptr} { ptr #1 + 'ptr := }

FUNCTION {current.char} { ptr #1 substring$ }
FUNCTION {copy.two.chars} { t * incr.ptr s current.char * incr.ptr }

FUNCTION {is.digit} {
    chr.to.int$
    duplicate$
    "0" chr.to.int$ #1 - >
    swap$
    "9" chr.to.int$ #1 + <
    AND
}

FUNCTION {is.lowercase.letter} {
    chr.to.int$ duplicate$
    "a" chr.to.int$ #1 - >
    swap$
    "z" chr.to.int$ #1 + <
    AND
}
FUNCTION {is.uppercase.letter} {
    chr.to.int$ duplicate$
    "A" chr.to.int$ #1 - >
    swap$
    "Z" chr.to.int$ #1 + <
    AND
}
FUNCTION {is.letter} {
    duplicate$
    is.lowercase.letter
        { pop$ TRUE }
        { is.uppercase.letter }
    if$
}
FUNCTION {skip.accent} { pop$ incr.ptr }
FUNCTION {strip.label} {
    's :=
    #1 'ptr :=
    ""
    { s ptr #1 substring$ duplicate$ empty$ NOT }
        { duplicate$ is.letter
            { * }
            { duplicate$ "\" =
                { skip.accent }
                'pop$
              if$
            }
          if$
          incr.ptr
        }
    while$
    pop$
}
FUNCTION {sortify} {
    purify$
    "l" change.case$
}
FUNCTION {rpad} {
    swap$
    'l :=
    l text.length$ - 'len :=
    { len #0 > }
        {
            l " " * 'l :=
            len #1 - 'len :=
        }
    while$
    l
}
FUNCTION {lpad} {
    swap$
    'l :=
    l text.length$ - 'len :=
    { len #0 > }
        {
            " " l * 'l :=
            len #1 - 'len :=
        }
    while$
    l
}
FUNCTION {field.or.null} {
    duplicate$ empty$
        { pop$ "" }
        'skip$
    if$
}
FUNCTION {missing.or.empty} { duplicate$ missing$ swap$ empty$ OR }

FUNCTION {append.ndash.and.skip.hyphens} {
    "\ndash " *
    incr.ptr
    { s current.char "-" = }
        { incr.ptr }
    while$
}
FUNCTION {n.dashify} {
    's :=
    #1 'ptr :=
    ""
    { s current.char 't :=  t "" = NOT }
      { t "\" =
          { copy.two.chars }
          { t "-" =
              { append.ndash.and.skip.hyphens }
              { t * incr.ptr }
            if$
          }
        if$
      }
    while$
}
FUNCTION {missing.warning} {
    "missing " swap$ * " in " * cite$ * warning$
}

INTEGERS { string.ptr }

FUNCTION {string.length} {
    #1 'string.ptr :=
    { duplicate$ string.ptr #1 substring$ "" = 'FALSE 'TRUE if$ }
        { string.ptr #1 + 'string.ptr := }
    while$
    pop$
    string.ptr #1 -
}
FUNCTION {format.title}{
    duplicate$
    missing.or.empty
        { pop$ "" }
        { "t" change.case$ }
    if$
}
FUNCTION {start.field} {
    #12 lpad "={" * write$
}
FUNCTION {fin.field} { "}," write$ newline$ }

FUNCTION {write.field} {
    duplicate$
    missing$
        { pop$ missing.warning }
        { duplicate$ empty$
            { pop$ missing.warning }
            { swap$ start.field write$ fin.field }
          if$
        }
    if$
}
FUNCTION {optional.field} {
    duplicate$
    missing.or.empty
        { pop$ pop$ }
        { swap$ start.field write$ fin.field }
    if$
}
FUNCTION {optional.title.field} {
    duplicate$
    missing.or.empty
        { pop$ pop$ }
        { format.title swap$ start.field write$ fin.field }
    if$
}
FUNCTION {optional.pages.field} {
    duplicate$
    missing.or.empty
        { pop$ pop$ }
        { swap$ start.field
          n.dashify
          write$ fin.field
        }
    if$
}
FUNCTION {optional.mr.field} {
    duplicate$
    missing.or.empty
        { pop$ pop$ }
        {
          swap$ start.field
          "\MR{" swap$ * "}" * write$ fin.field
        }
    if$
}
FUNCTION {remove.ordinal.suffix} {
    's :=
    s string.length 'len :=
    #1 'ptr :=
    { ptr len < s ptr #1 substring$ is.digit AND }
        'incr.ptr
    while$
    s ptr global.max$ substring$ 'p :=
    p "st" =
        { TRUE }
        { p "nd" =
            { TRUE }
            { p "rd" =
                { TRUE }
                { p "th" =
                    { TRUE }
                    { FALSE }
                  if$
                }
              if$
            }
          if$
        }
    if$
    { s #1 ptr #1 - substring$ }
    { s }
  if$
}
FUNCTION {optional.edition.field} {
    duplicate$ missing.or.empty
        { pop$ pop$ }
        { swap$ start.field
          remove.ordinal.suffix
          write$ fin.field
        }
    if$
}
INTEGERS { nameptr numnames namesleft }
STRINGS { namelist fieldname }
FUNCTION {format.name} { "{vv~}{ll}{, ff}{, jj}" format.name$ }

FUNCTION {optional.name.field}{
    duplicate$ missing.or.empty
        { pop$ pop$ }
        { swap$ 'fieldname :=
          duplicate$ num.names$
          'namesleft :=
          #1 'ptr :=
          { namesleft #0 > }
            { fieldname start.field
              duplicate$ ptr format.name write$
              fin.field
              ptr #1 + 'ptr :=
              namesleft #1 - 'namesleft :=
            }
          while$
        pop$
        }
    if$
}
FUNCTION {assemble.date.field}
{ date missing$
    { year missing.or.empty
        { status missing.or.empty
            { "No year or other date information for "
               cite$ * warning$ }
            'skip$
          if$
          "status"
        }
        { year month missing$ { "" } { month } if$ * }
      if$
    }
    { date }
  if$
  duplicate$ "status" =
    { pop$ pop$ }
    { swap$ start.field write$ fin.field }
  if$
}
FUNCTION {optional.journal.field} {
    duplicate$ missing.or.empty
        { pop$ pop$ }
        {
            's :=
            start.field
            #1 'ptr :=
            s current.char is.lowercase.letter
                {
                    s "/" * 's :=
                    { s ptr #1 substring$ "/" = NOT }
                        { incr.ptr }
                    while$
                    s ptr #1 + #1 substring$ empty$
                        { s #1 ptr #1 - substring$ }
                        {
                            s #1 ptr #1 - substring$
                            write$
                            fin.field
                            "ISSN" start.field
                            s ptr #1 + #9 substring$
                        }
                    if$
                }
                { s }
            if$
            write$
            fin.field
        }
    if$
}
FUNCTION {optional.type.field}
{ duplicate$ missing.or.empty
    { type$ "mastersthesis" = { "Master's Thesis" }
        { type$ "phdthesis" = { "Ph.D. Thesis" }
          { "" } if$ } if$
      swap$ pop$
    }
    'skip$
  if$
  duplicate$ empty$
    { pop$ pop$ }
    { swap$ start.field write$ fin.field }
  if$
}
FUNCTION {url.note.status.review} {
    "url" url optional.field
    "note" note optional.field
    "status" status optional.field
    "review" review optional.field
    "review" mrnumber optional.mr.field
}
FUNCTION {start.entry} {
    newline$
    "\bib{" cite$ * "}{" *
    type$ "mastersthesis" = type$ "phdthesis" =  OR
        { "thesis" }
        { type$ }
    if$
    * "}{" * write$
    newline$
}
FUNCTION {fin.entry} {
    "}" write$
    newline$
}
FUNCTION {article} {
    start.entry
    "author" author optional.name.field
    "translator" translator optional.name.field
    "title" title format.title write.field
    "subtitle" subtitle optional.title.field
    "language" language optional.field
    "organization" organization optional.field
    "how" howpublished optional.field
    "date" assemble.date.field
    "ISSN" issn optional.field
    "journal" journal optional.journal.field
    "volume" volume optional.field
    "number" number optional.field
    "pages" pages optional.pages.field
    "PII" pii optional.field
    "archive" archive optional.field
    "eprint" eprint optional.field
    "preprint" preprint optional.field
    url.note.status.review
    fin.entry
}
FUNCTION {inproceedings} {
    start.entry
    "author" author optional.name.field
    "translator" translator optional.name.field
    "title" title format.title write.field
    "subtitle" subtitle optional.title.field
    "language" language optional.field
    "organization" organization optional.field
    "how" howpublished optional.field
    "date" assemble.date.field
    "xid" xid optional.field
    "conference" meeting optional.field
    "booktitle" booktitle optional.title.field
    "edition" edition optional.edition.field
    "editor" editor optional.name.field
    "series" series optional.field
    "volume" volume optional.field
    "publisher" publisher optional.field
    "address" address optional.field
    "pages" pages optional.pages.field
    url.note.status.review
    fin.entry
}
FUNCTION {inbook} { inproceedings }
FUNCTION {incollection} { inproceedings }
FUNCTION {conference} { inproceedings }
FUNCTION {book} {
    start.entry
    "author" author optional.name.field
    "editor" editor optional.name.field
    "translator" translator optional.name.field
    "title" title missing$ { booktitle } { title } if$
       format.title write.field
    "subtitle" subtitle optional.title.field
    "type" type optional.type.field
    "language" language optional.field
    "conference" meeting optional.field
    "edition" edition optional.edition.field
    "series" series optional.field
    "publisher" publisher optional.field
    organization missing.or.empty
        { "institution" institution optional.field }
        { "organization" organization optional.field }
    if$
    "address" address optional.field
    "how" howpublished optional.field
    "date" year write.field
    "volume" volume optional.field
    "number" number optional.field
    "ISBN" isbn optional.field
    url.note.status.review
    fin.entry
}
FUNCTION {booklet} { book }
FUNCTION {manual} { book }
FUNCTION {mastersthesis} { book }

FUNCTION {phdthesis} { book }

FUNCTION {proceedings} { book }

FUNCTION {collection} { book }

FUNCTION {techreport} { book }

FUNCTION {unpublished} { book }

FUNCTION {misc} { book }

FUNCTION {default.type} { misc }


READ

FUNCTION {write.with.newlines}
{ 's :=  #1 'ptr :=
  { s ptr #2 + #1 substring$ "" = NOT }
  { s ptr #3 substring$ "^^M" =
    { s #1 ptr #1 - substring$ write$ newline$
      ptr #3 + 'ptr :=
      s ptr global.max$ substring$ 's :=
      #1 'ptr :=
    }
    { incr.ptr }
    if$
  }
  while$
  s write$ newline$
}
FUNCTION {begin.bib} {
    preamble$ empty$
        'skip$
        { preamble$ write.with.newlines }
    if$
    "% \bib, bibdiv, biblist are defined by the amsrefs package."
        write$ newline$
    "\begin{bibdiv}" write$ newline$
    "\begin{biblist}" write$ newline$
}
FUNCTION {end.bib} {
    newline$
    "\end{biblist}" write$ newline$
    "\end{bibdiv}" write$ newline$
}
EXECUTE {begin.bib}

ITERATE {call.type$}

EXECUTE {end.bib}
%% 
%%
%% End of file `amsru.bst'.
