public class Host
{ 
    public static void Main()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ConfigureMetrics(serviceCollection);
        
        var provider = serviceCollection.BuildServiceProvider();
        provider.GetRequiredService<IMetrics>();

        // Use this to start recording metrics
        var metrics = provider.GetRequiredService<IMetrics>();   			        
                    
        var reporterFactory = provider.GetRequiredService<IReportFactory>();
        var reporter = reporterFactory.CreateReporter();
        // Will continue to run for the confgured report internal
        reporter.RunReportsAsync(metrics, cancellationTokenSource.Token);           

        Console.ReadKey();
    }

    private static void ConfigureMetrics(IServiceCollection services)
    {
        services
            .AddMetrics()
            .AddHealthChecks()
            .AddReporting(factory =>
            {                    
                factory.AddConsole(new ConsoleReporterSettings
                {
                    ReportInterval = TimeSpan.FromSeconds(10),                        
                });
            });
    }

    private static void ConfigureServices(IServiceCollection services)
    {                      
        var loggerFactory = new LoggerFactory();
        loggerFactory.AddConsole((l, s) => s == LogLevel.Trace);            
        services.AddSingleton<ILoggerFactory, LoggerFactory>();
        services.AddLogging();            
    }       
} 