using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Health;
using App.Metrics.Utils;
using HealthCheck.Samples;
using Metrics.Samples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Serilog;

namespace App.Sample
{
    public class Host
    {
        public static void Main()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ConfigureMetrics(serviceCollection);

            var provider = serviceCollection.BuildServiceProvider();

            var application = new Application(provider);
            application.RunReports();

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

                application.MetricsContext.Gauge(AppMetricsRegistry.Gauges.Errors, () => 1);
                application.MetricsContext.Gauge(AppMetricsRegistry.Gauges.PercentGauge, () => 1);
                application.MetricsContext.Gauge(AppMetricsRegistry.Gauges.ApmGauge, () => 1);
                application.MetricsContext.Gauge(AppMetricsRegistry.Gauges.ParenthesisGauge, () => 1);
                application.MetricsContext.Gauge(AppMetricsRegistry.Gauges.GaugeWithNoValue, () => double.NaN);

                Console.WriteLine("done setting things up");
                Console.ReadKey();
            }
        }

        private static void ConfigureMetrics(IServiceCollection services)
        {
            services.AddMetrics(options =>
            {
                options.Reporters = reports =>
                {
                    reports.WithConsoleReport(TimeSpan.FromSeconds(3));
                    reports.WithTextFileReport(@"C:\metrics-sample.txt", TimeSpan.FromSeconds(3));
                };
                options.HealthCheckRegistry = checks =>
                {
                    checks.Register("DatabaseConnected", () => Task.FromResult("Database Connection OK"));
                    checks.Register("DiskSpace", () =>
                    {
                        var freeDiskSpace = GetFreeDiskSpace();

                        return Task.FromResult(freeDiskSpace <= 512
                            ? HealthCheckResult.Unhealthy("Not enough disk space: {0}", freeDiskSpace)
                            : HealthCheckResult.Unhealthy("Disk space ok: {0}", freeDiskSpace));
                    });
                };
            });
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var env = PlatformServices.Default.Application;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.RollingFile(Path.Combine($@"C:\logs\{env.ApplicationName}", "log-{Date}.txt"))
                .CreateLogger();

            services.AddSingleton<ILoggerFactory>(provider =>
            {
                var logFactory = new LoggerFactory();
                logFactory.AddConsole();
                logFactory.AddSerilog(Log.Logger);
                return logFactory;
            });

            services.AddTransient<IDatabase, Database>();
        }

        private static int GetFreeDiskSpace()
        {
            return 1024;
        }
    }

    public class Application
    {
        public Application(IServiceProvider provider)
        {
            MetricsContext = provider.GetRequiredService<IMetricsContext>();

            Token = new CancellationToken();
        }

        public IMetricsContext MetricsContext { get; set; }

        public CancellationToken Token { get; set; }

        public void RunReports()
        {
            MetricsContext.Advanced.ReportManager.RunReports(Token);
        }
    }
}