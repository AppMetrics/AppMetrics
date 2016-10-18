using System;

namespace AspNet.Metrics.Internal
{
    internal static class AspNetMetricsServicesHelper
    {
        /// <summary>
        /// Throws InvalidOperationException when MetricsMarkerService is not present
        /// in the list of services.
        /// </summary>
        /// <param name="services">The list of services.</param>
        public static void ThrowIfMetricsNotRegistered(IServiceProvider services)
        {
            if (services.GetService(typeof(AspNetMetricsMarkerService)) == null)
            {
                throw new InvalidOperationException(
                    "IServiceCollection.AddAspNetMetrics()\n");
            }
        }
    }
}