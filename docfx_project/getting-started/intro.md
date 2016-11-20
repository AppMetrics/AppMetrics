## Getting started

### Configure a web application

Add the App Metrics AspNet extension nuget package to your project.json, as well as the json serilization formatter nuget package.

````
    "App.Metrics.Extensions.Middleware": "1.0.0-*

    "App.Metrics.Formatters.Json": "1.0.0-*"
````

----------

App Metrics is configured using the usual pattern to provide services and configuration to an ASP.NET Core project by adding required services to the `ConfigureServices` method in the `Startup.cs`

Modify your `Startup.cs` with the following:

[!code-csharp[Main](../src/samples/Startup.cs)]    

- `AddMetrics` registers App Metrics services with the `IServiceCollection`. This method also also provides options to change the default metrics configuration if you wish to do so.
- `AddHealthChecks` registers the Health Checking sytem, it will scan for classes within the project and any other project which has a reference to App Metrics and register any class which inherits `HealthCheck`. Alternatively, this method also provides access to the `IHealthCheckFactory` where you can register health checks inline.
- `AddMetricsMiddleware` registers middleware components providing a serveral metrics related endpoints, which are:
	1.  `/ping`: Used to determine if you can get a successful pont response, useful for load balancers
	2.  `/metrics`: Renders all metrics using the configured `IMetricsDataSerializer`, in our case in json format
	3.  `/metrics-text`: Renders a Humannized version of the metric data captured by the application
	4.  `/health`: Executes all health checks registered to determin the health status of the application

### Configure a console application

Add the App Metrics nuget package to your project.json, as well as the console reporter nuget package

````
    "App.Metrics": "1.0.0-*"    
	"App.Metrics.Extensions.Reporting.Console": "1.0.0-*"
````

----------

Modify your `Program.cs` with the following:

[!code-csharp[Main](../src/samples/MetricsProgram.cs)]    	 

- `AddMetrics` registers App Metrics services with the `IServiceCollection`. This method also also provides options to change the default metrics configuration if you wish to do so.
- `AddHealthChecks` registers the Health Checking sytem, it will scan for classes within the project and any other project which has a reference to App Metrics and register any class which inherits `HealthCheck`. Alternatively, this method also provides access to the `IHealthCheckFactory` where you can register health checks inline.
- `AddReporting` provides access to the `IReportFactory` allowing multiple metric report providers to be configured that will execute on each report run at the specified interval

----------

### Recording Metrics

App Metrics provides access to an `IMetrics` interface which is registered as a Single Instance. This can be injected where required to start mesuring different types of metrics.

Each metric being measured should be described through one of the following: `GaugeOptions`, `CounterOptions`, `MeterOptions`, `TimerOptions`, `HistogramOptions`. They provide the details about the metric being measured, the only required property is the `Name` property.

Below is an example of how organize an application's metrics

[!code-csharp[Main](../src/samples/AppMetricsRegistry.cs)]    	     

And to record the above metrics through `IMetrics`

[!code-csharp[Main](../src/samples/RecordMetrics.cs)]    	 

----------

### Next Steps

- Supported Metrics Types: [Gauges](metric-types/gauges.md), [Counters](metric-types/counters.md), [Meters](metric-types/meters.md), [Histograms](metric-types/histograms.md), [Timers](metric-types/timers.md)
- [Configuration Options](fundamentals/configuration.md)  
- [Nuget Packages](fundamentals/nuget-packages.md)
- [Sample Applications](../samples/index.md)