using System;
using System.Linq;
using AspNet.Metrics;
using AspNet.Metrics.Internal;
using AspNet.Metrics.Middleware;
using Metrics;
using Metrics.Core;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNet.Builder
// ReSharper restore CheckNamespace
{
    public static class MetricsAppBuilderExtensions
    {
        public static IApplicationBuilder UseMetrics(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Verify if AddMetrics was done before calling UseMetrics
            // We use the MetricsMarkerService to make sure if all the services were added.
            MetricsServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);

            var options = app.ApplicationServices.GetService<IOptions<MetricsOptions>>().Value;

            Metric.Config.WithConfigExtension((ctx, hs) =>
            {
                var metricsContext = new AspNetMetricsContext(ctx, hs);

                // Metrics Endpoint Middleware
                app.Use(next => new MetricsEndpointMiddleware(next, options, metricsContext).Invoke);
                app.Use(next => new PingEndpointEndpointMiddleware(next, options).Invoke);
                app.Use(next => new HealthEndpointEndpointMiddleware(next, options, metricsContext).Invoke);
                app.Use(next => new MetricsEndpointTextEndpointMiddleware(next, options, metricsContext).Invoke);
                app.Use(next => new MetricsEndpointVisualizationEndpointMiddleware(next, options).Invoke);

                // Web Metrics Middldware
                app.Use(next => new ErrorMeterMiddleware(next, options, metricsContext).Invoke);
                app.Use(next => new PerRequestTimerMiddleware(next, options, metricsContext).Invoke);
                app.Use(next => new RequestTimerMiddleware(next, options, metricsContext).Invoke);
                app.Use(next => new ActiveRequestCounterEndpointMiddleware(next, options, metricsContext).Invoke);
                app.Use(next => new PostAndPutRequestSizeHistogramMiddleware(next, options, metricsContext).Invoke);
            });

            UseHealthChecks(app);

            return app;
        }

        private static void UseHealthChecks(IApplicationBuilder app)
        {
            var healthChecks = app.ApplicationServices.GetService<HealthCheck[]>();

            if (healthChecks == null || !healthChecks.Any())
            {
                return;
            }

            foreach (var check in healthChecks)
            {
                HealthChecks.RegisterHealthCheck(check);
            }
        }
    }
}