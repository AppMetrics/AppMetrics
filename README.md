# App Metrics

[![Official Site](https://img.shields.io/badge/site-appmetrics-blue.svg)](https://alhardy.github.io/app-metrics-docs/getting-started/intro.html) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0) [![Coverage Status](https://coveralls.io/repos/github/alhardy/AppMetrics/badge.svg?branch=master&cacheBuster=1)](https://coveralls.io/github/alhardy/AppMetrics?branch=master)

|AppVeyor|Travis|
|:--------:|:--------:|
|[![Build status](https://ci.appveyor.com/api/projects/status/r4x0et4g6mr5vttf?svg=true)](https://ci.appveyor.com/project/alhardy/appmetrics/branch/master)|[![Build status](https://travis-ci.org/alhardy/AppMetrics.svg?branch=1.0.0)](https://travis-ci.org/alhardy/AppMetrics)|

|Package|Dev Release|Pre Release|Latest Release|
|------|:--------:|:--------:|:--------:|
|App.Metrics|[![MyGet Status](https://img.shields.io/myget/alhardy/v/App.Metrics.svg)](https://www.myget.org/feed/alhardy/package/nuget/App.Metrics)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.svg)](https://www.nuget.org/packages/App.Metrics/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.svg)](https://www.nuget.org/packages/App.Metrics/)
|App.Metrics.Extensions.Mvc|[![MyGet Status](https://img.shields.io/myget/alhardy/v/App.Metrics.Extensions.Mvc.svg)](https://www.myget.org/feed/alhardy/package/nuget/App.Metrics.Extensions.Mvc)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Extensions.Mvc.svg)](https://www.nuget.org/packages/App.Metrics.Extensions.Mvc/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Extensions.Mvc.svg)](https://www.nuget.org/packages/App.Metrics.Extensions.Mvc/)
|App.Metrics.Extensions.Middleware|[![MyGet Status](https://img.shields.io/myget/alhardy/v/App.Metrics.Extensions.Middleware.svg)](https://www.myget.org/feed/alhardy/package/nuget/App.Metrics.Extensions.Middleware)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Extensions.Middleware.svg)](https://www.nuget.org/packages/App.Metrics.Extensions.Middleware/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Extensions.Middleware.svg)](https://www.nuget.org/packages/App.Metrics.Extensions.Middleware/)
|App.Metrics.Formatters.Json|[![MyGet Status](https://img.shields.io/myget/alhardy/v/App.Metrics.Formatters.Json.svg)](https://www.myget.org/feed/alhardy/package/nuget/App.Metrics.Formatters.Json)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Formatters.Json.svg)](https://www.nuget.org/packages/App.Metrics.Formatters.Json/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Formatters.Json.svg)](https://www.nuget.org/packages/App.Metrics.Formatters.Json/)|

## What is App Metrics?

App Metrics is an open-source and cross-platform .NET library used to record metrics within an application. App Metrics can run on .NET Core or on the full .NET framework also supporting .NET 4.5.2. App Metrics abstracts away the underlaying repository of your Metrics for example InfluxDB, Graphite, Elasticsearch etc, by sampling and aggregating in memory and providing extensibility points to flush metrics to a repository at a specified interval. For pre .NET Core web applications see [AppMetrics.Owin](https://github.com/alhardy/AppMetrics.Owin)

App Metrics provides various metric types to measure things such as the rate of requests, counting the number of user logins over time, measure the time taken to execute a database query, measure the amount of free memory and so on. Metrics types supported are Gauges, Counters, Meters, Histograms and Timers and Application Performance Indexes [Apdex](http://apdex.org/overview.html).

For metric reporting capabilities see the [reporting repo](https://github.com/alhardy/AppMetrics.Reporters).

`App.Metrics` includes an Exponentially Forward Decaying, Sliding Window and Algorithm R reservoir implementations, for additional reservoir sampling see the [reservoir repo](https://github.com/alhardy/AppMetrics.Reservoirs). For more details on reservoir sampling see the [docs](https://alhardy.github.io/app-metrics-docs/getting-started/sampling/index.html).

App Metrics also provides a health checking system allowing you to monitor the health of your application through user defined checks.

- [Getting Started](https://alhardy.github.io/app-metrics-docs/getting-started/intro.html)
- [Sample Applications & Grafana Dashbaords](https://github.com/alhardy/AppMetrics.Samples)
- [Api Documentation](https://alhardy.github.io/app-metrics-docs/api/index.html)

## How to build

[AppVeyor](https://ci.appveyor.com/project/alhardy/appmetrics/branch/master) and [Travis CI](https://travis-ci.org/alhardy/AppMetrics) builds are triggered on commits and PRs to `dev` and `master` branches.

See the following for build arguments and running locally.

|Configuration|Description|Default|Environment|Required|
|------|--------|:--------:|:--------:|:--------:|
|Configuration|The configuration to run the build, **Debug** or **Release** |*Release*|All|Optional|
|PreReleaseSuffix|The pre-release suffix for versioning nuget package artifacts e.g. `beta`|*ci*|All|Optional|
|SkipOpenCover|**false** to calculate and report code coverage, **true** to skip. When **true**, an open cover code coverage file and html report will be generated at `./artifacts/coverage`|*true*|Windows Only|Optional|
|BuildNumber|The build number to use for pre-release versions|*0*|All|Optional|


### Windows

Run `build.ps1` from the repositories root directory.

```
	.\build.ps1'
```

**With Arguments**

```
	.\build.ps1 --ScriptArgs '-Configuration=Release -PreReleaseSuffix=beta -SkipOpenCover=false -BuildNumber=1'
```

### Linux & OSX

Run `build.sh` from the repositories root directory. Code Coverage reports are now supported on Linux and OSX, it will be skipped running in these environments.

```
	.\build.sh'
```

**With Arguments**

```
	.\build.sh --ScriptArgs '-Configuration=Release -PreReleaseSuffix=beta -BuildNumber=1'
```

## Contributing

See the [contribution guidlines](CONTRIBUTING.md) for details.

## Acknowledgements

* [ASP.NET Core](https://github.com/aspnet)
* [DocFX](https://dotnet.github.io/docfx/)
* [Fluent Assertions](http://www.fluentassertions.com/)
* [XUnit](https://xunit.github.io/)
* [StyleCopAnalyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)
* [Cake](https://github.com/cake-build/cake)
* [OpenCover](https://github.com/OpenCover/opencover)

***Thanks for providing free open source licensing***

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
