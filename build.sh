#!/usr/bin/env bash
set -euo pipefail

mkdir nuget -p

dotnet tool restore

pushd ./src/Core
./build.sh "$@"
popd

pushd ./src/Extensions
./build.ps1 "$@"
popd

pushd ./src/Reporting
./build.ps1 $args
popd

pushd ./src/AspNetCore
./build.ps1 $args
popd