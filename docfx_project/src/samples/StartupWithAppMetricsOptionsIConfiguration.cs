public class Startup
{
    public Startup(IHostingEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
        Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMetrics(Configuration.GetSection("AppMetrics"))
            .AddJsonSerialization()
            .AddHealthChecks()
            .AddMetricsMiddleware();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {	        
        app.UseMetrics();
    }
}