del *.log

candle -nologo Features.wxs >Features.wxs.log
candle -nologo PwCtx.wxs >PwCtx.wxs.log
candle -nologo PwCtxUI.wxs >PwCtxUI.wxs.log
candle -nologo Files.wxs -sw1044 >Files.wxs.log

light -nologo wixca.wixlib Features.wixobj PwCtx.wixobj Files.wixobj PwCtxUI.wixobj -out SetupCtx.msi >WixLink.log
