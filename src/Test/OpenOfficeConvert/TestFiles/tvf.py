#-----------------------------------------------------------------------------
# Name:        tvf.py
# Purpose:     Compare output to expected data highlighting changes
# see: http://stackoverflow.com/questions/749796/pretty-printing-xml-in-python
#
# Author:      <greg_trihus@sil.org>
#
# Created:     2013/01/23
# RCS-ID:      $Id: tvf.py $
# Copyright:   (c) 2013 Greg Trihus
# Licence:     <LPGL>
#-----------------------------------------------------------------------------
#Boa:Frame:treeReview

import os,sys,re,wx,glob,shutil
from os.path import basename, splitext
from xml.dom.minidom import parseString

winMerge = "C:\\Program Files (x86)\\WinMerge\\WinMergeU.exe"

def create(parent):
    return treeReview(parent)

[wxID_TREEREVIEW, wxID_TREEREVIEWCOMPARE, wxID_TREEREVIEWCORRECT, 
 wxID_TREEREVIEWLISTBOX1, wxID_TREEREVIEWTEXTCTRL1, wxID_TREEREVIEWTEXTCTRL2, 
 wxID_TREEREVIEWTEXTCTRL3, 
] = [wx.NewId() for _init_ctrls in range(7)]

class treeReview(wx.Frame):
    def _init_ctrls(self, prnt):
        # generated method, don't edit
        wx.Frame.__init__(self, id=wxID_TREEREVIEW, name='treeReview',
              parent=prnt, pos=wx.Point(596, 278), size=wx.Size(1191, 627),
              style=wx.DEFAULT_FRAME_STYLE, title='Tree Review')
        self.SetClientSize(wx.Size(1173, 582))

        self.listBox1 = wx.ListBox(choices=[], id=wxID_TREEREVIEWLISTBOX1,
              name='listBox1', parent=self, pos=wx.Point(32, 32),
              size=wx.Size(240, 496), style=0)
        self.listBox1.Bind(wx.EVT_LISTBOX, self.OnListBox1Listbox,
              id=wxID_TREEREVIEWLISTBOX1)

        self.textCtrl1 = wx.TextCtrl(id=wxID_TREEREVIEWTEXTCTRL1,
              name='textCtrl1', parent=self, pos=wx.Point(304, 32),
              size=wx.Size(280, 496),
              style=wx.TE_MULTILINE | wx.VSCROLL | wx.TE_READONLY | wx.HSCROLL,
              value='')
        self.textCtrl1.SetToolTipString('Input Data')
        self.textCtrl1.SetThemeEnabled(False)

        self.textCtrl2 = wx.TextCtrl(id=wxID_TREEREVIEWTEXTCTRL2,
              name='textCtrl2', parent=self, pos=wx.Point(912, 32),
              size=wx.Size(240, 496),
              style=wx.TE_MULTILINE | wx.VSCROLL | wx.HSCROLL, value='')
        self.textCtrl2.SetToolTipString('Tree View')

        self.correct = wx.Button(id=wxID_TREEREVIEWCORRECT, label='&Correct',
              name='correct', parent=self, pos=wx.Point(1072, 544),
              size=wx.Size(75, 23), style=0)
        self.correct.Bind(wx.EVT_BUTTON, self.OnCorrectButton,
              id=wxID_TREEREVIEWCORRECT)

        self.textCtrl3 = wx.TextCtrl(id=wxID_TREEREVIEWTEXTCTRL3,
              name='textCtrl3', parent=self, pos=wx.Point(616, 32),
              size=wx.Size(264, 496),
              style=wx.HSCROLL | wx.VSCROLL | wx.TE_READONLY | wx.TE_MULTILINE,
              value='')

        self.compare = wx.Button(id=wxID_TREEREVIEWCOMPARE, label='Compare',
              name='compare', parent=self, pos=wx.Point(912, 544),
              size=wx.Size(75, 23), style=0)
        self.compare.Bind(wx.EVT_BUTTON, self.OnCompareButton,
              id=wxID_TREEREVIEWCOMPARE)

    def __init__(self, parent):
        self._init_ctrls(parent)
        self.inp = glob.glob('output/*.xml')
        self.listBox1.Set(self.inp)

    def OnListBox1Listbox(self, event):
        sel = event.GetSelection()
        fileName = splitext(basename(self.inp[sel]))[0].replace('content','').replace('styles','')
        inpName = 'input/%s.xhtml' % fileName
        tmpName1 = self._XmlPretty(inpName)
        inpData = open(tmpName1).read().decode('utf-8')
        self.textCtrl1.SetValue(inpData)
        cssName = 'input/%s.css' % fileName
        cssData = open(cssName).read().decode('utf-8')
        self.textCtrl3.SetValue(cssData)
        treeFile = self.inp[sel]
        tmpName2 = self._XmlPretty(treeFile)
        treeData = open(tmpName2).read().decode('utf-8')
        self.textCtrl2.SetValue(treeData)
        os.unlink(tmpName1)
        os.unlink(tmpName2)

    def OnCorrectButton(self, event):
        sel = self.listBox1.GetSelection()
        base = splitext(basename(self.inp[sel]))[0]
        src = 'output/%s.xml' % base
        dst = 'expected/%s.xml' % base
        shutil.copy2(src, dst)
        wx.MessageBox('%s copied to %s' % (src, dst))

    def _Q(self, s):
        return '"' + s + '"'
		
    def _XmlPretty(self, s):
        text_re = re.compile('>\n\s+([^<>\s].*?)\n\s+</', re.DOTALL)
        data = open(s).read()
        xmldom = parseString(data)
        uglyXml = xmldom.toprettyxml(indent='  ')
        prettyXml0 = text_re.sub('>\g<1></', uglyXml)
        prettyXml1 = re.compile(' +\n').sub('\n', prettyXml0)   # remove spaces at end of lines
        prettyXml = re.compile('\n+', re.MULTILINE).sub('\n', prettyXml1) # multiple lines to 1
        tempName = os.path.join(os.getenv('TMP'),s)
        try:
            os.makedirs(os.path.dirname(tempName))
        except:
            pass
        f = open(tempName, 'w')
        f.write(prettyXml.encode('utf-8'))
        f.close()
        return tempName
        
    def OnCompareButton(self, event):
        sel = self.listBox1.GetSelection()
        base = splitext(basename(self.inp[sel]))[0]
        src = self._XmlPretty('output/%s.xml' % base)
        dst = self._XmlPretty('expected/%s.xml' % base)
        progParts = os.path.split(winMerge)
        os.spawnl(os.P_WAIT, winMerge, progParts[1], self._Q(src), self._Q(dst))
        os.unlink(src)
        os.unlink(dst)
