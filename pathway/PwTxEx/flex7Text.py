#-----------------------------------------------------------------------------
# Name:        flex7Text.py
# Purpose:     Exports Texts from Fieldworks into XHTML for use by Pathway
#
# Author:      <greg_trihus@sil.org>
#
# Created:     2011/01/23
# RCS-ID:      $Id: flex7Text.py $
# Copyright:   (c) 2011 SIL International
# Licence:     <mit>
#-----------------------------------------------------------------------------
#!/usr/bin/env python
#Boa:PyApp:main

import os, _winreg
from lxml import etree

XHTML_NAMESPACE = "http://www.w3.org/1999/xhtml"
XHTML = "{%s}" % XHTML_NAMESPACE
NSMAP = {None : XHTML_NAMESPACE} # default namespace
FOLDER = 'TextExport'

modules ={}

class MissingSoftware(Exception):
    def __init__(self, value):
        self.value = value
        
    def __str__(self):
        return repr(self.value)
    
class DataFormat(Exception):
    def __init__(self, value):
        self.value = value
    
    def __str(self):
        return repr(self.value)

class fwdata:
    def __init__(self, fullname):
        try:
            self.root = etree.parse(fullname)
        except IOError:
            nameparts = os.path.splitext(fullname)
            if os.path.isfile(nameparts[0] + '.fwdb'):
                raise DataFormat(fullname)
            raise
        
    def TextNodes(self):
        return self.root.xpath('//rt[@class="Text"]')
    
    def Names(self, node):
        print 'Names for', node.attrib['guid']
        nameNode = node.find('Name')
        if nameNode == None:
            return []
        return self._ProcRuns(list(nameNode))
    
    def ContentGuid(self, node):
        return node.find('Contents').find('objsur').attrib['guid']
       
    def TextParagraphGuids(self, guid):
        print 'Paragraphs for', guid
        return self.root.xpath('//rt[@class="StText"][@guid="%s"]/Paragraphs/objsur/@guid' % guid)
    
    def ParagraphRuns(self, guid):
        print 'Runs for', guid
        return self._ProcRuns(self.root.xpath('//rt[@class="StTxtPara"][@guid="%s"]/Contents/Str/Run' % guid))
    
    def _ProcRuns(self, runnodes):
        runs = []
        for runnode in runnodes:
            text = runnode.text
            if text:
                runs.append([runnode.attrib['ws'], text])
        return runs
    
class WritingSystemFonts:
    def __init__(self, proj):
        self.ws = {}
        myProj = os.path.join(ProjDir(), proj)
        self.wsPath = os.path.join(myProj, "WritingSystemStore")
        
    def Add(self, ws):
        if self.ws.has_key(ws):
            return
        fullName = os.path.join(self.wsPath, ws + ".ldml")
        if os.path.isfile(fullName):
            ldml = open(fullName).read()
            begText = '<palaso:defaultFontFamily value="'
            i = ldml.index(begText)
            if i > 0:
                i += len(begText)
                j = ldml[i:].index('"')
                self.ws[ws] = ldml[i:i + j]
                
    def AddList(self, runs):
        for run in runs:
            self.Add(run[0])
                
    def Css(self):
        css = ''
        for wsPair in self.ws.items():
            css += 'span[lang="%s"] { font-family: "%s"; }\n' % wsPair
        return css

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
        for run in runs:
            book = self._DoChild(self.body, "div", "mainTitle")
            self._OneRun(book, run, "span", None)
        self.content = self._DoChild(self.body, "div", "columns")
            
    def Paragraph(self, runs):
        para = self._DoChild(self.content, "div", "content")
        self._DoRuns(para, runs, "span", None)
        
    def Xhtml(self):
        return self.xhtml
    
    def _DoRuns(self, parent, runs, tag, tagClass):
        for run in runs:
            self._OneRun(parent, run, tag, tagClass)

    def _OneRun(self, parent, run, tag, tagClass):
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
    textNodes = data.TextNodes()
    if progress != None:
        progress.gauge1.SetRange(len(textNodes))
    outName = '%s %d Texts.xhtml' % (proj, len(textNodes))
    xhtml = xhtmlDoc(outName, 'main.css')
    n = 0
    ws = WritingSystemFonts(proj)
    for textNode in textNodes:
        names = data.Names(textNode)
        ws.AddList(names)
        xhtml.Book(names)
        contGuid = data.ContentGuid(textNode)
        for paraGuid in data.TextParagraphGuids(contGuid):
            runs = data.ParagraphRuns(paraGuid)
            ws.AddList(runs)
            xhtml.Paragraph(runs)
        if progress != None:
            if progress.canceled:
                break
            n += 1
            progress.gauge1.SetValue(n)
            progress.Update()
    if not os.path.isdir(FOLDER):
        os.mkdir(FOLDER)
    os.chdir(FOLDER)
    fl = open(outName, 'w')
    fl.write(etree.tostring(xhtml.Xhtml(), encoding='UTF-8', xml_declaration=True))
    fl.close()
    fl = open("main.css", 'w')
    fl.write(mainCss % ws.Css())
    fl.close()
    return outName

mainCss = '''
.mainTitle {
    text-align: center;
    font-weight: bold;
    font-size: 24pt;
    margin-top: 18pt;
    margin-bottom: 6pt;
    column-count: 1;
    direction: ltr;
}
.columns {
    column-count: 2;   -moz-column-count: 2;
    column-gap: 1.5em; -moz-column-gap: 1.5em;
    column-fill: balance;
    text-align: left;
}
.content {
    /* font-family: "Charis SIL", serif;	 default Serif font */
    font-size: 12pt;	/* inherited */
    border-style: solid;
    border-top-width: 0pt;
    border-bottom-width: 0pt;
    border-left-width: 0pt;
    border-right-width: 0pt;
    text-indent: 36pt;
    margin-left: 0pt;
    padding-left: 9pt;
    padding-bottom: 2pt;
    padding-top: 1pt;
}
%s
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
    folder = os.path.join(projDir, FOLDER)
    xhtml = os.path.join(folder, outName)
    css = os.path.join(folder, "main.css")
    pathwayb = os.popen('PathwayB -x "%s" -c "%s" -l' % (xhtml, css))
    res = pathwayb.read()
    fl = open(os.path.join(projDir, "PathwayB.log"), 'w')
    fl.write(res)
    fl.close()

def ProcessProj(proj, progress=None):
    outName = TextExport(proj, progress)
    if progress:
        parent = progress.GetParent()
        parent.Produced(os.path.join(os.path.join(os.path.join(ProjDir(), proj), FOLDER), outName))
    LaunchOpenOffice(proj, outName)

def main():
    proj = 'AkooseDic'
    ProcessProj(proj)

if __name__ == '__main__':
    main()
