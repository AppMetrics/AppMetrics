## Introduction 

App Metrics is a significant redesign and .NET Standard port of the [Metrics.NET](https://github.com/etishor/Metrics.NET) library, which is a port of the Java [Metrics](https://github.com/dropwizard/metrics) library. This library for now includes the [sampling code](https://github.com/etishor/Metrics.NET/tree/master/Src/Metrics/Sampling) written by Metrics.NET. Metrics.NET features that have been removed for now are the following:

1. Visualization, I believe most will be using something like Grafana to visualize their metrics
2. Remote Metrics
3. Performance counters as they are windows specific
4. The Graphite, InfluxDB and Elasticsearch reporters, which can be re-written as an App Metrics Report Provider

Why another .NET port? The main reason for porting Metrics.NET was to have it run on .NET Standard. Intially I refactored Metrics.NET stripping out features which required a fairly large refactor such as visualization, however the maintainers did not see .NET Standard or Core a priority at the time. 

## What is App Metrics?

App Metrics is an open-source and cross-platform .NET library used to record metrics within an application. App Metrics can run on .NET Core or on the full .NET framework. App Metrics abstracts away the underlaying repository of your Metrics for example InfluxDB, Graphite, Elasticsearch etc, by sampling and aggregating in memory and providing extensibility points to flush metrics to a repository at a specified interval. 

App Metrics provides various metric types to measure things such as the rate of requests, counting the number of user logins over time, measure the time taken to execute a database query, measure the amount of free memory and so on. Metrics types supporter are Gauges, Counters, Meters, Histograms and Timers.

App Metrics also provides a health checking system allowing you to monitor the health of your application through user defined checks.

## Why build App Metrics?

App Metrics was built to provide an easy way to capture the desired metrics within an application whilst having minimal impact on the performance of your application, and allowing these metrics to be reported to the desired respository through the library's reporting capabilities.

There are many open-source time series databases and payed monitoring solutions out there each with their own pros and cons, App Metrics provides your application the ability to capture metrics and then easily swap out the underlying metric repository or report to multiple repositories with little effort.

With App Metrics you can:

- Capture application metrics within any type of .NET application e.g. Windows Service, MVC Site, Web API etc
- Automatically measure the performance and error of each endpoint in an MVC or Web API project
- When securing an API with OAuth2, automatically measure the request rate and error rate per client
- Choose where to persist captured metrics and the dashboard you wish to use to visualize these metrics

