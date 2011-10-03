#!/usr/bin/env python
#Boa:PyApp:main
#-----------------------------------------------------------------------------
# Name:        XMLtagsNattrsNvalsWrefs.py
# Purpose:     Displays xml tags with attributes used and values. Gives
#              reference for first occurance of each value.
#
# Author:      <greg_trihus@sil.org>
#
# Created:     2011/08/16
# RCS-ID:      $Id: XMLtagsNattrsNvalsWrefs.py $
# Copyright:   (c) 2011 SIL International
# Licence:     <http://creativecommons.org/licenses/by-sa/3.0/>
#-----------------------------------------------------------------------------

import os, sys, glob
import xml.parsers.expat
from zipfile import *

modules ={}

outname = 'xmlMarkup.txt'

#These globals keep track of the current reference
book = ''
chap = ''
vers = ''

#elems is a dictionary of elements whose value is a dictionary of attributes. In
#the attribute dictionary, the value for each attribute is the first reference
#where it occurs.
elems = {}

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
        book = ''
        if attrs.has_key('code'):
            book = attrs['code']
        chap = ''
        vers = ''
    if name == 'chapter':
        chap = attrs['number']
        vers = ''
    if name == 'verse':
        vers = attrs['number']

def _Element(name, attrs):
    '''Parser calls this method at the begining of each element. It sets the 
    reference. Then it goes through the list of attributes and adds the new
    values and references where they occur.'''
    setRef(name, attrs)
    try:
        cur = elems[name]
        for a in attrs:
            try:
                if a == 'id': continue
                if not cur[a].has_key(attrs[a]):
                    cur[a][attrs[a]] = getRef()
            except:
                cur[a] = {attrs[a]: getRef()} 
        elems[name] = cur
    except:
        cur = {}
        for a in attrs:
            cur[a] = {attrs[a]: getRef()} 
        elems[name] = cur

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
    '''set up the parser and parse the file (parsing collects element info in
    global variables).'''
    #If no command line argument, set it to *.usx by default
    if len(argv) > 1:
        input = usxInput(argv[1])
        os.chdir(os.path.dirname(argv[1]))
    else:
        input = usxInput('*.usx')
    for f in input.list():
        #print 'reading ' + f
        if f[:1] == '1': continue   # Skip font matter, back matter, etc.
        p = xml.parsers.expat.ParserCreate()
        p.StartElementHandler = _Element
        p.Parse(input.read(f))

def printReport():
    '''print report of elements, attributes, and values'''
    global outname
    outfl = open(outname, 'w')
    kys = elems.keys()
    kys.sort()
    for k in kys:
        atrs = elems[k].keys()
        atrs.sort()
        sum = ''
        for a in atrs:
            vals = elems[k][a].keys()
            vals.sort()
            valsum = ''
            punct = '['
            for v in vals:
                valsum += punct
                #print reference detail for all elements except book and chapter
                if not k in ['book', 'chapter']:
                    valsum += '%s(%s)' % (v, elems[k][a][v])
                    punct = ',\n\t\t'
                else:
                    valsum += v
                    punct = ', '
            valsum += ']'
            sum += ' %s: %s' % (a, valsum)
        outfl.write('%s:\t%s\n' % (k, sum))

def main(argv):
    global outname
    parseFiles(argv)
    printReport()
    os.startfile(outname)

if __name__ == '__main__':
    main(sys.argv)
