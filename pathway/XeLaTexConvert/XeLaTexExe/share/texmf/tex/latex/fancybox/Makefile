
# `Makefile' for 'fancybox-doc', hv, 2010/05/15

.SUFFIXES : .tex .ltx .dvi .ps .pdf .eps

PACKAGE = fancybox

MAIN = $(PACKAGE)-doc

TDS = ~/PSTricks/PSTricks-TDS

LATEX = pdflatex

ARCHNAME = $(MAIN)-$(shell date +%y%m%d)

ARCHFILES = $(PACKAGE).sty $(PACKAGE).tex $(MAIN).tex README Changes Makefile

all : doc clean 
doc: $(MAIN).tex
	$(LATEX) $<
	$(LATEX) $<
	if ! test -f $(basename $<).glo ; then touch $(basename $<).glo; fi
	if ! test -f $(basename $<).idx ; then touch $(basename $<).idx; fi
	makeindex -t $(basename $<).ilg -s pst-doc.ist -o $(basename $<).ind $(basename $<).idx
	makeindex -s gglo.ist -t $(basename $<).glg -o $(basename $<).gls \
	  $(basename $<).glo
#	bibtex $(basename $<)
	$(LATEX) $<
	$(LATEX) $<

clean : 
	$(RM) $(addprefix $(MAIN), .cb .cb? .log .aux .glg .glo .gls .ilg .idx .ind .tmp .toc .out .blg .bbl )
	$(RM) $(addprefix $(MAIN), .dvi .ps .xcp .ex1)

veryclean : clean
	$(RM) $(addprefix $(MAIN), .pdf .bbl .blg)

arch :
	zip $(ARCHNAME).zip $(ARCHFILES)

tds:
	cp -u Changes     $(TDS)/doc/generic/$(PACKAGE)/
	cp -u README      $(TDS)/doc/generic/$(PACKAGE)/
	cp -u $(MAIN).pdf $(TDS)/doc/generic/$(PACKAGE)/
#
	cp -u Changes        $(TDS)/tex/latex/$(PACKAGE)/
	cp -u $(PACKAGE).sty $(TDS)/tex/latex/$(PACKAGE)/
#
	cp -u Changes        $(TDS)/tex/generic/$(PACKAGE)/
	cp -u $(PACKAGE).tex $(TDS)/tex/generic/$(PACKAGE)/
#
	cp -u Changes        $(TDS)/dvips/$(PACKAGE)/
	cp -u $(PACKAGE).pro $(TDS)/dvips/$(PACKAGE)/
	cp -u $(PACKAGE).pro ~/Links/dvips-local/
#
	cp -u Changes     $(TDS)/source/$(PACKAGE)/
	cp -u $(MAIN).tex $(TDS)/source/$(PACKAGE)/
	cp -u $(MAIN).bib $(TDS)/source/$(PACKAGE)/
	cp -u Makefile    $(TDS)/source/$(PACKAGE)/


# EOF
