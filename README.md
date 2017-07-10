# App Metrics <img src="http://app-metrics.io/logo.png" alt="App Metrics" width="50px"/> 
[![Official Site](https://img.shields.io/badge/site-appmetrics-blue.svg?style=flat-square)](http://app-metrics.io/getting-started/intro.html) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg?style=flat-square)](https://opensource.org/licenses/Apache-2.0) [![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/app-metrics/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## What is App Metrics?

App Metrics is an open-source and cross-platform .NET library used to record metrics within an application. App Metrics can run on .NET Core or on the full .NET framework also supporting .NET 4.5.2. App Metrics abstracts away the underlaying repository of your Metrics for example InfluxDB, Graphite, Elasticsearch etc, by sampling and aggregating in memory and providing extensibility points to flush metrics to a repository at a specified interval.

App Metrics provides various metric types to measure things such as the rate of requests, counting the number of user logins over time, measure the time taken to execute a database query, measure the amount of free memory and so on. Metrics types supported are Gauges, Counters, Meters, Histograms and Timers and Application Performance Indexes [Apdex](http://apdex.org/overview.html).

`App.Metrics` includes an Exponentially Forward Decaying, Sliding Window and Algorithm R reservoir implementations. For more details on reservoir sampling see the [docs](http://app-metrics.io/getting-started/sampling/index.html).

### Reporting Features

- [Console & Text File Reporters](https://github.com/alhardy/AppMetrics.Reporters)
- [InfluxDB Extensions](https://github.com/alhardy/AppMetrics.Extensions.InfluxDB)
- [Elasticsearch Extensions](https://github.com/alhardy/AppMetrics.Extensions.Elasticsearch)
- [Prometheus Extensions](https://github.com/alhardy/AppMetrics.Extensions.Prometheus)
- [Graphite Extensions](https://github.com/alhardy/AppMetrics.Extensions.Graphite)

### Application Health

App Metrics also provides a [health checking](https://github.com/AppMetrics/Health) system allowing you to monitor the health of your application through user defined checks.

- [Getting Started](http://app-metrics.io/getting-started/intro.html)
- [Sample Applications & Grafana Dashbaords](https://github.com/alhardy/AppMetrics.Samples)
- [Api Documentation](http://app-metrics.io/api/index.html)

## Latest Builds, Packages & Repo Stats

|Branch|AppVeyor|Travis|Coverage|
|------|:--------:|:--------:|:--------:|
|dev|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/AppMetrics/dev.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/appmetrics/branch/dev)|[![Travis](https://img.shields.io/travis/alhardy/AppMetrics/dev.svg?style=flat-square&label=travis%20build)](https://travis-ci.org/alhardy/AppMetrics)|[![Coveralls](https://img.shields.io/coveralls/alhardy/AppMetrics/dev.svg?style=flat-square)](https://coveralls.io/github/alhardy/AppMetrics?branch=dev)
|master|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/AppMetrics/master.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/appmetrics/branch/master)| [![Travis](https://img.shields.io/travis/alhardy/AppMetrics/master.svg?style=flat-square&label=travis%20build)](https://travis-ci.org/alhardy/AppMetrics)| [![Coveralls](https://img.shields.io/coveralls/alhardy/AppMetrics/master.svg?style=flat-square)](https://coveralls.io/github/alhardy/AppMetrics?branch=master)|

|Package|Dev Release|Pre-Release|Release|
|------|:--------:|:--------:|:--------:|
|App.Metrics|[![MyGet Status](https://img.shields.io/myget/AppMetrics/v/App.Metrics.svg?style=flat-square)](https://www.myget.org/feed/AppMetrics/package/nuget/App.Metrics)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics/)
|App.Metrics.Extensions.Mvc|[![MyGet Status](https://img.shields.io/myget/AppMetrics/v/App.Metrics.Extensions.Mvc.svg?style=flat-square0)](https://www.myget.org/feed/AppMetrics/package/nuget/App.Metrics.Extensions.Mvc)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Extensions.Mvc.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Extensions.Mvc/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Extensions.Mvc.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Extensions.Mvc/)
|App.Metrics.Extensions.Middleware|[![MyGet Status](https://img.shields.io/myget/AppMetrics/v/App.Metrics.Extensions.Middleware.svg?style=flat-square&maxAge=7200)](https://www.myget.org/feed/AppMetrics/package/nuget/App.Metrics.Extensions.Middleware)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Extensions.Middleware.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Extensions.Middleware/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Extensions.Middleware.svg)](https://www.nuget.org/packages/App.Metrics.Extensions.Middleware/)
|App.Metrics.Formatters.Json|[![MyGet Status](https://img.shields.io/myget/AppMetrics/v/App.Metrics.Formatters.Json.svg?style=flat-square)](https://www.myget.org/feed/AppMetrics/package/nuget/App.Metrics.Formatters.Json)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Formatters.Json.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Formatters.Json/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Formatters.Json.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Formatters.Json/)|

[![GitHub issues](https://img.shields.io/github/issues/alhardy/AppMetrics.svg?style=flat-square&maxAge=7200)](https://github.com/alhardy/AppMetrics/issues?q=is%3Aopen+is%3Aissue) [![GitHub closed issues](https://img.shields.io/github/issues-closed/alhardy/AppMetrics.svg?style=flat-square&maxAge=7200)](https://github.com/alhardy/AppMetrics/issues?q=is%3Aissue+is%3Aclosed) [![GitHub closed pull requests](https://img.shields.io/github/issues-pr-closed/alhardy/AppMetrics.svg?style=flat-square&maxAge=7200)](https://github.com/alhardy/AppMetrics/pulls?q=is%3Apr+is%3Aclosed) [![Issue Stats](https://img.shields.io/issuestats/p/long/github/alhardy/AppMetrics.svg?style=flat-square&maxAge=7200)](http://www.issuestats.com/github/alhardy/AppMetrics) [![Issue Stats](https://img.shields.io/issuestats/i/github/alhardy/AppMetrics.svg?style=flat-square&maxAge=7200)](http://www.issuestats.com/github/alhardy/AppMetrics)
----------

## Visualization

#### Grafana Web Monitoring

![Grafana/InfluxDB Generic Web Dashboard Demo](https://github.com/alhardy/AppMetrics.DocFx/blob/master/images/generic_grafana_dashboard_demo.gif)

> Grab the [InfluxDB](https://grafana.com/dashboards/2125) or [Elasticsearch](https://grafana.com/dashboards/2140) dashboard.

#### Grafana OAuth2 Client Web Monitoring

![Grafana/InfluxDB Generic OAuth2 Web Dashboard Demo](https://github.com/alhardy/AppMetrics.DocFx/blob/master/images/generic_grafana_oauth2_dashboard_demo.gif)

> Grab the [InfluxDB](https://grafana.com/dashboards/2137) or [Elasticsearch](https://grafana.com/dashboards/2143) dashboard

## How to build

[AppVeyor](https://ci.appveyor.com/project/alhardy/appmetrics/branch/master) and [Travis CI](https://travis-ci.org/alhardy/AppMetrics) builds are triggered on commits and PRs to `dev` and `master` branches.

See the following for build arguments and running locally.

|Configuration|Description|Default|Environment|Required|
|------|--------|:--------:|:--------:|:--------:|
|BuildConfiguration|The configuration to run the build, **Debug** or **Release** |*Release*|All|Optional|
|PreReleaseSuffix|The pre-release suffix for versioning nuget package artifacts e.g. `beta`|*ci*|All|Optional|
|CoverWith|**DotCover** or **OpenCover** to calculate and report code coverage, **None** to skip. When not **None**, a coverage file and html report will be generated at `./artifacts/coverage`|*OpenCover*|Windows Only|Optional|
|SkipCodeInspect|**false** to run ReSharper code inspect and report results, **true** to skip. When **true**, the code inspection html report and xml output will be generated at `./artifacts/resharper-reports`|*false*|Windows Only|Optional|
|BuildNumber|The build number to use for pre-release versions|*0*|All|Optional|


### Windows

Run `build.ps1` from the repositories root directory.

```
	.\build.ps1
```

**With Arguments**

```
	.\build.ps1 --ScriptArgs '-BuildConfiguration=Release -PreReleaseSuffix=beta -CoverWith=OpenCover -SkipCodeInspect=false -BuildNumber=1'
```

### Linux & OSX

Run `build.sh` from the repositories root directory. Code Coverage reports are now supported on Linux and OSX, it will be skipped running in these environments.

```
	.\build.sh
```

**With Arguments**


```
	.\build.sh --ScriptArgs '-BuildConfiguration=Release -PreReleaseSuffix=beta -BuildNumber=1'
```

> #### Nuget Packages
> Nuget packages won't be generated on non-windows environments by default.
> 
> Unfortunately there is [currently no way out-of-the-box to conditionally build & pack a project by framework](https://github.com/dotnet/roslyn-project-system/issues/1586#issuecomment-280978851). Because `App.Metrics` packages target `.NET 4.5.2` as well as `dotnet standard` there is a work around in the build script to force `dotnet standard` on build but no work around for packaging on non-windows environments. 

## How to run benchmarks

App.Metrics includes benchmarking using [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet). You can find the benchmark results [here](https://github.com/alhardy/AppMetrics/tree/master/benchmarks/App.Metrics.Benchmarks.Runner/BenchmarkDotNet.Artifacts/results).

To run, fron the solution's root:

```
	cd .\benchmarks\App.Metrics.Benchmarks.Runner\
	dotnet run -c "Release"
```

You'll then be prompted to choose a benchmark to run which will output a markdown file with the result in directory `.\benchmarks\App.Metrics.Benchmarks.Runner\BenchmarkDotNet.Artifacts\results`.

Alternatively, you can run the same benchmarks from visual studio using xUnit.net in the [benchmark project](https://github.com/alhardy/AppMetrics/tree/master/benchmarks/App.Metrics.Benchmarks).

## Contributing

See the [contribution guidlines](CONTRIBUTING.md) for details.

## Acknowledgements

* [ASP.NET Core](https://github.com/aspnet)
* [Grafana](https://grafana.com/)
* [DocFX](https://dotnet.github.io/docfx/)
* [Fluent Assertions](http://www.fluentassertions.com/)
* [xUnit.net](https://xunit.github.io/)
* [StyleCopAnalyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)
* [Cake](https://github.com/cake-build/cake)
* [OpenCover](https://github.com/OpenCover/opencover)

***Thanks for providing free open source licensing***

* [NDepend](http://www.ndepend.com/) 
* [Jetbrains](https://www.jetbrains.com/dotnet/) 
* [AppVeyor](https://www.appveyor.com/)
* [Travis CI](https://travis-ci.org/)
* [Coveralls](https://coveralls.io/)

## License

This library is release under Apache 2.0 License ( see LICENSE ) Copyright (c) 2016 Allan Hardy

See [LICENSE](https://github.com/alhardy/AppMetrics/blob/dev/LICENSE)

----------

App Metrics is based on the [Metrics.NET](https://github.com/etishor/Metrics.NET) library, and at the moment uses the same reservoir sampling code from the original library which is a port of the Java [Dropwizard Metrics](https://github.com/dropwizard/metrics) library. 

*Metrics.NET Licensed under these terms*:
"Metrics.NET is release under Apache 2.0 License Copyright (c) 2014 Iulian Margarintescu" see [LICENSE](https://github.com/etishor/Metrics.NET/blob/master/LICENSE)

*Dropwizard Metrics* Licensed under these terms*:
"Copyright (c) 2010-2013 Coda Hale, Yammer.com Published under Apache Software License 2.0, see [LICENSE](https://github.com/dropwizard/metrics/blob/3.2-development/LICENSE)"

----------
[![Powered By NDepend](https://github.com/alhardy/AppMetrics.DocFx/blob/master/images/PoweredByNDepend.png)](http://www.ndepend.com/)

----------

[![](https://codescene.io/projects/792/status.svg) Get more details at **codescene.io**.](https://codescene.io/projects/792/jobs/latest-successful/results)
