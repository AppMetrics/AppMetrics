using System;

namespace App.Metrics.Internal
{
    internal static class HealthServicesHelper
    {
        /// <summary>
        /// Throws InvalidOperationException when MetricsMarkerService is not present
        /// in the list of services.
        /// </summary>
        /// <param name="services">The list of services.</param>
        public static void ThrowIfMetricsNotRegistered(IServiceProvider services)
        {
            if (services.GetService(typeof(HealthCheckMarkerService)) == null)
            {
                throw new InvalidOperationException(
                    "IServiceCollection.AddMetrics().AddHealthChecks()\n" +
                    "IApplicationBuilder.ConfigureServices(...)\n" +
                    "IApplicationBuilder.UseMetrics(...)\n");
            }
        }
    }
}