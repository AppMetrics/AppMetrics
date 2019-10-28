$ErrorActionPreference = "Stop";

New-Item -ItemType Directory -Force -Path ./nuget

pushd ./src/Core
./build.ps1 $args
popd

pushd ./src/Extensions
./build.ps1 $args
popd

pushd ./src/Reporting
./build.ps1 $args
popd

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