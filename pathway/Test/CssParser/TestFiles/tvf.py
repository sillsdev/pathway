#Boa:Frame:treeReview

import wx,glob,shutil
from os.path import basename, splitext

def create(parent):
    return treeReview(parent)

[wxID_TREEREVIEW, wxID_TREEREVIEWCORRECT, wxID_TREEREVIEWLISTBOX1, 
 wxID_TREEREVIEWTEXTCTRL1, wxID_TREEREVIEWTEXTCTRL2, 
] = [wx.NewId() for _init_ctrls in range(5)]

class treeReview(wx.Frame):
    def _init_ctrls(self, prnt):
        # generated method, don't edit
        wx.Frame.__init__(self, id=wxID_TREEREVIEW, name='treeReview',
              parent=prnt, pos=wx.Point(237, 105), size=wx.Size(1046, 616),
              style=wx.DEFAULT_FRAME_STYLE, title='Tree Review')
        self.SetClientSize(wx.Size(1038, 582))

        self.listBox1 = wx.ListBox(choices=[], id=wxID_TREEREVIEWLISTBOX1,
              name='listBox1', parent=self, pos=wx.Point(32, 32),
              size=wx.Size(132, 496), style=0)
        self.listBox1.Bind(wx.EVT_LISTBOX, self.OnListBox1Listbox,
              id=wxID_TREEREVIEWLISTBOX1)

        self.textCtrl1 = wx.TextCtrl(id=wxID_TREEREVIEWTEXTCTRL1,
              name='textCtrl1', parent=self, pos=wx.Point(192, 32),
              size=wx.Size(544, 496),
              style=wx.TE_MULTILINE | wx.VSCROLL | wx.TE_READONLY | wx.HSCROLL,
              value='')
        self.textCtrl1.SetToolTipString('Input Data')
        self.textCtrl1.SetThemeEnabled(False)

        self.textCtrl2 = wx.TextCtrl(id=wxID_TREEREVIEWTEXTCTRL2,
              name='textCtrl2', parent=self, pos=wx.Point(768, 32),
              size=wx.Size(240, 496),
              style=wx.TE_MULTILINE | wx.VSCROLL | wx.HSCROLL, value='')
        self.textCtrl2.SetToolTipString('Tree View')

        self.correct = wx.Button(id=wxID_TREEREVIEWCORRECT, label='&Correct',
              name='correct', parent=self, pos=wx.Point(848, 544),
              size=wx.Size(75, 23), style=0)
        self.correct.Bind(wx.EVT_BUTTON, self.OnCorrectButton,
              id=wxID_TREEREVIEWCORRECT)

    def __init__(self, parent):
        self._init_ctrls(parent)
        self.inp = glob.glob('gramInput/*.css')
        self.listBox1.Set(self.inp)

    def OnListBox1Listbox(self, event):
        sel =event.GetSelection()
        inpData = open(self.inp[sel]).read()
        self.textCtrl1.SetValue(inpData)
        treeFile = 'gramOutput/%s.txt' % splitext(basename(self.inp[sel]))[0]
        treeData = open(treeFile).read()
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
        src = 'gramOutput/%s.txt' % base
        dst = 'gramExpect/%s.txt' % base
        shutil.copy2(src, dst)
        wx.MessageBox('%s copied to %s' % (src, dst))
