@echo off

set DLL_PATH=bin\Debug\ComWrapper.WinForms.dll
set DLL_64_PATH=bin\x64\Debug\ComWrapper.WinForms.dll
set REGASM=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe
set REGASM_64=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe

if "%1"=="-x86" goto run32

:run64
"%REGASM_64%" /tlb /codebase "%DLL_64_PATH%"
"%REGASM_64%" "%DLL_64_PATH%"
goto end

:run32
"%REGASM%" /tlb /codebase "%DLL_PATH%"
"%REGASM%" "%DLL_PATH%"
goto end

:end
pause