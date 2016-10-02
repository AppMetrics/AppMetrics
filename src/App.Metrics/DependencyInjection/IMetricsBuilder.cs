// ReSharper disable CheckNamespace

using App.Metrics;

namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public interface IMetricsBuilder
    {
        IServiceCollection Services { get; }

        IMetricsEnvironment Environment { get; }
    }
}