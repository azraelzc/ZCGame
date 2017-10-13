@SET DBPath="C:\Program Files\MySQL\MySQL Server 5.5\bin"
@SET PWD=123456
@SET DBName=zcgame
@SET SolutionDir=%~dp0
@SET SQLFile=%SolutionDir%sql.txt
@SET SQLStr = ""
echo %SQLFile%
set aa=11111
set bb=22222
echo %aa%%bb%

echo aa=%aa%
echo bb=%bb%
set aa=%aa%%bb%
set aa=%aa%%bb%
echo aa=%aa%

FOR /F "delims=" %%i in (%SQLFile%) do (SET SQLStr=%SQLStr%%%i)
echo %SQLStr%
cd /d %DBPath%
@pause