public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddMetrics()
			.AddJsonSerialization()
			.AddHealthChecks()
			.AddMetricsMiddleware();
	}

	public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
	{            
		app.UseMetrics();
	}
}