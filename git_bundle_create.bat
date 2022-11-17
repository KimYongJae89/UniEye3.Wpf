@@echo off

REM git bundle create "UniEye.bundle" master

rem Get the time from WMI - at least that's a format we can work with
set X=
for /f "skip=1 delims=" %%x in ('wmic os get localdatetime') do if not defined X set X=%%x
echo.%X%

rem dissect into parts
set DATE.YEAR=%X:~0,4%
set DATE.MONTH=%X:~4,2%
set DATE.DAY=%X:~6,2%
set DATE.HOUR=%X:~8,2%
set DATE.MINUTE=%X:~10,2%
set DATE.SECOND=%X:~12,2%
set DATE.FRACTIONS=%X:~15,6%
set DATE.OFFSET=%X:~21,4%

REM echo %DATE.YEAR% - %DATE.MONTH% - %DATE.DAY% %DATE.HOUR% : %DATE.MINUTE% : %DATE.SECOND% . %DATE.FRACTIONS%

set FILENAME=UniEye3Wpf_master_%DATE.YEAR%%DATE.MONTH%%DATE.DAY%_%DATE.HOUR%%DATE.MINUTE%%DATE.SECOND%.bundle
echo FinleName : %FILENAME%
echo.
echo.
echo.
git bundle create %FILENAME% master

echo.
echo.
echo.
git bundle verify %FILENAME%
pause