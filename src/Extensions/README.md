# App Metrics Microsoft Extensions <img src="https://avatars0.githubusercontent.com/u/29864085?v=4&s=200" alt="App Metrics" width="50px"/> 
[![Official Site](https://img.shields.io/badge/site-appmetrics-blue.svg?style=flat-square)](http://app-metrics.io/getting-started/intro.html) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg?style=flat-square)](https://opensource.org/licenses/Apache-2.0)

## What is App Metrics Microsoft Extensions?

App Metrics is an open-source and cross-platform .NET library used to record metrics within an application. This repository includes Microsoft.Extensions packages for App Metrics.

## Latest Builds & Packages

|Branch|AppVeyor|Travis|Coverage|
|------|:--------:|:--------:|:--------:|
|dev|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/microsoftextensions/dev.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/appmetrics/branch/dev)|[![Travis](https://img.shields.io/travis/AppMetrics/MicrosoftExtensions/dev.svg?style=flat-square&label=travis%20build)](https://travis-ci.org/alhardy/AppMetrics)|[![Coveralls](https://img.shields.io/coveralls/AppMetrics/MicrosoftExtensions/dev.svg?style=flat-square)](https://coveralls.io/github/AppMetrics/MicrosoftExtensions?branch=dev)
|master|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/microsoftextensions/master.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/appmetrics/branch/master)| [![Travis](https://img.shields.io/travis/AppMetrics/MicrosoftExtensions/master.svg?style=flat-square&label=travis%20build)](https://travis-ci.org/alhardy/AppMetrics)| [![Coveralls](https://img.shields.io/coveralls/AppMetrics/MicrosoftExtensions/master.svg?style=flat-square)](https://coveralls.io/github/AppMetrics/MicrosoftExtensions?branch=master)|

|Package|Dev Release|Pre-Release|Release|
|------|:--------:|:--------:|:--------:|
|App.Metrics.AppNetCore|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.AppNetCore.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.AppNetCore)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.AppNetCore.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AppNetCore/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.AppNetCore.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.AppNetCore/)
|App.Metrics.Extensions.Configuration|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Extensions.Configuration.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Extensions.Configuration)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Extensions.Configuration.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Extensions.Configuration/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Extensions.Configuration.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Extensions.Configuration/)
|App.Metrics.Extensions.DependencyInjection|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Extensions.DependencyInjection.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Extensions.DependencyInjection)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Extensions.DependencyInjection.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Extensions.DependencyInjection/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Extensions.DependencyInjection.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Extensions.DependencyInjection/)
|App.Metrics.Extensions.Hosting|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Extensions.Hosting.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Extensions.Hosting)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Extensions.Hosting.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Extensions.Hosting/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Extensions.Hosting.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Extensions.Hosting/)
|App.Metrics.Health.AppNetCore|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Health.AppNetCore.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Health.AppNetCore)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Health.AppNetCore.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.AppNetCore/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Health.AppNetCore.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.AppNetCore/)
|App.Metrics.Health.Extensions.Configuration|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Health.Extensions.Configuration.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Health.Extensions.Configuration)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Health.Extensions.Configuration.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Extensions.Configuration/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Health.Extensions.Configuration.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Extensions.Configuration/)
|App.Metrics.Health.Extensions.DependencyInjection|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Health.Extensions.DependencyInjection.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Health.Extensions.DependencyInjection)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Health.Extensions.DependencyInjection.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Extensions.DependencyInjection/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Health.Extensions.DependencyInjection.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Extensions.DependencyInjection/)|
|App.Metrics.Health.Extensions.Hosting|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Health.Extensions.Hosting.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Extensions.Hosting)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Health.Extensions.Hosting.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Extensions.Hosting/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Health.Extensions.Hosting.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Extensions.Hosting/)|
|App.Metrics.Extensions.HealthChecks|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Extensions.HealthChecks.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Extensions.HealthChecks)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Extensions.HealthChecks.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Extensions.HealthChecks/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Extensions.HealthChecks.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Extensions.HealthChecks/)

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

This library is release under Apache 2.0 License ( see LICENSE ) Copyright (c) 2017 Allan Hardy

See [LICENSE](https://github.com/AppMetrics/MicrosoftExtensions/blob/master/LICENSE)
