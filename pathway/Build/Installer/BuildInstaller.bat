del *.log

candle -nologo Features.wxs >Features.wxs.log
candle -nologo OOS.wxs >OOS.wxs.log
candle -nologo OosUI.wxs >OosUI.wxs.log
candle -nologo Files.wxs -sw1044 >Files.wxs.log

light -nologo wixca.wixlib Features.wixobj OOS.wixobj Files.wixobj OosUI.wixobj -out SetupOos.msi >WixLink.log