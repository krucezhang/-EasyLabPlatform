@ECHO OFF

REM The following directory is for .NET 4.5
set DOTNETFX4=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX4%
set RUNDIR=%~dp0

cd /d %RUNDIR%

InstallUtil /u ./bin/EasyLab.WS.DeviceAgent.exe
echo.

