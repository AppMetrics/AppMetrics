using System;
using App.Metrics;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    internal class MetricsBuilder : IMetricsBuilder
    {
        public MetricsBuilder(IServiceCollection services, IMetricsEnvironment environment)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (environment == null) throw new ArgumentNullException(nameof(environment));

            Services = services;
            Environment = environment;
        }

        public IServiceCollection Services { get; }

        public IMetricsEnvironment Environment { get; }
    }
}