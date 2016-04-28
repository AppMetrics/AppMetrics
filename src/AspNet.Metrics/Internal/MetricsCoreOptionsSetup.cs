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
            
        }
    }
}