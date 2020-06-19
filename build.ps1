$ErrorActionPreference = "Stop";

New-Item -ItemType Directory -Force -Path ./nuget

pushd ./src/Core
./build.ps1 $args
popd

pushd ./src/Extensions
./build.ps1 $args
popd

pushd ./src/AspNetCore
./build.ps1 $args
popd

pushd ./src/Reporting
set-location ./src/AspNetCore
./build.ps1 $args
popd

pushd ./src/NServiceBus
./build.ps1 $args
popd

pushd ./src/MetaPackages
./build.ps1 $args
popd