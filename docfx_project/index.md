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

### Next Steps

- [Getting Started](getting-started/intro.md)
- [Nuget Packages](getting-started/fundamentals/nuget-packages.md)
- [Sample Applications](samples/index.md)

