public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMetrics(options => {
                options.DefaultContextLabel = "MyContext",
                options.DefaultSamplingType = SamplingType.SlidingWindow;
                options.GlobalTags.Add("env", "stage");
                options.MetricsEnabled = true;
                options.ReportingEnabled = true;     	
            })
            .AddJsonSerialization()
            .AddHealthChecks()
            .AddMetricsMiddleware();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {	        
        app.UseMetrics();
    }
}