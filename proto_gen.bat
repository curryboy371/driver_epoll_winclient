@echo off

set SCRIPT_DIR=%~dp0
set PROTO_DIR=%SCRIPT_DIR%\_proto
set PROTOC=%PROTO_DIR%\protoc.exe
set OUT_DIR=%SCRIPT_DIR%\chat_client\proto

if not exist "%OUT_DIR%" (
    mkdir "%OUT_DIR%"
)

echo Generating C# code from all proto files...

"%PROTOC%" --csharp_out="%OUT_DIR%" --proto_path="%PROTO_DIR%" ^
    "%PROTO_DIR%\common.proto" ^
	"%PROTO_DIR%\chat.proto" ^
	"%PROTO_DIR%\login.proto" ^
	"%PROTO_DIR%\join.proto" ^
	"%PROTO_DIR%\leave.proto" ^
	"%PROTO_DIR%\admin.proto"

echo Done.
pause