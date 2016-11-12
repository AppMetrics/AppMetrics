using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.DependencyInjection;
using App.Metrics.Extensions.Reporting.Console;
using App.Metrics.Extensions.Reporting.TextFile;
using App.Metrics.Internal;
using App.Metrics.Reporting;
using App.Metrics.Scheduling;
using HealthCheck.Samples;
using Metrics.Samples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

            var process = Process.GetCurrentProcess();

            var provider = serviceCollection.BuildServiceProvider();

            var application = new Application(provider);
            var scheduler = new DefaultTaskScheduler();

            var simpleMetrics = new SampleMetrics(application.Metrics);
            var setCounterSample = new SetCounterSample(application.Metrics);
            var setMeterSample = new SetMeterSample(application.Metrics);
            var userValueHistogramSample = new UserValueHistogramSample(application.Metrics);
            var userValueTimerSample = new UserValueTimerSample(application.Metrics);

            var cancellationTokenSource = new CancellationTokenSource();
            var task = scheduler.Interval(
                TimeSpan.FromMilliseconds(300), () =>
                {
                    setCounterSample.RunSomeRequests();
                    setMeterSample.RunSomeRequests();
                    userValueHistogramSample.RunSomeRequests();
                    //userValueTimerSample.RunSomeRequests(); //TODO: AH - why's this taking so long?
                    simpleMetrics.RunSomeRequests();

                    application.Metrics.Gauge(AppMetricsRegistry.Gauges.Errors, () => 1);
                    application.Metrics.Gauge(AppMetricsRegistry.Gauges.PercentGauge, () => 1);
                    application.Metrics.Gauge(AppMetricsRegistry.Gauges.ApmGauge, () => 1);
                    application.Metrics.Gauge(AppMetricsRegistry.Gauges.ParenthesisGauge, () => 1);
                    application.Metrics.Gauge(AppMetricsRegistry.Gauges.GaugeWithNoValue, () => double.NaN);

                    application.Metrics.Gauge(AppMetricsRegistry.ProcessMetrics.ProcessPagedMemorySizeGauge, () => process.PagedMemorySize64);
                    application.Metrics.Gauge(AppMetricsRegistry.ProcessMetrics.ProcessPeekPagedMemorySizeGauge, () => process.PeakPagedMemorySize64);
                    application.Metrics.Gauge(AppMetricsRegistry.ProcessMetrics.ProcessPeekVirtualMemorySizeGauge, () => process.PeakVirtualMemorySize64);
                    application.Metrics.Gauge(AppMetricsRegistry.ProcessMetrics.ProcessPeekWorkingSetSizeGauge, () => process.WorkingSet64);
                    application.Metrics.Gauge(AppMetricsRegistry.ProcessMetrics.ProcessPrivateMemorySizeGauge, () => process.PrivateMemorySize64);
                    application.Metrics.Gauge(AppMetricsRegistry.ProcessMetrics.ProcessVirtualMemorySizeGauge, () => process.VirtualMemorySize64);
                    application.Metrics.Gauge(AppMetricsRegistry.ProcessMetrics.SystemNonPagedMemoryGauge, () => process.NonpagedSystemMemorySize64);
                    application.Metrics.Gauge(AppMetricsRegistry.ProcessMetrics.SystemPagedMemorySizeGauge, () => process.PagedSystemMemorySize64);

                }, cancellationTokenSource.Token);

            application.Reporter.RunReports(application.Metrics, cancellationTokenSource.Token);

            Console.WriteLine("Setup Complete, waiting for report run...");

            Console.ReadKey();
        }

        private static void ConfigureMetrics(IServiceCollection services)
        {
            services
                .AddMetrics(options =>
                {                    
                })
                .AddHealthChecks(factory =>
                {
                    factory.Register("DatabaseConnected", () => Task.FromResult("Database Connection OK"));
                    factory.Register("DiskSpace", () =>
                    {
                        var freeDiskSpace = GetFreeDiskSpace();

                        return Task.FromResult(freeDiskSpace <= 512
                            ? HealthCheckResult.Unhealthy("Not enough disk space: {0}", freeDiskSpace)
                            : HealthCheckResult.Unhealthy("Disk space ok: {0}", freeDiskSpace));
                    });
                })
                .AddReporting((options, factory) =>
                {
                    var globalTags = new MetricTags().With("env", "stage");

                    options.IsEnabled = false;

                    var filter = new DefaultMetricsFilter()
                        .WhereType(MetricType.Counter)
                        .WhereMetricTaggedWith("filter-tag1", "filter-tag2")
                        .WithHealthChecks(true)
                        .WithEnvironmentInfo(true);

                    factory.AddConsole(new ConsoleReporterSettings
                    {
                        ReportInterval = TimeSpan.FromSeconds(10),
                        GlobalTags = globalTags
                    }, filter);

                    factory.AddTextFile(new TextFileReporterSettings
                    {
                        ReportInterval = TimeSpan.FromSeconds(30),
                        FileName = @"C:\metrics\sample.txt"
                    });
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
                logFactory.AddConsole((l, s) => s == LogLevel.Trace);
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
            Metrics = provider.GetRequiredService<IMetrics>();

            var reporterFactory = provider.GetRequiredService<IReportFactory>();
            Reporter = reporterFactory.CreateReporter();

            Token = new CancellationToken();
        }

        public IMetrics Metrics { get; set; }

        public IReporter Reporter { get; set; }

        public CancellationToken Token { get; set; }
    }
}