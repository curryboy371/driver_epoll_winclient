@echo off
setlocal enabledelayedexpansion
set interval=3000000

echo Starting app with ID=test1 interval=!interval!
start chat_client\bin\Release\chat_client.exe debug test1 !interval!
timeout /t 1 >nul

echo All done!
pause