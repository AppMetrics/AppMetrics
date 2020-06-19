$ErrorActionPreference = "Stop";

New-Item -ItemType Directory -Force -Path ./nuget

pushd ./src/Concurrency
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/Core
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/Extensions
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/AspNetCore
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/Reporting
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/NServiceBus
Invoke-Expression "./build.ps1 $args"
popd

pushd ./src/MetaPackages
Invoke-Expression "./build.ps1 $args"
popd