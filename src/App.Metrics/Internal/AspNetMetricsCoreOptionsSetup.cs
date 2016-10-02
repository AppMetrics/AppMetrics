using Microsoft.Extensions.Options;

namespace App.Metrics.Internal
{
    public class AppMetricsCoreOptionsSetup : ConfigureOptions<AppMetricsOptions>
    {
        public AppMetricsCoreOptionsSetup() : base(ConfigureMetrics)
        {
        }

        public static void ConfigureMetrics(AppMetricsOptions options)
        {
        }
    }
}