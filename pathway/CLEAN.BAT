E:
cd E:\btai\PublishingSolution\Build\Builder
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\Build\SetVersion
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\Build\BuilderBTE
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\Build\SetVersionBTE
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\Build\BuilderSE
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\Build\SetVersionSE
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\Build\BuilderBL
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\ConfigurationTool
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\CssDialog
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\CssParser
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\InDesignConvert
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\LiftPrepare
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\OdtConvert
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\OpenOfficeConvert
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\PdfConvert
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\GoBibleConvert
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\PsExport
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\PsTool
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\PublishingSolutionExe
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\Test
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\TestBed
rmdir /s/q obj
rmdir /s/q bin
cd E:\btai\PublishingSolution\XeTeXConvert
rmdir /s/q obj
rmdir /s/q bin

cd E:\btai\PublishingSolution\Test\ConfigurationTool\TestFiles
del /q output\*.*
cd E:\btai\PublishingSolution\Test\CssParser\TestFiles
del /q gramOutput\*.*
cd E:\btai\PublishingSolution\Test\GoBibleConvert\TestFiles
del /s/q output
cd E:\btai\PublishingSolution\Test\InDesignConvert\TestFiles
del /q output\*.*
del /q output\MasterSpreads\*.*
del /q output\Resources\*.*
del /q output\Spreads\*.*
del /q output\Stories\*.*
cd E:\btai\PublishingSolution\Test\LiftPrepare\TestFiles
del /q Output\*.*
rmdir /s/q output\akoose
rmdir /s/q output\simple
cd E:\btai\PublishingSolution\Test\OpenOfficeConvert\TestFiles
del /q output\*.*
cd E:\btai\PublishingSolution\Test\PsExport\TestFiles
rmdir /s/q Output
cd E:\btai\PublishingSolution\Test\PsTool\TestFiles
rmdir /s/q output
cd E:\btai\PublishingSolution\Test\PublishingSolutionExeUi\TestFiles
rmdir /s/q Output
cd E:\btai\PublishingSolution\Test\Stylesheet\TestFiles
rmdir /s/q Output
cd E:\btai\PublishingSolution\Test\XeTeXConvert\TestFiles
rmdir /s/q Output

cd E:\btai\PublishingSolution\Build\Files
rmdir /s/q PsDll
rmdir /s/q PublishingSolution
rmdir /s/q PathwaySupport
rmdir /s/q ConfigurationTool
pause
