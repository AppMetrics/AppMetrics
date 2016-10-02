using System;
using System.Linq;
using App.Metrics;
using App.Metrics.Core;
using App.Metrics.Internal;
using App.Metrics.Utils;
using AspNet.Metrics;
using AspNet.Metrics.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable CheckNamespace
namespace Microsoft.AspNet.Builder
// ReSharper restore CheckNamespace
{
    public static class AspNetMetricsAppBuilderExtensions
    {
        public static IApplicationBuilder UseMetrics(this IApplicationBuilder app)
        {
            return app.UseMetrics(Metric.Config);
        }

        public static IApplicationBuilder UseMetrics(this IApplicationBuilder app, MetricsConfig config)
        {
            return app.UseMetrics(config, Clock.Default);
        }

        public static IApplicationBuilder UseMetrics(this IApplicationBuilder app, MetricsConfig config, Clock clock)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Verify if AddMetrics was done before calling UseMetrics
            // We use the MetricsMarkerService to make sure if all the services were added.
            MetricsServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);

            var options = app.ApplicationServices.GetService<IOptions<AspNetMetricsOptions>>().Value;

            config.WithConfigExtension((ctx, hs) =>
            {
                var metricsContext = new AspNetMetricsContext(ctx, hs, clock);

                // Metrics Endpoint Middleware
                app.Use(next => new MetricsEndpointMiddleware(next, options, metricsContext).Invoke);
                app.Use(next => new PingEndpointMiddleware(next, options).Invoke);
                app.Use(next => new HealthCheckEndpointMiddleware(next, options, metricsContext).Invoke);
                app.Use(next => new MetricsEndpointTextEndpointMiddleware(next, options, metricsContext).Invoke);

                // Web Metrics Middleware
                app.Use(next => new ErrorRequestMeterMiddleware(next, options, metricsContext).Invoke);
                app.Use(next => new OAuth2ClientWebRequestMeterMiddleware(next, options, metricsContext).Invoke);
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
            var healthChecks = app.ApplicationServices.GetServices<HealthCheck>();

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