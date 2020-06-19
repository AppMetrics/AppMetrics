# App Metrics <img src="https://www.app-metrics.io/images/logo.png" alt="App Metrics" width="50px"/> 
[![Official Site](https://img.shields.io/badge/site-appmetrics-blue.svg?style=flat-square)](https://www.app-metrics.io/getting-started/) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg?style=flat-square)](https://opensource.org/licenses/Apache-2.0)
[![Donate](https://img.shields.io/badge/donorbox-donate-blue.svg)](https://donorbox.org/help-support-appmetrics?recurring=true) 

## What is App Metrics?

App Metrics is an open-source and cross-platform .NET library used to record metrics within an application. App Metrics can run on .NET Core or on the full .NET framework. App Metrics abstracts away the underlaying repository of your Metrics for example InfluxDB, Graphite, Prometheus etc, by sampling and aggregating in memory and providing extensibility points to flush metrics to a repository at a specified interval.

App Metrics provides various metric types to measure things such as the rate of requests, counting the number of user logins over time, measure the time taken to execute a database query, measure the amount of free memory and so on. Metrics types supported are Gauges, Counters, Meters, Histograms and Timers and Application Performance Indexes [Apdex](https://www.apdex.org/overview).

`App.Metrics` includes an Exponentially Forward Decaying, Sliding Window and Algorithm R reservoir implementations. For more details on reservoir sampling see the [docs](https://www.app-metrics.io/getting-started/reservoir-sampling/).

### Documentation

- [Getting Started](https://www.app-metrics.io/getting-started/)
- [ASP.NET Core 2.0](https://www.app-metrics.io/web-monitoring/aspnet-core/)
- [Reporting](https://www.app-metrics.io/reporting/reporters/)
- [Sample Applications & Grafana Dashboards](https://www.app-metrics.io/samples/)

## Latest Builds, Packages & Repo Stats

|Branch|Azure Devops|
|------|:--------:|
|dev|[![Azure Devops](https://img.shields.io/azure-devops/build/AppMetrics/AppMetrics/3/dev.svg?style=flat-square&label=build)](https://dev.azure.com/appmetrics/AppMetrics/_build?definitionId=3)
|main|[![AppVeyor](https://img.shields.io/azure-devops/build/AppMetrics/AppMetrics/3/main.svg?style=flat-square&label=build)](https://dev.azure.com/appmetrics/AppMetrics/_build?definitionId=3)
----------

## Visualization

Dashboards can be imported from [Grafana](https://grafana.com/dashboards?search=app%20metrics)

#### Grafana Web Monitoring

![Grafana/InfluxDB Generic Web Dashboard Demo](https://raw.githubusercontent.com/AppMetrics/Docs.V2.Hugo/main/static/images/generic_grafana_dashboard_demo.gif)


#### Grafana OAuth2 Client Web Monitoring

![Grafana/InfluxDB Generic OAuth2 Web Dashboard Demo](https://raw.githubusercontent.com/AppMetrics/Docs.V2.Hugo/main/static/images/generic_grafana_oauth2_dashboard_demo.gif)


## How to build

[Azure Devops](https://dev.azure.com/appmetrics/AppMetrics/_build?definitionId=3) builds are triggered on commits and PRs to the `dev` branch

- Install the latest [.NET Core 2.x SDK](https://dotnet.microsoft.com/download#/current)
- Run `build.ps1` or `build.sh` in the root of the repository

## How to run benchmarks

App.Metrics includes benchmarking using [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).

Two benchmark projects exist targeting App.Metrics.Core and App.Metrics.Concurrency

```
	cd .\src\Core\benchmarks\App.Metrics.Benchmarks.Runner
	dotnet run -c "Release" --framework netcoreapp3.1

	cd .\src\Concurrency\benchmarks\App.Metrics.Concurrency.Benchmarks.Runner
	dotnet run -c "Release" --framework netcoreapp3.1
```

You'll then be prompted to choose a benchmark to run which will output a markdown file with the result in directory.

You can find the benchmark results [here](https://github.com/alhardy/AppMetrics/tree/dev/src/Core/benchmarks/App.Metrics.Benchmarks.Runner/BenchmarkDotNet.Artifacts/results) and [here](https://github.com/alhardy/AppMetrics/tree/dev/src/Concurrency/benchmarks/App.Metrics.Concurrency.Benchmarks.Runner/BenchmarkDotNet.Artifacts/results).

## Contributing

See the [contribution guidlines](CONTRIBUTING.md) for details.

## Acknowledgements

* [ASP.NET Core](https://github.com/aspnet)
* [Grafana](https://grafana.com/)
* [Fluent Assertions](http://www.fluentassertions.com/)
* [xUnit.net](https://xunit.github.io/)
* [Hugo](https://gohugo.io/)
* [Netlify](https://www.netlify.com/)

***Thanks for providing free open source licensing***

* [NDepend](http://www.ndepend.com/) 
* [Jetbrains](https://www.jetbrains.com/dotnet/) 

## License

This library is release under Apache 2.0 License ( see LICENSE ) Copyright (c) 2016 Allan Hardy

See [LICENSE](https://github.com/AppMetrics/AppMetrics/blob/dev/LICENSE)

----------

App Metrics is based on the [Metrics.NET](https://github.com/etishor/Metrics.NET) library, and at the moment uses the same reservoir sampling code from the original library which is a port of the Java [Dropwizard Metrics](https://github.com/dropwizard/metrics) library. 

*Metrics.NET Licensed under these terms*:
"Metrics.NET is release under Apache 2.0 License Copyright (c) 2014 Iulian Margarintescu" see [LICENSE](https://github.com/etishor/Metrics.NET/blob/main/LICENSE)

*Dropwizard Metrics* Licensed under these terms*:
"Copyright (c) 2010-2013 Coda Hale, Yammer.com Published under Apache Software License 2.0, see [LICENSE](https://github.com/dropwizard/metrics/blob/3.2-development/LICENSE)"