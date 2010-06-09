del *.log

candle -nologo Features.wxs >Features.wxs.log
candle -nologo pathwaySE.wxs >pathwaySE.wxs.log
candle -nologo pathwayUI.wxs >pathwayUI.wxs.log
candle -nologo Files.wxs -sw1044 >Files.wxs.log

light -nologo wixca.wixlib Features.wixobj pathwaySE.wixobj Files.wixobj pathwayUI.wixobj -out SetupPwSE.msi >WixLink.log
