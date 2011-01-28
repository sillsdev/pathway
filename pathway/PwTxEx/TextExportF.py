#-----------------------------------------------------------------------------
# Name:        TextExportF.py
# Purpose:     Main form for TextExport application. Handles ui
#
# Author:      <greg_trihus@sil.org>
#
# Created:     2011/01/26
# RCS-ID:      $Id: TextExportF.py $
# Copyright:   (c) 2011 SIL International
# Licence:     <mit>
#-----------------------------------------------------------------------------
#Boa:Frame:TextExport

import wx, os, sys, glob
import flex7Text as f7tx
import ProgressGauge

version = '0.1.5.523'

def create(parent):
    return TextExport(parent)

[wxID_TEXTEXPORT, wxID_TEXTEXPORTINDESIGN, wxID_TEXTEXPORTLISTBOX1, 
 wxID_TEXTEXPORTOK, wxID_TEXTEXPORTSTATICTEXT1, 
] = [wx.NewId() for _init_ctrls in range(5)]

class TextExport(wx.Frame):
    def _init_ctrls(self, prnt):
        # generated method, don't edit
        wx.Frame.__init__(self, id=wxID_TEXTEXPORT, name='TextExport',
              parent=prnt, pos=wx.Point(686, 257), size=wx.Size(329, 453),
              style=wx.DEFAULT_FRAME_STYLE, title='Text Export')
        self.SetClientSize(wx.Size(311, 408))

        self.staticText1 = wx.StaticText(id=wxID_TEXTEXPORTSTATICTEXT1,
              label='Choose Project', name='staticText1', parent=self,
              pos=wx.Point(16, 16), size=wx.Size(86, 16), style=0)

        self.listBox1 = wx.ListBox(choices=[], id=wxID_TEXTEXPORTLISTBOX1,
              name='listBox1', parent=self, pos=wx.Point(16, 40),
              size=wx.Size(272, 312), style=0)

        self.Ok = wx.Button(id=wxID_TEXTEXPORTOK, label='Ok', name='Ok',
              parent=self, pos=wx.Point(104, 368), size=wx.Size(87, 28),
              style=0)
        self.Ok.Bind(wx.EVT_BUTTON, self.OnOkButton, id=wxID_TEXTEXPORTOK)

        self.inDesign = wx.CheckBox(id=wxID_TEXTEXPORTINDESIGN,
              label='InDesign', name='inDesign', parent=self, pos=wx.Point(200,
              16), size=wx.Size(82, 16), style=0)
        self.inDesign.SetValue(True)

    def __init__(self, parent):
        self._init_ctrls(parent)
        self.SetTitle('%s %s' % (self.GetTitle(), version))
        self.SetIcon(wx.Icon(u'graphics/CHECKFRE.ICO',wx.BITMAP_TYPE_ICO))
        projDir = f7tx.ProjDir()
        self.projs = []
        for n in glob.glob(os.path.join(projDir, "*")):
            if os.path.isdir(n):
                self.projs.append(os.path.basename(n))
        self.listBox1.Set(self.projs)

    def OnOkButton(self, event):
        sel = self.listBox1.GetSelection()
        if sel >= 0:
            pg = ProgressGauge.ProgressGauge(self)
            pg.Show()
            pg.Update()
            try:
                f7tx.ProcessProj(self.projs[sel], pg, self.inDesign.GetValue())
            except f7tx.MissingSoftware, e:
                dlg = wx.MessageDialog(self, 'Please install %s' % e.value, 'Missing Software', wx.OK | wx.ICON_ERROR)
                try:
                    dlg.ShowModal()
                finally:
                    dlg.Destroy()
                    sys.exit(-2)
            except f7tx.DataFormat:
                dlg = wx.MessageDialog(self, 'Please convert this project to the stand-alone format.', 'Data Format', wx.OK | wx.ICON_ERROR)
                try:
                    dlg.ShowModal()
                finally:
                    dlg.Destroy()
                    pg.Destroy()
                    return
            self.Destroy()
        else:
            dlg = wx.MessageDialog(self, 'Please select a project', 'Error', wx.OK | wx.ICON_ERROR)
            try:
                dlg.ShowModal()
            finally:
                dlg.Destroy()
            
    def Produced(self, name):
        dlg = wx.MessageDialog(self, '%s created.' % name, 'Intermediate Result', wx.OK | wx.ICON_INFORMATION)
        try:
            dlg.ShowModal()
        finally:
            dlg.Destroy()
