using System.Text.RegularExpressions;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.OptionsModel;

namespace AspNet.Metrics.Internal
{
    public class MetricsCoreOptionsSetup : ConfigureOptions<MetricsOptions>
    {
        public MetricsCoreOptionsSetup() : base(ConfigureMetrics)
        {
        }

        public static void ConfigureMetrics(MetricsOptions options)
        {
            options.HealthEndpoint = new PathString("/health");
            options.MetricsEndpoint = new PathString("/json");
            options.MetricsVisualizationEndpoint = new PathString("/metrics-visual");
            options.MetricsTextEndpoint = new PathString("/metrics-text");
            options.PingEndpoint = new PathString("/ping");

            options.HealthEnabled = true;
            options.MetricsEnabled = true;
            options.MetricsTextEnabled = true;
            options.MetricsVisualisationEnabled = true;
            options.PingEnabled = true;

            options.IgnoredRequestPatterns.Add(new Regex("(?i)^swagger"));
            options.IgnoredRequestPatterns.Add(new Regex("(?i)^metrics"));
            options.IgnoredRequestPatterns.Add(new Regex("(?i)^metrics-text"));
            options.IgnoredRequestPatterns.Add(new Regex("(?i)^metrics-display"));
            options.IgnoredRequestPatterns.Add(new Regex("(?i)^health"));
            options.IgnoredRequestPatterns.Add(new Regex("(?i)^ping"));
            options.IgnoredRequestPatterns.Add(new Regex("(?i)^favicon.ico"));
        }
    }
}