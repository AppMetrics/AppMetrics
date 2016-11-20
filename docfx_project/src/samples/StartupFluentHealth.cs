public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMetrics()
            .AddJsonSerialization()
            .AddHealthChecks(factory =>
            {
                factory.Register("DatabaseConnected", 
                    () => Task.FromResult("Database Connection OK"));                    
            })
            .AddMetricsMiddleware();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {            
        app.UseMetrics();
    }
}