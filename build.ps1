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

# health
"######### Health #########"
set-location ./src/Health
& ./build.ps1 $args
Set-Location $cd

if ($LASTEXITCODE -ne 0)
{
    exit $LASTEXITCODE
}

Copy-Item -path .\src\Health\artifacts\packages\*.nupkg -Destination .\nuget

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

# aspnetcore health
"######### AspNetCoreHealth #########"
set-location ./src/AspNetCoreHealth
& ./build.ps1 $args
Set-Location $cd

if ($LASTEXITCODE -ne 0)
{
    exit $LASTEXITCODE
}

Copy-Item -path .\src\AspNetCoreHealth\artifacts\packages\*.nupkg -Destination .\nuget

# aspnetcore health
"######### Azure #########"
set-location ./src/Azure
& ./build.ps1 $args
Set-Location $cd

if ($LASTEXITCODE -ne 0)
{
    exit $LASTEXITCODE
}

Copy-Item -path .\src\Azure\artifacts\packages\*.nupkg -Destination .\nuget