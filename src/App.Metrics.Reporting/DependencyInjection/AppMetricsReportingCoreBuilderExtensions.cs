using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.Reporting.DependencyInjection
{
    public static class AppMetricsReportingCoreBuilderExtensions
    {
        public static IMetricsHost AddReporting(this IMetricsHost host)
        {
            host.AddReporting(setupAction: null);
            return host;
        }

        public static IMetricsHost AddReporting(
            this IMetricsHost host,
            Action<AppMetricsReportingOptions> setupAction)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));

            host.AddMetricsReportingCore(setupAction);

            return host;
        }
    }
}