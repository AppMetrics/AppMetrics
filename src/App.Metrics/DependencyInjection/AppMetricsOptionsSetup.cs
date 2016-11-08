using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.DependencyInjection
{
    public class AppMetricsOptionsSetup : ConfigureOptions<AppMetricsOptions>
    {
        public AppMetricsOptionsSetup(IServiceProvider serviceProvider)
            : base(options => ConfigureMetrics(options, serviceProvider))
        {
        }

        public static void ConfigureMetrics(AppMetricsOptions options, IServiceProvider serviceProvider)
        {
           
        }
    }
}