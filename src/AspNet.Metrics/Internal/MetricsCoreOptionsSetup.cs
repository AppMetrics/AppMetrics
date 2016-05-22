using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Internal
{
    public class MetricsCoreOptionsSetup : ConfigureOptions<MetricsOptions>
    {
        public MetricsCoreOptionsSetup() : base(ConfigureMetrics)
        {
        }

        public static void ConfigureMetrics(MetricsOptions options)
        {
        }
    }
}