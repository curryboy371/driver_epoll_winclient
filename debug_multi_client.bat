@echo off
setlocal enabledelayedexpansion
set interval=200


:: for /L %%I in (start, step, end)
for /L %%I in (1,1,10) do (
    echo Starting app with ID=%%I interval=!interval!
    start chat_client\bin\Release\chat_client.exe debug %%I !interval! 10
    timeout /t 1 >nul
)

echo All done!
pause