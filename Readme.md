Métricas de la aplicación Métricas de la aplicación
Sitio oficial Licencia Donar

¿Qué son las métricas de la aplicación?
App Metrics es una biblioteca .NET de código abierto y multiplataforma que se utiliza para registrar métricas dentro de una aplicación. Las métricas de la aplicación se pueden ejecutar en .NET Core o en el marco .NET completo. App Metrics abstrae el repositorio subyacente de sus métricas, por ejemplo, InfluxDB, Graphite, Prometheus, etc., muestreando y agregando en la memoria y proporcionando puntos de extensibilidad para vaciar las métricas a un repositorio en un intervalo específico.

App Metrics proporciona varios tipos de métricas para medir cosas como la tasa de solicitudes, contar el número de inicios de sesión de usuarios a lo largo del tiempo, medir el tiempo necesario para ejecutar una consulta de base de datos, medir la cantidad de memoria libre, etc. Los tipos de métricas admitidos son medidores, contadores, medidores, histogramas y temporizadores e índices de rendimiento de aplicaciones Apdex .

App.Metricsincluye implementaciones de yacimiento con descomposición hacia adelante exponencial, ventana deslizante y algoritmo R. Para obtener más detalles sobre el muestreo de yacimientos, consulte los documentos .

Documentación
Empezando
ASP.NET Core 2.0
Reportando
Aplicaciones de muestra y paneles de Grafana
Últimas compilaciones, paquetes y estadísticas de repositorios
Rama	Azure Devops
dev	Azure Devops
principal	AppVeyor
Visualización
Los paneles se pueden importar desde Grafana

Monitoreo web de Grafana
Demostración del panel web genérico de Grafana / InfluxDB

Monitoreo web del cliente Grafana OAuth2
Demostración del panel web OAuth2 genérico de Grafana / InfluxDB

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

Contribuyendo
Consulte las pautas de contribución para obtener más detalles.

Agradecimientos
ASP.NET Core
Grafana
Afirmaciones fluidas
xUnit.net
Hugo
Netlify
Gracias por proporcionar licencias de código abierto gratuitas

NDepender
Jetbrains
Licencia
Esta biblioteca se publica bajo la licencia Apache 2.0 (ver LICENCIA) Copyright (c) 2016 Allan Hardy

Ver LICENCIA

App Metrics se basa en la biblioteca Metrics.NET y, en este momento, utiliza el mismo código de muestreo de yacimientos de la biblioteca original, que es un puerto de la biblioteca Java Dropwizard Metrics .

Metrics.NET Licencia bajo estos términos : "Metrics.NET se publica bajo la licencia Apache 2.0 Copyright (c) 2014 Iulian Margarintescu" ver LICENCIA

Dropwizard Metrics Licencia bajo estos términos *: "Copyright (c) 2010-2013 Coda Hale, Yammer.com Publicado bajo Apache Software License 2.0, consulte LICENCIA "
