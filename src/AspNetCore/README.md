# App Metrics AspNetCore <img src="https://avatars0.githubusercontent.com/u/29864085?v=4&s=200" alt="App Metrics" width="50px"/> 
[![Official Site](https://img.shields.io/badge/site-appmetrics-blue.svg?style=flat-square)](http://app-metrics.io/getting-started/intro.html) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg?style=flat-square)](https://opensource.org/licenses/Apache-2.0)

## What is App Metrics AspNetCore?

App Metrics is an open-source and cross-platform .NET library used to record metrics within an application. This repository includes AspNetCore middleware and extensions to [App Metrics](https://github.com/AppMetrics/AppMetrics) which track typical metrics recorded in a web application and provide the ability to expose recorded metrics over HTTP. See the [Getting Started Guide](https://www.app-metrics.io/getting-started/) for more details and documentation on [App Metrics Web Monitoring](https://www.app-metrics.io/web-monitoring/aspnet-core/quick-start/).

## Latest Builds & Packages

|Branch|AppVeyor|Travis|Coverage|
|------|:--------:|:--------:|:--------:|
|dev|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/aspnetcore/dev.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/aspnetcore/branch/dev)|[![Travis](https://img.shields.io/travis/alhardy/aspnetcore/dev.svg?style=flat-square&label=travis%20build)](https://travis-ci.org/alhardy/aspnetcore)|[![Coveralls](https://img.shields.io/coveralls/AppMetrics/AspNetCore/dev.svg?style=flat-square)](https://coveralls.io/github/AppMetrics/AspNetCore?branch=dev)
|master|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/aspnetcore/master.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/aspnetcore/branch/master)| [![Travis](https://img.shields.io/travis/alhardy/aspnetcore/master.svg?style=flat-square&label=travis%20build)](https://travis-ci.org/alhardy/aspnetcore)| [![Coveralls](https://img.shields.io/coveralls/AppMetrics/AspNetCore/master.svg?style=flat-square)](https://coveralls.io/github/AppMetrics/AspNetCore?branch=master)|

|Package|Dev Release|Pre-Release|Release|
|------|:--------:|:--------:|:--------:|
|App.Metrics.AspNetCore|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.AspNetCore.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.AspNetCore)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.AspNetCore.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.AspNetCore..svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore/)
|App.Metrics.AspNetCore.Abstractions|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.AspNetCore.Abstractions.svg?style=flat-square0)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.AspNetCore.Abstractions)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.AspNetCore.Abstractions.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Abstractions/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.AspNetCore.Abstractions.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Abstractions/)
|App.Metrics.AspNetCore.Core|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.AspNetCore.Core.svg?style=flat-square0)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.AspNetCore.Core)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.AspNetCore.Core.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Core/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.AspNetCore.Core.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Core/)
|App.Metrics.AspNetCore.Hosting|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.AspNetCore.Hosting.svg?style=flat-square&maxAge=7200)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.AspNetCore.Hosting)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.AspNetCore.Hosting.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Hosting/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.AspNetCore.Hosting.svg)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Hosting/)
|App.Metrics.AspNetCore.Mvc|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.AspNetCore.Mvc.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.AspNetCore.Mvc)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.AspNetCore.Mvc.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.AspNetCore.Mvc.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Mvc/)
|App.Metrics.AspNetCore.Mvc.Core|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.AspNetCore.Mvc.Core.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.AspNetCore.Mvc.Core)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.AspNetCore.Mvc.Core.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Core/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.AspNetCore.Mvc.Core.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Mvc.Core/)
|App.Metrics.AspNetCore.Tracking|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.AspNetCore.Tracking.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.AspNetCore.Tracking)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.AspNetCore.Tracking.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Tracking/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.AspNetCore.Tracking.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Tracking/)|
|App.Metrics.AspNetCore.Endpoints|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.AspNetCore.Endpoints.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.AspNetCore.Endpoints)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.AspNetCore.Endpoints.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Endpoints/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.AspNetCore.Endpoints.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Endpoints/)|
|App.Metrics.AspNetCore.Reporting|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.AspNetCore.Reporting.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.AspNetCore.Reporting)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.AspNetCore.Reporting.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Reporting/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.AspNetCore.Reporting.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AspNetCore.Reporting/)|
----------

## Visualization

#### Grafana Web Monitoring

![Grafana/InfluxDB Generic Web Dashboard Demo](https://github.com/alhardy/AppMetrics.DocFx/blob/master/images/generic_grafana_dashboard_demo.gif)

> Dashboards for each reporter are available on [Grafana Dashbaords](https://grafana.com/dashboards?search=app%20metrics).

#### Grafana OAuth2 Client Web Monitoring

![Grafana/InfluxDB Generic OAuth2 Web Dashboard Demo](https://github.com/alhardy/AppMetrics.DocFx/blob/master/images/generic_grafana_oauth2_dashboard_demo.gif)

> Dashboards for each reporter are available on [Grafana Dashbaords](https://grafana.com/dashboards?search=app%20metrics).

## How to build

[AppVeyor](https://ci.appveyor.com/project/alhardy/aspnetcore/branch/master) and [Travis CI](https://travis-ci.org/alhardy/AspNetCore) builds are triggered on commits and PRs to `dev` and `master` branches.

See the following for build arguments and running locally.

|Configuration|Description|Default|Environment|Required|
|------|--------|:--------:|:--------:|:--------:|
|BuildConfiguration|The configuration to run the build, **Debug** or **Release** |*Release*|All|Optional|
|PreReleaseSuffix|The pre-release suffix for versioning nuget package artifacts e.g. `beta`|*ci*|All|Optional|
|CoverWith|**DotCover** or **OpenCover** to calculate and report code coverage, **None** to skip. When not **None**, a coverage file and html report will be generated at `./artifacts/coverage`|*OpenCover*|Windows Only|Optional|
|SkipCodeInspect|**false** to run ReSharper code inspect and report results, **true** to skip. When **true**, the code inspection html report and xml output will be generated at `./artifacts/resharper-reports`|*false*|Windows Only|Optional|
|BuildNumber|The build number to use for pre-release versions|*0*|All|Optional|
|LinkSources|[Source link](https://github.com/ctaggart/SourceLink) support allows source code to be downloaded on demand while debugging|*true*|All|Optional|


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
[![Powered By NDepend](https://github.com/alhardy/AppMetrics.DocFx/blob/master/images/PoweredByNDepend.png)](http://www.ndepend.com/)
----------
