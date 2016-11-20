# Configuration Options

App Metrics provides access to configuration options through the `AddMetrics` extension on the `IServiceCollection`. 

These configuration options include:

- **DefaultContextLabel**: Metrics recorded through the `IMetrics` interface are grouped into "Contexts", for example a database context or application context. Metrics names should be unique per context. When there is not context label presented when recording a metric the default will be used which is "Application", this value can be changed through the DefaultContextLabel option.
- **DefaultSamplingType**: Histograms track the statistical distribution of a set of values, they do this by generating values from a resevoir of values using sampling. App Metrics provides different types of sampling, the default sampling type is Exponentially Decaying, this property allows that default value to be changed. The default sampling type is only used if the Histogram being updated does not specify it's own sampling type.
- **GlobalTags**: All metric types can be tagged, for example we could tag all metrics by environment e.g. staging or production so that when we report metrics we know the environment for which they originated. GlobalTags provide access to a `Dictionary<string, string>` which is used to tag all metrics.
- **MetricsEnabled**: Allows recording of all metrics to be disabled.
- **ReportingEnabled**: Allows all configured reporters to be disabled.

## Changing options using `Action<AppMetricOptions>`

[!code-csharp[Main](../../src/samples/StartupWithAppMetricsOptions.cs?highlight=5)]  

## Changing options using `Microsoft.Extensions.Configuration.IConfiguration`

Below is an example `Startup.cs` using `appsettings.json` as a configuration source:

[!code-csharp[Main](../../src/samples/StartupWithAppMetricsOptionsIConfiguration.cs?highlight=17)]     

And the `appsettings.json` file

[!code-json[Main](../../src/samples/AppMetricsOptions.json)]    

## Next Steps

- [AspNet Core Middleware Configuration Options](middleware-configuration.md)
- [Sample Applications](../../samples/index.md)
