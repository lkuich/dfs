setlocal

cd /d %~dp0

set TOOLS_PATH=C:\Users\loren\.nuget\packages\Grpc.Tools\1.15.0\tools\windows_x64

set PROJECT=protos/gen

%TOOLS_PATH%\protoc.exe -I protos --csharp_out %PROJECT% protos/io.proto --grpc_out %PROJECT% --plugin=protoc-gen-grpc=%TOOLS_PATH%\grpc_csharp_plugin.exe

endlocal
pause