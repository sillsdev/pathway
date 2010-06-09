del *.log

candle -nologo Features.wxs >Features.wxs.log
candle -nologo pathwayBTE.wxs >pathwayBTE.wxs.log
candle -nologo pathwayUI.wxs >pathwayUI.wxs.log
candle -nologo Files.wxs -sw1044 >Files.wxs.log

light -nologo wixca.wixlib Features.wixobj pathwayBTE.wixobj Files.wixobj pathwayUI.wixobj -out SetupPwBTE.msi >WixLink.log
