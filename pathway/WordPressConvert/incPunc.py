#!/usr/bin/env python
#-----------------------------------------------------------------------------
# Name:        incPunc.py
# Purpose:     move punctuation from css to xhtml, simplify css
#
# Author:      <greg_trihus@sil.org>
#
# Created:     2012/03/02
# RCS-ID:      $Id: incPunc.py $
# Copyright:   (c) 2012 SIL International
# Licence:     <LPGL>
#-----------------------------------------------------------------------------
# -*- coding: UTF-8 -*-
#Boa:PyApp:main

import os, sys, re, getopt
from lxml import etree

modules ={}

XHTML_NAMESPACE = "http://www.w3.org/1999/xhtml"

def FindXpath(root, xpath):
    """return the first result of applying xpath to root"""
    result = root.find(xpath)
    return result

def IncludePunct(fullname, page=True):
    """
    incPunc [-p] fullpath
    
    include punctuation in the xhtml file instead of the css file.
    
    option:
    -p, -page\tDon't include @page declarations in the css file either
    """
    name = os.path.splitext(fullname)[0]
    css = name + '.css'
    f = open(css)
    data = f.read()
    f.close()
    os.rename(css, name + '0.css')
    # match lines like: .variant-primary-minor:before { content: " " }
    befPat = re.compile('\n\.([-a-zA-Z]+)\:before\s{\scontent:\s"([^"]*)')
    before = {}
    for cls,val in befPat.findall(data):
        before[cls] = val.decode('utf-8')
##    print 'before:',before
    # match lines like: .complexformrefs>.xitem + .xitem:before { content: " " }
    betPat = re.compile('\n\.([-a-zA-Z]+)\>\.xitem\s\+\s\.xitem\:before\s{\scontent:\s"([^"]*)')
    between = {}
    for cls,val in betPat.findall(data):
        between[cls] = val.decode('utf-8')
##    print 'between:',between
    # match lines like: .variant-primary-minor:after { content: " " }
    aftPat = re.compile('\n\.([-a-zA-Z]+)\:after\s{\scontent:\s"([^"]*)')
    after = {}
    for cls,val in aftPat.findall(data):
        after[cls] = val.decode('utf-8')
##    print 'after:',after
    xhtml = name + '.xhtml'
    bak = name + '0.xhtml'
    os.rename(xhtml, bak)
    root = etree.parse(bak)
    for elem in root.iter():
        if elem.attrib.has_key('class'):
            cls = elem.attrib['class']
            if before.has_key(cls):
                if elem.text != None:
                    elem.text = before[cls] + elem.text
                else:
                    elem.text = before[cls]
            children = elem.getchildren()
            if between.has_key(cls):
                if len(children) > 0:
                    first = children[0]
                    if first.attrib.has_key('class'):
                        firstcls = first.attrib['class']
                        if firstcls == 'xitem':
                            for child in children[1:]:
                                if child.text != None:
                                    child.text = between[cls] + child.text
                                else:
                                    child.text = between[cls]
            if after.has_key(cls):
                if len(children) > 0:
                    last = children[-1]
                    if last.tail != None:
                        last.tail += after[cls]
                    else:
                        last.tail = after[cls]
                else:
                    if elem.text != None:
                        elem.text += after[cls]
                    else:
                        elem.text = after[cls]
    outname = xhtml
    outdata = etree.tostring(root, encoding='utf-8', xml_declaration=True)
    f = open(outname,'w')
    f.write(outdata)
    f.close()


    f = open(css,'w')
    # match lines like: .variant-primary-minor {
    declPat = re.compile('\.([-a-zA-Z]+)\s{')
    propPat = re.compile('\s+([-a-zA-Z]+)\:')
    isSpan = False
    spanTag = '{%s}span' % XHTML_NAMESPACE
    firstDecl = False
    for line in data.split('\n'):
        if line.find(':before') == -1 and line.find(':after') == -1:
            mDecl = declPat.match(line)
            if mDecl:
                firstDecl = True
                xpath = '//*[@class="%s"]' % mDecl.group(1)
                node = FindXpath(root, xpath)
                isSpan = (node.tag == spanTag)
            elif isSpan:
                mProp = propPat.match(line)
                if mProp:
                    if mProp.group(1) in ['padding-left', 'text-indent', 'margin-left', 'padding-bottom', 'padding-top']:
                        continue
            if firstDecl or page:
                f.write(line + '\n')
    f.close()
        
def Usage():
    """display documentation of program for user"""
    sys.stderr.write(IncludePunct.__doc__)
    sys.exit(2)

if __name__ == '__main__':
    longopt = ['--nopage']
    try:
        optlist, args = getopt.getopt(sys.argv[1:], 'p', longopt)
    except getopt.GetoptError:
        Usage()
    page = True
    for o, a in optlist:
        if o in ("-p", "--page"):
            page = not page
    if len(args) != 1:
        Usage()
    if args[0][:1] == '"':
        fullname = args[0][1:-1]
    else:
        fullname = args[0]
    IncludePunct(fullname, page=page)
