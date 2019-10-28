$ErrorActionPreference = "Stop";

New-Item -ItemType Directory -Force -Path ./nuget

# core
"######### AppMetrics #########"
pushd ./src/Core
./build.ps1 $args
popd

# extensions
"######### Extensions #########"
pushd ./src/Extensions
./build.ps1 $args
popd

# # reporting
# "######### Reporting #########"
# set-location ./src/Reporting
# & ./build.ps1 $args
# Set-Location $cd

# if ($LASTEXITCODE -ne 0)
# {
#     exit $LASTEXITCODE
# }

# Copy-Item -path .\src\Reporting\artifacts\packages\*.nupkg -Destination .\nuget

# # aspnetcore
# "######### AspNetCore #########"
# set-location ./src/AspNetCore
# & ./build.ps1 $args
# Set-Location $cd

# if ($LASTEXITCODE -ne 0)
# {
#     exit $LASTEXITCODE
# }

# Copy-Item -path .\src\AspNetCore\artifacts\packages\*.nupkg -Destination .\nuget