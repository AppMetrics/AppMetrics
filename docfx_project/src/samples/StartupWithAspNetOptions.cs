public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMetrics()
            .AddJsonSerialization()
            .AddHealthChecks()
            .AddMetricsMiddleware(options => {
                options.MetricsTextEndpointEnabled = true;
                options.HealthEndpointEnabled = true;
                options.MetricsEndpointEnabled = true;
                options.PingEndpointEnabled = true;
                options.OAuth2TrackingEnabled = true;

                options.HealthEndpoint = "/app-health";
                options.MetricsEndpoint = "/app-metrics";
                options.MetricsTextEndpoint = "/app-metrics-text";
                options.PingEndpoint = "/api-up";

                options.IgnoredRoutesRegexPatterns = new[] {"(?i)^api/test/ignore"};
            });
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {	        
        app.UseMetrics();
    }
}