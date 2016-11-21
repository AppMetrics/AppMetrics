public void ConfigureServices(IServiceCollection services)
{
    services
        .AddLogging()
        .AddRouting(options => { options.LowercaseUrls = true; });

    services.AddMvc();

    var filter = new DefaultMetricsFilter().WhereType(MetricType.Counter);

    services
        .AddMetrics()
        .AddGlobalFilter(filter)
        .AddJsonSerialization()
        .AddHealthChecks()
        .AddMetricsMiddleware();
}