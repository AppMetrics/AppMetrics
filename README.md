# aspnet-metrics

dev branch build [![Build status](https://ci.appveyor.com/api/projects/status/mj9wj9m5hq5g0mh9/branch/dev?svg=true)](https://ci.appveyor.com/project/alhardy/aspnet-metrics/branch/dev)

## Overview

This library provides support for integrating ASP.NET Core with the [Metrics.NET Library](https://github.com/Recognos/Metrics.NET)

Currently there is a dependency on ASP.NET Core MVC to allow request metrics for both conventional based routing and attribute routing. This dependency in future will be removed so that by default there is only a dependency on ASP.NET Core Routing, and only when needed a seperate package could be referenced with an ASP.NET Core MVC dependency for use of attribute routing. 

Metrics.NET at the moment requires for full .NET CLR hence the library will not yet be able to run the .NET Core CLR.

## Getting started

In your ```Startup.cs``` add the libraries dependencies using the ```IServiceCollection``` extension method.

```
public void ConfigureServices(IServiceCollection services)
{
	...
	service.AddMetrics()
		   .AddAllPerformanceCounters()
           .AddHealthChecks()
	...
}
```

Then in your ```Startup.cs``` add the required ASP.NET Middleware

```
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
	...
	app.UseMvcWithMetrics(routes => {
		routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
	});
	...
}
```

###Health checks

Health checks are automatically registered, any class which inherits ```Metrics.Core.HealthCheck``` will be registered with the metrics context and results displayed to the /health endpoint. Health checks can by ignored by marking them with a ```System.Obsolete``` attribute.

Health checks can live in any assembly referenced by your hosting application and they will also be automatically registered, provding they have a dependency on either [Metrics.NET Library](https://github.com/Recognos/Metrics.NET) or AspNet.Metrics

*Example:*

```
public class SampleHealthCheck : Metrics.Core.HealthCheck
{
    public SampleHealthCheck() : base("Sample Healthy")
    {
    }

    protected override HealthCheckResult Check()
    {
        return HealthCheckResult.Healthy("OK"); 
		// or  return HealthCheckResult.Unhealthy("OOPS");
    }
}
```


And that's it you can now run your site or api and visit the following urls:

- /ping: Used to determine if you can get a successful pong response
- /json: Renders all registered metrics in json format
- /metrics-visual: Renders a UI showing metrics in realtime
- /metrics-text: Renders registered metrics in a human readable format 
- /health: Executes all health checks registered to determine the health of the application


> See the Mvc.Sample and Api.Sample in the repositories src directory.
> For more details on what the core of the library offers visit [Metrics.NET Library](https://github.com/Recognos/Metrics.NET)

## Customization

This library registers default settings which can be customize. These settings include:

- Enabling/disabling each of the endpoints mentioned above
- Changing path string of each of the endpoints mentioned above
- Adding additional regular expressions to ignore requires from being monitored 
- Creating a custom implementation of the ```AspNet.Metrics.Infrastructure.IRouteNameResolver```, which is used to determine the route template of the current request and is then used as the name of the metric

*Example:*

```
	public void ConfigureServices(IServiceCollection services)
	{
		...
		service
                .AddMetrics(options =>
                {
                    options.MetricsVisualisationEnabled = false;
                    options.MetricsEndpoint = new PathString("/metrics");                    
                })
			   .AddAllPerformanceCounters()
	           .AddHealthChecks()
		...
	}
```
