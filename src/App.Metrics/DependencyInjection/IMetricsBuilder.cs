// ReSharper disable CheckNamespace

using System.Threading;
using App.Metrics;

namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public interface IMetricsBuilder
    {
        IServiceCollection Services { get; }

        IMetricsEnvironment Environment { get; }

        void RunReports(CancellationToken token);
    }
}