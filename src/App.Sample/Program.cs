using System;
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

            using (var scheduler = new ActionScheduler())
            {
                SampleMetrics.RunSomeRequests();

                scheduler.Start(TimeSpan.FromMilliseconds(500), () =>
                {
                    SetCounterSample.RunSomeRequests();
                    SetMeterSample.RunSomeRequests();
                    UserValueHistogramSample.RunSomeRequests();
                    UserValueTimerSample.RunSomeRequests();
                    SampleMetrics.RunSomeRequests();
                });

                Metric.Gauge("Errors", () => 1, Unit.None);
                Metric.Gauge("% Percent/Gauge|test", () => 1, Unit.None);
                Metric.Gauge("& AmpGauge", () => 1, Unit.None);
                Metric.Gauge("()[]{} ParantesisGauge", () => 1, Unit.None);
                Metric.Gauge("Gauge With No Value", () => double.NaN, Unit.None);

                HealthChecksSample.RegisterHealthChecks();

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
            Logger.LogInformation("Application created successfully.");
        }

        public ILogger Logger { get; set; }

        public IServiceProvider Services { get; set; }


        private void ConfigureServices(IServiceCollection services)
        {
            services.AddMetrics()
                .AddReporter(reports => reports.WithConsoleReport(TimeSpan.FromSeconds(2)));
        }
    }
}