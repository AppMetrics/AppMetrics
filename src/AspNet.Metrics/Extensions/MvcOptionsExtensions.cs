using AspNet.Metrics.Infrastructure;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNetCore.Mvc
// ReSharper restore CheckNamespace
{
    public static class MvcOptionsExtensions
    {
        public static MvcOptions AddMetricsResourceFilter(this MvcOptions options)
        {
            options.Filters.Add(new MetricsResourceFilter(new DefaultRouteTemplateResolver()));

            return options;
        }
    }
}