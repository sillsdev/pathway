#!/usr/bin/env python
#Boa:App:BoaApp

import wx

import TextExportF

modules ={u'TextExportF': [1, 'Main frame of Application', u'TextExportF.py'],
          u'ProgressGauge': [1, 'Progress Bar Frame', u'ProgressGauge.py']}

class BoaApp(wx.App):
    def OnInit(self):
        self.main = TextExportF.create(None)
        self.main.Show()
        self.SetTopWindow(self.main)
        return True

def main():
    application = BoaApp(0)
    application.MainLoop()

if __name__ == '__main__':
    main()
