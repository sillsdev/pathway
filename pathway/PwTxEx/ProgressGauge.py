#-----------------------------------------------------------------------------
# Name:        ProgressGauge.py
# Purpose:     Progress dialog for TextExport
#
# Author:      <greg_trihus@sil.org>
#
# Created:     2011/01/26
# RCS-ID:      $Id: ProgressGauge.py $
# Copyright:   (c) 2011 SIL International
# Licence:     <mit>
#-----------------------------------------------------------------------------
#Boa:Frame:ProgressGauge

import wx

def create(parent):
    return ProgressGauge(parent)

[wxID_PROGRESSGAUGE, wxID_PROGRESSGAUGECANCEL, wxID_PROGRESSGAUGEGAUGE1, 
 wxID_PROGRESSGAUGESTATICTEXT1, 
] = [wx.NewId() for _init_ctrls in range(4)]

class ProgressGauge(wx.Dialog):
    def _init_ctrls(self, prnt):
        # generated method, don't edit
        wx.Dialog.__init__(self, id=wxID_PROGRESSGAUGE, name='ProgressGauge',
              parent=prnt, pos=wx.Point(767, 351), size=wx.Size(315, 135),
              style=wx.STATIC_BORDER | wx.NO_BORDER, title='Progress')
        self.SetClientSize(wx.Size(315, 135))

        self.gauge1 = wx.Gauge(id=wxID_PROGRESSGAUGEGAUGE1, name='gauge1',
              parent=self, pos=wx.Point(24, 40), range=100, size=wx.Size(264,
              28), style=wx.GA_HORIZONTAL)

        self.staticText1 = wx.StaticText(id=wxID_PROGRESSGAUGESTATICTEXT1,
              label='Texts Processed', name='staticText1', parent=self,
              pos=wx.Point(104, 16), size=wx.Size(94, 16), style=0)

        self.cancel = wx.Button(id=wxID_PROGRESSGAUGECANCEL, label='Cancel',
              name='cancel', parent=self, pos=wx.Point(112, 80),
              size=wx.Size(87, 28), style=0)
        self.cancel.Bind(wx.EVT_BUTTON, self.OnCancelButton,
              id=wxID_PROGRESSGAUGECANCEL)

    def __init__(self, parent):
        self._init_ctrls(parent)
        self.canceled = False

    def OnCancelButton(self, event):
        self.canceled = True
        self.Hide()
