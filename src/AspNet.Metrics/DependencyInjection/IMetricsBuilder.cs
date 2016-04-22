
// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
// ReSharper restore CheckNamespace
{
    public interface IMetricsBuilder
    {
        IServiceCollection Services { get; }
    }
}