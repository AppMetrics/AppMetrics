#!/usr/bin/env bash
set -euo pipefail

mkdir nuget -p

dotnet tool restore

pushd ./src/AppMetrics
./build.sh "$@"
popd