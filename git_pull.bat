@echo off

cd /d %~dp0

if "%~1" == "" goto LOCAL
git pull %1 master
goto END
 

 :LOCAL
git pull "UniEye.bundle" master
goto END

 :END
pause