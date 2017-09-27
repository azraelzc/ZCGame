@SET DBPath="C:\Program Files\MySQL\MySQL Server 5.5\bin"
@SET PWD=123456
@SET DBName=zcgame
@SET SolutionDir=%~dp0
@SET SQLFile=%SolutionDir%sql.txt
@SET SQLStr = ""
echo %SQLFile%
set aa=伟大的中国！
set bb=我为你自豪
echo %aa%%bb%

echo aa=%aa%
echo bb=%bb%
set "aa=%aa%%bb%"
echo aa=%aa%

FOR /F "delims=" %%i in (%SQLFile%) do (SET SQLStr=%SQLStr%%PWD%)
echo %SQLStr%
cd /d %DBPath%
@pause