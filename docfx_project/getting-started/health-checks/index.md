# Health Checks

Health Checks give you the ability to monitor the health of your application by writing a small tests which returns either a healthy or un-healthy result. This is useful not only to test the internal health of your application but also it's external dependencies such as an third party api which your application relies on to function correctly.

Health checks are written by either inheriting `HealthCheck` or using the `IHealthCheckFactory` provided via the `IMetricsHostBuilder`. App Metrics automatically registers any class inheriting `HealthCheck` and will execute all checks asynchronously by either an `IMetricReporter` that is configured or when using [App.Metrics.Extensions.Middleware](../fundamentals/middleware-configuration.md) and the `/health` endpoint is requested. External monitoring tools can be configured to request the `/health` endpoint to continously test the health of your api and alert via desired means. Healthy results from this endpoint will return a 200 status code whereas if any health check fails the endpoint will return a 500 status code.

## Configuring Health Checks

Ensure that health checking is enabled in your `Startup.cs` in the case of an AspNet Application or in your `Program.cs` in the case of a Console Application.
	
[!code-csharp[Main](../../src/samples/Startup.cs?highlight=7)]    

## Implementing a Health Check

Healths checks can be implemented as a stand alone class:
 	
[!code-csharp[Main](../../src/samples/DatabaseHealthCheck.cs)]   

Or via the fluent building in your startup code:
       
[!code-csharp[Main](../../src/samples/StartupFluentHealth.cs?highlight=9,10)]

> [!NOTE]
> As well as scanning the executing assembly for health checks, App Metrics will also scan all referenced assemblies which have a dependency on App.Metrics and register any health checks it finds.