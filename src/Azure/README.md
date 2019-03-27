# App Metrics Health - Azure Extensions  <img src="https://avatars0.githubusercontent.com/u/29864085?v=4&s=200" alt="App Metrics" width="50px"/> 
[![Official Site](https://img.shields.io/badge/site-appmetrics-blue.svg?style=flat-square)](http://app-metrics.io/getting-started/intro.html) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg?style=flat-square)](https://opensource.org/licenses/Apache-2.0)

## What is App Metrics Health - Azure Extensions?

App Metrics is an open-source and cross-platform .NET library used to record metrics within an application. This repository includes [App Metrics Health Check](https://github.com/AppMetrics/Health) extension packages for check Azure resource health.

## Latest Builds & Packages

|Branch|AppVeyor|
|------|:--------:|
|dev|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/healthazure/dev.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/healthazure/branch/dev)|
|master|[![AppVeyor](https://img.shields.io/appveyor/ci/alhardy/healthazure/master.svg?style=flat-square&label=appveyor%20build)](https://ci.appveyor.com/project/alhardy/healthazure/branch/master)|

|Package|Dev Release|Pre-Release|Release|
|------|:--------:|:--------:|:--------:|
|App.Metrics.Health.Checks.AzureDocumentDB|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Health.Checks.AzureDocumentDB.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Health.Checks.AzureDocumentDB)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Health.Checks.AzureDocumentDB.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Checks.AzureDocumentDB/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Health.Checks.AzureDocumentDB.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Checks.AzureDocumentDB/)
|App.Metrics.Health.Checks.AzureStorage|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Health.Checks.AzureStorage.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Health.Checks.AzureStorage)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Health.Checks.AzureStorage.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Checks.AzureStorage/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Health.Checks.AzureStorage.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Checks.AzureStorage/)|
|App.Metrics.Health.Checks.AzureServiceBus|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Health.Checks.AzureServiceBus.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Health.Checks.AzureServiceBus)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Health.Checks.AzureServiceBus.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Checks.AzureServiceBus/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Health.Checks.AzureServiceBus.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Checks.AzureServiceBus/)|
|App.Metrics.Health.Checks.AzureEventHubs|[![MyGet Status](https://img.shields.io/myget/appmetrics/v/App.Metrics.Health.Checks.AzureEventHubs.svg?style=flat-square)](https://www.myget.org/feed/appmetrics/package/nuget/App.Metrics.Health.Checks.AzureEventHubs)|[![NuGet Status](https://img.shields.io/nuget/vpre/App.Metrics.Health.Checks.AzureEventHubs.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Checks.AzureEventHubs/)|[![NuGet Status](https://img.shields.io/nuget/v/App.Metrics.Health.Checks.AzureEventHubs.svg?style=flat-square)](https://www.nuget.org/packages/App.Metrics.Health.Checks.AzureEventHubs/)|

## How to build

[AppVeyor](https://ci.appveyor.com/project/alhardy/appmetrics/branch/master) builds are triggered on commits and PRs to `dev` and `master` branches.

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

## Contributing

See the [contribution guidlines](CONTRIBUTING.md) for details.

## License

This library is release under Apache 2.0 License ( see LICENSE ) Copyright (c) 2017 Allan Hardy

See [LICENSE](https://github.com/AppMetrics/HealthAzure/blob/master/LICENSE)
