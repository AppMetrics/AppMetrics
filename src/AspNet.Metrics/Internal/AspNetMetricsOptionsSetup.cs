using System;
using Microsoft.Extensions.Options;

namespace AspNet.Metrics.Internal
{
    public class AspNetMetricsOptionsSetup : ConfigureOptions<AspNetMetricsOptions>
    {
        //TODO: AH - remove this or add setup config here?
        public AspNetMetricsOptionsSetup(IServiceProvider serviceProvider)
            : base(options => ConfigureMetrics(options, serviceProvider))
        {
        }

        public static void ConfigureMetrics(AspNetMetricsOptions options, 
            IServiceProvider serviceProvider)
        {
        }
    }
}