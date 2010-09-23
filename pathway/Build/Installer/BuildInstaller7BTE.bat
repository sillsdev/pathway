del *.log

candle -nologo Features.wxs >Features.wxs.log
candle -nologo pathway7BTE.wxs >pathway7BTE.wxs.log
candle -nologo pathwayUI.wxs >pathwayUI.wxs.log
candle -nologo Files.wxs -sw1044 >Files.wxs.log

light -nologo wixca.wixlib Features.wixobj pathway7BTE.wixobj Files.wixobj pathwayUI.wixobj -out SetupPw7BTE.msi >WixLink.log
