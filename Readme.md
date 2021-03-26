Cómo construir
Las compilaciones de Azure Devops se activan en las confirmaciones y los RP de la devsucursal

Instale el SDK de .NET Core 2.x más reciente
Ejecutar build.ps1o build.shen la raíz del repositorio
Cómo ejecutar evaluaciones comparativas
App.Metrics incluye evaluaciones comparativas mediante BenchmarkDotNet .

Existen dos proyectos de referencia dirigidos a App.Metrics.Core y App.Metrics.Concurrency

	cd .\src\Core\benchmarks\App.Metrics.Benchmarks.Runner
	dotnet run -c "Release" --framework netcoreapp3.1

	cd .\src\Concurrency\benchmarks\App.Metrics.Concurrency.Benchmarks.Runner
	dotnet run -c "Release" --framework netcoreapp3.1
Luego se le pedirá que elija un punto de referencia para ejecutar que generará un archivo de rebajas con el resultado en el directorio.

Puede encontrar los resultados del benchmark aquí y aquí .
