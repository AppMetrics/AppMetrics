using System;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.Extensions.NServiceBus;
using App.Metrics.Formatters.InfluxDB;
using App.Metrics.Infrastructure;
using App.Metrics.Internal.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

namespace EndpointOne
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            Console.Title = nameof(EndpointOne);

            var host = Host
                .CreateDefaultBuilder(args)
                .ConfigureMetricsWithDefaults(builder =>
                {
                    builder.OutputMetrics.AsInfluxDbLineProtocol();
                    builder.Report.ToInfluxDb("http://127.0.0.1:8086", "nservicebus", TimeSpan.FromSeconds(1));
                    builder.Report.ToConsole(options =>
                    {
                        options.FlushInterval = TimeSpan.FromSeconds(5);
                        options.MetricsOutputFormatter = new MetricsInfluxDbLineProtocolOutputFormatter();
                    });
                })
                .UseNServiceBus(context =>
                {
                    var cfg = new EndpointConfiguration(nameof(EndpointOne));
                    cfg.UsePersistence<LearningPersistence>();
                    cfg.UseTransport<LearningTransport>();
                    cfg.EnableFeature<AppMetricsFeature>();
                    
                    var pipeline = cfg.Pipeline;
                    pipeline.StripAssemblyVersionFromEnclosedMessageTypePipeline();

                    return cfg;
                })
                .UseConsoleLifetime()
                .Build();

            await host.StartAsync();

            var messageSession = host.Services.GetRequiredService<IMessageSession>();

            var simulator = new LoadSimulator(messageSession, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            simulator.Start();

            try
            {
                Console.WriteLine("Endpoint started. Press 'enter' to send a message");
                Console.WriteLine("Press ESC key to quit");

                while (true)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape) break;

                    await messageSession.SendLocal(new SomeCommandOne()).ConfigureAwait(false);
                }
            }
            finally
            {
                await simulator.Stop().ConfigureAwait(false);
                await host.StopAsync();
            }
        }
    }
}