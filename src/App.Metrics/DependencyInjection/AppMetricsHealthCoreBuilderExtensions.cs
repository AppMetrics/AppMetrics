using System;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.DependencyInjection
{
    public static class AppMetricsHealthCoreBuilderExtensions
    {
        public static IMetricsHost AddHealthChecks(this IMetricsHost host)
        {
            host.AddHealthChecks(setupAction: null);
            return host;
        }

        public static IMetricsHost AddHealthChecks(
            this IMetricsHost host,
            Action<AppMetricsHealthCheckOptions> setupAction)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));

            host.AddMetricsHealthCheckCore(setupAction);

            return host;
        }
    }
}