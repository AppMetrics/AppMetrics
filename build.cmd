@echo off
cd %~dp0

IF EXIST .nuget\nuget.exe goto restore

echo Downloading nuget.exe
md .nuget
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile '.nuget\nuget.exe'"

:restore
IF EXIST packages goto run
.nuget\NuGet.exe install psake -ExcludeVersion -o packages -nocache
.nuget\NuGet.exe install newtonsoft.json -Version 7.0.1 -ExcludeVersion -o packages -nocache

:run
:: Get Psake to Return Non-Zero Return Code on Build Failure (https://github.com/psake/psake/issues/58)
@powershell -NoProfile -ExecutionPolicy unrestricted -command "&{ Import-Module .\packages\psake\tools\psake.psm1; Invoke-Psake .\scripts\default.ps1 -parameters @{'configuration'='Release'}; exit !($psake.build_success) }"