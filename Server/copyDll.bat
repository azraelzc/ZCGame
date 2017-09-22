@SET SolutionDir=%~dp0
@echo SolutionDir
@SET SolutionDir1=%SolutionDir%\output\
@SET SolutionDir2=%SolutionDir%\TianyuCommonServer\Library\
@SET SolutionDir3=%SolutionDir%\GameEditor\plugins\TianyuCommonSkill\

@echo -----------------------------------------------------------------------------------
@echo - 复制最新的DLL到output
@echo -----------------------------------------------------------------------------------
@xcopy /Y /Q    "%SolutionDir%Common\bin\Debug\*.dll"                                   "%SolutionDir1%"
@xcopy /Y /Q    "%SolutionDir%CommonClient\bin\Debug\*.dll"                                   "%SolutionDir1%"
@xcopy /Y /Q    "%SolutionDir%CommonLang\bin\Debug\*.dll"                                   "%SolutionDir1%"
@xcopy /Y /Q    "%SolutionDir%CommonUnit\bin\Debug\*.dll"                                   "%SolutionDir1%"




@echo -----------------------------------------------------------------------------------
@echo - Done
@echo -----------------------------------------------------------------------------------
@pause