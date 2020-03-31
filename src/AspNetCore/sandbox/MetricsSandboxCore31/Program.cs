using App.Metrics;
using App.Metrics.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MetricsSandboxCore31
{
    public static class Program
    {
        public static IMetricsRoot Metrics { get; set; }

        public static void Main(string[] args)
        {
            Metrics = AppMetrics.CreateDefaultBuilder()
                .OutputMetrics.AsJson()
                .OutputMetrics.AsPlainText()
                .Build();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureMetrics(Metrics)
                .UseMetrics()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
