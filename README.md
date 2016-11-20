# App Metrics

AppVeyor: [![Build status](https://ci.appveyor.com/api/projects/status/r4x0et4g6mr5vttf?svg=true)](https://ci.appveyor.com/project/alhardy/appmetrics)

## What is App Metrics?

App Metrics is an open-source and cross-platform .NET library used to record metrics within an application. App Metrics can run on .NET Core or on the full .NET framework. App Metrics abstracts away the underlaying repository of your Metrics for example InfluxDB, Graphite, Elasticsearch etc, by sampling and aggregating in memory and providing extensibility points to flush metrics to a repository at a specified interval.

App Metrics provides various metric types to measure things such as the rate of requests, counting the number of user logins over time, measure the time taken to execute a database query, measure the amount of free memory and so on. Metrics types supporter are Gauges, Counters, Meters, Histograms and Timers.

App Metrics also provides a health checking system allowing you to monitor the health of your application through user defined checks.

- [Getting Started](https://alhardy.github.io/app-metrics-docs/getting-started/intro.html)
- [Api Documentation](https://alhardy.github.io/app-metrics-docs/api/index.html)
- [Samples](https://github.com/alhardy/AppMetrics/tree/master/samples)

## Acknowledgements

**Built using the following open source projects**

* [ASP.NET Core](https://github.com/aspnet)
* [DocFX](https://dotnet.github.io/docfx/)
* [Json.Net](http://www.newtonsoft.com/json)
* [Fluent Assertions](http://www.fluentassertions.com/)
* [XUnit](https://xunit.github.io/)

----------

**Ported from [Metrics.NET](https://github.com/etishor/Metrics.NET)**

App Metrics is a significant redesign and .NET Standard port of the [Metrics.NET](https://github.com/etishor/Metrics.NET) library, which is a port of the Java [Metrics](https://github.com/dropwizard/metrics) library. This library for now includes the original [sampling code](https://github.com/etishor/Metrics.NET/tree/master/Src/Metrics/Sampling) written by Metrics.NET. Metrics.NET features that have been removed for now are the following:

1. Visualization, I believe most will be using something like Grafana to visualize their metrics
2. Remote Metrics
3. Performance counters as they are windows specific
4. The Graphite, InfluxDB and Elasticsearch reporters, which can be re-written as an App Metrics Report Provider

Why another .NET port? The main reason for porting Metrics.NET was to have it run on .NET Standard. Intially I refactored Metrics.NET stripping out features which required a fairly large refactor such as visualization, however the maintainers did not see .NET Standard or Core a priority at the time. 

This library will always keep the same license as the original [Metrics.NET Library](https://github.com/etishor/Metrics.NET) (as long as its an open source, permisive license). 

The original metrics project is released under these terms

"Metrics.NET is release under Apache 2.0 License 
Copyright (c) 2014 Iulian Margarintescu"

## License

This library is release under Apache 2.0 License ( see LICENSE ) Copyright (c) 2016 Allan Hardy
