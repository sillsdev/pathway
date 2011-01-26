#-----------------------------------------------------------------------------
# Name:        flex7Text.py
# Purpose:     Exports Texts from Fieldworks into XHTML for use by Pathway
#
# Author:      <greg_trihus@sil.org>
#
# Created:     2011/01/23
# RCS-ID:      $Id: flex7Text.py $
# Copyright:   (c) 2006
# Licence:     <mit>
#-----------------------------------------------------------------------------
#!/usr/bin/env python
#Boa:PyApp:main

import os, _winreg
from lxml import etree

XHTML_NAMESPACE = "http://www.w3.org/1999/xhtml"
XHTML = "{%s}" % XHTML_NAMESPACE
NSMAP = {None : XHTML_NAMESPACE} # default namespace

modules ={}

class MissingSoftware(Exception):
    def __init__(self, value):
        self.value = value
        
    def __str__(self):
        return repr(self.value)

class fwdata:
    def __init__(self, fullname):
        self.root = etree.parse(fullname)
   
    def TextGuids(self):
        return self.root.xpath('//rt[@class="Text"]/@guid')

    def TextNames(self, text):
        return self._GetRuns('//rt[@class="Text"][@guid="%s"]/Name/AUni' % text)
    
    def TextContentGuid(self, text):
        return self.root.xpath('//rt[@class="Text"][@guid="%s"]/Contents/objsur/@guid' % text)[0]
    
    def TextParagraphGuids(self, guid):
        return self.root.xpath('//rt[@class="StText"][@guid="%s"]/Paragraphs/objsur/@guid' % guid)
    
    def ParagraphRuns(self, guid):
        return self._GetRuns('//rt[@class="StTxtPara"][@guid="%s"]/Contents/Str/Run' % guid)
    
    def ParagraphText(self, guid):
        return self.root.xpath('//rt[@class="StTxtPara"][@guid="%s"]/Contents/Str/Run/text()' % guid)
    
    def _GetRuns(self, xpath):
        print xpath
        ws = self.root.xpath(xpath + '/@ws')
        text = self.root.xpath(xpath + '/text()')
        runs = []
        if len(ws) == len(text):
            for i in range(len(ws)):
                runs.append([ws[i], text[i]])
        return runs
    
class xhtmlDoc:
    def __init__(self, title, css):
        self.xhtml = etree.Element(XHTML + "html", nsmap = NSMAP)
        head = etree.SubElement(self.xhtml, XHTML + "head")
        headTitle = etree.SubElement(head, XHTML + "title")
        headTitle.text = title
        headLink = etree.SubElement(head, XHTML + "link")
        headLink.set("rel", "stylesheet")
        headLink.set("href", css)
        headLink.set("type", "text/css")
        self.body = etree.SubElement(self.xhtml, XHTML + "body")

    def Book(self, runs):
        book = self._DoChild(self.body, "div", "letHead")
        self._DoRuns(book, runs, "div", "letter")
        self.content = self._DoChild(self.body, "div", "letData")
            
    def Paragraph(self, runs):
        para = self._DoChild(self.content, "div", "Entry")
        self._DoRuns(para, runs, "span", None)
        
    def Xhtml(self):
        return self.xhtml
    
    def _DoRuns(self, parent, runs, tag, tagClass):
        for run in runs:
            span = self._DoChild(parent, tag, tagClass)
            span.set("lang", run[0])
            span.text = run[1]
            
    def _DoChild(self, parent, tag, tagClass):
        span = etree.SubElement(parent, XHTML + tag)
        if tagClass != None:
            span.set("class", tagClass)
        return span

def ProjDir():
    try:
        fw7keys = _winreg.OpenKey(_winreg.HKEY_LOCAL_MACHINE,'SOFTWARE\\SIL\\FieldWorks\\7.0')
    except:
        raise MissingSoftware('FieldWorks 7')
    projPath, projPathType = _winreg.QueryValueEx(fw7keys, 'ProjectsDir')
    return projPath

def TextExport(proj, progress):
    os.chdir(os.path.join(ProjDir(), proj))
    data = fwdata(proj + '.fwdata')
    texts = data.TextGuids()
    if progress != None:
        progress.gauge1.SetRange(len(texts))
    outName = '%s %d Texts.xhtml' % (proj, len(texts))
    xhtml = xhtmlDoc(outName, 'main.css')
    n = 0
    for text in texts:
        xhtml.Book(data.TextNames(text))
        contGuid = data.TextContentGuid(text)
        for paraGuid in data.TextParagraphGuids(contGuid):
            xhtml.Paragraph(data.ParagraphRuns(paraGuid))
        if progress != None:
            if progress.canceled:
                break
            n += 1
            progress.gauge1.SetValue(n)
            progress.Update()
    fl = open(outName, 'w')
    fl.write(etree.tostring(xhtml.Xhtml(), encoding='UTF-8', xml_declaration=True))
    fl.close()
    fl = open("main.css", 'w')
    fl.write(mainCss)
    fl.close()
    return outName

mainCss = '''
.letHead {
    column-count: 1;
    clear: both;
}
.letter {
    text-align: center;
    width: 100%;
    margin-top: 18pt;
    margin-bottom: 18pt;
    direction: ltr;
    font-family: "Charis SIL", serif;	/* default Serif font */
    font-weight: bold;
    font-size: 24pt;
}
.letData {
    column-count: 2;   -moz-column-count: 2;
    column-gap: 1.5em; -moz-column-gap: 1.5em;
    column-fill: balance;
    text-align: left;
}
.entry {
    font-family: "Times New Roman", serif;	/* default Serif font */
    font-size: 10pt;	/* inherited */
    border-style: solid;
    border-top-width: 0pt;
    border-bottom-width: 0pt;
    border-left-width: 0pt;
    border-right-width: 0pt;
    text-indent: -36pt;
    margin-left: 36pt;
    padding-left: 9pt;
    padding-bottom: 2pt;
    padding-top: 1pt;
}
'''

def PwDir():
    try:
        pwKeys = _winreg.OpenKey(_winreg.HKEY_LOCAL_MACHINE,'SOFTWARE\\SIL\\Pathway')
    except:
        raise MissingSoftware('Pathway for FieldWorks 7')
    projPath, projPathType = _winreg.QueryValueEx(pwKeys, 'PathwayDir')
    return projPath

def LaunchOpenOffice(proj, outName):
    os.chdir(PwDir())
    projDir = os.path.join(ProjDir(), proj)
    xhtml = os.path.join(projDir, outName)
    css = os.path.join(projDir, "main.css")
    pathwayb = os.popen('PathwayB -x "%s" -c "%s" -l' % (xhtml, css))
    res = pathwayb.read()
    fl = open(os.path.join(projDir, "PathwayB.log"), 'w')
    fl.write(res)
    fl.close()

def ProcessProj(proj, progress=None):
    outName = TextExport(proj, progress)
    return LaunchOpenOffice(proj, outName)

def main():
    proj = 'AkooseDic'
    ProcessProj(proj)

if __name__ == '__main__':
    main()
