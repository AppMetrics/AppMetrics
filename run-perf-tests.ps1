Push-Location $PSScriptRoot

.\build.cmd

foreach ($test in ls test/*.Performance.Tests) {
    Push-Location $test

	echo "perf: Running benchmark test project in $test"

    & dotnet test -c Release
    if($LASTEXITCODE -ne 0) { exit 2 }

    Pop-Location
}

Copy-Item $test/BenchmarkDotNet.Artifacts/results -Destination ./artifacts/perf-tests -Recurse -Force


Pop-Location