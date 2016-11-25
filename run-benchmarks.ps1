Push-Location $PSScriptRoot

#.\build.cmd

foreach ($test in ls benchmarks/*.Benchmarks) {
    Push-Location $test

	echo "perf: Running benchmark test project in $test"

    & dotnet test -c Release
    if($LASTEXITCODE -ne 0) { exit 2 }

    Pop-Location

    Copy-Item $test\BenchmarkDotNet.Artifacts\results -Destination .\benchmark-results -Recurse -Force -Filter "*.md"
}


Pop-Location