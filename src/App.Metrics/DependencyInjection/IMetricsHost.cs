using System.Threading;
using App.Metrics;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public interface IMetricsHost
    {
        IServiceCollection Services { get; }

        IMetricsEnvironment Environment { get; }

        void RunReports(CancellationToken token);
    }
}