#!/usr/bin/env bash
set -euo pipefail

mkdir nuget -p

dotnet tool restore

pushd ./src/Concurrency
./build.sh "$@"
popd

pushd ./src/Core
./build.sh "$@"
popd

pushd ./src/Extensions
./build.sh "$@"
popd

pushd ./src/AspNetCore
./build.sh "$@"
popd

pushd ./src/Reporting
./build.sh "$@"
popd

pushd ./src/NServiceBus
./build.sh "$@"
popd

pushd ./src/MetaPackages
./build.sh "$@"
popd