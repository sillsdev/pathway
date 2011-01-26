del *.log

candle -nologo Features.wxs >Features.wxs.log
candle -nologo PwTxEx.wxs >PwTxEx.wxs.log
candle -nologo PwTxExUI.wxs >PwTxExUI.wxs.log
candle -nologo Files.wxs -sw1044 >Files.wxs.log

light -nologo wixca.wixlib Features.wixobj PwTxEx.wixobj Files.wixobj PwTxExUI.wixobj -out SetupTxEx.msi >WixLink.log
