using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Internal
{
    public class AspNetMetricsCoreOptionsSetup : ConfigureOptions<AspNetMetricsOptions>
    {
        public AspNetMetricsCoreOptionsSetup() : base(ConfigureMetrics)
        {
        }

        public static void ConfigureMetrics(AspNetMetricsOptions options)
        {
        }
    }
}