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

pushd ./src/AspNetCore
./build.ps1 $args
popd