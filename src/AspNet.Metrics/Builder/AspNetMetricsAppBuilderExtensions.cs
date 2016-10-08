using System;
using App.Metrics;
using App.Metrics.Internal;
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
            return app.UseMetrics(new AspNetMetricsOptions());
        }

        public static IApplicationBuilder UseMetrics(this IApplicationBuilder app, AspNetMetricsOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Verify if AddMetrics was done before calling UseMetrics
            // We use the MetricsMarkerService to make sure if all the services were added.
            MetricsServicesHelper.ThrowIfMetricsNotRegistered(app.ApplicationServices);

            var appMetricsOptions = app.ApplicationServices.GetRequiredService<IOptions<AppMetricsOptions>>().Value;

            app.UseMiddleware<PingEndpointMiddleware>(Options.Create(options));


            if (!appMetricsOptions.DisableHealthChecks)
            {
                app.UseMiddleware<HealthCheckEndpointMiddleware>(Options.Create(options));
            }

            if (!appMetricsOptions.DisableMetrics)
            {
                app.UseMiddleware<MetricsEndpointTextEndpointMiddleware>(Options.Create(options));
                app.UseMiddleware<MetricsEndpointMiddleware>(Options.Create(options));
                app.UseMiddleware<ActiveRequestCounterEndpointMiddleware>(Options.Create(options));
                app.UseMiddleware<ErrorRequestMeterMiddleware>(Options.Create(options));
                app.UseMiddleware<OAuth2ClientWebRequestMeterMiddleware>(Options.Create(options));
                app.UseMiddleware<PerRequestTimerMiddleware>(Options.Create(options));
                app.UseMiddleware<PostAndPutRequestSizeHistogramMiddleware>(Options.Create(options));
                app.UseMiddleware<RequestTimerMiddleware>(Options.Create(options));
            }            
           

            return app;
        }
    }
}