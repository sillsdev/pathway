if not exist "C:\ProgramData\SIL" goto noWin7Set
c:
cd C:\ProgramData\Sil
rmdir /s/q Pathway
:noWin7Set
set base=C:\Projects\btai\PublishingSolution
if exist "%base%" goto found
set base=C:\SIL\btai\PublishingSoltuion
if exist "%base%" goto found
set base=C:\svn\google\pathway
if exist "%base%" goto found
set base=C:\svn\btai\PublishingSolution
if exist "%base%" goto found
set base=E:\btai\PublishingSolution
E:
if exist "%base%" goto found
echo %base% not found. Please update .bat file
goto done
:found
cd %base%\Build\Builder
rmdir /s/q obj
rmdir /s/q bin
cd %base%\Build\SetVersion
rmdir /s/q obj
rmdir /s/q bin
cd %base%\Build\BuilderBTE
rmdir /s/q obj
rmdir /s/q bin
cd %base%\Build\BuilderSE
rmdir /s/q obj
rmdir /s/q bin
cd %base%\Build\Builder7BTE
rmdir /s/q obj
rmdir /s/q bin
cd %base%\Build\Builder7SE
rmdir /s/q obj
rmdir /s/q bin
cd %base%\Build\BuilderBL
rmdir /s/q obj
rmdir /s/q bin
cd %base%\Build\BuildCtx
rmdir /s/q obj
rmdir /s/q bin
cd %base%\BookletBuilder
rmdir /s/q obj
rmdir /s/q bin
cd %base%\ConfigurationTool
rmdir /s/q obj
rmdir /s/q bin
cd %base%\CssDialog
rmdir /s/q obj
rmdir /s/q bin
cd %base%\CssParser
rmdir /s/q obj
rmdir /s/q bin
cd %base%\PathwayB
rmdir /s/q obj
rmdir /s/q bin
cd %base%\InDesignConvert
rmdir /s/q obj
rmdir /s/q bin
cd %base%\LiftPrepare
rmdir /s/q obj
rmdir /s/q bin
cd %base%\OpenOfficeWriter
rmdir /s/q obj
rmdir /s/q bin
cd %base%\OpenOfficeConvert
rmdir /s/q obj
rmdir /s/q bin
cd %base%\PdfConvert
rmdir /s/q obj
rmdir /s/q bin
cd %base%\epubConvert
rmdir /s/q obj
rmdir /s/q bin
cd %base%\epubValidator
rmdir /s/q obj
rmdir /s/q bin
cd %base%\GoBibleConvert
rmdir /s/q obj
rmdir /s/q bin
cd %base%\LogosConvert
rmdir /s/q obj
rmdir /s/q bin
cd %base%\ParatextSupport
rmdir /s/q obj
rmdir /s/q bin
cd %base%\PsExport
rmdir /s/q obj
rmdir /s/q bin
cd %base%\PsTool
rmdir /s/q obj
rmdir /s/q bin
cd %base%\PublishingSolutionExe
rmdir /s/q obj
rmdir /s/q bin
cd %base%\Test
rmdir /s/q obj
rmdir /s/q bin
cd %base%\TestBed
rmdir /s/q obj
rmdir /s/q bin
cd %base%\WordPressConvert
rmdir /s/q obj
rmdir /s/q bin
rmdir /s/q build
rmdir /s/q dist
cd %base%\XeTeXConvert
rmdir /s/q obj
rmdir /s/q bin

cd %base%\Test\ConfigurationTool\TestFiles
rmdir /s /q output
cd %base%\Test\CssDialog\TestFiles
rmdir /s /q output
cd %base%\Test\CssParser\TestFiles
del /q gramOutput\*.*
cd %base%\Test\epubConvert\TestFiles
rmdir /s /q Output
cd %base%\Test\GoBibleConvert\TestFiles
rmdir /s /q Output
cd %base%\Test\LogosConvert\TestFiles
rmdir /s /q Output
cd %base%\Test\InDesignConvert\TestFiles
del /q output\*.*
del /q output\MasterSpreads\*.*
del /q output\Resources\*.*
del /q output\Spreads\*.*
del /q output\Stories\*.*
cd %base%\Test\LiftPrepare\TestFiles
del /q Output\*.*
rmdir /s/q output\akoose
rmdir /s/q output\simple
cd %base%\Test\OpenOfficeConvert\TestFiles
del /q output\*.*
cd %base%\Test\PsExport\TestFiles
rmdir /s/q Output
cd %base%\Test\PsTool\TestFiles
rmdir /s/q output
cd %base%\Test\PublishingSolutionExeUi\TestFiles
rmdir /s/q Output
cd %base%\Test\Stylesheet\TestFiles
rmdir /s/q Output
cd %base%\Test\WordPressConvert\TestFiles
rmdir /s/q output
cd %base%\Test\XeTeXConvert\TestFiles
rmdir /s/q Output

cd %base%\Build\Files
rmdir /s/q PsDll
rmdir /s/q PublishingSolution
rmdir /s/q PathwaySupport
rmdir /s/q ConfigurationTool
:done
pause
