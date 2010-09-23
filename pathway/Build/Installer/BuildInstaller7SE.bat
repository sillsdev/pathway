del *.log

candle -nologo Features.wxs >Features.wxs.log
candle -nologo pathway7SE.wxs >pathway7SE.wxs.log
candle -nologo pathwayUI.wxs >pathwayUI.wxs.log
candle -nologo Files.wxs -sw1044 >Files.wxs.log

light -nologo wixca.wixlib Features.wixobj pathway7SE.wixobj Files.wixobj pathwayUI.wixobj -out SetupPw7SE.msi >WixLink.log
