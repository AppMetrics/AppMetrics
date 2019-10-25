New-Item -ItemType Directory -Force -Path ./nuget
Remove-Item -Path ./nuget/*.*

$cd = Get-Location

# core
"######### AppMetrics #########"
set-location ./src/Core
& ./build.ps1 $args
Set-Location $cd

if ($LASTEXITCODE -ne 0)
{
    exit $LASTEXITCODE
}

Copy-Item -path .\src\Core\artifacts\packages\*.nupkg -Destination .\nuget

# extensions
"######### Extensions #########"
set-location ./src/Extensions
& ./build.ps1 $args
Set-Location $cd

if ($LASTEXITCODE -ne 0)
{
    exit $LASTEXITCODE
}

Copy-Item -path .\src\Extensions\artifacts\packages\*.nupkg -Destination .\nuget

# reporting
"######### Reporting #########"
set-location ./src/Reporting
& ./build.ps1 $args
Set-Location $cd

if ($LASTEXITCODE -ne 0)
{
    exit $LASTEXITCODE
}

Copy-Item -path .\src\Reporting\artifacts\packages\*.nupkg -Destination .\nuget

# aspnetcore
"######### AspNetCore #########"
set-location ./src/AspNetCore
& ./build.ps1 $args
Set-Location $cd

if ($LASTEXITCODE -ne 0)
{
    exit $LASTEXITCODE
}

Copy-Item -path .\src\AspNetCore\artifacts\packages\*.nupkg -Destination .\nuget