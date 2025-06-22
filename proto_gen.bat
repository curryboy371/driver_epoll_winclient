@echo off

REM 현재 배치파일이 있는 경로를 기준으로 사용
set SCRIPT_DIR=%~dp0
set PROTO_DIR=%SCRIPT_DIR%\_proto
set PROTOC=%PROTO_DIR%\protoc.exe

set OUT_DIR=%SCRIPT_DIR%\chat_client\proto

REM 출력 폴더 없으면 생성
if not exist "%OUT_DIR%" (
    mkdir "%OUT_DIR%"
)

echo Generating C# code from all proto files...

REM 모든 .proto 파일 순회
for %%f in ("%PROTO_DIR%\*.proto") do (
    echo Processing %%~nxf ...
    "%PROTOC%" --csharp_out="%OUT_DIR%" --proto_path="%PROTO_DIR%" "%%f"
)

echo Done.
pause