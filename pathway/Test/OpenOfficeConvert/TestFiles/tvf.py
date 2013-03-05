#-----------------------------------------------------------------------------
# Name:        tvf.py
# Purpose:     Compare output to expected data highlighting changes
#
# Author:      <greg_trihus@sil.org>
#
# Created:     2013/01/23
# RCS-ID:      $Id: tvf.py $
# Copyright:   (c) 2013 Greg Trihus
# Licence:     <LPGL>
#-----------------------------------------------------------------------------
#Boa:Frame:treeReview

import os,wx,glob,shutil
from os.path import basename, splitext

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
        self.inp = glob.glob('Output/*.xml')
        self.listBox1.Set(self.inp)

    def OnListBox1Listbox(self, event):
        sel = event.GetSelection()
        fileName = splitext(basename(self.inp[sel]))[0].replace('content','').replace('styles','')
        inpName = 'Input/%s.xhtml' % fileName
        inpData = open(inpName).read().decode('utf-8')
        self.textCtrl1.SetValue(inpData)
        cssName = 'Input/%s.css' % fileName
        cssData = open(cssName).read().decode('utf-8')
        self.textCtrl3.SetValue(cssData)
        treeFile = self.inp[sel]
        treeData = open(treeFile).read().decode('utf-8')
        res = ''
        cnt = 0
        for c in treeData:
            if c == '(':
                res += '\n'
                cnt += 4;
                for x in range(cnt):
                    res += ' ';
                res += c
            elif c == ')':
                cnt -= 4;
                res += c
            else:
                res += c
        self.textCtrl2.SetValue(res)

    def OnCorrectButton(self, event):
        sel = self.listBox1.GetSelection()
        base = splitext(basename(self.inp[sel]))[0]
        src = 'Output/%s.xml' % base
        dst = 'Expected/%s.xml' % base
        shutil.copy2(src, dst)
        wx.MessageBox('%s copied to %s' % (src, dst))

    def _Q(self, s):
        return '"' + s + '"'
        
    def OnCompareButton(self, event):
        sel = self.listBox1.GetSelection()
        base = splitext(basename(self.inp[sel]))[0]
        src = 'Output/%s.xml' % base
        dst = 'Expected/%s.xml' % base
        progParts = os.path.split(winMerge)
        os.spawnl(os.P_WAIT, winMerge, progParts[1], self._Q(src), self._Q(dst))
