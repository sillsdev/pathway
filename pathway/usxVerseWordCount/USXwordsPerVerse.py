#!/usr/bin/env python
#Boa:PyApp:main
#-----------------------------------------------------------------------------
# Name:        USXwordsPerVerse.py
# Purpose:     prints the count of words in each verse
#
# Author:      <greg_trihus@sil.org>
#
# Created:     2011/08/16
# RCS-ID:      $Id: USXwordsPerVerse.py $
# Copyright:   (c) 2011 SIL International
# Licence:     <http://creativecommons.org/licenses/by-sa/3.0/>
#-----------------------------------------------------------------------------

import os, sys, glob
import xml.parsers.expat
from zipfile import *

modules ={}

outname = 'usxVerseWords.txt'

#These globals are used to keep track of the latest reference
book = ''
chap = ''
vers = ''

curRef = ''

#Count of the words in this verse
words = 0

#True if we are in a note field (notes are not counted)
inNote = False

class Report:
    '''This class accumulates all output lines and outputs the books in sorted order'''
    output = {}
    def write(self, s):
        '''Each output line is added to the end of a dictionary value where the key is the book code'''
        if self.output.has_key(book):
            self.output[book] += s
        else:
            self.output[book] = s
    def printReport(self):
        '''The report is printed by testing to see if each book in order exists in the corpus.'''
        global outname
        outfl = open(outname, 'w')    
        for b in self.bookOrder:
            if self.output.has_key(b):
                outfl.write(self.output[b])

    bookOrder =  ["GEN" ,          # Genesis
            "EXO" ,          # Exodus
            "LEV" ,          # Leviticus
            "NUM" ,          # Numbers
            "DEU" ,          # Deuteronomy
            "JOS" ,          # Joshua
            "JDG" ,          # Judges
            "RUT" ,          # Ruth
            "1SA" ,          # 1 Samuel
            "2SA" ,          # 2 Samuel
            "1KI" ,          # 1 Kings
            "2KI" ,          # 2 Kings
            "1CH" ,          # 1 Chronicles
            "2CH" ,          # 2 Chronicles
            "EZR" ,          # Ezra
            "NEH" ,          # Nehemiah
            "EST" ,          # Esther (Hebrew)
            "JOB" ,          # Job
            "PSA" ,          # Psalms
            "PRO" ,          # Proverbs
            "ECC" ,          # Ecclesiastes
            "SNG" ,          # Song of Songs
            "ISA" ,          # Isaiah
            "JER" ,          # Jeremiah
            "LAM" ,          # Lamentations
            "EZK" ,          # Ezekiel
            "DAN" ,          # Daniel (Hebrew)
            "HOS" ,          # Hosea
            "JOL" ,          # Joel
            "AMO" ,          # Amos
            "OBA" ,          # Obadiah
            "JON" ,          # Jonah
            "MIC" ,          # Micah
            "NAM" ,          # Nahum
            "HAB" ,          # Habakkuk
            "ZEP" ,          # Zephaniah
            "HAG" ,          # Haggai
            "ZEC" ,          # Zechariah
            "MAL" ,          # Malachi
            "MAT" ,          # Matthew
            "MRK" ,          # Mark
            "LUK" ,          # Luke
            "JHN" ,          # John
            "ACT" ,          # Acts
            "ROM" ,          # Romans
            "1CO" ,          # 1 Corinthians
            "2CO" ,          # 2 Corinthians
            "GAL" ,          # Galatians
            "EPH" ,          # Ephesians
            "PHP" ,          # Philippians
            "COL" ,          # Colossians
            "1TH" ,          # 1 Thessalonians
            "2TH" ,          # 2 Thessalonians
            "1TI" ,          # 1 Timothy
            "2TI" ,          # 2 Timothy
            "TIT" ,          # Titus
            "PHM" ,          # Philemon
            "HEB" ,          # Hebrews
            "JAS" ,          # James
            "1PE" ,          # 1 Peter
            "2PE" ,          # 2 Peter
            "1JN" ,          # 1 John
            "2JN" ,          # 2 John
            "3JN" ,          # 3 John
            "JUD" ,          # Jude
            "REV" ,          # Revelation
            "TOB" ,          # Tobit
            "JDT" ,          # Judith
            "ESG" ,          # Esther Greek
            "WIS" ,          # Wisdom of Solomon
            "SIR" ,          # Sirach (Ecclesiasticus)
            "BAR" ,          # Baruch
            "LJE" ,          # Letter of Jeremiah
            "S3Y" ,          # Song of 3 Young Men
            "SUS" ,          # Susanna
            "BEL" ,          # Bel and the Dragon
            "1MA" ,          # 1 Maccabees
            "2MA" ,          # 2 Maccabees
            "3MA" ,          # 3 Maccabees
            "4MA" ,          # 4 Maccabees
            "1ES" ,          # 1 Esdras (Greek)
            "2ES" ,          # 2 Esdras (Latin)
            "MAN" ,          # Prayer of Manasseh
            "PS2" ,          # Psalm 151
            "ODA" ,          # Odes
            "PSS" ,          # Psalms of Solomon
            "EZA" ,          # Apocalypse of Ezra
            "5EZ" ,          # 5 Ezra
            "6EZ" ,          # 6 Ezra
            "DAG" ,          # Daniel Greek
            "PS3" ,          # Psalms 152-155
            "2BA" ,          # 2 Baruch (Apocalypse)
            "LBA" ,          # Letter of Baruch
            "JUB" ,          # Jubilees
            "ENO" ,          # Enoch
            "1MQ" ,          # 1 Meqabyan
            "2MQ" ,          # 2 Meqabyan
            "3MQ" ,          # 3 Meqabyan
            "REP" ,          # Reproof
            "4BA" ,          # 4 Baruch
            "LAO" ]          # Laodiceans


def getRef():
    '''Returns the current scripture reference.'''
    ref = book
    if chap != '':
        ref += ' %s' % chap
    if vers != '':
        ref += ':%s' % vers
    return ref

def setRef(name, attrs):
    '''Sets the global reference values based on the element name and attributes'''
    global book, chap, vers
    if name == 'book':
        if attrs.has_key('code'):
            book = attrs['code']
        else:
            book = ''
        chap = ''
        vers = ''
    if name == 'chapter':
        chap = attrs['number']
        vers = ''
    if name == 'verse':
        vers = attrs['number']

def PrintRef():
    '''print the current reference and the number of words counted'''
    global curRef, words
    chrReport.write('%s\t%d\n' % (curRef, words))
    words = 0

def _Element(name, attrs):
    '''Parser calls this method at the begining of each element. It prints the
    results for the last verse and it sets the reference and whether we are in 
    a note field.'''
    global curRef, words, inNote
    setRef(name, attrs)
    if name == 'verse':
        if curRef != '':
            PrintRef()
        curRef = getRef()
        words = 0
    if name == 'para' and attrs['style'] in ['s1', 's', 's2', 'sp', 'cp', 'cl', 'mte', 'mte1', 'rem'] or name == 'chapter':
        if curRef != '':
            PrintRef()
        curRef = ''
    if name == 'note':
        inNote = True
        
def _EndElement(name):
    '''Called by the parse when closing an element. It prints out the last verse
    and resets the flag if we are exiting the note field.'''
    global curRef, inNote
    if name == 'usfm':
        if curRef != '':
            PrintRef()
        curRef = ''
    if name == 'note':
        inNote = False

def _Data(data):
    '''The parser calls this method for the data in a tag. It counts words.'''
    global words, inNote
    if not inNote:
        words += len(data.split())

class usxInput:
    '''Take input from dbl bundle, or from a wild card construct'''
    def __init__(self, name):
        self.isZip = is_zipfile(name)
        if self.isZip:
            self.zFile = ZipFile(name)
        self.name = name

    #these book ids are not Scripture text
    nonScripture = ["XXA", "XXB", "XXC", "XXD", "XXE", "XXF", "XXG", "FRT", "BAK", "OTH", "INT", "CNC", "GLO", "TDX", "NDX"]
        
    def list(self):
        '''The list contains all the books with Scripture in them.'''
        if self.isZip:
            return [x for x in self.zFile.namelist() if len(x) > 4 and x[:4] == 'USX/' and not x[4:7] in self.nonScripture]
        return glob.glob(self.name)
        
    def read(self, name):
        '''return all the contents of the file name'''
        if self.isZip:
            return self.zFile.read(name)
        return open(name).read()
    
def parseFiles(argv):
    #If no command line argument, set it to *.usx by default
    if len(argv) > 1:
        input = usxInput(argv[1])
        os.chdir(os.path.dirname(argv[1]))
    else:
        input = usxInput('*.usx')
    for f in input.list():
        #print 'reading ' + f
        if f[:1] == '1': continue   # Skip font matter, back matter, etc.
        #set up the parser and parse the file (parsing prints the results).
        p = xml.parsers.expat.ParserCreate()
        p.StartElementHandler = _Element
        p.EndElementHandler = _EndElement
        p.buffer_text = True
        p.CharacterDataHandler = _Data
        p.Parse(input.read(f))

def main(argv):
    global chrReport
    chrReport = Report()
    parseFiles(argv)
    chrReport.printReport()
    os.startfile(outname)
    
if __name__ == '__main__':
    main(sys.argv)
