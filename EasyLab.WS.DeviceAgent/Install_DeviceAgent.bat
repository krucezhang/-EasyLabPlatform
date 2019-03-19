@ECHO OFF

REM The following directory is for .NET 4.5
set DOTNETFX4=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX4%
set RUNDIR=%~dp0

echo ---------------------------------
echo Installing EasyLabDeviceAgent 
echo --------------------------------

cd /d %RUNDIR%

InstallUtil /i ./bin/EasyLab.WS.DeviceAgent.exe
sc failure "EasyLabDeviceAgent"  actions= restart/60000/restart/60000// reset= 0
echo.

echo ---------------------------
echo Starting EasyLabDeviceAgent
echo --------------------------
net start "EasyLabDeviceAgent"
echo.
