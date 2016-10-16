using System;
using System.Threading;
using App.Metrics;
using App.Metrics.Registries;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    internal class MetricsHost : IMetricsHost
    {
        internal MetricsHost(IServiceCollection services, IMetricsEnvironment environment)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (environment == null) throw new ArgumentNullException(nameof(environment));

            Services = services;
            Environment = environment;
        }

        public IMetricsEnvironment Environment { get; }

        public IServiceCollection Services { get; }

        public void RunReports(CancellationToken token)
        {
            var reportRegistry = Services.BuildServiceProvider().GetRequiredService<IMetricReporterRegistry>();
            reportRegistry.Reports.ForEach(r => r.Start(token));
        }
    }
}