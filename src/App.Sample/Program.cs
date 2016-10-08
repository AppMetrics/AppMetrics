using System;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Utils;
using Metrics.Samples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace App.Sample
{
    public class Host
    {
        public static void Main()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var application = new Application(serviceCollection);

            var simpleMetrics = new SampleMetrics(application.MetricsContext);
            var setCounterSample = new SetCounterSample(application.MetricsContext);
            var setMeterSample = new SetMeterSample(application.MetricsContext);
            var userValueHistogramSample = new UserValueHistogramSample(application.MetricsContext);
            var userValueTimerSample = new UserValueTimerSample(application.MetricsContext);

            using (var scheduler = new ActionScheduler())
            {
                simpleMetrics.RunSomeRequests();

                scheduler.Start(TimeSpan.FromMilliseconds(500), () =>
                {
                    setCounterSample.RunSomeRequests();
                    setMeterSample.RunSomeRequests();
                    userValueHistogramSample.RunSomeRequests();
                    userValueTimerSample.RunSomeRequests();
                    simpleMetrics.RunSomeRequests();
                });

                application.MetricsContext.Gauge("Errors", () => 1, Unit.None);
                application.MetricsContext.Gauge("% Percent/Gauge|test", () => 1, Unit.None);
                application.MetricsContext.Gauge("& AmpGauge", () => 1, Unit.None);
                application.MetricsContext.Gauge("()[]{} ParantesisGauge", () => 1, Unit.None);
                application.MetricsContext.Gauge("Gauge With No Value", () => double.NaN, Unit.None);

                Console.WriteLine("done setting things up");
                Console.ReadKey();
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ILoggerFactory>(provider =>
            {
                var logFactory = new LoggerFactory();
                logFactory.AddConsole();
                return logFactory;
            });
        }
    }

    public class Application
    {
        public Application(IServiceCollection services)
        {
            ConfigureServices(services);
            Services = services.BuildServiceProvider();
            Logger = Services.GetRequiredService<ILoggerFactory>().CreateLogger<Application>();
            MetricsContext = Services.GetRequiredService<IMetricsContext>();
            Logger.LogInformation("Application created successfully.");
        }

        public ILogger Logger { get; set; }

        public IMetricsContext MetricsContext { get; set; }

        public IServiceProvider Services { get; set; }

        private static int GetFreeDiskSpace()
        {
            return 1024;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddMetrics(options =>
            {
                options.Reporters = reports => { reports.WithConsoleReport(TimeSpan.FromSeconds(3)); };
                options.HealthChecks = checks =>
                {
                    checks.RegisterHealthCheck("DatabaseConnected", () => Task.FromResult("Database Connection OK"));
                    checks.RegisterHealthCheck("DiskSpace", () =>
                    {
                        var freeDiskSpace = GetFreeDiskSpace();

                        return Task.FromResult(freeDiskSpace <= 512
                            ? HealthCheckResult.Unhealthy("Not enough disk space: {0}", freeDiskSpace)
                            : HealthCheckResult.Unhealthy("Disk space ok: {0}", freeDiskSpace));
                    });
                    checks.RegisterHealthCheck(new DatabaseHealthCheck(null));
                };
            });
        }
    }
}