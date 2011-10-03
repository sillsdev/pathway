#!/usr/bin/env python
#Boa:PyApp:main
#-----------------------------------------------------------------------------
# Name:        USXchars.py
# Purpose:     print a chart with the unusal chars and their first 5 references
#
# Author:      <greg_trihus@sil.org>
#
# Created:     2011/08/17
# RCS-ID:      $Id: USXchars.py $
# Copyright:   (c) 2011 SIL International
# Licence:     <http://creativecommons.org/licenses/by-sa/3.0/>
#-----------------------------------------------------------------------------

import os, sys, glob
import xml.parsers.expat
from zipfile import *

modules ={}

outname = 'usxExtendedChars.txt'

#These globals keep track of the current reference
book = ''
chap = ''
vers = ''

curRef = ''

#This flag is true if we are in a note field.
inNote = False

#The chars dictionary contains a list of references where the char occurs
chars = {}

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

def _Element(name, attrs):
    '''Parser calls this method at the begining of each element. It sets the 
    reference and whether we are in a note field.'''
    global curRef, inNote
    setRef(name, attrs)
    if name == 'verse':
        curRef = getRef()
    if name == 'para' and attrs['style'] in ['s1', 's', 's2', 'sp', 'cp', 'cl', 'mte', 'mte1', 'rem'] or name == 'chapter':
        curRef = ''
    if name == 'note':
        inNote = True
        
def _EndElement(name):
    '''Called by the parse when closing an element. It resets the flag if we are 
    exiting the note field.'''
    global inNote
    if name == 'note':
        inNote == False

def _Data(data):
    '''The parser calls this method for the data in a tag. It adds the first 10
    references for characters whose code numberse are > 128 to the global chars
    dictionary.'''
    global inNote, chars
    for c in data:
        if ord(c) > 128:
            try:
                if len(chars[c]) > 9:
                    continue
                chars[c].append(getRef())
            except:
                chars[c] = [getRef()]

class usxInput:
    def __init__(self, name):
        self.isZip = is_zipfile(name)
        if self.isZip:
            self.zFile = ZipFile(name)
        self.name = name

    nonScripture = ["XXA", "XXB", "XXC", "XXD", "XXE", "XXF", "XXG", "FRT", "BAK", "OTH", "INT", "CNC", "GLO", "TDX", "NDX"]
        
    def list(self):
        if self.isZip:
            return [x for x in self.zFile.namelist() if len(x) > 4 and x[:4] == 'USX/' and not x[4:7] in self.nonScripture]
        return glob.glob(self.name)
        
    def read(self, name):
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
        #set up the parser and parse the file (parsing collects char info).
        p = xml.parsers.expat.ParserCreate()
        p.StartElementHandler = _Element
        p.EndElementHandler = _EndElement
        p.buffer_text = True
        p.CharacterDataHandler = _Data
        p.Parse(input.read(f))

def printReport():
    global chars, outname
    outfl = open(outname, 'w')
    outfl.write('Char\tUnicode\tRefs\n')
    keys = chars.keys()
    keys.sort()
    for c in keys:
        refs = ''
        punct = ''
        for v in chars[c][:5]:
            refs += punct
            punct = '; '
            refs += v
        outfl.write(c.encode('utf-8'))
        outfl.write('\t%04x\t%s\n' % (ord(c), refs))

def main(argv):
    global outname
    parseFiles(argv)
    printReport()
    os.startfile(outname)
    
if __name__ == '__main__':
    main(sys.argv)
